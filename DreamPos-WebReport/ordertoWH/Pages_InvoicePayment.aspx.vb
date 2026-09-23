Imports System.IO
Imports NPOI.XSSF.UserModel
Imports NPOI.SS.UserModel
Imports System.Net
Imports ClosedXML.Excel
Public Class Pages_InvoicePayment
    Inherits System.Web.UI.Page
    Private mfunction As New ModFunction
    Private wType As String
    Private clsInv As New clsInventory
    Private clsinvw As New clsInventory_Warehouse
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
    Public Function InvPriv() As Integer
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        If cookie Is Nothing Then

            Response.Redirect("default.aspx", False)
        End If
        Return cookie("invpriv")
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
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
                loadFrom()
                txtDate.Text = Format(Now, "dd MMM yyyy")
                txtDateto.Text = Format(Now, "dd MMM yyyy")
                binding(modulepriv)
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


    End Sub
    Sub binding(ByVal modpriv As Integer)

        If custid() = 0 Then
            Response.Redirect("~/Default.aspx")
        Else
            Dim ds As New DataSet
            If modpriv And usrModulepriv.moduleuser = usrModulepriv.moduleuser Then
                ds = clsinvw.SelectDS_invoiceStrSrch(Format(Convert.ToDateTime(txtDate.Text), "MM/dd/yyyy"), Format(Convert.ToDateTime(txtDateto.Text), "MM/dd/yyyy"), DdlType.SelectedValue, "", custid, modulepriv, username, DdlCc.SelectedValue)
            Else

                ds = clsinvw.SelectDS_invoiceStrSrch(Format(Convert.ToDateTime(txtDate.Text), "MM/dd/yyyy"), Format(Convert.ToDateTime(txtDateto.Text), "MM/dd/yyyy"), DdlType.SelectedValue, "", custid, modulepriv, username, 0)
            End If
            'ds = clsInv.getInvWareSearch(DdlType.SelectedValue, txtsearch.Text, custid, modulepriv, username, DdlCc.SelectedValue)
            'WService.getInvWareSearch(DdlType.SelectedValue, txtSearch.Text, cust, ModPrv, username, DdlCc.SelectedValue)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 Then
                lstViewDetails.DataSource = ds
            Else
                lstViewDetails.DataSource = Nothing
            End If
            lstViewDetails.DataBind()
        End If

    End Sub

    Sub loadFrom()
        Dim ds As New DataSet
        ds = clsInv.getIccOnly(custid, username, modulepriv)
        'WService.getIccOnly(cust, username, ModPrv)
        If ds IsNot Nothing AndAlso ds.Tables.Count > 0 Then
            DdlCc.DataSource = ds
        Else
            DdlCc.DataSource = Nothing
        End If
        DdlCc.DataValueField = "Id"
        DdlCc.DataTextField = "Name"
        DdlCc.DataBind()

        If DdlCc.Items.Count > 0 Then
            DdlCc.SelectedIndex = 0
        End If

    End Sub


    Sub delete(ByVal id As String, ByVal icc As Integer)
        Try
            'WService.DeleteInvWare(id, cust, icc, 0)
            clsInv.DeleteInvWare(id, custid, icc, 0, username)
            binding(modulepriv)
        Catch ex As Exception
            errLog(Err.Description, "Pages_InvOrderW:delete")
            Err.Clear()
        End Try

    End Sub
    Protected Sub lstView_ItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ListViewCommandEventArgs) Handles lstViewDetails.ItemCommand

        Select Case (e.CommandName)

            Case "Edt"
                Dim arg As String() = e.CommandArgument.ToString().Split(";")
                mfunction.CreateCookie("woID", arg(0))
                mfunction.CreateCookie("woCc", arg(1))
                mfunction.CreateCookie("wofrom", arg(2))
                mfunction.CreateCookie("woDelDate", arg(3))
                mfunction.CreateCookie("woDelID", arg(4))
                Dim sStatus As String = arg(5)
                mfunction.CreateCookie("woNotes", arg(6))

                'If DdlType.SelectedValue = 1 Then
                '    CreateCookie("woDelstatus", 0)
                'Else
                '    CreateCookie("woDelstatus", 1)
                'End If

                If sStatus = "Order" Then
                    mfunction.CreateCookie("woDelstatus", 0)
                ElseIf sStatus = "Delivery" Then
                    mfunction.CreateCookie("woDelstatus", 1)
                    'Receive
                ElseIf sStatus = "Receive" Then
                    mfunction.CreateCookie("woDelstatus", 2)
                End If

                Response.Redirect("~/Pages_InvoicePaymentEdit.aspx")


            Case "Send"

                Dim arg As String() = e.CommandArgument.ToString().Split(";")
                Dim id As String = arg(0)

                Dim confirmValue As String = Request.Form("confirm_value")
                If confirmValue = "Yes" Then
                    mfunction.CreateCookie("from", "~/Pages_InvOrderWHO.aspx")
                    mfunction.CreateCookie("woID", id)
                    mfunction.CreateCookie("print", "INV")
                    Response.Redirect("~/printprev.aspx")

                Else
                    Exit Sub
                End If
            Case "Send2"

                Dim arg As String() = e.CommandArgument.ToString().Split(";")
                Dim id As String = arg(0)

                Dim confirmValue As String = Request.Form("confirm_value")
                If confirmValue = "Yes" Then
                    mfunction.CreateCookie("print", "DO")
                    mfunction.CreateCookie("woID", id)
                    mfunction.CreateCookie("from", "~/Pages_InvOrderWHO.aspx")
                    Response.Redirect("~/printprev.aspx")

                Else
                    Exit Sub
                End If
        End Select
    End Sub






    Private Sub bindingsearch()
        Dim ds As New DataSet

        'If modulepriv() And usrModulepriv.moduleuser = usrModulepriv.moduleuser Then
        ds = clsinvw.SelectDS_invoiceStrSrch(Format(Convert.ToDateTime(txtDate.Text), "MM/dd/yyyy"), Format(Convert.ToDateTime(txtDateto.Text), "MM/dd/yyyy"), DdlType.SelectedValue, "", custid, modulepriv, username, DdlCc.SelectedValue)
        'Else

        '    ds = clsinvw.SelectDS_invoiceStrSrch(DdlType.SelectedValue, txtsearch.Text, custid, modulepriv, username, 0)
        'End If
        'WService.getInvWareSearch(DdlType.SelectedValue, txtSearch.Text, cust, ModPrv, username, DdlCc.SelectedValue)

        If ds IsNot Nothing AndAlso ds.Tables.Count > 0 Then
            lstViewDetails.DataSource = ds
        Else
            lstViewDetails.DataSource = Nothing
        End If
        lstViewDetails.DataBind()
    End Sub
    Protected Function GetBackground()
        Dim imgstat As String
        If DdlType.SelectedValue = 1 Then
            imgstat = "displayaction"
        Else
            imgstat = "nodisplay"
        End If

        Return imgstat
    End Function
    Protected Function GetBackgroundDO()
        Dim imgstat As String
        If DdlType.SelectedValue = 3 Then
            imgstat = "displayaction"
        Else
            imgstat = "nodisplay"
        End If

        Return imgstat
    End Function

    Private Sub DdlType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DdlType.SelectedIndexChanged
        bindingsearch()
    End Sub


    Protected Sub DdlCc_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles DdlCc.SelectedIndexChanged
        bindingsearch()
    End Sub

    Protected Sub btnNew_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnnew.ServerClick
        Try
            mfunction.CreateCookie("invoiceid", 0)
            Response.Redirect("~/Pages_InvoicePaymentEdit.aspx")
        Catch ex As Exception
            errLog(Err.Description, "Pages_InvoicePayment:btnNew_Click")
            Err.Clear()
        End Try
    End Sub

    Protected Sub btnExportExcel_Click(sender As Object, e As EventArgs) Handles btnExportExcel.ServerClick

        Try

            'Dim dt As DataTable
            Dim ds As New DataSet


            Dim sFile As String = "Invoice Payment"

            ds = clsinvw.SelectDS_invoicepayment(Format(Convert.ToDateTime(txtDate.Text), "MM/dd/yyyy"), Format(Convert.ToDateTime(txtDateto.Text), "MM/dd/yyyy"), DdlType.SelectedValue, "", custid, modulepriv, username, DdlCc.SelectedValue)

            'Test Export CSV File1
            Dim sb As StringBuilder = New StringBuilder
            If ds.Tables.Count > 0 Then


                Using wb As New XLWorkbook()
                    wb.Worksheets.Add(ds.Tables(0), "Sheet1")
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
                    ds.Tables.Clear()
                    ds.Dispose()

                End Using
                HttpContext.Current.Response.SuppressContent = True
                HttpContext.Current.ApplicationInstance.CompleteRequest()



            End If

        Catch ex As Exception
            mfunction.errLog(Err.Description, "Report:ExportData")
            Err.Clear()
        Finally
            'loadgif.Visible = False
            'HttpContext.Current.Response.SuppressContent = True
            'HttpContext.Current.ApplicationInstance.CompleteRequest()

        End Try

        Exit Sub
    End Sub

    Protected Sub txtDate_TextChanged(sender As Object, e As EventArgs) Handles txtDate.TextChanged
        If txtDate.Text = "" Then
            txtDate.Text = Format(Now, "dd MMM yyyy")
            txtDateto.Text = Format(Now, "dd MMM yyyy")
        End If
        binding(modulepriv)
    End Sub

    Protected Sub txtDateto_TextChanged(sender As Object, e As EventArgs) Handles txtDateto.TextChanged
        If txtDate.Text = "" Then
            txtDate.Text = Format(Now, "dd MMM yyyy")
            txtDateto.Text = Format(Now, "dd MMM yyyy")
        End If
        binding(modulepriv)
    End Sub
End Class
