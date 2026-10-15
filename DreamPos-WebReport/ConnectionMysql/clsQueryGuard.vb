Imports MySql.Data.MySqlClient
Imports System.IO
Imports System.Security.Cryptography
Imports System.Text
Imports System.Threading

''' <summary>
''' clsQueryGuard — Query Safety Wrapper
''' Fitur:
'''   1. Timeout otomatis dengan nilai default yang bisa dikonfigurasi
'''   2. Pending Query Log: simpan query SEBELUM dieksekusi
'''      - Jika SUKSES -> ditandai selesai, lalu dihapus secara batch
'''      - Jika CRASH/TIMEOUT -> entry tetap di pending log untuk analisa
'''   3. Durasi eksekusi dicatat ke slow_queries.log jika melebihi threshold
''' </summary>
Public Class clsQueryGuard
    Private Const CompletionPrefix As String = "[COMPLETED]|"
    Private Const CompactionBatchSize As Integer = 100
    Private Const CompactionIntervalSeconds As Integer = 60
    Private Shared ReadOnly LogSync As New Object()
    Private Shared CompletedSinceCompaction As Integer = 0
    Private Shared LastCompactionUtc As DateTime = DateTime.UtcNow

    Private Shared Sub WithLogLock(ByVal writeLog As Action)
        SyncLock LogSync
            Dim mutexName As String
            Using hash As SHA256 = SHA256.Create()
                mutexName = "Local\DreamPosQueryGuard_" & BitConverter.ToString(
                    hash.ComputeHash(Encoding.UTF8.GetBytes(Path.GetFullPath(PendingLogPath).ToUpperInvariant()))).Replace("-", "")
            End Using
            Using logMutex As New Mutex(False, mutexName)
                Dim acquired As Boolean = False
                Try
                    Try
                        acquired = logMutex.WaitOne(1000)
                    Catch ex As AbandonedMutexException
                        acquired = True
                    End Try
                    If acquired Then writeLog()
                Finally
                    If acquired Then logMutex.ReleaseMutex()
                End Try
            End Using
        End SyncLock
    End Sub

    Private Shared Sub CompactPendingLogIfDue()
        If CompletedSinceCompaction < CompactionBatchSize AndAlso
           (DateTime.UtcNow - LastCompactionUtc).TotalSeconds < CompactionIntervalSeconds Then Return

        If File.Exists(PendingLogPath) Then
            Dim lines As String() = File.ReadAllLines(PendingLogPath)
            Dim completed As New HashSet(Of String)(StringComparer.Ordinal)
            For Each line As String In lines
                If line.StartsWith(CompletionPrefix, StringComparison.Ordinal) Then
                    completed.Add(line.Substring(CompletionPrefix.Length))
                End If
            Next
            If completed.Count > 0 Then
                Dim remaining As New List(Of String)
                For Each line As String In lines
                    If line.StartsWith(CompletionPrefix, StringComparison.Ordinal) Then Continue For
                    Dim separator As Integer = line.IndexOf("|"c)
                    If separator < 0 OrElse Not completed.Contains(line.Substring(0, separator)) Then remaining.Add(line)
                Next
                ' Replace atomically: a crash must not truncate the existing journal.
                Dim tempPath As String = PendingLogPath & "." & Guid.NewGuid().ToString("N") & ".tmp"
                Try
                    File.WriteAllLines(tempPath, remaining.ToArray())
                    File.Replace(tempPath, PendingLogPath, Nothing)
                Finally
                    If File.Exists(tempPath) Then File.Delete(tempPath)
                End Try
            End If
        End If
        CompletedSinceCompaction = 0
        LastCompactionUtc = DateTime.UtcNow
    End Sub

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
        Dim token As String = Guid.NewGuid().ToString("N")
        Dim entry As String = String.Format("{0}|{1}|{2}|{3}",
            token,
            Format(Now, "yyyy-MM-dd HH:mm:ss"),
            caller,
            sql.Replace(vbCrLf, " ").Replace(vbLf, " ").Trim())
        Try
            WithLogLock(Sub()
                            File.AppendAllText(PendingLogPath, entry & Environment.NewLine)
                            CompactPendingLogIfDue()
                        End Sub)
        Catch
        End Try
        Return token
    End Function

    ''' <summary>
    ''' Tandai query sukses tanpa menulis ulang seluruh file pada setiap query.
    ''' </summary>
    Public Shared Sub CommitQuery(ByVal token As String)
        If String.IsNullOrWhiteSpace(token) Then Exit Sub
        Try
            WithLogLock(Sub()
                            File.AppendAllText(PendingLogPath, CompletionPrefix & token & Environment.NewLine)
                            CompletedSinceCompaction += 1
                            CompactPendingLogIfDue()
                        End Sub)
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
            WithLogLock(Sub() File.AppendAllText(SlowLogPath, entry & Environment.NewLine))
        Catch
        End Try
    End Sub

    Public Shared Sub LogSlowQuery(ByVal sql As String, ByVal caller As String, ByVal elapsedMs As Long)
        Try
            Dim entry As String = String.Format("[SLOW {0}ms] {1} | Caller:{2} | SQL:{3}",
                elapsedMs, Format(Now, "yyyy-MM-dd HH:mm:ss"), caller,
                sql.Replace(vbCrLf, " ").Replace(vbLf, " ").Trim())
            WithLogLock(Sub() File.AppendAllText(SlowLogPath, entry & Environment.NewLine))
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
