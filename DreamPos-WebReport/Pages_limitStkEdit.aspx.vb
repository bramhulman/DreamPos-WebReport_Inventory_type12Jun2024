Imports System.Threading
Imports System.Net
Imports System.Text
Imports System.IO

Public Class Pages_limitStkEdit
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
    'Private ccid As Integer
    Private imgByte As Byte() = Nothing

    Private CRcp As New Cls_IRcp
    Private cStock As New clsInventory_stk
    Private ciStock As New Cls_IStk
    Private CIcc As New Cls_ICc
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
        MTid = Request.Cookies("InvStk_ID").Value
        ccid = Request.Cookies("InvStk_cc").Value
        Try

            If Request.Cookies("UserLogin_inv") IsNot Nothing Then
                sNameuser = username()
                icustid = custid()
                imodul = modul()
                iModPriv = modulepriv()
                irptpriv = rptpriv()
                irptf = rptf()
                If IsPostBack = False Then
                    If MTid = 0 And ccid = 0 Then
                        hapus()
                        'loadStkGrp()
                        'loadRcp()
                        loadCc()
                        loadStk()
                    Else
                        'loadStkGrp() 
                        loadCc()
                        loadStk()
                        ddlcc.SelectedValue = ccid
                        ddlstok.SelectedValue = MTid
                        txtNo.Text = ciStock.SelectDataset_StkEdit(ddlstok.SelectedValue, custid).Tables(0).Rows(0).Item("number")
                        editlist()

                        'loadRcp()
                    End If
                    ScriptManager.GetCurrent(Me).RegisterPostBackControl(btnInsert)
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

    Sub hapus()

        'txtName.Text = ""
        txtNo.Text = ""
        'txtUnit1.Text = ""
        'txtUnit2.Text = ""
        'txtUnit3.Text = ""

        txtStkLimit.Text = 0


        'txtFac1.Text = 1
        'txtFac2.Text = 1

        'txtNo.Enabled = True

        btnUpdate.Visible = False
        btnInsert.Visible = True
    End Sub

    Sub editlist()


        Dim ds As New DataSet
        ds = cStock.SelectDataset_limitStkEdit(ddlstok.SelectedValue, ddlcc.SelectedValue, custid)     'WService.getStkEdit(id, cust)
        If ds.Tables(0).Rows.Count = 0 Then
            Exit Sub
        End If

        'txtNo.Text = ds.Tables(0).Rows(0).Item("number")
        'ddlstok.SelectedValue = ds.Tables(0).Rows(0).Item("id")
        'ddlcc.SelectedValue = ds.Tables(0).Rows(0).Item("ccid")
        'txtUnit1.Text = ds.Tables(0).Rows(0).Item("Unit1")
        'txtUnit2.Text = ds.Tables(0).Rows(0).Item("Unit2")
        'txtUnit3.Text = ds.Tables(0).Rows(0).Item("Unit3")
        'txtFac1.Text = ds.Tables(0).Rows(0).Item("Factor1")
        'txtFac2.Text = ds.Tables(0).Rows(0).Item("Factor2")
        'DdlGrpStk.SelectedValue = ds.Tables(0).Rows(0).Item("grpcode")
        'DdlLink.SelectedValue = ds.Tables(0).Rows(0).Item("Blink")

        txtStkLimit.Text = ds.Tables(0).Rows(0).Item("Stklimit")



        'chkbatch.Checked = ds.Tables(0).Rows(0).Item("batch")
        'If chkbatch.Checked = True Then
        '    DdlLink.Enabled = True
        'End If
        'txtTelp.Text = ds.Tables(0).Rows(0).Item("Telp")
        'txtRemarks.Text = ds.Tables(0).Rows(0).Item("remarks")
        'txtotype.Text = ds.Tables(0).Rows(0).Item("otype")
        'TxtCust.Text = ds.Tables(0).Rows(0).Item("CustID")

        btnUpdate.Visible = True
        btnInsert.Visible = False
    End Sub

    Protected Sub btnInsert_Click(ByVal sender As Object, ByVal e As EventArgs) 'Handles btnInsert.Click
        insert()
    End Sub

    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As EventArgs) 'Handles btnUpdate.Click
        insert()
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancel.Click

        Try
            hapus()
            'mfunction.CreateCookie("ID", 0)
            'Dim modfunc As New ModFunction
            mfunction.CreateCookie("InvStk_ID", 0)
            mfunction.CreateCookie("InvStk_cc", 0)

            Response.Redirect("~/Pages_limitStk.aspx")



        Catch ex As Exception
            errLog(Err.Description, "Pages_StkEdit:btnCancel_Click")
            Err.Clear()
        End Try

    End Sub
    'Sub loadStkGrp()
    '    Try

    '        'Dim cookie As HttpCookie = Request.Cookies("UserName")

    '        'Dim Cust As Integer = cookie("CustID")
    '        Dim ds As New DataSet
    '        ds = cStockGrp.SelectDataset_StkGroup(custid())   'WService.getStkGroup(cust())
    '        DdlGrpStk.DataSource = ds
    '        DdlGrpStk.DataValueField = "Code"
    '        DdlGrpStk.DataTextField = "Name"
    '        DdlGrpStk.DataBind()

    '        'DdlGrpStk.DataSource = ds
    '        'DdlGrpStk.DataValueField = "Id"
    '        'DdlGrpStk.DataTextField = "Name"
    '        'DdlGrpStk.DataBind()

    '    Catch ex As Exception
    '        errLog(Err.Description, "Pages_StkEdit:loadStkGrp")
    '        Err.Clear()
    '    End Try

    'End Sub
    'Sub loadRcp()
    '    'Dim cookie As HttpCookie = Request.Cookies("UserName")

    '    'Dim Cust As Integer = cookie("CustID")
    '    Dim ds As New DataSet
    '    ds = CRcp.SelectDataset_rcp(custid)
    '    DdlLink.DataSource = ds
    '    DdlLink.DataValueField = "Id"
    '    DdlLink.DataTextField = "Name"
    '    DdlLink.DataBind()

    'End Sub
    'Protected Sub chkbatch_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkbatch.CheckedChanged
    '    If chkbatch.Checked = True Then
    '        DdlLink.Enabled = True
    '    Else
    '        DdlLink.Enabled = False
    '        DdlLink.SelectedValue = 0
    '    End If
    'End Sub

    Sub insert()



        Try



            cStock.insertlimitStk(0, custid, txtNo.Text, ddlstok.SelectedValue, txtStkLimit.Text, ddlcc.SelectedValue)

            'MsgBox("Data Berhasil Di input Mysql")

            Response.Redirect("~/Pages_limitStk.aspx")

        Catch ex As Exception
            'MsgBox(ex.Message)
        End Try

    End Sub
    'Protected Sub txtFac1_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtFac1.TextChanged

    '    If IsNumeric(txtFac1.Text) = False Then
    '        txtFac1.Text = 0
    '    End If

    '    If txtFac1.Text = "" Then
    '        txtFac1.Text = 0
    '    ElseIf txtFac1.Text < 0 Then
    '        txtFac1.Text = 0
    '    End If
    'End Sub

    'Protected Sub txtFac2_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtFac2.TextChanged


    '    If IsNumeric(txtFac2.Text) = False Then
    '        txtFac2.Text = 0
    '    End If

    '    If txtFac2.Text = "" Then

    '        txtFac2.Text = 0
    '    ElseIf txtFac2.Text < 0 Then
    '        txtFac2.Text = 0
    '    End If
    'End Sub

    'Protected Sub btnlimit_Click(sender As Object, e As EventArgs) Handles btnlimit.Click
    '    Try
    '        mfunction.CreateCookie("ID", MTid)

    '        Response.Redirect("~/Pages_Stk.aspx")
    '    Catch ex As Exception

    '    End Try
    'End Sub
    Sub loadCc()
        ddlcc.Items.Clear()
        ddlcc.SelectedValue = Nothing
        Dim ds As New DataSet
        ds = CIcc.SelectDataset_Icc(custid, username, modulepriv, 0)
        If ds IsNot Nothing AndAlso ds.Tables.Count > 0 Then
            ddlcc.DataSource = ds
        Else
            ddlcc.DataSource = Nothing
        End If
        ddlcc.DataValueField = "Id"
        ddlcc.DataTextField = "Name"
        ddlcc.DataBind()
        If ddlcc.Items.Count > 0 Then
            ddlcc.SelectedIndex = 0
        End If

    End Sub
    Sub loadStk()
        Try

            Dim ds As New DataSet
            ds = ciStock.SelectDataset_StkAll(custid())
            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 Then
                ddlstok.DataSource = ds
            Else
                ddlstok.DataSource = Nothing
            End If
            ddlstok.DataValueField = "id"
            ddlstok.DataTextField = "Name"
            ddlstok.DataBind()



        Catch ex As Exception
            errLog(Err.Description, "Pages_StkEdit:loadStk")
            Err.Clear()
        End Try

    End Sub

    Private Sub ddlstok_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlstok.SelectedIndexChanged
        Dim ds As DataSet = ciStock.SelectDataset_StkEdit(ddlstok.SelectedValue, custid)
        If ds.Tables(0).Rows.Count > 0 Then
            txtNo.Text = ds.Tables(0).Rows(0).Item("number")
        End If
        editlist()
    End Sub

    Private Sub ddlcc_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlcc.SelectedIndexChanged
        editlist()
    End Sub
End Class
