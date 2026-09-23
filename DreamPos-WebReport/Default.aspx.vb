Imports System.IO
Imports System.Data
Imports System.Configuration
Imports System.Net
Imports System.Web.Script.Serialization
'Imports Microsoft.AspNet.Identity  ' BC40056: namespace not found, removed
Public Class _Default
    Inherits System.Web.UI.Page
    Private mfunction As New ModFunction
    'Public WService As New WServices.WebService1
    Dim Nameuser As String
    Dim custid As Integer
    Dim modul As Integer
    Dim ModPriv As Integer
    Dim rptpriv As Integer
    Dim rptf As Integer
    Dim invpriv As Integer
    Public Function icustid() As Integer
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")

        If cookie Is Nothing Then
            Return -1
            Response.Redirect("~/Default.aspx", False)
        Else
            Return cookie("Custid")
        End If

    End Function
    Public Function susername() As String
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        If cookie Is Nothing Then
            Return ""
        Else
            Return cookie("userName")
        End If


    End Function
    Public Function modulepriv() As Integer
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        If cookie Is Nothing Then
            Return -1
        Else
            Return cookie("modulepriv")
        End If


    End Function
    Public Function imodul() As Integer
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        If cookie Is Nothing Then
            Return -1
        Else
            Return cookie("modul")
        End If


    End Function
    Public Function irptpriv() As Integer
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        If cookie Is Nothing Then
            Return -1
        Else
            Return cookie("rptpriv")
        End If


    End Function

    Public Function irptf() As Integer
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        If cookie Is Nothing Then
            Return -1
        Else
            Return cookie("rptf")
        End If


    End Function
    Public Function inv_priv() As Integer
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        If cookie Is Nothing Then
            Return -1
        Else
            Return cookie("invpriv")
        End If


    End Function
    Protected Sub Page_Init()
        ' Handle Instant Logout
        If Request.QueryString("logout") = "true" Or Request.Headers("X-Logout") = "true" Or Request.QueryString("logout") = "timeout" Then
            FormsAuthentication.SignOut() ' Clear Forms Auth
            Session.Clear()
            Session.Abandon()
            
            ' Clear Custom Cookies
            If Request.Cookies("UserLogin_inv") IsNot Nothing Then
                Dim myCookie As New HttpCookie("UserLogin_inv")
                myCookie.Expires = DateTime.Now.AddDays(-1)
                Response.Cookies.Add(myCookie)
            End If
            
            ' Clear LastActive
            If Request.Cookies("LastActive") IsNot Nothing Then
                 Dim laCookie As New HttpCookie("LastActive")
                 laCookie.Expires = DateTime.Now.AddDays(-1)
                 Response.Cookies.Add(laCookie)
            End If

            If Request.QueryString("logout") = "timeout" Then
                Response.Redirect("~/Default.aspx?logout=timeout_clean", True)
            Else
                Response.Redirect("~/Default.aspx", True) ' Redirect cleanly back to login
            End If
        End If
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim sReturnUrl As String = Request.QueryString("ReturnUrl")
        Dim sLogout As String = Request.QueryString("logout")
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        Dim sCookieUser As String = If(cookie IsNot Nothing, If(cookie("userName"), cookie("username")), "NO_COOKIE")
        Dim sSessUser As String = If(Session("username") IsNot Nothing, Session("username").ToString(), "NO_SESSION")

        If Not String.IsNullOrEmpty(sReturnUrl) OrElse Not String.IsNullOrEmpty(sLogout) Then
            mfunction.errLog("Default.aspx Page_Load: ReturnUrl=" & sReturnUrl & ", logout=" & sLogout & ", cookieUser=" & sCookieUser & ", sessUser=" & sSessUser, "Default.aspx:Page_Load")
        End If

        If Request.QueryString("logout") = "timeout_clean" Then
            messagesuccesfull("Sesi Anda telah berakhir karena tidak aktif selama 5 menit.", False)
            Exit Sub
        End If
        If Request.QueryString("login") IsNot Nothing Then
            mfunction.errLog("session login nothing", "login=success")
            Dim slogin As String = Request.QueryString("login").ToString
            mfunction.errLog("request login", "login=" + slogin)

            Dim sServerHash As String = GenerateSHA512String(Date.Now().ToString("ddMMMyyyyHH"))
            Dim sClientHash As String = Server.UrlDecode(Request.QueryString("time").ToString)
            mfunction.errLog("hash compare", "server=" & sServerHash.Substring(0, 10) & "... client=" & sClientHash.Substring(0, Math.Min(10, sClientHash.Length)) & "...")
            
            If sServerHash = sClientHash Then
                mfunction.errLog("process querystring", "make cookies")
                Nameuser = Server.UrlDecode(Request.QueryString("username").ToString)
                mfunction.errLog("username", Nameuser)
                Integer.TryParse(Server.UrlDecode(Request.QueryString("custid").ToString), custid)
                mfunction.errLog("custid", custid)
                Integer.TryParse(Server.UrlDecode(Request.QueryString("modulepriv").ToString), ModPriv)
                mfunction.errLog("modulepriv", ModPriv)
                Integer.TryParse(Server.UrlDecode(Request.QueryString("rptpriv").ToString), rptpriv)
                mfunction.errLog("rptpriv", rptpriv)
                Integer.TryParse(Server.UrlDecode(Request.QueryString("modul").ToString), modul)
                mfunction.errLog("modul", modul)
                Integer.TryParse(Server.UrlDecode(Request.QueryString("rptf").ToString), rptf)
                mfunction.errLog("rptf", rptf)
                Integer.TryParse(Server.UrlDecode(Request.QueryString("invpriv").ToString), invpriv)
                mfunction.errLog("invpriv", invpriv)

                ' Centralized cookie creation
                loadcookies()

                mfunction.errLog("end process querystring", "make cookies")
                FormsAuthentication.SetAuthCookie(Nameuser, True)
                mfunction.errLog("setauthcookie", Nameuser)
                Session("username") = Nameuser
                Session("custid") = custid
                
                If Not String.IsNullOrEmpty(sReturnUrl) Then
                    Response.Redirect(sReturnUrl, False)
                Else
                    Response.Redirect("~/Inventoryhome.aspx", False)
                End If
                mfunction.errLog("login", "to home")
            Else
                mfunction.errLog("hash FAILED", "server time=" & Date.Now().ToString("ddMMMyyyyHH") & " serverHash=" & sServerHash.Substring(0, 10) & " clientHash=" & sClientHash.Substring(0, Math.Min(10, sClientHash.Length)))
                messagesuccesfull("Login session expired, please login again.", False)
            End If

        ElseIf Not IsPostBack AndAlso Request.IsAuthenticated AndAlso Not String.IsNullOrEmpty(mHelper.Username()) Then
            ' Jika sudah login dan membuka Default.aspx tanpa aksi logout, langsung arahkan ke tujuan
            If Not String.IsNullOrEmpty(sReturnUrl) Then
                Response.Redirect(sReturnUrl, False)
            Else
                Response.Redirect("~/Inventoryhome.aspx", False)
            End If
        End If

    End Sub
    Sub loadcookies(Optional ByVal bStatus As Boolean = True)
        Dim userCookie As HttpCookie

        If bStatus Then
            userCookie = New HttpCookie("UserLogin_inv")
            userCookie("userName") = Nameuser
            userCookie("username") = Nameuser
            userCookie("custid") = custid.ToString()
            userCookie("Custid") = custid.ToString()
            userCookie("modulepriv") = ModPriv.ToString()
            userCookie("rptpriv") = rptpriv.ToString()
            userCookie("modul") = modul.ToString()
            userCookie("rptf") = rptf.ToString()
            userCookie("invpriv") = invpriv.ToString()
            userCookie("LastActive") = Now.ToString("yyyyMMddHHmmss")
        Else
            userCookie = New HttpCookie("UserLogin_inv")
            Dim oldCookie As HttpCookie = Request.Cookies("UserLogin_inv")
            If oldCookie IsNot Nothing Then
                userCookie("userName") = If(oldCookie("userName"), oldCookie("username"))
                userCookie("username") = userCookie("userName")
                userCookie("custid") = If(oldCookie("custid"), oldCookie("Custid"))
                userCookie("Custid") = userCookie("custid")
                userCookie("modulepriv") = oldCookie("modulepriv")
                userCookie("rptpriv") = oldCookie("rptpriv")
                userCookie("modul") = oldCookie("modul")
                userCookie("rptf") = oldCookie("rptf")
                userCookie("invpriv") = oldCookie("invpriv")
            End If
            userCookie("LastActive") = Now.ToString("yyyyMMddHHmmss")
        End If

        ' Set expire & tambahkan ke response
        If RememberMe.Checked Then
            userCookie.Expires = Now.AddDays(30) ' Remember for 30 days
        Else
            userCookie.Expires = Now.AddMinutes(60) ' Default session (60 mins)
        End If
        
        userCookie.Path = "/"           ' Opsional: agar bisa diakses di semua path
        userCookie.Secure = False       ' True jika pakai HTTPS

        Response.Cookies.Set(userCookie)

    End Sub

    Sub hapus()
        txtUser.Enabled = True
        'txtUser.Text = ""
        txtPasword.Text = ""
    End Sub

    Sub messagesuccesfull(ByVal msg As String, Optional ByVal bsuccess As Boolean = False)

        Dim message As String = msg
        Dim url As String
        If bsuccess Then
            url = "../Default"
        Else
            url = "Default.aspx"
        End If

        Dim script As String = "window.onload = function(){ alert('"
        script += message
        script += "');"
        script += "window.location = '"
        script += url
        script += "'; }"
        ClientScript.RegisterStartupScript(Me.GetType(), "Redirect", script, True)

    End Sub

    Protected Sub LogIn(ByVal sender As Object, ByVal e As EventArgs) '(sender As Object, e As EventArgs)
        Dim ds As DataSet
        Dim userName As String = txtUser.Text
        Dim clsLoginData As New ClsLogin
        Dim c_cmd As New Cls_command
        Dim sbrowser As String
        Try
            If txtUser.Text = "" Or txtPasword.Text = "" Then
                messagesuccesfull("User / Password can not be blank", False)

                'Exit Sub
            End If
            'If IsValid Then

            'ds = WService.getLogin(txtuserid.Text, txtpassword.Text)
            sbrowser = HttpContext.Current.Request.UserAgent.ToString
            If sbrowser.Contains("MSIE") OrElse sbrowser.Contains("Trident") Then
                messagesuccesfull("Please use Chrome Browser", False)

                Exit Sub
            ElseIf sbrowser.Contains("Chrome") AndAlso Not sbrowser.Contains("Edg") Then
                'Response.Write("Browser yang digunakan adalah Google Chrome.")
            Else
                messagesuccesfull("Please use Chrome Browser", False)

                Exit Sub
            End If
            ds = clsLoginData.getLogin(txtUser.Text, txtPasword.Text)
            If ds.Tables(0).Rows.Count > 0 Then

                'kalau masih ada proses jadi looping terus
                'count(custid) ... invflag = 0 
                'OPTIMIZED: Single query to get closing info
                Dim dtClosing As DataTable = c_cmd.ReturnDataset("SELECT createdate FROM cld_inv_closing WHERE custid = " & ds.Tables(0).Rows(0)("custid") & " AND invdate = '" & Date.Now.ToString("yyyy-MM-dd") & "' AND invflag = 0 LIMIT 1").Tables(0)
                
                If dtClosing.Rows.Count > 0 Then
                     Dim createDate As DateTime = Convert.ToDateTime(dtClosing.Rows(0)("createdate"))
                     Dim minutesDiff As Double = (Date.Now - createDate).TotalMinutes

                     If minutesDiff > 30 Or minutesDiff < -30 Then
                        FormsAuthentication.SetAuthCookie(userName, False)
                        messagesuccesfull("Closing process (" & Math.Round(minutesDiff) & " mins), Please login again.", False)
                        hapus()
                     End If

                    'Else


                Else
                    Nameuser = ds.Tables(0).Rows(0)("usr").ToString
                    custid = ds.Tables(0).Rows(0)("custid")
                    modul = ds.Tables(0).Rows(0)("modul").ToString
                    ModPriv = ds.Tables(0).Rows(0)("modulepriv").ToString
                    rptpriv = ds.Tables(0).Rows(0)("rptpriv").ToString
                    rptf = ds.Tables(0).Rows(0)("rptf").ToString
                    invpriv = ds.Tables(0).Rows(0)("invpriv").ToString
                    'StoreID = ds.Tables(0).Rows(0)("StoreID").ToString
                    'OutletID = ds.Tables(0).Rows(0)("OutletID").ToString
                    loadcookies()

                    ' Set session
                    'Session("UserID") = userId
                    Session("LoginLock") = DateTime.Now.Ticks.ToString()

                    ' Update LastActive di cookie
                    'Response.Cookies("UserLogin_inv")("LastActive") = Now.ToString("yyyyMMddHHmmss")

                    FormsAuthentication.SetAuthCookie(userName, RememberMe.Checked)
                    Session("username") = Nameuser
                    Session("custid") = custid

                    Dim sReturn As String = Request.QueryString("ReturnUrl")
                    If Not String.IsNullOrEmpty(sReturn) Then
                        Response.Redirect(sReturn, False)
                    Else
                        Response.Redirect("~/Inventoryhome.aspx", False)
                    End If



                    'mp1.Enabled = True

                    'mp1.Show()
                End If



            Else
                    FormsAuthentication.SetAuthCookie(userName, False)
                'messagesuccesfull("Userid NOT VALID !", False)
                messagesuccesfull("Your user or password is incorrect.", False)
                'FailureText.Text = "Userid NOT VALID !"
                hapus()

            End If

            'End If
        Catch ex As Exception
            mfunction.errLog(Err.Description, "Default:Login")
            Err.Clear()
        End Try
    End Sub




    Protected Sub Clear(sender As Object, e As EventArgs)
        Try
            hapus()

        Catch ex As Exception
            mfunction.errLog(Err.Description, "Default:Clear")
            Err.Clear()
        End Try
    End Sub



End Class
