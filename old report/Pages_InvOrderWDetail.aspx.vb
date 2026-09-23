Imports System.Threading
Imports System.Net
Imports System.Text
Imports System.IO
Imports System.Data
Imports System.Drawing

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
                End If
                ScriptManager.GetCurrent(Me).RegisterPostBackControl(RadButton1)
                'This for Search Button
                txtSearch.Attributes.Add("onkeyup", Me.Page.ClientScript.GetPostBackEventReference(txtSearch, ""))

                'ScriptManager.GetCurrent(Me).RegisterPostBackControl(RadButton1)
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
            errLog(Err.Description, "Pages_ReceipeEdit:Page_Load")
            Err.Clear()
        End Try
    End Sub
    Sub loadFrom()
        Dim ds As New DataSet

        Dim cicc As New clsInventory_Cc
        Try
            ds = cicc.SelectDataset_IccOnly(custid, username, modulepriv)
            cicc = Nothing

            ddlFrom.DataSource = ds
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

    Sub loadwarehouse()

        Dim ds As New DataSet

        Dim CIcc As New clsInventory_Cc
        Try
            ds = CIcc.SelectDataset_IWare(custid)
            CIcc = Nothing

            ddlWare.DataSource = ds
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
    Protected Sub BindData(ByVal modpriv As Integer)
        Try
            If custid() = 0 Then
                Response.Redirect("~/Default.aspx")
            Else

                DgvTransfer.DataSource = DirectCast(ViewState("orderW"), DataTable)
                If modpriv And mHelper.Modulepriv.moduleuser = mHelper.Modulepriv.moduleuser Then
                Else
                    DgvTransfer.Columns(4).Visible = True
                End If
                DgvTransfer.DataBind()
                'DgvTransfer.


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
                Response.Redirect("~/Default.aspx")
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

    Protected Sub Page_unLoad(sender As Object, e As EventArgs) Handles Me.Unload
    End Sub

    Public Function username() As String
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        Return cookie("userName")

    End Function






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
    Protected Sub RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow AndAlso DgvTransfer.EditIndex = e.Row.RowIndex Then

        End If
    End Sub

    'Protected Sub btnFinish_Click(sender As Object, e As EventArgs)
    '    wID = insert(2)
    'End Sub
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
                Exit Function
            Else
                hasil = clsInv.insertInvOrderW(txtID.Text, custid(), 0, 1, ddlWare.SelectedValue, ddlWare.SelectedItem.ToString, ddlFrom.SelectedValue, ddlFrom.SelectedItem.ToString, username, username, CDate(txtcreatedate.Text), dt, "", txtNotes.Text)

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
        End Try


