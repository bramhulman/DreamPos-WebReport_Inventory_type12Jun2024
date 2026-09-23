Public Class Pages_Calculate
    Inherits System.Web.UI.Page
    Private mfunction As New ModFunction
    Private cInv As New clsInventory
    Private CRtn As New Cls_IReturn

    Public Function modulepriv() As Integer
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        If cookie Is Nothing OrElse String.IsNullOrEmpty(cookie("modulepriv")) Then Return -1
        Dim val As Integer = -1
        Integer.TryParse(cookie("modulepriv"), val)
        Return val
    End Function

    Public Function custid() As Integer
        Dim icust As Integer = 0
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")

        If cookie IsNot Nothing Then
            If Not String.IsNullOrEmpty(cookie("custid")) Then
                Integer.TryParse(cookie("custid"), icust)
            ElseIf Not String.IsNullOrEmpty(cookie("Custid")) Then
                Integer.TryParse(cookie("Custid"), icust)
            End If
        End If

        ' Fallback jika custid bernilai 0 atau belum terisi
        If icust < 1 Then
            Dim c_cmd As New Cls_command
            Try
                icust = c_cmd.ExecuteScalar("SELECT id FROM `cld_cust` LIMIT 1", 1)
            Catch ex As Exception
                icust = 1
            End Try
        End If

        Return icust
    End Function

    Public Function username() As String
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        If cookie IsNot Nothing Then
            If Not String.IsNullOrEmpty(cookie("userName")) Then
                Return cookie("userName")
            ElseIf Not String.IsNullOrEmpty(cookie("username")) Then
                Return cookie("username")
            End If
        End If
        If Session("username") IsNot Nothing Then
            Return Session("username").ToString()
        End If
        Return ""
    End Function

    Public Function rptpriv() As Integer
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        If cookie Is Nothing OrElse String.IsNullOrEmpty(cookie("rptpriv")) Then Return -1
        Dim val As Integer = -1
        Integer.TryParse(cookie("rptpriv"), val)
        Return val
    End Function

    Public Function modul() As Integer
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        If cookie Is Nothing OrElse String.IsNullOrEmpty(cookie("modul")) Then Return -1
        Dim val As Integer = -1
        Integer.TryParse(cookie("modul"), val)
        Return val
    End Function

    Public Function rptf() As Integer
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        If cookie Is Nothing OrElse String.IsNullOrEmpty(cookie("rptf")) Then Return -1
        Dim val As Integer = -1
        Integer.TryParse(cookie("rptf"), val)
        Return val
    End Function

    Public Function inv_priv() As Integer
        Dim cookie As HttpCookie = Request.Cookies("UserLogin_inv")
        If cookie Is Nothing OrElse String.IsNullOrEmpty(cookie("invpriv")) Then Return -1
        Dim val As Integer = -1
        Integer.TryParse(cookie("invpriv"), val)
        Return val
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' Jalankan validasi dan pemulihan sesi standar proyek
        mHelper.CekSession()

        Try
            sNameuser = username()
            icustid = custid()
            imodul = modul()
            iModPriv = modulepriv()
            irptpriv = rptpriv()
            irptf = rptf()
            invpriv = inv_priv()

            If IsPostBack = False Then
                txtDate.Text = Format(Now.AddHours(-1), "dd MMM yyyy")
                LoadDropdowns()
            End If
        Catch ex As Exception
            mfunction.errLog(Err.Description, "Pages_Calculate:Page_Load")
            Err.Clear()
        End Try
    End Sub
    Protected Sub txtDate_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtDate.TextChanged
        If txtDate.Text = "" Then
            txtDate.Text = Format(Now, "dd MMM yyyy")

        End If

    End Sub

    'Protected Sub txtto_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtTo.TextChanged
    '    If txtTo.Text = "" Then

    '        txtTo.Text = Format(Now, "dd MMM yyyy")
    '    End If

    'End Sub
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



    Protected Sub btncalcusage_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btncalcusage.ServerClick
        Try
            Dim x As Integer
            Dim i As Integer = DateDiff(DateInterval.Day, Convert.ToDateTime(txtDate.Text), Today())
            Dim c_cmd As New clsDbo("")
            If i > My.Settings.limitday Then
                notsuccesfull("Maximum " & My.Settings.limitday & " days ago")
                Exit Sub
            ElseIf i < 0 Then
                notsuccesfull("Date Must be before Date Now")
                Exit Sub
            End If
            If i > 0 Then
                Dim dfrom As Date
                dfrom = Convert.ToDateTime(txtDate.Text)
                x = 0
                Dim clsInv As New clsInventory

                'usage recalculate
                'cInv.recall_InvUsage(CDate(txtDate.Text), custid)


                Do While x <= i
                    'cInv.proc_invusage(custid, dfrom)
                    'Dim ds As New DataSet

                    clsInv.Process_InvUsage(dfrom, custid)


                    dfrom = dfrom.AddDays(1)
                    x = x + 1
                Loop
                'clsInv.InvCaclculate(Convert.ToDateTime(txtDate.Text), custid, username)
                'c_cmd.executeNonQuery("DELETE FROM cld_inv_closing 
                'where invdate = '" & Now.ToString("yyyy-MM-dd 00:00:00") & "' and custid = " & custid() & "")


                messagesuccesfull("Calculate Inventory Succes")
            End If
        Catch ex As Exception
            mfunction.errLog(Err.Description, "Pages_Calculate:btncalcusage_Click")
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
    Sub messagesuccesfull(ByVal msg As String)
        Dim message As String = msg
        Dim script As String = "window.onload = function(){ alert('"
        script += message
        script += "'); }"
        If (Page IsNot Nothing And Not Page.ClientScript.IsClientScriptBlockRegistered("alert")) Then
            Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "alert", script, True)
        End If
    End Sub

    Protected Sub btncalccost_ServerClick(sender As Object, e As EventArgs) Handles btncalccost.ServerClick
        Try
            Dim clsrecal As New cls_recalculate
            Dim selCC As Integer = 0
            Dim selStock As Integer = 0
            If ddlCC.SelectedValue <> "" Then selCC = Convert.ToInt32(ddlCC.SelectedValue)
            If ddlStock.SelectedValue <> "" Then selStock = Convert.ToInt32(ddlStock.SelectedValue)
            
            ' Gunakan fungsi baru untuk rekalkulasi cost secara spesifik sesuai logic tofix_cost
            clsrecal.InvCaclculate_Cost_ActiveTransOnly(CDate(txtDate.Text), Now.Date, custid, selCC, selStock)
            
            Dim clsInv_cost As New clsInventory_Receipe
            Dim clsinv_itemmaster As New clsInventory_ItemMaster

            'buat kalkulasi cost resep dan item master (HANYA dijalankan jika memproses SEMUA barang, karena sangat berat)
            If selCC = 0 AndAlso selStock = 0 Then
                clsInv_cost.Process_UpdateCostReceipe(custid, Now.Date.ToString("dd MMM yyyy"))
                clsinv_itemmaster.Process_UpdateCostitemMaster(custid)
            End If
            
            messagesuccesfull("Calculate Moving Average Cost Success! Please click 'Home' to view updated stock data.")
        Catch ex As Exception
            mfunction.errLog(Err.Description, "Pages_Calculate:btncalccost_ServerClick")
            Err.Clear()
        End Try
    End Sub
    Protected Sub btnrecal_ServerClick(sender As Object, e As EventArgs) Handles btnrecal.ServerClick
        Dim clsrecal As New cls_recalculate
        Dim selCC As Integer = 0
        Dim selStock As Integer = 0
        If ddlCC.SelectedValue <> "" Then selCC = Convert.ToInt32(ddlCC.SelectedValue)
        If ddlStock.SelectedValue <> "" Then selStock = Convert.ToInt32(ddlStock.SelectedValue)
            
        ' Gunakan query teroptimasi yang hanya memproses item dengan transaksi (sesuai saran agar cepat)
        clsrecal.InvCaclculate_ActiveTransOnly(CDate(txtDate.Text), Now.Date, custid, selCC, selStock)
        
        messagesuccesfull("Calculate Full Stock Success")
    End Sub

    Protected Sub btnSyncCost_ServerClick(sender As Object, e As EventArgs) Handles btnSyncCost.ServerClick
        Try
            Dim clsrecal As New cls_recalculate
            Dim selCC As Integer = 0
            Dim selStock As Integer = 0
            If ddlCC.SelectedValue <> "" Then selCC = Convert.ToInt32(ddlCC.SelectedValue)
            If ddlStock.SelectedValue <> "" Then selStock = Convert.ToInt32(ddlStock.SelectedValue)
            
            clsrecal.SyncCostFromWarehouse(CDate(txtDate.Text), custid, selCC, selStock)
            
            Showalert("Sync Cost from Warehouse Success")
        Catch ex As Exception
            mfunction.errLog(Err.Description, "Pages_Calculate:btnSyncCost_ServerClick")
            Err.Clear()
        End Try
    End Sub
    Private Sub LoadDropdowns()
        Dim c_cmd As New clsDbo("")
        Try
            ddlCC.Items.Clear()
            ddlCC.Items.Add(New ListItem("-- All Cost Centre --", "0"))
            Dim dsCC As DataSet = c_cmd.ReturnDataset("SELECT id, name as ccname FROM cld_inv_cc WHERE custid = " & custid() & " ORDER BY name")
            If dsCC IsNot Nothing AndAlso dsCC.Tables.Count > 0 Then
                For Each row As DataRow In dsCC.Tables(0).Rows
                    ddlCC.Items.Add(New ListItem(row("ccname").ToString(), row("id").ToString()))
                Next
            End If
            
            LoadStock(0)
        Catch ex As Exception
            mfunction.errLog(Err.Description, "Pages_Calculate:LoadDropdowns")
            Err.Clear()
        Finally
            c_cmd.dispose()
        End Try
    End Sub

    Private Sub LoadStock(ByVal ccid As Integer)
        Dim c_cmd As New clsDbo("")
        Try
            ddlStock.Items.Clear()
            ddlStock.Items.Add(New ListItem("-- All Stock --", "0"))
            Dim strSQL As String = "SELECT id as stk_id, name as stk_name FROM cld_inv_stk WHERE custid = " & custid()
            If ccid > 0 Then
                strSQL &= " AND id IN (SELECT stkid FROM cld_inv_stk_cc WHERE ccid = " & ccid & ")"
            End If
            strSQL &= " ORDER BY name"
            Dim dsStock As DataSet = c_cmd.ReturnDataset(strSQL)
            If dsStock IsNot Nothing AndAlso dsStock.Tables.Count > 0 Then
                For Each row As DataRow In dsStock.Tables(0).Rows
                    ddlStock.Items.Add(New ListItem(row("stk_name").ToString(), row("stk_id").ToString()))
                Next
            End If
        Catch ex As Exception
            mfunction.errLog(Err.Description, "Pages_Calculate:LoadStock")
            Err.Clear()
        Finally
            c_cmd.dispose()
        End Try
    End Sub

    Protected Sub ddlCC_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlCC.SelectedIndexChanged
        If ddlCC.SelectedValue <> "" Then
            LoadStock(Convert.ToInt32(ddlCC.SelectedValue))
        End If
    End Sub

End Class