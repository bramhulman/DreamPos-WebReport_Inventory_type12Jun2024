Imports System.Web
Imports System.Web.Security

Public Class Logout
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' 1. Sign out from Forms Authentication
        FormsAuthentication.SignOut()

        ' 2. Clear Session
        Session.Clear()
        Session.Abandon()
        Session.RemoveAll()

        ' 3. Expire Protocol Cookies
        If Request.Cookies("UserLogin_inv") IsNot Nothing Then
            Dim myCookie As New HttpCookie("UserLogin_inv")
            myCookie.Expires = DateTime.Now.AddYears(-1)
            Response.Cookies.Add(myCookie)
        End If

        If Request.Cookies("LastActive") IsNot Nothing Then
            Dim laCookie As New HttpCookie("LastActive")
            laCookie.Expires = DateTime.Now.AddYears(-1)
            Response.Cookies.Add(laCookie)
        End If
        
        If Request.Cookies("ASP.NET_SessionId") IsNot Nothing Then
            Dim sessCookie As New HttpCookie("ASP.NET_SessionId")
            sessCookie.Expires = DateTime.Now.AddYears(-1)
            Response.Cookies.Add(sessCookie)
        End If

        ' 4. Instant Redirect
        Response.Redirect("Default.aspx")
    End Sub

End Class
