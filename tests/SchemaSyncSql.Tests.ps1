# Exercise SQL generation from the updater without running deployment or connecting to MySQL.
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version 2.0
$scriptPath = Join-Path $PSScriptRoot '..\DreamPos-WebReport\apply_server_update.ps1'
$tokens = $null
$parseErrors = $null
$ast = [System.Management.Automation.Language.Parser]::ParseFile((Resolve-Path $scriptPath), [ref]$tokens, [ref]$parseErrors)
if ($parseErrors.Count -gt 0) { throw ($parseErrors | Out-String) }
$helper = $ast.Find({ param($node) $node -is [System.Management.Automation.Language.FunctionDefinitionAst] -and $node.Name -eq 'Quote-MySqlIdentifier' }, $true)
Invoke-Expression $helper.Extent.Text
function Invoke-UpdaterAssignment([string]$Name) {
    $assignment = $ast.Find({ param($node) $node -is [System.Management.Automation.Language.AssignmentStatementAst] -and $node.Left.Extent.Text -eq ('$' + $Name) }, $true)
    if ($null -eq $assignment) { throw "Assignment $Name not found" }
    Invoke-Expression $assignment.Right.Extent.Text
}
function Assert-Equal($actual, $expected) {
    if ($actual -cne $expected) { throw "Expected: $expected`nActual: $actual" }
}
Assert-Equal (Quote-MySqlIdentifier 'order') '`order`'
Assert-Equal (Quote-MySqlIdentifier 'odd`name') '`odd``name`'
$schema = Get-Content (Join-Path $PSScriptRoot '..\DreamPos-WebReport\Pathdata\schema_master.json') -Raw -Encoding UTF8 | ConvertFrom-Json
$tName = 'cld_inv_ordercust'
$tData = $schema.tables.$tName
$colDefs = New-Object System.Collections.Generic.List[string]
$columnStatement = $ast.Find({ param($node) $node -is [System.Management.Automation.Language.InvokeMemberExpressionAst] -and $node.Extent.Text -like '$colDefs.Add(*Quote-MySqlIdentifier*' }, $true)
foreach ($cProp in $tData.columns.PSObject.Properties) {
    Invoke-Expression $columnStatement.Extent.Text
}
$pkNames = Invoke-UpdaterAssignment 'pkNames'
$colDefs.Add("PRIMARY KEY ($pkNames)")
$createSql = Invoke-UpdaterAssignment 'createSql'
if (-not $createSql.StartsWith('CREATE TABLE IF NOT EXISTS `cld_inv_ordercust` (`ID` ')) { throw $createSql }
if ($createSql.Contains('"')) { throw 'Double-quoted identifier in CREATE TABLE' }
if (-not $createSql.Contains('PRIMARY KEY (`ID`, `CustID`)')) { throw $createSql }
$tName = 'order'
$cName = 'odd`column'
$cProp = [pscustomobject]@{ Value = [pscustomobject]@{ definition = "varchar(50) DEFAULT 'a value'" } }
Assert-Equal (Invoke-UpdaterAssignment 'alterSql') 'ALTER TABLE `order` ADD COLUMN `odd``column` varchar(50) DEFAULT ''a value'';'
Assert-Equal (Invoke-UpdaterAssignment 'modifySql') 'ALTER TABLE `order` MODIFY COLUMN `odd``column` varchar(50) DEFAULT ''a value'';'
$historySql = Invoke-UpdaterAssignment 'createHistSql'
if (-not $historySql.Contains('`migration_id`') -or -not $historySql.Contains('`executed_at`')) { throw 'Migration history identifiers were altered by PowerShell escaping' }
Write-Host 'PASS: parser, CREATE from master schema, composite primary key, ADD/MODIFY COLUMN, escaped identifiers, migration history.'
