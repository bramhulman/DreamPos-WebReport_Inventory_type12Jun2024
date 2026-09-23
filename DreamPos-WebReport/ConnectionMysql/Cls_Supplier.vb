Public Class Cls_Supplier
    Public Function SelectDataSupplier(ByVal id As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""


        Dim a As New clsDbo("SELECT * FROM cld_Inv_Supplier where id = " & id)


        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function
    Public Function SelectDataset_Supplier(ByVal custid As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""


        Dim a As New clsDbo("SELECT cld_inv_Supplier.*, cld_cust.nama as custname from cld_inv_Supplier inner join cld_cust On cld_cust.id = cld_inv_Supplier.custid  where cld_inv_Supplier.custid = " & custid & " order by name")


        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function
    Public Function SelectDataset_SupplierSearch(ByVal name As String, ByVal custid As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""


        Dim a As New clsDbo("Select cld_inv_Supplier.*, cld_cust.nama as custname  from cld_inv_Supplier inner join cld_cust On cld_cust.id = cld_inv_Supplier.custid  where cld_inv_Supplier.custid = " & custid & "  and cld_inv_Supplier.name like '%" & name & "%' order by name")

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function
    Public Function SelectDataset_SupplierEdit(ByVal Id As Integer, ByVal custid As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""


        Dim a As New clsDbo("SELECT * from cld_inv_Supplier where id = " & Id & " and custid = " & custid & "")

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function
End Class
