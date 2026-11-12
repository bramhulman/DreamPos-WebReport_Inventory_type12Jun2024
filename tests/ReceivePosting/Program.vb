Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.IO
Imports System.Reflection
Imports MySql.Data.MySqlClient
Imports DreamPos_WebReport

Module Program
    Private Const Connection As String = "server=127.0.0.1;port=33179;uid=root;password=;database=codex_receive_posting_test;SslMode=None;Pooling=False"
    Private logs As New List(Of String)()
    Private root As String
    Private origin As Date = Date.Today.AddDays(-3)
    Private destination As Date = Date.Today.AddDays(-8)
    Private count As Integer

    Sub Main(ByVal args As String())
        root = Path.GetFullPath(args(0))
        Using conn As New MySqlConnection(Connection.Replace("database=codex_receive_posting_test;", ""))
            conn.Open()
            Using cmd As New MySqlCommand("SELECT @@datadir", conn)
                Dim datadir As String = Path.GetFullPath(CStr(cmd.ExecuteScalar()))
                If Not datadir.StartsWith(Path.Combine(root, "runtime", "mysql-data"), StringComparison.OrdinalIgnoreCase) Then Throw New Exception("Refusing to test on an existing server datadir")
            End Using
            Using cmd As New MySqlCommand("CREATE DATABASE IF NOT EXISTS codex_receive_posting_test", conn)
                cmd.ExecuteNonQuery()
            End Using
        End Using
        ConfigureSettings()
        For Each statement As String In File.ReadAllText(Path.Combine(root, "schema.sql")).Split(";"c)
            If statement.Trim().Length > 0 Then Exec(statement.Replace("CREATE TABLE `", "CREATE TABLE IF NOT EXISTS `"))
        Next
        TestCancelOnceAndRepeat()
        TestOverlappingOperations()
        TestFailureRollback()
        TestLockTimeout()
        TestZeroBucketAndOtherReceipts()
        TestZeroPostingRejected()
        TestReReceiveBackdateAndDuplicateFinish()
        TestOpnameAnchor()
        TestMissingBalance()
        TestCostFailureRollback()
        TestNullBuckets()
        TestUnknownCommit()
        File.WriteAllLines(Path.Combine(root, "runtime", "captured-operation-log.txt"), logs.ToArray())
        Console.WriteLine("PASS: " & count & " integration scenarios on isolated MySQL 5.7")
    End Sub

    Private Sub ConfigureSettings()
        Dim settingsType As Type = GetType(clsInventory).Assembly.GetType("DreamPos_WebReport.My.MySettings", True)
        Dim settings As Object = settingsType.GetProperty("Default", BindingFlags.Public Or BindingFlags.NonPublic Or BindingFlags.Static).GetValue(Nothing, Nothing)
        For Each pair As KeyValuePair(Of String, Object) In New Dictionary(Of String, Object) From {{"ServerStr", Connection}, {"errLog", Path.Combine(root, "runtime", "app-errLog.txt")}, {"supp_tax", "0"}, {"tosupplier_byacc", 0}, {"stock_cost", "1"}}
            Dim prop As PropertyInfo = settingsType.GetProperty(pair.Key)
            DirectCast(settings, System.Configuration.ApplicationSettingsBase)(pair.Key) = Convert.ChangeType(pair.Value, prop.PropertyType)
        Next
    End Sub

    Private Function Cmd(ByVal sql As String, ByVal conn As MySqlConnection, ByVal values As Object()) As MySqlCommand
        Dim command As New MySqlCommand(sql, conn)
        For i As Integer = 0 To values.Length - 1 Step 2
            command.Parameters.AddWithValue(CStr(values(i)), values(i + 1))
        Next
        Return command
    End Function
    Private Sub Exec(ByVal sql As String, ParamArray ByVal values As Object())
        Using conn As New MySqlConnection(Connection)
            conn.Open()
            Using command As MySqlCommand = Cmd(sql, conn, values)
                command.ExecuteNonQuery()
            End Using
        End Using
    End Sub
    Private Function Value(ByVal sql As String, ParamArray ByVal values As Object()) As Decimal
        Using conn As New MySqlConnection(Connection)
            conn.Open()
            Using command As MySqlCommand = Cmd(sql, conn, values)
                Return Convert.ToDecimal(command.ExecuteScalar())
            End Using
        End Using
    End Function
    Private Sub Equal(ByVal expected As Decimal, ByVal actual As Decimal, ByVal label As String)
        If actual <> expected Then Throw New Exception(label & ": expected=" & expected & " actual=" & actual)
    End Sub
    Private Sub Pass(ByVal label As String)
        count += 1
        Console.WriteLine("PASS " & label)
    End Sub
    Private Sub Log(ByVal text As String, ByVal source As String)
        logs.Add(text)
    End Sub
    Private Function Scope(ByVal conn As MySqlConnection, Optional ByVal doc As String = "RC-TEST-1") As clsReceivePosting
        Return New clsReceivePosting(conn, 22, doc, "tester", "cancel", AddressOf Log)
    End Function
    Private Sub Seed(Optional ByVal receive As Decimal = 55D)
        Exec("DROP TRIGGER IF EXISTS fail_balance")
        For Each table As String In New String() {"cld_inv_trans", "cld_invlk_rcv", "cld_inv_transstockcls", "cld_inv_transusage", "cld_inv_transusagesum", "cld_inv_stk_cc"}
            Exec("DELETE FROM " & table)
        Next
        Exec("INSERT INTO cld_invlk_rcv (id,custid,flag,tipe,cc,InvDate,DocTot) VALUES ('RC-TEST-1',22,2,1,2,@day,550)", "@day", origin)
        Exec("INSERT INTO cld_inv_trans (ID,CustID,InvDate,type,cc,Stk_Id,Stk_Name,U1,U2,Factor1,ttlStock,CostU1,Cost,LinkID,flagtrans,U1_nm,U2_nm,CreatedBy,CreatedDate,stk_no,cc_name) VALUES (1,22,@day,3,2,301,'Red Velvet Cake',0,@qty,1,@qty,10,10,'RC-TEST-1',1,'Pcs','Pcs','tester',NOW(),'DS00002','WAREHOUSE')", "@day", origin, "@qty", receive)
        Dim day As Date = destination.AddDays(-1)
        Do While day <= Date.Today
            Dim active As Decimal = If(day = origin, receive, 0D)
            Dim opening As Decimal = If(day > origin, 5D + receive, 5D)
            Exec("INSERT INTO cld_inv_transstockcls (invDate,custid,stkid,ccid,stkNo,stkName,ccName,openQty,OpenCost,Cost,rcvQty,rtnQty,tinQty,toutQty,rwasteQty,fwasteQty,usageQty,ttlQty,bClose,closeQty,closeCost,varianceQty,HoOutQty,HoInQty,BPIn,BPout,cwasteqty) VALUES (@day,22,301,2,'DS00002','Red Velvet Cake','WAREHOUSE',@open,10,10,@rcv,0,0,0,0,0,0,@total,0,0,0,0,0,0,0,0,0)", "@day", day, "@open", opening, "@rcv", active, "@total", opening + active)
            day = day.AddDays(1)
        Loop
    End Sub
    Private Sub Cancel()
        Dim engine As New clsInventory()
        engine.CancelInvReceive("RC-TEST-1", "tester", 22)
    End Sub
    Private Sub ExpectRejected(ByVal action As Action, ByVal message As String)
        Try
            action()
            Throw New Exception("Expected rejection: " & message)
        Catch ex As ReceivePostingException
            If Not ex.Message.Contains(message) Then Throw
        End Try
    End Sub

    Private Sub TestCancelOnceAndRepeat()
        Seed()
        Cancel()
        Equal(0, Value("SELECT rcvQty FROM cld_inv_transstockcls WHERE invDate=@day", "@day", origin), "cancel receive")
        Equal(5, Value("SELECT ttlQty FROM cld_inv_transstockcls WHERE invDate=CURDATE()"), "carry through today")
        Equal(1, Value("SELECT flag FROM cld_invlk_rcv"), "header pending")
        Equal(0, Value("SELECT flagtrans FROM cld_inv_trans"), "detail pending")
        ExpectRejected(AddressOf Cancel, "sudah dibatalkan")
        Equal(0, Value("SELECT rcvQty FROM cld_inv_transstockcls WHERE invDate=@day", "@day", origin), "repeat must not subtract")
        Equal(10, Value("SELECT cost FROM cld_inv_stk_cc WHERE stkID=301 AND CustID=22 AND ccid=2"), "master cost synced")
        Pass("cancel once, repeated cancel no-op/rejection and carry-forward")
    End Sub

    Private Sub TestOverlappingOperations()
        Seed()
        Using conn As New MySqlConnection(Connection)
            Using first As clsReceivePosting = Scope(conn)
                first.Begin()
                first.Cancel()
                ExpectRejected(AddressOf Cancel, "sedang diproses")
                ExpectRejected(Sub() FinishReceipt(origin), "sedang diproses")
                first.Commit()
            End Using
        End Using
        CancelMustRemainZero()
        Pass("overlapping cancel/receive requests rejected while first holds document lock")
    End Sub
    Private Sub CancelMustRemainZero()
        Equal(0, Value("SELECT rcvQty FROM cld_inv_transstockcls WHERE invDate=@day", "@day", origin), "single effect")
    End Sub

    Private Sub TestFailureRollback()
        Seed()
        Exec("CREATE TRIGGER fail_balance BEFORE UPDATE ON cld_inv_transstockcls FOR EACH ROW SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='injected balance failure'")
        ExpectRejected(AddressOf Cancel, "Proses penerimaan gagal")
        Equal(2, Value("SELECT flag FROM cld_invlk_rcv"), "rollback header")
        Equal(1, Value("SELECT flagtrans FROM cld_inv_trans"), "rollback detail")
        Equal(55, Value("SELECT rcvQty FROM cld_inv_transstockcls WHERE invDate=@day", "@day", origin), "rollback bucket")
        Dim errorLog As String = File.ReadAllText(Path.Combine(root, "runtime", "app-errLog.txt"))
        For Each token As String In New String() {"RECEIVE_FAILED", "rollback=succeeded", "stage=write-balance", "mysqlNumber=1644", "SQL=UPDATE", "@stk=301", "ref=", "injected balance failure"}
            If Not errorLog.Contains(token) Then Throw New Exception("Missing failure diagnostic: " & token)
        Next
        Exec("DROP TRIGGER fail_balance")
        Pass("injected SQL failure rolls back detail/header/buckets and records full context")
    End Sub

    Private Sub TestLockTimeout()
        Seed()
        Using blocker As New MySqlConnection(Connection)
            blocker.Open()
            Using transaction As MySqlTransaction = blocker.BeginTransaction()
                Using command As New MySqlCommand("SELECT id FROM cld_invlk_rcv FOR UPDATE", blocker, transaction)
                    command.ExecuteScalar()
                End Using
                Using conn As New MySqlConnection(Connection)
                    Using attempt As clsReceivePosting = Scope(conn)
                        Try
                            attempt.Begin()
                            attempt.Write("test-timeout", "SET SESSION innodb_lock_wait_timeout=1")
                            attempt.Cancel()
                            Throw New Exception("Expected lock timeout")
                        Catch ex As MySqlException
                            Equal(1205, ex.Number, "MySQL lock timeout")
                            attempt.Fail(ex)
                        End Try
                    End Using
                End Using
                transaction.Rollback()
            End Using
        End Using
        Equal(1, Value("SELECT flagtrans FROM cld_inv_trans"), "timeout does not cancel")
        If Not String.Join(Environment.NewLine, logs.ToArray()).Contains("mysqlNumber=1205") Then Throw New Exception("Missing timeout error number")
        Pass("lock timeout aborts atomically and logs MySQL code/stage")
    End Sub

    Private Sub TestZeroBucketAndOtherReceipts()
        Seed()
        Exec("UPDATE cld_inv_transstockcls SET rcvQty=0 WHERE invDate=@day", "@day", origin)
        Cancel()
        CancelMustRemainZero()
        Seed()
        Exec("INSERT INTO cld_inv_trans (ID,custid,InvDate,type,cc,Stk_Id,ttlStock,Cost,LinkID,flagtrans) VALUES (2,22,@day,3,2,301,10,10,'RC-OTHER',1)", "@day", origin)
        Exec("UPDATE cld_inv_transstockcls SET rcvQty=65,ttlQty=70 WHERE invDate=@day", "@day", origin)
        Cancel()
        Equal(10, Value("SELECT rcvQty FROM cld_inv_transstockcls WHERE invDate=@day", "@day", origin), "preserve other receipt")
        Equal(15, Value("SELECT ttlQty FROM cld_inv_transstockcls WHERE invDate=CURDATE()"), "carry other receipt")
        Seed()
        Exec("UPDATE cld_inv_transstockcls SET rcvQty=-55 WHERE invDate=@day", "@day", origin)
        Cancel()
        CancelMustRemainZero()
        If Not File.ReadAllText(Path.Combine(root, "runtime", "app-errLog.txt")).Contains("RECEIVE_BUCKET_MISMATCH") Then Throw New Exception("Missing pre-existing bucket discrepancy log")
        Pass("zero/negative bucket never subtracts twice; other active receipt remains counted")
    End Sub

    Private Sub TestZeroPostingRejected()
        Seed(0)
        ExpectRejected(AddressOf Cancel, "sudah 0")
        Equal(1, Value("SELECT flagtrans FROM cld_inv_trans"), "zero posting unchanged")
        Pass("active zero quantity rejected with notification/reference")
    End Sub

    Private Sub FinishReceipt(ByVal day As Date)
        Dim details As New DataTable()
        Using conn As New MySqlConnection(Connection)
            conn.Open()
            Using adapter As New MySqlDataAdapter("SELECT *,ttlStock*Cost AS ttlCost FROM cld_inv_trans WHERE LinkID='RC-TEST-1'", conn)
                adapter.Fill(details)
            End Using
        End Using
        Dim engine As New clsInventory()
        engine.insertInvRcv("RC-TEST-1", 2, "WAREHOUSE", details, "", 1, 2, "tester", 22, day, day, "test")
    End Sub
    Private Sub TestReReceiveBackdateAndDuplicateFinish()
        Seed()
        Cancel()
        FinishReceipt(destination)
        Equal(55, Value("SELECT rcvQty FROM cld_inv_transstockcls WHERE invDate=@day", "@day", destination), "backdate destination receive")
        Equal(0, Value("SELECT rcvQty FROM cld_inv_transstockcls WHERE invDate=@day", "@day", origin), "old date receive cleared")
        Equal(60, Value("SELECT ttlQty FROM cld_inv_transstockcls WHERE invDate=CURDATE()"), "backdate carry")
        ExpectRejected(Sub() FinishReceipt(destination), "sudah Receive")
        Equal(55, Value("SELECT SUM(rcvQty) FROM cld_inv_transstockcls"), "no duplicate finish")
        Cancel()
        Equal(5, Value("SELECT ttlQty FROM cld_inv_transstockcls WHERE invDate=CURDATE()"), "cancel uses persisted destination date")
        Pass("move 9->4, receive once, reject duplicate finish and cancel using original persisted date")
    End Sub

    Private Sub TestOpnameAnchor()
        Seed()
        Exec("UPDATE cld_inv_transstockcls SET bClose=1,closeQty=80,closeCost=10 WHERE invDate=@day", "@day", origin.AddDays(1))
        Exec("UPDATE cld_inv_transstockcls SET openQty=80,ttlQty=80 WHERE invDate>@day", "@day", origin.AddDays(1))
        Cancel()
        Equal(5, Value("SELECT ttlQty FROM cld_inv_transstockcls WHERE invDate=@day", "@day", origin), "before opname")
        Equal(80, Value("SELECT openQty FROM cld_inv_transstockcls WHERE invDate=CURDATE()"), "respect opname close")
        Pass("carry-forward respects physical opname boundary")
    End Sub

    Private Sub TestMissingBalance()
        Seed()
        Exec("DELETE FROM cld_inv_transstockcls WHERE invDate=@day", "@day", origin.AddDays(1))
        Cancel()
        Equal(5, Value("SELECT ttlQty FROM cld_inv_transstockcls WHERE invDate=@day", "@day", origin.AddDays(1)), "fill missing quiet day")
        Pass("missing quiet daily row created with correct explicit column names")
    End Sub

    Private Sub TestCostFailureRollback()
        Seed()
        Exec("CREATE TRIGGER fail_cost BEFORE INSERT ON cld_inv_stk_cc FOR EACH ROW SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT='injected cost failure'")
        Try
            ExpectRejected(AddressOf Cancel, "Proses penerimaan gagal")
            Equal(2, Value("SELECT flag FROM cld_invlk_rcv"), "late failure header rollback")
            Equal(55, Value("SELECT rcvQty FROM cld_inv_transstockcls WHERE invDate=@day", "@day", origin), "late failure first day rollback")
            Equal(60, Value("SELECT ttlQty FROM cld_inv_transstockcls WHERE invDate=CURDATE()"), "late failure carry rollback")
        Finally
            Exec("DROP TRIGGER fail_cost")
        End Try
        Pass("failure after full carry-forward still rolls back every affected day")
    End Sub

    Private Sub TestNullBuckets()
        Seed()
        Exec("UPDATE cld_inv_transstockcls SET rtnQty=NULL,tinQty=NULL,usageQty=NULL,HoOutQty=NULL,ttlQty=NULL WHERE invDate=@day", "@day", origin)
        Cancel()
        Equal(5, Value("SELECT ttlQty FROM cld_inv_transstockcls WHERE invDate=@day", "@day", origin), "NULL-safe total")
        Pass("nullable movement buckets produce verified numeric total")
    End Sub

    Private Sub TestUnknownCommit()
        Seed()
        Using conn As New MySqlConnection(Connection)
            Using attempt As clsReceivePosting = Scope(conn)
                attempt.Begin()
                attempt.Cancel()
                Dim thread As Integer = conn.ServerThread
                Exec("KILL CONNECTION " & thread)
                Try
                    attempt.Commit()
                    Throw New Exception("Expected disconnected commit")
                Catch ex As MySqlException
                    Dim friendly As ReceivePostingException = attempt.Fail(ex)
                    If Not friendly.Message.Contains("belum dapat dipastikan") Then Throw New Exception("Commit ambiguity not reported")
                End Try
            End Using
        End Using
        Equal(1, Value("SELECT flagtrans FROM cld_inv_trans"), "killed connection rollback")
        If Not String.Join(Environment.NewLine, logs.ToArray()).Contains("UNKNOWN_CHECK_DATABASE") Then Throw New Exception("Missing uncertain commit outcome")
        Pass("connection loss at commit reported as uncertain, never auto-retried")
    End Sub
End Module
