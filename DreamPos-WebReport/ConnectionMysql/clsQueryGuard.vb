Imports MySql.Data.MySqlClient
Imports System.IO

''' <summary>
''' clsQueryGuard — Query Safety Wrapper
''' Fitur:
'''   1. Timeout otomatis dengan nilai default yang bisa dikonfigurasi
'''   2. Pending Query Log: simpan query SEBELUM dieksekusi
'''      - Jika SUKSES  -> entry dihapus dari pending log
'''      - Jika CRASH/TIMEOUT -> entry tetap di pending log untuk analisa
'''   3. Durasi eksekusi dicatat ke slow_queries.log jika melebihi threshold
''' </summary>
Public Class clsQueryGuard

    ' == KONFIGURASI ================================================
    ''' <summary>Timeout per query dalam detik. Default 30 detik.</summary>
    Public Shared Property DefaultTimeoutSeconds As Integer = 30

    ''' <summary>Query lambat jika durasi (ms) melebihi nilai ini. Default 3000ms.</summary>
    Public Shared Property SlowQueryThresholdMs As Integer = 3000

    Public Shared ReadOnly Property PendingLogPath As String
        Get
            Dim dir As String = Path.GetDirectoryName(My.Settings.errLog)
            Return Path.Combine(dir, "pending_queries.log")
        End Get
    End Property

    Public Shared ReadOnly Property SlowLogPath As String
        Get
            Dim dir As String = Path.GetDirectoryName(My.Settings.errLog)
            Return Path.Combine(dir, "slow_queries.log")
        End Get
    End Property
    ' ================================================================

    ' == PENDING QUERY LOG ===========================================

    ''' <summary>
    ''' Simpan query ke pending log SEBELUM dieksekusi.
    ''' Return token unik. Format baris: [token]|[timestamp]|[caller]|[sql]
    ''' </summary>
    Public Shared Function BeginQuery(ByVal sql As String, ByVal caller As String) As String
        If String.IsNullOrWhiteSpace(sql) Then Return ""
        Dim token As String = Guid.NewGuid().ToString("N").Substring(0, 12)
        Dim entry As String = String.Format("{0}|{1}|{2}|{3}",
            token,
            Format(Now, "yyyy-MM-dd HH:mm:ss"),
            caller,
            sql.Replace(vbCrLf, " ").Replace(vbLf, " ").Trim())
        Try
            SyncLock GetType(clsQueryGuard)
                File.AppendAllText(PendingLogPath, entry & Environment.NewLine)
            End SyncLock
        Catch
        End Try
        Return token
    End Function

    ''' <summary>
    ''' Hapus entry dari pending log setelah query SUKSES.
    ''' </summary>
    Public Shared Sub CommitQuery(ByVal token As String)
        If String.IsNullOrWhiteSpace(token) Then Exit Sub
        Try
            SyncLock GetType(clsQueryGuard)
                If Not File.Exists(PendingLogPath) Then Exit Sub
                Dim lines() As String = File.ReadAllLines(PendingLogPath)
                Dim remaining As New List(Of String)
                For Each line As String In lines
                    If Not line.StartsWith(token) Then remaining.Add(line)
                Next
                File.WriteAllLines(PendingLogPath, remaining.ToArray())
            End SyncLock
        Catch
        End Try
    End Sub

    ''' <summary>
    ''' Tulis info timeout/crash ke slow query log (pending log entry dibiarkan).
    ''' </summary>
    Public Shared Sub MarkTimeout(ByVal token As String, ByVal sql As String, ByVal caller As String, ByVal ex As Exception)
        If String.IsNullOrWhiteSpace(token) AndAlso String.IsNullOrWhiteSpace(sql) Then Exit Sub
        Try
            Dim entry As String = String.Format("[TIMEOUT/CRASH] {0} | Token:{1} | Caller:{2} | Error:{3} | SQL:{4}",
                Format(Now, "yyyy-MM-dd HH:mm:ss"), token, caller,
                ex.Message.Replace(vbCrLf, " "),
                sql.Replace(vbCrLf, " ").Replace(vbLf, " ").Trim())
            SyncLock GetType(clsQueryGuard)
                File.AppendAllText(SlowLogPath, entry & Environment.NewLine)
            End SyncLock
        Catch
        End Try
    End Sub

    Private Shared Sub LogSlowQuery(ByVal sql As String, ByVal caller As String, ByVal elapsedMs As Long)
        Try
            Dim entry As String = String.Format("[SLOW {0}ms] {1} | Caller:{2} | SQL:{3}",
                elapsedMs, Format(Now, "yyyy-MM-dd HH:mm:ss"), caller,
                sql.Replace(vbCrLf, " ").Replace(vbLf, " ").Trim())
            SyncLock GetType(clsQueryGuard)
                File.AppendAllText(SlowLogPath, entry & Environment.NewLine)
            End SyncLock
        Catch
        End Try
    End Sub
    ' ================================================================

    ' == METODE EKSEKUSI QUERY ======================================

    ''' <summary>
    ''' ExecuteNonQuery aman: timeout, pending log, slow query log.
    ''' Return True jika sukses, False jika gagal/timeout.
    ''' </summary>
    Public Shared Function SafeExecuteNonQuery(
        ByVal sql As String,
        ByVal caller As String,
        Optional ByVal timeoutSec As Integer = -1) As Boolean

        If timeoutSec < 0 Then timeoutSec = DefaultTimeoutSeconds
        Dim token As String = BeginQuery(sql, caller)
        Dim sw As New Diagnostics.Stopwatch()
        Dim success As Boolean = False

        Try
            Using conn As New MySqlConnection(My.Settings.ServerStr)
                Using cmd As New MySqlCommand(sql, conn)
                    cmd.CommandTimeout = timeoutSec
                    conn.Open()
                    sw.Start()
                    cmd.ExecuteNonQuery()
                    sw.Stop()
                    success = True
                End Using
            End Using
            CommitQuery(token)
            If sw.ElapsedMilliseconds >= SlowQueryThresholdMs Then
                LogSlowQuery(sql, caller, sw.ElapsedMilliseconds)
            End If
        Catch ex As MySqlException When ex.Number = 0 OrElse ex.Message.ToLower().Contains("timeout")
            sw.Stop()
            MarkTimeout(token, sql, caller, ex)
            errLog("TIMEOUT (" & sw.ElapsedMilliseconds & "ms): " & ex.Message, caller)
        Catch ex As Exception
            sw.Stop()
            MarkTimeout(token, sql, caller, ex)
            errLog(ex.Message, caller)
        End Try

        Return success
    End Function

    ''' <summary>
    ''' ExecuteScalar aman: timeout, pending log, slow query log.
    ''' Return Nothing jika gagal/timeout.
    ''' </summary>
    Public Shared Function SafeExecuteScalar(
        ByVal sql As String,
        ByVal caller As String,
        Optional ByVal timeoutSec As Integer = -1) As Object

        If timeoutSec < 0 Then timeoutSec = DefaultTimeoutSeconds
        Dim token As String = BeginQuery(sql, caller)
        Dim sw As New Diagnostics.Stopwatch()
        Dim result As Object = Nothing

        Try
            Using conn As New MySqlConnection(My.Settings.ServerStr)
                Using cmd As New MySqlCommand(sql, conn)
                    cmd.CommandTimeout = timeoutSec
                    conn.Open()
                    sw.Start()
                    Dim raw As Object = cmd.ExecuteScalar()
                    sw.Stop()
                    result = If(raw Is DBNull.Value OrElse raw Is Nothing, Nothing, raw)
                End Using
            End Using
            CommitQuery(token)
            If sw.ElapsedMilliseconds >= SlowQueryThresholdMs Then
                LogSlowQuery(sql, caller, sw.ElapsedMilliseconds)
            End If
        Catch ex As MySqlException When ex.Number = 0 OrElse ex.Message.ToLower().Contains("timeout")
            sw.Stop()
            MarkTimeout(token, sql, caller, ex)
            errLog("TIMEOUT (" & sw.ElapsedMilliseconds & "ms): " & ex.Message, caller)
        Catch ex As Exception
            sw.Stop()
            MarkTimeout(token, sql, caller, ex)
            errLog(ex.Message, caller)
        End Try

        Return result
    End Function

    ''' <summary>
    ''' Fill DataTable aman: timeout, pending log, slow query log.
    ''' Return DataTable kosong jika gagal/timeout.
    ''' </summary>
    Public Shared Function SafeFillDataTable(
        ByVal sql As String,
        ByVal caller As String,
        Optional ByVal timeoutSec As Integer = -1) As DataTable

        If timeoutSec < 0 Then timeoutSec = DefaultTimeoutSeconds
        Dim dt As New DataTable()
        Dim token As String = BeginQuery(sql, caller)
        Dim sw As New Diagnostics.Stopwatch()

        Try
            Using conn As New MySqlConnection(My.Settings.ServerStr)
                Using cmd As New MySqlCommand(sql, conn)
                    cmd.CommandTimeout = timeoutSec
                    Using da As New MySqlDataAdapter(cmd)
                        sw.Start()
                        da.Fill(dt)
                        sw.Stop()
                    End Using
                End Using
            End Using
            CommitQuery(token)
            If sw.ElapsedMilliseconds >= SlowQueryThresholdMs Then
                LogSlowQuery(sql, caller, sw.ElapsedMilliseconds)
            End If
        Catch ex As MySqlException When ex.Number = 0 OrElse ex.Message.ToLower().Contains("timeout")
            sw.Stop()
            MarkTimeout(token, sql, caller, ex)
            errLog("TIMEOUT (" & sw.ElapsedMilliseconds & "ms): " & ex.Message, caller)
        Catch ex As Exception
            sw.Stop()
            MarkTimeout(token, sql, caller, ex)
            errLog(ex.Message, caller)
        End Try

        Return dt
    End Function
    ' ================================================================

End Class
