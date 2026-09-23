Public Class Cls_ITrf


    Public Function SelectDataset_longId(ByVal custid As Integer) As String
        Dim ds As New DataSet

        Dim strFilter As String = ""


        Dim a As New clsDbo("SELECT  * from cld_invlk_trf where MONTH(createddate) = " & Date.Now.Month & " and custid = " & custid & " order by id desc")

        ds = a.selectDataset

        If ds.Tables(0).Rows.Count > 0 Then
            strFilter = ds.Tables(0).Rows(0).Item("Id")
        End If

        a.dispose()
        a = Nothing

        Return strFilter

    End Function

    Public Function SelectDataset_invtrf(ByVal bydate As Date, ByVal custid As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""

        'revisi 8 nov 2016, soalnya error di test pake tgl
        ' Dim a As New clsDbo("SELECT cld_invlk_trf.*, DATE_FORMAT(cld_invlk_trf.CreatedDate,'%d-%b-%Y') AS CreateD, cld_inv_cc.Name AS ccname  FROM cld_invlk_trf LEFT JOIN cld_inv_cc ON cld_invlk_trf.ccFrom = cld_inv_cc.ID where cld_invlk_trf.custid = " & custid & " and  cld_invlk_trf.CreatedDate = '" & bydate.ToString("yyyy-MM-dd") & "'")
        Dim a As New clsDbo("SELECT cld_invlk_trf.*, DATE_FORMAT(cld_invlk_trf.CreatedDate,'%d-%b-%Y') AS CreateD, cld_inv_cc.Name AS ccname  FROM cld_invlk_trf LEFT JOIN cld_inv_cc ON cld_invlk_trf.ccFrom = cld_inv_cc.ID where cld_invlk_trf.custid = " & custid & " and  cld_invlk_trf.CreatedDate = '" & bydate.ToString("yyyy-MM-dd") & "'")

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        ' errLog("SELECT cld_invlk_trf.*, DATE_FORMAT(cld_invlk_trf.CreatedDate,'%d-%b-%Y') AS CreateD, cld_inv_cc.Name AS ccname  FROM cld_invlk_trf LEFT JOIN cld_inv_cc ON cld_invlk_trf.ccFrom = cld_inv_cc.ID where cld_invlk_trf.custid = " & custid & " and  cld_invlk_trf.CreatedDate = '" & bydate.ToString("yyyy-MM-dd") & "'", "webservice:DeleteInvrcpdtl")

        'errLog("SELECT cld_invlk_trf.*, DATE_FORMAT(cld_invlk_trf.CreatedDate,'%d-%b-%Y') AS CreateD, cld_inv_cc.Name AS ccname  FROM cld_invlk_trf LEFT JOIN cld_inv_cc ON cld_invlk_trf.ccFrom = cld_inv_cc.ID where cld_invlk_trf.custid = " & custid & " and  cld_invlk_trf.CreatedDate = '" & bydate.ToString("yyyy-MM-dd") & "'", "webservice:DeleteInvrcpdtl")

        Return ds

    End Function



    Public Function SelectDataset_invtrfSearch(ByVal name As String, ByVal bydate As Date, ByVal custid As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""

        'revisi 8 nov 2016, soalnya error di test pake tgl
        'Dim a As New clsDbo("SELECT cld_invlk_trf.*, DATE_FORMAT(cld_invlk_trf.CreatedDate,'%d-%b-%Y') AS CreateD, cld_inv_cc.Name AS ccname  FROM cld_invlk_trf LEFT JOIN cld_inv_cc ON cld_invlk_trf.ccFrom = cld_inv_cc.ID where  cld_invlk_trf.custid = " & custid & " and cld_invlk_trf.CreatedDate = '" & bydate.ToString("yyyy-MM-dd") & "' and (cld_inv_cc.name Like '%" & name & "%' or cld_invlk_trf.id Like '%" & name & "%')")
        Dim a As New clsDbo("SELECT cld_invlk_trf.*, DATE_FORMAT(cld_invlk_trf.CreatedDate,'%d-%b-%Y') AS CreateD, cld_inv_cc.Name AS ccname  FROM cld_invlk_trf LEFT JOIN cld_inv_cc ON cld_invlk_trf.ccFrom = cld_inv_cc.ID where  cld_invlk_trf.custid = " & custid & " and cld_invlk_trf.CreatedDate = '" & bydate.ToString("yyyy-MM-dd") & "' and (cld_inv_cc.name Like '%" & name & "%' or cld_invlk_trf.id Like '%" & name & "%')")

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function
End Class
