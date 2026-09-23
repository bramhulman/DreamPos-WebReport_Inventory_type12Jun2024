Imports System.Threading
Imports System.Net
Imports System.Text
Imports System.IO
Imports System.Data
Imports System.Drawing

Public Class Pages_InvTrfEdit
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
        Try
            ' Safe QueryString check with Cookie fallback
            If Not String.IsNullOrEmpty(Request.QueryString("id")) Then
                Mtid = Request.QueryString("id")
            ElseIf Request.Cookies("ID") IsNot Nothing Then
                Mtid = Request.Cookies("ID").Value
            Else
                Mtid = ""
            End If

            If Not String.IsNullOrEmpty(Request.QueryString("ccf")) Then
                Integer.TryParse(Request.QueryString("ccf"), CCf)
            ElseIf Request.Cookies("CcF") IsNot Nothing Then
                Integer.TryParse(Request.Cookies("CcF").Value, CCf)
            Else
                CCf = 0
            End If

            If Request.Cookies("UserLogin_inv") IsNot Nothing Then
                sNameuser = username()
                icustid = custid()
                imodul = modul()
                iModPriv = modulepriv()
                irptpriv = rptpriv()
                irptf = rptf()

                If IsPostBack = False Then
                    ViewState("Mtid") = Mtid
                    ViewState("CCf") = CCf

                    loadFrom()
                    btnedit.InnerText = "edit"

                    If Mtid = "" Then
                        ddlFrom.SelectedValue = 0

                        Dim dt As DataTable = cInv.getDTInvTrans(Mtid, custid)
                        ViewState("InvTrans") = dt
                        BindData()
                        Bindingstk1()
                    Else
                        ddlFrom.Enabled = False
                        If CCf > 0 AndAlso ddlFrom.Items.FindByValue(CCf.ToString()) IsNot Nothing Then
                            ddlFrom.SelectedValue = CCf
                        End If
                        txtLinkid.Text = Mtid

                        Dim dt As DataTable = cInv.getDTInvTrans(Mtid, custid)
                        ViewState("InvTrans") = dt
                        BindData()
                        Bindingstk1()
                    End If
                Else
                    If ViewState("Mtid") IsNot Nothing Then Mtid = ViewState("Mtid").ToString()
                    If ViewState("CCf") IsNot Nothing Then Integer.TryParse(ViewState("CCf").ToString(), CCf)
                End If

                ScriptManager.GetCurrent(Me).RegisterPostBackControl(RadButton1)
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
            errLog(Err.Description, "Pages_InvTrfEdit:Page_Load")
            Err.Clear()
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
            errLog(Err.Description, "Pages_InvTrfEdit:loadFrom")
            Err.Clear()
        End Try


    End Sub
    'Sub loadto()
    '    If e.Row.RowType = DataControlRowType.DataRow AndAlso DgvTransfer.EditIndex = e.Row.RowIndex Then
    '        Dim ddlTo As DropDownList = DirectCast(e.Row.FindControl("DdlTo"), DropDownList)
    '        Dim lblCc As Label = DirectCast(e.Row.FindControl("lblEditCc"), Label)

    '        Dim ds As New DataSet
    '        ds = cInv.getInvcc(custid, username, modulepriv, 1)

    '        Dim dt As New DataTable
    '        dt = ds.Tables(0)
    '        dt.DefaultView.RowFilter = "id<>'" & ddlFrom.SelectedValue & "'"

    '        'WService.getIcc(cust, username, ModPrv, 1)

    '        ddlTo.DataSource = dt.DefaultView
    '        ddlTo.DataValueField = "Id"
    '        ddlTo.DataTextField = "Name"
    '        ddlTo.DataBind()
    '        ddlTo.Items.Insert(0, "")

    '        If lblCc.Text <> 0 Then
    '            ddlTo.Items.FindByValue(TryCast(e.Row.FindControl("lblEditCc"), Label).Text).Selected = True
    '        End If

    '    End If
    'End Sub


    Protected Sub BindData()
        Try
            If custid() = 0 Then
                Response.Redirect("~/Default.aspx", False)
            Else


                DgvTransfer.DataSource = DirectCast(ViewState("InvTrans"), DataTable)
                DgvTransfer.DataBind()

                'If DgvTransfer.Rows.Count > 0 Then
                '    DgvTransfer.Columns(13).Visible = True
                '    btnedit.Visible = True
                '    If GetBackground(Mtid) = 0 Then
                '        DgvTransfer.Columns(13).Visible = False
                '        btnedit.Visible = False

                '    End If
                'Else
                '    DgvTransfer.Columns(13).Visible = False
                '    btnedit.Visible = False
                'End If
            End If
        Catch ex As Exception
            errLog(Err.Description, "Pages_InvTrfEdit:BindData")
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

                dt = DirectCast(ViewState("InvTrans"), DataTable)
                'dt2 = cInv.getDtStk(dt, custid, txtSearch.Text, 0)
                dt2 = cInv.getDtStk2(dt, custid, txtSearch.Text, modulepriv, ddlFrom.SelectedValue)
                ViewState("InvStk") = dt2
                LsvStk.DataSource = DirectCast(ViewState("InvStk"), DataTable)
                LsvStk.DataBind()


            End If
        Catch ex As Exception
            errLog(Err.Description, "Pages_InvTrfEdit:Bindingstk1")
            Err.Clear()
        End Try

    End Sub
    Sub bindingstk()
        Try
            If custid() = 0 Then
                Response.Redirect("~/Default.aspx", False)
            Else
                Dim dt, dt2 As New DataTable
                dt = DirectCast(ViewState("InvTrans"), DataTable)

                Debug.Print(dt.Rows.Count.ToString)
                'dt2 = clsStk.SelectDataTable_Stk_wStock(dt, custid, txtSearch.Text, ddlFrom.SelectedValue)
                'ViewState("InvStk") = dt2
                'LsvStk.DataSource = DirectCast(ViewState("InvStk"), DataTable)
                'LsvStk.DataBind()

                dt2 = DirectCast(ViewState("InvStk"), DataTable)

                If dt2.Rows.Count = 0 Then
                    'dt2 = cInv.getDtStk(dt, custid, txtSearch.Text, 0)
                    dt2 = cInv.getDtStk2(dt, custid, txtSearch.Text, modulepriv, ddlFrom.SelectedValue)
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
            errLog(Err.Description, "Pages_InvTrfEdit:bindingstk")
            Err.Clear()
        End Try



    End Sub

    Protected Sub Page_unLoad(sender As Object, e As EventArgs) Handles Me.Unload
    End Sub

    Public Function username() As String
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        Return cookie("userName")

    End Function


    Sub delete(ByVal id As Integer)

        ' WService.DeleteInvTrans(id, cust, username)
        cInv.DeleteInvTrans(id, custid, username)

    End Sub
    Public Sub DeleteRcpDtl(ByVal id As Integer, ByVal Custid As Integer, ByVal username As String)
        'Dim pc As String = pic.ToString
        Try

            Dim str As String
            Dim c_cmd As New Cls_command

            str = "DELETE FROM cld_inv_rcpdtl where id = " & id & ""

            c_cmd.executeNonQuery(str)

            'Revisi 10 Mei
            insertLogModule(username, "InvRcpdtl", "Delete", Custid)

        Catch ex As Exception

            errLog(Err.Description, "Pages_ReceipeEdit:DeleteInvrcpdtl")
            Err.Clear()
        End Try
    End Sub



    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.ServerClick
        If txtSearch.Text = "" Then
            ShowToastr(Page, "Text Search tidak boleh kosong", "Error", "Error", False, "toast-top-right", False)
        Else
            BindCheckItem()
            bindingstk()
            CheckedItem()
        End If
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Pop", "openModalItem();", True)

    End Sub
    Protected Sub RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles DgvTransfer.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim ddlTo As DropDownList = DirectCast(e.Row.FindControl("DdlTo"), DropDownList)
            Dim lblCc As Label = DirectCast(e.Row.FindControl("lblCc"), Label)

            If ddlTo IsNot Nothing Then
                Dim ds As DataSet = cInv.getInvcc(custid, username, modulepriv, 1)
                If ds IsNot Nothing AndAlso ds.Tables.Count > 0 Then
                    Dim dt As DataTable = ds.Tables(0)
                    dt.DefaultView.RowFilter = "id<>'" & ddlFrom.SelectedValue & "'"
                    ddlTo.DataSource = dt.DefaultView
                    ddlTo.DataValueField = "Id"
                    ddlTo.DataTextField = "Name"
                    ddlTo.DataBind()
                    ddlTo.Items.Insert(0, New ListItem("-- Select Destination --", "0"))
                End If

                Dim selectedCc As String = ""
                If lblCc IsNot Nothing AndAlso Not String.IsNullOrEmpty(lblCc.Text) AndAlso lblCc.Text <> "0" Then
                    selectedCc = lblCc.Text
                Else
                    Dim drv As DataRowView = TryCast(e.Row.DataItem, DataRowView)
                    If drv IsNot Nothing AndAlso drv.Row.Table.Columns.Contains("Cc") AndAlso Not IsDBNull(drv("Cc")) Then
                        selectedCc = drv("Cc").ToString()
                    End If
                End If

                If Not String.IsNullOrEmpty(selectedCc) AndAlso selectedCc <> "0" Then
                    Dim item = ddlTo.Items.FindByValue(selectedCc)
                    If item IsNot Nothing Then
                        item.Selected = True
                    End If
                End If
            End If
        End If
    End Sub

    Sub insert()
        Dim clsprocess As New clsInventoryProcess
        Dim dt As DataTable = DirectCast(ViewState("InvTrans"), DataTable)
        Try
            Dim newLine As String = "\n"
            Dim errorMsg As String = ""

            If ddlFrom.SelectedValue = "0" OrElse ddlFrom.SelectedValue = "" Then
                notsuccesfull("Source Cost Centre is required.")
                Exit Sub
            End If

            If dt Is Nothing OrElse dt.Rows.Count = 0 Then
                notsuccesfull("Please add at least one item to transfer.")
                Exit Sub
            End If

            ' Sync values from GridView controls to DataTable
            For Each row As GridViewRow In DgvTransfer.Rows
                If row.RowType = DataControlRowType.DataRow Then
                    Dim rowID As Integer = row.RowIndex
                    If rowID < dt.Rows.Count Then
                        Dim ddlto As DropDownList = TryCast(row.FindControl("DdlTo"), DropDownList)
                        Dim txtEditUnit1 As TextBox = TryCast(row.FindControl("txtEditUnit1"), TextBox)
                        Dim txtEditUnit2 As TextBox = TryCast(row.FindControl("txtEditUnit2"), TextBox)
                        Dim lblfactor As Label = TryCast(row.FindControl("lblfactor"), Label)
                        Dim txtCost As Label = TryCast(row.FindControl("lblCost"), Label)
                        Dim lblstckqty As Label = TryCast(row.FindControl("lblStock"), Label)

                        Dim u1Val As Double = 0
                        Dim u2Val As Double = 0
                        If txtEditUnit1 IsNot Nothing Then Double.TryParse(txtEditUnit1.Text, u1Val)
                        If txtEditUnit2 IsNot Nothing Then Double.TryParse(txtEditUnit2.Text, u2Val)

                        Dim factorVal As Double = 1
                        If lblfactor IsNot Nothing Then Double.TryParse(lblfactor.Text, factorVal)
                        If factorVal <= 0 Then factorVal = 1

                        Dim total As Double = (u1Val * factorVal) + u2Val
                        Dim initialTrf As Double = 0
                        If dt.Rows(rowID)("ttlStock") IsNot DBNull.Value Then
                            Double.TryParse(dt.Rows(rowID)("ttlStock").ToString(), initialTrf)
                        End If

                        Dim currentStock As Double = 0
                        If lblstckqty IsNot Nothing Then Double.TryParse(lblstckqty.Text, currentStock)

                        Dim itemName As String = dt.Rows(rowID)("Stk_Name").ToString()

                        If ddlto Is Nothing OrElse ddlto.SelectedValue = "0" OrElse ddlto.SelectedValue = "" Then
                            errorMsg &= "Please select Destination Cost Centre for: " & itemName & newLine
                        ElseIf ddlto.SelectedValue = ddlFrom.SelectedValue Then
                            errorMsg &= "Source and Destination Cost Centre cannot be the same for: " & itemName & newLine
                        End If

                        If total <= 0 Then
                            errorMsg &= "Transfer quantity must be greater than 0 for: " & itemName & newLine
                        ElseIf (total - initialTrf) > currentStock Then
                            errorMsg &= "Transfer quantity (" & total & ") exceeds available stock (" & currentStock & ") for: " & itemName & newLine
                        End If

                        If ddlto IsNot Nothing AndAlso ddlto.SelectedValue <> "0" Then
                            dt.Rows(rowID)("Cc") = CInt(ddlto.SelectedValue)
                            dt.Rows(rowID)("Cc_name") = ddlto.SelectedItem.Text
                        End If
                        dt.Rows(rowID)("U1") = u1Val
                        dt.Rows(rowID)("U2") = u2Val
                        dt.Rows(rowID)("ttlStock") = total
                        If txtCost IsNot Nothing Then dt.Rows(rowID)("Cost") = txtCost.Text
                    End If
                End If
            Next

            If Not String.IsNullOrEmpty(errorMsg) Then
                notsuccesfull(errorMsg)
                Exit Sub
            End If

            dt.AcceptChanges()
            ViewState("InvTrans") = dt

            ' Reversal if updating existing transfer
            If Mtid <> "" And CCf <> 0 Then
                Dim cinvtrans As New clsInventory_Transfer
                Dim dt1 As DataTable = cinvtrans.SelectDataTable_invtrans_Transfer(Mtid, custid)
                If dt1.Rows.Count > 0 Then
                    Dim clsicc As New Cls_ICc
                    For i As Integer = 0 To dt1.Rows.Count - 1
                        clsprocess.InvstkId = dt1.Rows(i).Item("stk_id").ToString
                        clsprocess.InvCc = dt1.Rows(i).Item("cc").ToString
                        clsprocess.InvCcName = dt1.Rows(i).Item("cc_name").ToString
                        clsprocess.InvCustId = custid()
                        clsprocess.InvstkName = dt1.Rows(i).Item("Stk_Name").ToString
                        clsprocess.t_in = -(dt1.Rows(i).Item("ttlStock").ToString)
                        clsprocess.t_out = 0
                        clsprocess.InvStkNo = cInv.getStkByid(dt1.Rows(i).Item("stk_id").ToString, custid).Tables(0).Rows(0).Item("Number").ToString
                        clsprocess.InvDate = Date.Now.Date.ToString("yyyy-MM-dd 00:00:00")
                        clsprocess.BCost = True
                        clsprocess.executeNonQuery_Inv_ProcessCls()

                        clsprocess.InvstkId = dt1.Rows(i).Item("stk_id").ToString
                        clsprocess.InvCc = dt1.Rows(i).Item("ccfrom").ToString
                        clsprocess.InvCcName = clsicc.SelectDataIcc(dt1.Rows(i).Item("ccfrom").ToString).Tables(0).Rows(0).Item("name")
                        clsprocess.InvCustId = custid()
                        clsprocess.InvstkName = dt1.Rows(i).Item("Stk_Name").ToString
                        clsprocess.t_in = 0
                        clsprocess.t_out = -(dt1.Rows(i).Item("ttlStock").ToString)
                        clsprocess.InvStkNo = cInv.getStkByid(dt1.Rows(i).Item("stk_id").ToString, custid).Tables(0).Rows(0).Item("Number").ToString
                        clsprocess.InvDate = Date.Now.Date.ToString("yyyy-MM-dd 00:00:00")
                        clsprocess.executeNonQuery_Inv_ProcessCls()
                    Next i
                End If
            End If

            ' Insert/update atomically
            Dim savedId As String = cInv.insertInvtrf(txtLinkid.Text, ddlFrom.SelectedValue, dt, username, custid)
            If Not String.IsNullOrEmpty(savedId) Then
                txtLinkid.Text = savedId
            End If

            ' Apply stock movements
            For i As Integer = 0 To dt.Rows.Count - 1
                ' Destination CC (transfer in)
                clsprocess.InvstkId = dt.Rows(i).Item("stk_id").ToString
                clsprocess.InvCc = dt.Rows(i).Item("cc").ToString
                clsprocess.InvCcName = dt.Rows(i).Item("cc_name").ToString
                clsprocess.InvCustId = custid()
                clsprocess.InvstkName = dt.Rows(i).Item("Stk_Name").ToString
                clsprocess.t_in = dt.Rows(i).Item("ttlStock").ToString
                clsprocess.t_out = 0
                clsprocess.InvStkNo = cInv.getStkByid(dt.Rows(i).Item("stk_id").ToString, custid).Tables(0).Rows(0).Item("Number").ToString
                clsprocess.InvDate = Date.Now.Date.ToString("yyyy-MM-dd 00:00:00")
                clsprocess.BCost = True
                clsprocess.executeNonQuery_Inv_ProcessCls()

                ' Source CC (transfer out)
                clsprocess.InvstkId = dt.Rows(i).Item("stk_id").ToString
                clsprocess.InvCc = ddlFrom.SelectedValue
                clsprocess.InvCcName = ddlFrom.SelectedItem.Text
                clsprocess.InvCustId = custid()
                clsprocess.InvstkName = dt.Rows(i).Item("Stk_Name").ToString
                clsprocess.t_out = dt.Rows(i).Item("ttlStock").ToString
                clsprocess.t_in = 0
                clsprocess.InvStkNo = cInv.getStkByid(dt.Rows(i).Item("stk_id").ToString, custid).Tables(0).Rows(0).Item("Number").ToString
                clsprocess.InvDate = Date.Now.Date.ToString("yyyy-MM-dd 00:00:00")
                clsprocess.executeNonQuery_Inv_ProcessCls()
            Next i

            Response.Redirect("~/Pages_InvTransfer.aspx", False)

        Catch ex As Exception
            errLog(ex.Message & " - " & ex.StackTrace, "Pages_InvTrfEdit:insert")
            Err.Clear()
        End Try
    End Sub
    Sub notsuccesfull(ByVal msg As String)
        Dim message As String = msg
        Dim sb As New System.Text.StringBuilder()
        ' Dim url As String = "~/Pages_InvTransfer.aspx"
        sb.Append("<script type='text/javascript'>")
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
            dtstk.AcceptChanges
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
        Dim txtCost As Label = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("txtEditcost"), Label)
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
        'Dim total As Decimal = Val(txtUnit1.Text) * Val(lblLosF1.Text) + Val(txtUnit2.Text)
        Dim total As Double = Val(txtUnit1.Text) * Val(lblLosF1.Text) + Val(txtUnit2.Text)
        Dim lblUnit1 As Label = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("lblEditEmpID"), Label)

        Dim dt As DataTable = DirectCast(ViewState("InvTrans"), DataTable)
        dt.Rows(e.RowIndex)("Cc") = ddlTo.SelectedValue
        dt.Rows(e.RowIndex)("Cc_name") = ddlTo.SelectedItem.Text
        dt.Rows(e.RowIndex)("U1") = txtUnit1.Text
        dt.Rows(e.RowIndex)("U2") = txtUnit2.Text
        dt.Rows(e.RowIndex)("ttlStock") = total
        dt.Rows(e.RowIndex)("Cost") = txtCost.Text
        dt.AcceptChanges()
        BindData()

        If total - txtTotaltrfawal.Text > lblstckqty.Text Then
            txtUnit1.Text = 0
            txtUnit2.Text = 0
            dt.Rows(e.RowIndex)("U1") = txtUnit1.Text
            dt.Rows(e.RowIndex)("U2") = txtUnit2.Text
            total = Val(txtUnit1.Text) * Val(lblLosF1.Text) + Val(txtUnit2.Text)
            dt.Rows(e.RowIndex)("ttlStock") = total
            dt.AcceptChanges()
            BindData()
            notsuccesfull("Total Transfer > Stock")
            Exit Sub
        ElseIf ddlFrom.SelectedValue = ddlTo.SelectedValue Then
            notsuccesfull("Cost Centre From and To Cannot Same")
            Exit Sub
        Else
            DgvTransfer.EditIndex = -1
            BindData()
        End If

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
        Public Property ItemName As String = ""
        Public Property Status As Boolean = False
    End Class
    'Start : Store
    Private Sub BindCheckItem()
        Try

            If CBool(IIf(IsNothing(ViewState("ListItem")), True, False)) Then


                Dim ListItem2 As List(Of Item_Detail) = New List(Of Item_Detail)()

                Dim dtorderw, dtitem As New DataTable
                dtorderw = DirectCast(ViewState("InvTrans"), DataTable)
                'dtitem = cInv.getDtStk(dtorderw, custid, txtSearch.Text, 0)
                dtitem = cInv.getDtStk(dtorderw, custid, "", 0)
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
            If ddlFrom.SelectedValue <> 0 Then
                Add2()
                Session("xfrom") = ddlFrom.SelectedValue
            Else
                notsuccesfull("you must choose Cost Centre first")
            End If
        Catch ex As Exception

        End Try


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
            Dim i As Integer = 0
            If DirectCast(ViewState("InvTrans"), DataTable).Rows.Count >= My.Settings.limitorder Then
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
                        Dim cost As Integer
                        Dim dsA As DataSet = cInv.getStkcostByid((DirectCast(item.FindControl("lblID"), Label).Text), ddlFrom.SelectedValue, custid, CDate(Now()))

                        If dsA.Tables(0).Rows.Count > 0 Then
                            cost = dsA.Tables(0).Rows(0).Item("cost")
                        Else
                            cost = 0
                        End If

                        Dim ds As DataSet = cInv.getStkByid2((DirectCast(item.FindControl("lblID"), Label).Text), 0, custid, ddlFrom.SelectedValue, CDate(Now()))

                        If ds.Tables(0).Rows.Count > 0 Then
                            Dim u1 As String = ds.Tables(0).Rows(0).Item("Unit1").ToString
                            Dim u2 As String = ds.Tables(0).Rows(0).Item("Unit2").ToString
                            Dim dt As DataTable = DirectCast(ViewState("InvTrans"), DataTable)
                            dt.Rows.Add(0, custid, Date.Now.ToString("yyyy-MM-dd"), 1, 0, (DirectCast(item.FindControl("lblID"), Label).Text), (DirectCast(item.FindControl("lblName"), Label).Text), 0, 0, ds.Tables(0).Rows(0).Item("Factor1"), 0, 0, cost, "", 1, ds.Tables(0).Rows(0).Item("Unit1").ToString, ds.Tables(0).Rows(0).Item("Unit2").ToString, username, Date.Now.ToString("yyyy-MM-dd"), "", Date.Now.ToString("yyyy-MM-dd"), ds.Tables(0).Rows(0).Item("Number").ToString, "", ds.Tables(0).Rows(0).Item("Stock").ToString)
                            ViewState("InvTrans") = dt
                            Dim dtStk As DataTable = DirectCast(ViewState("InvStk"), DataTable)
                            dtStk.Rows(i).Delete()
                            dtStk.AcceptChanges()
                            ViewState("InvStk") = dtStk
                            i = i - 1
                        End If

                        BindData()
                        txtSearch.Text = ""
                        bindingstk()
                    Catch error_t As Exception
                        MsgBox(error_t.ToString)
                    End Try

                End If
                i = i + 1
            Next
