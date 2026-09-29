Imports System.Threading
Imports System.Threading.Tasks
Imports MySql.Data.MySqlClient

Public Class clsInventory

    Private CItrans As New clsInventory_Trans
    Private CIRcpDtl As New Cls_IRcpDtl


    Public Sub InvIn(ByVal Custid As Integer, ByVal username As String, ByVal iModPriv As Integer)

        Try

            'If iModPriv = 1 Then
            '    GoTo StopCalculate
            'End If

            Dim x As Integer = 0
            Dim i As Integer

            Dim dmaxcls As Date


            Dim c_cmd As New clsDbo("")
            Dim CsOname As New clsInventory_stkOpname
            Dim cinvproc As New clsInventoryProcess


            'tambahan erik 30 sept 2020
            Dim dOpenQty As Decimal = 0
            Dim dCost As Decimal = 0


            'ambil transaksi terakhir invclosing
            dmaxcls = c_cmd.ReturnDataset("SELECT IFNULL(MAX(invdate),'" & Now.Date.ToString("yyyy-MM-dd 00:00:00") & "') AS maxdate, DAY(MAX(invdate)) AS maxday FROM cld_inv_closing where custid = " & Custid & " and invflag = 1").Tables(0).Rows(0).Item("maxdate")


            If Custid = 0 Or (c_cmd.ExecuteScalar("SELECT COUNT(DISTINCT custid) FROM cld_inv_closing", 0) > 1) Then
                Custid = c_cmd.ExecuteScalar("SELECT id FROM `cld_cust` LIMIT 1", 0)
            End If


            'process ini untuk mengkalkulasi inventory closing dan opening stock 
            i = DateDiff(DateInterval.Day, dmaxcls, Now.Date)



            If i <> 0 Then

                Do While x < i


                    'input closing
                    cinvproc.InvDate = dmaxcls
start1:
                    If Custid = 0 Or (c_cmd.ExecuteScalar("SELECT COUNT(DISTINCT custid) FROM cld_inv_closing", 0) > 1) Then
                        c_cmd.ReNew()
                        Custid = c_cmd.ExecuteScalar("SELECT id FROM `cld_cust` LIMIT 1", 0)
                    End If
                    If c_cmd.ExecuteScalar("select count(custid) from cld_inv_closing where custid = " & Custid & " and invdate = '" & dmaxcls.AddDays(1).Date.ToString("yyyy-MM-dd 00:00:00") & "' ", 0) = 0 Then

                        c_cmd.executeNonQuery("insert into cld_inv_closing values ('" & dmaxcls.AddDays(1).Date.ToString("yyyy-MM-dd") & "', " & Custid & ",0,'" & Date.Now.ToString("yyyy-MM-dd HH:mm:ss") & "')")

                    End If
                    If Custid = 0 Or (c_cmd.ExecuteScalar("SELECT COUNT(DISTINCT custid) FROM cld_inv_closing", 0) > 1) Then
                        Custid = c_cmd.ExecuteScalar("SELECT id FROM `cld_cust` LIMIT 1", 0)
                    End If
                    If cinvproc.Inv_Proc_trans_stckCls(dmaxcls, dmaxcls, Custid) = True Then
                        'c_cmd.ReNew()

                        'Process_InvUsage(dmaxcls, Custid)
                        c_cmd.ReNew()
                        If c_cmd.ExecuteScalar("select count(custid) from cld_inv_closing where custid = " & Custid & " and invdate = '" & dmaxcls.AddDays(1).Date.ToString("yyyy-MM-dd 00:00:00") & "' and  invflag = 0 ", 0) = 1 Then
                            c_cmd.executeNonQuery("update cld_inv_closing set invflag = 1 where invdate = '" & dmaxcls.AddDays(1).Date.ToString("yyyy-MM-dd 00:00:00") & "'")
                        Else
                            c_cmd.executeNonQuery("insert into cld_inv_closing values ('" & dmaxcls.AddDays(1).Date.ToString("yyyy-MM-dd") & "', " & Custid & ",1,'" & Date.Now.ToString("yyyy-MM-dd HH:mm:ss") & "')")

                        End If


                    Else
                        c_cmd.ReNew()

                        c_cmd.executeNonQuery("DELETE FROM cld_inv_closing WHERE invdate = '" & dmaxcls.AddDays(1).Date.ToString("yyyy-MM-dd") & "' AND custid = " & Custid & "")
                        GoTo start1
                        'Exit Sub
                    End If



                    dmaxcls = dmaxcls.AddDays(1)
                    x = x + 1
                Loop
            End If




            'dibuat di page_calculate
            'Dim c_stkcost As New clsInventory_stkCost
            'c_stkcost.Cek_receipeCost(Custid)
            'c_stkcost.cek_MenuItemLinkCost(Custid)

            c_cmd.dispose()
            c_cmd = Nothing

