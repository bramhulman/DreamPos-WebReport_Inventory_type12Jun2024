Imports System.Threading
Imports System.Net
Imports System.Text
Imports System.IO
Imports System.Data
Imports System.Drawing
Imports System.Reflection

Public Class Pages_InvOrderWDetail
    Inherits System.Web.UI.Page
    Private sNameuser As String
    Private Userid As Integer
    Private icustid As Integer
    Private imodul As Integer
    Private iModPriv As Integer
    Private irptpriv As Integer
    Private irptf As Integer
    Private mfunction As New ModFunction

    Private wID As String
    Private wCc As Integer
    Private wFrom As Integer
    Private wDel As Date
    Private wCreate As Date
    Private wRcv As Date
    Private wNotes As String
    Public xware As Integer
    Public xfrom As Integer
    Dim alert1 As Integer
    Dim alert2 As Integer
    Private clsInv As New clsInventory
    Private clsStk As New clsInventory_stk

    Private cSupp As New clsInventory_Supplier
    Private CStk As New Cls_IStk


    'Private transpriv As Integer
    Public Function modulepriv() As Integer
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")

        If cookie Is Nothing Then
            Return -1
        Else
            Return cookie("modulepriv")
        End If
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


        If cookie Is Nothing Then
            Return -1
        Else
            Return cookie("rptpriv")
        End If
    End Function
    Public Function modul() As Integer
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")

        If cookie Is Nothing Then
            Return -1
        Else
            Return cookie("modul")
        End If
    End Function
    Public Function rptf() As Integer
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")


        If cookie Is Nothing Then
            Return -1
        Else
            Return cookie("rptf")
        End If
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
        Dim c_cmd As New clsDbo("")
        Dim notes As String
        Try
            wID = Request.Cookies("wID").Value
            wCc = Request.Cookies("wCc").Value
            wFrom = Request.Cookies("wFrom").Value
            wNotes = Request.Cookies("wNotes").Value

            If IsDate(Request.Cookies("wstatDate").Value) Then
                wDel = Request.Cookies("wstatDate").Value
            End If


            If IsDate(Request.Cookies("wCreatedDate").Value) Then
                wCreate = (Request.Cookies("wCreatedDate").Value)
                txtcreatedate.Text = Format(wCreate, "dd MMM yyyy")
                Createdate.Visible = True
                txtcreatedate.Enabled = False
            End If

            If IsDate(Request.Cookies("wDelDate").Value) Then
                wDel = (Request.Cookies("wDelDate").Value)
                txtexpD.Text = Format(wDel, "dd MMM yyyy")

                Deliverydate.Visible = True
                txtexpD.Enabled = False
            End If

            If IsDate(Request.Cookies("wrcvDate").Value) Then
                wRcv = (Request.Cookies("wrcvDate").Value)
                txtrcvdate.Text = Format(wRcv, "dd MMM yyyy")

                receivedate.Visible = True
                txtrcvdate.Enabled = False
            End If

            If Request.Cookies("UserLogin_inv") IsNot Nothing Then
                sNameuser = username()
                icustid = custid()
                imodul = modul()
                iModPriv = modulepriv()
                irptpriv = rptpriv()
                irptf = rptf()

                If IsPostBack = False Then

                    'txtexpD.Text = Format(Now, "dd MMM yyyy")
                    txtcreatedate.Text = Format(Now, "dd MMM yyyy")
                    loadFrom()
                    loadwarehouse()
                    btnedit.InnerText = "edit"
                    If wID = "" Then
                        txtID.Attributes.Add("placeholder", "New")
                        'txtID.Attributes.Add("style", "color:Blue")
                        'lblStatus.Text = "New "
                        'lblStatus.ForeColor = Drawing.Color.Blue

                        txtID.Text = ""
                        txtID.ReadOnly = True
                        Dim dt, dt2 As New DataTable
                        dt = clsInv.getDTInvWare2(wID, custid, 0, 0)
                        ViewState("orderW") = dt

                        BindData(modulepriv)
                        Bindingstk1()
                        Createdate.Visible = True
                        Label4.Text = "ExpDel Date"
                    Else

                        txtID.Text = wID
                        txtID.ReadOnly = True

                        ddlFrom.SelectedValue = wCc
                        ddlFrom.Enabled = False

                        ddlWare.SelectedValue = wFrom
                        ddlWare.Enabled = False

                        txtNotes.Text = wNotes
                        'txtNotes.ReadOnly = True

                        'If IsDate(wDel) Then
                        '    txtexpD.Text = Format(wDel, "dd MMM yyyy")
                        'Else
                        '    txtexpD.Text = Format(Now, "dd MMM yyyy")
                        'End If

                        Dim dt, dt2 As New DataTable
                        dt = clsInv.getDTInvWare2(wID, custid, ddlFrom.SelectedValue, ddlWare.SelectedValue)
                        ViewState("orderW") = dt
                        BindData(modulepriv)
                        Bindingstk1()

                    End If
                    alert2 = 0
                    alert1 = 0
                    notsuccesfull("Silakan order di bawah " & My.Settings.limitorder & " item, pisah jika lebih !!")
                    'Else
                    '    BindData(modulepriv)
                End If
                Dim script As String = "$(document).ready(function () { $('[id*=btnShow]').click(); });"

                ClientScript.RegisterStartupScript(Me.GetType, "load", script, True)
                
                ScriptManager.GetCurrent(Me).RegisterPostBackControl(RadButton1)
             
                txtSearch.Attributes.Add("onfocus", "javascript: this.setSelectionRange(this.value.length,this.value.length);")
                txtNotes.Attributes.Add("maxlength", txtNotes.MaxLength.ToString)
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
            errLog(Err.Description, "Pages_InvRcvEdit:Page_Load")
            Err.Clear()
        End Try
    End Sub
    Protected Sub BindData(ByVal modpriv As Integer)
        Try
            If custid() = 0 Then
                Response.Redirect("~/Default.aspx", False)
            Else

                DgvTransfer.DataSource = DirectCast(ViewState("orderW"), DataTable)
                If modpriv And usrModulepriv.moduleuser = usrModulepriv.moduleuser Then
                Else
                    DgvTransfer.Columns(5).Visible = True
                End If
                DgvTransfer.DataBind()
                'DgvTransfer.
                If DgvTransfer.Rows.Count = 0 Then
                    btnedit.Visible = False
                Else
                    btnedit.Visible = True
                End If

            End If
        Catch ex As Exception
            errLog(Err.Description, "Pages_InvOrderWDetail:BindData")
            Err.Clear()
        End Try
    End Sub
    Sub Bindingstk1()
        Try
            If icustid = 0 Then
                Response.Redirect("~/Default.aspx", False)
            Else

                'Bram4Sep2018 Update Report
                Dim dt, dt2 As New DataTable
                dt = DirectCast(ViewState("orderW"), DataTable)
                dt2 = clsStk.SelectDataTable_Stk_wStockWH(dt, custid, txtSearch.Text, ddlFrom.SelectedValue, ddlWare.SelectedValue)
                'dt2 = clsStk.SelectDataTable_Stk_wStock(dt, custid, txtSearch.Text, ddlFrom.SelectedValue)
                ViewState("InvStk") = dt2

                If dt2.Rows.Count > 0 Then
                    LsvStk.Items.Clear()
                    countlvstore = dt2.Rows.Count
                    LsvStk.DataSource = dt2.DefaultView
                    LsvStk.DataBind()
                    UpdatePanel3.Update()
                End If




            End If
        Catch ex As Exception
            errLog(Err.Description, "Pages_InvOrderWDetail:Bindingstk1")
            Err.Clear()
        End Try
    End Sub
    Sub bindingstk()

        Try
            If custid() = 0 Then
                Response.Redirect("~/Default.aspx", False)
            Else
                Dim dt, dt2 As New DataTable
                dt = DirectCast(ViewState("orderW"), DataTable)

                Debug.Print(dt.Rows.Count.ToString)
                'dt2 = clsStk.SelectDataTable_Stk_wStock(dt, custid, txtSearch.Text, ddlFrom.SelectedValue)
                'ViewState("InvStk") = dt2
                'LsvStk.DataSource = DirectCast(ViewState("InvStk"), DataTable)
                'LsvStk.DataBind()

                dt2 = DirectCast(ViewState("InvStk"), DataTable)

                If dt2.Rows.Count = 0 Then
                    ' dt2 = clsStk.SelectDataTable_Stk_wStock(dt, custid, txtSearch.Text, ddlFrom.SelectedValue)
                    dt2 = clsStk.SelectDataTable_Stk_wStockWH(dt, custid, txtSearch.Text, ddlFrom.SelectedValue, ddlWare.SelectedValue)
                    ViewState("InvStk") = dt2
                Else



                    dt2.DefaultView.RowFilter = "name like '%" & txtSearch.Text & "%'"
                    ViewState("InvStk") = dt2
                End If

                If dt2.Rows.Count > 0 Then
                    LsvStk.Items.Clear()
                    countlvstore = dt2.Rows.Count
                    LsvStk.DataSource = dt2.DefaultView
                    LsvStk.DataBind()
                    UpdatePanel3.Update()

                End If

            End If
        Catch ex As Exception
            errLog(Err.Description, "Pages_InvOrderWDetail:bindingstk")
            Err.Clear()
        End Try


    End Sub

    Sub loadwarehouse()

        Dim ds As New DataSet

        Dim CIcc As New clsInventory_Cc
        Try
            ds = CIcc.SelectDataset_IWare(custid)
            CIcc = Nothing

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 Then
                ddlWare.DataSource = ds
            Else
                ddlWare.DataSource = Nothing
            End If
            ddlWare.DataValueField = "Id"
            ddlWare.DataTextField = "Name"
            ddlWare.DataBind()

            If ddlWare.Items.Count > 0 Then
                ddlWare.SelectedIndex = 0
                ddlWare.SelectedValue = ddlWare.SelectedItem.Value
            End If
            ds.Dispose()
        Catch ex As Exception
            errLog(Err.Description, "Pages_InvOrderWDetail:loadwarehouse")
            Err.Clear()
        End Try

    End Sub


    Protected Sub Page_unLoad(sender As Object, e As EventArgs) Handles Me.Unload
    End Sub

    Public Function username() As String
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        Return cookie("userName")

    End Function


    Protected Sub RadButton1_ServerClick(sender As Object, e As EventArgs) Handles RadButton1.ServerClick
        Try
            If ddlFrom.SelectedValue <> 0 And ddlWare.SelectedValue <> 0 Then
                Add2()
                Session("xware") = ddlWare.SelectedValue
                Session("xfrom") = ddlFrom.SelectedValue
            Else
                notsuccesfull("you must choose Cost Centre first")
            End If

        Catch ex As Exception
            errLog(Err.Description, "Pages_InvOrderWDetail:RadButton1_ServerClick")
            Err.Clear()
        End Try        'Add2()
        'DgvTransfer.DataBind()
    End Sub
    Sub Add(Optional ByVal pilih As Boolean = True)

        Try
            Dim shsl As String = ""
            Dim i As Integer = 0
            If pilih = False Then
                For Each item As ListViewItem In LsvStk.Items
                    Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox"), CheckBox)
                    'Update Ihsan 19032019

                    If ck.Checked Then
                        ck.Checked = False
                    End If


                Next
                lblnoitem.Text = ""

            Else
                For Each item As ListViewItem In LsvStk.Items
                    Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox"), CheckBox)
                    'Update Ihsan 19032019

                    If ck.Checked Then
                        If shsl = "" Then
                            shsl = (DirectCast(item.FindControl("lblID"), Label).Text)
                        Else
                            shsl = shsl & "," & (DirectCast(item.FindControl("lblID"), Label).Text)
                        End If
                    End If


                Next
                If shsl = "" Then
                    lblnoitem.Text = ""

                Else
                    lblnoitem.Text = shsl

                End If
            End If




        Catch ex As Exception
            errLog(Err.Description, "Pages_InvOrderWDetail:Add")
            Err.Clear()
            'MsgBox(ex.Message)
        End Try

    End Sub
    Sub Add2()
        'untuk nambah record ke temporary sebelum ke database
        Dim dt As New DataTable
        Dim dtStk As New DataTable
        Try
            Dim i As Integer = 0
            If DirectCast(ViewState("orderW"), DataTable).Rows.Count >= My.Settings.limitorder Then
                notsuccesfull("Silakan order di bawah " & My.Settings.limitorder & " item, pisah jika lebih !!")
                GoTo ef
            End If
            txtSearch.Text = ""
            BindCheckItem()
            bindingstk()
            CheckedItem()
            For Each item As ListViewItem In LsvStk.Items.ToList()
                Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox"), CheckBox)

                If ck.Checked Then
                    Try
                        'Dim ds As DataSet = WService.getStkByid2((DirectCast(item.FindControl("lblID"), Label).Text), ddlFrom.SelectedValue, cust, ddlWare.SelectedValue)

                        Dim ds As DataSet = clsStk.SelectDataStk_wOther2((DirectCast(item.FindControl("lblID"), Label).Text), ddlFrom.SelectedValue, custid, ddlWare.SelectedValue, CDate(Now()))


                        If ds.Tables(0).Rows.Count > 0 Then
                            Dim u1 As String = ds.Tables(0).Rows(0).Item("Unit1").ToString
                            Dim factor1 As String = ds.Tables(0).Rows(0).Item("factor1").ToString
                            Dim dCost As Decimal = 0 'ds.Tables(0).Rows(0).Item("coststk").ToString
                            'Dim dstockcc As Decimal = ds.Tables(0).Rows(0).Item("stockcc") / ds.Tables(0).Rows(0).Item("factor1")
                            'Dim dstock As Decimal = ds.Tables(0).Rows(0).Item("stock") / ds.Tables(0).Rows(0).Item("factor1")
                            'Dim dstklimit As Decimal = ds.Tables(0).Rows(0).Item("stk_limit") / ds.Tables(0).Rows(0).Item("factor1")
                            'Bram 20Jun22 menggunakan unit2
                            Dim dstockcc As Decimal = ds.Tables(0).Rows(0).Item("stockcc")
                            Dim dstock As Decimal = ds.Tables(0).Rows(0).Item("stock")
                            Dim dstklimit As Decimal = ds.Tables(0).Rows(0).Item("stk_limit")

                            Dim snumber As String = ds.Tables(0).Rows(0).Item("Number").ToString
                            Dim u2 As String = ds.Tables(0).Rows(0).Item("Unit2").ToString

                            dt = DirectCast(ViewState("orderW"), DataTable)

                            Debug.Print((DirectCast(item.FindControl("lblID"), Label).Text))
                            Debug.Print((DirectCast(item.FindControl("lblName"), Label).Text))
                            Debug.Print(u1.ToString)
                            Debug.Print(factor1.ToString)

                            dt.Rows.Add("", custid, 0, (DirectCast(item.FindControl("lblID"), Label).Text), (DirectCast(item.FindControl("lblName"), Label).Text), 0, u1, 0, u2, 0, 0, 0, dCost, factor1, snumber, u2, dstock, dstockcc, dstock, dstklimit)

                            ViewState("orderW") = dt
                            dtStk = DirectCast(ViewState("InvStk"), DataTable)
                            dtStk.Rows(i).Delete()
                            dtStk.AcceptChanges()
                            i = i - 1

                        End If


                        txtSearch.Text = ""
                        bindingstk()
                        BindData(modulepriv)
                    Catch error_t As Exception
                        MsgBox(error_t.ToString)
                    End Try

                End If
                i = i + 1

                If dt.Rows.Count >= My.Settings.limitorder Then
                    notsuccesfull("Hanya bisa order " & dt.Rows.Count.ToString & " item !!")
                    Exit For
                End If

            Next
