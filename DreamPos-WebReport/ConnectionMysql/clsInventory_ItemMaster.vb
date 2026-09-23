Public Class clsInventory_ItemMaster

    Public Function SelectDataset_Itemmaster(ByVal custid As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""


        Dim a As New clsDbo("SELECT cld_itemmaster.*, CASE WHEN cld_itemmaster.type = 1 THEN 'Simple Inventory' ELSE 'Inventory' END AS Typs,  " &
                            "  CASE cld_itemmaster.itype WHEN  1 THEN 'Inventory' WHEN 2 THEN 'Receipe' WHEN 0 THEN '' END AS typename, CASE WHEN cld_itemmaster.itype = 1  " &
                            " THEN cld_inv_stk.Name ELSE cld_inv_rcp.Name END AS dtlName, CASE WHEN cld_itemmaster.link = '' THEN 'False' ELSE 'True' END linkname FROM cld_itemmaster " &
                            " LEFT JOIN cld_inv_stk ON cld_inv_stk.ID = cld_itemmaster.iId " &
                           " LEFT JOIN cld_inv_rcp ON cld_inv_rcp.ID = cld_itemmaster.rId  WHERE cld_itemmaster.custid = " & custid & " order by item.itemno")


        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function

    Public Function SelectDataset_Itemcost(ByVal custid As Integer, ByVal itemno As String) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""


        Dim a As New clsDbo("SELECT item.*, CASE WHEN item.type = 1 THEN 'Simple Inventory' ELSE 'Inventory' END AS Typs,  cc.name ccname,
                CASE item.itype WHEN  1 THEN 'Inventory' WHEN 2 THEN 'Receipe' WHEN 0 THEN '' END AS typename, CASE WHEN item.itype = 1   THEN item.itemName ELSE rcp.Name END AS dtlName, CASE WHEN item.link = '' THEN 'False' ELSE 'True' END linkname ,
                             IFNULL(CASE
                 WHEN item.rid IS NOT NULL AND item.itype=2 THEN rcc.cost
                 WHEN item.iid IS NOT NULL AND item.itype=1 THEN ICC.COST
                 END,0) AS cost2
                             FROM cld_itemmaster item
                 inner JOIN cld_itemmaster_cc icc ON item.itemno = icc.itemno AND item.custid = icc.custid
                 LEFT JOIN cld_inv_cc cc ON cc.custid = item.custid AND cc.id = icc.ccid
                 LEFT JOIN cld_inv_rcp rcp ON rcp.id = item.rid AND rcp.custid = item.custid
                 inner JOIN cld_inv_rcp_cc rcc ON rcc.rcpid = rcp.id AND rcc.custid = item.custid  AND rcc.ccid = cc.id
         WHERE item.custid = " & custid & " AND item.ItemNo ='" & itemno & "' order by item.ItemNo ")


        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function

    Public Function SelectDataset_ItemmasterSearch(ByVal name As String, ByVal custid As Integer, ByVal Blink As Boolean) As DataSet
        Dim ds As New DataSet
        Dim strQuery As String = ""
        Try
            'If Blink Then

            '    strQuery = "SELECT item.*, CASE WHEN item.type = 1 THEN 'Simple Inventory' ELSE 'Inventory' END AS Typs,   
            '        CASE item.itype WHEN  1 THEN 'Inventory' WHEN 2 THEN 'Receipe' WHEN 0 THEN '' END AS typename, CASE WHEN item.itype = 1   THEN item.itemName ELSE rcp.Name END AS dtlName, CASE WHEN item.link = '' THEN 'False' ELSE 'True' END linkname ,
            '                     IFNULL(CASE
            '         WHEN item.rid IS NOT NULL AND item.itype=2 THEN rcc.cost
            '         WHEN item.iid IS NOT NULL AND item.itype=1 THEN ICC.COST
            '         END,0) AS cost2
            '                     FROM cld_itemmaster item
            '         JOIN cld_itemmaster_cc icc ON item.itemno = icc.itemno AND item.custid = icc.custid
            '         JOIN cld_inv_cc cc ON cc.custid = item.custid AND cc.id = icc.ccid
            '         LEFT JOIN cld_inv_rcp rcp ON rcp.id = item.rid AND rcp.custid = item.custid
            '         LEFT JOIN cld_inv_rcp_cc rcc ON rcc.rcpid = rcp.id AND rcc.custid = item.custid  AND rcc.ccid = cc.id
            '    WHERE item.custid = " & custid & " AND (item.ItemNo like  '%" & name & "%' Or item.ItemName like '%" & name & "%') order by item.itemno "

            '    ',cc.id ccid,cc.name ccname
            'Else

            '    strQuery = "SELECT item.*, CASE WHEN item.type = 1 THEN 'Simple Inventory' ELSE 'Inventory' END AS Typs,  
            '        CASE item.itype WHEN  1 THEN 'Inventory' WHEN 2 THEN 'Receipe' WHEN 0 THEN '' END AS typename, CASE WHEN item.itype = 1   THEN item.itemName ELSE rcp.Name END AS dtlName, CASE WHEN item.link = '' THEN 'False' ELSE 'True' END linkname ,
            '                     IFNULL(CASE
            '         WHEN item.rid IS NOT NULL AND item.itype=2 THEN rcc.cost
            '         WHEN item.iid IS NOT NULL AND item.itype=1 THEN ICC.COST
            '         END,0) AS cost2
            '                     FROM cld_itemmaster item
            '         LEFT JOIN cld_itemmaster_cc icc ON item.itemno = icc.itemno AND item.custid = icc.custid
            '         LEFT JOIN cld_inv_cc cc ON cc.custid = item.custid AND cc.id = icc.ccid
            '         LEFT JOIN cld_inv_rcp rcp ON rcp.id = item.rid AND rcp.custid = item.custid
            '         LEFT JOIN cld_inv_rcp_cc rcc ON rcc.rcpid = rcp.id AND rcc.custid = item.custid  AND rcc.ccid = cc.id
            '    WHERE item.link <>1 and item.custid = " & custid & " AND (item.ItemNo like  '%" & name & "%' Or item.ItemName like '%" & name & "%') order by item.ItemNo "

            'End If


            If Blink Then
                strQuery = "SELECT item.*,(SELECT NAME FROM cld_majorgroup WHERE id=item.`majorgroup`) mgnm,(SELECT NAME FROM cld_itemgroup 
 WHERE id=item.`itemgroup`) ignm, CASE WHEN item.type = 1 THEN 'Simple Inventory' ELSE 'Inventory' END AS Typs,  
                CASE item.itype WHEN  1 THEN 'Inventory' WHEN 2 THEN 'Receipe' WHEN 0 THEN '' END AS typename, CASE WHEN item.itype = 1   
                THEN item.itemName ELSE rcp.Name END AS dtlName, 
                CASE WHEN item.link = '' THEN 'False' ELSE 'True' END linkname,CASE WHEN item.nonusage = 0 THEN 'True' ELSE 'False' END xusage
                             FROM cld_itemmaster item
                 LEFT JOIN cld_inv_rcp rcp ON rcp.id = item.rid AND rcp.custid = item.custid 
                 HAVING custid = " & custid & " AND (ItemNo LIKE '%" & name & "%' OR ItemName LIKE '%" & name & "%' OR mgnm LIKE '%" & name & "%' OR ignm LIKE '%" & name & "%') order by item.itemno "

            Else
                strQuery = "SELECT item.*,(SELECT NAME FROM cld_majorgroup WHERE id=item.`majorgroup`) mgnm,(SELECT NAME FROM cld_itemgroup 
 WHERE id=item.`itemgroup`) ignm, CASE WHEN item.type = 1 THEN 'Simple Inventory' ELSE 'Inventory' END AS Typs,  
                CASE item.itype WHEN  1 THEN 'Inventory' WHEN 2 THEN 'Receipe' WHEN 0 THEN '' END AS typename, CASE WHEN item.itype = 1   
                THEN item.itemName ELSE rcp.Name END AS dtlName, 
                CASE WHEN item.link = '' THEN 'False' ELSE 'True' END linkname,CASE WHEN item.nonusage = 0 THEN 'True' ELSE 'False' END xusage
                             FROM cld_itemmaster item
                 LEFT JOIN cld_inv_rcp rcp ON rcp.id = item.rid AND rcp.custid = item.custid
                  HAVING custid = " & custid & " AND link =1 AND (ItemNo LIKE '%" & name & "%' OR ItemName LIKE '%" & name & "%' OR mgnm LIKE '%" & name & "%' OR ignm LIKE '%" & name & "%') order by item.ItemNo "


            End If


            Dim a As New clsDbo(strQuery, 1)



            ds = a.selectDataset

            a.dispose()
            a = Nothing
        Catch ex As Exception
            errLog(Err.Description, "clsInventory_ItemMaster:SelectDataset_ItemmasterSearch")
            Err.Clear()
        End Try



        Return ds

    End Function
    Public Function SelectDT_itemCost(ByVal custid As Integer, ByVal itemno As String, Optional ByVal usr As String = "", Optional ByVal modpriv As Integer = 0) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""




        Try
            If modpriv And usrModulepriv.moduleuser = usrModulepriv.moduleuser Then

                strFilter = "SELECT item.itemno,item.custid,item.itemno,item.itemname, icc.cost cost2, cc.name AS ccname ,cc.id ccid,IFnull(icc.addcost,0) addcost
             FROM cld_itemmaster item 
                 INNER JOIN cld_itemmaster_cc icc ON icc.custid = item.custid  AND icc.itemno = item.itemno
                 LEFT JOIN cld_inv_cc cc ON cc.id = icc.ccid AND cc.custid = icc.custid
                left Join cld_outlet ON cc.Olid = cld_outlet.id and cc.custid = cld_outlet.custid
            WHERE item.custid = " & custid & "  AND item.itemno = '" & itemno & "' and olid IN (SELECT Store FROM cld_userxstore WHERE USER = '" & usr & "') ORDER BY item.itemno"
            Else
                strFilter = "SELECT item.itemno,item.custid,item.itemno,item.itemname, icc.cost cost2, cc.name AS ccname ,cc.id ccid,IFnull(icc.addcost,0) addcost
             FROM cld_itemmaster item 
                 INNER JOIN cld_itemmaster_cc icc ON icc.custid = item.custid  AND icc.itemno = item.itemno
                 LEFT JOIN cld_inv_cc cc ON cc.id = icc.ccid AND cc.custid = icc.custid
                left Join cld_outlet ON cc.Olid = cld_outlet.id and cc.custid = cld_outlet.custid
            WHERE item.custid = " & custid & "  AND item.itemno = '" & itemno & "' ORDER BY item.itemno"

            End If
            Dim a As New clsDbo(strFilter, 1)

            ds = a.selectDataset

            a.dispose()
            a = Nothing
        Catch ex As Exception
            errLog(Err.Description, "clsInventory_ItemMaster:SelectDT_itemCost")
            Err.Clear()
        End Try

        Return ds

    End Function
    Public Sub deleteItemcc(ByVal itemno As String, ByVal custid As Integer)
        Dim str As String
        Dim c_cmd As New Cls_command


        Try

            ' If My.Settings.itemmaster_cc = 1 Then
            str = "DELETE FROM cld_itemmaster_cc WHERE itemno = '" & itemno & "' AND custid = " & custid & ""
            ServerString = My.Settings.slssvr
            c_cmd.executeNonQuery(str)
            ServerString = My.Settings.ServerStr
            c_cmd = Nothing
            ' End If

        Catch ex As Exception
            errLog(Err.Description, "clsInventory_ItemMaster:deleteItemcc")
            Err.Clear()
        End Try


    End Sub


    'Public Sub Process_UpdateCostitemMaster(ByVal iCust As Integer, Optional ByVal xrid As Integer = 0)
    '    Dim dLinkStock As New DataSet
    '    Dim dsCostCentre As New DataSet

    '    Dim cData As New clsDbo("")
    '    Dim dTotal As Integer

    '    Try
    '        ' get itemmaster
    '        dLinkStock = getDataset_ItemMaster(iCust, xrid)
    '        'get costcentre
    '        If xrid > 0 Then
    '            dsCostCentre = cData.ReturnDataset("Select ccid id from cld_itemmaster_cc where custid = " & iCust & " and itemno = '" & dLinkStock.Tables(0).Rows(0).Item("itemno") & "'")
    '        Else
    '            dsCostCentre = cData.ReturnDataset("Select * from cld_inv_cc where custid = " & iCust)
    '        End If

    '        For Each data_row As DataRow In dLinkStock.Tables(0).Rows

    '            For Each data_rowCostCentre As DataRow In dsCostCentre.Tables(0).Rows
    '                dTotal = 0
    '                If IsDBNull(data_row.Item("rID")) = False And data_row.Item("rID") <> 0 Then

    '                    cData.ReNew()
    '                    dTotal = cData.ExecuteScalar("Select Cost from cld_inv_rcp_cc where rcpid = " & data_row.Item("rID") & " and ccid = " & data_rowCostCentre.Item("ID"), 2)

    '                ElseIf IsDBNull(data_row.Item("iID")) = False And data_row.Item("iID") <> 0 Then

    '                    cData.ReNew()
    '                    dTotal = get_stkCost(data_row.Item("iID"), data_rowCostCentre.Item("ID"), iCust, Now.Date)
    '                    'cData.ExecuteScalar("SELECT  Avg(cost) as Cost FROM cld_inv_stkcost where stkid = " & data_row.Item("iID") & " and Custid = " & iCust, 2)

    '                End If


    '                Set_MenuItemCost(data_row.Item("ItemNo"), dTotal, data_rowCostCentre.Item("ID"), iCust)

    '            Next

    '        Next


    '    Catch ex As Exception
    '        errLog(Err.Description, "clsInventory_ItemMaster:Process_UpdateCostitemMaster")
    '        Err.Clear()
    '    End Try


    '    cData.dispose()
    '    cData = Nothing

    '    dLinkStock.Dispose()
    '    dLinkStock = Nothing

    '    dsCostCentre.Dispose()
    '    dsCostCentre = Nothing
    'End Sub

    Public Sub Process_UpdateCostitemMaster(ByVal iCust As Integer, Optional ByVal xrid As Integer = 0)
        Dim dLinkStock As New DataSet
        Dim dsCostCentre As New DataSet

        Dim cData As New clsDbo("")
        Dim dTotal As Integer


        Try
            ' get itemmaster
            dLinkStock = getDataset_ItemMaster(iCust, xrid)
            'get costcentre
            If xrid > 0 Then
                dsCostCentre = cData.ReturnDataset("Select ccid id from cld_itemmaster_cc where custid = " & iCust & " and itemno = '" & dLinkStock.Tables(0).Rows(0).Item("itemno") & "'")
            Else
                dsCostCentre = cData.ReturnDataset("Select * from cld_inv_cc where custid = " & iCust)
            End If

            For Each data_row As DataRow In dLinkStock.Tables(0).Rows

                For Each data_rowCostCentre As DataRow In dsCostCentre.Tables(0).Rows
                    dTotal = 0
                    If IsDBNull(data_row.Item("rID")) = False And data_row.Item("rID") <> 0 Then

                        cData.ReNew()
                        dTotal = cData.ExecuteScalar("Select Cost from cld_inv_rcp_cc where rcpid = " & data_row.Item("rID") & " and ccid = " & data_rowCostCentre.Item("ID"), 2)

                    ElseIf IsDBNull(data_row.Item("iID")) = False And data_row.Item("iID") <> 0 Then

                        cData.ReNew()
                        dTotal = get_stkCost(data_row.Item("iID"), data_rowCostCentre.Item("ID"), iCust, Now.Date)
                        'cData.ExecuteScalar("SELECT  Avg(cost) as Cost FROM cld_inv_stkcost where stkid = " & data_row.Item("iID") & " and Custid = " & iCust, 2)

                    End If


                    Set_MenuItemCost(data_row.Item("ItemNo"), dTotal * data_row.Item("Factor"), data_rowCostCentre.Item("ID"), iCust)

                Next

            Next

        Catch ex As Exception
            errLog(Err.Description, "clsInventory_stkCost:Process_UpdateCostitemMaster")
            Err.Clear()
        End Try


        cData.dispose()
        cData = Nothing

        dLinkStock.Dispose()
        dLinkStock = Nothing

        dsCostCentre.Dispose()
        dsCostCentre = Nothing
    End Sub

    'Select all Receipe
    Public Function getDataset_ItemMaster(ByVal custid As Integer, Optional ByVal xrid As Integer = 0) As DataSet

        Dim ds As New DataSet

        Try
            Dim c_Cmd As New clsDbo("")
            If xrid = 0 Then
                ds = c_Cmd.ReturnDataset("Select * from cld_itemmaster where link = 1 and custid = " & custid)
            Else
                ds = c_Cmd.ReturnDataset("Select * from cld_itemmaster where link = 1 and rid = " & xrid & " and custid = " & custid)
            End If


            c_Cmd = Nothing

        Catch ex As Exception
            errLog(Err.Description, "clsInventory_ItemMaster:getDataset_ItemMaster")
            Err.Clear()
        End Try


        Return ds


    End Function
    Public Function get_stkCost(ByVal iStk As Integer, ByVal iCc As Integer, ByVal iCust As Integer, ByVal dInvDate As Date) As Decimal

        Dim hasil As Decimal = 0
        Try

            Dim cCmd As New clsDbo("")
            hasil = cCmd.ExecuteScalar("SELECT Cost FROM cld_inv_transstockcls WHERE ccid = " & iCc & " AND stkid = " & iStk & " AND invDate = '" & dInvDate.ToString("yyyy-MM-dd HH:mm:ss") & "'", 0)

            cCmd.dispose()
            cCmd = Nothing

        Catch ex As Exception
            errLog(Err.Description, "clsInventory_ItemMaster:get_stkCost")
            Err.Clear()
        End Try

        Return hasil

    End Function
    Public Sub Set_MenuItemCost(ByVal sItemNo As String, ByVal dTotalCost As Decimal, ByVal iCC As Integer, ByVal icust As Integer)

        Dim str As String = ""
        Dim c_cmd As New clsDbo("")
        Dim ihasil As Integer
        Try


            ihasil = c_cmd.ExecuteScalar("SELECT ccid FROM cld_itemmaster_cc WHERE itemNo = '" & sItemNo & "' AND ccid = " & iCC, 11)

            If ihasil <> 0 Then
                str = "update cld_itemmaster_cc set Cost = " & dTotalCost & " WHERE itemNo = '" & sItemNo & "' AND ccid = " & iCC
            Else

                If My.Settings.itemmaster_cc = 1 Then
                    str = "Insert Into cld_itemmaster_cc (ItemNo,CustID,cost,ccid) values('" & sItemNo & "'," & icust & "," & dTotalCost & "," & iCC & ")"
                End If


            End If

            If str <> "" Then
                c_cmd.executeNonQuery(str)
            End If


        Catch ex As Exception
            errLog(Err.Description, "clsInventory_ItemMaster:Set_MenuItemCost")
            Err.Clear()
        End Try

        c_cmd.dispose()
        c_cmd = Nothing

    End Sub

    Public Function SelectDataset_majorgroup(ByVal custid As Integer, Optional ByVal name As String = "") As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""


        Dim a As New clsDbo("SELECT * FROM cld_majorgroup where custid = " & custid & " and (name like  '%" & name & "%' or majorcoa like '%" & name & "%') order by majorcoa")


        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function
    Public Function SelectDataset_itemgroup(ByVal custid As Integer, Optional ByVal name As String = "") As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""


        Dim a As New clsDbo("SELECT * FROM cld_itemgroup where custid = " & custid & " and (name like  '%" & name & "%') order by name")


        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function

End Class
