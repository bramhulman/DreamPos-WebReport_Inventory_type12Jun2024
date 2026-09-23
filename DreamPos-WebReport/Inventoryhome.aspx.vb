Imports System.Reflection
Imports ClosedXML.Excel
Imports System.IO
Imports Microsoft.VisualBasic
Imports System.Configuration
Imports System.Globalization
Imports System.Net
Imports System.Web.Compilation
Imports System.Data
Imports System.Web.Script.Serialization
Imports System.Security.Cryptography
Imports DocumentFormat.OpenXml.Drawing.Charts
Imports DataTable = System.Data.DataTable

Public Class Inventoryhome
    Inherits System.Web.UI.Page


    Private sNameuser As String
    Private icustid As Integer
    Private imodul As Integer
    Private iModPriv As Integer
    Private irptpriv As Integer
    Private irptf As Integer
    Private mfunction As New ModFunction

    Private clsreport As New ClsReports
    Private clsOut As New ClsOutlet
    Private clsExport As New clsExportData
    Private cOut As New ClsOutlet
    Public storename As String = ""
    Dim versionNumber As Version = Assembly.GetExecutingAssembly().GetName().Version
    Public countlvbrand As Integer
    Public countlvcity As Integer
    Public countlvstore As Integer
    Public allout As String
    Dim clsInv As New clsInventory
    Private _invHomeListInfos As List(Of InvHomeListInfo) = Nothing
    Private Property InvHomeListInfos As List(Of InvHomeListInfo)
        Get
            If _invHomeListInfos Is Nothing Then _invHomeListInfos = New List(Of InvHomeListInfo)()
            Return _invHomeListInfos
        End Get
        Set(value As List(Of InvHomeListInfo))
            _invHomeListInfos = value
        End Set
    End Property

    Dim clslstkgroup As New Cls_IStkGroup
    Dim invpriv As Integer
    Dim cinvProcess As New clsInventoryProcess
    Private prevGroupName As String = ""
    Private InvHomeListInfo As New InvHomeListInfo()

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
    Public Function inv_priv() As Integer
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")

        Return cookie("invpriv")

    End Function


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        mHelper.CekSession()
        Dim bprocessUsageData As Boolean = False

        'If Session("username") Is Nothing Or custid() < 1 Then
        '    Session.Clear()
        '    Session.Abandon()
        '    Session.RemoveAll()
        '    FormsAuthentication.RedirectToLoginPage()
        '    Exit Sub
        'End If
        Try

            If Request.Cookies("UserLogin_inv") IsNot Nothing Then
                sNameuser = username()
                icustid = custid()
                imodul = modul()
                iModPriv = modulepriv()
                irptpriv = rptpriv()
                irptf = rptf()
                invpriv = inv_priv()
                'Session("login_inv") = "success"
                If IsPostBack = False Then
                    If Session("FlashMessage") IsNot Nothing Then
                        Dim flashMsg As String = Session("FlashMessage").ToString()
                        Session.Remove("FlashMessage")
                        Dim cleanMsg As String = flashMsg.Replace("\", "\\").Replace("'", "\'").Replace(vbCrLf, "\n").Replace(vbCr, "\n").Replace(vbLf, "\n")
                        Dim flashScript As String = "$(document).ready(function() { alert('" & cleanMsg & "'); });"
                        ClientScript.RegisterStartupScript(Me.GetType(), "FlashAlert", flashScript, True)
                    End If

                    'ScriptManager.GetCurrent(Me).RegisterPostBackControl(ddlOutlet)
                    'ScriptManager.GetCurrent(Me).RegisterPostBackControl(ddlGrp)
                    'ScriptManager.GetCurrent(Me).RegisterPostBackControl(RadButton3)


                    Dim c_cmd As New Cls_command

                    'cekclosing:
                    If icustid = 0 Or (c_cmd.ExecuteScalar("SELECT COUNT(DISTINCT custid) FROM cld_inv_closing", 0) > 1) Then
                        icustid = c_cmd.ExecuteScalar("SELECT id FROM `cld_cust` LIMIT 1", 0)
                    End If

                    ' OPTIMIZED: Get closing status in ONE query instead of 3
                    Dim invDate As String = Date.Now.ToString("yyyy-MM-dd")
                    Dim closingStatus As Integer = -1
                    Dim objStatus As Object = c_cmd.ExecuteScalar("SELECT invflag FROM cld_inv_closing WHERE custid = " & icustid & " AND invdate = '" & invDate & "' LIMIT 1", Nothing)

                    If objStatus IsNot Nothing AndAlso Not IsDBNull(objStatus) Then
                        closingStatus = Convert.ToInt32(objStatus)
                    End If

                    ' OPTIMIZED: Skip processing if already Done (1) or currently Processing (2)
                    If closingStatus = 1 Or closingStatus = 2 Then
                        bprocessUsageData = True
                    End If

                    If closingStatus = -1 Then
                        Session.Clear()
                        Session.Abandon()
                        Session.RemoveAll()

                        showmessage("Closing process,Please login again.")
                        Exit Sub

                    ElseIf closingStatus = 0 Then

                        Session.Clear()
                        Session.Abandon()
                        Session.RemoveAll()

                        showmessage("Closing process not complete yet,Please login again.")
                        Exit Sub
                    End If

                    'tambahan Bram 5Okt 2023 tambah cost di cld_inv_stk_cc
                    cinvProcess.incost(custid)

                    'loadOutlet()
                    'loadGroup()

                    lblTitle.Text = "Inventory Stock " & Date.Now.Date.ToString("dd MMM yyyy") & ""
                    prevGroupName = ""

                    ' --- OPTIMIZATION START: Logic moved inside IsPostBack ---
                    Dim c_dbo As New clsDbo("")
                    If bprocessUsageData = False Then
                        c_dbo.executeNonQuery("update cld_inv_closing set invflag = 2 where invdate = '" & Date.Now.Date.ToString("yyyy-MM-dd 00:00:00") & "'")
                        clsInv.Process_InvUsage(Now.AddDays(-2), custid)
                        clsInv.Process_InvUsage(Now.AddDays(-1), custid)
                        clsInv.Process_InvUsage(CDate(Now), custid)
                        c_dbo.executeNonQuery("update cld_inv_closing set invflag = 1 where invdate = '" & Date.Now.Date.ToString("yyyy-MM-dd 00:00:00") & "'")
                    End If

                    ' Alert Logic
                    Dim dsusage As New DataSet
                    dsusage = clsInv.lastdate_InvUsage(custid)

                    If dsusage.Tables(0).Rows.Count > 0 Then
                        For Each row As DataRow In dsusage.Tables(0).Rows
                            Dim ldate As String = row.Item("ldate").ToString
                            alert1.Text = "( Need recalculate from " & ldate & "! )"
                            If getlatepost() <> "" Then
                                alert1.Text = "( Need recalculate from " & ldate & "! )" & "<br/>" & getlatepost.ToString
                            End If
                            alert1.Attributes.Add("style", "Z-INDEX: 101; OVERFLOW: hidden; color:red; ")
                            alert1.Visible = True
                            pnlAlert.Visible = True
                            Exit For
                        Next
                    Else
                        Dim msg As String = ""
                        msg = getlatepost.ToString
                        If msg <> "" Then
                            alert1.Text = getlatepost.ToString
                            alert1.Attributes.Add("style", "Z-INDEX: 101; OVERFLOW: hidden; color:red; ")
                            alert1.Visible = True
                            pnlAlert.Visible = True
                        Else
                            alert1.Text = ""
                            alert1.Visible = False
                            alert1.Attributes.Add("style", "display:none; ")
                            pnlAlert.Visible = False
                        End If
                    End If

                    ' 1. Pastikan saldo awal hari ini sudah tercipta dari closing kemarin (sebelum query dijalankan)
                    insert_closing()

                    ' 2. Binding data hari ini (query database dan panggil DataBind() satu kali saja)
                    binding()

                    ' 3. Isi dropdown filter dari list data
                    FillClientFilters()
                    ' --- OPTIMIZATION END ---

                    CheckCostAnomalies()
                End If
                'txtsearch.Attributes.Add("onkeyup", Me.Page.ClientScript.GetPostBackEventReference(txtsearch, ""))
                'txtsearch.Attributes.Add("onfocus", "javascript: this.setSelectionRange(this.value.length,this.value.length);")
                'untuk process usage dari transaksi
                'clsInv.proc_invusage(icustid)
                ' Logic moved inside IsPostBack for optimization


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
            mfunction.errLog(Err.Description, "InventoryHome:Page_Load")
            Err.Clear()
        End Try
    End Sub
    Protected Sub Page_unLoad(sender As Object, e As EventArgs) Handles Me.Unload
        Try
            'WService.Dispose()


        Catch ex As Exception

            mfunction.errLog(Err.Description, "InventoryHome:Page_unLoad")
            Err.Clear()
        End Try
    End Sub
    Protected Sub btnSubmit2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles RadButton3.ServerClick
        Dim c_cmd As New Cls_command

        openlabel.Text = "Please wait.."
        clsInv.InvIn(icustid, username, iModPriv)
        binding()


    End Sub

    Sub messagesuccesfull(ByVal msg As String, Optional ByVal bsuccess As Boolean = True)

        Dim message As String = msg
        Dim url As String
        If bsuccess Then
            url = "../Inventoryhome.aspx"
        Else
            url = "Default.aspx"
        End If

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
        Dim url As String = "/Inventoryhome.aspx"
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
        ' Dim url As String = "~/Pages_InvTransfer.aspx"
        sb.Append("<script type = 'text/javascript'>")
        sb.Append("window.onload=function(){")
        sb.Append("alert('")
        sb.Append(message)
        sb.Append("')};")
        sb.Append("</script>")
        ClientScript.RegisterClientScriptBlock(Me.GetType(), "Alert", sb.ToString())
    End Sub
    Sub FillClientFilters()
        If InvHomeListInfos Is Nothing OrElse InvHomeListInfos.Count = 0 Then Exit Sub

        '--- Cost Center ---
        Dim ccList = InvHomeListInfos.Select(Function(x) x.ccname).Distinct().OrderBy(Function(x) x).ToList()
        ddlCcFilter.Items.Clear()
        ddlCcFilter.Items.Add(New ListItem("-- All Cost Centres --", ""))
        For Each cc In ccList : ddlCcFilter.Items.Add(New ListItem(cc, cc)) : Next

        '--- Stock Group ---
        Dim grpList = InvHomeListInfos.Select(Function(x) x.sgname).Distinct().OrderBy(Function(x) x).ToList()
        ddlGrpFilter.Items.Clear()
        ddlGrpFilter.Items.Add(New ListItem("-- All Groups --", ""))
        For Each grp In grpList : ddlGrpFilter.Items.Add(New ListItem(grp, grp)) : Next


    End Sub

    Private Function GetColumnDataField(ByVal gridView As GridView, ByVal cellIndex As Integer) As String
        If gridView Is Nothing OrElse cellIndex < 0 OrElse cellIndex >= gridView.Columns.Count Then
            Return String.Empty
        End If

        Dim column = gridView.Columns(cellIndex)

        ' 1. BoundField → langsung ambil DataField
        Dim boundField = TryCast(column, BoundField)
        If boundField IsNot Nothing AndAlso Not String.IsNullOrEmpty(boundField.DataField) Then
            Return boundField.DataField
        End If

        ' 2. TemplateField → fallback ke HeaderText (atau custom logic)
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
    Protected Sub gvHome_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvHome.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim item As InvHomeListInfo = TryCast(e.Row.DataItem, InvHomeListInfo)
            If item IsNot Nothing Then
                Dim ttlqty As Double = item.ttlqty
                Dim stkLimit As Double = item.stkLimit

                If stkLimit <= 0 Then
                    e.Row.CssClass = "table-warning"
                ElseIf ttlqty <= 0 Then
                    e.Row.CssClass = "table-danger"
                ElseIf ttlqty <= stkLimit * 0.3 Then
                    e.Row.CssClass = "table-danger"
                ElseIf ttlqty <= stkLimit * 0.7 Then
                    e.Row.CssClass = "table-warning"
                Else
                    e.Row.CssClass = "table-success"
                End If

                ' Format angka rupiah/desimal secara langsung & cepat tanpa refleksi lambat
                Dim idCulture As New System.Globalization.CultureInfo("id-ID")
                e.Row.Cells(5).Text = item.cost.ToString("N2", idCulture)
                e.Row.Cells(6).Text = item.Total.ToString("N2", idCulture)
                e.Row.Cells(7).Text = item.stkLimit.ToString("N2", idCulture)
            End If
        End If
        If e.Row.RowType = DataControlRowType.Header Then
            e.Row.TableSection = TableRowSection.TableHeader
        ElseIf e.Row.RowType = DataControlRowType.Footer Then
            e.Row.TableSection = TableRowSection.TableFooter
        End If
    End Sub
    Sub binding()

        Dim ds As New DataSet
        Dim dthome As New DataTable
        Try
            ds = clsInv.SelectDataStkHomeSearch2(icustid, 0, 0, 0, username, iModPriv)

            If ds.Tables(0).Rows.Count > 0 Then
                dthome = ds.Tables(0)
                Dim list As New List(Of InvHomeListInfo)
                For Each dr As DataRow In dthome.Rows
                    Dim obj As New InvHomeListInfo
                    obj.custid = If(IsDBNull(dr("custid")), 0, dr("custid"))
                    obj.ccid = If(IsDBNull(dr("ccid")), 0, dr("ccid"))
                    obj.stkid = If(IsDBNull(dr("stkid")), 0, dr("stkid"))
                    obj.ccname = dr("ccname").ToString()
                    obj.stkname = dr("stkname").ToString()
                    obj.sgname = dr("sgname").ToString()
                    obj.ttlqty = If(IsDBNull(dr("ttlqty")), 0, CDec(dr("ttlqty")))
                    obj.cost = If(IsDBNull(dr("cost")), 0, CDec(dr("cost")))
                    obj.Total = If(IsDBNull(dr("Total")), 0, CDec(dr("Total")))
                    obj.stkLimit = If(IsDBNull(dr("stkLimit")), 0, CDec(dr("stkLimit")))
                    obj.unit2 = dr("unit2").ToString()
                    obj.stat = dr("stat").ToString()

                    ' Format: 10.00 kg
                    obj.ttlqty_with_unit = obj.ttlqty.ToString("N2", New System.Globalization.CultureInfo("id-ID")) & " " & obj.unit2

                    list.Add(obj)
                Next

                InvHomeListInfos = list
                gvHome.DataSource = InvHomeListInfos
                gvHome.DataBind()
            Else
                InvHomeListInfos = New List(Of InvHomeListInfo)()
                gvHome.DataSource = InvHomeListInfos
                gvHome.DataBind()
            End If
            dthome.Rows.Clear()
            dthome.Dispose()
        Catch ex As Exception
            mfunction.errLog(Err.Description, "InventoryHome:binding")
            Err.Clear()
        End Try

    End Sub


    Protected Function GetBackground(ByVal Type As Boolean)

        Dim imgstat As String
        If Type = True Then
            imgstat = "bgtablewhite"
        Else
            imgstat = "bgtablelavender"

        End If

        Return imgstat
    End Function

    Protected Function getlatepost()

        Dim msg As String = ""

        Dim newLine As String = "<br/>"
        Dim dtpost As New DataSet
        Dim c_cmd As New Cls_command
        Try
            ' OPTIMIZED: Use JOIN instead of dependent subqueries, and simplified Date logic
            dtpost = c_cmd.ReturnDataset("SELECT O.nama as outlet, MAX(L.postdate) as latepost " &
                                         "FROM cld_last L " &
                                         "INNER JOIN cld_outlet O ON L.storeid = O.id " &
                                         "WHERE O.active = 1 AND O.custid = " & icustid & " " &
                                         "GROUP BY L.storeid, O.nama " &
                                         "HAVING MAX(L.postdate) < DATE_SUB(NOW(), INTERVAL 8 HOUR)")
            If dtpost.Tables(0).Rows.Count > 0 Then
                For Each row As DataRow In dtpost.Tables(0).Rows
                    msg = msg + row.Item("outlet") + " last sync " + Format(row.Item("latepost"), "dd MMM yyyy HH:mm:ss") & newLine
                Next
            End If
        Catch ex As Exception
            mfunction.errLog(Err.Description, "InventoryHome:getlatepost")
            Err.Clear()
        End Try

        Return msg
    End Function

    'Protected Sub lstView_ItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ListViewCommandEventArgs) Handles lstViewDetails.ItemCommand
    '    '<%--autocomplete="off"--%>
    '    Select Case (e.CommandName)


    '        Case "Edt"
    '            ID = Convert.ToInt32(e.CommandArgument)

    '            'editlist(ID)
    '            mfunction.CreateCookie("Outlet_edt_ID", e.CommandArgument)

    '            'CreateCookie("Name", getStoreID())
    '            Response.Redirect("outlet_Edit")



    '    End Select
    'End Sub
    'Protected Sub OnPagePropertiesChanging(ByVal sender As Object, ByVal e As PagePropertiesChangingEventArgs)
    '    lvDataPager1.SetPageProperties(e.StartRowIndex, e.MaximumRows, False)
    '    bindData2()
    '    'binding()
    'End Sub
    'Protected Sub btnsearch(ByVal sender As Object, ByVal e As EventArgs)
    '    Try

    '        Dim dt As New DataTable
    '        dt = ViewState("homeStk")
    '        dt.DefaultView.RowFilter = "stkname like '%" & txtsearch.Text & "%'"


    '        lstViewDetails.DataSource = dt.DefaultView
    '        lstViewDetails.DataBind()
    '    Catch ex As Exception
    '        mfunction.errLog(Err.Description, "InventoryHome:btnsearch")
    '        Err.Clear()
    '    End Try
    'End Sub
    'Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtsearch.TextChanged
    '    Threading.Thread.Sleep(500)
    '    Dim dt As New DataTable
    '    Try
    '        dt = ViewState("homeStk")
    '        dt.DefaultView.RowFilter = "stkname like '%" & txtsearch.Text & "%'"


    '        lstViewDetails.DataSource = dt.DefaultView
    '        lstViewDetails.DataBind()
    '    Catch ex As Exception
    '        mfunction.errLog(Err.Description, "InventoryHome:txtSearch_TextChanged")
    '        Err.Clear()
    '    End Try

    'End Sub
    Sub showmessage(ByVal msg As String, Optional ByVal bsuccess As Boolean = False)

        Dim message As String = msg
        Dim url As String
        If bsuccess Then
            url = "Inventoryhome.aspx"
        Else
            url = "Default.aspx"
        End If

        Dim script As String = "window.onload = function(){ alert('"
        script += message
        script += "');"
        script += "window.location.href = '"
        script += url
        script += "'; }"
        ClientScript.RegisterStartupScript(Me.GetType(), "Redirect", script, True)

    End Sub

    'Private Sub Inventoryhome_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
    '    ' If c_cmd.ExecuteScalar("select count(custid) from cld_inv_closing where custid = " & custid() & " and invdate = '" & Date.Now.Date.ToString("yyyy-MM-dd") & "' and  invflag = 2 ", 0) >= 1 Then
    '    ' bprocessUsageData = True
    '    ' End If
    'End Sub

    Protected Sub insert_closing()


        Dim dtpost As New DataSet
        Dim c_cmd As New Cls_command
        Try
            ' OPTIMIZED: INSERT directly with LEFT JOIN (No preliminary check needed)
            Dim yesterday As String = Date.Now.AddDays(-1).ToString("yyyy-MM-dd 00:00:00")
            Dim today As String = Date.Now.ToString("yyyy-MM-dd 00:00:00")

            Dim strSql As String = "INSERT INTO `cld_inv_transstockcls` " &
                                   "SELECT ADDDATE(T1.invdate, 1), T1.custid, T1.stkid, T1.stkno, T1.stkname, T1.ccid, T1.ccname, T1.cost, T1.ttlqty, T1.cost, 0, 0, 0, 0, 0, 0, 0, 0, T1.bclose, 0, 0, 0, 0, 0, 0, 0, 0 " &
                                   "FROM `cld_inv_transstockcls` T1 " &
                                   "LEFT JOIN `cld_inv_transstockcls` T2 ON T1.ccid = T2.ccid AND T1.stkid = T2.stkid AND T2.invdate = '" & today & "' " &
                                   "WHERE T1.invdate = '" & yesterday & "' AND T2.ccid IS NULL"

            c_cmd.executeNonQuery(strSql)

        Catch ex As Exception
            mfunction.errLog(Err.Description, "InventoryHome:insert_closing")
            Err.Clear()
        End Try

    End Sub

    Protected Sub gvHome_PreRender(sender As Object, e As EventArgs) 'Handles gvHome.PreRender
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

    Private Sub CheckCostAnomalies()
        Try
            Dim c_cmd As New clsDbo("")
            ' Query mendeteksi transaksi masuk dalam 30 hari terakhir yang mengalami anomali / lonjakan cost mencurigakan (>= 50%)
            ' Hanya menampilkan outlet/cost centre aktif (cld_inv_cc.custid <> 0)
            ' Membandingkan cost transaksi dengan opencost hari ini, closecost/cost kemarin, atau master price
            Dim qAnomaly As String = "SELECT t.id, DATE_FORMAT(t.invdate, '%d-%b-%Y') AS tgl, t.cc, COALESCE(cc.name, t.Cc_name) AS Cc_name, t.stk_id, t.stk_name, " &
                                     "t.ttlstock, t.cost, t.type, " &
                                     "CASE t.type " &
                                     "    WHEN 1 THEN 'Transfer In' " &
                                     "    WHEN 8 THEN 'HO In' " &
                                     "    WHEN 9 THEN 'Batch Prod' " &
                                     "    ELSE 'Trans In' " &
                                     "END AS type_desc, " &
                                     "ROUND(COALESCE(NULLIF(today.opencost, 0), NULLIF(c.closecost, 0), NULLIF(c.cost, 0), NULLIF(s.price2, 0), s.price1), 2) AS baseline_cost, " &
                                     "ROUND(((t.cost - COALESCE(NULLIF(today.opencost, 0), NULLIF(c.closecost, 0), NULLIF(c.cost, 0), NULLIF(s.price2, 0), s.price1)) / COALESCE(NULLIF(today.opencost, 0), NULLIF(c.closecost, 0), NULLIF(c.cost, 0), NULLIF(s.price2, 0), s.price1)) * 100, 1) AS pct_diff " &
                                     "FROM cld_inv_trans t " &
                                     "INNER JOIN cld_inv_cc cc ON t.cc = cc.id AND cc.custid <> 0 " &
                                     "INNER JOIN cld_inv_stk s ON t.stk_id = s.id AND t.custid = s.custid " &
                                     "LEFT JOIN cld_inv_transstockcls today " &
                                     "       ON t.stk_id = today.stkid AND t.cc = today.ccid AND t.custid = today.custid " &
                                     "      AND today.invdate = t.invdate " &
                                     "LEFT JOIN cld_inv_transstockcls c " &
                                     "       ON t.stk_id = c.stkid AND t.cc = c.ccid AND t.custid = c.custid " &
                                     "      AND c.invdate = DATE_SUB(DATE(t.invdate), INTERVAL 1 DAY) " &
                                     "WHERE t.custid = " & custid() & " " &
                                     "  AND t.type IN (1, 8, 9) " &
                                     "  AND t.flagtrans = 1 " &
                                     "  AND t.invdate >= DATE_SUB(CURDATE(), INTERVAL 30 DAY) " &
                                     "  AND t.ttlstock > 0 " &
                                     "  AND t.cost > 0 " &
                                     "  AND COALESCE(NULLIF(today.opencost, 0), NULLIF(c.closecost, 0), NULLIF(c.cost, 0), NULLIF(s.price2, 0), s.price1) > 0 " &
                                     "  AND ( " &
                                     "      t.cost >= 1.5 * COALESCE(NULLIF(today.opencost, 0), NULLIF(c.closecost, 0), NULLIF(c.cost, 0), NULLIF(s.price2, 0), s.price1) " &
                                     "      OR " &
                                     "      t.cost <= 0.5 * COALESCE(NULLIF(today.opencost, 0), NULLIF(c.closecost, 0), NULLIF(c.cost, 0), NULLIF(s.price2, 0), s.price1) " &
                                     "  ) " &
                                     "ORDER BY t.invdate DESC, t.id DESC " &
                                     "LIMIT 20"

            Dim dt As DataTable = c_cmd.ReturnDataset(qAnomaly).Tables(0)
            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                Dim serializer As New JavaScriptSerializer()
                Dim rows As New List(Of Dictionary(Of String, Object))()
                For Each dr As DataRow In dt.Rows
                    Dim row As New Dictionary(Of String, Object)()
                    For Each col As DataColumn In dt.Columns
                        row.Add(col.ColumnName, dr(col))
                    Next
                    rows.Add(row)
                Next
                Dim jsonAnomalies As String = serializer.Serialize(rows)
                Dim script As String = "var anomalyData = " & jsonAnomalies & "; $(document).ready(function() { if (typeof initAnomalyBubble === 'function') { initAnomalyBubble(anomalyData); } });"
                ClientScript.RegisterStartupScript(Me.GetType(), "AnomalyAlertScript", script, True)
            End If
        Catch ex As Exception
            mfunction.errLog(ex.Message, "inventoryHome:CheckCostAnomalies")
        End Try
    End Sub

    Protected Sub btnAutoRecalculateAnomalies_Click(sender As Object, e As EventArgs)
        Try
            Dim c_cmd As New clsDbo("")
            ' 1. Ambil tanggal terawal (MIN(invdate)) dan jumlah baris anomali (maks 20)
            Dim qMinDate As String = "SELECT MIN(DATE(sub.invdate)) AS min_date, COUNT(*) AS cnt " &
                                     "FROM (" &
                                     "    SELECT t.invdate " &
                                     "    FROM cld_inv_trans t " &
                                     "    INNER JOIN cld_inv_cc cc ON t.cc = cc.id AND cc.custid <> 0 " &
                                     "    INNER JOIN cld_inv_stk s ON t.stk_id = s.id AND t.custid = s.custid " &
                                     "    LEFT JOIN cld_inv_transstockcls today " &
                                     "           ON t.stk_id = today.stkid AND t.cc = today.ccid AND t.custid = today.custid " &
                                     "          AND today.invdate = t.invdate " &
                                     "    LEFT JOIN cld_inv_transstockcls c " &
                                     "           ON t.stk_id = c.stkid AND t.cc = c.ccid AND t.custid = c.custid " &
                                     "          AND c.invdate = DATE_SUB(DATE(t.invdate), INTERVAL 1 DAY) " &
                                     "    WHERE t.custid = " & custid() & " " &
                                     "      AND t.type IN (1, 8, 9) " &
                                     "      AND t.flagtrans = 1 " &
                                     "      AND t.invdate >= DATE_SUB(CURDATE(), INTERVAL 30 DAY) " &
                                     "      AND t.ttlstock > 0 " &
                                     "      AND t.cost > 0 " &
                                     "      AND COALESCE(NULLIF(today.opencost, 0), NULLIF(c.closecost, 0), NULLIF(c.cost, 0), NULLIF(s.price2, 0), s.price1) > 0 " &
                                     "      AND ( " &
                                     "          t.cost >= 1.5 * COALESCE(NULLIF(today.opencost, 0), NULLIF(c.closecost, 0), NULLIF(c.cost, 0), NULLIF(s.price2, 0), s.price1) " &
                                     "          OR " &
                                     "          t.cost <= 0.5 * COALESCE(NULLIF(today.opencost, 0), NULLIF(c.closecost, 0), NULLIF(c.cost, 0), NULLIF(s.price2, 0), s.price1) " &
                                     "      ) " &
                                     "    ORDER BY t.invdate DESC, t.id DESC " &
                                     "    LIMIT 20 " &
                                     ") sub"

            Dim dtMin As DataTable = c_cmd.ReturnDataset(qMinDate).Tables(0)
            If dtMin IsNot Nothing AndAlso dtMin.Rows.Count > 0 AndAlso Not IsDBNull(dtMin.Rows(0)("min_date")) Then
                Dim minDate As Date = Convert.ToDateTime(dtMin.Rows(0)("min_date"))
                Dim cnt As Integer = Convert.ToInt32(dtMin.Rows(0)("cnt"))

                ' 2. Sesuai temuan: looping recalculate harus dimulai dari H-1 tanggal anomali terawal
                Dim calcStartDate As Date = minDate.AddDays(-1)

                Dim clsRecal As New cls_recalculate()
                clsRecal.InvCaclculate_Cost_ActiveTransOnly(calcStartDate, Today.Date, custid())

                ' 3. Perbarui cost resep & master jika tersedia
                Try
                    Dim clsInv_cost As New clsInventory_Receipe()
                    Dim clsinv_itemmaster As New clsInventory_ItemMaster()
                    clsInv_cost.Process_UpdateCostReceipe(custid(), Now.Date.ToString("dd MMM yyyy"))
                    clsinv_itemmaster.Process_UpdateCostitemMaster(custid())
                Catch exRec As Exception
                    ' Non-fatal
                End Try

                ' 4. Gunakan Session FlashMessage + Response.Redirect untuk memicu reload murni (HTTP GET) dengan data terbaru
                Dim sMsg As String = "Successfully processed " & cnt & " cost anomalies! Transaction costs normalized and Moving Average Cost recalculated from D-1 (" & calcStartDate.ToString("dd-MMM-yyyy") & ") to " & Today.ToString("dd-MMM-yyyy") & "."
                Session("FlashMessage") = sMsg
                Response.Redirect("Inventoryhome.aspx?recal=ok", False)
                Context.ApplicationInstance.CompleteRequest()
            Else
                Session("FlashMessage") = "No cost anomalies found to process."
                Response.Redirect("Inventoryhome.aspx", False)
                Context.ApplicationInstance.CompleteRequest()
            End If
        Catch ex As Exception
            mfunction.errLog(ex.Message, "inventoryHome:btnAutoRecalculateAnomalies_Click")
            Session("FlashMessage") = "Failed to process auto-recalculate: " & ex.Message
            Response.Redirect("Inventoryhome.aspx", False)
            Context.ApplicationInstance.CompleteRequest()
        End Try
    End Sub

End Class


