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
        ddlItem.SelectedValue = MTid

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
    Protected Sub btnInsert_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnInsert.ServerClick
        insert()
    End Sub

    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnUpdate.ServerClick
        insert()
    End Sub
    Sub hapus()

        ddlTipe.SelectedValue = 0

        txtQty.Text = 0
        txtLosF.Text = 0



        btnUpdate.Visible = False
        btnInsert.Visible = True
    End Sub
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancel.ServerClick

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

        Try
            Dim msg As String = ""
            Dim newLine As String = "\n"

            'RequiredFieldValidator1.Text = ""
            'RequiredFieldValidator5.Text = ""

            If IsNumeric(txtQty.Text) = False Or txtQty.Text = 0 Then
                txtQty.Text = 0
                msg = "Qty Must Be fill" & newLine
                'RequiredFieldValidator1.Text = "Qty Must Be numeric"

                'Exit Sub
            End If

            If IsNumeric(txtLosF.Text) = False Then
                txtLosF.Text = 0
                msg = msg + "Loss Factor Must Be numeric" & newLine
                'RequiredFieldValidator5.Text = "LossFactor Must be numeric"
                'Exit Sub
            End If
            If msg = "" Then
                Crcpdtl.insertRcpDtl(dtlid, custid, MTid, ddlTipe.SelectedValue, ddlItem.SelectedValue,
                                             txtQty.Text, txtLosF.Text, username)
            Else
                notsuccesfull(msg)
                Exit Sub
            End If




            'MsgBox("Data Berhasil Di input Mysql")
            mfunction.CreateCookie("rcpID", MTid)
            Response.Redirect("~/Pages_ReceipeEdit.aspx")

        Catch ex As Exception
            'MsgBox(ex.Message)
        End Try

    End Sub



    Sub notsuccesfull(ByVal msg As String)
        Dim message As String = msg
        Dim sb As New System.Text.StringBuilder()
        ' Dim url As String = "~/Pages_InvTransfer.aspx"
        sb.Append("<script type = 'text/javascript'>")
        sb.Append("window.onload=function(){")
        sb.Append("alert('")
        sb.Append(message)
        sb.Append("')};")
        sb.Append("</script>")
        ClientScript.RegisterClientScriptBlock(Me.GetType(), "Alert", sb.ToString())
    End Sub

End Class
