Imports Microsoft.AspNet.Identity
Imports System.Reflection
Imports ClosedXML.Excel
Imports System.IO

Public Class ReportViewer
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
    Private clsExport As New clsExportData
    Public storename As String = ""
    Dim versionNumber As Version = Assembly.GetExecutingAssembly().GetName().Version
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
                    ddlRpt.Items.Add("Sales Summary Report")
                    ddlRpt.Items.Add("Payment Summary")
                    ddlRpt.Items.Add("Recept Detail")
                    ddlRpt.Items.Add("Sales Pos Vs Web")
                    ddlRpt.Items.Add("Item Sales")
                    ' ddlRpt.Items.Add("Cashier Report")
                    ddlRpt.Items.Add("Top Store Revenue")
                    'tambahan bram 23 maret 2018
                    ddlRpt.Items.Add("SalesSum ItemGroup")

                    ddlRpt.Items.Add("Top 10 Item by Qty")
                    ddlRpt.Items.Add("Top10 ItembyAmount")
                    ddlRpt.Items.Add("Top10 Group by Qty")
                    ddlRpt.Items.Add("Top10 GroupbyAmount")
                    'tambahan 28 maret 2018
                    ddlRpt.Items.Add("Hourly Visitor Sales")
                    'tambahan 19 july 2019
                    ddlRpt.Items.Add("SalesDetail")

                    dataParam = New ClsParamRpt
                    dataParam.custID = icustid


                    'SetOutlet()
                    loadKepemilikan()
                    'BindingStk()
                    BindingStore()
                    BindingBrand()
                    BindingCity()


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
                dt2 = clsExport.SelectDataset_Outlet2(ddlOwn.SelectedValue, lblnobrand.Text, lblnoCity.Text, icustid, txtSearch.Text, "", modulepriv)

            Else
                dt2 = clsExport.SelectDataset_Outlet(ddlOwn.SelectedValue, lblnobrand.Text, icustid, txtSearch.Text, "", modulepriv)

            End If
            LsvStk.DataSource = dt2
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
    Sub messagesuccesfull(ByVal message As String)

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

    'Untuk Button Submit
    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs)

    End Sub

    Protected Sub btnSubmit2_Click(ByVal sender As Object, ByVal e As System.EventArgs)

    End Sub

    Protected Sub btnSubmit3_Click(ByVal sender As Object, ByVal e As System.EventArgs)

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
            lblOutletNameData.Text = "Selected Store"

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
                        shsl = (DirectCast(item.FindControl("LblIdBrand"), Label).Text)
                    Else
                        shsl = shsl & "," & (DirectCast(item.FindControl("LblIdBrand"), Label).Text)
                    End If
                End If

            Next

            lblnobrand.Text = shsl
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
                        shsl = (DirectCast(item.FindControl("LblIdCity"), Label).Text)
                    Else
                        shsl = shsl & "," & (DirectCast(item.FindControl("LblIdCity"), Label).Text)
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

            lblnoCity.Text = shsl
            LblNmCity.Text = "Selected City"


        Catch ex As Exception
            'MsgBox(ex.Message)
        End Try

    End Sub

    'Untuk Button All
    Protected Sub btnall_Click(ByVal sender As Object, ByVal e As System.EventArgs)

    End Sub

    Protected Sub btnall2_Click(ByVal sender As Object, ByVal e As System.EventArgs)

    End Sub

    Protected Sub btnall3_Click(ByVal sender As Object, ByVal e As System.EventArgs)

    End Sub

    Sub SetClearColoum()

        'lbl1.Text = ""
        'lbl2.Text = ""
        'lbl3.Text = ""
        'lbl4.Text = ""
        'lbl5.Text = ""
        'lbl6.Text = ""
        'lbl7.Text = ""
        'lbl8.Text = ""
        'lbl9.Text = ""
        'lbl10.Text = ""
        'lbl11.Text = ""

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
            sFile = ddlRpt.Text.ToString & " " & txtFrom.Text & " - " & txtTo.Text
            ' End If

            'set dataparam
            dataParam = New ClsParamRpt
            dataParam = set_dataparam(True)


            ds = clsreport.getDataset(ddlRpt.Text, dataParam)

            If ds.Tables.Count > 0 Then
                dt = ds.Tables(0)

                Dim csv As String = String.Empty

                For Each column As DataColumn In dt.Columns
                    'Add the Header row for CSV file.
                    csv += column.ColumnName + ","c
                Next

                'Add new line.
                csv += vbCr & vbLf

                For Each row As DataRow In dt.Rows
                    For Each column As DataColumn In dt.Columns
                        'Add the Data rows.
                        csv += row(column.ColumnName).ToString().Replace(",", ";") + ","c
                    Next

                    'Add new line.
                    csv += vbCr & vbLf
                Next

                'Download the CSV file.
                Response.Clear()
                Response.Buffer = True
                Response.AddHeader("content-disposition", "attachment;filename=" & sFile & ".csv")
                Response.Charset = ""
                Response.ContentType = "application/text"
                Response.Output.Write(csv)

                HttpContext.Current.Response.Flush()
                HttpContext.Current.Response.SuppressContent = True
                HttpContext.Current.ApplicationInstance.CompleteRequest()
            End If

            'If ds.Tables.Count > 0 Then
            '    dt = ds.Tables(0)

            '    Using wb As New XLWorkbook()
            '        wb.Worksheets.Add(dt, "Sheet1")
            '        Response.Clear()
            '        Response.Buffer = True
            '        Response.Charset = ""
            '        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            '        Response.AddHeader("content-disposition", "attachment;filename=" & sFile & ".xlsx")
            '        Using MyMemoryStream As New MemoryStream()
            '            wb.SaveAs(MyMemoryStream)
            '            MyMemoryStream.WriteTo(Response.OutputStream)
            '            'Response.Flush()
            '            'Response.End()

            '            HttpContext.Current.Response.Flush()
            '            HttpContext.Current.Response.SuppressContent = True
            '            HttpContext.Current.ApplicationInstance.CompleteRequest()

            '        End Using
            '    End Using

            'End If

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

        'Dim dtparam As New ClsParamRpt
        'dtparam = New cls_ParamRpt

        dataParam.custID = icustid
        dataParam.Modpriv = iModPriv
        dataParam.usr = sNameuser
        dataParam.paramFrom = tglAwal
        dataParam.paramTo = tglAkhir
        If ddlOwn.Visible = True Then
            Dim Own As Integer = 0

            If Not (ddlOwn.SelectedValue = "0" Or ddlOwn.SelectedValue = "") Then
                Own = ddlOwn.SelectedValue
            End If

            'myRptDoc.SetParameterValue("ownID", Own)
            dataParam.ownID = Own
        Else
            dataParam.ownID = 0
        End If
        'ambil data outlet
        Dim storeNo As String = ""

            If Not (lblNoOutlet.Text = "0" Or lblNoOutlet.Text = "") Then
                storeNo = lblNoOutlet.Text
            End If
        dataParam.storestr = storeNo
        'ambil data brand
        Dim brandNo As String = ""

        If Not (lblnobrand.Text = "0" Or lblnobrand.Text = "") Then
            brandNo = lblnobrand.Text
        End If
        dataParam.brandstr = brandNo
        'ambil data city
        Dim cityNo As String = ""

        If Not (lblnoCity.Text = "0" Or lblnoCity.Text = "") Then
            cityNo = lblnoCity.Text
        End If
        dataParam.citystr = cityNo
        'ambil data item
        Dim itemNo As String = ""

        If Not (lblnoitem.Text = "0" Or lblnoitem.Text = "") Then
            itemNo = lblnoitem.Text
        End If
        dataParam.itemstr = itemNo





        Return dataParam

    End Function

    Protected Sub btnShow_Click(sender As Object, e As EventArgs) Handles btnShow.Click
        If ddlRpt.SelectedIndex >= 0 Then
            showRpt()
        End If
    End Sub

    Private Sub showRpt()
        Try
            If CDate(txtTo.Text) < CDate(txtFrom.Text) Then
                messagesuccesfull("date range not valid")
                Exit Sub
            End If
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

            ' setColumName()
            'perubahan 04 jul 2017
            If ds.Tables.Count > 0 Then
                dg1.DataSource = ds
                dg1.DataBind()
            Else
                dg1.DataSource = Nothing
                dg1.DataBind()
            End If

            ds.Dispose()

        Catch ex As Exception
            mfunction.errLog(Err.Description, "Report:showRpt")
            Err.Clear()
        End Try
    End Sub

    Protected Sub btnSubmit3_Click1(sender As Object, e As EventArgs)
        Try
            Add3()
            BindingStk()
            'loadOutlet()
            'Add2()
            'Add3()
        Catch ex As Exception
            'MsgBox(ex.Message)
        End Try
    End Sub

    Protected Sub btnSearch3_Click1(sender As Object, e As EventArgs)
        BindingCity()
    End Sub

    Protected Sub btnSearch_Click1(sender As Object, e As EventArgs)
        BindingStk()
    End Sub

    Protected Sub btnSubmit_Click1(sender As Object, e As EventArgs)
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

    Protected Sub btnall_Click1(sender As Object, e As EventArgs)
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
            lblOutletNameData.Text = "All Store"

            'End If

        Catch ex As Exception
            'MsgBox(ex.Message)
        End Try
    End Sub

    Protected Sub btnSearch2_Click1(sender As Object, e As EventArgs)
        BindingBrand()
    End Sub

    Protected Sub btnSubmit2_Click1(sender As Object, e As EventArgs)
        Try
            Add2()
            BindingStk()
            ' loadOutlet()
            'Add2()
            'Add3()
        Catch ex As Exception
            'MsgBox(ex.Message)
        End Try
    End Sub

    Protected Sub btnAll2_Click1(sender As Object, e As EventArgs)
        Try
            Dim shsl As String = "0"

            'Update Ihsan 19032019 
            For Each item As ListViewItem In ListViewBrand.Items
                Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox2"), CheckBox)

                ck.Checked = False

            Next

            lblnobrand.Text = shsl
            lblNmBrand.Text = "All Brand"
            'loadOutlet()
            BindingStk()

        Catch ex As Exception
            'MsgBox(ex.Message)
        End Try
    End Sub

    Protected Sub btnAll3_Click1(sender As Object, e As EventArgs)
        Try
            Dim shsl As String = "0"

            'Update Ihsan 19032019 
            For Each item As ListViewItem In ListViewCity.Items
                Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox3"), CheckBox)

                ck.Checked = False

            Next

            lblnoCity.Text = shsl
            LblNmCity.Text = "All City"
            'loadOutlet()
            BindingStk()

        Catch ex As Exception
            'MsgBox(ex.Message)
        End Try
    End Sub
    Sub BindingStore2()
        If icustid = 0 Then
            Response.Redirect("~/Default.aspx", False)
        Else

            'Bram4Sep2018 Update Report
            Dim dtstore2 As DataTable = DirectCast(ViewState("vsoutlet"), DataTable)

            If dtstore2.Rows.Count = 0 Then
                dtstore2 = clsExport.SelectDataset_Outlet2(ddlOwn.SelectedValue, lblnobrand.Text, lblnoCity.Text, icustid, txtSearch.Text, sNameuser, iModPriv).Tables(0)
                ViewState("vsoutlet") = dtstore2
            Else

                Dim sOwn As String = ""
                Dim sBrand As String = ""
                Dim sCity As String = ""
                Dim sout As String = ""
                If lblnobrand.Text = "0" Or lblnobrand.Text = "" Then
                    sBrand = ""
                Else
                    sBrand = " and brandID in (" & lblnobrand.Text & ")"
                End If

                If ddlOwn.SelectedValue = "0" Or ddlOwn.SelectedValue = "" Then
                    sOwn = ""
                Else
                    sOwn = " and ownID = " & ddlOwn.SelectedValue
                End If
                If lblnoCity.Text = "0" Or lblnoCity.Text = "" Then
                    sCity = ""
                Else
                    sCity = " and cityID in (" & lblnoCity.Text & ")"
                End If

                dtstore2.DefaultView.RowFilter = "custid = " & icustid & sOwn & sBrand & sCity & "and (Kode like  '%" & txtSearch.Text & "%' Or nama like '%" & txtSearch.Text & "%')"
                ViewState("vsoutlet") = dtstore2
            End If

            LsvStk.DataSource = dtstore2.DefaultView
            LsvStk.DataBind()
        End If
    End Sub
    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        BindingStore2()
    End Sub
    Sub BindingStore()
        If icustid = 0 Then
            Response.Redirect("~/Default.aspx", False)
        Else

            'Bram4Sep2018 Update Report
            Dim dtstore2 As New DataTable


            dtstore2 = clsreport.getds_viewOutlet(ddlOwn.SelectedValue, lblnobrand.Text, lblnoCity.Text, icustid, txtSearch.Text, sNameuser, iModPriv).Tables(0)
            ViewState("vsoutlet") = dtstore2


            LsvStk.DataSource = dtstore2.DefaultView
            LsvStk.DataBind()
        End If
    End Sub
End Class