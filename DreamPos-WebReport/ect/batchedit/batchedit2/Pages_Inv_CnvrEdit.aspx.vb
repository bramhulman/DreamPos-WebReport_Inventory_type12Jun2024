Imports System.Threading
Imports System.Net
Imports System.Text
Imports System.IO
Imports System.Data
Imports System.Drawing
Imports System.Globalization

Public Class Pages_Inv_CnvrEdit
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
    Private CCf As Integer
    Private Supid As Integer
    Private Dos As String

    Private iflag As Integer
    Private dCreate As Date
    Private drcvdate As Date
    Private dexdeldate As Date
    Private acc As String
    Private bacc As Boolean = False
    Private CIcc As New Cls_ICc
    Private cInv As New clsInventory
    Private cSupp As New clsInventory_Supplier
    Private CStk As New Cls_IStk
    Private CBatch As New Cls_IBatch
    Private CIRcpDtl As New Cls_IRcpDtl

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
            Mtid = Request.Cookies("iBatchID").Value
            c_CC = Request.Cookies("cc").Value
            c_stkid = Request.Cookies("stk_id").Value
            c_linkid = 0 'Request.Cookies("linkid").Value


            If Request.Cookies("UserLogin_inv") IsNot Nothing Then
                sNameuser = username()
                icustid = custid()
                imodul = modul()
                iModPriv = modulepriv()
                irptpriv = rptpriv()
                irptf = rptf()
                'loadCc()
                'loadItem()
                If IsPostBack = False Then
                    loadCc()
                    'loadItem()
                    'loadRcp()
                    'editlist(MTid)

                    If c_CC = 0 Or c_stkid = 0 Then
                        'hapus()
                        txtcreatedate.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                    Else
                        editlist(Mtid, custid)
                    End If
                    Dim dt, dt2 As New DataTable
                    dt = cInv.getDTInvBatch(Mtid, custid)
                    ViewState("InvRcv") = dt
                End If
                BindData()

                bindingstk()
                ScriptManager.GetCurrent(Me).RegisterPostBackControl(RadButton1)
                'ScriptManager.GetCurrent(Me).RegisterPostBackControl(btnUpdate)
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
    Sub loadCc()
        'ddlfrom.Items.Clear()
        'ddlfrom.SelectedValue = Nothing
        Dim ds As New DataSet
        ds = CIcc.SelectDataset_Icc(custid, username, modulepriv, 0)
        If ds IsNot Nothing AndAlso ds.Tables.Count > 0 Then
            Ddlfrom.DataSource = ds
        Else
            Ddlfrom.DataSource = Nothing
        End If
        Ddlfrom.DataValueField = "Id"
        Ddlfrom.DataTextField = "Name"
        Ddlfrom.DataBind()
        If Ddlfrom.Items.Count > 0 Then
            Ddlfrom.SelectedValue = 0
        End If
    End Sub
    Sub editlist(ByVal id As Integer, ByVal custid As Integer)

        Dim invC As CultureInfo = CultureInfo.InvariantCulture
        Dim ds As New DataSet
        ds = CBatch.SelectDataset_InBatchEdit(id, custid)
        If ds.Tables(0).Rows.Count = 0 Then
            Exit Sub
        End If

        ddlfrom.SelectedValue = ds.Tables(0).Rows(0).Item("cc")
        'ddlItem.SelectedValue = ds.Tables(0).Rows(0).Item("stk_id")
        'Dim unit1 As Decimal = ds.Tables(0).Rows(0).Item("U1")

        txtcreatedate.Text = ds.Tables(0).Rows(0).Item("invdate")
        'txtUnit1.Text = unit1.ToString("N", invC)
        'Dim unit2 As Decimal = ds.Tables(0).Rows(0).Item("U2")
        'txtUnit2.Text = unit2.ToString("N", invC)


        'txtstkNo.Text = ds.Tables(0).Rows(0).Item("stk_no")
        Dim losf As Decimal = ds.Tables(0).Rows(0).Item("factor1")
        'txtLosF.Text = losf.ToString("N", invC)

        'lblUnit1.Text = ds.Tables(0).Rows(0).Item("U1_nm")
        'lblUnit2.Text = ds.Tables(0).Rows(0).Item("U2_nm")
        'lbltotal.Text = ds.Tables(0).Rows(0).Item("U2_nm") 'ds.Tables(0).Rows(0).Item("u2").ToString
        'lblcurrent.Text = ds.Tables(0).Rows(0).Item("U2_nm") 'ds.Tables(0).Rows(0).Item("u2").ToString
        Dim totstk As Decimal = ds.Tables(0).Rows(0).Item("ttlstock")
        'txtTotalStk.Text = totstk.ToString("N", invC)
        'txtTotalStk.ReadOnly = True
        Dim stock As Decimal = ds.Tables(0).Rows(0).Item("stock")
        'txtStock.Text = stock.ToString("N", invC)
        'txtStock.ReadOnly = True
        'ddlItem.Enabled = False
        'ddlfrom.Enabled = False

        'Perubahan 3 maret 2017, jadi klo transaksi2 sebelumnya ga bisa di edit
        If ds.Tables(0).Rows(0).Item("InvDate") < DateTime.Now Then

            'btnUpdate.Visible = False
            If username() = "support" Then
                'btnUpdate.Visible = True

            End If
        Else
            'btnUpdate.Visible = True
            btnFinish.InnerText = "update"
        End If

        'btnInsert.Visible = False
    End Sub
    Protected Sub BindData()

        Try
            If custid() = 0 Then
                Response.Redirect("~/Default.aspx", False)
            Else
                Dim totalcost As Double = 0

                For Each dr As DataRow In DirectCast(ViewState("InvRcv"), DataTable).Rows
                    totalcost += Convert.ToDouble(dr("ttlCost"))
                Next

                DgvTransfer.Columns(11).FooterText = totalcost.ToString("#,##0.00")
                DgvTransfer.DataSource = DirectCast(ViewState("InvRcv"), DataTable)
                DgvTransfer.DataBind()
                DgvTransfer.ShowFooter = True

                If DgvTransfer.Rows.Count > 0 Then
                    DgvTransfer.Columns(12).Visible = True
                    btnedit.Visible = True
                    If Mtid <> "" Then
                        If GetBackground(Mtid) = 0 Then
                            DgvTransfer.Columns(12).Visible = False
                            btnedit.Visible = False

                        End If
                    End If

                Else
                    DgvTransfer.Columns(12).Visible = False
                    btnedit.Visible = False
                End If
            End If
        Catch ex As Exception
            errLog(Err.Description, "Pages_InvRcvEdit:BindData")
            Err.Clear()
        End Try

    End Sub

    Sub bindingstk()

        Try
            If custid() = 0 Then
                Response.Redirect("~/Default.aspx", False)
            Else
                Dim dt, dt2 As New DataTable
                dt = DirectCast(ViewState("InvRcv"), DataTable)
                'dt2 = cInv.getDtStk(dt, custid, txtSearch.Text, modulepriv)
                dt2 = CStk.SelectDataTable_Stkall(dt, custid, txtSearch.Text, modulepriv, ddlFrom.SelectedValue)
                'WService.getDtStk(dt, cust, txtSearch.Text, ModPrv)
                ViewState("InvStk") = dt2
                LsvStk2.DataSource = DirectCast(ViewState("InvStk"), DataTable)
                LsvStk2.DataBind()
            End If
        Catch ex As Exception
            errLog(Err.Description, "Pages_InvRcvEdit:bindingstk")
            Err.Clear()
        End Try


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
            Add()
            'DgvTransfer.DataBind()
        Catch ex As Exception
            errLog(Err.Description, "Pages_InvRcvEdit:RadButton1_ServerClick")
            Err.Clear()
        End Try
    End Sub
    Sub Add()
        Dim dt As New DataTable
        Try
            Dim i As Integer = 0
            If DirectCast(ViewState("InvRcv"), DataTable).Rows.Count >= My.Settings.limitorder Then
                notsuccesfull("Silakan Batch di bawah " & My.Settings.limitorder & " item, pisah jika lebih !!")
                GoTo ef
            End If
            For Each item As ListViewItem In lsvstk2.Items
                Dim ck As CheckBox = CType(item.FindControl("CheckBox2"), CheckBox)

                If ck IsNot Nothing AndAlso ck.Checked Then
                    Try
                        Dim cstk As New clsInventory_stk
                        Dim ds As DataSet = cstk.SelectData_Inv_Stkcls((DirectCast(item.FindControl("lblID"), Label).Text), ddlFrom.SelectedValue, custid, Date.Now.ToString("yyyy-MM-dd"))
                        If ds.Tables(0).Rows.Count > 0 Then
                            Dim u1 As String = ds.Tables(0).Rows(0).Item("Unit1").ToString
                            Dim u2 As String = ds.Tables(0).Rows(0).Item("Unit2").ToString
                            dt = DirectCast(ViewState("InvRcv"), DataTable)
                            Dim costu1 As String = ds.Tables(0).Rows(0).Item("coststk").ToString * ds.Tables(0).Rows(0).Item("Factor1")
                            Dim costu2 As String = ds.Tables(0).Rows(0).Item("coststk").ToString
                            dt.Rows.Add(0, custid, Date.Now.ToString("yyyy-MM-dd"), 1, ddlFrom.SelectedValue, (DirectCast(item.FindControl("lblID"), Label).Text), (DirectCast(item.FindControl("lblName"), Label).Text), 0, 0, ds.Tables(0).Rows(0).Item("Factor1"), 0, costu1, costu2, "", 1, ds.Tables(0).Rows(0).Item("Unit1").ToString, ds.Tables(0).Rows(0).Item("Unit2").ToString, username, Date.Now.ToString("yyyy-MM-dd"), "", Date.Now.ToString("yyyy-MM-dd"), ds.Tables(0).Rows(0).Item("Number").ToString, ddlFrom.SelectedItem, 0)
                            ViewState("InvRcv") = dt
                            Dim dtStk As DataTable = DirectCast(ViewState("InvStk"), DataTable)
                            dtStk.Rows(i).Delete()
                            dtStk.AcceptChanges()
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

    Sub loadFrom()

        Dim ds As New DataSet
        Try
            ds = cInv.getInvcc(custid, username, modulepriv, 0)
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
            errLog(Err.Description, "Pages_InvRcvEdit:loadFrom")
            Err.Clear()
        End Try

    End Sub

    'Sub loadSup(ByVal Optional xccid As Integer = 0)

    '    Dim ds As New DataSet
    '    Try
    '        If xccid = 0 Then
    '            ds = cInv.getSupplierbyUser(custid, username, modulepriv)
    '        Else
    '            DdlSup.Items.Clear()
    '            ds = cSupp.SelectDataset_SupplierCCview(custid, ddlFrom.SelectedValue, "")
    '        End If


    '        DdlSup.DataSource = ds
    '        DdlSup.DataValueField = "Id"
    '        DdlSup.DataTextField = "Name"
    '        DdlSup.DataBind()

    '        If DdlSup.Items.Count > 0 Then
    '            DdlSup.SelectedIndex = 0
    '        End If
    '    Catch ex As Exception
    '        errLog(Err.Description, "Pages_InvRcvEdit:loadSup")
    '        Err.Clear()
    '    End Try

    'End Sub
    Sub insert(ByVal stat As Integer, ByVal Optional xcancel As Boolean = False, ByVal Optional xacc As Boolean = False) 'As Boolean
        Dim hasil As Boolean = False
        Try
            Dim msg As String = ""
            Dim dt As DataTable = DirectCast(ViewState("InvRcv"), DataTable)
            Dim newLine As String = "\n"
            Dim useddate As Date
            If dt.Rows.Count = 0 Then 'And stat <> 2 Then
                'cInv.setemptyreceive(txtLinkid.Text, username, custid, txtNotes.Text)
                msg = msg + "Item is Nothing" & newLine
            End If
            If ddlFrom.SelectedValue = 0 Or ddlFrom.SelectedItem.Text = "" Then
                msg = msg + "Cost Centre Must be Choice " & newLine
            End If


            For Each row As DataRow In dt.Rows
                If row.Item("ttlStock") = 0 Or row.Item("Cost") = 0 Then
                    msg = msg + row.Item("Stk_name") + " stock Or Cost Not Fill " & newLine
                    'notsuccesfull(msg)
                End If

            Next row
            If stat = 1 Then
                If txtcreatedate.Text = "" Then
                    txtcreatedate.Text = Date.Now.Date.ToString("yyyy-MM-dd 00:00:00")
                End If
                'useddate = CDate(txtcreatedate.Text)
            Else
                'If txtfinishdate.Text = "" Then
                '    txtfinishdate.Text = Date.Now.Date.ToString("yyyy-MM-dd 00:00:00")
                'End If
                'useddate = CDate(txtfinishdate.Text)
            End If
            'If txtexdel.Text = "" Then
            '    txtexdel.Text = Date.Now.Date.ToString("yyyy-MM-dd 00:00:00")
            '    useddate = CDate(txtexdel.Text)
            'Else
            '    useddate = CDate(txtexdel.Text)
            'End If
            If msg <> "" Then
                notsuccesfull(msg)
                Exit Sub
            Else
                'txtLinkid.Text = cInv.insertInvRcv(txtLinkid.Text, ddlFrom.SelectedValue, ddlFrom.SelectedItem.Text, dt, txtDO.Text, DdlSup.SelectedValue, stat, username, custid, useddate, txtNotes.Text, xcancel, xacc)  'WService.insertInvRcv(txtLinkid.Text, ddlFrom.SelectedValue, ddlFrom.SelectedItem.Text, dt, txtDO.Text, DdlSup.SelectedValue, stat, username, cust, CDate(txtExp.Text))

                hasil = True
            End If

            'txtLinkid.Text = WService.insertInvRcv(txtLinkid.Text, ddlFrom.SelectedValue, ddlFrom.SelectedItem.Text, dt, txtDO.Text, DdlSup.SelectedValue, stat, username, cust, CDate(txtExp.Text))
            '  ViewState("InvRcvEdit") = 0


            Response.Redirect("~/Pages_InvRcv.aspx", False)
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
                    deleteitem(id, lblstkid.Text)
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

            Dim txtUnit1 As TextBox = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("txtEditUnit1"), TextBox)
            If IsNumeric(txtUnit1.Text) = False Then
                notsuccesfull("Unit 1 must be number")
                Exit Sub
            End If

            'Dim txtCost As Label = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("txtEditCost"), Label)

            Dim txtUnit2 As TextBox = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("txtEditUnit2"), TextBox)
            If IsNumeric(txtUnit2.Text) = False Then
                notsuccesfull("Unit 2 must be number")
                Exit Sub
            End If

            Dim txtEditU1Cost As TextBox = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("txtEditU1Cost"), TextBox)

            Dim clsI_stk As New clsInventory_stk

            'Dim lblStkEditID As Label = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("lblStkEditID"), Label)
            Dim lblLosF1 As Label = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("lblfactor"), Label) 'lblfactor FindControl("lblEditfactor"), Label)
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

            Dim total As Decimal = (Val(txtUnit1.Text) * Val(lblLosF1.Text)) + Val(txtUnit2.Text)

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

            Dim dt As DataTable = DirectCast(ViewState("InvRcv"), DataTable)
            'Dim dtupdt As New DataTable
            'If Mtid <> "" Then

            '    dtupdt = dt.Clone
            '    dtupdt.Rows.Add(dt.Rows(e.RowIndex).ItemArray)
            '    maxrowup = dtupdt.Rows.Count - 1

            '    dtupdt.Rows(maxrowupx)("Factor1") = lblLosF1.Text

            '    dtupdt.Rows(maxrowup)("U1") = txtUnit1.Text

            '    dtupdt.Rows(maxrowup)("U2") = txtUnit2.Text

            '    dtupdt.Rows(maxrowup)("ttlStock") = total

            '    dtupdt.Rows(maxrowup)("CostU1") = txtEditU1Cost.Text
            'End If

            If lblLosF1.Text = 0 Then
                lblLosF1.Text = 1
                total = (Val(txtUnit1.Text) * Val(lblLosF1.Text)) + Val(txtUnit2.Text)
            End If

            dt.Rows(e.RowIndex)("Factor1") = clsI_stk.get_stkFactor1(dt.Rows(e.RowIndex)("stk_id"), custid) 'lblLosF1.Text

            dt.Rows(e.RowIndex)("U1") = txtUnit1.Text

            dt.Rows(e.RowIndex)("U2") = txtUnit2.Text

            dt.Rows(e.RowIndex)("ttlStock") = total

            dt.Rows(e.RowIndex)("CostU1") = txtEditU1Cost.Text




            Dim cost As Decimal = txtEditU1Cost.Text / lblLosF1.Text

            dt.Rows(e.RowIndex)("Cost") = cost

            Dim ttlcost As Decimal = total * cost
            dt.Rows(e.RowIndex)("ttlCost") = ttlcost

            'If Mtid <> "" Then
            '    dtupdt.Rows(maxrowup)("Cost") = cost
            '    dtupdt.Rows(maxrowup)("ttlCost") = ttlcost
            '    dtupdt.AcceptChanges()

            '    ViewState("InvRcvupdt") = dtupdt
            'End If

            dt.AcceptChanges()


            DgvTransfer.EditIndex = -1
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

            Dim newLine As String = "\n"
            If iflag = 2 Then

                notsuccesfull("this order was complete")
                Exit Sub
            Else

                For Each row As GridViewRow In DgvTransfer.Rows

                    If row.RowType = DataControlRowType.DataRow Then

                        If row.RowType = DataControlRowType.DataRow Then
                            Dim lblUnit1 As Label = (TryCast(row.FindControl("lblUnit1"), Label))
                            Dim Label1 As Label = (TryCast(row.FindControl("Label1"), Label))
                            Dim txtEditUnit1 As TextBox = (TryCast(row.FindControl("txtEditUnit1"), TextBox))
                            Dim txtEditUnit2 As TextBox = (TryCast(row.FindControl("txtEditUnit2"), TextBox))
                            Dim lblU1Cost As Label = (TryCast(row.FindControl("lblU1Cost"), Label))
                            Dim lblttl_Cost As Label = (TryCast(row.FindControl("lblttlCost"), Label))
                            Dim txtEditU1Cost As TextBox = (TryCast(row.FindControl("txtEditU1Cost"), TextBox))
                            lblUnit1.Visible = True
                            txtEditUnit1.Visible = False
                            Label1.Visible = True
                            txtEditUnit2.Visible = False
                            lblU1Cost.Visible = True
                            txtEditU1Cost.Visible = False
                            lblUnit1.Text = txtEditUnit1.Text
                            Dim lblLosF1 As Label = (TryCast(row.FindControl("lblfactor"), Label))
                            Dim total As Decimal = (Val(txtEditUnit1.Text) * Val(lblLosF1.Text)) + Val(txtEditUnit2.Text)
                            If IsNumeric(txtEditUnit1.Text) = False Then
                                If IsNumeric(txtEditUnit2.Text) = False Then
                                    msg = msg + " row " + row.RowIndex.ToString + " Unit 1 must be number" & newLine
                                End If
                            End If
                            Label1.Text = txtEditUnit2.Text
                            If IsNumeric(txtEditUnit2.Text) = False Then
                                If IsNumeric(txtEditUnit1.Text) = False Then
                                    msg = msg + " row " + row.RowIndex.ToString + " Unit 2 must be number" & newLine
                                End If
                            End If
                            lblU1Cost.Text = txtEditU1Cost.Text
                            If IsNumeric(txtEditU1Cost.Text) = False Then
                                msg = msg + " row " + row.RowIndex.ToString + " cost must be number" & newLine
                                txtEditU1Cost.Text = 0
                            End If

                            If txtEditU1Cost.Text = 0 Then

                                msg = msg + " row " + row.RowIndex.ToString + " Please Input Cost" &
                                txtEditU1Cost.Text = 0
                                txtEditUnit2.Text = 0

                            End If

                            If Val(txtEditUnit1.Text) >= 999999999 Then

                                msg = msg + " row " + row.RowIndex.ToString + " Unit 1 must less than 999999" & newLine
                            End If

                            If Val(txtEditU1Cost.Text) >= 999999999 Then
                                msg = msg + " row " + row.RowIndex.ToString + " Cost must less than 999999" & newLine
                            End If

                            If Val(total) >= 999999999 Then

                                msg = msg + " row " + row.RowIndex.ToString + " Total Stock must less than 999999" & newLine

                            End If


                            Dim lblUnit_1 As Label = (TryCast(row.FindControl("lblEditEmpID"), Label))

                            Dim dt As DataTable = DirectCast(ViewState("InvRcv"), DataTable)
                            Dim clsI_stk As New clsInventory_stk

                            If lblLosF1.Text = 0 Then
                                lblLosF1.Text = 1
                                total = (Val(txtEditU1Cost.Text) * Val(lblLosF1.Text)) + Val(txtEditU1Cost.Text)
                            End If


                            If msg <> "" Then
                                notsuccesfull(msg)
                                Exit Sub
                            End If

                            Dim lbl_stkid As Label = (TryCast(row.FindControl("lblStkID"), Label))


                            dt.Rows(row.RowIndex)("Factor1") = clsI_stk.get_stkFactor1(lbl_stkid.Text, custid) 'lblLosF1.Text

                            dt.Rows(row.RowIndex)("U1") = txtEditUnit1.Text

                            dt.Rows(row.RowIndex)("U2") = txtEditUnit2.Text

                            dt.Rows(row.RowIndex)("ttlStock") = total

                            dt.Rows(row.RowIndex)("CostU1") = txtEditU1Cost.Text


                            Dim cost As Decimal = txtEditU1Cost.Text / lblLosF1.Text

                            dt.Rows(row.RowIndex)("Cost") = cost

                            Dim ttlcost As Decimal = total * cost
                            dt.Rows(row.RowIndex)("ttlCost") = ttlcost
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

    Protected Sub btnFinish_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnFinish.ServerClick

        Try

            For Each row As GridViewRow In DgvTransfer.Rows

                If row.RowType = DataControlRowType.DataRow Then

                    If row.RowType = DataControlRowType.DataRow Then
                        Dim lblUnit1 As Label = (TryCast(row.FindControl("lblUnit1"), Label))
                        Dim Label1 As Label = (TryCast(row.FindControl("Label1"), Label))
                        Dim txtEditUnit1 As TextBox = (TryCast(row.FindControl("txtEditUnit1"), TextBox))
                        Dim txtEditUnit2 As TextBox = (TryCast(row.FindControl("txtEditUnit2"), TextBox))
                        Dim lblU1Cost As Label = (TryCast(row.FindControl("lblU1Cost"), Label))
                        Dim lblttl_Cost As Label = (TryCast(row.FindControl("lblttlCost"), Label))
                        Dim txtEditU1Cost As TextBox = (TryCast(row.FindControl("txtEditU1Cost"), TextBox))
                        lblUnit1.Visible = True
                        txtEditUnit1.Visible = False
                        Label1.Visible = True
                        txtEditUnit2.Visible = False
                        lblU1Cost.Visible = True
                        txtEditU1Cost.Visible = False
                        lblUnit1.Text = txtEditUnit1.Text
                        Dim lblLosF1 As Label = (TryCast(row.FindControl("lblfactor"), Label))
                        Dim total As Decimal = (Val(txtEditUnit1.Text) * Val(lblLosF1.Text)) + Val(txtEditUnit2.Text)
                        If IsNumeric(txtEditUnit1.Text) = False Then
                            msg = msg + " row " + row.RowIndex.ToString + " Unit 1 must be number" & newLine

                        End If
                        Label1.Text = txtEditUnit2.Text
                        If IsNumeric(txtEditUnit2.Text) = False Then
                            msg = msg + " row " + row.RowIndex.ToString + " Unit 2 must be number" & newLine

                        End If
                        lblU1Cost.Text = txtEditU1Cost.Text
                        If IsNumeric(txtEditU1Cost.Text) = False Then
                            msg = msg + " row " + row.RowIndex.ToString + " cost must be number" & newLine
                            txtEditU1Cost.Text = 0
                        End If

                        If txtEditU1Cost.Text = 0 Then

                            msg = msg + " row " + row.RowIndex.ToString + " Please Input Cost" & newLine
                            txtEditU1Cost.Text = 0
                            txtEditUnit2.Text = 0

                        End If

                        If Val(txtEditUnit1.Text) >= 999999999 Then

                            msg = msg + " row " + row.RowIndex.ToString + " Unit 1 must less than 999999" & newLine
                        End If

                        If Val(txtEditU1Cost.Text) >= 999999999 Then
                            msg = msg + " row " + row.RowIndex.ToString + " Cost must less than 999999" & newLine
                        End If

                        If Val(total) >= 999999999 Then

                            msg = msg + " row " + row.RowIndex.ToString + " Total Stock must less than 999999" & newLine

                        End If


                        Dim lblUnit_1 As Label = (TryCast(row.FindControl("lblEditEmpID"), Label))

                        Dim dt As DataTable = DirectCast(ViewState("InvRcv"), DataTable)
                        Dim clsI_stk As New clsInventory_stk

                        If lblLosF1.Text = 0 Then
                            lblLosF1.Text = 1
                            total = (Val(txtEditU1Cost.Text) * Val(lblLosF1.Text)) + Val(txtEditU1Cost.Text)
                        End If

                        If msg <> "" Then
                            notsuccesfull(msg)
                            Exit Sub
                        End If

                        Dim lbl_stkid As Label = (TryCast(row.FindControl("lblStkID"), Label))


                        dt.Rows(row.RowIndex)("Factor1") = clsI_stk.get_stkFactor1(lbl_stkid.Text, custid) 'lblLosF1.Text

                        dt.Rows(row.RowIndex)("U1") = txtEditUnit1.Text

                        dt.Rows(row.RowIndex)("U2") = txtEditUnit2.Text

                        dt.Rows(row.RowIndex)("ttlStock") = total

                        dt.Rows(row.RowIndex)("CostU1") = txtEditU1Cost.Text


                        Dim cost As Decimal = txtEditU1Cost.Text / lblLosF1.Text

                        dt.Rows(row.RowIndex)("Cost") = cost

                        Dim ttlcost As Decimal = total * cost
                        dt.Rows(row.RowIndex)("ttlCost") = ttlcost
                        dt.AcceptChanges()
                        bindingstk()
                        BindData()

                    End If
                End If
            Next


            insert(2, False, bacc)


        Catch ex As Exception
            errLog(Err.Description, "Pages_InvRcvEdit:btnFinish_Click")
            Err.Clear()
        End Try


    End Sub




    Protected Sub ddlFrom_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlFrom.SelectedIndexChanged
        Dim dt As DataTable = DirectCast(ViewState("InvRcv"), DataTable)
        Try
            If Session("xfrom") Is Nothing Then
            Else
                If dt.Rows.Count > 0 And ddlFrom.SelectedValue <> Session("xfrom").ToString Then
                    notsuccesfull("you must delete item before change")
                    ddlFrom.SelectedValue = Session("xfrom").ToString
                Else
                    'bindingstk()
                    check_validation()
                End If

            End If
            'loadSup(ddlFrom.SelectedValue)
            bindingstk()
        Catch ex As Exception
            errLog(Err.Description, "Pages_InvRcvEdit:ddlFrom_SelectedIndexChanged")
            Err.Clear()
        End Try

    End Sub
    Private Sub check_validation()
        Try
            'If txtDO.Text <> "" And ddlFrom.SelectedIndex >= 0 And DdlSup.SelectedIndex >= 0 Then
            RadButton1.Disabled = False
            'Else
            '    RadButton1.Disabled = True
            'End If
        Catch ex As Exception
            errLog(Err.Description, "Pages_InvRcvEdit:check_validation")
            Err.Clear()
        End Try
    End Sub

    Protected Sub btnBack_ServerClick(sender As Object, e As EventArgs) Handles btnBack.ServerClick
        Response.Redirect("~/Pages_Inv_Cnvr.aspx", False)
    End Sub
    Protected Sub btnClearSearch_Click(sender As Object, e As EventArgs)
        Try
            For Each item As ListViewItem In lsvstk2.Items
                Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox2"), CheckBox)

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
                dt = DirectCast(ViewState("InvRcv"), DataTable)
                dt2 = cInv.getDtStk(dt, custid, txtSearch.Text, modulepriv)
                If dt2.Rows.Count > 0 Then
                    For Each dtrow As DataRow In dt2.Rows
                        ListItem2.Add(New Item_Detail With {
                                           .ItemName = dtrow.Item("name"),
                                           .Status = False
                                           })
                    Next
                End If


                For Each item As ListViewItem In lsvstk2.Items
                    Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox2"), CheckBox)
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

                For Each item As ListViewItem In lsvstk2.Items
                    Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox2"), CheckBox)
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
            For Each item As ListViewItem In lsvstk2.Items
                Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox2"), CheckBox)
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
                        row.FindControl("txtEditUnit1").Visible = True
                        row.FindControl("lblUnit1").Visible = False
                        'lblUnit1
                        row.FindControl("txtEditUnit2").Visible = True
                        row.FindControl("Label1").Visible = False
                        row.FindControl("txtEditU1Cost").Visible = True
                        row.FindControl("lblU1Cost").Visible = False

                    End If
                Next

                'btnedit.InnerText = "Calc."
                btnedit.InnerHtml = "<i class='fas fa-sync'></i>"
            Else
                For Each row As GridViewRow In DgvTransfer.Rows

                    If row.RowType = DataControlRowType.DataRow Then
                        row.FindControl("txtEditUnit1").Visible = False
                        row.FindControl("lblUnit1").Visible = True
                        'lblUnit1
                        row.FindControl("txtEditUnit2").Visible = False
                        row.FindControl("Label1").Visible = True
                        row.FindControl("txtEditU1Cost").Visible = False
                        row.FindControl("lblU1Cost").Visible = True

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
            If ViewState("InvRcv") IsNot Nothing Then
                Dim dt As DataTable = CType(ViewState("InvRcv"), DataTable)
                Dim dtstk As DataTable = DirectCast(ViewState("InvStk"), DataTable)
                Dim xdoctot As Double
                ' If dt.Rows.Count > 1 Then
                Dim lblstknm As Label = DirectCast(DgvTransfer.Rows(rowID).FindControl("lblName"), Label)
                Dim lblstkid As Label = DirectCast(DgvTransfer.Rows(rowID).FindControl("lblStkID"), Label)


                Dim confirmValue As String = Request.Form("confirm_value")

                If confirmValue = "Yes" Then
                    If gvRow.RowIndex <= dt.Rows.Count - 1 Then
                        lid = dt.Rows(rowID).Item("linkid")
                        cInv.DeleteInvRcvitembylid(lid, dt.Rows(rowID).Item("stk_ID"), custid, username)
                        'cInv.DeleteInvRcvDetail(dt.Rows(rowID).Item("ID"), custid, username)

                        dtstk.Rows.Add(lblstkid.Text, lblstknm.Text)
                        ViewState("InvStk") = dtstk

                        dt.Rows.Remove(dt.Rows(rowID))
                        dt.AcceptChanges()


                        ResetRowID(dt)
                    End If
                Else
                    Exit Sub
                End If

                'End If

                ViewState("InvRcv") = dt

                Dim totalcost As Double = 0
                For Each dr As DataRow In dt.Rows
                    totalcost += Convert.ToDouble(dr("ttlCost"))
                Next

                DgvTransfer.Columns(11).FooterText = totalcost.ToString("#,##0.00")

                '    DgvTransfer.DataSource = dt
                'DgvTransfer.DataBind()
                BindData()
                bindingstk()
                DgvTransfer.ShowFooter = True
                If dt.Rows.Count > 0 Then
                    xdoctot = dt.Compute("sum(ttlCost)", String.Empty)
                Else
                    xdoctot = 0
                End If

                strmaster = "update cld_invlk_rcv set  doctot = " & xdoctot & " where id = '" & lid & "' and custid = " & custid() & ""
                ''" & dt.Rows(rowID).Item("linkid") & "'
                c_cmd.executeNonQuery(strmaster)
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
        For Each row As GridViewRow In DgvTransfer.Rows

            If row.RowType = DataControlRowType.DataRow Then

                If row.RowType = DataControlRowType.DataRow Then
                    Dim lblUnit1 As Label = (TryCast(row.FindControl("lblUnit1"), Label))
                    Dim Label1 As Label = (TryCast(row.FindControl("Label1"), Label))
                    Dim txtEditUnit1 As TextBox = (TryCast(row.FindControl("txtEditUnit1"), TextBox))
                    Dim txtEditUnit2 As TextBox = (TryCast(row.FindControl("txtEditUnit2"), TextBox))
                    Dim lblU1Cost As Label = (TryCast(row.FindControl("lblU1Cost"), Label))
                    Dim lblttl_Cost As Label = (TryCast(row.FindControl("lblttlCost"), Label))
                    Dim txtEditU1Cost As TextBox = (TryCast(row.FindControl("txtEditU1Cost"), TextBox))
                    lblUnit1.Visible = True
                    txtEditUnit1.Visible = False
                    Label1.Visible = True
                    txtEditUnit2.Visible = False
                    lblU1Cost.Visible = True
                    txtEditU1Cost.Visible = False
                    lblUnit1.Text = txtEditUnit1.Text
                    Dim lblLosF1 As Label = (TryCast(row.FindControl("lblfactor"), Label))
                    Dim total As Decimal = (Val(txtEditUnit1.Text) * Val(lblLosF1.Text)) + Val(txtEditUnit2.Text)
                    If IsNumeric(txtEditUnit1.Text) = False Then
                        msg = msg + " row " + row.RowIndex.ToString + " Unit 1 must be number" & newLine

                    End If
                    Label1.Text = txtEditUnit2.Text
                    If IsNumeric(txtEditUnit2.Text) = False Then
                        msg = msg + " row " + row.RowIndex.ToString + " Unit 2 must be number" & newLine

                    End If
                    lblU1Cost.Text = txtEditU1Cost.Text
                    If IsNumeric(txtEditU1Cost.Text) = False Then
                        msg = msg + " row " + row.RowIndex.ToString + " cost must be number" & newLine
                        txtEditU1Cost.Text = 0
                    End If

                    If txtEditU1Cost.Text = 0 Then

                        msg = msg + " row " + row.RowIndex.ToString + " Please Input Cost" & newLine
                        txtEditU1Cost.Text = 0
                        txtEditUnit2.Text = 0

                    End If

                    If Val(txtEditUnit1.Text) >= 999999999 Then

                        msg = msg + " row " + row.RowIndex.ToString + " Unit 1 must less than 999999" & newLine
                    End If

                    If Val(txtEditU1Cost.Text) >= 999999999 Then
                        msg = msg + " row " + row.RowIndex.ToString + " Cost must less than 999999" & newLine
                    End If

                    If Val(total) >= 999999999 Then

                        msg = msg + " row " + row.RowIndex.ToString + " Total Stock must less than 999999" & newLine

                    End If


                    Dim lblUnit_1 As Label = (TryCast(row.FindControl("lblEditEmpID"), Label))

                    Dim dt As DataTable = DirectCast(ViewState("InvRcv"), DataTable)
                    Dim clsI_stk As New clsInventory_stk

                    If lblLosF1.Text = 0 Then
                        lblLosF1.Text = 1
                        total = (Val(txtEditUnit1.Text) * Val(lblLosF1.Text)) + Val(txtEditUnit2.Text)
                    End If



                    If msg <> "" Then
                        notsuccesfull(msg)
                        Exit Sub
                    End If

                    Dim lbl_stkid As Label = (TryCast(row.FindControl("lblStkID"), Label))


                    dt.Rows(row.RowIndex)("Factor1") = clsI_stk.get_stkFactor1(lbl_stkid.Text, custid) 'lblLosF1.Text

                    dt.Rows(row.RowIndex)("U1") = txtEditUnit1.Text

                    dt.Rows(row.RowIndex)("U2") = txtEditUnit2.Text

                    dt.Rows(row.RowIndex)("ttlStock") = total

                    dt.Rows(row.RowIndex)("CostU1") = txtEditU1Cost.Text


                    Dim cost As Decimal = txtEditU1Cost.Text / lblLosF1.Text

                    dt.Rows(row.RowIndex)("Cost") = cost

                    Dim ttlcost As Decimal = total * cost
                    dt.Rows(row.RowIndex)("ttlCost") = ttlcost
                    dt.AcceptChanges()
                    bindingstk()
                    BindData()

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
End Class
