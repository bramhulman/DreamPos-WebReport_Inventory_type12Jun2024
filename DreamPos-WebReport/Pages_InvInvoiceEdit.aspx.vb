Imports System.Threading
Imports System.Net
Imports System.Text
Imports System.IO
Imports System.Globalization

Public Class Pages_InvInvoiceEdit
    Inherits System.Web.UI.Page
    Private sNameuser As String
    Private Userid As Integer
    Private icustid As Integer
    Private imodul As Integer
    Private iModPriv As Integer
    Private irptpriv As Integer
    Private irptf As Integer
    Private mfunction As New ModFunction
    Private rcvid As String
    Private rcvdo As String
    Private rcvdoctot As Decimal
    Private rcvoutlet As String
    Private rcvcreateddate As Date
    Private invoiceid As Integer
    Private number As String
    Private invoicedate As Date
    Private invdate As Date
    Private invoicetot As Decimal
    Private cInvInvoice As New cls_IInvoice


    'Private transpriv As Integer
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
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

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
        Try

            If Request.Cookies("UserLogin_inv") IsNot Nothing Then
                sNameuser = username()
                icustid = custid()
                imodul = modul()
                iModPriv = modulepriv()
                irptpriv = rptpriv()
                irptf = rptf()
                If IsPostBack = False Then
                    hapus()
                    loadSup()
                    bindinginvrcv(DdlSup.SelectedValue)
                    Dim dt As New DataTable
                    dt = cInvInvoice.Selectrcv_invoice_col(custid, DdlSup.SelectedValue).Tables(0)
                    ViewState("InvRcv_get") = dt
                    check_validation()
                End If
                ScriptManager.GetCurrent(Me).RegisterPostBackControl(RadButton1)
                'This for Search Button
                txtSearch.Attributes.Add("onkeyup", Me.Page.ClientScript.GetPostBackEventReference(txtSearch, ""))

                'ScriptManager.GetCurrent(Me).RegisterPostBackControl(RadButton1)
                txtSearch.Attributes.Add("onfocus", "javascript: this.setSelectionRange(this.value.length,this.value.length);")
                txtnotes.Attributes.Add("maxlength", txtnotes.MaxLength.ToString)
            Else
                Dim script As String = "window.onload = function(){ alert('"
                script += "Please Login"
                script += "');"
                script += "window.location = '"
                script += "../Default"
                script += " '; }"
                ClientScript.RegisterStartupScript(Me.GetType(), "Redirect", script, True)
            End If




            'yestr = CDate(Now.AddDays(-1)).ToString("yyyyMMdd")

        Catch ex As Exception

        End Try
    End Sub
    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        Threading.Thread.Sleep(500)
        BindCheckItem()
        bindinginvrcv(DdlSup.SelectedValue, txtSearch.Text)
        CheckedItem()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Pop", "openModalItem();", True)
        txtSearch.Attributes("onfocus") = "javascript:this.setSelectionRange(this.value.length,this.value.length);"
        txtSearch.Focus()
    End Sub
    Protected Sub Page_unLoad(sender As Object, e As EventArgs) Handles Me.Unload
    End Sub
    Sub hapus()
        'txtCreatedby.Enabled = True
        DdlSup.Enabled = True
        DdlSup.SelectedValue = Nothing
        txtinvoiceno.Text = ""
        txtinvoicedate.Text = Format(Now, "dd MMM yyyy")
        txtinvoicetot.Text = ""
        txtnotes.Text = ""


        txtSearch.Text = ""
        InsertButton.Focus()

        ViewState("getrcv") = Nothing
        ViewState("InvRcv_get") = Nothing

        DgvRcv.DataSource = Nothing
        DgvRcv.DataBind()
    End Sub
    Sub gettotalinvoice()
        Dim dt As DataTable = DirectCast(ViewState("InvRcv_get"), DataTable)
        Dim x As Double = 0
        For i As Integer = 0 To dt.Rows.Count - 1
            x = x + Val(dt.Rows(i).Item("DocTot"))

        Next i
        txtinvoicetot.Text = x
        check_validation()
    End Sub

    Sub loadSup()

        Dim ds As New DataSet
        ds = cInvInvoice.getsupplierinv(custid)
        If ds IsNot Nothing AndAlso ds.Tables.Count > 0 Then
            DdlSup.DataSource = ds
        Else
            DdlSup.DataSource = Nothing
        End If
        DdlSup.DataValueField = "Id"
        DdlSup.DataTextField = "Name"
        DdlSup.DataBind()

        If DdlSup.Items.Count > 0 Then
            DdlSup.SelectedIndex = 0
        End If

    End Sub
    Private Sub check_validation()
        If txtinvoiceno.Text = "" And Val(txtinvoicetot.Text) = 0 Then
            'btnsave.Enabled = False
            'btnAdd.Enabled = False
            DdlSup.Enabled = True
        ElseIf txtinvoiceno.Text <> "" And Val(txtinvoicetot.Text) = 0 Then
            'btnAdd.Enabled = True
            'btnsave.Enabled = False
            DdlSup.Enabled = True
        ElseIf txtinvoiceno.Text <> "" And Val(txtinvoicetot.Text) <> 0 Then
            'btnsave.Enabled = True
            'btnAdd.Enabled = True
            DdlSup.Enabled = False
        End If
    End Sub
    Sub Add()

        Try
            Dim i As Integer = 0
            For Each item As ListViewItem In LsvInvRcv.Items
                Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox1"), CheckBox)


                If ck.Checked Then
                    Try
                        Dim ds As DataSet = cInvInvoice.Selectrcv_invByID(custid, (DirectCast(item.FindControl("lblID"), Label).Text))
                        If ds.Tables(0).Rows.Count > 0 Then
                            rcvid = ds.Tables(0).Rows(0).Item("id").ToString
                            rcvdo = ds.Tables(0).Rows(0).Item("do").ToString
                            rcvdoctot = ds.Tables(0).Rows(0).Item("DocTot").ToString
                            rcvoutlet = ds.Tables(0).Rows(0).Item("outlet").ToString
                            rcvcreateddate = ds.Tables(0).Rows(0).Item("CreatedDate").ToString
                            Dim dt As DataTable = DirectCast(ViewState("InvRcv_get"), DataTable)
                            'dt.TableName = "getInvrcv"
                            'dt.Clear()
                            ''create coloum detail
                            'dt.Columns.Add(New DataColumn("id", GetType(String)))
                            'dt.Columns.Add(New DataColumn("do", GetType(String)))
                            'dt.Columns.Add(New DataColumn("DocTot", GetType(String)))
                            'dt.Columns.Add(New DataColumn("outlet", GetType(String)))
                            'dt.Columns.Add(New DataColumn("CreatedDate", GetType(String)))

                            dt.Rows.Add(rcvid, rcvdo, rcvdoctot, rcvoutlet, rcvcreateddate)
                            ViewState("InvRcv_get") = dt
                            Dim dtrcv As DataTable = DirectCast(ViewState("InvRcv"), DataTable)
                            dtrcv.Rows(i).Delete()
                            dtrcv.AcceptChanges()
                            ViewState("InvRcv") = dtrcv
                            i = i - 1
                        End If
                        Bind_invrcv()
                        BindData()
                        txtSearch.Text = ""
                        DdlSup.Enabled = "false"
                    Catch error_t As Exception
                        MsgBox(error_t.ToString)
                    End Try

                End If
                i = i + 1
            Next
            gettotalinvoice()
        Catch ex As Exception
            'MsgBox(ex.Message)
        End Try

    End Sub
    Protected Sub BindData()
        If custid() = 0 Then

            Response.Redirect("~/Default.aspx")

        Else

            DgvRcv.DataSource = DirectCast(ViewState("InvRcv_get"), DataTable)
            DgvRcv.DataBind()
        End If

    End Sub
    Protected Sub Bind_invrcv()
        If custid() = 0 Then
            Response.Redirect("~/Default.aspx")
        Else
            LsvInvRcv.DataSource = DirectCast(ViewState("InvRcv"), DataTable)
            LsvInvRcv.DataBind()
        End If

    End Sub

    Sub bindinginvrcv(ByVal SupId As Integer, Optional ByVal stext As String = "") 'binding awal

        If custid() = 0 Then
            Response.Redirect("~/Default.aspx")
        Else

            Dim dt As New DataTable
            dt = cInvInvoice.Selectrcv_invoice(custid, SupId, stext).Tables(0)
            ViewState("InvRcv") = dt
            LsvInvRcv.DataSource = DirectCast(ViewState("InvRcv"), DataTable)
            LsvInvRcv.DataBind()
        End If


    End Sub
    Protected Sub RadButton1_ServerClick(sender As Object, e As EventArgs) Handles RadButton1.ServerClick
        Add()

    End Sub
    Public Function username() As String
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        Return cookie("userName")

    End Function



    Protected Sub RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow AndAlso DgvRcv.EditIndex = e.Row.RowIndex Then


        End If
    End Sub

    Protected Sub DgvRcv_RowCancelingEdit(ByVal sender As Object, ByVal e As GridViewCancelEditEventArgs)
        DgvRcv.EditIndex = -1
        BindData()
    End Sub

    Protected Sub DgvRcv_RowEditing(ByVal sender As Object, ByVal e As GridViewEditEventArgs)
        DgvRcv.EditIndex = e.NewEditIndex
        BindData()
    End Sub
    Protected Sub DgvRcv_RowDeleting(ByVal sender As Object, ByVal e As GridViewDeleteEventArgs)
        Dim lblID As Label = DirectCast(DgvRcv.Rows(e.RowIndex).FindControl("lblID"), Label)
        Dim lblDO As Label = DirectCast(DgvRcv.Rows(e.RowIndex).FindControl("lblDO"), Label)
        Dim lblDocTot As Label = DirectCast(DgvRcv.Rows(e.RowIndex).FindControl("lblDocTot"), Label)
        Dim lblOutlet As Label = DirectCast(DgvRcv.Rows(e.RowIndex).FindControl("lblOutlet"), Label)
        Dim lblCreatedDate As Label = DirectCast(DgvRcv.Rows(e.RowIndex).FindControl("lblCreatedDate"), Label)
        Dim dtgetrcv As DataTable = DirectCast(ViewState("InvRcv_get"), DataTable)
        Dim dtrcv As DataTable = DirectCast(ViewState("InvRcv"), DataTable)

        dtrcv.Rows.Add(lblID.Text, lblDO.Text, lblDocTot.Text, lblOutlet.Text, lblCreatedDate.Text)
        ViewState("InvRcv") = dtrcv
        Dim confirmValue As String = Request.Form("confirm_value")
        If confirmValue = "Yes" Then

            dtgetrcv.Rows(e.RowIndex).Delete()
            dtgetrcv.AcceptChanges()


        Else
            Exit Sub
        End If
        'dtgetrcv.Rows(e.RowIndex).Delete()
        'dtgetrcv.AcceptChanges()
        'End If
        BindData()
        Bind_invrcv()
        gettotalinvoice()
    End Sub
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btncancel.ServerClick

        Try
            hapus()
            mfunction.CreateCookie("ID", 0)

            Response.Redirect("~/Pages_InvInvoice.aspx")
        Catch ex As Exception
            errLog(Err.Description, "WebBatchEdit:btnCancel_Click")
            Err.Clear()
        End Try

    End Sub
    Sub insert()

        Try
            Dim msg As String = ""
            Dim dt As DataTable = DirectCast(ViewState("InvRcv_get"), DataTable)
            Dim newLine As String = "\n"
            If dt Is Nothing Then
                msg = msg + "Item is Nothing" & newLine
            End If
            If txtinvoiceno.Text = "" Then
                msg = msg + "invoice Number must be fill" & newLine
            End If

            If DdlSup.SelectedValue = 0 Or DdlSup.SelectedItem.Text = "" Then
                msg = msg + "Supplier Must be Choice" & newLine
            End If
            For Each row As DataRow In dt.Rows
                If row.Item("DocTot") = 0 Then
                    msg = msg + row.Item("id") + " haveno Total " & newLine
                End If

            Next row

            If msg <> "" Then
                notsuccesfull(msg)
                Exit Sub
            End If
            If txtinvoiceno.Text <> "" And Val(txtinvoicetot.Text) <> 0 And dt.Rows.Count <> 0 And txtinvoicedate.Text <> "" Then
                Dim idinvoice As Integer
                idinvoice = cInvInvoice.InsertInv_invoice(custid, txtinvoiceno.Text, CDate(txtinvoicedate.Text), Val(txtinvoicetot.Text), txtnotes.Text)

                For Each row As DataRow In dt.Rows
                    cInvInvoice.Update_invrcv(row.Item("id"), custid, idinvoice)

                Next row
                ViewState("getrcv") = Nothing
                ViewState("InvRcv_get") = Nothing


                Response.Redirect("~/Pages_InvInvoice.aspx")
            Else
                'lblMsg.Text = lblMsg.Text & " Inventory Invoice Failed to save"
                'If txtinvoiceno.Text = "" Then
                '    lblMsg.Text = lblMsg.Text & ", Number is blank"
                'End If
                'If txtinvoicedate.Text = "" Then
                '    lblMsg.Text = lblMsg.Text & ", Date is blank"
                'End If
                'If txtinvoicetot.Text = "" Or Val(txtinvoicetot.Text) = 0 Then
                '    lblMsg.Text = lblMsg.Text & ", Total is blank"
                'End If
                If dt.Rows.Count = 0 Then
                    msg = msg + "Received Inventory is blank" & newLine
                    ' lblMsg.Text = lblMsg.Text & ", Received Inventory is blank"
                    alertMessage(msg, 2)
                End If


            End If


        Catch ex As Exception

            'MsgBox(Err.Description, MsgBoxStyle.Information)
            Err.Clear()
        End Try

    End Sub
    'Protected Sub btnmsg_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnmsg.Click
    '    lblMsg.Text = "Failed to save Invoice"
    '    lblMsg.Visible = False
    '    btnmsg.Visible = False
    '    btnSave.Focus()
    'End Sub
    Sub alertMessage(ByVal msg As String, ByVal type As Integer) '1 onload, 2 onsubmit, 3 confirmation
        Dim message As String = msg
        Dim sb As New System.Text.StringBuilder()
        ' Dim url As String = "~/Pages_InvTransfer.aspx"
        sb.Append("<script type = 'text/javascript'>")
        sb.Append("window.onload=function(){")
        If type = 3 Then
            sb.Append("return confirm('")
        Else
            sb.Append("alert('")
        End If

        sb.Append(message)
        sb.Append("')};")
        sb.Append("</script>")
        If type = 1 Then 'onload event
            ClientScript.RegisterClientScriptBlock(Me.GetType(), "alert", sb.ToString())
        Else 'onsubmit event / confirmation
            ClientScript.RegisterOnSubmitStatement(Me.GetType(), "alert", sb.ToString())
        End If

    End Sub
    Protected Sub txtinvoiceno_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtinvoiceno.TextChanged
        check_validation()
    End Sub




    Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.ServerClick
        insert()
    End Sub
    Protected Sub btnSearch_ServerClick(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.ServerClick
        bindinginvrcv(DdlSup.SelectedValue, txtSearch.Text)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Pop", "openModalItem();", True)
    End Sub
    Protected Sub DgvRcv_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles DgvRcv.RowUpdating

    End Sub
    Protected Sub btnClearSearch_Click(sender As Object, e As EventArgs)
        For Each item As ListViewItem In LsvInvRcv.Items
            Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox1"), CheckBox)

            ck.Checked = False


        Next
        txtSearch.Text = ""
        lblnoitem.Text = 0
        'BindData()
        bindinginvrcv(DdlSup.SelectedValue)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Pop", "openModalItem();", True)
    End Sub
    Private Sub ShowToastr(ByVal page As Page, ByVal message As String, ByVal title As String, Optional ByVal type As String = "info", Optional ByVal clearToast As Boolean = False, Optional ByVal pos As String = "toast-top-left", Optional ByVal Sticky As Boolean = False)
        Dim toastrScript As String = [String].Format("Notify('{0}','{1}','{2}', '{3}', '{4}', '{5}');", message, title, type, clearToast, pos, Sticky)
        page.ClientScript.RegisterStartupScript(page.[GetType](), "toastr_message", toastrScript, addScriptTags:=True)
    End Sub

    'Sub bindingstk()

    '    If custid() = 0 Then
    '        Response.Redirect("~/Default.aspx")
    '    Else
    '        Dim dt, dt2 As New DataTable
    '        dt = DirectCast(ViewState("InvRcv"), DataTable)

    '        Debug.Print(dt.Rows.Count.ToString)
    '        'dt2 = clsStk.SelectDataTable_Stk_wStock(dt, custid, txtSearch.Text, ddlFrom.SelectedValue)
    '        'ViewState("InvStk") = dt2
    '        'LsvStk.DataSource = DirectCast(ViewState("InvStk"), DataTable)
    '        'LsvStk.DataBind()

    '        dt2 = DirectCast(ViewState("InvRcv_get"), DataTable)

    '        If dt2.Rows.Count = 0 Then
    '            dt2 = cInvInvoice.Selectrcv_invoice_col(custid, DdlSup.SelectedValue).Tables(0)
    '            ViewState("InvRcv_get") = dt2
    '        Else



    '            dt2.DefaultView.RowFilter = "id like '%" & txtSearch.Text & "%'"
    '            ViewState("InvRcv_get") = dt2
    '        End If

    '        If dt2.Rows.Count > 0 Then
    '            LsvInvRcv.Items.Clear()
    '            countlvstore = dt2.Rows.Count
    '            LsvInvRcv.DataSource = dt2.DefaultView
    '            LsvInvRcv.DataBind()
    '            UpdatePanel3.Update()

    '        End If

    '    End If


    'End Sub
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
    <Serializable>
    Public Class Invoice_Detail 'Class untuk ListInvoice
        Public Property Invoiceid As String = ""
        Public Property Status As Boolean = False
    End Class
    Private Sub BindCheckItem()
        Try

            If CBool(IIf(IsNothing(ViewState("ListItem")), True, False)) Then


                Dim ListItem2 As List(Of Invoice_Detail) = New List(Of Invoice_Detail)()

                Dim dtrcv, dtrcv_get As New DataTable
                dtrcv = DirectCast(ViewState("InvRcv"), DataTable)
                dtrcv_get = cInvInvoice.Selectrcv_invoice_col(custid, DdlSup.SelectedValue).Tables(0)

                If dtrcv_get.Rows.Count > 0 Then
                    For Each dt As DataRow In dtrcv_get.Rows
                        ListItem2.Add(New Invoice_Detail With {
                                           .Invoiceid = dt.Item("id"),
                                           .Status = False
                                           })
                    Next
                End If


                For Each item As ListViewItem In LsvInvRcv.Items
                    Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox1"), CheckBox)
                    Dim Labels As Label = DirectCast(item.FindControl("lblID"), Label)
                    Dim label2 As String = Labels.Text



                    If ck.Checked Then
                        For Each a In ListItem2
                            If a.Invoiceid = label2 Then
                                a.Status = True
                            End If
                        Next
                        '  clsCheck.IDBrand.Add(label2)
                    End If
                Next
                ViewState("ListItem") = ListItem2

            Else

                Dim ListItem2 As List(Of Invoice_Detail) = TryCast(ViewState("ListItem"), List(Of Invoice_Detail))

                For Each item As ListViewItem In LsvInvRcv.Items
                    Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox1"), CheckBox)
                    Dim Labels As Label = DirectCast(item.FindControl("lblID"), Label)
                    Dim label2 As String = Labels.Text

                    If ck.Checked Then
                        For Each a In ListItem2
                            If a.Invoiceid = label2 Then
                                a.Status = True
                            End If
                        Next

                    Else
                        For Each a In ListItem2
                            If a.Invoiceid = label2 Then
                                a.Status = False
                            End If
                        Next
                    End If
                Next
                ViewState("ListItem") = ListItem2
            End If
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub DdlSup_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles DdlSup.SelectedIndexChanged
        check_validation()
        bindinginvrcv(DdlSup.SelectedValue)
    End Sub
    Private Sub CheckedItem()
        Try


            Dim ListStore As List(Of Invoice_Detail) = TryCast(ViewState("ListItem"), List(Of Invoice_Detail))

            Dim i As Integer = 0
            For Each item As ListViewItem In LsvInvRcv.Items
                Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox1"), CheckBox)
                Dim Labels As Label = DirectCast(item.FindControl("lblID"), Label)
                Dim label2 As String = Labels.Text

                For Each a In ListStore
                    If a.Invoiceid = label2 Then
                        If a.Status = True Then
                            ck.Checked = True
                        Else
                            ck.Checked = False
                        End If

                    End If
                Next

            Next
            ViewState("ListItem") = ListStore
        Catch ex As Exception

        End Try
    End Sub

    Sub delete(ByVal id As Integer)
        'WService.DeleteInvRcv(id, cust, username)
        'cInv.DeleteInvRcvDetail(id, custid, username)
    End Sub
End Class
