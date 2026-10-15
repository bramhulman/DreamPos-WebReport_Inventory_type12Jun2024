function Get-ProcessCpuPercent {
 param([double]$CpuSeconds,$Previous,[double]$Now,[string]$Started,[int]$LogicalProcessors)
 if (-not $Previous -or $LogicalProcessors -lt 1 -or $Previous.started -ne $Started) { return $null }
 $seconds=($Now-[double]$Previous.at)/1000
 $delta=$CpuSeconds-[double]$Previous.cpuSeconds
 if ($seconds -lt 5 -or $seconds -gt 120 -or $delta -lt 0) { return $null }
 # Normalize against all logical processors: one fully busy thread on 2 vCPU = 50%.
 $percent=100*$delta/($seconds*$LogicalProcessors)
 if ($percent -gt 101) { return $null }
 return [math]::Round([math]::Min(100,$percent),2)
}
