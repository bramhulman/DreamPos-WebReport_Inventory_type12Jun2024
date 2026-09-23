Public Class PurchaseRequest
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
        'wType = Request.Cookies("wType").Value
        AddHandler DdlType.SelectedIndexChanged, AddressOf DdlType_SelectedIndexChanged
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        If Not cookie Is Nothing Then

            If Not Me.IsPostBack Then

                'If wType = "1" Then
                '    DdlType.SelectedIndex = 0
                'ElseIf wType = "2" Then
                '    DdlType.SelectedIndex = 1
                'ElseIf wType = "3" Then
                '    DdlType.SelectedIndex = 2
                'End If

                'If DdlType.SelectedIndex > 0 Then
                '    DdlType.Items.RemoveAt(0)
                '    DdlType.Enabled = True
                'Else
                '    DdlType.Enabled = False
                'End If

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
                ds = clsInv.getPurchaseRequest(DdlType.SelectedItem.ToString, modulepriv, username, 0)
                'WService.getInvWare(DdlType.SelectedValue, cust, ModPrv1, username, 0)
                If ds IsNot Nothing AndAlso ds.Tables.Count > 0 Then
                    lstViewDetails.DataSource = ds
                Else
                    lstViewDetails.DataSource = Nothing
                End If
                lstViewDetails.DataBind()
            End If

        Catch ex As Exception
            errLog(Err.Description, "PurchaseRequest:binding")
            Err.Clear()
        End Try

    End Sub

    Sub getweblic()
        Try

            If custid() = 0 Then
                Response.Redirect("~/Default.aspx", False)
            Else
                Dim ds As New DataSet
                ds = clsInv.getPurchaseRequest(DdlType.SelectedValue, modulepriv, username, 0)
                If ds IsNot Nothing AndAlso ds.Tables.Count > 0 Then
                    lstViewDetails.DataSource = ds
                Else
                    lstViewDetails.DataSource = Nothing
                End If
                lstViewDetails.DataBind()
            End If

        Catch ex As Exception
            errLog(Err.Description, "PurchaseRequest:binding")
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
            'id is pr_id
            clsInv.RejectInvPR(CInt(id), username)
            binding()
        Catch ex As Exception
            errLog(Err.Description, "PurchaseRequest:delete")
            Err.Clear()
        End Try
    End Sub

    'Catch ex As Exception
    '        errLog(Err.Description, "PurchaseRequest:delete")
    '        Err.Clear()
    '    End Try

    'End Sub
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
                    mfunction.CreateCookie("pr_id", arg(0))
                    mfunction.CreateCookie("pr_no", arg(1))
                    mfunction.CreateCookie("pr_date", arg(2))
                    mfunction.CreateCookie("required_date", arg(3))
                    mfunction.CreateCookie("Cc", arg(4))
                    mfunction.CreateCookie("request_by", arg(5))
                    mfunction.CreateCookie("stat", arg(6))
                    mfunction.CreateCookie("origin", "store")

                    Response.Redirect("~/PurchaseRequestDetail.aspx", False)

                Case "Send"

                    Dim arg As String() = e.CommandArgument.ToString().Split(";")
                    Dim id As String = arg(0)

                    Dim confirmValue As String = Request.Form("confirm_value")
                    If confirmValue = "Yes" Then
                        mfunction.CreateCookie("pr_id", arg(0))
                        mfunction.CreateCookie("pr_no", arg(1))
                        mfunction.CreateCookie("prdate", arg(2))
                        mfunction.CreateCookie("requireddate", arg(3))
                        mfunction.CreateCookie("cc", arg(4))
                        mfunction.CreateCookie("ccname", arg(5))
                        mfunction.CreateCookie("request_by", arg(6))
                        mfunction.CreateCookie("stat", arg(7))
                        Response.Redirect("~/printprev_PR.aspx", False)

                    Else
                        Exit Sub
                    End If
            End Select


        Catch ex As Exception
            errLog(Err.Description, "PurchaseRequest:lstView_ItemCommand")
            Err.Clear()
        End Try
    End Sub

    Protected Sub btnNew_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnnew.ServerClick
        Try
            ' Clear PR edit cookies agar tidak carry-over ke mode New
            mfunction.CreateCookie("pr_id", "0")
            mfunction.CreateCookie("pr_no", "")
            mfunction.CreateCookie("pr_date", "")
            mfunction.CreateCookie("required_date", "")
            mfunction.CreateCookie("Cc", "0")
            mfunction.CreateCookie("request_by", "")
            mfunction.CreateCookie("stat", "")
            mfunction.CreateCookie("origin", "store")

            Response.Redirect("~/PurchaseRequestDetail.aspx", False)
        Catch ex As Exception
            errLog(Err.Description, "PurchaseRequest:btnNew_Click")
            Err.Clear()
        End Try
    End Sub



    Protected Sub OnPagePropertiesChanging(ByVal sender As Object, ByVal e As PagePropertiesChangingEventArgs)
        lvDataPager1.SetPageProperties(e.StartRowIndex, e.MaximumRows, False)
        binding()
    End Sub
    Protected Function GetBackground()
        Return "nodisplay"
    End Function
    Protected Function Getprintbg()
        Dim imgstat As String
        If check_usrInvPriv(24, InvPriv) Then
            imgstat = "displayaction"
        Else
            imgstat = "nodisplay"
        End If


        Return imgstat
    End Function


    Private Sub DdlType_SelectedIndexChanged(sender As Object, e As EventArgs)
        bindingsearch()
    End Sub
    Private Sub bindingsearch()

        Try

            Dim ds As New DataSet

            ds = clsInv.getPurchaseRequest(DdlType.SelectedItem.ToString, modulepriv, username, 0, txtsearch.Text)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 Then
                lstViewDetails.DataSource = ds
            Else
                lstViewDetails.DataSource = Nothing
            End If
            lstViewDetails.DataBind()

        Catch ex As Exception
            errLog(Err.Description, "PurchaseRequest:bindingsearch")
            Err.Clear()
        End Try
    End Sub

    Protected Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.ServerClick
        bindingsearch()
    End Sub
End Class
