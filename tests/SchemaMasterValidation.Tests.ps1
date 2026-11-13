# Regression test: inspect helper AST without executing any deployment code.
$ErrorActionPreference = 'Stop'
$tokens = $null
$errors = $null
$scriptPath = Join-Path $PSScriptRoot '..\DreamPos-WebReport\apply_server_update.ps1'
$ast = [System.Management.Automation.Language.Parser]::ParseFile((Resolve-Path $scriptPath), [ref]$tokens, [ref]$errors)
if ($errors.Count) { throw ($errors | Out-String) }
foreach ($functionName in @('Test-SchemaSqlFragment', 'Test-MasterSchemaColumn')) {
    $node = $ast.Find({param($n) $n -is [System.Management.Automation.Language.FunctionDefinitionAst] -and $n.Name -eq $functionName}, $true)
    Invoke-Expression $node.Extent.Text
}
if (Test-MasterSchemaColumn 'x.FULLTEXT' ([pscustomobject]@{type='KEY';definition='KEY NULL DEFAULT NULL'})) { throw 'Index treated as column' }
if (-not (Test-MasterSchemaColumn 'x.FULLTEXT' ([pscustomobject]@{type='varchar(30)';definition='varchar(30) NULL'}))) { throw 'Legitimate FULLTEXT-named column rejected' }
try {
    Test-MasterSchemaColumn 'x.qty' ([pscustomobject]@{type='decimal(10';definition='decimal(10 NULL DEFAULT NULL'})
    throw 'Truncated decimal accepted'
} catch { if ($_.Exception.Message -notlike 'Skema master tidak valid*') { throw } }
if (-not (Test-SchemaSqlFragment "enum('a,b','(') DEFAULT 'a,b'")) { throw 'Quoted comma/parenthesis rejected' }
if (Test-SchemaSqlFragment 'int; DROP TABLE x') { throw 'Multiple statements accepted' }
$master = Get-Content (Join-Path $PSScriptRoot '..\docs\analysis\schema_master.repaired.json') -Raw -Encoding UTF8 | ConvertFrom-Json
$count = 0
foreach ($table in $master.tables.PSObject.Properties) {
    foreach ($col in $table.Value.columns.PSObject.Properties) {
        if (-not (Test-MasterSchemaColumn ($table.Name + '.' + $col.Name) $col.Value)) { throw 'Repaired master contains index pseudocolumn' }
        $count++
    }
}
Write-Host "PASS: index versus real column, truncated type, quoted enum, statement guard; $count columns in $($master.tables.PSObject.Properties.Name.Count) repaired tables validated."
