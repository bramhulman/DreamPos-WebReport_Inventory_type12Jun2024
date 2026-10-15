Public Class Cls_IRcp
    Public Function SelectDataRcp(ByVal sCode As String) As DataSet
        Dim a As New clsDbo("SELECT ID FROM cld_inv_rcp WHERE code = @code")
        Try
            a.addParam(sCode, "@code", MySql.Data.MySqlClient.MySqlDbType.VarChar)
            Return a.selectDataset(True)
        Finally
            a.dispose()
        End Try
    End Function
    Public Function SelectDataset_rcp(ByVal custid As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""

        'Dim a As New clsDbo("SELECT rcp.*, rcc.cost cost2, cc.name AS ccname 
        '    FROM cld_inv_cc cc 
        '         LEFT JOIN cld_inv_rcp_cc rcc ON rcc.custid = cc.custid  AND rcc.ccid = cc.id
        '         LEFT JOIN cld_inv_rcp rcp ON rcp.id = rcc.rcpid AND rcp.custid = rcc.custid
        '    where rcp.custid = " & custid & " order by rcp.code ")

        Dim a As New clsDbo("SELECT * FROM cld_inv_rcp rcp 
                 
            where rcp.custid = " & custid & " order by rcp.code ")

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function
    Public Function SelectDataset_sXrcp(ByVal custid As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""

        'Dim a As New clsDbo("SELECT rcp.*, rcc.cost cost2, cc.name AS ccname 
        '    FROM cld_inv_cc cc 
        '         LEFT JOIN cld_inv_rcp_cc rcc ON rcc.custid = cc.custid  AND rcc.ccid = cc.id
        '         LEFT JOIN cld_inv_rcp rcp ON rcp.id = rcc.rcpid AND rcp.custid = rcc.custid
        '    where rcp.custid = " & custid & " order by rcp.code ")

        Dim a As New clsDbo("SELECT * FROM cld_inv_rcp rcp 
                 JOIN `cld_inv_stkxrcp` sxr ON rcp.`ID` = sxr.`rcpid`
            where rcp.custid = " & custid & " order by rcp.code ")

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function
    Public Function Selectds_sXrcpddl(ByVal custid As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""

        'Dim a As New clsDbo("SELECT rcp.*, rcc.cost cost2, cc.name AS ccname 
        '    FROM cld_inv_cc cc 
        '         LEFT JOIN cld_inv_rcp_cc rcc ON rcc.custid = cc.custid  AND rcc.ccid = cc.id
        '         LEFT JOIN cld_inv_rcp rcp ON rcp.id = rcc.rcpid AND rcp.custid = rcc.custid
        '    where rcp.custid = " & custid & " order by rcp.code ")

        Dim a As New clsDbo("SELECT id,name,stkid FROM cld_inv_rcp rcp 
                 JOIN `cld_inv_stkxrcp` sxr ON rcp.`ID` = sxr.`rcpid`
            where rcp.custid = " & custid & " order by rcp.code ")

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function
    Public Function Selectds_rcpexport(ByVal custid As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""

        strFilter = "SELECT CODE,NAME,remarks FROM cld_inv_rcp rcp where rcp.custid = " & custid & " order by rcp.code "
        Dim a As New clsDbo(strFilter)

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function
    Public Function SelectDataset_rcpdtlExport(ByVal custid As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""


        Dim a As New clsDbo("SELECT CODE ResepNo, cld_inv_rcp.Name AS ResepNama,cld_inv_stk.Number KodeBarang,cld_inv_stk.`Name` NamaBarang,qty,lossf lossFactor,
                          CASE WHEN tipe = 1 THEN  cld_inv_stk.Unit3 ELSE 'Pcs' END AS unit,remarks FROM cld_inv_rcpdtl 
                          INNER JOIN cld_inv_rcp ON cld_inv_rcp.ID = cld_inv_rcpdtl.`rcpid`   
                           LEFT JOIN cld_inv_stk ON cld_inv_stk.ID = cld_inv_rcpdtl.kodeId 
                                            WHERE cld_inv_rcpdtl.custid = " & custid & " ORDER BY CODE")

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function

    Public Function SelectDataset_rcpSearch(ByVal name As String, ByVal custid As Integer) As DataSet
        Dim a As New clsDbo("SELECT * FROM cld_inv_rcp rcp WHERE rcp.custid = @custid " &
                           "AND (rcp.code LIKE @search OR rcp.name LIKE @search) ORDER BY rcp.code")
        Try
            a.addParam(custid, "@custid", MySql.Data.MySqlClient.MySqlDbType.Int32)
            a.addParam("%" & name & "%", "@search", MySql.Data.MySqlClient.MySqlDbType.VarChar)
            Return a.selectDataset(True)
        Finally
            a.dispose()
        End Try
    End Function

    Public Function Selectdt_rcpsearchdt(ByVal dta As DataTable, ByVal custid As Integer, ByVal name As String) As DataSet
        Dim excludedIds As New System.Collections.Generic.List(Of Integer)()
        If dta IsNot Nothing Then
            For Each row As DataRow In dta.Rows
                If row.RowState = DataRowState.Deleted OrElse row.IsNull("rcpid") Then Continue For
                Dim recipeId As Integer
                If Integer.TryParse(Convert.ToString(row("rcpid"), Globalization.CultureInfo.InvariantCulture), recipeId) AndAlso recipeId > 0 Then
                    If Not excludedIds.Contains(recipeId) Then excludedIds.Add(recipeId)
                End If
            Next
        End If
        Dim placeholders As New System.Collections.Generic.List(Of String)()
        For i As Integer = 0 To excludedIds.Count - 1
            placeholders.Add("@excluded" & i.ToString(Globalization.CultureInfo.InvariantCulture))
        Next
        Dim sql As String = "SELECT * FROM cld_inv_rcp rcp WHERE rcp.custid = @custid "
        If placeholders.Count > 0 Then sql &= "AND rcp.id NOT IN (" & String.Join(",", placeholders.ToArray()) & ") "
        sql &= "AND (rcp.code LIKE @search OR rcp.name LIKE @search) ORDER BY rcp.name"
        Dim a As New clsDbo(sql)
        Try
            a.addParam(custid, "@custid", MySql.Data.MySqlClient.MySqlDbType.Int32)
            a.addParam("%" & name & "%", "@search", MySql.Data.MySqlClient.MySqlDbType.VarChar)
            For i As Integer = 0 To excludedIds.Count - 1
                a.addParam(excludedIds(i), placeholders(i), MySql.Data.MySqlClient.MySqlDbType.Int32)
            Next
            Return a.selectDataset(True)
        Finally
            a.dispose()
        End Try
    End Function

    Public Function SelectDataset_rcpEdit(ByVal Id As Integer, ByVal custid As Integer) As DataSet
        Dim a As New clsDbo("SELECT * FROM cld_inv_rcp WHERE id = @id AND custid = @custid")
        Try
            a.addParam(Id, "@id", MySql.Data.MySqlClient.MySqlDbType.Int32)
            a.addParam(custid, "@custid", MySql.Data.MySqlClient.MySqlDbType.Int32)
            Return a.selectDataset(True)
        Finally
            a.dispose()
        End Try
    End Function
    Public Function SelectDataset_Idrcp(ByVal blink As Integer) As Integer
        Dim ds, dsb As New DataSet
        Dim strFilter As String = ""
        Dim Qty As Integer

        Dim a As New clsDbo("Select * from cld_inv_stk where Blink = " & blink & "")
        Dim b As New clsDbo("Select * from cld_inv_rcpdtl where kodeid = " & blink & "")
        ds = a.selectDataset
        dsb = b.selectDataset
        If ds.Tables(0).Rows.Count > 0 Then
            Qty = 1
        ElseIf dsb.Tables(0).Rows.Count > 0 Then
            Qty = 2
        Else

            Qty = 0
        End If
        a.dispose()
        a = Nothing

        Return Qty

    End Function
    Public Function SelectDataset_rcpCost(ByVal custid As Integer, ByVal code As String, Optional ByVal usr As String = "", Optional ByVal modpriv As Integer = 0) As DataSet
        Dim sql As String = "SELECT rcp.id,rcp.custid,rcp.code,rcp.name,rcc.cost cost2,cc.name AS ccname,cc.id ccid " &
            "FROM cld_inv_rcp rcp " &
            "INNER JOIN cld_inv_rcp_cc rcc ON rcc.custid = rcp.custid AND rcc.rcpid = rcp.id " &
            "LEFT JOIN cld_inv_cc cc ON cc.id = rcc.ccid AND cc.custid = rcc.custid " &
            "LEFT JOIN cld_outlet ON cc.olid = cld_outlet.id AND cc.custid = cld_outlet.custid " &
            "WHERE rcp.custid = @custid AND rcp.code = @code "
        Dim restricted As Boolean = (modpriv And usrModulepriv.moduleuser) = usrModulepriv.moduleuser
        If restricted Then sql &= "AND cc.olid IN (SELECT Store FROM cld_userxstore WHERE USER = @usr) "
        sql &= "ORDER BY rcp.code"
        Dim a As New clsDbo(sql)
        Try
            a.addParam(custid, "@custid", MySql.Data.MySqlClient.MySqlDbType.Int32)
            a.addParam(code, "@code", MySql.Data.MySqlClient.MySqlDbType.VarChar)
            If restricted Then a.addParam(usr, "@usr", MySql.Data.MySqlClient.MySqlDbType.VarChar)
            Return a.selectDataset(True)
        Finally
            a.dispose()
        End Try
    End Function
    Public Function insertRcp(ByVal code As String, ByVal nama As String, ByVal remarks As String, ByVal custid As Integer, ByVal username As String)

        Dim c_cmd As New clsDbo("")
        Dim id As Integer = 0
        Try

            Dim ds As DataSet
            'Dim id As Integer = 0

            ds = SelectDataRcp_wName(nama, code)

            If ds.Tables(0).Rows.Count > 0 Then
                id = ds.Tables(0).Rows(0).Item("id")
            End If


            Dim str As String


            If ds.Tables(0).Rows.Count = 0 Then
                str = "insert into cld_inv_rcp values (" & c_cmd.ExecuteScalar("select max(ID) from cld_inv_rcp") & ", " & custid & ", '" & code & "',   '" & nama & "',0, '" & remarks & "')"

                'Revisi 10 Mei
                insertLogModule(username, "receipe", "Insert", custid)
            Else
                str = "Update cld_inv_rcp set  name = '" & nama & "', code = '" & code & "', remarks = '" & remarks & "' where custid = " & custid & " and id = " & id & ""


                insertLogModule(username, "receipe", "Update", custid)
            End If

            c_cmd.executeNonQuery(str)

        Catch ex As Exception

            errLog(Err.Description, "webservice:insertRcp")
            Err.Clear()
        End Try
        Return id

        c_cmd = Nothing

    End Function
    Public Function SelectDataRcp(ByVal id As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""


        Dim a As New clsDbo("SELECT CODE,NAME,remarks FROM cld_inv_rcp where id = " & id)


        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function


    Public Function SelectDataRcp_wName(ByVal sName As String, ByVal scode As String) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""

        Dim a As New clsDbo("SELECT id,CODE,NAME,remarks FROM cld_inv_rcp where code ='" & scode & "'")

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function
    Public Function SelectDataRcpStk(ByVal scode As String) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""


        Dim a As New clsDbo("SELECT sxr.*,s.number numberstk,s.`Name` namestk,r.code codercp,r.`Name`namercp FROM `cld_inv_stkxrcp` sxr
                            JOIN `cld_inv_stk` s ON s.`ID`=sxr.`stkid`
                            JOIN `cld_inv_rcp` r ON r.`ID`=sxr.`rcpid`
                             WHERE s.`Number` = '" & scode & "'")


        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function



End Class
