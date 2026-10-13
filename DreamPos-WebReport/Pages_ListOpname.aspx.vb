Imports System.IO
Imports System.Net
Imports ClosedXML.Excel
Imports DreamPos_WebReport.UserList
Imports Microsoft.VisualBasic.ApplicationServices
Imports NPOI.SS.UserModel
Imports NPOI.XSSF.UserModel
Public Class Pages_ListOpname
    Inherits System.Web.UI.Page
    Private sNameuser As String
    Private icustid As Integer
    Private imodul As Integer
    Private iModPriv As Integer
    Private irptpriv As Integer
    Private irptf As Integer
    Private mfunction As New ModFunction
    Private cInv As New clsInventory
    Private clsInventory As New clsInventory_stkOpname
    Dim invpriv As Integer
    'Private OpnameInfos As New List(Of OpnameListInfo)()
    Private OpnameInfo As New OpnameListInfo()
    '<Serializable>
    'Public Class OpnameListInfo
    '    Public Property invdate As Date
    '    Public Property inv_date As String
    '    Public Property custid As Integer
    '    Public Property stkid As Integer
    '    Public Property stkno As String
    '    Public Property stkname As String
    '    Public Property ccid As Integer
    '    Public Property ccname As String
    '    Public Property opencost As Decimal
    '    'Public Property openqtyu1 As Decimal
    '    'Public Property openqtyu2 As Decimal
    '    Public Property openqty As Decimal
    '    Public Property cost As Decimal
    '    Public Property ttlqty As Decimal
    '    Public Property bclose As Integer
    '    Public Property closeqty As Decimal
    '    Public Property closecost As Decimal
    '    Public Property varianceqty As Decimal
    '    Public Property factor1 As Decimal
    '    Public Property unit1 As String
    '    Public Property unit2 As String
    '    Public Property createddate As Date
    '    Public Property trtype As Integer
    '    Public Property type As String
    'End Class
    Private Property OpnameInfos As List(Of OpnameListInfo)
        Get
            Dim list = TryCast(ViewState("OpnameInfos"), List(Of OpnameListInfo))
            If list Is Nothing Then list = New List(Of OpnameListInfo)()
            Return list
        End Get
        Set(value As List(Of OpnameListInfo))
            ViewState("OpnameInfos") = value
        End Set
    End Property
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

                lblTitle.Text = "List Opname " & Date.Now.Date.ToString("dd MMM yyyy") & ""
                'txtDate.Text = Format(Now.AddDays(-1), "dd MMM yyyy")
                binding()
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
        'If (username() = My.Settings.admin Or username() = "support") Then
        '    btnnew.Visible = True
        'End If

    End Sub
    Sub binding()

        Try
            'If custid() = 0 Then
            '    Response.Redirect("~/Default.aspx")
            'Else
            '    Dim ds, ds2 As New DataSet
            '    'ds = cInv.getInvStkOpname(custid, modulepriv, username, ddlOutlet.SelectedValue)
            '    ds = cInv.getInvStkOpnameSearch2(txtsearch.Text, custid, modulepriv, username, ddlOutlet.SelectedValue, CDate(txtDate.Text))
            '    lstViewDetails.DataSource = ds
            '    lstViewDetails.DataBind()
            '    ds2 = cInv.getInvStkOpnameid(custid)
            '    If ds2.Tables(0).Rows.Count = 0 Then
            '        btnnew.Visible = False
            '    End If
            'End If
            Dim dtopname As New DataTable
            dtopname = cInv.getInvStkOpnameSearch2("", modulepriv, username).Tables(0)
            If dtopname.Rows.Count > 0 Then 'And dtopname.Rows.Count < 100 Then
                OpnameInfos = ToList(Of OpnameListInfo)(dtopname)
                ViewState("OpnameInfos") = OpnameInfos 'dtopname
            Else
                ViewState("OpnameInfos") = OpnameInfos 'dtopname
            End If
            gvOpname.DataSource = OpnameInfos
            gvOpname.DataBind()
            Dim upCrudGridPanel As UpdatePanel = CType(FindControl("upCrudGrid"), UpdatePanel)
            If upCrudGridPanel IsNot Nothing Then
                upCrudGridPanel.Update()
            End If
            dtopname.Rows.Clear()
            dtopname.Dispose()
        Catch ex As Exception

        End Try

    End Sub
    Sub delete(ByVal id As String, ByVal ccid As Integer)
        Try
            DeleteInvStkOpname(id, ccid, custid, username)
            binding()
        Catch ex As Exception

        End Try
    End Sub
    Public Sub DeleteInvStkOpname(ByVal id As String, ByVal ccid As Integer, ByVal Custid As Integer, ByVal username As String)
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
            errLog(Err.Description, "webservice:Deleteinv_transstock")
            Err.Clear()
        End Try
    End Sub






    Protected Sub gvOpname_PreRender(sender As Object, e As EventArgs) 'Handles gvOpname.PreRender
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

        End Try
    End Sub
    Protected Sub gvOpname_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs)




        If e.CommandName.Equals("editRecord") Then

            Dim rowindex As Integer = Convert.ToInt32(e.CommandArgument)
            Dim invdate As Date = CDate(gvOpname.DataKeys(rowindex).Values("invdate"))
            Dim ccid As Integer = CInt(gvOpname.DataKeys(rowindex).Values("ccid"))
            Dim stkid As Integer = CInt(gvOpname.DataKeys(rowindex).Values("stkid"))
            If OpnameInfos.Count = 0 Then

                OpnameInfos = DataTableToList(Of OpnameListInfo)(cInv.getInvStkOpnameSearch2("", modulepriv, username).Tables(0))
            End If
            Dim OpnameInfo = OpnameInfos.FirstOrDefault(Function(i) i.stkid = stkid And i.ccid = ccid And i.invdate = invdate)
            If OpnameInfo Is Nothing Then Exit Sub

            ' Simpan untuk btnUpdate_Click
            ViewState("CurrentOpname") = OpnameInfo

            HfUpdatecc.Value = OpnameInfo.ccid
            Hffactor1.Value = OpnameInfo.factor1
            Hfstkno.Value = OpnameInfo.stkno
            HfUpdatestock.Value = OpnameInfo.stkid
            HfUpdatedate.Value = OpnameInfo.invdate
            txtcostcentre.Text = OpnameInfo.ccname
            txtcost.Text = OpnameInfo.cost
            txtclosecost.Text = If(OpnameInfo.bclose <> 0, OpnameInfo.closecost, OpnameInfo.opencost)
            txtttlqty.Text = OpnameInfo.ttlqty
            lblttlu2.Text = OpnameInfo.unit2
            txtcloseqtyu1.Text = 0 'OpnameInfo.openqtyu1
            lblunit1.Text = OpnameInfo.unit1
            txtcloseqtyu2.Text = If(OpnameInfo.bclose <> 0, OpnameInfo.closeqty, OpnameInfo.openqty)
            lblunit2.Text = OpnameInfo.unit2
            txtStockNameUpdate.Text = OpnameInfo.stkname
            'upEdit.Update()
            Dim upEditPanel As UpdatePanel = Nothing
            If Me.Master IsNot Nothing Then
                Dim cp As ContentPlaceHolder = CType(Me.Master.FindControl("BodyContent"), ContentPlaceHolder)
                If cp IsNot Nothing Then
                    upEditPanel = CType(cp.FindControl("upEdit"), UpdatePanel)
                End If
            End If
            If upEditPanel Is Nothing Then
                upEditPanel = CType(Me.FindControl("upEdit"), UpdatePanel)
            End If
            
            If upEditPanel IsNot Nothing Then
                upEditPanel.Update()
            End If
            Dim script As String = "$('#editModal').modal('show');" &
                                   "$('#editModal').on('shown.bs.modal', function () { " &
                                   "   setTimeout(function() { " &
                                   "       var el = document.getElementById('" & txtcloseqtyu2.ClientID & "');" &
                                   "       if(el) { el.focus(); el.select(); }" &
                                   "   }, 100);" &
                                   "   $(this).off('shown.bs.modal');" &
                                   "});"
            Dim ctrl As Control = If(upEditPanel IsNot Nothing, CType(upEditPanel, Control), Me)
            ScriptManager.RegisterStartupScript(ctrl, ctrl.GetType(), "showModal", script, True)
            'Dim sb As System.Text.StringBuilder = New System.Text.StringBuilder()
            'sb.Append("<script type='text/javascript'>")
            'sb.Append("$('#editModal').modal('show');")
            'sb.Append("</script>")
            'ScriptManager.RegisterStartupScript(upEdit, upEdit.GetType(), "showModal", sb.ToString(), True)


        End If

        'If e.CommandName.Equals("deleteRecord") Then

        '    HfDeleteusr.Value = UInfo.Usr.ToString()
        '    HfDeletepsw.Value = UInfo.Usr.ToString()
        '    'txtNameUpdate.Text = MGInfo.Name
        '    Dim sb As System.Text.StringBuilder = New System.Text.StringBuilder()
        '    sb.Append("<script type='text/javascript'>")
        '    sb.Append("$('#deleteModal').modal('show');")
        '    sb.Append("</script>")
        '    ScriptManager.RegisterClientScriptBlock(Me, Me.[GetType](), "DeleteModalScript", sb.ToString(), False)
        'End If
    End Sub
    'Protected Sub txtDate_TextChanged(sender As Object, e As EventArgs) Handles txtDate.TextChanged
    '    binding()
    'End Sub
    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As EventArgs) 'Handles btnUpdate.Click
        Dim info As OpnameListInfo = CType(ViewState("CurrentOpname"), OpnameListInfo)
        If info Is Nothing Then Exit Sub
        Dim msg As String = ""
        Dim newLine As String = "\n"

        Try
            If info.bclose <> 0 AndAlso IsNumeric(txtcloseqtyu1.Text) AndAlso
               IsNumeric(txtcloseqtyu2.Text) AndAlso IsNumeric(txtclosecost.Text) AndAlso
               CDec(txtcloseqtyu1.Text) = 0 AndAlso CDec(txtcloseqtyu2.Text) = 0 AndAlso CDec(txtclosecost.Text) = 0 Then
                If info.custid <> custid() Then Throw New InvalidOperationException("Akses opname tidak diizinkan.")
                cInv.CancelStockOpname(info.stkid, info.ccid, info.custid, info.invdate, info.trtype)
                insertLogModule(username(), "InvStockOpname", "Cancel", info.custid)
                Response.Redirect("~/Pages_ListOpname.aspx", False)
                Context.ApplicationInstance.CompleteRequest()
                Return
            End If

            If Val(txtclosecost.Text) <= 0 And cInv.cekstocklink(custid, HfUpdatestock.Value) = 1 Then
                msg = "Cost cannot 0" & newLine
                'Exit Sub
            End If

            If IsNumeric(txtclosecost.Text) = False Or IsNumeric(txtcloseqtyu2.Text) = False Then
                msg = msg + "Unit must be numeric" & newLine
                'Exit Sub
            Else
                If txtcloseqtyu2.Text < 0 And txtcloseqtyu1.Text < 0 Then
                    msg = msg + "qty < 0" & newLine
                End If
            End If


            If cInv.get_bclose(HfUpdatestock.Value, HfUpdatecc.Value, Date.Now.AddDays(-1).Date.ToString("yyyy-MM-dd 00:00:00"), custid) = True Then
                Dim adtype As Integer = 0
                adtype = cInv.get_typestockopname(HfUpdatestock.Value, HfUpdatecc.Value, Date.Now.AddDays(-1).Date.ToString("yyyy-MM-dd 00:00:00"), custid)
                If adtype > 0 Then

                    adjustnow = Right(adtype.ToString, 1)
                Else
                    'msg = msg + "this stock has been adjusted" & newLine
                End If
            End If
            If msg = "" Then

                cInv.insertStockOpname(HfUpdatecc.Value, HfUpdatestock.Value, txtStockNameUpdate.Text, 0,
                                txtcloseqtyu1.Text, Hffactor1.Value, txtcloseqtyu2.Text, txtclosecost.Text, lblunit1.Text, lblunit2.Text, username, username, Hfstkno.Value, HfUpdatecc.Value, username, custid, 2)
                'gvOpname.DataBind()
                binding()
                upCrudGrid.Update()
                Dim upEditPanel As UpdatePanel = CType(FindControl("upEdit"), UpdatePanel)
                If upEditPanel IsNot Nothing Then
                    upEditPanel.Update()
                End If
                'upEdit.Update()
                ' ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hide", "$('#editModal').modal('hide');", True)
                Response.Redirect("~/Pages_ListOpname.aspx", False)
            Else

                Dim script As String = "window.onload = function(){ alert('"
                script += msg
                script += "');"
                script += "window.location = '"
                script += "Pages_ListOpname.aspx"
                script += " '; }"
                ClientScript.RegisterStartupScript(Me.GetType(), "Redirect", script, True)

                'notsuccesfull(msg)
                Exit Sub
            End If

            'MsgBox("Data Berhasil Di input Mysql")

            'Response.Redirect("~/Pages_ListOpname.aspx", False)

        Catch ex As Exception
            errLog(ex.Message, "Pages_ListOpname:btnUpdate_Click")
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "opnameUpdateError",
                "alert('Perubahan opname gagal. Silakan periksa log error.');", True)
        End Try
    End Sub

End Class
