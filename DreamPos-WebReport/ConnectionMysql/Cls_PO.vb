Imports System.Collections.Generic

Public Class Cls_PO
    Public Function SelectDataset_Supplier(ByVal custid As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""


        Dim a As New clsDbo("SELECT cld_inv_Supplier.*, cld_cust.nama as custname from cld_inv_Supplier inner join cld_cust On cld_cust.id = cld_inv_Supplier.custid  where cld_inv_Supplier.custid = " & custid & " order by name")


        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function
    Public Function SelectDataTable_Stkall(ByVal dta As DataTable, ByVal custid As Integer, ByVal name As String, ByVal modpriv As Integer, ByVal ccid As Integer) As DataTable
        Dim dt As New DataTable
        Dim strFilter As String = ""
        Dim strf As String = ""
        Dim str As String = ""

        Dim colBValues = New List(Of String)

        If dta.Rows.Count > 0 Then
            For Each row As DataRow In dta.Rows
                If strFilter = "" Then
                    strFilter = "'" & row.Item("itemno") & "'"
                Else
                    strFilter = strFilter & "," & "'" & row.Item("itemno") & "'"
                End If

            Next
            strf = "and itemno not in  (" & strFilter & ") "
        End If

        str = "SELECT itemno, itemname FROM `cld_itemmaster`  WHERE link = 1 AND cld_itemmaster.custid = " & custid & " " & strf & "  
                    AND itemname LIKE '%" & name & "%' ORDER BY itemname"


        Dim a As New clsDbo(str)

        dt = a.selectDatatable

        a.dispose()
        a = Nothing

        Return dt

    End Function
End Class
