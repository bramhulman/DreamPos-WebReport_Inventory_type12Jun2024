Imports System.Threading
Imports System.Net
Imports System.Text
Imports System.IO
Imports System.Globalization

Public Class Pages_StockOpnameEdit
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
    Private ccid As Integer
    Private adjustnow As Integer
    Private CIcc As New Cls_ICc
    Private CStk As New Cls_IStk
    Private CBatch As New Cls_IBatch
    Private CsOname As New Cls_StkOpname
    Dim cInv As New clsInventory


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
        MTid = Request.Cookies("stkid").Value
        ccid = Request.Cookies("Ccid").Value
        adjustnow = Request.Cookies("adjustnow").Value
        Try

            If Request.Cookies("UserLogin_inv") IsNot Nothing Then
                sNameuser = username()
                icustid = custid()
                imodul = modul()
                iModPriv = modulepriv()
                irptpriv = rptpriv()
                irptf = rptf()

                If adjustnow = 0 Then
                    lblh1.Text = "Stock Opname Edit"
                    lblli1.Text = "Stock Opname Edit"
                ElseIf adjustnow = 1 Then
                    lblh1.Text = "Adjust Today"
                    lblli1.Text = "Adjust Today"
                End If

                If IsPostBack = False Then
                    loadCc()
                    loadItem()

                    If MTid = 0 Then
                        ddlItem.Enabled = True
                        DdlTo.Enabled = True
                        hapus()
                        DdlTo.SelectedValue = ccid
                        'CreateCookie("ID", 0)
                        'loadCust()
                    Else
                        ' loadItem()
                        editlist(MTid, ccid)
                        ' loadCust()
                    End If
                    ScriptManager.GetCurrent(Me).RegisterPostBackControl(btnInsert)
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

    End Sub
    Sub loadItem()
        'ddlItem.Items.Clear()
        'ddlItem.SelectedValue = Nothing
        Dim ds As New DataSet

        ds = CStk.SelectDataset_StkAll(custid)


        If ds IsNot Nothing AndAlso ds.Tables.Count > 0 Then
            ddlItem.DataSource = ds
        Else
            ddlItem.DataSource = Nothing
        End If
        ddlItem.DataValueField = "Id"
        ddlItem.DataTextField = "Name"
        ddlItem.DataBind()
        'ds = CStk.SelectDataStk(ddlItem.SelectedValue, custid)
        'If ds.Tables(0).Rows.Count > 0 Then
        '    txtLosF.Text = FormatCurrency(ds.Tables(0).Rows(0).Item("Factor1"), 0).Replace("$", "")
        '    txtstkNo.Text = ds.Tables(0).Rows(0).Item("number").ToString
        '    lblUnit1.Text = ds.Tables(0).Rows(0).Item("unit1").ToString
        '    lblUnit2.Text = ds.Tables(0).Rows(0).Item("unit2").ToString
        '    txtTotalStk.Text = FormatCurrency(Val(txtUnit1.Text) * Val(txtLosF.Text) + Val(txtUnit2.Text), 0).Replace("$", "")
        '    txtCost.Text = ds.Tables(0).Rows(0).Item("number").ToString
        'End If
        ds = cInv.Select_StockData(ddlItem.SelectedValue, DdlTo.SelectedValue, custid)
        If ds.Tables(0).Rows.Count > 0 Then
            txtLosF.Text = ds.Tables(0).Rows(0).Item("Factor1").ToString
            txtstkNo.Text = ds.Tables(0).Rows(0).Item("number").ToString
            lblUnit1.Text = ds.Tables(0).Rows(0).Item("unit1").ToString
            lblUnit2.Text = ds.Tables(0).Rows(0).Item("unit2").ToString
            lblcostu1.Text = "/ " & ds.Tables(0).Rows(0).Item("unit1").ToString
            lblunit22.Text = ds.Tables(0).Rows(0).Item("unit2").ToString
            lblcostu2.Text = "/ " & ds.Tables(0).Rows(0).Item("unit2").ToString
            txtinstock.Text = FormatCurrency(Val(ds.Tables(0).Rows(0).Item("ttlqty").ToString), 0).Replace("$", "")
            txtCost.Text = ds.Tables(0).Rows(0).Item("cost").ToString
            txtCostu1.Text = ds.Tables(0).Rows(0).Item("cost").ToString * ds.Tables(0).Rows(0).Item("Factor1").ToString
        End If
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
        txtCost.Text = 0
        txtCostu1.Text = 0
        'txtName.Text = ""
        'txtNo.Text = ""


        btnUpdate.Visible = False
        btnInsert.Visible = True
    End Sub

    Sub editlist(ByVal id As Integer, ByVal ccid As Integer)
        ddlItem.Enabled = False
        DdlTo.Enabled = False

        Dim ds As New DataSet
        ds = CsOname.SelectDataset_InvstkOpnameEdit(id, ccid, custid)
        'ds = cInv.Select_StockData(id, ccid, custid)
        If ds.Tables(0).Rows.Count = 0 Then
            ddlItem.SelectedValue = id
            DdlTo.SelectedValue = ccid
            hapus()
            'CreateCookie("ID", 0)
            Dim ds2 As DataSet = cInv.Select_StockData(id, ccid, custid)
            If ds2.Tables(0).Rows.Count > 0 Then
                txtLosF.Text = ds2.Tables(0).Rows(0).Item("Factor1").ToString
                txtstkNo.Text = ds2.Tables(0).Rows(0).Item("number").ToString
                lblUnit1.Text = ds2.Tables(0).Rows(0).Item("unit1").ToString
                lblUnit2.Text = ds2.Tables(0).Rows(0).Item("unit2").ToString
                lblunit22.Text = ds2.Tables(0).Rows(0).Item("unit2").ToString
                lblcostu2.Text = "/ " & ds2.Tables(0).Rows(0).Item("unit2").ToString
                lblcostu1.Text = "/ " & ds2.Tables(0).Rows(0).Item("unit1").ToString
                txtTotalStk.Text = 0
                txtCost.Text = ds2.Tables(0).Rows(0).Item("cost").ToString
                txtCostu1.Text = ds2.Tables(0).Rows(0).Item("cost").ToString * ds2.Tables(0).Rows(0).Item("Factor1").ToString
                txtinstock.Text = ds2.Tables(0).Rows(0).Item("ttlqty").ToString
            End If

            Exit Sub
        End If
        'CreateCookie("ID", ds.Tables(0).Rows(0).Item("id"))
        DdlTo.SelectedValue = ds.Tables(0).Rows(0).Item("cc")
        ddlItem.SelectedValue = ds.Tables(0).Rows(0).Item("stk_id")
        txtUnit1.Text = ds.Tables(0).Rows(0).Item("U1")
        txtUnit2.Text = ds.Tables(0).Rows(0).Item("U2")
        txtstkNo.Text = ds.Tables(0).Rows(0).Item("stk_no")
        txtLosF.Text = ds.Tables(0).Rows(0).Item("factor1")
        lblUnit1.Text = ds.Tables(0).Rows(0).Item("U1_nm")
        lblUnit2.Text = ds.Tables(0).Rows(0).Item("U2_nm")
        lblunit22.Text = ds.Tables(0).Rows(0).Item("U2_nm")
        lblcostu2.Text = "/ " & ds.Tables(0).Rows(0).Item("U2_nm")
        lblcostu1.Text = "/ " & ds.Tables(0).Rows(0).Item("U1_nm")
        txtTotalStk.Text = Val(txtUnit1.Text) * Val(txtLosF.Text) + Val(txtUnit2.Text) 'ds.Tables(0).Rows(0).Item("ttlstock")
        txtCost.Text = FormatCurrency(ds.Tables(0).Rows(0).Item("totCost"), 0).Replace("$", "")
        txtCostu1.Text = ds.Tables(0).Rows(0).Item("cost").ToString * ds.Tables(0).Rows(0).Item("Factor1").ToString
        txtinstock.Text = ds.Tables(0).Rows(0).Item("ttlqty")
        btnUpdate.Visible = True
        btnInsert.Visible = False
    End Sub

    Protected Sub btnInsert_Click(ByVal sender As Object, ByVal e As EventArgs) 'Handles btnInsert.Click

        If adjustnow = 0 Then
            insert()
        Else
            insert(ddladjustnow.SelectedValue)
        End If

    End Sub

    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As EventArgs) 'Handles btnUpdate.Click
        If adjustnow = 0 Then
            insert()
        Else
            insert(ddladjustnow.SelectedValue)
        End If
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancel.Click

        Try
            hapus()
            mfunction.CreateCookie("stkid", 0)
            mfunction.CreateCookie("Ccid", 0)

            Response.Redirect("~/Pages_StockOpname.aspx", False)


        Catch ex As Exception
            errLog(Err.Description, "WebBatchEdit:btnCancel_Click")
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
    Sub insert(ByVal Optional adjustnow As Integer = 0) '0 kemaren, 1 sekarang, 2 opening hari ini dikurang transaksi berjalan

        Dim msg As String = ""
        Dim newLine As String = "\n"

        Try

            If Val(txtCost.Text) <= 0 And cInv.cekstocklink(custid, ddlItem.SelectedValue) = 1 Then
                msg = "Cost cannot 0" & newLine
                'Exit Sub
            End If

            If IsNumeric(txtUnit1.Text) = False Or IsNumeric(txtUnit2.Text) = False Then
                msg = msg + "Unit must be numeric" & newLine
                'Exit Sub
            Else
                If txtUnit1.Text < 0 Or txtUnit2.Text < 0 Then
                    msg = msg + "qty < 0" & newLine
                End If
            End If
            If cInv.check_belumTrans(ddlItem.SelectedValue, DdlTo.SelectedValue, custid) = False Then

                If ddladjustnow.SelectedValue <> 2 Then
                    msg = msg + "already have transaction" & newLine
                End If
                'Exit Sub
            End If

            'If Val(txtTotalStk.Text) = 0 Then
            '    msg = msg + "Qty must be fill" & newLine
            '    'Exit Sub
            'End If

            If cInv.get_bclose(ddlItem.SelectedValue, DdlTo.SelectedValue, Date.Now.AddDays(-1).Date.ToString("yyyy-MM-dd 00:00:00"), custid) = True Then
                Dim adtype As Integer = 0
                adtype = cInv.get_typestockopname(ddlItem.SelectedValue, DdlTo.SelectedValue, Date.Now.AddDays(-1).Date.ToString("yyyy-MM-dd 00:00:00"), custid)
                If adtype > 0 Then

                    adjustnow = Right(adtype.ToString, 1)
                Else
                    'msg = msg + "this stock has been adjusted" & newLine
                End If
            End If
            If msg = "" Then
                If adjustnow = 0 Then
                    cInv.insertStockOpname(DdlTo.SelectedValue, ddlItem.SelectedValue, ddlItem.SelectedItem.Text, txtUnit1.Text,
                                txtUnit2.Text, txtLosF.Text, txtTotalStk.Text, txtCost.Text, lblUnit1.Text, lblUnit2.Text, username, username, txtstkNo.Text, DdlTo.SelectedItem.Text, username, custid)
                ElseIf adjustnow = 1 Then
                    cInv.insertStockOpname(DdlTo.SelectedValue, ddlItem.SelectedValue, ddlItem.SelectedItem.Text, txtUnit1.Text,
                                txtUnit2.Text, txtLosF.Text, txtTotalStk.Text, txtCost.Text, lblUnit1.Text, lblUnit2.Text, username, username, txtstkNo.Text, DdlTo.SelectedItem.Text, username, custid, adjustnow)
                ElseIf adjustnow = 2 Then
                    cInv.insertStockOpname(DdlTo.SelectedValue, ddlItem.SelectedValue, ddlItem.SelectedItem.Text, txtUnit1.Text,
                                txtUnit2.Text, txtLosF.Text, txtTotalStk.Text, txtCost.Text, lblUnit1.Text, lblUnit2.Text, username, username, txtstkNo.Text, DdlTo.SelectedItem.Text, username, custid, adjustnow)
                End If

            Else

                Dim script As String = "window.onload = function(){ alert('"
                script += msg
                script += "');"
                script += "window.location = '"
                script += "Pages_StockOpname.aspx"
                script += " '; }"
                ClientScript.RegisterStartupScript(Me.GetType(), "Redirect", script, True)

                'notsuccesfull(msg)
                Exit Sub
            End If

            'MsgBox("Data Berhasil Di input Mysql")

            Response.Redirect("~/Pages_StockOpname.aspx", False)

        Catch ex As Exception
            'MsgBox(ex.Message)
        End Try

    End Sub




    Protected Sub ddlItem_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlItem.SelectedIndexChanged
        Try
            If ddlItem.SelectedValue > 0 Then


                Dim ds As DataSet = cInv.Select_StockData(ddlItem.SelectedValue, DdlTo.SelectedValue, custid)
                If ds.Tables(0).Rows.Count > 0 Then
                    txtLosF.Text = ds.Tables(0).Rows(0).Item("Factor1").ToString
                    txtstkNo.Text = ds.Tables(0).Rows(0).Item("number").ToString
                    lblUnit1.Text = ds.Tables(0).Rows(0).Item("unit1").ToString
                    lblUnit2.Text = ds.Tables(0).Rows(0).Item("unit2").ToString
                    lblunit22.Text = ds.Tables(0).Rows(0).Item("unit2").ToString
                    lblcostu2.Text = "/ " & ds.Tables(0).Rows(0).Item("unit2").ToString
                    lblcostu1.Text = "/ " & ds.Tables(0).Rows(0).Item("unit1").ToString
                    txtinstock.Text = FormatCurrency(Val(ds.Tables(0).Rows(0).Item("ttlqty").ToString), 2).Replace("$", "")
                    txtCost.Text = ds.Tables(0).Rows(0).Item("cost").ToString
                    txtCostu1.Text = ds.Tables(0).Rows(0).Item("cost").ToString * ds.Tables(0).Rows(0).Item("Factor1").ToString

                End If
            End If
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub DdlTo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DdlTo.SelectedIndexChanged
        Try
            If DdlTo.SelectedValue > 0 Then
                Dim ds As DataSet = cInv.Select_StockData(ddlItem.SelectedValue, DdlTo.SelectedValue, custid)
                If ds.Tables(0).Rows.Count > 0 Then
                    txtLosF.Text = ds.Tables(0).Rows(0).Item("Factor1").ToString
                    txtstkNo.Text = ds.Tables(0).Rows(0).Item("number").ToString
                    lblUnit1.Text = ds.Tables(0).Rows(0).Item("unit1").ToString
                    lblUnit2.Text = ds.Tables(0).Rows(0).Item("unit2").ToString
                    lblunit22.Text = ds.Tables(0).Rows(0).Item("unit2").ToString
                    lblcostu2.Text = "/ " & ds.Tables(0).Rows(0).Item("unit2").ToString
                    lblcostu1.Text = "/ " & ds.Tables(0).Rows(0).Item("unit1").ToString
                    txtinstock.Text = FormatCurrency(Val(ds.Tables(0).Rows(0).Item("ttlqty").ToString), 2).Replace("$", "")
                    txtCost.Text = ds.Tables(0).Rows(0).Item("cost").ToString
                    txtCostu1.Text = ds.Tables(0).Rows(0).Item("cost").ToString * ds.Tables(0).Rows(0).Item("Factor1").ToString
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub


    Protected Sub txtUnit1_TextChanged(sender As Object, e As EventArgs) Handles txtUnit1.TextChanged
        If txtUnit1.Text <> "" Then

            If IsNumeric(txtUnit1.Text) = False Then
                txtUnit1.Text = 0
            End If

            Dim total As Decimal = Val(txtUnit1.Text) * Val(txtLosF.Text) + Val(txtUnit2.Text)
            txtTotalStk.Text = total 'FormatCurrency(total, 0).Replace("$", "")

        Else
            txtUnit1.Text = 0

        End If
    End Sub

    Protected Sub txtUnit2_TextChanged(sender As Object, e As EventArgs) Handles txtUnit2.TextChanged
        If txtUnit2.Text <> "" Then
            If IsNumeric(txtUnit2.Text) = False Then
                txtUnit2.Text = 0
            End If

            Dim total As Decimal = Val(txtUnit1.Text) * Val(txtLosF.Text) + Val(txtUnit2.Text)
            txtTotalStk.Text = total 'FormatCurrency(total, 0).Replace("$", "")

        Else
            txtUnit2.Text = 0

        End If
    End Sub

    Protected Sub ddladjustnow_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddladjustnow.SelectedIndexChanged
        mfunction.CreateCookie("adjustnow", ddladjustnow.SelectedValue)
    End Sub

    Protected Sub txtCost_TextChanged(sender As Object, e As EventArgs) Handles txtCost.TextChanged
        Try
            txtCostu1.Text = txtCost.Text * txtLosF.Text
        Catch ex As Exception

        End Try


    End Sub

    Protected Sub txtCostu1_TextChanged(sender As Object, e As EventArgs) Handles txtCostu1.TextChanged
        Try

            txtCost.Text = txtCostu1.Text / txtLosF.Text
        Catch ex As Exception

        End Try

    End Sub
End Class
