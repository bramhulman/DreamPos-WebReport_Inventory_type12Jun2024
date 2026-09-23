Imports NPOI.SS.Formula.Functions

Public Class Pages_ExpOther
    Inherits System.Web.UI.Page
    Private mfunction As New ModFunction
    Dim cInv As New clsInventory
    Private CEOther As New Cls_ExOther
    Public Function modulepriv() As Integer
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        If cookie Is Nothing Then

            Response.Redirect("default.aspx")
        End If
        Return cookie("modulepriv")




    End Function
    Public Function custid() As Integer
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        If cookie Is Nothing Then

            Response.Redirect("default.aspx")

        End If
        Return cookie("custid")

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

            If (Page.IsPostBack) Then
            Else

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
                Response.Redirect("~/Default.aspx", False)
            Else
                Dim ds As New DataSet
                ds = CEOther.SelectDataset_ExpOther(custid, modulepriv, username, Date.Now.Date.AddDays(-7).Date)
                If ds IsNot Nothing AndAlso ds.Tables.Count > 0 Then
                    lstViewDetails.DataSource = ds
                Else
                    lstViewDetails.DataSource = Nothing
                End If
                lstViewDetails.DataBind()
            End If
        Catch ex As Exception

        End Try

    End Sub
    Sub delete(ByVal id As String)
        Try
            DeleteExpOther(id, custid, username)
            binding()
        Catch ex As Exception

        End Try
    End Sub
    Public Sub DeleteExpOther(ByVal id As String, ByVal Custid As Integer, ByVal username As String)
        'Dim pc As String = pic.ToString
        Try

            Dim str As String
            Dim c_cmd As New Cls_command

            str = "DELETE FROM cld_ExpOther  where id = '" & id & "'"

            c_cmd.executeNonQuery(str)

            'Revisi 10 Mei
            insertLogModule(username, "ExpOther", "Delete", Custid)

        Catch ex As Exception

            errLog(Err.Description, "webservice:DeleteExpOther")
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

                        Dim id As String = e.CommandArgument

                        delete(id)
                        'End If

                    Else
                        Exit Sub
                    End If


                Case "Edt"

                    ID = e.CommandArgument

                    mfunction.CreateCookie("ID", ID)

                    Response.Redirect("~/Pages_ExpOtherEdit.aspx", False)



            End Select
        Catch ex As Exception
            errLog(Err.Description, "Pages_ExpOther:lstView_ItemCommand")
            Err.Clear()
        End Try
    End Sub


    Protected Sub btnsearch(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim ds As New DataSet

            ds = CEOther.SelectDataset_ExOtherSearch(txtsearch.Text, custid, modulepriv, username)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 Then
                lstViewDetails.DataSource = ds
            Else
                lstViewDetails.DataSource = Nothing
            End If
            lstViewDetails.DataBind()
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub OnPagePropertiesChanging(ByVal sender As Object, ByVal e As PagePropertiesChangingEventArgs)
        lvDataPager1.SetPageProperties(e.StartRowIndex, e.MaximumRows, False)
        binding()
    End Sub


    Protected Sub btnnew_ServerClick(sender As Object, e As EventArgs) Handles btnnew.ServerClick
        Try
            mfunction.CreateCookie("ID", "")


            Response.Redirect("~/Pages_ExpOtherEdit.aspx")
        Catch ex As Exception

        End Try
    End Sub

End Class
