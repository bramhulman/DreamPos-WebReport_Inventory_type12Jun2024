Imports System.Threading
Imports System.Net
Imports System.Text
Imports System.IO
Imports System.Globalization

Public Class Pages_ExpOtherEdit
    Inherits System.Web.UI.Page
    Private sNameuser As String
    Private Userid As Integer
    Private icustid As Integer
    Private imodul As Integer
    Private iModPriv As Integer
    Private irptpriv As Integer
    Private irptf As Integer
    Private mfunction As New ModFunction
    Private MTid As String
    Private cOut As New ClsOutlet
    Private CEOther As New Cls_ExOther
    Private CItrans As New Cls_ITrans
    Private cInv As New clsInventory


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
        MTid = Request.Cookies("ID").Value

        Try

            If Request.Cookies("UserLogin_inv") IsNot Nothing Then
                sNameuser = username()
                icustid = custid()
                imodul = modul()
                iModPriv = modulepriv()
                irptpriv = rptpriv()
                irptf = rptf()
                If IsPostBack = False Then
                    'editlist(MTid)
                    loadoutlet()
                    If MTid = "" Then

                        DdlOutlet.Text = ""
                        hapus()
                        Busidate.Text = Format(Now, "dd MMM yyyy")
                        'loadCust()
                    Else

                        editlist(MTid)
                        ' loadCust()
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
    Sub loadoutlet()
        DdlOutlet.Items.Clear()

        Dim ds As New DataSet
        ds = cInv.getInvcc(custid, username, modulepriv, 0)
        'ds = cOut.SelectDataset_Outlet(custid, username, modulepriv)
        If ds IsNot Nothing AndAlso ds.Tables.Count > 0 Then
            DdlOutlet.DataSource = ds
        Else
            DdlOutlet.DataSource = Nothing
        End If
        DdlOutlet.DataValueField = "Id"
        DdlOutlet.DataTextField = "Name"

        DdlOutlet.DataBind()

    End Sub
    Sub editlist(ByVal id As String)


        Dim ds As New DataSet
        ds = CEOther.SelectDataset_ExOtherEdit(id, custid)
        If ds.Tables(0).Rows.Count = 0 Then
            Exit Sub
        End If
        Dim out As Integer = ds.Tables(0).Rows(0).Item("ccID")
        DdlOutlet.SelectedValue = out
        Busidate.Text = Format(CDate(ds.Tables(0).Rows(0).Item("Busidate")), "dd MMM yyyy")
        txtSubtotal.Text = FormatCurrency(ds.Tables(0).Rows(0).Item("Subtotal"), 0).Replace("$", "")

        txtName.Text = ds.Tables(0).Rows(0).Item("Name")


        DdlOutlet.Enabled = False
        Busidate.Enabled = False

        btnUpdate.Visible = True
        btnInsert.Visible = False
    End Sub
    Public Function username() As String
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        Return cookie("userName")

    End Function

    Sub hapus()

        Try
            txtName.Text = ""
            '  txtRemarks.Text = ""


            btnUpdate.Visible = False
            btnInsert.Visible = True
        Catch ex As Exception

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

            Response.Redirect("~/Pages_ExpOther.aspx")
        Catch ex As Exception
            errLog(Err.Description, "WebBatchEdit:btnCancel_Click")
            Err.Clear()
        End Try

    End Sub




    Sub insert()
        Dim msg As String = ""

        Dim newLine As String = "\n"
        Dim c_recal As New cls_recalculate
        Try
            Dim busi As Date = CDate(Busidate.Text)
            If txtSubtotal.Text = 0 Or txtSubtotal.Text = "" Then
                msg = "Subtotal must be fill" & newLine
                ' Exit Sub
            End If
            If txtName.Text = "" Then
                msg = msg + "Name must be fill"
                ' Exit Sub
            End If
            If msg = "" Then
                insertExOther(MTid, custid, busi, DdlOutlet.SelectedValue, txtName.Text, txtSubtotal.Text, username)

            Else
                notsuccesfull(msg)
                Exit Sub
            End If
            c_recal.recalFlag_dailycost(DdlOutlet.SelectedValue, busi, custid)

            'MsgBox("Data Berhasil Di input Mysql")

            Response.Redirect("~/Pages_ExpOther.aspx", False)

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
    Public Sub insertExOther(ByVal id As String, ByVal custid As Integer, ByVal busidate As Date, ByVal ccid As Integer, ByVal nama As String, ByVal Subtotal As Integer, ByVal username As String)
        Try

            Dim ds As DataSet
            ds = CEOther.SelectDataExpOtherbyid(id)

            Dim str As String
            Dim c_cmd As New Cls_command
            Dim idtrans As String = CItrans.idExOther(custid)
            If ds.Tables(0).Rows.Count = 0 Then
                str = "insert into cld_ExpOther values ('" & idtrans & "', " & custid & ", " & ccid & ", '" & busidate.Date.ToString("yyyy-MM-dd") & "', '" & nama & "',  " & Subtotal & ")"


                'Revisi 10 Mei
                insertLogModule(username, "ExpOther", "Insert", custid)
            Else
                str = "Update cld_ExpOther set  name = '" & nama & "', Subtotal = " & Subtotal & "  where id = '" & id & "'"



                insertLogModule(username, "ExpOther", "Update", custid)
            End If

            c_cmd.executeNonQuery(str)

        Catch ex As Exception

            errLog(Err.Description, "webservice:insertExpOther")
            Err.Clear()
        End Try
    End Sub

End Class
