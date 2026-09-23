Imports System.IO
Imports NPOI.XSSF.UserModel
Imports NPOI.SS.UserModel
Imports System.Net
Imports ClosedXML.Excel
Public Class Pages_RtnWH
    Inherits System.Web.UI.Page
    Private mfunction As New ModFunction
    Private clsInv As New clsInventory

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

            If Not Me.IsPostBack Then
                txtDate.Text = Format(Now, "dd MMM yyyy")
                txtDateto.Text = Format(Now, "dd MMM yyyy")
                'txtDate.Text = Format(CDate(Date.Now), "MM/dd/yyyy")
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


    End Sub
    Protected Sub txtDate_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtDate.TextChanged
        If txtDate.Text = "" Then
            txtDate.Text = Format(Now, "dd MMM yyyy")
            txtDateto.Text = Format(Now, "dd MMM yyyy")
        End If
        'GetBackground
        binding()
    End Sub
    Protected Function GetBackground(ByVal xdate As String)

        Dim imgstat As String

        If CDate(xdate) <> Format(CDate(Date.Now), "MM/dd/yyyy") Then
            imgstat = "nodisplay"
        Else
            imgstat = "displayaction"
        End If

        Return imgstat

    End Function
    Sub binding()

        If custid() = 0 Then
            Response.Redirect("~/Default.aspx")
        Else
            Dim ds As New DataSet
            ds = clsInv.getInvRtntoWH(Format(Convert.ToDateTime(txtDate.Text), "MM/dd/yyyy"), Format(Convert.ToDateTime(txtDateto.Text), "MM/dd/yyyy"), custid) 'WService.getInvTrf(Format(Convert.ToDateTime(txtDate.Text), "MM/dd/yyyy"), cust)
            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 Then
                lstViewDetails.DataSource = ds
            Else
                lstViewDetails.DataSource = Nothing
            End If
            lstViewDetails.DataBind()
        End If

    End Sub
    Sub delete(ByVal id As String)
        Dim cInv As New clsInventory
        Dim dt As DataTable = cInv.getDTInvRtntoWH(id, custid)
        clsInv.DeleteInvTrf(id, custid, username)
        If dt.Rows.Count > 0 Then
            'Dim cInv As New clsInventory
            Dim clsprocess As New clsInventoryProcess
            Dim clsicc As New Cls_ICc
            For i As Integer = 0 To dt.Rows.Count - 1
                clsprocess.InvstkId = dt.Rows(i).Item("stk_id").ToString
                clsprocess.InvCc = dt.Rows(i).Item("cc").ToString
                clsprocess.InvCcName = dt.Rows(i).Item("cc_name").ToString
                clsprocess.InvCustId = custid()
                clsprocess.InvstkName = dt.Rows(i).Item("Stk_Name").ToString
                clsprocess.t_in = -(dt.Rows(i).Item("ttlStock").ToString)
                clsprocess.t_out = 0
                clsprocess.InvStkNo = cInv.getStkByid(dt.Rows(i).Item("stk_id").ToString, custid).Tables(0).Rows(0).Item("Number").ToString
                clsprocess.InvDate = Date.Now.Date.ToString("yyyy-MM-dd 00:00:00")
                clsprocess.BCost = True
                'cInv.executeNonQuery_Inv_ProcessCls(Date.Now.Date.ToString("yyyy-MM-dd"), ddlFrom.SelectedValue, ddlFrom.SelectedItem.Text, stkid, stknumber, stkname, custid, 0, stktotal)
                clsprocess.executeNonQuery_Inv_ProcessCls()

                clsprocess.InvstkId = dt.Rows(i).Item("stk_id").ToString
                clsprocess.InvCc = dt.Rows(i).Item("ccfrom").ToString
                clsprocess.InvCcName = clsicc.SelectDataIcc(dt.Rows(i).Item("ccfrom").ToString).Tables(0).Rows(0).Item("name")
                clsprocess.InvCustId = custid()
                clsprocess.InvstkName = dt.Rows(i).Item("Stk_Name").ToString
                clsprocess.t_in = 0
                clsprocess.t_out = -(dt.Rows(i).Item("ttlStock").ToString)
                clsprocess.InvStkNo = cInv.getStkByid(dt.Rows(i).Item("stk_id").ToString, custid).Tables(0).Rows(0).Item("Number").ToString
                clsprocess.InvDate = Date.Now.Date.ToString("yyyy-MM-dd 00:00:00")
                'cInv.executeNonQuery_Inv_ProcessCls(Date.Now.Date.ToString("yyyy-MM-dd"), ddlFrom.SelectedValue, ddlFrom.SelectedItem.Text, stkid, stknumber, stkname, custid, 0, stktotal)
                clsprocess.executeNonQuery_Inv_ProcessCls()
            Next i
        End If
        'clsInv.DeleteInvTrf(id, custid, username)
        binding()
    End Sub
    Public Sub DeleteInvBatch(ByVal id As Integer, ByVal Custid As Integer, ByVal username As String)

        Try

            Dim strTr As String
            Dim c_cmd As New Cls_command

            Dim ds As New DataSet

            Dim a As New clsDbo("Select * FROM cld_inv_transusage  where linkid = " & id & " and type = 11")
            ds = a.selectDataset
            If ds.Tables(0).Rows.Count > 0 Then
                For Each row As DataRow In ds.Tables(0).Rows

                    strTr = "DELETE FROM cld_inv_transusage  where linkid = " & id & " and cc = " & row.Item("cc") & " and stk_id = " & row.Item("stk_id")
                    c_cmd.executeNonQuery(strTr)
                    'c_cmd.executeNonQuery_Inv_CalcCost(row.Item("stk_id"), row.Item("cc"), Custid)
                    'c_cmd.executeNonQuery_Inv_CalcStock(row.Item("stk_id"), row.Item("Cc"), Custid)

                Next
            End If

            a.dispose()
            a = Nothing


            strTr = "DELETE FROM cld_inv_trans  where ID = " & id & " and type = 9 "
            c_cmd.executeNonQuery(strTr)


        Catch ex As Exception
            errLog(Err.Description, "webservice:DeleteInvBatch")
            Err.Clear()
        End Try
    End Sub
    Protected Sub lstView_ItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ListViewCommandEventArgs) Handles lstViewDetails.ItemCommand

        Try
            Select Case (e.CommandName)
                Case "Del"
                    'Dim id As Integer = Convert.ToInt32(e.CommandArgument)

                    'delete(id)

                    Dim confirmValue As String = Request.Form("confirm_value")
                    If confirmValue = "Yes" Then
                        Dim arg As String() = e.CommandArgument.ToString().Split(";")

                        Dim id As String = arg(0)

                        delete(id)

                    Else
                        Exit Sub
                    End If


                Case "Edt"
                    Dim arg As String() = e.CommandArgument.ToString().Split(";")
                    mfunction.CreateCookie("ID", arg(0))
                    mfunction.CreateCookie("CcF", arg(1))


                    Response.Redirect("~/Pages_RtnWHEdit.aspx", False)

            End Select


        Catch ex As Exception
            errLog(Err.Description, "Pages_InvRcv:ItemCommand")
            Err.Clear()
        End Try
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
    Protected Sub btnNew_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnnew.ServerClick
        Try
            mfunction.CreateCookie("ID", "")
            mfunction.CreateCookie("CcF", 0)

            Response.Redirect("~/Pages_RtnWHEdit.aspx")
        Catch ex As Exception
            errLog(Err.Description, "WebMember:btnNew_Click")
            Err.Clear()
        End Try
    End Sub
    Protected Sub OnPagePropertiesChanging(ByVal sender As Object, ByVal e As PagePropertiesChangingEventArgs)
        lvDataPager1.SetPageProperties(e.StartRowIndex, e.MaximumRows, False)
        binding()
    End Sub
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.ServerClick
        Dim ds As New DataSet

        ds = clsInv.getInvRtntoWHSearch(txtsearch.Text, Convert.ToDateTime(txtDate.Text), Convert.ToDateTime(txtDateto.Text), custid)   'WService.getInvTrfSearch(txtSearch.Text, Convert.ToDateTime(txtDate.Text), cust)

        If ds IsNot Nothing AndAlso ds.Tables.Count > 0 Then
            lstViewDetails.DataSource = ds
        Else
            lstViewDetails.DataSource = Nothing
        End If
        lstViewDetails.DataBind()
    End Sub

    Protected Sub btnExportExcel_Click(sender As Object, e As EventArgs) Handles btnExportExcel.ServerClick

        Try

            'Dim dt As DataTable
            Dim ds As New DataSet


            Dim sFile As String = "Return to Warehouse"

            ds = clsInv.getInvRtntoWHExp(txtsearch.Text, Convert.ToDateTime(txtDate.Text), Convert.ToDateTime(txtDateto.Text), custid)

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

        Exit Sub
    End Sub

    Protected Sub btnExportDetail_Click(sender As Object, e As EventArgs) Handles btnExportDetail.ServerClick

        Try

            'Dim dt As DataTable
            Dim ds As New DataSet


            Dim sFile As String = "Detail Return to Warehouse"

            ds = clsInv.getInvRtntoWHDtl(txtsearch.Text, Convert.ToDateTime(txtDate.Text), Convert.ToDateTime(txtDateto.Text), custid)

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

        Exit Sub
    End Sub
End Class
