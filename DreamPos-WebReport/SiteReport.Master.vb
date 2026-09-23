Public Class SiteReport
    Inherits System.Web.UI.MasterPage
    Dim assembly As System.Reflection.Assembly = System.Reflection.Assembly.GetExecutingAssembly()
    Dim fvi As System.Diagnostics.FileVersionInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location)
    Dim version As String = fvi.FileVersion
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblversion.Text = version
        If Not IsPostBack Then
            ' Update LastActive saat halaman dimuat (hanya jika sudah login)
            Dim userCookie As HttpCookie
            If Request.Cookies("UserLogin_inv") IsNot Nothing Then
                'Response.Cookies("UserLogin_inv")("LastActive") = Now.ToString("yyyyMMddHHmmss")
                'Response.Cookies("UserLogin_inv").Expires = DateTime.Now.AddMinutes(10)
                userCookie = New HttpCookie("UserLogin_inv")  ' BUAT BARU!
                If Session("username") Is Nothing Then
                    Session("username") = Request.Cookies("UserLogin_inv")("userName")
                End If
                ' Ambil nilai lama dari Request (jika ingin dipertahankan)
                Dim oldCookie As HttpCookie = Request.Cookies("UserLogin_inv")
                If oldCookie IsNot Nothing Then
                    userCookie("userName") = oldCookie("userName")
                    userCookie("custid") = oldCookie("custid")
                    userCookie("modulepriv") = oldCookie("modulepriv")
                    userCookie("rptpriv") = oldCookie("rptpriv")
                    userCookie("modul") = oldCookie("modul")
                    userCookie("rptf") = oldCookie("rptf")
                    userCookie("invpriv") = oldCookie("invpriv")
                    userCookie("LastActive") = Now.ToString("yyyyMMddHHmmss")
                End If

                userCookie.Expires = Now.AddMinutes(10)
                userCookie.Path = "/"           ' Opsional: agar bisa diakses di semua path
                userCookie.Secure = False       ' True jika pakai HTTPS

                Response.Cookies.Add(userCookie)
            End If
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