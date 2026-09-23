Imports MySql.Data.MySqlClient
Imports MySql.Data.Types
Public Class cls_recalculate
    Private myCmd As New MySqlCommand
    Public Sub InvCaclculate_rec(ByVal dstart As Date, ByVal Custid As Integer, ByVal username As String)

        Try
            Dim x As Integer = 0
            Dim i As Integer
            Dim df As Date
            Dim dmax As Date
            Dim dsin As DataSet
            Dim delstr As String
            Dim c_cmd As New clsDbo("")
            Dim CsOname As New clsInventory_stkOpname
            Dim clsinv As New clsInventory

            'tambahan erik 30 sept 2020
            Dim dOpenQty As Decimal = 0
            Dim dCost As Decimal = 0

            df = dstart
            dmax = dstart
            i = DateDiff(DateInterval.Day, df, Today())

            df.AddDays(-1)

            If i <> 0 Then


                Do While x < i

                    delstr = "delete from cld_inv_Trans where type = 6 and custid = " & Custid & " and invdate ='" & df.AddDays(1).ToString("yyyy-MM-dd") & "'"


                    executeNonQuery_Inv_CalcCost_rec(0, 0, Custid, df, df)


                    'process untuk salesdetail yang kosong
                    'Process_InvUsage_rec(df, Custid
                    clsinv.Process_InvUsage(df, Custid)

                    'closing di process dlu
                    executeNonQuery_Inv_ProcessCls_rec(df, df, Custid)

                    dsin = SelectDataset_VInvtransstock_rec(Custid, df)

                    If dsin.Tables(0).Rows.Count > 0 Then
                        c_cmd.executeNonQuery(delstr)

                        For Each row As DataRow In dsin.Tables(0).Rows

                            If row.Item("bclose") = 1 Then
                                dOpenQty = row.Item("CloseQty")
                            ElseIf row.Item("bclose") = 0 Then
                                'inv.OpenQty + inv.rcvqty - inv.rtnqty + inv.tinQty - inv.toutQty - inv.rwasteQty - inv.fwasteQty - inv.usageqty + inv.HOin - inv.HOout + inv.BPin - inv.BPOut
                                dOpenQty = row.Item("OpenQty") + row.Item("rcvqty") - row.Item("rtnqty") + row.Item("tinQty") - row.Item("toutQty") - row.Item("rwasteQty") - row.Item("fwasteQty") - row.Item("usageqty") + row.Item("HOin") - row.Item("HOout") + row.Item("BPin") - row.Item("BPOut")
                            Else
                                dOpenQty = 0
                            End If

                            If row.Item("bclose") = 1 Then

                                dCost = row.Item("closecost")
                            Else
                                dCost = row.Item("cost")

                            End If

                            c_cmd.ReNew()



                            dTotalQty = row.Item("ttlqty")

                            'CASE WHEN bclose = 0 THEN 0 ELSE   (OpenQty + rcvqty - rtnqty + tinQty - toutQty - rwasteQty - fwasteQty - usageqty - closeQty - HOout + Hoin + BPin - BPout)  END as varianceQty
                            If row.Item("bClose") <> 0 Then
                                dvariantQty = row.Item("ttlqty") - row.Item("closeQty")
                            Else
                                dvariantQty = 0
                            End If

                            dDate = row.Item("invdate")
                            If c_cmd.ExecuteScalar("SELECT COUNT(ccid) FROM cld_inv_transstockcls WHERE custid = " & Custid & " AND ccid= " & row.Item("cc") & " AND stkid= " & row.Item("stk_id") & " AND invdate='" & dDate.Date.AddDays(1).ToString("yyyy-MM-dd 00:00:00") & "'", 0) > 0 Then
                                If c_cmd.executeNonQuery("update cld_inv_transstockcls set  openqty = " & row.Item("ttlqty") & ", varianceQty = " & dvariantQty & ",
                                     opencost = " & row.Item("cost") & "
                                    where stkid = " & row.Item("stk_id") & " and ccid = " & row.Item("cc") & " and invdate = '" & dDate.AddDays(1).ToString("yyyy-MM-dd") & "' and custid = " & Custid & "") = False Then
                                    writeerrcsv(True, dDate.adddays(1), row.Item("cc"), row.Item("stk_id"))
                                End If


                            Else
                                If c_cmd.executeNonQuery("insert into cld_inv_transstockcls values ('" & dDate.Date.AddDays(1).ToString("yyyy-MM-dd") & "', " & Custid & "," & row.Item("stk_id") & ", '" & row.Item("stk_No") & "', '" & row.Item("Stk_name") & "'," &
                               " " & row.Item("cc") & ", '" & row.Item("cc_name") & "', " & row.Item("cost") & ", " & row.Item("ttlqty") & ", " & row.Item("Cost") & ", 0, 0, 0, " &
                               " 0, 0, 0, 0, " & dTotalQty & ", 0, 0, 0, " & dvariantQty & ", 0, 0, 0, 0, 0)") = False Then
                                    writeerrcsv(True, dDate.adddays(1), row.Item("cc"), row.Item("stk_id"))
                                End If
                            End If

                            If c_cmd.executeNonQuery("update cld_inv_transstockcls set  ttlqty = Openqty + rcvqty-rtnqty+tinqty-toutqty-rwasteqty-fwasteqty-usageqty-hooutqty+hoinqty-bpout+bpin where stkid = " & row.Item("stk_id") & " and ccid = " & row.Item("cc") & " and invdate = '" & dDate.Date.AddDays(1).ToString("yyyy-MM-dd") & "' and custid = " & Custid & "") = False Then
                                writeerrcsv(True, dDate.adddays(1), row.Item("cc"), row.Item("stk_id"))
                            End If

                            c_cmd.dispose()

                            'insert_invtrans_opening_rec(Custid, df, row.Item("cc"), row.Item("stk_id"), row.Item("stk_name"), dOpenQty, row.Item("factor1"), row.Item("stk_no"), row.Item("unit1"), row.Item("unit2"), dCost, row.Item("cc_name"), username)
                        Next
                    End If

                    df = df.AddDays(1)
                    x = x + 1
                Loop
            End If
            'End If
            If i = 0 Then
                executeNonQuery_Inv_ProcessCls_rec(dmax, Date.Now.AddDays(-1), Custid)
            End If

            executeNonQuery_Inv_CalcCost_rec(0, 0, Custid, Now, Now)
            'executeNonQuery_Inv_CalcStock_rec(0, 0, Custid)

            Dim clsInv_cost As New clsInventory_Receipe
            Dim clsinv_itemmaster As New clsInventory_ItemMaster

            'buat kalkulasi cost
            clsInv_cost.Process_UpdateCostReceipe(Custid, Now.Date.ToString("dd MMM yyyy"))
            clsinv_itemmaster.Process_UpdateCostitemMaster(Custid)


        Catch ex As Exception
            errLog(Err.Description, "clsInventory:InvCaclculate")
            Err.Clear()
        End Try
    End Sub

    Public Sub executeNonQuery_Inv_CalcCost_rec(ByVal istk As Integer, ByVal icc As Integer, ByVal CustId As Integer, ByVal dstart As Date, ByVal dend As Date)


        'Process_InvDataStock(dstart, dend)
        'dari cld_inv_transstockcls

        Dim c_cmd As New clsDbo("")
        Dim ds As DataSet

        Dim dCost As Decimal = 0


        errLog("Inv_CalculateCost Start-" & dstart.Date & "--" & dend.Date & " Time: " & Now, "clsDBO:executeNonQuery_Inv_CalcCost")

        Try

            c_cmd.executeNonQuery("Truncate table cld_inv_stkcost")
            c_cmd.ReNew()

            ds = c_cmd.ReturnDataset("SELECT " &
  "`stkData`.`InvDate`  AS `InvDate`, `stkData`.`Ccid` AS `Cc`, " &
  "`cld_inv_cc`.`Name`  AS `Cc_name`, `stkData`.`StkId`   AS `Stk_id`, " &
  "cld_inv_stk.Name AS Stk_Name, `stkData`.`CustID`   AS `CustID`, " &
  "IFNULL((SELECT SUM((`sd`.`ttlStock` * `sd`.`Cost`)) AS `data1` FROM `cld_inv_trans` `sd` WHERE ((`sd`.`type` IN (1,3,6,8,9)) AND (`sd`.`flagtrans` = 1) AND (`sd`.`ttlstock` > 0) AND (`sd`.`CustID` = `stkData`.`CustID`) AND (`sd`.`Stk_Id` = `stkData`.`StkId`) AND (`sd`.`InvDate` = `stkData`.`InvDate`) AND (`sd`.`Cc` = `stkData`.`Ccid`))),0) AS `Cost`, " &
  "IFNULL((SELECT SUM(`sd`.`ttlStock`) AS `data1` FROM `cld_inv_trans` `sd` WHERE ((`sd`.`type` IN (1,3,6,8,9)) AND (`sd`.`flagtrans` = 1) AND (`sd`.`ttlstock` > 0) AND (`sd`.`CustID` = `stkData`.`CustID`) AND (`sd`.`Stk_Id` = `stkData`.`StkId`) AND (`sd`.`InvDate` = `stkData`.`InvDate`) AND (`sd`.`Cc` = `stkData`.`Ccid`))),0) AS `CostQty` " &
  "FROM cld_inv_transstockcls AS stkData INNER JOIN `cld_inv_stk` " &
  "ON `stkData`.`CustID` = `cld_inv_stk`.`CustID` AND `cld_inv_stk`.`ID` = `stkData`.`StkId` " &
  "INNER JOIN cld_inv_cc ON cld_inv_cc.ID = stkData.Ccid AND cld_inv_cc.custid = stkData.custid " &
  "WHERE stkData.InvDate >= '" & dstart.Date.ToString("yyyy-MM-dd") & "' AND stkData.invDate <= '" & dend.Date.ToString("yyyy-MM-dd") & "' AND stkData.`custid` = " & CustId)

            If ds.Tables(0).Rows.Count > 0 Then
                For Each row As DataRow In ds.Tables(0).Rows
                    c_cmd.ReNew()

                    If row.Item("COstQty") <> 0 Then
                        dCost = row.Item("Cost") / row.Item("CostQty")
                    Else
                        dCost = row.Item("Cost")
                    End If
                    If dCost < 0 Then
                        dCost = dCost * -1
                    End If

                    ' PERBAIKAN: Ganti SELECT COUNT+INSERT/UPDATE (race condition) dengan INSERT ON DUPLICATE KEY UPDATE (atomic)
                    ' Kedua koneksi concurrent tidak bisa lagi sama-sama INSERT karena jika duplicate, otomatis jadi UPDATE
                    c_cmd.executeNonQuery("INSERT INTO cld_inv_stkcost (stkid, cc, cost, custid) VALUES (" &
                        row.Item("stk_id") & ", " & row.Item("cc") & ", " & dCost & ", " & row.Item("CustID") & ")" &
                        " ON DUPLICATE KEY UPDATE cost = VALUES(cost)")


                Next
            End If


        Catch ex As Exception
            errLog(Err.Description, "clsDBO:executeNonQuery_Inv_CalcCost")
            Err.Clear()
        End Try


        errLog("Inv_CalculateCost END" & Now, "clsDBO:executeNonQuery_Inv_CalcCost")

        dispose()
        c_cmd = Nothing



    End Sub
    Public Sub Process_InvUsage_rec(ByVal dDate As Date, ByVal iCustID As Integer)
        Dim c_cmd As New clsDbo("")
        Dim i_SalesID As Integer = 0
        Dim ds As DataSet
        Try

            ds = c_cmd.ReturnDataset("SELECT cld_salesdetail.Sid, cld_salesdetail.outletid, cld_salesdetail.smno, cld_salesdetail.ItemNo, cld_salesmaster.Busidate, cld_salesdetail.qty FROM cld_salesdetail INNER JOIN cld_salesmaster ON cld_salesmaster.number = cld_salesdetail.smno AND cld_salesmaster.olid = cld_salesdetail.outletid INNER JOIN cld_outlet ON cld_salesmaster.olid = cld_outlet.ID WHERE SID = 0 AND cld_outlet.otype = 2 and cld_salesmaster.Busidate = '" & dDate.Date.ToString("yyyy-MM-dd") & "' ORDER BY cld_salesdetail.smno, cld_salesdetail.ItemNo")

            If ds.Tables(0).Rows.Count > 0 Then

                For Each row As DataRow In ds.Tables(0).Rows
                    c_cmd.ReNew()
                    i_SalesID = c_cmd.ExecuteScalar("SELECT MAX(Sid) FROM cld_salesdetail where custid = " & iCustID)

                    c_cmd.ReNew()
                    c_cmd.executeNonQuery("Update cld_salesdetail Set SID = " & i_SalesID & " where smno = '" & row.Item("smno") & "' and ItemNo = '" & row.Item("ItemNo") & "' and Outletid = " & row.Item("Outletid"))

                    Dim c_invUsage As New clsInventory_Usage
                    c_invUsage.ProcessUsage(iCustID, row.Item("Outletid"), row.Item("ItemNo"), row.Item("qty"), dDate, True, i_SalesID)
                    c_invUsage = Nothing
                Next


            End If


        Catch ex As Exception

            errLog(Err.Description, "clsInventory:insertInvStockOpname")
            Err.Clear()
        End Try


        c_cmd.dispose()
        c_cmd = Nothing

    End Sub
    Public Sub executeNonQuery_Inv_ProcessCls_rec(ByVal dstart As Date, ByVal dend As Date, ByVal CustId As Integer)

        'Process_InvDataStock(dstart, dend)

        Dim c_cmd As New clsDbo("")
        Dim i_SalesID As Integer = 0
        Dim ds As DataSet

        Dim dTotalQty As Decimal = 0
        Dim dvariantQty As Decimal = 0
        Dim dDate As Date



        Try


            ' c_cmd.executeNonQuery("Delete from cld_inv_transstockcls WHERE InvDate >= '" & dstart.Date.ToString("yyyy-MM-dd") & "' AND InvDate <= '" & dend.Date.ToString("yyyy-MM-dd") & "' AND CustID = " & CustId)

            c_cmd.ReNew()


            errLog("Inv_ProcessClsQuery START " & Now & "-" & dstart.Date & ":" & dend.Date, "clsDBO:executeNonQuery_Inv_ProcessCls")

            'perubahan Bram 8 Nov23 dari type=5 menjadi type=12

            ds = c_cmd.ReturnDataset("SELECT " &
  "`i_trans`.`invDate`    AS `invDate`, `cld_inv_stk`.`CustID` AS `Custid`, " &
  "`i_trans`.`stkid`     AS `stk_id`, `cld_inv_stk`.`Number` AS `stk_No`, " &
  "`cld_inv_stk`.`Name`   AS `Stk_name`, `cld_inv_stk`.`Unit2`  AS `u2_nm`, " &
  "`i_trans`.`Ccid` AS `cc`, `cc`.`Name` AS `cc_name`, " &
  "IFNULL((SELECT `sd`.`Cost` FROM `cld_inv_trans` `sd` WHERE ((`sd`.`type` = 6) AND (`sd`.`flagtrans` = 1) AND (`sd`.`Stk_Id` = `i_trans`.`stkid`) AND (`sd`.`InvDate` = `i_trans`.`invDate`) AND (`sd`.`Cc` = `i_trans`.`Ccid`))),0) AS `OpenCost`, " &
  "IFNULL((SELECT `sd`.`ttlStock` FROM `cld_inv_trans` `sd` WHERE ((`sd`.`type` = 6) AND (`sd`.`flagtrans` = 1) AND (`sd`.`CustID` = `i_trans`.`custid`) AND (`sd`.`Stk_Id` = `i_trans`.`stkid`) AND (`sd`.`InvDate` = `i_trans`.`invDate`) AND (`sd`.`Cc` = `i_trans`.`Ccid`))),0) AS `OpenQty`, " &
  "IFNULL((SELECT `sd`.`Cost` FROM `cld_inv_stkcost` `sd` WHERE ((`sd`.`Custid` = `i_trans`.`custid`) AND (`sd`.`stkID` = `i_trans`.`stkid`) AND (`sd`.`cc` = `i_trans`.`Ccid`))),0) AS `Cost`, " &
  "IFNULL((SELECT SUM(`sd`.`ttlStock`) AS `ds` FROM `cld_inv_trans` `sd` WHERE ((`sd`.`type` = 3) AND (`sd`.`flagtrans` = 1) AND (`sd`.`CustID` = `i_trans`.`custid`) AND (`sd`.`Stk_Id` = `i_trans`.`stkid`) AND (`sd`.`InvDate` = `i_trans`.`invDate`) AND (`sd`.`Cc` = `i_trans`.`Ccid`))),0) AS `rcvQty`, " &
  "IFNULL((SELECT SUM(`sd`.`ttlStock`) AS `ds` FROM `cld_inv_trans` `sd` WHERE ((`sd`.`type` = 4) AND (`sd`.`flagtrans` = 1) AND (`sd`.`CustID` = `i_trans`.`custid`) AND (`sd`.`Stk_Id` = `i_trans`.`stkid`) AND (`sd`.`InvDate` = `i_trans`.`invDate`) AND (`sd`.`Cc` = `i_trans`.`Ccid`))),0) AS `rtnqty`, " &
  "IFNULL((SELECT SUM(`sd`.`ttlStock`) AS `ds` FROM `cld_inv_trans` `sd` WHERE ((`sd`.`type` = 1) AND (`sd`.`flagtrans` = 1) AND (`sd`.`CustID` = `i_trans`.`custid`) AND (`sd`.`Stk_Id` = `i_trans`.`stkid`) AND (`sd`.`InvDate` = `i_trans`.`invDate`) AND (`sd`.`Cc` = `i_trans`.`Ccid`))),0) AS `tinQty`, " &
  "IFNULL((SELECT SUM(`sd`.`ttlStock`) AS `ds` FROM (`cld_inv_trans` `sd` JOIN `cld_invlk_trf` ON(((`sd`.`Linkid` = `cld_invlk_trf`.`id`) AND (`sd`.`CustID` = `cld_invlk_trf`.`CustID`)))) WHERE ((`sd`.`type` = 1) AND (`sd`.`flagtrans` = 1) AND (`cld_invlk_trf`.`CustID` = `i_trans`.`custid`) AND (`sd`.`Stk_Id` = `i_trans`.`stkid`) AND (`sd`.`InvDate` = `i_trans`.`invDate`) AND (`cld_invlk_trf`.`ccFrom` = `i_trans`.`Ccid`))),0) AS `toutQty`, " &
  "IFNULL((SELECT SUM(`sd`.`ttlStock`) AS `ds` FROM `cld_inv_trans` `sd` WHERE ((`sd`.`type` = 2) AND (`sd`.`flagtrans` = 1) AND (`sd`.`CustID` = `i_trans`.`custid`) AND (`sd`.`Stk_Id` = `i_trans`.`stkid`) AND (`sd`.`InvDate` = `i_trans`.`invDate`) AND (`sd`.`Cc` = `i_trans`.`Ccid`))),0) AS `rwasteQty`, " &
  "0 AS `fwasteQty`, " &
  "IFNULL((SELECT SUM(`sd`.`ttlStock`) AS `ds` FROM `cld_inv_transusage` `sd` WHERE ((`sd`.`type` = 10) AND (`sd`.`flagtrans` = 1) AND (`sd`.`CustID` = `i_trans`.`custid`) AND (`sd`.`Stk_Id` = `i_trans`.`stkid`) AND (`sd`.`InvDate` = `i_trans`.`invDate`) AND (`sd`.`Cc` = `i_trans`.`Ccid`))),0) AS `usageqty`, " &
  "IFNULL((SELECT `sd`.`flagtrans` FROM `cld_inv_trans` `sd` WHERE ((`sd`.`type` = 12) AND (`sd`.`flagtrans` = 1) AND (`sd`.`CustID` = `i_trans`.`custid`) AND (`sd`.`Stk_Id` = `i_trans`.`stkid`) AND (`sd`.`InvDate` = `i_trans`.`invDate`) AND (`sd`.`Cc` = `i_trans`.`Ccid`))),0) AS `bClose`, " &
  "IFNULL((SELECT `sd`.`ttlStock` FROM `cld_inv_trans` `sd` WHERE ((`sd`.`type` = 12) AND (`sd`.`flagtrans` = 1) AND (`sd`.`CustID` = `i_trans`.`custid`) AND (`sd`.`Stk_Id` = `i_trans`.`stkid`) AND (`sd`.`InvDate` = `i_trans`.`invDate`) AND (`sd`.`Cc` = `i_trans`.`Ccid`))),0) AS `CloseQty`, " &
  "IFNULL((SELECT `sd`.`Cost` FROM `cld_inv_trans` `sd` WHERE ((`sd`.`type` = 12) AND (`sd`.`flagtrans` = 1) AND (`sd`.`CustID` = `i_trans`.`custid`) AND (`sd`.`Stk_Id` = `i_trans`.`stkid`) AND (`sd`.`InvDate` = `i_trans`.`invDate`) AND (`sd`.`Cc` = `i_trans`.`Ccid`))),0) AS `CloseCost`, " &
  "IFNULL((SELECT SUM(`sd`.`ttlStock`) AS `ds` FROM `cld_inv_trans` `sd` WHERE ((`sd`.`type` = 7) AND (`sd`.`flagtrans` = 1) AND (`sd`.`CustID` = `i_trans`.`custid`) AND (`sd`.`Stk_Id` = `i_trans`.`stkid`) AND (`sd`.`InvDate` = `i_trans`.`invDate`) AND (`sd`.`Cc` = `i_trans`.`Ccid`))),0) AS `HOout`, " &
  "IFNULL((SELECT SUM(`sd`.`ttlStock`) AS `ds` FROM `cld_inv_trans` `sd` WHERE ((`sd`.`type` = 8) AND (`sd`.`flagtrans` = 1) AND (`sd`.`CustID` = `i_trans`.`custid`) AND (`sd`.`Stk_Id` = `i_trans`.`stkid`) AND (`sd`.`InvDate` = `i_trans`.`invDate`) AND (`sd`.`Cc` = `i_trans`.`Ccid`))),0) AS `HOIn`, " &
  "IFNULL((SELECT SUM(`sd`.`ttlStock`) AS `ds` FROM `cld_inv_trans` `sd` WHERE ((`sd`.`type` = 9) AND (`sd`.`flagtrans` = 1) AND (`sd`.`CustID` = `i_trans`.`custid`) AND (`sd`.`Stk_Id` = `i_trans`.`stkid`) AND (`sd`.`InvDate` = `i_trans`.`invDate`) AND (`sd`.`Cc` = `i_trans`.`Ccid`))),0) AS `BPIn`, " &
  "IFNULL((SELECT SUM(`sd`.`ttlStock`) AS `ds` FROM `cld_inv_transusage` `sd` WHERE ((`sd`.`type` = 11) AND (`sd`.`flagtrans` = 1) AND (`sd`.`CustID` = `i_trans`.`custid`) AND (`sd`.`Stk_Id` = `i_trans`.`stkid`) AND (`sd`.`InvDate` = `i_trans`.`invDate`) AND (`sd`.`Cc` = `i_trans`.`Ccid`))),0) AS `BPOut`, " &
  "IFNULL((SELECT SUM(`sd`.`ttlStock`) AS `ds` FROM `cld_inv_trans` `sd` WHERE ((`sd`.`type` = 14) AND (`sd`.`flagtrans` = 1) AND (`sd`.`CustID` = `i_trans`.`custid`) AND (`sd`.`Stk_Id` = `i_trans`.`stkid`) AND (`sd`.`InvDate` = `i_trans`.`invDate`) AND (`sd`.`Cc` = `i_trans`.`Ccid`))),0) AS `cwasteqty` " &
  "FROM (`cld_inv_transstockcls` `i_trans` INNER JOIN `cld_inv_stk` " &
  "ON ((`cld_inv_stk`.`ID` = `i_trans`.`stkid`)) INNER JOIN `cld_inv_cc` `cc` " &
  "ON ((`cc`.`ID` = `i_trans`.`Ccid`))) " &
  "WHERE i_trans.`invDate` >= '" & dstart.Date.ToString("yyyy-MM-dd") & "' AND i_trans.`invDate` <= '" & dend.Date.ToString("yyyy-MM-dd") & "' AND I_trans.`custid` = " & CustId)


            errLog("Inv_ProcessClsQuery END " & Now, "clsDBO:executeNonQuery_Inv_ProcessCls")

            If ds.Tables(0).Rows.Count > 0 Then

                ' c_cmd.executeNonQuery("Delete from cld_inv_transstockcls WHERE InvDate >= '" & dstart.Date.ToString("yyyy-MM-dd") & "' AND InvDate <= '" & dend.Date.ToString("yyyy-MM-dd") & "' AND CustID = " & CustId)

                For Each row As DataRow In ds.Tables(0).Rows

                    '(OpenQty + rcvqty - rtnqty + tinQty - toutQty - rwasteQty - fwasteQty - usageqty - HOout + Hoin + BPin - BPout) AS ttlQty

                    dTotalQty = row.Item("OpenQty") + row.Item("rcvqty") - row.Item("rtnqty") + row.Item("tinQty") - row.Item("toutQty") - row.Item("rwasteQty") - row.Item("fwasteQty") - row.Item("usageqty") - row.Item("HOout") + row.Item("Hoin") + row.Item("BPin") - row.Item("BPout")


                    'CASE WHEN bclose = 0 THEN 0 ELSE   (OpenQty + rcvqty - rtnqty + tinQty - toutQty - rwasteQty - fwasteQty - usageqty - closeQty - HOout + Hoin + BPin - BPout)  END as varianceQty
                    If row.Item("bClose") <> 0 Then
                        dvariantQty = row.Item("OpenQty") + row.Item("rcvqty") - row.Item("rtnqty") + row.Item("tinQty") - row.Item("toutQty") - row.Item("rwasteQty") - row.Item("fwasteQty") - row.Item("usageqty") - row.Item("closeQty") - row.Item("HOout") + row.Item("Hoin") + row.Item("BPin") - row.Item("BPout")
                    Else
                        dvariantQty = 0
                    End If

                    dDate = row.Item("invdate")
                    Dim dcwaste As Decimal
                    dcwaste = c_cmd.ExecuteScalar("SELECT ttlqty FROM cld_inv_transstockcls WHERE stkid = " & row.Item("stk_id") & " AND ccid = " & row.Item("cc") & " AND invdate = '" & dDate.Date.ToString("yyyy-MM-dd") & "' AND custid = " & CustId & "", 0)
                    If row.Item("cwasteqty") <> 0 Then
                        dTotalQty = dcwaste
                        dvariantQty = dcwaste
                    End If

                    c_cmd.ReNew()
                    ' PERBAIKAN: Ganti DELETE+INSERT (gap lock) dengan INSERT ON DUPLICATE KEY UPDATE (atomic)
                    ' Statement tunggal ini tidak meninggalkan gap antara delete dan insert sehingga tidak ada lock wait
                    If c_cmd.executeNonQuery("INSERT INTO cld_inv_transstockcls" &
                        " (InvDate, CustID, Stk_ID, Stk_No, Stk_Name, Ccid, Cc_Name, OpenCost, OpenQty, Cost," &
                        "  RcvQty, RtnQty, TinQty, ToutQty, RwasteQty, FwasteQty, UsageQty, TtlQty, bClose," &
                        "  CloseQty, CloseCost, VarianceQty, HoOutQty, HoInQty, BPIn, BPout, CWasteQty)" &
                        " VALUES ('" & dDate.Date.ToString("yyyy-MM-dd") & "', " & CustId & "," & row.Item("stk_id") & ", '" & row.Item("stk_No") & "', '" & row.Item("Stk_name") & "'," &
                        " " & row.Item("cc") & ", '" & row.Item("cc_name") & "', " & row.Item("Opencost") & ", " & row.Item("OpenQty") & ", " & row.Item("Cost") & ", " & row.Item("rcvqty") & ", " & row.Item("rtnqty") & ", " & row.Item("tinQty") & "," &
                        " " & row.Item("toutQty") & ", " & row.Item("rwasteQty") & ", " & row.Item("fwasteQty") & ", " & row.Item("usageqty") & ", " & dTotalQty & ", " & row.Item("bclose") & ", " & row.Item("closeQty") & ", " & row.Item("closeCost") & ", " & dvariantQty & ", " & row.Item("HOout") & ", " & row.Item("Hoin") & ", " & row.Item("BPin") & ", " & row.Item("BPout") & ", " & row.Item("cwasteqty") & ")" &
                        " ON DUPLICATE KEY UPDATE" &
                        "  Stk_No       = VALUES(Stk_No)," &
                        "  Stk_Name     = VALUES(Stk_Name)," &
                        "  Cc_Name      = VALUES(Cc_Name)," &
                        "  OpenCost     = VALUES(OpenCost)," &
                        "  OpenQty      = VALUES(OpenQty)," &
                        "  Cost         = VALUES(Cost)," &
                        "  RcvQty       = VALUES(RcvQty)," &
                        "  RtnQty       = VALUES(RtnQty)," &
                        "  TinQty       = VALUES(TinQty)," &
                        "  ToutQty      = VALUES(ToutQty)," &
                        "  RwasteQty    = VALUES(RwasteQty)," &
                        "  FwasteQty    = VALUES(FwasteQty)," &
                        "  UsageQty     = VALUES(UsageQty)," &
                        "  TtlQty       = VALUES(TtlQty)," &
                        "  bClose       = VALUES(bClose)," &
                        "  CloseQty     = VALUES(CloseQty)," &
                        "  CloseCost    = VALUES(CloseCost)," &
                        "  VarianceQty  = VALUES(VarianceQty)," &
                        "  HoOutQty     = VALUES(HoOutQty)," &
                        "  HoInQty      = VALUES(HoInQty)," &
                        "  BPIn         = VALUES(BPIn)," &
                        "  BPout        = VALUES(BPout)," &
                        "  CWasteQty    = VALUES(CWasteQty)") = False Then
                        writeerrcsv(True, dDate.Date.ToString("yyyy-MM-dd 00:00:00"), row.Item("cc"), row.Item("stk_id"))
                    End If

                Next
            End If


        Catch ex As Exception

            errLog(Err.Description, "clsDBO:executeNonQuery_Inv_ProcessCls")
            Err.Clear()
        End Try


        c_cmd.dispose()
        c_cmd = Nothing



        dispose()
        myCmd.CommandType = Nothing

    End Sub
    Public Sub ReNew()

        myConn = New MySqlConnection(ServerString)
        myCmd.Connection = myConn

    End Sub
    Public Function SelectDataset_VInvtransstock_rec(ByVal custid As Integer, ByVal dt As Date) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""

        'dasda()

        errLog("VInvtransstock Start-" & dt.Date & " Time: " & Now, "clsDBO:SelectDataset_VInvtransstock")

        Dim a As New clsDbo("")

        'Process_InvDataStock(dt.Date, dt.Date)

        a.ReNew()



        'perubahan dari type = 5 menjadi type = 12 untuk stock opname

        ds = a.ReturnDataset("SELECT " &
"`i_trans`.`invDate`    AS `invDate`, `cld_inv_stk`.`CustID` AS `Custid`, " &
"`i_trans`.`stk_id`     AS `stk_id`, `cld_inv_stk`.`Number` AS `stk_No`, " &
"`cld_inv_stk`.`Name`   AS `Stk_name`, `i_trans`.`Ccid` AS `cc`, `cc`.`Name` AS `cc_name`, " &
"cld_inv_stk.factor1, cld_inv_stk.Unit1, cld_inv_stk.Unit2, " &
"IFNULL((SELECT `sd`.`ttlStock` FROM `cld_inv_trans` `sd` WHERE ((`sd`.`type` = 6) AND (`sd`.`flagtrans` = 1) AND (`sd`.`CustID` = `i_trans`.`custid`) AND (`sd`.`Stk_Id` = `i_trans`.`stk_id`) AND (`sd`.`InvDate` = `i_trans`.`invDate`) AND (`sd`.`Cc` = `i_trans`.`Ccid`))),0) AS `OpenQty`, " &
"IFNULL((SELECT `sd`.`Cost` FROM `cld_inv_stkcost` `sd` WHERE ((`sd`.`Custid` = `i_trans`.`custid`) AND (`sd`.`stkID` = `i_trans`.`stk_id`) AND (`sd`.`cc` = `i_trans`.`Ccid`))),0) AS `Cost`, " &
"IFNULL((SELECT SUM(`sd`.`ttlStock`) AS `ds` FROM `cld_inv_trans` `sd` WHERE ((`sd`.`type` = 3) AND (`sd`.`flagtrans` = 1) AND (`sd`.`CustID` = `i_trans`.`custid`) AND (`sd`.`Stk_Id` = `i_trans`.`stk_id`) AND (`sd`.`InvDate` = `i_trans`.`invDate`) AND (`sd`.`Cc` = `i_trans`.`Ccid`))),0) AS `rcvQty`, " &
"IFNULL((SELECT SUM(`sd`.`ttlStock`) AS `ds` FROM `cld_inv_trans` `sd` WHERE ((`sd`.`type` = 4) AND (`sd`.`flagtrans` = 1) AND (`sd`.`CustID` = `i_trans`.`custid`) AND (`sd`.`Stk_Id` = `i_trans`.`stk_id`) AND (`sd`.`InvDate` = `i_trans`.`invDate`) AND (`sd`.`Cc` = `i_trans`.`Ccid`))),0) AS `rtnqty`, " &
"IFNULL((SELECT SUM(`sd`.`ttlStock`) AS `ds` FROM `cld_inv_trans` `sd` WHERE ((`sd`.`type` = 1) AND (`sd`.`flagtrans` = 1) AND (`sd`.`CustID` = `i_trans`.`custid`) AND (`sd`.`Stk_Id` = `i_trans`.`stk_id`) AND (`sd`.`InvDate` = `i_trans`.`invDate`) AND (`sd`.`Cc` = `i_trans`.`Ccid`))),0) AS `tinQty`, " &
"IFNULL((SELECT SUM(`sd`.`ttlStock`) AS `ds` FROM (`cld_inv_trans` `sd` JOIN `cld_invlk_trf` ON(((`sd`.`Linkid` = `cld_invlk_trf`.`id`) AND (`sd`.`CustID` = `cld_invlk_trf`.`CustID`)))) WHERE ((`sd`.`type` = 1) AND (`sd`.`flagtrans` = 1) AND (`cld_invlk_trf`.`CustID` = `i_trans`.`custid`) AND (`sd`.`Stk_Id` = `i_trans`.`stk_id`) AND (`sd`.`InvDate` = `i_trans`.`invDate`) AND (`cld_invlk_trf`.`ccFrom` = `i_trans`.`Ccid`))),0) AS `toutQty`, " &
"IFNULL((SELECT SUM(`sd`.`ttlStock`) AS `ds` FROM `cld_inv_trans` `sd` WHERE ((`sd`.`type` = 2) AND (`sd`.`flagtrans` = 1) AND (`sd`.`CustID` = `i_trans`.`custid`) AND (`sd`.`Stk_Id` = `i_trans`.`stk_id`) AND (`sd`.`InvDate` = `i_trans`.`invDate`) AND (`sd`.`Cc` = `i_trans`.`Ccid`))),0) AS `rwasteQty`, " &
"0 AS `fwasteQty`, " &
"IFNULL((SELECT SUM(`sd`.`ttlStock`) AS `ds` FROM `cld_inv_transusage` `sd` WHERE ((`sd`.`type` = 10) AND (`sd`.`flagtrans` = 1) AND (`sd`.`CustID` = `i_trans`.`custid`) AND (`sd`.`Stk_Id` = `i_trans`.`stk_id`) AND (`sd`.`InvDate` = `i_trans`.`invDate`) AND (`sd`.`Cc` = `i_trans`.`Ccid`))),0) AS `usageqty`, " &
"IFNULL((SELECT `sd`.`flagtrans` FROM `cld_inv_trans` `sd` WHERE ((`sd`.`type` = 12) AND (`sd`.`flagtrans` = 1) AND (`sd`.`CustID` = `i_trans`.`custid`) AND (`sd`.`Stk_Id` = `i_trans`.`stk_id`) AND (`sd`.`InvDate` = `i_trans`.`invDate`) AND (`sd`.`Cc` = `i_trans`.`Ccid`))),0) AS `bClose`, " &
"IFNULL((SELECT `sd`.`ttlStock` FROM `cld_inv_trans` `sd` WHERE ((`sd`.`type` = 12) AND (`sd`.`flagtrans` = 1) AND (`sd`.`CustID` = `i_trans`.`custid`) AND (`sd`.`Stk_Id` = `i_trans`.`stk_id`) AND (`sd`.`InvDate` = `i_trans`.`invDate`) AND (`sd`.`Cc` = `i_trans`.`Ccid`))),0) AS `CloseQty`, " &
"IFNULL((SELECT `sd`.`Cost` FROM `cld_inv_trans` `sd` WHERE ((`sd`.`type` = 12) AND (`sd`.`flagtrans` = 1) AND (`sd`.`CustID` = `i_trans`.`custid`) AND (`sd`.`Stk_Id` = `i_trans`.`stk_id`) AND (`sd`.`InvDate` = `i_trans`.`invDate`) AND (`sd`.`Cc` = `i_trans`.`Ccid`))),0) AS `CloseCost`, " &
"IFNULL((SELECT SUM(`sd`.`ttlStock`) AS `ds` FROM `cld_inv_trans` `sd` WHERE ((`sd`.`type` = 7) AND (`sd`.`flagtrans` = 1) AND (`sd`.`CustID` = `i_trans`.`custid`) AND (`sd`.`Stk_Id` = `i_trans`.`stk_id`) AND (`sd`.`InvDate` = `i_trans`.`invDate`) AND (`sd`.`Cc` = `i_trans`.`Ccid`))),0) AS `HOout`, " &
"IFNULL((SELECT SUM(`sd`.`ttlStock`) AS `ds` FROM `cld_inv_trans` `sd` WHERE ((`sd`.`type` = 8) AND (`sd`.`flagtrans` = 1) AND (`sd`.`CustID` = `i_trans`.`custid`) AND (`sd`.`Stk_Id` = `i_trans`.`stk_id`) AND (`sd`.`InvDate` = `i_trans`.`invDate`) AND (`sd`.`Cc` = `i_trans`.`Ccid`))),0) AS `HOIn`, " &
"IFNULL((SELECT SUM(`sd`.`ttlStock`) AS `ds` FROM `cld_inv_trans` `sd` WHERE ((`sd`.`type` = 9) AND (`sd`.`flagtrans` = 1) AND (`sd`.`CustID` = `i_trans`.`custid`) AND (`sd`.`Stk_Id` = `i_trans`.`stk_id`) AND (`sd`.`InvDate` = `i_trans`.`invDate`) AND (`sd`.`Cc` = `i_trans`.`Ccid`))),0) AS `BPIn`, " &
"IFNULL((SELECT SUM(`sd`.`ttlStock`) AS `ds` FROM `cld_inv_transusage` `sd` WHERE ((`sd`.`type` = 11) AND (`sd`.`flagtrans` = 1) AND (`sd`.`CustID` = `i_trans`.`custid`) AND (`sd`.`Stk_Id` = `i_trans`.`stk_id`) AND (`sd`.`InvDate` = `i_trans`.`invDate`) AND (`sd`.`Cc` = `i_trans`.`Ccid`))),0) AS `BPOut` " &
"IFNULL((SELECT SUM(`sd`.`ttlStock`) AS `ds` FROM `cld_inv_trans` `sd` WHERE ((`sd`.`type` = 14) AND (`sd`.`flagtrans` = 1) AND (`sd`.`CustID` = `i_trans`.`custid`) AND (`sd`.`Stk_Id` = `i_trans`.`stkid`) AND (`sd`.`InvDate` = `i_trans`.`invDate`) AND (`sd`.`Cc` = `i_trans`.`Ccid`))),0) AS `cwasteqty` " &
"FROM (`cld_inv_transstockcls` `i_trans` INNER JOIN `cld_inv_stk` " &
"ON ((`cld_inv_stk`.`ID` = `i_trans`.`stk_id`))) INNER JOIN `cld_inv_cc` `cc` " &
"ON ((`cc`.`ID` = `i_trans`.`Ccid`)) " &
"WHERE i_trans.`invDate` = '" & dt.Date.ToString("yyyy-MM-dd") & "' AND I_trans.`custid` = " & custid)


        a.dispose()
        a = Nothing


        errLog("VInvtransstock END" & Now, "clsDBO:SelectDataset_VInvtransstock")

        Return ds

    End Function
    Public Sub insert_invtrans_opening_rec(ByVal custid As Integer, ByVal maxdate As Date, ByVal cc As Integer, ByVal stkid As Integer,
                              ByVal stkname As String, ByVal u2 As Decimal, ByVal factor1 As Decimal, ByVal stkno As String,
                              ByVal u1_nm As String, ByVal u2_nm As String, ByVal cost As Decimal, ByVal ccname As String, ByVal username As String)
        Dim c_cmd As New clsDbo("")


        Dim str As String
        str = "insert into cld_inv_Trans values (" & c_cmd.ExecuteScalar("select Max(ID) from cld_inv_Trans") & ", " & custid & ", DATE_ADD('" & maxdate.ToString("yyyy-MM-dd") & "', INTERVAL 1 DAY), 6, " & cc & ", " &
                        " " & stkid & ", '" & stkname & "', 0, " & u2 & ", " & factor1 & ", " & u2 & ", 0," & cost & ", " &
                        " '', 1, '" & u1_nm & "', '" & u2_nm & "', '" & username & "', DATE_ADD('" & maxdate.ToString("yyyy-MM-dd HH:mm:ss") & "', INTERVAL 1 DAY), Null, Null, '" & stkno & "', '" & ccname & "')"
        c_cmd.executeNonQuery(str)
        c_cmd = Nothing

    End Sub
    Public Sub executeNonQuery_Inv_CalcStock_rec2(ByVal istk As Integer, ByVal icc As Integer, ByVal CustId As Integer)

        'Process_InvDataStock(Date.Now, Date.Now)


        errLog("Inv_ProcessCalcStockNOW START" & Now, "clsDBO:executeNonQuery_Inv_CalcStock")


        Dim c_cmd As New clsDbo("")
        Dim ds As DataSet

        Dim dTotalQty As Decimal = 0
        Dim dvariantQty As Decimal = 0
        Dim dcost As Decimal = 0



        Try

            c_cmd.executeNonQuery("Truncate table cld_inv_transstock")
            c_cmd.ReNew()
            'perubahan dari type = 5 menjadi type = 12 untuk stockopname

            ds = c_cmd.ReturnDataset("SELECT " &
  "`i_trans`.`invDate`    AS `invDate`, `cld_inv_stk`.`CustID` AS `Custid`, " &
  "`i_trans`.`stk_id`     AS `stk_id`, `cld_inv_stk`.`Number` AS `stk_No`, " &
  "`cld_inv_stk`.`Name`   AS `Stk_name`, `cld_inv_stk`.`Unit2`  AS `u2_nm`, " &
  "`i_trans`.`Ccid`         AS `cc`, `cc`.`Name` AS `cc_name`, " &
  "IFNULL((SELECT `sd`.`Cost` FROM `cld_inv_trans` `sd` WHERE ((`sd`.`type` = 6) AND (`sd`.`flagtrans` = 1) AND (`sd`.`Stk_Id` = `i_trans`.`stk_id`) AND (`sd`.`InvDate` = `i_trans`.`invDate`) AND (`sd`.`Cc` = `i_trans`.`Ccid`))),0) AS `OpenCost`, " &
  "IFNULL((SELECT `sd`.`ttlStock` FROM `cld_inv_trans` `sd` WHERE ((`sd`.`type` = 6) AND (`sd`.`flagtrans` = 1) AND (`sd`.`CustID` = `i_trans`.`custid`) AND (`sd`.`Stk_Id` = `i_trans`.`stk_id`) AND (`sd`.`InvDate` = `i_trans`.`invDate`) AND (`sd`.`Cc` = `i_trans`.`Ccid`))),0) AS `OpenQty`, " &
  "IFNULL((SELECT `sd`.`Cost` FROM `cld_inv_stkcost` `sd` WHERE ((`sd`.`Custid` = `i_trans`.`custid`) AND (`sd`.`stkID` = `i_trans`.`stk_id`) AND (`sd`.`cc` = `i_trans`.`Ccid`))),0) AS `Cost`, " &
  "IFNULL((SELECT SUM(`sd`.`ttlStock`) AS `ds` FROM `cld_inv_trans` `sd` WHERE ((`sd`.`type` = 3) AND (`sd`.`flagtrans` = 1) AND (`sd`.`CustID` = `i_trans`.`custid`) AND (`sd`.`Stk_Id` = `i_trans`.`stk_id`) AND (`sd`.`InvDate` = `i_trans`.`invDate`) AND (`sd`.`Cc` = `i_trans`.`Ccid`))),0) AS `rcvQty`, " &
  "IFNULL((SELECT SUM(`sd`.`ttlStock`) AS `ds` FROM `cld_inv_trans` `sd` WHERE ((`sd`.`type` = 4) AND (`sd`.`flagtrans` = 1) AND (`sd`.`CustID` = `i_trans`.`custid`) AND (`sd`.`Stk_Id` = `i_trans`.`stk_id`) AND (`sd`.`InvDate` = `i_trans`.`invDate`) AND (`sd`.`Cc` = `i_trans`.`Ccid`))),0) AS `rtnqty`, " &
  "IFNULL((SELECT SUM(`sd`.`ttlStock`) AS `ds` FROM `cld_inv_trans` `sd` WHERE ((`sd`.`type` = 1) AND (`sd`.`flagtrans` = 1) AND (`sd`.`CustID` = `i_trans`.`custid`) AND (`sd`.`Stk_Id` = `i_trans`.`stk_id`) AND (`sd`.`InvDate` = `i_trans`.`invDate`) AND (`sd`.`Cc` = `i_trans`.`Ccid`))),0) AS `tinQty`, " &
  "IFNULL((SELECT SUM(`sd`.`ttlStock`) AS `ds` FROM (`cld_inv_trans` `sd` JOIN `cld_invlk_trf` ON(((`sd`.`Linkid` = `cld_invlk_trf`.`id`) AND (`sd`.`CustID` = `cld_invlk_trf`.`CustID`)))) WHERE ((`sd`.`type` = 1) AND (`sd`.`flagtrans` = 1) AND (`cld_invlk_trf`.`CustID` = `i_trans`.`custid`) AND (`sd`.`Stk_Id` = `i_trans`.`stk_id`) AND (`sd`.`InvDate` = `i_trans`.`invDate`) AND (`cld_invlk_trf`.`ccFrom` = `i_trans`.`Ccid`))),0) AS `toutQty`, " &
  "IFNULL((SELECT SUM(`sd`.`ttlStock`) AS `ds` FROM `cld_inv_trans` `sd` WHERE ((`sd`.`type` = 2) AND (`sd`.`flagtrans` = 1) AND (`sd`.`CustID` = `i_trans`.`custid`) AND (`sd`.`Stk_Id` = `i_trans`.`stk_id`) AND (`sd`.`InvDate` = `i_trans`.`invDate`) AND (`sd`.`Cc` = `i_trans`.`Ccid`))),0) AS `rwasteQty`, " &
  "0 AS `fwasteQty`, " &
  "IFNULL((SELECT SUM(`sd`.`ttlStock`) AS `ds` FROM `cld_inv_transusage` `sd` WHERE ((`sd`.`type` = 10) AND (`sd`.`flagtrans` = 1) AND (`sd`.`CustID` = `i_trans`.`custid`) AND (`sd`.`Stk_Id` = `i_trans`.`stk_id`) AND (`sd`.`InvDate` = `i_trans`.`invDate`) AND (`sd`.`Cc` = `i_trans`.`Ccid`))),0) AS `usageqty`, " &
  "IFNULL((SELECT `sd`.`flagtrans` FROM `cld_inv_trans` `sd` WHERE ((`sd`.`type` = 12) AND (`sd`.`flagtrans` = 1) AND (`sd`.`CustID` = `i_trans`.`custid`) AND (`sd`.`Stk_Id` = `i_trans`.`stk_id`) AND (`sd`.`InvDate` = `i_trans`.`invDate`) AND (`sd`.`Cc` = `i_trans`.`Ccid`))),0) AS `bClose`, " &
  "IFNULL((SELECT `sd`.`ttlStock` FROM `cld_inv_trans` `sd` WHERE ((`sd`.`type` = 12) AND (`sd`.`flagtrans` = 1) AND (`sd`.`CustID` = `i_trans`.`custid`) AND (`sd`.`Stk_Id` = `i_trans`.`stk_id`) AND (`sd`.`InvDate` = `i_trans`.`invDate`) AND (`sd`.`Cc` = `i_trans`.`Ccid`))),0) AS `CloseQty`, " &
  "IFNULL((SELECT `sd`.`Cost` FROM `cld_inv_trans` `sd` WHERE ((`sd`.`type` = 12) AND (`sd`.`flagtrans` = 1) AND (`sd`.`CustID` = `i_trans`.`custid`) AND (`sd`.`Stk_Id` = `i_trans`.`stk_id`) AND (`sd`.`InvDate` = `i_trans`.`invDate`) AND (`sd`.`Cc` = `i_trans`.`Ccid`))),0) AS `CloseCost`, " &
  "IFNULL((SELECT SUM(`sd`.`ttlStock`) AS `ds` FROM `cld_inv_trans` `sd` WHERE ((`sd`.`type` = 7) AND (`sd`.`flagtrans` = 1) AND (`sd`.`CustID` = `i_trans`.`custid`) AND (`sd`.`Stk_Id` = `i_trans`.`stk_id`) AND (`sd`.`InvDate` = `i_trans`.`invDate`) AND (`sd`.`Cc` = `i_trans`.`Ccid`))),0) AS `HOout`, " &
  "IFNULL((SELECT SUM(`sd`.`ttlStock`) AS `ds` FROM `cld_inv_trans` `sd` WHERE ((`sd`.`type` = 8) AND (`sd`.`flagtrans` = 1) AND (`sd`.`CustID` = `i_trans`.`custid`) AND (`sd`.`Stk_Id` = `i_trans`.`stk_id`) AND (`sd`.`InvDate` = `i_trans`.`invDate`) AND (`sd`.`Cc` = `i_trans`.`Ccid`))),0) AS `HOIn`, " &
  "IFNULL((SELECT SUM(`sd`.`ttlStock`) AS `ds` FROM `cld_inv_trans` `sd` WHERE ((`sd`.`type` = 9) AND (`sd`.`flagtrans` = 1) AND (`sd`.`CustID` = `i_trans`.`custid`) AND (`sd`.`Stk_Id` = `i_trans`.`stk_id`) AND (`sd`.`InvDate` = `i_trans`.`invDate`) AND (`sd`.`Cc` = `i_trans`.`Ccid`))),0) AS `BPIn`, " &
  "IFNULL((SELECT SUM(`sd`.`ttlStock`) AS `ds` FROM `cld_inv_transusage` `sd` WHERE ((`sd`.`type` = 11) AND (`sd`.`flagtrans` = 1) AND (`sd`.`CustID` = `i_trans`.`custid`) AND (`sd`.`Stk_Id` = `i_trans`.`stk_id`) AND (`sd`.`InvDate` = `i_trans`.`invDate`) AND (`sd`.`Cc` = `i_trans`.`Ccid`))),0) AS `BPOut` " &
  "FROM (`cld_inv_transstockcls` `i_trans` INNER JOIN `cld_inv_stk` " &
  "ON ((`cld_inv_stk`.`ID` = `i_trans`.`stk_id`)) INNER JOIN `cld_inv_cc` `cc` " &
  "ON ((`cc`.`ID` = `i_trans`.`Ccid`))) " &
  "WHERE i_trans.`invDate` >= '" & Now.Date.ToString("yyyy-MM-dd") & "' AND i_trans.`invDate` <= '" & Now.Date.ToString("yyyy-MM-dd") & "' AND I_trans.`custid` = " & CustId)


            If ds.Tables(0).Rows.Count > 0 Then
                For Each row As DataRow In ds.Tables(0).Rows
                    c_cmd.ReNew()

                    dTotalQty = row.Item("OpenQty") + row.Item("rcvqty") - row.Item("rtnqty") + row.Item("tinQty") - row.Item("toutQty") - row.Item("rwasteQty") - row.Item("fwasteQty") - row.Item("usageqty") + row.Item("BPIn") - row.Item("BPOut")


                    If row.Item("bClose") <> 0 Then
                        dvariantQty = row.Item("OpenQty") + row.Item("rcvqty") - row.Item("rtnqty") + row.Item("tinQty") - row.Item("toutQty") - row.Item("rwasteQty") - row.Item("fwasteQty") - row.Item("usageqty") - row.Item("closeQty") + row.Item("BPIn") - row.Item("BPOut")
                    Else
                        dvariantQty = 0
                    End If


                    dcost = row.Item("Cost")
                    If dcost < 0 Then
                        dcost = dcost * -1
                    End If


                    c_cmd.executeNonQuery("insert into cld_inv_transstock values ( " & CustId & "," & row.Item("stk_id") & ", '" & row.Item("stk_No") & "', '" & row.Item("Stk_name") & "'," &
                        " " & row.Item("cc") & ", '" & row.Item("cc_name") & "', " & row.Item("Opencost") & ", " & row.Item("OpenQty") & ", " & dcost & ", " & row.Item("rcvqty") & ", " & row.Item("rtnqty") & ", " & row.Item("tinQty") & ", " &
                        " " & row.Item("toutQty") & ", " & row.Item("rwasteQty") & ", " & row.Item("fwasteQty") & ", " & row.Item("usageqty") & ", " & dTotalQty & ", " & row.Item("bclose") & ", " & row.Item("closeQty") & ", " & row.Item("closeCost") & ", " & dvariantQty & ", " & row.Item("BPIn") & ", " & row.Item("BPOut") & ")")


                Next
            End If


        Catch ex As Exception

            errLog(Err.Description, "clsDBO:executeNonQuery_Inv_ProcessCls")
            Err.Clear()
        End Try



        errLog("Inv_ProcessCalcStockNOW END" & Now, "clsDBO:executeNonQuery_Inv_CalcStock")


        dispose()
        myCmd.CommandType = Nothing

    End Sub
    Sub Cek_receipeCost_rec2(ByVal iCust As Integer)

        Dim dReceipe As New DataSet
        dReceipe = getDataset_Receipe(iCust)

        Dim i As Integer = 0

        Try

            For Each data_row As DataRow In dReceipe.Tables(0).Rows

                Dim Total As Decimal = 0

                Dim cCmd As New clsDbo("")
                Dim dData As New DataSet
                dData = cCmd.ReturnDataset("SELECT * from cld_inv_rcpdtl where rcpid = " & data_row.Item("ID") & " and Custid = " & iCust)

                For Each dData_row As DataRow In dData.Tables(0).Rows
                    If dData_row.Item("tipe") = 1 Then 'inventory
                        Total = Total + ReceiptCost_ByInventory(dData_row.Item("kodeid").ToString, dData_row.Item("qty"), dData_row.Item("lossf"), iCust)
                    Else
                        If dData_row.Item("tipe") = 2 Then 'inventory
                            Total = Total + (dData_row.Item("qty") * Cek_Receipe(dData_row.Item("kodeid").ToString, iCust))
                        End If
                    End If
                Next

                Update_ReceipeCost(data_row.Item("ID").ToString, Total, iCust)

                cCmd = Nothing

                i = i + 1
            Next data_row

        Catch ex As Exception
            errLog(Err.Description, "clsInventory_stkCost:Cek_receipeCost")
            Err.Clear()
        End Try

        dReceipe.Dispose()

    End Sub
    Public Sub cek_MenuItemLinkCost_rec(ByVal iCust As Integer)
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
    Public Sub Process_InvDataStock2(ByVal dInvStart As Date, ByVal dInvEnd As Date)
        Delete_InvDataStock(dInvStart, dInvEnd)
        Try

            myCmd.CommandText = "INSERT INTO cld_inv_transstockcls SELECT Stk_ID, Cc, InvDate, custid FROM `v_inv_stktrans` " &
                                "WHERE InvDate >= @dStart and InvDate <= @dEnd " &
                                "GROUP BY Stk_ID, Cc"


            With myCmd.Parameters
                .Clear()
                .AddWithValue("@dStart", dInvStart.ToString("yyyy-MM-dd"))
                .AddWithValue("@dEnd", dInvEnd.ToString("yyyy-MM-dd"))
            End With

            myCmd.Connection.Open()
            myCmd.ExecuteNonQuery()

        Catch ex As Exception
            errLog(Err.Description, "clsDBO.Process_BeforeInv_CalcStock")
            Err.Clear()
        End Try

        dispose()
        myCmd.CommandType = Nothing

    End Sub
    Public Sub Delete_InvDataStock(ByVal dInvStart As Date, ByVal dInvEnd As Date)

        Try

            myCmd.CommandText = "Delete From cld_inv_transstockcls " &
                                "WHERE InvDate >= @dStart and InvDate <= @dEnd "


            With myCmd.Parameters
                .Clear()
                .AddWithValue("@dStart", dInvStart.ToString("yyyy-MM-dd"))
                .AddWithValue("@dEnd", dInvEnd.ToString("yyyy-MM-dd"))
            End With

            myCmd.Connection.Open()
            myCmd.ExecuteNonQuery()

        Catch ex As Exception
            errLog(Err.Description, "clsDBO.Process_BeforeInv_CalcStock")
            Err.Clear()
        End Try

        dispose()
        myCmd.CommandType = Nothing

    End Sub
    Public Sub dispose()

        Try

            If myCmd IsNot Nothing AndAlso myCmd.Connection IsNot Nothing Then
                If myCmd.Connection.State = ConnectionState.Open Then
                    myCmd.Connection.Close()
                End If
                myCmd.Dispose()
            ElseIf myCmd IsNot Nothing Then
                myCmd.Dispose()
            End If

        Catch ex As Exception
            errLog(Err.Description, "clsDbo.dispose")
            Err.Clear()
        End Try


    End Sub

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
    Private Function ReceiptCost_ByInventory(ByVal Item As String, ByVal qty As Integer, ByVal lossfactor As Integer, ByVal iCust As Integer) As Decimal

        Dim TotalCost As Decimal = 0
        Dim pembagianTotalcost As Decimal = 0
        Dim cCmd As New clsDbo("")

        Try

            Dim dData As New DataSet
            dData = cCmd.ReturnDataset("SELECT IFNULL(AVG(tstk.Cost),0) AS Cost , stk.Factor1, stk.Factor2 from cld_inv_stk AS stk INNER JOIN cld_inv_stkcost AS tstk ON (stk.CustID = tstk.custId) AND (tstk.stkId = stk.ID) " &
                                       "where tstk.custid  = " & iCust & " and tstk.stkid  = " & Item)

            If dData.Tables(0).Rows.Count > 0 Then

                If IsDBNull(dData.Tables(0).Rows(0).Item("Factor2")) = False Then
                    TotalCost = (qty + (qty * lossfactor / 100))


                    pembagianTotalcost = CDec(dData.Tables(0).Rows(0).Item("Factor2")) * CDec(dData.Tables(0).Rows(0).Item("Cost"))

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
    Public Sub InvCaclculate_ActiveTransOnly(ByVal dstart As Date, ByVal dend As Date, ByVal Custid As Integer, Optional ByVal ccidFilter As Integer = 0, Optional ByVal stkidFilter As Integer = 0)
        Dim dsDistinct As DataSet
        Try
            ' 1. Ambil data stok yang benar-benar memiliki transaksi saja di rentang tanggal tersebut
            Dim strQry As String = "SELECT DISTINCT DATE(InvDate) AS InvDate, cc, Stk_Id FROM cld_inv_trans WHERE InvDate >= '" & dstart.ToString("yyyy-MM-dd 00:00:00") & "' AND InvDate <= '" & dend.ToString("yyyy-MM-dd 23:59:59") & "' AND CustID = " & Custid
            If ccidFilter > 0 Then strQry &= " AND cc = " & ccidFilter
            If stkidFilter > 0 Then strQry &= " AND Stk_Id = " & stkidFilter
            strQry &= " ORDER BY InvDate ASC"

            Dim cmdDist As New clsDbo("")
            dsDistinct = cmdDist.ReturnDataset(strQry)

            If dsDistinct.Tables(0).Rows.Count > 0 Then
                For Each row As DataRow In dsDistinct.Tables(0).Rows
                    Dim dDate As Date = Convert.ToDateTime(row("InvDate"))
                    Dim ccid As Integer = Convert.ToInt32(row("cc"))
                    Dim stkid As Integer = Convert.ToInt32(row("Stk_Id"))

                    ' 2. Hitung ulang komponen qty HANYA untuk item yang bergerak di hari tersebut (menggunakan SQL Atomic UPDATE)
                    Dim sqlRecal As String = "UPDATE cld_inv_transstockcls SET " &
                        "rcvqty = IFNULL((SELECT SUM(ttlStock) FROM cld_inv_trans WHERE type = 3 AND flagtrans = 1 AND CustID = " & Custid & " AND Stk_Id = " & stkid & " AND InvDate = '" & dDate.ToString("yyyy-MM-dd") & "' AND Cc = " & ccid & "), 0), " &
                        "rtnqty = IFNULL((SELECT SUM(ttlStock) FROM cld_inv_trans WHERE type = 4 AND flagtrans = 1 AND CustID = " & Custid & " AND Stk_Id = " & stkid & " AND InvDate = '" & dDate.ToString("yyyy-MM-dd") & "' AND Cc = " & ccid & "), 0), " &
                        "tinQty = IFNULL((SELECT SUM(ttlStock) FROM cld_inv_trans WHERE type = 1 AND flagtrans = 1 AND CustID = " & Custid & " AND Stk_Id = " & stkid & " AND InvDate = '" & dDate.ToString("yyyy-MM-dd") & "' AND Cc = " & ccid & "), 0), " &
                        "toutQty = IFNULL((SELECT SUM(sd.ttlStock) FROM cld_inv_trans sd JOIN cld_invlk_trf ON (sd.Linkid = cld_invlk_trf.id AND sd.CustID = cld_invlk_trf.CustID) WHERE sd.type = 1 AND sd.flagtrans = 1 AND sd.CustID = " & Custid & " AND sd.Stk_Id = " & stkid & " AND sd.InvDate = '" & dDate.ToString("yyyy-MM-dd") & "' AND cld_invlk_trf.ccFrom = " & ccid & "), 0), " &
                        "rwasteQty = IFNULL((SELECT SUM(ttlStock) FROM cld_inv_trans WHERE type = 2 AND flagtrans = 1 AND CustID = " & Custid & " AND Stk_Id = " & stkid & " AND InvDate = '" & dDate.ToString("yyyy-MM-dd") & "' AND Cc = " & ccid & "), 0) " &
                        "WHERE CustID = " & Custid & " AND stkid = " & stkid & " AND ccid = " & ccid & " AND invdate = '" & dDate.ToString("yyyy-MM-dd 00:00:00") & "'"
                    
                    Dim cmdUpd1 As New clsDbo("")
                    cmdUpd1.executeNonQuery(sqlRecal)

                    ' 3. Setelah komponen quantity benar, kalkulasi ulang TtlQty nya
                    Dim sqlTtl As String = "UPDATE cld_inv_transstockcls SET ttlqty = openqty + rcvqty - rtnqty + tinQty - toutQty - rwasteQty - fwasteQty - usageqty - HoOutQty + HoInQty + bpin - bpout - cwasteqty " &
                        "WHERE CustID = " & Custid & " AND stkid = " & stkid & " AND ccid = " & ccid & " AND invdate = '" & dDate.ToString("yyyy-MM-dd 00:00:00") & "'"
                    
                    Dim cmdUpd2 As New clsDbo("")
                    cmdUpd2.executeNonQuery(sqlTtl)

                    ' 4. Dorong saldo akhir hari ini (ttlqty) menjadi saldo awal (openqty) hari besoknya
                    Dim clsinvpro As New clsInventoryProcess()
                    clsinvpro.Inv_Proc_trans_stckClsbyid(dDate, dDate, Custid, ccid, stkid)
                Next
            End If
        Catch ex As Exception
            errLog(Err.Description, "cls_recalculate:InvCaclculate_ActiveTransOnly")
            Err.Clear()
        End Try
    End Sub
    Public Sub InvCaclculate_Cost_ActiveTransOnly(ByVal dstart As Date, ByVal dend As Date, ByVal Custid As Integer, Optional ByVal ccidFilter As Integer = 0, Optional ByVal stkidFilter As Integer = 0)
        Dim c_cmd As New clsDbo("")
        Try
            ' 1. Sesuai pedoman: kalkulasi dimulai dari H-1 (dstart.AddDays(-1)) untuk memastikan modal opencost hari pertama valid dan bersih
            Dim dFrom As Date = dstart.Date.AddDays(-1)
            Dim dTo As Date = dend.Date
            If dTo < dFrom Then dTo = dFrom

            Dim filterCC As String = ""
            Dim filterStk As String = ""
            Dim filterCCTrans As String = ""
            Dim filterStkTrans As String = ""

            If ccidFilter > 0 Then
                filterCC = " AND today.ccid = " & ccidFilter
                filterCCTrans = " AND cc = " & ccidFilter
            End If
            If stkidFilter > 0 Then
                filterStk = " AND today.stkid = " & stkidFilter
                filterStkTrans = " AND stk_id = " & stkidFilter
            End If

            ' 2. Pastikan opencost pada hari awal (dFrom / H-1) tidak nol/kosong, jika masih nol fallback ke harga master stock
            Dim strInitOpen As String = "UPDATE cld_inv_transstockcls today " &
            "INNER JOIN cld_inv_stk stk ON today.stkid = stk.id AND today.custid = stk.custid " &
            "SET today.opencost = CASE WHEN stk.price2 > 0 THEN stk.price2 ELSE stk.price1 END " &
            "WHERE today.invdate = '" & dFrom.ToString("yyyy-MM-dd 00:00:00") & "' " &
            "  AND today.custid = " & Custid & " AND (today.opencost IS NULL OR today.opencost <= 0)" &
            filterCC & filterStk
            c_cmd.executeNonQuery(strInitOpen)

            ' 3. Looping berurutan HARI DEMI HARI (Day-by-Day Cascade) dari dFrom (H-1) s/d dTo
            Dim currDate As Date = dFrom.Date
            Do While currDate <= dTo
                Dim prevDate As Date = currDate.AddDays(-1)
                Dim sCurrDate As String = currDate.ToString("yyyy-MM-dd 00:00:00")
                Dim sPrevDate As String = prevDate.ToString("yyyy-MM-dd 00:00:00")

                ' STEP A: Alirkan closing cost kemarin (prevDate) menjadi opencost hari ini (currDate)
                Dim strCascadeOpen As String = "UPDATE cld_inv_transstockcls today " &
                "INNER JOIN cld_inv_transstockcls yest " &
                "   ON today.stkid = yest.stkid AND today.ccid = yest.ccid AND today.custid = yest.custid " &
                "  AND yest.invdate = '" & sPrevDate & "' " &
                "SET today.opencost = CASE " &
                "        WHEN (yest.bclose = 1 OR yest.bclose = b'1') AND yest.closecost > 0 THEN yest.closecost " &
                "        WHEN yest.cost > 0 THEN yest.cost " &
                "        ELSE today.opencost " &
                "    END, " &
                "    today.openqty = CASE " &
                "        WHEN (yest.bclose = 1 OR yest.bclose = b'1') THEN yest.closeqty " &
                "        ELSE today.openqty " &
                "    END " &
                "WHERE today.invdate = '" & sCurrDate & "' " &
                "  AND today.custid = " & Custid &
                "  AND (yest.cost > 0 OR yest.closecost > 0)" & filterCC & filterStk
                c_cmd.executeNonQuery(strCascadeOpen)

                ' STEP B0: Pembersihan (Sanitasi) Otomatis Anomali Ekstrem di cld_inv_trans untuk hari ini
                ' Prioritas:
                ' 1. today.opencost (Moving Average Cost aktual dari closing kemarin di CC ini)
                ' 2. s.price2 / s.price1 (Harga master jika opencost belum ada)
                Dim strSanitizeTrans As String = "UPDATE cld_inv_trans t " &
                "INNER JOIN cld_inv_transstockcls today " &
                "        ON t.stk_id = today.stkid AND t.cc = today.ccid AND t.custid = today.custid " &
                "       AND today.invdate = '" & sCurrDate & "' " &
                "LEFT JOIN cld_inv_stk s ON t.stk_id = s.id AND t.custid = s.custid " &
                "SET t.cost = CASE " &
                "        WHEN today.opencost > 0 THEN today.opencost " &
                "        WHEN s.price2 > 0 THEN s.price2 " &
                "        WHEN s.price1 > 0 THEN s.price1 " &
                "        ELSE t.cost " &
                "    END " &
                "WHERE t.custid = " & Custid & " " &
                "  AND t.type IN (1, 8, 9) " &
                "  AND t.flagtrans = 1 " &
                "  AND t.invdate = '" & sCurrDate & "' " &
                "  AND ( " &
                "      (today.opencost > 0 AND (t.cost >= 1.5 * today.opencost OR t.cost <= 0.5 * today.opencost)) " &
                "      OR " &
                "      (today.opencost <= 0 AND IFNULL(NULLIF(s.price2, 0), s.price1) > 0 AND (t.cost >= 1.5 * IFNULL(NULLIF(s.price2, 0), s.price1) OR t.cost <= 0.5 * IFNULL(NULLIF(s.price2, 0), s.price1))) " &
                "  )" & filterCCTrans & filterStkTrans
                c_cmd.executeNonQuery(strSanitizeTrans)

                ' STEP B: Hitung closing cost hari ini berdasarkan transaksi masuk (type IN (1, 3, 8, 9))
                ' PERBAIKAN DILUSI: Jika openqty <= 0 ATAU opencost <= 0, modal 100% mengikuti unit cost transaksi baru (transcost / transqty)
                Dim strUpdateTodayCost As String = "UPDATE cld_inv_transstockcls today " &
                "LEFT JOIN (" &
                "    SELECT stk_id, cc, " &
                "           SUM(ttlstock) AS transqty, " &
                "           SUM(ttlstock * cost) AS transcost " &
                "    FROM cld_inv_trans " &
                "    WHERE type IN (1, 3, 8, 9) " &
                "      AND flagtrans = 1 " &
                "      AND custid = " & Custid & " " &
                "      AND invdate = '" & sCurrDate & "' " &
                "      AND ttlstock > 0 AND cost > 0 " &
                filterCCTrans & filterStkTrans & " " &
                "    GROUP BY stk_id, cc" &
                ") trans ON today.stkid = trans.stk_id AND today.ccid = trans.cc " &
                "SET today.cost = CASE " &
                "    WHEN trans.transqty IS NULL OR trans.transqty <= 0 THEN " &
                "        CASE WHEN today.opencost > 0 THEN today.opencost ELSE today.cost END " &
                "    WHEN today.openqty <= 0 OR today.opencost <= 0 THEN " &
                "        (trans.transcost / trans.transqty) " &
                "    ELSE " &
                "        ((today.opencost * today.openqty) + trans.transcost) / (today.openqty + trans.transqty) " &
                "END " &
                "WHERE today.invdate = '" & sCurrDate & "' " &
                "  AND today.custid = " & Custid & filterCC & filterStk
                c_cmd.executeNonQuery(strUpdateTodayCost)

                ' STEP C: Pastikan jika cost hasil kalkulasi <= 0 tapi opencost > 0, pertahankan opencost
                Dim strSanityCost As String = "UPDATE cld_inv_transstockcls today " &
                "SET today.cost = today.opencost " &
                "WHERE today.invdate = '" & sCurrDate & "' " &
                "  AND today.custid = " & Custid & " " &
                "  AND (today.cost IS NULL OR today.cost <= 0) AND today.opencost > 0" & filterCC & filterStk
                c_cmd.executeNonQuery(strSanityCost)

                ' STEP D: Jika memproses hari ini, sinkronkan cost ke cld_inv_stk_cc
                If currDate = Today.Date Then
                    Dim strUpdateStkCc As String = "UPDATE cld_inv_stk_cc sc " &
                    "INNER JOIN cld_inv_transstockcls today " &
                    "   ON sc.stkid = today.stkid AND sc.ccid = today.ccid AND sc.custid = today.custid " &
                    "SET sc.cost = CASE WHEN today.cost > 0 THEN today.cost ELSE sc.cost END " &
                    "WHERE today.invdate = '" & sCurrDate & "' " &
                    "  AND today.custid = " & Custid & filterCC & filterStk
                    c_cmd.executeNonQuery(strUpdateStkCc)
                End If

                currDate = currDate.AddDays(1)
            Loop

        Catch ex As Exception
            errLog(Err.Description, "cls_recalculate:InvCaclculate_Cost_ActiveTransOnly")
            Err.Clear()
        Finally
            c_cmd.dispose()
        End Try
    End Sub

    Public Sub InvCaclculate_Cost_ActiveTransOnly_old(ByVal dstart As Date, ByVal dend As Date, ByVal Custid As Integer, Optional ByVal ccidFilter As Integer = 0, Optional ByVal stkidFilter As Integer = 0)
        Dim c_cmd As New clsDbo("")
        Try
            ' 1. Sesuai pedoman: pengecekan dimulai dari kemarinnya (dstart.AddDays(-1)) untuk memastikan modal opencost hari pertama valid
            Dim dFrom As Date = dstart.Date.AddDays(-1)
            Dim dTo As Date = dend.Date
            If dTo < dstart.Date Then dTo = dstart.Date

            Dim filterCC As String = ""
            Dim filterStk As String = ""
            Dim filterCCTrans As String = ""
            Dim filterStkTrans As String = ""

            If ccidFilter > 0 Then
                filterCC = " AND today.ccid = " & ccidFilter
                filterCCTrans = " AND cc = " & ccidFilter
            End If
            If stkidFilter > 0 Then
                filterStk = " AND today.stkid = " & stkidFilter
                filterStkTrans = " AND stk_id = " & stkidFilter
            End If

            ' 2. Pastikan opencost pada hari awal (dstart) tidak nol/kosong, jika masih nol fallback ke harga master stock
            Dim strInitOpen As String = "UPDATE cld_inv_transstockcls today " &
                "INNER JOIN cld_inv_stk stk ON today.stkid = stk.id AND today.custid = stk.custid " &
                "SET today.opencost = CASE WHEN stk.price2 > 0 THEN stk.price2 ELSE stk.price1 END " &
                "WHERE today.invdate = '" & dstart.ToString("yyyy-MM-dd 00:00:00") & "' " &
                "  AND today.custid = " & Custid & " AND (today.opencost IS NULL OR today.opencost <= 0)" &
                filterCC & filterStk
            c_cmd.executeNonQuery(strInitOpen)

            ' 3. Looping berurutan HARI DEMI HARI (Day-by-Day Cascade) dari dstart s/d dTo
            '    Hal ini menjamin cost mengalir runtut, membersihkan tanggal tanpa transaksi, dan tidak ada blind forward update
            Dim currDate As Date = dstart.Date
            Do While currDate <= dTo
                Dim prevDate As Date = currDate.AddDays(-1)
                Dim sCurrDate As String = currDate.ToString("yyyy-MM-dd 00:00:00")
                Dim sPrevDate As String = prevDate.ToString("yyyy-MM-dd 00:00:00")
                Dim sTransDate As String = currDate.ToString("yyyy-MM-dd")

                ' STEP A: Alirkan closing cost kemarin (prevDate) menjadi opencost hari ini (currDate)
                ' Pedoman: Jika bclose = 1 / b'1', maka keesokan hari opencost = closecost dan openqty = closeqty
                Dim strCascadeOpen As String = "UPDATE cld_inv_transstockcls today " &
                    "INNER JOIN cld_inv_transstockcls yest " &
                    "   ON today.stkid = yest.stkid AND today.ccid = yest.ccid AND today.custid = yest.custid " &
                    "  AND yest.invdate = '" & sPrevDate & "' " &
                    "SET today.opencost = CASE " &
                    "        WHEN (yest.bclose = 1 OR yest.bclose = b'1') AND yest.closecost > 0 THEN yest.closecost " &
                    "        WHEN yest.cost > 0 THEN yest.cost " &
                    "        ELSE today.opencost " &
                    "    END, " &
                    "    today.openqty = CASE " &
                    "        WHEN (yest.bclose = 1 OR yest.bclose = b'1') THEN yest.closeqty " &
                    "        ELSE today.openqty " &
                    "    END " &
                    "WHERE today.invdate = '" & sCurrDate & "' " &
                    "  AND today.custid = " & Custid &
                    "  AND (yest.cost > 0 OR yest.closecost > 0)" & filterCC & filterStk
                c_cmd.executeNonQuery(strCascadeOpen)

                ' STEP B0: Pembersihan (Sanitasi) Otomatis Anomali Ekstrem di cld_inv_trans untuk hari ini
                ' Aturan Pengguna: Langsung update cost anomali ekstrem pada cld_inv_trans, KECUALI yang beli dari supplier (type = 3)
                ' Kriteria ekstrem: cost >= 3x lipat atau cost <= 0.2x lipat dari baseline (opencost / master price)
                Dim strSanitizeTrans As String = "UPDATE cld_inv_trans t " &
                    "INNER JOIN cld_inv_transstockcls today " &
                    "        ON t.stk_id = today.stkid AND t.cc = today.ccid AND t.custid = today.custid " &
                    "       AND today.invdate = '" & sCurrDate & "' " &
                    "LEFT JOIN cld_inv_stk s ON t.stk_id = s.id AND t.custid = s.custid " &
                    "SET t.cost = CASE " &
                    "        WHEN today.opencost > 0 THEN today.opencost " &
                    "        WHEN s.price2 > 0 THEN s.price2 " &
                    "        WHEN s.price1 > 0 THEN s.price1 " &
                    "        ELSE t.cost " &
                    "    END " &
                    "WHERE t.custid = " & Custid & " " &
                    "  AND t.type IN (1, 8, 9) " &
                    "  AND t.type <> 3 " &
                    "  AND t.flagtrans = 1 " &
                    "  AND DATE(t.invdate) = '" & sTransDate & "' " &
                    "  AND ( " &
                    "      (today.opencost > 0 AND (t.cost >= 3.0 * today.opencost OR t.cost <= 0.2 * today.opencost)) " &
                    "      OR " &
                    "      (today.opencost <= 0 AND s.price1 > 0 AND (t.cost >= 3.0 * s.price1 OR t.cost <= 0.2 * s.price1)) " &
                    "  )" & filterCCTrans & filterStkTrans
                c_cmd.executeNonQuery(strSanitizeTrans)

                ' STEP B: Hitung closing cost hari ini berdasarkan transaksi masuk (type IN (1, 3, 8, 9))
                '         - type 1: Transfer In (tinQty)
                '         - type 3: Receiving PO / Supplier (rcvQty)
                '         - type 8: Head Office In (hoInQty)
                '         - type 9: Batch Production In (bpIn)
                '         Aturan Qty Minus:
                '         - Jika tidak ada transaksi masuk (transqty <= 0): cost tetap opencost (tidak diubah dgn qty minus)
                '         - Jika ada transaksi masuk dan openqty <= 0 (Opsi A): cost murni mengikuti harga barang baru masuk (transcost / transqty)
                '         - Jika ada transaksi masuk dan openqty > 0: rumus Moving Average berbobot
                Dim strUpdateTodayCost As String = "UPDATE cld_inv_transstockcls today " &
                    "LEFT JOIN (" &
                    "    SELECT stk_id, cc, " &
                    "           SUM(ttlstock) AS transqty, " &
                    "           SUM(ttlstock * cost) AS transcost " &
                    "    FROM cld_inv_trans " &
                    "    WHERE type IN (1, 3, 8, 9) " &
                    "      AND flagtrans = 1 " &
                    "      AND custid = " & Custid & " " &
                    "      AND DATE(invdate) = '" & sTransDate & "' " &
                    "      AND ttlstock > 0 AND cost > 0 " &
                    filterCCTrans & filterStkTrans & " " &
                    "    GROUP BY stk_id, cc" &
                    ") trans ON today.stkid = trans.stk_id AND today.ccid = trans.cc " &
                    "SET today.cost = CASE " &
                    "    WHEN trans.transqty IS NULL OR trans.transqty <= 0 THEN " &
                    "        CASE WHEN today.opencost > 0 THEN today.opencost ELSE today.cost END " &
                    "    WHEN today.openqty <= 0 THEN " &
                    "        (trans.transcost / trans.transqty) " &
                    "    ELSE " &
                    "        ((today.opencost * today.openqty) + trans.transcost) / (today.openqty + trans.transqty) " &
                    "END " &
                    "WHERE today.invdate = '" & sCurrDate & "' " &
                    "  AND today.custid = " & Custid & filterCC & filterStk
                c_cmd.executeNonQuery(strUpdateTodayCost)

                ' STEP C: Pastikan jika cost hasil kalkulasi <= 0 tapi opencost > 0, pertahankan opencost
                Dim strSanityCost As String = "UPDATE cld_inv_transstockcls today " &
                    "SET today.cost = today.opencost " &
                    "WHERE today.invdate = '" & sCurrDate & "' " &
                    "  AND today.custid = " & Custid & " " &
                    "  AND (today.cost IS NULL OR today.cost <= 0) AND today.opencost > 0" & filterCC & filterStk
                c_cmd.executeNonQuery(strSanityCost)

                ' STEP D: Jika memproses hari ini, sinkronkan cost ke cld_inv_stk_cc
                If currDate = Today.Date Then
                    Dim strUpdateStkCc As String = "UPDATE cld_inv_stk_cc sc " &
                        "INNER JOIN cld_inv_transstockcls today " &
                        "   ON sc.stkid = today.stkid AND sc.ccid = today.ccid AND sc.custid = today.custid " &
                        "SET sc.cost = CASE WHEN today.cost > 0 THEN today.cost ELSE sc.cost END " &
                        "WHERE today.invdate = '" & sCurrDate & "' " &
                        "  AND today.custid = " & Custid & filterCC & filterStk
                    c_cmd.executeNonQuery(strUpdateStkCc)
                End If

                currDate = currDate.AddDays(1)
            Loop

        Catch ex As Exception
            errLog(Err.Description, "cls_recalculate:InvCaclculate_Cost_ActiveTransOnly")
            Err.Clear()
        Finally
            c_cmd.dispose()
        End Try
    End Sub
    Public Sub InvCaclculate_rec2(ByVal dstart As Date, ByVal Custid As Integer, ByVal username As String)
        Try
            Dim x As Integer = 0
            Dim i As Integer
            Dim df As Date
            Dim dmax As Date
            Dim dsin As DataSet
            Dim ddate As Date = Nothing
            Dim c_cmd As New clsDbo("")
            Dim CsOname As New clsInventory_stkOpname
            Dim clsinv As New clsInventory
            Dim clsinvpro As New clsInventoryProcess

            'tambahan erik 30 sept 2020
            Dim dOpenQty As Decimal = 0
            Dim dCost As Decimal = 0

            df = dstart
            dmax = dstart
            i = DateDiff(DateInterval.Day, df, Today())

            df.AddDays(-1)





            dsin = SelectDs_stockopname(Custid, df)

                    If dsin.Tables(0).Rows.Count > 0 Then
                        'c_cmd.executeNonQuery(delstr)

                        For Each row As DataRow In dsin.Tables(0).Rows





                            dTotalQty = row.Item("ttlStock")


                            dDate = row.Item("invdate")
                            If c_cmd.ExecuteScalar("SELECT COUNT(ccid) FROM cld_inv_transstockcls WHERE custid = " & Custid & " AND ccid= " & row.Item("cc") & " AND stkid= " & row.Item("stk_id") & " AND invdate='" & dDate.ToString("yyyy-MM-dd 00:00:00") & "'", 0) > 0 Then
                        If c_cmd.executeNonQuery("update cld_inv_transstockcls set bclose=1, closeqty=" & dTotalQty & ", varianceqty= openqty - " & dTotalQty & ", closecost=" & row.Item("cost") & "
                                    where stkid = " & row.Item("stk_id") & " And ccid = " & row.Item("cc") & " And invdate = '" & ddate.ToString("yyyy-MM-dd 00:00:00") & "' and custid = " & Custid & "") = False Then
                            writeerrcsv(True, ddate.Date.ToString("yyyy-MM-dd 00:00:00"), row.Item("cc"), row.Item("stk_id"))
                        End If

                    Else
                        If c_cmd.executeNonQuery("insert into cld_inv_transstockcls values ('" & ddate.ToString("yyyy-MM-dd 00:00:00") & "', " & Custid & "," & row.Item("stk_id") & ", '" & row.Item("stk_No") & "', '" & row.Item("Stk_name") & "'," &
                               " " & row.Item("cc") & ", '" & row.Item("cc_name") & "', 0, 0, 0, 0, 0, 0, " &
                               " 0, 0, 0, 0, 0, 1, " & dTotalQty & ", " & row.Item("Cost") & ", " & -dTotalQty & ", 0, 0, 0, 0, 0)") = False Then
                            writeerrcsv(True, ddate.Date.ToString("yyyy-MM-dd 00:00:00"), row.Item("cc"), row.Item("stk_id"))
                        End If
                    End If

                    clsinvpro.InvDate = CDate(Now)
                    clsinvpro.executeNonQuery_Inv_Processstock(ddate, CDate(Now), Custid, row.Item("stk_id"), row.Item("cc"), True)
                    'c_cmd.executeNonQuery("update cld_inv_transstockcls set  ttlqty = Openqty + rcvqty-rtnqty+tinqty-toutqty-rwasteqty-fwasteqty-usageqty-hooutqty+hoinqty-bpout+bpin where stkid = " & row.Item("stk_id") & " and ccid = " & row.Item("cc") & " and invdate = '" & dDate.Date.AddDays(1).ToString("yyyy-MM-dd") & "' and custid = " & Custid & "")


                    c_cmd.dispose()

                            'insert_invtrans_opening_rec(Custid, df, row.Item("cc"), row.Item("stk_id"), row.Item("stk_name"), dOpenQty, row.Item("factor1"), row.Item("stk_no"), row.Item("unit1"), row.Item("unit2"), dCost, row.Item("cc_name"), username)
                        Next
                    End If





        Catch ex As Exception
            errLog(Err.Description, "clsInventory:InvCaclculate")
            Err.Clear()
        End Try
    End Sub

    Public Function SelectDs_stockopname(ByVal custid As Integer, ByVal dt As Date) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""


        Dim a As New clsDbo("")

        'Process_InvDataStock(dt.Date, dt.Date)
        Try
            a.ReNew()



            'perubahan dari type = 5 menjadi type = 12 untuk stock opname

            ds = a.ReturnDataset("SELECT DATE_ADD(invdate,INTERVAL -1 DAY) as invdate,ttlStock,cost,(select number from cld_inv_stk where id = i_trans.stk_id) stk_no,(select name from cld_inv_stk where id = i_trans.stk_id) stk_name,stk_id,cc,(select name from cld_inv_cc where id = i_trans.cc) cc_name FROM `cld_inv_trans` i_trans WHERE TYPE=12 AND i_trans.invdate= '" & dt.Date.ToString("yyyy-MM-dd 00:00:00") & "' AND I_trans.`custid` = " & custid & " order by i_trans.stk_id desc")



        Catch ex As Exception
            errLog(Err.Description, "cls_recalculate:SelectDs_stockopname")
            Err.Clear()
        End Try



        Return ds
        a.dispose()
        a = Nothing
    End Function

    Public Sub recalFlag_dailycost(ByVal ccid As Integer, ByVal invdate As Date, ByVal custid As Integer)
        Dim c_cmd As New clsDbo("")
        Dim olid As Integer

        Try
            olid = c_cmd.ExecuteScalar("select olid from cld_inv_cc where id = " & ccid & "", 0)
            If olid <> 0 Then
                c_cmd.ReNew()
                If c_cmd.ExecuteScalar("SELECT COUNT(olid) FROM `cld_sumcostdaily` where olid=" & olid & " and invdate ='" & invdate.ToString("yyyy-MM-dd 00:00:00") & "' and custid = " & custid & " ", 0) > 0 Then
                    c_cmd.executeNonQuery("update cld_sumcostdaily set recal = 1 where olid=" & olid & " and invdate ='" & invdate.ToString("yyyy-MM-dd 00:00:00") & "' and custid = " & custid & "")

                End If

            End If

        Catch ex As Exception
            errLog(Err.Description, "cls_recalculate:recalFlag_dailycost")
            Err.Clear()
        End Try
        c_cmd.dispose()
    End Sub
    Public Sub InvCaclculate_opening(ByVal dstart As Date, ByVal Custid As Integer, ByVal username As String)
        Try

            Dim df As Date

            Dim ddate As Date = Nothing
            Dim c_cmd As New clsDbo("")
            Dim CsOname As New clsInventory_stkOpname
            Dim clsinv As New clsInventory
            Dim clsinvpro As New clsInventoryProcess
            Dim dsin As DataSet
            'tambahan erik 30 sept 2020
            Dim dOpenQty As Decimal = 0
            Dim dCost As Decimal = 0
            Dim xtry As Integer = 0
            df = dstart


            df.AddDays(-1)



try1:

            dsin = getDs_difcls(Custid, df)

            If dsin.Tables(0).Rows.Count > 0 Then
                'c_cmd.executeNonQuery(delstr)

                For Each row As DataRow In dsin.Tables(0).Rows



                    ddate = row.Item("invdate")
                    If c_cmd.executeNonQuery("update cld_inv_transstockcls set ttlqty= " & row.Item("ttlcal") & " where stkid = " & row.Item("stkid") & " And ccid = " & row.Item("ccid") & " And invdate = '" & ddate.ToString("yyyy-MM-dd 00:00:00") & "' and custid = " & Custid & "") = False Then
                        writeerrcsv(True, ddate.Date.ToString("yyyy-MM-dd 00:00:00"), row.Item("ccid"), row.Item("stkid"))
                    End If




                    clsinvpro.InvDate = row.Item("invdate") 'ddate
                    clsinvpro.Inv_Proc_trans_stckClsbyid(row.Item("invdate"), Today(), Custid, row.Item("ccid"), row.Item("stkid"))

                    c_cmd.dispose()

                Next

                If xtry <> 0 Then
                    Exit Sub
                End If

                If cekopening(Custid, dstart) = True Then

                    If xtry = 0 Then
                        xtry = xtry + 1
                        GoTo try1
                    End If

                End If
            Else
                If cekopening(Custid, dstart) = True Then

                    If xtry = 0 Then
                        xtry = xtry + 1
                        GoTo try1
                    End If
                End If
                'Exit Sub
            End If
            'dsin = Nothing

            'dsin = getdifopening(Custid, df)
            'If dsin.Tables(0).Rows.Count > 0 Then
            '    'c_cmd.executeNonQuery(delstr)

            '    For Each row As DataRow In dsin.Tables(0).Rows


            '        clsinvpro.InvDate = row.Item("invdate") 'ddate
            '        clsinvpro.Inv_Proc_trans_stckClsbyid(row.Item("invdate"), Today(), Custid, row.Item("ccid"), row.Item("stkid"))

            '        c_cmd.dispose()

            '    Next
            '    GoTo try1
            'Else
            '    Exit Sub
            'End If



        Catch ex As Exception
            errLog(Err.Description, "clsInventory:InvCaclculate_opening")
            Err.Clear()
        End Try
    End Sub

    Public Function getDs_difcls(ByVal custid As Integer, ByVal dt As Date) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""


        Dim a As New clsDbo("")

        'Process_InvDataStock(dt.Date, dt.Date)
        Try
            a.ReNew()



            'perubahan dari type = 5 menjadi type = 12 untuk stock opname

            ds = a.ReturnDataset(" SELECT invdate,stkid,stkno,stkname,ccid,ccname,ttlqty,
(openqty+rcvqty-rtnqty+tinqty-toutqty-rwasteqty-fwasteQty-usageqty-HoOutQty+HoInQty+bpin-bpout-cwasteqty)ttlcal
 FROM cld_inv_transstockcls WHERE invdate>= '" & dt.Date.ToString("yyyy-MM-dd 00:00:00") & "' 
 AND ttlqty<>(openqty+rcvqty-rtnqty+tinqty-toutqty-rwasteqty-fwasteQty-usageqty-HoOutQty+HoInQty+bpin-bpout-cwasteqty) AND `custid` = " & custid)



        Catch ex As Exception
            errLog(Err.Description, "cls_recalculate:getDs_difcls")
            Err.Clear()
        End Try



        Return ds
        a.dispose()
        a = Nothing
    End Function
    Public Function getdifopening(ByVal custid As Integer, ByVal dt As Date) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""


        Dim a As New clsDbo("")

        'Process_InvDataStock(dt.Date, dt.Date)
        Try
            a.ReNew()



            'perubahan dari type = 5 menjadi type = 12 untuk stock opname

            ds = a.ReturnDataset("SELECT c1.invdate,c1.stkid,c1.stkno,c1.stkname,c1.ccid,c1.ccname,c1.openqty,
   (SELECT CASE WHEN bclose=0 THEN ttlqty ELSE closeqty END FROM cld_inv_transstockcls WHERE invdate=DATE_ADD(c1.invdate,INTERVAL -1 DAY) AND ccid = c1.ccid AND stkid = c1.stkid) opttl
  FROM cld_inv_transstockcls c1 WHERE c1.invdate>= '" & dt.Date.ToString("yyyy-MM-dd 00:00:00") & "' AND c1.custid = " & custid & "
  AND openqty<>(SELECT CASE WHEN bclose=0 THEN ttlqty ELSE closeqty END FROM cld_inv_transstockcls WHERE invdate=DATE_ADD(c1.invdate,INTERVAL -1 DAY) AND ccid = c1.ccid AND stkid = c1.stkid)")


        Catch ex As Exception
            errLog(Err.Description, "cls_recalculate:getdifopening")
            Err.Clear()
        End Try



        Return ds
        a.dispose()
        a = Nothing
    End Function

    Public Function cekopening(ByVal custid As Integer, ByVal invdate As Date) As Boolean
        Dim c_cmd As New clsDbo("")
        Dim hasil As Boolean = False
        Dim ds As DataSet
        Dim openqty As Double
        Dim ttlyes As Double
        Dim ddate As Date
        Dim clsinvpro As New clsInventoryProcess
        Try
            ds = c_cmd.ReturnDataset("SELECT invdate,stkid,stkno,stkname,ccid,ccname,openqty FROM cld_inv_transstockcls
                                       WHERE invdate >= '" & invdate.ToString("yyyy-MM-dd 00:00:00") & "' AND custid = " & custid)
            If ds.Tables(0).Rows.Count > 0 Then
                For Each row As DataRow In ds.Tables(0).Rows
                    openqty = row.Item("openqty")
                    ddate = row.Item("invdate")
                    ttlyes = c_cmd.ExecuteScalar("SELECT CASE WHEN bclose=0 THEN ttlqty ELSE closeqty END from cld_inv_transstockcls where invdate='" & ddate.AddDays(-1).ToString("yyyy-MM-dd 00:00:00") & "' AND ccid = " & row.Item("ccid") & " AND stkid = " & row.Item("stkid") & " ", 0)
                    If openqty <> ttlyes Then
                        clsinvpro.InvDate = ddate
                        clsinvpro.Inv_Proc_trans_stckClsbyid(ddate.AddDays(-1), Today(), custid, row.Item("ccid"), row.Item("stkid"))

                    End If
                Next

                hasil = True

            End If



        Catch ex As Exception
            hasil = False
            errLog(Err.Description, "cls_recalculate:recalFlag_dailycost")
            Err.Clear()
        End Try
        Return hasil
        c_cmd.dispose()
    End Function

    Public Sub SyncCostFromWarehouse(ByVal dstart As Date, ByVal Custid As Integer, ByVal ccidFilter As Integer, ByVal stkidFilter As Integer)
        ' STEP 1: Cari ID Cost Centre WAREHOUSE
        Dim wh_ccid As Integer = 0
        Dim cmdWH As New clsDbo("")
        Try
            wh_ccid = Convert.ToInt32(cmdWH.ExecuteScalar("SELECT id FROM cld_inv_cc WHERE custid=" & Custid & " AND name LIKE '%WAREHOUSE%' LIMIT 1", 0))
        Catch
        End Try
        If wh_ccid = 0 Then wh_ccid = 2 ' default fallback

        Try
            Dim dBaseDate As Date = dstart.Date.AddDays(-1)
            
            ' STEP 2: Sinkronkan cost & opencost outlet pada dBaseDate berdasarkan cost Warehouse + markup outlet
            Dim strSyncBase As String = "UPDATE cld_inv_transstockcls dest " &
                "INNER JOIN cld_inv_cc cc ON dest.ccid = cc.id " &
                "LEFT JOIN cld_outlet ol ON cc.olid = ol.id " &
                "LEFT JOIN cld_own own ON ol.ownid = own.id " &
                "INNER JOIN cld_inv_transstockcls wh " &
                "   ON dest.stkid = wh.stkid AND wh.ccid = " & wh_ccid & " AND wh.custid = dest.custid AND wh.invdate = '" & dBaseDate.ToString("yyyy-MM-dd 00:00:00") & "' " &
                "SET dest.cost = ROUND(wh.cost * (1 + (IFNULL(own.markup, 0) / 100)), 4), " &
                "    dest.opencost = ROUND(wh.cost * (1 + (IFNULL(own.markup, 0) / 100)), 4) " &
                "WHERE dest.custid = " & Custid & " " &
                "  AND dest.ccid <> " & wh_ccid & " " &
                "  AND dest.invdate = '" & dBaseDate.ToString("yyyy-MM-dd 00:00:00") & "' " &
                "  AND wh.cost > 0"
            If ccidFilter > 0 Then strSyncBase &= " AND dest.ccid = " & ccidFilter
            If stkidFilter > 0 Then strSyncBase &= " AND dest.stkid = " & stkidFilter

            Dim cmdSync As New clsDbo("")
            cmdSync.executeNonQuery(strSyncBase)
            cmdSync.dispose()

            ' STEP 3: Jalankan rekalkulasi cascading Moving Average dari dstart sampai hari ini
            '         Nilai base dari warehouse di dBaseDate akan mengalir menjadi opencost di dstart
            '         dan bergerak runtut hari-demi-hari mengikuti transaksi masuk yang ada.
            InvCaclculate_Cost_ActiveTransOnly(dstart, Now.Date, Custid, ccidFilter, stkidFilter)

        Catch ex As Exception
            errLog(Err.Description, "cls_recalculate:SyncCostFromWarehouse")
            Err.Clear()
        Finally
            cmdWH.dispose()
        End Try
    End Sub

End Class
