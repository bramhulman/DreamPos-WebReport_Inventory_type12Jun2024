function Get-MySQLInterval {
 param($Values, $Previous, [double]$Now, [string]$HostName, [string]$LoginPath)
 if (-not $Previous) { return $null }
 $seconds=($Now-[double]$Previous.at)/1000
 if ($seconds -lt 5 -or $seconds -gt 120 -or $Previous.hostName -ne $HostName -or $Previous.loginPath -ne $LoginPath) { return $null }
 foreach ($key in @('Uptime','Uptime_since_flush_status','Queries','Aborted_connects','Aborted_clients')) {
  if (-not ($Previous.values.PSObject.Properties.Name -contains $key)) { return $null }
 }
 foreach ($key in @('Uptime','Uptime_since_flush_status')) {
  $advance=$Values[$key]-[double]$Previous.values.$key
  if ([math]::Abs($advance-$seconds) -gt 5) { return $null }
 }
 foreach ($key in @('Queries','Aborted_connects','Aborted_clients')) { if ($Values[$key] -lt [double]$Previous.values.$key) { return $null } }
 return @{
  intervalSeconds=[math]::Round($seconds,2)
  rps=[math]::Round(($Values.Queries-[double]$Previous.values.Queries)/$seconds,3)
  abortedConnectsDelta=$Values.Aborted_connects-[double]$Previous.values.Aborted_connects
  abortedClientsDelta=$Values.Aborted_clients-[double]$Previous.values.Aborted_clients
 }
}
