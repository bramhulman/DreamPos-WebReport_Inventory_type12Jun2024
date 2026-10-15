Imports System.Threading
Imports System.Net
Imports System.Text
Imports System.IO
Imports iTextSharp.tool.xml.html.table
Imports System.Web.Services.Description

Public Class Pages_ReceipeDetailAdd
    Inherits System.Web.UI.Page
    Private sNameuser As String
    Private Userid As Integer
    Private icustid As Integer
    Private imodul As Integer
    Private iModPriv As Integer
    Private irptpriv As Integer
    Private irptf As Integer
    Private mfunction As New ModFunction
    Private Mdd As String = ""
    Private MTid As Integer
    Private Crcpdtl As New Cls_IRcpDtl



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
            MTid = Request.Cookies("ID").Value
            If Request.Cookies("UserLogin_inv") IsNot Nothing Then
                sNameuser = username()
                icustid = custid()
                imodul = modul()
                iModPriv = modulepriv()
                irptpriv = rptpriv()
                irptf = rptf()
                If IsPostBack = False Then
                    ddlTipe.SelectedValue = 1
                    binding(MTid, 1)
                    hapus()
                    txtSearch.Attributes.Add("onkeyup", Me.Page.ClientScript.GetPostBackEventReference(txtSearch, ""))
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
            errLog(Err.Description, "Pages_ReceipeDetailAdd:Page_Load")
            Err.Clear()
        End Try
    End Sub
    Sub hapus()

        txtQty.Text = 0
        txtLosF.Text = 0


        btnUpdate.Visible = False
        btnInsert.Visible = True
    End Sub
    Sub binding(ByVal rcpid As Integer, ByVal tipe As Integer)

        Try
            If custid() = 0 Then
                Response.Redirect("~/Default.aspx", False)
            Else
                Dim ds As New DataSet
                ds = Crcpdtl.SelectDataRcpItemDtlAllSearch(icustid, MTid, ddlTipe.SelectedValue, txtSearch.Text) 'Crcpdtl.SelectDataRcpItemDtlAll(custid, rcpid, tipe)
                If ds IsNot Nothing AndAlso ds.Tables.Count > 0 Then
                    lstViewDetails.DataSource = ds
                Else
                    lstViewDetails.DataSource = Nothing
                End If
                lstViewDetails.DataBind()
            End If
        Catch ex As Exception
            errLog(Err.Description, "Pages_ReceipeDetailAdd:binding")
            Err.Clear()
        End Try


    End Sub
    Sub insert()

        Try
            Dim itemdtl As Integer
            Dim countitem As Integer = 0
            Dim rcp_name As String = ""
            Dim c_cmd As New Cls_command
            For Each item As ListViewItem In lstViewDetails.Items
                Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox1"), CheckBox)
                Dim lblname As Label = DirectCast(item.FindControl("Label8"), Label)

                If ck.Checked Then
                    Try
                        If txtQty.Text = 0 Or txtQty.Text = "" Then
                            notsuccesfull("qty must be fill")
                            Exit Sub

                        Else
                            itemdtl = (DirectCast(item.FindControl("lblID"), Label).Text)
                            rcp_name = c_cmd.ExecuteScalar("select name from cld_inv_rcp where id = " & MTid.ToString & " and custid = " & custid() & "", 0)
                            If ddlTipe.SelectedValue = 1 Then
                                insertRcpDtl(0, custid, MTid, ddlTipe.SelectedValue, itemdtl, txtQty.Text, txtLosF.Text, username)
                            ElseIf ddlTipe.SelectedValue = 2 Then
                                Dim ds As New DataSet
                                ds = Crcpdtl.SelectDataset_rcpdtl(custid, itemdtl)

                                insertLogModule(username, "Invrcpdtl, add " & itemdtl.ToString & " " & lblname.Text & " to " & MTid & " " & rcp_name, "Insert", custid)
                                insertextremark(MTid, custid, itemdtl)
                                If ds.Tables(0).Rows.Count > 0 Then
                                    For Each row As DataRow In ds.Tables(0).Rows
                                        insertRcpDtl(0, custid, MTid, 1, row.Item("kodeid"), row.Item("qty"), row.Item("lossf"), username)
                                    Next
                                End If
                            End If

                        End If



                    Catch error_t As Exception
                        MsgBox(error_t.ToString)
                    End Try

                    countitem = countitem + 1
                End If

            Next
            If countitem = 0 Then
                notsuccesfull("Please choose item")
                Exit Sub
            End If

            'MsgBox("Data Berhasil Di input Mysql")

            Response.Redirect("~/Pages_ReceipeEdit.aspx", False)

        Catch ex As Exception
            'MsgBox(ex.Message)
            errLog(Err.Description, "Pages_ReceipeDetailAdd:insert")
            Err.Clear()
        End Try

    End Sub
    Protected Sub Page_unLoad(sender As Object, e As EventArgs) Handles Me.Unload
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
    Public Function username() As String
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        Return cookie("userName")

    End Function

    Protected Sub OnPagePropertiesChanging(ByVal sender As Object, ByVal e As PagePropertiesChangingEventArgs)


        binding(MTid, 1)
    End Sub








    Sub fillplease()
        Dim message As String = "Please Choice Item Name"
        Dim sb As New System.Text.StringBuilder()
        Dim url As String = "~/Pages_ItemMaster.aspx"
        sb.Append("<script type = 'text/javascript'>")
        sb.Append("window.onload=function(){")
        sb.Append("alert('")
        sb.Append(message)
        sb.Append("')};")
        sb.Append("</script>")
        ClientScript.RegisterClientScriptBlock(Me.GetType(), "Alert", sb.ToString())
    End Sub
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.ServerClick
        'Dim cookie As HttpCookie = Request.Cookies("UserName")

        ' Dim Cust As Integer = cookie("CustID")
        Dim ds As New DataSet

        ds = Crcpdtl.SelectDataRcpItemDtlAllSearch(custid, MTid, ddlTipe.SelectedValue, txtSearch.Text)

        If ds IsNot Nothing AndAlso ds.Tables.Count > 0 Then
            lstViewDetails.DataSource = ds
        Else
            lstViewDetails.DataSource = Nothing
        End If
        lstViewDetails.DataBind()

        'If ddlTipe.SelectedValue = 2 Then
        '    thid.Visible = True
        '    'tdid.visible = True
        'Else
        '    thid.Visible = False
        '    'tdid.visible = False
        'End If
        For Each item As ListViewItem In lstViewDetails.Items
            If ddlTipe.SelectedValue = 2 Then
                item.FindControl("tdid").Visible = True

            Else
                item.FindControl("tdid").Visible = False
            End If

        Next

    End Sub
    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnUpdate.ServerClick
        insert()
    End Sub

    Protected Sub btnCancel_ServerClick(sender As Object, e As EventArgs) Handles btnCancel.ServerClick
        Try
            hapus()
            mfunction.CreateCookie("Idrcp", 0)
            mfunction.CreateCookie("ID", MTid)

            Response.Redirect("~/Pages_ReceipeEdit.aspx")



        Catch ex As Exception
            errLog(Err.Description, "Pages_ReceipeDetailAdd:btnCancel_Click")
            Err.Clear()
        End Try
    End Sub
    Public Sub insertRcpDtl(ByVal id As Integer, ByVal custid As Integer, ByVal rcpid As Integer, ByVal tipe As Integer, ByVal kodeid As Integer, ByVal qty As Decimal, ByVal Lossf As Decimal, ByVal username As String)
        Cls_IRcpDtl.SaveDetail(id, custid, rcpid, tipe, kodeid, qty, Lossf, "add")
        insertLogModule(username, "Invrcpdtl", "Update", custid)
    End Sub
    Protected Sub lstView_ItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ListViewCommandEventArgs) Handles lstViewDetails.ItemCommand

        Select Case (e.CommandName)
            Case "Del"
                'Dim id As Integer = Convert.ToInt32(e.CommandArgument)

                'delete(id)

                Dim confirmValue As String = Request.Form("confirm_value")
                If confirmValue = "Yes" Then

                    Dim id As Integer = Convert.ToInt32(e.CommandArgument)

                    delete(id)

                Else
                    Exit Sub
                End If


            Case "Edt"

                ID = Convert.ToInt32(e.CommandArgument)

                mfunction.CreateCookie("Idrcp", ID)
                mfunction.CreateCookie("ID", MTid)

                Response.Redirect("~/Pages_ReceipeDtlEdit.aspx")



        End Select
    End Sub
    Public Sub DeleteRcpDtl(ByVal id As Integer, ByVal Custid As Integer, ByVal username As String)
        Using db As New RecipeDb()
            db.SetParameter("id", id)
            db.SetParameter("custid", Custid)
            db.Execute("DELETE FROM cld_inv_rcpdtl WHERE id=@id AND custid=@custid")
            db.Commit()
        End Using
        insertLogModule(username, "InvRcpdtl", "Delete", Custid)
    End Sub
    Sub delete(ByVal id As Integer)

        DeleteRcpDtl(id, custid, username)
        binding(MTid, ddlTipe.SelectedValue)
    End Sub
    Protected Sub ddlTipe_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlTipe.SelectedIndexChanged
        Try
            If ddlTipe.SelectedValue > 0 Then

                binding(MTid, ddlTipe.SelectedValue)
                If ddlTipe.SelectedValue = 2 Then
                    txtQty.Text = 1
                    txtQty.Enabled = False
                Else
                    txtQty.Enabled = True
                End If
            Else

            End If
        Catch ex As Exception
            errLog(Err.Description, "Pages_ReceipeDetailAdd:ddlTipe_SelectedIndexChanged")
            Err.Clear()
        End Try
    End Sub

    Protected Sub btnInsert_ServerClick(sender As Object, e As EventArgs) Handles btnInsert.ServerClick
        insert()
    End Sub

    Public Sub insertextremark(ByVal id As Integer, ByVal custid As Integer, ByVal rcpid As Integer)
        Using db As New RecipeDb()
            db.SetParameter("id", id)
            db.SetParameter("custid", custid)
            db.SetParameter("rcpid", rcpid)
            db.Execute("UPDATE cld_inv_rcp SET remarks=CONCAT(remarks,';',@rcpid) WHERE id=@id AND custid=@custid")
            db.Commit()
        End Using
    End Sub

    Protected Sub ddlTipe_TextChanged(sender As Object, e As EventArgs) Handles ddlTipe.TextChanged
        Try
            If ddlTipe.SelectedValue = 2 Then
                thid.Visible = True
                'tdid.visible = True
            Else
                thid.Visible = False
                'tdid.visible = False
            End If
            For Each item As ListViewItem In lstViewDetails.Items
                If ddlTipe.SelectedValue = 2 Then
                    item.FindControl("tdid").Visible = True

                Else
                    item.FindControl("tdid").Visible = False
                End If

            Next
        Catch ex As Exception

        End Try
    End Sub

    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        Threading.Thread.Sleep(500)
        BindCheckStock()
        binding(MTid, 1)
        CheckedStock()

        For Each item As ListViewItem In lstViewDetails.Items
            If ddlTipe.SelectedValue = 2 Then
                item.FindControl("tdid").Visible = True

            Else
                item.FindControl("tdid").Visible = False
            End If

        Next

        txtSearch.Attributes("onfocus") = "javascript:this.setSelectionRange(this.value.length,this.value.length);"
        txtSearch.Focus()
    End Sub
    <Serializable>
    Public Class Stock_Detail 'Class untuk ListStock
        Public Property StockName As String = ""
        Public Property Status As Boolean = False
        'Public Property CcID As Integer
    End Class

    'Start : Stock
    Private Sub BindCheckStock()
        Try

            If CBool(IIf(IsNothing(ViewState("ListStock")), True, False)) Then


                Dim ListStock As List(Of Stock_Detail) = New List(Of Stock_Detail)()

                Dim dsStock As New DataSet

                dsStock = Crcpdtl.SelectDataRcpItemDtlAllSearch(icustid, MTid, ddlTipe.SelectedValue, txtSearch.Text) 'SelectDataset_Stock(icustid, "")

                If dsStock.Tables(0).Rows.Count > 0 Then
                    For Each dt As DataRow In dsStock.Tables(0).Rows
                        ListStock.Add(New Stock_Detail With {
                                           .StockName = dt.Item("Name"),
                                           .Status = False
                                           })
                    Next
                    '.CcID = dt.Item("ID"),
                End If


                For Each item As ListViewItem In lstViewDetails.Items
                    Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox1"), CheckBox)
                    Dim Labels As Label = DirectCast(item.FindControl("Label8"), Label)
                    Dim label2 As String = Labels.Text



                    If ck.Checked Then
                        For Each a In ListStock
                            If a.StockName = label2 Then
                                a.Status = True
                            End If
                        Next
                        '  clsCheck.IDBrand.Add(label2)
                    End If
                Next
                ViewState("ListStock") = ListStock

            Else

                Dim ListStock As List(Of Stock_Detail) = TryCast(ViewState("ListStock"), List(Of Stock_Detail))

                For Each item As ListViewItem In lstViewDetails.Items
                    Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox1"), CheckBox)
                    Dim Labels As Label = DirectCast(item.FindControl("Label8"), Label)
                    Dim label2 As String = Labels.Text

                    If ck.Checked Then
                        For Each a In ListStock
                            If a.StockName = label2 Then
                                a.Status = True
                            End If
                        Next

                    Else
                        For Each a In ListStock
                            If a.StockName = label2 Then
                                a.Status = False
                            End If
                        Next
                    End If
                Next
                ViewState("ListStock") = ListStock
            End If
        Catch ex As Exception
            errLog(Err.Description, "kstock:BindCheckStock")
            Err.Clear()
        End Try
    End Sub

    Private Sub CheckedStock()
        Try


            Dim ListStock As List(Of Stock_Detail) = TryCast(ViewState("ListStock"), List(Of Stock_Detail))

            Dim i As Integer = 0
            For Each item As ListViewItem In lstViewDetails.Items
                Dim ck As CheckBox = DirectCast(item.FindControl("CheckBox1"), CheckBox)
                Dim Labels As Label = DirectCast(item.FindControl("Label8"), Label)
                Dim label2 As String = Labels.Text

                For Each a In ListStock
                    If a.StockName = label2 Then
                        If a.Status = True Then
                            ck.Checked = True
                        Else
                            ck.Checked = False
                        End If

                    End If
                Next

            Next
            ViewState("ListStock") = ListStock
        Catch ex As Exception
            errLog(Err.Description, "kstock:CheckStock")
            Err.Clear()
        End Try
    End Sub
    'END : Stock
    'Protected Function GetBackground()

    '    Dim imgstat As String
    '    If ddlTipe.SelectedValue = 2 Then
    '        imgstat = "displayaction"
    '    Else
    '        imgstat = "nodisplay"

    '    End If

    '    Return imgstat
    'End Function


    'Protected Sub lstViewDetails_DataBound(sender As Object, e As EventArgs) Handles lstViewDetails.DataBound

    '    For Each item As ListViewItem In lstViewDetails.Items
    '        Dim lblname As Label = DirectCast(item.FindControl("lblID"), Label)
    '        If ddlTipe.SelectedValue = 2 Then
    '            lblname.Visible = True
    '        Else
    '            lblname.Visible = False
    '        End If

    '    Next

    'End Sub
End Class
