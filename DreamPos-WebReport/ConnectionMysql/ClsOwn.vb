Public Class ClsOwn

    Public Function SelectDataset_own(ByVal id As Integer, ByVal custid As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""


        Dim a As New clsDbo("SELECT * from cld_own where id = " & id & " and custid = " & custid & "")

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function



    Public Function SelectDataset_ownSearch(ByVal name As String, ByVal custid As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""


        Dim a As New clsDbo("SELECT * from cld_own Where Name like '%" & name & "%' and custid = " & custid & " order by id ")

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function

    'Revisi 10 Mei CustName
    Public Function SelectDataset_ownName(ByVal Custid As Integer) As String
        Dim ds As New DataSet

        Dim Qty As String = ""

        Dim a As New clsDbo("SELECT Name from cld_own where Custid = " & Custid & "")

        ds = a.selectDataset

        Qty = ds.Tables(0).Rows(0)("Name")

        a.dispose()
        a = Nothing

        Return Qty

    End Function

End Class
