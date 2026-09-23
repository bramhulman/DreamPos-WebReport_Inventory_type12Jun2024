Public Class CCoutlet
    Inherits System.Web.UI.Page
    Private sNameuser As String
    Private icustid As Integer
    Private imodul As Integer
    Private iModPriv As Integer
    Private irptpriv As Integer
    Private irptf As Integer
    Private mfunction As New ModFunction
    Private cSupp As New clsInventory_Supplier
    Private cicc As New clsInventory_Cc
    Private clsExport As New clsExportData
    Private clsreport As New ClsReports
    'Private userweb As Integer

    'Private transpriv As Integer

    <Serializable>
    Public Class Supplier_Detail 'Class untuk ListSupplier
        Public Property SupplierName As String = ""
        Public Property Status As Boolean = False
    End Class

    <Serializable>
    Public Class SupplierId
        Public Property Supplierid As Integer = 0
    End Class

    'Start : Store
    Private Sub BindCheckSupplier()
        Try

            If CBool(IIf(IsNothing(ViewState("ListSupplier")), True, False)) Then


                Dim ListSupplier2 As List(Of Supplier_Detail) = New List(Of Supplier_Detail)()

                Dim dsSupplier As New DataSet

                dsSupplier = clsExport.SelectDataset_Supplier(icustid, "")

                If dsSupplier.Tables(0).Rows.Count > 0 Then
                    For Each dt As DataRow In dsSupplier.Tables(0).Rows
                        ListSupplier2.Add(New Supplier_Detail With {
                                           .SupplierName = dt.Item("name"),
                                           .Status = False
                                           })
                    Next
                End If


                For Each item As ListViewItem In LsvStk.Items
                    Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox"), CheckBox)
                    Dim Labels As Label = DirectCast(item.FindControl("lblName"), Label)
                    Dim label2 As String = Labels.Text



                    If ck.Checked Then
                        For Each a In ListSupplier2
                            If a.SupplierName = label2 Then
                                a.Status = True
                            End If
                        Next
                        '  clsCheck.IDBrand.Add(label2)
                    End If
                Next

                ViewState("ListSupplier") = ListSupplier2

            Else

                Dim ListSupplier2 As List(Of Supplier_Detail) = TryCast(ViewState("ListSupplier"), List(Of Supplier_Detail))

                For Each item As ListViewItem In LsvStk.Items
                    Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox"), CheckBox)
                    Dim Labels As Label = DirectCast(item.FindControl("lblName"), Label)
                    Dim label2 As String = Labels.Text

                    If ck.Checked Then
                        For Each a In ListSupplier2
                            If a.SupplierName = label2 Then
                                a.Status = True
                            End If
                        Next

                    Else
                        For Each a In ListSupplier2
                            If a.SupplierName = label2 Then
                                a.Status = False
                            End If
                        Next
                    End If
                Next
                ViewState("ListSupplier") = ListSupplier2
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub CheckedSupplier()
        Try


            Dim ListSupplier As List(Of Supplier_Detail) = TryCast(ViewState("ListSupplier"), List(Of Supplier_Detail))

            Dim i As Integer = 0
            For Each item As ListViewItem In LsvStk.Items
                Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox"), CheckBox)
                Dim Labels As Label = DirectCast(item.FindControl("lblName"), Label)
                Dim label2 As String = Labels.Text

                For Each a In ListSupplier
                    If a.SupplierName = label2 Then
                        If a.Status = True Then
                            ck.Checked = True
                        Else
                            ck.Checked = False
                        End If

                    End If
                Next

            Next
            ViewState("ListSupplier") = ListSupplier
        Catch ex As Exception

        End Try
    End Sub
    'END : Store


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
    Public Function username() As String
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        Return cookie("userName")

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
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")

        If Not cookie Is Nothing Then
            sNameuser = username()
            icustid = custid()
            imodul = modul()
            iModPriv = modulepriv()
            irptpriv = rptpriv()
            irptf = rptf()
            If Not Me.IsPostBack Then
                loadCC()
                'loadKepemilikan()
                BindingSupplier()
                'getactive()
                Me.binding()
                'ddlUser.SelectedIndex = userweb
                txtSearch.Attributes.Add("onkeyup", Me.Page.ClientScript.GetPostBackEventReference(txtSearch, ""))
                Checkbox_allstore.Attributes.Add("onclick", Me.Page.ClientScript.GetPostBackEventReference(Checkbox_allstore, ""))
                'ddlActive.Attributes.Add("onchange", "confirm2()")
                ScriptManager.GetCurrent(Me).RegisterPostBackControl(RadButton1)
            End If

        Else
            Dim script As String = "window.onload = function(){ alert('"
            script += "Please Login"
            script += "');"
            script += "window.location = '"
            script += "../Dashboard.aspx"
            script += " '; }"
            ClientScript.RegisterStartupScript(Me.GetType(), "Redirect", script, True)
        End If

    End Sub

    'Private Sub loadKepemilikan()
    '    Try

    '        Dim ds As New DataSet
    '        'Revisi 08 Juni
    '        ds = clsExport.SelectDataset_Kepemilikan(icustid)

    '        If ds.Tables.Count > 0 Then
    '            With ddlOwn
    '                .DataSource = ds.Tables(0)
    '                .DataTextField = "Nama"
    '                .DataValueField = "ID"
    '                .DataBind()
    '            End With
    '        End If

    '        ds.Dispose()
    '        ds = Nothing

    '    Catch ex As Exception
    '        errLog(Err.Description, "rptViewer:loadKepemilikan")
    '        Err.Clear()
    '    End Try
    'End Sub

    Sub BindingSupplier()
        If icustid = 0 Then
            Response.Redirect("~/Default.aspx", False)
        Else

            'Bram4Sep2018 Update Report
            Dim dtsupplier2 As New DataTable


            dtsupplier2 = cSupp.SelectDataset_SupplierLoadXtrans(custid, ddlCC.SelectedValue, txtSearch.Text).Tables(0)
            'ViewState("vssupplier") = dtsupplier2

            If dtsupplier2.Rows.Count > 0 Then
                LsvStk.Items.Clear()
                countlvstore = dtsupplier2.Rows.Count
                LsvStk.DataSource = dtsupplier2.DefaultView
                LsvStk.DataBind()
                UpdatePanel3.Update()
            End If

        End If
    End Sub
    'Sub getactive()


    '    'Bram4Sep2018 Update Report
    '    Dim active As Boolean = True

    '    active = cuser.getactive(ddlUser.Text, custid)
    '    If active Then
    '        ddlActive.SelectedValue = "active"
    '    Else
    '        ddlActive.SelectedValue = "nonactive"
    '    End If



    'End Sub
    Protected Sub Page_unLoad(sender As Object, e As EventArgs) Handles Me.Unload
        Try
            'WService.Dispose()

        Catch ex As Exception
            MsgBox(ex.Message)
            mfunction.errLog(Err.Description, "PosUser:Page_unLoad")
            Err.Clear()
        End Try
    End Sub

    Public Function Id_User() As Integer
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        Return cookie("ID")
    End Function

    Public Function FlagStatus() As Integer
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        Return cookie("flagstatus")
    End Function

    Public Function UserTypeID() As Integer
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        Return cookie("UserTypeID")
    End Function

    'Revisi 08 Juni
    Public Function StoreID() As Integer
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        Return cookie("StoreID")

    End Function

    Public Function OutletID() As String
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        Return cookie("OutletID")
    End Function



    Sub binding()
        ' Dim Cust As Integer
        ' Dim cookie As HttpCookie = Request.Cookies("UserName")
        If icustid = 0 Then
            Response.Redirect("~/Default.aspx", False)
        Else
            'Cust = cookie("CustID")
        End If
        Dim ds As New DataSet
        ds = cSupp.SelectDataset_SupplierCCview(icustid, ddlCC.SelectedValue, txtSearch.Text)
        If ds IsNot Nothing AndAlso ds.Tables.Count > 0 Then
            lstViewDetails.DataSource = ds
        Else
            lstViewDetails.DataSource = Nothing
        End If
        lstViewDetails.DataBind()
    End Sub
    Sub loadCC()
        ' Dim cookie As HttpCookie = Request.Cookies("UserName")

        Dim ds As New DataSet
        ds = cicc.SelectDataset_IccOnly(custid, username, modulepriv)
        If ds IsNot Nothing AndAlso ds.Tables.Count > 0 Then
            ddlCC.DataSource = ds
        Else
            ddlCC.DataSource = Nothing
        End If
        ddlCC.DataValueField = "id"
        ddlCC.DataTextField = "name"
        ddlCC.DataBind()

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

                'Case "Edt"
                '    ID = Convert.ToInt32(e.CommandArgument)

                '    'editlist(ID)
                '    mfunction.CreateCookie("PosUser_ID", e.CommandArgument)

                '    Response.Redirect("PosUser_Edit")

        End Select

    End Sub
    Protected Sub btnInsert_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnInsert.Click
        insert()
        'Response.Redirect("PosUser.aspx")
        txtSearch.Text = ""
        Checkbox_allstore.Checked = False
        For Each item As ListViewItem In LsvStk.Items
            Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox"), CheckBox)
            'Update Ihsan 19032019

            If ck.Checked Then
                ck.Checked = False
            End If


        Next

    End Sub

    Sub insert()
        'If ddlOutlet.SelectedIndex <> 0 Then
        Dim ListSupplierNumber As New List(Of String)
        Dim ListSupplier2 As List(Of Supplier_Detail) = TryCast(ViewState("ListSupplier"), List(Of Supplier_Detail))


        If CBool(IIf(IsNothing(ViewState("ListSupplier")), True, False)) Then
            ListSuppNumber = TryCast(ViewState("ListSupplierNumber"), List(Of String))
        Else
            For Each a In ListSupplier2
                If a.Status = True Then
                    ListSupplierNumber.Add(cSupp.SelectDataSupplier_id(a.SupplierName))
                End If
            Next
        End If


        cicc.insertSupplierX(ddlCC.SelectedValue, ListSupplierNumber)
        binding()
        ' ddlOutlet.Items.Clear()
        'ddlOutlet.Items.Insert(0, New ListItem("--Select Outlet--"))
        BindingSupplier()


    End Sub
    Sub notsuccesfull()
        Dim message As String = "Please Select Outlet"
        Dim sb As New System.Text.StringBuilder()

        sb.Append("<script type = 'text/javascript'>")
        sb.Append("window.onload=function(){")
        sb.Append("alert('")
        sb.Append(message)
        sb.Append("')};")
        sb.Append("</script>")
        ClientScript.RegisterClientScriptBlock(Me.GetType(), "Alert", sb.ToString())
    End Sub
    Sub delete(ByVal id As Integer)


        cSupp.DeleteCCXtranSupplier(id, icustid, ddlCC.SelectedValue, username)

        binding()
        BindingSupplier()
        ' ddlOutlet.Items.Clear()
        ' ddlOutlet.Items.Insert(0, New ListItem("--Select Outlet--"))
        ' loadOutlet()
    End Sub

    'Sub updateactive(ByVal active As Boolean)

    '    cuser.Update_user(custid, ddlUser.SelectedValue, active)



    'End Sub
    Protected Sub OnPagePropertiesChanging(ByVal sender As Object, ByVal e As PagePropertiesChangingEventArgs)
        lvDataPager1.SetPageProperties(e.StartRowIndex, e.MaximumRows, False)

        binding()
    End Sub
    Protected Sub Ddlcc_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlCC.SelectedIndexChanged
        ' userweb = ddlUser.SelectedIndex

        'ddlOutlet.Items.Clear()
        'ddlOutlet.Items.Insert(0, New ListItem("--Select Outlet--"))
        BindingSupplier()
        'getactive()
        binding()
        GeName()

    End Sub

    Protected Function GeName() As String

        Dim str As String = ""
        str = "CC Access For " & ddlCC.SelectedItem.ToString


        Return str
    End Function

    Protected Sub ddlOwn_SelectedIndexChanged(sender As Object, e As EventArgs)
        BindingSupplier()
    End Sub

    Sub Add(Optional ByVal pilih As Boolean = True)

        Try
            Dim shsl As String = ""
            Dim i As Integer = 0
            Dim ListSupplierNumber As List(Of String) = New List(Of String)()

            If pilih = False Then
                For Each item As ListViewItem In LsvStk.Items
                    Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox"), CheckBox)
                    'Update Ihsan 19032019

                    If ck.Checked Then
                        ck.Checked = False
                    End If


                Next
                '  lblOutletNameData.Text = "All Store"
            Else
                For Each item As ListViewItem In LsvStk.Items
                    Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox"), CheckBox)
                    'Update Ihsan 19032019

                    If ck.Checked Then
                        'If shsl = "" Then
                        '    shsl = (DirectCast(item.FindControl("lblkode"), Label).Text)
                        'Else
                        '    shsl = shsl & "," & (DirectCast(item.FindControl("lblkode"), Label).Text)
                        'End If
                        'shsl = (DirectCast(item.FindControl("lblkode"), Label).Text)

                        ListSupplierNumber.Add(DirectCast(item.FindControl("lblkode"), Label).Text)

                    End If


                Next

                ViewState("ListSupplierNumber") = ListSupplierNumber

            End If




        Catch ex As Exception
            'MsgBox(ex.Message)
        End Try

    End Sub

    Protected Sub RadButton1_ServerClick(sender As Object, e As EventArgs)
        Add()
        BindCheckSupplier()
    End Sub


    Protected Sub btnAll1_ServerClick(sender As Object, e As EventArgs)
        Checkbox_allstore.Checked = True
        For Each item As ListViewItem In LsvStk.Items
            Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox"), CheckBox)
            ck.Checked = Checkbox_allstore.Checked
        Next
    End Sub

    Protected Sub Checkbox_allstore_CheckedChanged(sender As Object, e As EventArgs)
        For Each item As ListViewItem In LsvStk.Items
            Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox"), CheckBox)
            ck.Checked = Checkbox_allstore.Checked
        Next

        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Pop", "openModalSupplier();", True)
    End Sub

    Protected Sub txtSearch_TextChanged(sender As Object, e As EventArgs)
        Threading.Thread.Sleep(500)
        BindCheckSupplier()
        BindingSupplier()
        CheckedSupplier()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Pop", "openModalSupplier();", True)
        txtSearch.Attributes("onfocus") = "javascript:this.setSelectionRange(this.value.length,this.value.length);"
        txtSearch.Focus()
    End Sub

    Private Sub btnSearch_ServerClick(sender As Object, e As EventArgs) Handles btnSearch.ServerClick
        If txtSearch.Text <> "" Then

            BindCheckSupplier()
            BindingSupplier()
            CheckedSupplier()
        End If
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Pop", "openModalSupplier();", True)
    End Sub

    Protected Sub btnsrc_suppXcc(ByVal sender As Object, ByVal e As EventArgs)
        Dim ds As New DataSet
        ds = cSupp.SelectDataset_SupplierLoadXtrans(icustid, ddlCC.SelectedValue, txtsrc_outxuser.Text)
        If ds IsNot Nothing AndAlso ds.Tables.Count > 0 Then
            lstViewDetails.DataSource = ds
        Else
            lstViewDetails.DataSource = Nothing
        End If
        lstViewDetails.DataBind()
        txtsrc_outxuser.Text = ""
    End Sub
    Protected Sub btnClearSupplier_ServerClick(sender As Object, e As EventArgs) Handles btnClearSupplier.ServerClick
        Try
            For Each item As ListViewItem In LsvStk.Items
                Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox"), CheckBox)

                ck.Checked = False


            Next
            txtSearch.Text = ""
            lblNoOutlet.Text = 0
            BindingSupplier()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Pop", "openModalSupplier();", True)
        Catch ex As Exception
            mfunction.errLog(Err.Description, "CCoutlet:btnClearSupplier_ServerClick")
            Err.Clear()
        End Try
    End Sub

    Private Sub ShowToastr(ByVal page As Page, ByVal message As String, ByVal title As String, Optional ByVal type As String = "info", Optional ByVal clearToast As Boolean = False, Optional ByVal pos As String = "toast-top-left", Optional ByVal Sticky As Boolean = False)
        Dim toastrScript As String = [String].Format("Notify('{0}','{1}','{2}', '{3}', '{4}', '{5}');", message, title, type, clearToast, pos, Sticky)
        page.ClientScript.RegisterStartupScript(page.[GetType](), "toastr_message", toastrScript, addScriptTags:=True)
    End Sub

    'Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged

    'End Sub
End Class
