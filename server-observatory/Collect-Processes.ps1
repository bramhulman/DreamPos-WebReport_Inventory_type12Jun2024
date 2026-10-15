param(
 [string]$Endpoint='http://127.0.0.1:8787/api/ingest',
 [string]$StatePath=(Join-Path $PSScriptRoot '.process-state.json')
)
$ErrorActionPreference='Stop'
. (Join-Path $PSScriptRoot 'Process-Interval.ps1')
$uri=[Uri]$Endpoint
if ($uri.Scheme -ne 'https' -and -not ($uri.Scheme -eq 'http' -and $uri.IsLoopback)) { throw 'Remote ingestion requires HTTPS.' }
if ($env:MONITOR_TOKEN.Length -lt 32) { throw 'Collector MONITOR_TOKEN required.' }
$lock=$null
try {
 $lock=[IO.File]::Open(($StatePath+'.lock'),[IO.FileMode]::OpenOrCreate,[IO.FileAccess]::ReadWrite,[IO.FileShare]::None)
 $previous=$null
 if (Test-Path -LiteralPath $StatePath) { $previous=Get-Content -LiteralPath $StatePath -Raw | ConvertFrom-Json }
 $now=[DateTimeOffset]::UtcNow.ToUnixTimeMilliseconds()
 $deadline=[DateTime]::UtcNow.AddSeconds(20)
 $cores=[Environment]::ProcessorCount
 $states=@{}
 $samples=@()
 # Only relevant process names, no executable paths, command lines, SQL or connection strings.
 $processes=@(Get-Process -Name w3wp,mysqld,sales_summary -ErrorAction SilentlyContinue)
 if ($processes.Count -gt 128) { throw 'Process inventory limit exceeded; narrow monitoring scope before continuing.' }
 foreach ($process in $processes) {
  try {
   $started=$process.StartTime.ToUniversalTime().ToString('o')
   $seconds=$process.TotalProcessorTime.TotalSeconds
   $key=[string]$process.Id
   $old=$null
   if ($previous -and $previous.hostName -eq $env:COMPUTERNAME -and $previous.logicalProcessors -eq $cores) { $old=$previous.processes.$key }
   $pct=Get-ProcessCpuPercent $seconds $old $now $started $cores
   $row=@{id=('process-'+$env:COMPUTERNAME+'-'+$key);name=($process.ProcessName+' PID '+$key);server=$env:COMPUTERNAME;kind='process';memoryMB=[math]::Round($process.WorkingSet64/1MB,2);logicalProcessors=$cores}
   if ($null -ne $pct) { $row.cpu=$pct }
   $states[$key]=@{at=$now;cpuSeconds=$seconds;started=$started}
   $samples+=[pscustomobject]$row
  } catch { Write-Warning 'One process exited or could not be read; no zero CPU was substituted.' }
 }
 # Top 24 sends bound API work; all readable process counters retained for next CPU interval.
 $sent=0
 foreach ($sample in ($samples | Sort-Object @{Expression='cpu';Descending=$true},@{Expression='memoryMB';Descending=$true} | Select-Object -First 24)) {
  if ([DateTime]::UtcNow -ge $deadline) { Write-Warning 'Process collector budget reached; remaining snapshots skipped.'; break }
  Invoke-RestMethod -Uri $uri -Method Post -ContentType 'application/json' -Headers @{Authorization=('Bearer '+$env:MONITOR_TOKEN)} -Body ($sample | ConvertTo-Json -Compress) -TimeoutSec 3 | Out-Null
  $sent++
 }
 $next=@{hostName=$env:COMPUTERNAME;logicalProcessors=$cores;processes=$states} | ConvertTo-Json -Depth 5
 [IO.File]::WriteAllText(($StatePath+'.tmp'),$next)
 Move-Item -LiteralPath ($StatePath+'.tmp') -Destination $StatePath -Force
 Write-Output "Process snapshots accepted: $sent. Scope: w3wp, mysqld, sales_summary. First CPU sample needs a baseline."
} finally { if ($lock) { $lock.Dispose() } }