ef:
            'BindData()
        Catch ex As Exception
            'MsgBox(ex.Message)
            errLog(Err.Description, "Pages_InvOrderWDetail:Add2")
            Err.Clear()
        End Try
        'dt = Nothing
    End Sub


    Sub delete(ByVal id As String, ByVal iccTo As Integer, ByVal istk As Integer)
        clsInv.DeleteInvWare(id, custid, iccTo, istk, username)
    End Sub
    Public Sub DeleteRcpDtl(ByVal id As Integer, ByVal Custid As Integer, ByVal username As String)
        'Dim pc As String = pic.ToString
        Try

            Dim str As String
            Dim c_cmd As New Cls_command

            str = "DELETE FROM cld_inv_rcpdtl  where id = " & id & ""

            c_cmd.executeNonQuery(str)

            'Revisi 10 Mei
            insertLogModule(username, "InvRcpdtl", "Delete", Custid)

        Catch ex As Exception

            errLog(Err.Description, "Pages_InvOrderWDetail:DeleteInvrcpdtl")
            Err.Clear()
        End Try
    End Sub





    Protected Sub RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
        Try
            If e.Row.RowType = DataControlRowType.DataRow AndAlso DgvTransfer.EditIndex = e.Row.RowIndex Then

            End If
        Catch ex As Exception

        End Try
    End Sub

    Sub loadFrom()
        Dim ds As New DataSet

        Dim cicc As New clsInventory_Cc
        Try
            ds = cicc.SelectDataset_IccOnly(custid, username, modulepriv)
            cicc = Nothing

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 Then
                ddlFrom.DataSource = ds
            Else
                ddlFrom.DataSource = Nothing
            End If
            ddlFrom.DataValueField = "Id"
            ddlFrom.DataTextField = "Name"
            ddlFrom.DataBind()

            If ddlFrom.Items.Count > 0 Then
                ddlFrom.SelectedIndex = 0
            End If
        Catch ex As Exception
            errLog(Err.Description, "Pages_InvOrderWDetail:loadFrom")
            Err.Clear()
        End Try

    End Sub
    Public Function insert(ByVal stat As Integer, Optional ByVal xcancel As Integer = 0) As String
        Dim hasil As String = ""
        Dim newLine As String = "\n"
        Dim c_cmd As New Cls_command
        Try
            Dim msg As String = ""
            Dim msg2 As String = ""
            Dim dt As DataTable = DirectCast(ViewState("orderW"), DataTable)

            If dt.Rows.Count = 0 Then
                msg = msg + "Item is Nothing" & newLine
            End If

            If ddlFrom.SelectedValue = 0 Or ddlFrom.SelectedItem.Text = "" Then
                msg = msg + "Cost Centre Must be Choice " & newLine
            End If

            If ddlWare.SelectedValue = 0 Or ddlWare.SelectedItem.Text = "" Then
                msg = msg + "warehouse Must be Choice" & ":"
            End If

            For Each row As DataRow In dt.Rows
                If row.Item("totalu2") = 0 Then
                    msg = msg + " " & row.Item("Stk_name") + " order qty cannot 0 " & newLine
                ElseIf row.Item("totalu2") > row.Item("stockwh") Then
                    msg2 = msg2 + " " & row.Item("Stk_name") + ", OUT OF STOCK now, your order may be late " & newLine
                End If
            Next row

            If msg <> "" Then
                notsuccesfull(msg + msg2)
                'lblError.Text = msg
                GoTo ENDFUNCTION
            ElseIf msg2 <> "" Then
                'notsuccesfull(msg2)
                'lblError.Text = msg

            Else

                'dt.DefaultView.RowFilter = "stockwh > 0 "

                'dt = dt.DefaultView.ToTable

            End If

            If c_cmd.ExecuteScalar("select count(custid) from cld_inv_closing where custid = " & custid() & " and invdate = '" & Date.Now.Date.ToString("yyyy-MM-dd") & "' and  invflag = 0 ", 0) = 1 Then
                FormsAuthentication.SetAuthCookie(username, False)
                messagesuccesfull("Closing process,Please login again.", False)
                Return hasil
            Else
                Try
                    hasil = clsInv.insertInvOrderW(txtID.Text, custid(), 0, 1, ddlWare.SelectedValue, ddlWare.SelectedItem.ToString, ddlFrom.SelectedValue, ddlFrom.SelectedItem.ToString, username, username, CDate(txtcreatedate.Text), dt, "", txtNotes.Text)

                Catch ex As Exception
                    errLog(txtID.Text & " " & ddlWare.SelectedValue.ToString & " " & ddlWare.SelectedItem.ToString & " " & ddlFrom.SelectedValue.ToString & " " & ddlFrom.SelectedItem.ToString & " " & username() & " " & txtcreatedate.Text & " " & txtNotes.Text & " " & Err.Description, "Pages_InvOrderWDetail")
                    Err.Clear()
                End Try

            End If



            If hasil = "" Then
                hasil = txtID.Text
            End If



            mfunction.CreateCookie("wType", "1")

            If msg2 <> "" Then
                messagesuccesfull(msg2)

            Else

                Response.Redirect("~/Pages_InvOrderW.aspx", False)
            End If
            'Response.Redirect("~/Pages_InvOrderW.aspx")

        Catch ex As Exception

            'MsgBox(Err.Description, MsgBoxStyle.Information)
            errLog(Err.Description, "Pages_InvOrderWDetail:insert")
            Err.Clear()
            Return hasil
        End Try


