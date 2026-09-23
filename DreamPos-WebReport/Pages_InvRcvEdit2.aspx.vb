Imports System.Threading
Imports System.Net
Imports System.Text
Imports System.IO
Imports System.Data
Imports System.Drawing

Public Class Pages_InvRcvEdit2
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
    Private dExdeldate As Date
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
        Try
            Mtid = Request.Cookies("rcvID").Value
            CCf = Request.Cookies("rcvCc").Value
            Supid = Request.Cookies("rcvSupid").Value

            Dos = Request.Cookies("rcvDO").Value
            iflag = Request.Cookies("rcvflag").Value
            dCreate = Request.Cookies("rcvCreate").Value
            dExdeldate = Request.Cookies("exDeldate").Value


            If Request.Cookies("UserLogin_inv") IsNot Nothing Then
                sNameuser = username()
                icustid = custid()
                imodul = modul()
                iModPriv = modulepriv()
                irptpriv = rptpriv()
                irptf = rptf()

                If IsPostBack = False Then
                    txtExp.Text = Format(Now, "dd MMM yyyy")

                    loadFrom()
                    loadSup()

                    If Mtid = "" Then

                        txtDO.Text = 0
                        Dim dt, dt2 As New DataTable
                        dt = cInv.getDTInvRcv(Mtid, custid)
                        ViewState("InvRcv") = dt
                        BindData()

                        bindingstk()

                    Else
                        If iflag = 2 Then
                            btnSave.Attributes.Add("style", "display:none")
                            btnshow.Attributes.Add("style", "display:none")
                            expdel.Attributes.Add("style", "display:none")
                        End If
                        txtDO.Text = Dos
                        ddlFrom.Enabled = False
                        DdlSup.Enabled = False
                        'txtDO.ReadOnly = True
                        ddlFrom.SelectedValue = CCf
                        DdlSup.SelectedValue = Supid
                        txtLinkid.Text = Mtid
                        txtExp.Text = dExdeldate.ToString("dd MMM yyyy")
                        Dim dt, dt2 As New DataTable

                        dt = cInv.getDTInvRcv(Mtid, custid) 'WService.getDTInvRcv(Mtid, cust)

                        ViewState("InvRcv") = dt
                        BindData()
                        bindingstk()

                        If iflag = 2 And dCreate.Date < Now.Date Then
                            btnshow.Disabled = True
                            btnSave.Disabled = True
                            btnFinish.Disabled = True
                            DgvTransfer.Columns(12).Visible = False
                        Else
                            btnshow.Disabled = False
                            btnSave.Disabled = False
                            btnFinish.Disabled = False
                            'btnFinish.InnerText = "Update"
                        End If

                    End If

                    check_validation()


                End If

                ScriptManager.GetCurrent(Me).RegisterPostBackControl(RadButton1)
                ' ScriptManager.GetCurrent(Me).RegisterPostBackControl(btnSave2)
                'This for Search Button
                txtSearch.Attributes.Add("onkeyup", Me.Page.ClientScript.GetPostBackEventReference(txtSearch, ""))

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
            errLog(Err.Description, "Pages_InvOrderWDetailRcv:Page_Load")
            Err.Clear()
        End Try
    End Sub
    Protected Sub BindData()

        If custid() = 0 Then
            Response.Redirect("~/Default.aspx")
        Else
            DgvTransfer.DataSource = DirectCast(ViewState("InvRcv"), DataTable)
            DgvTransfer.DataBind()
            'UpdatePanel10.DataBind()
        End If

    End Sub

    Sub bindingstk()

        If custid() = 0 Then
            Response.Redirect("~/Default.aspx")
        Else
            Dim dt, dt2 As New DataTable
            dt = DirectCast(ViewState("InvRcv"), DataTable)
            dt2 = cInv.getDtStk(dt, custid, txtSearch.Text, modulepriv)
            'WService.getDtStk(dt, cust, txtSearch.Text, ModPrv)
            ViewState("InvStk") = dt2
            LsvStk.DataSource = DirectCast(ViewState("InvStk"), DataTable)
            LsvStk.DataBind()
        End If


    End Sub


    Protected Sub Page_unLoad(sender As Object, e As EventArgs) Handles Me.Unload
    End Sub

    Public Function username() As String
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        Return cookie("userName")

    End Function

    'Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAdd.Click
    '    Add()
    'End Sub

    'Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.ServerClick
    '    bindingstk()
    'End Sub
    Protected Sub RadButton1_ServerClick(sender As Object, e As EventArgs) Handles RadButton1.ServerClick
        If ddlFrom.SelectedValue <> 0 And DdlSup.SelectedValue <> 0 Then
            Add()
            Session("xsup") = DdlSup.SelectedValue
            Session("xfrom") = ddlFrom.SelectedValue
        Else
            notsuccesfull("you must choose Cost Centre first")
        End If
        'DgvTransfer.DataBind()
    End Sub
    Sub Add()

        Try
            Dim i As Integer = 0
            For Each item As ListViewItem In LsvStk.Items
                Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox1"), CheckBox)

                If ck.Checked Then
                    Try
                        Dim ds As DataSet = cInv.getStkByid((DirectCast(item.FindControl("lblID"), Label).Text), custid)
                        If ds.Tables(0).Rows.Count > 0 Then
                            Dim u1 As String = ds.Tables(0).Rows(0).Item("Unit1").ToString
                            Dim u2 As String = ds.Tables(0).Rows(0).Item("Unit2").ToString
                            Dim dt As DataTable = DirectCast(ViewState("InvRcv"), DataTable)
                            dt.Rows.Add(0, custid, Date.Now.ToString("yyyy-MM-dd"), 1, ddlFrom.SelectedValue, (DirectCast(item.FindControl("lblID"), Label).Text), (DirectCast(item.FindControl("lblName"), Label).Text), 0, 0, ds.Tables(0).Rows(0).Item("Factor1"), 0, 0, 0, "", 1, ds.Tables(0).Rows(0).Item("Unit1").ToString, ds.Tables(0).Rows(0).Item("Unit2").ToString, username, Date.Now.ToString("yyyy-MM-dd"), "", Date.Now.ToString("yyyy-MM-dd"), ds.Tables(0).Rows(0).Item("Number").ToString, ddlFrom.SelectedItem, 0)
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
            Next

        Catch ex As Exception
            'MsgBox(ex.Message)
        End Try

    End Sub



    Sub delete(ByVal id As Integer)
        'WService.DeleteInvRcv(id, cust, username)
        cInv.DeleteInvRcvDetail(id, custid, username)
    End Sub
    Sub deleteitem(ByVal id As Integer, ByVal stkid As Integer)
        'WService.DeleteInvRcv(id, cust, username)
        cInv.DeleteInvRcvitem(id, stkid, custid, username)
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


    Protected Sub RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow AndAlso DgvTransfer.EditIndex = e.Row.RowIndex Then

        End If
    End Sub

    Sub loadFrom()

        Dim ds As New DataSet
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

    End Sub

    Sub loadSup()

        Dim ds As New DataSet
        ds = cInv.getSupplier(custid)
        If ds IsNot Nothing AndAlso ds.Tables.Count > 0 Then
            DdlSup.DataSource = ds
        Else
            DdlSup.DataSource = Nothing
        End If
        DdlSup.DataValueField = "Id"
        DdlSup.DataTextField = "Name"
        DdlSup.DataBind()

        If DdlSup.Items.Count > 0 Then
            DdlSup.SelectedIndex = 0
        End If

    End Sub
    Sub insert(ByVal stat As Integer) 'As Boolean
        Dim hasil As Boolean = False
        Try
            Dim msg As String = ""
            Dim dt As DataTable = DirectCast(ViewState("InvRcv"), DataTable)
            Dim newLine As String = "\n"
            If dt.Rows.Count = 0 Then
                msg = msg + "Item is Nothing" & newLine
            End If
            If ddlFrom.SelectedValue = 0 Or ddlFrom.SelectedItem.Text = "" Then
                msg = msg + "Cost Centre Must be Choice " & newLine
            End If

            If DdlSup.SelectedValue = 0 Or DdlSup.SelectedItem.Text = "" Then
                msg = msg + "Supplier Must be Choice" & newLine
            End If
            For Each row As DataRow In dt.Rows
                If row.Item("ttlStock") = 0 Or row.Item("Cost") = 0 Then
                    msg = msg + row.Item("Stk_name") + " stock Or Cost Not Fill " & newLine
                    'notsuccesfull(msg)
                End If

            Next row

            If msg <> "" Then
                notsuccesfull(msg)
                Exit Sub
            Else
                txtLinkid.Text = cInv.insertInvRcv(txtLinkid.Text, ddlFrom.SelectedValue, ddlFrom.SelectedItem.Text, dt, txtDO.Text, DdlSup.SelectedValue, stat, username, custid, CDate(txtExp.Text), CDate(txtExp.Text), txtNotes.Text)  'WService.insertInvRcv(txtLinkid.Text, ddlFrom.SelectedValue, ddlFrom.SelectedItem.Text, dt, txtDO.Text, DdlSup.SelectedValue, stat, username, cust, CDate(txtExp.Text))

                hasil = True
            End If

            'txtLinkid.Text = WService.insertInvRcv(txtLinkid.Text, ddlFrom.SelectedValue, ddlFrom.SelectedItem.Text, dt, txtDO.Text, DdlSup.SelectedValue, stat, username, cust, CDate(txtExp.Text))
            '  ViewState("InvRcvEdit") = 0


            Response.Redirect("~/Pages_InvRcv.aspx")
        Catch ex As Exception

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
            DgvTransfer.EditIndex = e.RowIndex
            '  ViewState("InvRcvEdit") = 1

            'Dim lblID As Label = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("lblID"), Label)
            ''Dim indexrow As String = e.RowIndex.ToString
            'Dim lblstknm As LinkButton = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("lnkstkname"), LinkButton) 'DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("lnkstkname"), Label)
            'Dim lblstkid As Label = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("lblStkID"), Label)
            'Dim dt As DataTable = DirectCast(ViewState("InvRcv"), DataTable)
            'Dim dtstk As DataTable = DirectCast(ViewState("InvStk"), DataTable)
            'If lblID.Text = 0 Then
            '    dtstk.Rows.Add(lblstkid.Text, lblstknm.Text)
            '    ViewState("InvStk") = dtstk
            '    dt.Rows(e.RowIndex).Delete()
            '    dt.AcceptChanges()
            'Else
            '    dtstk.Rows.Add(lblstkid.Text, lblstknm.Text)
            '    ViewState("InvStk") = dtstk
            '    Dim confirmValue As String = Request.Form("confirm_value")
            '    If confirmValue = "Yes" Then

            '        Dim id As Integer = lblID.Text
            '        deleteitem(id, lblstkid.Text)
            '        'delete(id)

            '    Else
            '        Exit Sub
            '    End If
            '    dt.Rows(e.RowIndex).Delete()
            '    dt.AcceptChanges()
            '    ViewState("InvRcv") = dt
            'End If
            'bindingstk()
            'BindData()

            'UpdatePanel10.DataBind()

        Catch ex As Exception
            errLog(Err.Description, "InvRcvEdit:DgvTransfer_RowDeleting")
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

            Dim txtCost As Label = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("txtEditCost"), Label)

            Dim txtUnit2 As TextBox = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("txtEditUnit2"), TextBox)
            If IsNumeric(txtUnit2.Text) = False Then
                notsuccesfull("Unit 2 must be number")
                Exit Sub
            End If

            Dim txtEditU1Cost As TextBox = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("txtEditU1Cost"), TextBox)

            Dim clsI_stk As New clsInventory_stk

            Dim lblLosF1 As Label = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("lblStkEditID"), Label)
            lblLosF1.Text = clsI_stk.get_stkFactor1(lblLosF1.Text, custid)   'DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("lblEditfactor"), Label)

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
                Exit Sub
            End If

            If Val(txtUnit1.Text) >= 999999 Then
                notsuccesfull("Unit 1 must less than 999999")
                Exit Sub
            End If

            If Val(txtUnit2.Text) >= 999999 Then
                notsuccesfull("Unit 2 must less than 999999")
                Exit Sub
            End If

            If Val(total) >= 999999 Then
                notsuccesfull("Total Stock must less than 999999")
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



            dt.Rows(e.RowIndex)("Factor1") = lblLosF1.Text

            dt.Rows(e.RowIndex)("U1") = txtUnit1.Text

            dt.Rows(e.RowIndex)("U2") = txtUnit2.Text

            dt.Rows(e.RowIndex)("ttlStock") = total

            dt.Rows(e.RowIndex)("CostU1") = txtEditU1Cost.Text


            If lblLosF1.Text = 0 Then
                lblLosF1.Text = 1
            End If

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
        'Dim sb As System.Text.StringBuilder = New System.Text.StringBuilder()
        'sb.Append("<script type='text/javascript'>")
        'sb.Append("$('#detailModal').modal('show');")
        'sb.Append("</script>")
        'ClientScript.RegisterClientScriptBlock(Me.GetType(), "DetailModalScrip", sb.ToString())
        'Exit Sub
        ''ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "DetailModalScript", sb.ToString, False)
        'BindData()
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
            If iflag = 2 Then

                notsuccesfull("this order was complete")
                Exit Sub
            Else
                insert(1)
            End If

        Catch ex As Exception
            Dim myStringVariable As String = "fail to save"
            ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('" + myStringVariable + "');", True)

        End Try



        ' Response.Redirect("~/Pages_InvRcv.aspx")
    End Sub

    Protected Sub btnFinish_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnFinish.ServerClick


        'Dim c_cmd As New clsDbo("")
        'c_cmd.Process_InvDataStock(Date.Now.Date.ToString("yyyy-MM-dd"), Date.Now.Date.ToString("yyyy-MM-dd"))
        'Dim clsprocess As New clsInventoryProcess


        'If Mtid <> "" And (ViewState("InvRcvupdt") IsNot Nothing) Then
        'Dim dt As DataTable = DirectCast(ViewState("InvRcvupdt"), DataTable)

        'For i As Integer = 0 To dt.Rows.Count - 1
        '    Dim c_cmd As New clsDbo("")
        '    clsprocess.InvstkId = dt.Rows(i).Item("stk_id").ToString
        '    clsprocess.InvCc = dt.Rows(i).Item("cc").ToString
        '    clsprocess.InvCcName = dt.Rows(i).Item("cc_name").ToString
        '    clsprocess.InvCustId = custid()
        '    clsprocess.InvstkName = dt.Rows(i).Item("Stk_Name").ToString
        '    clsprocess.Invrcvqty = -c_cmd.ExecuteScalar("SELECT ttlstock FROM cld_inv_trans WHERE linkid='" & Mtid & "' and stk_id = '" & dt.Rows(i).Item("stk_id").ToString & "' and cc = '" & dt.Rows(i).Item("cc").ToString & "' and custid = " & custid() & "", 0)
        '    clsprocess.InvStkNo = cInv.getStkByid(dt.Rows(i).Item("stk_id").ToString, custid).Tables(0).Rows(0).Item("Number").ToString
        '    clsprocess.InvDate = Date.Now.Date.ToString("yyyy-MM-dd 00:00:00")
        '    clsprocess.executeNonQuery_Inv_ProcessCls()


        '    clsprocess.InvstkId = dt.Rows(i).Item("stk_id").ToString
        '    clsprocess.InvCc = dt.Rows(i).Item("cc").ToString
        '    clsprocess.InvCcName = dt.Rows(i).Item("cc_name").ToString
        '    clsprocess.InvCustId = custid()
        '    clsprocess.InvstkName = dt.Rows(i).Item("Stk_Name").ToString
        '    clsprocess.Invrcvqty = dt.Rows(i).Item("ttlStock").ToString
        '    clsprocess.InvStkNo = cInv.getStkByid(dt.Rows(i).Item("stk_id").ToString, custid).Tables(0).Rows(0).Item("Number").ToString
        '    clsprocess.InvDate = Date.Now.Date.ToString("yyyy-MM-dd 00:00:00")
        '    clsprocess.executeNonQuery_Inv_ProcessCls()
        'Next i

        ' Else
        'Dim dt As DataTable = DirectCast(ViewState("InvRcv"), DataTable)

        '    For i As Integer = 0 To dt.Rows.Count - 1
        '        clsprocess.InvstkId = dt.Rows(i).Item("stk_id").ToString
        '        clsprocess.InvCc = dt.Rows(i).Item("cc").ToString
        '        clsprocess.InvCcName = dt.Rows(i).Item("cc_name").ToString
        '        clsprocess.InvCustId = custid()
        '        clsprocess.InvstkName = dt.Rows(i).Item("Stk_Name").ToString
        '        clsprocess.Invrcvqty = dt.Rows(i).Item("ttlStock").ToString
        '        clsprocess.InvStkNo = cInv.getStkByid(dt.Rows(i).Item("stk_id").ToString, custid).Tables(0).Rows(0).Item("Number").ToString
        '        clsprocess.InvDate = Date.Now.Date.ToString("yyyy-MM-dd 00:00:00")
        '        'cInv.executeNonQuery_Inv_ProcessCls(Date.Now.Date.ToString("yyyy-MM-dd"), ddlFrom.SelectedValue, ddlFrom.SelectedItem.Text, stkid, stknumber, stkname, custid, 0, stktotal)
        '        clsprocess.executeNonQuery_Inv_ProcessCls()
        '    Next i

        'End If



        ' c_cmd.executeNonQuery_Inv_ProcessCls_current(Date.Now, custid)
        insert(2) 'Then
        'Dim myStringVariable As String = "Save Success"
        '    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('" + myStringVariable + "');", True)

        'Response.Redirect("~/Pages_InvRcv.aspx")
        'Else
        '    notsuccesfull("Please check again!")
        'End If


    End Sub



    Private Sub txtDO_TextChanged(sender As Object, e As EventArgs) Handles txtDO.TextChanged
        check_validation()
    End Sub

    Protected Sub DdlSup_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DdlSup.SelectedIndexChanged
        Dim dt As DataTable = DirectCast(ViewState("InvRcv"), DataTable)
        Try
            If dt.Rows.Count > 0 And DdlSup.SelectedValue <> Session("xsup").ToString Then
                notsuccesfull("you change supplier !")
                'ddlFrom.SelectedValue = Session("xfrom").ToString
            Else
                check_validation()
            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub ddlFrom_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlFrom.SelectedIndexChanged
        Dim dt As DataTable = DirectCast(ViewState("InvRcv"), DataTable)
        Try
            If dt.Rows.Count > 0 And ddlFrom.SelectedValue <> Session("xfrom").ToString Then
                notsuccesfull("you must delete item before change")
                ddlFrom.SelectedValue = Session("xfrom").ToString
            Else
                check_validation()
            End If
        Catch ex As Exception

        End Try

    End Sub
    Private Sub check_validation()
        If txtDO.Text <> "" And ddlFrom.SelectedIndex >= 0 And DdlSup.SelectedIndex >= 0 Then
            RadButton1.Disabled = False
        Else
            RadButton1.Disabled = True
        End If
    End Sub

    Protected Sub btnBack_ServerClick(sender As Object, e As EventArgs) Handles btnBack.ServerClick
        Response.Redirect("~/Pages_InvRcv.aspx")
    End Sub
    Protected Sub btnClearSearch_Click(sender As Object, e As EventArgs)
        For Each item As ListViewItem In LsvStk.Items
            Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox1"), CheckBox)

            ck.Checked = False


        Next
        txtSearch.Text = ""
        lblnoitem.Text = 0

        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Pop", "openModalItem();", True)
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
    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        Threading.Thread.Sleep(500)
        BindCheckItem()
        bindingstk()
        CheckedItem()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Pop", "openModalItem();", True)
        txtSearch.Attributes("onfocus") = "javascript:this.setSelectionRange(this.value.length,this.value.length);"
        txtSearch.Focus()
    End Sub
    'Protected Sub imgbtndetails_Click(ByVal sender As Object, ByVal e As EventArgs)
    '    DetailsView1.DataSource = DirectCast(ViewState("InvRcv"), DataTable)
    '    DetailsView1.DataBind()
    '    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Pop", "openModalItem();", True)

    'End Sub
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

        End Try
    End Sub
    'Protected Sub gvContact_PreRender(ByVal sender As Object, ByVal e As EventArgs)
    '    Dim gv As GridView = CType(sender, GridView)

    '    If (gv.ShowHeader = True AndAlso gv.Rows.Count > 0) OrElse (gv.ShowHeaderWhenEmpty = True) Then
    '        gv.HeaderRow.TableSection = TableRowSection.TableHeader
    '    End If

    '    If gv.ShowFooter = True AndAlso gv.Rows.Count > 0 Then
    '        gv.FooterRow.TableSection = TableRowSection.TableFooter
    '    End If
    'End Sub
    Protected Sub DgvTransfer_RowCommand(sender As Object, e As GridViewCommandEventArgs) 'Handles DgvTransfer.RowCommand
        'Dim index As Integer = Convert.ToInt32(e.CommandArgument)
        Dim stkid As Integer = Convert.ToInt32(e.CommandArgument)
        Dim dt As DataTable = ViewState("InvRcv")
        dt.DefaultView.RowFilter = "stk_id='" & stkid & "'"
        dt.AcceptChanges()

        If e.CommandName.Equals("detail") Then
            DetailsView1.DataSource = dt.DefaultView

            DetailsView1.DataBind()
            Dim sb As New System.Text.StringBuilder()
            Dim url As String = "~/Pages_InvTransfer.aspx"
            sb.Append("<script type = 'text/javascript'>")
            sb.Append("$('#detailModal').modal('show');")
            sb.Append("</script>")

            ScriptManager.RegisterClientScriptBlock(Me, [GetType](), "DetailModalScript", sb.ToString(), False)

        End If
        If e.CommandName.Equals("Edit") Then
            Dim dr As DataRowView = dt.DefaultView(0)

            lblstk_name.Text = dr.Item("stk_name").ToString
            txtU1.Text = dr.Item("U1").ToString
            lblU1_nm.Text = dr.Item("U1_nm").ToString
            txtU2.Text = dr.Item("U2").ToString
            lblU2_nm.Text = dr.Item("U2_nm").ToString
            lblfactor1.Text = dr.Item("factor1").ToString
            lblttlStock.Text = dr.Item("ttlStock").ToString
            txtCostU1.Text = dr.Item("CostU1").ToString
            lblCost.Text = dr.Item("Cost").ToString
            txtCostU1.Text = dr.Item("CostU1").ToString
            lblttlCost.Text = dr.Item("ttlCost").ToString
            lblResult.Visible = False
            Dim sb As New System.Text.StringBuilder()
            Dim url As String = "~/Pages_InvTransfer.aspx"
            sb.Append("<script type='text/javascript'>")
            sb.Append("$('#editModal').modal('show');")
            sb.Append("</script>")
            ScriptManager.RegisterClientScriptBlock(Me, [GetType](), "EditModalScript", sb.ToString(), False)
            'txtU1.Focus()
            'sb.Append("$('#detailModal').modal('show');")
            'sb.Append("</script>")

            'ScriptManager.RegisterClientScriptBlock(Me, [GetType](), "DetailModalScript", sb.ToString(), False)
        End If
        If e.CommandName.Equals("Delete") Then

            Dim sb As New System.Text.StringBuilder()
            Dim url As String = "~/Pages_InvTransfer.aspx"
            sb.Append("<script type = 'text/javascript'>")
            sb.Append("$('#confirmModal').modal('show');")
            sb.Append("</script>")

            ScriptManager.RegisterClientScriptBlock(Me, [GetType](), "ConfirmModalScript", sb.ToString(), False)


        End If
    End Sub
    'Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As EventArgs)
    '    Dim sb As System.Text.StringBuilder = New System.Text.StringBuilder()
    '    sb.Append("<script type='text/javascript'>")
    '    sb.Append("$('#editModal').modal('show');")
    '    sb.Append("</script>")
    '    ScriptManager.RegisterClientScriptBlock(Me, Me.[GetType](), "AddShowModalScript", sb.ToString(), False)
    'End Sub
    Protected Sub btnSave2_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim rowIndex As Integer = DgvTransfer.EditIndex

        'Dim txtUnit1 As TextBox = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("txtEditUnit1"), TextBox)
        If IsNumeric(txtU1.Text) = False Then
            notsuccesfull("Unit 1 must be number")
            Exit Sub
        End If

        ' Dim txtCost As Label = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("txtEditCost"), Label)

        'Dim txtUnit2 As TextBox = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("txtEditUnit2"), TextBox)
        If IsNumeric(txtU2.Text) = False Then
            notsuccesfull("Unit 2 must be number")
            Exit Sub
        End If

        'Dim txtEditU1Cost As TextBox = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("txtEditU1Cost"), TextBox)

        Dim clsI_stk As New clsInventory_stk

        Dim lblLosF1 As Label = DirectCast(DgvTransfer.Rows(rowIndex).FindControl("lblStkEditID"), Label)
        lblLosF1.Text = clsI_stk.get_stkFactor1(lblLosF1.Text, custid)   'DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("lblEditfactor"), Label)

        If IsNumeric(txtU1.Text) = False Then
            txtU1.Text = 0
        End If

        If IsNumeric(txtU2.Text) = False Then
            txtU2.Text = 0
        End If

        If IsNumeric(txtCostU1.Text) = False Then
            txtCostU1.Text = 0
        End If

        'If txtEditU1Cost.Text = 0 Or txtUnit1.Text = 0 Then
        '    notsuccesfull("Please Input Unit 1 and Cost Unit 1")
        '    Exit Sub
        'End If

        Dim total As Decimal = (Val(txtU1.Text) * Val(lblLosF1.Text)) + Val(txtU2.Text)

        'tambahan erik 21 sept 2020, buat jagain nilai tidak tll besar
        If txtCostU1.Text = 0 Then
            notsuccesfull("Please Input Cost Unit 1")
            Exit Sub
        End If

        If Val(txtU1.Text) >= 999999 Then
            notsuccesfull("Unit 1 must less than 999999")
            Exit Sub
        End If

        If Val(txtU2.Text) >= 999999 Then
            notsuccesfull("Unit 2 must less than 999999")
            Exit Sub
        End If

        If Val(total) >= 999999 Then
            notsuccesfull("Total Stock must less than 999999")
            Exit Sub
        End If

        If Val(txtCostU1.Text) >= 999999999 Then
            notsuccesfull("Cost Must less than 999999999")
            Exit Sub
        End If

        Dim lblUnit1 As Label = DirectCast(DgvTransfer.Rows(rowIndex).FindControl("lblEditEmpID"), Label)

        Dim dt As DataTable = DirectCast(ViewState("InvRcv"), DataTable)

        dt.Rows(rowIndex)("Factor1") = lblLosF1.Text

        dt.Rows(rowIndex)("U1") = txtU1.Text

        dt.Rows(rowIndex)("U2") = txtU2.Text

        dt.Rows(rowIndex)("ttlStock") = total

        dt.Rows(rowIndex)("CostU1") = txtCostU1.Text


        If lblLosF1.Text = 0 Then
            lblLosF1.Text = 1
        End If

        Dim cost As Decimal = txtCostU1.Text / lblLosF1.Text

        dt.Rows(rowIndex)("Cost") = cost

        Dim ttlcost As Decimal = total * cost
        dt.Rows(rowIndex)("ttlCost") = ttlcost

        'If Mtid <> "" Then
        '    dtupdt.Rows(maxrowup)("Cost") = cost
        '    dtupdt.Rows(maxrowup)("ttlCost") = ttlcost
        '    dtupdt.AcceptChanges()

        '    ViewState("InvRcvupdt") = dtupdt
        'End If

        dt.AcceptChanges()




        Dim sb As System.Text.StringBuilder = New System.Text.StringBuilder()
        sb.Append("<script type='text/javascript'>")
        sb.Append("$('#editModal').modal('hide');")
        sb.Append("</script>")
        ScriptManager.RegisterClientScriptBlock(Me, Me.[GetType](), "Update HideModalScript", sb.ToString(), False)
        DgvTransfer.EditIndex = -1
        BindData()
        UpdatePanel10.DataBind()
    End Sub

    Protected Sub btnok_Click(sender As Object, e As EventArgs) 'Handles btnok.Click
        Dim rowIndex As Integer = DgvTransfer.EditIndex
        Dim lblID As Label = DirectCast(DgvTransfer.Rows(rowIndex).FindControl("lblEditID"), Label)
        'Dim indexrow As String = e.RowIndex.ToString
        Dim lblstknm As LinkButton = DirectCast(DgvTransfer.Rows(rowIndex).FindControl("lnkstkname"), LinkButton) 'DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("lnkstkname"), Label)
        Dim lblstkid As Label = DirectCast(DgvTransfer.Rows(rowIndex).FindControl("lblStkEditID"), Label)
        Dim dt As DataTable = DirectCast(ViewState("InvRcv"), DataTable)
        Dim dtstk As DataTable = DirectCast(ViewState("InvStk"), DataTable)
        If lblID.Text = 0 Then
            dtstk.Rows.Add(lblstkid.Text, lblstknm.Text)
            ViewState("InvStk") = dtstk
            dt.Rows(rowIndex).Delete()
            dt.AcceptChanges()
        Else
            dtstk.Rows.Add(lblstkid.Text, lblstknm.Text)
            ViewState("InvStk") = dtstk
            Dim confirmValue As String = Request.Form("confirm_value")
            'If confirmValue = "Yes" Then

                Dim id As Integer = lblID.Text
                deleteitem(id, lblstkid.Text)
                'delete(id)

                'Else
                '    Exit Sub
                'End If
                dt.Rows(rowIndex).Delete()
            dt.AcceptChanges()
            ViewState("InvRcv") = dt
        End If
        bindingstk()
        BindData()

        UpdatePanel10.DataBind()
    End Sub
End Class
