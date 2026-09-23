Imports MySql.Data.MySqlClient

Public Class ClsReports

    'Revisi 08 Juni
    'Private Enum Modulepriv
    '    moduleuser = &H1
    'End Enum

    '16 mei 2018,
    'ada penambahan report:
    '1 Report Disc Summary
    '2 Report Payment
    '3 Report Payment Summary

    'Revisi 08 Juni
    'update data ne
    Public Function getDataset(ByVal namaRpt As String, ByVal rptParam As ClsParamRpt) As DataSet
        Dim dsRslt As New DataSet

        Try
            Select Case namaRpt.ToUpper

                Case "outlet".ToUpper
                    dsRslt = loadOutlet(rptParam.custID, rptParam.usr, rptParam.Modpriv)

                Case "memberList".ToUpper
                    dsRslt = memberList(rptParam.custID, rptParam.usr, rptParam.Modpriv)

                Case "totalSales".ToUpper
                    dsRslt = totalSales(rptParam.busiDate, rptParam.outletNo, rptParam.custID, rptParam.usr, rptParam.Modpriv)

                Case "dailySales".ToUpper
                    dsRslt = dailySales(rptParam.busiDate, rptParam.outletNo, rptParam.custID, rptParam.usr, rptParam.Modpriv)

                Case "hourlySales".ToUpper
                    dsRslt = hourlySales(rptParam.busiDate, rptParam.outletNo, rptParam.custID, rptParam.usr, rptParam.Modpriv)

                Case "WeeklySales".ToUpper
                    dsRslt = WeeklySales(rptParam.busiDate, rptParam.outletNo, rptParam.custID, rptParam.usr, rptParam.Modpriv, rptParam.weekofmonth)
                Case "WeekofMonth".ToUpper
                    dsRslt = WeekOfMonth(rptParam.busiDate, rptParam.custID, rptParam.usr, rptParam.Modpriv)


                Case "itemByAmt".ToUpper
                    dsRslt = itemByAmt(rptParam.busiDate, rptParam.outletNo, rptParam.custID, rptParam.usr, rptParam.Modpriv)

                Case "itemByQty".ToUpper
                    dsRslt = itemByQty(rptParam.busiDate, rptParam.outletNo, rptParam.custID, rptParam.usr, rptParam.Modpriv)

                Case "memberTop".ToUpper
                    dsRslt = memberTop(rptParam.busiDate, rptParam.outletNo, rptParam.custID, rptParam.usr, rptParam.Modpriv)

                Case "memberSalesSum".ToUpper
                    dsRslt = memberSalesSum(rptParam.paramFrom, rptParam.paramTo, rptParam.outletNo, rptParam.custID, rptParam.usr, rptParam.Modpriv, rptParam.memberNo)

                Case "memberSalesDtl".ToUpper
                    dsRslt = memberSalesDtl(rptParam.paramFrom, rptParam.paramTo, rptParam.outletNo, rptParam.custID, rptParam.usr, rptParam.Modpriv, rptParam.memberNo)

                Case "salesSum".ToUpper
                    dsRslt = salesSum(rptParam.paramFrom, rptParam.paramTo, rptParam.outletNo, rptParam.memberID, rptParam.custID, rptParam.usr, rptParam.Modpriv)
'perubahan 15Mei2019
                Case "Sales Summary Report".ToUpper
                    dsRslt = salesSum2(rptParam.custID, rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.ownID, rptParam.brandstr, rptParam.citystr, rptParam.usr, rptParam.Modpriv)

                Case "Non-sales Summary Report".ToUpper
                    dsRslt = NonsalesSum2(rptParam.custID, rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.ownID, rptParam.brandstr, rptParam.citystr, rptParam.usr, rptParam.Modpriv)


                Case "salesSumMember".ToUpper
                    dsRslt = salesSumMember(rptParam.paramFrom, rptParam.paramTo, rptParam.outletNo, rptParam.memberID, rptParam.custID, rptParam.usr, rptParam.Modpriv)

                Case "salesDetail".ToUpper
                    dsRslt = salesDetail(rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.ownID, rptParam.brandstr, rptParam.citystr, rptParam.memberID, rptParam.custID, rptParam.usr, rptParam.Modpriv)

                Case "Sales Detail Report".ToUpper
                    dsRslt = salesDetail2(rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.ownID, rptParam.brandstr, rptParam.citystr, rptParam.custID, rptParam.usr, rptParam.Modpriv)

                Case "Sales Detail Report 2018".ToUpper
                    dsRslt = salesDetail2_2018(rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.ownID, rptParam.brandstr, rptParam.citystr, rptParam.custID, rptParam.usr, rptParam.Modpriv)



                Case "Item Sales".ToUpper
                    dsRslt = itemSales(rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.ownID, rptParam.brandstr, rptParam.citystr, rptParam.custID, rptParam.usr, rptParam.Modpriv)

                Case "Item Non-sales".ToUpper
                    dsRslt = itemNonSales(rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.ownID, rptParam.brandstr, rptParam.citystr, rptParam.custID, rptParam.usr, rptParam.Modpriv)


                Case "itemSales2".ToUpper
                    dsRslt = itemSales2(rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.ownID, rptParam.brandstr, rptParam.citystr, rptParam.itemstr, rptParam.custID, rptParam.usr, rptParam.Modpriv)

                Case "itemGroupSales".ToUpper
                    dsRslt = itemGroupSales(rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.ownID, rptParam.brandstr, rptParam.citystr, rptParam.custID, rptParam.usr, rptParam.Modpriv)

                Case "Discount Report".ToUpper
                    dsRslt = Discount(rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.ownID, rptParam.brandstr, rptParam.citystr, rptParam.custID, rptParam.usr, rptParam.Modpriv)

                Case "Discount Report 2018".ToUpper
                    dsRslt = Discount2018(rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.ownID, rptParam.brandstr, rptParam.citystr, rptParam.custID, rptParam.usr, rptParam.Modpriv)

                '16 mei 2018
                Case "Discount Summary Report".ToUpper
                    dsRslt = DiscountSummary(rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.ownID, rptParam.brandstr, rptParam.citystr, rptParam.custID, rptParam.usr, rptParam.Modpriv)

                 '16 mei 2018
                Case "Payment".ToUpper
                    dsRslt = Payment2(rptParam.paramFrom, rptParam.paramTo, rptParam.outletNo, rptParam.custID, rptParam.usr, rptParam.Modpriv)

                 '16 mei 2018
                Case "Payment Summary".ToUpper
                    dsRslt = PaymentSummary(rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.ownID, rptParam.brandstr, rptParam.citystr, rptParam.custID, rptParam.usr, rptParam.Modpriv)

                Case "Payment Non-sales Summary".ToUpper
                    dsRslt = PaymentSummary_Nonsales(rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.ownID, rptParam.brandstr, rptParam.citystr, rptParam.custID, rptParam.usr, rptParam.Modpriv)


                Case "member".ToUpper
                    dsRslt = member(rptParam.memberID)

                Case "salesItem".ToUpper
                    dsRslt = salesItem(rptParam.paramFrom, rptParam.paramTo, rptParam.outletNo, rptParam.memberID, rptParam.usr, rptParam.Modpriv)

                Case "salesItemByStore".ToUpper
                    dsRslt = salesItemByStore(rptParam.paramFrom, rptParam.paramTo, rptParam.outletNo, rptParam.memberID, rptParam.usr, rptParam.Modpriv)

                Case "salesItemByHour".ToUpper
                    dsRslt = salesItemByHour(rptParam.paramFrom, rptParam.paramTo, rptParam.outletNo, rptParam.memberID, rptParam.usr, rptParam.Modpriv)

                Case "itemStoreChart".ToUpper
                    dsRslt = itemStoreChart(rptParam.paramFrom, rptParam.paramTo, rptParam.outletNo, rptParam.memberID, rptParam.usr, rptParam.Modpriv)

                Case "ItemHourChart".ToUpper
                    dsRslt = ItemHourChart(rptParam.paramFrom, rptParam.paramTo, rptParam.outletNo, rptParam.memberID, rptParam.usr, rptParam.Modpriv)

                Case "topItemQty".ToUpper
                    dsRslt = topItemQty(rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.ownID, rptParam.brandstr, rptParam.citystr, rptParam.custID, rptParam.usr, rptParam.Modpriv)
'tambahan 17Mei2019
                Case "topItemAmt".ToUpper
                    dsRslt = topItemAmt(rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.ownID, rptParam.brandstr, rptParam.citystr, rptParam.custID, rptParam.usr, rptParam.Modpriv)

                Case "topMemberSpend".ToUpper
                    dsRslt = topMemberSpend(rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.ownID, rptParam.brandstr, rptParam.citystr, rptParam.custID, rptParam.usr, rptParam.Modpriv)

                Case "topMemberVisit".ToUpper
                    dsRslt = topMemberVisit(rptParam.paramFrom, rptParam.paramTo, rptParam.outletNo, rptParam.custID, rptParam.usr, rptParam.Modpriv)

                Case "Top Store Revenue".ToUpper
                    dsRslt = topStore(rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.ownID, rptParam.brandstr, rptParam.citystr, rptParam.custID, rptParam.usr, rptParam.Modpriv)
                Case "Top Store Visit".ToUpper
                    dsRslt = topStore_visit(rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.ownID, rptParam.brandstr, rptParam.citystr, rptParam.custID, rptParam.usr, rptParam.Modpriv)


                Case "Top Store Revenue by Hour".ToUpper
                    dsRslt = topStoreByHour(rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.ownID, rptParam.brandstr, rptParam.citystr, rptParam.custID, rptParam.usr, rptParam.Modpriv)
                Case "Top Store Visit by Hour".ToUpper
                    dsRslt = top_visit_StoreByHour(rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.ownID, rptParam.brandstr, rptParam.citystr, rptParam.custID, rptParam.usr, rptParam.Modpriv)

                Case "memberPoint".ToUpper
                    dsRslt = memberPoint(rptParam.paramFrom, rptParam.paramTo, rptParam.outletNo, rptParam.custID, rptParam.usr, rptParam.Modpriv, rptParam.memberNo)

                    'penambahan 15 mei 2017
                    'report untuk perubahan point, 

                Case "memberRewardPoint".ToUpper
                    dsRslt = memberRewardPoint(rptParam.paramFrom, rptParam.paramTo, rptParam.outletNo, rptParam.custID, rptParam.usr, rptParam.Modpriv, rptParam.memberNo)

                    'penambahan 15 mei 2017
                    'report untuk perubahan point, 
                Case "memberpointEdit".ToUpper
                    dsRslt = memberpointEdit(rptParam.paramFrom, rptParam.paramTo, rptParam.custID, rptParam.usr, rptParam.Modpriv, rptParam.memberNo)

                Case "comission".ToUpper
                    dsRslt = comission(rptParam.paramFrom, rptParam.paramTo, rptParam.outletNo, rptParam.custID, rptParam.usr, rptParam.Modpriv)


                'Case "Void Bill".ToUpper
                '    dsRslt = voidBill(rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.ownID, rptParam.brandstr, rptParam.citystr, rptParam.custID, rptParam.usr, rptParam.Modpriv)

                'Case "Void Bill 2018".ToUpper
                '    dsRslt = voidBill2018(rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.ownID, rptParam.brandstr, rptParam.citystr, rptParam.custID, rptParam.usr, rptParam.Modpriv)



                Case "Void Report".ToUpper
                    dsRslt = voidItem(rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.ownID, rptParam.brandstr, rptParam.citystr, rptParam.custID, rptParam.usr, rptParam.Modpriv)

                Case "Void Report 2018".ToUpper
                    dsRslt = voidItem2018(rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.ownID, rptParam.brandstr, rptParam.citystr, rptParam.custID, rptParam.usr, rptParam.Modpriv)


                Case "Non-sales Detail".ToUpper
                    dsRslt = Nonsales_detail(rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.ownID, rptParam.brandstr, rptParam.citystr, rptParam.custID, rptParam.usr, rptParam.Modpriv)

                Case "Non-sales Notes".ToUpper
                    dsRslt = Nonsales_notes(rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.ownID, rptParam.brandstr, rptParam.citystr, rptParam.custID, rptParam.usr, rptParam.Modpriv)


                    'perubahan 27 feb 2017, 'berdasarkan user modpriv
                Case "receiveReturn".ToUpper
                    dsRslt = receiveReturn(rptParam.paramFrom, rptParam.paramTo, rptParam.custID, rptParam.usr, rptParam.Modpriv, rptParam.ccID)

                    'perubahan 27 feb 2017, 'berdasarkan user modpriv
                Case "invTransferFrom".ToUpper
                    dsRslt = invTransferFrom(rptParam.paramFrom, rptParam.paramTo, rptParam.custID, rptParam.usr, rptParam.Modpriv, rptParam.ccID)

                Case "invTransferTo".ToUpper
                    dsRslt = invTransferTo(rptParam.paramFrom, rptParam.paramTo, rptParam.custID, rptParam.usr, rptParam.Modpriv, rptParam.ccID)

                Case "stkOnHand".ToUpper
                    dsRslt = stkOnHand(rptParam.custID, rptParam.usr, rptParam.Modpriv, rptParam.ccID)

                Case "invStockOpname".ToUpper
                    dsRslt = invStockOpname(rptParam.paramFrom, rptParam.paramTo, rptParam.custID, rptParam.usr, rptParam.Modpriv, rptParam.ccID)

                Case "otherExpense".ToUpper
                    dsRslt = otherExpense(rptParam.paramFrom, rptParam.paramTo, rptParam.custID, rptParam.usr, rptParam.Modpriv)

                Case "movementCurrent".ToUpper
                    dsRslt = movementCurrent(rptParam.custID, rptParam.usr, rptParam.Modpriv, rptParam.ccID)

                Case "menuItem".ToUpper
                    dsRslt = menuItem(rptParam.custID)

                Case "revenueVsCost".ToUpper
                    dsRslt = revenueVsCost(rptParam.paramFrom, rptParam.paramTo, rptParam.custID, rptParam.usr, rptParam.Modpriv)

                    'Bram Tambahan
                    '21 april 2017
                    'Receipt Detail Reg CRP
                    'revisi 9 Mei 2017
                'Case "rcpdetails".ToUpper
                '    dsRslt = Rcptdetils(rptParam.paramFrom, rptParam.paramTo, rptParam.outletNo, rptParam.custID, rptParam.usr, rptParam.Modpriv)
                Case "Receipt Detail".ToUpper
                    dsRslt = Rcptdetils2(rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.custID, rptParam.usr, rptParam.Modpriv, rptParam.ownID, rptParam.brandstr)

                Case "Receipt Detail 2018".ToUpper
                    dsRslt = Rcptdetils2_2018(rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.custID, rptParam.usr, rptParam.Modpriv, rptParam.ownID, rptParam.brandstr)

                    '30 maret 2017
                    'Review/ FeedBack
                Case "Poorreviews".ToUpper
                    dsRslt = PoorReviews(rptParam.paramFrom, rptParam.paramTo, rptParam.outletNo, rptParam.custID, rptParam.usr, rptParam.Modpriv)
                Case "Goodreviews".ToUpper
                    dsRslt = GoodReviews(rptParam.paramFrom, rptParam.paramTo, rptParam.outletNo, rptParam.custID, rptParam.usr, rptParam.Modpriv)
                Case "feedbackDtl".ToUpper
                    dsRslt = feedbackDtl(rptParam.paramFrom, rptParam.paramTo, rptParam.outletNo, rptParam.custID, rptParam.usr, rptParam.Modpriv)
                Case "feedbackSum".ToUpper
                    dsRslt = feedbackSum(rptParam.paramFrom, rptParam.paramTo, rptParam.outletNo, rptParam.custID, rptParam.usr, rptParam.Modpriv)
                Case "feedbackGraph".ToUpper
                    dsRslt = feedbackGraph(rptParam.paramFrom, rptParam.paramTo, rptParam.outletNo, rptParam.memberID, rptParam.custID, rptParam.usr, rptParam.Modpriv)

                    'Deposit
                Case "DepMemberBalance".ToUpper
                    dsRslt = DepMemberBalance(rptParam.custID, rptParam.memberNo)
                Case "DepositSum".ToUpper
                    dsRslt = DepositSum(rptParam.paramFrom, rptParam.paramTo, rptParam.outletNo, rptParam.custID, rptParam.usr, rptParam.Modpriv, rptParam.memberNo)
                Case "DepositDetail".ToUpper
                    dsRslt = DepositDetail(rptParam.paramFrom, rptParam.paramTo, rptParam.outletNo, rptParam.custID, rptParam.usr, rptParam.Modpriv, rptParam.memberNo)
                Case "DepositTopup".ToUpper
                    dsRslt = DepositTopup(rptParam.paramFrom, rptParam.paramTo, rptParam.outletNo, rptParam.custID, rptParam.usr, rptParam.Modpriv, rptParam.memberNo)

                    'inventory Order HO 
                    '20 sept 2017

                Case "order_req".ToUpper
                    dsRslt = orderReq(rptParam.order_req, rptParam.custID, rptParam.ccID)
                    '22 sept 2017
                Case "send_req_order".ToUpper
                    dsRslt = send_req_order(rptParam.deliv_no, rptParam.custID, rptParam.ccID)
                    '25 sept 2017
                Case "stk_req_rpt".ToUpper
                    dsRslt = stk_req_rpt(rptParam.paramFrom, rptParam.paramTo, rptParam.custID, rptParam.outletNo, rptParam.usr, rptParam.Modpriv)
                Case "stk_req_sum_rpt".ToUpper
                    dsRslt = stk_req_rpt(rptParam.paramFrom, rptParam.paramTo, rptParam.custID, rptParam.outletNo, rptParam.usr, rptParam.Modpriv)

                    'Report tambahan bram 19 oct 2017

                Case "productionRpt".ToUpper
                    dsRslt = productionRpt(rptParam.paramFrom, rptParam.paramTo, rptParam.ccID, rptParam.custID, rptParam.usr, rptParam.Modpriv)
                Case "detailproductionRpt".ToUpper
                    dsRslt = detailproductionRpt(rptParam.paramFrom, rptParam.paramTo, rptParam.ccID, rptParam.custID, rptParam.usr, rptParam.Modpriv)

                Case "WasteRpt".ToUpper
                    dsRslt = WasteRpt(rptParam.paramFrom, rptParam.paramTo, rptParam.ccID, rptParam.custID, rptParam.usr, rptParam.Modpriv)

                    'bram 12 Oct 2017
                Case "sumPendPoRptDetail".ToUpper
                    dsRslt = sumPendPoRptDetail(rptParam.paramFrom, rptParam.paramTo, rptParam.ccID, rptParam.custID, rptParam.usr, rptParam.Modpriv)
                    'bram 12 Oct 2017
                Case "sumPendPoRpt".ToUpper
                    dsRslt = sumPendPoRpt(rptParam.paramFrom, rptParam.paramTo, rptParam.ccID, rptParam.custID, rptParam.usr, rptParam.Modpriv)
                    'bram 12 Oct 2017
                Case "InvSupplier".ToUpper
                    dsRslt = InvSupplier(rptParam.custID, rptParam.usr, rptParam.Modpriv)
                    'bram 12 Oct 2017
                Case "InvStock".ToUpper
                    dsRslt = InvStock(rptParam.custID, rptParam.usr, rptParam.Modpriv)

                    'bram 9 nov 2017 minimum stock
                Case "minLimitStkRpt".ToUpper
                    dsRslt = minLimitStkRpt(rptParam.ccID, rptParam.custID, rptParam.usr, rptParam.Modpriv)
                    'bram 9 nov 2017 Invoice Pending
                Case "notinvoicingInvRpt".ToUpper
                    dsRslt = notinvoicingInvRpt(rptParam.paramFrom, rptParam.paramTo, rptParam.ccID, rptParam.custID, rptParam.usr, rptParam.Modpriv)
                    'bram 9 nov 2017 Invoice Finish
                Case "InvReceiveRpt".ToUpper
                    dsRslt = InvReceiveRpt(rptParam.paramFrom, rptParam.paramTo, rptParam.ccID, rptParam.custID, rptParam.usr, rptParam.Modpriv)
                    'bram 9 nov 2017 Master CC
                Case "InvCallcenterRpt".ToUpper
                    dsRslt = InvCallcenterRpt(rptParam.custID, rptParam.usr, rptParam.Modpriv)

                    'erik 11 april 2019 PosvsWeb
                Case "Sales Pos Vs Web".ToUpper
                    dsRslt = PosVSWeb(rptParam.custID, rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.ownID, rptParam.brandstr, rptParam.citystr, rptParam.usr, rptParam.Modpriv)
                Case "Non-sales Pos Vs Web".ToUpper
                    dsRslt = PosVSWeb_nonsales(rptParam.custID, rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.ownID, rptParam.brandstr, rptParam.citystr, rptParam.usr, rptParam.Modpriv)

                    'dari menu EXPORT
                Case "Daily Sales".ToUpper
                    dsRslt = SelectDataTable_DailySales(rptParam.custID, rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.ownID, rptParam.brandstr, rptParam.citystr, rptParam.usr, rptParam.Modpriv)

                Case "Sales By Channel".ToUpper
                    dsRslt = SalesSumBySalesType(rptParam.custID, rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.ownID, rptParam.brandstr, rptParam.citystr, rptParam.usr, rptParam.Modpriv)
                Case "Monthly Sales".ToUpper
                    dsRslt = SelectDataTable_MonthlySales(rptParam.custID, rptParam.paramFrom, rptParam.storestr, rptParam.ownID, rptParam.brandstr, rptParam.citystr, rptParam.usr, rptParam.Modpriv)
                Case "Item Online Orders".ToUpper
                    dsRslt = SelectDataTable_Onlineorders(rptParam.custID, rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.ownID, rptParam.brandstr, rptParam.citystr, rptParam.usr, rptParam.Modpriv)
                Case "ItemGofood".ToUpper
                    dsRslt = SelectDataTable_GoFood(rptParam.custID, rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.ownID, rptParam.brandstr, rptParam.citystr, rptParam.usr, rptParam.Modpriv)
                Case "Online Delivery Orders".ToUpper
                    dsRslt = Selectdt_onlineorders(rptParam.custID, rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.ownID, rptParam.brandstr, rptParam.citystr, rptParam.usr, rptParam.Modpriv)
                Case "Item Sales Detail".ToUpper
                    dsRslt = SelectDataTable_SlsItem(rptParam.custID, rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.ownID, rptParam.brandstr, rptParam.citystr, rptParam.usr, rptParam.Modpriv)
                Case "Monthly Channel".ToUpper
                    dsRslt = SelectDataTable_Monthly_Channel(rptParam.custID, rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.ownID, rptParam.brandstr, rptParam.citystr, rptParam.usr, rptParam.Modpriv)
                Case "Sales Cover".ToUpper
                    dsRslt = SelectDataTable_Sales_Cover(rptParam.custID, rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.ownID, rptParam.brandstr, rptParam.citystr, rptParam.usr, rptParam.Modpriv)
                Case "Sales Vs Payment".ToUpper
                    dsRslt = SelectDataTable_SalesVSPayment(rptParam.custID, rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.ownID, rptParam.brandstr, rptParam.citystr, rptParam.usr, rptParam.Modpriv)
                Case "Cashier Report".ToUpper
                    dsRslt = LoanReport(rptParam.custID, rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.ownID, rptParam.brandstr, rptParam.citystr, rptParam.usr, rptParam.Modpriv)
                Case "Apps Orders Report".ToUpper
                    dsRslt = Appsorder(rptParam.custID, rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.ownID, rptParam.brandstr, rptParam.citystr, rptParam.usr, rptParam.Modpriv)

                Case "Payment Detail Report".ToUpper
                    dsRslt = Payment_Report(rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.ownID, rptParam.brandstr, rptParam.citystr, rptParam.custID, rptParam.usr, rptParam.Modpriv)
                Case "Item Group Sales Report".ToUpper
                    dsRslt = SelectDataTable_ItemGroup(rptParam.custID, rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.ownID, rptParam.brandstr, rptParam.citystr, rptParam.usr, rptParam.Modpriv)

                Case "Top 10 Item".ToUpper
                    dsRslt = top10item(rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.ownID, rptParam.brandstr, rptParam.citystr, rptParam.custID, rptParam.usr, rptParam.Modpriv)


                Case "Top 10 Item by Amount".ToUpper
                    dsRslt = topItemAmt(rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.ownID, rptParam.brandstr, rptParam.citystr, rptParam.custID, rptParam.usr, rptParam.Modpriv)
                Case "Top 10 Item by Qty".ToUpper
                    dsRslt = topItemQty(rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.ownID, rptParam.brandstr, rptParam.citystr, rptParam.custID, rptParam.usr, rptParam.Modpriv)

                Case "KDS Report".ToUpper
                    dsRslt = kds_report(rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.ownID, rptParam.brandstr, rptParam.citystr, rptParam.custID, rptParam.usr, rptParam.Modpriv)
                Case "Direct Delivery".ToUpper
                    dsRslt = PATOnlineOrder(rptParam.custID, rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.ownID, rptParam.brandstr, rptParam.citystr, rptParam.usr, rptParam.Modpriv)

                    'Case "ItemGofoodDaily".ToUpper
                    '    dsRslt = SelectDataTable_GoFood_Daily(CustID, sOutlet, dFrom, dTo, iOwn, iBrand)

                    '        'dari menu EXPORT All Store access
                    'Case "Sales_Item".ToUpper
                    '    dsRslt = SelectDataTable_SlsItem(CustID, sOutlet, dFrom, dTo, iOwn, iBrand, iCity)
                    'Case "Monthly_Channel".ToUpper
                    '    dsRslt = SelectDataTable_MonChannel(CustID, sOutlet, dFrom, dTo, iOwn, iBrand, iCity)


            End Select

        Catch ex As Exception
            errLog(Err.Description, "ClsReport:reportDataset")
            Err.Clear()
        End Try

        Return dsRslt

    End Function

    Public Function getDatasetgv(ByVal namaRpt As String, ByVal rptParam As ClsParamRpt) As DataSet
        Dim dsRslt As New DataSet

        Try
            Select Case namaRpt.ToUpper

                Case "outlet".ToUpper
                    dsRslt = loadOutlet(rptParam.custID, rptParam.usr, rptParam.Modpriv)

                Case "memberList".ToUpper
                    dsRslt = memberList(rptParam.custID, rptParam.usr, rptParam.Modpriv)

                Case "totalSales".ToUpper
                    dsRslt = totalSales(rptParam.busiDate, rptParam.outletNo, rptParam.custID, rptParam.usr, rptParam.Modpriv)

                Case "dailySales".ToUpper
                    dsRslt = dailySales(rptParam.busiDate, rptParam.outletNo, rptParam.custID, rptParam.usr, rptParam.Modpriv)

                Case "hourlySales".ToUpper
                    dsRslt = hourlySales(rptParam.busiDate, rptParam.outletNo, rptParam.custID, rptParam.usr, rptParam.Modpriv)

                Case "WeeklySales".ToUpper
                    dsRslt = WeeklySales(rptParam.busiDate, rptParam.outletNo, rptParam.custID, rptParam.usr, rptParam.Modpriv, rptParam.weekofmonth)
                Case "WeekofMonth".ToUpper
                    dsRslt = WeekOfMonth(rptParam.busiDate, rptParam.custID, rptParam.usr, rptParam.Modpriv)

                Case "itemByAmt".ToUpper
                    dsRslt = itemByAmt(rptParam.busiDate, rptParam.outletNo, rptParam.custID, rptParam.usr, rptParam.Modpriv)

                Case "itemByQty".ToUpper
                    dsRslt = itemByQty(rptParam.busiDate, rptParam.outletNo, rptParam.custID, rptParam.usr, rptParam.Modpriv)

                Case "memberTop".ToUpper
                    dsRslt = memberTop(rptParam.busiDate, rptParam.outletNo, rptParam.custID, rptParam.usr, rptParam.Modpriv)

                Case "memberSalesSum".ToUpper
                    dsRslt = memberSalesSum(rptParam.paramFrom, rptParam.paramTo, rptParam.outletNo, rptParam.custID, rptParam.usr, rptParam.Modpriv, rptParam.memberNo)

                Case "memberSalesDtl".ToUpper
                    dsRslt = memberSalesDtl(rptParam.paramFrom, rptParam.paramTo, rptParam.outletNo, rptParam.custID, rptParam.usr, rptParam.Modpriv, rptParam.memberNo)

                Case "salesSum".ToUpper
                    dsRslt = salesSum(rptParam.paramFrom, rptParam.paramTo, rptParam.outletNo, rptParam.memberID, rptParam.custID, rptParam.usr, rptParam.Modpriv)
'perubahan 15Mei2019
                Case "Sales Summary Report".ToUpper
                    dsRslt = salesSum_gv(rptParam.custID, rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.ownID, rptParam.brandstr, rptParam.citystr, rptParam.usr, rptParam.Modpriv)

                Case "Non-sales Summary Report".ToUpper
                    dsRslt = NonsalesSum_gv(rptParam.custID, rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.ownID, rptParam.brandstr, rptParam.citystr, rptParam.usr, rptParam.Modpriv)


                Case "salesSumMember".ToUpper
                    dsRslt = salesSumMember(rptParam.paramFrom, rptParam.paramTo, rptParam.outletNo, rptParam.memberID, rptParam.custID, rptParam.usr, rptParam.Modpriv)

                Case "salesDetail".ToUpper
                    dsRslt = salesDetail(rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.ownID, rptParam.brandstr, rptParam.citystr, rptParam.memberID, rptParam.custID, rptParam.usr, rptParam.Modpriv)

                Case "Sales Detail Report".ToUpper
                    dsRslt = salesDetail2_gv(rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.ownID, rptParam.brandstr, rptParam.citystr, rptParam.custID, rptParam.usr, rptParam.Modpriv)

                Case "Sales Detail Report 2018".ToUpper
                    dsRslt = salesDetail2_2018_gv(rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.ownID, rptParam.brandstr, rptParam.citystr, rptParam.custID, rptParam.usr, rptParam.Modpriv)


                Case "Item Sales".ToUpper
                    dsRslt = itemSales_gv(rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.ownID, rptParam.brandstr, rptParam.citystr, rptParam.custID, rptParam.usr, rptParam.Modpriv)

                Case "Item Non-sales".ToUpper
                    dsRslt = itemNonSales_gv(rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.ownID, rptParam.brandstr, rptParam.citystr, rptParam.custID, rptParam.usr, rptParam.Modpriv)


                Case "itemSales2".ToUpper
                    dsRslt = itemSales2(rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.ownID, rptParam.brandstr, rptParam.citystr, rptParam.itemstr, rptParam.custID, rptParam.usr, rptParam.Modpriv)

                Case "itemGroupSales".ToUpper
                    dsRslt = itemGroupSales(rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.ownID, rptParam.brandstr, rptParam.citystr, rptParam.custID, rptParam.usr, rptParam.Modpriv)

                Case "Discount Report".ToUpper
                    dsRslt = Discount_gv(rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.ownID, rptParam.brandstr, rptParam.citystr, rptParam.custID, rptParam.usr, rptParam.Modpriv)

                Case "Discount Report 2018".ToUpper
                    dsRslt = Discount2018_gv(rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.ownID, rptParam.brandstr, rptParam.citystr, rptParam.custID, rptParam.usr, rptParam.Modpriv)


                '16 mei 2018
                Case "Discount Summary Report".ToUpper
                    dsRslt = DiscountSummary_gv(rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.ownID, rptParam.brandstr, rptParam.citystr, rptParam.custID, rptParam.usr, rptParam.Modpriv)

                 '16 mei 2018
                Case "Payment".ToUpper
                    dsRslt = Payment2(rptParam.paramFrom, rptParam.paramTo, rptParam.outletNo, rptParam.custID, rptParam.usr, rptParam.Modpriv)

                 '16 mei 2018
                Case "Payment Summary".ToUpper
                    dsRslt = PaymentSummary_gv(rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.ownID, rptParam.brandstr, rptParam.citystr, rptParam.custID, rptParam.usr, rptParam.Modpriv)

                Case "Payment Non-sales Summary".ToUpper
                    dsRslt = PaymentSummary_Nonsales_gv(rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.ownID, rptParam.brandstr, rptParam.citystr, rptParam.custID, rptParam.usr, rptParam.Modpriv)


                Case "member".ToUpper
                    dsRslt = member(rptParam.memberID)

                Case "salesItem".ToUpper
                    dsRslt = salesItem(rptParam.paramFrom, rptParam.paramTo, rptParam.outletNo, rptParam.memberID, rptParam.usr, rptParam.Modpriv)

                Case "salesItemByStore".ToUpper
                    dsRslt = salesItemByStore(rptParam.paramFrom, rptParam.paramTo, rptParam.outletNo, rptParam.memberID, rptParam.usr, rptParam.Modpriv)

                Case "salesItemByHour".ToUpper
                    dsRslt = salesItemByHour(rptParam.paramFrom, rptParam.paramTo, rptParam.outletNo, rptParam.memberID, rptParam.usr, rptParam.Modpriv)

                Case "itemStoreChart".ToUpper
                    dsRslt = itemStoreChart(rptParam.paramFrom, rptParam.paramTo, rptParam.outletNo, rptParam.memberID, rptParam.usr, rptParam.Modpriv)

                Case "ItemHourChart".ToUpper
                    dsRslt = ItemHourChart(rptParam.paramFrom, rptParam.paramTo, rptParam.outletNo, rptParam.memberID, rptParam.usr, rptParam.Modpriv)

                Case "topItemQty".ToUpper
                    dsRslt = topItemQty_gv(rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.ownID, rptParam.brandstr, rptParam.citystr, rptParam.custID, rptParam.usr, rptParam.Modpriv)
'tambahan 17Mei2019
                Case "topItemAmt".ToUpper
                    dsRslt = topItemAmt_gv(rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.ownID, rptParam.brandstr, rptParam.citystr, rptParam.custID, rptParam.usr, rptParam.Modpriv)

                Case "topMemberSpend".ToUpper
                    dsRslt = topMemberSpend(rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.ownID, rptParam.brandstr, rptParam.citystr, rptParam.custID, rptParam.usr, rptParam.Modpriv)

                Case "topMemberVisit".ToUpper
                    dsRslt = topMemberVisit(rptParam.paramFrom, rptParam.paramTo, rptParam.outletNo, rptParam.custID, rptParam.usr, rptParam.Modpriv)

                Case "Top Store Revenue".ToUpper
                    dsRslt = topStore_gv(rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.ownID, rptParam.brandstr, rptParam.citystr, rptParam.custID, rptParam.usr, rptParam.Modpriv)
                Case "Top Store Visit".ToUpper
                    dsRslt = topStore_visit_gv(rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.ownID, rptParam.brandstr, rptParam.citystr, rptParam.custID, rptParam.usr, rptParam.Modpriv)


                Case "Top Store Revenue by Hour".ToUpper
                    dsRslt = topStoreByHour_gv(rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.ownID, rptParam.brandstr, rptParam.citystr, rptParam.custID, rptParam.usr, rptParam.Modpriv)

                Case "Top Store Visit by Hour".ToUpper
                    dsRslt = top_visit_StoreByHour_gv(rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.ownID, rptParam.brandstr, rptParam.citystr, rptParam.custID, rptParam.usr, rptParam.Modpriv)

                Case "memberPoint".ToUpper
                    dsRslt = memberPoint(rptParam.paramFrom, rptParam.paramTo, rptParam.outletNo, rptParam.custID, rptParam.usr, rptParam.Modpriv, rptParam.memberNo)

                    'penambahan 15 mei 2017
                    'report untuk perubahan point, 

                Case "memberRewardPoint".ToUpper
                    dsRslt = memberRewardPoint(rptParam.paramFrom, rptParam.paramTo, rptParam.outletNo, rptParam.custID, rptParam.usr, rptParam.Modpriv, rptParam.memberNo)

                    'penambahan 15 mei 2017
                    'report untuk perubahan point, 
                Case "memberpointEdit".ToUpper
                    dsRslt = memberpointEdit(rptParam.paramFrom, rptParam.paramTo, rptParam.custID, rptParam.usr, rptParam.Modpriv, rptParam.memberNo)

                Case "comission".ToUpper
                    dsRslt = comission(rptParam.paramFrom, rptParam.paramTo, rptParam.outletNo, rptParam.custID, rptParam.usr, rptParam.Modpriv)


                'Case "Void Bill".ToUpper
                '    dsRslt = voidBill_gv(rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.ownID, rptParam.brandstr, rptParam.citystr, rptParam.custID, rptParam.usr, rptParam.Modpriv)

                'Case "Void Bill 2018".ToUpper
                '    dsRslt = voidBill2018_gv(rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.ownID, rptParam.brandstr, rptParam.citystr, rptParam.custID, rptParam.usr, rptParam.Modpriv)


                Case "Void Report".ToUpper
                    dsRslt = voidItem_gv(rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.ownID, rptParam.brandstr, rptParam.citystr, rptParam.custID, rptParam.usr, rptParam.Modpriv)

                Case "Void Report 2018".ToUpper
                    dsRslt = voidItem2018_gv(rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.ownID, rptParam.brandstr, rptParam.citystr, rptParam.custID, rptParam.usr, rptParam.Modpriv)

                Case "Non-sales Detail".ToUpper
                    dsRslt = Nonsales_detail_gv(rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.ownID, rptParam.brandstr, rptParam.citystr, rptParam.custID, rptParam.usr, rptParam.Modpriv)

                Case "Non-sales Notes".ToUpper
                    dsRslt = Nonsales_notes_gv(rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.ownID, rptParam.brandstr, rptParam.citystr, rptParam.custID, rptParam.usr, rptParam.Modpriv)


                    'perubahan 27 feb 2017, 'berdasarkan user modpriv
                Case "receiveReturn".ToUpper
                    dsRslt = receiveReturn(rptParam.paramFrom, rptParam.paramTo, rptParam.custID, rptParam.usr, rptParam.Modpriv, rptParam.ccID)

                    'perubahan 27 feb 2017, 'berdasarkan user modpriv
                Case "invTransferFrom".ToUpper
                    dsRslt = invTransferFrom(rptParam.paramFrom, rptParam.paramTo, rptParam.custID, rptParam.usr, rptParam.Modpriv, rptParam.ccID)

                Case "invTransferTo".ToUpper
                    dsRslt = invTransferTo(rptParam.paramFrom, rptParam.paramTo, rptParam.custID, rptParam.usr, rptParam.Modpriv, rptParam.ccID)

                Case "stkOnHand".ToUpper
                    dsRslt = stkOnHand(rptParam.custID, rptParam.usr, rptParam.Modpriv, rptParam.ccID)

                Case "invStockOpname".ToUpper
                    dsRslt = invStockOpname(rptParam.paramFrom, rptParam.paramTo, rptParam.custID, rptParam.usr, rptParam.Modpriv, rptParam.ccID)

                Case "otherExpense".ToUpper
                    dsRslt = otherExpense(rptParam.paramFrom, rptParam.paramTo, rptParam.custID, rptParam.usr, rptParam.Modpriv)

                Case "movementCurrent".ToUpper
                    dsRslt = movementCurrent(rptParam.custID, rptParam.usr, rptParam.Modpriv, rptParam.ccID)

                Case "menuItem".ToUpper
                    dsRslt = menuItem(rptParam.custID)

                Case "revenueVsCost".ToUpper
                    dsRslt = revenueVsCost(rptParam.paramFrom, rptParam.paramTo, rptParam.custID, rptParam.usr, rptParam.Modpriv)

                    'Bram Tambahan
                    '21 april 2017
                    'Receipt Detail Reg CRP
                    'revisi 9 Mei 2017
                Case "rcpdetails".ToUpper
                    dsRslt = Rcptdetils(rptParam.paramFrom, rptParam.paramTo, rptParam.outletNo, rptParam.custID, rptParam.usr, rptParam.Modpriv)
                Case "Receipt Detail".ToUpper
                    dsRslt = Rcptdetils_gv(rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.custID, rptParam.usr, rptParam.Modpriv, rptParam.ownID, rptParam.brandstr)

                Case "Receipt Detail 2018".ToUpper
                    dsRslt = Rcptdetils2018_gv(rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.custID, rptParam.usr, rptParam.Modpriv, rptParam.ownID, rptParam.brandstr)

                    '30 maret 2017
                    'Review/ FeedBack
                Case "Poorreviews".ToUpper
                    dsRslt = PoorReviews(rptParam.paramFrom, rptParam.paramTo, rptParam.outletNo, rptParam.custID, rptParam.usr, rptParam.Modpriv)
                Case "Goodreviews".ToUpper
                    dsRslt = GoodReviews(rptParam.paramFrom, rptParam.paramTo, rptParam.outletNo, rptParam.custID, rptParam.usr, rptParam.Modpriv)
                Case "feedbackDtl".ToUpper
                    dsRslt = feedbackDtl(rptParam.paramFrom, rptParam.paramTo, rptParam.outletNo, rptParam.custID, rptParam.usr, rptParam.Modpriv)
                Case "feedbackSum".ToUpper
                    dsRslt = feedbackSum(rptParam.paramFrom, rptParam.paramTo, rptParam.outletNo, rptParam.custID, rptParam.usr, rptParam.Modpriv)
                Case "feedbackGraph".ToUpper
                    dsRslt = feedbackGraph(rptParam.paramFrom, rptParam.paramTo, rptParam.outletNo, rptParam.memberID, rptParam.custID, rptParam.usr, rptParam.Modpriv)

                    'Deposit
                Case "DepMemberBalance".ToUpper
                    dsRslt = DepMemberBalance(rptParam.custID, rptParam.memberNo)
                Case "DepositSum".ToUpper
                    dsRslt = DepositSum(rptParam.paramFrom, rptParam.paramTo, rptParam.outletNo, rptParam.custID, rptParam.usr, rptParam.Modpriv, rptParam.memberNo)
                Case "DepositDetail".ToUpper
                    dsRslt = DepositDetail(rptParam.paramFrom, rptParam.paramTo, rptParam.outletNo, rptParam.custID, rptParam.usr, rptParam.Modpriv, rptParam.memberNo)
                Case "DepositTopup".ToUpper
                    dsRslt = DepositTopup(rptParam.paramFrom, rptParam.paramTo, rptParam.outletNo, rptParam.custID, rptParam.usr, rptParam.Modpriv, rptParam.memberNo)

                    'inventory Order HO 
                    '20 sept 2017

                Case "order_req".ToUpper
                    dsRslt = orderReq(rptParam.order_req, rptParam.custID, rptParam.ccID)
                    '22 sept 2017
                Case "send_req_order".ToUpper
                    dsRslt = send_req_order(rptParam.deliv_no, rptParam.custID, rptParam.ccID)
                    '25 sept 2017
                Case "stk_req_rpt".ToUpper
                    dsRslt = stk_req_rpt(rptParam.paramFrom, rptParam.paramTo, rptParam.custID, rptParam.outletNo, rptParam.usr, rptParam.Modpriv)
                Case "stk_req_sum_rpt".ToUpper
                    dsRslt = stk_req_rpt(rptParam.paramFrom, rptParam.paramTo, rptParam.custID, rptParam.outletNo, rptParam.usr, rptParam.Modpriv)

                    'Report tambahan bram 19 oct 2017

                Case "productionRpt".ToUpper
                    dsRslt = productionRpt(rptParam.paramFrom, rptParam.paramTo, rptParam.ccID, rptParam.custID, rptParam.usr, rptParam.Modpriv)
                Case "detailproductionRpt".ToUpper
                    dsRslt = detailproductionRpt(rptParam.paramFrom, rptParam.paramTo, rptParam.ccID, rptParam.custID, rptParam.usr, rptParam.Modpriv)

                Case "WasteRpt".ToUpper
                    dsRslt = WasteRpt(rptParam.paramFrom, rptParam.paramTo, rptParam.ccID, rptParam.custID, rptParam.usr, rptParam.Modpriv)

                    'bram 12 Oct 2017
                Case "sumPendPoRptDetail".ToUpper
                    dsRslt = sumPendPoRptDetail(rptParam.paramFrom, rptParam.paramTo, rptParam.ccID, rptParam.custID, rptParam.usr, rptParam.Modpriv)
                    'bram 12 Oct 2017
                Case "sumPendPoRpt".ToUpper
                    dsRslt = sumPendPoRpt(rptParam.paramFrom, rptParam.paramTo, rptParam.ccID, rptParam.custID, rptParam.usr, rptParam.Modpriv)
                    'bram 12 Oct 2017
                Case "InvSupplier".ToUpper
                    dsRslt = InvSupplier(rptParam.custID, rptParam.usr, rptParam.Modpriv)
                    'bram 12 Oct 2017
                Case "InvStock".ToUpper
                    dsRslt = InvStock(rptParam.custID, rptParam.usr, rptParam.Modpriv)

                    'bram 9 nov 2017 minimum stock
                Case "minLimitStkRpt".ToUpper
                    dsRslt = minLimitStkRpt(rptParam.ccID, rptParam.custID, rptParam.usr, rptParam.Modpriv)
                    'bram 9 nov 2017 Invoice Pending
                Case "notinvoicingInvRpt".ToUpper
                    dsRslt = notinvoicingInvRpt(rptParam.paramFrom, rptParam.paramTo, rptParam.ccID, rptParam.custID, rptParam.usr, rptParam.Modpriv)
                    'bram 9 nov 2017 Invoice Finish
                Case "InvReceiveRpt".ToUpper
                    dsRslt = InvReceiveRpt(rptParam.paramFrom, rptParam.paramTo, rptParam.ccID, rptParam.custID, rptParam.usr, rptParam.Modpriv)
                    'bram 9 nov 2017 Master CC
                Case "InvCallcenterRpt".ToUpper
                    dsRslt = InvCallcenterRpt(rptParam.custID, rptParam.usr, rptParam.Modpriv)

                    'erik 11 april 2019 PosvsWeb
                Case "Sales Pos Vs Web".ToUpper
                    dsRslt = PosVSWeb_gv(rptParam.custID, rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.ownID, rptParam.brandstr, rptParam.citystr, rptParam.usr, rptParam.Modpriv)

                Case "Non-sales Pos Vs Web".ToUpper
                    dsRslt = PosVSWeb_nonsales_gv(rptParam.custID, rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.ownID, rptParam.brandstr, rptParam.citystr, rptParam.usr, rptParam.Modpriv)


                Case "Sales By Channel".ToUpper
                    dsRslt = SalesSumBySalesType_gv(rptParam.custID, rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.ownID, rptParam.brandstr, rptParam.citystr, rptParam.usr, rptParam.Modpriv)
                Case "Item Online Orders".ToUpper
                    dsRslt = SelectDataTable_Onlineorders_gv(rptParam.custID, rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.ownID, rptParam.brandstr, rptParam.citystr, rptParam.usr, rptParam.Modpriv)
                       'dari menu EXPORT
                Case "Daily Sales".ToUpper
                    dsRslt = SelectDataTable_DailySales_gv(rptParam.custID, rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.ownID, rptParam.brandstr, rptParam.citystr, rptParam.usr, rptParam.Modpriv)
                Case "Online Delivery Orders".ToUpper
                    dsRslt = Selectdt_onlineorders_gv(rptParam.custID, rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.ownID, rptParam.brandstr, rptParam.citystr, rptParam.usr, rptParam.Modpriv)
                Case "Item Sales Detail".ToUpper
                    dsRslt = SelectDataTable_SlsItem_gv(rptParam.custID, rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.ownID, rptParam.brandstr, rptParam.citystr, rptParam.usr, rptParam.Modpriv)

                Case "Monthly Sales".ToUpper
                    dsRslt = SelectDataTable_MonthlySales_gv(rptParam.custID, rptParam.paramFrom, rptParam.storestr, rptParam.ownID, rptParam.brandstr, rptParam.citystr, rptParam.usr, rptParam.Modpriv)
                Case "Monthly Channel".ToUpper
                    dsRslt = SelectDataTable_Monthly_Channel_gv(rptParam.custID, rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.ownID, rptParam.brandstr, rptParam.citystr, rptParam.usr, rptParam.Modpriv)
                Case "Sales Cover".ToUpper
                    dsRslt = SelectDataTable_Sales_Cover_gv(rptParam.custID, rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.ownID, rptParam.brandstr, rptParam.citystr, rptParam.usr, rptParam.Modpriv)
                Case "Sales Vs Payment".ToUpper
                    dsRslt = SelectDataTable_SalesVSPayment_gv(rptParam.custID, rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.ownID, rptParam.brandstr, rptParam.citystr, rptParam.usr, rptParam.Modpriv)
                Case "Cashier Report".ToUpper
                    dsRslt = LoanReport(rptParam.custID, rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.ownID, rptParam.brandstr, rptParam.citystr, rptParam.usr, rptParam.Modpriv)
                Case "Apps Orders Report".ToUpper
                    dsRslt = Appsorder_gv(rptParam.custID, rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.ownID, rptParam.brandstr, rptParam.citystr, rptParam.usr, rptParam.Modpriv)
                Case "Payment Detail Report".ToUpper
                    dsRslt = Payment_Report_gv(rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.ownID, rptParam.brandstr, rptParam.citystr, rptParam.custID, rptParam.usr, rptParam.Modpriv)
                    'Item Group Sales Report
                Case "Item Group Sales Report".ToUpper
                    dsRslt = SelectDataTable_ItemGroup_gv(rptParam.custID, rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.ownID, rptParam.brandstr, rptParam.citystr, rptParam.usr, rptParam.Modpriv)

                Case "Top 10 Item".ToUpper
                    dsRslt = top10item_gv(rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.ownID, rptParam.brandstr, rptParam.citystr, rptParam.custID, rptParam.usr, rptParam.Modpriv)


                Case "Top 10 Item by Amount".ToUpper
                    dsRslt = topItemAmt_gv(rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.ownID, rptParam.brandstr, rptParam.citystr, rptParam.custID, rptParam.usr, rptParam.Modpriv)
                Case "Top 10 Item by Qty".ToUpper
                    dsRslt = topItemQty_gv(rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.ownID, rptParam.brandstr, rptParam.citystr, rptParam.custID, rptParam.usr, rptParam.Modpriv)

                Case "KDS Report".ToUpper
                    dsRslt = kds_report(rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.ownID, rptParam.brandstr, rptParam.citystr, rptParam.custID, rptParam.usr, rptParam.Modpriv)
                Case "Direct Delivery".ToUpper
                    dsRslt = PATOnlineOrder_gv(rptParam.custID, rptParam.paramFrom, rptParam.paramTo, rptParam.storestr, rptParam.ownID, rptParam.brandstr, rptParam.citystr, rptParam.usr, rptParam.Modpriv)


                    '        'dari menu EXPORT All Store access
                    'Case "Sales_Item".ToUpper
                    '    dsRslt = SelectDataTable_SlsItem(CustID, sOutlet, dFrom, dTo, iOwn, iBrand, iCity)
                    'Case "Monthly_Channel".ToUpper
                    '    dsRslt = SelectDataTable_MonChannel(CustID, sOutlet, dFrom, dTo, iOwn, iBrand, iCity)
                    'Case "Sales_Cover".ToUpper
                    '    dsRslt = SelectDataTable_slsCover(CustID, sOutlet, dFrom, dTo, iOwn, iBrand, iCity)

            End Select

        Catch ex As Exception
            errLog(Err.Description, "ClsReport:reportDataset")
            Err.Clear()
        End Try

        Return dsRslt

    End Function

    Private Function Appsorder(ByVal custID As Integer, ByVal dfrom As Date, ByVal dTo As Date, ByVal sOutlet As String, ByVal iOwn As String, ByVal iBrand As String, ByVal iCity As String, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim dsRslt As New DataSet
        Dim sOut As String = ""
        Dim sBrand As String = ""
        Dim sOwn As String = ""
        Dim sCity As String = ""
        Dim sSlsType As String = ""

        Try
            Dim connDb As New clsDbConn(ServerString)

            If iBrand = "0" Or iBrand = "" Then
                sBrand = ""
            Else
                sBrand = " and o.brandID in (" & iBrand & ")"
            End If

            If iOwn = "0" Or iOwn = "" Then
                sOwn = ""
            Else
                sOwn = " and o.ownID = " & iOwn
            End If
            If iCity = "0" Or iCity = "" Then
                sCity = ""
            Else
                sCity = " and o.cityID in (" & iCity & ")"
            End If

            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If sOutlet = "" Then
                    sOut = " and DailySales.olId in (Select Store From cld_userxstore where user = '" & usr & "')"
                Else
                    sOut = " and DailySales.olnumber in (" & sOutlet & ")"
                End If
            Else
                If sOutlet = "" Then
                    sOut = ""
                Else
                    sOut = " and DailySales.olnumber in (" & sOutlet & ")"
                End If
            End If
            If My.Settings.patSales = "" And My.Settings.dirdeliver = "" Then
                sSlsType = ""
            Else
                sSlsType = " and SlsTypeNm in (" & My.Settings.patSales & "," & My.Settings.dirdeliver & ")"
            End If

            'connDb.resetCmd("SELECT DATE as 'Sales Date', StoreNo AS 'Store Code', StoreName as 'Store Name', PATTRANS as 'PAT Transaction', PATITEM as 'PAT Item', PATSALES as 'PAT Sales', PATAVG as 'PAT Average', AllTrans as 'All Transaction', AllItem as 'All Item', AllSales as 'All Sales', AllAVG as 'All Average', PercenTrans as '% of All Transaction', PercentItem as '% of All Item', PercentSales as '% of All Sales'
            '                FROM
            '                (
            '                SELECT DATE_FORMAT(DailySales.Busidate, '%d %b %Y') AS DATE, DailySales.olnumber 'StoreNo', 
            '                DailySales.olname 'StoreName', IFNULL(( SELECT SUM(QtyTrans) FROM cld_sumsalesdaily 
            '                WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid " & sSlsType & "),0) AS PATTRANS
            '                , IFNULL(( SELECT SUM(Qty) FROM cld_sumsalesdetaildaily 
            '                WHERE BusiDate = DailySales.Busidate AND olId = DailySales.Olid " & sSlsType & "),0) AS PATITEM, 
            '                IFNULL(( SELECT SUM(Subtotal - Discount) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate 
            '                AND cld_sumsalesdaily.olId = DailySales.Olid " & sSlsType & "),0) AS PATSALES
            '                , IFNULL(ROUND(( SELECT SUM(Subtotal - Discount) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate 
            '                AND cld_sumsalesdaily.olId = DailySales.Olid " & sSlsType & ")/( SELECT SUM(QtyTrans) 
            '                FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid  
            '                " & sSlsType & ")),0) AS PATAVG, SUM(QtyTrans) AS AllTrans, IFNULL(( SELECT SUM(Qty) 
            '                FROM cld_sumsalesdetaildaily WHERE BusiDate = DailySales.Busidate AND olId = DailySales.Olid ),0) AS AllItem,
            '                 SUM(Subtotal - Discount) AS AllSales, IFNULL(ROUND(( SELECT SUM(Subtotal - Discount) FROM cld_sumsalesdaily 
            '                 WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid  )/( SELECT SUM(QtyTrans) FROM cld_sumsalesdaily 
            '                 WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid )),0) AS ALLAVG, 
            '                 IFNULL(ROUND(( SELECT SUM(QtyTrans) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate 
            '                 AND cld_sumsalesdaily.olId = DailySales.Olid  " & sSlsType & " )/SUM(QtyTrans) * 100),0) PercenTrans, 
            '                 IFNULL(ROUND(( SELECT SUM(Qty) FROM cld_sumsalesdetaildaily WHERE BusiDate = DailySales.Busidate AND olId = DailySales.Olid  " & sSlsType & ") /( SELECT SUM(Qty) FROM cld_sumsalesdetaildaily WHERE BusiDate = DailySales.Busidate AND olId = DailySales.Olid )  * 100) ,0) PercentItem, IFNULL(ROUND(( SELECT SUM(Subtotal - Discount) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid  " & sSlsType & " )/SUM(Subtotal - Discount) * 100),0) PercentSales FROM cld_sumsalesdaily AS DailySales INNER JOIN cld_outlet AS o ON o.ID = DailySales.olid AND o.custid = DailySales.custid 
            '                 WHERE DailySales.Busidate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' AND DailySales.Busidate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "' " & sOut & sBrand & sOwn & sCity &
            '                " GROUP BY DailySales.Busidate, DailySales.olnumber, DailySales.olname 
            '                 HAVING patsales >0

            '                 UNION ALL

            '                SELECT '' AS DATE, 'Total' AS StoreNo, 
            '                '' AS StoreName, IFNULL(( SELECT SUM(QtyTrans) FROM cld_sumsalesdaily 
            '                WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate " & sSlsType & "),0) AS PATTRANS
            '                , IFNULL(( SELECT SUM(Qty) FROM cld_sumsalesdetaildaily 
            '                WHERE BusiDate = DailySales.Busidate " & sSlsType & "),0) AS PATITEM, 
            '                IFNULL(( SELECT SUM(Subtotal - Discount) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate 
            '                " & sSlsType & "),0) AS PATSALES, FORMAT(IFNULL(ROUND(( SELECT SUM(Subtotal - Discount) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate 
            '                " & sSlsType & ")/( SELECT SUM(QtyTrans) 
            '                FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate   
            '                " & sSlsType & ")),0), 2,'de_DE') AS PATAVG, SUM(QtyTrans) AS AllTrans
            '                , IFNULL(( SELECT SUM(Qty) 
            '                FROM cld_sumsalesdetaildaily WHERE BusiDate = DailySales.Busidate AND
            '                 (olid, Busidate) IN (SELECT olid, Busidate FROM cld_sumsalesdaily WHERE Busidate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' AND Busidate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "' " & sOut & sBrand & sOwn & sCity & sSlsType & ")
            '                ),0) AS AllItem,
            '                 SUM(Subtotal - Discount) AS AllSales, IFNULL(ROUND(( SELECT SUM(Subtotal - Discount) FROM cld_sumsalesdaily 
            '                 WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND
            '                 (olid, Busidate) IN (SELECT olid, Busidate FROM cld_sumsalesdaily WHERE Busidate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' AND Busidate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "' " & sOut & sBrand & sOwn & sCity & sSlsType & ") )/( SELECT SUM(QtyTrans) FROM cld_sumsalesdaily 
            '                 WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND
            '                 (olid, Busidate) IN (SELECT olid, Busidate FROM cld_sumsalesdaily WHERE Busidate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' AND Busidate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "' " & sOut & sBrand & sOwn & sCity & sSlsType & "))),0) AS ALLAVG, 
            '                 IFNULL(ROUND(( SELECT SUM(QtyTrans) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate 
            '                 " & sSlsType & " )/SUM(QtyTrans) * 100),0) PercenTrans, 
            '                 IFNULL(ROUND(( SELECT SUM(Qty) FROM cld_sumsalesdetaildaily WHERE BusiDate = DailySales.Busidate " & sSlsType & ") /( SELECT SUM(Qty) 
            '                 FROM cld_sumsalesdetaildaily WHERE BusiDate = DailySales.Busidate AND
            '                 (olid, Busidate) IN (SELECT olid, Busidate FROM cld_sumsalesdaily WHERE Busidate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' AND Busidate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "' " & sOut & sBrand & sOwn & sCity & sSlsType & "))  * 100) ,0) PercentItem
            '                 , IFNULL(ROUND(( SELECT SUM(Subtotal - Discount) 
            '                 FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate " & sSlsType & " )/SUM(Subtotal - Discount) * 100),0) PercentSales 
            '                 FROM cld_sumsalesdaily AS DailySales WHERE DailySales.Busidate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' AND DailySales.Busidate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "' " & sOut & sBrand & sOwn & sCity & " AND
            '                 (olid, Busidate) IN (SELECT olid, Busidate FROM cld_sumsalesdaily WHERE Busidate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' AND Busidate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "' " & sOut & sBrand & sOwn & sCity & sSlsType & ")

            '                 ) t")


            'connDb.resetCmd("SELECT DATE_FORMAT(DailySales.Busidate, '%d %b %Y') 'Sales Date', DailySales.olnumber 'Store Code', DailySales.olname 'Store Name', " &
            '"IFNULL(( SELECT SUM(QtyTrans) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid " & sSlsType & "),0) 'PAT Transaction', " &
            '"IFNULL(( SELECT SUM(Qty) FROM cld_sumsalesdetaildaily WHERE BusiDate = DailySales.Busidate AND olId = DailySales.Olid " & sSlsType & "),0) 'PAT Item', " &
            '"IFNULL(( SELECT SUM(Subtotal - Discount) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid " & sSlsType & "),0) AS PATSALES, " &
            '"IFNULL(ROUND(( SELECT SUM(Subtotal - Discount) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid " & sSlsType & ")/( SELECT SUM(QtyTrans) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid " & sSlsType & ")),0) AS PATAVG, " &
            '"SUM(QtyTrans) 'All Transaction', " &
            '"IFNULL(( SELECT SUM(Qty) FROM cld_sumsalesdetaildaily WHERE BusiDate = DailySales.Busidate AND olId = DailySales.Olid ),0) 'All Item', " &
            '"SUM(Subtotal - Discount) AS AllSales, " &
            '"IFNULL(ROUND(( SELECT SUM(Subtotal - Discount) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid  )/( SELECT SUM(QtyTrans) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid )),0) AS ALLAVG, " &
            '"IFNULL(ROUND(( SELECT SUM(QtyTrans) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid " & sSlsType & " )/SUM(QtyTrans) * 100),0) PercenTrans, " &
            '"IFNULL(ROUND(( SELECT SUM(Qty) FROM cld_sumsalesdetaildaily WHERE BusiDate = DailySales.Busidate AND olId = DailySales.Olid " & sSlsType & ") /( SELECT SUM(Qty) FROM cld_sumsalesdetaildaily WHERE BusiDate = DailySales.Busidate AND olId = DailySales.Olid )  * 100) ,0) PercentItem, " &
            '"IFNULL(ROUND(( SELECT SUM(Subtotal - Discount) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid " & sSlsType & " )/SUM(Subtotal - Discount) * 100),0) PercentSales " &
            '"FROM cld_sumsalesdaily AS DailySales INNER JOIN cld_outlet AS o ON o.ID = DailySales.olid AND o.custid = DailySales.custid " &
            '"WHERE DailySales.Busidate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' AND DailySales.Busidate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "' " & sOut & sBrand & sOwn & sCity &
            '" GROUP BY DailySales.Busidate, DailySales.olnumber, DailySales.olname having patsales>0 ")

            connDb.resetCmd("SELECT DATE as 'Sales Date',olnumber 'Store Code',olname 'Store Name',appstrans 'Apps Transaction',appsitem 'Apps Item',appssales 'Apps Sales',appsavg 'Apps Avg',alltrans 'All Transaction',allitem 'All Item',allsales 'All Sales',allavg 'All Avg', PercenTrans as '% of All Transaction', PercentItem as '% of All Item', PercentSales as '% of All Sales' FROM
            (SELECT DATE_FORMAT(DailySales.Busidate, '%d-%b-%Y') date, DailySales.olnumber , DailySales.olname , " &
            "IFNULL(( SELECT SUM(QtyTrans) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid " & sSlsType & "),0) appstrans, " &
            "IFNULL(( SELECT SUM(Qty) FROM cld_sumsalesdetaildaily WHERE BusiDate = DailySales.Busidate AND olId = DailySales.Olid " & sSlsType & "),0) appsitem, " &
            "IFNULL(( SELECT SUM(Subtotal - Discount) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid " & sSlsType & "),0) AS appsSALES, " &
            "IFNULL(ROUND(( SELECT SUM(Subtotal - Discount) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid " & sSlsType & ")/( SELECT SUM(QtyTrans) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid " & sSlsType & ")),0) AS appsAVG, " &
            "SUM(QtyTrans) alltrans, " &
            "IFNULL(( SELECT SUM(Qty) FROM cld_sumsalesdetaildaily WHERE BusiDate = DailySales.Busidate AND olId = DailySales.Olid ),0) allitem, " &
            "SUM(Subtotal - Discount) AllSales, " &
            "IFNULL(ROUND(( SELECT SUM(Subtotal - Discount) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid  )/( SELECT SUM(QtyTrans) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid )),0) AS ALLAVG, " &
            "IFNULL(ROUND(( SELECT SUM(QtyTrans) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid " & sSlsType & " )/SUM(QtyTrans) * 100,2),0) PercenTrans, " &
            "IFNULL(ROUND(( SELECT SUM(Qty) FROM cld_sumsalesdetaildaily WHERE BusiDate = DailySales.Busidate AND olId = DailySales.Olid " & sSlsType & ") /( SELECT SUM(Qty) FROM cld_sumsalesdetaildaily WHERE BusiDate = DailySales.Busidate AND olId = DailySales.Olid )  * 100,2) ,0) PercentItem, " &
            "IFNULL(ROUND(( SELECT SUM(Subtotal - Discount) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid " & sSlsType & " )/SUM(Subtotal - Discount) * 100,2),0) PercentSales " &
            "FROM cld_sumsalesdaily AS DailySales INNER JOIN cld_outlet AS o ON o.ID = DailySales.olid AND o.custid = DailySales.custid " &
            "WHERE DailySales.Busidate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' AND DailySales.Busidate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "' " & sOut & sBrand & sOwn & sCity &
            " GROUP BY DailySales.Busidate, DailySales.olnumber, DailySales.olname having appssales>0 ) a")

            'union all

            'SELECT '','','Total',SUM(pattrans),SUM(patitem),SUM(patsales),SUM(patavg),SUM(alltrans),SUM(allitem),SUM(allsales),SUM(allavg),SUM(pattrans)*100/SUM(alltrans),SUM(patitem)*100/SUM(allitem),SUM(patsales)*100/SUM(allsales) FROM
            '(SELECT DATE_FORMAT(DailySales.Busidate, '%d %b %Y') date, DailySales.olnumber , DailySales.olname , " &
            '"IFNULL(( SELECT SUM(QtyTrans) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid " & sSlsType & "),0) pattrans, " &
            '"IFNULL(( SELECT SUM(Qty) FROM cld_sumsalesdetaildaily WHERE BusiDate = DailySales.Busidate AND olId = DailySales.Olid " & sSlsType & "),0) patitem, " &
            '"IFNULL(( SELECT SUM(Subtotal - Discount) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid " & sSlsType & "),0) AS PATSALES, " &
            '"IFNULL(ROUND(( SELECT SUM(Subtotal - Discount) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid " & sSlsType & ")/( SELECT SUM(QtyTrans) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid " & sSlsType & ")),0) AS PATAVG, " &
            '"SUM(QtyTrans) alltrans, " &
            '"IFNULL(( SELECT SUM(Qty) FROM cld_sumsalesdetaildaily WHERE BusiDate = DailySales.Busidate AND olId = DailySales.Olid ),0) allitem, " &
            '"SUM(Subtotal - Discount) AllSales, " &
            '"IFNULL(ROUND(( SELECT SUM(Subtotal - Discount) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid  )/( SELECT SUM(QtyTrans) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid )),0) AS ALLAVG, " &
            '"IFNULL(ROUND(( SELECT SUM(QtyTrans) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid " & sSlsType & " )/SUM(QtyTrans) * 100),0) PercenTrans, " &
            '"IFNULL(ROUND(( SELECT SUM(Qty) FROM cld_sumsalesdetaildaily WHERE BusiDate = DailySales.Busidate AND olId = DailySales.Olid " & sSlsType & ") /( SELECT SUM(Qty) FROM cld_sumsalesdetaildaily WHERE BusiDate = DailySales.Busidate AND olId = DailySales.Olid )  * 100) ,0) PercentItem, " &
            '"IFNULL(ROUND(( SELECT SUM(Subtotal - Discount) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid " & sSlsType & " )/SUM(Subtotal - Discount) * 100),0) PercentSales " &
            '"FROM cld_sumsalesdaily AS DailySales INNER JOIN cld_outlet AS o ON o.ID = DailySales.olid AND o.custid = DailySales.custid " &
            '"WHERE DailySales.Busidate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' AND DailySales.Busidate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "' " & sOut & sBrand & sOwn & sCity &
            '" GROUP BY DailySales.Busidate, DailySales.olnumber, DailySales.olname having patsales>0 ) b

            '")

            connDb.addParam(dfrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(dTo, "@paramTo", MySqlDbType.DateTime)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)
            ' connDb.addParam(MemberID, "@memberID", MySqlDbType.Int32)


            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:PayAtTable")
            Err.Clear()
        End Try

        Return dsRslt

    End Function
    Private Function Appsorder_gv(ByVal custID As Integer, ByVal dfrom As Date, ByVal dTo As Date, ByVal sOutlet As String, ByVal iOwn As String, ByVal iBrand As String, ByVal iCity As String, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim dsRslt As New DataSet
        Dim sOut As String = ""
        Dim sBrand As String = ""
        Dim sOwn As String = ""
        Dim sCity As String = ""
        Dim sSlsType As String = ""

        Try
            Dim connDb As New clsDbConn(ServerString)

            If iBrand = "0" Or iBrand = "" Then
                sBrand = ""
            Else
                sBrand = " and o.brandID in (" & iBrand & ")"
            End If

            If iOwn = "0" Or iOwn = "" Then
                sOwn = ""
            Else
                sOwn = " and o.ownID = " & iOwn
            End If
            If iCity = "0" Or iCity = "" Then
                sCity = ""
            Else
                sCity = " and o.cityID in (" & iCity & ")"
            End If

            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If sOutlet = "" Then
                    sOut = " and DailySales.olId in (Select Store From cld_userxstore where user = '" & usr & "')"
                Else
                    sOut = " and DailySales.olnumber in (" & sOutlet & ")"
                End If
            Else
                If sOutlet = "" Then
                    sOut = ""
                Else
                    sOut = " and DailySales.olnumber in (" & sOutlet & ")"
                End If
            End If
            If My.Settings.patSales = "" And My.Settings.dirdeliver = "" Then
                sSlsType = ""
            Else
                sSlsType = " and SlsTypeNm in (" & My.Settings.patSales & "," & My.Settings.dirdeliver & ")"
            End If
            connDb.resetCmd("SELECT DATE as 'Sales Date',olnumber 'Store Code',olname 'Store Name',appstrans 'Apps Transaction',appsitem 'Apps Item',appssales 'Apps Sales',appsavg 'Apps Avg',alltrans 'All Transaction',allitem 'All Item',allsales 'All Sales',allavg 'All Avg', PercenTrans as '% of All Transaction', PercentItem as '% of All Item', PercentSales as '% of All Sales' FROM
            (SELECT DATE_FORMAT(DailySales.Busidate, '%d-%b-%Y') date, DailySales.olnumber , DailySales.olname , " &
            "IFNULL(( SELECT SUM(QtyTrans) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid " & sSlsType & "),0) appstrans, " &
            "IFNULL(( SELECT SUM(Qty) FROM cld_sumsalesdetaildaily WHERE BusiDate = DailySales.Busidate AND olId = DailySales.Olid " & sSlsType & "),0) Appsitem, " &
            "FORMAT(IFNULL(( SELECT SUM(Subtotal - Discount) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid " & sSlsType & "),0), 2,'de_DE') AS appsSALES, " &
            "FORMAT(IFNULL(ROUND(( SELECT SUM(Subtotal - Discount) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid " & sSlsType & ")/( SELECT SUM(QtyTrans) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid " & sSlsType & ")),0), 2,'de_DE') AS appsAVG, " &
            "SUM(QtyTrans) alltrans, " &
            "IFNULL(( SELECT SUM(Qty) FROM cld_sumsalesdetaildaily WHERE BusiDate = DailySales.Busidate AND olId = DailySales.Olid ),0) allitem, " &
            "FORMAT(SUM(Subtotal - Discount), 2,'de_DE') AllSales, " &
            "FORMAT(IFNULL(ROUND(( SELECT SUM(Subtotal - Discount) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid  )/( SELECT SUM(QtyTrans) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid )),0), 2,'de_DE') AS ALLAVG, " &
            "FORMAT(IFNULL(ROUND(( SELECT SUM(QtyTrans) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid " & sSlsType & " )/SUM(QtyTrans) * 100, 2),0), 2,'de_DE') PercenTrans, " &
            "FORMAT(IFNULL(ROUND(( SELECT SUM(Qty) FROM cld_sumsalesdetaildaily WHERE BusiDate = DailySales.Busidate AND olId = DailySales.Olid " & sSlsType & ") /( SELECT SUM(Qty) FROM cld_sumsalesdetaildaily WHERE BusiDate = DailySales.Busidate AND olId = DailySales.Olid )  * 100, 2) ,0), 2,'de_DE') PercentItem, " &
            "FORMAT(IFNULL(ROUND(( SELECT SUM(Subtotal - Discount) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid " & sSlsType & " )/SUM(Subtotal - Discount) * 100,2),0), 2,'de_DE') PercentSales " &
            "FROM cld_sumsalesdaily AS DailySales INNER JOIN cld_outlet AS o ON o.ID = DailySales.olid AND o.custid = DailySales.custid " &
            "WHERE DailySales.Busidate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' AND DailySales.Busidate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "' " & sOut & sBrand & sOwn & sCity &
            " GROUP BY DailySales.Busidate, DailySales.olnumber, DailySales.olname having appssales>0 ) a
            
            union all

            SELECT '','','Total',FORMAT(SUM(appstrans), 0,'de_DE'),FORMAT(SUM(appsitem), 0,'de_DE'),FORMAT(SUM(appssales), 2,'de_DE'),FORMAT(SUM(appsavg), 2,'de_DE'),FORMAT(SUM(alltrans), 0,'de_DE'),FORMAT(SUM(allitem), 0,'de_DE'),FORMAT(SUM(allsales), 2,'de_DE'),FORMAT(SUM(allavg), 2,'de_DE'),FORMAT(SUM(appstrans)*100/SUM(alltrans), 2,'de_DE'),FORMAT(SUM(appsitem)*100/SUM(allitem), 2,'de_DE'),FORMAT(SUM(appssales)*100/SUM(allsales), 2,'de_DE') FROM
            (SELECT DATE_FORMAT(DailySales.Busidate, '%d %b %Y') date, DailySales.olnumber , DailySales.olname , " &
            "IFNULL(( SELECT SUM(QtyTrans) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid " & sSlsType & "),0) appstrans, " &
            "IFNULL(( SELECT SUM(Qty) FROM cld_sumsalesdetaildaily WHERE BusiDate = DailySales.Busidate AND olId = DailySales.Olid " & sSlsType & "),0) appsitem, " &
            "IFNULL(( SELECT SUM(Subtotal - Discount) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid " & sSlsType & "),0) AS appsSALES, " &
            "IFNULL(ROUND(( SELECT SUM(Subtotal - Discount) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid " & sSlsType & ")/( SELECT SUM(QtyTrans) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid " & sSlsType & ")),0) AS appsAVG, " &
            "SUM(QtyTrans) alltrans, " &
            "IFNULL(( SELECT SUM(Qty) FROM cld_sumsalesdetaildaily WHERE BusiDate = DailySales.Busidate AND olId = DailySales.Olid ),0) allitem, " &
            "SUM(Subtotal - Discount) AllSales, " &
            "IFNULL(ROUND(( SELECT SUM(Subtotal - Discount) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid  )/( SELECT SUM(QtyTrans) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid )),0) AS ALLAVG, " &
            "IFNULL(ROUND(( SELECT SUM(QtyTrans) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid " & sSlsType & " )/SUM(QtyTrans) * 100,2),0) PercenTrans, " &
            "IFNULL(ROUND(( SELECT SUM(Qty) FROM cld_sumsalesdetaildaily WHERE BusiDate = DailySales.Busidate AND olId = DailySales.Olid " & sSlsType & ") /( SELECT SUM(Qty) FROM cld_sumsalesdetaildaily WHERE BusiDate = DailySales.Busidate AND olId = DailySales.Olid )  * 100,2) ,0) PercentItem, " &
            "IFNULL(ROUND(( SELECT SUM(Subtotal - Discount) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid " & sSlsType & " )/SUM(Subtotal - Discount) * 100,2),0) PercentSales " &
            "FROM cld_sumsalesdaily AS DailySales INNER JOIN cld_outlet AS o ON o.ID = DailySales.olid AND o.custid = DailySales.custid " &
            "WHERE DailySales.Busidate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' AND DailySales.Busidate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "' " & sOut & sBrand & sOwn & sCity &
            " GROUP BY DailySales.Busidate, DailySales.olnumber, DailySales.olname having appssales>0 ) b
            
            ")



            'connDb.resetCmd("SELECT DATE_FORMAT(DailySales.Busidate, '%d %b %Y') 'Sales Date', DailySales.olnumber 'Store Code', DailySales.olname 'Store Name', " &
            '"IFNULL(( SELECT SUM(QtyTrans) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid " & sSlsType & "),0) 'PAT Transaction', " &
            '"IFNULL(( SELECT SUM(Qty) FROM cld_sumsalesdetaildaily WHERE BusiDate = DailySales.Busidate AND olId = DailySales.Olid " & sSlsType & "),0) 'PAT Item', " &
            '"FORMAT(IFNULL(( SELECT SUM(Subtotal - Discount) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid " & sSlsType & "),0), 2,'de_DE') AS PATSALES, " &
            '"FORMAT(IFNULL(ROUND(( SELECT SUM(Subtotal - Discount) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid " & sSlsType & ")/( SELECT SUM(QtyTrans) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid " & sSlsType & ")),0), 2,'de_DE') AS PATAVG, " &
            '"SUM(QtyTrans) 'All Transaction', " &
            '"IFNULL(( SELECT SUM(Qty) FROM cld_sumsalesdetaildaily WHERE BusiDate = DailySales.Busidate AND olId = DailySales.Olid ),0) 'All Item', " &
            '"FORMAT(SUM(Subtotal - Discount), 2,'de_DE') AS AllSales, " &
            '"FORMAT(IFNULL(ROUND(( SELECT SUM(Subtotal - Discount) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid  )/( SELECT SUM(QtyTrans) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid )),0), 2,'de_DE') AS ALLAVG, " &
            '"IFNULL(ROUND(( SELECT SUM(QtyTrans) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid " & sSlsType & " )/SUM(QtyTrans) * 100),0) PercenTrans, " &
            '"IFNULL(ROUND(( SELECT SUM(Qty) FROM cld_sumsalesdetaildaily WHERE BusiDate = DailySales.Busidate AND olId = DailySales.Olid " & sSlsType & ") /( SELECT SUM(Qty) FROM cld_sumsalesdetaildaily WHERE BusiDate = DailySales.Busidate AND olId = DailySales.Olid )  * 100) ,0) PercentItem, " &
            '"IFNULL(ROUND(( SELECT SUM(Subtotal - Discount) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid " & sSlsType & " )/SUM(Subtotal - Discount) * 100),0) PercentSales " &
            '"FROM cld_sumsalesdaily AS DailySales INNER JOIN cld_outlet AS o ON o.ID = DailySales.olid AND o.custid = DailySales.custid " &
            '"WHERE DailySales.Busidate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' AND DailySales.Busidate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "' " & sOut & sBrand & sOwn & sCity &
            '" GROUP BY DailySales.Busidate, DailySales.olnumber, DailySales.olname having patsales>0 ")

            'connDb.resetCmd("SELECT DATE as 'Sales Date', StoreNo AS 'Store Code', StoreName as 'Store Name', FORMAT(PATTRANS, 0,'de_DE') as 'PAT Transaction', FORMAT(PATITEM, 0,'de_DE') as 'PAT Item', FORMAT(PATSALES, 2,'de_DE') as 'PAT Sales', FORMAT(PATAVG, 2,'de_DE') as 'PAT Average', FORMAT(AllTrans, 0,'de_DE') as 'All Transaction', FORMAT(AllItem, 0,'de_DE') as 'All Item', FORMAT(AllSales, 2,'de_DE') as 'All Sales', FORMAT(AllAVG, 2,'de_DE') as 'All Average', PercenTrans as '% of All Transaction', PercentItem as '% of All Item', PercentSales as '% of All Sales'
            '                FROM
            '                (
            '                SELECT DATE_FORMAT(DailySales.Busidate, '%d %b %Y') AS DATE, DailySales.olnumber 'StoreNo', 
            '                DailySales.olname 'StoreName', IFNULL(( SELECT SUM(QtyTrans) FROM cld_sumsalesdaily 
            '                WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid " & sSlsType & "),0) AS PATTRANS
            '                , IFNULL(( SELECT SUM(Qty) FROM cld_sumsalesdetaildaily 
            '                WHERE BusiDate = DailySales.Busidate AND olId = DailySales.Olid " & sSlsType & "),0) AS PATITEM, 
            '                IFNULL(( SELECT SUM(Subtotal - Discount) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate 
            '                AND cld_sumsalesdaily.olId = DailySales.Olid " & sSlsType & "),0) AS PATSALES
            '                , IFNULL(ROUND(( SELECT SUM(Subtotal - Discount) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate 
            '                AND cld_sumsalesdaily.olId = DailySales.Olid " & sSlsType & ")/( SELECT SUM(QtyTrans) 
            '                FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid  
            '                " & sSlsType & ")),0) AS PATAVG, SUM(QtyTrans) AS AllTrans, IFNULL(( SELECT SUM(Qty) 
            '                FROM cld_sumsalesdetaildaily WHERE BusiDate = DailySales.Busidate AND olId = DailySales.Olid ),0) AS AllItem,
            '                 SUM(Subtotal - Discount) AS AllSales, IFNULL(ROUND(( SELECT SUM(Subtotal - Discount) FROM cld_sumsalesdaily 
            '                 WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid  )/( SELECT SUM(QtyTrans) FROM cld_sumsalesdaily 
            '                 WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid )),0) AS ALLAVG, 
            '                 IFNULL(ROUND(( SELECT SUM(QtyTrans) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate 
            '                 AND cld_sumsalesdaily.olId = DailySales.Olid  " & sSlsType & " )/SUM(QtyTrans) * 100),0) PercenTrans, 
            '                 IFNULL(ROUND(( SELECT SUM(Qty) FROM cld_sumsalesdetaildaily WHERE BusiDate = DailySales.Busidate AND olId = DailySales.Olid  " & sSlsType & ") /( SELECT SUM(Qty) FROM cld_sumsalesdetaildaily WHERE BusiDate = DailySales.Busidate AND olId = DailySales.Olid )  * 100) ,0) PercentItem, IFNULL(ROUND(( SELECT SUM(Subtotal - Discount) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid  " & sSlsType & " )/SUM(Subtotal - Discount) * 100),0) PercentSales FROM cld_sumsalesdaily AS DailySales INNER JOIN cld_outlet AS o ON o.ID = DailySales.olid AND o.custid = DailySales.custid 
            '                 WHERE DailySales.Busidate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' AND DailySales.Busidate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "' " & sOut & sBrand & sOwn & sCity &
            '                " GROUP BY DailySales.Busidate, DailySales.olnumber, DailySales.olname 
            '                 HAVING patsales >0

            '                 UNION ALL

            '                SELECT '' AS DATE, 'Total' AS StoreNo, 
            '                '' AS StoreName, IFNULL(( SELECT SUM(QtyTrans) FROM cld_sumsalesdaily 
            '                WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate " & sSlsType & "),0) AS PATTRANS
            '                , IFNULL(( SELECT SUM(Qty) FROM cld_sumsalesdetaildaily 
            '                WHERE BusiDate = DailySales.Busidate " & sSlsType & "),0) AS PATITEM, 
            '                IFNULL(( SELECT SUM(Subtotal - Discount) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate 
            '                " & sSlsType & "),0) AS PATSALES, FORMAT(IFNULL(ROUND(( SELECT SUM(Subtotal - Discount) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate 
            '                " & sSlsType & ")/( SELECT SUM(QtyTrans) 
            '                FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate   
            '                " & sSlsType & ")),0), 2,'de_DE') AS PATAVG, SUM(QtyTrans) AS AllTrans
            '                , IFNULL(( SELECT SUM(Qty) 
            '                FROM cld_sumsalesdetaildaily WHERE BusiDate = DailySales.Busidate AND
            '                 (olid, Busidate) IN (SELECT olid, Busidate FROM cld_sumsalesdaily WHERE Busidate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' AND Busidate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "' " & sOut & sBrand & sOwn & sCity & sSlsType & ")
            '                ),0) AS AllItem,
            '                 SUM(Subtotal - Discount) AS AllSales, IFNULL(ROUND(( SELECT SUM(Subtotal - Discount) FROM cld_sumsalesdaily 
            '                 WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND
            '                 (olid, Busidate) IN (SELECT olid, Busidate FROM cld_sumsalesdaily WHERE Busidate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' AND Busidate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "' " & sOut & sBrand & sOwn & sCity & sSlsType & ") )/( SELECT SUM(QtyTrans) FROM cld_sumsalesdaily 
            '                 WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND
            '                 (olid, Busidate) IN (SELECT olid, Busidate FROM cld_sumsalesdaily WHERE Busidate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' AND Busidate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "' " & sOut & sBrand & sOwn & sCity & sSlsType & "))),0) AS ALLAVG, 
            '                 IFNULL(ROUND(( SELECT SUM(QtyTrans) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate 
            '                 " & sSlsType & " )/SUM(QtyTrans) * 100),0) PercenTrans, 
            '                 IFNULL(ROUND(( SELECT SUM(Qty) FROM cld_sumsalesdetaildaily WHERE BusiDate = DailySales.Busidate " & sSlsType & ") /( SELECT SUM(Qty) 
            '                 FROM cld_sumsalesdetaildaily WHERE BusiDate = DailySales.Busidate AND
            '                 (olid, Busidate) IN (SELECT olid, Busidate FROM cld_sumsalesdaily WHERE Busidate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' AND Busidate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "' " & sOut & sBrand & sOwn & sCity & sSlsType & "))  * 100) ,0) PercentItem
            '                 , IFNULL(ROUND(( SELECT SUM(Subtotal - Discount) 
            '                 FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate " & sSlsType & " )/SUM(Subtotal - Discount) * 100),0) PercentSales 
            '                 FROM cld_sumsalesdaily AS DailySales WHERE DailySales.Busidate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' AND DailySales.Busidate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "' " & sOut & sBrand & sOwn & sCity & " AND
            '                 (olid, Busidate) IN (SELECT olid, Busidate FROM cld_sumsalesdaily WHERE Busidate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' AND Busidate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "' " & sOut & sBrand & sOwn & sCity & sSlsType & ")

            '                 ) t")


            connDb.addParam(dfrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(dTo, "@paramTo", MySqlDbType.DateTime)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)
            ' connDb.addParam(MemberID, "@memberID", MySqlDbType.Int32)


            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:PayAtTable")
            Err.Clear()
        End Try

        Return dsRslt

    End Function

    Private Function PATOnlineOrder(ByVal custID As Integer, ByVal dfrom As Date, ByVal dTo As Date, ByVal sOutlet As String, ByVal iOwn As String, ByVal iBrand As String, ByVal iCity As String, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim dsRslt As New DataSet
        Dim sOut As String = ""
        Dim sBrand As String = ""
        Dim sOwn As String = ""
        Dim sCity As String = ""
        Dim sSlsType As String = ""

        Try
            Dim connDb As New clsDbConn(ServerString)

            If iBrand = "0" Or iBrand = "" Then
                sBrand = ""
            Else
                sBrand = " and o.brandID in (" & iBrand & ")"
            End If

            If iOwn = "0" Or iOwn = "" Then
                sOwn = ""
            Else
                sOwn = " and o.ownID = " & iOwn
            End If
            If iCity = "0" Or iCity = "" Then
                sCity = ""
            Else
                sCity = " and o.cityID in (" & iCity & ")"
            End If

            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If sOutlet = "" Then
                    sOut = " and DailySales.olId in (Select Store From cld_userxstore where user = '" & usr & "')"
                Else
                    sOut = " and DailySales.olnumber in (" & sOutlet & ")"
                End If
            Else
                If sOutlet = "" Then
                    sOut = ""
                Else
                    sOut = " and DailySales.olnumber in (" & sOutlet & ")"
                End If
            End If
            If My.Settings.patSales = "" Then
                sSlsType = ""
            Else
                sSlsType = " and SlsTypeNm in (" & My.Settings.dirdeliver & ")"
            End If

            connDb.resetCmd("SELECT DATE as 'Sales Date',olnumber 'Store Code',olname 'Store Name',pattrans 'Direct Delivery Transaction',patitem 'Direct Delivery Item',patsales 'Direct Delivery Sales',patavg 'Direct Delivery Avg',alltrans 'All Transaction',allitem 'All Item',allsales 'All Sales',allavg 'All Avg', PercenTrans as '% of All Transaction', PercentItem as '% of All Item', PercentSales as '% of All Sales' FROM
            (SELECT DATE_FORMAT(DailySales.Busidate, '%d %b %Y') date, DailySales.olnumber , DailySales.olname , " &
            "IFNULL(( SELECT SUM(QtyTrans) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid " & sSlsType & "),0) pattrans, " &
            "IFNULL(( SELECT SUM(Qty) FROM cld_sumsalesdetaildaily WHERE BusiDate = DailySales.Busidate AND olId = DailySales.Olid " & sSlsType & "),0) patitem, " &
            "IFNULL(( SELECT SUM(Subtotal - Discount) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid " & sSlsType & "),0) AS PATSALES, " &
            "IFNULL(ROUND(( SELECT SUM(Subtotal - Discount) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid " & sSlsType & ")/( SELECT SUM(QtyTrans) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid " & sSlsType & ")),0) AS PATAVG, " &
            "SUM(QtyTrans) alltrans, " &
            "IFNULL(( SELECT SUM(Qty) FROM cld_sumsalesdetaildaily WHERE BusiDate = DailySales.Busidate AND olId = DailySales.Olid ),0) allitem, " &
            "SUM(Subtotal - Discount) AllSales, " &
            "IFNULL(ROUND(( SELECT SUM(Subtotal - Discount) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid  )/( SELECT SUM(QtyTrans) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid )),0) AS ALLAVG, " &
            "IFNULL(ROUND(( SELECT SUM(QtyTrans) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid " & sSlsType & " )/SUM(QtyTrans) * 100),0) PercenTrans, " &
            "IFNULL(ROUND(( SELECT SUM(Qty) FROM cld_sumsalesdetaildaily WHERE BusiDate = DailySales.Busidate AND olId = DailySales.Olid " & sSlsType & ") /( SELECT SUM(Qty) FROM cld_sumsalesdetaildaily WHERE BusiDate = DailySales.Busidate AND olId = DailySales.Olid )  * 100) ,0) PercentItem, " &
            "IFNULL(ROUND(( SELECT SUM(Subtotal - Discount) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid " & sSlsType & " )/SUM(Subtotal - Discount) * 100),0) PercentSales " &
            "FROM cld_sumsalesdaily AS DailySales INNER JOIN cld_outlet AS o ON o.ID = DailySales.olid AND o.custid = DailySales.custid " &
            "WHERE DailySales.Busidate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' AND DailySales.Busidate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "' " & sOut & sBrand & sOwn & sCity &
            " GROUP BY DailySales.Busidate, DailySales.olnumber, DailySales.olname having patsales>0 ) a
            
            union all

            SELECT '','','Total',SUM(pattrans),SUM(patitem),SUM(patsales),SUM(patavg),SUM(alltrans),SUM(allitem),SUM(allsales),SUM(allavg),SUM(pattrans)*100/SUM(alltrans),SUM(patitem)*100/SUM(allitem),SUM(patsales)*100/SUM(allsales) FROM
            (SELECT DATE_FORMAT(DailySales.Busidate, '%d %b %Y') date, DailySales.olnumber , DailySales.olname , " &
            "IFNULL(( SELECT SUM(QtyTrans) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid " & sSlsType & "),0) pattrans, " &
            "IFNULL(( SELECT SUM(Qty) FROM cld_sumsalesdetaildaily WHERE BusiDate = DailySales.Busidate AND olId = DailySales.Olid " & sSlsType & "),0) patitem, " &
            "IFNULL(( SELECT SUM(Subtotal - Discount) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid " & sSlsType & "),0) AS PATSALES, " &
            "IFNULL(ROUND(( SELECT SUM(Subtotal - Discount) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid " & sSlsType & ")/( SELECT SUM(QtyTrans) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid " & sSlsType & ")),0) AS PATAVG, " &
            "SUM(QtyTrans) alltrans, " &
            "IFNULL(( SELECT SUM(Qty) FROM cld_sumsalesdetaildaily WHERE BusiDate = DailySales.Busidate AND olId = DailySales.Olid ),0) allitem, " &
            "SUM(Subtotal - Discount) AllSales, " &
            "IFNULL(ROUND(( SELECT SUM(Subtotal - Discount) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid  )/( SELECT SUM(QtyTrans) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid )),0) AS ALLAVG, " &
            "IFNULL(ROUND(( SELECT SUM(QtyTrans) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid " & sSlsType & " )/SUM(QtyTrans) * 100),0) PercenTrans, " &
            "IFNULL(ROUND(( SELECT SUM(Qty) FROM cld_sumsalesdetaildaily WHERE BusiDate = DailySales.Busidate AND olId = DailySales.Olid " & sSlsType & ") /( SELECT SUM(Qty) FROM cld_sumsalesdetaildaily WHERE BusiDate = DailySales.Busidate AND olId = DailySales.Olid )  * 100) ,0) PercentItem, " &
            "IFNULL(ROUND(( SELECT SUM(Subtotal - Discount) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid " & sSlsType & " )/SUM(Subtotal - Discount) * 100),0) PercentSales " &
            "FROM cld_sumsalesdaily AS DailySales INNER JOIN cld_outlet AS o ON o.ID = DailySales.olid AND o.custid = DailySales.custid " &
            "WHERE DailySales.Busidate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' AND DailySales.Busidate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "' " & sOut & sBrand & sOwn & sCity &
            " GROUP BY DailySales.Busidate, DailySales.olnumber, DailySales.olname having patsales>0 ) b
            
            ")


            'connDb.resetCmd("SELECT DATE as 'Sales Date', StoreNo AS 'Store Code', StoreName as 'Store Name', PATTRANS as 'Online PAT Transaction', PATITEM as 'Online PAT Item', PATSALES as 'Online PAT Sales', PATAVG as 'Online PAT Average', AllTrans as 'All Transaction', AllItem as 'All Item', AllSales as 'All Sales', AllAVG as 'All Average', PercenTrans as '% of All Transaction', PercentItem as '% of All Item', PercentSales as '% of All Sales'
            '                FROM
            '                (
            '                SELECT DATE_FORMAT(DailySales.Busidate, '%d %b %Y') AS DATE, DailySales.olnumber 'StoreNo', 
            '                DailySales.olname 'StoreName', IFNULL(( SELECT SUM(QtyTrans) FROM cld_sumsalesdaily 
            '                WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid " & sSlsType & "),0) AS PATTRANS
            '                , IFNULL(( SELECT SUM(Qty) FROM cld_sumsalesdetaildaily 
            '                WHERE BusiDate = DailySales.Busidate AND olId = DailySales.Olid " & sSlsType & "),0) AS PATITEM, 
            '                IFNULL(( SELECT SUM(Subtotal - Discount) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate 
            '                AND cld_sumsalesdaily.olId = DailySales.Olid " & sSlsType & "),0) AS PATSALES
            '                , IFNULL(ROUND(( SELECT SUM(Subtotal - Discount) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate 
            '                AND cld_sumsalesdaily.olId = DailySales.Olid " & sSlsType & ")/( SELECT SUM(QtyTrans) 
            '                FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid  
            '                " & sSlsType & ")),0) AS PATAVG, SUM(QtyTrans) AS AllTrans, IFNULL(( SELECT SUM(Qty) 
            '                FROM cld_sumsalesdetaildaily WHERE BusiDate = DailySales.Busidate AND olId = DailySales.Olid ),0) AS AllItem,
            '                 SUM(Subtotal - Discount) AS AllSales, IFNULL(ROUND(( SELECT SUM(Subtotal - Discount) FROM cld_sumsalesdaily 
            '                 WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid  )/( SELECT SUM(QtyTrans) FROM cld_sumsalesdaily 
            '                 WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid )),0) AS ALLAVG, 
            '                 IFNULL(ROUND(( SELECT SUM(QtyTrans) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate 
            '                 AND cld_sumsalesdaily.olId = DailySales.Olid  " & sSlsType & " )/SUM(QtyTrans) * 100),0) PercenTrans, 
            '                 IFNULL(ROUND(( SELECT SUM(Qty) FROM cld_sumsalesdetaildaily WHERE BusiDate = DailySales.Busidate AND olId = DailySales.Olid  " & sSlsType & ") /( SELECT SUM(Qty) FROM cld_sumsalesdetaildaily WHERE BusiDate = DailySales.Busidate AND olId = DailySales.Olid )  * 100) ,0) PercentItem, 
            '                 IFNULL(ROUND(( SELECT SUM(Subtotal - Discount) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid  " & sSlsType & " )/SUM(Subtotal - Discount) * 100),0) PercentSales FROM cld_sumsalesdaily AS DailySales INNER JOIN cld_outlet AS o ON o.ID = DailySales.olid AND o.custid = DailySales.custid 
            '                 WHERE DailySales.Busidate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' AND DailySales.Busidate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "' " & sOut & sBrand & sOwn & sCity &
            '                " GROUP BY DailySales.Busidate, DailySales.olnumber, DailySales.olname 
            '                 HAVING patsales >0

            '                 UNION ALL

            '                SELECT '' AS DATE, 'Total' AS StoreNo, 
            '                '' AS StoreName, IFNULL(( SELECT SUM(QtyTrans) FROM cld_sumsalesdaily 
            '                WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate " & sSlsType & "),0) AS PATTRANS
            '                , IFNULL(( SELECT SUM(Qty) FROM cld_sumsalesdetaildaily 
            '                WHERE BusiDate = DailySales.Busidate " & sSlsType & "),0) AS PATITEM, 
            '                IFNULL(( SELECT SUM(Subtotal - Discount) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate 
            '                " & sSlsType & "),0) AS PATSALES, FORMAT(IFNULL(ROUND(( SELECT SUM(Subtotal - Discount) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate 
            '                " & sSlsType & ")/( SELECT SUM(QtyTrans) 
            '                FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate   
            '                " & sSlsType & ")),0), 2,'de_DE') AS PATAVG, SUM(QtyTrans) AS AllTrans
            '                , IFNULL(( SELECT SUM(Qty) 
            '                FROM cld_sumsalesdetaildaily WHERE BusiDate = DailySales.Busidate AND
            '                 (olid, Busidate) IN (SELECT olid, Busidate FROM cld_sumsalesdaily WHERE Busidate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' AND Busidate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "' " & sOut & sBrand & sOwn & sCity & sSlsType & ")
            '                ),0) AS AllItem,
            '                 SUM(Subtotal - Discount) AS AllSales, IFNULL(ROUND(( SELECT SUM(Subtotal - Discount) FROM cld_sumsalesdaily 
            '                 WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND
            '                 (olid, Busidate) IN (SELECT olid, Busidate FROM cld_sumsalesdaily WHERE Busidate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' AND Busidate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "' " & sOut & sBrand & sOwn & sCity & sSlsType & ") )/( SELECT SUM(QtyTrans) FROM cld_sumsalesdaily 
            '                 WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND
            '                 (olid, Busidate) IN (SELECT olid, Busidate FROM cld_sumsalesdaily WHERE Busidate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' AND Busidate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "' " & sOut & sBrand & sOwn & sCity & sSlsType & "))),0) AS ALLAVG, 
            '                 IFNULL(ROUND(( SELECT SUM(QtyTrans) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate 
            '                 " & sSlsType & " )/SUM(QtyTrans) * 100),0) PercenTrans, 
            '                 IFNULL(ROUND(( SELECT SUM(Qty) FROM cld_sumsalesdetaildaily WHERE BusiDate = DailySales.Busidate " & sSlsType & ") /( SELECT SUM(Qty) 
            '                 FROM cld_sumsalesdetaildaily WHERE BusiDate = DailySales.Busidate AND
            '                 (olid, Busidate) IN (SELECT olid, Busidate FROM cld_sumsalesdaily WHERE Busidate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' AND Busidate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "' " & sOut & sBrand & sOwn & sCity & sSlsType & "))  * 100) ,0) PercentItem
            '                 , IFNULL(ROUND(( SELECT SUM(Subtotal - Discount) 
            '                 FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate " & sSlsType & " )/SUM(Subtotal - Discount) * 100),0) PercentSales 
            '                 FROM cld_sumsalesdaily AS DailySales WHERE DailySales.Busidate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' AND DailySales.Busidate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "' " & sOut & sBrand & sOwn & sCity & " AND
            '                 (olid, Busidate) IN (SELECT olid, Busidate FROM cld_sumsalesdaily WHERE Busidate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' AND Busidate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "' " & sOut & sBrand & sOwn & sCity & sSlsType & ")

            '                 ) t")


            'connDb.resetCmd("SELECT DATE_FORMAT(DailySales.Busidate, '%d %b %Y') 'Sales Date', DailySales.olnumber 'Store Code', DailySales.olname 'Store Name', " &
            '"IFNULL(( SELECT SUM(QtyTrans) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid " & sSlsType & "),0) 'DIRDELIVER Transaction', " &
            '"IFNULL(( SELECT SUM(Qty) FROM cld_sumsalesdetaildaily WHERE BusiDate = DailySales.Busidate AND olId = DailySales.Olid " & sSlsType & "),0) 'DIRDELIVER Item', " &
            '"IFNULL(( SELECT SUM(Subtotal - Discount) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid " & sSlsType & "),0) AS DIRDELIVERSALES, " &
            '"IFNULL(ROUND(( SELECT SUM(Subtotal - Discount) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid " & sSlsType & ")/( SELECT SUM(QtyTrans) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid " & sSlsType & ")),0) AS DIRDELIVERAVG, " &
            '"SUM(QtyTrans) 'All Transaction', " &
            '"IFNULL(( SELECT SUM(Qty) FROM cld_sumsalesdetaildaily WHERE BusiDate = DailySales.Busidate AND olId = DailySales.Olid ),0) 'All Item', " &
            '"SUM(Subtotal - Discount) AS AllSales, " &
            '"IFNULL(ROUND(( SELECT SUM(Subtotal - Discount) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid  )/( SELECT SUM(QtyTrans) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid )),0) AS ALLAVG, " &
            '"IFNULL(ROUND(( SELECT SUM(QtyTrans) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid " & sSlsType & " )/SUM(QtyTrans) * 100),0) PercenTrans, " &
            '"IFNULL(ROUND(( SELECT SUM(Qty) FROM cld_sumsalesdetaildaily WHERE BusiDate = DailySales.Busidate AND olId = DailySales.Olid " & sSlsType & ") /( SELECT SUM(Qty) FROM cld_sumsalesdetaildaily WHERE BusiDate = DailySales.Busidate AND olId = DailySales.Olid )  * 100) ,0) PercentItem, " &
            '"IFNULL(ROUND(( SELECT SUM(Subtotal - Discount) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid " & sSlsType & " )/SUM(Subtotal - Discount) * 100),0) PercentSales " &
            '"FROM cld_sumsalesdaily AS DailySales INNER JOIN cld_outlet AS o ON o.ID = DailySales.olid AND o.custid = DailySales.custid " &
            '"WHERE DailySales.Busidate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' AND DailySales.Busidate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "' " & sOut & sBrand & sOwn & sCity &
            '" GROUP BY DailySales.Busidate, DailySales.olnumber, DailySales.olname having DIRDELIVERsales>0 ")


            connDb.addParam(dfrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(dTo, "@paramTo", MySqlDbType.DateTime)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)
            ' connDb.addParam(MemberID, "@memberID", MySqlDbType.Int32)


            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:PayAtTable")
            Err.Clear()
        End Try

        Return dsRslt

    End Function

    Private Function PATOnlineOrder_gv(ByVal custID As Integer, ByVal dfrom As Date, ByVal dTo As Date, ByVal sOutlet As String, ByVal iOwn As String, ByVal iBrand As String, ByVal iCity As String, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim dsRslt As New DataSet
        Dim sOut As String = ""
        Dim sBrand As String = ""
        Dim sOwn As String = ""
        Dim sCity As String = ""
        Dim sSlsType As String = ""

        Try
            Dim connDb As New clsDbConn(ServerString)

            If iBrand = "0" Or iBrand = "" Then
                sBrand = ""
            Else
                sBrand = " and o.brandID in (" & iBrand & ")"
            End If

            If iOwn = "0" Or iOwn = "" Then
                sOwn = ""
            Else
                sOwn = " and o.ownID = " & iOwn
            End If
            If iCity = "0" Or iCity = "" Then
                sCity = ""
            Else
                sCity = " and o.cityID in (" & iCity & ")"
            End If

            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If sOutlet = "" Then
                    sOut = " and DailySales.olId in (Select Store From cld_userxstore where user = '" & usr & "')"
                Else
                    sOut = " and DailySales.olnumber in (" & sOutlet & ")"
                End If
            Else
                If sOutlet = "" Then
                    sOut = ""
                Else
                    sOut = " and DailySales.olnumber in (" & sOutlet & ")"
                End If
            End If
            If My.Settings.patSales = "" Then
                sSlsType = ""
            Else
                sSlsType = " and SlsTypeNm in (" & My.Settings.dirdeliver & ")"
            End If
            connDb.resetCmd("SELECT DATE as 'Sales Date',olnumber 'Store Code',olname 'Store Name',pattrans 'Direct Delivery Transaction',patitem 'Direct Delivery Item',patsales 'Direct Delivery Sales',patavg 'Direct Delivery Avg',alltrans 'All Transaction',allitem 'All Item',allsales 'All Sales',allavg 'All Avg', PercenTrans as '% of All Transaction', PercentItem as '% of All Item', PercentSales as '% of All Sales' FROM
            (SELECT DATE_FORMAT(DailySales.Busidate, '%d %b %Y') date, DailySales.olnumber , DailySales.olname , " &
            "IFNULL(( SELECT SUM(QtyTrans) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid " & sSlsType & "),0) pattrans, " &
            "IFNULL(( SELECT SUM(Qty) FROM cld_sumsalesdetaildaily WHERE BusiDate = DailySales.Busidate AND olId = DailySales.Olid " & sSlsType & "),0) patitem, " &
            "FORMAT(IFNULL(( SELECT SUM(Subtotal - Discount) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid " & sSlsType & "),0), 2,'de_DE') AS PATSALES, " &
            "FORMAT(IFNULL(ROUND(( SELECT SUM(Subtotal - Discount) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid " & sSlsType & ")/( SELECT SUM(QtyTrans) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid " & sSlsType & ")),0), 2,'de_DE') AS PATAVG, " &
            "SUM(QtyTrans) alltrans, " &
            "IFNULL(( SELECT SUM(Qty) FROM cld_sumsalesdetaildaily WHERE BusiDate = DailySales.Busidate AND olId = DailySales.Olid ),0) allitem, " &
            "FORMAT(SUM(Subtotal - Discount), 2,'de_DE') AllSales, " &
            "FORMAT(IFNULL(ROUND(( SELECT SUM(Subtotal - Discount) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid  )/( SELECT SUM(QtyTrans) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid )),0), 2,'de_DE') AS ALLAVG, " &
            "FORMAT(IFNULL(ROUND(( SELECT SUM(QtyTrans) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid " & sSlsType & " )/SUM(QtyTrans) * 100),0), 2,'de_DE') PercenTrans, " &
            "FORMAT(IFNULL(ROUND(( SELECT SUM(Qty) FROM cld_sumsalesdetaildaily WHERE BusiDate = DailySales.Busidate AND olId = DailySales.Olid " & sSlsType & ") /( SELECT SUM(Qty) FROM cld_sumsalesdetaildaily WHERE BusiDate = DailySales.Busidate AND olId = DailySales.Olid )  * 100) ,0), 2,'de_DE') PercentItem, " &
            "FORMAT(IFNULL(ROUND(( SELECT SUM(Subtotal - Discount) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid " & sSlsType & " )/SUM(Subtotal - Discount) * 100),0), 2,'de_DE') PercentSales " &
            "FROM cld_sumsalesdaily AS DailySales INNER JOIN cld_outlet AS o ON o.ID = DailySales.olid AND o.custid = DailySales.custid " &
            "WHERE DailySales.Busidate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' AND DailySales.Busidate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "' " & sOut & sBrand & sOwn & sCity &
            " GROUP BY DailySales.Busidate, DailySales.olnumber, DailySales.olname having patsales>0 ) a
            
            union all

            SELECT '','','Total',FORMAT(SUM(pattrans), 0,'de_DE'),FORMAT(SUM(patitem), 0,'de_DE'),FORMAT(SUM(patsales), 2,'de_DE'),FORMAT(SUM(patavg), 2,'de_DE'),FORMAT(SUM(alltrans), 0,'de_DE'),FORMAT(SUM(allitem), 0,'de_DE'),FORMAT(SUM(allsales), 2,'de_DE'),FORMAT(SUM(allavg), 2,'de_DE'),FORMAT(SUM(pattrans)*100/SUM(alltrans), 2,'de_DE'),FORMAT(SUM(patitem)*100/SUM(allitem), 2,'de_DE'),FORMAT(SUM(patsales)*100/SUM(allsales), 2,'de_DE') FROM
            (SELECT DATE_FORMAT(DailySales.Busidate, '%d %b %Y') date, DailySales.olnumber , DailySales.olname , " &
            "IFNULL(( SELECT SUM(QtyTrans) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid " & sSlsType & "),0) pattrans, " &
            "IFNULL(( SELECT SUM(Qty) FROM cld_sumsalesdetaildaily WHERE BusiDate = DailySales.Busidate AND olId = DailySales.Olid " & sSlsType & "),0) patitem, " &
            "IFNULL(( SELECT SUM(Subtotal - Discount) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid " & sSlsType & "),0) AS PATSALES, " &
            "IFNULL(ROUND(( SELECT SUM(Subtotal - Discount) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid " & sSlsType & ")/( SELECT SUM(QtyTrans) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid " & sSlsType & ")),0) AS PATAVG, " &
            "SUM(QtyTrans) alltrans, " &
            "IFNULL(( SELECT SUM(Qty) FROM cld_sumsalesdetaildaily WHERE BusiDate = DailySales.Busidate AND olId = DailySales.Olid ),0) allitem, " &
            "SUM(Subtotal - Discount) AllSales, " &
            "IFNULL(ROUND(( SELECT SUM(Subtotal - Discount) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid  )/( SELECT SUM(QtyTrans) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid )),0) AS ALLAVG, " &
            "IFNULL(ROUND(( SELECT SUM(QtyTrans) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid " & sSlsType & " )/SUM(QtyTrans) * 100),0) PercenTrans, " &
            "IFNULL(ROUND(( SELECT SUM(Qty) FROM cld_sumsalesdetaildaily WHERE BusiDate = DailySales.Busidate AND olId = DailySales.Olid " & sSlsType & ") /( SELECT SUM(Qty) FROM cld_sumsalesdetaildaily WHERE BusiDate = DailySales.Busidate AND olId = DailySales.Olid )  * 100) ,0) PercentItem, " &
            "IFNULL(ROUND(( SELECT SUM(Subtotal - Discount) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid " & sSlsType & " )/SUM(Subtotal - Discount) * 100),0) PercentSales " &
            "FROM cld_sumsalesdaily AS DailySales INNER JOIN cld_outlet AS o ON o.ID = DailySales.olid AND o.custid = DailySales.custid " &
            "WHERE DailySales.Busidate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' AND DailySales.Busidate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "' " & sOut & sBrand & sOwn & sCity &
            " GROUP BY DailySales.Busidate, DailySales.olnumber, DailySales.olname having patsales>0 ) b
            
            ")

            'connDb.resetCmd("SELECT DATE as 'Sales Date', StoreNo AS 'Store Code', StoreName as 'Store Name', FORMAT(PATTRANS, 0,'de_DE') as 'Online PAT Transaction', FORMAT(PATITEM, 0,'de_DE') as 'Online PAT Item', FORMAT(PATSALES, 2,'de_DE') as 'Online PAT Sales', FORMAT(PATAVG, 2,'de_DE') as 'Online PAT Average', FORMAT(AllTrans, 0,'de_DE') as 'All Transaction', FORMAT(AllItem, 0,'de_DE') as 'All Item', FORMAT(AllSales, 2,'de_DE') as 'All Sales', FORMAT(AllAVG, 2,'de_DE') as 'All Average', PercenTrans as '% of All Transaction', PercentItem as '% of All Item', PercentSales as '% of All Sales'
            '                FROM
            '                (
            '                SELECT DATE_FORMAT(DailySales.Busidate, '%d %b %Y') AS DATE, DailySales.olnumber 'StoreNo', 
            '                DailySales.olname 'StoreName', IFNULL(( SELECT SUM(QtyTrans) FROM cld_sumsalesdaily 
            '                WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid " & sSlsType & "),0) AS PATTRANS
            '                , IFNULL(( SELECT SUM(Qty) FROM cld_sumsalesdetaildaily 
            '                WHERE BusiDate = DailySales.Busidate AND olId = DailySales.Olid " & sSlsType & "),0) AS PATITEM, 
            '                IFNULL(( SELECT SUM(Subtotal - Discount) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate 
            '                AND cld_sumsalesdaily.olId = DailySales.Olid " & sSlsType & "),0) AS PATSALES
            '                , IFNULL(ROUND(( SELECT SUM(Subtotal - Discount) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate 
            '                AND cld_sumsalesdaily.olId = DailySales.Olid " & sSlsType & ")/( SELECT SUM(QtyTrans) 
            '                FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid  
            '                " & sSlsType & ")),0) AS PATAVG, SUM(QtyTrans) AS AllTrans, IFNULL(( SELECT SUM(Qty) 
            '                FROM cld_sumsalesdetaildaily WHERE BusiDate = DailySales.Busidate AND olId = DailySales.Olid ),0) AS AllItem,
            '                 SUM(Subtotal - Discount) AS AllSales, IFNULL(ROUND(( SELECT SUM(Subtotal - Discount) FROM cld_sumsalesdaily 
            '                 WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid  )/( SELECT SUM(QtyTrans) FROM cld_sumsalesdaily 
            '                 WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid )),0) AS ALLAVG, 
            '                 IFNULL(ROUND(( SELECT SUM(QtyTrans) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate 
            '                 AND cld_sumsalesdaily.olId = DailySales.Olid  " & sSlsType & " )/SUM(QtyTrans) * 100),0) PercenTrans, 
            '                 IFNULL(ROUND(( SELECT SUM(Qty) FROM cld_sumsalesdetaildaily WHERE BusiDate = DailySales.Busidate AND olId = DailySales.Olid  " & sSlsType & ") /( SELECT SUM(Qty) FROM cld_sumsalesdetaildaily WHERE BusiDate = DailySales.Busidate AND olId = DailySales.Olid )  * 100) ,0) PercentItem, IFNULL(ROUND(( SELECT SUM(Subtotal - Discount) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid  " & sSlsType & " )/SUM(Subtotal - Discount) * 100),0) PercentSales FROM cld_sumsalesdaily AS DailySales INNER JOIN cld_outlet AS o ON o.ID = DailySales.olid AND o.custid = DailySales.custid 
            '                 WHERE DailySales.Busidate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' AND DailySales.Busidate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "' " & sOut & sBrand & sOwn & sCity &
            '                " GROUP BY DailySales.Busidate, DailySales.olnumber, DailySales.olname 
            '                 HAVING patsales >0

            '                 UNION ALL

            '                SELECT '' AS DATE, 'Total' AS StoreNo, 
            '                '' AS StoreName, IFNULL(( SELECT SUM(QtyTrans) FROM cld_sumsalesdaily 
            '                WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate " & sSlsType & "),0) AS PATTRANS
            '                , IFNULL(( SELECT SUM(Qty) FROM cld_sumsalesdetaildaily 
            '                WHERE BusiDate = DailySales.Busidate " & sSlsType & "),0) AS PATITEM, 
            '                IFNULL(( SELECT SUM(Subtotal - Discount) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate 
            '                " & sSlsType & "),0) AS PATSALES, FORMAT(IFNULL(ROUND(( SELECT SUM(Subtotal - Discount) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate 
            '                " & sSlsType & ")/( SELECT SUM(QtyTrans) 
            '                FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate   
            '                " & sSlsType & ")),0), 2,'de_DE') AS PATAVG, SUM(QtyTrans) AS AllTrans
            '                , IFNULL(( SELECT SUM(Qty) 
            '                FROM cld_sumsalesdetaildaily WHERE BusiDate = DailySales.Busidate AND
            '                 (olid, Busidate) IN (SELECT olid, Busidate FROM cld_sumsalesdaily WHERE Busidate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' AND Busidate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "' " & sOut & sBrand & sOwn & sCity & sSlsType & ")
            '                ),0) AS AllItem,
            '                 SUM(Subtotal - Discount) AS AllSales, IFNULL(ROUND(( SELECT SUM(Subtotal - Discount) FROM cld_sumsalesdaily 
            '                 WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND
            '                 (olid, Busidate) IN (SELECT olid, Busidate FROM cld_sumsalesdaily WHERE Busidate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' AND Busidate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "' " & sOut & sBrand & sOwn & sCity & sSlsType & ") )/( SELECT SUM(QtyTrans) FROM cld_sumsalesdaily 
            '                 WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND
            '                 (olid, Busidate) IN (SELECT olid, Busidate FROM cld_sumsalesdaily WHERE Busidate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' AND Busidate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "' " & sOut & sBrand & sOwn & sCity & sSlsType & "))),0) AS ALLAVG, 
            '                 IFNULL(ROUND(( SELECT SUM(QtyTrans) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate 
            '                 " & sSlsType & " )/SUM(QtyTrans) * 100),0) PercenTrans, 
            '                 IFNULL(ROUND(( SELECT SUM(Qty) FROM cld_sumsalesdetaildaily WHERE BusiDate = DailySales.Busidate " & sSlsType & ") /( SELECT SUM(Qty) 
            '                 FROM cld_sumsalesdetaildaily WHERE BusiDate = DailySales.Busidate AND
            '                 (olid, Busidate) IN (SELECT olid, Busidate FROM cld_sumsalesdaily WHERE Busidate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' AND Busidate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "' " & sOut & sBrand & sOwn & sCity & sSlsType & "))  * 100) ,0) PercentItem
            '                 , IFNULL(ROUND(( SELECT SUM(Subtotal - Discount) 
            '                 FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate " & sSlsType & " )/SUM(Subtotal - Discount) * 100),0) PercentSales 
            '                 FROM cld_sumsalesdaily AS DailySales WHERE DailySales.Busidate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' AND DailySales.Busidate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "' " & sOut & sBrand & sOwn & sCity & " AND
            '                 (olid, Busidate) IN (SELECT olid, Busidate FROM cld_sumsalesdaily WHERE Busidate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' AND Busidate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "' " & sOut & sBrand & sOwn & sCity & sSlsType & ")

            '                 ) t")

            'connDb.resetCmd("SELECT DATE_FORMAT(DailySales.Busidate, '%d %b %Y') 'Sales Date', DailySales.olnumber 'Store Code', DailySales.olname 'Store Name', " &
            '"IFNULL(( SELECT SUM(QtyTrans) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid " & sSlsType & "),0) 'DIRDELIVER Transaction', " &
            '"IFNULL(( SELECT SUM(Qty) FROM cld_sumsalesdetaildaily WHERE BusiDate = DailySales.Busidate AND olId = DailySales.Olid " & sSlsType & "),0) 'DIRDELIVER Item', " &
            '"FORMAT(IFNULL(( SELECT SUM(Subtotal - Discount) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid " & sSlsType & "),0), 2,'de_DE') AS 'DIRDELIVERSALES', " &
            '"FORMAT(IFNULL(ROUND(( SELECT SUM(Subtotal - Discount) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid " & sSlsType & ")/( SELECT SUM(QtyTrans) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid " & sSlsType & ")),0), 2,'de_DE') AS DIRDELIVERAVG, " &
            '"SUM(QtyTrans) 'All Transaction', " &
            '"IFNULL(( SELECT SUM(Qty) FROM cld_sumsalesdetaildaily WHERE BusiDate = DailySales.Busidate AND olId = DailySales.Olid ),0) 'All Item', " &
            '"FORMAT(SUM(Subtotal - Discount), 2,'de_DE') AS AllSales, " &
            '"FORMAT(IFNULL(ROUND(( SELECT SUM(Subtotal - Discount) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid  )/( SELECT SUM(QtyTrans) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid )),0), 2,'de_DE') AS ALLAVG, " &
            '"IFNULL(ROUND(( SELECT SUM(QtyTrans) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid " & sSlsType & " )/SUM(QtyTrans) * 100),0) PercenTrans, " &
            '"IFNULL(ROUND(( SELECT SUM(Qty) FROM cld_sumsalesdetaildaily WHERE BusiDate = DailySales.Busidate AND olId = DailySales.Olid " & sSlsType & ") /( SELECT SUM(Qty) FROM cld_sumsalesdetaildaily WHERE BusiDate = DailySales.Busidate AND olId = DailySales.Olid )  * 100) ,0) PercentItem, " &
            '"IFNULL(ROUND(( SELECT SUM(Subtotal - Discount) FROM cld_sumsalesdaily WHERE cld_sumsalesdaily.BusiDate = DailySales.Busidate AND cld_sumsalesdaily.olId = DailySales.Olid " & sSlsType & " )/SUM(Subtotal - Discount) * 100),0) PercentSales " &
            '"FROM cld_sumsalesdaily AS DailySales INNER JOIN cld_outlet AS o ON o.ID = DailySales.olid AND o.custid = DailySales.custid " &
            '"WHERE DailySales.Busidate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' AND DailySales.Busidate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "' " & sOut & sBrand & sOwn & sCity &
            '" GROUP BY DailySales.Busidate, DailySales.olnumber, DailySales.olname having DIRDELIVERsales>0 ")


            connDb.addParam(dfrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(dTo, "@paramTo", MySqlDbType.DateTime)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)
            ' connDb.addParam(MemberID, "@memberID", MySqlDbType.Int32)


            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:PayAtTable")
            Err.Clear()
        End Try

        Return dsRslt

    End Function

    'Tambahan 22 agustus 2019
    Public Function LoanReport(ByVal custID As Integer, ByVal dfrom As Date, ByVal dTo As Date, ByVal Outletstr As String, ByVal iOwn As String, ByVal iBrand As String, ByVal iCity As String, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim dsRslt As New DataSet
        Dim sOut As String = ""
        Dim sBrand As String = ""
        Dim sOwn As String = ""
        Dim sCity As String = ""

        Try
            Dim c_Cmd As New clsDbo(ServerString)
            If iBrand = "0" Or iBrand = "" Then
                sBrand = ""
            Else
                sBrand = " and cld_outlet.brandID in (" & iBrand & ")"
            End If

            If iOwn = "0" Or iOwn = "" Then
                sOwn = ""
            Else
                sOwn = " and cld_outlet.ownID = " & iOwn
            End If

            If iCity = "0" Or iCity = "" Then
                sCity = ""
            Else
                sCity = " and cld_outlet.cityID in (" & iCity & ")"
            End If


            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If Outletstr = "" Then
                    sOut = " and cld_loan.olid in (Select Store From cld_userxstore where user = '" & usr & "')"
                Else
                    sOut = " and cld_loan.olnumber in (" & Outletstr & ")"
                End If
            Else
                If Outletstr = "" Then
                    sOut = ""
                Else
                    sOut = " and cld_loan.olnumber in (" & Outletstr & ")"
                End If
            End If


            c_Cmd.resetCmd("SELECT DATE_FORMAT(cld_loan.Busidate,'%d-%b-%Y') 'Sales Date', cld_loan.olNumber 'Store Code', cld_loan.olName 'Store Name', DATE_FORMAT(cld_loan.Startime,'%H:%i:%s') 'Start Time', DATE_FORMAT(cld_loan.Endtime,'%H:%i:%s') 'End Time' " &
                     "FROM cld_loan INNER JOIN cld_outlet " &
                     "ON cld_loan.olId = cld_outlet.id AND cld_loan.CustID = cld_outlet.custID " &
                     "where cld_loan.Busidate >= @paramFrom AND cld_loan.Busidate <= @paramTo AND (cld_loan.CustID =@custID OR @custID=0) " & sOut & sOwn & sBrand & sCity & " order by cld_loan.Busidate,cld_loan.olNumber,cld_loan.Startime ")

            c_Cmd.addParam(dfrom, "@paramFrom", MySqlDbType.DateTime)
            c_Cmd.addParam(dTo, "@paramTo", MySqlDbType.DateTime)
            c_Cmd.addParam(custID, "@custID", MySqlDbType.Int32)


            dsRslt = c_Cmd.selectDataset(True)

            c_Cmd.dispose()
            c_Cmd = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:salesSum")
            Err.Clear()
        End Try

        Return dsRslt
    End Function

    'Tambah Bram 31 Maret 2017 Rcptdetils
    'request upnormal
    Public Function Rcptdetils(ByVal paramFrom As Date, ByVal paramTo As Date, ByVal outletNo As String, ByVal custID As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim dsRslt As New DataSet

        Try
            Dim connDb As New clsDbConn(ServerString)
            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then

                If outletNo = "" Then
                    connDb.resetCmd("SELECT m.BusiDate, m.Transdate,  m.Number, d.ItemNo, d.ItemName, " & _
                                        "d.ItemGroup,d.MajorGroup,d.Qty, d.Price, d.Subtotal, m.PayNm, m.MemberNo, m.SlsTypeNm,0 AS disc, " & _
                                        "m.Discount, 0 AS itemdisc, m.olName AS Nama " & _
                                    "FROM cld_salesmaster m INNER JOIN " & _
                                        "cld_salesdetail d ON d.smNo = m.Number AND d.Outletid = m.olId AND d.CustID = m.CustID " & _
                                   "where m.BusiDate >=@paramFrom AND m.BusiDate <=@paramTo " & _
                                        "AND (m.olId in (Select Store From cld_userxstore where user = '" & usr & "') " & _
                                        "AND (d.CustID =@custID OR @custID=0) AND d.flag = 1 AND m.flag = 1")
                Else

                    connDb.resetCmd("SELECT m.BusiDate, m.Transdate,  m.Number, d.ItemNo, d.ItemName, d.ItemGroup,d.MajorGroup,d.Qty, " & _
                                        "d.Price, d.Subtotal,m.PayNm,m.MemberNo, m.SlsTypeNm,0 AS disc, m.Discount, 0 AS itemdisc, m.olName AS Nama " & _
                                    "FROM cld_salesmaster m INNER JOIN " & _
                                        "cld_salesdetail d ON d.smNo = m.Number AND d.Outletid = m.olId AND d.CustID = m.CustID " & _
                                    "where m.BusiDate >=@paramFrom AND m.BusiDate <=@paramTo AND (m.olNumber=@outlet) " & _
                                       "AND (d.CustID =@custID OR @custID=0) AND d.flag = 1 AND m.flag = 1")
                End If

            Else
                connDb.resetCmd("SELECT m.BusiDate, m.Transdate , m.Number, d.ItemNo, d.ItemName, d.ItemGroup,d.MajorGroup,d.Qty, d.Price, d.Subtotal,m.PayNm,m.MemberNo, m.SlsTypeNm,0 AS disc, m.Discount, 0 AS itemdisc, m.olName AS Nama " & _
                                 "FROM cld_salesmaster m INNER JOIN cld_salesdetail d ON d.smNo = m.Number AND d.Outletid = m.olId AND d.CustID = m.CustID " & _
                              "where m.BusiDate >=@paramFrom AND m.BusiDate <=@paramTo AND (m.olNumber =@outletNo or @outletNo = '') " & _
                                   "AND (d.CustID =@custID OR @custID=0) AND d.flag = 1 AND m.flag = 1")
            End If
            connDb.addParam(paramFrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(paramTo, "@paramTo", MySqlDbType.DateTime)

            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If outletNo <> "" Then
                    connDb.addParam(outletNo, "@outletNo", MySqlDbType.VarChar)
                End If
            Else
                connDb.addParam(outletNo, "@outletNo", MySqlDbType.VarChar)
            End If

            'connDb.addParam(MemberID, "@memberID", MySqlDbType.Int32)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)


            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:Rcptdetils")
            Err.Clear()
        End Try

        Return dsRslt
    End Function

    'Penambahan 26 apriL 2018
    'perubahan 2 mei 2018, nambahin param from dan param to
    Public Function Rcptdetils2(ByVal paramFrom As Date, ByVal paramTo As Date, ByVal outletNo As String, ByVal custID As Integer, ByVal usr As String, ByVal modpriv As Integer, ByVal iOwn As Integer, ByVal iBrand As String) As DataSet
        Dim dsRslt As New DataSet
        Dim sOut As String = ""
        Dim sBrand As String = ""
        Dim sOwn As String = ""

        Try
            Dim connDb As New clsDbConn(ServerString)
            If iBrand = "0" Or iBrand = "" Then
                sBrand = ""
            Else
                sBrand = " and o.brandID = " & iBrand
            End If

            If iOwn = "0" Then
                sOwn = ""
            Else
                sOwn = " and o.ownID = " & iOwn
            End If

            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If outletNo = "" Then
                    sOut = " and sm.olId in (Select Store From cld_userxstore where user = '" & usr & "')"
                Else
                    sOut = " and sm.olnumber in (" & outletNo & ")"
                End If
            Else
                If outletNo = "" Then
                    sOut = ""
                Else
                    sOut = " and sm.olnumber in (" & outletNo & ")"
                End If
            End If

            'connDb.resetCmd("SELECT sm.BusiDate, sm.Transdate,  sm.Number as 'Receipt No', d.ItemNo as 'PLU No', d.ItemName as 'PLU Name', " &
            '                    "d.ItemGroup 'Group',d.MajorGroup as Category,d.Qty, d.Price as ItemAmmount, d.Subtotal, sm.PayNm as Payment, sm.MemberNo as 'Customer ID', sm.SlsTypeNm as 'Trans Type',0 AS Discount, " &
            '                    "sm.Discount as 'Bill Disc', 0 AS 'Item Disc', sm.olName AS StoreName " &
            '                "FROM cld_salesmaster sm INNER JOIN " &
            '                    "cld_salesdetail d ON d.smNo = sm.Number AND d.Outletid = sm.olId AND d.CustID = sm.CustID " &
            '                    "INNER JOIN cld_outlet o On (o.id = sm.olid And o.custid = sm.custid) " &
            '               "where sm.BusiDate >=@paramFrom AND sm.BusiDate <=@paramTo AND d.flag = 1 AND sm.flag = 1 " &
            '                    " AND (sm.CustID=@custID OR @custID=0) " & sOut & sOwn & sBrand)


            connDb.resetCmd("SELECT DATE_FORMAT(sm.BusiDate,'%d-%b-%Y') AS 'Sales Date', sm.olnumber AS 'Store Code', sm.olName AS 'Store Name', sm.Transdate AS 'Transaction Date',
                          sm.Number AS 'Receipt No',

                           CASE

                            WHEN sm.SlsTypeNm = 'DineIn' THEN 'DINE IN'

                            WHEN sm.SlsTypeNm = 'TakeAway' THEN 'TAKE AWAY'

                            ELSE sm.SlsTypeNm

                        END AS 'Channel' ,

                            d.MajorGroup 'Major Group', d.ItemGroup 'Item Group', d.ItemNo AS 'Item Code', d.ItemName AS 'Item Name',
                          d.Qty, d.Price 'Item Amount', 
                          d.Subtotal 'Net Sales', sm.PayNm AS 'Payment Name',
                          0 AS Discount, sm.Discount AS 'Bill Disc', 0 AS 'Item Disc'
                            FROM cld_salesmaster sm INNER JOIN cld_salesdetail d ON d.smNo = sm.Number AND d.Outletid = sm.olId 
                            AND d.CustID = sm.CustID INNER JOIN cld_outlet o ON (o.id = sm.olid AND o.custid = sm.custid) 
                            WHERE sm.BusiDate >=@paramFrom AND sm.BusiDate <=@paramTo 
                            AND d.flag = 1 AND sm.flag = 1  AND (sm.CustID=@CustID OR @CustID=0) " & sOut & sOwn & sBrand)

            connDb.addParam(paramFrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(paramTo, "@paramTo", MySqlDbType.DateTime)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)

            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:Rcptdetils")
            Err.Clear()
        End Try

        Return dsRslt
    End Function

    'untuk gv
    Public Function Rcptdetils_gv(ByVal paramFrom As Date, ByVal paramTo As Date, ByVal outletNo As String, ByVal custID As Integer, ByVal usr As String, ByVal modpriv As Integer, ByVal iOwn As Integer, ByVal iBrand As String) As DataSet
        Dim dsRslt As New DataSet
        Dim sOut As String = ""
        Dim sBrand As String = ""
        Dim sOwn As String = ""

        Try
            Dim connDb As New clsDbConn(ServerString)
            If iBrand = "0" Or iBrand = "" Then
                sBrand = ""
            Else
                sBrand = " and o.brandID = " & iBrand
            End If

            If iOwn = "0" Then
                sOwn = ""
            Else
                sOwn = " and o.ownID = " & iOwn
            End If

            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If outletNo = "" Then
                    sOut = " and sm.olId in (Select Store From cld_userxstore where user = '" & usr & "')"
                Else
                    sOut = " and sm.olnumber in (" & outletNo & ")"
                End If
            Else
                If outletNo = "" Then
                    sOut = ""
                Else
                    sOut = " and sm.olnumber in (" & outletNo & ")"
                End If
            End If

            connDb.resetCmd("SELECT DATE_FORMAT(sm.BusiDate,'%d-%b-%Y') AS 'Sales Date', sm.olNumber AS 'Store Code', sm.olName AS 'Store Name', sm.Transdate AS 'Transaction Date',
                          sm.Number AS 'Receipt No',

                            CASE

                            WHEN sm.SlsTypeNm = 'DineIn' THEN 'DINE IN'

                            WHEN sm.SlsTypeNm = 'TakeAway' THEN 'TAKE AWAY'

                            ELSE sm.SlsTypeNm

                        END AS 'Channel' ,

                        d.MajorGroup 'Major Group', d.ItemGroup 'Item Group', d.ItemNo AS 'Item Code', d.ItemName AS 'Item Name',
                          FORMAT(d.Qty, 0,'de_DE') Qty, FORMAT(d.Price, 2,'de_DE') 'Item Amount', 
                          FORMAT(d.Subtotal, 2,'de_DE') 'Net Sales', sm.PayNm AS 'Payment Name',
                           0 AS Discount, FORMAT(sm.Discount, 2,'de_DE') AS 'Bill Disc', 0 AS 'Item Disc'
                            FROM cld_salesmaster sm INNER JOIN cld_salesdetail d ON d.smNo = sm.Number AND d.Outletid = sm.olId 
                            AND d.CustID = sm.CustID INNER JOIN cld_outlet o ON (o.id = sm.olid AND o.custid = sm.custid) 
                            WHERE sm.BusiDate >=@paramFrom AND sm.BusiDate <=@paramTo 
                            AND d.flag = 1 AND sm.flag = 1  AND (sm.CustID=@CustID OR @CustID=0) " & sOut & sOwn & sBrand) ' &
            '" UNION ALL SELECT '','','','','','','','','','Total',FORMAT(SUM(d.Qty), 0,'de_DE'),'',FORMAT(SUM(d.Subtotal), 2,'de_DE'),
            '',SUM(0),FORMAT(SUM(sm.Discount), 2,'de_DE'),SUM(0) FROM cld_salesmaster sm INNER JOIN cld_salesdetail d ON d.smNo = sm.Number AND d.Outletid = sm.olId 
            'AND d.CustID = sm.CustID INNER JOIN cld_outlet o ON (o.id = sm.olid AND o.custid = sm.custid) 
            'WHERE sm.BusiDate >=@paramFrom AND sm.BusiDate <=@paramTo 
            'AND d.flag = 1 AND sm.flag = 1  AND (sm.CustID=@CustID OR @CustID=0) " & sOut & sOwn & sBrand)

            'connDb.resetCmd("SELECT DATE_FORMAT(sm.BusiDate,'%d-%b-%Y') as busidate, sm.Transdate,  sm.Number as 'Receipt No', d.ItemNo as 'PLU No', d.ItemName as 'PLU Name', " &
            '                    "d.ItemGroup 'Group',d.MajorGroup as Category,FORMAT(d.Qty, 0,'de_DE') Qty, d.Price as ItemAmmount, FORMAT(d.Subtotal, 2,'de_DE') Subtotal, sm.PayNm as Payment, sm.MemberNo as 'Customer ID', sm.SlsTypeNm as 'Trans Type',0 AS Discount, " &
            '                    "sm.Discount as 'Bill Disc', 0 AS 'Item Disc', sm.olName AS StoreName " &
            '                "FROM cld_salesmaster sm INNER JOIN " &
            '                    "cld_salesdetail d ON d.smNo = sm.Number AND d.Outletid = sm.olId AND d.CustID = sm.CustID " &
            '                    "INNER JOIN cld_outlet o On (o.id = sm.olid And o.custid = sm.custid) " &
            '               "where sm.BusiDate >=@paramFrom AND sm.BusiDate <=@paramTo AND d.flag = 1 AND sm.flag = 1 " &
            '                    " AND (sm.CustID=@custID OR @custID=0) " & sOut & sOwn & sBrand)

            connDb.addParam(paramFrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(paramTo, "@paramTo", MySqlDbType.DateTime)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)

            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:Rcptdetils")
            Err.Clear()
        End Try

        Return dsRslt
    End Function

    Public Function Rcptdetils2_2018(ByVal paramFrom As Date, ByVal paramTo As Date, ByVal outletNo As String, ByVal custID As Integer, ByVal usr As String, ByVal modpriv As Integer, ByVal iOwn As Integer, ByVal iBrand As String) As DataSet
        Dim dsRslt As New DataSet
        Dim sOut As String = ""
        Dim sBrand As String = ""
        Dim sOwn As String = ""

        Try
            Dim connDb As New clsDbConn(ServerString)
            If iBrand = "0" Or iBrand = "" Then
                sBrand = ""
            Else
                sBrand = " and o.brandID = " & iBrand
            End If

            If iOwn = "0" Then
                sOwn = ""
            Else
                sOwn = " and o.ownID = " & iOwn
            End If

            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If outletNo = "" Then
                    sOut = " and sm.olId in (Select Store From cld_userxstore where user = '" & usr & "')"
                Else
                    sOut = " and sm.olnumber in (" & outletNo & ")"
                End If
            Else
                If outletNo = "" Then
                    sOut = ""
                Else
                    sOut = " and sm.olnumber in (" & outletNo & ")"
                End If
            End If

            'connDb.resetCmd("SELECT sm.BusiDate, sm.Transdate,  sm.Number as 'Receipt No', d.ItemNo as 'PLU No', d.ItemName as 'PLU Name', " &
            '                    "d.ItemGroup 'Group',d.MajorGroup as Category,d.Qty, d.Price as ItemAmmount, d.Subtotal, sm.PayNm as Payment, sm.MemberNo as 'Customer ID', sm.SlsTypeNm as 'Trans Type',0 AS Discount, " &
            '                    "sm.Discount as 'Bill Disc', 0 AS 'Item Disc', sm.olName AS StoreName " &
            '                "FROM cld_salesmaster2018 sm INNER JOIN " &
            '                    "cld_salesdetail2018 d ON d.smNo = sm.Number AND d.Outletid = sm.olId AND d.CustID = sm.CustID " &
            '                    "INNER JOIN cld_outlet o On (o.id = sm.olid And o.custid = sm.custid) " &
            '               "where sm.BusiDate >=@paramFrom AND sm.BusiDate <=@paramTo AND d.flag = 1 AND sm.flag = 1 " &
            '                    " AND (sm.CustID=@custID OR @custID=0) " & sOut & sOwn & sBrand)
            connDb.resetCmd("SELECT DATE_FORMAT(sm.BusiDate,'%d-%b-%Y') AS 'Sales Date', sm.olnumber AS 'Store Code', sm.olName AS 'Store Name', sm.Transdate AS 'Transaction Date',
                          sm.Number AS 'Receipt No',

                            CASE

                            WHEN sm.SlsTypeNm = 'DineIn' THEN 'DINE IN'

                            WHEN sm.SlsTypeNm = 'TakeAway' THEN 'TAKE AWAY'

                            ELSE sm.SlsTypeNm

                        END AS 'Channel' ,

                            d.MajorGroup 'Major Group', d.ItemGroup 'Item Group', d.ItemNo AS 'Item Code', d.ItemName AS 'Item Name',
                          d.Qty, d.Price 'Item Amount', 
                          d.Subtotal 'Net Sales', sm.PayNm AS 'Payment Name',
                          0 AS Discount, sm.Discount AS 'Bill Disc', 0 AS 'Item Disc'
                            FROM cld_salesmaster2018 sm INNER JOIN cld_salesdetail2018 d ON d.smNo = sm.Number AND d.Outletid = sm.olId 
                            AND d.CustID = sm.CustID INNER JOIN cld_outlet o ON (o.id = sm.olid AND o.custid = sm.custid) 
                            WHERE sm.BusiDate >=@paramFrom AND sm.BusiDate <=@paramTo 
                            AND d.flag = 1 AND sm.flag = 1  AND (sm.CustID=@CustID OR @CustID=0) " & sOut & sOwn & sBrand)

            connDb.addParam(paramFrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(paramTo, "@paramTo", MySqlDbType.DateTime)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)

            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:Rcptdetils")
            Err.Clear()
        End Try

        Return dsRslt
    End Function
    Public Function Rcptdetils2018_gv(ByVal paramFrom As Date, ByVal paramTo As Date, ByVal outletNo As String, ByVal custID As Integer, ByVal usr As String, ByVal modpriv As Integer, ByVal iOwn As Integer, ByVal iBrand As String) As DataSet
        Dim dsRslt As New DataSet
        Dim sOut As String = ""
        Dim sBrand As String = ""
        Dim sOwn As String = ""

        Try
            Dim connDb As New clsDbConn(ServerString)
            If iBrand = "0" Or iBrand = "" Then
                sBrand = ""
            Else
                sBrand = " and o.brandID = " & iBrand
            End If

            If iOwn = "0" Then
                sOwn = ""
            Else
                sOwn = " and o.ownID = " & iOwn
            End If

            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If outletNo = "" Then
                    sOut = " and sm.olId in (Select Store From cld_userxstore where user = '" & usr & "')"
                Else
                    sOut = " and sm.olnumber in (" & outletNo & ")"
                End If
            Else
                If outletNo = "" Then
                    sOut = ""
                Else
                    sOut = " and sm.olnumber in (" & outletNo & ")"
                End If
            End If

            'connDb.resetCmd("SELECT DATE_FORMAT(sm.BusiDate,'%d-%b-%Y') as busidate, sm.Transdate,  sm.Number as 'Receipt No', d.ItemNo as 'PLU No', d.ItemName as 'PLU Name', " &
            '                    "d.ItemGroup 'Group',d.MajorGroup as Category,FORMAT(d.Qty, 0,'de_DE') Qty, d.Price as ItemAmmount, FORMAT(d.Subtotal, 2,'de_DE') Subtotal, sm.PayNm as Payment, sm.MemberNo as 'Customer ID', sm.SlsTypeNm as 'Trans Type',0 AS Discount, " &
            '                    "sm.Discount as 'Bill Disc', 0 AS 'Item Disc', sm.olName AS StoreName " &
            '                "FROM cld_salesmaster2018 sm INNER JOIN " &
            '                    "cld_salesdetail2018 d ON d.smNo = sm.Number AND d.Outletid = sm.olId AND d.CustID = sm.CustID " &
            '                    "INNER JOIN cld_outlet o On (o.id = sm.olid And o.custid = sm.custid) " &
            '               "where sm.BusiDate >=@paramFrom AND sm.BusiDate <=@paramTo AND d.flag = 1 AND sm.flag = 1 " &
            '                    " AND (sm.CustID=@custID OR @custID=0) " & sOut & sOwn & sBrand)
            connDb.resetCmd("SELECT DATE_FORMAT(sm.BusiDate,'%d-%b-%Y') AS 'Sales Date', sm.olNumber AS 'Store Code', sm.olName AS 'Store Name', sm.Transdate AS 'Transaction Date',
                          sm.Number AS 'Receipt No',

                           CASE

                            WHEN sm.SlsTypeNm = 'DineIn' THEN 'DINE IN'

                            WHEN sm.SlsTypeNm = 'TakeAway' THEN 'TAKE AWAY'

                            ELSE sm.SlsTypeNm

                        END AS 'Channel' ,
                
                            d.MajorGroup 'Major Group', d.ItemGroup 'Item Group', d.ItemNo AS 'Item Code', d.ItemName AS 'Item Name',
                          FORMAT(d.Qty, 0,'de_DE') Qty, FORMAT(d.Price, 2,'de_DE') 'Item Amount', 
                          FORMAT(d.Subtotal, 2,'de_DE') 'Net Sales', sm.PayNm AS 'Payment Name',
                           0 AS Discount, FORMAT(sm.Discount, 2,'de_DE') AS 'Bill Disc', 0 AS 'Item Disc'
                            FROM cld_salesmaster2018 sm INNER JOIN cld_salesdetail2018 d ON d.smNo = sm.Number AND d.Outletid = sm.olId 
                            AND d.CustID = sm.CustID INNER JOIN cld_outlet o ON (o.id = sm.olid AND o.custid = sm.custid) 
                            WHERE sm.BusiDate >=@paramFrom AND sm.BusiDate <=@paramTo 
                            AND d.flag = 1 AND sm.flag = 1  AND (sm.CustID=@CustID OR @CustID=0) " & sOut & sOwn & sBrand) ' &
            '" UNION ALL SELECT '','','','','','','','','','Total',FORMAT(SUM(d.Qty), 0,'de_DE'),'',FORMAT(SUM(d.Subtotal), 2,'de_DE'),
            '',SUM(0),FORMAT(SUM(sm.Discount), 2,'de_DE'),SUM(0) FROM cld_salesmaster2018 sm INNER JOIN cld_salesdetail2018 d ON d.smNo = sm.Number AND d.Outletid = sm.olId 
            'And d.CustID = sm.CustID INNER JOIN cld_outlet o ON (o.id = sm.olid AND o.custid = sm.custid) 
            'WHERE sm.BusiDate >=@paramFrom AND sm.BusiDate <=@paramTo 
            'AND d.flag = 1 AND sm.flag = 1  AND (sm.CustID=@CustID OR @CustID=0) " & sOut & sOwn & sBrand)

            connDb.addParam(paramFrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(paramTo, "@paramTo", MySqlDbType.DateTime)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)

            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:Rcptdetils")
            Err.Clear()
        End Try

        Return dsRslt
    End Function

    'Bram 03Feb2017
    Public Function DepMemberBalance(ByVal custID As Integer, ByVal memberNo As String) As DataSet

        Dim dsRslt As New DataSet

        Try
            Dim connDb As New clsDbConn(ServerString)

            connDb.resetCmd("SELECT CAST(AES_DECRYPT(cld_memberdep.DepTotal, 'CRM8899')AS DECIMAL (15.2)) AS DeptTotal ,cld_memberdep.memberno, cld_member.Name, cld_member.JoinDate, cld_member.MobileNo " & _
                   "FROM cld_memberdep INNER JOIN cld_member " & _
                        "ON(cld_memberdep.memberno = cld_member.Number AND cld_memberdep.Custid = cld_member.CustID) " & _
                   "where cld_memberdep.CustID =@custID " & _
                        "AND (cld_memberdep.memberNo = @memberNo OR @memberNo='')")

            connDb.addParam(custID, "@custID", MySqlDbType.Int32)
            connDb.addParam(memberno, "@memberNo", MySqlDbType.String)

            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:memberDeposit1")
            Err.Clear()
        End Try

        Return dsRslt

    End Function

    'Bram depositperMember
    Public Function DepositSum(ByVal paramFrom As Date, ByVal paramTo As Date, ByVal outletNo As String, ByVal custID As Integer, _
                               ByVal usr As String, ByVal modpriv As Integer, ByVal memberNo As String) As DataSet
        Dim dsRslt As New DataSet

        Try
            Dim connDb As New clsDbConn(ServerString)
            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then

                If outletNo = "" Then

                    connDb.resetCmd("SELECT m.Number, m.name, sum(if((d.DepGet)>=0,d.DepGet,0)) as deposit, SUM(IF((d.DepGet)<0,d.DepGet,0)) AS deposituse, sum(d.DepGet), case d.flag when 1 then 'deposit' when 2 then 'use deposit' when 0 then 'open deposit' end as type " & _
                                   "FROM cld_memberdeposit d INNER JOIN cld_member m ON (d.memberNo = m.number) INNER JOIN cld_outlet o ON(d.OlNumber = o.Kode AND d.Custid = o.custID) " & _
                              "where d.flag <> 3 and d.Busidate >=@paramFrom AND d.Busidate <=@paramTo " & _
                                    "AND (o.ID in (Select Store From cld_userxstore where user = '" & usr & "')) " & _
                                    "AND (d.memberNo = @memberNo OR @memberNo='') " & _
                                   "AND (d.CustID =@custID OR @custID=0) GROUP BY m.Number ORDER BY SUM(d.DepGet) DESC ")

                Else

                    connDb.resetCmd("SELECT m.Number, m.name, sum(if((d.DepGet)>=0,d.DepGet,0)) as deposit, SUM(IF((d.DepGet)<0,d.DepGet,0)) AS deposituse, sum(d.DepGet), case d.flag when 1 then 'deposit' when 2 then 'use deposit' when 0 then 'open deposit' end as type " & _
                                   "FROM cld_memberdeposit d INNER JOIN cld_member m ON (d.memberNo = m.number) INNER JOIN cld_outlet o ON(d.OlNumber = o.Kode AND d.Custid = o.custID)) " & _
                              "where d.flag <> 3 and d.Busidate >=@paramFrom AND d.Busidate <=@paramTo AND (d.olNumber=@outletNo) " & _
                                    "AND (d.memberNo = @memberNo OR @memberNo='') " & _
                                   "AND (d.CustID =@custID OR @custID=0) GROUP BY m.Number ORDER BY SUM(d.DepGet) DESC ")

                End If

            Else
                connDb.resetCmd("SELECT m.Number, m.name, sum(if((d.DepGet)>=0,d.DepGet,0)) as deposit, SUM(IF((d.DepGet)<0,d.DepGet,0)) AS deposituse, sum(d.DepGet), case d.flag when 1 then 'deposit' when 2 then 'use deposit' when 0 then 'open deposit' end as deposit " & _
                                 "FROM cld_memberdeposit d INNER JOIN cld_member m ON (d.memberNo = m.number) INNER JOIN cld_outlet o ON(d.OlNumber = o.Kode AND d.Custid = o.custID) " & _
                              "where d.flag <> 3 and d.Busidate >=@paramFrom AND d.Busidate <=@paramTo AND (d.olNumber =@outletNo or @outletNo = '') " & _
                                    "AND (d.memberNo = @memberNo OR @memberNo='') " & _
                                   "AND (d.CustID =@custID OR @custID=0) GROUP BY m.Number ORDER BY SUM(d.DepGet) DESC ")
            End If
            connDb.addParam(paramFrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(paramTo, "@paramTo", MySqlDbType.DateTime)

            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If outletNo <> "" Then
                    connDb.addParam(outletNo, "@outletNo", MySqlDbType.VarChar)
                End If
            Else
                connDb.addParam(outletNo, "@outletNo", MySqlDbType.VarChar)
            End If

            connDb.addParam(memberNo, "@memberNo", MySqlDbType.String)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)


            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:memberDeposit1")
            Err.Clear()
        End Try

        Return dsRslt
    End Function

    'Bram member deposit 
    Public Function DepositDetail(ByVal paramFrom As Date, ByVal paramTo As Date, ByVal outletNo As String, ByVal custID As Integer, _
                                  ByVal usr As String, ByVal modpriv As Integer, ByVal memberNo As String) As DataSet
        Dim dsRslt As New DataSet

        Try

            Dim connDb As New clsDbConn(ServerString)
            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then


                If outletNo = "" Then

                    connDb.resetCmd("SELECT m.Number, m.name, d.OlNumber, o.Nama, d.TransNo, d.TDate, d.Busidate, d.TransTtl, d.DepGet, d.DepBefore, case d.flag when 1 then d.TransName when 2 then 'use deposit' when 0 then 'open deposit' end as deposit, d.TransName, CAST(AES_DECRYPT(md.DepTotal, 'CRM8899')AS DECIMAL (15.2)) AS Balance " & _
                               "FROM cld_memberdeposit d INNER JOIN cld_member m ON (d.memberNo = m.number) INNER JOIN cld_outlet o ON(d.OlNumber = o.Kode AND d.Custid = o.custID) INNER JOIN cld_memberdep md ON md.memberno = m.Number AND md.Custid = m.CustID " & _
                          "where d.flag <> 3 and d.Busidate >=@paramFrom AND d.Busidate <=@paramTo " & _
                                "AND (o.ID in (Select Store From cld_userxstore where user = '" & usr & "')) " & _
                                "AND (d.memberNo = @memberNo OR @memberNo='') " & _
                               "AND (d.CustID =@custID OR @custID=0) ")
                Else


                    connDb.resetCmd("SELECT m.Number, m.name, d.OlNumber, o.Nama, d.TransNo, d.TDate, d.Busidate, d.TransTtl, d.DepGet, d.DepBefore, case d.flag when 1 then d.TransName when 2 then 'use deposit' when 0 then 'open deposit' end as deposit, d.TransName, CAST(AES_DECRYPT(md.DepTotal, 'CRM8899')AS DECIMAL (15.2)) AS Balance " & _
                               "FROM cld_memberdeposit d INNER JOIN cld_member m ON (d.memberNo = m.number) INNER JOIN cld_outlet o ON(d.OlNumber = o.Kode AND d.Custid = o.custID) INNER JOIN cld_memberdep md ON md.memberno = m.Number AND md.Custid = m.CustID " & _
                          "where d.flag <> 3 and d.Busidate >=@paramFrom AND d.Busidate <=@paramTo AND (d.olNumber=@outletNo) " & _
                                "AND (d.memberNo = @memberNo OR @memberNo='') " & _
                               "AND (d.CustID =@custID OR @custID=0) ")

                End If

            Else
                connDb.resetCmd("SELECT m.Number, m.name, d.OlNumber, o.Nama, d.TransNo, d.TDate, d.Busidate, d.TransTtl, d.DepGet, d.DepBefore, case d.flag when 1 then d.TransName when 2 then 'use deposit' when 0 then 'open deposit' end as deposit, d.TransName, CAST(AES_DECRYPT(md.DepTotal, 'CRM8899')AS DECIMAL (15.2)) AS Balance " & _
                                 "FROM cld_memberdeposit d INNER JOIN cld_member m ON (d.memberNo = m.number) INNER JOIN cld_outlet o ON(d.OlNumber = o.Kode AND d.Custid = o.custID) INNER JOIN cld_memberdep md ON md.memberno = m.Number AND md.Custid = m.CustID " & _
                              "where d.flag <> 3 and d.Busidate >=@paramFrom AND d.Busidate <=@paramTo " & _
                                    "AND (d.olNumber =@outletNo or @outletNo = '') " & _
                                    "AND (d.memberNo = @memberNo OR @memberNo='') " & _
                                   "AND (d.CustID =@custID OR @custID=0) ")
            End If

            connDb.addParam(paramFrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(paramTo, "@paramTo", MySqlDbType.DateTime)

            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If outletNo <> "" Then
                    connDb.addParam(outletNo, "@outletNo", MySqlDbType.VarChar)
                End If
            Else
                connDb.addParam(outletNo, "@outletNo", MySqlDbType.VarChar)
            End If

            connDb.addParam(memberNo, "@memberNo", MySqlDbType.String)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)


            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:memberDeposit1")
            Err.Clear()
        End Try

        Return dsRslt
    End Function

    'Bram member deposit1 
    Public Function DepositTopup(ByVal paramFrom As Date, ByVal paramTo As Date, ByVal outletNo As String, ByVal custID As Integer, _
                                 ByVal usr As String, ByVal modpriv As Integer, ByVal memberNo As String) As DataSet
        Dim dsRslt As New DataSet

        Try
            Dim connDb As New clsDbConn(ServerString)
            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then


                If outletNo = "" Then

                    connDb.resetCmd("SELECT m.Number, m.name, d.OlNumber, o.Nama, d.TransNo, d.TDate, d.Busidate, d.TransTtl, d.DepGet, d.DepBefore, case d.flag when 1 then 'deposit' when 2 then 'use deposit' when 0 then 'open deposit' end as deposit " & _
                                   "FROM cld_memberdeposit d INNER JOIN cld_member m ON (d.memberNo = m.number) INNER JOIN cld_outlet o ON(d.OlNumber = o.Kode AND d.Custid = o.custID) " & _
                              "where d.Flag=1 AND d.Busidate >=@paramFrom AND d.Busidate <=@paramTo " & _
                                    "AND (o.ID in (Select Store From cld_userxstore where user = '" & usr & "')) " & _
                                    "AND (d.memberNo = @memberNo OR @memberNo='') " & _
                                   "AND (d.CustID =@custID OR @custID=0) ")
                Else

                    connDb.resetCmd("SELECT m.Number, m.name, d.OlNumber, o.Nama, d.TransNo, d.TDate, d.Busidate, d.TransTtl, d.DepGet, d.DepBefore, case d.flag when 1 then 'deposit' when 2 then 'use deposit' when 0 then 'open deposit' end as deposit " & _
                                   "FROM cld_memberdeposit d INNER JOIN cld_member m ON (d.memberNo = m.number) INNER JOIN cld_outlet o ON(d.OlNumber = o.Kode AND d.Custid = o.custID) " & _
                              "where d.Flag=1 AND d.Busidate >=@paramFrom AND d.Busidate <=@paramTo " & _
                                    "AND (d.olNumber=@outletNo) " & _
                                    "AND (d.memberNo = @memberNo OR @memberNo='') " & _
                                    "AND (d.CustID =@custID OR @custID=0) ")
                End If


            Else
                connDb.resetCmd("SELECT m.Number, m.name, d.OlNumber, o.Nama, d.TransNo, d.TDate, d.Busidate, d.TransTtl, d.DepGet, d.DepBefore, case d.flag when 1 then 'deposit' when 2 then 'use deposit' when 0 then 'open deposit' end as deposit " & _
                                 "FROM cld_memberdeposit d INNER JOIN cld_member m ON (d.memberNo = m.number) INNER JOIN cld_outlet o ON(d.OlNumber = o.Kode AND d.Custid = o.custID) " & _
                              "where d.Flag=1 AND d.Busidate >=@paramFrom AND d.Busidate <=@paramTo " & _
                                    "AND (d.olNumber =@outletNo or @outletNo = '') " & _
                                    "AND (d.memberNo = @memberNo OR @memberNo='') " & _
                                    "AND (d.CustID =@custID OR @custID=0) ")
            End If
            connDb.addParam(paramFrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(paramTo, "@paramTo", MySqlDbType.DateTime)

            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If outletNo <> "" Then
                    connDb.addParam(outletNo, "@outletNo", MySqlDbType.VarChar)
                End If
            Else
                connDb.addParam(outletNo, "@outletNo", MySqlDbType.VarChar)
            End If
            connDb.addParam(memberNo, "@memberNO", MySqlDbType.String)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)


            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:memberDeposit")
            Err.Clear()
        End Try

        Return dsRslt
    End Function

    'Revisi 08 Juni
    Public Function loadOutlet(ByVal custID As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim dsRslt As New DataSet

        Try
            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then

                Dim dbConn As New clsDbConn(ServerString)

                dbConn.resetCmd("SELECT '0' AS kode, 'All Store' AS Nama UNION ALL " &
                                        "SELECT Kode, Nama FROM cld_outlet WHERE CustID=@CustID " &
                                        "and id in (Select store from cld_userxstore where user = @user) ORDER BY kode")

                dbConn.addParam(custID, "@custID", MySqlDbType.Int32)
                dbConn.addParam(usr, "@user", MySqlDbType.VarChar)
                dsRslt = dbConn.selectDataset()

                dbConn.dispose()
                dbConn = Nothing
            Else

                Dim dbConn As New clsDbConn(ServerString)

                dbConn.resetCmd("SELECT '0' AS kode, 'All Store' AS Nama UNION ALL " &
                                        "SELECT Kode, Nama FROM cld_outlet WHERE CustID=@CustID " &
                                        " ORDER BY kode")

                dbConn.addParam(custID, "@custID", MySqlDbType.Int32)

                dsRslt = dbConn.selectDataset()

                dbConn.dispose()
                dbConn = Nothing
            End If
        Catch ex As Exception
            errLog(Err.Description, "ClsReports:Outlet")
            Err.Clear()
        End Try

        Return dsRslt

    End Function

    'Dashboard
    Public Function totalSales(ByVal Tgl1 As Date, ByVal outletNo As String, ByVal custID As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim dsRslt As New DataSet

        '17 jan 2018
        'nett = subtotal - discount
        Try
            Dim connDb As New clsDbConn(ServerString)

            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                connDb.resetCmd("SELECT FORMAT(IFNULL(SUM(total),0), 2,'de_DE') AS gross " &
                                        ", FORMAT(IFNULL(SUM(subtotal - discount),0), 2,'de_DE') AS nett, IFNULL(SUM(subtotal - discount),0) AS nett2 " &
                                        ", FORMAT(IFNULL(Sum(QtyTrans),0), 0,'de_DE') AS ttlTrans, IFNULL(Sum(QtyTrans),0) AS ttlTrans2, FORMAT(IFNULL(SUM(subtotal - discount)/Sum(QtyTrans),0), 2,'de_DE') AS avg " &
                                    "FROM cld_sumsalesdaily " &
                                    "WHERE busiDate >='" & Tgl1.Date.ToString("yyyy-MM-01 00:00:00") & "' And busiDate <=(SELECT LAST_DAY('" & Tgl1.Date.ToString("yyyy-MM-dd 00:00:00") & "')) " &
                                        "AND ((olNumber=@outlet AND @outlet <> '') or (@outlet='' AND (olid IN (Select Store from cld_userxstore where user = '" & usr & "'))) ) " &
                                        "AND (CustID=@custID OR @custID=0)")
            Else
                connDb.resetCmd("SELECT FORMAT(IFNULL(SUM(total),0), 2,'de_DE') AS gross " &
                                    ", FORMAT(IFNULL(SUM(subtotal- discount),0), 2,'de_DE') AS nett, IFNULL(SUM(subtotal - discount),0) AS nett2 " &
                                    ", FORMAT(IFNULL(Sum(QtyTrans),0), 0,'de_DE') AS ttlTrans, IFNULL(Sum(QtyTrans),0) AS ttlTrans2, FORMAT(IFNULL(SUM(subtotal - discount)/Sum(QtyTrans),0), 2,'de_DE') AS avg " &
                                "FROM cld_sumsalesdaily " &
                                "WHERE busiDate >='" & Tgl1.Date.ToString("yyyy-MM-01 00:00:00") & "' And busiDate <=(SELECT LAST_DAY('" & Tgl1.Date.ToString("yyyy-MM-dd 00:00:00") & "')) " &
                                    "AND (olNumber=@outlet or @outlet='') " &
                                    "AND (CustID=@custID OR @custID=0)")

            End If
            'connDb.addParam(Month(busiDate), "@bulan", MySqlDbType.Int32)
            'connDb.addParam(Year(busiDate), "@tahun", MySqlDbType.Int32)
            connDb.addParam(outletNo, "@outlet", MySqlDbType.VarChar)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)

            dsRslt = connDb.selectDataset()

            connDb.dispose()
            connDb = Nothing
        Catch ex As Exception
            errLog(Err.Description, "ClsReports:totalSales")
            Err.Clear()
        End Try

        Return dsRslt



    End Function

    'Dashboard
    Public Function dailySales(ByVal Tgl1 As Date, ByVal outletNo As String, ByVal custID As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet

        Dim dsRslt As New DataSet

        Try
            Dim connDb As New clsDbConn(ServerString)
            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                connDb.resetCmd("SELECT busidate " &
                                            ", IFNULL(SUM(subtotal-Discount),0) AS sales " &
                                         "FROM cld_sumsalesdaily " &
                                         "WHERE busiDate >='" & Tgl1.Date.ToString("yyyy-MM-01 00:00:00") & "' And busiDate <=(SELECT LAST_DAY('" & Tgl1.Date.ToString("yyyy-MM-dd 00:00:00") & "')) " &
                                            "AND ((olNumber=@outlet AND @outlet <> '') or (@outlet='' AND (olid IN (Select Store from cld_userxstore where user = '" & usr & "'))) ) " &
                                            "AND (CustID=@custID OR @custID=0) " &
                                         "GROUP BY busidate " &
                                         "ORDER BY busidate")
            Else
                connDb.resetCmd("SELECT busidate " &
                                            ", IFNULL(SUM(subtotal-Discount),0) AS sales " &
                                         "FROM cld_sumsalesdaily " &
                                         "WHERE busiDate >='" & Tgl1.Date.ToString("yyyy-MM-01 00:00:00") & "' And busiDate <=(SELECT LAST_DAY('" & Tgl1.Date.ToString("yyyy-MM-dd 00:00:00") & "')) " &
                                            "AND (olNumber=@outlet or @outlet='') " &
                                            "AND (CustID=@custID OR @custID=0) " &
                                         "GROUP BY busidate " &
                                         "ORDER BY busidate")
            End If
            'connDb.addParam(Month(busiDate), "@bulan", MySqlDbType.Int32)
            'connDb.addParam(Year(busiDate), "@tahun", MySqlDbType.Int32)
            connDb.addParam(outletNo, "@outlet", MySqlDbType.VarChar)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)

            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:dailySales")
            Err.Clear()
        End Try

        Return dsRslt


    End Function

    'Dashboard
    Public Function hourlySales(ByVal Tgl1 As Date, ByVal outletNo As String, ByVal custID As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim dsRslt As New DataSet

        Try
            Dim connDb As New clsDbConn(ServerString)
            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                connDb.resetCmd("SELECT sum(t0) as t0, sum(t1) as t1, sum(t2) as t2, sum(t3) as t3, sum(t4) as t4, sum(t5) as t5, sum(t6) as t6, " &
                                    "sum(t7) as t7, sum(t8) as t8, sum(t9) as t9, sum(t10) as t10, sum(t11) as t11, sum(t12) as t12, sum(t13) as t13, " &
                                    "sum(t14) as t14, sum(t15) as t15, sum(t16) as t16, sum(t17) as t17, sum(t18) as t18, sum(t19) as t19, " &
                                    "sum(t20) as t20, sum(t21) as t21, sum(t22) as t22, sum(t23) as t23 " &
                                "FROM cld_sumsalesdaily " &
                                "WHERE busiDate >='" & Tgl1.Date.ToString("yyyy-MM-01 00:00:00") & "' And busiDate <=(SELECT LAST_DAY('" & Tgl1.Date.ToString("yyyy-MM-dd 00:00:00") & "')) " &
                                    "AND ((olNumber=@outlet AND @outlet <> '') or (@outlet='' AND (olid IN (Select Store from cld_userxstore where user = '" & usr & "'))) ) " &
                                    "AND (CustID=@custID OR @custID=0) ")
            Else
                connDb.resetCmd("SELECT sum(t0) as t0, sum(t1) as t1, sum(t2) as t2, sum(t3) as t3, sum(t4) as t4, sum(t5) as t5, sum(t6) as t6, " &
                                    "sum(t7) as t7, sum(t8) as t8, sum(t9) as t9, sum(t10) as t10, sum(t11) as t11, sum(t12) as t12, sum(t13) as t13, " &
                                    "sum(t14) as t14, sum(t15) as t15, sum(t16) as t16, sum(t17) as t17, sum(t18) as t18, sum(t19) as t19, " &
                                    "sum(t20) as t20, sum(t21) as t21, sum(t22) as t22, sum(t23) as t23 " &
                               "FROM cld_sumsalesdaily " &
                               "WHERE busiDate >='" & Tgl1.Date.ToString("yyyy-MM-01 00:00:00") & "' And busiDate <=(SELECT LAST_DAY('" & Tgl1.Date.ToString("yyyy-MM-dd 00:00:00") & "')) AND (olNumber=@outlet or @outlet='') " &
                                   "AND (CustID=@custID OR @custID=0) ")
            End If

            'connDb.addParam(Month(busiDate), "@bulan", MySqlDbType.Int32)
            'connDb.addParam(Year(busiDate), "@tahun", MySqlDbType.Int32)
            connDb.addParam(outletNo, "@outlet", MySqlDbType.VarChar)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)

            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:hourlySales")
            Err.Clear()
        End Try

        Return dsRslt


    End Function



    'Dashboard
    Public Function WeeklySales(ByVal busiDate As Date, ByVal outletNo As String, ByVal custID As Integer, ByVal usr As String, ByVal modpriv As Integer, ByVal WeekOfMonth As String) As DataSet

        Dim dsRslt As New DataSet
        Dim sStrData As String = ""
        Dim weekofMonth2 As String = ""

        If WeekOfMonth = "" Then
            weekofMonth2 = "month(busidate) =@bulan AND "
        Else

            weekofMonth2 = "Week(Busidate) in (" & WeekOfMonth & ") AND "
        End If


        Try

            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                sStrData = "SELECT WEEK(busidate) weekofmonth,LEFT(DATE_FORMAT(busiDate,'%W'),3) AS hari,busidate,IFNULL(SUM(subtotal-discount),0) sales " &
                                     "FROM cld_sumsalesdaily " &
                                     "WHERE " & weekofMonth2 & " YEAR(BusiDate) =@tahun " &
                                             "AND ((olNumber=@outlet AND @outlet <> '') or (@outlet='' AND (olid IN (Select Store from cld_userxstore where user = '" & usr & "'))) ) " &
                                             "AND (CustID=@custID OR @custID=0) " &
                                          "GROUP BY busiDate, WEEK(Busidate) ORDER BY busidate,DAYOFWEEK(busidate)"
            Else



                sStrData = "SELECT WEEK(busidate) weekofmonth,LEFT(DATE_FORMAT(busiDate,'%W'),3) AS hari,busidate,IFNULL(SUM(subtotal-discount),0) sales " &
                                     "FROM cld_sumsalesdaily " &
                                     "WHERE " & weekofMonth2 & " YEAR(BusiDate) =@tahun " &
                                        "AND (olNumber=@outlet or @outlet='') " &
                                        "AND (CustID=@custID OR @custID=0) " &
                                     "GROUP BY busiDate, WEEK(Busidate) ORDER BY busidate,DAYOFWEEK(busidate)"

            End If

            Dim c_Cmd As New clsDbo(sStrData)
            'c_Cmd.addParam(WeekOfMonth, "@weekofmonth", MySqlDbType.VarChar)
            c_Cmd.addParam(Month(busiDate), "@bulan", MySqlDbType.Int32)
            c_Cmd.addParam(Year(busiDate), "@tahun", MySqlDbType.Int32)
            c_Cmd.addParam(outletNo, "@outlet", MySqlDbType.VarChar)
            c_Cmd.addParam(custID, "@custID", MySqlDbType.Int32)

            dsRslt = c_Cmd.selectDataset(True)

            c_Cmd.dispose()
            c_Cmd = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsPerports:WeeklySales")
            Err.Clear()
        End Try

        Return dsRslt


    End Function

    'Dashboard
    Public Function itemByAmt(ByVal busiDate As Date, ByVal outletNo As String, ByVal custID As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim dsRslt As New DataSet

        Try
            Dim connDb As New clsDbConn(ServerString)
            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                connDb.resetCmd("SELECT ItemName " &
                                        ", SUM(Qty) AS Qty " &
                                        ", SUM(Total) AS Amount " &
                                     "FROM  " &
                                        "cld_sumsalesdetaildaily " &
                                     "WHERE total>0 and busiDate >=@bulan And busiDate <=(SELECT LAST_DAY(@bulan)) " &
                                        "AND ((olNumber=@outlet AND @outlet <> '') or (@outlet='' AND (olid IN (Select Store from cld_userxstore where user = '" & usr & "'))) ) " &
                                        "AND (CustID =@custID OR @custID=0) " &
                                     "GROUP BY ItemNo, itemGroup " &
                                     "ORDER BY SUM(Total) DESC " &
                                     "Limit 10")
            Else
                connDb.resetCmd("SELECT ItemName " &
                                        ", SUM(Qty) AS Qty " &
                                        ", SUM(Total) AS Amount " &
                                     "FROM  " &
                                        "cld_sumsalesdetaildaily " &
                                     "WHERE total>0 and busiDate >=@bulan And busiDate <=(SELECT LAST_DAY(@bulan)) " &
                                        "AND (olNumber=@outlet or @outlet='') " &
                                        "AND (CustID =@custID OR @custID=0) " &
                                     "GROUP BY ItemNo, itemGroup " &
                                     "ORDER BY SUM(Total) DESC " &
                                     "Limit 10")
                'connDb.resetCmd("SELECT ItemName " &
                '                        ", SUM(Qty) AS Qty " &
                '                        ", SUM(Total) AS Amount " &
                '                     "FROM  " &
                '                        "cld_sumsalesdetaildaily " &
                '                     "WHERE total>0 and MONTH(BusiDate) =@bulan AND YEAR(BusiDate) =@tahun " &
                '                        "AND ((olNumber=@outlet AND @outlet <> '') or (@outlet='' AND (olid IN (Select Store from cld_userxstore where user = '" & usr & "'))) ) " &
                '                        "AND (CustID =@custID OR @custID=0) " &
                '                     "GROUP BY ItemName " &
                '                     "ORDER BY SUM(Total) DESC " &
                '                     "Limit 10")
                'Else
                'connDb.resetCmd("SELECT ItemName " &
                '                        ", SUM(Qty) AS Qty " &
                '                        ", SUM(Total) AS Amount " &
                '                     "FROM  " &
                '                        "cld_sumsalesdetaildaily " &
                '                     "WHERE total>0 and MONTH(BusiDate) =@bulan AND YEAR(BusiDate) =@tahun " &
                '                        "AND (olNumber=@outlet or @outlet='') " &
                '                        "AND (CustID =@custID OR @custID=0) " &
                '                     "GROUP BY ItemName " &
                '                     "ORDER BY SUM(Total) DESC " &
                '                     "Limit 10")
            End If
            connDb.addParam(busiDate, "@bulan", MySqlDbType.Int32)
            'connDb.addParam(Year(busiDate), "@tahun", MySqlDbType.Int32)
            connDb.addParam(outletNo, "@outlet", MySqlDbType.VarChar)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)

            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing
        Catch ex As Exception
            errLog(Err.Description, "ClsReports:itemByAmt")
            Err.Clear()
        End Try

        Return dsRslt


    End Function

    'Dashboard
    Public Function itemByQty(ByVal busiDate As Date, ByVal outletNo As String, ByVal custID As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim dsRslt As New DataSet

        Try
            Dim connDb As New clsDbConn(ServerString)
            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                connDb.resetCmd("SELECT ItemName " &
                                        ", SUM(Qty) AS Qty " &
                                        ", SUM(Total) AS Amount " &
                                     "FROM  " &
                                        "cld_sumsalesdetaildaily " &
                                    "WHERE total>0 and busiDate >=@bulan And busiDate <=(SELECT LAST_DAY(@bulan)) " &
                                        "AND ((olNumber=@outlet AND @outlet <> '') or (@outlet='' AND (olid IN (Select Store from cld_userxstore where user = '" & usr & "'))) ) " &
                                        "AND (CustID = @custID OR @custID =0) " &
                                     "GROUP BY ItemNo, itemGroup " &
                                     "ORDER BY SUM(Qty) DESC " &
                                     "LIMIT 10")
            Else
                connDb.resetCmd("SELECT ItemName " &
                                        ", SUM(Qty) AS Qty " &
                                        ", SUM(Total) AS Amount " &
                                     "FROM  " &
                                        "cld_sumsalesdetaildaily " &
                                     "WHERE total>0 and busiDate >=@bulan And busiDate <=(SELECT LAST_DAY(@bulan)) " &
                                        "AND (olNumber=@outlet or @outlet='') " &
                                        "AND (CustID = @custID OR @custID =0) " &
                                     "GROUP BY ItemNo, itemGroup " &
                                     "ORDER BY SUM(Qty) DESC " &
                                     "LIMIT 10")
            End If
            connDb.addParam(busiDate, "@bulan", MySqlDbType.Int32)
            'connDb.addParam(Year(busiDate), "@tahun", MySqlDbType.Int32)
            connDb.addParam(outletNo, "@outlet", MySqlDbType.VarChar)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)

            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:itemByQty")
            Err.Clear()
        End Try

        Return dsRslt


    End Function

    'Dashboard
    Private Function memberTop(ByVal busiDate As Date, ByVal outletNo As String, ByVal custID As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet

        Dim dsRslt As New DataSet

        Try
            Dim connDb As New clsDbConn(ServerString)
            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                connDb.resetCmd("SELECT sm.Memberno as number " & _
                                        ",sm.MemberName as Name " & _
                                        ",SUM(sm.TotalTrans) AS sales " & _
                                "FROM cld_sumpermember as sm  " & _
                                "WHERE MONTH(sm.BusiDate) =@bulan AND YEAR(sm.BusiDate) =@tahun " & _
                                        "AND ((sm.olNumber=@outlet AND @outlet <> '') or (@outlet='' AND (sm.olid IN (Select Store from cld_userxstore where user = '" & usr & "'))) ) " & _
                                        "AND (sm.CustID =@custID OR @custID=0) " & _
                                "GROUP BY sm.Memberno, sm.MemberName " & _
                                "ORDER BY SUM(sm.TotalTrans) DESC, sm.Memberno,  sm.MemberName " & _
                                "LIMIT 10")

            Else
                connDb.resetCmd("SELECT sm.Memberno as number " & _
                                       ",sm.MemberName as Name " & _
                                       ",SUM(sm.TotalTrans) AS sales " & _
                               "FROM cld_sumpermember as sm  " & _
                               "WHERE MONTH(sm.BusiDate) =@bulan AND YEAR(sm.BusiDate) =@tahun " & _
                                       "AND (sm.olNumber=@outlet or @outlet = '') " & _
                                       "AND (sm.CustID =@custID OR @custID=0) " & _
                               "GROUP BY sm.Memberno, sm.MemberName " & _
                               "ORDER BY SUM(sm.TotalTrans) DESC, sm.Memberno,  sm.MemberName " & _
                               "LIMIT 10")

            End If

            connDb.addParam(Month(busiDate), "@bulan", MySqlDbType.Int32)
            connDb.addParam(Year(busiDate), "@tahun", MySqlDbType.Int32)
            connDb.addParam(outletNo, "@outlet", MySqlDbType.VarChar)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)

            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing
        Catch ex As Exception
            errLog(Err.Description, "ClsReports:memberTop")
            Err.Clear()
        End Try

        Return dsRslt
    End Function

    'Revisi 08 Juni
    Private Function memberSalesSum(ByVal paramFrom As Date, ByVal paramTo As Date, ByVal outletNo As String, ByVal custID As Integer, _
                                    ByVal usr As String, ByVal modpriv As Integer, ByVal memberNo As String) As DataSet
        Dim dsRslt As New DataSet

        'perubahan 17 jan 2018,
        'subtotal = subtotal - discount
        Try
            Dim connDb As New clsDbConn(ServerString)
            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                connDb.resetCmd("SELECT m.Number " & _
                                    ",m.NAME " & _
                                    ",SUM(subtotal - discount) AS subtotal " & _
                                    ",SUM(tax) AS tax " & _
                                    ",SUM(service) AS service " & _
                                    ",SUM(discount) AS disc " & _
                                    ",SUM(sm.Total) AS total " & _
                                "FROM cld_salesmaster sm INNER JOIN " & _
                                    "cld_member m ON (m.number = sm.MemberNo and m.custid = sm.custid) " & _
                                "WHERE sm.flag=1 AND sm.busiDate >=@paramFrom AND sm.busiDate <=@paramTo " & _
                                    "AND (sm.memberNo = @memberNo or @memberNo='') " & _
                                    "AND ((sm.olNumber=@outletNo AND @outletNo <> '') or (@outletNo='' AND (sm.olid IN (Select Store from cld_userxstore where user = '" & usr & "'))) ) " & _
                                    "AND (sm.CustID=@custID OR @custID=0) AND sm.memberno <> '' " & _
                                "GROUP BY m.number,m.NAME")
            Else
                connDb.resetCmd("SELECT m.Number " & _
                                ",m.NAME " & _
                                ",SUM(subtotal - discount) AS subtotal " & _
                                ",SUM(tax) AS tax " & _
                                ",SUM(service) AS service " & _
                                ",SUM(discount) AS disc " & _
                                ",SUM(sm.Total) AS total " & _
                            "FROM cld_salesmaster sm INNER JOIN " & _
                                "cld_member m ON (m.number = sm.MemberNo and m.custid = sm.custid) " & _
                            "WHERE sm.flag=1 AND sm.busiDate >=@paramFrom AND sm.busiDate <=@paramTo " & _
                                "AND (sm.olNumber =@outletNo or @outletNo = '') " & _
                                "AND (sm.memberNo = @memberNo or @memberNo='') " & _
                                "AND (sm.CustID=@custID OR @custID=0) AND sm.memberno <> '' " & _
                            "GROUP BY m.number,m.NAME")
            End If
            connDb.addParam(paramFrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(paramTo, "@paramTo", MySqlDbType.DateTime)
            connDb.addParam(outletNo, "@outletNo", MySqlDbType.VarChar)
            connDb.addParam(memberNo, "@memberNo", MySqlDbType.String)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)

            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:memberSalesSum")
            Err.Clear()
        End Try

        Return dsRslt
    End Function

    'Revisi 08 Juni
    Private Function memberSalesDtl(ByVal paramFrom As Date, ByVal paramTo As Date, ByVal outletNo As String, ByVal custID As Integer, _
                                    ByVal usr As String, ByVal modpriv As Integer, ByVal memberNo As String) As DataSet
        Dim dsRslt As New DataSet

        Try
            Dim connDb As New clsDbConn(ServerString)
            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                connDb.resetCmd("SELECT v.Number " & _
                                    ",v.NAME " & _
                                    ",v.Transdate as busiDate " & _
                                    ",o.nama outlet " & _
                                    ",v.ItemName " & _
                                    ",v.Qty " & _
                                    ",v.ItemTotal " & _
                                "FROM v_membertrans v INNER JOIN " & _
                                    "cld_outlet o ON (v.OutletId= o.id and v.custid = o.custid) " & _
                                "WHERE v.smFlag=1 AND v.busiDate >=@paramFrom AND v.busiDate <=@paramTo " & _
                                    "AND (v.Number = @memberNo OR @memberNo='') " & _
                                    "AND ((o.kode=@outletNo AND @outletNo <> '') or (@outletNo = '' AND (o.id IN (Select Store from cld_userxstore where user = '" & usr & "'))) ) " & _
                                    "AND (v.CustID=@custID OR @custID=0) ")
            Else
                connDb.resetCmd("SELECT v.Number " & _
                                    ",v.NAME " & _
                                    ",v.Transdate as busiDate " & _
                                    ",o.nama outlet " & _
                                    ",v.ItemName " & _
                                    ",v.Qty " & _
                                    ",v.ItemTotal " & _
                                "FROM v_membertrans v INNER JOIN " & _
                                    "cld_outlet o ON (v.OutletId= o.id and v.custid = o.custid) " & _
                                "WHERE v.smFlag=1 AND v.busiDate >=@paramFrom AND v.busiDate <=@paramTo AND (o.kode =@outletNo or @outletNo = '') " & _
                                    "AND (v.Number = @memberNo OR @memberNo='') " & _
                                    "AND (v.CustID=@custID OR @custID=0) ")
            End If
            connDb.addParam(paramFrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(paramTo, "@paramTo", MySqlDbType.DateTime)
            connDb.addParam(outletNo, "@outletNo", MySqlDbType.VarChar)
            connDb.addParam(memberNo, "@memberNo", MySqlDbType.String)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)

            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:memberSalesDtl")
            Err.Clear()
        End Try

        Return dsRslt
    End Function

    'Revisi 2 feb 2017
    Public Function salesSum(ByVal paramFrom As Date, ByVal paramTo As Date, ByVal outletNo As String, ByVal MemberID As Integer, ByVal custID As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim dsRslt As New DataSet

        Try
            Dim connDb As New clsDbConn(ServerString)
            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then


                If outletNo = "" Then

                    connDb.resetCmd("SELECT DATE_FORMAT(sm.busidate,'%d-%b-%Y') as busidate " &
                                    ",sm.olnumber " &
                                     ",sm.olname " &
                                 ",SUM(sm.Subtotal - sm.Discount) AS subt " &
                                 ",SUM(sm.Tax) AS Tax " &
                                 ",SUM(sm.total) AS Total " &
                                 ",SUM(sm.cover) AS cover " &
                                 ",SUM(sm.total) / SUM(sm.cover) AS avgCover " &
                                 ",Sum(sm.QtyTrans) AS bill " &
                                 ",SUM(sm.total) / Sum(sm.QtyTrans) AS avgBill " &
                                 ", Sum(ppn) as ppn " &
                                 ", Sum(pb1) as pb1 " &
                             "FROM cld_sumsalesdaily sm " &
                             "where sm.busiDate >=@paramFrom And sm.busiDate <=@paramTo " &
                                 "And (sm.olid In (select store from cld_userxstore where user = '" & usr & "')) " &
                                 " AND (sm.CustID =@custID OR @custID=0) " &
                             "GROUP BY sm.busidate,sm.olid")

                Else

                    connDb.resetCmd("SELECT DATE_FORMAT(sm.busidate,'%d-%b-%Y') as busidate " &
                                      ",sm.olnumber " &
                                     ",sm.olname " &
                                 ",SUM(sm.Subtotal - sm.Discount) AS subt " &
                                 ",SUM(sm.Tax) AS Tax " &
                                 ",SUM(sm.total) AS Total " &
                                 ",SUM(sm.cover) AS cover " &
                                 ",SUM(sm.total) / SUM(sm.cover) AS avgCover " &
                                 ",Sum(sm.QtyTrans) AS bill " &
                                 ",SUM(sm.total) / Sum(sm.QtyTrans) AS avgBill " &
                                  ", Sum(ppn) as ppn " &
                                 ", Sum(pb1) as pb1 " &
                             "FROM cld_sumsalesdaily sm " &
                             "where sm.busiDate >=@paramFrom AND sm.busiDate <=@paramTo AND (sm.olNumber=@outletNo ) " &
                                 " AND (sm.CustID =@custID OR @custID=0) " &
                             "GROUP BY sm.busidate,sm.olid")

                End If



            Else
                connDb.resetCmd("SELECT DATE_FORMAT(sm.busidate,'%d-%b-%Y') as busidate " &
                                     ",sm.olnumber " &
                                     ",sm.olname " &
                                   ",SUM(sm.Subtotal - sm.Discount) AS subt " &
                                   ",SUM(sm.Tax) AS Tax " &
                                   ",SUM(sm.total) AS Total " &
                                   ",SUM(sm.cover) AS cover " &
                                   ",SUM(sm.total) / SUM(sm.cover) AS avgCover " &
                                   ",Sum(sm.QtyTrans) AS bill " &
                                   ",SUM(sm.total) / Sum(sm.QtyTrans) AS avgBill " &
                                    ", Sum(ppn) as ppn " &
                                 ", Sum(pb1) as pb1 " &
                               "FROM cld_sumsalesdaily sm " &
                               "where sm.busiDate >=@paramFrom AND sm.busiDate <=@paramTo AND (sm.olNumber=@outletNo or @outletNo ='') " &
                                   " AND (sm.CustID =@custID OR @custID=0) " &
                               "GROUP BY sm.busidate,sm.olid")

            End If

            connDb.addParam(paramFrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(paramTo, "@paramTo", MySqlDbType.DateTime)

            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If outletNo <> "" Then
                    connDb.addParam(outletNo, "@outletNo", MySqlDbType.VarChar)
                End If
            Else
                connDb.addParam(outletNo, "@outletNo", MySqlDbType.VarChar)
            End If

            ' connDb.addParam(MemberID, "@memberID", MySqlDbType.Int32)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)

            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:salesSum")
            Err.Clear()
        End Try

        Return dsRslt
    End Function



    'revisi 23apr2018
    Public Function salesSum_gv(ByVal custID As Integer, ByVal dfrom As Date, ByVal dTo As Date, ByVal sOutlet As String, ByVal iOwn As String, ByVal iBrand As String, ByVal iCity As String, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim dsRslt As New DataSet
        Dim sOut As String = ""
        Dim sBrand As String = ""
        Dim sOwn As String = ""
        Dim sCity As String = ""

        Try
            Dim connDb As New clsDbConn(ServerString)



            If iBrand = "0" Or iBrand = "" Then
                sBrand = ""
            Else
                sBrand = " and o.brandID in (" & iBrand & ")"
            End If

            If iOwn = "0" Or iOwn = "" Then
                sOwn = ""
            Else
                sOwn = " and o.ownID = " & iOwn
            End If
            If iCity = "0" Or iCity = "" Then
                sCity = ""
            Else
                sCity = " and o.cityID in (" & iCity & ")"
            End If

            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If sOutlet = "" Then
                    sOut = " and sm.olId in (Select Store From cld_userxstore where user = '" & usr & "')"
                Else
                    sOut = " and sm.olnumber in (" & sOutlet & ")"
                End If
            Else
                If sOutlet = "" Then
                    sOut = ""
                Else
                    sOut = " and sm.olnumber in (" & sOutlet & ")"
                End If
            End If

            'dengan ppn
            '            connDb.resetCmd("(SELECT DATE_FORMAT(sm.busidate,'%d-%b-%Y') 'Sales Date' ,sm.olnumber as 'Store Code' ,sm.olname as 'Store Name' 
            ',FORMAT((IFNULL((SELECT SUM(total) FROM cld_sumsalesdetaildaily sd WHERE (sd.ItemGroup<>'MERCHANDISE') AND(sd.olnumber = sm.olnumber) AND (sd.busidate = sm.busidate)),0)), 2,'de_DE') 'Restaurant' 
            ',FORMAT(Sum(pb1), 2,'de_DE') as Pb1
            ',FORMAT((IFNULL((SELECT SUM(total) FROM cld_sumsalesdetaildaily sd WHERE (sd.ItemGroup='MERCHANDISE') AND(sd.olnumber = sm.olnumber) AND (sd.busidate = sm.busidate)),0)), 2,'de_DE') 'Other Revenue'
            ',FORMAT(SUM(sm.service), 2,'de_DE') 'Service atau Delivery Charge', FORMAT(Sum(ppn), 2,'de_DE') as Ppn,
            'FORMAT(SUM(sm.total), 2,'de_DE') 'Total Sales' ,FORMAT(SUM(sm.cover), 0,'de_DE') AS 'Number Of Cover' ,
            'FORMAT(SUM(sm.total) / SUM(sm.cover), 2,'de_DE') AS 'Avg Sales Per Cover' ,FORMAT(Sum(sm.QtyTrans), 0,'de_DE') AS 'Number Of Transaction' 
            ',FORMAT(SUM(sm.total) / Sum(sm.QtyTrans), 2,'de_DE') AS 'Avg Sales Per Transaction'  
            'FROM cld_sumsalesdaily sm INNER JOIN cld_outlet o ON sm.CustID = o.custID AND sm.olId = o.id " &
            '                     "where sm.busiDate >=@paramFrom And sm.busiDate <=@paramTo " &
            '                     " AND (sm.CustID =@custID OR @custID=0) " & sOut & sOwn & sBrand & sCity & " GROUP BY sm.busidate, sm.olid Order BY sm.olid,sm.busidate) " &
            '                      " UNION ALL " &
            '                     "SELECT '' " &
            '                         ",'' " &
            '                         ",'Total' olname " &
            '                         ",FORMAT((IFNULL((SELECT SUM(total) FROM cld_sumsalesdetaildaily sd WHERE (sd.ItemGroup<>'MERCHANDISE') AND(sd.olnumber = sm.olnumber) AND (sd.busidate = sm.busidate)),0)), 2,'de_DE') 'Restaurant' " &
            '                         ",FORMAT(Sum(pb1), 2,'de_DE') as Pb1 " &
            '                         ",FORMAT((IFNULL((SELECT SUM(total) FROM cld_sumsalesdetaildaily sd WHERE (sd.ItemGroup='MERCHANDISE') AND(sd.olnumber = sm.olnumber) AND (sd.busidate = sm.busidate)),0)), 2,'de_DE') " &
            '                         ",FORMAT(SUM(sm.service), 2,'de_DE') 'Service atau Delivery Charge', FORMAT(Sum(ppn), 2,'de_DE') as Ppn,
            'FORMAT(SUM(sm.total), 2,'de_DE') 'Total Sales' ,FORMAT(SUM(sm.cover), 0,'de_DE') AS 'Number Of Cover' ,
            'FORMAT(SUM(sm.total) / SUM(sm.cover), 2,'de_DE') AS 'Avg Sales Per Cover' ,FORMAT(Sum(sm.QtyTrans), 0,'de_DE') AS 'Number Of Transaction' 
            ',FORMAT(SUM(sm.total) / Sum(sm.QtyTrans), 2,'de_DE') AS 'Avg Sales Per Transaction' " &
            '                     "FROM cld_sumsalesdaily sm INNER JOIN cld_outlet o " &
            '                    "On sm.CustID = o.custID And sm.olId = o.id " &
            '                     "where sm.busiDate >=@paramFrom And sm.busiDate <=@paramTo " &
            '                     " And (sm.CustID =@custID Or @custID=0) " & sOut & sOwn & sBrand & sCity)


            connDb.resetCmd("(SELECT DATE_FORMAT(sm.busidate,'%d-%b-%Y') 'Sales Date' ,sm.olnumber as 'Store Code' ,sm.olname as 'Store Name' 
            ,FORMAT(SUM(sm.Subtotal - sm.Discount), 2,'de_DE') 'Restaurant Revenue' ,FORMAT(Sum(pb1), 2,'de_DE') as Pb1 ,FORMAT(SUM(sm.OtherRevenue), 2,'de_DE') 'Other Revenue', FORMAT(Sum(ppn), 2,'de_DE') as Ppn,
            FORMAT(SUM(sm.service - sm.DeliveryFee), 2,'de_DE') 'Service Charge',FORMAT(SUM(sm.DeliveryFee), 2,'de_DE') 'Delivery Charge',
            FORMAT(SUM(sm.total), 2,'de_DE') 'Total Sales' ,FORMAT(SUM(sm.cover), 0,'de_DE') AS 'Number Of Cover' ,
            FORMAT(SUM(sm.total) / SUM(sm.cover), 2,'de_DE') AS 'Avg Sales Per Cover' ,FORMAT(Sum(sm.QtyTrans), 0,'de_DE') AS 'Number Of Transaction' 
            ,FORMAT(SUM(sm.total) / Sum(sm.QtyTrans), 2,'de_DE') AS 'Avg Sales Per Transaction'  
            FROM cld_sumsalesdaily sm INNER JOIN cld_outlet o ON sm.CustID = o.custID AND sm.olId = o.id " &
                     "where sm.busiDate >=@paramFrom And sm.busiDate <=@paramTo " &
                     " AND (sm.CustID =@custID OR @custID=0) " & sOut & sOwn & sBrand & sCity & " GROUP BY sm.busidate, sm.olid Order BY sm.olid,sm.busidate) " &
                      " UNION ALL " &
                     "SELECT '' " &
                         ",'' " &
                         ",'Total' olname " &
                         ",FORMAT(SUM(sm.Subtotal - sm.Discount), 2,'de_DE')‎‎ " &
                         ",FORMAT(Sum(pb1), 2,'de_DE') as Pb1, FORMAT(SUM(sm.OtherRevenue), 2,'de_DE') 'Other Revenue', FORMAT(Sum(ppn), 2,'de_DE') as Ppn,
                         FORMAT(SUM(sm.service - sm.DeliveryFee), 2,'de_DE') 'Service Charge',FORMAT(SUM(sm.DeliveryFee), 2,'de_DE') 'Delivery Charge',
            FORMAT(SUM(sm.total), 2,'de_DE') 'Total Sales' ,FORMAT(SUM(sm.cover), 0,'de_DE') AS 'Number Of Cover' ,
            FORMAT(SUM(sm.total) / SUM(sm.cover), 2,'de_DE') AS 'Avg Sales Per Cover' ,FORMAT(Sum(sm.QtyTrans), 0,'de_DE') AS 'Number Of Transaction' 
            ,FORMAT(SUM(sm.total) / Sum(sm.QtyTrans), 2,'de_DE') AS 'Avg Sales Per Transaction' " &
                     "FROM cld_sumsalesdaily sm INNER JOIN cld_outlet o " &
                    "On sm.CustID = o.custID And sm.olId = o.id " &
                     "where sm.busiDate >=@paramFrom And sm.busiDate <=@paramTo " &
                     " And (sm.CustID =@custID Or @custID=0) " & sOut & sOwn & sBrand & sCity)

            'connDb.resetCmd("(Select DATE_FORMAT(sm.busidate,'%d-%b-%Y') 'Sales Date' ,sm.olnumber as 'Store Code' ,sm.olname as 'Store Name' 
            ',FORMAT(SUM(sm.Subtotal - sm.Discount), 2,'de_DE') 'Net Sales' ,FORMAT(SUM(sm.Tax), 2,'de_DE') AS Tax ,
            'FORMAT(SUM(sm.total), 2,'de_DE') 'Total Sales' ,FORMAT(SUM(sm.cover), 0,'de_DE') AS 'Number Of Cover' ,
            'FORMAT(SUM(sm.total) / SUM(sm.cover), 2,'de_DE') AS 'Avg Sales Per Cover' ,FORMAT(Sum(sm.QtyTrans), 0,'de_DE') AS 'Number Of Transaction' 
            ',FORMAT(SUM(sm.total) / Sum(sm.QtyTrans), 2,'de_DE') AS 'Avg Sales Per Transaction' , FORMAT(Sum(ppn), 2,'de_DE') as Ppn , 
            'FORMAT(Sum(pb1), 2,'de_DE') as Pb1
            'FROM cld_sumsalesdaily sm INNER JOIN cld_outlet o ON sm.CustID = o.custID AND sm.olId = o.id " &
            '         "where sm.busiDate >=@paramFrom And sm.busiDate <=@paramTo " &
            '         " AND (sm.CustID =@custID OR @custID=0) " & sOut & sOwn & sBrand & sCity & " GROUP BY sm.busidate, sm.olid Order BY sm.olid,sm.busidate) " &
            '          " UNION ALL " &
            '         "SELECT '' " &
            '             ",'' " &
            '             ",'Total' olname " &
            '             ",FORMAT(SUM(sm.Subtotal - sm.Discount), 2,'de_DE')‎‎ " &
            '             ",FORMAT(SUM(sm.Tax), 2,'de_DE') AS Tax " &
            '             ",FORMAT(SUM(sm.total), 2,'de_DE') AS Ttl_Sales‎ " &
            '             ",FORMAT(SUM(sm.cover), 0,'de_DE') AS Ttl_Cover " &
            '             ",'' AS Avg_PerCover " &
            '             ",FORMAT(Sum(sm.QtyTrans), 0,'de_DE') AS Ttl_Check " &
            '             ",'' AS Avg_PerCheck " &
            '             ", FORMAT(Sum(ppn), 2,'de_DE') as ppn " &
            '             ", FORMAT(Sum(pb1), 2,'de_DE') as pb1 " &
            '         "FROM cld_sumsalesdaily sm INNER JOIN cld_outlet o " &
            '        "ON sm.CustID = o.custID AND sm.olId = o.id " &
            '         "where sm.busiDate >=@paramFrom And sm.busiDate <=@paramTo " &
            '         " AND (sm.CustID =@custID OR @custID=0) " & sOut & sOwn & sBrand & sCity)

            'connDb.resetCmd("(SELECT DATE_FORMAT(sm.busidate,'%d-%b-%Y') 'Sales Date' ,sm.olnumber as 'Store Code' ,sm.olname as 'Store Name' 
            ',FORMAT(SUM(sm.Subtotal - sm.Discount), 2,'de_DE') 'Net Sales' ,FORMAT(SUM(sm.Tax), 2,'de_DE') AS Tax ,
            'FORMAT(SUM(sm.total), 2,'de_DE') 'Total Sales' ,FORMAT(SUM(sm.cover), 0,'de_DE') AS 'Total Cover' ,
            'FORMAT(SUM(sm.total) / SUM(sm.cover), 2,'de_DE') AS 'AVG per Cover' ,FORMAT(Sum(sm.QtyTrans), 0,'de_DE') AS 'Total Check' 
            ',FORMAT(SUM(sm.total) / Sum(sm.QtyTrans), 2,'de_DE') AS 'AVG per Check' , FORMAT(Sum(ppn), 2,'de_DE') as PPN , 
            'FORMAT(Sum(pb1), 2,'de_DE') as PB1
            ', FORMAT(Sum(RoundOff), 2,'de_DE') as Rounding 
            'FROM cld_sumsalesdaily sm INNER JOIN cld_outlet o ON sm.CustID = o.custID AND sm.olId = o.id " &
            '         "where sm.busiDate >=@paramFrom And sm.busiDate <=@paramTo " &
            '         " AND (sm.CustID =@custID OR @custID=0) " & sOut & sOwn & sBrand & sCity & " GROUP BY sm.busidate, sm.olid Order BY sm.olid,sm.busidate) " &
            '          " UNION ALL " &
            '         "SELECT '' " &
            '             ",'' " &
            '             ",'Total' olname " &
            '             ",FORMAT(SUM(sm.Subtotal - sm.Discount), 2,'de_DE')‎‎ " &
            '             ",FORMAT(SUM(sm.Tax), 2,'de_DE') AS Tax " &
            '             ",FORMAT(SUM(sm.total), 2,'de_DE') AS Ttl_Sales‎ " &
            '             ",FORMAT(SUM(sm.cover), 0,'de_DE') AS Ttl_Cover " &
            '             ",'' AS Avg_PerCover " &
            '             ",FORMAT(Sum(sm.QtyTrans), 0,'de_DE') AS Ttl_Check " &
            '             ",'' AS Avg_PerCheck " &
            '             ", '' as ppn " &
            '             ", FORMAT(Sum(pb1), 2,'de_DE') as pb1 " &
            '              ", FORMAT(Sum(RoundOff), 0,'de_DE') as Rounding " &
            '         "FROM cld_sumsalesdaily sm INNER JOIN cld_outlet o " &
            '        "ON sm.CustID = o.custID AND sm.olId = o.id " &
            '         "where sm.busiDate >=@paramFrom And sm.busiDate <=@paramTo " &
            '         " AND (sm.CustID =@custID OR @custID=0) " & sOut & sOwn & sBrand & sCity)




            connDb.addParam(dfrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(dTo, "@paramTo", MySqlDbType.DateTime)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)
            ' connDb.addParam(MemberID, "@memberID", MySqlDbType.Int32)


            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:salesSum_gv")
            Err.Clear()
        End Try

        Return dsRslt
    End Function

    Public Function NonsalesSum_gv(ByVal custID As Integer, ByVal dfrom As Date, ByVal dTo As Date, ByVal sOutlet As String, ByVal iOwn As String, ByVal iBrand As String, ByVal iCity As String, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim dsRslt As New DataSet
        Dim sOut As String = ""
        Dim sBrand As String = ""
        Dim sOwn As String = ""
        Dim sCity As String = ""

        Try
            Dim connDb As New clsDbConn(ServerString)



            If iBrand = "0" Or iBrand = "" Then
                sBrand = ""
            Else
                sBrand = " and o.brandID in (" & iBrand & ")"
            End If

            If iOwn = "0" Or iOwn = "" Then
                sOwn = ""
            Else
                sOwn = " and o.ownID = " & iOwn
            End If
            If iCity = "0" Or iCity = "" Then
                sCity = ""
            Else
                sCity = " and o.cityID in (" & iCity & ")"
            End If

            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If sOutlet = "" Then
                    sOut = " and sm.olId in (Select Store From cld_userxstore where user = '" & usr & "')"
                Else
                    sOut = " and sm.olnumber in (" & sOutlet & ")"
                End If
            Else
                If sOutlet = "" Then
                    sOut = ""
                Else
                    sOut = " and sm.olnumber in (" & sOutlet & ")"
                End If
            End If

            connDb.resetCmd("(SELECT DATE_FORMAT(sm.busidate,'%d-%b-%Y') 'Sales Date' ,sm.olnumber as 'Store Code' ,sm.olname as 'Store Name' 
            ,FORMAT(SUM(sm.Subtotal - sm.Discount), 2,'de_DE') 'Restaurant Revenue' ,FORMAT(Sum(pb1), 2,'de_DE') as Pb1 ,FORMAT(SUM(sm.service), 2,'de_DE') 'Service & Delivery Charge', FORMAT(Sum(ppn), 2,'de_DE') as Ppn,
            FORMAT(SUM(sm.total), 2,'de_DE') 'Total Sales' ,FORMAT(SUM(sm.cover), 0,'de_DE') AS 'Number Of Cover' ,
            FORMAT(SUM(sm.total) / SUM(sm.cover), 2,'de_DE') AS 'Avg Sales Per Cover' ,FORMAT(Sum(sm.QtyTrans), 0,'de_DE') AS 'Number Of Transaction' 
            ,FORMAT(SUM(sm.total) / Sum(sm.QtyTrans), 2,'de_DE') AS 'Avg Sales Per Transaction'  
            FROM cld_sumnosalesdaily sm INNER JOIN cld_outlet o ON sm.CustID = o.custID AND sm.olId = o.id " &
                     "where sm.busiDate >=@paramFrom And sm.busiDate <=@paramTo " &
                     " AND (sm.CustID =@custID OR @custID=0) " & sOut & sOwn & sBrand & sCity & " GROUP BY sm.busidate, sm.olid Order BY sm.olid,sm.busidate) " &
                      " UNION ALL " &
                     "SELECT '' " &
                         ",'' " &
                         ",'Total' olname " &
                         ",FORMAT(SUM(sm.Subtotal - sm.Discount), 2,'de_DE')‎‎ " &
                         ",FORMAT(Sum(pb1), 2,'de_DE') as Pb1 ,FORMAT(SUM(sm.service), 2,'de_DE') 'Service atau Delivery Charge', FORMAT(Sum(ppn), 2,'de_DE') as Ppn,
            FORMAT(SUM(sm.total), 2,'de_DE') 'Total Sales' ,FORMAT(SUM(sm.cover), 0,'de_DE') AS 'Number Of Cover' ,
            FORMAT(SUM(sm.total) / SUM(sm.cover), 2,'de_DE') AS 'Avg Sales Per Cover' ,FORMAT(Sum(sm.QtyTrans), 0,'de_DE') AS 'Number Of Transaction' 
            ,FORMAT(SUM(sm.total) / Sum(sm.QtyTrans), 2,'de_DE') AS 'Avg Sales Per Transaction' " &
                     "FROM cld_sumnosalesdaily sm INNER JOIN cld_outlet o " &
                    "On sm.CustID = o.custID And sm.olId = o.id " &
                     "where sm.busiDate >=@paramFrom And sm.busiDate <=@paramTo " &
                     " And (sm.CustID =@custID Or @custID=0) " & sOut & sOwn & sBrand & sCity)

            connDb.addParam(dfrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(dTo, "@paramTo", MySqlDbType.DateTime)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)
            ' connDb.addParam(MemberID, "@memberID", MySqlDbType.Int32)


            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:NonsalesSum_gv")
            Err.Clear()
        End Try

        Return dsRslt
    End Function

    Public Function salesSum2(ByVal custID As Integer, ByVal dfrom As Date, ByVal dTo As Date, ByVal sOutlet As String, ByVal iOwn As String, ByVal iBrand As String, ByVal iCity As String, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim dsRslt As New DataSet
        Dim sOut As String = ""
        Dim sBrand As String = ""
        Dim sOwn As String = ""
        Dim sCity As String = ""

        Try
            Dim connDb As New clsDbConn(ServerString)



            If iBrand = "0" Or iBrand = "" Then
                sBrand = ""
            Else
                sBrand = " and o.brandID in (" & iBrand & ")"
            End If

            If iOwn = "0" Or iOwn = "" Then
                sOwn = ""
            Else
                sOwn = " and o.ownID = " & iOwn
            End If
            If iCity = "0" Or iCity = "" Then
                sCity = ""
            Else
                sCity = " and o.cityID in (" & iCity & ")"
            End If

            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If sOutlet = "" Then
                    sOut = " and sm.olId in (Select Store From cld_userxstore where user = '" & usr & "')"
                Else
                    sOut = " and sm.olnumber in (" & sOutlet & ")"
                End If
            Else
                If sOutlet = "" Then
                    sOut = ""
                Else
                    sOut = " and sm.olnumber in (" & sOutlet & ")"
                End If
            End If

            'dengan ppn
            '            connDb.resetCmd("(SELECT DATE_FORMAT(sm.busidate,'%d-%b-%Y') 'Sales Date' ,sm.olnumber as 'Store Code' ,sm.olname as 'Store Name' 
            ',(IFNULL((SELECT SUM(total) FROM cld_sumsalesdetaildaily sd WHERE (sd.ItemGroup<>'MERCHANDISE') AND(sd.olnumber = sm.olnumber) AND (sd.busidate = sm.busidate)),0)) 'Restaurant' 
            ',Sum(pb1) as Pb1
            ',(IFNULL((SELECT SUM(total) FROM cld_sumsalesdetaildaily sd WHERE (sd.ItemGroup='MERCHANDISE') AND(sd.olnumber = sm.olnumber) AND (sd.busidate = sm.busidate)),0)) 'Other Revenue'
            ',SUM(sm.service) 'Service atau Delivery Charge', Sum(ppn) as Ppn,
            'SUM(sm.total) 'Total Sales' ,SUM(sm.cover) AS 'Number Of Cover' ,
            'SUM(sm.total) / SUM(sm.cover) AS 'Avg Sales Per Cover' ,Sum(sm.QtyTrans) AS 'Number Of Transaction' 
            ',SUM(sm.total) / Sum(sm.QtyTrans) AS 'Avg Sales Per Transaction'  
            'FROM cld_sumsalesdaily sm INNER JOIN cld_outlet o ON sm.CustID = o.custID AND sm.olId = o.id " &
            '                     "where sm.busiDate >=@paramFrom And sm.busiDate <=@paramTo " &
            '                     " AND (sm.CustID =@custID OR @custID=0) " & sOut & sOwn & sBrand & sCity & " GROUP BY sm.busidate, sm.olid Order BY sm.olid,sm.busidate) " &
            '                      " UNION ALL " &
            '                     "SELECT '' " &
            '                         ",'' " &
            '                         ",'Total' olname " &
            '                         ",(IFNULL((SELECT SUM(total) FROM cld_sumsalesdetaildaily sd WHERE (sd.ItemGroup<>'MERCHANDISE') AND(sd.olnumber = sm.olnumber) AND (sd.busidate = sm.busidate)),0)) 'Restaurant' " &
            '                         ",Sum(pb1) as Pb1 " &
            '                         ",(IFNULL((SELECT SUM(total) FROM cld_sumsalesdetaildaily sd WHERE (sd.ItemGroup='MERCHANDISE') AND(sd.olnumber = sm.olnumber) AND (sd.busidate = sm.busidate)),0)) " &
            '                         ",SUM(sm.service) 'Service atau Delivery Charge', Sum(ppn) as Ppn,
            'SUM(sm.total) 'Total Sales' ,SUM(sm.cover) AS 'Number Of Cover' ,
            'SUM(sm.total) / SUM(sm.cover) AS 'Avg Sales Per Cover' ,Sum(sm.QtyTrans) AS 'Number Of Transaction' 
            ',SUM(sm.total) / Sum(sm.QtyTrans) AS 'Avg Sales Per Transaction' " &
            '                     "FROM cld_sumsalesdaily sm INNER JOIN cld_outlet o " &
            '                    "On sm.CustID = o.custID And sm.olId = o.id " &
            '                     "where sm.busiDate >=@paramFrom And sm.busiDate <=@paramTo " &
            '                     " And (sm.CustID =@custID Or @custID=0) " & sOut & sOwn & sBrand & sCity)

            connDb.resetCmd("SELECT DATE_FORMAT(sm.busidate,'%d-%b-%Y') 'Sales Date' ,sm.olnumber as 'Store Code' ,sm.olname as 'Store Name' 
            ,SUM(sm.Subtotal - sm.Discount) 'Restaurant Revenue' ,Sum(pb1) as Pb1 ,SUM(sm.OtherRevenue) 'Other Revenue', Sum(ppn) as Ppn,
            SUM(sm.service - sm.DeliveryFee) 'Service Charge', SUM(sm.DeliveryFee) 'Delivery Charge',
            SUM(sm.total) 'Total Sales' ,SUM(sm.cover) AS 'Number Of Cover' ,
            SUM(sm.total) / SUM(sm.cover) AS 'Avg Sales Per Cover' ,Sum(sm.QtyTrans) AS 'Number Of Transaction' 
            ,SUM(sm.total) / Sum(sm.QtyTrans) AS 'Avg Sales Per Transaction'  
            FROM cld_sumsalesdaily sm INNER JOIN cld_outlet o ON sm.CustID = o.custID AND sm.olId = o.id " &
                     "where sm.busiDate >=@paramFrom And sm.busiDate <=@paramTo " &
                     " AND (sm.CustID =@custID OR @custID=0) " & sOut & sOwn & sBrand & sCity & " GROUP BY sm.busidate, sm.olid Order BY sm.busidate,sm.olid ")



            '            connDb.resetCmd("(SELECT DATE_FORMAT(sm.busidate,'%d-%b-%Y') 'Sales Date' ,sm.olnumber as 'Store Code' ,sm.olname as 'Store Name' 
            ',SUM(sm.Subtotal - sm.Discount) 'Restaurant' ,Sum(pb1) as Pb1 ,SUM(sm.service) 'Service atau Delivery Charge', Sum(ppn) as Ppn,
            'SUM(sm.total) 'Total Sales' ,SUM(sm.cover) AS 'Number Of Cover' ,
            'SUM(sm.total) / SUM(sm.cover) AS 'Avg Sales Per Cover' ,Sum(sm.QtyTrans) AS 'Number Of Transaction' 
            ',SUM(sm.total) / Sum(sm.QtyTrans) AS 'Avg Sales Per Transaction'  
            'FROM cld_sumsalesdaily sm INNER JOIN cld_outlet o ON sm.CustID = o.custID AND sm.olId = o.id " &
            '                     "where sm.busiDate >=@paramFrom And sm.busiDate <=@paramTo " &
            '                     " AND (sm.CustID =@custID OR @custID=0) " & sOut & sOwn & sBrand & sCity & " GROUP BY sm.busidate, sm.olid Order BY sm.olid,sm.busidate) " &
            '                      " UNION ALL " &
            '                     "SELECT '' " &
            '                         ",'' " &
            '                         ",'Total' olname " &
            '                         ",SUM(sm.Subtotal - sm.Discount)‎‎ " &
            '                         ",Sum(pb1) as Pb1 ,SUM(sm.service) 'Service atau Delivery Charge', Sum(ppn) as Ppn,
            'SUM(sm.total) 'Total Sales' ,SUM(sm.cover) AS 'Number Of Cover' ,
            'SUM(sm.total) / SUM(sm.cover) AS 'Avg Sales Per Cover' ,Sum(sm.QtyTrans) AS 'Number Of Transaction' 
            ',SUM(sm.total) / Sum(sm.QtyTrans) AS 'Avg Sales Per Transaction' " &
            '                     "FROM cld_sumsalesdaily sm INNER JOIN cld_outlet o " &
            '                    "On sm.CustID = o.custID And sm.olId = o.id " &
            '                     "where sm.busiDate >=@paramFrom And sm.busiDate <=@paramTo " &
            '                     " And (sm.CustID =@custID Or @custID=0) " & sOut & sOwn & sBrand & sCity)
            '            connDb.resetCmd("(SELECT DATE_FORMAT(sm.busidate,'%d-%b-%Y') 'Sales Date' ,sm.olnumber as 'Store Code' ,sm.olname as 'Store Name' 
            ',SUM(sm.Subtotal - sm.Discount) 'Net Sales' ,SUM(sm.Tax) AS Tax ,
            'SUM(sm.total) 'Total Sales' ,SUM(sm.cover) 'Number Of Cover' ,
            'SUM(sm.total) / SUM(sm.cover) AS 'Avg Sales Per Cover' ,Sum(sm.QtyTrans) AS 'Number Of Transaction' 
            ',SUM(sm.total) / Sum(sm.QtyTrans) AS 'Avg Sales Per Transaction' , Sum(ppn) as Ppn , 
            'Sum(pb1) as Pb1
            'FROM cld_sumsalesdaily sm INNER JOIN cld_outlet o ON sm.CustID = o.custID AND sm.olId = o.id " &
            '                     "where sm.busiDate >=@paramFrom And sm.busiDate <=@paramTo " &
            '                     " AND (sm.CustID =@custID OR @custID=0) " & sOut & sOwn & sBrand & sCity & " GROUP BY sm.busidate, sm.olid Order BY sm.olid,sm.busidate) " &
            '                      " UNION ALL " &
            '                     "SELECT '' " &
            '                         ",'' " &
            '                         ",'Total' olname " &
            '                         ",SUM(sm.Subtotal - sm.Discount)‎‎ " &
            '                         ",SUM(sm.Tax) AS Tax " &
            '                         ",SUM(sm.total) AS Ttl_Sales‎ " &
            '                         ",SUM(sm.cover) AS Ttl_Cover " &
            '                         ",'' AS Avg_PerCover " &
            '                         ",Sum(sm.QtyTrans) AS Ttl_Check " &
            '                         ",'' AS Avg_PerCheck " &
            '                         ", Sum(ppn) as ppn " &
            '                         ", Sum(pb1) as pb1 " &
            '                     "FROM cld_sumsalesdaily sm INNER JOIN cld_outlet o " &
            '                    "ON sm.CustID = o.custID AND sm.olId = o.id " &
            '                     "where sm.busiDate >=@paramFrom And sm.busiDate <=@paramTo " &
            '                     " AND (sm.CustID =@custID OR @custID=0) " & sOut & sOwn & sBrand & sCity)



            connDb.addParam(dfrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(dTo, "@paramTo", MySqlDbType.DateTime)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)
            ' connDb.addParam(MemberID, "@memberID", MySqlDbType.Int32)


            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:salesSum2")
            Err.Clear()
        End Try

        Return dsRslt
    End Function

    Public Function NonsalesSum2(ByVal custID As Integer, ByVal dfrom As Date, ByVal dTo As Date, ByVal sOutlet As String, ByVal iOwn As String, ByVal iBrand As String, ByVal iCity As String, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim dsRslt As New DataSet
        Dim sOut As String = ""
        Dim sBrand As String = ""
        Dim sOwn As String = ""
        Dim sCity As String = ""

        Try
            Dim connDb As New clsDbConn(ServerString)



            If iBrand = "0" Or iBrand = "" Then
                sBrand = ""
            Else
                sBrand = " and o.brandID in (" & iBrand & ")"
            End If

            If iOwn = "0" Or iOwn = "" Then
                sOwn = ""
            Else
                sOwn = " and o.ownID = " & iOwn
            End If
            If iCity = "0" Or iCity = "" Then
                sCity = ""
            Else
                sCity = " and o.cityID in (" & iCity & ")"
            End If

            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If sOutlet = "" Then
                    sOut = " and sm.olId in (Select Store From cld_userxstore where user = '" & usr & "')"
                Else
                    sOut = " and sm.olnumber in (" & sOutlet & ")"
                End If
            Else
                If sOutlet = "" Then
                    sOut = ""
                Else
                    sOut = " and sm.olnumber in (" & sOutlet & ")"
                End If
            End If


            connDb.resetCmd("SELECT DATE_FORMAT(sm.busidate,'%d-%b-%Y') 'Sales Date' ,sm.olnumber as 'Store Code' ,sm.olname as 'Store Name' 
            ,SUM(sm.Subtotal - sm.Discount) 'Restaurant Revenue' ,Sum(pb1) as Pb1 ,SUM(sm.service) 'Service & Delivery Charge', Sum(ppn) as Ppn,
            SUM(sm.total) 'Total Sales' ,SUM(sm.cover) AS 'Number Of Cover' ,
            SUM(sm.total) / SUM(sm.cover) AS 'Avg Sales Per Cover' ,Sum(sm.QtyTrans) AS 'Number Of Transaction' 
            ,SUM(sm.total) / Sum(sm.QtyTrans) AS 'Avg Sales Per Transaction'  
            FROM cld_sumnosalesdaily sm INNER JOIN cld_outlet o ON sm.CustID = o.custID AND sm.olId = o.id " &
                     "where sm.busiDate >=@paramFrom And sm.busiDate <=@paramTo " &
                     " AND (sm.CustID =@custID OR @custID=0) " & sOut & sOwn & sBrand & sCity & " GROUP BY sm.busidate, sm.olid Order BY sm.olid,sm.busidate ")

            connDb.addParam(dfrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(dTo, "@paramTo", MySqlDbType.DateTime)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)
            ' connDb.addParam(MemberID, "@memberID", MySqlDbType.Int32)


            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:NonsalesSum2")
            Err.Clear()
        End Try

        Return dsRslt
    End Function

    Public Function salesSumMember(ByVal paramFrom As Date, ByVal paramTo As Date, ByVal outletNo As String, ByVal MemberID As Integer, ByVal custID As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim dsRslt As New DataSet

        Try
            Dim connDb As New clsDbConn(ServerString)
            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then


                connDb.resetCmd("SELECT busidate " & _
                                  ",SUM(sm.total) AS Total " & _
                                  ",SUM(sm.cover) AS cover " & _
                                  ",SUM(sm.total) / SUM(sm.cover) AS avgCover " & _
                                  ",COUNT(*) AS bill " & _
                                  ",SUM(total) / COUNT(*) AS avgBill " & _
                              "FROM cld_salesmaster sm INNER join cld_member m on (m.Number = sm.MemberNo and m.Custid = sm.custid)" & _
                              "where sm.Flag=1 AND sm.busiDate >=@paramFrom AND sm.busiDate <=@paramTo " & _
                                    "AND ((sm.olNumber=@outletNo AND @outletNo <> '') or (@outletNo = '' AND (sm.olid IN (Select Store from cld_userxstore where user = '" & usr & "'))) ) " & _
                                    " AND (m.ID =@memberID) AND (sm.CustID =@custID) AND sm.memberno <> '' " & _
                              "GROUP BY sm.busidate")
            Else


                connDb.resetCmd("SELECT busidate " & _
                                 ",SUM(sm.total) AS Total " & _
                                 ",SUM(sm.cover) AS cover " & _
                                 ",SUM(sm.total) / SUM(sm.cover) AS avgCover " & _
                                 ",COUNT(*) AS bill " & _
                                 ",SUM(total) / COUNT(*) AS avgBill " & _
                             "FROM cld_salesmaster sm INNER join cld_member m on (m.Number = sm.MemberNo and m.Custid = sm.custid)" & _
                             "where sm.Flag=1 AND sm.busiDate >=@paramFrom AND sm.busiDate <=@paramTo AND (sm.olNumber=@outletNo or @outletNo ='') " & _
                                 " AND (m.ID =@memberID) AND (sm.CustID =@custID) AND sm.memberno <> '' " & _
                             "GROUP BY sm.busidate")
            End If
            connDb.addParam(paramFrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(paramTo, "@paramTo", MySqlDbType.DateTime)
            connDb.addParam(outletNo, "@outletNo", MySqlDbType.VarChar)
            connDb.addParam(MemberID, "@memberID", MySqlDbType.Int32)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)

            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:salesSumMember")
            Err.Clear()
        End Try

        Return dsRslt
    End Function


    'Revisi 2 feb 2017
    'revisi 19 april 2018
    Private Function salesDetail(ByVal paramFrom As Date, ByVal paramTo As Date, ByVal Outletstr As String, ByVal iOwn As Integer, ByVal iBrand As String, ByVal iCity As String, ByVal memberID As Integer, ByVal custID As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim dsRslt As New DataSet
        Dim sOut As String = ""
        Dim sBrand As String = ""
        Dim sOwn As String = ""
        Dim sCity As String = ""

        Try
            Dim connDb As New clsDbConn(ServerString)

            If iBrand = "0" Then
                sBrand = ""
            Else
                sBrand = " and o.brandID in (" & iBrand & ")"
            End If

            If iOwn = "0" Then
                sOwn = ""
            Else
                sOwn = " and o.ownID = " & iOwn
            End If
            If iCity = "0" Then
                sCity = ""
            Else
                sCity = " and o.cityID in (" & iCity & ")"
            End If

            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If Outletstr = "" Then
                    sOut = " and sm.olId in (Select Store From cld_userxstore where user = '" & usr & "')"
                Else
                    sOut = " and sm.olnumber in (" & Outletstr & ")"
                End If
            Else
                If Outletstr = "" Then
                    sOut = ""
                Else
                    sOut = " and sm.olnumber in (" & Outletstr & ")"
                End If
            End If


            connDb.resetCmd("SELECT busidate " &
                                  ",sm.Number " &
                                  ",SUM(sm.subtotal) AS subtotal " &
                                  ",SUM(sm.tax) AS tax " &
                                  ",SUM(sm.service) AS service " &
                                  ",SUM(sm.discount) AS discount " &
                                  ",SUM(sm.total) AS total " &
                                  ",SUM(sm.ppn) AS ppn " &
                                  ",SUM(sm.pb1) AS pb1,olnumber,olname " &
                              "FROM cld_salesmaster sm " &
                              "where sm.flag = 1 and sm.busiDate >=@paramFrom AND sm.busiDate <=@paramTo " &
                                    " AND (sm.CustID=@custID OR @custID=0) " & sOut & sOwn & sBrand & sCity &
                              "GROUP BY busidate, Number")

            connDb.addParam(paramFrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(paramTo, "@paramTo", MySqlDbType.DateTime)
            'connDb.addParam(outletNo, "@outletNo", MySqlDbType.VarChar)
            'connDb.addParam(memberID, "@memberID", MySqlDbType.Int32)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)

            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing
        Catch ex As Exception
            errLog(Err.Description, "ClsReports: salesDetail")
            Err.Clear()
        End Try

        Return dsRslt

    End Function

    'penambahan 26 april 2018
    Private Function salesDetail2(ByVal paramFrom As Date, ByVal paramTo As Date, ByVal Outletstr As String, ByVal iOwn As String, ByVal iBrand As String, ByVal iCity As String, ByVal custID As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim dsRslt As New DataSet
        Dim sOut As String = ""
        Dim sBrand As String = ""
        Dim sOwn As String = ""
        Dim sCity As String = ""

        Try
            Dim connDb As New clsDbConn(ServerString)
            If iBrand = "0" Or iBrand = "" Then
                sBrand = ""
            Else
                sBrand = " and o.brandID in (" & iBrand & ")"
            End If

            If iOwn = "0" Or iOwn = "" Then
                sOwn = ""
            Else
                sOwn = " and o.ownID = " & iOwn
            End If
            If iCity = "0" Or iCity = "" Then
                sCity = ""
            Else
                sCity = " and o.cityID in (" & iCity & ")"
            End If

            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If Outletstr = "" Then
                    sOut = " and sm.olId in (Select Store From cld_userxstore where user = '" & usr & "')"
                Else
                    sOut = " and sm.olnumber in (" & Outletstr & ")"
                End If
            Else
                If Outletstr = "" Then
                    sOut = ""
                Else
                    sOut = " and sm.olnumber in (" & Outletstr & ")"
                End If
            End If

            connDb.resetCmd("SELECT Business_Date 'Sales Date', StoreNo 'Store Code', StoreName 'Store Name', Number 'Sales Number', Transdate 'Transaction Date', subtotal 'Subtotal', Tax, Service, Discount, total 'Gross Sales', Ppn, Pb1 FROM 
                            (SELECT Busidate ,DATE_FORMAT(Busidate, '%d-%b-%Y') AS Business_Date ,olnumber StoreNo,olname StoreName ,sm.Number ,DATE_FORMAT(sm.transdate,'%d-%b-%Y %H:%i:%s') transdate
                            ,SUM(sm.subtotal) AS subtotal ,SUM(sm.tax) AS tax ,SUM(sm.service) AS service 
                            ,SUM(sm.discount) AS discount ,SUM(sm.total) AS total ,SUM(sm.ppn) AS ppn 
                            ,SUM(sm.pb1) AS pb1 FROM cld_salesmaster sm INNER JOIN cld_outlet o ON sm.CustID = o.custID AND sm.olId = o.id 
                            WHERE sm.flag = 1 AND sm.busiDate >=@paramFrom AND sm.busiDate <=@paramTo  AND (sm.CustID=@custID OR @custID=0) " & sOut & sOwn & sBrand & sCity &
                            " GROUP BY number,olnumber, busidate, number, transdate) a
                             ORDER BY StoreNo, Busidate,number ")

            'connDb.resetCmd("SELECT Business_Date 'Sales Date', StoreNo 'Store Code', StoreName 'Store Name', Number 'Sales Number', Transdate 'Transaction Date', subtotal 'Subtotal', Tax, Service, Discount, total 'Gross Sales', Ppn, Pb1 FROM (
            '                (SELECT 1 AS urut, Busidate ,DATE_FORMAT(Busidate, '%d-%b-%Y') AS Business_Date ,olnumber StoreNo,olname StoreName ,sm.Number ,DATE_FORMAT(sm.transdate,'%d-%b-%Y %H:%i:%s') transdate
            '                ,SUM(sm.subtotal) AS subtotal ,SUM(sm.tax) AS tax ,SUM(sm.service) AS service 
            '                ,SUM(sm.discount) AS discount ,SUM(sm.total) AS total ,SUM(sm.ppn) AS ppn 
            '                ,SUM(sm.pb1) AS pb1 FROM cld_salesmaster sm INNER JOIN cld_outlet o ON sm.CustID = o.custID AND sm.olId = o.id 
            '                WHERE sm.flag = 1 AND sm.busiDate >=@paramFrom AND sm.busiDate <=@paramTo  AND (sm.CustID=@custID OR @custID=0) " & sOut & sOwn & sBrand & sCity &
            '                " GROUP BY number,olnumber, busidate, number, transdate) 
            '                UNION ALL
            '                SELECT 2 AS urut, @paramTo AS Busidate ,'' AS Business_Date,'',' Total' AS StoreNo ,'' ,'' ,SUM(sm.subtotal) 
            '                AS subtotal ,SUM(sm.tax) AS tax ,SUM(sm.service) AS service ,SUM(sm.discount) 
            '                AS discount ,SUM(sm.total) AS total ,SUM(sm.ppn) AS ppn ,SUM(sm.pb1) AS pb1
            '                FROM cld_salesmaster sm
            '                 INNER JOIN cld_outlet o ON sm.CustID = o.custID AND sm.olId = o.id WHERE sm.flag = 1 AND sm.busiDate >=@paramFrom AND 
            '                 sm.busiDate <=@paramTo  AND (sm.CustID=@custID OR @custID=0) " & sOut & sOwn & sBrand & sCity & ") a
            '                 ORDER BY Urut, StoreNo, Busidate,number ")


            connDb.addParam(paramFrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(paramTo, "@paramTo", MySqlDbType.DateTime)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)
            'connDb.addParam(memberID, "@memberID", MySqlDbType.Int32)


            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing
        Catch ex As Exception
            errLog(Err.Description, "ClsReports:salesDetail")
            Err.Clear()
        End Try

        Return dsRslt

    End Function

    Private Function salesDetail2_2018(ByVal paramFrom As Date, ByVal paramTo As Date, ByVal Outletstr As String, ByVal iOwn As String, ByVal iBrand As String, ByVal iCity As String, ByVal custID As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim dsRslt As New DataSet
        Dim sOut As String = ""
        Dim sBrand As String = ""
        Dim sOwn As String = ""
        Dim sCity As String = ""

        Try
            Dim connDb As New clsDbConn(ServerString)
            If iBrand = "0" Or iBrand = "" Then
                sBrand = ""
            Else
                sBrand = " and o.brandID in (" & iBrand & ")"
            End If

            If iOwn = "0" Or iOwn = "" Then
                sOwn = ""
            Else
                sOwn = " and o.ownID = " & iOwn
            End If
            If iCity = "0" Or iCity = "" Then
                sCity = ""
            Else
                sCity = " and o.cityID in (" & iCity & ")"
            End If

            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If Outletstr = "" Then
                    sOut = " and sm.olId in (Select Store From cld_userxstore where user = '" & usr & "')"
                Else
                    sOut = " and sm.olnumber in (" & Outletstr & ")"
                End If
            Else
                If Outletstr = "" Then
                    sOut = ""
                Else
                    sOut = " and sm.olnumber in (" & Outletstr & ")"
                End If
            End If
            connDb.resetCmd("SELECT Business_Date 'Sales Date', StoreNo 'Store Code', StoreName 'Store Name', Number 'Sales Number', Transdate 'Transaction Date', subtotal 'Subtotal', Tax, Service, Discount, total 'Gross Sales', Ppn, Pb1 FROM 
                            (SELECT 1 AS urut, Busidate ,DATE_FORMAT(Busidate, '%d %b %Y') AS Business_Date ,olnumber StoreNo,olname StoreName ,sm.Number ,sm.TransDate 
                            ,SUM(sm.subtotal) AS subtotal ,SUM(sm.tax) AS tax ,SUM(sm.service) AS service 
                            ,SUM(sm.discount) AS discount ,SUM(sm.total) AS total ,SUM(sm.ppn) AS ppn 
                            ,SUM(sm.pb1) AS pb1 FROM cld_salesmaster2018 sm INNER JOIN cld_outlet o ON sm.CustID = o.custID AND sm.olId = o.id 
                            WHERE sm.flag = 1 AND sm.busiDate >=@paramFrom AND sm.busiDate <=@paramTo  AND (sm.CustID=@custID OR @custID=0) " & sOut & sOwn & sBrand & sCity &
                            " GROUP BY number,olnumber, busidate, number, transdate) a
                             ORDER BY Urut, StoreNo, Busidate,number ")


            'connDb.resetCmd("SELECT Business_Date 'Sales Date', StoreNo 'Store Code', StoreName 'Store Name', Number 'Sales Number', Transdate 'Transaction Date', subtotal 'Subtotal', Tax, Service, Discount, total 'Gross Sales', Ppn, Pb1 FROM (
            '                (SELECT 1 AS urut, Busidate ,DATE_FORMAT(Busidate, '%d %b %Y') AS Business_Date ,olnumber StoreNo,olname StoreName ,sm.Number ,sm.TransDate 
            '                ,SUM(sm.subtotal) AS subtotal ,SUM(sm.tax) AS tax ,SUM(sm.service) AS service 
            '                ,SUM(sm.discount) AS discount ,SUM(sm.total) AS total ,SUM(sm.ppn) AS ppn 
            '                ,SUM(sm.pb1) AS pb1 FROM cld_salesmaster2018 sm INNER JOIN cld_outlet o ON sm.CustID = o.custID AND sm.olId = o.id 
            '                WHERE sm.flag = 1 AND sm.busiDate >=@paramFrom AND sm.busiDate <=@paramTo  AND (sm.CustID=@custID OR @custID=0) " & sOut & sOwn & sBrand & sCity &
            '                " GROUP BY number,olnumber, busidate, number, transdate) 
            '                UNION ALL
            '                SELECT 2 AS urut, @paramTo AS Busidate ,'' AS Business_Date,'',' Total' AS StoreNo ,'' ,'' ,SUM(sm.subtotal) 
            '                AS subtotal ,SUM(sm.tax) AS tax ,SUM(sm.service) AS service ,SUM(sm.discount) 
            '                AS discount ,SUM(sm.total) AS total ,SUM(sm.ppn) AS ppn ,SUM(sm.pb1) AS pb1
            '                FROM cld_salesmaster2018 sm
            '                 INNER JOIN cld_outlet o ON sm.CustID = o.custID AND sm.olId = o.id WHERE sm.flag = 1 AND sm.busiDate >=@paramFrom AND 
            '                 sm.busiDate <=@paramTo  AND (sm.CustID=@custID OR @custID=0) " & sOut & sOwn & sBrand & sCity & ") a
            '                 ORDER BY Urut, StoreNo, Busidate,number ")

            connDb.addParam(paramFrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(paramTo, "@paramTo", MySqlDbType.DateTime)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)
            'connDb.addParam(memberID, "@memberID", MySqlDbType.Int32)


            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing
        Catch ex As Exception
            errLog(Err.Description, "ClsReports:salesDetail")
            Err.Clear()
        End Try

        Return dsRslt

    End Function

    Private Function salesDetail2_gv(ByVal paramFrom As Date, ByVal paramTo As Date, ByVal Outletstr As String, ByVal iOwn As String, ByVal iBrand As String, ByVal iCity As String, ByVal custID As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim dsRslt As New DataSet
        Dim sOut As String = ""
        Dim sBrand As String = ""
        Dim sOwn As String = ""
        Dim sCity As String = ""

        Try
            Dim connDb As New clsDbConn(ServerString)
            If iBrand = "0" Or iBrand = "" Then
                sBrand = ""
            Else
                sBrand = " and o.brandID in (" & iBrand & ")"
            End If

            If iOwn = "0" Or iOwn = "" Then
                sOwn = ""
            Else
                sOwn = " and o.ownID = " & iOwn
            End If
            If iCity = "0" Or iCity = "" Then
                sCity = ""
            Else
                sCity = " and o.cityID in (" & iCity & ")"
            End If

            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If Outletstr = "" Then
                    sOut = " and sm.olId in (Select Store From cld_userxstore where user = '" & usr & "')"
                Else
                    sOut = " and sm.olnumber in (" & Outletstr & ")"
                End If
            Else
                If Outletstr = "" Then
                    sOut = ""
                Else
                    sOut = " and sm.olnumber in (" & Outletstr & ")"
                End If
            End If



            connDb.resetCmd("SELECT Business_Date 'Sales Date', StoreNo 'Store Code', StoreName 'Store Name', Number 'Sales Number', Transdate 'Transaction Date', subtotal 'Subtotal', Tax, Service, Discount, total 'Gross Sales', Ppn, Pb1 FROM (
                            (SELECT 1 AS urut, Busidate ,DATE_FORMAT(Busidate, '%d-%b-%Y') AS Business_Date ,olnumber StoreNo,olname StoreName ,sm.Number ,DATE_FORMAT(sm.transdate,'%d-%b-%Y %H:%i:%s') transdate 
                            ,FORMAT(SUM(sm.subtotal), 2,'de_DE') AS subtotal ,FORMAT(SUM(sm.tax), 2,'de_DE') AS tax ,FORMAT(SUM(sm.service), 2,'de_DE') AS service 
                            ,FORMAT(SUM(sm.discount), 2,'de_DE') AS discount ,FORMAT(SUM(sm.total), 2,'de_DE') AS total ,FORMAT(SUM(sm.ppn), 2,'de_DE') AS ppn 
                            ,FORMAT(SUM(sm.pb1), 2,'de_DE') AS pb1 FROM cld_salesmaster sm INNER JOIN cld_outlet o ON sm.CustID = o.custID AND sm.olId = o.id 
                            WHERE sm.flag = 1 AND sm.busiDate >=@paramFrom AND sm.busiDate <=@paramTo  AND (sm.CustID=@custID OR @custID=0) " & sOut & sOwn & sBrand & sCity &
                            " GROUP BY number,olnumber, busidate, number, transdate) 
                            UNION ALL
                            SELECT 2 AS urut, @paramTo AS Busidate ,'' AS Business_Date,'',' Total' AS StoreNo ,'' ,'' ,FORMAT(SUM(sm.subtotal), 2,'de_DE') 
                            AS subtotal ,FORMAT(SUM(sm.tax), 2,'de_DE') AS tax ,FORMAT(SUM(sm.service), 2,'de_DE') AS service ,FORMAT(SUM(sm.discount), 2,'de_DE') 
                            AS discount ,FORMAT(SUM(sm.total), 2,'de_DE') AS total ,FORMAT(SUM(sm.ppn), 2,'de_DE') AS ppn ,FORMAT(SUM(sm.pb1), 2,'de_DE') AS pb1
                            FROM cld_salesmaster sm
                             INNER JOIN cld_outlet o ON sm.CustID = o.custID AND sm.olId = o.id WHERE sm.flag = 1 AND sm.busiDate >=@paramFrom AND 
                             sm.busiDate <=@paramTo  AND (sm.CustID=@custID OR @custID=0) " & sOut & sOwn & sBrand & sCity & ") a
                             ORDER BY Urut, StoreNo, Busidate,number ")

            'connDb.resetCmd("SELECT Business_Date, StoreNo, StoreName, Number, Transdate, subtotal, tax, service, discount, total, ppn, pb1, Rounding FROM (
            '                (SELECT 1 AS urut, Busidate ,DATE_FORMAT(Busidate, '%d %b %Y') AS Business_Date ,olnumber StoreNo,olname StoreName ,sm.Number ,sm.TransDate 
            '                ,FORMAT(SUM(sm.subtotal), 2,'de_DE') AS subtotal ,FORMAT(SUM(sm.tax), 2,'de_DE') AS tax ,FORMAT(SUM(sm.service), 2,'de_DE') AS service 
            '                ,FORMAT(SUM(sm.discount), 2,'de_DE') AS discount ,FORMAT(SUM(sm.total), 2,'de_DE') AS total ,FORMAT(SUM(sm.ppn), 2,'de_DE') AS ppn 
            '                ,FORMAT(SUM(sm.pb1), 2,'de_DE') AS pb1,FORMAT(SUM(sm.RoundOff), 2,'de_DE') AS Rounding FROM cld_salesmaster sm INNER JOIN cld_outlet o ON sm.CustID = o.custID AND sm.olId = o.id 
            '                WHERE sm.flag = 1 AND sm.busiDate >=@paramFrom AND sm.busiDate <=@paramTo  AND (sm.CustID=@custID OR @custID=0) " & sOut & sOwn & sBrand & sCity &
            '                " GROUP BY number,olnumber, busidate, number, transdate) 
            '                UNION ALL
            '                SELECT 2 AS urut, @paramTo AS Busidate ,'' AS Business_Date,'',' Total' AS StoreNo ,'' ,'' ,FORMAT(SUM(sm.subtotal), 2,'de_DE') 
            '                AS subtotal ,FORMAT(SUM(sm.tax), 2,'de_DE') AS tax ,FORMAT(SUM(sm.service), 2,'de_DE') AS service ,FORMAT(SUM(sm.discount), 2,'de_DE') 
            '                AS discount ,FORMAT(SUM(sm.total), 2,'de_DE') AS total ,FORMAT(SUM(sm.ppn), 2,'de_DE') AS ppn ,FORMAT(SUM(sm.pb1), 2,'de_DE') AS pb1,FORMAT(SUM(sm.RoundOff), 2,'de_DE') AS Rounding
            '                FROM cld_salesmaster sm
            '                 INNER JOIN cld_outlet o ON sm.CustID = o.custID AND sm.olId = o.id WHERE sm.flag = 1 AND sm.busiDate >=@paramFrom AND 
            '                 sm.busiDate <=@paramTo  AND (sm.CustID=@custID OR @custID=0) " & sOut & sOwn & sBrand & sCity & ") a
            '                 ORDER BY Urut, StoreNo, Busidate ")




            connDb.addParam(paramFrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(paramTo, "@paramTo", MySqlDbType.DateTime)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)
            'connDb.addParam(memberID, "@memberID", MySqlDbType.Int32)


            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing
        Catch ex As Exception
            errLog(Err.Description, "ClsReports: salesDetail")
            Err.Clear()
        End Try

        Return dsRslt

    End Function

    Private Function salesDetail2_2018_gv(ByVal paramFrom As Date, ByVal paramTo As Date, ByVal Outletstr As String, ByVal iOwn As String, ByVal iBrand As String, ByVal iCity As String, ByVal custID As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim dsRslt As New DataSet
        Dim sOut As String = ""
        Dim sBrand As String = ""
        Dim sOwn As String = ""
        Dim sCity As String = ""

        Try
            Dim connDb As New clsDbConn(ServerString)
            If iBrand = "0" Or iBrand = "" Then
                sBrand = ""
            Else
                sBrand = " and o.brandID in (" & iBrand & ")"
            End If

            If iOwn = "0" Or iOwn = "" Then
                sOwn = ""
            Else
                sOwn = " and o.ownID = " & iOwn
            End If
            If iCity = "0" Or iCity = "" Then
                sCity = ""
            Else
                sCity = " and o.cityID in (" & iCity & ")"
            End If

            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If Outletstr = "" Then
                    sOut = " and sm.olId in (Select Store From cld_userxstore where user = '" & usr & "')"
                Else
                    sOut = " and sm.olnumber in (" & Outletstr & ")"
                End If
            Else
                If Outletstr = "" Then
                    sOut = ""
                Else
                    sOut = " and sm.olnumber in (" & Outletstr & ")"
                End If
            End If

            connDb.resetCmd("SELECT Business_Date 'Sales Date', StoreNo 'Store Code', StoreName 'Store Name', Number 'Sales Number', Transdate 'Transaction Date', subtotal 'Subtotal', Tax, Service, Discount, total 'Gross Sales', Ppn, Pb1 FROM (
                            (SELECT 1 AS urut, Busidate ,DATE_FORMAT(Busidate, '%d-%b-%Y') AS Business_Date ,olnumber StoreNo,olname StoreName ,sm.Number ,DATE_FORMAT(sm.transdate,'%d-%b-%Y %H:%i:%s') transdate 
                            ,FORMAT(SUM(sm.subtotal), 2,'de_DE') AS subtotal ,FORMAT(SUM(sm.tax), 2,'de_DE') AS tax ,FORMAT(SUM(sm.service), 2,'de_DE') AS service 
                            ,FORMAT(SUM(sm.discount), 2,'de_DE') AS discount ,FORMAT(SUM(sm.total), 2,'de_DE') AS total ,FORMAT(SUM(sm.ppn), 2,'de_DE') AS ppn 
                            ,FORMAT(SUM(sm.pb1), 2,'de_DE') AS pb1 FROM cld_salesmaster2018 sm INNER JOIN cld_outlet o ON sm.CustID = o.custID AND sm.olId = o.id 
                            WHERE sm.flag = 1 AND sm.busiDate >=@paramFrom AND sm.busiDate <=@paramTo  AND (sm.CustID=@custID OR @custID=0) " & sOut & sOwn & sBrand & sCity &
                            " GROUP BY number,olnumber, busidate, number, transdate) 
                            UNION ALL
                            SELECT 2 AS urut, @paramTo AS Busidate ,'' AS Business_Date,'',' Total' AS StoreNo ,'' ,'' ,FORMAT(SUM(sm.subtotal), 2,'de_DE') 
                            AS subtotal ,FORMAT(SUM(sm.tax), 2,'de_DE') AS tax ,FORMAT(SUM(sm.service), 2,'de_DE') AS service ,FORMAT(SUM(sm.discount), 2,'de_DE') 
                            AS discount ,FORMAT(SUM(sm.total), 2,'de_DE') AS total ,FORMAT(SUM(sm.ppn), 2,'de_DE') AS ppn ,FORMAT(SUM(sm.pb1), 2,'de_DE') AS pb1
                            FROM cld_salesmaster2018 sm
                             INNER JOIN cld_outlet o ON sm.CustID = o.custID AND sm.olId = o.id WHERE sm.flag = 1 AND sm.busiDate >=@paramFrom AND 
                             sm.busiDate <=@paramTo  AND (sm.CustID=@custID OR @custID=0) " & sOut & sOwn & sBrand & sCity & ") a
                             ORDER BY Urut, StoreNo, Busidate,number ")


            connDb.addParam(paramFrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(paramTo, "@paramTo", MySqlDbType.DateTime)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)
            'connDb.addParam(memberID, "@memberID", MySqlDbType.Int32)


            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing
        Catch ex As Exception
            errLog(Err.Description, "ClsReports: salesDetail")
            Err.Clear()
        End Try

        Return dsRslt

    End Function

    'Revisi 2 Feb 2017
    Private Function itemSales(ByVal paramFrom As Date, ByVal paramTo As Date, ByVal Outletstr As String, ByVal iOwn As String, ByVal iBrand As String, ByVal iCity As String, ByVal custID As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim dsRslt As New DataSet
        Dim sOut As String = ""
        Dim sBrand As String = ""
        Dim sOwn As String = ""
        Dim sCity As String = ""
        Try
            Dim connDb As New clsDbConn(ServerString)
            If iBrand = "0" Or iBrand = "" Then
                sBrand = ""
            Else
                sBrand = " and o.brandID in (" & iBrand & ")"
            End If

            If iOwn = "0" Or iOwn = "" Then
                sOwn = ""
            Else
                sOwn = " and o.ownID = " & iOwn
            End If
            If iCity = "0" Or iCity = "" Then
                sCity = ""
            Else
                sCity = " and o.cityID in (" & iCity & ")"
            End If

            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If Outletstr = "" Then
                    sOut = " and sd.olId in (Select Store From cld_userxstore where user = '" & usr & "')"
                Else
                    sOut = " and sd.olnumber in (" & Outletstr & ")"
                End If
            Else
                If Outletstr = "" Then
                    sOut = ""
                Else
                    sOut = " and sd.olnumber in (" & Outletstr & ")"
                End If
            End If



            'connDb.resetCmd("SELECT sd.ItemNo, sd.itemName as Item " &
            '                          ",sd.itemGroup " &
            '                          ",SUM(sd.Qty) AS qty " &
            '                          ",SUM(sd.total) AS Subtotal " &
            '                          ",o.Kode AS StoreNo " &
            '                          ",o.Nama AS StoreName " &
            '                      "FROM cld_sumsalesdetaildaily sd INNER JOIN " &
            '                          "cld_outlet o On (o.id = sd.olid And o.custid = sd.custid) " &
            '                      "where sd.busidate >=@paramFrom And sd.busiDate <=@paramTo " &
            '                            "AND (sd.CustID=@custID OR @custID=0) " & sOut & sOwn & sBrand & sCity &
            '                      "GROUP BY sd.ItemNo, sd.itemName, sd.itemGroup, o.Kode")

            connDb.resetCmd("Select DATE_FORMAT(sd.busidate,'%d-%b-%Y') 'Sales Date', o.Kode AS 'Store Code' ,o.Nama AS 'Store Name', sd.itemGroup AS 'Item Group', sd.ItemNo As 'Item Code', sd.itemName AS 'Item Name', SUM(sd.Qty) AS Qty ,
                        SUM(sd.total) AS 'Net Sales'  
                        From cld_sumsalesdetaildaily sd INNER Join cld_outlet o ON (o.id = sd.olid And o.custid = sd.custid) 
                        Where sd.busidate >=@paramFrom AND sd.busiDate <=@paramTo AND (sd.CustID=@CustID OR @CustID=0) " & sOut & sOwn & sBrand & sCity &
                        " Group BY sd.busiDate, sd.ItemNo, sd.itemName, sd.itemGroup, o.Kode")
            'UNION ALL SELECT '' ItemNo, 'Total' ,'' ,SUM(sd.Qty) AS qty ,SUM(sd.total) 
            '            ,'' AS storekode , '' AS storenama FROM cld_sumsalesdetaildaily sd 
            '            INNER Join cld_outlet o ON (o.id = sd.olid And o.custid = sd.custid) 
            '            WHERE sd.busidate >=@paramFrom AND sd.busiDate <=@paramTo AND (sd.CustID=@CustID OR @CustID=0) " & sOut & sOwn & sBrand & sCity)


            connDb.addParam(paramFrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(paramTo, "@paramTo", MySqlDbType.DateTime)
            'connDb.addParam(outletNo, "@outletNo", MySqlDbType.VarChar)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)


            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing
        Catch ex As Exception
            errLog(Err.Description, "ClsReports: itemSales")
            Err.Clear()
        End Try

        Return dsRslt


    End Function

    Private Function itemNonSales(ByVal paramFrom As Date, ByVal paramTo As Date, ByVal Outletstr As String, ByVal iOwn As String, ByVal iBrand As String, ByVal iCity As String, ByVal custID As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim dsRslt As New DataSet
        Dim sOut As String = ""
        Dim sBrand As String = ""
        Dim sOwn As String = ""
        Dim sCity As String = ""
        Try
            Dim connDb As New clsDbConn(ServerString)
            If iBrand = "0" Or iBrand = "" Then
                sBrand = ""
            Else
                sBrand = " and o.brandID in (" & iBrand & ")"
            End If

            If iOwn = "0" Or iOwn = "" Then
                sOwn = ""
            Else
                sOwn = " and o.ownID = " & iOwn
            End If
            If iCity = "0" Or iCity = "" Then
                sCity = ""
            Else
                sCity = " and o.cityID in (" & iCity & ")"
            End If

            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If Outletstr = "" Then
                    sOut = " and sd.olId in (Select Store From cld_userxstore where user = '" & usr & "')"
                Else
                    sOut = " and sd.olnumber in (" & Outletstr & ")"
                End If
            Else
                If Outletstr = "" Then
                    sOut = ""
                Else
                    sOut = " and sd.olnumber in (" & Outletstr & ")"
                End If
            End If


            connDb.resetCmd("Select DATE_FORMAT(sd.busidate,'%d-%b-%Y') 'Sales Date', o.Kode AS 'Store Code' ,o.Nama AS 'Store Name', sd.itemGroup AS 'Item Group', sd.ItemNo As 'Item Code', sd.itemName AS 'Item Name', SUM(sd.Qty) AS Qty ,
                        SUM(sd.total) AS 'Net Sales'  
                        From cld_sumnosalesdetaildaily sd INNER Join cld_outlet o ON (o.id = sd.olid And o.custid = sd.custid) 
                        Where sd.busidate >=@paramFrom AND sd.busiDate <=@paramTo AND (sd.CustID=@CustID OR @CustID=0) " & sOut & sOwn & sBrand & sCity &
                        " Group BY sd.ItemNo, sd.itemName, sd.itemGroup, o.Kode")

            connDb.addParam(paramFrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(paramTo, "@paramTo", MySqlDbType.DateTime)
            'connDb.addParam(outletNo, "@outletNo", MySqlDbType.VarChar)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)


            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing
        Catch ex As Exception
            errLog(Err.Description, "ClsReports: itemNonSales")
            Err.Clear()
        End Try

        Return dsRslt


    End Function

    Private Function itemSales_gv(ByVal paramFrom As Date, ByVal paramTo As Date, ByVal Outletstr As String, ByVal iOwn As String, ByVal iBrand As String, ByVal iCity As String, ByVal custID As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim dsRslt As New DataSet
        Dim sOut As String = ""
        Dim sBrand As String = ""
        Dim sOwn As String = ""
        Dim sCity As String = ""
        Try
            Dim connDb As New clsDbConn(ServerString)
            If iBrand = "0" Or iBrand = "" Then
                sBrand = ""
            Else
                sBrand = " and o.brandID in (" & iBrand & ")"
            End If

            If iOwn = "0" Or iOwn = "" Then
                sOwn = ""
            Else
                sOwn = " and o.ownID = " & iOwn
            End If
            If iCity = "0" Or iCity = "" Then
                sCity = ""
            Else
                sCity = " and o.cityID in (" & iCity & ")"
            End If

            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If Outletstr = "" Then
                    sOut = " and sd.olId in (Select Store From cld_userxstore where user = '" & usr & "')"
                Else
                    sOut = " and sd.olnumber in (" & Outletstr & ")"
                End If
            Else
                If Outletstr = "" Then
                    sOut = ""
                Else
                    sOut = " and sd.olnumber in (" & Outletstr & ")"
                End If
            End If



            'connDb.resetCmd("SELECT sd.ItemNo, sd.itemName as Item " &
            '                          ",sd.itemGroup " &
            '                          ",SUM(sd.Qty) AS Qty " &
            '                          ",FORMAT(SUM(sd.total), 2,'de_DE') AS Subtotal " &
            '                          ",o.Kode AS StoreNo " &
            '                          ",o.Nama AS StoreName " &
            '                      "FROM cld_sumsalesdetaildaily sd INNER JOIN " &
            '                          "cld_outlet o On (o.id = sd.olid And o.custid = sd.custid) " &
            '                      "where sd.busidate >=@paramFrom And sd.busiDate <=@paramTo " &
            '                            "AND (sd.CustID=@custID OR @custID=0) " & sOut & sOwn & sBrand & sCity &
            '                      "GROUP BY sd.ItemNo, sd.itemName, sd.itemGroup, o.Kode " &
            '                      " UNION ALL " &
            '                      "SELECT '' ItemNo, 'Total' " &
            '                          ",'' " &
            '                          ",SUM(sd.Qty) AS qty " &
            '                          ",FORMAT(SUM(sd.total), 2,'de_DE') AS subtotal " &
            '                          ",'' AS storekode " &
            '                          ", '' AS storenama " &
            '                      "FROM cld_sumsalesdetaildaily sd INNER JOIN " &
            '                          "cld_outlet o On (o.id = sd.olid And o.custid = sd.custid) " &
            '                      "where sd.busidate >=@paramFrom And sd.busiDate <=@paramTo " &
            '                            "AND (sd.CustID=@custID OR @custID=0) " & sOut & sOwn & sBrand & sCity)
            connDb.resetCmd("Select DATE_FORMAT(sd.BusiDate,'%d-%b-%Y') AS 'Sales Date', o.Kode AS 'Store Code' ,o.Nama AS 'Store Name', sd.itemGroup AS 'Item Group', sd.ItemNo As 'Item Code', sd.itemName AS 'Item Name', FORMAT(SUM(sd.Qty), 0,'de_DE') AS Qty ,
                        Format(SUM(sd.total), 2,'de_DE') AS 'Net Sales' 
                        From cld_sumsalesdetaildaily sd INNER Join cld_outlet o ON (o.id = sd.olid And o.custid = sd.custid) 
                        Where sd.busidate >=@paramFrom AND sd.busiDate <=@paramTo AND (sd.CustID=@CustID OR @CustID=0) " & sOut & sOwn & sBrand & sCity &
                        " Group BY sd.BusiDate, sd.ItemNo, sd.itemName, sd.itemGroup, o.Kode  
                        UNION ALL SELECT '','' AS storekode , '' AS storenama,'','' ItemNo, 'Total' ,FORMAT(SUM(sd.Qty), 0,'de_DE') AS Qty ,FORMAT(SUM(sd.total), 2,'de_DE') 
                        AS subtotal FROM cld_sumsalesdetaildaily sd 
                        INNER Join cld_outlet o ON (o.id = sd.olid And o.custid = sd.custid) 
                        WHERE sd.busidate >=@paramFrom AND sd.busiDate <=@paramTo AND (sd.CustID=@CustID OR @CustID=0) " & sOut & sOwn & sBrand & sCity)


            connDb.addParam(paramFrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(paramTo, "@paramTo", MySqlDbType.DateTime)
            'connDb.addParam(outletNo, "@outletNo", MySqlDbType.VarChar)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)


            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing
        Catch ex As Exception
            errLog(Err.Description, "ClsReports:itemSales")
            Err.Clear()
        End Try

        Return dsRslt


    End Function
    Private Function itemNonSales_gv(ByVal paramFrom As Date, ByVal paramTo As Date, ByVal Outletstr As String, ByVal iOwn As String, ByVal iBrand As String, ByVal iCity As String, ByVal custID As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim dsRslt As New DataSet
        Dim sOut As String = ""
        Dim sBrand As String = ""
        Dim sOwn As String = ""
        Dim sCity As String = ""
        Try
            Dim connDb As New clsDbConn(ServerString)
            If iBrand = "0" Or iBrand = "" Then
                sBrand = ""
            Else
                sBrand = " and o.brandID in (" & iBrand & ")"
            End If

            If iOwn = "0" Or iOwn = "" Then
                sOwn = ""
            Else
                sOwn = " and o.ownID = " & iOwn
            End If
            If iCity = "0" Or iCity = "" Then
                sCity = ""
            Else
                sCity = " and o.cityID in (" & iCity & ")"
            End If

            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If Outletstr = "" Then
                    sOut = " and sd.olId in (Select Store From cld_userxstore where user = '" & usr & "')"
                Else
                    sOut = " and sd.olnumber in (" & Outletstr & ")"
                End If
            Else
                If Outletstr = "" Then
                    sOut = ""
                Else
                    sOut = " and sd.olnumber in (" & Outletstr & ")"
                End If
            End If

            connDb.resetCmd("Select DATE_FORMAT(sd.BusiDate,'%d-%b-%Y') AS 'Sales Date', o.Kode AS 'Store Code' ,o.Nama AS 'Store Name', sd.itemGroup AS 'Item Group', sd.ItemNo As 'Item Code', sd.itemName AS 'Item Name', FORMAT(SUM(sd.Qty), 0,'de_DE') AS Qty ,
                        Format(SUM(sd.total), 2,'de_DE') AS 'Net Sales' 
                        From cld_sumnosalesdetaildaily sd INNER Join cld_outlet o ON (o.id = sd.olid And o.custid = sd.custid) 
                        Where sd.busidate >=@paramFrom AND sd.busiDate <=@paramTo AND (sd.CustID=@CustID OR @CustID=0) " & sOut & sOwn & sBrand & sCity &
                        " Group BY sd.ItemNo, sd.itemName, sd.itemGroup, o.Kode  
                        UNION ALL SELECT '','' AS storekode , '' AS storenama,'','' ItemNo, 'Total' ,FORMAT(SUM(sd.Qty), 0,'de_DE') AS Qty ,FORMAT(SUM(sd.total), 2,'de_DE') 
                        AS subtotal FROM cld_sumnosalesdetaildaily sd 
                        INNER Join cld_outlet o ON (o.id = sd.olid And o.custid = sd.custid) 
                        WHERE sd.busidate >=@paramFrom AND sd.busiDate <=@paramTo AND (sd.CustID=@CustID OR @CustID=0) " & sOut & sOwn & sBrand & sCity)


            connDb.addParam(paramFrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(paramTo, "@paramTo", MySqlDbType.DateTime)
            'connDb.addParam(outletNo, "@outletNo", MySqlDbType.VarChar)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)


            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing
        Catch ex As Exception
            errLog(Err.Description, "ClsReports:itemNonSales_gv")
            Err.Clear()
        End Try

        Return dsRslt


    End Function
    Public Function itemSales2(ByVal paramFrom As Date, ByVal paramTo As Date, ByVal Outletstr As String, ByVal iOwn As String, ByVal iBrand As String, ByVal iCity As String, ByVal sitemno As String, ByVal custID As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim dsRslt As New DataSet
        Dim sOut As String = ""
        Dim pitemno As String = ""

        Dim sBrand As String = ""
        Dim sOwn As String = ""
        Dim sCity As String = ""

        Try
            Dim connDb As New clsDbConn(ServerString)

            If sitemno = "" Then
                pitemno = ""
            Else
                pitemno = " and sd.ItemNo in (" & sitemno & ")"
            End If
            If iBrand = "0" Or iBrand = "" Then
                sBrand = ""
            Else
                sBrand = " and o.brandID in (" & iBrand & ")"
            End If

            If iOwn = "0" Or iOwn = "" Then
                sOwn = ""
            Else
                sOwn = " and o.ownID = " & iOwn
            End If
            If iCity = "0" Or iCity = "" Then
                sCity = ""
            Else
                sCity = " and o.cityID in (" & iCity & ")"
            End If

            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If Outletstr = "" Then
                    sOut = " and sd.olId in (Select Store From cld_userxstore where user = '" & usr & "')"
                Else
                    sOut = " and sd.olnumber in (" & Outletstr & ")"
                End If
            Else
                If Outletstr = "" Then
                    sOut = ""
                Else
                    sOut = " and sd.olnumber in (" & Outletstr & ")"
                End If
            End If


            connDb.resetCmd("SELECT sd.busiDate, sd.ItemNo, sd.itemName " &
                                      ",sd.itemGroup " &
                                      ",SUM(sd.Qty) AS qty " &
                                      ",SUM(sd.total) AS subtotal " &
                                      ",o.Kode AS storekode " &
                                      ",o.Nama AS storenama " &
                                  "FROM cld_sumsalesdetaildaily sd INNER JOIN " &
                                      "cld_outlet o On (o.id = sd.olid And o.custid = sd.custid) " &
                                  "where sd.busidate >=@paramFrom And sd.busiDate <=@paramTo " &
                     " AND (sd.CustID =@custID OR @custID=0) " & pitemno & sOut & sOwn & sBrand & sCity & " GROUP BY sd.busiDate,sd.ItemNo, sd.itemGroup, o.Kode")



            connDb.addParam(paramFrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(paramTo, "@paramTo", MySqlDbType.DateTime)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)
            ' connDb.addParam(MemberID, "@memberID", MySqlDbType.Int32)
            ' connDb.addParam(outletNo, "@outletNo", MySqlDbType.VarChar)


            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:itemSales2")
            Err.Clear()
        End Try

        Return dsRslt
    End Function


    'Revisi 08 Juni
    Private Function itemGroupSales(ByVal paramFrom As Date, ByVal paramTo As Date, ByVal Outletstr As String, ByVal iOwn As String, ByVal iBrand As String, ByVal iCity As String, ByVal custID As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim dsRslt As New DataSet
        Dim sOut As String = ""
        Dim sBrand As String = ""
        Dim sOwn As String = ""
        Dim sCity As String = ""
        Try
            Dim connDb As New clsDbConn(ServerString)
            If iBrand = "0" Or iBrand = "" Then
                sBrand = ""
            Else
                sBrand = " and o.brandID in (" & iBrand & ")"
            End If

            If iOwn = "0" Or iOwn = "" Then
                sOwn = ""
            Else
                sOwn = " and o.ownID = " & iOwn
            End If
            If iCity = "0" Or iCity = "" Then
                sCity = ""
            Else
                sCity = " and o.cityID in (" & iCity & ")"
            End If

            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If Outletstr = "" Then
                    sOut = " and sd.olId in (Select Store From cld_userxstore where user = '" & usr & "')"
                Else
                    sOut = " and sd.olnumber in (" & Outletstr & ")"
                End If
            Else
                If Outletstr = "" Then
                    sOut = ""
                Else
                    sOut = " and sd.olnumber in (" & Outletstr & ")"
                End If
            End If


            connDb.resetCmd("SELECT sd.itemGroup " &
                                 ",SUM(sd.Qty) AS qty " &
                                 ",SUM(sd.total) AS subtotal " &
                                 ",o.Kode AS storekode " &
                                 ",o.Nama AS storenama " &
                             "FROM cld_sumsalesdetaildaily sd INNER JOIN " &
                                 "cld_outlet o ON (o.id = sd.olid and o.custid = sd.custid) " &
                             "where sd.busidate >=@paramFrom AND sd.busiDate <=@paramTo " &
                                    "AND (sd.CustID =@custID OR @custID=0) " & sOut & sOwn & sBrand & sCity &
                             " GROUP BY sd.itemGroup, o.Kode")


            connDb.addParam(paramFrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(paramTo, "@paramTo", MySqlDbType.DateTime)
            'connDb.addParam(outletNo, "@outletNo", MySqlDbType.VarChar)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)

            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports: itemGroupSales")
            Err.Clear()
        End Try

        Return dsRslt

    End Function

    '16 Mei 2018
    Private Function DiscountSummary(ByVal paramFrom As Date, ByVal paramTo As Date, ByVal Outletstr As String, ByVal iOwn As String, ByVal iBrand As String, ByVal iCity As String, ByVal custID As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim dsRslt As New DataSet
        Dim sOut As String = ""
        Dim sBrand As String = ""
        Dim sOwn As String = ""
        Dim sCity As String = ""
        Try
            Dim connDb As New clsDbConn(ServerString)
            If iBrand = "0" Or iBrand = "" Then
                sBrand = ""
            Else
                sBrand = " and o.brandID in (" & iBrand & ")"
            End If

            If iOwn = "0" Or iOwn = "" Then
                sOwn = ""
            Else
                sOwn = " and o.ownID = " & iOwn
            End If
            If iCity = "0" Or iCity = "" Then
                sCity = ""
            Else
                sCity = " and o.cityID in (" & iCity & ")"
            End If

            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If Outletstr = "" Then
                    sOut = " and sm.olId in (Select Store From cld_userxstore where user = '" & usr & "')"
                Else
                    sOut = " and sm.olnumber in (" & Outletstr & ")"
                End If
            Else
                If Outletstr = "" Then
                    sOut = ""
                Else
                    sOut = " and sm.olnumber in (" & Outletstr & ")"
                End If
            End If

            connDb.resetCmd("SELECT DATE_FORMAT(sm.BusiDate,'%d-%b-%Y') AS 'Sales Date', Olnumber AS 'Store Code', Olname AS 'Store Name', DiscNm AS 'Discount Name', SUM(Qty) Qty, SUM(Total) Total
                            From cld_sumdiscdaily SM INNER Join cld_outlet o ON sm.`olid` = o.`ID` And sm.`olnumber` = o.`kode` 
                            And sm.`custid` = o.`custid` WHERE busiDate >=@paramFrom AND busiDate <=@paramTo AND (sm.CustID=@CustID OR @CustID =0) " & sOut & sOwn & sBrand & sCity &
                             "Group BY DiscNm, Olnumber, Olname HAVING SUM(Total)>0 ORDER BY olnumber, DiscNm")
            'UNION ALL SELECT 'Total', SUM(Qty), SUM(Total),'',''
            '                            From cld_sumdiscdaily SM INNER Join cld_outlet o ON sm.`olid` = o.`ID` And sm.`olnumber` = o.`kode` 
            '                And sm.`custid` = o.`custid` WHERE total>0 and busiDate >=@paramFrom AND busiDate <=@paramTo AND (sm.CustID=@CustID OR @CustID =0) " & sOut & sOwn & sBrand & sCity)

            connDb.addParam(paramFrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(paramTo, "@paramTo", MySqlDbType.DateTime)
            'connDb.addParam(outletNo, "@outletNo", MySqlDbType.VarChar)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)

            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:Discount")
            Err.Clear()
        End Try

        Return dsRslt
    End Function

    Private Function DiscountSummary_gv(ByVal paramFrom As Date, ByVal paramTo As Date, ByVal Outletstr As String, ByVal iOwn As String, ByVal iBrand As String, ByVal iCity As String, ByVal custID As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim dsRslt As New DataSet
        Dim sOut As String = ""
        Dim sBrand As String = ""
        Dim sOwn As String = ""
        Dim sCity As String = ""
        Try
            Dim connDb As New clsDbConn(ServerString)
            If iBrand = "0" Or iBrand = "" Then
                sBrand = ""
            Else
                sBrand = " and o.brandID in (" & iBrand & ")"
            End If

            If iOwn = "0" Or iOwn = "" Then
                sOwn = ""
            Else
                sOwn = " and o.ownID = " & iOwn
            End If
            If iCity = "0" Or iCity = "" Then
                sCity = ""
            Else
                sCity = " and o.cityID in (" & iCity & ")"
            End If

            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If Outletstr = "" Then
                    sOut = " and sm.olId in (Select Store From cld_userxstore where user = '" & usr & "')"
                Else
                    sOut = " and sm.olnumber in (" & Outletstr & ")"
                End If
            Else
                If Outletstr = "" Then
                    sOut = ""
                Else
                    sOut = " and sm.olnumber in (" & Outletstr & ")"
                End If
            End If

            'connDb.resetCmd("SELECT DiscNm, SUM(Qty) as qty, FORMAT(SUM(Total), 2,'de_DE') as Total,  Olnumber, Olname " &
            '                    "FROM cld_sumdiscdaily SM " &
            '                    "INNER JOIN cld_outlet o ON sm.`olid` = o.`ID` AND sm.`olnumber` = o.`kode` AND sm.`custid` = o.`custid` " &
            '                    "where busiDate >=@paramFrom And busiDate <=@paramTo " &
            '                    "AND (SM.CustID=@custID OR @custID =0) " & sOut & sOwn & sBrand & sCity & " GROUP BY DiscNm, Olnumber, Olname ORDER BY olnumber, DiscNm")

            connDb.resetCmd("(SELECT DATE_FORMAT(sm.BusiDate,'%d-%b-%Y') AS 'Sales Date', Olnumber AS 'Store Code', Olname AS 'Store Name', DiscNm AS 'Discount Name', FORMAT(SUM(Qty), 0,'de_DE') AS Qty, FORMAT(SUM(Total), 2,'de_DE') AS Total
                            From cld_sumdiscdaily SM INNER Join cld_outlet o ON sm.`olid` = o.`ID` And sm.`olnumber` = o.`kode` 
                            And sm.`custid` = o.`custid` WHERE busiDate >=@paramFrom AND busiDate <=@paramTo AND (sm.CustID=@CustID OR @CustID =0) " & sOut & sOwn & sBrand & sCity &
                             "Group BY DiscNm, Olnumber, Olname HAVING SUM(Total)>0 ORDER BY olnumber, DiscNm)
                             UNION ALL SELECT '','','', 'Total', FORMAT(SUM(Qty), 0,'de_DE'), FORMAT(SUM(Total), 2,'de_DE')
                                        From cld_sumdiscdaily SM INNER Join cld_outlet o ON sm.`olid` = o.`ID` And sm.`olnumber` = o.`kode` 
                            And sm.`custid` = o.`custid` WHERE total>0 and busiDate >=@paramFrom AND busiDate <=@paramTo AND (sm.CustID=@CustID OR @CustID =0) " & sOut & sOwn & sBrand & sCity)


            connDb.addParam(paramFrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(paramTo, "@paramTo", MySqlDbType.DateTime)
            'connDb.addParam(outletNo, "@outletNo", MySqlDbType.VarChar)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)

            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports: Discount")
            Err.Clear()
        End Try

        Return dsRslt
    End Function



    'Revisi 08 Juni
    Private Function Discount(ByVal paramFrom As Date, ByVal paramTo As Date, ByVal Outletstr As String, ByVal iOwn As String, ByVal iBrand As String, ByVal iCity As String, ByVal custID As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim dsRslt As New DataSet

        Dim sOut As String = ""
        Dim sBrand As String = ""
        Dim sOwn As String = ""
        Dim sCity As String = ""
        Try
            Dim connDb As New clsDbConn(ServerString)




            If iBrand = "0" Or iBrand = "" Then
                sBrand = ""
            Else
                sBrand = " and o.brandID in (" & iBrand & ")"
            End If

            If iOwn = "0" Or iOwn = "" Then
                sOwn = ""
            Else
                sOwn = " and o.ownID = " & iOwn
            End If
            If iCity = "0" Or iCity = "" Then
                sCity = ""
            Else
                sCity = " and o.cityID in (" & iCity & ")"
            End If

            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If Outletstr = "" Then
                    sOut = " and sm.olId in (Select Store From cld_userxstore where user = '" & usr & "')"
                Else
                    sOut = " and sm.olnumber in (" & Outletstr & ")"
                End If
            Else
                If Outletstr = "" Then
                    sOut = ""
                Else
                    sOut = " and sm.olnumber in (" & Outletstr & ")"
                End If
            End If


            'connDb.resetCmd("SELECT busidate " &
            '                       ",number " &
            '                       ",discount " &
            '                       ",olnumber " &
            '                       ",olname " &
            '                       ",DiscNm " &
            '                   "FROM cld_salesmaster sm " &
            '                   "INNER JOIN cld_outlet o ON sm.`olid` = o.`ID` AND sm.`olnumber` = o.`kode` AND sm.`custid` = o.`custid` " &
            '                   "where flag = 1 and discount > 0 and busiDate >=@paramFrom and busiDate <=@paramTo " &
            '                        "AND (sm.CustID=@custID OR @custID =0) " & sOut & sOwn & sBrand & sCity & " ORDER BY olnumber, number")

            connDb.resetCmd("(SELECT DATE_FORMAT(busidate, '%d %b %Y') AS 'Sales Date' ,
                                number AS 'Sales Number',Discount ,olnumber AS 'Store Code',
                                olname AS 'Store Name',DiscNm AS 'Discount Name' FROM cld_salesmaster sm INNER JOIN cld_outlet o ON sm.`olid` = o.`ID` 
                                AND sm.`olnumber` = o.`kode` AND sm.`custid` = o.`custid` WHERE flag = 1 AND discount > 0 
                                AND busiDate >=@paramFrom AND busiDate <=@paramTo AND (sm.CustID=@CustID OR @CustID =0) " & sOut & sOwn & sBrand & sCity &
                                "ORDER BY olnumber, number)
                                UNION ALL SELECT '','Total' number, SUM(discount),'','','' FROM cld_salesmaster sm INNER JOIN cld_outlet o ON sm.`olid` = o.`ID` 
                                AND sm.`olnumber` = o.`kode` AND sm.`custid` = o.`custid` WHERE flag = 1 AND discount > 0 
                                AND busiDate >=@paramFrom AND busiDate <=@paramTo AND (sm.CustID=@CustID OR @CustID =0) " & sOut & sOwn & sBrand & sCity)

            connDb.addParam(paramFrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(paramTo, "@paramTo", MySqlDbType.DateTime)
            'connDb.addParam(outletNo, "@outletNo", MySqlDbType.VarChar)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)

            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:Discount")
            Err.Clear()
        End Try

        Return dsRslt
    End Function

    Private Function Discount_gv(ByVal paramFrom As Date, ByVal paramTo As Date, ByVal Outletstr As String, ByVal iOwn As String, ByVal iBrand As String, ByVal iCity As String, ByVal custID As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim dsRslt As New DataSet

        Dim sOut As String = ""
        Dim sBrand As String = ""
        Dim sOwn As String = ""
        Dim sCity As String = ""
        Try
            Dim connDb As New clsDbConn(ServerString)




            If iBrand = "0" Or iBrand = "" Then
                sBrand = ""
            Else
                sBrand = " and o.brandID in (" & iBrand & ")"
            End If

            If iOwn = "0" Or iOwn = "" Then
                sOwn = ""
            Else
                sOwn = " and o.ownID = " & iOwn
            End If
            If iCity = "0" Or iCity = "" Then
                sCity = ""
            Else
                sCity = " and o.cityID in (" & iCity & ")"
            End If

            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If Outletstr = "" Then
                    sOut = " and sm.olId in (Select Store From cld_userxstore where user = '" & usr & "')"
                Else
                    sOut = " and sm.olnumber in (" & Outletstr & ")"
                End If
            Else
                If Outletstr = "" Then
                    sOut = ""
                Else
                    sOut = " and sm.olnumber in (" & Outletstr & ")"
                End If
            End If


            'connDb.resetCmd("SELECT date_format(busidate, '%d %b %Y') as busidate " &
            '                       ",number " &
            '                       ",FORMAT(discount, 2,'de_DE') as discount " &
            '                       ",olnumber " &
            '                       ",olname " &
            '                       ",DiscNm " &
            '                   "FROM cld_salesmaster sm " &
            '                   "INNER JOIN cld_outlet o ON sm.`olid` = o.`ID` AND sm.`olnumber` = o.`kode` AND sm.`custid` = o.`custid` " &
            '                   "where flag = 1 and discount > 0 and busiDate >=@paramFrom and busiDate <=@paramTo " &
            '                        "AND (sm.CustID=@custID OR @custID =0) " & sOut & sOwn & sBrand & sCity & " ORDER BY olnumber, number")
            connDb.resetCmd("(SELECT DATE_FORMAT(busidate, '%d %b %Y') AS 'Sales Date' ,
                                number AS 'Sales Number',FORMAT(discount, 2,'de_DE') AS Discount ,olnumber AS 'Store Code',
                                olname AS 'Store Name',DiscNm AS 'Discount Name' FROM cld_salesmaster sm INNER JOIN cld_outlet o ON sm.`olid` = o.`ID` 
                                AND sm.`olnumber` = o.`kode` AND sm.`custid` = o.`custid` WHERE flag = 1 AND discount > 0 
                                AND busiDate >=@paramFrom AND busiDate <=@paramTo AND (sm.CustID=@CustID OR @CustID =0) " & sOut & sOwn & sBrand & sCity &
                                "ORDER BY olnumber, number)
                                UNION ALL SELECT '','Total' number, FORMAT(SUM(discount), 2,'de_DE'),'','','' FROM cld_salesmaster sm INNER JOIN cld_outlet o ON sm.`olid` = o.`ID` 
                                AND sm.`olnumber` = o.`kode` AND sm.`custid` = o.`custid` WHERE flag = 1 AND discount > 0 
                                AND busiDate >=@paramFrom AND busiDate <=@paramTo AND (sm.CustID=@CustID OR @CustID =0) " & sOut & sOwn & sBrand & sCity)

            connDb.addParam(paramFrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(paramTo, "@paramTo", MySqlDbType.DateTime)
            'connDb.addParam(outletNo, "@outletNo", MySqlDbType.VarChar)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)

            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:Discount")
            Err.Clear()
        End Try

        Return dsRslt
    End Function

    Private Function Discount2018(ByVal paramFrom As Date, ByVal paramTo As Date, ByVal Outletstr As String, ByVal iOwn As String, ByVal iBrand As String, ByVal iCity As String, ByVal custID As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim dsRslt As New DataSet

        Dim sOut As String = ""
        Dim sBrand As String = ""
        Dim sOwn As String = ""
        Dim sCity As String = ""
        Try
            Dim connDb As New clsDbConn(ServerString)




            If iBrand = "0" Or iBrand = "" Then
                sBrand = ""
            Else
                sBrand = " and o.brandID in (" & iBrand & ")"
            End If

            If iOwn = "0" Or iOwn = "" Then
                sOwn = ""
            Else
                sOwn = " and o.ownID = " & iOwn
            End If
            If iCity = "0" Or iCity = "" Then
                sCity = ""
            Else
                sCity = " and o.cityID in (" & iCity & ")"
            End If

            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If Outletstr = "" Then
                    sOut = " and sm.olId in (Select Store From cld_userxstore where user = '" & usr & "')"
                Else
                    sOut = " and sm.olnumber in (" & Outletstr & ")"
                End If
            Else
                If Outletstr = "" Then
                    sOut = ""
                Else
                    sOut = " and sm.olnumber in (" & Outletstr & ")"
                End If
            End If


            'connDb.resetCmd("SELECT busidate " &
            '                       ",number " &
            '                       ",discount " &
            '                       ",olnumber " &
            '                       ",olname " &
            '                       ",DiscNm " &
            '                   "FROM cld_salesmaster2018 sm " &
            '                   "INNER JOIN cld_outlet o ON sm.`olid` = o.`ID` AND sm.`olnumber` = o.`kode` AND sm.`custid` = o.`custid` " &
            '                   "where flag = 1 and discount > 0 and busiDate >=@paramFrom and busiDate <=@paramTo " &
            '                        "AND (sm.CustID=@custID OR @custID =0) " & sOut & sOwn & sBrand & sCity & " ORDER BY olnumber, number")
            connDb.resetCmd("(SELECT DATE_FORMAT(busidate, '%d %b %Y') AS 'Sales Date' ,
                                number AS 'Sales Number',Discount ,olnumber AS 'Store Code',
                                olname AS 'Store Name',DiscNm AS 'Discount Name' FROM cld_salesmaster2018 sm INNER JOIN cld_outlet o ON sm.`olid` = o.`ID` 
                                AND sm.`olnumber` = o.`kode` AND sm.`custid` = o.`custid` WHERE flag = 1 AND discount > 0 
                                AND busiDate >=@paramFrom AND busiDate <=@paramTo AND (sm.CustID=@CustID OR @CustID =0) " & sOut & sOwn & sBrand & sCity &
                                "ORDER BY olnumber, number)
                                UNION ALL SELECT '','Total' number, SUM(discount),'','','' FROM cld_salesmaster2018 sm INNER JOIN cld_outlet o ON sm.`olid` = o.`ID` 
                                AND sm.`olnumber` = o.`kode` AND sm.`custid` = o.`custid` WHERE flag = 1 AND discount > 0 
                                AND busiDate >=@paramFrom AND busiDate <=@paramTo AND (sm.CustID=@CustID OR @CustID =0) " & sOut & sOwn & sBrand & sCity)

            connDb.addParam(paramFrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(paramTo, "@paramTo", MySqlDbType.DateTime)
            'connDb.addParam(outletNo, "@outletNo", MySqlDbType.VarChar)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)

            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:Discount")
            Err.Clear()
        End Try

        Return dsRslt
    End Function
    Private Function Discount2018_gv(ByVal paramFrom As Date, ByVal paramTo As Date, ByVal Outletstr As String, ByVal iOwn As String, ByVal iBrand As String, ByVal iCity As String, ByVal custID As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim dsRslt As New DataSet

        Dim sOut As String = ""
        Dim sBrand As String = ""
        Dim sOwn As String = ""
        Dim sCity As String = ""
        Try
            Dim connDb As New clsDbConn(ServerString)




            If iBrand = "0" Or iBrand = "" Then
                sBrand = ""
            Else
                sBrand = " and o.brandID in (" & iBrand & ")"
            End If

            If iOwn = "0" Or iOwn = "" Then
                sOwn = ""
            Else
                sOwn = " and o.ownID = " & iOwn
            End If
            If iCity = "0" Or iCity = "" Then
                sCity = ""
            Else
                sCity = " and o.cityID in (" & iCity & ")"
            End If

            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If Outletstr = "" Then
                    sOut = " and sm.olId in (Select Store From cld_userxstore where user = '" & usr & "')"
                Else
                    sOut = " and sm.olnumber in (" & Outletstr & ")"
                End If
            Else
                If Outletstr = "" Then
                    sOut = ""
                Else
                    sOut = " and sm.olnumber in (" & Outletstr & ")"
                End If
            End If


            'connDb.resetCmd("SELECT date_format(busidate, '%d %b %Y') as busidate " &
            '                       ",number " &
            '                       ",FORMAT(discount, 2,'de_DE') as discount " &
            '                       ",olnumber " &
            '                       ",olname " &
            '                       ",DiscNm " &
            '                   "FROM cld_salesmaster2018 sm " &
            '                   "INNER JOIN cld_outlet o ON sm.`olid` = o.`ID` AND sm.`olnumber` = o.`kode` AND sm.`custid` = o.`custid` " &
            '                   "where flag = 1 and discount > 0 and busiDate >=@paramFrom and busiDate <=@paramTo " &
            '                        "AND (sm.CustID=@custID OR @custID =0) " & sOut & sOwn & sBrand & sCity & " ORDER BY olnumber, number")

            connDb.resetCmd("(SELECT DATE_FORMAT(busidate, '%d %b %Y') AS 'Sales Date' ,
                                number AS 'Sales Number',FORMAT(discount, 2,'de_DE') AS Discount ,olnumber AS 'Store Code',
                                olname AS 'Store Name',DiscNm AS 'Discount Name' FROM cld_salesmaster2018 sm INNER JOIN cld_outlet o ON sm.`olid` = o.`ID` 
                                AND sm.`olnumber` = o.`kode` AND sm.`custid` = o.`custid` WHERE flag = 1 AND discount > 0 
                                AND busiDate >=@paramFrom AND busiDate <=@paramTo AND (sm.CustID=@CustID OR @CustID =0) " & sOut & sOwn & sBrand & sCity &
                                "ORDER BY olnumber, number)
                                UNION ALL SELECT '','Total' number, FORMAT(SUM(discount), 2,'de_DE'),'','','' FROM cld_salesmaster2018 sm INNER JOIN cld_outlet o ON sm.`olid` = o.`ID` 
                                AND sm.`olnumber` = o.`kode` AND sm.`custid` = o.`custid` WHERE flag = 1 AND discount > 0 
                                AND busiDate >=@paramFrom AND busiDate <=@paramTo AND (sm.CustID=@CustID OR @CustID =0) " & sOut & sOwn & sBrand & sCity)

            connDb.addParam(paramFrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(paramTo, "@paramTo", MySqlDbType.DateTime)
            'connDb.addParam(outletNo, "@outletNo", MySqlDbType.VarChar)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)

            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:Discount")
            Err.Clear()
        End Try

        Return dsRslt
    End Function

    '16 Mei 2018
    Private Function PaymentSummary(ByVal paramFrom As Date, ByVal paramTo As Date, ByVal Outletstr As String, ByVal iOwn As String, ByVal iBrand As String, ByVal iCity As String, ByVal custID As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim dsRslt As New DataSet
        Dim sOut As String = ""
        Dim sBrand As String = ""
        Dim sOwn As String = ""
        Dim sCity As String = ""
        Try
            Dim connDb As New clsDbConn(ServerString)

            If iBrand = "0" Or iBrand = "" Then
                sBrand = ""
            Else
                sBrand = " and o.brandID in (" & iBrand & ")"
            End If

            If iOwn = "0" Or iOwn = "" Then
                sOwn = ""
            Else
                sOwn = " and o.ownID = " & iOwn
            End If
            If iCity = "0" Or iCity = "" Then
                sCity = ""
            Else
                sCity = " and o.cityID in (" & iCity & ")"
            End If

            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If Outletstr = "" Then
                    sOut = " and sm.olId in (Select Store From cld_userxstore where user = '" & usr & "')"
                Else
                    sOut = " and sm.olnumber in (" & Outletstr & ")"
                End If
            Else
                If Outletstr = "" Then
                    sOut = ""
                Else
                    sOut = " and sm.olnumber in (" & Outletstr & ")"
                End If
            End If

            connDb.resetCmd("SELECT DATE_FORMAT(busidate,'%d-%b-%Y') 'Sales Date', Olnumber 'Store Code', Olname 'Store Name', PayNo 'Payment Code', payName 'Payment Name', SUM(Qty) as Qty, SUM(Total) as Total " &
                                "FROM cld_sumpaymentdaily SM " &
                                "INNER JOIN cld_outlet o ON sm.`olid` = o.`ID` AND sm.`olnumber` = o.`kode` AND sm.`custid` = o.`custid` " &
                                "where busiDate >=@paramFrom And busiDate <=@paramTo " &
                                "AND (SM.CustID=@custID OR @custID =0) " & sOut & sOwn & sBrand & sCity & " GROUP BY busidate, PayNo, PayNaME, Olnumber, Olname")

            connDb.addParam(paramFrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(paramTo, "@paramTo", MySqlDbType.DateTime)
            'connDb.addParam(outletNo, "@outletNo", MySqlDbType.VarChar)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)

            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:PaymentSummary")
            Err.Clear()
        End Try

        Return dsRslt
    End Function

    Private Function PaymentSummary_Nonsales(ByVal paramFrom As Date, ByVal paramTo As Date, ByVal Outletstr As String, ByVal iOwn As String, ByVal iBrand As String, ByVal iCity As String, ByVal custID As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim dsRslt As New DataSet
        Dim sOut As String = ""
        Dim sBrand As String = ""
        Dim sOwn As String = ""
        Dim sCity As String = ""
        Try
            Dim connDb As New clsDbConn(ServerString)

            If iBrand = "0" Or iBrand = "" Then
                sBrand = ""
            Else
                sBrand = " and o.brandID in (" & iBrand & ")"
            End If

            If iOwn = "0" Or iOwn = "" Then
                sOwn = ""
            Else
                sOwn = " and o.ownID = " & iOwn
            End If
            If iCity = "0" Or iCity = "" Then
                sCity = ""
            Else
                sCity = " and o.cityID in (" & iCity & ")"
            End If

            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If Outletstr = "" Then
                    sOut = " and sm.olId in (Select Store From cld_userxstore where user = '" & usr & "')"
                Else
                    sOut = " and sm.olnumber in (" & Outletstr & ")"
                End If
            Else
                If Outletstr = "" Then
                    sOut = ""
                Else
                    sOut = " and sm.olnumber in (" & Outletstr & ")"
                End If
            End If

            connDb.resetCmd("SELECT DATE_FORMAT(busidate,'%d-%b-%Y') 'Sales Date', Olnumber 'Store Code', Olname 'Store Name', PayNo 'Payment Code', payName 'Payment Name', SUM(Qty) as Qty, SUM(Total) as Total " &
                                "FROM cld_sumnopaymentdaily SM " &
                                "INNER JOIN cld_outlet o ON sm.`olid` = o.`ID` AND sm.`olnumber` = o.`kode` AND sm.`custid` = o.`custid` " &
                                "where busiDate >=@paramFrom And busiDate <=@paramTo " &
                                "AND (SM.CustID=@custID OR @custID =0) " & sOut & sOwn & sBrand & sCity & " GROUP BY busidate, PayNo, PayNaME, Olnumber, Olname")

            connDb.addParam(paramFrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(paramTo, "@paramTo", MySqlDbType.DateTime)
            'connDb.addParam(outletNo, "@outletNo", MySqlDbType.VarChar)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)

            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:PaymentSummary_Nonsales")
            Err.Clear()
        End Try

        Return dsRslt
    End Function

    'untuk gv
    Private Function PaymentSummary_gv(ByVal paramFrom As Date, ByVal paramTo As Date, ByVal Outletstr As String, ByVal iOwn As String, ByVal iBrand As String, ByVal iCity As String, ByVal custID As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim dsRslt As New DataSet
        Dim sOut As String = ""
        Dim sBrand As String = ""
        Dim sOwn As String = ""
        Dim sCity As String = ""
        Try
            Dim connDb As New clsDbConn(ServerString)

            If iBrand = "0" Or iBrand = "" Then
                sBrand = ""
            Else
                sBrand = " and o.brandID in (" & iBrand & ")"
            End If

            If iOwn = "0" Or iOwn = "" Then
                sOwn = ""
            Else
                sOwn = " and o.ownID = " & iOwn
            End If
            If iCity = "0" Or iCity = "" Then
                sCity = ""
            Else
                sCity = " and o.cityID in (" & iCity & ")"
            End If

            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If Outletstr = "" Then
                    sOut = " and sm.olId in (Select Store From cld_userxstore where user = '" & usr & "')"
                Else
                    sOut = " and sm.olnumber in (" & Outletstr & ")"
                End If
            Else
                If Outletstr = "" Then
                    sOut = ""
                Else
                    sOut = " and sm.olnumber in (" & Outletstr & ")"
                End If
            End If

            connDb.resetCmd("SELECT DATE_FORMAT(sm.busidate,'%d-%b-%Y') 'Sales Date', Olnumber 'Store Code', Olname 'Store Name', PayNo 'Payment Code', payName 'Payment Name', SUM(Qty) as Qty, FORMAT(SUM(Total), 2,'de_DE') as Total " &
                                "FROM cld_sumpaymentdaily SM " &
                                "INNER JOIN cld_outlet o ON sm.`olid` = o.`ID` AND sm.`olnumber` = o.`kode` AND sm.`custid` = o.`custid` " &
                                "where busiDate >=@paramFrom And busiDate <=@paramTo " &
                                "AND (SM.CustID=@custID OR @custID =0) " & sOut & sOwn & sBrand & sCity & " GROUP BY busidate, PayNo, PayNaME, Olnumber, Olname " &
                               " UNION ALL " &
                                "SELECT '', '' Olnumber, '' Olname, '' PayNo, 'Total' payName, SUM(Qty) as qty, FORMAT(SUM(Total), 2,'de_DE') as Total " &
                                "FROM cld_sumpaymentdaily SM " &
                                "INNER JOIN cld_outlet o ON sm.`olid` = o.`ID` AND sm.`olnumber` = o.`kode` AND sm.`custid` = o.`custid` " &
                                "where busiDate >=@paramFrom And busiDate <=@paramTo " &
                                "AND (SM.CustID=@custID OR @custID =0) " & sOut & sOwn & sBrand & sCity)

            connDb.addParam(paramFrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(paramTo, "@paramTo", MySqlDbType.DateTime)
            'connDb.addParam(outletNo, "@outletNo", MySqlDbType.VarChar)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)

            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:PaymentSummary_gv")
            Err.Clear()
        End Try

        Return dsRslt
    End Function

    Private Function PaymentSummary_Nonsales_gv(ByVal paramFrom As Date, ByVal paramTo As Date, ByVal Outletstr As String, ByVal iOwn As String, ByVal iBrand As String, ByVal iCity As String, ByVal custID As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim dsRslt As New DataSet
        Dim sOut As String = ""
        Dim sBrand As String = ""
        Dim sOwn As String = ""
        Dim sCity As String = ""
        Try
            Dim connDb As New clsDbConn(ServerString)

            If iBrand = "0" Or iBrand = "" Then
                sBrand = ""
            Else
                sBrand = " and o.brandID in (" & iBrand & ")"
            End If

            If iOwn = "0" Or iOwn = "" Then
                sOwn = ""
            Else
                sOwn = " and o.ownID = " & iOwn
            End If
            If iCity = "0" Or iCity = "" Then
                sCity = ""
            Else
                sCity = " and o.cityID in (" & iCity & ")"
            End If

            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If Outletstr = "" Then
                    sOut = " and sm.olId in (Select Store From cld_userxstore where user = '" & usr & "')"
                Else
                    sOut = " and sm.olnumber in (" & Outletstr & ")"
                End If
            Else
                If Outletstr = "" Then
                    sOut = ""
                Else
                    sOut = " and sm.olnumber in (" & Outletstr & ")"
                End If
            End If

            connDb.resetCmd("SELECT DATE_FORMAT(sm.busidate,'%d-%b-%Y') 'Sales Date', Olnumber 'Store Code', Olname 'Store Name', PayNo 'Payment Code', payName 'Payment Name', SUM(Qty) as Qty, FORMAT(SUM(Total), 2,'de_DE') as Total " &
                                "FROM cld_sumnopaymentdaily SM " &
                                "INNER JOIN cld_outlet o ON sm.`olid` = o.`ID` AND sm.`olnumber` = o.`kode` AND sm.`custid` = o.`custid` " &
                                "where busiDate >=@paramFrom And busiDate <=@paramTo " &
                                "AND (SM.CustID=@custID OR @custID =0) " & sOut & sOwn & sBrand & sCity & " GROUP BY busidate, PayNo, PayNaME, Olnumber, Olname " &
                               " UNION ALL " &
                                "SELECT '', '' Olnumber, '' Olname, '' PayNo, 'Total' payName, SUM(Qty) as qty, FORMAT(SUM(Total), 2,'de_DE') as Total " &
                                "FROM cld_sumnopaymentdaily SM " &
                                "INNER JOIN cld_outlet o ON sm.`olid` = o.`ID` AND sm.`olnumber` = o.`kode` AND sm.`custid` = o.`custid` " &
                                "where busiDate >=@paramFrom And busiDate <=@paramTo " &
                                "AND (SM.CustID=@custID OR @custID =0) " & sOut & sOwn & sBrand & sCity)

            connDb.addParam(paramFrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(paramTo, "@paramTo", MySqlDbType.DateTime)
            'connDb.addParam(outletNo, "@outletNo", MySqlDbType.VarChar)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)

            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:PaymentSummary_Nonsales_gv")
            Err.Clear()
        End Try

        Return dsRslt
    End Function

    '16 Mei 2018
    Private Function Payment(ByVal paramFrom As Date, ByVal paramTo As Date, ByVal Outletstr As String, ByVal iOwn As String, ByVal iBrand As String, ByVal iCity As String, ByVal custID As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim dsRslt As New DataSet
        Dim sOut As String = ""
        Dim sBrand As String = ""
        Dim sOwn As String = ""
        Dim sCity As String = ""
        Try
            Dim connDb As New clsDbConn(ServerString)
            If iBrand = "0" Or iBrand = "" Then
                sBrand = ""
            Else
                sBrand = " and o.brandID in (" & iBrand & ")"
            End If

            If iOwn = "0" Or iOwn = "" Then
                sOwn = ""
            Else
                sOwn = " and o.ownID = " & iOwn
            End If
            If iCity = "0" Or iCity = "" Then
                sCity = ""
            Else
                sCity = " and o.cityID in (" & iCity & ")"
            End If

            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If Outletstr = "" Then
                    sOut = " and sm.olId in (Select Store From cld_userxstore where user = '" & usr & "')"
                Else
                    sOut = " and sm.olnumber in (" & Outletstr & ")"
                End If
            Else
                If Outletstr = "" Then
                    sOut = ""
                Else
                    sOut = " and sm.olnumber in (" & Outletstr & ")"
                End If
            End If


            connDb.resetCmd("SELECT sm.BusiDate, o.Kode, o.Nama,cld_payment.Smno, cld_payment.PaymentName,cld_payment.Total " &
                                "FROM cld_payment " &
                                "INNER JOIN cld_outlet o ON cld_payment.CustID = o.custID AND cld_payment.outletID = o.ID " &
                                "INNER JOIN cld_salesmaster sm ON cld_payment.CustID = sm.custID AND cld_payment.outletID = sm.olId AND cld_payment.smno = sm.number " &
                                "where sm.flag =1 AND cld_payment.flag = 1 and sm.busiDate >=@paramFrom And sm.busiDate <=@paramTo " &
                                "AND (cld_payment.CustID=@custID OR @custID =0) " & sOut & sOwn & sBrand & sCity & " ORDER BY sm.BusiDate, cld_payment.Smno, cld_payment.Number")

            connDb.addParam(paramFrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(paramTo, "@paramTo", MySqlDbType.DateTime)
            'connDb.addParam(outletNo, "@outletNo", MySqlDbType.VarChar)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)

            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:Discount")
            Err.Clear()
        End Try

        Return dsRslt
    End Function
    Private Function Payment2(ByVal paramFrom As Date, ByVal paramTo As Date, ByVal Outletstr As String, ByVal custID As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim dsRslt As New DataSet
        Dim sOut As String = ""

        Try
            Dim connDb As New clsDbConn(ServerString)


            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If Outletstr = "" Then
                    sOut = " and sm.olId in (Select Store From cld_userxstore where user = '" & usr & "')"
                Else
                    sOut = " and sm.olnumber in (" & Outletstr & ")"
                End If
            Else
                If Outletstr = "" Then
                    sOut = ""
                Else
                    sOut = " and sm.olnumber in (" & Outletstr & ")"
                End If
            End If


            connDb.resetCmd("SELECT sm.BusiDate, o.Kode, o.Nama,cld_payment.Smno, cld_payment.PaymentName,cld_payment.Total " &
                                "FROM cld_payment " &
                                "INNER JOIN cld_outlet o ON cld_payment.CustID = o.custID AND cld_payment.outletID = o.ID " &
                                "INNER JOIN cld_salesmaster sm ON cld_payment.CustID = sm.custID AND cld_payment.outletID = sm.olId AND cld_payment.smno = sm.number " &
                                "where sm.flag =1 AND cld_payment.flag = 1 and sm.busiDate >=@paramFrom And sm.busiDate <=@paramTo " &
                                "AND (cld_payment.CustID=@custID OR @custID =0) " & sOut & " ORDER BY sm.BusiDate, cld_payment.Smno, cld_payment.Number")

            connDb.addParam(paramFrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(paramTo, "@paramTo", MySqlDbType.DateTime)
            'connDb.addParam(outletNo, "@outletNo", MySqlDbType.VarChar)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)

            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:Discount")
            Err.Clear()
        End Try

        Return dsRslt
    End Function




    'Revisi 08 Juni
    Private Function feedbackDtl(ByVal paramFrom As Date, ByVal paramTo As Date, ByVal outletNo As String, ByVal custID As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim dsRslt As New DataSet

        Try
            Dim connDb As New clsDbConn(ServerString)
            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                connDb.resetCmd("SELECT o.nama AS outlet " & _
                                    ",transdate " & _
                                    ",f.memberNo " & _
                                    ",f.memberName " & _
                                    ",fdtl.typeName " & _
                                    ",fdtl.val " & _
                                    ",(CASE WHEN fdtl.val = 1 THEN 'Sangat Tidak Puas' WHEN fdtl.val=2 THEN 'Tidak Puas' WHEN fdtl.val=3 THEN 'ok' " & _
                                    " WHEN  fdtl.val=4 THEN 'Puas' WHEN fdtl.val=5 THEN 'Sangat Puas' END) descrip " & _
                                    ",f.remarks " & _
                                "FROM cld_outlet o INNER JOIN cld_feedbacktrans f ON (f.outletid = o.id AND f.Custid = o.Custid) " & _
                                    "INNER JOIN cld_feedbacktransdtl fdtl ON (f.outletid = fdtl.outletid AND f.Custid = fdtl.Custid AND f.Salesmasterid = fdtl.Salesmasterid) " & _
                                "where f.transDate >=@paramFrom and f.transDate <=@paramTo " & _
                                    "AND ((o.kode=@outletNo AND @outletNo <> '') or (@outletNo='' AND (o.id IN (Select Store from cld_userxstore where user = '" & usr & "'))) ) " & _
                                    "AND (f.CustID=@custID OR @custID=0) ")
            Else

                connDb.resetCmd("SELECT o.nama AS outlet " & _
                                      ",transdate " & _
                                      ",f.memberNo " & _
                                      ",f.memberName " & _
                                      ",fdtl.typeName " & _
                                      ",fdtl.val " & _
                                      ",(CASE WHEN fdtl.val = 1 THEN 'Sangat Tidak Puas' WHEN fdtl.val=2 THEN 'Tidak Puas' WHEN fdtl.val=3 THEN 'ok' " & _
                                      " WHEN  fdtl.val=4 THEN 'Puas' WHEN fdtl.val=5 THEN 'Sangat Puas' END) descrip " & _
                                      ",f.remarks " & _
                                 "FROM cld_outlet o INNER JOIN cld_feedbacktrans f ON (f.outletid = o.id AND f.Custid = o.Custid) " & _
                                        "INNER JOIN cld_feedbacktransdtl fdtl ON (f.outletid = fdtl.outletid AND f.Custid = fdtl.Custid AND f.Salesmasterid = fdtl.Salesmasterid) " & _
                                 "where f.transDate >=@paramFrom and f.transDate <=@paramTo and (o.Kode =@outletNo or @outletNo='')" & _
                                        "AND (f.CustID=@custID OR @custID=0) ")


            End If
            connDb.addParam(paramFrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(paramTo, "@paramTo", MySqlDbType.DateTime)
            connDb.addParam(outletNo, "@outletNo", MySqlDbType.VarChar)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)

            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:feedbackDtl")
            Err.Clear()
        End Try

        Return dsRslt
    End Function

    'Revisi 08 Juni
    Private Function feedbackSum(ByVal paramFrom As Date, ByVal paramTo As Date, ByVal outletNo As String, ByVal custID As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim dsRslt As New DataSet

        Try
            Dim connDb As New clsDbConn(ServerString)
            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then

                connDb.resetCmd("SELECT o.nama AS outlet " & _
                                        ",f.typeName " & _
                                        ",AVG(f.val) AS VALUE " & _
                                "FROM cld_outlet o  " & _
                                        "INNER JOIN cld_feedbacktransdtl f ON (f.outletid = o.id) AND (f.Custid = o.custID) " & _
                                        "INNER JOIN cld_feedbacktrans fb ON (f.outletid = fb.outletid) AND (f.Custid = fb.CustID) " & _
                                "where fb.transDate >=@paramFrom and fb.transDate <=@paramTo " & _
                                        "AND ((o.kode=@outletNo AND @outletNo<>'') or (@outletNo='' AND (o.id IN (Select Store from cld_userxstore where user = '" & usr & "'))) ) " & _
                                        "AND (fb.CustID = @custID OR @custID=0) " & _
                                "Group By o.nama, f.typeName")

            Else
                connDb.resetCmd("SELECT o.nama AS outlet " & _
                                   ",f.typeName " & _
                                   ",AVG(f.val) AS VALUE " & _
                                   "FROM cld_outlet o  " & _
                                   "INNER JOIN cld_feedbacktransdtl f ON (f.outletid = o.id) AND (f.Custid = o.custID) " & _
                                   "INNER JOIN cld_feedbacktrans fb ON (f.outletid = fb.outletid) AND (f.Custid = fb.CustID) " & _
                                   "where fb.transDate >=@paramFrom and fb.transDate <=@paramTo and (o.Kode =@outletNo or @outletNo='') " & _
                                   "AND (fb.CustID = @custID OR @custID=0) " & _
                                   "Group By o.nama, f.typeName")
            End If
            connDb.addParam(paramFrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(paramTo, "@paramTo", MySqlDbType.DateTime)
            connDb.addParam(outletNo, "@outletNo", MySqlDbType.VarChar)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)

            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:feedbackSum")
            Err.Clear()
        End Try

        Return dsRslt
    End Function

    'Revisi 08 Juni
    Private Function feedbackGraph(ByVal paramFrom As Date, ByVal paramTo As Date, ByVal outletNo As String, ByVal memberNo As String, ByVal custID As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim dsRslt As New DataSet

        Try
            Dim connDb As New clsDbConn(ServerString)
            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                connDb.resetCmd("SELECT f.typeName " & _
                                   ",AVG(f.val) AS VALUE " & _
                                "FROM cld_outlet o " & _
                                   "INNER JOIN cld_feedbacktransdtl f ON (f.outletid = o.id) AND (f.Custid = o.custID) " & _
                                   "INNER JOIN cld_feedbacktrans fb ON (f.outletid = fb.outletid) AND (f.Custid = " & _
                                   "fb.CustID) AND (f.Salesmasterid = fb.Salesmasterid) " & _
                                "where fb.transDate >=@paramFrom and fb.transDate <=@paramTo " & _
                                    "AND ((o.kode=@outletNo AND @outletNo<>'') or (@outletNo='' AND (o.id IN (Select Store from cld_userxstore where user = '" & usr & "'))) ) " & _
                                    "AND (f.CustID=@custID OR @custID=0) " & _
                                "GROUP BY f.typeName")
            Else
                connDb.resetCmd("SELECT f.typeName " & _
                                   ",AVG(f.val) AS VALUE " & _
                                "FROM cld_outlet o " & _
                                   "INNER JOIN cld_feedbacktransdtl f ON (f.outletid = o.id) AND (f.Custid = o.custID) " & _
                                   "INNER JOIN cld_feedbacktrans fb ON (f.outletid = fb.outletid) AND (f.Custid = " & _
                                   "fb.CustID) AND (f.Salesmasterid = fb.Salesmasterid) " & _
                                "where fb.transDate >=@paramFrom and fb.transDate <=@paramTo and (o.Kode =@outletNo " & _
                                   "or @outletNo='') AND (f.CustID=@custID OR @custID=0) " & _
                                "GROUP BY f.typeName")
            End If

            connDb.addParam(paramFrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(paramTo, "@paramTo", MySqlDbType.DateTime)
            connDb.addParam(outletNo, "@outletNo", MySqlDbType.VarChar)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)

            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:feedbackGraph")
            Err.Clear()
        End Try

        Return dsRslt

    End Function

    Private Function member(ByVal memberID As Integer) As DataSet
        Dim dsRslt As New DataSet

        Try
            Dim connDb As New clsDbConn(ServerString)

            connDb.resetCmd("SELECT * FROM cld_member where ID=@ID")

            connDb.addParam(memberID, "@ID", MySqlDbType.Int32)

            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:member")
            Err.Clear()
        End Try

        Return dsRslt

    End Function

    Private Function memberList(ByVal custID As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim dsRslt As New DataSet

        Try
            Dim connDb As New clsDbConn(ServerString)

            'If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then

            'Else

            'End If

            'connDb.resetCmd("SELECT '0' AS kode, 'All Member' AS Nama UNION ALL " & _
            '                            "SELECT Number as kode, Name as Nama FROM cld_member where custID=@custID")

            'connDb.resetCmd("SELECT '0' AS kode, '  All Member' AS Nama UNION ALL " & _
            '                          "(SELECT Number AS kode, CONCAT(Number, ' ' ,NAME ) AS Nama  FROM cld_member WHERE custID=@custID) ORDER BY Nama")


            connDb.resetCmd("SELECT '0' AS kode, '  All Member' AS Nama")

            connDb.addParam(custID, "@custID", MySqlDbType.Int32)
            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:memberList")
            Err.Clear()
        End Try

        Return dsRslt

    End Function


    'ga ada
    'Revisi 2 feb 2017
    Private Function salesItem(ByVal paramFrom As Date, ByVal paramTo As Date, ByVal outletNo As String, ByVal memberID As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim dsRslt As New DataSet

        Try
            Dim connDb As New clsDbConn(ServerString)
            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                connDb.resetCmd("SELECT " & _
                                     "dtl.ItemName " & _
                                     ", SUM(dtl.Qty) AS Qty " & _
                                     ", SUM(dtl.Price * Qty) AS Amount " & _
                                  "FROM cld_salesdetail dtl " & _
                                     "INNER JOIN cld_salesmaster mst ON (dtl.smNo = mst.Number and dtl.outletid = mst.olid and dtl.Custid = mst.custid) " & _
                                     "inner join cld_member m on (m.Number = mst.MemberNo and  m.Custid = mst.Custid) " & _
                                  "WHERE mst.flag =1 AND dtl.flag=1 AND mst.BusiDate >=@paramFrom AND mst.BusiDate <=@paramTo " & _
                                    "AND ((mst.olNumber=@outletNo AND @outletNo<>'') or (@outletNo='' AND (mst.olid IN (Select Store from cld_userxstore where user = '" & usr & "'))) ) " & _
                                    " AND (m.ID =@memberID or @memberID=0) " & _
                                  "GROUP BY dtl.ItemName " & _
                                  "ORDER BY SUM(dtl.Qty) DESC")
            Else
                connDb.resetCmd("SELECT " & _
                                    "dtl.ItemName " & _
                                    ", SUM(dtl.Qty) AS Qty " & _
                                    ", SUM(dtl.Price * Qty) AS Amount " & _
                                 "FROM cld_salesdetail dtl " & _
                                    "INNER JOIN cld_salesmaster mst ON (dtl.smNo = mst.Number and dtl.outletid = mst.olid and dtl.Custid = mst.custid) " & _
                                    "inner join cld_member m on (m.Number = mst.MemberNo and m.Custid = mst.Custid) " & _
                                 "WHERE mst.flag =1 AND dtl.flag=1 AND mst.BusiDate >=@paramFrom AND mst.BusiDate <=@paramTo AND (mst.olNumber=@outletNo OR @outletNo='') " & _
                                   " AND (m.ID =@memberID or @memberID=0) " & _
                                 "GROUP BY dtl.ItemName " & _
                                 "ORDER BY SUM(dtl.Qty) DESC")
            End If

            connDb.addParam(paramFrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(paramTo, "@paramTo", MySqlDbType.DateTime)
            connDb.addParam(outletNo, "@outletNo", MySqlDbType.VarChar)
            connDb.addParam(memberID, "@memberID", MySqlDbType.Int32)

            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:itemByQty")
            Err.Clear()
        End Try

        Return dsRslt

    End Function

    'ga ada
    'Revisi 08 Juni
    Private Function salesItemByStore(ByVal paramFrom As Date, ByVal paramTo As Date, ByVal outletNo As String, ByVal memberID As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim dsRslt As New DataSet

        Try
            Dim connDb As New clsDbConn(ServerString)
            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then

                connDb.resetCmd("SELECT mst.olName " & _
                                      ", dtl.ItemName " & _
                                      ", SUM(dtl.Qty) AS Qty " & _
                                      ", SUM(dtl.Price * Qty) AS Amount " & _
                                   "FROM cld_salesdetail dtl " & _
                                      "INNER JOIN cld_salesmaster mst ON (dtl.smNo = mst.Number and dtl.outletid = mst.olid and dtl.Custid = mst.custid) " & _
                                      "inner join cld_member m on (m.Number = mst.MemberNo and  m.Custid = mst.Custid) " & _
                                   "WHERE mst.flag=1 AND dtl.flag=1 AND mst.BusiDate >=@paramFrom AND mst.BusiDate <=@paramTo " & _
                                        "AND ((mst.olNumber=@outletNo AND @outletNo<>'') or (@outletNo='' AND (mst.olid IN (Select Store from cld_userxstore where user = '" & usr & "'))) ) " & _
                                        " AND (m.ID =@memberID or @memberID=0) " & _
                                   "GROUP BY mst.olName, dtl.ItemName " & _
                                   "ORDER BY mst.olName, SUM(dtl.Qty) DESC")
            Else
                connDb.resetCmd("SELECT mst.olName " & _
                                 ", dtl.ItemName " & _
                                 ", SUM(dtl.Qty) AS Qty " & _
                                 ", SUM(dtl.Price * Qty) AS Amount " & _
                              "FROM cld_salesdetail dtl " & _
                                 "INNER JOIN cld_salesmaster mst ON (dtl.smNo = mst.Number and dtl.outletid = mst.olid and dtl.Custid = mst.custid) " & _
                                 "inner join cld_member m on (m.Number = mst.MemberNo and  m.Custid = mst.Custid) " & _
                              "WHERE mst.flag=1 AND dtl.flag=1 AND mst.BusiDate >=@paramFrom AND mst.BusiDate <=@paramTo AND (mst.olNumber=@outletNo OR @outletNo='') " & _
                                " AND (m.ID =@memberID or @memberID=0) " & _
                              "GROUP BY mst.olName, dtl.ItemName " & _
                              "ORDER BY mst.olName, SUM(dtl.Qty) DESC")
            End If

            connDb.addParam(paramFrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(paramTo, "@paramTo", MySqlDbType.DateTime)
            connDb.addParam(outletNo, "@outletNo", MySqlDbType.VarChar)
            connDb.addParam(memberID, "@memberID", MySqlDbType.Int32)

            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:itemByQty")
            Err.Clear()
        End Try

        Return dsRslt

    End Function

    'ga ada
    'Revisi 2 feb 2017
    Private Function itemStoreChart(ByVal paramFrom As Date, ByVal paramTo As Date, ByVal outletNo As String, ByVal memberID As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim dsRslt As New DataSet

        Try
            Dim connDb As New clsDbConn(ServerString)
            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then

                connDb.resetCmd("SELECT mst.olName " & _
                                     ", count(*) AS Qty " & _
                                     ", SUM(mst.total) AS Amount " & _
                                  "FROM cld_salesmaster mst " & _
                                     "inner join cld_member m on (m.Number = mst.MemberNo and m.custid = mst.custid) " & _
                                  "WHERE mst.flag=1 AND mst.BusiDate >=@paramFrom AND mst.BusiDate <=@paramTo " & _
                                    "AND ((mst.olNumber=@outletNo AND @outletNo<>'') or (@outletNo='' AND (mst.olid IN (Select Store from cld_userxstore where user = '" & usr & "'))) ) " & _
                                    " AND (m.ID =@memberID or @memberID=0) " & _
                                  "GROUP BY mst.olName " & _
                                  "ORDER BY mst.olName")
            Else

                connDb.resetCmd("SELECT mst.olName " & _
                                    ", count(*) AS Qty " & _
                                    ", SUM(mst.total) AS Amount " & _
                                 "FROM cld_salesmaster mst " & _
                                    "inner join cld_member m on (m.Number = mst.MemberNo and m.custid = mst.custid) " & _
                                 "WHERE mst.flag=1 AND mst.BusiDate >=@paramFrom AND mst.BusiDate <=@paramTo AND (mst.olNumber=@outletNo  OR @outletNo='') " & _
                                   " AND (m.ID =@memberID or @memberID=0) " & _
                                 "GROUP BY mst.olName " & _
                                 "ORDER BY mst.olName")

            End If
            connDb.addParam(paramFrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(paramTo, "@paramTo", MySqlDbType.DateTime)
            connDb.addParam(outletNo, "@outletNo", MySqlDbType.VarChar)
            connDb.addParam(memberID, "@memberID", MySqlDbType.Int32)

            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:itemStoreChart")
            Err.Clear()
        End Try

        Return dsRslt

    End Function

    'ga ada
    'Revisi 08 Juni
    Private Function salesItemByHour(ByVal paramFrom As Date, ByVal paramTo As Date, ByVal outletNo As String, ByVal memberID As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim dsRslt As New DataSet

        Try
            Dim connDb As New clsDbConn(ServerString)
            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then


                connDb.resetCmd("SELECT HOUR(mst.transdate) AS jam" & _
                                       ", dtl.ItemName " & _
                                       ", SUM(dtl.Qty) AS Qty " & _
                                       ", SUM(dtl.Price * Qty) AS Amount " & _
                                    "FROM cld_salesdetail dtl " & _
                                       "INNER JOIN cld_salesmaster mst ON (dtl.smNo = mst.Number and dtl.outletid = mst.olid and dtl.Custid = mst.custid) " & _
                                       "inner join cld_member m on (m.Number = mst.MemberNo and m.custid = mst.custid) " & _
                                    "WHERE mst.flag=1 AND dtl.flag=1 AND mst.BusiDate >=@paramFrom AND mst.BusiDate <=@paramTo AND (mst.olNumber=@outletNo  OR mst.olid IN (SELECT store FROM cld_userxstore WHERE USER = '" & usr & "' )) " & _
                                      " AND (m.ID =@memberID or @memberID=0) " & _
                                    "GROUP BY HOUR(mst.transdate), dtl.ItemName  " & _
                                    "ORDER BY HOUR(mst.transdate), SUM(dtl.Qty) DESC")
            Else
                connDb.resetCmd("SELECT HOUR(mst.transdate) AS jam" & _
                                       ", dtl.ItemName " & _
                                       ", SUM(dtl.Qty) AS Qty " & _
                                       ", SUM(dtl.Price * Qty) AS Amount " & _
                                    "FROM cld_salesdetail dtl " & _
                                       "INNER JOIN cld_salesmaster mst ON (dtl.smNo = mst.Number and dtl.outletid = mst.olid and dtl.Custid = mst.custid) " & _
                                       "inner join cld_member m on (m.Number = mst.MemberNo and m.custid = mst.custid) " & _
                                    "WHERE mst.flag=1 AND dtl.flag=1 AND mst.BusiDate >=@paramFrom AND mst.BusiDate <=@paramTo AND (mst.olNumber=@outletNo  OR @outletNo='') " & _
                                      " AND (m.ID =@memberID or @memberID=0) " & _
                                    "GROUP BY HOUR(mst.transdate), dtl.ItemName  " & _
                                    "ORDER BY HOUR(mst.transdate), SUM(dtl.Qty) DESC")
            End If
            connDb.addParam(paramFrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(paramTo, "@paramTo", MySqlDbType.DateTime)
            connDb.addParam(outletNo, "@outletNo", MySqlDbType.VarChar)
            connDb.addParam(memberID, "@memberID", MySqlDbType.Int32)

            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:itemByQty")
            Err.Clear()
        End Try

        Return dsRslt
    End Function

    '"AND ((olNumber=@outletNo AND @outletNo<>'') or (@outletNo='' AND (olid IN (Select Store from cld_userxstore where user = '" & usr & "'))) ) " & _
    'ga ada
    'Revisi 2 Feb 2017
    Private Function ItemHourChart(ByVal paramFrom As Date, ByVal paramTo As Date, ByVal outletNo As String, ByVal memberID As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim dsRslt As New DataSet

        Try
            Dim connDb As New clsDbConn(ServerString)
            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                connDb.resetCmd("SELECT HOUR(mst.transdate) AS jam" & _
                                         ", count(*) AS Qty " & _
                                         ", SUM(mst.total) AS Amount " & _
                                      "FROM cld_salesmaster mst " & _
                                         "inner join cld_member m on (m.Number = mst.MemberNo and m.custid = mst.custid) " & _
                                      "WHERE mst.flag =1 AND mst.BusiDate >=@paramFrom AND mst.BusiDate <=@paramTo " & _
                                        "AND ((mst.olNumber=@outletNo AND @outletNo<>'') or (@outletNo='' AND (mst.olid IN (Select Store from cld_userxstore where user = '" & usr & "'))) ) " & _
                                        " AND (m.ID =@memberID or @memberID=0) " & _
                                      "GROUP BY HOUR(mst.transdate) " & _
                                      "ORDER BY HOUR(mst.transdate)")

            Else
                connDb.resetCmd("SELECT HOUR(mst.transdate) AS jam" & _
                                         ", count(*) AS Qty " & _
                                         ", SUM(mst.total) AS Amount " & _
                                      "FROM cld_salesmaster mst " & _
                                         "inner join cld_member m on (m.Number = mst.MemberNo and m.custid = mst.custid) " & _
                                      "WHERE mst.flag =1 AND mst.BusiDate >=@paramFrom AND mst.BusiDate <=@paramTo AND (mst.olNumber=@outletNo  OR @outletNo = '') " & _
                                        " AND (m.ID =@memberID or @memberID=0) " & _
                                      "GROUP BY HOUR(mst.transdate) " & _
                                      "ORDER BY HOUR(mst.transdate)")
            End If
            connDb.addParam(paramFrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(paramTo, "@paramTo", MySqlDbType.DateTime)
            connDb.addParam(outletNo, "@outletNo", MySqlDbType.VarChar)
            connDb.addParam(memberID, "@memberID", MySqlDbType.Int32)

            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:itemByQty")
            Err.Clear()
        End Try

        Return dsRslt

    End Function

    'Revisi 08 Juni
    Private Function topItemQty(ByVal paramFrom As Date, ByVal paramTo As Date, ByVal Outletstr As String, ByVal iOwn As String, ByVal iBrand As String, ByVal iCity As String, ByVal custID As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim dsRslt As New DataSet
        Dim sOut As String = ""
        Dim sBrand As String = ""
        Dim sOwn As String = ""
        Dim sCity As String = ""
        Try

            Dim connDb As New clsDbConn(ServerString)
            If iBrand = "0" Or iBrand = "" Then
                sBrand = ""
            Else
                sBrand = " and o.brandID in (" & iBrand & ")"
            End If

            If iOwn = "0" Or iOwn = "" Then
                sOwn = ""
            Else
                sOwn = " and o.ownID = " & iOwn
            End If
            If iCity = "0" Or iCity = "" Then
                sCity = ""
            Else
                sCity = " and o.cityID in (" & iCity & ")"
            End If

            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If Outletstr = "" Then
                    sOut = " and sd.olId in (Select Store From cld_userxstore where user = '" & usr & "')"
                Else
                    sOut = " and sd.olnumber in (" & Outletstr & ")"
                End If
            Else
                If Outletstr = "" Then
                    sOut = ""
                Else
                    sOut = " and sd.olnumber in (" & Outletstr & ")"
                End If
            End If

            'connDb.resetCmd("SELECT sd.ItemNo, sd.itemName " &
            '                        ",sd.itemGroup " &
            '                        ",SUM(sd.Qty) AS qty " &
            '                        ",SUM(sd.total) AS subtotal " &
            '                    "FROM cld_sumsalesdetaildaily sd INNER JOIN " &
            '                        "cld_outlet o ON (o.id = sd.olid and o.Custid = sd.Custid) " &
            '                    "where sd.busidate >=@paramFrom AND sd.busiDate <=@paramTo " &
            '                        "AND (sd.CustID =@custID OR @custID=0) " & sOut & sOwn & sBrand & sCity &
            '                    " GROUP BY sd.ItemNo, sd.itemName, sd.itemGroup " &
            '                    "order by sum(Qty) desc " &
            '                    "limit 10")

            connDb.resetCmd("SELECT sd.ItemNo AS 'Item Code', sd.itemName AS 'Item Name' ,sd.itemGroup AS 'Item Group' ,SUM(sd.Qty) AS Qty ,SUM(sd.total) AS 'Net Sales' 
                            FROM cld_sumsalesdetaildaily sd INNER JOIN cld_outlet o ON (o.id = sd.olid AND o.Custid = sd.Custid) where sd.total>0 and sd.busidate >=@paramFrom AND sd.busiDate <=@paramTo AND 
                            (sd.CustID =@custID OR @custID=0) " & sOut & sOwn & sBrand & sCity &
"GROUP BY sd.ItemNo, sd.itemGroup order by sum(Qty) desc limit 10")

            connDb.addParam(paramFrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(paramTo, "@paramTo", MySqlDbType.DateTime)
            'connDb.addParam(outletNo, "@outletNo", MySqlDbType.VarChar)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)

            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:topItemQty")
            Err.Clear()
        End Try

        Return dsRslt
    End Function
    'Revisi 08 Juni
    Private Function topItemAmt(ByVal paramFrom As Date, ByVal paramTo As Date, ByVal outletstr As String, ByVal ownID As String, ByVal brandstr As String, ByVal citystr As String, ByVal custID As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim dsRslt As New DataSet
        Dim sOut As String = ""
        Dim sBrand As String = ""
        Dim sOwn As String = ""
        Dim sCity As String = ""

        Try
            Dim connDb As New clsDbConn(ServerString)



            If brandstr = "0" Or brandstr = "" Then
                sBrand = ""
            Else
                sBrand = " and o.brandID in (" & brandstr & ")"
            End If

            If ownID = "0" Or ownID = "" Then
                sOwn = ""
            Else
                sOwn = " and o.ownID = " & ownID
            End If
            If citystr = "0" Or citystr = "" Then
                sCity = ""
            Else
                sCity = " and o.cityID in (" & citystr & ")"
            End If

            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If outletstr = "" Then
                    sOut = " and sd.olId in (Select Store From cld_userxstore where user = '" & usr & "')"
                Else
                    sOut = " and sd.olnumber in (" & outletstr & ")"
                End If
            Else
                If outletstr = "" Then
                    sOut = ""
                Else
                    sOut = " and sd.olnumber in (" & outletstr & ")"
                End If
            End If


            'connDb.resetCmd("SELECT sd.ItemNo, sd.itemName " &
            '                       ",sd.itemGroup " &
            '                       ",SUM(sd.Qty) AS qty " &
            '                       ",SUM(sd.total) AS subtotal " &
            '                   "FROM cld_sumsalesdetaildaily sd INNER JOIN " &
            '                       "cld_outlet o ON (o.id = sd.olid and o.Custid = sd.Custid) " &
            '                   "where sd.busidate >=@paramFrom AND sd.busiDate <=@paramTo " &
            '                        "AND (sd.CustID=@custID OR @custID =0) " & sOut & sOwn & sBrand & sCity &
            '                   " GROUP BY sd.ItemNo, sd.itemName, sd.itemGroup " &
            '                   "order by sum(sd.total) desc " &
            '                   "limit 10")

            connDb.resetCmd("SELECT sd.ItemNo AS 'Item Code', sd.itemName AS 'Item Name' ,sd.itemGroup ' Item Group',SUM(sd.Qty) AS Qty ,SUM(sd.total) AS 'Net Sales' FROM cld_sumsalesdetaildaily sd INNER JOIN cld_outlet o ON (o.id = sd.olid AND o.Custid = sd.Custid)
WHERE sd.total>0 and sd.busidate >=@paramFrom And sd.busiDate <=@paramTo And (sd.CustID=@custID Or @custID =0) " & sOut & sOwn & sBrand & sCity &
"GROUP BY sd.ItemNo, sd.itemGroup ORDER BY SUM(sd.total) DESC LIMIT 10")



            connDb.addParam(paramFrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(paramTo, "@paramTo", MySqlDbType.DateTime)
            'connDb.addParam(outletNo, "@outletNo", MySqlDbType.VarChar)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)

            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing


        Catch ex As Exception
            errLog(Err.Description, "ClsReports: topItemAmt")
            Err.Clear()
        End Try

        Return dsRslt
    End Function

    Private Function top10item(ByVal paramFrom As Date, ByVal paramTo As Date, ByVal outletstr As String, ByVal ownID As String, ByVal brandstr As String, ByVal citystr As String, ByVal custID As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim dsRslt As New DataSet
        Dim sOut As String = ""
        Dim sBrand As String = ""
        Dim sOwn As String = ""
        Dim sCity As String = ""

        Try
            Dim connDb As New clsDbConn(ServerString)



            If brandstr = "0" Or brandstr = "" Then
                sBrand = ""
            Else
                sBrand = " and o.brandID in (" & brandstr & ")"
            End If

            If ownID = "0" Or ownID = "" Then
                sOwn = ""
            Else
                sOwn = " and o.ownID = " & ownID
            End If
            If citystr = "0" Or citystr = "" Then
                sCity = ""
            Else
                sCity = " and o.cityID in (" & citystr & ")"
            End If

            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If outletstr = "" Then
                    sOut = " and sd.olId in (Select Store From cld_userxstore where user = '" & usr & "')"
                Else
                    sOut = " and sd.olnumber in (" & outletstr & ")"
                End If
            Else
                If outletstr = "" Then
                    sOut = ""
                Else
                    sOut = " and sd.olnumber in (" & outletstr & ")"
                End If
            End If


            '            connDb.resetCmd("(select 'By Amount' AS 'Item Code','' as 'Item Name','' as 'Item Group','' AS Qty,'' AS 'Net Sales') union all
            '(SELECT sd.ItemNo AS 'Item Code', sd.itemName AS 'Item Name' ,sd.itemGroup ' Item Group',SUM(sd.Qty) AS Qty ,SUM(sd.total) AS 'Net Sales' FROM cld_sumsalesdetaildaily sd INNER JOIN cld_outlet o ON (o.id = sd.olid AND o.Custid = sd.Custid)
            'WHERE sd.total>0 and sd.busidate >=@paramFrom And sd.busiDate <=@paramTo And (sd.CustID=@custID Or @custID =0) " & sOut & sOwn & sBrand & sCity &
            '"GROUP BY sd.ItemNo, sd.itemName, sd.itemGroup ORDER BY SUM(sd.total) DESC LIMIT 10)" &
            '"Union All (select '','','','','')union all(select 'By Qty','','','','') union all" &
            '            "(SELECT sd.ItemNo AS 'Item Code', sd.itemName AS 'Item Name' ,sd.itemGroup AS 'Item Group' ,SUM(sd.Qty) AS Qty ,SUM(sd.total) AS 'Net Sales' 
            '                            FROM cld_sumsalesdetaildaily sd INNER JOIN cld_outlet o ON (o.id = sd.olid AND o.Custid = sd.Custid) where sd.total>0 and sd.busidate >=@paramFrom AND sd.busiDate <=@paramTo AND 
            '                            (sd.CustID =@custID OR @custID=0) " & sOut & sOwn & sBrand & sCity &
            '"GROUP BY sd.ItemNo, sd.itemName, sd.itemGroup order by sum(Qty) desc limit 10)")

            connDb.resetCmd("(select 'By Amount' AS 'Item Code','' AS 'Item Name','' as 'Item Group','' AS Qty,'' AS 'Net Sales') union all
(SELECT sd.ItemNo AS 'Item Code', sd.itemName AS 'Item Name' ,sd.itemGroup 'Item Group',FORMAT(SUM(sd.Qty), 0,'de_DE') AS Qty ,FORMAT(SUM(sd.total), 0,'de_DE') AS 'Net Sales' FROM cld_sumsalesdetaildaily sd INNER JOIN cld_outlet o ON (o.id = sd.olid AND o.Custid = sd.Custid)
WHERE sd.total>0 and sd.busidate >=@paramFrom And sd.busiDate <=@paramTo And (sd.CustID=@custID Or @custID =0) " & sOut & sOwn & sBrand & sCity &
"GROUP BY sd.ItemNo, sd.itemGroup ORDER BY SUM(sd.total) DESC LIMIT 10)" &
"Union All (select '','','','','')union all(select 'By Qty','','','','') union all" &
            "(SELECT sd.ItemNo AS 'Item Code', sd.itemName AS 'Item Name' ,sd.itemGroup AS 'Item Group' ,FORMAT(SUM(sd.Qty), 0,'de_DE') AS Qty ,FORMAT(SUM(sd.total), 0,'de_DE') AS 'Net Sales' 
                            FROM cld_sumsalesdetaildaily sd INNER JOIN cld_outlet o ON (o.id = sd.olid AND o.Custid = sd.Custid) where sd.total>0 and sd.busidate >=@paramFrom AND sd.busiDate <=@paramTo AND 
                            (sd.CustID =@custID OR @custID=0) " & sOut & sOwn & sBrand & sCity &
"GROUP BY sd.ItemNo, sd.itemGroup order by sum(Qty) desc limit 10)")

            connDb.addParam(paramFrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(paramTo, "@paramTo", MySqlDbType.DateTime)
            'connDb.addParam(outletNo, "@outletNo", MySqlDbType.VarChar)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)

            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing


        Catch ex As Exception
            errLog(Err.Description, "ClsReports: topItemAmt")
            Err.Clear()
        End Try

        Return dsRslt
    End Function

    Private Function top10item_gv(ByVal paramFrom As Date, ByVal paramTo As Date, ByVal outletstr As String, ByVal ownID As String, ByVal brandstr As String, ByVal citystr As String, ByVal custID As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim dsRslt As New DataSet
        Dim sOut As String = ""
        Dim sBrand As String = ""
        Dim sOwn As String = ""
        Dim sCity As String = ""

        Try
            Dim connDb As New clsDbConn(ServerString)



            If brandstr = "0" Or brandstr = "" Then
                sBrand = ""
            Else
                sBrand = " and o.brandID in (" & brandstr & ")"
            End If

            If ownID = "0" Or ownID = "" Then
                sOwn = ""
            Else
                sOwn = " and o.ownID = " & ownID
            End If
            If citystr = "0" Or citystr = "" Then
                sCity = ""
            Else
                sCity = " and o.cityID in (" & citystr & ")"
            End If

            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If outletstr = "" Then
                    sOut = " and sd.olId in (Select Store From cld_userxstore where user = '" & usr & "')"
                Else
                    sOut = " and sd.olnumber in (" & outletstr & ")"
                End If
            Else
                If outletstr = "" Then
                    sOut = ""
                Else
                    sOut = " and sd.olnumber in (" & outletstr & ")"
                End If
            End If


            connDb.resetCmd("(select 'By Amount' AS 'Item Code','' AS 'Item Name','' as 'Item Group','' AS Qty,'' AS 'Net Sales') union all
(SELECT sd.ItemNo AS 'Item Code', sd.itemName AS 'Item Name' ,sd.itemGroup 'Item Group',FORMAT(SUM(sd.Qty), 0,'de_DE') AS Qty ,FORMAT(SUM(sd.total), 2,'de_DE') AS 'Net Sales' FROM cld_sumsalesdetaildaily sd INNER JOIN cld_outlet o ON (o.id = sd.olid AND o.Custid = sd.Custid)
WHERE sd.total>0 and sd.busidate >=@paramFrom And sd.busiDate <=@paramTo And (sd.CustID=@custID Or @custID =0) " & sOut & sOwn & sBrand & sCity &
"GROUP BY sd.ItemNo, sd.itemGroup ORDER BY SUM(sd.total) DESC LIMIT 10)" &
"Union All (select '','','','','')union all(select 'By Qty','','','','') union all" &
            "(SELECT sd.ItemNo AS 'Item Code', sd.itemName AS 'Item Name' ,sd.itemGroup AS 'Item Group' ,FORMAT(SUM(sd.Qty), 0,'de_DE') AS Qty ,FORMAT(SUM(sd.total), 2,'de_DE') AS 'Net Sales' 
                            FROM cld_sumsalesdetaildaily sd INNER JOIN cld_outlet o ON (o.id = sd.olid AND o.Custid = sd.Custid) where sd.total>0 and sd.busidate >=@paramFrom AND sd.busiDate <=@paramTo AND 
                            (sd.CustID =@custID OR @custID=0) " & sOut & sOwn & sBrand & sCity &
"GROUP BY sd.ItemNo, sd.itemGroup order by sum(Qty) desc limit 10)")



            connDb.addParam(paramFrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(paramTo, "@paramTo", MySqlDbType.DateTime)
            'connDb.addParam(outletNo, "@outletNo", MySqlDbType.VarChar)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)

            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing


        Catch ex As Exception
            errLog(Err.Description, "ClsReports: topItemAmt")
            Err.Clear()
        End Try

        Return dsRslt
    End Function

    Private Function topItemQty_gv(ByVal paramFrom As Date, ByVal paramTo As Date, ByVal Outletstr As String, ByVal iOwn As String, ByVal iBrand As String, ByVal iCity As String, ByVal custID As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim dsRslt As New DataSet
        Dim sOut As String = ""
        Dim sBrand As String = ""
        Dim sOwn As String = ""
        Dim sCity As String = ""
        Try

            Dim connDb As New clsDbConn(ServerString)
            If iBrand = "0" Or iBrand = "" Then
                sBrand = ""
            Else
                sBrand = " and o.brandID in (" & iBrand & ")"
            End If

            If iOwn = "0" Or iOwn = "" Then
                sOwn = ""
            Else
                sOwn = " and o.ownID = " & iOwn
            End If
            If iCity = "0" Or iCity = "" Then
                sCity = ""
            Else
                sCity = " and o.cityID in (" & iCity & ")"
            End If

            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If Outletstr = "" Then
                    sOut = " and sd.olId in (Select Store From cld_userxstore where user = '" & usr & "')"
                Else
                    sOut = " and sd.olnumber in (" & Outletstr & ")"
                End If
            Else
                If Outletstr = "" Then
                    sOut = ""
                Else
                    sOut = " and sd.olnumber in (" & Outletstr & ")"
                End If
            End If

            'connDb.resetCmd("SELECT sd.ItemNo, sd.itemName " &
            '                        ",sd.itemGroup " &
            '                        ",SUM(sd.Qty) AS qty " &
            '                        ",FORMAT(SUM(sd.total), 2,'de_DE') AS subtotal " &
            '                    "FROM cld_sumsalesdetaildaily sd INNER JOIN " &
            '                        "cld_outlet o ON (o.id = sd.olid and o.Custid = sd.Custid) " &
            '                    "where sd.busidate >=@paramFrom AND sd.busiDate <=@paramTo " &
            '                        "AND (sd.CustID =@custID OR @custID=0) " & sOut & sOwn & sBrand & sCity &
            '                    " GROUP BY sd.ItemNo, sd.itemName, sd.itemGroup " &
            '                    "order by sum(Qty) desc " &
            '                    "limit 10")
            connDb.resetCmd("SELECT sd.ItemNo AS 'Item Code', sd.itemName AS 'Item Name' ,sd.itemGroup AS 'Item Group' ,FORMAT(SUM(sd.Qty), 0,'de_DE') AS Qty ,FORMAT(SUM(sd.total), 2,'de_DE') AS 'Net Sales' 
                            FROM cld_sumsalesdetaildaily sd INNER JOIN cld_outlet o ON (o.id = sd.olid AND o.Custid = sd.Custid) where sd.total>0 and sd.busidate >=@paramFrom AND sd.busiDate <=@paramTo AND 
                            (sd.CustID =@custID OR @custID=0) " & sOut & sOwn & sBrand & sCity &
"GROUP BY sd.ItemNo, sd.itemGroup order by sum(Qty) desc limit 10")


            connDb.addParam(paramFrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(paramTo, "@paramTo", MySqlDbType.DateTime)
            'connDb.addParam(outletNo, "@outletNo", MySqlDbType.VarChar)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)

            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:topItemQty")
            Err.Clear()
        End Try

        Return dsRslt
    End Function
    'Revisi 08 Juni
    Private Function topItemAmt_gv(ByVal paramFrom As Date, ByVal paramTo As Date, ByVal outletstr As String, ByVal ownID As String, ByVal brandstr As String, ByVal citystr As String, ByVal custID As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim dsRslt As New DataSet
        Dim sOut As String = ""
        Dim sBrand As String = ""
        Dim sOwn As String = ""
        Dim sCity As String = ""

        Try
            Dim connDb As New clsDbConn(ServerString)



            If brandstr = "0" Or brandstr = "" Then
                sBrand = ""
            Else
                sBrand = " and o.brandID in (" & brandstr & ")"
            End If

            If ownID = "0" Or ownID = "" Then
                sOwn = ""
            Else
                sOwn = " and o.ownID = " & ownID
            End If
            If citystr = "0" Or citystr = "" Then
                sCity = ""
            Else
                sCity = " and o.cityID in (" & citystr & ")"
            End If

            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If outletstr = "" Then
                    sOut = " and sd.olId in (Select Store From cld_userxstore where user = '" & usr & "')"
                Else
                    sOut = " and sd.olnumber in (" & outletstr & ")"
                End If
            Else
                If outletstr = "" Then
                    sOut = ""
                Else
                    sOut = " and sd.olnumber in (" & outletstr & ")"
                End If
            End If


            'connDb.resetCmd("SELECT sd.ItemNo, sd.itemName " &
            '                       ",sd.itemGroup " &
            '                       ",SUM(sd.Qty) AS qty " &
            '                       ",FORMAT(SUM(sd.total), 2,'de_DE') AS subtotal " &
            '                   "FROM cld_sumsalesdetaildaily sd INNER JOIN " &
            '                       "cld_outlet o ON (o.id = sd.olid and o.Custid = sd.Custid) " &
            '                   "where sd.busidate >=@paramFrom AND sd.busiDate <=@paramTo " &
            '                        "AND (sd.CustID=@custID OR @custID =0) " & sOut & sOwn & sBrand & sCity &
            '                   " GROUP BY sd.ItemNo, sd.itemName, sd.itemGroup " &
            '                   "order by sum(sd.total) desc " &
            '                   "limit 10")

            connDb.resetCmd("SELECT sd.ItemNo AS 'Item Code', sd.itemName AS 'Item Name' ,sd.itemGroup 'Item Group',FORMAT(SUM(sd.Qty), 0,'de_DE') AS Qty ,FORMAT(SUM(sd.total), 2,'de_DE') AS 'Net Sales' FROM cld_sumsalesdetaildaily sd INNER JOIN cld_outlet o ON (o.id = sd.olid AND o.Custid = sd.Custid)
WHERE sd.total>0 and sd.busidate >=@paramFrom And sd.busiDate <=@paramTo And (sd.CustID=@custID Or @custID =0) " & sOut & sOwn & sBrand & sCity &
"GROUP BY sd.ItemNo, sd.itemGroup ORDER BY SUM(sd.total) DESC LIMIT 10")


            connDb.addParam(paramFrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(paramTo, "@paramTo", MySqlDbType.DateTime)
            'connDb.addParam(outletNo, "@outletNo", MySqlDbType.VarChar)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)

            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing


        Catch ex As Exception
            errLog(Err.Description, "ClsReports:topItemAmt")
            Err.Clear()
        End Try

        Return dsRslt
    End Function

    'Revisi 08 Juni
    Private Function topMemberSpend(ByVal paramFrom As Date, ByVal paramTo As Date, ByVal outletstr As String, ByVal ownID As Integer, ByVal brandstr As String, ByVal citystr As String, ByVal custID As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim dsRslt As New DataSet
        Dim sOut As String = ""
        Dim sBrand As String = ""
        Dim sOwn As String = ""
        Dim sCity As String = ""
        Try
            Dim connDb As New clsDbConn(ServerString)




            If brandstr = "0" Or brandstr = "" Then
                sBrand = ""
            Else
                sBrand = " and o.brandID in (" & brandstr & ")"
            End If

            If ownID = "0" Or ownID = "" Then
                sOwn = ""
            Else
                sOwn = " and o.ownID = " & ownID
            End If
            If citystr = "0" Or citystr = "" Then
                sCity = ""
            Else
                sCity = " and o.cityID in (" & citystr & ")"
            End If

            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If outletstr = "" Then
                    sOut = " and sm.olId in (Select Store From cld_userxstore where user = '" & usr & "')"
                Else
                    sOut = " and sm.olnumber in (" & outletstr & ")"
                End If
            Else
                If outletstr = "" Then
                    sOut = ""
                Else
                    sOut = " and sm.olnumber in (" & outletstr & ")"
                End If
            End If



            connDb.resetCmd("SELECT m.Number " &
                                    ",m.NAME " &
                                    ",count(*) as qty " &
                                    ",SUM(sm.Total) AS total " &
                                "FROM cld_salesmaster sm INNER JOIN " &
                                    "cld_member m ON (m.number = sm.MemberNo and m.Custid = sm.Custid) " &
                                "WHERE sm.flag=1 AND sm.busiDate >=@paramFrom AND sm.busiDate <=@paramTo " &
                                    "AND ((sm.olNumber=@outletNo AND @outletNo<>'') or (@outletNo='' AND (sm.olid IN (Select Store from cld_userxstore where user = '" & usr & "'))) ) " &
                                    "AND (sm.CustID=@custID OR @custID=0) " & sOut & sOwn & sBrand & sCity &
                                "GROUP BY m.number,m.NAME " &
                                "order by sum(sm.Total) desc " &
                                "limit 10")

            connDb.addParam(paramFrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(paramTo, "@paramTo", MySqlDbType.DateTime)
            'connDb.addParam(outletNo, "@outletNo", MySqlDbType.VarChar)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)

            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:topItemQty")
            Err.Clear()
        End Try

        Return dsRslt
    End Function
    'Revisi 08 Juni
    Private Function topMemberVisit(ByVal paramFrom As Date, ByVal paramTo As Date, ByVal outletNo As String, ByVal custID As Integer, ByVal usr As String, ByVal modprv As Integer) As DataSet
        Dim dsRslt As New DataSet

        Try
            Dim connDb As New clsDbConn(ServerString)

            If modprv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                connDb.resetCmd("SELECT m.Number " & _
                                    ",m.NAME " & _
                                    ",count(*) as qty " & _
                                    ",SUM(sm.Total) AS total " & _
                                "FROM cld_salesmaster sm INNER JOIN " & _
                                    "cld_member m ON (m.number = sm.MemberNo and m.Custid = sm.Custid) " & _
                                "WHERE sm.flag=1 AND sm.busiDate >=@paramFrom AND sm.busiDate <=@paramTo " & _
                                    "AND ((sm.olNumber=@outletNo AND @outletNo<>'') or (@outletNo='' AND (sm.olid IN (Select Store from cld_userxstore where user = '" & usr & "'))) ) " & _
                                    "AND (sm.CustID=@custID OR @custID=0) AND sm.memberno <> '' " & _
                                "GROUP BY m.number, m.NAME " & _
                                "order by count(*) desc " & _
                                "limit 10")

            Else

                connDb.resetCmd("SELECT m.Number " & _
                                    ",m.NAME " & _
                                    ",count(*) as qty " & _
                                    ",SUM(sm.Total) AS total " & _
                                "FROM cld_salesmaster sm INNER JOIN " & _
                                    "cld_member m ON (m.number = sm.MemberNo and m.Custid = sm.Custid) " & _
                                "WHERE sm.flag=1 AND sm.busiDate >=@paramFrom AND sm.busiDate <=@paramTo AND (olnumber =@outletNo OR @outletNo='') " & _
                                    "AND (sm.CustID=@custID OR @custID=0) AND sm.memberno <> '' " & _
                                "GROUP BY m.number, m.NAME " & _
                                "order by count(*) desc " & _
                                "limit 10")
            End If

            connDb.addParam(paramFrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(paramTo, "@paramTo", MySqlDbType.DateTime)
            connDb.addParam(outletNo, "@outletNo", MySqlDbType.VarChar)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)
            connDb.addParam(usr, "@usr", MySqlDbType.VarChar)
            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:topItemQty")
            Err.Clear()
        End Try

        Return dsRslt
    End Function



    'Revisi 08 Juni
    Private Function topStore(ByVal paramFrom As Date, ByVal paramTo As Date, ByVal Outletstr As String, ByVal iOwn As String, ByVal iBrand As String, ByVal iCity As String, ByVal custID As Integer, ByVal usr As String, ByVal modprv As Integer) As DataSet
        Dim dsRslt As New DataSet
        Dim sOut As String = ""
        Dim sBrand As String = ""
        Dim sOwn As String = ""
        Dim sCity As String = ""

        Try
            Dim connDB As New clsDbConn(ServerString)
            If iBrand = "0" Or iBrand = "" Then
                sBrand = ""
            Else
                sBrand = " and o.brandID in (" & iBrand & ")"
            End If

            If iOwn = "0" Or iOwn = "" Then
                sOwn = ""
            Else
                sOwn = " and o.ownID = " & iOwn
            End If
            If iCity = "0" Or iCity = "" Then
                sCity = ""
            Else
                sCity = " and o.cityID in (" & iCity & ")"
            End If

            If modprv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If Outletstr = "" Then
                    sOut = " and sm.olId in (Select Store From cld_userxstore where user = '" & usr & "')"
                Else
                    sOut = " and sm.olnumber in (" & Outletstr & ")"
                End If
            Else
                If Outletstr = "" Then
                    sOut = ""
                Else
                    sOut = " and sm.olnumber in (" & Outletstr & ")"
                End If
            End If


            connDB.resetCmd("SELECT sm.olNumber 'Store No', sm.olName 'Store Name', sum(qtytrans) 'Number Of Transaction', SUM(total) 'Gross Sales' " &
                          "FROM cld_sumsalesdaily sm " &
                              "INNER JOIN cld_outlet o ON (o.id = sm.olid and o.Custid = sm.Custid) " &
                          "where sm.busidate >=@paramFrom AND sm.busiDate <= @paramTo " &
                              "AND (sm.CustID=@custID OR @custID=0) " & sOut & sOwn & sBrand & sCity &
                          " GROUP BY sm.olnumber,sm.olname order by SUM(total) desc ")





            connDB.addParam(paramFrom, "@paramFrom", MySqlDbType.DateTime)
            connDB.addParam(paramTo, "@paramTO", MySqlDbType.DateTime)
            connDB.addParam(custID, "@custID", MySqlDbType.Int32)

            dsRslt = connDB.selectDataset

            connDB.dispose()
            connDB = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReport:topStoreAmt")
            Err.Clear()
        End Try

        Return dsRslt

    End Function

    Private Function topStore_gv(ByVal paramFrom As Date, ByVal paramTo As Date, ByVal Outletstr As String, ByVal iOwn As String, ByVal iBrand As String, ByVal iCity As String, ByVal custID As Integer, ByVal usr As String, ByVal modprv As Integer) As DataSet
        Dim dsRslt As New DataSet
        Dim sOut As String = ""
        Dim sBrand As String = ""
        Dim sOwn As String = ""
        Dim sCity As String = ""

        Try
            Dim connDB As New clsDbConn(ServerString)
            If iBrand = "0" Or iBrand = "" Then
                sBrand = ""
            Else
                sBrand = " and o.brandID in (" & iBrand & ")"
            End If

            If iOwn = "0" Or iOwn = "" Then
                sOwn = ""
            Else
                sOwn = " and o.ownID = " & iOwn
            End If
            If iCity = "0" Or iCity = "" Then
                sCity = ""
            Else
                sCity = " and o.cityID in (" & iCity & ")"
            End If

            If modprv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If Outletstr = "" Then
                    sOut = " and sm.olId in (Select Store From cld_userxstore where user = '" & usr & "')"
                Else
                    sOut = " and sm.olnumber in (" & Outletstr & ")"
                End If
            Else
                If Outletstr = "" Then
                    sOut = ""
                Else
                    sOut = " and sm.olnumber in (" & Outletstr & ")"
                End If
            End If

            connDB.resetCmd("SELECT sm.olNumber 'Store No', sm.olName 'Store Name', sum(qtytrans) 'Number Of Transaction', FORMAT(SUM(total), 2,'de_DE') 'Gross Sales' " &
                          "FROM cld_sumsalesdaily sm " &
                              "INNER JOIN cld_outlet o ON (o.id = sm.olid and o.Custid = sm.Custid) " &
                          "where sm.busidate >=@paramFrom AND sm.busiDate <= @paramTo " &
                              "AND (sm.CustID=@custID OR @custID=0) " & sOut & sOwn & sBrand & sCity &
                          " GROUP BY sm.olnumber,sm.olname order by SUM(total) desc ")





            connDB.addParam(paramFrom, "@paramFrom", MySqlDbType.DateTime)
            connDB.addParam(paramTo, "@paramTO", MySqlDbType.DateTime)
            connDB.addParam(custID, "@custID", MySqlDbType.Int32)

            dsRslt = connDB.selectDataset

            connDB.dispose()
            connDB = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReport:topStoreAmt")
            Err.Clear()
        End Try

        Return dsRslt

    End Function

    'Revisi 08 Juni
    Public Function topStoreByHour(ByVal paramFrom As Date, ByVal paramTo As Date, ByVal outletstr As String, ByVal ownID As String, ByVal brandstr As String, ByVal citystr As String, ByVal custID As Integer, ByVal usr As String, ByVal modprv As Integer) As DataSet
        Dim dsRslt As New DataSet
        Dim sOut As String = ""
        Dim sBrand As String = ""
        Dim sOwn As String = ""
        Dim sCity As String = ""

        Try
            Dim connDb As New clsDbConn(ServerString)
            If brandstr = "0" Or brandstr = "" Then
                sBrand = ""
            Else
                sBrand = " and o.brandID in (" & brandstr & ")"
            End If

            If ownID = "0" Or ownID = "" Then
                sOwn = ""
            Else
                sOwn = " and o.ownID = " & ownID
            End If
            If citystr = "0" Or citystr = "" Then
                sCity = ""
            Else
                sCity = " and o.cityID in (" & citystr & ")"
            End If

            If modprv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If outletstr = "" Then
                    sOut = " and sm.olId in (Select Store From cld_userxstore where user = '" & usr & "')"
                Else
                    sOut = " and sm.olnumber in (" & outletstr & ")"
                End If
            Else
                If outletstr = "" Then
                    sOut = ""
                Else
                    sOut = " and sm.olnumber in (" & outletstr & ")"
                End If
            End If

            connDb.resetCmd("(SELECT olnumber 'Store Code',olname 'Store Name',Hour, trans 'Number Of Transaction', trans*100/alltrans '% Trans'
,gs 'Gross Sales',gs*100/allgs '% Gross Sales' FROM
(SELECT sm.olNumber, sm.olName, DATE_FORMAT(transdate,'%H:00') AS HOUR, COUNT(*) trans,
 SUM(total) gs FROM cld_salesMaster sm INNER JOIN cld_outlet o ON(o.id = sm.olid AND o.Custid = sm.Custid) 
 WHERE sm.flag=1 AND sm.busidate >=@paramFrom AND busiDate <= @paramTo
             AND (sm.CustID=@custID OR @custID=0) " & sOut & sOwn & sBrand & sCity &
" Group BY sm.olnumber,sm.olname, HOUR(transdate) ORDER BY SUM(total) DESC, DATE_FORMAT(transdate,'%Y:%M:%D %H:00'))AS t1
CROSS JOIN 
 (SELECT COUNT(*) alltrans,SUM(total) allgs FROM cld_salesMaster sm INNER JOIN cld_outlet o ON(o.id = sm.olid AND o.Custid = sm.Custid) 
 WHERE sm.flag=1 AND sm.busidate >=@paramFrom AND busiDate <= @paramTo 
                        AND (sm.CustID=@custID OR @custID=0) " & sOut & sOwn & sBrand & sCity & ") AS t2 )")
            '            UNION ALL
            ' SELECT '', 'Total','', COUNT(number),100
            ',SUM(total),100 FROM cld_salesMaster sm INNER JOIN cld_outlet o ON(o.id = sm.olid AND o.Custid = sm.Custid) 
            ' WHERE sm.flag=1 AND sm.busidate >=@paramFrom AND busiDate <= @paramTo 
            '                        AND (sm.CustID=@custID OR @custID=0) " & sOut & sOwn & sBrand & sCity)

            'connDb.resetCmd("SELECT sm.olNumber 'Store Code', sm.olName 'Store Name', DATE_FORMAT(transdate,'%H:00') AS jam, COUNT(*) AS visit, SUM(total) AS Revenue " &
            '             "FROM cld_salesMaster sm " &
            '                 "INNER JOIN cld_outlet o ON(o.id = sm.olid and o.Custid = sm.Custid) " &
            '             "WHERE sm.flag=1 AND sm.busidate >=@paramFrom AND busiDate <= @paramTo " &
            '               "AND (sm.CustID=@custID OR @custID=0) " & sOut & sOwn & sBrand & sCity &
            '             " GROUP BY sm.olnumber,sm.olname, HOUR(transdate) order by sm.olNumber, DATE_FORMAT(transdate,'%Y:%M:%D %H:00')")

            'connDb.resetCmd("(SELECT sm.olNumber 'Store Code', sm.olName 'Store Name', DATE_FORMAT(transdate,'%H:00') AS Hour, COUNT(*) 'Number Of Transaction', SUM(total) 'Gross Sales' " &
            '             "FROM cld_salesMaster sm " &
            '                 "INNER JOIN cld_outlet o ON(o.id = sm.olid and o.Custid = sm.Custid) " &
            '             "WHERE sm.flag=1 AND sm.busidate >=@paramFrom AND busiDate <= @paramTo " &
            '               "AND (sm.CustID=@custID OR @custID=0) " & sOut & sOwn & sBrand & sCity &
            '             " GROUP BY sm.olnumber,sm.olname, HOUR(transdate) order by SUM(total) desc, DATE_FORMAT(transdate,'%Y:%M:%D %H:00') ) ")



            connDb.addParam(paramFrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(paramTo, "@paramTo", MySqlDbType.DateTime)
            connDb.addParam(custID, "@custID", MySqlDbType.UInt32)

            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:topStoreByHour")
            Err.Clear()
        End Try

        Return dsRslt

    End Function
    Public Function topStoreByHour_gv(ByVal paramFrom As Date, ByVal paramTo As Date, ByVal outletstr As String, ByVal ownID As String, ByVal brandstr As String, ByVal citystr As String, ByVal custID As Integer, ByVal usr As String, ByVal modprv As Integer) As DataSet
        Dim dsRslt As New DataSet
        Dim sOut As String = ""
        Dim sBrand As String = ""
        Dim sOwn As String = ""
        Dim sCity As String = ""

        Try
            Dim connDb As New clsDbConn(ServerString)
            If brandstr = "0" Or brandstr = "" Then
                sBrand = ""
            Else
                sBrand = " and o.brandID in (" & brandstr & ")"
            End If

            If ownID = "0" Or ownID = "" Then
                sOwn = ""
            Else
                sOwn = " and o.ownID = " & ownID
            End If
            If citystr = "0" Or citystr = "" Then
                sCity = ""
            Else
                sCity = " and o.cityID in (" & citystr & ")"
            End If

            If modprv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If outletstr = "" Then
                    sOut = " and sm.olId in (Select Store From cld_userxstore where user = '" & usr & "')"
                Else
                    sOut = " and sm.olnumber in (" & outletstr & ")"
                End If
            Else
                If outletstr = "" Then
                    sOut = ""
                Else
                    sOut = " and sm.olnumber in (" & outletstr & ")"
                End If
            End If

            connDb.resetCmd("(SELECT olnumber 'Store Code',olname 'Store Name',Hour, trans 'Number Of Transaction', FORMAT(trans*100/alltrans, 2,'de_DE') '% Trans'
,FORMAT(gs, 0,'de_DE') 'Gross Sales',FORMAT(gs*100/allgs, 2,'de_DE') '% Gross Sales' FROM
(SELECT sm.olNumber, sm.olName, DATE_FORMAT(transdate,'%H:00') AS HOUR, COUNT(*) trans,
 SUM(total) gs FROM cld_salesMaster sm INNER JOIN cld_outlet o ON(o.id = sm.olid AND o.Custid = sm.Custid) 
 WHERE sm.flag=1 AND sm.busidate >=@paramFrom AND busiDate <= @paramTo
             AND (sm.CustID=@custID OR @custID=0) " & sOut & sOwn & sBrand & sCity &
" Group BY sm.olnumber,sm.olname, HOUR(transdate) ORDER BY SUM(total) DESC, DATE_FORMAT(transdate,'%Y:%M:%D %H:00'))AS t1
CROSS JOIN 
 (SELECT COUNT(*) alltrans,SUM(total) allgs FROM cld_salesMaster sm INNER JOIN cld_outlet o ON(o.id = sm.olid AND o.Custid = sm.Custid) 
 WHERE sm.flag=1 AND sm.busidate >=@paramFrom AND busiDate <= @paramTo 
                        AND (sm.CustID=@custID OR @custID=0) " & sOut & sOwn & sBrand & sCity & ") As t2 )
 UNION ALL
 SELECT '', 'Total','', COUNT(number),'100'
,FORMAT(SUM(total), 0,'de_DE'),'100' FROM cld_salesMaster sm INNER JOIN cld_outlet o ON(o.id = sm.olid AND o.Custid = sm.Custid) 
 WHERE sm.flag=1 AND sm.busidate >=@paramFrom AND busiDate <= @paramTo 
                        AND (sm.CustID=@custID OR @custID=0) " & sOut & sOwn & sBrand & sCity)

            'connDb.resetCmd("(SELECT sm.olNumber 'Store Code', sm.olName 'Store Name', DATE_FORMAT(transdate,'%H:00') AS Hour, COUNT(*) 'Number Of Transaction', FORMAT(SUM(total), 0,'de_DE') 'Gross Sales' " &
            '             "FROM cld_salesMaster sm " &
            '                 "INNER JOIN cld_outlet o ON(o.id = sm.olid and o.Custid = sm.Custid) " &
            '             "WHERE sm.flag=1 AND sm.busidate >=@paramFrom AND busiDate <= @paramTo " &
            '               "AND (sm.CustID=@custID OR @custID=0) " & sOut & sOwn & sBrand & sCity &
            '             " GROUP BY sm.olnumber,sm.olname, HOUR(transdate) order by SUM(total) desc, DATE_FORMAT(transdate,'%Y:%M:%D %H:00') ) ")

            'connDb.resetCmd("(SELECT sm.olNumber 'Store Code', sm.olName 'Store Name', DATE_FORMAT(transdate,'%H:00') AS jam, COUNT(*) AS visit, FORMAT(SUM(total), 0,'de_DE') AS Revenue " &
            '             "FROM cld_salesMaster sm " &
            '                 "INNER JOIN cld_outlet o ON(o.id = sm.olid and o.Custid = sm.Custid) " &
            '             "WHERE sm.flag=1 AND sm.busidate >=@paramFrom AND busiDate <= @paramTo " &
            '               "AND (sm.CustID=@custID OR @custID=0) " & sOut & sOwn & sBrand & sCity &
            '             " GROUP BY sm.olnumber,sm.olname, HOUR(transdate) order by sm.olNumber, DATE_FORMAT(transdate,'%Y:%M:%D %H:00')) " &
            '            " UNION ALL " &
            '             "SELECT '', 'Total', '' jam, COUNT(*) AS visit, FORMAT(SUM(total), 0,'de_DE') AS Revenue " &
            '             "FROM cld_salesMaster sm " &
            '                 "INNER JOIN cld_outlet o ON(o.id = sm.olid and o.Custid = sm.Custid) " &
            '             "WHERE sm.flag=1 AND sm.busidate >=@paramFrom AND busiDate <= @paramTo " &
            '               "AND (sm.CustID=@custID OR @custID=0) " & sOut & sOwn & sBrand & sCity)





            connDb.addParam(paramFrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(paramTo, "@paramTo", MySqlDbType.DateTime)
            connDb.addParam(custID, "@custID", MySqlDbType.UInt32)

            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:topStoreByHour")
            Err.Clear()
        End Try

        Return dsRslt

    End Function
    'Revisi 08 Juni

    Public Function memberPoint(ByVal paramFrom As Date, ByVal paramTo As Date, ByVal outletNo As String, ByVal custID As Integer, _
                                ByVal usr As String, ByVal modpriv As Integer, ByVal memberNo As String) As DataSet
        Dim dsRslt As New DataSet

        Try
            Dim connDb As New clsDbConn(ServerString)

            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then

                'connDb.resetCmd("SELECT m.Number, m.name, p.OlNumber, p.OlName" & _
                '               ", p.salesNo, p.transdate, p.total, p.PointGet, p.pointBefore, m.points " & _
                '           "FROM cld_memberPoint p INNER JOIN " & _
                '               "cld_member m ON (p.memberNo = m.number and p.custid = m.custid) " & _
                '           "WHERE p.flag <> 0 and p.Busidate >= @paramFrom AND p.Busidate <= @paramTo " & _
                '                "AND (p.memberNo=@memberNo OR @memberNo='') " & _
                '                "AND ((p.OlNumber =@outletNo AND @outletNo<>'') or (@outletNo='' AND p.olnumber in (SELECT o.kode FROM cld_userxstore ux INNER JOIN cld_outlet o ON (ux.store = o.ID) WHERE USER = '" & usr & "'))) AND ( p.CustID = @CustID OR @CustID=0)")


                connDb.resetCmd("SELECT m.Number, m.name, p.OlNumber, p.OlName" & _
                             ", p.salesNo, p.transdate, p.total, p.PointGet, p.pointBefore, m.points, p.flag " & _
                         "FROM cld_memberPoint p INNER JOIN " & _
                             "cld_member m ON (p.memberNo = m.number and p.custid = m.custid) " & _
                         "WHERE p.Busidate >= @paramFrom AND p.Busidate <= @paramTo " & _
                              "AND (p.memberNo=@memberNo OR @memberNo='') " & _
                              "AND ((p.OlNumber =@outletNo AND @outletNo<>'') or (@outletNo='' AND p.olnumber in (SELECT o.kode FROM cld_userxstore ux INNER JOIN cld_outlet o ON (ux.store = o.ID) WHERE USER = '" & usr & "'))) AND ( p.CustID = @CustID OR @CustID=0)")


            Else
                'connDb.resetCmd("SELECT m.Number, m.name, p.OlNumber, p.OlName" & _
                '               ", p.salesNo, p.transdate, p.total, p.PointGet, p.pointBefore, m.points " & _
                '           "FROM cld_memberPoint p INNER JOIN " & _
                '               "cld_member m ON (p.memberNo = m.number and p.custid = m.custid) " & _
                '           "WHERE p.flag <> 0 and p.Busidate >= @paramFrom AND p.Busidate <= @paramTo " & _
                '                "AND (p.memberNo=@memberNo OR @memberNo='') " & _
                '                "AND (p.OlNumber =@outletNo OR @outletNo = '') AND ( p.CustID = @CustID OR @CustID=0)")

                connDb.resetCmd("SELECT m.Number, m.name, p.OlNumber, p.OlName" & _
                              ", p.salesNo, p.transdate, p.total, p.PointGet, p.pointBefore, m.points, p.flag " & _
                          "FROM cld_memberPoint p INNER JOIN " & _
                              "cld_member m ON (p.memberNo = m.number and p.custid = m.custid) " & _
                          "WHERE p.Busidate >= @paramFrom AND p.Busidate <= @paramTo " & _
                               "AND (p.memberNo=@memberNo OR @memberNo='') " & _
                               "AND (p.OlNumber =@outletNo OR @outletNo = '') AND ( p.CustID = @CustID OR @CustID=0)")

            End If


            connDb.addParam(paramFrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(paramTo, "@paramTo", MySqlDbType.DateTime)
            connDb.addParam(outletNo, "@outletNo", MySqlDbType.String)
            connDb.addParam(memberNo, "@memberNo", MySqlDbType.String)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)

            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:memberPoint")
            Err.Clear()
        End Try

        Return dsRslt

    End Function

    Public Function memberRewardPoint(ByVal paramFrom As Date, ByVal paramTo As Date, ByVal outletNo As String, ByVal custID As Integer, _
                               ByVal usr As String, ByVal modpriv As Integer, ByVal memberNo As String) As DataSet
        Dim dsRslt As New DataSet

        Try
            Dim connDb As New clsDbConn(ServerString)

            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then

                connDb.resetCmd("SELECT m.Number, m.name, p.OlNumber, p.OlName" & _
                               ", p.salesNo, p.transdate, p.total, p.PointGet, p.pointBefore, m.points " & _
                           "FROM cld_memberPoint p INNER JOIN " & _
                               "cld_member m ON (p.memberNo = m.number and p.custid = m.custid) " & _
                           "WHERE p.flag = 4 and p.Busidate >= @paramFrom AND p.Busidate <= @paramTo " & _
                                "AND (p.memberNo=@memberNo OR @memberNo='') " & _
                                "AND ((p.OlNumber =@outletNo AND @outletNo<>'') or (@outletNo='' AND p.olnumber in (SELECT o.kode FROM cld_userxstore ux INNER JOIN cld_outlet o ON (ux.store = o.ID) WHERE USER = '" & usr & "'))) AND ( p.CustID = @CustID OR @CustID=0)")

            Else
                connDb.resetCmd("SELECT m.Number, m.name, p.OlNumber, p.OlName" & _
                               ", p.salesNo, p.transdate, p.total, p.PointGet, p.pointBefore, m.points " & _
                           "FROM cld_memberPoint p INNER JOIN " & _
                               "cld_member m ON (p.memberNo = m.number and p.custid = m.custid) " & _
                           "WHERE p.flag = 4 and p.Busidate >= @paramFrom AND p.Busidate <= @paramTo " & _
                                "AND (p.memberNo=@memberNo OR @memberNo='') " & _
                                "AND (p.OlNumber =@outletNo OR @outletNo = '') AND ( p.CustID = @CustID OR @CustID=0)")

            End If


            connDb.addParam(paramFrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(paramTo, "@paramTo", MySqlDbType.DateTime)
            connDb.addParam(outletNo, "@outletNo", MySqlDbType.String)
            connDb.addParam(memberNo, "@memberNo", MySqlDbType.String)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)

            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:memberPoint")
            Err.Clear()
        End Try

        Return dsRslt

    End Function


    Public Function memberpointEdit(ByVal paramFrom As Date, ByVal paramTo As Date, ByVal custID As Integer, ByVal usr As String, _
                                    ByVal modpriv As Integer, ByVal memberNo As String) As DataSet
        Dim dsRslt As New DataSet

        Try
            Dim connDb As New clsDbConn(ServerString)


            connDb.resetCmd("SELECT mp.`Usr`,mp.`Busidate`, m.name, mp.`memberNo`, mp.`PointGet`, mp.`PointBefore` " & _
                                "FROM cld_memberpoint mp LEFT JOIN cld_member m ON mp.`memberNo` = m.`Number` AND mp.`CustID` = m.`CustID` " & _
                           "where mp.BusiDate >=@paramFrom AND mp.BusiDate <=@paramTo " & _
                                "AND (mp.memberNo= @memberNo OR @memberNo='') " & _
                                 "AND (mp.CustID =@custID OR @custID=0) AND mp.flag = 0 AND mp.type = 2")

            connDb.addParam(paramFrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(paramTo, "@paramTo", MySqlDbType.DateTime)
            'connDb.addParam(outletNo, "@outletNo", MySqlDbType.VarChar)
            'connDb.addParam(MemberID, "@memberID", MySqlDbType.Int32)
            connDb.addParam(memberNo, "@memberNo", MySqlDbType.String)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)


            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:memberpoint")
            Err.Clear()
        End Try

        Return dsRslt
    End Function



    'Revisi 08 Juni
    Public Function comission(ByVal paramFrom As Date, ByVal paramTo As Date, ByVal outletNo As String, ByVal custID As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim dsRslt As New DataSet

        Try
            Dim connDb As New clsDbConn(ServerString)
            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then


                connDb.resetCmd("SELECT  sd.serveEmployee, sm.transDate, sm.Number, sm.olNumber, sm.olName, sd.itemName " & _
                                     ", sd.Qty, sd.komisi, (sd.qty * sd.komisi) AS ttlKomisi " & _
                                 "FROM cld_salesmaster sm INNER JOIN cld_salesdetail sd ON (sm.number = sd.smno AND sm.olId = sd.Outletid AND sm.CustID = sd.CustID) " & _
                                 "WHERE sm.flag=1 AND sd.flag= 1 AND sd.ServeEmployee <> '' AND sm.Busidate >= @paramFrom AND sm.BusiDate <= @paramTo " & _
                                     "AND (sm.CustID=@custID OR @custID=0) " & _
                                 "AND ((sm.olNumber=@outletNo AND @outletNo<>'') or (@outletNo='' AND (sm.olid IN (Select Store from cld_userxstore where user = '" & usr & "'))) ) ")
            Else
                connDb.resetCmd("SELECT  sd.serveEmployee, sm.transDate, sm.Number, sm.olNumber, sm.olName, sd.itemName " & _
                                    ", sd.Qty, sd.komisi, (sd.qty * sd.komisi) AS ttlKomisi " & _
                                "FROM cld_salesmaster sm INNER JOIN cld_salesdetail sd ON (sm.number = sd.smno AND sm.olId = sd.Outletid AND sm.CustID = sd.CustID) " & _
                                "WHERE sm.flag=1 AND sd.flag= 1 AND sd.ServeEmployee <> '' AND sm.Busidate >= @paramFrom AND sm.BusiDate <= @paramTo " & _
                                    "AND (sm.CustID=@custID OR @custID=0) " & _
                                "AND (sm.olNumber=@outletNo OR @outletNo='')")
            End If

            connDb.addParam(paramFrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(paramTo, "@paramTo", MySqlDbType.DateTime)
            connDb.addParam(outletNo, "@outletNo", MySqlDbType.String)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)

            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:comission")
            Err.Clear()
        End Try

        Return dsRslt

    End Function

    'Revisi 08 Juni
    Private Function voidBill(ByVal paramFrom As Date, ByVal paramTo As Date, ByVal Outletstr As String, ByVal iOwn As String, ByVal iBrand As String, ByVal iCity As String, ByVal custID As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim dsRslt As New DataSet
        Dim sOut As String = ""
        Dim sBrand As String = ""
        Dim sOwn As String = ""
        Dim sCity As String = ""
        Try
            Dim connDb As New clsDbConn(ServerString)

            If iBrand = "0" Or iBrand = "" Then
                sBrand = ""
            Else
                sBrand = " and o.brandID in (" & iBrand & ")"
            End If

            If iOwn = "0" Or iOwn = "" Then
                sOwn = ""
            Else
                sOwn = " and o.ownID = " & iOwn
            End If
            If iCity = "0" Or iCity = "" Then
                sCity = ""
            Else
                sCity = " and o.cityID in (" & iCity & ")"
            End If

            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If Outletstr = "" Then
                    sOut = " and sm.olId in (Select Store From cld_userxstore where user = '" & usr & "')"
                Else
                    sOut = " and sm.olnumber in (" & Outletstr & ")"
                End If
            Else
                If Outletstr = "" Then
                    sOut = ""
                Else
                    sOut = " and sm.olnumber in (" & Outletstr & ")"
                End If
            End If


            connDb.resetCmd("SELECT olNumber 'Store Code', olName 'Store Name',number 'Sales Number',DATE_FORMAT(transdate,'%d-%b-%Y %H:%i:%s') 'Transaction Date',total 'Gross Sales',memberNo 'Member Number',memberName 'Member Name',voidRmk 'Remark',VoidAth 'Auth' " &
                                "FROM cld_salesmaster sm " &
                                 "INNER JOIN cld_outlet o ON(o.id = sm.olid and o.Custid = sm.Custid) " &
                                "WHERE voidRmk NOT LIKE '%join%' AND sm.custID=@custID AND flag=2 and busiDate >=@paramFrom AND busiDate <=@paramTo " &
                                    "AND (sm.CustID=@custID OR @custID=0)" & sOut & sOwn & sBrand & sCity)

            connDb.addParam(paramFrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(paramTo, "@paramTo", MySqlDbType.DateTime)
            'connDb.addParam(outletNo, "@outletNo", MySqlDbType.String)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)

            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:comission")
            Err.Clear()
        End Try

        Return dsRslt

    End Function

    Private Function voidBill2018(ByVal paramFrom As Date, ByVal paramTo As Date, ByVal Outletstr As String, ByVal iOwn As String, ByVal iBrand As String, ByVal iCity As String, ByVal custID As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim dsRslt As New DataSet
        Dim sOut As String = ""
        Dim sBrand As String = ""
        Dim sOwn As String = ""
        Dim sCity As String = ""
        Try
            Dim connDb As New clsDbConn(ServerString)

            If iBrand = "0" Or iBrand = "" Then
                sBrand = ""
            Else
                sBrand = " and o.brandID in (" & iBrand & ")"
            End If

            If iOwn = "0" Or iOwn = "" Then
                sOwn = ""
            Else
                sOwn = " and o.ownID = " & iOwn
            End If
            If iCity = "0" Or iCity = "" Then
                sCity = ""
            Else
                sCity = " and o.cityID in (" & iCity & ")"
            End If

            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If Outletstr = "" Then
                    sOut = " and sm.olId in (Select Store From cld_userxstore where user = '" & usr & "')"
                Else
                    sOut = " and sm.olnumber in (" & Outletstr & ")"
                End If
            Else
                If Outletstr = "" Then
                    sOut = ""
                Else
                    sOut = " and sm.olnumber in (" & Outletstr & ")"
                End If
            End If


            connDb.resetCmd("SELECT olNumber 'Store Code', olName 'Store Name',number 'Sales Number',DATE_FORMAT(transdate,'%d-%b-%Y %H:%i:%s') 'Transaction Date',total 'Gross Sales',memberNo 'Member Number',memberName 'Member Name',voidRmk 'Remark',VoidAth 'Auth' " &
                                "FROM cld_salesmaster2018 sm " &
                                 "INNER JOIN cld_outlet o ON(o.id = sm.olid and o.Custid = sm.Custid) " &
                                "WHERE voidRmk NOT LIKE '%join%' AND sm.custID=@custID AND flag=2 and busiDate >=@paramFrom AND busiDate <=@paramTo " &
                                    "AND (sm.CustID=@custID OR @custID=0)" & sOut & sOwn & sBrand & sCity)

            connDb.addParam(paramFrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(paramTo, "@paramTo", MySqlDbType.DateTime)
            'connDb.addParam(outletNo, "@outletNo", MySqlDbType.String)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)

            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:comission")
            Err.Clear()
        End Try

        Return dsRslt

    End Function

    'Revisi 08 Juni
    Private Function voidItem(ByVal paramFrom As Date, ByVal paramTo As Date, ByVal Outletstr As String, ByVal iOwn As String, ByVal iBrand As String, ByVal iCity As String, ByVal custID As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim dsRslt As New DataSet
        Dim sOut As String = ""
        Dim sBrand As String = ""
        Dim sOwn As String = ""
        Dim sCity As String = ""
        Try
            Dim connDb As New clsDbConn(ServerString)

            If iBrand = "0" Or iBrand = "" Then
                sBrand = ""
            Else
                sBrand = " and o.brandID in (" & iBrand & ")"
            End If

            If iOwn = "0" Or iOwn = "" Then
                sOwn = ""
            Else
                sOwn = " and o.ownID = " & iOwn
            End If
            If iCity = "0" Or iCity = "" Then
                sCity = ""
            Else
                sCity = " and o.cityID in (" & iCity & ")"
            End If

            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If Outletstr = "" Then
                    sOut = " and sm.olId in (Select Store From cld_userxstore where user = '" & usr & "')"
                Else
                    sOut = " and sm.olnumber in (" & Outletstr & ")"
                End If
            Else
                If Outletstr = "" Then
                    sOut = ""
                Else
                    sOut = " and sm.olnumber in (" & Outletstr & ")"
                End If
            End If



            connDb.resetCmd("SELECT DATE_FORMAT(sm.BusiDate,'%d-%b-%Y') AS 'Sales Date', sm.olNumber 'Store Code' ,sm.olName 'Store Name' ,sm.number 'Sales Number' ,sm.transdate 'Transaction Date', sd.itemno 'Item Code' " &
                                    " ,sd.itemname 'Item Name' ,sd.qty 'Qty' ,sd.subtotal 'Net Sales' ,sm.memberNo 'Member Number' ,sm.memberName 'Member Name', sd.voidRmk 'Remark' ,sd.VoidAth 'Auth' " &
                                "FROM cld_salesmaster sm " &
                                 "INNER JOIN cld_outlet o ON(o.id = sm.olid and o.Custid = sm.Custid) " &
                                    "INNER JOIN cld_salesdetail sd ON (sm.number = sd.smno AND SM.olid = sd.Outletid and SM.Custid = sd.Custid) " &
                                "WHERE sm.custID=@custID AND sd.flag=2 AND sm.busidate >=@paramFrom AND sm.busidate <=@paramTo " &
                                    "AND (sm.CustID=@custID OR @custID=0)" & sOut & sOwn & sBrand & sCity & "ORDER BY sm.olNumber,sm.transdate")


            connDb.addParam(paramFrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(paramTo, "@paramTo", MySqlDbType.DateTime)
            'connDb.addParam(outletNo, "@outletNo", MySqlDbType.String)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)

            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:voidItem")
            Err.Clear()
        End Try

        Return dsRslt

    End Function

    Private Function Nonsales_notes(ByVal paramFrom As Date, ByVal paramTo As Date, ByVal Outletstr As String, ByVal iOwn As String, ByVal iBrand As String, ByVal iCity As String, ByVal custID As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim dsRslt As New DataSet
        Dim sOut As String = ""
        Dim sBrand As String = ""
        Dim sOwn As String = ""
        Dim sCity As String = ""
        Try
            Dim connDb As New clsDbConn(ServerString)

            If iBrand = "0" Or iBrand = "" Then
                sBrand = ""
            Else
                sBrand = " and o.brandID in (" & iBrand & ")"
            End If

            If iOwn = "0" Or iOwn = "" Then
                sOwn = ""
            Else
                sOwn = " and o.ownID = " & iOwn
            End If
            If iCity = "0" Or iCity = "" Then
                sCity = ""
            Else
                sCity = " and o.cityID in (" & iCity & ")"
            End If

            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If Outletstr = "" Then
                    sOut = " and sd.outletid in (Select Store From cld_userxstore where user = '" & usr & "')"
                Else
                    sOut = " and o.kode in (" & Outletstr & ")"
                End If
            Else
                If Outletstr = "" Then
                    sOut = ""
                Else
                    sOut = " and o.kode in (" & Outletstr & ")"
                End If
            End If



            connDb.resetCmd("SELECT DATE_FORMAT(p.BusiDate,'%d-%b-%Y') AS 'Sales Date', o.kode 'Store Code' ,o.nama 'Store Name' ,sd.smno 'Sales Number',
                            sd.itemno 'Item Code' ,sd.itemname 'Item Name' ,
                            sd.qty 'Qty', sd.subtotal AS 'Net Sales', p.refno 'Refno',p.notes 'Notes' 
                            FROM cld_salesdetail sd INNER JOIN cld_outlet o ON(o.id = sd.Outletid AND o.Custid = sd.Custid) 
                            INNER JOIN cld_payment p ON (sd.smno = p.smno AND Sd.outletid = p.Outletid AND Sd.Custid = p.Custid) " &
                           "WHERE sd.custID=@custID And sd.flag=3 And p.flag=3 And p.busidate >=@paramFrom And p.busidate <=@paramTo " &
                                    "And (sd.CustID=@custID Or @custID=0)" & sOut & sOwn & sBrand & sCity & " ORDER BY o.kode,p.busidate,p.smno")

            connDb.addParam(paramFrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(paramTo, "@paramTo", MySqlDbType.DateTime)
            'connDb.addParam(outletNo, "@outletNo", MySqlDbType.String)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)

            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:Nonsales_notes_gv")
            Err.Clear()
        End Try

        Return dsRslt

    End Function

    Private Function Nonsales_detail(ByVal paramFrom As Date, ByVal paramTo As Date, ByVal Outletstr As String, ByVal iOwn As String, ByVal iBrand As String, ByVal iCity As String, ByVal custID As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim dsRslt As New DataSet
        Dim sOut As String = ""
        Dim sBrand As String = ""
        Dim sOwn As String = ""
        Dim sCity As String = ""
        Try
            Dim connDb As New clsDbConn(ServerString)

            If iBrand = "0" Or iBrand = "" Then
                sBrand = ""
            Else
                sBrand = " and o.brandID in (" & iBrand & ")"
            End If

            If iOwn = "0" Or iOwn = "" Then
                sOwn = ""
            Else
                sOwn = " and o.ownID = " & iOwn
            End If
            If iCity = "0" Or iCity = "" Then
                sCity = ""
            Else
                sCity = " and o.cityID in (" & iCity & ")"
            End If

            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If Outletstr = "" Then
                    sOut = " and sm.olId in (Select Store From cld_userxstore where user = '" & usr & "')"
                Else
                    sOut = " and sm.olnumber in (" & Outletstr & ")"
                End If
            Else
                If Outletstr = "" Then
                    sOut = ""
                Else
                    sOut = " and sm.olnumber in (" & Outletstr & ")"
                End If
            End If



            connDb.resetCmd("SELECT DATE_FORMAT(sm.BusiDate,'%d-%b-%Y') AS 'Sales Date', sm.olNumber 'Store Code' ,sm.olName 'Store Name' ,sm.number 'Sales Number' ,sm.transdate 'Transaction Date', sd.itemno 'Item Code' " &
                                    " ,sd.itemname 'Item Name' ,sd.qty 'Qty' ,sd.subtotal 'Net Sales',sm.VoidAth 'Auth' " &
                                "FROM cld_salesmaster sm " &
                                 "INNER JOIN cld_outlet o ON(o.id = sm.olid and o.Custid = sm.Custid) " &
                                    "INNER JOIN cld_salesdetail sd ON (sm.number = sd.smno AND SM.olid = sd.Outletid and SM.Custid = sd.Custid) " &
                                "WHERE sm.custID=@custID AND sd.flag=3 and sm.flag=3 AND sm.busidate >=@paramFrom AND sm.busidate <=@paramTo " &
                                    "AND (sm.CustID=@custID OR @custID=0)" & sOut & sOwn & sBrand & sCity & " ORDER BY sm.olNumber,sm.transdate")


            connDb.addParam(paramFrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(paramTo, "@paramTo", MySqlDbType.DateTime)
            'connDb.addParam(outletNo, "@outletNo", MySqlDbType.String)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)

            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:Nonsales_detail")
            Err.Clear()
        End Try

        Return dsRslt

    End Function

    Private Function voidItem2018(ByVal paramFrom As Date, ByVal paramTo As Date, ByVal Outletstr As String, ByVal iOwn As String, ByVal iBrand As String, ByVal iCity As String, ByVal custID As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim dsRslt As New DataSet
        Dim sOut As String = ""
        Dim sBrand As String = ""
        Dim sOwn As String = ""
        Dim sCity As String = ""
        Try
            Dim connDb As New clsDbConn(ServerString)

            If iBrand = "0" Or iBrand = "" Then
                sBrand = ""
            Else
                sBrand = " and o.brandID in (" & iBrand & ")"
            End If

            If iOwn = "0" Or iOwn = "" Then
                sOwn = ""
            Else
                sOwn = " and o.ownID = " & iOwn
            End If
            If iCity = "0" Or iCity = "" Then
                sCity = ""
            Else
                sCity = " and o.cityID in (" & iCity & ")"
            End If

            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If Outletstr = "" Then
                    sOut = " and sm.olId in (Select Store From cld_userxstore where user = '" & usr & "')"
                Else
                    sOut = " and sm.olnumber in (" & Outletstr & ")"
                End If
            Else
                If Outletstr = "" Then
                    sOut = ""
                Else
                    sOut = " and sm.olnumber in (" & Outletstr & ")"
                End If
            End If



            connDb.resetCmd("SELECT DATE_FORMAT(sm.BusiDate,'%d-%b-%Y') AS 'Sales Date', sm.olNumber 'Store Code' ,sm.olName 'Store Name' ,sm.number 'Sales Number' ,sm.transdate 'Transaction Date', sd.itemno 'Item Code' " &
                                    " ,sd.itemname 'Item Name' ,sd.qty 'Qty' ,sd.subtotal 'Net Sales' ,sm.memberNo 'Member Number' ,sm.memberName 'Member Name', sd.voidRmk 'Remark' ,sd.VoidAth 'Auth' " &
                                "FROM cld_salesmaster2018 sm " &
                                 "INNER JOIN cld_outlet o ON(o.id = sm.olid and o.Custid = sm.Custid) " &
                                    "INNER JOIN cld_salesdetail2018 sd ON (sm.number = sd.smno AND SM.olid = sd.Outletid and SM.Custid = sd.Custid) " &
                                "WHERE sm.custID=@custID AND sd.flag=2 AND sm.busidate >=@paramFrom AND sm.busidate <=@paramTo " &
                                    "AND (sm.CustID=@custID OR @custID=0)" & sOut & sOwn & sBrand & sCity & "ORDER BY sm.olNumber,sm.transdate")

            connDb.addParam(paramFrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(paramTo, "@paramTo", MySqlDbType.DateTime)
            'connDb.addParam(outletNo, "@outletNo", MySqlDbType.String)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)

            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:comission")
            Err.Clear()
        End Try

        Return dsRslt

    End Function


    Private Function voidBill_gv(ByVal paramFrom As Date, ByVal paramTo As Date, ByVal Outletstr As String, ByVal iOwn As String, ByVal iBrand As String, ByVal iCity As String, ByVal custID As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim dsRslt As New DataSet
        Dim sOut As String = ""
        Dim sBrand As String = ""
        Dim sOwn As String = ""
        Dim sCity As String = ""
        Try
            Dim connDb As New clsDbConn(ServerString)

            If iBrand = "0" Or iBrand = "" Then
                sBrand = ""
            Else
                sBrand = " and o.brandID in (" & iBrand & ")"
            End If

            If iOwn = "0" Or iOwn = "" Then
                sOwn = ""
            Else
                sOwn = " and o.ownID = " & iOwn
            End If
            If iCity = "0" Or iCity = "" Then
                sCity = ""
            Else
                sCity = " and o.cityID in (" & iCity & ")"
            End If

            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If Outletstr = "" Then
                    sOut = " and sm.olId in (Select Store From cld_userxstore where user = '" & usr & "')"
                Else
                    sOut = " and sm.olnumber in (" & Outletstr & ")"
                End If
            Else
                If Outletstr = "" Then
                    sOut = ""
                Else
                    sOut = " and sm.olnumber in (" & Outletstr & ")"
                End If
            End If


            connDb.resetCmd("SELECT olNumber 'Store Code', olName 'Store Name',number 'Sales Number',DATE_FORMAT(transdate,'%d-%b-%Y %H:%i:%s') 'Transaction Date',FORMAT(total, 2,'de_DE') 'Gross Sales',memberNo 'Member Number',memberName 'Member Name',voidRmk 'Remark',VoidAth 'Auth' " &
                                "FROM cld_salesmaster sm " &
                                 "INNER JOIN cld_outlet o ON(o.id = sm.olid and o.Custid = sm.Custid) " &
                                "WHERE voidRmk NOT LIKE '%join%' AND sm.custID=@custID AND flag=2 and busiDate >=@paramFrom AND busiDate <=@paramTo " &
                                    "AND (sm.CustID=@custID OR @custID=0)" & sOut & sOwn & sBrand & sCity)

            connDb.addParam(paramFrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(paramTo, "@paramTo", MySqlDbType.DateTime)
            'connDb.addParam(outletNo, "@outletNo", MySqlDbType.String)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)

            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:voidBill_gv")
            Err.Clear()
        End Try

        Return dsRslt

    End Function

    Private Function voidBill2018_gv(ByVal paramFrom As Date, ByVal paramTo As Date, ByVal Outletstr As String, ByVal iOwn As String, ByVal iBrand As String, ByVal iCity As String, ByVal custID As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim dsRslt As New DataSet
        Dim sOut As String = ""
        Dim sBrand As String = ""
        Dim sOwn As String = ""
        Dim sCity As String = ""
        Try
            Dim connDb As New clsDbConn(ServerString)

            If iBrand = "0" Or iBrand = "" Then
                sBrand = ""
            Else
                sBrand = " and o.brandID in (" & iBrand & ")"
            End If

            If iOwn = "0" Or iOwn = "" Then
                sOwn = ""
            Else
                sOwn = " and o.ownID = " & iOwn
            End If
            If iCity = "0" Or iCity = "" Then
                sCity = ""
            Else
                sCity = " and o.cityID in (" & iCity & ")"
            End If

            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If Outletstr = "" Then
                    sOut = " and sm.olId in (Select Store From cld_userxstore where user = '" & usr & "')"
                Else
                    sOut = " and sm.olnumber in (" & Outletstr & ")"
                End If
            Else
                If Outletstr = "" Then
                    sOut = ""
                Else
                    sOut = " and sm.olnumber in (" & Outletstr & ")"
                End If
            End If


            connDb.resetCmd("SELECT olNumber 'Store Code', olName 'Store Name',number 'Sales Number',DATE_FORMAT(transdate,'%d-%b-%Y %H:%i:%s') 'Transaction Date',FORMAT(total, 2,'de_DE') 'Gross Sales',memberNo 'Member Number',memberName 'Member Name',voidRmk 'Remark',VoidAth 'Auth' " &
                                "FROM cld_salesmaster2018 sm " &
                                 "INNER JOIN cld_outlet o ON(o.id = sm.olid and o.Custid = sm.Custid) " &
                                "WHERE voidRmk NOT LIKE '%join%' AND sm.custID=@custID AND flag=2 and busiDate >=@paramFrom AND busiDate <=@paramTo " &
                                    "AND (sm.CustID=@custID OR @custID=0)" & sOut & sOwn & sBrand & sCity)

            connDb.addParam(paramFrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(paramTo, "@paramTo", MySqlDbType.DateTime)
            'connDb.addParam(outletNo, "@outletNo", MySqlDbType.String)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)

            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:voidBill_gv")
            Err.Clear()
        End Try

        Return dsRslt

    End Function

    'Revisi 08 Juni
    Private Function voidItem_gv(ByVal paramFrom As Date, ByVal paramTo As Date, ByVal Outletstr As String, ByVal iOwn As String, ByVal iBrand As String, ByVal iCity As String, ByVal custID As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim dsRslt As New DataSet
        Dim sOut As String = ""
        Dim sBrand As String = ""
        Dim sOwn As String = ""
        Dim sCity As String = ""
        Try
            Dim connDb As New clsDbConn(ServerString)

            If iBrand = "0" Or iBrand = "" Then
                sBrand = ""
            Else
                sBrand = " and o.brandID in (" & iBrand & ")"
            End If

            If iOwn = "0" Or iOwn = "" Then
                sOwn = ""
            Else
                sOwn = " and o.ownID = " & iOwn
            End If
            If iCity = "0" Or iCity = "" Then
                sCity = ""
            Else
                sCity = " and o.cityID in (" & iCity & ")"
            End If

            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If Outletstr = "" Then
                    sOut = " and sm.olId in (Select Store From cld_userxstore where user = '" & usr & "')"
                Else
                    sOut = " and sm.olnumber in (" & Outletstr & ")"
                End If
            Else
                If Outletstr = "" Then
                    sOut = ""
                Else
                    sOut = " and sm.olnumber in (" & Outletstr & ")"
                End If
            End If



            connDb.resetCmd("SELECT DATE_FORMAT(sm.BusiDate,'%d-%b-%Y') AS 'Sales Date', sm.olNumber 'Store Code' ,sm.olName 'Store Name' ,sm.number 'Sales Number' ,DATE_FORMAT(sm.transdate,'%d-%b-%Y %H:%i:%s') 'Transaction Date', sd.itemno 'Item Code' ,sd.itemname 'Item Name' ,FORMAT(sd.qty, 0,'de_DE') 'Qty' " &
                                    ",FORMAT(sd.subtotal, 2,'de_DE') as 'Net Sales',sm.memberNo 'Member Number' ,sm.memberName 'Member Name',sd.voidRmk 'Remark' ,sd.VoidAth 'Auth' " &
                                "FROM cld_salesmaster sm " &
                                 "INNER JOIN cld_outlet o ON(o.id = sm.olid and o.Custid = sm.Custid) " &
                                    "INNER JOIN cld_salesdetail sd ON (sm.number = sd.smno AND SM.olid = sd.Outletid and SM.Custid = sd.Custid) " &
                                "WHERE sm.custID=@custID AND sd.flag=2 AND sm.busidate >=@paramFrom AND sm.busidate <=@paramTo " &
                                    "AND (sm.CustID=@custID OR @custID=0)" & sOut & sOwn & sBrand & sCity & "ORDER BY sm.olNumber,sm.transdate")

            connDb.addParam(paramFrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(paramTo, "@paramTo", MySqlDbType.DateTime)
            'connDb.addParam(outletNo, "@outletNo", MySqlDbType.String)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)

            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:voidItem_gv")
            Err.Clear()
        End Try

        Return dsRslt

    End Function

    Private Function Nonsales_notes_gv(ByVal paramFrom As Date, ByVal paramTo As Date, ByVal Outletstr As String, ByVal iOwn As String, ByVal iBrand As String, ByVal iCity As String, ByVal custID As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim dsRslt As New DataSet
        Dim sOut As String = ""
        Dim sBrand As String = ""
        Dim sOwn As String = ""
        Dim sCity As String = ""
        Try
            Dim connDb As New clsDbConn(ServerString)

            If iBrand = "0" Or iBrand = "" Then
                sBrand = ""
            Else
                sBrand = " and o.brandID in (" & iBrand & ")"
            End If

            If iOwn = "0" Or iOwn = "" Then
                sOwn = ""
            Else
                sOwn = " and o.ownID = " & iOwn
            End If
            If iCity = "0" Or iCity = "" Then
                sCity = ""
            Else
                sCity = " and o.cityID in (" & iCity & ")"
            End If

            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If Outletstr = "" Then
                    sOut = " and sd.outletid in (Select Store From cld_userxstore where user = '" & usr & "')"
                Else
                    sOut = " and o.kode in (" & Outletstr & ")"
                End If
            Else
                If Outletstr = "" Then
                    sOut = ""
                Else
                    sOut = " and o.kode in (" & Outletstr & ")"
                End If
            End If



            connDb.resetCmd("SELECT DATE_FORMAT(p.BusiDate,'%d-%b-%Y') AS 'Sales Date', o.kode 'Store Code' ,o.nama 'Store Name' ,sd.smno 'Sales Number',
                            sd.itemno 'Item Code' ,sd.itemname 'Item Name' ,
                            FORMAT(sd.qty, 0,'de_DE') 'Qty' ,FORMAT(sd.subtotal, 2,'de_DE') AS 'Net Sales',p.refno 'Refno',p.notes 'Notes' 
                            FROM cld_salesdetail sd INNER JOIN cld_outlet o ON(o.id = sd.Outletid AND o.Custid = sd.Custid) 
                            INNER JOIN cld_payment p ON (sd.smno = p.smno AND Sd.outletid = p.Outletid AND Sd.Custid = p.Custid) " &
                           "WHERE sd.custID=@custID And sd.flag=3 And p.flag=3 And p.busidate >=@paramFrom And p.busidate <=@paramTo " &
                                    "And (sd.CustID=@custID Or @custID=0)" & sOut & sOwn & sBrand & sCity & " ORDER BY o.kode,p.busidate,p.smno")

            connDb.addParam(paramFrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(paramTo, "@paramTo", MySqlDbType.DateTime)
            'connDb.addParam(outletNo, "@outletNo", MySqlDbType.String)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)

            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:Nonsales_notes_gv")
            Err.Clear()
        End Try

        Return dsRslt

    End Function


    Private Function Nonsales_detail_gv(ByVal paramFrom As Date, ByVal paramTo As Date, ByVal Outletstr As String, ByVal iOwn As String, ByVal iBrand As String, ByVal iCity As String, ByVal custID As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim dsRslt As New DataSet
        Dim sOut As String = ""
        Dim sBrand As String = ""
        Dim sOwn As String = ""
        Dim sCity As String = ""
        Try
            Dim connDb As New clsDbConn(ServerString)

            If iBrand = "0" Or iBrand = "" Then
                sBrand = ""
            Else
                sBrand = " and o.brandID in (" & iBrand & ")"
            End If

            If iOwn = "0" Or iOwn = "" Then
                sOwn = ""
            Else
                sOwn = " and o.ownID = " & iOwn
            End If
            If iCity = "0" Or iCity = "" Then
                sCity = ""
            Else
                sCity = " and o.cityID in (" & iCity & ")"
            End If

            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If Outletstr = "" Then
                    sOut = " and sm.olId in (Select Store From cld_userxstore where user = '" & usr & "')"
                Else
                    sOut = " and sm.olnumber in (" & Outletstr & ")"
                End If
            Else
                If Outletstr = "" Then
                    sOut = ""
                Else
                    sOut = " and sm.olnumber in (" & Outletstr & ")"
                End If
            End If



            connDb.resetCmd("SELECT DATE_FORMAT(sm.BusiDate,'%d-%b-%Y') AS 'Sales Date', sm.olNumber 'Store Code' ,sm.olName 'Store Name' ,sm.number 'Sales Number' ,DATE_FORMAT(sm.transdate,'%d-%b-%Y %H:%i:%s') 'Transaction Date', sd.itemno 'Item Code' ,sd.itemname 'Item Name' ,FORMAT(sd.qty, 0,'de_DE') 'Qty' " &
                                    ",FORMAT(sd.subtotal, 2,'de_DE') as 'Net Sales',sm.VoidAth 'Auth' " &
                                "FROM cld_salesmaster sm " &
                                 "INNER JOIN cld_outlet o ON(o.id = sm.olid and o.Custid = sm.Custid) " &
                                    "INNER JOIN cld_salesdetail sd ON (sm.number = sd.smno AND SM.olid = sd.Outletid and SM.Custid = sd.Custid) " &
                                "WHERE sm.custID=@custID AND sd.flag=3 AND sm.flag=3 AND sm.busidate >=@paramFrom AND sm.busidate <=@paramTo " &
                                    "AND (sm.CustID=@custID OR @custID=0)" & sOut & sOwn & sBrand & sCity & " ORDER BY sm.olNumber,sm.transdate")

            connDb.addParam(paramFrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(paramTo, "@paramTo", MySqlDbType.DateTime)
            'connDb.addParam(outletNo, "@outletNo", MySqlDbType.String)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)

            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:Nonsales_detail_gv")
            Err.Clear()
        End Try

        Return dsRslt

    End Function
    Private Function voidItem2018_gv(ByVal paramFrom As Date, ByVal paramTo As Date, ByVal Outletstr As String, ByVal iOwn As String, ByVal iBrand As String, ByVal iCity As String, ByVal custID As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim dsRslt As New DataSet
        Dim sOut As String = ""
        Dim sBrand As String = ""
        Dim sOwn As String = ""
        Dim sCity As String = ""
        Try
            Dim connDb As New clsDbConn(ServerString)

            If iBrand = "0" Or iBrand = "" Then
                sBrand = ""
            Else
                sBrand = " and o.brandID in (" & iBrand & ")"
            End If

            If iOwn = "0" Or iOwn = "" Then
                sOwn = ""
            Else
                sOwn = " and o.ownID = " & iOwn
            End If
            If iCity = "0" Or iCity = "" Then
                sCity = ""
            Else
                sCity = " and o.cityID in (" & iCity & ")"
            End If

            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If Outletstr = "" Then
                    sOut = " and sm.olId in (Select Store From cld_userxstore where user = '" & usr & "')"
                Else
                    sOut = " and sm.olnumber in (" & Outletstr & ")"
                End If
            Else
                If Outletstr = "" Then
                    sOut = ""
                Else
                    sOut = " and sm.olnumber in (" & Outletstr & ")"
                End If
            End If



            connDb.resetCmd("SELECT DATE_FORMAT(sm.BusiDate,'%d-%b-%Y') AS 'Sales Date', sm.olNumber 'Store Code' ,sm.olName 'Store Name' ,sm.number 'Sales Number' ,DATE_FORMAT(sm.transdate,'%d-%b-%Y %H:%i:%s') 'Transaction Date', sd.itemno 'Item Code' ,sd.itemname 'Item Name' ,FORMAT(sd.qty, 0,'de_DE') 'Qty' " &
                                    ",FORMAT(sd.subtotal, 2,'de_DE') as 'Net Sales',sm.memberNo 'Member Number' ,sm.memberName 'Member Name',sd.voidRmk 'Remark' ,sd.VoidAth 'Auth' " &
                                "FROM cld_salesmaster2018 sm " &
                                 "INNER JOIN cld_outlet o ON(o.id = sm.olid and o.Custid = sm.Custid) " &
                                    "INNER JOIN cld_salesdetail2018 sd ON (sm.number = sd.smno AND SM.olid = sd.Outletid and SM.Custid = sd.Custid) " &
                                "WHERE sm.custID=@custID AND sd.flag=2 AND sm.busidate >=@paramFrom AND sm.busidate <=@paramTo " &
                                    "AND (sm.CustID=@custID OR @custID=0)" & sOut & sOwn & sBrand & sCity & "ORDER BY sm.olNumber,sm.transdate")

            connDb.addParam(paramFrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(paramTo, "@paramTo", MySqlDbType.DateTime)
            'connDb.addParam(outletNo, "@outletNo", MySqlDbType.String)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)

            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:voidItem_gv")
            Err.Clear()
        End Try

        Return dsRslt

    End Function

    'perubahan 27 feb 2017, ditambahkan modpiv dan user
    Private Function receiveReturn(ByVal paramFrom As Date, ByVal paramTo As Date, ByVal custID As Integer, ByVal usr As String, ByVal modpriv As Integer, ByVal ccid As Integer) As DataSet
        Dim dsRslt As New DataSet

        Try
            Dim connDb As New clsDbConn(ServerString)


            With connDb
                If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then

                    .resetCmd("SELECT rcv.*, trans.*, sup.name AS supname " & _
                            "FROM cld_invlk_rcv rcv INNER JOIN " & _
                                "cld_inv_trans trans ON rcv.id = trans.Linkid AND trans.custid = rcv.CustID INNER JOIN " & _
                                "cld_inv_supplier sup ON sup.ID = rcv.SupId AND sup.CustID = rcv.CustID Inner JOIN " & _
                                 "cld_inv_cc cc ON trans.cc = cc.id AND trans.CustID = cc.CustID " & _
                            "WHERE trans.type IN (3,4) " & _
                                "AND trans.flagtrans = 1 " & _
                                "AND trans.invdate >= @paramFrom AND trans.invdate <=@paramTo " & _
                                "AND trans.CustID=@custID " & _
                                "AND ((rcv.cc=@ccid) or cc.Olid In (Select Store From cld_userxstore where user = '" & usr & "'))")

                Else

                    .resetCmd("SELECT rcv.*, trans.*, sup.name AS supname " & _
                            "FROM cld_invlk_rcv rcv INNER JOIN " & _
                                "cld_inv_trans trans ON rcv.id = trans.Linkid AND trans.custid = rcv.CustID INNER JOIN " & _
                                "cld_inv_supplier sup ON sup.ID = rcv.SupId AND sup.CustID = rcv.CustID " & _
                            "WHERE trans.type IN (3,4) " & _
                                "AND trans.flagtrans = 1 " & _
                                "AND trans.invdate >= @paramFrom AND trans.invdate <=@paramTo " & _
                                "AND trans.CustID=@custID " & _
                                "AND ((rcv.cc=@ccid or @ccid=0)) ")

                End If



                .addParam(paramFrom, "@paramFrom", MySqlDbType.DateTime)
                .addParam(paramTo, "@paramTo", MySqlDbType.DateTime)
                .addParam(custID, "@custID", MySqlDbType.Int32)
                .addParam(ccid, "@ccid", MySqlDbType.Int32)

                dsRslt = .selectDataset

                .dispose()

            End With

            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReport:receiveReturn")
            Err.Clear()
        End Try

        Return dsRslt

    End Function

    'perubahan 27 feb 2017, ditambahkan modpiv dan user
    'transfer from
    Private Function invTransferTo(ByVal paramFrom As Date, ByVal paramTo As Date, ByVal custID As Integer, ByVal usr As String, ByVal modpriv As Integer, ByVal ccid As Integer) As DataSet
        Dim dsRslt As New DataSet

        Try
            Dim dbConn As New clsDbConn(ServerString)

            With dbConn
                If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then

                    .resetCmd("SELECT trf.*, cc.Name AS ccfName, trans.* " & _
                            "FROM cld_invlk_trf trf " & _
                                "INNER JOIN cld_inv_trans trans ON trf.id = trans.Linkid AND trans.custid = trf.CustID " & _
                                "INNER JOIN cld_inv_cc cc ON cc.ID = trans.cc and cc.custid = trans.custid " & _
                            "WHERE (trans.type = 1 And trans.flagtrans = 1) " & _
                                "AND trans.invdate >= @paramFrom AND trans.invdate <=@paramTo " & _
                                "AND trans.CustID=@custID " & _
                                "AND ((trans.cc=@ccid) or (cc.Olid In (Select Store From cld_userxstore where user = '" & usr & "')) )")


                Else

                    .resetCmd("SELECT trf.*, cc.Name AS ccfName, trans.* " & _
                                "FROM cld_invlk_trf trf " & _
                                    "INNER JOIN cld_inv_trans trans ON trf.id = trans.Linkid AND trans.custid = trf.CustID " & _
                                    "INNER JOIN cld_inv_cc cc ON cc.ID = trans.cc and cc.custid = trans.custid " & _
                                "WHERE (trans.type = 1 And trans.flagtrans = 1) " & _
                                    "AND trans.invdate >= @paramFrom AND trans.invdate <=@paramTo " & _
                                    "AND trans.CustID=@custID " & _
                                    "AND ((trans.cc=@ccid or @ccid=0)) ")

                End If



                .addParam(paramFrom, "@paramFrom", MySqlDbType.DateTime)
                .addParam(paramTo, "@paramTo", MySqlDbType.DateTime)
                .addParam(custID, "@custID", MySqlDbType.Int32)
                .addParam(ccid, "@ccid", MySqlDbType.Int32)

                dsRslt = .selectDataset

                .dispose()

            End With


            dbConn = Nothing

        Catch ex As Exception
            errLog(Err.Description, "Clsreports:invTransfer")
            Err.Clear()
        End Try

        Return dsRslt

    End Function


    'perubahan 27 feb 2017, ditambahkan modpiv dan user
    'transfer from
    Private Function invTransferFrom(ByVal paramFrom As Date, ByVal paramTo As Date, ByVal custID As Integer, ByVal usr As String, ByVal modpriv As Integer, ByVal ccid As Integer) As DataSet
        Dim dsRslt As New DataSet

        Try
            Dim dbConn As New clsDbConn(ServerString)

            With dbConn
                If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then

                    .resetCmd("SELECT trf.*, cc.Name AS ccfName, trans.* " & _
                            "FROM cld_invlk_trf trf " & _
                                "INNER JOIN cld_inv_trans trans ON trf.id = trans.Linkid AND trans.custid = trf.CustID " & _
                                "INNER JOIN cld_inv_cc cc ON cc.ID = trf.ccFrom and cc.custid = trf.custid " & _
                            "WHERE (trans.type = 1 And trans.flagtrans = 1) " & _
                                "AND trans.invdate >= @paramFrom AND trans.invdate <=@paramTo " & _
                                "AND trans.CustID=@custID " & _
                                "AND ((trf.ccFrom=@ccid) or (cc.Olid In (Select Store From cld_userxstore where user = '" & usr & "')) )")

                Else

                    .resetCmd("SELECT trf.*, cc.Name AS ccfName, trans.* " & _
                                "FROM cld_invlk_trf trf " & _
                                    "INNER JOIN cld_inv_trans trans ON trf.id = trans.Linkid AND trans.custid = trf.CustID " & _
                                    "INNER JOIN cld_inv_cc cc ON cc.ID = trf.ccFrom and cc.custid = trf.custid " & _
                                "WHERE (trans.type = 1 And trans.flagtrans = 1) " & _
                                    "AND trans.invdate >= @paramFrom AND trans.invdate <=@paramTo " & _
                                    "AND trans.CustID=@custID " & _
                                    "AND trf.ccFrom = @ccid or @ccid=0")

                End If

                .addParam(paramFrom, "@paramFrom", MySqlDbType.DateTime)
                .addParam(paramTo, "@paramTo", MySqlDbType.DateTime)
                .addParam(custID, "@custID", MySqlDbType.Int32)
                .addParam(ccid, "@ccid", MySqlDbType.Int32)

                dsRslt = .selectDataset

                .dispose()

            End With


            dbConn = Nothing

        Catch ex As Exception
            errLog(Err.Description, "Clsreports:invTransfer")
            Err.Clear()
        End Try

        Return dsRslt

    End Function


    'perubahan 27 feb 2017, ditambahkan modpiv dan user
    Private Function stkOnHand(ByVal custID As Integer, ByVal usr As String, ByVal modpriv As Integer, ByVal ccid As Integer) As DataSet
        Dim dsRslt As New DataSet
        Try
            Dim dbConn As New clsDbConn(ServerString)

            With dbConn
                If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then

                    .resetCmd("SELECT cld_inv_transstock.* " & _
                                   ", cld_inv_stkgroup.Code AS GrpCode " & _
                                   ", cld_inv_stkgroup.Name AS GrpNm " & _
                                   ",`cld_inv_stk`.Unit2 " & _
                           "FROM cld_inv_stk " & _
                                   "INNER JOIN cld_inv_stkgroup " & _
                                   "ON (cld_inv_stk.CustID = cld_inv_stkgroup.CustID) AND (cld_inv_stk.grpid = cld_inv_stkgroup.ID) " & _
                                   "INNER JOIN cld_inv_transstock " & _
                                   "ON (cld_inv_transstock.custId = cld_inv_stk.CustID) AND (cld_inv_transstock.stkId = cld_inv_stk.ID) " & _
                                   "INNER JOIN cld_inv_cc " & _
                                   "ON (cld_inv_transstock.custId = cld_inv_cc.CustID) AND (cld_inv_transstock.ccid = cld_inv_cc.ID) " & _
                           "WHERE cld_inv_transstock.custId=@custID " & _
                           "and ((cld_inv_transstock.ccid=@ccid) or ((cld_inv_cc.olid In (Select Store From cld_userxstore where user = '" & usr & "'))) )")

                Else

                    .resetCmd("SELECT cld_inv_transstock.* " & _
                                    ", cld_inv_stkgroup.Code AS GrpCode " & _
                                    ", cld_inv_stkgroup.Name AS GrpNm " & _
                                     ",`cld_inv_stk`.Unit2 " & _
                            "FROM cld_inv_stk " & _
                                    "INNER JOIN cld_inv_stkgroup " & _
                                    "ON (cld_inv_stk.CustID = cld_inv_stkgroup.CustID) AND (cld_inv_stk.grpid = cld_inv_stkgroup.ID) " & _
                                    "INNER JOIN cld_inv_transstock " & _
                                    "ON (cld_inv_transstock.custId = cld_inv_stk.CustID) AND (cld_inv_transstock.stkId = cld_inv_stk.ID) " & _
                            "WHERE cld_inv_transstock.custId=@custID " & _
                                    "AND ((cld_inv_transstock.ccid=@ccid or @ccid=0)) ")

                End If


                .addParam(custID, "@custID", MySqlDbType.Int32)
                .addParam(ccid, "@ccid", MySqlDbType.Int32)

                dsRslt = .selectDataset

                .dispose()

            End With

            dbConn = Nothing
        Catch ex As Exception
            errLog(Err.Description, "ClsReports:stkOnHand")
            Err.Clear()
        End Try

        Return dsRslt

    End Function

    'perubahan 27 feb 2017, ditambahkan modpiv dan user
    Private Function invStockOpname(ByVal paramFrom As Date, ByVal paramTo As Date, ByVal custID As Integer, ByVal usr As String, ByVal modpriv As Integer, ByVal ccid As Integer) As DataSet
        Dim dsRslt As New DataSet

        Try
            Dim dbConn As New clsDbConn(ServerString)

            With dbConn
                If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then

                    .resetCmd("SELECT transtk.* " & _
                                     ", transtk.openqty + transtk.rcvqty - transtk.rtnqty + transtk.tinqty - transtk.toutQty - transtk.rwasteqty " & _
                                        "- transtk.fwasteQty - transtk.usageqty AS ttlQty " & _
                                     ", transtk.openqty + transtk.rcvqty - transtk.rtnqty + transtk.tinqty - transtk.toutQty " & _
                                        "- transtk.rwasteqty - transtk.fwasteQty - transtk.usageqty - transtk.closeqty  AS varQty " & _
                                     ", grp.code AS grpcode,  grp.Name AS grpname " & _
                                 "FROM v_inv_transstock transtk " & _
                                     "INNER JOIN cld_inv_stk stk ON stk.ID = transtk.stk_Id AND stk.CustID = transtk.custId " & _
                                     "INNER JOIN cld_inv_stkgroup grp ON grp.ID = stk.grpid AND grp.CustID = stk.CustID " & _
                                     "INNER JOIN cld_inv_cc cc ON cc.ID = transtk.cc AND cc.CustID = transtk.CustID " & _
                                 "WHERE transtk.bclose = 1 " & _
                                     "AND transtk.InvDate >=@paramFrom AND transtk.InvDate <= @paramTo " & _
                                     "AND transtk.custId=@custID AND ((transtk.cc=@ccid) or (cc.olid In (Select Store From cld_userxstore where user = '" & usr & "')) )")


                Else

                    .resetCmd("SELECT transtk.* " & _
                                    ", transtk.openqty + transtk.rcvqty - transtk.rtnqty + transtk.tinqty - transtk.toutQty - transtk.rwasteqty " & _
                                       "- transtk.fwasteQty - transtk.usageqty AS ttlQty " & _
                                    ", transtk.openqty + transtk.rcvqty - transtk.rtnqty + transtk.tinqty - transtk.toutQty " & _
                                       "- transtk.rwasteqty - transtk.fwasteQty - transtk.usageqty - transtk.closeqty  AS varQty " & _
                                    ", grp.code AS grpcode,  grp.Name AS grpname " & _
                                "FROM v_inv_transstock transtk " & _
                                    "INNER JOIN cld_inv_stk stk ON stk.ID = transtk.stk_Id AND stk.CustID = transtk.custId " & _
                                    "INNER JOIN cld_inv_stkgroup grp ON grp.ID = stk.grpid AND grp.CustID = stk.CustID " & _
                                "WHERE transtk.bclose = 1 " & _
                                    "AND transtk.InvDate >=@paramFrom AND transtk.InvDate <= @paramTo " & _
                                    "AND transtk.custId=@custID " & _
                                    "AND ((transtk.cc=@ccid or @ccid=0)) ")

                End If


                .addParam(paramFrom, "@paramFrom", MySqlDbType.DateTime)
                .addParam(paramTo, "@paramTo", MySqlDbType.DateTime)
                .addParam(custID, "@custID", MySqlDbType.Int32)
                .addParam(ccid, "@ccid", MySqlDbType.Int32)


                dsRslt = .selectDataset

                .dispose()

            End With

            dbConn = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:invStockOpname")
            Err.Clear()
        End Try

        Return dsRslt

    End Function

    Private Function otherExpense(ByVal paramFrom As Date, ByVal paramTo As Date, ByVal custID As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim dsRslt As New DataSet

        Try
            Dim dbConn As New clsDbConn(ServerString)

            With dbConn
                If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then


                    .resetCmd("SELECT c.nama AS custName, ol.nama AS outlet, o.* " & _
                            "FROM cld_expother o INNER JOIN " & _
                                    "cld_cust c ON (o.custID=c.ID) INNER JOIN " & _
                                    "cld_outlet ol ON (o.olid = ol.id) " & _
                            "where o.BusiDate >=@paramFrom and o.Olid In (Select Store From cld_userxstore where user = '" & usr & "') AND o.BusiDate <=@paramTo " & _
                                    "AND o.CustID=@custID")

                Else


                    .resetCmd("SELECT c.nama AS custName, ol.nama AS outlet, o.* " & _
                            "FROM cld_expother o INNER JOIN " & _
                                    "cld_cust c ON (o.custID=c.ID) INNER JOIN " & _
                                    "cld_outlet ol ON (o.olid = ol.id) " & _
                            "where o.BusiDate >=@paramFrom AND o.BusiDate <=@paramTo " & _
                                    "AND o.CustID=@custID")


                End If


                .addParam(paramFrom, "@paramFrom", MySqlDbType.DateTime)
                .addParam(paramTo, "@paramTo", MySqlDbType.DateTime)
                .addParam(custID, "@custID", MySqlDbType.Int32)

                dsRslt = .selectDataset

                .dispose()

            End With

            dbConn = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:otherExpense")
            Err.Clear()
        End Try

        Return dsRslt

    End Function

    Private Function movementCurrent(ByVal custID As Integer, ByVal usr As String, ByVal modpriv As Integer, ByVal ccid As Integer) As DataSet
        Dim dsRslt As New DataSet

        Try
            Dim dbConn As New clsDbConn(ServerString)

            With dbConn
                If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then

                    .resetCmd("SELECT transtk.* , grp.code AS grpcode,  grp.Name AS grpname " & _
                                    "FROM cld_inv_transstock transtk " & _
                                        "INNER JOIN cld_inv_stk stk ON stk.ID = transtk.stkId AND stk.CustID = transtk.custId " & _
                                        "INNER JOIN cld_inv_stkgroup grp ON grp.ID = stk.grpid AND grp.CustID = stk.CustID " & _
                                         "INNER JOIN cld_inv_cc ON cld_inv_cc.ID = transtk.ccid AND cld_inv_cc.CustID = transtk.CustID " & _
                                    "WHERE stk.CustID = @custID " & _
                                    "and ((transtk.ccid=@ccid) or ((cld_inv_cc.olid In (Select Store From cld_userxstore where user = '" & usr & "'))) )")


                Else

                    .resetCmd("SELECT transtk.* , grp.code AS grpcode,  grp.Name AS grpname " & _
                                    "FROM cld_inv_transstock transtk " & _
                                        "INNER JOIN cld_inv_stk stk ON stk.ID = transtk.stkId AND stk.CustID = transtk.custId " & _
                                        "INNER JOIN cld_inv_stkgroup grp ON grp.ID = stk.grpid AND grp.CustID = stk.CustID " & _
                                    "WHERE stk.CustID=@custID " & _
                                    "AND ((transtk.ccid=@ccid or @ccid=0)) ")


                End If


                .addParam(custID, "@custID", MySqlDbType.Int32)
                .addParam(ccid, "@ccid", MySqlDbType.Int32)

                dsRslt = .selectDataset

                .dispose()

            End With


            dbConn = Nothing
        Catch ex As Exception
            errLog(Err.Description, "ClsReports:movementCurrent")
            Err.Clear()
        End Try

        Return dsRslt

    End Function

    'Tambah Bram 30 Maret 2017 poor review
    Public Function PoorReviews(ByVal paramFrom As Date, ByVal paramTo As Date, ByVal outletNo As String, ByVal custID As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim dsRslt As New DataSet

        Try
            Dim connDb As New clsDbConn(ServerString)
            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                connDb.resetCmd("SELECT f.TransDate, f.MemberNo, f.MemberName,m.Email,f.MemberType,f.OlNumber, f.OlName,d.TypeID, d.TypeName, d.val,f.Remarks, m.mobileno " & _
                                    "FROM cld_feedbacktransdtl d INNER JOIN cld_feedbacktrans f ON d.Custid = f.CustID AND d.OutletId = f.OutletId AND d.Salesmasterid = f.Salesmasterid LEFT JOIN cld_member m ON d.Custid = m.CustID AND f.MemberNo = m.Number " & _
                               "where d.val < 3 and f.TransDate >=@paramFrom AND f.TransDate <=@paramTo " & _
                                    "AND ((f.olNumber=@outletNo AND @outletNo<>'') or (@outletNo='' AND (f.outletid IN (Select Store from cld_userxstore where user = '" & usr & "'))) ) " & _
                                    "AND (d.CustID =@custID OR @custID=0) ORDER BY d.typename ")
            Else
                connDb.resetCmd("SELECT f.TransDate, f.MemberNo, f.MemberName,m.Email,f.MemberType,f.OlNumber, f.OlName,d.TypeID, d.TypeName, d.val,f.Remarks, m.mobileno " & _
                                 "FROM cld_feedbacktransdtl d INNER JOIN cld_feedbacktrans f ON d.Custid = f.CustID AND d.OutletId = f.OutletId AND d.Salesmasterid = f.Salesmasterid LEFT JOIN cld_member m ON d.Custid = m.CustID AND f.MemberNo = m.Number " & _
                              "where d.val < 3 and f.TransDate >=@paramFrom AND f.TransDate <=@paramTo AND (f.olNumber =@outletNo or @outletNo = '') " & _
                                   "AND (d.CustID =@custID OR @custID=0) ORDER BY d.typename ")
            End If
            connDb.addParam(paramFrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(paramTo, "@paramTo", MySqlDbType.DateTime)
            connDb.addParam(outletNo, "@outletNo", MySqlDbType.VarChar)
            'connDb.addParam(MemberID, "@memberID", MySqlDbType.Int32)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)


            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:memberDeposit1")
            Err.Clear()
        End Try

        Return dsRslt
    End Function

    'Tambah Bram 30 Maret 2017 goodreviews
    Public Function GoodReviews(ByVal paramFrom As Date, ByVal paramTo As Date, ByVal outletNo As String, ByVal custID As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim dsRslt As New DataSet

        Try
            Dim connDb As New clsDbConn(ServerString)
            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                connDb.resetCmd("SELECT f.TransDate, f.MemberNo, f.MemberName,m.Email,f.MemberType,f.OlNumber, f.OlName,d.TypeID, d.TypeName, d.val,f.Remarks, m.mobileno " & _
                                    "FROM cld_feedbacktransdtl d INNER JOIN cld_feedbacktrans f ON d.Custid = f.CustID AND d.OutletId = f.OutletId AND d.Salesmasterid = f.Salesmasterid LEFT JOIN cld_member m ON d.Custid = m.CustID AND f.MemberNo = m.Number " & _
                               "where d.val >= 3 and f.TransDate >=@paramFrom AND f.TransDate <=@paramTo " & _
                                    "AND (f.(olNumber=@outletNo AND @outletNo<>'') or (@outletNo='' AND (f.outletid IN (Select Store from cld_userxstore where user = '" & usr & "'))) ) " & _
                                    "AND (d.CustID =@custID OR @custID=0) ORDER BY d.typename ")
            Else
                connDb.resetCmd("SELECT f.TransDate, f.MemberNo, f.MemberName,m.Email,f.MemberType,f.OlNumber, f.OlName,d.TypeID, d.TypeName, d.val,f.Remarks, m.mobileno " & _
                                 "FROM cld_feedbacktransdtl d INNER JOIN cld_feedbacktrans f ON d.Custid = f.CustID AND d.OutletId = f.OutletId AND d.Salesmasterid = f.Salesmasterid LEFT JOIN cld_member m ON d.Custid = m.CustID AND f.MemberNo = m.Number " & _
                              "where d.val >= 3 and f.TransDate >=@paramFrom AND f.TransDate <=@paramTo AND (f.olNumber =@outletNo or @outletNo = '') " & _
                                   "AND (d.CustID =@custID OR @custID=0) ORDER BY d.typename ")
            End If
            connDb.addParam(paramFrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(paramTo, "@paramTo", MySqlDbType.DateTime)
            connDb.addParam(outletNo, "@outletNo", MySqlDbType.VarChar)
            'connDb.addParam(MemberID, "@memberID", MySqlDbType.Int32)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)


            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:memberDeposit1")
            Err.Clear()
        End Try

        Return dsRslt
    End Function


    Private Function menuItem(ByVal custID As Integer) As DataSet
        Dim dsRslt As New DataSet

        Try
            Dim connDb As New clsDbConn(ServerString)

            With connDb
                .resetCmd("SELECT Itemno, ItemName, Custid, Price, cost, price - cost AS profit " & _
                          "FROM cld_itemmaster " & _
                          "WHERE custID=@custID " & _
                          "ORDER BY profit Desc")

                .addParam(custID, "@custID", MySqlDbType.Int32)

                dsRslt = .selectDataset

                .dispose()
            End With

            connDb = Nothing
        Catch ex As Exception
            errLog(Err.Description, "clsReports:menuItem")
            Err.Clear()
        End Try

        Return dsRslt

    End Function

    Private Function revenueVsCost(ByVal paramFrom As Date, ByVal paramTo As Date, ByVal custID As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim dsRslt As New DataSet

        Try
            Dim dbConn As New clsDbConn(ServerString)

            With dbConn
                If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then


                    .resetCmd("SELECT  sm.Busidate,  sm.olNumber,sm.olName, SUM(sm.Subtotal - sm.Discount) AS revenue " & _
                                    ",SUM(IFNULL((SELECT SUM((sd.qty * sd.Cost)) AS `data1` FROM cld_salesdetail sd WHERE (sd.flag =1) AND (sd.CustID = sm.CustID) AND (sd.smNO = sm.Number) AND (sd.Outletid = sm.olId ) ),0)) AS `Cost` " & _
                                    ",IFNULL((SELECT SUM((oth.Subtotal)) AS `data1` FROM cld_expother oth WHERE (oth.custid = sm.CustID) AND (oth.olId = sm.olId ) AND (oth.BusiDate = sm.BusiDate )  ),0) AS `other expens` " & _
                              "FROM cld_salesmaster AS sm " & _
                              "WHERE (sm.Flag = 1) " & _
                                    "and (sm.olid In (Select Store From cld_userxstore where user = '" & usr & "')) " & _
                                    "and (sm.busiDate >=@paramFrom AND sm.busiDate <=@paramTo) " & _
                                    "and (sm.CustID =@custID) " & _
                              "GROUP BY sm.Busidate")

                Else

                    .resetCmd("SELECT  sm.Busidate,  sm.olNumber,sm.olName, SUM(sm.Subtotal - sm.Discount) AS revenue " & _
                                    ",SUM(IFNULL((SELECT SUM((sd.qty * sd.Cost)) AS `data1` FROM cld_salesdetail sd WHERE (sd.flag =1) AND (sd.CustID = sm.CustID) AND (sd.smNO = sm.Number) AND (sd.Outletid = sm.olId ) ),0)) AS `Cost` " & _
                                    ",IFNULL((SELECT SUM((oth.Subtotal)) AS `data1` FROM cld_expother oth WHERE (oth.custid = sm.CustID) AND (oth.olId = sm.olId ) AND (oth.BusiDate = sm.BusiDate )  ),0) AS `other expens` " & _
                              "FROM cld_salesmaster AS sm " & _
                              "WHERE (sm.Flag = 1) " & _
                                    "and (sm.busiDate >=@paramFrom AND sm.busiDate <=@paramTo) " & _
                                    "and (sm.CustID =@custID) " & _
                              "GROUP BY sm.Busidate")

                End If

                .addParam(paramFrom, "@paramFrom", MySqlDbType.DateTime)
                .addParam(paramTo, "@paramTo", MySqlDbType.DateTime)
                .addParam(custID, "@custID", MySqlDbType.Int32)

                dsRslt = .selectDataset

                .dispose()
            End With


            dbConn = Nothing
        Catch ex As Exception
            errLog(Err.Description, "ClsReports:revenueVsCost")
            Err.Clear()
        End Try

        Return dsRslt

    End Function



    'Tambahan Bram order request 20 sept 2017 
    Public Function orderReq(ByVal order_req As String, ByVal custID As Integer, ByVal ccID As Integer) As DataSet
        Dim dsRslt As New DataSet

        Try

            Dim connDb As New clsDbConn(ServerString)

            connDb.resetCmd("SELECT a.id,a.`CreatedBy`,a.`CreatedDate`,a.`exDelDate`, a.`ccFromNm`, a.`ccToNm`, c.`Name`, c.`Number`, b.`U1`,c.`Unit1`, b.`u1Del`, b.`u1Rcv`, SUM(b.`U1` - b.`u1Del`) AS deldif, c.`Unit2`, SUM(b.`u1Del` - b.`u1Rcv`) AS rcvdif, c.`Unit3` " & _
           "FROM cld_inv_w a INNER JOIN cld_inv_w_dtl b ON a.`ID` = b.`wID` and a.custid = b.custid " & _
           "INNER JOIN cld_inv_stk c ON b.`stk_id` = c.`ID` and b.custid = c.custid " & _
           "WHERE a.id =@paramOrderId AND a.ccTO=@ccID  AND a.`custid` =@custID " & _
           "GROUP BY c.`Number`, a.id ORDER BY a.`CreatedDate` ")

            connDb.addParam(order_req, "@paramOrderId", MySqlDbType.VarChar)
            connDb.addParam(ccID, "@ccID", MySqlDbType.Int32)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)

            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:order_req")
            Err.Clear()
        End Try

        Return dsRslt
    End Function


    Public Function send_req_order(ByVal delivno As String, ByVal custID As Integer, ByVal ccID As Integer) As DataSet
        Dim dsRslt As New DataSet

        Try

            Dim connDb As New clsDbConn(ServerString)

            connDb.resetCmd("SELECT a.id, a.`DelID`,a.`CreatedBy`,a.`CreatedDate`,a.`exDelDate`,a.`DelDate`, a.`ccFromNm`, a.`ccToNm`,  b.`U1`,  b.`u1Del`, b.`U1` - b.`u1Del` AS deldif, b.U1_nm, c.number, b.stk_Name " & _
                            "FROM cld_inv_w a INNER JOIN cld_inv_w_dtl b " & _
                            "ON a.ID = b.wID AND a.custid = b.custid " & _
                            "INNER JOIN cld_inv_stk C " & _
                            "ON C.ID = b.stk_ID AND c.custid = b.custid " & _
                            "WHERE a.DelID =@paramOrderId AND a.ccTO=@ccID  AND a.`custid` =@custID " & _
                            "ORDER BY a.CreatedDate ")

            connDb.addParam(delivno, "@paramOrderId", MySqlDbType.VarChar)
            connDb.addParam(ccID, "@ccID", MySqlDbType.Int32)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)


            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:send_req_order")
            Err.Clear()
        End Try

        Return dsRslt
    End Function


    Public Function stk_req_rpt(ByVal paramFrom As Date, ByVal paramTo As Date, ByVal custID As Integer, ByVal outletNO As String, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim dsRslt As New DataSet

        Try

            Dim connDb As New clsDbConn(ServerString)
            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                connDb.resetCmd("SELECT a.id, a.`DelID`,a.`CreatedBy`,a.`CreatedDate`,a.`exDelDate`,a.`DelDate`, a.rcvDate, a.`ccFromNm`, a.`ccToNm`, d.`olId`, b.`stk_id` AS stkid, b.`stk_name` AS NAME, c.`Number`, b.`U1`,c.`Unit1`, b.`u1Del`, b.`u1Rcv`, (b.`U1` - b.`u1Del`) AS deldif, (b.`u1Del` - b.`u1Rcv`) AS rcvdif,  b.cost " & _
                "FROM cld_inv_w a INNER JOIN cld_inv_w_dtl b ON a.`ID` = b.`wID` and a.`custid` = b.`custid` INNER JOIN cld_inv_stk c ON b.`stk_id` = c.`ID` and b.`custid` = c.`custid` INNER JOIN cld_inv_cc d ON a.`ccTo` = d.`ID` and a.`custid` = d.`custid` INNER JOIN cld_outlet o ON d.`olId` = o.`ID` and d.`custid` = o.`custid` " & _
                "WHERE a.CreatedDate >=@paramFrom AND a.CreatedDate <=@paramTo AND (o.Kode=@outletNo or o.ID in (Select Store From cld_userxstore where user = '" & usr & "')) AND (a.`custid` =@custID OR @custID=0) " & _
                "GROUP BY c.`Number`, a.id ORDER BY a.`CreatedDate` ")

            Else

                connDb.resetCmd("SELECT a.id, a.`DelID`,a.`CreatedBy`,a.`CreatedDate`,a.`exDelDate`,a.`DelDate`, a.rcvDate, a.`ccFromNm`, a.`ccToNm`, d.`olId`, b.`stk_id` AS stkid, b.`stk_name` AS NAME, c.`Number`, b.`U1`,c.`Unit1`, b.`u1Del`, b.`u1Rcv`, (b.`U1` - b.`u1Del`) AS deldif, (b.`u1Del` - b.`u1Rcv`) AS rcvdif,  b.cost " & _
                "FROM cld_inv_w a INNER JOIN cld_inv_w_dtl b ON a.`ID` = b.`wID` and a.`custid` = b.`custid` INNER JOIN cld_inv_stk c ON b.`stk_id` = c.`ID` and b.`custid` = c.`custid` INNER JOIN cld_inv_cc d ON a.`ccTo` = d.`ID` and a.`custid` = d.`custid` INNER JOIN cld_outlet o ON d.`olId` = o.`ID` and d.`custid` = o.`custid` " & _
                "WHERE a.CreatedDate >= @paramFrom AND a.CreatedDate <= @paramTo AND (o.Kode=@outletNo or @outletNo = '') AND (a.`custid` =@custID OR @custID=0) " & _
                "GROUP BY c.`Number`, a.id ORDER BY a.`CreatedDate` ")
            End If
            connDb.addParam(paramFrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(paramTo, "@paramTo", MySqlDbType.DateTime)
            connDb.addParam(outletNO, "@outletNo", MySqlDbType.VarChar)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)

            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:orderwarehouse")
            Err.Clear()
        End Try

        Return dsRslt
    End Function

    'report tambahan bram 19 oct 2017
    'bram 17 Oct 2017
    Public Function detailproductionRpt(ByVal paramFrom As Date, ByVal paramTo As Date, ByVal ccid As Integer, ByVal custID As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim dsRslt As New DataSet

        Try

            Dim connDb As New clsDbConn(ServerString)
            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                connDb.resetCmd("SELECT trans.ID, trans.`InvDate`, trans.`Cc_name`, trans.`CreatedBy`, trans.`Stk_no` AS stk_no_trans, trans.`Stk_Name` AS stk_nm_trans, trans.`ttlStock` AS ttl_stk_trans, trans.`U1_nm` AS u1_nm_trans " & _
                                ", usg.`Stk_no`, usg.`Stk_Name`, usg.`U2`, usg.`U2_nm`, usg.`Factor2`, usg.`U3`, usg.`U3_nm`, usg.`ttlStock` " & _
                                "FROM`cld_inv_trans` trans LEFT JOIN `cld_inv_transusage` usg ON trans.`ID`=usg.`Linkid` and trans.`Custid`= usg.`Custid` " & _
                                "WHERE (trans.`flagtrans` = 1 AND trans.`type`=9) " & _
                                "AND trans.`InvDate` >=@paramFrom AND trans.`InvDate` <=@paramTo " & _
                                "AND (trans.`custid` =@custID OR @custID=0) " & _
                                "AND ((trans.Cc=@ccid) or (cc.Olid in (Select Store From cld_userxstore where user = '" & usr & "'))) " & _
                                "ORDER BY trans.`InvDate` ")

            Else

                connDb.resetCmd("SELECT trans.ID, trans.`InvDate`, trans.`Cc_name`, trans.`CreatedBy`, trans.`Stk_no` AS stk_no_trans, trans.`Stk_Name` AS stk_nm_trans, trans.`ttlStock` AS ttl_stk_trans, trans.`U1_nm` AS u1_nm_trans " & _
                                ", usg.`Stk_no`, usg.`Stk_Name`, usg.`U2`, usg.`U2_nm`, usg.`Factor2`, usg.`U3`, usg.`U3_nm`, usg.`ttlStock` " & _
                                "FROM`cld_inv_trans` trans LEFT JOIN `cld_inv_transusage` usg ON trans.`ID`= usg.`Linkid` and trans.`Custid`= usg.`Custid` " & _
                                "WHERE (trans.`flagtrans` = 1 AND trans.`type`=9) " & _
                                "AND trans.`InvDate` >=@paramFrom AND trans.`InvDate` <=@paramTo " & _
                                "AND (trans.`custid` =@custID OR @custID=0) " & _
                                "AND (trans.Cc=@ccid or @ccid=0) " & _
                                "ORDER BY trans.`InvDate` ")
            End If
            connDb.addParam(paramFrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(paramTo, "@paramTo", MySqlDbType.DateTime)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)
            connDb.addParam(ccid, "@ccid", MySqlDbType.Int32)



            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:productionRpt")
            Err.Clear()
        End Try

        Return dsRslt
    End Function

    'bram 17 Oct 2017
    Public Function productionRpt(ByVal paramFrom As Date, ByVal paramTo As Date, ByVal ccid As Integer, ByVal custID As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim dsRslt As New DataSet

        Try

            Dim connDb As New clsDbConn(ServerString)
            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                connDb.resetCmd("SELECT trans.`Cc_name`, trans.`CreatedBy`, trans.`Stk_no`, trans.`Stk_Name` " & _
                                ", SUM(trans.`U1`), trans.`U1_nm`, trans.`Factor1`, SUM(trans.u2), trans.`U2_nm`, SUM(trans.`ttlStock`) " & _
                                "FROM`cld_inv_trans` trans " & _
                                "WHERE (trans.`flagtrans` = 1 AND trans.`type`=9) " & _
                                "AND trans.`InvDate` >=@paramFrom AND trans.`InvDate` <=@paramTo " & _
                                "AND (trans.`custid` =@custID OR @custID=0) " & _
                                "AND ((trans.Cc=@ccid) or (cc.Olid in (Select Store From cld_userxstore where user = '" & usr & "'))) " & _
                                "GROUP BY trans.Cc, trans.`Stk_no` " & _
                                 "ORDER BY trans.`Cc` ")

            Else

                connDb.resetCmd("SELECT trans.`Cc_name`, trans.`CreatedBy`, trans.`Stk_no`, trans.`Stk_Name` " & _
                               ", SUM(trans.`U1`), trans.`U1_nm`, trans.`Factor1`, SUM(trans.u2), trans.`U2_nm`, SUM(trans.`ttlStock`) " & _
                                "FROM`cld_inv_trans` trans " & _
                                "WHERE (trans.`flagtrans` = 1 AND trans.`type`=9) " & _
                                "AND trans.`InvDate` >=@paramFrom AND trans.`InvDate` <=@paramTo " & _
                                "AND (trans.`custid` =@custID OR @custID=0) " & _
                                "AND (trans.Cc=@ccid or @ccid=0) " & _
                                "GROUP BY trans.Cc, trans.`Stk_no` " & _
                                 "ORDER BY trans.`Cc` ")
            End If
            connDb.addParam(paramFrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(paramTo, "@paramTo", MySqlDbType.DateTime)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)
            connDb.addParam(ccid, "@ccid", MySqlDbType.Int32)

            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:productionRpt")
            Err.Clear()
        End Try

        Return dsRslt
    End Function

    Public Function WasteRpt(ByVal paramFrom As Date, ByVal paramTo As Date, ByVal ccid As Integer, ByVal custID As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim dsRslt As New DataSet

        Try

            Dim connDb As New clsDbConn(ServerString)
            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                connDb.resetCmd("SELECT trans.`InvDate`, trans.`Cc_name`, trans.`Stk_Name`, trans.`U1`, trans.`U1_nm`, trans.`Factor1`, trans.`U2`, trans.`U2_nm`,trans.`ttlStock` " & _
                                "FROM `cld_inv_trans` trans " & _
                                "LEFT JOIN `cld_inv_cc` cc ON trans.`Cc`=cc.`ID` " & _
                                "WHERE (trans.`flagtrans` = 1 AND trans.`type`=2) " & _
                                "AND trans.`InvDate` >=@paramFrom AND trans.`InvDate` <=@paramTo " & _
                                "AND (trans.`custid` =@custID OR @custID=0) " & _
                                "AND ((trans.Cc=@ccid) or (cc.Olid in (Select Store From cld_userxstore where user = '" & usr & "'))) " & _
                                 "ORDER BY trans.`InvDate` ")

            Else

                connDb.resetCmd("SELECT trans.`InvDate`, trans.`Cc_name`, trans.`Stk_Name`, trans.`U1`, trans.`U1_nm`, trans.`Factor1`, trans.`U2`, trans.`U2_nm`,trans.`ttlStock` " & _
                                "FROM `cld_inv_trans` trans " & _
                                "LEFT JOIN `cld_inv_cc` cc ON trans.`Cc`=cc.`ID` " & _
                                "WHERE (trans.`flagtrans` = 1 AND trans.`type`=2) " & _
                                "AND trans.`InvDate` >=@paramFrom AND trans.`InvDate` <=@paramTo " & _
                                "AND (trans.`custid` =@custID OR @custID=0) " & _
                                "AND (trans.Cc=@ccid or @ccid=0) " & _
                                 "ORDER BY trans.`InvDate` ")
            End If
            connDb.addParam(paramFrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(paramTo, "@paramTo", MySqlDbType.DateTime)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)
            connDb.addParam(ccid, "@ccid", MySqlDbType.Int32)



            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:WasteRpt")
            Err.Clear()
        End Try

        Return dsRslt
    End Function

    Public Function sumPendPoRptDetail(ByVal paramFrom As Date, ByVal paramTo As Date, ByVal ccid As Integer, ByVal custID As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim dsRslt As New DataSet

        Try

            Dim connDb As New clsDbConn(ServerString)
            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                connDb.resetCmd("SELECT rcv.id, b.`Name`, cc.Name as ccname, rcv.`CreatedBy`, date_format(rcv.`CreatedDate`, '%d %b %Y') as CreatedDate, date_format(rcv.`exDelDate`, '%d %b %Y') as exDelDate, rcv.`send`, t.`Stk_no`, t.`Stk_Name`, t.u1, t.`U1_nm` " & _
                                "FROM `cld_invlk_rcv` rcv " & _
                                "LEFT JOIN `cld_inv_supplier` b ON rcv.`SupId`=b.`ID` and rcv.`Custid`= b.`Custid` " & _
                                "LEFT JOIN `cld_inv_cc` cc ON rcv.`Cc`=cc.`ID` and rcv.`Custid`= cc.`Custid` " & _
                                "LEFT JOIN `cld_inv_trans` t ON rcv.`ID`= t.`Linkid` and rcv.`Custid`= t.`Custid` " & _
                                "WHERE (rcv.flag = 1 and t.type=3) " & _
                                "and rcv.`InvDate` >=@paramFrom AND rcv.`InvDate` <=@paramTo " & _
                                 "AND (rcv.`custid` =@custID OR @custID=0) " & _
                                "AND ((rcv.Cc=@ccid) or (cc.Olid in (Select Store From cld_userxstore where user = '" & usr & "'))) " & _
                                "ORDER BY rcv.`exDelDate` DESC ")

            Else

                connDb.resetCmd("SELECT rcv.id, b.`Name`, cc.Name as ccname, rcv.`CreatedBy`, date_format(rcv.`CreatedDate`, '%d %b %Y') as CreatedDate, date_format(rcv.`exDelDate`, '%d %b %Y') as exDelDate, rcv.`send`, t.`Stk_no`, t.`Stk_Name`, t.u1, t.`U1_nm` " & _
                                "FROM `cld_invlk_rcv` rcv " & _
                                 "LEFT JOIN `cld_inv_supplier` b ON rcv.`SupId`=b.`ID` and rcv.`Custid`= b.`Custid` " & _
                                "LEFT JOIN `cld_inv_cc` cc ON rcv.`Cc`=cc.`ID` and rcv.`Custid`= cc.`Custid` " & _
                                "LEFT JOIN `cld_inv_trans` t ON rcv.`ID`= t.`Linkid` and rcv.`Custid`= t.`Custid` " & _
                                "WHERE (rcv.flag = 1 and t.type=3) " & _
                                "and rcv.`InvDate` >=@paramFrom AND rcv.`InvDate` <=@paramTo " & _
                                 "AND (rcv.`custid` =@custID OR @custID=0) " & _
                                "AND (rcv.Cc=@ccid or @ccid=0) " & _
                                "ORDER BY rcv.`exDelDate` DESC ")
            End If
            connDb.addParam(paramFrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(paramTo, "@paramTo", MySqlDbType.DateTime)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)
            connDb.addParam(ccid, "@ccid", MySqlDbType.Int32)


            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:sumPendPoRptDetail")
            Err.Clear()
        End Try

        Return dsRslt
    End Function

    Public Function sumPendPoRpt(ByVal paramFrom As Date, ByVal paramTo As Date, ByVal ccid As Integer, ByVal custID As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim dsRslt As New DataSet

        Try

            Dim connDb As New clsDbConn(ServerString)
            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                connDb.resetCmd("SELECT rcv.id, cc.Name as ccname, b.`Name`, rcv.`DocTot`, rcv.`CreatedBy`, date_format(rcv.`CreatedDate`, '%d %b %Y') as CreatedDate, date_format(rcv.`exDelDate`, '%d %b %Y') as exDelDate, rcv.`send`, COUNT(t.`Stk_Id`) AS jmlitem " & _
                                "FROM `cld_invlk_rcv` rcv " & _
                                "LEFT JOIN `cld_inv_supplier` b ON rcv.`SupId`=b.`ID` and rcv.`Custid`= b.`Custid` " & _
                                "LEFT JOIN `cld_inv_cc` cc ON rcv.`Cc`=cc.`ID` and rcv.`Custid`= cc.`Custid` " & _
                                "LEFT JOIN `cld_inv_trans` t ON rcv.`ID`= t.`Linkid` and rcv.`Custid`= t.`Custid` " & _
                                "WHERE (rcv.flag = 1 and t.type=3) " & _
                                "and rcv.`InvDate` >=@paramFrom AND rcv.`InvDate` <=@paramTo " & _
                                 "AND (rcv.`custid` =@custID OR @custID=0) " & _
                                "AND ((rcv.Cc=@ccid) or (cc.Olid in (Select Store From cld_userxstore where user = '" & usr & "'))) " & _
                                "GROUP BY rcv.id " & _
                                "ORDER BY rcv.`exDelDate` DESC ")

            Else

                connDb.resetCmd("SELECT rcv.id, cc.Name as ccname, b.`Name`, rcv.`DocTot`, rcv.`CreatedBy`, date_format(rcv.`CreatedDate`, '%d %b %Y') as CreatedDate, date_format(rcv.`exDelDate`, '%d %b %Y') as exDelDate, rcv.`send`, COUNT(t.`Stk_Id`) AS jmlitem " & _
                                 "FROM `cld_invlk_rcv` rcv " & _
                                "LEFT JOIN `cld_inv_supplier` b ON rcv.`SupId`=b.`ID` and rcv.`Custid`= b.`Custid` " & _
                                "LEFT JOIN `cld_inv_cc` cc ON rcv.`Cc`=cc.`ID` and rcv.`Custid`= cc.`Custid` " & _
                                "LEFT JOIN `cld_inv_trans` t ON rcv.`ID`= t.`Linkid` and rcv.`Custid`= t.`Custid` " & _
                                 "WHERE (rcv.flag = 1 and t.type=3) " & _
                                 "and rcv.`InvDate` >=@paramFrom AND rcv.`InvDate` <=@paramTo " & _
                                  "AND (rcv.`custid` =@custID OR @custID=0) " & _
                                 "AND (rcv.Cc=@ccid or @ccid=0) " & _
                                 "GROUP BY rcv.id " & _
                                 "ORDER BY rcv.`exDelDate` DESC ")
            End If
            connDb.addParam(paramFrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(paramTo, "@paramTo", MySqlDbType.DateTime)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)
            connDb.addParam(ccid, "@ccid", MySqlDbType.Int32)




            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:sumPendPoRpt")
            Err.Clear()
        End Try

        Return dsRslt
    End Function

    Private Function InvSupplier(ByVal custID As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim dsRslt As New DataSet

        Try
            Dim connDb As New clsDbConn(ServerString)


            connDb.resetCmd("SELECT NAME, email, address, phone, city, state, pcode " & _
            "FROM `cld_inv_supplier`" & _
            "WHERE (CustID=@custID OR @custID=0)")


            connDb.addParam(custID, "@custID", MySqlDbType.Int32)

            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing
        Catch ex As Exception
            errLog(Err.Description, "ClsReports:InvSupplier")
            Err.Clear()
        End Try

        Return dsRslt
    End Function

    Private Function InvStock(ByVal custID As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim dsRslt As New DataSet

        Try
            Dim connDb As New clsDbConn(ServerString)

            connDb.resetCmd("SELECT a.number, a.name, b.`Name` AS grpname, a.`Unit1`, a.factor1 , a.Unit2, a.factor2, a.unit3, a.`stkLimit` " & _
"FROM `cld_inv_stk` a LEFT JOIN `cld_inv_stkgroup` b ON a.`grpid`=b.`ID` and a.`Custid`=b.`Custid` " & _
"WHERE a.`Batch`=FALSE AND (a.CustID=@custID OR @custID=0) " & _
                              "ORDER BY a.`grpid`")

            connDb.addParam(custID, "@custID", MySqlDbType.Int32)

            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing
        Catch ex As Exception
            errLog(Err.Description, "ClsReports:InvStock")
            Err.Clear()
        End Try

        Return dsRslt


    End Function

    '9 nov 2017
    Public Function minLimitStkRpt(ByVal ccid As Integer, ByVal custID As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim dsRslt As New DataSet

        Try

            Dim connDb As New clsDbConn(ServerString)
            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                connDb.resetCmd("SELECT trstk.`stkNo`, trstk.`stkName`,trstk.`ccName`, trstk.`ttlQty`, stk.`Unit1`, stk.`stkLimit`, stk.`Unit1` " & _
                                "FROM `cld_inv_transstock` trstk LEFT JOIN `cld_inv_stk` stk " & _
                                "ON stk.`ID` = trstk.`stkId` AND stk.`Number` = trstk.`stkNo` AND stk.`CustID` = trstk.`custId` " & _
                                "LEFT JOIN `cld_inv_cc` cc ON cc.id = trstk.ccid AND cc.custid = trstk.custid " & _
                                "WHERE trstk.`ttlQty`<= stk.`stkLimit` " & _
                                "AND (trstk.`custId` =@custID OR @custID=0) " & _
                                "AND ((trstk.`ccid`=@ccid) or (cc.Olid in (Select Store From cld_userxstore where user = '" & usr & "'))) " & _
                                 "ORDER BY trstk.`stkId` ")

            Else

                connDb.resetCmd("SELECT trstk.`stkNo`, trstk.`stkName`,trstk.`ccName`, trstk.`ttlQty`, stk.`Unit1`, stk.`stkLimit`, stk.`Unit1` " & _
                                "FROM `cld_inv_transstock` trstk LEFT JOIN `cld_inv_stk` stk " & _
                                "ON stk.`ID` = trstk.`stkId` AND stk.`Number` = trstk.`stkNo` AND stk.`CustID` = trstk.`custId` " & _
                                "LEFT JOIN `cld_inv_cc` cc ON cc.id = trstk.ccid AND cc.custid = trstk.custid " & _
                                "WHERE trstk.`ttlQty`<= stk.`stkLimit` " & _
                                "AND (trstk.`custId` =@custID OR @custID=0) " & _
                                "AND (trstk.`ccid`=@ccid or @ccid=0) " & _
                                 "ORDER BY trstk.`stkId` ")
            End If
            'connDb.addParam(paramFrom, "@paramFrom", MySqlDbType.DateTime)
            'connDb.addParam(paramTo, "@paramTo", MySqlDbType.DateTime)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)
            connDb.addParam(ccid, "@ccid", MySqlDbType.Int32)



            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:minLimitStkRpt")
            Err.Clear()
        End Try

        Return dsRslt
    End Function

    Public Function InvCallcenterRpt(ByVal custID As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim dsRslt As New DataSet

        Try

            Dim connDb As New clsDbConn(ServerString)

            connDb.resetCmd("SELECT cc.name, cc.remarks, CASE cc.olid WHEN 0 THEN cc.name ELSE o.nama END AS outlet, cc.email, cc.address, cc.pic " & _
                            "FROM `cld_inv_cc` cc LEFT JOIN `cld_outlet` o ON o.id = cc.olid AND o.custid = cc.custid " & _
                            "WHERE (cc.`custid` =@custID OR @custID=0) ")


            'connDb.addParam(paramFrom, "@paramFrom", MySqlDbType.DateTime)
            'connDb.addParam(paramTo, "@paramTo", MySqlDbType.DateTime)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)
            'connDb.addParam(ccid, "@ccid", MySqlDbType.Int32)



            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:receivedInvoiceInvRpt")
            Err.Clear()
        End Try

        Return dsRslt
    End Function

    Public Function InvReceiveRpt(ByVal paramFrom As Date, ByVal paramTo As Date, ByVal ccid As Integer, ByVal custID As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim dsRslt As New DataSet

        Try

            Dim connDb As New clsDbConn(ServerString)
            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                connDb.resetCmd("SELECT rcv.id, rcv.do, supp.name AS supplier, " & _
                                "CASE rcv.flag WHEN 1 THEN 'submit' ELSE 'finish' END AS orderstat,rcv.doctot, " & _
                                "CASE rcv.tipe WHEN 1 THEN 'Receiving' ELSE 'Return' END AS tipe, " & _
                                "CASE cc.olid WHEN 0 THEN cc.name ELSE o.nama END AS outlet, rcv.createdby, rcv.invdate, invoice.number " & _
                                "FROM `cld_invlk_rcv` rcv LEFT JOIN `cld_inv_cc` cc ON cc.id = rcv.cc AND cc.custid = rcv.custid " & _
                                "LEFT JOIN `cld_outlet` o ON o.id = cc.olid AND o.custid = rcv.custid " & _
                                "LEFT JOIN`cld_inv_supplier` supp ON supp.id = rcv.SupId AND supp.custid = rcv.custid " & _
                                "LEFT JOIN `cld_inv_invoice` invoice ON invoice.id = rcv.invoiceID AND invoice.custid = rcv.custid " & _
                                "WHERE (rcv.invoiceid <> 0) and rcv.`InvDate` >=@paramFrom AND rcv.`InvDate` <=@paramTo " & _
                                "AND (rcv.`custid` =@custID OR @custID=0) " & _
                                "AND ((rcv.Cc=@ccid) or (cc.Olid in (Select Store From cld_userxstore where user = '" & usr & "'))) " & _
                                 "ORDER BY rcv.`InvDate` ")

            Else

                connDb.resetCmd("SELECT rcv.id, rcv.do, supp.name AS supplier, " & _
                                "CASE rcv.flag WHEN 1 THEN 'submit' ELSE 'finish' END AS orderstat,rcv.doctot, " & _
                                "CASE rcv.tipe WHEN 1 THEN 'Receiving' ELSE 'Return' END AS tipe, " & _
                                "CASE cc.olid WHEN 0 THEN cc.name ELSE o.nama END AS outlet, rcv.createdby, rcv.invdate, invoice.number " & _
                                "FROM `cld_invlk_rcv` rcv LEFT JOIN `cld_inv_cc` cc ON cc.id = rcv.cc AND cc.custid = rcv.custid " & _
                                "LEFT JOIN `cld_outlet` o ON o.id = cc.olid AND o.custid = rcv.custid " & _
                                "LEFT JOIN`cld_inv_supplier` supp ON supp.id = rcv.SupId AND supp.custid = rcv.custid " & _
                                "LEFT JOIN `cld_inv_invoice` invoice ON invoice.id = rcv.invoiceID AND invoice.custid = rcv.custid " & _
                                "WHERE (rcv.invoiceid <> 0) and rcv.`InvDate` >=@paramFrom AND rcv.`InvDate` <=@paramTo " & _
                                "AND (rcv.`custid` =@custID OR @custID=0) " & _
                                "AND (rcv.Cc=@ccid or @ccid=0) " & _
                                 "ORDER BY rcv.`InvDate` ")
            End If
            connDb.addParam(paramFrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(paramTo, "@paramTo", MySqlDbType.DateTime)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)
            connDb.addParam(ccid, "@ccid", MySqlDbType.Int32)



            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:InvReceiveRpt")
            Err.Clear()
        End Try

        Return dsRslt
    End Function

    Public Function notinvoicingInvRpt(ByVal paramFrom As Date, ByVal paramTo As Date, ByVal ccid As Integer, ByVal custID As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim dsRslt As New DataSet

        Try

            Dim connDb As New clsDbConn(ServerString)
            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                connDb.resetCmd("SELECT rcv.id, rcv.do, supp.name AS supplier, " &
                                "CASE rcv.flag WHEN 1 THEN 'submit' ELSE 'finish' END AS orderstat,rcv.doctot, " &
                                "CASE rcv.tipe WHEN 1 THEN 'Receiving' ELSE 'Return' END AS tipe, " &
                                "CASE cc.olid WHEN 0 THEN cc.name ELSE o.nama END AS outlet, rcv.createdby, rcv.invdate, rcv.invoiceid " &
                                "FROM `cld_invlk_rcv` rcv LEFT JOIN `cld_inv_cc` cc ON cc.id = rcv.cc AND cc.custid = rcv.custid " &
                                "LEFT JOIN `cld_outlet` o ON o.id = cc.olid AND o.custid = rcv.custid " &
                                "LEFT JOIN`cld_inv_supplier` supp ON supp.id = rcv.SupId AND supp.custid = rcv.custid " &
                                "WHERE (rcv.invoiceid = 0) " &
                                "AND rcv.`InvDate` >=@paramFrom AND rcv.`InvDate` <=@paramTo " &
                                "AND (rcv.`custid` =@custID OR @custID=0) " &
                                "AND ((rcv.Cc=@ccid) or (cc.Olid in (Select Store From cld_userxstore where user = '" & usr & "'))) " &
                                 "ORDER BY rcv.`InvDate` ")

            Else

                connDb.resetCmd("SELECT rcv.id, rcv.do, supp.name AS supplier, " &
                                "CASE rcv.flag WHEN 1 THEN 'submit' ELSE 'finish' END AS orderstat,rcv.doctot, " &
                                "CASE rcv.tipe WHEN 1 THEN 'Receiving' ELSE 'Return' END AS tipe, " &
                                "CASE cc.olid WHEN 0 THEN cc.name ELSE o.nama END AS outlet, rcv.createdby, rcv.invdate, rcv.invoiceid " &
                                "FROM `cld_invlk_rcv` rcv LEFT JOIN `cld_inv_cc` cc ON cc.id = rcv.cc AND cc.custid = rcv.custid " &
                                "LEFT JOIN `cld_outlet` o ON o.id = cc.olid AND o.custid = rcv.custid " &
                                "LEFT JOIN`cld_inv_supplier` supp ON supp.id = rcv.SupId AND supp.custid = rcv.custid " &
                                "WHERE (rcv.invoiceid = 0) " &
                                "AND rcv.`InvDate` >=@paramFrom AND rcv.`InvDate` <=@paramTo " &
                                "AND (rcv.`custid` =@custID OR @custID=0) " &
                                "AND (rcv.Cc=@ccid or @ccid=0) " &
                                 "ORDER BY rcv.`InvDate` ")
            End If
            connDb.addParam(paramFrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(paramTo, "@paramTo", MySqlDbType.DateTime)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)
            connDb.addParam(ccid, "@ccid", MySqlDbType.Int32)



            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:receivedInvoiceInvRpt")
            Err.Clear()
        End Try

        Return dsRslt
    End Function


    'revisi 23apr2018
    Public Function PosVSWeb(ByVal custID As Integer, ByVal dfrom As Date, ByVal dTo As Date, ByVal Outletstr As String, ByVal iOwn As String, ByVal iBrand As String, ByVal iCity As String, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim dsRslt As New DataSet
        Dim sOut As String = ""
        Dim sBrand As String = ""
        Dim sOwn As String = ""
        Dim sCity As String = ""

        Try
            Dim connDb As New clsDbConn(ServerString)
            If iBrand = "0" Or iBrand = "" Then
                sBrand = ""
            Else
                sBrand = " and cld_outlet.brandID in (" & iBrand & ")"
            End If

            If iOwn = "0" Or iOwn = "" Then
                sOwn = ""
            Else
                sOwn = " and cld_outlet.ownID = " & iOwn
            End If
            If iCity = "0" Or iCity = "" Then
                sCity = ""
            Else
                sCity = " and cld_outlet.cityID in (" & iCity & ")"
            End If

            'If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
            '    If Outletstr = "" Then
            '        sOut = " and cld_eod.storeID in (Select Store From cld_userxstore where user = '" & usr & "')"
            '    Else
            '        sOut = " and cld_eod.StoreNo in (" & Outletstr & ")"
            '    End If
            'Else
            '    If Outletstr = "" Then
            '        sOut = ""
            '    Else
            '        sOut = " and cld_eod.StoreNo in (" & Outletstr & ")"
            '    End If
            'End If

            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If Outletstr = "" Then
                    sOut = " and cld_outlet.ID in (Select Store From cld_userxstore where user = '" & usr & "')"
                Else
                    sOut = " and cld_outlet.kode in (" & Outletstr & ")"
                End If
            Else
                If Outletstr = "" Then
                    sOut = ""
                Else
                    sOut = " and cld_outlet.Kode in (" & Outletstr & ")"
                End If
            End If


            'perubahan 24 juni 2019, menampilkan subtotal yang 0, hanya yang - 1 tidak tampil
            'connDb.resetCmd("SELECT Storeno, cld_outlet.`Nama` ,EodDate, Subtotal, IFNULL((SELECT SUM(Subtotal -discount) " &
            '         "FROM cld_sumsalesdaily WHERE Busidate = cld_eod.`eodDate` AND olid = cld_eod.`storeID`),0) AS webTotal " &
            '         "FROM cld_eod INNER JOIN cld_outlet ON cld_eod.`storeID` = cld_outlet.`ID` AND " &
            '         "cld_eod.`CustId` = cld_outlet.`custID` " &
            '         "where subtotal <> -1 and cld_eod.EodDate >= @paramFrom And cld_eod.EodDate <= @paramTo " &
            '         " AND (cld_eod.CustID =@custID OR @custID=0) " & sOut & sOwn & sBrand & sCity)

            'connDb.resetCmd("SELECT Storeno, cld_outlet.`Nama` ,EodDate, IF(subtotal > -1, subtotal, 0) as Subtotal, Subtotal as Subtotal2, IFNULL((SELECT SUM(Subtotal -discount) " &
            '         "FROM cld_sumsalesdaily WHERE Busidate = cld_eod.`eodDate` AND olid = cld_eod.`storeID`),0) AS webTotal " &
            '         "FROM cld_eod INNER JOIN cld_outlet ON cld_eod.`storeID` = cld_outlet.`ID` AND " &
            '         "cld_eod.`CustId` = cld_outlet.`custID` " &
            '         "where cld_eod.EodDate >= @paramFrom And cld_eod.EodDate <= @paramTo " &
            '         " AND (cld_eod.CustID =@custID OR @custID=0) " & sOut & sOwn & sBrand & sCity &
            '         " UNION ALL " &
            '         "SELECT Kode AS Storeno, Nama, @paramTo AS EodDate, 0 AS Subtotal, -1 AS Subtotal2, 0 AS webtotal " &
            '         "From cld_outlet Where Id Not IN (SELECT Storeid FROM cld_eod WHERE eodDate = @paramTo) " &
            '         "AND active = 1 AND startdate <= @paramTo AND (cld_outlet.CustID =@custID OR @custID=0) " & sOut & sOwn & sBrand & sCity)


            'perubahan 4 july 2019
            'connDb.resetCmd("SELECT Storeno, cld_outlet.`Nama` ,EodDate, IF(subtotal > -1, subtotal, 0) as Subtotal, Subtotal as Subtotal2, IFNULL((SELECT SUM(Subtotal -discount) " &
            '         "FROM cld_sumsalesdaily WHERE Busidate = cld_eod.`eodDate` AND olid = cld_eod.`storeID`),0) AS webTotal, IFNULL((SELECT MAX(postdate) " &
            '         "FROM cld_last WHERE storeid = cld_eod.`storeID`),'') AS lastsync " &
            '         "FROM cld_eod INNER JOIN cld_outlet ON cld_eod.`storeID` = cld_outlet.`ID` AND " &
            '         "cld_eod.`CustId` = cld_outlet.`custID` " &
            '         "where cld_eod.EodDate >= @paramFrom And cld_eod.EodDate <= @paramTo " &
            '         " AND (cld_eod.CustID =@custID OR @custID=0) " & sOut & sOwn & sBrand & sCity &
            '         " UNION ALL " &
            '         "SELECT Kode AS Storeno, Nama, @paramTo AS EodDate, 0 AS Subtotal, -1 AS Subtotal2, 0 AS webtotal, IFNULL((SELECT MAX(postdate) " &
            '         "FROM cld_last WHERE storeid = cld_outlet.id),'') AS lastsync " &
            '         "From cld_outlet Where Id Not IN (SELECT Storeid FROM cld_eod WHERE eodDate = @paramTo) " &
            '         "AND active = 1 AND startdate <= @paramTo AND (cld_outlet.CustID =@custID OR @custID=0) " & sOut & sOwn & sBrand & sCity)

            'perubahan 5 july 2019
            'connDb.resetCmd("SELECT Storeno, StoreName 'Store Name', Eoddate 'Eod Date', POS_sub_after_disc, WebReport_sub_after_disc, selisih, lastsync, STATUS  FROM ( " &
            '            "SELECT Storeno, cld_outlet.`Nama` 'StoreName',EodDate, IF(subtotal > -1, subtotal, 0) as POS_sub_after_disc, IFNULL((SELECT SUM(Subtotal -discount) " &
            '         "FROM cld_sumsalesdaily WHERE Busidate = cld_eod.`eodDate` AND olid = cld_eod.`storeID`),0) AS WebReport_sub_after_disc " &
            '         ",FORMAT(IF(subtotal > -1, subtotal, 0)-IFNULL((SELECT SUM(Subtotal -discount) FROM cld_sumsalesdaily WHERE Busidate = cld_eod.`eodDate` AND olid = cld_eod.`storeID`),0),0) as selisih " &
            '         ", IFNULL((SELECT MAX(postdate) " &
            '         "FROM cld_last WHERE storeid = cld_eod.`storeID`),'') AS lastsync " &
            '         ", CASE
            '                WHEN FORMAT((IF(subtotal > -1, subtotal, 0)-IFNULL((SELECT SUM(Subtotal -discount) FROM cld_sumsalesdaily WHERE Busidate = cld_eod.`eodDate` AND olid = cld_eod.`storeID`),0)),0) = 0 THEN 'OK'
            '                WHEN (FORMAT((IF(subtotal > -1, subtotal, 0)-IFNULL((SELECT SUM(Subtotal -discount) FROM cld_sumsalesdaily WHERE Busidate = cld_eod.`eodDate` AND olid = cld_eod.`storeID`),0)),0) <> 0  
            '                AND dc = 0) THEN 'Selisih'
            '                WHEN (FORMAT((IF(subtotal > -1, subtotal, 0)-IFNULL((SELECT SUM(Subtotal -discount) FROM cld_sumsalesdaily WHERE Busidate = cld_eod.`eodDate` AND olid = cld_eod.`storeID`),0)),0) <> 0  
            '                AND dc = 1) THEN 'Dreamcloud'
            '                END
            '                 AS STATUS " &
            '         "FROM cld_eod INNER JOIN cld_outlet ON cld_eod.`storeID` = cld_outlet.`ID` AND " &
            '         "cld_eod.`CustId` = cld_outlet.`custID` " &
            '         "where cld_eod.EodDate >= @paramFrom And cld_eod.EodDate <= @paramTo " &
            '         " AND (cld_eod.CustID =@custID OR @custID=0) " & sOut & sOwn & sBrand & sCity &
            '         " UNION ALL " &
            '         "SELECT Kode AS Storeno, Nama, @paramTo AS EodDate, 0 AS POS_sub_after_disc, IFNULL((SELECT SUM(Subtotal -discount) " &
            '         "FROM cld_sumsalesdaily WHERE Busidate = @paramTo AND olid = cld_outlet.id),0) AS WebReport_sub_after_disc " &
            '         ", (0-(IFNULL((SELECT SUM(Subtotal -discount)FROM cld_sumsalesdaily WHERE Busidate = @paramTo AND olid = cld_outlet.id),0))) as selisih, IFNULL((SELECT MAX(postdate) " &
            '         "FROM cld_last WHERE storeid = cld_outlet.id),'') AS lastsync " &
            '         ", 'belum eod' AS status " &
            '         "From cld_outlet Where Id Not IN (SELECT Storeid FROM cld_eod WHERE eodDate = @paramTo) " &
            '         "AND active = 1 AND startdate <= @paramTo AND (cld_outlet.CustID =@custID OR @custID=0) " & sOut & sOwn & sBrand & sCity & ") a order by status,eoddate,storeno ")

            connDb.resetCmd("SELECT Eod_Date 'Sales Date', Storeno 'Store Code', StoreName 'Store Name', POS_sub_after_disc 'Pos Net Sales', WebReport_sub_after_disc 'Web Net Sales', Selisih, DATE_FORMAT(lastsync,'%d-%b-%Y %H:%i:%s') 'Last Sync', Status  FROM ( " &
                        "Select Storeno, cld_outlet.`Nama` 'StoreName' ,date_format(EodDate, '%d-%b-%Y') Eod_Date, IF(subtotal > -1, subtotal, 0) as POS_sub_after_disc, IFNULL((SELECT SUM(Subtotal -discount) " &
                     "FROM cld_sumsalesdaily WHERE Busidate = cld_eod.`eodDate` AND olid = cld_eod.`storeID`),0) AS WebReport_sub_after_disc " &
                     ", (IF(subtotal > -1, subtotal, 0)-IFNULL((SELECT SUM(Subtotal -discount) FROM cld_sumsalesdaily WHERE Busidate = cld_eod.`eodDate` AND olid = cld_eod.`storeID`),0)) as selisih " &
                     ", IFNULL((SELECT MAX(postdate) " &
                     "FROM cld_last WHERE storeid = cld_eod.`storeID`),'') AS lastsync " &
                      ", CASE
                            WHEN FORMAT((IF(subtotal > -1, subtotal, 0)-IFNULL((SELECT SUM(Subtotal -discount) FROM cld_sumsalesdaily WHERE Busidate = cld_eod.`eodDate` AND olid = cld_eod.`storeID`),0)),0) = 0 THEN 'OK'
                            WHEN (FORMAT((IF(subtotal > -1, subtotal, 0)-IFNULL((SELECT SUM(Subtotal -discount) FROM cld_sumsalesdaily WHERE Busidate = cld_eod.`eodDate` AND olid = cld_eod.`storeID`),0)),0) <> 0  
                            AND dc = 0) THEN 'Selisih'
                            WHEN (FORMAT((IF(subtotal > -1, subtotal, 0)-IFNULL((SELECT SUM(Subtotal -discount) FROM cld_sumsalesdaily WHERE Busidate = cld_eod.`eodDate` AND olid = cld_eod.`storeID`),0)),0) <> 0  
                            AND dc = 1) THEN 'Dreamcloud'
                            END
                             AS STATUS " &
                             ", eoddate " &
                     "FROM cld_eod INNER JOIN cld_outlet On cld_eod.`storeID` = cld_outlet.`ID` And " &
                     "cld_eod.`CustId` = cld_outlet.`custID` " &
                     "where cld_eod.EodDate >= @paramFrom And cld_eod.EodDate <= @paramTo " &
                     " And (cld_eod.CustID =@custID Or @custID=0) " & sOut & sOwn & sBrand & sCity &
                      " UNION ALL " &
                     "Select Kode As Storeno, Nama, date_format(@paramTo, '%d %b %Y') AS Eod_Date, 0 AS POS_sub_after_disc, IFNULL((SELECT SUM(Subtotal -discount) " &
                     "FROM cld_sumsalesdaily WHERE Busidate = @paramTo AND olid = cld_outlet.id),0) AS WebReport_sub_after_disc " &
                     ", (0-(IFNULL((SELECT SUM(Subtotal -discount)FROM cld_sumsalesdaily WHERE Busidate = @paramTo AND olid = cld_outlet.id),0))) as selisih, IFNULL((SELECT MAX(postdate) " &
                     "FROM cld_last WHERE storeid = cld_outlet.id),'') AS lastsync " &
                     ", 'belum eod' AS status, @paramTo AS EodDate " &
                     "From cld_outlet Where Id Not IN (SELECT Storeid FROM cld_eod WHERE eodDate = @paramTo) " &
                     "AND active = 1 AND startdate <= @paramTo AND (cld_outlet.CustID =@custID OR @custID=0) " & sOut & sOwn & sBrand & sCity & ") a order by status,eoddate,storeno ")

            connDb.addParam(dfrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(dTo, "@paramTo", MySqlDbType.DateTime)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)


            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:salesSum")
            Err.Clear()
        End Try

        Return dsRslt
    End Function

    Public Function PosVSWeb_nonsales(ByVal custID As Integer, ByVal dfrom As Date, ByVal dTo As Date, ByVal Outletstr As String, ByVal iOwn As String, ByVal iBrand As String, ByVal iCity As String, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim dsRslt As New DataSet
        Dim sOut As String = ""
        Dim sBrand As String = ""
        Dim sOwn As String = ""
        Dim sCity As String = ""

        Try
            Dim connDb As New clsDbConn(ServerString)
            If iBrand = "0" Or iBrand = "" Then
                sBrand = ""
            Else
                sBrand = " and cld_outlet.brandID in (" & iBrand & ")"
            End If

            If iOwn = "0" Or iOwn = "" Then
                sOwn = ""
            Else
                sOwn = " and cld_outlet.ownID = " & iOwn
            End If
            If iCity = "0" Or iCity = "" Then
                sCity = ""
            Else
                sCity = " and cld_outlet.cityID in (" & iCity & ")"
            End If



            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If Outletstr = "" Then
                    sOut = " and cld_outlet.ID in (Select Store From cld_userxstore where user = '" & usr & "')"
                Else
                    sOut = " and cld_outlet.kode in (" & Outletstr & ")"
                End If
            Else
                If Outletstr = "" Then
                    sOut = ""
                Else
                    sOut = " and cld_outlet.Kode in (" & Outletstr & ")"
                End If
            End If


            connDb.resetCmd("SELECT Eod_Date 'Sales Date', Storeno 'Store Code', StoreName 'Store Name', POS_sub_after_disc 'Pos Net Sales', WebReport_sub_after_disc 'Web Net Sales', Selisih, DATE_FORMAT(lastsync,'%d-%b-%Y %H:%i:%s') 'Last Sync', Status  FROM ( " &
                        "Select Storeno, cld_outlet.`Nama` 'StoreName' ,date_format(EodDate, '%d-%b-%Y') Eod_Date, FORMAT(IF(subtotal > -1, subtotal, 0), 2,'de_DE') as POS_sub_after_disc, FORMAT(IFNULL((SELECT SUM(Subtotal -discount) " &
                     "FROM cld_sumnosalesdaily WHERE Busidate = cld_eod_nonsales.`eodDate` AND olid = cld_eod_nonsales.`storeID`),0), 2,'de_DE') AS WebReport_sub_after_disc " &
                     ", FORMAT((IF(subtotal > -1, subtotal, 0)-IFNULL((SELECT SUM(Subtotal -discount) FROM cld_sumnosalesdaily WHERE Busidate = cld_eod_nonsales.`eodDate` AND olid = cld_eod_nonsales.`storeID`),0)), 0,'de_DE') as selisih " &
                     ", IFNULL((SELECT MAX(postdate) " &
                     "FROM cld_last WHERE storeid = cld_eod_nonsales.`storeID`),'') AS lastsync " &
                      ", CASE
                            WHEN FORMAT((IF(subtotal > -1, subtotal, 0)-IFNULL((SELECT SUM(Subtotal -discount) FROM cld_sumnosalesdaily WHERE Busidate = cld_eod_nonsales.`eodDate` AND olid = cld_eod_nonsales.`storeID`),0)),0) = 0 THEN 'OK'
                            WHEN (FORMAT((IF(subtotal > -1, subtotal, 0)-IFNULL((SELECT SUM(Subtotal -discount) FROM cld_sumnosalesdaily WHERE Busidate = cld_eod_nonsales.`eodDate` AND olid = cld_eod_nonsales.`storeID`),0)),0) <> 0  
                            AND dc = 0) THEN 'Selisih'
                            WHEN (FORMAT((IF(subtotal > -1, subtotal, 0)-IFNULL((SELECT SUM(Subtotal -discount) FROM cld_sumnosalesdaily WHERE Busidate = cld_eod_nonsales.`eodDate` AND olid = cld_eod_nonsales.`storeID`),0)),0) <> 0  
                            AND dc = 1) THEN 'Dreamcloud'
                            END
                             AS STATUS " &
                             ", eoddate " &
                     "FROM cld_eod_nonsales INNER JOIN cld_outlet On cld_eod_nonsales.`storeID` = cld_outlet.`ID` And " &
                     "cld_eod_nonsales.`CustId` = cld_outlet.`custID` " &
                     "where cld_eod_nonsales.EodDate >= @paramFrom And cld_eod_nonsales.EodDate <= @paramTo " &
                     " And (cld_eod_nonsales.CustID =@custID Or @custID=0) " & sOut & sOwn & sBrand & sCity &
                      " UNION ALL " &
                     "Select Kode As Storeno, Nama, date_format(@paramTo, '%d %b %Y') AS Eod_Date, 0 AS POS_sub_after_disc, IFNULL((SELECT SUM(Subtotal -discount) " &
                     "FROM cld_sumnosalesdaily WHERE Busidate = @paramTo AND olid = cld_outlet.id),0) AS WebReport_sub_after_disc " &
                     ", (0-(IFNULL((SELECT SUM(Subtotal -discount)FROM cld_sumnosalesdaily WHERE Busidate = @paramTo AND olid = cld_outlet.id),0))) as selisih, IFNULL((SELECT MAX(postdate) " &
                     "FROM cld_last WHERE storeid = cld_outlet.id),'') AS lastsync " &
                     ", 'belum eod' AS status, @paramTo AS EodDate " &
                     "From cld_outlet Where Id Not IN (SELECT Storeid FROM cld_eod_nonsales WHERE eodDate = @paramTo) " &
                     "AND active = 1 AND startdate <= @paramTo AND (cld_outlet.CustID =@custID OR @custID=0) " & sOut & sOwn & sBrand & sCity & ") a order by status,eoddate,storeno ")

            connDb.addParam(dfrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(dTo, "@paramTo", MySqlDbType.DateTime)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)


            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:salesSum")
            Err.Clear()
        End Try

        Return dsRslt
    End Function

    Public Function PosVSWeb_gv(ByVal custID As Integer, ByVal dfrom As Date, ByVal dTo As Date, ByVal Outletstr As String, ByVal iOwn As String, ByVal iBrand As String, ByVal iCity As String, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim dsRslt As New DataSet
        Dim sOut As String = ""
        Dim sBrand As String = ""
        Dim sOwn As String = ""
        Dim sCity As String = ""

        Try
            Dim connDb As New clsDbConn(ServerString)
            If iBrand = "0" Or iBrand = "" Then
                sBrand = ""
            Else
                sBrand = " and cld_outlet.brandID in (" & iBrand & ")"
            End If

            If iOwn = "0" Or iOwn = "" Then
                sOwn = ""
            Else
                sOwn = " and cld_outlet.ownID = " & iOwn
            End If
            If iCity = "0" Or iCity = "" Then
                sCity = ""
            Else
                sCity = " and cld_outlet.cityID in (" & iCity & ")"
            End If

            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If Outletstr = "" Then
                    sOut = " and cld_outlet.ID in (Select Store From cld_userxstore where user = '" & usr & "')"
                Else
                    sOut = " and cld_outlet.kode in (" & Outletstr & ")"
                End If
            Else
                If Outletstr = "" Then
                    sOut = ""
                Else
                    sOut = " and cld_outlet.Kode in (" & Outletstr & ")"
                End If
            End If

            connDb.resetCmd("SELECT Eod_Date 'Sales Date', Storeno 'Store Code', StoreName 'Store Name', POS_sub_after_disc 'Pos Net Sales', WebReport_sub_after_disc 'Web Net Sales', Selisih, DATE_FORMAT(lastsync,'%d-%b-%Y %H:%i:%s') 'Last Sync', Status  FROM ( " &
                        "Select Storeno, cld_outlet.`Nama` 'StoreName' ,date_format(EodDate, '%d-%b-%Y') Eod_Date, FORMAT(IF(subtotal > -1, subtotal, 0), 2,'de_DE') as POS_sub_after_disc, FORMAT(IFNULL((SELECT SUM(Subtotal -discount) " &
                     "FROM cld_sumsalesdaily WHERE Busidate = cld_eod.`eodDate` AND olid = cld_eod.`storeID`),0), 2,'de_DE') AS WebReport_sub_after_disc " &
                     ", FORMAT((IF(subtotal > -1, subtotal, 0)-IFNULL((SELECT SUM(Subtotal -discount) FROM cld_sumsalesdaily WHERE Busidate = cld_eod.`eodDate` AND olid = cld_eod.`storeID`),0)), 0,'de_DE') as selisih " &
                     ", IFNULL((SELECT MAX(postdate) " &
                     "FROM cld_last WHERE storeid = cld_eod.`storeID`),'') AS lastsync " &
                      ", CASE
                            WHEN FORMAT((IF(subtotal > -1, subtotal, 0)-IFNULL((SELECT SUM(Subtotal -discount) FROM cld_sumsalesdaily WHERE Busidate = cld_eod.`eodDate` AND olid = cld_eod.`storeID`),0)),0) = 0 THEN 'OK'
                            WHEN (FORMAT((IF(subtotal > -1, subtotal, 0)-IFNULL((SELECT SUM(Subtotal -discount) FROM cld_sumsalesdaily WHERE Busidate = cld_eod.`eodDate` AND olid = cld_eod.`storeID`),0)),0) <> 0  
                            AND dc = 0) THEN 'Selisih'
                            WHEN (FORMAT((IF(subtotal > -1, subtotal, 0)-IFNULL((SELECT SUM(Subtotal -discount) FROM cld_sumsalesdaily WHERE Busidate = cld_eod.`eodDate` AND olid = cld_eod.`storeID`),0)),0) <> 0  
                            AND dc = 1) THEN 'Dreamcloud'
                            END
                             AS STATUS " &
                             ", eoddate " &
                     "FROM cld_eod INNER JOIN cld_outlet On cld_eod.`storeID` = cld_outlet.`ID` And " &
                     "cld_eod.`CustId` = cld_outlet.`custID` " &
                     "where cld_eod.EodDate >= @paramFrom And cld_eod.EodDate <= @paramTo " &
                     " And (cld_eod.CustID =@custID Or @custID=0) " & sOut & sOwn & sBrand & sCity &
                      " UNION ALL " &
                     "Select Kode As Storeno, Nama, date_format(@paramTo, '%d-%b-%Y') AS Eod_Date, 0 AS POS_sub_after_disc, IFNULL((SELECT SUM(Subtotal -discount) " &
                     "FROM cld_sumsalesdaily WHERE Busidate = @paramTo AND olid = cld_outlet.id),0) AS WebReport_sub_after_disc " &
                     ", (0-(IFNULL((SELECT SUM(Subtotal -discount)FROM cld_sumsalesdaily WHERE Busidate = @paramTo AND olid = cld_outlet.id),0))) as selisih, IFNULL((SELECT MAX(postdate) FROM cld_last WHERE storeid = cld_outlet.id),'') AS lastsync " &
                     ", 'belum eod' AS status, @paramTo AS EodDate " &
                     "From cld_outlet Where Id Not IN (SELECT Storeid FROM cld_eod WHERE eodDate = @paramTo) " &
                     "AND active = 1 AND startdate <= @paramTo AND (cld_outlet.CustID =@custID OR @custID=0) " & sOut & sOwn & sBrand & sCity & ") a order by status,eoddate,storeno ")

            connDb.addParam(dfrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(dTo, "@paramTo", MySqlDbType.DateTime)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)


            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:PosVSWeb_gv")
            Err.Clear()
        End Try

        Return dsRslt
    End Function

    Public Function PosVSWeb_nonsales_gv(ByVal custID As Integer, ByVal dfrom As Date, ByVal dTo As Date, ByVal Outletstr As String, ByVal iOwn As String, ByVal iBrand As String, ByVal iCity As String, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim dsRslt As New DataSet
        Dim sOut As String = ""
        Dim sBrand As String = ""
        Dim sOwn As String = ""
        Dim sCity As String = ""

        Try
            Dim connDb As New clsDbConn(ServerString)
            If iBrand = "0" Or iBrand = "" Then
                sBrand = ""
            Else
                sBrand = " and cld_outlet.brandID in (" & iBrand & ")"
            End If

            If iOwn = "0" Or iOwn = "" Then
                sOwn = ""
            Else
                sOwn = " and cld_outlet.ownID = " & iOwn
            End If
            If iCity = "0" Or iCity = "" Then
                sCity = ""
            Else
                sCity = " and cld_outlet.cityID in (" & iCity & ")"
            End If

            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If Outletstr = "" Then
                    sOut = " and cld_outlet.ID in (Select Store From cld_userxstore where user = '" & usr & "')"
                Else
                    sOut = " and cld_outlet.kode in (" & Outletstr & ")"
                End If
            Else
                If Outletstr = "" Then
                    sOut = ""
                Else
                    sOut = " and cld_outlet.Kode in (" & Outletstr & ")"
                End If
            End If

            connDb.resetCmd("SELECT Eod_Date 'Sales Date', Storeno 'Store Code', StoreName 'Store Name', POS_sub_after_disc 'Pos Net Sales', WebReport_sub_after_disc 'Web Net Sales', Selisih, DATE_FORMAT(lastsync,'%d-%b-%Y %H:%i:%s') 'Last Sync', Status  FROM ( " &
                        "Select Storeno, cld_outlet.`Nama` 'StoreName' ,date_format(EodDate, '%d-%b-%Y') Eod_Date, FORMAT(IF(subtotal > -1, subtotal, 0), 2,'de_DE') as POS_sub_after_disc, FORMAT(IFNULL((SELECT SUM(Subtotal -discount) " &
                     "FROM cld_sumnosalesdaily WHERE Busidate = cld_eod_nonsales.`eodDate` AND olid = cld_eod_nonsales.`storeID`),0), 2,'de_DE') AS WebReport_sub_after_disc " &
                     ", FORMAT((IF(subtotal > -1, subtotal, 0)-IFNULL((SELECT SUM(Subtotal -discount) FROM cld_sumnosalesdaily WHERE Busidate = cld_eod_nonsales.`eodDate` AND olid = cld_eod_nonsales.`storeID`),0)), 0,'de_DE') as selisih " &
                     ", IFNULL((SELECT MAX(postdate) " &
                     "FROM cld_last WHERE storeid = cld_eod_nonsales.`storeID`),'') AS lastsync " &
                      ", CASE
                            WHEN FORMAT((IF(subtotal > -1, subtotal, 0)-IFNULL((SELECT SUM(Subtotal -discount) FROM cld_sumnosalesdaily WHERE Busidate = cld_eod_nonsales.`eodDate` AND olid = cld_eod_nonsales.`storeID`),0)),0) = 0 THEN 'OK'
                            WHEN (FORMAT((IF(subtotal > -1, subtotal, 0)-IFNULL((SELECT SUM(Subtotal -discount) FROM cld_sumnosalesdaily WHERE Busidate = cld_eod_nonsales.`eodDate` AND olid = cld_eod_nonsales.`storeID`),0)),0) <> 0  
                            AND dc = 0) THEN 'Selisih'
                            WHEN (FORMAT((IF(subtotal > -1, subtotal, 0)-IFNULL((SELECT SUM(Subtotal -discount) FROM cld_sumnosalesdaily WHERE Busidate = cld_eod_nonsales.`eodDate` AND olid = cld_eod_nonsales.`storeID`),0)),0) <> 0  
                            AND dc = 1) THEN 'Dreamcloud'
                            END
                             AS STATUS " &
                             ", eoddate " &
                     "FROM cld_eod_nonsales INNER JOIN cld_outlet On cld_eod_nonsales.`storeID` = cld_outlet.`ID` And " &
                     "cld_eod_nonsales.`CustId` = cld_outlet.`custID` " &
                     "where cld_eod_nonsales.EodDate >= @paramFrom And cld_eod_nonsales.EodDate <= @paramTo " &
                     " And (cld_eod_nonsales.CustID =@custID Or @custID=0) " & sOut & sOwn & sBrand & sCity &
                      " UNION ALL " &
                     "Select Kode As Storeno, Nama, date_format(@paramTo, '%d-%b-%Y') AS Eod_Date, 0 AS POS_sub_after_disc, IFNULL((SELECT SUM(Subtotal -discount) " &
                     "FROM cld_sumnosalesdaily WHERE Busidate = @paramTo AND olid = cld_outlet.id),0) AS WebReport_sub_after_disc " &
                     ", (0-(IFNULL((SELECT SUM(Subtotal -discount)FROM cld_sumnosalesdaily WHERE Busidate = @paramTo AND olid = cld_outlet.id),0))) as selisih, IFNULL((SELECT MAX(postdate) " &
                     "FROM cld_last WHERE storeid = cld_outlet.id),'') AS lastsync " &
                     ", 'belum eod' AS status, @paramTo AS EodDate " &
                     "From cld_outlet Where Id Not IN (SELECT Storeid FROM cld_eod_nonsales WHERE eodDate = @paramTo) " &
                     "AND active = 1 AND startdate <= @paramTo AND (cld_outlet.CustID =@custID OR @custID=0) " & sOut & sOwn & sBrand & sCity & ") a order by status,eoddate,storeno ")

            connDb.addParam(dfrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(dTo, "@paramTo", MySqlDbType.DateTime)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)


            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:PosVSWeb_gv")
            Err.Clear()
        End Try

        Return dsRslt
    End Function

    Public Function DayToDay(ByVal Tgl1 As Date, ByVal outletNo As String, ByVal custID As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet

        Dim dsRslt As New DataSet
        Dim sStrData As String = ""

        Try

            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                sStrData = "SELECT csd.busidate " &
                                            ", IFNULL(SUM(csd.Subtotal - csd.Discount),0) AS sales " &
                                            ", IFNULL((SELECT SUM(Subtotal-Discount ) FROM cld_sumsalesdaily WHERE (olNumber=@outlet AND @outlet <> '') or (@outlet='' AND (olid IN (Select Store from cld_userxstore where user = '" & usr & "'))) AND (CustID=@custID OR @custID=0) AND cld_sumsalesdaily.BusiDate = DATE_ADD( csd.BusiDate, INTERVAL -1 year)),0) salesBefore " &
                                         "FROM cld_sumsalesdaily AS csd " &
                                         "WHERE csd.busiDate >='" & Tgl1.Date.ToString("yyyy-MM-01 00:00:00") & "' And csd.busiDate <=(SELECT LAST_DAY('" & Tgl1.Date.ToString("yyyy-MM-dd 00:00:00") & "')) " &
                                            "AND ((csd.olNumber=@outlet AND @outlet <> '') or (@outlet='' AND (csd.olid IN (Select Store from cld_userxstore where user = '" & usr & "'))) ) " &
                                            "AND (csd.CustID=@custID OR @custID=0) " &
                                         "GROUP BY csd.busidate " &
                                         "ORDER BY csd.busidate"
            Else
                sStrData = "SELECT csd.busidate " &
                                             ", IFNULL(SUM(csd.Subtotal - csd.Discount),0) AS sales " &
                                             ", IFNULL((SELECT SUM(Subtotal-Discount ) FROM cld_sumsalesdaily WHERE (olNumber=@outlet or @outlet='') AND (CustID=@custID OR @custID=0) AND cld_sumsalesdaily.BusiDate = DATE_ADD( csd.BusiDate, INTERVAL -1 year)),0) salesBefore " &
                                          "FROM cld_sumsalesdaily AS csd " &
                                          "WHERE csd.busiDate >='" & Tgl1.Date.ToString("yyyy-MM-01 00:00:00") & "' And csd.busiDate <=(SELECT LAST_DAY('" & Tgl1.Date.ToString("yyyy-MM-dd 00:00:00") & "')) " &
                                             "AND (csd.olNumber=@outlet or @outlet='') " &
                                             "AND (csd.CustID=@custID OR @custID=0) " &
                                          "GROUP BY csd.busidate " &
                                          "ORDER BY csd.busidate"
            End If


            Dim c_Cmd As New clsDbo(sStrData)
            'c_Cmd.addParam(Month(busiDate), "@bulan", MySqlDbType.Int32)
            'c_Cmd.addParam(Year(busiDate), "@tahun", MySqlDbType.Int32)
            c_Cmd.addParam(outletNo, "@outlet", MySqlDbType.VarChar)
            c_Cmd.addParam(custID, "@custID", MySqlDbType.Int32)

            dsRslt = c_Cmd.selectDataset(True)

            c_Cmd.dispose()
            c_Cmd = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:dailySales")
            Err.Clear()
        End Try

        Return dsRslt


    End Function
    Public Function WeekOfMonth(ByVal Tgl1 As Date, ByVal custID As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet

        Dim dsRslt As New DataSet
        Dim sStrData As String = ""

        Try



            sStrData = "SELECT WEEK(busidate) weekofmonth " &
                           "FROM cld_sumsalesdaily AS csd " &
                           "WHERE busiDate >='" & Tgl1.Date.ToString("yyyy-MM-01 00:00:00") & "' And busiDate <=(SELECT LAST_DAY('" & Tgl1.Date.ToString("yyyy-MM-dd 00:00:00") & "')) " &
                           "GROUP BY WEEK(busidate) ORDER BY busidate,DAYOFWEEK(busidate)"


            Dim c_Cmd As New clsDbo(sStrData)
            'c_Cmd.addParam(Month(busiDate), "@bulan", MySqlDbType.Int32)
            'c_Cmd.addParam(Year(busiDate), "@tahun", MySqlDbType.Int32)


            dsRslt = c_Cmd.selectDataset(True)

            c_Cmd.dispose()
            c_Cmd = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsPerports:WeekOfMonth")
            Err.Clear()
        End Try

        Return dsRslt


    End Function
    Public Function WeeklySales2(ByVal Tgl1 As Date, ByVal outletNo As String, ByVal custID As Integer, ByVal usr As String, ByVal modpriv As Integer, ByVal WeekOfMonth As String) As DataSet

        Dim dsRslt As New DataSet
        Dim sStrData As String = ""
        Dim weekofMonth2 As String = ""

        If WeekOfMonth = "" Then
            weekofMonth2 = "month(busidate) =@bulan AND "
        Else

            weekofMonth2 = "Week(Busidate) in (" & WeekOfMonth & ") AND "
        End If


        Try

            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                sStrData = "SELECT DAYOFWEEK(busiDate) AS hari1 " &
                                             ", LEFT(DATE_FORMAT(busiDate,'%W'),3) AS hari " &
                                             ", SUM(Subtotal - Discount) AS sales " &
                                          "FROM cld_sumsalesdaily " &
                                          "WHERE busiDate >='" & Tgl1.Date.ToString("yyyy-MM-01 00:00:00") & "' And busiDate <=(SELECT LAST_DAY('" & Tgl1.Date.ToString("yyyy-MM-dd 00:00:00") & "')) " &
                                             "AND ((olNumber=@outlet AND @outlet <> '') or (@outlet='' AND (olid IN (Select Store from cld_userxstore where user = '" & usr & "'))) ) " &
                                             "AND (CustID=@custID OR @custID=0) " &
                                          "GROUP BY DAYOFWEEK(busiDate), DATE_FORMAT(busiDate,'%W') " &
                                          "ORDER BY DAYOFWEEK(busiDate)"
            Else

                'sStrData = "SELECT DAYOFWEEK(busiDate) AS hari1 " & _
                '                         ", LEFT(DATE_FORMAT(busiDate,'%W'),3) AS hari " & _
                '                         ", SUM(Subtotal - Discount) AS sales " & _
                '                      "FROM cld_sumsalesdaily " & _
                '                      "WHERE MONTH(BusiDate) =@bulan AND YEAR(BusiDate) =@tahun " & _
                '                         "AND (olNumber=@outlet or @outlet='') " & _
                '                         "AND (CustID=@custID OR @custID=0) " & _
                '                      "GROUP BY DAYOFWEEK(busiDate), DATE_FORMAT(busiDate,'%W') " & _
                '                      "ORDER BY DAYOFWEEK(busiDate)"

                sStrData = "SELECT WEEK(busidate) weekofmonth,LEFT(DATE_FORMAT(busiDate,'%W'),3) AS hari,busidate,IFNULL(SUM(subtotal-discount),0) sales " &
                                     "FROM cld_sumsalesdaily " &
                                     "WHERE " & weekofMonth2 & " YEAR(BusiDate) =@tahun " &
                                        "AND (olNumber=@outlet or @outlet='') " &
                                        "AND (CustID=@custID OR @custID=0) " &
                                     "GROUP BY busiDate, WEEK(Busidate) ORDER BY busidate,DAYOFWEEK(busidate)"

                'sStrData = "SELECT WEEK(busidate) weekofmonth, (SELECT SUM(subtotal-discount) FROM cld_sumsalesdaily WHERE MONTH(busidate)=@bulan AND YEAR(busidate)=@tahun AND DAYNAME(busidate)='Sunday' AND WEEK(cld_sumsalesdaily.`BusiDate`) = WEEK(csd.`BusiDate`) AND (olNumber=@outlet or @outlet='') AND (CustID=@custID OR @custID=0) GROUP BY WEEK(cld_sumsalesdaily.Busidate) " & _
                '           " ORDER BY busidate,DAYOFWEEK(busidate) LIMIT 1) salesSunday, " & _
                '           "(SELECT SUM(subtotal-discount) FROM cld_sumsalesdaily WHERE MONTH(busidate)=@bulan AND YEAR(busidate)=@tahun AND DAYNAME(busidate)='Monday' AND WEEK(cld_sumsalesdaily.`BusiDate`) = WEEK(csd.`BusiDate`) AND (olNumber=@outlet or @outlet='') AND (CustID=@custID OR @custID=0) GROUP BY WEEK(cld_sumsalesdaily.Busidate) " & _
                '           " ORDER BY busidate,DAYOFWEEK(busidate) LIMIT 1) salesMon, " & _
                '           "(SELECT SUM(subtotal-discount) FROM cld_sumsalesdaily WHERE MONTH(busidate)=@bulan AND YEAR(busidate)=@tahun AND DAYNAME(busidate)='Tuesday' AND WEEK(cld_sumsalesdaily.`BusiDate`) = WEEK(csd.`BusiDate`) AND (olNumber=@outlet or @outlet='') AND (CustID=@custID OR @custID=0) GROUP BY WEEK(cld_sumsalesdaily.Busidate) " & _
                '           "ORDER BY busidate,DAYOFWEEK(busidate) LIMIT 1) salesTue, " & _
                '           "(SELECT SUM(subtotal-discount) FROM cld_sumsalesdaily WHERE MONTH(busidate)=@bulan AND YEAR(busidate)=@tahun AND DAYNAME(busidate)='Wednesday' AND WEEK(cld_sumsalesdaily.`BusiDate`) = WEEK(csd.`BusiDate`) AND (olNumber=@outlet or @outlet='') AND (CustID=@custID OR @custID=0) GROUP BY WEEK(cld_sumsalesdaily.Busidate) " & _
                '           "ORDER BY busidate,DAYOFWEEK(busidate) LIMIT 1) salesWed, " & _
                '           "(SELECT SUM(subtotal-discount) FROM cld_sumsalesdaily WHERE MONTH(busidate)=@bulan AND YEAR(busidate)=@tahun AND DAYNAME(busidate)='Thursday' AND WEEK(cld_sumsalesdaily.`BusiDate`) = WEEK(csd.`BusiDate`) AND (olNumber=@outlet or @outlet='') AND (CustID=@custID OR @custID=0) GROUP BY WEEK(cld_sumsalesdaily.Busidate) " & _
                '           "ORDER BY busidate,DAYOFWEEK(busidate) LIMIT 1) salesThu, " & _
                '           "(SELECT SUM(subtotal-discount) FROM cld_sumsalesdaily WHERE MONTH(busidate)=@bulan AND YEAR(busidate)=@tahun AND DAYNAME(busidate)='Friday' AND WEEK(cld_sumsalesdaily.`BusiDate`) = WEEK(csd.`BusiDate`) AND (olNumber=@outlet or @outlet='') AND (CustID=@custID OR @custID=0) GROUP BY WEEK(cld_sumsalesdaily.Busidate) " & _
                '           "ORDER BY busidate,DAYOFWEEK(busidate) LIMIT 1) salesFri, " & _
                '           " (SELECT SUM(subtotal-discount) FROM cld_sumsalesdaily WHERE MONTH(busidate)=@bulan AND YEAR(busidate)=@tahun AND DAYNAME(busidate)='Saturday' AND WEEK(cld_sumsalesdaily.`BusiDate`) = WEEK(csd.`BusiDate`) AND (olNumber=@outlet or @outlet='') AND (CustID=@custID OR @custID=0) GROUP BY WEEK(cld_sumsalesdaily.Busidate) " & _
                '           " ORDER BY busidate,DAYOFWEEK(busidate) LIMIT 1) salesSat " & _
                '           "FROM cld_sumsalesdaily AS csd " & _
                '           "WHERE MONTH(BusiDate) =@bulan AND YEAR(BusiDate) =@tahun " & _
                '           "AND (olNumber=@outlet or @outlet='') " & _
                '           "AND (CustID=@custID OR @custID=0) " & _
                '           "GROUP BY WEEK(busidate) ORDER BY busidate,DAYOFWEEK(busidate)"

                'sStrData = "SELECT WEEK(busidate) weekofmonth, (SELECT SUM(subtotal-discount) FROM cld_sumsalesdaily WHERE " & weekofMonth2 & " YEAR(busidate)=@tahun AND DAYOFWEEK(busidate)=1 AND WEEK(cld_sumsalesdaily.`BusiDate`) = WEEK(csd.`BusiDate`) AND (olNumber=@outlet or @outlet='') AND (CustID=@custID OR @custID=0) GROUP BY WEEK(cld_sumsalesdaily.Busidate) " & _
                '           " ORDER BY busidate,DAYOFWEEK(busidate) LIMIT 1) salesSunday, " & _
                '           "(SELECT SUM(subtotal-discount) FROM cld_sumsalesdaily WHERE " & weekofMonth2 & " YEAR(busidate)=@tahun AND DAYOFWEEK(busidate)=2 AND WEEK(cld_sumsalesdaily.`BusiDate`) = WEEK(csd.`BusiDate`) AND (olNumber=@outlet or @outlet='') AND (CustID=@custID OR @custID=0) GROUP BY WEEK(cld_sumsalesdaily.Busidate) " & _
                '           " ORDER BY busidate,DAYOFWEEK(busidate) LIMIT 1) salesMon, " & _
                '           "(SELECT SUM(subtotal-discount) FROM cld_sumsalesdaily WHERE " & weekofMonth2 & " YEAR(busidate)=@tahun AND DAYOFWEEK(busidate)=3 AND WEEK(cld_sumsalesdaily.`BusiDate`) = WEEK(csd.`BusiDate`) AND (olNumber=@outlet or @outlet='') AND (CustID=@custID OR @custID=0) GROUP BY WEEK(cld_sumsalesdaily.Busidate) " & _
                '           "ORDER BY busidate,DAYOFWEEK(busidate) LIMIT 1) salesTue, " & _
                '           "(SELECT SUM(subtotal-discount) FROM cld_sumsalesdaily WHERE " & weekofMonth2 & " YEAR(busidate)=@tahun AND DAYOFWEEK(busidate)=4 AND WEEK(cld_sumsalesdaily.`BusiDate`) = WEEK(csd.`BusiDate`) AND (olNumber=@outlet or @outlet='') AND (CustID=@custID OR @custID=0) GROUP BY WEEK(cld_sumsalesdaily.Busidate) " & _
                '           "ORDER BY busidate,DAYOFWEEK(busidate) LIMIT 1) salesWed, " & _
                '           "(SELECT SUM(subtotal-discount) FROM cld_sumsalesdaily WHERE " & weekofMonth2 & " YEAR(busidate)=@tahun AND DAYOFWEEK(busidate)=5 AND WEEK(cld_sumsalesdaily.`BusiDate`) = WEEK(csd.`BusiDate`) AND (olNumber=@outlet or @outlet='') AND (CustID=@custID OR @custID=0) GROUP BY WEEK(cld_sumsalesdaily.Busidate) " & _
                '           "ORDER BY busidate,DAYOFWEEK(busidate) LIMIT 1) salesThu, " & _
                '           "(SELECT SUM(subtotal-discount) FROM cld_sumsalesdaily WHERE " & weekofMonth2 & " YEAR(busidate)=@tahun AND DAYOFWEEK(busidate)=6 AND WEEK(cld_sumsalesdaily.`BusiDate`) = WEEK(csd.`BusiDate`) AND (olNumber=@outlet or @outlet='') AND (CustID=@custID OR @custID=0) GROUP BY WEEK(cld_sumsalesdaily.Busidate) " & _
                '           "ORDER BY busidate,DAYOFWEEK(busidate) LIMIT 1) salesFri, " & _
                '           " (SELECT SUM(subtotal-discount) FROM cld_sumsalesdaily WHERE " & weekofMonth2 & " YEAR(busidate)=@tahun AND DAYOFWEEK(busidate)=7 AND WEEK(cld_sumsalesdaily.`BusiDate`) = WEEK(csd.`BusiDate`) AND (olNumber=@outlet or @outlet='') AND (CustID=@custID OR @custID=0) GROUP BY WEEK(cld_sumsalesdaily.Busidate) " & _
                '           " ORDER BY busidate,DAYOFWEEK(busidate) LIMIT 1) salesSat " & _
                '           "FROM cld_sumsalesdaily AS csd " & _
                '           "WHERE MONTH(BusiDate) =@bulan AND YEAR(BusiDate) =@tahun " & _
                '           "AND (olNumber=@outlet or @outlet='') " & _
                '           "AND (CustID=@custID OR @custID=0) " & _
                '           "GROUP BY WEEK(busidate) ORDER BY busidate,DAYOFWEEK(busidate)"

                '     sStrData = "SELECT WEEK(busidate) weekofmonth, (SUM(subtotal-discount) FROM cld_sumsalesdaily WHERE YEAR(busidate)=@tahun " & weekofMonth2 & " AND (olNumber=@outlet or @outlet='') AND (CustID=@custID OR @custID=0) GROUP BY WEEK(Busidate) ORDER BY busidate,DAYOFWEEK(busidate)) SalesSunday " & _
                '                  ", (SUM(subtotal-discount) FROM cld_sumsalesdaily WHERE YEAR(busidate)=@tahun " & weekofMonth2 & " AND (olNumber=@outlet or @outlet='') AND (CustID=@custID OR @custID=0) GROUP BY WEEK(Busidate) ORDER BY busidate,DAYOFWEEK(busidate)) SalesMonday " & _
                '", (SUM(subtotal-discount) FROM cld_sumsalesdaily WHERE YEAR(busidate)=@tahun " & weekofMonth2 & " AND (olNumber=@outlet or @outlet='') AND (CustID=@custID OR @custID=0) GROUP BY WEEK(Busidate) ORDER BY busidate,DAYOFWEEK(busidate)) SalesTuesday " & _
                '", (SUM(subtotal-discount) FROM cld_sumsalesdaily WHERE YEAR(busidate)=@tahun " & weekofMonth2 & " AND (olNumber=@outlet or @outlet='') AND (CustID=@custID OR @custID=0) GROUP BY WEEK(Busidate) ORDER BY busidate,DAYOFWEEK(busidate)) SalesWednesday " & _
                '", (SUM(subtotal-discount) FROM cld_sumsalesdaily WHERE YEAR(busidate)=@tahun " & weekofMonth2 & " AND (olNumber=@outlet or @outlet='') AND (CustID=@custID OR @custID=0) GROUP BY WEEK(Busidate) ORDER BY busidate,DAYOFWEEK(busidate)) SalesThursday " & _
                '", (SUM(subtotal-discount) FROM cld_sumsalesdaily WHERE YEAR(busidate)=@tahun " & weekofMonth2 & " AND (olNumber=@outlet or @outlet='') AND (CustID=@custID OR @custID=0) GROUP BY WEEK(Busidate) ORDER BY busidate,DAYOFWEEK(busidate)) SalesFriday " & _
                '", (SUM(subtotal-discount) FROM cld_sumsalesdaily WHERE YEAR(busidate)=@tahun " & weekofMonth2 & " AND (olNumber=@outlet or @outlet='') AND (CustID=@custID OR @custID=0) GROUP BY WEEK(Busidate) ORDER BY busidate,DAYOFWEEK(busidate)) SalesSaturday " & _
                '"FROM cld_sumsalesdaily AS csd " & _
                '                  "WHERE MONTH(BusiDate) =@bulan AND YEAR(BusiDate) =@tahun " & _
                '                  "AND (olNumber=@outlet or @outlet='') " & _
                '                  "AND (CustID=@custID OR @custID=0) " & _
                '                  "GROUP BY WEEK(busidate) ORDER BY busidate,DAYOFWEEK(busidate) "
            End If

            Dim c_Cmd As New clsDbo(sStrData)
            'c_Cmd.addParam(WeekOfMonth, "@weekofmonth", MySqlDbType.VarChar)
            c_Cmd.addParam(Month(Tgl1), "@bulan", MySqlDbType.Int32)
            c_Cmd.addParam(Year(Tgl1), "@tahun", MySqlDbType.Int32)
            c_Cmd.addParam(outletNo, "@outlet", MySqlDbType.VarChar)
            c_Cmd.addParam(custID, "@custID", MySqlDbType.Int32)

            dsRslt = c_Cmd.selectDataset(True)

            c_Cmd.dispose()
            c_Cmd = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsPerports:WeeklySales")
            Err.Clear()
        End Try

        Return dsRslt


    End Function
    Public Function getds_viewOutlet(ByVal iown As String, ByVal ibrand As String, ByVal iCity As String, ByVal CustID As Integer, ByVal sText As String, ByVal sUser As String, ByVal ModPriv As Integer) As DataSet

        Dim ds As New DataSet

        Dim sOwn As String = ""
        Dim sBrand As String = ""
        Dim sCity As String = ""
        Dim sout As String = ""

        Try
            If ibrand = "0" Or ibrand = "" Then
                sBrand = ""
            Else
                sBrand = " and brandID in (" & ibrand & ")"
            End If

            If iown = "0" Or iown = "" Then
                sOwn = ""
            Else
                sOwn = " and ownID = " & iown
            End If
            If iCity = "0" Or iCity = "" Then
                sCity = ""
            Else
                sCity = " and cityID in (" & iCity & ")"
            End If

            If ModPriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then

                sout = " and Id in (Select Store From cld_userxstore where user = '" & sUser & "')"

            End If

            Dim c_Cmd As New clsDbo("Select * from cld_outlet where custid = " & CustID & sOwn & sBrand & sCity & sout &
                " and ( cld_outlet.Kode like  '%" & sText & "%' Or cld_outlet.nama like '%" & sText & "%' Or cld_outlet.Kode2 like '%" & sText & "%' ) order by Kode")

            ds = c_Cmd.selectDataset()

            c_Cmd.dispose()
            c_Cmd = Nothing

        Catch ex As Exception
            errLog(Err.Description, "clsReport:getds_viewOutlet")
            Err.Clear()
        End Try

        Return ds
    End Function

    Public Function getds_view_Outxuser(ByVal iown As String, ByVal CustID As Integer, ByVal sText As String, ByVal sUser As String, ByVal ModPriv As Integer) As DataSet

        Dim ds As New DataSet

        Dim sOwn As String = ""
        Dim sBrand As String = ""
        Dim sCity As String = ""
        Dim sout As String = ""

        Try


            If iown = "0" Or iown = "" Then
                sOwn = ""
            Else
                sOwn = " and ownID = " & iown
            End If


            If ModPriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then

                sout = " and Id in (Select Store From cld_userxstore where user = '" & sUser & "')"

            End If

            Dim c_Cmd As New clsDbo("Select * from cld_outlet where custid = " & CustID & sOwn & sBrand & sCity & sout &
                " and ( cld_outlet.Kode like  '%" & sText & "%' Or cld_outlet.nama like '%" & sText & "%' Or cld_outlet.Kode2 like '%" & sText & "%' ) and id not IN (SELECT store FROM cld_userxstore WHERE USER = '" & sUser & "') order by Kode")

            ds = c_Cmd.selectDataset()

            c_Cmd.dispose()
            c_Cmd = Nothing

        Catch ex As Exception
            errLog(Err.Description, "clsReport:getds_viewOutlet")
            Err.Clear()
        End Try

        Return ds
    End Function

    Public Function getds_ViewBrand(ByVal CustID As Integer, ByVal sSearch As String, Optional ByVal withall As Boolean = True) As DataSet

        Dim ds As New DataSet
        Dim strfilter As String = ""


        Try

            If withall Then
                strfilter = "SELECT '0' AS ID, 'All' AS Nama, 'All' AS name3 UNION ALL " &
                      "Select ID, Name Nama,name3 from cld_brand where custid = " & CustID & " and ( Name like '%" & sSearch & "%' or Name3 like '%" & sSearch & "%' )"
            Else
                strfilter = "Select ID, Name Nama, name3 from cld_brand where custid = " & CustID & " and ( Name like '%" & sSearch & "%' or Name3 like '%" & sSearch & "%' )"
            End If

            Dim c_Cmd As New clsDbo(strfilter)

            ds = c_Cmd.selectDataset()

            c_Cmd.dispose()
            c_Cmd = Nothing

        Catch ex As Exception
            errLog(Err.Description, "clsReport:getds_ViewBrand")
            Err.Clear()
        End Try

        Return ds
    End Function
    Public Function getds_ViewCity(ByVal CustID As Integer, ByVal sSearch As String) As DataSet

        Dim ds As New DataSet
        Dim strfilter As String = ""

        Try
            strfilter = "Select ID, Name nama,kota from cld_city where custid = " & CustID & " and ( Name like '%" & sSearch & "%' or kota like '%" & sSearch & "%' )"

            Dim c_Cmd As New clsDbo(strfilter)

            ds = c_Cmd.selectDataset()

            c_Cmd.dispose()
            c_Cmd = Nothing

        Catch ex As Exception
            errLog(Err.Description, "clsReport:getds_ViewCity")
            Err.Clear()
        End Try


        Return ds


    End Function

    Public Function SelectDataTable_DailySales(ByVal custID As Integer, ByVal dfrom As Date, ByVal dTo As Date, ByVal Outletstr As String, ByVal iOwn As String, ByVal iBrand As String, ByVal iCity As String, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim dsRslt As New DataSet
        Dim sOut As String = ""
        Dim sBrand As String = ""
        Dim sOwn As String = ""
        Dim sCity As String = ""

        Try
            Dim connDb As New clsDbConn(ServerString)
            If iBrand = "0" Or iBrand = "" Then
                sBrand = ""
            Else
                sBrand = " and o.brandID in (" & iBrand & ")"
            End If

            If iOwn = "0" Or iOwn = "" Then
                sOwn = ""
            Else
                sOwn = " and o.ownID = " & iOwn
            End If
            If iCity = "0" Or iCity = "" Then
                sCity = ""
            Else
                sCity = " and o.cityID in (" & iCity & ")"
            End If

            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If Outletstr = "" Then
                    sOut = " and o.ID in (Select Store From cld_userxstore where user = '" & usr & "')"
                Else
                    sOut = " and o.kode in (" & Outletstr & ")"
                End If
            Else
                If Outletstr = "" Then
                    sOut = ""
                Else
                    sOut = " and o.Kode in (" & Outletstr & ")"
                End If
            End If

            connDb.resetCmd("SELECT  o.kode 'Store Code', o.nama 'Store Name', DATE_FORMAT(smd.BusiDate,'%d-%b-%Y') 'Sales Date' , sum(smd.Subtotal - smd.Discount) 'Net Sales', 'actual' Type, MONTH(smd.BusiDate) Wpb, " &
               "CASE  MONTH(smd.BusiDate) " &
               "WHEN 1 THEN 'Q1' " &
               "WHEN 2 THEN 'Q1' " &
               "WHEN 3 THEN 'Q1' " &
               "WHEN 4 THEN 'Q2' " &
               "WHEN 5 THEN 'Q2' " &
               "WHEN 6 THEN 'Q2' " &
               "WHEN 7 THEN 'Q3' " &
               "WHEN 8 THEN 'Q3' " &
               "WHEN 9 THEN 'Q3' " &
               "ELSE 'Q4' " &
               "END AS Quarter " &
               ",YEAR(smd.BusiDate) Year, brd.Name3 Brand " &
               "FROM cld_sumsalesdaily smd INNER JOIN cld_outlet o " &
               "ON smd.CustID = o.custID AND smd.olId = o.id " &
               "INNER JOIN cld_brand brd " &
               "ON brd.CustID = o.custID AND brd.ID = o.brandID " &
               "Where smd.custid = " & custID & " and BusiDate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' and BusiDate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "'" & sOut & sOwn & sBrand & sCity &
               " Group by o.Kode2, smd.BusiDate, brd.Name")

            'connDb.resetCmd("SELECT  o.kode 'Store No', o.nama 'Store Name', smd.BusiDate AS Date, sum(smd.Subtotal - smd.Discount) AS Amount, 'actual' Type, MONTH(smd.BusiDate) wpb, " &
            '   "CASE  MONTH(smd.BusiDate) " &
            '   "WHEN 1 THEN 'Q1' " &
            '   "WHEN 2 THEN 'Q1' " &
            '   "WHEN 3 THEN 'Q1' " &
            '   "WHEN 4 THEN 'Q2' " &
            '   "WHEN 5 THEN 'Q2' " &
            '   "WHEN 6 THEN 'Q2' " &
            '   "WHEN 7 THEN 'Q3' " &
            '   "WHEN 8 THEN 'Q3' " &
            '   "WHEN 9 THEN 'Q3' " &
            '   "ELSE 'Q4' " &
            '   "END AS quarter " &
            '   ",YEAR(smd.BusiDate) year, brd.Name brand " &
            '   "FROM cld_sumsalesdaily smd INNER JOIN cld_outlet o " &
            '   "ON smd.CustID = o.custID AND smd.olId = o.id " &
            '   "INNER JOIN cld_brand brd " &
            '   "ON brd.CustID = o.custID AND brd.ID = o.brandID " &
            '   "Where smd.custid = " & custID & " and BusiDate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' and BusiDate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "'" & sOut & sOwn & sBrand & sCity &
            '   " Group by o.Kode2, smd.BusiDate, brd.Name")

            connDb.addParam(dfrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(dTo, "@paramTo", MySqlDbType.DateTime)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)


            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:SelectDataTable_DailySales")
            Err.Clear()
        End Try

        Return dsRslt
    End Function

    Public Function SelectDataTable_DailySales_gv(ByVal custID As Integer, ByVal dfrom As Date, ByVal dTo As Date, ByVal Outletstr As String, ByVal iOwn As String, ByVal iBrand As String, ByVal iCity As String, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim dsRslt As New DataSet
        Dim sOut As String = ""
        Dim sBrand As String = ""
        Dim sOwn As String = ""
        Dim sCity As String = ""

        Try
            Dim connDb As New clsDbConn(ServerString)
            If iBrand = "0" Or iBrand = "" Then
                sBrand = ""
            Else
                sBrand = " and o.brandID in (" & iBrand & ")"
            End If

            If iOwn = "0" Or iOwn = "" Then
                sOwn = ""
            Else
                sOwn = " and o.ownID = " & iOwn
            End If
            If iCity = "0" Or iCity = "" Then
                sCity = ""
            Else
                sCity = " and o.cityID in (" & iCity & ")"
            End If

            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If Outletstr = "" Then
                    sOut = " and o.ID in (Select Store From cld_userxstore where user = '" & usr & "')"
                Else
                    sOut = " and o.kode in (" & Outletstr & ")"
                End If
            Else
                If Outletstr = "" Then
                    sOut = ""
                Else
                    sOut = " and o.Kode in (" & Outletstr & ")"
                End If
            End If

            connDb.resetCmd("SELECT  o.kode 'Store Code', o.nama 'Store Name', DATE_FORMAT(smd.BusiDate,'%d-%b-%Y') 'Sales Date' , FORMAT(sum(smd.Subtotal - smd.Discount), 2,'de_DE') 'Net Sales', 'actual' Type, MONTH(smd.BusiDate) Wpb, " &
               "CASE  MONTH(smd.BusiDate) " &
               "WHEN 1 THEN 'Q1' " &
               "WHEN 2 THEN 'Q1' " &
               "WHEN 3 THEN 'Q1' " &
               "WHEN 4 THEN 'Q2' " &
               "WHEN 5 THEN 'Q2' " &
               "WHEN 6 THEN 'Q2' " &
               "WHEN 7 THEN 'Q3' " &
               "WHEN 8 THEN 'Q3' " &
               "WHEN 9 THEN 'Q3' " &
               "ELSE 'Q4' " &
               "END AS Quarter " &
               ",YEAR(smd.BusiDate) Year, brd.Name3 Brand " &
               "FROM cld_sumsalesdaily smd INNER JOIN cld_outlet o " &
               "ON smd.CustID = o.custID AND smd.olId = o.id " &
               "INNER JOIN cld_brand brd " &
               "ON brd.CustID = o.custID AND brd.ID = o.brandID " &
               "Where smd.custid = " & custID & " and BusiDate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' and BusiDate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "'" & sOut & sOwn & sBrand & sCity &
               " Group by o.Kode2, smd.BusiDate, brd.Name")

            connDb.addParam(dfrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(dTo, "@paramTo", MySqlDbType.DateTime)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)


            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:SelectDataTable_DailySales")
            Err.Clear()
        End Try

        Return dsRslt
    End Function
    Public Function SelectDataTable_MonthlySales(ByVal custID As Integer, ByVal dfrom As Date, ByVal Outletstr As String, ByVal iOwn As String, ByVal iBrand As String, ByVal iCity As String, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim dsRslt As New DataSet
        Dim sOut As String = ""
        Dim sBrand As String = ""
        Dim sOwn As String = ""
        Dim sCity As String = ""

        Try
            Dim connDb As New clsDbConn(ServerString)
            If iBrand = "0" Or iBrand = "" Then
                sBrand = ""
            Else
                sBrand = " and o.brandID in (" & iBrand & ")"
            End If

            If iOwn = "0" Or iOwn = "" Then
                sOwn = ""
            Else
                sOwn = " and o.ownID = " & iOwn
            End If
            If iCity = "0" Or iCity = "" Then
                sCity = ""
            Else
                sCity = " and o.cityID in (" & iCity & ")"
            End If

            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If Outletstr = "" Then
                    sOut = " and o.ID in (Select Store From cld_userxstore where user = '" & usr & "')"
                Else
                    sOut = " and o.kode in (" & Outletstr & ")"
                End If
            Else
                If Outletstr = "" Then
                    sOut = ""
                Else
                    sOut = " and o.Kode in (" & Outletstr & ")"
                End If
            End If

            connDb.resetCmd("SELECT date_format(smd.BusiDate,'%M') AS Month, o.Kode AS 'Store Code', o.nama 'Store Name', Sum(smd.Subtotal - smd.Discount) 'Net Sales' " &
               "FROM cld_sumsalesdaily smd INNER JOIN cld_outlet o " &
               "ON smd.CustID = o.custID AND smd.olId = o.id " &
               "INNER JOIN cld_brand brd " &
               "ON brd.CustID = o.custID AND brd.ID = o.brandID " &
               "INNER JOIN cld_own own " &
               "ON own.CustID = o.custID AND own.ID = o.ownid " &
               "INNER JOIN cld_city ct " &
               "ON ct.CustID = o.custID AND ct.ID = o.cityid " &
               "INNER JOIN cld_area ar " &
               "ON ar.CustID = ct.custID AND ar.ID = ct.AreaID " &
               "Where busiDate >='" & dfrom.Date.ToString("yyyy-MM-01 00:00:00") & "' And busiDate <=(SELECT LAST_DAY('" & dfrom.Date.ToString("yyyy-MM-dd 00:00:00") & "')) and smd.custid = " & custID & sOut & sOwn & sBrand &
               " Group by o.Kode2, month(busidate), brd.Name order by month(busidate),o.kode, brd.Name")

            'connDb.resetCmd("SELECT Sum(smd.Subtotal - smd.Discount) 'Net Sales', o.Kode AS 'Store Code',o.nama 'Store Name', date_format(smd.BusiDate,'%M') AS Month,  'Actual' Type, MONTH(smd.BusiDate) WPB, " &
            '   "CASE  MONTH(smd.BusiDate) " &
            '   "WHEN 1 THEN 'Q1' " &
            '   "WHEN 2 THEN 'Q1' " &
            '   "WHEN 3 THEN 'Q1' " &
            '   "WHEN 4 THEN 'Q2' " &
            '   "WHEN 5 THEN 'Q2' " &
            '   "WHEN 6 THEN 'Q2' " &
            '   "WHEN 7 THEN 'Q3' " &
            '   "WHEN 8 THEN 'Q3' " &
            '   "WHEN 9 THEN 'Q3' " &
            '   "ELSE 'Q4' " &
            '   "END AS Quarter " &
            '   ",YEAR(smd.BusiDate) Year, brd.Name3 Brand , ct.kota City, own.Name Owner,  ar.Name Area " &
            '   "FROM cld_sumsales smd INNER JOIN cld_outlet o " &
            '   "ON smd.CustID = o.custID AND smd.olId = o.id " &
            '   "INNER JOIN cld_brand brd " &
            '   "ON brd.CustID = o.custID AND brd.ID = o.brandID " &
            '   "INNER JOIN cld_own own " &
            '   "ON own.CustID = o.custID AND own.ID = o.ownid " &
            '   "INNER JOIN cld_city ct " &
            '   "ON ct.CustID = o.custID AND ct.ID = o.cityid " &
            '   "INNER JOIN cld_area ar " &
            '   "ON ar.CustID = ct.custID AND ar.ID = ct.AreaID " &
            '   "Where month(busidate) = " & dfrom.Month & " and year(busidate) = " & dfrom.Year & " and smd.custid = " & custID & sOut & sOwn & sBrand &
            '   " Group by o.Kode2, smd.BusiDate, brd.Name order by smd.BusiDate,o.kode, brd.Name")

            'connDb.addParam(dfrom, "@paramFrom", MySqlDbType.DateTime)
            'connDb.addParam(dTo, "@paramTo", MySqlDbType.DateTime)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)


            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:SelectDataTable_MonthlySales")
            Err.Clear()
        End Try

        Return dsRslt
    End Function

    Public Function SelectDataTable_MonthlySales_gv(ByVal custID As Integer, ByVal dfrom As Date, ByVal Outletstr As String, ByVal iOwn As String, ByVal iBrand As String, ByVal iCity As String, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim dsRslt As New DataSet
        Dim sOut As String = ""
        Dim sBrand As String = ""
        Dim sOwn As String = ""
        Dim sCity As String = ""

        Try
            Dim connDb As New clsDbConn(ServerString)
            If iBrand = "0" Or iBrand = "" Then
                sBrand = ""
            Else
                sBrand = " and o.brandID in (" & iBrand & ")"
            End If

            If iOwn = "0" Or iOwn = "" Then
                sOwn = ""
            Else
                sOwn = " and o.ownID = " & iOwn
            End If
            If iCity = "0" Or iCity = "" Then
                sCity = ""
            Else
                sCity = " and o.cityID in (" & iCity & ")"
            End If

            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If Outletstr = "" Then
                    sOut = " and o.ID in (Select Store From cld_userxstore where user = '" & usr & "')"
                Else
                    sOut = " and o.kode in (" & Outletstr & ")"
                End If
            Else
                If Outletstr = "" Then
                    sOut = ""
                Else
                    sOut = " and o.Kode in (" & Outletstr & ")"
                End If
            End If

            connDb.resetCmd("SELECT date_format(smd.BusiDate,'%M') AS Month, o.Kode AS 'Store Code',o.nama 'Store Name', FORMAT(Sum(smd.Subtotal - smd.Discount), 2,'de_DE') 'Net Sales' " &
               "FROM cld_sumsalesdaily smd INNER JOIN cld_outlet o " &
               "ON smd.CustID = o.custID AND smd.olId = o.id " &
               "INNER JOIN cld_brand brd " &
               "ON brd.CustID = o.custID AND brd.ID = o.brandID " &
               "INNER JOIN cld_own own " &
               "ON own.CustID = o.custID AND own.ID = o.ownid " &
               "INNER JOIN cld_city ct " &
               "ON ct.CustID = o.custID AND ct.ID = o.cityid " &
               "INNER JOIN cld_area ar " &
               "ON ar.CustID = ct.custID AND ar.ID = ct.AreaID " &
               "Where busiDate >='" & dfrom.Date.ToString("yyyy-MM-01 00:00:00") & "' And busiDate <=(SELECT LAST_DAY('" & dfrom.Date.ToString("yyyy-MM-dd 00:00:00") & "')) and smd.custid = " & custID & sOut & sOwn & sBrand &
               " Group by o.Kode2, month(busidate), brd.Name order by month(busidate),o.kode, brd.Name")

            'connDb.resetCmd("SELECT FORMAT(Sum(smd.Subtotal - smd.Discount), 2,'de_DE') 'Net Sales', o.Kode AS 'Store Code',o.nama 'Store Name', date_format(smd.BusiDate,'%M') AS Month,  'Actual' Type, MONTH(smd.BusiDate) WPB, " &
            '   "CASE  MONTH(smd.BusiDate) " &
            '   "WHEN 1 THEN 'Q1' " &
            '   "WHEN 2 THEN 'Q1' " &
            '   "WHEN 3 THEN 'Q1' " &
            '   "WHEN 4 THEN 'Q2' " &
            '   "WHEN 5 THEN 'Q2' " &
            '   "WHEN 6 THEN 'Q2' " &
            '   "WHEN 7 THEN 'Q3' " &
            '   "WHEN 8 THEN 'Q3' " &
            '   "WHEN 9 THEN 'Q3' " &
            '   "ELSE 'Q4' " &
            '   "END AS Quarter " &
            '   ",YEAR(smd.BusiDate) Year, brd.Name3 Brand , ct.kota City, own.Name Owner,  ar.Name Area " &
            '   "FROM cld_sumsales smd INNER JOIN cld_outlet o " &
            '   "ON smd.CustID = o.custID AND smd.olId = o.id " &
            '   "INNER JOIN cld_brand brd " &
            '   "ON brd.CustID = o.custID AND brd.ID = o.brandID " &
            '   "INNER JOIN cld_own own " &
            '   "ON own.CustID = o.custID AND own.ID = o.ownid " &
            '   "INNER JOIN cld_city ct " &
            '   "ON ct.CustID = o.custID AND ct.ID = o.cityid " &
            '   "INNER JOIN cld_area ar " &
            '   "ON ar.CustID = ct.custID AND ar.ID = ct.AreaID " &
            '   "Where month(busidate) = " & dfrom.Month & " and year(busidate) = " & dfrom.Year & " and smd.custid = " & custID & sOut & sOwn & sBrand &
            '   " Group by o.Kode2, smd.BusiDate, brd.Name order by smd.BusiDate,o.kode, brd.Name")
            'connDb.addParam(dfrom, "@paramFrom", MySqlDbType.DateTime)
            'connDb.addParam(dTo, "@paramTo", MySqlDbType.DateTime)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)


            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:SelectDataTable_MonthlySales")
            Err.Clear()
        End Try

        Return dsRslt
    End Function
Private Function SalesSumBySalesType_gv(ByVal custID As Integer, ByVal dfrom As Date, ByVal dTo As Date, ByVal sOutlet As String, ByVal iOwn As String, ByVal iBrand As String, ByVal iCity As String, ByVal usr As String, ByVal modpriv As Integer) As DataSet

        Dim dsRslt As New DataSet
        Dim sOut As String = ""
        Dim sBrand As String = ""
        Dim sOwn As String = ""
        Dim sCity As String = ""

        Try
            Dim connDb As New clsDbConn(ServerString)

            If iBrand = "0" Or iBrand = "" Then
                sBrand = ""
            Else
                sBrand = " and o.brandID in (" & iBrand & ")"
            End If

            If iOwn = "0" Or iOwn = "" Then
                sOwn = ""
            Else
                sOwn = " and o.ownID = " & iOwn
            End If
            If iCity = "0" Or iCity = "" Then
                sCity = ""
            Else
                sCity = " and o.cityID in (" & iCity & ")"
            End If

            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If sOutlet = "" Then
                    sOut = " and sm.olId in (Select Store From cld_userxstore where user = '" & usr & "')"
                Else
                    sOut = " and sm.olnumber in (" & sOutlet & ")"
                End If
            Else
                If sOutlet = "" Then
                    sOut = ""
                Else
                    sOut = " and sm.olnumber in (" & sOutlet & ")"
                End If
            End If


            connDb.resetCmd("(SELECT DATE_FORMAT(sm.busidate,'%d-%b-%Y') 'Sales Date' " &
                         ",sm.olnumber 'Store Code' " &
                         ",sm.olname 'Store Name' " &
                         ", CASE

                            WHEN sm.SlsTypeNm = 'DineIn' THEN 'DINE IN'

                            WHEN sm.SlsTypeNm = 'TakeAway' THEN 'TAKE AWAY'

                            ELSE sm.SlsTypeNm

                        END AS 'Channel' " &
                         ",FORMAT(SUM(sm.Subtotal - sm.Discount), 2,'de_DE') 'Net Sales' " &
                         ",FORMAT(SUM(sm.Tax), 2,'de_DE') AS Tax " &
                         ",FORMAT(SUM(sm.total), 2,'de_DE') 'Gross Sales' " &
                         ",FORMAT(Sum(sm.QtyTrans), 0,'de_DE') 'Total Check' " &
                         ",FORMAT(SUM(sm.total) / Sum(sm.QtyTrans), 2,'de_DE') 'Avg Sales Per Check' " &
                           "FROM cld_sumsalesdaily sm INNER JOIN cld_outlet o " &
                    "ON sm.CustID = o.custID AND sm.olId = o.id " &
                     "where sm.busiDate >=@paramFrom And sm.busiDate <=@paramTo " &
                     " AND (sm.CustID =@custID OR @custID=0) " & sOut & sOwn & sBrand & sCity & " GROUP BY sm.busidate, sm.olid, sm.slstypenm Order BY sm.olid, sm.busidate, sm.slstypenm) " &
                     "Union all " &
                     "SELECT '' as Date " &
                         ",'' olnumber " &
                         ",'Total' olname " &
                         ", '' AS Channel " &
                         ",FORMAT(SUM(sm.Subtotal - sm.Discount), 2,'de_DE') AS Subt_AfterDisc‎ " &
                         ",FORMAT(SUM(sm.Tax), 2,'de_DE') AS Tax " &
                         ",FORMAT(SUM(sm.total), 2,'de_DE') AS Ttl_Sales‎ " &
                         ",FORMAT(Sum(sm.QtyTrans), 0,'de_DE') AS Ttl_Check " &
                         ",'' AS Avg_PerCheck " &
                          "FROM cld_sumsalesdaily sm INNER JOIN cld_outlet o " &
                    "ON sm.CustID = o.custID AND sm.olId = o.id " &
                     "where sm.busiDate >=@paramFrom And sm.busiDate <=@paramTo " &
                     " AND (sm.CustID =@custID OR @custID=0) " & sOut & sOwn & sBrand & sCity)



            connDb.addParam(dfrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(dTo, "@paramTo", MySqlDbType.DateTime)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)
            ' connDb.addParam(MemberID, "@memberID", MySqlDbType.Int32)


            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:salesSumBySalesType")
            Err.Clear()
        End Try

        Return dsRslt
    End Function
    Private Function SalesSumBySalesType(ByVal custID As Integer, ByVal dfrom As Date, ByVal dTo As Date, ByVal sOutlet As String, ByVal iOwn As String, ByVal iBrand As String, ByVal iCity As String, ByVal usr As String, ByVal modpriv As Integer) As DataSet

        Dim dsRslt As New DataSet
        Dim sOut As String = ""
        Dim sBrand As String = ""
        Dim sOwn As String = ""
        Dim sCity As String = ""

        Try
            Dim connDb As New clsDbConn(ServerString)

            If iBrand = "0" Or iBrand = "" Then
                sBrand = ""
            Else
                sBrand = " and o.brandID in (" & iBrand & ")"
            End If

            If iOwn = "0" Or iOwn = "" Then
                sOwn = ""
            Else
                sOwn = " and o.ownID = " & iOwn
            End If
            If iCity = "0" Or iCity = "" Then
                sCity = ""
            Else
                sCity = " and o.cityID in (" & iCity & ")"
            End If

            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If sOutlet = "" Then
                    sOut = " and sm.olId in (Select Store From cld_userxstore where user = '" & usr & "')"
                Else
                    sOut = " and sm.olnumber in (" & sOutlet & ")"
                End If
            Else
                If sOutlet = "" Then
                    sOut = ""
                Else
                    sOut = " and sm.olnumber in (" & sOutlet & ")"
                End If
            End If


            'connDb.resetCmd("SELECT DATE_FORMAT(sm.busidate,'%d-%b-%Y') as Date " &
            '             ",sm.olnumber 'Store No' " &
            '             ",sm.olname 'Store name' " &
            '             ",SUM(sm.Subtotal - sm.Discount) AS Subt_AfterDisc‎ " &
            '             ",SUM(sm.Tax) AS Tax " &
            '             ",SUM(sm.total) AS Ttl_Sales‎ " &
            '             ",SUM(sm.cover) AS Ttl_Cover " &
            '             ",SUM(sm.total) / SUM(sm.cover) AS Avg_PerCover " &
            '             ",Sum(sm.QtyTrans) AS Ttl_Check " &
            '             ",SUM(sm.total) / Sum(sm.QtyTrans) AS Avg_PerCheck " &
            '             ", Sum(ppn) as ppn " &
            '             ", Sum(pb1) as pb1 " &
            '             ", sm.slstypenm AS SalesType " &
            '         "FROM cld_sumsalesdaily sm INNER JOIN cld_outlet o " &
            '        "ON sm.CustID = o.custID AND sm.olId = o.id " &
            '         "where sm.busiDate >=@paramFrom And sm.busiDate <=@paramTo " &
            '         " AND (sm.CustID =@custID OR @custID=0) " & sOut & sOwn & sBrand & sCity & " GROUP BY sm.busidate, sm.olid, sm.slstypenm Order BY sm.olid, sm.busidate, sm.slstypenm")
            connDb.resetCmd("SELECT DATE_FORMAT(sm.busidate,'%d-%b-%Y') 'Sales Date' " &
                         ",sm.olnumber 'Store Code' " &
                         ",sm.olname 'Store Name' " &
                         ", CASE

                            WHEN sm.SlsTypeNm = 'DineIn' THEN 'DINE IN'

                            WHEN sm.SlsTypeNm = 'TakeAway' THEN 'TAKE AWAY'

                            ELSE sm.SlsTypeNm

                        END AS 'Channel' " &
                         ",SUM(sm.Subtotal - sm.Discount) 'Net Sales' " &
                         ",SUM(sm.Tax) AS Tax " &
                         ",SUM(sm.total) 'Gross Sales' " &
                         ",Sum(sm.QtyTrans) 'Total Check' " &
                         ",SUM(sm.total) / Sum(sm.QtyTrans) 'Avg Sales Per Check' " &
                          "FROM cld_sumsalesdaily sm INNER JOIN cld_outlet o " &
                    "ON sm.CustID = o.custID AND sm.olId = o.id " &
                     "where sm.busiDate >=@paramFrom And sm.busiDate <=@paramTo " &
                     " AND (sm.CustID =@custID OR @custID=0) " & sOut & sOwn & sBrand & sCity & " GROUP BY sm.busidate, sm.olid, sm.slstypenm Order BY sm.olid, sm.busidate, sm.slstypenm ")



            connDb.addParam(dfrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(dTo, "@paramTo", MySqlDbType.DateTime)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)
            ' connDb.addParam(MemberID, "@memberID", MySqlDbType.Int32)


            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:salesSumBySalesType")
            Err.Clear()
        End Try

        Return dsRslt
    End Function
    Private Function SelectDataTable_GoFood(ByVal custID As Integer, ByVal dfrom As Date, ByVal dTo As Date, ByVal sOutlet As String, ByVal iOwn As String, ByVal iBrand As String, ByVal iCity As String, ByVal usr As String, ByVal modpriv As Integer) As DataSet

        Dim dsRslt As New DataSet
        Dim sOut As String = ""
        Dim sBrand As String = ""
        Dim sOwn As String = ""
        Dim sCity As String = ""
        Dim sSlsType As String = ""

        Try
            Dim connDb As New clsDbConn(ServerString)

            If iBrand = "0" Or iBrand = "" Then
                sBrand = ""
            Else
                sBrand = " and o.brandID in (" & iBrand & ")"
            End If

            If iOwn = "0" Or iOwn = "" Then
                sOwn = ""
            Else
                sOwn = " and o.ownID = " & iOwn
            End If
            If iCity = "0" Or iCity = "" Then
                sCity = ""
            Else
                sCity = " and o.cityID in (" & iCity & ")"
            End If

            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If sOutlet = "" Then
                    sOut = " and sm.olId in (Select Store From cld_userxstore where user = '" & usr & "')"
                Else
                    sOut = " and sm.olnumber in (" & sOutlet & ")"
                End If
            Else
                If sOutlet = "" Then
                    sOut = ""
                Else
                    sOut = " and sm.olnumber in (" & sOutlet & ")"
                End If
            End If
            If My.Settings.gofood = "" Then
                sSlsType = ""
            Else
                sSlsType = " and sm.SlsTypeNm in (" & My.Settings.gofood & ")"
            End If

            connDb.resetCmd("SELECT ItemNo 'Item Code', ItemName as 'Item Name', sum(Qty) as Qty, sum(total) as 'Net Sales', o.kode 'Store Code',o.nama 'Store Name', DATE_FORMAT(sm.BusiDate, '%M %Y') AS Periode " &
               "FROM cld_sumsalesdetaildaily sm INNER JOIN cld_outlet o " &
               "ON sm.olId = o.ID AND sm.CustID = o.custID " &
                     "where Month(sm.busiDate) = " & Month(dfrom) & " and year(sm.busiDate) = " & Year(dfrom) &
                     " AND (sm.CustID =@custID OR @custID=0) " & sOut & sOwn & sBrand & sCity & sSlsType & " GROUP BY DATE_FORMAT(sm.BusiDate, '%m/%Y'), sm.olid Order BY sm.olid, sm.busidate, sm.slstypenm")



            connDb.addParam(dfrom, "@paramFrom", MySqlDbType.DateTime)
            'connDb.addParam(dTo, "@paramTo", MySqlDbType.DateTime)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)
            ' connDb.addParam(MemberID, "@memberID", MySqlDbType.Int32)


            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:SelectDataTable_GoFood")
            Err.Clear()
        End Try

        Return dsRslt
    End Function

    Private Function SelectDataTable_Onlineorders(ByVal custID As Integer, ByVal dfrom As Date, ByVal dTo As Date, ByVal sOutlet As String, ByVal iOwn As String, ByVal iBrand As String, ByVal iCity As String, ByVal usr As String, ByVal modpriv As Integer) As DataSet

        Dim dsRslt As New DataSet
        Dim sOut As String = ""
        Dim sBrand As String = ""
        Dim sOwn As String = ""
        Dim sCity As String = ""
        Dim sSlsType As String = ""

        Try
            Dim connDb As New clsDbConn(ServerString)

            If iBrand = "0" Or iBrand = "" Then
                sBrand = ""
            Else
                sBrand = " and o.brandID in (" & iBrand & ")"
            End If

            If iOwn = "0" Or iOwn = "" Then
                sOwn = ""
            Else
                sOwn = " and o.ownID = " & iOwn
            End If
            If iCity = "0" Or iCity = "" Then
                sCity = ""
            Else
                sCity = " and o.cityID in (" & iCity & ")"
            End If

            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If sOutlet = "" Then
                    sOut = " and sm.olId in (Select Store From cld_userxstore where user = '" & usr & "')"
                Else
                    sOut = " and sm.olnumber in (" & sOutlet & ")"
                End If
            Else
                If sOutlet = "" Then
                    sOut = ""
                Else
                    sOut = " and sm.olnumber in (" & sOutlet & ")"
                End If
            End If
            If My.Settings.gofood = "" And My.Settings.grab = "" Then
                sSlsType = ""
            Else
                sSlsType = " and sm.SlsTypeNm in (" & My.Settings.gofood & "," & My.Settings.grab & ")"
            End If

            connDb.resetCmd("SELECT ItemNo 'Item Code', ItemName as 'Item Name', sum(Qty) as Qty, sum(total) as 'Net Sales', o.kode 'Store Code',o.nama 'Store Name', DATE_FORMAT(sm.BusiDate, '%M %Y') AS Periode " &
               "FROM cld_sumsalesdetaildaily sm INNER JOIN cld_outlet o " &
               "ON sm.olId = o.ID AND sm.CustID = o.custID " &
                     "where sm.busiDate >='" & dfrom.Date.ToString("yyyy-MM-01 00:00:00") & "' And sm.busiDate <=(SELECT LAST_DAY('" & dfrom.Date.ToString("yyyy-MM-dd 00:00:00") & "')) " &
                     " AND (sm.CustID =@custID OR @custID=0) " & sOut & sOwn & sBrand & sCity & sSlsType & " GROUP BY DATE_FORMAT(sm.BusiDate, '%m/%Y'), sm.olid Order BY sm.olid, sm.busidate, sm.slstypenm")



            'connDb.addParam(dfrom, "@paramFrom", MySqlDbType.DateTime)
            'connDb.addParam(dTo, "@paramTo", MySqlDbType.DateTime)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)
            ' connDb.addParam(MemberID, "@memberID", MySqlDbType.Int32)


            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:SelectDataTable_GoFood")
            Err.Clear()
        End Try

        Return dsRslt
    End Function

    Private Function SelectDataTable_GoFood_gv(ByVal custID As Integer, ByVal dfrom As Date, ByVal dTo As Date, ByVal sOutlet As String, ByVal iOwn As String, ByVal iBrand As String, ByVal iCity As String, ByVal usr As String, ByVal modpriv As Integer) As DataSet

        Dim dsRslt As New DataSet
        Dim sOut As String = ""
        Dim sBrand As String = ""
        Dim sOwn As String = ""
        Dim sCity As String = ""
        Dim sSlsType As String = ""

        Try
            Dim connDb As New clsDbConn(ServerString)

            If iBrand = "0" Or iBrand = "" Then
                sBrand = ""
            Else
                sBrand = " and o.brandID in (" & iBrand & ")"
            End If

            If iOwn = "0" Or iOwn = "" Then
                sOwn = ""
            Else
                sOwn = " and o.ownID = " & iOwn
            End If
            If iCity = "0" Or iCity = "" Then
                sCity = ""
            Else
                sCity = " and o.cityID in (" & iCity & ")"
            End If

            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If sOutlet = "" Then
                    sOut = " and sm.olId in (Select Store From cld_userxstore where user = '" & usr & "')"
                Else
                    sOut = " and sm.olnumber in (" & sOutlet & ")"
                End If
            Else
                If sOutlet = "" Then
                    sOut = ""
                Else
                    sOut = " and sm.olnumber in (" & sOutlet & ")"
                End If
            End If
            If My.Settings.gofood = "" Then
                sSlsType = ""
            Else
                sSlsType = " and sm.SlsTypeNm in (" & My.Settings.gofood & ")"
            End If

            connDb.resetCmd("SELECT ItemNo 'Item Code', ItemName as 'Item Name', sum(Qty) as Qty, FORMAT(sum(total), 2,'de_DE') as 'Net Sales', o.kode 'Store Code',o.nama 'Store Name', DATE_FORMAT(sm.BusiDate, '%M %Y') AS Periode " &
               "FROM cld_sumsalesdetaildaily sm INNER JOIN cld_outlet o " &
               "ON sm.olId = o.ID AND sm.CustID = o.custID " &
                     "where Month(sm.busiDate) = " & Month(dfrom) & " and year(sm.busiDate) = " & Year(dfrom) &
                     " AND (sm.CustID =@custID OR @custID=0) " & sOut & sOwn & sBrand & sCity & sSlsType & " GROUP BY DATE_FORMAT(sm.BusiDate, '%m/%Y'), sm.olid Order BY sm.olid, sm.busidate, sm.slstypenm")



            connDb.addParam(dfrom, "@paramFrom", MySqlDbType.DateTime)
            'connDb.addParam(dTo, "@paramTo", MySqlDbType.DateTime)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)
            ' connDb.addParam(MemberID, "@memberID", MySqlDbType.Int32)


            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:SelectDataTable_GoFood_gv")
            Err.Clear()
        End Try

        Return dsRslt
    End Function

    Private Function SelectDataTable_Onlineorders_gv(ByVal custID As Integer, ByVal dfrom As Date, ByVal dTo As Date, ByVal sOutlet As String, ByVal iOwn As String, ByVal iBrand As String, ByVal iCity As String, ByVal usr As String, ByVal modpriv As Integer) As DataSet

        Dim dsRslt As New DataSet
        Dim sOut As String = ""
        Dim sBrand As String = ""
        Dim sOwn As String = ""
        Dim sCity As String = ""
        Dim sSlsType As String = ""

        Try
            Dim connDb As New clsDbConn(ServerString)

            If iBrand = "0" Or iBrand = "" Then
                sBrand = ""
            Else
                sBrand = " and o.brandID in (" & iBrand & ")"
            End If

            If iOwn = "0" Or iOwn = "" Then
                sOwn = ""
            Else
                sOwn = " and o.ownID = " & iOwn
            End If
            If iCity = "0" Or iCity = "" Then
                sCity = ""
            Else
                sCity = " and o.cityID in (" & iCity & ")"
            End If

            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If sOutlet = "" Then
                    sOut = " and sm.olId in (Select Store From cld_userxstore where user = '" & usr & "')"
                Else
                    sOut = " and sm.olnumber in (" & sOutlet & ")"
                End If
            Else
                If sOutlet = "" Then
                    sOut = ""
                Else
                    sOut = " and sm.olnumber in (" & sOutlet & ")"
                End If
            End If
            If My.Settings.gofood = "" And My.Settings.grab = "" Then
                sSlsType = ""
            Else
                sSlsType = " and sm.SlsTypeNm in (" & My.Settings.gofood & "," & My.Settings.grab & ")"
            End If

            connDb.resetCmd("SELECT ItemNo 'Item Code', ItemName as 'Item Name', sum(Qty) as Qty, FORMAT(sum(total), 2,'de_DE') as 'Net Sales', o.kode 'Store Code',o.nama 'Store Name', DATE_FORMAT(sm.BusiDate, '%M %Y') AS Periode " &
               "FROM cld_sumsalesdetaildaily sm INNER JOIN cld_outlet o " &
               "ON sm.olId = o.ID AND sm.CustID = o.custID " &
                     "where sm.busiDate >='" & dfrom.Date.ToString("yyyy-MM-01 00:00:00") & "' And sm.busiDate <=(SELECT LAST_DAY('" & dfrom.Date.ToString("yyyy-MM-dd 00:00:00") & "')) " &
                     " AND (sm.CustID =@custID OR @custID=0) " & sOut & sOwn & sBrand & sCity & sSlsType & " GROUP BY DATE_FORMAT(sm.BusiDate, '%m/%Y'), sm.olid Order BY sm.olid, sm.busidate, sm.slstypenm")



            'connDb.addParam(dfrom, "@paramFrom", MySqlDbType.DateTime)
            'connDb.addParam(dTo, "@paramTo", MySqlDbType.DateTime)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)
            ' connDb.addParam(MemberID, "@memberID", MySqlDbType.Int32)


            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:SelectDataTable_GoFood_gv")
            Err.Clear()
        End Try

        Return dsRslt
    End Function
    Private Function Selectdt_onlineorders(ByVal custID As Integer, ByVal dfrom As Date, ByVal dTo As Date, ByVal sOutlet As String, ByVal iOwn As String, ByVal iBrand As String, ByVal iCity As String, ByVal usr As String, ByVal modpriv As Integer) As DataSet

        Dim dsRslt As New DataSet
        Dim sOut As String = ""
        Dim sBrand As String = ""
        Dim sOwn As String = ""
        Dim sCity As String = ""
        Dim sSlsType As String = ""

        Try
            Dim connDb As New clsDbConn(ServerString)

            If iBrand = "0" Or iBrand = "" Then
                sBrand = ""
            Else
                sBrand = " and o.brandID in (" & iBrand & ")"
            End If

            If iOwn = "0" Or iOwn = "" Then
                sOwn = ""
            Else
                sOwn = " and o.ownID = " & iOwn
            End If
            If iCity = "0" Or iCity = "" Then
                sCity = ""
            Else
                sCity = " and o.cityID in (" & iCity & ")"
            End If

            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If sOutlet = "" Then
                    sOut = " and sm.olId in (Select Store From cld_userxstore where user = '" & usr & "')"
                Else
                    sOut = " and sm.olnumber in (" & sOutlet & ")"
                End If
            Else
                If sOutlet = "" Then
                    sOut = ""
                Else
                    sOut = " and sm.olnumber in (" & sOutlet & ")"
                End If
            End If
            If My.Settings.gofood = "" And My.Settings.grab = "" Then
                sSlsType = ""
            Else
                sSlsType = " and sm.SlsTypeNm in (" & My.Settings.gofood & "," & My.Settings.grab & ")"
            End If

            connDb.resetCmd("SELECT DATE_FORMAT(sm.busidate,'%d-%b-%Y') 'Sales Date', o.kode 'Store Code', o.nama 'Store Name', ItemNo 'Item Code', ItemName as 'Item Name', sum(Qty) as Qty, sum(total) as 'Net Sales' " &
               "FROM cld_sumsalesdetaildaily sm INNER JOIN cld_outlet o " &
               "ON sm.olId = o.ID AND sm.CustID = o.custID " &
               "LEFT JOIN `cld_brand` b ON o.`custID` = b.`CustID` AND o.`brandID` = b.`ID` " &
               "Where sm.custid = " & custID & " and BusiDate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' and BusiDate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "'" & sOut & sOwn & sBrand & sSlsType &
               "GROUP BY sm.BusiDate, o.Kode, ItemNo, ItemName Order by o.Kode, sm.BusiDate, ItemNo, ItemName ")



            connDb.addParam(dfrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(dTo, "@paramTo", MySqlDbType.DateTime)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)
            ' connDb.addParam(MemberID, "@memberID", MySqlDbType.Int32)


            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:salesSumBySalesType")
            Err.Clear()
        End Try

        Return dsRslt
    End Function
    Private Function Selectdt_onlineorders_gv(ByVal custID As Integer, ByVal dfrom As Date, ByVal dTo As Date, ByVal sOutlet As String, ByVal iOwn As String, ByVal iBrand As String, ByVal iCity As String, ByVal usr As String, ByVal modpriv As Integer) As DataSet

        Dim dsRslt As New DataSet
        Dim sOut As String = ""
        Dim sBrand As String = ""
        Dim sOwn As String = ""
        Dim sCity As String = ""
        Dim sSlsType As String = ""

        Try
            Dim connDb As New clsDbConn(ServerString)

            If iBrand = "0" Or iBrand = "" Then
                sBrand = ""
            Else
                sBrand = " and o.brandID in (" & iBrand & ")"
            End If

            If iOwn = "0" Or iOwn = "" Then
                sOwn = ""
            Else
                sOwn = " and o.ownID = " & iOwn
            End If
            If iCity = "0" Or iCity = "" Then
                sCity = ""
            Else
                sCity = " and o.cityID in (" & iCity & ")"
            End If

            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If sOutlet = "" Then
                    sOut = " and sm.olId in (Select Store From cld_userxstore where user = '" & usr & "')"
                Else
                    sOut = " and sm.olnumber in (" & sOutlet & ")"
                End If
            Else
                If sOutlet = "" Then
                    sOut = ""
                Else
                    sOut = " and sm.olnumber in (" & sOutlet & ")"
                End If
            End If
            If My.Settings.gofood = "" And My.Settings.grab = "" Then
                sSlsType = ""
            Else
                sSlsType = " and sm.SlsTypeNm in (" & My.Settings.gofood & "," & My.Settings.grab & ")"
            End If

            connDb.resetCmd("SELECT DATE_FORMAT(sm.busidate,'%d-%b-%Y') 'Sales Date', o.kode 'Store Code', o.nama 'Store Name', ItemNo 'Item Code', ItemName as 'Item Name', sum(Qty) as Qty, FORMAT(sum(total), 2,'de_DE') as 'Net Sales' " &
               "FROM cld_sumsalesdetaildaily sm INNER JOIN cld_outlet o " &
               "ON sm.olId = o.ID AND sm.CustID = o.custID " &
               "LEFT JOIN `cld_brand` b ON o.`custID` = b.`CustID` AND o.`brandID` = b.`ID` " &
               "Where sm.custid = " & custID & " and BusiDate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' and BusiDate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "'" & sOut & sOwn & sBrand & sSlsType &
               "GROUP BY sm.BusiDate, o.Kode, ItemNo, ItemName Order by o.Kode, sm.BusiDate, ItemNo, ItemName ")



            connDb.addParam(dfrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(dTo, "@paramTo", MySqlDbType.DateTime)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)
            ' connDb.addParam(MemberID, "@memberID", MySqlDbType.Int32)


            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:salesSumBySalesType")
            Err.Clear()
        End Try

        Return dsRslt
    End Function
    Private Function SelectDataTable_SlsItem(ByVal custID As Integer, ByVal dfrom As Date, ByVal dTo As Date, ByVal sOutlet As String, ByVal iOwn As String, ByVal iBrand As String, ByVal iCity As String, ByVal usr As String, ByVal modpriv As Integer) As DataSet

        Dim dsRslt As New DataSet
        Dim sOut As String = ""
        Dim sBrand As String = ""
        Dim sOwn As String = ""
        Dim sCity As String = ""
        Dim sSlsType As String = ""

        Try
            Dim connDb As New clsDbConn(ServerString)

            If iBrand = "0" Or iBrand = "" Then
                sBrand = ""
            Else
                sBrand = " and o.brandID in (" & iBrand & ")"
            End If

            If iOwn = "0" Or iOwn = "" Then
                sOwn = ""
            Else
                sOwn = " and o.ownID = " & iOwn & " "
            End If
            If iCity = "0" Or iCity = "" Then
                sCity = ""
            Else
                sCity = " and o.cityID in (" & iCity & ")"
            End If

            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If sOutlet = "" Then
                    sOut = " and s.olId in (Select Store From cld_userxstore where user = '" & usr & "')"
                Else
                    sOut = " and s.olnumber in (" & sOutlet & ")"
                End If
            Else
                If sOutlet = "" Then
                    sOut = ""
                Else
                    sOut = " and s.olnumber in (" & sOutlet & ")"
                End If
            End If
            If My.Settings.gofood = "" Then
                sSlsType = ""
            Else
                sSlsType = " and s.SlsTypeNm in (" & My.Settings.gofood & ")"
            End If

            connDb.resetCmd("SELECT Date AS 'Sales Date',

                CASE

                            WHEN SlsTypeNm = 'DineIn' THEN 'DINE IN'

                            WHEN SlsTypeNm = 'TakeAway' THEN 'TAKE AWAY'

                            ELSE SlsTypeNm

                        END AS 'Channel' ,

                itemno 'Item Code', Item 'Item Name',Qty, total 'Net Sales',(total * 100 / t2.sum) AS '% Contribution' " &
                "FROM " &
                "(SELECT s.ItemNo ,s.`ItemName` item,SUM(s.`Qty`) qty,SUM(s.`Total`)total,DATE_FORMAT(s.busidate,'%d-%b-%Y') DATE,s.`SlsTypeNm` " &
                "FROM `cld_sumsalesdetaildaily` s " &
                "LEFT JOIN cld_outlet o ON s.`CustID` = o.`custID` And s.`olId` = o.`ID` " &
                "LEFT JOIN `cld_brand` b ON o.`custID` = b.`CustID` AND o.`brandID` = b.`ID` " &
                "Where s.custid = " & custID & " And BusiDate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' and BusiDate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "'" & sOut & sOwn & sBrand & sCity &
                "GROUP BY s.`BusiDate`,s.itemno,s.`SlsTypeNm`)AS t1 " &
                "CROSS JOIN " &
                "(SELECT SUM(s.total) AS SUM " &
                "FROM `cld_sumsalesdetaildaily` s " &
                "LEFT JOIN cld_outlet o ON s.`CustID` = o.`custID` And s.`olId` = o.`ID` " &
                "LEFT JOIN `cld_brand` b ON o.`custID` = b.`CustID` AND o.`brandID` = b.`ID` " &
                 "Where s.custid = " & custID & " And BusiDate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' and BusiDate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "'" & sOut & sOwn & sBrand & sCity & ") t2 "
            )

            'connDb.resetCmd("SELECT ItemNo 'Item Code', Item , Qty,  NetSales 'Net Sales', Contribution, DATE as 'Sales Date', TYPE, MONTH, QUARTER, YEAR, Brand 
            '                    FROM 
            '                    ( 
            '                     SELECT ItemNo , Item,Qty, total AS NetSales,
            '                     (total * 100 / t2.sum) AS Contribution, DATE, SlsTypeNm AS TYPE, MONTH, QUARTER, YEAR, NAME3 AS Brand 
            '                     FROM
            '                      (SELECT s.ItemNo,s.`ItemName` item,SUM(s.`Qty`) qty,SUM(s.`Total`)total,DATE_FORMAT(s.busidate,'%d-%b-%Y') DATE,s.`SlsTypeNm`,MONTH(s.busidate) MONTH, 
            '                      CONCAT('Q',QUARTER(s.`BusiDate`)) QUARTER, YEAR(s.`BusiDate`) YEAR,b.`Name3` FROM `cld_sumsalesdetaildaily` s 
            '                      LEFT JOIN cld_outlet o ON s.`CustID` = o.`custID` AND s.`olId` = o.`ID` LEFT JOIN `cld_brand` b ON o.`custID` = b.`CustID` AND o.`brandID` = b.`ID` 
            '                       Where s.custid = " & custID & " And BusiDate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' and BusiDate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "'" & sOut & sOwn & sBrand & sCity &
            '                        "Group BY s.`BusiDate`, s.itemno, s.`SlsTypeNm`)AS t1 
            '                      CROSS JOIN (SELECT SUM(s.total) AS SUM FROM `cld_sumsalesdetaildaily` s LEFT JOIN cld_outlet o ON s.`CustID` = o.`custID` AND s.`olId` = o.`ID` 
            '                      LEFT JOIN `cld_brand` b ON o.`custID` = b.`CustID` AND o.`brandID` = b.`ID` WHERE s.custid = " & custID & " AND 
            '                      BusiDate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' and BusiDate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "'" & sOut & sOwn & sBrand & sCity & ") t2

            '                     UNION ALL

            '                     SELECT 'Total' AS ItemNo, '' item ,SUM(s.`Qty`) qty, SUM(s.`Total`) total, 100, '' DATE, '' SlsTypeNm , '' MONTH, 
            '                     '' QUARTER, '' YEAR, '' Name3 FROM `cld_sumsalesdetaildaily` s 
            '                     WHERE s.custid = " & custID & " AND BusiDate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' and BusiDate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "'" & sOut & sOwn & sBrand & sCity & ") a ")


            'connDb.resetCmd("SELECT itemno 'Item Code', Item,Qty, total AS NetSales,(total * 100 / t2.sum) AS Contribution, Date, SlsTypeNm AS Type, Month, Quarter, Year, NAME3 AS Brand " &
            '    "FROM " &
            '    "(SELECT s.ItemNo ,s.`ItemName` item,SUM(s.`Qty`) qty,SUM(s.`Total`)total,DATE_FORMAT(s.busidate,'%d-%b-%Y') DATE,s.`SlsTypeNm`,MONTH(s.busidate) MONTH, " &
            '    "CONCAT('Q',QUARTER(s.`BusiDate`)) QUARTER, YEAR(s.`BusiDate`) YEAR,b.`Name3` " &
            '     "FROM `cld_sumsalesdetaildaily` s " &
            '    "LEFT JOIN cld_outlet o ON s.`CustID` = o.`custID` And s.`olId` = o.`ID` " &
            '    "LEFT JOIN `cld_brand` b ON o.`custID` = b.`CustID` AND o.`brandID` = b.`ID` " &
            '    "Where s.custid = " & custID & " And BusiDate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' and BusiDate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "'" & sOut & sOwn & sBrand & sCity &
            '    "GROUP BY s.`BusiDate`,s.itemno,s.`SlsTypeNm`)AS t1 " &
            '    "CROSS JOIN " &
            '    "(SELECT SUM(s.total) AS SUM " &
            '    "FROM `cld_sumsalesdetaildaily` s " &
            '    "LEFT JOIN cld_outlet o ON s.`CustID` = o.`custID` And s.`olId` = o.`ID` " &
            '    "LEFT JOIN `cld_brand` b ON o.`custID` = b.`CustID` AND o.`brandID` = b.`ID` " &
            '     "Where s.custid = " & custID & " And BusiDate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' and BusiDate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "'" & sOut & sOwn & sBrand & sCity & ") t2 ")

            connDb.addParam(dfrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(dTo, "@paramTo", MySqlDbType.DateTime)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)
            ' connDb.addParam(MemberID, "@memberID", MySqlDbType.Int32)


            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:SelectDataTable_SlsItem")
            Err.Clear()
        End Try

        Return dsRslt
    End Function
    Private Function SelectDataTable_SlsItem_gv(ByVal custID As Integer, ByVal dfrom As Date, ByVal dTo As Date, ByVal sOutlet As String, ByVal iOwn As String, ByVal iBrand As String, ByVal iCity As String, ByVal usr As String, ByVal modpriv As Integer) As DataSet

        Dim dsRslt As New DataSet
        Dim sOut As String = ""
        Dim sBrand As String = ""
        Dim sOwn As String = ""
        Dim sCity As String = ""
        Dim sSlsType As String = ""

        Try
            Dim connDb As New clsDbConn(ServerString)

            If iBrand = "0" Or iBrand = "" Then
                sBrand = ""
            Else
                sBrand = " and o.brandID in (" & iBrand & ")"
            End If

            If iOwn = "0" Or iOwn = "" Then
                sOwn = ""
            Else
                sOwn = " and o.ownID = " & iOwn & " "
            End If
            If iCity = "0" Or iCity = "" Then
                sCity = ""
            Else
                sCity = " and o.cityID in (" & iCity & ")"
            End If

            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If sOutlet = "" Then
                    sOut = " and s.olId in (Select Store From cld_userxstore where user = '" & usr & "')"
                Else
                    sOut = " and s.olnumber in (" & sOutlet & ")"
                End If
            Else
                If sOutlet = "" Then
                    sOut = ""
                Else
                    sOut = " and s.olnumber in (" & sOutlet & ")"
                End If
            End If
            If My.Settings.gofood = "" Then
                sSlsType = ""
            Else
                sSlsType = " and s.SlsTypeNm in (" & My.Settings.gofood & ")"
            End If

            'connDb.resetCmd("SELECT ItemNo 'Kode Item', Item,Qty, FORMAT(total, 2,'de_DE') AS NetSales,FORMAT((total * 100 / t2.sum), 2,'de_DE') AS Contribution, Date, SlsTypeNm AS Type, Month, Quarter, Year, NAME3 AS Brand " &
            '    "FROM " &
            '    "(SELECT s.ItemNo,s.`ItemName` item,SUM(s.`Qty`) qty,SUM(s.`Total`)total,DATE_FORMAT(s.busidate,'%d-%b-%Y') DATE,s.`SlsTypeNm`,MONTH(s.busidate) MONTH, " &
            '    "CONCAT('Q',QUARTER(s.`BusiDate`)) QUARTER, YEAR(s.`BusiDate`) YEAR,b.`Name3` " &
            '     "FROM `cld_sumsalesdetaildaily` s " &
            '    "LEFT JOIN cld_outlet o ON s.`CustID` = o.`custID` And s.`olId` = o.`ID` " &
            '    "LEFT JOIN `cld_brand` b ON o.`custID` = b.`CustID` AND o.`brandID` = b.`ID` " &
            '    "Where s.custid = " & custID & " And BusiDate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' and BusiDate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "'" & sOut & sOwn & sBrand & sCity &
            '    "GROUP BY s.`BusiDate`,s.itemno,s.`SlsTypeNm`)AS t1 " &
            '    "CROSS JOIN " &
            '    "(SELECT SUM(s.total) AS SUM " &
            '    "FROM `cld_sumsalesdetaildaily` s " &
            '    "LEFT JOIN cld_outlet o ON s.`CustID` = o.`custID` And s.`olId` = o.`ID` " &
            '    "LEFT JOIN `cld_brand` b ON o.`custID` = b.`CustID` AND o.`brandID` = b.`ID` " &
            '     "Where s.custid = " & custID & " And BusiDate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' and BusiDate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "'" & sOut & sOwn & sBrand & sCity & ") t2 ")

            'connDb.resetCmd("SELECT ItemNo 'Item Code', Item , Qty,  FORMAT(NetSales, 2,'de_DE') 'Net Sales', FORMAT(Contribution, 2,'de_DE') 'Contribution', DATE as 'Sales Date', TYPE, MONTH, QUARTER, YEAR, Brand 
            '                    FROM 
            '                    ( 
            '                     SELECT ItemNo , Item,Qty, total AS NetSales,
            '                     (total * 100 / t2.sum) AS Contribution, DATE, SlsTypeNm AS TYPE, MONTH, QUARTER, YEAR, NAME3 AS Brand 
            '                     FROM
            '                      (SELECT s.ItemNo,s.`ItemName` item,SUM(s.`Qty`) qty,SUM(s.`Total`)total,DATE_FORMAT(s.busidate,'%d-%b-%Y') DATE,s.`SlsTypeNm`,MONTH(s.busidate) MONTH, 
            '                      CONCAT('Q',QUARTER(s.`BusiDate`)) QUARTER, YEAR(s.`BusiDate`) YEAR,b.`Name3` FROM `cld_sumsalesdetaildaily` s 
            '                      LEFT JOIN cld_outlet o ON s.`CustID` = o.`custID` AND s.`olId` = o.`ID` LEFT JOIN `cld_brand` b ON o.`custID` = b.`CustID` AND o.`brandID` = b.`ID` 
            '                       Where s.custid = " & custID & " And BusiDate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' and BusiDate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "'" & sOut & sOwn & sBrand & sCity &
            '                        "Group BY s.`BusiDate`, s.itemno, s.`SlsTypeNm`)AS t1 
            '                      CROSS JOIN (SELECT SUM(s.total) AS SUM FROM `cld_sumsalesdetaildaily` s LEFT JOIN cld_outlet o ON s.`CustID` = o.`custID` AND s.`olId` = o.`ID` 
            '                      LEFT JOIN `cld_brand` b ON o.`custID` = b.`CustID` AND o.`brandID` = b.`ID` WHERE s.custid = " & custID & " AND 
            '                      BusiDate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' and BusiDate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "'" & sOut & sOwn & sBrand & sCity & ") t2

            '                     UNION ALL

            '                     SELECT 'Total' AS ItemNo, '' item ,SUM(s.`Qty`) qty, SUM(s.`Total`) total, 100, '' DATE, '' SlsTypeNm , '' MONTH, 
            '                     '' QUARTER, '' YEAR, '' Name3 FROM `cld_sumsalesdetaildaily` s 
            '                     WHERE s.custid = " & custID & " AND BusiDate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' and BusiDate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "'" & sOut & sOwn & sBrand & sCity & ") a ")


            connDb.resetCmd("SELECT DATE AS 'Sales Date',Type as Channel, itemno 'Item Code',Item 'Item Name',Qty,FORMAT(total, 2,'de_DE') 'Net Sales',FORMAT(Contribution, 2,'de_DE') AS '% Contribution'  FROM
                (SELECT itemno, Item,Qty, total,(total * 100 / t2.sum) AS Contribution, Date, 
                
                CASE

                            WHEN SlsTypeNm = 'DineIn' THEN 'DINE IN'

                            WHEN SlsTypeNm = 'TakeAway' THEN 'TAKE AWAY'

                            ELSE SlsTypeNm

                        END AS 'type' " &
                "FROM " &
                "(SELECT s.ItemNo ,s.`ItemName` item,SUM(s.`Qty`) qty,SUM(s.`Total`)total,DATE_FORMAT(s.busidate,'%d-%b-%Y') DATE,s.`SlsTypeNm` " &
                "FROM `cld_sumsalesdetaildaily` s " &
                "LEFT JOIN cld_outlet o ON s.`CustID` = o.`custID` And s.`olId` = o.`ID` " &
                "LEFT JOIN `cld_brand` b ON o.`custID` = b.`CustID` AND o.`brandID` = b.`ID` " &
                "Where s.custid = " & custID & " And BusiDate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' and BusiDate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "'" & sOut & sOwn & sBrand & sCity &
                "GROUP BY s.`BusiDate`,s.itemno,s.`SlsTypeNm`)AS t1 " &
                "CROSS JOIN " &
                "(SELECT SUM(s.total) AS SUM " &
                "FROM `cld_sumsalesdetaildaily` s " &
                "LEFT JOIN cld_outlet o ON s.`CustID` = o.`custID` And s.`olId` = o.`ID` " &
                "LEFT JOIN `cld_brand` b ON o.`custID` = b.`CustID` AND o.`brandID` = b.`ID` " &
                 "Where s.custid = " & custID & " And BusiDate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' and BusiDate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "'" & sOut & sOwn & sBrand & sCity & ") t2) a

            union all
                 SELECT '','','','Total',FORMAT(SUM(Qty), 0,'de_DE'),FORMAT(SUM(total), 2,'de_DE'),FORMAT(SUM(Contribution), 2,'de_DE') FROM
             (SELECT itemno, Item,Qty, total,(total * 100 / t2.sum) AS Contribution, Date, SlsTypeNm AS Type " &
                "FROM " &
                "(SELECT s.ItemNo ,s.`ItemName` item,SUM(s.`Qty`) qty,SUM(s.`Total`)total,DATE_FORMAT(s.busidate,'%d-%b-%Y') DATE,s.`SlsTypeNm` " &
             "FROM `cld_sumsalesdetaildaily` s " &
                "LEFT JOIN cld_outlet o ON s.`CustID` = o.`custID` And s.`olId` = o.`ID` " &
                "LEFT JOIN `cld_brand` b ON o.`custID` = b.`CustID` AND o.`brandID` = b.`ID` " &
                "Where s.custid = " & custID & " And BusiDate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' and BusiDate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "'" & sOut & sOwn & sBrand & sCity &
                "GROUP BY s.`BusiDate`,s.itemno,s.`SlsTypeNm`)AS t1 " &
                "CROSS JOIN " &
                "(SELECT SUM(s.total) AS SUM " &
                "FROM `cld_sumsalesdetaildaily` s " &
                "LEFT JOIN cld_outlet o ON s.`CustID` = o.`custID` And s.`olId` = o.`ID` " &
                "LEFT JOIN `cld_brand` b ON o.`custID` = b.`CustID` AND o.`brandID` = b.`ID` " &
                 "Where s.custid = " & custID & " And BusiDate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' and BusiDate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "'" & sOut & sOwn & sBrand & sCity & ") t2) b "
            )

            connDb.addParam(dfrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(dTo, "@paramTo", MySqlDbType.DateTime)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)
            ' connDb.addParam(MemberID, "@memberID", MySqlDbType.Int32)


            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports: SelectDataTable_SlsItem")
            Err.Clear()
        End Try

        Return dsRslt
    End Function
    Private Function SelectDataTable_Monthly_Channel_gv(ByVal custID As Integer, ByVal dfrom As Date, ByVal dTo As Date, ByVal sOutlet As String, ByVal iOwn As String, ByVal iBrand As String, ByVal iCity As String, ByVal usr As String, ByVal modpriv As Integer) As DataSet

        Dim dsRslt As New DataSet
        Dim sOut As String = ""
        Dim sBrand As String = ""
        Dim sOwn As String = ""
        Dim sCity As String = ""
        Dim sSlsType As String = ""

        Try
            Dim connDb As New clsDbConn(ServerString)

            If iBrand = "0" Or iBrand = "" Then
                sBrand = ""
            Else
                sBrand = " and o.brandID in (" & iBrand & ")"
            End If

            If iOwn = "0" Or iOwn = "" Then
                sOwn = ""
            Else
                sOwn = " and o.ownID = " & iOwn
            End If
            If iCity = "0" Or iCity = "" Then
                sCity = ""
            Else
                sCity = " and o.cityID in (" & iCity & ")"
            End If

            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If sOutlet = "" Then
                    sOut = " and s.olId in (Select Store From cld_userxstore where user = '" & usr & "')"
                Else
                    sOut = " and s.olnumber in (" & sOutlet & ")"
                End If
            Else
                If sOutlet = "" Then
                    sOut = ""
                Else
                    sOut = " and s.olnumber in (" & sOutlet & ")"
                End If
            End If
            If My.Settings.gofood = "" Then
                sSlsType = ""
            Else
                sSlsType = " and s.SlsTypeNm in (" & My.Settings.gofood & ")"
            End If

            connDb.resetCmd("SELECT DATE_FORMAT(s.busidate,'%d-%b-%y') 'Sales Date',s.`olnumber` 'Store Code',s.`olName` 'Store Name', 
    
           CASE

                            WHEN s.SlsTypeNm = 'DineIn' THEN 'DINE IN'

                            WHEN s.SlsTypeNm = 'TakeAway' THEN 'TAKE AWAY'

                            ELSE s.SlsTypeNm

                        END AS 'Channel' ,

            FORMAT(SUM(s.`Total`), 0,'de_DE') 'Net Sales',DATE_FORMAT(s.busidate,'%M') AS Month, " &
            "CONCAT('Q',QUARTER(s.`BusiDate`)) Quarter, DATE_FORMAT(s.busidate,'%Y') AS Year, b.name as Brand, " &
            "FORMAT(SUM(CASE WHEN s.`SlsTypeNm`IN(" & My.Settings.gofood & ")THEN s.total ELSE 0 END), 2,'de_DE') 'Gofood Sales', " &
            "FORMAT(SUM(CASE WHEN s.`SlsTypeNm`IN(" & My.Settings.gofood & ")THEN s.total ELSE 0 END) * 100 / " &
            "(SELECT SUM(s.`Total`) " &
             "FROM `cld_sumsalesdetaildaily` s " &
            "LEFT JOIN cld_outlet o ON s.`CustID` = o.`custID` And s.`olId` = o.`ID` " &
            "LEFT JOIN `cld_brand` b ON o.`custID` = b.`CustID` And o.`brandID` = b.`ID` " &
            "Where s.custid = " & custID & " And BusiDate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' and BusiDate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "'" & sOut & sOwn & sBrand & sCity & ") " &
                   ",2,'de_DE') '% Gofood' " &
             "FROM `cld_sumsalesdetaildaily` s " &
            "LEFT JOIN cld_outlet o ON s.`CustID` = o.`custID` And s.`olId` = o.`ID` " &
            "LEFT JOIN `cld_brand` b ON o.`custID` = b.`CustID` AND o.`brandID` = b.`ID` " &
            "Where s.custid = " & custID & " And BusiDate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' and BusiDate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "'" & sOut & sOwn & sBrand & sCity &
            "GROUP BY s.`olId`,s.`BusiDate`,s.`SlsTypeNm` 
            UNION ALL
            SELECT '', '', '','','Total','',FORMAT(SUM(s.`Total`), 0,'de_DE'),
             '' ,'', " &
            "FORMAT(SUM(CASE WHEN s.`SlsTypeNm`IN(" & My.Settings.gofood & ")THEN s.total ELSE 0 END), 2,'de_DE'), " &
            "FORMAT(SUM(CASE WHEN s.`SlsTypeNm`IN(" & My.Settings.gofood & ")THEN s.total ELSE 0 END) * 100 / " &
            "(SELECT SUM(s.`Total`) " &
             "FROM `cld_sumsalesdetaildaily` s " &
            "LEFT JOIN cld_outlet o ON s.`CustID` = o.`custID` And s.`olId` = o.`ID` " &
            "LEFT JOIN `cld_brand` b ON o.`custID` = b.`CustID` And o.`brandID` = b.`ID` " &
            "Where s.custid = " & custID & " And BusiDate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' and BusiDate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "'" & sOut & sOwn & sBrand & sCity & ") " &
                   ",2,'de_DE') " &
             "FROM `cld_sumsalesdetaildaily` s " &
            "LEFT JOIN cld_outlet o ON s.`CustID` = o.`custID` And s.`olId` = o.`ID` " &
            "LEFT JOIN `cld_brand` b ON o.`custID` = b.`CustID` AND o.`brandID` = b.`ID` " &
            "Where s.custid = " & custID & " And BusiDate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' and BusiDate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "'" & sOut & sOwn & sBrand & sCity)



            connDb.addParam(dfrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(dTo, "@paramTo", MySqlDbType.DateTime)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)
            ' connDb.addParam(MemberID, "@memberID", MySqlDbType.Int32)


            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:SelectDataTable_Monthly_Channel")
            Err.Clear()
        End Try

        Return dsRslt
    End Function
    Private Function SelectDataTable_Monthly_Channel(ByVal custID As Integer, ByVal dfrom As Date, ByVal dTo As Date, ByVal sOutlet As String, ByVal iOwn As String, ByVal iBrand As String, ByVal iCity As String, ByVal usr As String, ByVal modpriv As Integer) As DataSet

        Dim dsRslt As New DataSet
        Dim sOut As String = ""
        Dim sBrand As String = ""
        Dim sOwn As String = ""
        Dim sCity As String = ""
        Dim sSlsType As String = ""

        Try
            Dim connDb As New clsDbConn(ServerString)

            If iBrand = "0" Or iBrand = "" Then
                sBrand = ""
            Else
                sBrand = " and o.brandID in (" & iBrand & ")"
            End If

            If iOwn = "0" Or iOwn = "" Then
                sOwn = ""
            Else
                sOwn = " and o.ownID = " & iOwn
            End If
            If iCity = "0" Or iCity = "" Then
                sCity = ""
            Else
                sCity = " and o.cityID in (" & iCity & ")"
            End If

            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If sOutlet = "" Then
                    sOut = " and s.olId in (Select Store From cld_userxstore where user = '" & usr & "')"
                Else
                    sOut = " and s.olnumber in (" & sOutlet & ")"
                End If
            Else
                If sOutlet = "" Then
                    sOut = ""
                Else
                    sOut = " and s.olnumber in (" & sOutlet & ")"
                End If
            End If
            If My.Settings.gofood = "" Then
                sSlsType = ""
            Else
                sSlsType = " and s.SlsTypeNm in (" & My.Settings.gofood & ")"
            End If

            'connDb.resetCmd("SELECT s.`olnumber` 'Store No',s.`olName` 'Store Name',DATE_FORMAT(s.busidate,'%d-%b-%y') Date,ROUND(SUM(s.`Total`),0) AS NetSales, s.SlsTypeNm as SalesType ,DATE_FORMAT(s.busidate,'%M') AS Month, " &
            '"CONCAT('Q',QUARTER(s.`BusiDate`)) Quarter, DATE_FORMAT(s.busidate,'%Y') AS Year, b.name as Brand, " &
            '"ROUND(SUM(CASE WHEN s.`SlsTypeNm`IN(" & My.Settings.gofood & ")THEN s.total ELSE 0 END),0) AS GofoodSales, " &
            '"ROUND(SUM(CASE WHEN s.`SlsTypeNm`IN(" & My.Settings.gofood & ")THEN s.total ELSE 0 END) * 100 / SUM(s.`Total`),2) AS Gofood " &
            ' "FROM `cld_sumsalesdetaildaily` s " &
            '"LEFT JOIN cld_outlet o ON s.`CustID` = o.`custID` And s.`olId` = o.`ID` " &
            '"LEFT JOIN `cld_brand` b ON o.`custID` = b.`CustID` AND o.`brandID` = b.`ID` " &
            '"Where s.custid = " & custID & " And BusiDate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' and BusiDate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "'" & sOut & sOwn & sBrand & sCity &
            '"GROUP BY s.`olId`,s.`BusiDate`,s.`SlsTypeNm` ")
            connDb.resetCmd("SELECT DATE_FORMAT(s.busidate,'%d-%b-%Y') 'Sales Date',s.`olnumber` 'Store Code',s.`olName` 'Store Name',

            CASE

                            WHEN s.SlsTypeNm = 'DineIn' THEN 'DINE IN'

                            WHEN s.SlsTypeNm = 'TakeAway' THEN 'TAKE AWAY'

                            ELSE s.SlsTypeNm

                        END AS 'Channel' ,

            SUM(s.`Total`) 'Net Sales',DATE_FORMAT(s.busidate,'%M') AS Month, " &
            "CONCAT('Q',QUARTER(s.`BusiDate`)) Quarter, DATE_FORMAT(s.busidate,'%Y') AS Year, b.name as Brand, " &
            "SUM(CASE WHEN s.`SlsTypeNm`IN(" & My.Settings.gofood & ")THEN s.total ELSE 0 END) 'Gofood Sales', " &
            "SUM(CASE WHEN s.`SlsTypeNm`IN(" & My.Settings.gofood & ")THEN s.total ELSE 0 END) * 100 / " &
            "(SELECT SUM(s.`Total`) " &
             "FROM `cld_sumsalesdetaildaily` s " &
            "LEFT JOIN cld_outlet o ON s.`CustID` = o.`custID` And s.`olId` = o.`ID` " &
            "LEFT JOIN `cld_brand` b ON o.`custID` = b.`CustID` And o.`brandID` = b.`ID` " &
            "Where s.custid = " & custID & " And BusiDate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' and BusiDate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "'" & sOut & sOwn & sBrand & sCity & ") " &
                   " '% Gofood' " &
             "FROM `cld_sumsalesdetaildaily` s " &
            "LEFT JOIN cld_outlet o ON s.`CustID` = o.`custID` And s.`olId` = o.`ID` " &
            "LEFT JOIN `cld_brand` b ON o.`custID` = b.`CustID` AND o.`brandID` = b.`ID` " &
            "Where s.custid = " & custID & " And BusiDate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' and BusiDate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "'" & sOut & sOwn & sBrand & sCity &
            "GROUP BY s.`olId`,s.`BusiDate`,s.`SlsTypeNm`")
            'UNION ALL
            'SELECT '','Total','',SUM(s.`Total`),
            ' '' ,'', '', '',
            '  '', " &
            '"SUM(CASE WHEN s.`SlsTypeNm`IN(" & My.Settings.gofood & ")THEN s.total ELSE 0 END), " &
            '"SUM(CASE WHEN s.`SlsTypeNm`IN(" & My.Settings.gofood & ")THEN s.total ELSE 0 END) * 100 / " &
            '"(SELECT SUM(s.`Total`) " &
            ' "FROM `cld_sumsalesdetaildaily` s " &
            '"LEFT JOIN cld_outlet o ON s.`CustID` = o.`custID` And s.`olId` = o.`ID` " &
            '"LEFT JOIN `cld_brand` b ON o.`custID` = b.`CustID` And o.`brandID` = b.`ID` " &
            '"Where s.custid = " & custID & " And BusiDate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' and BusiDate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "'" & sOut & sOwn & sBrand & sCity & ") " &
            ' "FROM `cld_sumsalesdetaildaily` s " &
            '"LEFT JOIN cld_outlet o ON s.`CustID` = o.`custID` And s.`olId` = o.`ID` " &
            '"LEFT JOIN `cld_brand` b ON o.`custID` = b.`CustID` AND o.`brandID` = b.`ID` " &
            '"Where s.custid = " & custID & " And BusiDate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' and BusiDate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "'" & sOut & sOwn & sBrand & sCity)



            connDb.addParam(dfrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(dTo, "@paramTo", MySqlDbType.DateTime)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)
            ' connDb.addParam(MemberID, "@memberID", MySqlDbType.Int32)


            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:SelectDataTable_Monthly_Channel")
            Err.Clear()
        End Try

        Return dsRslt
    End Function

    Private Function SelectDataTable_Sales_Cover(ByVal custID As Integer, ByVal dfrom As Date, ByVal dTo As Date, ByVal sOutlet As String, ByVal iOwn As String, ByVal iBrand As String, ByVal iCity As String, ByVal usr As String, ByVal modpriv As Integer) As DataSet

        Dim dsRslt As New DataSet
        Dim sOut As String = ""
        Dim sBrand As String = ""
        Dim sOwn As String = ""
        Dim sCity As String = ""
        Dim sSlsType As String = ""

        Try
            Dim connDb As New clsDbConn(ServerString)

            If iBrand = "0" Or iBrand = "" Then
                sBrand = ""
            Else
                sBrand = " and o.brandID in (" & iBrand & ")"
            End If

            If iOwn = "0" Or iOwn = "" Then
                sOwn = ""
            Else
                sOwn = " and o.ownID = " & iOwn
            End If
            If iCity = "0" Or iCity = "" Then
                sCity = ""
            Else
                sCity = " and o.cityID in (" & iCity & ")"
            End If

            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If sOutlet = "" Then
                    sOut = " and s.olId in (Select Store From cld_userxstore where user = '" & usr & "')"
                Else
                    sOut = " and s.olnumber in (" & sOutlet & ")"
                End If
            Else
                If sOutlet = "" Then
                    sOut = ""
                Else
                    sOut = " and s.olnumber in (" & sOutlet & ")"
                End If
            End If
            If My.Settings.gofood = "" Then
                sSlsType = ""
            Else
                sSlsType = " and s.SlsTypeNm in (" & My.Settings.gofood & ")"
            End If

            connDb.resetCmd("SELECT DATE_FORMAT(s.busidate,'%d-%b-%y') 'Sales Date',s.olnumber as 'Store Code',s.`olName` as 'Store Name', 
            
            CASE

                            WHEN s.SlsTypeNm = 'DineIn' THEN 'DINE IN'

                            WHEN s.SlsTypeNm = 'TakeAway' THEN 'TAKE AWAY'

                            ELSE s.SlsTypeNm

                        END AS 'Channel' , 
            SUM(s.`Subtotal`-s.`Discount`) AS 'Net Sales',
            DATE_FORMAT(s.busidate,'%M') AS Month, " &
            "CONCAT('Q',QUARTER(s.`BusiDate`)) AS Quarter, DATE_FORMAT(s.busidate,'%Y') AS Year, " &
            "b.`Name` AS Brand, SUM(s.cover) AS Cover, IFNULL(ROUND(SUM(s.`Subtotal`-s.`discount`)/SUM(s.`Cover`),0),0) AS Average , SUM(s.QtyTrans) AS Bill, IFNULL((SELECT SUM(Qty) FROM cld_sumsalesdetaildaily WHERE Busidate = s.BusiDate AND olid = s.olId AND custid = s.CustID AND SlsTypeNm = s.SlsTypeNm),0) 'Total Item' " &
             "FROM `cld_sumsalesdaily` s " &
            "Left Join cld_outlet o ON s.`CustID` = o.`custID` And s.`olId` = o.`ID` " &
            "LEFT JOIN `cld_brand` b ON o.`custID` = b.`CustID` AND o.`brandID` = b.`ID` " &
            "Where s.custid = " & custID & " And BusiDate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' and BusiDate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "'" & sOut & sOwn & sBrand & sCity &
            "GROUP BY s.`olId`,s.`BusiDate`, s.`SlsTypeNm` ")
            '"Union all " &
            '"SELECT '','Total' AS Outlet, '' Date, SUM(s.`Subtotal`-s.`Discount`) AS NetSales, '' AS SalesType, '' AS Month, " &
            '"'' AS Quarter, '' AS Year, " &
            '"'' AS Brand, SUM(s.cover) AS Cover, IFNULL(ROUND(SUM(s.`Subtotal`-s.`discount`)/SUM(s.`Cover`),0),0) AS Average, SUM(s.QtyTrans) AS Bill, SUM(IFNULL((SELECT SUM(Qty) FROM cld_sumsalesdetaildaily WHERE Busidate = s.BusiDate AND olid = s.olId AND custid = s.CustID AND SlsTypeNm = s.SlsTypeNm),0)) TotalItem " &
            '"FROM `cld_sumsalesdaily` s " &
            '"LEFT JOIN cld_outlet o ON s.`CustID` = o.`custID` AND s.`olId` = o.`ID` " &
            '"LEFT JOIN `cld_brand` b ON o.`custID` = b.`CustID` AND o.`brandID` = b.`ID` " &
            '"Where s.custid = " & custID & " And BusiDate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' and BusiDate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "'" & sOut & sOwn & sBrand & sCity)



            connDb.addParam(dfrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(dTo, "@paramTo", MySqlDbType.DateTime)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)
            ' connDb.addParam(MemberID, "@memberID", MySqlDbType.Int32)


            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:SelectDataTable_Sales_Cover")
            Err.Clear()
        End Try

        Return dsRslt
    End Function
    Private Function SelectDataTable_Sales_Cover_gv(ByVal custID As Integer, ByVal dfrom As Date, ByVal dTo As Date, ByVal sOutlet As String, ByVal iOwn As String, ByVal iBrand As String, ByVal iCity As String, ByVal usr As String, ByVal modpriv As Integer) As DataSet

        Dim dsRslt As New DataSet
        Dim sOut As String = ""
        Dim sBrand As String = ""
        Dim sOwn As String = ""
        Dim sCity As String = ""
        Dim sSlsType As String = ""

        Try
            Dim connDb As New clsDbConn(ServerString)

            If iBrand = "0" Or iBrand = "" Then
                sBrand = ""
            Else
                sBrand = " and o.brandID in (" & iBrand & ")"
            End If

            If iOwn = "0" Or iOwn = "" Then
                sOwn = ""
            Else
                sOwn = " and o.ownID = " & iOwn
            End If
            If iCity = "0" Or iCity = "" Then
                sCity = ""
            Else
                sCity = " and o.cityID in (" & iCity & ")"
            End If

            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If sOutlet = "" Then
                    sOut = " and s.olId in (Select Store From cld_userxstore where user = '" & usr & "')"
                Else
                    sOut = " and s.olnumber in (" & sOutlet & ")"
                End If
            Else
                If sOutlet = "" Then
                    sOut = ""
                Else
                    sOut = " and s.olnumber in (" & sOutlet & ")"
                End If
            End If
            If My.Settings.gofood = "" Then
                sSlsType = ""
            Else
                sSlsType = " and s.SlsTypeNm in (" & My.Settings.gofood & ")"
            End If

            connDb.resetCmd("SELECT DATE_FORMAT(s.busidate,'%d-%b-%y') 'Sales Date', s.olnumber 'Store Code',s.`olName` 'Store Name',  

            CASE

                            WHEN s.SlsTypeNm = 'DineIn' THEN 'DINE IN'

                            WHEN s.SlsTypeNm = 'TakeAway' THEN 'TAKE AWAY'

                            ELSE s.SlsTypeNm

                        END AS 'Channel' , 
            FORMAT(SUM(s.`Subtotal`-s.`Discount`), 2,'de_DE') 'Net Sales',
            DATE_FORMAT(s.busidate,'%M') AS Month, " &
            "CONCAT('Q',QUARTER(s.`BusiDate`)) AS Quarter, DATE_FORMAT(s.busidate,'%Y') AS Year, " &
            "b.`Name` AS Brand, FORMAT(SUM(s.cover), 0,'de_DE') AS Cover, FORMAT(IFNULL(SUM(s.`Subtotal`-s.`discount`)/SUM(s.`Cover`),0), 0,'de_DE') AS Average , FORMAT(SUM(s.QtyTrans), 0,'de_DE') AS Bill, FORMAT(IFNULL((SELECT SUM(Qty) FROM cld_sumsalesdetaildaily WHERE Busidate = s.BusiDate AND olid = s.olId AND custid = s.CustID AND SlsTypeNm = s.SlsTypeNm),0), 0,'de_DE') 'Total Item' " &
             "FROM `cld_sumsalesdaily` s " &
            "Left Join cld_outlet o ON s.`CustID` = o.`custID` And s.`olId` = o.`ID` " &
            "LEFT JOIN `cld_brand` b ON o.`custID` = b.`CustID` AND o.`brandID` = b.`ID` " &
            "Where s.custid = " & custID & " And BusiDate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' and BusiDate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "'" & sOut & sOwn & sBrand & sCity &
            "GROUP BY s.`olId`,s.`BusiDate`, s.`SlsTypeNm` " &
            "Union all " &
            "SELECT'' Date,'', 'Total' AS Outlet,'', FORMAT(SUM(s.`Subtotal`-s.`Discount`), 2,'de_DE') AS NetSales, '' AS Month, " &
            "'' AS Quarter, '' AS Year, " &
            "'' AS Brand, FORMAT(SUM(s.cover), 0,'de_DE') AS Cover, FORMAT(IFNULL(SUM(s.`Subtotal`-s.`discount`)/SUM(s.`Cover`),0), 0,'de_DE') AS Average, FORMAT(SUM(s.QtyTrans), 0,'de_DE') AS Bill, FORMAT(SUM(IFNULL((SELECT SUM(Qty) FROM cld_sumsalesdetaildaily WHERE Busidate = s.BusiDate AND olid = s.olId AND custid = s.CustID AND SlsTypeNm = s.SlsTypeNm),0)), 0,'de_DE') TotalItem " &
            "FROM `cld_sumsalesdaily` s " &
            "LEFT JOIN cld_outlet o ON s.`CustID` = o.`custID` AND s.`olId` = o.`ID` " &
            "LEFT JOIN `cld_brand` b ON o.`custID` = b.`CustID` AND o.`brandID` = b.`ID` " &
            "Where s.custid = " & custID & " And BusiDate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' and BusiDate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "'" & sOut & sOwn & sBrand & sCity)



            connDb.addParam(dfrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(dTo, "@paramTo", MySqlDbType.DateTime)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)
            ' connDb.addParam(MemberID, "@memberID", MySqlDbType.Int32)


            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:SelectDataTable_Sales_Cover")
            Err.Clear()
        End Try

        Return dsRslt
    End Function
    Private Function SelectDataTable_SalesVSPayment(ByVal custID As Integer, ByVal dfrom As Date, ByVal dTo As Date, ByVal sOutlet As String, ByVal iOwn As String, ByVal iBrand As String, ByVal iCity As String, ByVal usr As String, ByVal modpriv As Integer) As DataSet

        Dim dsRslt As New DataSet
        Dim sOut As String = ""
        Dim sBrand As String = ""
        Dim sOwn As String = ""
        Dim sCity As String = ""
        Dim sSlsType As String = ""

        Try
            Dim connDb As New clsDbConn(ServerString)

            If iBrand = "0" Or iBrand = "" Then
                sBrand = ""
            Else
                sBrand = " and o.brandID in (" & iBrand & ")"
            End If

            If iOwn = "0" Or iOwn = "" Then
                sOwn = ""
            Else
                sOwn = " and o.ownID = " & iOwn
            End If
            If iCity = "0" Or iCity = "" Then
                sCity = ""
            Else
                sCity = " and o.cityID in (" & iCity & ")"
            End If

            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If sOutlet = "" Then
                    sOut = " and cld_sumsalesdaily.olId in (Select Store From cld_userxstore where user = '" & usr & "')"
                Else
                    sOut = " and cld_sumsalesdaily.olnumber in (" & sOutlet & ")"
                End If
            Else
                If sOutlet = "" Then
                    sOut = ""
                Else
                    sOut = " and cld_sumsalesdaily.olnumber in (" & sOutlet & ")"
                End If
            End If
            If My.Settings.gofood = "" Then
                sSlsType = ""
            Else
                sSlsType = " and cld_sumsalesdaily.SlsTypeNm in (" & My.Settings.gofood & ")"
            End If

            '    connDb.resetCmd("SELECT DATE_FORMAT(cld_sumsalesdaily.busidate,'%d-%b-%y') as busidate, cld_sumsalesdaily.`olnumber` 'Store No',cld_sumsalesdaily.olname 'Store Name',SUM(cld_sumsalesdaily.`Subtotal`) subtotal,SUM(cld_sumsalesdaily.total)totalsm " &
            '",(SELECT SUM(total) FROM cld_sumpaymentdaily p WHERE p.busidate = cld_sumsalesdaily.busidate AND p.olid = cld_sumsalesdaily.`olId`) paymentdaily " &
            '" FROM(cld_sumsalesdaily)" &
            '"Left Join cld_outlet o On cld_sumsalesdaily.`CustID` = o.`custID` And cld_sumsalesdaily.`olId` = o.`ID` " &
            '    "LEFT JOIN `cld_brand` b On o.`custID` = b.`CustID` And o.`brandID` = b.`ID` " &
            '"WHERE cld_sumsalesdaily.busidate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' AND cld_sumsalesdaily.busidate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "' " & sOut & sOwn & sBrand & sCity &
            '" GROUP BY cld_sumsalesdaily.olnumber,cld_sumsalesdaily.busidate ORDER BY cld_sumsalesdaily.olnumber,cld_sumsalesdaily.busidate")

            connDb.resetCmd("SELECT DATE_FORMAT(cld_sumsalesdaily.busidate,'%d-%b-%Y') 'Sales Date', cld_sumsalesdaily.`olnumber` 'Store Code',cld_sumsalesdaily.olname 'Store Name',SUM(cld_sumsalesdaily.`Subtotal`-cld_sumsalesdaily.`Discount`) 'Net Sales',SUM(cld_sumsalesdaily.total) 'Gross Sales' " &
        ",(SELECT SUM(total) FROM cld_sumpaymentdaily p WHERE p.busidate = cld_sumsalesdaily.busidate AND p.olid = cld_sumsalesdaily.`olId`) 'Total Payment' " &
        " FROM(cld_sumsalesdaily)" &
        "Left Join cld_outlet o On cld_sumsalesdaily.`CustID` = o.`custID` And cld_sumsalesdaily.`olId` = o.`ID` " &
            "LEFT JOIN `cld_brand` b On o.`custID` = b.`CustID` And o.`brandID` = b.`ID` " &
        "WHERE cld_sumsalesdaily.busidate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' AND cld_sumsalesdaily.busidate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "' " & sOut & sOwn & sBrand & sCity &
        " GROUP BY cld_sumsalesdaily.olnumber,cld_sumsalesdaily.busidate ORDER BY cld_sumsalesdaily.olnumber,cld_sumsalesdaily.busidate ") ' &
            '"UNION All " &
            '"SELECT '', '', 'Total',SUM(cld_sumsalesdaily.`Subtotal`),SUM(cld_sumsalesdaily.total) " &
            '",(SELECT SUM(total) FROM cld_sumpaymentdaily p WHERE p.busidate = cld_sumsalesdaily.busidate AND p.olid = cld_sumsalesdaily.`olId`) " &
            '" FROM(cld_sumsalesdaily)" &
            '"Left Join cld_outlet o On cld_sumsalesdaily.`CustID` = o.`custID` And cld_sumsalesdaily.`olId` = o.`ID` " &
            '    "LEFT JOIN `cld_brand` b On o.`custID` = b.`CustID` And o.`brandID` = b.`ID` " &
            '"WHERE cld_sumsalesdaily.busidate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' AND cld_sumsalesdaily.busidate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "' " & sOut & sOwn & sBrand & sCity)

            connDb.addParam(dfrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(dTo, "@paramTo", MySqlDbType.DateTime)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)
            ' connDb.addParam(MemberID, "@memberID", MySqlDbType.Int32)


            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:SelectDataTable_SalesVSPayment")
            Err.Clear()
        End Try

        Return dsRslt
    End Function

    Private Function SelectDataTable_SalesVSPayment_gv(ByVal custID As Integer, ByVal dfrom As Date, ByVal dTo As Date, ByVal sOutlet As String, ByVal iOwn As String, ByVal iBrand As String, ByVal iCity As String, ByVal usr As String, ByVal modpriv As Integer) As DataSet

        Dim dsRslt As New DataSet
        Dim sOut As String = ""
        Dim sBrand As String = ""
        Dim sOwn As String = ""
        Dim sCity As String = ""
        Dim sSlsType As String = ""

        Try
            Dim connDb As New clsDbConn(ServerString)

            If iBrand = "0" Or iBrand = "" Then
                sBrand = ""
            Else
                sBrand = " and o.brandID in (" & iBrand & ")"
            End If

            If iOwn = "0" Or iOwn = "" Then
                sOwn = ""
            Else
                sOwn = " and o.ownID = " & iOwn
            End If
            If iCity = "0" Or iCity = "" Then
                sCity = ""
            Else
                sCity = " and o.cityID in (" & iCity & ")"
            End If

            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If sOutlet = "" Then
                    sOut = " and cld_sumsalesdaily.olId in (Select Store From cld_userxstore where user = '" & usr & "')"
                Else
                    sOut = " and cld_sumsalesdaily.olnumber in (" & sOutlet & ")"
                End If
            Else
                If sOutlet = "" Then
                    sOut = ""
                Else
                    sOut = " and cld_sumsalesdaily.olnumber in (" & sOutlet & ")"
                End If
            End If
            If My.Settings.gofood = "" Then
                sSlsType = ""
            Else
                sSlsType = " and cld_sumsalesdaily.SlsTypeNm in (" & My.Settings.gofood & ")"
            End If

            '    connDb.resetCmd("(SELECT DATE_FORMAT(cld_sumsalesdaily.busidate,'%d-%b-%Y') 'Sales Date', cld_sumsalesdaily.`olnumber` 'Store Code',cld_sumsalesdaily.olname 'Store Name',FORMAT(SUM(cld_sumsalesdaily.`Subtotal`), 2,'de_DE') 'Net Sales',FORMAT(SUM(cld_sumsalesdaily.total), 2,'de_DE') 'Gross Sales' " &
            '",FORMAT((SELECT SUM(total) FROM cld_sumpaymentdaily p WHERE p.busidate = cld_sumsalesdaily.busidate AND p.olid = cld_sumsalesdaily.`olId`), 2,'de_DE') 'Total Payment' " &
            '" FROM(cld_sumsalesdaily)" &
            '"Left Join cld_outlet o On cld_sumsalesdaily.`CustID` = o.`custID` And cld_sumsalesdaily.`olId` = o.`ID` " &
            '    "LEFT JOIN `cld_brand` b On o.`custID` = b.`CustID` And o.`brandID` = b.`ID` " &
            '"WHERE cld_sumsalesdaily.busidate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' AND cld_sumsalesdaily.busidate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "' " & sOut & sOwn & sBrand & sCity &
            '" GROUP BY cld_sumsalesdaily.olnumber,cld_sumsalesdaily.busidate ORDER BY cld_sumsalesdaily.olnumber,cld_sumsalesdaily.busidate) " &
            '"UNION All " &
            '"SELECT '', '', 'Total',FORMAT(SUM(cld_sumsalesdaily.`Subtotal`), 2,'de_DE') 'Net Sales',FORMAT(SUM(cld_sumsalesdaily.total), 2,'de_DE') 'Gross Sales' " &
            '",FORMAT((SELECT SUM(total) FROM cld_sumpaymentdaily p WHERE p.busidate = cld_sumsalesdaily.busidate AND p.olid = cld_sumsalesdaily.`olId`), 2,'de_DE') 'Total Payment' " &
            '" FROM(cld_sumsalesdaily)" &
            '"Left Join cld_outlet o On cld_sumsalesdaily.`CustID` = o.`custID` And cld_sumsalesdaily.`olId` = o.`ID` " &
            '    "LEFT JOIN `cld_brand` b On o.`custID` = b.`CustID` And o.`brandID` = b.`ID` " &
            '"WHERE cld_sumsalesdaily.busidate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' AND cld_sumsalesdaily.busidate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "' " & sOut & sOwn & sBrand & sCity)

            connDb.resetCmd("(SELECT DATE_FORMAT(cld_sumsalesdaily.busidate,'%d-%b-%Y') 'Sales Date', cld_sumsalesdaily.`olnumber` 'Store Code',cld_sumsalesdaily.olname 'Store Name',FORMAT(SUM(cld_sumsalesdaily.`Subtotal`-cld_sumsalesdaily.`Discount`), 2,'de_DE') 'Net Sales',FORMAT(SUM(cld_sumsalesdaily.total), 2,'de_DE') 'Gross Sales' " &
        ",FORMAT((SELECT SUM(total) FROM cld_sumpaymentdaily p WHERE p.busidate = cld_sumsalesdaily.busidate AND p.olid = cld_sumsalesdaily.`olId`), 2,'de_DE') 'Total Payment' " &
        " FROM(cld_sumsalesdaily)" &
        "Left Join cld_outlet o On cld_sumsalesdaily.`CustID` = o.`custID` And cld_sumsalesdaily.`olId` = o.`ID` " &
            "LEFT JOIN `cld_brand` b On o.`custID` = b.`CustID` And o.`brandID` = b.`ID` " &
        "WHERE cld_sumsalesdaily.busidate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' AND cld_sumsalesdaily.busidate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "' " & sOut & sOwn & sBrand & sCity &
        " GROUP BY cld_sumsalesdaily.olnumber,cld_sumsalesdaily.busidate ORDER BY cld_sumsalesdaily.olnumber,cld_sumsalesdaily.busidate) " &
        "UNION All " &
        "SELECT '', '', 'Total',FORMAT(SUM(netsales), 2,'de_DE') 'Net Sales',FORMAT(SUM(GrossSales), 2,'de_DE') 'Gross Sales' " &
        ",FORMAT(SUM(TotalPayment), 2,'de_DE') 'Total Payment' " &
        " FROM(SELECT DATE_FORMAT(cld_sumsalesdaily.busidate,'%d-%b-%Y'), cld_sumsalesdaily.`olnumber`,cld_sumsalesdaily.olname,SUM(cld_sumsalesdaily.`Subtotal`-cld_sumsalesdaily.`Discount`) 'NetSales',SUM(cld_sumsalesdaily.total) 'GrossSales' " &
        ",(SELECT SUM(total) FROM cld_sumpaymentdaily p WHERE p.busidate = cld_sumsalesdaily.busidate AND p.olid = cld_sumsalesdaily.`olId`) 'TotalPayment' " &
        " FROM(cld_sumsalesdaily)" &
        "Left Join cld_outlet o On cld_sumsalesdaily.`CustID` = o.`custID` And cld_sumsalesdaily.`olId` = o.`ID` " &
            "LEFT JOIN `cld_brand` b On o.`custID` = b.`CustID` And o.`brandID` = b.`ID` " &
        "WHERE cld_sumsalesdaily.busidate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' AND cld_sumsalesdaily.busidate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "' " & sOut & sOwn & sBrand & sCity &
        " GROUP BY cld_sumsalesdaily.olnumber,cld_sumsalesdaily.busidate ORDER BY cld_sumsalesdaily.olnumber,cld_sumsalesdaily.busidate) a")


            connDb.addParam(dfrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(dTo, "@paramTo", MySqlDbType.DateTime)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)
            ' connDb.addParam(MemberID, "@memberID", MySqlDbType.Int32)


            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports: SelectDataTable_SalesVSPayment")
            Err.Clear()
        End Try

        Return dsRslt
    End Function
    Private Function Payment_Report(ByVal paramFrom As Date, ByVal paramTo As Date, ByVal Outletstr As String, ByVal iOwn As String, ByVal iBrand As String, ByVal iCity As String, ByVal custID As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim dsRslt As New DataSet
        Dim sOut As String = ""
        Dim sBrand As String = ""
        Dim sOwn As String = ""
        Dim sCity As String = ""
        Try
            Dim connDb As New clsDbConn(ServerString)

            If iBrand = "0" Or iBrand = "" Then
                sBrand = ""
            Else
                sBrand = " and o.brandID in (" & iBrand & ")"
            End If

            If iOwn = "0" Or iOwn = "" Then
                sOwn = ""
            Else
                sOwn = " and o.ownID = " & iOwn
            End If
            If iCity = "0" Or iCity = "" Then
                sCity = ""
            Else
                sCity = " and o.cityID in (" & iCity & ")"
            End If

            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If Outletstr = "" Then
                    sOut = " and p.outletid in (Select Store From cld_userxstore where user = '" & usr & "')"
                Else
                    sOut = " and o.kode in (" & Outletstr & ")"
                End If
            Else
                If Outletstr = "" Then
                    sOut = ""
                Else
                    sOut = " and o.kode in (" & Outletstr & ")"
                End If
            End If

            'connDb.resetCmd("SELECT o.`Kode` 'Store No',o.nama 'Store Name',p.`BusiDate`,p.`smNo` 'Sales No',PaymentName 'Payment Name',total,refno,notes " &
            '             "FROM cld_payment p " &
            '                    "INNER JOIN cld_outlet o ON p.outletid = o.`ID` AND p.`custid` = o.`custid` " &
            '                    "where p.flag=1 and busiDate >=@paramFrom And busiDate <=@paramTo " &
            '                    "AND (p.CustID=@custID OR @custID =0) " & sOut & sOwn & sBrand & sCity & " order BY o.kode, p.busidate, p.smno")
            connDb.resetCmd("SELECT DATE_FORMAT(p.`BusiDate`,'%d-%b-%Y') 'Sales Date',o.`Kode` 'Store Code',o.nama 'Store Name',p.`smNo` 'Sales Number',PaymentName  'Payment Name',total 'Payment Total',refno 'Reference Number',Notes " &
                         "FROM cld_payment p " &
                                "INNER JOIN cld_outlet o ON p.outletid = o.`ID` AND p.`custid` = o.`custid` " &
                                "where p.flag=1 and busiDate >=@paramFrom And busiDate <=@paramTo " &
                                "AND (p.CustID=@custID OR @custID =0) " & sOut & sOwn & sBrand & sCity & " order BY o.kode, p.busidate, p.smno ") '&
            '       "UNION ALL " &
            '       "Select '','','','','Total',sum(total),'','' " &
            '"FROM cld_payment p " &
            '       "INNER JOIN cld_outlet o ON p.outletid = o.`ID` AND p.`custid` = o.`custid` " &
            '       "where p.flag=1 and busiDate >=@paramFrom And busiDate <=@paramTo " &
            '       "AND (p.CustID=@custID OR @custID =0) " & sOut & sOwn & sBrand & sCity)

            connDb.addParam(paramFrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(paramTo, "@paramTo", MySqlDbType.DateTime)
            'connDb.addParam(outletNo, "@outletNo", MySqlDbType.VarChar)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)

            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:Payment_Report")
            Err.Clear()
        End Try

        Return dsRslt
    End Function
    Private Function Payment_Report_gv(ByVal paramFrom As Date, ByVal paramTo As Date, ByVal Outletstr As String, ByVal iOwn As String, ByVal iBrand As String, ByVal iCity As String, ByVal custID As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim dsRslt As New DataSet
        Dim sOut As String = ""
        Dim sBrand As String = ""
        Dim sOwn As String = ""
        Dim sCity As String = ""
        Try
            Dim connDb As New clsDbConn(ServerString)

            If iBrand = "0" Or iBrand = "" Then
                sBrand = ""
            Else
                sBrand = " and o.brandID in (" & iBrand & ")"
            End If

            If iOwn = "0" Or iOwn = "" Then
                sOwn = ""
            Else
                sOwn = " and o.ownID = " & iOwn
            End If
            If iCity = "0" Or iCity = "" Then
                sCity = ""
            Else
                sCity = " and o.cityID in (" & iCity & ")"
            End If

            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If Outletstr = "" Then
                    sOut = " and p.outletid in (Select Store From cld_userxstore where user = '" & usr & "')"
                Else
                    sOut = " and o.kode in (" & Outletstr & ")"
                End If
            Else
                If Outletstr = "" Then
                    sOut = ""
                Else
                    sOut = " and o.kode in (" & Outletstr & ")"
                End If
            End If

            connDb.resetCmd("(SELECT DATE_FORMAT(p.`BusiDate`,'%d-%b-%Y') 'Sales Date',o.`Kode` 'Store Code',o.nama 'Store Name',p.`smNo` 'Sales Number',PaymentName  'Payment Name',FORMAT(total, 2,'de_DE') 'Payment Total',refno 'Reference Number',Notes " &
                         "FROM cld_payment p " &
                                "INNER JOIN cld_outlet o ON p.outletid = o.`ID` AND p.`custid` = o.`custid` " &
                                "where p.flag=1 and busiDate >=@paramFrom And busiDate <=@paramTo " &
                                "AND (p.CustID=@custID OR @custID =0) " & sOut & sOwn & sBrand & sCity & " order BY o.kode, p.busidate, p.smno) " &
                                "UNION ALL " &
                                "Select '','','','','Total',FORMAT(sum(total), 2,'de_DE'),'','' " &
                         "FROM cld_payment p " &
                                "INNER JOIN cld_outlet o ON p.outletid = o.`ID` AND p.`custid` = o.`custid` " &
                                "where p.flag=1 and busiDate >=@paramFrom And busiDate <=@paramTo " &
                                "AND (p.CustID=@custID OR @custID =0) " & sOut & sOwn & sBrand & sCity)

            connDb.addParam(paramFrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(paramTo, "@paramTo", MySqlDbType.DateTime)
            'connDb.addParam(outletNo, "@outletNo", MySqlDbType.VarChar)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)

            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:Payment_Report")
            Err.Clear()
        End Try

        Return dsRslt
    End Function
    Private Function SelectDataTable_ItemGroup(ByVal custID As Integer, ByVal dfrom As Date, ByVal dTo As Date, ByVal sOutlet As String, ByVal iOwn As String, ByVal iBrand As String, ByVal iCity As String, ByVal usr As String, ByVal modpriv As Integer) As DataSet

        Dim dsRslt As New DataSet
        Dim sOut As String = ""
        Dim sBrand As String = ""
        Dim sOwn As String = ""
        Dim sCity As String = ""
        Dim sSlsType As String = ""

        Try
            Dim connDb As New clsDbConn(ServerString)

            If iBrand = "0" Or iBrand = "" Then
                sBrand = ""
            Else
                sBrand = " and o.brandID in (" & iBrand & ")"
            End If

            If iOwn = "0" Or iOwn = "" Then
                sOwn = ""
            Else
                sOwn = " and o.ownID = " & iOwn
            End If
            If iCity = "0" Or iCity = "" Then
                sCity = ""
            Else
                sCity = " and o.cityID in (" & iCity & ")"
            End If

            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If sOutlet = "" Then
                    sOut = " and s.olId in (Select Store From cld_userxstore where user = '" & usr & "')"
                Else
                    sOut = " and s.olnumber in (" & sOutlet & ")"
                End If
            Else
                If sOutlet = "" Then
                    sOut = ""
                Else
                    sOut = " and s.olnumber in (" & sOutlet & ")"
                End If
            End If
            If My.Settings.gofood = "" Then
                sSlsType = ""
            Else
                sSlsType = " and s.SlsTypeNm in (" & My.Settings.gofood & ")"
            End If

            'connDb.resetCmd("SELECT o.Kode AS StoreNo " &
            '                     ",o.Nama AS StoreName,s.itemGroup as 'Item Group' " &
            '                     ",SUM(s.Qty) AS qty " &
            '                     ",SUM(s.total) AS subtotal " &
            '     "FROM `cld_sumsalesdetaildaily` s " &
            '    "LEFT JOIN cld_outlet o ON s.`CustID` = o.`custID` And s.`olId` = o.`ID` " &
            '    "LEFT JOIN `cld_brand` b ON o.`custID` = b.`CustID` AND o.`brandID` = b.`ID` " &
            '    "Where s.custid = " & custID & " And BusiDate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' and BusiDate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "'" & sOut & sOwn & sBrand & sCity &
            '      "GROUP BY o.Kode,s.itemGroup order by o.id,s.itemGroup")

            connDb.resetCmd("(SELECT o.Kode AS 'Store Code' ,o.Nama AS 'Store Name',s.itemGroup AS 'Item Group' ,SUM(s.Qty) AS Qty ,
                                SUM(s.total) AS 'Net Sales' FROM `cld_sumsalesdetaildaily` s 
                                Left JOIN cld_outlet o ON s.`CustID` = o.`custID` And s.`olId` = o.`ID` 
                                Left Join `cld_brand` b ON o.`custID` = b.`CustID` And o.`brandID` = b.`ID` 
                                WHERE(s.custid = @CustID Or @CustID =0) And BusiDate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' and BusiDate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "'" & sOut & sOwn & sBrand & sCity &
                                " Group BY o.Kode, s.itemGroup ORDER BY o.id, s.itemGroup)  

                                UNION ALL SELECT '' AS olnumber ,'' AS Outlet,'Total' ,SUM(s.Qty) AS qty ,
                                            SUM(s.total) FROM `cld_sumsalesdetaildaily` s 
                                Left JOIN cld_outlet o ON s.`CustID` = o.`custID` And s.`olId` = o.`ID` 
                                Left Join `cld_brand` b ON o.`custID` = b.`CustID` And o.`brandID` = b.`ID` 
                                WHERE(s.custid = @CustID Or @CustID =0) And BusiDate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' and BusiDate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "'" & sOut & sOwn & sBrand & sCity)



            connDb.addParam(dfrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(dTo, "@paramTo", MySqlDbType.DateTime)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)
            ' connDb.addParam(MemberID, "@memberID", MySqlDbType.Int32)


            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports: SelectDataTable_SlsItem")
            Err.Clear()
        End Try

        Return dsRslt
    End Function
    Private Function SelectDataTable_ItemGroup_gv(ByVal custID As Integer, ByVal dfrom As Date, ByVal dTo As Date, ByVal sOutlet As String, ByVal iOwn As String, ByVal iBrand As String, ByVal iCity As String, ByVal usr As String, ByVal modpriv As Integer) As DataSet

        Dim dsRslt As New DataSet
        Dim sOut As String = ""
        Dim sBrand As String = ""
        Dim sOwn As String = ""
        Dim sCity As String = ""
        Dim sSlsType As String = ""

        Try
            Dim connDb As New clsDbConn(ServerString)

            If iBrand = "0" Or iBrand = "" Then
                sBrand = ""
            Else
                sBrand = " and o.brandID in (" & iBrand & ")"
            End If

            If iOwn = "0" Or iOwn = "" Then
                sOwn = ""
            Else
                sOwn = " and o.ownID = " & iOwn
            End If
            If iCity = "0" Or iCity = "" Then
                sCity = ""
            Else
                sCity = " and o.cityID in (" & iCity & ")"
            End If

            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If sOutlet = "" Then
                    sOut = " and s.olId in (Select Store From cld_userxstore where user = '" & usr & "')"
                Else
                    sOut = " and s.olnumber in (" & sOutlet & ")"
                End If
            Else
                If sOutlet = "" Then
                    sOut = ""
                Else
                    sOut = " and s.olnumber in (" & sOutlet & ")"
                End If
            End If
            If My.Settings.gofood = "" Then
                sSlsType = ""
            Else
                sSlsType = " and s.SlsTypeNm in (" & My.Settings.gofood & ")"
            End If

            'connDb.resetCmd("(SELECT o.Kode AS StoreNo " &
            '                     ",o.Nama AS StoreName,s.itemGroup as 'Item Group' " &
            '                     ",SUM(s.Qty) AS qty " &
            '                     ",FORMAT(SUM(s.total), 2,'de_DE') AS subtotal " &
            '     "FROM `cld_sumsalesdetaildaily` s " &
            '    "LEFT JOIN cld_outlet o ON s.`CustID` = o.`custID` And s.`olId` = o.`ID` " &
            '    "LEFT JOIN `cld_brand` b ON o.`custID` = b.`CustID` AND o.`brandID` = b.`ID` " &
            '    "Where s.custid = " & custID & " And BusiDate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' and BusiDate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "'" & sOut & sOwn & sBrand & sCity &
            '    "GROUP BY o.Kode,s.itemGroup order by o.id,s.itemGroup) " &
            '    " UNION ALL " &
            '                "SELECT '' AS olnumber " &
            '                     ",'' AS Outlet,'Total' " &
            '                     ",SUM(s.Qty) AS qty " &
            '                     ",FORMAT(SUM(s.total), 2,'de_DE') AS subtotal " &
            '     "FROM `cld_sumsalesdetaildaily` s " &
            '    "LEFT JOIN cld_outlet o ON s.`CustID` = o.`custID` And s.`olId` = o.`ID` " &
            '    "LEFT JOIN `cld_brand` b ON o.`custID` = b.`CustID` AND o.`brandID` = b.`ID` " &
            '    "Where s.custid = " & custID & " And BusiDate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' and BusiDate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "'" & sOut & sOwn & sBrand & sCity)
            connDb.resetCmd("(SELECT o.Kode AS 'Store Code' ,o.Nama AS 'Store Name',s.itemGroup AS 'Item Group' ,FORMAT(SUM(s.Qty), 0,'de_DE') AS Qty ,
                                Format(SUM(s.total), 2,'de_DE') AS 'Net Sales' FROM `cld_sumsalesdetaildaily` s 
                                Left JOIN cld_outlet o ON s.`CustID` = o.`custID` And s.`olId` = o.`ID` 
                                Left Join `cld_brand` b ON o.`custID` = b.`CustID` And o.`brandID` = b.`ID` 
                                WHERE(s.custid = @CustID Or @CustID =0) And BusiDate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' and BusiDate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "'" & sOut & sOwn & sBrand & sCity &
                                " Group BY o.Kode, s.itemGroup ORDER BY o.id, s.itemGroup)  

                                UNION ALL SELECT '' AS olnumber ,'' AS Outlet,'Total' ,FORMAT(SUM(s.Qty), 0,'de_DE') AS qty ,
                                            Format(SUM(s.total), 2,'de_DE') FROM `cld_sumsalesdetaildaily` s 
                                Left JOIN cld_outlet o ON s.`CustID` = o.`custID` And s.`olId` = o.`ID` 
                                Left Join `cld_brand` b ON o.`custID` = b.`CustID` And o.`brandID` = b.`ID` 
                                WHERE(s.custid = @CustID Or @CustID =0) And BusiDate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' and BusiDate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "'" & sOut & sOwn & sBrand & sCity)


            connDb.addParam(dfrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(dTo, "@paramTo", MySqlDbType.DateTime)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)
            ' connDb.addParam(MemberID, "@memberID", MySqlDbType.Int32)


            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports: SelectDataTable_SlsItem")
            Err.Clear()
        End Try

        Return dsRslt
    End Function
    'Private Function SelectDataTable_TopItemAmt(ByVal custID As Integer, ByVal dfrom As Date, ByVal dTo As Date, ByVal sOutlet As String, ByVal iOwn As String, ByVal iBrand As String, ByVal iCity As String, ByVal usr As String, ByVal modpriv As Integer) As DataSet

    '    Dim dsRslt As New DataSet
    '    Dim sOut As String = ""
    '    Dim sBrand As String = ""
    '    Dim sOwn As String = ""
    '    Dim sCity As String = ""
    '    Dim sSlsType As String = ""

    '    Try
    '        Dim connDb As New clsDbConn(ServerString)

    '        If iBrand = "0" Or iBrand = "" Then
    '            sBrand = ""
    '        Else
    '            sBrand = " and o.brandID in (" & iBrand & ")"
    '        End If

    '        If iOwn = "0" Or iOwn = "" Then
    '            sOwn = ""
    '        Else
    '            sOwn = " and o.ownID = " & iOwn
    '        End If
    '        If iCity = "0" Or iCity = "" Then
    '            sCity = ""
    '        Else
    '            sCity = " and o.cityID in (" & iCity & ")"
    '        End If

    '        If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
    '            If sOutlet = "" Then
    '                sOut = " and s.olId in (Select Store From cld_userxstore where user = '" & usr & "')"
    '            Else
    '                sOut = " and s.olnumber in (" & sOutlet & ")"
    '            End If
    '        Else
    '            If sOutlet = "" Then
    '                sOut = ""
    '            Else
    '                sOut = " and s.olnumber in (" & sOutlet & ")"
    '            End If
    '        End If
    '        If My.Settings.gofood = "" Then
    '            sSlsType = ""
    '        Else
    '            sSlsType = " and s.SlsTypeNm in (" & My.Settings.gofood & ")"
    '        End If

    '        connDb.resetCmd("SELECT s.ItemNo, s.itemName " &
    '                               ",s.itemGroup " &
    '                               ",SUM(s.Qty) AS qty " &
    '                               ",SUM(s.total) AS subtotal " &
    '             "FROM `cld_sumsalesdetaildaily` s " &
    '            "LEFT JOIN cld_outlet o ON s.`CustID` = o.`custID` And s.`olId` = o.`ID` " &
    '            "LEFT JOIN `cld_brand` b ON o.`custID` = b.`CustID` AND o.`brandID` = b.`ID` " &
    '            "Where s.custid = " & custID & " And BusiDate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' and BusiDate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "'" & sOut & sOwn & sBrand & sCity &
    '            "GROUP BY s.ItemNo, s.itemName, s.itemGroup " &
    '                           "order by sum(s.total) desc " &
    '                           "limit 10")



    '        connDb.addParam(dfrom, "@paramFrom", MySqlDbType.DateTime)
    '        connDb.addParam(dTo, "@paramTo", MySqlDbType.DateTime)
    '        connDb.addParam(custID, "@custID", MySqlDbType.Int32)
    '        ' connDb.addParam(MemberID, "@memberID", MySqlDbType.Int32)


    '        dsRslt = connDb.selectDataset

    '        connDb.dispose()
    '        connDb = Nothing

    '    Catch ex As Exception
    '        errLog(Err.Description, "ClsReports:SelectDataTable_TopItemAmt")
    '        Err.Clear()
    '    End Try

    '    Return dsRslt
    'End Function
    'Private Function SelectDataTable_TopItemQty(ByVal custID As Integer, ByVal dfrom As Date, ByVal dTo As Date, ByVal sOutlet As String, ByVal iOwn As String, ByVal iBrand As String, ByVal iCity As String, ByVal usr As String, ByVal modpriv As Integer) As DataSet

    '    Dim dsRslt As New DataSet
    '    Dim sOut As String = ""
    '    Dim sBrand As String = ""
    '    Dim sOwn As String = ""
    '    Dim sCity As String = ""
    '    Dim sSlsType As String = ""

    '    Try
    '        Dim connDb As New clsDbConn(ServerString)

    '        If iBrand = "0" Or iBrand = "" Then
    '            sBrand = ""
    '        Else
    '            sBrand = " and o.brandID in (" & iBrand & ")"
    '        End If

    '        If iOwn = "0" Or iOwn = "" Then
    '            sOwn = ""
    '        Else
    '            sOwn = " and o.ownID = " & iOwn
    '        End If
    '        If iCity = "0" Or iCity = "" Then
    '            sCity = ""
    '        Else
    '            sCity = " and o.cityID in (" & iCity & ")"
    '        End If

    '        If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
    '            If sOutlet = "" Then
    '                sOut = " and s.olId in (Select Store From cld_userxstore where user = '" & usr & "')"
    '            Else
    '                sOut = " and s.olnumber in (" & sOutlet & ")"
    '            End If
    '        Else
    '            If sOutlet = "" Then
    '                sOut = ""
    '            Else
    '                sOut = " and s.olnumber in (" & sOutlet & ")"
    '            End If
    '        End If
    '        If My.Settings.gofood = "" Then
    '            sSlsType = ""
    '        Else
    '            sSlsType = " and s.SlsTypeNm in (" & My.Settings.gofood & ")"
    '        End If

    '        connDb.resetCmd("SELECT s.ItemNo, s.itemName " &
    '                               ",s.itemGroup " &
    '                               ",SUM(s.Qty) AS qty " &
    '                               ",SUM(s.total) AS subtotal " &
    '             "FROM `cld_sumsalesdetaildaily` s " &
    '            "LEFT JOIN cld_outlet o ON s.`CustID` = o.`custID` And s.`olId` = o.`ID` " &
    '            "LEFT JOIN `cld_brand` b ON o.`custID` = b.`CustID` AND o.`brandID` = b.`ID` " &
    '            "Where s.custid = " & custID & " And BusiDate >= '" & dfrom.ToString("yyyy-MM-dd HH:mm:ss") & "' and BusiDate <= '" & dTo.ToString("yyyy-MM-dd HH:mm:ss") & "'" & sOut & sOwn & sBrand & sCity &
    '            "GROUP BY s.ItemNo, s.itemName, s.itemGroup " &
    '                           "order by sum(s.Qty) desc " &
    '                           "limit 10")



    '        connDb.addParam(dfrom, "@paramFrom", MySqlDbType.DateTime)
    '        connDb.addParam(dTo, "@paramTo", MySqlDbType.DateTime)
    '        connDb.addParam(custID, "@custID", MySqlDbType.Int32)
    '        ' connDb.addParam(MemberID, "@memberID", MySqlDbType.Int32)


    '        dsRslt = connDb.selectDataset

    '        connDb.dispose()
    '        connDb = Nothing

    '    Catch ex As Exception
    '        errLog(Err.Description, "ClsReports:SelectDataTable_TopItemQty")
    '        Err.Clear()
    '    End Try

    '    Return dsRslt
    'End Function
    Private Function topStore_visit(ByVal paramFrom As Date, ByVal paramTo As Date, ByVal Outletstr As String, ByVal iOwn As String, ByVal iBrand As String, ByVal iCity As String, ByVal custID As Integer, ByVal usr As String, ByVal modprv As Integer) As DataSet
        Dim dsRslt As New DataSet
        Dim sOut As String = ""
        Dim sBrand As String = ""
        Dim sOwn As String = ""
        Dim sCity As String = ""

        Try
            Dim connDB As New clsDbConn(ServerString)
            If iBrand = "0" Or iBrand = "" Then
                sBrand = ""
            Else
                sBrand = " and o.brandID in (" & iBrand & ")"
            End If

            If iOwn = "0" Or iOwn = "" Then
                sOwn = ""
            Else
                sOwn = " and o.ownID = " & iOwn
            End If
            If iCity = "0" Or iCity = "" Then
                sCity = ""
            Else
                sCity = " and o.cityID in (" & iCity & ")"
            End If

            If modprv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If Outletstr = "" Then
                    sOut = " and sm.olId in (Select Store From cld_userxstore where user = '" & usr & "')"
                Else
                    sOut = " and sm.olnumber in (" & Outletstr & ")"
                End If
            Else
                If Outletstr = "" Then
                    sOut = ""
                Else
                    sOut = " and sm.olnumber in (" & Outletstr & ")"
                End If
            End If


            connDB.resetCmd("SELECT sm.olNumber 'Store Code', sm.olName 'Store Name', sum(qtytrans) 'Number Of Transaction', SUM(total) 'Gross Sales' " &
                          "FROM cld_sumsalesdaily sm " &
                              "INNER JOIN cld_outlet o ON (o.id = sm.olid and o.Custid = sm.Custid) " &
                          "where sm.busidate >=@paramFrom AND sm.busiDate <= @paramTo " &
                              "AND (sm.CustID=@custID OR @custID=0) " & sOut & sOwn & sBrand & sCity &
                          " GROUP BY sm.olnumber,sm.olname order by sum(qtytrans) desc ")





            connDB.addParam(paramFrom, "@paramFrom", MySqlDbType.DateTime)
            connDB.addParam(paramTo, "@paramTO", MySqlDbType.DateTime)
            connDB.addParam(custID, "@custID", MySqlDbType.Int32)

            dsRslt = connDB.selectDataset

            connDB.dispose()
            connDB = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReport:topStoreAmt")
            Err.Clear()
        End Try

        Return dsRslt

    End Function

    Private Function topStore_visit_gv(ByVal paramFrom As Date, ByVal paramTo As Date, ByVal Outletstr As String, ByVal iOwn As String, ByVal iBrand As String, ByVal iCity As String, ByVal custID As Integer, ByVal usr As String, ByVal modprv As Integer) As DataSet
        Dim dsRslt As New DataSet
        Dim sOut As String = ""
        Dim sBrand As String = ""
        Dim sOwn As String = ""
        Dim sCity As String = ""

        Try
            Dim connDB As New clsDbConn(ServerString)
            If iBrand = "0" Or iBrand = "" Then
                sBrand = ""
            Else
                sBrand = " and o.brandID in (" & iBrand & ")"
            End If

            If iOwn = "0" Or iOwn = "" Then
                sOwn = ""
            Else
                sOwn = " and o.ownID = " & iOwn
            End If
            If iCity = "0" Or iCity = "" Then
                sCity = ""
            Else
                sCity = " and o.cityID in (" & iCity & ")"
            End If

            If modprv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If Outletstr = "" Then
                    sOut = " and sm.olId in (Select Store From cld_userxstore where user = '" & usr & "')"
                Else
                    sOut = " and sm.olnumber in (" & Outletstr & ")"
                End If
            Else
                If Outletstr = "" Then
                    sOut = ""
                Else
                    sOut = " and sm.olnumber in (" & Outletstr & ")"
                End If
            End If

            connDB.resetCmd("SELECT sm.olNumber 'Store Code', sm.olName 'Store Name', sum(qtytrans) 'Number Of Transaction', FORMAT(SUM(total), 2,'de_DE') 'Gross Sales' " &
                          "FROM cld_sumsalesdaily sm " &
                              "INNER JOIN cld_outlet o ON (o.id = sm.olid and o.Custid = sm.Custid) " &
                          "where sm.busidate >=@paramFrom AND sm.busiDate <= @paramTo " &
                              "AND (sm.CustID=@custID OR @custID=0) " & sOut & sOwn & sBrand & sCity &
                          " GROUP BY sm.olnumber,sm.olname order by sum(qtytrans) desc ")





            connDB.addParam(paramFrom, "@paramFrom", MySqlDbType.DateTime)
            connDB.addParam(paramTo, "@paramTO", MySqlDbType.DateTime)
            connDB.addParam(custID, "@custID", MySqlDbType.Int32)

            dsRslt = connDB.selectDataset

            connDB.dispose()
            connDB = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReport:topStoreAmt")
            Err.Clear()
        End Try

        Return dsRslt

    End Function

    Private Function kds_report(ByVal paramFrom As Date, ByVal paramTo As Date, ByVal Outletstr As String, ByVal iOwn As String, ByVal iBrand As String, ByVal iCity As String, ByVal custID As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim dsRslt As New DataSet
        Dim sOut As String = ""
        Dim sBrand As String = ""
        Dim sOwn As String = ""
        Dim sCity As String = ""

        Try
            Dim connDb As New clsDbConn(ServerString)
            If iBrand = "0" Or iBrand = "" Then
                sBrand = ""
            Else
                sBrand = " and o.brandID in (" & iBrand & ")"
            End If

            If iOwn = "0" Or iOwn = "" Then
                sOwn = ""
            Else
                sOwn = " and o.ownID = " & iOwn
            End If
            If iCity = "0" Or iCity = "" Then
                sCity = ""
            Else
                sCity = " and o.cityID in (" & iCity & ")"
            End If

            If modpriv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If Outletstr = "" Then
                    sOut = " and sm.olId in (Select Store From cld_userxstore where user = '" & usr & "')"
                Else
                    sOut = " and sm.olnumber in (" & Outletstr & ")"
                End If
            Else
                If Outletstr = "" Then
                    sOut = ""
                Else
                    sOut = " and sm.olnumber in (" & Outletstr & ")"
                End If
            End If


            connDb.resetCmd("SELECT DATE_FORMAT(Busidate, '%d %b %Y') as busidate,olnumber 'Store No',olname 'Store Name' " &
                              ",sm.Number 'Transaction No' " &
                              ",sm.ChefName 'Name Chef' " &
                              ",sm.totalitem 'Qty Item' " &
                              ",date_format(SEC_TO_TIME(sm.orderdur),'%i:%s') AS 'Duration Order' " &
                              ",date_format(SEC_TO_TIME(sm.processdur),'%i:%s') AS 'Duration Of Cook' " &
                             ",date_format(SEC_TO_TIME(IFNULL((sm.processdur/sm.totalitem),0)),'%i:%s') AS 'Average Cook' " &
                              "FROM cld_salesmaster sm INNER JOIN cld_outlet o " &
                    "ON sm.CustID = o.custID AND sm.olId = o.id " &
                          "where sm.flag = 1 and sm.busiDate >=@paramFrom AND sm.busiDate <=@paramTo " &
                          " AND (sm.CustID=@custID OR @custID=0) " & sOut & sOwn & sBrand & sCity & " GROUP BY number,olnumber, busidate, number, transdate")


            connDb.addParam(paramFrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(paramTo, "@paramTo", MySqlDbType.DateTime)
            connDb.addParam(custID, "@custID", MySqlDbType.Int32)
            'connDb.addParam(memberID, "@memberID", MySqlDbType.Int32)


            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing
        Catch ex As Exception
            errLog(Err.Description, "ClsReports:salesDetail")
            Err.Clear()
        End Try

        Return dsRslt

    End Function
    Public Function top_visit_StoreByHour_gv(ByVal paramFrom As Date, ByVal paramTo As Date, ByVal outletstr As String, ByVal ownID As String, ByVal brandstr As String, ByVal citystr As String, ByVal custID As Integer, ByVal usr As String, ByVal modprv As Integer) As DataSet
        Dim dsRslt As New DataSet
        Dim sOut As String = ""
        Dim sBrand As String = ""
        Dim sOwn As String = ""
        Dim sCity As String = ""

        Try
            Dim connDb As New clsDbConn(ServerString)
            If brandstr = "0" Or brandstr = "" Then
                sBrand = ""
            Else
                sBrand = " and o.brandID in (" & brandstr & ")"
            End If

            If ownID = "0" Or ownID = "" Then
                sOwn = ""
            Else
                sOwn = " and o.ownID = " & ownID
            End If
            If citystr = "0" Or citystr = "" Then
                sCity = ""
            Else
                sCity = " and o.cityID in (" & citystr & ")"
            End If

            If modprv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If outletstr = "" Then
                    sOut = " and sm.olId in (Select Store From cld_userxstore where user = '" & usr & "')"
                Else
                    sOut = " and sm.olnumber in (" & outletstr & ")"
                End If
            Else
                If outletstr = "" Then
                    sOut = ""
                Else
                    sOut = " and sm.olnumber in (" & outletstr & ")"
                End If
            End If

            connDb.resetCmd("(SELECT olnumber 'Store Code',olname 'Store Name',Hour, visit 'Number Of Visit', FORMAT(visit*100/allvisit, 2,'de_DE') '% Visit'
,FORMAT(gs, 0,'de_DE') 'Gross Sales',FORMAT(gs*100/allgs, 2,'de_DE') '% Gross Sales' FROM
(SELECT sm.olNumber, sm.olName, DATE_FORMAT(transdate,'%H:00') AS HOUR, sum(cover) visit,
 SUM(total) gs FROM cld_salesMaster sm INNER JOIN cld_outlet o ON(o.id = sm.olid AND o.Custid = sm.Custid) 
 WHERE sm.flag=1 AND sm.busidate >=@paramFrom AND busiDate <= @paramTo
             AND (sm.CustID=@custID OR @custID=0) " & sOut & sOwn & sBrand & sCity &
" Group BY sm.olnumber,sm.olname, HOUR(transdate) ORDER BY SUM(cover) DESC, DATE_FORMAT(transdate,'%Y:%M:%D %H:00'))AS t1
CROSS JOIN 
 (SELECT sum(cover) allvisit,SUM(total) allgs FROM cld_salesMaster sm INNER JOIN cld_outlet o ON(o.id = sm.olid AND o.Custid = sm.Custid) 
 WHERE sm.flag=1 AND sm.busidate >=@paramFrom AND busiDate <= @paramTo 
                        AND (sm.CustID=@custID OR @custID=0) " & sOut & sOwn & sBrand & sCity & ") As t2 )
 UNION ALL
 SELECT '', 'Total','', sum(cover),'100'
,FORMAT(SUM(total), 0,'de_DE'),'100' FROM cld_salesMaster sm INNER JOIN cld_outlet o ON(o.id = sm.olid AND o.Custid = sm.Custid) 
 WHERE sm.flag=1 AND sm.busidate >=@paramFrom AND busiDate <= @paramTo 
                        AND (sm.CustID=@custID OR @custID=0) " & sOut & sOwn & sBrand & sCity)

            'connDb.resetCmd("(SELECT sm.olNumber 'Store Code', sm.olName 'Store Name', DATE_FORMAT(transdate,'%H:00') AS Hour, sum(cover) 'Number Of Visit', FORMAT(SUM(total), 0,'de_DE') 'Gross Sales' " &
            '             "FROM cld_salesMaster sm " &
            '                 "INNER JOIN cld_outlet o ON(o.id = sm.olid and o.Custid = sm.Custid) " &
            '             "WHERE sm.flag=1 AND sm.busidate >=@paramFrom AND busiDate <= @paramTo " &
            '               "AND (sm.CustID=@custID OR @custID=0) " & sOut & sOwn & sBrand & sCity &
            '             " GROUP BY sm.olnumber,sm.olname, HOUR(transdate) order by sum(cover) desc, DATE_FORMAT(transdate,'%Y:%M:%D %H:00') ) ")



            connDb.addParam(paramFrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(paramTo, "@paramTo", MySqlDbType.DateTime)
            connDb.addParam(custID, "@custID", MySqlDbType.UInt32)

            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:top_visit_StoreByHour_gv")
            Err.Clear()
        End Try

        Return dsRslt

    End Function

    Public Function top_visit_StoreByHour(ByVal paramFrom As Date, ByVal paramTo As Date, ByVal outletstr As String, ByVal ownID As String, ByVal brandstr As String, ByVal citystr As String, ByVal custID As Integer, ByVal usr As String, ByVal modprv As Integer) As DataSet
        Dim dsRslt As New DataSet
        Dim sOut As String = ""
        Dim sBrand As String = ""
        Dim sOwn As String = ""
        Dim sCity As String = ""

        Try
            Dim connDb As New clsDbConn(ServerString)
            If brandstr = "0" Or brandstr = "" Then
                sBrand = ""
            Else
                sBrand = " and o.brandID in (" & brandstr & ")"
            End If

            If ownID = "0" Or ownID = "" Then
                sOwn = ""
            Else
                sOwn = " and o.ownID = " & ownID
            End If
            If citystr = "0" Or citystr = "" Then
                sCity = ""
            Else
                sCity = " and o.cityID in (" & citystr & ")"
            End If

            If modprv And Modulepriv.moduleuser = Modulepriv.moduleuser Then
                If outletstr = "" Then
                    sOut = " and sm.olId in (Select Store From cld_userxstore where user = '" & usr & "')"
                Else
                    sOut = " and sm.olnumber in (" & outletstr & ")"
                End If
            Else
                If outletstr = "" Then
                    sOut = ""
                Else
                    sOut = " and sm.olnumber in (" & outletstr & ")"
                End If
            End If

            connDb.resetCmd("(SELECT olnumber 'Store Code',olname 'Store Name',Hour, visit 'Number Of Visit', visit*100/allvisit '% Visit'
,gs 'Gross Sales',gs*100/allgs '% Gross Sales' FROM
(SELECT sm.olNumber, sm.olName, DATE_FORMAT(transdate,'%H:00') AS HOUR,sum(cover) visit,
 SUM(total) gs FROM cld_salesMaster sm INNER JOIN cld_outlet o ON(o.id = sm.olid AND o.Custid = sm.Custid) 
 WHERE sm.flag=1 AND sm.busidate >=@paramFrom AND busiDate <= @paramTo
             AND (sm.CustID=@custID OR @custID=0) " & sOut & sOwn & sBrand & sCity &
" Group BY sm.olnumber,sm.olname, HOUR(transdate) ORDER BY SUM(cover) DESC, DATE_FORMAT(transdate,'%Y:%M:%D %H:00'))AS t1
CROSS JOIN 
 (SELECT sum(cover) allvisit,SUM(total) allgs FROM cld_salesMaster sm INNER JOIN cld_outlet o ON(o.id = sm.olid AND o.Custid = sm.Custid) 
 WHERE sm.flag=1 AND sm.busidate >=@paramFrom AND busiDate <= @paramTo 
                        AND (sm.CustID=@custID OR @custID=0) " & sOut & sOwn & sBrand & sCity & ") AS t2 )")
            ' UNION ALL
            ' SELECT '', 'Total','', sum(cover),100
            ',SUM(total),100 FROM cld_salesMaster sm INNER JOIN cld_outlet o ON(o.id = sm.olid AND o.Custid = sm.Custid) 
            ' WHERE sm.flag=1 AND sm.busidate >=@paramFrom AND busiDate <= @paramTo 
            '                        AND (sm.CustID=@custID OR @custID=0) " & sOut & sOwn & sBrand & sCity)

            'connDb.resetCmd("SELECT sm.olNumber 'Store Code', sm.olName 'Store Name', DATE_FORMAT(transdate,'%H:00') AS jam, COUNT(*) AS visit, SUM(total) AS Revenue " &
            '             "FROM cld_salesMaster sm " &
            '                 "INNER JOIN cld_outlet o ON(o.id = sm.olid and o.Custid = sm.Custid) " &
            '             "WHERE sm.flag=1 AND sm.busidate >=@paramFrom AND busiDate <= @paramTo " &
            '               "AND (sm.CustID=@custID OR @custID=0) " & sOut & sOwn & sBrand & sCity &
            '             " GROUP BY sm.olnumber,sm.olname, HOUR(transdate) order by sm.olNumber, DATE_FORMAT(transdate,'%Y:%M:%D %H:00')")

            'connDb.resetCmd("(SELECT sm.olNumber 'Store Code', sm.olName 'Store Name', DATE_FORMAT(transdate,'%H:00') AS Hour, sum(cover) 'Number Of Visit', SUM(total) 'Gross Sales' " &
            '             "FROM cld_salesMaster sm " &
            '                 "INNER JOIN cld_outlet o ON(o.id = sm.olid and o.Custid = sm.Custid) " &
            '             "WHERE sm.flag=1 AND sm.busidate >=@paramFrom AND busiDate <= @paramTo " &
            '               "AND (sm.CustID=@custID OR @custID=0) " & sOut & sOwn & sBrand & sCity &
            '             " GROUP BY sm.olnumber,sm.olname, HOUR(transdate) order by sum(cover) desc, DATE_FORMAT(transdate,'%Y:%M:%D %H:00') ) ")



            connDb.addParam(paramFrom, "@paramFrom", MySqlDbType.DateTime)
            connDb.addParam(paramTo, "@paramTo", MySqlDbType.DateTime)
            connDb.addParam(custID, "@custID", MySqlDbType.UInt32)

            dsRslt = connDb.selectDataset

            connDb.dispose()
            connDb = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsReports:topStoreByHour")
            Err.Clear()
        End Try

        Return dsRslt

    End Function

End Class
