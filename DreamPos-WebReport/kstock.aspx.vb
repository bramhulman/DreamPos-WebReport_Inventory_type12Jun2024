Imports System.IO
Imports System.Reflection
Imports System.Globalization
Imports ClosedXML.Excel

Public Class kstock
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
    Public countlvcc As Integer
    Public countlvstock As Integer
    Public allout As String
    Dim dsrpt As New DataSet
    Private clsdbo As clsDbo
    Private clsStk As New clsInventory_stk

    Public Function mpriv() As Integer
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
    Public Class Cc_Detail 'Class untuk ListCc
        Public Property CcName As String = ""
        Public Property Status As Boolean = False
        'Public Property CcID As Integer
    End Class
    <Serializable>
    Public Class Stock_Detail 'Class untuk ListStock
        Public Property StockName As String = ""
        Public Property Status As Boolean = False
        'Public Property CcID As Integer
    End Class

    'Start : BRAND
    'Private Sub BindCheckBrand()
    '    Try

    '        If CBool(IIf(IsNothing(ViewState("ListBrand")), True, False)) Then


    '            Dim ListBrand2 As List(Of Brand_Detail) = New List(Of Brand_Detail)()

    '            Dim dsBrand As New DataSet

    '            dsBrand = clsExport.SelectDataset_Brand(icustid, "", False)

    '            If dsBrand.Tables(0).Rows.Count > 0 Then
    '                For Each dt As DataRow In dsBrand.Tables(0).Rows
    '                    ListBrand2.Add(New Brand_Detail With {
    '                                       .BrandName = dt.Item("nama"),
    '                                       .Status = False
    '                                       })
    '                Next
    '            End If


    '            For Each item As ListViewItem In ListViewBrand.Items
    '                Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox2"), CheckBox)
    '                Dim Labels As Label = DirectCast(item.FindControl("lblBrand2"), Label)
    '                Dim label2 As String = Labels.Text



    '                If ck.Checked Then
    '                    For Each a In ListBrand2
    '                        If a.BrandName = label2 Then
    '                            a.Status = True
    '                        End If
    '                    Next
    '                    '  clsCheck.IDBrand.Add(label2)
    '                End If
    '            Next
    '            ViewState("ListBrand") = ListBrand2

    '        Else

    '            Dim ListBrand2 As List(Of Brand_Detail) = TryCast(ViewState("ListBrand"), List(Of Brand_Detail))

    '            '     Dim dsBrand As New DataSet




    '            For Each item As ListViewItem In ListViewBrand.Items
    '                Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox2"), CheckBox)
    '                Dim Labels As Label = DirectCast(item.FindControl("lblBrand2"), Label)
    '                Dim label2 As String = Labels.Text

    '                If ck.Checked Then
    '                    For Each a In ListBrand2
    '                        If a.BrandName = label2 Then
    '                            a.Status = True
    '                        End If
    '                    Next

    '                Else
    '                    For Each a In ListBrand2
    '                        If a.BrandName = label2 Then
    '                            a.Status = False
    '                        End If
    '                    Next
    '                End If
    '            Next
    '            ViewState("ListBrand") = ListBrand2
    '        End If
    '    Catch ex As Exception
    '        errLog(Err.Description, "kstock:BindCheckBrand")
    '        Err.Clear()
    '    End Try
    'End Sub
    'Private Sub CheckedBrand()
    '    Try


    '        Dim ListBrand As List(Of Brand_Detail) = TryCast(ViewState("ListBrand"), List(Of Brand_Detail))

    '        Dim i As Integer = 0
    '        For Each item As ListViewItem In ListViewBrand.Items
    '            Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox2"), CheckBox)
    '            Dim Labels As Label = DirectCast(item.FindControl("lblBrand2"), Label)
    '            Dim label2 As String = Labels.Text

    '            For Each a In ListBrand
    '                If a.BrandName = label2 Then
    '                    If a.Status = True Then
    '                        ck.Checked = True
    '                    Else
    '                        ck.Checked = False
    '                    End If

    '                End If
    '            Next

    '        Next
    '        ViewState("ListBrand") = ListBrand
    '    Catch ex As Exception
    '        errLog(Err.Description, "kstock:CheckBrand")
    '        Err.Clear()
    '    End Try
    'End Sub
    ''END : BRAND

    ''Start : CITY
    'Private Sub BindCheckCity()
    '    Try

    '        If CBool(IIf(IsNothing(ViewState("ListCity")), True, False)) Then


    '            Dim ListCity2 As List(Of City_Detail) = New List(Of City_Detail)()

    '            Dim dsCity As New DataSet

    '            dsCity = clsExport.SelectDataset_City(icustid, "")

    '            If dsCity.Tables(0).Rows.Count > 0 Then
    '                For Each dt As DataRow In dsCity.Tables(0).Rows
    '                    ListCity2.Add(New City_Detail With {
    '                                       .CityName = dt.Item("kota"),
    '                                       .Status = False
    '                                       })
    '                Next
    '            End If


    '            For Each item As ListViewItem In ListViewCity.Items
    '                Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox3"), CheckBox)
    '                Dim Labels As Label = DirectCast(item.FindControl("lblcity2"), Label)
    '                Dim label2 As String = Labels.Text



    '                If ck.Checked Then
    '                    For Each a In ListCity2
    '                        If a.CityName = label2 Then
    '                            a.Status = True
    '                        End If
    '                    Next
    '                    '  clsCheck.IDBrand.Add(label2)
    '                End If
    '            Next
    '            ViewState("ListCity") = ListCity2

    '        Else

    '            Dim ListCity2 As List(Of City_Detail) = TryCast(ViewState("ListCity"), List(Of City_Detail))

    '            For Each item As ListViewItem In ListViewCity.Items
    '                Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox3"), CheckBox)
    '                Dim Labels As Label = DirectCast(item.FindControl("lblCity2"), Label)
    '                Dim label2 As String = Labels.Text

    '                If ck.Checked Then
    '                    For Each a In ListCity2
    '                        If a.CityName = label2 Then
    '                            a.Status = True
    '                        End If
    '                    Next

    '                Else
    '                    For Each a In ListCity2
    '                        If a.CityName = label2 Then
    '                            a.Status = False
    '                        End If
    '                    Next
    '                End If
    '            Next
    '            ViewState("ListCity") = ListCity2
    '        End If
    '    Catch ex As Exception
    '        errLog(Err.Description, "kstock:BindCheckCity")
    '        Err.Clear()
    '    End Try
    'End Sub
    'Private Sub CheckedCity()
    '    Try


    '        Dim ListCity As List(Of City_Detail) = TryCast(ViewState("ListCity"), List(Of City_Detail))

    '        Dim i As Integer = 0
    '        For Each item As ListViewItem In ListViewCity.Items
    '            Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox3"), CheckBox)
    '            Dim Labels As Label = DirectCast(item.FindControl("lblCity2"), Label)
    '            Dim label2 As String = Labels.Text

    '            For Each a In ListCity
    '                If a.CityName = label2 Then
    '                    If a.Status = True Then
    '                        ck.Checked = True
    '                    Else
    '                        ck.Checked = False
    '                    End If

    '                End If
    '            Next

    '        Next
    '        ViewState("ListCity") = ListCity
    '    Catch ex As Exception
    '        errLog(Err.Description, "kstock:CheckCity")
    '        Err.Clear()
    '    End Try
    'End Sub
    'END : CITY

    'Start : Cc
    Private Sub BindCheckCc()
        Try

            If CBool(IIf(IsNothing(ViewState("ListCc")), True, False)) Then


                Dim ListCc As List(Of Cc_Detail) = New List(Of Cc_Detail)()

                Dim dsCc As New DataSet

                dsCc = SelectDataset_Cc(icustid, "")

                If dsCc.Tables(0).Rows.Count > 0 Then
                    For Each dt As DataRow In dsCc.Tables(0).Rows
                        ListCc.Add(New Cc_Detail With {
                                           .CcName = dt.Item("Name"),
                                           .Status = False
                                           })
                    Next
                    '.CcID = dt.Item("ID"),
                End If


                For Each item As ListViewItem In LsvStk.Items
                    Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox"), CheckBox)
                    Dim Labels As Label = DirectCast(item.FindControl("lblName"), Label)
                    Dim label2 As String = Labels.Text



                    If ck.Checked Then
                        For Each a In ListCc
                            If a.CcName = label2 Then
                                a.Status = True
                            End If
                        Next
                        '  clsCheck.IDBrand.Add(label2)
                    End If
                Next
                ViewState("ListCc") = ListCc

            Else

                Dim ListCc As List(Of Cc_Detail) = TryCast(ViewState("ListCc"), List(Of Cc_Detail))

                For Each item As ListViewItem In LsvStk.Items
                    Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox"), CheckBox)
                    Dim Labels As Label = DirectCast(item.FindControl("lblName"), Label)
                    Dim label2 As String = Labels.Text

                    If ck.Checked Then
                        For Each a In ListCc
                            If a.CcName = label2 Then
                                a.Status = True
                            End If
                        Next

                    Else
                        For Each a In ListCc
                            If a.CcName = label2 Then
                                a.Status = False
                            End If
                        Next
                    End If
                Next
                ViewState("ListCc") = ListCc
            End If
        Catch ex As Exception
            errLog(Err.Description, "kstock:BindCheckCc")
            Err.Clear()
        End Try
    End Sub

    Private Sub CheckedCc()
        Try


            Dim ListCc As List(Of Cc_Detail) = TryCast(ViewState("ListCc"), List(Of Cc_Detail))

            Dim i As Integer = 0
            For Each item As ListViewItem In LsvStk.Items
                Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox"), CheckBox)
                Dim Labels As Label = DirectCast(item.FindControl("lblName"), Label)
                Dim label2 As String = Labels.Text

                For Each a In ListCc
                    If a.CcName = label2 Then
                        If a.Status = True Then
                            ck.Checked = True
                        Else
                            ck.Checked = False
                        End If

                    End If
                Next

            Next
            ViewState("ListCc") = ListCc
        Catch ex As Exception
            errLog(Err.Description, "kstock:CheckedCc")
            Err.Clear()
        End Try
    End Sub
    'END : Cc

    'Start : Stock
    Private Sub BindCheckStock()
        Try

            If CBool(IIf(IsNothing(ViewState("ListStock")), True, False)) Then


                Dim ListStock As List(Of Stock_Detail) = New List(Of Stock_Detail)()

                Dim dsStock As New DataSet

                dsStock = SelectDataset_Stock(icustid, "")

                If dsStock.Tables(0).Rows.Count > 0 Then
                    For Each dt As DataRow In dsStock.Tables(0).Rows
                        ListStock.Add(New Stock_Detail With {
                                           .StockName = dt.Item("Name"),
                                           .Status = False
                                           })
                    Next
                    '.CcID = dt.Item("ID"),
                End If


                For Each item As ListViewItem In LsvStock.Items
                    Dim ck As CheckBox = DirectCast(item.FindControl("CheckBoxStock"), CheckBox)
                    Dim Labels As Label = DirectCast(item.FindControl("lblNamestock"), Label)
                    Dim label2 As String = Labels.Text



                    If ck.Checked Then
                        For Each a In ListStock
                            If a.StockName = label2 Then
                                a.Status = True
                            End If
                        Next
                        '  clsCheck.IDBrand.Add(label2)
                    End If
                Next
                ViewState("ListStock") = ListStock

            Else

                Dim ListStock As List(Of Stock_Detail) = TryCast(ViewState("ListStock"), List(Of Stock_Detail))

                For Each item As ListViewItem In LsvStock.Items
                    Dim ck As CheckBox = DirectCast(item.FindControl("CheckBoxStock"), CheckBox)
                    Dim Labels As Label = DirectCast(item.FindControl("lblNamestock"), Label)
                    Dim label2 As String = Labels.Text

                    If ck.Checked Then
                        For Each a In ListStock
                            If a.StockName = label2 Then
                                a.Status = True
                            End If
                        Next

                    Else
                        For Each a In ListStock
                            If a.StockName = label2 Then
                                a.Status = False
                            End If
                        Next
                    End If
                Next
                ViewState("ListStock") = ListStock
            End If
        Catch ex As Exception
            errLog(Err.Description, "kstock:BindCheckStock")
            Err.Clear()
        End Try
    End Sub

    Private Sub CheckedStock()
        Try


            Dim ListStock As List(Of Stock_Detail) = TryCast(ViewState("ListStock"), List(Of Stock_Detail))

            Dim i As Integer = 0
            For Each item As ListViewItem In LsvStock.Items
                Dim ck As CheckBox = DirectCast(item.FindControl("CheckBoxStock"), CheckBox)
                Dim Labels As Label = DirectCast(item.FindControl("lblNamestock"), Label)
                Dim label2 As String = Labels.Text

                For Each a In ListStock
                    If a.StockName = label2 Then
                        If a.Status = True Then
                            ck.Checked = True
                        Else
                            ck.Checked = False
                        End If

                    End If
                Next

            Next
            ViewState("ListStock") = ListStock
        Catch ex As Exception
            errLog(Err.Description, "kstock:CheckStock")
            Err.Clear()
        End Try
    End Sub
    'END : Stock


