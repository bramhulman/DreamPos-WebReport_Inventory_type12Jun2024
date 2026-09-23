Imports System.Threading
Imports System.Net
Imports System.Text
Imports System.IO
Imports System.Data
Imports System.Drawing
Imports System.Reflection

Public Class PurchaseRequestDetail
    Inherits System.Web.UI.Page

    Private sNameuser As String
    Private Userid As Integer
    Private icustid As Integer
    Private imodul As Integer
    Private iModPriv As Integer
    Private irptpriv As Integer
    Private irptf As Integer
    Private mfunction As New ModFunction

    Public Function IsValidStkId(stkIdObj As Object) As Boolean
        If stkIdObj Is Nothing OrElse IsDBNull(stkIdObj) Then Return False
        Dim id As Integer = 0
        If Integer.TryParse(stkIdObj.ToString(), id) Then
            Return id > 0
        End If
        Return False
    End Function

    Public Function GetStockColor(stockccObj As Object) As System.Drawing.Color
        If stockccObj Is Nothing OrElse IsDBNull(stockccObj) Then
            Return System.Drawing.ColorTranslator.FromHtml("#28a745")
        End If
        Dim val As Double = 0
        If Double.TryParse(stockccObj.ToString(), val) Then
            If val < 0 Then
                Return System.Drawing.ColorTranslator.FromHtml("#dc3545")
            End If
        End If
        Return System.Drawing.ColorTranslator.FromHtml("#28a745")
    End Function

    ' === PR fields ÃƒÂ¢Ã¢â€šÂ¬Ã¢â‚¬Â dibaca dari cookies yang di-set oleh PurchaseRequest.aspx ===
    ' Edit mode  : Edt command set pr_id, pr_no, pr_date, required_date, Cc, request_by, stat
    ' New mode   : btnNew_Click set pr_no="" (kosong), pr_id="0"
    Private xpr_id As Integer       ' purchase request id
    Private xpr_no As String        ' pr number ÃƒÂ¢Ã¢â€šÂ¬Ã¢â‚¬Â kosong = new mode
    Private xpr_date As Date        ' pr_date
    Private xrequired_date As Date  ' required_date
    Private xCc As Integer          ' cost center
    Private xrequest_by As String   ' request_by
    Private xstat As String         ' status

    Public xware As Integer
    Public xfrom As Integer
    Public countlvstore As Integer
    Dim alert1 As Integer
    Dim alert2 As Integer

    Private clsInv As New clsInventory
    Private clsStk As New clsInventory_stk
