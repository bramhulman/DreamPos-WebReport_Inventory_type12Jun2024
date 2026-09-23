Public Class clsInventory_Batch


    'revisi 20 okt 2020
    Public Function getproperty_RcpDetail(ByVal Custid As Integer, ByVal stkid As Integer, ByVal dQty As Decimal, ByVal dInvDate As Date, ByVal ilinkId As Integer, ByVal iCc As Integer, ByVal sCcNm As String, Optional ByVal rcpid As Integer = 0) As Decimal
        Dim bcost As Decimal = 0
        Dim c_recal As New cls_recalculate
        Try
            Dim clsprocess As New clsInventoryProcess
            Dim cmd As New clsDbo("")

            Dim dData As New DataSet
            ' dData = cmd.ReturnDataset("SELECT rDtl.*, stk.Number as stkNo, stk.Name as stkNm, stk.Factor2, stk.Unit2, stk.Unit3   FROM cld_inv_rcpdtl AS rDtl LEFT JOIN cld_inv_stk AS stk ON rDtl.kodeId = stk.ID AND rdtl.CustID = stk.CustID where rdtl.custid = " & Custid & " and rcpid = " & rcpid)

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

                            insert_Batchdetail_Stk(Custid, iCc, sCcNm, dData.Tables(0).Rows(x).Item("kodeid"), dData.Tables(0).Rows(x).Item("stkNo"), dData.Tables(0).Rows(x).Item("stkNm"),
                                                       dQty * (dData.Tables(0).Rows(x).Item("Qty")),
                                                       dData.Tables(0).Rows(x).Item("Factor2"), dData.Tables(0).Rows(x).Item("Unit2"), dData.Tables(0).Rows(x).Item("Unit3"), dInvDate.ToString("yyyy-MM-dd HH:mm:ss"), ilinkId)



                            '+ (dData.Tables(0).Rows(x).Item("lossf") / 100 * dData.Tables(0).Rows(x).Item("Qty"))

                            clsprocess.InvstkId = dData.Tables(0).Rows(x).Item("kodeid")
                            clsprocess.InvCc = iCc
                            clsprocess.InvCcName = sCcNm
                            clsprocess.InvCustId = Custid
                            clsprocess.InvstkName = dData.Tables(0).Rows(x).Item("stkNm")
                            clsprocess.BP_in = 0
                            'If dData.Tables(0).Rows(x).Item("lossf") = 0 Then
                            clsprocess.BP_out = CDbl(dQty * (dData.Tables(0).Rows(x).Item("Qty")) / dData.Tables(0).Rows(x).Item("factor2"))
                            If cmd.ExecuteScalar("SELECT IFNULL(custid,0) cust_id FROM `cld_inv_stk_cc` WHERE custid<>" & Custid & " LIMIT 1", 0) > 0 Then
                                cmd.ReNew()
                                cmd.executeNonQuery("DELETE FROM `cld_inv_stk_cc` WHERE custid<>" & Custid)
                            End If
                            If My.Settings.stock_cost = 1 Then

                                If cmd.ExecuteScalar("select ifnull(cost,0) cost from cld_inv_stk_cc a WHERE A.Custid = " & Custid & " and ccid = " & iCc & " and stkid =" & dData.Tables(0).Rows(x).Item("kodeid") & "", 0) <> 0 Then
                                    bcost = bcost + (CDbl(dQty * (dData.Tables(0).Rows(x).Item("Qty")) / dData.Tables(0).Rows(x).Item("factor2")) * cmd.ExecuteScalar("select ifnull(cost,0) cost from cld_inv_stk_cc a WHERE A.Custid = " & Custid & " and ccid = " & iCc & " and stkid =" & dData.Tables(0).Rows(x).Item("kodeid") & "", 0))
                                    clsprocess.InvCost = cmd.ExecuteScalar("select ifnull(cost,0) cost from cld_inv_stk_cc a WHERE A.Custid = " & Custid & " and ccid = " & iCc & " and stkid =" & dData.Tables(0).Rows(x).Item("kodeid") & "", 0)
                                Else
                                    bcost = bcost + (CDbl(dQty * (dData.Tables(0).Rows(x).Item("Qty")) / dData.Tables(0).Rows(x).Item("factor2")) * cmd.ExecuteScalar("select ifnull(cost,0) cost from cld_inv_transstockcls a WHERE A.invdate ='" & Now.Date.ToString("yyyy-MM-dd 00:00:00") & "' and A.Custid = " & Custid & " and ccid = " & iCc & " and stkid =" & dData.Tables(0).Rows(x).Item("kodeid") & "", 0))
                                    clsprocess.InvCost = cmd.ExecuteScalar("select ifnull((case when cost>0 then cost else 0 end),0) as cost from cld_inv_transstockcls a WHERE A.invdate ='" & Now.Date.ToString("yyyy-MM-dd 00:00:00") & "' and A.Custid = " & Custid & " and ccid = " & iCc & " and stkid =" & dData.Tables(0).Rows(x).Item("kodeid") & "", 0)
                                End If

                            Else
                                bcost = bcost + (CDbl(dQty * (dData.Tables(0).Rows(x).Item("Qty")) / dData.Tables(0).Rows(x).Item("factor2")) * cmd.ExecuteScalar("select ifnull((case when cost>0 then cost else 0 end),0) as cost from cld_inv_transstockcls a WHERE A.invdate ='" & Now.Date.ToString("yyyy-MM-dd 00:00:00") & "' and A.Custid = " & Custid & " and ccid = " & iCc & " and stkid =" & dData.Tables(0).Rows(x).Item("kodeid") & "", 0))
                                'Else
                                '    clsprocess.BP_out = CDbl(dQty * (dData.Tables(0).Rows(x).Item("Qty") + (dData.Tables(0).Rows(x).Item("lossf") / 100 * dData.Tables(0).Rows(x).Item("Qty"))) / dData.Tables(0).Rows(x).Item("factor2"))
                                '    bcost = bcost + (CDbl(dQty * (dData.Tables(0).Rows(x).Item("Qty") + (dData.Tables(0).Rows(x).Item("lossf") / 100 * dData.Tables(0).Rows(x).Item("Qty"))) / dData.Tables(0).Rows(x).Item("factor2")) * cmd.ExecuteScalar("select cost from cld_inv_transstockcls a WHERE A.invdate ='" & Now.Date.ToString("yyyy-MM-dd 00:00:00") & "' and A.Custid = " & Custid & " and ccid = " & iCc & " and stkid =" & dData.Tables(0).Rows(x).Item("kodeid") & "", 0))
                                'End If
                                clsprocess.InvCost = cmd.ExecuteScalar("select ifnull((case when cost>0 then cost else 0 end),0) as cost from cld_inv_transstockcls a WHERE A.invdate ='" & Now.Date.ToString("yyyy-MM-dd 00:00:00") & "' and A.Custid = " & Custid & " and ccid = " & iCc & " and stkid =" & dData.Tables(0).Rows(x).Item("kodeid") & "", 0)

                            End If
                            'clsprocess.BP_out = CDbl(dQty * (dData.Tables(0).Rows(x).Item("Qty") + (dData.Tables(0).Rows(x).Item("lossf") / 100 * dData.Tables(0).Rows(x).Item("Qty"))))
                            clsprocess.InvStkNo = dData.Tables(0).Rows(x).Item("stkNo")
                            clsprocess.InvDate = dInvDate.ToString("yyyy-MM-dd 00:00:00")
                            'cInv.executeNonQuery_Inv_ProcessCls(Date.Now.Date.ToString("yyyy-MM-dd"), ddlFrom.SelectedValue, ddlFrom.SelectedItem.Text, stkid, stknumber, stkname, custid, 0, stktotal)
                            clsprocess.executeNonQuery_Inv_ProcessCls()

                            c_recal.recalFlag_dailycost(iCc, dInvDate, Custid)

                        ElseIf dData.Tables(0).Rows(x).Item("tipe") = 2 Then
                            'link ke receipe
                            'getproperty_RcpDetail(Custid, iCc, sCcNm, dData.Tables(0).Rows(0).Item("kodeid"), dQty * dData.Tables(0).Rows(0).Item("Factor"), dInvDate.ToString("yyyy-MM-dd HH:mm:ss"), ilinkId)
                            getproperty_RcpDetail(Custid, dData.Tables(0).Rows(x).Item("kodeid"), dQty * dData.Tables(0).Rows(x).Item("qty"), dInvDate.ToString("yyyy-MM-dd HH:mm:ss"), ilinkId, iCc, sCcNm)


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
    'untuk delete stock_cls
    Public Function getproperty_RcpDetail_del(ByVal Custid As Integer, ByVal stkid As Integer, ByVal dQty As Decimal, ByVal dInvDate As Date, ByVal ilinkId As Integer, ByVal iCc As Integer, ByVal sCcNm As String, ByVal id As Integer, Optional ByVal rcpid As Integer = 0) As Decimal
        Dim bcost As Decimal = 0
        Try
            Dim clsprocess As New clsInventoryProcess
            Dim cmd As New clsDbo("")
            Dim qtybatch As Double


            Dim dData As New DataSet
            'dData = cmd.ReturnDataset("SELECT rDtl.*, stk.Number as stkNo, stk.Name as stkNm, stk.Factor2, stk.Unit2, stk.Unit3   FROM cld_inv_rcpdtl AS rDtl LEFT JOIN cld_inv_stk AS stk ON rDtl.kodeId = stk.ID AND rdtl.CustID = stk.CustID where rdtl.custid = " & Custid & " and rcpid = " & rcpid)

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

                    If IsDBNull(dData.Tables(0).Rows(x).Item("tipe")) = False Then

                        If dData.Tables(0).Rows(x).Item("tipe") = 1 And
                            cmd.ExecuteScalar("select bpout from cld_inv_transstockcls where stkid = " & dData.Tables(0).Rows(x).Item("kodeid") & " AND CCid = " & iCc & " AND invdate = '" & dInvDate.ToString("yyyy-MM-dd 00:00:00") & "' and custid = " & Custid, 0) > 0 Then

                            cmd.ReNew()
                            cmd.executeNonQuery("Delete from cld_inv_transusage where type = 11 and cc= " & iCc & " and linkid= " & ilinkId & " and stk_id = " & dData.Tables(0).Rows(x).Item("kodeid") & "")
                            'cmd.ReNew()
                            'cmd.executeNonQuery("Delete from cld_inv_trans where type = 9 and cc= " & iCc & " and id= " & ilinkId & " and stk_id = " & dData.Tables(0).Rows(x).Item("kodeid") & "")


                            'link ke Stock master
                            'kalau ada lossreceipt




                            clsprocess.InvstkId = dData.Tables(0).Rows(x).Item("kodeid")
                            clsprocess.InvCc = iCc
                            clsprocess.InvCcName = sCcNm
                            clsprocess.InvCustId = Custid
                            clsprocess.InvstkName = dData.Tables(0).Rows(x).Item("stkNm")
                            clsprocess.BP_in = 0
                            'If dData.Tables(0).Rows(x).Item("lossf") = 0 Then
                            qtybatch = (CDbl(cmd.ExecuteScalar("select ttlstock/factor1 from cld_Inv_Trans where id = " & id & " and custid = " & Custid, 0)))
                            clsprocess.BP_out = -CDbl(qtybatch * (dData.Tables(0).Rows(x).Item("Qty")) / dData.Tables(0).Rows(x).Item("factor2"))
                            '-CDbl(dQty * (dData.Tables(0).Rows(x).Item("Qty")) / dData.Tables(0).Rows(x).Item("factor2"))
                            'bcost = bcost + (-CDbl(dQty * (dData.Tables(0).Rows(x).Item("Qty")) / dData.Tables(0).Rows(x).Item("factor2")) * cmd.ExecuteScalar("select cost from cld_inv_transstockcls a WHERE A.invdate ='" & Now.Date.ToString("yyyy-MM-dd 00:00:00") & "' and A.Custid = " & Custid & " and ccid = " & iCc & " and stkid =" & dData.Tables(0).Rows(x).Item("kodeid") & "", 0))
                            ' Else
                            'clsprocess.BP_out = -CDbl(dQty * (dData.Tables(0).Rows(x).Item("Qty") + (dData.Tables(0).Rows(x).Item("lossf") / 100 * dData.Tables(0).Rows(x).Item("Qty"))) / dData.Tables(0).Rows(x).Item("factor2"))
                            'bcost = bcost + (-CDbl(dQty * (dData.Tables(0).Rows(x).Item("Qty") + (dData.Tables(0).Rows(x).Item("lossf") / 100 * dData.Tables(0).Rows(x).Item("Qty"))) / dData.Tables(0).Rows(x).Item("factor2")) * cmd.ExecuteScalar("select cost from cld_inv_transstockcls a WHERE A.invdate ='" & Now.Date.ToString("yyyy-MM-dd 00:00:00") & "' and A.Custid = " & Custid & " and ccid = " & iCc & " and stkid =" & dData.Tables(0).Rows(x).Item("kodeid") & "", 0))

                            'End If
                            If cmd.ExecuteScalar("SELECT IFNULL(custid,0) cust_id FROM `cld_inv_stk_cc` WHERE custid<>" & Custid & " LIMIT 1", 0) > 0 Then
                                cmd.ReNew()
                                cmd.executeNonQuery("DELETE FROM `cld_inv_stk_cc` WHERE custid<>" & Custid)
                            End If
                            If My.Settings.stock_cost = 1 And (cmd.ExecuteScalar("select ifnull(cost,0) cost from cld_inv_stk_cc a WHERE A.Custid = " & Custid & " and ccid = " & iCc & " and stkid =" & dData.Tables(0).Rows(x).Item("kodeid") & "", 0) > 1) Then

                                clsprocess.InvCost = cmd.ExecuteScalar("select ifnull(cost,0) cost from cld_inv_stk_cc a WHERE A.Custid = " & Custid & " and ccid = " & iCc & " and stkid =" & dData.Tables(0).Rows(x).Item("kodeid") & "", 0)

                            Else
                                clsprocess.InvCost = cmd.ExecuteScalar("select ifnull((case when cost>0 then cost else 0 end),0) as cost from cld_inv_transstockcls a WHERE A.invdate ='" & dInvDate.ToString("yyyy-MM-dd 00:00:00") & "' and A.Custid = " & Custid & " and ccid = " & iCc & " and stkid =" & dData.Tables(0).Rows(x).Item("kodeid") & "", 0)

                            End If
                            clsprocess.InvStkNo = dData.Tables(0).Rows(x).Item("stkNo")
                            clsprocess.InvDate = dInvDate.ToString("yyyy-MM-dd 00:00:00")
                            'cInv.executeNonQuery_Inv_ProcessCls(Date.Now.Date.ToString("yyyy-MM-dd"), ddlFrom.SelectedValue, ddlFrom.SelectedItem.Text, stkid, stknumber, stkname, custid, 0, stktotal)
                            If clsprocess.InvCost > 1 Then

                                clsprocess.BCost = True

                            End If


                            clsprocess.executeNonQuery_Inv_ProcessCls()



                        ElseIf dData.Tables(0).Rows(x).Item("tipe") = 2 Then
                            'link ke receipe
                            'getproperty_RcpDetail(Custid, iCc, sCcNm, dData.Tables(0).Rows(0).Item("kodeid"), dQty * dData.Tables(0).Rows(0).Item("Factor"), dInvDate.ToString("yyyy-MM-dd HH:mm:ss"), ilinkId)
                            getproperty_RcpDetail_del(Custid, dData.Tables(0).Rows(x).Item("kodeid"), dQty * dData.Tables(0).Rows(x).Item("qty"), dInvDate.ToString("yyyy-MM-dd HH:mm:ss"), ilinkId, iCc, sCcNm, id)


                        End If

                    End If

                Next

            End If

            cmd.dispose()
            cmd = Nothing

        Catch ex As Exception
            errLog(Err.Description, "Cls_IBatch.getproperty_RcpDetail_del")
            Err.Clear()
        End Try

        Return bcost
    End Function
    Private Sub insert_Batchdetail_Stk(ByVal iCust As Integer, ByVal iCC As Integer, ByVal sCc As String, ByVal iStk As Integer, ByVal sStkno As String, ByVal sStknm As String, ByVal dqtyFac As Decimal, ByVal dFactor As Integer, ByVal sU2 As String, ByVal sU3 As String, ByVal dInvDate As Date, ByVal iBatchId As Integer)
        Dim c_stkCost As New clsInventory_stkCost
        Dim str As String = ""
        Try
            str = "insert into cld_inv_transusage(CustID, InvDate, type, Cc, Cc_name, Stk_Id, Stk_no, Stk_Name, U2, U3, Factor2, ttlStock, Cost, Linkid, flagtrans, U2_nm, U3_nm, CreatedBy, CreatedDate) values (" & iCust & ", '" & dInvDate.ToString("yyyy-MM-dd HH:mm:ss") & "', 11, " & iCC & ", '" & sCc & "', " & iStk & ", '" & sStkno & "', '" & sStknm & "',0 ," & dqtyFac & ", " & dFactor & ", " & CDec(dqtyFac / dFactor) & "," & CInt(c_stkCost.get_stkCost(iStk, iCC, iCust)) & ", " & iBatchId & " , 1, '" & sU2 & "','" & sU3 & "','SysAdmin','" & Now.ToString("yyyy-MM-dd HH:mm:ss") & "')"

            Dim cmdprocess As New clsDbo("")
            cmdprocess.executeNonQuery(Str)

            cmdprocess = Nothing


        Catch ex As Exception
            errLog(Err.Description, str & "Cls_IBatch.insert_Batchdetail_Stk")
            Err.Clear()

        End Try

    End Sub

End Class
