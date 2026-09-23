Imports NPOI.SS.Formula.Functions

Public Class ChangedCost
    Inherits System.Web.UI.Page
    Private mfunction As New ModFunction

    Private CBatch As New Cls_IBatch
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
                txtDate.Text = DateTime.Now.ToString("dd MMM yyyy")
                'txtDate.Text = Format(CDate(Date.Now), "MM/dd/yyyy")
                binding()
                If modulepriv() And mHelper.Modulepriv.moduleuser = mHelper.Modulepriv.moduleuser Then
                Else
                    txtDate.ReadOnly = False
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


    End Sub
    Protected Sub txtDate_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtDate.TextChanged
        If txtDate.Text = "" Then
            txtDate.Text = DateTime.Now.ToString("dd MMM yyyy")
        End If
        'GetBackground
        binding()
    End Sub
    Sub binding()

        If custid() = 0 Then
            Response.Redirect("~/Default.aspx")
        Else
            Dim ds As New DataSet
            ds = CBatch.SelectDataset_invBatch(Convert.ToDateTime(txtDate.Text), custid, modulepriv, username)
            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 Then
                lstViewDetails.DataSource = ds
            Else
                lstViewDetails.DataSource = Nothing
            End If
            lstViewDetails.DataBind()
        End If

    End Sub
    Sub delete(ByVal id As Integer)
        Try
            If (DateTime.Now.Hour >= 23 And DateTime.Now.Minute >= 30) Or (DateTime.Now.Hour = 0 And DateTime.Now.Minute <= 30) Then
                notsuccesfull("Please process the transaction after 1.00 AM")
                Exit Sub
            End If
            DeleteInvBatch(id, custid, username)
            binding()
        Catch ex As Exception
            errLog(Err.Description, "Pages_InvBatch:delete")
            Err.Clear()
        End Try

    End Sub
    Public Sub DeleteInvBatch(ByVal id As Integer, ByVal Custid As Integer, ByVal username As String)

        Try

            Dim strTr As String
            Dim c_cmd As New Cls_command

            Dim ds As New DataSet
            Dim clsprocess As New clsInventoryProcess
            'untuk hapus stockcls
            'Dim clsinv As New clsInventory
            Dim clsBatch As New clsInventory_Batch
            ds = CBatch.SelectDataset_InBatchEdit(id, Custid)
            If ds.Tables(0).Rows.Count > 0 Then

                'clsinv.del_stkcls_Batch(id, ds.Tables(0).Rows(0).Item("cc"), ds.Tables(0).Rows(0).Item("stk_id"), ds.Tables(0).Rows(0).Item("stk_name"), ds.Tables(0).Rows(0).Item("U1"), ds.Tables(0).Rows(0).Item("U2"), ds.Tables(0).Rows(0).Item("factor1"), ds.Tables(0).Rows(0).Item("ttlstock"), ds.Tables(0).Rows(0).Item("U1_nm"), ds.Tables(0).Rows(0).Item("U2_nm"), ds.Tables(0).Rows(0).Item("CreatedBy"), ds.Tables(0).Rows(0).Item("Stk_no"), ds.Tables(0).Rows(0).Item("Cc_name"), username, Custid)
                clsBatch.getproperty_RcpDetail_del(Custid, ds.Tables(0).Rows(0).Item("stk_id"), ds.Tables(0).Rows(0).Item("ttlstock"), ds.Tables(0).Rows(0).Item("Invdate"), id, ds.Tables(0).Rows(0).Item("Cc"), ds.Tables(0).Rows(0).Item("Cc_name"), id)
                clsprocess.InvstkId = ds.Tables(0).Rows(0).Item("stk_id")
                clsprocess.InvCc = ds.Tables(0).Rows(0).Item("Cc")
                clsprocess.InvCcName = ds.Tables(0).Rows(0).Item("Cc_name")
                clsprocess.InvCustId = Custid
                clsprocess.InvstkName = ds.Tables(0).Rows(0).Item("stk_name")
                clsprocess.BP_in = -(CDbl(ds.Tables(0).Rows(0).Item("ttlstock")))
                clsprocess.BP_out = 0
                clsprocess.InvStkNo = ds.Tables(0).Rows(0).Item("Stk_no")
                clsprocess.InvDate = ds.Tables(0).Rows(0).Item("Invdate") 'Date.Now.Date.ToString("yyyy-MM-dd")
                'clsprocess.InvCost = bcost
                clsprocess.BCost = True
                clsprocess.executeNonQuery_Inv_ProcessCls()
            End If


            'end untuk hapus stockcls

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


            strTr = "DELETE FROM cld_inv_trans  where ID = " & id & " and type = 9; DELETE FROM cld_inv_transnotes where transid = " & id & " "
            c_cmd.executeNonQuery(strTr)


        Catch ex As Exception
            errLog(Err.Description, "Pages_InvBatch:DeleteInvBatch")
            Err.Clear()
        End Try
    End Sub
    Protected Sub lstView_ItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ListViewCommandEventArgs) Handles lstViewDetails.ItemCommand
        Dim c_cmd As New clsDbo("")
        Try
            Select Case (e.CommandName)
                Case "Del"
                    'Dim id As Integer = Convert.ToInt32(e.CommandArgument)

                    'delete(id)

                    Dim confirmValue As String = Request.Form("confirm_value")
                    If confirmValue = "Yes" Then
                        Dim id As Integer = Convert.ToInt32(e.CommandArgument)

                        If c_cmd.ExecuteScalar("select count(custid) from cld_inv_closing where custid = " & custid() & " and invdate = '" & Date.Now.Date.ToString("yyyy-MM-dd") & "' and  invflag = 0 ", 0) = 1 Then
                            'FormsAuthentication.SetAuthCookie(username, False)
                            Session.Clear()
                            Session.Abandon()
                            Session.RemoveAll()


                            messagesuccesfull("Closing process,Please login again.", False)
                            FormsAuthentication.RedirectToLoginPage()
                            Exit Sub
                        Else

                            delete(id)
                        End If


                    Else
                        Exit Sub
                    End If


                Case "Edt"
                    Dim id As Integer = Convert.ToInt32(e.CommandArgument)

                    mfunction.CreateCookie("iBatchID", id)

                    Response.Redirect("~/Pages_InvBatchEdit.aspx")

            End Select


        Catch ex As Exception
            errLog(Err.Description, "Pages_InvBatch:ItemCommand")
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
            mfunction.CreateCookie("iBatchID", 0)

            Response.Redirect("~/Pages_InvBatchEdit.aspx", False)
        Catch ex As Exception
            errLog(Err.Description, "Pages_InvBatch:btnNew_Click")
            Err.Clear()
        End Try
    End Sub
    Protected Sub OnPagePropertiesChanging(ByVal sender As Object, ByVal e As PagePropertiesChangingEventArgs)
        lvDataPager1.SetPageProperties(e.StartRowIndex, e.MaximumRows, False)
        binding()
    End Sub
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.ServerClick
        Dim ds As New DataSet
        Try

            ds = CBatch.SelectDataset_invBatchSearch(txtsearch.Text, Convert.ToDateTime(txtDate.Text), custid, modulepriv, username)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 Then
                lstViewDetails.DataSource = ds
            Else
                lstViewDetails.DataSource = Nothing
            End If
            lstViewDetails.DataBind()
        Catch ex As Exception
            errLog(Err.Description, "Pages_InvBatch:btnSearch_Click")
            Err.Clear()
        End Try
    End Sub

    Sub messagesuccesfull(ByVal msg As String, Optional ByVal bsuccess As Boolean = True)

        Dim message As String = msg
        Dim url As String
        If bsuccess Then
            url = "../Pages_InvBatch.aspx"
        Else
            url = "Default.aspx"
        End If

        Dim script As String = "window.onload = function(){ alert('"
        script += message
        script += "');"
        script += "window.location = '"
        script += url
        script += "'; }"
        ClientScript.RegisterStartupScript(Me.GetType(), "Redirect", script, True)

    End Sub
    Protected Function GetBackground()
        Dim imgstat As String
        If txtDate.Text <> DateTime.Now.ToString("dd MMM yyyy") Then
            imgstat = "nodisplay"
            If modulepriv() And mHelper.Modulepriv.moduleuser = mHelper.Modulepriv.moduleuser Then
            Else
                If (username() = My.Settings.admin Or username() = "support") Then
                    imgstat = "displayaction"
                End If

                If CDate(txtDate.Text) < DateTime.Now.AddDays(-My.Settings.limitday).ToString("dd MMM yyyy") Then
                    imgstat = "nodisplay"
                End If
            End If

        Else
            imgstat = "displayaction"

        End If

        Return imgstat
    End Function

End Class
