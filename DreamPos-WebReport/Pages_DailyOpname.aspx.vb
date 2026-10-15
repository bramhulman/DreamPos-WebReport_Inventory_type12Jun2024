Imports System.IO
Imports System.Data
Imports System.Globalization
Imports MySql.Data.MySqlClient
Imports ClosedXML.Excel

Public Class Pages_DailyOpname
    Inherits System.Web.UI.Page

    Private sNameuser As String
    Private icustid As Integer
    Private imodul As Integer
    Private iModPriv As Integer
    Private irptpriv As Integer
    Private irptf As Integer
    Private mfunction As New ModFunction
    Private cInv As New clsInventory

#Region "Authentication & Session"
    Public Function modulepriv() As Integer
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        If cookie Is Nothing Then
            Response.Redirect("default.aspx")
        End If
        Return Val(cookie("modulepriv"))
    End Function

    Public Function custid() As Integer
        Dim icust As Integer = 0
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        If cookie Is Nothing Then
            Response.Redirect("~/Default.aspx")
        Else
            icust = Val(cookie("custid"))
        End If
        Return icust
    End Function

    Public Function username() As String
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        If cookie Is Nothing Then
            Response.Redirect("default.aspx")
        End If
        Return cookie("userName")
    End Function

    Public Function modul() As Integer
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        If cookie Is Nothing Then
            Return 0
        End If
        Return Val(cookie("modul"))
    End Function
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("username") Is Nothing OrElse custid() < 1 Then
            Session.Clear()
            Session.Abandon()
            Session.RemoveAll()
            FormsAuthentication.RedirectToLoginPage()
            Exit Sub
        End If

        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        If cookie IsNot Nothing Then
            sNameuser = username()
            icustid = custid()
            imodul = modul()
            iModPriv = modulepriv()

            Dim action As String = Request("action")
            If Not String.IsNullOrEmpty(action) Then
                HandleAjaxRequest(action)
                Exit Sub
            End If

            ' ── Hak Akses Tanggal Opname ─────────────────────────────
            Dim canChangePastDate As Boolean = IsAdminOrSupport()
            Dim dtYesterday As Date = Date.Today.AddDays(-1)

            If Not canChangePastDate Then
                ' User biasa: tanggal dikunci di H-1 (kemarin)
                txtDate.Text = dtYesterday.ToString("dd MMM yyyy")
                txtDate.ReadOnly = True
                txtDate.Attributes.Add("style", "height: 31px; font-size: 0.82rem; background-color: #e9ecef; cursor: not-allowed;")
                txtDate.ToolTip = "Tanggal opname dikunci (H-1). Hanya user support dan admin yang dapat mengubah tanggal sebelum kemarin."
                calExt.Enabled = False
            Else
                ' Admin / Support: bebas memilih tanggal
                txtDate.ReadOnly = False
                txtDate.Attributes.Add("style", "height: 31px; font-size: 0.82rem;")
                calExt.Enabled = True
            End If

            If Not IsPostBack Then
                If canChangePastDate AndAlso String.IsNullOrEmpty(txtDate.Text) Then
                    txtDate.Text = dtYesterday.ToString("dd MMM yyyy")
                End If

                LoadCostCentres()
                LoadData()
            End If
        Else
            FormsAuthentication.RedirectToLoginPage()
        End If
    End Sub

    Private Sub HandleAjaxRequest(ByVal action As String)
        Response.Clear()
        Response.ContentType = "application/json"

        Dim result As New Dictionary(Of String, Object)
        Try
            Dim targetDateHMinus1 As Date
            If Not Date.TryParse(Request("date"), targetDateHMinus1) Then
                If Not Date.TryParse(txtDate.Text, targetDateHMinus1) Then
                    targetDateHMinus1 = Date.Today.AddDays(-1)
                End If
            End If

            ' ── Guard: Pembatasan Tanggal Sebelum Kemarin (H-1) ──────
            Dim dtYesterday As Date = Date.Today.AddDays(-1)
            If targetDateHMinus1.Date < dtYesterday.Date AndAlso Not IsAdminOrSupport() Then
                result("success") = False
                result("message") = "Hanya user support dan admin yang dapat mengubah data sebelum kemarin."
                Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(result))
                Response.End()
                Return
            End If
            ' ─────────────────────────────────────────────────────────

            Dim stkId As Integer = Val(Request("stkId"))
            Dim ccId As Integer = Val(Request("ccId"))

            ' ── Guard: custid harus valid ────────────────────────────
            If icustid <= 0 Then
                result("success") = False
                result("message") = "Sesi tidak valid. Silakan login ulang."
                Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(result))
                Response.End()
                Return
            End If

            If stkId <= 0 OrElse ccId <= 0 Then
                result("success") = False
                result("message") = "Parameter Stock ID / CC ID tidak valid."
                Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(result))
                Response.End()
                Return
            End If

            ' ── Guard: pastikan ccId memang milik custid ini ─────────
            Dim ccCheckCmd As String = "SELECT COUNT(1) FROM cld_inv_cc WHERE id = " & ccId & " AND custid = " & icustid
            Dim ccCheck As New clsDbo(ccCheckCmd)
            Dim ccDs As DataSet = ccCheck.selectDataset
            ccCheck.dispose()
            Dim ccValid As Boolean = (ccDs IsNot Nothing AndAlso ccDs.Tables.Count > 0 AndAlso
                                      ccDs.Tables(0).Rows.Count > 0 AndAlso
                                      CInt(ccDs.Tables(0).Rows(0)(0)) > 0)
            If Not ccValid Then
                result("success") = False
                result("message") = "Akses CC tidak diizinkan untuk akun ini."
                Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(result))
                Response.End()
                Return
            End If
            ' ─────────────────────────────────────────────────────────


            Dim opnameVal As Decimal = 0
            If action = "save_row" Then
                Dim rawVal As String = (Request("val") & "").Trim()
                If rawVal.Contains(",") AndAlso Not rawVal.Contains(".") Then
                    rawVal = rawVal.Replace(",", ".")
                ElseIf rawVal.Contains(",") AndAlso rawVal.Contains(".") Then
                    rawVal = rawVal.Replace(",", "")
                End If
                Decimal.TryParse(rawVal, NumberStyles.Any, CultureInfo.InvariantCulture, opnameVal)
            ElseIf action = "cancel_row" Then
                opnameVal = 0
            Else
                result("success") = False
                result("message") = "Aksi tidak dikenal."
                Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(result))
                Response.End()
                Return
            End If

            Dim rowMeta As DataRow = GetStockMeta(stkId, ccId, icustid)
            If rowMeta IsNot Nothing Then
                Dim stkNo As String = rowMeta("stkNo").ToString()
                Dim stkName As String = rowMeta("stkName").ToString()
                Dim ccName As String = rowMeta("ccName").ToString()
                Dim factor1 As Decimal = Convert.ToDecimal(rowMeta("Factor1"))
                Dim unit1 As String = rowMeta("Unit1").ToString()
                Dim unit2 As String = rowMeta("Unit2").ToString()
                Dim cost As Decimal = Convert.ToDecimal(rowMeta("Cost"))

                Dim adjustnow As Integer = 2
                If cInv.get_bclose(stkId, ccId, targetDateHMinus1.ToString("yyyy-MM-dd 00:00:00"), icustid) = True Then
                    Dim adtype As Integer = cInv.get_typestockopname(stkId, ccId, targetDateHMinus1.ToString("yyyy-MM-dd 00:00:00"), icustid)
                    If adtype > 0 Then
                        adjustnow = Right(adtype.ToString, 1)
                    End If
                End If

                If action = "cancel_row" Then
                    cInv.CancelStockOpname(stkId, ccId, icustid, targetDateHMinus1)
                Else
                    cInv.insertStockOpname(ccId, stkId, stkName, 0, 0, factor1, opnameVal, cost, unit1, unit2, sNameuser, sNameuser, stkNo, ccName, sNameuser, icustid, 2)
                End If

                result("success") = True
                result("stkId") = stkId
                result("ccId") = ccId
                result("stkName") = stkName
                result("opnameVal") = opnameVal
                result("isCancel") = (action = "cancel_row")
                result("message") = If(action = "cancel_row", "Opname dibatalkan (nilai kembali 0)", "Tersimpan")
            Else
                result("success") = False
                result("message") = "Data master stock tidak ditemukan."
            End If

        Catch ex As Exception
            mfunction.errLog(ex.Message, "Pages_DailyOpname:HandleAjaxRequest:" & action)
            result("success") = False
            result("message") = ex.Message
        End Try

        Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(result))
        Response.End()
    End Sub


    Private Sub LoadCostCentres()
        Try
            ddlCc.Items.Clear()
            Dim cCc As New clsInventory_Cc
            Dim ds As DataSet = cCc.SelectDataset_Icc(icustid, sNameuser, iModPriv, 0)

            Dim isModuleUser As Boolean = (iModPriv And usrModulepriv.moduleuser) = usrModulepriv.moduleuser
            If Not isModuleUser Then
                ddlCc.Items.Add(New ListItem("-- Semua Cost Centre --", "0"))
            End If

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 Then
                For Each r As DataRow In ds.Tables(0).Rows
                    Dim ccId As String = r("id").ToString()
                    Dim ccName As String = r("name").ToString()
                    ddlCc.Items.Add(New ListItem(ccName, ccId))
                Next
            End If

            ' Default ke outlet pertama yang aktif agar initial load cepat & ringan (~500 baris, bukan 12.000 baris)
            If ddlCc.Items.Count > 1 AndAlso Not isModuleUser Then
                ddlCc.SelectedIndex = 1
            ElseIf ddlCc.Items.Count > 0 Then
                ddlCc.SelectedIndex = 0
            End If
        Catch ex As Exception
            mfunction.errLog(ex.Message, "Pages_DailyOpname:LoadCostCentres")
        End Try
    End Sub

    Private Function GetData(ByVal targetDate As Date, Optional ByVal filterCcId As Integer = 0) As DataTable
        Dim dtResult As New DataTable
        Dim connStr As String = ServerString

        Try
            Using conn As New MySqlConnection(connStr)
                conn.Open()

                Dim sql As String = "SELECT " &
                    "TransStock.stkid, " &
                    "TransStock.ccid, " &
                    "TransStock.stkNo AS CODE, " &
                    "TransStock.stkName AS Inventory_Stock_Name, " &
                    "TransStock.ccName AS CCName, " &
                    "IFNULL(TransStock.OpenQty, 0) AS OpenQty, " &
                    "IFNULL(TransStock.rcvQty, 0) AS RcvQty, " &
                    "IFNULL(TransStock.tinQty, 0) AS TinQty, " &
                    "IFNULL(TransStock.toutQty, 0) AS ToutQty, " &
                    "IFNULL(TransStock.rwasteQty, 0) AS RWasteQTY, " &
                    "IFNULL(TransStock.BPin, 0) AS BPinQty, " &
                    "IFNULL(TransStock.BPout, 0) AS BPoutQty, " &
                    "IFNULL(TransStock.usageQty, 0) AS UsageQty, " &
                    "IFNULL(TransStock.ttlQty, 0) AS EndingQty, " &
                    "IFNULL(TransStock.ttlQty, 0) AS TotalQty, " &
                    "IFNULL(TransStock.Cost, 0) AS Cost, " &
                    "IFNULL(TransStock.bClose, 0) AS bClose, " &
                    "IFNULL(TransStock.closeQty, 0) AS closeQty, " &
                    "CASE WHEN TransStock.bClose = 1 THEN IFNULL(TransStock.varianceQty, (TransStock.closeQty - TransStock.ttlQty)) " &
                    "     ELSE 0 END AS Variance, " &
                    "IFNULL(cld_inv_stk.unit2, IFNULL(cld_inv_stk.unit1, 'Pcs')) AS Unit " &
                    "FROM cld_inv_transstockcls AS TransStock " &
                    "INNER JOIN cld_inv_stk ON (TransStock.custId = cld_inv_stk.CustID AND TransStock.stkId = cld_inv_stk.ID) " &
                    "INNER JOIN cld_inv_cc ON (TransStock.custId = cld_inv_cc.CustID AND TransStock.ccid = cld_inv_cc.ID) " &
                    "WHERE TransStock.invDate = @paramDate AND TransStock.custId = @custId "

                ' ── Pembatasan akses CC mengikuti pola Pages_ListOpname ──────────────────
                ' moduleuser (user outlet) → hanya CC yang ada di cld_userxstore milik user
                ' non-moduleuser (admin/HQ)→ semua CC boleh dilihat
                Dim isModuleUser As Boolean = (iModPriv And usrModulepriv.moduleuser) = usrModulepriv.moduleuser

                If isModuleUser Then
                    ' User outlet: selalu filter berdasarkan cld_userxstore
                    If filterCcId > 0 Then
                        ' Pilih CC tertentu, tapi tetap harus CC yang boleh diakses user
                        sql &= "AND TransStock.ccid = @ccId " &
                               "AND cld_inv_cc.olid IN (SELECT Store FROM cld_userxstore WHERE user = @userName) "
                    Else
                        ' Semua CC yang boleh diakses user ini
                        sql &= "AND cld_inv_cc.olid IN (SELECT Store FROM cld_userxstore WHERE user = @userName) "
                    End If
                Else
                    ' Admin / HQ: lihat semua CC, bisa filter by ccId jika dipilih
                    If filterCcId > 0 Then
                        sql &= "AND TransStock.ccid = @ccId "
                    End If
                End If
                ' ─────────────────────────────────────────────────────────────────────────

                sql &= "ORDER BY TransStock.stkNo ASC, TransStock.stkName ASC"

                Using cmd As New MySqlCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@paramDate", targetDate.ToString("yyyy-MM-dd 00:00:00"))
                    cmd.Parameters.AddWithValue("@custId", icustid)
                    If filterCcId > 0 Then
                        cmd.Parameters.AddWithValue("@ccId", filterCcId)
                    End If
                    If isModuleUser Then
                        cmd.Parameters.AddWithValue("@userName", sNameuser)
                    End If

                    Using da As New MySqlDataAdapter(cmd)
                        da.Fill(dtResult)
                    End Using
                End Using
            End Using
        Catch ex As Exception
            mfunction.errLog(ex.Message, "Pages_DailyOpname:GetData")
            ShowAlert("Error mengambil data: " & ex.Message, "danger")
        End Try

        Return dtResult
    End Function

    Private Sub LoadData()
        Try
            Dim targetDate As Date
            If Not Date.TryParse(txtDate.Text, targetDate) Then
                targetDate = Date.Today.AddDays(-1)
                txtDate.Text = targetDate.ToString("dd MMM yyyy")
            End If

            Dim selCcId As Integer = Val(ddlCc.SelectedValue)
            Dim dt As DataTable = GetData(targetDate, selCcId)

            rptOpname.DataSource = dt
            rptOpname.DataBind()

            ' Update Statistics
            Dim totalItems As Integer = dt.Rows.Count
            Dim opnamedItems As Integer = 0
            Dim varianceItems As Integer = 0

            For Each r As DataRow In dt.Rows
                Dim isClosed As Boolean = IsOpnamed(r("bClose"))

                If isClosed Then
                    opnamedItems += 1
                    If Not IsDBNull(r("Variance")) Then
                        Dim varVal As Decimal
                        If Decimal.TryParse(r("Variance").ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, varVal) Then
                            If Math.Abs(varVal) > 0.0001D Then
                                varianceItems += 1
                            End If
                        End If
                    End If
                End If
            Next

            lblStatTotal.InnerText = totalItems.ToString("N0", CultureInfo.InvariantCulture)
            lblStatOpnamed.InnerText = opnamedItems.ToString("N0", CultureInfo.InvariantCulture)
            lblStatPending.InnerText = (totalItems - opnamedItems).ToString("N0", CultureInfo.InvariantCulture)
            lblStatVariance.InnerText = varianceItems.ToString("N0", CultureInfo.InvariantCulture)
            badgeRowCount.InnerText = totalItems.ToString("N0", CultureInfo.InvariantCulture) & " Rows"

            ' Cache for export
            Session("DailyOpname_DataTable") = dt
            Session("DailyOpname_Date") = targetDate
        Catch ex As Exception
            mfunction.errLog(ex.Message, "Pages_DailyOpname:LoadData")
            ShowAlert("Gagal memuat data: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub btnFilter_Click(sender As Object, e As EventArgs)
        LoadData()
    End Sub

    Protected Sub ddlCc_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlCc.SelectedIndexChanged
        LoadData()
    End Sub

    Private Class OpnameInputItem
        Public Property stkId As Integer
        Public Property ccId As Integer
        Public Property val As String
    End Class

    Private Function GetStockMeta(ByVal stkId As Integer, ByVal ccId As Integer, ByVal custId As Integer) As DataRow
        Dim dt As New DataTable()
        Dim sql As String = "SELECT S.ID AS stkId, S.Number AS stkNo, S.Name AS stkName, " &
            "IFNULL(S.Factor1, 1) AS Factor1, " &
            "IFNULL(S.Unit1, 'Pcs') AS Unit1, " &
            "IFNULL(S.Unit2, IFNULL(S.Unit1, 'Pcs')) AS Unit2, " &
            "IFNULL(C.Name, '') AS ccName, " &
            "IFNULL((SELECT T.Cost FROM cld_inv_transstockcls T WHERE T.custId = @custId AND T.ccid = @ccId AND T.stkId = @stkId ORDER BY T.invDate DESC LIMIT 1), 0) AS Cost " &
            "FROM cld_inv_stk S " &
            "LEFT JOIN cld_inv_cc C ON (C.CustID = S.CustID AND C.ID = @ccId) " &
            "WHERE S.CustID = @custId AND S.ID = @stkId LIMIT 1"

        Try
            Using conn As New MySqlConnection(ServerString)
                conn.Open()
                Using cmd As New MySqlCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@custId", custId)
                    cmd.Parameters.AddWithValue("@ccId", ccId)
                    cmd.Parameters.AddWithValue("@stkId", stkId)
                    Using da As New MySqlDataAdapter(cmd)
                        da.Fill(dt)
                    End Using
                End Using
            End Using

            If dt.Rows.Count > 0 Then
                Return dt.Rows(0)
            End If
        Catch ex As Exception
            mfunction.errLog(ex.Message, "Pages_DailyOpname:GetStockMeta")
        End Try
        Return Nothing
    End Function

    Protected Sub btnSubmit_Click(sender As Object, e As EventArgs)
        Dim targetDateHMinus1 As Date
        If Not Date.TryParse(txtDate.Text, targetDateHMinus1) Then
            targetDateHMinus1 = Date.Today.AddDays(-1)
        End If

        ' ── Guard: Pembatasan Tanggal Sebelum Kemarin (H-1) ──────
        Dim dtYesterday As Date = Date.Today.AddDays(-1)
        If targetDateHMinus1.Date < dtYesterday.Date AndAlso Not IsAdminOrSupport() Then
            ShowAlert("Hanya user support dan admin yang dapat mengubah data sebelum kemarin.", "danger")
            Exit Sub
        End If
        ' ─────────────────────────────────────────────────────────

        Dim todayDate As Date = Date.Today
        Dim updateCount As Integer = 0

        If String.IsNullOrEmpty(sNameuser) Then sNameuser = username()
        If icustid <= 0 Then icustid = custid()

        ' Parse JSON from hfOpnameJson
        Dim modifiedList As New List(Of OpnameInputItem)
        If Not String.IsNullOrWhiteSpace(hfOpnameJson.Value) Then
            Try
                modifiedList = Newtonsoft.Json.JsonConvert.DeserializeObject(Of List(Of OpnameInputItem))(hfOpnameJson.Value)
            Catch exJson As Exception
                mfunction.errLog("JSON Parse error: " & exJson.Message, "Pages_DailyOpname:btnSubmit_Click")
            End Try
        End If

        ' Fallback: check Request.Form for opname_ keys if JSON empty
        If modifiedList Is Nothing OrElse modifiedList.Count = 0 Then
            For Each key As String In Request.Form.AllKeys
                If key IsNot Nothing AndAlso key.StartsWith("opname_") Then
                    Dim parts() As String = key.Split("_"c)
                    If parts.Length = 3 Then
                        Dim sId As Integer = Val(parts(1))
                        Dim cId As Integer = Val(parts(2))
                        Dim inputVal As String = Request.Form(key)
                        If sId > 0 AndAlso cId > 0 AndAlso Not String.IsNullOrWhiteSpace(inputVal) Then
                            modifiedList.Add(New OpnameInputItem With {.stkId = sId, .ccId = cId, .val = inputVal})
                        End If
                    End If
                End If
            Next
        End If

        If modifiedList Is Nothing OrElse modifiedList.Count = 0 Then
            ShowAlert("Peringatan: Belum ada kuantitas opname yang diisi atau diubah.", "warning")
            Exit Sub
        End If

        Try
            For Each item In modifiedList
                Dim opnameVal As Decimal = 0
                Dim cleanText As String = item.val.Trim()
                If cleanText.Contains(",") AndAlso Not cleanText.Contains(".") Then
                    cleanText = cleanText.Replace(",", ".")
                ElseIf cleanText.Contains(",") AndAlso cleanText.Contains(".") Then
                    cleanText = cleanText.Replace(",", "")
                End If

                If Decimal.TryParse(cleanText, NumberStyles.Any, CultureInfo.InvariantCulture, opnameVal) Then
                    Dim stkId As Integer = item.stkId
                    Dim ccId As Integer = item.ccId

                    ' Ambil metadata stock (persis seperti info di Pages_ListOpname)
                    Dim rowMeta As DataRow = GetStockMeta(stkId, ccId, icustid)
                    If rowMeta IsNot Nothing Then
                        Dim stkNo As String = rowMeta("stkNo").ToString()
                        Dim stkName As String = rowMeta("stkName").ToString()
                        Dim ccName As String = rowMeta("ccName").ToString()
                        Dim factor1 As Decimal = Convert.ToDecimal(rowMeta("Factor1"))
                        Dim unit1 As String = rowMeta("Unit1").ToString()
                        Dim unit2 As String = rowMeta("Unit2").ToString()
                        Dim cost As Decimal = Convert.ToDecimal(rowMeta("Cost"))

                        ' Meniru/menyamakan fungsi penyimpanan seperti pada Pages_ListOpname.aspx.vb line 344-357:
                        Dim adjustnow As Integer = 2
                        If cInv.get_bclose(stkId, ccId, targetDateHMinus1.ToString("yyyy-MM-dd 00:00:00"), icustid) = True Then
                            Dim adtype As Integer = cInv.get_typestockopname(stkId, ccId, targetDateHMinus1.ToString("yyyy-MM-dd 00:00:00"), icustid)
                            If adtype > 0 Then
                                adjustnow = Right(adtype.ToString, 1)
                            End If
                        End If

                        ' Eksekusi insertStockOpname sama persis seperti Pages_ListOpname.aspx
                        cInv.insertStockOpname(ccId, stkId, stkName, 0, 0, factor1, opnameVal, cost, unit1, unit2, sNameuser, sNameuser, stkNo, ccName, sNameuser, icustid, 2)

                        updateCount += 1
                    End If
                End If
            Next

            hfOpnameJson.Value = ""
            ShowAlert(String.Format("<b>Sukses!</b> Sebanyak <b>{0}</b> item opname berhasil disimpan dan diproses. CloseQty H-1 ({1}) dan OpenQty Hari Berjalan ({2}) telah diperbarui.",
                                    updateCount, targetDateHMinus1.ToString("dd MMM yyyy"), todayDate.ToString("dd MMM yyyy")), "success")
            LoadData()
        Catch ex As Exception
            mfunction.errLog(ex.Message, "Pages_DailyOpname:btnSubmit_Click")
            ShowAlert("Terjadi kesalahan saat menyimpan opname: " & ex.Message, "danger")
        End Try
    End Sub

    Protected Sub btnExportExcel_Click(sender As Object, e As EventArgs)
        ExportData(True)
    End Sub

    Protected Sub btnExportCsv_Click(sender As Object, e As EventArgs)
        ExportData(False)
    End Sub

    Private Sub ExportData(ByVal isExcel As Boolean)
        Try
            Dim dt As DataTable = TryCast(Session("DailyOpname_DataTable"), DataTable)
            If dt Is Nothing OrElse dt.Rows.Count = 0 Then
                ShowAlert("Tidak ada data untuk diexport.", "warning")
                Exit Sub
            End If

            Dim opDate As Date = If(Session("DailyOpname_Date") IsNot Nothing, CDate(Session("DailyOpname_Date")), Date.Today.AddDays(-1))
            Dim fileName As String = "Daily_Opname_" & opDate.ToString("yyyyMMdd")

            ' Prepare clean DataTable with specified columns
            Dim dtExport As New DataTable("DailyOpname")
            dtExport.Columns.Add("CODE", GetType(String))
            dtExport.Columns.Add("Inventory_Stock_Name", GetType(String))
            dtExport.Columns.Add("CCName", GetType(String))
            dtExport.Columns.Add("OpenQty", GetType(Double))
            dtExport.Columns.Add("RcvQty", GetType(Double))
            dtExport.Columns.Add("TinQty", GetType(Double))
            dtExport.Columns.Add("ToutQty", GetType(Double))
            dtExport.Columns.Add("RWasteQTY", GetType(Double))
            dtExport.Columns.Add("BPinQty", GetType(Double))
            dtExport.Columns.Add("BPoutQty", GetType(Double))
            dtExport.Columns.Add("UsageQty", GetType(Double))
            dtExport.Columns.Add("EndingQty", GetType(Double))
            dtExport.Columns.Add("Opname", GetType(String))
            dtExport.Columns.Add("Variance", GetType(String))
            dtExport.Columns.Add("Unit", GetType(String))

            For Each r As DataRow In dt.Rows
                Dim isClosed As Boolean = (r("bClose") IsNot DBNull.Value AndAlso Convert.ToInt32(r("bClose")) = 1)
                Dim opValStr As String = If(isClosed, Convert.ToDecimal(r("closeQty")).ToString("0.####", CultureInfo.InvariantCulture), "")
                Dim varValStr As String = If(isClosed, Convert.ToDecimal(r("Variance")).ToString("0.####", CultureInfo.InvariantCulture), "")

                dtExport.Rows.Add(
                    r("CODE").ToString(),
                    r("Inventory_Stock_Name").ToString(),
                    r("CCName").ToString(),
                    Convert.ToDouble(r("OpenQty")),
                    Convert.ToDouble(r("RcvQty")),
                    Convert.ToDouble(r("TinQty")),
                    Convert.ToDouble(r("ToutQty")),
                    Convert.ToDouble(r("RWasteQTY")),
                    Convert.ToDouble(r("BPinQty")),
                    Convert.ToDouble(r("BPoutQty")),
                    Convert.ToDouble(r("UsageQty")),
                    Convert.ToDouble(r("EndingQty")),
                    opValStr,
                    varValStr,
                    r("Unit").ToString()
                )
            Next

            If isExcel Then
                Using wb As New XLWorkbook()
                    Dim ws = wb.Worksheets.Add(dtExport, "Opname_H-1")
                    ws.Columns().AdjustToContents()

                    Response.Clear()
                    Response.Buffer = True
                    Response.Charset = ""
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                    Response.AddHeader("content-disposition", "attachment;filename=" & fileName & ".xlsx")
                    Using ms As New MemoryStream()
                        wb.SaveAs(ms)
                        ms.WriteTo(Response.OutputStream)
                        HttpContext.Current.Response.Flush()
                        HttpContext.Current.Response.SuppressContent = True
                        HttpContext.Current.ApplicationInstance.CompleteRequest()
                    End Using
                End Using
            Else
                ' Export CSV
                Response.Clear()
                Response.Buffer = True
                Response.ContentType = "text/csv"
                Response.AddHeader("content-disposition", "attachment;filename=" & fileName & ".csv")

                Using sw As New StreamWriter(Response.OutputStream)
                    ' Header
                    Dim colNames As New List(Of String)
                    For Each col As DataColumn In dtExport.Columns
                        colNames.Add(col.ColumnName)
                    Next
                    sw.WriteLine(String.Join(",", colNames))

                    ' Rows
                    For Each r As DataRow In dtExport.Rows
                        Dim rowVals As New List(Of String)
                        For Each col As DataColumn In dtExport.Columns
                            Dim val As String = r(col).ToString()
                            If val.Contains(",") OrElse val.Contains("""") OrElse val.Contains(vbCrLf) Then
                                val = """" & val.Replace("""", """""") & """"
                            End If
                            rowVals.Add(val)
                        Next
                        sw.WriteLine(String.Join(",", rowVals))
                    Next
                    sw.Flush()
                    HttpContext.Current.Response.Flush()
                    HttpContext.Current.Response.SuppressContent = True
                    HttpContext.Current.ApplicationInstance.CompleteRequest()
                End Using
            End If
        Catch ex As Exception
            mfunction.errLog(ex.Message, "Pages_DailyOpname:ExportData")
        End Try
    End Sub

#Region "Formatting Helpers"
    Public Function IsAdminOrSupport() As Boolean
        Dim u As String = sNameuser
        If String.IsNullOrEmpty(u) Then u = username()
        If String.IsNullOrEmpty(u) Then Return False

        Dim adminConfig As String = ""
        Try
            adminConfig = My.Settings.admin
        Catch ex As Exception
        End Try

        Return (Not String.IsNullOrEmpty(adminConfig) AndAlso u.Equals(adminConfig, StringComparison.OrdinalIgnoreCase)) OrElse
               u.Equals("support", StringComparison.OrdinalIgnoreCase)
    End Function

    Public Function IsOpnamed(ByVal bClose As Object) As Boolean
        If bClose Is Nothing OrElse IsDBNull(bClose) Then Return False
        Dim s As String = bClose.ToString().Trim().ToLower()
        Return s = "1" OrElse s = "true" OrElse s = "-1"
    End Function

    Public Function GetOpnameVal(ByVal bClose As Object, ByVal closeQty As Object) As String
        If Not IsOpnamed(bClose) Then Return "0"
        Return FormatInputQty(closeQty)
    End Function

    Public Function GetVarianceDisplay(ByVal bClose As Object, ByVal varianceVal As Object) As String
        If Not IsOpnamed(bClose) Then Return "-"
        Return FormatNumberQty(varianceVal)
    End Function

    Public Function HasVariance(ByVal bClose As Object, ByVal varianceVal As Object) As Boolean
        If Not IsOpnamed(bClose) Then Return False
        If varianceVal Is Nothing OrElse IsDBNull(varianceVal) Then Return False
        Dim dec As Decimal
        If Decimal.TryParse(varianceVal.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, dec) Then
            Return Math.Abs(dec) > 0.0001D
        End If
        Return False
    End Function

    Public Function FormatNumberQty(ByVal value As Object) As String
        If value Is Nothing OrElse IsDBNull(value) Then Return "0,00"
        Dim dbl As Decimal
        If Decimal.TryParse(value.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, dbl) Then
            Return dbl.ToString("N2", New CultureInfo("id-ID"))
        End If
        Return "0,00"
    End Function

    Public Function FormatInputQty(ByVal value As Object) As String
        If value Is Nothing OrElse IsDBNull(value) Then Return ""
        Dim dbl As Decimal
        If Decimal.TryParse(value.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, dbl) Then
            Return dbl.ToString("0.####", CultureInfo.InvariantCulture)
        End If
        Return ""
    End Function

    Public Function GetVarianceCss(ByVal bClose As Object, ByVal varianceVal As Object) As String
        If Not IsOpnamed(bClose) Then Return ""
        If varianceVal Is Nothing OrElse IsDBNull(varianceVal) Then Return ""
        Dim dec As Decimal
        If Decimal.TryParse(varianceVal.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, dec) Then
            If Math.Abs(dec) < 0.0001D Then Return "var-zero"
            If dec > 0 Then Return "var-positive"
            Return "var-negative"
        End If
        Return ""
    End Function

    Private Sub ShowAlert(ByVal msg As String, ByVal alertType As String)
        pnlAlert.Visible = True
        pnlAlert.CssClass = "alert alert-" & alertType & " alert-dismissible fade show shadow-sm"
        lblAlertMsg.Text = msg
    End Sub
#End Region

End Class
