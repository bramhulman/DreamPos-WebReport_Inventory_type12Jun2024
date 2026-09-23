Public Class Cls_IStk

    Public Function SelectDataStkHome(ByVal cust As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""


        Dim a As New clsDbo("SELECT A.*, B.stkLimit, CASE WHEN A.ttlQty > B.stklimit THEN TRUE ELSE FALSE END AS stat  FROM cld_inv_transstock A INNER JOIN cld_inv_stk B ON B.ID = A.stkId WHERE A.Custid = " & cust)


        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function

    Public Function SelectDataStkHomeSearch(ByVal cust As Integer, ByVal ccid As Integer, ByVal stat As Integer, ByVal grpid As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""
        Dim statFilter As String = ""
        Dim grpfilter As String = ""
        Dim staMod As String = ""

        If modpriv And usrModulepriv.moduleuser = usrModulepriv.moduleuser Then
            staMod = "and C.Olid in (Select Store From cld_userxstore where User = '" & usr & "')"
        End If


        If ccid <> 0 Then
            strFilter = "and A.ccid = " & ccid & ""
        End If

        If stat <> 0 Then
            If stat = 1 Then
                statFilter = "AND CASE WHEN A.ttlQty <= B.stklimit THEN TRUE ELSE FALSE END = 0"
            ElseIf stat = 2 Then
                statFilter = "AND CASE WHEN A.ttlQty <= B.stklimit THEN TRUE ELSE FALSE END = 1"
            End If
        End If

        If grpid <> 0 Then
            grpfilter = "and B.grpid = " & grpid & ""
        End If

        'Dim a As New clsDbo("SELECT A.*, B.stkLimit, CASE WHEN A.ttlQty > B.stklimit THEN TRUE ELSE FALSE END AS stat  FROM cld_inv_transstock A INNER JOIN cld_inv_stk B ON B.ID = A.stkId WHERE A.Custid = " & cust & " " & strFilter & " " & statFilter & " " & grpfilter & "")
        Dim a As New clsDbo("SELECT A.*, B.stkLimit, B.unit2 ,CASE WHEN A.ttlQty > B.stklimit THEN TRUE ELSE FALSE END AS stat " & _
                            "FROM cld_inv_transstock A INNER JOIN cld_inv_stk B ON B.ID = A.stkId " & _
                            "INNER JOIN cld_inv_cc C on C.Custid = A.custid and C.ID = A.ccID " & _
                            "WHERE A.Custid = " & cust & " " & staMod & " " & strFilter & " " & statFilter & " " & grpfilter & "")


        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function

    Public Function SelectDataStk(ByVal id As Integer, ByVal cust As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""

        Dim a As New clsDbo("SELECT * FROM cld_Inv_Stk where id = " & id & " and custid = " & cust)

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function

    Public Function SelectDataStkCost(ByVal id As Integer, ByVal cc As Integer, ByVal cust As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""


        Dim a As New clsDbo("SELECT * FROM cld_Inv_Stkcost where stkid = " & id & " and cc = " & cc & "  and custid = " & cust)


        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function

    Public Function SelectDataStk_wOther(ByVal id As Integer, ByVal ccid As Integer, ByVal cust As Integer, ByVal ccware As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""

        Dim a As New clsDbo("SELECT cld_Inv_Stk.*, IFNULL((SELECT ttlqty FROM cld_inv_transstock WHERE stkid = cld_Inv_Stk.id AND ccid = " & ccid & " and custid = " & cust & "), 0) AS stock , IFNULL((SELECT cost FROM cld_inv_stkcost WHERE stkid = cld_Inv_Stk.id AND cc = " & ccware & " AND custid = " & cust & " ), 0) AS coststk FROM cld_Inv_Stk WHERE id = " & id & " and custid = " & cust)

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function

    'penambahan 13 oct 2017,
    'untuk data raw waste detail
    Public Function SelectData_Inv_Stk(ByVal id As Integer, ByVal ccid As Integer, ByVal cust As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""

        Dim a As New clsDbo("SELECT cld_Inv_Stk.*, IFNULL((SELECT ttlqty FROM cld_inv_transstockcls WHERE stkid = cld_Inv_Stk.id AND ccid = " & ccid & " and custid = " & cust & " and invdate = '" & Now.ToString("yyyy-MM-dd 00:00:00") & "'), 0) AS stock , IFNULL((SELECT cost FROM cld_inv_stkcost WHERE stkid = cld_Inv_Stk.id AND cc = " & ccid & " AND custid = " & cust & " ), 0) AS coststk FROM cld_Inv_Stk WHERE id = " & id & " and custid = " & cust)

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function

    Public Function SelectDatastkqty(ByVal id As Integer, ByVal ccid As Integer, ByVal cust As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""

        Dim a As New clsDbo("SELECT cld_inv_transstock.*, IFNULL(ttlqty, 0) as stock FROM cld_inv_transstock WHERE stkid = " & id & " AND ccid = " & ccid & " and custid = " & cust & "")

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function

    Public Function SelectDataset_Stk(ByVal custid As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""

        Dim a As New clsDbo("SELECT cld_inv_stk.id,cld_inv_stk.number,cld_inv_stk.name, cld_cust.nama as custname, cld_inv_stkgroup.name as grpname from cld_inv_stk inner join cld_cust On cld_cust.id = cld_inv_stk.custid LEFT Join cld_inv_stkgroup On cld_inv_stk.grpid = cld_inv_Stkgroup.id where cld_inv_stk.custid = " & custid & " Order by cld_inv_stk.Number ")

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds
    End Function
    Public Function SelectDataset_limitStk(ByVal custid As Integer, Optional ByVal txt As String = "") As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""
        Dim txtsrc As String = ""
        If txt <> "" Then
            txtsrc = " and (cld_inv_stk.`Number` like '%" & txt & "%' or cld_inv_stk.`Name` like '%" & txt & "%' or cld_inv_cc.`Name` like '%" & txt & "%') "
        End If
        Dim a As New clsDbo("SELECT cld_inv_stk_cc.`stkID`,cld_inv_stk_cc.`ccid`,cld_inv_stk.`Number`,cld_inv_stk.`Name`,cld_inv_stk_cc.`stkLimit`,cld_inv_cc.`Name` ccname FROM cld_inv_stk_cc 
INNER JOIN cld_inv_stk ON cld_inv_stk_cc.`stkID` = cld_inv_stk.`ID` AND cld_inv_stk_cc.`CustID` = cld_inv_stk.`CustID`
INNER JOIN cld_inv_cc ON cld_inv_cc.`ID` = cld_inv_stk_cc.`ccid` AND cld_inv_cc.`CustID` = cld_inv_stk_cc.`CustID` where cld_inv_stk.custid = " & custid & " " & txtsrc & " Order by cld_inv_stk.number ")

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds
    End Function
    'Public Function SelectDataset_Stk(ByVal custid As Integer) As DataSet
    '    Dim ds As New DataSet
    '    Dim strFilter As String = ""

    '    Dim a As New clsDbo("SELECT cld_inv_stk.id,cld_inv_stk.number,cld_inv_stk.name,cld_inv_stk_cc.stklimit, cld_cust.nama as custname, cld_inv_stkgroup.name as grpname, (select name from cld_inv_cc where id = cld_inv_stk_cc.ccid) as ccname,cld_inv_stk_cc.ccid from cld_inv_stk inner join cld_inv_stk_cc On cld_inv_stk_cc.stkid = cld_inv_stk.id and cld_inv_stk_cc.custid = cld_inv_stk.custid inner join cld_cust On cld_cust.id = cld_inv_stk.custid inner Join cld_inv_stkgroup On cld_inv_stk.grpid = cld_inv_Stkgroup.id where cld_inv_stk.custid = " & custid & " Order by cld_inv_stk.number ")

    '    ds = a.selectDataset

    '    a.dispose()
    '    a = Nothing

    '    Return ds
    'End Function
    Public Function SelectDataTable_Stk(ByVal dta As DataTable, ByVal custid As Integer, ByVal name As String, ByVal modpriv As Integer) As DataTable
        Dim dt As New DataTable
        Dim strFilter As String = ""
        Dim strf As String = ""
        Dim str As String = ""

        Dim colBValues = New List(Of String)

        If dta.Rows.Count > 0 Then
            For Each row As DataRow In dta.Rows
                If strFilter = "" Then
                    strFilter = row.Item("stk_id")
                Else
                    strFilter = strFilter & "," & row.Item("stk_id")
                End If

            Next
            strf = "and id  not in  (" & strFilter & ") "
        End If
        ' Dim a As New clsDbo("SELECT id, name from cld_inv_stk  where cld_inv_stk.custid = " & custid & " and id  not in  (select stk_id from cld_inv_trans where linkid = '" & linkid & "' )  and name like '%" & name & "%'")

        If modpriv And usrModulepriv.moduleuser = usrModulepriv.moduleuser Then
            str = "SELECT id, name , CASE WHEN Batch = 1 THEN 'B' ELSE '' END AS Batch from cld_inv_stk  where Bsup = true and cld_inv_stk.custid = " & custid & " " & strf & "  and name like '%" & name & "%' order by name"
        Else
            str = "SELECT id, name , CASE WHEN Batch = 1 THEN 'B' ELSE '' END AS Batch from cld_inv_stk  where cld_inv_stk.custid = " & custid & " " & strf & "  and name like '%" & name & "%' order by name"
        End If

        Dim a As New clsDbo(str)

        dt = a.selectDatatable

        a.dispose()
        a = Nothing

        Return dt

    End Function

    Public Function SelectDataTable_Stkcc(ByVal dta As DataTable, ByVal custid As Integer, ByVal name As String, ByVal modpriv As Integer, ByVal ccid As Integer) As DataTable
        Dim dt As New DataTable
        Dim strFilter As String = ""
        Dim strf As String = ""
        Dim str As String = ""

        Dim colBValues = New List(Of String)

        If dta.Rows.Count > 0 Then
            For Each row As DataRow In dta.Rows
                If strFilter = "" Then
                    strFilter = row.Item("stk_id")
                Else
                    strFilter = strFilter & "," & row.Item("stk_id")
                End If

            Next
            strf = "and id  not in  (" & strFilter & ") "
        End If
        ' Dim a As New clsDbo("SELECT id, name from cld_inv_stk  where cld_inv_stk.custid = " & custid & " and id  not in  (select stk_id from cld_inv_trans where linkid = '" & linkid & "' )  and name like '%" & name & "%'")

        If modpriv And usrModulepriv.moduleuser = usrModulepriv.moduleuser Then
            str = "SELECT id, name , CASE WHEN Batch = 1 THEN 'B' ELSE '' END AS Batch,(SELECT ttlqty FROM cld_inv_transstockcls WHERE invdate = '" & Now.Date.ToString("yyyy-MM-dd") & "' and ccid = " & ccid & " AND stkid=id )ttlqty from cld_inv_stk  where Bsup = true and cld_inv_stk.custid = " & custid & " " & strf & "  
                    And id  In  (select stkid from cld_inv_transstockcls where invdate = '" & Now.Date.ToString("yyyy-MM-dd") & "' and ccid = " & ccid & " and ttlqty>1) 
                    and name like '%" & name & "%' order by name"
        Else
            str = "SELECT id, name , CASE WHEN Batch = 1 THEN 'B' ELSE '' END AS Batch,(SELECT ttlqty FROM cld_inv_transstockcls WHERE invdate = '" & Now.Date.ToString("yyyy-MM-dd") & "' and ccid = " & ccid & " AND stkid=id )ttlqty from cld_inv_stk  where cld_inv_stk.custid = " & custid & " " & strf & "  
                    And id  In  (select stkid from cld_inv_transstockcls where invdate = '" & Now.Date.ToString("yyyy-MM-dd") & "' and ccid = " & ccid & " and ttlqty>1) 
                    and name like '%" & name & "%' order by name"
        End If

        Dim a As New clsDbo(str)

        dt = a.selectDatatable

        a.dispose()
        a = Nothing

        Return dt

    End Function

    Public Function SelectDataTable_Stkall(ByVal dta As DataTable, ByVal custid As Integer, ByVal name As String, ByVal modpriv As Integer, ByVal ccid As Integer, Optional ByVal supplierId As Integer = 0) As DataTable
        Dim dt As New DataTable
        Dim strFilter As String = ""
        Dim strf As String = ""
        Dim str As String = ""

        Dim colBValues = New List(Of String)

        If dta.Rows.Count > 0 Then
            For Each row As DataRow In dta.Rows
                If strFilter = "" Then
                    strFilter = row.Item("stk_id")
                Else
                    strFilter = strFilter & "," & row.Item("stk_id")
                End If

            Next
            strf = "and s.id  not in  (" & strFilter & ") "
        End If

        Dim strSup As String = ""
        If supplierId > 0 Then
            strSup = " AND s.id IN (SELECT stk_id FROM cld_inv_stkxsupplier WHERE supplier_id = " & supplierId & " AND custid = " & custid & ") "
        End If

        'If modpriv And usrModulepriv.moduleuser = usrModulepriv.moduleuser Then
        '    str = "select id,name,case when Batch=1 then 'B' else '' end as Batch, " &
        '          "ifnull((select ttlqty from cld_inv_transstockcls where invdate ='" & Now.Date.ToString("yyyy-MM-dd") & "' and ccid=" & ccid & " and stkid=s.id and custid=" & custid & "),0) as ttlqty " &
        '          "from cld_inv_stk s where Bsup=true and custid=" & custid & " " & strf &
        '          " and name like '%" & name.Replace("'", "''") & "%' order by name "
        'Else
        str = "SELECT id, name, CASE WHEN Batch = 1 THEN 'B' ELSE '' END AS Batch, " &
                  "IFNULL((SELECT ttlqty FROM cld_inv_transstockcls WHERE invdate = '" & Now.Date.ToString("yyyy-MM-dd") & "' AND ccid = " & ccid & " AND stkid = s.id AND custid = " & custid & " LIMIT 1), 0) AS ttlqty " &
                  "FROM cld_inv_stk s " &
                  "WHERE custid = " & custid & " " &
                  strf & strSup &
                  "  AND name LIKE '%" & name.Replace("'", "''") & "%' " &
                  "ORDER BY name"
        'End If

        Dim a As New clsDbo(str)

        dt = a.selectDatatable

        a.dispose()
        a = Nothing

        Return dt

    End Function

    Public Function SelectDataTable_Stk_wStock(ByVal dta As DataTable, ByVal custid As Integer, ByVal name As String, ByVal iCc As Integer) As DataTable
        Dim dt As New DataTable
        Dim strFilter As String = ""
        Dim strf As String = ""
        Dim colBValues = New List(Of String)

        If dta.Rows.Count > 0 Then
            For Each row As DataRow In dta.Rows
                If strFilter = "" Then
                    strFilter = row.Item("stk_id")
                Else
                    strFilter = strFilter & "," & row.Item("stk_id")
                End If

            Next
            strf = "and id  not in  (" & strFilter & ") "
        End If
        ' Dim a As New clsDbo("SELECT id, name from cld_inv_stk  where cld_inv_stk.custid = " & custid & " and id  not in  (select stk_id from cld_inv_trans where linkid = '" & linkid & "' )  and name like '%" & name & "%'")
        Dim a As New clsDbo("SELECT id, name , IFNULL((SELECT ttlqty FROM cld_inv_transstock WHERE stkid = cld_Inv_Stk.id AND ccid = " & iCc & " and custid = " & custid & "), 0) AS stock from cld_inv_stk  where cld_inv_stk.custid = " & custid & " " & strf & "  and name like '%" & name & "%'")

        dt = a.selectDatatable

        a.dispose()
        a = Nothing

        Return dt

    End Function

    Public Function SelectDataTable_Stk_wMinStock(ByVal dta As DataTable, ByVal custid As Integer, ByVal iCc As Integer) As DataTable
        Dim dt As New DataTable
        Dim strFilter As String = ""
        Dim strf As String = ""
        Dim colBValues = New List(Of String)

        If dta.Rows.Count > 0 Then
            For Each row As DataRow In dta.Rows
                If strFilter = "" Then
                    strFilter = row.Item("stk_id")
                Else
                    strFilter = strFilter & "," & row.Item("stk_id")
                End If

            Next
            strf = "and transstock.stkID  not in  (" & strFilter & ") "
        End If

        Dim a As New clsDbo("SELECT transstock.stkID, transstock.stkName, transstock.ttlQty, stk.stkLimit,  stk.stkLimit - transstock.ttlQty AS addstock FROM cld_inv_transstock transstock INNER JOIN cld_inv_stk stk ON transstock.stkId = stk.ID AND transstock.CustID  = stk.CustID where transstock.ccid = " & iCc & " and transstock.custid = " & custid & " " & strf)

        dt = a.selectDatatable

        a.dispose()
        a = Nothing

        Return dt

    End Function

    Public Function SelectDataset_StkSearch(ByVal name As String, ByVal custid As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""


        'Dim a As New clsDbo("select cld_inv_stk.*, cld_cust.nama as custname, cld_inv_stkgroup.name as grpname  from cld_inv_stk inner join cld_cust On cld_cust.id = cld_inv_stk.custid inner Join cld_inv_stkgroup On cld_inv_stk.grpid = cld_inv_Stkgroup.id where cld_inv_stk.custid = " & custid & "  and cld_inv_stk.number like  '%" & name & "%' Or cld_inv_stk.name like '%" & name & "%' ")
        Dim a As New clsDbo("SELECT cld_inv_stk.id,cld_inv_stk.number,cld_inv_stk.name, cld_cust.nama as custname, cld_inv_stkgroup.name as grpname from cld_inv_stk inner join cld_cust On cld_cust.id = cld_inv_stk.custid LEFT Join cld_inv_stkgroup On cld_inv_stk.grpid = cld_inv_Stkgroup.id where cld_inv_stk.custid = " & custid & " and cld_inv_stk.number like  '%" & name & "%' Or cld_inv_stk.name like '%" & name & "%' Order by cld_inv_stk.number ")
        ', (select name from cld_inv_cc where id = cld_inv_stk_cc.ccid) as ccname,cld_inv_stk_cc.ccid inner join cld_inv_stk_cc On cld_inv_stk_cc.stkid = cld_inv_stk.id and cld_inv_stk_cc.custid = cld_inv_stk.custid ,cld_inv_stk_cc.stklimit
        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function
    Public Function SelectDataset_StkSearchlimit(ByVal name As String, ByVal custid As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""


        'Dim a As New clsDbo("select cld_inv_stk.*, cld_cust.nama as custname, cld_inv_stkgroup.name as grpname  from cld_inv_stk inner join cld_cust On cld_cust.id = cld_inv_stk.custid inner Join cld_inv_stkgroup On cld_inv_stk.grpid = cld_inv_Stkgroup.id where cld_inv_stk.custid = " & custid & "  and cld_inv_stk.number like  '%" & name & "%' Or cld_inv_stk.name like '%" & name & "%' ")
        Dim a As New clsDbo("SELECT cld_inv_stk.id,cld_inv_stk.number,cld_inv_stk.name, cld_cust.nama as custname, cld_inv_stkgroup.name as grpname from cld_inv_stk inner join cld_cust On cld_cust.id = cld_inv_stk.custid left Join cld_inv_stkgroup On cld_inv_stk.grpid = cld_inv_Stkgroup.id where cld_inv_stk.custid = " & custid & " and cld_inv_stk.number like  '%" & name & "%' Or cld_inv_stk.name like '%" & name & "%' Order by cld_inv_stk.name ")
        ', (select name from cld_inv_cc where id = cld_inv_stk_cc.ccid) as ccname,cld_inv_stk_cc.ccid inner join cld_inv_stk_cc On cld_inv_stk_cc.stkid = cld_inv_stk.id and cld_inv_stk_cc.custid = cld_inv_stk.custid ,cld_inv_stk_cc.stklimit
        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function
    Public Function SelectDataset_StkEdit(ByVal Id As Integer, ByVal custid As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""

        Dim a As New clsDbo("SELECT cld_inv_stk.*, sGrp.Code as grpcode from cld_inv_stk inner join cld_inv_stkgroup sGrp on sGrp.id = cld_inv_stk.grpid where cld_inv_stk.id = " & Id & " and cld_inv_stk.custid = " & custid & "")

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function

    Public Function SelectDataset_StkAll(ByVal custid As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""


        Dim a As New clsDbo("SELECT * from cld_inv_stk where custid = " & custid & " order by name ")

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function

    Public Function SelectDataset_BatchStkAll(ByVal custid As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""

        Dim a As New clsDbo("SELECT * from cld_inv_stk where batch = true and custid = " & custid & "")

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function

    Public Function SelectDataset_nolinkstk(ByVal custid As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""

        Dim a As New clsDbo("SELECT * from cld_inv_stk where blink = 0 and custid = " & custid & "")

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function

    Public Function SelectDataset_Stkbycc(ByVal custid As Integer, ByVal cc As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""

        Dim a As New clsDbo("SELECT * from cld_inv_stk where Id not in (select stkid from cld_inv_transstock where ccid = " & cc & " and custid = " & custid & ") and custid = " & custid & "")

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function

    Public Function SelectDataset_Stk_Export(ByVal custID As Integer) As DataSet
        Dim ds As New DataSet

        Try

            Dim a As New clsDbo("SELECT stk.Number, stk.Name, stkgroup.CODE as grpCode, stk.Unit1, stk.Unit2, stk.Unit3, stk.Factor1, stk.Factor2, stk.stkLimit, stk.price1, stk.price2 " &
                            "FROM cld_inv_stk AS stk INNER JOIN cld_inv_stkgroup AS stkgroup ON stk.grpid = stkgroup.ID AND stk.CustID = stkgroup.CustID " &
                            "WHERE stk.CustID = " & custID)

            ds = a.selectDataset

            a.dispose()
            a = Nothing

            Return ds

        Catch ex As Exception
            errLog(Err.Description, "Cls_IStk:SelectDataset_Stk_Export")
            Err.Clear()
        End Try

        Return ds
    End Function


    Public Function SelectDataTable_Stk_Export(ByVal CustID As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataTable
        Dim ds As New DataTable
        Dim strFilter As String = ""


        'perubahan 3 may 2016
        'Dim a As New clsDbo("SELECT stk.Number, stk.Name, stkgroup.CODE as grpCode, stk.Unit1, stk.Unit2, stk.Unit3, stk.Factor1, stk.Factor2, cld_inv_stk_cc.stkLimit,(select name from cld_inv_cc where id = cld_inv_stk_cc.ccid) as ccname " &
        '                    "FROM cld_inv_stk AS stk INNER JOIN cld_inv_stkgroup AS stkgroup ON stk.grpid = stkgroup.ID AND stk.CustID = stkgroup.CustID 
        '                    inner join cld_inv_stk_cc On cld_inv_stk_cc.stkid = stk.id and cld_inv_stk_cc.custid = stk.custid  " &
        '                    "WHERE stk.CustID = " & CustID)
        If modpriv And usrModulepriv.moduleuser = usrModulepriv.moduleuser Then
            strFilter = "SELECT stk.Number, stk.Name, stkgroup.CODE as grpCode, stk.Unit1, stk.Unit2, stk.Unit3, stk.Factor1, stk.Factor2 " &
                            "FROM cld_inv_stk AS stk INNER JOIN cld_inv_stkgroup AS stkgroup ON stk.grpid = stkgroup.ID AND stk.CustID = stkgroup.CustID " &
                            "WHERE stk.CustID = " & CustID
        Else
            'strFilter = "SELECT stk.Number, stk.Name, stkgroup.CODE as grpCode, stk.Unit1, stk.Unit2, stk.Unit3, stk.Factor1, stk.Factor2, IFNULL( cls.cost,0) cost, stk.Price1, stk.Price2 " &
            '                "FROM cld_inv_stk AS stk INNER JOIN cld_inv_stkgroup AS stkgroup ON stk.grpid = stkgroup.ID AND stk.CustID = stkgroup.CustID 
            '                LEFT JOIN (SELECT * FROM cld_inv_transstockcls WHERE invdate = '" & Now.Date.ToString("yyyy-MM-dd") & "' AND ccid = (select id from cld_inv_cc where olid = 0)) cls ON cls.stkid = stk.ID AND cls.custid = stk.custid " &
            '                "WHERE stk.CustID = " & CustID
            strFilter = "SELECT stk.Number, stk.Name, stkgroup.CODE as grpCode, stk.Unit1, stk.Unit2, stk.Unit3, stk.Factor1, stk.Factor2 " &
                            "FROM cld_inv_stk AS stk INNER JOIN cld_inv_stkgroup AS stkgroup ON stk.grpid = stkgroup.ID AND stk.CustID = stkgroup.CustID " &
                            "WHERE stk.CustID = " & CustID
        End If

        Dim a As New clsDbo(strFilter)
        ds = a.selectDatatable

        a.dispose()
        a = Nothing

        Return ds

    End Function
    Public Function SelectDataTable_Stk_Export2(ByVal CustID As Integer, ByVal usr As String, ByVal modpriv As Integer, Optional ByVal fcost As Integer = 0) As DataTable
        Dim ds As New DataTable
        Dim strFilter As String = ""


        If modpriv And usrModulepriv.moduleuser = usrModulepriv.moduleuser Then
            strFilter = "SELECT stk.Number, stk.Name, stkgroup.CODE as grpCode, stk.Unit1, stk.Unit2, stk.Unit3, stk.Factor1, stk.Factor2 " &
                            "FROM cld_inv_stk AS stk INNER JOIN cld_inv_stkgroup AS stkgroup ON stk.grpid = stkgroup.ID AND stk.CustID = stkgroup.CustID " &
                            "WHERE stk.CustID = " & CustID
        Else
            If fcost = 0 Then
                strFilter = "SELECT stk.Number, stk.Name, stkgroup.CODE as grpCode, stk.Unit1, stk.Unit2, stk.Unit3, stk.Factor1, stk.Factor2 " &
                            "FROM cld_inv_stk AS stk INNER JOIN cld_inv_stkgroup AS stkgroup ON stk.grpid = stkgroup.ID AND stk.CustID = stkgroup.CustID " &
                            "WHERE stk.CustID = " & CustID
            Else
                strFilter = "SELECT stk.Number, stk.Name, stkgroup.CODE as grpCode, stk.Unit1, stk.Unit2, stk.Unit3, stk.Factor1, stk.Factor2, fc.cost " &
                            "FROM cld_inv_stk AS stk INNER JOIN cld_inv_stkgroup AS stkgroup ON stk.grpid = stkgroup.ID AND stk.CustID = stkgroup.CustID " &
                 "INNER JOIN cld_inv_fixcost fc on fc.stkid = stk.id " &
                "WHERE stk.CustID = " & CustID
            End If

        End If

        Dim a As New clsDbo(strFilter)
        ds = a.selectDatatable

        a.dispose()
        a = Nothing

        Return ds

    End Function
    Public Function SelectDataTable_limitStk_Export(ByVal CustID As Integer) As DataTable
        Dim ds As New DataTable
        Dim strFilter As String = ""


        'perubahan 3 may 2016
        Dim a As New clsDbo("SELECT stk.Number, stk.Name, cld_inv_stk_cc.stkLimit,(select name from cld_inv_cc where id = cld_inv_stk_cc.ccid) as ccname " &
                            "FROM cld_inv_stk AS stk INNER JOIN cld_inv_stkgroup AS stkgroup ON stk.grpid = stkgroup.ID AND stk.CustID = stkgroup.CustID 
                            inner join cld_inv_stk_cc On cld_inv_stk_cc.stkid = stk.id and cld_inv_stk_cc.custid = stk.custid  " &
                            "WHERE stk.CustID = " & CustID)


        ds = a.selectDatatable

        a.dispose()
        a = Nothing

        Return ds

    End Function

    Public Function getds_Stkreplace(ByVal custid As Integer, Optional ByVal name As String = "") As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""

        Dim a As New clsDbo("SELECT sc.id,date_format(sc.transdate,'%d-%b-%Y') transdate,sc.username,stk1.number,stk1.name,stk.number kodebefore,stk.name namebefore FROM `cld_inv_stk_replace` sc
JOIN `cld_inv_stk` stk ON sc.beforestk=stk.id AND sc.custid=stk.custid
JOIN `cld_inv_stk` stk1 ON sc.afterstk=stk1.id AND sc.custid=stk1.custid
WHERE sc.custid = " & custid & " AND (sc.username LIKE '%" & name & "%' OR stk1.name LIKE '%" & name & "%' OR stk1.number LIKE '%" & name & "%') ORDER BY stk1.name LIMIT 200 ")

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds
    End Function

End Class
