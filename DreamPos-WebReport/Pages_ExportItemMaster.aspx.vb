Imports System.Threading
Imports System.Net
Imports System.Text
Imports System.IO

Imports NPOI.XSSF.UserModel
Imports NPOI.SS.UserModel

Imports ClosedXML.Excel

Public Class Pages_ExportItemMaster
    Inherits System.Web.UI.Page
    Private sNameuser As String
    Private Userid As Integer
    Private icustid As Integer
    Private imodul As Integer
    Private iModPriv As Integer
    Private irptpriv As Integer
    Private irptf As Integer
    Private mfunction As New ModFunction
    Private Mdd As String = ""
    Private MTid As Integer
    Private CStkGrp As New Cls_IStkGroup
    Private accItem As New clsDataItem


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
        If cookie Is Nothing Then


            Dim script As String = "window.onload = function(){ alert('"
            script += "Please Login"
            script += "');"
            script += "window.location = '"
            script += "../Default"
            script += " '; }"
            ClientScript.RegisterStartupScript(Me.GetType(), "Redirect", script, True)
        End If

    End Sub


    Protected Sub Page_unLoad(sender As Object, e As EventArgs) Handles Me.Unload
    End Sub

    Public Function username() As String
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        Return cookie("userName")

    End Function

    Sub ExportgiftData()
        Try

            Dim dt As DataTable = accItem.SelectDataset_ItemMasterEx2(custid)
            Dim csv As String = String.Empty

            For Each column As DataColumn In dt.Columns
                'Add the Header row for CSV file.
                csv += column.ColumnName + ","c
            Next

            'Add new line.
            csv += vbCr & vbLf

            For Each row As DataRow In dt.Rows
                For Each column As DataColumn In dt.Columns
                    'Add the Data rows.
                    csv += row(column.ColumnName).ToString().Replace(",", ";") + ","c
                Next

                'Add new line.
                csv += vbCr & vbLf
            Next

            'Download the CSV file.
            Response.Clear()
            Response.Buffer = True
            Response.AddHeader("content-disposition", "attachment;filename=" & txtEx.Text & ".csv")
            Response.Charset = ""
            Response.ContentType = "application/text"
            Response.Output.Write(csv)
            Response.Flush()
            Response.End()

        Catch ex As Exception
            errLog(Err.Description, "Pages_ExportItemMaster:ExportgiftData")
            Err.Clear()
        Finally

            HttpContext.Current.Response.SuppressContent = True
            HttpContext.Current.ApplicationInstance.CompleteRequest()

            ' Response.End()
            '  FailureText.Text() = "Export member Succesfull"
            messagesuccesfull("Export Data Successfully")

        End Try

        Exit Sub


    End Sub
    Sub messagesuccesfull(ByVal msg As String, Optional ByVal bsuccess As Boolean = False)


        Dim message As String = msg
        Dim url As String = ""


        If bsuccess Then
            url = "/Pages_ItemMaster.aspx"
        Else
            url = "/Pages_ExportItemMaster.aspx"
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
        Dim url As String = "~/Pages_ItemMaster.aspx"
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
        Dim url As String = "~/Pages_ItemMaster.aspx"
        sb.Append("<script type = 'text/javascript'>")
        sb.Append("window.onload=function(){")
        sb.Append("alert('")
        sb.Append(message)
        sb.Append("')};")
        sb.Append("</script>")
        ClientScript.RegisterClientScriptBlock(Me.GetType(), "Alert", sb.ToString())
    End Sub
    Sub SuccesExport()
        Dim message As String = "Succes Export File " & txtEx.Text & " to Excel In Directory C:\"
        Dim sb As New System.Text.StringBuilder()
        Dim url As String = "~/Pages_ItemMaster.aspx"
        sb.Append("<script type = 'text/javascript'>")
        sb.Append("window.onload=function(){")
        sb.Append("alert('")
        sb.Append(message)
        sb.Append("')};")
        sb.Append("</script>")
        ClientScript.RegisterClientScriptBlock(Me.GetType(), "Alert", sb.ToString())
    End Sub
    '    Sub Import()
    '        Dim msgerr As String = ""
    '        Try
    '            If FileUpLoad1.HasFile Then

    '                Dim fileName As String = System.IO.Path.GetFileName(FileUpLoad1.PostedFile.FileName)
    '                FileUpLoad1.PostedFile.SaveAs(Server.MapPath(("~/Uploads/" + fileName)))
    '                Dim csvData As String = File.ReadAllText(Server.MapPath(("~/Uploads/" + fileName)))


    '                Dim x As Integer = 0

    '                For Each row As String In csvData.Split(ControlChars.Lf)
    '                    If Not String.IsNullOrEmpty(row) Then
    '                        'If row.ToString.ToUpper = "itemNo,ItemName,price," & vbCr & "".ToString.ToUpper Then
    '                        '    GoTo Calculate
    '                        'End If

    '                        If x = 0 Then
    '                            GoTo Calculate
    '                        End If

    '                        Dim itemno As String = row.Split(",")(0)
    '                        Dim membername As String = row.Split(",")(1)
    '                        Dim price As Decimal = 0
    '                        If row.Split(",")(2) = "" Then
    '                            price = 0
    '                        Else
    '                            price = row.Split(",")(2)
    '                        End If
    '                        Dim ccname As String = row.Split(",")(3)
    '                        Dim ccid As Integer = accItem.selectccid(ccname)

    '                        insertItemMasterEx(itemno, membername, price, custid, ccid)

    '                    End If
    'Calculate:
    '                    x = x + 1
    '                Next

    '            Else
    '                notsuccesfull()
    '            End If
    '            If msgerr <> "" Then
    '                messagesuccesfull("Import From Excel Successfully But Error :" & msgerr)
    '            Else
    '                messagesuccesfull("Import From Excel Successfully")
    '            End If
    '        Catch ex As Exception
    '            errLog(Err.Description, "Pages_ExportItemMaster:Import")
    '            Err.Clear()
    '        End Try


    '        Exit Sub

    '    End Sub

    Sub Import()

        Dim koder As String = ""

        If FileUpLoad1.HasFile Then
            Dim itemno As String = ""
            Dim itemname As String = ""
            Dim majorgroup As String = ""
            Dim itemgroup As String = ""
            Dim ccname As String = ""
            'Dim price As Decimal = 0
            Dim itemgroupid As Integer = 0
            Dim majorgroupid As Integer = 0
            Dim ccid As Integer = 0
            Dim xusage As Integer = 0




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
                    If x = 0 Then
                        GoTo Calculate
                    End If
                    row = sheet.GetRow(x)
                    If row IsNot Nothing Then

                        itemno = ""
                        membername = ""
                        price = 0

                        cel = row.GetCell(0)
                        If cel IsNot Nothing Then
                            If cel.CellType = CellType.STRING Then
                                itemno = cel.StringCellValue
                            Else
                                itemno = cel.NumericCellValue
                            End If
                        End If

                        cel = row.GetCell(1)
                        If cel IsNot Nothing Then
                            If cel.CellType = CellType.STRING Then
                                itemname = Left(cel.StringCellValue, 25)
                            Else
                                itemname = Left(cel.NumericCellValue, 25)
                            End If
                        End If

                        cel = row.GetCell(2)
                        If cel IsNot Nothing Then
                            If cel.CellType = CellType.STRING Then
                                itemgroup = cel.StringCellValue
                            Else
                                itemgroup = cel.NumericCellValue
                            End If
                            itemgroupid = accItem.selectitemgrpid(itemgroup, custid)
                        End If
                        cel = row.GetCell(3)
                        If cel IsNot Nothing Then
                            If cel.CellType = CellType.STRING Then
                                majorgroup = cel.StringCellValue
                            Else
                                majorgroup = cel.NumericCellValue
                            End If
                            majorgroupid = accItem.selectmajorgrpid(majorgroup, custid)
                        End If
                        cel = row.GetCell(4)
                        If cel IsNot Nothing Then
                            If cel.CellType = CellType.STRING Then
                                ccname = cel.StringCellValue
                            Else
                                ccname = cel.NumericCellValue
                            End If

                            ccid = accItem.selectccid(ccname)
                        End If
                        cel = row.GetCell(5)
                        If cel IsNot Nothing Then
                            Dim nonusage As String = ""
                            If cel.CellType = CellType.STRING Then
                                nonusage = cel.StringCellValue
                            Else
                                nonusage = cel.NumericCellValue
                            End If
                            If nonusage = "True" Then
                                xusage = 0
                            Else
                                xusage = 1
                            End If

                        End If
                        If itemname <> "" Then
                            Try

                                insertItemMasterEx(itemno, itemname, majorgroupid, itemgroupid, custid, ccid, xusage)

                            Catch ex As Exception

                                koder = "Error insert in Row " & lines & " " & Err.Description
                                Err.Clear()

                            End Try

                        End If


                    End If
