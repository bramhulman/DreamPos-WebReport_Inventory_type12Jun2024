Imports MySql.Data.MySqlClient
Imports MySql.Data.Types

Public Class clsDbo
    Private myCmd As New MySqlCommand
    Private mfunction As ModFunction
    Private myConn As MySqlConnection
    Private myRdr As MySqlDataReader

    Public Sub New(ByVal queryString As String)

        myConn = New MySqlConnection(My.Settings.ServerStr)

        myCmd.Connection = myConn
        myCmd.CommandText = queryString

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
                mfunction.errLog(Err.Description, "clsDbo:selectDataset-FillData")
                Err.Clear()
            End Try

            da.Dispose()

        Catch ex As Exception
            mfunction.errLog(Err.Description, "clsDbo:selectDataset")
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
                mfunction.errLog(Err.Description, "clsDbo:selectDatatable-FillData")
                Err.Clear()
            End Try
            da.Dispose()

        Catch ex As Exception
            mfunction.errLog(Err.Description, "clsDbo:selectDatatable")
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
            mfunction.errLog(Err.Description, "clsDbo:addParam:" & myCmd.CommandText)
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
                mfunction.errLog(Err.Description & "-" & strquery, "Cls_command:ReturnDataset:FillData")
                Err.Clear()
            End Try

            da.Dispose()
            dispose()

        Catch ex As Exception
            mfunction.errLog(Err.Description & "-" & strquery, "Cls_command:ReturnDataset")
            Err.Clear()
        End Try

        Return ds

    End Function

    Public Function executeNonQuery(ByVal strQuery As String) As Boolean

        Dim hslFunction As Boolean = False

        Try

            If strQuery <> "" Then
                myCmd.CommandText = strQuery
                myCmd.CommandTimeout = 300
                myCmd.Connection.Open()

                myCmd.ExecuteNonQuery()

                hslFunction = True
            End If


        Catch ex As Exception
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

        myCmd.Connection.Close()
        myCmd.Dispose()

    End Sub

    Public Function SelectDataset_Icc_SelectionReport(ByVal custid As Integer, ByVal usr As String, ByVal modpriv As Integer) As DataSet
        Dim ds As New DataSet
        Dim strFilter As String = ""

        If modpriv And mfunction.Modulepriv.moduleuser = mfunction.Modulepriv.moduleuser Then

            strFilter = "SELECT '0' AS ID, 'All Cc' AS Nama UNION ALL " &
                        "SELECT ID , NAME FROM cld_inv_cc " &
                        "where cld_inv_cc.custid = " & custid & " and olid IN (SELECT Store FROM cld_userxstore WHERE USER = '" & usr & "')"
        Else
            strFilter = "SELECT '0' AS ID, 'All Store' AS Nama UNION ALL " & _
                        "SELECT ID , NAME FROM cld_inv_cc " & _
                        "where cld_inv_cc.custid = " & custid

        End If


        Dim a As New clsDbo(strFilter)


        ds = a.selectDataset

        a.dispose()
        a = Nothing

        Return ds

    End Function
    Public Sub executeNonQuery(Optional ByVal cmdType As CommandType = CommandType.Text)
        Try
            myCmd.CommandType = cmdType

            'open koneksi
            If myCmd.Connection.State = ConnectionState.Closed Then
                myCmd.Connection.Open()
            End If

            myCmd.ExecuteNonQuery()

            myCmd.Connection.Close()
        Catch ex As Exception
            mfunction.errLog(Err.Description, "clsdDbo:executeNonQuery: " & myCmd.CommandText, "")
            Err.Clear()
        End Try
    End Sub
    Public Sub resetCmd(ByVal strQuery As String)
        Try
            myCmd.Parameters.Clear()
            myCmd.CommandText = strQuery

        Catch ex As Exception
            mfunction.errLog(Err.Description, "clsDbConnReport:resetCmd")
            Err.Clear()
        End Try
    End Sub


End Class
