$ErrorActionPreference='Stop'
. (Join-Path $PSScriptRoot 'MySQL-Interval.ps1')
$previous=[pscustomobject]@{at=100000;hostName='TEST';loginPath='monitor';values=[pscustomobject]@{Uptime=100;Uptime_since_flush_status=100;Queries=1000;Aborted_connects=22;Aborted_clients=3}}
$values=@{Uptime=160;Uptime_since_flush_status=160;Queries=1120;Aborted_connects=22;Aborted_clients=4}
function Check($condition,$name) { if (-not $condition) { throw ('FAIL: '+$name) } }
$result=Get-MySQLInterval $values $previous 160000 TEST monitor
Check ($result.rps -eq 2) 'QPS delta'
Check ($result.abortedConnectsDelta -eq 0) 'historical aborted 22 is not an interval failure'
Check ($result.abortedClientsDelta -eq 1) 'aborted client delta'
Check ($null -eq (Get-MySQLInterval $values $null 160000 TEST monitor)) 'first sample'
Check ($null -eq (Get-MySQLInterval $values $previous 400000 TEST monitor)) 'stale interval'
Check ($null -eq (Get-MySQLInterval $values $previous 160000 OTHER monitor)) 'different server'
$values.Uptime=110
Check ($null -eq (Get-MySQLInterval $values $previous 160000 TEST monitor)) 'restart even when new uptime exceeds old uptime'
$values.Uptime=160; $values.Uptime_since_flush_status=10
Check ($null -eq (Get-MySQLInterval $values $previous 160000 TEST monitor)) 'flush reset'
$values.Uptime_since_flush_status=160; $values.Queries=2
Check ($null -eq (Get-MySQLInterval $values $previous 160000 TEST monitor)) 'counter rollback'
'MySQL interval checks passed.'
