param(
 [string]$MySQLCliPath='C:\Program Files\MySQL\MySQL Server 8.0\bin\mysql.exe',
 [string]$LoginPath='observatory',
 [string]$Endpoint='http://127.0.0.1:8787/api/ingest',
 [string]$StatePath=(Join-Path $PSScriptRoot '.mysql-state.json')
)
$ErrorActionPreference='Stop'
. (Join-Path $PSScriptRoot 'MySQL-Interval.ps1')
if ($LoginPath -notmatch '^[a-zA-Z0-9_-]{1,64}$') { throw 'Invalid login path.' }
$uri=[Uri]$Endpoint
if ($uri.Scheme -ne 'https' -and -not ($uri.Scheme -eq 'http' -and $uri.IsLoopback)) { throw 'Remote ingestion requires HTTPS.' }
if ($env:MONITOR_TOKEN.Length -lt 32) { throw 'Set collector MONITOR_TOKEN.' }
# ACL the state directory to the collector account and administrators before scheduling.
$lock=$null
$process=$null
$started=$false
try {
 $lock=[IO.File]::Open(($StatePath+'.lock'),[IO.FileMode]::OpenOrCreate,[IO.FileAccess]::ReadWrite,[IO.FileShare]::None)
 $info=New-Object Diagnostics.ProcessStartInfo
 $info.FileName=$MySQLCliPath
 $info.Arguments="--login-path=$LoginPath --connect-timeout=3 --batch --raw --skip-column-names --default-character-set=utf8mb4"
 $info.UseShellExecute=$false
 $info.CreateNoWindow=$true
 $info.RedirectStandardInput=$true
 $info.RedirectStandardOutput=$true
 $info.RedirectStandardError=$true
 $process=New-Object Diagnostics.Process
 $process.StartInfo=$info
 $started=$process.Start()
 $outputTask=$process.StandardOutput.ReadToEndAsync()
 $errorTask=$process.StandardError.ReadToEndAsync()
 # Select only status counters; no application tables, SQL text or connection strings.
 $process.StandardInput.WriteLine("SELECT VARIABLE_NAME, VARIABLE_VALUE FROM performance_schema.global_status WHERE VARIABLE_NAME IN ('Uptime','Uptime_since_flush_status','Queries','Threads_connected','Threads_running','Aborted_connects','Aborted_clients');")
 $process.StandardInput.Close()
 if (-not $process.WaitForExit(8000)) { $process.Kill(); throw 'MySQL collection timeout; no service action performed.' }
 if ($process.ExitCode -ne 0) { throw 'MySQL collection failed; verify login-path, account grants and Performance Schema. Raw errors suppressed.' }
 $values=@{}
 foreach ($line in ($outputTask.Result -split "`n")) {
  $fields=$line.Trim() -split "`t"
  if ($fields.Count -eq 2 -and $fields[1] -match '^\d+$') { $values[$fields[0]]=[double]$fields[1] }
 }
 foreach ($key in @('Uptime','Uptime_since_flush_status','Queries','Threads_connected','Threads_running','Aborted_connects','Aborted_clients')) {
  if (-not $values.ContainsKey($key)) { throw 'Incomplete status snapshot; no metrics sent.' }
 }
 $now=[DateTimeOffset]::UtcNow.ToUnixTimeMilliseconds()
 $previous=$null
 if (Test-Path -LiteralPath $StatePath) { $previous=Get-Content -LiteralPath $StatePath -Raw | ConvertFrom-Json }
 $payload=@{id=('mysql-'+$env:COMPUTERNAME);name='MySQL instance';server=$env:COMPUTERNAME;kind='database';connections=$values.Threads_connected;threadsRunning=$values.Threads_running;uptimeSeconds=$values.Uptime}
 $interval=Get-MySQLInterval -Values $values -Previous $previous -Now $now -HostName $env:COMPUTERNAME -LoginPath $LoginPath
 if ($interval) { foreach ($key in $interval.Keys) { $payload[$key]=$interval[$key] } }
 elseif ($previous) { Write-Warning 'Counter reset or invalid interval: rate omitted, baseline renewed.' }
 Invoke-RestMethod -Method Post -Uri $uri -ContentType 'application/json' -Headers @{Authorization=('Bearer '+$env:MONITOR_TOKEN)} -Body ($payload | ConvertTo-Json -Compress) -TimeoutSec 10 | Out-Null
 $next=@{at=$now;hostName=$env:COMPUTERNAME;loginPath=$LoginPath;values=$values} | ConvertTo-Json -Depth 4
 [IO.File]::WriteAllText(($StatePath+'.tmp'),$next)
 Move-Item -LiteralPath ($StatePath+'.tmp') -Destination $StatePath -Force
 Write-Output 'MySQL aggregate snapshot accepted. No restart, kill or database writes performed.'
} finally {
 if ($process) { if ($started -and -not $process.HasExited) { $process.Kill() }; $process.Dispose() }
 if ($lock) { $lock.Dispose() }
}
