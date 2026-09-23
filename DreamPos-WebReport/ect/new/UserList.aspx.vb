Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Linq
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.Services.Description
Imports ClosedXML.Excel
Imports System.IO
Imports NPOI.XSSF.UserModel
Imports NPOI.SS.UserModel
'Imports System.Configuration
'Imports System.Drawing
'Imports System.Drawing.Imaging
Public Class UserList
    Inherits System.Web.UI.Page
    Private sNameuser As String

    Dim Pass As String
    Dim Priv As Integer
    Private mfunction As New ModFunction
    Private cHQ As New ClsHQ
    Private cuser As New clsUser
    Private citemmaster As New Clsitemmaster
    Private cout As New ClsOutlet
    Private xhref As String
    Dim typedet As Integer = 1
    Private str_id As Integer
    Private str_no As String
    Private str_name As String
    Dim tablename As String = "itemmaster"
    Private IMInfos As New List(Of Itemmasterlistinfo)()
    Private IMInfo As New Itemmasterlistinfo()

    'Private MajorgroupId As Integer = 0
    'Private userweb As Integer

    'Private transpriv As Integer

    <Serializable>
    Public Class Store_Detail 'Class untuk ListStore
        Public Property StoreName As String = ""
        Public Property Status As Boolean = False
    End Class

    <Serializable>
    Public Class StoreNumber
        Public Property StoreNumber As Integer = 0
    End Class

    Public Function susername() As String
        Dim cookie As HttpCookie = Request.Cookies("UserLogin")
        Return cookie("username")

    End Function
    Public Function iPriv() As Integer
        Dim cookie As HttpCookie = Request.Cookies("UserLogin")
        Return cookie("Priv")

    End Function
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not (User.Identity.IsAuthenticated) Then
            FormsAuthentication.RedirectToLoginPage()
            Exit Sub
        End If
        Dim cookie As HttpCookie = Request.Cookies("UserLogin")
        Dim car As HttpCookie = Request.Cookies("car")
        If Not car Is Nothing Then
            xhref = car.Value
        End If

        If Not cookie Is Nothing Then
            sNameuser = susername()
            Priv = iPriv()

            If Priv = 0 Then
                formload.Visible = False
                searchForm.Visible = False
            End If

            If Not Me.IsPostBack Then
                'BindingUser()

                getModulePriv()
                binding()

                Dim script As String = "$(document).ready(function () { $('[id*=btnShow]').click(); });"

                ClientScript.RegisterStartupScript(Me.GetType, "load", script, True)
                txtSearch.Attributes.Add("onkeyup", Me.Page.ClientScript.GetPostBackEventReference(txtSearch, ""))
                Checkbox_allstore.Attributes.Add("onclick", Me.Page.ClientScript.GetPostBackEventReference(Checkbox_allstore, ""))
                'ddlActive.Attributes.Add("onchange", "confirm2()")
                ScriptManager.GetCurrent(Me).RegisterPostBackControl(RadButton1)
                ' grpbtnstore.Visible = False
                txtSearch.Attributes.Add("onfocus", "javascript: this.setSelectionRange(this.value.length,this.value.length);")

            End If
            'txtCustID.Text = custid()
            'txtModule.Text = modul()
            'txtInvPriv.Text = 0
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
        Dim cookie As HttpCookie = Request.Cookies("UserLogin")
        Return cookie("ID")
    End Function

    Public Function FlagStatus() As Integer
        Dim cookie As HttpCookie = Request.Cookies("UserLogin")
        Return cookie("flagstatus")
    End Function

    Public Function UserTypeID() As Integer
        Dim cookie As HttpCookie = Request.Cookies("UserLogin")
        Return cookie("UserTypeID")
    End Function

    'Revisi 08 Juni
    Public Function StoreID() As Integer
        Dim cookie As HttpCookie = Request.Cookies("UserLogin")
        Return cookie("StoreID")

    End Function

    Public Function OutletID() As String
        Dim cookie As HttpCookie = Request.Cookies("UserLogin")
        Return cookie("OutletID")
    End Function



    Sub notsuccesfull(ByVal Optional message As String = "")
        If message = "" Then
            message = "Please Select Outlet"
        End If

        Dim sb As New System.Text.StringBuilder()

        sb.Append("<script type = 'text/javascript'>")
        sb.Append("window.onload=function(){")
        sb.Append("alert('")
        sb.Append(message)
        sb.Append("')};")
        sb.Append("</script>")
        ClientScript.RegisterClientScriptBlock(Me.GetType(), "Alert", sb.ToString())
    End Sub


    Protected Sub OnPagePropertiesChanging(ByVal sender As Object, ByVal e As PagePropertiesChangingEventArgs)
        'lvDataPager1.SetPageProperties(e.StartRowIndex, e.MaximumRows, False)

        'BindingUser()
    End Sub


    Protected Function GeName() As String

        Dim str As String = ""
        str = "User Access For " '& ddlUser.SelectedValue


        Return str
    End Function


    Sub binding() 'ambil data awal
        If Priv = 0 Then
            Response.Redirect("~/Default.aspx", False)
        Else
            'Cust = cookie("CustID")
        End If

        Dim dtitemmaster2 As New DataTable
        dtitemmaster2 = citemmaster.Selectlist_Itemmaster("").Tables(0)
        If dtitemmaster2.Rows.Count > 0 And dtitemmaster2.Rows.Count < 100 Then
            IMInfos = Helper.DataTableToList(Of Itemmasterlistinfo)(dtitemmaster2)
            ViewState("itemmasterListinfo") = dtitemmaster2
            gvItemMaster.DataSource = IMInfos
            gvItemMaster.DataBind()
        Else
            ViewState("itemmasterListinfo") = dtitemmaster2
            gvItemMaster.DataSource = dtitemmaster2
            gvItemMaster.DataBind()
        End If


        'MGInfos = Helper.DataTableToList(Of majorgroupListinfo)(dtstore2)
        'gvItemMaster.DataSource = IMInfos ' dtstore2
        'gvItemMaster.DataBind()
    End Sub
    Sub binding2()

        If ViewState("itemgroupListinfo") IsNot Nothing Then
            IMInfos = DirectCast(ViewState("itemmasterListinfo"), List(Of Itemmasterlistinfo))
        End If

        gvItemMaster.DataSource = IMInfos
        gvItemMaster.DataBind()
    End Sub


    Sub userExist()
        Dim message As String = "User already exist!"
        Dim sb As New System.Text.StringBuilder()

        sb.Append("<script type = 'text/javascript'>")
        sb.Append("window.onload=function(){")
        sb.Append("alert('")
        sb.Append(message)
        sb.Append("')};")
        sb.Append("</script>")
        ClientScript.RegisterClientScriptBlock(Me.GetType(), "Alert", sb.ToString())
    End Sub

    Private Sub getModulePriv()
        Dim newListItem As ListItem
        Dim newListItem2 As ListItem
        newListItem = New ListItem("Admin", "0")
        newListItem2 = New ListItem("User", "1")
        '  newListItem.Selected = True

        'With ddlPriv
        '    .Items.Clear()
        '    .Items.Add(newListItem)
        '    .Items.Add(newListItem2)
        'End With

    End Sub


    Protected Sub btnsrc_outxuser(ByVal sender As Object, ByVal e As EventArgs)
        Dim dtstore2 As New DataTable
        Try
            If ViewState("itemmasterListinfo") IsNot Nothing Then
                dtstore2 = DirectCast(ViewState("itemmasterListinfo"), DataTable)
                dtstore2.DefaultView.RowFilter = "majorgroupname like '%" & "" & "%' or storenumber like '%" & "" & "%' or storename like '%" & "" & "%' or itemnumber like '%" & "" & "%' or itemname like '%" & "" & "%' or itemgroupname like '%" & "" & "%'"
                dtstore2 = dtstore2.DefaultView.ToTable
                IMInfos = Helper.DataTableToList(Of Itemmasterlistinfo)(dtstore2)
            Else
                dtstore2 = citemmaster.Selectlist_Itemmaster("").Tables(0)
                If dtstore2.Rows.Count > 0 Then
                    'lstViewDetails.DataSource = dtstore2
                    'lstViewDetails.DataBind()
                    IMInfos = Helper.DataTableToList(Of Itemmasterlistinfo)(dtstore2)

                End If
            End If
            gvItemMaster.DataSource = IMInfos
            gvItemMaster.DataBind()
            'txtsrc_outxuser.Text = ""

        Catch ex As Exception
            mfunction.errLog(Err.Description, "Major_group:btnsrc_outxuser")
            Err.Clear()
        End Try
        dtstore2.Dispose()
        dtstore2.Clear()
        dtstore2 = Nothing
    End Sub


    Public Sub DataLoad()
        'lblMessage.Text = String.Empty
        Dim dt As New DataTable
        Try
            'objInfos = objCtrl.GetAllContacts()
            dt = citemmaster.SelectDataset_itemmaster("").Tables(0)
            IMInfos = Helper.DataTableToList(Of Itemmasterlistinfo)(dt)
            gvItemMaster.DataSource = dt
            gvItemMaster.DataBind()
        Catch ex As Exception
            'lblMessage.Text = ex.Message
        End Try
    End Sub

    Protected Sub gvItemMaster_PreRender(ByVal sender As Object, ByVal e As EventArgs)
        Dim gv As GridView = CType(sender, GridView)
        Try
            If (gv.ShowHeader = True AndAlso gv.Rows.Count > 0) OrElse (gv.ShowHeaderWhenEmpty = True) Then
                gv.HeaderRow.TableSection = TableRowSection.TableHeader
            End If

            If gv.ShowFooter = True AndAlso gv.Rows.Count > 0 Then
                gv.FooterRow.TableSection = TableRowSection.TableFooter
            End If
        Catch ex As Exception

        End Try

    End Sub

    Protected Sub gvItemMaster_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs)
        Dim rowindex As Integer = Convert.ToInt32(e.CommandArgument)
        'Dim str_id As Integer = Convert.ToInt32(gvItemMaster.DataKeys(rowindex).Values("store_id"))
        Dim itemmaster_id As Integer = Convert.ToInt32(gvItemMaster.DataKeys(rowindex).Values("itemmasterid"))
        Dim majorgroup_id As Integer = Convert.ToInt32(gvItemMaster.DataKeys(rowindex).Values("majorgroupid"))
        Dim itemgroup_id As Integer = Convert.ToInt32(gvItemMaster.DataKeys(rowindex).Values("itemgroupid"))
        'If IMInfos.Count = 0 Then

        '    IMInfos = Helper.DataTableToList(Of Itemmasterlistinfo)(citemmaster.Selectlist_Itemmaster("").Tables(0))
        'End If
        'Dim IMInfo = IMInfos.Where(Function(i) i.store_id = str_id And i.itemmasterid = itemmaster_id).FirstOrDefault
        Dim dt As New DataTable

        If e.CommandName.Equals("detail") Then
            'Dim detailInfos = objInfos.Where(Function(i) i.ContactId = ContactId): 'Sequence contains no 
            'dt =
            'DetailsView1.DataSource = detailInfos
            'DetailsView1.DataBind()
            'Dim sb As System.Text.StringBuilder = New System.Text.StringBuilder()
            'sb.Append("<script type='text/javascript'>")
            'sb.Append("$('#detailModal').modal('show');")
            'sb.Append("</script>")
            'ScriptManager.RegisterClientScriptBlock(Me, Me.[GetType](), "DetailModalScript", sb.ToString(), False)
        End If
        If e.CommandName.Equals("addstore") Then
            HfUpdateitemID.Value = itemmaster_id
            BindingStore()
            'BindCheckStore()
            CheckedStore()
            Dim sb As System.Text.StringBuilder = New System.Text.StringBuilder()
            sb.Append("<script type='text/javascript'>")
            sb.Append("$('#ModalStore').modal('show');")
            sb.Append("</script>")
            ScriptManager.RegisterClientScriptBlock(Me, Me.[GetType](), "ModalStoreScript", sb.ToString(), False)

        End If
        If e.CommandName.Equals("editRecord") Then


            Dim modfunc As New ModFunction
            mfunction.CreateCookie("itemmaster_edt_ID", itemmaster_id)
            mfunction.CreateCookie("majorgroup_edt_ID", majorgroup_id)
            mfunction.CreateCookie("itemgroup_edt_ID", itemgroup_id)


            Response.Redirect("~/ItemMaster_Edit.aspx", False)

        End If

        If e.CommandName.Equals("deleteRecord") Then

            HfDeleteID.Value = IMInfo.itemmasterid.ToString()
            'txtNameUpdate.Text = MGInfo.Name
            Dim sb As System.Text.StringBuilder = New System.Text.StringBuilder()
            sb.Append("<script type='text/javascript'>")
            sb.Append("$('#deleteModal').modal('show');")
            sb.Append("</script>")
            ScriptManager.RegisterClientScriptBlock(Me, Me.[GetType](), "DeleteModalScript", sb.ToString(), False)
        End If
    End Sub
    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim modfunc As New ModFunction
        mfunction.CreateCookie("itemmaster_edt_ID", 0)
        mfunction.CreateCookie("majorgroup_edt_ID", 0)
        mfunction.CreateCookie("itemgroup_edt_ID", 0)


        Response.Redirect("~/ItemMaster_Edit.aspx", False)
    End Sub

    Protected Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        Dim dt As DataTable
        Dim ds As New DataSet
        Try
            'ds = WService.getitemmaster_search(cStoreID, cOutletID, "", True)

            If ds.Tables.Count > 0 Then
                dt = ds.Tables(0)

                Using wb As New XLWorkbook()
                    wb.Worksheets.Add(dt, "Sheet1")
                    Response.Clear()
                    Response.Buffer = True
                    Response.Charset = ""
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                    Response.AddHeader("content-disposition", "attachment;filename=ItemMaster.xlsx")
                    Using MyMemoryStream As New MemoryStream()
                        wb.SaveAs(MyMemoryStream)
                        MyMemoryStream.WriteTo(Response.OutputStream)


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

    Protected Sub btnImport_Click(sender As Object, e As EventArgs) Handles btnImport.Click
        Import()
    End Sub
    Sub Import()

        Dim koder As String = ""

        FailureText.Text = ""

        Try

            If FileUpLoad1.HasFile Then

                Dim Number As String = 0
                Dim Name As String = ""
                Dim ItemGroup As String = ""
                Dim Price As String = ""
                Dim Symbol As String = ""
                Dim Barcode As String = ""

                Dim fileName As String = System.IO.Path.GetFileName(FileUpLoad1.PostedFile.FileName)
                FileUpLoad1.PostedFile.SaveAs(Server.MapPath(("~/Uploads/" + fileName)))
                Dim Extension As String = Path.GetExtension(FileUpLoad1.PostedFile.FileName)
                Dim FolderPath As String = Server.MapPath(FileUpLoad1.FileName)

                'Dim FilePath As String = Server.MapPath(FolderPath + FileName)
                Dim wb As IWorkbook = WorkbookFactory.Create(New FileStream(Server.MapPath(("~/Uploads/" + fileName)), FileMode.Open, FileAccess.Read))
                Dim sheet As ISheet = wb.GetSheet("Sheet1")
                Dim row As IRow
                Dim cel As ICell

                'ProgressBar1.Visible = True
                Dim x As Integer
                Dim ddd As Integer = sheet.LastRowNum

                x = 1

                Dim lines As Integer = x




                Try

                    Do While sheet.GetRow(x) IsNot Nothing

                        '    Application.DoEvents()
                        row = sheet.GetRow(x)
                        If row IsNot Nothing Then

                            'awal dikosongin dlu
                            'Dim lines1 As Integer = x + 1

                            Name = ""
                            Number = ""
                            ItemGroup = ""
                            Price = ""
                            Symbol = ""
                            Barcode = ""


                            cel = row.GetCell(0)
                            If cel IsNot Nothing Then
                                If cel.CellType = CellType.STRING Then
                                    Number = cel.StringCellValue
                                End If
                            Else

                                Number = "0"
                            End If

                            cel = row.GetCell(1)
                            If cel IsNot Nothing Then
                                If cel.CellType = CellType.STRING Then
                                    Name = cel.StringCellValue.Replace("'", "")
                                Else
                                    Name = cel.StringCellValue.Replace("'", "")
                                End If

                            End If

                            cel = row.GetCell(2)
                            If cel IsNot Nothing Then
                                If cel.CellType = CellType.STRING Then
                                    ItemGroup = cel.StringCellValue.Replace("'", "")
                                End If

                            End If

                            cel = row.GetCell(3)
                            If cel IsNot Nothing Then
                                If cel.CellType = CellType.STRING Then
                                    Price = cel.StringCellValue
                                Else
                                    Price = cel.NumericCellValue
                                End If
                            End If

                            cel = row.GetCell(4)
                            If cel IsNot Nothing Then
                                If cel.CellType = CellType.STRING Then
                                    Symbol = cel.StringCellValue
                                End If

                            End If

                            cel = row.GetCell(5)
                            If cel IsNot Nothing Then
                                If cel.CellType = CellType.STRING Then
                                    Barcode = cel.StringCellValue

                                End If

                            End If

                            If (Number <> "0") And (IsNumeric(Price) = True) And Name.Length <= 25 Then

                                Try
                                    'WService.setItem_Excel(Number, Name, ItemGroup, Price, Symbol, Barcode, cStoreID, cOutletID, cIdUser, chkLog.Checked)
                                Catch ex As Exception

                                    koder = koder & " Error insert in Row " & lines & " " & Err.Description & vbCrLf
                                    Err.Clear()

                                End Try
                            Else
                                koder = koder & " Not valid Kode or Price or Name.Length > 25 in Row " & lines & " " & Err.Description & vbCrLf
                            End If


                        End If

                        x += 1
                        lines = x
                    Loop

                Catch ex As Exception
                    koder = koder & "Error in Row " & lines & " " & Err.Description
                    'Dim koderr As String = "Error in Row " & lines & " " & Err.Description
                    'msgerr += msgerr & newLine & koderr
                    'errLogimport("Error in Row " & lines & " " & Err.Description, "Pages_ExportMember:ImportmemberData")

                    Err.Clear()

                End Try


                GC.Collect()
                GC.WaitForPendingFinalizers()

            Else
                FailureText.Text = "Please Choose File "

            End If


        Catch ex As Exception
            koder = koder & Err.Description

            Err.Clear()
        End Try


        If FailureText.Text = "" Then
            If koder <> "" Then
                'messagesuccesfull("Import From Excel Successfully But Error :" & msgerr)
                FailureText.Text = koder
            Else

                FailureText.Text = "Import From Excel Successfully"

                'set Dataset
                Dim ds As New DataSet
                'ds = WService.getitemmaster_search(cStoreID, cOutletID, txtsearch.Text, False)
                'lstViewDetails.DataSource = ds
                'lstViewDetails.DataBind()
                'ds.Dispose()


            End If



        End If

    End Sub

    Protected Sub RadButton1_ServerClick(sender As Object, e As EventArgs) 'Handles RadButton1.ServerClick
        Try
            Add()
            'btnshowmdlstore.Visible = False
            'btnInsert.Visible = True
        Catch ex As Exception

        End Try

    End Sub

    Protected Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        Threading.Thread.Sleep(500)
        BindCheckStore()
        BindingStore2()
        CheckedStore()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Pop", "openModalStore();", True)
        txtSearch.Attributes("onfocus") = "javascript:this.setSelectionRange(this.value.length,this.value.length);"
        txtSearch.Focus()
    End Sub
    Sub Add(Optional ByVal pilih As Boolean = True)

        Try
            Dim shsl As String = ""
            Dim i As Integer = 0
            Dim ListStoreNumber As List(Of Store_Detail) = New List(Of Store_Detail)()

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
                Dim ListStore2 As List(Of Store_Detail) = New List(Of Store_Detail)()
                Dim dt As New DataTable
                For Each item As ListViewItem In LsvStk.Items
                    Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox"), CheckBox)
                    'Update Ihsan 19032019

                    If ck.Checked Then
                        Try
                            Dim ds As DataSet = cuser.getstorebyid((DirectCast(item.FindControl("lblkode"), Label).Text))
                            If ds.Tables(0).Rows.Count > 0 Then
                                str_id = ds.Tables(0).Rows(0).Item("id").ToString
                                str_no = ds.Tables(0).Rows(0).Item("number").ToString
                                str_name = ds.Tables(0).Rows(0).Item("name").ToString
                                ListStoreNumber.Add(New Store_Detail With {
                                   .StoreName = str_name,
                                   .Status = True
                                   })

                                If ViewState("StoreItem") Is Nothing Then
                                    dt = DirectCast(ViewState("ListStore"), DataTable)
                                    dt.Rows.Clear()
                                    dt.Rows.Add(str_id, str_no, str_name)
                                Else
                                    dt = DirectCast(ViewState("StoreItem"), DataTable)
                                    If dt.Rows(0).Item("id") <> str_id Then
                                        dt.Rows.Add(str_id, str_no, str_name)
                                    End If
                                End If




                                'ViewState("ListStoreNumber") = ListStore2
                                'akhir nambah ke liststorenumber

                                'Dim dtlstore As DataTable = DirectCast(ViewState("ListStore"), DataTable)
                                'dtlstore.Rows(i).Delete()
                                'dtlstore.AcceptChanges()
                                'ViewState("ListStore") = dtlstore
                                i = i - 1
                                'dtlstore.Clear()
                                'dtlstore.Dispose()
                                'dtlstore = Nothing
                            End If
                                'BindingStore2()
                                'binding2()
                                txtSearch.Text = ""
                            'ds.Clear()
                            'ds.Dispose()
                            'ds = Nothing
                            'ddlPeriode.Enabled = "false"
                        Catch error_t As Exception
                            MsgBox(error_t.ToString)
                        End Try

                        'ListStoreNumber.Add(cHQ.GetIDOutlet(DirectCast(item.FindControl("lblName"), Label).Text))

                    End If
                    i = i + 1

                Next
                ViewState("StoreItem") = dt

                'nambah ke liststorenumber
                'Dim ListStore2 As List(Of Store_Detail) = New List(Of Store_Detail)()

                If dt.Rows.Count > 0 Then
                    For Each xrow As DataRow In dt.Rows
                        ListStore2.Add(New Store_Detail With {
                                            .StoreName = xrow.Item("name"),
                                            .Status = True
                                            })
                    Next
                End If
                ViewState("ListStoreNumber") = ListStore2 'Number
                itemxstoreload()
            End If

            'BindingStore2()
            'binding2()



            'Next

        Catch ex As Exception
            mfunction.errLog(Err.Description, "Item_group:Add")
            Err.Clear()
        End Try

    End Sub
    'Start : Store
    Private Sub BindCheckStore()
        Try

            If CBool(IIf(IsNothing(ViewState("ListStoreNumber")), True, False)) Then


                Dim ListStore2 As List(Of Store_Detail) = New List(Of Store_Detail)()

                Dim dtstore, dtstore_get As New DataTable

                dtstore = DirectCast(ViewState("ListStore"), DataTable)
                dtstore_get = cuser.getstoreitem(HfUpdateitemID.Value).Tables(0)

                If dtstore_get.Rows.Count > 0 Then
                    For Each dt As DataRow In dtstore_get.Rows
                        ListStore2.Add(New Store_Detail With {
                                           .StoreName = dt.Item("name"),
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

                ViewState("ListStoreNumber") = ListStore2
                'ListView1.DataSource = ViewState("ListStore")
                'ListView1.DataBind()
                'dtstore.Clear()
                'dtstore.Dispose()
                'dtstore = Nothing
                'dtstore_get.Clear()
                'dtstore_get.Dispose()
                'dtstore_get = Nothing
            Else

                Dim ListStore2 As List(Of Store_Detail) = TryCast(ViewState("ListStoreNumber"), List(Of Store_Detail))

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
                ViewState("ListStoreNumber") = ListStore2
            End If

        Catch ex As Exception
            mfunction.errLog(Err.Description, "Item_group:BindCheckStore")
            Err.Clear()
        End Try
    End Sub

    Private Sub CheckedStore()
        Try


            Dim ListStore As List(Of Store_Detail) = TryCast(ViewState("ListStoreNumber"), List(Of Store_Detail))

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
            ViewState("ListStoreNumber") = ListStore
        Catch ex As Exception
            mfunction.errLog(Err.Description, "Item_group:CheckedStore")
            Err.Clear()
        End Try
    End Sub
    'END : Store

    Sub BindingStore() 'store yang belum terpilih

        Try
            If Priv = 0 Then
                Response.Redirect("~/Default.aspx", False)
            Else
                Dim dt, dtitemstore As New DataTable
                Dim ListStore2 As List(Of Store_Detail) = New List(Of Store_Detail)()

                dtitemstore = cuser.getstoreitem(HfUpdateitemID.Value).Tables(0)
                If dtitemstore.Rows.Count > 0 Then
                    For Each xrow As DataRow In dtitemstore.Rows
                        ListStore2.Add(New Store_Detail With {
                                    .StoreName = xrow.Item("name"),
                                    .Status = True
                                    })
                    Next
                    ViewState("StoreItem") = dtitemstore

                End If
                ViewState("ListStoreNumber") = ListStore2
                'dt = cuser.getds_notin_item_Store(HfUpdateitemID.Value, txtSearch.Text).Tables(0)
                dt = cuser.getstore_all(txtSearch.Text).Tables(0)
                ViewState("ListStore") = dt
                If dt.Rows.Count > 0 Then
                    LsvStk.Items.Clear()
                    countlvstore = dt.Rows.Count
                    dt.DefaultView.RowFilter = "Name like '%" & txtSearch.Text & "%'"
                    LsvStk.DataSource = dt.DefaultView
                    LsvStk.DataBind()
                    UpdatePanel3.Update()



                End If
                'dtstr = cuser.getds_notin_item_Store(HfUpdateitemID.Value, txtSearch.Text).Tables(0)
                'If dtstr.Rows.Count > 0 Then
                '    For Each xrow As DataRow In dtstr.Rows
                '        ListStore2.Add(New Store_Detail With {
                '                    .StoreName = xrow.Item("name"),
                '                    .Status = False
                '                    })
                '    Next
                '    ViewState("ListStoreNumber") = dtitemstore
                'End If
            End If
        Catch ex As Exception
            mfunction.errLog(Err.Description, "Item_group:BindingStore")
            Err.Clear()
        End Try
    End Sub
    Sub BindingStore2()

        Dim dtstore2 As New DataTable

        Try
            If ViewState("ListStore") IsNot Nothing Then
                dtstore2 = DirectCast(ViewState("ListStore"), DataTable)
                'Else
                'dtstore2 = cuser.getds_view_upd_Store(CDate(txtBusidate.Text), txtSearch.Text, "NOT", "majorgroup").Tables(0)
                'ViewState("ListStore") = dtstore2
            End If


            If dtstore2.Rows.Count > 0 Then
                LsvStk.Items.Clear()
                countlvstore = dtstore2.Rows.Count
                dtstore2.DefaultView.RowFilter = "Name like '%" & txtSearch.Text & "%'"
                LsvStk.DataSource = dtstore2.DefaultView
                LsvStk.DataBind()
                UpdatePanel3.Update()
            End If
        Catch ex As Exception
            mfunction.errLog(Err.Description, "Item_group:BindingStore2")
            Err.Clear()
        End Try
        'dtstore2.Clear()
        'dtstore2.Dispose()
        'dtstore2 = Nothing
    End Sub
    Protected Sub Checkbox_allstore_CheckedChanged(sender As Object, e As EventArgs)
        For Each item As ListViewItem In LsvStk.Items
            Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox"), CheckBox)
            ck.Checked = Checkbox_allstore.Checked
        Next

        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Pop", "openModalStore();", True)
    End Sub
    Public Sub itemxstoreload()

        Dim dtstore As DataTable = Nothing
        Dim updatedstores As New List(Of ItemxStoreInfo)
        Dim ListStore2 As List(Of Store_Detail) = TryCast(ViewState("ListStoreNumber"), List(Of Store_Detail))
        Dim strid As String = ""
        For Each a In ListStore2
            If a.Status = True Then

                strid = strid & "'" & cHQ.GetIDOutlet(a.StoreName) & "'" & ","
            End If
        Next
        Try
            strid = strid.Substring(0, strid.Length - 1)
            dtstore = citemmaster.Selectdt_itemstore2(HfUpdateitemID.Value, strid, "")
            ISInfos = Helper.DataTableToList(Of ItemxStoreInfo)(dtstore)

            Session("ItemxStoreInfo") = ISInfos

            citemmaster.insert_itemxstore(HfUpdateitemID.Value, ISInfos)

            'If Session("updatedItemxStore") IsNot Nothing Then
            '    updatedstores = TryCast(Session("updatedItemxStore"), List(Of ItemxStoreInfo))
            'End If

            'ISInfos = TryCast(Session("ItemxStoreInfo"), List(Of ItemxStoreInfo))
            'ISInfo = ISInfos.FirstOrDefault(Function(i) i.StoreiD = HfUpdateitemID.Value)

            'If ISInfo IsNot Nothing Then
            '    ISInfo.StoreiD = Convert.ToInt32(HfUpdateitemID.Value)
            '    ISInfo.BitDisplay = 1
            '    ISInfo.DiscItemizerID = 0
            '    ISInfo.ItemmasterId = HfUpdateitemID.Value

            '    ISInfo.Price1 = 0
            '    ISInfo.Price2 = 0
            '    ISInfo.Price3 = 0
            '    ISInfo.Price4 = 0
            '    ISInfo.Price5 = 0
            '    ISInfo.Price6 = 0
            '    ISInfo.Price7 = 0
            '    ISInfo.Price8 = 0
            '    ISInfo.Price9 = 0
            '    ISInfo.Price10 = 0
            '    ISInfo.Price11 = 0
            '    ISInfo.Price12 = 0
            '    ISInfo.Price13 = 0
            '    ISInfo.Price14 = 0
            '    ISInfo.Price15 = 0
            '    updatedstores.Add(ISInfo)
            '    Session("updatedItemxStore") = updatedstores
            '    Session("ItemxStoreInfo") = ISInfos

            'citemmaster.insert_itemxstore(HfUpdateitemID.Value, updatedstores)

            'End If
        Catch ex As Exception
            lblMessage.Text = ex.Message
        End Try

    End Sub
End Class