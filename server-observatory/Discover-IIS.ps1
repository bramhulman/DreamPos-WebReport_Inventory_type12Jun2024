$ErrorActionPreference='Stop'
Import-Module WebAdministration -ErrorAction Stop
# Console inventory only. No web.config, connection strings, credentials or changes.
Get-Website | ForEach-Object {
 [pscustomobject]@{
  SiteId=$_.Id
  SiteName=$_.Name
  State=[string]$_.State
  ApplicationPool=$_.ApplicationPool
  LogDirectory=$_.logFile.directory
 }
} | ConvertTo-Json -Depth 3
