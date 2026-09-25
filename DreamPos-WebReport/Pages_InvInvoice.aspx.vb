Public Class Pages_InvInvoice
    Inherits System.Web.UI.Page
    Private mfunction As New ModFunction
    Private cInv As New clsInventory
    Private CRtn As New Cls_IReturn

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
                txtinvoicedate.Text = Format(Now, "dd MMM yyyy")
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
    Protected Sub txtinvoicedate_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtinvoicedate.TextChanged
        If txtinvoicedate.Text = "" Then
            txtinvoicedate.Text = Format(Now, "dd MMM yyyy")
        End If
        'GetBackground
        binding()
    End Sub
    Protected Function GetBackground()

        Dim imgstat As String

        If CDate(txtinvoicedate.Text) <> Format(CDate(Date.Now), "MM/dd/yyyy") Then
            imgstat = "nodisplay"
        Else
            imgstat = "displayaction"
        End If

        Return imgstat

    End Function
    Sub binding()

        Try
            Dim ds As New DataSet

            'ds = WService.get_ds_InvInvoice(cust(), txtinvoicedate.Text)

            Dim clsInvoice As New clsInventory_Invoice

            ds = clsInvoice.getDS_InvInvoice(custid, CDate(txtinvoicedate.Text))  'WService.get_ds_InvInvoice(cust(), txtinvoicedate.Text)
            clsInvoice = Nothing

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 Then
                lstViewDetails.DataSource = ds
            Else
                lstViewDetails.DataSource = Nothing
            End If
            lstViewDetails.DataBind()
        Catch ex As Exception

        End Try

    End Sub
    Sub delete(ByVal id As String)

        DeleteInvReturn(id, custid, username)
        binding()
    End Sub
    Public Sub DeleteInvReturn(ByVal id As String, ByVal Custid As Integer, ByVal username As String)
        'Dim pc As String = pic.ToString
        Try

            Dim str As String
            Dim strTr As String
            Dim c_cmd As New Cls_command

            '   strTr = "DELETE FROM cld_inv_trans  where linkid = '" & id & "' and custid = " & Custid & ""
            str = "DELETE FROM cld_invlk_Rcv  where id = '" & id & "' and custid = " & Custid & ""
            Dim ds As New DataSet
            Dim a As New clsDbo("Select * FROM cld_inv_trans  where linkid = '" & id & "' and custid = " & Custid & "")
            ds = a.selectDataset
            If ds.Tables(0).Rows.Count > 0 Then
                For Each row As DataRow In ds.Tables(0).Rows
                    strTr = "DELETE FROM cld_inv_trans  where id = " & row.Item("id") & ""
                    c_cmd.executeNonQuery(strTr)


                    'c_cmd.executeNonQuery_Inv_CalcCost(row.Item("stk_id"), row.Item("cc"), Custid)
                    'c_cmd.executeNonQuery_Inv_CalcStock(row.Item("stk_id"), row.Item("cc"), Custid)

                Next
            End If
            a.dispose()
            a = Nothing
            c_cmd.executeNonQuery(str)
            insertLogModule(username, "Inv_Return", "Delete", Custid)
        Catch ex As Exception
            errLog(Err.Description, "Pages_InvReturn:DeleteInv_Return")
            Err.Clear()
        End Try
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


            ' PERBAIKAN: Jangan hapus transaksi type=9 (Batch Production) dari halaman Invoice
            ' strTr = "DELETE FROM cld_inv_trans where ID = " & id & " and type = 9 "
            ' c_cmd.executeNonQuery(strTr)


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
                    mfunction.CreateCookie("Cc", arg(1))
                    mfunction.CreateCookie("Supid", arg(2))
                    mfunction.CreateCookie("DO", arg(3))



                    Response.Redirect("~/Pages_InvReturnEdit.aspx")



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
            mfunction.CreateCookie("invoiceid", 0)
            Response.Redirect("~/Pages_InvInvoiceEdit.aspx")
        Catch ex As Exception
            errLog(Err.Description, "Pages_InvInvoice:btnNew_Click")
            Err.Clear()
        End Try
    End Sub
    Protected Sub OnPagePropertiesChanging(ByVal sender As Object, ByVal e As PagePropertiesChangingEventArgs)
        lvDataPager1.SetPageProperties(e.StartRowIndex, e.MaximumRows, False)
        binding()
    End Sub
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.ServerClick
        Try
            Dim ds As New DataSet
            Dim clsInvoice As New clsInventory_Invoice

            ds = clsInvoice.getDS_InvInvoice(custid, CDate(txtinvoicedate.Text))  'WService.get_ds_InvInvoice(cust(), txtinvoicedate.Text)
            clsInvoice = Nothing

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 Then
                lstViewDetails.DataSource = ds
            Else
                lstViewDetails.DataSource = Nothing
            End If
            lstViewDetails.DataBind()
        Catch ex As Exception

        End Try
    End Sub


End Class
