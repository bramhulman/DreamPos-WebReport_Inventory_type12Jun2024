Public Class clsInventory_Cc

    Public Function SelectDataset_Icc(ByVal custid As Integer, ByVal usr As String, ByVal modpriv As Integer, ByVal itipe As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""

        If itipe = 0 Then

            If modpriv <> 0 Then

                strFilter = "SELECT cld_inv_cc.*, cld_cust.nama as custname, IFNULL(cld_outlet.nama,'Warehouse') as OutletName " &
                            "from cld_inv_cc inner join cld_cust On cld_cust.id = cld_inv_cc.custid left Join cld_outlet ON cld_inv_cc.Olid = cld_outlet.id and cld_inv_cc.custid = cld_outlet.custid " &
                            "where cld_inv_cc.custid = " & custid & " and olid IN (SELECT Store FROM cld_userxstore WHERE USER = '" & usr & "')"
            Else
                strFilter = "SELECT cld_inv_cc.*, cld_cust.nama as custname, IFNULL(cld_outlet.nama,'Warehouse') as OutletName " &
                            "from cld_inv_cc inner join cld_cust On cld_cust.id = cld_inv_cc.custid left Join cld_outlet ON cld_inv_cc.Olid = cld_outlet.id and cld_inv_cc.custid = cld_outlet.custid " &
                            "where cld_inv_cc.custid = " & custid

            End If

        ElseIf itipe = 1 Then


            If modpriv <> 0 Then

                strFilter = "SELECT cld_inv_cc.*, cld_cust.nama as custname, IFNULL(cld_outlet.nama,'Warehouse') as OutletName " &
                            "from cld_inv_cc inner join cld_cust On cld_cust.id = cld_inv_cc.custid left Join cld_outlet ON cld_inv_cc.Olid = cld_outlet.id and cld_inv_cc.custid = cld_outlet.custid " &
                            "where cld_inv_cc.olid <> 0 and cld_inv_cc.custid = " & custid & " and olid <> 0 " 'and olid NOT IN (SELECT Store FROM cld_userxstore WHERE USER = '" & usr & "')"
            Else
                strFilter = "SELECT cld_inv_cc.*, cld_cust.nama as custname, IFNULL(cld_outlet.nama,'Warehouse') as OutletName " &
                            "from cld_inv_cc inner join cld_cust On cld_cust.id = cld_inv_cc.custid left Join cld_outlet ON cld_inv_cc.Olid = cld_outlet.id and cld_inv_cc.custid = cld_outlet.custid " &
                            "where cld_inv_cc.olid <> 0 and cld_inv_cc.custid = " & custid & " and olid <> 0 "

            End If
        ElseIf itipe = 2 Then


            If modpriv And usrModulepriv.moduleuser = usrModulepriv.moduleuser Then

                strFilter = "SELECT cld_inv_cc.*, cld_cust.nama as custname, IFNULL(cld_outlet.nama,'Warehouse') as OutletName " &
                            "from cld_inv_cc inner join cld_cust On cld_cust.id = cld_inv_cc.custid left Join cld_outlet ON cld_inv_cc.Olid = cld_outlet.id and cld_inv_cc.custid = cld_outlet.custid " &
                            "where cld_inv_cc.olid = 0 and cld_inv_cc.custid = " & custid & " and olid IN (SELECT Store FROM cld_userxstore WHERE USER = '" & usr & "')"
            Else
                strFilter = "SELECT cld_inv_cc.*, cld_cust.nama as custname, IFNULL(cld_outlet.nama,'Warehouse') as OutletName " &
                            "from cld_inv_cc inner join cld_cust On cld_cust.id = cld_inv_cc.custid left Join cld_outlet ON cld_inv_cc.Olid = cld_outlet.id and cld_inv_cc.custid = cld_outlet.custid " &
                            "where cld_inv_cc.olid = 0 and cld_inv_cc.custid = " & custid

            End If


        End If


        Dim a As New clsDbo(strFilter)

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function

    Public Function SelectDataset_IccOnly(ByVal custid As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""

        Try

            If modpriv <> 0 Then

                strFilter = "SELECT * from cld_inv_cc " &
                            "where olid <> 0 and custid = " & custid & " and olid IN (SELECT Store FROM cld_userxstore WHERE USER = '" & usr & "')"
            Else
                strFilter = "SELECT * from cld_inv_cc " &
                            "where olid <> 0 and custid = " & custid
            End If


            Dim a As New clsDbo(strFilter)


            ds = a.selectDataset

            a.dispose()
            a = Nothing

        Catch ex As Exception
            errLog(Err.Description, "clsInventory_Cc:SelectDataset_IccOnly")
            Err.Clear()
        End Try

        Return ds

    End Function
    Public Function SelectDataset_IWare(ByVal custid As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""

        Try

            strFilter = "Select * from cld_inv_cc where olid = 0 and custid = " & custid

            Dim a As New clsDbo(strFilter)

            ds = a.selectDataset

            a.dispose()
            a = Nothing


        Catch ex As Exception
            errLog(Err.Description, "clsInventory_Cc:SelectDataset_IWare")
            Err.Clear()
        End Try

        Return ds

    End Function
    Public Sub insertSupplierX(ByVal cc As Integer, ByVal Supplier As List(Of String))
        Try

            Dim str As String
            Dim c_cmd As New Cls_command

            Dim id_cc As Integer = 0




            For i As Integer = 0 To Supplier.Count - 1
                id_cc = c_cmd.ExecuteScalar("SELECT COUNT(supplier)FROM cld_ccxsupplier WHERE cc='" & cc & "' AND store = " & Supplier(i) & "", 0)
                If id_cc = 0 Then
                    str = "insert into cld_ccxsupplier(cc, Supplier) values ('" & cc & "'," & Supplier(i) & ")"
                    c_cmd.executeNonQuery(str)
                End If



            Next

        Catch ex As Exception

            errLog(Err.Description, "clsOutlet:insertSupplierX")
            Err.Clear()
        End Try
    End Sub

End Class