' ============================================================
    ' Cookie helpers

    ' ============================================================
    Public Function modulepriv() As Integer
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        If cookie Is Nothing Then Return -1
        Return cookie("modulepriv")
    End Function
    Public Function custid() As Integer
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        If cookie Is Nothing Then Return -1
        Return cookie("Custid")
    End Function
    Public Function rptpriv() As Integer
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        If cookie Is Nothing Then Return -1
        Return cookie("rptpriv")
    End Function
    Public Function modul() As Integer
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        If cookie Is Nothing Then Return -1
        Return cookie("modul")
    End Function
    Public Function rptf() As Integer
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        If cookie Is Nothing Then Return -1
        Return cookie("rptf")
    End Function
    Public Function username() As String
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        Return cookie("userName")
    End Function

    ''' <summary>Baca cookie secara aman ÃƒÂ¢Ã¢â€šÂ¬Ã¢â‚¬Â return "" jika tidak ada</summary>
    Private Function getCookie(ByVal name As String) As String
        Dim c As HttpCookie = Request.Cookies(name)
        If c Is Nothing Then Return ""
        Return c.Value
    End Function

    Private Property IsEditingMode() As Boolean
        Get
            If ViewState("IsEditingMode") Is Nothing Then
                Return False
            End If
            Return CBool(ViewState("IsEditingMode"))
        End Get
        Set(value As Boolean)
            ViewState("IsEditingMode") = value
        End Set
    End Property

    Private Property EditingRowIndex() As Integer
        Get
            If ViewState("EditingRowIndex") Is Nothing Then
                Return -1
            End If
            Return CInt(ViewState("EditingRowIndex"))
        End Get
        Set(value As Integer)
            ViewState("EditingRowIndex") = value
        End Set
    End Property

    ' ============================================================
    ' Page_Load

    ' ============================================================
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("username") Is Nothing Or custid() < 1 Then
            Session.Clear()
            Session.Abandon()
            Session.RemoveAll()
            FormsAuthentication.RedirectToLoginPage()
            Exit Sub
        End If

        Try
            ' === Baca PR cookies (set dari PurchaseRequest.aspx) ===
            Dim sPrId As String = getCookie("pr_id")
            Dim sPrNo As String = getCookie("pr_no")
            Dim sPrDate As String = getCookie("pr_date")
            Dim sRequiredDate As String = getCookie("required_date")
            Dim sCc As String = getCookie("Cc")
            Dim sReqBy As String = getCookie("request_by")
            Dim sStat As String = getCookie("stat")

            xpr_no = sPrNo
            xpr_id = If(IsNumeric(sPrId), CInt(sPrId), 0)
            xCc = If(IsNumeric(sCc), CInt(sCc), 0)
            xrequest_by = sReqBy
            xstat = sStat
            If IsDate(sPrDate) Then xpr_date = CDate(sPrDate)
            If IsDate(sRequiredDate) Then xrequired_date = CDate(sRequiredDate)
            If Request.Cookies("UserLogin_inv") IsNot Nothing Then
                sNameuser = username()
                icustid = custid()
                imodul = modul()
                iModPriv = modulepriv()
                irptpriv = rptpriv()
                irptf = rptf()

                If IsPostBack = False Then
                    loadFrom()
                    'loadwarehouse()
                    IsEditingMode = False
                    ToggleEditButton(False)
                    txtcreatedate.Text = Format(Now, "dd MMM yyyy")

                    ' === Deteksi: New atau Edit berdasarkan xpr_no ===
                    ' New:  xpr_no = "" (di-clear oleh btnNew_Click)
                    ' Edit: xpr_no <> "" (di-set oleh Edt command)

                    If xpr_no = "" Then
                        ' =========================================
                        ' NEW MODE
                        ' =========================================
                        txtID.Attributes.Add("placeholder", "Auto Generated (New)")
                        txtID.Text = ""
                        txtID.ReadOnly = True
                        lblStatusBadge.Text = "NEW"
                        lblStatusBadge.CssClass = "prd-badge-new"

                        Dim dt As New DataTable
                        dt = clsInv.getDTInvPR("", 0)
                        ensurePRWorkingColumns(dt)
                        ViewState("orderW") = dt

                        BindData(modulepriv())
                        Bindingstk1()
                        Createdate.Visible = True
                        Label4.Text = "Request Date"

                    Else
                        ' =========================================
                        ' EDIT MODE
                        ' =========================================
                        txtID.Text = xpr_no
                        txtID.ReadOnly = True

                        lblStatusBadge.Text = "EDIT - " & xpr_no
                        lblStatusBadge.CssClass = "prd-badge-edit"

                        ' Cek editable
                        If Not clsInv.isPREditable(txtID.Text) Then
                            notsuccesfull("Purchase Request cannot be edited because it is not in Draft/Request state.")
                        End If

                        ' Set tanggal
                        If IsDate(xpr_date) Then
                            txtcreatedate.Text = Format(xpr_date, "dd MMM yyyy")
                        End If
                        If IsDate(xrequired_date) Then
                            txtRequiredDate.Text = Format(xrequired_date, "dd MMM yyyy")
                        End If
                        requireddate.Visible = True
                        Createdate.Visible = True
                        txtcreatedate.Enabled = False
                        Label4.Text = "Request Date"

                        ' Set Cost Centre
                        If xCc > 0 AndAlso ddlFrom.Items.FindByValue(xCc.ToString) IsNot Nothing Then
                            ddlFrom.SelectedValue = xCc.ToString
                        End If
                        ddlFrom.Enabled = False
                        'ddlWare.Enabled = False

                        ' Load items dari cld_inv_prd
                        Dim dt As New DataTable
                        dt = clsInv.getDTInvPR(xpr_no, xCc)
                        ensurePRWorkingColumns(dt)
                        ViewState("orderW") = dt
                        BindData(modulepriv())
                        Bindingstk1()

                        ' Load Remarks dari cld_inv_pr
                        Dim dbPR As New clsDbo("SELECT remarks FROM cld_inv_pr WHERE pr_no='" & xpr_no & "'")
                        Dim dsPR As DataSet = dbPR.selectDataset
                        dbPR.dispose()
                        If dsPR.Tables(0).Rows.Count > 0 Then
                            txtNotes.Text = dsPR.Tables(0).Rows(0)("remarks").ToString()
                        End If

                        ' Check if Store user is viewing a processed PR
                        Dim origin As String = getCookie("origin")
                        If origin <> "ho" AndAlso xstat <> "REQUEST" Then
                            ApplyStoreReadOnlyMode()
                        End If

                    End If

                    alert2 = 0
                    alert1 = 0
                    notsuccesfull("Purchase Request max " & My.Settings.limitorder & " items per document, please split if more.")

                End If

                Dim script As String = "$(document).ready(function () { $('[id*=btnShow]').click(); });"
                ClientScript.RegisterStartupScript(Me.GetType, "load", script, True)

                ScriptManager.GetCurrent(Me).RegisterPostBackControl(RadButton1)

                txtSearch.Attributes.Add("onfocus", "javascript: this.setSelectionRange(this.value.length,this.value.length);")
                txtNotes.Attributes.Add("maxlength", txtNotes.MaxLength.ToString)

            Else
                Dim script As String = "window.onload = function(){ alert('Please Login'); window.location = '../Default'; }"
                ClientScript.RegisterStartupScript(Me.GetType(), "Redirect", script, True)
            End If

        Catch ex As Exception
            errLog(Err.Description & " | " & ex.StackTrace, "PurchaseRequestDetail:Page_Load")
            Err.Clear()
        End Try
    End Sub

    Protected Sub DgvTransfer_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim origin As String = getCookie("origin")
            Dim imgbtnDelete As ImageButton = TryCast(e.Row.FindControl("imgbtnDelete"), ImageButton)
            Dim imgbtnReject As ImageButton = TryCast(e.Row.FindControl("imgbtnReject"), ImageButton)

            Dim rowView As DataRowView = TryCast(e.Row.DataItem, DataRowView)
            If rowView IsNot Nothing Then
                Dim stkId As Integer = If(IsDBNull(rowView("stk_id")), 0, CInt(rowView("stk_id")))

                ' Controls
                Dim lblName As Label = TryCast(e.Row.FindControl("lblName"), Label)
                Dim ddlEditItem As DropDownList = TryCast(e.Row.FindControl("ddlEditItem"), DropDownList)

                Dim lblEditUnit1 As Label = TryCast(e.Row.FindControl("lblEditUnit1"), Label)
                Dim txtEditUnit1 As TextBox = TryCast(e.Row.FindControl("txtEditUnit1"), TextBox)

                Dim lblEditUnit2 As Label = TryCast(e.Row.FindControl("lblEditUnit2"), Label)
                Dim txtEditUnit2 As TextBox = TryCast(e.Row.FindControl("txtEditUnit2"), TextBox)

                Dim lblRemarks As Label = TryCast(e.Row.FindControl("lblRemarks"), Label)
                Dim txtEditRemarks As TextBox = TryCast(e.Row.FindControl("txtEditRemarks"), TextBox)

                Dim lblAppQty As Label = TryCast(e.Row.FindControl("lblAppQty"), Label)
                Dim txtEditAppQty As TextBox = TryCast(e.Row.FindControl("txtEditAppQty"), TextBox)

                Dim chkApprove As CheckBox = TryCast(e.Row.FindControl("chkApprove"), CheckBox)
                Dim ddlSupplier As DropDownList = TryCast(e.Row.FindControl("ddlSupplier"), DropDownList)
                Dim lblSupplier As Label = TryCast(e.Row.FindControl("lblSupplier"), Label)

                ' If stkId = 0, this is a newly added row that hasn't selected an item yet.
                ' It should always show the dropdown list and inputs, regardless of IsEditingMode!
                If stkId = 0 Then
                    If lblName IsNot Nothing Then lblName.Visible = False
                    If ddlEditItem IsNot Nothing Then
                        ddlEditItem.Visible = True
                        ' Populate ddlEditItem
                        Dim cId As Integer = custid()
                        Dim ccVal As String = ddlFrom.SelectedValue
                        Dim dtItems As DataTable = clsStk.SelectDt_Stk_all2(DirectCast(ViewState("orderW"), DataTable), cId, "", ccVal)
                        Dim rowCount As Integer = If(dtItems IsNot Nothing, dtItems.Rows.Count, -1)
                        errLog("DgvTransfer_RowDataBound Debug: stkId=0, custid=" & cId & ", cc=" & ccVal & ", rowCount=" & rowCount, "PurchaseRequestDetail:RowDataBound")
                        ddlEditItem.DataSource = dtItems
                        ddlEditItem.DataTextField = "name"
                        ddlEditItem.DataValueField = "id"
                        ddlEditItem.DataBind()
                        ddlEditItem.Items.Insert(0, New ListItem("--- Select Item ---", "0"))
                    End If

                    If lblEditUnit1 IsNot Nothing Then lblEditUnit1.Visible = False
                    If txtEditUnit1 IsNot Nothing Then
                        txtEditUnit1.Visible = True
                        txtEditUnit1.Text = "0"
                    End If

                    If lblEditUnit2 IsNot Nothing Then lblEditUnit2.Visible = False
                    If txtEditUnit2 IsNot Nothing Then
                        txtEditUnit2.Visible = True
                        txtEditUnit2.Text = "0"
                    End If

                    If lblRemarks IsNot Nothing Then lblRemarks.Visible = False
                    If txtEditRemarks IsNot Nothing Then
                        txtEditRemarks.Visible = True
                        txtEditRemarks.Text = ""
                    End If

                    ' Hide other columns/controls that don't apply to a blank new row
                    If lblAppQty IsNot Nothing Then lblAppQty.Visible = False
                    If txtEditAppQty IsNot Nothing Then txtEditAppQty.Visible = False
                    If chkApprove IsNot Nothing Then chkApprove.Visible = False
                    If ddlSupplier IsNot Nothing Then ddlSupplier.Visible = False
                    If lblSupplier IsNot Nothing Then lblSupplier.Visible = False

                    If imgbtnDelete IsNot Nothing Then imgbtnDelete.Visible = (origin = "store")
                    If imgbtnReject IsNot Nothing Then imgbtnReject.Visible = False
                Else
                    ' stkId > 0 (existing row or newly selected row)
                    If lblName IsNot Nothing Then lblName.Visible = True
                    If ddlEditItem IsNot Nothing Then ddlEditItem.Visible = False

                    ' Standard Edit/View Toggle based on isThisRowEditable
                    Dim isThisRowEditable As Boolean = False
                    If EditingRowIndex >= 0 Then
                        isThisRowEditable = (e.Row.RowIndex = EditingRowIndex)
                    Else
                        isThisRowEditable = IsEditingMode
                    End If

                    If origin <> "ho" Then
                        If txtEditUnit1 IsNot Nothing Then txtEditUnit1.Visible = isThisRowEditable
                        If lblEditUnit1 IsNot Nothing Then lblEditUnit1.Visible = Not isThisRowEditable

                        If txtEditUnit2 IsNot Nothing Then txtEditUnit2.Visible = isThisRowEditable
                        If lblEditUnit2 IsNot Nothing Then lblEditUnit2.Visible = Not isThisRowEditable
                    Else
                        ' HO mode
                        If txtEditUnit1 IsNot Nothing Then txtEditUnit1.Visible = False
                        If lblEditUnit1 IsNot Nothing Then lblEditUnit1.Visible = True

                        If txtEditUnit2 IsNot Nothing Then txtEditUnit2.Visible = False
                        If lblEditUnit2 IsNot Nothing Then lblEditUnit2.Visible = True
                    End If

                    If txtEditRemarks IsNot Nothing Then txtEditRemarks.Visible = isThisRowEditable
                    If lblRemarks IsNot Nothing Then lblRemarks.Visible = Not isThisRowEditable

                    ' HO Mode Approval & Supplier Edit
                    If origin = "ho" Then
                        If txtEditAppQty IsNot Nothing Then txtEditAppQty.Visible = IsEditingMode
                        If lblAppQty IsNot Nothing Then lblAppQty.Visible = Not IsEditingMode

                        If chkApprove IsNot Nothing Then
                            chkApprove.Visible = True
                            chkApprove.Enabled = IsEditingMode

                            Dim stat As String = rowView("stat").ToString().ToUpper()
                            chkApprove.Checked = (stat = "APPROVED" Or stat = "APPROVE" Or stat = "CLOSED")

                            If txtEditAppQty IsNot Nothing Then
                                txtEditAppQty.Enabled = chkApprove.Checked
                            End If
                        End If

                        If lblSupplier IsNot Nothing Then lblSupplier.Visible = False
                        If ddlSupplier IsNot Nothing Then
                            ddlSupplier.Visible = True
                            ddlSupplier.Enabled = IsEditingMode
                            Dim dtSup As DataTable = getSupplierDT()
                            ddlSupplier.DataSource = dtSup
                            ddlSupplier.DataTextField = "Name"
                            ddlSupplier.DataValueField = "Id"
                            ddlSupplier.DataBind()
                            ddlSupplier.Items.Insert(0, New ListItem("belum pilih", "0"))
                            If rowView.DataView.Table.Columns.Contains("supplier_id") AndAlso Not IsDBNull(rowView("supplier_id")) Then
                                Try
                                    ddlSupplier.SelectedValue = rowView("supplier_id").ToString()
                                Catch ex As Exception
                                End Try
                            End If
                        End If
                        If imgbtnDelete IsNot Nothing Then imgbtnDelete.Visible = False
                        If imgbtnReject IsNot Nothing Then imgbtnReject.Visible = True
                    Else
                        ' Store Mode
                        If txtEditAppQty IsNot Nothing Then txtEditAppQty.Visible = False
                        If lblAppQty IsNot Nothing Then lblAppQty.Visible = True

                        If chkApprove IsNot Nothing Then
                            chkApprove.Visible = True
                            chkApprove.Enabled = False
                            Dim stat As String = rowView("stat").ToString().ToUpper()
                            chkApprove.Checked = (stat = "APPROVED" Or stat = "APPROVE" Or stat = "CLOSED")
                        End If

                        If ddlSupplier IsNot Nothing Then ddlSupplier.Visible = False
                        If lblSupplier IsNot Nothing Then lblSupplier.Visible = True

                        If imgbtnDelete IsNot Nothing Then imgbtnDelete.Visible = True
                        If imgbtnReject IsNot Nothing Then imgbtnReject.Visible = False
                    End If
                End If
            End If
        End If
    End Sub

    ' ============================================================
    ' BindData

    ' ============================================================
    Private Function getSupplierDT() As DataTable
        Dim dt As DataTable = CType(ViewState("supplierDT"), DataTable)
        If dt Is Nothing Then
            Try
                ' Use standard method as requested by feedback "all suppliers"
                dt = clsInv.getSupplierbyUser(custid(), sNameuser, iModPriv).Tables(0)
                dt.DefaultView.Sort = "Name ASC"
                dt = dt.DefaultView.ToTable()
                ViewState("supplierDT") = dt
            Catch ex As Exception
                dt = New DataTable()
                dt.Columns.Add("id", GetType(Integer))
                dt.Columns.Add("name", GetType(String))
            End Try
        End If
        Return dt
    End Function

    Protected Sub BindData(ByVal modpriv As Integer)
        Try
            If custid() = 0 Then
                Response.Redirect("~/Default.aspx", False)
            Else
                DgvTransfer.DataSource = DirectCast(ViewState("orderW"), DataTable)
                DgvTransfer.DataBind()
                btnedit.Visible = (DgvTransfer.Rows.Count > 0)

                ' Set edit button visual state
                ToggleEditButton(IsEditingMode)

                ' Toggle columns and buttons based on origin
                Dim origin As String = getCookie("origin")
                btnshow.Visible = (origin <> "ho")

                ' Manage new dynamic btnAddNewRow visibility (only for store and when not read-only/processed)
                If origin = "ho" Then
                    btnAddNewRow.Visible = False
                Else
                    If xstat <> "REQUEST" AndAlso xpr_no <> "" Then
                        btnAddNewRow.Visible = False
                    Else
                        btnAddNewRow.Visible = True
                    End If
                End If

                ' Index 12 is Approve, Index 14 is Supplier, Index 15 is Action
                DgvTransfer.Columns(12).Visible = (origin = "ho")
                DgvTransfer.Columns(14).Visible = (origin = "ho")
                DgvTransfer.Columns(15).Visible = (origin <> "ho")
            End If
        Catch ex As Exception
            errLog(ex.ToString(), "PurchaseRequestDetail:BindData")
            Err.Clear()
        End Try
    End Sub

    Private Sub ToggleEditButton(ByVal isEditing As Boolean)
        If isEditing Then
            btnedit.InnerHtml = "<i class='fas fa-sync'></i>"
        Else
            btnedit.InnerText = "edit"
        End If
    End Sub

    ' ============================================================
    ' Bindingstk1 ÃƒÂ¢Ã¢â€šÂ¬Ã¢â‚¬Â fresh query modal ListView

    ' ============================================================
    Sub Bindingstk1()
        Try
            If icustid = 0 Then
                Response.Redirect("~/Default.aspx", False)
            Else
                Dim dt, dt2 As New DataTable
                dt = DirectCast(ViewState("orderW"), DataTable)
                dt2 = clsStk.SelectDt_Stk_all2(dt, custid, txtSearch.Text, ddlFrom.SelectedValue)
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
            errLog(Err.Description, "PurchaseRequestDetail:Bindingstk1")
            Err.Clear()
        End Try
    End Sub

    ' ============================================================
    ' bindingstk ÃƒÂ¢Ã¢â€šÂ¬Ã¢â‚¬Â re-filter dari ViewState

    ' ============================================================
    Sub bindingstk()
        Try
            If custid() = 0 Then
                Response.Redirect("~/Default.aspx", False)
            Else
                Dim dt, dt2 As New DataTable
                dt = DirectCast(ViewState("orderW"), DataTable)
                dt2 = DirectCast(ViewState("InvStk"), DataTable)
                If dt2.Rows.Count = 0 Then
                    dt2 = clsStk.SelectDt_Stk_all2(dt, custid, txtSearch.Text, ddlFrom.SelectedValue)
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
            errLog(Err.Description, "PurchaseRequestDetail:bindingstk")
            Err.Clear()
        End Try
    End Sub

    ' ============================================================
    ' loadFrom

    ' ============================================================
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
            errLog(Err.Description, "PurchaseRequestDetail:loadFrom")
            Err.Clear()
        End Try
    End Sub

    ' ============================================================
    ' Add selected items dari modal

    ' ============================================================
    Protected Sub RadButton1_ServerClick(sender As Object, e As EventArgs) Handles RadButton1.ServerClick
        Try
            If ddlFrom.SelectedValue <> "0" Then
                Add2()
                'Session("xware") = ddlWare.SelectedValue
                Session("xfrom") = ddlFrom.SelectedValue
            Else
                notsuccesfull("You must select Cost Centre and Warehouse first.")
            End If
        Catch ex As Exception
            errLog(Err.Description, "PurchaseRequestDetail:RadButton1_ServerClick")
            Err.Clear()
        End Try
    End Sub

    Sub Add2()
        Dim dt As New DataTable
        Dim dtStk As New DataTable
        Try
            Dim i As Integer = 0
            If DirectCast(ViewState("orderW"), DataTable).Rows.Count >= My.Settings.limitorder Then
                notsuccesfull("Purchase Request max " & My.Settings.limitorder & " items, please split if more.")
                GoTo ef
            End If
            txtSearch.Text = ""
            BindCheckItem()
            bindingstk()
            CheckedItem()

            For Each item As ListViewItem In LsvStk.Items
                Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox"), CheckBox)
                If ck IsNot Nothing AndAlso ck.Checked Then
                    Try
                        Dim stkId As String = DirectCast(item.FindControl("lblID"), Label).Text
                        Dim stkName As String = DirectCast(item.FindControl("lblName"), Label).Text

                        Dim ds As DataSet = clsStk.SelectDataStk_wOther3(
                            stkId,
                            ddlFrom.SelectedValue,
                            custid,
                            CDate(Now()))

                        If ds.Tables(0).Rows.Count > 0 Then
                            Dim u1 As String = ds.Tables(0).Rows(0).Item("Unit1").ToString
                            Dim factor1 As Object = ds.Tables(0).Rows(0).Item("factor1")
                            Dim dfactor1 As Decimal = If(IsNumeric(factor1), CDec(factor1), 1)
                            Dim dstklimit As Decimal = 0
                            If Not IsDBNull(ds.Tables(0).Rows(0).Item("stk_limit")) Then dstklimit = CDec(ds.Tables(0).Rows(0).Item("stk_limit"))
                            Dim snumber As String = ds.Tables(0).Rows(0).Item("Number").ToString
                            Dim u2 As String = ds.Tables(0).Rows(0).Item("Unit2").ToString
                            'Dim stat As String = ds.Tables(0).Rows(0).Item("stat").ToString
                            dt = DirectCast(ViewState("orderW"), DataTable)
                            ensurePRColumns(dt)

                            Dim newRow As DataRow = dt.NewRow()
                            If dt.Columns("pr_id").DataType Is GetType(String) Then
                                newRow("pr_id") = ""
                            Else
                                newRow("pr_id") = 0
                            End If
                            newRow("pr_no") = txtID.Text
                            If dt.Columns.Contains("custid") Then
                                newRow("custid") = custid()
                            End If
                            newRow("cc") = Val(ddlFrom.SelectedValue)
                            newRow("stk_id") = Val(stkId)
                            newRow("stk_name") = stkName
                            newRow("qty_req") = 0
                            newRow("u2_nm") = u2
                            newRow("number") = snumber
                            newRow("unit1") = u1
                            newRow("U1_nm") = u1
                            newRow("factor1") = dfactor1
                            newRow("stklimit") = dstklimit
                            newRow("U1") = 0
                            newRow("U2") = 0
                            newRow("Total") = 0
                            newRow("app_qty") = 0
                            newRow("stat") = "REQUEST"
                            newRow("remarks") = ""
                            dt.Rows.Add(newRow)
                            ViewState("orderW") = dt

                            ' Remove from search results (InvStk) safely
                            dtStk = DirectCast(ViewState("InvStk"), DataTable)
                            Dim rowsToRemove = dtStk.Select("id = " & stkId)
                            For Each r In rowsToRemove
                                dtStk.Rows.Remove(r)
                            Next
                            dtStk.AcceptChanges()
                            ViewState("InvStk") = dtStk
                        End If

                    Catch error_t As Exception
                        errLog(error_t.ToString, "PurchaseRequestDetail:Add2-inner")
                    End Try
                End If

                If DirectCast(ViewState("orderW"), DataTable).Rows.Count >= My.Settings.limitorder Then
                    notsuccesfull("Maximum " & My.Settings.limitorder & " items reached for this PR.")
                    Exit For
                End If
            Next

            txtSearch.Text = ""
            ViewState("ListItem") = Nothing ' Clear selections
            bindingstk()
            BindData(modulepriv)
ef:
        Catch ex As Exception
            errLog(Err.Description, "PurchaseRequestDetail:Add2")
            Err.Clear()
        End Try
    End Sub

    ''' <summary>Pastikan DataTable memiliki kolom schema cld_inv_prd</summary>
    Private Sub ensurePRColumns(dt As DataTable)
        If Not dt.Columns.Contains("pr_id") Then dt.Columns.Add("pr_id", GetType(Long))
        If Not dt.Columns.Contains("pr_no") Then dt.Columns.Add("pr_no", GetType(String))
        If Not dt.Columns.Contains("custid") Then dt.Columns.Add("custid", GetType(Integer))
        If Not dt.Columns.Contains("cc") Then dt.Columns.Add("cc", GetType(Integer))
        If Not dt.Columns.Contains("stk_id") Then dt.Columns.Add("stk_id", GetType(Integer))
        If Not dt.Columns.Contains("stk_name") Then dt.Columns.Add("stk_name", GetType(String))
        If Not dt.Columns.Contains("qty_req") Then dt.Columns.Add("qty_req", GetType(Decimal))
        If Not dt.Columns.Contains("u2_nm") Then dt.Columns.Add("u2_nm", GetType(String))
        If Not dt.Columns.Contains("number") Then dt.Columns.Add("number", GetType(String))
        If Not dt.Columns.Contains("unit1") Then dt.Columns.Add("unit1", GetType(String))
        If Not dt.Columns.Contains("U1_nm") Then dt.Columns.Add("U1_nm", GetType(String))
        If Not dt.Columns.Contains("factor1") Then dt.Columns.Add("factor1", GetType(Decimal))
        If Not dt.Columns.Contains("stockcc") Then dt.Columns.Add("stockcc", GetType(Decimal))
        If Not dt.Columns.Contains("stklimit") Then dt.Columns.Add("stklimit", GetType(Decimal))
        ' Kolom yang digunakan oleh Eval di GridView
        If Not dt.Columns.Contains("U1") Then dt.Columns.Add("U1", GetType(Decimal))
        If Not dt.Columns.Contains("U2") Then dt.Columns.Add("U2", GetType(Decimal))
        If Not dt.Columns.Contains("Total") Then dt.Columns.Add("Total", GetType(Decimal))
        If Not dt.Columns.Contains("app_qty") Then dt.Columns.Add("app_qty", GetType(Decimal))
        If Not dt.Columns.Contains("stat") Then dt.Columns.Add("stat", GetType(String))
        If Not dt.Columns.Contains("remarks") Then dt.Columns.Add("remarks", GetType(String))
        If Not dt.Columns.Contains("supplier_id") Then dt.Columns.Add("supplier_id", GetType(Integer))
        If Not dt.Columns.Contains("supplier_name") Then dt.Columns.Add("supplier_name", GetType(String))
    End Sub

    Private Sub saveCurrentGridValues()
        Try
            Dim dt As DataTable = DirectCast(ViewState("orderW"), DataTable)
            If dt Is Nothing Then Exit Sub

            For Each row As GridViewRow In DgvTransfer.Rows
                If row.RowType = DataControlRowType.DataRow Then
                    Dim rowIndex As Integer = row.RowIndex
                    If rowIndex < dt.Rows.Count Then
                        Dim txtEditUnit1 As TextBox = TryCast(row.FindControl("txtEditUnit1"), TextBox)
                        Dim txtEditUnit2 As TextBox = TryCast(row.FindControl("txtEditUnit2"), TextBox)
                        Dim txtEditRemarks As TextBox = TryCast(row.FindControl("txtEditRemarks"), TextBox)
                        Dim lblfactor As Label = TryCast(row.FindControl("lblfactor"), Label)

                        ' Save unit quantities and remarks if they exist and are visible
                        If txtEditUnit1 IsNot Nothing AndAlso txtEditUnit1.Visible AndAlso IsNumeric(txtEditUnit1.Text) Then
                            dt.Rows(rowIndex)("U1") = CDec(txtEditUnit1.Text)
                        End If
                        If txtEditUnit2 IsNot Nothing AndAlso txtEditUnit2.Visible AndAlso IsNumeric(txtEditUnit2.Text) Then
                            dt.Rows(rowIndex)("U2") = CDec(txtEditUnit2.Text)
                        End If
                        If txtEditRemarks IsNot Nothing AndAlso txtEditRemarks.Visible Then
                            dt.Rows(rowIndex)("remarks") = txtEditRemarks.Text
                        End If

                        ' Re-calculate Total quantities
                        Dim u1Val As Decimal = If(dt.Columns.Contains("U1") AndAlso Not IsDBNull(dt.Rows(rowIndex)("U1")), CDec(dt.Rows(rowIndex)("U1")), 0)
                        Dim u2Val As Decimal = If(dt.Columns.Contains("U2") AndAlso Not IsDBNull(dt.Rows(rowIndex)("U2")), CDec(dt.Rows(rowIndex)("U2")), 0)
                        Dim factorVal As Decimal = 1
                        If lblfactor IsNot Nothing AndAlso IsNumeric(lblfactor.Text) Then
                            factorVal = CDec(lblfactor.Text)
                            If factorVal <= 0 Then factorVal = 1
                        End If

                        Dim ttlReq As Decimal = u1Val * factorVal + u2Val
                        If dt.Columns.Contains("Total") Then dt.Rows(rowIndex)("Total") = ttlReq
                        If dt.Columns.Contains("qty_req") Then dt.Rows(rowIndex)("qty_req") = ttlReq
                    End If
                End If
            Next
            dt.AcceptChanges()
            ViewState("orderW") = dt
        Catch ex As Exception
            errLog(ex.Message, "PurchaseRequestDetail:saveCurrentGridValues")
        End Try
    End Sub

    ''' <summary>
    ''' Tambah kolom U1, U2, Total dan isi nilainya dari data DB.
    ''' Dipanggil setelah getDTInvPR agar GridView bisa render Eval("U1"), Eval("U2"), Eval("Total").
    ''' </summary>
    Private Sub ensurePRWorkingColumns(dt As DataTable)
        ' Tambah kolom jika belum ada
        If Not dt.Columns.Contains("U1") Then dt.Columns.Add("U1", GetType(Decimal))
        If Not dt.Columns.Contains("U2") Then dt.Columns.Add("U2", GetType(Decimal))
        If Not dt.Columns.Contains("Total") Then dt.Columns.Add("Total", GetType(Decimal))
        If Not dt.Columns.Contains("factor1") Then dt.Columns.Add("factor1", GetType(Decimal))
        If Not dt.Columns.Contains("U1_nm") Then dt.Columns.Add("U1_nm", GetType(String))
        If Not dt.Columns.Contains("U2_nm") Then dt.Columns.Add("U2_nm", GetType(String))
        If Not dt.Columns.Contains("qty_req") Then dt.Columns.Add("qty_req", GetType(Decimal))
        If Not dt.Columns.Contains("app_qty") Then dt.Columns.Add("app_qty", GetType(Decimal))
        If Not dt.Columns.Contains("stat") Then dt.Columns.Add("stat", GetType(String))
        If Not dt.Columns.Contains("remarks") Then dt.Columns.Add("remarks", GetType(String))
        If Not dt.Columns.Contains("supplier_id") Then dt.Columns.Add("supplier_id", GetType(Integer))
        If Not dt.Columns.Contains("supplier_name") Then dt.Columns.Add("supplier_name", GetType(String))
        ' Isi nilai per baris menggunakan qty_req sebagai Total awal
        For Each row As DataRow In dt.Rows
            Dim qtyReq As Decimal = 0
            If Not IsDBNull(row("qty_req")) AndAlso IsNumeric(row("qty_req")) Then
                qtyReq = CDec(row("qty_req"))
            End If
            If IsDBNull(row("Total")) OrElse CDec(row("Total")) = 0 Then row("Total") = qtyReq
            If IsDBNull(row("U1")) Then row("U1") = 0
            If IsDBNull(row("U2")) Then row("U2") = qtyReq
            If IsDBNull(row("app_qty")) Then row("app_qty") = 0
            If IsDBNull(row("stat")) Then row("stat") = "1"
            If IsDBNull(row("remarks")) Then row("remarks") = ""
            If dt.Columns.Contains("stockcc") AndAlso IsDBNull(row("stockcc")) Then row("stockcc") = 0
            If dt.Columns.Contains("stklimit") AndAlso IsDBNull(row("stklimit")) Then row("stklimit") = 0
        Next
        'Next
        dt.AcceptChanges()
    End Sub

    ' ============================================================
    ' Save PR

    ' ============================================================
    Public Function insertPR(Optional ByVal warningMsg As String = "") As String
        Dim hasil As String = ""
        Try
            Dim msg As String = ""
            Dim newLine As String = "\n"
            Dim dt As DataTable = DirectCast(ViewState("orderW"), DataTable)

            If dt Is Nothing OrElse dt.Rows.Count = 0 Then
                msg = "Item list cannot be empty." & newLine
            End If
            If ddlFrom.SelectedValue = "0" Or ddlFrom.SelectedValue = "" Then
                msg = msg & "Cost Centre must be selected." & newLine
            End If
            If dt IsNot Nothing Then
                For Each row As DataRow In dt.Rows
                    If CDec(row("Total")) = 0 Then
                        msg = msg & " " & row("stk_name").ToString & " - Request Qty cannot be 0." & newLine
                    End If
                Next row
            End If

            If msg <> "" Then
                notsuccesfull(msg)
                GoTo ENDFUNCTION
            End If

            Dim prDate As Date = If(IsDate(txtcreatedate.Text), CDate(txtcreatedate.Text), Date.Now)
            Dim prNo As String = txtID.Text.Trim   ' kosong = new, ada isi = update
            Dim requiredDate As Date = If(IsDate(txtRequiredDate.Text), CDate(txtRequiredDate.Text), Date.Now)
            Dim appDate As Date = Date.Now ' Backend will handle NULL if stat is REQUEST

            ' Determine status for HO
            Dim currentStat As String = "REQUEST"
            Dim origin As String = getCookie("origin")
            If origin = "ho" Then
                currentStat = "REJECTED" ' Default if nothing is approved
                For Each r As DataRow In dt.Rows
                    If r("stat").ToString().ToUpper() = "APPROVED" Then
                        currentStat = "APPROVED"
                        Exit For
                    End If
                Next
            End If

            Dim appByStr As String = If(origin = "ho", username(), "")

            hasil = clsInv.insertInvPR(0,
                prNo, prDate, requiredDate,
                CInt(ddlFrom.SelectedValue),
                username(), currentStat, appByStr,
                appDate,
                dt,
                txtNotes.Text)

            If hasil = "Error" Or hasil = "" Then
                notsuccesfull("Failed to save Purchase Request. Please try again.")
                GoTo ENDFUNCTION
            End If

            mfunction.CreateCookie("wType", "1")

            Dim alertMsg As String = "Purchase Request saved successfully."
            If warningMsg <> "" Then
                alertMsg = warningMsg & "\n\n" & alertMsg
            End If

            Dim finalPRNo As String = hasil
            If hasil.Contains("|") Then
                Dim parts() As String = hasil.Split("|"c)
                finalPRNo = parts(0)
                alertMsg &= "\n\n" & parts(1)
            End If

            Dim targetPage As String = If(origin = "ho", "PurchaseRequestHO.aspx", "PurchaseRequest.aspx")
            Dim script As String = "alert('" & alertMsg.Replace("'", "\'").Replace(vbCrLf, "\n").Replace("\n", "\\n") & "'); window.location.href='" & targetPage & "';"
            ClientScript.RegisterStartupScript(Me.GetType(), "saveSuccess", script, True)

        Catch ex As Exception
            errLog(Err.Description, "PurchaseRequestDetail:insertPR")
            Err.Clear()
        End Try
