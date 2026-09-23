Public Class Cls_ExOther
    Public Function SelectDataExpOtherbyid(ByVal id As String) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""


        Dim a As New clsDbo("SELECT * FROM cld_ExpOther where id = '" & id & "'")


        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function

    Public Function SelectDataset_ExpOther(ByVal custid As Integer, ByVal ModPriv As Integer, ByVal user As String, ByVal bydate As Date) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""

        'revisi 27 feb 2016, perubahan di inv trans, jadi ada user yang hanya boleh liat storenya
        'Dim a As New clsDbo("SELECT cld_expOther.*, DATE_FORMAT(cld_expOther.Busidate,'%d-%b-%Y') AS BDate , cld_outlet.nama as OlName from cld_expOther inner join cld_outlet ON cld_expOther.olid = cld_outlet.id where cld_expOther.custid = " & custid & " ")

        If ModPriv And usrModulepriv.moduleuser = usrModulepriv.moduleuser Then

            strFilter = "SELECT cld_expOther.*, DATE_FORMAT(cld_expOther.Busidate,'%d-%b-%Y') AS BDate , cld_inv_cc.name as OlName from cld_expOther INNER JOIN cld_inv_cc ON cld_expOther.ccid = cld_inv_cc.id AND cld_expOther.Custid = cld_inv_cc.Custid " &
                        "where cld_expOther.custid = " & custid & " AND cld_inv_cc.olid IN (SELECT Store FROM cld_userxstore WHERE USER = '" & user & "') and cld_expOther.Busidate>= '" & bydate.ToString("yyyy-MM-dd") & "' "

        Else

            strFilter = "SELECT cld_expOther.*, DATE_FORMAT(cld_expOther.Busidate,'%d-%b-%Y') AS BDate , cld_inv_cc.name as OlName from cld_expOther inner join cld_inv_cc ON cld_expOther.ccid = cld_inv_cc.id and cld_expOther.Custid = cld_inv_cc.Custid " &
                        "where cld_expOther.custid = " & custid & " and cld_expOther.Busidate>= '" & bydate.ToString("yyyy-MM-dd") & "' "

        End If

        Dim a As New clsDbo(strFilter)

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function

    Public Function SelectDataset_ExOtherSearch(ByVal name As String, ByVal custid As Integer, ByVal ModPriv As Integer, ByVal user As String) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""

        'revisi 27 feb 2016, perubahan di inv trans, jadi ada user yang hanya boleh liat storenya
        ' Dim a As New clsDbo("SELECT  cld_expOther.*, DATE_FORMAT(cld_expOther.Busidate,'%d-%b-%Y') AS BDate , cld_outlet.nama as OlName from cld_expOther inner join cld_outlet ON cld_expOther.olid = cld_outlet.id  where cld_ExpOther.custid = " & custid & "  and  (cld_ExpOTher.name like '%" & name & "%' or cld_ExpOTher.id like '%" & name & "%') ")

        If ModPriv And usrModulepriv.moduleuser = usrModulepriv.moduleuser Then

            strFilter = "SELECT cld_expOther.*, DATE_FORMAT(cld_expOther.Busidate,'%d-%b-%Y') AS BDate , cld_outlet.nama as OlName from cld_expOther INNER JOIN cld_inv_cc ON cld_expOther.ccid = cld_inv_cc.id AND cld_expOther.Custid = cld_inv_cc.Custid " &
                        "where cld_ExpOther.custid = " & custid & " and  (cld_ExpOTher.name like '%" & name & "%' or cld_ExpOTher.id like '%" & name & "%') AND cld_inv_cc.olid IN (SELECT Store FROM cld_userxstore WHERE USER = '" & user & "')"

        Else

            strFilter = "SELECT cld_expOther.*, DATE_FORMAT(cld_expOther.Busidate,'%d-%b-%Y') AS BDate , cld_outlet.nama as OlName from cld_expOther INNER JOIN cld_inv_cc ON cld_expOther.ccid = cld_inv_cc.id AND cld_expOther.Custid = cld_inv_cc.Custid " &
                        "where cld_ExpOther.custid = " & custid & " and  (cld_ExpOTher.name like '%" & name & "%' or cld_ExpOTher.id like '%" & name & "%') "

        End If

        Dim a As New clsDbo(strFilter)
       
        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function
    Public Function SelectDataset_ExOtherEdit(ByVal Id As String, ByVal custid As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""


        Dim a As New clsDbo("SELECT * from cld_ExpOther where id = '" & Id & "' and custid = " & custid & "")

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function
    Public Function SelectDataset_longId(ByVal custid As Integer) As String
        Dim ds As New DataSet

        Dim strFilter As String = ""


        Dim a As New clsDbo("SELECT  * from cld_ExpOther where MONTH(busidate) = " & Date.Now.Month & " and custid = " & custid & " order by id desc")

        ds = a.selectDataset

        If ds.Tables(0).Rows.Count > 0 Then
            strFilter = ds.Tables(0).Rows(0).Item("Id")

        End If
        a.dispose()
        a = Nothing

        Return strFilter

    End Function
End Class
