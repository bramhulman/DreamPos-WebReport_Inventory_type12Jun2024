Public Class ClsOutlet

    'keterangan 
    'oType 1 = biasa, 2 = inventory

    'store
    Public storeName As String = ""
    Public StoreID As Integer = 0
    Public storeType As Integer = 0
    Public custId As Integer = 0
    Public expiredDate As Date = Now.Date.AddYears(-10)

    Public Function SelectDataset_Outlet(ByVal custid As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim ds As New DataSet

        Try
            Dim strFilter As String = ""

            Dim strData As String = ""

            If modpriv And usrModulepriv.moduleuser = usrModulepriv.moduleuser Then

                strData = "SELECT cld_outlet.*, cld_cust.Nama AS CustName, DATE_FORMAT(cld_outlet.endDate,'%d-%b-%Y') AS eDate1, (percentage * biaya / 100) AS biaya1 FROM cld_outlet INNER JOIN cld_cust " &
                          " ON (cld_outlet.custID = cld_cust.ID) where cld_outlet.custid = " & custid & " and  cld_outlet.id In (Select store from cld_userxstore where user = '" & usr & "')"

            Else
                strData = "SELECT cld_outlet.*, cld_cust.Nama AS CustName, DATE_FORMAT(cld_outlet.endDate,'%d-%b-%Y') AS eDate1, (percentage * biaya / 100) AS biaya1 FROM cld_outlet INNER JOIN cld_cust " &
                          " ON (cld_outlet.custID = cld_cust.ID) where cld_outlet.custid = " & custid & ""

            End If


            Dim a As New clsDbo(strData)

            ds = a.selectDataset

            a.dispose()
            a = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsOutlet.SelectDataset_Outlet")
            Err.Clear()

        End Try

        Return ds

    End Function

    Public Function SelectDataset_Outletbycust(ByVal custid As Integer, ByVal id As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim ds As New DataSet
        Dim str As String = ""
        Try
            Dim strFilter As String = ""
            If modpriv And usrModulepriv.moduleuser = usrModulepriv.moduleuser Then ' If id <> 0 Then
                str = "SELECT * FROM cld_outlet where custid = " & custid & " and id Not in (select Olid from cld_inv_cc where custid = " & custid & " and id <> " & id & ") "
            Else
                str = "SELECT * FROM cld_outlet where custid = " & custid & ""

            End If
            Dim a As New clsDbo(str)

            ds = a.selectDataset

            a.dispose()
            a = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsOutlet.SelectDataset_Outlet")
            Err.Clear()

        End Try

        Return ds

    End Function

    Public Function SelectDataset_OutletSearch(ByVal name As String, ByVal custid As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim ds As New DataSet

        Try
            Dim strFilter As String = ""
            Dim strData As String = ""

            If modpriv And usrModulepriv.moduleuser = usrModulepriv.moduleuser Then

                strData = "SELECT cld_outlet.*, cld_cust.Nama AS CustName, DATE_FORMAT(cld_outlet.endDate,'%d-%b-%Y') AS eDate1, (percentage * biaya / 100) AS biaya1 FROM cld_outlet INNER JOIN cld_cust " &
            " ON (cld_outlet.custID = cld_cust.ID) Where cld_outlet.kode like '%" & name & "%' Or cld_outlet.Nama like '%" & name & "%' Or cld_outlet.PosVer like '%" & name & "%' and  cld_outlet.custid = " & custid & " and  cld_outlet.id In (Select store from cld_userxstore where user = '" & usr & "')"

            Else

                strData = "SELECT cld_outlet.*, cld_cust.Nama AS CustName, DATE_FORMAT(cld_outlet.endDate,'%d-%b-%Y') AS eDate1, (percentage * biaya / 100) AS biaya1 FROM cld_outlet INNER JOIN cld_cust " &
            " ON (cld_outlet.custID = cld_cust.ID) Where cld_outlet.kode like '%" & name & "%' Or cld_outlet.Nama like '%" & name & "%' Or cld_outlet.PosVer like '%" & name & "%' and  cld_outlet.custid = " & custid & " "

            End If


            Dim a As New clsDbo(strData)

            ds = a.selectDataset

            a.dispose()
            a = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsOutlet.SelectDataset_OutletSearch")
            Err.Clear()

        End Try


        Return ds

    End Function

    Public Function SelectDataset_OutletEdit(ByVal Id As Integer) As DataSet
        Dim ds As New DataSet
        Try
            Dim strFilter As String = ""

            Dim a As New clsDbo("Select * from cld_Outlet where ID  = " & Id & "")

            ds = a.selectDataset

            a.dispose()
            a = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsOutlet.SelectDataset_OutletEdit")
            Err.Clear()
        End Try

        Return ds

    End Function

    Public Function SelectDataset_OutletName(ByVal out As Integer) As String

        Dim name As String = ""

        Try
            Dim ds As New DataSet
            Dim strFilter As String = ""


            Dim a As New clsDbo("Select nama from cld_Outlet where ID  = " & out & "")

            ds = a.selectDataset
            If ds.Tables(0).Rows.Count > 0 Then
                name = ds.Tables(0).Rows(0)("nama")
            Else
                name = ""
            End If
            a.dispose()
            a = Nothing

        Catch ex As Exception
            errLog(Err.Description, "ClsOutlet.SelectDataset_OutletName")
            Err.Clear()

        End Try

        Return name

    End Function



#Region "Erik"

    '5 april 2019
    'perubahan update pos version pada saat EOD
    Public Sub getProperty_Outlet(ByVal storeno As String, Optional ByVal sVersion As String = "")
        Try

            Dim dtOutlet As New DataSet
            Dim c_cmd As New Cls_command

            dtOutlet = c_cmd.ReturnDataset(getstr_Outlet(storeno))

            If dtOutlet.Tables(0).Rows.Count > 0 Then

                storeName = dtOutlet.Tables(0).Rows(0).Item("Nama")
                StoreID = dtOutlet.Tables(0).Rows(0).Item("ID")
                storeType = dtOutlet.Tables(0).Rows(0).Item("oType")
                custId = dtOutlet.Tables(0).Rows(0).Item("CustID")
                'penambahan ada expired date
                expiredDate = dtOutlet.Tables(0).Rows(0).Item("eDate")
            End If


            If sVersion <> "" Then
                c_cmd.executeNonQuery("Update cld_outlet set PosVer = '" & sVersion & "' where ID = " & StoreID)
            End If


            c_cmd.closeConnection()
            c_cmd = Nothing


        Catch ex As Exception
            errLog(Err.Description, "ClsOutlet.getProperty_Outlet")
            Err.Clear()

        End Try

    End Sub


    Public Function SelectDataset_OutletLoad(ByVal id As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet

        Dim ds As New DataSet
        Dim strData As String = ""

        If modpriv And usrModulepriv.moduleuser = usrModulepriv.moduleuser Then

            strData = "Select * from cld_Outlet where custid = " & id & " and id in (Select Store from cld_userxstore where user = '" & usr & "')"

        Else
            strData = "Select * from cld_Outlet where custid = " & id

        End If

        Dim a As New clsDbo(strData)

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function
    'Revisi 08 Juni
    Public Function SelectDataset_OutletLoadXtrans(ByVal id As Integer, ByVal name As String, Optional ByVal src As String = "") As DataSet

        Dim ds As New DataSet
        Dim strFilter As String = ""

        Dim a As New clsDbo("Select * from cld_Outlet where custid = " & id & " and (kode like '%" & src & "%' Or kode2 like '%" & src & "%' Or Nama like '%" & src & "%') and id IN (SELECT store FROM cld_userxstore WHERE USER = '" & name & "') ")

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function
    'Revisi 08 Juni
    Public Function SelectDataset_OutletLoadXtrans2(ByVal id As Integer, ByVal name As String) As DataSet

        Dim ds As New DataSet
        Dim strFilter As String = ""

        Dim a As New clsDbo("Select * from cld_Outlet where custid = " & id & " and id Not IN (SELECT store FROM cld_userxstore WHERE USER = '" & name & "') ")

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function
    'Revisi 08 Juni
    Public Function SelectDataset_UserTrans(ByVal id As Integer, ByVal modpriv As Integer) As DataSet

        Dim ds As New DataSet
        Dim strFilter As String = ""

        ' Dim a As New clsDbo("Select * from cld_user  where modulepriv & 1 <> " & modpriv & " and custid = " & id)
        Dim a As New clsDbo("Select * from cld_user  where modulepriv <> " & modpriv & " and custid = " & id)
        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function
   
    Public Function getstr_Outlet(ByVal storeno As String) As String

        Dim str As String = "Select * from cld_outlet where Kode = '" & storeno & "'"

        Return str

    End Function



    Public Function getstr_OutletByCust(ByVal custid As Integer) As String

        Dim str As String = "Select * from cld_outlet where CustID = '" & custid & "'"

        Return str

    End Function



    Public Function loadOutletExport(ByVal custID As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim ds As New DataSet

        Dim strFilter As String = ""

        If modpriv And usrModulepriv.moduleuser = usrModulepriv.moduleuser Then

            strFilter = "SELECT ID, Kode, Nama FROM cld_outlet WHERE CustID=" & custID &
                        " and iImport = 1 and id in (Select store from cld_userxstore where user = '" & usr & "') ORDER BY kode"

        Else

            strFilter = "SELECT ID, Kode, Nama FROM cld_outlet WHERE CustID=" & custID & _
                        " and iImport = 1 ORDER BY kode"

        End If

        Dim a As New clsDbo(strFilter)

        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds





    End Function


#End Region



    'tambahan bram 3 mei 2018
    Public Function SelectDataset_Outlet2(ByVal custid As Integer) As DataSet
        Dim ds As New DataSet

        Try
            Dim strFilter As String = ""

            Dim a As New clsDbo("SELECT ol.id, kode, kode2, nama,alamat, telp, remarks, own.`Name` AS own,CONCAT(br.`Name2`,' / ',br.`Name`) AS brand, ar.`Name` AS AREA, ct.`Name` AS city " &
                                "FROM cld_outlet ol INNER JOIN cld_own own ON ol.`ownID` = own.`ID` AND ol.`custID` = own.`CustID` " &
                                "INNER JOIN `cld_brand` br ON ol.`brandID` = br.`ID` AND ol.`custID`= br.`CustID` " &
                                "INNER JOIN `cld_area` ar ON ol.`areaID` = ar.`ID` AND ol.`custID` = ar.`CustID` " &
                                "INNER JOIN `cld_city` ct ON ol.`cityID` = ct.`ID` AND ol.`custID` = ct.`CustID` where ol.`custID` = " & custid)

            ds = a.selectDataset

            a.dispose()
            a = Nothing

        Catch ex As Exception
            errLog(Err.Description, "clsOutlet2.SelectDataset_Outlet")
            Err.Clear()

        End Try

        Return ds

    End Function

    Public Function SelectDataset_OutletSearch2(ByVal custid As Integer, ByVal key As String, Optional ByVal name As String = "") As DataSet
        Dim ds As New DataSet

        Try
            Dim strFilter As String = ""
            Dim sGenerate As String = "custid=" & custid.ToString.ToLower & "&name=" & name.ToString.ToLower & "&key=" & SecretKey.ToString.ToLower

            If key <> GenerateSHA512String(sGenerate) Then
                custid = -1
                name = "kosong"
            End If

            Dim a As New clsDbo("SELECT ol.id, kode, kode2, nama,alamat, telp, remarks, endDate, biaya, percentage, own.`Name` AS own,CONCAT(br.`Name2`,' / ',br.`Name`) AS brand, ar.`Name` AS AREA, ct.`Name` AS city " &
                            "FROM cld_outlet ol LEFT JOIN cld_own own ON ol.`ownID` = own.`ID` AND ol.`custID` = own.`CustID` " &
                            "LEFT JOIN `cld_brand` br ON ol.`brandID` = br.`ID` AND ol.`custID`= br.`CustID` " &
                            "LEFT JOIN `cld_area` ar ON ol.`areaID` = ar.`ID` AND ol.`custID` = ar.`CustID` " &
                            "LEFT JOIN `cld_city` ct ON ol.`cityID` = ct.`ID` AND ol.`custID` = ct.`CustID` where ol.`custID` = " & custid & " and (ol.kode like '%" & name & "%' Or ol.kode2 like '%" & name & "%' Or ol.Nama like '%" & name & "%' Or own.`Name` like '%" & name & "%' Or CONCAT(br.`Name2`,' / ',br.`Name`) like '%" & name & "%' Or ar.`Name` like '%" & name & "%' Or ct.`Name` like '%" & name & "%') ")




            ds = a.selectDataset

            a.dispose()
            a = Nothing

        Catch ex As Exception
            errLog(Err.Description, "clsOutlet2.SelectDataset_OutletSearch")
            Err.Clear()

        End Try


        Return ds

    End Function

    Public Function SelectDataset_OutletEdit2(ByVal custid As Integer, ByVal Id As Integer, ByVal key As String) As DataSet
        Dim ds As New DataSet
        Try
            Dim strFilter As String = ""
            Dim sGenerate As String = "custid=" & custid.ToString.ToLower & "&id=" & Id.ToString.ToLower & "&key=" & SecretKey.ToString.ToLower

            If key <> GenerateSHA512String(sGenerate) Then
                custid = -1
                Id = -1
            End If

            Dim a As New clsDbo("Select * from cld_Outlet where custid = " & custid & " and ID = " & Id)

            ds = a.selectDataset

            a.dispose()
            a = Nothing

        Catch ex As Exception
            errLog(Err.Description, "clsOutlet2.SelectDataset_OutletEdit")
            Err.Clear()

        End Try

        Return ds

    End Function

    Public Function Update_Outlet2(ByVal id As Integer, ByVal custid As Integer, ByVal kode2 As String, ByVal Alamat As String, ByVal telp As String, ByVal remarks As String, ByVal endDate As Date, ByVal biaya As Integer, ByVal percentage As Decimal, ByVal Ownid As Integer, ByVal Brandid As Integer, ByVal Areaid As Integer, ByVal cityid As Integer, ByVal key As String) As Boolean

        Dim hsl As Boolean = False
        Dim str As String
        Dim c_cmd As New Cls_command
        Dim x_edate As Date = endDate.AddDays(21)
        Try
            Dim sGenerate As String = "custid=" & custid.ToString.ToLower & "&id=" & id.ToString.ToLower & "&key=" & SecretKey.ToString.ToLower

            If key <> GenerateSHA512String(sGenerate) Then
                custid = -1
                id = -1
            End If


            str = "UPDATE cld_outlet SET  Kode2 = '" & kode2 & "',alamat = '" & Alamat & "',telp = '" & telp & "', Remarks = '" & remarks & "' " &
                    ", endDate = '" & endDate.ToString("yyyy-MM-dd 00:00:00") & "', eDate = '" & x_edate.ToString("yyyy-MM-dd 00:00:00") & "', biaya = " & biaya & ", percentage = " & percentage & ", ownID = " & Ownid & ", brandID = " & Brandid & ", areaID = " & Areaid & ", cityID = " & cityid & " WHERE ID = " & id & " and CustId = " & custid

            c_cmd.executeNonQuery(str)


            hsl = True

        Catch ex As Exception
            errLog(Err.Description, "clsOutlet2.Update_Outlet")
            Err.Clear()

        End Try

        c_cmd = Nothing
        Return hsl
    End Function
    Public Sub insertUserX(ByVal user As String, ByVal Store As List(Of String))
        Try

            Dim str As String
            Dim c_cmd As New Cls_command

            Dim ic_user As Integer = 0




            For i As Integer = 0 To Store.Count - 1
                ic_user = c_cmd.ExecuteScalar("SELECT COUNT(USER)FROM cld_userxstore WHERE USER='" & user & "' AND store = " & Store(i) & "", 0)
                If ic_user = 0 Then
                    str = "insert into cld_userxstore(User, Store) values ('" & user & "'," & Store(i) & ")"
                    c_cmd.executeNonQuery(str)
                End If



            Next

        Catch ex As Exception

            errLog(Err.Description, "clsOutlet:insertuserx")
            Err.Clear()
        End Try
    End Sub
    Public Sub DeleteUserXtranOutlet(ByVal id As Integer, ByVal Custid As Integer, ByVal username As String)
        'Dim pc As String = pic.ToString
        Try

            Dim str As String
            Dim c_cmd As New Cls_command

            str = "DELETE FROM cld_Userxstore  where Store = " & id & " and user = '" & username & "'"

            c_cmd.executeNonQuery(str)
            'Revisi 10 Mei
            insertLogModule(username, "UserXtrans", "Delete", Custid)
        Catch ex As Exception

            errLog(Err.Description, "clsOutlet:DeleteOutlet")
            Err.Clear()

        End Try
    End Sub

    Public Function GetIDOutlet(ByVal nama As String) As String
        Dim sHasil As String = ""

        Dim ds As New DataSet
        Dim strFilter As String = ""

        Try

            Dim a As New clsDbo("Select id from cld_Outlet where nama  = '" & nama & "'")

            ds = a.selectDataset
            If ds.Tables(0).Rows.Count > 0 Then
                sHasil = ds.Tables(0).Rows(0)("id")
            Else
                sHasil = ""
            End If
            a.dispose()
            a = Nothing

        Catch ex As Exception

            errLog(Err.Description, "clsOutlet:GetIDOutlet")
            Err.Clear()

        End Try

        Return sHasil
    End Function

    Public Function GetKodeOutlet(ByVal nama As String) As String
        Dim sHasil As String = ""

        Dim ds As New DataSet
        Dim strFilter As String = ""

        Try

            Dim a As New clsDbo("Select kode from cld_Outlet where nama  = '" & nama & "'")

            ds = a.selectDataset
            If ds.Tables(0).Rows.Count > 0 Then
                sHasil = ds.Tables(0).Rows(0)("kode")
            Else
                sHasil = ""
            End If
            a.dispose()
            a = Nothing

        Catch ex As Exception

            errLog(Err.Description, "clsOutlet:GetKodeOutlet")
            Err.Clear()

        End Try

        Return sHasil
    End Function

    Public Function GetIDBrandOutlet(ByVal nama As String) As String
        Dim sHasil As String = ""

        Dim ds As New DataSet
        Dim strFilter As String = ""

        Try

            Dim a As New clsDbo("Select ID from cld_brand where name3 = '" & nama & "'")

            ds = a.selectDataset
            If ds.Tables(0).Rows.Count > 0 Then
                sHasil = ds.Tables(0).Rows(0)("ID")
            Else
                sHasil = ""
            End If
            a.dispose()
            a = Nothing

        Catch ex As Exception

            errLog(Err.Description, "clsOutlet:getIDBrand")
            Err.Clear()

        End Try

        Return sHasil
    End Function


    Public Function getIdCity(ByVal nama As String) As String
        Dim sHasil As String = ""

        Dim ds As New DataSet
        Dim strFilter As String = ""

        Try

            Dim a As New clsDbo("Select ID from cld_city where kota = '" & nama & "'")

            ds = a.selectDataset
            If ds.Tables(0).Rows.Count > 0 Then
                sHasil = ds.Tables(0).Rows(0)("ID")
            Else
                sHasil = ""
            End If
            a.dispose()
            a = Nothing

        Catch ex As Exception

            errLog(Err.Description, "clsOutlet:getIDCity")
            Err.Clear()

        End Try

        Return sHasil
    End Function
    Public Function getIdStock(ByVal nama As String) As String
        Dim sHasil As String = ""

        Dim ds As New DataSet
        Dim strFilter As String = ""

        Try

            Dim a As New clsDbo("Select ID from cld_inv_stk where name = '" & nama & "'")

            ds = a.selectDataset
            If ds.Tables(0).Rows.Count > 0 Then
                sHasil = ds.Tables(0).Rows(0)("ID")
            Else
                sHasil = ""
            End If
            a.dispose()
            a = Nothing

        Catch ex As Exception

            errLog(Err.Description, "clsOutlet:getIdStock")
            Err.Clear()

        End Try

        Return sHasil
    End Function
End Class
