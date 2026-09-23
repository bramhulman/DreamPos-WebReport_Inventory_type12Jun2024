Imports NPOI.SS.Formula.Functions

Public Class Pages_CustomerPO
    Inherits System.Web.UI.Page
    Private mfunction As New ModFunction
    Private clsOutlet As New ClsOutlet
    Private CSup As New Cls_Supplier
    Private cpo As New Cls_PO
    Public Function modpriv() As Integer
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        If cookie Is Nothing Then

            Response.Redirect("~/default.aspx", False)
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


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'If Not (User.Identity.IsAuthenticated) Or custid() = -1 Then
        '    FormsAuthentication.RedirectToLoginPage()
        '    Exit Sub
        'End If
        'mfunction.ceksession()
        'If Session("username") Is Nothing Or custid() < 1 Then
        '    Session.Clear()
        '    Session.Abandon()
        '    Session.RemoveAll()
        '    FormsAuthentication.RedirectToLoginPage()
        '    Exit Sub
        'End If
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        If Not cookie Is Nothing Then

            If Not Me.IsPostBack Then


                Me.binding()
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


    End Sub
    Sub binding()

        Try
            If custid() = 0 Then
                Response.Redirect("~/Default.aspx", False)
            Else

                'If modpriv And usrModulepriv.moduleuser = usrModulepriv Then

                'End If

                lstViewDetails.DataSource = Nothing
                lstViewDetails.Items.Clear()
                Dim ds As New DataSet
                ds = cpo.SelectDs_custPO(custid)
                If ds IsNot Nothing AndAlso ds.Tables.Count > 0 Then
                    lstViewDetails.DataSource = ds
                Else
                    lstViewDetails.DataSource = Nothing
                End If
                lstViewDetails.DataBind()
                If mHelper.ModulePriv And usrModulepriv.moduleuser = usrModulepriv.moduleuser Then
                    'btnExportExcel.InnerText = "Export Excel"
                    btnnew.Visible = False
                Else
                    'btnExportExcel.InnerText = "Export / Import Excel"
                End If
            End If
        Catch ex As Exception
            errLog(Err.Description, "Pages_CustomerPO:binding")
            Err.Clear()
        End Try

    End Sub
    Protected Function Getdisplay()
        Dim imgstat As String = ""

        Try
            If modpriv() And usrModulepriv.moduleuser = usrModulepriv.moduleuser Then
                imgstat = "nodisplay"
            Else
                If (username() = My.Settings.admin Or username() = "support") Then
                    imgstat = "displayaction"
                Else
                    imgstat = "nodisplay"
                End If


            End If

        Catch ex As Exception
            errLog(Err.Description, "Pages_CustomerPO:Getdisplay")
            Err.Clear()
        End Try


        Return imgstat
    End Function
    Sub delete(ByVal id As String)
        FailureText.Text = ""

        Dim iHasil As Integer = 0
        Try
            DeleteCustPO(id)

            binding()
        Catch ex As Exception
            errLog(Err.Description, "Pages_CustomerPO:delete")
            Err.Clear()
        End Try
        clsInv = Nothing
    End Sub
    Protected Sub lstView_ItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ListViewCommandEventArgs) Handles lstViewDetails.ItemCommand
        '<%--autocomplete="off"--%>
        Try
            Select Case (e.CommandName)
                Case "Del"

                    Dim confirmValue As String = Request.Form("confirm_value")
                    If confirmValue = "Yes" Then

                        Dim id As Integer = Convert.ToInt32(e.CommandArgument)

                        delete(id)

                    Else
                        Exit Sub
                    End If

                Case "Edt"
                    Dim modfunc As New ModFunction
                    Dim id As Integer = Convert.ToInt32(e.CommandArgument)

                    modfunc.CreateCookie("InvCustomer_ID", id)

                    Response.Redirect("~/CustomerPO_Edit.aspx", False)



            End Select
        Catch ex As Exception
            errLog(Err.Description, "Pages_CustomerPO:lstView_ItemCommand")
            Err.Clear()
        End Try
    End Sub

    Protected Sub btnNew_Click(ByVal sender As Object, ByVal e As EventArgs) 'Handles btnnew.Click
        Try
            Dim modfunc As New ModFunction
            modfunc.CreateCookie("InvCustomer_ID", 0)
            Response.Redirect("~/CustomerPO_Edit.aspx", False)

            Exit Sub

        Catch ex As Exception
            errLog(Err.Description, "Pages_CustomerPO:btnNew_Click")
            Err.Clear()
        End Try
    End Sub

    'Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.Click

    '    Dim ds As New DataSet
    '    ds = clsOutlet.SelectDataset_OutletSearch(txtsearch.Text, custid, username, modulepriv)
    '    lstViewDetails.DataSource = ds
    '    lstViewDetails.DataBind()
    'End Sub
    Protected Sub btnsearch(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim ds As New DataSet

            ds = cpo.SelectDs_custPO(custid, txtsearch.Text)

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 Then
                lstViewDetails.DataSource = ds
            Else
                lstViewDetails.DataSource = Nothing
            End If
            lstViewDetails.DataBind()
        Catch ex As Exception
            errLog(Err.Description, "Pages_CustomerPO:btnsearch")
            Err.Clear()
        End Try
    End Sub

    Protected Sub OnPagePropertiesChanging(ByVal sender As Object, ByVal e As PagePropertiesChangingEventArgs)
        lvDataPager1.SetPageProperties(e.StartRowIndex, e.MaximumRows, False)

        binding()
    End Sub
    Protected Sub btnExportExcel_Click(sender As Object, e As EventArgs) 'Handles btnExportExcel.Click
        Try
            'CreateCookie("ID", 0)

            Response.Redirect("~/Pages_ExportInv_Supplier.aspx", False)
        Catch ex As Exception
            errLog(Err.Description, "Pages_CustomerPO:btnExportExcel_Click")
            Err.Clear()
        End Try
    End Sub
    Public Sub DeleteCustPO(ByVal id As Integer)
        'Dim pc As String = pic.ToString
        Try

            Dim str As String
            Dim c_cmd As New Cls_command

            str = "DELETE FROM cld_inv_ordercust  where id = " & id & ""

            c_cmd.executeNonQuery(str)

            'Revisi 10 Mei
            insertLogModule(username, "cld_inv_ordercust", "Delete", custid)

        Catch ex As Exception

            errLog(Err.Description, "Pages_CustomerPO:DeleteCustPO")
            Err.Clear()
        End Try
    End Sub
End Class
