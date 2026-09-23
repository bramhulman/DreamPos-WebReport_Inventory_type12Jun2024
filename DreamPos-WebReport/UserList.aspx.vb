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
Imports Org.BouncyCastle.Asn1.X509
'Imports System.Configuration
'Imports System.Drawing
'Imports System.Drawing.Imaging
Public Class UserList
    Inherits System.Web.UI.Page
    Private sNameuser As String

    Dim Pass As String
    'Dim modulePriv As Integer
    Private mfunction As New ModFunction
    'Private cOutlet As New ClsOutlet
    Private cuser As New clsUser
    Private cout As New ClsOutlet
    Private xhref As String
    Dim typedet As Integer = 1
    Private str_id As Integer
    Private str_no As String
    Private str_name As String
    Dim tablename As String = "user"
    Private UxoInfos As New List(Of UserxOutletInfo)()
    Private UxoInfo As New UserxOutletInfo()
    Private UInfos As New List(Of UserListInfo)()
    Private UInfo As New UserListInfo()

    'Private MajorgroupId As Integer = 0
    'Private userweb As Integer

    'Private transpriv As Integer
    <Serializable>
    Public Class UserxOutletInfo
        Public Property user As String
        Public Property outletid As Integer

    End Class
    <Serializable>
    Public Class UserListInfo
        Public Property Usr As String
        Public Property psw As String
        Public Property modulepriv As Integer
        Public Property usertype As String
        Public Property modultype As String
        Public Property rptpriv As Integer
        Public Property email As String
        Public Property modul As Integer
        Public Property rptf As Integer
        Public Property invpriv As Integer
    End Class
    <Serializable>
    Public Class Store_Detail 'Class untuk ListStore
        Public Property StoreName As String = ""
        Public Property Status As Boolean = False
    End Class

    <Serializable>
    Public Class StoreNumber
        Public Property StoreNumber As Integer = 0
    End Class
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
        If cookie Is Nothing Then
            Return ""
        Else
            Return cookie("userName")
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
    Public Function invpriv() As Integer
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        If cookie Is Nothing Then
            Return -1
        Else
            Return cookie("invPriv")
        End If


    End Function
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not (User.Identity.IsAuthenticated) Then
            FormsAuthentication.RedirectToLoginPage()
            Exit Sub
        End If
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        Dim car As HttpCookie = Request.Cookies("car")
        If Not car Is Nothing Then
            xhref = car.Value
        End If

        If Not cookie Is Nothing Then
            sNameuser = username()



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


        Dim dtuser2 As New DataTable
        dtuser2 = cuser.Selectlist_User("").Tables(0)
        If dtuser2.Rows.Count > 0 And dtuser2.Rows.Count < 100 Then
            UInfos = DataTableToList(Of UserListInfo)(dtuser2)
            ViewState("userListinfo") = UInfos 'dtuser2
            gvUser.DataSource = ViewState("userListinfo") 'UInfos
            gvUser.DataBind()
        Else
            ViewState("userListinfo") = dtuser2
            gvUser.DataSource = ViewState("userListinfo") 'dtuser2
            gvUser.DataBind()
        End If
        dtuser2.Rows.Clear()
        dtuser2.Dispose()

        'MGInfos = Helper.DataTableToList(Of majorgroupListinfo)(dtstore2)
        'gvItemMaster.DataSource = IMInfos ' dtstore2
        'gvItemMaster.DataBind()
    End Sub
    Sub binding2()

        'If ViewState("itemgroupListinfo") IsNot Nothing Then
        UInfos = DirectCast(ViewState("userListinfo"), List(Of UserListInfo))
        'End If

        gvUser.DataSource = UInfos
        gvUser.DataBind()
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
        Dim dtuser2 As New DataTable
        Try
            If ViewState("userListinfo") IsNot Nothing Then
                dtuser2 = DirectCast(ViewState("userListinfo"), DataTable)
                dtuser2.DefaultView.RowFilter = "usr like '%" & "" & "%' or usertype like '%" & "" & "%' or email like '%" & "" & "%' or modultype like '%" & "" & "%' or modul like '%" & "" & "%' or rptf like '%" & "" & "%' or invpriv like '%" & "" & "%'"
                dtuser2 = dtuser2.DefaultView.ToTable
                UInfos = DataTableToList(Of UserListInfo)(dtuser2)
            Else
                dtuser2 = cuser.Selectlist_User("").Tables(0)
                If dtuser2.Rows.Count > 0 Then
                    'lstViewDetails.DataSource = dtstore2
                    'lstViewDetails.DataBind()
                    UInfos = DataTableToList(Of UserListInfo)(dtuser2)

                End If
            End If
            gvUser.DataSource = UInfos
            gvUser.DataBind()
            'txtsrc_outxuser.Text = ""

        Catch ex As Exception
            mfunction.errLog(Err.Description, "Major_group:btnsrc_outxuser")
            Err.Clear()
        End Try
        dtuser2.Dispose()
        dtuser2.Clear()
        dtuser2 = Nothing
    End Sub


    Public Sub DataLoad()
        'lblMessage.Text = String.Empty
        Dim dt As New DataTable
        Try
            'objInfos = objCtrl.GetAllContacts()
            dtuser2 = cuser.Selectlist_User("").Tables(0)
            UInfos = DataTableToList(Of UserListInfo)(dt)
            gvUser.DataSource = dt
            gvUser.DataBind()
        Catch ex As Exception
            'lblMessage.Text = ex.Message
        End Try
    End Sub

    Protected Sub gvUser_PreRender(ByVal sender As Object, ByVal e As EventArgs)
        Dim gv As GridView = CType(sender, GridView)
        Try
            If gv IsNot Nothing Then
                If gv.HeaderRow IsNot Nothing Then
                    gv.HeaderRow.TableSection = TableRowSection.TableHeader
                End If

                If gv.FooterRow IsNot Nothing Then
                    gv.FooterRow.TableSection = TableRowSection.TableFooter
                End If
            End If
        Catch ex As Exception

        End Try

    End Sub

    Protected Sub gvUser_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs)
        Dim rowindex As Integer = Convert.ToInt32(e.CommandArgument)
        'Dim str_id As Integer = Convert.ToInt32(gvItemMaster.DataKeys(rowindex).Values("store_id"))
        Dim usr As String = (gvUser.DataKeys(rowindex).Values(0).ToString)
        Dim psw As String = (gvUser.DataKeys(rowindex).Values(1).ToString)

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
            HfUpdateuser.Value = usr
            HfUpdatepsw.Value = psw
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
            mfunction.CreateCookie("usr_edt", usr)
            mfunction.CreateCookie("psw_edt", psw)



            Response.Redirect("~/User_Edit.aspx", False)

        End If

        If e.CommandName.Equals("deleteRecord") Then

            HfDeleteusr.Value = UInfo.Usr.ToString()
            HfDeletepsw.Value = UInfo.Usr.ToString()
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
        mfunction.CreateCookie("usr_edt", "")
        mfunction.CreateCookie("psw_edt", "")



        Response.Redirect("~/User_Edit.aspx", False)
    End Sub

    Protected Sub btnExport_Click(sender As Object, e As EventArgs) 'Handles btnExport.Click
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

    'Protected Sub btnImport_Click(sender As Object, e As EventArgs) Handles btnImport.Click
    '    Import()
    'End Sub
    Sub Import()

        Dim koder As String = ""

        FailureText.Text = ""

        Try

            'If FileUpLoad1.HasFile Then

            '    Dim Number As String = 0
            '    Dim Name As String = ""
            '    Dim ItemGroup As String = ""
            '    Dim Price As String = ""
            '    Dim Symbol As String = ""
            '    Dim Barcode As String = ""

            '    Dim fileName As String = System.IO.Path.GetFileName(FileUpLoad1.PostedFile.FileName)
            '    FileUpLoad1.PostedFile.SaveAs(Server.MapPath(("~/Uploads/" + fileName)))
            '    Dim Extension As String = Path.GetExtension(FileUpLoad1.PostedFile.FileName)
            '    Dim FolderPath As String = Server.MapPath(FileUpLoad1.FileName)

            '    'Dim FilePath As String = Server.MapPath(FolderPath + FileName)
            '    Dim wb As IWorkbook = WorkbookFactory.Create(New FileStream(Server.MapPath(("~/Uploads/" + fileName)), FileMode.Open, FileAccess.Read))
            '    Dim sheet As ISheet = wb.GetSheet("Sheet1")
            '    Dim row As IRow
            '    Dim cel As ICell

            '    'ProgressBar1.Visible = True
            '    Dim x As Integer
            '    Dim ddd As Integer = sheet.LastRowNum

            '    x = 1

            '    Dim lines As Integer = x




            '    Try

            '        Do While sheet.GetRow(x) IsNot Nothing

            '            '    Application.DoEvents()
            '            row = sheet.GetRow(x)
            '            If row IsNot Nothing Then

            '                'awal dikosongin dlu
            '                'Dim lines1 As Integer = x + 1

            '                Name = ""
            '                Number = ""
            '                ItemGroup = ""
            '                Price = ""
            '                Symbol = ""
            '                Barcode = ""


            '                cel = row.GetCell(0)
            '                If cel IsNot Nothing Then
            '                    If cel.CellType = CellType.STRING Then
            '                        Number = cel.StringCellValue
            '                    End If
            '                Else

            '                    Number = "0"
            '                End If

            '                cel = row.GetCell(1)
            '                If cel IsNot Nothing Then
            '                    If cel.CellType = CellType.STRING Then
            '                        Name = cel.StringCellValue.Replace("'", "")
            '                    Else
            '                        Name = cel.StringCellValue.Replace("'", "")
            '                    End If

            '                End If

            '                cel = row.GetCell(2)
            '                If cel IsNot Nothing Then
            '                    If cel.CellType = CellType.STRING Then
            '                        ItemGroup = cel.StringCellValue.Replace("'", "")
            '                    End If

            '                End If

            '                cel = row.GetCell(3)
            '                If cel IsNot Nothing Then
            '                    If cel.CellType = CellType.STRING Then
            '                        Price = cel.StringCellValue
            '                    Else
            '                        Price = cel.NumericCellValue
            '                    End If
            '                End If

            '                cel = row.GetCell(4)
            '                If cel IsNot Nothing Then
            '                    If cel.CellType = CellType.STRING Then
            '                        Symbol = cel.StringCellValue
            '                    End If

            '                End If

            '                cel = row.GetCell(5)
            '                If cel IsNot Nothing Then
            '                    If cel.CellType = CellType.STRING Then
            '                        Barcode = cel.StringCellValue

            '                    End If

            '                End If

            '                If (Number <> "0") And (IsNumeric(Price) = True) And Name.Length <= 25 Then

            '                    Try
            '                        'WService.setItem_Excel(Number, Name, ItemGroup, Price, Symbol, Barcode, cStoreID, cOutletID, cIdUser, chkLog.Checked)
            '                    Catch ex As Exception

            '                        koder = koder & " Error insert in Row " & lines & " " & Err.Description & vbCrLf
            '                        Err.Clear()

            '                    End Try
            '                Else
            '                    koder = koder & " Not valid Kode or Price or Name.Length > 25 in Row " & lines & " " & Err.Description & vbCrLf
            '                End If


            '            End If

            '            x += 1
            '            lines = x
            '        Loop

            '    Catch ex As Exception
            '        koder = koder & "Error in Row " & lines & " " & Err.Description
            '        'Dim koderr As String = "Error in Row " & lines & " " & Err.Description
            '        'msgerr += msgerr & newLine & koderr
            '        'errLogimport("Error in Row " & lines & " " & Err.Description, "Pages_ExportMember:ImportmemberData")

            '        Err.Clear()

            '    End Try


            '    GC.Collect()
            '    GC.WaitForPendingFinalizers()

            'Else
            '    FailureText.Text = "Please Choose File "

            'End If


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
        'BindCheckStore()
        'BindingStore2()
        'CheckedStore()
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
                If ViewState("StoreUser") IsNot Nothing Then
                    dt = DirectCast(ViewState("ListStore"), DataTable)
                    dt.Rows.Clear()
                    ViewState("StoreUser") = dt
                End If
                For Each item As ListViewItem In LsvStk.Items
                    Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox"), CheckBox)
                    'Update Ihsan 19032019

                    If ck.Checked Then
                        Try
                            Dim ds As DataSet = cuser.getstorebyid((DirectCast(item.FindControl("lblkode"), Label).Text))
                            If ds.Tables(0).Rows.Count > 0 Then
                                str_id = ds.Tables(0).Rows(0).Item("id").ToString
                                str_no = ds.Tables(0).Rows(0).Item("kode").ToString
                                str_name = ds.Tables(0).Rows(0).Item("nama").ToString
                                ListStoreNumber.Add(New Store_Detail With {
                                   .StoreName = str_name,
                                   .Status = True
                                   })

                                If ViewState("StoreUser") Is Nothing Then
                                    dt = DirectCast(ViewState("ListStore"), DataTable)
                                    dt.Rows.Clear()
                                    dt.Rows.Add(str_id, str_no, str_name)
                                Else
                                    dt = DirectCast(ViewState("StoreUser"), DataTable)
                                    'If dt.Rows(0).Item("id") <> str_id Then
                                    dt.Rows.Add(str_id, str_no, str_name)
                                    'End If
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
                ViewState("StoreUser") = dt

                'nambah ke liststorenumber
                'Dim ListStore2 As List(Of Store_Detail) = New List(Of Store_Detail)()

                If dt.Rows.Count > 0 Then
                    For Each xrow As DataRow In dt.Rows
                        ListStore2.Add(New Store_Detail With {
                                            .StoreName = xrow.Item("nama"),
                                            .Status = True
                                            })
                    Next
                End If
                ViewState("ListStoreNumber") = ListStore2 'Number
                userxstoreload()
            End If

            'BindingStore2()
            'binding2()

            Response.Redirect("~/UserList.aspx", False)

            'Next

        Catch ex As Exception
            mfunction.errLog(Err.Description, "Item_group:Add")
            Err.Clear()
        End Try

    End Sub
    'Start : Store

    Protected Sub Checkbox_allstore_CheckedChanged(sender As Object, e As EventArgs)
        For Each item As ListViewItem In LsvStk.Items
            Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox"), CheckBox)
            ck.Checked = Checkbox_allstore.Checked
        Next

        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Pop", "openModalStore();", True)
    End Sub

    'Start : Store
    Private Sub BindCheckStore()
        Try

            If CBool(IIf(IsNothing(ViewState("ListStoreNumber")), True, False)) Then


                Dim ListStore2 As List(Of Store_Detail) = New List(Of Store_Detail)()

                Dim dtstore, dtstore_get As New DataTable

                dtstore = DirectCast(ViewState("ListStore"), DataTable)
                dtstore_get = cuser.getstoreuser(HfDeleteusr.Value).Tables(0)

                If dtstore_get.Rows.Count > 0 Then
                    For Each dt As DataRow In dtstore_get.Rows
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

                ViewState("ListStoreNumber") = ListStore2

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

            Dim dt, dtuserstore As New DataTable
            Dim ListStore2 As List(Of Store_Detail) = New List(Of Store_Detail)()

            dtuserstore = cuser.getstoreuser(HfUpdateuser.Value).Tables(0)
            If dtuserstore.Rows.Count > 0 Then
                For Each xrow As DataRow In dtuserstore.Rows
                    ListStore2.Add(New Store_Detail With {
                                    .StoreName = xrow.Item("nama"),
                                    .Status = True
                                    })
                Next
                ViewState("StoreUser") = dtuserstore

            End If
            ViewState("ListStoreNumber") = ListStore2
                'dt = cuser.getds_notin_item_Store(HfUpdateitemID.Value, txtSearch.Text).Tables(0)
                dt = cuser.getstore_all(txtSearch.Text).Tables(0)
                ViewState("ListStore") = dt
                If dt.Rows.Count > 0 Then
                    LsvStk.Items.Clear()
                    countlvstore = dt.Rows.Count
                dt.DefaultView.RowFilter = "Nama like '%" & txtSearch.Text & "%'"
                LsvStk.DataSource = dt.DefaultView
                    LsvStk.DataBind()
                    UpdatePanel3.Update()



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

            End If


            If dtstore2.Rows.Count > 0 Then
                LsvStk.Items.Clear()
                countlvstore = dtstore2.Rows.Count
                dtstore2.DefaultView.RowFilter = "Nama like '%" & txtSearch.Text & "%'"
                LsvStk.DataSource = dtstore2.DefaultView
                LsvStk.DataBind()
                UpdatePanel3.Update()
            End If
        Catch ex As Exception
            mfunction.errLog(Err.Description, "Item_group:BindingStore2")
            Err.Clear()
        End Try

    End Sub
    Public Sub userxstoreload()

        Dim dtstore As DataTable = Nothing
        Dim updatedstores As New List(Of UserxOutletInfo)
        Dim ListStore2 As List(Of Store_Detail) = TryCast(ViewState("ListStoreNumber"), List(Of Store_Detail))
        Dim strid As String = ""
        For Each a In ListStore2
            If a.Status = True Then

                strid = strid & "'" & cuser.GetIDOutlet(a.StoreName) & "'" & ","
            End If
        Next
        Try
            strid = strid.Substring(0, strid.Length - 1)
            dtstore = cuser.Selectdt_useroutlet(HfUpdateuser.Value, strid, "")
            updatedstores = DataTableToList(Of UserxOutletInfo)(dtstore)

            Session("ItemxStoreInfo") = updatedstores

            cuser.insert_userxoutlet(HfUpdateuser.Value, updatedstores)


        Catch ex As Exception
            lblMessage.Text = ex.Message
        End Try

    End Sub
End Class