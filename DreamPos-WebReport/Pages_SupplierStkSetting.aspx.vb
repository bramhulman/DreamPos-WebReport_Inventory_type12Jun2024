Imports System.Data
Imports System.Web.Security

Public Class Pages_SupplierStkSetting
    Inherits System.Web.UI.Page

    Private mfunction As New ModFunction

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

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("username") Is Nothing Or custid() < 1 Then
            Session.Clear()
            Session.Abandon()
            Session.RemoveAll()
            FormsAuthentication.RedirectToLoginPage()
            Exit Sub
        End If

        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        If Not cookie Is Nothing Then
            If Not Me.IsPostBack Then
                lblMessage.Text = ""
                LoadDropdowns()
                BindOverviewGrid()

                ' Load active mappings for initially selected items
                If ddlSupplierSearch.Items.Count > 0 Then
                    LoadStockItemsForSupplier(Convert.ToInt32(ddlSupplierSearch.SelectedValue))
                End If

                If ddlStockSearch.Items.Count > 0 Then
                    LoadSuppliersForStock(Convert.ToInt32(ddlStockSearch.SelectedValue))
                End If
            End If
        Else
            Dim script As String = "window.onload = function(){ alert('Please Login'); window.location = '../Default'; }"
            ClientScript.RegisterStartupScript(Me.GetType(), "Redirect", script, True)
        End If
    End Sub

    Private Sub LoadDropdowns()
        Try
            ' 1. Load Suppliers
            Dim dsSup As DataSet = GetDataSet("SELECT Id, Name FROM cld_Inv_Supplier WHERE custid = " & custid() & " ORDER BY Name")
            ddlSupplierSearch.DataSource = dsSup.Tables(0)
            ddlSupplierSearch.DataTextField = "Name"
            ddlSupplierSearch.DataValueField = "Id"
            ddlSupplierSearch.DataBind()

            ' 2. Load Stock Items
            Dim dsStk As DataSet = GetDataSet("SELECT Id, CONCAT(Number, ' - ', Name) AS Name FROM cld_inv_stk WHERE custid = " & custid() & " ORDER BY Name")
            ddlStockSearch.DataSource = dsStk.Tables(0)
            ddlStockSearch.DataTextField = "Name"
            ddlStockSearch.DataValueField = "Id"
            ddlStockSearch.DataBind()
        Catch ex As Exception
            ShowMessage("Error loading dropdowns: " & ex.Message, True)
        End Try
    End Sub

    Private Sub BindOverviewGrid(Optional ByVal supplierId As Integer = 0, Optional ByVal stkId As Integer = 0)
        Try
            Dim sql As String = "SELECT x.supplier_id, s.Name AS supplier_name, x.stk_id, st.Number AS stk_number, st.Name AS stk_name " & _
                               "FROM cld_inv_stkxsupplier x " & _
                               "INNER JOIN cld_Inv_Supplier s ON x.supplier_id = s.Id " & _
                               "INNER JOIN cld_inv_stk st ON x.stk_id = st.Id " & _
                               "WHERE x.custid = " & custid() & " "

            If supplierId > 0 Then
                sql &= " AND x.supplier_id = " & supplierId & " "
            End If
            If stkId > 0 Then
                sql &= " AND x.stk_id = " & stkId & " "
            End If

            sql &= "ORDER BY s.Name, st.Name"
            Dim ds As DataSet = GetDataSet(sql)
            rptMappings.DataSource = ds.Tables(0)
            rptMappings.DataBind()
            upGrid.Update()
        Catch ex As Exception
            ShowMessage("Error loading active mappings: " & ex.Message, True)
        End Try
    End Sub

    ' --- TAB 1: BY SUPPLIER ---
    Protected Sub ddlSupplierSearch_SelectedIndexChanged(sender As Object, e As EventArgs)
        If ddlSupplierSearch.SelectedIndex >= 0 Then
            Dim supplierId As Integer = Convert.ToInt32(ddlSupplierSearch.SelectedValue)
            LoadStockItemsForSupplier(supplierId)
            BindOverviewGrid(supplierId, 0)
        Else
            BindOverviewGrid()
        End If
    End Sub

    Private Sub LoadStockItemsForSupplier(ByVal supplierId As Integer)
        Try
            ' Get all stock items
            Dim dsAllStk As DataSet = GetDataSet("SELECT Id, CONCAT(Number, ' - ', Name) AS Name FROM cld_inv_stk WHERE custid = " & custid() & " ORDER BY Name")
            cblStockItems.Items.Clear()

            For Each row As DataRow In dsAllStk.Tables(0).Rows
                Dim li As New ListItem(row("Name").ToString(), row("Id").ToString())
                cblStockItems.Items.Add(li)
            Next

            ' Get mapped items for this supplier
            Dim dsMapped As DataSet = GetDataSet("SELECT stk_id FROM cld_inv_stkxsupplier WHERE supplier_id = " & supplierId & " AND custid = " & custid())
            For Each row As DataRow In dsMapped.Tables(0).Rows
                Dim stkId As String = row("stk_id").ToString()
                Dim li As ListItem = cblStockItems.Items.FindByValue(stkId)
                If li IsNot Nothing Then
                    li.Selected = True
                End If
            Next
            upBySupplier.Update()
        Catch ex As Exception
            ShowMessage("Error loading stock items: " & ex.Message, True)
        End Try
    End Sub

    Protected Sub btnSaveBySupplier_Click(sender As Object, e As EventArgs)
        If ddlSupplierSearch.SelectedIndex < 0 Then
            ShowMessage("Please select a supplier first.", True)
            Exit Sub
        End If

        Dim supplierId As Integer = Convert.ToInt32(ddlSupplierSearch.SelectedValue)
        Try
            ' 1. Delete all existing mappings for this supplier
            ExecuteNonQuery("DELETE FROM cld_inv_stkxsupplier WHERE supplier_id = " & supplierId & " AND custid = " & custid())

            ' 2. Insert new checked mappings
            Dim insertCount As Integer = 0
            For Each li As ListItem In cblStockItems.Items
                If li.Selected Then
                    Dim stkId As Integer = Convert.ToInt32(li.Value)
                    ExecuteNonQuery("INSERT INTO cld_inv_stkxsupplier (stk_id, supplier_id, custid) VALUES (" & stkId & ", " & supplierId & ", " & custid() & ")")
                    insertCount += 1
                End If
            Next

            ' Log insertion
            insertLogModule(username(), "SupplierStkSetting", "SaveBySupplier: " & ddlSupplierSearch.SelectedItem.Text & " (" & insertCount & " items)", custid())

            ShowMessage("Successfully saved " & insertCount & " mappings for supplier '" & ddlSupplierSearch.SelectedItem.Text & "'!", False)
            BindOverviewGrid()
            
            ' Refresh the other Tab's loaded selection as well to keep UI in perfect sync
            If ddlStockSearch.SelectedIndex >= 0 Then
                LoadSuppliersForStock(Convert.ToInt32(ddlStockSearch.SelectedValue))
            End If
        Catch ex As Exception
            ShowMessage("Error saving mappings: " & ex.Message, True)
        End Try
    End Sub

    ' --- TAB 2: BY STOCK ITEM ---
    Protected Sub ddlStockSearch_SelectedIndexChanged(sender As Object, e As EventArgs)
        If ddlStockSearch.SelectedIndex >= 0 Then
            Dim stkId As Integer = Convert.ToInt32(ddlStockSearch.SelectedValue)
            LoadSuppliersForStock(stkId)
            BindOverviewGrid(0, stkId)
        Else
            BindOverviewGrid()
        End If
    End Sub

    Private Sub LoadSuppliersForStock(ByVal stkId As Integer)
        Try
            ' Get all suppliers
            Dim dsAllSup As DataSet = GetDataSet("SELECT Id, Name FROM cld_Inv_Supplier WHERE custid = " & custid() & " ORDER BY Name")
            cblSuppliers.Items.Clear()

            For Each row As DataRow In dsAllSup.Tables(0).Rows
                Dim li As New ListItem(row("Name").ToString(), row("Id").ToString())
                cblSuppliers.Items.Add(li)
            Next

            ' Get mapped suppliers for this stock item
            Dim dsMapped As DataSet = GetDataSet("SELECT supplier_id FROM cld_inv_stkxsupplier WHERE stk_id = " & stkId & " AND custid = " & custid())
            For Each row As DataRow In dsMapped.Tables(0).Rows
                Dim supplierId As String = row("supplier_id").ToString()
                Dim li As ListItem = cblSuppliers.Items.FindByValue(supplierId)
                If li IsNot Nothing Then
                    li.Selected = True
                End If
            Next
            upByStock.Update()
        Catch ex As Exception
            ShowMessage("Error loading suppliers: " & ex.Message, True)
        End Try
    End Sub

    Protected Sub btnSaveByStock_Click(sender As Object, e As EventArgs)
        If ddlStockSearch.SelectedIndex < 0 Then
            ShowMessage("Please select a stock item first.", True)
            Exit Sub
        End If

        Dim stkId As Integer = Convert.ToInt32(ddlStockSearch.SelectedValue)
        Try
            ' 1. Delete all existing mappings for this stock item
            ExecuteNonQuery("DELETE FROM cld_inv_stkxsupplier WHERE stk_id = " & stkId & " AND custid = " & custid())

            ' 2. Insert new checked mappings
            Dim insertCount As Integer = 0
            For Each li As ListItem In cblSuppliers.Items
                If li.Selected Then
                    Dim supplierId As Integer = Convert.ToInt32(li.Value)
                    ExecuteNonQuery("INSERT INTO cld_inv_stkxsupplier (stk_id, supplier_id, custid) VALUES (" & stkId & ", " & supplierId & ", " & custid() & ")")
                    insertCount += 1
                End If
            Next

            ' Log insertion
            insertLogModule(username(), "SupplierStkSetting", "SaveByStock: " & ddlStockSearch.SelectedItem.Text & " (" & insertCount & " suppliers)", custid())

            ShowMessage("Successfully saved " & insertCount & " supplier mappings for stock item '" & ddlStockSearch.SelectedItem.Text & "'!", False)
            BindOverviewGrid()

            ' Refresh the other Tab's loaded selection as well to keep UI in perfect sync
            If ddlSupplierSearch.SelectedIndex >= 0 Then
                LoadStockItemsForSupplier(Convert.ToInt32(ddlSupplierSearch.SelectedValue))
            End If
        Catch ex As Exception
            ShowMessage("Error saving mappings: " & ex.Message, True)
        End Try
    End Sub

    ' --- DELETE ACTION FROM OVERVIEW GRID ---
    Protected Sub rptMappings_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles rptMappings.ItemCommand
        If e.CommandName = "DeleteMapping" Then
            Try
                Dim args As String() = e.CommandArgument.ToString().Split(";"c)
                Dim supplierId As Integer = Convert.ToInt32(args(0))
                Dim stkId As Integer = Convert.ToInt32(args(1))

                ' Execute deletion
                ExecuteNonQuery("DELETE FROM cld_inv_stkxsupplier WHERE supplier_id = " & supplierId & " AND stk_id = " & stkId & " AND custid = " & custid())

                ' Log deletion
                insertLogModule(username(), "SupplierStkSetting", "DeleteMapping supplier: " & supplierId & ", item: " & stkId, custid())

                ShowMessage("Relationship deleted successfully!", False)

                ' Refresh grid and current selections
                BindOverviewGrid()

                If ddlSupplierSearch.SelectedIndex >= 0 Then
                    LoadStockItemsForSupplier(Convert.ToInt32(ddlSupplierSearch.SelectedValue))
                End If

                If ddlStockSearch.SelectedIndex >= 0 Then
                    LoadSuppliersForStock(Convert.ToInt32(ddlStockSearch.SelectedValue))
                End If
            Catch ex As Exception
                ShowMessage("Error deleting relationship: " & ex.Message, True)
            End Try
        End If
    End Sub

    ' --- HELPERS ---
    Private Function GetDataSet(ByVal sql As String) As DataSet
        Dim a As New clsDbo(sql)
        Dim ds As DataSet = a.selectDataset
        a.dispose()
        Return ds
    End Function

    Private Sub ExecuteNonQuery(ByVal sql As String)
        Dim a As New clsDbo("")
        a.executeNonQuery(sql)
        a.dispose()
    End Sub

    Private Sub ShowMessage(ByVal message As String, ByVal isError As Boolean)
        If isError Then
            lblMessage.Text = "<div class='alert alert-danger shadow-sm' style='border-radius:6px;'><i class='fas fa-exclamation-triangle mr-2'></i>" & message & "</div>"
        Else
            lblMessage.Text = "<div class='alert alert-success shadow-sm' style='border-radius:6px;'><i class='fas fa-check-circle mr-2'></i>" & message & "</div>"
        End If
    End Sub
End Class