ENDFUNCTION:
        Return hasil
    End Function

    ' ============================================================
    ' Button Save

    ' ============================================================
    Private Sub ProcessSave(ByVal skipWarning As Boolean)
        Dim msgSave As String = ""
        Try
            ' Validate: Ensure no row has stk_id = 0 (blank dynamic row)
            Dim dtCheck As DataTable = DirectCast(ViewState("orderW"), DataTable)
            If dtCheck IsNot Nothing Then
                For i As Integer = 0 To dtCheck.Rows.Count - 1
                    Dim stkId As Integer = If(IsDBNull(dtCheck.Rows(i)("stk_id")), 0, CInt(dtCheck.Rows(i)("stk_id")))
                    If stkId = 0 Then
                        notsuccesfull("Please select an item for all added rows or delete empty rows before saving.")
                        Exit Sub
                    End If
                Next
            End If
            For Each row As GridViewRow In DgvTransfer.Rows
                If row.RowType = DataControlRowType.DataRow Then
                    Dim originS As String = getCookie("origin")
                    Dim lblEditUnit2 As Label = TryCast(row.FindControl("lblEditUnit2"), Label)
                    Dim txtEditUnit2 As TextBox = TryCast(row.FindControl("txtEditUnit2"), TextBox)
                    Dim lblfactor As Label = TryCast(row.FindControl("lblfactor"), Label)
                    Dim txtEditUnit1 As TextBox = TryCast(row.FindControl("txtEditUnit1"), TextBox)
                    Dim ttlReq As Decimal = 0
                    If txtEditUnit2 IsNot Nothing Then
                        lblEditUnit2.Visible = True
                        txtEditUnit2.Visible = False
                        lblEditUnit2.Text = txtEditUnit2.Text
                        If IsNumeric(txtEditUnit2.Text) = False Then
                            msgSave = msgSave & " Row " & (row.RowIndex + 1).ToString & " Qty must be a number.\n"
                        End If
                        If Val(txtEditUnit2.Text) >= 999999999 Then
                            msgSave = msgSave & " Row " & (row.RowIndex + 1).ToString & " Qty must be < 999,999,999.\n"
                        End If

                        Dim dt As DataTable = DirectCast(ViewState("orderW"), DataTable)

                        If originS = "ho" Then
                            Dim chkApp As CheckBox = TryCast(row.FindControl("chkApprove"), CheckBox)
                            Dim txtAppQty As TextBox = TryCast(row.FindControl("txtEditAppQty"), TextBox)
                            Dim ddlSup As DropDownList = TryCast(row.FindControl("ddlSupplier"), DropDownList)
                            Dim lblNm As Label = TryCast(row.FindControl("lblName"), Label)

                            If chkApp IsNot Nothing AndAlso chkApp.Checked Then
                                Dim appQty As Decimal = 0
                                If txtAppQty IsNot Nothing AndAlso IsNumeric(txtAppQty.Text) Then appQty = CDec(txtAppQty.Text)

                                If appQty = 0 OrElse (ddlSup IsNot Nothing AndAlso ddlSup.SelectedValue = "0") Then
                                    Dim itemName As String = If(lblNm IsNot Nothing, lblNm.Text, "Item")
                                    msgSave = msgSave & "- " & itemName & "\n"
                                End If
                            End If
                        End If

                        If CDec(txtEditUnit1.Text) < 0 Then
                            txtEditUnit1.Text = "0"
                        End If
                        If CDec(lblfactor.Text) <= 0 Then
                            lblfactor.Text = "1"
                        End If

                        If originS <> "ho" Then
                            ttlReq = CDec(txtEditUnit1.Text) * CDec(lblfactor.Text) + CDec(txtEditUnit2.Text)
                            If dt.Columns.Contains("U1") Then dt.Rows(row.RowIndex)("U1") = CDec(txtEditUnit1.Text)
                            If dt.Columns.Contains("U2") Then dt.Rows(row.RowIndex)("U2") = CDec(txtEditUnit2.Text)
                            If dt.Columns.Contains("Total") Then dt.Rows(row.RowIndex)("Total") = ttlReq
                            If dt.Columns.Contains("qty_req") Then dt.Rows(row.RowIndex)("qty_req") = ttlReq
                        End If

                        ' HO Approval capture
                        Dim chkApprove As CheckBox = TryCast(row.FindControl("chkApprove"), CheckBox)
                        Dim txtEditAppQty As TextBox = TryCast(row.FindControl("txtEditAppQty"), TextBox)
                        Dim txtEditRemarks As TextBox = TryCast(row.FindControl("txtEditRemarks"), TextBox)

                        If txtEditRemarks IsNot Nothing Then
                            dt.Rows(row.RowIndex)("remarks") = txtEditRemarks.Text
                        End If

                        If chkApprove IsNot Nothing AndAlso originS = "ho" Then
                            If chkApprove.Checked Then
                                dt.Rows(row.RowIndex)("stat") = "APPROVED"
                                If txtEditAppQty IsNot Nothing AndAlso IsNumeric(txtEditAppQty.Text) Then
                                    dt.Rows(row.RowIndex)("app_qty") = CDec(txtEditAppQty.Text)
                                End If
                                Dim ddlSupplier As DropDownList = TryCast(row.FindControl("ddlSupplier"), DropDownList)
                                If ddlSupplier IsNot Nothing AndAlso ddlSupplier.SelectedValue <> "0" Then
                                    dt.Rows(row.RowIndex)("supplier_id") = CInt(ddlSupplier.SelectedValue)
                                    dt.Rows(row.RowIndex)("supplier_name") = ddlSupplier.SelectedItem.Text
                                End If
                            Else
                                dt.Rows(row.RowIndex)("stat") = "REJECTED"
                                dt.Rows(row.RowIndex)("app_qty") = 0
                            End If
                        End If

                        dt.AcceptChanges()
                        ViewState("orderW") = dt
                    End If
                End If
            Next

            If Not skipWarning AndAlso msgSave <> "" Then
                Dim fullMsg As String = "PERHATIAN:\nItem berikut tidak akan diorder ke supplier karena Supplier belum dipilih atau App Qty = 0:\n\n" & msgSave.Replace("'", "\'") & "\nTetap lanjutkan proses Save?"
                Dim script As String = "if(confirm('" & fullMsg & "')) { document.getElementById('" & btnForceSave.ClientID & "').click(); }"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ConfirmSave", script, True)
                Exit Sub
            End If

            insertPR("")
        Catch ex As Exception
            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('Failed to save. Please try again.');", True)
            errLog(Err.Description, "PurchaseRequestDetail:ProcessSave")
            Err.Clear()
        End Try
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSave.ServerClick
        ProcessSave(False)
    End Sub

    Protected Sub btnForceSave_ServerClick(ByVal sender As Object, ByVal e As EventArgs)
        ProcessSave(True)
    End Sub

    Protected Sub btnedit_ServerClick(sender As Object, e As EventArgs) Handles btnedit.ServerClick
        Try
            Dim origin As String = getCookie("origin")
            If Not IsEditingMode Then
                IsEditingMode = True
                BindData(modulepriv())
            Else
                IsEditingMode = False
                update()
                BindData(modulepriv())
            End If
        Catch ex As Exception
            errLog(Err.Description, "PurchaseRequestDetail:btnedit_ServerClick")
            Err.Clear()
        End Try
    End Sub

    Protected Sub btnSetAll_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSetAll.ServerClick
        Dim dt As DataTable = DirectCast(ViewState("orderW"), DataTable)
        Try
            If dt.Rows.Count = 0 Then
                notsuccesfull("You must add items before using Set Stock Limit.")
            Else
                For Each row As DataRow In dt.Rows
                    Dim limit As Decimal = 0
                    If dt.Columns.Contains("stklimit") AndAlso Not IsDBNull(row("stklimit")) Then limit = CDec(row("stklimit"))
                    Dim stockcc As Decimal = 0
                    If dt.Columns.Contains("stockcc") AndAlso Not IsDBNull(row("stockcc")) Then stockcc = CDec(row("stockcc"))
                    Dim reqQty As Decimal = limit - stockcc
                    If reqQty < 0 Then reqQty = limit
                    If dt.Columns.Contains("Total") Then row("Total") = reqQty
                    If dt.Columns.Contains("qty_req") Then row("qty_req") = reqQty
                Next row
                dt.AcceptChanges()
                ViewState("orderW") = dt
            End If
            BindData(modulepriv())
        Catch ex As Exception
            errLog(Err.Description, "PurchaseRequestDetail:btnSetAll_Click")
            Err.Clear()
        End Try
    End Sub

    Protected Sub btnBack_ServerClick(sender As Object, e As EventArgs) Handles btnBack.ServerClick
        Dim origin As String = getCookie("origin")
        If origin = "ho" Then
            Response.Redirect("~/PurchaseRequestHO.aspx", False)
        Else
            Response.Redirect("~/PurchaseRequest.aspx", False)
        End If
    End Sub

    ' ============================================================
    ' Search & Modal

    ' ============================================================
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.ServerClick
        Try
            BindCheckItem()
            bindingstk()
            CheckedItem()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Pop", "openModalItem();", True)
        Catch ex As Exception
            errLog(Err.Description, "PurchaseRequestDetail:btnSearch_Click")
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
            errLog(Err.Description, "PurchaseRequestDetail:txtSearch_TextChanged")
            Err.Clear()
        End Try
    End Sub

    Protected Sub ddlFrom_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlFrom.SelectedIndexChanged
        Dim dt As New DataTable
        If DirectCast(ViewState("orderW"), DataTable) IsNot Nothing Then
            dt = DirectCast(ViewState("orderW"), DataTable)
        End If
        If Session("xfrom") Is Nothing Then Session("xfrom") = 0
        Try
            If dt.Rows.Count > 0 And ddlFrom.SelectedValue <> Session("xfrom").ToString Then
                notsuccesfull("You must delete all items before changing Cost Centre.")
                ddlFrom.SelectedValue = Session("xfrom").ToString
            End If
            Bindingstk1()
        Catch ex As Exception
            errLog(Err.Description, "PurchaseRequestDetail:ddlFrom_SelectedIndexChanged")
            Err.Clear()
        End Try
    End Sub

    ' ============================================================
    ' Delete item per row

    ' ============================================================
    Protected Sub imgbtnDelete_Click(sender As Object, e As ImageClickEventArgs)
        Dim lb As ImageButton = CType(sender, ImageButton)
        Dim gvRow As GridViewRow = CType(lb.NamingContainer, GridViewRow)
        Dim rowID As Integer = gvRow.RowIndex
        Try
            If ViewState("orderW") IsNot Nothing Then
                Dim dt As DataTable = CType(ViewState("orderW"), DataTable)
                Dim dtstk As DataTable = DirectCast(ViewState("InvStk"), DataTable)
                Dim confirmValue As String = Request.Form("confirm_value")
                If confirmValue = "Yes" Then
                    If gvRow.RowIndex <= dt.Rows.Count - 1 Then
                        Dim prDetailId As String = dt.Rows(rowID).Item("pr_id").ToString
                        Dim stkName As String = dt.Rows(rowID).Item("stk_name").ToString
                        Dim stkId As String = dt.Rows(rowID).Item("stk_id").ToString
                        If IsNumeric(prDetailId) AndAlso CInt(prDetailId) > 0 Then
                            clsInv.DeleteInvPRd(CInt(prDetailId), username())
                        End If
                        dt.Rows.Remove(dt.Rows(rowID))
                        dt.AcceptChanges()
                        EditingRowIndex = -1
                        If dtstk IsNot Nothing Then
                            dtstk.Rows.Add(stkId, stkName)
                            ViewState("InvStk") = dtstk
                        End If
                    End If
                Else
                    Exit Sub
                End If
                ViewState("orderW") = dt
                DgvTransfer.DataSource = dt
                DgvTransfer.DataBind()
                btnedit.Visible = (DgvTransfer.Rows.Count > 0)
            End If
        Catch ex As Exception
            errLog(Err.Description, "PurchaseRequestDetail:imgbtnDelete_Click")
            Err.Clear()
        End Try
    End Sub

    Protected Sub imgbtnReject_Click(sender As Object, e As ImageClickEventArgs)
        Dim lb As ImageButton = CType(sender, ImageButton)
        Dim gvRow As GridViewRow = CType(lb.NamingContainer, GridViewRow)
        Dim rowID As Integer = gvRow.RowIndex
        Try
            If ViewState("orderW") IsNot Nothing Then
                Dim dt As DataTable = CType(ViewState("orderW"), DataTable)
                Dim confirmValue As String = Request.Form("confirm_value")
                If confirmValue = "Yes" Then
                    If gvRow.RowIndex <= dt.Rows.Count - 1 Then
                        Dim prDetailId As String = dt.Rows(rowID).Item("pr_id").ToString
                        Dim stkName As String = dt.Rows(rowID).Item("stk_name").ToString

                        If IsNumeric(prDetailId) AndAlso CInt(prDetailId) > 0 Then
                            ' Soft Reject: Update status to REJECTED in DB
                            clsInv.RejectInvPRd(CInt(prDetailId), username())
                        End If

                        ' Update DataTable status for UI feedback (if stat column exists)
                        If dt.Columns.Contains("stat") Then
                            dt.Rows(rowID)("stat") = "REJECTED"
                        End If

                        ' Also set app_qty to 0
                        If dt.Columns.Contains("app_qty") Then
                            dt.Rows(rowID)("app_qty") = 0
                        End If

                        dt.AcceptChanges()
                        notsuccesfull("Item '" & stkName & "' has been REJECTED.")
                    End If
                Else
                    Exit Sub
                End If
                ViewState("orderW") = dt
                BindData(modulepriv())
            End If
        Catch ex As Exception
            errLog(Err.Description, "PurchaseRequestDetail:imgbtnReject_Click")
            Err.Clear()
        End Try
    End Sub

    ' ============================================================
    ' Not Approve per row

    ' ============================================================
    Protected Sub btnNotApprove_Click(sender As Object, e As EventArgs)
        Try
            Dim lb As LinkButton = CType(sender, LinkButton)
            Dim gvRow As GridViewRow = CType(lb.NamingContainer, GridViewRow)
            Dim rowID As Integer = gvRow.RowIndex
            If ViewState("orderW") IsNot Nothing Then
                Dim dt As DataTable = CType(ViewState("orderW"), DataTable)
                If rowID <= dt.Rows.Count - 1 Then
                    ' Set app_qty = 0 untuk baris ini (tandai sebagai not approved)
                    If dt.Columns.Contains("app_qty") Then
                        dt.Rows(rowID)("app_qty") = 0
                    End If
                    ' Jika sudah tersimpan di DB (pr_id > 0), biarkan update ke DB dilakukan saat klik tombol Save
                    ' karena insertInvPR akan menyimpan DataTable (termasuk nilai app_qty = 0).
                    Dim prDetailId As String = dt.Rows(rowID).Item("pr_id").ToString
                    'If IsNumeric(prDetailId) AndAlso CInt(prDetailId) > 0 Then
                    '    clsInv.setNotApproveInvPRd(CInt(prDetailId), custid(), username())
                    'End If
                    dt.AcceptChanges()
                    ViewState("orderW") = dt
                    BindData(modulepriv())
                    notsuccesfull("Item '" & dt.Rows(rowID).Item("stk_name").ToString & "' telah ditandai Not Approve.")
                End If
            End If
        Catch ex As Exception
            errLog(Err.Description, "PurchaseRequestDetail:btnNotApprove_Click")
            Err.Clear()
        End Try
    End Sub

    ' ============================================================
    ' Checkbox state management

    ' ============================================================
    Private Sub BindCheckItem()
        Try
            If CBool(IIf(IsNothing(ViewState("ListItem")), True, False)) Then
                Dim ListItem2 As New List(Of Item_Detail)()
                Dim dtorderw As DataTable = DirectCast(ViewState("orderW"), DataTable)
                Dim dtitem As DataTable = clsStk.SelectDt_Stk_all2(dtorderw, custid, "", ddlFrom.SelectedValue)
                If dtitem.Rows.Count > 0 Then
                    For Each drow As DataRow In dtitem.Rows
                        ListItem2.Add(New Item_Detail With {.ItemName = drow.Item("name"), .Status = False})
                    Next
                End If
                For Each item As ListViewItem In LsvStk.Items
                    Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox"), CheckBox)
                    Dim lbl As Label = DirectCast(item.FindControl("lblName"), Label)
                    If ck.Checked Then
                        For Each a In ListItem2
                            If a.ItemName = lbl.Text Then a.Status = True
                        Next
                    End If
                Next
                ViewState("ListItem") = ListItem2
            Else
                Dim ListItem2 As List(Of Item_Detail) = TryCast(ViewState("ListItem"), List(Of Item_Detail))
                For Each item As ListViewItem In LsvStk.Items
                    Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox"), CheckBox)
                    Dim lbl As Label = DirectCast(item.FindControl("lblName"), Label)
                    If ck.Checked Then
                        For Each a In ListItem2
                            If a.ItemName = lbl.Text Then a.Status = True
                        Next
                    Else
                        For Each a In ListItem2
                            If a.ItemName = lbl.Text Then a.Status = False
                        Next
                    End If
                Next
                ViewState("ListItem") = ListItem2
            End If
        Catch ex As Exception
            errLog(Err.Description, "PurchaseRequestDetail:BindCheckItem")
            Err.Clear()
        End Try
    End Sub

    Private Sub CheckedItem()
        Try
            Dim ListStore As List(Of Item_Detail) = TryCast(ViewState("ListItem"), List(Of Item_Detail))
            For Each item As ListViewItem In LsvStk.Items
                Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox"), CheckBox)
                Dim lbl As Label = DirectCast(item.FindControl("lblName"), Label)
                For Each a In ListStore
                    If a.ItemName = lbl.Text Then ck.Checked = a.Status
                Next
            Next
            ViewState("ListItem") = ListStore
        Catch ex As Exception
            errLog(Err.Description, "PurchaseRequestDetail:CheckedItem")
            Err.Clear()
        End Try
    End Sub

    ' ============================================================
    ' Update qty dari edit TextBox ÃƒÂ¢Ã¢â‚¬Â Ã¢â‚¬â„¢ DataTable
    ' Hitung Total = U1 * factor1 + U2, update lblttlReq

    ' ============================================================
    Protected Sub update()
        Dim msg As String = ""
        Dim dt As DataTable = DirectCast(ViewState("orderW"), DataTable)
        Dim origin As String = getCookie("origin")

        For Each row As GridViewRow In DgvTransfer.Rows
            If row.RowType = DataControlRowType.DataRow Then
                Dim lblStkID As Label = TryCast(row.FindControl("lblStkID"), Label)
                Dim stkId As Integer = If(lblStkID IsNot Nothing AndAlso IsNumeric(lblStkID.Text), CInt(lblStkID.Text), 0)

                Dim txtEditUnit1 As TextBox = TryCast(row.FindControl("txtEditUnit1"), TextBox)
                Dim txtEditUnit2 As TextBox = TryCast(row.FindControl("txtEditUnit2"), TextBox)

                ' If stkId = 0 (new empty row), check if they have selected an item.
                ' If not, they shouldn't be allowed to save/complete until selecting or deleting it.
                If stkId = 0 Then
                    msg = msg & " Row " & (row.RowIndex + 1).ToString & ": Please select an item." & Chr(10)
                Else
                    ' Validasi U1
                    If txtEditUnit1 IsNot Nothing AndAlso txtEditUnit1.Visible AndAlso IsNumeric(txtEditUnit1.Text) = False Then
                        msg = msg & " Row " & (row.RowIndex + 1).ToString & ": Qty U1 harus angka." & Chr(10)
                    End If
                    ' Validasi U2
                    If txtEditUnit2 IsNot Nothing AndAlso txtEditUnit2.Visible AndAlso IsNumeric(txtEditUnit2.Text) = False Then
                        msg = msg & " Row " & (row.RowIndex + 1).ToString & ": Qty U2 harus angka." & Chr(10)
                    End If
                End If
            End If
        Next

        If msg <> "" Then
            notsuccesfull(msg)
            Exit Sub
        End If

        For Each row As GridViewRow In DgvTransfer.Rows
            If row.RowType = DataControlRowType.DataRow Then
                Dim lblStkID As Label = TryCast(row.FindControl("lblStkID"), Label)
                Dim stkId As Integer = If(lblStkID IsNot Nothing AndAlso IsNumeric(lblStkID.Text), CInt(lblStkID.Text), 0)

                If stkId > 0 Then
                    Dim lblEditUnit1 As Label = TryCast(row.FindControl("lblEditUnit1"), Label)
                    Dim txtEditUnit1 As TextBox = TryCast(row.FindControl("txtEditUnit1"), TextBox)
                    Dim lblEditUnit2 As Label = TryCast(row.FindControl("lblEditUnit2"), Label)
                    Dim txtEditUnit2 As TextBox = TryCast(row.FindControl("txtEditUnit2"), TextBox)
                    Dim lblfactor As Label = TryCast(row.FindControl("lblfactor"), Label)
                    Dim lblttlReq As Label = TryCast(row.FindControl("lblttlReq"), Label)

                    Dim dU1 As Decimal = If(txtEditUnit1 IsNot Nothing AndAlso IsNumeric(txtEditUnit1.Text), CDec(txtEditUnit1.Text), 0)
                    Dim dU2 As Decimal = If(txtEditUnit2 IsNot Nothing AndAlso IsNumeric(txtEditUnit2.Text), CDec(txtEditUnit2.Text), 0)
                    Dim dFactor As Decimal = 1
                    If lblfactor IsNot Nothing AndAlso IsNumeric(lblfactor.Text) AndAlso CDec(lblfactor.Text) > 0 Then
                        dFactor = CDec(lblfactor.Text)
                    End If

                    Dim dTotal As Decimal = (dU1 * dFactor) + dU2

                    If origin = "ho" Then
                        ' HO Mode: Only App Qty was potentially edited
                        Dim txtEditAppQty As TextBox = TryCast(row.FindControl("txtEditAppQty"), TextBox)
                        Dim lblAppQty As Label = TryCast(row.FindControl("lblAppQty"), Label)

                        If row.RowIndex <= dt.Rows.Count - 1 Then
                            If txtEditAppQty IsNot Nothing AndAlso IsNumeric(txtEditAppQty.Text) Then
                                dt.Rows(row.RowIndex)("app_qty") = CDec(txtEditAppQty.Text)
                            End If
                            Dim txtEditRemarks As TextBox = TryCast(row.FindControl("txtEditRemarks"), TextBox)
                            If txtEditRemarks IsNot Nothing Then
                                dt.Rows(row.RowIndex)("remarks") = txtEditRemarks.Text
                            End If
                        End If
                    Else
                        ' Store Mode: Qty U1/U2 edited
                        Dim txtEditRemarks As TextBox = TryCast(row.FindControl("txtEditRemarks"), TextBox)

                        ' Simpan ke DataTable
                        If row.RowIndex <= dt.Rows.Count - 1 Then
                            If dt.Columns.Contains("U1") Then dt.Rows(row.RowIndex)("U1") = dU1
                            If dt.Columns.Contains("U2") Then dt.Rows(row.RowIndex)("U2") = dU2
                            If dt.Columns.Contains("Total") Then dt.Rows(row.RowIndex)("Total") = dTotal
                            If dt.Columns.Contains("qty_req") Then dt.Rows(row.RowIndex)("qty_req") = dTotal
                            If txtEditRemarks IsNot Nothing Then dt.Rows(row.RowIndex)("remarks") = txtEditRemarks.Text
                        End If
                    End If
                End If
            End If
        Next

        dt.AcceptChanges()
        ViewState("orderW") = dt
    End Sub

    ' ============================================================
    ' Helper class

    ' ============================================================
    <Serializable>
    Public Class Item_Detail
        Public Property ItemName As String = ""
        Public Property Status As Boolean = False


    End Class

    ' ============================================================
    ' Alert helpers

    ' ============================================================
    Sub notsuccesfull(ByVal msg As String)
        Dim sb As New System.Text.StringBuilder()
        sb.Append("<script type='text/javascript'>")
        sb.Append("window.onload=function(){")
        sb.Append("alert('")
        sb.Append(msg)
        sb.Append("')};")
        sb.Append("</script>")
        ClientScript.RegisterClientScriptBlock(Me.GetType(), "Alert", sb.ToString())
    End Sub

    Protected Sub Page_unLoad(sender As Object, e As EventArgs) Handles Me.Unload
    End Sub

    Protected Sub RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
    End Sub

    Protected Sub DgvTransfer_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) 'Handles DgvTransfer.RowUpdating
        Dim txtUnit1 As TextBox = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("txtEditUnit1"), TextBox)
        If IsNumeric(txtUnit1.Text) = False Then
            notsuccesfull("Unit 1 must be number")
            Exit Sub
        End If

        Dim txtUnit2 As TextBox = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("txtEditUnit2"), TextBox)
        If IsNumeric(txtUnit2.Text) = False Then
            notsuccesfull("Unit 2 must be number")
            Exit Sub
        End If

        Dim txtEditU1Cost As TextBox = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("txtEditU1Cost"), TextBox)

        Dim clsI_stk As New clsInventory_stk

        Dim lblfactor1 As Label = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("lblfactor"), Label) 'lblfactor FindControl("lblEditfactor"), Label)
        'lblLosF1.Text = clsI_stk.get_stkFactor1(lblLosF1.Text, custid)   'DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("lblEditfactor"), Label)

        If IsNumeric(txtUnit1.Text) = False Then
            txtUnit1.Text = 0
        End If

        If IsNumeric(txtUnit2.Text) = False Then
            txtUnit2.Text = 0
        End If

        If IsNumeric(txtEditU1Cost.Text) = False Then
            txtEditU1Cost.Text = 0
        End If

        'If txtEditU1Cost.Text = 0 Or txtUnit1.Text = 0 Then
        '    notsuccesfull("Please Input Unit 1 and Cost Unit 1")
        '    Exit Sub
        'End If

        Dim total As Decimal = (Val(txtUnit1.Text) * Val(lblfactor1.Text)) + Val(txtUnit2.Text)

        'tambahan erik 21 sept 2020, buat jagain nilai tidak tll besar
        If txtEditU1Cost.Text = 0 Then
            notsuccesfull("Please Input Cost Unit 1")
            txtUnit1.Text = 0
            txtUnit2.Text = 0
            Exit Sub
        End If

        If Val(txtUnit1.Text) >= 999999999 Then
            notsuccesfull("Unit 1 must less than 999999999")
            Exit Sub
        End If

        If Val(txtUnit2.Text) >= 999999999 Then
            notsuccesfull("Unit 2 must less than 999999999")
            Exit Sub
        End If

        If Val(total) >= 999999999 Then
            notsuccesfull("Total Stock must less than 999999999")
            Exit Sub
        End If

        If Val(txtEditU1Cost.Text) >= 999999999 Then
            notsuccesfull("Cost Must less than 999999999")
            Exit Sub
        End If

        Dim lblUnit1 As Label = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("lblEditEmpID"), Label)

        Dim dt As DataTable = DirectCast(ViewState("orderW"), DataTable)

        If lblfactor1.Text = 0 Then
            lblfactor1.Text = 1
            total = (Val(txtUnit1.Text) * Val(lblfactor1.Text)) + Val(txtUnit2.Text)
        End If

        dt.Rows(e.RowIndex)("factor1") = clsI_stk.get_stkFactor1(dt.Rows(e.RowIndex)("stk_id"), custid) 'lblLosF1.Text

        dt.Rows(e.RowIndex)("U1") = txtUnit1.Text

        dt.Rows(e.RowIndex)("U2") = txtUnit2.Text

        dt.Rows(e.RowIndex)("ttlStock") = total

        dt.Rows(e.RowIndex)("CostU1") = txtEditU1Cost.Text




        Dim cost As Decimal = txtEditU1Cost.Text / lblfactor1.Text

        dt.Rows(e.RowIndex)("Cost") = cost

        Dim ttlcost As Decimal = total * cost
        dt.Rows(e.RowIndex)("ttlCost") = ttlcost


        dt.AcceptChanges()


        DgvTransfer.EditIndex = -1
        BindData()
    End Sub
    Protected Sub BindData()
        Try
            If custid() = 0 Then
                Response.Redirect("~/Default.aspx", False)
            Else
                DgvTransfer.DataSource = DirectCast(ViewState("orderW"), DataTable)
                DgvTransfer.DataBind()
            End If
        Catch ex As Exception

        End Try
    End Sub


    Private Sub ApplyStoreReadOnlyMode()
        Try
            ' 1. Logic Status Badge
            Dim dt As DataTable = DirectCast(ViewState("orderW"), DataTable)
            Dim isApproved As Boolean = False
            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                For Each row As DataRow In dt.Rows
                    If row("stat").ToString().ToUpper() = "APPROVED" AndAlso CDec(row("app_qty")) > 0 Then
                        isApproved = True
                        Exit For
                    End If
                Next
            End If

            If isApproved Then
                lblStatusBadge.Text = "APPROVED"
                lblStatusBadge.CssClass = "prd-badge-approved"
            Else
                lblStatusBadge.Text = "REJECTED"
                lblStatusBadge.CssClass = "prd-badge-rejected"
            End If

            ' 2. Disable Inputs
            txtRequiredDate.Enabled = False
            txtNotes.Enabled = False
            ddlFrom.Enabled = False

            ' 3. Manage Buttons
            btnedit.Disabled = True
            btnshow.Disabled = True ' User requested "disabled", not hidden
            btnSave.Disabled = True
            btnSetAll.Visible = False ' Hide this as it's purely for editing

            ' 4. Manage GridView Columns
            ' Column 14 is the Action column (Delete/Reject)
            If DgvTransfer.Columns.Count > 14 Then
                DgvTransfer.Columns(14).Visible = False
            End If

        Catch ex As Exception
            errLog(ex.Message, "PurchaseRequestDetail:ApplyStoreReadOnlyMode")
        End Try
    End Sub

    Protected Sub btnAddNewRow_Click(sender As Object, e As EventArgs)
        Try
            ' 1. Validate Cost Centre selection
            If ddlFrom.SelectedValue = "0" OrElse ddlFrom.SelectedValue = "" Then
                notsuccesfull("You must select Cost Centre first.")
                Exit Sub
            End If

            ' First, save all entered quantities and remarks from current grid rows!
            saveCurrentGridValues()

            Dim dt As DataTable = DirectCast(ViewState("orderW"), DataTable)
            If dt Is Nothing Then
                dt = New DataTable()
            End If
            ensurePRColumns(dt)

            ' 2. Validate max item count limit
            If dt.Rows.Count >= My.Settings.limitorder Then
                notsuccesfull("Purchase Request max " & My.Settings.limitorder & " items, please split if more.")
                Exit Sub
            End If

            ' 3. Append a blank placeholder row (stk_id = 0, stk_name = "")
            Dim newRow As DataRow = dt.NewRow()
            If dt.Columns("pr_id").DataType Is GetType(String) Then
                newRow("pr_id") = ""
            Else
                newRow("pr_id") = 0
            End If
            newRow("pr_no") = txtID.Text
            If dt.Columns.Contains("custid") Then
                newRow("custid") = custid()
            End If
            newRow("cc") = Val(ddlFrom.SelectedValue)
            newRow("stk_id") = 0
            newRow("stk_name") = ""
            newRow("qty_req") = 0
            newRow("u2_nm") = ""
            newRow("number") = ""
            newRow("unit1") = ""
            newRow("U1_nm") = ""
            newRow("factor1") = 1
            newRow("stockcc") = 0
            newRow("stklimit") = 0
            newRow("U1") = 0
            newRow("U2") = 0
            newRow("Total") = 0
            newRow("app_qty") = 0
            newRow("stat") = "REQUEST"
            newRow("remarks") = ""
            newRow("supplier_id") = 0
            newRow("supplier_name") = ""

            dt.Rows.Add(newRow)
            ViewState("orderW") = dt

            ' 4. Automatically switch to editing mode for the newly added row
            IsEditingMode = True
            EditingRowIndex = dt.Rows.Count - 1 ' Track new row

            ' 5. Re-bind GridView
            BindData(modulepriv())

            ' Set focus and open the newly added dropdown!
            Try
                Dim newRowIndex As Integer = dt.Rows.Count - 1
                If newRowIndex >= 0 AndAlso DgvTransfer.Rows.Count > newRowIndex Then
                    Dim lastRow As GridViewRow = DgvTransfer.Rows(newRowIndex)
                    Dim ddlEditItem As DropDownList = TryCast(lastRow.FindControl("ddlEditItem"), DropDownList)
                    If ddlEditItem IsNot Nothing Then
                        Dim scriptFocus As String = "setTimeout(function() { $('#" & ddlEditItem.ClientID & "').select2('open'); }, 200);"
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "FocusNewDropdown", scriptFocus, True)
                    End If
                End If
            Catch ex As Exception
                ' Fail-safe
            End Try

        Catch ex As Exception
            errLog(Err.Description, "PurchaseRequestDetail:btnAddNewRow_Click")
            Err.Clear()
        End Try
    End Sub

    Protected Sub ddlEditItem_SelectedIndexChanged(sender As Object, e As EventArgs)
        Try
            ' First, save all entered quantities and remarks from current grid rows!
            saveCurrentGridValues()

            Dim ddl As DropDownList = CType(sender, DropDownList)
            Dim gvRow As GridViewRow = CType(ddl.NamingContainer, GridViewRow)
            Dim rowIndex As Integer = gvRow.RowIndex

            Dim selectedStkId As Integer = Val(ddl.SelectedValue)
            If selectedStkId > 0 Then
                Dim ds As DataSet = clsStk.SelectDataStk_wOther3(
                    selectedStkId.ToString(),
                    ddlFrom.SelectedValue,
                    custid(),
                    CDate(Now()))

                If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then
                    Dim rowData As DataRow = ds.Tables(0).Rows(0)

                    Dim u1 As String = rowData("Unit1").ToString()
                    Dim factor1 As Object = rowData("factor1")
                    Dim dfactor1 As Decimal = If(IsNumeric(factor1), CDec(factor1), 1)
                    Dim dstklimit As Decimal = 0
                    If Not IsDBNull(rowData("stk_limit")) Then dstklimit = CDec(rowData("stk_limit"))
                    Dim snumber As String = rowData("Number").ToString()
                    Dim u2 As String = rowData("Unit2").ToString()
                    Dim stockcc As Decimal = 0
                    If rowData.Table.Columns.Contains("Stockcc") AndAlso Not IsDBNull(rowData("Stockcc")) Then
                        stockcc = CDec(rowData("Stockcc"))
                    End If
                    Dim stkName As String = ddl.SelectedItem.Text

                    Dim dt As DataTable = DirectCast(ViewState("orderW"), DataTable)
                    If rowIndex < dt.Rows.Count Then
                        Dim row As DataRow = dt.Rows(rowIndex)
                        row("stk_id") = selectedStkId
                        row("stk_name") = stkName
                        row("u2_nm") = u2
                        row("number") = snumber
                        row("unit1") = u1
                        row("U1_nm") = u1
                        row("factor1") = dfactor1
                        row("stockcc") = stockcc
                        row("stklimit") = dstklimit
                        row("U1") = 0
                        row("U2") = 0
                        row("Total") = 0
                        row("app_qty") = 0
                        row("stat") = "REQUEST"
                        row("remarks") = ""

                        dt.AcceptChanges()
                        ViewState("orderW") = dt

                        ' Keep this row index as the only active editing row
                        EditingRowIndex = rowIndex

                        ' Update active warehouse selection tracking
                        Session("xfrom") = ddlFrom.SelectedValue
                    End If
                End If
            End If
            
            ' Re-bind GridView to update UI properly
            BindData(modulepriv())

            ' Set focus to txtEditUnit1 of the same row!
            Try
                Dim dt As DataTable = DirectCast(ViewState("orderW"), DataTable)
                If dt IsNot Nothing AndAlso rowIndex < dt.Rows.Count AndAlso DgvTransfer.Rows.Count > rowIndex Then
                    Dim targetRow As GridViewRow = DgvTransfer.Rows(rowIndex)
                    Dim txtEditUnit1 As TextBox = TryCast(targetRow.FindControl("txtEditUnit1"), TextBox)
                    If txtEditUnit1 IsNot Nothing Then
                        Dim scriptFocus As String = "setTimeout(function() { $('#" & txtEditUnit1.ClientID & "').focus().select(); }, 200);"
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "FocusQtyBox", scriptFocus, True)
                    End If
                End If
            Catch ex As Exception
                ' Fail-safe
            End Try
            
        Catch ex As Exception
            errLog(Err.Description, "PurchaseRequestDetail:ddlEditItem_SelectedIndexChanged")
            Err.Clear()
        End Try
    End Sub

End Class


