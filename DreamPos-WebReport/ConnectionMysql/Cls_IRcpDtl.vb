Public Class Cls_IRcpDtl

    Public Function SelectDataRcpItemDtl(ByVal custid As Integer, ByVal id As Integer, ByVal iddtl As Integer, ByVal tipe As Integer) As DataSet

        Dim ds As New DataSet
        Dim strFilter As String = ""

        Dim a As clsDbo
        If tipe = 1 Then
            'perubahan 10 oct 2017
            'a = New clsDbo("SELECT * FROM cld_Inv_stk where custid = " & custid & " and id Not In (Select kodeid from cld_inv_rcpdtl where rcpid = " & id & " and tipe = 1 and Kodeid = " & iddtl & ")")
            a = New clsDbo("SELECT * FROM cld_Inv_stk where custid = " & custid)
        Else
            'perubahan 10 oct 2017
            'a = New clsDbo("SELECT * FROM cld_Inv_rcp where custid = " & custid & " and id Not In (Select kodeid from cld_inv_rcpdtl where rcpid = " & id & " and tipe = 2 and Kodeid = " & iddtl & " ) and id <> " & id & "")
            a = New clsDbo("SELECT * FROM cld_Inv_rcp where custid = " & custid)
        End If

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function
    Public Function SelectDataInvItemAll(ByVal custid As Integer, ByVal tipe As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""

        Dim a As clsDbo
        If tipe = 1 Then

            a = New clsDbo("SELECT * FROM cld_Inv_stk where custid = " & custid & " ORDER BY NAME")
        Else
            a = New clsDbo("SELECT * FROM cld_Inv_rcp where custid = " & custid & " ORDER BY NAME")


        End If

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function
    Public Function SelectDataRcpItemDtlAll(ByVal custid As Integer, ByVal id As Integer, ByVal tipe As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""

        Dim a As clsDbo
        If tipe = 1 Then

            a = New clsDbo("SELECT * FROM cld_Inv_stk where custid = " & custid & " and id Not In (Select kodeid from cld_inv_rcpdtl where rcpid = " & id & " and tipe = 1) order by name")
        Else
            a = New clsDbo("SELECT * FROM cld_Inv_rcp where custid = " & custid & " and id Not In (Select kodeid from cld_inv_rcpdtl where rcpid = " & id & " and tipe = 2) and id <> " & id & " order by name")


        End If

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function
    Public Function SelectDataRcpItemDtlAllSearch(ByVal custid As Integer, ByVal id As Integer, ByVal tipe As Integer, ByVal nama As String) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""

        Dim a As clsDbo
        If tipe = 1 Then

            a = New clsDbo("SELECT * FROM cld_Inv_stk where custid = " & custid & " and id Not In (Select kodeid from cld_inv_rcpdtl where rcpid = " & id & " and tipe = 1) and name like '%" & nama & "%'")
        Else
            a = New clsDbo("SELECT * FROM cld_Inv_rcp where custid = " & custid & " and id Not In (Select kodeid from cld_inv_rcpdtl where rcpid = " & id & " and tipe = 2) and id <> " & id & " and name like '%" & nama & "%'")


        End If

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function
    Public Function SelectDataRcpDtl(ByVal id As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""


        Dim a As New clsDbo("SELECT * FROM cld_Inv_rcpdtl where id = " & id)


        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function

    Public Function SelectQtyRcpDtl(ByVal id As Integer, ByVal kodeid As Integer) As Decimal
        'Dim ds As New DataSet
        Dim qtyrcp As Decimal = 0
        Dim strFilter As String = ""
        Dim c_cmd As New Cls_command

        strFilter = ("SELECT IFNULL(qty,0) FROM cld_Inv_rcpdtl where rcpid = " & id & " and kodeid= " & kodeid & " ")

        Try
            qtyrcp = c_cmd.ExecuteScalar(strFilter, 0)


        Catch ex As Exception

        End Try


        Return qtyrcp

    End Function

    Public Function SelectDataset_rcpdtl(ByVal custid As Integer, ByVal rcpid As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""


        Dim a As New clsDbo("SELECT cld_inv_rcpdtl.*,CASE WHEN tipe = 1 THEN 'Inventory' ELSE 'Receipe' END AS typename, CASE WHEN tipe = 1 " &
                            " THEN cld_inv_stk.Name ELSE cld_inv_rcp.Name END AS dtlName, CASE WHEN tipe = 1 THEN  cld_inv_stk.Unit3 ELSE 'Pcs' END AS unit, " &
                            " 0 AS cost_per_unit3, 0 AS total_cost FROM cld_inv_rcpdtl " &
                            " LEFT JOIN cld_inv_stk ON cld_inv_stk.ID = cld_inv_rcpdtl.kodeId " &
                            " LEFT JOIN cld_inv_rcp ON cld_inv_rcp.ID = cld_inv_rcpdtl.kodeId " &
                            " WHERE cld_inv_rcpdtl.custid = " & custid & "  AND cld_inv_rcpdtl.rcpid = " & rcpid & " ")

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function

    Public Function SelectDataset_rcpdtl_withcost(ByVal custid As Integer, ByVal rcpid As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""


        Dim a As New clsDbo("SELECT cld_inv_rcpdtl.*,CASE WHEN tipe = 1 THEN 'Inventory' ELSE 'Receipe' END AS typename, CASE WHEN tipe = 1 " &
                            " THEN cld_inv_stk.Name ELSE cld_inv_rcp.Name END AS dtlName, CASE WHEN tipe = 1 THEN  cld_inv_stk.Unit3 ELSE 'Pcs' END AS unit, " &
                            " CASE WHEN tipe = 1 THEN IFNULL((SELECT IFNULL(SUM(ts.cost),0) / IFNULL(NULLIF(cld_inv_stk.factor2,0),1) FROM cld_inv_transstockcls ts WHERE ts.invdate = CURDATE() AND ts.ccid = (SELECT id FROM cld_inv_cc WHERE olid = 0 LIMIT 1) AND ts.stkid = cld_inv_rcpdtl.kodeId), 0) ELSE 0 END AS cost_per_unit3, " &
                            " CASE WHEN tipe = 1 THEN IFNULL((SELECT IFNULL(SUM(ts.cost),0) / IFNULL(NULLIF(cld_inv_stk.factor2,0),1) FROM cld_inv_transstockcls ts WHERE ts.invdate = CURDATE() AND ts.ccid = (SELECT id FROM cld_inv_cc WHERE olid = 0 LIMIT 1) AND ts.stkid = cld_inv_rcpdtl.kodeId), 0) * (cld_inv_rcpdtl.qty * (100 + IFNULL(cld_inv_rcpdtl.lossf, 0)) / 100) ELSE 0 END AS total_cost " &
                            " FROM cld_inv_rcpdtl " &
                            " LEFT JOIN cld_inv_stk ON cld_inv_stk.ID = cld_inv_rcpdtl.kodeId " &
                            " LEFT JOIN cld_inv_rcp ON cld_inv_rcp.ID = cld_inv_rcpdtl.kodeId " &
                            " WHERE cld_inv_rcpdtl.custid = " & custid & "  AND cld_inv_rcpdtl.rcpid = " & rcpid & " ")

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function

    Public Function SelectDataset_rcpdtl_nr(ByVal custid As Integer, ByVal rcpid As Integer) As DataTable
        Dim ds, dsid As New DataSet
        Dim dt As New DataTable
        Dim c_cmd As New Cls_command
        Dim strFilter As String = ""
        Dim c1, c2 As Integer
        Dim sid As String = ""
        Dim dv As DataView

        sid = rcpid
getsid:
        Dim a As New clsDbo("SELECT cld_inv_rcpdtl.*, cld_inv_stk.Name  AS dtlName, cld_inv_stk.Unit3 unit, cld_inv_stk.factor2, cld_inv_stk.number FROM cld_inv_rcpdtl " &
                            " LEFT JOIN cld_inv_stk ON cld_inv_stk.ID = cld_inv_rcpdtl.kodeId " &
                            " LEFT JOIN cld_inv_rcp ON cld_inv_rcp.ID = cld_inv_rcpdtl.kodeId " &
                            " WHERE cld_inv_rcpdtl.custid = " & custid & "  AND cld_inv_rcpdtl.rcpid in (" & sid & ") ")

        ds = (a.selectDataset)
        dv = ds.Tables(0).DefaultView
        c1 = ds.Tables(0).Rows.Count
        dv.RowFilter = "tipe = 1"
        c2 = dv.ToTable.Rows.Count
        dt.Merge(dv.ToTable)

        If c1 - c2 <> 0 Then
            dsid = c_cmd.ReturnDataset("SELECT kodeid FROM cld_inv_rcpdtl WHERE tipe = 2 AND rcpid = " & rcpid & " AND custid =  " & custid & "")
            If dsid.Tables(0).Rows.Count > 0 Then
                sid = ""
                For Each row As DataRow In dsid.Tables(0).Rows
                    For Each column As DataColumn In dsid.Tables(0).Columns
                        'Add the Data rows.
                        sid += row(column.ColumnName).ToString().Replace(",", ";") + ","c
                    Next

                    'Add new line.
                    'sid += vbCr & vbLf
                Next
                sid = sid.TrimEnd(",")
            End If
            GoTo getsid

        End If



        a.dispose()
        a = Nothing

        Return dt

    End Function

    Public Function SelectDataset_rcpdtlSearch(ByVal name As String, ByVal custid As Integer, ByVal rcpid As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""


        Dim a As New clsDbo("SELECT cld_inv_rcpdtl.*,CASE WHEN tipe = 1 THEN 'Inventory' ELSE 'Receipe' END AS typs, CASE WHEN tipe = 1 " & _
                            " THEN cld_inv_stk.Name ELSE cld_inv_rcp.Name END AS dtlName FROM cld_inv_rcpdtl " & _
                            " LEFT JOIN cld_inv_stk ON cld_inv_stk.ID = cld_inv_rcpdtl.kodeId " & _
                            " LEFT JOIN cld_inv_rcp ON cld_inv_rcp.ID = cld_inv_rcpdtl.kodeId " & _
                            " WHERE cld_inv_rcpdtl.custid = " & custid & "  AND cld_inv_rcpdtl.rcpid = " & rcpid & " AND " & _
                            " CASE WHEN tipe = 1 THEN cld_inv_stk.Name ELSE cld_inv_rcp.Name END Like '%" & name & "%' ")

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function
    Public Function SelectDataset_rcpdtlEdit(ByVal Id As Integer, ByVal custid As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""


        Dim a As New clsDbo("SELECT * from cld_inv_rcpdtl where id = " & Id & " and custid = " & custid & "")

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function


    Public Sub insertRcpDtl(ByVal id As Integer, ByVal custid As Integer, ByVal rcpid As Integer, ByVal tipe As Integer, ByVal kodeid As Integer, ByVal qty As Decimal, ByVal Lossf As Decimal, ByVal username As String)
        SaveDetail(id, custid, rcpid, tipe, kodeid, qty, Lossf, "edit")
        insertLogModule(username, "Invrcpdtl", "Update", custid)
    End Sub

    Public Shared Sub SaveDetail(ByVal id As Integer, ByVal custid As Integer, ByVal rcpid As Integer,
                                 ByVal tipe As Integer, ByVal kodeid As Integer, ByVal qty As Decimal,
                                 ByVal lossf As Decimal, ByVal mode As String)
        If tipe <> 1 AndAlso tipe <> 2 Then Throw New ArgumentException("Invalid recipe detail type.")
        Using db As New RecipeDb()
            db.SetParameter("id", id)
            db.SetParameter("custid", custid)
            db.SetParameter("rcpid", rcpid)
            db.SetParameter("tipe", tipe)
            db.SetParameter("kodeid", kodeid)
            db.SetParameter("qty", qty)
            db.SetParameter("lossf", lossf)
            If db.Scalar("SELECT id FROM cld_inv_rcp WHERE id=@rcpid AND custid=@custid FOR UPDATE") Is Nothing Then
                Throw New InvalidOperationException("Recipe not found for this customer.")
            End If
            Dim itemSql As String = If(tipe = 1,
                "SELECT id FROM cld_inv_stk WHERE id=@kodeid AND custid=@custid",
                "SELECT id FROM cld_inv_rcp WHERE id=@kodeid AND custid=@custid")
            If db.Scalar(itemSql) Is Nothing Then
                Throw New InvalidOperationException("Recipe item not found for this customer.")
            End If
            Dim existing As Object
            If mode = "edit" Then
                existing = db.Scalar("SELECT id FROM cld_inv_rcpdtl WHERE id=@id AND custid=@custid AND rcpid=@rcpid FOR UPDATE")
            Else
                existing = db.Scalar("SELECT id FROM cld_inv_rcpdtl WHERE custid=@custid AND rcpid=@rcpid AND kodeid=@kodeid AND tipe=@tipe ORDER BY id LIMIT 1 FOR UPDATE")
            End If
            If mode = "import" Then
                db.Execute("DELETE FROM cld_inv_rcpdtl WHERE custid=@custid AND rcpid=@rcpid AND kodeid=@kodeid AND tipe=@tipe")
                existing = Nothing
            End If
            If existing Is Nothing OrElse IsDBNull(existing) Then
                Dim lastId As Object = db.Scalar("SELECT id FROM cld_inv_rcpdtl ORDER BY id DESC LIMIT 1 FOR UPDATE")
                db.SetParameter("id", If(lastId Is Nothing OrElse IsDBNull(lastId), 1, Convert.ToInt32(lastId) + 1))
                db.Execute("INSERT INTO cld_inv_rcpdtl (id,custid,rcpid,tipe,kodeid,qty,lossf) VALUES (@id,@custid,@rcpid,@tipe,@kodeid,@qty,@lossf)")
            Else
                db.SetParameter("id", Convert.ToInt32(existing))
                If mode = "add" Then
                    db.Execute("UPDATE cld_inv_rcpdtl SET qty=qty+@qty,lossf=@lossf WHERE id=@id AND custid=@custid AND rcpid=@rcpid")
                Else
                    db.Execute("UPDATE cld_inv_rcpdtl SET tipe=@tipe,kodeid=@kodeid,qty=@qty,lossf=@lossf WHERE id=@id AND custid=@custid AND rcpid=@rcpid")
                End If
            End If
            db.Commit()
        End Using
    End Sub
    Public Function SelectDataRcpcc(ByVal id As Integer, ByVal ccid As Integer, ByVal custid As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""


        Dim a As New clsDbo("SELECT * FROM cld_inv_rcp_cc WHERE rcpid = " & id & "  AND custid = " & custid & " AND ccid = " & ccid & "")


        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function
    Public Sub deleteRcpcc(ByVal id As Integer, ByVal custid As Integer)
        If My.Settings.rcp_cc <> 1 Then Return
        Using db As New RecipeDb()
            db.SetParameter("id", id)
            db.SetParameter("custid", custid)
            db.Execute("DELETE FROM cld_inv_rcp_cc WHERE rcpid=@id AND custid=@custid")
            db.Commit()
        End Using
    End Sub
    Public Sub importRcpDtl(ByVal custid As Integer, ByVal rcpid As Integer, ByVal tipe As Integer, ByVal kodeid As Integer, ByVal qty As Decimal, ByVal Lossf As Decimal, ByVal username As String)
        SaveDetail(0, custid, rcpid, tipe, kodeid, qty, Lossf, "import")
        insertLogModule(username, "Invrcpdtl", "Update", custid)
    End Sub


    Public Sub deleteRcpStk(ByVal id As Integer, ByVal custid As Integer)
        If My.Settings.rcp_cc <> 1 Then Return
        Using db As New RecipeDb()
            db.SetParameter("id", id)
            db.SetParameter("custid", custid)
            db.Execute("DELETE FROM cld_inv_stkxrcp WHERE stkid=@id AND custid=@custid")
            db.Commit()
        End Using
    End Sub
End Class
