param(
    [string]$MySqlBaseDir = 'C:\Program Files\MySQL\MySQL Server 5.7.18',
    [string]$VSToolsPath = 'C:\Program Files (x86)\MSBuild\Microsoft\VisualStudio\v14.0'
)
$ErrorActionPreference = 'Stop'
$testRoot = $PSScriptRoot
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
    & node (Join-Path $testRoot 'ui.test.cjs')
    if ($LASTEXITCODE -ne 0) { throw 'UI tests failed.' }
    $buildArgs = @((Join-Path $repoRoot 'DreamPos-WebReport\DreamPos-WebReport.vbproj'), '/t:Build', '/p:Configuration=Debug', '/p:VisualStudioVersion=14.0', "/p:VSToolsPath=$VSToolsPath", "/p:VbcToolPath=$compilerDir", ("/p:OutDir=$appBuild\"), '/p:PreBuildEvent=', '/p:Disable_CopyWebApplication=true', '/v:minimal', '/nologo')
    & $msbuild @buildArgs
    if ($LASTEXITCODE -ne 0) { throw 'Application build failed.' }
    $testExe = Join-Path $appBuild 'ReceivePosting.Tests.exe'
    & (Join-Path $compilerDir 'vbc.exe') /nologo /target:exe "/out:$testExe" ("/reference:" + (Join-Path $appBuild 'DreamPos-WebReport.dll')) ("/reference:" + (Join-Path $appBuild 'MySql.Data.dll')) /reference:System.Data.dll /reference:System.Configuration.dll /reference:System.Web.dll (Join-Path $testRoot 'Program.vb')
    if ($LASTEXITCODE -ne 0) { throw 'Test compilation failed.' }
    & $testExe $testRoot
    if ($LASTEXITCODE -ne 0) { throw 'Integration tests failed.' }
} finally {
    if (-not $server.HasExited) {
        & $mysqladmin --no-defaults --host=127.0.0.1 --port=33179 --user=root shutdown 2>$null | Out-Null
        if (-not $server.WaitForExit(5000)) { Stop-Process -Id $server.Id }
    }
}
