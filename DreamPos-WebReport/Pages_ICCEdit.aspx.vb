Imports System.Threading
Imports System.Net
Imports System.Text
Imports System.IO

Public Class Pages_ICCEdit
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

    Private cOut As New ClsOutlet
    Private CIcc As New Cls_ICc


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
        MTid = Request.Cookies("ccID").Value
        Try

            If Request.Cookies("UserLogin_inv") IsNot Nothing Then
                sNameuser = username()
                icustid = custid()
                imodul = modul()
                iModPriv = modulepriv()
                irptpriv = rptpriv()
                irptf = rptf()
                If IsPostBack = False Then
                    loadoutlet(MTid)
                    If MTid = 0 Then

                        DdlOutlet.Text = ""
                        hapus()
                    Else

                        editlist(MTid)

                    End If
                    check_Batch()
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


        Catch ex As Exception
            errLog(Err.Description, "Pages_ICCEdit:Page_Load")
            Err.Clear()
        End Try
    End Sub

    Protected Sub Page_unLoad(sender As Object, e As EventArgs) Handles Me.Unload
    End Sub

    Public Function username() As String
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        Return cookie("userName")

    End Function

    Sub hapus()

        txtName.Text = ""
        txtRemarks.Text = ""

        btnUpdate.Visible = False
        btnInsert.Visible = True
    End Sub

    Sub editlist(ByVal id As Integer)
        Try
            Dim ds As New DataSet
            ds = CIcc.SelectDataset_IccEdit(id, custid)
            If ds.Tables(0).Rows.Count = 0 Then
                Exit Sub
            End If
            Dim out As Integer = ds.Tables(0).Rows(0).Item("olid")
            If out = 0 Then
                chkbatch.Checked = True
            Else
                chkbatch.Checked = False
                DdlOutlet.SelectedValue = out
            End If

            txtRemarks.Text = ds.Tables(0).Rows(0).Item("remarks")
            txtName.Text = ds.Tables(0).Rows(0).Item("Name")

            If IsDBNull(ds.Tables(0).Rows(0).Item("Email")) = False Then
                txtEmail.Text = ds.Tables(0).Rows(0).Item("Email")
            End If

            If IsDBNull(ds.Tables(0).Rows(0).Item("Address")) = False Then
                txtAddress.Text = ds.Tables(0).Rows(0).Item("Address")
            End If

            If IsDBNull(ds.Tables(0).Rows(0).Item("Pic")) = False Then
                txtPIC.Text = ds.Tables(0).Rows(0).Item("Pic")
            End If


            btnUpdate.Visible = True
            btnInsert.Visible = False
        Catch ex As Exception
            errLog(Err.Description, "Pages_ICCEdit:editlist")
            Err.Clear()
        End Try
    End Sub

    Protected Sub btnInsert_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnInsert.Click
        insert()
    End Sub

    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnUpdate.Click
        insert()
    End Sub
    Sub loadoutlet(ByVal id As Integer)
        'DdlOutlet.Items.Clear()

        Dim ds As New DataSet
        ds = cOut.SelectDataset_Outletbycust(custid, id, username, modulepriv)
        If ds IsNot Nothing AndAlso ds.Tables.Count > 0 Then
            DdlOutlet.DataSource = ds
        Else
            DdlOutlet.DataSource = Nothing
        End If
        DdlOutlet.DataValueField = "Id"
        DdlOutlet.DataTextField = "Nama"

        DdlOutlet.DataBind()
        DdlOutlet.SelectedValue = id
    End Sub
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancel.Click

        Try
            hapus()
            mfunction.CreateCookie("ID", 0)


            Response.Redirect("~/Pages_ICC.aspx")
        Catch ex As Exception
            errLog(Err.Description, "Pages_ICCEdit:btnCancel_Click")
            Err.Clear()
        End Try

    End Sub

    Protected Sub chkbatch_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkbatch.CheckedChanged
        check_Batch()
    End Sub
    Private Sub check_Batch()
        If chkbatch.Checked = True Then
            RequiredFieldValidator1.Enabled = False

            DdlOutlet.Enabled = False

            txtAddress.Enabled = True
            txtEmail.Enabled = True
            txtPIC.Enabled = True
        ElseIf chkbatch.Checked = False Then
            RequiredFieldValidator1.Enabled = True

            DdlOutlet.Enabled = True
            If DdlOutlet.Items.Count = 0 Then
                loadoutlet(0)
            End If
            txtAddress.Enabled = False
            txtEmail.Enabled = False
            txtPIC.Enabled = False

        End If
    End Sub
    Public Sub insertIcc(ByVal id As Integer, ByVal custid As Integer, ByVal nama As String, ByVal remarks As String, ByVal outletid As Integer, ByVal username As String, ByVal sEmail As String, ByVal sAddress As String, ByVal sPic As String)
        Try

            Dim ds As DataSet
            ds = CIcc.SelectDataIcc(id)

            Dim str As String
            Dim c_cmd As New Cls_command
            If ds.Tables(0).Rows.Count = 0 Then
                str = "insert into cld_Inv_cc values (" & c_cmd.ExecuteScalar("select max(ID) from cld_inv_cc") & ", " & custid & ", '" & nama & "',  '" & remarks & "', " & outletid & ", '" & sEmail & "', '" & sAddress & "', '" & sPic & "')"
                insertLogModule(username, "InvCc", "Insert", custid)
            Else
                str = "Update cld_inv_cc set name = '" & nama & "', remarks = '" & remarks & "', Email = '" & sEmail & "', Address = '" & sAddress & "', Pic = '" & sPic & "', Olid = " & outletid & " where id = " & id & ""
                insertLogModule(username, "InvCc", "Update", custid)
            End If

            c_cmd.executeNonQuery(str)

        Catch ex As Exception

            errLog(Err.Description, "Pages_ICCEdit:insertIcc")
            Err.Clear()
        End Try
    End Sub
    Sub insert()

        Try

            Dim iStore As Integer = 0

            If chkbatch.Checked = True Then
                iStore = 0
            Else
                iStore = DdlOutlet.SelectedValue
            End If

            insertIcc(MTid, custid, txtName.Text, txtRemarks.Text, iStore, username, txtEmail.Text, txtAddress.Text, txtPIC.Text)

            'MsgBox("Data Berhasil Di input Mysql")

            Response.Redirect("~/Pages_ICC.aspx")

            Exit Sub

        Catch ex As Exception
            errLog(Err.Description, "Pages_ICCEdit:insert")
            Err.Clear()
        End Try

    End Sub


End Class
