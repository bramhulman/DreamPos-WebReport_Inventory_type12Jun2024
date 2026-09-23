Imports Microsoft.AspNet.Identity
Imports System.Reflection
Imports ClosedXML.Excel
Imports System.IO
Public Class reportviewer
    Inherits System.Web.UI.Page
    Private sNameuser As String
    Private icustid As Integer
    Private imodul As Integer
    Private iModPriv As Integer
    Private irptpriv As Integer
    Private irptf As Integer
    Private mfunction As New ModFunction
    'Public WService As New WServices.WebService1
    Private dataParam As New ClsParamRpt
    Private clsreport As New ClsReports
    Private clsOut As New ClsOutlet
    Public storename As String = ""
    Dim versionNumber As Version = Assembly.GetExecutingAssembly().GetName().Version
    Private clsExport As New clsExportData
    'Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    'End Sub
    Public Function modulepriv() As Integer
        Dim cookie As HttpCookie = Request.Cookies("UserLogin")
        Return cookie("modulepriv")

    End Function
    Public Function custid() As Integer
        Dim cookie As HttpCookie = Request.Cookies("UserLogin")

        If cookie Is Nothing Then
            Return -1
        Else
            Return cookie("Custid")
        End If

    End Function
    Public Function username() As String
        Dim cookie As HttpCookie = Request.Cookies("UserLogin")
        Return cookie("userName")

    End Function
    Public Function rptpriv() As Integer
        Dim cookie As HttpCookie = Request.Cookies("UserLogin")

        Return cookie("rptpriv")

    End Function
    Public Function modul() As Integer
        Dim cookie As HttpCookie = Request.Cookies("UserLogin")
        Return cookie("modul")

    End Function
    Public Function rptf() As Integer
        Dim cookie As HttpCookie = Request.Cookies("UserLogin")

        Return cookie("rptf")

    End Function
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            If Request.Cookies("UserLogin") IsNot Nothing Then
                sNameuser = username()
                icustid = custid()
                imodul = modul()
                iModPriv = modulepriv()
                irptpriv = rptpriv()
                irptf = rptf()
                If IsPostBack = False Then
                    txtFrom.Text = Format(Now, "dd MMM yyyy")
                    txtTo.Text = Format(Now, "dd MMM yyyy")
                    btnShow.Text = ".SHOW EXPORT DATA."

                    ddlRpt.Items.Clear()
                    ddlRpt.Items.Add("Sales Summary")
                    ddlRpt.Items.Add("Payment Summary")
                    ddlRpt.Items.Add("Recept Detail")
                    ddlRpt.Items.Add("Sales Pos Vs Web")
                    ddlRpt.Items.Add("Item Sales")
                    ddlRpt.Items.Add("Cashier Report")
                    ddlRpt.Items.Add("Sales Summary By type Sales")
                    ddlRpt.Items.Add("Top Store Revenue")
                    ddlRpt.Items.Add("Void detail")


                    dataParam = New ClsParamRpt
                    dataParam.custID = icustid
                    loadKepemilikan()
                    loadCity2()
                    BindingStore()

                    SetClearColoum()
                End If
            Else
                Dim script As String = "window.onload = Function(){ alert('"
                script += "Please Login"
                script += "');"
                script += "window.location = '"
                script += "../Default"
                script += " '; }"
                ClientScript.RegisterStartupScript(Me.GetType(), "Redirect", script, True)

            End If

        Catch ex As Exception

            mfunction.errLog(Err.Description, "ItemGroup_Edit:Page_Load")
            Err.Clear()
        End Try
    End Sub
    Protected Sub Page_unLoad(sender As Object, e As EventArgs) Handles Me.Unload
        Try
            'WService.Dispose()


        Catch ex As Exception

            mfunction.errLog(Err.Description, "ItemGroup_Edit:Page_unLoad")
            Err.Clear()
        End Try
    End Sub
    'Sub SetOutlet()

    '    Dim dt2 As New DataSet

    '    dt2 = clsreport.getDataset("outlet", dataParam)
    '    ddlStore.DataSource = dt2
    '    ddlStore.DataValueField = "ID"
    '    ddlStore.DataTextField = "Nama"
    '    ddlStore.DataBind()

    'End Sub

    Sub SetClearColoum()

        lbl1.Text = ""
        lbl2.Text = ""
        lbl3.Text = ""
        lbl4.Text = ""
        lbl5.Text = ""
        lbl6.Text = ""
        lbl7.Text = ""
        lbl8.Text = ""
        lbl9.Text = ""
        lbl10.Text = ""
        lbl11.Text = ""

    End Sub

    Sub setColumName()

        If ddlRpt.SelectedIndex = 0 Then
            lbl1.Text = "TransDate"
            lbl2.Text = "Subtotal"
            lbl3.Text = "Discount"
            lbl4.Text = "Charge"
            lbl5.Text = "Tax"
            lbl6.Text = "Total"
            lbl7.Text = "Pax"
            lbl8.Text = "Avg Pax"
            lbl9.Text = "QtyTrans"
            lbl10.Text = "Avg Trans"
            lbl11.Text = "Rounding"

        ElseIf ddlRpt.SelectedIndex = 1 Then
            lbl1.Text = "TransCode"
            lbl2.Text = "TransName"
            lbl3.Text = "TotalQty"
            lbl4.Text = "Subtotal"
            lbl5.Text = "Discount"
            lbl6.Text = "Total"
            lbl7.Text = "Total Tax"
            lbl8.Text = ""
            lbl9.Text = ""
            lbl10.Text = ""
            lbl11.Text = ""

        ElseIf ddlRpt.SelectedIndex = 2 Then
            lbl1.Text = "SalesType"
            lbl2.Text = "Subtotal"
            lbl3.Text = "Discount"
            lbl4.Text = "Charge"
            lbl5.Text = "Tax"
            lbl6.Text = "Total"
            lbl7.Text = "Pax"
            lbl8.Text = "QtyTrans"
            lbl9.Text = ""
            lbl10.Text = ""
            lbl11.Text = ""

        ElseIf ddlRpt.SelectedIndex = 3 Then
            lbl1.Text = "ItemGroup"
            lbl2.Text = "TotalQty"
            lbl3.Text = "Subtotal"
            lbl4.Text = "Discount"
            lbl5.Text = "Total"
            lbl6.Text = ""
            lbl7.Text = ""
            lbl8.Text = ""
            lbl9.Text = ""
            lbl10.Text = ""
            lbl11.Text = ""

        ElseIf ddlRpt.SelectedIndex = 4 Then
            lbl1.Text = "PaymentName"
            lbl2.Text = "PaymentQty"
            lbl3.Text = "PaymentTotal"
            lbl4.Text = ""
            lbl5.Text = ""
            lbl6.Text = ""
            lbl7.Text = ""
            lbl8.Text = ""
            lbl9.Text = ""
            lbl10.Text = ""
            lbl11.Text = ""

        ElseIf ddlRpt.SelectedIndex = 5 Or ddlRpt.SelectedIndex = 6 Then
            lbl1.Text = "TransCode"
            lbl2.Text = "TransName"
            lbl3.Text = "TotalQty"
            lbl4.Text = "Total"
            lbl5.Text = ""
            lbl6.Text = ""
            lbl7.Text = ""
            lbl8.Text = ""
            lbl9.Text = ""
            lbl10.Text = ""
            lbl11.Text = ""
        ElseIf ddlRpt.SelectedIndex = 7 Or ddlRpt.SelectedIndex = 8 Then
            lbl1.Text = "ItemGroup"
            lbl2.Text = "TotalQty"
            lbl3.Text = "Total"
            lbl4.Text = ""
            lbl5.Text = ""
            lbl6.Text = ""
            lbl7.Text = ""
            lbl8.Text = ""
            lbl9.Text = ""
            lbl10.Text = ""
            lbl11.Text = ""

        ElseIf ddlRpt.SelectedIndex = 9 Then
            lbl1.Text = "Day"
            lbl2.Text = "Hour"
            lbl3.Text = "Cover"
            lbl4.Text = "QtyTrans"
            lbl5.Text = "Subtotal"
            lbl6.Text = "AVG Cover"
            lbl7.Text = "AVG Trans"
            lbl8.Text = ""
            lbl9.Text = ""
            lbl10.Text = ""
            lbl11.Text = ""


        ElseIf ddlRpt.SelectedIndex = 10 Then
            lbl1.Text = "SalesNo"
            lbl2.Text = "TransDate"
            lbl3.Text = "SalesType"
            lbl4.Text = "Pax"
            lbl5.Text = "TableName"
            lbl6.Text = "Subtotal"
            lbl7.Text = "Disc"
            lbl8.Text = "Charge"
            lbl9.Text = "Tax"
            lbl10.Text = "SalesTotal"
            lbl11.Text = "PaymentName"

        End If


    End Sub
    'Private Sub ddlStore_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlStore.SelectedIndexChanged
    '    SetClearColoum()
    'End Sub

    Sub binding()

    End Sub


    Sub loadOutlet()

        Dim ds As New DataSet
        ds = clsOut.SelectDataset_Outlet(custid, username, modulepriv)

    End Sub

    Protected Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        Try

            Dim dt As DataTable
            Dim ds As New DataSet

            'Dim tglAwal As Date
            'Dim tglAkhir As Date

            Dim sFile As String = ""

            ' If ddlRpt.SelectedIndex = 0 Then
            '    tglAwal = CDate(txtFrom.Text)
            '    tglAkhir = CDate(txtTo.Text)
            sFile = ddlRpt.Text.ToString & txtFrom.Text & " - " & txtTo.Text
            ' End If

            'set dataparam
            dataParam = New ClsParamRpt
            dataParam = set_dataparam(True)


            ds = clsreport.getDataset(ddlRpt.Text, dataParam)


            If ds.Tables.Count > 0 Then
                dt = ds.Tables(0)

                Using wb As New XLWorkbook()
                    wb.Worksheets.Add(dt, "Sheet1")
                    Response.Clear()
                    Response.Buffer = True
                    Response.Charset = ""
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                    Response.AddHeader("content-disposition", "attachment;filename=" & sFile & ".xlsx")
                    Using MyMemoryStream As New MemoryStream()
                        wb.SaveAs(MyMemoryStream)
                        MyMemoryStream.WriteTo(Response.OutputStream)
                        'Response.Flush()
                        'Response.End()

                        HttpContext.Current.Response.Flush()
                        HttpContext.Current.Response.SuppressContent = True
                        HttpContext.Current.ApplicationInstance.CompleteRequest()

                    End Using
                End Using

            End If

        Catch ex As Exception
            mfunction.errLog(Err.Description, "Report:ExportData")
            Err.Clear()
        Finally
            HttpContext.Current.Response.SuppressContent = True
            HttpContext.Current.ApplicationInstance.CompleteRequest()

        End Try

        Exit Sub
    End Sub
    Private Function set_dataparam(ByVal b_datatable As Boolean)

        Dim tglAwal As Date
        Dim tglAkhir As Date

        'If ddlRpt.SelectedIndex = 0 Then
        tglAwal = CDate(txtFrom.Text)
        tglAkhir = CDate(txtTo.Text)
        ' End If

        Dim dtparam As New ClsParamRpt
        'dtparam = New cls_ParamRpt

        dtparam.custID = icustid
        dtparam.paramFrom = tglAwal
        dtparam.paramTo = tglAkhir
        dtparam.outletNo = lblNoOutlet.Text


        Return dtparam

    End Function

    Protected Sub btnShow_Click(sender As Object, e As EventArgs) Handles btnShow.Click
        If ddlRpt.SelectedIndex >= 0 Then
            showRpt()
        End If
    End Sub

    Private Sub showRpt()
        Try

            Dim ds As New DataSet

            'set dataparam
            dataParam = New ClsParamRpt
            dataParam = set_dataparam(False)

            'dataParam.custID = StoreID()
            'dataParam.paramFrom = tglAwal
            'dataParam.paramTo = tglAkhir
            'dataParam.outletID = ddlStore.SelectedValue
            'dataParam.bDatatable = False

            ds = clsreport.getDataset(ddlRpt.Text, dataParam)

            setColumName()
            'perubahan 04 jul 2017
            If ds.Tables.Count > 0 Then
                lstViewReport.DataSource = ds
                lstViewReport.DataBind()
            Else
                lstViewReport.DataSource = Nothing
                lstViewReport.DataBind()
            End If

            ds.Dispose()

        Catch ex As Exception
            mfunction.errLog(Err.Description, "Report:showRpt")
            Err.Clear()
        End Try
    End Sub

    Protected Sub OnPagePropertiesChanging(ByVal sender As Object, ByVal e As PagePropertiesChangingEventArgs)
        'lvDataPager1.SetPageProperties(e.StartRowIndex, e.MaximumRows, False)

        binding()
    End Sub
    Sub BindingStk()
        '  Dim dt2 As New DataSet

        ' dt2 = clsreport.getDataset("outlet", dataParam)
        ' ddlStore.DataSource = dt2
        '  ddlStore.DataValueField = "ID"
        '  ddlStore.DataTextField = "Nama"
        '    ddlStore.DataBind()

        If icustid = 0 Then
            Response.Redirect("~/Default.aspx", False)
        Else
            Dim dt2 As New DataSet

            'Bram4Sep2018 Update Report
            If ddlRpt.SelectedIndex >= 4 And ddlRpt.SelectedIndex <= 22 Then
                dt2 = clsExport.SelectDataset_Outlet2(ddlOwn.SelectedValue, lblNoBrand.Text, lblNoCity.Text, icustid, txtSearch.Text, "", modulepriv)

            Else
                dt2 = clsExport.SelectDataset_Outlet(ddlOwn.SelectedValue, lblNoBrand.Text, icustid, txtSearch.Text, "", modulepriv)

            End If
            LsvStk.DataSource = dt2
            LsvStk.DataBind()
        End If
    End Sub
    Sub BindingStore()
        If icustid = 0 Then
            Response.Redirect("~/Default.aspx", False)
        Else

            'Bram4Sep2018 Update Report
            Dim dtstore2 As New DataTable


            dtstore2 = clsreport.getds_viewOutlet(ddlOwn.SelectedValue, lblNoBrand.Text, lblNoCity.Text, icustid, txtSearch.Text, sNameuser, iModPriv).Tables(0)
            ViewState("vsoutlet") = dtstore2


            LsvStk.DataSource = dtstore2.DefaultView
            LsvStk.DataBind()
        End If
    End Sub

    Sub BindingStore2()
        If icustid = 0 Then
            Response.Redirect("~/Default.aspx", False)
        Else

            'Bram4Sep2018 Update Report
            Dim dtstore2 As DataTable = DirectCast(ViewState("vsoutlet"), DataTable)

            If dtstore2.Rows.Count = 0 Then
                dtstore2 = clsExport.SelectDataset_Outlet2(ddlOwn.SelectedValue, lblNoBrand.Text, lblNoCity.Text, icustid, txtSearch.Text, sNameuser, iModPriv).Tables(0)
                ViewState("vsoutlet") = dtstore2
            Else

                Dim sOwn As String = ""
                Dim sBrand As String = ""
                Dim sCity As String = ""
                Dim sout As String = ""
                If lblNoBrand.Text = "0" Or lblNoBrand.Text = "" Then
                    sBrand = ""
                Else
                    sBrand = " and brandID in (" & lblNoBrand.Text & ")"
                End If

                If ddlOwn.SelectedValue = "0" Or ddlOwn.SelectedValue = "" Then
                    sOwn = ""
                Else
                    sOwn = " and ownID = " & ddlOwn.SelectedValue
                End If
                If lblNoCity.Text = "0" Or lblNoCity.Text = "" Then
                    sCity = ""
                Else
                    sCity = " and cityID in (" & lblNoCity.Text & ")"
                End If

                dtstore2.DefaultView.RowFilter = "custid = " & icustid & sOwn & sBrand & sCity & "and (Kode like  '%" & txtSearch.Text & "%' Or nama like '%" & txtSearch.Text & "%')"
                ViewState("vsoutlet") = dtstore2
            End If

            LsvStk.DataSource = dtstore2.DefaultView
            LsvStk.DataBind()
        End If
    End Sub
    Sub BindingBrand()
        If icustid = 0 Then
            Response.Redirect("~/Default.aspx", False)
        Else
            Dim dsBrand As New DataSet

            'Bram4Sep2018 Update Report

            If ddlRpt.SelectedIndex >= 0 And ddlRpt.SelectedIndex <= 22 Then
                dsBrand = clsExport.SelectDataset_Brand(icustid)
            End If

            ListViewBrand.DataSource = dsBrand
            ListViewBrand.DataBind()
        End If
    End Sub
    Private Sub loadBrand2()
        Dim dsBrand As New DataSet
        If icustid = 0 Then
            Response.Redirect("~/Default.aspx", False)
        Else

            dsBrand = clsreport.getds_ViewBrand(icustid, txtSearch2.Text)

            ListViewBrand.DataSource = dsBrand
            ListViewBrand.DataBind()
        End If
    End Sub
    Sub BindingCity()
        If icustid = 0 Then
            Response.Redirect("~/Default.aspx", False)
        Else
            Dim dsCity As New DataSet

            'Bram4Sep2018 Update Report

            If ddlRpt.SelectedIndex >= 0 And ddlRpt.SelectedIndex <= 22 Then
                dsCity = clsExport.SelectDataset_City(icustid)
            End If

            ListViewCity.DataSource = dsCity
            ListViewCity.DataBind()
        End If
    End Sub

    Private Sub loadKepemilikan()
        Try

            Dim ds As New DataSet
            'Revisi 08 Juni
            ds = clsExport.SelectDataset_Kepemilikan(icustid)

            If ds.Tables.Count > 0 Then
                With ddlOwn
                    .DataSource = ds.Tables(0)
                    .DataTextField = "Nama"
                    .DataValueField = "ID"
                    .DataBind()
                End With
            End If

            ds.Dispose()
            ds = Nothing

        Catch ex As Exception
            errLog(Err.Description, "rptViewer:loadKepemilikan")
            Err.Clear()
        End Try
    End Sub

    Sub Add()

        Try
            Dim shsl As String = ""
            Dim i As Integer = 0
            For Each item As ListViewItem In LsvStk.Items
                Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox1"), CheckBox)
                'Update Ihsan 19032019

                If ck.Checked Then
                    If shsl = "" Then
                        shsl = (DirectCast(item.FindControl("lblID"), Label).Text)
                    Else
                        shsl = shsl & "," & (DirectCast(item.FindControl("lblID"), Label).Text)
                    End If
                End If

                'If ddlstore.SelectedValue Then
                '    If shsl = "" Then
                '        shsl = (DirectCast(item.FindControl("lblID"), Label).Text)
                '    Else
                '        shsl = shsl & "," & (DirectCast(item.FindControl("lblID"), Label).Text)
                '    End If
                'End If

            Next


            lblNoOutlet.Text = shsl
            lblOutletName.Text = "Selected Store"

        Catch ex As Exception
            'MsgBox(ex.Message)
        End Try

    End Sub

    Sub Add2()

        Try
            Dim shsl As String = ""
            Dim i As Integer = 0
            For Each item As ListViewItem In ListViewBrand.Items
                Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox2"), CheckBox)
                'Update Ihsan 19032019

                If ck.Checked Then
                    If shsl = "" Then
                        shsl = (DirectCast(item.FindControl("lblIdBrand"), Label).Text)
                    Else
                        shsl = shsl & "," & (DirectCast(item.FindControl("lblIdBrand"), Label).Text)
                    End If
                End If

            Next

            lblNoBrand.Text = shsl
            lblNmBrand.Text = "Selected Brand"


        Catch ex As Exception
            'MsgBox(ex.Message)
        End Try

    End Sub

    Sub Add3()

        Try
            Dim shsl As String = ""
            Dim i As Integer = 0
            For Each item As ListViewItem In ListViewCity.Items
                Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox3"), CheckBox)
                'Update Ihsan 19032019

                If ck.Checked Then
                    If shsl = "" Then
                        shsl = (DirectCast(item.FindControl("lblIdCity"), Label).Text)
                    Else
                        shsl = shsl & "," & (DirectCast(item.FindControl("lblIdCity"), Label).Text)
                    End If
                End If

                'If ddlstore.SelectedValue Then
                '    If shsl = "" Then
                '        shsl = (DirectCast(item.FindControl("lblID"), Label).Text)
                '    Else
                '        shsl = shsl & "," & (DirectCast(item.FindControl("lblID"), Label).Text)
                '    End If
                'End If

            Next

            lblNoCity.Text = shsl
            lblNmCity.Text = "Selected City"


        Catch ex As Exception
            'MsgBox(ex.Message)
        End Try

    End Sub

    Sub messagePilihSatu(ByVal message As String)

        'Dim message As String = "You have no privilage to this Report."
        Dim url As String = "/rptExport.aspx"
        Dim script As String = "window.onload = function(){ alert('"
        script += message
        script += "');"
        script += "window.location = '"
        script += url
        script += "'; }"
        ClientScript.RegisterStartupScript(Me.GetType(), "Redirect", script, True)
    End Sub


    'Untuk Button Search
    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        BindingStk()
    End Sub

    Private Sub btnSearch2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        BindingBrand()
    End Sub

    Private Sub btnSearch3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        BindingCity()
    End Sub

    'Untuk Button Submit
    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            If ddlRpt.SelectedIndex = 13 Then
                Add()
                If lblNoOutlet.Text = "" Then
                    messagePilihSatu("Harus Pilih Salah Satu")
                    Exit Sub
                End If
            Else
                Add()
            End If

            'Add2()
            'Add3()
        Catch ex As Exception
            'MsgBox(ex.Message)
        End Try
    End Sub

    Protected Sub btnSubmit2_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Add2()
            BindingStk()
            loadOutlet()
            'Add2()
            'Add3()
        Catch ex As Exception
            'MsgBox(ex.Message)
        End Try
    End Sub

    Protected Sub btnSubmit3_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Add3()
            BindingStk()
            loadOutlet()
            'Add2()
            'Add3()
        Catch ex As Exception
            'MsgBox(ex.Message)
        End Try
    End Sub

    'Untuk Button All
    Protected Sub btnall_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim shsl As String = "0"

            'Update Ihsan 19032019 
            'If ddlRpt.SelectedIndex = 13 Or ddlRpt.SelectedIndex = 14 Then
            '    For Each item As ListViewItem In LsvStk.Items
            '        Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox1"), CheckBox)

            '        ck.Checked = False
            '        'If ddListStore.SelectedValue = False Then
            '        '    btnSearch.Enabled = False
            '        'End If

            '    Next

            '    messagesuccesfull("Tidak Bisa Select All")
            '    Exit Sub

            'Else
            For Each item As ListViewItem In LsvStk.Items
                Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox1"), CheckBox)

                ck.Checked = False
                'If ddListStore.SelectedValue = False Then
                '    btnSearch.Enabled = False
                'End If

            Next

            lblNoOutlet.Text = shsl
            lblOutletName.Text = "All Store"

            'End If

        Catch ex As Exception
            'MsgBox(ex.Message)
        End Try
    End Sub

    Protected Sub btnall2_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim shsl As String = "0"

            'Update Ihsan 19032019 
            For Each item As ListViewItem In ListViewBrand.Items
                Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox2"), CheckBox)

                ck.Checked = False

            Next

            lblNoBrand.Text = shsl
            lblNmBrand.Text = "All Brand"
            loadOutlet()
            BindingStk()

        Catch ex As Exception
            'MsgBox(ex.Message)
        End Try
    End Sub

    Protected Sub btnall3_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim shsl As String = "0"

            'Update Ihsan 19032019 
            For Each item As ListViewItem In ListViewCity.Items
                Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox3"), CheckBox)

                ck.Checked = False

            Next

            lblNoCity.Text = shsl
            lblNmCity.Text = "All City"
            loadOutlet()
            BindingStk()

        Catch ex As Exception
            'MsgBox(ex.Message)
        End Try
    End Sub
    Protected Sub ddlRpt_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlRpt.SelectedIndexChanged

    End Sub
    Private Sub loadCity2()
        Dim dsCity As New DataSet
        If icustid = 0 Then
            Response.Redirect("~/Default.aspx", False)
        Else

            dsCity = clsreport.getds_ViewCity(icustid, txtSearch3.Text)

            ListViewCity.DataSource = dsCity
            ListViewCity.DataBind()

        End If
    End Sub
End Class