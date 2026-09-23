Public Class Cls_IBatch

    Public Function SelectDataset_invBatch(ByVal bydate As Date, ByVal custid As Integer, ByVal ModPriv As Integer, ByVal user As String) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""


        If ModPriv And usrModulepriv.moduleuser = usrModulepriv.moduleuser Then

            strFilter = "SELECT cld_inv_Trans.*, DATE_FORMAT(cld_inv_trans.invDate,'%d-%b-%Y') AS InvD  FROM cld_inv_trans LEFT JOIN cld_inv_cc ON cld_inv_trans.cc = cld_inv_cc.ID and cld_inv_trans.Custid = cld_inv_cc.Custid " &
                        "where  cld_inv_Trans.type =  9 and cld_inv_trans.custid = " & custid & " and cld_inv_Trans.InvDate = '" & bydate.ToString("yyyy-MM-dd") & "' and  cld_inv_cc.olid IN (SELECT Store FROM cld_userxstore WHERE USER = '" & user & "')"

        Else

            strFilter = "SELECT cld_inv_Trans.*, DATE_FORMAT(cld_inv_trans.invDate,'%d-%b-%Y') AS InvD  FROM cld_inv_trans " & _
                        "where  cld_inv_Trans.type =  9 and cld_inv_trans.custid = " & custid & " and cld_inv_Trans.InvDate = '" & bydate.ToString("yyyy-MM-dd") & "'"

        End If

        Dim a As New clsDbo(strFilter)

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function

    Public Function SelectDataset_invBatchSearch(ByVal name As String, ByVal bydate As Date, ByVal custid As Integer, ByVal ModPriv As Integer, ByVal user As String) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""

        If ModPriv And usrModulepriv.moduleuser = usrModulepriv.moduleuser Then


            strFilter = "SELECT cld_inv_Trans.*, DATE_FORMAT(cld_inv_trans.invDate,'%d-%b-%Y') AS InvD, cld_inv_cc.Name AS ccname  FROM cld_inv_trans LEFT JOIN cld_inv_cc ON cld_inv_trans.cc = cld_inv_cc.ID and cld_inv_trans.Custid = cld_inv_cc.Custid " &
                        "where cld_inv_Trans.type =  9 and cld_inv_trans.custid = " & custid & " and cld_inv_Trans.InvDate = '" & bydate.ToString("yyyy-MM-dd") & "' and (cld_inv_cc.name Like '%" & name & "%' or cld_inv_Trans.stk_name Like '%" & name & "%') and cld_inv_cc.olid IN (SELECT Store FROM cld_userxstore WHERE USER = '" & user & "')"

        Else

            strFilter = "SELECT cld_inv_Trans.*, DATE_FORMAT(cld_inv_trans.invDate,'%d-%b-%Y') AS InvD, cld_inv_cc.Name AS ccname  FROM cld_inv_trans LEFT JOIN cld_inv_cc ON cld_inv_trans.cc = cld_inv_cc.ID and cld_inv_trans.Custid = cld_inv_cc.Custid " &
                        "where cld_inv_Trans.type =  9 and cld_inv_trans.custid = " & custid & " and cld_inv_Trans.InvDate = '" & bydate.ToString("yyyy-MM-dd") & "' and (cld_inv_cc.name Like '%" & name & "%' or cld_inv_Trans.stk_name Like '%" & name & "%')"


        End If


        Dim a As New clsDbo(strFilter)

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function


    Public Function SelectDataset_InBatchEdit(ByVal Id As Integer, ByVal custid As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""

        'perubahan 11 oct 2017
        Dim a As New clsDbo("SELECT cld_inv_trans.*, IFNULL((SELECT ttlqty FROM cld_inv_transstockcls WHERE stkid = cld_inv_trans.stk_id AND ccid = cld_inv_trans.Cc AND custid = cld_inv_trans.CustID and invdate = cld_inv_trans.invdate), 0) AS stock " &
                            ", IFNULL((SELECT cost FROM cld_inv_transstockcls WHERE stkid = cld_inv_trans.`Stk_Id` AND ccid = cld_inv_trans.`Cc`
AND custid = cld_inv_trans.`CustID` AND invdate = cld_inv_trans.invdate), 0) AS coststk,(select stk_link from cld_inv_transnotes where transid = cld_inv_trans.id and cc = cld_inv_trans.cc) rcpid" &
                            " FROM cld_inv_trans WHERE type = 9 and id = " & Id & " AND  custid = " & custid & "")

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function

    'okt 2017
    'untuk ambil relasi data batchnya
    'Public Sub getproperty_RcpDetail(ByVal Custid As Integer, ByVal rcpid As Integer, ByVal dQty As Decimal, ByVal dInvDate As Date, ByVal ilinkId As Integer, ByVal iCc As Integer, ByVal sCcNm As String)
    '    Try

    '        Dim cmd As New Cls_command

    '        Dim dData As New DataSet
    '        dData = cmd.ReturnDataset("SELECT rDtl.*, stk.Number as stkNo, stk.Name as stkNm, stk.Factor2, stk.Unit2, stk.Unit3   FROM cld_inv_rcpdtl AS rDtl LEFT JOIN cld_inv_stk AS stk ON rDtl.kodeId = stk.ID AND rdtl.CustID = stk.CustID where rdtl.custid = " & Custid & " and rcpid = " & rcpid)

    '        If dData.Tables(0).Rows.Count > 0 Then

    '            For x = 0 To dData.Tables(0).Rows.Count - 1

    '                If IsDBNull(dData.Tables(0).Rows(x).Item("tipe")) = False Then

    '                    If dData.Tables(0).Rows(x).Item("tipe") = 1 Then
    '                        'link ke Stock master
    '                        insert_Batchdetail_Stk(Custid, iCc, sCcNm, dData.Tables(0).Rows(x).Item("kodeid"), dData.Tables(0).Rows(x).Item("stkNo"), dData.Tables(0).Rows(x).Item("stkNm"), dQty * dData.Tables(0).Rows(x).Item("Qty"), dData.Tables(0).Rows(x).Item("Factor2"), dData.Tables(0).Rows(x).Item("Unit2"), dData.Tables(0).Rows(x).Item("Unit3"), dInvDate.ToString("yyyy-MM-dd HH:mm:ss"), ilinkId)

    '                    ElseIf dData.Tables(0).Rows(x).Item("tipe") = 2 Then
    '                        'link ke receipe
    '                        'getproperty_RcpDetail(Custid, iCc, sCcNm, dData.Tables(0).Rows(0).Item("kodeid"), dQty * dData.Tables(0).Rows(0).Item("Factor"), dInvDate.ToString("yyyy-MM-dd HH:mm:ss"), ilinkId)
    '                        getproperty_RcpDetail(Custid, dData.Tables(0).Rows(x).Item("kodeid"), dQty * dData.Tables(0).Rows(x).Item("qty"), dInvDate.ToString("yyyy-MM-dd HH:mm:ss"), ilinkId, iCc, sCcNm)


    '                    End If

    '                End If

    '            Next

    '        End If

    '        cmd.closeConnection()
    '        cmd = Nothing

    '    Catch ex As Exception
    '        errLog(Err.Description, "Cls_IBatch.getproperty_RcpDetail")
    '        Err.Clear()
    '    End Try


    'End Sub

    'revisi 20 okt 2020
    Public Function getproperty_RcpDetail(ByVal Custid As Integer, ByVal rcpid As Integer, ByVal dQty As Decimal, ByVal dInvDate As Date, ByVal ilinkId As Integer, ByVal iCc As Integer, ByVal sCcNm As String) As Decimal
        Dim bcost As Decimal = 0
        Dim c_recal As New cls_recalculate
        Try
            Dim clsprocess As New clsInventoryProcess
            Dim cmd As New clsDbo("")

            Dim dData As New DataSet
            ' dData = cmd.ReturnDataset("SELECT rDtl.*, stk.Number as stkNo, stk.Name as stkNm, stk.Factor2, stk.Unit2, stk.Unit3   FROM cld_inv_rcpdtl AS rDtl LEFT JOIN cld_inv_stk AS stk ON rDtl.kodeId = stk.ID AND rdtl.CustID = stk.CustID where rdtl.custid = " & Custid & " and rcpid = " & rcpid)

            dData = cmd.ReturnDataset("SELECT stkr.id kodeid,stkr.number stkno,rcpdtl.tipe,stkr.name stknm,(rcpdtl.Qty + (rcpdtl.Qty*rcpdtl.lossf/100)) qty, rcpdtl.lossf, stkr.`Factor2`,stkr.unit2,stkr.unit3
                                         FROM cld_inv_stk stk
                                        LEFT JOIN cld_inv_rcpdtl rcpdtl ON stk.blink = rcpdtl.rcpid AND stk.custid = rcpdtl.custid
                                        LEFT JOIN cld_inv_stk stkr ON stkr.id = rcpdtl.kodeid AND stkr.custid = rcpdtl.custid
                                        where rcpdtl.custid = " & Custid & " and stk.id = " & rcpid)
            If dData.Tables(0).Rows.Count > 0 Then

                For x = 0 To dData.Tables(0).Rows.Count - 1

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

                                If cmd.ExecuteScalar("select ifnull(cost,0) cost from cld_inv_stk_cc a WHERE A.Custid = " & Custid & " and ccid = " & iCc & " and stkid =" & dData.Tables(0).Rows(x).Item("kodeid") & "", 0) >= 0 Then
                                    bcost = bcost + (CDbl(dQty * (dData.Tables(0).Rows(x).Item("Qty")) / dData.Tables(0).Rows(x).Item("factor2")) * cmd.ExecuteScalar("select IFNULL((CASE WHEN cost>0 THEN cost ELSE 0 END),0) AS cost from cld_inv_stk_cc a WHERE A.Custid = " & Custid & " and ccid = " & iCc & " and stkid =" & dData.Tables(0).Rows(x).Item("kodeid") & "", 0))
                                    clsprocess.InvCost = cmd.ExecuteScalar("select IFNULL((CASE WHEN cost>0 THEN cost ELSE 0 END),0) AS cost from cld_inv_stk_cc a WHERE A.Custid = " & Custid & " and ccid = " & iCc & " and stkid =" & dData.Tables(0).Rows(x).Item("kodeid") & "", 0)
                                Else
                                    bcost = bcost + (CDbl(dQty * (dData.Tables(0).Rows(x).Item("Qty")) / dData.Tables(0).Rows(x).Item("factor2")) * cmd.ExecuteScalar("select IFNULL((CASE WHEN cost>0 THEN cost ELSE 0 END),0) AS cost from cld_inv_transstockcls a WHERE A.invdate ='" & Now.Date.ToString("yyyy-MM-dd 00:00:00") & "' and A.Custid = " & Custid & " and ccid = " & iCc & " and stkid =" & dData.Tables(0).Rows(x).Item("kodeid") & "", 0))
                                    clsprocess.InvCost = cmd.ExecuteScalar("select IFNULL((CASE WHEN cost>0 THEN cost ELSE 0 END),0) AS cost from cld_inv_transstockcls a WHERE A.invdate ='" & Now.Date.ToString("yyyy-MM-dd 00:00:00") & "' and A.Custid = " & Custid & " and ccid = " & iCc & " and stkid =" & dData.Tables(0).Rows(x).Item("kodeid") & "", 0)
                                End If

                            Else
                                bcost = bcost + (CDbl(dQty * (dData.Tables(0).Rows(x).Item("Qty")) / dData.Tables(0).Rows(x).Item("factor2")) * cmd.ExecuteScalar("select IFNULL((CASE WHEN cost>0 THEN cost ELSE 0 END),0) AS cost from cld_inv_transstockcls a WHERE A.invdate ='" & Now.Date.ToString("yyyy-MM-dd 00:00:00") & "' and A.Custid = " & Custid & " and ccid = " & iCc & " and stkid =" & dData.Tables(0).Rows(x).Item("kodeid") & "", 0))
                                'Else
                                '    clsprocess.BP_out = CDbl(dQty * (dData.Tables(0).Rows(x).Item("Qty") + (dData.Tables(0).Rows(x).Item("lossf") / 100 * dData.Tables(0).Rows(x).Item("Qty"))) / dData.Tables(0).Rows(x).Item("factor2"))
                                '    bcost = bcost + (CDbl(dQty * (dData.Tables(0).Rows(x).Item("Qty") + (dData.Tables(0).Rows(x).Item("lossf") / 100 * dData.Tables(0).Rows(x).Item("Qty"))) / dData.Tables(0).Rows(x).Item("factor2")) * cmd.ExecuteScalar("select cost from cld_inv_transstockcls a WHERE A.invdate ='" & Now.Date.ToString("yyyy-MM-dd 00:00:00") & "' and A.Custid = " & Custid & " and ccid = " & iCc & " and stkid =" & dData.Tables(0).Rows(x).Item("kodeid") & "", 0))
                                'End If
                                clsprocess.InvCost = cmd.ExecuteScalar("select IFNULL((CASE WHEN cost>0 THEN cost ELSE 0 END),0) AS cost from cld_inv_transstockcls a WHERE A.invdate ='" & Now.Date.ToString("yyyy-MM-dd 00:00:00") & "' and A.Custid = " & Custid & " and ccid = " & iCc & " and stkid =" & dData.Tables(0).Rows(x).Item("kodeid") & "", 0)

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
    Public Function getproperty_RcpDetail_del(ByVal Custid As Integer, ByVal rcpid As Integer, ByVal dQty As Decimal, ByVal dInvDate As Date, ByVal ilinkId As Integer, ByVal iCc As Integer, ByVal sCcNm As String, ByVal id As Integer) As Decimal
        Dim bcost As Decimal = 0
        Try
            Dim clsprocess As New clsInventoryProcess
            Dim cmd As New clsDbo("")
            Dim qtybatch As Double


            Dim dData As New DataSet
            'dData = cmd.ReturnDataset("SELECT rDtl.*, stk.Number as stkNo, stk.Name as stkNm, stk.Factor2, stk.Unit2, stk.Unit3   FROM cld_inv_rcpdtl AS rDtl LEFT JOIN cld_inv_stk AS stk ON rDtl.kodeId = stk.ID AND rdtl.CustID = stk.CustID where rdtl.custid = " & Custid & " and rcpid = " & rcpid)
            dData = cmd.ReturnDataset("SELECT stkr.id kodeid,stkr.number stkno,rcpdtl.tipe,stkr.name stknm,(rcpdtl.Qty + (rcpdtl.Qty*rcpdtl.lossf/100)) qty, rcpdtl.lossf, stkr.`Factor2`,stkr.unit2,stkr.unit3
                                         FROM cld_inv_stk stk
                                        LEFT JOIN cld_inv_rcpdtl rcpdtl ON stk.blink = rcpdtl.rcpid AND stk.custid = rcpdtl.custid
                                        LEFT JOIN cld_inv_stk stkr ON stkr.id = rcpdtl.kodeid AND stkr.custid = rcpdtl.custid
                                        where rcpdtl.custid = " & Custid & " and stk.id = " & rcpid)
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
                                clsprocess.InvCost = cmd.ExecuteScalar("select ifnull(cost,0) cost from cld_inv_transstockcls a WHERE A.invdate ='" & dInvDate.ToString("yyyy-MM-dd 00:00:00") & "' and A.Custid = " & Custid & " and ccid = " & iCc & " and stkid =" & dData.Tables(0).Rows(x).Item("kodeid") & "", 0)

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
        Try

            Dim c_stkCost As New clsInventory_stkCost
            Dim str As String = ""

            str = "insert into cld_inv_transusage(CustID, InvDate, type, Cc, Cc_name, Stk_Id, Stk_no, Stk_Name, U2, U3, Factor2, ttlStock, Cost, Linkid, flagtrans, U2_nm, U3_nm, CreatedBy, CreatedDate) values (" & iCust & ", '" & dInvDate.ToString("yyyy-MM-dd HH:mm:ss") & "', 11, " & iCC & ", '" & sCc & "', " & iStk & ", '" & sStkno & "', '" & sStknm & "',0 ," & dqtyFac & ", " & dFactor & ", " & CDec(dqtyFac / dFactor) & "," & CInt(c_stkCost.get_stkCost(iStk, iCC, iCust)) & ", " & iBatchId & " , 1, '" & sU2 & "','" & sU3 & "','SysAdmin','" & Now.ToString("yyyy-MM-dd HH:mm:ss") & "')"

            Dim cmdprocess As New clsDbo("")
            cmdprocess.executeNonQuery(str)

            cmdprocess = Nothing


        Catch ex As Exception
            errLog(Err.Description, "Cls_IBatch.insert_Batchdetail_Stk")
            Err.Clear()

        End Try

    End Sub

    Public Function batch_date(ByVal ID As Integer) As Integer
        'Dim ds As New DataSet

        Dim edtable As Integer = 0

        Try
            Dim a As New clsDbo("")
            edtable = a.ExecuteScalar("SELECT count(id) from cld_inv_trans where id = '" & ID & "' and CreatedDate >= '" & Now.AddDays(-7).ToString("yyyy-MM-dd") & "'", 0)


            a.dispose()
            a = Nothing

        Catch ex As Exception
            errLog(Err.Description, "clsInventory_Warehouse:rcv_date")
            Err.Clear()
        End Try

        Return edtable

    End Function
    Public Function SelectDataTable_invtrans_Batch(ByVal linkid As String, ByVal custid As Integer, ByVal dinvdate As Date) As DataTable
        Dim ds As New DataTable
        Dim strFilter As String = ""


        Dim a As New clsDbo("SELECT A.*,c.rcpid,(SELECT NAME FROM `cld_inv_rcp` WHERE id = c.rcpid) Rcp_Name, IFNULL(B.ttlQty, 0) AS Stock,c.cc  FROM cld_inv_trans A INNER JOIN cld_invlk_batch c ON a.CustID = c.CustID AND a.Linkid = c.id " &
                            "LEFT JOIN cld_inv_transstockcls B ON B.stkId = A.Stk_Id  AND B.ccid = c.cc AND b.custId = c.CustID " &
                            "WHERE A.linkid = '" & linkid & "' and A.type = 9 and A.custid = " & custid & " and B.invdate = '" & dinvdate.ToString("yyyy-MM-dd 00:00:00") & "'")
        'Dim a As New clsDbo("SELECT A.*, IFNULL(B.ttlQty, 0) AS Stock  FROM cld_inv_trans A INNER JOIN cld_invlk_trf c ON a.CustID = c.CustID AND a.Linkid = c.id " &
        '                    "LEFT JOIN cld_inv_transstock B ON B.stkId = A.Stk_Id  AND B.ccid = c.ccFrom AND b.custId = c.CustID " &
        '                    "WHERE A.linkid = '" & linkid & "' and A.type = 1 and A.custid = " & custid & "")
        ds = a.selectDatatable

        a.dispose()
        a = Nothing

        Return ds

    End Function
End Class
