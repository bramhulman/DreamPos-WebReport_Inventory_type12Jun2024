Imports System.IO
Imports iTextSharp.text
Imports iTextSharp.text.html.simpleparser
Imports iTextSharp.text.pdf
Imports iTextSharp.tool.xml
Imports System.Data
Imports System.Threading
Imports System.Net
Imports System.Text
Imports System.Drawing

Public Class Pages_InvOrderWHODetail
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
    Private wDelID As String

    Private wDel As Date
    Private wCreate As Date
    Private wRcv As Date

    Private wDelStatus As String

    Private wNotes As String

    Private clsInv As New clsInventory
    Private clsStk As New clsInventory_stk
    Private try1 As Integer
    Private try2 As Integer

    'Private transpriv As Integer
    Public Function modulepriv() As Integer
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        If cookie Is Nothing Then

            Response.Redirect("default.aspx", False)
        End If
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

        If cookie Is Nothing Then

            Response.Redirect("default.aspx", False)
        End If
        Return cookie("rptpriv")

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
            wID = Request.Cookies("woID").Value
            wCc = Request.Cookies("woCc").Value
            wFrom = Request.Cookies("woFrom").Value
            wDelID = Request.Cookies("woDelID").Value

            wNotes = Request.Cookies("woNotes").Value



            wDelStatus = Request.Cookies("woDelstatus").Value
            If Request.Cookies("UserLogin_inv") IsNot Nothing Then
                sNameuser = username()
                icustid = custid()
                imodul = modul()
                iModPriv = modulepriv()
                irptpriv = rptpriv()
                irptf = rptf()

                If IsPostBack = False Then

                    If IsDate(Request.Cookies("woStatDate").Value) Then
                        wDel = Request.Cookies("woStatDate").Value
                    End If

                    If IsDate(Request.Cookies("wDelDate").Value) Then
                        wDel = (Request.Cookies("wDelDate").Value)
                        txtexpD2.Text = Format(wDel, "dd MMM yyyy")

                        Deliverydate.Visible = True
                        'txtexpD2.Enabled = False
                        receivedate.Visible = True
                        txtrcvdate2.Text = Format(Now, "dd MMM yyyy")
                    End If

                    If IsDate(Request.Cookies("wCreatedDate").Value) Then
                        wCreate = (Request.Cookies("wCreatedDate").Value)
                        txtcreatedate2.Text = Format(wCreate, "dd MMM yyyy")
                        Createdate.Visible = True
                        txtcreatedate2.Enabled = False
                        Deliverydate.Visible = True
                        ' txtexpD2.Text = Format(Now, "dd MMM yyyy")
                        If (username() = My.Settings.admin Or username() = "support") And wDel.Date > Now.Date.AddDays(-7) Then
                        Else
                            cle.StartDate = Now.AddDays(-1)
                            cle.EndDate = Now.Date
                        End If
                    End If

                    If IsDate(Request.Cookies("wrcvDate").Value) Then
                        wRcv = (Request.Cookies("wrcvDate").Value)
                        txtrcvdate2.Text = Format(wRcv, "dd MMM yyyy")

                        receivedate.Visible = True
                        txtrcvdate2.Enabled = False
                    End If

                    loadFrom()
                    loadwarehouse()
                    try1 = 0
                    try2 = 0
                    btnedit.InnerText = "Edit"
                    If wDelID = "" Then

                        txtDel.Text = ""
                        'txtID.ReadOnly = True
                        Dim dt, dt2 As New DataTable
                        'dt = WService.getDTInvWare(wID, cust, 0, 0)
                        dt = clsInv.getDTInvWare(wID, custid, wCc, wFrom)

                        ViewState("orderW") = dt
                        txtID.Text = wID
                        txtID.ReadOnly = True

                        txtDel.Text = wDelID
                        txtDel.ReadOnly = True

                        ddlFrom.SelectedValue = wCc
                        ddlFrom.Enabled = False

                        ddlWare.SelectedValue = wFrom
                        ddlWare.Enabled = False
                        txtNotes.Text = wNotes
                        txtNotes.ReadOnly = True
                        BindData(modulepriv)
                        Label3.Text = "Create Date"
                        If wDelStatus = 1 Then
                            ' btnFinish.Enabled = False
                            btnSetAll.Disabled = True
                            DgvTransfer.Columns(5).Visible = False
                            'If Not (modulepriv() = usrModulepriv.moduleuser) And wDel.Date > Now.Date.AddDays(-7) Then

                            If (username() = My.Settings.admin Or username() = "support") And wDel.Date > Now.Date.AddDays(My.Settings.limitday * -1) Then
                                btncancel.Visible = True
                            End If
                            'End If
                            'If CDate(txtexpD.Text).Date < Now.Date Then
                            '    btnFinish.Enabled = False
                            '    DgvTransfer.Columns(10).Visible = False
                            '    lblError.Text = "This transaction already delivery on other day, cannot be modified"
                            'End If
                        ElseIf wDelStatus = 2 Then
                            btnSetAll.Disabled = True
                            btnFinish.Disabled = True
                            btnSetAll.Visible = False
                            DgvTransfer.Columns(10).Visible = False
                            DgvTransfer.Columns(5).Visible = False
                            DgvTransfer.Columns(6).Visible = False
                            DgvTransfer.Columns(7).Visible = False
                        Else
                            DgvTransfer.Columns(6).Visible = False
                            DgvTransfer.Columns(7).Visible = False
                        End If

                    Else

                        txtID.Text = wID
                        txtID.ReadOnly = True

                        txtDel.Text = wDelID
                        txtDel.ReadOnly = True

                        ddlFrom.SelectedValue = wCc
                        ddlFrom.Enabled = False

                        ddlWare.SelectedValue = wFrom
                        ddlWare.Enabled = False

                        'If IsDate(wDel) Then
                        '    txtexpD.Text = Format(wDel, "dd MMM yyyy")
                        'Else
                        '    txtexpD.Text = Format(Now, "dd MMM yyyy")
                        'End If

                        txtNotes.Text = wNotes
                        txtNotes.ReadOnly = True



                        'txtexpD.Enabled = False

                        Dim dt, dt2 As New DataTable
                        dt = clsInv.getDTInvWare(wID, custid, ddlFrom.SelectedValue, ddlWare.SelectedValue)
                        'WService.getDTInvWare(wID, cust, ddlFrom.SelectedValue, ddlWare.SelectedValue)
                        ViewState("orderW") = dt
                        BindData(modulepriv)
                        If wDelStatus = 1 Then
                            ' btnFinish.Enabled = False
                            btnSetAll.Disabled = True
                            DgvTransfer.Columns(5).Visible = False
                            'If Not (modulepriv() = usrModulepriv.moduleuser) And wDel.Date > Now.Date.AddDays(-7) Then

                            If (username() = My.Settings.admin Or username() = "support") And wDel.Date > Now.Date.AddDays(My.Settings.limitday * -1) Then
                                btncancel.Visible = True
                            End If
                            'End If
                            'If CDate(txtexpD.Text).Date < Now.Date Then
                            '    btnFinish.Enabled = False
                            '    DgvTransfer.Columns(10).Visible = False
                            '    lblError.Text = "This transaction already delivery on other day, cannot be modified"
                            'End If
                        ElseIf wDelStatus = 2 Then
                            btnSetAll.Disabled = True
                            btnFinish.Disabled = True
                            btnSetAll.Visible = False
                            DgvTransfer.Columns(10).Visible = False
                            DgvTransfer.Columns(5).Visible = False
                            DgvTransfer.Columns(6).Visible = False
                            DgvTransfer.Columns(7).Visible = False
                        Else
                            DgvTransfer.Columns(6).Visible = False
                            DgvTransfer.Columns(7).Visible = False
                        End If
                    End If

                    ApplyDelModeUI()

                End If
                'btnrcp.InnerText = "receipecost"
                ScriptManager.GetCurrent(Me).RegisterPostBackControl(RadButton1)
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
            errLog(Err.Description, "Pages_InvOrderWHODetail:Page_Load")
            Err.Clear()
        End Try
    End Sub
    Sub loadFrom()
        Dim ds As New DataSet
        Try
            ds = clsInv.getIccOnly(custid, username, modulepriv)
            'WService.getIccOnly(cust, username, ModPrv)

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
            errLog(Err.Description, "Pages_InvOrderWHODetail:loadFrom")
            Err.Clear()
        End Try
    End Sub

    Sub loadwarehouse()
        Dim ds As New DataSet
        Try
            ds = clsInv.getIWareOnly(custid)
            'WService.getIWareOnly(cust)
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
            End If
        Catch ex As Exception
            errLog(Err.Description, "Pages_InvOrderWHODetail:loadwarehouse")
            Err.Clear()
        End Try
    End Sub
    Protected Sub BindData(ByVal modpriv As Integer)

        Try
            If custid() = 0 Then
                Response.Redirect("~/Default.aspx", False)
            Else

                Dim totalcost As Double = 0

                For Each dr As DataRow In DirectCast(ViewState("orderW"), DataTable).Rows
                    totalcost += Convert.ToDouble(dr("totalcost"))
                Next

                Dim dt As DataTable = DirectCast(ViewState("orderW"), DataTable)
                If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                    If dt.Columns.Contains("Number") Then
                        dt.DefaultView.Sort = "Number ASC"
                    ElseIf dt.Columns.Contains("stk_no") Then
                        dt.DefaultView.Sort = "stk_no ASC"
                    ElseIf dt.Columns.Contains("stk_name") Then
                        dt.DefaultView.Sort = "stk_name ASC"
                    End If
                End If
                
                DgvTransfer.Columns(10).FooterText = totalcost.ToString("#,##0.00")
                DgvTransfer.DataSource = dt
                If modpriv And usrModulepriv.moduleuser = usrModulepriv.moduleuser Then
                Else
                    DgvTransfer.Columns(8).Visible = True
                End If
                DgvTransfer.DataBind()
                DgvTransfer.ShowFooter = True
                If GetBackground(wID) = 0 Then
                    DgvTransfer.Columns(11).Visible = False
                    btnedit.Visible = False
                    btnFinish.Visible = False
                    txtexpD2.Enabled = False
                End If

                'DgvTransfer.DataSource = DirectCast(ViewState("orderW"), DataTable)
                'If modpriv And usrModulepriv.moduleuser = usrModulepriv.moduleuser Then
                'Else
                '    DgvTransfer.Columns(8).Visible = True
                'End If
                DgvTransfer.DataBind()
            End If
        Catch ex As Exception
            errLog(Err.Description, "Pages_InvOrderWHODetail:BindData")
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
        Catch ex As Exception
            errLog(Err.Description, "Pages_InvOrderWHODetail:Bindingstk1")
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


                If IsNothing(ViewState("InvStk")) = False Then

                    dt2 = DirectCast(ViewState("InvStk"), DataTable)

                    If dt2.Rows.Count = 0 Then
                        dt2 = clsStk.SelectDataTable_Stk_wStock(dt, custid, txtSearch.Text, ddlFrom.SelectedValue)
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

            End If
        Catch ex As Exception
            errLog(Err.Description, "Pages_InvOrderWHODetail:bindingstk")
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
        Try
            clsInv.DeleteInvWare(id, custid, iccTo, istk, username)
        Catch ex As Exception

        End Try
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

            errLog(Err.Description, "Pages_InvOrderWHODetail:DeleteRcpDtl")
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
    Sub insert(ByVal stat As Integer, Optional ByVal xcancel As Integer = 0)
        Try
            Dim msg As String = ""
            Dim msg2 As String = ""
            Dim dt As DataTable = DirectCast(ViewState("orderW"), DataTable)
            Dim newLine As String = "\n"
            Dim c_cmd As New Cls_command
            If dt.Rows.Count = 0 Then
                msg = msg + "Item is Nothing" & ":"
            End If
            If ddlFrom.SelectedValue = 0 Or ddlFrom.SelectedItem.Text = "" Then
                msg = msg + "Cost Centre Must be Choice " & ":"
            End If

            If ddlWare.SelectedValue = 0 Or ddlWare.SelectedItem.Text = "" Then
                msg = msg + "Warehouse Must be Choice" & ":"
            End If

            For Each rowview As DataRow In dt.Rows
                If rowview.Item("U1Del") = 0 Then
                    msg2 = msg2 + rowview.Item("Stk_name") + " DEL QTY = 0 " & ";"
                End If
                If rowview.Item("cost") = 0 Then
                    msg2 = msg2 + rowview.Item("Stk_name") + " DEL PRICE = 0 " & "\n"

                End If
            Next rowview

            If msg <> "" Then
                notsuccesfull(msg + msg2)
                'lblError.Text = msg
                GoTo ENDFUNCTION
            ElseIf msg2 <> "" And try1 = 0 Then
                'messagesuccesfull(msg2)
                'try1 = try1 + 1
                'GoTo ENDFUNCTION
                'Dim confirmValue As String = Request.Form("confirm_price")
                'If confirmValue = "Yes" Then
                'Else
                '    Exit Sub

                'End If
            End If
            If txtexpD2.Text = "" Then
                txtexpD2.Text = Format(Now, "dd MMM yyyy")
            End If

            'ambil data dari dgv

            If c_cmd.ExecuteScalar("select count(custid) from cld_inv_closing where custid = " & custid() & " and invdate = '" & Date.Now.Date.ToString("yyyy-MM-dd") & "' and  invflag = 0 ", 0) = 1 Then
                FormsAuthentication.SetAuthCookie(username, False)
                messagesuccesfull("Closing is in progress. Please log in again.", False)
                Exit Sub
            Else
                Dim delMode As String = clsInv.getInvOrderDelMode(txtID.Text, custid(), ddlFrom.SelectedValue)
                If delMode <> "P" AndAlso rblDelMode.SelectedValue = "P" Then
                    clsInv.setInvOrderDelMode(txtID.Text, custid(), ddlFrom.SelectedValue, "P")
                    delMode = "P"
                End If
                Dim res As String
                If delMode = "P" AndAlso xcancel = 0 Then
                    res = clsInv.insertInvOrderWPartialDel(txtID.Text, custid(), ddlWare.SelectedValue, ddlWare.SelectedItem.ToString,
                        ddlFrom.SelectedValue, ddlFrom.SelectedItem.ToString, username, CDate(txtexpD2.Text), dt, txtNotes.Text)
                    If res <> "Error" Then
                        txtDel.Text = res
                        messagesuccesfull("Delivery note " & res & " has been saved.")
                        ResetPartialShipmentGrid()
                        BindDelHistory()
                        ApplyDelModeUI()
                        Exit Sub
                    End If
                Else
                    res = clsInv.insertInvOrderW(txtID.Text, custid(), 0, 2, ddlWare.SelectedValue, ddlWare.SelectedItem.ToString, ddlFrom.SelectedValue, ddlFrom.SelectedItem.ToString, username, username, CDate(txtexpD2.Text), dt, txtDel.Text, txtNotes.Text, xcancel)
                End If

                If res = "Error" Then
                    notsuccesfull("Transaction Failed. Check logs.")
                    Exit Sub
                End If

            End If


            If msg2 <> "" Then
                msg2 = msg2 + " are you sure? " & "\n"
                messagesuccesfull(msg2)

            Else
                Response.Redirect("~/Pages_InvOrderWHO.aspx", False)
            End If

        Catch ex As Exception

            errLog(Err.Description, "Pages_InvOrderWHODetail:insert")
            Err.Clear()
        End Try