#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        mHelper.CekSession()
        Try
            CalendarExtender1.EndDate = DateTime.Now
            CalendarExtender2.EndDate = DateTime.Now
            If Request.Cookies("UserLogin_inv") IsNot Nothing Then
                sNameuser = username()
                icustid = custid()
                imodul = modul()
                iModPriv = mpriv()
                irptpriv = rptpriv()
                irptf = rptf()
                If IsPostBack = False Then

                    'dg2.Visible = False
                    Dim script As String = "$(document).ready(function () { $('[id*=btnShow]').click(); });"

                    ClientScript.RegisterStartupScript(Me.GetType, "load", script, True)


                    txtFrom.Text = Format(Now, "dd MMM yyyy")
                    txtTo.Text = Format(Now, "dd MMM yyyy")
                    btnShow.Text = "OK"



                    dataParam = New ClsParamRpt
                    dataParam.custID = icustid


                    'loadKepemilikan()
                    'BindingBrand()
                    'BindingCity()
                    BindingCc()
                    BindingStock()

                    'SetClearColoum()

                    'Update 13 Februari 2020

                    'This for Search Button
                    'txtSearch.Attributes.Add("onkeyup", Me.Page.ClientScript.GetPostBackEventReference(txtSearch, ""))
                    'txtSearch2.Attributes.Add("onkeyup", Me.Page.ClientScript.GetPostBackEventReference(txtSearch2, ""))
                    'txtSearch3.Attributes.Add("onkeyup", Me.Page.ClientScript.GetPostBackEventReference(txtSearch3, ""))
                    txtSearchStock.Attributes.Add("onkeyup", Me.Page.ClientScript.GetPostBackEventReference(txtSearchStock, ""))

                    'This for Check All = True/Checked
                    'Checkbox_allbrand.Attributes.Add("onclick", Me.Page.ClientScript.GetPostBackEventReference(Checkbox_allbrand, ""))
                    Checkbox_allstore.Attributes.Add("onclick", Me.Page.ClientScript.GetPostBackEventReference(Checkbox_allstore, ""))
                    'Checkbox_allcity.Attributes.Add("onclick", Me.Page.ClientScript.GetPostBackEventReference(Checkbox_allcity, ""))
                    CheckboxAllStock.Attributes.Add("onclick", Me.Page.ClientScript.GetPostBackEventReference(CheckboxAllStock, ""))

                    'ScriptManager.GetCurrent(Me).RegisterPostBackControl(ddlOwn)
                    ScriptManager.GetCurrent(Me).RegisterPostBackControl(RadButton1)
                    'ScriptManager.GetCurrent(Me).RegisterPostBackControl(RadButton2)
                    'ScriptManager.GetCurrent(Me).RegisterPostBackControl(RadButton3)
                    ScriptManager.GetCurrent(Me).RegisterPostBackControl(RadButtonStock)

                    ScriptManager.GetCurrent(Me).RegisterPostBackControl(ddlRpt)
                    'ScriptManager.GetCurrent(Me).RegisterAsyncPostBackControl(ddlRpt)


                    txtTo.Attributes.Add("readonly", "true")
                    txtFrom.Attributes.Add("readonly", "true")
                    txtSearch.Attributes.Add("onfocus", "javascript: this.setSelectionRange(this.value.length,this.value.length);")
                    'txtSearch2.Attributes.Add("onfocus", "javascript:this.setSelectionRange(this.value.length,this.value.length);")
                    'txtSearch3.Attributes.Add("onfocus", "javascript:this.setSelectionRange(this.value.length,this.value.length);")
                    txtSearchStock.Attributes.Add("onfocus", "javascript:this.setSelectionRange(this.value.length,this.value.length);")

                    'End Update 9 Januari 2020


                End If
            Else


                mfunction.errLog(Err.Description, "reportinventory:Please Login")
                errLog(Err.Description, "reportinventory:Please Login")
                Dim script As String = "window.onload = Function(){ alert('"
                script += "Please Login"
                script += "');"
                script += "window.location = '"
                script += "../Default"
                script += " '; }"
                ClientScript.RegisterStartupScript(Me.GetType(), "Redirect", script, True)
                'End If



            End If


        Catch ex As Exception
            mfunction.errLog(Err.Description, "reportinventory:Page_Load")
            Err.Clear()
        End Try
    End Sub

    Public Enum Modulepriv
        moduleuser = &H1
    End Enum
    Sub BindingCc()
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        If cookie("Custid") = 0 Or cookie("Custid") Is Nothing Then
            errLog("icustid = 0", "cookienothing" & "_reportinventory")
            Response.Redirect("~/Default.aspx", False)
        Else
            Dim dsCc As New DataSet
            If mpriv() And usrModulepriv.moduleuser = usrModulepriv.moduleuser Then
                dsCc = SelectDs_Cc_byuser(icustid, txtSearch.Text, username)
            Else
                dsCc = SelectDataset_Cc(icustid, txtSearch.Text)
            End If
            'Bram4Sep2018 Update Report
            ' dsCc = clsExport.SelectDataset_Cc(icustid, txtSearch.Text)


            If dsCc.Tables(0).Rows.Count > 0 Then
                LsvStk.Items.Clear()
                countlvcc = dsCc.Tables(0).Rows.Count
                If dsCc IsNot Nothing AndAlso dsCc.Tables.Count > 0 Then
                    LsvStk.DataSource = dsCc
                Else
                    LsvStk.DataSource = Nothing
                End If
                LsvStk.DataBind()
                'UpdatePanel3.Update()

            End If

        End If
    End Sub
    Sub BindingStock()
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")

        If cookie("Custid") = 0 Or cookie("Custid") Is Nothing Then
            errLog("icustid = 0", "cookienothing" & "_reportinventory")
            Response.Redirect("~/Default.aspx", False)
        Else
            'Dim dsStock As New DataSet
            Dim dt, dt2 As New DataTable
            'dt = DirectCast(ViewState("getstock"), DataTable)
            If CBool(IIf(IsNothing(ViewState("getstock")), True, False)) Then

                dt2 = SelectDataset_Stock(icustid, txtSearchStock.Text).Tables(0)
            Else
                dt = DirectCast(ViewState("getstock"), DataTable)
                dt2 = clsStk.SelectDataTable_Stk(dt, custid, txtSearch.Text, mpriv)
            End If
            'dt2 = SelectDataset_Stock(icustid, txtSearchStock.Text)
            'dt2 = clsStk.SelectDataTable_Stk(dt, custid, txtSearch.Text, mpriv)
            ViewState("vsstock") = dt2 'dsStock.Tables(0)
            'Bram4Sep2018 Update Report
            ' dsCc = clsExport.SelectDataset_Cc(icustid, txtSearch.Text)


            If dt2.Rows.Count > 0 Then
                LsvStock.Items.Clear()
                countlvstock = dt2.Rows.Count
                LsvStock.DataSource = dt2
                LsvStock.DataBind()
                'UpdatePanelStock.Update()

            End If

        End If
    End Sub
    Sub BindingStock2()
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")

        If cookie("Custid") = 0 Or cookie("Custid") Is Nothing Then
            errLog("icustid = 0", "cookienothing" & "_reportinventory")
            Response.Redirect("~/Default.aspx", False)
        Else
            Dim dt, dt2 As New DataTable
            'Bram4Sep2018 Update Report
            'Dim dtstock2 As DataTable
            dt = DirectCast(ViewState("getstock"), DataTable)
            dt2 = DirectCast(ViewState("vsstock"), DataTable)

            If dt2.Rows.Count = 0 Then
                dt2 = clsStk.SelectDataTable_Stk(dt, custid, txtSearch.Text, mpriv)
                ViewState("vsstock") = dt2
            Else



                dt2.DefaultView.RowFilter = "custid = " & icustid & "and (number like '%" & txtSearchStock.Text & "%' Or name like '%" & txtSearchStock.Text & "%')"
                ViewState("vsstock") = dt2
            End If

            If dt2.Rows.Count > 0 Then
                LsvStock.Items.Clear()
                countlvstock = dt2.Rows.Count
                LsvStock.DataSource = dt2.DefaultView
                LsvStock.DataBind()
                UpdatePanelStock.Update()

            End If

        End If
    End Sub
    'Private Sub loadBrand2()
    '    Dim dsBrand As New DataSet
    '    Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
    '    If cookie("Custid") = 0 Or cookie("Custid") Is Nothing Then
    '        errLog("icustid = 0", "cookienothing" & "_reportinventory")
    '        Response.Redirect("~/Default.aspx", False)
    '    Else

    '        dsBrand = clsreport.getds_ViewBrand(icustid, txtSearch2.Text)

    '        ListViewBrand.DataSource = dsBrand
    '        ListViewBrand.DataBind()
    '    End If
    'End Sub

    'Private Sub loadCity2()
    '    Dim dsCity As New DataSet
    '    Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
    '    If cookie("Custid") = 0 Or cookie("Custid") Is Nothing Then
    '        errLog("icustid = 0", "cookienothing" & "_reportinventory")
    '        Response.Redirect("~/Default.aspx", False)
    '    Else

    '        dsCity = clsreport.getds_ViewCity(icustid, txtSearch3.Text)

    '        ListViewCity.DataSource = dsCity
    '        ListViewCity.DataBind()

    '    End If
    'End Sub
    Protected Sub btnExport_Click(sender As Object, e As EventArgs)
        If CDate(txtTo.Text) < CDate(txtFrom.Text) Then
            mfunction.ClnMsg("date range not valid" & " Please check Again", Me.Page)
            Exit Sub
        ElseIf CDate(txtTo.Text) > CDate(Now) Or CDate(txtFrom.Text) > CDate(Now) Then
            mfunction.ClnMsg("date range not valid" & " Please check Again", Me.Page)
            Exit Sub
            'ElseIf ddlRpt.SelectedItem.Text.Contains("2018") And (CDate(txtTo.Text).Year > 2018 Or CDate(txtFrom.Text).Year > 2018) Then
            '    mfunction.ClnMsg("you must select date before 2019" & " Please check Again", Me.Page)
            '    Exit Sub
        End If
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
                        'Add the Data rows - format numeric columns using US English (InvariantCulture)
                        Dim cellValue As String = ""
                        Dim rawVal = row(column.ColumnName)
                        Dim typeCode As TypeCode = Type.GetTypeCode(column.DataType)
                        If typeCode = TypeCode.Decimal OrElse typeCode = TypeCode.Double OrElse
                           typeCode = TypeCode.Single OrElse typeCode = TypeCode.Int32 OrElse
                           typeCode = TypeCode.Int64 OrElse typeCode = TypeCode.Object Then
                            ' Try parse as numeric for safe format
                            Dim dblVal As Double
                            If rawVal IsNot DBNull.Value AndAlso rawVal IsNot Nothing AndAlso
                               Double.TryParse(rawVal.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, dblVal) Then
                                cellValue = dblVal.ToString(CultureInfo.InvariantCulture)
                            Else
                                cellValue = If(rawVal Is Nothing OrElse rawVal Is DBNull.Value, "", rawVal.ToString().Replace(",", ";"))
                            End If
                        Else
                            cellValue = If(rawVal Is Nothing OrElse rawVal Is DBNull.Value, "", rawVal.ToString().Replace(",", ";"))
                        End If
                        csv += cellValue + ","c
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
            'If ddlRpt.Text = "Receipt Detail" Then
            If lblNoOutlet.Text = "" Or lblNoOutlet.Text = 0 Then
                allout = "All"
            Else
                allout = ""
            End If
            insertLogModule(username, ddlRpt.Text & ", " & txtFrom.Text & " / " & txtTo.Text & " " & allout, "Export", custid)
            'End If
        Catch ex As Exception
            mfunction.errLog(Err.Description, "Report:ExportData")
            Err.Clear()
        Finally
            'loadgif.Visible = False
            HttpContext.Current.Response.SuppressContent = True
            HttpContext.Current.ApplicationInstance.CompleteRequest()

        End Try

        Exit Sub
    End Sub
    'export To Excel
    Protected Sub btnExport2_Click(sender As Object, e As EventArgs)
        If CDate(txtTo.Text) < CDate(txtFrom.Text) Then
            mfunction.ClnMsg("date range not valid" & " Please check Again", Me.Page)
            Exit Sub
        ElseIf CDate(txtTo.Text) > CDate(Now) Or CDate(txtFrom.Text) > CDate(Now) Then
            mfunction.ClnMsg("date range not valid" & " Please check Again", Me.Page)
            Exit Sub
        ElseIf ddlRpt.SelectedItem.Text.Contains("2018") And (CDate(txtTo.Text).Year > 2018 Or CDate(txtFrom.Text).Year > 2018) Then
            mfunction.ClnMsg("you must select date before 2019" & " Please check Again", Me.Page)
            Exit Sub
        End If
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

                If ddlRpt.Text = "Receipt Detail" Then

                    'ubah kecsv

                    Dim csv As String = String.Empty

                    For Each column As DataColumn In dt.Columns
                        'Add the Header row for CSV file.
                        csv += column.ColumnName + ","c
                    Next

                    'Add new line.
                    csv += vbCr & vbLf

                    For Each row As DataRow In dt.Rows
                        For Each column As DataColumn In dt.Columns
                            'Add the Data rows - format numeric columns using US English (InvariantCulture)
                            Dim cellValue2 As String = ""
                            Dim rawVal2 = row(column.ColumnName)
                            Dim tc2 As TypeCode = Type.GetTypeCode(column.DataType)
                            If tc2 = TypeCode.Decimal OrElse tc2 = TypeCode.Double OrElse
                               tc2 = TypeCode.Single OrElse tc2 = TypeCode.Int32 OrElse
                               tc2 = TypeCode.Int64 OrElse tc2 = TypeCode.Object Then
                                Dim dblVal2 As Double
                                If rawVal2 IsNot DBNull.Value AndAlso rawVal2 IsNot Nothing AndAlso
                                   Double.TryParse(rawVal2.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, dblVal2) Then
                                    cellValue2 = dblVal2.ToString(CultureInfo.InvariantCulture)
                                Else
                                    cellValue2 = If(rawVal2 Is Nothing OrElse rawVal2 Is DBNull.Value, "", rawVal2.ToString().Replace(",", ";"))
                                End If
                            Else
                                cellValue2 = If(rawVal2 Is Nothing OrElse rawVal2 Is DBNull.Value, "", rawVal2.ToString().Replace(",", ";"))
                            End If
                            csv += cellValue2 + ","c
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

                    'akhir ubah ke csv
                    If lblNoOutlet.Text = "" Or lblNoOutlet.Text = 0 Then
                        allout = "All"
                    Else
                        allout = ""
                    End If
                    insertLogModule(username, ddlRpt.Text & ", " & txtFrom.Text & " / " & txtTo.Text & " " & allout, "Export", custid)
                    'End If

                Else
                    ' Convert numeric columns (in/out) to Double so Excel treats them as numbers
                    Dim dtExport As DataTable = dt.Clone()
                    For Each col As DataColumn In dtExport.Columns
                        Dim tc As TypeCode = Type.GetTypeCode(col.DataType)
                        If tc = TypeCode.Object OrElse tc = TypeCode.String Then
                            ' Check if this column contains numeric data by column name or sample value
                            If col.ColumnName.ToLower() = "in" OrElse col.ColumnName.ToLower() = "out" OrElse
                               col.ColumnName.ToLower() = "i" OrElse col.ColumnName.ToLower() = "o" Then
                                col.DataType = GetType(Double)
                            End If
                        ElseIf tc = TypeCode.Decimal Then
                            col.DataType = GetType(Double)
                        End If
                    Next
                    ' Copy rows with proper numeric conversion
                    For Each srcRow As DataRow In dt.Rows
                        Dim newRow As DataRow = dtExport.NewRow()
                        For Each col As DataColumn In dtExport.Columns
                            Dim srcVal = srcRow(col.ColumnName)
                            If col.DataType Is GetType(Double) Then
                                Dim dblVal As Double
                                If srcVal IsNot DBNull.Value AndAlso srcVal IsNot Nothing AndAlso
                                   Double.TryParse(srcVal.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, dblVal) Then
                                    newRow(col.ColumnName) = dblVal
                                Else
                                    newRow(col.ColumnName) = DBNull.Value
                                End If
                            Else
                                newRow(col.ColumnName) = srcVal
                            End If
                        Next
                        dtExport.Rows.Add(newRow)
                    Next

                    Using wb As New XLWorkbook()
                        wb.Worksheets.Add(dtExport, "Sheet1")
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
    'Brand
    'Protected Sub btnSearch2_ServerClick(sender As Object, e As EventArgs)
    '    If txtSearch2.Text = "" Then
    '        ShowToastr(Page, "Text Search tidak boleh kosong", "Error", "Error", False, "toast-top-right", False)
    '    Else
    '        BindCheckBrand()
    '        BindingBrand()
    '        CheckedBrand()

    '    End If
    '    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Pop", "openModalBrand();", True)
    'End Sub
    ''City
    'Protected Sub btnSearch3_ServerClick(sender As Object, e As EventArgs)
    '    If txtSearch3.Text = "" Then
    '        ShowToastr(Page, "Text Search tidak boleh kosong", "Error", "Error", False, "toast-top-right", False)
    '    Else
    '        BindCheckCity()
    '        BindingCity()
    '        CheckedCity()

    '    End If
    '    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Pop", "openModalCity();", True)
    'End Sub
    'Cc
    Protected Sub btnSearch_ServerClick(sender As Object, e As EventArgs)
        If txtSearch.Text = "" Then
            ShowToastr(Page, "Text Search tidak boleh kosong", "Error", "Error", False, "toast-top-right", False)
        Else
            BindCheckCc()
            'BindingStore2()
            CheckedCc()
        End If
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Pop", "openModalCc();", True)
    End Sub

    Protected Sub BtnSearchStock_ServerClick(sender As Object, e As EventArgs)
        If txtSearchStock.Text = "" Then
            ShowToastr(Page, "Text Search tidak boleh kosong", "Error", "Error", False, "toast-top-right", False)
        Else
            BindCheckStock()
            BindingStock2()
            CheckedStock()
        End If
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Pop", "openModalStock();", True)
    End Sub

    Protected Sub btnClear_ServerClick(sender As Object, e As EventArgs)
        Try
            For Each item As ListViewItem In LsvStk.Items
                Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox"), CheckBox)

                ck.Checked = False


            Next
            txtSearch.Text = ""
            lblNoOutlet.Text = 0
            BindingCc()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Pop", "openModalCc();", True)
        Catch ex As Exception
            mfunction.errLog(Err.Description, "kstock:btnClear_ServerClick")
            Err.Clear()
        End Try
    End Sub
    Protected Sub btnClearStock_ServerClick(sender As Object, e As EventArgs) Handles btnClearStock.ServerClick
        Try
            For Each item As ListViewItem In LsvStock.Items
                Dim ck As CheckBox = DirectCast(item.FindControl("CheckBoxStock"), CheckBox)

                ck.Checked = False


            Next
            txtSearchStock.Text = ""
            lblnostock.Text = 0
            BindingStock2()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Pop", "openModalStock();", True)
        Catch ex As Exception
            mfunction.errLog(Err.Description, "kstock:btnClearStock_ServerClick")
            Err.Clear()
        End Try
    End Sub

    'Protected Sub ddlOwn_SelectedIndexChanged1(sender As Object, e As EventArgs)
    '    BindingStore2()
    'End Sub

    'Protected Sub dg1_PageIndexChanged(source As Object, e As DataGridPageChangedEventArgs)
    '    dg1.CurrentPageIndex = e.NewPageIndex
    '    Dim ds As New DataSet

    '    'set dataparam
    '    dataParam = New ClsParamRpt
    '    dataParam = set_dataparam(False)



    '    ds = clsreport.getDatasetgv(ddlRpt.Text, dataParam)

    '    ' setColumName()
    '    'perubahan 04 jul 2017
    '    If ds.Tables.Count > 0 Then
    '        dg1.DataSource = ds
    '        dg1.DataBind()
    '    Else
    '        dg1.DataSource = Nothing
    '        dg1.DataBind()
    '    End If

    '    ds.Dispose()
    'End Sub
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
        'If ddlOwn.Visible = True Then
        '    Dim Own As Integer = 0

        '    If Not (ddlOwn.SelectedValue = "0" Or ddlOwn.SelectedValue = "") Then
        '        Own = ddlOwn.SelectedValue
        '    End If

        '    'myRptDoc.SetParameterValue("ownID", Own)
        '    dataParam.ownID = Own
        'Else
        dataParam.ownID = 0
        'End If
        'ambil data outlet
        Dim storeNo As String = ""
        Dim CcId As String = ""

        Dim ListStore2 As List(Of Cc_Detail) = TryCast(ViewState("ListCc"), List(Of Cc_Detail))

        If CBool(IIf(IsNothing(ViewState("ListCc")), True, False)) Then
            If Not (lblNoOutlet.Text = "0" Or lblNoOutlet.Text = "") Then
                CcId = lblNoOutlet.Text
            End If
        Else

            For Each a In ListStore2
                If a.Status = True Then
                    If CcId = "" Then
                        CcId = GetKodeCc(a.CcName)
                    Else
                        CcId = CcId & "," & GetKodeCc(a.CcName)
                    End If
                End If
            Next
        End If


        'If Not (lblNoOutlet.Text = "0" Or lblNoOutlet.Text = "") Then
        '    storeNo = lblNoOutlet.Text
        'End If
        dataParam.ccID = CcId
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

        'ambil data stock
        Dim stockNo As String = ""

        Dim ListStock As List(Of Stock_Detail) = TryCast(ViewState("ListStock"), List(Of Stock_Detail))

        If CBool(IIf(IsNothing(ViewState("ListStock")), True, False)) Then
            If Not (lblnostock.Text = "0" Or lblnostock.Text = "") Then
                stockNo = lblnostock.Text
            End If
        Else

            For Each a In ListStock
                If a.Status = True Then
                    If stockNo = "" Then
                        stockNo = cOut.getIdStock(a.StockName)
                    Else
                        stockNo = stockNo & "," & cOut.getIdStock(a.StockName)
                    End If
                End If
            Next
        End If


        dataParam.stockstr = stockNo

        'ambil data item
        Dim itemNo As String = ""

        If Not (lblnoitem.Text = "0" Or lblnoitem.Text = "") Then
            itemNo = lblnoitem.Text
        End If
        dataParam.itemstr = itemNo

        Return dataParam



    End Function
    ''Brand
    'Protected Sub RadButton3_ServerClick(sender As Object, e As EventArgs)
    '    Add2()
    '    BindCheckBrand()
    '    BindingStore2()
    'End Sub
    ''City
    'Protected Sub RadButton2_ServerClick(sender As Object, e As EventArgs)
    '    Add3()
    '    BindCheckCity()
    '    BindingStore2()
    'End Sub
    'Cc
    Protected Sub RadButton1_ServerClick(sender As Object, e As EventArgs)
        Try
            Add()
            BindCheckCc()
        Catch ex As Exception
            'MsgBox(ex.Message)
        End Try
    End Sub
    Protected Sub RadButtonStock_ServerClick(sender As Object, e As EventArgs)
        Try
            AddStock()
            BindCheckStock()
        Catch ex As Exception
            'MsgBox(ex.Message)
        End Try
    End Sub
    'Protected Sub Checkbox_allbrand_CheckedChanged1(sender As Object, e As EventArgs)
    '    For Each item As ListViewItem In ListViewBrand.Items
    '        Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox2"), CheckBox)

    '        ck.Checked = Checkbox_allbrand.Checked

    '    Next

    '    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Pop", "openModalBrand();", True)
    'End Sub

    'Protected Sub Checkbox_allcity_CheckedChanged1(sender As Object, e As EventArgs)
    '    For Each item As ListViewItem In ListViewCity.Items
    '        Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox3"), CheckBox)

    '        ck.Checked = Checkbox_allcity.Checked

    '    Next
    '    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Pop", "openModalCity();", True)
    'End Sub
    'Cc
    Protected Sub Checkbox_allstore_CheckedChanged1(sender As Object, e As EventArgs)
        For Each item As ListViewItem In LsvStk.Items
            Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox"), CheckBox)
            ck.Checked = Checkbox_allstore.Checked
        Next

        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Pop", "openModalCc();", True)
    End Sub

    Protected Sub CheckboxAllStock_CheckedChanged(sender As Object, e As EventArgs)
        For Each item As ListViewItem In LsvStock.Items
            Dim ck As CheckBox = DirectCast(item.FindControl("CheckBoxStock"), CheckBox)
            ck.Checked = CheckboxAllStock.Checked
        Next

        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Pop", "openModalStock();", True)
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
                lblCcNameData.Text = "All Cc"
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
                    lblCcNameData.Text = "All Cc"
                Else
                    lblNoOutlet.Text = shsl
                    lblCcNameData.Text = "Selected Cc"
                End If
            End If




        Catch ex As Exception
            'MsgBox(ex.Message)
        End Try

    End Sub

    'Sub Add2(Optional ByVal pilih As Boolean = True)

    '    Try
    '        Dim shsl As String = ""
    '        Dim i As Integer = 0
    '        If pilih = False Then
    '            For Each item As ListViewItem In ListViewBrand.Items
    '                Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox2"), CheckBox)
    '                'Update Ihsan 19032019

    '                If ck.Checked Then
    '                    ck.Checked = False
    '                End If

    '            Next

    '            lblnobrand.Text = ""
    '            'lblNmBrand.Text = "All Brand"
    '        Else
    '            For Each item As ListViewItem In ListViewBrand.Items
    '                Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox2"), CheckBox)
    '                'Update Ihsan 19032019

    '                If ck.Checked Then
    '                    If shsl = "" Then
    '                        shsl = (DirectCast(item.FindControl("LblIdBrand"), Label).Text)
    '                    Else
    '                        shsl = shsl & "," & (DirectCast(item.FindControl("LblIdBrand"), Label).Text)
    '                    End If
    '                End If

    '            Next
    '            If shsl = "" Then
    '                lblnobrand.Text = ""
    '                'lblNmBrand.Text = "All Brand"
    '            Else
    '                lblnobrand.Text = shsl
    '                'lblNmBrand.Text = "Selected Brand"
    '            End If

    '        End If



    '    Catch ex As Exception
    '        'MsgBox(ex.Message)
    '    End Try

    'End Sub

    'Sub Add3(Optional ByVal pilih As Boolean = True)

    '    Try
    '        Dim shsl As String = ""
    '        Dim i As Integer = 0
    '        If pilih = False Then
    '            For Each item As ListViewItem In ListViewCity.Items
    '                Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox3"), CheckBox)
    '                'Update Ihsan 19032019

    '                If ck.Checked Then
    '                    ck.Checked = False
    '                End If

    '            Next

    '            lblnoCity.Text = ""
    '            'LblNmCity.Text = "All City"
    '        Else
    '            For Each item As ListViewItem In ListViewCity.Items
    '                Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox3"), CheckBox)
    '                'Update Ihsan 19032019

    '                If ck.Checked Then
    '                    If shsl = "" Then
    '                        shsl = (DirectCast(item.FindControl("LblIdCity"), Label).Text)
    '                    Else
    '                        shsl = shsl & "," & (DirectCast(item.FindControl("LblIdCity"), Label).Text)
    '                    End If
    '                End If

    '            Next
    '            If shsl = "" Then
    '                lblnoCity.Text = ""
    '                'LblNmCity.Text = "All City"
    '            Else
    '                lblnoCity.Text = shsl
    '                ' LblNmCity.Text = "Selected City"
    '            End If

    '        End If



    '    Catch ex As Exception
    '        'MsgBox(ex.Message)
    '    End Try

    'End Sub

    Sub AddStock(Optional ByVal pilih As Boolean = True)

        Try
            Dim shsl As String = ""
            Dim i As Integer = 0
            If pilih = False Then
                For Each item As ListViewItem In LsvStock.Items
                    Dim ck As CheckBox = DirectCast(item.FindControl("CheckBoxStock"), CheckBox)
                    'Update Ihsan 19032019

                    If ck.Checked Then
                        ck.Checked = False
                    End If


                Next
                lblnostock.Text = ""
                lblStockNameData.Text = "All Stock"
            Else
                For Each item As ListViewItem In LsvStock.Items
                    Dim ck As CheckBox = DirectCast(item.FindControl("CheckBoxStock"), CheckBox)
                    'Update Ihsan 19032019

                    If ck.Checked Then
                        If shsl = "" Then
                            shsl = (DirectCast(item.FindControl("lblkodestock"), Label).Text)
                        Else
                            shsl = shsl & "," & (DirectCast(item.FindControl("lblkodestock"), Label).Text)
                        End If
                    End If


                Next
                If shsl = "" Then
                    lblnostock.Text = ""
                    lblStockNameData.Text = "All Stock"
                Else
                    lblnostock.Text = shsl
                    lblStockNameData.Text = "Selected Stock"
                End If
            End If




        Catch ex As Exception
            'MsgBox(ex.Message)
        End Try

    End Sub

    Protected Sub ddlRpt_SelectedIndexChanged(sender As Object, e As EventArgs)

        Try
            If ddlRpt.SelectedValue.ToString = "Inv MenuItem Cost" Or
            ddlRpt.SelectedValue.ToString = "Inv Minimum Stock" Or ddlRpt.SelectedValue.ToString = "Inv Stock OnHand(Current Stock)" Then
                calFrom.Visible = False
            Else
                calFrom.Visible = True
            End If


        Catch ex As Exception
            errLog(Err.Description, "reportInventory:ddlRpt_SelectedIndexChanged")
            Err.Clear()
        End Try
    End Sub

    Private Sub ShowToastr(ByVal page As Page, ByVal message As String, ByVal title As String, Optional ByVal type As String = "info", Optional ByVal clearToast As Boolean = False, Optional ByVal pos As String = "toast-top-left", Optional ByVal Sticky As Boolean = False)
        Dim toastrScript As String = [String].Format("Notify('{0}','{1}','{2}', '{3}', '{4}', '{5}');", message, title, type, clearToast, pos, Sticky)
        page.ClientScript.RegisterStartupScript(page.[GetType](), "toastr_message", toastrScript, addScriptTags:=True)
    End Sub

    'Private Sub btnShow_Click(sender As Object, e As EventArgs) Handles btnShow.Click

    'End Sub



    Private Sub showRpt(Optional ByVal rpt As String = "")
        Try
            'dg1.CurrentPageIndex = 0
            bindData_New(rpt)


            'ds.Dispose()
            ReportName.Text = ddlRpt.SelectedValue
            'If ddlRpt.Text = "Receipt Detail" Then
            If lblNoOutlet.Text = "" Or lblNoOutlet.Text = 0 Then
                allout = "All"
            Else
                allout = ""
            End If
            insertLogModule(username, ddlRpt.Text & ", " & txtFrom.Text & " / " & txtTo.Text & " " & allout, "Show", custid)
            'End If
            'programmaticModalPopup.Hide()

        Catch ex As Exception
            mfunction.errLog(Err.Description, "Report:showRpt")
            Err.Clear()
        End Try
    End Sub
    'Private Sub bindData(Optional ByVal rpt As String = "")

    '    dsrpt.Clear()


    '    Try
    '        'set dataparam
    '        dataParam = New ClsParamRpt
    '        dataParam = set_dataparam(False)


    '        If rpt = "" Then
    '            dsrpt = clsreport.getDatasetgv(ddlRpt.Text, dataParam)
    '        Else

    '            Dim aryTextFile() As String = rpt.Split(",")

    '            dsrpt = clsreport.getDatasetgv(aryTextFile(0), dataParam)
    '            dsrpt2 = clsreport.getDatasetgv(aryTextFile(1), dataParam)

    '        End If


    '        ' setColumName()
    '        'perubahan 04 jul 2017



    '        If dsrpt.Tables.Count > 0 Then
    '            dg1.DataSource = dsrpt
    '            ViewState("vsreport") = dsrpt.Tables(0)
    '            dg1.DataBind()
    '        Else
    '            dg1.DataSource = Nothing
    '            dg1.DataBind()
    '        End If

    '        'If ds2.Tables.Count > 0 Then
    '        '    dg2.DataSource = ds
    '        '    dg2.DataBind()
    '        'Else
    '        '    dg2.DataSource = Nothing
    '        '    dg2.DataBind()
    '        'End If


    '        'dg2.DataSource = ds2
    '        'dg2.DataBind()
    '        'dg2.Visible = True

    '        pagelbl.Text = " " & dg1.CurrentPageIndex + 1.ToString & " of " & dg1.PageCount.ToString & " "
    '        If dsrpt.Tables(0).Rows.Count <= 31 Then
    '            hidepage()
    '        Else
    '            hidefirst()
    '        End If

    '    Finally
    '        dsrpt.Dispose()
    '        'dsrpt2.Dispose()
    '    End Try
    'End Sub
    Private Sub bindData(Optional ByVal rpt As String = "")

        dsrpt.Clear()


        Try
            'set dataparam
            dataParam = New ClsParamRpt
            dataParam = set_dataparam(False)


            If rpt = "" Then
                dsrpt = clsreport.getDatasetgv(ddlRpt.Text, dataParam)
            Else

                Dim aryTextFile() As String = rpt.Split(",")

                dsrpt = clsreport.getDatasetgv(aryTextFile(0), dataParam)
                'dsrpt2 = clsreport.getDatasetgv(aryTextFile(1), dataParam)

            End If


            ' setColumName()
            'perubahan 04 jul 2017

            If dsrpt.Tables.Count > 0 Then
                Dim dt As New DataTable
                dt = dsrpt.Tables(0)
               
                BindGrid_table(dt)


                'dt.Rows.Clear()
                dt.Dispose()

                dsrpt.Tables.Clear()
                dsrpt.Dispose()
                lblMsg.Visible = False
               

            Else
                dg1.DataSource = Nothing
                dg1.DataBind()
                'paging()
                dsrpt.Tables.Clear()
                dsrpt.Dispose()
            End If

        Catch
            mfunction.errLog(Err.Description, "Report:BindData")
            Err.Clear()
            'Finally
            '    dsrpt.Dispose()
            'dsrpt2.Dispose()
        End Try
    End Sub

    Protected Sub dg1_PreRender(sender As Object, e As EventArgs) Handles dg1.PreRender
        If dg1.Rows.Count > 0 Then
            dg1.UseAccessibleHeader = True
            dg1.HeaderRow.TableSection = TableRowSection.TableHeader
            dg1.FooterRow.TableSection = TableRowSection.TableFooter
        End If
    End Sub
    Protected Sub btnShow_Click1(sender As Object, e As EventArgs)
        If CDate(txtTo.Text) < CDate(txtFrom.Text) Then
            mfunction.ClnMsg("date range not valid" & " Please check Again", Me.Page)
            Exit Sub
        ElseIf CDate(txtTo.Text) > CDate(Now) Or CDate(txtFrom.Text) > CDate(Now) Then
            mfunction.ClnMsg("date range not valid" & " Please check Again", Me.Page)
            Exit Sub
        ElseIf ddlRpt.SelectedItem.Text.Contains("2018") And (CDate(txtTo.Text).Year > 2018 Or CDate(txtFrom.Text).Year > 2018) Then
            mfunction.ClnMsg("you must select date before 2019" & " Please check Again", Me.Page)
            Exit Sub
        End If

        If ddlRpt.SelectedIndex >= 0 And CDate(txtTo.Text) >= CDate(txtFrom.Text) And (CDate(txtTo.Text) <= CDate(Now) Or CDate(txtFrom.Text) <= CDate(Now)) Then
            If ddlRpt.SelectedItem.Text.Contains("2018") And (CDate(txtTo.Text).Year > 2018 Or CDate(txtFrom.Text).Year > 2018) Then
                dg1.DataSource = Nothing
                dg1.DataBind()
                'hidepage()
            Else
                'If ddlRpt.SelectedValue = "Top 10 Item" Then
                '    showRpt("Top 10 Item by Amount,Top 10 Item by Qty")
                'Else
                showRpt()
                'End If

            End If

        Else
            dg1.DataSource = Nothing
            dg1.DataBind()
            'hidepage()
        End If
    End Sub
    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        Threading.Thread.Sleep(500)
        BindCheckCc()
        BindingCc()
        CheckedCc()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Pop", "openModalCc();", True)
        txtSearch.Attributes("onfocus") = "javascript:this.setSelectionRange(this.value.length,this.value.length);"
        txtSearch.Focus()
    End Sub
    Private Sub txtSearchStock_TextChanged(sender As Object, e As EventArgs) Handles txtSearchStock.TextChanged
        Threading.Thread.Sleep(500)
        BindCheckStock()
        BindingStock2()
        CheckedStock()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Pop", "openModalStock();", True)
        txtSearchStock.Attributes("onfocus") = "javascript:this.setSelectionRange(this.value.length,this.value.length);"
        txtSearchStock.Focus()
    End Sub

    Public Shared Sub BootstrapAlert(MsgLabel As Label, Message As String, Optional MessageType As BootstrapAlertType = BootstrapAlertType.Plain, Optional Dismissable As Boolean = False)
        Dim style As String = ""
        Dim icon As String = ""
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
    Public Enum BootstrapAlertType
        Plain
        Success
        Information
        Warning
        Danger
        Primary
    End Enum
    Public Function SelectDs_Cc_byuser(ByVal CustID As Integer, ByVal sText As String, ByVal user As String) As DataSet
        Dim ds As New DataSet
        Dim strfilter As String = ""

        Try
            Dim c_Cmd As New clsDbo(ServerString)

            'strfilter = "SELECT '0' AS ID, 'All' AS Nama UNION ALL " &
            ' OPTIMIZED: Use JOIN instead of IN (Subquery)
            strfilter = "SELECT A.* FROM cld_inv_cc A " &
                        "INNER JOIN cld_userxstore B ON A.olid = B.Store " &
                        "WHERE A.custid = " & CustID & " AND B.user = '" & user & "' "
            
            If Not String.IsNullOrEmpty(sText) Then
                strfilter &= " AND A.name LIKE '%" & sText & "%' "
            End If
            
            strfilter &= " ORDER BY A.id"

            ds = c_Cmd.ReturnDataset(strfilter)

            c_Cmd.dispose()
            c_Cmd = Nothing

        Catch ex As Exception
            errLog(Err.Description, "clsExportData:SelectDataset_Cc")
            Err.Clear()
        End Try


        Return ds
    End Function
    Public Function SelectDataset_Cc(ByVal CustID As Integer, ByVal sText As String) As DataSet
        Dim ds As New DataSet
        Dim strfilter As String = ""

        Try
            Dim c_Cmd As New clsDbo(ServerString)

            'strfilter = "SELECT '0' AS ID, 'All' AS Nama UNION ALL " &
            ' OPTIMIZED: Only apply LIKE if text is provided
            strfilter = "Select * from cld_inv_cc where custid = " & CustID
            
            If Not String.IsNullOrEmpty(sText) Then
                strfilter &= " AND name LIKE '%" & sText & "%' "
            End If

            strfilter &= " order by id"

            ds = c_Cmd.ReturnDataset(strfilter)

            c_Cmd.dispose()
            c_Cmd = Nothing

        Catch ex As Exception
            errLog(Err.Description, "clsExportData:SelectDataset_Cc")
            Err.Clear()
        End Try


        Return ds
    End Function
    Public Function GetKodeCc(ByVal nama As String) As String
        Dim sHasil As String = ""

        Dim ds As New DataSet
        Dim strFilter As String = ""

        Try

            Dim a As New clsDbo("Select ID from cld_inv_cc where Name  = '" & nama & "'")

            ds = a.selectDataset
            If ds.Tables(0).Rows.Count > 0 Then
                sHasil = ds.Tables(0).Rows(0)("ID")
            Else
                sHasil = ""
            End If
            a.dispose()
            a = Nothing

        Catch ex As Exception

            errLog(Err.Description, "clsOutlet:GetKodeCc")
            Err.Clear()

        End Try

        Return sHasil
    End Function
    Public Function SelectDataset_Stock(ByVal CustID As Integer, ByVal sText As String) As DataSet
        Dim ds As New DataSet
        Dim strfilter As String = ""

        Try
            Dim c_Cmd As New clsDbo(ServerString)

            'strfilter = "SELECT '0' AS ID, 'All' AS Nama UNION ALL " &
            ' OPTIMIZED: Only apply LIKE if text is provided
            strfilter = "Select * from cld_inv_stk where custid = " & CustID
            
            If Not String.IsNullOrEmpty(sText) Then
                strfilter &= " AND (name LIKE '%" & sText & "%' OR number LIKE '%" & sText & "%') "
            End If
            
            strfilter &= " order by id"

            ds = c_Cmd.ReturnDataset(strfilter)

            c_Cmd.dispose()
            c_Cmd = Nothing

        Catch ex As Exception
            errLog(Err.Description, "clsExportData:SelectDataset_Stock")
            Err.Clear()
        End Try


        Return ds
    End Function


    Private Sub bindData_New(Optional ByVal rpt As String = "")

        dsrpt.Clear()


        Try
            'set dataparam
            dataParam = New ClsParamRpt
            dataParam = set_dataparam(False)


            If rpt = "" Then
                dsrpt = clsreport.getDatasetgv(ddlRpt.Text, dataParam)
            Else

                Dim aryTextFile() As String = rpt.Split(",")

                dsrpt = clsreport.getDatasetgv(aryTextFile(0), dataParam)
                'dsrpt2 = clsreport.getDatasetgv(aryTextFile(1), dataParam)

            End If


            ' setColumName()
            'perubahan 04 jul 2017

            If dsrpt.Tables.Count > 0 Then
                Dim dt As New DataTable
                dt = dsrpt.Tables(0)
               
                BindGrid_table(dt)


                'dt.Rows.Clear()
                dt.Dispose()

                dsrpt.Tables.Clear()
                dsrpt.Dispose()
                lblMsg.Visible = False
               

            Else
                dg1.DataSource = Nothing
                dg1.DataBind()
                'paging()
                dsrpt.Tables.Clear()
                dsrpt.Dispose()
            End If

        Catch
            mfunction.errLog(Err.Description, "Report:BindData")
            Err.Clear()
            'Finally
            '    dsrpt.Dispose()
            'dsrpt2.Dispose()
        End Try
    End Sub

    Private Sub BindGrid_table(ByVal dt As DataTable)

        ' ViewState("vsreport") = Nothing

        'If dt.Rows.Count > 0 Then
        'report_display1.Visible = False
        'report_display2.Visible = True
        dg1.DataSource = Nothing
        dg1.Columns.Clear()
        dg1.AllowSorting = False
        For Each column As DataColumn In dt.Columns
            Dim bfield As New BoundField()
            bfield.HeaderText = column.ColumnName
            bfield.DataField = column.ColumnName
            bfield.SortExpression = If(column.ColumnName = "Sales Date", "", column.ColumnName)
            'bfield.SortExpression = column.ColumnName
            dg1.Columns.Add(bfield)
        Next
        'Dim dt2 As New DataTable
        'For Each dr As DataRow In dt.Rows
        '    dt2.Rows.Add(dr.ItemArray)
        'Next
        'ViewState("vsreport") = dt
        'gvreport.Columns("Sales Date").SortExpression = ""
        dg1.DataSource = dt 'DirectCast(ViewState("vsreport"), DataTable)
        dg1.DataBind()
        Session("DataAwal") = dt   ' SIMPAN DATA ASLI
        'h1Report.Visible = False
        'ReportName2.Visible = True
        ReportName.Text = ddlRpt.SelectedValue
        'End If


    End Sub
    Private Function GetColumnDataField(ByVal gridView As GridView, ByVal cellIndex As Integer) As String
        If gridView Is Nothing OrElse cellIndex < 0 OrElse cellIndex >= gridView.Columns.Count Then
            Return String.Empty
        End If

        Dim column = gridView.Columns(cellIndex)

        ' 1. BoundField ? langsung ambil DataField
        Dim boundField = TryCast(column, BoundField)
        If boundField IsNot Nothing AndAlso Not String.IsNullOrEmpty(boundField.DataField) Then
            Return boundField.DataField
        End If

        ' 2. TemplateField ? fallback ke HeaderText (atau custom logic)
        Dim templateField = TryCast(column, TemplateField)
        If templateField IsNot Nothing Then
            If Not String.IsNullOrEmpty(templateField.HeaderText) Then
                Return templateField.HeaderText ' atau simpan DataField di atribut lain
            End If
        End If

        ' 3. CommandField, ButtonField, etc.
        Dim commandField = TryCast(column, CommandField)
        If commandField IsNot Nothing Then
            Return commandField.HeaderText
        End If

        Return String.Empty
    End Function
    Protected Sub dg1_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles dg1.RowDataBound
        Dim skipFields As String() = {"Store Code", "Item Code"}
        'tambahan erik 30 oktober 25, untuk hitung sumnya dengan merubah format stringnya
        If e.Row.RowType = DataControlRowType.DataRow Then
            If e.Row.Cells(0).Text = "TOTAL" Then
                e.Row.Font.Bold = True
                e.Row.BackColor = System.Drawing.Color.LightGray
                e.Row.Cells(0).Font.Bold = True
            End If
            For i As Integer = 0 To e.Row.Cells.Count - 1
                Dim cellValue As String = e.Row.Cells(i).Text
                Dim decValue As Decimal
                Dim fieldName As String = GetColumnDataField(dg1, i)
                If Not String.IsNullOrEmpty(fieldName) AndAlso skipFields.Contains(fieldName, StringComparer.OrdinalIgnoreCase) Then
                    Continue For
                End If
                ' Jika cell numeric, format tampilan
                If Decimal.TryParse(cellValue, Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, decValue) Then
                    e.Row.Cells(i).Text = decValue.ToString("N2", New System.Globalization.CultureInfo("id-ID"))
                End If

                'Dim tgl As Date
                'If Date.TryParse(cellValue, tgl) Then
                '    e.Row.Cells(i).Text = tgl.ToString("dd-MMM-yyyy", New Globalization.CultureInfo("id-ID"))
                'End If

            Next
        End If

        If e.Row.RowType = DataControlRowType.Header Then
            e.Row.TableSection = TableRowSection.TableHeader
        ElseIf e.Row.RowType = DataControlRowType.Footer Then
            e.Row.TableSection = TableRowSection.TableFooter
        End If
    End Sub
    Protected Function coloumn_toSum(ByVal coloums As Integer) As String

        Dim str As String = ""
        For i As Integer = 0 To coloums - 1
            str = str & i
            If i < coloums - 1 Then
                str = str & (",")
            End If

            'i = i + 1
        Next


        Return str
    End Function
    Protected Function inner_colsum(ByVal coloums As Integer) As String

        Dim str As String = ""
        Dim tosum As String = ""
        Dim Ttls As String = ""
        If ReportName.Text = "Item Sales" Then
            tosum = "6,7"
            Ttls = "5"
        ElseIf ReportName.Text = "Sales Summary Report" Then
            tosum = "3,4,5,6,11,12,13,14,15"
            Ttls = "2"
        ElseIf ReportName.Text = "Payment Summary" Then
            tosum = "5,6"
            Ttls = "4"
        ElseIf ReportName.Text = "Sales Detail Report" Then
            tosum = "5,6,7,8,9,10,11"
            Ttls = "4"
        ElseIf ReportName.Text = "Discount Summary Report" Then
            tosum = "4,5"
            Ttls = "3"
        ElseIf ReportName.Text = "Sales By Channel" Then
            tosum = "4,5,6,7"
            Ttls = "3"
        ElseIf ReportName.Text = "Sales Vs Payment" Then
            tosum = "3,4,5"
            Ttls = "2"
        ElseIf ReportName.Text = "Payment Detail Report" Then
            tosum = "6"
            Ttls = "5"
        ElseIf ReportName.Text = "Receipt Detail" Then
            tosum = "10,12,15"
            Ttls = "9"
        ElseIf ReportName.Text = "Item Sales Detail" Then
            tosum = "4,5,6"
            Ttls = "3"
        ElseIf ReportName.Text = "Item Sales by Type" Then
            tosum = "6,10"
            Ttls = "5"
        ElseIf ReportName.Text = "Item Price Consolidate" Then
            tosum = "6,7"
            Ttls = "5"
        ElseIf ReportName.Text = "Item Sales Profit" Then
            tosum = "3,4,6,7"
            Ttls = "2"
        ElseIf ReportName.Text = "Item Sales Profit Add Cost" Then
            tosum = "4,10"
            Ttls = "2"
        ElseIf ReportName.Text = "Item Group Sales Report" Then
            tosum = "3,4"
            Ttls = "2"
        ElseIf ReportName.Text = "Item Sales Consolidate" Then
            tosum = "2,3,4"
            Ttls = "1"
        ElseIf ReportName.Text = "Sales By Channel consolidate" Then
            tosum = "3,4,5,6"
            Ttls = "1"
        ElseIf ReportName.Text = "Item Sales Summary By Store" Then
            tosum = "4,5,6"
            Ttls = "3"
        ElseIf ReportName.Text = "Item Sales Detail Consolidate" Then
            tosum = "3,4,5"
            Ttls = "2"
        ElseIf ReportName.Text = "Item Sales Detail Consolidate By Store" Then
            tosum = "5,6,7"
            Ttls = "4"
        ElseIf ReportName.Text = "Discount Report" Then
            tosum = "2"
            Ttls = "1"
        ElseIf ReportName.Text = "Daily Sales" Then
            tosum = "3"
            Ttls = "1"
        ElseIf ReportName.Text = "Sales Cover" Then
            tosum = "4,9,10,11,12"
            Ttls = "3"
        ElseIf ReportName.Text = "Inv Cost vs Sales" Then
            tosum = "4,5,6,7,8,9,10,11"
            Ttls = "3"
        ElseIf ReportName.Text = "Top Store Revenue by Hour" Then
            tosum = "3,4,5,6"
            Ttls = "1"
        ElseIf ReportName.Text = "Top Store Visit by Hour" Then
            tosum = "3,4,5,6"
            Ttls = "1"
        ElseIf ReportName.Text = "Item_Sales Consolidate By Combo" Then
            tosum = "2,3,4"
            Ttls = "1"
        ElseIf ReportName.Text = "Item Sales Consolidate By Combo" Then
            tosum = "4,5,6"
            Ttls = "1"
        End If
        For i As Integer = 0 To coloums
            If tosum.Contains(i) Then
                If IsNumeric(Left(tosum, 2)) Then
                    If i < Left(tosum, 2) Then
                        Continue For
                    End If
                End If


                'str = str & "api.column(" & i & ").footer().innerHTML = '' + api.column(columns[" & i & "], { filter: 'applied', page: 'current' }).data().sum()
                '            +' ('+ api.column(columns[" & i & "], { filter: 'applied', page: 'all' }).data().sum() +')';"

                'tambahan erik 30 oktober 25, untuk hitung sumnya dengan merubah format stringnya
                str &= "
                        var parseNumber = function (value) {
                            if (typeof value === 'string') {
                                value = value.replace(/\./g, '').replace(',', '.');
                                return parseFloat(value) || 0;
                            } else if (typeof value === 'number') {
                                return value;
                            } else {
                                return 0;
                            }
                        };

                        var colDataCurrent = api.column(columns[" & i & "], { filter: 'applied', page: 'current' }).data();
                        var colDataAll = api.column(columns[" & i & "], { filter: 'applied', page: 'all' }).data();

                        var sumCurrent = colDataCurrent.reduce(function(a, b) { return parseNumber(a) + parseNumber(b); }, 0);
                        var sumAll = colDataAll.reduce(function(a, b) { return parseNumber(a) + parseNumber(b); }, 0);

                        api.column(" & i & ").footer().innerHTML = sumCurrent.toLocaleString('id-ID', {minimumFractionDigits: 2}) + 
                            ' (' + sumAll.toLocaleString('id-ID', {minimumFractionDigits: 2}) + ')';
                        "

            End If
            If Ttls.Contains(i) Then
                str = str & "api.column(" & i & ").footer().innerHTML = 'Total (All)' ;"

            End If
        Next
        'str = ""
        tosum = ""
        Ttls = ""

        Return str
    End Function
End Class
