Imports System.Threading
Imports System.Net
Imports System.Text
Imports System.IO

Public Class Pages_ReceipeDtlEdit
    Inherits System.Web.UI.Page
    Private sNameuser As String
    Private Userid As Integer
    Private icustid As Integer
    Private imodul As Integer
    Private iModPriv As Integer
    Private irptpriv As Integer
    Private irptf As Integer
    Private mfunction As New ModFunction
    Private MTid As Integer
    Private dtlid As Integer
    Private sUnit As String
    'Private ccid As Integer
    Private imgByte As Byte() = Nothing

    Private CRcp As New Cls_IRcp
    Private cStock As New clsInventory_stk
    Private ciStock As New Cls_IStk
    Private CIcc As New Cls_ICc
    Private Crcpdtl As New Cls_IRcpDtl
    Dim cInv As New clsInventory

    'Private transpriv As Integer
    Public Function modulepriv() As Integer
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        Return cookie("modulepriv")

    End Function
    Public Function custid() As Integer
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")

        If cookie Is Nothing Then
            Return -1
        Else
            Return cookie("Custid")
        End If

    End Function

    Public Function rptpriv() As Integer
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")

        Return cookie("rptpriv")

    End Function
    Public Function modul() As Integer
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        Return cookie("modul")

    End Function
    Public Function rptf() As Integer
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")

        Return cookie("rptf")

    End Function
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        'If Not (User.Identity.IsAuthenticated) Or custid() = -1 Then
        '    FormsAuthentication.RedirectToLoginPage()
        '    Exit Sub
        'End If
        If Session("username") Is Nothing Or custid() < 1 Then
            Session.Clear()
            Session.Abandon()
            Session.RemoveAll()
            FormsAuthentication.RedirectToLoginPage()
            Exit Sub
        End If
        MTid = Request.Cookies("rcpID").Value
        dtlid = Request.Cookies("rcpIdrcp").Value
        sUnit = Request.Cookies("rcpunt").Value
        Dim tips As Integer = Request.Cookies("rcptipe").Value
        Try

            If Request.Cookies("UserLogin_inv") IsNot Nothing Then
                sNameuser = username()
                icustid = custid()
                imodul = modul()
                iModPriv = modulepriv()
                irptpriv = rptpriv()
                irptf = rptf()
                If IsPostBack = False Then
                    loadRcpdtl(tips)

                    editlist(dtlid)
                End If
            Else
                Dim script As String = "window.onload = function(){ alert('"
                script += "Please Login"
                script += "');"
                script += "window.location = '"
                script += "../Default"
                script += " '; }"
                ClientScript.RegisterStartupScript(Me.GetType(), "Redirect", script, True)
            End If




            'yestr = CDate(Now.AddDays(-1)).ToString("yyyyMMdd")

        Catch ex As Exception

        End Try
    End Sub

    Protected Sub Page_unLoad(sender As Object, e As EventArgs) Handles Me.Unload
    End Sub

    Public Function username() As String
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        Return cookie("userName")

    End Function

    Sub loadRcpdtl(ByVal tipe As Integer)
        'ddlItem.Items.Clear()
        'ddlItem.Text = ""
        Dim ds As New DataSet
        'If dtlid = 0 Then
        '    ds = WService.getRcpItemDtlAll(cust, MTid, ddlTipe.SelectedValue)
        'Else
        ds = Crcpdtl.SelectDataRcpItemDtl(custid, MTid, dtlid, tipe)
        ' End If

        If ds IsNot Nothing AndAlso ds.Tables.Count > 0 Then
            ddlItem.DataSource = ds
        Else
            ddlItem.DataSource = Nothing
        End If
        ddlItem.DataValueField = "Id"
        ddlItem.DataTextField = "Name"
        ddlItem.DataBind()
        'The selected item comes from editlist, not the parent recipe ID.
        ddlItem.SelectedValue = "0"

    End Sub
    Sub editlist(ByVal id As Integer)


        Dim ds As New DataSet
        ds = Crcpdtl.SelectDataset_rcpdtlEdit(id, custid)
        If ds.Tables(0).Rows.Count = 0 Then
            Exit Sub
        End If
        ddlTipe.SelectedValue = ds.Tables(0).Rows(0).Item("tipe")
        ddlItem.SelectedValue = ds.Tables(0).Rows(0).Item("kodeid")
        txtQty.Text = ds.Tables(0).Rows(0).Item("qty")
        txtLosF.Text = ds.Tables(0).Rows(0).Item("Lossf")
        lblQty.Text = sUnit

        btnUpdate.Visible = True
        btnInsert.Visible = False
    End Sub
    Protected Sub btnInsert_Click(ByVal sender As Object, ByVal e As EventArgs)
        insert()
    End Sub

    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As EventArgs)
        insert()
    End Sub
    Sub hapus()

        ddlTipe.SelectedValue = 0

        txtQty.Text = 0
        txtLosF.Text = 0



        btnUpdate.Visible = False
        btnInsert.Visible = True
    End Sub
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs)

        Try
            hapus()
            mfunction.CreateCookie("rcpIdrcp", 0)
            mfunction.CreateCookie("rcpID", MTid)
            Response.Redirect("~/Pages_ReceipeEdit.aspx")



        Catch ex As Exception
            errLog(Err.Description, "Pages_ReceipeDtlEdit:btnCancel_Click")
            Err.Clear()
        End Try

    End Sub


    Sub insert()
        recipeDetailError.Visible = False
        Page.Validate()
        If Not Page.IsValid Then
            notsuccesfull("Please select a type and an item before saving.")
            Return
        End If

        Dim qty As Decimal
        Dim lossFactor As Decimal
        Dim numberStyle As Global.System.Globalization.NumberStyles =
            Global.System.Globalization.NumberStyles.AllowLeadingSign Or
            Global.System.Globalization.NumberStyles.AllowDecimalPoint
        Dim culture = Global.System.Globalization.CultureInfo.CurrentCulture

        'Do not accept thousands separators: a mistyped decimal must not become a larger quantity.
        If Not Decimal.TryParse(txtQty.Text.Trim(), numberStyle, culture, qty) OrElse qty = 0D Then
            notsuccesfull("Enter a non-zero Qty, for example " & 1.5D.ToString(culture) & ". Do not use thousands separators.")
            txtQty.Focus()
            Return
        End If
        If Not Decimal.TryParse(txtLosF.Text.Trim(), numberStyle, culture, lossFactor) Then
            notsuccesfull("Enter a numeric Loss Factor, for example " & 0.5D.ToString(culture) & ". Do not use thousands separators.")
            txtLosF.Focus()
            Return
        End If

        Try
            Crcpdtl.insertRcpDtl(dtlid, custid, MTid, ddlTipe.SelectedValue, ddlItem.SelectedValue,
                               qty, lossFactor, username)
            mfunction.CreateCookie("rcpID", MTid)
            Response.Redirect("~/Pages_ReceipeEdit.aspx", False)
            Context.ApplicationInstance.CompleteRequest()
        Catch ex As Exception
            errLog(ex.ToString(), "Pages_ReceipeDtlEdit:insert")
            notsuccesfull("The recipe detail could not be saved. Please try again.")
        End Try
    End Sub

    Sub notsuccesfull(ByVal msg As String)
        recipeDetailError.InnerText = msg
        recipeDetailError.Visible = True
    End Sub

End Class
