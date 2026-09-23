Imports System.Collections.Generic

Public Class Cls_PO
    Public Function SelectDataset_Supplier(ByVal custid As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""


        Dim a As New clsDbo("SELECT cld_inv_Supplier.*, cld_cust.nama as custname from cld_inv_Supplier inner join cld_cust On cld_cust.id = cld_inv_Supplier.custid  where cld_inv_Supplier.custid = " & custid & " order by name")


        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function
    Public Function Selectdt_Item_all(ByVal dta As DataTable, ByVal custid As Integer, ByVal name As String, ByVal modpriv As Integer, ByVal ccid As Integer) As DataTable
        Dim dt As New DataTable
        Dim strFilter As String = ""
        Dim strf As String = ""
        Dim str As String = ""

        Dim colBValues = New List(Of String)


        If dta Is Nothing Then
            str = "SELECT itemno, itemname FROM `cld_itemmaster`  WHERE link = 1 AND cld_itemmaster.custid = " & custid & "
                    AND itemname LIKE '%" & name & "%' ORDER BY itemname"
        Else
            If dta.Rows.Count > 0 Then
                For Each row As DataRow In dta.Rows
                    If strFilter = "" Then
                        strFilter = "'" & row.Item("itemno") & "'"
                    Else
                        strFilter = strFilter & "," & "'" & row.Item("itemno") & "'"
                    End If

                Next
                strf = "and itemno not in  (" & strFilter & ") "
            End If
            str = "SELECT itemno, itemname FROM `cld_itemmaster`  WHERE link = 1 AND cld_itemmaster.custid = " & custid & " " & strf & "  
                    AND itemname LIKE '%" & name & "%' ORDER BY itemname"
        End If



        Dim a As New clsDbo(str)

        dt = a.selectDatatable

        a.dispose()
        a = Nothing

        Return dt

    End Function
    Public Function SelectDataset_CustomerPO(ByVal custid As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""
        Try
            Dim a As New clsDbo("SELECT `cld_inv_ordercust`.*, cld_cust.nama AS custname FROM cld_inv_ordercust 
                  INNER JOIN cld_cust ON cld_cust.id = cld_inv_ordercust.custid 
                  WHERE cld_inv_ordercust.custid = " & custid & " ")

            ds = a.selectDataset

            a.dispose()
            a = Nothing
        Catch ex As Exception

        End Try
        Return ds

    End Function
    Public Function gtmaster_preorder(ByVal name As String, ByVal custid As Integer, Optional ByVal linkid As String = "") As DataTable
        Dim ds As New DataTable
        Dim strFilter As String = ""
        If linkid = "" Then
            strFilter = "SELECT * FROM `cld_salesmaster_cpo` sm
                          WHERE sm.flag = 1 AND sm.custid = " & custid & ""

        Else
            strFilter = "SELECT * FROM `cld_salesmaster_cpo` sm
                     WHERE number = '" & linkid & "' AND sm.flag = 1 AND sm.custid = " & custid & ""

        End If

        Dim a As New clsDbo(strFilter)

        ds = a.selectDatatable

        a.dispose()
        a = Nothing

        Return ds

    End Function
    Public Function preorder_search(ByVal name As String, ByVal fromdate As Date, ByVal todate As Date, ByVal custid As Integer, ByVal usrname As String, ByVal modpriv As Integer) As DataTable
        Dim ds As New DataTable
        Dim strFilter As String = ""
        If modpriv And usrModulepriv.moduleuser = usrModulepriv.moduleuser Then
            strFilter = "SELECT * FROM `cld_salesmaster_cpo` sm
                     WHERE Transdate >= '" & fromdate.ToString("yyyy-MM-dd") & "' and Transdate <= '" & todate.ToString("yyyy-MM-dd") & "' and (number Like '%" & name & "%' or custname Like '%" & name & "%' or olname Like '%" & name & "%') and custid = " & custid & " and flag = 1 
                            and olid IN (SELECT Store FROM cld_userxstore WHERE USER = '" & usrname & "')
                            ORDER BY transdate desc, number desc"
        Else
            strFilter = "SELECT * FROM `cld_salesmaster_cpo` sm
                     WHERE Transdate >= '" & fromdate.ToString("yyyy-MM-dd") & "' and Transdate <= '" & todate.ToString("yyyy-MM-dd") & "' and (number Like '%" & name & "%' or custname Like '%" & name & "%' or olname Like '%" & name & "%') and custid = " & custid & " and flag = 1 
                                                     ORDER BY transdate desc, number desc"
        End If



        Dim a As New clsDbo(strFilter)

        ds = a.selectDatatable

        a.dispose()
        a = Nothing

        Return ds

    End Function

    Public Function SelectDataTable_preorder(ByVal linkid As String, ByVal custid As Integer) As DataTable
        Dim ds As New DataTable
        Dim strFilter As String = ""


        Dim a As New clsDbo("SELECT * FROM `cld_salesdetail_cpo` WHERE smno = '" & linkid & "' AND flag = 1 AND custid = " & custid & "")

        ds = a.selectDatatable

        a.dispose()
        a = Nothing

        Return ds

    End Function

    Public Function insertPreOrder(ByVal smno As String, ByVal olid As Integer, ByVal olnumber As String, ByVal olname As String, ByVal dt As DataTable, ByVal pocust As Integer, ByVal custname As String, ByVal stat As Integer, ByVal username As String, ByVal custid As Integer, ByVal busidate As Date, ByVal notes As String, ByVal payid As Integer, ByVal payname As String, ByVal paytipe As Integer, ByVal paytotal As Decimal, ByVal taxprc As Decimal) As String
        Dim c_cmd As New clsDbo("")
        'Dim strheader As String
        'Dim strmaster As String
        Dim flagtrans As Integer
        ' Dim iacc As Integer'
        Dim pmno As String = ""
        Dim shasil As String = ""
        Dim subtotalpo As Decimal = 0
        Dim totalitem As Decimal = 0
        flagtrans = stat



        Dim doctot As Decimal = 0
        Try
            If smno = "" Then
                'klo insert baru
                Dim idtrans As String = PONumber(custid)
                Dim try2 As Integer = 0
cek1:

                If c_cmd.ExecuteScalar("SELECT COUNT(custid) FROM cld_salesmaster_cpo WHERE number='" & idtrans & "' AND custid=" & custid & " AND olid=" & olid & "", 0) > 0 Then
                    try2 = try2 + 1
                    idtrans = PONumber(custid, try2)
                    GoTo cek1
                Else
                    For Each row As DataRow In dt.Rows
                        insert_detailpo(idtrans, row.Item("itemno").ToString, row.Item("itemname").ToString, row.Item("qty").ToString, olid, custid, flagtrans, username, "preorder")
                        'subtotalpo = subtotalpo + CDec(row.Item("subtotal").ToString)
                        totalitem = totalitem + 1
                    Next

                    insert_salespo(idtrans, busidate, pocust, custname, olnumber, olname, olid, custid, flagtrans, totalitem, "")
                End If
                shasil = idtrans




            Else

                'klo Rubah atau Update
                If stat = 1 Then 'pending
                    If c_cmd.ExecuteScalar("SELECT COUNT(custid) FROM cld_salesmaster_cpo WHERE number='" & smno & "' AND custid=" & custid & " AND olid=" & olid & "", 0) > 0 Then
                        c_cmd.executeNonQuery("DELETE FROM cld_salesdetail_cpo WHERE smno='" & smno & "' AND outletid = " & olid & "")

                        For Each row As DataRow In dt.Rows
                            'insert_detailpo(smno, row.Item("itemno").ToString, row.Item("itemname").ToString, row.Item("qty").ToString, row.Item("price").ToString, row.Item("subtotal").ToString, olid, custid, flagtrans, "", "", username, "preorder", "")
                            insert_detailpo(smno, row.Item("itemno").ToString, row.Item("itemname").ToString, row.Item("qty").ToString, olid, custid, flagtrans, username, "preorder")

                            'subtotalpo = subtotalpo + CDec(row.Item("subtotal").ToString)
                            totalitem = totalitem + 1
                        Next
                        insert_salespo(smno, busidate, pocust, custname, olnumber, olname, olid, custid, flagtrans, totalitem, "")
                        'insert_salespo(smno, busidate, subtotalpo, (subtotalpo * taxprc / 100), subtotalpo + (subtotalpo * taxprc / 100), pocust, custname, olnumber, olname, olid, custid, flagtrans, (subtotalpo * taxprc / 100), 0, "")
                    End If




                End If

                '8 nov 2017, set feedback hasil = ID receivenya
                shasil = smno

            End If

            insertLogModule(username, "InsertPreorder", "Update", custid)

        Catch ex As Exception

            errLog(Err.Description, "cls_PO:InsertPreorder")
            Err.Clear()
        End Try

        c_cmd.dispose()
        c_cmd = Nothing


        Return shasil

    End Function
    Public Sub Calculasi_inventorypo(ByVal dwaktu As Date, ByVal custid As Integer)
        errLog("update USAGE", "START")
        Dim clsInv As New clsInventory
        clsInv.Process_InvUsagePO(Convert.ToDateTime(dwaktu), custid)

        errLog("update USAGECANCEL", "START")
        clsInv.process_InvUsageCancelPO()
        errLog("update USAGECANCEL", "END")



        errLog("update USAGE", "END")
    End Sub

    Public Function PONumber(ByVal custid As Integer, Optional ByVal try2 As Integer = 0) As String
        Dim str As String = ""
        Dim awal As String = "00001"
        Dim idtrans As String = SelectDS_POnumber(custid)
        Dim test As String = Val(Mid(idtrans, 7, 11))
        Dim ids As String = Val(Mid(idtrans, 7, 11)) + 1
        If try2 > 0 Then
            ids = ids + try2
        End If
        Dim mon As String = Date.Now.Month
        Dim yer As String = Mid(Date.Now.Year, 3, 4)

        If mon.Length = 1 Then
            mon = "0" & mon
        End If
        If Date.Now.Day = 1 And idtrans = "" Then
            str = "PO" & mon & yer & awal

        Else
            If ids.Length = 1 Then
                str = "PO" & mon & yer & "0000" & ids
            ElseIf ids.Length = 2 Then
                str = "PO" & mon & yer & "000" & ids
            ElseIf ids.Length = 3 Then
                str = "PO" & mon & yer & "00" & ids
            ElseIf ids.Length = 4 Then
                str = "PO" & mon & yer & "0" & ids
            ElseIf ids.Length = 5 Then

                str = "PO" & mon & yer & ids

            End If
        End If

        Return str

    End Function
    Public Function SelectDs_custPO(ByVal custid As Integer, Optional ByVal name As String = "") As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""

        strFilter = "SELECT * FROM cld_inv_ordercust 
                               WHERE cld_inv_ordercust.custid = " & custid & " 
                                and name like '%" & name & "%' or email like '%" & name & "%' or Address like '%" & name & "%' or Phone like '%" & name & "%' 
                                order by cld_inv_ordercust.name"



        Dim a As New clsDbo(strFilter)
        Try

            ds = a.selectDataset

            a.dispose()
            a = Nothing

        Catch ex As Exception

        End Try
        Return ds

    End Function
    Public Function getproperty_RcpDtlPO(ByVal Custid As Integer, ByVal stkid As Integer, ByVal dQty As Decimal, ByVal dInvDate As Date, ByVal ilinkId As Integer, ByVal iCc As Integer, ByVal sCcNm As String, Optional ByVal rcpid As Integer = 0) As Decimal
        Dim bcost As Decimal = 0
        Dim c_recal As New cls_recalculate
        Try
            Dim clsprocess As New clsInventoryProcess
            Dim cmd As New clsDbo("")

            Dim dData As New DataSet

            If rcpid = 0 Then
                dData = cmd.ReturnDataset("SELECT stkr.id kodeid,stkr.number stkno,rcpdtl.tipe,stkr.name stknm,(rcpdtl.Qty + (rcpdtl.Qty*rcpdtl.lossf/100)) qty, rcpdtl.lossf, stkr.`Factor2`,stkr.unit2,stkr.unit3
                                         FROM cld_inv_stk stk
                                        LEFT JOIN cld_inv_rcpdtl rcpdtl ON stk.blink = rcpdtl.rcpid AND stk.custid = rcpdtl.custid
                                        LEFT JOIN cld_inv_stk stkr ON stkr.id = rcpdtl.kodeid AND stkr.custid = rcpdtl.custid
                                        where rcpdtl.custid = " & Custid & " and stk.id = " & stkid)
            Else
                dData = cmd.ReturnDataset("SELECT stk.id kodeid,stk.number stkno,rcpdtl.tipe,stk.name stknm,(rcpdtl.Qty + (rcpdtl.Qty*rcpdtl.lossf/100)) qty, rcpdtl.lossf, stk.`Factor2`,stk.unit2,stk.unit3
                                         FROM cld_inv_stk stk
                                         LEFT JOIN cld_inv_rcpdtl rcpdtl ON stk.id = rcpdtl.kodeid AND stk.custid = rcpdtl.custid
                                        where rcpdtl.custid = " & Custid & " and rcpdtl.rcpid = " & rcpid)
            End If



            If dData.Tables(0).Rows.Count > 0 Then

                For x = 0 To dData.Tables(0).Rows.Count - 1
                    If Custid = 0 Then
                        Custid = cmd.ExecuteScalar("SELECT id FROM `cld_cust` LIMIT 1", 0)
                    Else
                        If Custid <> cmd.ExecuteScalar("SELECT id FROM `cld_cust` LIMIT 1", 0) Then
                            errLog("wrong custid" & " clsInventory_stk:insertlimitStk", Custid.ToString)
                            Custid = cmd.ExecuteScalar("SELECT id FROM `cld_cust` LIMIT 1", 0)
                        End If
                    End If
                    If IsDBNull(dData.Tables(0).Rows(x).Item("tipe")) = False Then

                        If dData.Tables(0).Rows(x).Item("tipe") = 1 Then
                            'link ke Stock master
                            'kalau ada lossreceipt

                            insert_preorder_usage(Custid, iCc, sCcNm, dData.Tables(0).Rows(x).Item("kodeid"), dData.Tables(0).Rows(x).Item("stkNo"), dData.Tables(0).Rows(x).Item("stkNm"),
                                                       dQty * (dData.Tables(0).Rows(x).Item("Qty")),
                                                       dData.Tables(0).Rows(x).Item("Factor2"), dData.Tables(0).Rows(x).Item("Unit2"), dData.Tables(0).Rows(x).Item("Unit3"), dInvDate.ToString("yyyy-MM-dd HH:mm:ss"), ilinkId)


                        ElseIf dData.Tables(0).Rows(x).Item("tipe") = 2 Then
                            'link ke receipe
                            'getproperty_RcpDetail(Custid, iCc, sCcNm, dData.Tables(0).Rows(0).Item("kodeid"), dQty * dData.Tables(0).Rows(0).Item("Factor"), dInvDate.ToString("yyyy-MM-dd HH:mm:ss"), ilinkId)
                            getproperty_RcpDtlPO(Custid, dData.Tables(0).Rows(x).Item("kodeid"), dQty * dData.Tables(0).Rows(x).Item("qty"), dInvDate.ToString("yyyy-MM-dd HH:mm:ss"), ilinkId, iCc, sCcNm)


                        End If

                    End If

                Next

            End If

            cmd.dispose()
            cmd = Nothing

        Catch ex As Exception
            errLog(Err.Description, "Cls_IBatch.getproperty_RcpDetail")
            Err.Clear()
        End Try

        Return bcost
    End Function
    Private Sub insert_preorder_usage(ByVal iCust As Integer, ByVal iCC As Integer, ByVal sCc As String, ByVal iStk As Integer, ByVal sStkno As String, ByVal sStknm As String, ByVal dqtyFac As Decimal, ByVal dFactor As Integer, ByVal sU2 As String, ByVal sU3 As String, ByVal dInvDate As Date, ByVal iBatchId As Integer)
        Dim c_stkCost As New clsInventory_stkCost
        Dim str As String = ""
        Try
            str = "insert into cld_inv_transusage(CustID, InvDate, type, Cc, Cc_name, Stk_Id, Stk_no, Stk_Name, U2, U3, Factor2, ttlStock, Cost, Linkid, flagtrans, U2_nm, U3_nm, CreatedBy, CreatedDate) values (" & iCust & ", '" & dInvDate.ToString("yyyy-MM-dd HH:mm:ss") & "', 11, " & iCC & ", '" & sCc & "', " & iStk & ", '" & sStkno & "', '" & sStknm & "',0 ," & dqtyFac & ", " & dFactor & ", " & CDec(dqtyFac / dFactor) & "," & CInt(c_stkCost.get_stkCost(iStk, iCC, iCust)) & ", " & iBatchId & " , 1, '" & sU2 & "','" & sU3 & "','SysAdmin','" & Now.ToString("yyyy-MM-dd HH:mm:ss") & "')"

            Dim cmdprocess As New clsDbo("")
            cmdprocess.executeNonQuery(str)

            cmdprocess = Nothing


        Catch ex As Exception
            errLog(Err.Description, str & "Cls_IBatch.insert_Batchdetail_Stk")
            Err.Clear()

        End Try

    End Sub
    Private Sub insert_salespo(ByVal smno As String, ByVal busidate As Date, ByVal ordercust As Integer, ByVal custname As String, ByVal olnumber As String, ByVal olname As String, ByVal olid As Integer, ByVal custid As Integer, ByVal flag As Integer, ByVal ttlitem As Decimal, Optional ByVal tableno As String = "")
        Dim c_stkCost As New clsInventory_stkCost
        Dim str As String = ""
        Try
            'str = "insert into cld_salesmaster_cpo(Number,TransDate,BusiDate,Subtotal,Tax,Service,Total,TotalCost,ordercust,custname,MemberTypeNm,MemberParentNO,MemberparentNm,RvcName,olNumber,
            '        olName,Cover,getPoint,UsedSaldo,Discount,olId,CustID,flag,VoidRmk,VoidAth,ppn,pb1,DiscNm,tableno) 
            '        values ('" & smno & "', '" & Now.Date.ToString("yyyy-MM-dd HH:mm:ss") & "', '" & busidate.ToString("yyyy-MM-dd HH:mm:ss") & "', " & subtotal & ", " & tax & ",0, " & total & ",0," & ordercust & ",'" & custname & "','','','','', '" & olnumber & "', '" & olname & "',1,0,0,0, " & olid & ", " & custid & "," & flag & " ,'','', " & tax & ",0,'', '" & tableno & "')"
            str = "insert into cld_salesmaster_cpo(Number,TransDate,BusiDate,ordercust,custname,olNumber,
                    olName,olId,CustID,flag) 
                    values ('" & smno & "', NOW(), '" & busidate.ToString("yyyy-MM-dd HH:mm:ss") & "'," & ordercust & ",'" & custname & "', '" & olnumber & "', '" & olname & "', " & olid & ", " & custid & "," & flag & ")"

            Dim cmdprocess As New clsDbo("")
            If cmdprocess.ExecuteScalar("SELECT count(custid) FROM cld_salesmaster_cpo WHERE number = '" & smno & "' AND olid = " & olid & "", 0) > 0 Then
                cmdprocess.executeNonQuery("DELETE FROM cld_salesmaster_cpo WHERE number = '" & smno & "' AND olid = " & olid & "")
                cmdprocess.ReNew()
            End If
            cmdprocess.executeNonQuery(str)


            cmdprocess = Nothing


        Catch ex As Exception
            errLog(Err.Description, str & "Cls_PO.insert_salespo")
            Err.Clear()

        End Try

    End Sub
    Private Sub insert_detailpo(ByVal smno As String, ByVal itemno As String, ByVal itemname As String, ByVal qty As Decimal, ByVal olid As Integer, ByVal custid As Integer, ByVal flag As Integer, ByVal userid As String, ByVal compname As String)
        Dim c_stkCost As New clsInventory_stkCost
        Dim str As String = ""
        Try
            'str = "insert into cld_salesdetail_cpo(smNo,ItemNo,ItemName,Qty,Price,Subtotal,Cost,ServeEmployee,Outletid,CustID,flag,
            '        ParentNo,ParentName,UserID,CompName,Transdate,SlsTypeNm,SlsTypePromo) 
            '        values ('" & smno & "','" & itemno & "','" & itemname & "'," & qty & "," & price & "," & subtotal & ",0,''," & olid & "," & custid & "," & flag & ",'" & ParentNo & "','" & ParentName & "','" & userid & "','" & compname & "', '" & Now.Date.ToString("yyyy-MM-dd HH:mm:ss") & "', '" & slstypenm & "',0)"

            str = "insert into cld_salesdetail_cpo(smNo,ItemNo,ItemName,Qty,Outletid,CustID,flag,
                    UserID,CompName,Transdate) 
                    values ('" & smno & "','" & itemno & "','" & itemname & "'," & qty & "," & olid & "," & custid & "," & flag & ",'" & userid & "','" & compname & "', '" & Now.Date.ToString("yyyy-MM-dd HH:mm:ss") & "')"

            Dim cmdprocess As New clsDbo("")
            If cmdprocess.ExecuteScalar("SELECT count(custid) FROM cld_salesdetail_cpo WHERE smno='" & smno & "' AND outletid = " & olid & " AND itemno = '" & itemno & "'", 0) > 0 Then
                cmdprocess.executeNonQuery("DELETE FROM cld_salesdetail_cpo WHERE smno='" & smno & "' AND outletid = " & olid & " AND itemno = '" & itemno & "'")
                cmdprocess.ReNew()
            End If
            cmdprocess.executeNonQuery(str)



            cmdprocess = Nothing


        Catch ex As Exception
            errLog(Err.Description, str & "Cls_PO.insert_detailpo")
            Err.Clear()

        End Try

    End Sub
    Private Sub insert_paymentpo(ByVal smno As String, ByVal olid As Integer, ByVal number As String, ByVal paymentname As String, ByVal total As Decimal, ByVal tipe As Integer, ByVal custid As Integer, ByVal flag As Integer, ByVal busidate As Date, ByVal pmNo As String, ByVal EDCRef As String, ByVal EDCApp As String, ByVal EDCtrace As String)
        Dim c_stkCost As New clsInventory_stkCost
        Dim str As String = ""
        Try
            str = "insert into cld_payment_cpo(smNo,outletID,Number,PaymentName,Total,Tipe,CustID,flag,BusiDate,TransDate,Refno,Notes,pmNo,EDCRef,EDCApp,EDCtrace) 
                    values ('" & smno & "'," & olid & ",'" & number & "','" & paymentname & "'," & total & "," & tipe & "," & custid & "," & flag & ", '" & busidate.ToString("yyyy-MM-dd HH:mm:ss") & "', '" & Now.Date.ToString("yyyy-MM-dd HH:mm:ss") & "','','','" & pmNo & "','" & EDCRef & "','" & EDCApp & "','" & EDCtrace & "')"

            Dim cmdprocess As New clsDbo("")
            If cmdprocess.ExecuteScalar("SELECT count(custid) FROM cld_payment_cpo WHERE smno='" & smno & "' AND outletid = " & olid & "", 0) > 0 Then
                cmdprocess.executeNonQuery("DELETE FROM cld_payment_cpo WHERE smno='" & smno & "' AND outletid = " & olid & "")
                cmdprocess.ReNew()
            Else
                cmdprocess.executeNonQuery(str)
            End If
            cmdprocess = Nothing


        Catch ex As Exception
            errLog(Err.Description, str & "Cls_PO.insert_paymentpo")
            Err.Clear()

        End Try

    End Sub
    Public Function SelectDS_POnumber(ByVal custid As Integer) As String
        Dim ds As New DataSet

        Dim strFilter As String = ""

        Try

            Dim a As New clsDbo("SELECT * from cld_salesmaster_cpo where MONTH(busidate) = " & Date.Now.Month & " and custid = " & custid & "  and flag = 1 order by number desc LIMIT 1")

            ds = a.selectDataset

            If ds.Tables(0).Rows.Count > 0 Then
                strFilter = ds.Tables(0).Rows(0).Item("Number")
            End If

            a.dispose()
            a = Nothing

        Catch ex As Exception

            errLog(Err.Description, "Cls_PO:SelectDS_POnumber")
            Err.Clear()
        End Try


        ds.Dispose()

        Return strFilter

    End Function
    Public Function SelectDs_CustPOEdit(ByVal Id As Integer, ByVal custid As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""


        Dim a As New clsDbo("SELECT * from cld_inv_ordercust where id = " & Id & " and custid = " & custid & "")

        Try
            ds = a.selectDataset

            a.dispose()
            a = Nothing
        Catch ex As Exception

        End Try

        Return ds

    End Function
    Public Function SelectDatacustpo(ByVal id As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""


        Dim a As New clsDbo("SELECT Id, Name FROM cld_inv_ordercust where id = " & id)


        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function


    Public Function SelectDatacustpo_wName(ByVal sName As String) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""

        Dim a As New clsDbo("SELECT Id, Name FROM cld_inv_ordercust where name = '" & sName & "'")

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function
    Public Sub insertcustPO(ByVal id As Integer, ByVal custid As Integer, ByVal nama As String, ByVal email As String, ByVal Address As String, ByVal phone As String, ByVal City As String, ByVal State As String, ByVal Pcode As String, ByVal Notes As String, ByVal username As String, Optional ByVal itipe As Integer = 0)

        Dim c_cmd As New clsDbo("")
        Try

            Dim ds As DataSet

            If itipe = 0 Then
                ds = SelectDatacustpo(id)
            Else
                ds = SelectDatacustpo_wName(nama)

                If ds.Tables(0).Rows.Count > 0 Then
                    id = ds.Tables(0).Rows(0).Item("id")
                End If

            End If


            Dim str As String

            If ds.Tables(0).Rows.Count = 0 Then
                str = "insert into cld_inv_ordercust values (" & c_cmd.ExecuteScalar("select max(ID) from cld_inv_ordercust") & ", " & custid & ", '" & nama & "',  '" & email & "', '" & Address & "', '" & phone & "', '" & City & "', '" & State & "', '" & Pcode & "', '" & Notes & "')"

                'Revisi 10 Mei
                insertLogModule(username, "CustPO", "Insert", custid)
            Else
                str = "Update cld_inv_ordercust set  name = '" & nama & "', email = '" & email & "',  Address= '" & Address & "', phone = '" & phone & "', City = '" & City & "', state = '" & State & "',  Pcode ='" & Pcode & "', Notes = '" & Notes & "' where id = " & id & ""


                insertLogModule(username, "InvCustPO", "Update", custid)
            End If

            c_cmd.executeNonQuery(str)

        Catch ex As Exception

            errLog(Err.Description, "Cls_PO:insertcustPO")
            Err.Clear()
        End Try


        c_cmd = Nothing

    End Sub
    Public Function Selectdt_CPO_grid(ByVal linkid As String, ByVal custid As Integer, ByVal olid As Integer) As DataTable
        Dim ds As New DataTable
        Dim strFilter As String = ""

        strFilter = "SELECT smno,sm.custname,cust.phone,sm.olnumber,sm.olname,itemno,itemname,qty,sm.transdate,sm.busidate FROM `cld_salesdetail_cpo` sd
        JOIN `cld_salesmaster_cpo` sm ON sd.smno=sm.number AND sd.outletid=sm.olid
         JOIN cld_inv_ordercust cust on sm.ordercust = cust.id WHERE smno = '" & linkid & "'
        AND sm.flag = 1 AND sm.custid = " & custid & " AND sm.olid = " & olid & ""

        Dim a As New clsDbo(strFilter)

        ds = a.selectDatatable

        a.dispose()
        a = Nothing

        Return ds

    End Function
End Class

