Public Class clsInventory_Supplier

    Public Function SelectDataSupplier(ByVal id As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""


        Dim a As New clsDbo("SELECT Id, Name FROM cld_Inv_Supplier where id = " & id)


        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function


    Public Function SelectDataSupplier_wName(ByVal sName As String) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""

        Dim a As New clsDbo("SELECT Id, Name FROM cld_Inv_Supplier where name = '" & sName & "'")

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function


    Public Sub insertSupplier(ByVal id As Integer, ByVal custid As Integer, ByVal nama As String, ByVal email As String, ByVal Address As String, ByVal phone As String, ByVal City As String, ByVal State As String, ByVal Pcode As String, ByVal Notes As String, ByVal username As String, Optional ByVal itipe As Integer = 0)

        Dim c_cmd As New clsDbo("")
        Try

            Dim ds As DataSet

            If itipe = 0 Then
                ds = SelectDataSupplier(id)
            Else
                ds = SelectDataSupplier_wName(nama)

                If ds.Tables(0).Rows.Count > 0 Then
                    id = ds.Tables(0).Rows(0).Item("id")
                End If

            End If


            Dim str As String

            If ds.Tables(0).Rows.Count = 0 Then
                str = "insert into cld_Inv_Supplier values (" & c_cmd.ExecuteScalar("select max(ID) from cld_inv_Supplier") & ", " & custid & ", '" & nama & "',  '" & email & "', '" & Address & "', '" & phone & "', '" & City & "', '" & State & "', '" & Pcode & "', '" & Notes & "')"

                'Revisi 10 Mei
                insertLogModule(username, "InvSupplier", "Insert", custid)
            Else
                str = "Update cld_inv_Supplier set  name = '" & nama & "', email = '" & email & "',  Address= '" & Address & "', phone = '" & phone & "', City = '" & City & "', state = '" & State & "',  Pcode ='" & Pcode & "', Notes = '" & Notes & "' where id = " & id & ""


                insertLogModule(username, "InvSupplier", "Update", custid)
            End If

            c_cmd.executeNonQuery(str)

        Catch ex As Exception

            errLog(Err.Description, "webservice:insertInvSupplier")
            Err.Clear()
        End Try


        c_cmd = Nothing

    End Sub


    Public Function SelectDataTable_SupplierExport(ByVal custid As Integer) As DataTable
        Dim ds As New DataTable
        Dim strFilter As String = ""

        Dim a As New clsDbo("SELECT cld_inv_Supplier.Name, cld_inv_Supplier.Email, cld_inv_Supplier.Address, cld_inv_Supplier.Phone, cld_inv_Supplier.City, cld_inv_Supplier.State, cld_inv_Supplier.Pcode From cld_inv_Supplier where cld_inv_Supplier.custid = " & custid & " ")

        ds = a.selectDatatable

        a.dispose()
        a = Nothing

        Return ds

    End Function
    Public Function SelectDataset_Supplier(ByVal custid As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""


        Dim a As New clsDbo("SELECT cld_inv_Supplier.*, cld_cust.nama as custname from cld_inv_Supplier inner join cld_cust On cld_cust.id = cld_inv_Supplier.custid  where cld_inv_Supplier.custid = " & custid & " ")

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function

    Public Function SelectDataset_Payment(ByVal custid As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""
        Try
            Dim a As New clsDbo("SELECT `cld_payment_type`.*, cld_cust.nama AS custname FROM cld_payment_type 
                  INNER JOIN cld_cust ON cld_cust.id = cld_payment_type.cust 
                  WHERE cld_payment_type.cust = " & custid & " ")

            ds = a.selectDataset

            a.dispose()
            a = Nothing
        Catch ex As Exception

        End Try
        Return ds

    End Function

    Public Function SelectDataset_SupplierbyUser(ByVal custid As Integer, ByVal userName As String, ByVal modpriv As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""

        If modpriv <> 0 Then
            strFilter = "SELECT cld_inv_Supplier.*, cld_cust.nama AS custname FROM cld_inv_Supplier 
                                INNER JOIN cld_cust ON cld_cust.id = cld_inv_Supplier.custid
                                INNER JOIN cld_ccxsupplier ON cld_inv_Supplier.id=cld_ccxsupplier.`supplier`
                                INNER JOIN cld_inv_cc ON cld_inv_cc.id = cld_ccxsupplier.`cc`
                                INNER JOIN cld_outlet ON cld_inv_cc.`Olid` = cld_outlet.`ID`
                                INNER JOIN cld_userxstore ON cld_outlet.id=cld_userxstore.`Store`
                                WHERE cld_inv_Supplier.custid = " & custid & " AND cld_userxstore.`User` = '" & userName & "'
                                order by cld_inv_Supplier.name"
        Else
            strFilter = "SELECT cld_inv_Supplier.*, cld_cust.nama as custname from cld_inv_Supplier inner join cld_cust On cld_cust.id = cld_inv_Supplier.custid  where cld_inv_Supplier.custid = " & custid
        End If


        Dim a As New clsDbo(strFilter)

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function

    Public Function SelectDataSupplier_id(ByVal sName As String) As Integer
        Dim idsup As Integer
        Dim strFilter As String = ""

        Dim a As New clsDbo("")

        Try
            idsup = a.ExecuteScalar("SELECT Id FROM cld_Inv_Supplier where name = '" & sName & "'", 0)
        Catch ex As Exception

        End Try

        a.dispose()
        a = Nothing

        Return idsup

    End Function
    Public Function SelectDataset_SupplierLoadXtrans(ByVal custid As Integer, ByVal icc As Integer, Optional ByVal src As String = "") As DataSet

        Dim ds As New DataSet
        Dim strFilter As String = ""

        Dim a As New clsDbo("Select * from cld_inv_supplier where custid = " & custid & " and (name like '%" & src & "%' Or email like '%" & src & "%' Or address like '%" & src & "%') and id not IN (SELECT supplier FROM cld_ccxsupplier WHERE cc = '" & icc & "') ")

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function
    Public Function SelectDataset_SupplierCCview(ByVal custid As Integer, ByVal icc As Integer, Optional ByVal src As String = "") As DataSet

        Dim ds As New DataSet
        Dim strFilter As String = ""
        Dim c_cmd1 As New clsDbo("")
        If c_cmd1.ExecuteScalar("SELECT olid FROM `cld_inv_cc` WHERE id=" & icc & "", 0) = 0 Then
            strFilter = "Select * from cld_inv_supplier where custid = " & custid & " and (name like '%" & src & "%' Or email like '%" & src & "%' Or address like '%" & src & "%') order by cld_inv_Supplier.name"
        Else

            strFilter = "Select * from cld_inv_supplier where custid = " & custid & " and (name like '%" & src & "%' Or email like '%" & src & "%' Or address like '%" & src & "%') and id IN (SELECT supplier FROM cld_ccxsupplier WHERE cc = '" & icc & "')order by cld_inv_Supplier.name"
        End If


        Dim a As New clsDbo(strFilter)

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function
    Public Sub DeleteCCXtranSupplier(ByVal supplier As Integer, ByVal Custid As Integer, ByVal xcc As Integer, ByVal username As String)
        'Dim pc As String = pic.ToString
        Try

            Dim str As String
            Dim c_cmd As New Cls_command

            str = "DELETE FROM cld_ccxsupplier  where supplier = " & supplier & " and cc = '" & xcc & "'"

            c_cmd.executeNonQuery(str)
            'Revisi 10 Mei
            insertLogModule(username, "DeleteCCXtranSupplier", "Delete", Custid)
        Catch ex As Exception

            errLog(Err.Description, "clsOutlet:DeleteOutlet")
            Err.Clear()

        End Try
    End Sub

    Public Function SelectDataset_SupplierByStk(ByVal custid As Integer, ByVal stkId As Integer) As DataSet
        Dim ds As New DataSet
        Try
            Dim a As New clsDbo("SELECT s.Id, s.Name FROM cld_Inv_Supplier s " & _
                                "INNER JOIN cld_inv_stkxsupplier x ON s.Id = x.supplier_id " & _
                                "WHERE s.custid = " & custid & " AND x.stk_id = " & stkId & " " & _
                                "ORDER BY s.Name")
            ds = a.selectDataset
            a.dispose()
            a = Nothing
        Catch ex As Exception
            errLog(Err.Description, "clsInventory_Supplier:SelectDataset_SupplierByStk")
        End Try
        Return ds
    End Function
End Class
