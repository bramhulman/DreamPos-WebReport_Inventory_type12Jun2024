Imports System.IO
Imports NPOI.XSSF.UserModel
Imports NPOI.SS.UserModel
Imports System.Net
Imports ClosedXML.Excel
Public Class Pages_StockOpname
    Inherits System.Web.UI.Page
    Private sNameuser As String
    Private icustid As Integer
    Private imodul As Integer
    Private iModPriv As Integer
    Private irptpriv As Integer
    Private irptf As Integer
    Private mfunction As New ModFunction
    Dim cInv As New clsInventory
    Private clsInventory As New clsInventory_stkOpname
    Public Function modulepriv() As Integer
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        If cookie Is Nothing Then

            Response.Redirect("default.aspx")
        End If
        Return cookie("modulepriv")




    End Function
    Public Function custid() As Integer
        Dim icust As Integer = 0
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")

        If cookie Is Nothing Then
            Response.Redirect("~/Default.aspx")

        Else
            icust = cookie("custid")
        End If
        Return icust
    End Function
    Public Function username() As String
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        If cookie Is Nothing Then
            Response.Redirect("default.aspx")
        End If
        Return cookie("userName")

    End Function
    Public Function rptpriv() As Integer
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        If cookie Is Nothing Then

            Response.Redirect("default.aspx")
        End If
        Return cookie("rptpriv")
        'Return cookie("modul")
        'Return cookie("rptf")


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
            invpriv = inv_priv()
            If (Page.IsPostBack) Then
            Else
                ScriptManager.GetCurrent(Me).RegisterPostBackControl(ddlOutlet)
                'txtDate.Text = Format(CDate(Date.Now), "MM/dd/yyyy")
                loadOutlet()

                lblTitle.Text = "Stock Opname " & Date.Now.Date.ToString("dd MMM yyyy") & ""
                txtDate.Text = Format(Now.AddDays(-1), "dd MMM yyyy")
                binding()
            End If
        Else
            Dim script As String = "window.onload = function(){ alert('"
            script += "Please Login"
            script += "');"
            script += "window.location = '"
            script += "../Default"
            script += " '; }"
            ClientScript.RegisterStartupScript(Me.GetType(), "Redirect", script, True)
        End If
        If (username() = My.Settings.admin Or username() = "support") Then
            btnnew.Visible = True
        End If

    End Sub
    Sub binding()

        Try
            If custid() = 0 Then
                Response.Redirect("~/Default.aspx")
            Else
                Dim ds, ds2 As New DataSet
                'ds = cInv.getInvStkOpname(custid, modulepriv, username, ddlOutlet.SelectedValue)
                ds = cInv.getInvStkOpnameSearch(txtsearch.Text, custid, modulepriv, username, ddlOutlet.SelectedValue, CDate(txtDate.Text))
                If ds IsNot Nothing AndAlso ds.Tables.Count > 0 Then
                    lstViewDetails.DataSource = ds
                Else
                    lstViewDetails.DataSource = Nothing
                End If
                lstViewDetails.DataBind()
                ds2 = cInv.getInvStkOpnameid(custid)
                If ds2.Tables(0).Rows.Count = 0 Then
                    btnnew.Visible = False
                End If
            End If
        Catch ex As Exception

        End Try

    End Sub
    Sub delete(ByVal id As String, ByVal ccid As Integer)
        Try
            DeleteInvStkOpname(id, ccid, custid, username)
            binding()
        Catch ex As Exception

        End Try
    End Sub
    Public Sub DeleteInvStkOpname(ByVal id As String, ByVal ccid As Integer, ByVal Custid As Integer, ByVal username As String)
        'Dim pc As String = pic.ToString
        Try
            'perubahan dari type = 5 menjadi type = 12

            Dim strTr As String
            Dim c_cmd As New Cls_command

            strTr = "DELETE FROM cld_inv_trans where stk_id = " & id & " and cc = " & ccid & " and type = 12 and custid = " & Custid & ""
            c_cmd.executeNonQuery(strTr)
            c_cmd.ReNew()
            c_cmd.executeNonQuery("update cld_inv_transstockcls set  bclose = 0, closeqty = 0, closecost = 0
            where stkid = " & id & " and ccid = " & ccid & " and invdate = '" & Now.Date.ToString("yyyy-MM-dd") & "' and custid = " & Custid & "")

            insertLogModule(username, "Inv_inv_transstock", "Delete", Custid)
        Catch ex As Exception
            errLog(Err.Description, "webservice:Deleteinv_transstock")
            Err.Clear()
        End Try
    End Sub
    Protected Sub lstView_ItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ListViewCommandEventArgs) Handles lstViewDetails.ItemCommand
        '<%--autocomplete="off"--%>
        Try
            Select Case (e.CommandName)
                Case "Del"

                    Dim confirmValue As String = Request.Form("confirm_value")
                    If confirmValue = "Yes" Then
                        Dim arg As String() = e.CommandArgument.ToString().Split(";")

                        Dim id As Integer = arg(0)
                        Dim ccid As Integer = arg(1)
                        delete(id, ccid)

                    Else
                        Exit Sub
                    End If


                Case "Edt"
                    Dim arg As String() = e.CommandArgument.ToString().Split(";")
                    mfunction.CreateCookie("stkid", arg(0))
                    mfunction.CreateCookie("Ccid", arg(1))
                    mfunction.CreateCookie("adjustnow", 0)

                    Response.Redirect("~/Pages_StockOpnameEdit.aspx")

            End Select
        Catch ex As Exception
            errLog(Err.Description, "Pages_Stk:lstView_ItemCommand")
            Err.Clear()
        End Try
    End Sub

    'Protected Sub btnNew_Click(ByVal sender As Object, ByVal e As EventArgs) 'Handles btnnew.Click
    '    Try
    '        Dim modfunc As New ModFunction
    '        modfunc.CreateCookie("InvStk_ID", 0)
    '        Response.Redirect("~/Pages_StkEdit.aspx")

    '        Exit Sub

    '    Catch ex As Exception
    '        errLog(Err.Description, "Pages_Stk:btnNew_Click")
    '        Err.Clear()
    '    End Try
    'End Sub

    'Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.Click

    '    Dim ds As New DataSet
    '    ds = clsOutlet.SelectDataset_OutletSearch(txtsearch.Text, custid, username, modulepriv)
    '    lstViewDetails.DataSource = ds
    '    lstViewDetails.DataBind()
    'End Sub
    Protected Sub btnsearch(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim ds As New DataSet

            ds = cInv.getInvStkOpnameSearch(txtsearch.Text, custid, modulepriv, username, ddlOutlet.SelectedValue, CDate(txtDate.Text))
            'ds = cInv.getInvStkOpname(txtsearch.Text, custid, modulepriv, username, ddlOutlet.SelectedValue)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 Then
                lstViewDetails.DataSource = ds
            Else
                lstViewDetails.DataSource = Nothing
            End If
            lstViewDetails.DataBind()
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub OnPagePropertiesChanging(ByVal sender As Object, ByVal e As PagePropertiesChangingEventArgs)
        lvDataPager1.SetPageProperties(e.StartRowIndex, e.MaximumRows, False)
        binding()
    End Sub
    'Protected Sub btnExportExcel_Click(sender As Object, e As EventArgs) 'Handles btnExportExcel.Click
    '    Try
    '        'CreateCookie("ID", 0)

    '        Response.Redirect("~/Pages_ExportInv_Stk.aspx")
    '    Catch ex As Exception
    '        errLog(Err.Description, "Pages_Stk:btnNew_Click")
    '        Err.Clear()
    '    End Try
    'End Sub

    Protected Sub btnnew_ServerClick(sender As Object, e As EventArgs) Handles btnnew.ServerClick
        Try
            mfunction.CreateCookie("stkid", 0)
            If ddlOutlet.SelectedValue = 0 Then
                mfunction.CreateCookie("Ccid", ddlOutlet.SelectedValue + 1)
            Else
                mfunction.CreateCookie("Ccid", ddlOutlet.SelectedValue)
            End If

            mfunction.CreateCookie("adjustnow", 1)

            Response.Redirect("~/Pages_StockOpnameEdit.aspx", False)
        Catch ex As Exception
            mfunction.errLog(Err.Description, "Pages_StockOpname:btnnew_ServerClick")
            Err.Clear()
        End Try
    End Sub


    Sub loadOutlet()


        Dim ds As New DataSet

        ds = cInv.getInvcc(icustid, username, iModPriv, 0)
        If ds IsNot Nothing AndAlso ds.Tables.Count > 0 Then
            ddlOutlet.DataSource = ds
        Else
            ddlOutlet.DataSource = Nothing
        End If
        ddlOutlet.DataValueField = "Id"
        ddlOutlet.DataTextField = "Name"
        ddlOutlet.DataBind()

    End Sub
    Protected Sub DdlOutlet_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlOutlet.SelectedIndexChanged
        binding()
    End Sub

    Protected Sub btnImport_Click(sender As Object, e As EventArgs) Handles btnImport.ServerClick
        Try
            If ddlOutlet.SelectedValue = 0 Then
                mfunction.CreateCookie("Ccid", ddlOutlet.SelectedValue + 1)
            Else
                mfunction.CreateCookie("Ccid", ddlOutlet.SelectedValue)
            End If
            mfunction.CreateCookie("invdate", CDate(txtDate.Text))
            Response.Redirect("~/Pages_ExprtImprt.aspx", False)
        Catch ex As Exception
            mfunction.errLog(Err.Description, "Pages_StockOpname:btnImport_Click")
            Err.Clear()
        End Try
    End Sub

    Protected Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.ServerClick
        Try

            'Dim dt As DataTable
            Dim ds As New DataSet


            Dim sFile As String = "Stock Opname " & txtDate.Text

            ds = clsInventory.SelectDataset_invStkOpnameExp("", custid, modulepriv, username, ddlOutlet.SelectedValue, CDate(txtDate.Text))

            'Test Export CSV File1
            Dim sb As StringBuilder = New StringBuilder
            If ds.Tables.Count > 0 Then


                Using wb As New XLWorkbook()
                    wb.Worksheets.Add(ds.Tables(0), "Sheet1")
                    mfunction.errLog("get datatable", "export to excel")
                    Response.Clear()
                    Response.Buffer = True
                    Response.Charset = ""
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                    Response.AddHeader("content-disposition", "attachment;filename=" & sFile & ".xlsx")
                    Using MyMemoryStream As New MemoryStream()
                        wb.SaveAs(MyMemoryStream)
                        mfunction.errLog("save as", "export to excel")
                        MyMemoryStream.WriteTo(Response.OutputStream)
                        mfunction.errLog("write", "export to excel")
                        'Response.Flush()
                        'Response.End()

                        HttpContext.Current.Response.Flush()
                        mfunction.errLog("flush", "export to excel")
                        HttpContext.Current.Response.SuppressContent = True
                        mfunction.errLog("SuppressContent", "export to excel")
                        HttpContext.Current.ApplicationInstance.CompleteRequest()
                        mfunction.errLog("CompleteRequest", "export to excel")
                    End Using
                    'dt.Rows.Clear()
                    'dt.Dispose()
                    ds.Tables.Clear()
                    ds.Dispose()

                End Using
                HttpContext.Current.Response.SuppressContent = True
                HttpContext.Current.ApplicationInstance.CompleteRequest()



            End If

        Catch ex As Exception
            mfunction.errLog(Err.Description, "Report:ExportData")
            Err.Clear()
        Finally
            'loadgif.Visible = False
            'HttpContext.Current.Response.SuppressContent = True
            'HttpContext.Current.ApplicationInstance.CompleteRequest()

        End Try
    End Sub

    'Protected Sub txtDate_TextChanged(sender As Object, e As EventArgs) Handles txtDate.TextChanged
    '    binding()
    'End Sub
End Class
