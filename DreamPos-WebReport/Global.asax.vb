Imports System
Imports System.Web
Imports System.Web.SessionState
Imports Microsoft.VisualBasic
Public Class Global_asax
    Inherits System.Web.HttpApplication

    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the application is started
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the session is started
    End Sub

    Sub Application_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires at the beginning of each request
        ' Hanya update cookie jika sudah ada cookie valid dari browser
        Dim oldCookie As HttpCookie = Request.Cookies("UserLogin_inv")
        If oldCookie IsNot Nothing AndAlso Not String.IsNullOrEmpty(oldCookie("userName")) Then
            ' --- UPDATE COOKIE YANG SUDAH ADA ---
            Dim userCookie As New HttpCookie("UserLogin_inv")
            userCookie("userName") = oldCookie("userName")
            userCookie("custid") = oldCookie("custid")
            userCookie("modulepriv") = oldCookie("modulepriv")
            userCookie("rptpriv") = oldCookie("rptpriv")
            userCookie("modul") = oldCookie("modul")
            userCookie("rptf") = oldCookie("rptf")
            userCookie("invpriv") = oldCookie("invpriv")

            ' Update LastActive
            userCookie("LastActive") = Now.ToString("yyyyMMddHHmmss")
            userCookie.Expires = Now.AddMinutes(10)
            userCookie.Path = "/"
            userCookie.Secure = False

            Response.Cookies.Add(userCookie)
        End If
    End Sub

    Sub Application_AuthenticateRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires upon attempting to authenticate the use
    End Sub

    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        Dim failure As Exception = Server.GetLastError()
        If failure Is Nothing Then Return
        Try
            mHelper.errLog(failure.ToString(), "Application_Error: " & Request.Url.AbsolutePath)
        Catch
            ' Error reporting must not prevent ASP.NET from showing the friendly page.
        End Try
    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the session ends
    End Sub

    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the application ends
    End Sub

End Class
