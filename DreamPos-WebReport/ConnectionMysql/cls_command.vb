Imports MySql.Data.MySqlClient

Public Class Cls_command

    Private mycmd As New MySqlCommand

    Public Sub New()
        mycmd.Connection = GetConnection()
    End Sub

    Public Sub ReNew()
        mycmd.Connection = GetConnection()
    End Sub

    'Revisi 16 Maret 2017
    'untuk Feedback
    Public Sub New(ByVal sDB As String)
        mycmd.Connection = GetConnectionFeedback(sDB)
    End Sub

    Public Function connection()
        Return mycmd.Connection
    End Function

    Public Function GetConnection() As MySqlConnection
        Return New MySqlConnection(ServerString)
    End Function

    Public Function GetConnectionFeedback(ByVal sDB As String) As MySqlConnection

        Dim strcon As String = ""
        Dim strObjeck = ServerString.ToString.Split(";")
        Dim iCount As Integer = 0

        For Each word In strObjeck
            If iCount < 3 Then
                strcon += word & ";"
                iCount += 1
            Else
                Exit For
            End If

        Next

        strcon = strcon & "database=" & sDB

        Return New MySqlConnection(strcon)

    End Function

    'Public Function executeNonQueryMember(ByVal strQuery As String, ByVal id As Integer, ByVal Number As String, ByVal name As String, ByVal memberType As Integer, ByVal JoinDate As Date, ByVal ExpireDate As Date, ByVal ParentID As Integer, ByVal sex As String, ByVal Religion As String, ByVal Address As String, ByVal mobileno As String, ByVal email As String, ByVal status As String _
    '                       , ByVal Dob As Date, ByVal dMarrige As Date, ByVal saldo As Integer, ByVal Points As Decimal, ByVal Notes As String, ByVal CustId As Integer, ByVal img As Byte()) As Boolean

    '    Dim bhasil As Boolean = False

    '    Try


    '        mycmd.CommandText = strQuery

    '        mycmd.Parameters.AddWithValue("@id", id)

    '        mycmd.Parameters.AddWithValue("@number", Number)
    '        mycmd.Parameters.AddWithValue("@name", name)
    '        mycmd.Parameters.AddWithValue("@membertype", memberType)

    '        mycmd.Parameters.AddWithValue("@joindate", JoinDate.ToString("yyyy-MM-dd HH:mm:ss"))
    '        mycmd.Parameters.AddWithValue("@expiredate", ExpireDate.ToString("yyyy-MM-dd HH:mm:ss"))
    '        mycmd.Parameters.AddWithValue("@parentID", ParentID)
    '        mycmd.Parameters.AddWithValue("@sex", sex)
    '        mycmd.Parameters.AddWithValue("@religion", Religion)
    '        mycmd.Parameters.AddWithValue("@address", Address)
    '        mycmd.Parameters.AddWithValue("@mobileNo", mobileno)
    '        mycmd.Parameters.AddWithValue("@email", email)
    '        mycmd.Parameters.AddWithValue("@status", status)
    '        mycmd.Parameters.AddWithValue("@dob", Dob.ToString("yyyy-MM-dd HH:mm:ss"))

    '        'If Year(dMarrige) = 1910 Then
    '        '    mycmd.Parameters.AddWithValue("@dmarrige", System.DBNull.Value)
    '        'Else
    '        mycmd.Parameters.AddWithValue("@dmarrige", dMarrige.ToString("yyyy-MM-dd HH:mm:ss"))
    '        'End If

    '        mycmd.Parameters.AddWithValue("@saldo", saldo)
    '        mycmd.Parameters.AddWithValue("@points", Points)
    '        mycmd.Parameters.AddWithValue("@notes", Notes)
    '        mycmd.Parameters.AddWithValue("@CustId", CustId)
    '        mycmd.Parameters.AddWithValue("@image", img)
    '        mycmd.Parameters.AddWithValue("@LastTrans", Now.ToString("yyyy-MM-dd HH:mm:ss"))

    '        mycmd.Connection.Open()

    '        mycmd.ExecuteNonQuery()
    '        bhasil = True

    '    Catch ex As Exception
    '        errLog(Err.Description, "Cls_command.executeNonQueryMember")
    '        Err.Clear()
    '    End Try

    '    closeConnection()

    '    Return bhasil
    'End Function



    'Public Function executeNonQueryMember2(ByVal strQuery As String, ByVal Number As String, ByVal name As String, ByVal memberType As Integer, ByVal JoinDate As Date, ByVal ExpireDate As Date, ByVal ParentID As Integer, ByVal sex As String, ByVal Religion As String, ByVal Address As String, ByVal mobileno As String, ByVal email As String, ByVal status As String _
    '                   , ByVal Dob As Date, ByVal dMarrige As Date, ByVal saldo As Integer, ByVal Points As Decimal, ByVal Notes As String, ByVal img As Byte()) As Boolean

    '    Dim bhasil As Boolean = False

    '    Try

    '        mycmd.CommandText = strQuery

    '        mycmd.Parameters.AddWithValue("@number", Number)
    '        mycmd.Parameters.AddWithValue("@name", name)
    '        mycmd.Parameters.AddWithValue("@membertype", memberType)

    '        mycmd.Parameters.AddWithValue("@joindate", JoinDate.ToString("yyyy-MM-dd HH:mm:ss"))
    '        mycmd.Parameters.AddWithValue("@expiredate", ExpireDate.ToString("yyyy-MM-dd HH:mm:ss"))

    '        If ParentID <> 0 Then
    '            mycmd.Parameters.AddWithValue("@parentID", ParentID)
    '        End If

    '        mycmd.Parameters.AddWithValue("@sex", sex)
    '        mycmd.Parameters.AddWithValue("@religion", Religion)
    '        mycmd.Parameters.AddWithValue("@address", Address)
    '        mycmd.Parameters.AddWithValue("@mobileNo", mobileno)
    '        mycmd.Parameters.AddWithValue("@email", email)
    '        mycmd.Parameters.AddWithValue("@status", status)
    '        mycmd.Parameters.AddWithValue("@dob", Dob.ToString("yyyy-MM-dd HH:mm:ss"))

    '        If dMarrige.Year = 1910 Then
    '            mycmd.Parameters.AddWithValue("@dmarrige", System.DBNull.Value)
    '        Else
    '            mycmd.Parameters.AddWithValue("@dmarrige", dMarrige.ToString("yyyy-MM-dd HH:mm:ss"))
    '        End If

    '        'mycmd.Parameters.AddWithValue("@saldo", saldo)
    '        'mycmd.Parameters.AddWithValue("@points", Points)
    '        mycmd.Parameters.AddWithValue("@notes", Notes)
    '        mycmd.Parameters.AddWithValue("@image", img)

    '        mycmd.Connection.Open()

    '        mycmd.ExecuteNonQuery()
    '        bhasil = True

    '    Catch ex As Exception
    '        errLog(Err.Description, "Cls_command.executeNonQueryMember2")
    '        Err.Clear()
    '    End Try

    '    closeConnection()

    '    Return bhasil

    'End Function

    'Public Sub executeNonQuery_Inv_CalcCost(ByVal istk As Integer, ByVal icc As Integer, ByVal CustId As Integer)
    '    'Dim cmd As New MySqlCommand("sp_insert_Inv_Trans", GetConnection)
    '    'cmd.CommandType = CommandType.StoredProcedure

    '    Try

    '        mycmd.CommandText = "sp_process_stkcost"
    '        mycmd.CommandType = CommandType.StoredProcedure

    '        With mycmd.Parameters
    '            .Clear()

    '            .AddWithValue("@dStart", Date.Now.ToString("yyyy-MM-dd"))
    '            .AddWithValue("@dEnd", Date.Now.ToString("yyyy-MM-dd"))
    '            .AddWithValue("@iStk", istk)
    '            .AddWithValue("@iCc", icc)
    '            .AddWithValue("@iCustid", CustId)

    '        End With
    '        mycmd.Connection.Open()

    '        mycmd.ExecuteNonQuery()

    '        'closeConnection()
    '        'mycmd.CommandType = Nothing

    '    Catch ex As Exception
    '        errLog(Err.Description, "Cls_command.executeNonQuery_Inv_CalcCost")
    '        Err.Clear()
    '    End Try

    '    closeConnection()
    '    mycmd.CommandType = Nothing

    'End Sub

    ''Revisi 12 Okt
    'Public Sub executeNonQuery_Inv_CalcCostByDate(ByVal dstart As Date, ByVal dend As Date, ByVal istk As Integer, ByVal icc As Integer, ByVal CustId As Integer)
    '    'Dim cmd As New MySqlCommand("sp_insert_Inv_Trans", GetConnection)
    '    'cmd.CommandType = CommandType.StoredProcedure

    '    Try

    '        mycmd.CommandText = "sp_process_stkcost"
    '        mycmd.CommandType = CommandType.StoredProcedure

    '        With mycmd.Parameters
    '            .Clear()

    '            .AddWithValue("@dStart", dstart.Date.ToString("yyyy-MM-dd"))
    '            .AddWithValue("@dEnd", dend.Date.ToString("yyyy-MM-dd"))
    '            .AddWithValue("@iStk", istk)
    '            .AddWithValue("@iCc", icc)
    '            .AddWithValue("@iCustid", CustId)

    '        End With
    '        mycmd.Connection.Open()

    '        mycmd.ExecuteNonQuery()


    '    Catch ex As Exception
    '        errLog(Err.Description, "Cls_command.executeNonQuery_Inv_CalcCostByDate")
    '        Err.Clear()
    '    End Try

    '    closeConnection()
    '    mycmd.CommandType = Nothing
    'End Sub

    'Public Sub executeNonQuery_Inv_CalcStock(ByVal istk As Integer, ByVal icc As Integer, ByVal CustId As Integer)

    '    Try

    '        mycmd.CommandText = "sp_process_transstock"
    '        mycmd.CommandType = CommandType.StoredProcedure

    '        With mycmd.Parameters

    '            .Clear()

    '            .AddWithValue("@dStart", Date.Now.ToString("yyyy-MM-dd"))
    '            .AddWithValue("@dEnd", Date.Now.ToString("yyyy-MM-dd"))
    '            .AddWithValue("@iStk", istk)
    '            .AddWithValue("@iCc", icc)
    '            .AddWithValue("@iCustid", CustId)

    '        End With
    '        mycmd.Connection.Open()

    '        mycmd.ExecuteNonQuery()

    '    Catch ex As Exception
    '        errLog(Err.Description, "Cls_command.executeNonQuery_Inv_CalcStock")
    '        Err.Clear()
    '    End Try

    '    closeConnection()
    '    mycmd.CommandType = Nothing

    'End Sub

    'Public Sub executeNonQuery_Inv_ProcessCls(ByVal dstart As Date, ByVal dend As Date, ByVal CustId As Integer)

    '    Try

    '        mycmd.CommandText = "sp_process_transstockcls"
    '        mycmd.CommandType = CommandType.StoredProcedure

    '        With mycmd.Parameters

    '            .Clear()

    '            .AddWithValue("@dStart", dstart.ToString("yyyy-MM-dd"))
    '            .AddWithValue("@dEnd", dend.ToString("yyyy-MM-dd"))
    '            .AddWithValue("@iCustid", CustId)

    '        End With
    '        mycmd.Connection.Open()

    '        mycmd.ExecuteNonQuery()

    '    Catch ex As Exception
    '        errLog(Err.Description, "Cls_command.executeNonQuery_Inv_ProcessCls(")
    '        Err.Clear()
    '    End Try

    '    closeConnection()
    '    mycmd.CommandType = Nothing

    'End Sub

    'Public Sub executeNonQuerysp_test()

    '    mycmd.CommandText = "sp_test"
    '    mycmd.CommandType = CommandType.StoredProcedure

    '    mycmd.Connection.Open()

    '    mycmd.ExecuteNonQuery()

    '    closeConnection()
    '    mycmd.CommandType = Nothing
    'End Sub



    'perubahan 27 agustus 2018, kurangin klo error
    Public Function ExecuteScalar(ByVal strQuery As String, Optional ByVal tipe As Integer = 1)

        'perubahan 30 okt 2018
        '1 nilai nullnya 1 dan nilainya akan ditambah 1
        '10 nilai nullnya '' dan nilainya biasa (String) (Untuk cek server Version)
        '100 nilai nullnya -3550 dan nilai biasa (untuk cek perbedaan salesDif)
        'else nilai nullnya 0 dan nilainya biasa

        Dim hasil As String = 0

        mycmd.CommandText = strQuery
        mycmd.Connection.Open()

        Try

            If tipe = 1 Then

                If IsDBNull(mycmd.ExecuteScalar()) Then
                    hasil = 1
                Else
                    hasil = Val(mycmd.ExecuteScalar()) + 1
                End If

            ElseIf tipe = 10 Then

                If IsDBNull(mycmd.ExecuteScalar()) Then
                    hasil = ""
                ElseIf IsNothing(mycmd.ExecuteScalar()) Then
                    hasil = ""
                Else
                    hasil = mycmd.ExecuteScalar()
                End If

            ElseIf tipe = 100 Then

                If IsDBNull(mycmd.ExecuteScalar()) Then
                    hasil = -3550
                ElseIf IsNothing(mycmd.ExecuteScalar()) Then
                    hasil = -3550
                Else
                    hasil = mycmd.ExecuteScalar()
                End If

            Else

                If IsDBNull(mycmd.ExecuteScalar()) Then
                    hasil = 0
                Else
                    hasil = mycmd.ExecuteScalar()
                End If

            End If

        Catch ex As Exception
            errLog(Err.Description, "Cls_command.ExecuteScalar")
            Err.Clear()
        Finally
            closeConnection()
        End Try

        Return hasil

    End Function

    'return String
    'Public Function ExecuteScalar22323(ByVal strQuery As String, Optional ByVal tipe As Integer = 1)

    '    Try

    '        Dim hasil As String = ""

    '        mycmd.CommandText = strQuery
    '        mycmd.Connection.Open()
    '        If tipe = 1 Then

    '            If IsDBNull(mycmd.ExecuteNonQuery()) Then
    '                hasil = 1
    '            Else
    '                hasil = Val(mycmd.ExecuteNonQuery()) + 2
    '            End If

    '        Else
    '            hasil = mycmd.ExecuteScalar
    '        End If

    '        closeConnection()

    '        Return hasil

    '    Catch ex As Exception
    '        errLog(Err.Description, "Cls_command.ExecuteScalar2")
    '        Err.Clear()
    '    End Try

    'End Function

    'perubahan 6 juni 2017 close connection di taruh diluar try biar ga error

    'perubahan 13 agustus 2018
    '-di tambahin klo strQuerynya kosong, jangan di process
    Public Function executeNonQuery(ByVal strQuery As String) As Boolean

        Dim hslFunction As Boolean = False

        Try

            If strQuery <> "" Then
                mycmd.CommandText = strQuery
                mycmd.CommandTimeout = 300
                mycmd.Connection.Open()

                mycmd.ExecuteNonQuery()

                hslFunction = True
            End If


        Catch ex As Exception
            errLog(strQuery & " " & Err.Description, "Cls_command.executeNONQuery")
            Err.Clear()

        End Try

        If strQuery <> "" Then
            'close, dluar try, jika error tetep close connection
            closeConnection()
        End If

        Return hslFunction

    End Function

    'Penambahan 6 juni 2017
    'function, will return bo0lean
    Public Function executeNonQueryImport(ByVal strQuery As String) As Boolean

        Dim hslFunction As Boolean = False

        mycmd.CommandText = strQuery
        mycmd.Connection.Open()

        Try

            mycmd.ExecuteNonQuery()

            hslFunction = True

        Catch ex As Exception
            errLog(Err.Description, "Cls_command.executeNonQueryImport")
            Err.Clear()

        End Try

        closeConnection()

        Return hslFunction

    End Function


    Public Function executeNonQuery_Sum(ByVal strQuery As String) As Boolean

        Dim hslFunction As Boolean = False

        Try

            If strQuery <> "" Then
                mycmd.CommandText = strQuery
                mycmd.CommandTimeout = 360

                mycmd.Connection.Open()

                ' READ COMMITTED: mencegah phantom-read lock pada INSERT...SELECT dan query agregat
                ' sehingga tidak perlu menunggu shared lock dari transaksi lain selesai
                Dim cmdIso As New MySqlCommand("SET SESSION TRANSACTION ISOLATION LEVEL READ COMMITTED", mycmd.Connection)
                cmdIso.ExecuteNonQuery()
                cmdIso.Dispose()

                mycmd.ExecuteNonQuery()

                hslFunction = True
            End If


        Catch ex As Exception
            errLog(strQuery & " " & Err.Description, "executeNonQuery_Sum")
            Err.Clear()
        End Try

        ' Perbaikan: tutup koneksi agar tidak ada connection leak yang menahan lock
        If strQuery <> "" Then
            closeConnection()
        End If

        Return hslFunction

    End Function


    Public Function executeReader(ByVal strquery As String) As Boolean

        Try

            Dim bHasil As Boolean = False
            Dim rdr As MySqlDataReader


            mycmd.CommandText = strquery
            mycmd.Connection.Open()

            rdr = mycmd.ExecuteReader()

            If rdr.HasRows Then
                bHasil = True
            End If


            rdr.Close()

            closeConnection()

            Return bHasil

        Catch ex As Exception
            errLog(Err.Description, "Cls_command.executeReader")
            Err.Clear()
            Return False
        End Try

    End Function



    'Public Sub executeSendEMail(ByVal strQuery As String, ByVal transdate As Date, ByVal Subject As String, ByVal img As Byte(), ByVal Header As String, ByVal content As String, ByVal mtype As Integer, ByVal mtypename As String, ByVal createdby As String, ByVal imagepath As String, ByVal imagelink As String)

    '    Try
    '        mycmd.CommandText = strQuery

    '        mycmd.Parameters.AddWithValue("@Transdate", transdate.ToString("yyyy-MM-dd HH:mm:ss"))

    '        mycmd.Parameters.AddWithValue("@Subject", Subject)
    '        mycmd.Parameters.AddWithValue("@ImageDir", img)
    '        mycmd.Parameters.AddWithValue("@Header", Header)
    '        mycmd.Parameters.AddWithValue("@Content", content)
    '        mycmd.Parameters.AddWithValue("@Mtype", mtype)
    '        mycmd.Parameters.AddWithValue("@Mtypename", mtypename)
    '        mycmd.Parameters.AddWithValue("@Createdby", createdby)
    '        mycmd.Parameters.AddWithValue("@Imagepath", imagepath)
    '        mycmd.Parameters.AddWithValue("@ImageLink", imagelink)
    '        mycmd.Connection.Open()

    '        mycmd.ExecuteNonQuery()

    '    Catch ex As Exception
    '        errLog(Err.Description, "Cls_command.executeSendEMail")
    '        Err.Clear()
    '    End Try

    '    closeConnection()

    'End Sub


    Public Sub closeConnection()
        Try

            If mycmd.Connection.State = ConnectionState.Open Then

                mycmd.Connection.Close()
                mycmd.Dispose()

            Else
                mycmd.Dispose()
            End If

        Catch ex As Exception
            errLog(Err.Description, "Cls_command.closeConnection")
            Err.Clear()
        End Try

    End Sub

    'perubahan 6 juni 2017 close connection di taruh diluar try biar ga error
    Public Function ReturnDataset(ByVal strquery As String) As DataSet

        Dim ds As New DataSet

        Try
            mycmd.CommandText = strquery
            mycmd.CommandTimeout = 900
            Dim da As New MySqlDataAdapter(mycmd.CommandText, mycmd.Connection)

            Try
                da.Fill(ds)
            Catch ex As Exception
                errLog(Err.Description & "-" & strquery, "Cls_command:ReturnDataset:FillData")
                Err.Clear()
            End Try

            da.Dispose()
            closeConnection()

        Catch ex As Exception
            errLog(Err.Description & "-" & strquery, "Cls_command:ReturnDataset")
            Err.Clear()
        End Try

        Return ds

    End Function

    'perubahan 6 juni 2017 close connection di taruh diluar try biar ga error
    Public Function ReturnDataset_Table(ByVal strquery As String, ByVal tblName As String) As DataSet

        Dim ds As New DataSet

        Try

            mycmd.CommandText = strquery

            Dim da As New MySqlDataAdapter(mycmd.CommandText, mycmd.Connection)

            Try
                da.Fill(ds, tblName)
            Catch ex As Exception
                errLog(Err.Description & "-" & strquery, "Cls_command:ReturnDataset_Table:FillData")
                Err.Clear()
            End Try

            da.Dispose()

        Catch ex As Exception
                errLog(Err.Description, "Cls_command:ReturnDataset_Table")
            Err.Clear()
        End Try

        Return ds

    End Function

End Class
