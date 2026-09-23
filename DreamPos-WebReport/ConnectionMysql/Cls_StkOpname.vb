Public Class Cls_StkOpname

    Public Function SelectDataset_invStkOpname(ByVal custid As Integer, ByVal ModPriv As Integer, ByVal user As String) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""

        'revisi 27 feb 2017, sekarang berdasarkan user, jadi ada user yang hanya dapat lihat pergerakan inventory tokonya saja
        'Dim a As New clsDbo("SELECT *, IFNULL((SELECT ttlstock FROM cld_inv_trans WHERE custid = cld_inv_transstock.custid AND TYPE = 5 AND stk_id = cld_inv_transstock.stkid AND cc = cld_inv_transstock.ccid ) , 0) AS ttlstk   FROM cld_inv_transstock where custid = " & custid & "")

        If ModPriv And usrModulepriv.moduleuser = usrModulepriv.moduleuser Then

            'strFilter = "SELECT *, IFNULL((SELECT ttlstock FROM cld_inv_trans WHERE custid = cld_inv_transstock.custid AND TYPE = 5 AND stk_id = cld_inv_transstock.stkid AND cc = cld_inv_transstock.ccid ) , 0) AS ttlstk FROM cld_inv_transstock INNER JOIN cld_inv_cc ON cld_inv_transstock.ccid = cld_inv_cc.ID and cld_inv_transstock.custid = cld_inv_cc.custid " & _
            '            "where cld_inv_transstock.custid = " & custid & " and cld_inv_cc.olid IN (SELECT Store FROM cld_userxstore WHERE USER = '" & user & "')"

            strFilter = "SELECT * FROM cld_inv_transstock INNER JOIN cld_inv_cc ON cld_inv_transstock.ccid = cld_inv_cc.ID and cld_inv_transstock.custid = cld_inv_cc.custid " &
                       "where cld_inv_transstock.custid = " & custid & " and cld_inv_cc.olid IN (SELECT Store FROM cld_userxstore WHERE USER = '" & user & "')"
        Else

            'strFilter = "SELECT *, IFNULL((SELECT ttlstock FROM cld_inv_trans WHERE custid = cld_inv_transstock.custid AND TYPE = 5 AND stk_id = cld_inv_transstock.stkid AND cc = cld_inv_transstock.ccid ) , 0) AS ttlstk FROM cld_inv_transstock " & _
            '            "where cld_inv_transstock.custid = " & custid

            strFilter = "SELECT * FROM cld_inv_transstock " & _
                       "where cld_inv_transstock.custid = " & custid

        End If


        Dim a As New clsDbo(strFilter)

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function

    Public Function SelectDataset_invStkOpnameSearch(ByVal name As String, ByVal custid As Integer, ByVal ModPriv As Integer, ByVal user As String) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""

        'revisi 27 feb 2017, sekarang berdasarkan user, jadi ada user yang hanya dapat lihat pergerakan inventory tokonya saja
        'Dim a As New clsDbo("SELECT *, IFNULL((SELECT ttlstock FROM cld_inv_trans WHERE custid = cld_inv_transstock.custid AND TYPE = 5 AND stk_id = cld_inv_transstock.stkid AND cc = cld_inv_transstock.ccid ) , 0) AS ttlstk   FROM cld_inv_transstock where custid = " & custid & "  and (ccname Like '%" & name & "%' or stkname Like '%" & name & "%')")


        If ModPriv And usrModulepriv.moduleuser = usrModulepriv.moduleuser Then

            strFilter = "SELECT * FROM cld_inv_transstock INNER JOIN cld_inv_cc ON cld_inv_transstock.ccid = cld_inv_cc.ID and cld_inv_transstock.custid = cld_inv_cc.custid " &
                        "where cld_inv_transstock.custid = " & custid & " and (ccname Like '%" & name & "%' or stkname Like '%" & name & "%') and cld_inv_cc.olid IN (SELECT Store FROM cld_userxstore WHERE USER = '" & user & "')"
        Else
            strFilter = "SELECT * FROM cld_inv_transstock " & _
                        "where cld_inv_transstock.custid = " & custid & " and (ccname Like '%" & name & "%' or stkname Like '%" & name & "%')"

        End If

        Dim a As New clsDbo(strFilter)

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function

    Public Function SelectDataset_invStkOpnamebyid(ByVal custid As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""


        Dim a As New clsDbo("SELECT * FROM cld_inv_stk  WHERE id NOT IN (SELECT stkid FROM cld_inv_transstock where custid = " & custid & ") and custid = " & custid & "")

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function
   
    Public Function SelectDataset_longId(ByVal custid As Integer) As String
        Dim ds As New DataSet

        Dim strFilter As String = ""


        Dim a As New clsDbo("SELECT  * from cld_inv_Trans where MONTH(invDate) = " & Date.Now.Month & " and type =  5 and custid = " & custid & " order by linkid desc")

        ds = a.selectDataset

        If ds.Tables(0).Rows.Count > 0 Then
            strFilter = ds.Tables(0).Rows(0).Item("linkid")

        End If
        a.dispose()
        a = Nothing

        Return strFilter

    End Function

    Public Function SelectDataset_InvstkOpnameEdit(ByVal stkId As Integer, ByVal ccid As Integer, ByVal custid As Integer) As DataSet
        '8Nov2023 perubahan type = 12 dari type = 5
        Dim ds As New DataSet
        Dim strFilter As String = ""
        'Dim a As New clsDbo("SELECT A.*, B.closeCost AS TotCost FROM cld_inv_trans A LEFT JOIN cld_inv_transstock B ON  A.Stk_Id = B.stkId AND A.Cc = B.ccid AND A.CustID = B.custId WHERE A.stk_id = " & stkId & " and A.type = 5 and invdate = '" & Date.Now.Date.ToString("yyyy-MM-dd") & "' and A.cc = " & ccid & " and  A.custid = " & custid & "")


        Dim a As New clsDbo("SELECT A.*, B.Cost AS TotCost,B.ttlqty FROM cld_inv_trans A LEFT JOIN (select * from cld_inv_transstockcls where 
invdate = '" & Now.ToString("yyyy-MM-dd 00:00:00") & "') B ON  A.Stk_Id = B.stkId AND A.Cc = B.ccid AND A.CustID = B.custId WHERE A.stk_id = " & stkId & " and A.type = 12 and a.invdate = '" & Date.Now.Date.ToString("yyyy-MM-dd") & "' and A.cc = " & ccid & " and  A.custid = " & custid & "")

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function
    Public Function SelectDataset_VInvtransstock(ByVal custid As Integer, ByVal dt As Date) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""
        Dim a As New clsDbo("")



        a.ReNew()


        'Dim a As New clsDbo("SELECT inv.invDate, inv.Custid, inv.stk_id, inv.stk_no, inv.Stk_name, inv.cc, inv.cc_name, CASE inv.bclose " & _
        '                 " WHEN 1 THEN inv.CloseQty WHEN 0 THEN inv.OpenQty + inv.rcvqty - inv.rtnqty + inv.tinQty - inv.toutQty - inv.rwasteQty - inv.fwasteQty - inv.usageqty " & _
        '                 " ELSE 0 END  AS openqty, stk.factor1, stk.Unit1, stk.Unit2, CASE inv.bclose WHEN 1 THEN inv.closecost WHEN 0 THEN inv.cost END as Cost  FROM cld_inv_stk AS stk " & _
        '                 " INNER JOIN v_inv_transstock AS inv ON (inv.CustID = stk.CustID) AND (stk.ID = inv.stk_id) " & _
        '                 " WHERE inv.invDate = '" & dt.ToString("yyyy-MM-dd") & "' and inv.custid = " & custid & "")

        'perubahan 18 sept 2017
        'jadi untuk openingnya dtambahn - hoout dan + hoin

        'Dim a As New clsDbo("SELECT inv.invDate, inv.Custid, inv.stk_id, inv.stk_no, inv.Stk_name, inv.cc, inv.cc_name, CASE inv.bclose " & _
        '              " WHEN 1 THEN inv.CloseQty WHEN 0 THEN inv.OpenQty + inv.rcvqty - inv.rtnqty + inv.tinQty - inv.toutQty - inv.rwasteQty - inv.fwasteQty - inv.usageqty + inv.HOin - inv.HOout + inv.BPin - inv.BPOut " & _
        '              " ELSE 0 END  AS openqty, stk.factor1, stk.Unit1, stk.Unit2, CASE inv.bclose WHEN 1 THEN inv.closecost WHEN 0 THEN inv.cost END as Cost  FROM cld_inv_stk AS stk " & _
        '              " INNER JOIN v_inv_transstock AS inv ON (inv.CustID = stk.CustID) AND (stk.ID = inv.stk_id) " & _
        '              " WHERE inv.invDate = '" & dt.ToString("yyyy-MM-dd") & "' and inv.custid = " & custid & "")

        ds = a.ReturnDataset("SELECT " &
"`i_trans`.`invDate`    AS `invDate`, `cld_inv_stk`.`CustID` AS `Custid`, " &
"`i_trans`.`stk_id`     AS `stk_id`, `cld_inv_stk`.`Number` AS `stk_No`, " &
"`cld_inv_stk`.`Name`   AS `Stk_name`, `i_trans`.`Cc` AS `cc`, `cc`.`Name` AS `cc_name`, " &
"cld_inv_stk.factor1, cld_inv_stk.Unit1, cld_inv_stk.Unit2, " &
"IFNULL((SELECT `sd`.`ttlStock` FROM `cld_inv_trans` `sd` WHERE ((`sd`.`type` = 6) AND (`sd`.`flagtrans` = 1) AND (`sd`.`CustID` = `i_trans`.`custid`) AND (`sd`.`Stk_Id` = `i_trans`.`stk_id`) AND (`sd`.`InvDate` = `i_trans`.`invDate`) AND (`sd`.`Cc` = `i_trans`.`Cc`))),0) AS `OpenQty`, " &
"IFNULL((SELECT `sd`.`Cost` FROM `cld_inv_stkcost` `sd` WHERE ((`sd`.`Custid` = `i_trans`.`custid`) AND (`sd`.`stkID` = `i_trans`.`stk_id`) AND (`sd`.`cc` = `i_trans`.`Cc`))),0) AS `Cost`, " &
"IFNULL((SELECT SUM(`sd`.`ttlStock`) AS `ds` FROM `cld_inv_trans` `sd` WHERE ((`sd`.`type` = 3) AND (`sd`.`flagtrans` = 1) AND (`sd`.`CustID` = `i_trans`.`custid`) AND (`sd`.`Stk_Id` = `i_trans`.`stk_id`) AND (`sd`.`InvDate` = `i_trans`.`invDate`) AND (`sd`.`Cc` = `i_trans`.`Cc`))),0) AS `rcvQty`, " &
"IFNULL((SELECT SUM(`sd`.`ttlStock`) AS `ds` FROM `cld_inv_trans` `sd` WHERE ((`sd`.`type` = 4) AND (`sd`.`flagtrans` = 1) AND (`sd`.`CustID` = `i_trans`.`custid`) AND (`sd`.`Stk_Id` = `i_trans`.`stk_id`) AND (`sd`.`InvDate` = `i_trans`.`invDate`) AND (`sd`.`Cc` = `i_trans`.`Cc`))),0) AS `rtnqty`, " &
"IFNULL((SELECT SUM(`sd`.`ttlStock`) AS `ds` FROM `cld_inv_trans` `sd` WHERE ((`sd`.`type` = 1) AND (`sd`.`flagtrans` = 1) AND (`sd`.`CustID` = `i_trans`.`custid`) AND (`sd`.`Stk_Id` = `i_trans`.`stk_id`) AND (`sd`.`InvDate` = `i_trans`.`invDate`) AND (`sd`.`Cc` = `i_trans`.`Cc`))),0) AS `tinQty`, " &
"IFNULL((SELECT SUM(`sd`.`ttlStock`) AS `ds` FROM (`cld_inv_trans` `sd` JOIN `cld_invlk_trf` ON(((`sd`.`Linkid` = `cld_invlk_trf`.`id`) AND (`sd`.`CustID` = `cld_invlk_trf`.`CustID`)))) WHERE ((`sd`.`type` = 1) AND (`sd`.`flagtrans` = 1) AND (`cld_invlk_trf`.`CustID` = `i_trans`.`custid`) AND (`sd`.`Stk_Id` = `i_trans`.`stk_id`) AND (`sd`.`InvDate` = `i_trans`.`invDate`) AND (`cld_invlk_trf`.`ccFrom` = `i_trans`.`Cc`))),0) AS `toutQty`, " &
"IFNULL((SELECT SUM(`sd`.`ttlStock`) AS `ds` FROM `cld_inv_trans` `sd` WHERE ((`sd`.`type` = 2) AND (`sd`.`flagtrans` = 1) AND (`sd`.`CustID` = `i_trans`.`custid`) AND (`sd`.`Stk_Id` = `i_trans`.`stk_id`) AND (`sd`.`InvDate` = `i_trans`.`invDate`) AND (`sd`.`Cc` = `i_trans`.`Cc`))),0) AS `rwasteQty`, " &
"0 AS `fwasteQty`, " &
"IFNULL((SELECT SUM(`sd`.`ttlStock`) AS `ds` FROM `cld_inv_transusage` `sd` WHERE ((`sd`.`type` = 10) AND (`sd`.`flagtrans` = 1) AND (`sd`.`CustID` = `i_trans`.`custid`) AND (`sd`.`Stk_Id` = `i_trans`.`stk_id`) AND (`sd`.`InvDate` = `i_trans`.`invDate`) AND (`sd`.`Cc` = `i_trans`.`Cc`))),0) AS `usageqty`, " &
"IFNULL((SELECT `sd`.`flagtrans` FROM `cld_inv_trans` `sd` WHERE ((`sd`.`type` = 5) AND (`sd`.`flagtrans` = 1) AND (`sd`.`CustID` = `i_trans`.`custid`) AND (`sd`.`Stk_Id` = `i_trans`.`stk_id`) AND (`sd`.`InvDate` = `i_trans`.`invDate`) AND (`sd`.`Cc` = `i_trans`.`Cc`))),0) AS `bClose`, " &
"IFNULL((SELECT `sd`.`ttlStock` FROM `cld_inv_trans` `sd` WHERE ((`sd`.`type` = 5) AND (`sd`.`flagtrans` = 1) AND (`sd`.`CustID` = `i_trans`.`custid`) AND (`sd`.`Stk_Id` = `i_trans`.`stk_id`) AND (`sd`.`InvDate` = `i_trans`.`invDate`) AND (`sd`.`Cc` = `i_trans`.`Cc`))),0) AS `CloseQty`, " &
"IFNULL((SELECT `sd`.`Cost` FROM `cld_inv_trans` `sd` WHERE ((`sd`.`type` = 5) AND (`sd`.`flagtrans` = 1) AND (`sd`.`CustID` = `i_trans`.`custid`) AND (`sd`.`Stk_Id` = `i_trans`.`stk_id`) AND (`sd`.`InvDate` = `i_trans`.`invDate`) AND (`sd`.`Cc` = `i_trans`.`Cc`))),0) AS `CloseCost`, " &
"IFNULL((SELECT SUM(`sd`.`ttlStock`) AS `ds` FROM `cld_inv_trans` `sd` WHERE ((`sd`.`type` = 7) AND (`sd`.`flagtrans` = 1) AND (`sd`.`CustID` = `i_trans`.`custid`) AND (`sd`.`Stk_Id` = `i_trans`.`stk_id`) AND (`sd`.`InvDate` = `i_trans`.`invDate`) AND (`sd`.`Cc` = `i_trans`.`Cc`))),0) AS `HOout`, " &
"IFNULL((SELECT SUM(`sd`.`ttlStock`) AS `ds` FROM `cld_inv_trans` `sd` WHERE ((`sd`.`type` = 8) AND (`sd`.`flagtrans` = 1) AND (`sd`.`CustID` = `i_trans`.`custid`) AND (`sd`.`Stk_Id` = `i_trans`.`stk_id`) AND (`sd`.`InvDate` = `i_trans`.`invDate`) AND (`sd`.`Cc` = `i_trans`.`Cc`))),0) AS `HOIn`, " &
"IFNULL((SELECT SUM(`sd`.`ttlStock`) AS `ds` FROM `cld_inv_trans` `sd` WHERE ((`sd`.`type` = 9) AND (`sd`.`flagtrans` = 1) AND (`sd`.`CustID` = `i_trans`.`custid`) AND (`sd`.`Stk_Id` = `i_trans`.`stk_id`) AND (`sd`.`InvDate` = `i_trans`.`invDate`) AND (`sd`.`Cc` = `i_trans`.`Cc`))),0) AS `BPIn`, " &
"IFNULL((SELECT SUM(`sd`.`ttlStock`) AS `ds` FROM `cld_inv_transusage` `sd` WHERE ((`sd`.`type` = 11) AND (`sd`.`flagtrans` = 1) AND (`sd`.`CustID` = `i_trans`.`custid`) AND (`sd`.`Stk_Id` = `i_trans`.`stk_id`) AND (`sd`.`InvDate` = `i_trans`.`invDate`) AND (`sd`.`Cc` = `i_trans`.`Cc`))),0) AS `BPOut` " &
"FROM (`cld_inv_stkdata` `i_trans` INNER JOIN `cld_inv_stk` " &
"ON ((`cld_inv_stk`.`ID` = `i_trans`.`stk_id`))) INNER JOIN `cld_inv_cc` `cc` " &
"ON ((`cc`.`ID` = `i_trans`.`Cc`)) " &
"WHERE i_trans.`invDate` = '" & dt.Date.ToString("yyyy-MM-dd") & "' AND I_trans.`custid` = " & custid)

        'ds = a.selectDataset

        a.dispose()
        a = Nothing
        errLog("VInvtransstock END" & Now, "clsDBO:SelectDataset_VInvtransstock")
        Return ds

    End Function
    Public Function SelectDataset_Maxdate(ByVal custid As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""


        Dim a As New clsDbo("SELECT MAX(invdate) as maxdate, day(MAX(invdate)) as maxday FROM v_inv_transstock where custid = " & custid & "")

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function

    Public Sub insert_invtrans_opening(ByVal custid As Integer, ByVal maxdate As Date, ByVal cc As Integer, ByVal stkid As Integer, _
                               ByVal stkname As String, ByVal u2 As Decimal, ByVal factor1 As Decimal, ByVal stkno As String, _
                               ByVal u1_nm As String, ByVal u2_nm As String, ByVal cost As Decimal, ByVal ccname As String, ByVal username As String)
        Dim c_cmd As New Cls_command


        Dim str As String
        str = "insert into cld_inv_Trans values (" & c_cmd.ExecuteScalar("select Max(ID) from cld_inv_Trans") & ", " & custid & ", DATE_ADD('" & maxdate.ToString("yyyy-MM-dd") & "', INTERVAL 1 DAY), 6, " & cc & ", " & _
                        " " & stkid & ", '" & stkname & "', 0, " & u2 & ", " & factor1 & ", " & u2 & ", 0," & cost & ", " & _
                        " '', 1, '" & u1_nm & "', '" & u2_nm & "', '" & username & "', DATE_ADD('" & maxdate.ToString("yyyy-MM-dd HH:mm:ss") & "', INTERVAL 1 DAY), Null, Null, '" & stkno & "', '" & ccname & "')"
        c_cmd.executeNonQuery(str)

    End Sub
End Class
