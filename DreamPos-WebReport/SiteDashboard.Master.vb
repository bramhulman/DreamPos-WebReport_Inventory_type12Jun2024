Public Class SiteDashboard
    Inherits System.Web.UI.MasterPage
    Public Function modulepriv() As Integer
        Dim cookie As HttpCookie = Request.Cookies("UserLogin")

        Return cookie("modulepriv")



    End Function
    Public Function custid() As Integer
        Dim cookie As HttpCookie = Request.Cookies("UserLogin")

        Return cookie("custid")

    End Function
    Public Function username() As Integer
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
        If Modulepriv() = 1 Then

            hOutlet.Visible = False
            lOutlet.Visible = False
            huseraccess.Visible = False
            luseraccess.Visible = False

        End If
    End Sub

    Protected Sub LoginStatus1_LoggingOut(sender As Object, e As LoginCancelEventArgs)
        'Dim cookie As HttpCookie = Request.Cookies("UserLogin")
        ' Context.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie)
        If Request.Cookies("userLogin") IsNot Nothing Then
            FormsAuthentication.SignOut()
            Request.Cookies("userLogin").Expires = DateTime.Now.AddDays(-1)
            Session.Abandon()
        End If

        'cookie.Expires = DateTime.Now.AddDays(-1D)

    End Sub

End Class