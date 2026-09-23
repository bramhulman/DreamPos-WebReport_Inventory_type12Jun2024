Imports System.IO
Imports System.Net
Imports ClosedXML.Excel
Imports DreamPos_WebReport.UserList
Imports Microsoft.VisualBasic.ApplicationServices
Imports NPOI.SS.UserModel
Imports NPOI.XSSF.UserModel
Public Class Pages_ListWaste
    Inherits System.Web.UI.Page
    Private sNameuser As String
    Private icustid As Integer
    Private imodul As Integer
    Private iModPriv As Integer
    Private irptpriv As Integer
    Private irptf As Integer
    Private mfunction As New ModFunction
    Private cInv As New clsInventory
    Private CWid As Integer
    'Private clsInventory As New clsInventory_stkOpname
    Dim invpriv As Integer

    Private WasteInfo As New WasteListInfo()

    Private Property WasteInfos As List(Of WasteListInfo)
        Get
            Dim list = TryCast(ViewState("WasteInfos"), List(Of WasteListInfo))
            If list Is Nothing Then list = New List(Of WasteListInfo)()
            Return list
        End Get
        Set(value As List(Of WasteListInfo))
            ViewState("WasteInfos") = value
        End Set
    End Property
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
    Public Function modul() As Integer
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        Return cookie("modul")

    End Function
    Public Function rptf() As Integer
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")

        Return cookie("rptf")

    End Function
    Public Function inv_priv() As Integer
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")

        Return cookie("invpriv")

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
            sNameuser = username()
            icustid = custid()
            imodul = modul()
            iModPriv = modulepriv()
            irptpriv = rptpriv()
            irptf = rptf()
            invpriv = inv_priv()
            If (Page.IsPostBack) Then
            Else

                'txtDate.Text = Format(CDate(Date.Now), "MM/dd/yyyy")
                'loadOutlet()

                lblTitle.Text = "List Waste " & Date.Now.Date.ToString("dd MMM yyyy") & ""
                'txtDate.Text = Format(Now.AddDays(-1), "dd MMM yyyy")
                binding()
            End If
            gvWaste.DataSource = WasteInfos
            gvWaste.DataBind()
        Else
            Dim script As String = "window.onload = function(){ alert('"
            script += "Please Login"
            script += "');"
            script += "window.location = '"
            script += "../Default"
            script += " '; }"
            ClientScript.RegisterStartupScript(Me.GetType(), "Redirect", script, True)
        End If
        'If (username() = My.Settings.admin Or username() = "support") Then
        '    btnnew.Visible = True
        'End If

    End Sub
    Sub binding()

        Try

            Dim dtWaste As New DataTable
            dtWaste = cInv.getInvRwasteSearch2("", modulepriv, username).Tables(0)
            If dtWaste.Rows.Count > 0 Then 'And dtopname.Rows.Count < 100 Then
                WasteInfos = ToList(Of WasteListInfo)(dtWaste)
                ViewState("WasteInfos") = WasteInfos 'dtopname

            Else
                ViewState("WasteInfos") = WasteInfos 'dtopname

            End If
            dtWaste.Rows.Clear()
            dtWaste.Dispose()
        Catch ex As Exception
            errLog(Err.Description, "Pages_ListWaste:binding")
            Err.Clear()
        End Try

    End Sub
    Sub delete(ByVal id As String, ByVal ccid As Integer)
        Try
            DeleteInvStkwaste(id, ccid, custid, username)
            binding()
        Catch ex As Exception
            errLog(Err.Description, "Pages_ListWaste:delete")
            Err.Clear()
        End Try
    End Sub
    Public Sub DeleteInvStkwaste(ByVal id As String, ByVal ccid As Integer, ByVal Custid As Integer, ByVal username As String)
        'Dim pc As String = pic.ToString
        Try
            'perubahan dari type = 5 menjadi type = 12

            Dim strTr As String
            Dim c_cmd As New Cls_command

            strTr = "DELETE FROM cld_inv_trans where stk_id = " & id & " and cc = " & ccid & " and type = 12 and custid = " & Custid & ""
            c_cmd.executeNonQuery(strTr)
            c_cmd.ReNew()
            c_cmd.executeNonQuery("update cld_inv_transstockcls set  bclose = 0, closeqty = 0, closecost = 0
            where stkid = " & id & " and ccid = " & ccid & " and invdate = '" & Now.Date.ToString("yyyy-MM-dd") & "' and custid = " & Custid & "")

            insertLogModule(username, "Inv_inv_transstock", "Delete", Custid)
        Catch ex As Exception
            errLog(Err.Description, "Pages_ListWaste:Deleteinv_transstock")
            Err.Clear()
        End Try
    End Sub






    Protected Sub gvwaste_PreRender(sender As Object, e As EventArgs) 'Handles gvOpname.PreRender
        Dim gv As GridView = CType(sender, GridView)
        Try
            If gv IsNot Nothing Then
                If gv.HeaderRow IsNot Nothing Then
                    gv.HeaderRow.TableSection = TableRowSection.TableHeader
                End If

                If gv.FooterRow IsNot Nothing Then
                    gv.FooterRow.TableSection = TableRowSection.TableFooter
                End If
            End If
        Catch ex As Exception
            errLog(Err.Description, "Pages_ListWaste:gvwaste_PreRender")
            Err.Clear()
        End Try
    End Sub
    Protected Sub gvWaste_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs)




        If e.CommandName.Equals("editRecord") Then

            Dim rowindex As Integer = Convert.ToInt32(e.CommandArgument)
            Dim invdate As Date = CDate(gvWaste.DataKeys(rowindex).Values("invdate"))
            Dim ccid As Integer = CInt(gvWaste.DataKeys(rowindex).Values("ccid"))
            Dim stkid As Integer = CInt(gvWaste.DataKeys(rowindex).Values("stkid"))
            If WasteInfos.Count = 0 Then

                WasteInfos = DataTableToList(Of WasteListInfo)(cInv.getInvRwasteSearch2("", modulepriv, username).Tables(0))
            End If
            Dim wasteInfo = WasteInfos.FirstOrDefault(Function(i) i.stkid = stkid And i.ccid = ccid And i.invdate = invdate)
            If wasteInfo Is Nothing Then Exit Sub

            ' Simpan untuk btnUpdate_Click
            ViewState("CurrentWaste") = wasteInfo

            HfUpdatecc.Value = wasteInfo.ccid
            Hffactor1.Value = wasteInfo.factor1
            Hfstkno.Value = wasteInfo.stkno
            HfUpdatestock.Value = wasteInfo.stkid
            HfUpdatedate.Value = wasteInfo.invdate
            txtcostcentre.Text = wasteInfo.ccname
            txtcost.Text = wasteInfo.cost
            txtfactor1.Text = wasteInfo.factor1
            'txtclosecost.Text = wasteInfo.opencost
            txtttlqty.Text = wasteInfo.ttlqty
            lblttlu2.Text = wasteInfo.unit2
            txtwasteqtyu1.Text = 0 'OpnameInfo.openqtyu1
            lblunit1.Text = wasteInfo.unit1
            'txtcloseqtyu2.Text = wasteInfo.openqty
            lblunit2.Text = wasteInfo.unit2
            txtStockNameUpdate.Text = wasteInfo.stkname
            'upEdit.Update()
            Dim upEditPanel As UpdatePanel = CType(FindControl("upEdit"), UpdatePanel)
            If upEditPanel IsNot Nothing Then
                upEditPanel.Update()
            End If
            Dim script As String = "$('#editModal').modal('show');"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModal", script, True)


        End If


    End Sub
    'Protected Sub txtDate_TextChanged(sender As Object, e As EventArgs) Handles txtDate.TextChanged
    '    binding()
    'End Sub
    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As EventArgs) 'Handles btnUpdate.Click

        Try
            insert()
        Catch ex As Exception
            errLog(Err.Description, "Pages_ListWaste:btnUpdate_Click")
            Err.Clear()
        End Try
    End Sub
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


            c_cmd.dispose()
            ds.Tables.Clear()
            ds.Dispose()
        Catch ex As Exception
            errLog(Err.Description, "Pages_ListWaste:delete")
            Err.Clear()
        End Try
    End Sub
    Sub insert()

        Try
            Dim msg As String = ""
            Dim newLine As String = "\n"
            Dim ds As New DataSet
            'Dim rwid As String = ""
            Dim upwaste As Decimal = 0
            Dim invdate As Date = CDate(Now)
            Dim ttlwaste As Decimal = 0
            If CWid <> 0 Then
                delete(CWid)
            End If

            CWid = 0

            If IsNumeric(txtwasteqtyu1.Text) = False Or IsNumeric(txtwasteqtyu2.Text) = False Then
                notsuccesfull("Unit must be numeric")
                Exit Sub
            End If

            If Val(txtwasteqtyu1.Text) = 0 And Val(txtwasteqtyu2.Text) = 0 Then
                msg = msg + "Unit must be Fill " & newLine
            End If
            ttlwaste = Val(txtwasteqtyu1.Text) * txtfactor1.Text + Val(txtwasteqtyu2.Text)
            If (ttlwaste) > Val(txtttlqty.Text) Then

                If txtStockNameUpdate.Enabled = False Then

                Else
                    msg = msg + "Total Input Cannot more than Stock available" & newLine
                End If
            End If



            If msg <> "" Then
                notsuccesfull(msg)
                Exit Sub
            End If


            ds = cInv.insertRwaste2(CWid, HfUpdatecc.Value, HfUpdatestock.Value, txtStockNameUpdate.Text, txtwasteqtyu1.Text,
                             txtwasteqtyu2.Text, txtfactor1.Text, ttlwaste, txtcost.Text, lblunit1.Text, lblunit2.Text, username, username, Hfstkno.Value, txtcostcentre.Text, username, custid, CDate(Now), txtNotes.Text)

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
            clsprocess.InvCc = HfUpdatecc.Value
            clsprocess.InvCcName = txtcostcentre.Text
            clsprocess.InvCustId = custid()
            clsprocess.InvstkId = HfUpdatestock.Value
            clsprocess.InvStkNo = Hfstkno.Value
            clsprocess.InvstkName = txtStockNameUpdate.Text
            clsprocess.InvCost = txtcost.Text
            clsprocess.InvWasteQty = ttlwaste
            'closing yesterday = 0
            'If ddlclosing.SelectedValue = 1 Then
            '    clsprocess.InvDate = Date.Now.Date.ToString("yyyy-MM-dd 00:00:00")
            'Else

            clsprocess.InvDate = Date.Now.Date.ToString("yyyy-MM-dd 00:00:00") 'Date.Now.AddDays(-1).Date.ToString("yyyy-MM-dd 00:00:00")

            'End If            'cInv.executeNonQuery_Inv_ProcessCls(Date.Now.Date.ToString("yyyy-MM-dd"), ddlFrom.SelectedValue, ddlFrom.SelectedItem.Text, stkid, stknumber, stkname, custid, 0, stktotal)
            ' clsprocess.insertclosingwaste()
            clsprocess.executeNonQuery_Inv_ProcessCls()
            'If Convert.ToDateTime(txtDate.Text) < Now.Date Then
            clsprocess.executeNonQuery_Inv_Processstock(Date.Now.Date, Date.Now.Date, custid, WasteInfo.stkid, HfUpdatecc.Value, True)

            'End If
            'clsprocess.InvDate = CDate(ds.Tables(0).Rows(0).Item("InvDate").ToString).AddDays(1)
            'clsprocess.varianceqty = 0
            'clsprocess.executeNonQuery_Inv_ProcessCls()
            Response.Redirect("~/Pages_ListWaste.aspx", False)

        Catch ex As Exception
            'MsgBox(ex.Message)
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
End Class