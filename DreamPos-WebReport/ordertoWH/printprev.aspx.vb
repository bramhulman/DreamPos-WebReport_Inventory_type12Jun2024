Imports System.IO
Imports iTextSharp.text
Imports iTextSharp.text.html.simpleparser
Imports iTextSharp.text.pdf
Imports iTextSharp.tool.xml
Imports System.Data
Public Class printprev
    Inherits System.Web.UI.Page
    Private wID As String
    Private wCctoname As String 'outlet
    Private wFromname As String 'warehouse/HO
    Private wtoid As Integer
    Private wDelID As String 'delivery_id

    Private wexpdate As String ' expecteddelivery
    Private worderdate As String ' orderdate
    Private wdeldate As String ' delivery
    Private wrcvdate As String ' receive
    Private wDelStatus As String
    Private wDelStatusid As Integer
    Private wDoctot As String
    Private mfunction As New ModFunction

    Private wNotes As String

    Private clsInv As New clsInventory
    Private clsStk As New clsInventory_stk

    Dim htmlstring As String
    Private from As String
    Private print As String
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
            'wID = Request.Cookies("woID").Value
            'from = Request.Cookies("from").Value
            'Print = Request.Cookies("print").Value
            If (Page.IsPostBack) Then
            Else
                wID = Request.Cookies("woID").Value
                from = Request.Cookies("from").Value
                print = Request.Cookies("print").Value
                dt = clsInv.getInvWareById(wID).Tables(0)
                If dt.Rows.Count > 0 Then
                    If (Not String.IsNullOrEmpty(dt.Rows(0).Item("delid").ToString)) Then
                        wDelID = dt.Rows(0).Item("delid")
                    Else
                        wDelID = ""

                    End If
                    If (Not String.IsNullOrEmpty(dt.Rows(0).Item("cctonm").ToString)) Then
                        wCctoname = dt.Rows(0).Item("cctonm")
                        wtoid = dt.Rows(0).Item("ccto")
                    Else
                        wCctoname = ""
                        wtoid = 0
                    End If
                    If (Not String.IsNullOrEmpty(dt.Rows(0).Item("ccFromNm").ToString)) Then

                        wFromname = dt.Rows(0).Item("ccFromNm")
                    Else
                        wFromname = ""

                    End If
                    If (Not String.IsNullOrEmpty(dt.Rows(0).Item("flag").ToString)) Then
                        If print = "DO" Then
                            wDelStatus = "Delivery"
                            ViewState("html_stringflag") = 2
                        Else
                            If dt.Rows(0).Item("flag") = 1 Then
                                wDelStatus = "Request"
                            ElseIf dt.Rows(0).Item("flag") = 2 Then
                                wDelStatus = "Delivery"
                            ElseIf dt.Rows(0).Item("flag") = 3 Then
                                wDelStatus = "Receive"
                            End If
                            'wDelStatusid = dt.Rows(0).Item("flag")
                            ViewState("html_stringflag") = dt.Rows(0).Item("flag")
                        End If


                    Else
                        wDelStatus = ""
                        'wDelStatusid = 0
                        ViewState("html_stringflag") = 0
                    End If
                    If (Not String.IsNullOrEmpty(dt.Rows(0).Item("Notes").ToString)) Then
                        wNotes = dt.Rows(0).Item("Notes")
                    Else
                        wNotes = ""

                    End If
                    If (Not String.IsNullOrEmpty(dt.Rows(0).Item("exDelDate").ToString)) Then
                        wexpdate = Format(dt.Rows(0).Item("exDelDate"), "dd MMM yyyy")
                    Else
                        wexpdate = ""

                    End If
                    If (Not String.IsNullOrEmpty(dt.Rows(0).Item("CreatedDate").ToString)) Then
                        worderdate = Format(dt.Rows(0).Item("CreatedDate"), "dd MMM yyyy")
                    Else
                        worderdate = ""

                    End If
                    If (Not String.IsNullOrEmpty(dt.Rows(0).Item("DelDate").ToString)) Then
                        wdeldate = Format(dt.Rows(0).Item("DelDate"), "dd MMM yyyy")
                    Else
                        wdeldate = ""

                    End If
                    If (Not String.IsNullOrEmpty(dt.Rows(0).Item("rcvDate").ToString)) Then
                        wrcvdate = Format(dt.Rows(0).Item("rcvDate"), "dd MMM yyyy")
                    Else
                        wrcvdate = ""

                    End If

                    If (dt.Rows(0).Item("DocTot")) > 0 Then
                        wDoctot = Format(dt.Rows(0).Item("DocTot"), "#,###.00")
                    Else
                        wDoctot = ""

                    End If
                    If print = "DO" Then
                        wrcvdate = ""
                        wDoctot = ""
                    End If
                    prevhtml()
                End If
            End If
            grid.InnerHtml = ViewState("html_string")
            wDelStatusid = ViewState("html_stringflag")
        Catch ex As Exception
            errLog(Err.Description, "printprev:Page_Load")
            Err.Clear()
        End Try
    End Sub
    Protected Sub btnpdf_Click(sender As Object, e As System.EventArgs) Handles btnpdf.Click

        htmlstring = ViewState("html_string")
        Dim bytes As Byte()
        wID = Request.Cookies("woID").Value

        'Boilerplate iTextSharp setup here
        'Create a stream that we can write to, in this case a MemoryStream
        Using ms = New MemoryStream()


            'Create an iTextSharp Document which is an abstraction of a PDF but **NOT** a PDF
            Using doc = New Document()


                'Create a writer that's bound to our PDF abstraction and our stream
                Using writer = PdfWriter.GetInstance(doc, ms)

                    'Open the document for writing
                    doc.Open()

                    'XMLWorker also reads from a TextReader and not directly from a string

                    Using srHtml = New StringReader(htmlstring)

                        'Parse the HTML
                        iTextSharp.tool.xml.XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, srHtml)
                    End Using


                    doc.Close()
                    doc.Dispose()
                End Using
            End Using


            'After all of the PDF "stuff" above is done and closed but **before** we
            'close the MemoryStream, grab all of the active bytes from the stream
            bytes = ms.ToArray()
            ms.Dispose()
        End Using


        'Now we just need to do something with those bytes.
        'Here I'm writing them to disk but if you were in ASP.Net you might Response.BinaryWrite() them.
        'You could also write the bytes to a database in a varbinary() column (but please don't) or you
        'could pass them to another function for further PDF processing.
        'Dim testFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Order.pdf")
        Dim testFile = Server.MapPath("/bin/order") & username.ToString + Format(DateTime.Now, "dd MMM yyyy").ToString & ".pdf"
        File.WriteAllBytes(testFile, bytes)

        htmlstring = ""
        'Tambahan - SafeFileDialog
        Response.ClearContent()
        Response.Clear()
        Response.ContentType = "application/pdf"
        Response.AddHeader("content-disposition", "attachment;filename=" & wID.ToString & "-" & Format(DateTime.Now, "dd MMM yyyy").ToString & ".pdf")
        Response.TransmitFile(testFile)

        HttpContext.Current.Response.Flush()

        System.IO.File.Delete(testFile)

    End Sub


    Protected Sub prevhtml()
        Dim sb As StringBuilder = New StringBuilder()
        Dim dt As New DataTable
        Try
            'If wDelStatusid = 1 Then
            If ViewState("html_stringflag") = 1 Then
                If wID = "" Then
                    dt = clsInv.getDTInvWare_rcv_grid(wID, custid, 0, 0)
                Else
                    dt = clsInv.getDTInvWare_rcv_grid(wID, custid, wtoid, 0)
                End If
                dt.Columns.Remove("Delivery")
                dt.Columns.Remove("Receive")
                dt.Columns.Remove("Price")
                dt.Columns.Remove("Total")
                'ElseIf wDelStatusid = 2 Then
            ElseIf ViewState("html_stringflag") = 2 Then
                If wID = "" Then
                    dt = clsInv.getDTInvWare_rcv_grid(wID, custid, 0, 0)
                Else
                    dt = clsInv.getDTInvWare_rcv_grid(wID, custid, wtoid, 0)
                End If

                dt.Columns.Remove("Receive")

                dt.Columns.Remove("Price")
                dt.Columns.Remove("Total")


                'ElseIf wDelStatusid = 3 Then
            ElseIf ViewState("html_stringflag") = 3 Then
                If wID = "" Then
                    dt = clsInv.getDTInvWare_rcv_grid(wID, custid, 0, 0)
                Else
                    dt = clsInv.getDTInvWare_rcv_grid(wID, custid, wtoid, 0)
                End If
                dt.Columns.Remove("Delivery")
                dt.Columns.Remove("Order")
                dt.Columns.Remove("Note")
            End If

            sb.Append("<html>")
            sb.Append("<table width='100%' cellspacing='0' cellpadding='5'>")
            Dim subject As String
            If wDelStatus = "Receive" Then
                subject = "Warehouse Invoice"
            ElseIf wDelStatus = "Delivery" Then

                subject = "Delivery Order"
                wDoctot = "" '0


            Else
                subject = "Order Request"
            End If
            If File.Exists(Server.MapPath("/logo") & ".png") Then
                Dim path As String = Server.MapPath("/bin/logo") & ".png"
                sb.Append("<tr height='100px'><td><img src='logo.png' alt= ''></img></td><th align='center' style='border-radius: 0.25em; color: #000; font: bold 100% sans-serif; '><strong style='font-size:180%;font-family:sans-serif;'> " & subject & "</strong></th></tr>")
            Else
                sb.Append("<tr height='20px'><th align='center' style='background-color: #000; border-radius: 0.25em; color: white; font: bold 100% sans-serif; '><strong style='font-size:180%;font-family:sans-serif;'> " & subject & "</strong></th></tr>")
            End If

            sb.Append("</table>")


            sb.Append("<table width='100%' cellspacing='0' cellpadding='2'>")
            sb.Append("<tr height='20px'><td colspan = '3'></td></tr>")
            sb.Append("<tr><td width='60%'><strong style='font-size:100%;font-family:sans-serif;'>Outlet:</strong></td><td></td><td></td></tr>")
            'sb.Append("<tr><td width='60%' style='font-size:100%;font-family:sans-serif;'>" & username().ToString & "</td>")
            sb.Append("<tr><td width='60%' style='font-size:100%;font-family:sans-serif;'>" & wCctoname & "</td>")
            sb.Append("<td width='15%' style='padding: 0.5em; position: relative; text-align: left;'><strong style='font-size:100%;font-family:sans-serif;'>Status:</strong></td>")
            sb.Append("<td width='25%' style='padding: 0.5em; position: relative; text-align: left;'>" & wDelStatus & "</td></tr>")
            sb.Append("<tr><td width='60%' style='font-size:100%;font-family:sans-serif;'></td><td></td><td></td></tr>")
            'sb.Append("<tr><td width='60%' style='font-size:100%;font-family:sans-serif;'>" & wCctoname & "</td><td></td><td></td></tr>")
            sb.Append("<tr height='20px'><td colspan = '3'></td></tr>")
            sb.Append("<tr height='25px'><td width='60%'><strong style='font-size:90%;font-family:sans-serif;'>Warehouse/Ho:</strong></td>")
            sb.Append("<td width='15%' style='padding: 0.5em; position: relative; text-align: left;background-color: #EEEEEE;border-radius: 0.25em; border: 0.5px solid #ccc;font-family:sans-serif;'>Order No</td>")
            sb.Append("<td width='25%' style='padding: 0.5em; position: relative; text-align: left;border-radius: 0.25em; border: 0.5px solid #ccc;font-family:sans-serif;'>" & wID & "</td></tr>")
            sb.Append("<tr height='25px'><td width='60%' style='font-size:90%;font-family:sans-serif;'>" & wFromname & "</td>")
            sb.Append("<td width='15%' style='padding: 0.5em; position: relative; text-align: left;background-color: #EEEEEE;border-radius: 0.25em; border: 0.5px solid #ccc;font-family:sans-serif;vertical-align: middle;'>Delivery Date</td>")
            sb.Append("<td width='25%' style='padding: 0.5em; position: relative; text-align: left;border-radius: 0.25em; border: 0.5px solid #ccc;font-family:sans-serif;vertical-align: middle;'>" & wdeldate & "</td></tr>")
            sb.Append("<tr height='25px'><td width='60%' style='font-size:90%;font-family:sans-serif;'></td>")
            sb.Append("<td width='15%' style='padding: 0.5em; position: relative; text-align: left;background-color: #EEEEEE;border-radius: 0.25em; border: 0.5px solid #ccc;font-family:sans-serif;vertical-align: middle;'>Order Date</td>")
            sb.Append("<td width='25%' style='padding: 0.5em; position: relative; text-align: left;border-radius: 0.25em; border: 0.5px solid #ccc;font-family:sans-serif;vertical-align: middle;'>" & worderdate & "</td></tr>")
            sb.Append("<tr height='25px'><td width='60%' style='font-size:90%;font-family:sans-serif;'><strong style='font-size:100%;font-family:sans-serif;'>Notes:</strong></td>")
            sb.Append("<td width='15%' style='padding: 0.5em; position: relative; text-align: left;background-color: #EEEEEE;border-radius: 0.25em; border: 0.5px solid #ccc;font-family:sans-serif;vertical-align: middle;'>Delivery No</td>")
            sb.Append("<td width='25%' style='padding: 0.5em; position: relative; text-align: left;border-radius: 0.25em; border: 0.5px solid #ccc;font-family:sans-serif;vertical-align: middle;'>" & wDelID & "</td></tr>")
            sb.Append("<tr height='25px'><td width='60%' style='font-size:90%;font-family:sans-serif;'>" & wNotes & "</td>")
            sb.Append("<td width='15%' style='padding: 0.5em; position: relative; text-align: left;background-color: #EEEEEE;border-radius: 0.25em; border: 0.5px solid #ccc;font-family:sans-serif;vertical-align: middle;'>Receive Date</td>")
            sb.Append("<td width='25%' style='padding: 0.5em; position: relative; text-align: left;border-radius: 0.25em; border: 0.5px solid #ccc;font-family:sans-serif;vertical-align: middle;'>" & wrcvdate & "</td></tr>")
            sb.Append("<tr height='25px'><td width='60%' style='font-size:90%;font-family:sans-serif;'></td>")
            sb.Append("<td width='15%' style='padding: 0.5em; position: relative; text-align: left;background-color: #EEEEEE;border-radius: 0.25em; border: 0.5px solid #ccc;font-family:sans-serif;vertical-align: middle;'>Doc. Total</td>")
            sb.Append("<td width='25%' style='padding: 0.5em; position: relative; text-align: left;border-radius: 0.25em; border: 0.5px solid #ccc;font-family:sans-serif;vertical-align: middle;'>" & wDoctot & "</td></tr>")
            'spasi sebelum table

            sb.Append("</table>")
            sb.Append("<table width='100%' cellspacing='0' cellpadding='2'>")
            sb.Append("<tr height='30px'><td></td></tr></table>")


            sb.Append("<table width='100%' cellspacing='0' cellpadding = '2' style='border-collapse: collapse;border: 1px solid #ccc;'>")
            sb.Append("<tr height='25px'>")

            For Each column As DataColumn In dt.Columns
                sb.Append("<th align='center' style='padding: 0.5em; position: relative;background-color: #B8DBFD; border: 0.5px solid #ccc;font-family:sans-serif;font-size:100%;'>")
                sb.Append(column.ColumnName)
                sb.Append("</th>")
            Next

            sb.Append("</tr>")

            For Each row As DataRow In dt.Rows
                sb.Append("<tr height='25px'>")

                For Each column As DataColumn In dt.Columns
                    sb.Append("<td align='center' style='padding: 0.5em; position: relative;border: 0.5px solid #ccc;font-family:sans-serif;font-size:100%;'>")
                    sb.Append(row(column))
                    sb.Append("</td>")
                Next

                sb.Append("</tr>")
            Next

            sb.Append("</table>")
            'spasi bawah table
            sb.Append("<table width='100%' cellspacing='0' cellpadding='2'>")
            sb.Append("<tr height='40px'><td></td></tr>") '(</table>")

            If subject = "Warehouse Invoice" Then
                sb.Append("<tr><td width='40%' style='font-size:100%;font-family:sans-serif;position: relative; text-align: center;'></td>")
                sb.Append("<td width='40%' style='font-size:100%;font-family:sans-serif; position: relative; text-align: center;'>prepared by:</td></tr><tr height='160px'><td></td></tr>")

            Else
                sb.Append("<tr><td width='40%' style='font-size:100%;font-family:sans-serif;position: relative; text-align: center;'>Received by:</td>")
                sb.Append("<td width='40%' style='font-size:100%;font-family:sans-serif; position: relative; text-align: center;'>prepared by:</td></tr><tr height='160px'><td></td></tr>")

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

    Protected Sub btnclose_Click(sender As Object, e As System.EventArgs) Handles btnclose.Click

        If wDelStatusid = 1 Then
            mfunction.CreateCookie("wType", wDelStatusid)
            Response.Redirect("~/Pages_InvOrderW.aspx")
        ElseIf wDelStatusid = 2 Then
            If from = "~/Pages_InvOrderWHO.aspx" Then
                Response.Redirect("~/Pages_InvOrderWHO.aspx")
            Else
                mfunction.CreateCookie("wType", wDelStatusid)
                Response.Redirect("~/Pages_InvOrderW.aspx")
            End If

        ElseIf wDelStatusid = 3 Then
            If from = "~/Pages_InvOrderWHO.aspx" Then
                Response.Redirect("~/Pages_InvOrderWHO.aspx")
            Else
                mfunction.CreateCookie("wType", wDelStatusid)
                Response.Redirect("~/Pages_InvOrderW.aspx")
            End If
        End If

    End Sub
End Class