Imports System

Public Class Pages_CPO
    Inherits System.Web.UI.Page
    Private mfunction As New ModFunction
    'Private wType As String
    Private clsInv As New clsInventory
    Private cPO As New Cls_PO
    'Public Function modulepriv() As Integer
    '    Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
    '    If cookie Is Nothing Then

    '        Response.Redirect("default.aspx")
    '    End If
    '    Return cookie("modulepriv")




    'End Function
    'Public Function custid() As Integer
    '    Dim icust As Integer = 0
    '    Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")

    '    If cookie Is Nothing Then
    '        Response.Redirect("~/Default.aspx")

    '    Else
    '        icust = cookie("custid")
    '    End If
    '    Return icust
    'End Function
    'Public Function username() As String
    '    Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
    '    If cookie Is Nothing Then
    '        Response.Redirect("default.aspx")
    '    End If
    '    Return cookie("userName")

    'End Function
    'Public Function rptpriv() As Integer
    '    Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
    '    If cookie Is Nothing Then

    '        Response.Redirect("default.aspx")
    '    End If
    '    Return cookie("rptpriv")
    '    'Return cookie("modul")
    '    'Return cookie("rptf")


    'End Function


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'If Not (User.Identity.IsAuthenticated) Or custid() = -1 Then
        '    FormsAuthentication.RedirectToLoginPage()
        '    Exit Sub
        'End If
        If Session("username") Is Nothing Or mHelper.CustID() < 1 Then
            Session.Clear()
            Session.Abandon()
            Session.RemoveAll()
            FormsAuthentication.RedirectToLoginPage()
            Exit Sub
        End If
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        If Not cookie Is Nothing Then

            If Not Me.IsPostBack Then
                txtDate.Text = Format(Now.AddDays(-7), "dd MMM yyyy")
                txttoDate.Text = Format(Now, "dd MMM yyyy")
                'txtDate.Text = Format(CDate(Date.Now), "MM/dd/yyyy")
                binding()

            End If
            'bram 5April2024
            Dim msg As String = ""
            msg = "" 'orderasap().ToString
            If msg <> "" Then
                'messagePilihSatu(msg & "!")
                'getlatepost.Replace("\n", Environment.NewLine)
                alert1.Text = orderasap.ToString.Replace("\n", Environment.NewLine)
                xheight = ((orderasap.ToString).Split("\n").Length - 1) * 30
                alert1.Attributes.Add("style", "height:" & xheight & "px" & ";Z-INDEX: 101; OVERFLOW: hidden; color:red; ")

                'alert1.Attributes.Add("style", "height:" & (alert1.Text.Split("\n").Length - 1) * 10 & ";")
                alert1.ReadOnly = True

                alert1.Visible = True
            Else
                alert1.Visible = False
                alert1.Attributes.Add("style", "display:none; ")
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
        End If
        'GetBackground
        binding()
    End Sub
    Sub binding()

        If mHelper.CustID() = 0 Then
            Response.Redirect("~/Default.aspx", False)
        Else
            'Dim ichk As Integer

            'If chkDate.Checked Then
            '    ichk = 1
            'Else
            '    ichk = 2
            'End If

            Dim dt As New DataTable
            'ds = clsInv.getInvRcv(Convert.ToDateTime(txtDate.Text), Convert.ToDateTime(txttoDate.Text), mfunction.CustID(), mfunction.Username, mfunction.ModulePriv)
            dt = cPO.gtmaster_preorder("", mHelper.CustID)
            Dim dv As New DataView(dt)
            dv.Sort = "TransDate DESC, Number DESC"

            ViewState("vspo") = dv.ToTable()

            lstViewDetails.DataSource = ViewState("vspo")
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
            Dim dtcpo As New DataTable
            If ViewState("vspo") IsNot Nothing Then
                dtcpo = DirectCast(ViewState("vspo"), DataTable)
            End If


            If dtcpo.Rows.Count = 0 Then
                    Dim dt As New DataTable
                ' dtstore2 = clsExport.SelectDataset_Outlet2(ddlOwn.SelectedValue, lblnobrand.Text, lblnoCity.Text, icustid, txtSearch.Text, sNameuser, iModPriv).Tables(0)
                dt = cPO.preorder_search(txtsearch.Text, Convert.ToDateTime(txtDate.Text), Convert.ToDateTime(txttoDate.Text), mHelper.CustID(), mHelper.Username(), mHelper.ModulePriv())
                dtcpo = dt
                'Dim dv As New DataView(dt)
                dtcpo.DefaultView.Sort = "TransDate DESC, Number DESC"
                ViewState("vspo") = dtcpo.DefaultView.ToTable()
            Else
                Dim custId As Integer = mHelper.CustID()
                Dim fromDate As DateTime = Convert.ToDateTime(txtDate.Text)
                    Dim toDate As DateTime = Convert.ToDateTime(txttoDate.Text)
                    Dim keyword As String = txtsearch.Text.Replace("'", "''")

                    dtcpo.DefaultView.RowFilter =
        "custid = '" & custId & "' " &
        "AND transdate >= #" & fromDate.ToString("yyyy-MM-dd 00:00:00") & "# " &
        "AND transdate <= #" & toDate.ToString("yyyy-MM-dd 23:59:59") & "# " &
        "AND (" &
            "Number LIKE '%" & keyword & "%' " &
            "OR custname LIKE '%" & keyword & "%' " &
            "OR olname LIKE '%" & keyword & "%'" &
        ")"

                dtcpo.DefaultView.Sort = "TransDate DESC, Number DESC"

                ViewState("vspo") = dtcpo.DefaultView.ToTable()
                End If

                If dtcpo.Rows.Count > 0 Then
                    lstViewDetails.Items.Clear()
                    countlvstore = dtcpo.Rows.Count
                    lstViewDetails.DataSource = ViewState("vspo") 'dtcpo.DefaultView
                    lstViewDetails.DataBind()
                    'UpdatePanel3.Update()

                End If

            End If

    End Sub
    Sub delete(ByVal id As String)
        ' WService.DeleteInvRcv(id, cust, username)
        If (Now.Hour >= 23 And Now.Minute >= 30) Or (Now.Hour = 0 And Now.Minute <= 30) Then
            notsuccesfull("Please process the transaction after 1.00 AM")
            Exit Sub
        End If
        clsInv.DeleteInvRcv(id, mHelper.CustID, mHelper.Username)
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

                    If iflag = 2 And dCreate.Date < Now.Date And (mHelper.ModulePriv() And usrModulepriv.moduleuser = usrModulepriv.moduleuser) Then
                        ' lblError.Text = "Receiving no " & id & " cannot be delete, because already receive "
                        notsuccesfull("Receiving no " & id & " cannot be delete, because already receive ")
                        Exit Sub
                    ElseIf iflag = 2 And dCreate.Date < Format(Now.AddDays(-7), "dd MMM yyyy") Then
                        notsuccesfull("Receiving no " & id & " cannot be delete, because received 1 week ago ")
                        Exit Sub
                    End If

                    Dim confirmValue As String = Request.Form("confirm_value")
                    If confirmValue = "Yes" Then

                        If c_cmd.ExecuteScalar("select count(custid) from cld_inv_closing where custid = " & mHelper.CustID() & " and invdate = '" & Date.Now.Date.ToString("yyyy-MM-dd") & "' and  invflag = 0 ", 0) = 1 Then
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
                    mfunction.CreateCookie("Number", arg(0))
                    mfunction.CreateCookie("olid", arg(1))
                    'mfunction.CreateCookie("Payno", arg(2))
                    mfunction.CreateCookie("Ordercust", arg(2))
                    mfunction.CreateCookie("flag", arg(3))


                    If IsDate(arg(4)) = True Then
                        mfunction.CreateCookie("Transdate", arg(4))
                    Else
                        mfunction.CreateCookie("Transdate", "")
                    End If
                    If IsDate(arg(5)) = True Then
                        mfunction.CreateCookie("busidate", arg(5))
                    Else
                        mfunction.CreateCookie("busidate", "")
                    End If

                    Response.Redirect("~/Pages_CPOEdit.aspx", False)

                Case "Print"


                    Dim arg As String() = e.CommandArgument.ToString().Split(";")
                    Dim smno As String = arg(0)
                    Dim olid As String = arg(1)


                    Dim confirmValue As String = Request.Form("confirm_value")
                    If confirmValue = "Yes" Then




                        mfunction.CreateCookie("CPOID", smno)
                        mfunction.CreateCookie("olid", olid)
                        Session("CPOID") = smno
                        Session("olid") = olid
                        Response.Redirect("~/printprev_CPO.aspx", False)

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
    Protected Function GetBackgroundu1(ByVal id As Object) As String
        Dim rcvid As String = Convert.ToString(id)
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
            mfunction.CreateCookie("Number", "")
            mfunction.CreateCookie("olid", 0)

            mfunction.CreateCookie("Ordercust", 0)
            mfunction.CreateCookie("flag", 0)



            mfunction.CreateCookie("Transdate", CDate(Now))

            mfunction.CreateCookie("busidate", CDate(Now))

            Response.Redirect("~/Pages_CPOEdit.aspx", False)
        Catch ex As Exception
            errLog(Err.Description, "Pages_InvRcv:btnNew_Click")
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
        If txtsearch.Text = "" Then
            ViewState("vspo") = Nothing
        End If


        binding2()
    End Sub

    'Protected Function GetBackground() As String
    '    Dim imgstat As String
    '    If txtDate.Text <> Format(Now, "dd MMM yyyy") Then
    '        imgstat = "nodisplay"
    '    Else
    '        imgstat = "displayaction"

    '    End If

    '    Return imgstat
    'End Function
    Protected Function GetBackground2(ByVal id As Object) As String
        Dim rcvid As String = Convert.ToString(id)
        Dim imgstat As String
        Dim cinvrcv As New clsInventory_Receive
        If cinvrcv.rcv_date(rcvid) = 0 Then
            imgstat = "nodisplay"
        Else
            imgstat = "displayaction"
        End If

        Return imgstat

    End Function

    Private Sub txttoDate_TextChanged(sender As Object, e As EventArgs) Handles txttoDate.TextChanged
        If txttoDate.Text = "" Then
            txttoDate.Text = Format(Now, "dd MMM yyyy")
        End If
    End Sub
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
