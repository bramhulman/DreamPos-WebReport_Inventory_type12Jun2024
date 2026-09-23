Imports System.Threading
Imports System.Net
Imports System.Text
Imports System.IO

Public Class Pages_OwnEdit
    Inherits System.Web.UI.Page
    Private sNameuser As String
    Private Userid As Integer
    Private icustid As Integer
    Private imodul As Integer
    Private iModPriv As Integer
    Private irptpriv As Integer
    Private irptf As Integer
    Private mfunction As New ModFunction
    Private Ownid As Integer

    Private cOut As New ClsOutlet
    Private COwn As New ClsOwn


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
        Ownid = Request.Cookies("ownID").Value
        Try

            If Request.Cookies("UserLogin_inv") IsNot Nothing Then
                sNameuser = username()
                icustid = custid()
                imodul = modul()
                iModPriv = modulepriv()
                irptpriv = rptpriv()
                irptf = rptf()
                If IsPostBack = False Then

                    If Ownid = 0 Then


                        hapus()
                    Else

                        editlist(Ownid)

                    End If
                    'check_Batch()
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
            errLog(Err.Description, "Pages_OwnEdit:Page_Load")
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
        txtMarkup.Text = ""

        btnUpdate.Visible = False
        btnInsert.Visible = True
    End Sub

    Sub editlist(ByVal id As Integer)
        Try
            Dim ds As New DataSet
            ds = COwn.SelectDataset_own(id, custid)
            If ds.Tables(0).Rows.Count = 0 Then
                Exit Sub
            End If
            Dim xown As Integer = ds.Tables(0).Rows(0).Item("id")


            txtMarkup.Text = ds.Tables(0).Rows(0).Item("markup")
            txtName.Text = ds.Tables(0).Rows(0).Item("Name")




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

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancel.Click

        Try
            hapus()
            mfunction.CreateCookie("ID", 0)


            Response.Redirect("~/Pages_Own.aspx", False)
        Catch ex As Exception
            errLog(Err.Description, "Pages_OwnEdit:btnCancel_Click")
            Err.Clear()
        End Try

    End Sub



    Public Sub insertOwn(ByVal id As Integer, ByVal custid As Integer, ByVal nama As String, ByVal markup As String)
        Try

            Dim ds As DataSet
            ds = COwn.SelectDataset_own(id, custid)

            Dim str As String
            Dim c_cmd As New Cls_command
            If ds.Tables(0).Rows.Count = 0 Then
                str = "insert into cld_own values (" & c_cmd.ExecuteScalar("select max(ID) from cld_own") & ", " & custid & ", '" & nama & "',  '" & markup & "')"
                insertLogModule(username, "InvCc", "Insert", custid)
            Else
                str = "Update cld_own set name = '" & nama & "', markup = '" & markup & "' where id = " & id & " and custid = " & custid & ""
                insertLogModule(username, "InvCc", "Update", custid)
            End If

            c_cmd.executeNonQuery(str)

        Catch ex As Exception

            errLog(Err.Description, "Pages_OwnEdit:insertIcc")
            Err.Clear()
        End Try
    End Sub
    Sub insert()

        Try

            Dim iStore As Integer = 0



            insertOwn(Ownid, custid, txtName.Text, txtMarkup.Text)

            'MsgBox("Data Berhasil Di input Mysql")

            Response.Redirect("~/Pages_Own.aspx", False)

            Exit Sub

        Catch ex As Exception
            errLog(Err.Description, "Pages_OwnEdit:insert")
            Err.Clear()
        End Try

    End Sub


End Class