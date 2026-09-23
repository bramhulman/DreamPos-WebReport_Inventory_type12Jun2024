Imports System.IO
Imports System.Data
Imports MySql.Data.MySqlClient

Public Class Usedby
    Inherits System.Web.UI.Page
    Private mfunction As New ModFunction

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

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
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
                    Me.binding()
                End If
            Else
                Dim script As String = "window.onload = function(){ alert('Please Login'); window.location = '../Default'; }"
                ClientScript.RegisterStartupScript(Me.GetType(), "Redirect", script, True)
            End If
        Catch ex As Exception
            mfunction.errLog(Err.Description, "Usedby:Page_Load")
            Err.Clear()
        End Try
    End Sub

    Sub binding()
        Try
            Dim ds As New DataSet()

            Dim query As String = "SELECT " &
                "IFNULL(s.ID,0) AS RootStockID, " &
                "IFNULL(s.Number,'') AS RootStockNumber, " &
                "IFNULL(s.Name,'') AS RootStockName, " &
                "IFNULL(r1.ID,0) AS RecipeLevel1_ID, " &
                "IFNULL(r1.Code,'') AS RecipeLevel1_Code, " &
                "IFNULL(r1.Name,'') AS RecipeLevel1_Name, " &
                "IFNULL(s2.ID, IFNULL(m1.ItemNo, 0)) AS ProducedStockLevel1_ID, " &
                "IFNULL(s2.Number, IFNULL(m1.ItemNo, '')) AS ProducedStockLevel1_Number, " &
                "IFNULL(s2.Name, IFNULL(m1.ItemName, '')) AS ProducedStockLevel1_Name, " &
                "IFNULL(r2.ID,0) AS RecipeLevel2_ID, " &
                "IFNULL(r2.Code,'') AS RecipeLevel2_Code, " &
                "IFNULL(r2.Name,'') AS RecipeLevel2_Name, " &
                "IFNULL(s3.ID, IFNULL(m2.ItemNo, 0)) AS ProducedStockLevel2_ID, " &
                "IFNULL(s3.Number, IFNULL(m2.ItemNo, '')) AS ProducedStockLevel2_Number, " &
                "IFNULL(s3.Name, IFNULL(m2.ItemName, '')) AS ProducedStockLevel2_Name, " &
                "IFNULL(r3.ID,0) AS RecipeLevel3_ID, " &
                "IFNULL(r3.Code,'') AS RecipeLevel3_Code, " &
                "IFNULL(r3.Name,'') AS RecipeLevel3_Name, " &
                "IFNULL(s4.ID, IFNULL(m3.ItemNo, 0)) AS ProducedStockLevel3_ID, " &
                "IFNULL(s4.Number, IFNULL(m3.ItemNo, '')) AS ProducedStockLevel3_Number, " &
                "IFNULL(s4.Name, IFNULL(m3.ItemName, '')) AS ProducedStockLevel3_Name " &
                "FROM cld_inv_stk s " &
                "LEFT JOIN cld_inv_rcpdtl d1 ON d1.kodeId = s.ID AND d1.tipe = 1 " &
                "LEFT JOIN cld_inv_rcp r1 ON r1.ID = d1.rcpid AND r1.CustID = d1.CustID " &
                "LEFT JOIN cld_inv_stk s2 ON s2.BLink = r1.ID AND s2.CustID = r1.CustID " &
                "LEFT JOIN cld_itemmaster m1 ON m1.RID = r1.ID AND m1.CustID = r1.CustID AND m1.RID > 0 " &
                "LEFT JOIN cld_inv_rcpdtl d2 ON d2.kodeId = s2.ID AND d2.tipe = 1 " &
                "LEFT JOIN cld_inv_rcp r2 ON r2.ID = d2.rcpid AND r2.CustID = d2.CustID " &
                "LEFT JOIN cld_inv_stk s3 ON s3.BLink = r2.ID AND s3.CustID = r2.CustID " &
                "LEFT JOIN cld_itemmaster m2 ON m2.RID = r2.ID AND m2.CustID = r2.CustID AND m2.RID > 0 " &
                "LEFT JOIN cld_inv_rcpdtl d3 ON d3.kodeId = s3.ID AND d3.tipe = 1 " &
                "LEFT JOIN cld_inv_rcp r3 ON r3.ID = d3.rcpid AND r3.CustID = d3.CustID " &
                "LEFT JOIN cld_inv_stk s4 ON s4.BLink = r3.ID AND s4.CustID = r3.CustID " &
                "LEFT JOIN cld_itemmaster m3 ON m3.RID = r3.ID AND m3.CustID = r3.CustID AND m3.RID > 0 " &
                "WHERE (s2.ID IS NOT NULL OR m1.ItemNo IS NOT NULL OR s3.ID IS NOT NULL OR m2.ItemNo IS NOT NULL OR s4.ID IS NOT NULL OR m3.ItemNo IS NOT NULL) AND s.CustID = @CustID " &
                "ORDER BY s.number, r1.Code, r2.Code, r3.Code;"

            Using conn As New MySqlConnection(My.Settings.ServerStr)
                Using cmd As New MySqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@CustID", custid())
                    Using da As New MySqlDataAdapter(cmd)
                        da.Fill(ds)
                    End Using
                End Using
            End Using

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 Then
                lstViewDetails.DataSource = ds
            Else
                lstViewDetails.DataSource = Nothing
            End If
            lstViewDetails.DataBind()
        Catch ex As Exception
            mfunction.errLog(Err.Description, "Usedby:binding")
        End Try
    End Sub

End Class
