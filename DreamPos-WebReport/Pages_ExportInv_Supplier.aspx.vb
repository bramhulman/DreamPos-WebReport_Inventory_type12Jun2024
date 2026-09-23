Imports System.IO
Imports NPOI.XSSF.UserModel
Imports NPOI.SS.UserModel
Imports System.Net
Imports ClosedXML.Excel

Public Class Pages_ExportInv_Supplier
    Inherits System.Web.UI.Page
    Private sNameuser As String
    Private Userid As Integer
    Private icustid As Integer
    Private imodul As Integer
    Private iModPriv As Integer
    Private irptpriv As Integer
    Private irptf As Integer
    Private mfunction As New ModFunction
    Private CStk As New Cls_IStk
    Private Csupp2 As New clsInventory_Supplier


    'Private transpriv As Integer
    Public Function modpriv() As Integer
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
        Else

            If modpriv() And usrModulepriv.moduleuser = usrModulepriv.moduleuser Then
                btnImport.Visible = False

            End If
        End If
    End Sub

    Protected Sub Page_unLoad(sender As Object, e As EventArgs) Handles Me.Unload
    End Sub

    Public Function username() As String
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        Return cookie("userName")

    End Function

    Sub Export()

        Try
            'server path
            Dim strPathServer As String = "C:\" & txtEx.Text & " Server.xlsx"

            'webclient Path
            Dim strPath As String = "C:\" & txtEx.Text & ".xlsx"

            Dim fileReader As New WebClient()

            'jadi cek di local webclient, klo ada ga jadi
            If (System.IO.File.Exists(strPathServer)) Then
                System.IO.File.Delete(strPathServer)
            End If

            Dim ExcApp As IWorkbook = New XSSFWorkbook
            Dim sheet = ExcApp.CreateSheet("Sheet1")
            Dim row As IRow
            Dim intBaris As Integer = 0

            Dim dsData As New DataSet
            Dim ds As New DataSet

            If custid() = 0 Then
            Else
                ds = CStk.SelectDataset_Stk_Export(custid)
            End If

            dsData = ds

            row = sheet.CreateRow(intBaris)

            row.CreateCell(0).SetCellValue("Number")
            row.CreateCell(1).SetCellValue("Name")
            row.CreateCell(2).SetCellValue("Group Code")

            row.CreateCell(3).SetCellValue("unit1")
            row.CreateCell(4).SetCellValue("Unit2")
            row.CreateCell(5).SetCellValue("Unit3")

            row.CreateCell(6).SetCellValue("Factor1")
            row.CreateCell(6).SetCellValue("Factor2")

            row.CreateCell(6).SetCellValue("stkLimit")

            intBaris += 1

            If dsData.Tables(0).Rows.Count > 0 Then

                For Each ro As DataRow In dsData.Tables(0).Rows

                    row = sheet.CreateRow(intBaris)

                    row.CreateCell(0).SetCellValue(ro("Number").ToString)
                    row.CreateCell(1).SetCellValue(ro("Name").ToString)
                    row.CreateCell(2).SetCellValue(ro("grpcode").ToString)

                    row.CreateCell(3).SetCellValue(ro("Unit1").ToString)
                    row.CreateCell(4).SetCellValue(ro("Unit2").ToString)
                    row.CreateCell(4).SetCellValue(ro("Unit3").ToString)

                    row.CreateCell(5).SetCellValue(ro("Factor1").ToString)
                    row.CreateCell(5).SetCellValue(ro("Factor2").ToString)

                    row.CreateCell(5).SetCellValue(ro("stkLimit").ToString)

                    intBaris += 1
                Next
            End If

            Dim sw As FileStream = File.Create(strPathServer)
            ExcApp.Write(sw)
            sw.Close()
            sw.Dispose()

            fileReader.DownloadFile(strPathServer, strPath)
            'abis dowload di delete
            If (System.IO.File.Exists(strPathServer)) Then
                System.IO.File.Delete(strPathServer)
            End If

            FailureText.Text() = "Export Stock Succesfull Please check in " & strPath
        Catch ex As Exception
            errLog(Err.Description, "Pages_ExportInv_Stk:Export")
            Err.Clear()
        End Try

    End Sub

    Sub messagesuccesfull(ByVal msg As String, Optional ByVal bsuccess As Boolean = False)

        Dim message As String = msg '"Import From Excel Successfully."
        Dim url As String = ""

        If bsuccess Then
            url = "/Pages_Supplier.aspx"
        Else
            url = "/Pages_ExportInv_Supplier.aspx"
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
        Dim url As String = "~/Pages_Supplier.aspx"
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
        Dim url As String = "~/Pages_Supplier.aspx"
        sb.Append("<script type = 'text/javascript'>")
        sb.Append("window.onload=function(){")
        sb.Append("alert('")
        sb.Append(message)
        sb.Append("')};")
        sb.Append("</script>")
        ClientScript.RegisterClientScriptBlock(Me.GetType(), "Alert", sb.ToString())
    End Sub
    Sub Import()

        Try
            Dim koder As String = ""

            If FileUpLoad1.HasFile Then

                Dim Name As String = ""
                Dim Email As String = ""
                Dim Address As String = ""
                Dim Phone As String = ""
                Dim City As String = ""
                Dim State As String = ""
                Dim Pcode As String = ""
                Dim Notes As String = ""

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

                            Name = ""
                            Email = ""
                            Address = ""
                            Phone = ""
                            City = ""
                            State = ""
                            Pcode = ""
                            Notes = ""

                            cel = row.GetCell(2)
                            If cel IsNot Nothing Then
                                If cel.CellType = CellType.STRING Then
                                    Name = cel.StringCellValue
                                Else
                                    Name = cel.NumericCellValue
                                End If
                            End If

                            cel = row.GetCell(3)
                            If cel IsNot Nothing Then
                                If cel.CellType = CellType.STRING Then
                                    Email = cel.StringCellValue
                                Else
                                    Email = cel.NumericCellValue
                                End If
                            End If

                            cel = row.GetCell(4)
                            If cel IsNot Nothing Then
                                If cel.CellType = CellType.STRING Then
                                    Address = cel.StringCellValue
                                Else
                                    Address = cel.NumericCellValue
                                End If
                            End If

                            cel = row.GetCell(5)
                            If cel IsNot Nothing Then
                                If cel.CellType = CellType.STRING Then
                                    Phone = cel.StringCellValue
                                Else
                                    Phone = cel.NumericCellValue
                                End If
                            End If

                            cel = row.GetCell(6)
                            If cel IsNot Nothing Then
                                If cel.CellType = CellType.STRING Then
                                    City = cel.StringCellValue
                                Else
                                    City = cel.NumericCellValue
                                End If
                            End If

                            cel = row.GetCell(7)
                            If cel IsNot Nothing Then
                                If cel.CellType = CellType.STRING Then
                                    State = cel.StringCellValue
                                Else
                                    State = cel.NumericCellValue
                                End If
                            End If

                            cel = row.GetCell(8)
                            If cel IsNot Nothing Then
                                If cel.CellType = CellType.STRING Then
                                    Pcode = cel.StringCellValue
                                Else
                                    Pcode = cel.NumericCellValue
                                End If
                            End If
                            cel = row.GetCell(9)
                            If cel IsNot Nothing Then
                                If cel.CellType = CellType.STRING Then
                                    Notes = cel.StringCellValue
                                Else
                                    Notes = cel.NumericCellValue
                                End If
                            End If

                            If Name <> "" Then
                                Try

                                    'WService.insertSupplier(0, cust(), Name, Email, _
                                    '                      Address, Phone, City, State, _
                                    '                     Pcode, "", username())



                                    Dim clsSupplier As New clsInventory_Supplier
                                    clsSupplier.insertSupplier(0, custid(), Name, Email,
                                                      Address, Phone, City, State,
                                                     Pcode, Notes, username(), 1)


                                    clsSupplier = Nothing




                                Catch ex As Exception

                                    koder = "Error insert in Row " & lines & " " & Err.Description
                                    Err.Clear()

                                End Try

                            End If


                        End If

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
                messagesuccesfull(koder)
            Else
                messagesuccesfull("Import From Excel Successfully", True)
            End If
        Catch ex As Exception
            errLog(Err.Description, "Pages_ExportInv_Stk:Import")
            Err.Clear()
        End Try

    End Sub
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancel.Click
        Try
            'mfunction.CreateCookie("InvSup_ID", 0)

            Response.Redirect("~/Pages_Supplier.aspx", False)

        Catch ex As Exception
            errLog(Err.Description, "Pages_ExportInv_Supplier:btnCancel_Click")
            Err.Clear()
        End Try
    End Sub


    Protected Sub btnImport_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnImport.Click
        Import()
    End Sub


    Protected Sub exportSupplierData()

        Try
            Dim clsSupplier As New clsInventory_Supplier
            Dim dt As DataTable = clsSupplier.SelectDataset_SupplierbyUser(custid, username, modpriv).Tables(0)  'WService.getMemberExport(cust)
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
            errLog(Err.Description, "Pages_ExportSupplier:exportSupplierData")
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

            exportSupplierData()

        Catch ex As Exception
            errLog(Err.Description, "Pages_ExportInv_Supplier:btnExport_Click")
            Err.Clear()
        End Try
    End Sub
End Class