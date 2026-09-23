Public Class clsInventory_stkCost

    Public Function get_stkCost(ByVal iStk As Integer, ByVal iCc As Integer, ByVal iCust As Integer) As Decimal

        Dim hasil As Decimal = 0

        Try

            Dim cCmd As New clsDbo("")
            hasil = cCmd.ExecuteScalar("SELECT ifnull(Cost,0) cost FROM cld_inv_transstockcls where stkid = " & iStk & " and ccid = " & iCc & " and invdate ='" & Now.ToString("yyyy-MM-dd 00:00:00") & "' and Custid = " & iCust, 0)
            If hasil = 0 And cCmd.ExecuteScalar("SELECT count(custid) FROM cld_inv_transstockcls where stkid = " & iStk & " and ccid = " & iCc & " and invdate ='" & Now.ToString("yyyy-MM-dd 00:00:00") & "' and Custid = " & iCust, 0) = 0 Then
                insert_transstkcls(iStk, iCc, iCust)
            End If

            cCmd = Nothing

        Catch ex As Exception
            errLog(Err.Description, "clsInventory_stkCost:get_stkCost")
            Err.Clear()
        End Try

        Return hasil

    End Function
    Public Sub insert_transstkcls(ByVal iStk As Integer, ByVal iCc As Integer, ByVal iCust As Integer)
        Dim cinvstk As New clsInventory_stk
        Dim dt As New DataTable
        Dim cCmd As New clsDbo("")
        Try
            dt = cinvstk.SelectDataset_StkEdit(iStk, iCust).Tables(0)
            If dt.Rows.Count > 0 Then
                Dim ccname As String = ""
                For Each dr As DataRow In dt.Rows
                    ccname = cCmd.ExecuteScalar("SELECT name FROM cld_inv_cc where id = " & iCc, 10)
                    cCmd.executeNonQuery("insert into cld_inv_transstockcls values ('" & Now.ToString("yyyy-MM-dd 00:00:00") & "', " & iCust & "," & iStk & ", '" & dr.Item("number") & "', '" & dr.Item("name") & "'," &
                                        " " & iCc & ", '" & ccname & "', " & 0 & ", " & 0 & ", " & 0 & ", " & 0 & ", " & 0 & ", " & 0 & ", " &
                                        " " & 0 & ", " & 0 & ", " & 0 & ", " & 0 & ", " & 0 & ", " & 0 & ", " & 0 & ", " & 0 & ", " & 0 & ", " & 0 & ", " & 0 & ", " & 0 & ", " & 0 & ", " & 0 & ")")

                Next

            End If



        Catch ex As Exception

        End Try
    End Sub

    'Select all Receipe
    Public Function getDataset_Receipe(ByVal custid As Integer) As DataSet
        'perubahan tgl 2 nov 2016------++++
        Dim ds As New DataSet

        Try
            Dim c_Cmd As New clsDbo("")
            ds = c_Cmd.ReturnDataset("Select * from cld_inv_rcp where custid = " & custid)

            c_Cmd = Nothing

        Catch ex As Exception
            errLog(Err.Description, "clsInventory_stkCost:getDataset_Receipe")
            Err.Clear()
        End Try


        Return ds


    End Function

    Public Function getDataset_Receipecc(ByVal custid As Integer) As DataSet
        'perubahan tgl 2 nov 2016------++++
        Dim ds As New DataSet

        Try
            Dim c_Cmd As New clsDbo("")
            'ds = c_Cmd.ReturnDataset("SELECT * FROM cld_inv_rcp CROSS JOIN cld_inv_cc WHERE cld_inv_rcp.custid = " & custid)
            ds = c_Cmd.ReturnDataset("SELECT rcp.id rcpid,rcp.custid,cc.id ccid FROM cld_inv_rcp rcp CROSS JOIN cld_inv_cc cc WHERE rcp.custid = " & custid)
            c_Cmd = Nothing

        Catch ex As Exception
            errLog(Err.Description, "clsInventory_stkCost:getDataset_Receipecc")
            Err.Clear()
        End Try


        Return ds


    End Function

    'Select all Receipe
    Public Function getDataset_LinkMenu(ByVal custid As Integer) As DataSet

        Dim ds As New DataSet

        Try
            Dim c_Cmd As New clsDbo("")
            ds = c_Cmd.ReturnDataset("Select * from cld_itemmaster where iType > 0 and custid = " & custid)

            c_Cmd = Nothing

        Catch ex As Exception
            errLog(Err.Description, "clsInventory_stkCost:getDataset_LinkMenu")
            Err.Clear()
        End Try


        Return ds


    End Function
    Public Function getDataset_LinkMenucc(ByVal custid As Integer) As DataSet

        Dim ds As New DataSet

        Try
            Dim c_Cmd As New clsDbo("")
            ds = c_Cmd.ReturnDataset("SELECT * FROM cld_itemmaster item CROSS JOIN cld_inv_cc cc WHERE iType > 0 AND item.custid = " & custid)

            c_Cmd = Nothing

        Catch ex As Exception
            errLog(Err.Description, "clsInventory_stkCost:getDataset_LinkMenucc")
            Err.Clear()
        End Try


        Return ds


    End Function



    Public Sub cek_MenuItemLinkCostcc(ByVal iCust As Integer)
        Dim dLinkStock As New DataSet
        Dim cData As New clsDbo("")
        Dim dTotal As Integer

        Try

            dLinkStock = getDataset_LinkMenu(iCust)

            For Each data_row As DataRow In dLinkStock.Tables(0).Rows

                If IsDBNull(data_row.Item("rID")) = False And data_row.Item("rID") <> 0 Then

                    cData.ReNew()
                    dTotal = cData.ExecuteScalar("Select Cost from cld_inv_rcp where id = " & data_row.Item("rID") & " and Custid = " & iCust, 2)
                    dTotal = dTotal * data_row.Item("Factor")

                ElseIf IsDBNull(data_row.Item("iID")) = False And data_row.Item("iID") <> 0 Then

                    cData.ReNew()
                    dTotal = cData.ExecuteScalar("SELECT  Avg(cost) as Cost FROM cld_inv_stkcost where stkid = " & data_row.Item("iID") & " and Custid = " & iCust, 2)
                    dTotal = dTotal * data_row.Item("Factor")

                End If

                cData.ReNew()
                cData.executeNonQuery("Update cld_itemmaster set Cost = " & dTotal & " where ItemNo = '" & data_row.Item("ItemNo") & "' and CustID = " & data_row.Item("CustID"))

            Next data_row

        Catch ex As Exception
            errLog(Err.Description, "clsInventory_stkCost:cek_MenuItemLinkCost")
            Err.Clear()
        End Try

        cData = Nothing
        dLinkStock.Dispose()

    End Sub




    'perubahan 20 sept 2020, nambahin pengecekan klo null ga usah di process
    Private Function ReceiptCost_ByInventory(ByVal Item As String, ByVal qty As Integer, ByVal lossfactor As Integer, ByVal iCust As Integer, ByVal ccid As Integer) As Decimal

        Dim TotalCost As Decimal = 0
        Dim pembagianTotalcost As Decimal = 0
        Dim cCmd As New clsDbo("")

        Try

            Dim dData As New DataSet
            'dData = cCmd.ReturnDataset("SELECT IFNULL(AVG(tstk.Cost),0) AS Cost , stk.Factor1, stk.Factor2 from cld_inv_stk AS stk INNER JOIN (SELECT * FROM cld_inv_transstockcls WHERE invdate ='" & Now.ToString("yyyy-MM-dd 00:00:00") & "') AS tstk ON (stk.CustID = tstk.custId) AND (tstk.stkId = stk.ID) " &
            '                           "where tstk.custid  = " & iCust & " and tstk.stkid  = " & Item & " and tstk.ccid  = " & ccid)
            dData = cCmd.ReturnDataset("SELECT IFNULL(tstk.Cost,0) AS Cost , stk.Factor1, stk.Factor2 from cld_inv_stk AS stk INNER JOIN cld_inv_transstockcls AS tstk ON (stk.CustID = tstk.custId) AND (tstk.stkId = stk.ID) " &
                                       "where tstk.custid  = " & iCust & " and tstk.stkid  = " & Item & " and tstk.invdate ='" & Now.ToString("yyyy-MM-dd 00:00:00") & "' and tstk.ccid  = " & ccid)

            If dData.Tables(0).Rows.Count > 0 Then

                If IsDBNull(dData.Tables(0).Rows(0).Item("Factor2")) = False Then
                    TotalCost = (qty + (qty * lossfactor / 100)) * CDec(dData.Tables(0).Rows(0).Item("Cost"))


                    pembagianTotalcost = CDec(dData.Tables(0).Rows(0).Item("Factor2"))

                    If pembagianTotalcost > 0 Then
                        TotalCost = TotalCost / pembagianTotalcost
                    Else
                        TotalCost = 0
                    End If

                End If


            End If

        Catch ex As Exception
            errLog(Err.Description, "clsInventory_stkCost:ReceiptCost_ByInventory")
            Err.Clear()
        End Try

        cCmd = Nothing
        Return TotalCost

    End Function

    Private Function ReceiptCost_ByReceipe(ByVal RcpID As Integer, ByVal iCust As Integer, ByVal ccid As Integer) As Decimal
        'hitung cost per receipt
        Dim TotalCost As Decimal = 0

        Dim cCmd As New clsDbo("")

        Try

            Dim dData As New DataSet
            dData = cCmd.ReturnDataset("Select * from cld_inv_rcpdtl where RcpID = " & RcpID & " and CustID = " & iCust)

            For Each dData_row As DataRow In dData.Tables(0).Rows
                If dData_row.Item("tipe") = 1 Then 'inventory
                    TotalCost = TotalCost + ReceiptCost_ByInventory(dData_row.Item("kodeid").ToString, dData_row.Item("qty"), dData_row.Item("lossf"), iCust, ccid)
                Else
                    TotalCost = TotalCost + Cek_Receipecc(dData_row.Item("kodeid").ToString, iCust, ccid)
                End If
            Next

        Catch ex As Exception
            errLog(Err.Description, "clsInventory_stkCost:ReceiptCost_ByReceipe")
            Err.Clear()
        End Try

        cCmd = Nothing


        Return TotalCost

    End Function

    Private Function Cek_Receipe(ByVal RcpID As Integer, ByVal iCust As Integer) As Decimal

        Dim TotalCost As Decimal

        Dim cCmd As New clsDbo("")

        Try

            Dim dData As New DataSet
            dData = cCmd.ReturnDataset("Select IFNULL(cost,0) AS cost from cld_inv_rcp where ID = " & RcpID & " and CustID = " & iCust)

            If dData.Tables(0).Rows.Count > 0 Then
                TotalCost = dData.Tables(0).Rows(0).Item("Cost")  'ReceiptCost_ByReceipe(dData.Tables(0).Rows(0).Item("ID").ToString, iCust)
            End If

        Catch ex As Exception
            errLog(Err.Description, "clsInventory_stkCost:Cek_Receipe")
            Err.Clear()
        End Try

        cCmd = Nothing

        Return TotalCost

    End Function

    Private Function Cek_Receipecc(ByVal RcpID As Integer, ByVal iCust As Integer, ByVal ccid As Integer) As Decimal

        Dim TotalCost As Decimal

        Dim cCmd As New clsDbo("")

        Try

            Dim dData As New DataSet
            dData = cCmd.ReturnDataset("Select IFNULL(cost,0) AS cost from cld_inv_rcp_cc where rcpid = " & RcpID & " and ccid = " & ccid & " and CustID = " & iCust)

            If dData.Tables(0).Rows.Count > 0 Then
                TotalCost = dData.Tables(0).Rows(0).Item("Cost")  'ReceiptCost_ByReceipe(dData.Tables(0).Rows(0).Item("ID").ToString, iCust)
            End If

        Catch ex As Exception
            errLog(Err.Description, "clsInventory_stkCost:Cek_Receipe")
            Err.Clear()
        End Try

        cCmd = Nothing

        Return TotalCost

    End Function
    Private Sub Update_ReceipeCost(ByVal RcpID As Integer, ByVal TotalCost As Integer, ByVal iCust As Integer)

        Dim str As String
        Dim c_cmd As New clsDbo("")

        Try

            str = "update cld_inv_rcp set Cost = " & TotalCost & " where ID = " & RcpID & " and Custid = " & iCust
            c_cmd.executeNonQuery(str)

        Catch ex As Exception
            errLog(Err.Description, "clsInventory_stkCost:Update_ReceipeCost")
            Err.Clear()
        End Try

        c_cmd = Nothing


    End Sub

    Private Sub Update_ReceipeCostcc(ByVal rcpid As Integer, ByVal TotalCost As Integer, ByVal iCust As Integer, ByVal ccid As Integer)

        Dim str As String
        Dim c_cmd As New clsDbo("")

        Try

            If My.Settings.rcp_cc = 1 Then
                If c_cmd.ExecuteScalar("SELECT COUNT(rcpid) FROM cld_inv_rcp_cc WHERE rcpid = " & rcpid & " AND ccid = " & ccid & " and Custid = " & iCust & "", 0) > 0 Then
                    c_cmd.ReNew()
                    str = "update cld_inv_rcp_cc set Cost = " & TotalCost & " where rcpid = " & rcpid & " and ccid = " & ccid & " and Custid = " & iCust
                    c_cmd.executeNonQuery(str)
                Else
                    c_cmd.ReNew()
                    str = "insert into cld_inv_rcp_cc(rcpid, CustID, cost, ccid) values (" & rcpid & "," & iCust & "," & TotalCost & "," & ccid & ")"
                    c_cmd.executeNonQuery(str)
                End If
            End If

            'str = "update cld_inv_rcp set Cost = " & TotalCost & " where ID = " & rcpid & " and Custid = " & iCust
            'c_cmd.executeNonQuery(str)

        Catch ex As Exception
            errLog(Err.Description, "clsInventory_stkCost:Update_ReceipeCost")
            Err.Clear()
        End Try

        c_cmd = Nothing


    End Sub

End Class
