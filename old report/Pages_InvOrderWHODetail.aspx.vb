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
                    If wID = "" Then

                        txtID.Text = ""
                        txtID.ReadOnly = True
                        Dim dt, dt2 As New DataTable
                        'dt = WService.getDTInvWare(wID, cust, 0, 0)
                        dt = clsInv.getDTInvWare(wID, custid, 0, 0)

                        ViewState("orderW") = dt
                        ddlFrom.Enabled = False

                        ddlWare.Enabled = False
                        BindData(modulepriv)
                        Label3.Text = "Create Date"
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

                        If wDelStatus = 1 Then
                            ' btnFinish.Enabled = False
                            btnSetAll.Disabled = True
                            DgvTransfer.Columns(5).Visible = False
                            'If Not (modulepriv() = mHelper.Modulepriv.moduleuser) And wDel.Date > Now.Date.AddDays(-7) Then

                            If (username() = My.Settings.admin Or username() = "support") And wDel.Date > Now.Date.AddDays(-7) Then
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


                        'txtexpD.Enabled = False

                        Dim dt, dt2 As New DataTable
                        dt = clsInv.getDTInvWare(wID, custid, ddlFrom.SelectedValue, ddlWare.SelectedValue)
                        'WService.getDTInvWare(wID, cust, ddlFrom.SelectedValue, ddlWare.SelectedValue)
                        ViewState("orderW") = dt
                        BindData(modulepriv)

                    End If


                End If
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
            errLog(Err.Description, "Pages_ReceipeEdit:Page_Load")
            Err.Clear()
        End Try
    End Sub
    Sub loadFrom()
        Dim ds As New DataSet
        ds = clsInv.getIccOnly(custid, username, modulepriv)
        'WService.getIccOnly(cust, username, ModPrv)

        ddlFrom.DataSource = ds
        ddlFrom.DataValueField = "Id"
        ddlFrom.DataTextField = "Name"
        ddlFrom.DataBind()

        If ddlFrom.Items.Count > 0 Then
            ddlFrom.SelectedIndex = 0
        End If
    End Sub

    Sub loadwarehouse()
        Dim ds As New DataSet
        ds = clsInv.getIWareOnly(custid)
        'WService.getIWareOnly(cust)
        ddlWare.DataSource = ds
        ddlWare.DataValueField = "Id"
        ddlWare.DataTextField = "Name"
        ddlWare.DataBind()

        If ddlWare.Items.Count > 0 Then
            ddlWare.SelectedIndex = 0
        End If
    End Sub
    Protected Sub BindData(ByVal modpriv As Integer)

        If custid() = 0 Then
            Response.Redirect("~/Default.aspx")
        Else
            DgvTransfer.DataSource = DirectCast(ViewState("orderW"), DataTable)
            If modpriv And mHelper.Modulepriv.moduleuser = mHelper.Modulepriv.moduleuser Then
            Else
                DgvTransfer.Columns(8).Visible = True
            End If
            DgvTransfer.DataBind()
        End If

    End Sub
    Sub Bindingstk1()
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
    End Sub
    Sub bindingstk()

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

            For Each row As DataRow In dt.Rows
                If row.Item("U1Del") = 0 Then
                    msg2 = msg2 + row.Item("Stk_name") + " DEL QTY = 0 " & ";"
                End If
                If row.Item("cost") = 0 Then
                    msg2 = msg2 + row.Item("Stk_name") + " DEL PRICE = 0 " & "\n"

                End If
            Next row

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

            If c_cmd.ExecuteScalar("select count(custid) from cld_inv_closing where custid = " & custid() & " and invdate = '" & Date.Now.Date.ToString("yyyy-MM-dd") & "' and  invflag = 0 ", 0) = 1 Then
                FormsAuthentication.SetAuthCookie(username, False)
                messagesuccesfull("Closing process,Please login again.", False)
                Exit Sub
            Else
                clsInv.insertInvOrderW(txtID.Text, custid(), 0, 2, ddlWare.SelectedValue, ddlWare.SelectedItem.ToString, ddlFrom.SelectedValue, ddlFrom.SelectedItem.ToString, username, username, CDate(txtexpD2.Text), dt, txtDel.Text, txtNotes.Text, xcancel)

            End If


            If msg2 <> "" Then
                msg2 = msg2 + " are you sure? " & "\n"
                messagesuccesfull(msg2)

            Else
                Response.Redirect("~/Pages_InvOrderWHO.aspx")
            End If

        Catch ex As Exception

            'MsgBox(Err.Description, MsgBoxStyle.Information)
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

    End Sub
    Protected Sub DgvTransfer_RowUpdating(ByVal sender As Object, ByVal e As GridViewUpdateEventArgs)
        'Del 1
        Dim txtUnit1 As TextBox = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("txtEditUnit1Del"), TextBox)
        'cost
        Dim txtCost As TextBox = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("txtCost"), TextBox)
        'stock
        Dim lblStock As Label = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("lblEditStock"), Label)

        'Del2
        Dim lblUnit2 As Label = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("lblEditUnit1Del2"), Label)
        Dim txtUnit2 As TextBox = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("txtChange"), TextBox)

        Dim dt As DataTable = DirectCast(ViewState("orderW"), DataTable)

        Try
            If IsNumeric(txtUnit1.Text) = False Then
                notsuccesfull("Del qty must be number")
                Exit Sub
            End If
            If IsNumeric(txtCost.Text) = False Then
                notsuccesfull("Price must be number")
                Exit Sub
            End If
            lblError.Text = ""

            If wDelStatus = 0 Then
                'order atau normal

                If CDec(lblStock.Text) < CDec(txtUnit1.Text) And try1 = 0 Then
                    'notsuccesfull("are sure with Delivery Qty and Price ?")
                    notsuccesfull("Delivery Qty cannot more than Stock Qty")
                    If CDec(lblStock.Text) < 0 Then
                        dt.Rows(e.RowIndex)("U1Del") = 0
                    Else
                        dt.Rows(e.RowIndex)("U1Del") = CDec(lblStock.Text)
                    End If
                    try1 = try1 + 1
                Else
                    If (CDec(txtUnit1.Text) = 0 Or txtCost.Text = 0) And try2 = 0 Then
                        notsuccesfull("are sure with Delivery Qty and Price ?")
                        try2 = try2 + 1
                        'GoTo dtchange
                    End If
                    dt.Rows(e.RowIndex)("U1Del") = txtUnit1.Text





                    'try1 = try1 + 1
                    'lblError.Text = "Delivery Qty cannot more than Stock Qty"

                End If
                dt.Rows(e.RowIndex)("cost") = txtCost.Text
