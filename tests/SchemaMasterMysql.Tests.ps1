param(
    [string]$MySqlBaseDir = 'C:\Program Files\MySQL\MySQL Server 5.7.18',
    [string]$VSToolsPath = 'C:\Program Files (x86)\MSBuild\Microsoft\VisualStudio\v14.0'
)
$ErrorActionPreference = 'Stop'
$testRoot = Join-Path $PSScriptRoot 'ReceivePosting'
$repoRoot = (Resolve-Path (Join-Path $testRoot '..\..')).Path
$runtime = Join-Path $testRoot 'runtime'
$dataDir = Join-Path $runtime 'mysql-data'
$appBuild = Join-Path $runtime 'app-build'
$msbuild = Join-Path $env:WINDIR 'Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe'
$compilerDir = Join-Path $repoRoot 'packages\Microsoft.Net.Compilers.1.0.0\tools'
$mysqld = Join-Path $MySqlBaseDir 'bin\mysqld.exe'
$mysqladmin = Join-Path $MySqlBaseDir 'bin\mysqladmin.exe'
if (Get-NetTCPConnection -LocalPort 33179 -State Listen -ErrorAction SilentlyContinue) { throw 'Port 33179 is occupied. No existing server will be reused.' }
New-Item -ItemType Directory -Path $runtime -Force | Out-Null
if (-not (Test-Path $dataDir)) {
    & $mysqld --no-defaults --initialize-insecure "--basedir=$MySqlBaseDir" "--datadir=$dataDir" --console
    if ($LASTEXITCODE -ne 0) { throw 'Test MySQL initialization failed.' }
}
# Test DB is limited to this datadir and loopback. Program.vb checks @@datadir before any DDL.
$serverArgs = @('--no-defaults', '--console', '--bind-address=127.0.0.1', '--port=33179', '--server-id=33179', '--skip-log-bin', '--explicit_defaults_for_timestamp=1', '--innodb-buffer-pool-size=32M', ('--basedir="' + $MySqlBaseDir + '"'), ('--datadir="' + $dataDir + '"'), ('--pid-file="' + (Join-Path $runtime 'mysql-test.pid') + '"'))
$server = Start-Process -FilePath $mysqld -ArgumentList $serverArgs -WindowStyle Hidden -PassThru -RedirectStandardOutput (Join-Path $runtime 'mysql-stdout.log') -RedirectStandardError (Join-Path $runtime 'mysql-stderr.log')
try {
    $ready = $false
    for ($attempt = 0; $attempt -lt 40; $attempt++) {
        if ($server.HasExited) { throw 'Isolated MySQL exited. See runtime/mysql-stderr.log.' }
        & $mysqladmin --no-defaults --host=127.0.0.1 --port=33179 --user=root ping 2>$null | Out-Null
        if ($LASTEXITCODE -eq 0) { $ready = $true; break }
        Start-Sleep -Milliseconds 250
    }
    if (-not $ready) { throw 'Isolated MySQL did not become ready.' }
    [System.Reflection.Assembly]::LoadFrom((Join-Path $repoRoot 'DreamPos-WebReport\bin\MySql.Data.dll')) | Out-Null
    $conn = New-Object MySql.Data.MySqlClient.MySqlConnection('server=127.0.0.1;port=33179;uid=root;password=;SslMode=None;Pooling=False')
    $conn.Open()
    try {
        $cmd = $conn.CreateCommand()
        $cmd.CommandText = 'SELECT @@datadir'
        $actualDataDir = [IO.Path]::GetFullPath([string]$cmd.ExecuteScalar())
        if (-not $actualDataDir.StartsWith($dataDir, [StringComparison]::OrdinalIgnoreCase)) { throw 'Refusing non-test MySQL instance' }
        $dbName = 'codex_schema_' + [Guid]::NewGuid().ToString('N')
        $cmd.CommandText = 'CREATE DATABASE ' + $dbName
        $cmd.ExecuteNonQuery() | Out-Null
        $conn.ChangeDatabase($dbName)
        $schema = Get-Content (Join-Path $repoRoot 'docs\analysis\schema_master.repaired.json') -Raw -Encoding UTF8 | ConvertFrom-Json
        $count = 0
        foreach ($table in $schema.tables.PSObject.Properties) {
            $defs = @()
            foreach ($col in $table.Value.columns.PSObject.Properties) {
                $defs += ('`' + $col.Name.Replace('`','``') + '` ' + $col.Value.definition)
            }
            if ($table.Value.primary_key.Count -gt 0) {
                $pk = @($table.Value.primary_key | ForEach-Object { '`' + $_.Replace('`','``') + '`' }) -join ', '
                $defs += "PRIMARY KEY ($pk)"
            }
            $cmd.CommandText = 'CREATE TABLE `' + $table.Name.Replace('`','``') + '` (' + ($defs -join ', ') + ') ENGINE=InnoDB DEFAULT CHARSET=utf8;'
            try { $cmd.ExecuteNonQuery() | Out-Null } catch { throw "Table $($table.Name): $($_.Exception.Message); SQL=$($cmd.CommandText)" }
            $count++
        }
        Write-Host "PASS: all $count repaired CREATE TABLE statements executed successfully on isolated MySQL 5.7."
    } finally { $conn.Dispose() }
} finally {
    if (-not $server.HasExited) {
        & $mysqladmin --no-defaults --host=127.0.0.1 --port=33179 --user=root shutdown 2>$null | Out-Null
        if (-not $server.WaitForExit(5000)) { Stop-Process -Id $server.Id }
    }
}
