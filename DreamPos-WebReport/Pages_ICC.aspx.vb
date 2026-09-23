Public Class Pages_ICC
    Inherits System.Web.UI.Page
    Private mfunction As New ModFunction
    Private CIcc As New Cls_ICc
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


                Me.binding()
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
                ds = CIcc.SelectDataset_Icc(custid, username, modulepriv, 0)
                If ds IsNot Nothing AndAlso ds.Tables.Count > 0 Then
                    lstViewDetails.DataSource = ds
                Else
                    lstViewDetails.DataSource = Nothing
                End If
                lstViewDetails.DataBind()
            End If
        Catch ex As Exception
            errLog(Err.Description, "Pages_ICC:binding")
            Err.Clear()
        End Try

    End Sub
    Sub delete(ByVal id As Integer)
        Try
            DeleteIcc(id, custid, username)
            binding()
        Catch ex As Exception
            errLog(Err.Description, "Pages_ICC:delete")
            Err.Clear()
        End Try
    End Sub
    Public Sub DeleteIcc(ByVal id As Integer, ByVal Custid As Integer, ByVal username As String)
        'Dim pc As String = pic.ToString
        Try

            Dim str As String
            Dim c_cmd As New Cls_command

            str = "DELETE FROM cld_inv_cc  where id = " & id & ""

            c_cmd.executeNonQuery(str)

            'Revisi 10 Mei
            insertLogModule(username, "InvCc", "Delete", Custid)

        Catch ex As Exception

            errLog(Err.Description, "Pages_ICC:DeleteInvCc")
            Err.Clear()
        End Try
    End Sub
    Protected Sub lstView_ItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ListViewCommandEventArgs) Handles lstViewDetails.ItemCommand
        '<%--autocomplete="off"--%>
        Try
            Select Case (e.CommandName)
                Case "Del"

                    Dim confirmValue As String = Request.Form("confirm_value")
                    If confirmValue = "Yes" Then

                        Dim id As Integer = Convert.ToInt32(e.CommandArgument)

                        delete(id)

                    Else
                        Exit Sub
                    End If

                Case "Edt"
                    Dim modfunc As New ModFunction
                    Dim id As Integer = Convert.ToInt32(e.CommandArgument)

                    modfunc.CreateCookie("ccID", id)

                    Response.Redirect("~/Pages_ICCEdit.aspx")



            End Select
        Catch ex As Exception
            errLog(Err.Description, "Pages_ICC:lstView_ItemCommand")
            Err.Clear()
        End Try
    End Sub

    Protected Sub btnNew_Click(ByVal sender As Object, ByVal e As EventArgs) 'Handles btnnew.Click
        Try
            Dim modfunc As New ModFunction
            modfunc.CreateCookie("ccID", 0)

            Response.Redirect("~/Pages_ICCEdit.aspx")

            Exit Sub

        Catch ex As Exception
            errLog(Err.Description, "Pages_ICC:btnNew_Click")
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
        Try
            If txtsearch.Text.Trim() = "" Then
                binding()
                Exit Sub
            End If

            Dim ds As New DataSet

            ds = CIcc.SelectDataset_IccSearch(txtsearch.Text, custid, username, modulepriv)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 Then
                lstViewDetails.DataSource = ds
            Else
                lstViewDetails.DataSource = Nothing
            End If
            lstViewDetails.DataBind()
        Catch ex As Exception
            errLog(Err.Description, "Pages_ICC:btnsearch")
            Err.Clear()
        End Try
    End Sub

    Protected Sub OnPagePropertiesChanging(ByVal sender As Object, ByVal e As PagePropertiesChangingEventArgs)
        lvDataPager1.SetPageProperties(e.StartRowIndex, e.MaximumRows, False)

        binding()
    End Sub

End Class
