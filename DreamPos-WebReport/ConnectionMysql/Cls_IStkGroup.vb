Public Class Cls_IStkGroup

    Public Function SelectDataStkGroup(ByVal id As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""


        Dim a As New clsDbo("SELECT * FROM cld_Inv_Stkgroup where id = " & id)


        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function
    Public Function SelectDataset_StkGroup(ByVal custid As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""


        Dim a As New clsDbo("SELECT cld_inv_stkgroup.*, cld_cust.nama as custname from cld_inv_stkgroup inner join cld_cust On cld_cust.id = cld_inv_stkgroup.custid  where cld_inv_stkgroup.custid = " & custid & " order by code")


        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function
    Public Function select_StkGroup(ByVal custid As Integer, ByVal stkgrpid As Integer) As Boolean
        Dim notin As Boolean = False
        Dim strFilter As String = ""


        Dim a As New clsDbo("")
        If a.ExecuteScalar("SELECT count(id) from cld_inv_stkgroup where cld_inv_stkgroup.custid = " & custid & " and id ='" & stkgrpid & "'", 0) = 0 Then
            notin = True
        End If

        a.dispose()
        a = Nothing

        Return notin

    End Function
    Public Function SelectDataset_StkGroupSearch(ByVal name As String, ByVal custid As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""


        Dim a As New clsDbo("SELECT cld_inv_stkgroup.*, cld_cust.nama as custname from cld_inv_stkgroup inner join cld_cust On cld_cust.id = cld_inv_stkgroup.custid  where cld_inv_stkgroup.custid = " & custid & "  and cld_inv_stkgroup.code like  '%" & name & "%' Or cld_inv_stkgroup.name like '%" & name & "%' order by code")

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function
    Public Function SelectDataset_StkGroupEdit(ByVal Id As Integer, ByVal custid As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""


        Dim a As New clsDbo("SELECT * from cld_inv_stkgroup where id = " & Id & " and custid = " & custid & "")

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function
    Public Function SelectDataset_Idstkgroup(ByVal grpID As Integer) As Integer
        Dim ds As New DataSet
        Dim strFilter As String = ""
        Dim Qty As Integer

        Dim a As New clsDbo("Select * from cld_inv_stk where grpid = " & grpID & "")

        ds = a.selectDataset

        If ds.Tables(0).Rows.Count > 0 Then
            Qty = 1
        Else
            Qty = 0
        End If
        a.dispose()
        a = Nothing

        Return Qty

    End Function
    Public Sub insertStkgGroup(ByVal id As Integer, ByVal custid As Integer, ByVal code As String, ByVal nama As String, ByVal username As String)
        Try

            'Dim ds As DataSet
            'ds = SelectDataStkGroup(code)



            Dim str As String
            Dim c_cmd As New clsDbo("")
            If select_StkGroup(custid, id) = True Or id = 0 Then
                str = "insert into cld_inv_Stkgroup values (" & c_cmd.ExecuteScalar("select max(ID) from cld_inv_stkgroup") & ", " & custid & ",'" & code & "', '" & nama & "')"

                insertLogModule(username, "Stkgroup", "Insert", custid)
            Else
                str = "Update cld_inv_Stkgroup set code = '" & code & "', name = '" & nama & "' where id = " & id & ""

                insertLogModule(username, "Stkgroup", "Update", custid)
            End If

            c_cmd.executeNonQuery(str)

        Catch ex As Exception

            errLog(Err.Description, "webservice:insertStkGroup")
            Err.Clear()
        End Try
    End Sub

End Class
