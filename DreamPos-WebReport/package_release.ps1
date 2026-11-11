Param(
    [string]$projectDir = $PSScriptRoot,
    [string]$outputDir  = ""
)

$ErrorActionPreference = 'Stop'
Set-StrictMode -Version 2.0

if ([string]::IsNullOrWhiteSpace($projectDir)) {
    $projectDir = (Get-Location).Path
}
$projectDir = (Resolve-Path -LiteralPath $projectDir).Path

if ([string]::IsNullOrWhiteSpace($outputDir)) {
    $repoRoot = (Get-Item $projectDir).Parent.FullName
    $outputDir = Join-Path $repoRoot "Release_Packages"
}
$outputDir = [IO.Path]::GetFullPath($outputDir)

if ($outputDir.TrimEnd('\') -eq $projectDir.TrimEnd('\') -or
    $outputDir.StartsWith($projectDir.TrimEnd('\') + '\', [StringComparison]::OrdinalIgnoreCase)) {
    throw 'Folder output harus berada di luar folder project agar tidak ikut tersalin.'
}

Write-Host "==========================================================" -ForegroundColor Cyan
Write-Host "   DREAMPOS WEBREPORT - 1-KLIK PACKAGING RELEASE SCRIPT   " -ForegroundColor Cyan
Write-Host "==========================================================" -ForegroundColor Cyan
Write-Host "Project Source : $projectDir" -ForegroundColor White
Write-Host "Output Target  : $outputDir" -ForegroundColor White

# 1. Pastikan folder output ada
if (-not (Test-Path $outputDir)) {
    New-Item -ItemType Directory -Path $outputDir -Force | Out-Null
}

# 1b. Hapus legacy bin\ErrLog.txt di folder source jika ada
$legacyBinLog = Join-Path $projectDir "bin\ErrLog.txt"
if (Test-Path -LiteralPath $legacyBinLog) {
    Write-Host "-> Menghapus legacy bin\ErrLog.txt agar tidak memicu AppDomain restart..." -ForegroundColor Yellow
    Remove-Item -LiteralPath $legacyBinLog -Force -ErrorAction SilentlyContinue
}

# 2. Ambil versi terkini dari AssemblyInfo.vb & auto-build bila DLL belum sinkron
$assemblyInfoPath = Join-Path $projectDir "My Project\AssemblyInfo.vb"
$versionMatches = @(Select-String -LiteralPath $assemblyInfoPath -Pattern '^\s*<Assembly:\s*AssemblyVersion\("(\d+\.\d+\.\d+\.\d+)"\)>')
if ($versionMatches.Count -ne 1) {
    throw 'AssemblyInfo.vb harus memiliki tepat satu AssemblyVersion aktif yang valid.'
}
$version = $versionMatches[0].Matches[0].Groups[1].Value
$mainDll = Join-Path $projectDir 'bin\DreamPos-WebReport.dll'
$dllVersion = if (Test-Path -LiteralPath $mainDll) { [Reflection.AssemblyName]::GetAssemblyName($mainDll).Version.ToString() } else { "" }

if ($dllVersion -ne $version) {
    $msbuildPath = "C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe"
    $slnPath = Join-Path (Get-Item $projectDir).Parent.FullName "DreamPos-WebReport.sln"
    if ((Test-Path -LiteralPath $msbuildPath) -and (Test-Path -LiteralPath $slnPath)) {
        Write-Host "-> Versi DLL ($dllVersion) berbeda dengan AssemblyInfo ($version). Menjalankan MSBuild otomatis..." -ForegroundColor Yellow
        & $msbuildPath $slnPath /t:Build /p:Configuration=Debug /v:minimal
        if ($LASTEXITCODE -ne 0) { throw "MSBuild gagal dengan exit code $LASTEXITCODE." }
        
        # Baca ulang versi setelah MSBuild (karena PreBuildEvent mengupdate AssemblyInfo.vb)
        $versionMatches = @(Select-String -LiteralPath $assemblyInfoPath -Pattern '^\s*<Assembly:\s*AssemblyVersion\("(\d+\.\d+\.\d+\.\d+)"\)>')
        $version = $versionMatches[0].Matches[0].Groups[1].Value
        $dllVersion = [Reflection.AssemblyName]::GetAssemblyName($mainDll).Version.ToString()
    }
    if ($dllVersion -ne $version) {
        throw "Versi DLL ($dllVersion) tidak sama dengan source ($version). Build project terlebih dahulu."
    }
}
Write-Host "-> Versi Terdeteksi & Terverifikasi: $version" -ForegroundColor Green

# 3. Siapkan folder staging sementara
$timestamp = (Get-Date -Format 'yyyyMMdd_HHmmss_fff') + '_' + [Guid]::NewGuid().ToString('N').Substring(0,8)
$stagingDir = Join-Path $outputDir "staging_$timestamp"
$zipPath = $null
$zipAttempted = $false
$packageComplete = $false
New-Item -ItemType Directory -Path $stagingDir | Out-Null

try {
    Write-Host "-> Menyalin file binary, asset, & view ke folder staging..." -ForegroundColor Yellow

    # A. Daftar folder yang disertakan
    $includeDirs = @("bin", "Content", "dist", "images", "js", "pages", "Pages_InvBatchEdit", "plugins", "css", "docs", "webfonts")
    foreach ($d in $includeDirs) {
        $src = Join-Path $projectDir $d
        if (Test-Path $src) {
            $dest = Join-Path $stagingDir $d
            Copy-Item -LiteralPath $src -Destination $dest -Recurse -Force
        }
    }

    # B. Sertakan Pathdata\schema_master.json jika ada
    $schemaSrc = Join-Path $projectDir "Pathdata\schema_master.json"
    if (Test-Path $schemaSrc) {
        $destPathdata = Join-Path $stagingDir "Pathdata"
        New-Item -ItemType Directory -Path $destPathdata -Force | Out-Null
        Copy-Item -LiteralPath $schemaSrc -Destination (Join-Path $destPathdata "schema_master.json") -Force
        Write-Host "-> Kamus skema schema_master.json disertakan." -ForegroundColor Green
    }

    # C. Sertakan database\migrations jika ada di root repo
    $repoRoot = (Get-Item $projectDir).Parent.FullName
    $dbMigrationsSrc = Join-Path $repoRoot "database"
    if (Test-Path $dbMigrationsSrc) {
        $dbMigrationsDest = Join-Path $stagingDir "database"
        Copy-Item -Path $dbMigrationsSrc -Destination $dbMigrationsDest -Recurse -Force
        Write-Host "-> Folder database\migrations disertakan." -ForegroundColor Green
    }

    # D. Daftar ekstensi file root yang disertakan
    $includeExts = @("*.aspx", "*.ashx", "*.master", "*.html", "*.htm", "*.ico", "*.png", "*.jpg", "*.gif", "*.css", "*.js", "CHANGELOG.md")
    foreach ($ext in $includeExts) {
        Get-ChildItem -Path $projectDir -Filter $ext -File | ForEach-Object {
            Copy-Item -Path $_.FullName -Destination $stagingDir -Force
        }
    }

    # E. KEAMANAN KRITIS: Bersihkan file konfigurasi dev lokal / source code / temporary dari staging
    $excludePatterns = @(
        "Web.config",
        "Web.config.bak*",
        "Web.Debug.config",
        "Web.Release.config",
        "ErrLog.txt",
        "ErrLog_*.bak",
        "ErrClose.txt",
        "Err.csv",
        "*.vbproj",
        "*.vbproj.user",
        "*.vb",
        "*.recovered",
        "maintenance.flag",
        "*.backup_*",
        "*old.aspx",
        "update_version.ps1",
        "generate_schema_master.ps1",
        "package_release.ps1"
    )

    foreach ($pat in $excludePatterns) {
        Get-ChildItem -Path $stagingDir -Filter $pat -Recurse -File | ForEach-Object {
            if (-not $_.FullName.StartsWith($stagingDir + '\', [StringComparison]::OrdinalIgnoreCase)) { throw 'Path cleanup di luar staging.' }
            Remove-Item -LiteralPath $_.FullName -Force
        }
    }

    # F. Rename Web.config lokal menjadi Web.config.template dengan path errLog relatif
    $localWebConfig = Join-Path $projectDir "Web.config"
    if (Test-Path -LiteralPath $localWebConfig) {
        $templateDest = Join-Path $stagingDir "Web.config.template"
        Copy-Item -LiteralPath $localWebConfig -Destination $templateDest -Force
        try {
            [xml]$tplXml = Get-Content -LiteralPath $templateDest -Encoding UTF8
            $appSettingsNode = $tplXml.SelectSingleNode("//configuration/applicationSettings/DreamPos_WebReport.My.MySettings")
            if ($null -ne $appSettingsNode) {
                # Normalisasi path file log agar tidak hardcode folder lokal developer
                $mapKeys = @{
                    "errLog"   = "ErrLog.txt"
                    "errClose" = "ErrClose.txt"
                    "errcsv"   = "Err.csv"
                }
                foreach ($k in $mapKeys.Keys) {
                    $valNode = $appSettingsNode.SelectSingleNode("setting[@name='$k']/value")
                    if ($null -ne $valNode) { $valNode.InnerText = $mapKeys[$k] }
                }
            }

            $writerSettings = New-Object System.Xml.XmlWriterSettings
            $writerSettings.Indent = $true
            $writerSettings.IndentChars = "  "
            $writerSettings.Encoding = New-Object System.Text.UTF8Encoding($false)
            $w = [System.Xml.XmlWriter]::Create($templateDest, $writerSettings)
            try { $tplXml.Save($w) } finally { $w.Close() }
        } catch { }
        Write-Host "-> Menyertakan Web.config yang di-rename menjadi Web.config.template (Aman, TIDAK menimpa Web.config server)." -ForegroundColor Green
    }

    # Pastikan file Web.config asli tidak ada di staging
    if (Test-Path -LiteralPath (Join-Path $stagingDir "Web.config")) {
        Remove-Item -LiteralPath (Join-Path $stagingDir "Web.config") -Force
    }

    # G. Sertakan script update server (apply_server_update.ps1 & launcher batch)
    $serverScriptSrc = Join-Path $projectDir "apply_server_update.ps1"
    if (Test-Path -LiteralPath $serverScriptSrc) {
        Copy-Item -LiteralPath $serverScriptSrc -Destination (Join-Path $stagingDir "apply_server_update.ps1") -Force
    }

    $serverBatSrc = Join-Path $projectDir "JALANKAN_UPDATE_SERVER.bat"
    if (Test-Path -LiteralPath $serverBatSrc) {
        Copy-Item -LiteralPath $serverBatSrc -Destination (Join-Path $stagingDir "JALANKAN_UPDATE_SERVER.bat") -Force
    }

    # H. Generate Panduan Deployment
    $guideContent = @"
========================================================================
   PANDUAN UPDATE DAN DEPLOYMENT DREAMPOS WEBREPORT (v$version)
   Tanggal Paket: $(Get-Date -Format 'dd MMMM yyyy HH:mm')
========================================================================

1. KELENGKAPAN ISI PAKET UPDATE:
   - bin\                         : DLL terkompilasi v$version (lengkap dengan clsSchemaMigrator & perbaikan log)
   - Web.config.template          : Template konfigurasi terbaru (Web.config server asli TIDAK AKAN tertimpa)
   - *.aspx & *.ashx              : Seluruh view dan handler ASP.NET
   - apply_server_update.ps1      : Script PowerShell otomatis untuk menerapkan update di server
   - JALANKAN_UPDATE_SERVER.bat   : Launcher 1-klik (Run as Administrator) untuk server
   - database\migrations\         : Script migrasi database SQL (jika ada)
   - Content\, dist\, js\, css\   : Asset antarmuka aplikasi

2. ALUR UPDATE KE SERVER PRODUCTION (SMOOTH & AMAN):
   Langkah 1: Salin (Copy) file ZIP ini ke server.
   Langkah 2: Extract file ZIP langsung ke dalam folder web aplikasi di IIS
              (Contoh: C:\inetpub\wwwroot\DreamPos-WebReport).
              *CATATAN: Web.config server Anda 100% AMAN dan TIDAK TERTMPA karena
               di dalam paket bernama Web.config.template.*
   Langkah 3: Jalankan 'JALANKAN_UPDATE_SERVER.bat' (Klik Kanan -> Run as Administrator)
              ATAU jalankan PowerShell:
              powershell -ExecutionPolicy Bypass -File .\apply_server_update.ps1

3. APA YANG DILAKUKAN OLEH SCRIPT SERVER SECARA OTOMATIS:
   - Mengaktifkan Maintenance Mode (app_offline.htm) sehingga IIS membebaskan lock pada DLL.
   - Membuat Backup Full folder aplikasi ke _backups\Backup_YYYYMMDD_HHMMSS.
   - Menyinkronkan Web.config: menambahkan setting/appSettings baru dari Web.config.template
     tanpa mengubah ServerStr/password database server!
   - Mengoreksi path ErrLog.txt agar aman dan tidak memicu restart AppDomain.
   - Menjalankan migrasi SQL dari database\migrations\ ke database MySQL server.
   - Menghapus app_offline.htm dan melakukan health check otomatis ke Default.aspx.

4. LOGGING & TROUBLESHOOTING:
   - Seluruh aktivitas proses update dicatat di: UpdateLog_YYYYMMDD_HHMMSS.txt
   - Runtime error log aplikasi tercatat di: ErrLog.txt (dengan auto-rotation max 5MB).
========================================================================
"@
    Set-Content -Path (Join-Path $stagingDir "PANDUAN_UPDATE_SERVER.txt") -Value $guideContent -Encoding UTF8

    # I. Validasi file-file penting
    $requiredFiles = @(
        'bin\DreamPos-WebReport.dll',
        'Web.config.template',
        'apply_server_update.ps1',
        'JALANKAN_UPDATE_SERVER.bat',
        'Default.aspx',
        'Inventoryhome.aspx'
    )
    foreach ($req in $requiredFiles) {
        $fItem = Get-Item -LiteralPath (Join-Path $stagingDir $req)
        if ($fItem.PSIsContainer -or $fItem.Length -eq 0) {
            throw "File wajib tidak ditemukan atau berukuran 0: $req"
        }
    }

    # J. Kompresi ke file ZIP rilis
    $zipName = "Update_DreamPosWebReport_v$($version)_$timestamp.zip"
    $zipPath = Join-Path $outputDir $zipName

    Write-Host "-> Mengompresi paket rilis ke: $zipPath" -ForegroundColor Yellow
    Add-Type -AssemblyName System.IO.Compression.FileSystem
    $zipAttempted = $true
    [System.IO.Compression.ZipFile]::CreateFromDirectory($stagingDir, $zipPath)

    # Verifikasi isi ZIP
    $archive = [System.IO.Compression.ZipFile]::OpenRead($zipPath)
    try {
        $entries = @($archive.Entries | ForEach-Object { $_.FullName.Replace('\','/') })
        if ($entries -contains 'Web.config') {
            throw "BAHAYA: Web.config ditemukan di dalam ZIP! Packaging dibatalkan."
        }
        foreach ($req in $requiredFiles) {
            if ($entries -notcontains $req.Replace('\','/')) {
                throw "File wajib $req tidak masuk ke dalam ZIP."
            }
        }
    } finally {
        $archive.Dispose()
    }

    $packageComplete = $true

} finally {
    $resolvedStaging = [IO.Path]::GetFullPath($stagingDir)
    if (Test-Path -LiteralPath $resolvedStaging) {
        Remove-Item -LiteralPath $resolvedStaging -Recurse -Force -ErrorAction SilentlyContinue
    }
    if (-not $packageComplete -and $zipAttempted -and $zipPath -and (Test-Path -LiteralPath $zipPath)) {
        Remove-Item -LiteralPath $zipPath -Force -ErrorAction SilentlyContinue
    }
}

Write-Host "==========================================================" -ForegroundColor Cyan
Write-Host "  BERHASIL! Paket rilis siap didistribusikan:            " -ForegroundColor Green
Write-Host "  File   : $zipPath" -ForegroundColor White
$fileSizeMB = (Get-Item $zipPath).Length / 1MB
Write-Host "  Ukuran : $([math]::Round($fileSizeMB, 2)) MB" -ForegroundColor White
Write-Host "==========================================================" -ForegroundColor Cyan
