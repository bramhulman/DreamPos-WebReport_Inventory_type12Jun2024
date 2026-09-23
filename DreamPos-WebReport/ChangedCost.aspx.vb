Imports NPOI.SS.Formula.Functions

Public Class ChangedCost
    Inherits System.Web.UI.Page
    Private mfunction As New ModFunction

    Private CInvware As New clsInventory_Warehouse
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
        Try
            If Not cookie Is Nothing Then

                If Not Me.IsPostBack Then
                    txtDate.Text = DateTime.Now.ToString("dd MMM yyyy")
                    'txtDate.Text = Format(CDate(Date.Now), "MM/dd/yyyy")
                    binding()
                    If modulepriv() And usrModulepriv.moduleuser = usrModulepriv.moduleuser Then
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

        Catch ex As Exception
            errLog(Err.Description, "ChangedCost:Page_Load")
            Err.Clear()
        End Try


    End Sub
    Protected Sub txtDate_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtDate.TextChanged
        Try
            If txtDate.Text = "" Then
                txtDate.Text = DateTime.Now.ToString("dd MMM yyyy")
            End If
            ViewState("changecost") = Nothing
            lstViewDetails.Items.Clear()
            'GetBackground
            binding()
        Catch ex As Exception
            errLog(Err.Description, "ChangedCost:txtDate_TextChanged")
            Err.Clear()
        End Try

    End Sub
    Sub binding()
        Dim ds As New DataSet
        Try
            If custid() = 0 Then
                Response.Redirect("~/Default.aspx", False)
            Else

                ds = CInvware.get_invchangecostSrch(Convert.ToDateTime(txtDate.Text), txtsearch.Text)
                If ds.Tables(0).Rows.Count > 0 Then
                    ViewState("changecost") = ds.Tables(0)
                    If ds IsNot Nothing AndAlso ds.Tables.Count > 0 Then
                        lstViewDetails.DataSource = ds
                    Else
                        lstViewDetails.DataSource = Nothing
                    End If
                    lstViewDetails.DataBind()
                Else
                    Dim dt As New DataTable
                    dt.Clear()
                    lstViewDetails.DataSource = dt
                    'lstViewDetails.Items.Clear()
                    lstViewDetails.DataBind()
                End If

            End If
        Catch ex As Exception
            errLog(Err.Description, "ChangedCost:binding")
            Err.Clear()
        End Try


    End Sub
    Private Sub bindData2()
        Try
            If CBool(IIf(IsNothing(ViewState("changecost")), True, False)) Then
                binding()
            Else
                If DirectCast(ViewState("changecost"), DataTable).Rows.Count > 0 Then
                    Dim dt As New DataTable
                    dt = ViewState("changecost")
                    dt.DefaultView.RowFilter = "Stk_Name like '%" & txtsearch.Text & "%' Or wID like '%" & txtsearch.Text & "%'"


                    lstViewDetails.DataSource = dt.DefaultView
                    lstViewDetails.DataBind()
                End If
            End If
            'If ViewState("changecost") IsNot Nothing And DirectCast(ViewState("changecost"), DataTable).Rows.Count > 0 Then

            '    Dim dt As New DataTable
            '    dt = ViewState("changecost")
            '    dt.DefaultView.RowFilter = "Stk_Name like '%" & txtsearch.Text & "%' Or wID like '%" & txtsearch.Text & "%'"


            '    lstViewDetails.DataSource = dt.DefaultView
            '    lstViewDetails.DataBind()
            'Else
            '    binding()
            'End If
        Catch ex As Exception
            errLog(Err.Description, "ChangedCost:bindData2")
            Err.Clear()
        End Try


    End Sub

    Protected Sub lstView_ItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ListViewCommandEventArgs) Handles lstViewDetails.ItemCommand
        Dim c_cmd As New clsDbo("")
        Try
            Select Case (e.CommandName)



                Case "Edt"
                    'Dim id As Integer = Convert.ToInt32(e.CommandArgument)
                    Dim arg As String() = e.CommandArgument.ToString().Split(";")
                    Dim wid As String = arg(0)
                    Dim stkid As Integer = arg(1)
                    Try
                        If updatechangedcost(wid, stkid) = True Then
                            messagesuccesfull("Update Success", False)
                            binding()
                        End If
                    Catch ex As Exception
                        errLog(Err.Description, "ChangedCost:updatecost_perstkid")
                        Err.Clear()
                    End Try




            End Select


        Catch ex As Exception
            errLog(Err.Description, "ChangedCost:ItemCommand")
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
            If updatechangedcost("", 0) Then
                messagesuccesfull("Update Success", False)
                binding()
            End If
        Catch ex As Exception
            errLog(Err.Description, "ChangedCost:btnNew_Click")
            Err.Clear()
        End Try
    End Sub
    Protected Sub OnPagePropertiesChanging(ByVal sender As Object, ByVal e As PagePropertiesChangingEventArgs)
        lvDataPager1.SetPageProperties(e.StartRowIndex, e.MaximumRows, False)
        bindData2()
    End Sub
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.ServerClick
        Dim ds As New DataSet
        Try
            bindData2()
            'ds = CInvware.get_invchangecostSrch(Convert.ToDateTime(txtDate.Text), txtsearch.Text)

            'lstViewDetails.DataSource = ds
            'lstViewDetails.DataBind()
        Catch ex As Exception
            errLog(Err.Description, "ChangedCost:btnSearch_Click")
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
        Dim imgstat As String = ""
        Try
            If txtDate.Text <> DateTime.Now.ToString("dd MMM yyyy") Then
                imgstat = "nodisplay"
                If modulepriv() And usrModulepriv.moduleuser = usrModulepriv.moduleuser Then
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
        Catch ex As Exception
            errLog(Err.Description, "ChangedCost:GetBackground")
            Err.Clear()
        End Try


        Return imgstat
    End Function
    Protected Function updatechangedcost(ByVal wid As String, ByVal stkid As Integer) As Boolean
        Dim hasil As Boolean = False
        Dim c_cmd1 As New clsDbo("")
        Dim ccware As Integer
        Try
            ccware = c_cmd1.ExecuteScalar("select id from `cld_inv_cc` where olid = 0 ", 0)
            hasil = CInvware.Update_changedcost(wid, stkid, ccware, CDate(txtDate.Text))
        Catch ex As Exception
            errLog(Err.Description, "ChangedCost:updatechangedcost")
            Err.Clear()
        End Try
        Return hasil
    End Function

End Class
