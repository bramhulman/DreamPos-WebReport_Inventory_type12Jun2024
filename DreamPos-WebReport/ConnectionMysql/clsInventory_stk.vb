Imports Microsoft.VisualBasic.Devices

Public Class clsInventory_stk

    Public Function SelectDataset_StkEdit(ByVal Id As Integer, ByVal custid As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""
        'Dim p1 As Integer = 0
        'Dim p2 As Integer = 0

        If My.Settings.price1 IsNot Nothing Then
            p1 = My.Settings.price1
        Else
            p1 = 10
        End If
        If My.Settings.price2 IsNot Nothing Then
            p2 = My.Settings.price2
        Else
            p2 = 21
        End If
        'If My.Settings.fixcost <> 0 Then
        'strFilter = "SELECT cld_inv_stk.*, ifnull(sGrp.Code,0) as grpcode, ifnull(format(scls.cost,2),0) cost,ifnull(format(fc.cost,2),0) fcost
        '                       from cld_inv_stk 
        '                        left join cld_inv_fixcost fc on fc.stkid = cld_inv_stk.id
        '                        left join cld_inv_stkgroup sGrp on sGrp.id = cld_inv_stk.grpid 
        '                        left join cld_inv_transstockcls scls on scls.stkid = cld_inv_stk.id and ccid = (select id from cld_inv_cc where olid = 0) and invdate = '" & Now.Date.ToString("yyyy-MM-dd") & "'
        '                        where cld_inv_stk.id = " & Id & " and cld_inv_stk.custid = " & custid & ""

        'Else
        strFilter = "SELECT cld_inv_stk.*, ifnull(sGrp.Code,0) as grpcode, ifnull(format(scls.cost,2),0) cost
                           from cld_inv_stk 
                            left join cld_inv_stkgroup sGrp on sGrp.id = cld_inv_stk.grpid 
                            left join cld_inv_transstockcls scls on scls.stkid = cld_inv_stk.id and ccid = (select id from cld_inv_cc where olid = 0) and invdate = '" & Now.Date.ToString("yyyy-MM-dd") & "'
                            where cld_inv_stk.id = " & Id & " and cld_inv_stk.custid = " & custid & ""
        'End If
        Dim a As New clsDbo(strFilter)

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function
    Public Function SelectDataset_limitStkEdit(ByVal stkId As Integer, ByVal ccid As Integer, ByVal custid As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""

        Dim a As New clsDbo("SELECT * from cld_inv_stk_cc where stkid = " & stkId & " and ccid = " & ccid & " and custid = " & custid & "")

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function
    Public Function SelectDataset_StkEdit_percc(ByVal Id As Integer, ByVal custid As Integer, ByVal ccid As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""

        Dim a As New clsDbo("SELECT cld_inv_stk.id,cld_inv_stk.number,cld_inv_stk.name,cld_inv_stk_cc.stklimit, sGrp.Code as grpcode, cld_inv_stk_cc.ccid from cld_inv_stk inner join cld_inv_stk_cc On cld_inv_stk_cc.stkid = cld_inv_stk.id and cld_inv_stk_cc.custid = cld_inv_stk.custid inner join cld_inv_stkgroup sGrp on sGrp.id = cld_inv_stk.grpid where cld_inv_stk.id = " & Id & " and cld_inv_stk.custid = " & custid & "")

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function
    'Public Function SelectDataset_StkEdit(ByVal Id As Integer, ByVal custid As Integer) As DataSet
    '    Dim ds As New DataSet
    '    Dim strFilter As String = ""

    '    Dim a As New clsDbo("SELECT cld_inv_stk.*, sGrp.Code as grpcode from cld_inv_stk inner join cld_inv_stkgroup sGrp on sGrp.id = cld_inv_stk.grpid where cld_inv_stk.id = " & Id & " and cld_inv_stk.custid = " & custid & "")

    '    ds = a.selectDataset

    '    a.dispose()
    '    a = Nothing

    '    Return ds

    'End Function
    Public Function SelectDataset_StkByNumber(ByVal stkNo As String) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""

        Dim a As New clsDbo("SELECT * from cld_inv_stk Where Number = '" & stkNo & "'")

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function

    Public Sub insertStk(ByVal id As Integer, ByVal custid As Integer, ByVal code As String, ByVal nama As String, ByVal grpid As String, ByVal Unit1 As String, ByVal Unit2 As String, ByVal Unit3 As String, ByVal Factor1 As Decimal, ByVal Factor2 As Decimal, ByVal batch As Boolean, ByVal Blink As Integer, ByVal price1 As Decimal, ByVal price2 As Decimal, ByVal username As String)
        Try

            Dim str As String
            Dim c_cmd As New clsDbo("")
            Dim stklimit As Integer = 0
            Dim iGrp As Integer = 0

            iGrp = c_cmd.ExecuteScalar("Select ID from cld_inv_stkgroup where code = '" & grpid & "'", 2)
            If iGrp = 0 Then
                Exit Sub
            End If


            If id = 0 Then
                c_cmd.resetCmd("")
                id = c_cmd.ExecuteScalar("Select ID from cld_inv_stk where Number = '" & code & "'", 2)
            End If


            If nama.Length > 30 Then
                nama = Mid(nama, 1, 30)
            End If


            If id = 0 Then
                id = c_cmd.ExecuteScalar("select max(ID) from cld_inv_stk")
                str = "insert into cld_inv_stk values (" & c_cmd.ExecuteScalar("select max(ID) from cld_inv_stk") & ", " & custid & ",'" & code & "', '" & nama & "'," & iGrp & ", '" & Unit1 & "', '" & Unit2 & "', '" & Unit3 & "', " & Factor1 & ", " & Factor2 & ", " & stklimit & ", " & batch & ", " & Blink & ", " & price1 & ", " & price2 & ")"

            Else
                If batch = False Then
                    str = "Update cld_Inv_stk set batch = false, Blink = 0, Number = '" & code & "', name = '" & nama & "', grpid = " & iGrp & ", Unit1 = '" & Unit1 & "', Unit2 = '" & Unit2 & "', Unit3 ='" & Unit3 & "', Factor1 = " & Factor1 & ", Factor2 = " & Factor2 & ", stklimit = " & stklimit & ", price1 = " & price1 & ", price2 = " & price2 & " where id = " & id & ""

                Else
                    str = "Update cld_Inv_stk set batch = " & batch & ", Blink = " & Blink & ", Number = '" & code & "', name = '" & nama & "', grpid = " & iGrp & ", Unit1 = '" & Unit1 & "', Unit2 = '" & Unit2 & "', Unit3 ='" & Unit3 & "', Factor1 = " & Factor1 & ", Factor2 = " & Factor2 & ", stklimit = " & stklimit & ", price1 = " & price1 & ", price2 = " & price2 & " where id = " & id & ""

                End If

            End If

            c_cmd.resetCmd("")
            c_cmd.executeNonQuery(str)
            'tambahan untuk tambah per cc
            'c_cmd.resetCmd("")
            'If c_cmd.ExecuteScalar("Select count(stkID) from cld_inv_stk_cc where stkID = " & id & " and ccID = " & stkccid & " and custid = " & custid & "", 2) > 0 Then
            '    str = "Update cld_inv_stk_cc set stklimit = " & stklimit & " where stkID = " & id & " and ccID = " & stkccid & " and custid = " & custid & ""
            'Else
            '    str = "insert into cld_inv_stk_cc values (" & id & ", " & custid & "," & stklimit & ", " & stkccid & ")"

            'End If
            'c_cmd.resetCmd("")
            'c_cmd.executeNonQuery(str)

            c_cmd = Nothing

        Catch ex As Exception

            errLog(Err.Description, "Cls_IStk:insertStk")
            Err.Clear()
        End Try
    End Sub
    Public Sub insertStk_imp(ByVal id As Integer, ByVal custid As Integer, ByVal code As String, ByVal nama As String, ByVal grpid As String, ByVal Unit1 As String, ByVal Unit2 As String, ByVal Unit3 As String, ByVal Factor1 As Decimal, ByVal Factor2 As Decimal, ByVal batch As Boolean, ByVal Blink As Integer, ByVal price1 As Decimal, ByVal price2 As Decimal, ByVal username As String, Optional ByVal fcost As Double = 0)
        Dim str As String = ""
        Try


            Dim c_cmd As New clsDbo("")
            Dim stklimit As Integer = 0
            Dim iGrp As Integer = 0

            iGrp = c_cmd.ExecuteScalar("Select ID from cld_inv_stkgroup where code = '" & grpid & "'", 2)
            If iGrp = 0 Then
                Exit Sub
            End If


            If id = 0 Then
                c_cmd.resetCmd("")
                id = c_cmd.ExecuteScalar("Select ID from cld_inv_stk where Number = '" & code & "'", 2)
            End If


            If nama.Length > 30 Then
                nama = Mid(nama, 1, 30)
            End If


            If id = 0 Then
                id = c_cmd.ExecuteScalar("select max(ID) from cld_inv_stk")
                str = "insert into cld_inv_stk values (" & c_cmd.ExecuteScalar("select max(ID) from cld_inv_stk") & ", " & custid & ",'" & code & "', '" & nama & "'," & iGrp & ", '" & Unit1 & "', '" & Unit2 & "', '" & Unit3 & "', " & Factor1 & ", " & Factor2 & ", " & stklimit & ", " & batch & ", " & Blink & ", " & price1 & ", " & price2 & ")"


            Else
                'If batch = False Then
                '    str = "Update cld_Inv_stk set batch = false, Blink = 0, Number = '" & code & "', name = '" & nama & "', grpid = " & iGrp & ", Unit1 = '" & Unit1 & "', Unit2 = '" & Unit2 & "', Unit3 ='" & Unit3 & "', Factor1 = " & Factor1 & ", Factor2 = " & Factor2 & ", stklimit = " & stklimit & ", price1 = " & price1 & ", price2 = " & price2 & " where id = " & id & ""

                'Else
                '    str = "Update cld_Inv_stk set batch = " & batch & ", Blink = " & Blink & ", Number = '" & code & "', name = '" & nama & "', grpid = " & iGrp & ", Unit1 = '" & Unit1 & "', Unit2 = '" & Unit2 & "', Unit3 ='" & Unit3 & "', Factor1 = " & Factor1 & ", Factor2 = " & Factor2 & ", stklimit = " & stklimit & ", price1 = " & price1 & ", price2 = " & price2 & " where id = " & id & ""

                'End If
                str = "Update cld_Inv_stk set Number = '" & code & "', name = '" & nama & "', grpid = " & iGrp & ", Unit1 = '" & Unit1 & "', Unit2 = '" & Unit2 & "', Unit3 ='" & Unit3 & "', Factor1 = " & Factor1 & ", Factor2 = " & Factor2 & ", stklimit = " & stklimit & ", price1 = " & price1 & ", price2 = " & price2 & " where id = " & id & ""

            End If

            'If My.Settings.fixcost <> 0 Then
            '    insertfcost_Stk(id, custid, fcost, username, DateTime.Now)
            'End If

            c_cmd.resetCmd("")
            c_cmd.executeNonQuery(str)
            'tambahan untuk tambah per cc
            'c_cmd.resetCmd("")
            'If c_cmd.ExecuteScalar("Select count(stkID) from cld_inv_stk_cc where stkID = " & id & " and ccID = " & stkccid & " and custid = " & custid & "", 2) > 0 Then
            '    str = "Update cld_inv_stk_cc set stklimit = " & stklimit & " where stkID = " & id & " and ccID = " & stkccid & " and custid = " & custid & ""
            'Else
            '    str = "insert into cld_inv_stk_cc values (" & id & ", " & custid & "," & stklimit & ", " & stkccid & ")"

            'End If
            'c_cmd.resetCmd("")
            'c_cmd.executeNonQuery(str)

            c_cmd = Nothing

        Catch ex As Exception

            errLog(Err.Description, str & " Cls_IStk:insertStk")
            Err.Clear()
        End Try
    End Sub
    Public Sub insertlimitStk(ByVal stkid As Integer, ByVal custid As Integer, ByVal code As String, ByVal nama As String, ByVal stklimit As Integer, ByVal ccid As Integer)
        Dim str As String = ""
        Try


            Dim c_cmd As New clsDbo("")
            Dim nstkid As Integer = 0

            If ccid = 0 Then
                Exit Sub
            End If
            If stkid = 0 Then
                c_cmd.resetCmd("")
                nstkid = c_cmd.ExecuteScalar("Select ID from cld_inv_stk where Number = '" & code & "'", 2)
            End If
            'If custid = 0 Then
            '    c_cmd.resetCmd("")
            '    custid = c_cmd.ExecuteScalar(" SELECT id FROM `cld_cust` LIMIT 1", 0)
            'End If

            If nama.Length > 15 Then
                nama = Mid(nama, 1, 15)
            End If
            If custid = 0 Then
                c_cmd.resetCmd("")
                custid = c_cmd.ExecuteScalar("SELECT id FROM `cld_cust` LIMIT 1", 0)
            Else
                If custid <> c_cmd.ExecuteScalar("SELECT id FROM `cld_cust` LIMIT 1", 0) Then
                    errLog("wrong custid" & " clsInventory_stk:insertlimitStk", custid.ToString)
                    custid = c_cmd.ExecuteScalar("SELECT id FROM `cld_cust` LIMIT 1", 0)
                End If
            End If
            If c_cmd.ExecuteScalar("SELECT IFNULL(custid,0) cust_id FROM `cld_inv_stk_cc` WHERE custid<>" & custid & " LIMIT 1", 0) > 0 Then
                c_cmd.ReNew()
                c_cmd.executeNonQuery("DELETE FROM `cld_inv_stk_cc` WHERE custid<>" & custid)
            End If
            'If My.Settings.stk_cc = 1 Then
            If c_cmd.ExecuteScalar("Select count(stkID) from cld_inv_stk_cc where stkid = " & nstkid & " and ccid = " & ccid & "", 2) = 0 Then

                Str = "insert into cld_inv_stk_cc (stkID,CustID,stkLimit,ccid) values (" & nstkid & ", " & custid & "," & stklimit & ", " & ccid & ")"

            Else

                Str = "Update cld_inv_stk_cc set stklimit = " & stklimit & " where stkid = " & nstkid & " and ccid = " & ccid & " and custid = " & custid & ""



            End If

            c_cmd.resetCmd("")
            c_cmd.executeNonQuery(Str)
            'End If


            'tambahan untuk tambah per cc
            'c_cmd.resetCmd("")
            'If c_cmd.ExecuteScalar("Select count(stkID) from cld_inv_stk_cc where stkID = " & id & " and ccID = " & stkccid & " and custid = " & custid & "", 2) > 0 Then
            '    str = "Update cld_inv_stk_cc set stklimit = " & stklimit & " where stkID = " & id & " and ccID = " & stkccid & " and custid = " & custid & ""
            'Else
            '    str = "insert into cld_inv_stk_cc values (" & id & ", " & custid & "," & stklimit & ", " & stkccid & ")"

            'End If
            'c_cmd.resetCmd("")
            'c_cmd.executeNonQuery(str)

            c_cmd = Nothing

        Catch ex As Exception

            errLog(Err.Description, str & " Cls_IStk:insertStk")
            Err.Clear()
        End Try
    End Sub
    Public Function get_stkFactor1(ByVal stkid As Integer, ByVal stkcust As Integer) As Decimal

        Dim hasil As Decimal = 0
        Try

            Dim cCmd As New clsDbo("")
            hasil = cCmd.ExecuteScalar("SELECT Factor1 FROM cld_inv_stk  WHERE id = " & stkid & " AND Custid = " & stkcust & "", 0)

            cCmd = Nothing

        Catch ex As Exception
            errLog(Err.Description, "clsInventory_stkCost:get_stkCost")
            Err.Clear()

            hasil = 0

        End Try

        Return hasil

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

        Dim a As New clsDbo("SELECT id, name , IFNULL((TRIM(TRAILING '.' FROM(CAST(TRIM(TRAILING '0' FROM (SELECT ttlqty FROM cld_inv_transstockcls WHERE stkid = cld_Inv_Stk.id AND ccid = " & iCc & " and custid = " & custid & " AND invdate = '" & Now.Date.ToString("yyyy-MM-dd") & "' ))AS char)))) , 0) AS stock from cld_inv_stk  where cld_inv_stk.custid = " & custid & " " & strf & "  and name like '%" & name & "%' order by name ")

        dt = a.selectDatatable

        a.dispose()
        a = Nothing

        Return dt

    End Function
    Public Function SelectDt_Stk_all2(ByVal dta As DataTable, ByVal custid As Integer, ByVal name As String, ByVal iCc As Integer) As DataTable
        Dim dt As New DataTable
        Dim strFilter As String = ""
        Dim strf As String = ""
        Dim colBValues = New List(Of String)

        If dta.Rows.Count > 0 Then
            For Each row As DataRow In dta.Rows
                Dim sId As Object = row.Item("stk_id")
                If sId IsNot DBNull.Value AndAlso IsNumeric(sId) AndAlso CInt(sId) > 0 Then
                    If strFilter = "" Then
                        strFilter = sId.ToString()
                    Else
                        strFilter = strFilter & "," & sId.ToString()
                    End If
                End If
            Next
            If strFilter <> "" Then
                strf = "and id not in (" & strFilter & ") "
            End If
        End If


        Dim a As New clsDbo("SELECT id, name from cld_inv_stk  where cld_inv_stk.custid = " & custid & " " & strf & "  
                 and name like '%" & name & "%' order by name ")
        dt = a.selectDatatable

        a.dispose()
        a = Nothing

        Return dt

    End Function
    Public Function SelectDataTable_Stk_wStockWH(ByVal dta As DataTable, ByVal custid As Integer, ByVal name As String, ByVal iCc As Integer, ByVal WH As Integer) As DataTable
        Dim dt As New DataTable
        Dim strFilter As String = ""
        Dim strf As String = ""
        Dim colBValues = New List(Of String)

        If dta.Rows.Count > 0 Then
            For Each row As DataRow In dta.Rows
                Dim sId As Object = row.Item("stk_id")
                If sId IsNot DBNull.Value AndAlso IsNumeric(sId) AndAlso CInt(sId) > 0 Then
                    If strFilter = "" Then
                        strFilter = sId.ToString()
                    Else
                        strFilter = strFilter & "," & sId.ToString()
                    End If
                End If
            Next
            If strFilter <> "" Then
                strf = "and id not in (" & strFilter & ") "
            End If
        End If

        'Dim a As New clsDbo("SELECT id, name , IFNULL((TRIM(TRAILING '.' FROM(CAST(TRIM(TRAILING '0' FROM (SELECT ttlqty FROM cld_inv_transstockcls WHERE stkid = cld_Inv_Stk.id AND ccid = " & iCc & " and custid = " & custid & " AND invdate = '" & Now.Date.ToString("yyyy-MM-dd") & "' ))AS char)))) , 0) AS stock from cld_inv_stk  where cld_inv_stk.custid = " & custid & " " & strf & "  
        '        AND id  IN  (SELECT stkid FROM cld_inv_transstockcls WHERE invdate = '" & Now.Date.ToString("yyyy-MM-dd") & "' AND ccid = " & WH & " AND ttlqty>1) 
        '        and name like '%" & name & "%' order by name ")
        Dim a As New clsDbo("SELECT id, name , IFNULL((TRIM(TRAILING '.' FROM(CAST(TRIM(TRAILING '0' FROM (SELECT ttlqty FROM cld_inv_transstockcls WHERE stkid = cld_Inv_Stk.id AND ccid = " & iCc & " and custid = " & custid & " AND invdate = '" & Now.Date.ToString("yyyy-MM-dd") & "' ))AS char)))) , 0) AS stock from cld_inv_stk  where cld_inv_stk.custid = " & custid & " " & strf & "  
                 and name like '%" & name & "%' order by name ")
        dt = a.selectDatatable

        a.dispose()
        a = Nothing

        Return dt

    End Function

    'stock data, ttqty dan cost warehouse
    'getStkByid2 di ws lama
    Public Function SelectDataStk_wOther(ByVal id As Integer, ByVal ccid As Integer, ByVal cust As Integer, ByVal ccware As Integer, ByVal invdate As Date) As DataSet
        Dim ds As New DataSet

        Try
            Dim a As New clsDbo("SELECT cld_Inv_Stk.*, IFNULL((SELECT cost FROM cld_inv_transstockcls 
WHERE stkid = cld_Inv_Stk.id AND ccid = " & ccware & " and invdate ='" & invdate.Date.ToString("yyyy-MM-dd") & "' AND custid = " & cust & " ), 0) AS coststk 
, IFNULL((SELECT ttlqty FROM cld_inv_transstockcls 
WHERE stkid = cld_Inv_Stk.id AND ccid = " & ccware & " and invdate ='" & invdate.Date.ToString("yyyy-MM-dd") & "' AND custid = " & cust & " ), 0) AS stock 
FROM cld_Inv_Stk WHERE id = " & id & " and custid = " & cust & " order by cld_Inv_Stk.name ")
            ds = a.selectDataset

            a.dispose()
            a = Nothing
        Catch ex As Exception

            errLog(Err.Description, "clsInventory_stk:SelectDataStk_wOther")
            Err.Clear()
        End Try


        Return ds

    End Function
    Public Function SelectDataStk_wOther2(ByVal id As Integer, ByVal ccid As Integer, ByVal cust As Integer, ByVal ccware As Integer, ByVal invdate As Date) As DataSet
        Dim ds As New DataSet

        Try
            Dim a As New clsDbo("SELECT cld_Inv_Stk.*, IFNULL((SELECT stklimit FROM cld_inv_stk_cc WHERE stkid = " & id & " AND ccid = " & ccid & " 
and custid = " & cust & "), 0) AS stk_limit, IFNULL((SELECT ttlqty FROM cld_inv_transstockcls WHERE stkid = cld_Inv_Stk.id AND ccid = " & ccid & " 
and invdate ='" & invdate.Date.ToString("yyyy-MM-dd") & "' and custid = " & cust & "), 0) AS stockcc, IFNULL((SELECT ttlqty FROM cld_inv_transstockcls WHERE stkid = cld_Inv_Stk.id AND ccid = " & ccware & " 
and invdate ='" & invdate.Date.ToString("yyyy-MM-dd") & "' and custid = " & cust & "), 0) AS stock, IFNULL((SELECT cost FROM cld_inv_transstockcls 
WHERE stkid = cld_Inv_Stk.id AND ccid = " & ccware & " and invdate ='" & invdate.Date.ToString("yyyy-MM-dd") & "' AND custid = " & cust & " ), 0) AS coststk 
FROM cld_Inv_Stk WHERE id = " & id & " and custid = " & cust)
            ds = a.selectDataset

            a.dispose()
            a = Nothing
        Catch ex As Exception

            errLog(Err.Description, "clsInventory_stk:SelectDataStk_wOther")
            Err.Clear()
        End Try


        Return ds

    End Function
    Public Function SelectDataStk_wOther3(ByVal id As Integer, ByVal ccid As Integer, ByVal cust As Integer, ByVal invdate As Date) As DataSet
        Dim ds As New DataSet

        Try
            Dim a As New clsDbo("SELECT cld_Inv_Stk.*, IFNULL((SELECT stklimit FROM cld_inv_stk_cc WHERE stkid = " & id & " AND ccid = " & ccid & " 
and custid = " & cust & "), 0) AS stk_limit, IFNULL((SELECT ttlqty FROM cld_inv_transstockcls WHERE stkid = cld_Inv_Stk.id AND ccid = " & ccid & " 
and invdate ='" & invdate.Date.ToString("yyyy-MM-dd") & "' and custid = " & cust & "), 0) AS stockcc, 0 AS stock, 0 AS coststk 
FROM cld_Inv_Stk WHERE id = " & id & " and custid = " & cust)
            ds = a.selectDataset

            a.dispose()
            a = Nothing
        Catch ex As Exception

            errLog(Err.Description, "clsInventory_stk:SelectDataStk_wOther")
            Err.Clear()
        End Try


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

        'If modpriv And usrModulepriv.moduleuser = usrModulepriv Then
        '    str = "SELECT id, name , CASE WHEN Batch = 1 THEN 'B' ELSE '' END AS Batch from cld_inv_stk  where batch = true and cld_inv_stk.custid = " & custid & " " & strf & "  and name like '%" & name & "%' order by name"
        'Else
        str = "SELECT id, name , CASE WHEN Batch = 1 THEN 'B' ELSE '' END AS Batch from cld_inv_stk  where cld_inv_stk.custid = " & custid & " " & strf & "  and name like '%" & name & "%' order by name"
        'End If

        Dim a As New clsDbo(str)

        dt = a.selectDatatable

        a.dispose()
        a = Nothing

        Return dt

    End Function
    Public Function SelectDataTable_Stk2(ByVal dta As DataTable, ByVal custid As Integer, ByVal name As String, ByVal modpriv As Integer, ByVal ccid As Integer) As DataTable
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

        'If modpriv And usrModulepriv.moduleuser = usrModulepriv Then
        '    str = "SELECT id, name , CASE WHEN Batch = 1 THEN 'B' ELSE '' END AS Batch from cld_inv_stk  where batch = true and cld_inv_stk.custid = " & custid & " " & strf & " 
        '            And id  In  (select stkid from cld_inv_transstockcls where invdate = '" & Now.Date.ToString("yyyy-MM-dd") & "' and ccid = " & ccid & " and ttlqty>0) 
        '            and name like '%" & name & "%' order by name"
        'Else
        str = "SELECT id, name , CASE WHEN Batch = 1 THEN 'B' ELSE '' END AS Batch from cld_inv_stk  where cld_inv_stk.custid = " & custid & " " & strf & " 
                    And id  In  (select stkid from cld_inv_transstockcls where invdate = '" & Now.Date.ToString("yyyy-MM-dd") & "' and ccid = " & ccid & " and ttlqty>0) 
                    and name like '%" & name & "%' order by name"
        'End If

        Dim a As New clsDbo(str)

        dt = a.selectDatatable

        a.dispose()
        a = Nothing

        Return dt

    End Function
    Public Function SelectDataStkCost(ByVal id As Integer, ByVal cc As Integer, ByVal cust As Integer, ByVal invdate As Date) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""


        'Dim a As New clsDbo("SELECT * FROM cld_Inv_Stkcost where stkid = " & id & " and cc = " & cc & "  and custid = " & cust)
        Dim a As New clsDbo("SELECT * FROM cld_inv_transstockcls where stkid = " & id & " and ccid = " & cc & " and invdate ='" & invdate.Date.ToString("yyyy-MM-dd") & "' and custid = " & cust)
        ' Dim a As New clsDbo(" SELECT * FROM cld_inv_transstockcls A INNER JOIN cld_inv_stk B ON B.ID = A.stkId
        'INNER JOIN cld_inv_cc C ON C.Custid = A.custid AND C.ID = A.ccID 
        'INNER JOIN cld_inv_stk_cc D ON D.Custid = A.custid AND D.ccID = C.ID AND D.stkID = B.ID 
        'where a.stkid = " & id & " and a.ccid = " & cc & " and a.invdate ='" & invdate.Date.ToString("yyyy-MM-dd") & "' and a.custid = " & cust)

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

        Dim a As New clsDbo("SELECT cld_Inv_Stk.*, IFNULL((SELECT ttlqty FROM cld_inv_transstockcls WHERE stkid = cld_Inv_Stk.id AND ccid = " & ccid & " and custid = " & cust & " AND invdate = '" & Now.Date.ToString("yyyy-MM-dd") & "'), 0) AS stock , IFNULL((SELECT cost FROM cld_inv_transstockcls WHERE stkid = cld_Inv_Stk.id AND ccid = " & ccid & " AND custid = " & cust & " AND invdate = '" & Now.Date.ToString("yyyy-MM-dd") & "'), 0) AS coststk FROM cld_Inv_Stk WHERE id = " & id & " and custid = " & cust)

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function
    Public Function SelectData_Inv_Stkcls(ByVal id As Integer, ByVal ccid As Integer, ByVal cust As Integer, ByVal invdate As Date) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""

        Dim a As New clsDbo("SELECT cld_Inv_Stk.*, IFNULL((SELECT ttlqty FROM cld_inv_transstockcls WHERE stkid = cld_Inv_Stk.id AND ccid = " & ccid & " AND invdate = '" & invdate.Date.ToString("yyyy-MM-dd") & "' and custid = " & cust & "), 0) AS stock , IFNULL((SELECT cost FROM cld_inv_transstockcls WHERE stkid = cld_Inv_Stk.id AND ccid = " & ccid & " AND invdate = '" & invdate.Date.ToString("yyyy-MM-dd") & "' AND custid = " & cust & " ), 0) AS coststk FROM cld_Inv_Stk WHERE id = " & id & " and custid = " & cust)

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function
    Public Function SelectDataset_StkAll1(ByVal custid As Integer, Optional ByVal ccid As Integer = 0) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""
        If ccid <> 0 Then
            strFilter = "and ccid = " & ccid
        End If
        'Dim a As New clsDbo("SELECT * from cld_inv_stk where custid = " & custid & " order by name")
        Dim a As New clsDbo("SELECT DISTINCT stkid,stkname FROM `cld_inv_transstockcls` WHERE custid = " & custid & " " & strFilter & " AND invdate = CURDATE() AND ttlqty>0 ORDER BY stkname")

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function
    Public Function SelectDataset_StkAll(ByVal custid As Integer, Optional ByVal ccid As Integer = 0) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""
        If ccid <> 0 Then
            strFilter = "and ccid = " & ccid
        End If
        Dim a As New clsDbo("SELECT * from cld_inv_stk where custid = " & custid & " order by name")
        'Dim a As New clsDbo("SELECT DISTINCT stkid,stkname FROM `cld_inv_transstockcls` WHERE custid = " & custid & " " & strFilter & " AND invdate = CURDATE() AND ttlqty>0 ORDER BY stkname")

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function
    Public Function SelectDataset_StkCC(ByVal custid As Integer, ByVal ccid As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""

        Dim a As New clsDbo("SELECT id, name , CASE WHEN Batch = 1 THEN 'B' ELSE '' END AS Batch from cld_inv_stk  where cld_inv_stk.custid = " & custid & " 
                    And id  In  (select stkid from cld_inv_transstockcls where invdate = CURDATE() and ccid = " & ccid & " and ttlqty>1) 
                    order by name")

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function
    Public Function cekcode(ByVal id As Integer, ByVal code As String) As Boolean
        Dim usedcode As Boolean = False
        Try


            Dim c_cmd As New clsDbo("")
            Dim id2 As Integer = 0





            'If id <> 0 Then
            c_cmd.resetCmd("")
                id2 = c_cmd.ExecuteScalar("Select ID from cld_inv_stk where Number = '" & code & "'", 2)
            If id <> id2 And id2 <> 0 Then
                usedcode = True
            End If

            'End If


            c_cmd = Nothing

        Catch ex As Exception

            errLog(Err.Description, "Cls_IStk:cekcode")
            Err.Clear()
        End Try
        Return usedcode
    End Function
    Public Function cekname(ByVal id As Integer, ByVal name As String) As Boolean
        Dim usedname As Boolean = False
        Try


            Dim c_cmd As New clsDbo("")
            Dim id2 As Integer = 0





            'If id <> 0 Then
            c_cmd.resetCmd("")
                id2 = c_cmd.ExecuteScalar("Select ID from cld_inv_stk where name = '" & name & "'", 2)
            If id <> id2 And id2 <> 0 Then
                usedname = True
            End If

            'End If


            c_cmd = Nothing

        Catch ex As Exception

            errLog(Err.Description, "Cls_IStk:cekname")
            Err.Clear()
        End Try
        Return usedname
    End Function

    Public Sub insertStkreplace(ByVal stkid As Integer, ByVal custid As Integer, ByVal stkbefore As Integer, ByVal username As String, ByVal invdate As Date)
        Dim str As String = ""
        Dim str2 As String = ""
        Try


            Dim c_cmd As New clsDbo("")
            'Dim nstkid As Integer = 0


            If custid = 0 Then
                custid = c_cmd.ExecuteScalar("SELECT id FROM `cld_cust` LIMIT 1", 0)
            Else
                If custid <> c_cmd.ExecuteScalar("SELECT id FROM `cld_cust` LIMIT 1", 0) Then
                    errLog("wrong custid" & " clsInventory_stk:insertlimitStk", custid.ToString)
                    custid = c_cmd.ExecuteScalar("SELECT id FROM `cld_cust` LIMIT 1", 0)
                End If
            End If

            'If My.Settings.stk_cc = 1 Then

            str = "insert into cld_inv_stk_replace (CustID,Transdate,username,afterstk,beforestk) values (" & custid & ",'" & invdate.ToString("yyyy-MM-dd 00:00:00") & "','" & username & "'," & stkid & ", " & stkbefore & ")"
            str2 = "UPDATE `cld_inv_rcpdtl` SET kodeid = " & stkid & " WHERE kodeid = " & stkbefore & ""


            c_cmd.resetCmd("")
            c_cmd.executeNonQuery(str)
            c_cmd.resetCmd("")
            c_cmd.executeNonQuery(str2)
            c_cmd = Nothing

        Catch ex As Exception

            errLog(Err.Description, str & " clsInventory_stk:insertStkreplace")
            Err.Clear()
        End Try
    End Sub


    Public Sub insertfcost_Stk(ByVal id As Integer, ByVal custid As Integer, ByVal cost As Double, ByVal username As String, ByVal indate As DateTime)
        Try

            Dim str As String
            Dim c_cmd As New clsDbo("")
            If c_cmd.ExecuteScalar("select count(stkID) from cld_inv_fixcost where stkID = " & id & " and custid = " & custid & "") = 0 Then
                str = "insert into cld_inv_fixcost values (" & id & ", " & custid & "," & cost & ", '" & username & "','" & indate.ToString("yyyy-MM-dd HH:mm:ss") & "')"

            Else
                insertfcost_H(id, custid)
                str = "Update cld_inv_fixcost set cost = " & cost & ", user = '" & username & "', indate = '" & indate.ToString("yyyy-MM-dd HH:mm:ss") & "' where stkID = " & id & " and custID = " & custid & ""


            End If

            c_cmd.resetCmd("")
            c_cmd.executeNonQuery(str)


            c_cmd = Nothing

        Catch ex As Exception

            errLog(Err.Description, "Cls_IStk:insertfcost_Stk")
            Err.Clear()
        End Try
    End Sub
    Public Sub insertfcost_H(ByVal id As Integer, ByVal custid As Integer)
        Try

            Dim str As String = ""
            Dim c_cmd As New clsDbo("")
            If c_cmd.ExecuteScalar("select count(ID) from cld_inv_fixcost_h where stkID = " & id & " and custid = " & custid & "") = 0 Then


                str = "INSERT INTO `cld_inv_fixcost_h`
SELECT  * FROM `cld_inv_fixcost` where stkID = " & id & " and custID = " & custid & ""


            End If

            c_cmd.resetCmd("")
            c_cmd.executeNonQuery(str)


            c_cmd = Nothing

        Catch ex As Exception

            errLog(Err.Description, "Cls_IStk:insertfcost_H")
            Err.Clear()
        End Try
    End Sub
End Class
