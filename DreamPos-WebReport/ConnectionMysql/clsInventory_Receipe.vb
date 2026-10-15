Public Class clsInventory_Receipe

    Public Function SelectDataset_Idrcp(ByVal blink As Integer) As Integer
        Dim ds, dsb As New DataSet
        Dim strFilter As String = ""
        Dim Qty As Integer = 0

        '1 brarti link ke stockmaster
        '2 ada receipe detail
        '0 baru bisa diapus

        Try

            Dim a As New clsDbo("Select * from cld_inv_stk where Blink = " & blink & "")
            Dim b As New clsDbo("Select * from cld_inv_rcpdtl where rcpid = " & blink & "")
            ds = a.selectDataset
            dsb = b.selectDataset

            If ds.Tables(0).Rows.Count > 0 Then
                Qty = 1
            ElseIf dsb.Tables(0).Rows.Count > 0 Then
                Qty = 2
            Else
                Qty = 0
            End If

            ds.Dispose()
            dsb.Dispose()

            a.dispose()
            a = Nothing

            b.dispose()
            b = Nothing


        Catch ex As Exception
            errLog(Err.Description, "clsInventory_Receipe:SelectDataset_Idrcp")
            Err.Clear()
        End Try

        Return Qty

    End Function


    Public Function SelectDataset_rcpdtlSearch(ByVal name As String, ByVal custid As Integer, ByVal rcpid As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""


        Dim a As New clsDbo("SELECT cld_inv_rcpdtl.*,CASE WHEN tipe = 1 THEN 'Inventory' ELSE 'Receipe' END AS TypeName, CASE WHEN tipe = 1 " &
                            " THEN cld_inv_stk.Name ELSE cld_inv_rcp.Name END AS dtlName, CASE WHEN tipe = 1 THEN  cld_inv_stk.Unit3 ELSE 'Pcs' END AS unit, " &
                            " 0 AS cost_per_unit3, 0 AS total_cost FROM cld_inv_rcpdtl " &
                            " LEFT JOIN cld_inv_stk ON cld_inv_stk.ID = cld_inv_rcpdtl.kodeId " &
                            " LEFT JOIN cld_inv_rcp ON cld_inv_rcp.ID = cld_inv_rcpdtl.kodeId " &
                            " WHERE cld_inv_rcpdtl.custid = @custid AND cld_inv_rcpdtl.rcpid = @rcpid AND " &
                            " CASE WHEN tipe = 1 THEN cld_inv_stk.Name ELSE cld_inv_rcp.Name END LIKE @search ")

        Try
            a.addParam(custid, "@custid", MySql.Data.MySqlClient.MySqlDbType.Int32)
            a.addParam(rcpid, "@rcpid", MySql.Data.MySqlClient.MySqlDbType.Int32)
            a.addParam("%" & name & "%", "@search", MySql.Data.MySqlClient.MySqlDbType.VarChar)
            Return a.selectDataset(True)
        Finally
            a.dispose()
        End Try

    End Function

    Public Function SelectDataset_rcpdtlSearch_withcost(ByVal name As String, ByVal custid As Integer, ByVal rcpid As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""


        Dim a As New clsDbo("SELECT cld_inv_rcpdtl.*,CASE WHEN tipe = 1 THEN 'Inventory' ELSE 'Receipe' END AS TypeName, CASE WHEN tipe = 1 " &
                            " THEN cld_inv_stk.Name ELSE cld_inv_rcp.Name END AS dtlName, CASE WHEN tipe = 1 THEN  cld_inv_stk.Unit3 ELSE 'Pcs' END AS unit, " &
                            " CASE WHEN tipe = 1 THEN IFNULL((SELECT IFNULL(SUM(ts.cost),0) / IFNULL(NULLIF(cld_inv_stk.factor2,0),1) FROM cld_inv_transstockcls ts WHERE ts.invdate = CURDATE() AND ts.ccid = (SELECT id FROM cld_inv_cc WHERE olid = 0 LIMIT 1) AND ts.stkid = cld_inv_rcpdtl.kodeId), 0) ELSE 0 END AS cost_per_unit3, " &
                            " CASE WHEN tipe = 1 THEN IFNULL((SELECT IFNULL(SUM(ts.cost),0) / IFNULL(NULLIF(cld_inv_stk.factor2,0),1) FROM cld_inv_transstockcls ts WHERE ts.invdate = CURDATE() AND ts.ccid = (SELECT id FROM cld_inv_cc WHERE olid = 0 LIMIT 1) AND ts.stkid = cld_inv_rcpdtl.kodeId), 0) * (cld_inv_rcpdtl.qty * (100 + IFNULL(cld_inv_rcpdtl.lossf, 0)) / 100) ELSE 0 END AS total_cost " &
                            " FROM cld_inv_rcpdtl " &
                            " LEFT JOIN cld_inv_stk ON cld_inv_stk.ID = cld_inv_rcpdtl.kodeId " &
                            " LEFT JOIN cld_inv_rcp ON cld_inv_rcp.ID = cld_inv_rcpdtl.kodeId " &
                            " WHERE cld_inv_rcpdtl.custid = @custid AND cld_inv_rcpdtl.rcpid = @rcpid AND " &
                            " CASE WHEN tipe = 1 THEN cld_inv_stk.Name ELSE cld_inv_rcp.Name END LIKE @search ")

        Try
            a.addParam(custid, "@custid", MySql.Data.MySqlClient.MySqlDbType.Int32)
            a.addParam(rcpid, "@rcpid", MySql.Data.MySqlClient.MySqlDbType.Int32)
            a.addParam("%" & name & "%", "@search", MySql.Data.MySqlClient.MySqlDbType.VarChar)
            Return a.selectDataset(True)
        Finally
            a.dispose()
        End Try

    End Function
    Public Sub Process_UpdateCostReceipe(ByVal custid As Integer, ByVal dInvDate As Date, Optional ByVal rcp_code As String = "")
        Dim recipes As DataTable
        Using db As New RecipeDb()
            db.SetParameter("custid", custid)
            db.SetParameter("code", rcp_code)
            Dim sql As String = "SELECT id FROM cld_inv_rcp WHERE custid=@custid"
            If rcp_code <> "" Then sql &= " AND code=@code"
            recipes = db.ReadTable(sql & " ORDER BY id")
            db.Commit()
        End Using
        Using recipes
            For Each recipe As DataRow In recipes.Rows
                ' Each recipe's cost centres commit together; large batches do not hold one long transaction.
                Using db As New RecipeDb()
                    db.SetParameter("custid", custid)
                    db.SetParameter("rcpid", recipe("id"))
                    db.SetParameter("invdate", dInvDate)
                    If db.Scalar("SELECT id FROM cld_inv_rcp WHERE id=@rcpid AND custid=@custid FOR UPDATE") Is Nothing Then Continue For
                    Dim ccSql As String = If(rcp_code = "",
                        "SELECT id FROM cld_inv_cc WHERE custid=@custid ORDER BY id",
                        "SELECT ccid AS id FROM cld_inv_rcp_cc WHERE custid=@custid AND rcpid=@rcpid ORDER BY ccid")
                    Using centres As DataTable = db.ReadTable(ccSql)
                        For Each centre As DataRow In centres.Rows
                            db.SetParameter("ccid", centre("id"))
                            Dim result As Object = db.Scalar(
                                "SELECT ROUND(SUM(((d.qty+((d.qty*d.lossf)/100))/s.Factor2) * " &
                                "(SELECT Cost FROM cld_inv_transstockcls ts WHERE ts.stkid=d.kodeId AND ts.ccid=@ccid AND ts.InvDate=@invdate)),2) " &
                                "FROM cld_inv_rcpdtl d INNER JOIN cld_inv_stk s ON d.kodeId=s.id " &
                                "WHERE d.rcpid=@rcpid AND d.custid=@custid AND d.tipe=1")
                            ' Preserve the existing integer conversion in Set_ReceipeCost.
                            db.SetParameter("cost", If(result Is Nothing OrElse IsDBNull(result), 0, CInt(Convert.ToDecimal(result))))
                            db.Execute("UPDATE cld_inv_rcp_cc SET cost=@cost WHERE rcpid=@rcpid AND ccid=@ccid AND custid=@custid")
                        Next
                    End Using
                    db.Commit()
                End Using
            Next
        End Using
    End Sub


    'bram 18Aug2022

    'Select all Receipe
    'Public Sub Process_UpdateCostReceipe(ByVal custid As Integer)
    '    'perubahan 13 juni 22 jadi nanti akan kalkulasi receipe cost

    '    Dim ds As New DataSet
    '    Dim dsCostCentre As New DataSet

    '    Dim totalcost As Decimal = 0

    '    Try
    '        Dim c_Cmd As New clsDbo("")
    '        ds = c_Cmd.ReturnDataset("Select * from cld_inv_rcp where custid = " & custid)

    '        dsCostCentre = c_Cmd.ReturnDataset("Select * from cld_inv_cc where custid = " & custid)

    '        If ds.Tables(0).Rows.Count > 0 Then
    '            For Each row As DataRow In ds.Tables(0).Rows


    '                If dsCostCentre.Tables(0).Rows.Count > 0 Then
    '                    For Each rowCostCentre As DataRow In dsCostCentre.Tables(0).Rows


    '                        totalcost = 0
    '                        totalcost = c_Cmd.ExecuteScalar(" SELECT ROUND(SUM(((cld_inv_rcpdtl.qty +(( cld_inv_rcpdtl.qty * cld_inv_rcpdtl.lossf)/100))/ cld_inv_stk.Factor2) * (SELECT Cost FROM `cld_inv_transstockcls` WHERE cld_inv_rcpdtl.`kodeId` = stkid AND cld_inv_transstockcls.`ccid` = " & rowCostCentre.Item("ID") & " ORDER BY invdate DESC LIMIT 1)), 2)AS Totalcost" &
    '                                                        " FROM cld_inv_rcpdtl INNER JOIN cld_inv_stk" &
    '                                                        " ON cld_inv_rcpdtl.`kodeId` = cld_inv_stk.id" &
    '                                                        " WHERE rcpid = " & row.Item("ID") & " AND tipe = 1", 11)



    '                        Set_ReceipeCost(row.Item("ID"), totalcost, rowCostCentre.Item("ID"), custid)


    '                    Next

    '                End If


    '            Next
    '        End If


    '        c_Cmd.dispose()
    '        c_Cmd = Nothing

    '    Catch ex As Exception
    '        errLog(Err.Description, "clsInventory_stkCost:Process_UpdateCostReceipe")
    '        Err.Clear()
    '    End Try

    '    ds.Dispose()
    '    ds = Nothing

    '    dsCostCentre.Dispose()
    '    dsCostCentre = Nothing



    'End Sub
    Public Sub Set_ReceipeCost(ByVal RcpID As Integer, ByVal dTotalCost As Integer, ByVal iCC As Integer, ByVal icust As Integer)
        Using db As New RecipeDb()
            db.SetParameter("rcpid", RcpID)
            db.SetParameter("cost", dTotalCost)
            db.SetParameter("ccid", iCC)
            db.SetParameter("custid", icust)
            db.Execute("UPDATE cld_inv_rcp_cc SET cost=@cost WHERE rcpid=@rcpid AND ccid=@ccid AND custid=@custid")
            db.Commit()
        End Using
    End Sub

    'Public Function SelectDataset_rcpdtlExport(ByVal custid As Integer) As DataSet
    '    Dim ds As New DataSet
    '    Dim strFilter As String = ""


    '    Dim a As New clsDbo("SELECT CODE ResepNo, cld_inv_rcp.Name AS ResepNama,cld_inv_stk.Number KodeBarang,cld_inv_stk.`Name` NamaBarang,qty,lossf lossFactor,
    '                      CASE WHEN tipe = 1 THEN  cld_inv_stk.Unit3 ELSE 'Pcs' END AS unit FROM cld_inv_rcpdtl 
    '                      INNER JOIN cld_inv_rcp ON cld_inv_rcp.ID = cld_inv_rcpdtl.kodeId 
    '                         LEFT JOIN cld_inv_stk ON cld_inv_stk.ID = cld_inv_rcpdtl.kodeId 
    '                         WHERE cld_inv_rcpdtl.custid = " & custid & " ORDER BY CODE")

    '    ds = a.selectDataset

    '    a.dispose()
    '    a = Nothing

    '    Return ds

    'End Function

End Class
