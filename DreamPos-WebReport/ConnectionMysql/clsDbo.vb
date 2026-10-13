Imports MySql.Data.MySqlClient
Imports MySql.Data.Types

Public Class clsDbo
    Private myCmd As New MySqlCommand

    Private myConn As MySqlConnection
    Private myRdr As MySqlDataReader

    Public Sub New(ByVal queryString As String, Optional ByVal conn As Integer = 0)
        If conn = 0 Then
            myConn = New MySqlConnection(My.Settings.ServerStr)
        ElseIf conn = 1 Then
            myConn = New MySqlConnection(My.Settings.slssvr)
        End If

        myCmd.Connection = myConn
        myCmd.CommandText = queryString

    End Sub


    Public Sub ReNew()

        myConn = New MySqlConnection(ServerString)
        myCmd.Connection = myConn

    End Sub


    Public Function selectDataset(Optional ByVal bparam As Boolean = False) As DataSet
        Dim ds As New DataSet

        Try
            Dim da As New MySqlDataAdapter(myCmd.CommandText, myCmd.Connection)

            If bparam Then

                For Each param As MySqlParameter In myCmd.Parameters
                    da.SelectCommand.Parameters.AddWithValue(param.ParameterName, param.Value)
                Next

            End If

            Try
                da.Fill(ds)
            Catch ex As Exception
                errLog(Err.Description, "clsDbo:selectDataset-FillData")
                Err.Clear()
            End Try

            da.Dispose()

        Catch ex As Exception
            errLog(Err.Description, "clsDbo:selectDataset")
            Err.Clear()
        End Try


        Return ds
    End Function


    Public Function selectDatatable() As DataTable
        Dim dt As New DataTable

        Try
            dt.TableName = "MySampleTable"

            Dim da As New MySqlDataAdapter(myCmd.CommandText, myCmd.Connection)
            Try
                da.Fill(dt)
            Catch ex As Exception
                errLog(Err.Description, "clsDbo:selectDatatable-FillData")
                Err.Clear()
            End Try
            da.Dispose()

        Catch ex As Exception
            errLog(Err.Description, "clsDbo:selectDatatable")
            Err.Clear()
        End Try

        Return dt
    End Function


    Public Sub addParam(ByVal paramValue As Object, ByVal paramName As String, ByVal paramType As MySqlDbType)
        Try


            If paramType = MySqlDbType.LongBlob Or paramType = MySqlDbType.Binary Or paramType = MySqlDbType.VarBinary Then
                If IsDBNull(paramValue) Then
                    Dim bytPhoto As Byte() = Convert.FromBase64String("AA==")
                    myCmd.Parameters.AddWithValue(paramName, bytPhoto)
                Else
                    myCmd.Parameters.AddWithValue(paramName, paramValue)
                End If
            Else
                If paramValue Is Nothing Then
                    Select Case paramType
                        Case MySqlDbType.Decimal, MySqlDbType.Int32, MySqlDbType.Int24, MySqlDbType.Int16, MySqlDbType.Decimal, MySqlDbType.Float, MySqlDbType.Bit
                            myCmd.Parameters.AddWithValue(paramName, 0)
                        Case MySqlDbType.VarChar
                            myCmd.Parameters.AddWithValue(paramName, "")
                        Case Else
                            myCmd.Parameters.AddWithValue(paramName, DBNull.Value)
                    End Select

                Else
                    myCmd.Parameters.AddWithValue(paramName, paramValue)
                End If
            End If



        Catch ex As Exception
            errLog(Err.Description, "clsDbo:addParam:" & myCmd.CommandText)
            Err.Clear()
        End Try
    End Sub

    'perubahan 6 juni 2017 close connection di taruh diluar try biar ga error
    Public Function ReturnDataset(ByVal strquery As String) As DataSet

        Dim ds As New DataSet

        Try
            myCmd.CommandText = strquery
            myCmd.CommandTimeout = 3600
            Dim da As New MySqlDataAdapter(myCmd.CommandText, myCmd.Connection)

            Try
                da.Fill(ds)
            Catch ex As Exception
                errLog(Err.Description & "-" & strquery, "Cls_command:ReturnDataset:FillData")
                Err.Clear()
            End Try

            da.Dispose()
            dispose()

        Catch ex As Exception
            errLog(Err.Description & "-" & strquery, "Cls_command:ReturnDataset")
            Err.Clear()
        End Try

        Return ds

    End Function

    Public Function SelectDataset_Icc_SelectionReport(ByVal custid As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""

        Try
            If modpriv And usrModulepriv.moduleuser = usrModulepriv.moduleuser Then

                strFilter = "SELECT '0' AS ID, 'All Cc' AS Nama UNION ALL " &
                            "SELECT ID , NAME FROM cld_inv_cc " &
                            "where cld_inv_cc.custid = " & custid & " and olid IN (SELECT Store FROM cld_userxstore WHERE USER = '" & usr & "')"
            Else
                strFilter = "SELECT '0' AS ID, 'All Cc' AS Nama UNION ALL " &
                            "SELECT ID , NAME FROM cld_inv_cc " &
                            "where cld_inv_cc.custid = " & custid

            End If


            Dim a As New clsDbo(strFilter)


            ds = a.selectDataset
            a.dispose()
            a = Nothing
        Catch ex As Exception
            errLog(Err.Description, strFilter & "clsDbo.SelectDataset_Icc_SelectionReport")
            Err.Clear()
        End Try



        Return ds

    End Function

    Public Sub resetCmd(ByVal strQuery As String)
        Try
            myCmd.Parameters.Clear()
            myCmd.CommandText = strQuery

            ' errLog(strQuery, "CekDataReport")

        Catch ex As Exception
            errLog(Err.Description, "clsDbConnReport:resetCmd")
            Err.Clear()
        End Try
    End Sub



    'perubahan 27 agustus 2018, kurangin klo error
    Public Function ExecuteScalar(ByVal strQuery As String, Optional ByVal tipe As Integer = 1)

        'perubahan 30 okt 2018
        '1 nilai nullnya 1 dan nilainya akan ditambah 1
        '10 nilai nullnya '' dan nilainya biasa (String) (Untuk cek server Version)
        '100 nilai nullnya -3550 dan nilai biasa (untuk cek perbedaan salesDif)
        'else nilai nullnya 0 dan nilainya biasa

        If String.IsNullOrWhiteSpace(strQuery) Then Return "0"
        Dim hasil As String = 0
        Dim _qToken As String = clsQueryGuard.BeginQuery(strQuery, "clsDbo.ExecuteScalar")
        Dim _sw As New Diagnostics.Stopwatch()

        myCmd.CommandText = strQuery
        myCmd.CommandTimeout = clsQueryGuard.DefaultTimeoutSeconds
        myCmd.Connection.Open()

        Try
            _sw.Start()
            Dim scalarResult As Object = myCmd.ExecuteScalar()
            If tipe = 1 Then

                If IsDBNull(scalarResult) Then
                    hasil = 1
                Else
                    hasil = Val(scalarResult) + 1
                End If

            ElseIf tipe = 10 Then

                If IsDBNull(scalarResult) Then
                    hasil = ""
                ElseIf IsNothing(scalarResult) Then
                    hasil = ""
                Else
                    hasil = scalarResult
                End If

            ElseIf tipe = 100 Then

                If IsDBNull(scalarResult) Then
                    hasil = -3550
                ElseIf IsNothing(scalarResult) Then
                    hasil = -3550
                Else
                    hasil = scalarResult
                End If

            Else

                If IsDBNull(scalarResult) Then
                    hasil = 0
                Else
                    hasil = scalarResult
                End If

            End If
            _sw.Stop()
            clsQueryGuard.CommitQuery(_qToken) ' sukses

            If _sw.ElapsedMilliseconds >= clsQueryGuard.SlowQueryThresholdMs Then
                clsQueryGuard.MarkTimeout(_qToken, strQuery, "clsDbo.ExecuteScalar[SLOW " & _sw.ElapsedMilliseconds & "ms]",
                    New Exception("Slow query: " & _sw.ElapsedMilliseconds & "ms"))
            End If

        Catch ex As Exception
            _sw.Stop()
            clsQueryGuard.MarkTimeout(_qToken, strQuery, "clsDbo.ExecuteScalar", ex) ' gagal
            errLog(Err.Description, strQuery & "clsDbConnReport.ExecuteScalar")
            Err.Clear()
        Finally
            dispose()
        End Try

        Return hasil

    End Function

    Public Sub executeNonQuery2(Optional ByVal cmdType As CommandType = CommandType.Text)
        Try
            myCmd.CommandType = cmdType

            'open koneksi
            If myCmd.Connection.State = ConnectionState.Closed Then
                myCmd.Connection.Open()
            End If

            myCmd.ExecuteNonQuery()

            myCmd.Connection.Close()
        Catch ex As Exception
            errLog(Err.Description, "clsdDbo:executeNonQuery: " & myCmd.CommandText)
            Err.Clear()
        End Try
    End Sub

    Public Function executeNonQuery(ByVal strQuery As String) As Boolean
        If String.IsNullOrWhiteSpace(strQuery) Then Return False

        Dim hslFunction As Boolean = False
        Dim _qToken As String = clsQueryGuard.BeginQuery(strQuery, "clsDbo.executeNonQuery")
        Dim _sw As New Diagnostics.Stopwatch()

        Try

            If strQuery <> "" Then
                myCmd.CommandText = strQuery
                myCmd.CommandTimeout = clsQueryGuard.DefaultTimeoutSeconds
                myCmd.Connection.Open()

                _sw.Start()
                myCmd.ExecuteNonQuery()
                _sw.Stop()

                clsQueryGuard.CommitQuery(_qToken) ' sukses

                If _sw.ElapsedMilliseconds >= clsQueryGuard.SlowQueryThresholdMs Then
                    clsQueryGuard.MarkTimeout(_qToken, strQuery, "clsDbo.executeNonQuery[SLOW " & _sw.ElapsedMilliseconds & "ms]",
                        New Exception("Slow query: " & _sw.ElapsedMilliseconds & "ms"))
                End If

                hslFunction = True
            End If

        Catch ex As Exception
            _sw.Stop()
            clsQueryGuard.MarkTimeout(_qToken, strQuery, "clsDbo.executeNonQuery", ex) ' gagal
            errLog(strQuery & " " & Err.Description, "Cls_command.executeNONQuery")
            Err.Clear()

        End Try

        If strQuery <> "" Then
            'close, dluar try, jika error tetep close connection
            dispose()
        End If

        Return hslFunction

    End Function





    Public Sub dispose()

        Try

            If myCmd.Connection.State = ConnectionState.Open Then

                myCmd.Connection.Close()
                myCmd.Dispose()

            Else
                myCmd.Dispose()
            End If

        Catch ex As Exception
            errLog(Err.Description, "clsDbo.dispose")
            Err.Clear()
        End Try


    End Sub


End Class
