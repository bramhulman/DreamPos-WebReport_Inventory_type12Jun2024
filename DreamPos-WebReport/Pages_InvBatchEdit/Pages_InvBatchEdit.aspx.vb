Imports System.Threading
Imports System.Net
Imports System.Text
Imports System.IO
Imports System.Globalization

Public Class Pages_InvBatchEdit
    Inherits System.Web.UI.Page
    Private sNameuser As String
    Private Userid As Integer
    Private icustid As Integer
    Private imodul As Integer
    Private iModPriv As Integer
    Private irptpriv As Integer
    Private irptf As Integer
    Private mfunction As New ModFunction
    Private MTid As Integer
    Private CIcc As New Cls_ICc
    Private CStk As New Cls_IStk
    Private CBatch As New Cls_IBatch
    Private CIRcpDtl As New Cls_IRcpDtl
    'Private ddate As Date


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
        MTid = Request.Cookies("iBatchID").Value
        Try

            If Request.Cookies("UserLogin_inv") IsNot Nothing Then
                sNameuser = username()
                icustid = custid()
                imodul = modul()
                iModPriv = modulepriv()
                irptpriv = rptpriv()
                irptf = rptf()
                'loadCc()
                'loadItem()
                If IsPostBack = False Then
                    loadCc()
                    loadItem()
                    'editlist(MTid)

                    If MTid = 0 Then
                        hapus()
                        txtddate.Text = Now.Date.ToString("yyyy-MM-dd 00:00:00")
                    Else
                        editlist(MTid)
                    End If

                End If
                ScriptManager.GetCurrent(Me).RegisterPostBackControl(btnInsert)
                'ScriptManager.GetCurrent(Me).RegisterPostBackControl(btnUpdate)
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

    Protected Sub Page_unLoad(sender As Object, e As EventArgs) Handles Me.Unload
    End Sub
    Sub loadCc()
        'DdlTo.Items.Clear()
        'DdlTo.SelectedValue = Nothing
        Dim ds As New DataSet
        ds = CIcc.SelectDataset_Icc(custid, username, modulepriv, 0)
        If ds IsNot Nothing AndAlso ds.Tables.Count > 0 Then
            DdlTo.DataSource = ds
        Else
            DdlTo.DataSource = Nothing
        End If
        DdlTo.DataValueField = "Id"
        DdlTo.DataTextField = "Name"
        DdlTo.DataBind()
        If DdlTo.Items.Count > 0 Then
            DdlTo.SelectedValue = 0
        End If
    End Sub

    Sub loadItem()
        'ddlItem.Items.Clear()
        'ddlItem.SelectedValue = Nothing
        Dim ds As New DataSet
        ds = CStk.SelectDataset_BatchStkAll(custid)
        If ds IsNot Nothing AndAlso ds.Tables.Count > 0 Then
            ddlItem.DataSource = ds
        Else
            ddlItem.DataSource = Nothing
        End If
        ddlItem.DataValueField = "Id"
        ddlItem.DataTextField = "Name"
        ddlItem.DataBind()
        If ddlItem.Items.Count > 0 Then
            ddlItem.SelectedValue = 0
        End If
    End Sub
    Public Function username() As String
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        If cookie Is Nothing Then
            Return ""
        Else
            Return cookie("userName")
        End If

    End Function

    Sub hapus()

        txtUnit1.Text = 0
        txtUnit2.Text = 0
        txtTotalStk.Text = 0
        txtLosF.Text = 0
        txtStock.Text = 0
        txtStock.ReadOnly = True
        txtTotalStk.ReadOnly = True


        btnUpdate.Visible = False
        btnInsert.Visible = True
    End Sub

    Sub editlist(ByVal id As Integer)

        Dim invC As CultureInfo = CultureInfo.InvariantCulture
        Dim ds As New DataSet
        ds = CBatch.SelectDataset_InBatchEdit(id, custid)
        If ds.Tables(0).Rows.Count = 0 Then
            Exit Sub
        End If

        DdlTo.SelectedValue = ds.Tables(0).Rows(0).Item("cc")
        ddlItem.SelectedValue = ds.Tables(0).Rows(0).Item("stk_id")
        Dim unit1 As Decimal = ds.Tables(0).Rows(0).Item("U1")

        txtddate.Text = ds.Tables(0).Rows(0).Item("invdate")
        txtUnit1.Text = unit1.ToString("N", invC)
        Dim unit2 As Decimal = ds.Tables(0).Rows(0).Item("U2")
        txtUnit2.Text = unit2.ToString("N", invC)


        txtstkNo.Text = ds.Tables(0).Rows(0).Item("stk_no")
        Dim losf As Decimal = ds.Tables(0).Rows(0).Item("factor1")
        txtLosF.Text = losf.ToString("N", invC)

        lblUnit1.Text = ds.Tables(0).Rows(0).Item("U1_nm")
        lblUnit2.Text = ds.Tables(0).Rows(0).Item("U2_nm")
        lbltotal.Text = ds.Tables(0).Rows(0).Item("U2_nm") 'ds.Tables(0).Rows(0).Item("u2").ToString
        lblcurrent.Text = ds.Tables(0).Rows(0).Item("U2_nm") 'ds.Tables(0).Rows(0).Item("u2").ToString
        Dim totstk As Decimal = ds.Tables(0).Rows(0).Item("ttlstock")
        txtTotalStk.Text = totstk.ToString("N", invC)
        txtTotalStk.ReadOnly = True
        Dim stock As Decimal = ds.Tables(0).Rows(0).Item("stock")
        txtStock.Text = stock.ToString("N", invC)
        txtStock.ReadOnly = True
        ddlItem.Enabled = False
        DdlTo.Enabled = False

        'Perubahan 3 maret 2017, jadi klo transaksi2 sebelumnya ga bisa di edit
        If ds.Tables(0).Rows(0).Item("InvDate") < Now.Date Then

            btnUpdate.Visible = False
            If username() = "support" Then
                btnUpdate.Visible = True
            End If
        Else
            btnUpdate.Visible = True
        End If

        btnInsert.Visible = False
    End Sub

    Protected Sub btnInsert_Click(ByVal sender As Object, ByVal e As EventArgs) 'Handles btnInsert.Click
        insert()
    End Sub

    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnUpdate.Click
        insert()
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancel.Click

        Try
            'hapus()
            'mfunction.CreateCookie("iBatchID", 0)

            Response.Redirect("~/Pages_InvBatch.aspx", False)

        Catch ex As Exception
            errLog(Err.Description, "WebBatchEdit:btnCancel_Click")
            Err.Clear()
        End Try

    End Sub
    Sub messagesuccesfull(ByVal msg As String, Optional ByVal bsuccess As Boolean = True)

        Dim message As String = msg
        Dim url As String
        If bsuccess Then
            url = "../Pages_InvBatch.aspx"
        Else
            url = "Default.aspx"
        End If

        Dim script As String = "window.onload = function(){ alert('"
        script += message
        script += "');"
        script += "window.location = '"
        script += url
        script += "'; }"
        ClientScript.RegisterStartupScript(Me.GetType(), "Redirect", script, True)

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
    Sub insert()
        Dim msg As String = ""
        Dim newLine As String = "\n"
        ' Dim rcpid As Integer
        Dim c_cmd As New clsDbo("")
        Try
            If DdlTo.SelectedValue = 0 Then
                msg = "you must choose Cost Centre first"

            End If
            If txtUnit1.Text = 0 And txtUnit2.Text = 0 Then
                msg = msg + "Unit must be Fill " & newLine
            End If

            If msg <> "" Then
                notsuccesfull(msg)
                Exit Sub
            End If

            Dim clsBatch As New clsInventory

            'WService.insertBatch(MTid, DdlTo.SelectedValue, ddlItem.SelectedValue, ddlItem.SelectedItem.Text, txtUnit1.Text, _
            '                      txtUnit2.Text, CInt(txtLosF.Text), CDec(txtTotalStk.Text / txtLosF.Text), lblUnit1.Text, lblUnit2.Text, username, username, txtstkNo.Text, DdlTo.SelectedItem.Text, username, cust)

            If c_cmd.ExecuteScalar("select count(custid) from cld_inv_closing where custid = " & custid() & " and invdate = '" & Date.Now.Date.ToString("yyyy-MM-dd") & "' and  invflag = 0 ", 0) = 1 Then
                FormsAuthentication.SetAuthCookie(username, False)
                messagesuccesfull("Closing process,Please login again.", False)
                Exit Sub
            Else
                clsBatch.insertBatch(MTid, DdlTo.SelectedValue, ddlItem.SelectedValue, ddlItem.SelectedItem.Text, txtUnit1.Text,
                                  txtUnit2.Text, CDec(txtLosF.Text), CDec(txtTotalStk.Text), lblUnit1.Text, lblUnit2.Text, username, username, txtstkNo.Text, DdlTo.SelectedItem.Text, username, custid, CDate(txtddate.Text))

            End If


            'Dim clsprocess As New clsInventoryProcess
            'clsprocess.InvstkId = ddlItem.SelectedValue
            'clsprocess.InvCc = DdlTo.SelectedValue
            'clsprocess.InvCcName = DdlTo.SelectedItem.Text
            'clsprocess.InvCustId = custid()
            'clsprocess.InvstkName = ddlItem.SelectedItem.Text
            'clsprocess.BP_in = CDec(txtTotalStk.Text)
            'clsprocess.BP_out = 0
            'clsprocess.InvStkNo = txtstkNo.Text
            'clsprocess.InvDate = Date.Now.Date.ToString("yyyy-MM-dd 00:00:00")
            ''cInv.executeNonQuery_Inv_ProcessCls(Date.Now.Date.ToString("yyyy-MM-dd"), ddlFrom.SelectedValue, ddlFrom.SelectedItem.Text, stkid, stknumber, stkname, custid, 0, stktotal)
            'clsprocess.executeNonQuery_Inv_ProcessCls()

            'Dim dt As DataTable = CIRcpDtl.SelectDataset_rcpdtl_nr(custid, ddlItem.SelectedValue)
            'If dt.Rows.Count > 0 Then
            '    For Each row As DataRow In dt.Rows
            '        'untuk blink

            '        clsprocess.InvstkId = dt.Rows(0).Item("kodeid")
            '        clsprocess.InvCc = DdlTo.SelectedValue
            '        clsprocess.InvCcName = DdlTo.SelectedItem.Text
            '        clsprocess.InvCustId = custid()
            '        clsprocess.InvstkName = dt.Rows(0).Item("dtlname")
            '        clsprocess.BP_in = 0

            '        If dt.Rows(0).Item("lossf") <> 0 Then
            '            clsprocess.BP_out = CDbl((dt.Rows(0).Item("qty") + (dt.Rows(0).Item("qty") * dt.Rows(0).Item("lossf") / 100)) / dt.Rows(0).Item("factor2"))
            '        Else
            '            clsprocess.BP_out = CDbl((dt.Rows(0).Item("qty")) / dt.Rows(0).Item("factor2"))
            '        End If

            '        clsprocess.InvStkNo = dt.Rows(0).Item("number")
            '        clsprocess.InvDate = Date.Now.Date.ToString("yyyy-MM-dd 00:00:00")
            '        'cInv.executeNonQuery_Inv_ProcessCls(Date.Now.Date.ToString("yyyy-MM-dd"), ddlFrom.SelectedValue, ddlFrom.SelectedItem.Text, stkid, stknumber, stkname, custid, 0, stktotal)
            '        clsprocess.executeNonQuery_Inv_ProcessCls()
            '    Next

            'End If


            clsBatch = Nothing

            Response.Redirect("~/Pages_InvBatch.aspx", False)

        Catch ex As Exception
            'MsgBox(ex.Message)
        End Try

    End Sub




    Protected Sub ddlItem_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlItem.SelectedIndexChanged
        If ddlItem.SelectedValue > 0 And DdlTo.Text <> "" Then

            Dim invC As CultureInfo = CultureInfo.InvariantCulture
            Dim ds As DataSet = CStk.SelectData_Inv_Stk(ddlItem.SelectedValue, DdlTo.SelectedValue, custid)
            If ds.Tables(0).Rows.Count > 0 Then
                'txtLosF.Text = FormatCurrency(ds.Tables(0).Rows(0).Item("Factor1"), 0).Replace("$", "")
                'txtstkNo.Text = ds.Tables(0).Rows(0).Item("number").ToString
                'lblUnit1.Text = ds.Tables(0).Rows(0).Item("unit1").ToString
                'lblUnit2.Text = ds.Tables(0).Rows(0).Item("unit2").ToString
                'txtStock.Text = FormatCurrency(ds.Tables(0).Rows(0).Item("stock"), 0).Replace("$", "")
                'txtTotalStk.Text = FormatCurrency(Val(txtUnit1.Text) * Val(txtLosF.Text) + Val(txtUnit2.Text), 0).Replace("$", "")
                Dim stock As Decimal = ds.Tables(0).Rows(0).Item("stock")
                Dim factor1 As Decimal = ds.Tables(0).Rows(0).Item("Factor1")
                Dim total As Decimal = Val(txtUnit1.Text) * Val(factor1) + Val(txtUnit2.Text)

                'txtLosF.Text = FormatCurrency(ds.Tables(0).Rows(0).Item("Factor1"), 0).Replace("$", "")
                txtstkNo.Text = ds.Tables(0).Rows(0).Item("number").ToString
                lblUnit1.Text = ds.Tables(0).Rows(0).Item("unit1").ToString
                lblUnit2.Text = ds.Tables(0).Rows(0).Item("unit2").ToString
                lbltotal.Text = ds.Tables(0).Rows(0).Item("unit2").ToString
                lblcurrent.Text = ds.Tables(0).Rows(0).Item("unit2").ToString
                txtLosF.Text = ds.Tables(0).Rows(0).Item("Factor1") 'CInt(factor1)

                txtStock.Text = stock.ToString("N", invC)
                'txtTotalStk.Text = total.ToString("N", invC)
                txtTotalStk.Text = (total).ToString
            End If
        End If
    End Sub

    Protected Sub DdlTo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DdlTo.SelectedIndexChanged
        If DdlTo.SelectedValue > 0 And ddlItem.Text <> "" Then

            Dim invC As CultureInfo = CultureInfo.InvariantCulture
            Dim ds As DataSet = CStk.SelectData_Inv_Stk(ddlItem.SelectedValue, DdlTo.SelectedValue, custid)
            If ds.Tables(0).Rows.Count > 0 Then
                Dim stock As Decimal = ds.Tables(0).Rows(0).Item("stock")
                Dim factor1 As Decimal = ds.Tables(0).Rows(0).Item("Factor1")
                Dim total As Decimal = Val(txtUnit1.Text) * Val(factor1) + Val(txtUnit2.Text)

                txtstkNo.Text = ds.Tables(0).Rows(0).Item("number").ToString
                lblUnit1.Text = ds.Tables(0).Rows(0).Item("unit1").ToString
                lblUnit2.Text = ds.Tables(0).Rows(0).Item("unit2").ToString
                lbltotal.Text = ds.Tables(0).Rows(0).Item("unit2").ToString
                lblcurrent.Text = ds.Tables(0).Rows(0).Item("unit2").ToString
                txtLosF.Text = ds.Tables(0).Rows(0).Item("Factor1") 'CInt(factor1)

                txtStock.Text = stock.ToString("N", invC)
                'txtTotalStk.Text = total.ToString("N", invC)
                txtTotalStk.Text = (total).ToString
            End If
        End If
    End Sub

    Protected Sub txtUnit1_TextChanged(sender As Object, e As EventArgs) Handles txtUnit1.TextChanged
        If txtUnit1.Text <> "" Then
            Dim invC As CultureInfo = CultureInfo.InvariantCulture
            Dim total As Decimal = CDec(txtUnit1.Text) * CDec(txtLosF.Text) + CDec(txtUnit2.Text)
            If total > 50000 Then
                messagesuccesfull("batch qty too much!", False)
                Exit Sub
            End If
            'txtTotalStk.Text = total.ToString("N", invC)
            txtTotalStk.Text = (total).ToString
        Else
            txtUnit1.Text = 0
        End If
    End Sub

    Protected Sub txtUnit2_TextChanged(sender As Object, e As EventArgs) Handles txtUnit2.TextChanged
        If txtUnit2.Text <> "" Then
            Dim invC As CultureInfo = CultureInfo.InvariantCulture
            Dim total As Decimal = Val(txtUnit1.Text) * Val(txtLosF.Text) + Val(txtUnit2.Text)
            'txtTotalStk.Text = total.ToString("N", invC)
            txtTotalStk.Text = (total).ToString
        Else
            txtUnit2.Text = 0
        End If
    End Sub
End Class
