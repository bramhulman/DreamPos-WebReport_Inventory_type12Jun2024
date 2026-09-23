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

        Dim message As String = msg '"Import From Excel Successfully."
        Dim url As String = ""

        If bsuccess Then
            url = "/Pages_StockOpname.aspx"
        Else
            url = "/Pages_ExprtImprt.aspx"
        End If

        Dim script As String = "window.onload = function(){ alert('"
        script += message
        script += "');"
        script += "window.location = '"
        script += url
        script += "'; }"
        ClientScript.RegisterStartupScript(Me.GetType(), "Redirect", script, True)

    End Sub
    Sub notsuccesfull()
        Dim message As String = "File Does not exist"
        Dim sb As New System.Text.StringBuilder()
        Dim url As String = "~/Pages_ExprtImprt.aspx"
        sb.Append("<script type = 'text/javascript'>")
        sb.Append("window.onload=function(){")
        sb.Append("alert('")
        sb.Append(message)
        sb.Append("')};")
        sb.Append("</script>")
        ClientScript.RegisterClientScriptBlock(Me.GetType(), "Alert", sb.ToString())
    End Sub

    Sub fillplease()
        Dim message As String = "Fill Please File Name"
        Dim sb As New System.Text.StringBuilder()
        Dim url As String = "~/Pages_ExprtImprt.aspx"
        sb.Append("<script type = 'text/javascript'>")
        sb.Append("window.onload=function(){")
        sb.Append("alert('")
        sb.Append(message)
        sb.Append("')};")
        sb.Append("</script>")
        ClientScript.RegisterClientScriptBlock(Me.GetType(), "Alert", sb.ToString())
    End Sub

    Sub Import()

        Dim koder As String = ""

        If FileUpLoad1.HasFile Then

            Dim stokNo As String = ""
            Dim stokName As String = ""
            Dim unit1 As Decimal = 0

            Dim unitName As String = ""
            Dim unit2 As Decimal = 0
            Dim unitName2 As String = ""

            Dim factor1 As Decimal = 0
            Dim cost As Decimal = 0

            Dim name As String = ""

            Dim bClose As Integer

            Dim fileName As String = System.IO.Path.GetFileName(FileUpLoad1.PostedFile.FileName)
            FileUpLoad1.PostedFile.SaveAs(Server.MapPath(("~/Uploads/" + fileName)))
            Dim Extension As String = Path.GetExtension(FileUpLoad1.PostedFile.FileName)
            Dim FolderPath As String = Server.MapPath(FileUpLoad1.FileName)


            Dim wb As IWorkbook = WorkbookFactory.Create(New FileStream(Server.MapPath(("~/Uploads/" + fileName)), FileMode.Open, FileAccess.Read))
            Dim sheet As ISheet = wb.GetSheet("Sheet1")
            Dim row As IRow
            Dim cel As ICell

            'ProgressBar1.Visible = True
            Dim x As Integer
            Dim ddd As Integer = sheet.LastRowNum

            x = 1

            Dim lines As Integer = x

            Try

                Do While sheet.GetRow(x) IsNot Nothing

                    row = sheet.GetRow(x)
                    If row IsNot Nothing Then

                        stokNo = ""
                        stokName = ""
                        unit1 = 0

                        unitName = ""
                        unit2 = 0
                        unitName2 = ""

                        factor1 = 0
                        cost = 0

                        name = ""

                        bClose = 0

                        cel = row.GetCell(0)
                        If cel IsNot Nothing Then
                            If cel.CellType = CellType.STRING Then
                                stokNo = cel.StringCellValue
                            Else
                                stokNo = cel.NumericCellValue
                            End If
                        End If

                        cel = row.GetCell(1)
                        If cel IsNot Nothing Then
                            If cel.CellType = CellType.STRING Then
                                stokName = cel.StringCellValue
                            Else
                                stokName = cel.NumericCellValue
                            End If
                        End If

                        cel = row.GetCell(2)
                        If cel IsNot Nothing Then
                            If cel.CellType = CellType.STRING Then
                                unit1 = cel.StringCellValue
                            Else
                                'If IsNumeric(cel.NumericCellValue) = True Then
                                unit1 = cel.NumericCellValue
                                    'End If

                                End If
                        End If

                        cel = row.GetCell(3)
                        If cel IsNot Nothing Then
                            If cel.CellType = CellType.STRING Then
                                unitName = cel.StringCellValue
                            Else
                                unitName = cel.NumericCellValue
                            End If
                        End If

                        cel = row.GetCell(4)
                        If cel IsNot Nothing Then
                            If cel.CellType = CellType.STRING Then
                                unit2 = cel.StringCellValue
                            Else
                                Try
                                    'If IsNumeric(cel.NumericCellValue) = True Then
                                    unit2 = cel.NumericCellValue
                                    'End If
                                Catch ex As Exception
                                    GoTo loop1
                                End Try


                            End If
                        End If

                        If unit1 < 0 Or unit2 < 0 Then
                            GoTo loop1
                        End If

                        cel = row.GetCell(5)
                        If cel IsNot Nothing Then
                            If cel.CellType = CellType.STRING Then
                                unitName2 = cel.StringCellValue
                            Else
                                unitName2 = cel.NumericCellValue
                            End If
                        End If

                        cel = row.GetCell(6)
                        If cel IsNot Nothing Then
                            If cel.CellType = CellType.STRING Then
                                factor1 = 0
                            Else
                                factor1 = cel.NumericCellValue
                            End If
                        End If

                        cel = row.GetCell(7)
                        If cel IsNot Nothing Then
                            If cel.CellType = CellType.STRING Then
                                cost = 0
                            Else
                                cost = cel.NumericCellValue
                            End If
                        End If

                        cel = row.GetCell(8)
                        If cel IsNot Nothing Then
                            If cel.CellType = CellType.STRING Then
                                name = cel.StringCellValue
                            Else
                                name = cel.NumericCellValue
                            End If
                        End If

                        'cel = row.GetCell(9)
                        'If cel IsNot Nothing Then
                        '    If cel.CellType = CellType.STRING Then
                        '        bClose = 0
                        '    Else
                        '        bClose = cel.NumericCellValue
                        '    End If
                        'End If
                        bClose = 1

                        Dim cInv As New clsInventory
                        If name <> "" And bClose <> 0 Then
                            Try
                                Dim csOname As New clsInventory_stkOpname
                                Dim csStock As New clsInventory_stk

                                If cInv.check_belumTrans(csStock.SelectDataset_StkByNumber(stokNo).Tables(0).Rows(0).Item("id"), csOname.get_ccID_ByName(name), custid) = False And ddladjustnow.SelectedValue = 1 Then
                                    koder = koder + stokName + " today, already have transaction" & "\n"
                                    'Exit Sub
                                Else
                                    cInv.insertStockOpname_wDate(name, stokNo, stokName, unit1, factor1, unit2, cost, username, CDate(txtDate.Text), ddladjustnow.SelectedValue)
                                End If


                                'WService.insertStk(0, cust(), Number, Name, _
                                '                      grpcode, unit1, unit2, unit3, _
                                '                     factor1, factor2, stkLimit, False, 0, username())
                            Catch ex As Exception

                                koder = "Error insert in Row " & lines & " " & Err.Description
                                Err.Clear()

                            End Try
                        ElseIf name <> "" And bClose = 0 Then 'BClose 0
                            Try
                                cInv.DeleteInvTrans_ByBClose(custid, username, CDate(txtDate.Text), stokNo, ddlCCName.SelectedItem.ToString)

                            Catch ex As Exception

                                koder = "Error insert in Row " & lines & " " & Err.Description
                                Err.Clear()
                            End Try
                        End If


                    End If
