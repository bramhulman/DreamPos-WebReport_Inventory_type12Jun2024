Imports NPOI.SS.Formula.Functions

Public Class Pages_Inv_Cnvr
    Inherits System.Web.UI.Page
    Private mfunction As New ModFunction
    'Private wType As String
    Private clsInv As New clsInventory
    ' Private clsCnvers As New Cls_Conversion
    Private CBatch As New Cls_IBatch
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
                txtDate.Text = Format(DateTime.Now, "dd MMM yyyy")
                'txttoDate.Text = Format(Now, "dd MMM yyyy")
                'txtDate.Text = Format(CDate(Date.Now), "MM/dd/yyyy")
                binding()

            End If
            'bram 5April2024

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
            txtDate.Text = Format(DateTime.Now, "dd MMM yyyy")
        End If
        'GetBackground
        binding()
    End Sub
    Sub binding()

        If custid() = 0 Then
            Response.Redirect("~/Default.aspx", False)
        Else
            'Dim ichk As Integer

            'If chkDate.Checked Then
            '    ichk = 1
            'Else
            '    ichk = 2
            'End If

            Dim ds As New DataSet
            ds = CBatch.SelectDataset_invBatch(Convert.ToDateTime(txtDate.Text), custid, modulepriv, username)
            ViewState("vsconversion") = ds.Tables(0)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 Then
                lstViewDetails.DataSource = ds
            Else
                lstViewDetails.DataSource = Nothing
            End If
            lstViewDetails.DataBind()
        End If

    End Sub
    Sub binding2()

        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")

        If cookie("Custid") = 0 Or cookie("Custid") Is Nothing Then
            errLog("icustid = 0", "cookienothing" & "_reportinventory")
            Response.Redirect("~/Default.aspx", False)
        Else

            'Bram4Sep2018 Update Report
            Dim dtconversion As DataTable = DirectCast(ViewState("vsconversion"), DataTable)

            If dtconversion.Rows.Count = 0 Then
                ' dtstore2 = clsExport.SelectDataset_Outlet2(ddlOwn.SelectedValue, lblnobrand.Text, lblnoCity.Text, icustid, txtSearch.Text, sNameuser, iModPriv).Tables(0)
                dtconversion = CBatch.SelectDataset_invBatchSearch(txtsearch.Text, Convert.ToDateTime(txtDate.Text), custid, modulepriv, username).Tables(0)
                ViewState("vsconversion") = dtconversion
            Else



                dtconversion.DefaultView.RowFilter = "custid = " & custid() & " and invdate='" & Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd 00:00:00") & "' And (linkID Like  '%" & txtsearch.Text & "%' Or Stk_Name like '%" & txtsearch.Text & "%')"
                ViewState("vsconversion") = dtconversion
            End If

            If dtconversion.Rows.Count > 0 Then
                lstViewDetails.Items.Clear()
                countlvstore = dtconversion.Rows.Count
                lstViewDetails.DataSource = dtconversion.DefaultView
                lstViewDetails.DataBind()
                'UpdatePanel3.Update()

            End If

        End If

    End Sub
    Sub delete(ByVal id As String)
        ' WService.DeleteInvRcv(id, cust, username)
        clsInv.DeleteInvRcv(id, custid, username)
        binding()
    End Sub
    Protected Sub lstView_ItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ListViewCommandEventArgs) Handles lstViewDetails.ItemCommand
        Dim cinvrcv As New clsInventory_Receive
        Dim c_cmd As New clsDbo("")
        Try
            Select Case (e.CommandName)
                Case "Del"
                    'Dim id As Integer = Convert.ToInt32(e.CommandArgument)

                    'delete(id)

                    Dim arg As String() = e.CommandArgument.ToString().Split(";")
                    Dim id As String = arg(0)
                    Dim iflag As Integer = arg(1)
                    Dim dCreate As Date

                    If IsDate(arg(2)) = True Then
                        dCreate = arg(2)
                    End If

                    If iflag = 2 And dCreate.Date < DateTime.Now.Date And (modulepriv() And usrModulepriv.moduleuser = usrModulepriv.moduleuser) Then
                        ' lblError.Text = "Receiving no " & id & " cannot be delete, because already receive "
                        notsuccesfull("Receiving no " & id & " cannot be delete, because already receive ")
                        Exit Sub
                    ElseIf iflag = 2 And dCreate.Date < Format(datetime.Now.AddDays(-7), "dd MMM yyyy") Then
                        notsuccesfull("Receiving no " & id & " cannot be delete, because received 1 week ago ")
                        Exit Sub
                    End If

                    Dim confirmValue As String = Request.Form("confirm_value")
                    If confirmValue = "Yes" Then

                        If c_cmd.ExecuteScalar("select count(custid) from cld_inv_closing where custid = " & custid() & " and invdate = '" & DateTime.Now.Date.ToString("yyyy-MM-dd") & "' and  invflag = 0 ", 0) = 1 Then
                            'FormsAuthentication.SetAuthCookie(username, False)
                            Session.Clear()
                            Session.Abandon()
                            Session.RemoveAll()


                            messagesuccesfull("Closing process,Please login again.", False)
                            FormsAuthentication.RedirectToLoginPage()
                            Exit Sub
                        Else
                            delete(id)
                        End If


                    Else
                        Exit Sub
                    End If

                Case "Edt"
                    Dim arg As String() = e.CommandArgument.ToString().Split(";")
                    mfunction.CreateCookie("iBatchID", arg(0))
                    mfunction.CreateCookie("Cc", arg(1))
                    mfunction.CreateCookie("stk_id", arg(2))


                    Response.Redirect("~/Pages_Inv_CnvrEdit.aspx", False)

                Case "Send"


                    Dim arg As String() = e.CommandArgument.ToString().Split(";")
                    Dim id As String = arg(0)


                    Dim confirmValue As String = Request.Form("confirm_value")
                    If confirmValue = "Yes" Then




                        mfunction.CreateCookie("rcvID", id)
                        mfunction.CreateCookie("xunit1", 0)
                        Response.Redirect("~/printprev_supp.aspx", False)

                    Else
                        Exit Sub
                    End If
                Case "Send2"

                    Dim arg As String() = e.CommandArgument.ToString().Split(";")
                    Dim id As String = arg(0)



                    Dim confirmValue As String = Request.Form("confirm_value")
                    If confirmValue = "Yes" Then

                        'If cinvrcv.u1_invorderrcv(id) > 0 Then
                        '    Dim myStringVariable As String = "can't print only unit 1"
                        '    ClientScript.RegisterStartupScript(Me.GetType(), "myalert", "alert('" + myStringVariable + "');")

                        '    'notsuccesfull("can't print only unit 1")
                        '    Exit Sub
                        'End If

                        mfunction.CreateCookie("rcvID", id)
                        mfunction.CreateCookie("xunit1", 1)
                        mfunction.CreateCookie("from", "~/Pages_InvOrderWHO.aspx")
                        Response.Redirect("~/printprev_supp.aspx", False)

                    Else
                        Exit Sub
                    End If

            End Select


        Catch ex As Exception
            errLog(Err.Description, "Pages_InvRcv:ItemCommand")
            Err.Clear()
        End Try
    End Sub
    Sub messagesuccesfull(ByVal msg As String, Optional ByVal bsuccess As Boolean = True)

        Dim message As String = msg
        Dim url As String
        If bsuccess Then
            url = "../Pages_InvRcv.aspx"
        Else
            url = "Default.aspx"
        End If

        Dim script As String = "window.onload = function(){ alert('"
        script += message
        script += "');"
        script += "window.location = '"
        script += url
        script += "'; }"
        ClientScript.RegisterStartupScript(Me.GetType(), "Redirect", script, bsuccess)

    End Sub
    Protected Function GetBackgroundu1(ByVal rcvid As String)
        Dim imgstat As String
        Dim cinvrcv As New clsInventory_Receive
        If cinvrcv.u1_invorderrcv(rcvid) > 0 Then
            imgstat = "nodisplay"
        Else
            imgstat = "displayaction"
        End If

        Return imgstat
    End Function

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
            mfunction.CreateCookie("iBatchID", 0)
            mfunction.CreateCookie("Createddate", "")
            mfunction.CreateCookie("cc", 0)
            mfunction.CreateCookie("stk_id", 0)
            'mfunction.CreateCookie("rcvID", "")
            'mfunction.CreateCookie("rcvCc", 0)
            'mfunction.CreateCookie("rcvSupid", 0)
            'mfunction.CreateCookie("rcvDO", "")
            'mfunction.CreateCookie("rcvflag", 0)
            'mfunction.CreateCookie("Createddate", "")
            'mfunction.CreateCookie("modifdate2", "")
            'mfunction.CreateCookie("acc2", "")
            Response.Redirect("~/Pages_Inv_CnvrEdit.aspx", False)
        Catch ex As Exception
            errLog(Err.Description, "Pages_Inv_Cnvr:btnNew_Click")
            Err.Clear()
        End Try
    End Sub
    Protected Sub OnPagePropertiesChanging(ByVal sender As Object, ByVal e As PagePropertiesChangingEventArgs)
        lvDataPager1.SetPageProperties(e.StartRowIndex, e.MaximumRows, False)
        binding2()
    End Sub
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.ServerClick

        ' Dim Cust As Integer = cookie("CustID")
        ' Dim ds As New DataSet
        'Dim ichk As Integer

        'If chkDate.Checked Then
        '    ichk = 1
        'Else
        '    ichk = 2
        'End If

        'ds = clsInv.getInvRcvSearch(txtsearch.Text, Convert.ToDateTime(txtDate.Text), Convert.ToDateTime(txttoDate.Text), custid, username, modulepriv)

        'lstViewDetails.DataSource = ds
        'lstViewDetails.DataBind()
        binding2()
    End Sub

    Protected Function GetBackground()
        Dim imgstat As String
        If txtDate.Text <> Format(DateTime.Now.AddDays(-7), "dd MMM yyyy") Then
            imgstat = "nodisplay"
        Else
            imgstat = "displayaction"

        End If

        Return imgstat
    End Function
    Protected Function GetBackground2(ByVal rcvid As String)

        Dim imgstat As String
        Dim cinvrcv As New clsInventory_Receive
        If cinvrcv.rcv_date(rcvid) = 0 Then
            imgstat = "nodisplay"
        Else
            imgstat = "displayaction"
        End If

        Return imgstat

    End Function

    'Private Sub txttoDate_TextChanged(sender As Object, e As EventArgs) Handles txttoDate.TextChanged
    '    If txttoDate.Text = "" Then
    '        txttoDate.Text = Format(Now, "dd MMM yyyy")
    '    End If
    'End Sub
    'Protected Sub chkDate_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkDate.CheckedChanged

    '    If chkDate.Checked = True Then
    '        txtDate.ReadOnly = False
    '        txtDate.Enabled = True
    '    ElseIf chkDate.Checked = False Then
    '        txtDate.ReadOnly = True
    '        txtDate.Enabled = False
    '    End If

    '    Dim ichk As Integer

    '    If chkDate.Checked Then
    '        ichk = 1
    '    Else
    '        ichk = 2
    '    End If

    '    Dim ds As New DataSet
    '    ds = clsInv.getInvRcv(Convert.ToDateTime(txtDate.Text), custid, ichk, username, modulepriv)
    '    lstViewDetails.DataSource = ds
    '    lstViewDetails.DataBind()

    'End Sub
    Protected Function orderasap()

        Dim msg As String = ""

        Dim newLine As String = "\n"
        Dim dtpost As New DataSet
        Dim c_cmd As New Cls_command
        Try
            dtpost = c_cmd.ReturnDataset("
SELECT stk_id, (SELECT stkno FROM `cld_inv_transstockcls` WHERE ccid=2 AND stkid=dtl.stk_id AND invdate = CURDATE()) stocknum,stk_name,
round(SUM(totalu2)) torder,(SELECT round(ttlqty) FROM `cld_inv_transstockcls` WHERE ccid=2 AND stkid=dtl.stk_id AND invdate = CURDATE()) stocknow
 FROM `cld_inv_w_dtl` dtl WHERE wid IN (SELECT id FROM `cld_inv_w` WHERE flag = 1 ) GROUP BY stk_id
 HAVING torder>stocknow")
            If dtpost.Tables(0).Rows.Count > 0 Then
                For Each row As DataRow In dtpost.Tables(0).Rows
                    Dim kurang As Double = row.Item("torder") - row.Item("stocknow")
                    '    msg = msg + row.Item("stocknum") + " " + row.Item("stk_name") + " order from Outlet " + row.Item("torder").ToString + " stock " + row.Item("stocknow").ToString + " backorder " + kurang.ToString & newLine
                    msg = msg + row.Item("stk_name") + " Req: " + row.Item("torder").ToString + " stk: " + row.Item("stocknow").ToString + " backorder " + kurang.ToString & newLine

                Next
            End If
        Catch ex As Exception

        End Try

        Return msg
    End Function


End Class
