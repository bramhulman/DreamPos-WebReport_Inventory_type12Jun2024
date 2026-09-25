Public Class Pages_InvClosingStock
    Inherits System.Web.UI.Page
    Private mfunction As New ModFunction
    Private cInv As New clsInventory

    Public Function modulepriv() As Integer
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        If cookie Is Nothing Then

            Response.Redirect("default.aspx")
        End If
        Return cookie("modulepriv")




    End Function
    Public Function custid() As Integer
        Dim icust As Integer = 0
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")

        If cookie Is Nothing Then
            Response.Redirect("~/Default.aspx")

        Else
            icust = cookie("custid")
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
    Public Function rptpriv() As Integer
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        If cookie Is Nothing Then

            Response.Redirect("default.aspx")
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
                'txtDate.Text = Format(Now, "dd MMM yyyy")
                'txtDate.Text = Format(CDate(Date.Now), "MM/dd/yyyy")
                txtDate.Text = Format(Now.AddDays(-1), "dd MMM yyyy")
                loadCc()
                binding()

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
    Sub loadCc()
        'DdlTo.Items.Clear()
        'DdlTo.SelectedValue = Nothing
        Dim ds As New DataSet
        ds = cInv.getInvcc(custid, username, modulepriv, 0)
        If ds IsNot Nothing AndAlso ds.Tables.Count > 0 Then
            ddlOutlet.DataSource = ds
        Else
            ddlOutlet.DataSource = Nothing
        End If
        ddlOutlet.DataValueField = "Id"
        ddlOutlet.DataTextField = "Name"

        ddlOutlet.DataBind()
    End Sub
    Protected Sub txtDate_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtDate.TextChanged
        If txtDate.Text = "" Then
            txtDate.Text = Format(Now, "dd MMM yyyy")
        End If
        'GetBackground
        binding()
    End Sub
    Protected Function GetBackground()

        Dim imgstat As String

        If CDate(txtDate.Text) < Format(CDate(Date.Now.AddDays(-1)), "MM/dd/yyyy") Then
            imgstat = "nodisplay"
        Else
            imgstat = "displayaction"
        End If

        Return imgstat

    End Function
    Sub binding()

        If custid() = 0 Then
            Response.Redirect("~/Default.aspx")
        Else
            Dim ds As New DataSet
            'ds = WService.getInvRwaste(Convert.ToDateTime(txtDate.Text), cust, ModPrv, username)

            ds = cInv.getclosingwaste(txtsearch.Text, Convert.ToDateTime(txtDate.Text), custid, ddlOutlet.SelectedValue, modulepriv, username)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 Then
                lstViewDetails.DataSource = ds
            Else
                lstViewDetails.DataSource = Nothing
            End If
            lstViewDetails.DataBind()
        End If

    End Sub
    Sub delete(ByVal id As Integer)
        Dim ds As New DataSet
        Try
            'WService.DeleteInvRwaste(id, cust, username)
            ds = cInv.Select_invtrans(id, custid)
            cInv.DeleteInvRwaste(id, custid, username)
            Dim c_cmd As New clsDbo("")
            Dim secday As Date
            If ds.Tables(0).Rows.Count > 0 Then
                Dim clsprocess As New clsInventoryProcess
                clsprocess.InvCc = ds.Tables(0).Rows(0).Item("cc").ToString
                clsprocess.InvCcName = ds.Tables(0).Rows(0).Item("cc_name").ToString
                clsprocess.InvCustId = custid()
                clsprocess.InvstkId = ds.Tables(0).Rows(0).Item("Stk_Id").ToString
                clsprocess.InvStkNo = ds.Tables(0).Rows(0).Item("Stk_no").ToString
                clsprocess.InvstkName = ds.Tables(0).Rows(0).Item("Stk_Name").ToString
                ' clsprocess.Invrcvqty = (ds.Tables(0).Rows(0).Item("ttlStock").ToString)
                clsprocess.InvCost = ds.Tables(0).Rows(0).Item("cost").ToString

                clsprocess.InvDate = CDate(ds.Tables(0).Rows(0).Item("InvDate").ToString)
                'cInv.executeNonQuery_Inv_ProcessCls(Date.Now.Date.ToString("yyyy-MM-dd"), ddlFrom.SelectedValue, ddlFrom.SelectedItem.Text, stkid, stknumber, stkname, custid, 0, stktotal)
                clsprocess.delclosingwaste()
                'clsprocess.InvDate = CDate(ds.Tables(0).Rows(0).Item("InvDate").ToString).AddDays(1)
                secday = CDate(ds.Tables(0).Rows(0).Item("InvDate").ToString)
                clsprocess.Inv_Proc_trans_stckClsbyid(secday, secday, custid, ds.Tables(0).Rows(0).Item("cc").ToString, ds.Tables(0).Rows(0).Item("Stk_Id").ToString)
                'clsprocess.executeNonQuery_Inv_ProcessCls()

            End If


            'Response.Redirect("~/Pages_InvRwaste.aspx")
            binding()
            c_cmd.dispose()
            ds.Tables.Clear()
            ds.Dispose()
        Catch ex As Exception

        End Try
    End Sub
    Public Sub DeleteInvBatch(ByVal id As Integer, ByVal Custid As Integer, ByVal username As String)

        Try

            Dim strTr As String
            Dim c_cmd As New Cls_command

            Dim ds As New DataSet

            Dim a As New clsDbo("Select * FROM cld_inv_transusage  where linkid = " & id & " and type = 11")
            ds = a.selectDataset
            If ds.Tables(0).Rows.Count > 0 Then
                For Each row As DataRow In ds.Tables(0).Rows

                    strTr = "DELETE FROM cld_inv_transusage  where linkid = " & id & " and cc = " & row.Item("cc") & " and stk_id = " & row.Item("stk_id")
                    c_cmd.executeNonQuery(strTr)
                    'c_cmd.executeNonQuery_Inv_CalcCost(row.Item("stk_id"), row.Item("cc"), Custid)
                    'c_cmd.executeNonQuery_Inv_CalcStock(row.Item("stk_id"), row.Item("Cc"), Custid)

                Next
            End If

            a.dispose()
            a = Nothing


            ' PERBAIKAN: Jangan hapus transaksi type=9 (Batch Production) dari halaman Closing Stock
            ' strTr = "DELETE FROM cld_inv_trans where ID = " & id & " and type = 9 "
            ' c_cmd.executeNonQuery(strTr)


        Catch ex As Exception
            errLog(Err.Description, "webservice:DeleteInvBatch")
            Err.Clear()
        End Try
    End Sub
    Protected Sub lstView_ItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ListViewCommandEventArgs) Handles lstViewDetails.ItemCommand

        Try
            Select Case (e.CommandName)
                Case "Del"
                    'Dim id As Integer = Convert.ToInt32(e.CommandArgument)

                    'delete(id)

                    Dim confirmValue As String = Request.Form("confirm_value")
                    If confirmValue = "Yes" Then
                        'Dim commandArgs As String() = e.CommandArgument.ToString().Split(New Char() {","c})
                        'Dim id As Integer = Convert.ToInt32(commandArgs(0))
                        'Dim ccid As Integer = Convert.ToInt32(commandArgs(1))
                        'delete(id, ccid)
                        ' id As Integer = Convert.ToInt32(e.CommandArgument)
                        Dim id As Integer = Convert.ToInt32(e.CommandArgument)
                        delete(id)

                    Else
                        Exit Sub
                    End If


                Case "Edt"
                    'Dim id As Integer = Convert.ToInt32(e.CommandArgument)
                    Dim arg As String() = e.CommandArgument.ToString().Split(";")
                    mfunction.CreateCookie("CWasteID", arg(0))
                    mfunction.CreateCookie("wCc", arg(1))
                    'mfunction.CreateCookie("wInvDate", arg(2))

                    'mfunction.CreateCookie("CWasteID", id)

                    Response.Redirect("~/Pages_Invclswaste.aspx")

            End Select


        Catch ex As Exception
            errLog(Err.Description, "Pages_InvRcv:ItemCommand")
            Err.Clear()
        End Try
    End Sub

    Sub notsuccesfull(ByVal msg As String)
        Dim message As String = msg
        Dim sb As New System.Text.StringBuilder()
        ' Dim url As String = "~/Pages_InvTransfer.aspx"
        sb.Append("<script type = 'text/javascript'>")
        sb.Append("window.onload=function(){")
        sb.Append("alert('")
        sb.Append(message)
        sb.Append("')};")
        sb.Append("</script>")
        ClientScript.RegisterClientScriptBlock(Me.GetType(), "Alert", sb.ToString())
    End Sub
    Protected Sub btnNew_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnnew.ServerClick
        Try
            mfunction.CreateCookie("CWasteID", 0)
            mfunction.CreateCookie("wCc", 0)
            mfunction.CreateCookie("wInvDate", "")

            Response.Redirect("~/Pages_Invclswaste.aspx")
        Catch ex As Exception
            errLog(Err.Description, "WebRwaste:btnNew_Click")
            Err.Clear()
        End Try
    End Sub
    Protected Sub OnPagePropertiesChanging(ByVal sender As Object, ByVal e As PagePropertiesChangingEventArgs)
        lvDataPager1.SetPageProperties(e.StartRowIndex, e.MaximumRows, False)
        binding()
    End Sub
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.ServerClick
        Dim ds As New DataSet

        'ds = cInv.getInvclswasteSrch(txtsearch.Text, Convert.ToDateTime(txtDate.Text), custid, modulepriv, username)
        ds = cInv.getclosingwaste(txtsearch.Text, Convert.ToDateTime(txtDate.Text), custid, ddlOutlet.SelectedValue, modulepriv, username)
        'WService.getInvRwasteSearch(txtSearch.Text, Convert.ToDateTime(txtDate.Text), cust, ModPrv, username)

        If ds IsNot Nothing AndAlso ds.Tables.Count > 0 Then
            lstViewDetails.DataSource = ds
        Else
            lstViewDetails.DataSource = Nothing
        End If
        lstViewDetails.DataBind()
    End Sub
    Protected Sub DdlOutlet_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlOutlet.SelectedIndexChanged
        binding()
    End Sub

End Class