ENDFUNCTION:
    End Sub
    'Sub messagesuccesfull(ByVal msg As String, Optional ByVal bsuccess As Boolean = False)

    '    Dim message As String = msg '"Import From Excel Successfully."
    '    Dim url As String = ""

    '    If bsuccess Then
    '        url = "/Pages_InvOrderWHO.aspx"
    '    Else
    '        url = "/Pages_InvOrderWHO.aspx"
    '    End If

    '    Dim script As String = "window.onload = function(){ alert('"
    '    script += message
    '    script += "');"
    '    script += "window.location = '"
    '    script += url
    '    script += "'; }"
    '    ClientScript.RegisterStartupScript(Me.GetType(), "ServerControlScript", script, True)

    'End Sub
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
                bindingstk()


            Else
                Exit Sub
            End If
        Catch ex As Exception

            errLog(Err.Description, "Pages_InvOrderWHODetail:DgvTransfer_RowDeleting")
            Err.Clear()
        End Try

    End Sub
    '    Protected Sub DgvTransfer_RowUpdating(ByVal sender As Object, ByVal e As GridViewUpdateEventArgs)
    '        'Del 1
    '        Dim txtUnit1 As TextBox = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("txtEditUnit1Del"), TextBox)
    '        'cost
    '        Dim txtCost As TextBox = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("txtCost"), TextBox)
    '        'stock
    '        Dim lblStock As Label = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("lblEditStock"), Label)

    '        'Del2
    '        Dim lblUnit2 As Label = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("lblEditUnit1Del2"), Label)
    '        Dim txtUnit2 As TextBox = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("txtChange"), TextBox)

    '        Dim dt As DataTable = DirectCast(ViewState("orderW"), DataTable)

    '        Try
    '            If IsNumeric(txtUnit1.Text) = False Then
    '                notsuccesfull("Del qty must be number")
    '                Exit Sub
    '            End If
    '            If IsNumeric(txtCost.Text) = False Then
    '                notsuccesfull("Price must be number")
    '                Exit Sub
    '            End If
    '            lblError.Text = ""

    '            If wDelStatus = 0 Then
    '                'order atau normal

    '                If CDec(lblStock.Text) < CDec(txtUnit1.Text) And try1 = 0 Then
    '                    'notsuccesfull("are sure with Delivery Qty and Price ?")
    '                    notsuccesfull("Delivery Qty cannot more than Stock Qty")
    '                    If CDec(lblStock.Text) < 0 Then
    '                        dt.Rows(e.RowIndex)("U1Del") = 0
    '                    Else
    '                        dt.Rows(e.RowIndex)("U1Del") = CDec(lblStock.Text)
    '                    End If
    '                    try1 = try1 + 1
    '                Else
    '                    If (CDec(txtUnit1.Text) = 0 Or txtCost.Text = 0) And try2 = 0 Then
    '                        notsuccesfull("are sure with Delivery Qty and Price ?")
    '                        try2 = try2 + 1
    '                        'GoTo dtchange
    '                    End If
    '                    dt.Rows(e.RowIndex)("U1Del") = txtUnit1.Text





    '                    'try1 = try1 + 1
    '                    'lblError.Text = "Delivery Qty cannot more than Stock Qty"

    '                End If
    '                dt.Rows(e.RowIndex)("cost") = txtCost.Text
    'dtchange:
    '                dt.AcceptChanges()

    '                DgvTransfer.EditIndex = -1
    '                BindData(modulepriv)
    '            Else


    '                Dim a As Decimal = CDec(txtUnit2.Text) + CDec(txtUnit1.Text)


    '                If CDec(lblStock.Text) < CDec(txtUnit2.Text) And try1 = 0 Then
    '                    'notsuccesfull("are sure with Delivery Qty and Price ?")
    '                    notsuccesfull("Delivery Qty cannot more than Stock Qty")
    '                    If CDec(lblStock.Text) < 0 Then
    '                        dt.Rows(e.RowIndex)("U1Del") = 0
    '                    Else
    '                        'dt.Rows(e.RowIndex)("U1Del") = CDec(lblStock.Text)
    '                        txtUnit2.Text = CDec(lblStock.Text)
    '                        dt.Rows(e.RowIndex)("U1Del") = CDec(txtUnit1.Text) + CDec(txtUnit2.Text)
    '                    End If


    '                    try1 = try1 + 1


    '                Else
    '                    If ((CDec(txtUnit2.Text) + CDec(txtUnit1.Text)) = 0 Or txtCost.Text = 0) And try2 = 0 Then
    '                        notsuccesfull("are sure with Delivery Qty and Price ?")
    '                        try2 = try2 + 1
    '                        'GoTo dtchange2
    '                    End If
    '                    dt.Rows(e.RowIndex)("U1Del") = CDec(txtUnit1.Text) + CDec(txtUnit2.Text)





    '                End If
    '                dt.Rows(e.RowIndex)("cost") = txtCost.Text
    'dtchange2:
    '                dt.AcceptChanges()

    '                DgvTransfer.EditIndex = -1
    '                BindData(modulepriv)
    '            End If
    '        Catch ex As Exception

    '        End Try

    '    End Sub
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
    'Protected Sub DgvTransfer_RowCancelingEdit(ByVal sender As Object, ByVal e As GridViewCancelEditEventArgs)
    '    lblError.Text = ""
    '    DgvTransfer.EditIndex = -1
    '    BindData(modulepriv)
    'End Sub

    'Protected Sub DgvTransfer_RowEditing(ByVal sender As Object, ByVal e As GridViewEditEventArgs)
    '    DgvTransfer.EditIndex = e.NewEditIndex
    '    BindData(modulepriv)
    'End Sub
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
            errLog(Err.Description, "Pages_InvOrderWHODetail:BindCheckItem")
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
            errLog(Err.Description, "Pages_InvOrderWHODetail:CheckedItem")
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
        Add2()
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
            errLog(Err.Description, "Pages_InvOrderWHODetail:add")
            Err.Clear()
        End Try

    End Sub
    Sub Add2()
        'untuk nambah record ke temporary sebelum ke database
        Dim dt As New DataTable
        Try
            Dim i As Integer = 0
            If DirectCast(ViewState("orderW"), DataTable).Rows.Count >= My.Settings.limitorder Then
                notsuccesfull("Please keep each order below " & My.Settings.limitorder & " items. Split the order if needed.")
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

                        Dim ds As DataSet = clsStk.SelectDataStk_wOther((DirectCast(item.FindControl("lblID"), Label).Text), ddlFrom.SelectedValue, custid, ddlWare.SelectedValue, CDate(Now()))


                        If ds.Tables(0).Rows.Count > 0 Then
                            Dim u1 As String = ds.Tables(0).Rows(0).Item("Unit1").ToString
                            Dim u2 As String = ds.Tables(0).Rows(0).Item("Unit2").ToString
                            Dim number As String = ds.Tables(0).Rows(0).Item("Number").ToString
                            Dim stock As Decimal = ds.Tables(0).Rows(0).Item("stock").ToString
                            Dim factor1 As String = ds.Tables(0).Rows(0).Item("factor1").ToString
                            Dim dCost As Decimal = ds.Tables(0).Rows(0).Item("coststk").ToString

                            'Dim dt As DataTable
                            dt = DirectCast(ViewState("orderW"), DataTable)

                            Debug.Print((DirectCast(item.FindControl("lblID"), Label).Text))
                            Debug.Print((DirectCast(item.FindControl("lblName"), Label).Text))
                            Debug.Print(u1.ToString)
                            Debug.Print(factor1.ToString)

                            'dt.Rows.Add("", custid, 0, (DirectCast(item.FindControl("lblID"), Label).Text), (DirectCast(item.FindControl("lblName"), Label).Text), 0, u1, 0, 0, dCost, factor1)
                            dt.Rows.Add(txtID.Text, custid, ddlFrom.SelectedValue, (DirectCast(item.FindControl("lblID"), Label).Text), (DirectCast(item.FindControl("lblName"), Label).Text), 0, u2, 0, 0, factor1, number, u2, stock, dCost, 0, 0, u1)

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
                If dt.Rows.Count >= My.Settings.limitorder Then
                    notsuccesfull("Only " & dt.Rows.Count.ToString & " items can be ordered.")
                    Exit For
                End If
            Next
