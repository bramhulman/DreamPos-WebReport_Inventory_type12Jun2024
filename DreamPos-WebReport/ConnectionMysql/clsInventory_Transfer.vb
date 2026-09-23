Imports DocumentFormat.OpenXml.VariantTypes

Public Class clsInventory_Transfer



    'perubahan 12 oct 2017, ini untuk detail inv transfer
    Public Function SelectDataTable_invtrans_Transfer(ByVal linkid As String, ByVal custid As Integer) As DataTable
        Dim ds As New DataTable
        Dim strFilter As String = ""


        Dim a As New clsDbo("SELECT A.*, IFNULL(B.ttlQty, 0) AS Stock,c.ccfrom  FROM cld_inv_trans A INNER JOIN cld_invlk_trf c ON a.CustID = c.CustID AND a.Linkid = c.id " &
                            "LEFT JOIN cld_inv_transstockcls B ON B.stkId = A.Stk_Id  AND B.ccid = c.ccFrom AND b.custId = c.CustID " &
                            "WHERE A.linkid = '" & linkid & "' and A.type = 1 and A.custid = " & custid & " and B.invdate = '" & Date.Now.ToString("yyyy-MM-dd 00:00:00") & "'")
        'Dim a As New clsDbo("SELECT A.*, IFNULL(B.ttlQty, 0) AS Stock  FROM cld_inv_trans A INNER JOIN cld_invlk_trf c ON a.CustID = c.CustID AND a.Linkid = c.id " &
        '                    "LEFT JOIN cld_inv_transstock B ON B.stkId = A.Stk_Id  AND B.ccid = c.ccFrom AND b.custId = c.CustID " &
        '                    "WHERE A.linkid = '" & linkid & "' and A.type = 1 and A.custid = " & custid & "")
        ds = a.selectDatatable

        a.dispose()
        a = Nothing

        Return ds

    End Function


    Public Function SelectDataset_longId(ByVal custid As Integer) As String
        Dim ds As New DataSet

        Dim strFilter As String = ""


        Dim a As New clsDbo("SELECT  * from cld_invlk_trf where MONTH(createddate) = " & Date.Now.Month & " and custid = " & custid & " AND LEFT(id,2)<>'TW' order by id desc")

        ds = a.selectDataset

        If ds.Tables(0).Rows.Count > 0 Then
            strFilter = ds.Tables(0).Rows(0).Item("Id")
        End If

        a.dispose()
        a = Nothing

        Return strFilter

    End Function

    Public Function SelectDataset_longIdTW(ByVal custid As Integer) As String
        Dim ds As New DataSet

        Dim strFilter As String = ""


        Dim a As New clsDbo("SELECT  * from cld_invlk_trf where MONTH(createddate) = " & Date.Now.Month & " and custid = " & custid & " AND LEFT(id,2)='TW' order by id desc")

        ds = a.selectDataset

        If ds.Tables(0).Rows.Count > 0 Then
            strFilter = ds.Tables(0).Rows(0).Item("Id")
        End If

        a.dispose()
        a = Nothing

        Return strFilter

    End Function

    Public Function SelectDataset_invtrf(ByVal bydate As Date, ByVal custid As Integer) As DataSet
        Return SelectDataset_invtrf(bydate, bydate, custid, 0)
    End Function

    Public Function SelectDataset_invtrf(ByVal bydate As Date, ByVal bydateto As Date, ByVal custid As Integer, Optional ByVal ccFrom As Integer = 0) As DataSet
        Dim ds As New DataSet
        Dim strSQL As String = "SELECT cld_invlk_trf.*, DATE_FORMAT(cld_invlk_trf.CreatedDate,'%d-%b-%Y') AS CreateD, " &
                               "IFNULL(cld_inv_cc.Name, 'Warehouse') AS ccname, " &
                               "IFNULL(GROUP_CONCAT(DISTINCT cld_inv_trans.cc_name ORDER BY cld_inv_trans.cc_name SEPARATOR ', '), '-') AS ccToName, " &
                               "COUNT(DISTINCT cld_inv_trans.stk_id) AS totalItems, " &
                               "IFNULL(SUM(cld_inv_trans.ttlStock), 0) AS totalQty " &
                               "FROM cld_invlk_trf " &
                               "LEFT JOIN cld_inv_cc ON cld_invlk_trf.ccFrom = cld_inv_cc.ID " &
                               "LEFT JOIN cld_inv_trans ON cld_inv_trans.Linkid = cld_invlk_trf.ID AND cld_inv_trans.custid = cld_invlk_trf.custid AND cld_inv_trans.type = 1 " &
                               "WHERE cld_invlk_trf.custid = " & custid & " " &
                               "AND cld_invlk_trf.CreatedDate >= '" & bydate.ToString("yyyy-MM-dd") & "' " &
                               "AND cld_invlk_trf.CreatedDate <= '" & bydateto.ToString("yyyy-MM-dd") & "' "
        If ccFrom > 0 Then
            strSQL &= "AND cld_invlk_trf.ccFrom = " & ccFrom & " "
        End If
        strSQL &= "GROUP BY cld_invlk_trf.ID, cld_invlk_trf.CreatedDate, cld_inv_cc.Name " &
                  "ORDER BY cld_invlk_trf.CreatedDate DESC, cld_invlk_trf.ID DESC"

        Dim a As New clsDbo(strSQL)
        ds = a.selectDataset
        a.dispose()
        a = Nothing

        Return ds
    End Function

    Public Function SelectDataset_invtrfSearch(ByVal name As String, ByVal bydate As Date, ByVal custid As Integer) As DataSet
        Return SelectDataset_invtrfSearch(name, bydate, bydate, custid, 0)
    End Function

    Public Function SelectDataset_invtrfSearch(ByVal name As String, ByVal bydate As Date, ByVal bydateto As Date, ByVal custid As Integer, Optional ByVal ccFrom As Integer = 0) As DataSet
        Dim ds As New DataSet
        Dim safeName As String = If(name IsNot Nothing, name.Replace("'", "''"), "")
        Dim strSQL As String = "SELECT cld_invlk_trf.*, DATE_FORMAT(cld_invlk_trf.CreatedDate,'%d-%b-%Y') AS CreateD, " &
                               "IFNULL(cld_inv_cc.Name, 'Warehouse') AS ccname, " &
                               "IFNULL(GROUP_CONCAT(DISTINCT cld_inv_trans.cc_name ORDER BY cld_inv_trans.cc_name SEPARATOR ', '), '-') AS ccToName, " &
                               "COUNT(DISTINCT cld_inv_trans.stk_id) AS totalItems, " &
                               "IFNULL(SUM(cld_inv_trans.ttlStock), 0) AS totalQty " &
                               "FROM cld_invlk_trf " &
                               "LEFT JOIN cld_inv_cc ON cld_invlk_trf.ccFrom = cld_inv_cc.ID " &
                               "LEFT JOIN cld_inv_trans ON cld_inv_trans.Linkid = cld_invlk_trf.ID AND cld_inv_trans.custid = cld_invlk_trf.custid AND cld_inv_trans.type = 1 " &
                               "WHERE cld_invlk_trf.custid = " & custid & " " &
                               "AND cld_invlk_trf.CreatedDate >= '" & bydate.ToString("yyyy-MM-dd") & "' " &
                               "AND cld_invlk_trf.CreatedDate <= '" & bydateto.ToString("yyyy-MM-dd") & "' "
        If ccFrom > 0 Then
            strSQL &= "AND cld_invlk_trf.ccFrom = " & ccFrom & " "
        End If
        If Not String.IsNullOrEmpty(safeName) Then
            strSQL &= "AND (cld_inv_cc.name LIKE '%" & safeName & "%' OR cld_invlk_trf.id LIKE '%" & safeName & "%' OR cld_inv_trans.cc_name LIKE '%" & safeName & "%') "
        End If
        strSQL &= "GROUP BY cld_invlk_trf.ID, cld_invlk_trf.CreatedDate, cld_inv_cc.Name " &
                  "ORDER BY cld_invlk_trf.CreatedDate DESC, cld_invlk_trf.ID DESC"

        Dim a As New clsDbo(strSQL)
        ds = a.selectDataset
        a.dispose()
        a = Nothing

        Return ds
    End Function


    Public Function trf_date(ByVal ID As String) As Integer
        'Dim ds As New DataSet

        Dim edtable As Integer = 0

        Try
            Dim a As New clsDbo("")
            edtable = a.ExecuteScalar("SELECT count(id) from cld_invlk_trf where  cld_invlk_trf.id = '" & ID & "' and cld_invlk_trf.CreatedDate >= '" & Now.AddDays(-7).ToString("yyyy-MM-dd") & "'", 0)


            a.dispose()
            a = Nothing

        Catch ex As Exception
            errLog(Err.Description, "clsInventory_Warehouse:u1_invorderrcv")
            Err.Clear()
        End Try

        Return edtable

    End Function

End Class