loop1:
                    x += 1
                    lines = x
                Loop

            Catch ex As Exception
                koder = "Error in Row " & lines & " " & Err.Description

                Err.Clear()

            End Try

            GC.Collect()
            GC.WaitForPendingFinalizers()

        Else
            notsuccesfull()
        End If

        If koder <> "" Then
            messagesuccesfull(koder, True)
        Else
            messagesuccesfull("Import From Excel Successfully", True)
        End If

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
                'txtDate.Focus()
                Exit Sub
            Else
                Import()
            End If

        Catch ex As Exception

        End Try
    End Sub


    Protected Sub exportStockData()

        Try


            Dim ds As DataSet = 'clsInventory.SelectDataset_invStkOpnameSearch("", custid, modulepriv, username, ddlCCName.SelectedValue, CDate(txtDate.Text))
            clsInventory.selectdata_invstockopname(custid, CDate(Now).ToString("yyyy-MM-dd"), ddlCCName.SelectedItem.Value)
            'WService.get_datatableInvStock(cust) 'WService.getMemberExport(cust)
            Dim dt As DataTable = ds.Tables(0)
            Using wb As New XLWorkbook()
                wb.Worksheets.Add(dt, "Sheet1")
                Response.Clear()
                Response.Buffer = True
                Response.Charset = ""
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                Response.AddHeader("content-disposition", "attachment;filename=" & txtEx.Text & ".xlsx")
                Using MyMemoryStream As New MemoryStream()
                    wb.SaveAs(MyMemoryStream)
                    MyMemoryStream.WriteTo(Response.OutputStream)
                    'Response.Flush()
                    'Response.End()

                    HttpContext.Current.Response.Flush()
                    HttpContext.Current.Response.SuppressContent = True
                    HttpContext.Current.ApplicationInstance.CompleteRequest()


                End Using
            End Using


        Catch ex As Exception
            errLog(Err.Description, "Pages_ExportStock:exportStockData")
            Err.Clear()
        Finally
            HttpContext.Current.Response.SuppressContent = True
            HttpContext.Current.ApplicationInstance.CompleteRequest()

            messagesuccesfull("Export Data Successfully")

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