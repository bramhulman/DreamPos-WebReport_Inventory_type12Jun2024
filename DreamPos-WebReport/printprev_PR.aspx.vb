Imports System.IO
Imports System.Net
Imports System.Net.Mail
Imports System.Text.RegularExpressions
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports iTextSharp.tool.xml
Imports System.Data

Public Class printprev_PR
    Inherits System.Web.UI.Page

    Private mfunction As New ModFunction
    Private clsInv As New clsInventory

    Private xpr_id As Integer       ' purchase request id
    Private xpr_no As String        ' pr number — kosong = new mode
    Private xpr_date As Date        ' pr_date
    Private xrequired_date As Date  ' required_date
    Private xCc As Integer          ' cost center
    Private xrequest_by As String   ' request_by
    Private xstat As String         ' status
    Private xCctoname As String     ' outlet / ccname

    Dim htmlstring As String

    Public Function username() As String
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        Return cookie("userName")
    End Function

    Public Function custid() As Integer
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        If cookie Is Nothing Then Return -1
        Return cookie("Custid")
    End Function

    ' Baca semua cookies PR — dipanggil di setiap request (load & postback)
    Private Sub LoadCookies()
        Try
            xpr_no = Request.Cookies("pr_no")?.Value
            xCctoname = Request.Cookies("ccname")?.Value
            If Request.Cookies("cc") IsNot Nothing Then xCc = CInt(Request.Cookies("cc").Value)
            xstat = Request.Cookies("stat")?.Value
            xrequest_by = Request.Cookies("request_by")?.Value

            ' xpr_id
            Dim prdId As String = Request.Cookies("pr_id")?.Value
            Dim parsedId As Integer = 0
            If Integer.TryParse(prdId, parsedId) Then xpr_id = parsedId

            ' xpr_date
            Dim prdDate As String = Request.Cookies("prdate")?.Value
            If IsDate(prdDate) Then xpr_date = CDate(prdDate)

            ' xrequired_date
            Dim reqd As String = Request.Cookies("requireddate")?.Value
            If IsDate(reqd) Then xrequired_date = CDate(reqd)
        Catch ex As Exception
            errLog(ex.Message, "printprev_PR:LoadCookies")
        End Try
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Session("username") Is Nothing Or custid() < 1 Then
                Session.Clear()
                Session.Abandon()
                Session.RemoveAll()
                FormsAuthentication.RedirectToLoginPage()
                Exit Sub
            End If

            ' Load cookies setiap request agar variabel tersedia saat postback (Export PDF / Send Email)
            LoadCookies()

            If Not Page.IsPostBack Then
                prevhtml()

                ' Pre-fill email form
                txtSubject.Text = "Purchase Request #" & xpr_no
                txtBody.Text = "Yth. HO / Management," & vbCrLf & vbCrLf &
                               "Berikut kami lampirkan Purchase Request dari " & xCctoname & "." & vbCrLf & vbCrLf &
                               "Mohon untuk diproses dan dikonfirmasi." & vbCrLf & vbCrLf &
                               "Terima kasih." & vbCrLf & vbCrLf &
                               "Hormat kami," & vbCrLf &
                               xrequest_by & vbCrLf &
                               xCctoname
            End If

            grid.InnerHtml = ViewState("html_string")

        Catch ex As Exception
            errLog(Err.Description, "printprev_PR:Page_Load")
            Err.Clear()
        End Try
    End Sub

    ' ==============================
    ' Helper: Ganti src image ke base64
    ' ==============================
    Public Function ReplaceImagesWithBase64(html As String) As String
        Dim regex As New Regex("src=[""']([^""']+)[""']", RegexOptions.IgnoreCase)
        Dim matches = regex.Matches(html)
        For Each m As Match In matches
            Dim oldSrc = m.Groups(1).Value
            If oldSrc.StartsWith("http") Or oldSrc.StartsWith("data:") Then Continue For
            Dim physicalPath = Server.MapPath(oldSrc)
            If File.Exists(physicalPath) Then
                Dim bytes = File.ReadAllBytes(physicalPath)
                Dim base64 = Convert.ToBase64String(bytes)
                Dim mime = GetMimeType(physicalPath)
                Dim newSrc = "data:" & mime & ";base64," & base64
                html = html.Replace(oldSrc, newSrc)
            End If
        Next
        Return html
    End Function

    Private Function GetMimeType(path As String) As String
        Dim ext As String = System.IO.Path.GetExtension(path).ToLower()
        Select Case ext
            Case ".png" : Return "image/png"
            Case ".jpg", ".jpeg" : Return "image/jpeg"
            Case ".gif" : Return "image/gif"
            Case ".bmp" : Return "image/bmp"
            Case ".svg" : Return "image/svg+xml"
            Case Else : Return "image/png"
        End Select
    End Function

    ' ==============================
    ' Helper: Generate PDF bytes menggunakan wkhtmltopdf
    ' ==============================
    Private Function GeneratePdfBytes(ByRef fileName As String) As String
        Dim hs As String = CStr(ViewState("html_string"))
        Dim wID_safe As String = xpr_no.Replace("/", "-")
        fileName = "PR-" & wID_safe & "-" & Format(Now, "dd MMM yyyy") & ".pdf"

        Dim baseUrl As String = Request.Url.Scheme & "://" & Request.Url.Authority & ResolveUrl("~/")
        hs = hs.Replace("src=""", "src=""" & baseUrl)
        hs = ReplaceImagesWithBase64(hs)

        If Not Directory.Exists(Server.MapPath("~/temp/")) Then
            Directory.CreateDirectory(Server.MapPath("~/temp/"))
        End If

        Dim tempHtml As String = Server.MapPath("~/temp/") & Guid.NewGuid().ToString() & ".html"
        Dim tempPdf As String = Server.MapPath("~/temp/") & fileName
        File.WriteAllText(tempHtml, "<!DOCTYPE html><html><head><meta charset='utf-8'></head><body>" & hs & "</body></html>")

        Return tempPdf
    End Function

    Private Function RunWkhtmltopdf(tempHtml As String, tempPdf As String) As Integer
        Dim wkhtmlDir As String = Server.MapPath("~/bin/wkhtmltopdf/bin/")
        Dim wkhtmlExe As String = System.IO.Path.Combine(wkhtmlDir, "wkhtmltopdf.exe")

        If Not File.Exists(wkhtmlExe) Then
            Response.Write("Error: wkhtmltopdf.exe tidak ditemukan di " & wkhtmlExe)
            Response.End()
            Return -1
        End If

        Dim args As String = $"--page-size A4 --margin-top 15mm --margin-bottom 15mm --margin-left 10mm --margin-right 10mm --encoding utf-8 --quiet ""{tempHtml}"" ""{tempPdf}"""
        Dim proc As New ProcessStartInfo()
        proc.FileName = wkhtmlExe
        proc.Arguments = args
        proc.UseShellExecute = False
        proc.CreateNoWindow = False
        proc.RedirectStandardOutput = True
        proc.RedirectStandardError = True
        proc.WindowStyle = ProcessWindowStyle.Normal
        proc.WorkingDirectory = System.IO.Path.GetDirectoryName(wkhtmlExe)

        Dim exitCode As Integer = -1
        Using proc1 As Process = Process.Start(proc)
            If proc1 Is Nothing Then Return -1
            proc1.StandardError.ReadToEnd()
            proc1.WaitForExit(120000)
            exitCode = proc1.ExitCode
        End Using
        Return exitCode
    End Function

    ' ==============================
    ' Export PDF
    ' ==============================
    Protected Sub btnpdf_Click(sender As Object, e As System.EventArgs) Handles btnpdf.Click
        Try
            Dim fileName As String = ""
            Dim tempPdf As String = GeneratePdfBytes(fileName)
            Dim tempHtml As String = Server.MapPath("~/temp/") & Guid.NewGuid().ToString() & ".html"

            ' Re-build html file (GeneratePdfBytes only returns path, we need to write again correctly)
            Dim hs As String = CStr(ViewState("html_string"))
            Dim baseUrl As String = Request.Url.Scheme & "://" & Request.Url.Authority & ResolveUrl("~/")
            hs = hs.Replace("src=""", "src=""" & baseUrl)
            hs = ReplaceImagesWithBase64(hs)
            If Not Directory.Exists(Server.MapPath("~/temp/")) Then Directory.CreateDirectory(Server.MapPath("~/temp/"))
            tempHtml = Server.MapPath("~/temp/") & Guid.NewGuid().ToString() & ".html"
            Dim wID_safe As String = xpr_no.Replace("/", "-")
            fileName = "PR-" & wID_safe & "-" & Format(Now, "dd MMM yyyy") & ".pdf"
            tempPdf = Server.MapPath("~/temp/") & fileName
            File.WriteAllText(tempHtml, "<!DOCTYPE html><html><head><meta charset='utf-8'></head><body>" & hs & "</body></html>")

            Dim exitCode As Integer = RunWkhtmltopdf(tempHtml, tempPdf)

            If exitCode = 0 AndAlso File.Exists(tempPdf) AndAlso New FileInfo(tempPdf).Length > 1000 Then
                Response.Clear()
                Response.ContentType = "application/pdf"
                Response.AddHeader("Content-Disposition", "attachment; filename=""" & fileName & """")
                Response.BinaryWrite(File.ReadAllBytes(tempPdf))
                HttpContext.Current.ApplicationInstance.CompleteRequest()
            Else
                Response.Clear()
                Response.Write("<h3>Gagal generate PDF (Exit Code: " & exitCode & ")</h3>")
                HttpContext.Current.ApplicationInstance.CompleteRequest()
            End If

        Catch ex As Exception
            errLog(ex.Message, "printprev_PR:btnpdf_Click")
        Finally
            Try
                ' cleanup di Finally tidak reliable setelah BinaryWrite, skip
            Catch
            End Try
        End Try
    End Sub

    ' ==============================
    ' Attach to Email — generate PDF lalu buka modal
    ' ==============================
    Protected Sub btnemail_Click(sender As Object, e As EventArgs) Handles btnemail.Click
        htmlstring = ViewState("html_string")

        If String.IsNullOrEmpty(htmlstring) Then
            ClientScript.RegisterStartupScript(Me.GetType(), "alert", "alert('HTML kosong!');", True)
            Return
        End If

        Dim hs As String = htmlstring
        Dim baseUrl As String = Request.Url.Scheme & "://" & Request.Url.Authority & ResolveUrl("~/")
        hs = hs.Replace("src=""", "src=""" & baseUrl)
        hs = ReplaceImagesWithBase64(hs)

        If Not Directory.Exists(Server.MapPath("~/temp/")) Then Directory.CreateDirectory(Server.MapPath("~/temp/"))

        Dim wID_safe As String = xpr_no.Replace("/", "-")
        Dim fileName As String = "PR-" & wID_safe & "-" & Format(Now, "dd MMM yyyy") & ".pdf"
        Dim tempHtml As String = Server.MapPath("~/temp/") & Guid.NewGuid().ToString() & ".html"
        Dim tempPdf As String = Server.MapPath("~/temp/") & fileName
        File.WriteAllText(tempHtml, "<!DOCTYPE html><html><head><meta charset='utf-8'></head><body>" & hs & "</body></html>")

        Try
            RunWkhtmltopdf(tempHtml, tempPdf)
        Catch ex As Exception
        End Try

        Session("TempPdfPath_PR") = tempPdf

        ' Update subject dengan nomor PR
        txtSubject.Text = "Purchase Request #" & xpr_no

        ' Buka modal email
        ClientScript.RegisterStartupScript(Me.GetType(), "openModal", "$('#emailModal').modal('show');", True)
    End Sub

    ' ==============================
    ' Kirim Email dengan PDF attachment
    ' ==============================
    Protected Sub btnSendEmail_Click(sender As Object, e As EventArgs) Handles btnSendEmail.Click
        Dim pdfPath As String = TryCast(Session("TempPdfPath_PR"), String)
        If String.IsNullOrEmpty(pdfPath) OrElse Not File.Exists(pdfPath) Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('PDF belum siap. Klik Attach to Email terlebih dahulu!');", True)
            Return
        End If

        Try
            Using mail As New MailMessage()
                mail.From = New MailAddress(My.Settings.emailName)

                ' Support banyak email (pisah dengan koma atau titik koma)
                For Each addr In txtEmailTo.Text.Split({","c, ";"c}, StringSplitOptions.RemoveEmptyEntries)
                    mail.To.Add(addr.Trim())
                Next

                mail.Subject = txtSubject.Text
                mail.Body = txtBody.Text.Replace(vbCrLf, "<br/>")
                mail.IsBodyHtml = True

                ' Attach PDF
                mail.Attachments.Add(New Attachment(pdfPath))

                Using smtp As New SmtpClient()
                    smtp.Host = My.Settings.emailHost
                    smtp.Port = CInt(My.Settings.emailPort)
                    smtp.EnableSsl = True
                    smtp.Credentials = New NetworkCredential(My.Settings.emailName, My.Settings.emailPass)
                    smtp.Send(mail)
                End Using
            End Using

            ClientScript.RegisterStartupScript(Me.GetType(), "success",
                "alert('Email berhasil dikirim!'); $('#emailModal').modal('hide');", True)

        Catch ex As Exception
            ClientScript.RegisterStartupScript(Me.GetType(), "error",
                "alert('Gagal mengirim email: " & ex.Message.Replace("'", "\'") & "');", True)
        End Try
    End Sub

    ' ==============================
    ' Build HTML string untuk preview & PDF
    ' ==============================
    Protected Sub prevhtml()
        Dim sb As StringBuilder = New StringBuilder()
        Dim dt As New DataTable
        Try
            dt = clsInv.getDTInvPR(xpr_no, xCc)

            Dim dtPrint As New DataTable
            dtPrint.Columns.Add("No", GetType(Integer))
            dtPrint.Columns.Add("Item Name", GetType(String))
            dtPrint.Columns.Add("Unit", GetType(String))
            dtPrint.Columns.Add("Req Qty", GetType(Decimal))
            dtPrint.Columns.Add("App Qty", GetType(Decimal))
            dtPrint.Columns.Add("Remarks", GetType(String))

            Dim rowNo As Integer = 1
            For Each row As DataRow In dt.Rows
                Dim newRow As DataRow = dtPrint.NewRow()
                newRow("No") = rowNo
                newRow("Item Name") = If(dt.Columns.Contains("stk_name") AndAlso Not IsDBNull(row("stk_name")), row("stk_name"), "")
                newRow("Unit") = If(dt.Columns.Contains("u2_nm") AndAlso Not IsDBNull(row("u2_nm")), row("u2_nm"), "")

                Dim reqQty As Decimal = 0
                If dt.Columns.Contains("Total") AndAlso Not IsDBNull(row("Total")) Then
                    reqQty = CDec(row("Total"))
                ElseIf dt.Columns.Contains("qty_req") AndAlso Not IsDBNull(row("qty_req")) Then
                    reqQty = CDec(row("qty_req"))
                End If
                newRow("Req Qty") = reqQty

                Dim appQty As Decimal = 0
                If dt.Columns.Contains("app_qty") AndAlso Not IsDBNull(row("app_qty")) Then
                    appQty = CDec(row("app_qty"))
                End If
                newRow("App Qty") = appQty

                newRow("Remarks") = If(dt.Columns.Contains("remarks") AndAlso Not IsDBNull(row("remarks")), row("remarks"), "")
                dtPrint.Rows.Add(newRow)
                rowNo += 1
            Next

            sb.Append("<html>")
            sb.Append("<table width='100%' cellspacing='0' cellpadding='5'>")
            Dim subject As String = "Purchase Request"

            sb.Append("<tr height='20px'><th align='center' style='background-color: #000; border-radius: 0.25em; color: white; font: bold 100% sans-serif; '><strong style='font-size:180%;font-family:sans-serif;'> " & subject & "</strong></th></tr>")
            sb.Append("</table>")

            sb.Append("<table width='100%' cellspacing='0' cellpadding='2'>")
            sb.Append("<tr height='20px'><td colspan = '3'></td></tr>")
            sb.Append("<tr><td width='60%'><strong style='font-size:100%;font-family:sans-serif;'>Outlet:</strong></td><td></td><td></td></tr>")
            sb.Append("<tr><td width='60%' style='font-size:100%;font-family:sans-serif;'>" & xCctoname & "</td>")
            sb.Append("<td width='15%' style='padding: 0.5em; position: relative; text-align: left;'><strong style='font-size:100%;font-family:sans-serif;'>Status:</strong></td>")
            sb.Append("<td width='25%' style='padding: 0.5em; position: relative; text-align: left;'>" & xstat & "</td></tr>")
            sb.Append("<tr height='20px'><td colspan = '3'></td></tr>")

            sb.Append("<tr height='25px'><td width='60%'></td>")
            sb.Append("<td width='15%' style='padding: 0.5em; background-color: #EEEEEE;border-radius: 0.25em; border: 0.5px solid #ccc;font-family:sans-serif;'>PR No</td>")
            sb.Append("<td width='25%' style='padding: 0.5em; border-radius: 0.25em; border: 0.5px solid #ccc;font-family:sans-serif;'>" & xpr_no & "</td></tr>")

            sb.Append("<tr height='25px'><td width='60%'></td>")
            sb.Append("<td width='15%' style='padding: 0.5em; background-color: #EEEEEE;border-radius: 0.25em; border: 0.5px solid #ccc;font-family:sans-serif;'>Req Date</td>")
            sb.Append("<td width='25%' style='padding: 0.5em; border-radius: 0.25em; border: 0.5px solid #ccc;font-family:sans-serif;'>" & Format(xpr_date, "dd MMM yyyy") & "</td></tr>")

            sb.Append("<tr height='25px'><td width='60%'></td>")
            sb.Append("<td width='15%' style='padding: 0.5em; background-color: #EEEEEE;border-radius: 0.25em; border: 0.5px solid #ccc;font-family:sans-serif;'>Required Date</td>")
            sb.Append("<td width='25%' style='padding: 0.5em; border-radius: 0.25em; border: 0.5px solid #ccc;font-family:sans-serif;'>" & Format(xrequired_date, "dd MMM yyyy") & "</td></tr>")
            sb.Append("</table>")

            sb.Append("<table width='100%' cellspacing='0' cellpadding='2'>")
            sb.Append("<tr height='30px'><td></td></tr></table>")

            ' Item grid
            sb.Append("<table width='100%' cellspacing='0' cellpadding='2' style='border-collapse: collapse;border: 1px solid #ccc;'>")
            sb.Append("<tr height='25px'>")
            For Each column As DataColumn In dtPrint.Columns
                sb.Append("<th align='center' style='padding: 0.5em; background-color: #B8DBFD; border: 0.5px solid #ccc;font-family:sans-serif;font-size:100%;'>")
                sb.Append(column.ColumnName)
                sb.Append("</th>")
            Next
            sb.Append("</tr>")

            For Each row As DataRow In dtPrint.Rows
                sb.Append("<tr height='25px'>")
                For Each column As DataColumn In dtPrint.Columns
                    Dim align As String = "center"
                    If column.ColumnName = "Item Name" OrElse column.ColumnName = "Remarks" Then
                        align = "left"
                    End If
                    sb.Append("<td align='" & align & "' style='padding: 0.5em; border: 0.5px solid #ccc;font-family:sans-serif;font-size:100%;'>")
                    sb.Append(row(column))
                    sb.Append("</td>")
                Next
                sb.Append("</tr>")
            Next
            sb.Append("</table>")

            ' Signature area
            sb.Append("<table width='100%' cellspacing='0' cellpadding='2'>")
            sb.Append("<tr height='40px'><td></td></tr>")
            sb.Append("<tr><td width='40%' style='font-size:100%;font-family:sans-serif;position: relative; text-align: center;'>Received by (HO):</td>")
            sb.Append("<td width='40%' style='font-size:100%;font-family:sans-serif; position: relative; text-align: center;'>Prepared by:<br/><br/><br/><br/>" & xrequest_by & "</td></tr>")
            sb.Append("<tr height='160px'><td></td></tr>")
            sb.Append("</table>")

            sb.Append("<table width='100%' cellspacing='0' cellpadding='2'>")
            sb.Append("<tr height='20px'><th align='center' style='font: bold 100% sans-serif;'><strong style='font-size:100%;font-family:sans-serif;letter-spacing: 0.5em;'></strong></th></tr>")
            sb.Append("<tr style='color:white;' height='5px'><td></td></tr>")
            sb.Append("<tr><td align='center'><p style='font-size:100%;font-family:sans-serif;'></p></td></tr>")
            sb.Append("</table>")
            sb.Append("</html>")

            ViewState("html_string") = sb.ToString

        Catch ex As Exception
            errLog(Err.Description, "printprev_PR : prevhtml")
            Err.Clear()
        End Try
        sb = Nothing
        dt.Dispose()
        dt = Nothing
    End Sub

    Protected Sub btnclose_Click(sender As Object, e As System.EventArgs) Handles btnclose.Click
        Response.Redirect("~/PurchaseRequest.aspx")
    End Sub

End Class
