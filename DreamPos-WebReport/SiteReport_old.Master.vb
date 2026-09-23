Public Class SiteReport
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub LoginStatus1_LoggingOut(sender As Object, e As LoginCancelEventArgs)
        'Dim cookie As HttpCookie = Request.Cookies("UserLogin")
        ' Context.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie)
        If Request.Cookies("UserLogin_inv") IsNot Nothing Then
            FormsAuthentication.SignOut()
            Request.Cookies("UserLogin_inv").Expires = DateTime.Now.AddDays(-1)
            Session.Clear()
            Session.Abandon()
            Session.RemoveAll()
            FormsAuthentication.RedirectToLoginPage()
        End If

        'cookie.Expires = DateTime.Now.AddDays(-1D)

    End Sub

End Class