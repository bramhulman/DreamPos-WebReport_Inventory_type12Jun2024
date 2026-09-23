Public Class Outlet
    Inherits System.Web.UI.Page

    Private mfunction As New ModFunction
    Private clsOutlet As New ClsOutlet
    Public Function modulepriv() As Integer
        Dim cookie As HttpCookie = Request.Cookies("UserLogin")

        Return cookie("modulepriv")



    End Function
    Public Function custid() As Integer
        Dim cookie As HttpCookie = Request.Cookies("UserLogin")

        Return cookie("custid")

    End Function
    Public Function username() As String
        Dim cookie As HttpCookie = Request.Cookies("UserLogin")
        Return cookie("userName")

    End Function
    Public Function rptpriv() As Integer
        Dim cookie As HttpCookie = Request.Cookies("UserLogin")

        Return cookie("rptpriv")
        'Return cookie("modul")
        'Return cookie("rptf")


    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (User.Identity.IsAuthenticated) Or custid() = -1 Then
            FormsAuthentication.RedirectToLoginPage()
            Exit Sub
        End If
        Dim cookie As HttpCookie = Request.Cookies("UserLogin")
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


        Dim ds As New DataSet
        ds = clsOutlet.SelectDataset_Outlet(custid, username, modulepriv)

        If ds IsNot Nothing AndAlso ds.Tables.Count > 0 Then
            lstViewDetails.DataSource = ds
        Else
            lstViewDetails.DataSource = Nothing
        End If
        lstViewDetails.DataBind()

    End Sub

    Protected Sub lstView_ItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ListViewCommandEventArgs) Handles lstViewDetails.ItemCommand
        '<%--autocomplete="off"--%>
        Select Case (e.CommandName)
            'Case "Del"

            '    Dim confirmValue As String = Request.Form("confirm_value")
            '    If confirmValue = "Yes" Then

            '        Dim id As Integer = Convert.ToInt32(e.CommandArgument)

            '        delete(id)

            '    Else
            '        Exit Sub
            '    End If

            Case "Edt"
                ID = Convert.ToInt32(e.CommandArgument)

                'editlist(ID)
                mfunction.CreateCookie("Outlet_edt_ID", e.CommandArgument)

                'CreateCookie("Name", getStoreID())
                Response.Redirect("outlet_Edit")



        End Select
    End Sub

    'Protected Sub btnNew_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnNew.Click
    '    Try
    '        mfunction.CreateCookie("condiment_edt_ID", 0)


    '        Response.Redirect("Condiment_Edit")
    '    Catch ex As Exception
    '        mfunction.errLog(Err.Description, "Condiment:btnNew_Click")
    '        Err.Clear()
    '    End Try
    'End Sub

    'Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.Click

    '    Dim ds As New DataSet
    '    ds = clsOutlet.SelectDataset_OutletSearch(txtsearch.Text, custid, username, modulepriv)
    '    lstViewDetails.DataSource = ds
    '    lstViewDetails.DataBind()
    'End Sub
    Protected Sub btnsearch(ByVal sender As Object, ByVal e As EventArgs)
        Dim ds As New DataSet
        ds = clsOutlet.SelectDataset_OutletSearch(txtsearch.Text, custid, username, modulepriv)
        If ds IsNot Nothing AndAlso ds.Tables.Count > 0 Then
            lstViewDetails.DataSource = ds
        Else
            lstViewDetails.DataSource = Nothing
        End If
        lstViewDetails.DataBind()
    End Sub

    Protected Sub OnPagePropertiesChanging(ByVal sender As Object, ByVal e As PagePropertiesChangingEventArgs)
        lvDataPager1.SetPageProperties(e.StartRowIndex, e.MaximumRows, False)

        binding()
    End Sub
End Class
