Public Class clsInventory_stkOpname

    Public Function SelectDataset_invStkOpname(ByVal custid As Integer, ByVal ModPriv As Integer, ByVal user As String, ByVal ccid As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""
        Dim strFilter2 As String = ""

        If ccid <> 0 Then
            strFilter2 = "and i_trans.ccid = " & ccid & ""
        End If

        If ModPriv And usrModulepriv.moduleuser = usrModulepriv.moduleuser Then


            strFilter = "SELECT StkID, StkNo, StkName, ccID, ccName, ttlQTy, unit2, bClose, CloseQty, CASE " &
            "WHEN bClose = 1 THEN ttlQTy - CloseQty ELSE 0 END AS VarianceQty " &
            "FROM (SELECT StkID, StkNo, StkName, ccID, ccName, ttlQTy, unit2 ,`bClose` ,CASE WHEN bClose = 1 THEN closeqty ELSE 0 END AS closeqty
 FROM cld_inv_transstockcls AS i_trans INNER JOIN cld_inv_cc ON i_trans.ccid = cld_inv_cc.ID and i_trans.custid = cld_inv_cc.custid 
            INNER JOIN cld_inv_stk ON i_trans.custid = cld_inv_stk.custid and i_trans.stkid = cld_inv_stk.id where cld_inv_cc.olid IN (SELECT Store FROM cld_userxstore WHERE USER = '" & user & "' and invdate = '" & Now.Date.ToString("yyyy-MM-dd") & "' " & strFilter2 & ") order by stkName ) a "


            'strFilter = "SELECT StkID, StkNo, StkName, ccID, ccName, ttlQTy, bClose, CloseQty, CASE " & _
            '"WHEN bClose = 1 THEN ttlQTy - CloseQty ELSE 0 END AS VarianceQty " & _
            '"FROM (SELECT StkID, StkNo, StkName, ccID, ccName, ttlQTy " & _
            '",IFNULL((SELECT `sd`.`flagtrans` FROM `cld_inv_trans` `sd` WHERE ((`sd`.`type` = 5) AND (`sd`.`flagtrans` = 1) AND (`sd`.`CustID` = `i_trans`.`custid`) AND (`sd`.`Stk_Id` = `i_trans`.`stkid`) AND (`sd`.`InvDate` = '" & Now.Date.ToString("yyyy-MM-dd") & "') AND (`sd`.`Cc` = `i_trans`.`Ccid`))),0) AS `bClose` " & _
            '",IFNULL((SELECT `sd`.`ttlStock` FROM `cld_inv_trans` `sd` WHERE ((`sd`.`type` = 5) AND (`sd`.`flagtrans` = 1) AND (`sd`.`CustID` = `i_trans`.`custid`) AND (`sd`.`Stk_Id` = `i_trans`.`stkid`) AND (`sd`.`InvDate` = '" & Now.Date.ToString("yyyy-MM-dd") & "') AND (`sd`.`Cc` = `i_trans`.`Ccid`))),0) AS `CloseQty` " & _
            '"FROM cld_inv_transstock AS i_trans INNER JOIN cld_inv_cc ON i_trans.ccid = cld_inv_cc.ID and i_trans.custid = cld_inv_cc.custid where cld_inv_cc.olid IN (SELECT Store FROM cld_userxstore WHERE USER = '" & user & "') order by stkName ) a "

        Else
            strFilter = "SELECT StkID, StkNo, StkName, ccID, ccName, ttlQTy, unit2, bClose, CloseQty, CASE WHEN bClose = 1 THEN ttlQTy - CloseQty ELSE 0 END AS VarianceQty
                        FROM (SELECT StkID, StkNo, StkName, ccID, ccName, ttlQTy, unit2 ,`bClose` ,CASE WHEN bClose = 1 THEN closeqty ELSE 0 END AS closeqty
                        FROM cld_inv_transstockcls AS i_trans INNER JOIN cld_inv_stk ON i_trans.custid = cld_inv_stk.custid AND i_trans.stkid = cld_inv_stk.id
                        WHERE invdate = '" & Now.Date.ToString("yyyy-MM-dd") & "' " & strFilter2 & ") a 
                        ORDER BY a.StkName  "


            'strFilter = "SELECT StkID, StkNo, StkName, ccID, ccName, ttlQTy, unit2, bClose, CloseQty, CASE " &
            '            "WHEN bClose = 1 THEN ttlQTy - CloseQty ELSE 0 END AS VarianceQty " &
            '            "FROM (SELECT StkID, StkNo, StkName, ccID, ccName, ttlQTy, unit2 " &
            '            ",IFNULL((SELECT `sd`.`flagtrans` FROM `cld_inv_trans` `sd` WHERE ((`sd`.`type` = 5) AND (`sd`.`flagtrans` = 1) AND (`sd`.`CustID` = `i_trans`.`custid`) AND (`sd`.`Stk_Id` = `i_trans`.`stkid`) AND (`sd`.`InvDate` = '" & Now.Date.ToString("yyyy-MM-dd") & "') AND (`sd`.`Cc` = `i_trans`.`Ccid`))),0) AS `bClose` " &
            '            ",IFNULL((SELECT `sd`.`ttlStock` FROM `cld_inv_trans` `sd` WHERE ((`sd`.`type` = 5) AND (`sd`.`flagtrans` = 1) AND (`sd`.`CustID` = `i_trans`.`custid`) AND (`sd`.`Stk_Id` = `i_trans`.`stkid`) AND (`sd`.`InvDate` = '" & Now.Date.ToString("yyyy-MM-dd") & "') AND (`sd`.`Cc` = `i_trans`.`Ccid`))),0) AS `CloseQty` " &
            '            "FROM cld_inv_transstock AS i_trans INNER JOIN cld_inv_stk ON i_trans.custid = cld_inv_stk.custid and i_trans.stkid = cld_inv_stk.id) a ORDER BY a.StkName "


        End If


        Dim a As New clsDbo(strFilter)

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function

    Public Function SelectDataset_invStkOpnameSearch(ByVal name As String, ByVal custid As Integer, ByVal ModPriv As Integer, ByVal user As String, ByVal ccid As Integer, ByVal dDate As Date) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""
        Dim strFilter2 As String = ""
        If ccid <> 0 Then
            strFilter2 = "and cld_inv_transstockcls.ccid = " & ccid & ""
        End If

        If ModPriv And usrModulepriv.moduleuser = usrModulepriv.moduleuser Then

            strFilter = "SELECT cld_inv_transstockcls.*,unit2,DATE_FORMAT(trans.createddate,'%d-%b-%Y %k:%i:%s') AS createddate,case trans.type when 11 then 'Current' else 'Opening' end as type FROM cld_inv_transstockcls 
                INNER JOIN cld_inv_cc ON cld_inv_transstockcls.ccid = cld_inv_cc.ID and cld_inv_transstockcls.custid = cld_inv_cc.custid 
                         INNER JOIN cld_inv_stk ON cld_inv_transstockcls.`stkID` = cld_inv_stk.id AND cld_inv_transstockcls.`custId` = cld_inv_stk.`CustID` 
                        INNER JOIN (SELECT * FROM cld_inv_trans WHERE cld_inv_Trans.invdate = '" & dDate.Date.AddDays(1).ToString("yyyy-MM-dd") & "' AND TYPE in (11,12)) trans 
                        ON cld_inv_transstockcls.`stkID` = trans.stk_id AND cld_inv_transstockcls.`custId` = trans.`CustID` and cld_inv_transstockcls.`ccid`= trans.cc
                        where cld_inv_transstockcls.custid = " & custid & " " & strFilter2 & " And cld_inv_transstockcls.invdate = '" & dDate.Date.ToString("yyyy-MM-dd") & "' and (ccname Like '%" & name & "%' or stkname Like '%" & name & "%') 
                        And cld_inv_cc.olid in (Select Store From cld_userxstore where user = '" & user & "') and bclose = 1 order by stkName" 'and closeqty >0 
        Else
            strFilter = "SELECT cld_inv_transstockcls.*,unit2,DATE_FORMAT(trans.createddate,'%d-%b-%Y %k:%i:%s') AS createddate,case trans.type when 11 then 'Current' else 'Opening' end as type FROM cld_inv_transstockcls 
                        INNER JOIN cld_inv_cc ON cld_inv_transstockcls.ccid = cld_inv_cc.ID and cld_inv_transstockcls.custid = cld_inv_cc.custid 
                         INNER JOIN cld_inv_stk ON cld_inv_transstockcls.`stkID` = cld_inv_stk.id AND cld_inv_transstockcls.`custId` = cld_inv_stk.`CustID`
 INNER JOIN (SELECT * FROM cld_inv_trans WHERE cld_inv_Trans.invdate = '" & dDate.Date.AddDays(1).ToString("yyyy-MM-dd") & "' AND TYPE in (11,12)) trans 
                        ON cld_inv_transstockcls.`stkID` = trans.stk_id AND cld_inv_transstockcls.`custId` = trans.`CustID` and cld_inv_transstockcls.`ccid`= trans.cc
                        where cld_inv_transstockcls.custid = " & custid & " " & strFilter2 & " And cld_inv_transstockcls.invdate = '" & dDate.Date.ToString("yyyy-MM-dd") & "' and (ccname Like '%" & name & "%' or stkname Like '%" & name & "%') 
                        And bclose = 1 order by stkName" 'And closeqty >0 

        End If

        Dim a As New clsDbo(strFilter)

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function
    Public Function SelectDataset_invStkOpnameExp(ByVal name As String, ByVal custid As Integer, ByVal ModPriv As Integer, ByVal user As String, ByVal ccid As Integer, ByVal dDate As Date) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""
        Dim strFilter2 As String = ""
        If ccid <> 0 Then
            strFilter2 = "and cld_inv_transstockcls.ccid = " & ccid & ""
        End If

        If ModPriv And usrModulepriv.moduleuser = usrModulepriv.moduleuser Then

            strFilter = "SELECT cld_inv_transstockcls.stkno 'Stok No',cld_inv_transstockcls.stkname 'Stok Name',cld_inv_transstockcls.ccname 'Cost Centre',DATE_FORMAT(trans.createddate,'%d-%b-%Y %k:%i:%s') AS 'Created',case trans.type when 11 then 'Current' else 'Opening' end as type,ttlqty,closeqty,varianceqty FROM cld_inv_transstockcls 
                LEFT JOIN cld_inv_cc ON cld_inv_transstockcls.ccid = cld_inv_cc.ID and cld_inv_transstockcls.custid = cld_inv_cc.custid 
                         LEFT JOIN cld_inv_stk ON cld_inv_transstockcls.`stkID` = cld_inv_stk.id AND cld_inv_transstockcls.`custId` = cld_inv_stk.`CustID` 
                        LEFT JOIN (SELECT * FROM cld_inv_trans WHERE cld_inv_Trans.invdate = '" & dDate.Date.AddDays(1).ToString("yyyy-MM-dd") & "' AND TYPE in (11,12)) trans 
                        ON cld_inv_transstockcls.`stkID` = trans.stk_id AND cld_inv_transstockcls.`custId` = trans.`CustID` AND cld_inv_transstockcls.`ccid`= trans.cc
                        where cld_inv_transstockcls.custid = " & custid & " " & strFilter2 & " And cld_inv_transstockcls.invdate = '" & dDate.Date.ToString("yyyy-MM-dd") & "' and (ccname Like '%" & name & "%' or stkname Like '%" & name & "%') 
                        And cld_inv_cc.olid in (Select Store From cld_userxstore where user = '" & user & "') and bclose = 1 and closeqty >0 order by stkName"
        Else
            strFilter = "SELECT cld_inv_transstockcls.stkno 'Stok No',cld_inv_transstockcls.stkname 'Stok Name',cld_inv_transstockcls.ccname 'Cost Centre',DATE_FORMAT(trans.createddate,'%d-%b-%Y %k:%i:%s') AS 'Created',case trans.type when 11 then 'Current' else 'Opening' end as type,ttlqty,closeqty,varianceqty FROM cld_inv_transstockcls 
                        LEFT JOIN cld_inv_cc ON cld_inv_transstockcls.ccid = cld_inv_cc.ID and cld_inv_transstockcls.custid = cld_inv_cc.custid 
                         LEFT JOIN cld_inv_stk ON cld_inv_transstockcls.`stkID` = cld_inv_stk.id AND cld_inv_transstockcls.`custId` = cld_inv_stk.`CustID`
 LEFT JOIN (SELECT * FROM cld_inv_trans WHERE cld_inv_Trans.invdate = '" & dDate.Date.AddDays(1).ToString("yyyy-MM-dd") & "' AND TYPE in (11,12)) trans 
                        ON cld_inv_transstockcls.`stkID` = trans.stk_id AND cld_inv_transstockcls.`custId` = trans.`CustID` AND cld_inv_transstockcls.`ccid`= trans.cc
                        where cld_inv_transstockcls.custid = " & custid & " " & strFilter2 & " And cld_inv_transstockcls.invdate = '" & dDate.Date.ToString("yyyy-MM-dd") & "' and (ccname Like '%" & name & "%' or stkname Like '%" & name & "%') 
                        And bclose = 1 And closeqty >0 order by stkName"

        End If

        Dim a As New clsDbo(strFilter)

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function
    'untuk cek, apakah item inventorynya ada atau ngak
    Public Function SelectDataset_invStkOpnamebyid(ByVal custid As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""


        Dim a As New clsDbo("SELECT * FROM cld_inv_stk  WHERE id Not IN (SELECT stkid FROM cld_inv_transstock where custid = " & custid & ") And custid = " & custid & "")

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function

    Public Function Select_longId(ByVal custid As Integer) As String
        Dim ds As New DataSet

        Dim strFilter As String = ""


        Dim a As New clsDbo("SELECT  * from cld_inv_Trans where MONTH(invDate) = " & Date.Now.Month & " And type =  5 And custid = " & custid & " order by linkid desc")

        ds = a.selectDataset

        If ds.Tables(0).Rows.Count > 0 Then
            strFilter = ds.Tables(0).Rows(0).Item("linkid")
        End If

        ds.Dispose()
        ds = Nothing

        a.dispose()
        a = Nothing

        Return strFilter

    End Function

    Public Function SelectDataset_InvstkOpnameEdit(ByVal stkId As Integer, ByVal ccid As Integer, ByVal custid As Integer, Optional ByVal invdate As DateTime = Nothing) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""
        If invdate = Nothing Then
            invdate = Date.Now.ToString("yyyy-MM-dd 00:00:00")
        Else
            invdate = invdate.ToString("yyyy-MM-dd 00:00:00")
        End If
        'diupdate 26 Nov tambah type 11
        Dim a As New clsDbo("SELECT A.ID FROM cld_inv_trans A WHERE A.stk_id = " & stkId & " And A.type in(5,11,12) And A.invdate = '" & invdate.ToString("yyyy-MM-dd 00:00:00") & "' and A.cc = " & ccid & " and  A.custid = " & custid & "")

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function


    Public Function SelectDataset_InvstkOpnameEdit_ExportImport(ByVal StkNo As String, ByVal sCCName As String, ByVal dDate As Date) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""

        Dim a As New clsDbo("SELECT A.ID FROM cld_inv_trans A WHERE A.Stk_no = '" & StkNo & "' and A.type = 12 and A.invdate = '" & dDate.Date.ToString("yyyy-MM-dd") & "' and A.Cc_name = '" & sCCName & "'")

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function

    Public Function get_ccID_ByName(ByVal sCCName As String) As Integer

        Dim hasil As Integer = 0
        Try

            Dim cCmd As New clsDbo("")
            hasil = cCmd.ExecuteScalar("SELECT ID FROM cld_inv_cc WHERE name = '" & sCCName & "'", 0)

            cCmd = Nothing

        Catch ex As Exception
            errLog(Err.Description, "clsInventory_stkCost:get_stkID_ByName")
            Err.Clear()
        End Try

        Return hasil

    End Function


    '    Public Function SelectDataset_VInvtransstock(ByVal custid As Integer, ByVal dt As Date) As DataSet
    '        Dim ds As New DataSet
    '        Dim strFilter As String = ""

    '        'dasda()

    '        errLog("VInvtransstock Start-" & dt.Date & " Time: " & Now, "clsDBO:SelectDataset_VInvtransstock")

    '        Dim a As New clsDbo("")

    '        'a.Process_InvDataStock(dt.Date, dt.Date)

    '        a.ReNew()


    '        'ds = a.ReturnDataset("SELECT inv.invDate, inv.Custid, inv.stk_id, inv.stk_no, inv.Stk_name, inv.cc, inv.cc_name, CASE inv.bclose " & _
    '        '              " WHEN 1 THEN inv.CloseQty WHEN 0 THEN inv.OpenQty + inv.rcvqty - inv.rtnqty + inv.tinQty - inv.toutQty - inv.rwasteQty - inv.fwasteQty - inv.usageqty + inv.HOin - inv.HOout + inv.BPin - inv.BPOut " & _
    '        '              " ELSE 0 END  AS openqty, stk.factor1, stk.Unit1, stk.Unit2, CASE inv.bclose WHEN 1 THEN inv.closecost WHEN 0 THEN inv.cost END as Cost  FROM cld_inv_stk AS stk " & _
    '        '              " INNER JOIN v_inv_transstock AS inv ON (inv.CustID = stk.CustID) AND (stk.ID = inv.stk_id) " & _
    '        '              " WHERE inv.invDate = '" & dt.ToString("yyyy-MM-dd") & "' and inv.custid = " & custid & "")



    '        ds = a.ReturnDataset("SELECT " & _
    '"`i_trans`.`invDate`    AS `invDate`, `cld_inv_stk`.`CustID` AS `Custid`, " & _
    '"`i_trans`.`stk_id`     AS `stk_id`, `cld_inv_stk`.`Number` AS `stk_No`, " & _
    '"`cld_inv_stk`.`Name`   AS `Stk_name`, `i_trans`.`Cc` AS `cc`, `cc`.`Name` AS `cc_name`, " & _
    '"cld_inv_stk.factor1, cld_inv_stk.Unit1, cld_inv_stk.Unit2, " & _
    '"IFNULL((SELECT `sd`.`ttlStock` FROM `cld_inv_trans` `sd` WHERE ((`sd`.`type` = 6) AND (`sd`.`flagtrans` = 1) AND (`sd`.`CustID` = `i_trans`.`custid`) AND (`sd`.`Stk_Id` = `i_trans`.`stk_id`) AND (`sd`.`InvDate` = `i_trans`.`invDate`) AND (`sd`.`Cc` = `i_trans`.`Cc`))),0) AS `OpenQty`, " & _
    '"IFNULL((SELECT `sd`.`Cost` FROM `cld_inv_stkcost` `sd` WHERE ((`sd`.`Custid` = `i_trans`.`custid`) AND (`sd`.`stkID` = `i_trans`.`stk_id`) AND (`sd`.`cc` = `i_trans`.`Cc`))),0) AS `Cost`, " & _
    '"IFNULL((SELECT SUM(`sd`.`ttlStock`) AS `ds` FROM `cld_inv_trans` `sd` WHERE ((`sd`.`type` = 3) AND (`sd`.`flagtrans` = 1) AND (`sd`.`CustID` = `i_trans`.`custid`) AND (`sd`.`Stk_Id` = `i_trans`.`stk_id`) AND (`sd`.`InvDate` = `i_trans`.`invDate`) AND (`sd`.`Cc` = `i_trans`.`Cc`))),0) AS `rcvQty`, " & _
    '"IFNULL((SELECT SUM(`sd`.`ttlStock`) AS `ds` FROM `cld_inv_trans` `sd` WHERE ((`sd`.`type` = 4) AND (`sd`.`flagtrans` = 1) AND (`sd`.`CustID` = `i_trans`.`custid`) AND (`sd`.`Stk_Id` = `i_trans`.`stk_id`) AND (`sd`.`InvDate` = `i_trans`.`invDate`) AND (`sd`.`Cc` = `i_trans`.`Cc`))),0) AS `rtnqty`, " & _
    '"IFNULL((SELECT SUM(`sd`.`ttlStock`) AS `ds` FROM `cld_inv_trans` `sd` WHERE ((`sd`.`type` = 1) AND (`sd`.`flagtrans` = 1) AND (`sd`.`CustID` = `i_trans`.`custid`) AND (`sd`.`Stk_Id` = `i_trans`.`stk_id`) AND (`sd`.`InvDate` = `i_trans`.`invDate`) AND (`sd`.`Cc` = `i_trans`.`Cc`))),0) AS `tinQty`, " & _
    '"IFNULL((SELECT SUM(`sd`.`ttlStock`) AS `ds` FROM (`cld_inv_trans` `sd` JOIN `cld_invlk_trf` ON(((`sd`.`Linkid` = `cld_invlk_trf`.`id`) AND (`sd`.`CustID` = `cld_invlk_trf`.`CustID`)))) WHERE ((`sd`.`type` = 1) AND (`sd`.`flagtrans` = 1) AND (`cld_invlk_trf`.`CustID` = `i_trans`.`custid`) AND (`sd`.`Stk_Id` = `i_trans`.`stk_id`) AND (`sd`.`InvDate` = `i_trans`.`invDate`) AND (`cld_invlk_trf`.`ccFrom` = `i_trans`.`Cc`))),0) AS `toutQty`, " & _
    '"IFNULL((SELECT SUM(`sd`.`ttlStock`) AS `ds` FROM `cld_inv_trans` `sd` WHERE ((`sd`.`type` = 2) AND (`sd`.`flagtrans` = 1) AND (`sd`.`CustID` = `i_trans`.`custid`) AND (`sd`.`Stk_Id` = `i_trans`.`stk_id`) AND (`sd`.`InvDate` = `i_trans`.`invDate`) AND (`sd`.`Cc` = `i_trans`.`Cc`))),0) AS `rwasteQty`, " & _
    '"0 AS `fwasteQty`, " & _
    '"IFNULL((SELECT SUM(`sd`.`ttlStock`) AS `ds` FROM `cld_inv_transusage` `sd` WHERE ((`sd`.`type` = 10) AND (`sd`.`flagtrans` = 1) AND (`sd`.`CustID` = `i_trans`.`custid`) AND (`sd`.`Stk_Id` = `i_trans`.`stk_id`) AND (`sd`.`InvDate` = `i_trans`.`invDate`) AND (`sd`.`Cc` = `i_trans`.`Cc`))),0) AS `usageqty`, " & _
    '"IFNULL((SELECT `sd`.`flagtrans` FROM `cld_inv_trans` `sd` WHERE ((`sd`.`type` = 5) AND (`sd`.`flagtrans` = 1) AND (`sd`.`CustID` = `i_trans`.`custid`) AND (`sd`.`Stk_Id` = `i_trans`.`stk_id`) AND (`sd`.`InvDate` = `i_trans`.`invDate`) AND (`sd`.`Cc` = `i_trans`.`Cc`))),0) AS `bClose`, " & _
    '"IFNULL((SELECT `sd`.`ttlStock` FROM `cld_inv_trans` `sd` WHERE ((`sd`.`type` = 5) AND (`sd`.`flagtrans` = 1) AND (`sd`.`CustID` = `i_trans`.`custid`) AND (`sd`.`Stk_Id` = `i_trans`.`stk_id`) AND (`sd`.`InvDate` = `i_trans`.`invDate`) AND (`sd`.`Cc` = `i_trans`.`Cc`))),0) AS `CloseQty`, " & _
    '"IFNULL((SELECT `sd`.`Cost` FROM `cld_inv_trans` `sd` WHERE ((`sd`.`type` = 5) AND (`sd`.`flagtrans` = 1) AND (`sd`.`CustID` = `i_trans`.`custid`) AND (`sd`.`Stk_Id` = `i_trans`.`stk_id`) AND (`sd`.`InvDate` = `i_trans`.`invDate`) AND (`sd`.`Cc` = `i_trans`.`Cc`))),0) AS `CloseCost`, " & _
    '"IFNULL((SELECT SUM(`sd`.`ttlStock`) AS `ds` FROM `cld_inv_trans` `sd` WHERE ((`sd`.`type` = 7) AND (`sd`.`flagtrans` = 1) AND (`sd`.`CustID` = `i_trans`.`custid`) AND (`sd`.`Stk_Id` = `i_trans`.`stk_id`) AND (`sd`.`InvDate` = `i_trans`.`invDate`) AND (`sd`.`Cc` = `i_trans`.`Cc`))),0) AS `HOout`, " & _
    '"IFNULL((SELECT SUM(`sd`.`ttlStock`) AS `ds` FROM `cld_inv_trans` `sd` WHERE ((`sd`.`type` = 8) AND (`sd`.`flagtrans` = 1) AND (`sd`.`CustID` = `i_trans`.`custid`) AND (`sd`.`Stk_Id` = `i_trans`.`stk_id`) AND (`sd`.`InvDate` = `i_trans`.`invDate`) AND (`sd`.`Cc` = `i_trans`.`Cc`))),0) AS `HOIn`, " & _
    '"IFNULL((SELECT SUM(`sd`.`ttlStock`) AS `ds` FROM `cld_inv_trans` `sd` WHERE ((`sd`.`type` = 9) AND (`sd`.`flagtrans` = 1) AND (`sd`.`CustID` = `i_trans`.`custid`) AND (`sd`.`Stk_Id` = `i_trans`.`stk_id`) AND (`sd`.`InvDate` = `i_trans`.`invDate`) AND (`sd`.`Cc` = `i_trans`.`Cc`))),0) AS `BPIn`, " & _
    '"IFNULL((SELECT SUM(`sd`.`ttlStock`) AS `ds` FROM `cld_inv_transusage` `sd` WHERE ((`sd`.`type` = 11) AND (`sd`.`flagtrans` = 1) AND (`sd`.`CustID` = `i_trans`.`custid`) AND (`sd`.`Stk_Id` = `i_trans`.`stk_id`) AND (`sd`.`InvDate` = `i_trans`.`invDate`) AND (`sd`.`Cc` = `i_trans`.`Cc`))),0) AS `BPOut` " & _
    '"FROM (`cld_inv_stkdata` `i_trans` INNER JOIN `cld_inv_stk` " & _
    '"ON ((`cld_inv_stk`.`ID` = `i_trans`.`stk_id`))) INNER JOIN `cld_inv_cc` `cc` " & _
    '"ON ((`cc`.`ID` = `i_trans`.`Cc`)) " & _
    '"WHERE i_trans.`invDate` = '" & dt.Date.ToString("yyyy-MM-dd") & "' AND I_trans.`custid` = " & custid)


    '        a.dispose()
    '        a = Nothing


    '        errLog("VInvtransstock END" & Now, "clsDBO:SelectDataset_VInvtransstock")

    '        Return ds

    '    End Function


    'Public Sub insert_invtransstk_opening(ByVal custid As Integer, ByVal maxdate As Date, ByVal cc As Integer, ByVal stkid As Integer,
    '                           ByVal stkname As String, ByVal u2 As Decimal, ByVal factor1 As Decimal, ByVal stkno As String,
    '                           ByVal u1_nm As String, ByVal u2_nm As String, ByVal cost As Decimal, ByVal ccname As String, ByVal username As String)
    '    Dim c_cmd As New clsDbo("")


    '    Dim str As String
    '    str = "insert into cld_inv_Trans values (" & c_cmd.ExecuteScalar("select Max(ID) from cld_inv_Trans") & ", " & custid & ", DATE_ADD('" & maxdate.ToString("yyyy-MM-dd") & "', INTERVAL 1 DAY), 6, " & cc & ", " &
    '                    " " & stkid & ", '" & stkname & "', 0, " & u2 & ", " & factor1 & ", " & u2 & ", 0," & cost & ", " &
    '                    " '', 1, '" & u1_nm & "', '" & u2_nm & "', '" & username & "', DATE_ADD('" & maxdate.ToString("yyyy-MM-dd HH:mm:ss") & "', INTERVAL 1 DAY), Null, Null, '" & stkno & "', '" & ccname & "')"
    '    c_cmd.executeNonQuery(str)
    '    c_cmd = Nothing

    'End Sub
    'Public Sub insert_invtrans_opening(ByVal custid As Integer, ByVal maxdate As Date, ByVal cc As Integer, ByVal stkid As Integer, _
    '                           ByVal stkname As String, ByVal u2 As Decimal, ByVal factor1 As Decimal, ByVal stkno As String, _
    '                           ByVal u1_nm As String, ByVal u2_nm As String, ByVal cost As Decimal, ByVal ccname As String, ByVal username As String)
    '    Dim c_cmd As New clsDbo("")


    '    Dim str As String
    '    str = "insert into cld_inv_Trans values (" & c_cmd.ExecuteScalar("select Max(ID) from cld_inv_Trans") & ", " & custid & ", DATE_ADD('" & maxdate.ToString("yyyy-MM-dd") & "', INTERVAL 1 DAY), 6, " & cc & ", " & _
    '                    " " & stkid & ", '" & stkname & "', 0, " & u2 & ", " & factor1 & ", " & u2 & ", 0," & cost & ", " & _
    '                    " '', 1, '" & u1_nm & "', '" & u2_nm & "', '" & username & "', DATE_ADD('" & maxdate.ToString("yyyy-MM-dd HH:mm:ss") & "', INTERVAL 1 DAY), Null, Null, '" & stkno & "', '" & ccname & "')"
    '    c_cmd.executeNonQuery(str)
    '    c_cmd = Nothing

    'End Sub


    'Tambahan 18 Nov 2020 - Export
    Public Function selectdata_invstockopname(ByVal custID As Integer, ByVal paramFrom As Date, ByVal cc As Integer) As DataSet

        Dim ds As New DataSet
        Dim strFilter As String = ""

        'strFilter = "SELECT trans.stk_no, trans.stk_Name, trans.u1, trans.u1_nm , trans.u2, trans.u2_nm, trans.Factor1, " &
        '                   "trans.cost, cld_inv_cc.Name, 1 AS BClose " &
        '                   "FROM cld_inv_trans trans INNER JOIN cld_inv_stk ON trans.stk_id = cld_inv_stk.ID INNER JOIN cld_inv_cc ON trans.Cc = cld_inv_cc.id" &
        '                   " WHERE trans.Cc_name = '" & sCCname & "' and trans.type = 5 AND  trans.invDate ='" & paramFrom.ToString("yyyy-MM-dd") & "'"

        strFilter = "  SELECT stk.number stk_no, stk.name stk_Name, 0 AS u1, stk.unit1 u1_nm , IFNULL(cls.ttlqty,0) u2, stk.unit2 u2_nm, stk.Factor1, 
                           IFNULL( cls.cost,0) cost, cc.Name FROM cld_inv_stk stk
                        LEFT JOIN (SELECT* FROM cld_inv_cc WHERE id = " & cc & ") cc ON stk.custid = cc.custid
                        LEFT JOIN (SELECT * FROM cld_inv_transstockcls WHERE invdate = '" & paramFrom.ToString("yyyy-MM-dd") & "' AND ccid = " & cc & ") cls ON cls.stkid = stk.ID AND cls.custid = stk.custid"
        ', 1 AS BClose 
        Dim a As New clsDbo(strFilter)

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function

    Public Function SelectDataset_invStkOpnameSearch2(ByVal name As String, ByVal ModPriv As Integer, ByVal user As String) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""
        Dim strFilter2 As String = ""

        If ModPriv And usrModulepriv.moduleuser = usrModulepriv.moduleuser Then

            strFilter = "(SELECT cld_inv_transstockcls.invdate,DATE_FORMAT(cld_inv_transstockcls.invdate,'%d-%b-%Y')inv_date,cld_inv_transstockcls.custid,stkid,Stkno,stkname,ccid,ccname,opencost,openqty,cld_inv_transstockcls.cost,ttlqty,bclose,closeqty,closecost,varianceqty,u1,unit1,cld_inv_stk.factor1,u2,unit2,DATE_FORMAT(trans.createddate,'%d-%b-%Y %k:%i:%s') AS createddate,trans.type trtype,CASE trans.type WHEN 11 THEN 'Current' ELSE 'Opening' END AS TYPE FROM cld_inv_transstockcls 
                        INNER JOIN cld_inv_cc ON cld_inv_transstockcls.ccid = cld_inv_cc.ID AND cld_inv_transstockcls.custid = cld_inv_cc.custid 
                         INNER JOIN cld_inv_stk ON cld_inv_transstockcls.`stkID` = cld_inv_stk.id AND cld_inv_transstockcls.`custId` = cld_inv_stk.`CustID`
                        INNER JOIN (SELECT * FROM cld_inv_trans WHERE cld_inv_Trans.invdate = '" & Now.Date.ToString("yyyy-MM-dd") & "' AND TYPE in (11,12)) trans 
                        ON cld_inv_transstockcls.`stkID` = trans.stk_id AND cld_inv_transstockcls.`custId` = trans.`CustID` and cld_inv_transstockcls.`ccid`= trans.cc
                        where cld_inv_transstockcls.invdate = '" & Now.Date.AddDays(-1).ToString("yyyy-MM-dd") & "' and (ccname Like '%" & name & "%' or stkname Like '%" & name & "%') 
                        And cld_inv_cc.olid in (Select Store From cld_userxstore where user = '" & user & "') and bclose = 1)
                        UNION ALL
                        (select cld_inv_transstockcls.invdate,DATE_FORMAT(cld_inv_transstockcls.invdate,'%d-%b-%Y')inv_date,cld_inv_transstockcls.custid,stkid,Stkno,stkname,ccid,ccname,opencost,openqty,cld_inv_transstockcls.cost,ttlqty,bclose,closeqty,closecost,varianceqty,0 u1,unit1,cld_inv_stk.factor1,0 u2,unit2,DATE_FORMAT(invdate,'%d-%b-%Y %k:%i:%s') AS createddate,0 trtype,'' AS TYPE FROM cld_inv_transstockcls 
                        INNER JOIN cld_inv_cc ON cld_inv_transstockcls.ccid = cld_inv_cc.ID AND cld_inv_transstockcls.custid = cld_inv_cc.custid 
                         INNER JOIN cld_inv_stk ON cld_inv_transstockcls.`stkID` = cld_inv_stk.id AND cld_inv_transstockcls.`custId` = cld_inv_stk.`CustID`

                        WHERE cld_inv_transstockcls.invdate = '" & Now.Date.AddDays(-1).ToString("yyyy-MM-dd") & "' AND (ccname LIKE '%" & name & "%' OR stkname LIKE '%" & name & "%') 
                        And cld_inv_cc.olid in (Select Store From cld_userxstore where user = '" & user & "') and bclose <> 1)" 'and closeqty >0 
        Else
            strFilter = "(SELECT cld_inv_transstockcls.invdate,DATE_FORMAT(cld_inv_transstockcls.invdate,'%d-%b-%Y')inv_date,cld_inv_transstockcls.custid,stkid,Stkno,stkname,ccid,ccname,opencost,openqty,cld_inv_transstockcls.cost,ttlqty,bclose,closeqty,closecost,varianceqty,u1,unit1,cld_inv_stk.factor1,u2,unit2,DATE_FORMAT(trans.createddate,'%d-%b-%Y %k:%i:%s') AS createddate,trans.type trtype,CASE trans.type WHEN 11 THEN 'Current' ELSE 'Opening' END AS TYPE FROM cld_inv_transstockcls 
                        INNER JOIN cld_inv_cc ON cld_inv_transstockcls.ccid = cld_inv_cc.ID AND cld_inv_transstockcls.custid = cld_inv_cc.custid 
                         INNER JOIN cld_inv_stk ON cld_inv_transstockcls.`stkID` = cld_inv_stk.id AND cld_inv_transstockcls.`custId` = cld_inv_stk.`CustID`
                        INNER JOIN (SELECT * FROM cld_inv_trans WHERE cld_inv_Trans.invdate = '" & Now.Date.ToString("yyyy-MM-dd") & "' AND TYPE in (11,12)) trans 
                        ON cld_inv_transstockcls.`stkID` = trans.stk_id AND cld_inv_transstockcls.`custId` = trans.`CustID` and cld_inv_transstockcls.`ccid`= trans.cc
                        where cld_inv_transstockcls.invdate = '" & Now.Date.AddDays(-1).ToString("yyyy-MM-dd") & "' and (ccname Like '%" & name & "%' or stkname Like '%" & name & "%') 
                        and bclose = 1)
                        UNION ALL
                        (select cld_inv_transstockcls.invdate,DATE_FORMAT(cld_inv_transstockcls.invdate,'%d-%b-%Y')inv_date,cld_inv_transstockcls.custid,stkid,Stkno,stkname,ccid,ccname,opencost,openqty,cld_inv_transstockcls.cost,ttlqty,bclose,closeqty,closecost,varianceqty,0 u1,unit1,cld_inv_stk.factor1,0 u2,unit2,DATE_FORMAT(invdate,'%d-%b-%Y %k:%i:%s') AS createddate,0 trtype,'' AS TYPE FROM cld_inv_transstockcls 
                        INNER JOIN cld_inv_cc ON cld_inv_transstockcls.ccid = cld_inv_cc.ID AND cld_inv_transstockcls.custid = cld_inv_cc.custid 
                         INNER JOIN cld_inv_stk ON cld_inv_transstockcls.`stkID` = cld_inv_stk.id AND cld_inv_transstockcls.`custId` = cld_inv_stk.`CustID`

                        WHERE cld_inv_transstockcls.invdate = '" & Now.Date.AddDays(-1).ToString("yyyy-MM-dd") & "' AND (ccname LIKE '%" & name & "%' OR stkname LIKE '%" & name & "%') 
                        and bclose <> 1)" 'And closeqty >0 

        End If

        Dim a As New clsDbo(strFilter)

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function

End Class
