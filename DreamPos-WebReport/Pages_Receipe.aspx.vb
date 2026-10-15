Imports System.IO
Imports NPOI.XSSF.UserModel
Imports NPOI.SS.UserModel
Imports System.Net
Imports ClosedXML.Excel
Public Class Pages_Receipe
    Inherits System.Web.UI.Page
    Private mfunction As New ModFunction
    Private clsOutlet As New ClsOutlet
    Private CRcp As New Cls_IRcp
    ' Private clsInventory As New clsInventory_stkOpname

    Public Function modulepriv() As Integer
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        If cookie Is Nothing Then

            Response.Redirect("default.aspx", False)
        End If
        Return cookie("modulepriv")




    End Function
    Public Function custid() As Integer
        Dim icust As Integer = 0
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")

        If cookie Is Nothing Then
            Response.Redirect("~/Default.aspx", False)

        Else
            icust = cookie("custid")
        End If
        Return icust
    End Function
    Public Function username() As String
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        If cookie Is Nothing Then
            Response.Redirect("default.aspx", False)
        End If
        Return cookie("userName")

    End Function
    Public Function rptpriv() As Integer
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        If cookie Is Nothing Then

            Response.Redirect("default.aspx", False)
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


                Me.binding()
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
    Sub binding()

        Try
            If custid() = 0 Then
                Response.Redirect("~/Default.aspx", False)
            Else
                Dim ds As New DataSet
                ds = CRcp.SelectDataset_rcp(custid)
                If ds IsNot Nothing AndAlso ds.Tables.Count > 0 Then
                    lstViewDetails.DataSource = ds
                Else
                    lstViewDetails.DataSource = Nothing
                End If
                lstViewDetails.DataBind()
            End If
        Catch ex As Exception

        End Try

    End Sub
    Sub delete(ByVal id As String)
        FailureText.Text = ""
        Dim clsInv As New clsInventory
        Dim iHasil As Integer = 0
        Try
            DeleteRcp(id, custid, username)
            binding()


        Catch ex As Exception

        End Try
        clsInv = Nothing
    End Sub
    Public Sub DeleteRcp(ByVal id As Integer, ByVal Custid As Integer, ByVal username As String)
        Using db As New RecipeDb()
            db.SetParameter("id", id)
            db.SetParameter("custid", Custid)
            db.Execute("DELETE FROM cld_inv_rcp WHERE id=@id AND custid=@custid")
            db.Commit()
        End Using
        insertLogModule(username, "InvRcp", "Delete", Custid)
    End Sub
    Protected Sub lstView_ItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ListViewCommandEventArgs) Handles lstViewDetails.ItemCommand
        '<%--autocomplete="off"--%>
        Try
            Select Case (e.CommandName)
                Case "Del"

                    Dim confirmValue As String = Request.Form("confirm_value")
                    If confirmValue = "Yes" Then

                        Dim id As Integer = Convert.ToInt32(e.CommandArgument)
                        Dim clsReceipe As New clsInventory_Receipe

                        Dim rowid As Integer = clsReceipe.SelectDataset_Idrcp(id)
                        clsReceipe = Nothing

                        If rowid > 0 Then

                            Dim message As String = ""
                            If rowid = 1 Then
                                message = "Data can not be deleted because it is already in use Stock Master"

                            ElseIf rowid = 2 Then
                                message = "Data can not be deleted because it is already in use Receipe Detail "
                            End If

                            Dim sb As New System.Text.StringBuilder()
                            sb.Append("<script type = 'text/javascript'>")
                            sb.Append("window.onload=function(){")
                            sb.Append("alert('")
                            sb.Append(message)
                            sb.Append("')};")
                            sb.Append("</script>")
                            ClientScript.RegisterClientScriptBlock(Me.GetType(), "alert", sb.ToString())
                            ' delete(id)
                            Exit Sub
                        Else
                            delete(id)
                        End If
                    Else
                        Exit Sub
                    End If

                Case "Edt"
                    Dim modfunc As New ModFunction
                    Dim id As Integer = Convert.ToInt32(e.CommandArgument)

                    modfunc.CreateCookie("ID", id)

                    Response.Redirect("~/Pages_ReceipeEdit.aspx", False)



            End Select
        Catch ex As Exception
            errLog(Err.Description, "Pages_Receipe:lstView_ItemCommand")
            Err.Clear()
        End Try
    End Sub

    Protected Sub btnNew_Click(ByVal sender As Object, ByVal e As EventArgs) 'Handles btnnew.Click
        Try
            Dim modfunc As New ModFunction
            modfunc.CreateCookie("ID", 0)


            Response.Redirect("~/Pages_ReceipeEdit.aspx", False)
            Exit Sub

        Catch ex As Exception
            errLog(Err.Description, "Pages_Receipe:btnNew_Click")
            Err.Clear()
        End Try
    End Sub

    'Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.Click

    '    Dim ds As New DataSet
    '    ds = clsOutlet.SelectDataset_OutletSearch(txtsearch.Text, custid, username, modulepriv)
    '    lstViewDetails.DataSource = ds
    '    lstViewDetails.DataBind()
    'End Sub
    Protected Sub btnsearch(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim ds As New DataSet

            ds = CRcp.SelectDataset_rcpSearch(txtsearch.Text, custid)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 Then
                lstViewDetails.DataSource = ds
            Else
                lstViewDetails.DataSource = Nothing
            End If
            lstViewDetails.DataBind()
        Catch ex As Exception
            errLog(Err.Description, "Pages_Receipe:btnsearch")
            Err.Clear()
        End Try
    End Sub



    Protected Sub btncalc_Click(sender As Object, e As EventArgs) Handles btncalc.ServerClick
        Dim clsReceipe As New clsInventory_Receipe
        Try
            clsReceipe.Process_UpdateCostReceipe(custid, Now.ToString("yyyy-MM-dd 00:00:00"))
            messagesuccesfull("Recalculate Cost Success", True)
        Catch ex As Exception
            errLog(Err.Description, "Pages_Receipe:btncalc_Click")
            Err.Clear()
        End Try

    End Sub


    Sub messagesuccesfull(ByVal msg As String, Optional ByVal bsuccess As Boolean = False)

        Dim message As String = msg '"Import From Excel Successfully."
        Dim url As String = ""

        If bsuccess Then
            url = "/Pages_Receipe.aspx"
        Else
            url = "/Pages_Receipe.aspx"
        End If

        Dim script As String = "window.onload = function(){ alert('"
        script += message
        script += "');"
        script += "window.location = '"
        script += url
        script += "'; }"
        ClientScript.RegisterStartupScript(Me.GetType(), "Redirect", script, True)

    End Sub
    Protected Sub btnImport_Click(sender As Object, e As EventArgs) Handles btnImport.ServerClick

        Response.Redirect("~/Pages_ImprtRcp.aspx", False)
    End Sub

    Protected Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.ServerClick
        Try

            'Dim dt As DataTable
            Dim ds As New DataSet


            Dim sFile As String = "Receipe"

            'ds = CRcp.Selectds_rcpexport(custid)
            ds = CRcp.SelectDataset_rcpdtlExport(custid)

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
            mfunction.errLog(Err.Description, "Pages_Receipe:ExportData")
            Err.Clear()
        Finally


        End Try
    End Sub

End Class
