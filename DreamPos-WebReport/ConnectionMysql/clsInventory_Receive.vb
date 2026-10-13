Public Class clsInventory_Receive

    Public Function SelectDataset_longId(ByVal custid As Integer) As String
        Dim ds As New DataSet

        Dim strFilter As String = ""

        Try

            Dim a As New clsDbo("SELECT  * from cld_invlk_rcv where MONTH(createddate) = " & Date.Now.Month & " and custid = " & custid & "  and Tipe = 1 order by id desc")

            ds = a.selectDataset

            If ds.Tables(0).Rows.Count > 0 Then
                strFilter = ds.Tables(0).Rows(0).Item("Id")
            End If

            a.dispose()
            a = Nothing

        Catch ex As Exception

            errLog(Err.Description, "clsInventory_Receive:SelectDataset_longId")
            Err.Clear()
        End Try


        ds.Dispose()

        Return strFilter

    End Function
    'tanpa ACC
    Public Function SelectDataset_invRcv1(ByVal fromdate As Date, ByVal todate As Date, ByVal custid As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""
        If modpriv <> 0 Then

            strFilter = "SELECT cld_invlk_Rcv.*, case cld_invlk_Rcv.flag when 1 then 'Order' when 2 then 'Finish' END as Status,  DATE_FORMAT(cld_invlk_rcv.CreatedDate,'%d-%b-%Y') AS CreateD, 
                             DATE_FORMAT((case cld_invlk_Rcv.flag when 1 then InvDate WHEN 2 THEN ifnull(InvDate,createddate) END),'%d-%b-%Y') AS statdate,
                            cld_inv_cc.Name AS ccname, cld_inv_supplier.Name AS Supname , ifnull(cld_inv_supplier.email,'') AS Email FROM cld_invlk_Rcv 
                            INNER JOIN cld_inv_cc ON cld_invlk_Rcv.cc = cld_inv_cc.ID 
                            INNER JOIN cld_Inv_supplier ON cld_Invlk_Rcv.supid = cld_inv_supplier.id  
                            where cld_invlk_rcv.InvDate >= '" & fromdate.ToString("yyyy-MM-dd") & "' and cld_invlk_rcv.InvDate <= '" & todate.ToString("yyyy-MM-dd") & "' and cld_invlk_rcv.custid = " & custid & " and Tipe = 1 
                             AND cld_Invlk_Rcv.supid IN
			                (SELECT cld_inv_Supplier.id FROM cld_inv_Supplier 
			                INNER JOIN cld_cust ON cld_cust.id = cld_inv_Supplier.custid
			                INNER JOIN cld_ccxsupplier ON cld_inv_Supplier.id=cld_ccxsupplier.`supplier`
			                INNER JOIN cld_inv_cc ON cld_inv_cc.id = cld_ccxsupplier.`cc`
			                INNER JOIN cld_outlet ON cld_inv_cc.`Olid` = cld_outlet.`ID`
			                INNER JOIN cld_userxstore ON cld_outlet.id=cld_userxstore.`Store`
			                WHERE cld_inv_Supplier.custid = " & custid & " AND cld_userxstore.`User` = '" & usr & "')
                            and olid IN (SELECT Store FROM cld_userxstore WHERE USER = '" & usr & "') 
                             ORDER BY InvDate desc, id desc"

        Else

            strFilter = "SELECT cld_invlk_Rcv.*, case cld_invlk_Rcv.flag when 1 then 'Order' when 2 then 'Finish' END as Status,  DATE_FORMAT(cld_invlk_rcv.CreatedDate,'%d-%b-%Y') AS CreateD,
                            DATE_FORMAT((case cld_invlk_Rcv.flag when 1 then InvDate WHEN 2 THEN ifnull(InvDate,createddate) END),'%d-%b-%Y') AS statdate,
                            cld_inv_cc.Name AS ccname, cld_inv_supplier.Name AS Supname , ifnull(cld_inv_supplier.email,'') AS Email FROM cld_invlk_Rcv LEFT JOIN cld_inv_cc ON cld_invlk_Rcv.cc = cld_inv_cc.ID LEFT JOIN cld_Inv_supplier ON cld_Invlk_Rcv.supid = cld_inv_supplier.id  where cld_invlk_rcv.InvDate >= '" & fromdate.ToString("yyyy-MM-dd") & "' and cld_invlk_rcv.InvDate <= '" & todate.ToString("yyyy-MM-dd") & "' and cld_invlk_rcv.custid = " & custid & " and Tipe = 1 ORDER BY InvDate desc, id desc"

        End If


        Dim a As New clsDbo(strFilter)

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function
    Public Function SelectDataset_invRcv(ByVal fromdate As Date, ByVal todate As Date, ByVal custid As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""
        If modpriv <> 0 Then
            'If itipe = 1 Then
            'strFilter = "SELECT cld_invlk_Rcv.*, case cld_invlk_Rcv.flag when 1 then 'Order' when 2 then 'Finish' END as Status,  DATE_FORMAT(cld_invlk_rcv.CreatedDate,'%d-%b-%Y') AS CreateD, 
            '            DATE_FORMAT((case cld_invlk_Rcv.flag when 1 then InvDate WHEN 2 THEN ifnull(InvDate,createddate) END),'%d-%b-%Y') AS statdate,
            '            cld_inv_cc.Name AS ccname, cld_inv_supplier.Name AS Supname , ifnull(cld_inv_supplier.email,'') AS Email FROM cld_invlk_Rcv INNER JOIN cld_inv_cc ON cld_invlk_Rcv.cc = cld_inv_cc.ID 
            '            INNER JOIN cld_Inv_supplier ON cld_Invlk_Rcv.supid = cld_inv_supplier.id 
            '            INNER Join cld_outlet ON cld_inv_cc.Olid = cld_outlet.id and cld_inv_cc.custid = cld_outlet.custid
            '            INNER JOIN cld_ccxsupplier ON cld_inv_Supplier.id=cld_ccxsupplier.`supplier`
            '            INNER JOIN cld_userxstore ON cld_outlet.id=cld_userxstore.`Store`
            '            where cld_invlk_rcv.InvDate = '" & bydate.ToString("yyyy-MM-dd") & "' and cld_invlk_rcv.custid = " & custid & " and Tipe = 1 
            '            and olid IN (SELECT Store FROM cld_userxstore WHERE USER = '" & usr & "')
            '             GROUP BY cld_invlk_Rcv.id ORDER BY InvDate desc, Tipe"
            'strFilter = "SELECT cld_invlk_Rcv.*, case cld_invlk_Rcv.flag when 1 then 'Order' when 2 then 'Finish' END as Status,  DATE_FORMAT(cld_invlk_rcv.CreatedDate,'%d-%b-%Y') AS CreateD, 
            '            DATE_FORMAT((case cld_invlk_Rcv.flag when 1 then InvDate WHEN 2 THEN ifnull(InvDate,createddate) END),'%d-%b-%Y') AS statdate,
            '            cld_inv_cc.Name AS ccname, cld_inv_supplier.Name AS Supname , ifnull(cld_inv_supplier.email,'') AS Email FROM cld_invlk_Rcv 
            '            INNER JOIN cld_inv_cc ON cld_invlk_Rcv.cc = cld_inv_cc.ID 
            '            INNER JOIN cld_Inv_supplier ON cld_Invlk_Rcv.supid = cld_inv_supplier.id  
            '            where cld_invlk_rcv.InvDate = '" & bydate.ToString("yyyy-MM-dd") & "' and cld_invlk_rcv.custid = " & custid & " and Tipe = 1 
            '            AND cld_Invlk_Rcv.supid IN
            '   (SELECT cld_inv_Supplier.id FROM cld_inv_Supplier 
            '   INNER JOIN cld_cust ON cld_cust.id = cld_inv_Supplier.custid
            '   INNER JOIN cld_ccxsupplier ON cld_inv_Supplier.id=cld_ccxsupplier.`supplier`
            '   INNER JOIN cld_inv_cc ON cld_inv_cc.id = cld_ccxsupplier.`cc`
            '   INNER JOIN cld_outlet ON cld_inv_cc.`Olid` = cld_outlet.`ID`
            '   INNER JOIN cld_userxstore ON cld_outlet.id=cld_userxstore.`Store`
            '   WHERE cld_inv_Supplier.custid = " & custid & " AND cld_userxstore.`User` = '" & usr & "')
            '            and olid IN (SELECT Store FROM cld_userxstore WHERE USER = '" & usr & "')
            '             GROUP BY cld_invlk_Rcv.id ORDER BY InvDate desc, Tipe"
            'ElseIf itipe = 2 Then
            'strFilter = "SELECT cld_invlk_Rcv.*, case cld_invlk_Rcv.flag when 1 then 'Order' when 2 then 'Finish' END as Status,  DATE_FORMAT(cld_invlk_rcv.CreatedDate,'%d-%b-%Y') AS CreateD, 
            '             DATE_FORMAT((case cld_invlk_Rcv.flag when 1 then InvDate WHEN 2 THEN ifnull(InvDate,createddate) END),'%d-%b-%Y') AS statdate,
            '            cld_inv_cc.Name AS ccname, cld_inv_supplier.Name AS Supname , ifnull(cld_inv_supplier.email,'') AS Email FROM cld_invlk_Rcv INNER JOIN cld_inv_cc ON cld_invlk_Rcv.cc = cld_inv_cc.ID 
            '            INNER JOIN cld_Inv_supplier ON cld_Invlk_Rcv.supid = cld_inv_supplier.id  
            '            INNER Join cld_outlet ON cld_inv_cc.Olid = cld_outlet.id and cld_inv_cc.custid = cld_outlet.custid
            '            INNER JOIN cld_ccxsupplier ON cld_inv_Supplier.id=cld_ccxsupplier.`supplier`
            '            INNER JOIN cld_userxstore ON cld_outlet.id=cld_userxstore.`Store`
            '            where cld_invlk_rcv.custid = " & custid & " and Tipe = 1 
            '            and olid IN (SELECT Store FROM cld_userxstore WHERE USER = '" & usr & "') 
            '             GROUP BY cld_invlk_Rcv.id ORDER BY InvDate desc, Tipe"
            strFilter = "SELECT cld_invlk_Rcv.*, case cld_invlk_Rcv.flag when 1 then 'Order' when 2 then 'Finish' END as Status, case cld_invlk_Rcv.acc when 1 then 'acc' when 0 then 'not_acc' END as acc2,  DATE_FORMAT(cld_invlk_rcv.CreatedDate,'%d-%b-%Y') AS CreateD, 
                             DATE_FORMAT((case cld_invlk_Rcv.flag when 1 then InvDate WHEN 2 THEN ifnull(InvDate,createddate) END),'%d-%b-%Y') AS statdate,
                            cld_inv_cc.Name AS ccname, cld_inv_supplier.Name AS Supname , ifnull(cld_inv_supplier.email,'') AS Email FROM cld_invlk_Rcv 
                            INNER JOIN cld_inv_cc ON cld_invlk_Rcv.cc = cld_inv_cc.ID 
                            INNER JOIN cld_Inv_supplier ON cld_Invlk_Rcv.supid = cld_inv_supplier.id  
                            where cld_invlk_rcv.InvDate >= '" & fromdate.ToString("yyyy-MM-dd") & "' and cld_invlk_rcv.InvDate <= '" & todate.ToString("yyyy-MM-dd") & "' and cld_invlk_rcv.custid = " & custid & " and Tipe = 1 
                             AND cld_Invlk_Rcv.supid IN
			                (SELECT cld_inv_Supplier.id FROM cld_inv_Supplier 
			                INNER JOIN cld_cust ON cld_cust.id = cld_inv_Supplier.custid
			                INNER JOIN cld_ccxsupplier ON cld_inv_Supplier.id=cld_ccxsupplier.`supplier`
			                            WHERE cld_inv_Supplier.custid = " & custid & ")
                            and olid IN (SELECT Store FROM cld_userxstore WHERE USER = '" & usr & "') 
                             ORDER BY InvDate desc, id desc"


            'INNER JOIN cld_inv_cc ON cld_inv_cc.id = cld_ccxsupplier.`cc`
            '     INNER JOIN cld_outlet ON cld_inv_cc.`Olid` = cld_outlet.`ID`
            '     INNER JOIN cld_userxstore ON cld_outlet.id=cld_userxstore.`Store`
            '   AND cld_userxstore.`User` = '" & usr & "'
            'End If
        Else
            'If itipe = 1 Then
            '    strFilter = "SELECT cld_invlk_Rcv.*, case cld_invlk_Rcv.flag when 1 then 'Order' when 2 then 'Finish' END as Status,  DATE_FORMAT(cld_invlk_rcv.CreatedDate,'%d-%b-%Y') AS CreateD, 
            '                DATE_FORMAT((case cld_invlk_Rcv.flag when 1 then InvDate WHEN 2 THEN ifnull(InvDate,createddate) END),'%d-%b-%Y') AS statdate,
            '                cld_inv_cc.Name AS ccname, cld_inv_supplier.Name AS Supname , ifnull(cld_inv_supplier.email,'') AS Email FROM cld_invlk_Rcv LEFT JOIN cld_inv_cc ON cld_invlk_Rcv.cc = cld_inv_cc.ID LEFT JOIN cld_Inv_supplier ON cld_Invlk_Rcv.supid = cld_inv_supplier.id  where cld_invlk_rcv.InvDate = '" & bydate.ToString("yyyy-MM-dd") & "' and cld_invlk_rcv.custid = " & custid & " and Tipe = 1 ORDER BY InvDate desc, Tipe"
            'ElseIf itipe = 2 Then
            strFilter = "SELECT cld_invlk_Rcv.*, case cld_invlk_Rcv.flag when 1 then 'Order' when 2 then 'Finish' END as Status, case cld_invlk_Rcv.acc when 1 then 'acc' when 0 then 'not_acc' END as acc2,  DATE_FORMAT(cld_invlk_rcv.CreatedDate,'%d-%b-%Y') AS CreateD,
                            DATE_FORMAT((case cld_invlk_Rcv.flag when 1 then InvDate WHEN 2 THEN ifnull(InvDate,createddate) END),'%d-%b-%Y') AS statdate,
                            cld_inv_cc.Name AS ccname, cld_inv_supplier.Name AS Supname , ifnull(cld_inv_supplier.email,'') AS Email FROM cld_invlk_Rcv LEFT JOIN cld_inv_cc ON cld_invlk_Rcv.cc = cld_inv_cc.ID LEFT JOIN cld_Inv_supplier ON cld_Invlk_Rcv.supid = cld_inv_supplier.id  where cld_invlk_rcv.InvDate >= '" & fromdate.ToString("yyyy-MM-dd") & "' and cld_invlk_rcv.InvDate <= '" & todate.ToString("yyyy-MM-dd") & "' and cld_invlk_rcv.custid = " & custid & " and Tipe = 1 ORDER BY InvDate desc, id desc"
            'End If
        End If


        Dim a As New clsDbo(strFilter)

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function
    Public Function SelectDataset_invRcvSearch(ByVal name As String, ByVal fromdate As Date, ByVal todate As Date, ByVal custid As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""

        'revisi 8 nov 2016, soalnya error di test pake tgl
        'Dim a As New clsDbo("SELECT cld_invlk_rcv.*, case cld_invlk_Rcv.flag when 1 then 'Pending' when 2 then 'Finish' END as Status, DATE_FORMAT(cld_invlk_rcv.CreatedDate,'%d-%b-%Y') AS CreateD, cld_inv_cc.Name AS ccname, cld_inv_supplier.Name AS Supname   FROM cld_invlk_Rcv LEFT JOIN cld_inv_cc ON cld_invlk_Rcv.cc = cld_inv_cc.ID LEFT JOIN cld_Inv_supplier ON cld_Invlk_Rcv.supid = cld_inv_supplier.id  where  (cld_inv_supplier.name Like '%" & name & "%' or cld_invlk_rcv.id Like '%" & name & "%') and cld_invlk_rcv.custid = " & custid & " and Tipe = 1")

        If modpriv <> 0 Then
            ' If itipe = 1 Then
            strFilter = "SELECT cld_invlk_rcv.*, case cld_invlk_Rcv.flag when 1 then 'Pending' when 2 then 'Finish' END as Status, DATE_FORMAT(cld_invlk_rcv.CreatedDate,'%d-%b-%Y') AS CreateD, 
                            DATE_FORMAT((case cld_invlk_Rcv.flag when 1 then createddate WHEN 2 THEN ifnull(modifdate,createddate) END),'%d-%b-%Y') AS statdate,
                            cld_inv_cc.Name AS ccname, cld_inv_supplier.Name AS Supname, ifnull(cld_inv_supplier.email,'') AS Email FROM cld_invlk_Rcv 
                            LEFT JOIN cld_inv_cc ON cld_invlk_Rcv.cc = cld_inv_cc.ID LEFT JOIN cld_Inv_supplier ON cld_Invlk_Rcv.supid = cld_inv_supplier.id  
                            left Join cld_outlet ON cld_inv_cc.Olid = cld_outlet.id and cld_inv_cc.custid = cld_outlet.custid                            
                            where cld_invlk_Rcv.InvDate >= '" & fromdate.ToString("yyyy-MM-dd") & "' and cld_invlk_Rcv.InvDate <= '" & todate.ToString("yyyy-MM-dd") & "' and (cld_inv_supplier.name Like '%" & name & "%' or cld_invlk_rcv.id Like '%" & name & "%') and cld_invlk_rcv.custid = " & custid & " and Tipe = 1 
                            and olid IN (SELECT Store FROM cld_userxstore WHERE USER = '" & usr & "')
                            ORDER BY InvDate DESC, id DESC"
            'ElseIf itipe = 2 Then
            '    strFilter = "SELECT cld_invlk_rcv.*, case cld_invlk_Rcv.flag when 1 then 'Pending' when 2 then 'Finish' END as Status, DATE_FORMAT(cld_invlk_rcv.CreatedDate,'%d-%b-%Y') AS CreateD,
            '                DATE_FORMAT((case cld_invlk_Rcv.flag when 1 then createddate WHEN 2 THEN ifnull(modifdate,createddate) END),'%d-%b-%Y') AS statdate,
            '                cld_inv_cc.Name AS ccname, cld_inv_supplier.Name AS Supname, ifnull(cld_inv_supplier.email,'') AS Email FROM cld_invlk_Rcv 
            '                LEFT JOIN cld_inv_cc ON cld_invlk_Rcv.cc = cld_inv_cc.ID LEFT JOIN cld_Inv_supplier ON cld_Invlk_Rcv.supid = cld_inv_supplier.id  
            '                left Join cld_outlet ON cld_inv_cc.Olid = cld_outlet.id and cld_inv_cc.custid = cld_outlet.custid
            '                where (cld_inv_supplier.name Like '%" & name & "%' or cld_invlk_rcv.id Like '%" & name & "%') and cld_invlk_rcv.custid = " & custid & " and Tipe = 1 
            '                and olid IN (SELECT Store FROM cld_userxstore WHERE USER = '" & usr & "')
            '                ORDER BY InvDate, Tipe"
            'End If
        Else
            'If itipe = 1 Then
            strFilter = "SELECT cld_invlk_rcv.*, case cld_invlk_Rcv.flag when 1 then 'Pending' when 2 then 'Finish' END as Status, DATE_FORMAT(cld_invlk_rcv.CreatedDate,'%d-%b-%Y') AS CreateD,
                            DATE_FORMAT((case cld_invlk_Rcv.flag when 1 then createddate WHEN 2 THEN ifnull(modifdate,createddate) END),'%d-%b-%Y') AS statdate,
                            cld_inv_cc.Name AS ccname, cld_inv_supplier.Name AS Supname, ifnull(cld_inv_supplier.email,'') AS Email FROM cld_invlk_Rcv LEFT JOIN cld_inv_cc ON cld_invlk_Rcv.cc = cld_inv_cc.ID LEFT JOIN cld_Inv_supplier ON cld_Invlk_Rcv.supid = cld_inv_supplier.id  where cld_invlk_Rcv.InvDate >= '" & fromdate.ToString("yyyy-MM-dd") & "' and cld_invlk_Rcv.InvDate <= '" & todate.ToString("yyyy-MM-dd") & "' and (cld_inv_supplier.name Like '%" & name & "%' or cld_invlk_rcv.id Like '%" & name & "%') and cld_invlk_rcv.custid = " & custid & " and Tipe = 1 ORDER BY InvDate DESC, id DESC"
            'ElseIf itipe = 2 Then
            '    strFilter = "SELECT cld_invlk_rcv.*, case cld_invlk_Rcv.flag when 1 then 'Pending' when 2 then 'Finish' END as Status, DATE_FORMAT(cld_invlk_rcv.CreatedDate,'%d-%b-%Y') AS CreateD,
            '                DATE_FORMAT((case cld_invlk_Rcv.flag when 1 then createddate WHEN 2 THEN ifnull(modifdate,createddate) END),'%d-%b-%Y') AS statdate,
            '                cld_inv_cc.Name AS ccname, cld_inv_supplier.Name AS Supname, ifnull(cld_inv_supplier.email,'') AS Email FROM cld_invlk_Rcv LEFT JOIN cld_inv_cc ON cld_invlk_Rcv.cc = cld_inv_cc.ID LEFT JOIN cld_Inv_supplier ON cld_Invlk_Rcv.supid = cld_inv_supplier.id  where (cld_inv_supplier.name Like '%" & name & "%' or cld_invlk_rcv.id Like '%" & name & "%') and cld_invlk_rcv.custid = " & custid & " and Tipe = 1 ORDER BY InvDate, Tipe"
            'End If

        End If


        Dim a As New clsDbo(strFilter)


        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function
    Public Function SelectDataTable_invRcv(ByVal linkid As String, ByVal custid As Integer) As DataTable
        Dim ds As New DataTable
        Dim strFilter As String = ""


        Dim a As New clsDbo("SELECT *, (ttlStock * cost) AS ttlcost From cld_inv_trans where linkid = '" & linkid & "' and type = 3 and custid = " & custid & "")

        ds = a.selectDatatable

        a.dispose()
        a = Nothing

        Return ds

    End Function


    Public Function SelectDataset_invRcvbyid_grid(ByVal linkid As String, ByVal custid As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""


        strFilter = "SELECT cld_invlk_rcv.*, case cld_invlk_Rcv.flag when 1 then 'Order' when 2 then 'Receive' END as Status, DATE_FORMAT(cld_invlk_rcv.CreatedDate,'%d-%b-%Y') AS CreateD, cld_inv_cc.Name AS ccname, cld_inv_supplier.Name AS Supname   FROM cld_invlk_Rcv LEFT JOIN cld_inv_cc ON cld_invlk_Rcv.cc = cld_inv_cc.ID LEFT JOIN cld_Inv_supplier ON cld_Invlk_Rcv.supid = cld_inv_supplier.id  where cld_invlk_Rcv.id = '" & linkid & "' and cld_invlk_rcv.custid = " & custid & " and Tipe = 1 ORDER BY InvDate, Tipe"

        Dim a As New clsDbo(strFilter)

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function
    Public Function u1_invorderrcv(ByVal ID As String) As Integer
        'Dim ds As New DataSet

        Dim u1_0 As Integer

        Try



            Dim a As New clsDbo("")
            u1_0 = a.ExecuteScalar("SELECT EXISTS(SELECT 1 FROM cld_inv_trans WHERE linkid = '" & ID & "' AND u2>0)", 0)


            a.dispose()
            a = Nothing

        Catch ex As Exception
            errLog(Err.Description, "clsInventory_Warehouse:u1_invorderrcv")
            Err.Clear()
        End Try

        Return u1_0

    End Function
    Public Function SelectDataTable_invRcv_grid(ByVal linkid As String, ByVal custid As Integer, Optional ByVal unit1 As Boolean = False) As DataTable
        Dim ds As New DataTable
        Dim strFilter As String = ""

        If unit1 = 0 Then
            strFilter = "SELECT stk_name Name,FORMAT(ttlstock, 2) 'Qty', u2_nm 'Unit Name',FORMAT(cost, 2) 'Cost', FORMAT((ttlStock * cost), 2) AS 'Total Cost' From cld_inv_trans where linkid = '" & linkid & "' and type = 3 and custid = " & custid & ""
        Else
            strFilter = "SELECT stk_name Name,FORMAT(u1, 2) 'Qty', u1_nm 'Unit Name',FORMAT(costu1, 2) 'Cost', FORMAT((ttlStock * cost), 2) AS 'Total Cost' From cld_inv_trans where linkid = '" & linkid & "' and type = 3 and custid = " & custid & ""
        End If
        Dim a As New clsDbo(strFilter)

        ds = a.selectDatatable

        a.dispose()
        a = Nothing

        Return ds

    End Function

    Function rcv_date(ByVal ID As String) As Integer
        'Dim ds As New DataSet

        Dim edtable As Integer = 0

        Try
            Dim a As New clsDbo("")
            edtable = a.ExecuteScalar("SELECT count(id) from cld_invlk_rcv where  cld_invlk_rcv.id = '" & ID & "' and cld_invlk_rcv.CreatedDate >= '" & Now.AddDays(-7).ToString("yyyy-MM-dd") & "'", 0)


            a.dispose()
            a = Nothing

        Catch ex As Exception
            errLog(Err.Description, "clsInventory_Warehouse:u1_invorderrcv")
            Err.Clear()
        End Try

        Return edtable

    End Function

End Class
