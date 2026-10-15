Imports System.Threading
Imports System.Net
Imports System.Text
Imports System.IO
Imports System.Web.UI.HtmlControls

Public Class Pages_ReceipeEdit
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
    Private MTid As Integer
    Private Crcpdtl As New Cls_IRcpDtl
    Private CRcp As New Cls_IRcp
    Private CIcc As New Cls_ICc


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
            MTid = Request.Cookies("ID").Value
            If Request.Cookies("UserLogin_inv") IsNot Nothing Then
                sNameuser = username()
                icustid = custid()
                imodul = modul()
                iModPriv = modulepriv()
                irptpriv = rptpriv()
                irptf = rptf()
                If IsPostBack = False Then
                    'editlist(MTid)
                    If MTid = 0 Then

                        hapus()
                        Bindingcc1()
                        btnnew.Visible = False
                        btnSearch.Visible = False
                        btnaddcc.Visible = False
                    Else
                        editlist(MTid)
                        binding(MTid)

                        btnnew.Visible = True
                        btnSearch.Visible = True
                    End If
                End If
                ScriptManager.GetCurrent(Me).RegisterPostBackControl(RadButton1)
                'This for Search Button
                txtsearch.Attributes.Add("onkeyup", Me.Page.ClientScript.GetPostBackEventReference(txtsearch, ""))
                txtsearch.Attributes.Add("onkeyup", Me.Page.ClientScript.GetPostBackEventReference(txtSearchcc, ""))
                'ScriptManager.GetCurrent(Me).RegisterPostBackControl(RadButton1)
                txtsearch.Attributes.Add("onfocus", "javascript: this.setSelectionRange(this.value.length,this.value.length);")
                txtRemarks.Attributes.Add("maxlength", txtRemarks.MaxLength.ToString)
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

    Protected Sub Page_unLoad(sender As Object, e As EventArgs) Handles Me.Unload
    End Sub

    Public Function username() As String
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        Return cookie("userName")

    End Function

    Sub hapus()
        Try
            txtName.Text = ""
            txtNo.Text = ""
            txtCost.Text = 0
            txtRemarks.Text = ""

            btnUpdate.Visible = False
            btnInsert.Visible = True
            ViewState("codeawal") = ""
        Catch ex As Exception
            errLog(Err.Description, "Pages_ReceipeEdit:hapus")
            Err.Clear()
        End Try
    End Sub

    Sub editlist(ByVal id As String)
        Try
            Dim ds As New DataSet
            ds = CRcp.SelectDataset_rcpEdit(id, custid)
            If ds Is Nothing OrElse ds.Tables.Count = 0 Then
                Throw New InvalidOperationException("Unable to load recipe details.")
            End If
            If ds.Tables(0).Rows.Count = 0 Then
                Throw New InvalidOperationException("Recipe not found for the current customer.")
            End If

            txtNo.Text = ds.Tables(0).Rows(0).Item("code")
            ViewState("codeawal") = ds.Tables(0).Rows(0).Item("code")
            txtName.Text = ds.Tables(0).Rows(0).Item("Name")
            txtCost.Text = ds.Tables(0).Rows(0).Item("Cost")
            txtRemarks.Text = ds.Tables(0).Rows(0).Item("Remarks")
            'DdlType.SelectedValue = ds.Tables(0).Rows(0).Item("Type")

            Dim costData As DataSet = CRcp.SelectDataset_rcpCost(custid, txtNo.Text, username, modulepriv)
            If costData Is Nothing OrElse costData.Tables.Count = 0 Then
                Throw New InvalidOperationException("Unable to load recipe cost centres.")
            End If
            Dim dt As DataTable = costData.Tables(0)
            ViewState("costcc") = dt
            binding2()
            bindingcc()
            btnUpdate.Visible = True
            btnInsert.Visible = False
        Catch ex As Exception
            errLog(ex.Message, "Pages_ReceipeEdit:editlist")
            btnUpdate.Visible = False
            btnInsert.Visible = False
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "recipeLoadError",
                "document.getElementById('recipeLoadError').style.display = 'block';", True)
            Err.Clear()
        End Try

    End Sub

    Protected Sub btnInsert_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnInsert.ServerClick
        Try
            'If txtNo.Text = "" Or txtName.Text = "" Then
            '    'notsuccesfull("Code or Name can't be empty")
            '    'Exit Sub
            '    Dim myStringVariable As String = "Code or Name can't be empty"
            '    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('" + myStringVariable + "');", True)
            'Else
            insert()
            'End If
        Catch ex As Exception

        End Try

    End Sub

    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnUpdate.ServerClick
        Try
            If txtNo.Text = "" Or txtName.Text = "" Then
                'notsuccesfull("Code or Name can't be empty")
                'Exit Sub
                Dim myStringVariable As String = "Code or Name can't be empty"
                ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('" + myStringVariable + "');", True)
            Else
                insert()
            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancel.ServerClick
        Try
            hapus()

            mfunction.CreateCookie("rcp", 0)

            Response.Redirect("~/Pages_Receipe.aspx", False)
            Context.ApplicationInstance.CompleteRequest()
            Return


        Catch ex As Exception
            errLog(Err.Description, "Pages_ReceipeEdit:btnCancel_Click")
            Err.Clear()
        End Try
    End Sub

    Protected Sub OnPagePropertiesChanging(ByVal sender As Object, ByVal e As PagePropertiesChangingEventArgs)
        binding(MTid)
    End Sub
    Sub insert()
        'Dim act As Boolean
        Dim msg As String = ""
        Dim newLine As String = "\n"
        Dim dtccins As New DataTable
        Dim cinv_rcp As New clsInventory_Receipe
        Dim clsinv_itemmaster As New clsInventory_ItemMaster
        Try
            If txtNo.Text = "" Then
                msg = "number must be fill" & newLine
            End If
            If txtName.Text = "" Then
                msg = msg + "name must be fill" & newLine
            End If
            'If txtCost.Text = "" Or txtCost.Text = 0 Then
            '    msg = msg + "cost must be fill" & newLine
            'End If
            If msg = "" Then
                insertRcp(MTid, custid, txtNo.Text, txtName.Text, txtCost.Text, txtRemarks.Text, username)
                If ViewState("costcc") IsNot Nothing Then
                    dtccins = DirectCast(ViewState("costcc"), DataTable)
                    If dtccins.Rows.Count > 0 Then
                        insertcc()
                    End If
                End If
                cinv_rcp.Process_UpdateCostReceipe(custid, Now.ToString("yyyy-MM-dd 00:00:00"), txtNo.Text)
                clsinv_itemmaster.Process_UpdateCostitemMaster(custid, MTid)
            Else
                notsuccesfull(msg)
                Exit Sub
            End If


            Response.Redirect("~/Pages_Receipe.aspx", False)
        Catch ex As Exception
            errLog(Err.Description, "Pages_ReceipeEdit:insert")
            Err.Clear()
        End Try
    End Sub
    Public Sub insertRcp(ByVal id As Integer, ByVal custid As Integer, ByVal code As String, ByVal nama As String, ByVal cost As Integer, ByVal remarks As String, ByVal username As String)
        Dim activity As String = ""
        MTid = Cls_IRcp.SaveRecipe(custid, code, nama, remarks, CDec(cost), activity)
        insertLogModule(username, "InvRcp", activity, custid)
    End Sub
    Sub delete(ByVal id As Integer)
        Try
            DeleteRcpDtl(id, custid, username)
            binding(MTid)
        Catch ex As Exception

        End Try

    End Sub
    Sub deletecc(ByVal id As Integer, ByVal ccname As String)
        Try
            Dim dtcc As DataTable = DirectCast(ViewState("Invcc"), DataTable)
            dtcc.Rows.Add(id, custid, ccname)
            dtcc.AcceptChanges()
            ViewState("Invcc") = dtcc
            Deletecostcc(id, MTid, custid)
            binding1()
            bindingcc()
        Catch ex As Exception

        End Try

    End Sub
    Public Sub DeleteRcpDtl(ByVal id As Integer, ByVal Custid As Integer, ByVal username As String)
        Using db As New RecipeDb()
            db.SetParameter("id", id)
            db.SetParameter("custid", Custid)
            db.Execute("DELETE FROM cld_inv_rcpdtl WHERE id=@id AND custid=@custid")
            db.Commit()
        End Using
        insertLogModule(username, "InvRcpdtl", "Delete", Custid)
    End Sub
    Public Sub Deletecostcc(ByVal id As Integer, ByVal rcpid As Integer, ByVal Custid As Integer)
        If My.Settings.rcp_cc <> 1 Then Return
        Using db As New RecipeDb()
            db.SetParameter("id", id)
            db.SetParameter("rcpid", rcpid)
            db.SetParameter("custid", Custid)
            db.Execute("DELETE FROM cld_inv_rcp_cc WHERE rcpid=@rcpid AND ccid=@id AND custid=@custid")
            db.Commit()
        End Using
        insertLogModule(username, "InvRcpdtl", "Delete", Custid)
    End Sub
    Sub binding(ByVal rcpid As Integer)

        Try
            If custid() = 0 Then
                Response.Redirect("~/Default.aspx")
            Else
                Dim ds As New DataSet
                If modulepriv() = 0 Then
                    ds = Crcpdtl.SelectDataset_rcpdtl_withcost(custid, rcpid)
                Else
                    ds = Crcpdtl.SelectDataset_rcpdtl(custid, rcpid)
                End If

                If ds.Tables(0).Rows.Count > 0 Then
                    btnaddcc.Visible = True
                    If ds IsNot Nothing AndAlso ds.Tables.Count > 0 Then
                        lstViewDetails.DataSource = ds
                    Else
                        lstViewDetails.DataSource = Nothing
                    End If
                    lstViewDetails.DataBind()

                    ' Control header visibility after DataBind
                    If modulepriv() = 0 Then
                        Dim thCostUnit3 As HtmlTableCell = DirectCast(lstViewDetails.FindControl("thCostUnit3"), HtmlTableCell)
                        Dim thTotalCost As HtmlTableCell = DirectCast(lstViewDetails.FindControl("thTotalCost"), HtmlTableCell)
                        If thCostUnit3 IsNot Nothing Then thCostUnit3.Visible = True
                        If thTotalCost IsNot Nothing Then thTotalCost.Visible = True
                    End If
                Else
                    btnaddcc.Visible = False
                End If


            End If
        Catch ex As Exception

        End Try


    End Sub

    Protected Sub btnnew_ServerClick(sender As Object, e As EventArgs) Handles btnnew.ServerClick
        Try
            mfunction.CreateCookie("rcpIdrcp", 0)
            mfunction.CreateCookie("rcpID", MTid)

            Response.Redirect("~/Pages_ReceipeDetailAdd.aspx", False)
        Catch ex As Exception
            errLog(Err.Description, "Pages_ReceipeEdit:btnNew_Click")
            Err.Clear()
        End Try
    End Sub
    Protected Sub lstView_ItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ListViewCommandEventArgs) Handles lstViewDetails.ItemCommand

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
    'Protected Sub LsvStk_ItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ListViewCommandEventArgs) Handles LsvStk_ItemCommand.ItemCommand


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


                'Case "Edt"

                '    Dim arg As String() = e.CommandArgument.ToString().Split(";")


                '    mfunction.CreateCookie("rcpIdrcp", arg(0))
                '    mfunction.CreateCookie("rcptipe", arg(1))
                '    mfunction.CreateCookie("rcpID", MTid)
                '    mfunction.CreateCookie("rcpunt", arg(2))

                '    Response.Redirect("~/Pages_ReceipeDtlEdit.aspx")

        End Select
    End Sub
    Protected Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.ServerClick
        Dim ds As New DataSet

        Dim clsReceipe As New clsInventory_Receipe
        If modulepriv() = 0 Then
            ds = clsReceipe.SelectDataset_rcpdtlSearch_withcost(txtsearch.Text, custid, MTid)
        Else
            ds = clsReceipe.SelectDataset_rcpdtlSearch(txtsearch.Text, custid, MTid)
        End If
        clsReceipe = Nothing

        If ds IsNot Nothing AndAlso ds.Tables.Count > 0 Then
            lstViewDetails.DataSource = ds
        Else
            lstViewDetails.DataSource = Nothing
        End If
        lstViewDetails.DataBind()

        ' Control header visibility after DataBind
        If modulepriv() = 0 Then
            Dim thCostUnit3 As HtmlTableCell = DirectCast(lstViewDetails.FindControl("thCostUnit3"), HtmlTableCell)
            Dim thTotalCost As HtmlTableCell = DirectCast(lstViewDetails.FindControl("thTotalCost"), HtmlTableCell)
            If thCostUnit3 IsNot Nothing Then thCostUnit3.Visible = True
            If thTotalCost IsNot Nothing Then thTotalCost.Visible = True
        End If
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
    Sub binding2() 'ambil cc mana saja di itemxcc

        Dim c_rcp As New Cls_IRcp
        Dim dtcostcc As New DataTable

        Dim ds As New DataSet
        'ds = WService.getItemMaster(cust)
        If custid() = 0 Then
            Response.Redirect("~/Default.aspx", False)
        Else
            dtcostcc = DirectCast(ViewState("costcc"), DataTable)
            lstViewCost.DataSource = dtcostcc
            lstViewCost.DataBind()
        End If
        'ds = c_rcp.SelectDataset_rcpCost(custid, txtNo.Text)



    End Sub
    Sub binding1() 'ambil cc mana saja di itemxcc

        Dim c_rcp As New Cls_IRcp
        Dim dtcostcc As New DataTable

        Dim ds As New DataSet
        'ds = WService.getItemMaster(cust)
        If custid() = 0 Then
            Response.Redirect("~/Default.aspx", False)
        Else
            dtcostcc = CRcp.SelectDataset_rcpCost(custid, txtNo.Text, username, modulepriv).Tables(0)
            ViewState("costcc") = dtcostcc
            lstViewCost.DataSource = DirectCast(ViewState("costcc"), DataTable)
            lstViewCost.DataBind()
        End If
        'ds = c_rcp.SelectDataset_rcpCost(custid, txtNo.Text)



    End Sub
    Protected Sub OnPagecostChanging(ByVal sender As Object, ByVal e As PagePropertiesChangingEventArgs)
        lstViewCostPager1.SetPageProperties(e.StartRowIndex, e.MaximumRows, False)

        binding2()
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

                        'Dim u1 As String = ds.Tables(0).Rows(0).Item("Unit1").ToString
                        '    Dim factor1 As String = ds.Tables(0).Rows(0).Item("factor1").ToString
                        '    Dim dCost As Decimal = ds.Tables(0).Rows(0).Item("coststk").ToString
                        '    Dim dstockcc As Decimal = ds.Tables(0).Rows(0).Item("stockcc") / ds.Tables(0).Rows(0).Item("factor1")
                        '    Dim dstock As Decimal = ds.Tables(0).Rows(0).Item("stock") / ds.Tables(0).Rows(0).Item("factor1")
                        '    Dim dstklimit As Decimal = ds.Tables(0).Rows(0).Item("stk_limit") / ds.Tables(0).Rows(0).Item("factor1")
                        '    Dim snumber As String = ds.Tables(0).Rows(0).Item("Number").ToString
                        '    Dim u2 As String = ds.Tables(0).Rows(0).Item("Unit2").ToString
                        'Dim dt As DataTable = DirectCast(ViewState("costcc"), DataTable)
                        Dim dt As New DataTable '= DirectCast(ViewState("costcc"), DataTable)
                        If (DirectCast(ViewState("costcc"), DataTable)) IsNot Nothing Then
                            dt = DirectCast(ViewState("costcc"), DataTable)
                        Else
                            dt = CRcp.SelectDataset_rcpCost(custid, txtNo.Text).Tables(0)
                            ViewState("costcc") = dt
                        End If
                        'Debug.Print((DirectCast(item.FindControl("lblID"), Label).Text))
                        'Debug.Print((DirectCast(item.FindControl("lblName"), Label).Text))
                        'Debug.Print(u1.ToString)
                        'Debug.Print(factor1.ToString)

                        dt.Rows.Add(MTid, custid, txtNo.Text, txtName.Text, 0, (DirectCast(item.FindControl("lblName"), Label).Text), (DirectCast(item.FindControl("lblID"), Label).Text))

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




                        txtsearch.Text = ""
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
            'MsgBox(ex.Message)
        End Try

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



                dt2.DefaultView.RowFilter = "ccname like '%" & txtSearchcc.Text.Replace("'", "''") & "%'"
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
    'Protected Sub BindData()
    '    If custid() = 0 Then
    '        Response.Redirect("~/Default.aspx")
    '    Else

    '        DgvTransfer.DataSource = DirectCast(ViewState("orderW"), DataTable)
    '        DgvTransfer.DataBind()
    '        'DgvTransfer.


    '    End If
    'End Sub
    Sub Bindingcc1() 'buat ngurangin list cc dengan yang sudah kelink
        If icustid = 0 Then
            Response.Redirect("~/Default.aspx", False)
        Else

            'Bram4Sep2018 Update Report
            Dim dt, dt2 As New DataTable
            If (DirectCast(ViewState("costcc"), DataTable)) IsNot Nothing Then
                dt = DirectCast(ViewState("costcc"), DataTable)
            End If

            'dt2 = CIcc.Selectdt_CCsearch(dt, custid, txtsearch.Text)
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
    Protected Sub btnClearSearch_Click(sender As Object, e As EventArgs)
        For Each item As ListViewItem In LsvStk.Items
            Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox"), CheckBox)

            ck.Checked = False


        Next
        txtsearch.Text = ""
        'lblnoitem.Text = 0
        'BindData()
        Bindingcc1()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Pop", "openModalcc();", True)
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
                                           .CCName = dt.Item("name"),
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

        End Try
    End Sub

    Sub insertcc()
        Dim dtcc As DataTable = TryCast(ViewState("costcc"), DataTable)
        If My.Settings.rcp_cc <> 1 OrElse dtcc Is Nothing OrElse dtcc.Rows.Count = 0 Then Return
        Using db As New RecipeDb()
            db.SetParameter("id", MTid)
            db.SetParameter("custid", custid)
            If db.Scalar("SELECT id FROM cld_inv_rcp WHERE id=@id AND custid=@custid FOR UPDATE") Is Nothing Then
                Throw New InvalidOperationException("Recipe not found for this customer.")
            End If
            db.Execute("DELETE FROM cld_inv_rcp_cc WHERE rcpid=@id AND custid=@custid")
            For Each row As DataRow In dtcc.Rows
                db.SetParameter("ccid", Convert.ToInt32(row("ccid")))
                db.SetParameter("cost", Convert.ToDecimal(row("cost2")))
                If db.Scalar("SELECT id FROM cld_inv_cc WHERE id=@ccid AND custid=@custid") Is Nothing Then
                    Throw New InvalidOperationException("Cost centre not found for this customer.")
                End If
                db.Execute("INSERT INTO cld_inv_rcp_cc (rcpid,custid,cost,ccid) VALUES (@id,@custid,@cost,@ccid)")
            Next
            db.Commit()
        End Using
        insertLogModule(username, "Invrcpdtl", "Update", custid)
    End Sub
    Public Sub insertrcpxcostcc(ByVal id As Integer, ByVal ccid As Integer, ByVal cost As Double, ByVal custid As Integer)
        If My.Settings.rcp_cc <> 1 Then Return
        Using db As New RecipeDb()
            db.SetParameter("id", id)
            db.SetParameter("ccid", ccid)
            db.SetParameter("cost", cost)
            db.SetParameter("custid", custid)
            db.Execute("INSERT INTO cld_inv_rcp_cc (rcpid,custid,cost,ccid) VALUES (@id,@custid,@cost,@ccid)")
            db.Commit()
        End Using
        insertLogModule(username, "Invrcpdtl", "Update", custid)
    End Sub

    Protected Sub txtNo_TextChanged(sender As Object, e As EventArgs) Handles txtNo.TextChanged
        If txtNo.Text = ViewState("codeawal") Then

            errNo.Text = ""
            errNo.Visible = False
        Else
            errNo.Visible = True
            errNo.Text = "this code will saved as new receipe"
        End If
    End Sub

End Class
