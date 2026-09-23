Public Class Cls_IWarehouse


    Public Function ModPrvDep(ByVal ModPrive As Integer, ByVal name As String) As String
        If ModPrive And usrModulepriv.moduleuser = usrModulepriv.moduleuser Then
            Return " and cld_inv_cc.olid in (Select store from cld_userxstore where user = '" & name & "')"
        Else
            Return ""
        End If

    End Function


    Public Function SelectDataset_longId(ByVal custid As Integer, ByVal cc As Integer) As String
        Dim ds As New DataSet

        Dim strFilter As String = ""

        Dim a As New clsDbo("SELECT ID from cld_inv_w where MONTH(CreatedDate) = " & Date.Now.Month & " and custid = " & custid & " and ccTo = " & cc & " order by ID desc")

        ds = a.selectDataset

        If ds.Tables(0).Rows.Count > 0 Then
            strFilter = ds.Tables(0).Rows(0).Item("ID")
        End If

        a.dispose()
        a = Nothing

        Return strFilter

    End Function

    Public Function SelectDataset_longDelId(ByVal custid As Integer, ByVal cc As Integer) As String
        Dim ds As New DataSet

        Dim strFilter As String = ""

        Dim a As New clsDbo("SELECT DelID from cld_inv_w where DelID IS NOT null and MONTH(CreatedDate) = " & Date.Now.Month & " and custid = " & custid & " and ccFrom = " & cc & " order by DelID desc")

        ds = a.selectDataset

        If ds.Tables(0).Rows.Count > 0 Then
            strFilter = ds.Tables(0).Rows(0).Item("DelID")
        End If

        a.dispose()
        a = Nothing

        Return strFilter

    End Function


    Public Function SelectDataset_invWare(ByVal itipe As Integer, ByVal custid As Integer, ByVal Modpriv As Integer, ByVal user As String, ByVal icc As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""

        If icc = 0 Then
            strFilter = "SELECT cld_inv_w.*, case cld_inv_w.flag when 1 then 'Order' WHEN 2 THEN 'Delivery' WHEN 3 THEN 'Receive' END as Status,  DATE_FORMAT(cld_inv_w.CreatedDate,'%d-%b-%Y') AS CreateD, cld_inv_cc.Name AS ccname FROM cld_inv_w LEFT JOIN cld_inv_cc ON cld_inv_w.ccTo = cld_inv_cc.ID and cld_inv_w.custid = cld_inv_cc.custid  where cld_inv_w.custid = " & custid & " and cld_inv_w.flag = " & itipe & ModPrvDep(Modpriv, user) & " Order by cld_inv_w.id"
        Else
            strFilter = "SELECT cld_inv_w.*, case cld_inv_w.flag when 1 then 'Order' WHEN 2 THEN 'Delivery' WHEN 3 THEN 'Receive' END as Status,  DATE_FORMAT(cld_inv_w.CreatedDate,'%d-%b-%Y') AS CreateD, cld_inv_cc.Name AS ccname FROM cld_inv_w LEFT JOIN cld_inv_cc ON cld_inv_w.ccTo = cld_inv_cc.ID and cld_inv_w.custid = cld_inv_cc.custid  where cld_inv_w.ccto = " & icc & " and cld_inv_w.custid = " & custid & " and cld_inv_w.flag = " & itipe & ModPrvDep(Modpriv, user) & " Order by cld_inv_w.id"
        End If

        Dim a As New clsDbo(strFilter)

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function

    Public Function SelectDataset_invWareSearch(ByVal itipe As Integer, ByVal name As String, ByVal custid As Integer, ByVal Modpriv As Integer, ByVal user As String, ByVal icc As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""

        If icc = 0 Then
            If itipe = 0 Then
                strFilter = "SELECT cld_inv_w.*, case cld_inv_w.flag when 1 then 'Order' WHEN 2 THEN 'Delivery' WHEN 3 THEN 'Receive' END as Status,  DATE_FORMAT(cld_inv_w.CreatedDate,'%d-%b-%Y') AS CreateD, cld_inv_cc.Name AS ccname FROM cld_inv_w LEFT JOIN cld_inv_cc ON cld_inv_w.ccTo = cld_inv_cc.ID and cld_inv_w.custid = cld_inv_cc.custid  where  cld_inv_w.custid = " & custid & " and (cld_inv_w.id Like '%" & name & "%') and cld_inv_w.flag in (1,2) " & ModPrvDep(Modpriv, user) & " Order by cld_inv_w.id"
            Else
                strFilter = "SELECT cld_inv_w.*, case cld_inv_w.flag when 1 then 'Order' WHEN 2 THEN 'Delivery' WHEN 3 THEN 'Receive' END as Status,  DATE_FORMAT(cld_inv_w.CreatedDate,'%d-%b-%Y') AS CreateD, cld_inv_cc.Name AS ccname FROM cld_inv_w LEFT JOIN cld_inv_cc ON cld_inv_w.ccTo = cld_inv_cc.ID and cld_inv_w.custid = cld_inv_cc.custid  where  cld_inv_w.custid = " & custid & " and (cld_inv_w.id Like '%" & name & "%') and cld_inv_w.flag = " & itipe & ModPrvDep(Modpriv, user) & " Order by cld_inv_w.id"
            End If
        Else
            If itipe = 0 Then
                strFilter = "SELECT cld_inv_w.*, case cld_inv_w.flag when 1 then 'Order' WHEN 2 THEN 'Delivery' WHEN 3 THEN 'Receive' END as Status,  DATE_FORMAT(cld_inv_w.CreatedDate,'%d-%b-%Y') AS CreateD, cld_inv_cc.Name AS ccname FROM cld_inv_w LEFT JOIN cld_inv_cc ON cld_inv_w.ccTo = cld_inv_cc.ID and cld_inv_w.custid = cld_inv_cc.custid  where cld_inv_w.ccTo = " & icc & " and cld_inv_w.custid = " & custid & " and (cld_inv_w.id Like '%" & name & "%') and cld_inv_w.flag in (1,2) " & ModPrvDep(Modpriv, user) & " Order by cld_inv_w.id"
            Else
                strFilter = "SELECT cld_inv_w.*, case cld_inv_w.flag when 1 then 'Order' WHEN 2 THEN 'Delivery' WHEN 3 THEN 'Receive' END as Status,  DATE_FORMAT(cld_inv_w.CreatedDate,'%d-%b-%Y') AS CreateD, cld_inv_cc.Name AS ccname FROM cld_inv_w LEFT JOIN cld_inv_cc ON cld_inv_w.ccTo = cld_inv_cc.ID and cld_inv_w.custid = cld_inv_cc.custid  where cld_inv_w.ccTo = " & icc & " and cld_inv_w.custid = " & custid & " and (cld_inv_w.id Like '%" & name & "%') and cld_inv_w.flag = " & itipe & ModPrvDep(Modpriv, user) & " Order by cld_inv_w.id"
            End If
        End If

        Dim a As New clsDbo(strFilter)

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function


    Public Function SelectDataTable_invWare(ByVal id As String, ByVal custid As Integer, ByVal ccTo As Integer, ByVal ccware As Integer) As DataTable
        Dim ds As New DataTable
        Dim strFilter As String = ""


        'Dim a As New clsDbo("SELECT *  From cld_inv_w_dtl where wID = '" & id & "' and custid = " & custid & " and ccTo = " & ccTo)
        'perubahan 14 sept 2017

        'jadi klo ada warehousenya
        If ccware <> 0 Then
            strFilter = "SELECT w_dtl.*, cld_inv_stk.Number, cld_inv_stk.Unit2, IFNULL((SELECT ttlqty FROM cld_inv_transstock WHERE stkid = w_dtl.stk_id AND ccid = " & ccware & " AND custid = " & custid & "), 0) AS stock " & _
                            "FROM cld_inv_w_dtl AS w_dtl INNER JOIN cld_inv_stk " & _
                            "ON cld_inv_stk.id = w_dtl.stk_id AND w_dtl.custid = cld_inv_stk.CustID where w_dtl.wID = '" & id & "' and w_dtl.custid = " & custid & " and w_dtl.ccTo = " & ccTo

        Else
            strFilter = "SELECT w_dtl.*, cld_inv_stk.Number, cld_inv_stk.Unit2  FROM cld_inv_w_dtl AS w_dtl INNER JOIN cld_inv_stk " & _
                            "ON cld_inv_stk.id = w_dtl.stk_id AND w_dtl.custid = cld_inv_stk.CustID where w_dtl.wID = '" & id & "' and w_dtl.custid = " & custid & " and w_dtl.ccTo = " & ccTo
        End If

        Dim a As New clsDbo(strFilter)

        ds = a.selectDatatable

        a.dispose()
        a = Nothing

        Return ds

    End Function

    Public Sub InsertInvDetail_invWare(ByVal custid As Integer, ByVal iType As Integer, ByVal iCc As Integer, ByVal iStk As Integer, ByVal sStk As String, ByVal dU1 As Decimal, ByVal dU2 As Decimal, ByVal dFactor1 As Decimal, ByVal dCost As Decimal, ByVal slinkId As String, ByVal iFtrans As Integer, ByVal sU1nm As String, ByVal sCreatedBy As String, ByVal sStkNo As String, ByVal sCcNm As String, ByVal sU2nm As String)
        Dim str As String = ""
        Dim c_cmd As New Cls_command


        If cekInvDetail_invWare(custid, iCc, iStk, slinkId) > 0 Then
            str = "update cld_Inv_Trans set  U1 = " & dU1 & ", U2 = " & dU2 & ", factor1 = " & dFactor1 & ", ttlstock = " & (dU1 * dFactor1) & ", Cost = " & dCost & ", " & _
              " ModifBy = '" & sCreatedBy & "', flagtrans = " & iFtrans & ", Modifdate = '" & Date.Now.ToString("yyyy-MM-dd HH:mm:ss") & "'  where linkid = '" & slinkId & "' and custid = " & custid & " and cc = " & iCc & " and  stk_id = " & iStk
            c_cmd.executeNonQuery(str)
        Else
            str = "insert into cld_inv_Trans values (" & c_cmd.ExecuteScalar("select Max(ID) from cld_inv_Trans") & ", " & custid & ",'" & Date.Now.Date.ToString("yyyy-MM-dd") & "'," & iType & ", " & iCc & ", " & _
                " " & iStk & ", '" & sStk & "', " & dU1 & ", " & dU2 & ", " & dFactor1 & ", " & (dU1 * dFactor1) & ", 0, " & dCost & ", " & _
                " '" & slinkId & "', " & iFtrans & ", '" & sU1nm & "', '" & sU2nm & "', '" & sCreatedBy & "', '" & Date.Now.ToString("yyyy-MM-dd HH:mm:ss") & "' , Null, Null, '" & sStkNo & "', '" & sCcNm & "')"
            c_cmd.executeNonQuery(str)
        End If

        'c_cmd.executeNonQuery_Inv_CalcCost(iStk, iCc, custid)
        'c_cmd.executeNonQuery_Inv_CalcStock(iStk, iCc, custid)

    End Sub

    Private Function cekInvDetail_invWare(ByVal custid As Integer, ByVal iCc As Integer, ByVal iStk As Integer, ByVal slinkId As String) As Integer

        Dim c_cmd As New Cls_command

        Dim sID As Integer
        sID = c_cmd.ExecuteScalar("select id from cld_inv_trans where CustID = " & custid & " and Cc = " & iCc & " and Stk_Id = " & iStk & " and Linkid = '" & slinkId & "'", 2)

        Return sID

    End Function


End Class
