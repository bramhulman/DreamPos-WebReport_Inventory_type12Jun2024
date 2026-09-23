Imports System.IO
Imports NPOI.XSSF.UserModel
Imports NPOI.SS.UserModel
Imports System.Net
Imports ClosedXML.Excel
Imports DreamPos_WebReport.My

Public Class Pages_ExportInv_Stk
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



    'Private transpriv As Integer
    Public Function modulepriv() As Integer
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        If cookie Is Nothing Then
            Return -1
        Else
            Return cookie("modulepriv")
        End If

    End Function
    Public Function custid() As Integer
        Dim icust As Integer = 0
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")

        If cookie Is Nothing Then
            Response.Redirect("~/Default.aspx", False)

        Else
            icust = cookie("custid")
        End If
        Return icust
    End Function

    Public Function rptpriv() As Integer
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")


        If cookie Is Nothing Then
            Return -1
        Else
            Return cookie("rptpriv")
        End If
    End Function
    Public Function modul() As Integer
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")

        If cookie Is Nothing Then
            Return -1
        Else
            Return cookie("modul")
        End If
    End Function
    Public Function rptf() As Integer
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")

        If cookie Is Nothing Then
            Return -1
        Else
            Return cookie("rptf")
        End If

    End Function
    Public Function InvPriv() As Integer
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")

        If cookie Is Nothing Then
            Return -1
        Else
            Return cookie("invpriv")
        End If
    End Function
    'Public Function ModPrv() As Integer
    '    Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")


    '    If cookie Is Nothing Then
    '        Return -1
    '    Else
    '        Return cookie("modulepriv")
    '    End If
    'End Function
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
        If modul() = usrModul.moduleInv Then
            Dim userInvPriv As Integer = InvPriv()





            If userInvPriv < 8255 Then '524287 Then

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
            row.CreateCell(7).SetCellValue("Factor2")

            row.CreateCell(8).SetCellValue("stkLimit")
            row.CreateCell(9).SetCellValue("ccname")
            intBaris += 1

            If dsData.Tables(0).Rows.Count > 0 Then

                For Each ro As DataRow In dsData.Tables(0).Rows

                    row = sheet.CreateRow(intBaris)

                    row.CreateCell(0).SetCellValue(ro("Number").ToString)
                    row.CreateCell(1).SetCellValue(ro("Name").ToString)
                    row.CreateCell(2).SetCellValue(ro("grpcode").ToString)

                    row.CreateCell(3).SetCellValue(ro("Unit1").ToString)
                    row.CreateCell(4).SetCellValue(ro("Unit2").ToString)
                    row.CreateCell(5).SetCellValue(ro("Unit3").ToString)

                    row.CreateCell(6).SetCellValue(ro("Factor1").ToString)
                    row.CreateCell(7).SetCellValue(ro("Factor2").ToString)

                    row.CreateCell(8).SetCellValue(ro("stkLimit").ToString)
                    row.CreateCell(9).SetCellValue(ro("ccname").ToString)
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
            url = "/Pages_Stk.aspx"
            'Else
            '    url = "/Pages_ExportInv_Stk.aspx"
        End If

        Dim script As String = "window.onload = function(){ alert('"
        script += message
        script += "');"
        script += "window.location = '"
        script += url
        script += "'; }"
        ClientScript.RegisterStartupScript(Me.GetType(), "Redirect", script, True)

    End Sub
    'Sub notsuccesfull()
    '    Dim message As String = "File Does not exist"
    '    Dim sb As New System.Text.StringBuilder()
    '    Dim url As String = "~/Pages_Stk.aspx"
    '    sb.Append("<script type = 'text/javascript'>")
    '    sb.Append("window.onload=function(){")
    '    sb.Append("alert('")
    '    sb.Append(message)
    '    sb.Append("')};")
    '    sb.Append("</script>")
    '    ClientScript.RegisterClientScriptBlock(Me.GetType(), "Alert", sb.ToString())
    'End Sub
    Sub notsuccesfull(ByVal msg As String)
        Dim message As String = msg
        Dim sb As New System.Text.StringBuilder()
        Dim url As String = "~/Pages_Stk.aspx"
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
        Dim url As String = "~/Pages_Stk.aspx"
        sb.Append("<script type = 'text/javascript'>")
        sb.Append("window.onload=function(){")
        sb.Append("alert('")
        sb.Append(message)
        sb.Append("')};")
        sb.Append("</script>")
        ClientScript.RegisterClientScriptBlock(Me.GetType(), "Alert", sb.ToString())
    End Sub
    Protected Sub Import()

        Try
            Dim koder As String = ""

            Dim c_cmd As New clsDbo("")

            If c_cmd.ExecuteScalar("Select count(ID) from cld_inv_stkgroup", 0) = 0 Then
                notsuccesfull("Input Stock Group First!")
                Exit Sub
            End If
            If FileUpLoad1.HasFile Then

                Dim Number As String = ""
                Dim Name As String = ""
                Dim grpcode As String = ""

                Dim unit1 As String = ""
                Dim unit2 As String = ""
                Dim unit3 As String = ""

                Dim factor1 As Decimal = 0
                Dim factor2 As Decimal = 0

                Dim stkLimit As Decimal = 0
                Dim stkccid As Integer = 0

                'Dim price1 As Decimal = 0
                'Dim price2 As Decimal = 0
                Dim cost As Decimal = 0

                Dim fileName As String = System.IO.Path.GetFileName(FileUpLoad1.PostedFile.FileName)
                FileUpLoad1.PostedFile.SaveAs(Server.MapPath(("~/Uploads/" + fileName)))
                Dim Extension As String = Path.GetExtension(FileUpLoad1.PostedFile.FileName)
                Dim FolderPath As String = Server.MapPath(FileUpLoad1.FileName)


                Dim wb As IWorkbook = WorkbookFactory.Create(New FileStream(Server.MapPath(("~/Uploads/" + fileName)), FileMode.Open, FileAccess.Read))
                Dim sheet As ISheet = wb.GetSheet("Sheet1")
                Dim row As IRow
                Dim cel As ICell
                Dim newLine As String = "\n"

                'ProgressBar1.Visible = True
                Dim x As Integer
                Dim ddd As Integer = sheet.LastRowNum

                x = 1

                Dim lines As Integer = x

                Try

                    Do While sheet.GetRow(x) IsNot Nothing

                        row = sheet.GetRow(x)
                        If row IsNot Nothing Then

                            Number = ""
                            Name = ""
                            grpcode = ""

                            unit1 = ""
                            unit2 = ""
                            unit3 = ""

                            factor1 = 0
                            factor2 = 0

                            stkLimit = 0
                            stkccid = 0

                            'price1 = 0
                            'price2 = 0
                            cost = 0

                            cel = row.GetCell(0)
                            If cel IsNot Nothing Then
                                If cel.CellType = CellType.STRING Then
                                    Number = cel.StringCellValue
                                Else
                                    Number = cel.NumericCellValue
                                End If
                                'Dim c_cmd As New clsDbo("")

                                'If c_cmd.ExecuteScalar("Select ID from cld_inv_stk where Number = '" & Number & "'", 2) > 0 Then
                                '    koder = koder + " number '" & Number & "' already exist in Row " & lines & " " & newLine
                                'End If
                            End If

                            cel = row.GetCell(1)
                            If cel IsNot Nothing Then
                                If cel.CellType = CellType.STRING Then
                                    Name = Left(cel.StringCellValue, 30)
                                Else
                                    Name = Left(cel.NumericCellValue, 30)
                                End If
                            End If

                            cel = row.GetCell(2)
                            If cel IsNot Nothing Then
                                If cel.CellType = CellType.STRING Then
                                    grpcode = cel.StringCellValue
                                Else
                                    grpcode = cel.NumericCellValue
                                End If
                            End If

                            cel = row.GetCell(3)
                            If cel IsNot Nothing Then
                                If cel.CellType = CellType.STRING Then
                                    unit1 = cel.StringCellValue
                                Else
                                    unit1 = cel.NumericCellValue
                                End If
                            End If

                            cel = row.GetCell(4)
                            If cel IsNot Nothing Then
                                If cel.CellType = CellType.STRING Then
                                    unit2 = cel.StringCellValue
                                Else
                                    unit2 = cel.NumericCellValue
                                End If
                            End If

                            cel = row.GetCell(5)
                            If cel IsNot Nothing Then
                                If cel.CellType = CellType.STRING Then
                                    unit3 = cel.StringCellValue
                                Else
                                    unit3 = cel.NumericCellValue
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
                                    factor2 = 0
                                Else
                                    factor2 = cel.NumericCellValue
                                End If
                            End If

                            'cel = row.GetCell(8)
                            'If cel IsNot Nothing Then
                            '    If cel.CellType = CellType.STRING Then
                            '        stkLimit = 0
                            '    Else
                            '        stkLimit = cel.NumericCellValue
                            '    End If
                            'End If

                            'cel = row.GetCell(9)
                            'If cel IsNot Nothing Then
                            '    Dim ccname As String = ""
                            '    Dim c_cmd As New clsDbo("")
                            '    If cel.CellType = CellType.STRING Then
                            '        ccname = cel.StringCellValue
                            '    Else
                            '        ccname = cel.NumericCellValue
                            '    End If
                            '    stkccid = c_cmd.ExecuteScalar("Select ID from cld_inv_cc where name = '" & ccname & "'", 2)
                            'End If
                            'If My.Settings.fixcost <> 0 Then
                            '    cel = row.GetCell(8)
                            '    If cel IsNot Nothing Then
                            '        If cel.CellType = CellType.STRING Then
                            '            cost = 0
                            '        Else
                            '            cost = cel.NumericCellValue
                            '        End If
                            '    End If
                            'End If


                            'cel = row.GetCell(8)
                            'If cel IsNot Nothing Then
                            '    If cel.CellType = CellType.STRING Then
                            '        price1 = 0
                            '    Else
                            '        price1 = cel.NumericCellValue
                            '    End If
                            'End If
                            'cel = row.GetCell(9)
                            'If cel IsNot Nothing Then
                            '    If cel.CellType = CellType.STRING Then
                            '        price2 = 0
                            '    Else
                            '        price2 = cel.NumericCellValue
                            '    End If
                            'End If
                            If Name <> "" Then
                                Try



                                    Dim cStock As New clsInventory_stk
                                    'cStock.insertStk(0, custid(), Number, Name,
                                    '                      grpcode, unit1, unit2, unit3,
                                    '                    factor1, factor2, stkLimit, False, 0, username(), stkccid)

                                    If (factor1 = 0 Or factor2 = 0) Then
                                        If factor1 = 0 Then
                                            koder = koder + " Fail insert in Row " & Name & " factor1 = 0 " & newLine
                                        End If
                                        If factor2 = 0 Then
                                            koder = koder + " Fail insert in Row " & Name & " factor2 = 0 " & newLine
                                        End If
                                    Else
                                        cStock.insertStk_imp(0, custid(), Number, Name,
                                                          grpcode, unit1, unit2, unit3,
                                                        factor1, factor2, False, 0, "0", "0", username(), cost)
                                    End If

                                    'WService.insertStk(0, cust(), Number, Name, _
                                    '                      grpcode, unit1, unit2, unit3, _
                                    '                     factor1, factor2, stkLimit, False, 0, username())
                                Catch ex As Exception

                                    koder = koder + " Error insert in Row " & lines & " " & Err.Description & " " & newLine
                                    Err.Clear()

                                End Try

                            End If


                        End If

                        x += 1
                        lines = x
                    Loop

                Catch ex As Exception
                    koder = koder + " Error in Row " & lines & " " & Err.Description & " " & newLine

                    Err.Clear()

                End Try

                GC.Collect()
                GC.WaitForPendingFinalizers()

            Else
                notsuccesfull(koder)
                Exit Sub
            End If

            If koder <> "" Then
                'messagesuccesfull(koder, True)
                notsuccesfull(koder)
                Exit Sub
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
            mfunction.CreateCookie("InvStk_ID", 0)

            Response.Redirect("~/Pages_Stk.aspx", False)

        Catch ex As Exception
            errLog(Err.Description, "Pages_ExportInv_Stk:btnCancel_Click")
            Err.Clear()
        End Try
    End Sub


    Protected Sub btnImport_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnImport.Click
        Try

            If FileUpLoad1.HasFile Then
                btnImport.UseSubmitBehavior = True
            Else
                notsuccesfull("select excel file!")
                Exit Sub
            End If

            Import()
        Catch ex As Exception
            errLog(Err.Description, "Pages_ExportInv_Stk:btnImport_Click")
            Err.Clear()
        End Try

    End Sub


    Protected Sub exportStockData()

        Try

            Dim dt As DataTable = CStk.SelectDataTable_Stk_Export(custid, username, modulepriv) 'WService.getMemberExport(cust)
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
            errLog(Err.Description, "Pages_ExportInv_Stk:exportStockData")
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