Imports System.Threading
Imports System.Net
Imports System.Text
Imports System.IO

Public Class Pages_ItemMasterEdit
    Inherits System.Web.UI.Page
    Private sNameuser As String
    Private Userid As Integer
    Private icustid As Integer
    Private imodul As Integer
    Private iModPriv As Integer
    Private irptpriv As Integer
    Private irptf As Integer
    Private mfunction As New ModFunction
    Private Mdd As String = ""
    'Private ccid As Integer = 0
    Private Crcpdtl As New Cls_IRcpDtl
    Private accItem As New clsDataItem
    Private CIcc As New Cls_ICc
    Private CItmstr As New clsInventory_ItemMaster


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
        Mdd = Request.Cookies("ItemMasterID").Value
        'ccid = Request.Cookies("ccID").Value
        Try

            If Request.Cookies("UserLogin_inv") IsNot Nothing Then
                sNameuser = username()
                icustid = custid()
                imodul = modul()
                iModPriv = modulepriv()
                irptpriv = rptpriv()
                irptf = rptf()
                If IsPostBack = False Then
                    loadmajorgroup(custid)
                    loaditemgroup(custid)
                    If Mdd = "" Or Mdd = "0" Then
                        hapus()
                        Bindingcc1()

                    Else
                        editlist(Mdd) ', ccid)
                        'binding()
                    End If

                    'cek klo 
                    If chklink.Checked = True Then
                        RequiredFieldValidatorddlItem.Enabled = True
                    Else
                        RequiredFieldValidatorddlItem.Enabled = False
                    End If

                End If
                ScriptManager.GetCurrent(Me).RegisterPostBackControl(RadButton1)
                'This for Search Button
                txtSearchcc.Attributes.Add("onkeyup", Me.Page.ClientScript.GetPostBackEventReference(txtSearchcc, ""))
                Checkbox_allcc.Attributes.Add("onclick", Me.Page.ClientScript.GetPostBackEventReference(Checkbox_allcc, ""))

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
            errLog(Err.Description, "Pages_ItemMasterEdit:Page_Load")
            Err.Clear()
        End Try
    End Sub

    Protected Sub Page_unLoad(sender As Object, e As EventArgs) Handles Me.Unload
    End Sub

    Public Function username() As String
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        Return cookie("userName")

    End Function

    Sub hapus()
        txtNo.Enabled = True
        'txtNo.Attributes.Add("readonly", "false")
        txtNo.Text = ""
        txtName.Text = ""

        'txtExpireDate.Text = ""
        txtPrice.Text = 0
        'txtCost.Text = 0
        DdlType.SelectedIndex = 0
        'txtOutlet.Text = ""
        btnUpdate.Visible = False
        btnInsert.Visible = True
    End Sub

    Sub editlist(ByVal id As String)
        Dim ds As New DataSet
        ds = accItem.SelectDataset_ItemEdit(id, custid) ', ccid)
        If ds.Tables(0).Rows.Count = 0 Then
            Exit Sub
        End If
        'loadcc()
        ' ddlcc.SelectedValue = ds.Tables(0).Rows(0).Item("ccid")
        txtNo.Text = ds.Tables(0).Rows(0).Item("itemNo")
        txtName.Text = ds.Tables(0).Rows(0).Item("ItemName")
        ddlmajorgroup.SelectedValue = ds.Tables(0).Rows(0).Item("majorgroup")
        ddlitemgroup.SelectedValue = ds.Tables(0).Rows(0).Item("itemgroup")
        DdlType.SelectedValue = ds.Tables(0).Rows(0).Item("Type")
        txtPrice.Text = ds.Tables(0).Rows(0).Item("price")
        'txtCost.Text = ds.Tables(0).Rows(0).Item("cost2")

        If Not IsDBNull(ds.Tables(0).Rows(0).Item("nonusage")) Then
            If ds.Tables(0).Rows(0).Item("nonusage") = 0 Then
                Chkusage.Checked = True
            Else
                Chkusage.Checked = False
            End If
        End If

        If Not IsDBNull(ds.Tables(0).Rows(0).Item("link")) Then
            chklink.Checked = ds.Tables(0).Rows(0).Item("link")
        Else
            chklink.Checked = True
        End If


        If chklink.Checked = True Then
            DdlType.Enabled = True
            ddlItem.Enabled = True
        Else
            DdlType.Enabled = False
            ddlItem.Enabled = False
        End If

        If Not IsDBNull(ds.Tables(0).Rows(0).Item("iType")) Then
            DdlType.SelectedValue = ds.Tables(0).Rows(0).Item("iType")
        Else
            DdlType.SelectedValue = 0
        End If
        'ddlItem.SelectedValue = 1
        loadRcpdtl(DdlType.SelectedValue)
        If DdlType.SelectedValue = 1 Then
            If Not IsDBNull(ds.Tables(0).Rows(0).Item("iId")) Then
                ddlItem.SelectedValue = ds.Tables(0).Rows(0).Item("iId")
            End If


        Else
            If Not IsDBNull(ds.Tables(0).Rows(0).Item("rId")) Then
                ddlItem.SelectedValue = ds.Tables(0).Rows(0).Item("rId")
            End If

        End If
        'binding()
        Dim dt As New DataTable
        dt = CItmstr.SelectDT_itemCost(custid, txtNo.Text, username, modulepriv).Tables(0)
        ViewState("costcc") = dt
        lstViewCost.DataSource = dt
        lstViewCost.DataBind()
        'binding2()
        bindingcc()


        If Not IsDBNull(ds.Tables(0).Rows(0).Item("factor")) Then
            txtFactor.Text = ds.Tables(0).Rows(0).Item("factor")
        Else
            txtFactor.Text = ""
        End If
        txtNo.Attributes.Add("readonly", "true")
        txtName.Attributes.Add("readonly", "true")
        btnUpdate.Visible = True
        btnInsert.Visible = False

    End Sub

    Protected Sub btnInsert_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnInsert.Click
        insert()
    End Sub

    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnUpdate.Click
        insert()
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancel.Click
        Try

            'hapus()
            'mfunction.CreateCookie("ItemID", 0)

            Response.Redirect("~/Pages_ItemMaster.aspx", False)

        Catch ex As Exception
            errLog(Err.Description, "Pages_ItemMasterEdit:btnCancel_Click")
            Err.Clear()
        End Try
    End Sub

    Protected Sub chkbatch_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chklink.CheckedChanged
        If chklink.Checked = True Then
            DdlType.Enabled = True
            RequiredFieldValidatorddlItem.Enabled = True
        Else

            RequiredFieldValidatorddlItem.Enabled = False

            DdlType.Enabled = False
            ddlItem.Enabled = False
            'ddlItem.Text = ""
            DdlType.SelectedValue = 0
        End If
    End Sub

    Sub loadRcpdtl(ByVal tipe As Integer)
        'ddlItem.Items.Clear()
        'ddlItem.Text = ""
        Dim ds As New DataSet
        ds = Crcpdtl.SelectDataInvItemAll(custid, tipe)
        If ds IsNot Nothing AndAlso ds.Tables.Count > 0 Then
            ddlItem.DataSource = ds
        Else
            ddlItem.DataSource = Nothing
        End If
        ddlItem.DataValueField = "Id"
        ddlItem.DataTextField = "Name"
        ddlItem.DataBind()
    End Sub

    Protected Sub DdlType_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles DdlType.SelectedIndexChanged
        If DdlType.SelectedValue > 0 Then
            ddlItem.Enabled = True
            loadRcpdtl(DdlType.SelectedValue)
        Else
            ddlItem.Enabled = False
        End If
    End Sub
    Sub insert()
        'Dim act As Boolean

        Try
            If IsNumeric(txtFactor.Text) = False Then
                txtFactor.Text = 0
                Exit Sub
            End If


            '  Dim Expire As Date = Convert.ToDateTime(txtExpireDate.Text)
            Dim nonusage As Integer = 0
            If Chkusage.Checked = True Then
                nonusage = 0
            Else
                nonusage = 1
            End If
            Dim ck As Boolean
            If chklink.Checked = True Then
                ck = True

            Else
                ck = False
            End If
            Dim item As Integer

            If DdlType.SelectedValue = 0 Then
                item = 0
            Else
                item = ddlItem.SelectedValue
            End If
            If ck = True And item = 0 Then
                fillplease()
                Exit Sub
            End If
            If txtPrice.Text = "" Then
                txtPrice.Text = 0
            End If
            insertItemMaster(txtNo.Text, txtName.Text, 1, txtPrice.Text, 0, custid, ck, DdlType.SelectedValue, item, txtFactor.Text, nonusage)
            insertcc()
            'MsgBox("Data Berhasil Di input Mysql")

            Response.Redirect("~/Pages_ItemMaster.aspx", False)
        Catch ex As Exception
            errLog(Err.Description, "Pages_ItemMasterEdit:insert")
            Err.Clear()
        End Try
    End Sub
    Public Sub insertItemMaster(ByVal ItemNo As String, ByVal Name As String, ByVal Type As Integer, ByVal Price As Decimal, ByVal cost As Decimal, ByVal custid As Integer, ByVal link As Boolean, ByVal iType As Integer, ByVal iId As Integer, ByVal factor As Decimal, ByVal nonusage As Integer, ByVal Optional majorgroup As Integer = 0, ByVal Optional itemgroup As Integer = 0, ByVal Optional Displayname As String = "", ByVal Optional ItemNo1 As String = "")
        Try


            If ItemNo.Length > 8 Then
                ItemNo = Mid(ItemNo, 1, 8)
            End If

            Dim clsdt As New clsDataItem
            Dim ds As DataSet
            ds = clsdt.SelectItemNo(ItemNo) ', ddlcc.SelectedValue)

            Dim idlinkstk As Integer = 0
            Dim idlinkrcp As Integer = 0

            Dim str As String
            ServerString = My.Settings.slssvr
            Dim c_cmd As New Cls_command

            If iType = 1 Then
                idlinkstk = iId
                idlinkrcp = 0
            ElseIf iType = 2 Then
                idlinkrcp = iId
                idlinkstk = 0
            End If

            Name = Name.ToString.Replace("'", "''")


            If ds.Tables(0).Rows.Count = 0 Then
                str = "insert into cld_ItemMaster values ( '" & ItemNo & "','" & Name & "', " & Price & ", " & cost & ", " & Type & ", " & custid & ", " & link & ", " & iType & ", " & idlinkstk & ", " & idlinkrcp & ", " & factor & ", " & ddlitemgroup.SelectedValue & ", " & ddlmajorgroup.SelectedValue & ", " & nonusage & ",'" & Displayname & "','" & ItemNo1 & "' )"
            Else
                str = "Update cld_ItemMaster set  ItemName = '" & Name & "', price = " & Price & ", Type = " & Type & ", cost = " & cost & ", link = " & link & ", iType = " & iType & ", iid = " & idlinkstk & ", rid = " & idlinkrcp & ", factor = " & factor & ", nonusage = " & nonusage & " where ItemNo = '" & ItemNo & "'"
            End If

            c_cmd.executeNonQuery(str)
            ServerString = My.Settings.ServerStr
            c_cmd = Nothing

        Catch ex As Exception

            errLog(Err.Description, "Pages_ItemMasterEdit:insertItemMaster")
            Err.Clear()
        End Try
    End Sub
    Sub fillplease()
        Dim message As String = "Please Choice Item Type"
        Dim sb As New System.Text.StringBuilder()
        Dim url As String = "~/Pages_ItemMaster.aspx"
        sb.Append("<script type = 'text/javascript'>")
        sb.Append("window.onload=function(){")
        sb.Append("alert('")
        sb.Append(message)
        sb.Append("')};")
        sb.Append("</script>")
        ClientScript.RegisterClientScriptBlock(Me.GetType(), "Alert", sb.ToString())
    End Sub
    'Protected Sub ddlcc_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlcc.SelectedIndexChanged

    '    Try

    '    Catch ex As Exception

    '    End Try

    'End Sub
    'Sub loadcc()
    '    Dim ds As New DataSet

    '    Dim cicc As New clsInventory_Cc
    '    ds = cicc.SelectDataset_IccOnly(custid, username, modulepriv)
    '    cicc = Nothing

    '    ddlcc.DataSource = ds
    '    ddlcc.DataValueField = "Id"
    '    ddlcc.DataTextField = "Name"
    '    ddlcc.DataBind()

    '    If ddlcc.Items.Count > 0 Then
    '        ddlcc.SelectedIndex = 0
    '    End If

    'End Sub
    Sub binding()

        Dim cInv As New clsInventory_ItemMaster

        Dim ds As New DataSet
        'ds = WService.getItemMaster(cust)

        ds = cInv.SelectDT_itemCost(custid, txtNo.Text)

        If ds IsNot Nothing AndAlso ds.Tables.Count > 0 Then
            lstViewCost.DataSource = ds
        Else
            lstViewCost.DataSource = Nothing
        End If
        lstViewCost.DataBind()

    End Sub

    'Sub binding(ByVal rcpid As Integer)

    '    Try
    '        If custid() = 0 Then
    '            Response.Redirect("~/Default.aspx")
    '        Else
    '            Dim ds As New DataSet
    '            ds = Crcpdtl.SelectDataset_rcpdtl(custid, rcpid)
    '            lstViewDetails.DataSource = ds
    '            lstViewDetails.DataBind()
    '        End If
    '    Catch ex As Exception

    '    End Try


    'End Sub
    'Protected Sub OnPagePropertiesChanging(ByVal sender As Object, ByVal e As PagePropertiesChangingEventArgs)
    '    lstViewCostPager1.SetPageProperties(e.StartRowIndex, e.MaximumRows, False)


    '    binding()
    'End Sub

    Protected Sub lstViewCost_ItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ListViewCommandEventArgs) Handles lstViewCost.ItemCommand
        Select Case (e.CommandName)
            Case "Del"

                Dim confirmValue As String = Request.Form("confirm_value")
                If confirmValue = "Yes" Then

                    ' Dim id As Integer = Convert.ToInt32(e.CommandArgument)
                    Dim arg As String() = e.CommandArgument.ToString().Split(";")
                    deletecc(arg(0), arg(1))

                Else
                    Exit Sub
                End If


            Case "Edt"
                Dim txtaddcost As TextBox = CType(lstViewCost.Items(e.Item.DisplayIndex).FindControl("taddcost"), TextBox)
                Dim imbtn As ImageButton = CType(lstViewCost.Items(e.Item.DisplayIndex).FindControl("imgbtnUpdate"), ImageButton)
                '  Dim ModulePriv As TextBox = CType(lstViewDetails.Items(e.Item.DataItemIndex).FindControl("tModPriv"), TextBox)
                '   Dim rptf As TextBox = CType(lstViewDetails.Items(e.Item.DataItemIndex).FindControl("txtRptf"), TextBox)
                '  Dim asd As String = ""
                txtaddcost.Enabled = True
                txtaddcost.BorderWidth = 1
                imbtn.Visible = True
                '    Dim arg As String() = e.CommandArgument.ToString().Split(";")


                '    mfunction.CreateCookie("rcpIdrcp", arg(0))
                '    mfunction.CreateCookie("rcptipe", arg(1))
                '    mfunction.CreateCookie("rcpID", MTid)
                '    mfunction.CreateCookie("rcpunt", arg(2))

                '    Response.Redirect("~/Pages_ReceipeDtlEdit.aspx")

            Case "save"
                '  Dim DisplayName As TextBox = CType(lstViewDetails.Items(e.Item.DataItemIndex).FindControl("txtDisplayName"), TextBox)
                Dim txtaddcost As TextBox = CType(lstViewCost.Items(e.Item.DisplayIndex).FindControl("taddcost"), TextBox)

                'clsreport.UpdateRptList(rptLevel.Text, lblrptID.Text)
                Dim arg As String() = e.CommandArgument.ToString().Split(";")

                updateaddcostcc(arg(0), txtNo.Text, custid, CDec(txtaddcost.Text))
                txtaddcost.Enabled = False
                txtaddcost.BorderWidth = 0
                Dim imbtn As ImageButton = CType(lstViewCost.Items(e.Item.DisplayIndex).FindControl("imgbtnUpdate"), ImageButton)
                imbtn.Visible = False
        End Select
    End Sub
    Sub deletecc(ByVal id As Integer, ByVal ccname As String)
        Try
            Dim dtcc As New DataTable


            If (DirectCast(ViewState("Invcc"), DataTable)) IsNot Nothing Then
                dtcc = DirectCast(ViewState("Invcc"), DataTable)
            Else
                dtcc = CIcc.Selectdt_CCsearch(dtcc, custid, txtSearchcc.Text)
                dtcc.Clear()

            End If
            dtcc.Rows.Add(id, custid, ccname)
            dtcc.AcceptChanges()
            ViewState("Invcc") = dtcc
            Deletecostcc(id, Mdd, custid)
            binding1()
            bindingcc()
            Dim dtitemcc As DataTable = DirectCast(ViewState("costcc"), DataTable)
            Dim dnewcc As DataTable = dtitemcc.Clone
            dnewcc.Rows.Clear()

            For Each dr As DataRow In dtitemcc.Rows
                If dr.Item("ccid") <> id Then
                    dnewcc.ImportRow(dr)

                    'dtitemcc.Rows.Remove(dr)
                    dnewcc.AcceptChanges()
                    ViewState("costcc") = dnewcc
                    lstViewCost.DataSource = dnewcc
                    lstViewCost.DataBind()
                End If

            Next
            'For Each dr1 As DataRow In dtodel.Rows
            '    dtitemcc.Rows.Remove(dr1)
            '    dtitemcc.AcceptChanges()
            '    ViewState("costcc") = dtitemcc
            '    lstViewCost.DataSource = dtitemcc
            '    lstViewCost.DataBind()
            'Next

            'binding2()
        Catch ex As Exception
            errLog(Err.Description, "deletecc")
            Err.Clear()
        End Try

    End Sub
    Public Sub Deletecostcc(ByVal id As Integer, ByVal itemno As String, ByVal Custid As Integer)
        'Dim pc As String = pic.ToString
        Try

            Dim str As String
            ServerString = My.Settings.slssvr
            Dim c_cmd As New Cls_command
            If My.Settings.itemmaster_cc = 1 Then
                str = "DELETE FROM cld_itemmaster_cc WHERE itemno = '" & itemno & "' AND ccid = " & id & " AND custid = " & Custid & ""

                c_cmd.executeNonQuery(str)
            End If

            ServerString = My.Settings.ServerStr
            'Revisi 10 Mei
            insertLogModule(username, "Deletecostcc", "Delete", Custid)

        Catch ex As Exception

            errLog(Err.Description, "c:Deletecostcc")
            Err.Clear()
        End Try
    End Sub

    Public Sub updateaddcostcc(ByVal id As Integer, ByVal itemno As String, ByVal Custid As Integer, ByVal adcos As Decimal)
        'Dim pc As String = pic.ToString
        Try

            Dim str As String
            ServerString = My.Settings.slssvr
            Dim c_cmd As New Cls_command
            If My.Settings.itemmaster_cc = 1 Then
                str = "UPDATE cld_itemmaster_cc set addcost =" & adcos & "  WHERE itemno = '" & itemno & "' AND ccid = " & id & " AND custid = " & Custid & ""

                c_cmd.executeNonQuery(str)

                dt = CItmstr.SelectDT_itemCost(Custid, txtNo.Text, username, modulepriv).Tables(0)
                ViewState("costcc") = dt


            End If

            ServerString = My.Settings.ServerStr
            'Revisi 10 Mei
            insertLogModule(username, "updateaddcostcc", "update add cost", Custid)

        Catch ex As Exception

            errLog(Err.Description, "c:Deletecostcc")
            Err.Clear()
        End Try
    End Sub

    Sub binding1() 'ambil cc mana saja di itemxcc

        Dim clsitemmaster As New clsInventory_ItemMaster
        Dim dtcostcc As New DataTable

        Dim ds As New DataSet
        'ds = WService.getItemMaster(cust)
        If custid() = 0 Then
            Response.Redirect("~/Default.aspx")
        Else
            dtcostcc = clsitemmaster.SelectDT_itemCost(custid, txtNo.Text).Tables(0)
            ViewState("costcc") = dtcostcc
            lstViewCost.DataSource = DirectCast(ViewState("costcc"), DataTable)
            lstViewCost.DataBind()
        End If
        'ds = c_rcp.SelectDataset_rcpCost(custid, txtNo.Text)



    End Sub
    Sub bindingcc()

        If custid() = 0 Then
            Response.Redirect("~/Default.aspx")
        Else
            Dim dt, dt2 As New DataTable
            dt = DirectCast(ViewState("costcc"), DataTable)

            Debug.Print(dt.Rows.Count.ToString)
            'dt2 = clsStk.SelectDataTable_Stk_wStock(dt, custid, txtSearch.Text, ddlFrom.SelectedValue)
            'ViewState("InvStk") = dt2
            'LsvStk.DataSource = DirectCast(ViewState("InvStk"), DataTable)
            'LsvStk.DataBind()
            If Not DirectCast(ViewState("Invcc"), DataTable) Is Nothing Then
                dt2 = DirectCast(ViewState("Invcc"), DataTable)
            End If


            If dt2.Rows.Count = 0 Then
                'dt2.Rows.Count = 0
                'dt2 = CIcc.Selectdt_CCsearch(dt, custid, txtSearchcc.Text)
                dt2 = CIcc.Selectdt_CCsearchdt(dt, custid, txtSearchcc.Text, username, modulepriv, 0).Tables(0)
                ViewState("Invcc") = dt2
            Else



                dt2.DefaultView.RowFilter = "ccname like '%" & txtSearchcc.Text & "%'"
                ViewState("invcc") = dt2
            End If
            'disable supaya kosong bisa
            'If dt2.Rows.Count > 0 Then
            LsvStk.Items.Clear()
            countlvstore = dt2.Rows.Count
            LsvStk.DataSource = dt2.DefaultView
            LsvStk.DataBind()
            UpdatePanel3.Update()

            'End If

        End If


    End Sub
    Protected Sub OnPagecostChanging(ByVal sender As Object, ByVal e As PagePropertiesChangingEventArgs)
        lstViewCostPager1.SetPageProperties(e.StartRowIndex, e.MaximumRows, False)

        binding2()
    End Sub
    Protected Sub RadButton1_ServerClick(sender As Object, e As EventArgs) Handles RadButton1.ServerClick

        Add2()
        'DgvTransfer.DataBind()
    End Sub
    Sub binding2() 'ambil cc mana saja di itemxcc

        Dim c_rcp As New Cls_IRcp
        Dim dtcostcc As New DataTable

        Dim ds As New DataSet
        'ds = WService.getItemMaster(cust)
        If custid() = 0 Then
            Response.Redirect("~/Default.aspx")
        Else
            dtcostcc = DirectCast(ViewState("costcc"), DataTable)
            lstViewCost.DataSource = dtcostcc
            lstViewCost.DataBind()
        End If
        'ds = c_rcp.SelectDataset_rcpCost(custid, txtNo.Text)



    End Sub
    Sub Add2()
        'untuk nambah record ke temporary sebelum ke database
        Try
            Dim i As Integer = 0
            For Each item As ListViewItem In LsvStk.Items.ToList()
                Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox"), CheckBox)

                If ck.Checked Then
                    Try

                        'Dim u1 As String = ds.Tables(0).Rows(0).Item("Unit1").ToString
                        '    Dim factor1 As String = ds.Tables(0).Rows(0).Item("factor1").ToString
                        '    Dim dCost As Decimal = ds.Tables(0).Rows(0).Item("coststk").ToString
                        '    Dim dstockcc As Decimal = ds.Tables(0).Rows(0).Item("stockcc") / ds.Tables(0).Rows(0).Item("factor1")
                        '    Dim dstock As Decimal = ds.Tables(0).Rows(0).Item("stock") / ds.Tables(0).Rows(0).Item("factor1")
                        '    Dim dstklimit As Decimal = ds.Tables(0).Rows(0).Item("stk_limit") / ds.Tables(0).Rows(0).Item("factor1")
                        '    Dim snumber As String = ds.Tables(0).Rows(0).Item("Number").ToString
                        '    Dim u2 As String = ds.Tables(0).Rows(0).Item("Unit2").ToString
                        Dim dt As New DataTable '= DirectCast(ViewState("costcc"), DataTable)
                        ' If (DirectCast(ViewState("costcc"), DataTable)) IsNot Nothing Then
                        If DirectCast(ViewState("costcc"), DataTable).Rows.Count > 0 Then
                            dt = DirectCast(ViewState("costcc"), DataTable)
                        Else
                            dt = CItmstr.SelectDT_itemCost(custid, txtNo.Text).Tables(0)
                            ViewState("costcc") = dt
                        End If
                        'Debug.Print((DirectCast(item.FindControl("lblID"), Label).Text))
                        'Debug.Print((DirectCast(item.FindControl("lblName"), Label).Text))
                        'Debug.Print(u1.ToString)
                        'Debug.Print(factor1.ToString)

                        dt.Rows.Add(Mdd, custid, txtNo.Text, txtName.Text, 0, (DirectCast(item.FindControl("lblName"), Label).Text), (DirectCast(item.FindControl("lblID"), Label).Text), 0)

                        ViewState("costcc") = dt
                        Dim dtstk As New DataTable
                        If (DirectCast(ViewState("Invcc"), DataTable)) IsNot Nothing Then
                            dtstk = DirectCast(ViewState("Invcc"), DataTable)
                        Else
                            dtstk = CIcc.Selectdt_CCsearchdt(dtstk, custid, "", username, modulepriv, 0).Tables(0)
                            ViewState("Invcc") = dtstk
                        End If


                        dtstk.Rows(i).Delete()
                        dtStk.AcceptChanges()
                        i = i - 1




                        txtSearchcc.Text = ""
                        bindingcc()
                        binding2()
                    Catch error_t As Exception
                        MsgBox(error_t.ToString)
                    End Try

                End If
                i = i + 1
            Next

            'BindData()
        Catch ex As Exception
            errLog(Err.Description, "Add2")
            Err.Clear()
        End Try

    End Sub
    Protected Sub Searchcc_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearchcc.ServerClick
        If txtSearchcc.Text = "" Then
            ShowToastr(Page, "Text Search tidak boleh kosong", "Error", "Error", False, "toast-top-right", False)
        Else
            BindCheckItem()
            bindingcc()
            CheckedItem()
        End If
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Pop", "openModalcc();", True)

    End Sub
    Private Sub ShowToastr(ByVal page As Page, ByVal message As String, ByVal title As String, Optional ByVal type As String = "info", Optional ByVal clearToast As Boolean = False, Optional ByVal pos As String = "toast-top-left", Optional ByVal Sticky As Boolean = False)
        Dim toastrScript As String = [String].Format("Notify('{0}','{1}','{2}', '{3}', '{4}', '{5}');", message, title, type, clearToast, pos, Sticky)
        page.ClientScript.RegisterStartupScript(page.[GetType](), "toastr_message", toastrScript, addScriptTags:=True)
    End Sub
    <Serializable>
    Public Class CC_Detail 'Class untuk ListItem
        Public Property CCName As String = ""
        Public Property Status As Boolean = False
    End Class
    Private Sub BindCheckItem()
        Try

            If CBool(IIf(IsNothing(ViewState("ListCC")), True, False)) Then


                Dim ListItem2 As List(Of CC_Detail) = New List(Of CC_Detail)()

                Dim dtcc, dtinvcc As New DataTable
                dtcc = DirectCast(ViewState("costcc"), DataTable)
                dtinvcc = CIcc.Selectdt_CCsearch(dtcc, custid, txtSearchcc.Text)

                If dtinvcc.Rows.Count > 0 Then
                    For Each dt As DataRow In dtinvcc.Rows
                        ListItem2.Add(New CC_Detail With {
                                           .CCName = dt.Item("ccname"),
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
                            If a.CCName = label2 Then
                                a.Status = True
                            End If
                        Next
                        '  clsCheck.IDBrand.Add(label2)
                    End If
                Next
                ViewState("ListCC") = ListItem2

            Else

                Dim ListItem2 As List(Of CC_Detail) = TryCast(ViewState("ListCC"), List(Of CC_Detail))

                For Each item As ListViewItem In LsvStk.Items
                    Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox"), CheckBox)
                    Dim Labels As Label = DirectCast(item.FindControl("lblName"), Label)
                    Dim label2 As String = Labels.Text

                    If ck.Checked Then
                        For Each a In ListItem2
                            If a.CCName = label2 Then
                                a.Status = True
                            End If
                        Next

                    Else
                        For Each a In ListItem2
                            If a.CCName = label2 Then
                                a.Status = False
                            End If
                        Next
                    End If
                Next
                ViewState("ListCC") = ListItem2
            End If
        Catch ex As Exception
            errLog(Err.Description, "BindCheckItem")
            Err.Clear()
        End Try
    End Sub

    Private Sub CheckedItem()
        Try


            Dim ListStore As List(Of CC_Detail) = TryCast(ViewState("ListCC"), List(Of CC_Detail))

            Dim i As Integer = 0
            For Each item As ListViewItem In LsvStk.Items
                Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox"), CheckBox)
                Dim Labels As Label = DirectCast(item.FindControl("lblName"), Label)
                Dim label2 As String = Labels.Text

                For Each a In ListStore
                    If a.CCName = label2 Then
                        If a.Status = True Then
                            ck.Checked = True
                        Else
                            ck.Checked = False
                        End If

                    End If
                Next

            Next
            ViewState("ListCC") = ListStore
        Catch ex As Exception
            errLog(Err.Description, "CheckedItem")
            Err.Clear()
        End Try
    End Sub
    Protected Sub btnClearSearch_Click(sender As Object, e As EventArgs)
        For Each item As ListViewItem In LsvStk.Items
            Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox"), CheckBox)

            ck.Checked = False


        Next
        txtSearchcc.Text = ""
        'lblnoitem.Text = 0
        'BindData()
        Bindingcc1()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Pop", "openModalcc();", True)
    End Sub
    Sub Bindingcc1() 'buat ngurangin list cc dengan yang sudah kelink
        If icustid = 0 Then
            Response.Redirect("~/Default.aspx", False)
        Else

            'Bram4Sep2018 Update Report
            Dim dt, dt2 As New DataTable
            If (DirectCast(ViewState("costcc"), DataTable)) IsNot Nothing Then
                dt = DirectCast(ViewState("costcc"), DataTable)
            End If
            'dt2 = CIcc.Selectdt_CCsearch(dt, custid, txtSearchcc.Text)
            dt2 = CIcc.Selectdt_CCsearchdt(dt, custid, txtSearchcc.Text, username, modulepriv, 0).Tables(0)
            ViewState("Invcc") = dt2

            If dt2.Rows.Count > 0 Then
                LsvStk.Items.Clear()
                countlvstore = dt2.Rows.Count
                LsvStk.DataSource = dt2.DefaultView
                LsvStk.DataBind()
                UpdatePanel3.Update()
            End If




        End If
    End Sub

    Sub insertcc()
        Dim dtcc As New DataTable
        Try
            dtcc = ViewState("costcc")
            If dtcc.Rows.Count > 0 Then
                CItmstr.deleteItemcc(txtNo.Text, custid)

                For Each dr As DataRow In dtcc.Rows
                    Dim ccid As Integer = dr.Item("ccid")
                    Dim cost2 As Double = dr.Item("cost2")
                    Dim adcost As Decimal = 0
                    If dr.Item("addcost") IsNot Nothing Then
                        adcost = dr.Item("addcost")
                    End If

                    If adcost <> 0 Then
                        insertitemxcostcc(txtNo.Text, ccid, cost2, custid, adcost)
                    Else
                        insertitemxcostcc(txtNo.Text, ccid, cost2, custid)
                    End If


                Next
            Else
                CItmstr.deleteItemcc(txtNo.Text, custid)
            End If

        Catch ex As Exception
            errLog(Err.Description, "Pages_ItemMasterEdit:insertcc")
            Err.Clear()
        End Try



    End Sub

    '5 juni 2023 awalnya dijagain biar ga bisa masuk new record untuk cld_itemmaster_cc, sudah dirubah
    Public Sub insertitemxcostcc(ByVal itemno As String, ByVal ccid As Integer, ByVal cost As Double, ByVal custid As Integer, Optional ByVal adcost As Decimal = 0)
        Try

            Dim c_cmd As New clsDbo("", 1)
            Dim str As String = ""
            ' If My.Settings.itemmaster_cc = 1 Then
            If adcost <> 0 Then
                str = "insert into cld_itemmaster_cc values ('" & itemno & "', " & custid & ", " & cost & ",  " & ccid & ",  " & adcost & ")"
            Else
                str = "insert into cld_itemmaster_cc (ItemNo,CustID,cost,ccid) values ('" & itemno & "', " & custid & ", " & cost & ",  " & ccid & ")"
            End If

            insertLogModule(username, "ItemMaster_Edit", "Update", custid)

            ' End If


            c_cmd.executeNonQuery(str)

            c_cmd = Nothing
        Catch ex As Exception

            errLog(Err.Description, "Pages_ItemMasterEdit:insertitemxcostcc")
            Err.Clear()
        End Try
    End Sub
    Sub loadmajorgroup(ByVal tipe As Integer)
        'ddlItem.Items.Clear()
        'ddlItem.Text = ""
        Dim ds As New DataSet
        ds = CItmstr.SelectDataset_majorgroup(custid)
        If ds IsNot Nothing AndAlso ds.Tables.Count > 0 Then
            ddlmajorgroup.DataSource = ds
        Else
            ddlmajorgroup.DataSource = Nothing
        End If
        ddlmajorgroup.DataValueField = "Id"
        ddlmajorgroup.DataTextField = "Name"
        ddlmajorgroup.DataBind()
    End Sub
    Sub loaditemgroup(ByVal tipe As Integer)
        'ddlItem.Items.Clear()
        'ddlItem.Text = ""
        Dim ds As New DataSet
        ds = CItmstr.SelectDataset_itemgroup(custid)
        If ds IsNot Nothing AndAlso ds.Tables.Count > 0 Then
            ddlitemgroup.DataSource = ds
        Else
            ddlitemgroup.DataSource = Nothing
        End If
        ddlitemgroup.DataValueField = "Id"
        ddlitemgroup.DataTextField = "Name"
        ddlitemgroup.DataBind()
    End Sub

    Protected Sub Checkbox_allcc_CheckedChanged(sender As Object, e As EventArgs) 'Handles Checkbox_allcc.CheckedChanged
        For Each item As ListViewItem In LsvStk.Items
            Dim ck As CheckBox = DirectCast(item.FindControl("Checkbox"), CheckBox)

            ck.Checked = Checkbox_allcc.Checked

        Next

        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Pop", "openModalcc();", True)
    End Sub

    Protected Sub Chkusage_CheckedChanged(sender As Object, e As EventArgs) Handles Chkusage.CheckedChanged

    End Sub
End Class
