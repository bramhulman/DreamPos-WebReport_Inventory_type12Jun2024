Imports System.Threading
Imports System.Net
Imports System.Text
Imports System.IO
Imports System.Data
Imports System.Drawing

Public Class Pages_InvOrderWDetailRcv
    Inherits System.Web.UI.Page
    Private sNameuser As String
    Private Userid As Integer
    Private icustid As Integer
    Private imodul As Integer
    Private iModPriv As Integer
    Private irptpriv As Integer
    Private irptf As Integer
    Private mfunction As New ModFunction
    Private wID As String
    Private wCc As Integer
    Private wFrom As Integer
    Private wDelID As String
    Private wStatus As Date
    Private wDel As Date
    Private wCreate As Date
    Private wRcv As Date
    Private wFinish As String
    Private wNotes As String

    Private clsInv As New clsInventory
    Private clsStk As New clsInventory_stk


    'Private transpriv As Integer
    Public Function modulepriv() As Integer
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")

        If cookie Is Nothing Then
            Return -1
        Else
            Return cookie("modulepriv")
        End If
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


        If cookie Is Nothing Then
            Return -1
        Else
            Return cookie("rptpriv")
        End If
    End Function
    Public Function modul() As Integer
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")

        If cookie Is Nothing Then
            Return -1
        Else
            Return cookie("modul")
        End If
    End Function
    Public Function rptf() As Integer
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")


        If cookie Is Nothing Then
            Return -1
        Else
            Return cookie("rptf")
        End If
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
            wID = Request.Cookies("wID").Value
            wCc = Request.Cookies("wCc").Value
            wFrom = Request.Cookies("wFrom").Value
            If Request.Cookies("wDelID") IsNot Nothing Then
                wDelID = Request.Cookies("wDelID").Value
            Else
                wDelID = ""
            End If
            'wNotes = Request.Cookies("wNotes").Value


            wFinish = Request.Cookies("wfinish").Value

            If Request.Cookies("UserLogin_inv") IsNot Nothing Then
                sNameuser = username()
                icustid = custid()
                imodul = modul()
                iModPriv = modulepriv()
                irptpriv = rptpriv()
                irptf = rptf()
                cle.StartDate = Now.AddDays(-1)
                cle.EndDate = Now.Date

                If IsPostBack = False Then

                    If IsDate(Request.Cookies("wstatDate").Value) Then
                        wStatus = Request.Cookies("wstatDate").Value
                    End If


                    If IsDate(Request.Cookies("wDelDate").Value) Then
                        wDel = (Request.Cookies("wDelDate").Value)
                        txtexpD1.Text = Format(wDel, "dd MMM yyyy")

                        Deliverydate.Visible = True
                        txtexpD1.Enabled = False
                        receivedate.Visible = True
                        txtrcvdate1.Text = Format(Now, "dd MMM yyyy")
                    End If

                    If IsDate(Request.Cookies("wCreatedDate").Value) Then
                        wCreate = (Request.Cookies("wCreatedDate").Value)
                        txtcreatedate1.Text = Format(wCreate, "dd MMM yyyy")
                        Createdate.Visible = True
                        txtcreatedate1.Enabled = False
                    End If

                    If IsDate(Request.Cookies("wrcvDate").Value) Then
                        wRcv = (Request.Cookies("wrcvDate").Value)
                        txtrcvdate1.Text = Format(wRcv, "dd MMM yyyy")

                        receivedate.Visible = True
                        txtrcvdate1.Enabled = True
                    End If

                    'txtexpD.Text = Format(Now, "dd MMM yyyy")
                    loadFrom()
                    loadwarehouse()

                    If wID = "" Then

                        txtID.Text = ""
                        txtID.ReadOnly = True
                        Dim dt, dt2 As New DataTable
                        dt = clsInv.getDTInvWare(wID, custid, 0, 0)
                        ViewState("orderW") = dt
                        ddlFrom.Enabled = False

                        ddlWare.Enabled = False
                        BindData()


                    Else

                        txtID.Text = wID
                        txtID.ReadOnly = True

                        ddlFrom.SelectedValue = wCc
                        ddlFrom.Enabled = False

                        ddlWare.SelectedValue = wFrom
                        ddlWare.Enabled = False

                        'If IsDate(wStatus) Then
                        '    txtexpD.Text = Format(wStatus, "dd MMM yyyy")
                        'Else
                        '    txtexpD.Text = Format(Now, "dd MMM yyyy")
                        'End If
                        'txtexpD.Enabled = False

                        If wFinish = 1 Then
                            'DgvTransfer.Columns(7).Visible = False
                            'btnFinish.Enabled = False
                            'btnSetAll.Enabled = False
                            btnFinish.Disabled = True
                            btnSetAll.Disabled = True
                            'Label3.Text = "Receive Date:"
                            txtexpD1.Enabled = False
                            'If Not (modulepriv() = usrModulepriv.moduleuser) And wStatus.Date > Now.Date.AddDays(-7) Then
                            If (username() = My.Settings.admin Or username() = "support") And wStatus.Date > Now.Date.AddDays(My.Settings.limitday * -1) Then
                                btncancel.Visible = True
                            End If

                            'End If
                        Else
                            ' Label3.Text = "Delivery Date:"

                        End If

                        LoadReceiveDelMode()

                    End If


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




            'yestr = CDate(Now.AddDays(-1)).ToString("yyyyMMdd")

        Catch ex As Exception
            errLog(Err.Description, "Pages_InvOrderWDetailRcv:Page_Load")
            Err.Clear()
        End Try
    End Sub
    Sub loadFrom()
        Dim ds As New DataSet
        ds = clsInv.getIccOnly(custid, username, modulepriv)
        If ds IsNot Nothing AndAlso ds.Tables.Count > 0 Then
            ddlFrom.DataSource = ds
        Else
            ddlFrom.DataSource = Nothing
        End If
        ddlFrom.DataValueField = "Id"
        ddlFrom.DataTextField = "Name"
        ddlFrom.DataBind()

        If ddlFrom.Items.Count > 0 Then
            ddlFrom.SelectedIndex = 0
        End If

    End Sub
    Sub loadwarehouse()
        Dim ds As New DataSet
        ds = clsInv.getIWareOnly(custid)
        If ds IsNot Nothing AndAlso ds.Tables.Count > 0 Then
            ddlWare.DataSource = ds
        Else
            ddlWare.DataSource = Nothing
        End If
        ddlWare.DataValueField = "Id"
        ddlWare.DataTextField = "Name"
        ddlWare.DataBind()

        If ddlWare.Items.Count > 0 Then
            ddlWare.SelectedIndex = 0
        End If

    End Sub
    Protected Sub BindData()
        If custid() = 0 Then
            Response.Redirect("~/Default.aspx")
        Else

            DgvTransfer.DataSource = DirectCast(ViewState("orderW"), DataTable)
            DgvTransfer.DataBind()
            'DgvTransfer.


        End If
    End Sub

    Private Sub LoadReceiveDelMode()
        Dim delMode As String = clsInv.getInvOrderDelMode(wID, custid(), ddlFrom.SelectedValue)
        If delMode = "P" Then
            pnlPickDel.Visible = True
            Dim pending = clsInv.getInvWarePendingDel(wID, custid())
            ddlPendingDel.Items.Clear()
            ddlPendingDel.Items.Add(New ListItem("-- Select delivery note --", ""))
            For Each row As DataRow In pending.Rows
                ddlPendingDel.Items.Add(New ListItem(row.Item("DelID").ToString() & " (" & row.Item("DelDateDisp").ToString() & ")", row.Item("DelID").ToString()))
            Next
            If wDelID <> "" Then
                If ddlPendingDel.Items.FindByValue(wDelID) Is Nothing Then
                    ddlPendingDel.Items.Add(New ListItem(wDelID & " (selected)", wDelID))
                End If
                ddlPendingDel.SelectedValue = wDelID
                LoadDelReceiveGrid(ddlPendingDel.SelectedValue)
            ElseIf pending.Rows.Count = 1 Then
                ddlPendingDel.SelectedValue = pending.Rows(0).Item("DelID").ToString()
                LoadDelReceiveGrid(ddlPendingDel.SelectedValue)
            Else
                ViewState("orderW") = clsInv.getDTInvWare(wID, custid, ddlFrom.SelectedValue, ddlWare.SelectedValue)
                ViewState("selectedDelID") = ""
                BindData()
            End If
        Else
            pnlPickDel.Visible = False
            Dim dt = clsInv.getDTInvWare(wID, custid, ddlFrom.SelectedValue, ddlWare.SelectedValue)
            ViewState("orderW") = dt
            ViewState("selectedDelID") = ""
            BindData()
        End If
    End Sub

    Private Sub LoadDelReceiveGrid(ByVal delId As String)
        ViewState("selectedDelID") = delId
        Dim dt = clsInv.getInvWareDelDtl(delId, custid(), ddlFrom.SelectedValue, ddlWare.SelectedValue)
        ViewState("orderW") = dt
        BindData()
    End Sub

    Protected Sub ddlPendingDel_SelectedIndexChanged(sender As Object, e As EventArgs)
        If ddlPendingDel.SelectedValue <> "" Then
            LoadDelReceiveGrid(ddlPendingDel.SelectedValue)
        End If
    End Sub

    Protected Sub btnPrintDel_ServerClick(sender As Object, e As EventArgs)
        If ddlPendingDel.SelectedValue = "" Then
            notsuccesfull("Please select a delivery note first.")
            Exit Sub
        End If

        mfunction.CreateCookie("woID", txtID.Text)
        mfunction.CreateCookie("woPrintDelID", ddlPendingDel.SelectedValue)
        mfunction.CreateCookie("print", "DO")
        mfunction.CreateCookie("from", "~/Pages_InvOrderW.aspx")
        Response.Redirect("~/printprev.aspx", False)
    End Sub


    Protected Sub Page_unLoad(sender As Object, e As EventArgs) Handles Me.Unload
    End Sub

    Public Function username() As String
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        Return cookie("userName")

    End Function






    Sub delete(ByVal id As String, ByVal iccTo As Integer, ByVal istk As Integer)
        clsInv.DeleteInvWare(id, custid, iccTo, istk, username)
    End Sub
    Public Sub DeleteRcpDtl(ByVal id As Integer, ByVal Custid As Integer, ByVal username As String)
        'Dim pc As String = pic.ToString
        Try

            Dim str As String
            Dim c_cmd As New Cls_command

            str = "DELETE FROM cld_inv_rcpdtl  where id = " & id & ""

            c_cmd.executeNonQuery(str)

            'Revisi 10 Mei
            insertLogModule(username, "InvRcpdtl", "Delete", Custid)

        Catch ex As Exception

            errLog(Err.Description, "Pages_ReceipeEdit:DeleteInvrcpdtl")
            Err.Clear()
        End Try
    End Sub


    Protected Sub RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow AndAlso DgvTransfer.EditIndex = e.Row.RowIndex Then

        End If
    End Sub

    'Protected Sub btnFinish_Click(sender As Object, e As EventArgs)
    '    wID = insert(2)
    'End Sub
    Sub insert(ByVal stat As Integer, ByVal Optional xcancel As Integer = 0)
        Try
            Dim msg As String = ""
            Dim dt As DataTable = DirectCast(ViewState("orderW"), DataTable)
            Dim rowcount As Decimal = dt.Rows.Count
            Dim c_cmd As New Cls_command

            If CDate(txtrcvdate1.Text) < CDate(txtexpD1.Text) Then
                msg = msg + "received earlier than delivered" & ":"
            End If
            If dt.Rows.Count = 0 Then
                msg = msg + "Item is Nothing" & ":"
            End If
            If ddlFrom.SelectedValue = 0 Or ddlFrom.SelectedItem.Text = "" Then
                msg = msg + "Cost Centre Must be Choice " & ":"
            End If

            If ddlWare.SelectedValue = 0 Or ddlWare.SelectedItem.Text = "" Then
                msg = msg + "Supplier Must be Choice" & ":"
            End If
            Dim c1 As Decimal = 0
            For Each row As DataRow In dt.Rows
                'If row.Item("U1Rcv") = 0 Then
                '    msg = msg + row.Item("Stk_name") + " rcv qty cannot 0 " & ":"
                'End If
                If row.Item("U1Rcv") = 0 Then
                    c1 = c1 + 1
                End If

            Next row
            If rowcount = c1 Then
                msg = msg + "ALL Receive qty = 0" & ":"
            End If
            If msg <> "" Then
                lblError.Text = msg
                notsuccesfull(msg)
                Exit Sub
            End If

            If c_cmd.ExecuteScalar("select count(custid) from cld_inv_closing where custid = " & custid() & " and invdate = '" & Date.Now.Date.ToString("yyyy-MM-dd") & "' and  invflag = 0 ", 0) = 1 Then
                FormsAuthentication.SetAuthCookie(username, False)
                notsuccesfull("Closing process,Please login again.")
                Exit Sub
            Else
                Try
                    Dim res As String
                    Dim selDel As String = If(ViewState("selectedDelID"), "").ToString()
                    If pnlPickDel.Visible AndAlso selDel <> "" Then
                        res = clsInv.receiveInvOrderWDel(selDel, txtID.Text, custid(), ddlWare.SelectedValue, ddlWare.SelectedItem.ToString,
                            ddlFrom.SelectedValue, ddlFrom.SelectedItem.ToString, username(), CDate(txtrcvdate1.Text), dt)
                    Else
                        res = clsInv.insertInvOrderW(txtID.Text, custid(), 0, 3, ddlWare.SelectedValue, ddlWare.SelectedItem.ToString, ddlFrom.SelectedValue, ddlFrom.SelectedItem.ToString, username, username, CDate(txtrcvdate1.Text), dt, "", "", xcancel)
                    End If
                    If res = "Error" Then
                        notsuccesfull("Transaction Failed. Check logs.")
                        Exit Sub
                    End If
                Catch ex As Exception
                    errLog(txtID.Text & " " & ddlWare.SelectedValue.ToString & " " & ddlWare.SelectedItem.ToString & " " & ddlFrom.SelectedValue.ToString & " " & ddlFrom.SelectedItem.ToString & " " & username() & " " & txtrcvdate1.Text & " " & Err.Description, "Pages_InvOrderWDetail")
                    Err.Clear()
                End Try
            End If

            mfunction.CreateCookie("wType", "2")
            Response.Redirect("~/Pages_InvOrderW.aspx", False)

        Catch ex As Exception
            errLog(Err.Description, "Pages_InvOrderWDetailRcv:insert")
            Err.Clear()
        End Try

    End Sub
    Protected Sub DgvTransfer_RowDeleting(ByVal sender As Object, ByVal e As GridViewDeleteEventArgs)


    End Sub
    Protected Sub DgvTransfer_RowUpdating(ByVal sender As Object, ByVal e As GridViewUpdateEventArgs)

        Dim txtUnit1 As TextBox = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("txtEditUnit1Rcv"), TextBox)
        Dim lbldel1 As Label = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("lblEditUnit1Del"), Label)

        Dim dt As DataTable = DirectCast(ViewState("orderW"), DataTable)

        dt.Rows(e.RowIndex)("U1Rcv") = txtUnit1.Text

        If txtUnit1.Text <= lbldel1.Text Then

            lblError.Text = ""
            dt.Rows(e.RowIndex)("U1Rcv") = txtUnit1.Text

            dt.AcceptChanges()

            DgvTransfer.EditIndex = -1
            BindData()
        Else
            lblError.Text = "receive Qty cannot more than delivery Qty"

        End If

    End Sub
    Protected Sub DgvTransfer_RowCancelingEdit(ByVal sender As Object, ByVal e As GridViewCancelEditEventArgs)
        lblError.Text = ""
        DgvTransfer.EditIndex = -1
        BindData()
    End Sub

    Protected Sub DgvTransfer_RowEditing(ByVal sender As Object, ByVal e As GridViewEditEventArgs)
        DgvTransfer.EditIndex = e.NewEditIndex
        BindData()
    End Sub

    Private Sub ShowToastr(ByVal page As Page, ByVal message As String, ByVal title As String, Optional ByVal type As String = "info", Optional ByVal clearToast As Boolean = False, Optional ByVal pos As String = "toast-top-left", Optional ByVal Sticky As Boolean = False)
        Dim toastrScript As String = [String].Format("Notify('{0}','{1}','{2}', '{3}', '{4}', '{5}');", message, title, type, clearToast, pos, Sticky)
        page.ClientScript.RegisterStartupScript(page.[GetType](), "toastr_message", toastrScript, addScriptTags:=True)
    End Sub
    <Serializable>
    Public Class Item_Detail 'Class untuk ListItem
        Public Property ItemName As String = ""
        Public Property Status As Boolean = False
    End Class
    'Start : Store



    'END : Store





    'Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As EventArgs)
    '    mfunction.CreateCookie("wType", "1")
    '    Response.Redirect("~/Pages_InvOrderW.aspx")
    'End Sub


    Protected Sub btnFinish_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnFinish.ServerClick

        'If CDate(txtexpD.Text) >= Now.AddDays(-1) And CDate(txtexpD.Text) <= Now.Date Then
        'Else
        '    notsuccesfull("please change the date")
        '    Exit Sub
        'End If
        If (Now.Hour >= 23 And Now.Minute >= 30) Or (Now.Hour = 0 And Now.Minute <= 30) Then
            notsuccesfull("Please process the transaction after 1.00 AM")
            Exit Sub
        End If
        insert(2)
        'CreateCookie("woID", wID)

        'Response.Redirect("~/printprev.aspx")
    End Sub

    Protected Sub btnBack_Click(sender As Object, e As EventArgs) 'Handles btnBack.ServerClick
        If wFinish = 1 Then
            mfunction.CreateCookie("wType", "3")
        Else
            mfunction.CreateCookie("wType", "2")
        End If

        Response.Redirect("~/Pages_InvOrderW.aspx", False)
    End Sub
    Protected Sub btnSetAll_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSetAll.ServerClick
        Dim dt As DataTable = DirectCast(ViewState("orderW"), DataTable)

        For Each row As DataRow In dt.Rows
            row.Item("U1Rcv") = row.Item("u1Del")
            dt.AcceptChanges()
        Next row

        BindData()

    End Sub
    Protected Sub btncancel_ServerClick(sender As Object, e As EventArgs) Handles btncancel.ServerClick
        If (Now.Hour >= 23 And Now.Minute >= 30) Or (Now.Hour = 0 And Now.Minute <= 30) Then
            notsuccesfull("Please process the transaction after 1.00 AM")
            Exit Sub
        End If
        If clsInv.get_HOstatus(txtID.Text, ddlFrom.SelectedValue, custid) = 3 Then
            insert(2, 2)
        End If

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
End Class
