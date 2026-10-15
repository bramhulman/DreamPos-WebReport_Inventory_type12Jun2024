Imports MySql.Data.MySqlClient
Imports System.Globalization

Public Class Cls_ICc

    Public Function SelectDataIcc(ByVal id As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""


        Dim a As New clsDbo("SELECT * FROM cld_Inv_cc where id = " & id)


        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function


    Public Function SelectDataset_Icc(ByVal custid As Integer, ByVal usr As String, ByVal modpriv As Integer, ByVal itipe As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""

        If itipe = 0 Then

            If modpriv And usrModulepriv.moduleuser = usrModulepriv.moduleuser Then

                strFilter = "SELECT cld_inv_cc.*, cld_cust.nama as custname, IFNULL(cld_outlet.nama,'Warehouse') as OutletName, IFNULL(cld_own.name,'') ownname, IFNULL(cld_own.markup ,0) markup " &
                            "from cld_inv_cc inner join cld_cust On cld_cust.id = cld_inv_cc.custid left Join cld_outlet ON cld_inv_cc.Olid = cld_outlet.id and cld_inv_cc.custid = cld_outlet.custid
                            Left join cld_own On cld_outlet.ownid = cld_own.id and cld_cust.id = cld_own.custid " &
                            "where cld_inv_cc.custid = " & custid & " and olid IN (SELECT Store FROM cld_userxstore WHERE USER = '" & usr & "') order by name"
            Else
                strFilter = "SELECT cld_inv_cc.*, cld_cust.nama as custname, IFNULL(cld_outlet.nama,'Warehouse') as OutletName, IFNULL(cld_own.name,'') ownname, IFNULL(cld_own.markup ,0) markup " &
                            "from cld_inv_cc inner join cld_cust On cld_cust.id = cld_inv_cc.custid left Join cld_outlet ON cld_inv_cc.Olid = cld_outlet.id and cld_inv_cc.custid = cld_outlet.custid 
                            Left join cld_own On cld_outlet.ownid = cld_own.id and cld_cust.id = cld_own.custid " &
                            "where cld_inv_cc.custid = " & custid & " order by name"

            End If

        ElseIf itipe = 1 Then


            If modpriv And usrModulepriv.moduleuser = usrModulepriv.moduleuser Then

                strFilter = "SELECT cld_inv_cc.*, cld_cust.nama as custname, IFNULL(cld_outlet.nama,'Warehouse') as OutletName, cld_own.name ownname, 0 markup " &
                            "from cld_inv_cc inner join cld_cust On cld_cust.id = cld_inv_cc.custid left Join cld_outlet ON cld_inv_cc.Olid = cld_outlet.id and cld_inv_cc.custid = cld_outlet.custid 
                            inner join cld_own On cld_outlet.ownid = cld_own.id and cld_cust.id = cld_own.custid " &
                            "where cld_inv_cc.olid <> 0 and cld_inv_cc.custid = " & custid & " and olid IN (SELECT Store FROM cld_userxstore WHERE USER = '" & usr & "') order by name"
            Else
                strFilter = "SELECT cld_inv_cc.*, cld_cust.nama as custname, IFNULL(cld_outlet.nama,'Warehouse') as OutletName, cld_own.name ownname, cld_own.markup markup " &
                            "from cld_inv_cc inner join cld_cust On cld_cust.id = cld_inv_cc.custid left Join cld_outlet ON cld_inv_cc.Olid = cld_outlet.id and cld_inv_cc.custid = cld_outlet.custid 
                            inner join cld_own On cld_outlet.ownid = cld_own.id and cld_cust.id = cld_own.custid " &
                            "where cld_inv_cc.olid <> 0 and cld_inv_cc.custid = " & custid & " order by name"

            End If



        End If


        Dim a As New clsDbo(strFilter)

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function

    Public Function SelectDataset_IccSearch(ByVal name As String, ByVal custid As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""


        If modpriv And usrModulepriv.moduleuser = usrModulepriv.moduleuser Then

            strFilter = "SELECT cld_inv_cc.*, cld_cust.nama as custname, IFNULL(cld_outlet.nama,'Warehouse') as OutletName, IFNULL(cld_own.name,'') ownname, IFNULL(cld_own.markup ,0) markup " &
                        "from cld_inv_cc inner join cld_cust On cld_cust.id = cld_inv_cc.custid left Join cld_outlet ON cld_inv_cc.Olid = cld_outlet.id and cld_inv_cc.custid = cld_outlet.custid " &
                        "Left join cld_own On cld_outlet.ownid = cld_own.id and cld_cust.id = cld_own.custid " &
                        "where cld_inv_cc.custid = " & custid & " and  cld_inv_cc.name like '%" & name & "%' " & " and olid IN (SELECT Store FROM cld_userxstore WHERE USER = '" & usr & "') order by name"
        Else
            strFilter = "SELECT cld_inv_cc.*, cld_cust.nama as custname, IFNULL(cld_outlet.nama,'Warehouse') as OutletName, IFNULL(cld_own.name,'') ownname, IFNULL(cld_own.markup ,0) markup " &
                        "from cld_inv_cc inner join cld_cust On cld_cust.id = cld_inv_cc.custid left Join cld_outlet ON cld_inv_cc.Olid = cld_outlet.id and cld_inv_cc.custid = cld_outlet.custid " &
                        "Left join cld_own On cld_outlet.ownid = cld_own.id and cld_cust.id = cld_own.custid " &
                        "where cld_inv_cc.custid = " & custid & " and  cld_inv_cc.name like '%" & name & "%' order by name"

        End If


        'Dim a As New clsDbo("SELECT cld_inv_cc.*, cld_cust.nama as custname, cld_outlet.nama as OutletName from cld_inv_cc inner join cld_cust On cld_cust.id = cld_inv_cc.custid Inner Join cld_outlet ON cld_inv_cc.olid = cld_outlet.id where cld_inv_cc.custid = " & custid & "  and  cld_inv_cc.name like '%" & name & "%' ")
        Dim a As New clsDbo(strFilter)


        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function

    Public Function Selectdt_CCsearch(ByVal dta As DataTable, ByVal custid As Integer, ByVal name As String) As DataTable
        Return SelectAvailableCostCentres(dta, custid, name, "", 0, 0).Tables(0)
    End Function

    Public Function Selectdt_CCsearchdt(ByVal dta As DataTable, ByVal custid As Integer, ByVal name As String, ByVal usr As String, ByVal modpriv As Integer, ByVal itipe As Integer) As DataSet
        Return SelectAvailableCostCentres(dta, custid, name, usr, modpriv, itipe)
    End Function

    Private Function SelectAvailableCostCentres(ByVal dta As DataTable, ByVal custid As Integer, ByVal name As String, ByVal usr As String, ByVal modpriv As Integer, ByVal itipe As Integer) As DataSet
        If itipe <> 0 AndAlso itipe <> 1 Then Throw New ArgumentOutOfRangeException("itipe")
        Dim ds As New DataSet
        Try
            Using conn As New MySqlConnection(My.Settings.ServerStr)
                Using cmd As New MySqlCommand()
                    cmd.Connection = conn
                    cmd.CommandTimeout = clsQueryGuard.DefaultTimeoutSeconds
                    Dim conditions As New List(Of String) From {
                        "cc.custid = @custid",
                        "cc.name LIKE @name"
                    }
                    cmd.Parameters.AddWithValue("@custid", custid)
                    cmd.Parameters.AddWithValue("@name", "%" & If(name, "") & "%")

                    Dim excludedIds As New HashSet(Of Integer)
                    Dim excludedParameters As New List(Of String)
                    If dta IsNot Nothing AndAlso dta.Rows.Count > 0 Then
                        If Not dta.Columns.Contains("ccid") Then Throw New ArgumentException("Cost centre data must include ccid.")
                        For Each row As DataRow In dta.Rows
                            If row.RowState = DataRowState.Deleted OrElse row.IsNull("ccid") Then Continue For
                            Dim ccId As Integer
                            If Integer.TryParse(Convert.ToString(row("ccid"), CultureInfo.InvariantCulture),
                                                NumberStyles.Integer, CultureInfo.InvariantCulture, ccId) AndAlso
                               ccId > 0 AndAlso excludedIds.Add(ccId) Then
                                Dim parameterName As String = "@excludedCc" & excludedParameters.Count.ToString(CultureInfo.InvariantCulture)
                                excludedParameters.Add(parameterName)
                                cmd.Parameters.AddWithValue(parameterName, ccId)
                            End If
                        Next
                    End If
                    If excludedParameters.Count > 0 Then
                        conditions.Add("cc.id NOT IN (" & String.Join(",", excludedParameters.ToArray()) & ")")
                    End If
                    If itipe = 1 Then conditions.Add("cc.olid <> 0")
                    If (modpriv And usrModulepriv.moduleuser) = usrModulepriv.moduleuser Then
                        conditions.Add("cc.olid IN (SELECT Store FROM cld_userxstore WHERE `User` = @user)")
                        cmd.Parameters.AddWithValue("@user", If(usr, ""))
                    End If

                    ' Only fixed SQL fragments and parameter names are joined here.
                    cmd.CommandText = "SELECT cc.id, cc.custid, cc.name AS ccname FROM cld_inv_cc AS cc " &
                                      "INNER JOIN cld_cust AS cust ON cust.id = cc.custid WHERE " &
                                      String.Join(" AND ", conditions.ToArray()) & " ORDER BY cc.name"
                    Using adapter As New MySqlDataAdapter(cmd)
                        adapter.Fill(ds)
                    End Using
                End Using
            End Using
        Catch ex As Exception
            errLog(ex.Message, "Cls_ICc:SelectAvailableCostCentres")
            Throw
        End Try
        Return ds
    End Function

    Public Function SelectDataset_Icc_SelectionReport(ByVal custid As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""

        If modpriv And usrModulepriv.moduleuser = usrModulepriv.moduleuser Then

            strFilter = "SELECT '0' AS ID, 'All Cc' AS Nama UNION ALL " &
                        "SELECT ID , NAME FROM cld_inv_cc " &
                        "where cld_inv_cc.custid = " & custid & " and olid IN (SELECT Store FROM cld_userxstore WHERE USER = '" & usr & "')"
        Else
            strFilter = "SELECT '0' AS ID, 'All Store' AS Nama UNION ALL " & _
                        "SELECT ID , NAME FROM cld_inv_cc " & _
                        "where cld_inv_cc.custid = " & custid

        End If


        Dim a As New clsDbo(strFilter)


        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function


    Public Function SelectDataset_IccFromTo(ByVal id As Integer, ByVal custid As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""

        Dim a As New clsDbo("SELECT cld_inv_cc.*, cld_cust.nama as custname from cld_inv_cc inner join cld_cust On cld_cust.id = cld_inv_cc.custid  where cld_inv_cc.custid = " & custid & " and  cld_inv_cc.id <> " & id & "")

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function
  

    Public Function SelectDataset_IccEdit(ByVal Id As Integer, ByVal custid As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""


        Dim a As New clsDbo("SELECT * from cld_inv_cc where id = " & Id & " and custid = " & custid & "")

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function

    'tambahan tgl 29 agust 2017

    Public Function SelectDataset_IWare(ByVal custid As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""

        strFilter = "Select * from cld_inv_cc where olid = 0 and custid = " & custid

        Dim a As New clsDbo(strFilter)

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function


    Public Function SelectDataset_IccOnly(ByVal custid As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""

        If modpriv And usrModulepriv.moduleuser = usrModulepriv.moduleuser Then

            strFilter = "SELECT * from cld_inv_cc " &
                        "where olid <> 0 and custid = " & custid & " and olid IN (SELECT Store FROM cld_userxstore WHERE USER = '" & usr & "')"
        Else
            strFilter = "SELECT * from cld_inv_cc " & _
                        "where olid <> 0 and custid = " & custid
        End If


        Dim a As New clsDbo(strFilter)


        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function

End Class
