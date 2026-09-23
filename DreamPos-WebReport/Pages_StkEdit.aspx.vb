Imports System.Threading
Imports System.Net
Imports System.Text
Imports System.IO

Public Class Pages_StkEdit
    Inherits System.Web.UI.Page
    Private sNameuser As String
    Private Userid As Integer
    Private icustid As Integer
    Private imodul As Integer
    Private iModPriv As Integer
    Private irptpriv As Integer
    Private irptf As Integer
    Private mfunction As New ModFunction
    Private MTid As Integer
    'Private ccid As Integer
    Private imgByte As Byte() = Nothing

    Private CRcp As New Cls_IRcp
    Private cStock As New clsInventory_stk
    Private cStockGrp As New Cls_IStkGroup
    Private CIcc As New Cls_ICc
    Dim cInv As New clsInventory
    Dim msg As String = ""
    Dim newLine As String = "\n"
    Private Crcpdtl As New Cls_IRcpDtl


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
        MTid = Request.Cookies("InvStk_ID").Value
        'ccid = Request.Cookies("InvStk_cc").Value
        Try

            If Request.Cookies("UserLogin_inv") IsNot Nothing Then
                sNameuser = username()
                icustid = custid()
                imodul = modul()
                iModPriv = modulepriv()
                irptpriv = rptpriv()
                irptf = rptf()
                If IsPostBack = False Then
                    If MTid = 0 Then
                        hapus()
                        loadStkGrp()
                        BindingRcp1()
                        'loadRcp()
                        'loadCc()
                    Else
                        loadStkGrp()
                        editlist(MTid)
                        bindingrcp()
                        'loadRcp()
                    End If
                    'ScriptManager.GetCurrent(Me).RegisterPostBackControl(btnInsert)
                    'ScriptManager.GetCurrent(Me).RegisterPostBackControl(RadButton1)
                End If
                ScriptManager.GetCurrent(Me).RegisterPostBackControl(RadButton1)
                'ScriptManager.GetCurrent(Me).RegisterPostBackControl(txtName)
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

    Sub hapus()

        txtName.Text = ""
        txtNo.Text = ""
        txtUnit1.Text = ""
        txtUnit2.Text = ""
        txtUnit3.Text = ""

        'txtStkLimit.Text = 0


        txtFac1.Text = 1
        txtFac2.Text = 1

        txtNo.Enabled = True

        btnUpdate.Visible = False
        btnInsert.Visible = True
        ViewState("codeawal") = ""
    End Sub

    Sub editlist(ByVal id As Integer)


        Dim ds As New DataSet
        ds = cStock.SelectDataset_StkEdit(id, custid)     'WService.getStkEdit(id, cust)
        If ds.Tables(0).Rows.Count = 0 Then
            Exit Sub
        End If

        txtNo.Text = ds.Tables(0).Rows(0).Item("number")
        ViewState("codeawal") = ds.Tables(0).Rows(0).Item("number")
        txtName.Text = ds.Tables(0).Rows(0).Item("Name")
        'ddlcc.SelectedValue = ds.Tables(0).Rows(0).Item("ccid")
        txtUnit1.Text = ds.Tables(0).Rows(0).Item("Unit1")
        txtUnit2.Text = ds.Tables(0).Rows(0).Item("Unit2")
        txtUnit3.Text = ds.Tables(0).Rows(0).Item("Unit3")
        txtFac1.Text = ds.Tables(0).Rows(0).Item("Factor1")
        txtFac2.Text = ds.Tables(0).Rows(0).Item("Factor2")
        txtcost.Text = ds.Tables(0).Rows(0).Item("cost")
        lblcost.Text = " /" & ds.Tables(0).Rows(0).Item("Unit2").ToString

        'If My.Settings.fixcost <> 0 Then
        '    txtfcost.Text = ds.Tables(0).Rows(0).Item("fcost")
        '    lblfcost.Text = " /" & ds.Tables(0).Rows(0).Item("Unit2").ToString
        '    f_cost.Visible = True
        'Else
        f_cost.Visible = False
        'End If

        'txtprice1.Text = ds.Tables(0).Rows(0).Item("p1")
        'If txtprice1.Text = 0 Then
        '    btnprice1.Visible = True
        '    btnprice1.InnerText = "margin " & My.Settings.price1 & " % "
        'End If
        'lblprice1.Text = "margin " & My.Settings.price1 & " % "
        'txtprice2.Text = ds.Tables(0).Rows(0).Item("p2")
        'If txtprice2.Text = 0 Then
        '    btnprice2.Visible = True
        '    btnprice2.InnerText = "margin " & My.Settings.price2 & " % "
        'End If
        'lblprice2.Text = "margin " & My.Settings.price2 & " % "
        DdlGrpStk.SelectedValue = ds.Tables(0).Rows(0).Item("grpcode")
        If Not IsDBNull(ds.Tables(0).Rows(0).Item("Blink")) Then

            'DdlLink.SelectedValue = ds.Tables(0).Rows(0).Item("Blink").ToString

        End If

        'txtStkLimit.Text = ds.Tables(0).Rows(0).Item("Stklimit")


        If Not IsDBNull(ds.Tables(0).Rows(0).Item("Batch")) Then


            chkbatch.Checked = ds.Tables(0).Rows(0).Item("Batch").ToString
        End If

        If chkbatch.Checked = True Then
            'DdlLink.Enabled = True
            btnaddcc.Disabled = False
        End If
        'txtTelp.Text = ds.Tables(0).Rows(0).Item("Telp")
        'txtRemarks.Text = ds.Tables(0).Rows(0).Item("remarks")
        'txtotype.Text = ds.Tables(0).Rows(0).Item("otype")
        'TxtCust.Text = ds.Tables(0).Rows(0).Item("CustID")
        binding1()
        'bindingrcp()
        btnUpdate.Visible = True
        btnInsert.Visible = False
    End Sub

    Protected Sub btnInsert_Click(ByVal sender As Object, ByVal e As EventArgs) 'Handles btnInsert.ServerClick
        Try
            If IsNumeric(txtFac1.Text) = False Then
                ' txtFac1.Text = 1
                errfac1.Text = "Value must be a whole number"

                errfac1.Visible = True
            Else
                errfac1.Visible = False
                If txtFac1.Text = "" Then
                    txtFac1.Text = 1
                ElseIf txtFac1.Text < 0 Then
                    txtFac1.Text = 1
                ElseIf txtFac1.Text = 0 Then
                    msg = msg + "factor1 can't 0"
                End If
            End If
            If IsNumeric(txtFac2.Text) = False Then
                'txtFac2.Text = 1
                errfac2.Text = "Value must be a whole number"

                errfac2.Visible = True
            Else
                errfac2.Visible = False
                If txtFac2.Text = "" Then

                    txtFac2.Text = 1
                ElseIf txtFac2.Text < 0 Then
                    txtFac2.Text = 1
                ElseIf txtFac2.Text = 0 Then
                    msg = msg + "factor2 can't 0"
                End If
            End If
            If txtNo.Text = "" Or txtName.Text = "" Then
                msg = msg + "number and Name must be fill" & newLine
            End If
            'If (errfac1.Visible = True Or errfac2.Visible = True) Then
            '    Exit Sub
            'End If
            Dim c_cmd As New clsDbo("")

            If c_cmd.ExecuteScalar("Select ID from cld_inv_stk where Number = '" & txtNo.Text & "'", 2) > 0 Then
                msg = msg + "number already exist" & newLine
            End If


            'If cekbatch() = False Then

            '    ' msg = msg + " if batch, you must choose Recipe Link" & newLine
            '    BootstrapAlert(lblMsg, "This Stock will set to No Recipe !!", BootstrapAlertType.Warning, True)
            '    lblMsg.Visible = True
            'End If
            If msg = "" Then
                insert()
            Else

                notsuccesfull(msg)
                Exit Sub

            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As EventArgs) ' Handles btnUpdate.ServerClick

        Try
            If IsNumeric(txtFac1.Text) = False Then
                ' txtFac1.Text = 1
                errfac1.Text = "Value must be a whole number"

                errfac1.Visible = True
            Else
                errfac1.Visible = False
                If txtFac1.Text = "" Then
                    txtFac1.Text = 1
                ElseIf txtFac1.Text < 0 Then
                    txtFac1.Text = 1
                ElseIf txtFac1.Text = 0 Then
                    msg = msg + "factor1 can't 0"
                    errfac1.Text = "factor1 can't 0"
                    errfac1.Visible = True
                End If
            End If
            If IsNumeric(txtFac2.Text) = False Then
                'txtFac2.Text = 1
                errfac2.Text = "Value must be a whole number"

                errfac2.Visible = True
            Else
                errfac2.Visible = False
                If txtFac2.Text = "" Then

                    txtFac2.Text = 1
                ElseIf txtFac2.Text < 0 Then
                    txtFac2.Text = 1
                ElseIf txtFac2.Text = 0 Then
                    msg = msg + "factor2 can't 0"
                    errfac2.Text = "factor2 can't 0"
                    errfac2.Visible = True
                End If
            End If
            If txtNo.Text = "" Or txtName.Text = "" Then
                msg = "number and Name must be fill"
            End If
            'If (errfac1.Visible = True Or errfac2.Visible = True) Then
            '    Exit Sub
            'End If
            'If cekbatch() = False Then

            'msg = msg + " if batch, you must choose Recipe Link" & newLine
            'BootstrapAlert(lblMsg, "This Stock will set to No Recipe !!", BootstrapAlertType.Warning, True)
            'lblMsg.Visible = True
            'End If
            If msg = "" Then
                insert()
            Else

                notsuccesfull(msg)
                Exit Sub

            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancel.ServerClick

        Try
            hapus()
            mfunction.CreateCookie("ID", 0)

            Response.Redirect("~/Pages_Stk.aspx")



        Catch ex As Exception
            errLog(Err.Description, "Pages_StkEdit:btnCancel_Click")
            Err.Clear()
        End Try

    End Sub
    Sub loadStkGrp()
        Try

            'Dim cookie As HttpCookie = Request.Cookies("UserName")

            'Dim Cust As Integer = cookie("CustID")
            Dim ds As New DataSet
            ds = cStockGrp.SelectDataset_StkGroup(custid())   'WService.getStkGroup(cust())
            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 Then
                DdlGrpStk.DataSource = ds
            Else
                DdlGrpStk.DataSource = Nothing
            End If
            DdlGrpStk.DataValueField = "Code"
            DdlGrpStk.DataTextField = "Name"
            DdlGrpStk.DataBind()

            'DdlGrpStk.DataSource = ds
            'DdlGrpStk.DataValueField = "Id"
            'DdlGrpStk.DataTextField = "Name"
            'DdlGrpStk.DataBind()

        Catch ex As Exception
            errLog(Err.Description, "Pages_StkEdit:loadStkGrp")
            Err.Clear()
        End Try

    End Sub
    'Sub loadRcp()
    '    'Dim cookie As HttpCookie = Request.Cookies("UserName")

    '    'Dim Cust As Integer = cookie("CustID")
    '    Dim ds As New DataSet
    '    ds = CRcp.SelectDataset_rcp(custid)
    '    DdlLink.DataSource = ds
    '    DdlLink.DataValueField = "Id"
    '    DdlLink.DataTextField = "Name"
    '    DdlLink.DataBind()

    'End Sub
    Protected Sub chkbatch_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkbatch.CheckedChanged
        If chkbatch.Checked = True Then
            'DdlLink.Enabled = True
            btnaddcc.Disabled = False
        Else
            btnaddcc.Disabled = True
            'DdlLink.Enabled = False
            'DdlLink.SelectedValue = 0
        End If
    End Sub

    Sub insert()



        Dim act As Boolean
        If chkbatch.Checked = True Then
            act = True
        Else
            act = False
        End If
        Try

            If cStock.cekcode(MTid, txtNo.Text) = False And cStock.cekname(MTid, txtName.Text) = False Then

            Else
                notsuccesfull("Code or Name already exists")
                Exit Sub
            End If

            If txtNo.Text <> ViewState("codeawal").ToString And ViewState("codeawal").ToString <> "" Then
                ClientScript.RegisterStartupScript(Me.GetType(), "Javascript", "javascript:Confirmcode(); ", True)
                '(Me.GetType(), "alertMessage", "alert(" & msg & ")", True)
                Dim confirmValue As String = Request.Form("confirm_code")
                If confirmValue = "Yes" Then
                    cStock.insertStk(MTid, custid, txtNo.Text, txtName.Text, DdlGrpStk.SelectedValue, txtUnit1.Text,
         txtUnit2.Text, txtUnit3.Text, txtFac1.Text, txtFac2.Text, act, "0", "0", "0", username)
                    'If My.Settings.fixcost > 0 Then
                    '    cStock.insertfcost_Stk(MTid, custid, txtfcost.Text, username, DateTime.Now)

                    'End If
                Else
                    Exit Sub
                End If
            Else
                'If cStock.cekcode(MTid, txtNo.Text) = False And cStock.cekname(MTid, txtName.Text) = False Then
                cStock.insertStk(MTid, custid, txtNo.Text, txtName.Text, DdlGrpStk.SelectedValue, txtUnit1.Text,
          txtUnit2.Text, txtUnit3.Text, txtFac1.Text, txtFac2.Text, act, "0", "0", "0", username)

                'If My.Settings.fixcost > 0 Then
                '    cStock.insertfcost_Stk(MTid, custid, txtfcost.Text, username, DateTime.Now)

                'End If
                If ViewState("rcpstk") IsNot Nothing Then
                    dtrcpstk = DirectCast(ViewState("rcpstk"), DataTable)
                    If dtrcpstk.Rows.Count > 0 Then
                        insertrcp()
                    End If
                End If
                'Else
                '    notsuccesfull("Code or Name already exists")
                '    Exit Sub
                'End If

            End If


            Response.Redirect("~/Pages_Stk.aspx", False)

        Catch ex As Exception
            'MsgBox(ex.Message)
        End Try

    End Sub
    Protected Sub txtFac1_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtFac1.TextChanged

        If IsNumeric(txtFac1.Text) = False Then
            ' txtFac1.Text = 1
            errfac1.Text = "Value must be a whole number"

            errfac1.Visible = True
        Else
            errfac1.Visible = False
            If txtFac1.Text = "" Then
                txtFac1.Text = 1
            ElseIf txtFac1.Text < 0 Then
                txtFac1.Text = 1
            End If
        End If


    End Sub

    Protected Sub txtFac2_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtFac2.TextChanged


        If IsNumeric(txtFac2.Text) = False Then
            'txtFac2.Text = 1
            errfac2.Text = "Value must be a whole number"

            errfac2.Visible = True
        Else
            errfac2.Visible = False
            If txtFac2.Text = "" Then

                txtFac2.Text = 1
            ElseIf txtFac2.Text < 0 Then
                txtFac2.Text = 1
            End If
        End If


    End Sub

    'Protected Sub btnlimit_Click(sender As Object, e As EventArgs) Handles btnlimit.Click
    '    Try
    '        mfunction.CreateCookie("ID", MTid)

    '        Response.Redirect("~/Pages_Stk.aspx")
    '    Catch ex As Exception

    '    End Try
    'End Sub
    'Sub loadCc()
    '    ddlcc.Items.Clear()
    '    ddlcc.SelectedValue = Nothing
    '    Dim ds As New DataSet
    '    ds = CIcc.SelectDataset_Icc(custid, username, modulepriv, 0)
    '    ddlcc.DataSource = ds
    '    ddlcc.DataValueField = "Id"
    '    ddlcc.DataTextField = "Name"
    '    ddlcc.DataBind()
    '    If ddlcc.Items.Count > 0 Then
    '        ddlcc.SelectedIndex = 0
    '    End If
    'End Sub
    'Protected Function cekbatch() As Boolean
    '    Dim hasil As Boolean = False
    '    If Not (chkbatch.Checked = True And DdlLink.SelectedValue = 0) Then


    '        hasil = True
    '    End If
    '    Return hasil
    'End Function
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
    Public Sub Showalert(msg As String)
        Dim message As String = msg
        Dim sb As New System.Text.StringBuilder()
        ' Dim url As String = "~/Pages_InvTransfer.aspx"
        sb.Append("<script type = 'text/javascript'>")
        sb.Append("window.onload=function(){")
        sb.Append("alert('")
        sb.Append(message)
        sb.Append("')};")
        sb.Append("</script>")
        ClientScript.RegisterStartupScript(Me.GetType(), "Alert", sb.ToString())
    End Sub
    Public Shared Sub BootstrapAlert(MsgLabel As Label, Message As String, Optional MessageType As BootstrapAlertType = BootstrapAlertType.Plain, Optional Dismissable As Boolean = False)
        Dim style As String = "success", icon As String = "check"
        Select Case MessageType
            Case BootstrapAlertType.Plain
                style = "default"
                icon = ""
            Case BootstrapAlertType.Success
                style = "success"
                icon = "check"
            Case BootstrapAlertType.Information
                style = "info"
                icon = "info-circle"
            Case BootstrapAlertType.Warning
                style = "warning"
                icon = "warning"
            Case BootstrapAlertType.Danger
                style = "danger"
                icon = "remove"
            Case BootstrapAlertType.Primary
                style = "primary"
                icon = "info"
        End Select

        If (Not MsgLabel.Page.IsPostBack Or MsgLabel.Page.IsPostBack) And Message = Nothing Then
            MsgLabel.Visible = False
        Else
            MsgLabel.Visible = True
            MsgLabel.CssClass = "alert alert-" & style & If(Dismissable = True, " alert-dismissible fade show", "") 'in font2
            MsgLabel.Text = "<i class='fa fa-" & icon & "'></i>" & Message
            If Dismissable = True Then
                MsgLabel.Text &= "<button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>&times; </span></button>"
            End If
            MsgLabel.Focus()
            Message = ""
        End If
    End Sub

    Protected Sub lstViewRcp_ItemCommand(sender As Object, e As ListViewCommandEventArgs) Handles lstViewRcp.ItemCommand
        Select Case (e.CommandName)
            Case "Del"

                Dim confirmValue As String = Request.Form("confirm_value")
                If confirmValue = "Yes" Then

                    Dim id As Integer = Convert.ToInt32(e.CommandArgument)

                    delete(id)

                Else
                    Exit Sub
                End If


            Case "Edt"

                Dim arg As String() = e.CommandArgument.ToString().Split(";")


                mfunction.CreateCookie("rcpIdrcp", arg(0))
                mfunction.CreateCookie("rcptipe", arg(1))
                mfunction.CreateCookie("rcpID", MTid)
                mfunction.CreateCookie("rcpunt", arg(2))

                Response.Redirect("~/Pages_ReceipeDtlEdit.aspx", False)

        End Select
    End Sub

    Public Enum BootstrapAlertType
        Plain
        Success
        Information
        Warning
        Danger
        Primary
    End Enum

    Sub delete(ByVal id As Integer)
        Try
            DeleteRcpStk(MTid, custid, id)
            'bindingrcp()
            BindingRcp1()
            binding1()
            'binding(MTid)
        Catch ex As Exception

        End Try

    End Sub
    Protected Sub OnPageRcpChanging(ByVal sender As Object, ByVal e As PagePropertiesChangingEventArgs)
        lstViewRcpPager1.SetPageProperties(e.StartRowIndex, e.MaximumRows, False)

        binding2()
    End Sub
    Sub binding2() 'ambil cc mana saja di itemxcc

        Dim c_rcp As New Cls_IRcp
        Dim dtrcpstk As New DataTable

        'Dim ds As New DataSet
        'ds = WService.getItemMaster(cust)
        If custid() = 0 Then
            Response.Redirect("~/Default.aspx", False)
        Else
            dtrcpstk = DirectCast(ViewState("rcpstk"), DataTable)
            lstViewRcp.DataSource = dtrcpstk
            lstViewRcp.DataBind()
        End If
        'ds = c_rcp.SelectDataset_rcpCost(custid, txtNo.Text)



    End Sub
    Sub binding1() 'ambil cc mana saja di itemxcc

        Dim c_rcp As New Cls_IRcp
        Dim dtrcpstk As New DataTable

        'Dim ds As New DataSet
        'ds = WService.getItemMaster(cust)
        If custid() = 0 Then
            Response.Redirect("~/Default.aspx", False)
        Else
            dtrcpstk = CRcp.SelectDataRcpStk(txtNo.Text).Tables(0)
            ViewState("rcpstk") = dtrcpstk
            lstViewRcp.DataSource = DirectCast(ViewState("rcpstk"), DataTable)
            lstViewRcp.DataBind()
        End If
        'ds = c_rcp.SelectDataset_rcpCost(custid, txtNo.Text)



    End Sub

    Protected Sub Searchrcp_Click(sender As Object, e As EventArgs) Handles btnSearchrcp.ServerClick
        bindingrcp()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Pop", "openModalItem();", True)
    End Sub

    Protected Sub btnClearSearchrcp_Click(sender As Object, e As EventArgs) Handles btnClearSearchrcp.ServerClick

    End Sub
    Protected Sub RadButton1_ServerClick(sender As Object, e As EventArgs) Handles RadButton1.ServerClick

        Add2()
        'DgvTransfer.DataBind()
    End Sub
    Sub Add2()
        'untuk nambah record ke temporary sebelum ke database
        Try
            Dim i As Integer = 0
            For Each item As ListViewItem In LsvStk.Items.ToList()
                Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox"), CheckBox)

                If ck.Checked Then
                    Try

                        Dim dt As New DataTable '= DirectCast(ViewState("costcc"), DataTable)
                        If (DirectCast(ViewState("rcpstk"), DataTable)) IsNot Nothing Then
                            dt = DirectCast(ViewState("rcpstk"), DataTable)
                        Else
                            dt = CRcp.SelectDataRcpStk(txtNo.Text).Tables(0)
                            ViewState("rcpstk") = dt
                        End If


                        dt.Rows.Add(MTid, custid, (DirectCast(item.FindControl("lblID"), Label).Text), txtNo.Text, txtName.Text, (DirectCast(item.FindControl("lblNumber"), Label).Text), (DirectCast(item.FindControl("lblName"), Label).Text))
                        ViewState("rcpstk") = dt
                        Dim dtstk As New DataTable
                        If (DirectCast(ViewState("Invrcp"), DataTable)) IsNot Nothing Then
                            dtstk = DirectCast(ViewState("Invrcp"), DataTable)
                            dtstk.Rows(i).Delete()
                            dtstk.AcceptChanges()
                        Else
                            dtstk = CRcp.Selectdt_rcpsearchdt(dt, custid, txtSearchrcp.Text).Tables(0)
                            ViewState("invrcp") = dtstk
                        End If

                        i = i - 1




                        'txtsearch.Text = ""


                    Catch error_t As Exception
                        MsgBox(error_t.ToString)
                    End Try

                End If
                i = i + 1
            Next
            bindingrcp()
            binding2()
            'BindData()
        Catch ex As Exception
            'MsgBox(ex.Message)
        End Try

    End Sub
    Sub BindingRcp1() 'buat ngurangin list cc dengan yang sudah kelink
        If icustid = 0 Then
            Response.Redirect("~/Default.aspx", False)
        Else

            'Bram4Sep2018 Update Report
            Dim dt, dt2 As New DataTable
            If (DirectCast(ViewState("rcpstk"), DataTable)) IsNot Nothing Then
                dt = DirectCast(ViewState("rcpstk"), DataTable)
                dt2 = CRcp.Selectdt_rcpsearchdt(dt, custid, txtSearchrcp.Text).Tables(0)
            Else
                dt2 = CRcp.SelectDataset_rcpSearch(txtSearchrcp.Text, custid).Tables(0)
            End If

            'dt2 = CIcc.Selectdt_CCsearch(dt, custid, txtsearch.Text)

            ViewState("Invcc") = dt2

            If dt2.Rows.Count > 0 Then
                LsvStk.Items.Clear()
                countlvstore = dt2.Rows.Count
                LsvStk.DataSource = dt2 '.DefaultView
                LsvStk.DataBind()
                UpdatePanel3.Update()
            End If




        End If
    End Sub
    Sub bindingrcp()

        If custid() = 0 Then
            Response.Redirect("~/Default.aspx", False)
        Else
            Dim dt, dt2 As New DataTable
            dt = DirectCast(ViewState("rcpstk"), DataTable)

            'Debug.Print(dt.Rows.Count.ToString)

            If Not DirectCast(ViewState("invrcp"), DataTable) Is Nothing Then
                dt2 = DirectCast(ViewState("invrcp"), DataTable)
            End If


            If dt2.Rows.Count = 0 Then
                'dt2.Rows.Count = 0
                'dt2 = CIcc.Selectdt_CCsearch(dt, custid, txtSearchrcp.Text)
                dt2 = CRcp.Selectdt_rcpsearchdt(dt, custid, txtSearchrcp.Text).Tables(0)
                ViewState("invrcp") = dt2
            Else



                dt2.DefaultView.RowFilter = "name like '%" & txtSearchrcp.Text & "%'"
                ViewState("invrcp") = dt2
            End If
            'disable supaya kosong bisa
            'If dt2.Rows.Count > 0 Then
            LsvStk.Items.Clear()
            countlvstore = dt2.Rows.Count
            LsvStk.DataSource = dt2.DefaultView.ToTable
            LsvStk.DataBind()
            UpdatePanel3.Update()

            'End If

        End If


    End Sub
    Sub insertrcp()
        Dim dtrcp As New DataTable
        dtrcp = ViewState("rcpstk")
        If dtrcp.Rows.Count > 0 Then
            Crcpdtl.deleteRcpcc(MTid, custid)
            For Each dr As DataRow In dtrcp.Rows
                Dim rcpid As Integer = dr.Item("rcpid")

                insertrcpxstock(MTid, custid, rcpid)

            Next
        End If


    End Sub
    Public Sub insertrcpxstock(ByVal id As Integer, ByVal custid As Integer, ByVal rcpid As Integer)
        Try
            Dim c_cmd As New Cls_command

            Dim str As String
            If My.Settings.rcp_cc = 1 Then
                str = "insert into cld_inv_stkxrcp values (" & id & ", " & custid & ", " & rcpid & ")"
                insertLogModule(username, "cld_inv_stkxrcp", "Insert", custid)


                c_cmd.executeNonQuery(str)
            End If


        Catch ex As Exception

            errLog(Err.Description, "Pages_StkEdit:insertrcpxstock")
            Err.Clear()
        End Try
    End Sub

    Public Sub DeleteRcpStk(ByVal stkid As Integer, ByVal Custid As Integer, ByVal rcpid As String)
        'Dim pc As String = pic.ToString
        Dim dt As New DataTable
        Try

            Dim str As String
            Dim c_cmd As New Cls_command

            str = "DELETE FROM cld_inv_stkxrcp  where stkid = " & stkid & " and rcpid = " & rcpid & ""

            c_cmd.executeNonQuery(str)

            'Revisi 10 Mei
            insertLogModule(username, "InvRcpstk", "Delete", Custid)
            dt = ViewState("rcpstk")
            For i As Integer = 0 To dt.Rows.Count - 1
                Dim dr As DataRow = dt.Rows(i)
                If dr("rcpid") = rcpid Then dr.Delete()
            Next i
            dt.AcceptChanges()
            ViewState("rcpstk") = dt
        Catch ex As Exception

            errLog(Err.Description, "Pages_StkEdit:DeleteInvrcpdtl")
            Err.Clear()
        End Try
    End Sub

    Protected Sub txtNo_TextChanged(sender As Object, e As EventArgs) Handles txtName.TextChanged
        If MTid <> 0 Then
            If txtNo.Text = ViewState("codeawal") Then

                errNo.Text = ""
                errNo.Visible = False
            Else
                errNo.Visible = True
                errNo.Text = "Stock no won't change"
            End If
        Else
            errNo.Text = ""
            errNo.Visible = False
        End If



    End Sub
End Class
