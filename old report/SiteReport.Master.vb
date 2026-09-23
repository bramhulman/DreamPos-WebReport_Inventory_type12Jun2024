Public Class SiteReport
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

        'If check_UserTypePriv(0, TransPriv()) = True Then
        If modulepriv() <> 0 Then
            'hItemGroup.Visible = False
            'hCondiment.Visible = False
            'hCondGroup.Visible = False
            'hItemMaster.Visible = False
            'hTableSetting.Visible = False
            'hDiscount.Visible = False
            'hPromotion.Visible = False
            'hPaymentMode.Visible = False
            'hDevice.Visible = False
            'hMemberType.Visible = False
            'hStore.Visible = False
            hOutlet.Visible = False
            ' lOutlet.Visible = False
            'hExport.Visible = False
            huseraccess.Visible = False
            ' luseraccess.Visible = False
            'hShift.Visible = False
            'hSetting.Visible = False
            'hSalesType.Visible = False
            'hTableArea.Visible = False
            'tambahan 29 sept 2018, jadi ga bisa clear sales, klo priv User
            'myOtherSet.Visible = False
        End If
    End Sub
    Protected Sub LoginStatus1_LoggingOut(sender As Object, e As LoginCancelEventArgs)
        ' Context.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie)
        FormsAuthentication.SignOut()
    End Sub
End Class