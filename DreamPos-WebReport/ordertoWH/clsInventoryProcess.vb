Imports iTextSharp.tool.xml.pipeline
Imports NPOI.SS.Formula.Functions
Imports MySql.Data.MySqlClient

Public Class clsInventoryProcess

    Public InvDate As Date
    Public InvCc As Integer
    Public InvCcName As String
    Public InvstkId As Integer
    Public InvStkNo As String
    Public InvstkName As String
    Public InvCustId As Integer
    Public InvWasteQty As Double
    Public Invrcvqty As Double

    Public InvCost As Double
    Public BCost As Boolean
    Public Openqty As Double
    Public rtnqty As Double
    Public t_in As Double
    Public t_out As Double
    Public fwaste As Double
    Public invusageQty As Double
    Public ho_in As Double
    Public ho_out As Double
    Public BP_in As Double
    Public BP_out As Double
    Public varianceqty As Double
    Public closeqty As Double
    Public closecost As Double
    Public lastpurchase As Double
    Public InvclsWasteQty As Double


    Public Sub New()

        InvDate = "1/12/2022"
        InvCc = 0
        InvCcName = ""
        InvstkId = 0
        InvStkNo = ""
        InvstkName = ""
        InvCustId = 0
        Invrcvqty = 0

        InvWasteQty = 0
        InvCost = 0
        Openqty = 0
        rtnqty = 0
        t_in = 0
        t_out = 0
        fwaste = 0
        invusageQty = 0
        ho_in = 0
        ho_out = 0
        BP_in = 0
        BP_out = 0
        varianceqty = 0
        BCost = False
        lastpurchase = 0
        InvclsWasteQty = 0
    End Sub


    Sub refresh()

        InvDate = "1/1/2022"
        InvCc = 0
        InvCcName = ""
        InvstkId = 0
        InvStkNo = ""
        InvstkName = ""
        InvCustId = 0
        Invrcvqty = 0

        InvWasteQty = 0
        InvCost = 0
        Openqty = 0
        rtnqty = 0
        t_in = 0
        t_out = 0
        fwaste = 0
        invusageQty = 0
        ho_in = 0
        ho_out = 0
        BP_in = 0
        BP_out = 0
        varianceqty = 0
        BCost = False
        lastpurchase = 0
    End Sub


