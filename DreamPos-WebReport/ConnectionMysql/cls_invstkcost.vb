Public Class cls_invstkcost

    Public Function get_stkCost(ByVal iStk As Integer, ByVal iCc As Integer, ByVal iCust As Integer) As Decimal

        Dim hasil As Decimal

        Dim cCmd As New Cls_command
        hasil = cCmd.ExecuteScalar("SELECT Cost FROM cld_inv_stkcost AS Cost WHERE cc = " & iCc & " AND stkid = " & iStk & " AND Custid = " & iCust & "", 0)

        Return hasil

    End Function


    'Select all Receipe
    Public Function getDataset_Receipe(ByVal custid As Integer) As DataSet
        'perubahan tgl 2 nov 2016------++++
        Dim ds As New DataSet

        Try
            Dim c_Cmd As New Cls_command
            ds = c_Cmd.ReturnDataset("Select * from cld_inv_rcp where custid = " & custid)

            c_Cmd.closeConnection()
            c_Cmd = Nothing

        Catch ex As Exception
            errLog(Err.Description, "cls_invstkcost:getDataset_Receipe")
            Err.Clear()
        End Try


        Return ds


    End Function


    'Select all Receipe
    Public Function getDataset_LinkMenu(ByVal custid As Integer) As DataSet
        'perubahan tgl 2 nov 2016------++++
        Dim ds As New DataSet

        Try
            Dim c_Cmd As New Cls_command
            ds = c_Cmd.ReturnDataset("Select * from cld_itemmaster where iType > 0 and custid = " & custid)

            c_Cmd.closeConnection()
            c_Cmd = Nothing

        Catch ex As Exception
            errLog(Err.Description, "cls_invstkcost:getDataset_LinkMenu")
            Err.Clear()
        End Try


        Return ds


    End Function





End Class
