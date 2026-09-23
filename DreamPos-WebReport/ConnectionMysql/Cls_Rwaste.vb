Public Class Cls_Rwaste
    Public Function SelectDataset_InRwasteEdit(ByVal Id As Integer, ByVal custid As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""

        'Dim a As New clsDbo("SELECT cld_inv_trans.*, IFNULL((SELECT ttlqty FROM cld_inv_transstock WHERE stkid = cld_inv_trans.stk_id AND ccid = cld_inv_trans.Cc AND custid = cld_inv_trans.CustID), 0) AS stock FROM cld_inv_trans WHERE id = " & Id & " AND  custid = " & custid & "")

        'perubahan 11 oct 2017
        Dim a As New clsDbo("SELECT cld_inv_trans.*, IFNULL((SELECT ttlqty FROM cld_inv_transstock WHERE stkid = cld_inv_trans.stk_id AND ccid = cld_inv_trans.Cc AND custid = cld_inv_trans.CustID), 0) AS stock " & _
                            ", IFNULL((SELECT cost FROM cld_inv_stkcost WHERE stkid = cld_inv_trans.stk_id AND cc = cld_inv_trans.Cc AND custid = cld_inv_trans.CustID ), 0) AS coststk " & _
                            "FROM cld_inv_trans WHERE type = 2 and id = " & Id & " AND  custid = " & custid & "")

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function

    Public Function SelectDataset_invRwaste(ByVal bydate As Date, ByVal custid As Integer, ByVal ModPriv As Integer, ByVal user As String) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""

        'revisi 8 nov 2016, soalnya error di test pake tgl
        ' Dim a As New clsDbo("SELECT cld_inv_Trans.*, DATE_FORMAT(cld_inv_trans.invDate,'%d-%b-%Y') AS InvD, cld_inv_cc.Name AS ccname  FROM cld_inv_trans LEFT JOIN cld_inv_cc ON cld_inv_trans.cc = cld_inv_cc.ID where  cld_inv_Trans.type =  2 and cld_inv_trans.custid = " & custid & " and cld_inv_Trans.InvDate = '" & bydate.ToString("yyyy-MM-dd") & "'")

        'revisi 27 feb 2017, sekarang berdasarkan user, jadi ada user yang hanya dapat lihat pergerakan inventory tokonya saja
        'Dim a As New clsDbo("SELECT cld_inv_Trans.*, DATE_FORMAT(cld_inv_trans.invDate,'%d-%b-%Y') AS InvD, cld_inv_cc.Name AS ccname  FROM cld_inv_trans LEFT JOIN cld_inv_cc ON cld_inv_trans.cc = cld_inv_cc.ID where  cld_inv_Trans.type =  2 and cld_inv_trans.custid = " & custid)

        If ModPriv And usrModulepriv.moduleuser = usrModulepriv.moduleuser Then

            strFilter = "SELECT cld_inv_Trans.*, DATE_FORMAT(cld_inv_trans.invDate,'%d-%b-%Y') AS InvD, cld_inv_cc.Name AS ccname  FROM cld_inv_trans LEFT JOIN cld_inv_cc ON cld_inv_trans.cc = cld_inv_cc.ID and cld_inv_trans.Custid = cld_inv_cc.Custid " &
                        "where  cld_inv_Trans.type =  2 and cld_inv_trans.custid = " & custid & " and cld_inv_Trans.InvDate = '" & bydate.ToString("yyyy-MM-dd") & "' and  cld_inv_cc.olid IN (SELECT Store FROM cld_userxstore WHERE USER = '" & user & "')"

        Else

            strFilter = "SELECT cld_inv_Trans.*, DATE_FORMAT(cld_inv_trans.invDate,'%d-%b-%Y') AS InvD, cld_inv_cc.Name AS ccname  FROM cld_inv_trans LEFT JOIN cld_inv_cc ON cld_inv_trans.cc = cld_inv_cc.ID and cld_inv_trans.Custid = cld_inv_cc.Custid " & _
                        "where  cld_inv_Trans.type =  2 and cld_inv_trans.custid = " & custid & " and cld_inv_Trans.InvDate = '" & bydate.ToString("yyyy-MM-dd") & "'"

        End If

        Dim a As New clsDbo(strFilter)

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function

    Public Function SelectDataset_invRwasteSearch(ByVal name As String, ByVal bydate As Date, ByVal custid As Integer, ByVal ModPriv As Integer, ByVal user As String) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""

        'revisi 8 nov 2016, soalnya error di test pake tgl
        'Dim a As New clsDbo("SELECT cld_inv_Trans.*, DATE_FORMAT(cld_inv_trans.invDate,'%d-%b-%Y') AS InvD, cld_inv_cc.Name AS ccname  FROM cld_inv_trans LEFT JOIN cld_inv_cc ON cld_inv_trans.cc = cld_inv_cc.ID where cld_inv_Trans.type =  2 and cld_inv_trans.custid = " & custid & " and cld_inv_Trans.InvDate = '" & bydate.ToString("yyyy-MM-dd") & "' and (cld_inv_cc.name Like '%" & name & "%' or cld_inv_trans.linkid Like '%" & name & "%')")

        'revisi 27 feb 2016, perubahan di inv trans, jadi ada user yang hanya boleh liat storenya
        'Dim a As New clsDbo("SELECT cld_inv_Trans.*, DATE_FORMAT(cld_inv_trans.invDate,'%d-%b-%Y') AS InvD, cld_inv_cc.Name AS ccname  FROM cld_inv_trans LEFT JOIN cld_inv_cc ON cld_inv_trans.cc = cld_inv_cc.ID where cld_inv_Trans.type =  2 and cld_inv_trans.custid = " & custid & " and (cld_inv_cc.name Like '%" & name & "%' or cld_inv_trans.linkid Like '%" & name & "%')")

        If ModPriv And usrModulepriv.moduleuser = usrModulepriv.moduleuser Then


            strFilter = "SELECT cld_inv_Trans.*, DATE_FORMAT(cld_inv_trans.invDate,'%d-%b-%Y') AS InvD, cld_inv_cc.Name AS ccname  FROM cld_inv_trans LEFT JOIN cld_inv_cc ON cld_inv_trans.cc = cld_inv_cc.ID and cld_inv_trans.Custid = cld_inv_cc.Custid" &
                        " where cld_inv_Trans.type =  2 and cld_inv_trans.custid = " & custid & " and cld_inv_Trans.InvDate = '" & bydate.ToString("yyyy-MM-dd") & "' and (cld_inv_cc.name Like '%" & name & "%' or cld_inv_trans.linkid Like '%" & name & "%') and cld_inv_cc.olid IN (SELECT Store FROM cld_userxstore WHERE USER = '" & user & "')"

        Else

            strFilter = "SELECT cld_inv_Trans.*, DATE_FORMAT(cld_inv_trans.invDate,'%d-%b-%Y') AS InvD, cld_inv_cc.Name AS ccname  FROM cld_inv_trans LEFT JOIN cld_inv_cc ON cld_inv_trans.cc = cld_inv_cc.ID and cld_inv_trans.Custid = cld_inv_cc.Custid" & _
                        " where cld_inv_Trans.type =  2 and cld_inv_trans.custid = " & custid & " and cld_inv_Trans.InvDate = '" & bydate.ToString("yyyy-MM-dd") & "' and (cld_inv_cc.name Like '%" & name & "%' or cld_inv_trans.linkid Like '%" & name & "%')"


        End If


        Dim a As New clsDbo(strFilter)

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function

    Public Function SelectDataset_longId(ByVal custid As Integer) As String
        Dim ds As New DataSet

        Dim strFilter As String = ""


        Dim a As New clsDbo("SELECT  * from cld_inv_Trans where MONTH(invDate) = " & Date.Now.Month & " and type =  2 and custid = " & custid & " order by linkid desc")

        ds = a.selectDataset

        If ds.Tables(0).Rows.Count > 0 Then
            strFilter = ds.Tables(0).Rows(0).Item("linkid")

        End If
        a.dispose()
        a = Nothing

        Return strFilter

    End Function
   
End Class
