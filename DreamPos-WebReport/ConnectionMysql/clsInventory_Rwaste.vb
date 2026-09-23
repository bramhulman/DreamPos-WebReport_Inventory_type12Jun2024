Public Class clsInventory_Rwaste

    Public Function SelectDataset_longId(ByVal custid As Integer) As String
        Dim ds As New DataSet

        Dim strFilter As String = ""


        Dim a As New clsDbo("SELECT  * from cld_inv_Trans where MONTH(invDate) = " & Date.Now.Month & " and type =  2 and custid = " & custid & " order by linkid desc")

        ds = a.selectDataset

        If ds.Tables(0).Rows.Count > 0 Then
            strFilter = ds.Tables(0).Rows(0).Item("linkid")

        End If
        a.dispose()
        a = Nothing

        Return strFilter

    End Function
    'Bram 29 Aug 22 tambahan untuk closing waste
    Public Function SelectDataset_longId2(ByVal custid As Integer) As String
        Dim ds As New DataSet

        Dim strFilter As String = ""


        Dim a As New clsDbo("SELECT  * from cld_inv_Trans where MONTH(invDate) = " & Date.Now.Month & " and type =  14 and custid = " & custid & " order by linkid desc")

        ds = a.selectDataset

        If ds.Tables(0).Rows.Count > 0 Then
            strFilter = ds.Tables(0).Rows(0).Item("linkid")

        End If
        a.dispose()
        a = Nothing

        Return strFilter

    End Function


    Public Function SelectDataset_invRwaste(ByVal bydate As Date, ByVal custid As Integer, ByVal ModPriv As Integer, ByVal user As String) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""


        If ModPriv And usrModulepriv.moduleuser = usrModulepriv.moduleuser Then

            strFilter = "SELECT cld_inv_Trans.*, DATE_FORMAT(cld_inv_trans.invDate,'%d-%b-%Y') AS InvD, cld_inv_cc.Name AS ccname  FROM cld_inv_trans LEFT JOIN cld_inv_cc ON cld_inv_trans.cc = cld_inv_cc.ID and cld_inv_trans.Custid = cld_inv_cc.Custid " &
                        "where  cld_inv_Trans.type =  2 and cld_inv_trans.custid = " & custid & " and cld_inv_Trans.InvDate = '" & bydate.ToString("yyyy-MM-dd 00:00:00") & "' and  cld_inv_cc.olid IN (SELECT Store FROM cld_userxstore WHERE USER = '" & user & "')"

        Else

            strFilter = "SELECT cld_inv_Trans.*, DATE_FORMAT(cld_inv_trans.invDate,'%d-%b-%Y') AS InvD, cld_inv_cc.Name AS ccname  FROM cld_inv_trans LEFT JOIN cld_inv_cc ON cld_inv_trans.cc = cld_inv_cc.ID and cld_inv_trans.Custid = cld_inv_cc.Custid " &
                        "where  cld_inv_Trans.type =  2 and cld_inv_trans.custid = " & custid & " and cld_inv_Trans.InvDate = '" & bydate.ToString("yyyy-MM-dd 00:00:00") & "'"

        End If

        Dim a As New clsDbo(strFilter)

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function
    Public Function SelectDataset_clswastesrch(ByVal name As String, ByVal bydate As Date, ByVal custid As Integer, ByVal ModPriv As Integer, ByVal user As String) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""


        If ModPriv And usrModulepriv.moduleuser = usrModulepriv.moduleuser Then

            strFilter = "SELECT cld_inv_Trans.*, DATE_FORMAT(cld_inv_trans.invDate,'%d-%b-%Y') AS InvD, cld_inv_cc.Name AS ccname  FROM cld_inv_trans LEFT JOIN cld_inv_cc ON cld_inv_trans.cc = cld_inv_cc.ID and cld_inv_trans.Custid = cld_inv_cc.Custid " &
                        "where  cld_inv_Trans.type =  14 and cld_inv_trans.custid = " & custid & " and cld_inv_Trans.InvDate = '" & bydate.ToString("yyyy-MM-dd 00:00:00") & "' and (cld_inv_cc.name Like '%" & name & "%' or cld_inv_trans.Stk_Name Like '%" & name & "%') and  cld_inv_cc.olid IN (SELECT Store FROM cld_userxstore WHERE USER = '" & user & "')"

        Else

            strFilter = "SELECT cld_inv_Trans.*, DATE_FORMAT(cld_inv_trans.invDate,'%d-%b-%Y') AS InvD, cld_inv_cc.Name AS ccname  FROM cld_inv_trans LEFT JOIN cld_inv_cc ON cld_inv_trans.cc = cld_inv_cc.ID and cld_inv_trans.Custid = cld_inv_cc.Custid " &
                        "where  cld_inv_Trans.type =  14 and cld_inv_trans.custid = " & custid & " and cld_inv_Trans.InvDate = '" & bydate.ToString("yyyy-MM-dd 00:00:00") & "' and (cld_inv_cc.name Like '%" & name & "%' or cld_inv_trans.Stk_Name Like '%" & name & "%')"

        End If

        Dim a As New clsDbo(strFilter)

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function
    Public Function SelectDataset_clswasteEdit(ByVal Id As Integer, ByVal custid As Integer, ByVal ccid As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""



        strFilter = "SELECT cld_inv_Trans.*, DATE_FORMAT(cld_inv_trans.invDate,'%d-%b-%Y') AS InvD, cld_inv_cc.Name AS ccname  FROM cld_inv_trans LEFT JOIN cld_inv_cc ON cld_inv_trans.cc = cld_inv_cc.ID and cld_inv_trans.Custid = cld_inv_cc.Custid " &
                        "where  cld_inv_Trans.type =  14 and cld_inv_Trans.id = " & Id & " and cld_inv_Trans.cc = " & ccid & " AND  cld_inv_Trans.custid = " & custid & ""
        'and cld_inv_Trans.InvDate = '" & invdate.ToString("yyyy-MM-dd 00:00:00") & "' 




        Dim a As New clsDbo(strFilter)

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function
    Public Function SelectDataset_closingwasteSearch(ByVal name As String, ByVal custid As Integer, ByVal ModPriv As Integer, ByVal user As String, ByVal ccid As Integer, ByVal dDate As Date) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""
        Dim strFilter2 As String = ""
        If ccid <> 0 Then
            strFilter2 = "and cld_inv_transstockcls.ccid = " & ccid & ""
        End If

        If ModPriv And usrModulepriv.moduleuser = usrModulepriv.moduleuser Then

            strFilter = "SELECT trans.id,cld_inv_transstockcls.*,cld_inv_stk.unit2 FROM (SELECT * FROM cld_inv_trans WHERE cld_inv_Trans.invdate = '" & dDate.Date.ToString("yyyy-MM-dd") & "' AND TYPE = 14) trans 
                          LEFT JOIN cld_inv_stk ON trans.`stk_ID` = cld_inv_stk.id AND trans.`custId` = cld_inv_stk.`CustID`
                            LEFT JOIN cld_inv_transstockcls ON cld_inv_transstockcls.`stkID` = trans.stk_id AND cld_inv_transstockcls.`custId` = trans.`CustID` 
                            AND cld_inv_transstockcls.`ccid` = trans.`cc` AND cld_inv_transstockcls.`invdate` = trans.`invdate` " &
                        "where cld_inv_transstockcls.custid = " & custid & " " & strFilter2 & " and trans.invdate = '" & dDate.Date.ToString("yyyy-MM-dd") & "' and (ccname Like '%" & name & "%' or stkname Like '%" & name & "%') 
                        and cld_inv_cc.olid in (Select Store From cld_userxstore where user = '" & user & "') and bclose = 1 and closeqty =0 order by stkName"
        Else
            strFilter = "SELECT trans.id,cld_inv_transstockcls.*,cld_inv_stk.unit2 FROM (SELECT * FROM cld_inv_trans WHERE cld_inv_Trans.invdate = '" & dDate.Date.ToString("yyyy-MM-dd") & "' AND TYPE = 14) trans 
                          LEFT JOIN cld_inv_stk ON trans.`stk_ID` = cld_inv_stk.id AND trans.`custId` = cld_inv_stk.`CustID`
                            LEFT JOIN cld_inv_transstockcls ON cld_inv_transstockcls.`stkID` = trans.stk_id AND cld_inv_transstockcls.`custId` = trans.`CustID` 
                            AND cld_inv_transstockcls.`ccid` = trans.`cc` AND cld_inv_transstockcls.`invdate` = trans.`invdate` " &
                        "where cld_inv_transstockcls.custid = " & custid & " " & strFilter2 & " and trans.invdate = '" & dDate.Date.ToString("yyyy-MM-dd") & "' and (ccname Like '%" & name & "%' or stkname Like '%" & name & "%') 
                        and bclose = 1 and closeqty =0 order by stkName"

        End If

        Dim a As New clsDbo(strFilter)

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function
    Public Function SelectDataset_invRwasteSearch(ByVal name As String, ByVal bydate As Date, ByVal custid As Integer, ByVal ModPriv As Integer, ByVal user As String) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""

        'revisi 8 nov 2016, soalnya error di test pake tgl
        'Dim a As New clsDbo("SELECT cld_inv_Trans.*, DATE_FORMAT(cld_inv_trans.invDate,'%d-%b-%Y') AS InvD, cld_inv_cc.Name AS ccname  FROM cld_inv_trans LEFT JOIN cld_inv_cc ON cld_inv_trans.cc = cld_inv_cc.ID where cld_inv_Trans.type =  2 and cld_inv_trans.custid = " & custid & " and cld_inv_Trans.InvDate = '" & bydate.ToString("yyyy-MM-dd") & "' and (cld_inv_cc.name Like '%" & name & "%' or cld_inv_trans.linkid Like '%" & name & "%')")

        'revisi 27 feb 2016, perubahan di inv trans, jadi ada user yang hanya boleh liat storenya
        'Dim a As New clsDbo("SELECT cld_inv_Trans.*, DATE_FORMAT(cld_inv_trans.invDate,'%d-%b-%Y') AS InvD, cld_inv_cc.Name AS ccname  FROM cld_inv_trans LEFT JOIN cld_inv_cc ON cld_inv_trans.cc = cld_inv_cc.ID where cld_inv_Trans.type =  2 and cld_inv_trans.custid = " & custid & " and (cld_inv_cc.name Like '%" & name & "%' or cld_inv_trans.linkid Like '%" & name & "%')")

        If ModPriv And usrModulepriv.moduleuser = usrModulepriv.moduleuser Then


            strFilter = "SELECT cld_inv_Trans.*, DATE_FORMAT(cld_inv_trans.invDate,'%d-%b-%Y') AS InvD, cld_inv_cc.Name AS ccname  FROM cld_inv_trans LEFT JOIN cld_inv_cc ON cld_inv_trans.cc = cld_inv_cc.ID and cld_inv_trans.Custid = cld_inv_cc.Custid" &
                        " where cld_inv_Trans.type =  2 and cld_inv_trans.custid = " & custid & " and cld_inv_Trans.InvDate = '" & bydate.ToString("yyyy-MM-dd") & "' and (cld_inv_cc.name Like '%" & name & "%' or cld_inv_trans.linkid Like '%" & name & "%') and cld_inv_cc.olid IN (SELECT Store FROM cld_userxstore WHERE USER = '" & user & "')"

        Else

            strFilter = "SELECT cld_inv_Trans.*, DATE_FORMAT(cld_inv_trans.invDate,'%d-%b-%Y') AS InvD, cld_inv_cc.Name AS ccname  FROM cld_inv_trans LEFT JOIN cld_inv_cc ON cld_inv_trans.cc = cld_inv_cc.ID and cld_inv_trans.Custid = cld_inv_cc.Custid" &
                        " where cld_inv_Trans.type =  2 and cld_inv_trans.custid = " & custid & " and cld_inv_Trans.InvDate = '" & bydate.ToString("yyyy-MM-dd") & "' and (cld_inv_cc.name Like '%" & name & "%' or cld_inv_trans.linkid Like '%" & name & "%')"


        End If


        Dim a As New clsDbo(strFilter)

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function
    Public Function SelectDataset_invRwasteSearch2(ByVal name As String, ByVal ModPriv As Integer, ByVal user As String) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""
        Dim strFilter2 As String = ""

        If ModPriv And usrModulepriv.moduleuser = usrModulepriv.moduleuser Then

            strFilter = "(SELECT 
                            cld_inv_transstockcls.invdate,
                            DATE_FORMAT(cld_inv_transstockcls.invdate, '%d-%b-%Y')          AS inv_date,
                            cld_inv_transstockcls.custid,
                            stkid,
                            Stkno,
                            stkname,
                            ccid,
                            ccname,
                            opencost,
                            openqty,
                            cld_inv_transstockcls.cost,
                            ttlqty,
                            rwasteqty,
                            u1,
                            unit1,
                            cld_inv_stk.factor1,
                            u2,
                            unit2,
                            DATE_FORMAT(trans.createddate, '%d-%b-%Y %k:%i:%s')             AS createddate,
                            trans.type                                                      AS trtype

                        FROM cld_inv_transstockcls
                        INNER JOIN cld_inv_cc 
                            ON cld_inv_transstockcls.ccid   = cld_inv_cc.ID 
                            AND cld_inv_transstockcls.custid = cld_inv_cc.custid

                        INNER JOIN cld_inv_stk 
                            ON cld_inv_transstockcls.stkID  = cld_inv_stk.id 
                            AND cld_inv_transstockcls.custId = cld_inv_stk.CustID

                        INNER JOIN (
                            SELECT * 
                            FROM cld_inv_trans 
                            WHERE invdate = '" & Now.Date.ToString("yyyy-MM-dd") & "' 
                              AND TYPE IN (2)                  
                        ) trans
                            ON cld_inv_transstockcls.stkID  = trans.stk_id 
                            AND cld_inv_transstockcls.custId = trans.CustID 
                            AND cld_inv_transstockcls.ccid   = trans.cc

                        WHERE cld_inv_transstockcls.invdate = '" & Now.Date.ToString("yyyy-MM-dd") & "'
                          AND (ccname LIKE '%" & name & "%' OR stkname LIKE '%" & name & "%')
                          AND rwasteqty > 0 
                          And cld_inv_cc.olid in (Select Store From cld_userxstore where user = '" & user & "'))
                          
                        UNION ALL

                        (SELECT 
                            cld_inv_transstockcls.invdate,
                            DATE_FORMAT(cld_inv_transstockcls.invdate, '%d-%b-%Y')          AS inv_date,
                            cld_inv_transstockcls.custid,
                            stkid,
                            Stkno,
                            stkname,
                            ccid,
                            ccname,
                            opencost,
                            openqty,
                            cld_inv_transstockcls.cost,
                            ttlqty,
                            rwasteqty,
                            0                                                               AS u1,
                            unit1,
                            cld_inv_stk.factor1,
                            0                                                               AS u2,
                            unit2,
                            DATE_FORMAT(cld_inv_transstockcls.invdate, '%d-%b-%Y %k:%i:%s') AS createddate,
                            0                                                               AS trtype
                        FROM cld_inv_transstockcls
                        INNER JOIN cld_inv_cc 
                            ON cld_inv_transstockcls.ccid   = cld_inv_cc.ID 
                            AND cld_inv_transstockcls.custid = cld_inv_cc.custid

                        INNER JOIN cld_inv_stk 
                            ON cld_inv_transstockcls.stkID  = cld_inv_stk.id 
                            AND cld_inv_transstockcls.custId = cld_inv_stk.CustID

                        WHERE cld_inv_transstockcls.invdate = '" & Now.Date.ToString("yyyy-MM-dd") & "'
                          AND (ccname LIKE '%" & name & "%' OR stkname LIKE '%" & name & "%') AND (rwasteqty = 0)
                        And cld_inv_cc.olid in (Select Store From cld_userxstore where user = '" & user & "'))"
        Else
            strFilter = "(SELECT 
                            cld_inv_transstockcls.invdate,
                            DATE_FORMAT(cld_inv_transstockcls.invdate, '%d-%b-%Y')          AS inv_date,
                            cld_inv_transstockcls.custid,
                            stkid,
                            Stkno,
                            stkname,
                            ccid,
                            ccname,
                            opencost,
                            openqty,
                            cld_inv_transstockcls.cost,
                            ttlqty,
                            rwasteqty,
                            u1,
                            unit1,
                            cld_inv_stk.factor1,
                            u2,
                            unit2,
                            DATE_FORMAT(trans.createddate, '%d-%b-%Y %k:%i:%s')             AS createddate,
                            trans.type                                                      AS trtype

                        FROM cld_inv_transstockcls
                        INNER JOIN cld_inv_cc 
                            ON cld_inv_transstockcls.ccid   = cld_inv_cc.ID 
                            AND cld_inv_transstockcls.custid = cld_inv_cc.custid

                        INNER JOIN cld_inv_stk 
                            ON cld_inv_transstockcls.stkID  = cld_inv_stk.id 
                            AND cld_inv_transstockcls.custId = cld_inv_stk.CustID

                        INNER JOIN (
                            SELECT * 
                            FROM cld_inv_trans 
                            WHERE invdate = '" & Now.Date.ToString("yyyy-MM-dd") & "' 
                              AND TYPE IN (2)                  
                        ) trans
                            ON cld_inv_transstockcls.stkID  = trans.stk_id 
                            AND cld_inv_transstockcls.custId = trans.CustID 
                            AND cld_inv_transstockcls.ccid   = trans.cc

                        WHERE cld_inv_transstockcls.invdate = '" & Now.Date.ToString("yyyy-MM-dd") & "'
                          AND (ccname LIKE '%" & name & "%' OR stkname LIKE '%" & name & "%')
                          AND rwasteqty > 0) 
                          
                        UNION ALL

                        (SELECT 
                            cld_inv_transstockcls.invdate,
                            DATE_FORMAT(cld_inv_transstockcls.invdate, '%d-%b-%Y')          AS inv_date,
                            cld_inv_transstockcls.custid,
                            stkid,
                            Stkno,
                            stkname,
                            ccid,
                            ccname,
                            opencost,
                            openqty,
                            cld_inv_transstockcls.cost,
                            ttlqty,
                            rwasteqty,
                            0                                                               AS u1,
                            unit1,
                            cld_inv_stk.factor1,
                            0                                                               AS u2,
                            unit2,
                            DATE_FORMAT(cld_inv_transstockcls.invdate, '%d-%b-%Y %k:%i:%s') AS createddate,
                            0                                                               AS trtype
                        FROM cld_inv_transstockcls
                        INNER JOIN cld_inv_cc 
                            ON cld_inv_transstockcls.ccid   = cld_inv_cc.ID 
                            AND cld_inv_transstockcls.custid = cld_inv_cc.custid

                        INNER JOIN cld_inv_stk 
                            ON cld_inv_transstockcls.stkID  = cld_inv_stk.id 
                            AND cld_inv_transstockcls.custId = cld_inv_stk.CustID

                        WHERE cld_inv_transstockcls.invdate = '" & Now.Date.ToString("yyyy-MM-dd") & "'
                          AND (ccname LIKE '%" & name & "%' OR stkname LIKE '%" & name & "%') AND rwasteqty = 0)"

        End If

        Dim a As New clsDbo(strFilter)

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function
    Public Function SelectDataset_InRwasteEdit(ByVal Id As Integer, ByVal custid As Integer, ByVal CC As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""

        'perubahan 11 oct 2017
        Dim a As New clsDbo("SELECT cld_inv_trans.*, IFNULL((SELECT ttlqty FROM cld_inv_transstock WHERE stkid = cld_inv_trans.stk_id AND ccid = cld_inv_trans.Cc AND custid = cld_inv_trans.CustID), 0) AS stock " &
                            ", IFNULL((SELECT cost FROM cld_inv_stkcost WHERE stkid = cld_inv_trans.stk_id AND cc = cld_inv_trans.Cc AND custid = cld_inv_trans.CustID ), 0) AS coststk " &
                            ", IFNULL((SELECT Notes FROM cld_inv_transnotes WHERE transid = cld_inv_trans.id AND cc = cld_inv_trans.Cc), '') AS Notes " &
                            "FROM cld_inv_trans WHERE type = 2 and id = " & Id & " and cc = " & CC & " AND  custid = " & custid & "")

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function
    Public Function SelectDataset_InRwasteEdit_bydate(ByVal Id As Integer, ByVal custid As Integer, ByVal invdate As Date) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""

        'perubahan 11 oct 2017
        Dim a As New clsDbo("SELECT cld_inv_trans.*, IFNULL((SELECT ttlqty FROM cld_inv_transstockcls WHERE stkid = cld_inv_trans.stk_id AND ccid = cld_inv_trans.Cc AND custid = cld_inv_trans.CustID and invdate = cld_inv_Trans.InvDate), 0) AS stock " &
                            ", IFNULL((SELECT cost FROM cld_inv_transstockcls WHERE stkid = cld_inv_trans.stk_id AND ccid = cld_inv_trans.Cc AND custid = cld_inv_trans.CustID and invdate = cld_inv_Trans.InvDate), 0) AS coststk " &
         ", IFNULL((SELECT Notes FROM cld_inv_transnotes WHERE transid = cld_inv_trans.id AND cc = cld_inv_trans.Cc), '') AS Notes " &
        "FROM cld_inv_trans WHERE type = 2 and id = " & Id & " and cld_inv_Trans.InvDate = '" & invdate.ToString("yyyy-MM-dd") & "' AND  custid = " & custid & "")

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function
End Class
