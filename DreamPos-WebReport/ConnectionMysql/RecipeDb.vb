Imports MySql.Data.MySqlClient
Imports System.Collections.Generic

' One connection and transaction for each Recipe write operation.
Public NotInheritable Class RecipeDb
    Implements IDisposable

    Private ReadOnly connection As MySqlConnection
    Private ReadOnly transaction As MySqlTransaction
    Private ReadOnly parameters As New Dictionary(Of String, Object)()
    Private committed As Boolean

    Public Sub New()
        connection = New MySqlConnection(My.Settings.ServerStr)
        Try
            connection.Open()
            transaction = connection.BeginTransaction()
        Catch
            connection.Dispose()
            Throw
        End Try
    End Sub

    Public Sub SetParameter(ByVal name As String, ByVal value As Object)
        parameters("@" & name.TrimStart("@"c)) = If(value, DBNull.Value)
    End Sub

    Private Function CreateCommand(ByVal sql As String) As MySqlCommand
        Dim command As New MySqlCommand(sql, connection, transaction)
        command.CommandTimeout = clsQueryGuard.DefaultTimeoutSeconds
        For Each parameter In parameters
            command.Parameters.AddWithValue(parameter.Key, parameter.Value)
        Next
        Return command
    End Function

    Private Function Run(Of T)(ByVal sql As String, ByVal operation As Func(Of MySqlCommand, T)) As T
        Dim token As String = clsQueryGuard.BeginQuery(sql, "RecipeDb")
        Dim elapsed As Diagnostics.Stopwatch = Diagnostics.Stopwatch.StartNew()
        Try
            Using command = CreateCommand(sql)
                Dim result As T = operation(command)
                elapsed.Stop()
                clsQueryGuard.CommitQuery(token)
                If elapsed.ElapsedMilliseconds >= clsQueryGuard.SlowQueryThresholdMs Then
                    clsQueryGuard.LogSlowQuery(sql, "RecipeDb", elapsed.ElapsedMilliseconds)
                End If
                Return result
            End Using
        Catch ex As Exception
            elapsed.Stop()
            clsQueryGuard.MarkTimeout(token, sql, "RecipeDb", ex)
            mHelper.errLog(ex.ToString(), "RecipeDb")
            Throw
        End Try
    End Function

    Public Function Scalar(ByVal sql As String) As Object
        Return Run(sql, Function(command) command.ExecuteScalar())
    End Function

    Public Function ReadTable(ByVal sql As String) As DataTable
        Return Run(sql, Function(command)
                            Using adapter As New MySqlDataAdapter(command)
                                Dim result As New DataTable()
                                Try
                                    adapter.Fill(result)
                                    Return result
                                Catch
                                    result.Dispose()
                                    Throw
                                End Try
                            End Using
                        End Function)
    End Function

    Public Sub Execute(ByVal sql As String)
        Run(sql, Function(command) command.ExecuteNonQuery())
    End Sub

    Public Sub Commit()
        transaction.Commit()
        committed = True
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        Try
            If Not committed Then
                Try
                    transaction.Rollback()
                Catch ex As Exception
                    mHelper.errLog(ex.ToString(), "RecipeDb.Rollback")
                End Try
            End If
        Finally
            transaction.Dispose()
            connection.Dispose()
        End Try
    End Sub
End Class
