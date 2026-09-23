Public Class clsDataItem

    Public Function SelectDataset_Itemmaster(ByVal custid As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""


        Dim a As New clsDbo("SELECT cld_itemmaster.*, CASE WHEN cld_itemmaster.type = 1 THEN 'Simple Inventory' ELSE 'Inventory' END AS Typs,  " & _
                            "  CASE cld_itemmaster.itype WHEN  1 THEN 'Inventory' WHEN 2 THEN 'Receipe' WHEN 0 THEN '' END AS typename, CASE WHEN cld_itemmaster.itype = 1  " & _
                            " THEN cld_inv_stk.Name ELSE cld_inv_rcp.Name END AS dtlName, CASE WHEN cld_itemmaster.link = '' THEN 'False' ELSE 'True' END linkname FROM cld_itemmaster " & _
                            " LEFT JOIN cld_inv_stk ON cld_inv_stk.ID = cld_itemmaster.iId " & _
                           " LEFT JOIN cld_inv_rcp ON cld_inv_rcp.ID = cld_itemmaster.rId  WHERE cld_itemmaster.custid = " & custid & "")


        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function

    Public Function SelectDataset_ItemMasterEx() As DataTable
        Dim ds As New DataTable
        Dim strFilter As String = ""


        Dim a As New clsDbo("Select itemNo, ItemName, price from cld_ItemMaster")

        ds = a.selectDatatable

        a.dispose()
        a = Nothing

        Return ds

    End Function
    Public Function SelectDataset_ItemMasterEx2(ByVal custid As Integer) As DataTable
        Dim ds As New DataTable
        Dim strFilter As String = ""


        Dim a As New clsDbo("SELECT item.itemno,item.itemname,ig.name ItemGroup,mg.name MajorGroup, IFNULL(cc.name,'') AS ccname
                            ,CASE WHEN item.nonusage = 0 THEN 'True' ELSE 'False' END Usagestock
             FROM cld_itemmaster item 
                 LEFT JOIN cld_itemmaster_cc icc ON icc.custid = item.custid  AND icc.itemno = item.itemno
                 LEFT JOIN cld_majorgroup mg ON mg.id = item.majorgroup AND mg.custid = item.custid
                 LEFT JOIN cld_itemgroup ig ON ig.id = item.itemgroup AND ig.custid = item.custid AND ig.majorgroup = mg.id
                 LEFT JOIN cld_inv_cc cc ON cc.id = icc.ccid AND cc.custid = icc.custid
            WHERE item.custid = " & custid & " ORDER BY item.itemno", 1)

        ds = a.selectDatatable

        a.dispose()
        a = Nothing

        Return ds

    End Function
    Public Function SelectDataset_ItemEditcc(ByVal Id As String, ByVal custid As Integer, ByVal ccid As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""


        Dim a As New clsDbo("SELECT item.*,icc.ccid,icc.cost cost2 from cld_ItemMaster item
                                LEFT JOIN cld_itemmaster_cc icc ON item.itemno = icc.itemno AND item.custid = icc.custid
                                where item.itemno = '" & Id & "' and item.custid = " & custid & " and icc.ccid = " & ccid & "")

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function
    Public Function SelectDataset_ItemEdit(ByVal Id As String, ByVal custid As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""


        Dim a As New clsDbo("SELECT ItemNo,ItemName,IFNULL(Price,0) price,IFNULL(Cost,0) cost,IFNULL(TYPE,1) TYPE,CustID,link,iType,iId,rId,Factor,IFNULL(majorgroup,0) majorgroup,IFNULL(itemgroup,0) itemgroup,IFNULL(nonusage,0) nonusage from cld_ItemMaster where itemno = '" & Id & "' and custid = " & custid & "", 1)

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function

    Public Function SelectDataset_ItemmasterSearch(ByVal name As String, ByVal custid As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""


        Dim a As New clsDbo("SELECT cld_itemmaster.*, CASE WHEN cld_itemmaster.type = 1 THEN 'Simple Inventory' ELSE 'Inventory' END AS Typs,  " & _
                            "  CASE cld_itemmaster.itype WHEN  1 THEN 'Inventory' WHEN 2 THEN 'Receipe' WHEN 0 THEN '' END AS typename, CASE WHEN cld_itemmaster.itype = 1  " & _
                            " THEN cld_inv_stk.Name ELSE cld_inv_rcp.Name END AS dtlName, CASE WHEN cld_itemmaster.link = '' THEN 'False' ELSE 'True' END linkname FROM cld_itemmaster " & _
                            " LEFT JOIN cld_inv_stk ON cld_inv_stk.ID = cld_itemmaster.iId " & _
                           " LEFT JOIN cld_inv_rcp ON cld_inv_rcp.ID = cld_itemmaster.rId  WHERE cld_itemmaster.custid = " & custid & " AND cld_itemMaster.ItemNo like  '%" & name & "%' Or cld_itemMaster.ItemName like '%" & name & "%' ")


        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function
    Public Function SelectItemNo(ByVal kode As String) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""


        Dim a As New clsDbo("SELECT * FROM cld_itemMaster item where itemno = '" & kode & "'", 1)


        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function
    Public Function SelectItemxcost(ByVal kode As String, ByVal ccid As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""


        Dim a As New clsDbo("SELECT * FROM cld_itemmaster_cc where itemno = '" & kode & "' and ccid = " & ccid & "")


        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function

    Public Function SelectDataMember(ByVal number As String, ByVal email As String, ByVal custid As Integer, Optional bByNumber As Boolean = False) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""

        'Dim a As New clsDbo("SELECT * FROM cld_Member where number = '" & number & "' and Custid = " & custid)

        'perubahan 14 agustus 2018
        If bByNumber = False Then
            strFilter = "SELECT * FROM cld_Member where email = '" & email & "' and Custid = " & custid
        Else
            strFilter = "SELECT * FROM cld_Member where Number = '" & number & "' and Custid = " & custid
        End If


        Dim a As New clsDbo(strFilter)

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function

    Public Function SelectDataCustomer(ByVal id As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""


        Dim a As New clsDbo("SELECT * FROM cld_cust where id = " & id)


        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function
    Public Function SelectDataGiftStock(ByVal Custid As Integer, ByVal outlet As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""


        Dim a As New clsDbo("SELECT * FROM cld_GiftStock where GiftID = " & Custid & " and Outletid = " & outlet & "")


        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function

    Public Function SelectDataTrans(ByVal id As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""


        Dim a As New clsDbo("SELECT * FROM cld_transactionstock where id = " & id)


        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function

    Public Function SelectDataMemberType(ByVal id As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""


        Dim a As New clsDbo("SELECT * FROM cld_MemberType where id = " & id)


        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function
    Public Function SelectDataRent(ByVal id As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""


        Dim a As New clsDbo("SELECT * FROM cld_rent where id = " & id)


        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function

    Public Function SelectDataGift(ByVal kode As String) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""


        Dim a As New clsDbo("SELECT * FROM cld_Gift where kode = '" & kode & "'")


        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function

    Public Function SelectOutlet(ByVal id As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""


        Dim a As New clsDbo("SELECT * FROM cld_Outlet where id = " & id)


        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function

    Public Function SelectFBReplay(ByVal dates As String) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""


        Dim a As New clsDbo("SELECT * FROM cld_feedbackreplay where transdate = '" & dates & "' ")


        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function
    Public Function selectccid(ByVal ccname As String) As Integer
        Dim ccid As Integer

        Dim cdbo As New Cls_command

        Try
            ccid = cdbo.ExecuteScalar("SELECT id FROM cld_inv_cc where name = '" & ccname & "'", 0)


        Catch ex As Exception

        End Try
        cdbo = Nothing
        Return ccid

    End Function
    Public Function selectmajorgrpid(ByVal mgname As String, ByVal custid As Integer) As Integer
        Dim mgid As Integer

        Dim cdbo As New Cls_command

        Try
            mgid = cdbo.ExecuteScalar("SELECT id FROM cld_majorgroup where name = '" & mgname & "' and custid = " & custid & "", 0)

        Catch ex As Exception

        End Try
        cdbo = Nothing

        Return mgid

    End Function
    Public Function selectitemgrpid(ByVal igname As String, ByVal custid As Integer) As Integer
        Dim igid As Integer

        Dim cdbo As New Cls_command

        Try
            igid = cdbo.ExecuteScalar("SELECT id FROM cld_itemgroup where name = '" & igname & "' and custid = " & custid & "", 0)

        Catch ex As Exception

        End Try
        cdbo = Nothing

        Return igid

    End Function
End Class