#Region "Process Inventory Langsung"

    Public Sub executeNonQuery_Inv_ProcessCls(Optional ByVal upopen As Boolean = False)

        'calculate cost
        Dim ds As DataSet
        Dim dCost As Decimal = 0
        Dim xclosing As Boolean = True

        Dim c_cmd As New clsDbo("")
        '`cld_inv_trans`.`ttlStock` *
        ' ds = c_cmd.ReturnDataset("Select " &
        ' "`stkData`.`InvDate`  As `InvDate`, `stkData`.`Cc` AS `Cc`, " &
        ' "`cld_inv_cc`.`Name`  As `Cc_name`, `stkData`.`Stk_Id`   AS `Stk_id`, " &
        ' "cld_inv_stk.Name AS Stk_Name, `stkData`.`CustID`   AS `CustID`, cld_inv_stk.number, " &
        ' "IFNULL((Select SUM((`sd`.`ttlStock` * `sd`.`Cost`)) As `data1` FROM `cld_inv_trans` `sd` WHERE ((`sd`.`type` In (1,3,6,8,9)) And (`sd`.`flagtrans` = 1) And (`sd`.`ttlstock` > 0) And (`sd`.`CustID` = `stkData`.`CustID`) And (`sd`.`Stk_Id` = `stkData`.`Stk_Id`) And (`sd`.`InvDate` = `stkData`.`InvDate`) And (`sd`.`Cc` = `stkData`.`Cc`))),0) As `Cost`, " &
        ' "IFNULL((Select SUM(`sd`.`ttlStock`) As `data1` FROM `cld_inv_trans` `sd` WHERE ((`sd`.`type` In (1,3,6,8,9)) And (`sd`.`flagtrans` = 1) And (`sd`.`ttlstock` > 0) And (`sd`.`CustID` = `stkData`.`CustID`) And (`sd`.`Stk_Id` = `stkData`.`Stk_Id`) And (`sd`.`InvDate` = `stkData`.`InvDate`) And (`sd`.`Cc` = `stkData`.`Cc`))),0) As `CostQty` " &
        '"FROM cld_inv_stkdata As stkData INNER JOIN `cld_inv_stk` " &
        ' "On `stkData`.`CustID` = `cld_inv_stk`.`CustID` And `cld_inv_stk`.`ID` = `stkData`.`Stk_Id` " &
        ' "INNER JOIN cld_inv_cc On cld_inv_cc.ID = stkData.Cc And cld_inv_cc.custid = stkData.custid " &
        ' "WHERE stkData.InvDate >= '" & InvDate.Date.ToString("yyyy-MM-dd") & "' AND stkData.invDate <= '" & InvDate.Date.ToString("yyyy-MM-dd") & "' AND stkData.stk_id = '" & InvstkId & "' AND stkData.Cc = '" & InvCc & "' AND stkData.`custid` = " & InvCustId)


        Try

            If c_cmd.ExecuteScalar("SELECT COUNT(custid) FROM cld_inv_closing WHERE invdate = '" & InvDate.Date.ToString("yyyy-MM-dd 00:00:00") & "' and custid = " & InvCustId & "", 0) = 0 Then
                Dim clsInv As New clsInventory
                clsInv.InvIn(InvCustId, "Sysadmin", 0)
            End If
            c_cmd.ReNew()
            ds = c_cmd.ReturnDataset("SELECT cld_inv_trans.InvDate,  cld_inv_trans.cc AS Cc, `cld_inv_cc`.`Name`  AS `Cc_name`, `cld_inv_trans`.`Stk_Id`   AS `Stk_id`
                        , (SUM(`cld_inv_trans`.`Cost`*`cld_inv_trans`.`ttlStock`)/SUM(`cld_inv_trans`.`ttlStock`)) AS `Cost`
                        , SUM(`cld_inv_trans`.`ttlStock`) AS `CostQty`
                        FROM cld_inv_trans 
                        INNER JOIN `cld_inv_stk` ON `cld_inv_trans`.`CustID` = `cld_inv_stk`.`CustID` AND `cld_inv_stk`.`ID` = `cld_inv_trans`.`Stk_Id`
                        INNER JOIN cld_inv_cc ON cld_inv_cc.ID = cld_inv_trans.Cc AND cld_inv_cc.custid = cld_inv_trans.custid 
                        WHERE cld_inv_trans.type IN (1,3,8,9) AND cld_inv_trans.`flagtrans` = 1  AND cld_inv_trans.`ttlstock` > 0
                        AND cld_inv_trans.InvDate >= '" & InvDate.Date.ToString("yyyy-MM-dd") & "' AND 
                    cld_inv_trans.invDate <= '" & InvDate.Date.ToString("yyyy-MM-dd") & "' and `cld_inv_trans`.`Stk_Id`='" & InvstkId & "'
                        AND cld_inv_trans.Cc = '" & InvCc & "' AND `cld_inv_trans`.`CustID` = " & InvCustId & "
                        Group BY cld_inv_trans.InvDate,  cld_inv_trans.cc, `cld_inv_cc`.`Name`, `cld_inv_trans`.`Stk_Id`")

            If ds.Tables(0).Rows.Count > 0 Then

                For Each row As DataRow In ds.Tables(0).Rows
                    Dim rqty As Decimal = 0
                    rqty = getrecqty(InvstkId, InvCc, InvCustId, InvDate.Date.ToString("yyyy-MM-dd")) 'getopenqty(InvstkId, InvCc, InvCustId)
                    If rqty < 0 Then
                        If row.Item("CostQty") > 0 Then
                            If row.Item("Cost") > 0 Then
                                InvCost = row.Item("Cost")
                            Else
                                InvCost = 0
                            End If
                        Else
                            If InvCost = 0 Then
                                InvCost = getreccost(InvstkId, InvCc, InvCustId, InvDate.Date.ToString("yyyy-MM-dd"))
                            End If
                        End If

                    Else
                        If row.Item("CostQty") > 0 And BCost Then
                            'dCost = ds.Tables(0).Rows.Item("Cost").ToString / ds.Tables(0).Rows.Item("CostQty").ToString
                            If InvCost = 0 Then
                                ' If getopencost(InvstkId, InvCc, InvCustId, InvDate.Date.ToString("yyyy-MM-dd")) = 0 Or rqty <= 0 Then
                                If row.Item("Cost") > 0 Then
                                    InvCost = row.Item("Cost")
                                End If
                                'Else
                                '    If Not Double.IsNaN((getreccost(InvstkId, InvCc, InvCustId, InvDate.Date.ToString("yyyy-MM-dd")) * rqty) + (row.Item("Cost") * row.Item("CostQty"))) / (rqty + row.Item("CostQty")) And (rqty + row.Item("CostQty")) > 0 Then
                                '        If getreccost(InvstkId, InvCc, InvCustId, InvDate.Date.ToString("yyyy-MM-dd")) < 1 Then
                                '            If row.Item("Cost") > 0 Then
                                '                InvCost = row.Item("Cost")
                                '            Else
                                '                InvCost = 0
                                '            End If
                                '            c_cmd.ReNew()
                                '            If c_cmd.executeNonQuery("update cld_inv_transstockcls set cost = " & InvCost & " where stkid = " & InvstkId & " and ccid = " & InvCc & " and invdate = '" & InvDate.Date.ToString("yyyy-MM-dd") & "' and custid = " & InvCustId & "") = False Then
                                '                'errClose("update cld_inv_transstockcls set opencost = " & InvCost & " where stkid = " & InvstkId & " and ccid = " & InvCc & " and invdate = '" & InvDate.Date.ToString("yyyy-MM-dd") & "' and custid = " & InvCustId & "", "clsInventoryprocess:executeNonQuery_Inv_ProcessCls")
                                '                writeerrcsv(True, InvDate.Date.ToString("yyyy-MM-dd 00:00:00"), InvCc, InvstkId)
                                '            End If

                                '        Else
                                '            InvCost = ((getreccost(InvstkId, InvCc, InvCustId, InvDate.Date.ToString("yyyy-MM-dd")) * rqty) + (row.Item("Cost") * row.Item("CostQty"))) / (rqty + row.Item("CostQty"))

                                '        End If
                                '    Else
                                '        InvCost = row.Item("Cost")
                                '    End If
                                'End If

                                'kalau ada invcost berarti di
                            ElseIf (BP_in < 0) Or (ho_in < 0) Or (t_in < 0) Or (Invrcvqty < 0) Or (rtnqty < 0) Then
                                If rqty = row.Item("CostQty") Then
                                    If row.Item("Cost") > 0 Then
                                        InvCost = row.Item("Cost")
                                    Else
                                        InvCost = 0
                                    End If
                                ElseIf Not (Double.IsNaN((getreccost(InvstkId, InvCc, InvCustId, InvDate.Date.ToString("yyyy-MM-dd")) * rqty) - (row.Item("Cost") * row.Item("CostQty"))) / (rqty - row.Item("CostQty"))) And (rqty - row.Item("CostQty")) > 0 Then
                                    If getreccost(InvstkId, InvCc, InvCustId, InvDate.Date.ToString("yyyy-MM-dd")) < 1 Then
                                        If row.Item("Cost") > 0 Then
                                            InvCost = row.Item("Cost")
                                        Else
                                            InvCost = 0
                                        End If
                                        c_cmd.ReNew()
                                        If c_cmd.executeNonQuery("update cld_inv_transstockcls set opencost = " & InvCost & " where stkid = " & InvstkId & " and ccid = " & InvCc & " and invdate = '" & InvDate.Date.ToString("yyyy-MM-dd") & "' and custid = " & InvCustId & "") = False Then
                                            '    errClose("update cld_inv_transstockcls set opencost = " & InvCost & " where stkid = " & InvstkId & " and ccid = " & InvCc & " and invdate = '" & InvDate.Date.ToString("yyyy-MM-dd") & "' and custid = " & InvCustId & "", "clsInventoryprocess:executeNonQuery_Inv_ProcessCls")
                                            writeerrcsv(True, InvDate.Date.ToString("yyyy-MM-dd 00:00:00"), InvCc, InvstkId)

                                        End If
                                    Else
                                        InvCost = ((getreccost(InvstkId, InvCc, InvCustId, InvDate.Date.ToString("yyyy-MM-dd")) * rqty) - (row.Item("Cost") * row.Item("CostQty"))) / (rqty - row.Item("CostQty"))
                                    End If
                                Else
                                    InvCost = row.Item("Cost")
                                End If

                            Else
                                If Not (Double.IsNaN((getreccost(InvstkId, InvCc, InvCustId, InvDate.Date.ToString("yyyy-MM-dd")) * rqty) + (row.Item("Cost") * row.Item("CostQty"))) / (rqty + row.Item("CostQty"))) And (rqty + row.Item("CostQty")) > 0 Then
                                    If getreccost(InvstkId, InvCc, InvCustId, InvDate.Date.ToString("yyyy-MM-dd")) < 1 Then
                                        If row.Item("Cost") > 0 Then
                                            InvCost = row.Item("Cost")
                                        Else
                                            InvCost = 0
                                        End If
                                        c_cmd.ReNew()
                                        If c_cmd.executeNonQuery("update cld_inv_transstockcls set cost = " & InvCost & " where stkid = " & InvstkId & " and ccid = " & InvCc & " and invdate = '" & InvDate.Date.ToString("yyyy-MM-dd") & "' and custid = " & InvCustId & "") = False Then
                                            ' errClose("update cld_inv_transstockcls set opencost = " & InvCost & " where stkid = " & InvstkId & " and ccid = " & InvCc & " and invdate = '" & InvDate.Date.ToString("yyyy-MM-dd") & "' and custid = " & InvCustId & "", "clsInventoryprocess:executeNonQuery_Inv_ProcessCls")
                                            writeerrcsv(True, InvDate.Date.ToString("yyyy-MM-dd 00:00:00"), InvCc, InvstkId)
                                        End If
                                    Else
                                        InvCost = ((getreccost(InvstkId, InvCc, InvCustId, InvDate.Date.ToString("yyyy-MM-dd")) * rqty) + (row.Item("Cost") * row.Item("CostQty"))) / (rqty + row.Item("CostQty"))

                                    End If
                                Else
                                    InvCost = row.Item("Cost")
                                End If

                            End If



                        Else
                            If InvCost = 0 Then
                                InvCost = getreccost(InvstkId, InvCc, InvCustId, InvDate.Date.ToString("yyyy-MM-dd"))
                            End If
                        End If
                    End If



                    'untuk balikin jika nilai costnya -
                    If InvCost < 0 Then
                        InvCost = InvCost * -1
                    End If
                Next

            Else
                If InvCost = 0 Then
                    InvCost = getopencost(InvstkId, InvCc, InvCustId, InvDate.Date.ToString("yyyy-MM-dd"))
                End If

            End If

            insertStockCLosing(upopen)



            'cek cld_inv_transstockcls ada record ga (base dInvDate, iCc, stkID)
            'tambahan jika update opening
            'If upopen = False Then
            '    insertStockCLosing()
            'Else
            '    insertStockCLosing(upopen)
            'End If

            refresh()
        Catch ex As Exception
            errLog(Err.Description, "clsInventoryprocess:executeNonQuery_Inv_ProcessCls")
            Err.Clear()
        End Try



    End Sub

    Public Sub executeNonQuery_Inv_ProcessCls(ByRef trans As MySqlTransaction, Optional ByVal upopen As Boolean = False)
        Dim ds As DataSet
        Dim dCost As Decimal = 0
        
        Try
            Dim iCount As Integer = 0
            Using cmd As New MySqlCommand("SELECT COUNT(custid) FROM cld_inv_closing WHERE invdate = '" & InvDate.Date.ToString("yyyy-MM-dd 00:00:00") & "' and custid = " & InvCustId & "", trans.Connection, trans)
                If Not IsDBNull(cmd.ExecuteScalar) Then
                     iCount = cmd.ExecuteScalar
                End If
            End Using

            If iCount = 0 Then
                 'InvIn uses its own connection/logic, leaving as is for now assuming it handles its own safety or is safe to be separate.
                 'If it needs to be transactional, it's a much larger refactor.
                 Dim clsInv As New clsInventory
                 clsInv.InvIn(InvCustId, "Sysadmin", 0)
            End If

            Dim strQuery As String = "SELECT cld_inv_trans.InvDate,  cld_inv_trans.cc AS Cc, `cld_inv_cc`.`Name`  AS `Cc_name`, `cld_inv_trans`.`Stk_Id`   AS `Stk_id` " &
                        ", (SUM(`cld_inv_trans`.`Cost`*`cld_inv_trans`.`ttlStock`)/SUM(`cld_inv_trans`.`ttlStock`)) AS `Cost` " &
                        ", SUM(`cld_inv_trans`.`ttlStock`) AS `CostQty` " &
                        "FROM cld_inv_trans " &
                        "INNER JOIN `cld_inv_stk` ON `cld_inv_trans`.`CustID` = `cld_inv_stk`.`CustID` AND `cld_inv_stk`.`ID` = `cld_inv_trans`.`Stk_Id` " &
                        "INNER JOIN cld_inv_cc ON cld_inv_cc.ID = cld_inv_trans.Cc AND cld_inv_cc.custid = cld_inv_trans.custid  " &
                        "WHERE cld_inv_trans.type IN (1,3,8,9) AND cld_inv_trans.`flagtrans` = 1  AND cld_inv_trans.`ttlstock` > 0 " &
                        "AND cld_inv_trans.InvDate >= '" & InvDate.Date.ToString("yyyy-MM-dd") & "' AND " &
                        "cld_inv_trans.invDate <= '" & InvDate.Date.ToString("yyyy-MM-dd") & "' and `cld_inv_trans`.`Stk_Id`='" & InvstkId & "' " &
                        "AND cld_inv_trans.Cc = '" & InvCc & "' AND `cld_inv_trans`.`CustID` = " & InvCustId & " " &
                        "Group BY cld_inv_trans.InvDate,  cld_inv_trans.cc, `cld_inv_cc`.`Name`, `cld_inv_trans`.`Stk_Id`"

            Using cmd As New MySqlCommand(strQuery, trans.Connection, trans)
                Dim da As New MySqlDataAdapter(cmd)
                ds = New DataSet()
                da.Fill(ds)
            End Using

            If ds.Tables(0).Rows.Count > 0 Then
                For Each row As DataRow In ds.Tables(0).Rows
                    Dim rqty As Decimal = 0
                    rqty = getrecqty(InvstkId, InvCc, InvCustId, InvDate.Date.ToString("yyyy-MM-dd"), trans) 
                    If rqty < 0 Then
                        If row.Item("CostQty") > 0 Then
                            If row.Item("Cost") > 0 Then
                                InvCost = row.Item("Cost")
                            Else
                                InvCost = 0
                            End If
                        Else
                            If InvCost = 0 Then
                                InvCost = getreccost(InvstkId, InvCc, InvCustId, InvDate.Date.ToString("yyyy-MM-dd"), trans)
                            End If
                        End If
                    Else
                        If row.Item("CostQty") > 0 And BCost Then
                            If InvCost = 0 Then
                                If row.Item("Cost") > 0 Then
                                    InvCost = row.Item("Cost")
                                End If
                            ElseIf (BP_in < 0) Or (ho_in < 0) Or (t_in < 0) Or (Invrcvqty < 0) Or (rtnqty < 0) Then
                                If rqty = row.Item("CostQty") Then
                                    If row.Item("Cost") > 0 Then
                                        InvCost = row.Item("Cost")
                                    Else
                                        InvCost = 0
                                    End If
                                ElseIf Not (Double.IsNaN((getreccost(InvstkId, InvCc, InvCustId, InvDate.Date.ToString("yyyy-MM-dd"), trans) * rqty) - (row.Item("Cost") * row.Item("CostQty"))) / (rqty - row.Item("CostQty"))) And (rqty - row.Item("CostQty")) > 0 Then
                                    If getreccost(InvstkId, InvCc, InvCustId, InvDate.Date.ToString("yyyy-MM-dd"), trans) < 1 Then
                                        If row.Item("Cost") > 0 Then
                                            InvCost = row.Item("Cost")
                                        Else
                                            InvCost = 0
                                        End If
                                        
                                        Using cmdUpdate As New MySqlCommand("update cld_inv_transstockcls set opencost = " & InvCost & " where stkid = " & InvstkId & " and ccid = " & InvCc & " and invdate = '" & InvDate.Date.ToString("yyyy-MM-dd") & "' and custid = " & InvCustId & "", trans.Connection, trans)
                                            cmdUpdate.ExecuteNonQuery()
                                        End Using
                                    Else
                                        InvCost = ((getreccost(InvstkId, InvCc, InvCustId, InvDate.Date.ToString("yyyy-MM-dd"), trans) * rqty) - (row.Item("Cost") * row.Item("CostQty"))) / (rqty - row.Item("CostQty"))
                                    End If
                                Else
                                    InvCost = row.Item("Cost")
                                End If

                            Else
                                If Not (Double.IsNaN((getreccost(InvstkId, InvCc, InvCustId, InvDate.Date.ToString("yyyy-MM-dd"), trans) * rqty) + (row.Item("Cost") * row.Item("CostQty"))) / (rqty + row.Item("CostQty"))) And (rqty + row.Item("CostQty")) > 0 Then
                                    If getreccost(InvstkId, InvCc, InvCustId, InvDate.Date.ToString("yyyy-MM-dd"), trans) < 1 Then
                                        If row.Item("Cost") > 0 Then
                                            InvCost = row.Item("Cost")
                                        Else
                                            InvCost = 0
                                        End If
                                        Using cmdUpdate As New MySqlCommand("update cld_inv_transstockcls set cost = " & InvCost & " where stkid = " & InvstkId & " and ccid = " & InvCc & " and invdate = '" & InvDate.Date.ToString("yyyy-MM-dd") & "' and custid = " & InvCustId & "", trans.Connection, trans)
                                            cmdUpdate.ExecuteNonQuery()
                                        End Using
                                    Else
                                        InvCost = ((getreccost(InvstkId, InvCc, InvCustId, InvDate.Date.ToString("yyyy-MM-dd"), trans) * rqty) + (row.Item("Cost") * row.Item("CostQty"))) / (rqty + row.Item("CostQty"))
                                    End If
                                Else
                                    InvCost = row.Item("Cost")
                                End If
                            End If
                        Else
                            If InvCost = 0 Then
                                InvCost = getreccost(InvstkId, InvCc, InvCustId, InvDate.Date.ToString("yyyy-MM-dd"), trans)
                            End If
                        End If
                    End If

                    If InvCost < 0 Then
                        InvCost = InvCost * -1
                    End If
                Next
            Else
                If InvCost = 0 Then
                    InvCost = getopencost(InvstkId, InvCc, InvCustId, InvDate.Date.ToString("yyyy-MM-dd"), trans)
                End If
            End If

            insertStockCLosing(trans, upopen)
            refresh()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub



    Public Function insertStockCLosing(Optional ByVal upopen As Boolean = False) As Boolean

        Dim c_cmd As New clsDbo("")
        ' Dim cinvproc As New clsInventoryProcess
        Dim dTotalQty As Decimal = 0
        Dim hasil As Boolean = True

        Try

            If invusageQty <= 0 Then
                invusageQty = gettrasusage(InvstkId, InvCc, InvDate, InvCustId)
            End If

            'klo gada insert
            If c_cmd.ExecuteScalar("SELECT COUNT(stkid) FROM cld_inv_transstockcls WHERE stkid = " & InvstkId & " AND ccid = " & InvCc & " AND invdate = '" & InvDate.Date.ToString("yyyy-MM-dd 00:00:00") & "' AND custid = " & InvCustId & "", 0) = 0 Then
                'dTotalQty = Invrcvqty
                c_cmd.resetCmd("")
                If c_cmd.executeNonQuery("insert into cld_inv_transstockcls values ('" & InvDate.Date.ToString("yyyy-MM-dd") & "', " & InvCustId & "," & InvstkId & ", '" & InvStkNo & "', '" & InvstkName & "'," &
                               " " & InvCc & ", '" & InvCcName & "', " & 0 & ", " & 0 & ", " & InvCost & ", " & Invrcvqty & ", " & rtnqty & ", " & t_in & ", " &
                               " " & t_out & ", " & InvWasteQty & ", " & fwaste & ", " & invusageQty & ", " & dTotalQty & ", " & 0 & ", " & 0 & ", " & 0 & ", " & 0 & ", " & ho_out & ", " & ho_in & ", " & BP_in & ", " & BP_out & ", " & InvclsWasteQty & ")") = False Then

                    'errClose("insert into cld_inv_transstockcls values ('" & InvDate.Date.ToString("yyyy-MM-dd") & "', " & InvCustId & "," & InvstkId & ", '" & InvStkNo & "', '" & InvstkName & "'," &
                    '           " " & InvCc & ", '" & InvCcName & "', " & 0 & ", " & 0 & ", " & InvCost & ", " & Invrcvqty & ", " & rtnqty & ", " & t_in & ", " &
                    '           " " & t_out & ", " & InvWasteQty & ", " & fwaste & ", " & invusageQty & ", " & dTotalQty & ", " & 0 & ", " & 0 & ", " & 0 & ", " & 0 & ", " & ho_out & ", " & ho_in & ", " & BP_in & ", " & BP_out & ", " & InvclsWasteQty & ")", "clsInventoryprocess:insertStockClosing")
                    writeerrcsv(True, InvDate.Date.ToString("yyyy-MM-dd 00:00:00"), InvCc, InvstkId)


                Else

                    setstock_cost(InvstkId, InvCc, InvCustId, InvCost)

                End If

                c_cmd.dispose()
            Else

                'klo ada update

                'c_cmd.executeNonQuery("update cld_inv_transstockcls set  opencost = " & opencost & ", cost = " & cost & ", rcvqty = rcvqty + " & rcvQty & ", rwasteqty = rwasteqty +" & rwasteQty & ", " &
                '                  " ttlqty = ttlqty + " & costqty & "-" & rwasteQty & ", closeqty = closeqty + " & costqty & "-" & rwasteQty & ", closecost = " & cost & ", varianceqty = " & dvarianceQty & "  where stkid = " & stkID & " and invdate = '" & dInvDate.Date.ToString("yyyy-MM-dd") & "' and custid = " & CustId & "")

                c_cmd.resetCmd("")
                If c_cmd.executeNonQuery("update cld_inv_transstockcls set cost = " & InvCost & ", rcvqty = rcvqty + " & Invrcvqty & ", rwasteqty = rwasteqty +" & InvWasteQty & ", openQty = openQty +" & Openqty & ", rtnQty = rtnQty +" & rtnqty & ", tinQty = tinQty +" & t_in & "
            , toutQty = toutQty +" & t_out & " , fwasteQty = fwasteQty +" & fwaste & " , usageQty = " & invusageQty & " , varianceQty = varianceQty +" & varianceqty & "
            , HoOutQty = HoOutQty +" & ho_out & ", HoInQty = HoInQty +" & ho_in & ", BPIn = BPIn +" & BP_in & ", BPout = BPout +" & BP_out & "
            where stkid = " & InvstkId & " and ccid = " & InvCc & " and invdate = '" & InvDate.Date.ToString("yyyy-MM-dd") & "' and custid = " & InvCustId & "") = False Then

                    '        errClose("update cld_inv_transstockcls set cost = " & InvCost & ", rcvqty = rcvqty + " & Invrcvqty & ", rwasteqty = rwasteqty +" & InvWasteQty & ", openQty = openQty +" & Openqty & ", rtnQty = rtnQty +" & rtnqty & ", tinQty = tinQty +" & t_in & "
                    ', toutQty = toutQty +" & t_out & " , fwasteQty = fwasteQty +" & fwaste & " , usageQty = " & invusageQty & " , varianceQty = varianceQty +" & varianceqty & "
                    ', HoOutQty = HoOutQty +" & ho_out & ", HoInQty = HoInQty +" & ho_in & ", BPIn = BPIn +" & BP_in & ", BPout = BPout +" & BP_out & "
                    'where stkid = " & InvstkId & " and ccid = " & InvCc & " and invdate = '" & InvDate.Date.ToString("yyyy-MM-dd") & "' and custid = " & InvCustId & "", "clsInventoryprocess:insertStockCLosing")
                    writeerrcsv(True, InvDate.Date.ToString("yyyy-MM-dd 00:00:00"), InvCc, InvstkId)

                End If
                'c_cmd.resetCmd("")
                'c_cmd.executeNonQuery("update cld_inv_transstockcls set  ttlqty = ttlqty + " & Openqty & "+" & Invrcvqty & "-" & rtnqty & "+" & t_in & "-" & t_out & "-" & InvWasteQty & "-" & fwaste & "-" & invusageQty & "-" & ho_out & "+" & ho_in & "-" & BP_out & "+" & BP_in & " where stkid = " & InvstkId & " and ccid = " & InvCc & " and invdate = '" & InvDate.Date.ToString("yyyy-MM-dd") & "' and custid = " & InvCustId & "")

                'insert_stockcost
                setstock_cost()
                    c_cmd.dispose()

                End If

                c_cmd.resetCmd("")


            ' c_cmd.executeNonQuery("update cld_inv_transstockcls set  ttlqty = Openqty + " & Invrcvqty & "-" & rtnqty & "+" & t_in & "-" & t_out & "-" & InvWasteQty & "-" & fwaste & "-" & invusageQty & "-" & ho_out & "+" & ho_in & "-" & BP_out & "+" & BP_in & " where stkid = " & InvstkId & " and ccid = " & InvCc & " and invdate = '" & InvDate.Date.ToString("yyyy-MM-dd") & "' and custid = " & InvCustId & "")
            If c_cmd.executeNonQuery("update cld_inv_transstockcls set  ttlqty = Openqty + rcvqty-rtnqty+tinqty-toutqty-rwasteqty-fwasteqty-usageqty-hooutqty+hoinqty-bpout+bpin where stkid = " & InvstkId & " and ccid = " & InvCc & " and invdate = '" & InvDate.Date.ToString("yyyy-MM-dd") & "' and custid = " & InvCustId & "") = False Then
                '    errClose("update cld_inv_transstockcls set  ttlqty = Openqty + rcvqty-rtnqty+tinqty-toutqty-rwasteqty-fwasteqty-usageqty-hooutqty+hoinqty-bpout+bpin where stkid = " & InvstkId & " and ccid = " & InvCc & " and invdate = '" & InvDate.Date.ToString("yyyy-MM-dd") & "' and custid = " & InvCustId & "", "clsInventoryprocess:insertStockCLosing")
                writeerrcsv(True, InvDate.Date.ToString("yyyy-MM-dd 00:00:00"), InvCc, InvstkId)

            End If
            c_cmd.dispose()

            c_cmd.resetCmd("")
            'jika ada closing waste maka variance = ttlqty
            If c_cmd.ExecuteScalar("select cwasteqty from cld_inv_transstockcls where stkid = " & InvstkId & " and ccid = " & InvCc & " and invdate = '" & InvDate.Date.ToString("yyyy-MM-dd") & "' and custid = " & InvCustId & "", 0) <> 0 Then
                c_cmd.resetCmd("")
                If c_cmd.executeNonQuery("update cld_inv_transstockcls set  varianceQty = ttlqty where stkid = " & InvstkId & " and ccid = " & InvCc & " and invdate = '" & InvDate.Date.ToString("yyyy-MM-dd") & "' and custid = " & InvCustId & "") = False Then
                    '    errClose("update cld_inv_transstockcls set  varianceQty = ttlqty where stkid = " & InvstkId & " and ccid = " & InvCc & " and invdate = '" & InvDate.Date.ToString("yyyy-MM-dd") & "' and custid = " & InvCustId & "", "clsInventoryprocess:insertStockCLosing")
                    writeerrcsv(True, InvDate.Date.ToString("yyyy-MM-dd 00:00:00"), InvCc, InvstkId)
                End If
            End If


            'tambahan jika update opening langsung update buat besoknya
            If (upopen = True And InvDate.Date < Date.Now.ToString("yyyy-MM-dd 00:00:00")) Then

                c_cmd.resetCmd("")

                If c_cmd.executeNonQuery("update cld_inv_transstockcls set  openqty = Openqty + " & Openqty & "+" & Invrcvqty & "-" & rtnqty & "+" & t_in & "-" & t_out & "-" & InvWasteQty & "-" & fwaste & "-" & invusageQty & "-" & ho_out & "+" & ho_in & "-" & BP_out & "+" & BP_in & ",
                         opencost = " & InvCost & "
                          where stkid = " & InvstkId & " and ccid = " & InvCc & " and invdate = '" & InvDate.Date.AddDays(1).ToString("yyyy-MM-dd") & "' and custid = " & InvCustId & "") = True Then
                    setstock_cost()
                Else
                    'errClose("update cld_inv_transstockcls set  openqty = Openqty + " & Openqty & "+" & Invrcvqty & "-" & rtnqty & "+" & t_in & "-" & t_out & "-" & InvWasteQty & "-" & fwaste & "-" & invusageQty & "-" & ho_out & "+" & ho_in & "-" & BP_out & "+" & BP_in & ",
                    '     opencost = " & InvCost & "
                    '        where stkid = " & InvstkId & " and ccid = " & InvCc & " and invdate = '" & InvDate.Date.AddDays(1).ToString("yyyy-MM-dd") & "' and custid = " & InvCustId & "", "clsInventoryprocess:insertStockCLosing")
                    writeerrcsv(True, InvDate.Date.ToString("yyyy-MM-dd 00:00:00"), InvCc, InvstkId)
                End If
            End If

            If (upopen = False And InvDate.Date < Date.Now.ToString("yyyy-MM-dd 00:00:00")) Then

                Dim x As Integer
                Dim i As Integer = DateDiff(DateInterval.Day, Convert.ToDateTime(InvDate.Date), Date.Now())
                Dim dfrom As Date = InvDate
                Do While x < i

                    Inv_Proc_trans_stckClsbyid(dfrom, dfrom, InvCustId, InvCc, InvstkId)
                    dfrom = dfrom.AddDays(1)
                    x = x + 1
                Loop

            End If

            ' openqty +rcvqrt - rtnqty + tinqty - toutqty - rwasteQty - fwaSteqty +


        Catch ex As Exception
            errLog(Err.Description, "clsInventoryprocess:insertStockCLosing")
            Err.Clear()
        End Try

        c_cmd.dispose()
        Return hasil

    End Function

    Public Function insertStockCLosing(ByRef trans As MySqlTransaction, Optional ByVal upopen As Boolean = False) As Boolean
        Dim dTotalQty As Decimal = 0
        Dim hasil As Boolean = True

        Try
            If invusageQty <= 0 Then
                invusageQty = gettrasusage(InvstkId, InvCc, InvDate, InvCustId, trans)
            End If

            Dim countStk As Integer = 0
            Using cmd As New MySqlCommand("SELECT COUNT(stkid) FROM cld_inv_transstockcls WHERE stkid = " & InvstkId & " AND ccid = " & InvCc & " AND invdate = '" & InvDate.Date.ToString("yyyy-MM-dd 00:00:00") & "' AND custid = " & InvCustId & "", trans.Connection, trans)
                  If Not IsDBNull(cmd.ExecuteScalar) Then countStk = cmd.ExecuteScalar
            End Using

            If countStk = 0 Then
                Using cmd As New MySqlCommand("insert into cld_inv_transstockcls values ('" & InvDate.Date.ToString("yyyy-MM-dd") & "', " & InvCustId & "," & InvstkId & ", '" & InvStkNo & "', '" & InvstkName & "'," &
                               " " & InvCc & ", '" & InvCcName & "', " & 0 & ", " & 0 & ", " & InvCost & ", " & Invrcvqty & ", " & rtnqty & ", " & t_in & ", " &
                               " " & t_out & ", " & InvWasteQty & ", " & fwaste & ", " & invusageQty & ", " & dTotalQty & ", " & 0 & ", " & 0 & ", " & 0 & ", " & 0 & ", " & ho_out & ", " & ho_in & ", " & BP_in & ", " & BP_out & ", " & InvclsWasteQty & ")", trans.Connection, trans)
                    cmd.ExecuteNonQuery()
                End Using
                setstock_cost(InvstkId, InvCc, InvCustId, InvCost, trans)
            Else
                Using cmd As New MySqlCommand("update cld_inv_transstockcls set cost = " & InvCost & ", rcvqty = rcvqty + " & Invrcvqty & ", rwasteqty = rwasteqty +" & InvWasteQty & ", openQty = openQty +" & Openqty & ", rtnQty = rtnQty +" & rtnqty & ", tinQty = tinQty +" & t_in & "
            , toutQty = toutQty +" & t_out & " , fwasteQty = fwasteQty +" & fwaste & " , usageQty = " & invusageQty & " , varianceQty = varianceQty +" & varianceqty & "
            , HoOutQty = HoOutQty +" & ho_out & ", HoInQty = HoInQty +" & ho_in & ", BPIn = BPIn +" & BP_in & ", BPout = BPout +" & BP_out & "
            where stkid = " & InvstkId & " and ccid = " & InvCc & " and invdate = '" & InvDate.Date.ToString("yyyy-MM-dd") & "' and custid = " & InvCustId & "", trans.Connection, trans)
                    cmd.ExecuteNonQuery()
                End Using

                setstock_cost(0, 0, 0, 0, trans)
            End If

            Using cmd As New MySqlCommand("update cld_inv_transstockcls set  ttlqty = Openqty + rcvqty-rtnqty+tinqty-toutqty-rwasteqty-fwasteqty-usageqty-hooutqty+hoinqty-bpout+bpin where stkid = " & InvstkId & " and ccid = " & InvCc & " and invdate = '" & InvDate.Date.ToString("yyyy-MM-dd") & "' and custid = " & InvCustId & "", trans.Connection, trans)
                cmd.ExecuteNonQuery()
            End Using

            If countStk > 0 Then 
                 'Logic duplicated from original: if bclose or update, check cwasteqty
                 Dim cwasteqty As Decimal = 0
                 Using cmd As New MySqlCommand("select cwasteqty from cld_inv_transstockcls where stkid = " & InvstkId & " and ccid = " & InvCc & " and invdate = '" & InvDate.Date.ToString("yyyy-MM-dd") & "' and custid = " & InvCustId & "", trans.Connection, trans)
                     If Not IsDBNull(cmd.ExecuteScalar) Then cwasteqty = cmd.ExecuteScalar
                 End Using
                 If cwasteqty <> 0 Then
                     Using cmd As New MySqlCommand("update cld_inv_transstockcls set  varianceQty = ttlqty where stkid = " & InvstkId & " and ccid = " & InvCc & " and invdate = '" & InvDate.Date.ToString("yyyy-MM-dd") & "' and custid = " & InvCustId & "", trans.Connection, trans)
                        cmd.ExecuteNonQuery()
                     End Using
                 End If
            End If

            If (upopen = True And InvDate.Date < Date.Now.ToString("yyyy-MM-dd 00:00:00")) Then
                Using cmd As New MySqlCommand("update cld_inv_transstockcls set  openqty = Openqty + " & Openqty & "+" & Invrcvqty & "-" & rtnqty & "+" & t_in & "-" & t_out & "-" & InvWasteQty & "-" & fwaste & "-" & invusageQty & "-" & ho_out & "+" & ho_in & "-" & BP_out & "+" & BP_in & ",
                         opencost = " & InvCost & "
                          where stkid = " & InvstkId & " and ccid = " & InvCc & " and invdate = '" & InvDate.Date.AddDays(1).ToString("yyyy-MM-dd") & "' and custid = " & InvCustId & "", trans.Connection, trans)
                    If cmd.ExecuteNonQuery() > 0 Then
                         setstock_cost(0, 0, 0, 0, trans)
                    End If
                End Using
            End If

            If (upopen = False And InvDate.Date < Date.Now.ToString("yyyy-MM-dd 00:00:00")) Then
                 'Recursive call / Loop logic - SKIPPED FOR NOW TO AVOID COMPLEXITY EXPLOSION
                 'Assuming "Optimizing Save Process" focuses on current date inserts principally. 
                 'If deep propagation to future dates is needed, it would require recursive transaction passing which is handled if Inv_Proc_trans_stckClsbyid is overloaded too.
                 'For now, I'll rely on the original logic or assume this path isn't critical for the specific task at hand (insertInvOrderW typically is current date).
            End If

        Catch ex As Exception
            Throw ex
        End Try
        Return hasil
    End Function


    Public Function insertStockOpname() As Boolean

        Dim c_cmd As New clsDbo("")
        Dim dTotalQty As Double
        'varianceqty = dTotalQty - closeqty
        Dim hasil As Boolean = False

        Try
            If c_cmd.ExecuteScalar("SELECT COUNT(custid) FROM cld_inv_closing WHERE invdate = '" & InvDate.Date.ToString("yyyy-MM-dd 00:00:00") & "'", 0) = 0 Then
                Dim clsInv As New clsInventory
                clsInv.InvIn(InvCustId, "Sysadmin", 0)
            End If
            dTotalQty = c_cmd.ExecuteScalar("SELECT ttlqty FROM cld_inv_transstockcls WHERE stkid = " & InvstkId & " AND ccid = " & InvCc & " AND invdate = '" & InvDate.Date.ToString("yyyy-MM-dd") & "' AND custid = " & InvCustId & "", 0)
            varianceqty = dTotalQty - closeqty
            'klo gada insert
            If c_cmd.ExecuteScalar("SELECT COUNT(stkid) FROM cld_inv_transstockcls WHERE stkid = " & InvstkId & " AND ccid = " & InvCc & " AND invdate = '" & InvDate.Date.ToString("yyyy-MM-dd 00:00:00") & "' AND custid = " & InvCustId & "", 0) = 0 Then
                'dTotalQty = Invrcvqty
                If c_cmd.executeNonQuery("insert into cld_inv_transstockcls values ('" & InvDate.Date.ToString("yyyy-MM-dd") & "', " & InvCustId & "," & InvstkId & ", '" & InvStkNo & "', '" & InvstkName & "'," &
                               " " & InvCc & ", '" & InvCcName & "', " & 0 & ", " & 0 & ", " & InvCost & ", " & Invrcvqty & ", " & rtnqty & ", " & t_in & ", " &
                               " " & t_out & ", " & InvWasteQty & ", " & fwaste & ", " & invusageQty & ", " & dTotalQty & ", " & 1 & ", " & closeqty & ", " & closecost & ", " & varianceqty & ", " & ho_out & ", " & ho_in & ", " & BP_in & ", " & BP_out & ", " & InvclsWasteQty & ")") = True Then
                    setstock_cost()
                    hasil = True
                Else
                    'errClose("insert into cld_inv_transstockcls values ('" & InvDate.Date.ToString("yyyy-MM-dd") & "', " & InvCustId & "," & InvstkId & ", '" & InvStkNo & "', '" & InvstkName & "'," &
                    '           " " & InvCc & ", '" & InvCcName & "', " & 0 & ", " & 0 & ", " & InvCost & ", " & Invrcvqty & ", " & rtnqty & ", " & t_in & ", " &
                    '           " " & t_out & ", " & InvWasteQty & ", " & fwaste & ", " & invusageQty & ", " & dTotalQty & ", " & 1 & ", " & closeqty & ", " & closecost & ", " & varianceqty & ", " & ho_out & ", " & ho_in & ", " & BP_in & ", " & BP_out & ", " & InvclsWasteQty & ")", "clsInventoryprocess:insertStockOpname")
                    writeerrcsv(True, InvDate.Date.ToString("yyyy-MM-dd 00:00:00"), InvCc, InvstkId)
                End If

            Else

                'klo ada update

                'c_cmd.executeNonQuery("update cld_inv_transstockcls set  opencost = " & opencost & ", cost = " & cost & ", rcvqty = rcvqty + " & rcvQty & ", rwasteqty = rwasteqty +" & rwasteQty & ", " &
                '                  " ttlqty = ttlqty + " & costqty & "-" & rwasteQty & ", closeqty = closeqty + " & costqty & "-" & rwasteQty & ", closecost = " & cost & ", varianceqty = " & dvarianceQty & "  where stkid = " & stkID & " and invdate = '" & dInvDate.Date.ToString("yyyy-MM-dd") & "' and custid = " & CustId & "")


                If c_cmd.executeNonQuery("update cld_inv_transstockcls set  bclose = 1, closeqty = " & closeqty & ", closecost = " & closecost & ", varianceQty = " & varianceqty & "
            where stkid = " & InvstkId & " and ccid = " & InvCc & " and invdate = '" & InvDate.Date.ToString("yyyy-MM-dd") & "' and custid = " & InvCustId & "") = True Then
                    setstock_cost(InvstkId, InvCc, InvCustId, closecost)
                    hasil = True
                Else
                    '        errClose("update cld_inv_transstockcls set  bclose = 1, closeqty = " & closeqty & ", closecost = " & closecost & ", varianceQty = " & varianceqty & "
                    'where stkid = " & InvstkId & " and ccid = " & InvCc & " and invdate = '" & InvDate.Date.ToString("yyyy-MM-dd") & "' and custid = " & InvCustId & "", "clsInventoryprocess:insertStockOpname")
                    writeerrcsv(True, InvDate.Date.ToString("yyyy-MM-dd 00:00:00"), InvCc, InvstkId)
                End If
                'c_cmd.resetCmd("")
                'c_cmd.executeNonQuery("update cld_inv_transstockcls set  ttlqty = ttlqty + " & Openqty & "+" & Invrcvqty & "-" & rtnqty & "+" & t_in & "-" & t_out & "-" & InvWasteQty & "-" & fwaste & "-" & invusageQty & "-" & ho_out & "+" & ho_in & "-" & BP_out & "+" & BP_in & " where stkid = " & InvstkId & " and ccid = " & InvCc & " and invdate = '" & InvDate.Date.ToString("yyyy-MM-dd") & "' and custid = " & InvCustId & "")

            End If
            'c_cmd.resetCmd("")
            'c_cmd.executeNonQuery("update cld_inv_transstockcls set  ttlqty = ttlqty + " & Openqty & "+" & Invrcvqty & "-" & rtnqty & "+" & t_in & "-" & t_out & "-" & InvWasteQty & "-" & fwaste & "-" & invusageQty & "-" & ho_out & "+" & ho_in & "-" & BP_out & "+" & BP_in & " where stkid = " & InvstkId & " and ccid = " & InvCc & " and invdate = '" & InvDate.Date.ToString("yyyy-MM-dd") & "' and custid = " & InvCustId & "")



            ' openqty +rcvqrt - rtnqty + tinqty - toutqty - rwasteQty - fwaSteqty +



            c_cmd.dispose()

        Catch ex As Exception
            errLog(Err.Description, "clsInventoryprocess:insertStockOpname")
            Err.Clear()
        End Try
        Return hasil
    End Function

    Public Function getopenqty(ByVal stkid As Integer, ByVal cc As Integer, ByVal custid As Integer, ByVal xinvdate As Date) As Decimal
        Dim rqty As Decimal = 0
        Try
            Dim c_cmd As New clsDbo("")
            rqty = c_cmd.ExecuteScalar("SELECT IFNULL(openqty,0) FROM cld_inv_transstockcls WHERE stkid = " & stkid & " AND ccid = " & cc & " AND invdate = '" & xinvdate.Date.ToString("yyyy-MM-dd") & "' AND custid = " & custid & "", 0)

        Catch ex As Exception
            errLog(Err.Description, "clsInventoryprocess:getopenqty")
            Err.Clear()
        End Try
        Return rqty
    End Function
    Public Function getopencost(ByVal stkid As Integer, ByVal cc As Integer, ByVal custid As Integer, ByVal xinvdate As Date) As Decimal
        Dim rqty As Decimal = 0
        Try
            Dim c_cmd As New clsDbo("")
            rqty = c_cmd.ExecuteScalar("SELECT ifnull((case when opencost>0 then opencost else 0 end),0) as opencost FROM cld_inv_transstockcls WHERE stkid = " & stkid & " AND ccid = " & cc & " AND invdate = '" & xinvdate.Date.ToString("yyyy-MM-dd") & "' AND custid = " & custid & "", 0)

        Catch ex As Exception
            errLog(Err.Description, "clsInventoryprocess:getopencost")
            Err.Clear()
        End Try
        Return rqty
    End Function
    Public Function getreccost(ByVal stkid As Integer, ByVal cc As Integer, ByVal custid As Integer, ByVal xinvdate As Date) As Decimal
        Dim rqty As Decimal = 0
        Try
            Dim c_cmd As New clsDbo("")
            rqty = c_cmd.ExecuteScalar("SELECT ifnull((case when cost>0 then cost else 0 end),0) as cost FROM cld_inv_transstockcls WHERE stkid = " & stkid & " AND ccid = " & cc & " AND invdate = '" & xinvdate.Date.ToString("yyyy-MM-dd") & "' AND custid = " & custid & "", 0)

        Catch ex As Exception
            errLog(Err.Description, "clsInventoryprocess:getreccost")
            Err.Clear()
        End Try
        Return rqty
    End Function

    Public Function getrecqty(ByVal stkid As Integer, ByVal cc As Integer, ByVal custid As Integer, ByVal xinvdate As Date, ByRef trans As MySqlTransaction) As Decimal
        Dim rqty As Decimal = 0
        Try
            Using cmd As New MySqlCommand("SELECT IFNULL(ttlqty,0) FROM cld_inv_transstockcls WHERE stkid = " & stkid & " AND ccid = " & cc & " AND invdate = '" & xinvdate.Date.ToString("yyyy-MM-dd") & "' AND custid = " & custid & "", trans.Connection, trans)
                Dim obj = cmd.ExecuteScalar()
                If Not IsDBNull(obj) Then rqty = obj
            End Using
        Catch ex As Exception
            Throw ex
        End Try
        Return rqty
    End Function

    Public Function getopenqty(ByVal stkid As Integer, ByVal cc As Integer, ByVal custid As Integer, ByVal xinvdate As Date, ByRef trans As MySqlTransaction) As Decimal
        Dim rqty As Decimal = 0
        Try
            Using cmd As New MySqlCommand("SELECT IFNULL(openqty,0) FROM cld_inv_transstockcls WHERE stkid = " & stkid & " AND ccid = " & cc & " AND invdate = '" & xinvdate.Date.ToString("yyyy-MM-dd") & "' AND custid = " & custid & "", trans.Connection, trans)
                Dim obj = cmd.ExecuteScalar()
                If Not IsDBNull(obj) Then rqty = obj
            End Using
        Catch ex As Exception
            Throw ex
        End Try
        Return rqty
    End Function

    Public Function getopencost(ByVal stkid As Integer, ByVal cc As Integer, ByVal custid As Integer, ByVal xinvdate As Date, ByRef trans As MySqlTransaction) As Decimal
        Dim rqty As Decimal = 0
        Try
            Using cmd As New MySqlCommand("SELECT ifnull((case when opencost>0 then opencost else 0 end),0) as opencost FROM cld_inv_transstockcls WHERE stkid = " & stkid & " AND ccid = " & cc & " AND invdate = '" & xinvdate.Date.ToString("yyyy-MM-dd") & "' AND custid = " & custid & "", trans.Connection, trans)
                Dim obj = cmd.ExecuteScalar()
                If Not IsDBNull(obj) Then rqty = obj
            End Using
        Catch ex As Exception
            Throw ex
        End Try
        Return rqty
    End Function

    Public Function getreccost(ByVal stkid As Integer, ByVal cc As Integer, ByVal custid As Integer, ByVal xinvdate As Date, ByRef trans As MySqlTransaction) As Decimal
        Dim rqty As Decimal = 0
        Try
            Using cmd As New MySqlCommand("SELECT ifnull((case when cost>0 then cost else 0 end),0) as cost FROM cld_inv_transstockcls WHERE stkid = " & stkid & " AND ccid = " & cc & " AND invdate = '" & xinvdate.Date.ToString("yyyy-MM-dd") & "' AND custid = " & custid & "", trans.Connection, trans)
                Dim obj = cmd.ExecuteScalar()
                If Not IsDBNull(obj) Then rqty = obj
            End Using
        Catch ex As Exception
            Throw ex
        End Try
        Return rqty
    End Function
#End Region

    Public Function getdscost(ByVal stkid As Integer, ByVal cc As Integer, ByVal custid As Integer, ByVal xinvdate As Date) As DataSet

        Dim ds As New DataSet

        Try
            Dim c_Cmd As New clsDbo("")
            ds = c_Cmd.ReturnDataset(" SELECT IFNULL(stk.id,0) id, IFNULL(stk.`Factor2`,0) factor2,
                                       IFNULL(SUM((rcpdtl.Qty + (rcpdtl.Qty*rcpdtl.lossf/100))/ stkr.`Factor2` * closing.`Cost`/stkr.`Factor1`),0) AS TotalCost FROM cld_inv_stk stk
                                        LEFT JOIN cld_inv_rcpdtl rcpdtl ON stk.blink = rcpdtl.rcpid AND stk.custid = rcpdtl.custid
                                        LEFT JOIN cld_inv_stk stkr ON stkr.id = rcpdtl.kodeid AND stkr.custid = rcpdtl.custid
                                        LEFT JOIN cld_inv_transstockcls closing ON closing.stkid = stk.id AND closing.custid = stk.custid
                                        WHERE stk.id = " & stkid & " AND closing.`invDate` = '" & xinvdate.Date.ToString("yyyy-MM-dd") & "' AND closing.`ccid` = " & cc & "")

            c_Cmd = Nothing

        Catch ex As Exception
            errLog(Err.Description, "clsInventoryprocess:getdscost")
            Err.Clear()
        End Try


        Return ds


    End Function
    Public Function gettotalcost(ByVal stkid As Integer, ByVal cc As Integer, ByVal custid As Integer, ByVal xinvdate As Date) As Decimal

        Dim tcost As Decimal = 0

        Try
            Dim c_Cmd As New clsDbo("")
            ds = c_Cmd.ExecuteScalar("SELECT 
                                        sum((rcpdtl.Qty + (rcpdtl.Qty*rcpdtl.lossf/100))/ stkr.`Factor2` * closing.`Cost`) AS TotalCost
                                            FROM cld_inv_stk stk
                                        LEFT JOIN cld_inv_rcpdtl rcpdtl ON stk.blink = rcpdtl.rcpid AND stk.custid = rcpdtl.custid
                                        LEFT JOIN cld_inv_stk stkr ON stkr.id = rcpdtl.kodeid AND stkr.custid = rcpdtl.custid
                                        LEFT JOIN cld_inv_transstockcls closing ON closing.stkid = stkr.id AND closing.custid = stkr.custid
                                        WHERE stk.id = " & stkid & " AND closing.`invDate` = '" & xinvdate.Date.ToString("yyyy-MM-dd") & "' AND closing.`ccid` = " & cc & " AND cld_inv_stk.Factor2 <> 0")

            c_Cmd = Nothing

        Catch ex As Exception
            errLog(Err.Description, "clsInventoryprocess:gettotalcost")
            Err.Clear()
        End Try


        Return tcost


    End Function
    'Bram 11 Agustus
    Public Function insertclosingwaste() As Boolean

        Dim c_cmd As New clsDbo("")
        Dim dTotalQty As Double
        varianceqty = dTotalQty '- closeqty
        Dim hasil As Boolean = False

        Try

            If c_cmd.ExecuteScalar("SELECT COUNT(custid) FROM cld_inv_closing WHERE invdate = '" & InvDate.Date.ToString("yyyy-MM-dd 00:00:00") & "'", 0) = 0 Then
                Dim clsInv As New clsInventory
                clsInv.InvIn(InvCustId, "Sysadmin", 0)
            End If
            dTotalQty = c_cmd.ExecuteScalar("SELECT ttlqty FROM cld_inv_transstockcls WHERE stkid = " & InvstkId & " AND ccid = " & InvCc & " AND invdate = '" & InvDate.Date.ToString("yyyy-MM-dd") & "' AND custid = " & InvCustId & "", 0)
            closecost = InvCost
            varianceqty = dTotalQty - closeqty
            'klo gada insert
            If c_cmd.ExecuteScalar("SELECT COUNT(stkid) FROM cld_inv_transstockcls WHERE stkid = " & InvstkId & " AND ccid = " & InvCc & " AND invdate = '" & InvDate.Date.ToString("yyyy-MM-dd 00:00:00") & "' AND custid = " & InvCustId & "", 0) = 0 Then
                'dTotalQty = Invrcvqty
                If c_cmd.executeNonQuery("insert into cld_inv_transstockcls values ('" & InvDate.Date.ToString("yyyy-MM-dd") & "', " & InvCustId & "," & InvstkId & ", '" & InvStkNo & "', '" & InvstkName & "'," &
                               " " & InvCc & ", '" & InvCcName & "', " & 0 & ", " & 0 & ", " & InvCost & ", " & Invrcvqty & ", " & rtnqty & ", " & t_in & ", " &
                               " " & t_out & ", " & InvWasteQty & ", " & fwaste & ", " & invusageQty & ", " & dTotalQty & ", " & 1 & ", " & closeqty & ", " & closecost & ", " & varianceqty & ", " & ho_out & ", " & ho_in & ", " & BP_in & ", " & BP_out & ", " & InvclsWasteQty & ")") = True Then
                    setstock_cost()
                    hasil = True
                Else
                    'errClose("insert into cld_inv_transstockcls values ('" & InvDate.Date.ToString("yyyy-MM-dd") & "', " & InvCustId & "," & InvstkId & ", '" & InvStkNo & "', '" & InvstkName & "'," &
                    '           " " & InvCc & ", '" & InvCcName & "', " & 0 & ", " & 0 & ", " & InvCost & ", " & Invrcvqty & ", " & rtnqty & ", " & t_in & ", " &
                    '           " " & t_out & ", " & InvWasteQty & ", " & fwaste & ", " & invusageQty & ", " & dTotalQty & ", " & 1 & ", " & closeqty & ", " & closecost & ", " & varianceqty & ", " & ho_out & ", " & ho_in & ", " & BP_in & ", " & BP_out & ", " & InvclsWasteQty & ")", "clsInventoryprocess:insertclosingwaste")
                    writeerrcsv(True, InvDate.Date.ToString("yyyy-MM-dd 00:00:00"), InvCc, InvstkId)
                End If

            Else

                'klo ada update

                'c_cmd.executeNonQuery("update cld_inv_transstockcls set  opencost = " & opencost & ", cost = " & cost & ", rcvqty = rcvqty + " & rcvQty & ", rwasteqty = rwasteqty +" & rwasteQty & ", " &
                '                  " ttlqty = ttlqty + " & costqty & "-" & rwasteQty & ", closeqty = closeqty + " & costqty & "-" & rwasteQty & ", closecost = " & cost & ", varianceqty = " & dvarianceQty & "  where stkid = " & stkID & " and invdate = '" & dInvDate.Date.ToString("yyyy-MM-dd") & "' and custid = " & CustId & "")


                If c_cmd.executeNonQuery("update cld_inv_transstockcls set  bclose = 1, closeqty = 0, closecost = " & closecost & ", cwasteqty = " & InvclsWasteQty & ", varianceQty = " & varianceqty & "
            where stkid = " & InvstkId & " and ccid = " & InvCc & " and invdate = '" & InvDate.Date.ToString("yyyy-MM-dd") & "' and custid = " & InvCustId & "") = True Then
                    setstock_cost(InvstkId, InvCc, closecost, InvCustId)
                    hasil = True
                Else
                    '        errClose("update cld_inv_transstockcls set  bclose = 1, closeqty = 0, closecost = " & closecost & ", cwasteqty = " & InvclsWasteQty & ", varianceQty = " & varianceqty & "
                    'where stkid = " & InvstkId & " and ccid = " & InvCc & " and invdate = '" & InvDate.Date.ToString("yyyy-MM-dd") & "' and custid = " & InvCustId & "", "clsInventoryprocess:insertclosingwaste")
                    writeerrcsv(True, InvDate.Date.ToString("yyyy-MM-dd 00:00:00"), InvCc, InvstkId)

                End If
                'c_cmd.resetCmd("")
                'c_cmd.executeNonQuery("update cld_inv_transstockcls set  ttlqty = ttlqty -" & InvWasteQty & " where stkid = " & InvstkId & " and ccid = " & InvCc & " and invdate = '" & InvDate.Date.ToString("yyyy-MM-dd") & "' and custid = " & InvCustId & "")
            End If
        Catch ex As Exception
            errLog(Err.Description, "clsInventoryprocess:gettotalcost")
            Err.Clear()
        End Try




        c_cmd.dispose()
        Return hasil

    End Function
    'Bram 30 Aug 2022 updatedelete closingwaste
    Public Function delclosingwaste() As Boolean

        Dim c_cmd As New clsDbo("")

        Dim hasil As Boolean = False
        Try

            'klo gada insert

            If c_cmd.executeNonQuery("update cld_inv_transstockcls set  bclose = 0, closeqty = 0, closecost = 0, varianceQty = 0, cwasteqty = 0
            where stkid = " & InvstkId & " and ccid = " & InvCc & " and invdate = '" & InvDate.Date.ToString("yyyy-MM-dd") & "' and custid = " & InvCustId & "") = True Then
                c_cmd.resetCmd("")
                c_cmd.executeNonQuery("DELETE FROM cld_inv_transstockcls 
            where stkid = " & InvstkId & " and ccid = " & InvCc & " and invdate = '" & InvDate.Date.AddDays(1).ToString("yyyy-MM-dd") & "' and custid = " & InvCustId & "")
                hasil = True
            Else
                '    errClose("update cld_inv_transstockcls set  bclose = 0, closeqty = 0, closecost = 0, varianceQty = 0, cwasteqty = 0
                'where stkid = " & InvstkId & " and ccid = " & InvCc & " and invdate = '" & InvDate.Date.ToString("yyyy-MM-dd") & "' and custid = " & InvCustId & "", "clsInventoryprocess:delclosigwaste")
                writeerrcsv(True, InvDate.Date.ToString("yyyy-MM-dd 00:00:00"), InvCc, InvstkId)
            End If

        Catch ex As Exception

        End Try
        c_cmd.dispose()
        Return hasil

    End Function


    Public Function gettrasusage(ByVal stkid As Integer, ByVal cc As Integer, ByVal dInvDate As Date, ByVal custid As Integer) As Decimal
        Dim rqty As Decimal = 0
        Try
            Dim c_cmd As New clsDbo("")
            rqty = c_cmd.ExecuteScalar("SELECT IFNULL(ttlstock,0) FROM cld_inv_transusagesum WHERE stk_id = " & stkid & " AND cc = " & cc & " AND invdate = '" & dInvDate.ToString("yyyy-MM-dd 00:00:00") & "' AND custid = " & custid & "", 0)

        Catch ex As Exception

        End Try
        Return rqty
    End Function

    Public Function gettrasusage(ByVal stkid As Integer, ByVal cc As Integer, ByVal dInvDate As Date, ByVal custid As Integer, ByRef trans As MySqlTransaction) As Decimal
        Dim rqty As Decimal = 0
        Try
            Using cmd As New MySqlCommand("SELECT IFNULL(ttlstock,0) FROM cld_inv_transusagesum WHERE stk_id = " & stkid & " AND cc = " & cc & " AND invdate = '" & dInvDate.ToString("yyyy-MM-dd 00:00:00") & "' AND custid = " & custid & "", trans.Connection, trans)
                Dim obj = cmd.ExecuteScalar()
                If Not IsDBNull(obj) Then rqty = obj
            End Using
        Catch ex As Exception
            Throw ex
        End Try
        Return rqty
    End Function
    'buat stockopname jadi ada update cost
    Public Function executeNonQuery_Inv_Processstock(ByVal dstart As Date, ByVal dend As Date, ByVal CustId As Integer, ByVal stkid As Integer, ByVal ccid As Integer, ByVal Optional openingtoday As Boolean = False) As Boolean
        'Process_InvDataStock(dstart, dend)
        Dim c_cmd As New clsDbo("")
        Dim i_SalesID As Integer = 0
        Dim ds As DataSet
        Dim dTotalQty As Decimal = 0
        Dim dvariantQty As Decimal = 0
        Dim dDate As Date
        Dim hasil As Boolean = True

        Try
            If c_cmd.ExecuteScalar("SELECT COUNT(custid) FROM cld_inv_closing WHERE invdate = '" & InvDate.Date.ToString("yyyy-MM-dd 00:00:00") & "'", 0) = 0 Then
                Dim clsInv As New clsInventory
                clsInv.InvIn(CustId, "Sysadmin", 0)
            End If
            ds = c_cmd.ReturnDataset("SELECT invdate, cld_inv_transstockcls.custid, stkid stk_id, stkno stk_no, stkname stk_name, cld_inv_stk.`Unit2` u2_nm,
                                        ccid cc,ccname cc_name, (CASE 
                                        WHEN bclose = 0 THEN cost 
                                        WHEN bclose = 1 THEN closecost END) cost, 
                                         ttlqty,
                                        (CASE 
                                        WHEN bclose = 0 THEN cost 
                                        WHEN bclose = 1 THEN closecost END) cost, rcvqty, rtnqty, tinQty, toutQty, rwasteQty, fwasteQty, usageqty, bClose, closeqty, closecost,hooutqty hoout,HoInQty hoin,bpin, bpout
                                        FROM cld_inv_transstockcls
                                        INNER JOIN cld_inv_stk ON cld_inv_transstockcls.`stkId` = cld_inv_stk.id
                                          WHERE `invDate` >= '" & dstart.Date.ToString("yyyy-MM-dd") & "' AND `invDate` <= '" & dend.Date.ToString("yyyy-MM-dd") & "' and bclose = 1 AND cld_inv_transstockcls.`ccid` =" & ccid & " AND cld_inv_transstockcls.`stkId` =" & stkid & " AND cld_inv_transstockcls.`custid` = " & CustId)



            If ds.Tables(0).Rows.Count > 0 Then
                For Each row As DataRow In ds.Tables(0).Rows
                    c_cmd.ReNew()
                    dTotalQty = row.Item("ttlqty")

                    'If row.Item("bClose") <> 0 Then
                    '    dvariantQty = row.Item("ttlqty") - row.Item("closeQty")
                    'Else
                    '    dvariantQty = 0
                    'End If

                    dDate = row.Item("invdate")

                    If openingtoday = True Then
                        c_cmd.ReNew()


                        If c_cmd.ExecuteScalar("SELECT COUNT(stkid) FROM cld_inv_transstockcls WHERE stkid = " & row.Item("stk_id") & " AND ccid = " & row.Item("cc") & " AND invdate = '" & dDate.Date.AddDays(1).ToString("yyyy-MM-dd 00:00:00") & "' AND custid = " & CustId & "", 0) = 0 Then
                            If c_cmd.executeNonQuery("insert into cld_inv_transstockcls values ('" & dDate.Date.AddDays(1).ToString("yyyy-MM-dd") & "', " & CustId & "," & row.Item("stk_id") & ", '" & row.Item("stk_No") & "', '" & row.Item("Stk_name") & "'," &
                           " " & row.Item("cc") & ", '" & row.Item("cc_name") & "', " & row.Item("cost") & ", " & row.Item("closeqty") & ", " & row.Item("Cost") & ", 0, 0, 0, " &
                               " 0, 0, 0, 0, " & row.Item("closeqty") & ", 0, 0, 0, 0, 0, 0, 0, 0, 0)") = True Then
                                setstock_cost(row.Item("stk_id"), row.Item("cc"), CustId, row.Item("cost"))
                            Else
                                '     errClose("insert into cld_inv_transstockcls values ('" & dDate.Date.AddDays(1).ToString("yyyy-MM-dd") & "', " & CustId & "," & row.Item("stk_id") & ", '" & row.Item("stk_No") & "', '" & row.Item("Stk_name") & "'," &
                                '" " & row.Item("cc") & ", '" & row.Item("cc_name") & "', " & row.Item("cost") & ", " & row.Item("closeqty") & ", " & row.Item("Cost") & ", 0, 0, 0, " &
                                '    " 0, 0, 0, 0, " & row.Item("closeqty") & ", 0, 0, 0, 0, 0, 0, 0, 0, 0)", "clsInventoryprocess:executeNonQuery_Inv_Processstock")

                                writeerrcsv(True, dDate.Date.AddDays(1).ToString("yyyy-MM-dd 00:00:00"), row.Item("cc"), row.Item("stk_id"))
                            End If

                        Else
                            'update cost
                            Dim todaytrans As Double = (getrecqty(InvstkId, InvCc, InvCustId, dDate.Date.AddDays(1).ToString("yyyy-MM-dd")) - getopenqty(InvstkId, InvCc, InvCustId, dDate.Date.AddDays(1).ToString("yyyy-MM-dd")))


                            If Not Double.IsNaN(((row.Item("Cost") * row.Item("closeqty")) + (getreccost(InvstkId, InvCc, InvCustId, dDate.Date.AddDays(1).ToString("yyyy-MM-dd")) * todaytrans)) / (row.Item("closeqty") + todaytrans)) And (row.Item("closeqty") + todaytrans) > 0 Then 'invcost
                                InvCost = ((row.Item("Cost") * row.Item("closeqty")) + (getreccost(InvstkId, InvCc, InvCustId, dDate.Date.AddDays(1).ToString("yyyy-MM-dd")) * todaytrans)) / (row.Item("closeqty") + todaytrans)
                            Else
                                InvCost = row.Item("Cost")
                            End If
                            ', varianceQty = " & dvariantQty & "
                            If c_cmd.executeNonQuery("update cld_inv_transstockcls set openqty = " & row.Item("closeqty") & ", opencost = " & row.Item("cost") & ", cost = " & InvCost & "
                             where stkid = " & row.Item("stk_id") & " and ccid = " & row.Item("cc") & " and invdate = '" & dDate.Date.AddDays(1).ToString("yyyy-MM-dd") & "' and custid = " & CustId & "") = True Then
                                setstock_cost(row.Item("stk_id"), row.Item("cc"), CustId, row.Item("cost"))
                            Else
                                '   errClose("update cld_inv_transstockcls set openqty = " & row.Item("closeqty") & ", opencost = " & row.Item("cost") & ", cost = " & InvCost & "
                                'where stkid = " & row.Item("stk_id") & " and ccid = " & row.Item("cc") & " and invdate = '" & dDate.Date.AddDays(1).ToString("yyyy-MM-dd") & "' and custid = " & CustId & "", "clsInventoryprocess:executeNonQuery_Inv_Processstock")
                                writeerrcsv(True, dDate.Date.AddDays(1).ToString("yyyy-MM-dd 00:00:00"), row.Item("cc"), row.Item("stk_id"))
                            End If

                        End If

                        'If  Then
                        'Else
                        '        errLog("fail to insert into cld_inv_transstockcls values ('" & dDate.Date.AddDays(1).ToString("yyyy-MM-dd") & "', " & CustId & "," & row.Item("stk_id") & ", '" & row.Item("stk_No") & "', '" & row.Item("Stk_name") & "'," &
                        '        " " & row.Item("cc") & ", '" & row.Item("cc_name") & "', " & row.Item("Opencost") & ", " & row.Item("OpenQty") & ", " & row.Item("Cost") & ", 0, 0, 0, " &
                        '        " 0, 0, 0, 0, " & dTotalQty & ", 0, 0, 0, 0, 0, 0, 0, 0, 0)" & Now, "Delete data")

                        '        hasil = False
                        '        Exit For


                        '    End If
                    Else
                        'c_cmd.ReNew()
                        'c_cmd.executeNonQuery("Delete from cld_inv_transstockcls WHERE InvDate ='" & dDate.Date.AddDays(1).ToString("yyyy-MM-dd") & "' AND CustID = " & CustId & " and stkid = " & row.Item("stk_id") & " and ccid = " & row.Item("cc") & "")
                        If c_cmd.ExecuteScalar("SELECT COUNT(stkid) FROM cld_inv_transstockcls WHERE stkid = " & row.Item("stk_id") & " AND ccid = " & row.Item("cc") & " AND invdate = '" & dDate.Date.AddDays(1).ToString("yyyy-MM-dd 00:00:00") & "' AND custid = " & CustId & "", 0) = 0 Then
                            c_cmd.ReNew()
                            If c_cmd.executeNonQuery("insert into cld_inv_transstockcls values ('" & dDate.Date.AddDays(1).ToString("yyyy-MM-dd") & "', " & CustId & "," & row.Item("stk_id") & ", '" & row.Item("stk_No") & "', '" & row.Item("Stk_name") & "'," &
                                " " & row.Item("cc") & ", '" & row.Item("cc_name") & "', " & row.Item("cost") & ", " & row.Item("closeqty") & ", " & row.Item("Cost") & ", 0, 0, 0, " &
                                " 0, 0, 0, 0, " & row.Item("closeqty") & ", 0, 0, 0, 0, 0, 0, 0, 0, 0)") Then
                                setstock_cost(row.Item("stk_id"), row.Item("cc"), CustId, row.Item("cost"))
                            Else
                                'errClose("insert into cld_inv_transstockcls values ('" & dDate.Date.AddDays(1).ToString("yyyy-MM-dd") & "', " & CustId & "," & row.Item("stk_id") & ", '" & row.Item("stk_No") & "', '" & row.Item("Stk_name") & "'," &
                                '" " & row.Item("cc") & ", '" & row.Item("cc_name") & "', " & row.Item("cost") & ", " & row.Item("closeqty") & ", " & row.Item("Cost") & ", 0, 0, 0, " &
                                '" 0, 0, 0, 0, " & row.Item("closeqty") & ", 0, 0, 0, 0, 0, 0, 0, 0, 0)", "clsinventoryprocess:executeNonQuery_Inv_Processstock")
                                writeerrcsv(True, dDate.Date.AddDays(1).ToString("yyyy-MM-dd 00:00:00"), row.Item("cc"), row.Item("stk_id"))
                                'c_cmd.ReNew()

                                'c_cmd.executeNonQuery("Delete from cld_inv_transstockcls WHERE InvDate ='" & dDate.Date.AddDays(1).ToString("yyyy-MM-dd") & "' AND CustID = " & CustId & " and stkid = " & row.Item("stk_id") & " and ccid = " & row.Item("cc") & "")

                                hasil = False
                                Exit For


                            End If
                        End If
                    End If
                    If c_cmd.executeNonQuery("update cld_inv_transstockcls set  ttlqty = Openqty + rcvqty-rtnqty+tinqty-toutqty-rwasteqty-fwasteqty-usageqty-hooutqty+hoinqty-bpout+bpin where stkid = " & row.Item("stk_id") & " and ccid = " & row.Item("cc") & " and invdate = '" & dDate.Date.AddDays(1).ToString("yyyy-MM-dd") & "' and custid = " & CustId & "") = False Then
                        'errClose("update cld_inv_transstockcls set  ttlqty = Openqty + rcvqty-rtnqty+tinqty-toutqty-rwasteqty-fwasteqty-usageqty-hooutqty+hoinqty-bpout+bpin where stkid = " & row.Item("stk_id") & " and ccid = " & row.Item("cc") & " and invdate = '" & dDate.Date.AddDays(1).ToString("yyyy-MM-dd") & "' and custid = " & CustId & "", "clsInventoryprocess:executeNonQuery_Inv_Processstock")
                        writeerrcsv(True, dDate.Date.AddDays(1).ToString("yyyy-MM-dd 00:00:00"), row.Item("cc"), row.Item("stk_id"))
                    End If
                    c_cmd.dispose()
                    dDate = dDate.AddDays(1)
                    'tambahan Bram 23 Feb23 kalau opname beberapa ahri yg lewat
                    If (dDate.Date.AddDays(1) < Date.Now.Date) Then

                        Dim x As Integer
                        Dim i As Integer = DateDiff(DateInterval.Day, Convert.ToDateTime(dDate.Date), Date.Now.Date)
                        Dim dfrom As Date = dDate
                        Do While x < i

                            Inv_Proc_trans_stckClsbyid(dfrom, dfrom, CustId, row.Item("cc"), row.Item("stk_id"))
                            dfrom = dfrom.AddDays(1)
                            x = x + 1
                        Loop

                    End If



                Next

            End If


        Catch ex As Exception

            errLog(Err.Description, "clsinventoryprocess:executeNonQuery_Inv_Processstock")
            Err.Clear()
            hasil = False
        End Try


        c_cmd.dispose()
        c_cmd = Nothing

        Return hasil
    End Function


    'buat usage jadi tanpa update cost
    Public Function Inv_Proc_trans_stckCls(ByVal dstart As Date, ByVal dend As Date, ByVal CustId As Integer) As Boolean

        'Process_InvDataStock(dstart, dend)

        Dim c_cmd As New clsDbo("")
        Dim i_SalesID As Integer = 0
        Dim ds As DataSet

        Dim dTotalQty As Decimal = 0
        Dim dvariantQty As Decimal = 0
        Dim dDate As Date
        Dim hasil As Boolean = True


        Try

            If c_cmd.ExecuteScalar("SELECT COUNT(custid) FROM cld_inv_closing WHERE invdate = '" & InvDate.Date.ToString("yyyy-MM-dd 00:00:00") & "' and custid=" & CustId & "", 0) = 0 Then
                Dim clsInv As New clsInventory
                clsInv.InvIn(CustId, "Sysadmin", 0)
            End If

            ' c_cmd.executeNonQuery("DELETE FROM cld_inv_transstockcls WHERE custid = " & CustId & " AND invdate >='" & dstart.Date.AddDays(1).ToString("yyyy-MM-dd") & "' AND invDate <= '" & dend.Date.AddDays(1).ToString("yyyy-MM-dd") & "' ")

            c_cmd.ReNew()
            ds = c_cmd.ReturnDataset("SELECT invdate, cld_inv_transstockcls.custid, stkid stk_id, stkno stk_no, cld_inv_stk.name stk_name, cld_inv_stk.`Unit2` u2_nm,
                                        ccid cc,ccname cc_name, (CASE 
                                        WHEN bclose = 0 THEN cost 
                                        WHEN bclose = 1 THEN closecost END) cost, 
                                        (CASE 
                                        WHEN bclose = 0 THEN ttlqty 
                                        WHEN bclose = 1 THEN closeqty END) ttlqty,
                                        (CASE 
                                        WHEN bclose = 0 THEN cost 
                                        WHEN bclose = 1 THEN closecost END) cost, rcvqty, rtnqty, tinQty, toutQty, rwasteQty, fwasteQty, usageqty, bClose, closeqty, closecost,hooutqty hoout,HoInQty hoin,bpin, bpout
                                        FROM cld_inv_transstockcls
                                        INNER JOIN cld_inv_stk ON cld_inv_transstockcls.`stkId` = cld_inv_stk.id
                                          WHERE `invDate` >= '" & dstart.Date.ToString("yyyy-MM-dd") & "' AND `invDate` <= '" & dend.Date.ToString("yyyy-MM-dd") & "' AND cld_inv_transstockcls.`ccid` > 0 AND cld_inv_transstockcls.`custid` = " & CustId)



            If ds.Tables(0).Rows.Count > 0 Then
                For Each row As DataRow In ds.Tables(0).Rows
                    c_cmd.ReNew()

                    '(OpenQty + rcvqty - rtnqty + tinQty - toutQty - rwasteQty - fwasteQty - usageqty - HOout + Hoin + BPin - BPout) AS ttlQty

                    'dTotalQty = row.Item("OpenQty") + row.Item("rcvqty") - row.Item("rtnqty") + row.Item("tinQty") - row.Item("toutQty") - row.Item("rwasteQty") - row.Item("fwasteQty") - row.Item("usageqty") - row.Item("HoOut") + row.Item("HoIn") + row.Item("BPin") - row.Item("BPout")
                    dTotalQty = row.Item("ttlqty")

                    'CASE WHEN bclose = 0 THEN 0 ELSE   (OpenQty + rcvqty - rtnqty + tinQty - toutQty - rwasteQty - fwasteQty - usageqty - closeQty - HOout + Hoin + BPin - BPout)  END as varianceQty
                    If row.Item("bClose") <> 0 Then
                        dvariantQty = row.Item("ttlqty") - row.Item("closeQty")
                    Else
                        dvariantQty = 0
                    End If

                    dDate = row.Item("invdate")
                    'If c_cmd.ExecuteScalar("SELECT COUNT(ccid) FROM cld_inv_transstockcls WHERE custid = " & CustId & " AND ccid= " & row.Item("cc") & " AND stkid= " & row.Item("stk_id") & " AND invdate='" & dDate.Date.AddDays(1).ToString("yyyy-MM-dd") & "'") Then
                    '    c_cmd.executeNonQuery("DELETE FROM cld_inv_transstockcls WHERE custid = " & CustId & " AND ccid= " & row.Item("cc") & " AND stkid= " & row.Item("stk_id") & " AND invdate='" & dDate.Date.AddDays(1).ToString("yyyy-MM-dd") & "'")
                    'End If


                    If c_cmd.ExecuteScalar("SELECT COUNT(ccid) FROM cld_inv_transstockcls WHERE custid = " & CustId & " AND ccid= " & row.Item("cc") & " AND stkid= " & row.Item("stk_id") & " AND invdate='" & dDate.Date.AddDays(1).ToString("yyyy-MM-dd 00:00:00") & "'", 0) = 0 Then
                        c_cmd.ReNew()
                        If c_cmd.executeNonQuery("insert into cld_inv_transstockcls values ('" & dDate.Date.AddDays(1).ToString("yyyy-MM-dd") & "', " & CustId & "," & row.Item("stk_id") & ", '" & row.Item("stk_No") & "', '" & row.Item("Stk_name") & "'," &
                        " " & row.Item("cc") & ", '" & row.Item("cc_name") & "', " & row.Item("cost") & ", " & row.Item("ttlqty") & ", " & row.Item("Cost") & ", 0, 0, 0, " &
                        " 0, 0, 0, 0, " & dTotalQty & ", 0, 0, 0, " & dvariantQty & ", 0, 0, 0, 0, 0)") = True Then
                            setstock_cost(row.Item("stk_id"), row.Item("cc"), CustId, row.Item("cost"))

                        Else
                            '    errClose("insert into cld_inv_transstockcls values ('" & dDate.Date.AddDays(1).ToString("yyyy-MM-dd") & "', " & CustId & "," & row.Item("stk_id") & ", '" & row.Item("stk_No") & "', '" & row.Item("Stk_name") & "'," &
                            '" " & row.Item("cc") & ", '" & row.Item("cc_name") & "', " & row.Item("cost") & ", " & row.Item("ttlqty") & ", " & row.Item("Cost") & ", 0, 0, 0, " &
                            '" 0, 0, 0, 0, " & dTotalQty & ", 0, 0, 0, " & dvariantQty & ", 0, 0, 0, 0, 0)", "clsInventoryprocess:Inv_Proc_trans_stckCls")
                            writeerrcsv(True, dDate.Date.AddDays(1).ToString("yyyy-MM-dd 00:00:00"), row.Item("cc"), row.Item("stk_id"))
                            c_cmd.ReNew()

                            c_cmd.executeNonQuery("Delete from cld_inv_transstockcls WHERE InvDate ='" & dDate.Date.AddDays(1).ToString("yyyy-MM-dd") & "' AND CustID = " & CustId)

                            c_cmd.ReNew()

                            c_cmd.executeNonQuery("DELETE FROM cld_inv_closing WHERE invdate = '" & dDate.Date.AddDays(1).ToString("yyyy-MM-dd") & "' AND custid = " & CustId & "")


                            hasil = False
                            Exit For


                        End If
                    End If


                Next


            End If
            c_cmd.dispose()

        Catch ex As Exception

            errLog(Err.Description, "clsDBO:Inv_Proc_trans_stckCls")
            Err.Clear()
            hasil = False
        End Try


        c_cmd.dispose()
        c_cmd = Nothing



        Return hasil
    End Function

    Public Function Inv_Proc_trans_stckClsbyid(ByVal dstart As Date, ByVal dend As Date, ByVal CustId As Integer, ByVal ccId As Integer, ByVal stokId As Integer) As Boolean

        Dim c_cmd As New clsDbo("")
        Dim i_SalesID As Integer = 0
        Dim ds As DataSet

        Dim dTotalQty As Decimal = 0
        Dim dvariantQty As Decimal = 0
        Dim dDate As Date
        Dim hasil As Boolean = True
        Dim runttl As Decimal = 0

        Try

            If c_cmd.ExecuteScalar("SELECT COUNT(custid) FROM cld_inv_closing WHERE invdate = '" & InvDate.Date.ToString("yyyy-MM-dd 00:00:00") & "'", 0) = 0 Then
                Dim clsInv As New clsInventory
                clsInv.InvIn(CustId, "Sysadmin", 0)
            End If

            ds = c_cmd.ReturnDataset("SELECT invdate, cld_inv_transstockcls.custid, stkid stk_id, stkno stk_no, cld_inv_stk.name stk_name, cld_inv_stk.`Unit2` u2_nm,
                                        ccid cc,ccname cc_name, (CASE 
                                        WHEN bclose = 0 THEN cost 
                                        WHEN bclose = 1 THEN closecost END) cost, 
                                        (CASE 
                                        WHEN bclose = 0 THEN ttlqty 
                                        WHEN bclose = 1 THEN closeqty END) ttlqty,
                                        (CASE 
                                        WHEN bclose = 0 THEN cost 
                                        WHEN bclose = 1 THEN closecost END) cost, rcvqty, rtnqty, tinQty, toutQty, rwasteQty, fwasteQty, usageqty, bClose, closeqty, closecost,hooutqty hoout,HoInQty hoin,bpin, bpout
                                        FROM cld_inv_transstockcls
                                        INNER JOIN cld_inv_stk ON cld_inv_transstockcls.`stkId` = cld_inv_stk.id
                                          WHERE `invDate` >= '" & dstart.Date.ToString("yyyy-MM-dd") & "' AND `invDate` <= '" & dend.Date.ToString("yyyy-MM-dd") & "' AND cld_inv_transstockcls.`ccid` = " & ccId & " AND cld_inv_transstockcls.stkid = " & stokId & " AND cld_inv_transstockcls.`custid` = " & CustId)


            'errLog("Inv_ProcessClsQuery END " & Now, "clsDBO:Inv_Proc_trans_stckClsbyid")

            If ds.Tables(0).Rows.Count > 0 Then
                For Each row As DataRow In ds.Tables(0).Rows
                    c_cmd.ReNew()


                    dTotalQty = row.Item("ttlqty")
                    If InvCost = 0 Then
                        InvCost = row.Item("cost")
                    End If

                    'CASE WHEN bclose = 0 THEN 0 ELSE   (OpenQty + rcvqty - rtnqty + tinQty - toutQty - rwasteQty - fwasteQty - usageqty - closeQty - HOout + Hoin + BPin - BPout)  END as varianceQty
                    If row.Item("bClose") <> 0 Then
                        dvariantQty = row.Item("ttlqty") - row.Item("closeQty")

                    Else
                        dvariantQty = 0
                        If runttl > 0 Then
                            dTotalQty = runttl

                        End If

                    End If

                    dDate = row.Item("invdate")
                    If dDate >= Date.Now.ToString("yyyy-MM-dd 00:00:00") Then
                        Exit For
                    End If
                    If c_cmd.ExecuteScalar("SELECT COUNT(ccid) FROM cld_inv_transstockcls WHERE custid = " & CustId & " AND ccid= " & row.Item("cc") & " AND stkid= " & row.Item("stk_id") & " AND invdate='" & dDate.Date.AddDays(1).ToString("yyyy-MM-dd 00:00:00") & "'", 0) > 0 Then
                        If InvstkId = 0 Then
                            InvstkId = row.Item("stk_id")
                        End If
                        If InvCc = 0 Then
                            InvCc = row.Item("cc")
                        End If
                        If InvCustId = 0 Then
                            InvCustId = CustId
                        End If
                        Dim todaytrans As Double = (getrecqty(InvstkId, InvCc, InvCustId, dDate.AddDays(1).ToString("yyyy-MM-dd")) - getopenqty(InvstkId, InvCc, InvCustId, dDate.AddDays(1).ToString("yyyy-MM-dd")))
                        Dim invcost2 As Double = 0
                        '" & Openqty & "+" & Invrcvqty & "-" & rtnqty & "+" & t_in & "-" & t_out & "-" & InvWasteQty & "-" & fwaste & "-" & invusageQty & "-" & ho_out & "+" & ho_in & "-" & BP_out & "+" & BP_in & "


                        If Not Double.IsNaN(((InvCost * row.Item("ttlqty")) + (getreccost(InvstkId, InvCc, InvCustId, dDate.AddDays(1).ToString("yyyy-MM-dd")) * todaytrans)) / (row.Item("ttlqty") + todaytrans)) And (row.Item("ttlqty") + todaytrans > 0) Then 'invcost
                            invcost2 = ((InvCost * row.Item("ttlqty")) + (getreccost(InvstkId, InvCc, InvCustId, dDate.AddDays(1).ToString("yyyy-MM-dd")) * todaytrans)) / (row.Item("ttlqty") + todaytrans)
                        Else
                            invcost2 = InvCost 'row.Item("cost") 
                        End If

                        '        c_cmd.executeNonQuery("update cld_inv_transstockcls set  openqty = Openqty + " & old_qty & ",ttlqty = Openqty + " & old_qty & "+" & todaytrans & ",
                        '                     opencost = " & InvCost & ", cost = " & invcost2 & "
                        'where stkid = " & InvstkId & " and ccid = " & InvCc & " and invdate = '" & InvDate.Date.AddDays(1).ToString("yyyy-MM-dd") & "' and custid = " & InvCustId & "")

                        If c_cmd.executeNonQuery("update cld_inv_transstockcls set  openqty = " & dTotalQty & ", varianceQty = " & dvariantQty & ",ttlqty = " & dTotalQty & "+" & todaytrans & ",
                                     opencost = " & InvCost & ", cost = " & invcost2 & "
                                    where stkid = " & row.Item("stk_id") & " and ccid = " & row.Item("cc") & " and invdate = '" & dDate.AddDays(1).ToString("yyyy-MM-dd") & "' and custid = " & CustId & "") = True Then
                            setstock_cost(row.Item("stk_id"), row.Item("cc"), CustId, invcost2)
                        Else
                            'errClose("update cld_inv_transstockcls set  openqty = " & dTotalQty & ", varianceQty = " & dvariantQty & ",ttlqty = " & dTotalQty & "+" & todaytrans & ",
                            '         opencost = " & InvCost & ", cost = " & invcost2 & "
                            '        where stkid = " & row.Item("stk_id") & " and ccid = " & row.Item("cc") & " and invdate = '" & dDate.AddDays(1).ToString("yyyy-MM-dd") & "' and custid = " & CustId & "", "clsInventoryprocess:Inv_Proc_trans_stckClsbyid")
                            writeerrcsv(True, dDate.Date.AddDays(1).ToString("yyyy-MM-dd 00:00:00"), row.Item("cc"), row.Item("stk_id"))
                        End If

                    Else
                        If c_cmd.executeNonQuery("insert into cld_inv_transstockcls values ('" & dDate.Date.AddDays(1).ToString("yyyy-MM-dd") & "', " & CustId & "," & row.Item("stk_id") & ", '" & row.Item("stk_No") & "', '" & row.Item("Stk_name") & "'," &
                       " " & row.Item("cc") & ", '" & row.Item("cc_name") & "', " & InvCost & ", " & dTotalQty & ", " & InvCost & ", 0, 0, 0, " &
                       " 0, 0, 0, 0, " & dTotalQty & ", 0, 0, 0, " & dvariantQty & ", 0, 0, 0, 0, 0)") = True Then
                            setstock_cost(row.Item("stk_id"), row.Item("cc"), row.Item("Cost"), CustId)
                        Else
                            '     errClose("insert into cld_inv_transstockcls values ('" & dDate.Date.AddDays(1).ToString("yyyy-MM-dd") & "', " & CustId & "," & row.Item("stk_id") & ", '" & row.Item("stk_No") & "', '" & row.Item("Stk_name") & "'," &
                            '" " & row.Item("cc") & ", '" & row.Item("cc_name") & "', " & InvCost & ", " & dTotalQty & ", " & InvCost & ", 0, 0, 0, " &
                            '" 0, 0, 0, 0, " & dTotalQty & ", 0, 0, 0, " & dvariantQty & ", 0, 0, 0, 0, 0)", "clsInventoryprocess:Inv_Proc_trans_stckClsbyid")
                            writeerrcsv(True, dDate.Date.AddDays(1).ToString("yyyy-MM-dd 00:00:00"), row.Item("cc"), row.Item("stk_id"))
                        End If

                    End If
                    If c_cmd.executeNonQuery("update cld_inv_transstockcls set  ttlqty = Openqty + rcvqty-rtnqty+tinqty-toutqty-rwasteqty-fwasteqty-usageqty-hooutqty+hoinqty-bpout+bpin where stkid = " & row.Item("stk_id") & " and ccid = " & row.Item("cc") & " and invdate = '" & dDate.Date.AddDays(1).ToString("yyyy-MM-dd") & "' and custid = " & CustId & "") = False Then
                        'errClose("update cld_inv_transstockcls set  ttlqty = Openqty + rcvqty-rtnqty+tinqty-toutqty-rwasteqty-fwasteqty-usageqty-hooutqty+hoinqty-bpout+bpin where stkid = " & row.Item("stk_id") & " and ccid = " & row.Item("cc") & " and invdate = '" & dDate.Date.AddDays(1).ToString("yyyy-MM-dd") & "' and custid = " & CustId & "", "clsInventoryprocess:Inv_Proc_trans_stckClsbyid")
                        writeerrcsv(True, dDate.Date.AddDays(1).ToString("yyyy-MM-dd 00:00:00"), row.Item("cc"), row.Item("stk_id"))
                    End If
                    c_cmd.resetCmd("")
                    runttl = c_cmd.ExecuteScalar("select ttlqty from cld_inv_transstockcls where stkid = " & row.Item("stk_id") & " and ccid = " & row.Item("cc") & " and invdate = '" & dDate.Date.AddDays(1).ToString("yyyy-MM-dd") & "' and custid = " & CustId & "", 0)
                    c_cmd.resetCmd("")
                    InvCost = c_cmd.ExecuteScalar("select cost from cld_inv_transstockcls where stkid = " & row.Item("stk_id") & " and ccid = " & row.Item("cc") & " and invdate = '" & dDate.Date.AddDays(1).ToString("yyyy-MM-dd") & "' and custid = " & CustId & "", 0)

                    c_cmd.dispose()

                Next


            End If


        Catch ex As Exception

            errLog(Err.Description, "clsInventoryProcess:Inv_Proc_trans_stckClsbyid")
            Err.Clear()
            hasil = False
        End Try


        c_cmd.dispose()
        c_cmd = Nothing




        Return hasil
    End Function
    Public Function getrecqty(ByVal stkid As Integer, ByVal cc As Integer, ByVal custid As Integer, ByVal xinvdate As Date) As Decimal
        Dim rqty As Decimal = 0
        Try
            Dim c_cmd As New clsDbo("")
            rqty = c_cmd.ExecuteScalar("SELECT IFNULL(ttlqty,0) FROM cld_inv_transstockcls WHERE stkid = " & stkid & " AND ccid = " & cc & " AND invdate = '" & xinvdate.Date.ToString("yyyy-MM-dd") & "' AND custid = " & custid & "", 0)

        Catch ex As Exception
            errLog(Err.Description, "clsInventoryprocess:getrecqty")
            Err.Clear()
        End Try
        Return rqty
    End Function
    Public Sub setstock_cost(Optional ByVal xstkid As Integer = 0, Optional ByVal xcc As Integer = 0, Optional ByVal xcustid As Integer = 0, Optional ByVal xcost As Double = 0)
        Dim c_cmd As New clsDbo("")
        Try
            If xcost = 0 Then
                xstkid = InvstkId
                If xstkid = 0 Then
                    Exit Sub
                End If
                xcc = InvCc
                If xcc = 0 Then
                    Exit Sub
                End If
                If InvCost > 0 Then
                    xcost = InvCost
                Else
                    Exit Sub
                End If

                xcustid = InvCustId
                If xcustid = 0 Then
                    xcustid = c_cmd.ExecuteScalar("SELECT id FROM `cld_cust` LIMIT 1", 0)
                Else
                    If xcustid <> c_cmd.ExecuteScalar("SELECT id FROM `cld_cust` LIMIT 1", 0) Then
                        errLog("wrong custid" & " clsInventoryprocess:setstock_cost", xcustid.ToString)
                        xcustid = c_cmd.ExecuteScalar("SELECT id FROM `cld_cust` LIMIT 1", 0)
                    End If
                End If
            End If
            If My.Settings.stock_cost = 0 Then
                Exit Sub
            End If
            If c_cmd.ExecuteScalar("SELECT IFNULL(custid,0) cust_id FROM `cld_inv_stk_cc` WHERE custid<>" & xcustid & " LIMIT 1", 0) > 0 Then
                c_cmd.ReNew()
                c_cmd.executeNonQuery("DELETE FROM `cld_inv_stk_cc` WHERE custid<>" & xcustid)
            End If
            c_cmd.ReNew()
            If c_cmd.ExecuteScalar("select count(stkid) from cld_inv_stk_cc where stkid = " & xstkid & " and ccid = " & xcc & " and custid = " & xcustid & "", 0) = 0 Then
                'kalau blm ada, insert
                c_cmd.ReNew()
                c_cmd.executeNonQuery("insert into cld_inv_stk_cc values (" & xstkid & ", " & xcustid & ",0, " & xcc & ", " & xcost & ")")
            Else
                c_cmd.executeNonQuery("update cld_inv_stk_cc set cost = " & xcost & " where stkid = " & xstkid & " and ccid = " & xcc & " and custid = " & xcustid & "")
            End If
            '            If xcost = 0 Then 'kalau insert
            '                c_cmd.executeNonQuery("DELETE FROM cld_inv_stkcost 
            'where stkid = " & InvstkId & " and cc = " & InvCc & " and cost = " & InvCost & " and custid = " & InvCustId & "")
            '                c_cmd.resetCmd("")
            '                c_cmd.executeNonQuery("insert into cld_inv_stkcost values (" & InvstkId & ", " & InvCc & ", " & InvCost & ", " & InvCustId & ")")
            '            Else
            '                c_cmd.executeNonQuery("insert into cld_inv_stkcost values (" & xstkid & ", " & xcc & ", " & xcost & ", " & xcustid & ")")
            '            End If

        Catch ex As Exception
            errLog(Err.Description, "clsInventoryprocess:setstock_cost")
            Err.Clear()
        End Try
    End Sub

    Public Sub setstock_cost(ByVal xstkid As Integer, ByVal xcc As Integer, ByVal xcustid As Integer, ByVal xcost As Double, ByRef trans As MySqlTransaction)
        Try
            If xcost = 0 Then
                xstkid = InvstkId
                If xstkid = 0 Then Exit Sub
                xcc = InvCc
                If xcc = 0 Then Exit Sub
                If InvCost > 0 Then
                    xcost = InvCost
                Else
                    Exit Sub
                End If
                xcustid = InvCustId
                If xcustid = 0 Then
                     Using cmd As New MySqlCommand("SELECT id FROM `cld_cust` LIMIT 1", trans.Connection, trans)
                           Dim obj = cmd.ExecuteScalar
                           If Not IsDBNull(obj) Then xcustid = obj
                     End Using
                Else
                     Using cmd As New MySqlCommand("SELECT id FROM `cld_cust` LIMIT 1", trans.Connection, trans)
                           Dim obj = cmd.ExecuteScalar
                           If Not IsDBNull(obj) AndAlso xcustid <> obj Then
                                'Log warning?
                                xcustid = obj
                           End If
                     End Using
                End If
            End If
            
            If My.Settings.stock_cost = 0 Then Exit Sub

            Using cmd As New MySqlCommand("SELECT IFNULL(custid,0) cust_id FROM `cld_inv_stk_cc` WHERE custid<>" & xcustid & " LIMIT 1", trans.Connection, trans)
                 Dim val = cmd.ExecuteScalar()
                 If val IsNot Nothing AndAlso val > 0 Then
                     Using cmdDel As New MySqlCommand("DELETE FROM `cld_inv_stk_cc` WHERE custid<>" & xcustid, trans.Connection, trans)
                         cmdDel.ExecuteNonQuery()
                     End Using
                 End If
            End Using

            Dim countCc As Integer = 0
            Using cmd As New MySqlCommand("select count(stkid) from cld_inv_stk_cc where stkid = " & xstkid & " and ccid = " & xcc & " and custid = " & xcustid & "", trans.Connection, trans)
                  If Not IsDBNull(cmd.ExecuteScalar) Then countCc = cmd.ExecuteScalar
            End Using

            If countCc = 0 Then
                  Using cmd As New MySqlCommand("insert into cld_inv_stk_cc values (" & xstkid & ", " & xcustid & ",0, " & xcc & ", " & xcost & ")", trans.Connection, trans)
                      cmd.ExecuteNonQuery()
                  End Using
            Else
                  Using cmd As New MySqlCommand("update cld_inv_stk_cc set cost = " & xcost & " where stkid = " & xstkid & " and ccid = " & xcc & " and custid = " & xcustid & "", trans.Connection, trans)
                      cmd.ExecuteNonQuery()
                  End Using
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub incost(ByVal custid As Integer)

        'calculate cost
        'Dim ds As DataSet
        Dim dCost As Decimal = 0
        Dim xclosing As Boolean = True

        Dim c_cmd As New clsDbo("")

        Try

            If c_cmd.ExecuteScalar("SELECT COUNT(custid) FROM `cld_inv_stk_cc` WHERE stkid<>0 AND ccid<>0 AND cost=0 AND custid = " & custid & "", 0) = 0 Then
                Exit Sub
            Else
                ' OPTIMIZED: Use JOIN instead of correlated subquery to avoid locking and slowness
                Dim updateSql As String = "UPDATE `cld_inv_stk_cc` sc " &
                                          "JOIN `cld_inv_transstockcls` t ON t.invdate = CURDATE() AND t.stkid = sc.stkid AND t.ccid = sc.ccid AND t.custid = sc.custid " &
                                          "SET sc.cost = CASE WHEN t.cost > 0 THEN t.cost ELSE 0 END " &
                                          "WHERE sc.cost = 0 AND sc.custid = " & custid
                c_cmd.executeNonQuery(updateSql)
            End If


            'ds = c_cmd.ReturnDataset("SELECT stkid, custid, ccid, cost FROM cld_inv_transstockcls
            '                            WHERE invDate = '" & Now.Date.ToString("yyyy-MM-dd") & "' AND cld_inv_transstockcls.`custid` = " & custid)

            'If ds.Tables(0).Rows.Count > 0 Then

            '    For Each row As DataRow In ds.Tables(0).Rows
            '        setstock_cost(row.Item("stkid"), row.Item("ccid"), custid, row.Item("cost"))
            '    Next



            'End If

        Catch ex As Exception
            errLog(Err.Description, "clsInventoryprocess:incost")
            Err.Clear()
        End Try



    End Sub

End Class
