Imports System.Threading
Imports System.Net
Imports System.Text
Imports System.IO
Imports System.Data
Imports System.Drawing
Imports NPOI.SS.Formula.Functions
Imports System.Globalization

Public Class Pages_InvCnvrEdit
    Inherits System.Web.UI.Page
    Private sNameuser As String
    Private Userid As Integer
    Private icustid As Integer
    Private imodul As Integer
    Private iModPriv As Integer
    Private irptpriv As Integer
    Private irptf As Integer
    Private mfunction As New ModFunction
    Private Mtid As String
    Private c_CC As Integer
    Private c_stkid As Integer
    Private c_linkid As String
    Private Dos As String

    Private iflag As Integer
    Private Cnvr_date As Date
    Private CIcc As New Cls_ICc
    'Private acc As String
    Private bacc As Boolean = False

    Private cInv As New clsInventory
    Private clsCnvers As New Cls_Conversion
    Private CStk As New Cls_IStk


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
    Public Function InvPriv() As Integer
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        If cookie Is Nothing Then
            Return -1
        Else
            Return cookie("invpriv")
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
        Dim c_cmd As New clsDbo("")
        'Dim notes As String
        Try
            Mtid = Request.Cookies("iBatchID").Value
            c_CC = Request.Cookies("cc").Value
            c_stkid = Request.Cookies("stk_id").Value
            c_linkid = 0 'Request.Cookies("linkid").Value

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
                    'loadItem()
                    loadRcp()
                    'editlist(MTid)

                    If c_CC = 0 Or c_stkid = 0 Then
                        'hapus()
                        txtcreatedate.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                    Else
                        editlist(Mtid, custid)
                    End If
                    Dim dt, dt2 As New DataTable
                    dt = cInv.getDTInvBatch(Mtid, custid)
                    ViewState("InvRcv") = dt
                End If
                BindData()

                bindingstk()
                ScriptManager.GetCurrent(Me).RegisterPostBackControl(RadButton1)
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
        Catch ex As Exception
            errLog(Err.Description, "Pages_InvCnvrEdit:Page_Load")
            Err.Clear()
        End Try
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
    Protected Sub BindData()

        Try
            If custid() = 0 Then
                Response.Redirect("~/Default.aspx", False)
            Else
                'Dim totalcost As Double = 0

                'For Each dr As DataRow In DirectCast(ViewState("InvRcv"), DataTable).Rows
                '    totalcost += Convert.ToDouble(dr("ttlCost"))
                'Next

                'DgvBatch.Columns(11).FooterText = totalcost.ToString("#,##0.00")
                DgvBatch.DataSource = DirectCast(ViewState("InvRcv"), DataTable)
                DgvBatch.DataBind()
                DgvBatch.ShowFooter = True

                If DgvBatch.Rows.Count > 0 Then
                    DgvBatch.Columns(12).Visible = True
                    btnedit.Visible = True
                    If Mtid <> "" Then
                        If GetBackground(Mtid) = 0 Then
                            DgvBatch.Columns(12).Visible = False
                            btnedit.Visible = False

                        End If
                    End If

                Else
                    DgvBatch.Columns(12).Visible = False
                    btnedit.Visible = False
                End If
            End If
        Catch ex As Exception
            errLog(Err.Description, "Pages_InvCnvrEdit:BindData")
            Err.Clear()
        End Try

    End Sub
    Sub bindingstk()

        Try
            If custid() = 0 Then
                Response.Redirect("~/Default.aspx", False)
            Else
                Dim dt, dt2 As New DataTable
                dt = DirectCast(ViewState("InvRcv"), DataTable)
                'dt2 = cInv.getDtStk(dt, custid, txtSearch.Text, modulepriv)
                dt2 = CStk.SelectDataTable_Stkall(dt, custid, txtSearch.Text, modulepriv, DdlTo.SelectedValue)
                'WService.getDtStk(dt, cust, txtSearch.Text, ModPrv)
                ViewState("InvStk") = dt2
                LvStock.DataSource = DirectCast(ViewState("InvStk"), DataTable)
                LvStock.DataBind()
            End If
        Catch ex As Exception
            errLog(Err.Description, "Pages_InvCnvrEdit:bindingstk")
            Err.Clear()
        End Try


    End Sub
    Protected Function GetBackground(ByVal batchid As String)

        Dim imgstat As Integer = 0
        Dim ccnvr As New Cls_Conversion
        Try
            imgstat = ccnvr.batch_date(batchid)

        Catch ex As Exception
            errLog(Err.Description, "Pages_InvCnvrEdit:GetBackground")
            Err.Clear()
        End Try


        Return imgstat

    End Function
    'Sub hapus()

    '    txtUnit1.Text = 0
    '    txtUnit2.Text = 0
    '    txtTotalStk.Text = 0
    '    txtLosF.Text = 0
    '    txtStock.Text = 0
    '    txtStock.ReadOnly = True
    '    txtTotalStk.ReadOnly = True


    '    btnUpdate.Visible = False
    '    btnInsert.Visible = True
    'End Sub

    Sub editlist(ByVal id As Integer, ByVal custid As Integer)

        Dim invC As CultureInfo = CultureInfo.InvariantCulture
        Dim ds As New DataSet
        ds = clsCnvers.SelectDataset_InvConversionEdit(id, custid)
        If ds.Tables(0).Rows.Count = 0 Then
            Exit Sub
        End If

        DdlTo.SelectedValue = ds.Tables(0).Rows(0).Item("cc")
        'ddlItem.SelectedValue = ds.Tables(0).Rows(0).Item("stk_id")
        'Dim unit1 As Decimal = ds.Tables(0).Rows(0).Item("U1")

        txtcreatedate.Text = ds.Tables(0).Rows(0).Item("invdate")
        'txtUnit1.Text = unit1.ToString("N", invC)
        'Dim unit2 As Decimal = ds.Tables(0).Rows(0).Item("U2")
        'txtUnit2.Text = unit2.ToString("N", invC)


        'txtstkNo.Text = ds.Tables(0).Rows(0).Item("stk_no")
        Dim losf As Decimal = ds.Tables(0).Rows(0).Item("factor1")
        'txtLosF.Text = losf.ToString("N", invC)

        'lblUnit1.Text = ds.Tables(0).Rows(0).Item("U1_nm")
        'lblUnit2.Text = ds.Tables(0).Rows(0).Item("U2_nm")
        'lbltotal.Text = ds.Tables(0).Rows(0).Item("U2_nm") 'ds.Tables(0).Rows(0).Item("u2").ToString
        'lblcurrent.Text = ds.Tables(0).Rows(0).Item("U2_nm") 'ds.Tables(0).Rows(0).Item("u2").ToString
        Dim totstk As Decimal = ds.Tables(0).Rows(0).Item("ttlstock")
        'txtTotalStk.Text = totstk.ToString("N", invC)
        'txtTotalStk.ReadOnly = True
        Dim stock As Decimal = ds.Tables(0).Rows(0).Item("stock")
        'txtStock.Text = stock.ToString("N", invC)
        'txtStock.ReadOnly = True
        'ddlItem.Enabled = False
        'DdlTo.Enabled = False

        'Perubahan 3 maret 2017, jadi klo transaksi2 sebelumnya ga bisa di edit
        If ds.Tables(0).Rows(0).Item("InvDate") < DateTime.Now Then

            btnUpdate.Visible = False
            If username() = "support" Then
                btnUpdate.Visible = True
            End If
        Else
            btnUpdate.Visible = True
        End If

        btnInsert.Visible = False
    End Sub



    Protected Sub Page_unLoad(sender As Object, e As EventArgs) Handles Me.Unload
    End Sub

    Public Function username() As String
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        If cookie Is Nothing Then
            Return ""
        Else
            Return cookie("userName")
        End If

    End Function



    Sub delete(ByVal id As Integer)
        Try
            'WService.DeleteInvRcv(id, cust, username)
            cInv.DeleteInvRcvDetail(id, custid, username)
        Catch ex As Exception
            errLog(Err.Description, "Pages_InvCnvrEdit:Delete")
            Err.Clear()
        End Try
    End Sub
    Sub deleteitem(ByVal id As Integer, ByVal stkid As Integer)
        Try
            'WService.DeleteInvRcv(id, cust, username)
            cInv.DeleteInvRcvitem(id, stkid, custid, username)
        Catch ex As Exception
            errLog(Err.Description, "Pages_InvCnvrEdit:deleteitem")
            Err.Clear()
        End Try
    End Sub
    Public Sub DeleteRcpDtl(ByVal id As Integer, ByVal Custid As Integer, ByVal username As String)
        'Dim pc As String = pic.ToString
        Try

            Dim str As String
            Dim c_cmd As New Cls_command

            str = "DELETE FROM cld_inv_rcpdtl  where id = " & id & ""

            c_cmd.executeNonQuery(str)

            'Revisi 10 Mei
            insertLogModule(username, "InvRcpdtl", "Delete " & id, Custid)

        Catch ex As Exception

            errLog(Err.Description, "Pages_InvCnvrEdit:DeleteInvrcpdtl")
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


    Private Sub ShowToastr(ByVal page As Page, ByVal message As String, ByVal title As String, Optional ByVal type As String = "info", Optional ByVal clearToast As Boolean = False, Optional ByVal pos As String = "toast-top-left", Optional ByVal Sticky As Boolean = False)
        Dim toastrScript As String = [String].Format("Notify('{0}','{1}','{2}', '{3}', '{4}', '{5}');", message, title, type, clearToast, pos, Sticky)
        page.ClientScript.RegisterStartupScript(page.[GetType](), "toastr_message", toastrScript, addScriptTags:=True)
    End Sub

    'Protected Sub txtUnit1_TextChanged(sender As Object, e As EventArgs) Handles txtUnit1.TextChanged

    'End Sub

    Protected Sub btnInsert_Click(sender As Object, e As EventArgs) Handles btnInsert.ServerClick

    End Sub

    Protected Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click

    End Sub
    Protected Sub btnBack_ServerClick(sender As Object, e As EventArgs) Handles btnBack.ServerClick
        Response.Redirect("~/Pages_Inv_Cnvr.aspx", False)
    End Sub

    Sub loadRcp()
        'Dim cookie As HttpCookie = Request.Cookies("UserName")
        Dim crcp As New Cls_IRcp
        'Dim Cust As Integer = cookie("CustID")
        Dim ds As New DataSet
        ds = CRcp.SelectDataset_rcp(custid)
        'DdlLink.DataSource = ds
        'DdlLink.DataValueField = "Id"
        'DdlLink.DataTextField = "Name"
        'DdlLink.DataBind()

    End Sub

    Protected Sub btnedit_ServerClick(sender As Object, e As EventArgs) Handles btnedit.ServerClick

    End Sub
    Protected Sub imgbtnDelete_Click(sender As Object, e As ImageClickEventArgs)

    End Sub

    Protected Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.ServerClick
        Try
            If txtSearch.Text = "" Then
                ShowToastr(Page, "Text Search tidak boleh kosong", "Error", "Error", False, "toast-top-right", False)
            Else
                BindCheckItem()
                bindingstk()
                CheckedItem()
            End If
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Pop", "openModalItem();", True)

        Catch ex As Exception
            errLog(Err.Description, "Pages_InvCnvrEdit:btnSearch_Click")
            Err.Clear()
        End Try
    End Sub
    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        Try
            Threading.Thread.Sleep(500)
            BindCheckItem()
            bindingstk()
            CheckedItem()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Pop", "openModalItem();", True)
            txtSearch.Attributes("onfocus") = "javascript:this.setSelectionRange(this.value.length,this.value.length);"
            txtSearch.Focus()
        Catch ex As Exception
            errLog(Err.Description, "Pages_InvCnvrEdit:txtSearch_TextChanged")
            Err.Clear()
        End Try
    End Sub
    Private Sub BindCheckItem()
        Try

            If CBool(IIf(IsNothing(ViewState("ListItem")), True, False)) Then


                Dim ListItem2 As List(Of Item_Detail) = New List(Of Item_Detail)()

                Dim dt, dt2 As New DataTable
                dt = DirectCast(ViewState("InvRcv"), DataTable)
                dt2 = cInv.getDtStk(dt, custid, txtSearch.Text, modulepriv)
                If dt2.Rows.Count > 0 Then
                    For Each dtrow As DataRow In dt2.Rows
                        ListItem2.Add(New Item_Detail With {
                                           .ItemName = dtrow.Item("name"),
                                           .Status = False
                                           })
                    Next
                End If


                For Each item As ListViewItem In LvStock.Items
                    Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox1"), CheckBox)
                    Dim Labels As Label = DirectCast(item.FindControl("lblName"), Label)
                    Dim label2 As String = Labels.Text



                    If ck.Checked Then
                        For Each a In ListItem2
                            If a.ItemName = label2 Then
                                a.Status = True
                            End If
                        Next
                        '  clsCheck.IDBrand.Add(label2)
                    End If
                Next
                ViewState("ListItem") = ListItem2

            Else

                Dim ListItem2 As List(Of Item_Detail) = TryCast(ViewState("ListItem"), List(Of Item_Detail))

                For Each item As ListViewItem In LvStock.Items
                    Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox1"), CheckBox)
                    Dim Labels As Label = DirectCast(item.FindControl("lblName"), Label)
                    Dim label2 As String = Labels.Text

                    If ck.Checked Then
                        For Each a In ListItem2
                            If a.ItemName = label2 Then
                                a.Status = True
                            End If
                        Next

                    Else
                        For Each a In ListItem2
                            If a.ItemName = label2 Then
                                a.Status = False
                            End If
                        Next
                    End If
                Next
                ViewState("ListItem") = ListItem2
            End If
        Catch ex As Exception
            errLog(Err.Description, "Pages_InvCnvrEdit:BindCheckItem")
            Err.Clear()
        End Try
    End Sub

    Private Sub CheckedItem()
        Try


            Dim ListStore As List(Of Item_Detail) = TryCast(ViewState("ListItem"), List(Of Item_Detail))

            Dim i As Integer = 0
            For Each item As ListViewItem In LvStock.Items
                Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox1"), CheckBox)
                Dim Labels As Label = DirectCast(item.FindControl("lblName"), Label)
                Dim label2 As String = Labels.Text

                For Each a In ListStore
                    If a.ItemName = label2 Then
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
            errLog(Err.Description, "Pages_InvCnvrEdit:CheckedItem")
            Err.Clear()
        End Try
    End Sub
    Protected Sub btnClearSearch_Click(sender As Object, e As EventArgs)
        Try
            For Each item As ListViewItem In LvStock.Items
                Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox1"), CheckBox)

                ck.Checked = False


            Next
            txtSearch.Text = ""
            lblnoitem.Text = 0

            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Pop", "openModalItem();", True)
        Catch ex As Exception
            errLog(Err.Description, "Pages_InvCnvrEdit:btnClearSearch_Click")
            Err.Clear()
        End Try
    End Sub
    Protected Sub RadButton1_ServerClick(sender As Object, e As EventArgs) Handles RadButton1.ServerClick
        Try
            AddBatch()
        Catch ex As Exception

        End Try
    End Sub
    Sub AddBatch()
        Dim dt As New DataTable
        Try
            Dim i As Integer = 0
            If DirectCast(ViewState("InvRcv"), DataTable).Rows.Count >= My.Settings.limitorder Then
                notsuccesfull("Silakan batch di bawah " & My.Settings.limitorder & " item, pisah jika lebih !!")
                GoTo ef
            End If
            For Each item As ListViewItem In LvStock.Items
                Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox1"), CheckBox)

                If ck.Checked Then
                    Try
                        Dim cstk As New clsInventory_stk
                        Dim ds As DataSet = cstk.SelectData_Inv_Stkcls((DirectCast(item.FindControl("lblID"), Label).Text), DdlTo.SelectedValue, custid, Date.Now.ToString("yyyy-MM-dd"))
                        If ds.Tables(0).Rows.Count > 0 Then
                            Dim u1 As String = ds.Tables(0).Rows(0).Item("Unit1").ToString
                            Dim u2 As String = ds.Tables(0).Rows(0).Item("Unit2").ToString
                            dt = DirectCast(ViewState("InvRcv"), DataTable)
                            Dim costu1 As String = ds.Tables(0).Rows(0).Item("coststk").ToString * ds.Tables(0).Rows(0).Item("Factor1")
                            Dim costu2 As String = ds.Tables(0).Rows(0).Item("coststk").ToString
                            dt.Rows.Add(0, custid, Date.Now.ToString("yyyy-MM-dd"), 1, DdlTo.SelectedValue, (DirectCast(item.FindControl("lblID"), Label).Text), (DirectCast(item.FindControl("lblName"), Label).Text), 0, 0, ds.Tables(0).Rows(0).Item("Factor1"), 0, costu1, costu2, "", 1, ds.Tables(0).Rows(0).Item("Unit1").ToString, ds.Tables(0).Rows(0).Item("Unit2").ToString, username, Date.Now.ToString("yyyy-MM-dd"), "", Date.Now.ToString("yyyy-MM-dd"), ds.Tables(0).Rows(0).Item("Number").ToString, DdlTo.SelectedItem, 0)
                            ViewState("InvRcv") = dt
                            Dim dtStk As DataTable = DirectCast(ViewState("InvStk"), DataTable)
                            dtStk.Rows(i).Delete()
                            dtStk.AcceptChanges()
                            i = i - 1
                        End If

                        BindData()
                        txtSearch.Text = ""
                        bindingstk()
                    Catch error_t As Exception
                        MsgBox(error_t.ToString)
                    End Try

                End If
                i = i + 1

                If dt.Rows.Count >= My.Settings.limitorder Then
                    notsuccesfull("Hanya bisa order " & dt.Rows.Count.ToString & " item !!")
                    Exit For
                End If

            Next
ef:
            'update()
            btnedit.InnerText = "edit"
        Catch ex As Exception
            'MsgBox(ex.Message)
            errLog(Err.Description, "Pages_InvCnvrEdit:Add")
            Err.Clear()
        End Try

    End Sub
    <Serializable>
    Public Class Item_Detail 'Class untuk ListItem
        Public Property ItemName As String = ""
        Public Property Status As Boolean = False
    End Class
    'Start : Store



    'END : Store





End Class
