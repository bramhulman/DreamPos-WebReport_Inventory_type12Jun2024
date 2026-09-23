Public Class Pages_InvTransfer
    Inherits System.Web.UI.Page
    Private mfunction As New ModFunction
    Private clsInv As New clsInventory

    Public Function modpriv() As Integer
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
                txtDate.Text = Format(Now.AddDays(-7), "dd MMM yyyy")
                txtToDate.Text = Format(Now, "dd MMM yyyy")
                LoadCCFrom()
                binding()
            End If
            If modpriv() And usrModulepriv.moduleuser = usrModulepriv.moduleuser Then
                txtDate.ReadOnly = True
                txtToDate.ReadOnly = True
            Else
                txtDate.ReadOnly = False
                txtToDate.ReadOnly = False
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

    Private Sub LoadCCFrom()
        Dim cicc As New clsInventory_Cc
        Try
            Dim ds As DataSet = cicc.SelectDataset_IccOnly(custid, username, modpriv)
            ddlCCFrom.Items.Clear()
            ddlCCFrom.Items.Add(New ListItem("-- All Cost Centre --", "0"))
            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 Then
                For Each row As DataRow In ds.Tables(0).Rows
                    ddlCCFrom.Items.Add(New ListItem(row("name").ToString(), row("id").ToString()))
                Next
            End If
        Catch ex As Exception
            mfunction.errLog(Err.Description, "Pages_InvTransfer:LoadCCFrom")
            Err.Clear()
        End Try
    End Sub

    Protected Sub txtDate_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtDate.TextChanged
        If txtDate.Text = "" Then
            txtDate.Text = Format(Now.AddDays(-7), "dd MMM yyyy")
        End If
        binding()
    End Sub

    Protected Sub txtToDate_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtToDate.TextChanged
        If txtToDate.Text = "" Then
            txtToDate.Text = Format(Now, "dd MMM yyyy")
        End If
        binding()
    End Sub

    Protected Function GetBackground(ByVal trfid As String)

        Dim imgstat As String
        Dim cinvtrf As New clsInventory_Transfer
        If cinvtrf.trf_date(trfid) = 0 Then
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
            Dim dFrom As Date = Now.AddDays(-7)
            Dim dTo As Date = Now
            DateTime.TryParse(txtDate.Text, dFrom)
            DateTime.TryParse(txtToDate.Text, dTo)

            Dim selCC As Integer = 0
            If ddlCCFrom.SelectedValue <> "" Then Integer.TryParse(ddlCCFrom.SelectedValue, selCC)

            Dim ds As DataSet = clsInv.getInvTrf(dFrom, dTo, custid, selCC)
            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 Then
                lstViewDetails.DataSource = ds
            Else
                lstViewDetails.DataSource = Nothing
            End If
            lstViewDetails.DataBind()
        End If

    End Sub
    Sub delete(ByVal id As String)
        Dim cinvtrans As New clsInventory_Transfer
        Dim dt As DataTable = cinvtrans.SelectDataTable_invtrans_Transfer(id, custid)
        Dim ddate As Date

        If (Now.Hour >= 23 And Now.Minute >= 30) Or (Now.Hour = 0 And Now.Minute <= 30) Then
            notsuccesfull("Please process the transaction after 1.00 AM")
            Exit Sub
        End If

        clsInv.DeleteInvTrf(id, custid, username)
        If dt.Rows.Count > 0 Then
            Dim cInv As New clsInventory
            Dim clsprocess As New clsInventoryProcess
            Dim clsicc As New Cls_ICc

            For i As Integer = 0 To dt.Rows.Count - 1
                ddate = dt.Rows(i).Item("invdate")
                clsprocess.InvstkId = dt.Rows(i).Item("stk_id").ToString
                clsprocess.InvCc = dt.Rows(i).Item("cc").ToString
                clsprocess.InvCcName = dt.Rows(i).Item("cc_name").ToString
                clsprocess.InvCustId = custid()
                clsprocess.InvstkName = dt.Rows(i).Item("Stk_Name").ToString
                clsprocess.t_in = -(dt.Rows(i).Item("ttlStock").ToString)
                clsprocess.t_out = 0
                clsprocess.InvStkNo = cInv.getStkByid(dt.Rows(i).Item("stk_id").ToString, custid).Tables(0).Rows(0).Item("Number").ToString
                clsprocess.InvDate = ddate.ToString("yyyy-MM-dd 00:00:00")
                clsprocess.BCost = True
                'cInv.executeNonQuery_Inv_ProcessCls(Date.Now.Date.ToString("yyyy-MM-dd"), ddlFrom.SelectedValue, ddlFrom.SelectedItem.Text, stkid, stknumber, stkname, custid, 0, stktotal)
                clsprocess.executeNonQuery_Inv_ProcessCls()

                clsprocess.InvstkId = dt.Rows(i).Item("stk_id").ToString
                clsprocess.InvCc = dt.Rows(i).Item("ccfrom").ToString
                clsprocess.InvCcName = clsicc.SelectDataIcc(dt.Rows(i).Item("ccfrom").ToString).Tables(0).Rows(0).Item("name")
                clsprocess.InvCustId = custid()
                clsprocess.InvstkName = dt.Rows(i).Item("Stk_Name").ToString
                clsprocess.t_in = 0
                clsprocess.t_out = -(dt.Rows(i).Item("ttlStock").ToString)
                clsprocess.InvStkNo = cInv.getStkByid(dt.Rows(i).Item("stk_id").ToString, custid).Tables(0).Rows(0).Item("Number").ToString
                clsprocess.InvDate = ddate.ToString("yyyy-MM-dd 00:00:00")
                'cInv.executeNonQuery_Inv_ProcessCls(Date.Now.Date.ToString("yyyy-MM-dd"), ddlFrom.SelectedValue, ddlFrom.SelectedItem.Text, stkid, stknumber, stkname, custid, 0, stktotal)
                clsprocess.executeNonQuery_Inv_ProcessCls()
            Next i
        End If
        'clsInv.DeleteInvTrf(id, custid, username)
        binding()
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


            strTr = "DELETE FROM cld_inv_trans  where ID = " & id & " and type = 9 "
            c_cmd.executeNonQuery(strTr)


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
                        Dim arg As String() = e.CommandArgument.ToString().Split(";")

                        Dim id As String = arg(0)

                        delete(id)

                    Else
                        Exit Sub
                    End If


                Case "Edt"
                    Dim arg As String() = e.CommandArgument.ToString().Split(";")
                    Dim trfId As String = arg(0)
                    Dim ccFromId As String = If(arg.Length > 1, arg(1), "0")
                    mfunction.CreateCookie("ID", trfId)
                    mfunction.CreateCookie("CcF", ccFromId)

                    Response.Redirect(String.Format("~/Pages_InvTrfEdit.aspx?id={0}&ccf={1}", Server.UrlEncode(trfId), Server.UrlEncode(ccFromId)), False)

            End Select


        Catch ex As Exception
            errLog(Err.Description, "Pages_InvTransfer:ItemCommand")
            Err.Clear()
        End Try
    End Sub

    Sub notsuccesfull(ByVal msg As String)
        Dim message As String = msg
        Dim sb As New System.Text.StringBuilder()
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
            mfunction.CreateCookie("ID", "")
            mfunction.CreateCookie("CcF", 0)

            Response.Redirect("~/Pages_InvTrfEdit.aspx", False)
        Catch ex As Exception
            errLog(Err.Description, "Pages_InvTransfer:btnNew_Click")
            Err.Clear()
        End Try
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.ServerClick
        Dim dFrom As Date = Now.AddDays(-7)
        Dim dTo As Date = Now
        DateTime.TryParse(txtDate.Text, dFrom)
        DateTime.TryParse(txtToDate.Text, dTo)

        Dim selCC As Integer = 0
        If ddlCCFrom.SelectedValue <> "" Then Integer.TryParse(ddlCCFrom.SelectedValue, selCC)

        Dim ds As DataSet = clsInv.getInvTrfSearch(txtsearch.Text, dFrom, dTo, custid, selCC)
        If ds IsNot Nothing AndAlso ds.Tables.Count > 0 Then
            lstViewDetails.DataSource = ds
        Else
            lstViewDetails.DataSource = Nothing
        End If
        lstViewDetails.DataBind()
    End Sub

    Protected Sub txtsearch_TextChanged(sender As Object, e As EventArgs) Handles txtsearch.TextChanged
        btnSearch_Click(sender, e)
    End Sub
End Class
