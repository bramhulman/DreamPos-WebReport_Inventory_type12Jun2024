Public Class Pages_InvRcv
    Inherits System.Web.UI.Page
    Private mfunction As New ModFunction
    'Private wType As String
    Private clsInv As New clsInventory
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
        If custid() < 1 Then
            Session.Clear()
            Session.Abandon()
            Session.RemoveAll()
            FormsAuthentication.RedirectToLoginPage()
            Exit Sub
        End If

        If Session("username") Is Nothing Then
            Session("username") = username()
        End If
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        If Not cookie Is Nothing Then

            If Not Me.IsPostBack Then
                txtDate.Text = Format(Now.AddDays(-7), "dd MMM yyyy")
                txttoDate.Text = Format(Now, "dd MMM yyyy")
                'txtDate.Text = Format(CDate(Date.Now), "MM/dd/yyyy")
                binding()

                'bram 5April2024
                Dim msg As String = ""
                msg = orderasap().ToString

                If modulepriv() <> 0 Then
                    alert1.Visible = False
                    alert1.Attributes.Add("style", "display:none; ")
                    btnnew.Visible = False
                Else
                    btnnew.Visible = True
                    If msg <> "" Then
                        'messagePilihSatu(msg & "!")
                        'getlatepost.Replace("\n", Environment.NewLine)
                        alert1.Text = msg.Replace("\n", Environment.NewLine)
                        alert1.Attributes.Add("style", "max-height:130px; overflow-y:auto; overflow-x:hidden; resize:none; color:#721c24; background-color:#f8d7da; border:1px solid #f5c6cb; border-radius:8px; padding:8px 12px; box-shadow:0 4px 6px rgba(0,0,0,0.1); font-weight:bold; font-size:14px; line-height:1.5; width:100%; display:block;")

                        'alert1.Attributes.Add("style", "height:" & (alert1.Text.Split("\n").Length - 1) * 10 & ";")
                        alert1.ReadOnly = True

                        alert1.Visible = True
                    Else
                        alert1.Visible = False
                        alert1.Attributes.Add("style", "display:none; ")
                    End If
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
 
 
     End Sub
     Protected Sub txtDate_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtDate.TextChanged
         If txtDate.Text = "" Then
             txtDate.Text = Format(Now, "dd MMM yyyy")
         End If
         'GetBackground
         binding()
     End Sub
     Sub binding()
 
         If custid() = 0 Then
             Response.Redirect("~/Default.aspx", False)
         Else
            
 
             Dim ds As New DataSet
             ds = clsInv.getInvRcv(Convert.ToDateTime(txtDate.Text), Convert.ToDateTime(txttoDate.Text), custid, username, modulepriv)
            If ds.Tables.Count > 0 Then
                If Not ds.Tables(0).Columns.Contains("p_tax") Then
                    ds.Tables(0).Columns.Add("p_tax", GetType(Decimal))
                End If
                If Not ds.Tables(0).Columns.Contains("n_tax") Then
                    ds.Tables(0).Columns.Add("n_tax", GetType(Decimal))
                End If
                If Not ds.Tables(0).Columns.Contains("pr_id") Then
                    ds.Tables(0).Columns.Add("pr_id", GetType(Long))
                End If
                ViewState("vsreceive") = ds.Tables(0)
                If ds IsNot Nothing AndAlso ds.Tables.Count > 0 Then
                    lstViewDetails.DataSource = ds
                Else
                    lstViewDetails.DataSource = Nothing
                End If
                lstViewDetails.DataBind()
            End If

        End If
 
     End Sub
    
     Sub delete(ByVal id As String)
         ' WService.DeleteInvRcv(id, cust, username)
         If (Now.Hour >= 23 And Now.Minute >= 30) Or (Now.Hour = 0 And Now.Minute <= 30) Then
             notsuccesfull("Please process the transaction after 1.00 AM")
             Exit Sub
         End If
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
 
                     If iflag = 2 And dCreate.Date < Now.Date And (modulepriv() And usrModulepriv.moduleuser = usrModulepriv.moduleuser) Then
                         ' lblError.Text = "Receiving no " & id & " cannot be delete, because already receive "
                         notsuccesfull("Receiving no " & id & " cannot be delete, because already receive ")
                         Exit Sub
                     ElseIf iflag = 2 And dCreate.Date < Format(Now.AddDays(-7), "dd MMM yyyy") Then
                         notsuccesfull("Receiving no " & id & " cannot be delete, because received 1 week ago ")
                         Exit Sub
                     End If
 
                     Dim confirmValue As String = Request.Form("confirm_value")
                     If confirmValue = "Yes" Then
 
                         If c_cmd.ExecuteScalar("select count(custid) from cld_inv_closing where custid = " & custid() & " and invdate = '" & Date.Now.Date.ToString("yyyy-MM-dd") & "' and  invflag = 0 ", 0) = 1 Then
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
                     mfunction.CreateCookie("rcvID", arg(0))
                     mfunction.CreateCookie("rcvCc", arg(1))
                     mfunction.CreateCookie("rcvSupid", arg(2))
                     mfunction.CreateCookie("rcvDO", arg(3))
                     mfunction.CreateCookie("rcvflag", arg(4))
 
                     If IsDate(arg(5)) = True Then
                         mfunction.CreateCookie("Createddate", arg(5))
                     Else
                         mfunction.CreateCookie("Createddate", "")
                     End If
                     If IsDate(arg(6)) = True Then
                         mfunction.CreateCookie("modifdate2", arg(6))
                     Else
                         mfunction.CreateCookie("modifdate2", "")
                     End If
                     mfunction.CreateCookie("acc2", arg(7))
                    mfunction.CreateCookie("exDelDate", arg(8))
                    mfunction.CreateCookie("p_tax", arg(9))
                    mfunction.CreateCookie("n_tax", arg(10))
                    Response.Redirect("~/Pages_InvRcvEdit.aspx", False)
 
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
             mfunction.CreateCookie("rcvID", "")
             mfunction.CreateCookie("rcvCc", 0)
             mfunction.CreateCookie("rcvSupid", 0)
             mfunction.CreateCookie("rcvDO", "")
             mfunction.CreateCookie("rcvflag", 0)
             mfunction.CreateCookie("Createddate", "")
             mfunction.CreateCookie("modifdate2", "")
             mfunction.CreateCookie("exDelDate", "")
             mfunction.CreateCookie("acc2", "")
             Response.Redirect("~/Pages_InvRcvEdit.aspx", False)
         Catch ex As Exception
             errLog(Err.Description, "Pages_InvRcv:btnNew_Click")
             Err.Clear()
         End Try
     End Sub
  
  
 
     Protected Function GetBackground()
         Dim imgstat As String
         If txtDate.Text <> Format(Now, "dd MMM yyyy") Then
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
 
     Private Sub txttoDate_TextChanged(sender As Object, e As EventArgs) Handles txttoDate.TextChanged
         If txttoDate.Text = "" Then
             txttoDate.Text = Format(Now, "dd MMM yyyy")
         End If
         binding()
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
SELECT dtl.stk_id, c.stkno as stocknum, dtl.stk_name,
round(SUM(dtl.totalu2)) torder, round(c.ttlqty) stocknow
 FROM `cld_inv_w_dtl` dtl
 INNER JOIN `cld_inv_w` w ON w.id = dtl.wid AND w.flag = 1
 LEFT JOIN `cld_inv_transstockcls` c ON c.ccid=2 AND c.stkid=dtl.stk_id AND c.invdate = CURDATE()
 GROUP BY dtl.stk_id, dtl.stk_name, c.stkno, c.ttlqty
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