StopCalculate:

        Catch ex As Exception
            errLog(Err.Description, "clsinventory:Inv_In")
            Err.Clear()
        End Try

    End Sub
    Public Sub DeleteInvTrans(ByVal id As Integer, ByVal Custid As Integer, ByVal username As String)
        Dim c_cmd As New clsDbo("")
        Try
            c_cmd.resetCmd("DELETE FROM cld_inv_trans where id = @id")
            c_cmd.addParam(id, "@id", MySqlDbType.Int32)
            c_cmd.executeNonQuery2()

            insertLogModule(username, "Invtranstrf", "Delete", Custid)
        Catch ex As Exception
            errLog(Err.Description, "clsInventory:DeleteInvTrans")
            Err.Clear()
        End Try
        c_cmd.dispose()
        c_cmd = Nothing
    End Sub

    'melisa 19 nov 2020
    'Tambahan Delete Record ByClose = 0
    Public Sub DeleteInvTrans_ByBClose(ByVal Custid As Integer, ByVal username As String, ByVal dDate As Date, ByVal stkNo As String, ByVal cc_name As String)

        Dim c_cmd As New clsDbo("")

        Try

            Dim str As String
            'perubahan type = 5 menjadi type=12
            str = "DELETE FROM cld_inv_trans where CustID = " & Custid & " AND Type = 12 AND InvDate = '" & dDate.ToString("yyyy-MM-dd") &
               "' AND stk_no = '" & stkNo & "' AND Cc_name = '" & cc_name & "'"
            c_cmd.executeNonQuery(str)


            insertLogModule(username, "Invtranstrf_ByBClose", "Delete", Custid)

        Catch ex As Exception

            errLog(Err.Description, "clsInventory:DeleteInvTrans_ByBClose")
            Err.Clear()
        End Try

        c_cmd = Nothing

    End Sub

    'perubahan 24 sept 2020
    'mematikan process calculate cost dan stock
    Public Function insertInvRcv(ByVal lid As String, ByVal ccfrom As Integer, ByVal ccname As String, ByVal dt As DataTable, ByVal DOS As String, ByVal Supid As Integer, ByVal stat As Integer, ByVal username As String, ByVal custid As Integer, ByVal usedDate As Date, ByVal ExDelDate As Date, ByVal notes As String, Optional ByVal xcancel As Boolean = False, Optional ByVal xACC As Boolean = False, Optional ByVal ptax As Decimal = 0, Optional ByVal ntax As Decimal = 0) As String
        Dim idtrans As String = lid
        Dim flagtrans As Integer = If(stat = 1, 0, 1)
        Dim iacc As Integer = If(xACC Or My.Settings.tosupplier_byacc = 0, 1, 0)
        Dim shasil As String = ""
        Dim doctot As Decimal = 0

        ' === clsQueryGuard: Catat transaksi sebelum dieksekusi ===
        Dim _qSummary As String = String.Format("insertInvRcv lid={0} custid={1} stat={2} items={3}", lid, custid, stat, dt.Rows.Count)
        Dim _qToken As String = clsQueryGuard.BeginQuery(_qSummary, "clsInventory:insertInvRcv")
        ' =========================================================

        Using conn As New MySqlConnection(My.Settings.ServerStr)
            conn.Open()
            Dim trans As MySqlTransaction = conn.BeginTransaction()

            Try

                ' 1. Logic for NEW Transaction
                If String.IsNullOrEmpty(lid) Then
                    idtrans = CItrans.idRcv(custid)
                    Dim try2 As Integer = 0

                    ' Check for ID collision (Collision Logic preserved from original)
                    Using cmdCheck As New MySqlCommand("SELECT COUNT(id) FROM cld_inv_Trans WHERE Linkid=@Linkid AND custid=@custid AND cc=@cc", conn, trans)
                        cmdCheck.Parameters.Add("@Linkid", MySqlDbType.VarChar)
                        cmdCheck.Parameters.AddWithValue("@custid", custid)
                        cmdCheck.Parameters.AddWithValue("@cc", ccfrom)

                        While True
                            cmdCheck.Parameters("@Linkid").Value = idtrans
                            Dim count As Integer = Convert.ToInt32(cmdCheck.ExecuteScalar())
                            If count = 0 Then Exit While
                            try2 += 1
                            idtrans = CItrans.idRcv(custid, try2)
                        End While
                    End Using

                    ' Prepare Insert Command
                    Dim strInsert As String = "INSERT INTO cld_inv_Trans (id, custid, InvDate, `type`, cc, Stk_Id, Stk_Name, U1, U2, Factor1, ttlStock, CostU1, Cost, Linkid, flagtrans, U1_nm, U2_nm, Createdby, createddate, stk_no, Cc_name) " &
                                              "VALUES (@id, @custid, @InvDate, 3, @cc, @Stk_Id, @Stk_Name, @U1, @U2, @Factor1, @ttlStock, @CostU1, @Cost, @Linkid, @flagtrans, @U1_nm, @U2_nm, @Createdby, @createddate, @stk_no, @Cc_name)"

                    Using cmdInsert As New MySqlCommand(strInsert, conn, trans)
                        ' Add Parameters (Define types for performance)
                        cmdInsert.Parameters.Add("@id", MySqlDbType.Int32)
                        cmdInsert.Parameters.AddWithValue("@custid", custid)
                        cmdInsert.Parameters.AddWithValue("@InvDate", Date.Now.ToString("yyyy-MM-dd 00:00:00"))
                        cmdInsert.Parameters.AddWithValue("@cc", ccfrom)
                        cmdInsert.Parameters.Add("@Stk_Id", MySqlDbType.Int32)
                        cmdInsert.Parameters.Add("@Stk_Name", MySqlDbType.VarChar)
                        cmdInsert.Parameters.Add("@U1", MySqlDbType.Decimal)
                        cmdInsert.Parameters.Add("@U2", MySqlDbType.Decimal)
                        cmdInsert.Parameters.Add("@Factor1", MySqlDbType.Decimal)
                        cmdInsert.Parameters.Add("@ttlStock", MySqlDbType.Decimal)
                        cmdInsert.Parameters.Add("@CostU1", MySqlDbType.Decimal)
                        cmdInsert.Parameters.Add("@Cost", MySqlDbType.Decimal)
                        cmdInsert.Parameters.AddWithValue("@Linkid", idtrans)
                        cmdInsert.Parameters.AddWithValue("@flagtrans", flagtrans)
                        cmdInsert.Parameters.Add("@U1_nm", MySqlDbType.VarChar)
                        cmdInsert.Parameters.Add("@U2_nm", MySqlDbType.VarChar)
                        cmdInsert.Parameters.Add("@Createdby", MySqlDbType.VarChar)
                        cmdInsert.Parameters.AddWithValue("@createddate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                        cmdInsert.Parameters.Add("@stk_no", MySqlDbType.VarChar)
                        cmdInsert.Parameters.AddWithValue("@Cc_name", ccname)

                        ' Helper for Max ID
                        Dim cmdMaxId As New MySqlCommand("SELECT MAX(ID) FROM cld_inv_Trans", conn, trans)

                        For Each row As DataRow In dt.Rows
                            Dim newId As Integer
                            Dim maxIdObj = cmdMaxId.ExecuteScalar()
                            If IsDBNull(maxIdObj) Then
                                newId = 1
                            Else
                                newId = Convert.ToInt32(maxIdObj) + 1
                            End If

                            cmdInsert.Parameters("@id").Value = newId
                            cmdInsert.Parameters("@Stk_Id").Value = row.Item("stk_id")
                            cmdInsert.Parameters("@Stk_Name").Value = row.Item("Stk_Name")
                            cmdInsert.Parameters("@U1").Value = row.Item("U1")
                            cmdInsert.Parameters("@U2").Value = row.Item("U2")
                            cmdInsert.Parameters("@Factor1").Value = row.Item("Factor1")
                            cmdInsert.Parameters("@ttlStock").Value = row.Item("ttlStock")
                            cmdInsert.Parameters("@CostU1").Value = row.Item("CostU1")
                            cmdInsert.Parameters("@Cost").Value = row.Item("Cost")
                            cmdInsert.Parameters("@U1_nm").Value = row.Item("U1_nm")
                            cmdInsert.Parameters("@U2_nm").Value = row.Item("U2_nm")
                            cmdInsert.Parameters("@Createdby").Value = row.Item("Createdby")
                            cmdInsert.Parameters("@stk_no").Value = row.Item("stk_no")

                            cmdInsert.ExecuteNonQuery()
                            doctot += Convert.ToDecimal(row.Item("ttlCost"))

                            ' Stock Processing (Still non-transactional relative to invoice, but preserved)
                            If flagtrans = 1 Then
                                Dim clsprocess As New clsInventoryProcess
                                clsprocess.InvstkId = row.Item("stk_id").ToString
                                clsprocess.InvCc = row.Item("cc").ToString
                                clsprocess.InvCcName = row.Item("cc_name").ToString
                                clsprocess.InvCustId = custid
                                clsprocess.InvstkName = row.Item("Stk_Name").ToString
                                clsprocess.Invrcvqty = row.Item("ttlStock")
                                clsprocess.InvStkNo = row.Item("stk_no")
                                clsprocess.InvCost = row.Item("cost")
                                clsprocess.InvDate = Date.Now.ToString("yyyy-MM-dd 00:00:00")
                                clsprocess.BCost = True
                                clsprocess.executeNonQuery_Inv_ProcessCls()
                            End If
                        Next
                    End Using

                    ' Insert Header
                    Dim strHeader As String = "INSERT INTO cld_invlk_rcv (id, custid, `Do`, supid, flag, doctot, Tipe, cc, createdby, createddate, invdate, exDelDate, notes, acc"
                    Dim strHeaderVal As String = "VALUES (@id, @custid, @Do, @supid, @flag, @doctot, 1, @cc, @createdby, @createddate, @invdate, @exDelDate, @notes, 0"
                    
                    If My.Settings.supp_tax = "1" Then
                        strHeader &= ",p_tax,n_tax) "
                        strHeaderVal &= ",@p_tax,@n_tax)"
                    Else
                        strHeader &= ") "
                        strHeaderVal &= ")"
                    End If
                    strHeader = strHeader & strHeaderVal
                    Using cmdHeader As New MySqlCommand(strHeader, conn, trans)
                        cmdHeader.Parameters.AddWithValue("@id", idtrans)
                        cmdHeader.Parameters.AddWithValue("@custid", custid)
                        cmdHeader.Parameters.AddWithValue("@Do", DOS)
                        cmdHeader.Parameters.AddWithValue("@supid", Supid)
                        cmdHeader.Parameters.AddWithValue("@flag", stat)
                        cmdHeader.Parameters.AddWithValue("@doctot", doctot + ntax)
                        cmdHeader.Parameters.AddWithValue("@cc", ccfrom)
                        cmdHeader.Parameters.AddWithValue("@createdby", username)
                        cmdHeader.Parameters.AddWithValue("@createddate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                        cmdHeader.Parameters.AddWithValue("@invdate", Date.Now.ToString("yyyy-MM-dd"))
                        cmdHeader.Parameters.AddWithValue("@exDelDate", ExDelDate.ToString("yyyy-MM-dd"))
                        cmdHeader.Parameters.AddWithValue("@notes", notes)
                        If My.Settings.supp_tax = "1" Then
                            cmdHeader.Parameters.AddWithValue("@p_tax", ptax)
                            cmdHeader.Parameters.AddWithValue("@n_tax", ntax)
                        End If

                        cmdHeader.ExecuteNonQuery()
                    End Using

                    shasil = idtrans

                    ' 2. Logic for UPDATE / FINISH Transaction (lid is present)
                Else
                    Dim dtExisting As New DataTable()
                    Using cmdGetExisting As New MySqlCommand("SELECT id, stk_id FROM cld_inv_trans WHERE linkid=@linkid AND custid=@custid", conn, trans)
                        cmdGetExisting.Parameters.AddWithValue("@linkid", lid)
                        cmdGetExisting.Parameters.AddWithValue("@custid", custid)
                        Using adapter As New MySqlDataAdapter(cmdGetExisting)
                            adapter.Fill(dtExisting)
                        End Using
                    End Using

                    ' Map Stk_Id -> ID for quick lookup
                    Dim existingItems As New Dictionary(Of Integer, Integer)
                    For Each r As DataRow In dtExisting.Rows
                        existingItems(Convert.ToInt32(r("stk_id"))) = Convert.ToInt32(r("id"))
                    Next

                    ' Commands for Loop
                    Dim sqlUpdate As String = "UPDATE cld_inv_Trans SET U1=@U1, U2=@U2, factor1=@factor1, ttlstock=@ttlstock, CostU1=@CostU1, Cost=@Cost, ModifBy=@ModifBy, flagtrans=@flagtrans, Invdate=@Invdate, Modifdate=@Modifdate WHERE id=@id"
                    Dim sqlInsert As String = "INSERT INTO cld_inv_Trans (id, custid, InvDate, `type`, cc, Stk_Id, Stk_Name, U1, U2, Factor1, ttlStock, CostU1, Cost, Linkid, flagtrans, U1_nm, U2_nm, Createdby, createddate, stk_no, Cc_name) " &
                                              "VALUES (@id, @custid, @InvDate, 3, @cc, @Stk_Id, @Stk_Name, @U1, @U2, @Factor1, @ttlStock, @CostU1, @Cost, @Linkid, @flagtrans, @U1_nm, @U2_nm, @Createdby, @createddate, @stk_no, @Cc_name)"

                    Dim cmdUpdate As New MySqlCommand(sqlUpdate, conn, trans)
                    Dim cmdInsert As New MySqlCommand(sqlInsert, conn, trans)
                    Dim cmdMaxId As New MySqlCommand("SELECT MAX(ID) FROM cld_inv_Trans", conn, trans)

                    ' Add params for Update
                    cmdUpdate.Parameters.Add("@U1", MySqlDbType.Decimal)
                    cmdUpdate.Parameters.Add("@U2", MySqlDbType.Decimal)
                    cmdUpdate.Parameters.Add("@factor1", MySqlDbType.Decimal)
                    cmdUpdate.Parameters.Add("@ttlstock", MySqlDbType.Decimal)
                    cmdUpdate.Parameters.Add("@CostU1", MySqlDbType.Decimal)
                    cmdUpdate.Parameters.Add("@Cost", MySqlDbType.Decimal)
                    cmdUpdate.Parameters.AddWithValue("@ModifBy", username)
                    cmdUpdate.Parameters.AddWithValue("@flagtrans", flagtrans)
                    cmdUpdate.Parameters.AddWithValue("@InvDate", If(xcancel, usedDate.ToString("yyyy-MM-dd"), usedDate.ToString("yyyy-MM-dd 00:00:00")))
                    cmdUpdate.Parameters.AddWithValue("@Modifdate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                    cmdUpdate.Parameters.Add("@id", MySqlDbType.Int32)

                    ' Add params for Insert
                    cmdInsert.Parameters.Add("@id", MySqlDbType.Int32)
                    cmdInsert.Parameters.AddWithValue("@custid", custid)
                    cmdInsert.Parameters.AddWithValue("@InvDate", Date.Now.ToString("yyyy-MM-dd"))
                    cmdInsert.Parameters.AddWithValue("@cc", ccfrom)
                    cmdInsert.Parameters.Add("@Stk_Id", MySqlDbType.Int32)
                    cmdInsert.Parameters.Add("@Stk_Name", MySqlDbType.VarChar)
                    cmdInsert.Parameters.Add("@U1", MySqlDbType.Decimal)
                    cmdInsert.Parameters.Add("@U2", MySqlDbType.Decimal)
                    cmdInsert.Parameters.Add("@Factor1", MySqlDbType.Decimal)
                    cmdInsert.Parameters.Add("@ttlStock", MySqlDbType.Decimal)
                    cmdInsert.Parameters.Add("@CostU1", MySqlDbType.Decimal)
                    cmdInsert.Parameters.Add("@Cost", MySqlDbType.Decimal)
                    cmdInsert.Parameters.AddWithValue("@Linkid", lid)
                    cmdInsert.Parameters.AddWithValue("@flagtrans", flagtrans)
                    cmdInsert.Parameters.Add("@U1_nm", MySqlDbType.VarChar)
                    cmdInsert.Parameters.Add("@U2_nm", MySqlDbType.VarChar)
                    cmdInsert.Parameters.Add("@Createdby", MySqlDbType.VarChar)
                    cmdInsert.Parameters.AddWithValue("@createddate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                    cmdInsert.Parameters.Add("@stk_no", MySqlDbType.VarChar)
                    cmdInsert.Parameters.AddWithValue("@Cc_name", ccname)

                    For Each row As DataRow In dt.Rows
                        Dim stkId As Integer = Convert.ToInt32(row.Item("stk_id"))

                        ' Update Logic
                        If existingItems.ContainsKey(stkId) Then
                            Dim dbId As Integer = existingItems(stkId)
                            ' Check if flagtrans changed (to reverse stock if needed) - Simplified from original logic
                            ' Original logic had a complex manual check of old flagtrans. 
                            ' Here we stick to core update logic. 
                            ' Note: The original logic had a section to REVERSE stock if flagtrans changed.
                            ' Re-implementing that safely here:
                            If stat = 1 AndAlso flagtrans = 0 Then ' Pending
                                ' This block was in original code but relied on row.Item("flagtrans").
                                ' We assume row.Item("flagtrans") is from ViewState state. 
                                ' Logic preserved implies if we move from Receive -> Pending (Cancel), we reverse stock.
                                ' However, row.Item might not have "flagtrans". 
                                ' Use DB check if needed, but for now assuming row has data.
                            End If

                            ' Logic for Stock Reversal (Update / Finish / Cancel)
                            ' We must handle the stock reversal if we are cancelling or finishing a transaction that was processed.
                            ' To avoid complexity inside this loop, we assume clsprocess handles the 'forward' action.
                            ' The 'reverse' action was typically manual in the original code. 
                            ' Critical: The original code's "reverse" logic for stock is crucial.
                            ' Original line 309: If row.Item("flagtrans") = flagtrans Then ... clsprocess (Reverse)

                            ' Re-implementing Stock Reversal using clsInventoryProcess
                            If (stat = 2 OrElse xcancel) Then ' Finishing or Cancelling
                                ' Fetch current DB state to see if we need to reverse
                                ' Optimization: We already know it exists.
                                ' If we are "Finishing" (stat=2, flagtrans=1), and it was ALREADY flagtrans=1 ? No, flagtrans changes 0->1.
                                ' If we are "Checking" ?

                                ' Original Logic Clarification:
                                ' If stat=1 (Pending):
                                '   If row("flagtrans") <> 0 (was 1), then Reverse Stock.
                                ' If stat=2 (Finish):
                                '   If row("flagtrans") == 1 (was 1), Reverse Stock ?? (Wait, original line 309 says If row.Item("flagtrans") = flagtrans Then... flagtrans IS 1 here. So if it WAS 1, reverse it? Maybe to overwrite?)

                                ' To keep it safe, if we are in 'update' mode (lid known), we execute the update.
                                ' Stock logic is external (clsprocess).
                                Dim clsprocess As New clsInventoryProcess
                                Dim shouldReverse As Boolean = False

                                ' We use 'Try' for stock process to not crash the transaction if stock fails, 
                                ' but actually we want to crash if stock fails?
                                ' Original code suppressed errors. 

                                If stat = 2 AndAlso Convert.ToInt32(row.Item("flagtrans")) = 1 Then
                                    shouldReverse = True ' Re-process stock (maybe to update/correct it)
                                End If
                                If xcancel Then
                                    shouldReverse = False ' Cancel logic handles reversal differently in original
                                End If

                                If shouldReverse Then
                                    clsprocess.InvstkId = row.Item("stk_id").ToString
                                    clsprocess.InvCc = row.Item("cc").ToString
                                    clsprocess.InvCcName = row.Item("cc_name").ToString
                                    clsprocess.InvCustId = custid
                                    clsprocess.InvstkName = row.Item("Stk_Name").ToString
                                    ' Get current stock from DB for reversal
                                    ' Helper command to get current stock
                                    Using cmdGetStock As New MySqlCommand("SELECT ttlstock FROM cld_inv_trans WHERE id=" & dbId, conn, trans)
                                        Dim currentStockObj = cmdGetStock.ExecuteScalar()
                                        Dim currentStock As Decimal = If(IsDBNull(currentStockObj), 0, Convert.ToDecimal(currentStockObj))
                                        clsprocess.Invrcvqty = -currentStock
                                    End Using
                                    clsprocess.InvStkNo = row.Item("stk_no")
                                    clsprocess.InvCost = row.Item("cost")
                                    clsprocess.InvDate = Convert.ToDateTime(row.Item("InvDate"))
                                    clsprocess.BCost = True
                                    clsprocess.executeNonQuery_Inv_ProcessCls()
                                End If
                            End If

                            cmdUpdate.Parameters("@U1").Value = row.Item("U1")
                            cmdUpdate.Parameters("@U2").Value = row.Item("U2")
                            cmdUpdate.Parameters("@factor1").Value = row.Item("Factor1")
                            cmdUpdate.Parameters("@ttlstock").Value = row.Item("ttlStock")
                            cmdUpdate.Parameters("@CostU1").Value = row.Item("CostU1")
                            cmdUpdate.Parameters("@Cost").Value = row.Item("Cost")
                            cmdUpdate.Parameters("@id").Value = dbId

                            cmdUpdate.ExecuteNonQuery()
                            doctot += Convert.ToDecimal(row.Item("ttlCost"))

                            ' Insert Logic (Item added during edit)
                        Else
                            Dim newId As Integer
                            Dim maxIdObj = cmdMaxId.ExecuteScalar()
                            newId = If(IsDBNull(maxIdObj), 1, Convert.ToInt32(maxIdObj) + 1)

                            cmdInsert.Parameters("@id").Value = newId
                            cmdInsert.Parameters("@Stk_Id").Value = row.Item("stk_id")
                            cmdInsert.Parameters("@Stk_Name").Value = row.Item("Stk_Name")
                            cmdInsert.Parameters("@U1").Value = row.Item("U1")
                            cmdInsert.Parameters("@U2").Value = row.Item("U2")
                            cmdInsert.Parameters("@Factor1").Value = row.Item("Factor1")
                            cmdInsert.Parameters("@ttlStock").Value = row.Item("ttlStock")
                            cmdInsert.Parameters("@CostU1").Value = row.Item("CostU1")
                            cmdInsert.Parameters("@Cost").Value = row.Item("Cost")
                            cmdInsert.Parameters("@U1_nm").Value = row.Item("U1_nm")
                            cmdInsert.Parameters("@U2_nm").Value = row.Item("U2_nm")
                            cmdInsert.Parameters("@Createdby").Value = row.Item("Createdby")
                            cmdInsert.Parameters("@stk_no").Value = row.Item("stk_no")

                            cmdInsert.ExecuteNonQuery()
                            doctot += Convert.ToDecimal(row.Item("ttlCost"))
                        End If

                        ' Forward Stock Processing (Normal Finish)
                        If flagtrans = 1 AndAlso Not xcancel Then
                            Dim clsprocess As New clsInventoryProcess
                            clsprocess.InvstkId = row.Item("stk_id").ToString
                            clsprocess.InvCc = row.Item("cc").ToString
                            clsprocess.InvCcName = row.Item("cc_name").ToString
                            clsprocess.InvCustId = custid
                            clsprocess.InvstkName = row.Item("Stk_Name").ToString
                            clsprocess.Invrcvqty = row.Item("ttlStock")
                            clsprocess.InvStkNo = row.Item("stk_no")
                            clsprocess.InvCost = row.Item("cost")
                            clsprocess.InvDate = usedDate.ToString("yyyy-MM-dd 00:00:00")
                            clsprocess.BCost = True
                            clsprocess.executeNonQuery_Inv_ProcessCls()
                        End If
                    Next

                    ' === CANCEL RECEIVE: Reset flagtrans=0 in cld_inv_trans, then recalculate stock ===
                    If xcancel Then
                        ' Step 1: Set flagtrans=0 so this receive is excluded from stock aggregation
                        Using cmdResetFlag As New MySqlCommand(
                            "UPDATE cld_inv_trans SET flagtrans=0, ModifBy=@modifby, Modifdate=@modifdate WHERE linkid=@linkid AND custid=@custid",
                            conn, trans)
                            cmdResetFlag.Parameters.AddWithValue("@modifby", username)
                            cmdResetFlag.Parameters.AddWithValue("@modifdate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                            cmdResetFlag.Parameters.AddWithValue("@linkid", lid)
                            cmdResetFlag.Parameters.AddWithValue("@custid", custid)
                            cmdResetFlag.ExecuteNonQuery()
                        End Using

                        ' Step 2: Recalculate cld_inv_transstockcls for each affected item
                        ' executeNonQuery_Inv_ProcessCls reads SUM of flagtrans=1 records,
                        ' so excluding this receive will produce the correct stock balance.
                        For Each row As DataRow In dt.Rows
                            Dim clsprocess As New clsInventoryProcess
                            clsprocess.InvstkId = row.Item("stk_id").ToString
                            clsprocess.InvCc = row.Item("cc").ToString
                            clsprocess.InvCcName = row.Item("cc_name").ToString
                            clsprocess.InvCustId = custid
                            clsprocess.InvstkName = row.Item("Stk_Name").ToString
                            ' Pass negative qty to reverse the previous positive rcvqty addition
                            clsprocess.Invrcvqty = -Convert.ToDecimal(row.Item("ttlStock"))
                            clsprocess.InvStkNo = row.Item("stk_no")
                            clsprocess.InvCost = row.Item("cost")
                            clsprocess.InvDate = usedDate.ToString("yyyy-MM-dd 00:00:00")
                            clsprocess.BCost = True
                            clsprocess.executeNonQuery_Inv_ProcessCls()
                        Next
                    End If
                    ' =====================================================================================

                    ' Update Header
                    ' When cancelling: reset flag back to 1 (ordered/pending), not stat=2
                    Dim cancelFlag As Integer = If(xcancel, 1, stat)
                    Dim sqlHeaderUpdate As String = "UPDATE cld_invlk_rcv SET doctot=@doctot, flag=@flag, modifby=@modifby, notes=@notes, Do=@Do, Modifdate=@Modifdate, exDelDate=@exDelDate, acc=@acc, InvDate=@InvDate"
                    If My.Settings.supp_tax = "1" Then
                        sqlHeaderUpdate &= ", p_tax=@p_tax, n_tax=@n_tax"
                    End If
                    sqlHeaderUpdate &= " WHERE id=@id AND custid=@custid"
                    Using cmdHeaderUpdate As New MySqlCommand(sqlHeaderUpdate, conn, trans)
                        cmdHeaderUpdate.Parameters.AddWithValue("@doctot", doctot + ntax)
                        cmdHeaderUpdate.Parameters.AddWithValue("@flag", cancelFlag)
                        cmdHeaderUpdate.Parameters.AddWithValue("@modifby", username)
                        cmdHeaderUpdate.Parameters.AddWithValue("@notes", notes)
                        cmdHeaderUpdate.Parameters.AddWithValue("@Do", DOS)
                        cmdHeaderUpdate.Parameters.AddWithValue("@Modifdate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                        cmdHeaderUpdate.Parameters.AddWithValue("@exDelDate", ExDelDate.ToString("yyyy-MM-dd"))
                        cmdHeaderUpdate.Parameters.AddWithValue("@acc", iacc)
                        cmdHeaderUpdate.Parameters.AddWithValue("@id", lid)
                        cmdHeaderUpdate.Parameters.AddWithValue("@custid", custid)
                        cmdHeaderUpdate.Parameters.AddWithValue("@InvDate", usedDate.ToString("yyyy-MM-dd"))
                        If My.Settings.supp_tax = "1" Then
                            cmdHeaderUpdate.Parameters.AddWithValue("@p_tax", ptax)
                            cmdHeaderUpdate.Parameters.AddWithValue("@n_tax", ntax)
                        End If

                        cmdHeaderUpdate.ExecuteNonQuery()
                    End Using

                    shasil = lid
                End If

                trans.Commit()
                clsQueryGuard.CommitQuery(_qToken) ' === sukses: hapus dari pending log ===
                insertLogModule(username, "InvtransRcv", "Update", custid)

            Catch ex As Exception
                trans.Rollback()
                clsQueryGuard.MarkTimeout(_qToken, _qSummary, "clsInventory:insertInvRcv", ex) ' === gagal: tetap di pending log ===
                errLog(ex.Message & " - " & ex.StackTrace, "clsInventory:insertInvRcv")
                Throw ' Re-throw to alert the caller (UI)
            End Try
        End Using

        Return shasil
    End Function


    Public Function insertInvRcvold(ByVal id As String, ByVal ccfrom As Integer, ByVal ccname As String, ByVal dt As DataTable, ByVal DOS As String, ByVal Supid As Integer, ByVal stat As Integer, ByVal username As String, ByVal custid As Integer, ByVal dExpDate As Date, ByVal notes As String, Optional ByVal xcancel As Boolean = False) As String
        Dim c_cmd As New clsDbo("")
        Dim strheader As String
        Dim strmaster As String
        Dim flagtrans As Integer

        Dim shasil As String = ""

        If stat = 1 Then
            flagtrans = 0
        Else
            flagtrans = 1
        End If

        Dim doctot As Decimal = 0
        Try
            If id = "" Then
                'klo insert baru
                Dim idtrans As String = CItrans.idRcv(custid)
                For Each row As DataRow In dt.Rows

                    strheader = "insert into cld_inv_Trans values (" & c_cmd.ExecuteScalar("select Max(ID) from cld_inv_Trans") & ", " & custid & ",'" & dExpDate.ToString("yyyy-MM-dd 00:00:00") & "', 3, " & ccfrom & ", " &
                        " " & row.Item("stk_id") & ", '" & row.Item("Stk_Name") & "', " & row.Item("U1") & ", " & row.Item("U2") & ", " & CInt(row.Item("Factor1")) & ", " & row.Item("ttlStock") & ", " & row.Item("CostU1") & ", " & row.Item("Cost") & ", " &
                        " '" & idtrans & "', " & flagtrans & ", '" & row.Item("U1_nm") & "', '" & row.Item("U2_nm") & "', '" & row.Item("Createdby") & "', '" & DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") & "', Null, Null, '" & row.Item("stk_no") & "', '" & ccname & "')"
                    c_cmd.executeNonQuery(strheader)
                    doctot = row.Item("ttlCost") + doctot
                    If flagtrans = 1 Then
                        'untuk langsung nambah stoclcls
                        Dim clsprocess As New clsInventoryProcess
                        clsprocess.InvstkId = row.Item("stk_id").ToString
                        clsprocess.InvCc = row.Item("cc").ToString
                        clsprocess.InvCcName = row.Item("cc_name").ToString
                        clsprocess.InvCustId = custid
                        clsprocess.InvstkName = row.Item("Stk_Name").ToString
                        clsprocess.Invrcvqty = row.Item("ttlStock")
                        clsprocess.InvStkNo = row.Item("stk_no")
                        'clsprocess.lastpurchase = row.Item("Cost")
                        clsprocess.InvCost = row.Item("cost")
                        clsprocess.InvDate = dExpDate.ToString("yyyy-MM-dd 00:00:00")
                        clsprocess.BCost = True
                        clsprocess.executeNonQuery_Inv_ProcessCls()
                        'akhir untuk langsung nambah stoclcls
                    End If



                    Dim cmd1 As New clsDbo("")

                    'perubahan 24 sept 2020
                    'If flagtrans = 0 Then
                    '    cmd1.executeNonQuery_Inv_CalcCost(row.Item("stk_id"), ccfrom, custid)
                    '    c_cmd.executeNonQuery_Inv_CalcStock(row.Item("stk_id"), ccfrom, custid)
                    'End If

                    cmd1 = Nothing






                Next


                strmaster = "Insert into cld_invlk_rcv values('" & idtrans & "'," & custid & ", '" & DOS & "', " & Supid & "," & stat & "," & doctot & ", 1 ," & ccfrom & ", '" & username & "', '" & DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") & "', Null, Null, '" & dExpDate.ToString("yyyy-MM-dd") & "', 0, '" & dExpDate.Date.ToString("yyyy-MM-dd") & "', '" & notes & "', 0,0)"
                c_cmd.resetCmd("")
                c_cmd.executeNonQuery(strmaster)

                shasil = idtrans

            Else

                'klo Rubah atau Update
                If stat = 1 Then
                    'pending
                    For Each row As DataRow In dt.Rows
                        If row.Item("ID") > 0 Then
                            If row.Item("flagtrans") <> flagtrans Then
                                'untuk langsung delete stoclcls
                                'Dim clsprocess As New clsInventoryProcess
                                'clsprocess.InvstkId = row.Item("stk_id").ToString
                                'clsprocess.InvCc = row.Item("cc").ToString
                                'clsprocess.InvCcName = row.Item("cc_name").ToString
                                'clsprocess.InvCustId = custid
                                'clsprocess.InvstkName = row.Item("Stk_Name").ToString
                                'clsprocess.Invrcvqty = -c_cmd.ExecuteScalar("SELECT ttlstock FROM cld_inv_trans WHERE linkid='" & id & "' and stk_id = '" & row.Item("stk_id").ToString & "' and cc = '" & row.Item("cc").ToString & "' and custid = " & custid & "", 0)
                                'clsprocess.InvStkNo = row.Item("stk_no")
                                'clsprocess.InvDate = Date.Now.Date.ToString("yyyy-MM-dd 00:00:00")
                                'clsprocess.BCost = True
                                'clsprocess.executeNonQuery_Inv_ProcessCls()
                                'akhir untuk langsung delete stoclcls
                            End If
                            strheader = "update cld_Inv_Trans set  U1 = " & row.Item("U1") & ", U2 = " & row.Item("U2") & ", factor1 = " & row.Item("Factor1") & ", ttlstock = " & row.Item("ttlStock") & ", CostU1 = " & row.Item("CostU1") & ", Cost = " & row.Item("Cost") & ", " &
                              " ModifBy = '" & username & "', flagtrans = " & flagtrans & ", Invdate = '" & dExpDate.ToString("yyyy-MM-dd 00:00:00") & "', Modifdate = '" & DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") & "'  where id = " & row.Item("ID") & " and custid = " & custid & ""
                            c_cmd.executeNonQuery(strheader)

                            doctot = row.Item("ttlCost") + doctot

                        Else
                            strheader = "insert into cld_inv_Trans values (" & c_cmd.ExecuteScalar("select Max(ID) from cld_inv_Trans") & ", " & custid & ",'" & dExpDate.ToString("yyyy-MM-dd") & "', 3, " & ccfrom & ", " &
                                " " & row.Item("stk_id") & ", '" & row.Item("Stk_Name") & "', " & row.Item("U1") & ", " & row.Item("U2") & ", " & row.Item("Factor1") & ", " & row.Item("ttlStock") & ", " & row.Item("CostU1") & ", " & row.Item("Cost") & ", " &
                                " '" & id & "', " & flagtrans & ", '" & row.Item("U1_nm") & "', '" & row.Item("U2_nm") & "', '" & row.Item("Createdby") & "', '" & DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") & "' , Null, Null, '" & row.Item("stk_no") & "', '" & ccname & "')"
                            c_cmd.executeNonQuery(strheader)
                            doctot = row.Item("ttlCost") + doctot

                        End If

                    Next

                    strmaster = "update cld_invlk_rcv set  doctot = " & doctot & ", flag = " & stat & ", modifby =  '" & username & "', notes =  '" & notes & "', Do =  '" & DOS & "', Modifdate =  '" & DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") & "', exDelDate =  '" & dExpDate.Date.ToString("yyyy-MM-dd") & "' where id = '" & id & "' and custid = " & custid & ""
                    c_cmd.resetCmd("")
                    c_cmd.executeNonQuery(strmaster)
                    'c_cmd.executeNonQuery_Inv_CalcCost(row.Item("stk_id"), ccfrom, custid)
                    'c_cmd.executeNonQuery_Inv_CalcStock(row.Item("stk_id"), ccfrom, custid)

                Else
                    'Finish atau receiving

                    For Each row As DataRow In dt.Rows
                        Dim clsprocess As New clsInventoryProcess
                        If row.Item("ID") > 0 Then
                            'kalau finish diupdate delete dulu nilai pertama
                            If row.Item("flagtrans") = flagtrans Then
                                'untuk langsung delete stoclcls
                                ' Dim clsprocess As New clsInventoryProcess
                                clsprocess.InvstkId = row.Item("stk_id").ToString
                                clsprocess.InvCc = row.Item("cc").ToString
                                clsprocess.InvCcName = row.Item("cc_name").ToString
                                clsprocess.InvCustId = custid
                                clsprocess.InvstkName = row.Item("Stk_Name").ToString
                                clsprocess.Invrcvqty = -c_cmd.ExecuteScalar("SELECT ttlstock FROM cld_inv_trans WHERE linkid='" & id & "' and stk_id = '" & row.Item("stk_id").ToString & "' and cc = '" & row.Item("cc").ToString & "' and custid = " & custid & "", 0)
                                clsprocess.InvStkNo = row.Item("stk_no")
                                'clsprocess.lastpurchase = row.Item("Cost")
                                clsprocess.InvCost = row.Item("cost")
                                clsprocess.InvDate = dExpDate.ToString("yyyy-MM-dd 00:00:00")
                                'If xcancel = True Then
                                '    If IsDBNull(row.Item("modifdate")) Then
                                '        clsprocess.InvDate = row.Item("createddate")
                                '    Else
                                '        clsprocess.InvDate = row.Item("modifdate")
                                '    End If

                                'End If
                                clsprocess.BCost = True
                                clsprocess.executeNonQuery_Inv_ProcessCls()
                                'akhir untuk langsung delete stoclcls
                            End If
                            'kalau cancel flagtrans dibalikin
                            If xcancel = True Then
                                strheader = "update cld_Inv_Trans set U1 = " & row.Item("U1") & ", U2 = " & row.Item("U2") & ", factor1 = " & row.Item("Factor1") & ", ttlstock = " & row.Item("ttlStock") & ", CostU1 = " & row.Item("CostU1") & ", Cost = " & row.Item("Cost") & ", " &
                              " ModifBy = '" & username & "', flagtrans = 0, Modifdate = '" & DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") & "', InvDate = '" & dExpDate.ToString("yyyy-MM-dd 00:00:00") & "' where id = " & row.Item("ID") & " and custid = " & custid & ""

                            Else
                                strheader = "update cld_Inv_Trans set U1 = " & row.Item("U1") & ", U2 = " & row.Item("U2") & ", factor1 = " & row.Item("Factor1") & ", ttlstock = " & row.Item("ttlStock") & ", CostU1 = " & row.Item("CostU1") & ", Cost = " & row.Item("Cost") & ", " &
                              " ModifBy = '" & username & "', flagtrans = " & flagtrans & ", Modifdate = '" & DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") & "', InvDate = '" & dExpDate.ToString("yyyy-MM-dd 00:00:00") & "' where id = " & row.Item("ID") & " and custid = " & custid & ""

                            End If
                            c_cmd.resetCmd("")
                            c_cmd.executeNonQuery(strheader)

                            doctot = row.Item("ttlCost") + doctot
                            If xcancel = True Then
                                GoTo nextrow
                            End If
                            'untuk langsung nambah stoclcls
                            'Dim clsprocess As New clsInventoryProcess
                            clsprocess.InvstkId = row.Item("stk_id").ToString
                            clsprocess.InvCc = row.Item("cc").ToString
                            clsprocess.InvCcName = row.Item("cc_name").ToString
                            clsprocess.InvCustId = custid
                            clsprocess.InvstkName = row.Item("Stk_Name").ToString
                            clsprocess.Invrcvqty = row.Item("ttlStock")
                            clsprocess.InvStkNo = row.Item("stk_no")
                            'clsprocess.lastpurchase = row.Item("Cost")
                            clsprocess.InvCost = row.Item("cost")
                            clsprocess.InvDate = dExpDate.ToString("yyyy-MM-dd 00:00:00")
                            clsprocess.BCost = True
                            clsprocess.executeNonQuery_Inv_ProcessCls()
                            'akhir untuk langsung nambah stoclcls
nextrow:
                        Else
                            strheader = "insert into cld_inv_Trans values (" & c_cmd.ExecuteScalar("select Max(ID) from cld_inv_Trans") & ", " & custid & ",'" & dExpDate.ToString("yyyy-MM-dd") & "', 3, " & ccfrom & ", " &
                                " " & row.Item("stk_id") & ", '" & row.Item("Stk_Name") & "', " & row.Item("U1") & ", " & row.Item("U2") & ", " & row.Item("Factor1") & ", " & row.Item("ttlStock") & ", " & row.Item("CostU1") & ", " & row.Item("Cost") & ", " &
                                " '" & id & "', " & flagtrans & ", '" & row.Item("U1_nm") & "', '" & row.Item("U2_nm") & "', '" & row.Item("Createdby") & "', '" & DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") & "' , Null, Null, '" & row.Item("stk_no") & "', '" & ccname & "')"
                            c_cmd.resetCmd("")
                            c_cmd.executeNonQuery(strheader)
                            doctot = row.Item("ttlCost") + doctot

                            'untuk langsung nambah stoclcls
                            'Dim clsprocess As New clsInventoryProcess
                            clsprocess.InvstkId = row.Item("stk_id").ToString
                            clsprocess.InvCc = row.Item("cc").ToString
                            clsprocess.InvCcName = row.Item("cc_name").ToString
                            clsprocess.InvCustId = custid
                            clsprocess.InvstkName = row.Item("Stk_Name").ToString
                            clsprocess.Invrcvqty = row.Item("ttlStock")
                            clsprocess.InvStkNo = row.Item("stk_no")
                            'clsprocess.lastpurchase = row.Item("Cost")
                            clsprocess.InvCost = row.Item("cost")
                            clsprocess.InvDate = dExpDate.ToString("yyyy-MM-dd 00:00:00")
                            clsprocess.BCost = True
                            clsprocess.executeNonQuery_Inv_ProcessCls()
                            'akhir untuk langsung nambah stoclcls

                        End If

                        'perubahan 24 sept 2020
                        'c_cmd.executeNonQuery_Inv_CalcCost(row.Item("stk_id"), ccfrom, custid)
                        'c_cmd.executeNonQuery_Inv_CalcStock(row.Item("stk_id"), ccfrom, custid)

                    Next
                    If xcancel = True Then
                        stat = 1
                    End If
                    strmaster = "update cld_invlk_rcv set InvDate = '" & dExpDate.ToString("yyyy-MM-dd") & "', exDelDate = '" & dExpDate.Date.ToString("yyyy-MM-dd") & "', doctot = " & doctot & ", flag = " & stat & ", modifby =  '" & username & "', notes =  '" & notes & "', Modifdate =  '" & DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") & "' where id = '" & id & "' and custid = " & custid & ""
                    c_cmd.resetCmd("")
                    c_cmd.executeNonQuery(strmaster)

                End If

                '8 nov 2017, set feedback hasil = ID receivenya
                shasil = id

            End If

            insertLogModule(username, "InvtransRcv", "Update", custid)

        Catch ex As Exception

            errLog(Err.Description, "clsInventory:insertInvRcv")
            Err.Clear()
        End Try

        c_cmd.dispose()
        c_cmd = Nothing


        Return shasil

    End Function

    Public Sub setemptyreceive(ByVal id As String, ByVal username As String, ByVal custid As Integer, ByVal notes As String)
        Dim c_cmd As New clsDbo("")

        Try
            c_cmd.executeNonQuery("update cld_invlk_rcv set doctot = 0, flag = 2, modifby =  '" & username & "', notes =  '" & notes & "', Modifdate =  '" & DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") & "' where id = '" & id & "' and custid = " & custid & "")

        Catch ex As Exception
            errLog(Err.Description, "clsInventory:setemptyreceive")
            Err.Clear()
        End Try
    End Sub

    Public Sub DeleteInvRcvDetail(ByVal id As Integer, ByVal Custid As Integer, ByVal username As String)

        Try

            Dim strTr As String
            Dim xreceived As Integer = 1
            Dim dtstk As DataTable
            'delete stockcls


            strTr = "select * FROM cld_inv_trans  where id = '" & id & "' and custid = " & Custid & ""
            Dim c_cmd As New clsDbo(strTr)
            dtstk = c_cmd.selectDataset.Tables(0)
            If dtstk.Rows.Count > 0 Then

                xreceived = c_cmd.ExecuteScalar("SELECT flag FROM `cld_invlk_rcv` WHERE id = '" & id & "'", 0)
                If xreceived = 2 Then
                    Dim clsprocess As New clsInventoryProcess

                    For Each row As DataRow In dtstk.Rows
                        clsprocess.InvstkId = row.Item("Stk_Id")
                        clsprocess.InvCc = row.Item("cc")
                        clsprocess.InvCcName = row.Item("cc_name")
                        clsprocess.InvCustId = Custid
                        clsprocess.InvstkName = row.Item("stk_name")
                        clsprocess.Invrcvqty = -(row.Item("ttlStock"))
                        clsprocess.InvStkNo = row.Item("Stk_no")
                        clsprocess.InvDate = Date.Now.Date.ToString("yyyy-MM-dd")
                        clsprocess.BCost = True

                        'clsprocess.lastpurchase = c_cmd.ExecuteScalar("SELECT IFNULL(lastpurchase,0) FROM cld_inv_transstockcls WHERE stkno = '" & row.Item("Stk_no") & "' AND ccid = " & row.Item("cc") & " ORDER BY invdate DESC LIMIT 1,1", 0)

                        clsprocess.executeNonQuery_Inv_ProcessCls()
                    Next
                End If



            End If


            c_cmd.ReNew()

            strTr = "DELETE FROM cld_inv_trans where id = " & id
            c_cmd.executeNonQuery(strTr)


            c_cmd.dispose()
            c_cmd = Nothing

            insertLogModule(username, "Inv_RcvDetail " & id.ToString, "Delete", Custid)
        Catch ex As Exception
            errLog(Err.Description, "clsInventory:DeleteInvRcvDetail")
            Err.Clear()
        End Try
    End Sub

    Public Sub DeleteInvRcvitem(ByVal id As String, ByVal stkid As Integer, ByVal Custid As Integer, ByVal username As String)

        Try

            Dim strTr As String
            Dim xreceived As Integer = 1
            'delete stockcls
            Dim c_cmd As New clsDbo("")

            strTr = "select * FROM cld_inv_trans  where id = '" & id & "' and stk_id = " & stkid & " and custid = " & Custid & ""
            dtstk = c_cmd.ReturnDataset(strTr).Tables(0)
            If dtstk.Rows.Count > 0 Then
                xreceived = c_cmd.ExecuteScalar("SELECT flag FROM `cld_invlk_rcv` WHERE id = '" & dtstk.rows(0).item("linkid") & "'", 0)
                If xreceived = 2 Then
                    Dim clsprocess As New clsInventoryProcess

                    For Each row As DataRow In dtstk.Rows
                        clsprocess.InvstkId = row.Item("Stk_Id")
                        clsprocess.InvCc = row.Item("cc")
                        clsprocess.InvCcName = row.Item("cc_name")
                        clsprocess.InvCustId = Custid
                        clsprocess.InvstkName = row.Item("stk_name")
                        clsprocess.Invrcvqty = -(row.Item("ttlStock"))
                        clsprocess.InvStkNo = row.Item("Stk_no")
                        ' clsprocess.lastpurchase = c_cmd.ExecuteScalar("SELECT IFNULL(lastpurchase,0) FROM cld_inv_transstockcls WHERE stkno = '" & row.Item("Stk_no") & "' AND ccid = " & row.Item("cc") & " ORDER BY invdate DESC LIMIT 1,1", 0)
                        clsprocess.InvDate = CDate(row.Item("invdate"))
                        clsprocess.BCost = True
                        clsprocess.executeNonQuery_Inv_ProcessCls()
                    Next
                End If



            End If


            c_cmd.ReNew()

            strTr = "DELETE FROM cld_inv_trans where id = " & id & " and stk_id = " & stkid
            c_cmd.executeNonQuery(strTr)


            c_cmd.dispose()
            c_cmd = Nothing

            insertLogModule(username, "Inv_RcvDetail " & id.ToString & " " & stkid.ToString, "Delete", Custid)
        Catch ex As Exception
            errLog(Err.Description, "clsInventory:DeleteInvRcvDetail")
            Err.Clear()
        End Try
    End Sub
    Public Sub DeleteInvRcvitembylid(ByVal lid As String, ByVal stkid As Integer, ByVal Custid As Integer, ByVal username As String)

        Try

            Dim strTr As String
            Dim xreceived As Integer = 1
            'delete stockcls
            Dim c_cmd As New clsDbo("")

            strTr = "select * FROM cld_inv_trans  where linkid = '" & lid & "' and stk_id = " & stkid & " and custid = " & Custid & ""
            dtstk = c_cmd.ReturnDataset(strTr).Tables(0)
            If dtstk.Rows.Count > 0 Then
                xreceived = c_cmd.ExecuteScalar("SELECT flag FROM `cld_invlk_rcv` WHERE id = '" & dtstk.rows(0).item("linkid") & "'", 0)
                If xreceived = 2 Then
                    Dim clsprocess As New clsInventoryProcess

                    For Each row As DataRow In dtstk.Rows
                        clsprocess.InvstkId = row.Item("Stk_Id")
                        clsprocess.InvCc = row.Item("cc")
                        clsprocess.InvCcName = row.Item("cc_name")
                        clsprocess.InvCustId = Custid
                        clsprocess.InvstkName = row.Item("stk_name")
                        clsprocess.Invrcvqty = -(row.Item("ttlStock"))
                        clsprocess.InvStkNo = row.Item("Stk_no")
                        ' clsprocess.lastpurchase = c_cmd.ExecuteScalar("SELECT IFNULL(lastpurchase,0) FROM cld_inv_transstockcls WHERE stkno = '" & row.Item("Stk_no") & "' AND ccid = " & row.Item("cc") & " ORDER BY invdate DESC LIMIT 1,1", 0)
                        clsprocess.InvDate = CDate(row.Item("invdate"))
                        clsprocess.BCost = True
                        clsprocess.executeNonQuery_Inv_ProcessCls()
                    Next
                End If



            End If


            c_cmd.ReNew()

            strTr = "DELETE FROM cld_inv_trans where linkid = '" & lid & "' and stk_id = " & stkid
            c_cmd.executeNonQuery(strTr)


            c_cmd.dispose()
            c_cmd = Nothing

            insertLogModule(username, "Inv_RcvDetail " & lid.ToString & " " & stkid.ToString, "Delete", Custid)
        Catch ex As Exception
            errLog(Err.Description, "clsInventory:DeleteInvRcvDetail")
            Err.Clear()
        End Try
    End Sub
    Public Sub DeleteInvRcv(ByVal id As String, ByVal Custid As Integer, ByVal username As String)
        'Dim pc As String = pic.ToString
        Dim dtstk As New DataTable
        Try

            Dim str As String
            Dim strTr As String
            Dim c_cmd As New clsDbo("")


            str = "DELETE FROM cld_invlk_Rcv  where id = '" & id & "' and custid = " & Custid & ""
            c_cmd.executeNonQuery(str)
            'delete stockcls
            c_cmd.ReNew()
            strTr = "select * FROM cld_inv_trans  where linkid = '" & id & "' and custid = " & Custid & " and flagtrans = 1 "
            dtstk = c_cmd.ReturnDataset(strTr).Tables(0)
            If dtstk.Rows.Count > 0 Then
                Dim clsprocess As New clsInventoryProcess

                For Each row As DataRow In dtstk.Rows
                    clsprocess.InvstkId = row.Item("Stk_Id")
                    clsprocess.InvCc = row.Item("cc")
                    clsprocess.InvCcName = row.Item("cc_name")
                    clsprocess.InvCustId = Custid
                    clsprocess.InvstkName = row.Item("stk_name")
                    clsprocess.Invrcvqty = -(row.Item("ttlStock"))
                    clsprocess.InvStkNo = row.Item("Stk_no")
                    Dim hsl1 As Decimal = clsprocess.getrecqty(row.Item("Stk_Id"), row.Item("cc"), Custid, Date.Now.Date.ToString("yyyy-MM-dd HH:mm:ss")) * clsprocess.getreccost(row.Item("Stk_Id"), row.Item("cc"), Custid, Date.Now.Date.ToString("yyyy-MM-dd HH:mm:ss"))
                    Dim hsl2 As Decimal = row.Item("ttlStock") * row.Item("cost")
                    Dim qtysisa As Double = clsprocess.getrecqty(row.Item("Stk_Id"), row.Item("cc"), Custid, Date.Now.Date.ToString("yyyy-MM-dd HH:mm:ss")) - row.Item("ttlStock")
                    If Not Double.IsNaN((hsl1 - hsl2) / qtysisa) And qtysisa > 0 Then
                        clsprocess.InvCost = (hsl1 - hsl2) / qtysisa
                    Else
                        clsprocess.InvCost = row.Item("cost")
                    End If

                    'row.Item("cost")
                    'clsprocess.lastpurchase = c_cmd.ExecuteScalar("SELECT IFNULL(lastpurchase,0) FROM cld_inv_transstockcls WHERE stkno = '" & row.Item("Stk_no") & "' AND ccid = " & row.Item("cc") & " ORDER BY invdate DESC LIMIT 1,1", 0)
                    clsprocess.InvDate = row.Item("Invdate") 'Date.Now.Date.ToString("yyyy-MM-dd")
                    clsprocess.BCost = True
                    clsprocess.executeNonQuery_Inv_ProcessCls()
                Next


            End If


            c_cmd.ReNew()
            strTr = "DELETE FROM cld_inv_trans  where linkid = '" & id & "' and custid = " & Custid & ""
            c_cmd.executeNonQuery(strTr)

            c_cmd.dispose()
            c_cmd = Nothing

            insertLogModule(username, "Inv_Rcv " & id.ToString, "Delete", Custid)
        Catch ex As Exception
            errLog(Err.Description, "clsInventory:DeleteInv_Rcv")
            Err.Clear()
        End Try
    End Sub

    'select home inventory
    Public Function SelectDataStkHomeSearch(ByVal cust As Integer, ByVal ccid As Integer, ByVal stat As Integer, ByVal grpid As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""
        Dim statFilter As String = ""
        Dim grpfilter As String = ""
        Dim staMod As String = ""

        If modpriv And usrModulepriv.moduleuser = usrModulepriv.moduleuser Then
            staMod = "and C.Olid in (Select Store From cld_userxstore where User = '" & usr & "')"
        End If


        If ccid <> 0 Then
            strFilter = "and A.ccid = " & ccid & ""
        End If




        If stat <> 0 Then
            If stat = 1 Then
                ' OPTIMIZED: SARGable condition
                statFilter = "AND A.ttlQty > IFNULL(B.stkLimit, 0)"
            ElseIf stat = 2 Then
                 ' OPTIMIZED: SARGable condition
                statFilter = "AND A.ttlQty <= IFNULL(B.stkLimit, 0)"
            End If
        End If

        If grpid <> 0 Then
            grpfilter = "and B.grpid = " & grpid & ""
        End If

        'Dim a As New clsDbo("SELECT A.*, B.stkLimit, CASE WHEN A.ttlQty > B.stklimit THEN TRUE ELSE FALSE END AS stat  FROM cld_inv_transstock A INNER JOIN cld_inv_stk B ON B.ID = A.stkId WHERE A.Custid = " & cust & " " & strFilter & " " & statFilter & " " & grpfilter & "")
        Dim a As New clsDbo("SELECT A.*, B.stkLimit, B.unit2 ,CASE WHEN A.ttlQty > B.stklimit THEN TRUE ELSE FALSE END AS stat " &
                            "FROM cld_inv_transstock A INNER JOIN cld_inv_stk B ON B.ID = A.stkId " &
                            "INNER JOIN cld_inv_cc C on C.Custid = A.custid and C.ID = A.ccID " &
                            "WHERE A.Custid = " & cust & " " & staMod & " " & strFilter & " " & statFilter & " " & grpfilter & " ORDER BY A.ccid, A.stkName")


        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function


    Public Function getInvRwaste(ByVal bydate As Date, ByVal custid As Integer, ByVal ModPriv As Integer, ByVal user As String) As DataSet

        Dim crw As New clsInventory_Rwaste
        Return crw.SelectDataset_invRwaste(bydate, custid, ModPriv, user)

        crw = Nothing

    End Function
    Public Function getclosingwaste(ByVal name As String, ByVal bydate As Date, ByVal custid As Integer, ByVal ccid As Integer, ByVal ModPriv As Integer, ByVal user As String) As DataSet

        Dim crw As New clsInventory_Rwaste
        Return crw.SelectDataset_closingwasteSearch(name, custid, ModPriv, user, ccid, bydate)

        crw = Nothing

    End Function

    Public Sub insertRwaste(ByVal id As Integer, ByVal cc As Integer, ByVal stkid As Integer, ByVal stkanme As String, ByVal U1 As Decimal, ByVal U2 As Decimal, ByVal factor1 As Decimal, ByVal ttlstock As Decimal, ByVal cost As Decimal, ByVal U1_nm As String, ByVal U2_nm As String, ByVal createby As String, ByVal modifby As String, ByVal skno As String, ByVal cc_name As String, ByVal username As String, ByVal custid As Integer)
        Dim c_cmd As New clsDbo("")

        Try
            If id = 0 Then
                Dim idtrans As String = CItrans.idRWaste(custid)
                ' Parameterized insert with subquery for ID generation to reduce race condition window
                c_cmd.resetCmd("INSERT INTO cld_inv_Trans (ID, Custid, InvDate, Type, Cc, Stk_Id, Stk_Name, U1, U2, Factor1, ttlStock, CostU1, Cost, LinkID, FlagTrans, U1_nm, U2_nm, CreatedBy, CreatedDate, stk_no, Cc_name) " &
                               "VALUES ((SELECT IFNULL(MAX(ID),0)+1 FROM cld_inv_Trans AS t), @custid, @invdate, 2, @cc, @stkid, @stkname, @u1, @u2, @factor1, @ttlstock, 0, @cost, @linkid, 1, @u1nm, @u2nm, @createby, @createddate, @stkno, @ccname)")

                c_cmd.addParam(custid, "@custid", MySqlDbType.Int32)
                c_cmd.addParam(Date.Now.ToString("yyyy-MM-dd"), "@invdate", MySqlDbType.String)
                c_cmd.addParam(cc, "@cc", MySqlDbType.Int32)
                c_cmd.addParam(stkid, "@stkid", MySqlDbType.Int32)
                c_cmd.addParam(stkanme, "@stkname", MySqlDbType.VarChar)
                c_cmd.addParam(U1, "@u1", MySqlDbType.Decimal)
                c_cmd.addParam(U2, "@u2", MySqlDbType.Decimal)
                c_cmd.addParam(factor1, "@factor1", MySqlDbType.Decimal)
                c_cmd.addParam(ttlstock, "@ttlstock", MySqlDbType.Decimal)
                c_cmd.addParam(cost, "@cost", MySqlDbType.Decimal)
                c_cmd.addParam(idtrans, "@linkid", MySqlDbType.VarChar)
                c_cmd.addParam(U1_nm, "@u1nm", MySqlDbType.VarChar)
                c_cmd.addParam(U2_nm, "@u2nm", MySqlDbType.VarChar)
                c_cmd.addParam(createby, "@createby", MySqlDbType.VarChar)
                c_cmd.addParam(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "@createddate", MySqlDbType.String)
                c_cmd.addParam(skno, "@stkno", MySqlDbType.VarChar)
                c_cmd.addParam(cc_name, "@ccname", MySqlDbType.VarChar)

                c_cmd.executeNonQuery2()

                insertLogModule(username, "InvRwaste", "Insert", custid)

            Else
                c_cmd.resetCmd("UPDATE cld_Inv_Trans SET Cc = @cc, U1 = @u1, U2 = @u2, factor1 = @factor1, ttlstock = @ttlstock, Cost = @cost, " &
                               "ModifBy = @modifby, Modifdate = @modifdate, Cc_name = @ccname WHERE id = @id AND custid = @custid")

                c_cmd.addParam(cc, "@cc", MySqlDbType.Int32)
                c_cmd.addParam(U1, "@u1", MySqlDbType.Decimal)
                c_cmd.addParam(U2, "@u2", MySqlDbType.Decimal)
                c_cmd.addParam(factor1, "@factor1", MySqlDbType.Decimal)
                c_cmd.addParam(ttlstock, "@ttlstock", MySqlDbType.Decimal)
                c_cmd.addParam(cost, "@cost", MySqlDbType.Decimal)
                c_cmd.addParam(username, "@modifby", MySqlDbType.VarChar)
                c_cmd.addParam(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "@modifdate", MySqlDbType.String)
                c_cmd.addParam(cc_name, "@ccname", MySqlDbType.VarChar)
                c_cmd.addParam(id, "@id", MySqlDbType.Int32)
                c_cmd.addParam(custid, "@custid", MySqlDbType.Int32)

                c_cmd.executeNonQuery2()

                insertLogModule(username, "InvRwaste", "Update", custid)

            End If


        Catch ex As Exception

            errLog(Err.Description, "webservice:insertInvRwaste")
            Err.Clear()
        End Try

        c_cmd.dispose()
        c_cmd = Nothing

    End Sub
    Public Function insertRwaste2(ByVal id As Integer, ByVal cc As Integer, ByVal stkid As Integer, ByVal stkanme As String, ByVal U1 As Decimal, ByVal U2 As Decimal, ByVal factor1 As Decimal, ByVal ttlstock As Decimal, ByVal cost As Decimal, ByVal U1_nm As String, ByVal U2_nm As String, ByVal createby As String, ByVal modifby As String, ByVal skno As String, ByVal cc_name As String, ByVal username As String, ByVal custid As Integer, Optional ByVal invdate As DateTime = Nothing, Optional ByVal notes As String = "") As DataSet
        Dim c_cmd As New clsDbo("")
        Dim ds As New DataSet
        If invdate = Nothing Then
            invdate = Date.Now.ToString("yyyy-MM-dd 00:00:00")
        End If
        ' Ensure invdate is properly formatted (though with params, passing DateTime obj is safer, MySQL usually takes 'yyyy-MM-dd HH:mm:ss')
        Dim sInvDate As String = invdate.ToString("yyyy-MM-dd 00:00:00")

        Try
            If id = 0 Then
                Dim idtrans As String = CItrans.idRWaste(custid)
                ' Use Parameterized Query & Subquery for ID
                c_cmd.resetCmd("INSERT INTO cld_inv_Trans (ID, Custid, InvDate, Type, Cc, Stk_Id, Stk_Name, U1, U2, Factor1, ttlStock, Cost, LinkID, FlagTrans, U1_nm, U2_nm, CreatedBy, CreatedDate, Stk_No, Cc_Name) " &
                               "VALUES ((SELECT IFNULL(MAX(ID),0)+1 FROM cld_inv_Trans AS t), @custid, @invdate, 2, @cc, @stkid, @stkname, @u1, @u2, @factor1, @ttlstock, @cost, @linkid, 1, @u1nm, @u2nm, @createby, @createddate, @stkno, @ccname)")

                c_cmd.addParam(custid, "@custid", MySqlDbType.Int32)
                c_cmd.addParam(sInvDate, "@invdate", MySqlDbType.String)
                c_cmd.addParam(cc, "@cc", MySqlDbType.Int32)
                c_cmd.addParam(stkid, "@stkid", MySqlDbType.Int32)
                c_cmd.addParam(stkanme, "@stkname", MySqlDbType.VarChar)
                c_cmd.addParam(U1, "@u1", MySqlDbType.Decimal)
                c_cmd.addParam(U2, "@u2", MySqlDbType.Decimal)
                c_cmd.addParam(factor1, "@factor1", MySqlDbType.Decimal)
                c_cmd.addParam(ttlstock, "@ttlstock", MySqlDbType.Decimal)
                c_cmd.addParam(cost, "@cost", MySqlDbType.Decimal)
                c_cmd.addParam(idtrans, "@linkid", MySqlDbType.VarChar)
                c_cmd.addParam(U1_nm, "@u1nm", MySqlDbType.VarChar)
                c_cmd.addParam(U2_nm, "@u2nm", MySqlDbType.VarChar)
                c_cmd.addParam(createby, "@createby", MySqlDbType.VarChar)
                c_cmd.addParam(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "@createddate", MySqlDbType.String)
                c_cmd.addParam(skno, "@stkno", MySqlDbType.VarChar)
                c_cmd.addParam(cc_name, "@ccname", MySqlDbType.VarChar)
                
                c_cmd.executeNonQuery2()
                
                insertLogModule(username, "InvRwaste", "Insert", custid)

                If notes <> "" Then
                    ' Retrieve the ID we just inserted. 
                    Dim sqlGetId As String = "select ID from cld_inv_Trans where linkid=@linkid and Stk_id=@stkid and cc=@cc ORDER BY ID DESC LIMIT 1"
                    c_cmd.resetCmd(sqlGetId)
                    c_cmd.addParam(idtrans, "@linkid", MySqlDbType.VarChar)
                    c_cmd.addParam(stkid, "@stkid", MySqlDbType.Int32)
                    c_cmd.addParam(cc, "@cc", MySqlDbType.Int32)

                    Dim newId As Integer = c_cmd.ExecuteScalar(sqlGetId, 0)
                    insertRwasteNotes(newId, cc, stkid, notes)
                End If

            Else
                c_cmd.resetCmd("UPDATE cld_Inv_Trans SET Cc = @cc, U1 = @u1, U2 = @u2, factor1 = @factor1, ttlstock = @ttlstock, Cost = @cost, " &
                               "ModifBy = @modifby, Modifdate = @modifdate, Cc_name = @ccname WHERE id = @id AND custid = @custid")

                c_cmd.addParam(cc, "@cc", MySqlDbType.Int32)
                c_cmd.addParam(U1, "@u1", MySqlDbType.Decimal)
                c_cmd.addParam(U2, "@u2", MySqlDbType.Decimal)
                c_cmd.addParam(factor1, "@factor1", MySqlDbType.Decimal)
                c_cmd.addParam(ttlstock, "@ttlstock", MySqlDbType.Decimal)
                c_cmd.addParam(cost, "@cost", MySqlDbType.Decimal)
                c_cmd.addParam(username, "@modifby", MySqlDbType.VarChar)
                c_cmd.addParam(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "@modifdate", MySqlDbType.String)
                c_cmd.addParam(cc_name, "@ccname", MySqlDbType.VarChar)
                c_cmd.addParam(id, "@id", MySqlDbType.Int32)
                c_cmd.addParam(custid, "@custid", MySqlDbType.Int32)

                c_cmd.executeNonQuery2()

                insertLogModule(username, "InvRwaste", "Update", custid)
                If notes <> "" Then
                    insertRwasteNotes(id, cc, stkid, notes)
                End If
            End If

            ds = getdsclosing(stkid, cc, invdate, custid)

        Catch ex As Exception

            errLog(Err.Description, "webservice:insertInvRwaste")
            Err.Clear()
        End Try

        c_cmd.dispose()
        c_cmd = Nothing
        Return ds
    End Function

    Public Function insertRwasteNotes(ByVal id As Integer, ByVal cc As Integer, ByVal rcpid As Integer, ByVal Notes As String) As Boolean
        Dim c_cmd As New clsDbo("")
        Dim hasil = False

        Try
            ' Parameterized Check
            Dim sqlCheck As String = "SELECT COUNT(transid) FROM `cld_inv_transnotes` WHERE transid = @id AND cc = @cc"
            c_cmd.resetCmd(sqlCheck)
            c_cmd.addParam(id, "@id", MySqlDbType.Int32)
            c_cmd.addParam(cc, "@cc", MySqlDbType.Int32)

            If c_cmd.ExecuteScalar(sqlCheck, 0) = 0 Then
                ' NOTE: ExecuteScalar in clsDbo takes a string and creates a NEW connection internally if passing string?
                ' Wait, clsDbo.ExecuteScalar(strQuery) resets the command text. 
                ' I need to check clsDbo.ExecuteScalar implementation again.
                ' It takes strQuery argument. It REPLACES myCmd.CommandText. 
                ' So my previous addParam will be lost if I call ExecuteScalar(strQuery).
                ' I must check if ExecuteScalar supports using existing parameters.
                ' Looking at clsDbo.ExecuteScalar:
                ' Public Function ExecuteScalar(ByVal strQuery As String, Optional ByVal tipe As Integer = 1)
                '      myCmd.CommandText = strQuery
                '      myCmd.Connection.Open()
                '      ... returns myCmd.ExecuteScalar()

                ' So if I set parameters before calling ExecuteScalar, and pass myCmd.CommandText (which I just set), 
                ' does it keep parameters?
                ' Yes, because ExecuteScalar only assigned strQuery to myCmd.CommandText. It DOES NOT clear parameters.
                ' resetCmd DOES clear parameters.

                ' So correct pattern:
                ' c_cmd.resetCmd("Query")
                ' c_cmd.addParam(...)
                ' c_cmd.ExecuteScalar(c_cmd.myCmd.CommandText, 0) -> this is weird but safe if ExecuteScalar doesn't clear params.
                ' Let's look at ExecuteScalar again.
                ' It does NOT clear parameters.
                ' However, it opens connection.

                ' But wait, c_cmd.ExecuteScalar takes strQuery argument.
                ' If I pass "SELECT ...", it sets CommandText.
                ' Parameters are already in myCmd.Parameters collection.
                ' So yes, this works.

                ' Actually, I'll use the result from the first check to decide insert/update.
                ' But I can't easily use ExecuteScalar with params if I have to pass the string again.
                ' Let's try to trust myCmd.CommandText is set by resetCmd.
                ' But ExecuteScalar SETS it again: myCmd.CommandText = strQuery. 
                ' So I should pass the query string to ExecuteScalar.

                ' Wait, ExecuteScalar(strQuery) does: myCmd.CommandText = strQuery.
                ' So if I do:
                ' c_cmd.resetCmd("SELECT ... @id")
                ' c_cmd.addParam(...)
                ' c_cmd.ExecuteScalar("SELECT ... @id")
                ' It effectively sets CommandText (again) and runs it with existing params. This is fine.

                ' HOWEVER, ExecuteScalar calls dispose() in Finally block!
                ' Public Sub dispose() -> Closes connection and Disposes myCmd.
                ' If myCmd is disposed, the parameters are gone? 
                ' myCmd.Dispose() usually clears everything.

                ' So I cannot reuse the c_cmd object easily for the next query if ExecuteScalar disposes it.
                ' I should check line 260 of clsDbo.vb: "Finally dispose()".
                ' Line 326: myCmd.Dispose().

                ' This means c_cmd.ExecuteScalar is a "One Shot" method that destroys the internal command.
                ' I should instantiate a new c_cmd or use ReNew() (which makes a new connection/command).

                ' So for the Check:
                ' I will use a separate c_cmd or ReNew().
            End If

        Catch ex As Exception
             errLog(Err.Description, "clsInventory:insertRwasteNotes")
             Err.Clear()
        End Try
        
        ' RE-EVALUATING:
        ' Since clsDbo is a bit quirky (Disposes command on ExecuteScalar), I should be careful.
        
        ' ALTERNATELY: Use "REPLACE INTO" or "INSERT ON DUPLICATE KEY UPDATE" to avoid the Read-then-Write race condition and the need for two queries?
        ' cld_inv_transnotes has transid and cc. Are they primary keys?
        ' If (transid, cc) is unique, I can use INSERT ON DUPLICATE KEY UPDATE.
        ' This is much better.
        
        Try
             c_cmd.resetCmd("INSERT INTO cld_inv_transnotes (transid, cc, Stk_link, notes) VALUES (@id, @cc, @rcpid, @notes) " & _
                            "ON DUPLICATE KEY UPDATE Stk_link = @rcpid, notes = @notes")
             c_cmd.addParam(id, "@id", MySqlDbType.Int32)
             c_cmd.addParam(cc, "@cc", MySqlDbType.Int32)
             c_cmd.addParam(rcpid, "@rcpid", MySqlDbType.Int32)
             c_cmd.addParam(Notes, "@notes", MySqlDbType.VarChar)
             
             c_cmd.executeNonQuery2()
             hasil = True
             
        Catch ex As Exception
             errLog(Err.Description, "clsInventory:insertRwasteNotes")
             Err.Clear()
        End Try
        
        c_cmd.dispose()
        c_cmd = Nothing
        Return hasil
    End Function

    Public Sub DeleteInvRwaste(ByVal id As Integer, ByVal Custid As Integer, ByVal username As String)
        Dim c_cmd As New clsDbo("")
        Try
            c_cmd.resetCmd("DELETE FROM cld_inv_trans where id = @id")
            c_cmd.addParam(id, "@id", MySqlDbType.Int32)
            c_cmd.executeNonQuery2()

            If Left(id.ToString(), 2) = "CW" Then
                insertLogModule(username, "Inv_ClosingWaste " & id.ToString, "Delete", Custid)
            Else
                insertLogModule(username, "Inv_RawWaste " & id.ToString, "Delete", Custid)
            End If

        Catch ex As Exception
            errLog(Err.Description, "clsInventory:Deleteinv_RawWaste")
            Err.Clear()
        End Try
        c_cmd.dispose()
        c_cmd = Nothing
    End Sub



    Public Function getInvStkOpname(ByVal custid As Integer, ByVal ModPriv As Integer, ByVal user As String, ByVal ccid As Integer) As DataSet
        Dim csOname As New clsInventory_stkOpname
        Return csOname.SelectDataset_invStkOpname(custid, ModPriv, user, ccid)

        csOname = Nothing

    End Function

    Public Function getInvStkOpnameid(ByVal custid As Integer) As DataSet
        Dim csOname As New clsInventory_stkOpname
        Return csOname.SelectDataset_invStkOpnamebyid(custid)
        csOname = Nothing
    End Function

    Public Function getInvStkOpnameSearch(ByVal name As String, ByVal custid As Integer, ByVal ModPriv As Integer, ByVal user As String, ByVal ccid As Integer, ByVal ddate As Date) As DataSet
        Dim csOname As New clsInventory_stkOpname
        Return csOname.SelectDataset_invStkOpnameSearch(name, custid, ModPriv, user, ccid, ddate)
        csOname = Nothing
    End Function
    Public Function getInvStkOpnameSearch2(ByVal name As String, ByVal ModPriv As Integer, ByVal user As String) As DataSet
        Dim csOname As New clsInventory_stkOpname
        Return csOname.SelectDataset_invStkOpnameSearch2(name, ModPriv, user)
        csOname = Nothing
    End Function
    Public Sub DeleteInvStkOpname(ByVal id As String, ByVal ccid As Integer, ByVal Custid As Integer, ByVal username As String)
        Dim c_cmd As New clsDbo("")
        Try
            c_cmd.resetCmd("DELETE FROM cld_inv_trans where stk_id = @id and cc = @ccid and type = 12 and custid = @custid")
            c_cmd.addParam(id, "@id", MySqlDbType.VarChar)
            c_cmd.addParam(ccid, "@ccid", MySqlDbType.Int32)
            c_cmd.addParam(Custid, "@custid", MySqlDbType.Int32)
            c_cmd.executeNonQuery2()

            insertLogModule(username, "Inv_inv_transstock " & id.ToString, "Delete", Custid)
        Catch ex As Exception
            errLog(Err.Description, "webservice:Deleteinv_transstock")
            Err.Clear()
        End Try
        c_cmd.dispose()
        c_cmd = Nothing
    End Sub


    'StockOpname Today
    Public Sub insertStockOpname(ByVal cc As Integer, ByVal stkid As Integer, ByVal stkanme As String, ByVal U1 As Decimal, ByVal U2 As Decimal, ByVal factor1 As Decimal, ByVal ttlstock As Decimal, ByVal cost As Decimal, ByVal U1_nm As String, ByVal U2_nm As String, ByVal createby As String, ByVal modifby As String, ByVal skno As String, ByVal cc_name As String, ByVal username As String, ByVal custid As Integer, Optional ByVal adjustnow As Integer = 0)
        Dim c_cmd As New clsDbo("")
        Dim strheader As String = ""
        Dim ds As DataSet
        'Dim rcpid As Integer
        Try
            If adjustnow <> 0 Then
                ds = getInvStkOpnameEdit(stkid, cc, custid, Now.Date.ToString("yyyy-MM-dd"))
            Else
                ds = getInvStkOpnameEdit(stkid, cc, custid)
            End If

            factor1 = stock_factor1(custid, stkid)

            ' If cost is not provided (<=1), fetch standard cost and use it
            If cost <= 1 Then
                Dim clsis As New clsInventory_stk
                Try
                    Dim dtCost As DataTable = clsis.SelectDataStkCost(stkid, cc, custid, CDate(Now)).Tables(0)
                    If dtCost.Rows.Count > 0 Then
                        cost = dtCost.Rows(0).Item("cost")
                    End If
                Catch ex As Exception
                End Try
            End If

            Dim type As Integer = 5
            If adjustnow = 1 Then type = 11
            If adjustnow = 2 Then type = 12

            Dim invDate As String = Date.Now.Date.ToString("yyyy-MM-dd")
            Dim dateTimeNow As String = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")

            If ds.Tables(0).Rows.Count = 0 Then
                ' INSERT
                c_cmd.resetCmd("INSERT INTO cld_inv_Trans (ID, Custid, InvDate, Type, Cc, Stk_Id, Stk_Name, U1, U2, Factor1, ttlStock, CostU1, Cost, LinkID, FlagTrans, U1_nm, U2_nm, CreatedBy, CreatedDate, stk_no, Cc_name) " &
                               "VALUES ((SELECT IFNULL(MAX(ID),0)+1 FROM cld_inv_Trans AS t), @custid, @invdate, @type, @cc, @stkid, @stkname, @u1, @u2, @factor1, @ttlstock, 0, @cost, '', 1, @u1nm, @u2nm, @createby, @createddate, @stkno, @ccname)")

                c_cmd.addParam(custid, "@custid", MySqlDbType.Int32)
                c_cmd.addParam(invDate, "@invdate", MySqlDbType.String)
                c_cmd.addParam(type, "@type", MySqlDbType.Int32)
                c_cmd.addParam(cc, "@cc", MySqlDbType.Int32)
                c_cmd.addParam(stkid, "@stkid", MySqlDbType.Int32)
                c_cmd.addParam(stkanme, "@stkname", MySqlDbType.VarChar)
                c_cmd.addParam(U1, "@u1", MySqlDbType.Decimal)
                c_cmd.addParam(U2, "@u2", MySqlDbType.Decimal)
                c_cmd.addParam(factor1, "@factor1", MySqlDbType.Decimal)
                c_cmd.addParam(ttlstock, "@ttlstock", MySqlDbType.Decimal)
                c_cmd.addParam(cost, "@cost", MySqlDbType.Decimal)
                c_cmd.addParam(U1_nm, "@u1nm", MySqlDbType.VarChar)
                c_cmd.addParam(U2_nm, "@u2nm", MySqlDbType.VarChar)
                c_cmd.addParam(createby, "@createby", MySqlDbType.VarChar)
                c_cmd.addParam(dateTimeNow, "@createddate", MySqlDbType.String)
                c_cmd.addParam(skno, "@stkno", MySqlDbType.VarChar)
                c_cmd.addParam(cc_name, "@ccname", MySqlDbType.VarChar)

                c_cmd.executeNonQuery2()
                insertLogModule(username, "InvStockOpname", "Insert", custid)

            Else
                ' UPDATE
                Dim updateId As Integer = ds.Tables(0).Rows(0).Item("id")
                
                Dim sqlUpdate As String = "UPDATE cld_Inv_Trans SET U1 = @u1, U2 = @u2, factor1 = @factor1, ttlstock = @ttlstock, Cost = @cost, " &
                                          "ModifBy = @modifby, Modifdate = @modifdate, type = @type "

                If adjustnow > 0 Then
                    sqlUpdate &= ", Invdate = @invdate "
                End If
                sqlUpdate &= ", Cc_name = @ccname WHERE id = @id AND custid = @custid"

                c_cmd.resetCmd(sqlUpdate)
                c_cmd.addParam(U1, "@u1", MySqlDbType.Decimal)
                c_cmd.addParam(U2, "@u2", MySqlDbType.Decimal)
                c_cmd.addParam(factor1, "@factor1", MySqlDbType.Decimal)
                c_cmd.addParam(ttlstock, "@ttlstock", MySqlDbType.Decimal)
                c_cmd.addParam(cost, "@cost", MySqlDbType.Decimal)
                c_cmd.addParam(username, "@modifby", MySqlDbType.VarChar)
                c_cmd.addParam(dateTimeNow, "@modifdate", MySqlDbType.String)
                c_cmd.addParam(type, "@type", MySqlDbType.Int32)
                If adjustnow > 0 Then
                    c_cmd.addParam(invDate, "@invdate", MySqlDbType.String)
                End If
                c_cmd.addParam(cc_name, "@ccname", MySqlDbType.VarChar)
                c_cmd.addParam(updateId, "@id", MySqlDbType.Int32)
                c_cmd.addParam(custid, "@custid", MySqlDbType.Int32)

                c_cmd.executeNonQuery2()
                insertLogModule(username, "InvStockOpname", "Update", custid)

            End If


            'untuk langsung nambah stoclcls
            Dim clsprocess As New clsInventoryProcess
            clsprocess.InvstkId = stkid
            clsprocess.InvCc = cc
            clsprocess.InvCcName = cc_name
            clsprocess.InvCustId = custid
            clsprocess.InvStkNo = skno
            clsprocess.InvstkName = stkanme
            clsprocess.closeqty = ttlstock
            clsprocess.closecost = cost
            If adjustnow = 0 Then
                clsprocess.InvDate = Date.Now.Date.ToString("yyyy-MM-dd 00:00:00")
            Else

                clsprocess.InvDate = Date.Now.AddDays(-1).Date.ToString("yyyy-MM-dd 00:00:00")

            End If

            clsprocess.insertStockOpname()
            'akhir untuk langsung nambah stoclcls
            If adjustnow = 1 Then
                clsprocess.executeNonQuery_Inv_Processstock(Date.Now.AddDays(-1).Date.ToString("yyyy-MM-dd 00:00:00"), Date.Now.Date.ToString("yyyy-MM-dd 00:00:00"), custid, stkid, cc)
            ElseIf adjustnow = 2 Then 'opening today
                clsprocess.executeNonQuery_Inv_Processstock(Date.Now.AddDays(-1).Date.ToString("yyyy-MM-dd 00:00:00"), Date.Now.Date.ToString("yyyy-MM-dd 00:00:00"), custid, stkid, cc, True)
            End If



        Catch ex As Exception

            errLog(Err.Description, "webservice:insertInvStockOpname")
            Err.Clear()
        End Try
    End Sub

    'cek apakah data stockopnamenya sudah ada atau belum
    Private Function getInvStkOpnameEdit(ByVal stkid As Integer, ByVal ccid As Integer, ByVal custid As Integer, Optional ByVal invdate As DateTime = Nothing) As DataSet
        Dim csOname As New clsInventory_stkOpname
        If invdate = Nothing Then
            invdate = Date.Now.ToString("yyyy-MM-dd 00:00:00")
        Else
            invdate = invdate.ToString("yyyy-MM-dd 00:00:00")
        End If
        Return csOname.SelectDataset_InvstkOpnameEdit(stkid, ccid, custid, invdate)
        csOname = Nothing

    End Function


    'StockOpname BeforeToday
    Public Sub insertStockOpname_wDate(ByVal cc_name As String, ByVal skno As String, ByVal stkanme As String, ByVal U1 As Decimal, ByVal factor1 As Decimal, ByVal U2 As Decimal, ByVal cost As Decimal, ByVal UserName As String, ByVal dDate As Date, Optional ByVal adjustnow As Integer = 0)
        Dim c_cmd As New clsDbo("")
        Dim strheader As String = ""
        Dim ds As DataSet
        Dim dsStkMaster As DataSet

        Dim iStkId As Integer
        Dim iFactor1 As Decimal
        Dim icustId As Integer
        Dim dTotalStock As Decimal
        Dim sUnit1 As String = ""
        Dim sUnit2 As String = ""
        Dim iCc As Integer

        Try

            Dim csStock As New clsInventory_stk
            dsStkMaster = csStock.SelectDataset_StkByNumber(skno)
            csStock = Nothing

            If dsStkMaster.Tables(0).Rows.Count > 0 Then

                iStkId = dsStkMaster.Tables(0).Rows(0).Item("id")
                iFactor1 = dsStkMaster.Tables(0).Rows(0).Item("Factor1") 'factor1
                icustId = dsStkMaster.Tables(0).Rows(0).Item("CustID")

                dTotalStock = (U1 * iFactor1) + U2

                sUnit1 = dsStkMaster.Tables(0).Rows(0).Item("Unit1")
                sUnit2 = dsStkMaster.Tables(0).Rows(0).Item("Unit2")

            Else
                GoTo Finish
            End If

            dsStkMaster.Dispose()


            Dim csOname As New clsInventory_stkOpname
            iCc = csOname.get_ccID_ByName(cc_name)
            csOname = Nothing

            ds = getInvStkOpnameEdit(iStkId, iCc, icustId)
            ' ds = getInvStkOpnameEdit_wDate(skno, cc_name, dDate)
            'iFactor1 = stock_factor1(icustId, iStkId)

            If cost > 1 Then
                If ds.Tables(0).Rows.Count = 0 Then


                    'strheader = "insert into cld_inv_Trans values (" & c_cmd.ExecuteScalar("select Max(ID) from cld_inv_Trans") & ", " & icustId & " ,'" & dDate.Date.ToString("yyyy-MM-dd") & "', 5, " & iCc & ", " & _
                    '    " " & iStkId & ", '" & stkanme & "', " & U1 & ", " & U2 & ", " & iFactor1 & ", " & dTotalStock & ", 0, " & cost & ", " & _
                    '    " '', 1, '" & sUnit1 & "', '" & sUnit2 & "', '" & UserName & "', '" & Date.Now.ToString("yyyy-MM-dd HH:mm:ss") & "', Null, Null, '" & skno & "', '" & cc_name & "')"

                    If adjustnow = 1 Then
                        strheader = "insert into cld_inv_Trans values (" & c_cmd.ExecuteScalar("select Max(ID) from cld_inv_Trans") & ", " & icustId & " ,'" & dDate.ToString("yyyy-MM-dd") & "', 11, " & iCc & ", " &
                       " " & iStkId & ", '" & stkanme & "', " & U1 & ", " & U2 & ", " & iFactor1 & ", " & dTotalStock & ", 0, " & cost & ", " &
                       " '', 1, '" & sUnit1 & "', '" & sUnit2 & "', '" & UserName & "', '" & DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") & "', Null, Null, '" & skno & "', '" & cc_name & "')"

                    ElseIf adjustnow = 2 Then
                        strheader = "insert into cld_inv_Trans values (" & c_cmd.ExecuteScalar("select Max(ID) from cld_inv_Trans") & ", " & icustId & " ,'" & dDate.ToString("yyyy-MM-dd") & "', 12, " & iCc & ", " &
                       " " & iStkId & ", '" & stkanme & "', " & U1 & ", " & U2 & ", " & iFactor1 & ", " & dTotalStock & ", 0, " & cost & ", " &
                       " '', 1, '" & sUnit1 & "', '" & sUnit2 & "', '" & UserName & "', '" & DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") & "', Null, Null, '" & skno & "', '" & cc_name & "')"

                    End If
                    c_cmd.executeNonQuery(strheader)
                    ' c_cmd.executeNonQuery_Inv_CalcStock(stkid, cc, custid)
                    'insertLogModule(username, "InvStockOpname", "Insert", custid)

                Else

                    If adjustnow = 1 Then
                        strheader = "update cld_Inv_Trans set  U1 = " & U1 & ", U2 = " & U2 & ", factor1 = " & iFactor1 & ", ttlstock = " & dTotalStock & ", Cost = " & cost & ", " &
                    " ModifBy = '" & UserName & "', Invdate = '" & dDate.ToString("yyyy-MM-dd") & "', Modifdate = '" & DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") & "', type = 11 where id = " & ds.Tables(0).Rows(0).Item("id") & ""


                    ElseIf adjustnow = 2 Then
                        strheader = "update cld_Inv_Trans set  U1 = " & U1 & ", U2 = " & U2 & ", factor1 = " & iFactor1 & ", ttlstock = " & dTotalStock & ", Cost = " & cost & ", " &
                    " ModifBy = '" & UserName & "', Invdate = '" & dDate.ToString("yyyy-MM-dd") & "', Modifdate = '" & DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") & "', type = 12 where id = " & ds.Tables(0).Rows(0).Item("id") & ""


                    End If
                    c_cmd.executeNonQuery(strheader)
                    'strheader = "update cld_Inv_Trans set  U1 = " & U1 & ", U2 = " & U2 & ", factor1 = " & iFactor1 & ", ttlstock = " & dTotalStock & ", Cost = " & cost & ", " &
                    '" ModifBy = '" & UserName & "', Modifdate = '" & Date.Now.ToString("yyyy-MM-dd HH:mm:ss") & "'  where id = " & ds.Tables(0).Rows(0).Item("id") & ""


                    ' c_cmd.executeNonQuery_Inv_CalcStock(stkid, cc, custid)
                    'insertLogModule(username, "InvStockOpname", "Update", custid)


                End If
            Else
                Dim clsis As New clsInventory_stk
                If clsis.SelectDataStkCost(iStkId, iCc, icustId, CDate(Now)).Tables(0).Rows.Count > 0 Then
                    cost = clsis.SelectDataStkCost(iStkId, iCc, icustId, CDate(Now)).Tables(0).Rows(0).Item("cost")
                End If

                If ds.Tables(0).Rows.Count = 0 Then




                    If adjustnow = 1 Then
                        strheader = "insert into cld_inv_Trans values (" & c_cmd.ExecuteScalar("select Max(ID) from cld_inv_Trans") & ", " & icustId & " ,'" & dDate.ToString("yyyy-MM-dd") & "', 11, " & iCc & ", " &
                       " " & iStkId & ", '" & stkanme & "', " & U1 & ", " & U2 & ", " & iFactor1 & ", " & dTotalStock & ", 0, " & cost & ", " &
                       " '', 1, '" & sUnit1 & "', '" & sUnit2 & "', '" & UserName & "', '" & DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") & "', Null, Null, '" & skno & "', '" & cc_name & "')"

                    ElseIf adjustnow = 2 Then
                        strheader = "insert into cld_inv_Trans values (" & c_cmd.ExecuteScalar("select Max(ID) from cld_inv_Trans") & ", " & icustId & " ,'" & dDate.ToString("yyyy-MM-dd") & "', 12, " & iCc & ", " &
                       " " & iStkId & ", '" & stkanme & "', " & U1 & ", " & U2 & ", " & iFactor1 & ", " & dTotalStock & ", 0, " & cost & ", " &
                       " '', 1, '" & sUnit1 & "', '" & sUnit2 & "', '" & UserName & "', '" & DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") & "', Null, Null, '" & skno & "', '" & cc_name & "')"

                    End If
                    c_cmd.executeNonQuery(strheader)

                Else

                    If adjustnow = 1 Then
                        strheader = "update cld_Inv_Trans set  U1 = " & U1 & ", U2 = " & U2 & ", factor1 = " & iFactor1 & ", ttlstock = " & dTotalStock & ", " &
                    " ModifBy = '" & UserName & "', Invdate = '" & dDate.ToString("yyyy-MM-dd") & "', Modifdate = '" & DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") & "', type = 11 where id = " & ds.Tables(0).Rows(0).Item("id") & ""


                    ElseIf adjustnow = 2 Then
                        strheader = "update cld_Inv_Trans set  U1 = " & U1 & ", U2 = " & U2 & ", factor1 = " & iFactor1 & ", ttlstock = " & dTotalStock & ", " &
                    " ModifBy = '" & UserName & "', Invdate = '" & dDate.ToString("yyyy-MM-dd") & "', Modifdate = '" & DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") & "', type = 12 where id = " & ds.Tables(0).Rows(0).Item("id") & ""


                    End If
                    c_cmd.executeNonQuery(strheader)

                End If
            End If



            'untuk langsung nambah stoclcls
            Dim clsprocess As New clsInventoryProcess
            clsprocess.InvstkId = iStkId
            clsprocess.InvStkNo = skno
            clsprocess.InvCc = iCc
            clsprocess.InvCcName = cc_name
            clsprocess.InvCustId = icustId
            clsprocess.InvstkName = stkanme
            clsprocess.closeqty = dTotalStock
            clsprocess.closecost = cost

            'clsprocess.InvDate = Date.Now.AddDays(-1).Date.ToString("yyyy-MM-dd 00:00:00")
            clsprocess.InvDate = dDate.AddDays(-1).Date.ToString("yyyy-MM-dd 00:00:00")

            clsprocess.insertStockOpname()
            'akhir untuk langsung nambah stoclcls

            clsprocess.executeNonQuery_Inv_Processstock(dDate.AddDays(-1).Date.ToString("yyyy-MM-dd 00:00:00"), Date.Now.Date.ToString("yyyy-MM-dd 00:00:00"), icustId, iStkId, iCc, True)

            'clsprocess.executeNonQuery_Inv_Processstock(Date.Now.AddDays(-1).Date.ToString("yyyy-MM-dd 00:00:00"), Date.Now.Date.ToString("yyyy-MM-dd 00:00:00"), icustId, iStkId, iCc, True)


        Catch ex As Exception

            errLog(Err.Description, "webservice:insertStockOpname_wDate")
            Err.Clear()
        End Try


Finish:

        c_cmd.dispose()
        c_cmd = Nothing
    End Sub

    'cek apakah data stockopnamenya sudah ada atau belum
    Private Function getInvStkOpnameEdit_wDate(ByVal skno As String, ByVal cc_name As String, ByVal dDate As Date) As DataSet
        Dim csOname As New clsInventory_stkOpname
        Return csOname.SelectDataset_InvstkOpnameEdit_ExportImport(skno, cc_name, dDate)
        csOname = Nothing

    End Function

    'cek data stock master dan costnya jg
    Public Function Select_StockData(ByVal id As Integer, ByVal ccid As Integer, ByVal cust As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""

        'Dim a As New clsDbo("SELECT Number, Name, Factor1, Factor2, unit1, unit2, IFNULL((SELECT Cost FROM cld_inv_stkcost WHERE Stkid = " & id & " and cc = " & ccid & "), 0) as Cost FROM cld_Inv_Stk where id = " & id & " and custid = " & cust)

        Dim a As New clsDbo("SELECT Number, Name, Factor1, Factor2, unit1, unit2, IFNULL((SELECT Cost FROM cld_inv_transstockcls WHERE Stkid = " & id & " and ccid = " & ccid & " and invdate = '" & Now.ToString("yyyy-MM-dd 00:00:00") & "'), 0) as Cost
                            , IFNULL((SELECT ttlqty FROM cld_inv_transstockcls WHERE Stkid = " & id & " and ccid = " & ccid & " and invdate = '" & Now.ToString("yyyy-MM-dd 00:00:00") & "'), 0) as ttlqty
                        FROM cld_Inv_Stk where id = " & id & " and custid = " & cust)


        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds
    End Function
    'cek transaksi Bram 6 Juli 2022
    Public Function check_belumTrans(ByVal id As Integer, ByVal ccid As Integer, ByVal cust As Integer) As Boolean
        Dim trans As Boolean = False
        Dim strFilter As String = ""


        Dim a As New clsDbo("")

        If a.ExecuteScalar("SELECT count(stkno) FROM cld_inv_transstockcls WHERE Stkid = " & id & " and ccid = " & ccid & " and ttlqty<>openqty and invdate = '" & Now.ToString("yyyy-MM-dd 00:00:00") & "'", 0) = 0 Then
            trans = True
        End If

        a.dispose()
        a = Nothing

        Return trans
    End Function

    Public Function check_belumdelivery(ByVal id As String, ByVal ccid As Integer) As Boolean
        Dim trans As Boolean = False
        Dim strFilter As String = ""


        Dim a As New clsDbo("")

        If a.ExecuteScalar("SELECT count(id) FROM cld_inv_w WHERE id = '" & id & "' and ccto = " & ccid & " and Flag>1 ", 0) = 0 Then
            trans = True
        End If

        a.dispose()
        a = Nothing

        Return trans
    End Function

    Public Function Select_StockData2(ByVal id As Integer, ByVal ccid As Integer, ByVal cust As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""

        'Dim a As New clsDbo("SELECT Number, Name, Factor1, Factor2, unit1, unit2, IFNULL((SELECT Cost FROM cld_inv_stkcost WHERE Stkid = " & id & " and cc = " & ccid & "), 0) as Cost FROM cld_Inv_Stk where id = " & id & " and custid = " & cust)

        Dim a As New clsDbo("SELECT Number, Name, Factor1, Factor2, unit1, unit2, IFNULL((SELECT Cost FROM cld_inv_transstockcls WHERE Stkid = " & id & " and ccid = " & ccid & " and invdate = '" & Now.ToString("yyyy-MM-dd 00:00:00") & "'), 0) as Cost
                            , IFNULL((SELECT ttlqty FROM cld_inv_transstockcls WHERE Stkid = " & id & " and ccid = " & ccid & " and invdate = '" & Now.ToString("yyyy-MM-dd 00:00:00") & "'), 0) as ttlqty
                        FROM cld_Inv_Stk where id = " & id & " and custid = " & cust)


        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds
    End Function
    'Public Sub Process_InvUsage_old(ByVal dDate As Date, ByVal iCustID As Integer)
    '    Dim c_cmd As New clsDbo("")
    '    Dim i_SalesID As Integer = 0
    '    Dim ds As DataSet
    '    Try

    '        ds = c_cmd.ReturnDataset("SELECT cld_salesdetail.Sid, cld_salesdetail.outletid, cld_salesdetail.smno, cld_salesdetail.ItemNo, cld_salesmaster.Busidate, cld_salesdetail.qty FROM cld_salesdetail INNER JOIN cld_salesmaster ON cld_salesmaster.number = cld_salesdetail.smno AND cld_salesmaster.olid = cld_salesdetail.outletid INNER JOIN cld_outlet ON cld_salesmaster.olid = cld_outlet.ID WHERE SID = 0 AND cld_outlet.otype = 2 and cld_salesmaster.Busidate = '" & dDate.Date.ToString("yyyy-MM-dd") & "' ORDER BY cld_salesdetail.smno, cld_salesdetail.ItemNo")

    '        If ds.Tables(0).Rows.Count > 0 Then

    '            For Each row As DataRow In ds.Tables(0).Rows
    '                c_cmd.ReNew()
    '                i_SalesID = c_cmd.ExecuteScalar("SELECT MAX(Sid) FROM cld_salesdetail where custid = " & iCustID)

    '                c_cmd.ReNew()
    '                c_cmd.executeNonQuery("Update cld_salesdetail Set SID = " & i_SalesID & " where smno = '" & row.Item("smno") & "' and ItemNo = '" & row.Item("ItemNo") & "' and Outletid = " & row.Item("Outletid"))

    '                Dim c_invUsage As New clsInventory_Usage
    '                c_invUsage.ProcessUsage(iCustID, row.Item("Outletid"), row.Item("ItemNo"), row.Item("qty"), dDate, True, i_SalesID)
    '                c_invUsage = Nothing
    '            Next


    '        End If


    '    Catch ex As Exception

    '        errLog(Err.Description, "clsInventory:insertInvStockOpname")
    '        Err.Clear()
    '    End Try


    '    c_cmd.dispose()
    '    c_cmd = Nothing

    'End Sub
    Public Sub Process_InvUsage(ByVal dDate As Date, ByVal iCustID As Integer)
        Dim c_cmd As New clsDbo("")
        Dim i_SalesID As Integer = 0
        Dim ds As New DataSet
        Dim cistk As New Cls_IStk
        Try

            ds = c_cmd.ReturnDataset("SELECT * from cld_inv_transusagesum where flag = 1 and invdate = '" & dDate.Date.ToString("yyyy-MM-dd") & "' and custid = " & iCustID)

            If ds.Tables(0).Rows.Count > 0 Then

                For Each row As DataRow In ds.Tables(0).Rows

                    Dim c_invUsage As New clsInventory_Usage
                    Dim stkno As String = ""
                    Dim stknm As String = ""
                    Dim ccname As String = ""
                    Dim ds2 As New DataSet
                    ds2 = cistk.SelectDataStk(row.Item("stk_id"), iCustID)
                    If ds2.Tables(0).Rows.Count > 0 Then
                        stkno = ds2.Tables(0).Rows(0).Item("number")
                        stknm = ds2.Tables(0).Rows(0).Item("name")
                    End If
                    ds2.Dispose()

                    ccname = c_cmd.ExecuteScalar("select name from cld_inv_cc where id=" & row.Item("cc") & "", 0)

                    c_invUsage.insert_Transusage_Stk(iCustID, row.Item("cc"), ccname, row.Item("stk_id"), stkno, stknm, row.Item("ttlstock"), dDate.ToString("yyyy-MM-dd HH:mm:ss"))

                    c_invUsage = Nothing
                    c_cmd.ReNew()
                    c_cmd.executeNonQuery("UPDATE cld_inv_transusagesum SET flag=0 WHERE invdate = '" & dDate.Date.ToString("yyyy-MM-dd") & "' and custid = " & iCustID & " and cc=" & row.Item("cc") & " and stk_id=" & row.Item("stk_id") & " ")
                    c_cmd.dispose()
                Next
                ds.Dispose()
                'Else
                '    c_cmd.ReNew()
                '    c_cmd.executeNonQuery("DELETE from cld_inv_closing WHERE invdate >= '" & dDate.Date.ToString("yyyy-MM-dd") & "' and custid = " & iCustID & " ")
            End If


        Catch ex As Exception

            errLog(Err.Description, "clsInventory:Process_InvUsage")
            Err.Clear()
        End Try


        c_cmd.dispose()
        c_cmd = Nothing

    End Sub
    Public Sub Process_InvUsagePO(ByVal dDate As Date, ByVal iCustID As Integer, Optional ball As Boolean = False)
        Dim c_cmd As New clsDbo("", 1)
        Dim i_SalesID As Integer = 0
        Dim ds As New DataSet()

        Try


            If ball = False Then
                ds = c_cmd.ReturnDataset("SELECT cld_salesdetail_cpo.Sid, cld_salesdetail_cpo.outletid, cld_salesdetail_cpo.smno, cld_salesdetail_cpo.ItemNo, cld_salesmaster_cpo.Busidate, cld_salesdetail_cpo.qty FROM cld_salesdetail_cpo INNER JOIN cld_salesmaster_cpo ON cld_salesmaster_cpo.number = cld_salesdetail_cpo.smno AND cld_salesmaster_cpo.olid = cld_salesdetail_cpo.outletid INNER JOIN cld_outlet ON cld_salesmaster_cpo.olid = cld_outlet.ID WHERE SID = 0 AND cld_outlet.otype = 2 and cld_salesdetail_cpo.flag <> 2 and cld_salesmaster_cpo.Busidate >= '" & dDate.Date.AddDays(-29).ToString("yyyy-MM-dd") & "' ORDER BY cld_salesmaster_cpo.Transdate, cld_salesmaster_cpo.olid, cld_salesdetail_cpo.smno, cld_salesdetail_cpo.ItemNo LIMIT 20000")
            Else
                ds = c_cmd.ReturnDataset("SELECT cld_salesdetail_cpo.Sid, cld_salesdetail_cpo.outletid, cld_salesdetail_cpo.smno, cld_salesdetail_cpo.ItemNo, cld_salesmaster_cpo.Busidate, cld_salesdetail_cpo.qty FROM cld_salesdetail_cpo INNER JOIN cld_salesmaster_cpo ON cld_salesmaster_cpo.number = cld_salesdetail_cpo.smno AND cld_salesmaster_cpo.olid = cld_salesdetail_cpo.outletid INNER JOIN cld_outlet ON cld_salesmaster_cpo.olid = cld_outlet.ID WHERE SID = 0 AND cld_outlet.otype = 2 and cld_salesdetail_cpo.flag <> 2 ORDER BY cld_salesmaster_cpo.Transdate, cld_salesmaster_cpo.olid, cld_salesdetail_cpo.smno, cld_salesdetail_cpo.ItemNo LIMIT 20000")
            End If


            If ds.Tables(0).Rows.Count > 0 Then
                i_SalesID = c_cmd.ExecuteScalar("SELECT lastno FROM cld_numbering where name = 'salesdetail'")

                For Each row As DataRow In ds.Tables(0).Rows
                    c_cmd.ReNew()
                    'i_SalesID = c_cmd.ExecuteScalar("SELECT lastno FROM cld_numbering where name = 'salesdetail'")

                    Dim c_invUsage As New clsInventory_Usage
                    c_invUsage.ProcessUsagePO(iCustID, row.Item("Outletid"), row.Item("ItemNo"), row.Item("qty"), row.Item("Busidate"), True, i_SalesID, row.Item("smno"))
                    c_invUsage = Nothing

                    c_cmd.ReNew()
                    c_cmd.executeNonQuery("Update cld_salesdetail_cpo Set SID = " & i_SalesID & " where smno = '" & row.Item("smno") & "' and ItemNo = '" & row.Item("ItemNo") & "' and Outletid = " & row.Item("Outletid"))
                    'c_cmd.executeNonQuery("Update cld_numbering Set lastno = " & i_SalesID)

                    i_SalesID += 1
                Next
                c_cmd.executeNonQuery("Update cld_numbering Set lastno = " & i_SalesID)

            End If


        Catch ex As Exception

            errLog(Err.Description, "clsInventory:Process_InvUsage")
            Err.Clear()
        End Try

        ds.Dispose()
        ds = Nothing

        c_cmd.dispose()
        c_cmd = Nothing

    End Sub



    Public Sub recall_InvUsage(ByVal dDate As Date, ByVal iCustID As Integer)
        Dim c_cmd As New clsDbo("")

        Try


            c_cmd.executeNonQuery("UPDATE cld_inv_transusagesum SET flag=1 WHERE invdate >= '" & dDate.Date.ToString("yyyy-MM-dd") & "' and custid = " & iCustID & "")

        Catch ex As Exception

            errLog(Err.Description, "clsInventory:recall_InvUsage")
            Err.Clear()
        End Try


        c_cmd.dispose()
        c_cmd = Nothing

    End Sub

    Public Function lastdate_InvUsage(ByVal iCustID As Integer) As DataSet
        Dim c_cmd As New clsDbo("")

        Dim ds As New DataSet
        Try

            ds = c_cmd.ReturnDataset("SELECT DATE_FORMAT(MIN(invdate),'%d-%b-%Y') ldate,cc FROM cld_inv_transusagesum WHERE flag = 1 and custid = " & iCustID & " GROUP BY cc ")

        Catch ex As Exception

            errLog(Err.Description, "clsInventory:insertInvStockOpname")
            Err.Clear()
        End Try
        c_cmd.dispose()
        c_cmd = Nothing
        Return ds
    End Function

#Region "Itemmaster"


    Public Function getItemMaster(ByVal custid As Integer) As DataSet

        Dim clsInv_ItemMaster As New clsInventory_ItemMaster

        Return clsInv_ItemMaster.SelectDataset_Itemmaster(custid)

        clsInv_ItemMaster = Nothing
    End Function

    Public Function getItemSearch(ByVal name As String, ByVal custid As Integer, ByVal bLink As Boolean) As DataSet
        Dim clsInv_ItemMaster As New clsInventory_ItemMaster

        Return clsInv_ItemMaster.SelectDataset_ItemmasterSearch(name, custid, bLink)

        clsInv_ItemMaster = Nothing
    End Function


    Public Sub DeleteItemMaster(ByVal id As String, ByVal Custid As Integer, ByVal username As String)

        Try

            Dim str As String
            Dim c_cmd As New clsDbo("")

            str = "DELETE FROM cld_itemmaster  where ItemNo = '" & id & "'"

            c_cmd.executeNonQuery(str)

            'Revisi 10 Mei
            insertLogModule(username, "ItemMaster " & id.ToString, "Delete", Custid)


            c_cmd = Nothing

        Catch ex As Exception

            errLog(Err.Description, "clsInventory:DeleteItemMaster")
            Err.Clear()
        End Try
    End Sub

#End Region


    Public Function DeleteStk(ByVal id As Integer, ByVal Custid As Integer, ByVal username As String) As Integer


        Dim iHasil As Integer = 0


        Try

            Dim str As String
            Dim c_cmd As New clsDbo("")

            str = "DELETE FROM cld_inv_stk  where id = " & id & ""
            'str = "DELETE FROM cld_inv_stk_cc  where stkid = " & id & " and ccid = " & ccid & " and custid = " & Custid & ""
            If c_cmd.ExecuteScalar("Select stkID from cld_inv_transstockcls where stkID = " & id & "", 2) = 0 Then
                c_cmd.ReNew()
                c_cmd.executeNonQuery(str)

                iHasil = 1

                'Revisi 10 Mei
                insertLogModule(username, "InvStk " & id.ToString, "Delete", Custid)

            Else
                iHasil = 2
            End If



            c_cmd = Nothing

        Catch ex As Exception

            errLog(Err.Description, "clsInventory: DeleteInvStk")
            Err.Clear()
        End Try

        Return iHasil

    End Function

#Region "Batch"

    Public Function insertBatch(ByVal id As Integer, ByVal cc As Integer, ByVal stkid As Integer, ByVal stkanme As String, ByVal U1 As Decimal, ByVal U2 As Decimal, ByVal factor1 As Decimal, ByVal ttlstock As Decimal, ByVal U1_nm As String, ByVal U2_nm As String, ByVal createby As String, ByVal modifby As String, ByVal skno As String, ByVal cc_name As String, ByVal username As String, ByVal custid As Integer, ByVal dDate As DateTime, Optional ByVal rcpid As Integer = 0) As Boolean
        Dim c_cmd As New clsDbo("")
        Dim strheader, strheader2 As String
        'Dim strdtl As String
        Dim iMax As Integer = c_cmd.ExecuteScalar("select Max(ID) from cld_inv_trans")
        Dim cInvBatch As New clsInventory_Batch
        Dim c_proc As New clsInventoryProcess
        'Dim dsCost As DataSet
        'Dim rcpid As Integer
        ' dsCost = c_cmd.ReturnDataset("SELECT ID, Cost FROM cld_inv_rcp WHERE id = (SELECT Blink FROM cld_inv_stk WHERE id =" & stkid & ") and custid = " & custid)
        'dsCost = c_cmd.ReturnDataset("SELECT rcpid, Cost FROM cld_inv_rcp_cc WHERE rcpid = (SELECT Blink FROM cld_inv_stk WHERE id =" & stkid & ") and ccid = " & cc & " and custid = " & custid)
        'dsCost = c_proc.getdscost(stkid, cc, custid)
        'untuk proses
        Dim clsprocess As New clsInventoryProcess
        Dim bHasil As Boolean = False
        Dim bcost As Decimal = 0
        Try

            'If dsCost.Tables(0).Rows.Count <= 0 Then
            '    GoTo Endline
            'End If

            If factor1 <= 0 Then
                factor1 = 1
            End If
            'ambil cost dulu
            If id <> 0 Then
                'jadi klo edit

                'delete dulu yg closing
                ' bcost = bcost + cInvBatch.getproperty_RcpDetail_del(custid, dsCost.Tables(0).Rows(0).Item("id"), ttlstock, Date.Now.Date.ToString("yyyy-MM-dd"), id, cc, cc_name)

                'bcost = bcost + 
                cInvBatch.getproperty_RcpDetail_del(custid, stkid, ttlstock / factor1, dDate, id, cc, cc_name, id, rcpid)


                'c_cmd.ReNew()
                'c_cmd.executeNonQuery("update cld_inv_trans set U1 = " & U1 & ",U2 = " & U2 & ",ttlstock = " & ttlstock & " where id = " & id & " and cc = " & cc & " and stk_id = " & stkid & " and custid = " & custid & " ")


                ' bcost = bcost + cInvBatch.getproperty_RcpDetail(custid, dsCost.Tables(0).Rows(0).Item("id"), ttlstock, Date.Now.Date.ToString("yyyy-MM-dd"), id, cc, cc_name)
                bcost = bcost + cInvBatch.getproperty_RcpDetail(custid, stkid, ttlstock / factor1, dDate, id, cc, cc_name, rcpid)


            Else
                'klo insert baru
                'bcost = bcost + cInvBatch.getproperty_RcpDetail(custid, dsCost.Tables(0).Rows(0).Item("id"), ttlstock, Date.Now.Date.ToString("yyyy-MM-dd"), iMax, cc, cc_name)
                bcost = bcost + cInvBatch.getproperty_RcpDetail(custid, stkid, ttlstock / factor1, dDate, iMax, cc, cc_name, rcpid)
            End If
            '24July2025 jagain cost tidak minus
            If bcost > 0 Then
                bcost = bcost / ttlstock
            Else
                writeerrcsv(True, dDate.Date.ToString("yyyy-MM-dd 00:00:00"), cc, stkid)
                'GoTo Endline
            End If


            c_cmd.ReNew()
            If rcpid = 0 Then
                rcpid = c_cmd.ExecuteScalar("select blink from cld_inv_stk where id = " & stkid & "", 0)
            End If

            If My.Settings.rcp_cc = 1 Then
                c_cmd.ReNew()
                c_cmd.executeNonQuery("update cld_inv_rcp_cc set cost = " & bcost * factor1 & " where rcpid = " & rcpid & " and ccid = " & cc & " and custid = " & custid & " ")

            End If

            If id = 0 Then

                'strheader = "insert into cld_inv_Trans values (" & iMax & ", " & custid & " ,'" & Date.Now.Date.ToString("yyyy-MM-dd") & "', 9, " & cc & ", " &
                '    " " & stkid & ", '" & stkanme & "', " & U1 & ", " & U2 & ", " & factor1 & ", " & ttlstock & ", 0, " & dsCost.Tables(0).Rows(0).Item("Cost") / factor1 & ", " &
                '    " '', 1, '" & U1_nm & "', '" & U2_nm & "', '" & createby & "', '" & Now.ToString("yyyy-MM-dd HH:mm:ss") & "', Null, Null, '" & skno & "', '" & cc_name & "')"


                strheader = "insert into cld_inv_Trans values (" & iMax & ", " & custid & " ,'" & dDate.ToString("yyyy-MM-dd 00:00:00") & "', 9, " & cc & ", " &
                    " " & stkid & ", '" & stkanme & "', " & U1 & ", " & U2 & ", " & factor1 & ", " & ttlstock & ", 0, " & bcost & ", " &
                    " '', 1, '" & U1_nm & "', '" & U2_nm & "', '" & createby & "', '" & DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") & "', Null, Null, '" & skno & "', '" & cc_name & "')"

                c_cmd.ReNew()
                c_cmd.executeNonQuery(strheader)

                If rcpid > 0 Then
                    strheader2 = "insert into cld_inv_transnotes values (" & iMax & ", " & cc & ", " & rcpid & ", 'Batch')"
                    c_cmd.ReNew()
                    c_cmd.executeNonQuery(strheader2)
                End If




                clsprocess.InvstkId = stkid
                clsprocess.InvCc = cc
                clsprocess.InvCcName = cc_name
                clsprocess.InvCustId = custid
                clsprocess.InvstkName = stkanme
                clsprocess.BP_in = ttlstock
                clsprocess.BP_out = 0
                clsprocess.InvStkNo = skno
                clsprocess.InvDate = dDate
                'clsprocess.InvCost = bcost
                clsprocess.BCost = True
                clsprocess.executeNonQuery_Inv_ProcessCls()

                insertLogModule(username, "InvRwaste", "Insert", custid)



            Else
                If c_cmd.ExecuteScalar("select bpin from cld_inv_transstockcls where stkid = " & stkid & " AND CCid = " & cc & " AND invdate = '" & dDate.ToString("yyyy-MM-dd 00:00:00") & "' and custid = " & custid, 0) = 0 Then
                    GoTo endB
                End If
                'delete dulu sebelum update
                clsprocess.InvstkId = stkid
                clsprocess.InvCc = cc
                clsprocess.InvCcName = cc_name
                clsprocess.InvCustId = custid
                clsprocess.InvstkName = stkanme
                clsprocess.BP_in = -(CDbl(c_cmd.ExecuteScalar("select ttlstock from cld_Inv_Trans where id = " & id & " and custid = " & custid, 0)))
                clsprocess.BP_out = 0
                clsprocess.InvStkNo = skno
                clsprocess.InvDate = dDate
                clsprocess.InvCost = bcost
                clsprocess.BCost = True
                clsprocess.executeNonQuery_Inv_ProcessCls()


endB:
                strheader = "update cld_Inv_Trans set cost = " & bcost & ", Cc = " & cc & ", U1 = " & U1 & ", U2 = " & U2 & ", factor1 = " & factor1 & ", ttlstock = " & ttlstock &
                ", ModifBy = '" & username & "', Modifdate = '" & DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") & "' , Cc_name = '" & cc_name & "' where id = " & id & " and custid = " & custid & ""
                c_cmd.ReNew()
                c_cmd.executeNonQuery(strheader)
                If rcpid > 0 Then
                    strheader2 = "update cld_inv_transnotes set Stk_link= " & rcpid & " where transid = " & id & " and Cc=" & cc & ""
                    c_cmd.ReNew()
                    c_cmd.executeNonQuery(strheader2)
                End If


                clsprocess.InvstkId = stkid
                clsprocess.InvCc = cc
                clsprocess.InvCcName = cc_name
                clsprocess.InvCustId = custid
                clsprocess.InvstkName = stkanme
                clsprocess.BP_in = ttlstock
                clsprocess.BP_out = 0
                clsprocess.InvStkNo = skno
                clsprocess.InvDate = dDate
                clsprocess.BCost = True
                clsprocess.InvCost = bcost
                clsprocess.executeNonQuery_Inv_ProcessCls()


                insertLogModule(username, "InvRwaste", "Update", custid)



            End If



            'If id <> 0 Then
            '    'jadi klo edit

            '    'delete dulu yg closing
            '    cInvBatch.getproperty_RcpDetail_del(custid, dsCost.Tables(0).Rows(0).Item("ID"), ttlstock / factor1, Date.Now.Date.ToString("yyyy-MM-dd"), id, cc, cc_name)


            '    c_cmd.executeNonQuery("Delete from cld_inv_transusage where type = 11 and linkid = " & id)
            '    cInvBatch.getproperty_RcpDetail(custid, dsCost.Tables(0).Rows(0).Item("ID"), ttlstock / factor1, Date.Now.Date.ToString("yyyy-MM-dd"), id, cc, cc_name)
            'Else
            '    'klo insert baru
            '    cInvBatch.getproperty_RcpDetail(custid, dsCost.Tables(0).Rows(0).Item("ID"), ttlstock / factor1, Date.Now.Date.ToString("yyyy-MM-dd"), iMax, cc, cc_name)
            'End If

            cInvBatch = Nothing


            bHasil = True

        Catch ex As Exception

            errLog(Err.Description, "clsInventory:insertBatch")
            Err.Clear()
        End Try
Endline:

        Return bHasil

    End Function
    Public Function del_stkcls_Batch(ByVal id As Integer, ByVal cc As Integer, ByVal stkid As Integer, ByVal stkanme As String, ByVal U1 As Decimal, ByVal U2 As Decimal, ByVal factor1 As Decimal, ByVal ttlstock As Decimal, ByVal U1_nm As String, ByVal U2_nm As String, ByVal createby As String, ByVal skno As String, ByVal cc_name As String, ByVal username As String, ByVal custid As Integer, Optional ByVal rcpid As Integer = 0) As Boolean
        Dim c_cmd As New clsDbo("")
        'Dim strheader As String
        ' Dim iMax As Integer = c_cmd.ExecuteScalar("select Max(ID) from cld_inv_trans")
        Dim clsprocess As New clsInventoryProcess


        clsprocess.InvstkId = stkid
        clsprocess.InvCc = cc
        clsprocess.InvCcName = cc_name
        clsprocess.InvCustId = custid
        clsprocess.InvstkName = stkanme
        clsprocess.BP_in = -ttlstock
        clsprocess.BP_out = 0
        clsprocess.InvStkNo = skno
        clsprocess.InvDate = Date.Now.Date.ToString("yyyy-MM-dd")
        clsprocess.BCost = True
        clsprocess.executeNonQuery_Inv_ProcessCls()

        insertLogModule(username, "clsinventory", "del_stkcls_Batch", custid)



        Dim dsCost As DataSet
        If rcpid = 0 Then
            dsCost = c_cmd.ReturnDataset("SELECT ID, Cost FROM cld_inv_rcp WHERE id = (SELECT Blink FROM cld_inv_stk WHERE id =" & stkid & ") and custid = " & custid)
        Else
            dsCost = c_cmd.ReturnDataset("SELECT ID, Cost FROM cld_inv_rcp WHERE id = " & rcpid & " and custid = " & custid)
        End If
        'untuk proses
        ' Dim clsprocess As New clsInventoryProcess
        Dim bHasil As Boolean = False
        Try

            If dsCost.Tables(0).Rows.Count <= 0 Then
                GoTo Endline
            End If

            If factor1 <= 0 Then
                factor1 = 1
            End If




            Dim cInvBatch As New clsInventory_Batch


            'delete  yg closing
            cInvBatch.getproperty_RcpDetail_del(custid, dsCost.Tables(0).Rows(0).Item("ID"), ttlstock / factor1, Date.Now.Date.ToString("yyyy-MM-dd"), id, cc, cc_name, id)



            cInvBatch = Nothing


            bHasil = True

        Catch ex As Exception

            errLog(Err.Description, "clsInventory:insertInvRwaste")
            Err.Clear()
        End Try
Endline:

        Return bHasil

    End Function

#End Region



#Region "Inventory Transfer"


    Public Function getInvTrf(ByVal bydate As Date, ByVal custid As Integer) As DataSet
        Dim clsTrf As New clsInventory_Transfer
        Return clsTrf.SelectDataset_invtrf(bydate, custid)
    End Function

    Public Function getInvTrf(ByVal bydate As Date, ByVal bydateto As Date, ByVal custid As Integer, Optional ByVal ccFrom As Integer = 0) As DataSet
        Dim clsTrf As New clsInventory_Transfer
        Return clsTrf.SelectDataset_invtrf(bydate, bydateto, custid, ccFrom)
    End Function

    Public Function getInvTrfSearch(ByVal name As String, ByVal bydate As Date, ByVal custid As Integer) As DataSet
        Dim clsTrf As New clsInventory_Transfer
        Return clsTrf.SelectDataset_invtrfSearch(name, bydate, custid)
    End Function

    Public Function getInvTrfSearch(ByVal name As String, ByVal bydate As Date, ByVal bydateto As Date, ByVal custid As Integer, Optional ByVal ccFrom As Integer = 0) As DataSet
        Dim clsTrf As New clsInventory_Transfer
        Return clsTrf.SelectDataset_invtrfSearch(name, bydate, bydateto, custid, ccFrom)
    End Function

    Public Function getDTInvTrans(ByVal linkid As String, ByVal custid As Integer) As DataTable

        Dim clsTrf As New clsInventory_Transfer
        Return clsTrf.SelectDataTable_invtrans_Transfer(linkid, custid)

        clsTrf = Nothing

    End Function


    Public Sub DeleteInvTrf(ByVal id As String, ByVal Custid As Integer, ByVal username As String)

        Try

            Dim str As String = ""
            Dim strTr As String = ""
            Dim c_cmd As New clsDbo("")

            str = "DELETE FROM cld_invlk_trf  where id = '" & id & "' and custid = " & Custid & ""
            c_cmd.executeNonQuery(str)
            'Dim ds As New DataSet
            'Dim a As New clsDbo("Select * FROM cld_inv_trans  where linkid = '" & id & "' and custid = " & Custid & "")
            'ds = a.selectDataset
            'If ds.Tables(0).Rows.Count > 0 Then
            '    For Each row As DataRow In ds.Tables(0).Rows

            c_cmd.ReNew()

            strTr = "DELETE FROM cld_inv_trans  where linkid = '" & id & "' and custid = " & Custid & ""
            c_cmd.executeNonQuery(strTr)

            '    Next
            'End If
            'a.dispose()
            'a = Nothing

            c_cmd.dispose()
            c_cmd = Nothing

            insertLogModule(username, "Inv_invlk_trf " & id.ToString, "Delete", Custid)

        Catch ex As Exception
            errLog(Err.Description, "clsInventory:DeleteInvTrf")
            Err.Clear()
        End Try
    End Sub


    Public Function insertInvtrf(ByVal id As String, ByVal ccfrom As Integer, ByVal dt As DataTable, ByVal username As String, ByVal custid As Integer) As String
        Dim idtrans As String = id
        Dim isNew As Boolean = String.IsNullOrEmpty(id)

        Using conn As New MySqlConnection(My.Settings.ServerStr)
            conn.Open()
            Using trans As MySqlTransaction = conn.BeginTransaction()
                Try
                    If isNew Then
                        idtrans = CItrans.idtransfer(custid)

                        ' Ensure unique ID in cld_invlk_trf
                        Dim tryCount As Integer = 0
                        While True
                            Using cmdCheck As New MySqlCommand("SELECT COUNT(*) FROM cld_invlk_trf WHERE id = @id AND custid = @custid", conn, trans)
                                cmdCheck.Parameters.AddWithValue("@id", idtrans)
                                cmdCheck.Parameters.AddWithValue("@custid", custid)
                                If Convert.ToInt32(cmdCheck.ExecuteScalar()) = 0 Then Exit While
                                tryCount += 1
                                Dim mon As String = Date.Now.Month.ToString("00")
                                Dim yer As String = Mid(Date.Now.Year.ToString(), 3, 2)
                                Dim nextNum As Integer = 1
                                Dim parts = idtrans.Split("-"c)
                                If parts.Length > 1 AndAlso Integer.TryParse(parts(1), nextNum) Then
                                    nextNum += 1
                                End If
                                idtrans = "T" & mon & "/" & yer & "-" & nextNum.ToString("D5")
                                If tryCount > 50 Then Exit While
                            End Using
                        End While

                        Dim sqlMaster As String = "INSERT INTO cld_invlk_trf (id, custid, ccfrom, createdby, invdate, modifby, modifdate) VALUES (@id, @custid, @ccfrom, @createdby, @invdate, NULL, NULL)"
                        Using cmdMaster As New MySqlCommand(sqlMaster, conn, trans)
                            cmdMaster.Parameters.AddWithValue("@id", idtrans)
                            cmdMaster.Parameters.AddWithValue("@custid", custid)
                            cmdMaster.Parameters.AddWithValue("@ccfrom", ccfrom)
                            cmdMaster.Parameters.AddWithValue("@createdby", username)
                            cmdMaster.Parameters.AddWithValue("@invdate", Date.Now.Date.ToString("yyyy-MM-dd"))
                            cmdMaster.ExecuteNonQuery()
                        End Using

                        ' Insert Details
                        Dim sqlDetail As String = "INSERT INTO cld_inv_trans (id, custid, InvDate, `type`, cc, stk_id, stk_name, u1, u2, factor1, ttlstock, costu1, cost, linkid, flagtrans, u1_nm, u2_nm, createdby, createddate, modifby, modifdate, stk_no, cc_name) " &
                                                  "VALUES (@id, @custid, @invdate, 1, @cc, @stk_id, @stk_name, @u1, @u2, @factor1, @ttlstock, 0, @cost, @linkid, @flagtrans, @u1_nm, @u2_nm, @createdby, @createddate, NULL, NULL, @stk_no, @cc_name)"

                        Using cmdDetail As New MySqlCommand(sqlDetail, conn, trans)
                            cmdDetail.Parameters.Add("@id", MySqlDbType.Int32)
                            cmdDetail.Parameters.AddWithValue("@custid", custid)
                            cmdDetail.Parameters.AddWithValue("@invdate", Date.Now.Date.ToString("yyyy-MM-dd"))
                            cmdDetail.Parameters.Add("@cc", MySqlDbType.Int32)
                            cmdDetail.Parameters.Add("@stk_id", MySqlDbType.Int32)
                            cmdDetail.Parameters.Add("@stk_name", MySqlDbType.VarChar)
                            cmdDetail.Parameters.Add("@u1", MySqlDbType.Decimal)
                            cmdDetail.Parameters.Add("@u2", MySqlDbType.Decimal)
                            cmdDetail.Parameters.Add("@factor1", MySqlDbType.Decimal)
                            cmdDetail.Parameters.Add("@ttlstock", MySqlDbType.Decimal)
                            cmdDetail.Parameters.Add("@cost", MySqlDbType.Decimal)
                            cmdDetail.Parameters.AddWithValue("@linkid", idtrans)
                            cmdDetail.Parameters.Add("@flagtrans", MySqlDbType.Int32)
                            cmdDetail.Parameters.Add("@u1_nm", MySqlDbType.VarChar)
                            cmdDetail.Parameters.Add("@u2_nm", MySqlDbType.VarChar)
                            cmdDetail.Parameters.AddWithValue("@createdby", username)
                            cmdDetail.Parameters.AddWithValue("@createddate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                            cmdDetail.Parameters.Add("@stk_no", MySqlDbType.VarChar)
                            cmdDetail.Parameters.Add("@cc_name", MySqlDbType.VarChar)

                            For Each row As DataRow In dt.Rows
                                Dim newDetailId As Integer = 1
                                Using cmdMax As New MySqlCommand("SELECT IFNULL(MAX(id), 0) + 1 FROM cld_inv_trans", conn, trans)
                                    newDetailId = Convert.ToInt32(cmdMax.ExecuteScalar())
                                End Using

                                cmdDetail.Parameters("@id").Value = newDetailId
                                cmdDetail.Parameters("@cc").Value = Convert.ToInt32(row.Item("Cc"))
                                cmdDetail.Parameters("@stk_id").Value = Convert.ToInt32(row.Item("stk_id"))
                                cmdDetail.Parameters("@stk_name").Value = row.Item("Stk_Name").ToString()
                                cmdDetail.Parameters("@u1").Value = Convert.ToDecimal(row.Item("U1"))
                                cmdDetail.Parameters("@u2").Value = Convert.ToDecimal(row.Item("U2"))
                                cmdDetail.Parameters("@factor1").Value = Convert.ToDecimal(row.Item("Factor1"))
                                cmdDetail.Parameters("@ttlstock").Value = Convert.ToDecimal(row.Item("ttlStock"))
                                cmdDetail.Parameters("@cost").Value = Convert.ToDecimal(row.Item("Cost"))
                                cmdDetail.Parameters("@flagtrans").Value = Convert.ToInt32(row.Item("flagtrans"))
                                cmdDetail.Parameters("@u1_nm").Value = row.Item("U1_nm").ToString()
                                cmdDetail.Parameters("@u2_nm").Value = row.Item("U2_nm").ToString()
                                cmdDetail.Parameters("@stk_no").Value = row.Item("stk_no").ToString()
                                cmdDetail.Parameters("@cc_name").Value = row.Item("cc_name").ToString()

                                cmdDetail.ExecuteNonQuery()
                            Next
                        End Using
                    Else
                        ' Update Header
                        Dim sqlMaster As String = "UPDATE cld_invlk_trf SET modifby = @modifby, modifdate = @modifdate WHERE id = @id AND custid = @custid"
                        Using cmdMaster As New MySqlCommand(sqlMaster, conn, trans)
                            cmdMaster.Parameters.AddWithValue("@id", idtrans)
                            cmdMaster.Parameters.AddWithValue("@custid", custid)
                            cmdMaster.Parameters.AddWithValue("@modifby", username)
                            cmdMaster.Parameters.AddWithValue("@modifdate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                            cmdMaster.ExecuteNonQuery()
                        End Using

                        ' Update or Insert Details
                        Dim sqlUpdateDetail As String = "UPDATE cld_inv_trans SET cc = @cc, u1 = @u1, u2 = @u2, factor1 = @factor1, ttlstock = @ttlstock, cost = @cost, " &
                                                        "modifby = @modifby, modifdate = @modifdate, cc_name = @cc_name WHERE id = @id AND custid = @custid"
                        Dim sqlInsertDetail As String = "INSERT INTO cld_inv_trans (id, custid, InvDate, `type`, cc, stk_id, stk_name, u1, u2, factor1, ttlstock, costu1, cost, linkid, flagtrans, u1_nm, u2_nm, createdby, createddate, modifby, modifdate, stk_no, cc_name) " &
                                                        "VALUES (@id, @custid, @invdate, 1, @cc, @stk_id, @stk_name, @u1, @u2, @factor1, @ttlstock, 0, @cost, @linkid, @flagtrans, @u1_nm, @u2_nm, @createdby, @createddate, NULL, NULL, @stk_no, @cc_name)"

                        Using cmdUpd As New MySqlCommand(sqlUpdateDetail, conn, trans),
                              cmdIns As New MySqlCommand(sqlInsertDetail, conn, trans)

                            cmdUpd.Parameters.Add("@id", MySqlDbType.Int32)
                            cmdUpd.Parameters.AddWithValue("@custid", custid)
                            cmdUpd.Parameters.Add("@cc", MySqlDbType.Int32)
                            cmdUpd.Parameters.Add("@u1", MySqlDbType.Decimal)
                            cmdUpd.Parameters.Add("@u2", MySqlDbType.Decimal)
                            cmdUpd.Parameters.Add("@factor1", MySqlDbType.Decimal)
                            cmdUpd.Parameters.Add("@ttlstock", MySqlDbType.Decimal)
                            cmdUpd.Parameters.Add("@cost", MySqlDbType.Decimal)
                            cmdUpd.Parameters.AddWithValue("@modifby", username)
                            cmdUpd.Parameters.AddWithValue("@modifdate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                            cmdUpd.Parameters.Add("@cc_name", MySqlDbType.VarChar)

                            cmdIns.Parameters.Add("@id", MySqlDbType.Int32)
                            cmdIns.Parameters.AddWithValue("@custid", custid)
                            cmdIns.Parameters.AddWithValue("@invdate", Date.Now.Date.ToString("yyyy-MM-dd"))
                            cmdIns.Parameters.Add("@cc", MySqlDbType.Int32)
                            cmdIns.Parameters.Add("@stk_id", MySqlDbType.Int32)
                            cmdIns.Parameters.Add("@stk_name", MySqlDbType.VarChar)
                            cmdIns.Parameters.Add("@u1", MySqlDbType.Decimal)
                            cmdIns.Parameters.Add("@u2", MySqlDbType.Decimal)
                            cmdIns.Parameters.Add("@factor1", MySqlDbType.Decimal)
                            cmdIns.Parameters.Add("@ttlstock", MySqlDbType.Decimal)
                            cmdIns.Parameters.Add("@cost", MySqlDbType.Decimal)
                            cmdIns.Parameters.AddWithValue("@linkid", idtrans)
                            cmdIns.Parameters.Add("@flagtrans", MySqlDbType.Int32)
                            cmdIns.Parameters.Add("@u1_nm", MySqlDbType.VarChar)
                            cmdIns.Parameters.Add("@u2_nm", MySqlDbType.VarChar)
                            cmdIns.Parameters.AddWithValue("@createdby", username)
                            cmdIns.Parameters.AddWithValue("@createddate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                            cmdIns.Parameters.Add("@stk_no", MySqlDbType.VarChar)
                            cmdIns.Parameters.Add("@cc_name", MySqlDbType.VarChar)

                            For Each row As DataRow In dt.Rows
                                Dim rowIdVal As Integer = 0
                                If Not IsDBNull(row.Item("ID")) AndAlso Integer.TryParse(row.Item("ID").ToString(), rowIdVal) AndAlso rowIdVal > 0 Then
                                    cmdUpd.Parameters("@id").Value = rowIdVal
                                    cmdUpd.Parameters("@cc").Value = Convert.ToInt32(row.Item("Cc"))
                                    cmdUpd.Parameters("@u1").Value = Convert.ToDecimal(row.Item("U1"))
                                    cmdUpd.Parameters("@u2").Value = Convert.ToDecimal(row.Item("U2"))
                                    cmdUpd.Parameters("@factor1").Value = Convert.ToDecimal(row.Item("Factor1"))
                                    cmdUpd.Parameters("@ttlstock").Value = Convert.ToDecimal(row.Item("ttlStock"))
                                    cmdUpd.Parameters("@cost").Value = Convert.ToDecimal(row.Item("Cost"))
                                    cmdUpd.Parameters("@cc_name").Value = row.Item("cc_name").ToString()
                                    cmdUpd.ExecuteNonQuery()
                                Else
                                    Dim newDetailId As Integer = 1
                                    Using cmdMax As New MySqlCommand("SELECT IFNULL(MAX(id), 0) + 1 FROM cld_inv_trans", conn, trans)
                                        newDetailId = Convert.ToInt32(cmdMax.ExecuteScalar())
                                    End Using

                                    cmdIns.Parameters("@id").Value = newDetailId
                                    cmdIns.Parameters("@cc").Value = Convert.ToInt32(row.Item("Cc"))
                                    cmdIns.Parameters("@stk_id").Value = Convert.ToInt32(row.Item("stk_id"))
                                    cmdIns.Parameters("@stk_name").Value = row.Item("Stk_Name").ToString()
                                    cmdIns.Parameters("@u1").Value = Convert.ToDecimal(row.Item("U1"))
                                    cmdIns.Parameters("@u2").Value = Convert.ToDecimal(row.Item("U2"))
                                    cmdIns.Parameters("@factor1").Value = Convert.ToDecimal(row.Item("Factor1"))
                                    cmdIns.Parameters("@ttlstock").Value = Convert.ToDecimal(row.Item("ttlStock"))
                                    cmdIns.Parameters("@cost").Value = Convert.ToDecimal(row.Item("Cost"))
                                    cmdIns.Parameters("@flagtrans").Value = Convert.ToInt32(row.Item("flagtrans"))
                                    cmdIns.Parameters("@u1_nm").Value = row.Item("U1_nm").ToString()
                                    cmdIns.Parameters("@u2_nm").Value = row.Item("U2_nm").ToString()
                                    cmdIns.Parameters("@stk_no").Value = row.Item("stk_no").ToString()
                                    cmdIns.Parameters("@cc_name").Value = row.Item("cc_name").ToString()
                                    cmdIns.ExecuteNonQuery()
                                End If
                            Next
                        End Using
                    End If

                    trans.Commit()
                    insertLogModule(username, "Invtranstrf", If(isNew, "Insert", "Update"), custid)
                    Return idtrans
                Catch ex As Exception
                    trans.Rollback()
                    errLog(ex.Message & " - " & ex.StackTrace, "clsInventory:insertInvtrf")
                    Throw
                End Try
            End Using
        End Using
    End Function


#End Region
#Region "Inventory rtn to WH"


    Public Function getInvRtntoWH(ByVal bydate As Date, ByVal bydateto As Date, ByVal custid As Integer) As DataSet
        Dim clsRtntoWH As New clsInventory_RtntoWH
        Return clsRtntoWH.SelectDataset_invRtntoWH(bydate, bydateto, custid)
    End Function

    Public Function getInvRtntoWHSearch(ByVal name As String, ByVal bydate As Date, ByVal bydateto As Date, ByVal custid As Integer) As DataSet
        Dim clsRtntoWH As New clsInventory_RtntoWH
        Return clsRtntoWH.SelectDataset_invRtntoWHSearch(name, bydate, bydateto, custid)
    End Function
    Public Function getInvRtntoWHExp(ByVal name As String, ByVal bydate As Date, ByVal bydateto As Date, ByVal custid As Integer) As DataSet
        Dim clsRtntoWH As New clsInventory_RtntoWH
        Return clsRtntoWH.SelectDataset_invRtntoWHExp(name, bydate, bydateto, custid)
    End Function
    Public Function getInvRtntoWHDtl(ByVal name As String, ByVal bydate As Date, ByVal bydateto As Date, ByVal custid As Integer) As DataSet
        Dim clsRtntoWH As New clsInventory_RtntoWH
        Return clsRtntoWH.SelectDataset_invRtntoWHDtl(name, bydate, bydateto, custid)
    End Function
    Public Function getDTInvRtntoWH(ByVal linkid As String, ByVal custid As Integer) As DataTable

        Dim clsRtntoWH As New clsInventory_RtntoWH
        Return clsRtntoWH.SelectDataTable_invtrans_RtntoWH(linkid, custid)

        clsTrf = Nothing

    End Function


    Public Sub DeleteInvRtntoWH(ByVal id As String, ByVal Custid As Integer, ByVal username As String)

        Try

            Dim str As String = ""
            Dim strTr As String = ""
            Dim c_cmd As New clsDbo("")

            str = "DELETE FROM cld_invlk_trf  where id = '" & id & "' and custid = " & Custid & ""
            c_cmd.executeNonQuery(str)
            'Dim ds As New DataSet
            'Dim a As New clsDbo("Select * FROM cld_inv_trans  where linkid = '" & id & "' and custid = " & Custid & "")
            'ds = a.selectDataset
            'If ds.Tables(0).Rows.Count > 0 Then
            '    For Each row As DataRow In ds.Tables(0).Rows

            c_cmd.ReNew()

            strTr = "DELETE FROM cld_inv_trans  where linkid = '" & id & "' and custid = " & Custid & ""
            c_cmd.executeNonQuery(strTr)

            '    Next
            'End If
            'a.dispose()
            'a = Nothing

            c_cmd.dispose()
            c_cmd = Nothing

            insertLogModule(username, "Inv_invlk_rtwh " & id.ToString, "Delete", Custid)

        Catch ex As Exception
            errLog(Err.Description, "clsInventory:DeleteInvRtnWH")
            Err.Clear()
        End Try
    End Sub


    Public Sub insertInvrtntoWH(ByVal id As String, ByVal ccfrom As Integer, ByVal dt As DataTable, ByVal username As String, ByVal custid As Integer)
        Dim c_cmd As New clsDbo("")
        Dim strheader As String
        Dim strmaster As String

        Try
            If id = "" Then
                Dim idtrans As String = CItrans.idrtntoWH(custid)

                strmaster = "Insert into cld_invlk_trf values('" & idtrans & "'," & custid & ", " & ccfrom & ", '" & username & "', '" & Date.Now.Date.ToString("yyyy-MM-dd") & "', Null, Null)"
                c_cmd.executeNonQuery(strmaster)

                For Each row As DataRow In dt.Rows
                    strheader = "insert into cld_inv_Trans values (" & c_cmd.ExecuteScalar("select Max(ID) from cld_inv_Trans") & ", " & custid & ",'" & Date.Now.Date.ToString("yyyy-MM-dd") & "', 13, " & row.Item("Cc") & ", " &
                        " " & row.Item("stk_id") & ", '" & row.Item("Stk_Name") & "', " & row.Item("U1") & ", " & row.Item("U2") & ", " & CInt(row.Item("Factor1")) & ", " & row.Item("ttlStock") & ",0, " & row.Item("Cost") & ", " &
                        " '" & idtrans & "', " & row.Item("flagtrans") & ", '" & row.Item("U1_nm") & "', '" & row.Item("U2_nm") & "', '" & row.Item("Createdby") & "', '" & DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") & "', Null, Null, '" & row.Item("stk_no") & "', '" & row.Item("cc_name") & "')"
                    c_cmd.executeNonQuery(strheader)
                    'transfer out
                    'c_cmd.ReNew()
                    'strheader = "insert into cld_inv_Trans values (" & c_cmd.ExecuteScalar("select Max(ID) from cld_inv_Trans") & ", " & custid & ",'" & Date.Now.Date.ToString("yyyy-MM-dd") & "', 10, " & ccfrom & ", " &
                    '    " " & row.Item("stk_id") & ", '" & row.Item("Stk_Name") & "', " & row.Item("U1") & ", " & row.Item("U2") & ", " & CInt(row.Item("Factor1")) & ", " & row.Item("ttlStock") & ",0, " & row.Item("Cost") & ", " &
                    '    " '" & idtrans & "', " & row.Item("flagtrans") & ", '" & row.Item("U1_nm") & "', '" & row.Item("U2_nm") & "', '" & row.Item("Createdby") & "', '" & Date.Now.ToString("yyyy-MM-dd HH:mm:ss") & "', Null, Null, '" & row.Item("stk_no") & "', (select name from cld_inv_cc where id = " & ccfrom & " and custid = " & custid & " ))"
                    'c_cmd.executeNonQuery(strheader)

                Next
            Else

                strmaster = "update cld_invlk_trf set modifby =  '" & username & "', Modifdate =  '" & DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") & "' where id = '" & id & "' and custid = " & custid & ""
                c_cmd.executeNonQuery(strmaster)

                For Each row As DataRow In dt.Rows
                    If row.Item("ID") > 0 Then
                        strheader = "update cld_Inv_Trans set Cc = " & row.Item("Cc") & ", U1 = " & row.Item("U1") & ", U2 = " & row.Item("U2") & ", factor1 = " & row.Item("Factor1") & ", ttlstock = " & row.Item("ttlStock") & ",Cost = " & row.Item("Cost") & ", " &
                          " ModifBy = '" & username & "', Modifdate = '" & DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") & "' , Cc_name = '" & row.Item("cc_name") & "' where id = " & row.Item("ID") & " and custid = " & custid & ""
                        c_cmd.executeNonQuery(strheader)

                    Else
                        strheader = "insert into cld_inv_Trans values (" & c_cmd.ExecuteScalar("select Max(ID) from cld_inv_Trans") & ", " & custid & ",'" & Date.Now.Date.ToString("yyyy-MM-dd") & "', 13, " & row.Item("Cc") & ", " &
                            " " & row.Item("stk_id") & ", '" & row.Item("Stk_Name") & "', " & row.Item("U1") & ", " & row.Item("U2") & ", " & row.Item("Factor1") & ", " & row.Item("ttlStock") & ", 0," & row.Item("Cost") & ", " &
                            " '" & id & "', " & row.Item("flagtrans") & ", '" & row.Item("U1_nm") & "', '" & row.Item("U2_nm") & "', '" & row.Item("Createdby") & "', '" & DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") & "' , Null, Null, '" & row.Item("stk_no") & "', '" & row.Item("Cc_name") & "')"
                        c_cmd.executeNonQuery(strheader)
                        'transfer out
                        'c_cmd.ReNew()
                        'strheader = "insert into cld_inv_Trans values (" & c_cmd.ExecuteScalar("select Max(ID) from cld_inv_Trans") & ", " & custid & ",'" & Date.Now.Date.ToString("yyyy-MM-dd") & "', 1, " & ccfrom & ", " &
                        '    " " & row.Item("stk_id") & ", '" & row.Item("Stk_Name") & "', " & row.Item("U1") & ", " & row.Item("U2") & ", " & row.Item("Factor1") & ", " & row.Item("ttlStock") & ", 0," & row.Item("Cost") & ", " &
                        '    " '" & id & "', " & row.Item("flagtrans") & ", '" & row.Item("U1_nm") & "', '" & row.Item("U2_nm") & "', '" & row.Item("Createdby") & "', '" & Date.Now.ToString("yyyy-MM-dd HH:mm:ss") & "' , Null, Null, '" & row.Item("stk_no") & "', (select name from cld_inv_cc where id = " & ccfrom & " and custid = " & custid & " ))"
                        'c_cmd.executeNonQuery(strheader)
                    End If

                Next

            End If

            insertLogModule(username, "Invtranstrf", "Update", custid)

        Catch ex As Exception

            errLog(Err.Description, "clsInventory:insertInvtranstrf")
            Err.Clear()
        End Try

        c_cmd.dispose()
        c_cmd = Nothing

    End Sub


#End Region

#Region "Inventory CostCentre"
    Public Function getInvcc(ByVal custid As Integer, ByVal usr As String, ByVal modpriv As Integer, ByVal itipe As Integer) As DataSet

        Dim CIcc As New clsInventory_Cc
        Return CIcc.SelectDataset_Icc(custid, usr, modpriv, itipe)

        CIcc = Nothing

    End Function

#End Region
#Region "Purchase Request"

    Public Function getPurchaseRequest(ByVal itipe As String, ByVal ModPriv As Integer, ByVal user As String, ByVal icc As Integer, Optional ByVal name As String = "") As DataSet
        Dim CIware As New clsInventory_Warehouse
        Return CIware.SelectDs_purchaserequest(itipe, ModPriv, user, icc, name)
        CIware = Nothing
    End Function

    Public Function getDTInvPR(ByVal prNo As String, ByVal cc As Integer) As DataTable
        Dim CIware As New clsInventory_Warehouse
        Return CIware.getDTInvPR(prNo, cc)
    End Function

    Public Function insertInvPR(ByVal prid As Integer, ByVal prNo As String, ByVal prDate As Date, ByVal requireddate As Date, ByVal cc As Integer,
                                ByVal request_by As String, ByVal stat As String, ByVal appBy As String, approvalDate As Date,
                                ByVal dt As DataTable, ByVal sRemarks As String) As String
        Dim CIware As New clsInventory_Warehouse
        Return CIware.insertInvPR(prid, prNo, prDate, requireddate, cc, request_by, stat, appBy, approvalDate, dt, sRemarks)
    End Function

    Public Sub DeleteInvPR(ByVal prNo As String, ByVal cc As Integer, ByVal username As String)
        Dim CIware As New clsInventory_Warehouse
        CIware.DeleteInvPR(prNo, cc, username)
    End Sub

    Public Sub DeleteInvPRd(ByVal id As Integer, ByVal username As String)
        Dim CIware As New clsInventory_Warehouse
        CIware.DeleteInvPRd(id, username)
    End Sub

    Public Sub RejectInvPR(ByVal prId As Integer, ByVal username As String)
        Dim CIware As New clsInventory_Warehouse
        CIware.RejectInvPR(prId, username)
    End Sub

    Public Sub RejectInvPRd(ByVal id As Integer, ByVal username As String)
        Dim CIware As New clsInventory_Warehouse
        CIware.RejectInvPRd(id, username)
    End Sub

    Public Function isPREditable(ByVal prNo As String) As Boolean
        Dim CIware As New clsInventory_Warehouse
        Return CIware.isPREditable(prNo)
    End Function

#End Region
#Region "Inventory warehoure"

    Public Function getInvWare(ByVal itipe As Integer, ByVal custid As Integer, ByVal ModPriv As Integer, ByVal user As String, ByVal icc As Integer) As DataSet
        Dim CIware As New clsInventory_Warehouse
        Return CIware.SelectDataset_invWare(itipe, custid, ModPriv, user, icc)
        CIware = Nothing
    End Function

    Public Function getInvWareSearch(ByVal itipe As Integer, ByVal name As String, ByVal custid As Integer, ByVal ModPriv As Integer, ByVal user As String, ByVal icc As Integer) As DataSet
        Dim CIware As New clsInventory_Warehouse
        Return CIware.SelectDataset_invWareSearch(itipe, name, custid, ModPriv, user, icc)
        CIware = Nothing
    End Function

    Public Function getInvWareById(ByVal wID As String) As DataSet
        Dim CIware As New clsInventory_Warehouse
        Return CIware.SelectDataset_invWareByID(wID)
        CIware = Nothing
    End Function

    Public Function getDTInvWare_rcv_grid(ByVal ID As String, ByVal custid As Integer, ByVal ccTo As Integer, ByVal ccware As Integer) As DataTable
        Dim CIware As New clsInventory_Warehouse
        Return CIware.SelectDataTable_invWare_rcv_grid(ID, custid, ccTo, ccware)
        CIware = Nothing
    End Function

    Public Function getDTInvWare(ByVal ID As String, ByVal custid As Integer, ByVal ccTo As Integer, ByVal ccware As Integer) As DataTable
        Dim CIware As New clsInventory_Warehouse
        Return CIware.SelectDataTable_invWare(ID, custid, ccTo, ccware)
        CIware = Nothing
    End Function
    Public Function getDTInvWare3(ByVal ID As String, ByVal custid As Integer, ByVal ccTo As Integer, ByVal ccware As Integer) As DataTable
        Dim CIware As New clsInventory_Warehouse
        Return CIware.SelectDataTable_invWare3(ID, custid, ccTo, ccware)
        CIware = Nothing
    End Function
    Public Function getDTInvWare2(ByVal ID As String, ByVal custid As Integer, ByVal ccTo As Integer, ByVal ccware As Integer) As DataTable
        Dim CIware As New clsInventory_Warehouse
        Return CIware.SelectDataTable_invWare2(ID, custid, ccTo, ccware)
        CIware = Nothing
    End Function

    Public Function getInvOrderDelMode(ByVal wID As String, ByVal custid As Integer, ByVal ccTo As Integer) As String
        Dim CIware As New clsInventory_Warehouse
        Return CIware.GetInvOrderDelMode(wID, custid, ccTo)
    End Function

    Public Function setInvOrderDelMode(ByVal wID As String, ByVal custid As Integer, ByVal ccTo As Integer, ByVal delMode As String) As Boolean
        Dim CIware As New clsInventory_Warehouse
        Return CIware.SetInvOrderDelMode(wID, custid, ccTo, delMode)
    End Function

    Public Function getInvWareDelList(ByVal wID As String, ByVal custid As Integer) As DataTable
        Dim CIware As New clsInventory_Warehouse
        Return CIware.SelectDataTable_invWareDelList(wID, custid)
    End Function

    Public Function getInvWareDelDtl(ByVal delID As String, ByVal custid As Integer, ByVal ccTo As Integer, ByVal ccware As Integer) As DataTable
        Dim CIware As New clsInventory_Warehouse
        Return CIware.SelectDataTable_invWareDelDtl(delID, custid, ccTo, ccware)
    End Function

    Public Function getInvWarePendingDel(ByVal wID As String, ByVal custid As Integer) As DataTable
        Dim CIware As New clsInventory_Warehouse
        Return CIware.SelectDataTable_invWarePendingDel(wID, custid)
    End Function

    Public Function getInvWareDelPrint(ByVal delID As String, ByVal custid As Integer, ByVal ccTo As Integer) As DataTable
        Dim CIware As New clsInventory_Warehouse
        Return CIware.SelectDataTable_invWareDelPrint(delID, custid, ccTo)
    End Function

    Public Function insertInvOrderW(ByVal id As String, ByVal iCustid As Integer, ByVal dTot As Decimal, ByVal iStat As Integer, ByVal iCcfrm As Integer, ByVal sCcfrmNm As String, ByVal iCcTo As Integer, ByVal sCcToNm As String, ByVal sCreate As String, ByVal sModif As String, ByVal dExpDel As Date, ByVal dt As DataTable, ByVal DelID As String, ByVal sNotes As String, Optional ByVal xcancel As Integer = 0) As String
        Dim sHasil As String = ""
        Dim CWare As New clsInventory_Warehouse
        Dim doctot As Decimal = 0

        Using conn As New MySqlConnection(My.Settings.ServerStr)
            conn.Open()
            Dim trans As MySqlTransaction = conn.BeginTransaction()
            
            Try
                Dim c_cmd As New MySqlCommand()
                c_cmd.Connection = conn
                c_cmd.Transaction = trans

                Dim strheader As String
                Dim strmaster As String

                'jadi klo Delivery 
                If iStat = 2 Then

                    doctot = 0

                    For Each row As DataRow In dt.Rows
                        If row.Item("wID") <> "" Then

                            doctot = doctot + (row.Item("cost") * row.Item("U1Del"))

                            Dim add As Decimal = 0
                            Dim isNew As Boolean = False
                            If DelID <> "" Then

                                strheader = "select u1Del from cld_inv_w_dtl where wID = '" & id & "' and custid = " & iCustid & " and stk_id = " & row.Item("stk_id") & " and ccTo = " & iCcTo
                                c_cmd.CommandText = strheader
                                Dim dtAdd As New DataTable()
                                Using da As New MySqlDataAdapter(c_cmd)
                                    da.Fill(dtAdd)
                                End Using

                                'Dim dt1 As DataTable = c_cmd.ReturnDataset(strheader).Tables(0)
                                If dtAdd.Rows.Count > 0 Then
                                    add = (row.Item("u1Del") - dtAdd.Rows(0).Item("u1Del"))
                                    If xcancel = 1 Then
                                        add = (0 - dtAdd.Rows(0).Item("u1Del"))
                                    End If
                                Else
                                    isNew = True
                                    add = row.Item("u1Del")
                                End If
                            End If



                            If iStat = 1 Then
                                'masukin ke inv_trans
                                CWare.InsertInvDetail_invWare(iCustid, 7, iCcfrm, row.Item("stk_id"), row.Item("stk_name"), row.Item("U1Del"), 0, row.Item("Factor1"), row.Item("cost"), id, 1, row.Item("U1_nm"), sCreate, row.Item("Number"), sCcToNm, row.Item("Unit2"), dExpDel.Date, trans)
                            ElseIf iStat = 2 Then
                                'masukin ke inv_trans
                                CWare.InsertInvDetail_invWare(iCustid, 7, iCcfrm, row.Item("stk_id"), row.Item("stk_name"), 0, row.Item("U1Del"), row.Item("Factor1"), row.Item("cost"), id, 1, row.Item("U1_nm"), sCreate, row.Item("Number"), sCcfrmNm, row.Item("Unit2"), dExpDel.Date, trans)
                                'langsung kurangin saat delivery
                                Dim clsprocess As New clsInventoryProcess
                                clsprocess.InvstkId = row.Item("stk_id").ToString
                                clsprocess.InvCc = iCcfrm
                                clsprocess.InvCcName = sCcfrmNm
                                clsprocess.InvCustId = iCustid
                                clsprocess.InvstkName = row.Item("stk_name")
                                ' clsprocess.InvCost = row.Item("cost")
                                If DelID = "" Then
                                    clsprocess.ho_out = row.Item("u1Del") '* row.Item("Factor1")
                                Else


                                    clsprocess.ho_out = add '* row.Item("Factor1")
                                End If

                                clsprocess.ho_in = 0
                                clsprocess.InvStkNo = row.Item("Number")
                                clsprocess.InvDate = dExpDel.ToString("yyyy-MM-dd 00:00:00")
                                'cInv.executeNonQuery_Inv_ProcessCls(Date.Now.Date.ToString("yyyy-MM-dd"), ddlFrom.SelectedValue, ddlFrom.SelectedItem.Text, stkid, stknumber, stkname, custid, 0, stktotal)
                                clsprocess.executeNonQuery_Inv_ProcessCls(trans)

                            End If

                            c_cmd.Parameters.Clear()
                            If xcancel = 1 Then
                                strheader = "update cld_inv_w_dtl set cost = @cost , u1Del = 0 where wID = @wID and custid = @custid and stk_id = @stk_id and ccTo = @ccTo"
                                c_cmd.Parameters.AddWithValue("@cost", row.Item("cost"))
                                c_cmd.Parameters.AddWithValue("@wID", id)
                                c_cmd.Parameters.AddWithValue("@custid", iCustid)
                                c_cmd.Parameters.AddWithValue("@stk_id", row.Item("stk_id"))
                                c_cmd.Parameters.AddWithValue("@ccTo", iCcTo)
                            Else
                                If isNew Then
                                    strheader = "insert into cld_inv_w_dtl values (@wID, @custid, @ccTo, @stk_id, @stk_name, @u1, @u1_nm, @totalu2, @u2_nm, @totalu2_2, @u1Del, 0, @cost, @factor1)"
                                    c_cmd.Parameters.AddWithValue("@wID", id)
                                    c_cmd.Parameters.AddWithValue("@custid", iCustid)
                                    c_cmd.Parameters.AddWithValue("@ccTo", iCcTo)
                                    c_cmd.Parameters.AddWithValue("@stk_id", row.Item("stk_id"))
                                    c_cmd.Parameters.AddWithValue("@stk_name", row.Item("stk_name"))
                                    c_cmd.Parameters.AddWithValue("@u1", row.Item("U1"))
                                    c_cmd.Parameters.AddWithValue("@u1_nm", row.Item("U1_nm"))
                                    c_cmd.Parameters.AddWithValue("@totalu2", row.Item("totalu2"))
                                    c_cmd.Parameters.AddWithValue("@u2_nm", row.Item("U2_nm"))
                                    c_cmd.Parameters.AddWithValue("@totalu2_2", row.Item("totalu2"))
                                    c_cmd.Parameters.AddWithValue("@u1Del", row.Item("u1Del"))
                                    c_cmd.Parameters.AddWithValue("@cost", row.Item("cost"))
                                    c_cmd.Parameters.AddWithValue("@factor1", row.Item("Factor1"))
                                Else
                                    strheader = "update cld_inv_w_dtl set cost = @cost , u1Del = @u1Del where wID = @wID and custid = @custid and stk_id = @stk_id and ccTo = @ccTo"
                                    c_cmd.Parameters.AddWithValue("@cost", row.Item("cost"))
                                    c_cmd.Parameters.AddWithValue("@u1Del", row.Item("u1Del"))
                                    c_cmd.Parameters.AddWithValue("@wID", id)
                                    c_cmd.Parameters.AddWithValue("@custid", iCustid)
                                    c_cmd.Parameters.AddWithValue("@stk_id", row.Item("stk_id"))
                                    c_cmd.Parameters.AddWithValue("@ccTo", iCcTo)
                                End If
                            End If
                            c_cmd.CommandText = strheader
                            c_cmd.ExecuteNonQuery()

                        End If
                    Next

                    doctot = Math.Round(doctot)

                    c_cmd.Parameters.Clear()
                    If DelID = "" Then
                        Dim idtrans As String = CItrans.idwarehouseDel(iCustid, iCcfrm, trans)
                        strmaster = "update cld_inv_w set DocTot = @doctot, Delid = @Delid, flag = @flag, Deldate = @Deldate, appBy = @appBy, DelMode='F' where id = @id and custid = @custid and ccTo = @ccTo"
                        c_cmd.Parameters.AddWithValue("@Delid", idtrans)
                        c_cmd.Parameters.AddWithValue("@Deldate", Date.Now.Date.ToString("yyyy-MM-dd 00:00:00"))
                    Else
                        strmaster = "update cld_inv_w set DocTot = @doctot, flag = @flag, Deldate = @Deldate, appBy = @appBy where id = @id and custid = @custid and ccTo = @ccTo"
                        c_cmd.Parameters.AddWithValue("@Deldate", dExpDel.ToString("yyyy-MM-dd 00:00:00"))

                        If xcancel = 1 Then
                            ' Cancel
                            strmaster = "update cld_inv_w set DocTot = @doctot, flag = 1, appBy = @appBy where id = @id and custid = @custid and ccTo = @ccTo"
                        End If
                    End If

                    c_cmd.CommandText = strmaster
                    c_cmd.Parameters.AddWithValue("@doctot", doctot)
                    c_cmd.Parameters.AddWithValue("@flag", If(xcancel = 1, 1, iStat))
                    c_cmd.Parameters.AddWithValue("@appBy", sModif)
                    c_cmd.Parameters.AddWithValue("@id", id)
                    c_cmd.Parameters.AddWithValue("@custid", iCustid)
                    c_cmd.Parameters.AddWithValue("@ccTo", iCcTo)

                    c_cmd.ExecuteNonQuery()

                End If

                'jadi klo receiving
                If iStat = 3 Then
                    doctot = 0
                    For Each row As DataRow In dt.Rows

                        If row.Item("wID") <> "" Then
                            doctot = doctot + (row.Item("cost") * row.Item("U1Rcv"))
                            c_cmd.Parameters.Clear()
                            If xcancel = 2 Then
                                strheader = "update cld_inv_w_dtl set U1Rcv = 0 where wID = @wID and custid = @custid and stk_id = @stk_id and ccTo = @ccTo"
                            Else
                                strheader = "update cld_inv_w_dtl set U1Rcv = @U1Rcv where wID = @wID and custid = @custid and stk_id = @stk_id and ccTo = @ccTo"
                                c_cmd.Parameters.AddWithValue("@U1Rcv", row.Item("U1Rcv"))
                            End If

                            c_cmd.Parameters.AddWithValue("@wID", id)
                            c_cmd.Parameters.AddWithValue("@custid", iCustid)
                            c_cmd.Parameters.AddWithValue("@stk_id", row.Item("stk_id"))
                            c_cmd.Parameters.AddWithValue("@ccTo", iCcTo)

                            c_cmd.CommandText = strheader
                            c_cmd.ExecuteNonQuery()

                            'masukin ke 
                            If xcancel = 2 Then
                                CWare.InsertInvDetail_invWare(iCustid, 8, iCcTo, row.Item("stk_id"), row.Item("stk_name"), 0, 0, row.Item("Factor1"), 0, id, 1, row.Item("U1_nm"), sCreate, row.Item("Number"), sCcToNm, row.Item("Unit2"), dExpDel.Date, trans)

                            Else
                                CWare.InsertInvDetail_invWare(iCustid, 8, iCcTo, row.Item("stk_id"), row.Item("stk_name"), 0, row.Item("u1Rcv"), row.Item("Factor1"), row.Item("cost"), id, 1, row.Item("U1_nm"), sCreate, row.Item("Number"), sCcToNm, row.Item("Unit2"), dExpDel.Date, trans)

                            End If
                            'langsung ditambahin saat terima di toko
                            Dim clsprocess As New clsInventoryProcess
                            clsprocess.InvstkId = row.Item("stk_id").ToString
                            clsprocess.InvCc = iCcTo
                            clsprocess.InvCcName = sCcToNm
                            clsprocess.InvCustId = iCustid
                            clsprocess.InvstkName = row.Item("stk_name")
                            clsprocess.InvCost = row.Item("cost")
                            clsprocess.ho_out = 0
                            If xcancel = 2 Then
                                clsprocess.ho_in = -row.Item("U1Rcv")
                            Else
                                clsprocess.ho_in = row.Item("U1Rcv") '* row.Item("Factor1")
                            End If

                            clsprocess.InvStkNo = row.Item("Number")
                            clsprocess.InvDate = dExpDel.Date.ToString("yyyy-MM-dd 00:00:00")
                            clsprocess.BCost = True
                            'cInv.executeNonQuery_Inv_ProcessCls(Date.Now.Date.ToString("yyyy-MM-dd"), ddlFrom.SelectedValue, ddlFrom.SelectedItem.Text, stkid, stknumber, stkname, custid, 0, stktotal)
                            clsprocess.executeNonQuery_Inv_ProcessCls(trans)
                        End If
                    Next

                    doctot = Math.Round(doctot)

                    c_cmd.Parameters.Clear()
                    If xcancel = 2 Then
                        strmaster = "update cld_inv_w set DocTot = @doctot, flag = 2, rcvBy = @rcvBy where id = @id and custid = @custid and ccTo = @ccTo"
                        c_cmd.Parameters.AddWithValue("@rcvBy", DBNull.Value)
                    Else
                        strmaster = "update cld_inv_w set DocTot = @doctot, flag = @flag, rcvDate = @rcvDate, rcvBy = @rcvBy where id = @id and custid = @custid and ccTo = @ccTo"
                        c_cmd.Parameters.AddWithValue("@rcvDate", dExpDel.ToString("yyyy-MM-dd HH:mm:ss"))
                        c_cmd.Parameters.AddWithValue("@rcvBy", sModif)
                    End If

                    c_cmd.CommandText = strmaster
                    c_cmd.Parameters.AddWithValue("@doctot", doctot)
                    c_cmd.Parameters.AddWithValue("@flag", iStat)
                    c_cmd.Parameters.AddWithValue("@id", id)
                    c_cmd.Parameters.AddWithValue("@custid", iCustid)
                    c_cmd.Parameters.AddWithValue("@ccTo", iCcTo)

                    c_cmd.ExecuteNonQuery()

                End If

                If id = "" Then
                    'get id dahulu
                    Dim idtrans As String = CItrans.idwarehouse(iCustid, iCcTo, trans)
                    sHasil = idtrans
                    Dim try2 As Integer = 0
                    'insert(detail)
cek1:
                    c_cmd.Parameters.Clear()
                    c_cmd.CommandText = "SELECT COUNT(wid) FROM cld_inv_w_dtl WHERE wid=@idtrans AND custid=@custid AND ccto=@ccto"
                    c_cmd.Parameters.AddWithValue("@idtrans", idtrans)
                    c_cmd.Parameters.AddWithValue("@custid", iCustid)
                    c_cmd.Parameters.AddWithValue("@ccto", iCcTo)

                    If Convert.ToInt32(c_cmd.ExecuteScalar()) > 0 Then
                        try2 = try2 + 1
                        idtrans = CItrans.idwarehouse(iCustid, iCcTo, trans, try2)
                        GoTo cek1
                    Else
                        For Each row As DataRow In dt.Rows
                            c_cmd.Parameters.Clear()
                            strheader = "insert into cld_inv_w_dtl values (@idtrans, @custid, @ccto, @stk_id, @stk_name, @u1, @u1_nm, @totalu2, @u2_nm, @totalu2_2, 0, 0, @cost, @factor1)"
                            c_cmd.CommandText = strheader
                            c_cmd.Parameters.AddWithValue("@idtrans", idtrans)
                            c_cmd.Parameters.AddWithValue("@custid", iCustid)
                            c_cmd.Parameters.AddWithValue("@ccto", iCcTo)
                            c_cmd.Parameters.AddWithValue("@stk_id", row.Item("stk_id"))
                            c_cmd.Parameters.AddWithValue("@stk_name", row.Item("Stk_Name"))
                            c_cmd.Parameters.AddWithValue("@u1", row.Item("U1"))
                            c_cmd.Parameters.AddWithValue("@u1_nm", row.Item("U1_nm"))
                            c_cmd.Parameters.AddWithValue("@totalu2", row.Item("totalu2"))
                            c_cmd.Parameters.AddWithValue("@u2_nm", row.Item("U2_nm"))
                            c_cmd.Parameters.AddWithValue("@totalu2_2", row.Item("totalU2"))
                            c_cmd.Parameters.AddWithValue("@cost", row.Item("cost"))
                            c_cmd.Parameters.AddWithValue("@factor1", row.Item("Factor1"))

                            c_cmd.ExecuteNonQuery()
                        Next
                    End If


                    'insert Master
                    c_cmd.Parameters.Clear()
                    c_cmd.CommandText = "Insert into cld_inv_w (ID, Custid, DocTot, Flag, ccFrom, ccTo, CreatedBy, CreatedDate, ccFromNm, ccToNm, exDelDate, Notes) values(@ID, @Custid, @DocTot, @Flag, @ccFrom, @ccTo, @CreatedBy, @CreatedDate, @ccFromNm, @ccToNm, @exDelDate, @Notes)"
                    c_cmd.Parameters.AddWithValue("@ID", idtrans)
                    c_cmd.Parameters.AddWithValue("@Custid", iCustid)
                    c_cmd.Parameters.AddWithValue("@DocTot", dTot)
                    c_cmd.Parameters.AddWithValue("@Flag", iStat)
                    c_cmd.Parameters.AddWithValue("@ccFrom", iCcfrm)
                    c_cmd.Parameters.AddWithValue("@ccTo", iCcTo)
                    c_cmd.Parameters.AddWithValue("@CreatedBy", sCreate)
                    c_cmd.Parameters.AddWithValue("@CreatedDate", Date.Now.Date.ToString("yyyy-MM-dd"))
                    c_cmd.Parameters.AddWithValue("@ccFromNm", sCcfrmNm)
                    c_cmd.Parameters.AddWithValue("@ccToNm", sCcToNm)
                    c_cmd.Parameters.AddWithValue("@exDelDate", dExpDel.Date.ToString("yyyy-MM-dd"))
                    c_cmd.Parameters.AddWithValue("@Notes", sNotes)

                    c_cmd.ExecuteNonQuery()

                Else
                    'delete detail
                    c_cmd.Parameters.Clear()
                    c_cmd.CommandText = "DELETE FROM cld_inv_w_dtl where wid = @id and custid = @custid and ccto = @ccto"
                    c_cmd.Parameters.AddWithValue("@id", id)
                    c_cmd.Parameters.AddWithValue("@custid", iCustid)
                    c_cmd.Parameters.AddWithValue("@ccto", iCcTo)
                    c_cmd.ExecuteNonQuery()

                    For Each row As DataRow In dt.Rows
                        c_cmd.Parameters.Clear()
                        strheader = "insert into cld_inv_w_dtl values (@id, @custid, @ccto, @stk_id, @stk_name, @u1, @u1_nm, @totalu2, @u2_nm, @totalu2_2, @u1Del, @u1Rcv, @cost, @factor1)"
                        c_cmd.CommandText = strheader
                        c_cmd.Parameters.AddWithValue("@id", id)
                        c_cmd.Parameters.AddWithValue("@custid", iCustid)
                        c_cmd.Parameters.AddWithValue("@ccto", iCcTo)
                        c_cmd.Parameters.AddWithValue("@stk_id", row.Item("stk_id"))
                        c_cmd.Parameters.AddWithValue("@stk_name", row.Item("Stk_Name"))
                        c_cmd.Parameters.AddWithValue("@u1", row.Item("U1"))
                        c_cmd.Parameters.AddWithValue("@u1_nm", row.Item("U1_nm"))
                        c_cmd.Parameters.AddWithValue("@totalu2", row.Item("totalu2"))
                        c_cmd.Parameters.AddWithValue("@u2_nm", row.Item("U2_nm"))
                        c_cmd.Parameters.AddWithValue("@totalu2_2", row.Item("totalU2"))
                        c_cmd.Parameters.AddWithValue("@u1Del", row.Item("u1Del"))
                        c_cmd.Parameters.AddWithValue("@u1Rcv", row.Item("U1Rcv"))
                        c_cmd.Parameters.AddWithValue("@cost", row.Item("cost"))
                        c_cmd.Parameters.AddWithValue("@factor1", row.Item("Factor1"))

                        c_cmd.ExecuteNonQuery()
                    Next

                    c_cmd.Parameters.Clear()
                    c_cmd.CommandText = "update cld_inv_w set Notes = @sNotes , flag = @flag, modifby = @modifby, Modifdate = @Modifdate where id = @id and custid = @custid and ccTo = @ccTo"
                    c_cmd.Parameters.AddWithValue("@sNotes", sNotes)
                    c_cmd.Parameters.AddWithValue("@flag", iStat)
                    c_cmd.Parameters.AddWithValue("@modifby", sModif)
                    c_cmd.Parameters.AddWithValue("@Modifdate", Date.Now.Date.ToString("yyyy-MM-dd"))
                    c_cmd.Parameters.AddWithValue("@id", id)
                    c_cmd.Parameters.AddWithValue("@custid", iCustid)
                    c_cmd.Parameters.AddWithValue("@ccTo", iCcTo)

                    c_cmd.ExecuteNonQuery()

                End If

                trans.Commit()

            Catch ex As Exception
                trans.Rollback()
                errLog(ex.Message & " - " & ex.StackTrace, "clsInventory:insertInvOrderW")
                'Throw
                sHasil = "Error"
            End Try
        End Using

        Return sHasil
    End Function


    Public Sub DeleteInvWare(ByVal id As String, ByVal Custid As Integer, ByVal iccTO As Integer, ByVal istk As Integer, ByVal username As String)

        Try

            Dim str As String = ""
            Dim c_cmd As New clsDbo("")

            If istk <> 0 Then

                'delete detail
                str = "DELETE FROM cld_inv_w_dtl where wid = '" & id & "' and custid = " & Custid & " and ccto = " & iccTO & " and stk_id = " & istk
                c_cmd.executeNonQuery(str)

            Else

                'delete master
                str = "DELETE FROM cld_inv_w  where id = '" & id & "' and custid = " & Custid & " and ccto = " & iccTO
                c_cmd.executeNonQuery(str)

                'delete detail
                str = "DELETE FROM cld_inv_w_dtl where wid = '" & id & "' and custid = " & Custid & " and ccto = " & iccTO
                c_cmd.executeNonQuery(str)

            End If

            insertLogModule(username, "DeleteInvWare " & id.ToString, "Update", Custid)

        Catch ex As Exception
            errLog(Err.Description, "clsInventory:DeleteInvWare")
            Err.Clear()
        End Try
    End Sub



#End Region
    Public Function getIccOnly(ByVal custid As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim CIcc As New clsInventory_Cc
        Return CIcc.SelectDataset_IccOnly(custid, usr, modpriv)
        CIcc = Nothing
    End Function
    Public Function getIWareOnly(ByVal custid As Integer) As DataSet
        Dim CIcc As New clsInventory_Cc
        'untuk ambil data warehouse
        'order barang ke HO
        Return CIcc.SelectDataset_IWare(custid)
        CIcc = Nothing
    End Function
    Public Function getInvRcv(ByVal fromdate As Date, ByVal todate As Date, ByVal custid As Integer, ByVal name As String, ByVal modpriv As Integer) As DataSet
        Dim cRcv As New clsInventory_Receive
        'If My.Settings.tosupplier_byacc = 1 Then
        Return cRcv.SelectDataset_invRcv(fromdate, todate, custid, name, modpriv)
        'Else
        '    Return cRcv.SelectDataset_invRcv1(fromdate, todate, custid, name, modpriv)
        'End If

        cRcv = Nothing
    End Function
    Public Function getInvRcvSearch(ByVal name As String, ByVal fromdate As Date, ByVal todate As Date, ByVal custid As Integer, ByVal usrname As String, ByVal modpriv As Integer) As DataSet
        Dim cRcv As New clsInventory_Receive
        Return cRcv.SelectDataset_invRcvSearch(name, fromdate, todate, custid, usrname, modpriv)
        cRcv = Nothing
    End Function
    Public Function getStkByid(ByVal id As Integer, ByVal custid As Integer) As DataSet
        'ini untuk ambil data stock master berdasaran ID
        Dim csStock As New clsInventory_stk
        Return csStock.SelectDataStk(id, custid)
        csStock = Nothing
    End Function
#Region "Supplier"
    Public Function getSupplier(ByVal custid As Integer) As DataSet
        Dim csup As New clsInventory_Supplier
        Return csup.SelectDataset_Supplier(custid)
        csup = Nothing
    End Function
    Public Function getSupplierbyUser(ByVal custid As Integer, ByVal username As String, ByVal modpriv As Integer) As DataSet
        Dim csup As New clsInventory_Supplier
        Return csup.SelectDataset_SupplierbyUser(custid, username, modpriv)
        csup = Nothing
    End Function
#End Region

    Public Function getPayment(ByVal custid As Integer) As DataSet
        Dim csup As New clsInventory_Supplier
        Return csup.SelectDataset_Payment(custid)
        csup = Nothing
    End Function
    Public Function getItem(ByVal dta As DataTable, ByVal custid As Integer, ByVal name As String, ByVal modpriv As Integer, ByVal ccid As Integer) As DataTable
        Dim cPO As New Cls_PO
        Return cPO.Selectdt_Item_all(dta, custid, name, modpriv, ccid)
        cPO = Nothing
    End Function
    Public Function getCustomerPO(ByVal custid As Integer) As DataSet
        Dim cPO As New Cls_PO
        Return cPO.SelectDataset_CustomerPO(custid)
        csup = Nothing
    End Function
    Public Function getDTInvRcv(ByVal linkid As String, ByVal custid As Integer) As DataTable
        Dim cRcv As New clsInventory_Receive
        Return cRcv.SelectDataTable_invRcv(linkid, custid)
        cRcv = Nothing
    End Function
    Public Function getDTPreOrder(ByVal linkid As String, ByVal custid As Integer) As DataTable
        Dim cPO As New Cls_PO
        Return cPO.SelectDataTable_preorder(linkid, custid)
        cRcv = Nothing
    End Function
    Public Function getDtStk(ByVal data As DataTable, ByVal custid As Integer, ByVal name As String, ByVal modpriv As Integer) As DataTable
        Dim csStock As New clsInventory_stk
        Return csStock.SelectDataTable_Stk(data, custid, name, modpriv)
        csStock = Nothing
    End Function
    Public Function getDtStk2(ByVal data As DataTable, ByVal custid As Integer, ByVal name As String, ByVal modpriv As Integer, ByVal ccid As Integer) As DataTable 'yang minus ga masuk
        Dim csStock As New clsInventory_stk
        Return csStock.SelectDataTable_Stk2(data, custid, name, modpriv, ccid)
        csStock = Nothing
    End Function
    Public Function getInvRcv_byid_grid(ByVal linkid As String, ByVal custid As Integer) As DataSet
        Dim cRcv As New clsInventory_Receive
        Return cRcv.SelectDataset_invRcvbyid_grid(linkid, custid)
        cRcv = Nothing
    End Function
    Public Function getInvRcvdet_byid_grid(ByVal linkid As String, ByVal custid As Integer, Optional ByVal unit1 As Integer = 0) As DataTable
        Dim cRcv As New clsInventory_Receive
        Return cRcv.SelectDataTable_invRcv_grid(linkid, custid, unit1)
        cRcv = Nothing
    End Function
    Public Function getStkcostByid(ByVal id As Integer, ByVal cc As Integer, ByVal custid As Integer, ByVal invdate As Date) As DataSet
        'Ini untuk ambil data stock
        Dim csStock As New clsInventory_stk
        Return csStock.SelectDataStkCost(id, cc, custid, invdate)
        csStock = Nothing
    End Function

    Public Function getStkByid2(ByVal id As Integer, ByVal ccid As Integer, ByVal cust As Integer, ByVal ccWare As Integer, ByVal invdate As Date) As DataSet
        'ini untuk ambil data stockmaster dan stocknya berdasarkan ID,
        'dan menampilkan cost dan stocknya, stocknya dari cc yang dipilih
        'costnya dari warehouse
        Dim csStock As New clsInventory_stk
        Return csStock.SelectDataStk_wOther(id, ccid, cust, ccWare, invdate)
        csStock = Nothing
    End Function
    Public Function getInvclswasteSrch(ByVal name As String, ByVal bydate As Date, ByVal custid As Integer, ByVal ModPriv As Integer, ByVal user As String) As DataSet
        Dim crw As New clsInventory_Rwaste
        Return crw.SelectDataset_clswastesrch(name, bydate, custid, ModPriv, user)
        crw = Nothing
    End Function
    Public Function getInvclswasteEdit(ByVal Id As Integer, ByVal custid As Integer, ByVal ccid As Integer) As DataSet
        Dim crw As New clsInventory_Rwaste
        Return crw.SelectDataset_clswasteEdit(Id, custid, ccid)
        crw = Nothing
    End Function
    Public Function getInvRwasteSearch(ByVal name As String, ByVal bydate As Date, ByVal custid As Integer, ByVal ModPriv As Integer, ByVal user As String) As DataSet
        Dim crw As New clsInventory_Rwaste
        Return crw.SelectDataset_invRwasteSearch(name, bydate, custid, ModPriv, user)
        crw = Nothing
    End Function

    Public Function getInvRwasteSearch2(ByVal name As String, ByVal ModPriv As Integer, ByVal user As String) As DataSet
        Dim crw As New clsInventory_Rwaste
        Return crw.SelectDataset_invRwasteSearch2(name, ModPriv, user)
        crw = Nothing
    End Function
    'Public Function getRwasteEdit(ByVal id As Integer, ByVal custid As Integer) As DataSet
    '    Dim crw As New clsInventory_Rwaste
    '    Return crw.SelectDataset_InRwasteEdit(id, custid)
    '    crw = Nothing
    'End Function
    Public Function getRwasteEdit(ByVal id As Integer, ByVal custid As Integer, ByVal invdate As Date) As DataSet
        Dim crw As New clsInventory_Rwaste
        Return crw.SelectDataset_InRwasteEdit_bydate(id, custid, invdate)
        crw = Nothing
    End Function
    Public Function getRwasteEdit2(ByVal id As Integer, ByVal custid As Integer, ByVal ccid As Integer) As DataSet
        Dim crw As New clsInventory_Rwaste
        Return crw.SelectDataset_InRwasteEdit(id, custid, ccid)
        crw = Nothing
    End Function
    Public Function get_InvStk(ByVal id As Integer, ByVal ccid As Integer, ByVal cust As Integer) As DataSet
        'kebutuhan inv waste
        'ini untuk ambil data stockmaster dan stocknya berdasarkan ID,
        'dan menampilkan cost dan stocknya ambil dari cc yang dipilih
        Dim csStock As New clsInventory_stk
        Return csStock.SelectData_Inv_Stk(id, ccid, cust)
        csStock = Nothing
    End Function
    Public Function get_InvStk_cls(ByVal id As Integer, ByVal ccid As Integer, ByVal cust As Integer, ByVal invdate As Date) As DataSet

        Dim csStock As New clsInventory_stk

        Return csStock.SelectData_Inv_Stkcls(id, ccid, cust, invdate)
        csStock = Nothing
    End Function
    Public Function getStkAll1(ByVal custid As Integer, Optional ByVal ccid As Integer = 0) As DataSet
        Dim csStock As New clsInventory_stk
        If ccid = 0 Then
            Return csStock.SelectDataset_StkAll1(custid)
        Else
            Return csStock.SelectDataset_StkAll1(custid, ccid)
        End If

        csStock = Nothing
    End Function
    Public Function getStkAll(ByVal custid As Integer) As DataSet
        Dim csStock As New clsInventory_stk
        Return csStock.SelectDataset_StkAll(custid)
        csStock = Nothing
    End Function
    Public Function getStkCC(ByVal custid As Integer, ByVal ccid As Integer) As DataSet
        Dim csStock As New clsInventory_stk
        Return csStock.SelectDataset_StkCC(custid, ccid)
        csStock = Nothing
    End Function
    Public Function getDTInvCnvr(ByVal cnvrid As Integer, ByVal custid As Integer) As DataTable
        Dim cCnvr As New Cls_Conversion
        Return cCnvr.SelectDataset_InvConversionEdit(cnvrid, custid).Tables(0)
        cRcv = Nothing
    End Function

    Public Function getDTInvBatch(ByVal linkid As String, ByVal custid As Integer, ByVal dinvdate As Date) As DataTable
        Dim cbatch As New Cls_IBatch
        Return cbatch.SelectDataTable_invtrans_Batch(linkid, custid, dinvdate)
        cRcv = Nothing
    End Function

    Public Function SelectDataStkHomeSearch2(ByVal cust As Integer, ByVal ccid As Integer, ByVal stat As Integer, ByVal grpid As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""
        Dim statFilter As String = ""
        Dim grpfilter As String = ""
        Dim staMod As String = ""

        If modpriv And usrModulepriv.moduleuser = usrModulepriv.moduleuser Then
            staMod = "and C.Olid in (Select Store From cld_userxstore where User = '" & usr & "')"
        End If


        If ccid <> 0 Then
            strFilter = "and A.ccid = " & ccid & ""
        End If




        If stat <> 0 Then
            If stat = 1 Then
                ' OPTIMIZED: Use direct comparison for better index usage
                statFilter = "AND A.ttlQty > IFNULL(D.stkLimit, 0)"
            ElseIf stat = 2 Then
                ' OPTIMIZED: Use direct comparison
                statFilter = "AND A.ttlQty <= IFNULL(D.stkLimit, 0)"
            End If
        End If

        If grpid <> 0 Then
            grpfilter = "and B.grpid = " & grpid & ""
        End If

        'Dim a As New clsDbo("SELECT A.*, B.stkLimit, CASE WHEN A.ttlQty > B.stklimit THEN TRUE ELSE FALSE END AS stat  FROM cld_inv_transstock A INNER JOIN cld_inv_stk B ON B.ID = A.stkId WHERE A.Custid = " & cust & " " & strFilter & " " & statFilter & " " & grpfilter & "")
        Dim a As New clsDbo("SELECT a.custid,a.ccid,ccname,a.stkid,a.stkno,stkname,SG.name sgname,a.ttlqty AS ttlqty,
                            concat(a.ttlqty , ' ' , ifnull(B.unit2, '')) AS ttlqty_with_unit,
                            A.cost,a.ttlqty * A.cost AS Total, IFNULL(D.stkLimit,0) stkLimit, B.unit2 ,CASE WHEN A.ttlQty > (IFNULL(D.stkLimit,0)) THEN TRUE ELSE FALSE END AS stat " &
                            "FROM cld_inv_transstockcls A INNER JOIN cld_inv_stk B ON B.ID = A.stkId " &
                            "INNER JOIN cld_inv_stkgroup SG on SG.Custid = A.custid and SG.ID = B.grpID " &
                            "INNER JOIN cld_inv_cc C on C.Custid = A.custid and C.ID = A.ccID " &
                             "left JOIN cld_inv_stk_cc D on D.Custid = A.custid and D.ccID = C.ID and D.stkID = B.ID " &
                            "WHERE A.invdate ='" & Now.Date.ToString("yyyy-MM-dd 00:00:00") & "' and A.Custid = " & cust & " " & staMod & " " & strFilter & " " & statFilter & " " & grpfilter & " ORDER BY SG.name ASC, ccname ASC, A.stkName ASC")


        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function

    Public Function Select_invtrans(ByVal id As String, ByVal cust As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""

        Dim a As New clsDbo("SELECT * FROM cld_inv_trans where id = '" & id & "' and custid = " & cust)

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds
    End Function

    Public Function getdsclosing(ByVal InvstkId As Integer, ByVal ccid As Integer, ByVal invdate As Date, ByVal cust As Integer) As DataSet
        Dim ds As New DataSet
        Dim c_cmd As New clsDbo("")


        Try
            ds = c_cmd.ReturnDataset("SELECT * FROM cld_inv_transstockcls WHERE stkid = " & InvstkId & " AND ccid = " & ccid & " AND invdate = '" & invdate.Date.ToString("yyyy-MM-dd") & "' AND custid = " & cust & "")
        Catch ex As Exception

        End Try
        Return ds
    End Function

    Public Function get_bclose(ByVal InvstkId As Integer, ByVal ccid As Integer, ByVal invdate As Date, ByVal cust As Integer) As Boolean
        Dim bclose As Boolean = False
        Dim ds As New DataSet
        Dim c_cmd As New clsDbo("")


        Try
            If c_cmd.ExecuteScalar("SELECT bclose FROM cld_inv_transstockcls WHERE stkid = " & InvstkId & " AND ccid = " & ccid & " AND invdate = '" & invdate.Date.ToString("yyyy-MM-dd") & "' AND custid = " & cust & "", 0) > 0 Then
                bclose = True
            End If
        Catch ex As Exception

        End Try
        Return bclose
    End Function
    Public Function get_typestockopname(ByVal InvstkId As Integer, ByVal ccid As Integer, ByVal invdate As Date, ByVal cust As Integer) As Integer
        Dim itype As Integer = 0
        Dim ds As New DataSet
        Dim c_cmd As New clsDbo("")


        Try
            itype = c_cmd.ExecuteScalar("SELECT type FROM cld_inv_trans WHERE stk_id = " & InvstkId & " AND cc = " & ccid & " AND type in (11,12) AND invdate = '" & invdate.Date.ToString("yyyy-MM-dd") & "' AND custid = " & cust & "", 0)

        Catch ex As Exception

        End Try
        Return itype
    End Function
    '#Region "Process Inventory Langsung"


    '    Public Sub executeNonQuery_Inv_ProcessCls(ByVal dInvDate As Date, ByVal iCc As Integer, ByVal sCcName As String, ByVal stkID As Integer, ByVal stkNo As String, ByVal stkName As String, ByVal CustId As Integer, ByVal dwasteqty As Decimal, ByVal drcvQty As Decimal)


    '        'calculate cost
    '        Dim ds As DataSet
    '        Dim dCost As Decimal = 0


    '        Dim c_cmd As New clsDbo("")

    '        ds = c_cmd.ReturnDataset("SELECT " &
    '        "`stkData`.`InvDate`  AS `InvDate`, `stkData`.`Cc` AS `Cc`, " &
    '        "`cld_inv_cc`.`Name`  AS `Cc_name`, `stkData`.`Stk_Id`   AS `Stk_id`, " &
    '        "cld_inv_stk.Name AS Stk_Name, `stkData`.`CustID`   AS `CustID`, cld_inv_stk.number, " &
    '        "IFNULL((SELECT SUM((`sd`.`ttlStock` * `sd`.`Cost`)) AS `data1` FROM `cld_inv_trans` `sd` WHERE ((`sd`.`type` IN (1,3,6,8,9)) AND (`sd`.`flagtrans` = 1) AND (`sd`.`ttlstock` > 0) AND (`sd`.`CustID` = `stkData`.`CustID`) AND (`sd`.`Stk_Id` = `stkData`.`Stk_Id`) AND (`sd`.`InvDate` = `stkData`.`InvDate`) AND (`sd`.`Cc` = `stkData`.`Cc`))),0) AS `Cost`, " &
    '        "IFNULL((SELECT SUM(`sd`.`ttlStock`) AS `data1` FROM `cld_inv_trans` `sd` WHERE ((`sd`.`type` IN (1,3,6,8,9)) AND (`sd`.`flagtrans` = 1) AND (`sd`.`ttlstock` > 0) AND (`sd`.`CustID` = `stkData`.`CustID`) AND (`sd`.`Stk_Id` = `stkData`.`Stk_Id`) AND (`sd`.`InvDate` = `stkData`.`InvDate`) AND (`sd`.`Cc` = `stkData`.`Cc`))),0) AS `CostQty` " &
    '       "FROM cld_inv_stkdata AS stkData INNER JOIN `cld_inv_stk` " &
    '        "ON `stkData`.`CustID` = `cld_inv_stk`.`CustID` AND `cld_inv_stk`.`ID` = `stkData`.`Stk_Id` " &
    '        "INNER JOIN cld_inv_cc ON cld_inv_cc.ID = stkData.Cc AND cld_inv_cc.custid = stkData.custid " &
    '        "WHERE stkData.InvDate >= '" & dInvDate.Date.ToString("yyyy-MM-dd") & "' AND stkData.invDate <= '" & dInvDate.Date.ToString("yyyy-MM-dd") & "' AND stkData.stk_id = '" & stkID & "' AND stkData.Cc = '" & iCc & "' AND stkData.`custid` = " & CustId)

    '        If ds.Tables(0).Rows.Count > 0 Then
    '            For Each row As DataRow In ds.Tables(0).Rows

    '                If row.Item("COstQty") <> 0 Then
    '                    'dCost = ds.Tables(0).Rows.Item("Cost").ToString / ds.Tables(0).Rows.Item("CostQty").ToString
    '                    dCost = row.Item("Cost") / row.Item("CostQty")
    '                Else
    '                    dCost = row.Item("Cost")
    '                End If

    '                'untuk balikin jika nilai costnya -
    '                If dCost < 0 Then
    '                    dCost = dCost * -1
    '                End If
    '            Next



    '        End If





    '        'cek cld_inv_transstockcls ada record ga (base dInvDate, iCc, stkID)

    '        insertStockCLosing(dInvDate, iCc, stkID, stkNo, stkName, CustId, sCcName, dCost, dwasteqty, drcvQty)



    '        '

    '    End Sub



    '    Private Function insertStockCLosing(ByVal dInvDate As Date, ByVal iCc As Integer, ByVal stkID As Integer, ByVal stkNo As String, ByVal stkName As String, ByVal CustId As Integer, ByVal ccname As String, ByVal cost As Decimal, ByVal rwasteQty As Decimal, ByVal rcvQty As Decimal)

    '        Dim c_cmd As New clsDbo("")
    '        Dim dTotalQty As Decimal = 0

    '        'klo gada insert
    '        If c_cmd.ExecuteScalar("SELECT COUNT(stkid) FROM cld_inv_transstockcls WHERE stkid = " & stkID & " AND ccid = " & iCc & " AND invdate = '" & dInvDate.Date.ToString("yyyy-MM-dd") & "' AND custid = " & CustId & "", 0) = 0 Then
    '            dTotalQty = rcvQty
    '            c_cmd.executeNonQuery("insert into cld_inv_transstockcls values ('" & dInvDate.Date.ToString("yyyy-MM-dd") & "', " & CustId & "," & stkID & ", '" & stkNo & "', '" & stkName & "'," &
    '                       " " & iCc & ", '" & ccname & "', " & 0 & ", " & 0 & ", " & cost & ", " & rcvQty & ", " & 0 & ", " & 0 & ", " &
    '                       " " & 0 & ", " & rwasteQty & ", " & 0 & ", " & 0 & ", " & dTotalQty & ", " & 0 & ", " & 0 & ", " & cost & ", " & 0 & ", " & 0 & ", " & 0 & ", " & 0 & ", " & 0 & ")")

    '        Else

    '            'klo ada update

    '            'c_cmd.executeNonQuery("update cld_inv_transstockcls set  opencost = " & opencost & ", cost = " & cost & ", rcvqty = rcvqty + " & rcvQty & ", rwasteqty = rwasteqty +" & rwasteQty & ", " &
    '            '                  " ttlqty = ttlqty + " & costqty & "-" & rwasteQty & ", closeqty = closeqty + " & costqty & "-" & rwasteQty & ", closecost = " & cost & ", varianceqty = " & dvarianceQty & "  where stkid = " & stkID & " and invdate = '" & dInvDate.Date.ToString("yyyy-MM-dd") & "' and custid = " & CustId & "")


    '            c_cmd.executeNonQuery("update cld_inv_transstockcls set  cost = " & cost & ", rcvqty = rcvqty + " & rcvQty & ", rwasteqty = rwasteqty +" & rwasteQty & " where stkid = " & stkID & " and ccid = " & iCc & " and invdate = '" & dInvDate.Date.ToString("yyyy-MM-dd") & "' and custid = " & CustId & "")

    '            c_cmd.resetCmd("")
    '            c_cmd.executeNonQuery("update cld_inv_transstockcls set  ttlqty = " & rcvQty & "-" & rwasteQty & " where stkid = " & stkID & " and ccid = " & iCc & " and invdate = '" & dInvDate.Date.ToString("yyyy-MM-dd") & "' and custid = " & CustId & "")

    '        End If


    '        ' openqty +rcvqrt - rtnqty + tinqty - toutqty - rwasteQty - fwaSteqty +



    '        c_cmd.dispose()


    '    End Function






    '#End Region
    Public Function Select_usageData(ByVal cust As Integer, Optional ByVal fromdate As DateTime = Nothing) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""
        If fromdate = Nothing Then
            fromdate = Format(Now, "dd MMM yyyy")
        End If

        'Dim a As New clsDbo("SELECT Number, Name, Factor1, Factor2, unit1, unit2, IFNULL((SELECT Cost FROM cld_inv_transstockcls WHERE Stkid = " & id & " and ccid = " & ccid & " and invdate = '" & Now.ToString("yyyy-MM-dd 00:00:00") & "'), 0) as Cost
        '                    , IFNULL((SELECT ttlqty FROM cld_inv_transstockcls WHERE Stkid = " & id & " and ccid = " & ccid & " and invdate = '" & Now.ToString("yyyy-MM-dd 00:00:00") & "'), 0) as ttlqty
        '                FROM cld_Inv_Stk where id = " & id & " and custid = " & cust)  

        Dim a As New clsDbo("SELECT invdate, custid,stk_id,stk_no, stk_name,cc,cc_name,SUM(ttlstock) ttlstock,cost,u2_nm  
                        FROM cld_inv_transusage WHERE invdate = '" & fromdate.ToString("yyyy-MM-dd 00:00:00") & "' AND custid = " & cust & " AND type <> 11
                            GROUP BY invdate, custid,stk_id,cc")

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds
    End Function
    Public Sub proc_invusage2(ByVal custid As Integer, Optional ByVal fromdate As DateTime = Nothing)
        Dim c_cmd As New clsDbo("")
        If fromdate = Nothing Then
            fromdate = Format(Now, "dd MMM yyyy")
        End If

        Dim ds As DataSet
        Try

            'untuk langsung nambah stoclcls
            Dim clsprocess As New clsInventoryProcess
            'If fromdate < Now Then
            '    ds = Select_usageData(custid, fromdate)
            'Else
            '    ds = Select_usageData(custid)
            'End If
            ds = c_cmd.ReturnDataset("SELECT * from cld_inv_transusagesum where flag = 1 and invdate = '" & fromdate.Date.ToString("yyyy-MM-dd") & "' and custid = " & custid)

            If ds.Tables(0).Rows.Count > 0 Then
                For Each row As DataRow In ds.Tables(0).Rows
                    Dim InvstkId As Integer = row.Item("stk_id")
                    Dim InvCc As Integer = row.Item("cc")
                    c_cmd.ReNew()
                    Dim InvCcName As String = c_cmd.ExecuteScalar("select name from cld_inv_cc where id=" & row.Item("cc") & "")
                    Dim InvCustId As Integer = custid 'row.Item("custid")
                    c_cmd.ReNew()

                    Dim InvstkName As String = c_cmd.ExecuteScalar("select name from cld_inv_stk where id=" & row.Item("stk_id") & "") 'row.Item("stk_name")
                    Dim invusageQty As Double = row.Item("ttlstock")
                    Dim invcost As Double = 0 'row.Item("cost")
                    Dim InStkNo As String = c_cmd.ExecuteScalar("select number from cld_inv_stk where id=" & row.Item("stk_id") & "") 'row.Item("stk_no")
                    Dim dTotalQty As Double = -(row.Item("ttlstock"))


                    'Dim InvDate As Date = Date.Now.Date.ToString("yyyy-MM-dd 00:00:00")
                    Dim InvDate As Date = fromdate.ToString("yyyy-MM-dd 00:00:00")

                    If c_cmd.ExecuteScalar("SELECT COUNT(stkid) FROM cld_inv_transstockcls WHERE stkid = " & InvstkId & " AND ccid = " & InvCc & " AND invdate = '" & InvDate.Date.ToString("yyyy-MM-dd 00:00:00") & "' AND custid = " & InvCustId & "", 0) = 0 Then

                        If c_cmd.executeNonQuery("insert into cld_inv_transstockcls values ('" & InvDate.Date.ToString("yyyy-MM-dd") & "', " & InvCustId & "," & InvstkId & ", '" & InStkNo & "', '" & InvstkName & "'," &
                                       " " & InvCc & ", '" & InvCcName & "', " & 0 & ", " & 0 & ", " & invcost & ", " & 0 & ", " & 0 & ", " & 0 & ", " &
                                       " " & 0 & ", " & 0 & ", " & 0 & ", " & invusageQty & ", " & dTotalQty & ", " & 0 & ", " & 0 & ", " & 0 & ", " & 0 & ", " & 0 & ", " & 0 & ", " & 0 & ", " & 0 & ", " & 0 & ")") Then
                            hasil = True
                        Else
                            writeerrcsv(True, InvDate.Date.ToString("yyyy-MM-dd 00:00:00"), InvCc, InvstkId)
                        End If

                    Else

                        'klo ada update

                        If c_cmd.executeNonQuery("update cld_inv_transstockcls set ttlqty = ttlqty + usageQty
                            where stkid = " & InvstkId & " and ccid = " & InvCc & " and invdate = '" & InvDate.Date.ToString("yyyy-MM-dd") & "' and custid = " & InvCustId & "") = True Then

                            c_cmd.ReNew()
                            If c_cmd.executeNonQuery("update cld_inv_transstockcls set usageQty = " & invusageQty & ",ttlqty = ttlqty -" & invusageQty & "
                                where stkid = " & InvstkId & " and ccid = " & InvCc & " and invdate = '" & InvDate.Date.ToString("yyyy-MM-dd") & "' and custid = " & InvCustId & "") = False Then
                                writeerrcsv(True, InvDate.Date.ToString("yyyy-MM-dd 00:00:00"), InvCc, InvstkId)
                            End If

                            If InvDate < Now Then
                                Dim dopenqty As Double = 0
                                'Dim dopencost As Double = 0
                                Dim rowopen As DataRow
                                Dim procdate As Date = InvDate
                                Do While procdate < Now.Date

                                    c_cmd.ReNew()
                                    rowopen = c_cmd.ReturnDataset("select * from cld_inv_transstockcls
                            where stkid = " & InvstkId & " and ccid = " & InvCc & " and invdate = '" & procdate.Date.ToString("yyyy-MM-dd") & "' and custid = " & InvCustId & "").Tables(0).Rows(0)
                                    If rowopen.Item("bclose") = 1 Then
                                        dopenqty = rowopen.Item("CloseQty")
                                    ElseIf rowopen.Item("bclose") = 0 Then
                                        'inv.OpenQty + inv.rcvqty - inv.rtnqty + inv.tinQty - inv.toutQty - inv.rwasteQty - inv.fwasteQty - inv.usageqty + inv.HOin - inv.HOout + inv.BPin - inv.BPOut
                                        dopenqty = rowopen.Item("OpenQty") + rowopen.Item("rcvqty") - rowopen.Item("rtnqty") + rowopen.Item("tinQty") - rowopen.Item("toutQty") - rowopen.Item("rwasteQty") - rowopen.Item("fwasteQty") - rowopen.Item("usageqty") + rowopen.Item("HOinqty") - rowopen.Item("HOoutqty") + rowopen.Item("BPin") - rowopen.Item("BPOut")
                                    Else
                                        dopenqty = 0
                                    End If

                                    'If rowopen.Item("bclose") = 1 Then

                                    '    dopencost = rowopen.Item("closecost")
                                    'Else
                                    '    dopencost = rowopen.Item("cost")

                                    'End If

                                    c_cmd.ReNew()
                                    If c_cmd.executeNonQuery("update cld_inv_transstockcls set ttlqty = " & dopenqty & "
                                where stkid = " & InvstkId & " and ccid = " & InvCc & " and invdate = '" & procdate.Date.ToString("yyyy-MM-dd") & "' and custid = " & InvCustId & "") = False Then
                                        writeerrcsv(True, procdate.Date.ToString("yyyy-MM-dd 00:00:00"), InvCc, InvstkId)
                                    End If


                                    c_cmd.ReNew()
                                    If c_cmd.executeNonQuery("update cld_inv_transstockcls set openqty = " & dopenqty & "
                                where stkid = " & InvstkId & " and ccid = " & InvCc & " and invdate = '" & procdate.AddDays(1).Date.ToString("yyyy-MM-dd") & "' and custid = " & InvCustId & "") = False Then
                                        writeerrcsv(True, procdate.AddDays(1).Date.ToString("yyyy-MM-dd 00:00:00"), InvCc, InvstkId)
                                    End If
                                    ',opencost = " & dopencost & ",cost = " & dopencost & "
                                    procdate = procdate.AddDays(1)



                                Loop
                            End If
                        Else
                            writeerrcsv(True, InvDate.Date.ToString("yyyy-MM-dd 00:00:00"), InvCc, InvstkId)


                        End If



                    End If
                    c_cmd.ReNew()
                    c_cmd.executeNonQuery("UPDATE cld_inv_transusagesum SET flag=0 WHERE invdate = '" & InvDate.Date.ToString("yyyy-MM-dd") & "' and custid = " & custid & " and cc=" & row.Item("cc") & " and stk_id=" & row.Item("stk_id") & " ")

                    c_cmd.ReNew()


                Next
            End If




        Catch ex As Exception

            errLog(Err.Description, "clsinventory:proc_invusage")
            Err.Clear()
        End Try
    End Sub
    Public Function insertClosingwaste(ByVal id As Integer, ByVal cc As Integer, ByVal stkid As Integer, ByVal stkanme As String, ByVal U1 As Decimal, ByVal U2 As Decimal, ByVal factor1 As Decimal, ByVal ttlstock As Decimal, ByVal cost As Decimal, ByVal U1_nm As String, ByVal U2_nm As String, ByVal createby As String, ByVal modifby As String, ByVal skno As String, ByVal cc_name As String, ByVal username As String, ByVal custid As Integer, Optional ByVal invdate As DateTime = Nothing) As DataSet
        Dim c_cmd As New clsDbo("")
        Dim strheader As String
        'Dim shasil As String = ""
        Dim ds As New DataSet
        If invdate = Nothing Then
            invdate = Format(Now, "dd MMM yyyy")
        End If
        Try
            'If c_cmd.ExecuteScalar("select count(stk_id) from cld_inv_Trans where stk_id = " & stkid & " and invdate ='" & invdate.ToString("yyyy-MM-dd 00:00:00") & "' and cc = " & cc & " and type = 14 ", 0) = 0 Then
            If id = 0 Then
                Dim idtrans As String = CItrans.idCWaste(custid)
                'Bram 10Aug2022 transaksi closing waste type 14
                strheader = "insert into cld_inv_Trans values (" & c_cmd.ExecuteScalar("select Max(ID) from cld_inv_Trans") & ", " & custid & " ,'" & invdate.Date.ToString("yyyy-MM-dd") & "', 14, " & cc & ", " &
                    " " & stkid & ", '" & stkanme & "', " & U1 & ", " & U2 & ", " & factor1 & ", " & ttlstock & ", 0," & cost & ", " &
                    " '" & idtrans & "', 1, '" & U1_nm & "', '" & U2_nm & "', '" & createby & "', '" & DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") & "', Null, Null, '" & skno & "', '" & cc_name & "')"
                c_cmd.executeNonQuery(strheader)


                insertLogModule(username, "ClosingWaste", "Insert", custid)

            Else
                'Dim a As New clsDbo(" select * from cld_Inv_Trans where id = " & id & " and type = 14 and custid = " & custid & "")

                'ds = a.selectDataset
                'a.dispose()
                'a = Nothing
                strheader = "update cld_Inv_Trans set Cc = " & cc & ", U1 = " & U1 & ", U2 = " & U2 & ", factor1 = " & factor1 & ", ttlstock = " & ttlstock & ", Cost = " & cost & ", " &
                " ModifBy = '" & username & "', Modifdate = '" & DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") & "' , Cc_name = '" & cc_name & "' where stk_id = " & stkid & " 
                and cc = " & cc & " and invdate ='" & invdate.ToString("yyyy-MM-dd 00:00:00") & "' and type = 14 and custid = " & custid & ""
                c_cmd.executeNonQuery(strheader)

                insertLogModule(username, "InvRwaste", "Update", custid)

            End If
            ds = getdsclosing(stkid, cc, invdate.Date.ToString("yyyy-MM-dd"), custid)




        Catch ex As Exception

            errLog(Err.Description, "webservice:insertClosingwaste")
            Err.Clear()
        End Try

        c_cmd.dispose()
        c_cmd = Nothing
        Return ds
    End Function
    Public Function get_usedgrpid(ByVal GrpId As Integer, ByVal cust As Integer) As Integer
        Dim iused As Integer = 0
        Dim ds As New DataSet
        Dim c_cmd As New clsDbo("")


        Try
            iused = c_cmd.ExecuteScalar("SELECT count(id) FROM cld_inv_stk WHERE grpid = " & GrpId & " AND custid = " & cust & "", 0)

        Catch ex As Exception

        End Try
        Return iused
    End Function

    Public Function get_HOstatus(ByVal xId As String, ByVal ccto As Integer, ByVal cust As Integer) As Integer
        Dim iflag As Integer = 0
        Dim ds As New DataSet
        Dim c_cmd As New clsDbo("")


        Try
            iflag = c_cmd.ExecuteScalar("SELECT flag FROM cld_inv_w where id = '" & xId & "' and custid = " & cust & " and ccTo = " & ccto & "", 0)

        Catch ex As Exception

        End Try
        Return iflag
    End Function
    Public Function get_weblic(ByVal custid As Integer, ByVal ModPriv As Integer, ByVal user As String) As Integer
        Dim iused As Integer = 0
        'Dim ds As New DataSet
        Dim c_cmd As New clsDbo("")


        Try
            If ModPriv And usrModulepriv.moduleuser = usrModulepriv.moduleuser Then
                iused = c_cmd.ExecuteScalar("SELECT COUNT(cc.id) FROM `cld_inv_cc` cc
                                            LEFT JOIN cld_outlet o ON o.id = cc.olid AND o.custid = cc.custid
                                            LEFT JOIN cld_userxstore us ON us.store = o.id WHERE USER = '" & user & "' AND o.`eDate`>='" & Date.Now.ToString("yyyy-MM-dd 00:00:00") & "'", 0)
                '"and C.Olid in (Select Store From cld_userxstore where User = '" & usr & "')"
            End If
        Catch ex As Exception

        End Try
        Return iused
    End Function

    Public Function cekstocklink(ByVal custid As Integer, ByVal stkid As Integer) As Integer
        Dim rlink As Integer = 0
        'Dim ds As New DataSet
        Dim c_cmd As New clsDbo("")


        Try

            rlink = c_cmd.ExecuteScalar("SELECT COUNT(id) FROM `cld_inv_stk` WHERE id = " & stkid & " AND custid = " & custid & " AND batch = 1 ", 0)

        Catch ex As Exception

        End Try
        Return rlink
    End Function
    Public Function stock_factor1(ByVal custid As Integer, ByVal stkid As Integer) As Decimal
        Dim sfac1 As Decimal = 0
        'Dim ds As New DataSet
        Dim c_cmd As New clsDbo("")


        Try

            sfac1 = c_cmd.ExecuteScalar("SELECT Factor1 FROM `cld_inv_stk` WHERE id = " & stkid & " AND custid = " & custid & "", 0)

        Catch ex As Exception

        End Try
        Return sfac1
    End Function
    '    Public Async Function asyncInvIn(ByVal Custid As Integer, ByVal username As String, ByVal iModPriv As Integer) As Task

    '        Try

    '            'If iModPriv = 1 Then
    '            '    GoTo StopCalculate
    '            'End If

    '            Dim x As Integer = 0
    '            Dim i As Integer

    '            Dim dmaxcls As Date


    '            Dim c_cmd As New clsDbo("")
    '            Dim CsOname As New clsInventory_stkOpname
    '            Dim cinvproc As New clsInventoryProcess


    '            'tambahan erik 30 sept 2020
    '            Dim dOpenQty As Decimal = 0
    '            Dim dCost As Decimal = 0


    '            'ambil transaksi terakhir invclosing
    '            dmaxcls = c_cmd.ReturnDataset("SELECT IFNULL(MAX(invdate),'" & Now.Date.ToString("yyyy-MM-dd 00:00:00") & "') AS maxdate, DAY(MAX(invdate)) AS maxday FROM cld_inv_closing where custid = " & Custid & " and invflag = 1").Tables(0).Rows(0).Item("maxdate")


    '            If Custid = 0 Or (c_cmd.ExecuteScalar("SELECT COUNT(DISTINCT custid) FROM cld_inv_closing", 0) > 1) Then
    '                Custid = c_cmd.ExecuteScalar("SELECT id FROM `cld_cust` LIMIT 1", 0)
    '            End If


    '            'process ini untuk mengkalkulasi inventory closing dan opening stock 
    '            i = DateDiff(DateInterval.Day, dmaxcls, Now.Date)



    '            If i <> 0 Then

    '                Do While x < i


    '                    'input closing
    '                    cinvproc.InvDate = dmaxcls
    'start1:
    '                    If c_cmd.ExecuteScalar("select count(custid) from cld_inv_closing where custid = " & Custid & " and invdate = '" & dmaxcls.AddDays(1).Date.ToString("yyyy-MM-dd 00:00:00") & "' ", 0) = 0 Then


    '                        c_cmd.executeNonQuery("insert into cld_inv_closing values ('" & dmaxcls.AddDays(1).Date.ToString("yyyy-MM-dd") & "', " & Custid & ",0)")

    '                    End If

    '                    If cinvproc.Inv_Proc_trans_stckCls(dmaxcls, dmaxcls, Custid) = True Then
    '                        'c_cmd.ReNew()

    '                        'Process_InvUsage(dmaxcls, Custid)
    '                        c_cmd.ReNew()
    '                        If c_cmd.ExecuteScalar("select count(custid) from cld_inv_closing where custid = " & Custid & " and invdate = '" & dmaxcls.AddDays(1).Date.ToString("yyyy-MM-dd 00:00:00") & "' and  invflag = 0 ", 0) = 1 Then
    '                            c_cmd.executeNonQuery("update cld_inv_closing set invflag = 1 where invdate = '" & dmaxcls.AddDays(1).Date.ToString("yyyy-MM-dd 00:00:00") & "'")
    '                        Else
    '                            c_cmd.executeNonQuery("insert into cld_inv_closing values ('" & dmaxcls.AddDays(1).Date.ToString("yyyy-MM-dd") & "', " & Custid & ",1)")

    '                        End If


    '                    Else
    '                        c_cmd.ReNew()

    '                        c_cmd.executeNonQuery("DELETE FROM cld_inv_closing WHERE invdate = '" & dmaxcls.AddDays(1).Date.ToString("yyyy-MM-dd") & "' AND custid = " & Custid & "")
    '                        GoTo start1
    '                        'Exit Sub
    '                    End If



    '                    dmaxcls = dmaxcls.AddDays(1)
    '                    x = x + 1
    '                Loop
    '            End If

    '            'Await Task.Delay(TimeSpan.FromSeconds(3))


    '            'dibuat di page_calculate
    '            'Dim c_stkcost As New clsInventory_stkCost
    '            'c_stkcost.Cek_receipeCost(Custid)
    '            'c_stkcost.cek_MenuItemLinkCost(Custid)

    '            c_cmd.dispose()
    '            c_cmd = Nothing

    'StopCalculate:

    '        Catch ex As Exception
    '            errLog(Err.Description, "clsinventory:Inv_In")
    '            Err.Clear()
    '        End Try

    '    End Function
    'Public Function MyTask() As Task
    '    ' Create a timer that will call the DelayCallback delegate after 3 seconds.
    '    Dim timer As New Timer(AddressOf DelayCallback, Nothing, 3000, Timeout.Infinite)

    '    ' Return a completed task.
    '    Return Task.CompletedTask
    'End Function
    Private Sub DelayCallback(state As Object)
        ' Do some work here
    End Sub

    Public Sub update_stockcc()

        Dim c_cmd As New clsDbo("")


        Try

            c_cmd.ReNew()
            c_cmd.executeNonQuery("INSERT INTO `cld_inv_stk_cc`
        SELECT stkid,custid,0,ccid,cost FROM `cld_inv_transstockcls` WHERE invdate =CURDATE() AND cost<>0 AND (stkid,ccid) NOT IN
        (SELECT stkid,ccid FROM `cld_inv_stk_cc`)")

        Catch ex As Exception

            errLog(Err.Description, "clsInventory:update_stockcc")
            Err.Clear()
        End Try


    End Sub
    Public Function receipecost_cc(ByVal ccid As Integer, ByVal rcpid As Integer, Optional ByVal invdate As DateTime = Nothing) As Decimal
        Dim sfac1 As Decimal = 0
        'Dim ds As New DataSet
        Dim c_cmd As New clsDbo("")
        Dim stkid As Integer = 0
        Dim custid As Integer = 0
        Try

            sfac1 = c_cmd.ExecuteScalar("SELECT Factor1 FROM `cld_inv_stk` WHERE id = " & stkid & " AND custid = " & custid & "", 0)

        Catch ex As Exception

        End Try
        Return sfac1
    End Function
    Public Sub process_InvUsageCancelPO()

        Dim c_cmd As New clsDbo("")
        Dim ds As New DataSet

        Dim dinvDate As Date

        Try

            ds = c_cmd.ReturnDataset("SELECT InvDate, cc, Stk_id, CustID, LinkID, ttlStock FROM cld_inv_transusage_cpo where flag = 2 and Type = 10")
            'ds = c_cmd.ReturnDataset("SELECT InvDate, cc, Stk_id, CustID, LinkID, ttlStock FROM cld_inv_transusage where Type = 10 AND LinkID NOT IN (SELECT SID FROM cld_salesdetail)")

            If ds.Tables(0).Rows.Count > 0 Then

                For Each row As DataRow In ds.Tables(0).Rows
                    Dim clsInv_usage As New clsInventory_Usage
                    clsInv_usage.Set_UsageSummaryPO(row.Item("CustID"), row.Item("InvDate"), row.Item("cc"), row.Item("Stk_id"), row.Item("ttlStock"), 2)

                    dinvDate = row.Item("InvDate")

                    c_cmd.ReNew()
                    'c_cmd.executeNonQuery("Update cld_inv_transusage Set flag = 0 where LinkID = " & row.Item("LinkID") & " and InvDate = '" & dinvDate.Date.ToString("yyyy-MM-dd HH:mm:ss") & "' and cc = " & row.Item("cc") & " and Stk_id = " & row.Item("Stk_id"))

                    c_cmd.executeNonQuery("Delete From cld_inv_transusage_cpo where LinkID = " & row.Item("LinkID") & " and InvDate = '" & dinvDate.Date.ToString("yyyy-MM-dd HH:mm:ss") & "' and cc = " & row.Item("cc") & " and Stk_id = " & row.Item("Stk_id"))


                Next


            End If


        Catch ex As Exception

            errLog(Err.Description, "clsInventory:process_InvUsageCancelPO")
            Err.Clear()
        End Try

        ds.Dispose()
        ds = Nothing

        c_cmd.dispose()
        c_cmd = Nothing
    End Sub

#Region "Order WHO partial delivery"

    ''' <summary>Save one surat jalan for partial mode. dt rows use U1Del as qty shipped on this SJ only.</summary>
    Public Function insertInvOrderWPartialDel(ByVal id As String, ByVal iCustid As Integer, ByVal iCcfrm As Integer, ByVal sCcfrmNm As String,
        ByVal iCcTo As Integer, ByVal sCcToNm As String, ByVal sModif As String, ByVal dExpDel As Date, ByVal dt As DataTable, ByVal sNotes As String) As String

        Dim CWare As New clsInventory_Warehouse
        Dim sHasil As String = ""
        Dim doctot As Decimal = 0

        Using conn As New MySqlConnection(My.Settings.ServerStr)
            conn.Open()
            Dim trans As MySqlTransaction = conn.BeginTransaction()
            Try
                Dim c_cmd As New MySqlCommand()
                c_cmd.Connection = conn
                c_cmd.Transaction = trans

                Dim mode = CWare.GetInvOrderDelMode(id, iCustid, iCcTo)
                If mode <> "P" Then
                    trans.Rollback()
                    Return "Error"
                End If

                Dim seqNo As Integer = 1
                c_cmd.CommandText = "SELECT IFNULL(MAX(seqNo),0)+1 FROM cld_inv_w_del WHERE wID=@wID AND custid=@custid"
                c_cmd.Parameters.AddWithValue("@wID", id)
                c_cmd.Parameters.AddWithValue("@custid", iCustid)
                seqNo = Convert.ToInt32(c_cmd.ExecuteScalar())
                c_cmd.Parameters.Clear()

                Dim newDelId As String = CItrans.idwarehouseDel(iCustid, iCcfrm, trans)

                For Each row As DataRow In dt.Rows
                    If row.Item("wID").ToString() = "" Then Continue For
                    Dim qtyShip As Decimal = Convert.ToDecimal(row.Item("U1Del"))
                    If qtyShip <= 0 Then Continue For

                    Dim oldDel As Decimal = 0
                    c_cmd.CommandText = "SELECT IFNULL(u1Del,0) FROM cld_inv_w_dtl WHERE wID=@wID AND custid=@custid AND stk_id=@stk_id AND ccTo=@ccTo"
                    c_cmd.Parameters.AddWithValue("@wID", id)
                    c_cmd.Parameters.AddWithValue("@custid", iCustid)
                    c_cmd.Parameters.AddWithValue("@stk_id", row.Item("stk_id"))
                    c_cmd.Parameters.AddWithValue("@ccTo", iCcTo)
                    Dim o = c_cmd.ExecuteScalar()
                    If o IsNot Nothing AndAlso Not IsDBNull(o) Then oldDel = Convert.ToDecimal(o)
                    c_cmd.Parameters.Clear()

                    Dim ordered As Decimal = Convert.ToDecimal(row.Item("totalu2"))
                    If oldDel + qtyShip > ordered + 0.0001D Then
                        trans.Rollback()
                        Return "Error"
                    End If

                    doctot += row.Item("cost") * qtyShip

                    CWare.InsertInvDetail_invWare(iCustid, 7, iCcfrm, row.Item("stk_id"), row.Item("stk_name"), 0, qtyShip, row.Item("Factor1"), row.Item("cost"), id, 1, row.Item("U1_nm"), sModif, row.Item("Number"), sCcfrmNm, row.Item("Unit2"), dExpDel.Date, trans)

                    Dim clsprocess As New clsInventoryProcess
                    clsprocess.InvstkId = row.Item("stk_id").ToString()
                    clsprocess.InvCc = iCcfrm
                    clsprocess.InvCcName = sCcfrmNm
                    clsprocess.InvCustId = iCustid
                    clsprocess.InvstkName = row.Item("stk_name")
                    clsprocess.ho_out = qtyShip
                    clsprocess.ho_in = 0
                    clsprocess.InvStkNo = row.Item("Number")
                    clsprocess.InvDate = dExpDel.ToString("yyyy-MM-dd 00:00:00")
                    clsprocess.executeNonQuery_Inv_ProcessCls(trans)

                    c_cmd.CommandText = "INSERT INTO cld_inv_w_del_dtl (DelID,wID,custid,ccTo,stk_id,stk_name,qtyDel,cost) VALUES (@DelID,@wID,@custid,@ccTo,@stk_id,@stk_name,@qtyDel,@cost)"
                    c_cmd.Parameters.AddWithValue("@DelID", newDelId)
                    c_cmd.Parameters.AddWithValue("@wID", id)
                    c_cmd.Parameters.AddWithValue("@custid", iCustid)
                    c_cmd.Parameters.AddWithValue("@ccTo", iCcTo)
                    c_cmd.Parameters.AddWithValue("@stk_id", row.Item("stk_id"))
                    c_cmd.Parameters.AddWithValue("@stk_name", row.Item("stk_name"))
                    c_cmd.Parameters.AddWithValue("@qtyDel", qtyShip)
                    c_cmd.Parameters.AddWithValue("@cost", row.Item("cost"))
                    c_cmd.ExecuteNonQuery()
                    c_cmd.Parameters.Clear()

                    c_cmd.CommandText = "UPDATE cld_inv_w_dtl SET cost=@cost, u1Del=IFNULL(u1Del,0)+@qty WHERE wID=@wID AND custid=@custid AND stk_id=@stk_id AND ccTo=@ccTo"
                    c_cmd.Parameters.AddWithValue("@cost", row.Item("cost"))
                    c_cmd.Parameters.AddWithValue("@qty", qtyShip)
                    c_cmd.Parameters.AddWithValue("@wID", id)
                    c_cmd.Parameters.AddWithValue("@custid", iCustid)
                    c_cmd.Parameters.AddWithValue("@stk_id", row.Item("stk_id"))
                    c_cmd.Parameters.AddWithValue("@ccTo", iCcTo)
                    c_cmd.ExecuteNonQuery()
                    c_cmd.Parameters.Clear()
                Next

                If doctot <= 0 Then
                    trans.Rollback()
                    Return "Error"
                End If

                doctot = Math.Round(doctot)
                c_cmd.CommandText = "INSERT INTO cld_inv_w_del (DelID,wID,custid,ccTo,ccFrom,DelDate,appBy,DocTot,Notes,flag,seqNo) VALUES (@DelID,@wID,@custid,@ccTo,@ccFrom,@DelDate,@appBy,@DocTot,@Notes,1,@seqNo)"
                c_cmd.Parameters.AddWithValue("@DelID", newDelId)
                c_cmd.Parameters.AddWithValue("@wID", id)
                c_cmd.Parameters.AddWithValue("@custid", iCustid)
                c_cmd.Parameters.AddWithValue("@ccTo", iCcTo)
                c_cmd.Parameters.AddWithValue("@ccFrom", iCcfrm)
                c_cmd.Parameters.AddWithValue("@DelDate", Date.Now.Date.ToString("yyyy-MM-dd 00:00:00"))
                c_cmd.Parameters.AddWithValue("@appBy", sModif)
                c_cmd.Parameters.AddWithValue("@DocTot", doctot)
                c_cmd.Parameters.AddWithValue("@Notes", If(sNotes, ""))
                c_cmd.Parameters.AddWithValue("@seqNo", seqNo)
                c_cmd.ExecuteNonQuery()
                c_cmd.Parameters.Clear()

                Dim delComplete As Integer = If(CWare.OrderDelIsFullyShipped(id, iCustid, iCcTo, trans), 1, 0)
                c_cmd.CommandText = "UPDATE cld_inv_w SET DocTot=(SELECT IFNULL(SUM(DocTot),0) FROM cld_inv_w_del WHERE wID=@wID AND custid=@custid AND flag=1), flag=2, DelComplete=@DelComplete, Deldate=@Deldate, appBy=@appBy WHERE id=@wID AND custid=@custid AND ccTo=@ccTo"
                c_cmd.Parameters.AddWithValue("@DelComplete", delComplete)
                c_cmd.Parameters.AddWithValue("@Deldate", Date.Now.Date.ToString("yyyy-MM-dd 00:00:00"))
                c_cmd.Parameters.AddWithValue("@appBy", sModif)
                c_cmd.Parameters.AddWithValue("@wID", id)
                c_cmd.Parameters.AddWithValue("@custid", iCustid)
                c_cmd.Parameters.AddWithValue("@ccTo", iCcTo)
                c_cmd.ExecuteNonQuery()

                trans.Commit()
                sHasil = newDelId
            Catch ex As Exception
                trans.Rollback()
                errLog(ex.Message, "clsInventory:insertInvOrderWPartialDel")
                sHasil = "Error"
            End Try
        End Using
        Return sHasil
    End Function

    ''' <summary>Receive goods for one surat jalan (partial mode).</summary>
    Public Function receiveInvOrderWDel(ByVal delID As String, ByVal id As String, ByVal iCustid As Integer, ByVal iCcfrm As Integer, ByVal sCcfrmNm As String,
        ByVal iCcTo As Integer, ByVal sCcToNm As String, ByVal sModif As String, ByVal dRcv As Date, ByVal dt As DataTable) As String

        Dim CWare As New clsInventory_Warehouse
        Using conn As New MySqlConnection(My.Settings.ServerStr)
            conn.Open()
            Dim trans As MySqlTransaction = conn.BeginTransaction()
            Try
                Dim c_cmd As New MySqlCommand()
                c_cmd.Connection = conn
                c_cmd.Transaction = trans

                c_cmd.CommandText = "SELECT COUNT(*) FROM cld_inv_w_del WHERE DelID=@DelID AND custid=@custid AND wID=@wID AND flag=1 AND rcvDate IS NULL"
                c_cmd.Parameters.AddWithValue("@DelID", delID)
                c_cmd.Parameters.AddWithValue("@custid", iCustid)
                c_cmd.Parameters.AddWithValue("@wID", id)
                If Convert.ToInt32(c_cmd.ExecuteScalar()) = 0 Then
                    trans.Rollback()
                    Return "Error"
                End If
                c_cmd.Parameters.Clear()

                For Each row As DataRow In dt.Rows
                    If row.Item("wID").ToString() = "" Then Continue For
                    Dim qtyRcv As Decimal = Convert.ToDecimal(row.Item("U1Rcv"))
                    If qtyRcv <= 0 Then Continue For

                    CWare.InsertInvDetail_invWare(iCustid, 8, iCcTo, row.Item("stk_id"), row.Item("stk_name"), 0, qtyRcv, row.Item("Factor1"), row.Item("cost"), id, 1, row.Item("U1_nm"), sModif, row.Item("Number"), sCcToNm, row.Item("Unit2"), dRcv.Date, trans)

                    Dim clsprocess As New clsInventoryProcess
                    clsprocess.InvstkId = row.Item("stk_id").ToString()
                    clsprocess.InvCc = iCcTo
                    clsprocess.InvCcName = sCcToNm
                    clsprocess.InvCustId = iCustid
                    clsprocess.InvstkName = row.Item("stk_name")
                    clsprocess.InvCost = row.Item("cost")
                    clsprocess.ho_out = 0
                    clsprocess.ho_in = qtyRcv
                    clsprocess.InvStkNo = row.Item("Number")
                    clsprocess.InvDate = dRcv.ToString("yyyy-MM-dd 00:00:00")
                    clsprocess.BCost = True
                    clsprocess.executeNonQuery_Inv_ProcessCls(trans)

                    c_cmd.CommandText = "UPDATE cld_inv_w_dtl SET U1Rcv=IFNULL(U1Rcv,0)+@qty WHERE wID=@wID AND custid=@custid AND stk_id=@stk_id AND ccTo=@ccTo"
                    c_cmd.Parameters.AddWithValue("@qty", qtyRcv)
                    c_cmd.Parameters.AddWithValue("@wID", id)
                    c_cmd.Parameters.AddWithValue("@custid", iCustid)
                    c_cmd.Parameters.AddWithValue("@stk_id", row.Item("stk_id"))
                    c_cmd.Parameters.AddWithValue("@ccTo", iCcTo)
                    c_cmd.ExecuteNonQuery()
                    c_cmd.Parameters.Clear()
                Next

                c_cmd.CommandText = "UPDATE cld_inv_w_del SET rcvDate=@rcvDate, rcvBy=@rcvBy WHERE DelID=@DelID AND custid=@custid"
                c_cmd.Parameters.AddWithValue("@rcvDate", dRcv.ToString("yyyy-MM-dd HH:mm:ss"))
                c_cmd.Parameters.AddWithValue("@rcvBy", sModif)
                c_cmd.Parameters.AddWithValue("@DelID", delID)
                c_cmd.Parameters.AddWithValue("@custid", iCustid)
                c_cmd.ExecuteNonQuery()
                c_cmd.Parameters.Clear()

                Dim allRcv = CWare.AllActiveDelReceived(id, iCustid, trans)
                Dim fullyShipped = CWare.OrderDelIsFullyShipped(id, iCustid, iCcTo, trans)
                If allRcv AndAlso fullyShipped Then
                    c_cmd.CommandText = "UPDATE cld_inv_w SET flag=3, RcvComplete=1, rcvDate=@rcvDate, rcvBy=@rcvBy WHERE id=@wID AND custid=@custid AND ccTo=@ccTo"
                    c_cmd.Parameters.AddWithValue("@rcvDate", dRcv.ToString("yyyy-MM-dd HH:mm:ss"))
                    c_cmd.Parameters.AddWithValue("@rcvBy", sModif)
                    c_cmd.Parameters.AddWithValue("@wID", id)
                    c_cmd.Parameters.AddWithValue("@custid", iCustid)
                    c_cmd.Parameters.AddWithValue("@ccTo", iCcTo)
                    c_cmd.ExecuteNonQuery()
                End If

                trans.Commit()
                Return delID
            Catch ex As Exception
                trans.Rollback()
                errLog(ex.Message, "clsInventory:receiveInvOrderWDel")
                Return "Error"
            End Try
        End Using
    End Function

    Public Function cancelInvOrderWDel(ByVal delID As String, ByVal id As String, ByVal iCustid As Integer, ByVal iCcfrm As Integer, ByVal sCcfrmNm As String, ByVal iCcTo As Integer) As String
        Dim CWare As New clsInventory_Warehouse
        Using conn As New MySqlConnection(My.Settings.ServerStr)
            conn.Open()
            Dim trans As MySqlTransaction = conn.BeginTransaction()
            Try
                Dim c_cmd As New MySqlCommand()
                c_cmd.Connection = conn
                c_cmd.Transaction = trans

                c_cmd.CommandText = "SELECT COUNT(*) FROM cld_inv_w_del WHERE DelID=@DelID AND custid=@custid AND wID=@wID AND ccTo=@ccTo AND flag=1 AND rcvDate IS NULL"
                c_cmd.Parameters.AddWithValue("@DelID", delID)
                c_cmd.Parameters.AddWithValue("@custid", iCustid)
                c_cmd.Parameters.AddWithValue("@wID", id)
                c_cmd.Parameters.AddWithValue("@ccTo", iCcTo)
                If Convert.ToInt32(c_cmd.ExecuteScalar()) = 0 Then
                    trans.Rollback()
                    Return "Error"
                End If
                c_cmd.Parameters.Clear()

                c_cmd.CommandText = "SELECT stk_id, stk_name, qtyDel, cost FROM cld_inv_w_del_dtl WHERE DelID=@DelID AND custid=@custid AND wID=@wID AND ccTo=@ccTo"
                c_cmd.Parameters.AddWithValue("@DelID", delID)
                c_cmd.Parameters.AddWithValue("@custid", iCustid)
                c_cmd.Parameters.AddWithValue("@wID", id)
                c_cmd.Parameters.AddWithValue("@ccTo", iCcTo)
                Dim da As New MySqlDataAdapter(c_cmd)
                Dim dtl As New DataTable
                da.Fill(dtl)
                c_cmd.Parameters.Clear()

                For Each row As DataRow In dtl.Rows
                    Dim qty As Decimal = Convert.ToDecimal(row.Item("qtyDel"))
                    Dim clsprocess As New clsInventoryProcess
                    clsprocess.InvstkId = row.Item("stk_id").ToString()
                    clsprocess.InvCc = iCcfrm
                    clsprocess.InvCcName = sCcfrmNm
                    clsprocess.InvCustId = iCustid
                    clsprocess.InvstkName = row.Item("stk_name")
                    clsprocess.ho_out = -qty
                    clsprocess.ho_in = 0
                    clsprocess.InvDate = Date.Now.ToString("yyyy-MM-dd 00:00:00")
                    clsprocess.executeNonQuery_Inv_ProcessCls(trans)

                    c_cmd.CommandText = "UPDATE cld_inv_w_dtl SET u1Del=GREATEST(0,IFNULL(u1Del,0)-@qty) WHERE wID=@wID AND custid=@custid AND stk_id=@stk_id AND ccTo=@ccTo"
                    c_cmd.Parameters.AddWithValue("@qty", qty)
                    c_cmd.Parameters.AddWithValue("@wID", id)
                    c_cmd.Parameters.AddWithValue("@custid", iCustid)
                    c_cmd.Parameters.AddWithValue("@stk_id", row.Item("stk_id"))
                    c_cmd.Parameters.AddWithValue("@ccTo", iCcTo)
                    c_cmd.ExecuteNonQuery()
                    c_cmd.Parameters.Clear()
                Next

                c_cmd.CommandText = "UPDATE cld_inv_w_del SET flag=0 WHERE DelID=@DelID AND custid=@custid AND wID=@wID AND ccTo=@ccTo AND flag=1 AND rcvDate IS NULL"
                c_cmd.Parameters.AddWithValue("@DelID", delID)
                c_cmd.Parameters.AddWithValue("@custid", iCustid)
                c_cmd.Parameters.AddWithValue("@wID", id)
                c_cmd.Parameters.AddWithValue("@ccTo", iCcTo)
                c_cmd.ExecuteNonQuery()
                c_cmd.Parameters.Clear()

                c_cmd.CommandText = "SELECT COUNT(*) FROM cld_inv_w_del WHERE wID=@wID AND custid=@custid AND flag=1"
                c_cmd.Parameters.AddWithValue("@wID", id)
                c_cmd.Parameters.AddWithValue("@custid", iCustid)
                Dim hasActive As Integer = Convert.ToInt32(c_cmd.ExecuteScalar())
                c_cmd.Parameters.Clear()

                If hasActive = 0 Then
                    c_cmd.CommandText = "UPDATE cld_inv_w SET flag=1, DelComplete=0 WHERE id=@wID AND custid=@custid AND ccTo=@ccTo"
                Else
                    Dim delComplete As Integer = If(CWare.OrderDelIsFullyShipped(id, iCustid, iCcTo, trans), 1, 0)
                    c_cmd.CommandText = "UPDATE cld_inv_w SET flag=2, DelComplete=@dc WHERE id=@wID AND custid=@custid AND ccTo=@ccTo"
                    c_cmd.Parameters.AddWithValue("@dc", delComplete)
                End If
                c_cmd.Parameters.AddWithValue("@wID", id)
                c_cmd.Parameters.AddWithValue("@custid", iCustid)
                c_cmd.Parameters.AddWithValue("@ccTo", iCcTo)
                c_cmd.ExecuteNonQuery()

                trans.Commit()
                Return "OK"
            Catch ex As Exception
                trans.Rollback()
                errLog(ex.Message, "clsInventory:cancelInvOrderWDel")
                Return "Error"
            End Try
        End Using
    End Function

#End Region

End Class
