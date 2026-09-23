Imports System.Threading
Imports System.Net
Imports System.Text
Imports System.IO
Imports System.Globalization

Public Class wastelist_edit
    Inherits System.Web.UI.Page
    Private sNameuser As String
    Private Userid As Integer
    Private icustid As Integer
    Private imodul As Integer
    Private iModPriv As Integer
    Private irptpriv As Integer
    Private irptf As Integer
    Private mfunction As New ModFunction
    Private CWid As Integer
    Private cInv As New clsInventory
    'Private wstkID As Integer
    Private wCc As Integer
    Private oldttlstk As Integer
    'Private wInvdate As Date
    'Dim ex_u1 As Double
    'Dim ex_u2 As Double
    'Dim ex_ttlrw As Double


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
            CWid = Request.Cookies("WasteID").Value 'Request.Cookies("CWasteID").Value
            'wstkID = Request.Cookies("wstkID").Value
            wCc = Request.Cookies("wCc").Value

            'If IsDate(Request.Cookies("wInvDate").Value) Then
            '    wInvdate = Request.Cookies("wInvDate").Value
            'End If
            If Request.Cookies("UserLogin_inv") IsNot Nothing Then
                sNameuser = username()
                icustid = custid()
                imodul = modul()
                iModPriv = modulepriv()
                irptpriv = rptpriv()
                irptf = rptf()
                If IsPostBack = False Then
                    loadCc()
                    loadItemawal()
                    ' editlist(MTid)
                    txtDate.Text = Format(Now, "dd MMM yyyy")
                    'txtDate.Text = Format(Now.AddDays(-1), "dd MMM yyyy")
                    cle.StartDate = Date.Now.AddDays(-2)
                    cle.EndDate = Date.Now

                    If CWid = 0 Then
                        hapus()
                    Else
                        editlist(CWid, custid(), wCc)
                    End If

                End If
                ScriptManager.GetCurrent(Me).RegisterPostBackControl(btnInsert)
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
        ds = cInv.getInvcc(custid, username, modulepriv, 0)
        If ds IsNot Nothing AndAlso ds.Tables.Count > 0 Then
            DdlTo.DataSource = ds
        Else
            DdlTo.DataSource = Nothing
        End If
        DdlTo.DataValueField = "Id"
        DdlTo.DataTextField = "Name"

        DdlTo.DataBind()
    End Sub

    Sub loadItemawal()
        'ddlItem.Items.Clear()
        'ddlItem.SelectedValue = Nothing
        Dim ds As New DataSet
        'ds = cInv.getStkAll(custid)
        ds = cInv.getStkAll1(custid)
        If ds IsNot Nothing AndAlso ds.Tables.Count > 0 Then
            ddlItem.DataSource = ds
        Else
            ddlItem.DataSource = Nothing
        End If
        ddlItem.DataValueField = "stkid"
        ddlItem.DataTextField = "stkname"
        ddlItem.DataBind()
    End Sub
    Sub loadItem()
        ddlItem.Items.Clear()

        Dim ds As New DataSet
        'ds = cInv.getStkAll(custid)
        ds = cInv.getStkAll1(custid, DdlTo.SelectedValue)
        If ds IsNot Nothing AndAlso ds.Tables.Count > 0 Then
            ddlItem.DataSource = ds
        Else
            ddlItem.DataSource = Nothing
        End If
        ddlItem.DataValueField = "stkid"
        ddlItem.DataTextField = "stkname"
        ddlItem.DataBind()

        ddlItem.Items.Insert(0, New ListItem("--Select--", "0"))
        ddlItem.SelectedIndex = 0
    End Sub
    Public Function username() As String
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        Return cookie("userName")

    End Function

    Sub hapus()

        txtUnit1.Text = 0
        txtUnit2.Text = 0
        txtTotalStk.Text = 0
        txtLosF.Text = 0
        txtStock.Text = 0
        'txtName.Text = ""
        'txtNo.Text = ""


        btnUpdate.Visible = False
        btnInsert.Visible = True
    End Sub

    Sub editlist(ByVal Id As Integer, ByVal custid As Integer, ByVal ccid As Integer)

        Dim invC As CultureInfo = CultureInfo.InvariantCulture
        Dim ds As New DataSet
        ds = cInv.getRwasteEdit2(Id, custid, ccid)
        'ds = cInv.getInvclswasteEdit(Id, custid, ccid)
        If ds.Tables(0).Rows.Count = 0 Then
            Exit Sub
        End If
        If CDate(ds.Tables(0).Rows(0).Item("invdate")) <= Now.Date.AddDays(-2) Then
            txtDate.Enabled = False
        Else
            'If CDate(ds.Tables(0).Rows(0).Item("invdate")) = Now.Date.AddDays(-1) Then
            '    ddlclosing.SelectedValue = 0
            'Else
            '    ddlclosing.SelectedValue = 1
            'End If
            txtDate.Text = Format(ds.Tables(0).Rows(0).Item("invdate"), "dd-MMM-yyyy").ToString
            txtDate.Enabled = False
        End If

        DdlTo.SelectedValue = ds.Tables(0).Rows(0).Item("cc")
        ddlItem.SelectedValue = ds.Tables(0).Rows(0).Item("stk_id")
        Dim unit1 As Decimal = ds.Tables(0).Rows(0).Item("U1")
        ' txtUnit1.Text = FormatCurrency(ds.Tables(0).Rows(0).Item("U1"), 0).Replace("$", "")
            txtUnit1.Text = unit1.ToString("N", invC)
        Dim unit2 As Decimal = ds.Tables(0).Rows(0).Item("U2")
        oldttlstk = ds.Tables(0).Rows(0).Item("U2")
        txtUnit2.Text = unit2.ToString("N", invC)

        ' txtUnit2.Text = FormatCurrency(ds.Tables(0).Rows(0).Item("U2"), 0).Replace("$", "")
        txtstkNo.Text = ds.Tables(0).Rows(0).Item("stk_no")
        Dim losf As Decimal = ds.Tables(0).Rows(0).Item("factor1")
        txtLosF.Text = losf.ToString("N", invC)
        'txtLosF.Text = FormatCurrency(ds.Tables(0).Rows(0).Item("factor1"), 0).Replace("$", "")

        'cost


        lblUnit1.Text = ds.Tables(0).Rows(0).Item("U1_nm")
        lblUnit2.Text = ds.Tables(0).Rows(0).Item("U2_nm")
        lblcost.Text = "/ " & ds.Tables(0).Rows(0).Item("U2_nm")
        lblStock.Text = ds.Tables(0).Rows(0).Item("U2_nm")
        txtNotes.Text = ds.Tables(0).Rows(0).Item("Notes")
        Dim ds2 As DataSet = cInv.get_InvStk_cls(ddlItem.SelectedValue, DdlTo.SelectedValue, custid, Date.Now.Date.ToString("yyyy-MM-dd 00:00:00"))
        'Dim ds As DataSet = cInv.get_InvStk(ddlItem.SelectedValue, DdlTo.SelectedValue, custid)
        If ds2.Tables(0).Rows.Count > 0 Then
            lblUnit1.Text = ds2.Tables(0).Rows(0).Item("unit1").ToString
            lblUnit2.Text = ds2.Tables(0).Rows(0).Item("unit2").ToString
            Dim factor1 As Double = ds2.Tables(0).Rows(0).Item("Factor1")
            Dim total As Double = Val(txtUnit1.Text) * Val(factor1) + Val(txtUnit2.Text)
            txtTotalStk.Text = CInt(total)
            Dim stock As Decimal = Convert.ToDecimal(ds2.Tables(0).Rows(0).Item("stock")).ToString("#.000")
            txtStock.Text = stock 'stock.ToString("N", invC)
            If IsDBNull(ds2.Tables(0).Rows(0).Item("coststk")) = False Then
                txtcost.Text = ds2.Tables(0).Rows(0).Item("coststk")
            Else
                txtcost.Text = 0
            End If
        End If


        ' txtTotalStk.Text = FormatCurrency(ds.Tables(0).Rows(0).Item("ttlstock"), 0).Replace("$", "")
        'txtStock.Text = FormatCurrency(ds.Tables(0).Rows(0).Item("stock"), 0).Replace("$", "")
        ddlItem.Enabled = False
        DdlTo.Enabled = False

        'Perubahan 3 maret 2017, jadi klo transaksi2 sebelumnya ga bisa di edit
        If ds.Tables(0).Rows(0).Item("InvDate") <= Now.Date.AddDays(-2) Then
            btnUpdate.Visible = False
            txtDate.Enabled = False
        Else
            btnUpdate.Visible = True
        End If

        btnInsert.Visible = False
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

    'Sub delete(ByVal id As Integer)
    '    Dim ds As New DataSet
    '    Try
    '        'WService.DeleteInvRwaste(id, cust, username)
    '        ds = cInv.Select_invtrans(id, custid)
    '        cInv.DeleteInvRwaste(id, custid, username)
    '        Dim c_cmd As New clsDbo("")
    '        If ds.Tables(0).Rows.Count > 0 Then
    '            Dim clsprocess As New clsInventoryProcess
    '            clsprocess.InvCc = ds.Tables(0).Rows(0).Item("cc").ToString
    '            clsprocess.InvCcName = ds.Tables(0).Rows(0).Item("cc_name").ToString
    '            clsprocess.InvCustId = custid()
    '            clsprocess.InvstkId = ds.Tables(0).Rows(0).Item("Stk_Id").ToString
    '            clsprocess.InvStkNo = ds.Tables(0).Rows(0).Item("Stk_no").ToString
    '            clsprocess.InvstkName = ds.Tables(0).Rows(0).Item("Stk_Name").ToString
    '            ' clsprocess.Invrcvqty = (ds.Tables(0).Rows(0).Item("ttlStock").ToString)
    '            clsprocess.InvCost = ds.Tables(0).Rows(0).Item("cost").ToString

    '            clsprocess.InvDate = CDate(ds.Tables(0).Rows(0).Item("InvDate").ToString)
    '            'cInv.executeNonQuery_Inv_ProcessCls(Date.Now.Date.ToString("yyyy-MM-dd"), ddlFrom.SelectedValue, ddlFrom.SelectedItem.Text, stkid, stknumber, stkname, custid, 0, stktotal)
    '            clsprocess.delclosingwaste()
    '            clsprocess.InvDate = CDate(ds.Tables(0).Rows(0).Item("InvDate").ToString).AddDays(1)
    '            clsprocess.executeNonQuery_Inv_ProcessCls()
    '        End If


    '        'Response.Redirect("~/Pages_InvRwaste.aspx")
    '        'binding()
    '        c_cmd.dispose()
    '        ds.Tables.Clear()
    '        ds.Dispose()
    '    Catch ex As Exception

    '    End Try
    'End Sub
    Sub delete(ByVal id As Integer)
        Dim ds As New DataSet
        Try
            'WService.DeleteInvRwaste(id, cust, username)
            ds = cInv.Select_invtrans(id, custid)
            cInv.DeleteInvRwaste(id, custid, username)
            Dim c_cmd As New clsDbo("")
            If ds.Tables(0).Rows.Count > 0 Then
                Dim clsprocess As New clsInventoryProcess
                clsprocess.InvCc = ds.Tables(0).Rows(0).Item("cc").ToString
                clsprocess.InvCcName = ds.Tables(0).Rows(0).Item("cc_name").ToString
                clsprocess.InvCustId = custid()
                clsprocess.InvstkId = ds.Tables(0).Rows(0).Item("Stk_Id").ToString
                clsprocess.InvStkNo = ds.Tables(0).Rows(0).Item("Stk_no").ToString
                clsprocess.InvstkName = ds.Tables(0).Rows(0).Item("Stk_Name").ToString
                ' clsprocess.Invrcvqty = (ds.Tables(0).Rows(0).Item("ttlStock").ToString)
                clsprocess.InvCost = ds.Tables(0).Rows(0).Item("cost").ToString
                clsprocess.InvWasteQty = -(ds.Tables(0).Rows(0).Item("ttlStock").ToString)
                clsprocess.InvDate = CDate(ds.Tables(0).Rows(0).Item("InvDate").ToString)
                'cInv.executeNonQuery_Inv_ProcessCls(Date.Now.Date.ToString("yyyy-MM-dd"), ddlFrom.SelectedValue, ddlFrom.SelectedItem.Text, stkid, stknumber, stkname, custid, 0, stktotal)
                clsprocess.executeNonQuery_Inv_ProcessCls()
            End If


            'Response.Redirect("~/Pages_InvRwaste.aspx")
            'binding()
            c_cmd.dispose()
            ds.Tables.Clear()
            ds.Dispose()
        Catch ex As Exception

        End Try
    End Sub
    Sub insert()

        Try
            Dim msg As String = ""
            Dim newLine As String = "\n"
            Dim ds As New DataSet
            'Dim rwid As String = ""
            Dim upwaste As Decimal = 0
            delete(CWid)
            CWid = 0

            If IsNumeric(txtUnit1.Text) = False Or IsNumeric(txtUnit2.Text) = False Then
                notsuccesfull("Unit must be numeric")
                Exit Sub
            End If

            Dim totalStk As Decimal = (Val(txtUnit1.Text) * Val(txtLosF.Text)) + Val(txtUnit2.Text)
            txtTotalStk.Text = totalStk.ToString()

            If totalStk = 0 Then
                msg = msg + "Unit must be Fill " & newLine
            End If
            If totalStk > Val(txtStock.Text) Then

                ' If ddlItem.Enabled = False Then

                'Else
                msg = msg + "Total Input Cannot more than Stock available" & newLine
                ' End If
            End If

            'If ddlclosing.SelectedValue = 0 Then
            '    invdate = Date.Now.AddDays(-1).Date.ToString("yyyy-MM-dd 00:00:00")
            'ElseIf ddlclosing.SelectedValue = 1 Then
            '    invdate = Date.Now.Date.ToString("yyyy-MM-dd 00:00:00")
            'End If
            'If cInv.get_bclose(ddlItem.SelectedValue, DdlTo.SelectedValue, invdate, custid) = True Then
            '    msg = msg + "this stock already adjusted" & newLine
            'End If

            If msg <> "" Then
                notsuccesfull(msg)
                Exit Sub
            End If

            'If ddlclosing.SelectedValue = 1 Then
            '    ds = cInv.insertClosingwaste(CWid, DdlTo.SelectedValue, ddlItem.SelectedValue, ddlItem.SelectedItem.Text, txtUnit1.Text,
            '                 txtUnit2.Text, txtLosF.Text, txtTotalStk.Text, txtcost.Text, lblUnit1.Text, lblUnit2.Text, username, username, txtstkNo.Text, DdlTo.SelectedItem.Text, username, custid)
            'Else
            ds = cInv.insertRwaste2(CWid, DdlTo.SelectedValue, ddlItem.SelectedValue, ddlItem.SelectedItem.Text, txtUnit1.Text,
                             txtUnit2.Text, txtLosF.Text, totalStk.ToString(), txtcost.Text, lblUnit1.Text, lblUnit2.Text, username, username, txtstkNo.Text, DdlTo.SelectedItem.Text, username, custid, Convert.ToDateTime(txtDate.Text), txtNotes.Text)

            'ds = cInv.insertClosingwaste(CWid, DdlTo.SelectedValue, ddlItem.SelectedValue, ddlItem.SelectedItem.Text, txtUnit1.Text,
            '                txtUnit2.Text, txtLosF.Text, txtTotalStk.Text, txtcost.Text, lblUnit1.Text, lblUnit2.Text, username, username, txtstkNo.Text, DdlTo.SelectedItem.Text, username, custid, Convert.ToDateTime(txtDate.Text))
            'Date.Now.AddDays(-1).Date.ToString("yyyy-MM-dd 00:00:00")
            'End If

            'If ds.Tables(0).Rows.Count > 0 Then
            '    Dim wasteold As Decimal = Convert.ToDecimal(ds.Tables(0).Rows(0).Item("rwasteqty")).ToString("#.000")

            '    upwaste = wasteold + Val(txtTotalStk.Text)
            'End If
            'langsung kurangi stok
            'Dim c_cmd As New clsDbo("")
            'c_cmd.executeNonQuery_Inv_ProcessCls_current(Date.Now, custid)

            'cInv.executeNonQuery_Inv_ProcessCls(Date.Now.Date.ToString("yyyy-MM-dd"), DdlTo.SelectedValue, DdlTo.SelectedItem.Text, ddlItem.SelectedValue, txtstkNo.Text, ddlItem.SelectedItem.Text, custid, txtTotalStk.Text, 0)
            Dim c_cmd As New clsDbo("")
            'c_cmd.Process_InvDataStock(Date.Now.Date.ToString("yyyy-MM-dd"), Date.Now.Date.ToString("yyyy-MM-dd"))
            Dim clsprocess As New clsInventoryProcess
            clsprocess.InvCc = DdlTo.SelectedValue
            clsprocess.InvCcName = DdlTo.SelectedItem.Text
            clsprocess.InvCustId = custid()
            clsprocess.InvstkId = ddlItem.SelectedValue
            clsprocess.InvStkNo = txtstkNo.Text
            clsprocess.InvstkName = ddlItem.SelectedItem.Text
            clsprocess.InvCost = txtcost.Text
            clsprocess.InvWasteQty = totalStk.ToString()
            'closing yesterday = 0
            'If ddlclosing.SelectedValue = 1 Then
            '    clsprocess.InvDate = Date.Now.Date.ToString("yyyy-MM-dd 00:00:00")
            'Else

            clsprocess.InvDate = Convert.ToDateTime(txtDate.Text) 'Date.Now.AddDays(-1).Date.ToString("yyyy-MM-dd 00:00:00")

            'End If            'cInv.executeNonQuery_Inv_ProcessCls(Date.Now.Date.ToString("yyyy-MM-dd"), ddlFrom.SelectedValue, ddlFrom.SelectedItem.Text, stkid, stknumber, stkname, custid, 0, stktotal)
            ' clsprocess.insertclosingwaste()
            clsprocess.executeNonQuery_Inv_ProcessCls()
            If Convert.ToDateTime(txtDate.Text) < Now.Date Then
                clsprocess.executeNonQuery_Inv_Processstock(Convert.ToDateTime(txtDate.Text), Date.Now.Date.ToString("yyyy-MM-dd 00:00:00"), custid, ddlItem.SelectedValue, DdlTo.SelectedValue, True)

            End If
            'clsprocess.InvDate = CDate(ds.Tables(0).Rows(0).Item("InvDate").ToString).AddDays(1)
            'clsprocess.varianceqty = 0
            'clsprocess.executeNonQuery_Inv_ProcessCls()
            Response.Redirect("~/Pages_InvRwaste.aspx", False)

        Catch ex As Exception
            'MsgBox(ex.Message)
        End Try

    End Sub




    Protected Sub DdlItem_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlItem.SelectedIndexChanged
        If ddlItem.SelectedValue > 0 And DdlTo.Text <> "" Then

            Dim invC As CultureInfo = CultureInfo.InvariantCulture
            Dim ds As DataSet = cInv.get_InvStk_cls(ddlItem.SelectedValue, DdlTo.SelectedValue, custid, Date.Now.Date.ToString("yyyy-MM-dd 00:00:00"))
            'Dim ds As DataSet = cInv.get_InvStk(ddlItem.SelectedValue, DdlTo.SelectedValue, custid)
            If ds.Tables(0).Rows.Count > 0 Then
                'txtLosF.Text = FormatCurrency(ds.Tables(0).Rows(0).Item("Factor1"), 0).Replace("$", "")
                'txtstkNo.Text = ds.Tables(0).Rows(0).Item("number").ToString
                'lblUnit1.Text = ds.Tables(0).Rows(0).Item("unit1").ToString
                'lblUnit2.Text = ds.Tables(0).Rows(0).Item("unit2").ToString
                'txtStock.Text = FormatCurrency(ds.Tables(0).Rows(0).Item("stock"), 0).Replace("$", "")
                'txtTotalStk.Text = FormatCurrency(Val(txtUnit1.Text) * Val(txtLosF.Text) + Val(txtUnit2.Text), 0).Replace("$", "")
                Dim stock As Double = Convert.ToDecimal(ds.Tables(0).Rows(0).Item("stock")).ToString("#.##0")
                Dim factor1 As Double = ds.Tables(0).Rows(0).Item("Factor1")
                Dim total As Double = Val(txtUnit1.Text) * Val(factor1) + Val(txtUnit2.Text)

                If IsDBNull(ds.Tables(0).Rows(0).Item("coststk")) = False Then
                    txtcost.Text = ds.Tables(0).Rows(0).Item("coststk")
                Else
                    txtcost.Text = 0
                End If

                'txtLosF.Text = FormatCurrency(ds.Tables(0).Rows(0).Item("Factor1"), 0).Replace("$", "")
                txtstkNo.Text = ds.Tables(0).Rows(0).Item("number").ToString
                lblUnit1.Text = ds.Tables(0).Rows(0).Item("unit1").ToString
                lblUnit2.Text = ds.Tables(0).Rows(0).Item("unit2").ToString
                lblcost.Text = "/ " & ds.Tables(0).Rows(0).Item("unit2")
                lblStock.Text = ds.Tables(0).Rows(0).Item("unit2")
                txtLosF.Text = CInt(factor1)

                txtStock.Text = stock 'stock.ToString("N", invC)
                'txtTotalStk.Text = total.ToString("N", invC)
                txtTotalStk.Text = CInt(total)
                If Val(txtUnit1.Text) > 0 Or Val(txtUnit2.Text) > 0 Then
                    Dim totalcost As Decimal = (Val(txtUnit1.Text) * Val(txtLosF.Text) + Val(txtUnit2.Text)) * Val(txtcost.Text)
                    txtTotalCost.Text = totalcost
                End If
            End If

        End If
    End Sub

    Protected Sub DdlTo_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles DdlTo.SelectedIndexChanged
        If DdlTo.SelectedValue > 0 And ddlItem.Text <> "" Then

            Dim invC As CultureInfo = CultureInfo.InvariantCulture
            Dim ds As DataSet = cInv.get_InvStk(ddlItem.SelectedValue, DdlTo.SelectedValue, custid)
            If ds.Tables(0).Rows.Count > 0 Then

                Dim stock As Decimal = Convert.ToDecimal(ds.Tables(0).Rows(0).Item("stock")).ToString("#.000")
                Dim factor1 As Decimal = ds.Tables(0).Rows(0).Item("Factor1")
                Dim total As Decimal = Val(txtUnit1.Text) * Val(factor1) + Val(txtUnit2.Text)

                If IsDBNull(ds.Tables(0).Rows(0).Item("coststk")) = False Then
                    txtcost.Text = ds.Tables(0).Rows(0).Item("coststk")
                Else
                    txtcost.Text = 0
                End If

                'txtLosF.Text = FormatCurrency(ds.Tables(0).Rows(0).Item("Factor1"), 0).Replace("$", "")
                txtstkNo.Text = ds.Tables(0).Rows(0).Item("number").ToString
                lblUnit1.Text = ds.Tables(0).Rows(0).Item("unit1").ToString
                lblUnit2.Text = ds.Tables(0).Rows(0).Item("unit2").ToString
                lblcost.Text = "/ " & ds.Tables(0).Rows(0).Item("unit2")
                lblStock.Text = ds.Tables(0).Rows(0).Item("unit2")
                txtLosF.Text = CInt(factor1)

                txtStock.Text = stock 'stock.ToString("N", invC)
                'txtTotalStk.Text = total.ToString("N", invC)
                txtTotalStk.Text = CInt(total)
                If Val(txtUnit1.Text) > 0 Or Val(txtUnit2.Text) > 0 Then
                    Dim totalcost As Decimal = (Val(txtUnit1.Text) * Val(txtLosF.Text) + Val(txtUnit2.Text)) * Val(txtcost.Text)
                    txtTotalCost.Text = totalcost
                End If
            End If
            loadItem()
        End If
    End Sub

    Protected Sub txtUnit1_TextChanged(sender As Object, e As EventArgs) Handles txtUnit1.TextChanged
        If txtUnit1.Text <> "" Then

            If IsNumeric(txtUnit1.Text) = False Then
                notsuccesfull("Unit must be numeric")
                txtUnit1.Text = 0
            End If

            Dim invC As CultureInfo = CultureInfo.InvariantCulture
            Dim total As Decimal = Val(txtUnit1.Text) * Val(txtLosF.Text) + Val(txtUnit2.Text)
            'txtTotalStk.Text = total.ToString("N", invC)
            txtTotalStk.Text = total
            Dim totalcost As Decimal = (Val(txtUnit1.Text) * Val(txtLosF.Text) + Val(txtUnit2.Text)) * Val(txtcost.Text)
            txtTotalCost.Text = totalcost
        Else
            txtUnit1.Text = 0
        End If
    End Sub

            Protected Sub txtUnit2_TextChanged(sender As Object, e As EventArgs) Handles txtUnit2.TextChanged
        If txtUnit2.Text <> "" Then
            If IsNumeric(txtUnit2.Text) = False Then
                txtUnit2.Text = 0
            End If

            Dim invC As CultureInfo = CultureInfo.InvariantCulture
            Dim total As Decimal = Val(txtUnit1.Text) * Val(txtLosF.Text) + Val(txtUnit2.Text)
            'txtTotalStk.Text = total.ToString("N", invC)
            txtTotalStk.Text = total
            Dim totalcost As Decimal = (Val(txtUnit1.Text) * Val(txtLosF.Text) + Val(txtUnit2.Text)) * Val(txtcost.Text)
            txtTotalCost.Text = totalcost
        Else
            txtUnit2.Text = 0
        End If
    End Sub



    Protected Sub btnUpdate_ServerClick(sender As Object, e As EventArgs) 'Handles btnUpdate.ServerClick
        insert()
    End Sub

    Protected Sub btnCancel_ServerClick(sender As Object, e As EventArgs) Handles btnCancel.ServerClick
        Try
            hapus()
            mfunction.CreateCookie("WasteID", 0)

            Response.Redirect("~/Pages_InvRwaste.aspx", False)

        Catch ex As Exception
            errLog(Err.Description, "WebRwasteEdit:btnCancel_Click")
            Err.Clear()
        End Try
    End Sub

    Protected Sub btnInsert_Click(ByVal sender As Object, ByVal e As EventArgs) 'Handles btnInsert.ServerClick
        insert()
    End Sub

End Class
