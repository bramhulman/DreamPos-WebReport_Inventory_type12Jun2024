Public Class Inventory
    Inherits System.Web.UI.MasterPage
    Private mfunction As New ModFunction
    Public Function cust() As Integer
        Dim cookie As HttpCookie = Request.Cookies("UserName")

        If cookie Is Nothing Then
            Return -1
        Else
            Return cookie("Custid")
        End If

    End Function

   Public Function ModPrv() As Integer
        Dim cookie As HttpCookie = Request.Cookies("UserName")
        Return cookie("ModPriv")
    End Function

    Public Function modul() As Integer
        Dim cookie As HttpCookie = Request.Cookies("UserName")
        Return cookie("Modul")
    End Function


    Public Function InvPriv() As Integer
        Dim cookie As HttpCookie = Request.Cookies("UserName")
        Return cookie("invpriv")
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If cust() = -1 Then
            Exit Sub
        End If

        If modul() = mfunction.usrModul.moduleInv Then
            ' NavigationMenu.Items.Remove(NavigationMenu.FindItem("FeedBack"))

        ElseIf modul() = mfunction.usrModul.moduleCRM Then
            ' NavigationMenu.Items.Remove(NavigationMenu.FindItem("FeedBack"))
            NavigationMenu.Items.Remove(NavigationMenu.FindItem("Inventory"))


        ElseIf modul() = mfunction.usrModul.modulereport Then
            NavigationMenu.Items.Remove(NavigationMenu.FindItem("Other"))
            '  NavigationMenu.Items.Remove(NavigationMenu.FindItem("FeedBack"))
            NavigationMenu.Items.Remove(NavigationMenu.FindItem("Inventory"))


        End If

        If ModPrv() And Modulepriv.moduleuser = Modulepriv.moduleuser Then


            Dim userInvPriv As Integer = InvPriv()


            If userInvPriv = 0 Then


                LItemMaster.Visible = False
                LStockGroup.Visible = False
                LStockMaster.Visible = False
                LCostCentre.Visible = False
                LSupplier.Visible = False
                LReceipe.Visible = False
                LTransfer.Visible = False
                'LReceive.Visible = False
                LReturn.Visible = False
                LOrderS.Visible = False

            Else

                For x = 0 To 16
                    setAktifButton(x, mfunction.check_usrInvPriv(x, userInvPriv))
                Next


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

        End If

    End Sub


    Private Sub setAktifButton(ByVal iUrut As Integer, ByVal bActive As Boolean)

        If iUrut = 0 Then
            LItemMaster.Visible = bActive

        ElseIf iUrut = 1 Then
            LStockGroup.Visible = bActive

        ElseIf iUrut = 2 Then
            LStockMaster.Visible = bActive

        ElseIf iUrut = 3 Then
            LCostCentre.Visible = bActive

        ElseIf iUrut = 4 Then
            LSupplier.Visible = bActive

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
            LSOpname.Visible = bActive

        ElseIf iUrut = 14 Then
            LCalculate.Visible = bActive

        ElseIf iUrut = 15 Then
            LExpense.Visible = bActive

        ElseIf iUrut = 16 Then
            LInvoice.Visible = bActive
        End If

    End Sub


    Private Sub aCreate_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles aCreate.ServerClick
        mfunction.CreateCookie("wType", "1")
        Response.Redirect("~/Pages_InvOrderW.aspx")
    End Sub

    Private Sub aReceive_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles aReceive.ServerClick
        mfunction.CreateCookie("wType", "2")
        Response.Redirect("~/Pages_InvOrderW.aspx")
    End Sub

  
End Class