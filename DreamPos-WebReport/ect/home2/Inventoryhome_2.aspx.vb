Imports Microsoft.AspNet.Identity
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

Public Class Inventoryhome_2
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
    Dim clslstkgroup As New Cls_IStkGroup
    Dim invpriv As Integer
    Dim cinvProcess As New clsInventoryProcess
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

        'If Not (User.Identity.IsAuthenticated) Or custid() = -1 Then
        '    FormsAuthentication.RedirectToLoginPage()
        '    Exit Sub
        'End If
        ' Dim dmaxcls As Date
        Dim bprocessUsageData As Boolean = False

        If Session("username") Is Nothing Or custid() < 1 Then
            Session.Clear()
            Session.Abandon()
            Session.RemoveAll()
            FormsAuthentication.RedirectToLoginPage()
            Exit Sub
        End If
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
                    ScriptManager.GetCurrent(Me).RegisterPostBackControl(ddlOutlet)
                    ScriptManager.GetCurrent(Me).RegisterPostBackControl(ddlGrp)
                    ScriptManager.GetCurrent(Me).RegisterPostBackControl(RadButton3)


                    Dim c_cmd As New Cls_command

                    'cekclosing:
                    If icustid = 0 Or (c_cmd.ExecuteScalar("SELECT COUNT(DISTINCT custid) FROM cld_inv_closing", 0) > 1) Then
                        icustid = c_cmd.ExecuteScalar("SELECT id FROM `cld_cust` LIMIT 1", 0)
                    End If
                    If c_cmd.ExecuteScalar("select count(custid) from cld_inv_closing where custid = " & icustid & " and invdate = '" & Date.Now.Date.ToString("yyyy-MM-dd") & "' and  invflag = 2 ", 0) >= 1 Then
                        bprocessUsageData = True
                    End If

                    If c_cmd.ExecuteScalar("select count(custid) from cld_inv_closing where custid = " & icustid & " and invdate = '" & Date.Now.Date.ToString("yyyy-MM-dd") & "' ", 0) = 0 Then

                        'Dim script As String = "$(document).ready(function () { $('[id*=btnshowmdlprocess]').click(); });"
                        'ClientScript.RegisterStartupScript(Me.GetType, "load", script, True)
                        'langsung diproses

                        'dinonaktifkan closing terpisah
                        'alert1.Text = "Please wait.. Calculating Stock"
                        'alert1.Visible = True
                        'c_cmd.ReNew()
                        'c_cmd.executeNonQuery("insert into cld_inv_closing values ('" & Date.Now.Date.ToString("yyyy-MM-dd") & "', " & icustid & ",0,'" & Date.Now.ToString("yyyy-MM-dd HH:mm:ss") & "')")

                        'clsInv.InvIn(custid(), username, iModPriv)
                        'binding()
                        'alert1.Text = ""
                        'alert1.Visible = False

                        'dinonaktifkan closing terpisah

                        Session.Clear()
                        Session.Abandon()
                        Session.RemoveAll()

                        showmessage("Closing process,Please login again.")
                        'messagesuccesfull("Closing process,Please login again.", True)
                        'FormsAuthentication.RedirectToLoginPage()

                        Exit Sub
                    ElseIf c_cmd.ExecuteScalar("select count(custid) from cld_inv_closing where custid = " & icustid & " and invdate = '" & Date.Now.Date.ToString("yyyy-MM-dd") & "' and  invflag = 0 ", 0) = 1 Then
                        'dinonaktifkan closing terpisah
                        'If c_cmd.ExecuteScalar("SELECT TIMESTAMPDIFF(MINUTE,createdate,CURDATE()) FROM cld_inv_closing WHERE custid = " & icustid & " and invdate = '" & Date.Now.Date.ToString("yyyy-MM-dd") & "' and  invflag = 0 ", 0) < 30 Then
                        '    c_cmd.ReNew()
                        '    c_cmd.executeNonQuery("DELETE FROM cld_inv_transstockcls WHERE custid = " & icustid & " and invdate = '" & Date.Now.Date.ToString("yyyy-MM-dd") & "'")
                        '    c_cmd.executeNonQuery("DELETE FROM cld_inv_closing WHERE custid = " & icustid & " and invdate = '" & Date.Now.Date.ToString("yyyy-MM-dd") & "' and  invflag = 0")
                        'End If
                        'dinonaktifkan closing terpisah
                        'FormsAuthentication.SetAuthCookie(username, False)
                        Session.Clear()
                        Session.Abandon()
                        Session.RemoveAll()

                        showmessage("Closing process not complete yet,Please login again.")

                        Exit Sub

                    End If

                    'tambahan Bram 5Okt 2023 tambah cost di cld_inv_stk_cc
                    cinvProcess.incost(custid)

                    loadOutlet()
                    loadGroup()

                    lblTitle.Text = "Inventory Stock " & Date.Now.Date.ToString("dd MMM yyyy") & ""



                End If
                'txtsearch.Attributes.Add("onkeyup", Me.Page.ClientScript.GetPostBackEventReference(txtsearch, ""))

                txtsearch.Attributes.Add("onfocus", "javascript: this.setSelectionRange(this.value.length,this.value.length);")
                'untuk process usage dari transaksi
                'clsInv.proc_invusage(icustid)
                Dim c_dbo As New clsDbo("")
                'Dim ds1, ds2 As New DataSet
                'ds1 = clsInv.Process_InvUsage2(Now.AddDays(-1), custid)
                'If ds1.Tables(0).Rows.Count > 0 Then
                '    For Each row As DataRow In ds1.Tables(0).Rows
                '        c_dbo.executeNonQuery_Inv_ProcessClsbyid(Now.AddDays(-1), Now.AddDays(-1), custid, row.Item("cc"), row.Item("stk_id"))
                '    Next
                'End If
                'ds2 = clsInv.Process_InvUsage2(CDate(Now), custid)
                ''If ds2.Tables(0).Rows.Count > 0 Then
                ''    For Each row As DataRow In ds2.Tables(0).Rows
                ''        c_dbo.executeNonQuery_Inv_ProcessClsbyid(CDate(Now), CDate(Now), custid, row.Item("cc"), row.Item("stk_id"))
                ''    Next
                ''End If
                'ds1.Dispose()
                'ds2.Dispose()

                'matiin fungsi bram dlu

                If bprocessUsageData = False Then

                    'If c_dbo.ExecuteScalar("select count(custid) from cld_inv_closing where custid = " & custid() & " and invdate = '" & Date.Now.Date.ToString("yyyy-MM-dd 00:00:00") & "' and  invflag <> 2 ", 0) = 1 Then
                    c_dbo.executeNonQuery("update cld_inv_closing set invflag = 2 where invdate = '" & Date.Now.Date.ToString("yyyy-MM-dd 00:00:00") & "'")

                    clsInv.Process_InvUsage(Now.AddDays(-2), custid)
                    clsInv.Process_InvUsage(Now.AddDays(-1), custid)
                    clsInv.Process_InvUsage(CDate(Now), custid)

                    c_dbo.executeNonQuery("update cld_inv_closing set invflag = 1 where invdate = '" & Date.Now.Date.ToString("yyyy-MM-dd 00:00:00") & "'")
                    'End If
                End If

                Dim dsusage As New DataSet
                dsusage = clsInv.lastdate_InvUsage(custid)


                Dim xheight As Integer = 0
                If dsusage.Tables(0).Rows.Count > 0 Then
                    For Each row As DataRow In dsusage.Tables(0).Rows
                        Dim ldate As String = row.Item("ldate").ToString '("dd MMM yyyy")
                        'ldate
                        alert1.Text = "( Need recalculate from " & ldate & "! )"

                        If getlatepost() <> "" Then
                            alert1.Text = ("( Need recalculate from " & ldate & "! )" + "\n" + getlatepost.ToString).Replace("\n", Environment.NewLine)
                            xheight = (("( Need recalculate from " & ldate & "! )" + "\n" + getlatepost.ToString).Split("\n").Length - 1) * 30
                        End If

                        alert1.Attributes.Add("style", "height:" & xheight & "px" & ";Z-INDEX: 101; OVERFLOW: hidden; color:red; ")
                        alert1.ReadOnly = True
                        alert1.Visible = True
                        'messagePilihSatu("please recalculate from " & ldate & "!")

                        Exit For
                    Next

                Else

                    Dim msg As String = ""
                    msg = getlatepost.ToString
                    If msg <> "" Then
                        'messagePilihSatu(msg & "!")
                        'getlatepost.Replace("\n", Environment.NewLine)
                        alert1.Text = getlatepost.ToString.Replace("\n", Environment.NewLine)
                        xheight = ((getlatepost.ToString).Split("\n").Length - 1) * 30
                        alert1.Attributes.Add("style", "height:" & xheight & "px" & ";Z-INDEX: 101; OVERFLOW: hidden; color:red; ")

                        'alert1.Attributes.Add("style", "height:" & (alert1.Text.Split("\n").Length - 1) * 10 & ";")
                        alert1.ReadOnly = True

                        alert1.Visible = True
                    Else
                        alert1.Visible = False
                        alert1.Attributes.Add("style", "display:none; ")
                    End If
                End If

                binding()
                insert_closing()
                'Dim msg As String = ""
                'msg = getlatepost.ToString
                'If msg <> "" Then
                '    messagePilihSatu(msg & "!")
                'End If

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
        'Dim c_cmd As New Cls_command

        'openlabel.Text = "Please wait.."
        'clsInv.InvIn(icustid, username, iModPriv)
        '    binding()


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

    Sub loadOutlet()


        Dim ds As New DataSet
        Try
            ds = clsInv.getInvcc(icustid, username, iModPriv, 0)
            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 Then
                ddlOutlet.DataSource = ds
            Else
                ddlOutlet.DataSource = Nothing
            End If
            ddlOutlet.DataValueField = "Id"
            ddlOutlet.DataTextField = "Name"
            ddlOutlet.DataBind()
        Catch ex As Exception
            mfunction.errLog(Err.Description, "InventoryHome:loadOutlet")
            Err.Clear()
        End Try


    End Sub
    Sub loadGroup()


        Dim ds As New DataSet
        Try
            ds = clslstkgroup.SelectDataset_StkGroup(icustid)
            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 Then
                ddlGrp.DataSource = ds
            Else
                ddlGrp.DataSource = Nothing
            End If
            ddlGrp.DataValueField = "Id"
            ddlGrp.DataTextField = "Name"
            ddlGrp.DataBind()
        Catch ex As Exception
            mfunction.errLog(Err.Description, "InventoryHome:loadGroup")
            Err.Clear()
        End Try


    End Sub
    Protected Sub DdlOutlet_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlOutlet.SelectedIndexChanged
        binding()
    End Sub
    Protected Sub DdlStatus_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles DdlStatus.SelectedIndexChanged
        binding()
    End Sub
    Protected Sub DdlGrp_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlGrp.SelectedIndexChanged
        binding()
    End Sub
    Sub binding()

        Dim ds As New DataSet
        Try
            ds = clsInv.SelectDataStkHomeSearch2(icustid, ddlOutlet.SelectedValue, DdlStatus.SelectedValue, ddlGrp.SelectedValue, username, iModPriv)

            If ds.Tables(0).Rows.Count > 0 Then
                ViewState("homeStk") = ds.Tables(0)
                If ds IsNot Nothing AndAlso ds.Tables.Count > 0 Then
                    lstViewDetails.DataSource = ds
                Else
                    lstViewDetails.DataSource = Nothing
                End If
                lstViewDetails.DataBind()
            Else

                lstViewDetails.DataSource = Nothing
                lstViewDetails.DataBind()
            End If
        Catch ex As Exception
            mfunction.errLog(Err.Description, "InventoryHome:binding")
            Err.Clear()
        End Try


    End Sub
    Private Sub bindData2()
        Try
            If ViewState("homeStk") IsNot Nothing And DirectCast(ViewState("homeStk"), DataTable).Rows.Count > 0 Then
                'lstViewDetails.DataSource = DirectCast(ViewState("homeStk"), DataTable)
                'lstViewDetails.DataBind()
                Dim dt As New DataTable
                dt = ViewState("homeStk")
                dt.DefaultView.RowFilter = "stkname like '%" & txtsearch.Text & "%'"


                lstViewDetails.DataSource = dt.DefaultView
                lstViewDetails.DataBind()
            Else
                binding()
            End If
        Catch ex As Exception
            mfunction.errLog(Err.Description, "InventoryHome:bindData2")
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

        Dim newLine As String = "\n"
        Dim dtpost As New DataSet
        Dim c_cmd As New Cls_command
        Try
            dtpost = c_cmd.ReturnDataset("select * from
(SELECT (SELECT nama FROM cld_outlet WHERE id = cld_last.`storeid`) outlet,max(postdate) latepost FROM `cld_last`
where storeid in (select id from cld_outlet where active = 1 and custid = " & icustid & ")
group by cld_last.`storeid`) lastpost
where HOUR(latepost)-HOUR(NOW())<-8 or date(latepost)-date(NOW())<-1")
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

    Protected Sub lstView_ItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ListViewCommandEventArgs) Handles lstViewDetails.ItemCommand
        '<%--autocomplete="off"--%>
        Select Case (e.CommandName)
            'Case "Del"

            '    Dim confirmValue As String = Request.Form("confirm_value")
            '    If confirmValue = "Yes" Then

            '        Dim id As Integer = Convert.ToInt32(e.CommandArgument)

            '        delete(id)

            '    Else
            '        Exit Sub
            '    End If

            Case "Edt"
                ID = Convert.ToInt32(e.CommandArgument)

                'editlist(ID)
                mfunction.CreateCookie("Outlet_edt_ID", e.CommandArgument)

                'CreateCookie("Name", getStoreID())
                Response.Redirect("outlet_Edit")



        End Select
    End Sub
    Protected Sub OnPagePropertiesChanging(ByVal sender As Object, ByVal e As PagePropertiesChangingEventArgs)
        lvDataPager1.SetPageProperties(e.StartRowIndex, e.MaximumRows, False)
        bindData2()
        'binding()
    End Sub
    Protected Sub btnsearch(ByVal sender As Object, ByVal e As EventArgs)
        Try

            Dim dt As New DataTable
            dt = ViewState("homeStk")
            dt.DefaultView.RowFilter = "stkname like '%" & txtsearch.Text & "%'"


            lstViewDetails.DataSource = dt.DefaultView
            lstViewDetails.DataBind()
        Catch ex As Exception
            mfunction.errLog(Err.Description, "InventoryHome:btnsearch")
            Err.Clear()
        End Try
    End Sub
    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtsearch.TextChanged
        Threading.Thread.Sleep(500)
        Dim dt As New DataTable
        Try
            dt = ViewState("homeStk")
            dt.DefaultView.RowFilter = "stkname like '%" & txtsearch.Text & "%'"


            lstViewDetails.DataSource = dt.DefaultView
            lstViewDetails.DataBind()
        Catch ex As Exception
            mfunction.errLog(Err.Description, "InventoryHome:txtSearch_TextChanged")
            Err.Clear()
        End Try

    End Sub
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
            dtpost = c_cmd.ReturnDataset("SELECT invdate,ccid,ccname,stkid,stkname,ttlqty FROM `cld_inv_transstockcls` WHERE invdate = '" & Date.Now.AddDays(-1).Date.ToString("yyyy-MM-dd 00:00:00") & "' 
                                AND (ccid,stkid)NOT IN(SELECT ccid,stkid FROM `cld_inv_transstockcls` WHERE invdate = '" & Date.Now.Date.ToString("yyyy-MM-dd 00:00:00") & "')")
            If dtpost.Tables(0).Rows.Count > 0 Then

                c_cmd.executeNonQuery("INSERT INTO `cld_inv_transstockcls`
                                        SELECT ADDDATE(invdate,1),custid,stkid,stkno,stkname,ccid,ccname,cost,ttlqty,cost,0,0,0,0,0,0,0,0,bclose,0,0,0,0,0,0,0,0 
                                        FROM `cld_inv_transstockcls` WHERE invdate = '" & Date.Now.AddDays(-1).Date.ToString("yyyy-MM-dd 00:00:00") & "'  
                                        AND (ccid,stkid)NOT IN(SELECT ccid,stkid FROM `cld_inv_transstockcls` WHERE invdate = '" & Date.Now.Date.ToString("yyyy-MM-dd 00:00:00") & "')")

            End If
        Catch ex As Exception
            mfunction.errLog(Err.Description, "InventoryHome:insert_closing")
            Err.Clear()
        End Try

    End Sub

End Class
