Public Class clsInventory_Usage

    Public Sub ProcessUsage(ByVal Custid As Integer, ByVal oid As Integer, ByVal ItemNo As String, ByVal dQty As Decimal, ByVal dInvDate As Date, ByVal binsert1 As Boolean, ByVal iSalesID As Integer)

        Try


            Dim c_cmd As New clsDbo("")
            c_cmd.executeNonQuery("Delete From cld_inv_transusage where linkid = " & iSalesID & " and custId = " & Custid)
            c_cmd.dispose()
            c_cmd = Nothing


            getproperty_LinkItem(Custid, oid, ItemNo, dQty, dInvDate, iSalesID)

        Catch ex As Exception
            errLog(Err.Description, "clsInventory_Usage:ProcessUsage")
            Err.Clear()
        End Try

    End Sub
    Public Sub ProcessUsagePO(ByVal Custid As Integer, ByVal oid As Integer, ByVal ItemNo As String, ByVal dQty As Decimal, ByVal dInvDate As Date, ByVal binsert1 As Boolean, ByVal iSalesID As Integer, ByVal sSalesNo As String)

        Try
            Dim c_cmd As New clsDbo("")


            c_cmd.executeNonQuery("Update cld_inv_transusage_cpo set Flag = 2 where TYPE = 10 and linkid = " & iSalesID & " and custId = " & Custid)
            c_cmd.dispose()
            c_cmd = Nothing


            getproperty_LinkItem2PO(Custid, oid, ItemNo, dQty, dInvDate, iSalesID, sSalesNo)

        Catch ex As Exception
            errLog(Err.Description, "clsInventory_Usage:ProcessUsage")
            Err.Clear()
        End Try

    End Sub
    Private Sub getproperty_LinkItem2PO(ByVal Custid As Integer, ByVal oid As Integer, ByVal ItemNo As String, ByVal dQty As Decimal, ByVal dInvDate As Date, ByVal iSalesID As Integer, ByVal sSalesNo As String)

        Dim cmd As New clsDbo("", 1)
        Dim cmdinv As New clsDbo("")
        Dim c_stkCost As New clsInventory_stkCost

        Try

            bprocess_Usage = False

            Dim dtitem As New DataSet
            Dim dtstock As New DataSet
            dtitem = cmd.ReturnDataset("SELECT  `cld_itemmaster`.`ItemNo`, `cld_itemmaster`.`ItemName`, `cld_inv_cc`.`olId`, `cld_itemmaster`.`itype`, `cld_itemmaster`.`iID`, `cld_itemmaster`.`rID`,  `cld_itemmaster`.`Factor` , `cld_inv_cc`.`ID` AS ccid,`cld_inv_cc`.`Name` AS ccNm " &
                                      "FROM `cld_inv_cc` INNER JOIN `cld_itemmaster_cc` ON (`cld_inv_cc`.`ID` = `cld_itemmaster_cc`.`Ccid`) INNER JOIN cld_itemmaster ON cld_itemmaster.Itemno = cld_itemmaster_cc.itemNo " &
                                            "where `cld_itemmaster`.ItemNo = '" & ItemNo & "' and `cld_itemmaster`.`Custid` = " & Custid & " and `cld_inv_cc`.`oliD` = " & oid & " AND `cld_itemmaster`.`link` = 1 AND `cld_itemmaster`.`nonusage` = 0 ")

            If dtitem.Tables(0).Rows.Count > 0 Then

                If IsDBNull(dtitem.Tables(0).Rows(0).Item("itype")) = False Then

                    If dtitem.Tables(0).Rows(0).Item("itype") = 1 And IsDBNull(dtitem.Tables(0).Rows(0).Item("iID")) = False Then
                        dtstock = cmdinv.ReturnDataset("SELECT `cld_inv_stk`.`Number` AS stkNo, `cld_inv_stk`.`Name` AS stkNm , `cld_inv_stk`.`Unit1`, `cld_inv_stk`.`Unit2`, `cld_inv_stk`.`Unit3`, `cld_inv_stk`.`Factor1`, `cld_inv_stk`.`Factor2`, `cld_inv_stk`.ID " &
                                      "FROM `cld_inv_stk` 
                                      WHERE id = " & dtitem.Tables(0).Rows(0).Item("iID") & " and Custid = " & Custid)

                        Dim dCost As Decimal = 0

                        dCost = c_stkCost.get_stkCost(dtitem.Tables(0).Rows(0).Item("iID"), dtitem.Tables(0).Rows(0).Item("ccID"), Custid)





                            'link ke Stock master
                            insert_Transusage_StkPO(Custid, dtitem.Tables(0).Rows(0).Item("ccID"), dtitem.Tables(0).Rows(0).Item("ccNm"), dtitem.Tables(0).Rows(0).Item("iID"), dtstock.Tables(0).Rows(0).Item("stkNo"), dtstock.Tables(0).Rows(0).Item("stkNm"), dQty * dtitem.Tables(0).Rows(0).Item("Factor"), dtstock.Tables(0).Rows(0).Item("Factor2"), dtstock.Tables(0).Rows(0).Item("Unit2"), dtstock.Tables(0).Rows(0).Item("Unit3"), dInvDate.ToString("yyyy-MM-dd HH:mm:ss"), iSalesID, dCost, sSalesNo, oid, True)
                        '  c_stkCost.Set_MenuItemCost(dData.Tables(0).Rows(0).Item("ItemNo"), dCost, dData.Tables(0).Rows(0).Item("ccID"), Custid)


                    ElseIf dtitem.Tables(0).Rows(0).Item("itype") = 2 And IsDBNull(dtitem.Tables(0).Rows(0).Item("rID")) = False Then
                        dtstock = cmdinv.ReturnDataset("SELECT `cld_inv_stk`.`Number` AS stkNo, `cld_inv_stk`.`Name` AS stkNm , `cld_inv_stk`.`Unit1`, `cld_inv_stk`.`Unit2`, `cld_inv_stk`.`Unit3`, `cld_inv_stk`.`Factor1`, `cld_inv_stk`.`Factor2`, `cld_inv_rcp`.ID AS rcpID " &
                                      "FROM `cld_inv_stk` INNER JOIN `cld_inv_rcp` ON (`cld_inv_rcp`.id = `cld_inv_stk`.blink) AND (`cld_inv_rcp`.`CustID` = `cld_inv_stk`.`CustID`) 
                                      WHERE `cld_inv_rcp`.id = " & dtitem.Tables(0).Rows(0).Item("rID") & " and `cld_inv_stk`.`Custid` = " & Custid)

                        'link ke receipe
                        getproperty_ReceipeDetailPO(Custid, dtitem.Tables(0).Rows(0).Item("ccID"), dtitem.Tables(0).Rows(0).Item("ccNm"), dtitem.Tables(0).Rows(0).Item("rID"), dQty * dtitem.Tables(0).Rows(0).Item("Factor"), dInvDate.ToString("yyyy-MM-dd HH:mm:ss"), iSalesID, ItemNo, oid, sSalesNo)


                    End If

                End If

            End If

            dtitem.Dispose()
            dtstock.Dispose()

        Catch ex As Exception
            errLog(Err.Description, "Cls_InvUsage.getproperty_LinkItem2")
            Err.Clear()
        End Try

        cmd.dispose()
        cmd = Nothing


    End Sub
    Private Sub insert_Transusage_StkPO(ByVal iCust As Integer, ByVal iCC As Integer, ByVal sCc As String, ByVal iStk As Integer, ByVal sStkno As String, ByVal sStknm As String, ByVal dqtyFac As Decimal, ByVal dFactor As Decimal, ByVal sU2 As String, ByVal sU3 As String, ByVal dInvDate As Date, ByVal iSalesID As Integer, ByVal dcostStock As Decimal, ByVal sNumber As String, ByVal iOutlet As Integer, Optional ByVal bStock As Boolean = True)

        Dim c_cmd As New clsDbo("")
        Dim totalQty As Decimal = 0
        Dim str As String = ""
        Try
            'bStock, jadi klo true, brarti linknya langsung ke stk def dan pemakainnya, dari satuan inventory
            '      ,      klo false, brarti linknya dari receipe dan dari satuan receipe
            'perubahan 10 juni 22, menghilangkan cek record untuk mempercepat process



            ' If c_cmd.ExecuteScalar("Select CustID From cld_inv_transusage where Custid = " & iCust & " and type = 1 and cc = " & iCC & " and stk_ID =  " & iStk & " and linkid = " & iSalesID & " and InvDate = '" & dInvDate.ToString("yyyy-MM-dd HH:mm:ss") & "'", 1000) = 0 Then

            If bStock Then

                If dFactor = 0 Then
                    dFactor = 1
                End If
                If dqtyFac = 0 Then
                    dqtyFac = 1
                End If
                totalQty = dqtyFac
                'insert
                str = "insert into cld_inv_transusage_cpo(CustID, InvDate, type, Cc, Cc_name, Stk_Id, Stk_no, Stk_Name, U2, U3, Factor2, ttlStock, Cost, Linkid, flagtrans, U2_nm, U3_nm, CreatedBy, CreatedDate, smNo, Outletid) values (" & iCust & ", '" & dInvDate.ToString("yyyy-MM-dd HH:mm:ss") & "', 10, " & iCC & ", '" & sCc & "', " & iStk & ", '" & sStkno & "', '" & sStknm & "'," & dqtyFac & ", 0 ," & dFactor & ", " & totalQty & "," & dcostStock & ", " & iSalesID & " , 1, '" & sU2 & "','" & sU3 & "','SysAdmin','" & dInvDate.ToString("yyyy-MM-dd HH:mm:ss") & "','" & sNumber & "'," & iOutlet & ")"
            Else

                If dFactor = 0 Then
                    dFactor = 1
                End If
                If dqtyFac = 0 Then
                    dqtyFac = 1
                End If
                totalQty = CDec(dqtyFac / dFactor)

                str = "insert into cld_inv_transusage_cpo(CustID, InvDate, type, Cc, Cc_name, Stk_Id, Stk_no, Stk_Name, U2, U3, Factor2, ttlStock, Cost, Linkid, flagtrans, U2_nm, U3_nm, CreatedBy, CreatedDate, smNo, Outletid) values (" & iCust & ", '" & dInvDate.ToString("yyyy-MM-dd HH:mm:ss") & "', 10, " & iCC & ", '" & sCc & "', " & iStk & ", '" & sStkno & "', '" & sStknm & "',0 ," & dqtyFac & ", " & dFactor & ", " & totalQty & "," & dcostStock & ", " & iSalesID & " , 1, '" & sU2 & "','" & sU3 & "','SysAdmin','" & dInvDate.ToString("yyyy-MM-dd HH:mm:ss") & "','" & sNumber & "'," & iOutlet & ")"

            End If



            Dim cmdprocess As New clsDbo("")
            cmdprocess.executeNonQuery(str)

            cmdprocess.dispose()
            cmdprocess = Nothing

            bprocess_Usage = True

            'masukin data ke usage summary
            Set_UsageSummaryPO(iCust, dInvDate, iCC, iStk, totalQty, 1)


        Catch ex As Exception
            errLog(Err.Description, "Cls_InvUsage.insert_transactionstock " & str)
            Err.Clear()
        End Try

        c_cmd.dispose()
        c_cmd = Nothing

    End Sub
    Private Sub getproperty_LinkItemPO(ByVal Custid As Integer, ByVal oid As Integer, ByVal ItemNo As String, ByVal dQty As Decimal, ByVal dInvDate As Date, ByVal iSalesID As Integer, ByVal sSalesNo As String)

        Dim cmd As New clsDbo("")
        Dim c_stkCost As New clsInventory_stkCost

        Try

            bprocess_Usage = False

            Dim dData As New DataSet
            dData = cmd.ReturnDataset("SELECT  `cld_itemmaster`.`ItemNo`, `cld_itemmaster`.`ItemName`, `cld_inv_cc`.`olId`, `cld_itemmaster`.`itype`, `cld_itemmaster`.`iID`, `cld_itemmaster`.`rID`,  `cld_itemmaster`.`Factor` , `cld_inv_cc`.`ID` AS ccid,`cld_inv_cc`.`Name` AS ccNm, " &
                                      "`cld_inv_stk`.`Number` AS stkNo, `cld_inv_stk`.`Name` AS stkNm , `cld_inv_stk`.`Unit1`, `cld_inv_stk`.`Unit2`, `cld_inv_stk`.`Unit3`, `cld_inv_stk`.`Factor1`, `cld_inv_stk`.`Factor2`, `cld_inv_rcp`.ID AS rcpID " &
                                      "FROM `cld_inv_cc` INNER JOIN `cld_itemmaster_cc` ON (`cld_inv_cc`.`ID` = `cld_itemmaster_cc`.`Ccid`) INNER JOIN cld_itemmaster ON cld_itemmaster.Itemno = cld_itemmaster_cc.itemNo " &
                                      "LEFT JOIN `cld_inv_stk` ON (`cld_itemmaster`.`iId` = `cld_inv_stk`.`ID`) AND (`cld_itemmaster`.`CustID` = `cld_inv_stk`.`CustID`) " &
                                      "LEFT JOIN `cld_inv_rcp` ON (`cld_itemmaster`.`rID` = `cld_inv_rcp`.`ID`) AND (`cld_itemmaster`.`CustID` = `cld_inv_rcp`.`CustID`) " &
                                      "where `cld_itemmaster`.ItemNo = '" & ItemNo & "' and `cld_itemmaster`.`Custid` = " & Custid & " and `cld_inv_cc`.`oliD` = " & oid & " AND `cld_itemmaster`.`link` = 1 AND `cld_itemmaster`.`nonusage` = 0 ")

            If dData.Tables(0).Rows.Count > 0 Then

                If IsDBNull(dData.Tables(0).Rows(0).Item("itype")) = False Then

                    If dData.Tables(0).Rows(0).Item("itype") = 1 And IsDBNull(dData.Tables(0).Rows(0).Item("iID")) = False Then

                        Dim dCost As Decimal = 0

                        dCost = c_stkCost.get_stkCost(dData.Tables(0).Rows(0).Item("iID"), dData.Tables(0).Rows(0).Item("ccID"), Custid)


                            'link ke Stock master
                            insert_Transusage_StkPO(Custid, dData.Tables(0).Rows(0).Item("ccID"), dData.Tables(0).Rows(0).Item("ccNm"), dData.Tables(0).Rows(0).Item("iID"), dData.Tables(0).Rows(0).Item("stkNo"), dData.Tables(0).Rows(0).Item("stkNm"), dQty * dData.Tables(0).Rows(0).Item("Factor"), dData.Tables(0).Rows(0).Item("Factor2"), dData.Tables(0).Rows(0).Item("Unit2"), dData.Tables(0).Rows(0).Item("Unit3"), dInvDate.ToString("yyyy-MM-dd HH:mm:ss"), iSalesID, dCost, sSalesNo, oid, True)
                        '  c_stkCost.Set_MenuItemCost(dData.Tables(0).Rows(0).Item("ItemNo"), dCost, dData.Tables(0).Rows(0).Item("ccID"), Custid)


                    ElseIf dData.Tables(0).Rows(0).Item("itype") = 2 And IsDBNull(dData.Tables(0).Rows(0).Item("rID")) = False Then
                        'link ke receipe
                        getproperty_ReceipeDetailPO(Custid, dData.Tables(0).Rows(0).Item("ccID"), dData.Tables(0).Rows(0).Item("ccNm"), dData.Tables(0).Rows(0).Item("rID"), dQty * dData.Tables(0).Rows(0).Item("Factor"), dInvDate.ToString("yyyy-MM-dd HH:mm:ss"), iSalesID, ItemNo, oid, sSalesNo)


                    End If

                End If

            End If

            dData.Dispose()

        Catch ex As Exception
            errLog(Err.Description, "Cls_InvUsage.getproperty_LinkItem")
            Err.Clear()
        End Try

        cmd.dispose()
        cmd = Nothing


    End Sub
    Private Sub getproperty_LinkItem(ByVal Custid As Integer, ByVal oid As Integer, ByVal ItemNo As String, ByVal dQty As Decimal, ByVal dInvDate As Date, ByVal iSalesID As Integer)
        Try

            Dim cmd As New clsDbo("")

            Dim dData As New DataSet
            dData = cmd.ReturnDataset("SELECT  `cld_itemmaster`.`ItemNo`, `cld_itemmaster`.`ItemName`, `cld_inv_cc`.`olId`, `cld_itemmaster`.`itype`, `cld_itemmaster`.`iID`, `cld_itemmaster`.`rID`,  `cld_itemmaster`.`Factor` , `cld_inv_cc`.`ID` AS ccid,`cld_inv_cc`.`Name` AS ccNm, " & _
                                      "`cld_inv_stk`.`Number` AS stkNo, `cld_inv_stk`.`Name` AS stkNm , `cld_inv_stk`.`Unit1`, `cld_inv_stk`.`Unit2`, `cld_inv_stk`.`Unit3`, `cld_inv_stk`.`Factor1`, `cld_inv_stk`.`Factor2`, `cld_inv_rcp`.ID AS rcpID " & _
                                      "FROM `cld_inv_cc` INNER JOIN `cld_itemmaster` ON (`cld_inv_cc`.`CustId` = `cld_itemmaster`.`CustID`) " & _
                                      "LEFT JOIN `cld_inv_stk` ON (`cld_itemmaster`.`iId` = `cld_inv_stk`.`ID`) AND (`cld_itemmaster`.`CustID` = `cld_inv_stk`.`CustID`) " & _
                                      "LEFT JOIN `cld_inv_rcp` ON (`cld_itemmaster`.`rID` = `cld_inv_rcp`.`ID`) AND (`cld_itemmaster`.`CustID` = `cld_inv_rcp`.`CustID`) " & _
                                      "where `cld_itemmaster`.ItemNo = '" & ItemNo & "' and `cld_itemmaster`.`Custid` = " & Custid & " and `cld_inv_cc`.`oliD` = " & oid & " AND `cld_itemmaster`.`link` = 1")

            If dData.Tables(0).Rows.Count > 0 Then

                If IsDBNull(dData.Tables(0).Rows(0).Item("itype")) = False Then

                    If dData.Tables(0).Rows(0).Item("itype") = 1 And IsDBNull(dData.Tables(0).Rows(0).Item("iID")) = False Then
                        'link ke Stock master
                        insert_Transusage_Stk(Custid, dData.Tables(0).Rows(0).Item("ccID"), dData.Tables(0).Rows(0).Item("ccNm"), dData.Tables(0).Rows(0).Item("iID"), dData.Tables(0).Rows(0).Item("stkNo"), dData.Tables(0).Rows(0).Item("stkNm"), dQty * dData.Tables(0).Rows(0).Item("Factor"), dInvDate.ToString("yyyy-MM-dd HH:mm:ss"))




                    ElseIf dData.Tables(0).Rows(0).Item("itype") = 2 And IsDBNull(dData.Tables(0).Rows(0).Item("rID")) = False Then
                        'link ke receipe
                        getproperty_ReceipeDetail(Custid, dData.Tables(0).Rows(0).Item("ccID"), dData.Tables(0).Rows(0).Item("ccNm"), dData.Tables(0).Rows(0).Item("rID"), dQty * dData.Tables(0).Rows(0).Item("Factor"), dInvDate.ToString("yyyy-MM-dd HH:mm:ss"), iSalesID)


                    End If

                End If

            End If

            cmd.dispose()
            cmd = Nothing


        Catch ex As Exception
            errLog(Err.Description, "Cls_InvUsage.getproperty_LinkItem")
            Err.Clear()


        End Try


    End Sub


    Private Sub getproperty_ReceipeDetail(ByVal iCust As Integer, ByVal iCC As Integer, ByVal sCc As String, ByVal iRcp As Integer, ByVal dqtyFac As Decimal, ByVal dInvDate As Date, ByVal iSalesID As Integer)


        Try

            Dim cmd As New clsDbo("")

            Dim dData As New DataSet
            dData = cmd.ReturnDataset("SELECT * from cld_inv_rcpdtl where rcpid = " & iRcp & " and Custid = " & iCust)

            If dData.Tables(0).Rows.Count > 0 Then


                For x = 0 To dData.Tables(0).Rows.Count - 1

                    If IsDBNull(dData.Tables(0).Rows(x).Item("tipe")) = False Then

                        If dData.Tables(0).Rows(x).Item("tipe") = 1 And IsDBNull(dData.Tables(0).Rows(x).Item("KodeiD")) = False Then
                            'link ke Stock master
                            'insert_Transusage_Stk(Custid, dData.Tables(0).Rows(0).Item("ccID"), dData.Tables(0).Rows(0).Item("ccNm"), dData.Tables(0).Rows(0).Item("iID"), dData.Tables(0).Rows(0).Item("stkNo"), dData.Tables(0).Rows(0).Item("stkNm"), dQty * dData.Tables(0).Rows(0).Item("Factor"), dData.Tables(0).Rows(0).Item("Factor1"), dData.Tables(0).Rows(0).Item("Unit2"), dData.Tables(0).Rows(0).Item("Unit3"), Now.ToString("yyyy-MM-dd HH:mm:ss"), iSalesID)

                            Dim dDataReceipe As New DataSet
                            dDataReceipe = cmd.ReturnDataset("Select * from cld_inv_stk where CustID = " & iCust & " and ID = " & dData.Tables(0).Rows(x).Item("kodeID"))

                            If dDataReceipe.Tables(0).Rows.Count > 0 Then

                                'insert_Transusage_Stk(iCust, iCC, sCc, dDataReceipe.Tables(0).Rows(0).Item("ID"), dDataReceipe.Tables(0).Rows(0).Item("Number"), dDataReceipe.Tables(0).Rows(0).Item("Name"), dqtyFac * dData.Tables(0).Rows(x).Item("qty"), dDataReceipe.Tables(0).Rows(0).Item("Factor2"), dDataReceipe.Tables(0).Rows(0).Item("unit2"), dDataReceipe.Tables(0).Rows(0).Item("unit3"), dInvDate.ToString("yyyy-MM-dd HH:mm:ss"), iSalesID, False)
                                insert_Transusage_Stk(iCust, iCC, sCc, dDataReceipe.Tables(0).Rows(0).Item("ID"), dDataReceipe.Tables(0).Rows(0).Item("Number"), dDataReceipe.Tables(0).Rows(0).Item("Name"), dqtyFac * dData.Tables(0).Rows(x).Item("qty"), dInvDate.ToString("yyyy-MM-dd HH:mm:ss"))



                            End If


                        ElseIf dData.Tables(0).Rows(x).Item("tipe") = 2 And IsDBNull(dData.Tables(0).Rows(x).Item("KodeID")) = False Then
                            'link ke receipe


                            'Dim dDataReceipe As New DataSet
                            'dDataReceipe = cmd.ReturnDataset("Select * from cld_inv_rcpdtl where CustID = " & iCust & " and rcpid = " & dData.Tables(0).Rows(x).Item("kodeID"))

                            'If dDataReceipe.Tables(0).Rows.Count > 0 Then

                            ' getproperty_ReceipeDetail(iCust, iCC, sCc, dDataReceipe.Tables(0).Rows(0).Item("rcpID"), dqtyFac * dDataReceipe.Tables(0).Rows(0).Item("Qty"), dInvDate.ToString("yyyy-MM-dd HH:mm:ss"), iSalesID)
                            getproperty_ReceipeDetail(iCust, iCC, sCc, dData.Tables(0).Rows(x).Item("KodeID"), dqtyFac * dData.Tables(0).Rows(x).Item("qty"), dInvDate.ToString("yyyy-MM-dd HH:mm:ss"), iSalesID)


                            ' End If


                        End If

                    End If

                Next

            End If

            cmd.dispose()
            cmd = Nothing

        Catch ex As Exception
            errLog(Err.Description, "Cls_InvUsage.getproperty_ReceipeData")
            Err.Clear()
        End Try

    End Sub
    Private Sub getproperty_ReceipeDetailPO(ByVal iCust As Integer, ByVal iCC As Integer, ByVal sCc As String, ByVal iRcp As Integer, ByVal dqtyFac As Decimal, ByVal dInvDate As Date, ByVal iSalesID As Integer, ByVal sItemNo As String, ByVal iOutletID As Integer, ByVal sSalesNo As String)

        Dim cmd As New clsDbo("")

        Try

            Dim dCost As Decimal = 0
            Dim x As Integer
            '  Dim Totaldcost As Decimal = 0

            Dim c_stkCost As New clsInventory_stkCost

            Dim dData As New DataSet
            dData = cmd.ReturnDataset("SELECT * from cld_inv_rcpdtl where rcpid = " & iRcp & " and Custid = " & iCust)

            If dData.Tables(0).Rows.Count > 0 Then


                For x = 0 To dData.Tables(0).Rows.Count - 1

                    If IsDBNull(dData.Tables(0).Rows(x).Item("tipe")) = False Then

                        'tipe 1 adalah receipe yang ke stockmaster
                        If dData.Tables(0).Rows(x).Item("tipe") = 1 And IsDBNull(dData.Tables(0).Rows(x).Item("KodeiD")) = False Then
                            'link ke Stock master
                            'insert_Transusage_Stk(Custid, dData.Tables(0).Rows(0).Item("ccID"), dData.Tables(0).Rows(0).Item("ccNm"), dData.Tables(0).Rows(0).Item("iID"), dData.Tables(0).Rows(0).Item("stkNo"), dData.Tables(0).Rows(0).Item("stkNm"), dQty * dData.Tables(0).Rows(0).Item("Factor"), dData.Tables(0).Rows(0).Item("Factor1"), dData.Tables(0).Rows(0).Item("Unit2"), dData.Tables(0).Rows(0).Item("Unit3"), Now.ToString("yyyy-MM-dd HH:mm:ss"), iSalesID)

                            Dim dDataReceipe As New DataSet
                            dDataReceipe = cmd.ReturnDataset("Select * from cld_inv_stk where CustID = " & iCust & " and ID = " & dData.Tables(0).Rows(x).Item("kodeID"))

                            If dDataReceipe.Tables(0).Rows.Count > 0 Then

                                dCost = c_stkCost.get_stkCost(dDataReceipe.Tables(0).Rows(0).Item("ID"), iCC, iCust)
                                '   Totaldcost = Totaldcost + ((dCost * (dData.Tables(0).Rows(x).Item("qty") + (dData.Tables(0).Rows(x).Item("qty") * dData.Tables(0).Rows(x).Item("lossf") / 100))) / dDataReceipe.Tables(0).Rows(0).Item("Factor2"))
                                insert_Transusage_StkPO(iCust, iCC, sCc, dDataReceipe.Tables(0).Rows(0).Item("ID"), dDataReceipe.Tables(0).Rows(0).Item("Number"), dDataReceipe.Tables(0).Rows(0).Item("Name"), dqtyFac * (dData.Tables(0).Rows(x).Item("qty") + (dData.Tables(0).Rows(x).Item("qty") * dData.Tables(0).Rows(x).Item("lossf") / 100)), dDataReceipe.Tables(0).Rows(0).Item("Factor2"), dDataReceipe.Tables(0).Rows(0).Item("unit2"), dDataReceipe.Tables(0).Rows(0).Item("unit3"), dInvDate.ToString("yyyy-MM-dd HH:mm:ss"), iSalesID, dCost, sSalesNo, iOutletID, False)

                            End If
                            dDataReceipe.Dispose()




                        End If

                    End If

                Next

            End If


            dData.Dispose()


        Catch ex As Exception
            errLog(Err.Description, "Cls_InvUsage.getproperty_ReceipeData")
            Err.Clear()
        End Try

        cmd.dispose()
        cmd = Nothing

    End Sub
    'Private Sub insert_Transusage_Stk_old(ByVal iCust As Integer, ByVal iCC As Integer, ByVal sCc As String, ByVal iStk As Integer, ByVal sStkno As String, ByVal sStknm As String, ByVal dqtyFac As Decimal, ByVal dFactor As Decimal, ByVal sU2 As String, ByVal sU3 As String, ByVal dInvDate As Date, ByVal iSalesID As Integer, Optional ByVal bStock As Boolean = True)
    '    Try


    '        'bStock, jadi klo true, brarti linknya langsung ke stk def dan pemakainnya, dari satuan inventory
    '        '      ,      klo false, brarti linknya dari receipe dan dari satuan receipe

    '        Dim c_cmd As New clsDbo("")
    '        Dim c_stkCost As New clsInventory_stkCost
    '        Dim str As String = ""
    '        Dim clsprocess As New clsInventoryProcess
    '        ' INVUSAGE
    '        clsprocess.InvstkId = iStk
    '        clsprocess.InvCc = iCC
    '        clsprocess.InvCcName = sCc
    '        clsprocess.InvCustId = iCust
    '        clsprocess.InvstkName = sStknm
    '        clsprocess.InvStkNo = sStkno
    '        clsprocess.InvDate = dInvDate



    '        If c_cmd.ExecuteScalar("Select CustID From cld_inv_transusage where Custid = " & iCust & " and type = 1 and cc = " & iCC & " and stk_ID =  " & iStk & " and linkid = " & iSalesID & " and InvDate = '" & dInvDate.ToString("yyyy-MM-dd HH:mm:ss") & "'", 1000) = 0 Then

    '            If bStock Then
    '                'insert
    '                str = "insert into cld_inv_transusage(CustID, InvDate, type, Cc, Cc_name, Stk_Id, Stk_no, Stk_Name, U2, U3, Factor2, ttlStock, Cost, Linkid, flagtrans, U2_nm, U3_nm, CreatedBy, CreatedDate) values (" & iCust & ", '" & dInvDate.ToString("yyyy-MM-dd HH:mm:ss") & "', 10, " & iCC & ", '" & sCc & "', " & iStk & ", '" & sStkno & "', '" & sStknm & "'," & dqtyFac & ", 0 ," & dFactor & ", " & dqtyFac & "," & CInt(c_stkCost.get_stkCost(iStk, iCC, iCust)) & ", " & iSalesID & " , 1, '" & sU2 & "','" & sU3 & "','SysAdmin','" & dInvDate.ToString("yyyy-MM-dd HH:mm:ss") & "')"
    '                clsprocess.invusageQty = dqtyFac
    '            Else

    '                If dFactor = 0 Then
    '                    dFactor = 1
    '                End If
    '                str = "insert into cld_inv_transusage(CustID, InvDate, type, Cc, Cc_name, Stk_Id, Stk_no, Stk_Name, U2, U3, Factor2, ttlStock, Cost, Linkid, flagtrans, U2_nm, U3_nm, CreatedBy, CreatedDate) values (" & iCust & ", '" & dInvDate.ToString("yyyy-MM-dd HH:mm:ss") & "', 10, " & iCC & ", '" & sCc & "', " & iStk & ", '" & sStkno & "', '" & sStknm & "',0 ," & dqtyFac & ", " & dFactor & ", " & CDec(dqtyFac / dFactor) & "," & CInt(c_stkCost.get_stkCost(iStk, iCC, iCust)) & ", " & iSalesID & " , 1, '" & sU2 & "','" & sU3 & "','SysAdmin','" & dInvDate.ToString("yyyy-MM-dd HH:mm:ss") & "')"
    '                clsprocess.invusageQty = CDec(dqtyFac / dFactor)
    '            End If

    '        Else
    '            Dim prv_use As Decimal = 0
    '            If bStock Then
    '                c_cmd.ReNew()
    '                prv_use = c_cmd.ExecuteScalar("select ttlStock from cld_inv_transusage where CustID = " & iCust & " and type = 1 and cc = " & iCC & " and stk_ID =  " & iStk & " and linkid = " & iSalesID & " and InvDate = '" & dInvDate.ToString("yyyy-MM-dd HH:mm:ss") & "'", 0)

    '                'update
    '                str = "Update cld_inv_transusage set U2 = U2 + " & dqtyFac & ", ttlStock = U2 + (U3/Factor2) , ModifBy = 'SysAdmin' , ModifDate ='" & Now.ToString("yyyy-MM-dd HH:mm:ss") & "' where CustID = " & iCust & " and type = 1 and cc = " & iCC & " and stk_ID =  " & iStk & " and linkid = " & iSalesID & " and InvDate = '" & dInvDate.ToString("yyyy-MM-dd HH:mm:ss") & "'"

    '                clsprocess.invusageQty = dqtyFac - prv_use
    '            Else
    '                c_cmd.ReNew()
    '                prv_use = c_cmd.ExecuteScalar("select ttlStock from cld_inv_transusage where CustID = " & iCust & " and type = 1 and cc = " & iCC & " and stk_ID =  " & iStk & " and linkid = " & iSalesID & " and InvDate = '" & dInvDate.ToString("yyyy-MM-dd HH:mm:ss") & "'", 0)

    '                str = "Update cld_inv_transusage set U3 = U3 + " & dqtyFac & ", ttlStock = U2 + (U3/Factor2) , ModifBy = 'SysAdmin' , ModifDate ='" & Now.ToString("yyyy-MM-dd HH:mm:ss") & "' where CustID = " & iCust & " and type = 1 and cc = " & iCC & " and stk_ID =  " & iStk & " and linkid = " & iSalesID & " and InvDate = '" & dInvDate.ToString("yyyy-MM-dd HH:mm:ss") & "'"

    '                clsprocess.invusageQty = CDec(dqtyFac / dFactor) - prv_use
    '            End If

    '        End If

    '        'Dim cmdprocess As New clsDbo("")
    '        c_cmd.ReNew()
    '        c_cmd.executeNonQuery(str)
    '        clsprocess.executeNonQuery_Inv_ProcessCls()
    '        'If dInvDate.Date = Now.Date Then
    '        '    c_cmd.executeNonQuery_Inv_CalcCost(iStk, iCC, iCust)
    '        '    c_cmd.executeNonQuery_Inv_CalcStock(iStk, iCC, iCust)
    '        'End If

    '        c_cmd.dispose()
    '        c_cmd = Nothing


    '    Catch ex As Exception
    '        errLog(Err.Description, "Cls_InvUsage.insert_transactionstock")
    '        Err.Clear()

    '    End Try

    'End Sub
    Public Sub insert_Transusage_Stk(ByVal iCust As Integer, ByVal iCC As Integer, ByVal sCc As String, ByVal iStk As Integer, ByVal sStkno As String, ByVal sStknm As String, ByVal usedqty As Decimal, ByVal dInvDate As Date)
        Try


            'Dim c_cmd As New clsDbo("")
            'Dim c_stkCost As New clsInventory_stkCost
            '15Okt24
            'Dim str As String = ""
            Dim clsprocess As New clsInventoryProcess
            ' INVUSAGE
            clsprocess.InvstkId = iStk
            clsprocess.InvCc = iCC
            clsprocess.InvCcName = sCc
            clsprocess.InvCustId = iCust
            clsprocess.InvstkName = sStknm
            clsprocess.InvStkNo = sStkno
            clsprocess.InvDate = dInvDate


            clsprocess.invusageQty = usedqty


            'Dim cmdprocess As New clsDbo("")
            'c_cmd.ReNew()
            'c_cmd.executeNonQuery(str)
            clsprocess.executeNonQuery_Inv_ProcessCls(False)
            'If dInvDate.Date = Now.Date Then
            '    c_cmd.executeNonQuery_Inv_CalcCost(iStk, iCC, iCust)
            '    c_cmd.executeNonQuery_Inv_CalcStock(iStk, iCC, iCust)
            'End If

            'c_cmd.dispose()
            'c_cmd = Nothing


        Catch ex As Exception
            errLog(Err.Description, "Cls_InvUsage.insert_transactionstock")
            Err.Clear()

        End Try

    End Sub
    Public Sub Set_UsageSummaryPO(ByVal icust As Integer, ByVal dInvDate As Date, ByVal iCC As Integer, ByVal iStk As Integer, ByVal dUsage As Decimal, ByVal iflag As Integer)

        Dim str As String = ""
        Dim c_cmd As New clsDbo("")
        Dim cinv As New clsInventory
        Dim dTotalUsage As Decimal = 0

        If iflag = 2 Then
            dUsage = -1 * dUsage
        End If

        Try

            ' PERBAIKAN: Ganti pola SELECT lalu INSERT/UPDATE (race condition) dengan INSERT ON DUPLICATE KEY UPDATE (atomic)
            ' Statement tunggal ini thread-safe dan tidak menyebabkan lock wait antara SELECT dan INSERT/UPDATE

            If iflag = 1 Then
                ' flag=1: tambahkan usage (insert jika baru, update jika sudah ada)
                str = "INSERT INTO cld_inv_transusagesum_cpo (CustID, InvDate, Cc, Stk_Id, ttlStock, flag)" &
                      " VALUES(" & icust & ",'" & dInvDate.ToString("yyyy-MM-dd HH:mm:ss") & "'," &
                      iCC & "," & iStk & "," & dUsage & ", 1)" &
                      " ON DUPLICATE KEY UPDATE ttlStock = ttlStock + VALUES(ttlStock), flag = 1"
            Else
                ' flag=2: kurangi usage (hanya jika hasilnya tidak minus)
                str = "UPDATE cld_inv_transusagesum_cpo SET flag = 1, ttlStock = ttlStock + " & dUsage &
                      " WHERE Stk_Id = " & iStk & " AND Cc = " & iCC &
                      " AND InvDate = '" & dInvDate.Date.ToString("yyyy-MM-dd HH:mm:ss") & "'" &
                      " AND (ttlStock + " & dUsage & ") >= 0"
            End If

            If str <> "" Then
                c_cmd.executeNonQuery(str)
            End If


        Catch ex As Exception
            errLog(Err.Description, "clsInventory_stkCost:Set_UsageSummaryPO")
            Err.Clear()
        End Try


        c_cmd.dispose()
        c_cmd = Nothing

    End Sub

End Class