ef:
        Catch ex As Exception
            'MsgBox(ex.Message)
        End Try

    End Sub

    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
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

        Response.Redirect("~/Pages_InvTransfer.aspx", False)
    End Sub

    Protected Sub ddlFrom_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlFrom.SelectedIndexChanged
        Dim dt As DataTable = DirectCast(ViewState("InvTrans"), DataTable)
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
                    Dim total As Double = row.Item("u1") * row.Item("factor1") + row.Item("u2")
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
            Bindingstk1()
        Catch ex As Exception

        End Try


    End Sub
    Protected Sub imgbtnDelete_Click(sender As Object, e As EventArgs)
        Dim ctrl As Control = CType(sender, Control)
        Dim gvRow As GridViewRow = CType(ctrl.NamingContainer, GridViewRow)
        Dim rowID As Integer = gvRow.RowIndex
        Dim c_cmd As New clsDbo("")
        Dim lid As String = ""
        Try
            If ViewState("InvTrans") IsNot Nothing Then


                Dim lblID As Label = DirectCast(DgvTransfer.Rows(rowID).FindControl("lblID"), Label)
                Dim lblstknm As Label = DirectCast(DgvTransfer.Rows(rowID).FindControl("lblName"), Label)
                Dim lblstkid As Label = DirectCast(DgvTransfer.Rows(rowID).FindControl("lblStkID"), Label)

                Dim dt As DataTable = CType(ViewState("InvTrans"), DataTable)
                Dim dtstk As DataTable = DirectCast(ViewState("InvStk"), DataTable)
                ' If dt.Rows.Count > 1 Then
                Dim confirmValue As String = Request.Form("confirm_value")
                If confirmValue = "Yes" Then
                    If gvRow.RowIndex <= dt.Rows.Count - 1 Then
                        lid= dt.Rows(rowID).Item("ID")
                        'clsInv.DeleteInvWare(dt.Rows(rowID).Item("wid"), custid, ddlFrom.SelectedValue, dt.Rows(rowID).Item("stk_ID"), username)
                        delete(lid)
                        dt.Rows.Remove(dt.Rows(rowID))
                        dt.AcceptChanges()
                        ResetRowID(dt)
                        dtstk.Rows.Add(lblstkid.Text, lblstknm.Text)
                        ViewState("InvStk") = dtstk
                    End If
                Else
                    Exit Sub
                End If

                'End If

                ViewState("InvTrans") = dt

                BindData()
                bindingstk()

                'Dim totalcost As Double = 0

                'For Each dr As DataRow In dt.Rows
                '    totalcost += Convert.ToDouble(dr("totalcost"))
                'Next

                'DgvTransfer.Columns(11).FooterText = totalcost.ToString("#,##0.00")

                'DgvTransfer.DataSource = dt
                'DgvTransfer.DataBind()
                'DgvTransfer.ShowFooter = True
                'If dt.Rows.Count > 0 Then
                '    xdoctot = dt.Compute("sum(totalcost)", String.Empty)
                'Else
                '    xdoctot = 0
                'End If

                'strmaster = "update cld_inv_w set  doctot = " & xdoctot & " where id = '" & lid & "' and custid = " & custid() & ""
                '" & dt.Rows(rowID).Item("linkid") & "'
                'c_cmd.executeNonQuery(strmaster)
            End If
        Catch ex As Exception
            errLog(Err.Description, "Pages_InvOrderWHODetail:imgbtnDelete_Click")
            Err.Clear()
        End Try


    End Sub
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
            errLog(Err.Description, "Pages_InvOrderWHODetail:ResetRowID")
            Err.Clear()
        End Try
    End Sub
    Protected Sub update()
        Dim msg As String = ""
        Dim newLine As String = "\n"
        Try
            For Each row As GridViewRow In DgvTransfer.Rows
                Dim rowID As Integer = row.RowIndex

                If row.RowType = DataControlRowType.DataRow Then

                    If row.RowType = DataControlRowType.DataRow Then
                        Dim lblTo As Label = (TryCast(row.FindControl("lblTo"), Label))
                        'Dim Label1 As Label = (TryCast(row.FindControl("Label1"), Label))
                        Dim ddlto As DropDownList = (TryCast(row.FindControl("ddlto"), DropDownList))
                        'Dim txtEditUnit2 As TextBox = (TryCast(row.FindControl("txtEditUnit2"), TextBox))
                        Dim lblUnit1 As Label = (TryCast(row.FindControl("lblUnit1"), Label))
                        Dim txtEditUnit1 As TextBox = (TryCast(row.FindControl("txtEditUnit1"), TextBox))
                        Dim lblUnit2 As Label = (TryCast(row.FindControl("lblUnit2"), Label))
                        Dim txtEditUnit2 As TextBox = (TryCast(row.FindControl("txtEditUnit2"), TextBox))
                        Dim txtCost As Label = (TryCast(row.FindControl("lblCost"), Label))
                        Dim txtTotaltrfawal As Label = (TryCast(row.FindControl("lblttlStock"), Label))
                        Dim lblstckqty As Label = (TryCast(row.FindControl("lblStock"), Label))

                        'Dim txtEditU1Cost As TextBox = (TryCast(row.FindControl("txtEditU1Cost"), TextBox))
                        lblTo.Visible = True
                        ddlto.Visible = False
                        lblUnit1.Visible = True
                        txtEditUnit1.Visible = False
                        lblUnit2.Visible = True
                        txtEditUnit2.Visible = False
                        'Label1.Visible = True
                        'txtEditUnit2.Visible = False
                        'lblU1Cost.Visible = True
                        'txtEditU1Cost.Visible = False
                        lblUnit1.Text = txtEditUnit1.Text
                        lblUnit2.Text = txtEditUnit2.Text
                        lblTo.Text = ddlto.SelectedItem.Text
                        Dim lblfactor As Label = (TryCast(row.FindControl("lblfactor"), Label))
                        Dim total As Double = Val(txtEditUnit1.Text) * Val(lblfactor.Text) + Val(txtEditUnit2.Text)

                        Dim dt As DataTable = DirectCast(ViewState("InvTrans"), DataTable)
                        dt.Rows(rowID)("Cc") = CInt(ddlto.SelectedValue)
                        dt.Rows(rowID)("Cc_name") = ddlto.SelectedItem.Text
                        dt.Rows(rowID)("U1") = txtEditUnit1.Text
                        dt.Rows(rowID)("U2") = txtEditUnit2.Text
                        dt.Rows(rowID)("ttlStock") = total
                        dt.Rows(rowID)("Cost") = txtCost.Text
                        dt.AcceptChanges()
                        BindData()

                        If total - txtTotaltrfawal.Text > lblstckqty.Text Then
                            txtEditUnit1.Text = 0
                            txtEditUnit2.Text = 0
                            dt.Rows(rowID)("U1") = txtEditUnit1.Text
                            dt.Rows(rowID)("U2") = txtEditUnit2.Text
                            total = Val(txtEditUnit1.Text) * Val(lblfactor.Text) + Val(txtEditUnit2.Text)
                            dt.Rows(rowID)("ttlStock") = total
                            dt.AcceptChanges()
                            BindData()
                            notsuccesfull("Total Transfer > Stock")
                            Exit Sub
                        ElseIf ddlFrom.SelectedValue = ddlto.SelectedValue Then
                            notsuccesfull("Cost Centre From and To Cannot Same")
                            Exit Sub
                        Else
                            DgvTransfer.EditIndex = -1
                            BindData()
                        End If



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
        Catch ex As Exception
            errLog(Err.Description, "Pages_InvOrderWHODetail:update")
            Err.Clear()
        End Try
    End Sub
    Protected Sub btnedit_ServerClick(sender As Object, e As EventArgs) Handles btnedit.ServerClick
        Try
            update()
        Catch ex As Exception
            errLog(Err.Description, "Pages_InvTrfEdit:btnedit_ServerClick")
        End Try
    End Sub
    Protected Function GetBackground(ByVal trfid As String)

        Dim imgstat As Integer = 0
        Dim cinvtrf As New clsInventory_Transfer
        Try
            imgstat = cinvtrf.trf_date(trfid)

        Catch ex As Exception
            errLog(Err.Description, "Pages_InvTrfEdit:GetBackground")
            Err.Clear()
        End Try


        Return imgstat

    End Function
End Class
