Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Diagnostics
Imports System.Globalization
Imports System.Text
Imports MySql.Data.MySqlClient

' The UI may display this message; diagnostic SQL/exception details stay in errLog.
Public Class ReceivePostingException
    Inherits InvalidOperationException
    Public ReadOnly ReferenceId As String
    Public Sub New(ByVal message As String, ByVal reference As String, ByVal inner As Exception)
        MyBase.New(message & " [Ref: " & reference & "]", inner)
        ReferenceId = reference
    End Sub
End Class

' One receipt operation, one connection, one transaction. Never retries writes automatically.
Public NotInheritable Class clsReceivePosting
    Implements IDisposable

    Private ReadOnly connection As MySqlConnection
    Private ReadOnly customer As Integer
    Private document As String
    Private ReadOnly actor As String
    Private ReadOnly action As String
    Private ReadOnly log As Action(Of String, String)
    Private ReadOnly updateCurrentCost As Boolean
    Private ReadOnly trace As New StringBuilder()
    Private ReadOnly timer As Stopwatch = Stopwatch.StartNew()
    Private ReadOnly affected As New SortedDictionary(Of String, DataRow)(StringComparer.Ordinal)
    Private ReadOnly starts As New Dictionary(Of String, Date)()
    Private lockHeld As Boolean
    Private commitAttempted As Boolean
    Private committed As Boolean
    Private finished As Boolean
    Private stage As String = "initialise"
    Private lastSql As String = ""
    Private lastParameters As String = ""
    Public ReadOnly ReferenceId As String = Guid.NewGuid().ToString("N")
    Public Property Transaction As MySqlTransaction

    Public Sub New(ByVal conn As MySqlConnection, ByVal custid As Integer, ByVal receipt As String, ByVal username As String, ByVal operation As String, ByVal logger As Action(Of String, String), Optional ByVal syncCurrentCost As Boolean = False)
        connection = conn
        customer = custid
        document = receipt
        actor = username
        action = operation
        log = logger
        updateCurrentCost = syncCurrentCost
    End Sub

    Private Shared Function Clean(ByVal value As Object) As String
        If value Is Nothing OrElse Convert.IsDBNull(value) Then Return "NULL"
        If TypeOf value Is DateTime Then Return DirectCast(value, DateTime).ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
        Return Convert.ToString(value, CultureInfo.InvariantCulture).Replace(vbCr, " ").Replace(vbLf, " ")
    End Function

    Public Sub Note(ByVal message As String)
        trace.AppendLine(DateTime.UtcNow.ToString("o") & " " & message)
    End Sub

    Private Sub Record(ByVal outcome As String, Optional ByVal details As String = "")
        Dim message As String = String.Format(CultureInfo.InvariantCulture,
            "[RECEIVE_{0}] ref={1}; action={2}; custid={3}; document={4}; user={5}; stage={6}; elapsedMs={7}; committed={8}; commitAttempted={9}; serverThread={10}",
            outcome, ReferenceId, Clean(action), customer, Clean(document), Clean(actor), stage, timer.ElapsedMilliseconds, committed, commitAttempted, If(connection.State = ConnectionState.Open, connection.ServerThread, 0))
        Try
            log(message & Environment.NewLine & details & Environment.NewLine & trace.ToString(), "clsReceivePosting")
        Catch loggerError As Exception
            System.Diagnostics.Trace.TraceError(message & " loggingFailure=" & loggerError.ToString())
        End Try
    End Sub

    Public Function Begin() As MySqlTransaction
        stage = "connect"
        Record("STARTED")
        If customer <= 0 Then Throw New InvalidOperationException("Customer transaksi tidak valid.")
        connection.Open()
        If Not String.IsNullOrWhiteSpace(document) Then
            stage = "document-lock"
            ' Includes the database name: identical document numbers in other databases do not collide.
            Dim name As String = LockName()
            lockHeld = True ' Release our own named lock even if the acquisition response is lost.
            Dim acquired As Object = Scalar("document-lock", "SELECT GET_LOCK(@name, 0)", "@name", name)
            If acquired Is Nothing OrElse Convert.IsDBNull(acquired) OrElse Convert.ToInt32(acquired) <> 1 Then
                Throw New InvalidOperationException("Dokumen sedang diproses oleh permintaan lain. Muat ulang sebelum mencoba kembali.")
            End If
            lockHeld = True
        End If
        Dim engines As DataTable = Read("check-transaction-engines", "SELECT TABLE_NAME,ENGINE FROM information_schema.TABLES WHERE TABLE_SCHEMA=DATABASE() AND TABLE_NAME IN ('cld_inv_trans','cld_invlk_rcv','cld_inv_transstockcls')")
        If engines.Rows.Count <> 3 Then Throw New InvalidOperationException("Tabel inventory belum lengkap.")
        For Each table As DataRow In engines.Rows
            If Not String.Equals(CStr(table("ENGINE")), "InnoDB", StringComparison.OrdinalIgnoreCase) Then Throw New InvalidOperationException("Tabel inventory harus memakai InnoDB agar pembatalan atomik.")
        Next
        If updateCurrentCost Then
            Dim engine As Object = Scalar("check-cost-engine", "SELECT ENGINE FROM information_schema.TABLES WHERE TABLE_SCHEMA=DATABASE() AND TABLE_NAME='cld_inv_stk_cc'")
            If engine Is Nothing OrElse Not String.Equals(Convert.ToString(engine), "InnoDB", StringComparison.OrdinalIgnoreCase) Then Throw New InvalidOperationException("Tabel cost stock harus tersedia dan memakai InnoDB.")
        End If
        stage = "begin-transaction"
        Transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted)
        Return Transaction
    End Function

    Public Sub SetDocument(ByVal receipt As String)
        document = receipt
        lockHeld = True
        Dim acquired As Object = Scalar("new-document-lock", "SELECT GET_LOCK(@name, 0)", "@name", LockName())
        If acquired Is Nothing OrElse Convert.IsDBNull(acquired) OrElse Convert.ToInt32(acquired) <> 1 Then Throw New InvalidOperationException("Nomor dokumen sedang diproses. Muat ulang sebelum mencoba kembali.")
        lockHeld = True
    End Sub

    Public Sub Track(ByVal cmd As MySqlCommand, ByVal stepName As String)
        stage = stepName
        lastSql = cmd.CommandText
        cmd.CommandTimeout = 30
        Dim values As New StringBuilder()
        For Each parameter As MySqlParameter In cmd.Parameters
            Dim name As String = parameter.ParameterName.ToLowerInvariant()
            Dim value As String = If(name = "@notes" OrElse name = "@do" OrElse name = "@stk_name" OrElse name = "@cc_name", "[redacted-text]", Clean(parameter.Value))
            values.Append(parameter.ParameterName).Append("=").Append(value).Append("; ")
        Next
        lastParameters = values.ToString()
        Note("BEGIN stage=" & stage & "; params=" & lastParameters)
    End Sub

    Private Function LockName() As String
        Dim key As String = connection.Database & ":" & customer.ToString(CultureInfo.InvariantCulture) & ":" & document
        Using sha As System.Security.Cryptography.SHA256 = System.Security.Cryptography.SHA256.Create()
            Return "rcv:" & BitConverter.ToString(sha.ComputeHash(Encoding.UTF8.GetBytes(key))).Replace("-", "").Substring(0, 56)
        End Using
    End Function

    Private Function Command(ByVal stepName As String, ByVal sql As String, ByVal values As Object()) As MySqlCommand
        stage = stepName
        lastSql = sql
        Dim cmd As New MySqlCommand(sql, connection, Transaction)
        cmd.CommandTimeout = 30
        Dim parameters As New StringBuilder()
        For i As Integer = 0 To values.Length - 1 Step 2
            Dim name As String = CStr(values(i))
            cmd.Parameters.AddWithValue(name, If(values(i + 1), DBNull.Value))
            parameters.Append(name).Append("=").Append(Clean(values(i + 1))).Append("; ")
        Next
        lastParameters = parameters.ToString()
        Note("BEGIN stage=" & stage & "; params=" & lastParameters)
        Return cmd
    End Function

    Public Function Read(ByVal stepName As String, ByVal sql As String, ParamArray ByVal values As Object()) As DataTable
        Using cmd As MySqlCommand = Command(stepName, sql, values)
            Dim table As New DataTable()
            Using reader As MySqlDataReader = cmd.ExecuteReader()
                table.Load(reader)
            End Using
            Note("END stage=" & stepName & "; rows=" & table.Rows.Count)
            Return table
        End Using
    End Function

    Public Function Write(ByVal stepName As String, ByVal sql As String, ParamArray ByVal values As Object()) As Integer
        Using cmd As MySqlCommand = Command(stepName, sql, values)
            Dim count As Integer = cmd.ExecuteNonQuery()
            Note("END stage=" & stepName & "; affectedRows=" & count)
            Return count
        End Using
    End Function

    Public Function Scalar(ByVal stepName As String, ByVal sql As String, ParamArray ByVal values As Object()) As Object
        Using cmd As MySqlCommand = Command(stepName, sql, values)
            Dim value As Object = cmd.ExecuteScalar()
            Note("END stage=" & stepName & "; result=" & Clean(value))
            Return value
        End Using
    End Function

    Public Function LockHeader() As DataRow
        Dim rows As DataTable = Read("lock-header", "SELECT * FROM cld_invlk_rcv WHERE custid=@cust AND id=@doc AND Tipe=1 FOR UPDATE", "@cust", customer, "@doc", document)
        If rows.Rows.Count <> 1 Then Throw New InvalidOperationException("Dokumen penerimaan tidak ditemukan.")
        Note("header flag=" & Clean(rows.Rows(0)("flag")) & "; date=" & Clean(rows.Rows(0)("InvDate")) & "; cc=" & Clean(rows.Rows(0)("cc")) & "; modifiedBy=" & Clean(rows.Rows(0)("ModifBy")) & "; modifiedAt=" & Clean(rows.Rows(0)("ModifDate")))
        Return rows.Rows(0)
    End Function

    Public Function LockDetails() As DataTable
        Return Read("lock-details", "SELECT * FROM cld_inv_trans WHERE custid=@cust AND LinkID=@doc AND type=3 ORDER BY cc,Stk_Id,InvDate,ID FOR UPDATE", "@cust", customer, "@doc", document)
    End Function

    ' Original posting identity always comes from locked DB rows, never editable date/qty controls.
    Public Sub Affect(ByVal row As DataRow, ByVal postingDate As Date)
        Dim cc As Integer = Convert.ToInt32(row("cc"))
        Dim stock As Integer = Convert.ToInt32(row("stk_id"))
        If cc <= 0 OrElse stock <= 0 Then Throw New InvalidOperationException("Identitas stock/CC tidak valid.")
        Dim key As String = cc.ToString("D10", CultureInfo.InvariantCulture) & ":" & stock.ToString("D10", CultureInfo.InvariantCulture)
        If Not starts.ContainsKey(key) OrElse postingDate.Date < starts(key) Then starts(key) = postingDate.Date
        affected(key) = row
        Note("posting cc=" & cc & "; stock=" & stock & "; date=" & Clean(postingDate.Date) & "; qty=" & Clean(row("ttlStock")) & "; flag=" & Clean(row("flagtrans")))
    End Sub

    ' Acquire summary row/range locks in stable CC/stock/date order BEFORE changing ledger flags.
    Private Shared Function BalanceSnapshot(ByVal row As DataRow) As String
        Dim result As New StringBuilder()
        For Each field As String In New String() {"invDate", "ccid", "stkid", "openQty", "rcvQty", "rtnQty", "tinQty", "toutQty", "rwasteQty", "fwasteQty", "usageQty", "HoOutQty", "HoInQty", "BPIn", "BPout", "cwasteqty", "ttlQty", "bClose", "closeQty", "varianceQty", "OpenCost", "Cost"}
            result.Append(field).Append("=").Append(Clean(row(field))).Append("; ")
        Next
        Return result.ToString()
    End Function

    Public Sub LockBalances()
        For Each pair As KeyValuePair(Of String, DataRow) In affected
            Dim row As DataRow = pair.Value
            Dim rows As DataTable = Read("lock-balances", "SELECT * FROM cld_inv_transstockcls WHERE custid=@cust AND ccid=@cc AND stkid=@stk AND invDate>=@day ORDER BY invDate FOR UPDATE",
                "@cust", customer, "@cc", row("cc"), "@stk", row("stk_id"), "@day", starts(pair.Key).AddDays(-1))
            For Each balance As DataRow In rows.Rows
                Note("before " & BalanceSnapshot(balance))
            Next
            Dim postingDay As Date = starts(pair.Key)
            Dim activeQty As Decimal = Convert.ToDecimal(Scalar("check-receive-before", "SELECT COALESCE(SUM(ttlStock),0) FROM cld_inv_trans WHERE custid=@cust AND cc=@cc AND Stk_Id=@stk AND type=3 AND flagtrans=1 AND InvDate>=@day AND InvDate<@next", "@cust", customer, "@cc", row("cc"), "@stk", row("stk_id"), "@day", postingDay, "@next", postingDay.AddDays(1)))
            For Each balance As DataRow In rows.Rows
                If Convert.ToDateTime(balance("invDate")).Date = postingDay AndAlso Number(balance, "rcvQty") <> activeQty Then
                    Record("BUCKET_MISMATCH", "before-ledger-change; activeReceive=" & Clean(activeQty) & "; " & BalanceSnapshot(balance))
                End If
            Next
        Next
    End Sub

    Public Shared Function Number(ByVal row As DataRow, ByVal field As String) As Decimal
        If Convert.IsDBNull(row(field)) Then Return 0D
        Return Convert.ToDecimal(row(field), CultureInfo.InvariantCulture)
    End Function

    Public Shared Function IsClosed(ByVal row As DataRow) As Boolean
        If Convert.IsDBNull(row("bClose")) Then Return False
        Return Convert.ToBoolean(row("bClose"), CultureInfo.InvariantCulture)
    End Function

    ' Matches the live receive/opname formula; closing-waste accounting is not changed here.
    Public Shared Function Total(ByVal row As DataRow, ByVal opening As Decimal, ByVal receive As Decimal) As Decimal
        Return opening + receive - Number(row, "rtnQty") + Number(row, "tinQty") - Number(row, "toutQty") - Number(row, "rwasteQty") - Number(row, "fwasteQty") - Number(row, "usageQty") - Number(row, "HoOutQty") + Number(row, "HoInQty") + Number(row, "BPIn") - Number(row, "BPout")
    End Function

    Public Sub Reconcile()
        For Each pair As KeyValuePair(Of String, DataRow) In affected
            Dim meta As DataRow = pair.Value
            Dim first As Date = starts(pair.Key)
            Dim cc As Integer = Convert.ToInt32(meta("cc"))
            Dim stock As Integer = Convert.ToInt32(meta("stk_id"))
            Dim rows As DataTable = Read("read-balances", "SELECT * FROM cld_inv_transstockcls WHERE custid=@cust AND ccid=@cc AND stkid=@stk AND invDate>=@day ORDER BY invDate FOR UPDATE",
                "@cust", customer, "@cc", cc, "@stk", stock, "@day", first.AddDays(-1))
            Dim byDate As New Dictionary(Of Date, DataRow)()
            Dim last As Date = If(first > Date.Today, first, Date.Today)
            For Each row As DataRow In rows.Rows
                Dim day As Date = Convert.ToDateTime(row("invDate")).Date
                If byDate.ContainsKey(day) Then Throw New InvalidOperationException("Saldo harian ganda ditemukan.")
                byDate.Add(day, row)
                If day > last Then last = day
            Next
            Dim previous As DataRow = Nothing
            byDate.TryGetValue(first.AddDays(-1), previous)
            Dim dayCursor As Date = first
            Do While dayCursor <= last
                Dim current As DataRow = Nothing
                If Not byDate.TryGetValue(dayCursor, current) Then
                    ' Do not invent other movement buckets if this day is already used by another subsystem.
                    Dim other As Integer = Convert.ToInt32(Scalar("check-missing-balance", "SELECT COUNT(*) FROM cld_inv_trans WHERE custid=@cust AND cc=@cc AND Stk_Id=@stk AND InvDate>=@day AND InvDate<@next AND flagtrans=1 AND type NOT IN (3,5,11,12)",
                        "@cust", customer, "@cc", cc, "@stk", stock, "@day", dayCursor, "@next", dayCursor.AddDays(1)))
                    other += Convert.ToInt32(Scalar("check-missing-usage", "SELECT COUNT(*) FROM cld_inv_transusage WHERE custid=@cust AND cc=@cc AND Stk_Id=@stk AND InvDate>=@day AND InvDate<@next AND flagtrans=1",
                        "@cust", customer, "@cc", cc, "@stk", stock, "@day", dayCursor, "@next", dayCursor.AddDays(1)))
                    If other > 0 Then Throw New InvalidOperationException("Saldo harian belum tersedia tetapi sudah memiliki pergerakan lain. Perlu rekonsiliasi sebelum melanjutkan.")
                    If previous Is Nothing Then
                        Dim earlier As Integer = Convert.ToInt32(Scalar("check-opening-gap", "SELECT COUNT(*) FROM cld_inv_transstockcls WHERE custid=@cust AND ccid=@cc AND stkid=@stk AND invDate<@day", "@cust", customer, "@cc", cc, "@stk", stock, "@day", dayCursor))
                        If earlier > 0 Then Throw New InvalidOperationException("Rantai opening terputus. Perlu rekonsiliasi sebelum melanjutkan.")
                    End If
                    Dim opening As Decimal = If(previous Is Nothing, 0D, If(IsClosed(previous), Number(previous, "closeQty"), Number(previous, "ttlQty")))
                    Dim openingCost As Decimal = If(previous Is Nothing, 0D, If(IsClosed(previous), Number(previous, "closeCost"), Number(previous, "Cost")))
                    Write("create-balance", "INSERT INTO cld_inv_transstockcls (invDate,custid,stkid,stkNo,stkName,ccid,ccName,OpenCost,openQty,Cost,rcvQty,rtnQty,tinQty,toutQty,rwasteQty,fwasteQty,usageQty,ttlQty,bClose,closeQty,closeCost,varianceQty,HoOutQty,HoInQty,BPIn,BPout,cwasteqty) VALUES (@day,@cust,@stk,@no,@name,@cc,@ccname,@cost,@open,@cost,0,0,0,0,0,0,0,@open,0,0,0,0,0,0,0,0,0)",
                        "@day", dayCursor, "@cust", customer, "@stk", stock, "@no", meta("stk_no"), "@name", meta("Stk_Name"), "@cc", cc, "@ccname", meta("cc_name"), "@cost", openingCost, "@open", opening)
                    Dim created As DataTable = Read("read-created-balance", "SELECT * FROM cld_inv_transstockcls WHERE custid=@cust AND ccid=@cc AND stkid=@stk AND invDate=@day FOR UPDATE", "@cust", customer, "@cc", cc, "@stk", stock, "@day", dayCursor)
                    current = created.Rows(0)
                End If
                Dim open As Decimal = Number(current, "openQty")
                Dim openCost As Decimal = Number(current, "OpenCost")
                If previous IsNot Nothing Then
                    open = If(IsClosed(previous), Number(previous, "closeQty"), Number(previous, "ttlQty"))
                    openCost = If(IsClosed(previous), Number(previous, "closeCost"), Number(previous, "Cost"))
                End If
                Dim receive As Decimal = Convert.ToDecimal(Scalar("sum-active-receive", "SELECT COALESCE(SUM(ttlStock),0) FROM cld_inv_trans WHERE custid=@cust AND cc=@cc AND Stk_Id=@stk AND type=3 AND flagtrans=1 AND InvDate>=@day AND InvDate<@next",
                    "@cust", customer, "@cc", cc, "@stk", stock, "@day", dayCursor, "@next", dayCursor.AddDays(1)))
                If Number(current, "rcvQty") <> receive Then Note("receive-rebuild date=" & Clean(dayCursor) & "; stored=" & Clean(current("rcvQty")) & "; active=" & Clean(receive))
                If receive < 0D Then Throw New InvalidOperationException("Ledger penerimaan aktif berjumlah negatif. Perlu inspeksi sebelum melanjutkan.")
                Dim totalQty As Decimal = Total(current, open, receive)
                Dim incoming As DataTable = Read("cost-inputs", "SELECT COALESCE(SUM(ttlStock),0) qty, COALESCE(SUM(ttlStock*Cost),0) amount FROM cld_inv_trans WHERE custid=@cust AND cc=@cc AND Stk_Id=@stk AND type IN (1,3,8,9) AND flagtrans=1 AND ttlStock>0 AND InvDate>=@day AND InvDate<@next",
                    "@cust", customer, "@cc", cc, "@stk", stock, "@day", dayCursor, "@next", dayCursor.AddDays(1))
                Dim cost As Decimal = Number(current, "Cost")
                Dim qty As Decimal = Number(incoming.Rows(0), "qty")
                If qty > 0D AndAlso open + qty > 0D Then
                    cost = (open * openCost + Number(incoming.Rows(0), "amount")) / (open + qty)
                ElseIf qty = 0D AndAlso openCost > 0D Then
                    cost = openCost
                End If
                Dim variance As Decimal = If(IsClosed(current), Number(current, "closeQty") - totalQty, Number(current, "varianceQty"))
                If Number(current, "cwasteqty") <> 0D Then variance = totalQty
                Write("write-balance", "UPDATE cld_inv_transstockcls SET openQty=@open,OpenCost=@opencost,rcvQty=@receive,ttlQty=@total,Cost=@cost,varianceQty=@variance WHERE custid=@cust AND ccid=@cc AND stkid=@stk AND invDate=@day",
                    "@open", open, "@opencost", openCost, "@receive", receive, "@total", totalQty, "@cost", cost, "@variance", variance, "@cust", customer, "@cc", cc, "@stk", stock, "@day", dayCursor)
                Dim verified As DataTable = Read("verify-balance", "SELECT * FROM cld_inv_transstockcls WHERE custid=@cust AND ccid=@cc AND stkid=@stk AND invDate=@day FOR UPDATE", "@cust", customer, "@cc", cc, "@stk", stock, "@day", dayCursor)
                If verified.Rows.Count <> 1 Then Throw New InvalidOperationException("Saldo harian gagal diverifikasi.")
                current = verified.Rows(0)
                If Number(current, "ttlQty") <> Decimal.Round(totalQty, 4, MidpointRounding.AwayFromZero) OrElse Number(current, "rcvQty") <> Decimal.Round(receive, 4, MidpointRounding.AwayFromZero) Then Throw New InvalidOperationException("Hasil quantity tersimpan tidak sesuai perhitungan.")
                Note("after " & BalanceSnapshot(current))
                previous = current
                dayCursor = dayCursor.AddDays(1)
            Loop
            If updateCurrentCost Then
                Dim today As DataTable = Read("current-cost", "SELECT Cost FROM cld_inv_transstockcls WHERE custid=@cust AND ccid=@cc AND stkid=@stk AND invDate=@day", "@cust", customer, "@cc", cc, "@stk", stock, "@day", Date.Today)
                If today.Rows.Count = 1 Then
                    Write("sync-stock-cost", "INSERT INTO cld_inv_stk_cc (stkID,CustID,stkLimit,ccid,cost) VALUES (@stk,@cust,0,@cc,@cost) ON DUPLICATE KEY UPDATE cost=VALUES(cost)", "@stk", stock, "@cust", customer, "@cc", cc, "@cost", Number(today.Rows(0), "Cost"))
                End If
            End If
        Next
    End Sub

    Public Sub Cancel()
        Dim header As DataRow = LockHeader()
        Dim details As DataTable = LockDetails()
        Dim active As Integer = 0
        Dim postedQty As Decimal = 0D
        For Each row As DataRow In details.Rows
            Note("cancel-candidate id=" & Clean(row("ID")) & "; stock=" & Clean(row("Stk_Id")) & "; cc=" & Clean(row("Cc")) & "; date=" & Clean(row("InvDate")) & "; qty=" & Clean(row("ttlStock")) & "; flag=" & Clean(row("flagtrans")) & "; modifiedAt=" & Clean(row("ModifDate")))
            If Convert.ToInt32(row("flagtrans")) = 1 Then
                If Number(row, "ttlStock") < 0D Then Throw New InvalidOperationException("Quantity penerimaan negatif. Periksa dokumen sebelum pembatalan.")
                Affect(row, Convert.ToDateTime(row("InvDate")))
                active += 1
                postedQty += Number(row, "ttlStock")
            End If
        Next
        If Convert.ToInt32(header("flag")) <> 2 OrElse active = 0 Then Throw New InvalidOperationException("Penerimaan sudah dibatalkan atau belum berstatus Receive. Tidak ada quantity yang dikurangi.")
        If postedQty = 0D Then Throw New InvalidOperationException("Quantity penerimaan aktif sudah 0. Tidak ada quantity yang dikurangi.")
        LockBalances()
        Dim count As Integer = Write("cancel-active-details", "UPDATE cld_inv_trans SET flagtrans=0,ModifBy=@user,ModifDate=NOW() WHERE custid=@cust AND LinkID=@doc AND type=3 AND flagtrans=1", "@user", actor, "@cust", customer, "@doc", document)
        If count <> active Then Throw New InvalidOperationException("Status detail berubah saat pembatalan. Tidak ada perubahan yang disimpan.")
        Write("cancel-header", "UPDATE cld_invlk_rcv SET flag=1,ModifBy=@user,ModifDate=NOW() WHERE custid=@cust AND id=@doc AND Tipe=1 AND flag=2", "@user", actor, "@cust", customer, "@doc", document)
        Reconcile()
    End Sub

    Public Sub Commit()
        stage = "commit"
        commitAttempted = True
        Transaction.Commit()
        committed = True
        finished = True
        Record("COMMITTED")
    End Sub

    Public Function Fail(ByVal ex As Exception) As ReceivePostingException
        Dim failedStage As String = stage
        Dim rollback As String = "not-started"
        If Transaction IsNot Nothing AndAlso Not committed Then
            Try
                Transaction.Rollback()
                rollback = "succeeded"
            Catch rollbackError As Exception
                rollback = "FAILED: " & rollbackError.ToString()
            End Try
        End If
        stage = failedStage
        finished = True
        Dim mysql As MySqlException = TryCast(ex, MySqlException)
        Dim details As String = "rollback=" & rollback & "; outcome=" & If(commitAttempted, "UNKNOWN_CHECK_DATABASE", "not-committed") & "; mysqlNumber=" & If(mysql Is Nothing, "n/a", mysql.Number.ToString(CultureInfo.InvariantCulture)) & Environment.NewLine & "SQL=" & lastSql & Environment.NewLine & "Parameters=" & lastParameters & Environment.NewLine & ex.ToString()
        Record(If(TypeOf ex Is InvalidOperationException AndAlso Not commitAttempted, "REJECTED", "FAILED"), details)
        Dim message As String = If(TypeOf ex Is InvalidOperationException, ex.Message, "Proses penerimaan gagal. Tidak dapat menyimpan perubahan; lihat referensi pada errLog.")
        If commitAttempted Then message = "Koneksi gagal saat konfirmasi commit. Status transaksi belum dapat dipastikan. Muat ulang dan periksa dokumen sebelum mengulang."
        Return New ReceivePostingException(message, ReferenceId, ex)
    End Function

    Public Sub Dispose() Implements IDisposable.Dispose
        If Not finished AndAlso Transaction IsNot Nothing Then
            Try
                Transaction.Rollback()
            Catch ex As Exception
                Record("ROLLBACK_FAILED", ex.ToString())
            End Try
        End If
        If Transaction IsNot Nothing Then Transaction.Dispose()
        If lockHeld Then
            Try
                ' Release on the SAME connection before it is returned to the pool.
                Using cmd As New MySqlCommand("SELECT RELEASE_LOCK(@name)", connection)
                    cmd.Parameters.AddWithValue("@name", LockName())
                    cmd.CommandTimeout = 5
                    cmd.ExecuteScalar()
                End Using
            Catch ex As Exception
                Record("LOCK_RELEASE_FAILED", ex.ToString())
                MySqlConnection.ClearPool(connection)
            End Try
        End If
    End Sub
End Class