ENDFUNCTION:

        Return hasil

    End Function
    Protected Sub DgvTransfer_RowDeleting(ByVal sender As Object, ByVal e As GridViewDeleteEventArgs)
        'waktu delete record di list

        Dim confirmValue As String = Request.Form("confirm_value")
        Try
            If confirmValue = "Yes" Then


                Dim lblID As Label = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("lblID"), Label)
                Dim lblstknm As Label = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("lblName"), Label)
                Dim lblstkid As Label = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("lblStkID"), Label)
                Dim dt As DataTable = DirectCast(ViewState("orderW"), DataTable)
                ' Dim dtstk As DataTable = DirectCast(ViewState("InvStk"), DataTable)
                If lblID.Text = "" Then
                    'dtstk.Rows.Add(lblstkid.Text, lblstknm.Text)
                    'ViewState("InvStk") = dtstk
                    dt.Rows(e.RowIndex).Delete()
                    dt.AcceptChanges()
                Else
                    'dtstk.Rows.Add(lblstkid.Text, lblstknm.Text)
                    'ViewState("InvStk") = dtstk


                    Dim id As String = lblID.Text
                    Dim istk As Integer = lblstkid.Text
                    delete(id, ddlFrom.SelectedValue, istk)


                    dt.Rows(e.RowIndex).Delete()
                    dt.AcceptChanges()
                End If
                BindData(modulepriv)
                Bindingstk1()


            Else
                Exit Sub
            End If
        Catch ex As Exception
            errLog(Err.Description, "Pages_InvOrderWDetail:DgvTransfer_RowDeleting")
            Err.Clear()
        End Try

    End Sub
    Protected Sub DgvTransfer_RowUpdating(ByVal sender As Object, ByVal e As GridViewUpdateEventArgs)
        Dim txtUnit2 As TextBox = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("txtEditUnit2"), TextBox)
        Dim lblstk As Label = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("lblEditStockwh"), Label)
        Dim lblname As Label = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("lblEditName"), Label)
        Try

            'lblEditName
            If IsNumeric(txtUnit2.Text) = False Then
                notsuccesfull("Unit 1 must be number")
                Exit Sub
            End If
            Dim dt As DataTable = DirectCast(ViewState("orderW"), DataTable)

            If lblstk.Text = 0 And alert1 = 0 Then
                'notsuccesfull(lblname.Text & " => NO STOCK AVAILABLE !")
                dt.Rows(e.RowIndex)("totalu2") = txtUnit2.Text
                alert1 = alert1 + 1
            ElseIf lblstk.Text < txtUnit2.Text And alert2 = 0 Then
                'notsuccesfull(lblname.Text & " => NO STOCK AVAILABLE !")
                dt.Rows(e.RowIndex)("totalu2") = txtUnit2.Text
                alert2 = alert2 + 1
            Else
                dt.Rows(e.RowIndex)("totalu2") = txtUnit2.Text
            End If



            dt.AcceptChanges()

            DgvTransfer.EditIndex = -1
            BindData(modulepriv)
        Catch ex As Exception
            errLog(Err.Description, "Pages_InvOrderWDetail:DgvTransfer_RowUpdating")
            Err.Clear()
        End Try

    End Sub
    Protected Sub DgvTransfer_RowCancelingEdit(ByVal sender As Object, ByVal e As GridViewCancelEditEventArgs)
        DgvTransfer.EditIndex = -1
        BindData(modulepriv)
    End Sub

    Protected Sub DgvTransfer_RowEditing(ByVal sender As Object, ByVal e As GridViewEditEventArgs)
        DgvTransfer.EditIndex = e.NewEditIndex
        BindData(modulepriv)
    End Sub
    Protected Sub btnClearSearch_Click(sender As Object, e As EventArgs)
        Try
            For Each item As ListViewItem In LsvStk.Items
                Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox"), CheckBox)

                ck.Checked = False


            Next
            txtSearch.Text = ""
            lblnoitem.Text = 0
            'BindData()
            Bindingstk1()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Pop", "openModalItem();", True)
        Catch ex As Exception
            errLog(Err.Description, "Pages_InvOrderWDetail:btnClearSearch_Click")
            Err.Clear()
        End Try

    End Sub
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
    Private Sub BindCheckItem()
        Try

            If CBool(IIf(IsNothing(ViewState("ListItem")), True, False)) Then


                Dim ListItem2 As List(Of Item_Detail) = New List(Of Item_Detail)()

                Dim dtorderw, dtitem As New DataTable
                dtorderw = DirectCast(ViewState("orderW"), DataTable)
                dtitem = clsStk.SelectDataTable_Stk_wStock(dtorderw, custid, txtSearch.Text, ddlFrom.SelectedValue)

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
    'END : Store
    'Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs)
    '    Try
    '        Add2()

    '        'BindCheckItem()

    '    Catch ex As Exception

    '    End Try
    'End Sub
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
        Try
            Dim i As Integer = 0
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

                            Dim dt As DataTable = DirectCast(ViewState("orderW"), DataTable)

                            Debug.Print((DirectCast(item.FindControl("lblID"), Label).Text))
                            Debug.Print((DirectCast(item.FindControl("lblName"), Label).Text))
                            Debug.Print(u1.ToString)
                            Debug.Print(factor1.ToString)

                            dt.Rows.Add("", custid, 0, (DirectCast(item.FindControl("lblID"), Label).Text), (DirectCast(item.FindControl("lblName"), Label).Text), 0, u1, 0, u2, 0, 0, 0, dCost, factor1, snumber, u2, dstock, dstockcc, dstock, dstklimit)

                            ViewState("orderW") = dt
                            Dim dtStk As DataTable = DirectCast(ViewState("InvStk"), DataTable)
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
            Next

            'BindData()
        Catch ex As Exception
            'MsgBox(ex.Message)
            errLog(Err.Description, "Pages_InvOrderWDetail:Add2")
            Err.Clear()
        End Try

    End Sub

    Protected Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
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
    'Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As EventArgs)
    '    mfunction.CreateCookie("wType", "1")
    '    Response.Redirect("~/Pages_InvOrderW.aspx")
    'End Sub


    Protected Sub btnFinish_Click(sender As Object, e As EventArgs) 'Handles btnFinish.ServerClick
        wID = insert(2)
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
    Protected Sub btnBack_Click(sender As Object, e As EventArgs) 'Handles btnBack.ServerClick
        Try
            mfunction.CreateCookie("wType", "1")
            Response.Redirect("~/Pages_InvOrderW.aspx", False)
        Catch ex As Exception
            errLog(Err.Description, "Pages_InvOrderWDetail:btnBack_Click")
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
    'Protected Sub DgvTransfer_RowCreated(sender As Object, e As GridViewRowEventArgs) Handles DgvTransfer.RowCreated

    'End Sub
End Class