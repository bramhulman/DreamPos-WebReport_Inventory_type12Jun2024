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
    'Dim prevqty As Double = 0
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
                    loadrecipe()
                    'editlist(MTid)

                    If MTid = 0 Then
                        hapus()
                        txtddate.Text = Now.Date.ToString("yyyy-MM-dd 00:00:00")
                    Else
                        loadrecipe()
                        editlist(MTid)
                    End If

                End If
                'ScriptManager.GetCurrent(Me).RegisterPostBackControl(btnInsert)
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

    Sub loadrecipe()
        Dim crcp As New Cls_IRcp
        Dim ds As New DataSet
        ds = crcp.Selectds_sXrcpddl(custid)

        Dim dt As New DataTable
        dt = ds.Tables(0)
        If ddlItem.SelectedValue > 0 Then
            dt.DefaultView.RowFilter = "stkid='" & ddlItem.SelectedValue & "'"
            ddlrcp.DataSource = dt.DefaultView
        Else
            ddlrcp.DataSource = dt
        End If

        ddlrcp.DataValueField = "Id"
        ddlrcp.DataTextField = "Name"
        ddlrcp.DataBind()
        ddlrcp.Items.Insert(0, "--SELECT--")


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
        ddlrcp.SelectedValue = ds.Tables(0).Rows(0).Item("rcpid")
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
        ViewState("prevqty") = ds.Tables(0).Rows(0).Item("ttlstock")
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

    Protected Sub btnInsert_Click(ByVal sender As Object, e As EventArgs) Handles btnInsert.ServerClick
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
        Dim qty1 As Double = 0
        Dim expectqty As Double = 0

        Dim ciproc As New clsInventoryProcess
        Dim inv_date As Date
        Try
            If (Now.Hour >= 23 And Now.Minute >= 30) Or (Now.Hour = 0 And Now.Minute <= 30) Then
                notsuccesfull("Please process the transaction after 1.00 AM")
                Exit Sub
            End If
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

            Dim bNegative As Boolean = False
            Dim inv_dt As Date
            If Not IsDate(txtddate.Text) Then
                inv_dt = Date.Now.Date
            Else
                inv_dt = CDate(txtddate.Text)
            End If
            
            Dim strConfirm As String = Request.Form("hdnConfirmNegative")
            If String.IsNullOrEmpty(strConfirm) Then strConfirm = "0"

            If strConfirm <> "1" AndAlso inv_dt.Date = Date.Now.Date Then
                Dim qCheck2 As String = "SELECT d.kodeid, IFNULL((SELECT ttlqty FROM cld_inv_transstockcls WHERE stkid=d.kodeid AND ccid=" & DdlTo.SelectedValue & " AND invdate='" & inv_dt.ToString("yyyy-MM-dd 00:00:00") & "' AND custid=" & custid() & "),0) as ttlqty " &
                                        "FROM cld_inv_rcpdtl d WHERE d.rcpid=" & ddlrcp.SelectedValue & " AND d.CustID=" & custid() & " AND d.tipe=1"
                Dim dtBahan As DataTable = c_cmd.ReturnDataset(qCheck2).Tables(0)
                For Each rBahan As DataRow In dtBahan.Rows
                    If Convert.ToDouble(rBahan("ttlqty")) <= 0 Then
                        bNegative = True
                        Exit For
                    End If
                Next
                
                If bNegative Then
                    Dim targetBtnId As String = If(btnUpdate.Visible, btnUpdate.ClientID, btnInsert.ClientID)
                    Dim script As String = "if(confirm('Ada bahan material dengan stock 0 atau minus di hari ini. Tetap Lanjutkan?')) { document.getElementById('hdnConfirmNegative').value = '1'; document.getElementById('" & targetBtnId & "').click(); } else { $('.loading').hide(); }"
                    ClientScript.RegisterStartupScript(Me.GetType(), "Confirm", script, True)
                    Exit Sub
                End If
            End If

            ' --- Konfirmasi jika estimasi cost berbeda jauh (mencurigakan) dibandingkan cost saat ini di cld_inv_transstockcls ---
            Dim strConfirmCost As String = Request.Form("hdnConfirmCost")
            If String.IsNullOrEmpty(strConfirmCost) Then strConfirmCost = "0"

            If strConfirmCost <> "1" AndAlso Val(ddlrcp.SelectedValue) > 0 AndAlso Val(ddlItem.SelectedValue) > 0 AndAlso Val(DdlTo.SelectedValue) > 0 Then
                Dim targetStkId As Integer = Val(ddlItem.SelectedValue)
                Dim targetCcId As Integer = Val(DdlTo.SelectedValue)
                Dim targetRcpId As Integer = Val(ddlrcp.SelectedValue)
                Dim targetFactor1 As Decimal = Val(txtLosF.Text)
                If targetFactor1 <= 0 Then targetFactor1 = 1

                ' 1. Ambil cost saat ini dari cld_inv_transstockcls
                Dim qCurCost As String = "SELECT IFNULL(cost, 0) FROM cld_inv_transstockcls " &
                                         "WHERE stkid=" & targetStkId & " AND ccid=" & targetCcId & " AND custid=" & custid() & " AND invdate='" & inv_dt.ToString("yyyy-MM-dd 00:00:00") & "'"
                Dim curCostObj As Object = c_cmd.ExecuteScalar(qCurCost, 0)
                Dim curCost As Decimal = 0
                If curCostObj IsNot Nothing AndAlso IsNumeric(curCostObj) Then
                    curCost = Convert.ToDecimal(curCostObj)
                End If

                ' Jika di tanggal tsb cost = 0, cari cost terakhir yang > 0 sebelum tanggal tsb
                If curCost <= 0 Then
                    Dim qLastCost As String = "SELECT cost FROM cld_inv_transstockcls " &
                                              "WHERE stkid=" & targetStkId & " AND ccid=" & targetCcId & " AND custid=" & custid() & " AND cost > 0 AND invdate <= '" & inv_dt.ToString("yyyy-MM-dd 00:00:00") & "' " &
                                              "ORDER BY invdate DESC LIMIT 1"
                    Dim lastCostObj As Object = c_cmd.ExecuteScalar(qLastCost, 0)
                    If lastCostObj IsNot Nothing AndAlso IsNumeric(lastCostObj) Then
                        curCost = Convert.ToDecimal(lastCostObj)
                    End If
                End If

                ' 2. Hitung estimasi unit cost hasil batch dari recipe
                Dim qRcp As String = "SELECT d.kodeid, (d.Qty + (d.Qty * IFNULL(d.lossf, 0) / 100)) as qty_loss, IFNULL(s.Factor2, 1) as factor2 " &
                                     "FROM cld_inv_rcpdtl d " &
                                     "LEFT JOIN cld_inv_stk s ON d.kodeid = s.id AND d.CustID = s.CustID " &
                                     "WHERE d.rcpid=" & targetRcpId & " AND d.CustID=" & custid() & " AND d.tipe=1"
                Dim dtRcp As DataTable = c_cmd.ReturnDataset(qRcp).Tables(0)
                Dim estTotalCost As Decimal = 0

                For Each rRcp As DataRow In dtRcp.Rows
                    Dim ingStkId As Integer = Convert.ToInt32(rRcp("kodeid"))
                    Dim ingQty As Decimal = Convert.ToDecimal(rRcp("qty_loss"))
                    Dim ingFactor2 As Decimal = Convert.ToDecimal(rRcp("factor2"))
                    If ingFactor2 <= 0 Then ingFactor2 = 1

                    Dim qIngCost As String = "SELECT IFNULL((CASE WHEN cost > 0 THEN cost ELSE 0 END), 0) FROM cld_inv_transstockcls " &
                                             "WHERE invdate='" & inv_dt.ToString("yyyy-MM-dd 00:00:00") & "' AND custid=" & custid() & " AND ccid=" & targetCcId & " AND stkid=" & ingStkId
                    Dim ingCostObj As Object = c_cmd.ExecuteScalar(qIngCost, 0)
                    Dim ingCost As Decimal = 0
                    If ingCostObj IsNot Nothing AndAlso IsNumeric(ingCostObj) Then
                        ingCost = Convert.ToDecimal(ingCostObj)
                    End If

                    If ingCost <= 0 Then
                        Dim qIngLastCost As String = "SELECT cost FROM cld_inv_transstockcls " &
                                                     "WHERE stkid=" & ingStkId & " AND ccid=" & targetCcId & " AND custid=" & custid() & " AND cost > 0 AND invdate <= '" & inv_dt.ToString("yyyy-MM-dd 00:00:00") & "' " &
                                                     "ORDER BY invdate DESC LIMIT 1"
                        Dim ingLastObj As Object = c_cmd.ExecuteScalar(qIngLastCost, 0)
                        If ingLastObj IsNot Nothing AndAlso IsNumeric(ingLastObj) Then
                            ingCost = Convert.ToDecimal(ingLastObj)
                        End If
                    End If

                    estTotalCost += (ingQty / ingFactor2) * ingCost
                Next

                Dim estUnitCost As Decimal = estTotalCost / targetFactor1

                ' 3. Bandingkan jika ada baseline cost saat ini (> 0) dan estimasi cost > 0
                If curCost > 0 AndAlso estUnitCost > 0 Then
                    Dim diffPct As Decimal = Math.Abs(estUnitCost - curCost) / curCost * 100
                    ' Jika perbedaan >= 50%
                    If diffPct >= 50 Then
                        Dim targetBtnId As String = If(btnUpdate.Visible, btnUpdate.ClientID, btnInsert.ClientID)
                        Dim msgWarn As String = "Perhatian: Perkiraan cost hasil batch (Rp " & estUnitCost.ToString("N2") & ") berbeda jauh (" & diffPct.ToString("N1") & "%) dibandingkan cost saat ini (Rp " & curCost.ToString("N2") & "). Tetap Lanjutkan?"
                        Dim scriptCost As String = "if(confirm('" & msgWarn.Replace("'", "\'") & "')) { document.getElementById('hdnConfirmCost').value = '1'; document.getElementById('" & targetBtnId & "').click(); } else { $('.loading').hide(); }"
                        ClientScript.RegisterStartupScript(Me.GetType(), "ConfirmCost", scriptCost, True)
                        Exit Sub
                    End If
                End If
            End If

            Dim clsBatch As New clsInventory

            'WService.insertBatch(MTid, DdlTo.SelectedValue, ddlItem.SelectedValue, ddlItem.SelectedItem.Text, txtUnit1.Text, _
            '                      txtUnit2.Text, CInt(txtLosF.Text), CDec(txtTotalStk.Text / txtLosF.Text), lblUnit1.Text, lblUnit2.Text, username, username, txtstkNo.Text, DdlTo.SelectedItem.Text, username, cust)

            If c_cmd.ExecuteScalar("select count(custid) from cld_inv_closing where custid = " & custid() & " and invdate = '" & Date.Now.Date.ToString("yyyy-MM-dd") & "' and  invflag = 0 ", 0) = 1 Then
                FormsAuthentication.SetAuthCookie(username, False)
                messagesuccesfull("Closing process,Please login again.", False)
                Exit Sub
            Else
                If Not IsDate(txtddate.Text) Then
                    inv_date = Date.Now.Date
                Else
                    inv_date = CDate(txtddate.Text)
                End If
                qty1 = ciproc.getrecqty(ddlItem.SelectedValue, DdlTo.SelectedValue, custid, inv_date)
                If inv_date < Date.Now.Date Then
                    expectqty = qty1
                Else
                    expectqty = qty1 - ViewState("prevqty") + Val(txtTotalStk.Text)
                End If


                clsBatch.insertBatch(MTid, DdlTo.SelectedValue, ddlItem.SelectedValue, ddlItem.SelectedItem.Text, txtUnit1.Text,
                                  txtUnit2.Text, CDec(txtLosF.Text), CDec(txtTotalStk.Text), lblUnit1.Text, lblUnit2.Text, username, username, txtstkNo.Text, DdlTo.SelectedItem.Text, username, custid, inv_date, ddlrcp.SelectedValue)
                qty1 = ciproc.getrecqty(ddlItem.SelectedValue, DdlTo.SelectedValue, custid, inv_date)
                If qty1 < expectqty Then
                    errLog("qty lebih kecil", "batch " & DdlTo.SelectedItem.Text & " " & ddlItem.SelectedItem.Text)
                    DeleteInvBatch(MTid, custid, username)
                End If
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
                loadrecipe()
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
            If (IsNumeric(txtUnit1.Text) = False) Or (IsNumeric(txtUnit2.Text) = False) Then
                messagesuccesfull("please input number only !", True)
                Exit Sub
            End If
            Dim invC As CultureInfo = CultureInfo.InvariantCulture
            Dim total As Decimal = CDec(txtUnit1.Text) * CDec(txtLosF.Text) + CDec(txtUnit2.Text)
            If total > 50000 Then
                messagesuccesfull("batch qty too much!", True)
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
            If IsNumeric(txtUnit2.Text) = False Then
                messagesuccesfull("please input number only !", True)
                Exit Sub
            Else
                Dim invC As CultureInfo = CultureInfo.InvariantCulture
                Dim total As Decimal = Val(txtUnit1.Text) * Val(txtLosF.Text) + Val(txtUnit2.Text)
                'txtTotalStk.Text = total.ToString("N", invC)
                txtTotalStk.Text = (total).ToString
            End If

        Else
            txtUnit2.Text = 0
        End If
    End Sub
    Public Sub DeleteInvBatch(ByVal id As Integer, ByVal Custid As Integer, ByVal username As String)

        Try

            Dim strTr As String
            Dim c_cmd As New Cls_command

            Dim ds As New DataSet
            Dim clsprocess As New clsInventoryProcess
            'untuk hapus stockcls
            'Dim clsinv As New clsInventory
            Dim clsBatch As New clsInventory_Batch
            ds = CBatch.SelectDataset_InBatchEdit(id, Custid)
            If ds.Tables(0).Rows.Count > 0 Then

                'clsinv.del_stkcls_Batch(id, ds.Tables(0).Rows(0).Item("cc"), ds.Tables(0).Rows(0).Item("stk_id"), ds.Tables(0).Rows(0).Item("stk_name"), ds.Tables(0).Rows(0).Item("U1"), ds.Tables(0).Rows(0).Item("U2"), ds.Tables(0).Rows(0).Item("factor1"), ds.Tables(0).Rows(0).Item("ttlstock"), ds.Tables(0).Rows(0).Item("U1_nm"), ds.Tables(0).Rows(0).Item("U2_nm"), ds.Tables(0).Rows(0).Item("CreatedBy"), ds.Tables(0).Rows(0).Item("Stk_no"), ds.Tables(0).Rows(0).Item("Cc_name"), username, Custid)
                clsBatch.getproperty_RcpDetail_del(Custid, ds.Tables(0).Rows(0).Item("stk_id"), ds.Tables(0).Rows(0).Item("ttlstock"), ds.Tables(0).Rows(0).Item("Invdate"), id, ds.Tables(0).Rows(0).Item("Cc"), ds.Tables(0).Rows(0).Item("Cc_name"), id)
                clsprocess.InvstkId = ds.Tables(0).Rows(0).Item("stk_id")
                clsprocess.InvCc = ds.Tables(0).Rows(0).Item("Cc")
                clsprocess.InvCcName = ds.Tables(0).Rows(0).Item("Cc_name")
                clsprocess.InvCustId = Custid
                clsprocess.InvstkName = ds.Tables(0).Rows(0).Item("stk_name")
                clsprocess.BP_in = -(CDbl(ds.Tables(0).Rows(0).Item("ttlstock")))
                clsprocess.BP_out = 0
                clsprocess.InvStkNo = ds.Tables(0).Rows(0).Item("Stk_no")
                clsprocess.InvDate = ds.Tables(0).Rows(0).Item("Invdate") 'Date.Now.Date.ToString("yyyy-MM-dd")
                'clsprocess.InvCost = bcost
                clsprocess.BCost = True
                clsprocess.executeNonQuery_Inv_ProcessCls()
            End If


            'end untuk hapus stockcls

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


            strTr = "DELETE FROM cld_inv_trans  where ID = " & id & " and type = 9; DELETE FROM cld_inv_transnotes where transid = " & id & " "
            c_cmd.executeNonQuery(strTr)


        Catch ex As Exception
            errLog(Err.Description, "Pages_InvBatch:DeleteInvBatch")
            Err.Clear()
        End Try
    End Sub
End Class
