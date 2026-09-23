Public Class SiteInventoryHome
    Inherits System.Web.UI.MasterPage
    Private mfunction As New ModFunction
    Dim assembly As System.Reflection.Assembly = System.Reflection.Assembly.GetExecutingAssembly()
    Dim fvi As System.Diagnostics.FileVersionInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location)
    Dim version As String = fvi.FileVersion

    Public Function ModPrv() As Integer
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")

        If cookie Is Nothing Then
            Return -1
        Else
            Return cookie("modulepriv")
        End If
    End Function
    Public Function custid() As Integer
        Dim icust As Integer = 0
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")

        If cookie Is Nothing Then
            Response.Redirect("~/Default.aspx", False)

        Else
            icust = cookie("custid")
        End If
        Return icust
    End Function
    Public Function username() As String
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        If cookie Is Nothing Then
            Return ""
        Else
            Return cookie("userName")
        End If

    End Function
    Public Function rptpriv() As Integer
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")

        If cookie Is Nothing Then
            Return -1
        Else
            Return cookie("rptpriv")
        End If

    End Function

    Public Function modul() As Integer
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")


        If cookie Is Nothing Then
            Return -1
        Else
            Return cookie("modul")
        End If


    End Function

    Public Function InvPriv() As Integer
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")

        If cookie Is Nothing Then
            Return -1
        Else
            Return cookie("invpriv")
        End If
    End Function
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            'If custid() = -1 Then
            '    Exit Sub
            'End If

            'If Session("username") Is Nothing Or custid() < 1 Then
            '    Session.Clear()
            '    Session.Abandon()
            '    Session.RemoveAll()
            '    FormsAuthentication.RedirectToLoginPage()
            '    Exit Sub
            'End If
            
            If Not IsPostBack Then
                If modul() >= usrModul.moduleInv Then
                    Dim userInvPriv As Integer = InvPriv()
                    If ModPrv() = usrModulepriv.moduleuser Then
                        'If ModPrv() And usrModulepriv.moduleuser = usrModulepriv Then

                        'Dim userInvPriv As Integer = InvPriv()


                        If userInvPriv = 0 Then


                            LItemMaster.Visible = False
                            LStockGroup.Visible = False
                            LStockMaster.Visible = False
                            lcostcentre.Visible = False
                            LSupplier.Visible = False
                            LReceipe.Visible = False
                            LTransfer.Visible = False
                            LReceive.Visible = False
                            LReturn.Visible = False
                            LOrderS.Visible = False
                            LRtnWH.Visible = False
                            m_webreport.Visible = False
                            LInvoicePayment.Visible = False
                            lkstock.Visible = False
                            LCCSupplier.Visible = False
                            LCPO.Visible = False
                            LPR.Visible = False
                            'sementara dimatikan sampai beres semua untuk access
                            Session.Clear()
                            Session.Abandon()
                            Session.RemoveAll()
                            FormsAuthentication.RedirectToLoginPage()
                            Exit Sub

                        Else

                            For x = 0 To 25
                                setAktifButton(x, check_usrInvPriv(x, userInvPriv))
                            Next

                            'lown.Visible = False

                            If ModPrv() = 1 Then

                                LOrderS.Visible = False
                            End If
                            If (username() = My.Settings.admin Or username() = "support") Then
                            Else
                                Lchangedcost.Visible = False
                            End If
                        End If


                    Else
                        'LItemMaster.Visible = True
                        'LStockGroup.Visible = True
                        'LStockMaster.Visible = True
                        'LCostCentre.Visible = True
                        'LSupplier.Visible = True
                        'LReceipe.Visible = True
                        'LTransfer.Visible = True
                        'LReceive.Visible = True
                        'LReturn.Visible = True

                        If userInvPriv = 0 Then

                            LItemMaster.Visible = False
                            LStockGroup.Visible = False
                            LStockMaster.Visible = False
                            lcostcentre.Visible = False
                            LSupplierGroup.Visible = False
                            LReceipe.Visible = False
                            LTransfer.Visible = False
                            LReceive.Visible = False
                            LReturn.Visible = False
                            LOrderS.Visible = False
                            LRtnWH.Visible = False
                            m_webreport.Visible = False
                            LInvoicePayment.Visible = False
                            lkstock.Visible = False
                            LCCSupplier.Visible = False
                            LCPO.Visible = False
                            LPR.Visible = False
                            'sementara dimatikan sampai beres semua untuk access
                            Session.Clear()
                            Session.Abandon()
                            Session.RemoveAll()
                            FormsAuthentication.RedirectToLoginPage()
                            Exit Sub

                        Else

                            For x = 0 To 25
                                setAktifButton(x, check_usrInvPriv(x, userInvPriv))
                            Next
                            'If ModPrv() And usrModulepriv.moduleuser = usrModulepriv Then
                            '    LOrderS.Visible = False
                            'End If
                            'If ModPrv() = 1 Then

                            '    LOrderS.Visible = False
                            'End If
                            If (username() = My.Settings.admin Or username() = "support") Then
                            Else
                                Lchangedcost.Visible = False
                            End If

                        End If


                    End If
                    ' m_webreport.Visible = True
                    
                    ' Update LastActive saat halaman dimuat (hanya jika sudah login)
                    Dim userCookie As HttpCookie
                    If Request.Cookies("UserLogin_inv") IsNot Nothing Then
                        'Response.Cookies("UserLogin_inv")("LastActive") = Now.ToString("yyyyMMddHHmmss")
                        'Response.Cookies("UserLogin_inv").Expires = DateTime.Now.AddMinutes(10)
                        userCookie = New HttpCookie("UserLogin_inv")  ' BUAT BARU!
                        If Session("username") Is Nothing Then
                            Session("username") = username()
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
                Else

                    'Response.Redirect(My.Settings.webreport & "/Default.aspx")
                    Dim script As String = "window.onload = function(){ alert('"
                    script += "Please Login with Inventory modul Priviledge"
                    script += "');"
                    script += "window.location = '"
                    script += My.Settings.webreport + "/Default.aspx"
                    script += " '; }"
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Redirect", script, True)
                End If
                
                lblversion.Text = version
            End If
            
        Catch ex As Exception
            errLog(Err.Description, "SiteInventoryHome:Page_Load")
            Err.Clear()
        End Try


    End Sub
    Protected Sub LoginStatus1_LoggingOut(sender As Object, e As LoginCancelEventArgs)
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        ' Context.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie)
        If Request.Cookies("UserLogin_inv") IsNot Nothing Then
            FormsAuthentication.SignOut()
            Request.Cookies("UserLogin_inv").Expires = DateTime.Now.AddDays(-1)
            Session.Clear()
            Session.Abandon()
            Response.Cookies.Add(cookie)
        End If

        'cookie.Expires = DateTime.Now.AddDays(-1D)

    End Sub
    Private Sub setAktifButton(ByVal iUrut As Integer, ByVal bActive As Boolean)

        If iUrut = 0 Then
            LItemMaster.Visible = bActive

        ElseIf iUrut = 1 Then
            LStockGroup.Visible = bActive

        ElseIf iUrut = 2 Then
            LStockMaster.Visible = bActive

        ElseIf iUrut = 3 Then
            lcostcentre.Visible = bActive

        ElseIf iUrut = 4 Then
            LSupplierGroup.Visible = bActive

        ElseIf iUrut = 5 Then
            LReceipe.Visible = bActive

        ElseIf iUrut = 6 Then
            LOrderW.Visible = bActive

        ElseIf iUrut = 7 Then
            LOrderS.Visible = bActive

        ElseIf iUrut = 8 Then
            LBacth.Visible = bActive

        ElseIf iUrut = 9 Then
            LTransfer.Visible = bActive

        ElseIf iUrut = 10 Then
            LRWaste.Visible = bActive

        ElseIf iUrut = 11 Then
            LReceive.Visible = bActive

        ElseIf iUrut = 12 Then
            LReturn.Visible = bActive

        ElseIf iUrut = 13 Then
            LOpname.Visible = bActive

        ElseIf iUrut = 14 Then
            LCalculate.Visible = bActive

        ElseIf iUrut = 15 Then
            LExpense.Visible = bActive

        ElseIf iUrut = 16 Then
            LInvoice.Visible = bActive
        ElseIf iUrut = 17 Then
            LRtnWH.Visible = bActive
        ElseIf iUrut = 18 Then
            m_webreport.Visible = bActive
        ElseIf iUrut = 19 Then
            LInvoicePayment.Visible = bActive
        ElseIf iUrut = 20 Then
            lkstock.Visible = bActive
        ElseIf iUrut = 21 Then
            LCCSupplier.Visible = bActive
        ElseIf iUrut = 24 Then
            LCPO.Visible = bActive
        ElseIf iUrut = 25 Then
            LPR.Visible = bActive
        End If

    End Sub


    Private Sub aCreate_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles aCreate.ServerClick
        mfunction.CreateCookie("wType", "1")
        Response.Redirect("~/Pages_InvOrderW.aspx", False)
    End Sub

    Private Sub aReceive_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles aReceive.ServerClick
        mfunction.CreateCookie("wType", "2")
        Response.Redirect("~/Pages_InvOrderW.aspx", False)
    End Sub
    Private Sub astockmaster_ServerClick(sender As Object, e As EventArgs) Handles astockmaster.ServerClick
        Response.Redirect("~/Pages_Stk.aspx", False)
    End Sub
    Private Sub astocklimit_ServerClick(sender As Object, e As EventArgs) Handles astocklimit.ServerClick
        Response.Redirect("~/Pages_limitStk.aspx", False)
    End Sub
    Private Sub arawwaste_ServerClick(sender As Object, e As EventArgs) Handles arawwaste.ServerClick
        Response.Redirect("~/Pages_InvRwaste.aspx", False)
    End Sub
    'Private Sub aclosingstk_ServerClick(sender As Object, e As EventArgs) Handles aclosingstk.ServerClick
    '    Response.Redirect("~/Pages_InvClosingStock.aspx", False)
    'End Sub
    Protected Sub webreporthome_ServerClick(ByVal sender As Object, ByVal e As EventArgs)

        'Dim userCookie As HttpCookie = New HttpCookie("UserLogin")

        'userCookie("userName") = username()
        'userCookie("custid") = custid()
        'userCookie("modulepriv") = modulepriv()
        'userCookie("rptpriv") = rptpriv()
        'userCookie("modul") = modul()
        'userCookie("rptf") = rptf()

        Session("username") = Request.Cookies("UserLogin_inv")("username")


        Dim strusername As String = Server.UrlEncode(Request.Cookies("UserLogin_inv")("userName").ToString)
        Dim strcustid As String = Server.UrlEncode(Request.Cookies("UserLogin_inv")("custid").ToString)
        Dim strmodulepriv As String = Server.UrlEncode(Request.Cookies("UserLogin_inv")("modulepriv").ToString)
        Dim strrptpriv As String = Server.UrlEncode(Request.Cookies("UserLogin_inv")("rptpriv").ToString)
        Dim strmodule As String = Server.UrlEncode(Request.Cookies("UserLogin_inv")("modul").ToString)
        Dim strrptf As String = Server.UrlEncode(Request.Cookies("UserLogin_inv")("rptf").ToString)
        Dim strinvpriv As String = Server.UrlEncode(Request.Cookies("UserLogin_inv")("invpriv").ToString)
        Dim strtime As String = Server.UrlEncode(GenerateSHA512String(Date.Now().ToString("ddMMMyyyyHH")))
        'userCookie.Expires = Now.AddHours(1)
        'Response.Cookies.Add(userCookie)
        Dim urls As String = My.Settings.webreport + "?login=" + Session("username").ToString + "&username=" + strusername + "&custid=" + strcustid + "&modulepriv=" + strmodulepriv + "&rptpriv=" + strrptpriv + "&modul=" + strmodule + "&rptf=" + strrptf + "&invpriv=" + strinvpriv + "&time=" + strtime
        mfunction.errLog("url", urls)
        Response.Redirect(My.Settings.webreport + "?login=" + Session("username").ToString + "&username=" + strusername + "&custid=" + strcustid + "&modulepriv=" + strmodulepriv + "&rptpriv=" + strrptpriv + "&modul=" + strmodule + "&rptf=" + strrptf + "&invpriv=" + strinvpriv + "&time=" + strtime)
    End Sub

    Private Sub LSupplier_ServerClick(sender As Object, e As EventArgs) Handles LSupplier.ServerClick
        Response.Redirect("~/Pages_Supplier.aspx", False)
    End Sub

    Private Sub LCCSupplier_ServerClick(sender As Object, e As EventArgs) Handles LCCSupplier.ServerClick
        Response.Redirect("~/CCoutlet.aspx", False)
    End Sub

    Private Sub areplacestock_ServerClick(sender As Object, e As EventArgs) Handles areplacestock.ServerClick
        Response.Redirect("~/Pages_replaceStk.aspx", False)
    End Sub

    Private Sub achangedcost_ServerClick(sender As Object, e As EventArgs) Handles achangedcost.ServerClick
        Response.Redirect("~/ChangedCost.aspx", False)
    End Sub

    Private Sub aListWaste_ServerClick(sender As Object, e As EventArgs) Handles aListWaste.ServerClick
        Response.Redirect("~/Pages_ListWaste.aspx", False)
    End Sub

    Private Sub aListOpname_ServerClick(sender As Object, e As EventArgs) Handles aListOpname.ServerClick
        Response.Redirect("~/Pages_ListOpname.aspx", False)
    End Sub

    Private Sub aStockOpname_ServerClick(sender As Object, e As EventArgs) Handles aStockOpname.ServerClick
        Response.Redirect("~/Pages_StockOpname.aspx", False)
    End Sub

    Private Sub aCustomer_ServerClick(sender As Object, e As EventArgs) Handles aCustomer.ServerClick
        Response.Redirect("~/Pages_CustomerPO.aspx", False)
    End Sub

    'Private Sub aPayment_ServerClick(sender As Object, e As EventArgs) Handles aPayment.ServerClick
    '    Response.Redirect("~/Pages_StkGroup.aspx", False)
    'End Sub

    Private Sub aPO_ServerClick(sender As Object, e As EventArgs) Handles aPO.ServerClick
        Response.Redirect("~/Pages_CPO.aspx", False)
    End Sub

    Private Sub ausageList_ServerClick(sender As Object, e As EventArgs) Handles ausageList.ServerClick
        Response.Redirect("~/PreOrder_Usage.aspx", False)
    End Sub

    Private Sub acreaterequest_ServerClick(sender As Object, e As EventArgs) Handles acreaterequest.ServerClick
        mfunction.CreateCookie("wType", "1")
        Response.Redirect("~/PurchaseRequest.aspx", False)
    End Sub

    'Private Sub awastebystk_ServerClick(sender As Object, e As EventArgs) Handles awastebystk.ServerClick
    '    Response.Redirect("~/Pages_wastelist.aspx", False)
    'End Sub
End Class