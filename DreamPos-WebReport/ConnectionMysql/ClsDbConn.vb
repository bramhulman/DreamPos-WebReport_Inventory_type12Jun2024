Imports MySql.Data.MySqlClient
Imports MySql.Data.Types


Public Class clsDbConn
    Private myCmd As New MySqlCommand
    Private myConn As MySqlConnection
    Private myisConnect As Boolean = False
    Private myRdr As MySqlDataReader
    Private myConnStr As String

    Public Sub New(ByVal connStr As String)
        Try
            myConnStr = connStr

            myConn = New MySqlConnection(connStr)

            myCmd.Connection = myConn
            myCmd.CommandTimeout = 900
            resetCmd("Select 1 as cekConn")
            myisConnect = CBool(Val(executeScaler))

        Catch ex As Exception
            errLog(Err.Description, "clsDbConnReport:New: " & connStr)
            Err.Clear()
        End Try
    End Sub

    Public ReadOnly Property isConnect() As Boolean
        Get
            Return myisConnect
        End Get
    End Property

    Public ReadOnly Property reader() As MySqlDataReader
        Get
            Return myRdr
        End Get
    End Property

    Public Sub executeReader(ByVal StrQuery As String)

        resetCmd(StrQuery)

        'open koneksi
        If myCmd.Connection.State = ConnectionState.Closed Then
            myCmd.Connection.Open()
        End If

        myRdr = myCmd.ExecuteReader
    End Sub

    Public Sub closeReader()
        If Not myRdr.IsClosed Then
            myRdr.Close()
        End If
    End Sub

    Public ReadOnly Property connStr() As String
        Get
            Return myConnStr
        End Get
    End Property


    Public Sub resetCmd(ByVal strQuery As String)
        Try
            myCmd.Parameters.Clear()
            myCmd.CommandText = strQuery

        Catch ex As Exception
            errLog(Err.Description, "clsDbConnReport:resetCmd")
            Err.Clear()
        End Try
    End Sub

    Public Sub addParam(ByVal paramValue As Object, ByVal paramName As String, ByVal paramType As MySqlDbType)
        Try
            'Dim newParam As SqlParameter = myCmd.CreateParameter

            'newParam.ParameterName = paramName
            'newParam.Value = paramValue
            'newParam.DbType = paramType

            'myCmd.Parameters.Add(newParam)

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
            errLog(Err.Description, "clsDbConnReport:addParam:" & myCmd.CommandText)
            Err.Clear()
        End Try
    End Sub



    Public Function executeScaler() As String
        Dim strRslt As String = ""
        Dim _qToken As String = clsQueryGuard.BeginQuery(myCmd.CommandText, "clsDbConn.executeScaler")
        Dim _sw As New Diagnostics.Stopwatch()

        'Debug.Print(myCmd.CommandText)
        Try
            'open koneksi
            If myCmd.Connection.State = ConnectionState.Closed Then
                myCmd.CommandTimeout = clsQueryGuard.DefaultTimeoutSeconds
                myCmd.Connection.Open()
            End If
            Dim objRslt As Object

            _sw.Start()
            objRslt = myCmd.ExecuteScalar
            _sw.Stop()

            If Not objRslt Is Nothing Then
                strRslt = objRslt.ToString
            End If

            clsQueryGuard.CommitQuery(_qToken) ' sukses

            If _sw.ElapsedMilliseconds >= clsQueryGuard.SlowQueryThresholdMs Then
                clsQueryGuard.LogSlowQuery(myCmd.CommandText, "clsDbConn.executeScaler", _sw.ElapsedMilliseconds)
            End If

        Catch ex As Exception
            _sw.Stop()
            clsQueryGuard.MarkTimeout(_qToken, myCmd.CommandText, "clsDbConn.executeScaler", ex) ' gagal
            errLog(Err.Description, "clsDbConnReport:executeScaler: " & myCmd.CommandText)
            Err.Clear()
        End Try

        Return strRslt

    End Function


    Public Function selectDataset(Optional ByVal strQuery As String = "") As DataSet
        Dim ds As New DataSet

        Try
            If strQuery <> "" Then
                resetCmd(strQuery)
            End If

            Using da As New MySqlDataAdapter(myCmd.CommandText, myCmd.Connection)

                For Each param As MySqlParameter In myCmd.Parameters
                    da.SelectCommand.Parameters.AddWithValue(param.ParameterName, param.Value)
                Next

                Try
                    da.Fill(ds)
                Catch ex As Exception
                    errLog(Err.Description, "clsDbConnReport:selectDataset-FillData")
                    Err.Clear()
                End Try

                da.Dispose()
            End Using
        Catch ex As Exception
            errLog(Err.Description, "clsDbConnReport:selectDataset")
            Err.Clear()
        End Try

        Return ds

    End Function

    Public Sub dispose()
        Try
            myCmd.Connection.Close()
            myCmd.Dispose()

        Catch ex As Exception
            errLog(Err.Description, "clsDbConnReport:dispose")
            Err.Clear()
        End Try
    End Sub


End Class
