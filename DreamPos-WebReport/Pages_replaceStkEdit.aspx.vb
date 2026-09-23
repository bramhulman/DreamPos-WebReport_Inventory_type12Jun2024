Imports System.Threading
Imports System.Net
Imports System.Text
Imports System.IO
Imports System

Public Class Pages_replaceStkEdit
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


        If Session("username") Is Nothing Or custid() < 1 Then
            Session.Clear()
            Session.Abandon()
            Session.RemoveAll()
            FormsAuthentication.RedirectToLoginPage()
            Exit Sub
        End If
        ' MTid = Request.Cookies("InvStk_ID").Value
        'ccid = Request.Cookies("InvStk_cc").Value
        Try

            If Request.Cookies("UserLogin_inv") IsNot Nothing Then
                sNameuser = username()
                icustid = custid()
                imodul = modul()
                iModPriv = modulepriv()
                irptpriv = rptpriv()
                irptf = rptf()
                If IsPostBack = False Then

                    'loadCc()
                    loadStkbefore()
                    loadStkafter()
                    'ddlcc.SelectedValue = ccid
                    'ddlbefore.SelectedValue = MTid
                    'txtNo.Text = ciStock.SelectDataset_StkEdit(ddlstok.SelectedValue, custid).Tables(0).Rows(0).Item("number")
                    'editlist()

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



        btnUpdate.Visible = False
        btnInsert.Visible = True
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

            Response.Redirect("~/Pages_replaceStk.aspx", False)



        Catch ex As Exception
            errLog(Err.Description, "Pages_replaceStkEdit:btnCancel_Click")
            Err.Clear()
        End Try

    End Sub


    Sub insert()



        Try



            cStock.insertStkreplace(ddlafter.SelectedValue, custid, ddlbefore.SelectedValue, username, CDate(Now.Date))

            'MsgBox("Data Berhasil Di input Mysql")

            Response.Redirect("~/Pages_replaceStk.aspx", False)

        Catch ex As Exception
            errLog(Err.Description, "Pages_replaceStkEdit:insert")
            Err.Clear()
        End Try

    End Sub


    Sub loadStkbefore()
        Try

            Dim ds As New DataSet
            ds = ciStock.SelectDataset_StkAll(custid())
            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 Then
                ddlbefore.DataSource = ds
            Else
                ddlbefore.DataSource = Nothing
            End If
            ddlbefore.DataValueField = "id"
            ddlbefore.DataTextField = "Name"
            ddlbefore.DataBind()



        Catch ex As Exception
            errLog(Err.Description, "Pages_replaceStkEdit:loadStkbefore")
            Err.Clear()
        End Try

    End Sub

    Sub loadStkafter()
        Try

            Dim ds As New DataSet
            ds = ciStock.SelectDataset_StkAll(custid())
            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 Then
                ddlafter.DataSource = ds
            Else
                ddlafter.DataSource = Nothing
            End If
            ddlafter.DataValueField = "id"
            ddlafter.DataTextField = "Name"
            ddlafter.DataBind()



        Catch ex As Exception
            errLog(Err.Description, "Pages_replaceStkEdit:loadStkafter")
            Err.Clear()
        End Try

    End Sub



End Class
