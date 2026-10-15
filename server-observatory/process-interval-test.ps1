$ErrorActionPreference='Stop'
. (Join-Path $PSScriptRoot 'Process-Interval.ps1')
function Check($condition,$name) { if (-not $condition) { throw ('FAIL: '+$name) } }
$old=[pscustomobject]@{at=100000;cpuSeconds=10;started='A'}
Check ((Get-ProcessCpuPercent 70 $old 160000 A 2) -eq 50) 'one busy thread on two vCPUs'
Check ((Get-ProcessCpuPercent 130 $old 160000 A 2) -eq 100) 'both busy vCPUs'
Check ((Get-ProcessCpuPercent 10 $old 160000 A 2) -eq 0) 'idle'
Check ($null -eq (Get-ProcessCpuPercent 70 $null 160000 A 2)) 'first snapshot'
Check ($null -eq (Get-ProcessCpuPercent 70 $old 160000 B 2)) 'PID reuse'
Check ($null -eq (Get-ProcessCpuPercent 1 $old 160000 A 2)) 'counter reset'
Check ($null -eq (Get-ProcessCpuPercent 70 $old 500000 A 2)) 'stale interval'
Check ($null -eq (Get-ProcessCpuPercent 70 $old 160000 A 0)) 'invalid CPU count'
'Process CPU normalization checks passed.'
