Imports System.Threading
Imports System.Net
Imports System.Text
Imports System.IO
Imports HQWeb.Item_master


Public Class User_Edit
    Inherits System.Web.UI.Page
    Private sNameuser As String

    Dim Pass As String
    Dim Priv As Integer
    Dim Itemmasterid As Integer
    Dim majorgroupid As Integer
    Dim itemgroupid As Integer
    Private mfunction As New ModFunction

    Private citemmaster As New Clsitemmaster


    Public Function susername() As String
        Dim cookie As HttpCookie = Request.Cookies("UserLogin")
        Return cookie("username")

    End Function
    Public Function iPriv() As Integer
        Dim cookie As HttpCookie = Request.Cookies("UserLogin")
        Return cookie("Priv")

    End Function
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        mfunction.ceksession()
        Itemmasterid = Request.Cookies("itemmaster_edt_ID").Value
        majorgroupid = Request.Cookies("majorgroup_edt_ID").Value
        itgroupid = Request.Cookies("itemgroup_edt_ID").Value
        Try
            mfunction.ceksession()
            If Request.Cookies("UserLogin") IsNot Nothing Then
                sNameuser = susername()
                Priv = iPriv()
                If IsPostBack = False Then
                    'Set dataItem untuk listview option
                    Dim dt As New DataTable()
                    dt.Columns.Add("id", GetType(Integer))
                    dt.Columns.Add("Name", GetType(String))
                    dt.Columns.Add("Status", GetType(Boolean))
                    dt.Rows.Add(1, "Open_Item", False)
                    dt.Rows.Add(2, "Open_Price", False)
                    dt.Rows.Add(3, "NEW_MEMBER_ITEM", False)
                    dt.Rows.Add(4, "RENEW_MEMBER_ITEM", False)
                    dt.Rows.Add(5, "RESTRICTED_ITEM", False)
                    dt.Rows.Add(6, "TICKET_ITEM", False)
                    dt.Rows.Add(7, "FAVORITE_ITEM", False)
                    dt.Rows.Add(8, "EXCLUDE_FROM_MEMBER_POINT", False)
                    dt.Rows.Add(9, "REFUNDED_ITEM", False)
                    dt.Rows.Add(10, "GATE_ITEM", False)
                    dt.Rows.Add(11, "ItemVoucher_Sushitei", False)
                    dt.Rows.Add(12, "AddMoreCombo", False)
                    ViewState("ListOptions") = Helper.DataTableToList(Of Flag_option)(dt)

                    lvwOption.DataSource = dt
                    lvwOption.DataBind()
                    If Itemmasterid = 0 Then


                        hapus()
                        btnUpdate.Visible = True
                        btnUpdate.Text = "INSERT"
                    Else

                        editlist(Itemmasterid)

                    End If
                    'check_Batch()
                End If
            Else
                Dim script As String = "window.onload = function(){ alert('"
                script += "Please Login"
                script += "');"
                script += "window.location = '"
                script += "../Default"
                script += " '; }"
                ClientScript.RegisterStartupScript(Me.GetType(), "Redirect", script, True)
            End If


        Catch ex As Exception
            errLog(Err.Description, "StoreGrpEdit:Page_Load")
            Err.Clear()
        End Try
    End Sub

    'Protected Sub Page_unLoad(sender As Object, e As EventArgs) Handles Me.Unload
    'End Sub

    Public Function username() As String
        Dim cookie As HttpCookie = Request.Cookies("UserLogin")
        Return cookie("userName")

    End Function

    Sub hapus()
        txtnumber.Text = ""
        txtnama.Text = ""
        loadMajorgroup()
        loadItemgroup()
        'loadSLU()
        loadunit()
        loadCurrency()
        loadchargeitemizer()
        loadmenuitemizer()

    End Sub

    Sub editlist(ByVal itemid As Integer)
        Try
            Dim ds As New DataSet
            Dim dt As New DataTable
            Dim cmajorgroup As New Clsmajorgroup
            Dim iFlagoption As Integer = 0
            Dim iRow As Integer = 0
            loadMajorgroup()
            loadItemgroup()
            'loadSLU()
            loadunit()
            loadCurrency()
            loadchargeitemizer()
            loadmenuitemizer()

            ds = citemmaster.Selectds_itembyid(itemid)
            If ds.Tables(0).Rows.Count = 0 Then
                btnUpdate.Visible = False
                Exit Sub
            Else

                ddlitemgroup.SelectedValue = ds.Tables(0).Rows(0).Item("ItemGroupID").ToString
                dt = cmajorgroup.ds_majorgroup_byig(ds.Tables(0).Rows(0).Item("ItemGroupID")).Tables(0)
                ddlmajorgroup.SelectedValue = dt.Rows(0).Item("id").ToString
                ddlUnit.SelectedValue = ds.Tables(0).Rows(0).Item("UnitID").ToString
                ddlCurrency.SelectedValue = ds.Tables(0).Rows(0).Item("CurID").ToString
                ddlChargeItemizer.SelectedValue = ds.Tables(0).Rows(0).Item("ChargeItemizerID").ToString
                ddlMenuItemizer.SelectedValue = ds.Tables(0).Rows(0).Item("MenuItemizerID").ToString
                txtnumber.Text = ds.Tables(0).Rows(0).Item("number").ToString
                txtnumber.Enabled = False
                txtbarcode.Text = ds.Tables(0).Rows(0).Item("barcode").ToString
                txtnama.Text = ds.Tables(0).Rows(0).Item("name").ToString
                txtLang1.Text = ds.Tables(0).Rows(0).Item("Lang1")
                txtLang2.Text = ds.Tables(0).Rows(0).Item("Lang2")
                txtLang3.Text = ds.Tables(0).Rows(0).Item("Lang3")

                ViewState("item_oldname") = txtnama.Text
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("EffDate")) = True Then
                    chk_eff.Checked = True
                    txteffdate.Text = ds.Tables(0).Rows(0).Item("Eff_Date")
                Else
                    chk_eff.Checked = False
                    txteffdate.Text = ""
                End If
                If Not IsDBNull(ds.Tables(0).Rows(0).Item("EndDate")) = True Then
                    chk_end.Checked = True
                    txtenddate.Text = ds.Tables(0).Rows(0).Item("End_Date")
                Else
                    chk_end.Checked = False
                    txtenddate.Text = ""
                End If
                btnUpdate.Visible = True
                btnUpdate.Text = "UPDATE"
                'btnInsert.Visible = False
                'tambahan 1 feb 2018 erik
                iFlagoption = ds.Tables(0).Rows(0).Item("FlagOption")

                For Each item As ListViewItem In lvwOption.Items
                    Dim ck As CheckBox = DirectCast(item.FindControl("cbxOptionDetail"), CheckBox)

                    ck.Checked = mfunction.check_ItemOption(iRow, iFlagoption)

                    iRow += 1
                Next
            End If

        Catch ex As Exception
            errLog(Err.Description, "StoreGrpEdit:editlist")
            Err.Clear()
        End Try
    End Sub
    Private Sub loadMajorgroup(Optional ByVal itemgroup As Integer = 0)

        Try
            Dim cmajorgroup As New Clsmajorgroup
            Dim dtmajorgroup As New DataTable
            dtmajorgroup = cmajorgroup.ds_majorgroup_byig(itemgroup).Tables(0)

            If dtmajorgroup.Rows.Count > 0 Then
                ddlmajorgroup.DataSource = dtmajorgroup
                ddlmajorgroup.DataTextField = "Name"
                ddlmajorgroup.DataValueField = "ID"
                ddlmajorgroup.DataBind()
            End If

            dtmajorgroup.Dispose()
            dtmajorgroup = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ItemMaster_Edit:loadMajorgroup")
            Err.Clear()
        End Try
    End Sub
    Private Sub loadItemgroup()
        Try
            Dim citemgroup As New Clsitemgroup
            Dim dtitemgroup As New DataTable
            dtitemgroup = citemgroup.SelectDataset_itemgroup.Tables(0)

            If dtitemgroup.Rows.Count > 0 Then
                ddlitemgroup.DataSource = dtitemgroup
                ddlitemgroup.DataTextField = "Name"
                ddlitemgroup.DataValueField = "ID"
                ddlitemgroup.DataBind()
            End If

            dtitemgroup.Dispose()
            dtitemgroup = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ItemMaster_Edit:loadItemgroup")
            Err.Clear()
        End Try
    End Sub
    Private Sub loadCurrency()
        Try
            Dim citemmaster As New Clsitemmaster
            Dim dtcurrency As New DataTable
            dtcurrency = citemmaster.Selectdt_currency
            If dtcurrency.Rows.Count > 0 Then
                ddlCurrency.DataSource = dtcurrency
                ddlCurrency.DataTextField = "Name"
                ddlCurrency.DataValueField = "ID"
                ddlCurrency.DataBind()
            End If

            dtcurrency.Dispose()
            dtcurrency = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ItemMaster_Edit:loadCurrency")
            Err.Clear()
        End Try
    End Sub
    Private Sub loadmenuitemizer()
        Try
            Dim citemmaster As New Clsitemmaster
            Dim dtmenuitemizer As New DataTable
            dtmenuitemizer = citemmaster.Selectdt_menuitemizer
            If dtmenuitemizer.Rows.Count > 0 Then
                ddlMenuItemizer.DataSource = dtmenuitemizer
                ddlMenuItemizer.DataTextField = "Name"
                ddlMenuItemizer.DataValueField = "ID"
                ddlMenuItemizer.DataBind()
            End If

            dtmenuitemizer.Dispose()
            dtmenuitemizer = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ItemMaster_Edit:loadmenuitemizer")
            Err.Clear()
        End Try
    End Sub
    Private Sub loadchargeitemizer()
        Try
            Dim citemmaster As New Clsitemmaster
            Dim dtchargeitemizer As New DataTable
            dtchargeitemizer = citemmaster.Selectdt_chargeitemizer
            If dtchargeitemizer.Rows.Count > 0 Then
                ddlChargeItemizer.DataSource = dtchargeitemizer
                ddlChargeItemizer.DataTextField = "Name"
                ddlChargeItemizer.DataValueField = "ID"
                ddlChargeItemizer.DataBind()
            End If

            dtchargeitemizer.Dispose()
            dtchargeitemizer = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ItemMaster_Edit:loadchargeitemizer")
            Err.Clear()
        End Try
    End Sub
    Private Sub loadunit()
        Try
            Dim citemmaster As New Clsitemmaster
            Dim dtunit As New DataTable
            dtunit = citemmaster.Selectdt_unit
            If dtunit.Rows.Count > 0 Then
                ddlUnit.DataSource = dtunit
                ddlUnit.DataTextField = "Name"
                ddlUnit.DataValueField = "ID"
                ddlUnit.DataBind()
            End If

            dtunit.Dispose()
            dtunit = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ItemMaster_Edit:loadunit")
            Err.Clear()
        End Try
    End Sub
    Private Sub loadSLU()
        Try
            'Dim citemmaster As New Clsitemmaster
            'Dim dtSLU As New DataTable
            'dtSLU = citemmaster.SelectDatatable_SLU
            'If dtSLU.Rows.Count > 0 Then
            '    ddlSLU.DataSource = dtSLU
            '    ddlSLU.DataTextField = "Name"
            '    ddlSLU.DataValueField = "ID"
            '    ddlSLU.DataBind()
            'End If

            'dtSLU.Dispose()
            'dtSLU = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ItemMaster_Edit:loadSLU")
            Err.Clear()
        End Try
    End Sub
    Protected Sub btnInsert_Click(ByVal sender As Object, ByVal e As EventArgs) ' Handles btnInsert.Click
        insert()
    End Sub

    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As EventArgs) 'Handles btnUpdate.Click
        insert()
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) 'Handles btnCancel.Click

        Try
            hapus()
            mfunction.CreateCookie("ID", 0)


            Response.Redirect("~/itemmasterlist.aspx", False)
        Catch ex As Exception
            errLog(Err.Description, "StoreGrpEdit:btnCancel_Click")
            Err.Clear()
        End Try

    End Sub




    Sub insert()

        Dim hslOption As Integer
        ' Dim mod_option As New mod_Enum
        Dim irow As Integer = 0

        For Each item As ListViewItem In lvwOption.Items
            Dim ck As CheckBox = DirectCast(item.FindControl("cbxOptionDetail"), CheckBox)

            If ck.Checked = True Then
                hslOption = mfunction.Save_ItemOption(irow, hslOption)
            End If

            irow += 1
        Next
        Try
            If chk_eff.Checked = True And chk_end.Checked = True Then
                citemmaster.insertitemmasterupdate(txtnama.Text, txtnumber.Text, ViewState("selectedId"), ddlitemgroup.SelectedValue, "1", ddlCurrency.SelectedValue, ddlMenuItemizer.SelectedValue, ddlChargeItemizer.SelectedValue, ddlUnit.SelectedValue, CDate(txteffdate.Text), CDate(txtenddate.Text), hslOption, 0, txtfactor1.Text, txtnmfactor1.Text, txtfactor2.Text, txtnmfactor2.Text, txtbarcode.Text, txtLang1.Text, txtLang2.Text, txtLang3.Text, susername, CDate(DateTime.Now), ViewState("item_oldname"))

            ElseIf chk_eff.Checked = True And chk_end.Checked = False Then
                citemmaster.insertitemmaster_eff(txtnama.Text, txtnumber.Text, ViewState("selectedId"), ddlitemgroup.SelectedValue, "1", ddlCurrency.SelectedValue, ddlMenuItemizer.SelectedValue, ddlChargeItemizer.SelectedValue, ddlUnit.SelectedValue, CDate(txteffdate.Text), hslOption, 0, txtfactor1.Text, txtnmfactor1.Text, txtfactor2.Text, txtnmfactor2.Text, txtbarcode.Text, txtLang1.Text, txtLang2.Text, txtLang3.Text, susername, CDate(DateTime.Now), ViewState("item_oldname"))

            ElseIf chk_end.Checked = True And chk_eff.Checked = False Then
                citemmaster.insertitemmaster_end(txtnama.Text, txtnumber.Text, ViewState("selectedId"), ddlitemgroup.SelectedValue, "1", ddlCurrency.SelectedValue, ddlMenuItemizer.SelectedValue, ddlChargeItemizer.SelectedValue, ddlUnit.SelectedValue, CDate(txtenddate.Text), hslOption, 0, txtfactor1.Text, txtnmfactor1.Text, txtfactor2.Text, txtnmfactor2.Text, txtbarcode.Text, txtLang1.Text, txtLang2.Text, txtLang3.Text, susername, CDate(DateTime.Now))

            End If
            messagesuccesfull("Item saved successfully", True)
            Response.Redirect("~/ItemMasterList.aspx", False)

            Exit Sub

        Catch ex As Exception
            errLog(Err.Description, "StoreGrpEdit:insert")
            Err.Clear()
        End Try

    End Sub
    Sub storegroupExist()
        Dim message As String = "Area already exist!"
        Dim sb As New System.Text.StringBuilder()

        sb.Append("<script type = 'text/javascript'>")
        sb.Append("window.onload=function(){")
        sb.Append("alert('")
        sb.Append(message)
        sb.Append("')};")
        sb.Append("</script>")
        ClientScript.RegisterClientScriptBlock(Me.GetType(), "Alert", sb.ToString())
    End Sub

    Protected Sub ddlmajorgroup_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlmajorgroup.SelectedIndexChanged

    End Sub

    Protected Sub ddlitemgroup_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlitemgroup.SelectedIndexChanged

    End Sub

    Sub cbxOption_Clicked(sender As Object, e As EventArgs)
        Dim ckheader As CheckBox = DirectCast(sender, CheckBox)

        If ckheader.Checked = True Then
            For Each item As ListViewItem In lvwOption.Items
                Dim ckitem As CheckBox = DirectCast(item.FindControl("cbxOptionDetail"), CheckBox)
                ckitem.Checked = True
            Next
        Else
            For Each item As ListViewItem In lvwOption.Items
                Dim ckitem As CheckBox = DirectCast(item.FindControl("cbxOptionDetail"), CheckBox)
                ckitem.Checked = False
            Next
        End If

    End Sub
    Sub Checkshowdet_Clicked(sender As Object, e As EventArgs)
        Dim ckshowdet As CheckBox = DirectCast(sender, CheckBox)

        If ckshowdet.Checked = True Then
            DetailInputs.Visible = True
        Else
            DetailInputs.Visible = False
        End If

    End Sub
    Protected Sub chk_eff_CheckedChanged(sender As Object, e As EventArgs) Handles chk_eff.CheckedChanged
        If chk_eff.Checked = True Then
            txteffdate.Enabled = True
        Else
            txtenddate.Enabled = False
        End If

    End Sub

    Protected Sub chk_end_CheckedChanged(sender As Object, e As EventArgs) Handles chk_end.CheckedChanged
        If chk_end.Checked = True Then
            txtenddate.Enabled = True
        Else
            txtenddate.Enabled = False
        End If

    End Sub
    Sub messagesuccesfull(ByVal msg As String, Optional ByVal bsuccess As Boolean = False)

        Dim message As String = msg '"Import From Excel Successfully."
        Dim url As String = ""

        If bsuccess Then
            url = "~/ItemMasterList.aspx"
            'Else
            '    url = "/Pages_ExportInv_Stk.aspx"
        End If

        Dim script As String = "window.onload = function(){ alert('"
        script += message
        script += "');"
        script += "window.location = '"
        script += url
        script += "'; }"
        ClientScript.RegisterStartupScript(Me.GetType(), "Redirect", script, True)

    End Sub
    Sub notsuccesfull(ByVal msg As String)
        Dim message As String = msg
        Dim sb As New System.Text.StringBuilder()
        Dim url As String = "~/ItemMasterList.aspx"
        sb.Append("<script type = 'text/javascript'>")
        sb.Append("window.onload=function(){")
        sb.Append("alert('")
        sb.Append(message)
        sb.Append("')};")
        sb.Append("</script>")
        ClientScript.RegisterClientScriptBlock(Me.GetType(), "Alert", sb.ToString())
    End Sub

    Sub fillplease()
        Dim message As String = "Fill Please File Name"
        Dim sb As New System.Text.StringBuilder()
        Dim url As String = "~/ItemMasterList.aspx"
        sb.Append("<script type = 'text/javascript'>")
        sb.Append("window.onload=function(){")
        sb.Append("alert('")
        sb.Append(message)
        sb.Append("')};")
        sb.Append("</script>")
        ClientScript.RegisterClientScriptBlock(Me.GetType(), "Alert", sb.ToString())
    End Sub
End Class