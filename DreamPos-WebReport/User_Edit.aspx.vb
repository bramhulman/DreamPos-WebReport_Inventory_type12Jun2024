Imports System.Threading
Imports System.Net
Imports System.Text
Imports System.IO
Imports DreamPos_WebReport.UserList


Public Class User_Edit
    Inherits System.Web.UI.Page
    Private sNameuser As String

    Dim usr As String
    Dim shapsw As String
    'Dim itemgroupid As Integer
    Private mfunction As New ModFunction

    Private cuser As New clsUser


    Public Function susername() As String
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        Return cookie("username")

    End Function
    Public Function custid() As Integer
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")

        If cookie Is Nothing Then
            Return -1
        Else
            Return cookie("Custid")
        End If

    End Function
    Public Function iPriv() As Integer
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        Return cookie("Priv")

    End Function
    <Serializable>
    Public Class Flag_option 'Class untuk ListStore
        Public Property id As Integer = 0
        Public Property Name As String = ""
        Public Property Status As Boolean = False
    End Class
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        ceksession()
        usr = Request.Cookies("usr_edt").Value
        shapsw = Request.Cookies("psw_edt").Value

        Try
            ceksession()
            If Request.Cookies("UserLogin_inv") IsNot Nothing Then
                sNameuser = susername()
                Priv = iPriv()
                If IsPostBack = False Then
                    'Set dataItem untuk listview option
                    Dim dt As New DataTable()
                    dt.Columns.Add("id", GetType(Integer))
                    dt.Columns.Add("Name", GetType(String))
                    dt.Columns.Add("Status", GetType(Boolean))
                    dt.Rows.Add(1, "Item_Master", False)
                    dt.Rows.Add(2, "Stock_Group", False)
                    dt.Rows.Add(3, "Stock_Master", False)
                    dt.Rows.Add(4, "Cost_Centre", False)
                    dt.Rows.Add(5, "Supplier", False)
                    dt.Rows.Add(6, "Receipe", False)
                    dt.Rows.Add(7, "Order_to Warehouse", False)
                    dt.Rows.Add(8, "Order_from_Store", False)
                    dt.Rows.Add(9, "Batch_Process", False)
                    dt.Rows.Add(10, "Transfer", False)
                    dt.Rows.Add(11, "Raw_Waste", False)
                    dt.Rows.Add(12, "Order_to_Supplier", False)
                    dt.Rows.Add(13, "Return_to_Supplier", False)
                    dt.Rows.Add(14, "Stock_Opname", False)
                    dt.Rows.Add(15, "Calculate_Inventory", False)
                    dt.Rows.Add(16, "Expense_Other", False)
                    dt.Rows.Add(17, "Invoice", False)
                    dt.Rows.Add(18, "Return_to_Warehouse", False)
                    dt.Rows.Add(19, "Webreport", False)
                    dt.Rows.Add(20, "Invoice_Payment", False)
                    dt.Rows.Add(21, "Kartu_Stock", False)
                    dt.Rows.Add(22, "CC_Supplier", False)
                    dt.Rows.Add(23, "Print_order_to_WH", False)
                    dt.Rows.Add(24, "Acc_order_to_buy", False)
                    dt.Rows.Add(25, "Pre_Order_Usage", False)
                    dt.Rows.Add(26, "Purchase_Request", False)
                    ViewState("ListOptions") = DataTableToList(Of Flag_option)(dt)

                    lvwOption.DataSource = dt
                    lvwOption.DataBind()
                    txtpsw.Attributes("type") = "password"

                    If usr = "" Or shapsw = "" Then


                        hapus()
                        btnUpdate.Visible = True
                        btnUpdate.Text = "INSERT"
                    Else

                        editlist()

                    End If
                    'check_Batch()
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


        Catch ex As Exception
            errLog(Err.Description, "StoreGrpEdit:Page_Load")
            Err.Clear()
        End Try
    End Sub

    'Protected Sub Page_unLoad(sender As Object, e As EventArgs) Handles Me.Unload
    'End Sub

    Public Function username() As String
        Dim cookie As HttpCookie = Request.Cookies("UserLogin")
        Return cookie("userName")

    End Function

    Sub hapus()
        txtpsw.Text = ""
        txtusr.Text = ""

    End Sub

    Sub editlist()
        Try
            Dim ds As New DataSet
            Dim dt As New DataTable
            'Dim cuser As New clsUser
            Dim iFlagoption As Integer = 0
            Dim iRow As Integer = 0


            ds = cuser.getuser_edit(usr, shapsw)
            If ds.Tables(0).Rows.Count = 0 Then
                btnUpdate.Visible = False
                Exit Sub
            Else
                txtusr.Text = ds.Tables(0).Rows(0).Item("usr").ToString
                ViewState("oldusr") = ds.Tables(0).Rows(0).Item("usr").ToString
                ViewState("shapsw") = ds.Tables(0).Rows(0).Item("psw").ToString
                txtpsw.Text = "password"
                ddlmodulepriv.SelectedValue = ds.Tables(0).Rows(0).Item("modulepriv").ToString
                'dt = cmajorgroup.ds_majorgroup_byig(ds.Tables(0).Rows(0).Item("ItemGroupID")).Tables(0)
                ddlrptPriv.SelectedValue = ds.Tables(0).Rows(0).Item("rptpriv").ToString
                txtemail.Text = ds.Tables(0).Rows(0).Item("Email").ToString
                ddlModul.SelectedValue = ds.Tables(0).Rows(0).Item("Modul").ToString
                ddlRptf.SelectedValue = ds.Tables(0).Rows(0).Item("rptf").ToString

                btnUpdate.Visible = True
                btnUpdate.Text = "UPDATE"
                'btnInsert.Visible = False
                'tambahan 1 feb 2018 erik
                iFlagoption = ds.Tables(0).Rows(0).Item("invpriv")

                For Each item As ListViewItem In lvwOption.Items
                    Dim ck As CheckBox = DirectCast(item.FindControl("cbxOptionDetail"), CheckBox)

                    ck.Checked = check_usrInvPriv(iRow, iFlagoption)

                    iRow += 1
                Next
            End If

        Catch ex As Exception
            errLog(Err.Description, "StoreGrpEdit:editlist")
            Err.Clear()
        End Try
    End Sub

    Protected Sub btnInsert_Click(ByVal sender As Object, ByVal e As EventArgs) ' Handles btnInsert.Click
        insert()
    End Sub

    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As EventArgs) 'Handles btnUpdate.Click
        insert()
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) 'Handles btnCancel.Click

        Try
            hapus()


            Response.Redirect("~/Userlist.aspx", False)
        Catch ex As Exception
            errLog(Err.Description, "UserEdit:btnCancel_Click")
            Err.Clear()
        End Try

    End Sub




    Sub insert()

        Dim hslOption As Integer
        ' Dim mod_option As New mod_Enum
        Dim irow As Integer = 0
        Dim xshapsw As String = ViewState("shapsw")
        Dim xoldusr As String = ViewState("oldusr")
        For Each item As ListViewItem In lvwOption.Items
            Dim ck As CheckBox = DirectCast(item.FindControl("cbxOptionDetail"), CheckBox)

            If ck.Checked = True Then
                hslOption = Save_Rvc_usrInvPriv(irow, hslOption)
            End If

            irow += 1
        Next
        Try

            cuser.insertUser(txtusr.Text, txtpsw.Text, custid, ddlmodulepriv.SelectedValue, ddlrptPriv.SelectedValue, txtemail.Text, ddlModul.SelectedValue, ddlRptf.SelectedValue, hslOption, xoldusr, xshapsw)


            messagesuccesfull("User saved successfully", True)
            Response.Redirect("~/Userlist.aspx", False)

            Exit Sub

        Catch ex As Exception
            errLog(Err.Description, "StoreGrpEdit:insert")
            Err.Clear()
        End Try

    End Sub
    Sub storegroupExist()
        Dim message As String = "Area already exist!"
        Dim sb As New System.Text.StringBuilder()

        sb.Append("<script type = 'text/javascript'>")
        sb.Append("window.onload=function(){")
        sb.Append("alert('")
        sb.Append(message)
        sb.Append("')};")
        sb.Append("</script>")
        ClientScript.RegisterClientScriptBlock(Me.GetType(), "Alert", sb.ToString())
    End Sub


    Sub cbxOption_Clicked(sender As Object, e As EventArgs)
        Dim ckheader As CheckBox = DirectCast(sender, CheckBox)

        If ckheader.Checked = True Then
            For Each item As ListViewItem In lvwOption.Items
                Dim ckitem As CheckBox = DirectCast(item.FindControl("cbxOptionDetail"), CheckBox)
                ckitem.Checked = True
            Next
        Else
            For Each item As ListViewItem In lvwOption.Items
                Dim ckitem As CheckBox = DirectCast(item.FindControl("cbxOptionDetail"), CheckBox)
                ckitem.Checked = False
            Next
        End If

    End Sub

    Sub messagesuccesfull(ByVal msg As String, Optional ByVal bsuccess As Boolean = False)

        Dim message As String = msg '"Import From Excel Successfully."
        Dim url As String = ""

        If bsuccess Then
            url = "~/ItemMasterList.aspx"
            'Else
            '    url = "/Pages_ExportInv_Stk.aspx"
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
        Dim url As String = "~/ItemMasterList.aspx"
        sb.Append("<script type = 'text/javascript'>")
        sb.Append("window.onload=function(){")
        sb.Append("alert('")
        sb.Append(message)
        sb.Append("')};")
        sb.Append("</script>")
        ClientScript.RegisterClientScriptBlock(Me.GetType(), "Alert", sb.ToString())
    End Sub

    Sub fillplease()
        Dim message As String = "Fill Please File Name"
        Dim sb As New System.Text.StringBuilder()
        Dim url As String = "~/ItemMasterList.aspx"
        sb.Append("<script type = 'text/javascript'>")
        sb.Append("window.onload=function(){")
        sb.Append("alert('")
        sb.Append(message)
        sb.Append("')};")
        sb.Append("</script>")
        ClientScript.RegisterClientScriptBlock(Me.GetType(), "Alert", sb.ToString())
    End Sub

    Protected Sub ddlmodulepriv_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlmodulepriv.SelectedIndexChanged

    End Sub

    Protected Sub ddlModul_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlModul.SelectedIndexChanged

    End Sub

    Protected Sub ddlRptf_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlRptf.SelectedIndexChanged

    End Sub

    Protected Sub ddlrptPriv_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlrptPriv.SelectedIndexChanged

    End Sub
End Class