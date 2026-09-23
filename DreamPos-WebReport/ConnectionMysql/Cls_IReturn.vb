Public Class Cls_IReturn
    Public Function SelectDataset_invReturn(ByVal bydate As Date, ByVal custid As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""

        'revisi 8 nov 2016, soalnya error di test pake tgl
        Dim a As New clsDbo("SELECT cld_invlk_Rcv.*, case cld_invlk_Rcv.flag when 1 then 'Pending' when 2 then 'Finish' END as Status,  DATE_FORMAT(cld_invlk_rcv.CreatedDate,'%d-%b-%Y') AS CreateD, cld_inv_cc.Name AS ccname, cld_inv_supplier.Name AS Supname   FROM cld_invlk_Rcv LEFT JOIN cld_inv_cc ON cld_invlk_Rcv.cc = cld_inv_cc.ID LEFT JOIN cld_Inv_supplier ON cld_Invlk_Rcv.supid = cld_inv_supplier.id  where DATE(cld_invlk_rcv.CreatedDate) = '" & bydate.ToString("yyyy-MM-dd") & "' and cld_invlk_rcv.custid = " & custid & " and Tipe = 2")
        ' Dim a As New clsDbo("SELECT cld_invlk_Rcv.*, case cld_invlk_Rcv.flag when 1 then 'Pending' when 2 then 'Finish' END as Status,  DATE_FORMAT(cld_invlk_rcv.CreatedDate,'%d-%b-%Y') AS CreateD, cld_inv_cc.Name AS ccname, cld_inv_supplier.Name AS Supname   FROM cld_invlk_Rcv LEFT JOIN cld_inv_cc ON cld_invlk_Rcv.cc = cld_inv_cc.ID LEFT JOIN cld_Inv_supplier ON cld_Invlk_Rcv.supid = cld_inv_supplier.id  where  cld_invlk_rcv.custid = " & custid & " and Tipe = 2")

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function
    Public Function SelectDataset_invReturnSearch(ByVal name As String, ByVal bydate As Date, ByVal custid As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""

        'revisi 8 nov 2016, soalnya error di test pake tgl
        Dim a As New clsDbo("SELECT cld_invlk_rcv.*, case cld_invlk_Rcv.flag when 1 then 'Pending' when 2 then 'Finish' END as Status, DATE_FORMAT(cld_invlk_rcv.CreatedDate,'%d-%b-%Y') AS CreateD, cld_inv_cc.Name AS ccname, cld_inv_supplier.Name AS Supname   FROM cld_invlk_Rcv LEFT JOIN cld_inv_cc ON cld_invlk_Rcv.cc = cld_inv_cc.ID LEFT JOIN cld_Inv_supplier ON cld_Invlk_Rcv.supid = cld_inv_supplier.id  where cld_invlk_Rcv.CreatedDate = '" & bydate.ToString("yyyy-MM-dd") & "' and (cld_inv_supplier.name Like '%" & name & "%' or cld_invlk_rcv.id Like '%" & name & "%') and cld_invlk_rcv.custid = " & custid & " and Tipe = 2")
        'Dim a As New clsDbo("SELECT cld_invlk_rcv.*, case cld_invlk_Rcv.flag when 1 then 'Pending' when 2 then 'Finish' END as Status, DATE_FORMAT(cld_invlk_rcv.CreatedDate,'%d-%b-%Y') AS CreateD, cld_inv_cc.Name AS ccname, cld_inv_supplier.Name AS Supname   FROM cld_invlk_Rcv LEFT JOIN cld_inv_cc ON cld_invlk_Rcv.cc = cld_inv_cc.ID LEFT JOIN cld_Inv_supplier ON cld_Invlk_Rcv.supid = cld_inv_supplier.id  where  (cld_inv_supplier.name Like '%" & name & "%' or cld_invlk_rcv.id Like '%" & name & "%') and cld_invlk_rcv.custid = " & custid & " and Tipe = 2")

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function
    Public Function SelectDataset_longId(ByVal custid As Integer) As String
        Dim ds As New DataSet

        Dim strFilter As String = ""


        Dim a As New clsDbo("SELECT  * from cld_invlk_rcv where MONTH(createddate) = " & Date.Now.Month & " and custid = " & custid & " and Tipe = 2 order by id desc")

        ds = a.selectDataset

        If ds.Tables(0).Rows.Count > 0 Then
            strFilter = ds.Tables(0).Rows(0).Item("Id")

        End If
        a.dispose()
        a = Nothing

        Return strFilter

    End Function
    Public Function SelectDataTable_invReturn(ByVal linkid As String, ByVal custid As Integer) As DataTable
        Dim ds As New DataTable
        Dim strFilter As String = ""


        Dim a As New clsDbo("SELECT *, (ttlStock * (SELECT cost FROM cld_inv_transstockcls WHERE invdate = '" & Now.ToString("yyyy-MM-dd 00:00:00") & "' AND ccid = cc AND stkid=stk_id)) AS ttlcost,
(SELECT ttlqty FROM cld_inv_transstockcls WHERE invdate = '" & Now.ToString("yyyy-MM-dd 00:00:00") & "' AND ccid = cc AND stkid=stk_id) ttlqty
From cld_inv_trans where linkid = '" & linkid & "' and type = 4 and custid = " & custid & "")

        ds = a.selectDatatable

        a.dispose()
        a = Nothing

        Return ds

    End Function
    Function rtn_date(ByVal ID As String) As Integer
        'Dim ds As New DataSet

        Dim edtable As Integer = 0

        Try
            Dim a As New clsDbo("")
            edtable = a.ExecuteScalar("SELECT count(id) from cld_invlk_rcv where  cld_invlk_rcv.id = '" & ID & "' and tipe = 2 and cld_invlk_rcv.CreatedDate >= '" & Now.AddDays(-7).ToString("yyyy-MM-dd") & "'", 0)


            a.dispose()
            a = Nothing

        Catch ex As Exception
            errLog(Err.Description, "Cls_IReturn:rtn_date")
            Err.Clear()
        End Try

        Return edtable

    End Function
End Class