dtchange:
                dt.AcceptChanges()

                DgvTransfer.EditIndex = -1
                BindData(modulepriv)
            Else


                Dim a As Decimal = CDec(txtUnit2.Text) + CDec(txtUnit1.Text)


                If CDec(lblStock.Text) < CDec(txtUnit2.Text) And try1 = 0 Then
                    'notsuccesfull("are sure with Delivery Qty and Price ?")
                    notsuccesfull("Delivery Qty cannot more than Stock Qty")
                    If CDec(lblStock.Text) < 0 Then
                        dt.Rows(e.RowIndex)("U1Del") = 0
                    Else
                        'dt.Rows(e.RowIndex)("U1Del") = CDec(lblStock.Text)
                        txtUnit2.Text = CDec(lblStock.Text)
                        dt.Rows(e.RowIndex)("U1Del") = CDec(txtUnit1.Text) + CDec(txtUnit2.Text)
                    End If


                    try1 = try1 + 1


                Else
                    If ((CDec(txtUnit2.Text) + CDec(txtUnit1.Text)) = 0 Or txtCost.Text = 0) And try2 = 0 Then
                        notsuccesfull("are sure with Delivery Qty and Price ?")
                        try2 = try2 + 1
                        'GoTo dtchange2
                    End If
                    dt.Rows(e.RowIndex)("U1Del") = CDec(txtUnit1.Text) + CDec(txtUnit2.Text)





                End If
                dt.Rows(e.RowIndex)("cost") = txtCost.Text
dtchange2:
                dt.AcceptChanges()

                DgvTransfer.EditIndex = -1
                BindData(modulepriv)
            End If
        Catch ex As Exception

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
    Protected Sub DgvTransfer_RowCancelingEdit(ByVal sender As Object, ByVal e As GridViewCancelEditEventArgs)
        lblError.Text = ""
        DgvTransfer.EditIndex = -1
        BindData(modulepriv)
    End Sub

    Protected Sub DgvTransfer_RowEditing(ByVal sender As Object, ByVal e As GridViewEditEventArgs)
        DgvTransfer.EditIndex = e.NewEditIndex
        BindData(modulepriv)
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

                        Dim ds As DataSet = clsStk.SelectDataStk_wOther((DirectCast(item.FindControl("lblID"), Label).Text), ddlFrom.SelectedValue, custid, ddlWare.SelectedValue, CDate(Now()))


                        If ds.Tables(0).Rows.Count > 0 Then
                            Dim u1 As String = ds.Tables(0).Rows(0).Item("Unit1").ToString
                            Dim factor1 As String = ds.Tables(0).Rows(0).Item("factor1").ToString
                            Dim dCost As Decimal = ds.Tables(0).Rows(0).Item("coststk").ToString

                            Dim dt As DataTable = DirectCast(ViewState("orderW"), DataTable)

                            Debug.Print((DirectCast(item.FindControl("lblID"), Label).Text))
                            Debug.Print((DirectCast(item.FindControl("lblName"), Label).Text))
                            Debug.Print(u1.ToString)
                            Debug.Print(factor1.ToString)

                            dt.Rows.Add("", custid, 0, (DirectCast(item.FindControl("lblID"), Label).Text), (DirectCast(item.FindControl("lblName"), Label).Text), 0, u1, 0, 0, dCost, factor1)

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
        Try
            insert(2)
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub btnBack_Click(sender As Object, e As EventArgs) 'Handles btnBack.ServerClick
        Response.Redirect("~/Pages_InvOrderWHO.aspx", False)
    End Sub

    Protected Sub btnSetAll_Click(sender As Object, e As EventArgs) Handles btnSetAll.ServerClick
        Dim dt As DataTable = DirectCast(ViewState("orderW"), DataTable)

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

            ' row.Item("u1Del") = row.Item("u1")
            dt.AcceptChanges()
        Next row

        BindData(modulepriv)
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
            If clsInv.get_HOstatus(txtID.Text, ddlFrom.SelectedValue, custid) = 2 Then
                insert(2, 1)
            End If
        Catch ex As Exception

        End Try

    End Sub
End Class