ef:
            'BindData()
        Catch ex As Exception
            'MsgBox(ex.Message)
            errLog(Err.Description, "Pages_InvOrderWHODetail:Add2")
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
            errLog(Err.Description, "Pages_InvOrderWHODetail:btnSearch_Click")
            Err.Clear()
        End Try
    End Sub
    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        Threading.Thread.Sleep(500)
        BindCheckItem()
        bindingstk()
        CheckedItem()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Pop", "openModalItem();", True)
        txtSearch.Attributes("onfocus") = "javascript:this.setSelectionRange(this.value.length,this.value.length);"
        txtSearch.Focus()
    End Sub
    'Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As EventArgs)
    '    mfunction.CreateCookie("wType", "1")
    '    Response.Redirect("~/Pages_InvOrderW.aspx")
    'End Sub


    Protected Sub btnFinish_Click(sender As Object, e As EventArgs) 'Handles btnFinish.ServerClick
        Dim newLine As String = "\n"
        Dim msg As String = ""
        Try
            If (Now.Hour >= 23 And Now.Minute >= 30) Or (Now.Hour = 0 And Now.Minute <= 30) Then
                notsuccesfull("Please process the transaction after 1.00 AM")
                Exit Sub
            End If
            For Each row As GridViewRow In DgvTransfer.Rows

                If row.RowType = DataControlRowType.DataRow Then

                    If row.RowType = DataControlRowType.DataRow Then
                        Dim lblUnit1Del As Label = (TryCast(row.FindControl("lblUnit1Del"), Label))

                        Dim txtEditUnit1Del As TextBox = (TryCast(row.FindControl("txtEditUnit1Del"), TextBox))

                        Dim lblprice As Label = (TryCast(row.FindControl("lblprice"), Label))
                        Dim txtcost As TextBox = (TryCast(row.FindControl("txtcost"), TextBox))
                        Dim lblttl_Cost As Label = (TryCast(row.FindControl("lblttlCost"), Label))

                        lblUnit1Del.Visible = True
                        txtEditUnit1Del.Visible = False
                        lblprice.Visible = True
                        txtcost.Visible = False

                        lblUnit1Del.Text = txtEditUnit1Del.Text
                        lblprice.Text = txtcost.Text

                        Dim total As Decimal = Val(txtEditUnit1Del.Text) '(Val(txtEditUnit1.Text) * Val(lblLosF1.Text)) + Val(txtEditUnit2.Text)
                        If IsNumeric(txtEditUnit1Del.Text) = False Then
                            msg = msg + " row " + row.RowIndex.ToString + " Unit 1 must be number" & newLine

                        End If

                        If IsNumeric(txtcost.Text) = False Then
                            msg = msg + " row " + row.RowIndex.ToString + " cost must be number" & newLine
                            txtcost.Text = 0
                        End If

                        If txtcost.Text = 0 And lblUnit1Del.Text > 0 Then

                            msg = msg + " row " & row.RowIndex.ToString & " Please Input Cost"
                            txtcost.Text = 0
                            txtEditUnit1Del.Text = 0

                        End If

                        If Val(txtEditUnit1Del.Text) >= 999999999 Then

                            msg = msg + " row " + row.RowIndex.ToString + " Unit 1 must less than 999999" & newLine
                        End If

                        If Val(txtcost.Text) >= 999999999 Then
                            msg = msg + " row " + row.RowIndex.ToString + " Cost must less than 999999" & newLine
                        End If

                        If Val(total) >= 999999999 Then

                            msg = msg + " row " + row.RowIndex.ToString + " Total Stock must less than 999999" & newLine

                        End If


                        'Dim lblUnit_1 As Label = (TryCast(row.FindControl("lblEditEmpID"), Label))

                        Dim dt As DataTable = DirectCast(ViewState("orderW"), DataTable)
                        Dim clsI_stk As New clsInventory_stk

                        If msg <> "" Then
                            notsuccesfull(msg)
                            Exit Sub
                        End If

                        Dim lbl_stkid As Label = (TryCast(row.FindControl("lblStkID"), Label))


                        dt.Rows(row.RowIndex)("U1Del") = txtEditUnit1Del.Text
                        errLog("Row " & row.RowIndex & " Text: " & txtEditUnit1Del.Text & " dt: " & dt.Rows(row.RowIndex)("U1Del"), "Debug_btnFinish")
                        dt.Rows(row.RowIndex)("cost") = txtcost.Text


                        Dim ttlcost As Decimal = Val(txtcost.Text) * Val(txtEditUnit1Del.Text)
                        dt.Rows(row.RowIndex)("totalcost") = ttlcost
                        dt.AcceptChanges()



                    End If
                End If
            Next

            Dim dtview As DataTable = DirectCast(ViewState("orderW"), DataTable)


            lblError.Text = ""

            For Each row As DataRow In dtview.Rows

                If row.Item("u1del") > row.Item("stock") Then
                    If row.Item("stock") <= 0 Then
                        row.Item("u1Del") = 0
                    Else
                        row.Item("u1Del") = row.Item("stock")
                    End If


                    If lblError.Text <> "" Then
                        lblError.Text = lblError.Text & "<br>" & row.Item("stk_name") & " Delivery Qty cannot more than Stock Qty"
                    Else
                        'lblError.Text = row.Item("stk_name") & " Delivery Qty cannot more than Stock Qty"
                    End If

                End If

                ' row.Item("u1Del") = row.Item("u1")
                dtview.AcceptChanges()
            Next row

            Dim delMode As String = clsInv.getInvOrderDelMode(txtID.Text, custid(), ddlFrom.SelectedValue)
            If delMode = "P" Then
                For Each row As DataRow In dtview.Rows
                    If Not dtview.Columns.Contains("u1Shipped") Then
                        dtview.Columns.Add("u1Shipped", GetType(Decimal))
                        For Each r As DataRow In dtview.Rows
                            r("u1Shipped") = r("u1Del")
                        Next
                    End If
                    Dim shipped As Decimal = Convert.ToDecimal(row("u1Shipped"))
                    Dim remain As Decimal = Convert.ToDecimal(row("totalu2")) - shipped
                    If Convert.ToDecimal(row("u1Del")) > remain + 0.0001D Then
                        lblError.Text = (lblError.Text & "<br>" & row.Item("stk_name") & " exceeds the remaining quantity (" & remain.ToString("0.##") & ")").Trim()
                        row.Item("u1Del") = Math.Max(0, remain)
                    End If
                Next
                dtview.AcceptChanges()
            End If

            BindData(modulepriv)
            insert(2)
        Catch ex As Exception
            errLog(Err.Description, "Pages_InvOrderWHODetail:btnFinish_Click")
            Err.Clear()
        End Try
    End Sub

    Protected Sub btnBack_Click(sender As Object, e As EventArgs) 'Handles btnBack.ServerClick
        Response.Redirect("~/Pages_InvOrderWHO.aspx", False)
    End Sub

    Protected Sub btnSetAll_Click(sender As Object, e As EventArgs) Handles btnSetAll.ServerClick
        Dim dt As DataTable = DirectCast(ViewState("orderW"), DataTable)
        Try

            lblError.Text = ""

            For Each row As DataRow In dt.Rows

                If row.Item("totalu2") > row.Item("stock") Then
                    If row.Item("stock") < 0 Then
                        row.Item("u1Del") = 0
                    Else
                        row.Item("u1Del") = row.Item("stock")
                    End If


                    If lblError.Text <> "" Then
                        lblError.Text = lblError.Text & "<br>" & row.Item("stk_name") & " Delivery Qty cannot more than Stock Qty"
                    Else
                        'lblError.Text = row.Item("stk_name") & " Delivery Qty cannot more than Stock Qty"
                    End If
                Else

                    row.Item("u1Del") = row.Item("totalu2")

                End If

                dt.AcceptChanges()
            Next row

            BindData(modulepriv)
            btnedit.InnerText = "Edit"
            '25 Mei 23 tambahan erik, karena costnya tidak keupdate
            update()
        Catch ex As Exception
            errLog(Err.Description, "Pages_InvOrderWHODetail:btnSetAll_Click")
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
            If (Now.Hour >= 23 And Now.Minute >= 30) Or (Now.Hour = 0 And Now.Minute <= 30) Then
                notsuccesfull("Please process the transaction after 1.00 AM")
                Exit Sub
            End If
            If ViewState("orderW") IsNot Nothing Then


                Dim lblID As Label = DirectCast(DgvTransfer.Rows(rowID).FindControl("lblID"), Label)
                Dim lblstknm As Label = DirectCast(DgvTransfer.Rows(rowID).FindControl("lblName"), Label)
                Dim lblstkid As Label = DirectCast(DgvTransfer.Rows(rowID).FindControl("lblStkID"), Label)

                Dim dt As DataTable = CType(ViewState("orderW"), DataTable)
                Dim xdoctot As Double
                ' If dt.Rows.Count > 1 Then
                Dim confirmValue As String = Request.Form("confirm_value")
                If confirmValue = "Yes" Then
                    If gvRow.RowIndex <= dt.Rows.Count - 1 Then
                        lid = dt.Rows(rowID).Item("wid")
                        clsInv.DeleteInvWare(dt.Rows(rowID).Item("wid"), custid, ddlFrom.SelectedValue, dt.Rows(rowID).Item("stk_ID"), username)
                        'cInv.DeleteInvRcvDetail(dt.Rows(rowID).Item("ID"), custid, username)
                        dt.Rows.Remove(dt.Rows(rowID))
                        dt.AcceptChanges()


                        ResetRowID(dt)
                    End If
                Else
                    Exit Sub
                End If

                'End If

                ViewState("orderW") = dt

                Dim totalcost As Double = 0

                For Each dr As DataRow In dt.Rows
                    totalcost += Convert.ToDouble(dr("totalcost"))
                Next

                DgvTransfer.Columns(11).FooterText = totalcost.ToString("#,##0.00")

                If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                    If dt.Columns.Contains("Number") Then
                        dt.DefaultView.Sort = "Number ASC"
                    ElseIf dt.Columns.Contains("stk_no") Then
                        dt.DefaultView.Sort = "stk_no ASC"
                    ElseIf dt.Columns.Contains("stk_name") Then
                        dt.DefaultView.Sort = "stk_name ASC"
                    End If
                End If

                DgvTransfer.DataSource = dt
                DgvTransfer.DataBind()
                DgvTransfer.ShowFooter = True
                If dt.Rows.Count > 0 Then
                    xdoctot = dt.Compute("sum(totalcost)", String.Empty)
                Else
                    xdoctot = 0
                End If

                strmaster = "update cld_inv_w set  doctot = " & xdoctot & " where id = '" & lid & "' and custid = " & custid() & ""
                ''" & dt.Rows(rowID).Item("linkid") & "'
                c_cmd.executeNonQuery(strmaster)
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
    'Protected Sub btnSave2_Click(ByVal sender As Object, ByVal e As EventArgs)
    '    Dim rowIndex As Integer = DgvTransfer.EditIndex

    '    'Dim txtUnit1 As TextBox = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("txtEditUnit1"), TextBox)
    '    If IsNumeric(txtUnit1.Text) = False Then
    '        notsuccesfull("Unit 1 must be number")
    '        Exit Sub
    '    End If

    '    ' Dim txtCost As Label = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("txtEditCost"), Label)

    '    'Dim txtUnit2 As TextBox = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("txtEditUnit2"), TextBox)
    '    If IsNumeric(txtDelivery.Text) = False Then
    '        notsuccesfull("Unit 2 must be number")
    '        Exit Sub
    '    End If

    '    'Dim txtEditU1Cost As TextBox = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("txtEditU1Cost"), TextBox)

    '    Dim clsI_stk As New clsInventory_stk

    '    Dim lblLosF1 As Label = DirectCast(DgvTransfer.Rows(rowIndex).FindControl("lblStkEditID"), Label)
    '    lblLosF1.Text = clsI_stk.get_stkFactor1(lblLosF1.Text, custid)   'DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("lblEditfactor"), Label)

    '    If IsNumeric(txtUnit1.Text) = False Then
    '        txtUnit1.Text = 0
    '    End If

    '    If IsNumeric(txtDelivery.Text) = False Then
    '        txtDelivery.Text = 0
    '    End If

    '    If IsNumeric(txtPrice1.Text) = False Then
    '        txtPrice1.Text = 0
    '    End If

    '    'If txtEditU1Cost.Text = 0 Or txtUnit1.Text = 0 Then
    '    '    notsuccesfull("Please Input Unit 1 and Cost Unit 1")
    '    '    Exit Sub
    '    'End If

    '    Dim total As Decimal = (Val(txtUnit1.Text) * Val(lblLosF1.Text)) + Val(txtDelivery.Text)

    '    'tambahan erik 21 sept 2020, buat jagain nilai tidak tll besar
    '    If txtPrice1.Text = 0 Then
    '        notsuccesfull("Please Input Price Unit 1")
    '        Exit Sub
    '    End If

    '    If Val(txtUnit1.Text) >= 999999 Then
    '        notsuccesfull("Unit 1 must less than 999999")
    '        Exit Sub
    '    End If

    '    If Val(txtDelivery.Text) >= 999999 Then
    '        notsuccesfull("Unit 2 must less than 999999")
    '        Exit Sub
    '    End If

    '    If Val(total) >= 999999 Then
    '        notsuccesfull("Total Stock must less than 999999")
    '        Exit Sub
    '    End If

    '    If Val(txtPrice1.Text) >= 999999999 Then
    '        notsuccesfull("Cost Must less than 999999999")
    '        Exit Sub
    '    End If

    '    Dim lblUnit1 As Label = DirectCast(DgvTransfer.Rows(rowIndex).FindControl("lblEditEmpID"), Label)

    '    Dim dt As DataTable = DirectCast(ViewState("InvRcv"), DataTable)

    '    dt.Rows(rowIndex)("Factor1") = lblLosF1.Text

    '    dt.Rows(rowIndex)("U1") = txtUnit1.Text

    '    dt.Rows(rowIndex)("U2") = txtDelivery.Text

    '    dt.Rows(rowIndex)("ttlStock") = total

    '    dt.Rows(rowIndex)("CostU1") = txtPrice1.Text


    '    If lblLosF1.Text = 0 Then
    '        lblLosF1.Text = 1
    '    End If

    '    Dim cost As Decimal = txtPrice1.Text / lblLosF1.Text

    '    dt.Rows(rowIndex)("Cost") = cost

    '    Dim ttlcost As Decimal = total * cost
    '    dt.Rows(rowIndex)("ttlCost") = ttlcost

    '    'If Mtid <> "" Then
    '    '    dtupdt.Rows(maxrowup)("Cost") = cost
    '    '    dtupdt.Rows(maxrowup)("ttlCost") = ttlcost
    '    '    dtupdt.AcceptChanges()

    '    '    ViewState("InvRcvupdt") = dtupdt
    '    'End If

    '    dt.AcceptChanges()




    '    Dim sb As System.Text.StringBuilder = New System.Text.StringBuilder()
    '    sb.Append("<script type='text/javascript'>")
    '    sb.Append("$('#editModal').modal('hide');")
    '    sb.Append("</script>")
    '    ScriptManager.RegisterClientScriptBlock(Me, Me.[GetType](), "Update HideModalScript", sb.ToString(), False)
    '    DgvTransfer.EditIndex = -1
    '    BindData()
    '    UpdatePanel10.DataBind()
    'End Sub


    Sub messagesuccesfull(ByVal msg As String, Optional ByVal bsuccess As Boolean = True)

        Dim message As String = msg
        Dim url As String
        If bsuccess Then
            url = "../Pages_InvOrderWHO.aspx"
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

    Private Sub showAlertOnly(ByVal msg As String)
        Dim script As String = "window.onload = function(){ alert('" & msg.Replace("\", "\\").Replace("'", "\'").Replace(vbCrLf, "\n").Replace(vbLf, "\n") & "'); }"
        ClientScript.RegisterStartupScript(Me.GetType(), "AlertOnly", script, True)
    End Sub
    'Protected Sub DgvTransfer_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles DgvTransfer.RowCommand
    '    Dim stkid As Integer = Convert.ToInt32(e.CommandArgument)
    '    Dim dt As DataTable = ViewState("orderW")
    '    dt.DefaultView.RowFilter = "stk_id='" & stkid & "'"
    '    dt.AcceptChanges()

    '    If e.CommandName.Equals("detail") Then
    '        'DetailsView1.DataSource = dt.DefaultView

    '        'DetailsView1.DataBind()
    '        'Dim sb As New System.Text.StringBuilder()
    '        ''Dim url As String = "~/Pages_InvTransfer.aspx"
    '        'sb.Append("<script type = 'text/javascript'>")
    '        'sb.Append("$('#detailModal').modal('show');")
    '        'sb.Append("</script>")

    '        'ScriptManager.RegisterClientScriptBlock(Me, [GetType](), "DetailModalScript", sb.ToString(), False)

    '    End If
    '    If e.CommandName.Equals("Edit") Then
    '        Dim dr As DataRowView = dt.DefaultView(0)

    '        lblstk_name.Text = dr.Item("stk_name").ToString
    '        txtUnit1.Text = dr.Item("U1").ToString
    '        txtDelivery.Text = dr.Item("U1Del").ToString
    '        lblWs_stock.Text = dr.Item("stock").ToString
    '        lblcost1.Text = dr.Item("cost").ToString
    '        lblLast_purchase.Text = dr.Item("lastpurchase").ToString
    '        lblu1_name.Text = dr.Item("u1_nm").ToString
    '        lblResult.Visible = False
    '        Dim sb As New System.Text.StringBuilder()
    '        Dim url As String = "~/Pages_InvTransfer.aspx"
    '        sb.Append("<script type='text/javascript'>")
    '        sb.Append("$('#editModal').modal('show');")
    '        sb.Append("</script>")
    '        ScriptManager.RegisterClientScriptBlock(Me, [GetType](), "EditModalScript", sb.ToString(), False)
    '        'txtU1.Focus()
    '        'sb.Append("$('#detailModal').modal('show');")
    '        'sb.Append("</script>")

    '        'ScriptManager.RegisterClientScriptBlock(Me, [GetType](), "DetailModalScript", sb.ToString(), False)
    '    End If
    '    If e.CommandName.Equals("Delete") Then

    '        Dim sb As New System.Text.StringBuilder()
    '        Dim url As String = "~/Pages_InvTransfer.aspx"
    '        sb.Append("<script type = 'text/javascript'>")
    '        sb.Append("$('#confirmModal').modal('show');")
    '        sb.Append("</script>")

    '        ScriptManager.RegisterClientScriptBlock(Me, [GetType](), "ConfirmModalScript", sb.ToString(), False)


    '    End If
    'End Sub
    Protected Sub btnok_Click(sender As Object, e As EventArgs)

    End Sub

    'Protected Sub Pages_InvOrderWHODetail_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
    '    If DgvTransfer.EditIndex <> -1 Then

    '        If wDelStatus = 1 Then
    '            Dim txtUnit1 As TextBox = DirectCast(DgvTransfer.Rows(DgvTransfer.EditIndex).FindControl("txtEditUnit1Del"), TextBox)
    '            txtUnit1.Enabled = False
    '        End If
    '    End If
    'End Sub

    Protected Sub btncancel_ServerClick(sender As Object, e As EventArgs) Handles btncancel.ServerClick
        Try
            If (Now.Hour >= 23 And Now.Minute >= 30) Or (Now.Hour = 0 And Now.Minute <= 30) Then
                notsuccesfull("Please process the transaction after 1.00 AM")
                Exit Sub
            End If
            If clsInv.get_HOstatus(txtID.Text, ddlFrom.SelectedValue, custid) = 2 Then
                insert(2, 1)
            End If
        Catch ex As Exception
            errLog(Err.Description, "Pages_InvOrderWHODetail:btncancel_ServerClick")
            Err.Clear()
        End Try

    End Sub
    'erik perubahan 25 Mei 2023 waktu save jika ada yang cost dan stock 0 tetep lanjut process
    Protected Sub update()
        Dim msg As String = ""
        Dim newLine As String = "\n"
        Dim dt As DataTable = DirectCast(ViewState("orderW"), DataTable)
        Dim clsI_stk As New clsInventory_stk

        Try
            For Each row As GridViewRow In DgvTransfer.Rows

                If row.RowType = DataControlRowType.DataRow Then

                    If row.RowType = DataControlRowType.DataRow Then
                        Dim lblUnit1Del As Label = (TryCast(row.FindControl("lblUnit1Del"), Label))
                        'Dim Label1 As Label = (TryCast(row.FindControl("Label1"), Label))
                        Dim txtEditUnit1Del As TextBox = (TryCast(row.FindControl("txtEditUnit1Del"), TextBox))
                        'Dim txtEditUnit2 As TextBox = (TryCast(row.FindControl("txtEditUnit2"), TextBox))
                        Dim lblprice As Label = (TryCast(row.FindControl("lblprice"), Label))
                        Dim txtcost As TextBox = (TryCast(row.FindControl("txtcost"), TextBox))
                        Dim lblttl_Cost As Label = (TryCast(row.FindControl("lblttlCost"), Label))
                        'Dim txtEditU1Cost As TextBox = (TryCast(row.FindControl("txtEditU1Cost"), TextBox))
                        lblUnit1Del.Visible = True
                        txtEditUnit1Del.Visible = False
                        lblprice.Visible = True
                        txtcost.Visible = False
                        'Label1.Visible = True
                        'txtEditUnit2.Visible = False
                        'lblU1Cost.Visible = True
                        'txtEditU1Cost.Visible = False
                        lblUnit1Del.Text = txtEditUnit1Del.Text
                        lblprice.Text = txtcost.Text
                        'Dim lblLosF1 As Label = (TryCast(row.FindControl("lblfactor"), Label))
                        Dim total As Decimal = Val(txtEditUnit1Del.Text) '(Val(txtEditUnit1.Text) * Val(lblLosF1.Text)) + Val(txtEditUnit2.Text)
                        If IsNumeric(txtEditUnit1Del.Text) = False Then
                            msg = msg + " row " + row.RowIndex.ToString + " Unit 1 must be number" & newLine

                        End If
                        'Label1.Text = txtEditUnit2.Text
                        'If IsNumeric(txtEditUnit2.Text) = False Then
                        '    msg = msg + " row " + row.RowIndex.ToString + " Unit 2 must be number" & newLine

                        'End If
                        ' lblU1Cost.Text = txtEditU1Cost.Text
                        If IsNumeric(txtcost.Text) = False Then
                            msg = msg + " row " + row.RowIndex.ToString + " cost must be number" & newLine
                            txtcost.Text = 0
                        End If

                        If txtcost.Text = 0 And lblUnit1Del.Text > 0 Then

                            msg = msg + " row " & (TryCast(row.FindControl("lblname"), Label)).Text.ToString & " Please Input Cost"
                            txtcost.Text = 0
                            txtEditUnit1Del.Text = 0

                        End If

                        If Val(txtEditUnit1Del.Text) >= 999999999 Then

                            msg = msg + " row " + row.RowIndex.ToString + " Unit 1 must less than 999999" & newLine
                        End If

                        If Val(txtcost.Text) >= 999999999 Then
                            msg = msg + " row " + row.RowIndex.ToString + " Cost must less than 999999" & newLine
                        End If

                        If Val(total) >= 999999999 Then

                            msg = msg + " row " + row.RowIndex.ToString + " Total Stock must less than 999999" & newLine

                        End If


                        'Dim lblUnit_1 As Label = (TryCast(row.FindControl("lblEditEmpID"), Label))

                        'Dim dt As DataTable = DirectCast(ViewState("orderW"), DataTable)
                        'Dim clsI_stk As New clsInventory_stk
                        'If lblLosF1.Text = 0 Then
                        '    lblLosF1.Text = 1
                        '  total = (Val(txtEditUnit1.Text) * Val(lblLosF1.Text)) + Val(txtEditUnit2.Text)
                        'End If



                        If msg <> "" Then
                            notsuccesfull(msg)
                            Exit Sub
                        End If

                        Dim lbl_stkid As Label = (TryCast(row.FindControl("lblStkID"), Label))


                        'dt.Rows(row.RowIndex)("Factor1") = clsI_stk.get_stkFactor1(lbl_stkid.Text, custid) 'lblLosF1.Text

                        'dt.Rows(row.RowIndex)("totalU2") = txtEditUnit2.Text

                        dt.Rows(row.RowIndex)("U1Del") = txtEditUnit1Del.Text
                        dt.Rows(row.RowIndex)("cost") = txtcost.Text

                        'dt.Rows(row.RowIndex)("U2") = txtEditUnit2.Text

                        'dt.Rows(row.RowIndex)("totalcost") = total

                        'dt.Rows(row.RowIndex)("Cost") = txtcost.Text


                        'Dim cost As Decimal = txtEditU1Cost.Text / lblLosF1.Text

                        'dt.Rows(row.RowIndex)("Cost") = cost

                        Dim ttlcost As Decimal = Val(txtcost.Text) * Val(txtEditUnit1Del.Text)
                        dt.Rows(row.RowIndex)("totalcost") = ttlcost
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
        Catch ex As Exception
            errLog(Err.Description, "Pages_InvOrderWHODetail:update")
            Err.Clear()
        End Try
    End Sub
    Protected Sub btnedit_ServerClick(sender As Object, e As EventArgs) Handles btnedit.ServerClick
        Try
            If btnedit.InnerText.Trim().Equals("Edit", StringComparison.OrdinalIgnoreCase) Then
                For Each row As GridViewRow In DgvTransfer.Rows

                    If row.RowType = DataControlRowType.DataRow Then
                        row.FindControl("txtEditUnit1Del").Visible = True
                        row.FindControl("lblUnit1Del").Visible = False
                        'lblUnit1
                        row.FindControl("lblprice").Visible = False
                        row.FindControl("txtcost").Visible = True
                        'row.FindControl("txtEditU1Cost").Visible = True
                        'row.FindControl("lblU1Cost").Visible = False

                    End If
                Next

                'btnedit.InnerText = "Calc."
                btnedit.InnerHtml = "<i class='fas fa-sync'></i>"
            Else

                Dim dt As DataTable = DirectCast(ViewState("orderW"), DataTable)


                lblError.Text = ""

                For Each row As GridViewRow In DgvTransfer.Rows

                    If row.RowType = DataControlRowType.DataRow Then
                        row.FindControl("txtEditUnit1Del").Visible = False
                        row.FindControl("lblUnit1Del").Visible = True
                        'lblUnit1
                        row.FindControl("lblprice").Visible = True
                        row.FindControl("txtcost").Visible = False
                        'row.FindControl("txtEditU1Cost").Visible = False
                        'row.FindControl("lblU1Cost").Visible = True

                    End If
                Next
                'BindData(modulepriv)
                update()

                For Each row As DataRow In dt.Rows

                    If row.Item("u1del") > row.Item("stock") Then
                        If row.Item("stock") <= 0 Then
                            row.Item("u1Del") = 0
                        Else
                            row.Item("u1Del") = row.Item("stock")
                        End If


                        If lblError.Text <> "" Then
                            lblError.Text = lblError.Text & "<br>" & row.Item("stk_name") & " Delivery Qty cannot more than Stock Qty"
                        Else
                            'lblError.Text = row.Item("stk_name") & " Delivery Qty cannot more than Stock Qty"
                        End If

                        dt.AcceptChanges()
                    End If


                Next row

                BindData(modulepriv)
                btnedit.InnerText = "Edit"
                update()
            End If


        Catch ex As Exception
            errLog(Err.Description, "Pages_InvOrderWDetail:btnedit_ServerClick")
            Err.Clear()
        End Try
    End Sub
    Protected Function GetBackground(ByVal xwid As String)

        Dim imgstat As Integer = 0
        Dim cinvw As New clsInventory_Warehouse
        Try
            imgstat = cinvw.ware_date(xwid)

        Catch ex As Exception
            errLog(Err.Description, "Pages_InvTrfEdit:GetBackground")
            Err.Clear()
        End Try


        Return imgstat

    End Function

    Protected Sub btnrcp_ServerClick(sender As Object, e As EventArgs) Handles btnrcp.ServerClick
        Dim dt, dtr As New DataTable
        If btnrcp.InnerText.Trim().Equals("Recipe Cost", StringComparison.OrdinalIgnoreCase) Then
            dtr = clsInv.getDTInvWare3(wID, custid, ddlFrom.SelectedValue, ddlWare.SelectedValue)
            btnrcp.InnerText = "Recent Cost"
        Else
            dtr = clsInv.getDTInvWare(wID, custid, ddlFrom.SelectedValue, ddlWare.SelectedValue)
            btnrcp.InnerText = "Recipe Cost"
        End If
        If Not btnedit.InnerText.Trim().Equals("Edit", StringComparison.OrdinalIgnoreCase) Then
            update()
            btnedit.InnerText = "Edit"
        End If
        dt = DirectCast(ViewState("orderW"), DataTable)
        For i As Integer = 0 To dtr.Rows.Count - 1
            dt.Rows(i).Item("cost") = dtr.Rows(i).Item("cost").ToString()
            dt.Rows(i).Item("totalcost") = dtr.Rows(i).Item("cost").ToString() * dt.Rows(i).Item("u1Del")
            dt.AcceptChanges()
        Next
        ViewState("orderW") = dt
        BindData(modulepriv)
    End Sub

    Private Sub ApplyDelModeUI()
        Try
            If String.IsNullOrEmpty(txtID.Text) Then Return
            Dim delMode As String = clsInv.getInvOrderDelMode(txtID.Text, custid(), ddlFrom.SelectedValue)
            Dim canShip As Boolean = (wDelStatus = 0 AndAlso GetBackground(wID) = 1)
            pnlDelMode.Visible = canShip OrElse delMode = "P"
            rblDelMode.SelectedValue = If(delMode = "P", "P", "F")
            Dim delList = clsInv.getInvWareDelList(txtID.Text, custid())
            Dim hasDel As Boolean = delList.Rows.Count > 0
            rblDelMode.Enabled = Not hasDel AndAlso canShip
            btnSaveDelMode.Visible = Not hasDel AndAlso canShip
            If delMode = "P" Then
                lblDelModeInfo.Text = "Partial delivery mode is active"
                pnlDelHistory.Visible = True
                btnFinish.InnerText = "Create Delivery"
                txtDel.Text = If(hasDel, "See delivery history", "Auto on save")
                BindDelHistory()
                If canShip Then ResetPartialShipmentGrid()
            Else
                lblDelModeInfo.Text = If(hasDel, "", "Full delivery mode")
                pnlDelHistory.Visible = False
                btnFinish.InnerText = "Create Delivery"
            End If
            If delMode = "P" AndAlso clsInv.getInvOrderDelMode(txtID.Text, custid(), ddlFrom.SelectedValue) = "P" Then
                Dim dsH = clsInv.getInvWareById(txtID.Text)
                If dsH.Tables(0).Rows.Count > 0 AndAlso Convert.ToInt32(dsH.Tables(0).Rows(0).Item("DelComplete")) = 1 Then
                    btnFinish.Disabled = True
                End If
            End If
        Catch ex As Exception
            errLog(Err.Description, "Pages_InvOrderWHODetail:ApplyDelModeUI")
        End Try
    End Sub

    Private Sub BindDelHistory()
        gvDelList.DataSource = clsInv.getInvWareDelList(txtID.Text, custid())
        gvDelList.DataBind()
    End Sub

    Protected Function CanCancelDeliveryNote(ByVal receiveStatus As Object) As Boolean
        Dim status As String = If(receiveStatus Is Nothing OrElse IsDBNull(receiveStatus), "", receiveStatus.ToString())
        Dim canCancelUser As Boolean = (username() = My.Settings.admin OrElse username() = "support")
        Return canCancelUser AndAlso status = "Pending Receipt"
    End Function

    Private Sub ResetPartialShipmentGrid()
        Dim dt As DataTable = DirectCast(ViewState("orderW"), DataTable)
        If dt Is Nothing Then Return
        If Not dt.Columns.Contains("u1Shipped") Then dt.Columns.Add("u1Shipped", GetType(Decimal))
        For Each row As DataRow In dt.Rows
            row("u1Shipped") = row("u1Del")
            row("u1Del") = 0
            row("totalcost") = 0
        Next
        dt.AcceptChanges()
        ViewState("orderW") = dt
        BindData(modulepriv())
    End Sub

    Protected Sub btnSaveDelMode_Click(sender As Object, e As EventArgs)
        If clsInv.setInvOrderDelMode(txtID.Text, custid(), ddlFrom.SelectedValue, rblDelMode.SelectedValue) Then
            ApplyDelModeUI()
            showAlertOnly("Delivery mode has been saved.")
        Else
            notsuccesfull("Delivery mode cannot be changed because a delivery note already exists.")
        End If
    End Sub

    Protected Sub gvDelList_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        Dim delId As String = e.CommandArgument.ToString()
        Select Case e.CommandName
            Case "PrintDel"
                mfunction.CreateCookie("woID", txtID.Text)
                mfunction.CreateCookie("woPrintDelID", delId)
                mfunction.CreateCookie("print", "DO")
                mfunction.CreateCookie("from", "~/Pages_InvOrderWHO.aspx")
                Response.Redirect("~/printprev.aspx", False)
            Case "CancelDel"
                If (username() = My.Settings.admin Or username() = "support") Then
                    Dim res = clsInv.cancelInvOrderWDel(delId, txtID.Text, custid(), ddlWare.SelectedValue, ddlWare.SelectedItem.Text, ddlFrom.SelectedValue)
                    If res = "OK" Then
                        messagesuccesfull("Delivery note has been cancelled.")
                        Dim dt = clsInv.getDTInvWare(wID, custid(), ddlFrom.SelectedValue, ddlWare.SelectedValue)
                        ViewState("orderW") = dt
                        ApplyDelModeUI()
                        BindData(modulepriv())
                    Else
                        notsuccesfull("Failed to cancel the delivery note.")
                    End If
                Else
                    notsuccesfull("Only admin/support users can cancel a delivery note.")
                End If
        End Select
    End Sub
End Class
