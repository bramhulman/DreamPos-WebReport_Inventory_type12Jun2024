Public Class SiteHome
    Inherits System.Web.UI.MasterPage
    Private mfunction As New ModFunction
    Dim assembly As System.Reflection.Assembly = System.Reflection.Assembly.GetExecutingAssembly()
    Dim fvi As System.Diagnostics.FileVersionInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location)
    Dim version As String = fvi.FileVersion
    Public Function modulepriv() As Integer
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        If cookie Is Nothing Then
            Response.Redirect("~/Default.aspx", False)
            Return -1
        Else
            Return cookie("modulepriv")
        End If
    End Function
    Public Function custid() As Integer
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        If cookie Is Nothing Then
            Response.Redirect("~/Default.aspx", False)
            Return -1
        Else
            Return cookie("custid")
        End If
    End Function
    Public Function username() As String
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        If cookie Is Nothing Then
            Response.Redirect("~/Default.aspx", False)
            Return ""
        Else
            Return cookie("userName")
        End If
    End Function
    Public Function rptpriv() As Integer
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        If cookie Is Nothing Then
            Response.Redirect("~/Default.aspx", False)
            Return -1
        Else
            Return cookie("rptpriv")
        End If
    End Function
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'If check_UserTypePriv(0, TransPriv()) = True Then
        If modulepriv() <> 0 Then
            'huseraccess.Visible = False
            luseraccess.Visible = False
            If modulepriv() = 2 Then
                ldashboard.Visible = False
                lreport.Visible = False
                lreportmonthly.Visible = False
            End If
        End If

        lblversion.Text = version
    End Sub
    Protected Sub LoginStatus1_LoggingOut(sender As Object, e As LoginCancelEventArgs)
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        ' Context.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie)
        If Request.Cookies("UserLogin_inv") IsNot Nothing Then
            FormsAuthentication.SignOut()
            Request.Cookies("UserLogin_inv").Expires = DateTime.Now.AddDays(-1)
            Session.Clear()
            Session.RemoveAll()
            Session.Abandon()
            Response.Cookies.Add(cookie)
        End If

        'cookie.Expires = DateTime.Now.AddDays(-1D)

    End Sub
    Protected Sub webreport_ServerClick(ByVal sender As Object, ByVal e As EventArgs)
        RedirectToWebReportHome(Context)
    End Sub
End Class