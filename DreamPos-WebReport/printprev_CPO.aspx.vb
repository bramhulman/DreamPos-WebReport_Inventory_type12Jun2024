Imports System.IO
Imports System.Net
Imports System.Net.Mail
Imports System.Threading.Tasks
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports iTextSharp.tool.xml
Imports System.Text.RegularExpressions
Imports DocumentFormat.OpenXml.Drawing
Imports iTextSharp.xmp.impl.xpath
Public Class printprev_CPO
    Inherits System.Web.UI.Page
    Private Mtid As String
    Private olid As Integer 'ccname
    Private Customername As String
    Private olname As String
    Private Phone As String
    Private Inv_id As String 'nomor inv
    Private doctot As String
    Private rcvstatus As String

    Private Create_date As String
    Private Busidate As String
    Private TransDate As String
    Private Creatednm As String
    'Private punit1 As Integer = 0

    Private wNotes As String

    Private clsPO As New cls_PO
    'Private clsStk As New clsInventory_stk
    Dim htmlstring As String
    Private pdfBytes As Byte() = Nothing
    Public Function username() As String
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        Return cookie("userName")

    End Function
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
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim dt As New DataTable
        Try
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
            'Mtid = Request.Cookies("rcvID").Value
            ''If Request.Cookies("xunit1").Value IsNot Nothing Then
            ''    punit1 = Request.Cookies("xunit1").Value
            ''    Response.Cookies("xunit1").Expires = Now.AddDays(-1)
            ''End If
            ''Response.Cookies("rcvID").Expires = Now.AddDays(-1)

            If (Page.IsPostBack) Then
            Else

                If Request.Cookies("CPOID") IsNot Nothing Then
                    Mtid = Request.Cookies("CPOID").Value
                    olid = Request.Cookies("olid").Value
                Else
                    ' Kalau cookie hilang, ambil dari Session atau QueryString
                    Mtid = Session("CPOID")?.ToString()
                    olid = Session("olid")?.ToString()
                    If String.IsNullOrEmpty(Mtid) Then
                        Response.Redirect("~/Pages_CPO.aspx", False)
                        Exit Sub
                    End If
                End If
                'If Request.Cookies("xunit1").Value IsNot Nothing Then
                '    punit1 = Request.Cookies("xunit1").Value

                'End If
                'Response.Cookies("rcvID").Expires = Now.AddDays(-1)
                dt = clsPO.Selectdt_CPO_grid(Mtid, custid, olid)
                If dt.Rows.Count > 0 Then
                    If (Not String.IsNullOrEmpty(dt.Rows(0).Item("olname").ToString)) Then
                        olname = dt.Rows(0).Item("olname")
                    Else
                        olname = ""

                    End If
                    If (Not String.IsNullOrEmpty(dt.Rows(0).Item("custname").ToString)) Then
                        Customername = dt.Rows(0).Item("custname")

                    Else
                        Customername = ""

                    End If
                    If (Not String.IsNullOrEmpty(dt.Rows(0).Item("phone").ToString)) Then

                        Phone = dt.Rows(0).Item("phone")
                    Else
                        Phone = ""

                    End If
                    'If (Not String.IsNullOrEmpty(dt.Rows(0).Item("invoiceid").ToString)) Then

                    '    Inv_id = dt.Rows(0).Item("invoiceid")
                    'Else
                    '    Inv_id = ""

                    'End If
                    'If (Not String.IsNullOrEmpty(dt.Rows(0).Item("status").ToString)) Then
                    '    rcvstatus = dt.Rows(0).Item("status")
                    'Else
                    '    rcvstatus = ""
                    'End If
                    'If (Not String.IsNullOrEmpty(dt.Rows(0).Item("Notes").ToString)) Then
                    '    wNotes = dt.Rows(0).Item("Notes")
                    'Else
                    '    wNotes = ""

                    'End If
                    If (Not String.IsNullOrEmpty(dt.Rows(0).Item("TransDate").ToString)) Then
                        TransDate = Format(dt.Rows(0).Item("TransDate"), "dd MMM yyyy")
                    Else
                        TransDate = ""

                    End If
                    If (Not String.IsNullOrEmpty(dt.Rows(0).Item("Busidate").ToString)) Then
                        Busidate = Format(dt.Rows(0).Item("Busidate"), "dd MMM yyyy")
                    Else
                        Busidate = ""

                    End If
                    If (Not String.IsNullOrEmpty(dt.Rows(0).Item("Busidate").ToString)) Then
                        Busidate = Format(dt.Rows(0).Item("Busidate"), "dd MMM yyyy")
                    Else
                        Busidate = ""

                    End If
                    'If (dt.Rows(0).Item("DocTot")) > 0 Then
                    '    doctot = Format(dt.Rows(0).Item("DocTot"), "#,###.00")
                    'Else
                    '    doctot = ""

                    'End If
                    prevhtml()
                    ' Ambil data dari Session / QueryString
                    'htmlstring = Session("html_string")?.ToString()
                    'Mtid = Session("Mtid")?.ToString()


                    txtSubject.Text = "Pre Order #" & Mtid
                    txtBody.Text = "Yth. Bapak/Ibu," & vbCrLf & vbCrLf &
                                   "Berikut kami lampirkan Pre Order resmi." & vbCrLf & vbCrLf &
                                   "Mohon disimpan." & vbCrLf & vbCrLf &
                                   "Terima kasih."
                End If
            End If
            'Mtid = Request.Cookies("rcvID").Value
            grid.InnerHtml = ViewState("html_string")

        Catch ex As Exception
            errLog(Err.Description, "printprev:Page_Load")
            Err.Clear()
        End Try
    End Sub
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
            Case ".png"
                Return "image/png"
            Case ".jpg", ".jpeg"
                Return "image/jpeg"
            Case ".gif"
                Return "image/gif"
            Case ".bmp"
                Return "image/bmp"
            Case ".svg"
                Return "image/svg+xml"
            Case Else
                Return "image/png"
        End Select
    End Function
    Protected Sub btnpdf_Click(sender As Object, e As System.EventArgs) Handles btnpdf.Click
        Dim htmlstring As String = CStr(ViewState("html_string"))
        Dim Mtid As String = Request.Cookies("CPOID").Value.Replace("/", "-")
        Dim fileName As String = Mtid & "-" & Format(Now, "dd MMM yyyy") & ".pdf"

        '=== PASTIKAN LOGO MUNCUL: Ganti src relatif jadi absolut atau base64 ===
        Dim baseUrl As String = Request.Url.Scheme & "://" & Request.Url.Authority & ResolveUrl("~/")
        htmlstring = htmlstring.Replace("src=""", "src=""" & baseUrl)
        'Atau kalau mau lebih aman pakai base64 (lihat fungsi di bawah)

        '=== Buat file HTML sementara ===
        Dim tempHtml As String = Server.MapPath("~/temp/") & Guid.NewGuid().ToString() & ".html"
        Dim tempPdf As String = Server.MapPath("~/temp/") & fileName

        If Not Directory.Exists(Server.MapPath("~/temp/")) Then
            Directory.CreateDirectory(Server.MapPath("~/temp/"))
        End If
        htmlstring = ReplaceImagesWithBase64(htmlstring)
        File.WriteAllText(tempHtml, "<!DOCTYPE html><html><head><meta charset='utf-8'></head><body>" & htmlstring & "</body></html>")

        '=== Jalankan wkhtmltopdf ===
        'Dim wkhtmlPath As String = Server.MapPath("~/bin/wkhtmltopdf/bin/wkhtmltopdf.exe")
        Dim wkhtmlDir As String = Server.MapPath("~/bin/wkhtmltopdf/bin/")
        Dim wkhtmlExe As String = System.IO.Path.Combine(wkhtmlDir, "wkhtmltopdf.exe")

        If Not File.Exists(wkhtmlExe) Then
            Response.Write("Error: wkhtmltopdf.exe tidak ditemukan di " & wkhtmlExe)
            Response.End()
        End If
        Dim args As String = $"--page-size A4 --margin-top 15mm --margin-bottom 15mm --margin-left 10mm --margin-right 10mm --encoding utf-8 --quiet ""{tempHtml}"" ""{tempPdf}"""
        Dim proc As New ProcessStartInfo()
        proc.FileName = wkhtmlExe 'wkhtmlPath
        proc.Arguments = args
        proc.UseShellExecute = False
        proc.CreateNoWindow = True
        proc.RedirectStandardOutput = True
        proc.RedirectStandardError = True
        proc.CreateNoWindow = False
        proc.WindowStyle = ProcessWindowStyle.Normal
        'proc.WorkingDirectory = wkhtmlDir
        proc.WorkingDirectory = System.IO.Path.GetDirectoryName(wkhtmlExe)


        Dim exitCode As Integer = -1
        Dim stdErr As String = String.Empty
        Try
            Using proc1 As Process = Process.Start(proc)
                If proc1 Is Nothing Then
                    Response.Write("Error: Gagal menjalankan wkhtmltopdf.")
                    Response.End()
                    Return
                End If


                stdErr = proc1.StandardError.ReadToEnd()
                proc1.WaitForExit(120000) ' timeout 2 menit
                exitCode = proc1.ExitCode
            End Using


            If exitCode = 0 AndAlso File.Exists(tempPdf) AndAlso New FileInfo(tempPdf).Length > 1000 Then
                Response.Clear()
                Response.ContentType = "application/pdf"
                Response.AddHeader("Content-Disposition", "attachment; filename=""" & fileName & """")
                Response.BinaryWrite(File.ReadAllBytes(tempPdf))
                HttpContext.Current.ApplicationInstance.CompleteRequest()
            Else
                Response.Clear()
                Response.Write("<h3>Gagal generate PDF (Exit Code: " & exitCode & ")</h3>")
                Response.Write("<pre>" & Server.HtmlEncode(stdErr) & "</pre>")
                Response.Write("<p>HTML: <a href='../temp/" & System.IO.Path.GetFileName(tempHtml) & "' target='_blank'>Buka file HTML</a></p>")
                HttpContext.Current.ApplicationInstance.CompleteRequest()
            End If


        Catch ex As Exception
            Response.Write("Exception saat generate PDF: " & Server.HtmlEncode(ex.Message))
        Finally
            ' Cleanup temp files but don't remove until response sent
            Try
                If File.Exists(tempHtml) Then File.Delete(tempHtml)
                If File.Exists(tempPdf) Then File.Delete(tempPdf)
            Catch
            End Try
        End Try
    End Sub


    Protected Sub btnemail_Click(sender As Object, e As EventArgs) Handles btnemail.Click
        htmlstring = ViewState("html_string")

        If String.IsNullOrEmpty(htmlstring) Then
            ClientScript.RegisterStartupScript(Me.GetType(), "alert", "alert('HTML kosong!');", True)
            Return
        End If
        'Dim htmlstring As String = CStr(ViewState("html_string"))
        Dim Mtid As String = Request.Cookies("CPOID").Value.Replace("/", "-")
        Dim fileName As String = Mtid & "-" & Format(Now, "dd MMM yyyy") & ".pdf"

        '=== PASTIKAN LOGO MUNCUL: Ganti src relatif jadi absolut atau base64 ===
        Dim baseUrl As String = Request.Url.Scheme & "://" & Request.Url.Authority & ResolveUrl("~/")
        htmlstring = htmlstring.Replace("src=""", "src=""" & baseUrl)
        'Atau kalau mau lebih aman pakai base64 (lihat fungsi di bawah)

        '=== Buat file HTML sementara ===
        Dim tempHtml As String = Server.MapPath("~/temp/") & Guid.NewGuid().ToString() & ".html"
        Dim tempPdf As String = Server.MapPath("~/temp/") & fileName

        If Not Directory.Exists(Server.MapPath("~/temp/")) Then
            Directory.CreateDirectory(Server.MapPath("~/temp/"))
        End If
        htmlstring = ReplaceImagesWithBase64(htmlstring)
        File.WriteAllText(tempHtml, "<!DOCTYPE html><html><head><meta charset='utf-8'></head><body>" & htmlstring & "</body></html>")

        '=== Jalankan wkhtmltopdf ===
        'Dim wkhtmlPath As String = Server.MapPath("~/bin/wkhtmltopdf/bin/wkhtmltopdf.exe")
        Dim wkhtmlDir As String = Server.MapPath("~/bin/wkhtmltopdf/bin/")
        Dim wkhtmlExe As String = System.IO.Path.Combine(wkhtmlDir, "wkhtmltopdf.exe")

        If Not File.Exists(wkhtmlExe) Then
            Response.Write("Error: wkhtmltopdf.exe tidak ditemukan di " & wkhtmlExe)
            Response.End()
        End If
        Dim args As String = $"--page-size A4 --margin-top 15mm --margin-bottom 15mm --margin-left 10mm --margin-right 10mm --encoding utf-8 --quiet ""{tempHtml}"" ""{tempPdf}"""
        Dim proc As New ProcessStartInfo()
        proc.FileName = wkhtmlExe 'wkhtmlPath
        proc.Arguments = args
        proc.UseShellExecute = False
        proc.CreateNoWindow = True
        proc.RedirectStandardOutput = True
        proc.RedirectStandardError = True
        proc.CreateNoWindow = False
        proc.WindowStyle = ProcessWindowStyle.Normal
        'proc.WorkingDirectory = wkhtmlDir
        proc.WorkingDirectory = System.IO.Path.GetDirectoryName(wkhtmlExe)


        Dim exitCode As Integer = -1
        Dim stdErr As String = String.Empty
        Try
            Using proc1 As Process = Process.Start(proc)
                If proc1 Is Nothing Then
                    Response.Write("Error: Gagal menjalankan wkhtmltopdf.")
                    Response.End()
                    Return
                End If


                stdErr = proc1.StandardError.ReadToEnd()
                proc1.WaitForExit(120000) ' timeout 2 menit
                exitCode = proc1.ExitCode
            End Using
        Catch ex As Exception

        End Try

        Session("TempPdfPath") = tempPdf
        ' Otomatis isi subject dengan nomor PO yang benar
        txtSubject.Text = "Pre Order #" & Mtid

        ' Buka modal
        ClientScript.RegisterStartupScript(Me.GetType(), "openModal", "$('#emailModal').modal('show');", True)
    End Sub

    Protected Sub btnSendEmail_Click(sender As Object, e As EventArgs)
        Dim pdfPath As String = TryCast(Session("TempPdfPath"), String)
        If String.IsNullOrEmpty(pdfPath) OrElse Not File.Exists(pdfPath) Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('PDF belum siap!');", True)
            Return
        End If

        Try
            Using mail As New MailMessage()
                mail.From = New MailAddress(My.Settings.emailName)

                ' Support banyak email
                For Each addr In txtEmailTo.Text.Split({","c, ";"c}, StringSplitOptions.RemoveEmptyEntries)
                    mail.To.Add(addr.Trim())
                Next

                mail.Subject = txtSubject.Text
                mail.Body = txtBody.Text.Replace("%0A", vbCrLf).Replace(vbCrLf, "<br/>")
                mail.IsBodyHtml = True

                ' Attach file PDF
                mail.Attachments.Add(New Attachment(pdfPath))

                Using smtp As New SmtpClient()
                    smtp.Host = My.Settings.emailHost
                    smtp.Port = CInt(My.Settings.emailPort)
                    smtp.EnableSsl = True
                    smtp.Credentials = New NetworkCredential(My.Settings.emailName, My.Settings.emailPass)
                    smtp.Send(mail)
                End Using
            End Using

            ' Berhasil
            ClientScript.RegisterStartupScript(Me.GetType(), "success",
            "alert('Email berhasil dikirim!'); $('#emailModal').modal('hide');", True)

        Catch ex As Exception
            ClientScript.RegisterStartupScript(Me.GetType(), "error",
            "alert('Gagal mengirim email: " & ex.Message.Replace("'", "\'") & "');", True)
        End Try
    End Sub
    Protected Sub prevhtml()
        Dim sb As StringBuilder = New StringBuilder()
        Dim dt As New DataTable
        Dim subject As String = "CPO"
        Try
            dt = clsPO.Selectdt_CPO_grid(Mtid, custid, olid)

            sb.Append("<html>")
            sb.Append("<table width='100%' cellspacing='0' cellpadding='5'>")
            sb.Append("<tr height='20px'><th align='center' style='background-color: #000; border-radius: 0.25em; color: white; font: bold 100% sans-serif;text-align: center;'><strong style='font-size:180%;font-family:sans-serif;text-align: center;'> Pre Order</strong></th></tr>")
            If File.Exists(Server.MapPath("~/logo") & ".png") Then
                Dim path As String = Server.MapPath("/bin/logo") & ".png"
                sb.Append("<tr height='100px'><td><img src='logo.png' alt= ''></img></td><th align='center' style='text-align:center;border-radius: 0.25em; color: #000; font: bold 100% sans-serif; '><strong style='font-size:180%;font-family:sans-serif;'> " & subject & "</strong></th></tr>")
            Else
                sb.Append("<tr height='20px'><th align='center' style='background-color: #000; border-radius: 0.25em; color: white; font: bold 100% sans-serif; '><strong style='font-size:180%;font-family:sans-serif;'> " & subject & "</strong></th></tr>")
            End If
            sb.Append("</table>")


            sb.Append("<table width='100%' cellspacing='0' cellpadding='2'>")
            sb.Append("<tr height='20px'><td colspan = '3'></td></tr>")
            sb.Append("<tr><td width='60%'></td><td></td><td></td></tr>")
            sb.Append("<tr><td width='60%' style='font-size:100%;font-family:sans-serif;'></td>")
            sb.Append("<td width='15%' style='padding: 0.5em; position: relative; text-align: left;'><strong style='font-size:100%;font-family:sans-serif;'>Outlet:</strong></td>")
            sb.Append("<td width='25%' style='padding: 0.5em; position: relative; text-align: left;'>" & olname & "</td></tr>")
            sb.Append("<tr><td width='60%' style='font-size:100%;font-family:sans-serif;'></td><td></td><td></td></tr>")
            sb.Append("<tr height='20px'><td colspan = '3'></td></tr>")
            sb.Append("<tr height='25px'><td width='60%'><strong style='font-size:90%;font-family:sans-serif;'>Customer:</strong></td>")
            sb.Append("<td width='15%' style='padding: 0.5em; position: relative; text-align: left;background-color: #EEEEEE;border-radius: 0.25em; border: 0.5px solid #ccc;font-family:sans-serif;'>Order No</td>")
            sb.Append("<td width='25%' style='padding: 0.5em; position: relative; text-align: left;border-radius: 0.25em; border: 0.5px solid #ccc;font-family:sans-serif;'>" & Mtid & "</td></tr>")
            sb.Append("<tr height='25px'><td width='60%' style='font-size:90%;font-family:sans-serif;'>" & Customername & "</td>")
            sb.Append("<td width='15%' style='padding: 0.5em; position: relative; text-align: left;background-color: #EEEEEE;border-radius: 0.25em; border: 0.5px solid #ccc;font-family:sans-serif;vertical-align: middle;'>Created</td>")
            sb.Append("<td width='25%' style='padding: 0.5em; position: relative; text-align: left;border-radius: 0.25em; border: 0.5px solid #ccc;font-family:sans-serif;vertical-align: middle;'>" & TransDate & "</td></tr>")
            sb.Append("<tr height='25px'><td width='60%' style='font-size:90%;font-family:sans-serif;'></td>")
            sb.Append("<td width='15%' style='padding: 0.5em; position: relative; text-align: left;background-color: #EEEEEE;border-radius: 0.25em; border: 0.5px solid #ccc;font-family:sans-serif;vertical-align: middle;'>Sales Date</td>")
            sb.Append("<td width='25%' style='padding: 0.5em; position: relative; text-align: left;border-radius: 0.25em; border: 0.5px solid #ccc;font-family:sans-serif;vertical-align: middle;'>" & Busidate & "</td></tr>")
            sb.Append("<tr height='25px'><td width='60%' style='font-size:90%;font-family:sans-serif;'><strong style='font-size:100%;font-family:sans-serif;'>Phone:</strong></td>")
            sb.Append("<td width='15%' style='padding: 0.5em; position: relative; text-align: left;background-color: #EEEEEE;border-radius: 0.25em; border: 0.5px solid #ccc;font-family:sans-serif;vertical-align: middle;'>Total</td>")
            sb.Append("<td width='25%' style='padding: 0.5em; position: relative; text-align: left;border-radius: 0.25em; border: 0.5px solid #ccc;font-family:sans-serif;vertical-align: middle;'>" & doctot & "</td></tr>")
            sb.Append("<tr height='25px'><td width='60%' style='font-size:90%;font-family:sans-serif;'>" & Phone & "</td></tr>")

            'spasi sebelum table

            sb.Append("</table>")
            sb.Append("<table width='100%' cellspacing='0' cellpadding='2'>")
            sb.Append("<tr height='30px'><td></td></tr></table>")


            sb.Append("<table width='100%' cellspacing='0' cellpadding = '2' style='border-collapse: collapse;border: 1px solid #ccc;'>")
            sb.Append("<tr height='25px'>")

            For Each column As DataColumn In dt.Columns
                If column.ColumnName = "itemno" Or column.ColumnName = "itemname" Or column.ColumnName = "qty" Then
                    sb.Append("<th align='center' style='text-align: center;padding: 0.5em; position: relative;background-color: #B8DBFD; border: 0.5px solid #ccc;font-family:sans-serif;font-size:100%;'>")
                    sb.Append(column.ColumnName)
                    sb.Append("</th>")
                Else
                    Continue For
                End If

            Next

            sb.Append("</tr>")

            For Each row As DataRow In dt.Rows
                sb.Append("<tr height='25px'>")

                For Each column As DataColumn In dt.Columns
                    If column.ColumnName = "itemno" Or column.ColumnName = "itemname" Or column.ColumnName = "qty" Then
                        sb.Append("<td align='center' style='padding: 0.5em; position: relative;border: 0.5px solid #ccc;font-family:sans-serif;font-size:100%;'>")
                        sb.Append(row(column))
                        sb.Append("</td>")
                    Else
                        Continue For
                    End If

                Next

                sb.Append("</tr>")
            Next

            sb.Append("</table>")
            'spasi bawah table
            sb.Append("<table width='100%' cellspacing='0' cellpadding='2'>")
            sb.Append("<tr height='50px'><td></td></tr></table>")
            'Created By
            sb.Append("<table width='100%' cellspacing='0' cellpadding='2'>")
            'sb.Append("<tr><td width='60%'><strong style='font-size:100%;font-family:sans-serif;'>CreatedBy:</strong></td><td></td><td></td></tr>")
            'sb.Append("<tr><td width='60%' style='font-size:100%;font-family:sans-serif;'>" & username().ToString & "</td>")
            'sb.Append("<td width='15%' style='padding: 0.5em; position: relative; text-align: left;'></td>")
            'sb.Append("<td width='25%' style='padding: 0.5em; position: relative; text-align: left;'></td></tr>")
            'sb.Append("<tr><td width='60%' style='font-size:100%;font-family:sans-serif;'>" & CCfrom & "</td><td></td><td></td></tr>")

            If Not String.IsNullOrEmpty(My.Settings.approvedby) Then
                sb.Append("<tr><td width='30%' style='font-size:100%;font-family:sans-serif;position: relative; text-align: center;'></td>")
                sb.Append("<td width='30%' style='font-size:100%;font-family:sans-serif; position: relative; text-align: center;'></td>")
                sb.Append("<td width='30%' style='font-size:100%;font-family:sans-serif; position: relative; text-align: center;'>Created By:</td></tr><tr height='160px'></tr>")
                sb.Append("<tr><td width='30%' style='font-size:100%;font-family:sans-serif;position: relative; text-align: center;'></td>")
                sb.Append("<td width='30%' style='font-size:100%;font-family:sans-serif; position: relative; text-align: center;'></td><td></td></tr>")
            Else
                sb.Append("<tr><td width='40%' style='font-size:100%;font-family:sans-serif;position: relative; text-align: center;'></td>")
                sb.Append("<td width='40%' style='font-size:100%;font-family:sans-serif; position: relative; text-align: center;'>Created By:</td></tr><tr height='160px'><td></td></tr>")

            End If

            sb.Append("</table>")
            'kop bawah
            sb.Append("<table width='100%' cellspacing='0' cellpadding='2'>")
            sb.Append("<tr height='20px'><th align='center' style='font: bold 100% sans-serif; '><strong style='font-size:100%;font-family:sans-serif;letter-spacing: 0.5em;'></strong></th></tr>")
            'sb.Append("<tr height='0.2px'><td align='center' style='border: 0.5px solid #ccc; border-width: 0.5px;margin: 0 0 1em;'></td></tr>")
            sb.Append("<tr style='color:white;' height='5px'><td></td></tr>")
            sb.Append("<tr><td align='center'><p style='font-size:100%;font-family:sans-serif;'></p></td></tr>")
            sb.Append("</table>")


            sb.Append("</html>")


            'XMLWorker also reads from a TextReader and not directly from a string
            ViewState("html_string") = sb.ToString
            'grid.InnerHtml = htmlstring

            'CreateCookie("stringhtml", sb.ToString)

            'Response.Redirect("~/prev_InvOrderWDetailRcv.aspx")
        Catch ex As Exception
            errLog(Err.Description, "Pages_InvOrderWDetailRcv : btnNew_Click")
            Err.Clear()
        End Try
        sb = Nothing
        dt.Dispose()
        dt = Nothing
    End Sub

    Protected Sub btnclose_Click(sender As Object, e As System.EventArgs) Handles btnClose.Click

        Response.Redirect("~/Pages_CPO.aspx", False)

    End Sub
End Class