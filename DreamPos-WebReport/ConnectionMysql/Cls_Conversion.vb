Public Class Cls_Conversion
    Public Function SelectDataset_invConversion(ByVal bydate As Date, ByVal custid As Integer, ByVal ModPriv As Integer, ByVal user As String) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""


        If ModPriv And usrModulepriv.moduleuser = usrModulepriv.moduleuser Then

            strFilter = "SELECT cld_inv_Trans.*, DATE_FORMAT(cld_inv_trans.invDate,'%d-%b-%Y') AS InvD  FROM cld_inv_trans LEFT JOIN cld_inv_cc ON cld_inv_trans.cc = cld_inv_cc.ID and cld_inv_trans.Custid = cld_inv_cc.Custid " &
                        "where  cld_inv_Trans.type =  15 and cld_inv_trans.custid = " & custid & " and cld_inv_Trans.InvDate = '" & bydate.ToString("yyyy-MM-dd") & "' and  cld_inv_cc.olid IN (SELECT Store FROM cld_userxstore WHERE USER = '" & user & "')"

        Else

            strFilter = "SELECT cld_inv_Trans.*, DATE_FORMAT(cld_inv_trans.invDate,'%d-%b-%Y') AS InvD  FROM cld_inv_trans " &
                        "where  cld_inv_Trans.type =  15 and cld_inv_trans.custid = " & custid & " and cld_inv_Trans.InvDate = '" & bydate.ToString("yyyy-MM-dd") & "'"

        End If

        Dim a As New clsDbo(strFilter)

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function

    Public Function SelectDataset_invConversionSearch(ByVal name As String, ByVal bydate As Date, ByVal custid As Integer, ByVal ModPriv As Integer, ByVal user As String) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""

        If ModPriv And usrModulepriv.moduleuser = usrModulepriv.moduleuser Then


            strFilter = "SELECT cld_inv_Trans.*, DATE_FORMAT(cld_inv_trans.invDate,'%d-%b-%Y') AS InvD, cld_inv_cc.Name AS ccname  FROM cld_inv_trans LEFT JOIN cld_inv_cc ON cld_inv_trans.cc = cld_inv_cc.ID and cld_inv_trans.Custid = cld_inv_cc.Custid " &
                        "where cld_inv_Trans.type = 15 and cld_inv_trans.custid = " & custid & " and cld_inv_Trans.InvDate = '" & bydate.ToString("yyyy-MM-dd") & "' and (cld_inv_cc.name Like '%" & name & "%' or cld_inv_Trans.stk_name Like '%" & name & "%') and cld_inv_cc.olid IN (SELECT Store FROM cld_userxstore WHERE USER = '" & user & "')"

        Else

            strFilter = "SELECT cld_inv_Trans.*, DATE_FORMAT(cld_inv_trans.invDate,'%d-%b-%Y') AS InvD, cld_inv_cc.Name AS ccname  FROM cld_inv_trans LEFT JOIN cld_inv_cc ON cld_inv_trans.cc = cld_inv_cc.ID and cld_inv_trans.Custid = cld_inv_cc.Custid " &
                        "where cld_inv_Trans.type = 15 and cld_inv_trans.custid = " & custid & " and cld_inv_Trans.InvDate = '" & bydate.ToString("yyyy-MM-dd") & "' and (cld_inv_cc.name Like '%" & name & "%' or cld_inv_Trans.stk_name Like '%" & name & "%')"


        End If


        Dim a As New clsDbo(strFilter)

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function
    Public Function SelectDataset_InvConversionEdit(ByVal Id As Integer, ByVal custid As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""

        'perubahan 11 oct 2017
        Dim a As New clsDbo("SELECT cld_inv_trans.*, IFNULL((SELECT ttlqty FROM cld_inv_transstockcls WHERE stkid = cld_inv_trans.stk_id AND ccid = cld_inv_trans.Cc AND custid = cld_inv_trans.CustID and invdate = '" & Now.ToString("yyyy-MM-dd 00:00:00") & "'), 0) AS stock " &
                            ", IFNULL((SELECT cost FROM cld_inv_transstockcls WHERE stkid = cld_inv_trans.`Stk_Id` AND ccid = cld_inv_trans.`Cc`
AND custid = cld_inv_trans.`CustID` AND invdate = '" & Now.ToString("yyyy-MM-dd 00:00:00") & "'), 0) AS coststk " &
                            "FROM cld_inv_trans WHERE type = 15 and id = " & Id & " AND  custid = " & custid & "")

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function
    Public Function batch_date(ByVal linkID As String) As Integer
        'Dim ds As New DataSet

        Dim edtable As Integer = 0

        Try
            Dim a As New clsDbo("")
            edtable = a.ExecuteScalar("SELECT count(id) from cld_inv_trans where  linkid = '" & linkID & "' and CreatedDate >= '" & Now.AddDays(-7).ToString("yyyy-MM-dd") & "'", 0)


            a.dispose()
            a = Nothing

        Catch ex As Exception
            errLog(Err.Description, "Cls_Conversion:batch_date")
            Err.Clear()
        End Try

        Return edtable

    End Function
    Public Function SelectDataTable_invbatchedit(ByVal linkid As String, ByVal custid As Integer) As DataTable
        Dim ds As New DataTable
        Dim strFilter As String = ""


        Dim a As New clsDbo("SELECT *, (ttlStock * cost) AS ttlcost From cld_inv_trans where linkid = '" & linkid & "' and type = 9 and custid = " & custid & "")

        ds = a.selectDatatable

        a.dispose()
        a = Nothing

        Return ds

    End Function
    Public Function SelectDataTable_invbatch(ByVal linkid As String, ByVal custid As Integer) As DataTable
        Dim ds As New DataTable
        Dim strFilter As String = ""


        Dim a As New clsDbo("SELECT *, (ttlStock * cost) AS ttlcost From cld_inv_trans where linkid = '" & linkid & "' and type = 9 and custid = " & custid & "")

        ds = a.selectDatatable

        a.dispose()
        a = Nothing

        Return ds

    End Function
End Class
