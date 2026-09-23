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
    Private cOut As New ClsOutlet
    Public storename As String = ""
    Dim versionNumber As Version = Assembly.GetExecutingAssembly().GetName().Version
    Public countlvbrand As Integer
    Public countlvcity As Integer
    Public countlvstore As Integer
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

#Region "Update Buat Search"

    <Serializable>
    Public Class Brand_Detail 'Class untuk ListBrand
        Public Property BrandName As String = ""
        Public Property Status As Boolean = False
    End Class

    <Serializable>
    Public Class City_Detail 'Class untuk ListCity
        Public Property CityName As String = ""
        Public Property Status As Boolean = False
    End Class

    <Serializable>
    Public Class Store_Detail 'Class untuk ListStore
        Public Property StoreName As String = ""
        Public Property Status As Boolean = False
    End Class

    'Start : BRAND
    Private Sub BindCheckBrand()
        Try

            If CBool(IIf(IsNothing(ViewState("ListBrand")), True, False)) Then


                Dim ListBrand2 As List(Of Brand_Detail) = New List(Of Brand_Detail)()

                Dim dsBrand As New DataSet

                dsBrand = clsExport.SelectDataset_Brand(icustid, "", False)

                If dsBrand.Tables(0).Rows.Count > 0 Then
                    For Each dt As DataRow In dsBrand.Tables(0).Rows
                        ListBrand2.Add(New Brand_Detail With {
                                           .BrandName = dt.Item("nama"),
                                           .Status = False
                                           })
                    Next
                End If


                For Each item As ListViewItem In ListViewBrand.Items
                    Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox2"), CheckBox)
                    Dim Labels As Label = DirectCast(item.FindControl("lblBrand2"), Label)
                    Dim label2 As String = Labels.Text



                    If ck.Checked Then
                        For Each a In ListBrand2
                            If a.BrandName = label2 Then
                                a.Status = True
                            End If
                        Next
                        '  clsCheck.IDBrand.Add(label2)
                    End If
                Next
                ViewState("ListBrand") = ListBrand2

            Else

                Dim ListBrand2 As List(Of Brand_Detail) = TryCast(ViewState("ListBrand"), List(Of Brand_Detail))

                '     Dim dsBrand As New DataSet




                For Each item As ListViewItem In ListViewBrand.Items
                    Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox2"), CheckBox)
                    Dim Labels As Label = DirectCast(item.FindControl("lblBrand2"), Label)
                    Dim label2 As String = Labels.Text

                    If ck.Checked Then
                        For Each a In ListBrand2
                            If a.BrandName = label2 Then
                                a.Status = True
                            End If
                        Next

                    Else
                        For Each a In ListBrand2
                            If a.BrandName = label2 Then
                                a.Status = False
                            End If
                        Next
                    End If
                Next
                ViewState("ListBrand") = ListBrand2
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub CheckedBrand()
        Try


            Dim ListBrand As List(Of Brand_Detail) = TryCast(ViewState("ListBrand"), List(Of Brand_Detail))

            Dim i As Integer = 0
            For Each item As ListViewItem In ListViewBrand.Items
                Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox2"), CheckBox)
                Dim Labels As Label = DirectCast(item.FindControl("lblBrand2"), Label)
                Dim label2 As String = Labels.Text

                For Each a In ListBrand
                    If a.BrandName = label2 Then
                        If a.Status = True Then
                            ck.Checked = True
                        Else
                            ck.Checked = False
                        End If

                    End If
                Next

            Next
            ViewState("ListBrand") = ListBrand
        Catch ex As Exception

        End Try
    End Sub
    'END : BRAND

    'Start : CITY
    Private Sub BindCheckCity()
        Try

            If CBool(IIf(IsNothing(ViewState("ListCity")), True, False)) Then


                Dim ListCity2 As List(Of City_Detail) = New List(Of City_Detail)()

                Dim dsCity As New DataSet

                dsCity = clsExport.SelectDataset_City(icustid, "")

                If dsCity.Tables(0).Rows.Count > 0 Then
                    For Each dt As DataRow In dsCity.Tables(0).Rows
                        ListCity2.Add(New City_Detail With {
                                           .CityName = dt.Item("kota"),
                                           .Status = False
                                           })
                    Next
                End If


                For Each item As ListViewItem In ListViewCity.Items
                    Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox3"), CheckBox)
                    Dim Labels As Label = DirectCast(item.FindControl("lblcity2"), Label)
                    Dim label2 As String = Labels.Text



                    If ck.Checked Then
                        For Each a In ListCity2
                            If a.CityName = label2 Then
                                a.Status = True
                            End If
                        Next
                        '  clsCheck.IDBrand.Add(label2)
                    End If
                Next
                ViewState("ListCity") = ListCity2

            Else

                Dim ListCity2 As List(Of City_Detail) = TryCast(ViewState("ListCity"), List(Of City_Detail))

                For Each item As ListViewItem In ListViewCity.Items
                    Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox3"), CheckBox)
                    Dim Labels As Label = DirectCast(item.FindControl("lblCity2"), Label)
                    Dim label2 As String = Labels.Text

                    If ck.Checked Then
                        For Each a In ListCity2
                            If a.CityName = label2 Then
                                a.Status = True
                            End If
                        Next

                    Else
                        For Each a In ListCity2
                            If a.CityName = label2 Then
                                a.Status = False
                            End If
                        Next
                    End If
                Next
                ViewState("ListCity") = ListCity2
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub CheckedCity()
        Try


            Dim ListCity As List(Of City_Detail) = TryCast(ViewState("ListCity"), List(Of City_Detail))

            Dim i As Integer = 0
            For Each item As ListViewItem In ListViewCity.Items
                Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox3"), CheckBox)
                Dim Labels As Label = DirectCast(item.FindControl("lblCity2"), Label)
                Dim label2 As String = Labels.Text

                For Each a In ListCity
                    If a.CityName = label2 Then
                        If a.Status = True Then
                            ck.Checked = True
                        Else
                            ck.Checked = False
                        End If

                    End If
                Next

            Next
            ViewState("ListCity") = ListCity
        Catch ex As Exception

        End Try
    End Sub
    'END : CITY

    'Start : Store
    Private Sub BindCheckStore()
        Try

            If CBool(IIf(IsNothing(ViewState("ListStore")), True, False)) Then


                Dim ListStore2 As List(Of Store_Detail) = New List(Of Store_Detail)()

                Dim dsStore As New DataSet

                dsStore = clsExport.SelectDataset_Store(icustid, "")

                If dsStore.Tables(0).Rows.Count > 0 Then
                    For Each dt As DataRow In dsStore.Tables(0).Rows
                        ListStore2.Add(New Store_Detail With {
                                           .StoreName = dt.Item("nama"),
                                           .Status = False
                                           })
                    Next
                End If


                For Each item As ListViewItem In LsvStk.Items
                    Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox"), CheckBox)
                    Dim Labels As Label = DirectCast(item.FindControl("lblName"), Label)
                    Dim label2 As String = Labels.Text



                    If ck.Checked Then
                        For Each a In ListStore2
                            If a.StoreName = label2 Then
                                a.Status = True
                            End If
                        Next
                        '  clsCheck.IDBrand.Add(label2)
                    End If
                Next
                ViewState("ListStore") = ListStore2

            Else

                Dim ListStore2 As List(Of Store_Detail) = TryCast(ViewState("ListStore"), List(Of Store_Detail))

                For Each item As ListViewItem In LsvStk.Items
                    Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox"), CheckBox)
                    Dim Labels As Label = DirectCast(item.FindControl("lblName"), Label)
                    Dim label2 As String = Labels.Text

                    If ck.Checked Then
                        For Each a In ListStore2
                            If a.StoreName = label2 Then
                                a.Status = True
                            End If
                        Next

                    Else
                        For Each a In ListStore2
                            If a.StoreName = label2 Then
                                a.Status = False
                            End If
                        Next
                    End If
                Next
                ViewState("ListStore") = ListStore2
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub CheckedStore()
        Try


            Dim ListStore As List(Of Store_Detail) = TryCast(ViewState("ListStore"), List(Of Store_Detail))

            Dim i As Integer = 0
            For Each item As ListViewItem In LsvStk.Items
                Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox"), CheckBox)
                Dim Labels As Label = DirectCast(item.FindControl("lblName"), Label)
                Dim label2 As String = Labels.Text

                For Each a In ListStore
                    If a.StoreName = label2 Then
                        If a.Status = True Then
                            ck.Checked = True
                        Else
                            ck.Checked = False
                        End If

                    End If
                Next

            Next
            ViewState("ListStore") = ListStore
        Catch ex As Exception

        End Try
    End Sub
    'END : Store

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (User.Identity.IsAuthenticated) Or custid() = -1 Then
            FormsAuthentication.RedirectToLoginPage()
            Exit Sub
        End If
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
                    ddlRpt.Items.Add("Receipt Detail")
                    ddlRpt.Items.Add("Sales Pos Vs Web")
                    ddlRpt.Items.Add("Item Sales")
                    ' ddlRpt.Items.Add("Cashier Report")
                    ddlRpt.Items.Add("SalesSum By Sales Type")
                    ddlRpt.Items.Add("Top Store Revenue")
                    ddlRpt.Items.Add("Void Bill")
                    ddlRpt.Items.Add("Void Item")
                    'tambahan bram 23 maret 2018
                    'ddlRpt.Items.Add("SalesSum ItemGroup")




                    'ddlRpt.Items.Add("Top 10 Item by Qty")
                    'ddlRpt.Items.Add("Top10 ItembyAmount")
                    'ddlRpt.Items.Add("Top10 Group by Qty")
                    'ddlRpt.Items.Add("Top10 GroupbyAmount")
                    'tambahan 28 maret 2018

                    'tambahan 19 july 2019
                    ddlRpt.Items.Add("Sales Detail Report")
                    ddlRpt.Items.Add("Item Go Food")
                    ddlRpt.Items.Add("Daily sales")
                    ddlRpt.Items.Add("Monthly Sales")
                    ddlRpt.Items.Add("Item Go food Daily")
                    ddlRpt.Items.Add("Sales Item")
                    ddlRpt.Items.Add("Monthly Channel")
                    ddlRpt.Items.Add("Sales Cover")
                    ddlRpt.Items.Add("Sales Vs Payment")

                    ddlRpt.Items.Add("Cashier Report")
                    ddlRpt.Items.Add("Pay At Table")

                    dataParam = New ClsParamRpt
                    dataParam.custID = icustid


                    'SetOutlet()
                    loadKepemilikan()
                    'BindingStk()
                    BindingStore()
                    BindingBrand()
                    'BindingBrand2()
                    BindingCity()


                    'SetClearColoum()

                    'Update 13 Februari 2020

                    'This for Search Button
                    txtSearch.Attributes.Add("onkeyup", Me.Page.ClientScript.GetPostBackEventReference(txtSearch, ""))
                    txtSearch2.Attributes.Add("onkeyup", Me.Page.ClientScript.GetPostBackEventReference(txtSearch2, ""))
                    txtSearch3.Attributes.Add("onkeyup", Me.Page.ClientScript.GetPostBackEventReference(txtSearch3, ""))

                    'This for Check All = True/Checked
                    Checkbox_allbrand.Attributes.Add("onclick", Me.Page.ClientScript.GetPostBackEventReference(Checkbox_allbrand, ""))
                    Checkbox_allstore.Attributes.Add("onclick", Me.Page.ClientScript.GetPostBackEventReference(Checkbox_allstore, ""))
                    Checkbox_allcity.Attributes.Add("onclick", Me.Page.ClientScript.GetPostBackEventReference(Checkbox_allcity, ""))


                    ScriptManager.GetCurrent(Me).RegisterPostBackControl(RadButton1)
                    ScriptManager.GetCurrent(Me).RegisterPostBackControl(RadButton2)
                    ScriptManager.GetCurrent(Me).RegisterPostBackControl(RadButton3)

                    'End Update 9 Januari 2020

                    'Update 20 Februari 2020
                    txtFrom.Attributes.Add("readonly", "true")
                    txtTo.Attributes.Add("readonly", "true")



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

    Sub BindingBrand()
        If icustid = 0 Then
            Response.Redirect("~/Default.aspx", False)
        Else
            Dim dsBrand As New DataSet

            'Bram4Sep2018 Update Report

            ' If ddlRpt.SelectedIndex >= 0 And ddlRpt.SelectedIndex <= 22 Then
            dsBrand = clsExport.SelectDataset_Brand(icustid, txtSearch2.Text, False)
            ' End If

            If dsBrand.Tables(0).Rows.Count > 0 Then
                ListViewBrand.Items.Clear()
                countlvbrand = dsBrand.Tables(0).Rows.Count
                ListViewBrand.DataSource = dsBrand
                ListViewBrand.DataBind()
                UpdatePanel4.Update()

            End If

            'ListViewBrand.DataSource = dsBrand
            'ListViewBrand.DataBind()
        End If
    End Sub

    Sub BindingCity()
        If icustid = 0 Then
            Response.Redirect("~/Default.aspx", False)
        Else
            Dim dsCity As New DataSet

            'Bram4Sep2018 Update Report
            dsCity = clsExport.SelectDataset_City(icustid, txtSearch3.Text)

            If dsCity.Tables(0).Rows.Count > 0 Then
                ListViewCity.Items.Clear()
                countlvcity = dsCity.Tables(0).Rows.Count
                ListViewCity.DataSource = dsCity
                ListViewCity.DataBind()
            

            End If

            'ListViewCity.DataSource = dsCity
            'ListViewCity.DataBind()
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


    Private Sub ShowToastr(ByVal page As Page, ByVal message As String, ByVal title As String, Optional ByVal type As String = "info", Optional ByVal clearToast As Boolean = False, Optional ByVal pos As String = "toast-top-left", Optional ByVal Sticky As Boolean = False)
        Dim toastrScript As String = [String].Format("Notify('{0}','{1}','{2}', '{3}', '{4}', '{5}');", message, title, type, clearToast, pos, Sticky)
        page.ClientScript.RegisterStartupScript(page.[GetType](), "toastr_message", toastrScript, addScriptTags:=True)
    End Sub

    Protected Sub btnSearch_Click1(sender As Object, e As EventArgs)
        If txtSearch.Text = "" Then
            ShowToastr(Page, "Text Search tidak boleh kosong", "Error", "Error", False, "toast-top-right", False)
        End If
    End Sub
    Protected Sub btnSearch2_Click(sender As Object, e As EventArgs)
        If txtSearch2.Text = "" Then
            ShowToastr(Page, "Text Search tidak boleh kosong", "Error", "Error", False, "toast-top-right", False)
        End If
    End Sub


    Protected Sub btnSearch3_Click(sender As Object, e As EventArgs)
        If txtSearch3.Text = "" Then
            ShowToastr(Page, "Text Search tidak boleh kosong", "Error", "Error", False, "toast-top-right", False)
        End If
    End Sub
    Sub messagesuccesfull(ByVal message As String)

        'Dim message As String = "You have no privilage To this Report."
        Dim url As String = "/rptExport.aspx"
        Dim script As String = "window.onload = Function(){ alert('"
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
        Try
            Add()
            BindCheckStore()
            'BindingStore2()
            'loadOutlet()
            'Add2()
            'Add3()
        Catch ex As Exception
            'MsgBox(ex.Message)
        End Try
    End Sub

    'brand
    Protected Sub btnSubmit2_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Add2()
        BindCheckBrand()
        BindingStore2()
        'BindingBrand()
        'loadBrand2()
    End Sub

    Protected Sub btnSubmit3_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Add3()
        BindCheckCity()
        BindingStore2()
    End Sub

    Sub Add(Optional ByVal pilih As Boolean = True)

        Try
            Dim shsl As String = ""
            Dim i As Integer = 0
            If pilih = False Then
                For Each item As ListViewItem In LsvStk.Items
                    Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox"), CheckBox)
                    'Update Ihsan 19032019

                    If ck.Checked Then
                        ck.Checked = False
                    End If


                Next
                lblNoOutlet.Text = ""
                lblOutletNameData.Text = "All Store"
            Else
                For Each item As ListViewItem In LsvStk.Items
                    Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox"), CheckBox)
                    'Update Ihsan 19032019

                    If ck.Checked Then
                        If shsl = "" Then
                            shsl = (DirectCast(item.FindControl("lblkode"), Label).Text)
                        Else
                            shsl = shsl & "," & (DirectCast(item.FindControl("lblkode"), Label).Text)
                        End If
                    End If


                Next
                If shsl = "" Then
                    lblNoOutlet.Text = ""
                    lblOutletNameData.Text = "All Store"
                Else
                    lblNoOutlet.Text = shsl
                    lblOutletNameData.Text = "Selected Store"
                End If
            End If




        Catch ex As Exception
            'MsgBox(ex.Message)
        End Try

    End Sub

    Sub Add2(Optional ByVal pilih As Boolean = True)

        Try
            Dim shsl As String = ""
            Dim i As Integer = 0
            If pilih = False Then
                For Each item As ListViewItem In ListViewBrand.Items
                    Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox2"), CheckBox)
                    'Update Ihsan 19032019

                    If ck.Checked Then
                        ck.Checked = False
                    End If

                Next

                lblnobrand.Text = ""
                lblNmBrand.Text = "All Brand"
            Else
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
                If shsl = "" Then
                    lblnobrand.Text = ""
                    lblNmBrand.Text = "All Brand"
                Else
                    lblnobrand.Text = shsl
                    lblNmBrand.Text = "Selected Brand"
                End If

            End If



        Catch ex As Exception
            'MsgBox(ex.Message)
        End Try

    End Sub

    Sub Add3(Optional ByVal pilih As Boolean = True)

        Try
            Dim shsl As String = ""
            Dim i As Integer = 0
            If pilih = False Then
                For Each item As ListViewItem In ListViewCity.Items
                    Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox3"), CheckBox)
                    'Update Ihsan 19032019

                    If ck.Checked Then
                        ck.Checked = False
                    End If

                Next

                lblnoCity.Text = ""
                LblNmCity.Text = "All City"
            Else
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

                Next
                If shsl = "" Then
                    lblnoCity.Text = ""
                    LblNmCity.Text = "All City"
                Else
                    lblnoCity.Text = shsl
                    LblNmCity.Text = "Selected City"
                End If

            End If



        Catch ex As Exception
            'MsgBox(ex.Message)
        End Try

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

        Dim ListStore2 As List(Of Store_Detail) = TryCast(ViewState("ListStore"), List(Of Store_Detail))

        If CBool(IIf(IsNothing(ViewState("ListStore")), True, False)) Then
            If Not (lblNoOutlet.Text = "0" Or lblNoOutlet.Text = "") Then
                storeNo = lblNoOutlet.Text
            End If
        Else

            For Each a In ListStore2
                If a.Status = True Then
                    If storeNo = "" Then
                        storeNo = cOut.GetKodeOutlet(a.StoreName)
                    Else
                        storeNo = storeNo & "," & cOut.GetKodeOutlet(a.StoreName)
                    End If
                End If
            Next
        End If


        'If Not (lblNoOutlet.Text = "0" Or lblNoOutlet.Text = "") Then
        '    storeNo = lblNoOutlet.Text
        'End If
        dataParam.storestr = storeNo
        'ambil data brand

        Dim brandNo As String = ""

        Dim ListBrand2 As List(Of Brand_Detail) = TryCast(ViewState("ListBrand"), List(Of Brand_Detail))

        If CBool(IIf(IsNothing(ViewState("ListBrand")), True, False)) Then
            If Not (lblnobrand.Text = "0" Or lblnobrand.Text = "") Then
                brandNo = lblnobrand.Text
            End If
        Else

            For Each a In ListBrand2
                If a.Status = True Then
                    If brandNo = "" Then
                        brandNo = cOut.GetIDBrandOutlet(a.BrandName)
                    Else
                        brandNo = brandNo & "," & cOut.GetIDBrandOutlet(a.BrandName)
                    End If
                End If
            Next
        End If

        dataParam.brandstr = brandNo

        'ambil data city
        Dim cityNo As String = ""

        Dim ListCity2 As List(Of City_Detail) = TryCast(ViewState("ListCity"), List(Of City_Detail))

        If CBool(IIf(IsNothing(ViewState("ListCity")), True, False)) Then
            If Not (lblnoCity.Text = "0" Or lblnoCity.Text = "") Then
                cityNo = lblnoCity.Text
            End If
        Else

            For Each a In ListCity2
                If a.Status = True Then
                    If cityNo = "" Then
                        cityNo = cOut.getIdCity(a.CityName)
                    Else
                        cityNo = cityNo & "," & cOut.getIdCity(a.CityName)
                    End If
                End If
            Next
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
        'Update 21 Februari 2020
        Threading.Thread.Sleep(5000)
        programmaticModalPopup.Hide()

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


            ds = clsreport.getDatasetgv(ddlRpt.Text, dataParam)



            ' setColumName()
            'perubahan 04 jul 2017
            If ds.Tables.Count > 0 Then
                dg1.DataSource = ds
                dg1.CurrentPageIndex = 0
                dg1.DataBind()
            Else
                dg1.DataSource = Nothing
                dg1.CurrentPageIndex = 0
                dg1.DataBind()
            End If

            ds.Dispose()

        Catch ex As Exception
            mfunction.errLog(Err.Description, "Report:showRpt")
            Err.Clear()
        End Try
    End Sub







    Protected Sub btnall_Click1(sender As Object, e As EventArgs)
        Try
            Dim shsl As String = "0"

            'Update Ihsan 19032019 

            For Each item As ListViewItem In LsvStk.Items
                Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox"), CheckBox)

                ck.Checked = False


            Next

            lblNoOutlet.Text = shsl
            lblOutletNameData.Text = "All Store"

            'End If

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
            BindingStore2()

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
            BindingStore2()

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

            If dtstore2.Rows.Count > 0 Then
                LsvStk.Items.Clear()
                countlvstore = dtstore2.Rows.Count
                LsvStk.DataSource = dtstore2.DefaultView
                LsvStk.DataBind()
                UpdatePanel3.Update()

            End If

        End If
    End Sub

    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Pop", "openModalStore();", True)
        BindCheckStore()
        BindingStore2()
        CheckedStore()

    End Sub

    Sub BindingStore()
        If icustid = 0 Then
            Response.Redirect("~/Default.aspx", False)
        Else

            'Bram4Sep2018 Update Report
            Dim dtstore2 As New DataTable


            dtstore2 = clsreport.getds_viewOutlet(ddlOwn.SelectedValue, lblnobrand.Text, lblnoCity.Text, icustid, txtSearch.Text, sNameuser, iModPriv).Tables(0)
            ViewState("vsoutlet") = dtstore2

            If dtstore2.Rows.Count > 0 Then
                LsvStk.Items.Clear()
                countlvstore = dtstore2.Rows.Count
                LsvStk.DataSource = dtstore2.DefaultView
                LsvStk.DataBind()
                UpdatePanel3.Update()
            End If



            'LsvStk.DataSource = dtstore2.DefaultView
            'LsvStk.DataBind()
        End If
    End Sub

    Private Sub btnSearch3_ServerClick(sender As Object, e As EventArgs) Handles btnSearch3.ServerClick
        BindingCity()
    End Sub

    Sub loadOutlet()

        Dim ds As New DataSet
        ds = clsOut.SelectDataset_Outlet(custid, username, modulepriv)

    End Sub

    Private Sub ddlOwn_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlOwn.SelectedIndexChanged
        BindingStore2()
    End Sub

    Private Sub txtSearch2_TextChanged(sender As Object, e As EventArgs) Handles txtSearch2.TextChanged
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Pop", "openModalBrand();", True)
        BindCheckBrand()
        BindingBrand()
        CheckedBrand()
    End Sub

    Private Sub txtSearch3_TextChanged(sender As Object, e As EventArgs) Handles txtSearch3.TextChanged
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Pop", "openModalCity();", True)
        BindCheckCity()
        BindingCity()
        CheckedCity()
    End Sub


    Sub dg1_Paging(sender As Object, e As DataGridPageChangedEventArgs)
        dg1.CurrentPageIndex = e.NewPageIndex
        Dim ds As New DataSet

        'set dataparam
        dataParam = New ClsParamRpt
        dataParam = set_dataparam(False)



        ds = clsreport.getDatasetgv(ddlRpt.Text, dataParam)

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
    End Sub
    'export ke excel
    Protected Sub btnExport2_Click(sender As Object, e As EventArgs) Handles btnExport2.Click
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

            'If ds.Tables.Count > 0 Then
            '    dt = ds.Tables(0)

            '    Dim csv As String = String.Empty

            '    For Each column As DataColumn In dt.Columns
            '        'Add the Header row for CSV file.
            '        csv += column.ColumnName + ","c
            '    Next

            '    'Add new line.
            '    csv += vbCr & vbLf

            '    For Each row As DataRow In dt.Rows
            '        For Each column As DataColumn In dt.Columns
            '            'Add the Data rows.
            '            csv += row(column.ColumnName).ToString().Replace(",", ";") + ","c
            '        Next

            '        'Add new line.
            '        csv += vbCr & vbLf
            '    Next

            '    'Download the CSV file.
            '    Response.Clear()
            '    Response.Buffer = True
            '    Response.AddHeader("content-disposition", "attachment;filename=" & sFile & ".csv")
            '    Response.Charset = ""
            '    Response.ContentType = "application/text"
            '    Response.Output.Write(csv)

            '    HttpContext.Current.Response.Flush()
            '    HttpContext.Current.Response.SuppressContent = True
            '    HttpContext.Current.ApplicationInstance.CompleteRequest()
            'End If

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
    'Update 9 januari 2020
    Protected Sub Checkbox_allstore_CheckedChanged(sender As Object, e As EventArgs)
        For Each item As ListViewItem In LsvStk.Items
            Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox"), CheckBox)
            ck.Checked = Checkbox_allstore.Checked
        Next

        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Pop", "openModalStore();", True)

    End Sub
    Protected Sub Checkbox_allbrand_CheckedChange(sender As Object, e As EventArgs)
        For Each item As ListViewItem In ListViewBrand.Items
            Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox2"), CheckBox)

            ck.Checked = Checkbox_allbrand.Checked

        Next

        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Pop", "openModalBrand();", True)
    End Sub
    Protected Sub Checkbox_allcity_CheckedChange(sender As Object, e As EventArgs)
        For Each item As ListViewItem In ListViewCity.Items
            Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox3"), CheckBox)

            ck.Checked = Checkbox_allcity.Checked

        Next
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Pop", "openModalCity();", True)
    End Sub
    Protected Sub btnAll1_ServerClick(sender As Object, e As EventArgs)
        Checkbox_allstore.Checked = True
        For Each item As ListViewItem In LsvStk.Items
            Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox"), CheckBox)
            ck.Checked = Checkbox_allstore.Checked
        Next

    End Sub

    Protected Sub btnAll2_ServerClick(sender As Object, e As EventArgs)
        Checkbox_allcity.Checked = True
        For Each item As ListViewItem In ListViewCity.Items
            Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox3"), CheckBox)
            ck.Checked = Checkbox_allcity.Checked
        Next
    End Sub

    Protected Sub btnAll3_ServerClick(sender As Object, e As EventArgs)
        Checkbox_allbrand.Checked = True
        For Each item As ListViewItem In ListViewBrand.Items
            Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox2"), CheckBox)

            ck.Checked = Checkbox_allbrand.Checked

        Next
    End Sub

    Private Sub ddlRpt_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlRpt.SelectedIndexChanged
        If ddlRpt.SelectedItem.Text = "Item Go Food" Or ddlRpt.SelectedItem.Text = "Monthly Sales" Then
            'ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), " document.getElementById('paramto').style.visible='hidden';", True)
            paramto.Visible = False
            lblparamto.Visible = False
            iconparamto.Visible = False
            lblparamfrom.Text = "Month"
            CalendarExtender2.Format = "MMM yyyy"
        Else
            paramto.Visible = True
            lblparamto.Visible = True
            iconparamto.Visible = True
            lblparamfrom.Text = "Date From"
        End If
        If ddlRpt.SelectedItem.Text = "Item Sales" Then
            paramitem.Visible = True
        End If
    End Sub

    Private Sub ddlRpt_TextChanged(sender As Object, e As EventArgs) Handles ddlRpt.TextChanged

    End Sub


    'End Update 9 januari 2020
End Class