Imports System.Threading
Imports System.Net
Imports System.Text
Imports System.IO
Imports System.Data
Imports System.Drawing

Public Class Pages_InvReturnEdit
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
    Private CCf As Integer
    Private Supid As Integer
    Private Dos As String
    Private rinvdate As Date
    Private CRtn As New Cls_IReturn
    Private CIcc As New Cls_ICc
    Private CSup As New Cls_Supplier
    Private CStk As New Cls_IStk
    Private CProcess As New clsInventoryProcess
    Private CItrans As New Cls_ITrans

    Dim cInv As New clsInventory
    Private cSupp As New clsInventory_Supplier

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
        Mtid = Request.Cookies("ID").Value
        CCf = Request.Cookies("Cc").Value
        Supid = Request.Cookies("Supid").Value

        Dos = Request.Cookies("DO").Value
        If Request.Cookies("wInvDate").Value IsNot Nothing Then
            If IsDate(Request.Cookies("wInvDate").Value) Then
                rinvdate = Request.Cookies("wInvDate").Value
            End If
        End If

        Try

            If Request.Cookies("UserLogin_inv") IsNot Nothing Then
                sNameuser = username()
                icustid = custid()
                imodul = modul()
                iModPriv = modulepriv()
                irptpriv = rptpriv()
                irptf = rptf()
                If IsPostBack = False Then

                    loadFrom()
                    loadSup()
                    'loadTo()
                    If Mtid = "" Then
                        ddlFrom.Text = ""
                        DdlSup.Text = ""
                        txtDO.Text = ""
                        txtinvdate.Text = Format(Now, "dd MMM yyyy")
                        Dim dt, dt2 As New DataTable
                        dt = CRtn.SelectDataTable_invReturn(Mtid, custid)
                        ViewState("invRtn") = dt
                        BindData()

                        bindingstk1()

                    Else
                        txtDO.Text = Dos
                        ddlFrom.Enabled = False
                        DdlSup.Enabled = False
                        txtDO.ReadOnly = True
                        If IsDate(rinvdate) Then
                            txtinvdate.Text = Format(rinvdate, "dd MMM yyyy")
                        Else
                            txtinvdate.Text = Format(Now, "dd MMM yyyy")
                        End If
                        txtinvdate.Enabled = False
                        'txtinvdate.Attributes.Add("style", "width: 800px;")
                        ddlFrom.SelectedValue = CCf
                        DdlSup.SelectedValue = Supid
                        txtLinkid.Text = Mtid
                        Dim dt, dt2 As New DataTable
                        dt = CRtn.SelectDataTable_invReturn(Mtid, custid)
                        ViewState("invRtn") = dt
                        BindData()

                        bindingstk()

                    End If
                End If
                ScriptManager.GetCurrent(Me).RegisterPostBackControl(RadButton1)
                'This for Search Button
                txtSearch.Attributes.Add("onkeyup", Me.Page.ClientScript.GetPostBackEventReference(txtSearch, ""))

                'ScriptManager.GetCurrent(Me).RegisterPostBackControl(RadButton1)
                txtSearch.Attributes.Add("onfocus", "javascript: this.setSelectionRange(this.value.length,this.value.length);")

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
            errLog(Err.Description, "Pages_InvReturnEdit:Page_Load")
            Err.Clear()
        End Try
    End Sub
    Sub loadFrom()

        Dim ds As New DataSet
        Try
            ds = CIcc.SelectDataset_Icc(custid, username, modulepriv, 0)
            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 Then
                ddlFrom.DataSource = ds
            Else
                ddlFrom.DataSource = Nothing
            End If
            ddlFrom.DataValueField = "Id"
            ddlFrom.DataTextField = "Name"
            ddlFrom.DataBind()
        Catch ex As Exception
            errLog(Err.Description, "Pages_InvReturnEdit:loadFrom")
            Err.Clear()
        End Try

    End Sub

    Sub loadSup(ByVal Optional xccid As Integer = 0)

        Dim ds As New DataSet
        Try
            If xccid = 0 Then
                ds = cInv.getSupplierbyUser(custid, username, modulepriv)
            Else
                DdlSup.Items.Clear()
                ds = cSupp.SelectDataset_SupplierCCview(custid, ddlFrom.SelectedValue, "")
            End If


            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 Then
                DdlSup.DataSource = ds
            Else
                DdlSup.DataSource = Nothing
            End If
            DdlSup.DataValueField = "Id"
            DdlSup.DataTextField = "Name"
            DdlSup.DataBind()

            If DdlSup.Items.Count > 0 Then
                DdlSup.SelectedIndex = 0
            End If
        Catch ex As Exception
            errLog(Err.Description, "Pages_InvReturnEdit:loadSup")
            Err.Clear()
        End Try

    End Sub
    Protected Sub Page_unLoad(sender As Object, e As EventArgs) Handles Me.Unload
    End Sub

    Public Function username() As String
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        Return cookie("userName")

    End Function
    Protected Sub BindData()
        Try
            If custid() = 0 Then
                Response.Redirect("~/Default.aspx", False)
            Else

                DgvTransfer.DataSource = DirectCast(ViewState("invRtn"), DataTable)
                DgvTransfer.DataBind()

                'If DgvTransfer.Rows.Count > 0 Then
                '    DgvTransfer.Columns(10).Visible = True
                '    'btnedit.Visible = True
                '    If GetBackground(Mtid) = 0 Then
                '        DgvTransfer.Columns(10).Visible = False
                '        'btnedit.Visible = False

                '    End If
                'Else
                '    DgvTransfer.Columns(10).Visible = False
                '    'btnedit.Visible = False
                'End If
            End If

        Catch ex As Exception
            errLog(Err.Description, "Pages_InvReturnEdit:BindData")
            Err.Clear()
        End Try

    End Sub
    Sub bindingstk1()

        Try
            If custid() = 0 Then
                Response.Redirect("~/Default.aspx", False)
            Else
                Dim dt, dt2 As New DataTable
                dt = DirectCast(ViewState("invRtn"), DataTable)
                If ddlFrom.SelectedValue > 0 Then
                    dt2 = CStk.SelectDataTable_Stkcc(dt, custid, txtSearch.Text, modulepriv, ddlFrom.SelectedValue)
                Else
                    dt2 = CStk.SelectDataTable_Stk(dt, custid, txtSearch.Text, modulepriv)
                End If

                ViewState("InvStk") = dt2
                LsvStk.DataSource = DirectCast(ViewState("InvStk"), DataTable)
                LsvStk.DataBind()
            End If
        Catch ex As Exception
            errLog(Err.Description, "Pages_InvReturnEdit:bindingstk1")
            Err.Clear()
        End Try


    End Sub
    Sub bindingstkcc()

        Try
            If custid() = 0 Then
                Response.Redirect("~/Default.aspx", False)
            Else
                Dim dt, dt2 As New DataTable
                dt = DirectCast(ViewState("invRtn"), DataTable)
                dt2 = CStk.SelectDataTable_Stkcc(dt, custid, txtSearch.Text, modulepriv, ddlFrom.SelectedValue)
                ViewState("InvStk") = dt2
                LsvStk.DataSource = DirectCast(ViewState("InvStk"), DataTable)
                LsvStk.DataBind()
            End If
        Catch ex As Exception
            errLog(Err.Description, "Pages_InvReturnEdit:bindingstkcc")
            Err.Clear()
        End Try


    End Sub
    Sub bindingstk()

        Try
            If custid() = 0 Then
                Response.Redirect("~/Default.aspx", False)
            Else
                Dim dt, dt2 As New DataTable
                dt = DirectCast(ViewState("invRtn"), DataTable)

                Debug.Print(dt.Rows.Count.ToString)
                'dt2 = clsStk.SelectDataTable_Stk_wStock(dt, custid, txtSearch.Text, ddlFrom.SelectedValue)
                'ViewState("InvStk") = dt2
                'LsvStk.DataSource = DirectCast(ViewState("InvStk"), DataTable)
                'LsvStk.DataBind()
                If ViewState("InvStk") IsNot Nothing Then
                    dt2 = DirectCast(ViewState("InvStk"), DataTable)
                End If


                If dt2.Rows.Count = 0 Then
                    'dt2 = CStk.SelectDataTable_Stk(dt, custid, txtSearch.Text, modulepriv)
                    dt2 = CStk.SelectDataTable_Stkcc(dt, custid, txtSearch.Text, modulepriv, ddlFrom.SelectedValue)
                    ViewState("InvStk") = dt2
                Else



                    dt2.DefaultView.RowFilter = "name like '%" & txtSearch.Text & "%'"
                    ViewState("InvStk") = dt2
                End If

                If dt2.Rows.Count > 0 Then
                    LsvStk.Items.Clear()
                    countlvstore = dt2.Rows.Count
                    LsvStk.DataSource = dt2.DefaultView
                    LsvStk.DataBind()
                    UpdatePanel3.Update()

                End If

            End If
        Catch ex As Exception
            errLog(Err.Description, "Pages_InvReturnEdit:bindingstk")
            Err.Clear()
        End Try


    End Sub
    <Serializable>
    Public Class Item_Detail 'Class untuk ListItem
        Public Property ItemName As String = ""
        Public Property ItemID As String = ""
        Public Property Status As Boolean = False
    End Class
    Sub Add()

        Try
            BindCheckItem()
            Dim i As Integer = 0
            
            ' Iterate through the persisted LIST, not the visible ListView Items
            Dim ListStore As List(Of Item_Detail) = TryCast(ViewState("ListItem"), List(Of Item_Detail))
            Dim ItemsToAdd As New List(Of Item_Detail)

            If ListStore IsNot Nothing Then
                For Each a In ListStore
                    If a.Status = True Then
                        ItemsToAdd.Add(a)
                    End If
                Next
            End If

            For Each itemDetail In ItemsToAdd
                
                Try
                    Dim cost As Integer
                    'Dim dsA As DataSet = CProcess.getdscost((DirectCast(item.FindControl("lblID"), Label).Text), ddlFrom.SelectedValue, custid)
                    Dim dsA As DataSet = CStk.SelectDataStkCost(itemDetail.ItemID, ddlFrom.SelectedValue, custid)
                    If dsA.Tables(0).Rows.Count > 0 Then
                        cost = dsA.Tables(0).Rows(0).Item("totalcost")
                    Else
                        cost = 0
                    End If
                    Dim ds As DataSet = CStk.SelectDataStk(itemDetail.ItemID, custid)
                    If ds.Tables(0).Rows.Count > 0 Then
                        Dim u1 As String = ds.Tables(0).Rows(0).Item("Unit1").ToString
                        Dim u2 As String = ds.Tables(0).Rows(0).Item("Unit2").ToString
                        Dim dt As DataTable = DirectCast(ViewState("invRtn"), DataTable)
                        Dim dt2 As DataTable = CStk.SelectData_Inv_Stk(itemDetail.ItemID, ddlFrom.SelectedValue, custid).Tables(0)
                        Dim ttlqty As Double = dt2.Rows(0).Item("stock").ToString
                        dt.Rows.Add(0, custid, Date.Now.ToString("yyyy-MM-dd"), 1, 0, itemDetail.ItemID, itemDetail.ItemName, 0, 0, ds.Tables(0).Rows(0).Item("Factor1"), 0, 0, cost, "", 1, ds.Tables(0).Rows(0).Item("Unit1").ToString, ds.Tables(0).Rows(0).Item("Unit2").ToString, username, Date.Now.ToString("yyyy-MM-dd"), "", Date.Now.ToString("yyyy-MM-dd"), ds.Tables(0).Rows(0).Item("Number").ToString, "", 0, ttlqty)
                        ViewState("invRtn") = dt
                        
                        ' We do NOT manually delete from ViewState("InvStk") anymore because index can be wrong.
                        ' We will force a re-bind at the end.
                    End If

                Catch error_t As Exception
                    MsgBox(error_t.ToString)
                End Try

            Next
            
            ' Clear the selection state after adding
            ViewState("ListItem") = Nothing

            BindData()
            txtSearch.Text = ""
            
            ' Force re-fetch from DB to ensure added items are removed from the list correctly
            ViewState("InvStk") = Nothing 
            bindingstk()

        Catch ex As Exception
            errLog(Err.Description, "Pages_InvReturnEdit:Add")
            Err.Clear()
            'MsgBox(ex.Message)
        End Try

    End Sub
    Protected Sub RadButton1_ServerClick(sender As Object, e As EventArgs) Handles RadButton1.ServerClick
        Try
            If ddlFrom.SelectedValue <> 0 And DdlSup.SelectedValue <> 0 Then
                Add()
                Session("xsup") = DdlSup.SelectedValue
                Session("xfrom") = ddlFrom.SelectedValue
            Else
                notsuccesfull("you must choose Cost Centre and Supplier first")
            End If
        Catch ex As Exception
            errLog(Err.Description, "Pages_InvReturnEdit:RadButton1_ServerClick")
            Err.Clear()
        End Try
    End Sub
    Private Sub BindCheckItem()
        Try

            If CBool(IIf(IsNothing(ViewState("ListItem")), True, False)) Then


                Dim ListItem2 As List(Of Item_Detail) = New List(Of Item_Detail)()

                Dim dtreturn, dtitem As New DataTable
                dtreturn = DirectCast(ViewState("invRtn"), DataTable)
                ' Load ALL items initially to track state globally, not just searched items
                dtitem = CStk.SelectDataTable_Stk(dtreturn, custid, "", modulepriv)

                If dtitem.Rows.Count > 0 Then
                    For Each dt As DataRow In dtitem.Rows
                        ListItem2.Add(New Item_Detail With {
                                           .ItemName = dt.Item("name"),
                                           .ItemID = dt.Item("id").ToString(),
                                           .Status = False
                                           })
                    Next
                End If


                For Each item As ListViewItem In LsvStk.Items
                    Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox1"), CheckBox)
                    Dim Labels As Label = DirectCast(item.FindControl("lblName"), Label)
                    Dim LabelID As Label = DirectCast(item.FindControl("lblID"), Label)
                    Dim label2 As String = Labels.Text
                    Dim id2 As String = LabelID.Text



                    If ck.Checked Then
                        For Each a In ListItem2
                            If a.ItemID = id2 Then
                                a.Status = True
                            End If
                        Next
                        '  clsCheck.IDBrand.Add(label2)
                    End If
                Next
                ViewState("ListItem") = ListItem2

            Else

                Dim ListItem2 As List(Of Item_Detail) = TryCast(ViewState("ListItem"), List(Of Item_Detail))

                For Each item As ListViewItem In LsvStk.Items
                    Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox1"), CheckBox)
                    Dim Labels As Label = DirectCast(item.FindControl("lblName"), Label)
                    Dim LabelID As Label = DirectCast(item.FindControl("lblID"), Label)
                    Dim label2 As String = Labels.Text
                    Dim id2 As String = LabelID.Text

                    If ck.Checked Then
                        For Each a In ListItem2
                            If a.ItemID = id2 Then
                                a.Status = True
                            End If
                        Next

                    Else
                        For Each a In ListItem2
                            If a.ItemID = id2 Then
                                a.Status = False
                            End If
                        Next
                    End If
                Next
                ViewState("ListItem") = ListItem2
            End If
        Catch ex As Exception
            errLog(Err.Description, "Pages_InvReturnEdit:BindCheckItem")
            Err.Clear()
        End Try
    End Sub

    Private Sub CheckedItem()
        Try


            Dim ListStore As List(Of Item_Detail) = TryCast(ViewState("ListItem"), List(Of Item_Detail))

            Dim i As Integer = 0
            For Each item As ListViewItem In LsvStk.Items
                Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox1"), CheckBox)
                Dim Labels As Label = DirectCast(item.FindControl("lblName"), Label)
                Dim LabelID As Label = DirectCast(item.FindControl("lblID"), Label)
                Dim label2 As String = Labels.Text
                Dim id2 As String = LabelID.Text

                For Each a In ListStore
                    If a.ItemID = id2 Then
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
            errLog(Err.Description, "Pages_InvReturnEdit:CheckedItem")
            Err.Clear()
        End Try
    End Sub


    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        Threading.Thread.Sleep(500)
        BindCheckItem()
        bindingstk()
        CheckedItem()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Pop", "openModalItem();", True)
        txtSearch.Attributes("onfocus") = "javascript:this.setSelectionRange(this.value.length,this.value.length);"
        txtSearch.Focus()
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btncancel.ServerClick

        Try


            Response.Redirect("~/Pages_InvReturn.aspx", False)

        Catch ex As Exception
            errLog(Err.Description, "Pages_InvReturnEdit:btnCancel_Click")
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
        'Response.Write("<script>alert('" & message & "');</script>")
        ClientScript.RegisterClientScriptBlock(Me.GetType(), "Alert", sb.ToString())
    End Sub
    Public Sub DeleteInvReturn(ByVal id As String, ByVal Custid As Integer, ByVal username As String)
        'Dim pc As String = pic.ToString
        Try

            Dim str As String
            Dim strTr As String
            Dim c_cmd As New Cls_command

            '   strTr = "DELETE FROM cld_inv_trans  where linkid = '" & id & "' and custid = " & Custid & ""
            str = "DELETE FROM cld_invlk_Rcv  where id = '" & id & "' and custid = " & Custid & ""
            Dim ds As New DataSet
            Dim a As New clsDbo("Select * FROM cld_inv_trans  where linkid = '" & id & "' and custid = " & Custid & "")
            ds = a.selectDataset
            If ds.Tables(0).Rows.Count > 0 Then
                Dim clsprocess As New clsInventoryProcess
                For Each row As DataRow In ds.Tables(0).Rows

                    clsprocess.InvstkId = row.Item("Stk_Id")
                    clsprocess.InvCc = row.Item("cc")
                    clsprocess.InvCcName = row.Item("cc_name")
                    clsprocess.InvCustId = Custid
                    clsprocess.InvstkName = row.Item("stk_name")
                    clsprocess.rtnqty = -(row.Item("ttlStock"))
                    clsprocess.InvStkNo = row.Item("Stk_no")
                    clsprocess.InvDate = Date.Now.Date.ToString("yyyy-MM-dd")
                    clsprocess.BCost = True
                    clsprocess.executeNonQuery_Inv_ProcessCls()

                    strTr = "DELETE FROM cld_inv_trans  where id = " & row.Item("id") & ""
                    c_cmd.executeNonQuery(strTr)


                    'c_cmd.executeNonQuery_Inv_CalcCost(row.Item("stk_id"), row.Item("cc"), Custid)
                    'c_cmd.executeNonQuery_Inv_CalcStock(row.Item("stk_id"), row.Item("cc"), Custid)

                Next
            End If
            a.dispose()
            a = Nothing
            c_cmd.executeNonQuery(str)
            insertLogModule(username, "Inv_Return", "Delete", Custid)
        Catch ex As Exception
            errLog(Err.Description, "Pages_InvReturnEdit:DeleteInvReturn")
            Err.Clear()
        End Try
    End Sub
    Sub insert()
        Try
            Dim msg As String = ""
            Dim dt As DataTable = DirectCast(ViewState("invRtn"), DataTable)
            Dim newLine As String = "\n"
            If Mtid <> "" Then
                If CDate(txtinvdate.Text) < CDate(Date.Now) Then
                    DeleteInvReturn(Mtid, custid, username)
                Else
                    DeleteInvReturn(Mtid, custid, username)
                    txtLinkid.Text = ""
                    Mtid = ""
                End If

            End If

            If dt.Rows.Count = 0 Then
                msg = msg + "Item is Nothing" & newLine
            End If
            If ddlFrom.SelectedValue = 0 Or ddlFrom.SelectedItem.Text = "" Then
                msg = msg + "Cost Centre Must be Choice " & newLine
            End If

            If DdlSup.SelectedValue = 0 Or DdlSup.SelectedItem.Text = "" Then
                msg = msg + "Supplier Must be Choice" & newLine
            End If
            For Each row As DataRow In dt.Rows
                If row.Item("U1") = 0 And row.Item("U2") = 0 Then

                    msg = msg + row.Item("Stk_name") + " Qty Not Fill " & newLine

                End If

            Next row
            If msg = "" Then
                insertInvReturn(txtLinkid.Text, ddlFrom.SelectedValue, ddlFrom.SelectedItem.Text, dt, txtDO.Text, DdlSup.SelectedValue, 2, username, custid)

            Else
                notsuccesfull(msg)
                Exit Sub
            End If


            Response.Redirect("~/Pages_InvReturn.aspx", False)

        Catch ex As Exception

            'MsgBox(Err.Description, MsgBoxStyle.Information)
            errLog(Err.Description, "Pages_InvReturnEdit:Insert")
            Err.Clear()
        End Try

    End Sub

    Public Sub insertInvReturn(ByVal id As String, ByVal ccfrom As Integer, ByVal ccname As String, ByVal dt As DataTable, ByVal DOS As String, ByVal Supid As Integer, ByVal stat As Integer, ByVal username As String, ByVal custid As Integer)
        Dim c_cmd As New Cls_command
        Dim strheader As String
        Dim strmaster As String
        Dim doctot As Decimal = 0
        Try
            Dim idtrans As String = CItrans.idReturn(custid)
            If id = "" Then

                For Each row As DataRow In dt.Rows
                    strheader = "insert into cld_inv_Trans values (" & c_cmd.ExecuteScalar("select Max(ID) from cld_inv_Trans") & ", " & custid & ",'" & Date.Now.Date.ToString("yyyy-MM-dd") & "', 4, " & ccfrom & ", " &
                        " " & row.Item("stk_id") & ", '" & row.Item("Stk_Name") & "', " & row.Item("U1") & ", " & row.Item("U2") & ", " & CInt(row.Item("Factor1")) & ", " & row.Item("ttlStock") & ", " & row.Item("CostU1") & ", " & row.Item("Cost") & ", " &
                        " '" & idtrans & "', " & row.Item("flagtrans") & ", '" & row.Item("U1_nm") & "', '" & row.Item("U2_nm") & "', '" & row.Item("Createdby") & "', '" & Date.Now.ToString("yyyy-MM-dd HH:mm:ss") & "', Null, Null, '" & row.Item("stk_no") & "', '" & ccname & "')"
                    c_cmd.executeNonQuery(strheader)
                    doctot = row.Item("ttlCost") + doctot
                    'c_cmd.executeNonQuery_Inv_CalcCost(row.Item("stk_id"), ccfrom, custid)
                    'c_cmd.executeNonQuery_Inv_CalcStock(row.Item("stk_id"), ccfrom, custid)

                    'untuk langsung nambah stoclcls
                    Dim clsprocess As New clsInventoryProcess
                    clsprocess.InvstkId = row.Item("stk_id").ToString
                    clsprocess.InvCc = ccfrom
                    clsprocess.InvCcName = ccname
                    clsprocess.InvCustId = custid
                    clsprocess.InvstkName = row.Item("Stk_Name").ToString
                    clsprocess.rtnqty = row.Item("ttlStock")
                    clsprocess.InvStkNo = row.Item("stk_no")
                    clsprocess.InvCost = row.Item("Cost")
                    clsprocess.InvDate = Date.Now.Date.ToString("yyyy-MM-dd 00:00:00")
                    'clsprocess.BCost = True
                    clsprocess.executeNonQuery_Inv_ProcessCls()
                    'akhir untuk langsung nambah stoclcls

                Next

                strmaster = "Insert into cld_invlk_rcv values('" & idtrans & "'," & custid & ", '" & DOS & "', " & Supid & "," & stat & "," & doctot & ", 2 ," & ccfrom & ", '" & username & "', '" & Date.Now.Date.ToString("yyyy-MM-dd") & "', Null, Null, '" & Date.Now.Date.ToString("yyyy-MM-dd") & "',0, '" & Date.Now.Date.ToString("yyyy-MM-dd") & "', '', 0,0,1)"
                c_cmd.executeNonQuery(strmaster)

            Else
                idtrans = id
                For Each row As DataRow In dt.Rows
                    If row.Item("ID") > 0 Then

                        'untuk langsung delete stoclcls
                        Dim clsprocess As New clsInventoryProcess
                        clsprocess.InvstkId = row.Item("stk_id").ToString
                        clsprocess.InvCc = ccfrom
                        clsprocess.InvCcName = ccname
                        clsprocess.InvCustId = custid
                        clsprocess.InvstkName = row.Item("Stk_Name").ToString
                        clsprocess.rtnqty = -c_cmd.ExecuteScalar("SELECT ttlstock FROM cld_inv_trans WHERE linkid='" & id & "' and stk_id = '" & row.Item("stk_id").ToString & "' and cc = '" & row.Item("cc").ToString & "' and custid = " & custid & "", 0)
                        clsprocess.InvStkNo = row.Item("stk_no")
                        clsprocess.InvCost = row.Item("Cost")
                        ' foreach(DateTime day In EachDay(StartDate, EndDate))
                        If CDate(row.Item("invdate")) < CDate(Date.Now) Then
                            Dim ldate As Date
                            ldate = CDate(row.Item("invdate"))
                            While ldate <= CDate(Date.Now)
                                clsprocess.InvDate = CDate(row.Item("invdate")).ToString("yyyy-MM-dd 00:00:00")
                                'clsprocess.BCost = True
                                clsprocess.executeNonQuery_Inv_ProcessCls(True)
                                ldate = ldate.AddDays(1)
                            End While
                        Else
                            clsprocess.InvDate = Date.Now.Date.ToString("yyyy-MM-dd 00:00:00")
                            'clsprocess.BCost = True
                            clsprocess.executeNonQuery_Inv_ProcessCls()
                        End If



                        'akhir untuk langsung delete stoclcls
                        'kalau update yang kemaren
                        If CDate(row.Item("invdate")) < CDate(Date.Now) Then
                            strheader = "insert into cld_inv_Trans values (" & row.Item("ID") & ", " & custid & ",'" & CDate(row.Item("invdate")).ToString("yyyy-MM-dd 00:00:00") & "', 4, " & ccfrom & ", " &
                        " " & row.Item("stk_id") & ", '" & row.Item("Stk_Name") & "', " & row.Item("U1") & ", " & row.Item("U2") & ", " & CInt(row.Item("Factor1")) & ", " & row.Item("ttlStock") & ", " & row.Item("CostU1") & ", " & row.Item("Cost") & ", " &
                        " '" & idtrans & "', " & row.Item("flagtrans") & ", '" & row.Item("U1_nm") & "', '" & row.Item("U2_nm") & "', '" & row.Item("Createdby") & "', '" & Date.Now.ToString("yyyy-MM-dd HH:mm:ss") & "', '" & sNameuser & "', '" & Date.Now.ToString("yyyy-MM-dd HH:mm:ss") & "', '" & row.Item("stk_no") & "', '" & ccname & "')"

                        Else
                            strheader = "update cld_Inv_Trans set  U1 = " & row.Item("U1") & ", U2 = " & row.Item("U2") & ", factor1 = " & row.Item("Factor1") & ", ttlstock = " & row.Item("ttlStock") & "," & row.Item("CostU1") & ", Cost = " & row.Item("Cost") & ", " &
                          " ModifBy = '" & username & "', Modifdate = '" & Date.Now.ToString("yyyy-MM-dd HH:mm:ss") & "'  where id = " & row.Item("ID") & " and custid = " & custid & ""

                        End If
                        c_cmd.executeNonQuery(strheader)

                        doctot = row.Item("ttlCost") + doctot

                        'untuk langsung nambah stoclcls
                        'Dim clsprocess As New clsInventoryProcess
                        clsprocess.InvstkId = row.Item("stk_id").ToString
                        clsprocess.InvCc = ccfrom
                        clsprocess.InvCcName = ccname
                        clsprocess.InvCustId = custid
                        clsprocess.InvstkName = row.Item("Stk_Name").ToString
                        clsprocess.rtnqty = row.Item("ttlStock")
                        clsprocess.InvStkNo = row.Item("stk_no")
                        clsprocess.InvCost = row.Item("Cost")
                        clsprocess.InvDate = Date.Now.Date.ToString("yyyy-MM-dd 00:00:00")
                        'clsprocess.BCost = True
                        clsprocess.executeNonQuery_Inv_ProcessCls()
                        'akhir untuk langsung nambah stoclcls

                    Else
                        strheader = "insert into cld_inv_Trans values (" & c_cmd.ExecuteScalar("select Max(ID) from cld_inv_Trans") & ", " & custid & ",'" & Date.Now.Date.ToString("yyyy-MM-dd") & "', 4, " & ccfrom & ", " &
                            " " & row.Item("stk_id") & ", '" & row.Item("Stk_Name") & "', " & row.Item("U1") & ", " & row.Item("U2") & ", " & row.Item("Factor1") & ", " & row.Item("ttlStock") & ", " & row.Item("CostU1") & ", " & row.Item("Cost") & ", " &
                            " '" & id & "', " & row.Item("flagtrans") & ", '" & row.Item("U1_nm") & "', '" & row.Item("U2_nm") & "', '" & row.Item("Createdby") & "', '" & Date.Now.ToString("yyyy-MM-dd HH:mm:ss") & "' , Null, Null, '" & row.Item("stk_no") & "', '" & ccname & "')"
                        c_cmd.executeNonQuery(strheader)

                        doctot = row.Item("ttlCost") + doctot

                        'untuk langsung nambah stoclcls
                        Dim clsprocess As New clsInventoryProcess
                        clsprocess.InvstkId = row.Item("stk_id").ToString
                        clsprocess.InvCc = ccfrom
                        clsprocess.InvCcName = ccname
                        clsprocess.InvCustId = custid
                        clsprocess.InvstkName = row.Item("Stk_Name").ToString
                        clsprocess.rtnqty = row.Item("ttlStock")
                        clsprocess.InvStkNo = row.Item("stk_no")
                        clsprocess.InvCost = row.Item("Cost")
                        clsprocess.InvDate = Date.Now.Date.ToString("yyyy-MM-dd 00:00:00")
                        clsprocess.BCost = True
                        clsprocess.executeNonQuery_Inv_ProcessCls()
                        'akhir untuk langsung nambah stoclcls

                    End If
                    If CDate(row.Item("invdate")) < CDate(Date.Now) Then
                        strmaster = "Insert into cld_invlk_rcv values('" & idtrans & "'," & custid & ", '" & DOS & "', " & Supid & "," & stat & "," & doctot & ", 2 ," & ccfrom & ", '" & row.Item("Createdby") & "', '" & Date.Now.ToString("yyyy-MM-dd HH:mm:ss") & "', '" & username & "', '" & Date.Now.Date.ToString("yyyy-MM-dd HH:mm:ss") & "', '" & CDate(row.Item("invdate")).ToString("yyyy-MM-dd 00:00:00") & "',0, '" & CDate(row.Item("invdate")).ToString("yyyy-MM-dd 00:00:00") & "', '', 0,0,1)"

                    Else
                        strmaster = "update cld_invlk_rcv set  doctot = " & doctot & ", flag = " & stat & ", modifby =  '" & username & "', Modifdate =  '" & Date.Now.ToString("yyyy-MM-dd HH:mm:ss") & "' where id = '" & id & "' and custid = " & custid & ""

                    End If


                    c_cmd.executeNonQuery(strmaster)
                    'c_cmd.executeNonQuery_Inv_CalcCost(row.Item("stk_id"), ccfrom, custid)
                    'c_cmd.executeNonQuery_Inv_CalcStock(row.Item("stk_id"), ccfrom, custid)
                    'Str = ""
                Next

            End If


            insertLogModule(username, "InvtransRcv", "Update", custid)



        Catch ex As Exception

            errLog(Err.Description, "Pages_InvReturnEdit:insertInvReturn")
            Err.Clear()
        End Try
    End Sub


    Protected Sub btnSearch_ServerClick(sender As Object, e As EventArgs) Handles btnSearch.ServerClick
        bindingstk()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Pop", "openModalItem();", True)
    End Sub

    Protected Sub DgvTransfer_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles DgvTransfer.RowCancelingEdit
        DgvTransfer.EditIndex = -1
        BindData()
    End Sub

    Protected Sub DgvTransfer_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles DgvTransfer.RowEditing
        DgvTransfer.EditIndex = e.NewEditIndex
        BindData()
    End Sub

    Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.ServerClick
        insert()
    End Sub
    Protected Sub ddlFrom_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlFrom.SelectedIndexChanged
        Dim dt As New DataTable
        '= DirectCast(ViewState("invRtn"), DataTable)
        If Not DirectCast(ViewState("invRtn"), DataTable) Is Nothing Then
            dt = DirectCast(ViewState("invRtn"), DataTable)
        End If
        If Session("xfrom") Is Nothing Then
            Session("xfrom") = 0
        End If
        Try
            If dt.Rows.Count > 0 And ddlFrom.SelectedValue <> Session("xfrom").ToString Then
                notsuccesfull("you must delete item before change")
                ddlFrom.SelectedValue = Session("xfrom").ToString
            Else

                If dt.Rows.Count > 0 Then
                    For Each row As DataRow In dt.Rows
                        Dim cost As Integer
                        'Dim ds As DataSet = CStk.SelectDataStkCost(row.Item("stk_id"), ddlFrom.SelectedValue, custid)
                        Dim ds As DataSet = CProcess.getdscost(row.Item("stk_id"), ddlFrom.SelectedValue, custid, Date.Now.Date.ToString("yyyy-MM-dd HH:mm:ss"))
                        If ds.Tables(0).Rows.Count > 0 Then
                            cost = ds.Tables(0).Rows(0).Item("cost")
                        Else
                            cost = 0
                        End If
                        row.Item("Cost") = cost

                        row.Item("ttlCost") = cost * row.Item("ttlstock")
                        dt.AcceptChanges()

                    Next row
                    BindData()

                End If
                bindingstkcc()
                loadSup(ddlFrom.SelectedValue)
            End If
        Catch ex As Exception
            errLog(Err.Description, "Pages_InvReturnEdit:ddlFrom_SelectedIndexChanged")
            Err.Clear()
        End Try



    End Sub
    'Protected Sub RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)

    'End Sub
    Protected Sub DgvTransfer_RowDeleting(ByVal sender As Object, ByVal e As GridViewDeleteEventArgs)
        Dim lblID As Label = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("lblID"), Label)
        Dim lblstknm As Label = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("lblName"), Label)
        Dim lblstkid As Label = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("lblStkID"), Label)
        Dim dt As DataTable = DirectCast(ViewState("invRtn"), DataTable)
        Try
            If dt.Rows.Count <= 1 Then
                notsuccesfull("just 1 item, please delete on return list")
                Exit Sub
            End If
            Dim dtstk As DataTable = DirectCast(ViewState("InvStk"), DataTable)
            If lblID.Text = 0 Then
                dtstk.Rows.Add(lblstkid.Text, lblstknm.Text)
                ViewState("InvStk") = dtstk
                dt.Rows(e.RowIndex).Delete()
                dt.AcceptChanges()
            Else
                dtstk.Rows.Add(lblstkid.Text, lblstknm.Text)
                ViewState("InvStk") = dtstk
                Dim confirmValue As String = Request.Form("confirm_value")
                If confirmValue = "Yes" Then

                    Dim id As Integer = lblID.Text

                    delete(id)

                Else
                    Exit Sub
                End If
                dt.Rows(e.RowIndex).Delete()
                dt.AcceptChanges()
            End If
            BindData()
            bindingstk()
        Catch ex As Exception
            errLog(Err.Description, "Pages_InvReturnEdit:DgvTransfer_RowDeleting")
            Err.Clear()
        End Try

    End Sub
    Sub delete(ByVal id As Integer)
        DeleteInvTrans(id, custid, username)
    End Sub
    Public Sub DeleteInvTrans(ByVal id As Integer, ByVal Custid As Integer, ByVal username As String)
        'Dim pc As String = pic.ToString
        Try

            Dim str As String
            Dim c_cmd As New Cls_command


            Dim ds As New DataSet
            Dim a As New clsDbo("Select * FROM cld_inv_trans  where id = " & id & "")
            ds = a.selectDataset
            If ds.Tables(0).Rows.Count > 0 Then
                For Each row As DataRow In ds.Tables(0).Rows
                    str = "DELETE FROM cld_inv_trans  where id = " & id & ""
                    c_cmd.executeNonQuery(str)
                    'c_cmd.executeNonQuery_Inv_CalcCost(row.Item("stk_id"), 0, Custid)
                    'c_cmd.executeNonQuery_Inv_CalcStock(row.Item("stk_id"), 0, Custid)
                Next
            End If
            a.dispose()
            a = Nothing


            'Revisi 10 Mei
            insertLogModule(username, "InvTrans", "Delete", Custid)

        Catch ex As Exception

            errLog(Err.Description, "Pages_InvReturnEdit:DeleteInvTrans")
            Err.Clear()
        End Try
    End Sub
    Public Sub Showalert(message As String)
        Dim cleanMessage As String = message.Replace("'", "\'")
        Dim page As Page = HttpContext.Current.CurrentHandler
        Dim script As String = String.Format("alert('{0}');", cleanMessage)
        If (page IsNot Nothing And Not page.ClientScript.IsClientScriptBlockRegistered("alert")) Then
            page.ClientScript.RegisterClientScriptBlock(page.GetType(), "alert", script, True) ' /* addScriptTags */
        End If
    End Sub
    Protected Sub DgvTransfer_RowUpdating(ByVal sender As Object, ByVal e As GridViewUpdateEventArgs)
        '   notsuccesfull("return can't more than stock")
        'Dim lblrtn As Label = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("lblttlStock"), Label)
        Dim lblstk As Label = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("lblccStock"), Label)

        Dim txtUnit1 As TextBox = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("txtEditUnit1"), TextBox)
        Dim txtCost As Label = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("txtEditCost"), Label)
        Dim txtUnit2 As TextBox = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("txtEditUnit2"), TextBox)
        ' Dim txtEditU1Cost As TextBox = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("txtEditU1Cost"), TextBox)
        Dim lblLosF1 As Label = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("lblEditfactor"), Label)
        'Dim newLine As String = "\n"
        If txtUnit1.Text = "" Then
            txtUnit1.Text = 0
        End If

        If txtUnit2.Text = "" Then
            txtUnit2.Text = 0
        End If

        Dim total As Decimal = (Val(txtUnit1.Text) * Val(lblLosF1.Text)) + Val(txtUnit2.Text)


        Dim lblUnit1 As Label = DirectCast(DgvTransfer.Rows(e.RowIndex).FindControl("lblEditEmpID"), Label)
        Dim cinvstk As New clsInventory_stk

        Dim dt As DataTable = DirectCast(ViewState("invRtn"), DataTable)

        dt.Rows(e.RowIndex)("U1") = txtUnit1.Text
        dt.Rows(e.RowIndex)("U2") = txtUnit2.Text
        dt.Rows(e.RowIndex)("ttlStock") = total
        '   dt.Rows(e.RowIndex)("CostU1") = txtEditU1Cost.Text
        'Dim cost As Decimal = txtCost.Text  'txtEditU1Cost.Text / lblLosF1.Text
        Dim dt2 As New DataTable
        dt2 = cinvstk.SelectDataStkCost(dt.Rows(e.RowIndex).Item("stk_id"), ddlFrom.SelectedValue, custid, Date.Now.Date.ToString("yyyy-MM-dd")).Tables(0)


        dt.Rows(e.RowIndex)("Cost") = dt2.Rows(0).Item("cost").ToString '0 'cost
        Dim ttlcost As Decimal = total * dt2.Rows(0).Item("cost").ToString 'cost
        dt.Rows(e.RowIndex)("ttlCost") = ttlcost
        dt.AcceptChanges()

        DgvTransfer.EditIndex = -1
        BindData()
        If total > lblstk.Text Then
            Showalert("can't return more than stock")
            txtUnit1.Text = 0
            txtUnit2.Text = 0
            dt.Rows(e.RowIndex)("U1") = txtUnit1.Text
            dt.Rows(e.RowIndex)("U2") = txtUnit2.Text
            total = Val(txtUnit1.Text) * Val(lblLosF1.Text) + Val(txtUnit2.Text)
            dt.Rows(e.RowIndex)("ttlStock") = total
            dt.AcceptChanges()
            BindData()

            'notsuccesfull("can't return")
            'Response.Write("<script>alert('you');</script>")
            Exit Sub
        End If
    End Sub
    Protected Sub btnClearSearch_Click(sender As Object, e As EventArgs)
        For Each item As ListViewItem In LsvStk.Items
            Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox1"), CheckBox)

            ck.Checked = False


        Next
        txtSearch.Text = ""
        lblnoitem.Text = 0
        'BindData()
        Bindingstk1()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Pop", "openModalItem();", True)
    End Sub
    Private Sub ShowToastr(ByVal page As Page, ByVal message As String, ByVal title As String, Optional ByVal type As String = "info", Optional ByVal clearToast As Boolean = False, Optional ByVal pos As String = "toast-top-left", Optional ByVal Sticky As Boolean = False)
        Dim toastrScript As String = [String].Format("Notify('{0}','{1}','{2}', '{3}', '{4}', '{5}');", message, title, type, clearToast, pos, Sticky)
        page.ClientScript.RegisterStartupScript(page.[GetType](), "toastr_message", toastrScript, addScriptTags:=True)
    End Sub

    Protected Sub DdlSup_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DdlSup.SelectedIndexChanged
        Dim dt As DataTable = DirectCast(ViewState("invRtn"), DataTable)
        Try
            If dt.Rows.Count > 0 AndAlso Session("xsup") IsNot Nothing AndAlso DdlSup.SelectedValue <> Session("xsup").ToString Then
                notsuccesfull("you change supplier !")
                'ddlFrom.SelectedValue = Session("xfrom").ToString

            End If
        Catch ex As Exception
            Err.Clear()
        End Try
    End Sub
    Public Iterator Function EachDay(ByVal from As DateTime, ByVal thru As DateTime) As IEnumerable(Of DateTime)
        Dim day = from.Date

        While day.Date <= thru.Date
            Yield day
            day = day.AddDays(1)
        End While
    End Function
    Protected Function GetBackground(ByVal rtnid As String)

        Dim imgstat As Integer = 0

        Try
            imgstat = CRtn.rtn_date(rtnid)

        Catch ex As Exception
            errLog(Err.Description, "Pages_InvReturnEdit:GetBackground")
            Err.Clear()
        End Try


        Return imgstat

    End Function
End Class
