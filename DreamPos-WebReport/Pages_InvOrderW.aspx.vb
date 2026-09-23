Public Class Pages_InvOrderW
    Inherits System.Web.UI.Page
    Private mfunction As New ModFunction
    Private wType As String
    Private clsInv As New clsInventory
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
            Return -1
        Else
            Return cookie("invpriv")
        End If
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
        Dim cookieType As HttpCookie = Request.Cookies("wType")
        If cookieType IsNot Nothing Then
            wType = cookieType.Value
        Else
            wType = "1"
        End If
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        If Not cookie Is Nothing Then

            If Not Me.IsPostBack Then

                If wType = "1" Then
                    DdlType.SelectedIndex = 0
                ElseIf wType = "2" Then
                    DdlType.SelectedIndex = 1
                ElseIf wType = "3" Then
                    DdlType.SelectedIndex = 2
                End If

                If DdlType.SelectedIndex > 0 Then
                    DdlType.Items.RemoveAt(0)
                    DdlType.Enabled = True
                Else
                    DdlType.Enabled = False
                End If

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
    Sub binding()
        Try

            If custid() = 0 Then
                Response.Redirect("~/Default.aspx")
            Else
                Dim ds As New DataSet
                ds = clsInv.getInvWare(DdlType.SelectedValue, custid, modulepriv, username, 0)
                'WService.getInvWare(DdlType.SelectedValue, cust, ModPrv1, username, 0)
                If ds IsNot Nothing AndAlso ds.Tables.Count > 0 Then
                    lstViewDetails.DataSource = ds
                Else
                    lstViewDetails.DataSource = Nothing
                End If
                lstViewDetails.DataBind()
            End If

        Catch ex As Exception
            errLog(Err.Description, "Pages_InvOrderW:binding")
            Err.Clear()
        End Try

    End Sub

    Sub getweblic()
        Try

            If custid() = 0 Then
                Response.Redirect("~/Default.aspx", False)
            Else
                Dim ds As New DataSet
                ds = clsInv.getInvWare(DdlType.SelectedValue, custid, modulepriv, username, 0)
                'WService.getInvWare(DdlType.SelectedValue, cust, ModPrv1, username, 0)
                If ds IsNot Nothing AndAlso ds.Tables.Count > 0 Then
                    lstViewDetails.DataSource = ds
                Else
                    lstViewDetails.DataSource = Nothing
                End If
                lstViewDetails.DataBind()
            End If

        Catch ex As Exception
            errLog(Err.Description, "Pages_InvOrderW:binding")
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
    Sub delete(ByVal id As String, ByVal icc As Integer)
        Try
            'WService.DeleteInvWare(id, cust, icc, 0)
            If clsInv.check_belumdelivery(id, icc) = True Then
                clsInv.DeleteInvWare(id, custid, icc, 0, username)
                binding()
            Else
                notsuccesfull("orders was delivered")
                Exit Sub
            End If

        Catch ex As Exception
            errLog(Err.Description, "Pages_InvOrderW:delete")
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
                        Dim ccto As Integer = arg(1)

                        delete(id, ccto)

                    Else
                        Exit Sub
                    End If

                Case "Edt"
                    Dim arg As String() = e.CommandArgument.ToString().Split(";")
                    mfunction.CreateCookie("wID", arg(0))
                    mfunction.CreateCookie("wCc", arg(1))
                    mfunction.CreateCookie("wfrom", arg(2))
                    mfunction.CreateCookie("wstatDate", arg(3))
                    mfunction.CreateCookie("wNotes", arg(4))
                    mfunction.CreateCookie("wcreatedDate", arg(5))
                    mfunction.CreateCookie("wdelDate", arg(6))
                    mfunction.CreateCookie("wrcvDate", arg(7))
                    mfunction.CreateCookie("wDelID", If(arg.Length > 8, arg(8), ""))

                    If DdlType.SelectedValue = 3 Then
                        mfunction.CreateCookie("wfinish", 1)
                    Else
                        mfunction.CreateCookie("wfinish", 0)
                    End If

                    If DdlType.SelectedValue = 1 Then
                        Response.Redirect("~/Pages_InvOrderWDetail.aspx", False)
                    ElseIf DdlType.SelectedValue = 2 Or DdlType.SelectedValue = 3 Then
                        Response.Redirect("~/Pages_InvOrderWDetailRcv.aspx", False)
                    End If
                Case "Send"

                    Dim arg As String() = e.CommandArgument.ToString().Split(";")
                    Dim id As String = arg(0)
                    Dim delId As String = If(arg.Length > 1, arg(1), "")
                    Dim ccTo As String = If(arg.Length > 2, arg(2), "")

                    Dim confirmValue As String = Request.Form("confirm_value")
                    If confirmValue = "Yes" Then
                        If DdlType.SelectedValue = "2" OrElse DdlType.SelectedValue = "3" Then
                            mfunction.CreateCookie("print", "DO")
                            mfunction.CreateCookie("woPrintDelID", delId)
                        Else
                            mfunction.CreateCookie("print", "order")
                            mfunction.CreateCookie("woPrintDelID", "")
                        End If
                        mfunction.CreateCookie("from", "~/Pages_InvOrderW.aspx")
                        mfunction.CreateCookie("woID", id)
                        Response.Redirect("~/printprev.aspx", False)

                    Else
                        Exit Sub
                    End If
            End Select


        Catch ex As Exception
            errLog(Err.Description, "Pages_InvOrderW:lstView_ItemCommand")
            Err.Clear()
        End Try
    End Sub

    Protected Sub btnNew_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnnew.ServerClick
        Try
            mfunction.CreateCookie("wID", "")
            mfunction.CreateCookie("wCc", 0)
            mfunction.CreateCookie("wfrom", 0)
            mfunction.CreateCookie("wstatDate", "")
            mfunction.CreateCookie("wNotes", "")
            mfunction.CreateCookie("wcreatedDate", "")
            mfunction.CreateCookie("wdelDate", "")
            mfunction.CreateCookie("wrcvDate", "")

            Response.Redirect("~/Pages_InvOrderWDetail.aspx", False)
        Catch ex As Exception
            errLog(Err.Description, "Pages_InvOrderW:btnNew_Click")
            Err.Clear()
        End Try
    End Sub

    'Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.Click

    '    Dim ds As New DataSet
    '    ds = clsOutlet.SelectDataset_OutletSearch(txtsearch.Text, custid, username, modulepriv)
    '    lstViewDetails.DataSource = ds
    '    lstViewDetails.DataBind()
    'End Sub


    Protected Function GetBackground()
        Dim imgstat As String
        If DdlType.SelectedValue = 1 Then
            imgstat = "displayaction"
        Else
            imgstat = "nodisplay"
        End If

        Return imgstat
    End Function
    Protected Function Getprintbg()
        Dim imgstat As String
        If check_usrInvPriv(22, InvPriv) Then
            imgstat = "displayaction"
        Else
            imgstat = "nodisplay"
        End If


        Return imgstat
    End Function
    'Protected Sub btnExportExcel_Click(ByVal sender As Object, ByVal e As EventArgs) 'Handles btnExportExcel.Click
    '    Try
    '        mfunction.CreateCookie("ID", "")


    '        Response.Redirect("~/Pages_ExportItemMaster.aspx")
    '    Catch ex As Exception
    '        errLog(Err.Description, "Pages_ItemMaster:btnExportExcel_Click")
    '        Err.Clear()
    '    End Try
    'End Sub

    Private Sub DdlType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DdlType.SelectedIndexChanged
        bindingsearch()
    End Sub
    Private Sub bindingsearch()

        Try

            Dim ds As New DataSet

            ds = clsInv.getInvWareSearch(DdlType.SelectedValue, txtsearch.Text, custid, modulepriv, username, 0)
            'WService.getInvWareSearch(DdlType.SelectedValue, txtSearch.Text, cust, ModPrv1, username, 0)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 Then
                lstViewDetails.DataSource = ds
            Else
                lstViewDetails.DataSource = Nothing
            End If
            lstViewDetails.DataBind()

        Catch ex As Exception
            errLog(Err.Description, "Pages_InvOrderW:bindingsearch")
            Err.Clear()
        End Try
    End Sub

    Protected Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.ServerClick
        bindingsearch()
    End Sub
End Class