Calculate:
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

    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancel.Click
        Try
            Response.Redirect("~/Pages_ItemMaster.aspx", False)


        Catch ex As Exception
            errLog(Err.Description, "Pages_ExportItemMaster:btnCancel_Click")
            Err.Clear()
        End Try
    End Sub
    Public Sub insertItemMasterEx(ByVal ItemNo As String, ByVal Name As String, ByVal mgid As Integer, ByVal igid As Integer, ByVal custid As Integer, ByVal ccid As Integer, ByVal xusage As Integer, ByVal Optional Displayname As String = "", ByVal Optional ItemNo1 As String = "")
        Try

            Dim link As Boolean = False

            If ItemNo.Length > 8 Then
                ItemNo = Mid(ItemNo, 1, 8)
            End If

            Dim clsdt As New clsDataItem
            Dim ds, ds2 As DataSet
            ds = clsdt.SelectItemNo(ItemNo)
            ds2 = clsdt.SelectItemxcost(ItemNo, ccid)

            Name = Name.ToString.Replace("'", "''")

            Dim str As String
            Dim str1 As String = ""
            Dim c_cmd As New Cls_command

            If ds.Tables(0).Rows.Count = 0 Then
                str = "insert into cld_ItemMaster values ( '" & ItemNo & "','" & Name & "', 0, 0, 1, " & custid & ", " & link & ", 0,0,0,1," & igid & "," & mgid & "," & xusage & ",'" & Displayname & "','" & ItemNo1 & "' )"
            Else
                str = "Update cld_ItemMaster set  ItemName = '" & Name & "', itemgroup = " & igid & ", majorgroup = " & mgid & ", nonusage = " & xusage & " where ItemNo = '" & ItemNo & "'"
            End If
            If ccid > 0 And ds2.Tables(0).Rows.Count = 0 Then
                str1 = "insert into cld_itemmaster_cc values ( '" & ItemNo & "', " & custid & ",0, " & ccid & ")"

            End If
            'If ds2.Tables(0).Rows.Count = 0 Then
            '    str1 = "insert into cld_itemmaster_cc values ( '" & ItemNo & "', " & custid & ",0, " & ccid & ")"

            'End If
            c_cmd.executeNonQuery(str)
            If My.Settings.itemmaster_cc = 1 Then
                c_cmd.ReNew()
                c_cmd.executeNonQuery(str1)
            End If


            c_cmd = Nothing

        Catch ex As Exception

            errLog(Err.Description, "Pages_ExportItemMaster:insertItemMasterEx")
            Err.Clear()
        End Try
    End Sub

    Protected Sub btnImport_Click(sender As Object, e As EventArgs) Handles btnImport.Click
        Import()
    End Sub

    Protected Sub btnExport_Click(sender As Object, e As EventArgs) 'Handles btnExport.Click
        Try
            If txtEx.Text = "" Then
                fillplease()
                txtEx.Focus()
                Exit Sub
            End If
            ExportgiftData()
        Catch ex As Exception
            errLog(Err.Description, "Pages_ExportItemMaster:btnExport_Click")
            Err.Clear()
        End Try
    End Sub
End Class