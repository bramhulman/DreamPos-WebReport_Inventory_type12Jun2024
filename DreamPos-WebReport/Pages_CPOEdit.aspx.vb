Imports System.Data
Imports System.Drawing
Imports ClosedXML.Excel
Imports System.IO
Imports System.Net
Imports System.Text
Imports System.Threading
Imports Org.BouncyCastle.Crypto.Agreement

Public Class Pages_CPOEdit
    Inherits System.Web.UI.Page
    Private sNameuser As String
    Private Userid As Integer
    Private icustid As Integer
    Private imodul As Integer
    Private iModPriv As Integer
    Private irptpriv As Integer
    Private irptf As Integer
    Private mfunction As New ModFunction
    Private POnumber As String
    Private OrderCust As Integer
    Private olid As Integer
    Private Payno As Integer

    Private iflag As Integer
    Private dTransdate As Date
    Private dbusidate As Date
    'Private dexdeldate As Date
    Private acc As String
    Private bacc As Boolean = False
    Private dataParam As New ClsParamRpt
    Private clsreport As New ClsReports
    Private clsOut As New ClsOutlet
    Private cInv As New clsInventory
    Private cSupp As New clsInventory_Supplier
    Private CStk As New Cls_IStk
    Private CPO As New Cls_PO
    Dim dsrpt As New DataSet

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
    Public Function InvPriv() As Integer
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        If cookie Is Nothing Then
            Return -1
        Else
            Return cookie("invpriv")
        End If

    End Function
    <Serializable>
    Public Class Cc_Detail 'Class untuk ListCc
        Public Property CcName As String = ""
        Public Property Status As Boolean = False
        'Public Property CcID As Integer
    End Class
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
        'Dim notes As String
        Try
            POnumber = Request.Cookies("Number").Value
            olid = Request.Cookies("olid").Value
            OrderCust = Request.Cookies("Ordercust").Value
            'Payno = Request.Cookies("Payno").Value
            iflag = Request.Cookies("flag").Value
            dTransdate = Request.Cookies("transdate").Value.ToString
            dTransdate = Request.Cookies("busidate").Value.ToString


            If IsDate(Request.Cookies("transdate").Value) Then
                dTransdate = (Request.Cookies("transdate").Value)
                txttransdate.Text = Format(dTransdate, "dd MMM yyyy")

                transactiondate.Visible = True
                txttransdate.Enabled = False

            Else
                txttransdate.Text = Format(Now, "dd MMM yyyy")

                txtbusidate.Text = Format(Now, "dd MMM yyyy")
                transactiondate.Visible = True
                txttransdate.Enabled = False
            End If



            If Request.Cookies("UserLogin_inv") IsNot Nothing Then
                sNameuser = username()
                icustid = custid()
                imodul = modul()
                iModPriv = modulepriv()
                irptpriv = rptpriv()
                irptf = rptf()

                If IsPostBack = False Then
                    'txtcreatedate.Text = Now.Date

                    loadoutlet()
                    'loadPayment()
                    loadCustomer()
                    bindingstk1()
                    btnedit.InnerText = "edit"
                    If POnumber = "" Then

                        ddlCustomer.SelectedValue = 0
                        Dim dt, dt2 As New DataTable
                        dt = cInv.getDTPreOrder(POnumber, custid)
                        ViewState("InvPO") = dt
                        btnusage.Visible = False

                    Else

                        If iflag = 2 Then
                                btnSave.Attributes.Add("style", "display:none")
                                btnshow.Attributes.Add("style", "display:none")
                                'expdel.Attributes.Add("style", "display:none")
                            End If


                            ddlCustomer.SelectedValue = OrderCust
                        ddlOutlet.SelectedValue = olid
                        'DdlPayment.SelectedValue = Payno

                        txtLinkid.Text = POnumber
                        btnusage.Visible = True
                        'txtexdel.Text = dexdeldate.ToString("dd MMM yyyy")
                        If IsDate(Request.Cookies("Busidate").Value) Then
                            dbusidate = Request.Cookies("Busidate").Value
                            txtbusidate.Text = Format(dbusidate, "dd MMM yyyy")

                        Else
                            txtbusidate.Text = Format(Now, "dd MMM yyyy")

                        End If
                        If IsDate(Request.Cookies("transdate").Value) Then
                            dTransdate = Request.Cookies("transdate").Value
                            txttransdate.Text = Format(dTransdate, "dd MMM yyyy")

                        Else
                            txttransdate.Text = Format(Now, "dd MMM yyyy")

                        End If
                        Dim dt, dt2 As New DataTable

                        dt = cInv.getDTPreOrder(POnumber, custid) 'WService.getDTInvRcv(Mtid, cust)

                        ViewState("InvPO") = dt
                        If dt.Rows.Count > 0 Then
                            btnSave.InnerText = "update"
                        End If

                        BindData()
                        bindingstk1()
                        'Dim xinvdate As Date
                        'If drcvdate.Date < dCreate.Date Then
                        '    xinvdate = drcvdate.Date
                        'Else
                        '    xinvdate = dCreate.Date
                        'End If
                        If iflag = 2 And dTransdate.Date < Now.Date Then
                            btnshow.Disabled = True
                            btnSave.Disabled = True
                            'btnFinish.Disabled = True
                            DgvTransfer.Columns(12).Visible = False
                            'expdel.Visible = False


                            If Not (modulepriv() = usrModulepriv.moduleuser) And dTransdate.Date > Now.Date.AddDays(-20) And (username() = My.Settings.admin Or username() = "support") Then

                                btncancel.Visible = True
                            End If

                        Else

                            btnshow.Disabled = False
                                btnSave.Disabled = False
                            'btnFinish.Disabled = False
                            'btnFinish.Visible = True
                            Businessdate.Visible = True


                        End If

                    End If

                    check_validation()
                    notsuccesfull("Silakan order di bawah " & My.Settings.limitorder & " item, pisah jika lebih !!")

                End If
                Dim script As String = "$(document).ready(function () { $('[id*=btnShow]').click(); });"

                ClientScript.RegisterStartupScript(Me.GetType, "load", script, True)
                txtSearch.Attributes.Add("onkeyup", Me.Page.ClientScript.GetPostBackEventReference(txtSearch, ""))
                'Checkbox_allstore.Attributes.Add("onclick", Me.Page.ClientScript.GetPostBackEventReference(Checkbox_allstore, ""))

                ScriptManager.GetCurrent(Me).RegisterPostBackControl(RadButton1)

                txtSearch.Attributes.Add("onfocus", "javascript: this.setSelectionRange(this.value.length,this.value.length);")
                'txtNotes.Attributes.Add("maxlength", txtNotes.MaxLength.ToString)
                'txtDO.Attributes.Add("maxlength", txtDO.MaxLength.ToString)
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
    Protected Sub BindData()

        Try
            If custid() = 0 Then
                Response.Redirect("~/Default.aspx", False)
            Else
                'Dim totalcost As Double = 0

                'For Each dr As DataRow In DirectCast(ViewState("InvPO"), DataTable).Rows
                '    totalcost += Convert.ToDouble(dr("Subtotal"))
                'Next

                'DgvTransfer.Columns(5).FooterText = totalcost.ToString("#,##0.00")
                DgvTransfer.DataSource = DirectCast(ViewState("InvPO"), DataTable)
                DgvTransfer.DataBind()
                'DgvTransfer.ShowFooter = True

                If DgvTransfer.Rows.Count > 0 Then
                    DgvTransfer.Columns(4).Visible = True
                    btnedit.Visible = True
                    'If POnumber <> "" Then
                    '    If GetBackground(POnumber) = 0 Then
                    '        DgvTransfer.Columns(6).Visible = False
                    '        btnedit.Visible = False

                    '    End If
                    'End If
                    btnExportDetail.Visible = True
                Else
                    DgvTransfer.Columns(4).Visible = False
                    btnedit.Visible = False
                End If
            End If
        Catch ex As Exception
            errLog(Err.Description, "Pages_InvRcvEdit:BindData")
            Err.Clear()
        End Try

    End Sub

    Sub bindingstk1()

        Try
            If custid() = 0 Then
                Response.Redirect("~/Default.aspx", False)
            Else
                Dim dt, dt2 As New DataTable
                dt = DirectCast(ViewState("InvPO"), DataTable)
                'dt2 = cInv.getDtStk(dt, custid, txtSearch.Text, modulepriv)
                dt2 = CPO.Selectdt_Item_all(dt, custid, txtSearch.Text, modulepriv, ddlOutlet.SelectedValue)
                'WService.getDtStk(dt, cust, txtSearch.Text, ModPrv)
                ViewState("InvStk") = dt2
                LsvStk.DataSource = DirectCast(ViewState("InvStk"), DataTable)
                LsvStk.DataBind()
            End If
        Catch ex As Exception
            errLog(Err.Description, "Pages_InvPOEdit:bindingstk")
            Err.Clear()
        End Try


    End Sub
    Sub bindingstk()

        If custid() = 0 Then
            Response.Redirect("~/Default.aspx", False)
        Else
            Dim dt, dt2 As New DataTable
            dt = DirectCast(ViewState("InvPO"), DataTable)

            'dt2 = DirectCast(ViewState("ListItem"), DataTable)

            dt2 = DirectCast(ViewState("InvStk"), DataTable)

            If dt2.Rows.Count = 0 Then
                dt2 = CPO.Selectdt_Item_all(dt, custid, txtSearch.Text, modulepriv, ddlOutlet.SelectedValue)

                ViewState("InvStk") = dt2
            Else



                dt2.DefaultView.RowFilter = "itemname like '%" & txtSearch.Text & "%'"
                ViewState("InvStk") = dt2
            End If

            If dt2.Rows.Count > 0 Then
                LsvStk.Items.Clear()
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


        If cookie Is Nothing Then
            Return ""
        Else
            Return cookie("userName")
        End If

    End Function

    'Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAdd.Click
    '    Add()
    'End Sub

    'Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.ServerClick
    '    bindingstk()
    'End Sub
    Protected Sub RadButton1_ServerClick(sender As Object, e As EventArgs) Handles RadButton1.ServerClick
        Try
            If ddlOutlet.SelectedValue <> 0 Then
                Add()
                'Session("xpayment") = DdlPayment.SelectedValue
                Session("xOutlet") = ddlOutlet.SelectedValue
            Else
                notsuccesfull("you must choose Cost Centre first")
            End If
            'DgvTransfer.DataBind()
        Catch ex As Exception
            errLog(Err.Description, "Pages_InvPOEdit:RadButton1_ServerClick")
            Err.Clear()
        End Try
    End Sub
    Sub Add()
        Dim dt As New DataTable
        Try
            Dim i As Integer = 0
            If DirectCast(ViewState("InvPO"), DataTable).Rows.Count >= My.Settings.limitorder Then
                notsuccesfull("Silakan order di bawah " & My.Settings.limitorder & " item, pisah jika lebih !!")
                GoTo ef
            End If
            txtSearch.Text = ""
            BindCheckItem()
            bindingstk()
            CheckedItem()



            For Each item As ListViewItem In LsvStk.Items.ToList
                Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox1"), CheckBox)

                If ck.Checked Then
                    Try
                        Dim citem As New clsDataItem
                        Dim ds As DataSet = citem.SelectDataset_ItemEdit((DirectCast(item.FindControl("lblID"), Label).Text), custid)
                        If ds.Tables(0).Rows.Count > 0 Then
                            Dim itemno As String = ds.Tables(0).Rows(0).Item("itemno").ToString
                            Dim itemname As String = ds.Tables(0).Rows(0).Item("itemname").ToString
                            'dim qty As String = ds.Tables(0).Rows(0).Item("qty").ToString
                            Dim price As String = ds.Tables(0).Rows(0).Item("price").ToString
                            Dim subtotal As Double = 1 * price
                            'Dim xdate As Date = CDate(Now)
                            Dim cost As String = ds.Tables(0).Rows(0).Item("cost").ToString
                            dt = DirectCast(ViewState("InvPO"), DataTable)


                            dt.Rows.Add("", itemno, itemname, 0, ddlOutlet.SelectedValue, custid, 1, 0, "", "", DateTime.Now)
                            ViewState("InvPO") = dt
                            Dim dtStk As DataTable = DirectCast(ViewState("InvStk"), DataTable)
                            dtStk.Rows(i).Delete()
                            dtStk.AcceptChanges()
                            i = i - 1
                            btnedit.Visible = True
                        Else
                            btnedit.Visible = False
                        End If

                        BindData()
                        txtSearch.Text = ""
                        bindingstk()
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
            'update()
            btnedit.InnerText = "edit"
        Catch ex As Exception
            'MsgBox(ex.Message)
            errLog(Err.Description, "Pages_InvRcvEdit:Add")
            Err.Clear()
        End Try

    End Sub



    Sub delete(ByVal id As Integer)
        Try
            'WService.DeleteInvRcv(id, cust, username)
            cInv.DeleteInvRcvDetail(id, custid, username)
        Catch ex As Exception
            errLog(Err.Description, "Pages_InvRcvEdit:Delete")
            Err.Clear()
        End Try
    End Sub
    Sub deleteitem(ByVal id As Integer, ByVal stkid As Integer)
        Try
            'WService.DeleteInvRcv(id, cust, username)
            cInv.DeleteInvRcvitem(id, stkid, custid, username)
        Catch ex As Exception
            errLog(Err.Description, "Pages_InvRcvEdit:deleteitem")
            Err.Clear()
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
            insertLogModule(username, "InvRcpdtl", "Delete " & id, Custid)

        Catch ex As Exception

            errLog(Err.Description, "Pages_InvRcvEdit:DeleteInvrcpdtl")
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

    Sub loadoutlet()

        Dim ds As New DataSet
        Try
            ds = cInv.getInvcc(custid, username, modulepriv, 1)
            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 Then
                ddlOutlet.DataSource = ds
            Else
                ddlOutlet.DataSource = Nothing
            End If
            ddlOutlet.DataValueField = "Olid"
            ddlOutlet.DataTextField = "OutletName"
            ddlOutlet.DataBind()

            If ddlOutlet.Items.Count > 0 Then
                ddlOutlet.SelectedIndex = 0
            End If
        Catch ex As Exception
            errLog(Err.Description, "Pages_CPOEdit:loadoutlet")
            Err.Clear()
        End Try

    End Sub
    Sub loadCustomer()

        Dim ds As New DataSet
        Try

            ds = cInv.getCustomerPO(custid)



            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 Then
                ddlCustomer.DataSource = ds
            Else
                ddlCustomer.DataSource = Nothing
            End If
            ddlCustomer.DataValueField = "Id"
            ddlCustomer.DataTextField = "Name"
            ddlCustomer.DataBind()

            If ddlCustomer.Items.Count > 0 Then
                ddlCustomer.SelectedIndex = 0
            End If
        Catch ex As Exception
            errLog(Err.Description, "Pages_InvRcvEdit:loadCustomer")
            Err.Clear()
        End Try

    End Sub
    'Sub loadPayment()

    '    Dim ds As New DataSet
    '    Try

    '        ds = cInv.getPayment(custid)



    '            DdlPayment.DataSource = ds
    '        DdlPayment.DataValueField = "number"
    '        DdlPayment.DataTextField = "Name"
    '        DdlPayment.DataBind()

    '        If DdlPayment.Items.Count > 0 Then
    '            DdlPayment.SelectedIndex = 0
    '        End If
    '    Catch ex As Exception
    '        errLog(Err.Description, "Pages_InvRcvEdit:loadPayment")
    '        Err.Clear()
    '    End Try

    'End Sub
    Sub insert(ByVal stat As Integer, ByVal Optional xcancel As Boolean = False, ByVal Optional updaterow As Boolean = False) 'As Boolean
        Dim hasil As Boolean = False
        Dim msg As String = ""
        Dim newLine As String = "\n"
        ' Dim useddate As Date
        Dim c_cmd As New clsDbo("")
        Dim smno As String = ""
        Try

            Dim dt As DataTable = DirectCast(ViewState("InvPO"), DataTable)

            If ddlOutlet.SelectedValue = 0 Or ddlOutlet.SelectedItem.Text = "" Then
                msg = msg + "Cost Centre Must be Choice " & newLine
            End If

            'If DdlPayment.SelectedValue = 0 Or DdlPayment.SelectedItem.Text = "" Then
            '    msg = msg + "Supplier Must be Choice" & newLine
            'End If
            'For Each row As DataRow In dt.Rows
            '    If row.Item("Subtotal") = 0 Or row.Item("Price") = 0 Then
            '        'msg = msg + row.Item("Stk_name") + " stock Or Cost Not Fill " & newLine
            '        'notsuccesfull(msg)
            '    End If

            'Next row

            If txttransdate.Text = "" Then
                txttransdate.Text = Date.Now.Date.ToString("yyyy-MM-dd 00:00:00")
            End If
            dTransdate = CDate(txttransdate.Text)

            If txtbusidate.Text = "" Then
                txtbusidate.Text = Date.Now.Date.ToString("yyyy-MM-dd 00:00:00")
                dbusidate = CDate(txtbusidate.Text)
            Else
                dbusidate = CDate(txtbusidate.Text)
            End If


            If c_cmd.ExecuteScalar("select count(custid) from cld_inv_closing where custid = " & custid() & " and invdate = '" & Date.Now.Date.ToString("yyyy-MM-dd") & "' and  invflag = 0 ", 0) = 1 Then
                FormsAuthentication.SetAuthCookie(username, False)
                notsuccesfull("Closing process,Please login again.")
                Exit Sub
            ElseIf msg <> "" Then
                notsuccesfull(msg)
                Exit Sub
            Else
                Dim olnumber As String = ""
                olnumber = c_cmd.ExecuteScalar("SELECT kode FROM cld_outlet WHERE id=" & ddlOutlet.SelectedValue & " AND custid = " & custid() & "", 0)

                smno = CPO.insertPreOrder(txtLinkid.Text, ddlOutlet.SelectedValue, olnumber, ddlOutlet.SelectedItem.ToString, dt, ddlCustomer.SelectedValue, ddlCustomer.SelectedItem.Text, stat, username, custid, CDate(txtbusidate.Text), "", 1, "", 1, 0, 10)
                If c_cmd.ExecuteScalar("SELECT COUNT(custid) FROM cld_inv_transusage_cpo WHERE smno='" & smno & "' AND custid=" & custid() & " AND Outletid=" & olid & "", 0) > 0 Then
                    c_cmd.executeNonQuery("DELETE FROM cld_inv_transusage_cpo WHERE smno='" & smno & "' AND custid=" & custid() & " AND Outletid=" & olid & "")
                End If
                CPO.Calculasi_inventorypo(CDate(txtbusidate.Text), custid())

                'CPO.insertInvPO(Mtid, DdlTo.SelectedValue, ddlItem.SelectedValue, ddlItem.SelectedItem.Text, txtUnit1.Text,
                '                  txtUnit2.Text, CDec(txtLosF.Text), CDec(txtTotalStk.Text), lblUnit1.Text, lblUnit2.Text, username, username, txtstkNo.Text, DdlTo.SelectedItem.Text, username, custid, inv_date, ddlrcp.SelectedValue)

            End If
            If updaterow = True Then

            Else
                'Response.Redirect("~/Pages_CPO.aspx", False)
            End If
            btnusage.Visible = True
            txtLinkid.Text = smno

        Catch ex As Exception
            errLog(Err.Description, "Pages_InvRcvEdit:Insert")
            Err.Clear()
            'MsgBox(Err.Description, MsgBoxStyle.Information)
            Err.Clear()
        End Try
        'Return hasil
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
        Try
            '  ViewState("InvRcvEdit") = 1

            Dim lblID As Label = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("lblID"), Label)
            Dim lblItemNo As Label = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("lblItemNo"), Label)
            Dim lblItemname As Label = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("lblName"), Label)
            Dim lblQty As Label = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("lblQty"), Label)
            Dim txtEditQty As TextBox = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("txtEditQty"), TextBox)

            Dim lblPrice As Label = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("lblPrice"), Label)
            Dim lblttl_Cost As Label = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("lblttlCost"), Label)
            Dim txtEditPrice As TextBox = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("txtEditPrice"), TextBox)
            Dim dt As DataTable = DirectCast(ViewState("InvPO"), DataTable)
            Dim dtstk As DataTable = DirectCast(ViewState("InvStk"), DataTable)
            If lblID.Text = 0 Then
                dtstk.Rows.Add(lblItemNo.Text, lblItemname.Text)
                ViewState("InvStk") = dtstk
                dt.Rows(e.RowIndex).Delete()
                dt.AcceptChanges()
            Else
                dtstk.Rows.Add(lblItemNo.Text, lblItemname.Text)
                ViewState("InvStk") = dtstk
                Dim confirmValue As String = Request.Form("confirm_value")
                If confirmValue = "Yes" Then

                    Dim id As Integer = lblID.Text
                    deleteitem(id, lblItemNo.Text)
                    'delete(id)

                Else
                    Exit Sub
                End If
                dt.Rows(e.RowIndex).Delete()
                dt.AcceptChanges()
            End If
            BindData()
            bindingstk()


        Catch ex As Exception
            errLog(Err.Description, "Pages_InvRcvEdit:DgvTransfer_RowDeleting")
            Err.Clear()
        End Try

    End Sub
    Protected Sub DgvTransfer_RowUpdating(ByVal sender As Object, ByVal e As GridViewUpdateEventArgs)

        Try
            ' Dim maxrowup As Integer = 0
            '  ViewState("InvRcvEdit") = 1
            Dim lblItemNo As Label = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("lblItemNo"), Label)
            Dim lblName As Label = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("lblName"), Label)
            Dim lblQty As Label = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("lblQty"), Label)
            Dim txtEditQty As TextBox = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("txtEditQty"), TextBox)

            Dim lblPrice As Label = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("lblPrice"), Label)
            Dim lblttl_Cost As Label = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("lblttlCost"), Label)
            Dim txtEditPrice As TextBox = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("txtEditPrice"), TextBox)
            'Dim txtUnit1 As TextBox = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("txtEditQty"), TextBox)
            If IsNumeric(txtEditQty.Text) = False Then
                notsuccesfull("Qty must be number")
                Exit Sub
            End If


            Dim clsI_stk As New clsInventory_stk

            If IsNumeric(txtEditQty.Text) = False Then
                txtEditQty.Text = 0
            End If


            If IsNumeric(txtEditPrice.Text) = False Then
                txtEditQty.Text = 0
            End If




            'tambahan erik 21 sept 2020, buat jagain nilai tidak tll besar
            If txtEditQty.Text = 0 Then
                notsuccesfull("Please Input Price")
                txtEditQty.Text = 0

                Exit Sub
            End If

            If Val(txtEditQty.Text) >= 999999999 Then
                notsuccesfull("Qty must less than 999999999")
                Exit Sub
            End If

            If Val(txtEditQty.Text) >= 999999999 Then
                notsuccesfull("Price Must less than 999999999")
                Exit Sub
            End If




            Dim lblUnit1 As Label = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("lblEditEmpID"), Label)

            Dim dt As DataTable = DirectCast(ViewState("InvPO"), DataTable)

            total = (Val(txtEditQty.Text) * Val(txtEditQty.Text))

            If Val(total) >= 999999999 Then
                notsuccesfull("Subtotal must less than 999999999")
                Exit Sub
            End If


            dt.Rows(e.RowIndex)("ItemNo") = lblItemNo.Text

            dt.Rows(e.RowIndex)("ItemName") = lblName.Text

            dt.Rows(e.RowIndex)("Qty") = txtEditQty.Text
            dt.Rows(e.RowIndex)("Price") = lblPrice.Text
            Dim ttlcost As Decimal = txtEditQty.Text * lblPrice.Text
            dt.Rows(e.RowIndex)("Subtotal") = total
            dt.Rows(e.RowIndex)("Outletid") = ddlOutlet.SelectedValue
            dt.Rows(e.RowIndex)("CustID") = custid()


            dt.AcceptChanges()
            bindingstk()
            BindData()

        Catch ex As Exception
            errLog(Err.Description, "InvRcvEdit:DgvTransfer_RowUpdating")
            Err.Clear()
        End Try

    End Sub
    Protected Sub DgvTransfer_RowCancelingEdit(ByVal sender As Object, ByVal e As GridViewCancelEditEventArgs)
        DgvTransfer.EditIndex = -1
        BindData()
    End Sub

    Protected Sub DgvTransfer_RowEditing(ByVal sender As Object, ByVal e As GridViewEditEventArgs)
        DgvTransfer.EditIndex = e.NewEditIndex
        BindData()
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



    'END : Store




    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSave.ServerClick
        Try
            Dim msg As String = ""
            Dim total As Decimal = 0
            Dim newLine As String = "\n"
            If iflag = 2 Then

                notsuccesfull("this order was complete")
                Exit Sub
            Else

                For Each row As GridViewRow In DgvTransfer.Rows

                    If row.RowType = DataControlRowType.DataRow Then

                        If row.RowType = DataControlRowType.DataRow Then
                            Dim lblItemNo As Label = (TryCast(row.FindControl("lblItemNo"), Label))
                            Dim lblName As Label = (TryCast(row.FindControl("lblName"), Label))
                            Dim lblQty As Label = (TryCast(row.FindControl("lblQty"), Label))
                            Dim txtEditQty As TextBox = (TryCast(row.FindControl("txtEditQty"), TextBox))

                            Dim lblPrice As Label = (TryCast(row.FindControl("lblPrice"), Label))
                            Dim lblttl_Cost As Label = (TryCast(row.FindControl("lblttlCost"), Label))
                            Dim txtEditPrice As TextBox = (TryCast(row.FindControl("txtEditPrice"), TextBox))
                            lblQty.Visible = True
                            txtEditQty.Visible = False
                            Label1.Visible = True

                            'lblPrice.Visible = True
                            'txtEditPrice.Visible = False
                            lblQty.Text = txtEditQty.Text


                            lblQty.Text = txtEditQty.Text
                            'If IsNumeric(txtEditQty.Text) = False Then
                            '    If IsNumeric(txtEditQty.Text) = False Then
                            '        msg = msg + " row " + row.RowIndex.ToString + " Qty must be number" & newLine
                            '    End If
                            'End If

                            'lblPrice.Text = txtEditPrice.Text
                            'If IsNumeric(txtEditPrice.Text) = False Then
                            '    msg = msg + " row " + row.RowIndex.ToString + " Price must be number" & newLine
                            '    txtEditPrice.Text = 0
                            'End If

                            'If txtEditPrice.Text = 0 Then

                            '    msg = msg + " row " + row.RowIndex.ToString + " Please Input Price" &
                            '    txtEditPrice.Text = 0
                            '    txtEditQty.Text = 0

                            'End If

                            If Val(txtEditQty.Text) >= 999999999 Then

                                msg = msg + " row " + row.RowIndex.ToString + " Qty must less than 999999" & newLine
                            End If

                            'If Val(txtEditPrice.Text) >= 999999999 Then
                            '    msg = msg + " row " + row.RowIndex.ToString + " Price must less than 999999" & newLine
                            'End If

                            'If Val(total) >= 999999999 Then

                            '    msg = msg + " row " + row.RowIndex.ToString + " Total must less than 999999" & newLine

                            'End If


                            Dim lblUnit_1 As Label = (TryCast(row.FindControl("lblEditEmpID"), Label))

                            Dim dt As DataTable = DirectCast(ViewState("InvPO"), DataTable)
                            Dim clsI_stk As New clsInventory_stk

                            ' total = (Val(txtEditQty.Text) * Val(txtEditPrice.Text))


                            If msg <> "" Then
                                notsuccesfull(msg)
                                Exit Sub
                            End If

                            Dim lbl_stkid As Label = (TryCast(row.FindControl("lblStkID"), Label))

                            dt.Rows(row.RowIndex)("Smno") = txtLinkid.Text
                            dt.Rows(row.RowIndex)("ItemNo") = lblItemNo.Text

                            dt.Rows(row.RowIndex)("ItemName") = lblName.Text
                            If txtEditQty.Text = "" Then
                                txtEditQty.Text = 0
                            End If
                            dt.Rows(row.RowIndex)("Qty") = txtEditQty.Text
                            'If lblPrice.Text = "" Then
                            '    lblPrice.Text = 0
                            'End If
                            'dt.Rows(row.RowIndex)("Price") = lblPrice.Text


                            dt.Rows(row.RowIndex)("Outletid") = ddlOutlet.SelectedValue
                            dt.Rows(row.RowIndex)("CustID") = custid()

                            'Dim ttlcost As Decimal = CDec(txtEditQty.Text) * CDec(lblPrice.Text)

                            'dt.Rows(row.RowIndex)("Subtotal") = ttlcost
                            dt.AcceptChanges()

                        End If
                    End If
                Next

                insert(1)


                End If

        Catch ex As Exception
            Dim myStringVariable As String = "fail to save"
            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('" + myStringVariable + "');", True)
            errLog(Err.Description, "Pages_InvRcvEdit:btnSave_Click")
            Err.Clear()
        End Try



        ' Response.Redirect("~/Pages_InvRcv.aspx")
    End Sub

    'Protected Sub btnFinish_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnFinish.ServerClick
    '    Dim msg As String = ""
    '    Try
    '        If (Now.Hour >= 23 And Now.Minute >= 30) Or (Now.Hour = 0 And Now.Minute <= 30) Then
    '            notsuccesfull("Please process the transaction after 1.00 AM")
    '            Exit Sub
    '        End If
    '        For Each row As GridViewRow In DgvTransfer.Rows

    '            If row.RowType = DataControlRowType.DataRow Then

    '                If row.RowType = DataControlRowType.DataRow Then
    '                    Dim lblItemNo As Label = (TryCast(row.FindControl("lblItemNo"), Label))
    '                    Dim lblName As Label = (TryCast(row.FindControl("lblName"), Label))
    '                    Dim lblQty As Label = (TryCast(row.FindControl("lblQty"), Label))
    '                    Dim txtEditQty As TextBox = (TryCast(row.FindControl("txtEditQty"), TextBox))

    '                    Dim lblPrice As Label = (TryCast(row.FindControl("lblPrice"), Label))
    '                    Dim lblttl_Cost As Label = (TryCast(row.FindControl("lblttlCost"), Label))
    '                    Dim txtEditPrice As TextBox = (TryCast(row.FindControl("txtEditPrice"), TextBox))
    '                    lblName.Visible = True
    '                    txtEditQty.Visible = False
    '                    lblQty.Visible = True
    '                    txtEditPrice.Visible = False
    '                    lblPrice.Visible = True


    '                    lblQty.Text = txtEditQty.Text
    '                    If IsNumeric(txtEditQty.Text) = False Then
    '                        msg = msg + " row " + row.RowIndex.ToString + " Qty must be number" & newLine

    '                    End If

    '                    lblPrice.Text = txtEditPrice.Text
    '                    If IsNumeric(txtEditPrice.Text) = False Then
    '                        msg = msg + " row " + row.RowIndex.ToString + " cost must be number" & newLine
    '                        txtEditPrice.Text = 0
    '                    End If

    '                    If txtEditPrice.Text = 0 Then

    '                        msg = msg + " row " + row.RowIndex.ToString + " Please Input Price" & newLine
    '                        txtEditPrice.Text = 0
    '                        txtEditQty.Text = 0

    '                    End If

    '                    If Val(txtEditQty.Text) >= 999999999 Then

    '                        msg = msg + " row " + row.RowIndex.ToString + " Unit 1 must less than 999999" & newLine
    '                    End If

    '                    If Val(txtEditPrice.Text) >= 999999999 Then
    '                        msg = msg + " row " + row.RowIndex.ToString + " Cost must less than 999999" & newLine
    '                    End If




    '                    Dim lblUnit_1 As Label = (TryCast(row.FindControl("lblEditEmpID"), Label))

    '                    Dim dt As DataTable = DirectCast(ViewState("InvPO"), DataTable)
    '                    Dim clsI_stk As New clsInventory_stk

    '                    Dim total As Decimal = (Val(txtEditQty.Text) * Val(txtEditPrice.Text))

    '                    If msg <> "" Then
    '                        notsuccesfull(msg)
    '                        Exit Sub
    '                    End If

    '                    dt.Rows(row.RowIndex)("ItemNo") = lblItemNo.Text

    '                    dt.Rows(row.RowIndex)("ItemName") = lblName.Text

    '                    dt.Rows(row.RowIndex)("Qty") = txtEditQty.Text
    '                    dt.Rows(row.RowIndex)("Price") = lblPrice.Text

    '                    dt.Rows(row.RowIndex)("Subtotal") = total
    '                    dt.Rows(row.RowIndex)("Outletid") = ddlOutlet.SelectedValue
    '                    dt.Rows(row.RowIndex)("CustID") = custid()

    '                    Dim ttlcost As Decimal = txtEditQty.Text * lblPrice.Text
    '                    dt.Rows(row.RowIndex)("ttlCost") = ttlcost
    '                    dt.AcceptChanges()
    '                    bindingstk()
    '                    BindData()

    '                End If
    '            End If
    '        Next


    '        insert(2, False, bacc)


    '    Catch ex As Exception
    '        errLog(Err.Description, "Pages_InvRcvEdit:btnFinish_Click")
    '        Err.Clear()
    '    End Try


    'End Sub



    Private Sub check_validation()
        Try
            If ddlCustomer.SelectedValue <> 0 And ddlOutlet.SelectedIndex >= 0 Then
                RadButton1.Disabled = False
            Else
                RadButton1.Disabled = True
            End If
        Catch ex As Exception
            errLog(Err.Description, "Pages_InvRcvEdit:check_validation")
            Err.Clear()
        End Try
    End Sub

    Protected Sub btnBack_ServerClick(sender As Object, e As EventArgs) Handles btnBack.ServerClick
        Response.Redirect("~/Pages_CPO.aspx", False)
    End Sub
    Protected Sub btnClearSearch_Click(sender As Object, e As EventArgs)
        Try
            For Each item As ListViewItem In LsvStk.Items
                Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox1"), CheckBox)

                ck.Checked = False


            Next
            txtSearch.Text = ""
            lblnoitem.Text = 0
            bindingstk1()
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
            errLog(Err.Description, "Pages_InvRcvEdit:btnSearch_Click")
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
            errLog(Err.Description, "Pages_InvRcvEdit:txtSearch_TextChanged")
            Err.Clear()
        End Try
    End Sub
    Private Sub BindCheckItem()
        Try

            If CBool(IIf(IsNothing(ViewState("ListItem")), True, False)) Then


                Dim ListItem2 As List(Of Item_Detail) = New List(Of Item_Detail)()

                Dim dt, dt2 As New DataTable
                dt = DirectCast(ViewState("InvPO"), DataTable)
                'dt2 = cInv.getDtStk(dt, custid, txtSearch.Text, modulepriv)
                dt2 = cInv.getItem(dt, custid, "", modulepriv, ddlOutlet.SelectedValue)
                If dt2.Rows.Count > 0 Then
                    For Each dtrow As DataRow In dt2.Rows
                        ListItem2.Add(New Item_Detail With {
                                           .ItemName = dtrow.Item("itemname"),
                                           .Status = False
                                           })
                    Next
                End If


                For Each item As ListViewItem In LsvStk.Items
                    Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox1"), CheckBox)
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
                    Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox1"), CheckBox)
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
            errLog(Err.Description, "Pages_InvRcvEdit:BindCheckItem")
            Err.Clear()
        End Try
    End Sub

    Private Sub CheckedItem()
        Try


            Dim ListStore As List(Of Item_Detail) = TryCast(ViewState("ListItem"), List(Of Item_Detail))

            Dim i As Integer = 0
            For Each item As ListViewItem In LsvStk.Items
                Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox1"), CheckBox)
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
            errLog(Err.Description, "Pages_InvRcvEdit:CheckedItem")
            Err.Clear()
        End Try
    End Sub

    Protected Sub btncancel_ServerClick(sender As Object, e As EventArgs) Handles btncancel.ServerClick
        Try
            insert(2, True, bacc)
        Catch ex As Exception
            errLog(Err.Description, "Pages_InvRcvEdit:btncancel_ServerClick")
            Err.Clear()
        End Try
    End Sub

    Protected Sub btnedit_ServerClick(sender As Object, e As EventArgs) Handles btnedit.ServerClick
        Try
            If btnedit.InnerText = "edit" Then
                For Each row As GridViewRow In DgvTransfer.Rows

                    If row.RowType = DataControlRowType.DataRow Then
                        row.FindControl("txtEditQty").Visible = True
                        row.FindControl("lblQty").Visible = False
                        'lblUnit1
                        'row.FindControl("txtEditPrice").Visible = True
                        'row.FindControl("lblPrice").Visible = False


                    End If
                Next

                'btnedit.InnerText = "Calc."
                btnedit.InnerHtml = "<i class='fas fa-sync'></i>"
            Else
                For Each row As GridViewRow In DgvTransfer.Rows

                    If row.RowType = DataControlRowType.DataRow Then
                        row.FindControl("txtEditQty").Visible = False
                        row.FindControl("lblQty").Visible = True
                        'lblUnit1
                        'row.FindControl("txtEditPrice").Visible = False
                        'row.FindControl("lblPrice").Visible = True

                    End If
                Next
                update()
                btnedit.InnerText = "edit"
            End If


        Catch ex As Exception
            errLog(Err.Description, "Pages_InvRcvEdit:btnedit_ServerClick")
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
            If ViewState("InvPO") IsNot Nothing Then
                Dim dt As DataTable = CType(ViewState("InvPO"), DataTable)
                Dim dtstk As DataTable = DirectCast(ViewState("InvStk"), DataTable)
                Dim xdoctot As Double
                ' If dt.Rows.Count > 1 Then
                Dim lblstknm As Label = DirectCast(DgvTransfer.Rows(rowID).FindControl("lblName"), Label)
                Dim lblitemno As Label = DirectCast(DgvTransfer.Rows(rowID).FindControl("lblItemNo"), Label)


                Dim confirmValue As String = Request.Form("confirm_value")

                If confirmValue = "Yes" Then
                    If gvRow.RowIndex <= dt.Rows.Count - 1 Then
                        lid = dt.Rows(rowID).Item("smno")

                        dtstk.Rows.Add(lblitemno.Text, lblstknm.Text)
                        ViewState("InvStk") = dtstk

                        dt.Rows.Remove(dt.Rows(rowID))
                        dt.AcceptChanges()


                        ResetRowID(dt)
                    End If
                Else
                    Exit Sub
                End If

                'End If

                ViewState("InvPO") = dt

                Dim totalcost As Double = 0

                For Each dr As DataRow In dt.Rows
                    totalcost += Convert.ToDouble(dr("subtotal"))
                Next

                DgvTransfer.Columns(5).FooterText = totalcost.ToString("#,##0.00")

                '    DgvTransfer.DataSource = dt
                'DgvTransfer.DataBind()
                BindData()
                bindingstk()
                DgvTransfer.ShowFooter = True
                If dt.Rows.Count > 0 Then
                    xdoctot = dt.Compute("sum(subtotal)", String.Empty)
                Else
                    xdoctot = 0
                End If
                insert(1, False, True)
                'strmaster = "update cld_invlk_rcv set  doctot = " & xdoctot & " where id = '" & lid & "' and custid = " & custid() & ""
                ''" & dt.Rows(rowID).Item("linkid") & "'
                'c_cmd.executeNonQuery(strmaster)
            End If
        Catch ex As Exception
            errLog(Err.Description, "Pages_InvRcvEdit:imgbtnDelete_Click")
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
            errLog(Err.Description, "Pages_InvRcvEdit:ResetRowID")
            Err.Clear()
        End Try
    End Sub
    Protected Sub update()
        Dim msg As String = ""
        Dim newLine As String = "\n"
        For Each row As GridViewRow In DgvTransfer.Rows

            If row.RowType = DataControlRowType.DataRow Then

                If row.RowType = DataControlRowType.DataRow Then
                    Dim lblItemNo As Label = (TryCast(row.FindControl("lblItemNo"), Label))
                    Dim lblName As Label = (TryCast(row.FindControl("lblName"), Label))
                    Dim lblQty As Label = (TryCast(row.FindControl("lblQty"), Label))
                    Dim txtEditQty As TextBox = (TryCast(row.FindControl("txtEditQty"), TextBox))

                    'Dim lblPrice As Label = (TryCast(row.FindControl("lblPrice"), Label))
                    'Dim lblttl_Cost As Label = (TryCast(row.FindControl("lblttlCost"), Label))
                    'Dim txtEditPrice As TextBox = (TryCast(row.FindControl("txtEditPrice"), TextBox))
                    lblName.Visible = True
                    txtEditQty.Visible = False
                    lblQty.Visible = True
                    'txtEditPrice.Visible = False
                    'lblPrice.Visible = True

                    'Dim total As Decimal = (Val(txtEditQty.Text) * Val(txtEditPrice.Text))
                    lblQty.Text = txtEditQty.Text
                    'If IsNumeric(txtEditQty.Text) = False Then
                    '    msg = msg + " row " + row.RowIndex.ToString + " Qty must be number" & newLine

                    'End If

                    'lblPrice.Text = txtEditPrice.Text
                    'If IsNumeric(txtEditPrice.Text) = False Then
                    '    msg = msg + " row " + row.RowIndex.ToString + " cost must be number" & newLine
                    '    txtEditPrice.Text = 0
                    'End If

                    'If txtEditPrice.Text = 0 Then

                    '    msg = msg + " row " + row.RowIndex.ToString + " Please Input Price" & newLine
                    '    txtEditPrice.Text = 0
                    '    txtEditQty.Text = 0

                    'End If

                    If Val(txtEditQty.Text) >= 999999999 Then

                        msg = msg + " row " + row.RowIndex.ToString + " Unit 1 must less than 999999" & newLine
                    End If

                    'If Val(txtEditPrice.Text) >= 999999999 Then
                    '    msg = msg + " row " + row.RowIndex.ToString + " Cost must less than 999999" & newLine
                    'End If

                    'If Val(total) >= 999999999 Then

                    '    msg = msg + " row " + row.RowIndex.ToString + " Total must less than 999999" & newLine

                    'End If


                    Dim lblid As Label = (TryCast(row.FindControl("lblEditEmpID"), Label))

                    Dim dt As DataTable = DirectCast(ViewState("InvPO"), DataTable)
                    Dim clsI_stk As New clsInventory_stk



                    If msg <> "" Then
                        notsuccesfull(msg)
                        Exit Sub
                    End If




                    dt.Rows(row.RowIndex)("ItemNo") = lblItemNo.Text

                    dt.Rows(row.RowIndex)("ItemName") = lblName.Text

                    dt.Rows(row.RowIndex)("Qty") = txtEditQty.Text
                    'dt.Rows(row.RowIndex)("Price") = lblPrice.Text
                    'Dim ttlcost As Decimal = CDec(txtEditQty.Text) * CDec(lblPrice.Text)
                    'dt.Rows(row.RowIndex)("Subtotal") = ttlcost
                    dt.Rows(row.RowIndex)("Outletid") = ddlOutlet.SelectedValue
                    dt.Rows(row.RowIndex)("CustID") = custid()


                    dt.AcceptChanges()
                    bindingstk()
                    BindData()




                End If
            End If
        Next
    End Sub
    Protected Function GetBackground(ByVal rcvid As String)

        Dim imgstat As Integer = 0
        Dim cinvrcv As New Cls_IRcv
        Try
            imgstat = cinvrcv.rcv_date(rcvid)

        Catch ex As Exception
            errLog(Err.Description, "Pages_InvRcvEdit:GetBackground")
            Err.Clear()
        End Try


        Return imgstat

    End Function

    Protected Sub btnacc_ServerClick(sender As Object, e As EventArgs) Handles btnacc.ServerClick
        Try
            insert(1, False, True)
        Catch ex As Exception

        End Try

    End Sub
    Private Sub setAktifButton()
        btnacc.Visible = check_usrInvPriv(23, InvPriv)
    End Sub

    Protected Sub ddlCustomer_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlCustomer.SelectedIndexChanged
        check_validation()
    End Sub

    Protected Sub ddlOutlet_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlOutlet.SelectedIndexChanged
        'loadPayment()
        bindingstk1()
        check_validation()
    End Sub

    Private Sub bindusage(Optional ByVal rpt As String = "")

        dsrpt.Clear()
        Try


            If custid() <= 0 Or username() = "" Then
                FormsAuthentication.RedirectToLoginPage()
                Exit Sub
            End If

            dsrpt = clsreport.cal_usage(CDate(txtbusidate.Text), CDate(txtbusidate.Text), custid, username, modulepriv, ddlCustomer.SelectedValue, txtLinkid.Text)



            ' setColumName()
            'perubahan 04 jul 2017

            If dsrpt.Tables.Count > 0 Then
                Dim dt As New DataTable
                dt = dsrpt.Tables(0)
                usagepage.Visible = True
                lblusage.Visible = True
                btnExportUsage.Visible = True
                If dsrpt IsNot Nothing AndAlso dsrpt.Tables.Count > 0 Then
                    dg1.DataSource = dsrpt
                Else
                    dg1.DataSource = Nothing
                End If
                ViewState("vsreport") = dsrpt.Tables(0)
                dg1.DataBind()
                paging()
                'dt.Rows.Clear()
                dt.Dispose()

                dsrpt.Tables.Clear()
                dsrpt.Dispose()



            Else
                usagepage.Visible = False
                lblusage.Visible = False
                dg1.DataSource = Nothing
                dg1.DataBind()
                paging()
                dsrpt.Tables.Clear()
                dsrpt.Dispose()
            End If
            ScriptManager.RegisterStartupScript(
    Me,
    Me.GetType(),
    "scrollUsage",
    "document.getElementById('usageStock').scrollIntoView({behavior:'smooth'});",
    True
)
        Catch
            mfunction.errLog(Err.Description, "reportInventory:BindData")
            Err.Clear()
            'Finally
            '    dsrpt.Dispose()
            'dsrpt2.Dispose()
        End Try
    End Sub
    Private Sub hidepage()
        btnFirst.Visible = False
        btnPrev.Visible = False
        pagelbl.Visible = False
        btnNext.Visible = False
        btnLast.Visible = False


    End Sub
    Private Sub hidelast()

        btnNext.Visible = False
        btnLast.Visible = False
        btnFirst.Visible = True
        btnPrev.Visible = True
        pagelbl.Visible = True
    End Sub
    Private Sub hidefirst()
        btnFirst.Visible = False
        btnPrev.Visible = False
        btnNext.Visible = True
        btnLast.Visible = True
        pagelbl.Visible = True


    End Sub
    Private Sub btnFirst_Click(sender As Object, e As EventArgs) Handles btnFirst.Click

        If (dg1.CurrentPageIndex > 0) Then
            dg1.CurrentPageIndex = 0
            bindusage()
            hidefirst()
        End If

    End Sub

    Private Sub btnPrev_Click(sender As Object, e As EventArgs) Handles btnPrev.Click

        If (dg1.CurrentPageIndex > 0) Then
            dg1.CurrentPageIndex = dg1.CurrentPageIndex - 1
            bindusage()
            If dg1.CurrentPageIndex = 0 Then
                hidefirst()
            Else
                btnFirst.Visible = True
                btnPrev.Visible = True
                btnNext.Visible = True
                btnLast.Visible = True
                pagelbl.Visible = True
            End If
        End If


    End Sub

    Private Sub btnNext_Click(sender As Object, e As EventArgs) Handles btnNext.Click

        If (dg1.CurrentPageIndex < dg1.PageCount - 1) Then
            dg1.CurrentPageIndex = dg1.CurrentPageIndex + 1
            bindusage()
            If (dg1.CurrentPageIndex = dg1.PageCount - 1) Then
                hidelast()
            Else
                btnFirst.Visible = True
                btnPrev.Visible = True
                btnNext.Visible = True
                btnLast.Visible = True
                pagelbl.Visible = True

            End If
        End If

    End Sub

    Private Sub btnLast_Click(sender As Object, e As EventArgs) Handles btnLast.Click

        If (dg1.CurrentPageIndex < dg1.PageCount - 1) Then
            dg1.CurrentPageIndex = dg1.PageCount - 1
            bindusage()
            hidelast()
        End If

    End Sub
    Private Sub paging()
        pagelbl.Text = " " & dg1.CurrentPageIndex + 1.ToString & " of " & dg1.PageCount.ToString & " "
        If dsrpt.Tables(0).Rows.Count <= 31 Then
            hidepage()
        Else
            hidefirst()
        End If
    End Sub
    Protected Sub dg1_PageIndexChanged(source As Object, e As DataGridPageChangedEventArgs)
        dg1.CurrentPageIndex = e.NewPageIndex
        Dim ds As New DataSet

        Try




            dsrpt = clsreport.cal_usage(CDate(txtbusidate.Text), CDate(txtbusidate.Text), custid, username, modulepriv, ddlCustomer.SelectedValue, txtLinkid.Text)


            ' setColumName()
            'perubahan 04 jul 2017
            If ds.Tables.Count > 0 Then
                dg1.DataSource = ds
                dg1.DataBind()
            Else
                dg1.DataSource = Nothing
                dg1.DataBind()
            End If
        Catch ex As Exception
            mfunction.errLog(Err.Description, "ReportInventory:dg1_PageIndexChanged")
            Err.Clear()
        End Try

        ds.Dispose()
    End Sub

    Protected Sub btnusage_ServerClick(sender As Object, e As EventArgs) Handles btnusage.ServerClick
        bindusage(txtLinkid.Text)
    End Sub
    Protected Sub btnExportDetail_Click(sender As Object, e As EventArgs) Handles btnExportDetail.Click
        Try
            BindData()  ' Pastikan data ter-update

            Dim originalDt As DataTable = DirectCast(ViewState("InvPO"), DataTable)

            ' Buat DataTable baru hanya dengan kolom yang diinginkan
            Dim dtExport As New DataTable()
            dtExport.Columns.Add("smNo", GetType(String))
            dtExport.Columns.Add("ItemNo", GetType(String))
            dtExport.Columns.Add("ItemName", GetType(String))
            dtExport.Columns.Add("Qty", GetType(Decimal))  ' atau Integer kalau qty selalu bulat

            ' Isi data dari originalDt, tapi hanya kolom yang dipilih
            For Each row As DataRow In originalDt.Rows
                Dim newRow As DataRow = dtExport.NewRow()
                newRow("smNo") = row("smno").ToString()          ' PO Number diulang setiap baris
                newRow("ItemNo") = row("itemno").ToString()
                newRow("ItemName") = row("itemname").ToString()
                newRow("Qty") = Convert.ToDecimal(row("Qty"))    ' Pastikan numeric
                dtExport.Rows.Add(newRow)
            Next

            Using wb As New XLWorkbook()
                Dim ws As IXLWorksheet = wb.Worksheets.Add(dtExport, "Detail Pre-Order")

                ' Insert table dari DataTable
                'Dim dt As DataTable = DirectCast(ViewState("InvPO"), DataTable)
                'ws.Cell(1, 1).InsertTable(dt, False)

                ' Styling header
                ws.Row(1).Style.Font.Bold = True
                ws.Row(1).Style.Fill.BackgroundColor = XLColor.FromArgb(77, 77, 77)
                ws.Row(1).Style.Font.FontColor = XLColor.White

                ' Auto-fit
                ws.Columns().AdjustToContents()

                ' --- TOTAL ROW ---
                'Dim dataStartRow As Integer = 2  ' Header di row 1
                'Dim dataEndRow As Integer = ws.LastRowUsed().RowNumber()

                'If dataEndRow >= dataStartRow Then
                '    ' Cari kolom Subtotal secara otomatis berdasarkan header
                '    Dim totalColumn As Integer = 0
                '    For i As Integer = 1 To ws.Row(1).CellsUsed().Count()
                '        If ws.Cell(1, i).GetString().Trim().ToLower() = "subtotal" Then
                '            totalColumn = i
                '            Exit For
                '        End If
                '    Next

                '    If totalColumn > 0 Then  ' Kalau ketemu kolom Subtotal
                '        Dim totalRow As Integer = dataEndRow + 1

                '        ' Label "Total"
                '        ws.Cell(totalRow, totalColumn - 1).Value = "Total"
                '        ws.Cell(totalRow, totalColumn - 1).Style.Font.Bold = True
                '        ws.Cell(totalRow, totalColumn - 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right

                '        ' Formula SUM (SUBTOTAL(9, ...) untuk ignore hidden rows kalau ada filter nanti)
                '        Dim rangeAddress As String = $"{ws.Cell(dataStartRow, totalColumn).Address}:{ws.Cell(dataEndRow, totalColumn).Address}"
                '        ws.Cell(totalRow, totalColumn).FormulaA1 = $"=SUBTOTAL(9,{rangeAddress})"

                '        ' Styling total
                '        ws.Cell(totalRow, totalColumn).Style.Font.Bold = True
                '        ws.Cell(totalRow, totalColumn).Style.Fill.BackgroundColor = XLColor.LightGray
                '        ws.Cell(totalRow, totalColumn).Style.NumberFormat.Format = "#,##0.00"  ' Format angka
                '    End If
                'End If

                ' Download
                Response.Clear()
                Response.Buffer = True
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                Response.AddHeader("content-disposition", "attachment;filename=PreOrder_Detail_" & txtLinkid.Text & ".xlsx")

                Using ms As New MemoryStream()
                    wb.SaveAs(ms)
                    ms.WriteTo(Response.OutputStream)
                End Using
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideLoading", "HideProgress();", True)
                Response.Flush()

                Response.End()
            End Using
        Catch ex As Exception
            notsuccesfull("Export gagal: " & ex.Message)
        End Try
    End Sub

    Protected Sub btnExportUsage_Click(sender As Object, e As EventArgs) Handles btnExportUsage.Click
        Dim dt As New DataTable()
        Try
            ' Pastikan usage sudah di-load (kalau belum, load dulu)
            'If dg1.Items.Count = 0 Then
            '    bindusage()  ' Ini akan load data usage kalau belum
            'End If
            If DirectCast(ViewState("vsreport"), DataTable).Rows.Count = 0 Then
                bindusage()
            Else
                dt = DirectCast(ViewState("vsreport"), DataTable)
            End If
            ' Convert DataGrid ke DataTable

            For Each col As DataGridColumn In dg1.Columns
                dt.Columns.Add(col.HeaderText)
            Next

            For Each item As DataGridItem In dg1.Items
                Dim row As DataRow = dt.NewRow()
                For i As Integer = 0 To dg1.Columns.Count - 1
                    row(i) = item.Cells(i).Text.Replace("&nbsp;", "")
                Next
                dt.Rows.Add(row)
            Next

            Using wb As New XLWorkbook()
                Dim ws As IXLWorksheet = wb.Worksheets.Add(dt, "Usage Stock")

                ' Styling header
                ws.Row(1).Style.Font.Bold = True
                ws.Row(1).Style.Fill.BackgroundColor = XLColor.FromArgb(75, 108, 158)  ' Biru tua
                ws.Row(1).Style.Font.FontColor = XLColor.White

                ' Auto-fit columns
                ws.Columns().AdjustToContents()

                ' Download
                Response.Clear()
                Response.Buffer = True
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                Response.AddHeader("content-disposition", "attachment;filename=PreOrder_Usage_" & txtLinkid.Text & ".xlsx")

                Using ms As New MemoryStream()
                    wb.SaveAs(ms)
                    ms.WriteTo(Response.OutputStream)
                    ms.Close()
                End Using
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideLoading", "HideProgress();", True)
                Response.Flush()

                Response.End()
            End Using
        Catch ex As Exception
            notsuccesfull("Gagal export usage: " & ex.Message)
        End Try
    End Sub

    'Protected Sub DdlPayment_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DdlPayment.SelectedIndexChanged
    '    check_validation()
    'End Sub
End Class