ENDFUNCTION:

        Return hasil

    End Function

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
    Protected Sub DgvTransfer_RowDeleting(ByVal sender As Object, ByVal e As GridViewDeleteEventArgs)
        Try
            '  ViewState("InvRcvEdit") = 1

            Dim lblID As Label = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("lblID"), Label)
            Dim lblstknm As Label = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("lblName"), Label)
            Dim lblstkid As Label = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("lblStkID"), Label)
            Dim dt As DataTable = DirectCast(ViewState("InvRcv"), DataTable)
            Dim dtstk As DataTable = DirectCast(ViewState("InvStk"), DataTable)
            If lblID.Text = 0 Then
                dtstk.Rows.Add(lblstkid.Text, lblstknm.Text)
                ViewState("InvStk") = dtstk
                dt.Rows(e.RowIndex).Delete()
                dt.AcceptChanges()
            Else
                dtstk.Rows.Add(lblstkid.Text, lblstknm.Text)
                ViewState("InvStk") = dtstk
                Dim confirmValue As String = Request.Form("confirm_value")
                If confirmValue = "Yes" Then

                    Dim id As Integer = lblID.Text
                    Dim istk As Integer = CInt(Val(lblstkid.Text))
                    'deleteitem(id, lblstkid.Text)
                    'delete(id)
                    delete(id, ddlFrom.SelectedValue, istk)

                Else
                    Exit Sub
                End If
                dt.Rows(e.RowIndex).Delete()
                dt.AcceptChanges()
            End If
            BindData(modulepriv)
            bindingstk()


        Catch ex As Exception
            errLog(Err.Description, "Pages_InvRcvEdit:DgvTransfer_RowDeleting")
            Err.Clear()
        End Try

    End Sub
    'Protected Sub DgvTransfer_RowUpdating(ByVal sender As Object, ByVal e As GridViewUpdateEventArgs)

    '    Try
    '        ' Dim maxrowup As Integer = 0
    '        '  ViewState("InvRcvEdit") = 1

    '        Dim txtUnit1 As TextBox = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("txtEditUnit1"), TextBox)
    '        If IsNumeric(txtUnit1.Text) = False Then
    '            notsuccesfull("Unit 1 must be number")
    '            Exit Sub
    '        End If

    '        'Dim txtCost As Label = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("txtEditCost"), Label)

    '        Dim txtUnit2 As TextBox = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("txtEditUnit2"), TextBox)
    '        If IsNumeric(txtUnit2.Text) = False Then
    '            notsuccesfull("Unit 2 must be number")
    '            Exit Sub
    '        End If

    '        Dim txtEditU1Cost As TextBox = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("txtEditU1Cost"), TextBox)

    '        Dim clsI_stk As New clsInventory_stk

    '        'Dim lblStkEditID As Label = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("lblStkEditID"), Label)
    '        Dim lblLosF1 As Label = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("lblEditfactor"), Label)
    '        'lblLosF1.Text = clsI_stk.get_stkFactor1(lblLosF1.Text, custid)   'DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("lblEditfactor"), Label)

    '        If IsNumeric(txtUnit1.Text) = False Then
    '            txtUnit1.Text = 0
    '        End If

    '        If IsNumeric(txtUnit2.Text) = False Then
    '            txtUnit2.Text = 0
    '        End If

    '        If IsNumeric(txtEditU1Cost.Text) = False Then
    '            txtEditU1Cost.Text = 0
    '        End If

    '        'If txtEditU1Cost.Text = 0 Or txtUnit1.Text = 0 Then
    '        '    notsuccesfull("Please Input Unit 1 and Cost Unit 1")
    '        '    Exit Sub
    '        'End If

    '        Dim total As Decimal = (Val(txtUnit1.Text) * Val(lblLosF1.Text)) + Val(txtUnit2.Text)

    '        'tambahan erik 21 sept 2020, buat jagain nilai tidak tll besar
    '        If txtEditU1Cost.Text = 0 Then
    '            notsuccesfull("Please Input Cost Unit 1")
    '            txtUnit1.Text = 0
    '            txtUnit2.Text = 0
    '            Exit Sub
    '        End If

    '        If Val(txtUnit1.Text) >= 999999999 Then
    '            notsuccesfull("Unit 1 must less than 999999999")
    '            Exit Sub
    '        End If

    '        If Val(txtUnit2.Text) >= 999999999 Then
    '            notsuccesfull("Unit 2 must less than 999999999")
    '            Exit Sub
    '        End If

    '        If Val(total) >= 999999999 Then
    '            notsuccesfull("Total Stock must less than 999999999")
    '            Exit Sub
    '        End If

    '        If Val(txtEditU1Cost.Text) >= 999999999 Then
    '            notsuccesfull("Cost Must less than 999999999")
    '            Exit Sub
    '        End If

    '        Dim lblUnit1 As Label = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("lblEditEmpID"), Label)

    '        Dim dt As DataTable = DirectCast(ViewState("InvRcv"), DataTable)
    '        'Dim dtupdt As New DataTable
    '        'If Mtid <> "" Then

    '        '    dtupdt = dt.Clone
    '        '    dtupdt.Rows.Add(dt.Rows(e.RowIndex).ItemArray)
    '        '    maxrowup = dtupdt.Rows.Count - 1

    '        '    dtupdt.Rows(maxrowupx)("Factor1") = lblLosF1.Text

    '        '    dtupdt.Rows(maxrowup)("U1") = txtUnit1.Text

    '        '    dtupdt.Rows(maxrowup)("U2") = txtUnit2.Text

    '        '    dtupdt.Rows(maxrowup)("ttlStock") = total

    '        '    dtupdt.Rows(maxrowup)("CostU1") = txtEditU1Cost.Text
    '        'End If

    '        If lblLosF1.Text = 0 Then
    '            lblLosF1.Text = 1
    '            total = (Val(txtUnit1.Text) * Val(lblLosF1.Text)) + Val(txtUnit2.Text)
    '        End If

    '        dt.Rows(e.RowIndex)("Factor1") = clsI_stk.get_stkFactor1(dt.Rows(e.RowIndex)("stk_id"), custid) 'lblLosF1.Text

    '        dt.Rows(e.RowIndex)("U1") = txtUnit1.Text

    '        dt.Rows(e.RowIndex)("U2") = txtUnit2.Text

    '        dt.Rows(e.RowIndex)("ttlStock") = total

    '        dt.Rows(e.RowIndex)("CostU1") = txtEditU1Cost.Text




    '        Dim cost As Decimal = txtEditU1Cost.Text / lblLosF1.Text

    '        dt.Rows(e.RowIndex)("Cost") = cost

    '        Dim ttlcost As Decimal = total * cost
    '        dt.Rows(e.RowIndex)("ttlCost") = ttlcost

    '        'If Mtid <> "" Then
    '        '    dtupdt.Rows(maxrowup)("Cost") = cost
    '        '    dtupdt.Rows(maxrowup)("ttlCost") = ttlcost
    '        '    dtupdt.AcceptChanges()

    '        '    ViewState("InvRcvupdt") = dtupdt
    '        'End If

    '        dt.AcceptChanges()


    '        DgvTransfer.EditIndex = -1
    '        BindData(modulepriv)


    '    Catch ex As Exception
    '        errLog(Err.Description, "InvRcvEdit:DgvTransfer_RowUpdating")
    '        Err.Clear()
    '    End Try

    'End Sub
    'Protected Sub DgvTransfer_RowCancelingEdit(ByVal sender As Object, ByVal e As GridViewCancelEditEventArgs)
    '    DgvTransfer.EditIndex = -1
    '    BindData()
    'End Sub

    'Protected Sub DgvTransfer_RowEditing(ByVal sender As Object, ByVal e As GridViewEditEventArgs)
    '    DgvTransfer.EditIndex = e.NewEditIndex
    '    BindData()
    'End Sub

    Private Sub ShowToastr(ByVal page As Page, ByVal message As String, ByVal title As String, Optional ByVal type As String = "info", Optional ByVal clearToast As Boolean = False, Optional ByVal pos As String = "toast-top-left", Optional ByVal Sticky As Boolean = False)
        Dim toastrScript As String = [String].Format("Notify('{0}','{1}','{2}', '{3}', '{4}', '{5}');", message, title, type, clearToast, pos, Sticky)
        page.ClientScript.RegisterStartupScript(page.[GetType](), "toastr_message", toastrScript, addScriptTags:=True)
    End Sub
    <Serializable>
    Public Class Item_Detail 'Class untuk ListItem
        Public Property ItemName As String = ""
        Public Property Status As Boolean = False
    End Class
    'Start : Store



    'END : Store




    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSave.ServerClick
        Dim msg As String = ""

        Dim newLine As String = "\n"
        Try

            If (Now.Hour >= 23 And Now.Minute >= 30) Or (Now.Hour = 0 And Now.Minute <= 30) Then
                notsuccesfull("Please process the transaction after 1.00 AM")
                Exit Sub
            End If

            For Each row As GridViewRow In DgvTransfer.Rows

                If row.RowType = DataControlRowType.DataRow Then

                    If row.RowType = DataControlRowType.DataRow Then
                        Dim lblEditUnit2 As Label = (TryCast(row.FindControl("lblEditUnit2"), Label))
                        Dim txtEditUnit2 As TextBox = (TryCast(row.FindControl("txtEditUnit2"), TextBox))

                        lblEditUnit2.Visible = True
                        txtEditUnit2.Visible = False

                        lblEditUnit2.Text = txtEditUnit2.Text

                        If IsNumeric(txtEditUnit2.Text) = False Then
                            msg = msg + " row " + row.RowIndex.ToString + " Unit 1 must be number" & newLine

                        End If

                        If Val(txtEditUnit2.Text) >= 999999999 Then

                            msg = msg + " row " + row.RowIndex.ToString + " Unit 1 must less than 999999" & newLine
                        End If


                        Dim dt As DataTable = DirectCast(ViewState("orderW"), DataTable)
                        Dim clsI_stk As New clsInventory_stk

                        If msg <> "" Then
                            notsuccesfull(msg)
                            Exit Sub
                        End If

                        Dim lbl_stkid As Label = (TryCast(row.FindControl("lblStkID"), Label))

                        dt.Rows(row.RowIndex)("totalU2") = txtEditUnit2.Text

                        dt.AcceptChanges()
                        bindingstk()
                        BindData(modulepriv)

                    End If
                End If
            Next




            insert(2)




        Catch ex As Exception
            Dim myStringVariable As String = "fail to save"
            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('" + myStringVariable + "');", True)
            errLog(Err.Description, "Pages_InvRcvEdit:btnSave_Click")
            Err.Clear()
        End Try



        ' Response.Redirect("~/Pages_InvRcv.aspx")
    End Sub

    'Protected Sub btnFinish_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnFinish.ServerClick

    '    Try

    '        For Each row As GridViewRow In DgvTransfer.Rows

    '            If row.RowType = DataControlRowType.DataRow Then

    '                If row.RowType = DataControlRowType.DataRow Then
    '                    Dim lblUnit1 As Label = (TryCast(row.FindControl("lblUnit1"), Label))
    '                    Dim Label1 As Label = (TryCast(row.FindControl("Label1"), Label))
    '                    Dim txtEditUnit1 As TextBox = (TryCast(row.FindControl("txtEditUnit1"), TextBox))
    '                    Dim txtEditUnit2 As TextBox = (TryCast(row.FindControl("txtEditUnit2"), TextBox))
    '                    Dim lblU1Cost As Label = (TryCast(row.FindControl("lblU1Cost"), Label))
    '                    Dim lblttl_Cost As Label = (TryCast(row.FindControl("lblttlCost"), Label))
    '                    Dim txtEditU1Cost As TextBox = (TryCast(row.FindControl("txtEditU1Cost"), TextBox))
    '                    lblUnit1.Visible = True
    '                    txtEditUnit1.Visible = False
    '                    Label1.Visible = True
    '                    txtEditUnit2.Visible = False
    '                    lblU1Cost.Visible = True
    '                    txtEditU1Cost.Visible = False
    '                    lblUnit1.Text = txtEditUnit1.Text
    '                    Dim lblLosF1 As Label = (TryCast(row.FindControl("lblfactor"), Label))
    '                    Dim total As Decimal = (Val(txtEditUnit1.Text) * Val(lblLosF1.Text)) + Val(txtEditUnit2.Text)
    '                    If IsNumeric(txtEditUnit1.Text) = False Then
    '                        msg = msg + " row " + row.RowIndex.ToString + " Unit 1 must be number" & newLine

    '                    End If
    '                    Label1.Text = txtEditUnit2.Text
    '                    If IsNumeric(txtEditUnit2.Text) = False Then
    '                        msg = msg + " row " + row.RowIndex.ToString + " Unit 2 must be number" & newLine

    '                    End If
    '                    lblU1Cost.Text = txtEditU1Cost.Text
    '                    If IsNumeric(txtEditU1Cost.Text) = False Then
    '                        msg = msg + " row " + row.RowIndex.ToString + " cost must be number" & newLine
    '                        txtEditU1Cost.Text = 0
    '                    End If

    '                    If txtEditU1Cost.Text = 0 Then

    '                        msg = msg + " row " + row.RowIndex.ToString + " Please Input Cost" &
    '                            txtEditU1Cost.Text = 0
    '                        txtEditUnit2.Text = 0

    '                    End If

    '                    If Val(txtEditUnit1.Text) >= 999999999 Then

    '                        msg = msg + " row " + row.RowIndex.ToString + " Unit 1 must less than 999999" & newLine
    '                    End If

    '                    If Val(txtEditU1Cost.Text) >= 999999999 Then
    '                        msg = msg + " row " + row.RowIndex.ToString + " Cost must less than 999999" & newLine
    '                    End If

    '                    If Val(total) >= 999999999 Then

    '                        msg = msg + " row " + row.RowIndex.ToString + " Total Stock must less than 999999" & newLine

    '                    End If


    '                    Dim lblUnit_1 As Label = (TryCast(row.FindControl("lblEditEmpID"), Label))

    '                    Dim dt As DataTable = DirectCast(ViewState("InvRcv"), DataTable)
    '                    Dim clsI_stk As New clsInventory_stk

    '                    If lblLosF1.Text = 0 Then
    '                        lblLosF1.Text = 1
    '                        total = (Val(txtEditU1Cost.Text) * Val(lblLosF1.Text)) + Val(txtEditU1Cost.Text)
    '                    End If

    '                    If msg <> "" Then
    '                        notsuccesfull(msg)
    '                        Exit Sub
    '                    End If

    '                    Dim lbl_stkid As Label = (TryCast(row.FindControl("lblStkID"), Label))


    '                    dt.Rows(row.RowIndex)("Factor1") = clsI_stk.get_stkFactor1(lbl_stkid.Text, custid) 'lblLosF1.Text

    '                    dt.Rows(row.RowIndex)("U1") = txtEditUnit1.Text

    '                    dt.Rows(row.RowIndex)("U2") = txtEditUnit2.Text

    '                    dt.Rows(row.RowIndex)("ttlStock") = total

    '                    dt.Rows(row.RowIndex)("CostU1") = txtEditU1Cost.Text


    '                    Dim cost As Decimal = txtEditU1Cost.Text / lblLosF1.Text

    '                    dt.Rows(row.RowIndex)("Cost") = cost

    '                    Dim ttlcost As Decimal = total * cost
    '                    dt.Rows(row.RowIndex)("ttlCost") = ttlcost
    '                    dt.AcceptChanges()

    '                End If
    '            End If
    '        Next


    '        insert(2)


    '    Catch ex As Exception
    '        errLog(Err.Description, "Pages_InvRcvEdit:btnFinish_Click")
    '        Err.Clear()
    '    End Try


    'End Sub

    Protected Sub ddlFrom_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlFrom.SelectedIndexChanged
        Dim dt As New DataTable
        If DirectCast(ViewState("orderW"), DataTable) IsNot Nothing Then
            dt = DirectCast(ViewState("orderW"), DataTable)
        End If
        If Session("xfrom") Is Nothing Then
            Session("xfrom") = 0
        End If

        Try
            If dt.Rows.Count > 0 And ddlFrom.SelectedValue <> Session("xfrom").ToString Then
                notsuccesfull("you must delete item before change")
                ddlFrom.SelectedValue = Session("xfrom").ToString
            End If
            Bindingstk1()
        Catch ex As Exception
            errLog(Err.Description, "Pages_InvOrderWDetail:ddlFrom_SelectedIndexChanged")
            Err.Clear()
        End Try

    End Sub

    Protected Sub ddlWare_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlWare.SelectedIndexChanged
        Dim dt As DataTable = DirectCast(ViewState("orderW"), DataTable)
        Try
            If Session("xware") = Nothing Then
            Else
                If dt.Rows.Count > 0 And ddlWare.SelectedValue <> Session("xware").ToString Then
                    notsuccesfull("you must delete item before change")
                    ddlWare.SelectedValue = Session("xware").ToString
                End If
            End If

        Catch ex As Exception
            errLog(Err.Description, "Pages_InvOrderWDetail:ddlWare_SelectedIndexChanged")
            Err.Clear()
        End Try
    End Sub

    Protected Sub btnBack_ServerClick(sender As Object, e As EventArgs) Handles btnBack.ServerClick
        Response.Redirect("~/Pages_InvOrderW.aspx", False)
    End Sub
    Protected Sub btnClearSearch_Click(sender As Object, e As EventArgs)
        Try
            For Each item As ListViewItem In LsvStk.Items
                Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox1"), CheckBox)

                ck.Checked = False


            Next
            txtSearch.Text = ""
            lblnoitem.Text = 0

            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Pop", "openModalItem();", True)
        Catch ex As Exception
            errLog(Err.Description, "Pages_InvRcvEdit:btnClearSearch_Click")
            Err.Clear()
        End Try
    End Sub
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.ServerClick
        Try

            If txtSearch.Text = "" Then
                ShowToastr(Page, "Text Search tidak boleh kosong", "Error", "Error", False, "toast-top-right", False)
            Else
                BindCheckItem()
                bindingstk()
                CheckedItem()
            End If
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Pop", "openModalItem();", True)
        Catch ex As Exception
            errLog(Err.Description, "Pages_InvOrderWDetail:btnSearch_Click")
            Err.Clear()
        End Try

    End Sub
    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        Try
            Threading.Thread.Sleep(500)
            BindCheckItem()
            bindingstk()
            CheckedItem()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Pop", "openModalItem();", True)
            txtSearch.Attributes("onfocus") = "javascript:this.setSelectionRange(this.value.length,this.value.length);"
            txtSearch.Focus()
        Catch ex As Exception
            errLog(Err.Description, "Pages_InvOrderWDetail:txtSearch_TextChanged")
            Err.Clear()
        End Try
    End Sub
    Private Sub BindCheckItem()
        Try

            If CBool(IIf(IsNothing(ViewState("ListItem")), True, False)) Then


                Dim ListItem2 As List(Of Item_Detail) = New List(Of Item_Detail)()

                Dim dtorderw, dtitem As New DataTable
                dtorderw = DirectCast(ViewState("orderW"), DataTable)
                'dtitem = clsStk.SelectDataTable_Stk_wStock(dtorderw, custid, txtSearch.Text, ddlFrom.SelectedValue)
                dtitem = clsStk.SelectDataTable_Stk_wStock(dtorderw, custid, "", ddlFrom.SelectedValue)
                If dtitem.Rows.Count > 0 Then
                    For Each dt As DataRow In dtitem.Rows
                        ListItem2.Add(New Item_Detail With {
                                           .ItemName = dt.Item("name"),
                                           .Status = False
                                           })
                    Next
                End If


                For Each item As ListViewItem In LsvStk.Items
                    Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox"), CheckBox)
                    Dim Labels As Label = DirectCast(item.FindControl("lblName"), Label)
                    Dim label2 As String = Labels.Text



                    If ck.Checked Then
                        For Each a In ListItem2
                            If a.ItemName = label2 Then
                                a.Status = True
                            End If
                        Next
                        '  clsCheck.IDBrand.Add(label2)
                    End If
                Next
                ViewState("ListItem") = ListItem2

            Else

                Dim ListItem2 As List(Of Item_Detail) = TryCast(ViewState("ListItem"), List(Of Item_Detail))

                For Each item As ListViewItem In LsvStk.Items
                    Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox"), CheckBox)
                    Dim Labels As Label = DirectCast(item.FindControl("lblName"), Label)
                    Dim label2 As String = Labels.Text

                    If ck.Checked Then
                        For Each a In ListItem2
                            If a.ItemName = label2 Then
                                a.Status = True
                            End If
                        Next

                    Else
                        For Each a In ListItem2
                            If a.ItemName = label2 Then
                                a.Status = False
                            End If
                        Next
                    End If
                Next
                ViewState("ListItem") = ListItem2
            End If
        Catch ex As Exception
            errLog(Err.Description, "Pages_InvOrderWDetail:BindCheckItem")
            Err.Clear()
        End Try
    End Sub

    Private Sub CheckedItem()
        Try


            Dim ListStore As List(Of Item_Detail) = TryCast(ViewState("ListItem"), List(Of Item_Detail))

            Dim i As Integer = 0
            For Each item As ListViewItem In LsvStk.Items
                Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox"), CheckBox)
                Dim Labels As Label = DirectCast(item.FindControl("lblName"), Label)
                Dim label2 As String = Labels.Text

                For Each a In ListStore
                    If a.ItemName = label2 Then
                        If a.Status = True Then
                            ck.Checked = True
                        Else
                            ck.Checked = False
                        End If

                    End If
                Next

            Next
            ViewState("ListItem") = ListStore
        Catch ex As Exception
            errLog(Err.Description, "Pages_InvOrderWDetail:CheckedItem")
            Err.Clear()
        End Try
    End Sub


    'Protected Sub btnedit_ServerClick(sender As Object, e As EventArgs) Handles btnedit.ServerClick
    '    Try
    '        If btnedit.InnerText = "edit" Then
    '            For Each row As GridViewRow In DgvTransfer.Rows

    '                If row.RowType = DataControlRowType.DataRow Then
    '                    row.FindControl("txtEditUnit1").Visible = True
    '                    row.FindControl("lblUnit1").Visible = False
    '                    'lblUnit1
    '                    row.FindControl("txtEditUnit2").Visible = True
    '                    row.FindControl("Label1").Visible = False
    '                    row.FindControl("txtEditU1Cost").Visible = True
    '                    row.FindControl("lblU1Cost").Visible = False

    '                End If
    '            Next

    '            'btnedit.InnerText = "Calc."
    '            btnedit.InnerHtml = "<i class='fas fa-sync'></i>"
    '        Else
    '            For Each row As GridViewRow In DgvTransfer.Rows

    '                If row.RowType = DataControlRowType.DataRow Then
    '                    row.FindControl("txtEditUnit1").Visible = False
    '                    row.FindControl("lblUnit1").Visible = True
    '                    'lblUnit1
    '                    row.FindControl("txtEditUnit2").Visible = False
    '                    row.FindControl("Label1").Visible = True
    '                    row.FindControl("txtEditU1Cost").Visible = False
    '                    row.FindControl("lblU1Cost").Visible = True

    '                End If
    '            Next
    '            update()
    '            btnedit.InnerText = "edit"
    '        End If


    '    Catch ex As Exception
    '        errLog(Err.Description, "Pages_InvRcvEdit:btnedit_ServerClick")
    '        Err.Clear()
    '    End Try
    'End Sub


    Private Sub ResetRowID(ByVal dt As DataTable)
        Dim rowNumber As Integer = 1

        Try
            If dt.Rows.Count > 0 Then

                For Each row As DataRow In dt.Rows
                    row(0) = rowNumber
                    rowNumber += 1
                Next
            End If
        Catch ex As Exception
            errLog(Err.Description, "Pages_InvRcvEdit:imgbtnDelete_Click")
            Err.Clear()
        End Try
    End Sub
    Protected Sub update()
        Dim msg As String = ""
        Dim newLine As String = "\n"
        For Each row As GridViewRow In DgvTransfer.Rows

            If row.RowType = DataControlRowType.DataRow Then

                If row.RowType = DataControlRowType.DataRow Then
                    Dim lblEditUnit2 As Label = (TryCast(row.FindControl("lblEditUnit2"), Label))
                    'Dim Label1 As Label = (TryCast(row.FindControl("Label1"), Label))
                    Dim txtEditUnit2 As TextBox = (TryCast(row.FindControl("txtEditUnit2"), TextBox))
                    'Dim txtEditUnit2 As TextBox = (TryCast(row.FindControl("txtEditUnit2"), TextBox))
                    'Dim lblU1Cost As Label = (TryCast(row.FindControl("lblU1Cost"), Label))
                    'Dim lblttl_Cost As Label = (TryCast(row.FindControl("lblttlCost"), Label))
                    'Dim txtEditU1Cost As TextBox = (TryCast(row.FindControl("txtEditU1Cost"), TextBox))
                    lblEditUnit2.Visible = True
                    txtEditUnit2.Visible = False
                    'Label1.Visible = True
                    'txtEditUnit2.Visible = False
                    'lblU1Cost.Visible = True
                    'txtEditU1Cost.Visible = False
                    lblEditUnit2.Text = txtEditUnit2.Text
                    'Dim lblLosF1 As Label = (TryCast(row.FindControl("lblfactor"), Label))
                    'Dim total As Decimal = (Val(txtEditUnit1.Text) * Val(lblLosF1.Text)) + Val(txtEditUnit2.Text)
                    If IsNumeric(txtEditUnit2.Text) = False Then
                        msg = msg + " row " + row.RowIndex.ToString + " Unit 1 must be number" & newLine

                    End If
                    'Label1.Text = txtEditUnit2.Text
                    'If IsNumeric(txtEditUnit2.Text) = False Then
                    '    msg = msg + " row " + row.RowIndex.ToString + " Unit 2 must be number" & newLine

                    'End If
                    ' lblU1Cost.Text = txtEditU1Cost.Text
                    'If IsNumeric(txtEditU1Cost.Text) = False Then
                    '    msg = msg + " row " + row.RowIndex.ToString + " cost must be number" & newLine
                    '    txtEditU1Cost.Text = 0
                    'End If

                    'If txtEditU1Cost.Text = 0 Then

                    '    msg = msg + " row " + row.RowIndex.ToString + " Please Input Cost" &
                    '        txtEditU1Cost.Text = 0
                    '    txtEditUnit2.Text = 0

                    'End If

                    If Val(txtEditUnit2.Text) >= 999999999 Then

                        msg = msg + " row " + row.RowIndex.ToString + " Unit 1 must less than 999999" & newLine
                    End If

                    'If Val(txtEditU1Cost.Text) >= 999999999 Then
                    '    msg = msg + " row " + row.RowIndex.ToString + " Cost must less than 999999" & newLine
                    'End If

                    'If Val(total) >= 999999999 Then

                    '    msg = msg + " row " + row.RowIndex.ToString + " Total Stock must less than 999999" & newLine

                    'End If


                    'Dim lblUnit_1 As Label = (TryCast(row.FindControl("lblEditEmpID"), Label))

                    Dim dt As DataTable = DirectCast(ViewState("orderW"), DataTable)
                    Dim clsI_stk As New clsInventory_stk

                    'If lblLosF1.Text = 0 Then
                    '    lblLosF1.Text = 1
                    '    total = (Val(txtEditUnit1.Text) * Val(lblLosF1.Text)) + Val(txtEditUnit2.Text)
                    'End If



                    If msg <> "" Then
                        notsuccesfull(msg)
                        Exit Sub
                    End If

                    Dim lbl_stkid As Label = (TryCast(row.FindControl("lblStkID"), Label))


                    'dt.Rows(row.RowIndex)("Factor1") = clsI_stk.get_stkFactor1(lbl_stkid.Text, custid) 'lblLosF1.Text

                    dt.Rows(row.RowIndex)("totalU2") = txtEditUnit2.Text

                    'dt.Rows(row.RowIndex)("U2") = txtEditUnit2.Text

                    'dt.Rows(row.RowIndex)("ttlStock") = total

                    'dt.Rows(row.RowIndex)("CostU1") = txtEditU1Cost.Text


                    'Dim cost As Decimal = txtEditU1Cost.Text / lblLosF1.Text

                    'dt.Rows(row.RowIndex)("Cost") = cost

                    'Dim ttlcost As Decimal = total * cost
                    'dt.Rows(row.RowIndex)("ttlCost") = ttlcost
                    dt.AcceptChanges()
                    bindingstk()
                    BindData(modulepriv)

                    'Dim totalcost As Double = 0

                    'For Each dr As DataRow In dt.Rows
                    '    totalcost += Convert.ToDouble(dr("ttlCost"))
                    'Next

                    'DgvTransfer.Columns(11).FooterText = totalcost.ToString("#,##0.00")

                    'DgvTransfer.DataSource = dt
                    'DgvTransfer.DataBind()
                    'DgvTransfer.ShowFooter = True


                End If
            End If
        Next
    End Sub
    Sub messagesuccesfull(ByVal msg As String, Optional ByVal bsuccess As Boolean = True)

        Dim message As String = msg
        Dim url As String
        If bsuccess Then
            url = "../Pages_InvOrderW.aspx"
        Else
            url = "Default.aspx"
        End If

        Dim script As String = "window.onload = function(){ alert('"
        script += message
        script += "');"
        script += "window.location = '"
        script += url
        script += "'; }"
        ClientScript.RegisterStartupScript(Me.GetType(), "Redirect", script, True)

    End Sub
    Protected Sub btnedit_ServerClick(sender As Object, e As EventArgs) Handles btnedit.ServerClick
        Try
            If btnedit.InnerText = "edit" Then
                For Each row As GridViewRow In DgvTransfer.Rows

                    If row.RowType = DataControlRowType.DataRow Then
                        row.FindControl("txtEditUnit2").Visible = True
                        row.FindControl("lblEditUnit2").Visible = False
                        'lblUnit1
                        'row.FindControl("txtEditUnit2").Visible = True
                        'row.FindControl("Label1").Visible = False
                        'row.FindControl("txtEditU1Cost").Visible = True
                        'row.FindControl("lblU1Cost").Visible = False

                    End If
                Next

                'btnedit.InnerText = "Calc."
                btnedit.InnerHtml = "<i class='fas fa-sync'></i>"
            Else
                For Each row As GridViewRow In DgvTransfer.Rows

                    If row.RowType = DataControlRowType.DataRow Then
                        row.FindControl("txtEditUnit2").Visible = False
                        row.FindControl("lblEditUnit2").Visible = True
                        'lblUnit1
                        'row.FindControl("txtEditUnit2").Visible = False
                        'row.FindControl("Label1").Visible = True
                        'row.FindControl("txtEditU1Cost").Visible = False
                        'row.FindControl("lblU1Cost").Visible = True

                    End If
                Next
                update()
                btnedit.InnerText = "edit"
            End If


        Catch ex As Exception
            errLog(Err.Description, "Pages_InvOrderWDetail:btnedit_ServerClick")
            Err.Clear()
        End Try
    End Sub
    Protected Sub btnSetAll_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSetAll.ServerClick
        Dim dt As DataTable = DirectCast(ViewState("orderW"), DataTable)
        Try
            If dt.Rows.Count = 0 Then
                notsuccesfull("you must ADD item before Order Stock Limit")
            Else

                For Each row As DataRow In dt.Rows
                    row.Item("totalu2") = row.Item("stklimit") - row.Item("stockcc")
                    If row.Item("totalu2") < 0 Then
                        row.Item("totalu2") = row.Item("stklimit")
                    End If
                    dt.AcceptChanges()
                Next row


            End If
            BindData(modulepriv)
        Catch ex As Exception
            errLog(Err.Description, "Pages_InvOrderWDetail:btnSetAll_Click")
            Err.Clear()
        End Try


    End Sub
    Protected Sub imgbtnDelete_Click(sender As Object, e As ImageClickEventArgs)
        Dim lb As ImageButton = CType(sender, ImageButton)
        Dim gvRow As GridViewRow = CType(lb.NamingContainer, GridViewRow)
        Dim rowID As Integer = gvRow.RowIndex
        Dim c_cmd As New clsDbo("")
        Dim lid As String = ""
        Try
            If ViewState("orderW") IsNot Nothing Then

                Dim lblID As Label = DirectCast(DgvTransfer.Rows(rowID).FindControl("lblID"), Label)
                Dim lblstknm As Label = DirectCast(DgvTransfer.Rows(rowID).FindControl("lblName"), Label)
                Dim lblstkid As Label = DirectCast(DgvTransfer.Rows(rowID).FindControl("lblStkID"), Label)

                Dim dt As DataTable = CType(ViewState("orderW"), DataTable)
                Dim dtstk As DataTable = DirectCast(ViewState("InvStk"), DataTable)
                Dim xdoctot As Double
                ' If dt.Rows.Count > 1 Then


                Dim confirmValue As String = Request.Form("confirm_value")
                If confirmValue = "Yes" Then
                    If gvRow.RowIndex <= dt.Rows.Count - 1 Then
                        lid = dt.Rows(rowID).Item("stk_id")
                        clsInv.DeleteInvWare(dt.Rows(rowID).Item("wID"), icustid, ddlFrom.SelectedValue, dt.Rows(rowID).Item("stk_ID"), username)
                        'cInv.DeleteInvRcvDetail(dt.Rows(rowID).Item("ID"), custid, username)
                        dt.Rows.Remove(dt.Rows(rowID))
                        dt.AcceptChanges()
                        dtstk.Rows.Add(lblstkid.Text, lblstknm.Text)
                        ViewState("InvStk") = dtstk

                        ResetRowID(dt)
                    End If
                Else
                    Exit Sub

                End If

                'End If

                ViewState("orderW") = dt

                    Dim totalcost As Double = 0

                    'For Each dr As DataRow In dt.Rows
                    '    totalcost += Convert.ToDouble(dr("ttlCost"))
                    'Next

                    '  DgvTransfer.Columns(11).FooterText = totalcost.ToString("#,##0.00")

                    DgvTransfer.DataSource = dt
                DgvTransfer.DataBind()
                If DgvTransfer.Rows.Count = 0 Then
                    btnedit.Visible = False
                Else
                    btnedit.Visible = True
                End If
                'DgvTransfer.ShowFooter = True
                'If dt.Rows.Count > 0 Then
                '    xdoctot = dt.Compute("sum(ttlCost)", String.Empty)
                'Else
                '    xdoctot = 0
                'End If

                'strmaster = "update cld_invlk_rcv set  doctot = " & xdoctot & " where id = '" & lid & "' and custid = " & custid() & ""
                '" & dt.Rows(rowID).Item("linkid") & "'
                'c_cmd.executeNonQuery(strmaster)
            End If
        Catch ex As Exception
            errLog(Err.Description, "Pages_InvRcvEdit:imgbtnDelete_Click")
            Err.Clear()
        End Try


    End Sub


End Class
