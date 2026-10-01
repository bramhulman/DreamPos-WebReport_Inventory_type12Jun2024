Imports System.IO
Imports NPOI.XSSF.UserModel
Imports NPOI.SS.UserModel
Imports System.Net
Imports ClosedXML.Excel

Public Class Pages_ExprtImprt
    Inherits System.Web.UI.Page
    Private sNameuser As String
    Private Userid As Integer
    Private icustid As Integer
    Private imodul As Integer
    Private iModPriv As Integer
    Private irptpriv As Integer
    Private irptf As Integer
    Private mfunction As New ModFunction
    Private clsInventory As New clsInventory_stkOpname
    Private ccid As Integer
    Private inv_date As Date



    'Private transpriv As Integer
    Public Function modulepriv() As Integer
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        Return cookie("modulepriv")

    End Function
    Public Function custid() As Integer
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")

        If cookie Is Nothing Then
            Return -1
        Else
            Return cookie("Custid")
        End If

    End Function

    Public Function rptpriv() As Integer
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")

        Return cookie("rptpriv")

    End Function
    Public Function modul() As Integer
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        Return cookie("modul")

    End Function
    Public Function rptf() As Integer
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")

        Return cookie("rptf")


    End Function
    Public Sub loadCCName()
        Try
            Dim ds As DataSet
            Dim clsInv As New clsInventory
            'ddlCCName.Items.Clear()
            ds = clsInv.getInvcc(custid, username, modulepriv, 0)
            If ds.Tables.Count > 0 Then
                With ddlCCName
                    .DataSource = ds.Tables(0)
                    .DataTextField = "Name"
                    '.DataValueField = "Name"
                    .DataValueField = "ID"
                    .DataBind()
                End With
            End If
            ds.Dispose()
            ds = Nothing

            ' txtDate.Text = Now.Date.ToString("yyyy-MM-dd")

        Catch ex As Exception
            errLog(Err.Description, "Pages_ExprtImprt:loadCCName")
            Err.Clear()
        End Try

    End Sub
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        'If Not (User.Identity.IsAuthenticated) Or custid() = -1 Then
        '    FormsAuthentication.RedirectToLoginPage()
        '    Exit Sub
        'End If
        If Session("username") Is Nothing Or custid() < 1 Then
            Session.Clear()
            Session.Abandon()
            Session.RemoveAll()
            FormsAuthentication.RedirectToLoginPage()
            Exit Sub
        End If
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        If Not cookie Is Nothing Then

            If Not Me.IsPostBack Then
                ccid = Request.Cookies("Ccid").Value
                ddlCCName.SelectedValue = ccid
                inv_date = Request.Cookies("invdate").Value
                txtDate.Text = Now.Date.ToString("yyyy-MM-dd")
                'txtDate.Text = inv_date

                If (username() = My.Settings.admin Or username() = "support") Then
                    txtDate.Enabled = True
                Else
                    txtDate.Enabled = False
                End If

                loadCCName()

                End If
            Else
            Dim script As String = "window.onload = function(){ alert('"
            script += "Please Login"
            script += "');"
            script += "window.location = '"
            script += "../Default"
            script += " '; }"
            ClientScript.RegisterStartupScript(Me.GetType(), "Redirect", script, True)
        End If
        'loadCCName()

    End Sub

    Protected Sub Page_unLoad(sender As Object, e As EventArgs) Handles Me.Unload
    End Sub

    Public Function username() As String
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        Return cookie("userName")

    End Function



    Sub messagesuccesfull(ByVal msg As String, Optional ByVal bsuccess As Boolean = False)
        Dim message As String = msg
        If message IsNot Nothing Then
            message = message.Replace("\", "\\").Replace("'", "\'").Replace(vbCrLf, "\n").Replace(vbCr, "\n").Replace(vbLf, "\n")
        Else
            message = ""
        End If

        Dim url As String = ""
        If bsuccess Then
            url = ResolveUrl("~/Pages_StockOpname.aspx")
        Else
            url = ResolveUrl("~/Pages_ExprtImprt.aspx")
        End If

        Dim script As String = "window.onload = function(){ alert('" & message & "'); window.location = '" & url & "'; };"
        ClientScript.RegisterStartupScript(Me.GetType(), "Redirect", script, True)
    End Sub

    Sub notsuccesfull()
        messagesuccesfull("File does not exist atau belum dipilih.", False)
    End Sub

    Sub fillplease()
        messagesuccesfull("Silakan pilih file Excel terlebih dahulu.", False)
    End Sub

    Private Function GetCellStringVal(ByVal cel As ICell) As String
        If cel Is Nothing Then Return ""
        Try
            Select Case cel.CellType
                Case CellType.String
                    Return cel.StringCellValue.Trim()
                Case CellType.Numeric
                    Return cel.NumericCellValue.ToString()
                Case CellType.Formula
                    Try
                        Return cel.StringCellValue.Trim()
                    Catch
                        Return cel.NumericCellValue.ToString()
                    End Try
                Case CellType.Boolean
                    Return cel.BooleanCellValue.ToString()
                Case Else
                    Return cel.ToString().Trim()
            End Select
        Catch ex As Exception
            Return ""
        End Try
    End Function

    Private Function GetCellDecimalVal(ByVal cel As ICell) As Decimal
        If cel Is Nothing Then Return 0
        Try
            Select Case cel.CellType
                Case CellType.Numeric
                    Return CDec(cel.NumericCellValue)
                Case CellType.String
                    Dim valStr As String = cel.StringCellValue.Trim().Replace(",", ".")
                    Dim res As Decimal = 0
                    If Decimal.TryParse(valStr, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, res) Then
                        Return res
                    End If
                    Return 0
                Case CellType.Formula
                    Try
                        Return CDec(cel.NumericCellValue)
                    Catch
                        Dim valStr As String = cel.StringCellValue.Trim().Replace(",", ".")
                        Dim res As Decimal = 0
                        If Decimal.TryParse(valStr, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, res) Then
                            Return res
                        End If
                        Return 0
                    End Try
                Case Else
                    Dim valStr As String = cel.ToString().Trim().Replace(",", ".")
                    Dim res As Decimal = 0
                    If Decimal.TryParse(valStr, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, res) Then
                        Return res
                    End If
                    Return 0
            End Select
        Catch ex As Exception
            Return 0
        End Try
    End Function

    Sub Import()
        Dim koder As String = ""

        If Not FileUpLoad1.HasFile Then
            fillplease()
            Exit Sub
        End If

        Dim uploadDir As String = Server.MapPath("~/Uploads/")
        If Not Directory.Exists(uploadDir) Then
            Directory.CreateDirectory(uploadDir)
        End If

        Dim fileName As String = Path.GetFileName(FileUpLoad1.PostedFile.FileName)
        Dim filePath As String = Path.Combine(uploadDir, fileName)
        FileUpLoad1.PostedFile.SaveAs(filePath)

        Dim wb As IWorkbook = Nothing
        Try
            Using fs As New FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
                wb = WorkbookFactory.Create(fs)
            End Using
        Catch ex As Exception
            messagesuccesfull("Gagal membaca file Excel: " & ex.Message, False)
            Exit Sub
        End Try

        If wb Is Nothing Then
            messagesuccesfull("File Excel kosong atau format tidak didukung.", False)
            Exit Sub
        End If

        Dim sheet As ISheet = wb.GetSheet("Sheet1")
        If sheet Is Nothing Then sheet = wb.GetSheet("Sheet 1")
        If sheet Is Nothing AndAlso wb.NumberOfSheets > 0 Then sheet = wb.GetSheetAt(0)

        If sheet Is Nothing Then
            messagesuccesfull("Worksheet Excel tidak ditemukan.", False)
            Exit Sub
        End If

        Dim lastRow As Integer = sheet.LastRowNum
        Dim totalSuccess As Integer = 0
        Dim totalSkipped As Integer = 0
        Dim skippedList As New System.Collections.Generic.List(Of String)
        Dim alreadyTransList As New System.Collections.Generic.List(Of String)

        Dim cInv As New clsInventory
        Dim csStock As New clsInventory_stk
        Dim csOname As New clsInventory_stkOpname

        Dim defaultCCName As String = ""
        If ddlCCName.SelectedItem IsNot Nothing Then
            defaultCCName = ddlCCName.SelectedItem.Text
        End If

        Dim iCustId As Integer = custid()
        Dim sUser As String = username()
        Dim opDate As Date
        If Not Date.TryParse(txtDate.Text, opDate) Then
            opDate = DateTime.Now.Date
        End If

        Try
            For x As Integer = 1 To lastRow
                Dim row As IRow = sheet.GetRow(x)
                If row Is Nothing Then Continue For

                Dim stokNo As String = GetCellStringVal(row.GetCell(0))
                If String.IsNullOrWhiteSpace(stokNo) Then Continue For

                Dim stokName As String = GetCellStringVal(row.GetCell(1))
                Dim unit1 As Decimal = GetCellDecimalVal(row.GetCell(2))
                Dim unitName As String = GetCellStringVal(row.GetCell(3))
                Dim unit2 As Decimal = GetCellDecimalVal(row.GetCell(4))
                Dim unitName2 As String = GetCellStringVal(row.GetCell(5))
                Dim factor1 As Decimal = GetCellDecimalVal(row.GetCell(6))
                Dim cost As Decimal = GetCellDecimalVal(row.GetCell(7))
                Dim name As String = GetCellStringVal(row.GetCell(8))

                If String.IsNullOrWhiteSpace(name) Then
                    name = defaultCCName
                End If

                If unit1 < 0 OrElse unit2 < 0 Then
                    Continue For
                End If

                ' Validasi apakah kode stok terdaftar di database
                Dim dsStk As DataSet = csStock.SelectDataset_StkByNumber(stokNo)
                If dsStk Is Nothing OrElse dsStk.Tables.Count = 0 OrElse dsStk.Tables(0).Rows.Count = 0 Then
                    totalSkipped += 1
                    If skippedList.Count < 5 Then
                        skippedList.Add(stokNo)
                    End If
                    Continue For
                End If

                Dim stkId As Integer = CInt(dsStk.Tables(0).Rows(0).Item("id"))
                Dim ccId As Integer = csOname.get_ccID_ByName(name)
                If ccId = 0 AndAlso Not String.IsNullOrWhiteSpace(defaultCCName) Then
                    name = defaultCCName
                    ccId = csOname.get_ccID_ByName(name)
                End If

                If Not String.IsNullOrWhiteSpace(name) Then
                    Try
                        If cInv.check_belumTrans(stkId, ccId, iCustId) = False AndAlso ddladjustnow.SelectedValue = 1 Then
                            If alreadyTransList.Count < 5 Then
                                alreadyTransList.Add(stokName)
                            End If
                        Else
                            cInv.insertStockOpname_wDate(name, stokNo, stokName, unit1, factor1, unit2, cost, sUser, opDate, ddladjustnow.SelectedValue)
                            totalSuccess += 1
                        End If
                    Catch ex As Exception
                        koder = "Baris " & (x + 1) & " error: " & ex.Message
                    End Try
                End If
            Next

        Catch ex As Exception
            koder = "Error proses Excel: " & ex.Message
        End Try

        GC.Collect()
        GC.WaitForPendingFinalizers()

        Dim finalMsg As String = "Import Selesai: " & totalSuccess & " item berhasil disimpan."
        If totalSkipped > 0 Then
            finalMsg &= "\n" & totalSkipped & " item dilewati karena kode barang tidak terdaftar di Master Stock (" & String.Join(", ", skippedList.ToArray())
            If totalSkipped > skippedList.Count Then finalMsg &= ", dll"
            finalMsg &= ")."
        End If
        If alreadyTransList.Count > 0 Then
            finalMsg &= "\nBeberapa item dilewati karena sudah ada transaksi hari ini (" & String.Join(", ", alreadyTransList.ToArray()) & ")."
        End If
        If Not String.IsNullOrWhiteSpace(koder) Then
            finalMsg &= "\nCatatan: " & koder
        End If

        messagesuccesfull(finalMsg, True)
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancel.Click
        Try

            mfunction.CreateCookie("Ccid", 0)
            mfunction.CreateCookie("invdate", CDate(txtDate.Text))

            Response.Redirect("~/Pages_StockOpname.aspx")

        Catch ex As Exception
            errLog(Err.Description, "Pages_ExportInv_Stk:btnCancel_Click")
            Err.Clear()
        End Try
    End Sub


    Protected Sub btnImport_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnImport.Click
        Try
            If FileUpLoad1.FileName = "" Then
                fillplease()
                Exit Sub
            Else
                Import()
            End If

        Catch ex As Exception
            messagesuccesfull("Error import: " & ex.Message, False)
        End Try
    End Sub


    Protected Sub exportStockData()

        Try

            ' Validasi custid: harus > 0 (aktif) dan tidak null
            Dim iCustId As Integer = custid()
            If iCustId <= 0 Then
                messagesuccesfull("Export gagal: Customer ID tidak valid atau tidak aktif.")
                Exit Sub
            End If

            Dim ds As DataSet = 'clsInventory.SelectDataset_invStkOpnameSearch("", custid, modulepriv, username, ddlCCName.SelectedValue, CDate(txtDate.Text))
            clsInventory.selectdata_invstockopname(iCustId, CDate(Now).ToString("yyyy-MM-dd"), ddlCCName.SelectedItem.Value)
            'WService.get_datatableInvStock(cust) 'WService.getMemberExport(cust)
            Dim dt As DataTable = ds.Tables(0)
            Using wb As New XLWorkbook()
                wb.Worksheets.Add(dt, "Sheet1")
                Response.Clear()
                Response.Buffer = True
                Response.Charset = ""
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                Response.AddHeader("content-disposition", "attachment;filename=" & txtEx.Text & ".xlsx")

                ' Set download token cookie agar JS bisa deteksi selesainya download
                Dim tokenCookie As New HttpCookie("fileDownloadToken", "1")
                tokenCookie.Path = "/"
                Response.Cookies.Add(tokenCookie)

                Using MyMemoryStream As New MemoryStream()
                    wb.SaveAs(MyMemoryStream)
                    MyMemoryStream.WriteTo(Response.OutputStream)

                    HttpContext.Current.Response.Flush()
                    HttpContext.Current.Response.SuppressContent = True
                    HttpContext.Current.ApplicationInstance.CompleteRequest()

                End Using
            End Using

        Catch ex As Exception
            errLog(Err.Description, "Pages_ExportStock:exportStockData")
            Err.Clear()
        End Try

        Exit Sub

    End Sub

    Protected Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        Try
            If txtEx.Text = "" Then
                fillplease()
                txtEx.Focus()
                Exit Sub
            End If

            exportStockData()
        Catch ex As Exception
            errLog(Err.Description, "Pages_ExportInv_Stk:btnExport_Click")
            Err.Clear()
        End Try
    End Sub
End Class