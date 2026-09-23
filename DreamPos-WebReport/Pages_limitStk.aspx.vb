Public Class Pages_limitStk
    Inherits System.Web.UI.Page
    Private mfunction As New ModFunction
    Private clsOutlet As New ClsOutlet
    Private CStk As New Cls_IStk
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
                ds = CStk.SelectDataset_limitStk(custid)
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
    Sub delete(ByVal id As Integer)
        FailureText.Text = ""
        Dim clsInv As New clsInventory
        Dim iHasil As Integer = 0
        Try
            iHasil = clsInv.DeleteStk(id, custid, username)
            If iHasil = 1 Then

                binding()
            ElseIf iHasil = 2 Then
                FailureText.Text = "Delete Data Failed, this item has Transaction Record"
            Else
                FailureText.Text = "Delete Data Failed"
            End If


        Catch ex As Exception

        End Try
        clsInv = Nothing
    End Sub
    Protected Sub lstView_ItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ListViewCommandEventArgs) Handles lstViewDetails.ItemCommand
        '<%--autocomplete="off"--%>
        Dim modfunc As New ModFunction
        Try
            Select Case (e.CommandName)
                Case "Del"

                    Dim confirmValue As String = Request.Form("confirm_value")
                    If confirmValue = "Yes" Then

                        Dim id As Integer = Convert.ToInt32(e.CommandArgument)
                        delete(id)
                        'Dim arg As String() = e.CommandArgument.ToString().Split(";")
                        'modfunc.CreateCookie("InvStk_ID", arg(0))
                        'modfunc.CreateCookie("InvStk_cc", arg(1))
                        'delete(arg(0), arg(1))

                    Else
                        Exit Sub
                    End If

                Case "Edt"

                    'Dim id As Integer = Convert.ToInt32(e.CommandArgument)
                    'modfunc.CreateCookie("InvStk_ID", id)
                    Dim arg As String() = e.CommandArgument.ToString().Split(";")
                    modfunc.CreateCookie("InvStk_ID", arg(0))
                    modfunc.CreateCookie("InvStk_cc", arg(1))


                    Response.Redirect("~/Pages_limitStkEdit.aspx")



            End Select
        Catch ex As Exception
            errLog(Err.Description, "Pages_Stk:lstView_ItemCommand")
            Err.Clear()
        End Try
    End Sub

    Protected Sub btnNew_Click(ByVal sender As Object, ByVal e As EventArgs) 'Handles btnnew.Click
        Try
            Dim modfunc As New ModFunction
            modfunc.CreateCookie("InvStk_ID", 0)
            modfunc.CreateCookie("InvStk_cc", 0)
            Response.Redirect("~/Pages_limitStkEdit.aspx")

            Exit Sub

        Catch ex As Exception
            errLog(Err.Description, "Pages_Stk:btnNew_Click")
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
            Dim ds As New DataSet

            ds = CStk.SelectDataset_limitStk(custid, txtsearch.Text)

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
    Protected Sub btnExportExcel_Click(sender As Object, e As EventArgs) 'Handles btnExportExcel.Click
        Try
            'CreateCookie("ID", 0)

            Response.Redirect("~/Pages_ExportInv_limitStk.aspx")
        Catch ex As Exception
            errLog(Err.Description, "Pages_Stk:btnNew_Click")
            Err.Clear()
        End Try
    End Sub

End Class
