Public Class ClsCustomer

    Public Function SelectDataset_Customer(ByVal id As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""


        Dim a As New clsDbo("SELECT * from cld_cust where id = " & id & "")

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function

    Public Function SelectDataset_CustomerEdit(ByVal Id As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""


        Dim a As New clsDbo("SELECT * from cld_cust where id = " & Id & "")

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function

    Public Function SelectDataset_CustomerSearch(ByVal name As String, ByVal id As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""


        Dim a As New clsDbo("SELECT * from cld_cust Where No like '%" & name & "%' Or Nama like '%" & name & "%' and id = " & id & " ")

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function

    'Revisi 10 Mei CustName
    Public Function SelectDataset_CustName(ByVal Custid As Integer) As String
        Dim ds As New DataSet

        Dim Qty As String = ""

        Dim a As New clsDbo("SELECT Nama from cld_cust where id = " & Custid & "")

        ds = a.selectDataset

        Qty = ds.Tables(0).Rows(0)("Nama")

        a.dispose()
        a = Nothing

        Return Qty

    End Function

End Class
