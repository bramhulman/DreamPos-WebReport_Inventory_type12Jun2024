Imports System.Threading
Imports System.Net
Imports System.Text
Imports System.IO

Public Class CustomerPO_Edit
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


    Private cpo As New Cls_PO


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
        MTid = Request.Cookies("InvCustomer_ID").Value
        Try

            If Request.Cookies("UserLogin_inv") IsNot Nothing Then
                sNameuser = username()
                icustid = custid()
                imodul = modul()
                iModPriv = modulepriv()
                irptpriv = rptpriv()
                irptf = rptf()
                If IsPostBack = False Then
                    editlist(MTid)
                    If MTid = 0 Then
                        hapus()
                        '   loadStkGrp()
                        '  loadRcp()
                    Else
                        editlist(MTid)
                        txtAddress.Attributes.Add("maxlength", txtAddress.MaxLength.ToString)
                        txtNotes.Attributes.Add("maxlength", txtNotes.MaxLength.ToString)
                        'loadStkGrp()
                        'loadRcp()
                    End If

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

        txtName.Text = ""
        txtEmail.Text = ""
        txtAddress.Text = ""
        txtCity.Text = ""
        txtCpos.Text = ""
        txtState.Text = ""
        txtNotes.Text = ""
        txtPhone.Text = ""



        btnUpdate.Visible = False
        btnInsert.Visible = True
    End Sub

    Sub editlist(ByVal id As Integer)
        Dim ds As New DataSet
        Try
            ds = cpo.SelectDs_CustPOEdit(id, custid)
            If ds.Tables(0).Rows.Count = 0 Then
                Exit Sub
            End If

            txtEmail.Text = ds.Tables(0).Rows(0).Item("Email")
            txtName.Text = ds.Tables(0).Rows(0).Item("Name")
            txtAddress.Text = ds.Tables(0).Rows(0).Item("Address")
            txtCity.Text = ds.Tables(0).Rows(0).Item("City")
            txtPhone.Text = ds.Tables(0).Rows(0).Item("Phone")
            txtNotes.Text = ds.Tables(0).Rows(0).Item("Notes")
            txtCpos.Text = ds.Tables(0).Rows(0).Item("Pcode")
            txtState.Text = ds.Tables(0).Rows(0).Item("State")
            'txtRemarks.Text = ds.Tables(0).Rows(0).Item("remarks")
            'txtotype.Text = ds.Tables(0).Rows(0).Item("otype")
            'TxtCust.Text = ds.Tables(0).Rows(0).Item("CustID")

            btnUpdate.Visible = True
            btnInsert.Visible = False
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub btnInsert_Click(ByVal sender As Object, ByVal e As EventArgs) 'Handles btnInsert.Click
        insert()
    End Sub

    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnUpdate.Click
        insert()
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancel.Click

        Try
            hapus()
            mfunction.CreateCookie("InvCustomer_ID", 0)

            Response.Redirect("~/Pages_CustomerPO.aspx", False)



        Catch ex As Exception
            errLog(Err.Description, "CustomerPO_Edit:btnCancel_Click")
            Err.Clear()
        End Try

    End Sub


    Sub insert()

        Try


            cpo.insertcustPO(MTid, custid, txtName.Text, txtEmail.Text, txtAddress.Text,
                        txtPhone.Text, txtCity.Text, txtState.Text, txtCpos.Text, txtNotes.Text, username)

            Response.Redirect("~/Pages_CustomerPO.aspx", False)

        Catch ex As Exception
            'MsgBox(ex.Message)
            errLog(Err.Description, "CustomerPO_Edit:insert")
            Err.Clear()
        End Try

    End Sub


End Class