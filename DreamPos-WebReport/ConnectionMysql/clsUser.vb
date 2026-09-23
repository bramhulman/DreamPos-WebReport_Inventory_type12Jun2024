Imports Org.BouncyCastle.X509

Public Class clsUser
    Private mfunction As ModFunction
    Public Sub DeleteUserXtranOutlet(ByVal id As Integer, ByVal Custid As Integer, ByVal username As String)
        'Dim pc As String = pic.ToString
        Try

            Dim str As String
            Dim c_cmd As New Cls_command

            str = "DELETE FROM cld_Userxstore  where Store = " & id & ""

            c_cmd.executeNonQuery(str)
            'Revisi 10 Mei
            insertLogModule(username, "UserXtrans", "Delete", Custid)
        Catch ex As Exception

            errLog(Err.Description, "clsUser:DeleteUserXtranOutlet")
            Err.Clear()

        End Try
    End Sub
    Public Sub insertUserX(ByVal user As String, ByVal Store As Integer)
        Try


            'Dim clsdt As New clsDataItem
            Dim str As String
            Dim c_cmd As New Cls_command
            str = "insert into cld_userxstore(User, Store) values ('" & user & "'," & Store & ")"


            c_cmd.executeNonQuery(str)

        Catch ex As Exception

            errLog(Err.Description, "clsUser:insertuserx")
            Err.Clear()
        End Try
    End Sub

    Public Sub insertUser(ByVal usr As String, ByVal psw As String, ByVal custid As Integer, ByVal modulepriv As Integer, ByVal rptpriv As Integer, ByVal email As String, ByVal modul As Integer, ByVal rptf As Integer, ByVal invpriv As Integer, Optional ByVal oldusr As String = "", Optional ByVal shapsw As String = "")
        Try


            'Dim clsdt As New clsDataItem
            Dim str As String = ""
            Dim c_cmd As New Cls_command

            If c_cmd.ExecuteScalar("Select count(Usr) FROM `cld_user` WHERE usr = '" & oldusr & "' AND SHA2(psw,512)='" & shapsw & "'", 0) > 0 Then
                str = "UPDATE cld_user SET usr='" & usr & "', psw='" & psw & "', modulepriv=" & modulepriv & ", rptpriv=" & rptpriv & ", email='" & email & "', modul=" & modul & ", rptf=" & rptf & ", invpriv=" & invpriv & " WHERE usr = '" & oldusr & "' and SHA2(psw,512) = '" & shapsw & "'"
            Else
                str = "insert into cld_user(Usr,psw,custid,modulepriv,rptpriv,email,modul,rptf,invpriv) values ('" & usr & "','" & psw & "'," & custid & "," & modulepriv & "," & rptpriv & ",'" & email & "'," & modul & "," & rptf & "," & invpriv & ")"
            End If



            c_cmd.executeNonQuery(str)

        Catch ex As Exception

            errLog(Err.Description, "clsUser:insertuserx")
            Err.Clear()
        End Try
    End Sub

    Public Function getactive(ByVal user As String, ByVal CustID As Integer) As Boolean

        Dim active As Boolean = True

        Try
            Dim c_Cmd As New Cls_command

            If c_Cmd.ExecuteScalar("SELECT modulepriv FROM cld_user WHERE usr = '" & user & "' AND custid = " & CustID, 0) = 2 Then
                active = False

            End If



        Catch ex As Exception
            errLog(Err.Description, "clsUser:getactive")
            Err.Clear()
        End Try

        Return active
    End Function
    Public Sub Update_user(ByVal custid As Integer, ByVal user As String, ByVal active As Boolean)


        Dim str As String
        Dim c_cmd As New Cls_command
        Dim modpriv As Integer = 1
        Try
            If active Then
                modpriv = 1
            Else
                modpriv = 2
            End If


            str = "UPDATE cld_user SET modulepriv=" & modpriv & " WHERE usr = '" & user & "' and CustId = " & custid

            c_cmd.executeNonQuery(str)


            hsl = True

        Catch ex As Exception
            errLog(Err.Description, "clsUser:Update_user")
            Err.Clear()

        End Try

        c_cmd = Nothing

    End Sub
    Public Function getstorebyid(ByVal strid As Integer) As DataSet

        Dim ds As New DataSet

        Dim str As String = ""
        Try
            Dim cCmd As New clsDbo("")

            str = "select id,kode,nama from cld_outlet where id =" & strid & ""


            Dim c_Cmd As New clsDbo(str)

            ds = c_Cmd.selectDataset()

            c_Cmd.dispose()
            c_Cmd = Nothing

        Catch ex As Exception
            errLog(Err.Description, "clsReport:getstorebyid")
            Err.Clear()
        End Try

        Return ds
    End Function

    Public Function getuser_edit(ByVal usr As String, ByVal shapsw As String) As DataSet

        Dim ds As New DataSet

        Dim str As String = ""
        Try
            Dim cCmd As New clsDbo("")

            str = "SELECT Usr,cast(SHA2(psw,512) as char) psw,custid,modulepriv,rptpriv,IFNULL(email,'') email,modul,rptf,invpriv FROM `cld_user` WHERE
                    usr = '" & usr & "' AND SHA2(psw,512)='" & shapsw & "'"


            Dim c_Cmd As New clsDbo(str)

            ds = c_Cmd.selectDataset()

            c_Cmd.dispose()
            c_Cmd = Nothing

        Catch ex As Exception
            errLog(Err.Description, "clsReport:getuser_edit")
            Err.Clear()
        End Try

        Return ds
    End Function

    Public Function Selectlist_User(Optional ByVal nama As String = "") As DataSet

        Dim ds As New DataSet
        Dim strfilter As String = ""

        Try
            Dim c_Cmd As New clsDbo(ServerString)

            'strfilter = "SELECT '0' AS ID, 'All' AS Nama UNION ALL " &
            strfilter = " SELECT u.usr,SHA2(u.psw,512) psw,modulepriv,CASE WHEN modulepriv = 0 THEN 'admin' ELSE 'user' END AS usertype,u.rptpriv,u.email,u.modul,
	                     CASE WHEN modul=1 THEN 'web_report' WHEN modul = 2 THEN 'member' ELSE 'Webrepot and member' END AS modultype,rptf,invpriv
	                     FROM `cld_user` u
                     HAVING usr LIKE '%" & nama & "%' OR email LIKE '%" & nama & "%' OR usertype LIKE '%" & nama & "%' OR rptpriv LIKE '%" & nama & "%' OR modul LIKE '%" & nama & "%'
                     ORDER BY usr"

            ds = c_Cmd.ReturnDataset(strfilter)

            c_Cmd.dispose()
            c_Cmd = Nothing

        Catch ex As Exception
            errLog(Err.Description, "clsExportData:Selectlist_User")
            Err.Clear()
        End Try


        Return ds


    End Function
    Public Function getstoreuser(ByVal usr As String) As DataSet

        Dim ds As New DataSet

        Dim str As String = ""
        Try
            Dim cCmd As New clsDbo("")

            str = "select id,kode,nama from cld_outlet where id in (SELECT store FROM cld_userxstore WHERE user = '" & usr & "')"

            Dim c_Cmd As New clsDbo(str)

            ds = c_Cmd.selectDataset()

            c_Cmd.dispose()
            c_Cmd = Nothing

        Catch ex As Exception
            errLog(Err.Description, "clsReport:getstoreuser")
            Err.Clear()
        End Try

        Return ds
    End Function
    Public Function getstore_all(ByVal nama As String) As DataSet

        Dim ds As New DataSet

        Dim str As String = ""
        Try
            Dim cCmd As New clsDbo("")

            str = "select id,kode,nama from cld_outlet where cld_outlet.kode LIKE  '%" & nama & "%' OR cld_outlet.nama LIKE '%" & nama & "%' OR cld_outlet.Alamat LIKE '%" & nama & "%' ORDER BY Kode"

            Dim c_Cmd As New clsDbo(str)

            ds = c_Cmd.selectDataset()

            c_Cmd.dispose()
            c_Cmd = Nothing

        Catch ex As Exception
            errLog(Err.Description, "clsReport:getstoreitem")
            Err.Clear()
        End Try

        Return ds
    End Function
    Public Function GetIDOutlet(ByVal nama As String) As String
        Dim sHasil As String = ""

        Dim ds As New DataSet
        Dim strFilter As String = ""

        Try

            Dim a As New clsDbo("Select id from cld_outlet where nama  = '" & nama & "'")

            ds = a.selectDataset
            If ds.Tables(0).Rows.Count > 0 Then
                sHasil = ds.Tables(0).Rows(0)("id")
            Else
                sHasil = ""
            End If
            a.dispose()
            a = Nothing

        Catch ex As Exception

            errLog(Err.Description, "clsUser:GetIDOutlet")
            Err.Clear()

        End Try

        Return sHasil
    End Function
    Public Sub insert_userxoutlet(ByVal usr As String, ByVal Stores As List(Of UserList.UserxOutletInfo))
        Dim str As String
        Dim c_cmd As New Cls_command
        Dim countid As Integer
        Try
            str = "delete from cld_userxstore WHERE user = '" & usr & "'"
            c_cmd.executeNonQuery(str)
            For Each store As UserList.UserxOutletInfo In Stores

                countid = c_cmd.ExecuteScalar("select count(user) from cld_userxstore where user = " & usr & " and store = " & store.user & "", 0)
                If countid = 0 Then
                    str = "insert into cld_userxstore(User,store)values('" & store.user & "'," & store.outletid & ")"
                    c_cmd.executeNonQuery(str)
                Else
                    str = "update cld_userxstore set Store=" & store.outletid & " WHERE user = '" & store.user & "'"
                    c_cmd.executeNonQuery(str)
                End If
            Next
        Catch ex As Exception

            errLog(Err.Description, "clsUser:insert_userxoutlet")
            Err.Clear()
        End Try
    End Sub
    Public Function Selectdt_useroutlet(ByVal user As String, ByVal storeid As String, Optional ByVal nama As String = "") As DataTable
        Dim dt As New DataTable
        Dim strfilter As String = ""

        Try
            Dim c_Cmd As New clsDbo(ServerString)

            strfilter = "SELECT usr user,id outletid FROM
                    (SELECT u.usr,o.id FROM `cld_user` u
                    CROSS JOIN `cld_outlet` o WHERE (u.usr,o.id) NOT IN (SELECT USER,store FROM `cld_userxstore`) AND o.id IN(" & storeid & ")
                    UNION ALL
                    SELECT USER,store FROM cld_userxstore usx JOIN `cld_outlet` o ON usx.store = o.id AND o.id IN(" & storeid & ")
                    ) tbl1 
                    WHERE usr = '" & user & "' ORDER BY usr,id"

            dt = c_Cmd.ReturnDataset(strfilter).Tables(0)

            c_Cmd.dispose()
            c_Cmd = Nothing

        Catch ex As Exception
            errLog(Err.Description, "Clsitemmaster:Selectdt_itemstore")
            Err.Clear()
        End Try


        Return dt
    End Function
End Class
