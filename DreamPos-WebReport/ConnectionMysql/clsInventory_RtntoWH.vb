Public Class clsInventory_RtntoWH


    Public Function SelectDataTable_invtrans_RtntoWH(ByVal linkid As String, ByVal custid As Integer) As DataTable
        Dim ds As New DataTable
        Dim strFilter As String = ""


        Dim a As New clsDbo("SELECT A.*, IFNULL(B.ttlQty, 0) AS Stock,c.ccfrom  FROM cld_inv_trans A INNER JOIN cld_invlk_trf c ON a.CustID = c.CustID AND a.Linkid = c.id " &
                            "LEFT JOIN cld_inv_transstockcls B ON B.stkId = A.Stk_Id  AND B.ccid = c.ccFrom AND b.custId = c.CustID " &
                            "WHERE A.linkid = '" & linkid & "' and A.type = 13 and A.custid = " & custid & " and B.invdate = '" & Date.Now.ToString("yyyy-MM-dd 00:00:00") & "'")

        ds = a.selectDatatable

        a.dispose()
        a = Nothing

        Return ds

    End Function


    Public Function SelectDataset_longId(ByVal custid As Integer) As String
        Dim ds As New DataSet

        Dim strFilter As String = ""


        Dim a As New clsDbo("SELECT  * from cld_invlk_rtwh where MONTH(createddate) = " & Date.Now.Month & " and custid = " & custid & " order by id desc")

        ds = a.selectDataset

        If ds.Tables(0).Rows.Count > 0 Then
            strFilter = ds.Tables(0).Rows(0).Item("Id")
        End If

        a.dispose()
        a = Nothing

        Return strFilter

    End Function

    Public Function SelectDataset_invRtntoWH(ByVal bydate As Date, ByVal bydateto As Date, ByVal custid As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""

        'revisi 8 nov 2016, soalnya error di test pake tgl
        ' Dim a As New clsDbo("SELECT cld_invlk_trf.*, DATE_FORMAT(cld_invlk_trf.CreatedDate,'%d-%b-%Y') AS CreateD, cld_inv_cc.Name AS ccname  FROM cld_invlk_trf LEFT JOIN cld_inv_cc ON cld_invlk_trf.ccFrom = cld_inv_cc.ID where cld_invlk_trf.custid = " & custid & " and  cld_invlk_trf.CreatedDate = '" & bydate.ToString("yyyy-MM-dd") & "'")
        Dim a As New clsDbo("SELECT cld_invlk_trf.*, DATE_FORMAT(cld_invlk_trf.CreatedDate,'%d-%b-%Y') AS CreateD, cld_inv_cc.Name AS ccname, SUM(cld_inv_trans.`ttlStock` * cld_inv_trans.`Cost`) ttlrtn  FROM cld_invlk_trf LEFT JOIN cld_inv_cc ON cld_invlk_trf.ccFrom = cld_inv_cc.ID
                            LEFT JOIN cld_inv_trans ON cld_inv_trans.Linkid = cld_invlk_trf.ID and cld_inv_trans.custid = cld_invlk_trf.custid and cld_invlk_trf.CreatedDate = cld_inv_trans.InvDate
                            where cld_inv_trans.cc in (select id from cld_inv_cc where olid = 0) and cld_invlk_trf.custid = " & custid & " 
                            and  cld_invlk_trf.CreatedDate >= '" & bydate.ToString("yyyy-MM-dd") & "' and  cld_invlk_trf.CreatedDate <= '" & bydateto.ToString("yyyy-MM-dd") & "'
                            GROUP BY cld_inv_trans.Linkid, cld_inv_cc.Name")

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function


    Public Function SelectDataset_invRtntoWHSearch(ByVal name As String, ByVal bydate As Date, ByVal bydateto As Date, ByVal custid As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""

        'revisi 8 nov 2016, soalnya error di test pake tgl
        'Dim a As New clsDbo("SELECT cld_invlk_trf.*, DATE_FORMAT(cld_invlk_trf.CreatedDate,'%d-%b-%Y') AS CreateD, cld_inv_cc.Name AS ccname  FROM cld_invlk_trf LEFT JOIN cld_inv_cc ON cld_invlk_trf.ccFrom = cld_inv_cc.ID where  cld_invlk_trf.custid = " & custid & " and cld_invlk_trf.CreatedDate = '" & bydate.ToString("yyyy-MM-dd") & "' and (cld_inv_cc.name Like '%" & name & "%' or cld_invlk_trf.id Like '%" & name & "%')")
        'Dim a As New clsDbo("SELECT cld_invlk_rtwh.*, DATE_FORMAT(cld_invlk_trf.CreatedDate,'%d-%b-%Y') AS CreateD, cld_inv_cc.Name AS ccname  FROM cld_invlk_rtwh LEFT JOIN cld_inv_cc ON cld_invlk_rtwh.ccFrom = cld_inv_cc.ID where  cld_invlk_rtwh.custid = " & custid & " and cld_invlk_rtwh.CreatedDate = '" & bydate.ToString("yyyy-MM-dd") & "' and (cld_inv_cc.name Like '%" & name & "%' or cld_invlk_rtwh.id Like '%" & name & "%')")
        Dim a As New clsDbo("SELECT cld_invlk_trf.*, DATE_FORMAT(cld_invlk_trf.CreatedDate,'%d-%b-%Y') AS CreateD, cld_inv_cc.Name AS ccname, SUM(cld_inv_trans.`ttlStock` * cld_inv_trans.`Cost`) ttlrtn FROM cld_invlk_trf LEFT JOIN cld_inv_cc ON cld_invlk_trf.ccFrom = cld_inv_cc.ID
                            LEFT JOIN cld_inv_trans ON cld_inv_trans.Linkid = cld_invlk_trf.ID and cld_inv_trans.custid = cld_invlk_trf.custid and cld_invlk_trf.CreatedDate = cld_inv_trans.InvDate
                            where cld_inv_trans.cc in (select id from cld_inv_cc where olid = 0) and cld_invlk_trf.custid = " & custid & "
                            and  cld_invlk_trf.CreatedDate >= '" & bydate.ToString("yyyy-MM-dd") & "' and  cld_invlk_trf.CreatedDate <= '" & bydateto.ToString("yyyy-MM-dd") & "'
                            and (cld_inv_cc.name Like '%" & name & "%' or cld_invlk_trf.id Like '%" & name & "%')
                            GROUP BY cld_inv_trans.Linkid")
        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function

    Public Function SelectDataset_invRtntoWHExp(ByVal name As String, ByVal bydate As Date, ByVal bydateto As Date, ByVal custid As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""

        Dim a As New clsDbo("SELECT cld_invlk_trf.id,cld_inv_cc.Name AS ccname,cld_invlk_trf.`CreatedBy`,cld_invlk_trf.`CreatedDate`,cld_invlk_trf.`ModifBy`,cld_invlk_trf.`ModifDate`, SUM(cld_inv_trans.`ttlStock` * cld_inv_trans.`Cost`) ttlrtn FROM cld_invlk_trf LEFT JOIN cld_inv_cc ON cld_invlk_trf.ccFrom = cld_inv_cc.ID
                            LEFT JOIN cld_inv_trans ON cld_inv_trans.Linkid = cld_invlk_trf.ID and cld_inv_trans.custid = cld_invlk_trf.custid and cld_invlk_trf.CreatedDate = cld_inv_trans.InvDate
                            where cld_inv_trans.cc in (select id from cld_inv_cc where olid = 0) and cld_invlk_trf.custid = " & custid & "
                            and  cld_invlk_trf.CreatedDate >= '" & bydate.ToString("yyyy-MM-dd") & "' and  cld_invlk_trf.CreatedDate <= '" & bydateto.ToString("yyyy-MM-dd") & "'
                            and (cld_inv_cc.name Like '%" & name & "%' or cld_invlk_trf.id Like '%" & name & "%')
                            GROUP BY cld_inv_trans.Linkid
                            UNION ALL
                            Select 'Total', '', '', '', '', '', SUM(cld_inv_trans.`ttlStock` * cld_inv_trans.`Cost`) ttlrtn FROM cld_invlk_trf LEFT JOIN cld_inv_cc ON cld_invlk_trf.ccFrom = cld_inv_cc.ID
                            Left Join cld_inv_trans ON cld_inv_trans.Linkid = cld_invlk_trf.ID And cld_inv_trans.custid = cld_invlk_trf.custid And cld_invlk_trf.CreatedDate = cld_inv_trans.InvDate
                            where cld_inv_trans.cc in (select id from cld_inv_cc where olid = 0) And cld_invlk_trf.custid = " & custid & "
                            And  cld_invlk_trf.CreatedDate >= '" & bydate.ToString("yyyy-MM-dd") & "' and  cld_invlk_trf.CreatedDate <= '" & bydateto.ToString("yyyy-MM-dd") & "'
                            And (cld_inv_cc.name Like '%" & name & "%' or cld_invlk_trf.id Like '%" & name & "%')")
        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function
    Public Function SelectDataset_invRtntoWHDtl(ByVal name As String, ByVal bydate As Date, ByVal bydateto As Date, ByVal custid As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""

        Dim a As New clsDbo("SELECT cld_invlk_trf.id,cld_inv_cc.Name AS ccname,cld_invlk_trf.`CreatedBy`,cld_invlk_trf.`CreatedDate`,
                            cld_inv_trans.`InvDate`,cld_inv_trans.`Stk_no`,cld_inv_trans.`Stk_Name`,cld_inv_trans.`ttlStock`, cld_inv_trans.`U2_nm`,cld_inv_trans.`Cost`,(cld_inv_trans.`ttlStock` * cld_inv_trans.`Cost`) Total
                            FROM cld_invlk_trf LEFT JOIN cld_inv_cc ON cld_invlk_trf.ccFrom = cld_inv_cc.ID
                            LEFT JOIN cld_inv_trans ON cld_inv_trans.Linkid = cld_invlk_trf.ID AND cld_inv_trans.custid = cld_invlk_trf.custid AND cld_invlk_trf.CreatedDate = cld_inv_trans.InvDate
                            where cld_inv_trans.cc in (select id from cld_inv_cc where olid = 0) and cld_invlk_trf.custid = " & custid & "
                            and  cld_invlk_trf.CreatedDate >= '" & bydate.ToString("yyyy-MM-dd") & "' and  cld_invlk_trf.CreatedDate <= '" & bydateto.ToString("yyyy-MM-dd") & "'
                            and (cld_inv_cc.name Like '%" & name & "%' or cld_invlk_trf.id Like '%" & name & "%')
                            UNION ALL
                            SELECT 'Total','','','',
                            '','','','', '','',sum(cld_inv_trans.`ttlStock` * cld_inv_trans.`Cost`)
                            FROM cld_invlk_trf LEFT JOIN cld_inv_cc ON cld_invlk_trf.ccFrom = cld_inv_cc.ID
                            LEFT JOIN cld_inv_trans ON cld_inv_trans.Linkid = cld_invlk_trf.ID AND cld_inv_trans.custid = cld_invlk_trf.custid AND cld_invlk_trf.CreatedDate = cld_inv_trans.InvDate
                            where cld_inv_trans.cc in (select id from cld_inv_cc where olid = 0) and cld_invlk_trf.custid = " & custid & "
                            and  cld_invlk_trf.CreatedDate >= '" & bydate.ToString("yyyy-MM-dd") & "' and  cld_invlk_trf.CreatedDate <= '" & bydateto.ToString("yyyy-MM-dd") & "'
                            and (cld_inv_cc.name Like '%" & name & "%' or cld_invlk_trf.id Like '%" & name & "%')")
        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function
End Class
