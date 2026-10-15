param([string]$Endpoint='http://127.0.0.1:8787/api/ingest')
$ErrorActionPreference='Stop'
# One snapshot only: schedule externally every 60 seconds after a pilot review.
# No website requests, SQL queries, raw logs, process command lines or secrets collected.
$uri=[Uri]$Endpoint
if ($uri.Scheme -ne 'https' -and -not ($uri.Scheme -eq 'http' -and $uri.IsLoopback)) { throw 'Remote ingestion requires HTTPS.' }
if ([string]::IsNullOrWhiteSpace($env:MONITOR_TOKEN) -or $env:MONITOR_TOKEN.Length -lt 32) { throw 'MONITOR_TOKEN must contain at least 32 characters.' }
$os=Get-CimInstance Win32_OperatingSystem -OperationTimeoutSec 10
$cpu=Get-CimInstance Win32_Processor -OperationTimeoutSec 10 | Measure-Object LoadPercentage -Average
$disks=@(Get-CimInstance Win32_LogicalDisk -Filter 'DriveType=3' -OperationTimeoutSec 10 | Where-Object { $_.Size -gt 0 })
$payload=@{id=('server-'+$env:COMPUTERNAME);name=$env:COMPUTERNAME;server=$env:COMPUTERNAME;kind='server';logicalProcessors=[Environment]::ProcessorCount;memory=[math]::Round((1-$os.FreePhysicalMemory/$os.TotalVisibleMemorySize)*100,2)}
if ($null -ne $cpu.Average) { $payload.cpu=[math]::Round($cpu.Average,2) }
if ($disks.Count -gt 0) { $payload.diskFreeGB=[math]::Round((($disks | Measure-Object -Property FreeSpace -Minimum).Minimum)/1GB,2); $payload.disk=($disks | ForEach-Object { [math]::Round((1-$_.FreeSpace/$_.Size)*100,2) } | Measure-Object -Maximum).Maximum }
try {
 Invoke-RestMethod -Method Post -Uri $uri -ContentType 'application/json' -Headers @{Authorization=('Bearer '+$env:MONITOR_TOKEN)} -Body ($payload | ConvertTo-Json -Compress) -TimeoutSec 10 | Out-Null
 Write-Output 'Snapshot accepted.'
} catch { throw 'Snapshot delivery failed. Check endpoint, authorization, and collector health; no automatic retries performed.' }
