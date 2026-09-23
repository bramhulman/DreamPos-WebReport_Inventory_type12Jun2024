Imports System.Threading
Imports System.Net
Imports System.Text
Imports System.IO
Imports System.Data
Imports System.Drawing

Public Class Pages_RtnWHEdit
    Inherits System.Web.UI.Page
    Private sNameuser As String
    Private Userid As Integer
    Private icustid As Integer
    Private imodul As Integer
    Private iModPriv As Integer
    Private irptpriv As Integer
    Private irptf As Integer
    Private mfunction As New ModFunction
    Private Mtid As String
    Private wCc As Integer
    Private wFrom As Integer
    Private wDel As Date
    Private wNotes As String

    Private CCf As Integer

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
        Try
            Mtid = Request.Cookies("ID").Value
            CCf = Request.Cookies("CcF").Value


            If Request.Cookies("UserLogin_inv") IsNot Nothing Then
                sNameuser = username()
                icustid = custid()
                imodul = modul()
                iModPriv = modulepriv()
                irptpriv = rptpriv()
                irptf = rptf()

                If IsPostBack = False Then


                    loadFrom()
                    'loadTo()
                    If Mtid = "" Then
                        ddlFrom.SelectedValue = 0

                        Dim dt, dt2 As New DataTable
                        dt = cInv.getDTInvRtntoWH(Mtid, custid) 'WService.getDTInvTrans(Mtid, cust)

                        ViewState("InvTrans") = dt
                        BindData()

                        Bindingstk1()
                    Else
                        ddlFrom.Enabled = False
                        ddlFrom.SelectedValue = CCf
                        txtLinkid.Text = Mtid

                        Dim dt, dt2 As New DataTable
                        dt = cInv.getDTInvRtntoWH(Mtid, custid) 'WService.getDTInvTrans(Mtid, cust)

                        ViewState("InvTrans") = dt
                        BindData()

                        Bindingstk1()

                    End If


                End If
                ScriptManager.GetCurrent(Me).RegisterPostBackControl(RadButton1)
                'This for Search Button
                'txtSearch.Attributes.Add("onkeyup", Me.Page.ClientScript.GetPostBackEventReference(txtSearch, ""))

                'ScriptManager.GetCurrent(Me).RegisterPostBackControl(RadButton1)
                txtSearch.Attributes.Add("onfocus", "javascript: this.setSelectionRange(this.value.length,this.value.length);")
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

    End Sub

    Protected Sub BindData()
        If custid() = 0 Then
            Response.Redirect("~/Default.aspx")
        Else


            DgvTransfer.DataSource = DirectCast(ViewState("InvTrans"), DataTable)
            DgvTransfer.DataBind()


        End If
    End Sub
    Sub Bindingstk1()
        If icustid = 0 Then
            Response.Redirect("~/Default.aspx", False)
        Else

            'Bram4Sep2018 Update Report
            Dim dt, dt2 As New DataTable
            Dim clsStk As New clsInventory_stk

            dt = DirectCast(ViewState("InvTrans"), DataTable)
            If dt Is Nothing Then dt = New DataTable()

            ' Simply re-fetch here as per original logic, but ensure dt2 has correct structure
            dt2 = clsStk.SelectDataTable_Stk_wStock(dt, custid, txtSearch.Text, ddlFrom.SelectedValue)
            ViewState("InvStk") = dt2

            If dt2.Rows.Count > 0 Then
                LsvStk.Items.Clear()
                countlvstore = dt2.Rows.Count
                LsvStk.DataSource = dt2.DefaultView
                LsvStk.DataBind()
                UpdatePanel3.Update()
            End If


        End If
    End Sub
    Sub bindingstk()

        If custid() = 0 Then
            Response.Redirect("~/Default.aspx")
        Else
            Dim dt, dt2 As New DataTable
            dt = DirectCast(ViewState("InvTrans"), DataTable)

            Debug.Print(dt.Rows.Count.ToString)
            'dt2 = clsStk.SelectDataTable_Stk_wStock(dt, custid, txtSearch.Text, ddlFrom.SelectedValue)
            'ViewState("InvStk") = dt2
            'LsvStk.DataSource = DirectCast(ViewState("InvStk"), DataTable)
            'LsvStk.DataBind()

            dt2 = DirectCast(ViewState("InvStk"), DataTable)

            Dim clsStk As New clsInventory_stk

            If dt Is Nothing Then dt = New DataTable()

            ' Check if we need to refresh data (empty or missing "stock" column)
            Dim refreshData As Boolean = True
            If Not dt2 Is Nothing Then
                If dt2.Columns.Contains("stock") AndAlso dt2.Rows.Count > 0 Then
                    refreshData = False
                End If
            End If

            If refreshData Then
                dt2 = clsStk.SelectDataTable_Stk_wStock(dt, custid, txtSearch.Text, ddlFrom.SelectedValue)
                ViewState("InvStk") = dt2
            Else
                dt2.DefaultView.RowFilter = "name like '%" & txtSearch.Text & "%'"
                ' ViewState("InvStk") = dt2 ' Not strictly needed if we just modified the view of the existing object but good for safety
            End If

            If dt2.Rows.Count > 0 Then
                LsvStk.Items.Clear()
                countlvstore = dt2.Rows.Count
                LsvStk.DataSource = dt2.DefaultView
                LsvStk.DataBind()
                UpdatePanel3.Update()

            End If

        End If


    End Sub

    Protected Sub Page_unLoad(sender As Object, e As EventArgs) Handles Me.Unload
    End Sub

    Public Function username() As String
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        Return cookie("userName")

    End Function


    'Sub delete(ByVal id As Integer)

    '    ' WService.DeleteInvTrans(id, cust, username)
    '    cInv.DeleteInvTrans(id, custid, username)

    'End Sub
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
            errLog(Err.Description, "Pages_RtnWHEdit:DeleteRcpDtl")
            Err.Clear()
        End Try
    End Sub







    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.ServerClick
        'If txtSearch.Text = "" Then
        '    ShowToastr(Page, "Text Search tidak boleh kosong", "Error", "Error", False, "toast-top-right", False)
        'Else
        BindCheckItem()
        bindingstk()
        CheckedItem()
        'End If
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Pop", "openModalItem();", True)

    End Sub
    Protected Sub RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow AndAlso DgvTransfer.EditIndex = e.Row.RowIndex Then
            Dim ddlTo As DropDownList = DirectCast(e.Row.FindControl("DdlTo"), DropDownList)
            Dim lblCc As Label = DirectCast(e.Row.FindControl("lblEditCc"), Label)

            Dim ds As New DataSet
            ds = cInv.getInvcc(custid, username, modulepriv, 2)

            Dim dt As New DataTable
            dt = ds.Tables(0)
            dt.DefaultView.RowFilter = "id<>'" & ddlFrom.SelectedValue & "'"

            'WService.getIcc(cust, username, ModPrv, 1)

            ddlTo.DataSource = dt.DefaultView
            ddlTo.DataValueField = "Id"
            ddlTo.DataTextField = "Name"
            ddlTo.DataBind()
            ddlTo.Items.Insert(0, "")

            If lblCc.Text <> 0 Then
                ddlTo.Items.FindByValue(TryCast(e.Row.FindControl("lblEditCc"), Label).Text).Selected = True
            End If

        End If
    End Sub

    'Protected Sub btnFinish_Click(sender As Object, e As EventArgs)
    '    wID = insert(2)
    'End Sub
    Sub insert()
        Dim clsprocess As New clsInventoryProcess
        If Mtid <> "" Then
            delete(Mtid)
            txtLinkid.Text = ""
            Mtid = ""
        End If
        Try
            If Mtid <> "" And CCf <> 0 Then
                Dim cinvrtntoWH As New clsInventory_RtntoWH
                Dim dt1 As DataTable = cinvrtntoWH.SelectDataTable_invtrans_RtntoWH(Mtid, custid)
                If dt1.Rows.Count > 0 Then
                    Dim cInv As New clsInventory
                    ' Dim clsprocess As New clsInventoryProcess
                    Dim clsicc As New Cls_ICc
                    For i As Integer = 0 To dt1.Rows.Count - 1
                        clsprocess.InvstkId = dt1.Rows(i).Item("stk_id").ToString
                        clsprocess.InvCc = dt1.Rows(i).Item("cc").ToString
                        clsprocess.InvCcName = dt1.Rows(i).Item("cc_name").ToString
                        clsprocess.InvCustId = custid()
                        clsprocess.InvstkName = dt1.Rows(i).Item("Stk_Name").ToString
                        'clsprocess.t_in = -(dt1.Rows(i).Item("ttlStock").ToString)
                        'clsprocess.t_out = 0
                        clsprocess.ho_in = -(dt1.Rows(i).Item("ttlStock").ToString)
                        clsprocess.ho_out = 0
                        clsprocess.InvStkNo = cInv.getStkByid(dt1.Rows(i).Item("stk_id").ToString, custid).Tables(0).Rows(0).Item("Number").ToString
                        clsprocess.InvDate = Date.Now.Date.ToString("yyyy-MM-dd 00:00:00")
                        clsprocess.BCost = False
                        'cInv.executeNonQuery_Inv_ProcessCls(Date.Now.Date.ToString("yyyy-MM-dd"), ddlFrom.SelectedValue, ddlFrom.SelectedItem.Text, stkid, stknumber, stkname, custid, 0, stktotal)
                        clsprocess.executeNonQuery_Inv_ProcessCls()

                        clsprocess.InvstkId = dt1.Rows(i).Item("stk_id").ToString
                        clsprocess.InvCc = dt1.Rows(i).Item("ccfrom").ToString
                        clsprocess.InvCcName = clsicc.SelectDataIcc(dt1.Rows(i).Item("ccfrom").ToString).Tables(0).Rows(0).Item("name")
                        clsprocess.InvCustId = custid()
                        clsprocess.InvstkName = dt1.Rows(i).Item("Stk_Name").ToString
                        clsprocess.ho_in = 0
                        clsprocess.ho_out = -(dt1.Rows(i).Item("ttlStock").ToString)
                        clsprocess.InvStkNo = cInv.getStkByid(dt1.Rows(i).Item("stk_id").ToString, custid).Tables(0).Rows(0).Item("Number").ToString
                        clsprocess.InvDate = Date.Now.Date.ToString("yyyy-MM-dd 00:00:00")
                        'cInv.executeNonQuery_Inv_ProcessCls(Date.Now.Date.ToString("yyyy-MM-dd"), ddlFrom.SelectedValue, ddlFrom.SelectedItem.Text, stkid, stknumber, stkname, custid, 0, stktotal)
                        clsprocess.executeNonQuery_Inv_ProcessCls()
                    Next i
                End If
            End If
            Dim msg As String = ""
            Dim dt As DataTable = DirectCast(ViewState("InvTrans"), DataTable)
            Dim newLine As String = "\n"
            If ddlFrom.SelectedValue = 0 Then
                msg = msg + "Cost Centre From Not Fill " & newLine
            End If
            If dt.Rows.Count = 0 Then
                msg = msg + "Item is Nothing" & newLine
            End If
            For Each row As DataRow In dt.Rows
                'If row.Item("Cc") = 0 Or row.Item("U1") = 0 And row.Item("U2") Then
                'ttlStock

                If row.Item("Cc") = 0 Or row.Item("ttlStock") = 0 Then
                    msg = msg + row.Item("Stk_name") + " Not Fill " & newLine
                End If
                If ddlFrom.SelectedValue > 0 Then
                    If row.Item("Cc") = ddlFrom.SelectedValue Then
                        msg = msg + row.Item("Stk_name") + " Cost Centre From and To Cannot Same " & newLine
                    End If
                End If


            Next row
            If msg = "" Then

                cInv.insertInvrtntoWH(txtLinkid.Text, ddlFrom.SelectedValue, dt, username, custid)

            Else
                notsuccesfull(msg)
                Exit Sub
            End If

            'Dim clsprocess As New clsInventoryProcess



            For i As Integer = 0 To dt.Rows.Count - 1
                'tin
                clsprocess.InvstkId = dt.Rows(i).Item("stk_id").ToString
                clsprocess.InvCc = dt.Rows(i).Item("cc").ToString
                clsprocess.InvCcName = dt.Rows(i).Item("cc_name").ToString
                clsprocess.InvCustId = custid()
                clsprocess.InvstkName = dt.Rows(i).Item("Stk_Name").ToString
                clsprocess.ho_in = dt.Rows(i).Item("ttlStock").ToString
                clsprocess.ho_out = 0
                clsprocess.InvStkNo = cInv.getStkByid(dt.Rows(i).Item("stk_id").ToString, custid).Tables(0).Rows(0).Item("Number").ToString
                clsprocess.InvDate = Date.Now.Date.ToString("yyyy-MM-dd 00:00:00")
                clsprocess.BCost = True
                'cInv.executeNonQuery_Inv_ProcessCls(Date.Now.Date.ToString("yyyy-MM-dd"), ddlFrom.SelectedValue, ddlFrom.SelectedItem.Text, stkid, stknumber, stkname, custid, 0, stktotal)
                clsprocess.executeNonQuery_Inv_ProcessCls()



                clsprocess.InvstkId = dt.Rows(i).Item("stk_id").ToString
                clsprocess.InvCc = ddlFrom.SelectedValue
                clsprocess.InvCcName = ddlFrom.SelectedItem.Text
                clsprocess.InvCustId = custid()
                clsprocess.InvstkName = dt.Rows(i).Item("Stk_Name").ToString
                clsprocess.ho_out = dt.Rows(i).Item("ttlStock").ToString
                clsprocess.ho_in = 0
                clsprocess.InvStkNo = cInv.getStkByid(dt.Rows(i).Item("stk_id").ToString, custid).Tables(0).Rows(0).Item("Number").ToString
                clsprocess.InvDate = Date.Now.Date.ToString("yyyy-MM-dd 00:00:00")
                'cInv.executeNonQuery_Inv_ProcessCls(Date.Now.Date.ToString("yyyy-MM-dd"), ddlFrom.SelectedValue, ddlFrom.SelectedItem.Text, stkid, stknumber, stkname, custid, 0, stktotal)
                clsprocess.executeNonQuery_Inv_ProcessCls()
                'tout
            Next i


            Response.Redirect("~/Pages_RtnWH.aspx")

        Catch ex As Exception

            'MsgBox(Err.Description, MsgBoxStyle.Information)
            Err.Clear()
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
    Protected Sub DgvTransfer_RowDeleting(ByVal sender As Object, ByVal e As GridViewDeleteEventArgs)
        'waktu delete record di list
        Dim lblID As Label = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("lblID"), Label)
        Dim lblstknm As Label = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("lblName"), Label)
        Dim lblstkid As Label = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("lblStkID"), Label)
        Dim dt As DataTable = DirectCast(ViewState("InvTrans"), DataTable)
        Dim dtstk As DataTable = DirectCast(ViewState("InvStk"), DataTable)
        If lblID.Text = 0 Or lblID.Text = "" Then
            dtstk.Rows.Add(lblstkid.Text, lblstknm.Text, "")
            dtstk.AcceptChanges()
            ViewState("InvStk") = dtstk
            dt.Rows(e.RowIndex).Delete()
            dt.AcceptChanges()
        Else
            dtstk.Rows.Add(lblstkid.Text, lblstknm.Text)
            ViewState("InvStk") = dtstk
            Dim confirmValue As String = Request.Form("confirm_value")
            If confirmValue = "Yes" Then
                Dim id As Integer = lblID.Text
                delete(id)
            Else
                Exit Sub
            End If
            dt.Rows(e.RowIndex).Delete()
            dt.AcceptChanges()
        End If
        BindData()
        bindingstk()

    End Sub
    Protected Sub DgvTransfer_RowUpdating(ByVal sender As Object, ByVal e As GridViewUpdateEventArgs)

        Dim txtUnit1 As TextBox = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("txtEditUnit1"), TextBox)
        Dim txtUnit2 As TextBox = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("txtEditUnit2"), TextBox)
        Dim lblLosF1 As Label = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("lblEditfactor"), Label)
        Dim txtlblid As Label = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("lblStkEditID"), Label)

        'Dim txtCost As Label = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("txtEditcost"), Label)
        Dim ddlTo As DropDownList = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("DdlTo"), DropDownList)
        Dim lblstckqty As Label = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("lblEditStock"), Label)

        '12 okt 2017, perubahan jadi ambil data total awal, karena jika edit
        ' hrusnya total baru di kurangi total awal baru dbandingan dengan current stock
        Dim txtTotaltrfawal As Label = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("lblEdittlStock"), Label)

        If IsNumeric(txtUnit1.Text) = False Then
            txtUnit1.Text = 0
        End If

        If IsNumeric(txtUnit2.Text) = False Then
            txtUnit2.Text = 0
        End If

        If ddlTo.SelectedValue = "" Then
            notsuccesfull("Please Select Cost Centre")
            Exit Sub
        End If

        Dim total As Decimal = Val(txtUnit1.Text) * Val(lblLosF1.Text) + Val(txtUnit2.Text)
        If total - txtTotaltrfawal.Text > lblstckqty.Text Then
            notsuccesfull("Total Transfer > Stock")
            txtUnit1.Text = 0
            txtUnit2.Text = 0
            Exit Sub
        ElseIf ddlFrom.SelectedValue = ddlTo.SelectedValue Then
            notsuccesfull("Cost Centre From and To Cannot Same")
            txtUnit1.Text = 0
            txtUnit2.Text = 0
            Exit Sub
            'Else
            '    DgvTransfer.EditIndex = -1
            '    BindData()
        End If
        Dim lblUnit1 As Label = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("lblEditEmpID"), Label)

        Dim dt As DataTable = DirectCast(ViewState("InvTrans"), DataTable)
        dt.Rows(e.RowIndex)("Cc") = ddlTo.SelectedValue
        dt.Rows(e.RowIndex)("Cc_name") = ddlTo.SelectedItem.Text
        dt.Rows(e.RowIndex)("U1") = txtUnit1.Text
        dt.Rows(e.RowIndex)("U2") = txtUnit2.Text
        dt.Rows(e.RowIndex)("ttlStock") = total
        Dim dsA As DataSet = cInv.getStkcostByid(txtlblid.Text, ddlFrom.SelectedValue, custid, CDate(Now()))
        Dim cost As Double = 0
        If dsA.Tables(0).Rows.Count > 0 Then
            cost = dsA.Tables(0).Rows(0).Item("cost")
        Else
            cost = cInv.getStkcostByid(txtlblid.Text, ddlFrom.SelectedValue, custid, CDate(Now())).Tables(0).Rows(0).Item("cost")
        End If
        dt.Rows(e.RowIndex)("Cost") = cost
        dt.AcceptChanges()
        BindData()

        'If total - txtTotaltrfawal.Text > lblstckqty.Text Then
        '    notsuccesfull("Total Transfer > Stock")
        '    Exit Sub
        'ElseIf ddlFrom.SelectedValue = ddlTo.SelectedValue Then
        '    notsuccesfull("Cost Centre From and To Cannot Same")
        '    Exit Sub
        'Else
        DgvTransfer.EditIndex = -1
        BindData()
        'End If

    End Sub
    Protected Sub DgvTransfer_RowCancelingEdit(ByVal sender As Object, ByVal e As GridViewCancelEditEventArgs)
        DgvTransfer.EditIndex = -1
        BindData()
    End Sub

    Protected Sub DgvTransfer_RowEditing(ByVal sender As Object, ByVal e As GridViewEditEventArgs)
        DgvTransfer.EditIndex = e.NewEditIndex
        BindData()
    End Sub
    Protected Sub btnClearSearch_Click(sender As Object, e As EventArgs)
        For Each item As ListViewItem In LsvStk.Items
            Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox"), CheckBox)

            ck.Checked = False


        Next
        txtSearch.Text = ""
        lblnoitem.Text = 0
        'BindData()
        Bindingstk1()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Pop", "openModalItem();", True)
    End Sub
    Private Sub ShowToastr(ByVal page As Page, ByVal message As String, ByVal title As String, Optional ByVal type As String = "info", Optional ByVal clearToast As Boolean = False, Optional ByVal pos As String = "toast-top-left", Optional ByVal Sticky As Boolean = False)
        Dim toastrScript As String = [String].Format("Notify('{0}','{1}','{2}', '{3}', '{4}', '{5}');", message, title, type, clearToast, pos, Sticky)
        page.ClientScript.RegisterStartupScript(page.[GetType](), "toastr_message", toastrScript, addScriptTags:=True)
    End Sub
    <Serializable>
    Public Class Item_Detail 'Class untuk ListItem
        Public Property ItemID As String = ""
        Public Property ItemName As String = ""
        Public Property Status As Boolean = False
    End Class
    'Start : Store
    Private Sub BindCheckItem()
        Try
            Dim ListItem2 As List(Of Item_Detail)
            If ViewState("ListItem") Is Nothing Then
                ListItem2 = New List(Of Item_Detail)()
            Else
                ListItem2 = TryCast(ViewState("ListItem"), List(Of Item_Detail))
                If ListItem2 Is Nothing Then ListItem2 = New List(Of Item_Detail)()
            End If

            For Each item As ListViewItem In LsvStk.Items
                Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox"), CheckBox)
                Dim Labels As Label = DirectCast(item.FindControl("lblName"), Label)
                Dim lblID As Label = DirectCast(item.FindControl("lblID"), Label)

                Dim id As String = lblID.Text.Trim()
                Dim name As String = Labels.Text

                Dim found As Boolean = False
                For Each a In ListItem2
                    If a.ItemID = id Then
                        a.Status = ck.Checked
                        found = True
                        Exit For
                    End If
                Next

                If Not found Then
                    ListItem2.Add(New Item_Detail With {
                        .ItemID = id,
                        .ItemName = name,
                        .Status = ck.Checked
                    })
                End If
            Next
            ViewState("ListItem") = ListItem2
        Catch ex As Exception
        End Try
    End Sub

    Private Sub CheckedItem()
        Try
            Dim ListStore As List(Of Item_Detail) = TryCast(ViewState("ListItem"), List(Of Item_Detail))
            If ListStore Is Nothing Then Exit Sub

            For Each item As ListViewItem In LsvStk.Items
                Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox"), CheckBox)
                Dim lblID As Label = DirectCast(item.FindControl("lblID"), Label)
                Dim id As String = lblID.Text.Trim()

                For Each a In ListStore
                    If a.ItemID = id Then
                        ck.Checked = a.Status
                        Exit For
                    End If
                Next
            Next
        Catch ex As Exception
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
        If ddlFrom.SelectedValue <> 0 Then
            Add2()
            Session("xfrom") = ddlFrom.SelectedValue
        Else
            notsuccesfull("you must choose Cost Centre first")
        End If

        'Add()
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
            'MsgBox(ex.Message)
        End Try

    End Sub
    Sub Add2()
        Try
            ' Ensure latest check state is captured
            BindCheckItem()

            Dim ListItem2 As List(Of Item_Detail) = TryCast(ViewState("ListItem"), List(Of Item_Detail))
            Dim dt As DataTable = DirectCast(ViewState("InvTrans"), DataTable)
            Dim dtStk As DataTable = DirectCast(ViewState("InvStk"), DataTable)

            If ListItem2 IsNot Nothing Then
                For Each itemDetail As Item_Detail In ListItem2
                    If itemDetail.Status = True Then
                        Try
                            Dim cost As Integer
                            Dim dsA As DataSet = cInv.getStkcostByid(itemDetail.ItemID, ddlFrom.SelectedValue, custid, CDate(Now()))
                            If dsA.Tables(0).Rows.Count > 0 Then
                                cost = dsA.Tables(0).Rows(0).Item("cost")
                            Else
                                cost = 0
                            End If

                            Dim ds As DataSet = cInv.getStkByid2(itemDetail.ItemID, 0, custid, ddlFrom.SelectedValue, CDate(Now()))

                            If ds.Tables(0).Rows.Count > 0 Then
                                dt.Rows.Add(0, custid, Date.Now.ToString("yyyy-MM-dd"), 1, 0, itemDetail.ItemID, itemDetail.ItemName, 0, 0, ds.Tables(0).Rows(0).Item("Factor1"), 0, 0, cost, "", 1, ds.Tables(0).Rows(0).Item("Unit1").ToString, ds.Tables(0).Rows(0).Item("Unit2").ToString, username, Date.Now.ToString("yyyy-MM-dd"), "", Date.Now.ToString("yyyy-MM-dd"), ds.Tables(0).Rows(0).Item("Number").ToString, "", ds.Tables(0).Rows(0).Item("Stock").ToString)

                                ' Remove from available stock list
                                For i As Integer = dtStk.Rows.Count - 1 To 0 Step -1
                                    If dtStk.Rows(i)("id").ToString() = itemDetail.ItemID Then
                                        dtStk.Rows(i).Delete()
                                        Exit For
                                    End If
                                Next
                            End If

                        Catch error_t As Exception
                            MsgBox(error_t.ToString)
                        End Try
                    End If
                Next
            End If

            ViewState("InvTrans") = dt
            dtStk.AcceptChanges()
            ViewState("InvStk") = dtStk

            ' Clear selection state after adding
            If ListItem2 IsNot Nothing Then
                For Each itemDetail As Item_Detail In ListItem2
                    itemDetail.Status = False
                Next
                ViewState("ListItem") = ListItem2
            End If

            BindData()
            txtSearch.Text = ""
            bindingstk()

        Catch ex As Exception
            'MsgBox(ex.Message)
        End Try
    End Sub

    Protected Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        Threading.Thread.Sleep(500)
        BindCheckItem()
        bindingstk()
        CheckedItem()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Pop", "openModalItem();", True)
        txtSearch.Attributes("onfocus") = "javascript:this.setSelectionRange(this.value.length,this.value.length);"
        txtSearch.Focus()
    End Sub
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSave.ServerClick
        insert()
    End Sub

    Protected Sub btnBack_Click(sender As Object, e As EventArgs) 'Handles btnBack.ServerClick

        Response.Redirect("~/Pages_RtnWH.aspx")
    End Sub

    Protected Sub ddlFrom_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlFrom.SelectedIndexChanged
        Dim dt As DataTable = DirectCast(ViewState("InvTrans"), DataTable)
        ViewState("InvStk") = cInv.getDtStk2(dt, custid, txtSearch.Text, modulepriv, ddlFrom.SelectedValue)
        Try
            If Session("xfrom") Is Nothing Then
                Session("xfrom") = "0"
            End If
            If dt.Rows.Count > 0 And ddlFrom.SelectedValue <> Session("xfrom").ToString Then
                notsuccesfull("you must delete item before change CC")
                ddlFrom.SelectedValue = Session("xfrom").ToString
            Else
                For Each row As DataRow In dt.Rows
                    row.Item("stock") = cInv.getStkByid2(row.Item("stk_id"), ddlFrom.SelectedValue, custid, 0, CDate(Now())).Tables(0).Rows(0).Item("stock")
                    row.Item("cost") = cInv.getStkcostByid(row.Item("stk_id"), ddlFrom.SelectedValue, custid, CDate(Now())).Tables(0).Rows(0).Item("cost")
                    dt.AcceptChanges()
                    'BindData()
                    'Dim txtTotaltrfawal As Label = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("lblEdittlStock"), Label)
                    Dim total As Decimal = row.Item("u1") * row.Item("factor1") + row.Item("u2")
                    If total - row.Item("ttlstock") > row.Item("stock") Then
                        notsuccesfull("Total Transfer > Stock")
                        Exit Sub
                    ElseIf ddlFrom.SelectedValue = row.Item("cc") Then
                        notsuccesfull("Cost Centre From and To Cannot Same")
                        Exit Sub
                    Else
                        DgvTransfer.EditIndex = -1

                        BindData()
                    End If
                Next
            End If
            bindingstk()
        Catch ex As Exception

        End Try


    End Sub
    Sub delete(ByVal id As String)
        Dim cinvrtntoRW As New clsInventory_RtntoWH
        Dim clsInv As New clsInventory
        Dim dt As DataTable = cinvrtntoRW.SelectDataTable_invtrans_RtntoWH(id, custid)
        clsInv.DeleteInvRtntoWH(id, custid, username)
        If dt.Rows.Count > 0 Then
            Dim cInv As New clsInventory
            Dim clsprocess As New clsInventoryProcess
            Dim clsicc As New Cls_ICc
            For i As Integer = 0 To dt.Rows.Count - 1
                clsprocess.InvstkId = dt.Rows(i).Item("stk_id").ToString
                clsprocess.InvCc = dt.Rows(i).Item("cc").ToString
                clsprocess.InvCcName = dt.Rows(i).Item("cc_name").ToString
                clsprocess.InvCustId = custid()
                clsprocess.InvstkName = dt.Rows(i).Item("Stk_Name").ToString
                clsprocess.ho_in = -(dt.Rows(i).Item("ttlStock").ToString)
                clsprocess.ho_out = 0
                clsprocess.InvStkNo = cInv.getStkByid(dt.Rows(i).Item("stk_id").ToString, custid).Tables(0).Rows(0).Item("Number").ToString
                clsprocess.InvDate = Date.Now.Date.ToString("yyyy-MM-dd 00:00:00")
                clsprocess.BCost = True
                'cInv.executeNonQuery_Inv_ProcessCls(Date.Now.Date.ToString("yyyy-MM-dd"), ddlFrom.SelectedValue, ddlFrom.SelectedItem.Text, stkid, stknumber, stkname, custid, 0, stktotal)
                clsprocess.executeNonQuery_Inv_ProcessCls()

                clsprocess.InvstkId = dt.Rows(i).Item("stk_id").ToString
                clsprocess.InvCc = dt.Rows(i).Item("ccfrom").ToString
                clsprocess.InvCcName = clsicc.SelectDataIcc(dt.Rows(i).Item("ccfrom").ToString).Tables(0).Rows(0).Item("name")
                clsprocess.InvCustId = custid()
                clsprocess.InvstkName = dt.Rows(i).Item("Stk_Name").ToString
                clsprocess.ho_in = 0
                clsprocess.ho_out = -(dt.Rows(i).Item("ttlStock").ToString)
                clsprocess.InvStkNo = cInv.getStkByid(dt.Rows(i).Item("stk_id").ToString, custid).Tables(0).Rows(0).Item("Number").ToString
                clsprocess.InvDate = Date.Now.Date.ToString("yyyy-MM-dd 00:00:00")
                'cInv.executeNonQuery_Inv_ProcessCls(Date.Now.Date.ToString("yyyy-MM-dd"), ddlFrom.SelectedValue, ddlFrom.SelectedItem.Text, stkid, stknumber, stkname, custid, 0, stktotal)
                clsprocess.executeNonQuery_Inv_ProcessCls()
            Next i
        End If
        'clsInv.DeleteInvTrf(id, custid, username)
        'binding()
    End Sub
End Class
