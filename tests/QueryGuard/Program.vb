Imports System.IO
Imports System.Linq
Imports System.Threading.Tasks
Imports System.Diagnostics

Namespace My
    Public Module TestSettings
        Public ReadOnly Settings As New GuardSettings()
    End Module
    Public Class GuardSettings
        Public Property errLog As String
        Public Property ServerStr As String = ""
    End Class
End Namespace

Module Program
    Public Sub errLog(ByVal message As String, ByVal caller As String)
        Throw New InvalidOperationException(caller & ": " & message)
    End Sub

    Private Sub Check(ByVal condition As Boolean, ByVal message As String)
        If Not condition Then Throw New Exception(message)
    End Sub

    Public Sub Main(ByVal args As String())
        If args.Length > 0 Then
            My.Settings.errLog = Path.Combine(args(0), "errLog.txt")
            For i As Integer = 1 To 100
                Dim token = clsQueryGuard.BeginQuery("SELECT " & i, "worker")
                clsQueryGuard.CommitQuery(token)
            Next
            Return
        End If

        Dim testDirectory = Path.Combine(Path.GetTempPath(), "DreamPosQueryGuardTests-" & Guid.NewGuid().ToString("N"))
        Directory.CreateDirectory(testDirectory)
        Try
            My.Settings.errLog = Path.Combine(testDirectory, "errLog.txt")
            Dim pending = clsQueryGuard.PendingLogPath
            File.WriteAllLines(pending, New String() {
                "legacy-token|2026-10-13 00:00:00|legacy|SELECT 1",
                "legacy-token-extra|2026-10-13 00:00:00|legacy|SELECT 2",
                "malformed line retained for investigation"})

            Check(clsQueryGuard.BeginQuery("", "empty") = "", "Blank SQL must not create an entry.")
            clsQueryGuard.CommitQuery("")
            clsQueryGuard.CommitQuery("legacy-token")
            For i As Integer = 1 To 98
                Dim token = clsQueryGuard.BeginQuery("SELECT " & i, "batch")
                clsQueryGuard.CommitQuery(token)
            Next
            Check(File.ReadAllText(pending).Contains("legacy-token|"), "Log was rewritten before the batch threshold.")
            Check(File.ReadAllLines(pending).Count(Function(line) line.StartsWith("[COMPLETED]|")) = 99,
                  "Successful queries must have durable completion markers.")

            Dim failedToken = clsQueryGuard.BeginQuery("SELECT failure", "failed")
            clsQueryGuard.MarkTimeout(failedToken, "SELECT failure", "failed", New Exception("Connection failed"))
            Dim lastToken = clsQueryGuard.BeginQuery("SELECT final", "batch")
            clsQueryGuard.CommitQuery(lastToken)
            Dim compacted = File.ReadAllLines(pending)
            Check(compacted.Length = 3, "Compaction must retain only unresolved and malformed entries.")
            Check(compacted.Any(Function(line) line.StartsWith("legacy-token-extra|")), "Token prefix collision removed an unrelated entry.")
            Check(compacted.Any(Function(line) line.StartsWith(failedToken & "|")), "Failed query was lost.")
            Check(Not compacted.Any(Function(line) line.StartsWith("[COMPLETED]|")), "Completion markers were not compacted.")

            Parallel.For(0, 200, Sub(i)
                                     Dim token = clsQueryGuard.BeginQuery("SELECT " & i, "concurrent")
                                     clsQueryGuard.CommitQuery(token)
                                 End Sub)
            Check(File.ReadAllLines(pending).Length = 3, "Concurrent queries lost entries or left completed rows.")

            ' Exercise two independent processes sharing the same log file.
            Dim executable = Process.GetCurrentProcess().MainModule.FileName
            Dim workers As New List(Of Process)
            For i As Integer = 1 To 2
                Dim info As New ProcessStartInfo(executable, """" & testDirectory & """")
                info.UseShellExecute = False
                info.CreateNoWindow = True
                workers.Add(Process.Start(info))
            Next
            For Each worker As Process In workers
                Check(worker.WaitForExit(30000), "Worker did not finish.")
                Check(worker.ExitCode = 0, "Worker failed.")
                worker.Dispose()
            Next
            Check(File.ReadAllLines(pending).Length = 3, "Cross-process logging lost unresolved rows or completed queries.")

            ' A completion marker left by a previous process must survive until cleanup.
            File.AppendAllText(pending, "restart-token|2026-10-13 00:00:00|legacy|SELECT 3" & Environment.NewLine &
                               "[COMPLETED]|restart-token" & Environment.NewLine)
            For i As Integer = 1 To 100
                Dim token = clsQueryGuard.BeginQuery("SELECT restart", "restart")
                clsQueryGuard.CommitQuery(token)
            Next
            Check(File.ReadAllLines(pending).Length = 3, "Restart completion marker was ignored.")

            Dim intervalToken = clsQueryGuard.BeginQuery("SELECT interval", "interval")
            clsQueryGuard.CommitQuery(intervalToken)
            GetType(clsQueryGuard).GetField("LastCompactionUtc", Reflection.BindingFlags.NonPublic Or Reflection.BindingFlags.Static).
                SetValue(Nothing, DateTime.UtcNow.AddSeconds(-61))
            Dim activeToken = clsQueryGuard.BeginQuery("SELECT active", "interval")
            Dim intervalLines = File.ReadAllLines(pending)
            Check(Not intervalLines.Any(Function(line) line.StartsWith(intervalToken & "|")), "Time-based compaction did not run.")
            Check(intervalLines.Any(Function(line) line.StartsWith(activeToken & "|")), "Time-based compaction lost an active query.")

            clsQueryGuard.LogSlowQuery("SELECT slow", "slow", 3500)
            Dim slowLines = File.ReadAllLines(clsQueryGuard.SlowLogPath)
            Check(slowLines.Any(Function(line) line.StartsWith("[SLOW 3500ms]")), "Slow query was not classified correctly.")
            Check(slowLines.Any(Function(line) line.StartsWith("[TIMEOUT/CRASH]") AndAlso line.Contains(failedToken)),
                  "Failed query diagnostic was lost.")
            Check(Directory.GetFiles(testDirectory, "*.tmp").Length = 0, "Compaction left temporary files.")
            Console.WriteLine("PASS: batching, failure retention, exact tokens, concurrent threads/processes, restart markers, timed cleanup, slow/error classification.")
        Finally
            Directory.Delete(testDirectory, True)
        End Try
    End Sub
End Module
