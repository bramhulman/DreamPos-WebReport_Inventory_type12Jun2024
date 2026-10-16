Imports System.IO
Imports NPOI.XSSF.UserModel
Imports NPOI.SS.UserModel
Imports System.Net
Imports ClosedXML.Excel
Public Class Pages_ItemMaster
    Inherits System.Web.UI.Page
    Private mfunction As New ModFunction
    Private clsOutlet As New ClsOutlet
    Private accItem As New clsDataItem
    Public Function modulepriv() As Integer
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        If cookie Is Nothing Then

            Response.Redirect("default.aspx", False)
        End If
        Return cookie("modulepriv")




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
    Public Function username() As String
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        If cookie Is Nothing Then
            Response.Redirect("default.aspx", False)
        End If
        Return cookie("userName")

    End Function
    Public Function rptpriv() As Integer
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        If cookie Is Nothing Then

            Response.Redirect("default.aspx", False)
        End If
        Return cookie("rptpriv")
        'Return cookie("modul")
        'Return cookie("rptf")


    End Function


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'If Not (User.Identity.IsAuthenticated) Or custid() = -1 Then
        '    FormsAuthentication.RedirectToLoginPage()
        '    Exit Sub
        'End If
        Try
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

                    chklink.Checked = True
                    Me.binding()

                    ' Items are synchronized from the source system, never created from this list.
                    btnnew.Visible = False
                    ' Keep the existing Action column permissions.
                    If modulepriv() = usrModulepriv.moduleuser Then
                        btnnew.Visible = False
                        Dim thAction As Control = lstViewDetails.FindControl("thAction")
                        If thAction IsNot Nothing Then thAction.Visible = False
                    End If
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
        Catch ex As Exception
            errLog(Err.Description, "Pages_ItemMaster:Page_Load")
            Err.Clear()
        End Try


    End Sub
    Sub binding()

        Dim cInv As New clsInventory

        Dim ds As New DataSet
        'ds = WService.getItemMaster(cust)

        ds = cInv.getItemSearch(txtsearch.Text, custid, chklink.Checked)

        If ds IsNot Nothing AndAlso ds.Tables.Count > 0 Then
            lstViewDetails.DataSource = ds
        Else
            lstViewDetails.DataSource = Nothing
        End If
        lstViewDetails.DataBind()

    End Sub
    Sub delete(ByVal id As String)

        Dim clsInv As New clsInventory

        ' WService.DeleteItemMaster(id, cust, username)
        clsInv.DeleteItemMaster(id, custid, username)

        clsInv = Nothing

        binding()
    End Sub
    Protected Sub lstView_ItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ListViewCommandEventArgs) Handles lstViewDetails.ItemCommand
        '<%--autocomplete="off"--%>
        Try
            Select Case (e.CommandName)
                Case "Del"

                    Dim confirmValue As String = Request.Form("confirm_value")
                    If confirmValue = "Yes" Then

                        Dim id As String = e.CommandArgument

                        delete(id)

                    Else
                        Exit Sub
                    End If

                Case "Edt"
                    Dim modfunc As New ModFunction
                    'Dim arg As String() = e.CommandArgument.ToString().Split(";")


                    Dim arg As String = e.CommandArgument

                    modfunc.CreateCookie("ItemMasterID", arg)
                    'modfunc.CreateCookie("CCID", arg(1))
                    Response.Redirect("~/Pages_ItemMasterEdit.aspx", False)



            End Select
        Catch ex As Exception
            errLog(Err.Description, "Pages_ItemMaster:lstView_ItemCommand")
            Err.Clear()
        End Try
    End Sub

    Protected Sub lstViewDetails_ItemDataBound(ByVal sender As Object, ByVal e As ListViewItemEventArgs)
        If e.Item.ItemType = ListViewItemType.DataItem Then
            If modulepriv() = usrModulepriv.moduleuser Then
                Dim tdAction As HtmlControl = TryCast(e.Item.FindControl("tdAction"), HtmlControl)
                If tdAction IsNot Nothing Then
                    tdAction.Visible = False
                End If
            End If
        End If
    End Sub

    Protected Sub btnNew_Click(ByVal sender As Object, ByVal e As EventArgs) 'Handles btnnew.Click
        Try

            mfunction.CreateCookie("ItemMasterID", 0)
            ' mfunction.CreateCookie("CCid", 0)
            Response.Redirect("~/Pages_ItemMasterEdit.aspx")

            Exit Sub

        Catch ex As Exception
            errLog(Err.Description, "Pages_ItemMaster:btnNew_Click")
            Err.Clear()
        End Try
    End Sub

    'Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.Click

    '    Dim ds As New DataSet
    '    ds = clsOutlet.SelectDataset_OutletSearch(txtsearch.Text, custid, username, modulepriv)
    '    lstViewDetails.DataSource = ds
    '    lstViewDetails.DataBind()
    'End Sub
    Protected Sub btnsearch(ByVal sender As Object, ByVal e As EventArgs)
        Dim cInv As New clsInventory
        Dim ds As New DataSet
        'ds = WService.getItemSearch(txtSearch.Text, cust)

        ds = cInv.getItemSearch(txtsearch.Text, custid, chklink.Checked)

        If ds IsNot Nothing AndAlso ds.Tables.Count > 0 Then
            lstViewDetails.DataSource = ds
        Else
            lstViewDetails.DataSource = Nothing
        End If
        lstViewDetails.DataBind()
    End Sub

    'Protected Sub OnPagePropertiesChanging(ByVal sender As Object, ByVal e As PagePropertiesChangingEventArgs)
    '    lvDataPager1.SetPageProperties(e.StartRowIndex, e.MaximumRows, False)

    '    binding()
    'End Sub

    'Protected Sub btnExportExcel_Click(ByVal sender As Object, ByVal e As EventArgs) 'Handles btnExportExcel.Click
    '    Try
    '        mfunction.CreateCookie("ID", "")


    '        Response.Redirect("~/Pages_ExportItemMaster.aspx")
    '    Catch ex As Exception
    '        errLog(Err.Description, "Pages_ItemMaster:btnExportExcel_Click")
    '        Err.Clear()
    '    End Try
    'End Sub
    Protected Sub btnImport_Click(sender As Object, e As EventArgs) Handles btnImport.ServerClick

        Response.Redirect("~/Pages_ExportItemMaster.aspx", False)
    End Sub
    Protected Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.ServerClick
        Try

            'Dim dt As DataTable
            Dim dt As New DataTable


            Dim sFile As String = "Item_Master"

            dt = accItem.SelectDataset_ItemMasterEx2(custid)

            'Test Export CSV File1
            Dim sb As StringBuilder = New StringBuilder
            If dt.Rows.Count > 0 Then


                Using wb As New XLWorkbook()
                    wb.Worksheets.Add(dt, "Sheet1")
                    mfunction.errLog("get datatable", "export to excel")
                    Response.Clear()
                    Response.Buffer = True
                    Response.Charset = ""
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                    Response.AddHeader("content-disposition", "attachment;filename=" & sFile & ".xlsx")
                    Using MyMemoryStream As New MemoryStream()
                        wb.SaveAs(MyMemoryStream)
                        mfunction.errLog("save as", "export to excel")
                        MyMemoryStream.WriteTo(Response.OutputStream)
                        mfunction.errLog("write", "export to excel")
                        'Response.Flush()
                        'Response.End()

                        HttpContext.Current.Response.Flush()
                        mfunction.errLog("flush", "export to excel")
                        HttpContext.Current.Response.SuppressContent = True
                        mfunction.errLog("SuppressContent", "export to excel")
                        HttpContext.Current.ApplicationInstance.CompleteRequest()
                        mfunction.errLog("CompleteRequest", "export to excel")
                    End Using
                    'dt.Rows.Clear()
                    'dt.Dispose()
                    dt.Clear()
                    dt.Dispose()

                End Using
                HttpContext.Current.Response.SuppressContent = True
                HttpContext.Current.ApplicationInstance.CompleteRequest()



            End If

        Catch ex As Exception
            mfunction.errLog(Err.Description, "Pages_Receipe:ExportData")
            Err.Clear()
        Finally


        End Try
    End Sub

    Protected Sub chklink_CheckedChanged(sender As Object, e As EventArgs) Handles chklink.CheckedChanged
        binding()
    End Sub
End Class
