Imports System.Data
Imports System.Drawing
Imports ClosedXML.Excel
Imports System.IO
Imports System.Net
Imports System.Text
Imports System.Threading
Imports System
Imports System.Runtime.CompilerServices

Public Class PreOrder_usage
    Inherits System.Web.UI.Page
    Private clsreport As New ClsReports
    ' Manual declaration of controls since missing designer file
    Protected txtDate As System.Web.UI.WebControls.TextBox

    Protected btnSearch As System.Web.UI.HtmlControls.HtmlButton
    Protected btnBack As System.Web.UI.HtmlControls.HtmlButton
    Dim dsrpt As New DataSet
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            txtFrom.Text = Now.Date.AddDays(-30).ToString("dd MMM yyyy")
            txtTo.Text = Now.Date.ToString("dd MMM yyyy")
            bindusage()
            txtTo.Attributes.Add("readonly", "true")
            txtFrom.Attributes.Add("readonly", "true")
        End If

    End Sub
    Private Sub bindusage()

        dsrpt.Clear()
        Try


            If custid() <= 0 Or username() = "" Then
                FormsAuthentication.RedirectToLoginPage()
                Exit Sub
            End If

            dsrpt = clsreport.cal_usage(CDate(txtFrom.Text), CDate(txtTo.Text), CustID, Username, ModulePriv, 0, "")




            ' setColumName()
            'perubahan 04 jul 2017

            If dsrpt.Tables.Count > 0 Then
                Dim dt As New DataTable
                dt = dsrpt.Tables(0)
                usagepage.Visible = True
                'lblusage.Visible = True
                btnExportUsage.Visible = True
                If dsrpt IsNot Nothing AndAlso dsrpt.Tables.Count > 0 Then
                    dg1.DataSource = dsrpt
                Else
                    dg1.DataSource = Nothing
                End If
                ViewState("vsreport") = dsrpt.Tables(0)
                dg1.DataBind()
                paging()
                'dt.Rows.Clear()
                dt.Dispose()

                dsrpt.Tables.Clear()
                dsrpt.Dispose()



            Else
                usagepage.Visible = False
                'lblusage.Visible = False
                dg1.DataSource = Nothing
                dg1.DataBind()
                paging()
                dsrpt.Tables.Clear()
                dsrpt.Dispose()
            End If
            ScriptManager.RegisterStartupScript(
    Me,
    Me.GetType(),
    "scrollUsage",
    "document.getElementById('usageStock').scrollIntoView({behavior:'smooth'});",
    True
)
        Catch
            mHelper.errLog(Err.Description, "reportInventory:BindData")
            Err.Clear()
            'Finally
            '    dsrpt.Dispose()
            'dsrpt2.Dispose()
        End Try
    End Sub
    Private Sub hidepage()
        btnFirst.Visible = False
        btnPrev.Visible = False
        pagelbl.Visible = False
        btnNext.Visible = False
        btnLast.Visible = False


    End Sub
    Private Sub hidelast()

        btnNext.Visible = False
        btnLast.Visible = False
        btnFirst.Visible = True
        btnPrev.Visible = True
        pagelbl.Visible = True
    End Sub
    Private Sub hidefirst()
        btnFirst.Visible = False
        btnPrev.Visible = False
        btnNext.Visible = True
        btnLast.Visible = True
        pagelbl.Visible = True


    End Sub
    Private Sub btnFirst_Click(sender As Object, e As EventArgs) Handles btnFirst.Click

        If (dg1.CurrentPageIndex > 0) Then
            dg1.CurrentPageIndex = 0
            bindusage()
            hidefirst()
        End If

    End Sub

    Private Sub btnPrev_Click(sender As Object, e As EventArgs) Handles btnPrev.Click

        If (dg1.CurrentPageIndex > 0) Then
            dg1.CurrentPageIndex = dg1.CurrentPageIndex - 1
            bindusage()
            If dg1.CurrentPageIndex = 0 Then
                hidefirst()
            Else
                btnFirst.Visible = True
                btnPrev.Visible = True
                btnNext.Visible = True
                btnLast.Visible = True
                pagelbl.Visible = True
            End If
        End If


    End Sub

    Private Sub btnNext_Click(sender As Object, e As EventArgs) Handles btnNext.Click

        If (dg1.CurrentPageIndex < dg1.PageCount - 1) Then
            dg1.CurrentPageIndex = dg1.CurrentPageIndex + 1
            bindusage()
            If (dg1.CurrentPageIndex = dg1.PageCount - 1) Then
                hidelast()
            Else
                btnFirst.Visible = True
                btnPrev.Visible = True
                btnNext.Visible = True
                btnLast.Visible = True
                pagelbl.Visible = True

            End If
        End If

    End Sub

    Private Sub btnLast_Click(sender As Object, e As EventArgs) Handles btnLast.Click

        If (dg1.CurrentPageIndex < dg1.PageCount - 1) Then
            dg1.CurrentPageIndex = dg1.PageCount - 1
            bindusage()
            hidelast()
        End If

    End Sub
    Private Sub paging()
        pagelbl.Text = " " & dg1.CurrentPageIndex + 1.ToString & " of " & dg1.PageCount.ToString & " "
        If dsrpt.Tables(0).Rows.Count <= 31 Then
            hidepage()
        Else
            hidefirst()
        End If
    End Sub
    Protected Sub dg1_PageIndexChanged(source As Object, e As DataGridPageChangedEventArgs)
        dg1.CurrentPageIndex = e.NewPageIndex
        Dim ds As New DataSet

        Try




            dsrpt = clsreport.cal_usage(CDate(txtDate.Text), CDate(Now().AddDays(20)), CustID, Username, ModulePriv, 0, "")



            ' setColumName()
            'perubahan 04 jul 2017
            If ds.Tables.Count > 0 Then
                dg1.DataSource = ds
                dg1.DataBind()
            Else
                dg1.DataSource = Nothing
                dg1.DataBind()
            End If
        Catch ex As Exception
            mHelper.errLog(Err.Description, "ReportInventory:dg1_PageIndexChanged")
            Err.Clear()
        End Try

        ds.Dispose()
    End Sub
    Protected Sub btnExportUsage_Click(sender As Object, e As EventArgs) Handles btnExportUsage.Click
        Dim dt As New DataTable()
        Try
            ' Pastikan usage sudah di-load (kalau belum, load dulu)
            'If dg1.Items.Count = 0 Then
            '    bindusage()  ' Ini akan load data usage kalau belum
            'End If
            If DirectCast(ViewState("vsreport"), DataTable).Rows.Count = 0 Then
                bindusage()
            Else
                dt = DirectCast(ViewState("vsreport"), DataTable)
            End If
            ' Convert DataGrid ke DataTable

            For Each col As DataGridColumn In dg1.Columns
                dt.Columns.Add(col.HeaderText)
            Next

            For Each item As DataGridItem In dg1.Items
                Dim row As DataRow = dt.NewRow()
                For i As Integer = 0 To dg1.Columns.Count - 1
                    row(i) = item.Cells(i).Text.Replace("&nbsp;", "")
                Next
                dt.Rows.Add(row)
            Next

            Using wb As New XLWorkbook()
                Dim ws As IXLWorksheet = wb.Worksheets.Add(dt, "Usage Stock")

                ' Styling header
                ws.Row(1).Style.Font.Bold = True
                ws.Row(1).Style.Fill.BackgroundColor = XLColor.FromArgb(75, 108, 158)  ' Biru tua
                ws.Row(1).Style.Font.FontColor = XLColor.White

                ' Auto-fit columns
                ws.Columns().AdjustToContents()

                ' Download
                Response.Clear()
                Response.Buffer = True
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                Response.AddHeader("content-disposition", "attachment;filename=PreOrder_Usage_" & Now.ToString("dd MMM yyyy") & ".xlsx")

                Using ms As New MemoryStream()
                    wb.SaveAs(ms)
                    ms.WriteTo(Response.OutputStream)
                    ms.Close()
                End Using
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideLoading", "HideProgress();", True)
                Response.Flush()

                Response.End()
            End Using
        Catch ex As Exception
            notsuccesfull("Gagal export usage: " & ex.Message)
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

    Protected Sub btnFilter_Click(sender As Object, e As EventArgs) Handles btnFilter.Click
        bindusage()
    End Sub
End Class
