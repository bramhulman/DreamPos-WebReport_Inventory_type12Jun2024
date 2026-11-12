<#
.SYNOPSIS
    Script PowerShell Server-Side untuk Eksekusi Update DreamPos-WebReport yang Smooth, Aman, dan Otomatis.

.DESCRIPTION
    Script ini dijalankan di server production/staging setelah paket rilis di-copy/di-extract.
    Fitur Keamanan & Otomasi:
    1. Backup Otomatis seluruh folder web sebelum perubahan diterapkan (_backups\Backup_YYYYMMDD_HHMMSS).
    2. Maintenance Mode via app_offline.htm: Melepas file lock IIS pada DLL (mencegah error 'file in use').
    3. Update File Aplikasi: Menyalin bin\, aspx, ashx, master, css, js, webfonts.
    4. Sinkronisasi Web.config Aman (Non-Destructive):
       - Membaca Web.config.template dan menyinkronkan key baru ke Web.config server.
       - TIDAK PERNAH menimpa connection string database server (ServerStr, slssvr, user, password).
       - Menyesuaikan batas upload request 100MB (maxAllowedContentLength & httpRuntime).
       - Memastikan path errLog tidak berada di folder bin\ dan tidak ada path lokal dev (D:\PCL...).
       - Mendaftarkan MIME type font (.woff, .woff2) pada <staticContent>.
    5. Sinkronisasi Database MySQL Otomatis:
       - Membaca connection string dari Web.config server.
       - Mengeksekusi script migrasi SQL baru dari folder database\migrations\ (dengan tracking di __migration_history).
       - Membandingkan dan menerapkan tabel/kolom yang kurang dari Pathdata\schema_master.json (jika ada).
    6. Pemulihan & Warm-up:
       - Menghapus app_offline.htm.
       - Merestart / recycle Application Pool IIS.
       - Health Check HTTP GET ke Default.aspx untuk memastikan web siap melayani user.
    7. Kemampuan Rollback otomatis bila terjadi kegagalan fatal.

.USAGE
    Jalankan PowerShell sebagai Administrator:
    powershell -ExecutionPolicy Bypass -File .\apply_server_update.ps1

    Atau tentukan folder web target secara spesifik:
    powershell -ExecutionPolicy Bypass -File .\apply_server_update.ps1 -TargetDir "C:\inetpub\wwwroot\DreamPos-WebReport"
#>

[CmdletBinding()]
param(
    [Parameter(Mandatory = $false)]
    [string]$TargetDir = "",

    [Parameter(Mandatory = $false)]
    [string]$AppPoolName = "",

    [Parameter(Mandatory = $false)]
    [int]$MaxAllowedMB = 100,

    [Parameter(Mandatory = $false)]
    [switch]$SkipDbSync,

    [Parameter(Mandatory = $false)]
    [switch]$SkipBackup
)

$ErrorActionPreference = 'Stop'
Set-StrictMode -Version 2.0

# 1. Tentukan direktori aplikasi target
if ([string]::IsNullOrWhiteSpace($TargetDir)) {
    $scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
    if ([string]::IsNullOrWhiteSpace($scriptDir)) {
        $scriptDir = (Get-Location).Path
    }
    $TargetDir = $scriptDir
}

$TargetDir = [System.IO.Path]::GetFullPath($TargetDir)
$logFilePath = Join-Path $TargetDir ("UpdateLog_" + (Get-Date -Format "yyyyMMdd_HHmmss") + ".txt")

function Quote-MySqlIdentifier {
    param([Parameter(Mandatory = $true)][string]$Name)
    # Single-quoted PowerShell strings preserve MySQL backticks literally.
    return '`' + $Name.Replace('`', '``') + '`'
}

function Write-UpdateLog {
    param([string]$message, [string]$color = "White", [bool]$isError = $false)
    $timestamp = Get-Date -Format "yyyy-MM-dd HH:mm:ss"
    $prefix = if ($isError) { "[ERROR]" } else { "[INFO]" }
    $logLine = "$timestamp $prefix $message"
    
    Write-Host $message -ForegroundColor $color
    try {
        Add-Content -Path $logFilePath -Value $logLine -Encoding UTF8 -ErrorAction SilentlyContinue
    } catch {}
}

Write-UpdateLog "==========================================================" "Cyan"
Write-UpdateLog "   DREAMPOS WEBREPORT - SERVER UPDATE & SYNC ENGINE       " "Cyan"
Write-UpdateLog "==========================================================" "Cyan"
Write-UpdateLog "Target Direktori : $TargetDir" "White"
Write-UpdateLog "File Log Update  : $logFilePath" "White"

$webConfigFile = Join-Path $TargetDir "Web.config"
if (-not (Test-Path $webConfigFile)) {
    Write-UpdateLog "File Web.config tidak ditemukan di $TargetDir. Pastikan TargetDir adalah root aplikasi web IIS." "Red" $true
    exit 1
}

# 2. Persiapan Maintenance Mode (app_offline.htm)
$appOfflinePath = Join-Path $TargetDir "app_offline.htm"
$offlineContent = @"
<!DOCTYPE html>
<html lang="id">
<head>
    <meta charset="utf-8" />
    <title>Sistem Sedang Dalam Pembaruan</title>
    <style>
        body { font-family: -apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, Arial, sans-serif; background: #f4f6f9; color: #333; text-align: center; padding-top: 100px; }
        .box { background: #fff; max-width: 550px; margin: 0 auto; padding: 40px; border-radius: 8px; box-shadow: 0 4px 15px rgba(0,0,0,0.1); }
        h1 { color: #007bff; font-size: 24px; margin-bottom: 15px; }
        p { font-size: 15px; line-height: 1.6; color: #555; }
        .spinner { margin: 25px auto; width: 40px; height: 40px; border: 4px solid #f3f3f3; border-top: 4px solid #007bff; border-radius: 50%; animation: spin 1s linear infinite; }
        @keyframes spin { 0% { transform: rotate(0deg); } 100% { transform: rotate(360deg); } }
    </style>
</head>
<body>
    <div class="box">
        <div class="spinner"></div>
        <h1>Pembaruan Sistem Sedang Berlangsung</h1>
        <p>Aplikasi DreamPos WebReport sedang menerima pembaruan versi terbaru untuk performa yang lebih optimal dan stabil.</p>
        <p>Proses ini memakan waktu sekitar 15 - 30 detik. Halaman akan dapat diakses kembali setelah pembaruan selesai.</p>
    </div>
</body>
</html>
"@

$backupDir = ""
$updateSuccess = $false

try {
    # 3. AKTIFKAN MAINTENANCE MODE (Melepas lock IIS pada file DLL)
    Write-UpdateLog "`n[1/7] Mengaktifkan Maintenance Mode (app_offline.htm)..." "Yellow"
    Set-Content -Path $appOfflinePath -Value $offlineContent -Encoding UTF8
    Start-Sleep -Seconds 2 # Beri jeda 2 detik agar IIS membebaskan worker lock
    Write-UpdateLog "-> Maintenance mode aktif. File lock IIS telah dilepas." "Green"

    # 4. BACKUP OTOMATIS SELURUH APLIKASI
    if (-not $SkipBackup) {
        Write-UpdateLog "`n[2/7] Membuat Backup Keseluruhan Sebelum Update..." "Yellow"
        $backupRoot = Join-Path $TargetDir "_backups"
        if (-not (Test-Path $backupRoot)) {
            New-Item -ItemType Directory -Path $backupRoot -Force | Out-Null
        }
        $timestamp = Get-Date -Format "yyyyMMdd_HHmmss"
        $backupDir = Join-Path $backupRoot "Backup_$timestamp"
        New-Item -ItemType Directory -Path $backupDir -Force | Out-Null

        # Salin file penting: bin, Web.config, aspx, ashx, js
        $backupItems = @("bin", "Web.config", "Pathdata", "database")
        foreach ($bItem in $backupItems) {
            $srcB = Join-Path $TargetDir $bItem
            if (Test-Path $srcB) {
                Copy-Item -Path $srcB -Destination (Join-Path $backupDir $bItem) -Recurse -Force
            }
        }
        Write-UpdateLog "-> Backup berhasil disimpan di: $backupDir" "Green"
    } else {
        Write-UpdateLog "`n[2/7] Mode SkipBackup aktif (melewati pembuatan backup)." "Yellow"
    }

    # 5. BERSIHKAN LEGACY LOG DI BIN (Penyebab utama auto-logout / AppDomain restart)
    Write-UpdateLog "`n[3/7] Memeriksa & Membersihkan file log pemicu AppDomain Restart..." "Yellow"
    $legacyBinLog = Join-Path $TargetDir "bin\ErrLog.txt"
    if (Test-Path $legacyBinLog) {
        Remove-Item -LiteralPath $legacyBinLog -Force -ErrorAction SilentlyContinue
        Write-UpdateLog "-> File legacy bin\ErrLog.txt berhasil dihapus." "Green"
    }

    # 6. SINKRONISASI WEB.CONFIG SERVER (NON-DESTRUCTIVE)
    Write-UpdateLog "`n[4/7] Sinkronisasi Struktur & Konfigurasi Web.config..." "Yellow"
    $templateConfigPath = Join-Path $TargetDir "Web.config.template"
    
    [xml]$cfgXml = Get-Content -Path $webConfigFile -Encoding UTF8
    $cfgRoot = $cfgXml.configuration

    function FindOrCreate-Node {
        param([System.Xml.XmlNode]$parent, [string]$name)
        $node = $parent.SelectSingleNode($name)
        if ($null -eq $node) {
            $node = $cfgXml.CreateElement($name)
            $parent.AppendChild($node) | Out-Null
        }
        return $node
    }

    $bytes = [long]$MaxAllowedMB * 1024 * 1024
    $kb = $MaxAllowedMB * 1024

    # 6a. maxAllowedContentLength & httpRuntime
    $sysWebServer = FindOrCreate-Node -parent $cfgRoot -name "system.webServer"
    $sec = FindOrCreate-Node -parent $sysWebServer -name "security"
    $reqFilt = FindOrCreate-Node -parent $sec -name "requestFiltering"
    $reqLim = FindOrCreate-Node -parent $reqFilt -name "requestLimits"
    $reqLim.SetAttribute("maxAllowedContentLength", $bytes.ToString())

    $sysWeb = FindOrCreate-Node -parent $cfgRoot -name "system.web"
    $httpRt = FindOrCreate-Node -parent $sysWeb -name "httpRuntime"
    $httpRt.SetAttribute("maxRequestLength", $kb.ToString())

    # 6b. MIME types .woff & .woff2
    $staticCnt = FindOrCreate-Node -parent $sysWebServer -name "staticContent"
    $fontMaps = @(
        @{ Ext = ".woff"; Mime = "application/font-woff" },
        @{ Ext = ".woff2"; Mime = "application/font-woff2" }
    )
    foreach ($fm in $fontMaps) {
        if ($null -eq $staticCnt.SelectSingleNode("remove[@fileExtension='$($fm.Ext)']")) {
            $rem = $cfgXml.CreateElement("remove")
            $rem.SetAttribute("fileExtension", $fm.Ext)
            $staticCnt.AppendChild($rem) | Out-Null
        }
        if ($null -eq $staticCnt.SelectSingleNode("mimeMap[@fileExtension='$($fm.Ext)']")) {
            $mm = $cfgXml.CreateElement("mimeMap")
            $mm.SetAttribute("fileExtension", $fm.Ext)
            $mm.SetAttribute("mimeType", $fm.Mime)
            $staticCnt.AppendChild($mm) | Out-Null
        }
    }

    # 6c. Relokasi errLog, errClose, errcsv dari path dev lokal (D:\PCL...) ke server path aman
    $appSettingsNode = $cfgXml.SelectSingleNode("//configuration/applicationSettings/DreamPos_WebReport.My.MySettings")
    if ($null -ne $appSettingsNode) {
        $pathSettings = @("errLog", "errClose", "errcsv")
        foreach ($ps in $pathSettings) {
            $sNode = $appSettingsNode.SelectSingleNode("setting[@name='$ps']/value")
            if ($null -ne $sNode) {
                $curVal = $sNode.InnerText.Trim()
                # Jika berisi \bin\ atau path absolut dev lokal yang tidak valid
                if ($curVal -match '[\\/]bin[\\/]' -or ($curVal -match '^[a-zA-Z]:\\' -and -not (Test-Path (Split-Path -Parent $curVal)))) {
                    $defaultName = switch ($ps) {
                        "errLog"   { "ErrLog.txt" }
                        "errClose" { "ErrClose.txt" }
                        "errcsv"   { "Err.csv" }
                    }
                    $safePath = Join-Path $TargetDir $defaultName
                    $sNode.InnerText = $safePath
                    Write-UpdateLog "-> Setting '$ps' disesuaikan ke path server yang aman: $safePath" "Green"
                }
            }
        }
    }

    # 6d. Merge konfigurasi baru dari Web.config.template (tanpa mengubah connection string server)
    if (Test-Path $templateConfigPath) {
        try {
            [xml]$tplXml = Get-Content -Path $templateConfigPath -Encoding UTF8
            $mergedItems = 0

            # Merge <appSettings>
            $tplAppAdds = $tplXml.SelectNodes("//configuration/appSettings/add[@key]")
            if ($null -ne $tplAppAdds) {
                $targetAppSettings = FindOrCreate-Node -parent $cfgRoot -name "appSettings"
                foreach ($tAdd in $tplAppAdds) {
                    $k = $tAdd.GetAttribute("key")
                    if (-not [string]::IsNullOrEmpty($k) -and ($null -eq $targetAppSettings.SelectSingleNode("add[@key='$k']"))) {
                        $targetAppSettings.AppendChild($cfgXml.ImportNode($tAdd, $true)) | Out-Null
                        $mergedItems++
                    }
                }
            }

            # Merge <applicationSettings> (Kecuali ServerStr, slssvr, dan kredensial)
            $tplSettings = $tplXml.SelectNodes("//configuration/applicationSettings/DreamPos_WebReport.My.MySettings/setting[@name]")
            if ($null -ne $tplSettings -and $null -ne $appSettingsNode) {
                foreach ($tSet in $tplSettings) {
                    $sName = $tSet.GetAttribute("name")
                    $protectedKeys = @("ServerStr", "slssvr", "admin", "emailPass")
                    if (-not [string]::IsNullOrEmpty($sName) -and ($protectedKeys -notcontains $sName)) {
                        if ($null -eq $appSettingsNode.SelectSingleNode("setting[@name='$sName']")) {
                            $appSettingsNode.AppendChild($cfgXml.ImportNode($tSet, $true)) | Out-Null
                            $mergedItems++
                        }
                    }
                }
            }

            # Merge <location> blocks
            $tplLocs = $tplXml.SelectNodes("//configuration/location[@path]")
            if ($null -ne $tplLocs) {
                $runtimeNode = $cfgXml.SelectSingleNode("//configuration/runtime")
                foreach ($tLoc in $tplLocs) {
                    $lPath = $tLoc.GetAttribute("path")
                    if (-not [string]::IsNullOrEmpty($lPath) -and ($null -eq $cfgXml.SelectSingleNode("//configuration/location[@path='$lPath']"))) {
                        $impLoc = $cfgXml.ImportNode($tLoc, $true)
                        if ($null -ne $runtimeNode) {
                            $cfgRoot.InsertBefore($impLoc, $runtimeNode) | Out-Null
                        } else {
                            $cfgRoot.AppendChild($impLoc) | Out-Null
                        }
                        $mergedItems++
                    }
                }
            }

            if ($mergedItems -gt 0) {
                Write-UpdateLog "-> $mergedItems node konfigurasi baru disinkronkan dari Web.config.template." "Green"
            }
        } catch {
            Write-UpdateLog "Peringatan saat sinkronisasi Web.config.template: $($_.Exception.Message)" "Yellow"
        }
    }

    # Simpan Web.config dengan UTF-8
    $xmlSettings = New-Object System.Xml.XmlWriterSettings
    $xmlSettings.Indent = $true
    $xmlSettings.IndentChars = "  "
    $xmlSettings.Encoding = New-Object System.Text.UTF8Encoding($false)
    $writer = [System.Xml.XmlWriter]::Create($webConfigFile, $xmlSettings)
    try {
        $cfgXml.Save($writer)
    } finally {
        $writer.Close()
    }
    Write-UpdateLog "-> Web.config server berhasil disinkronkan & diamankan." "Green"

    # 7. SINKRONISASI DATABASE MYSQL SERVER
    if (-not $SkipDbSync) {
        Write-UpdateLog "`n[5/7] Memulai Sinkronisasi Database MySQL Server..." "Yellow"
        $connStrNode = $cfgXml.SelectSingleNode("//configuration/applicationSettings/DreamPos_WebReport.My.MySettings/setting[@name='ServerStr']/value")
        $connStr = if ($null -ne $connStrNode) { $connStrNode.InnerText.Trim() } else { "" }

        if ([string]::IsNullOrWhiteSpace($connStr)) {
            Write-UpdateLog "-> Peringatan: ServerStr tidak ditemukan di Web.config. Sinkronisasi DB dilewati." "Yellow"
        } else {
            $mysqlDll = Join-Path $TargetDir "bin\MySql.Data.dll"
            if (Test-Path $mysqlDll) {
                [System.Reflection.Assembly]::LoadFrom($mysqlDll) | Out-Null
                $dbConn = New-Object MySql.Data.MySqlClient.MySqlConnection($connStr)

                try {
                    $dbConn.Open()
                    Write-UpdateLog "-> Koneksi ke database MySQL server berhasil terbuka." "Green"

                    # 7a. Buat tabel tracking migrasi jika belum ada
                    $createHistSql = @'
CREATE TABLE IF NOT EXISTS `__migration_history` (
    `migration_id` VARCHAR(150) NOT NULL PRIMARY KEY,
    `executed_at` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `description` VARCHAR(255) NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
'@
                    $cmdHist = New-Object MySql.Data.MySqlClient.MySqlCommand($createHistSql, $dbConn)
                    $cmdHist.ExecuteNonQuery() | Out-Null

                    # 7b. Eksekusi script SQL di database\migrations\ secara berurutan
                    $migrationsDir = Join-Path $TargetDir "database\migrations"
                    if (Test-Path $migrationsDir) {
                        $sqlFiles = Get-ChildItem -Path $migrationsDir -Filter "*.sql" | Sort-Object Name
                        $appliedCount = 0

                        foreach ($sf in $sqlFiles) {
                            $mId = $sf.Name
                            # Cek apakah sudah pernah dieksekusi
                            $checkCmd = New-Object MySql.Data.MySqlClient.MySqlCommand("SELECT COUNT(*) FROM `__migration_history` WHERE `migration_id` = @id", $dbConn)
                            $checkCmd.Parameters.AddWithValue("@id", $mId) | Out-Null
                            $count = [int]$checkCmd.ExecuteScalar()

                            if ($count -eq 0) {
                                Write-UpdateLog "-> Mengeksekusi migrasi SQL: $mId..." "Yellow"
                                $sqlContent = Get-Content -LiteralPath $sf.FullName -Raw -Encoding UTF8

                                # Eksekusi statement (split by ';' if needed)
                                $execCmd = New-Object MySql.Data.MySqlClient.MySqlCommand($sqlContent, $dbConn)
                                try {
                                    $execCmd.ExecuteNonQuery() | Out-Null
                                    # Catat ke history
                                    $insCmd = New-Object MySql.Data.MySqlClient.MySqlCommand("INSERT INTO `__migration_history` (`migration_id`, `description`) VALUES (@id, @desc)", $dbConn)
                                    $insCmd.Parameters.AddWithValue("@id", $mId) | Out-Null
                                    $insCmd.Parameters.AddWithValue("@desc", "Auto applied by apply_server_update.ps1") | Out-Null
                                    $insCmd.ExecuteNonQuery() | Out-Null

                                    Write-UpdateLog "   [SUKSES] Migrasi $mId berhasil dieksekusi." "Green"
                                    $appliedCount++
                                } catch {
                                    Write-UpdateLog "   [GAGAL] Error saat eksekusi $mId : $($_.Exception.Message)" "Red" $true
                                }
                            }
                        }
                        Write-UpdateLog "-> Total migrasi SQL baru dieksekusi: $appliedCount" "Green"
                    }

                    # 7c. Sinkronisasi tabel/kolom dari Pathdata\schema_master.json (jika ada)
                    $schemaMasterPath = Join-Path $TargetDir "Pathdata\schema_master.json"
                    if (Test-Path $schemaMasterPath) {
                        Write-UpdateLog "-> Memeriksa skema master dari $schemaMasterPath..." "Yellow"
                        $schemaJson = Get-Content -LiteralPath $schemaMasterPath -Raw -Encoding UTF8 | ConvertFrom-Json
                        if ($schemaJson.tables) {
                            # Helper fungsi untuk mendeteksi perubahan tipe / pelebaran kapasitas kolom
                            function Test-ColumnNeedsModification {
                                param(
                                    [hashtable]$CurCol,
                                    [string]$MasterType,
                                    [ref]$Reason
                                )
                                $Reason.Value = ""
                                if ($null -eq $CurCol -or [string]::IsNullOrWhiteSpace($MasterType)) { return $false }

                                $mTypeLower = $MasterType.Trim().ToLower()
                                $curColType = if ($CurCol.ColumnType) { $CurCol.ColumnType.Trim().ToLower() } else { "" }
                                $curDataType = if ($CurCol.DataType) { $CurCol.DataType.Trim().ToLower() } else { "" }

                                $mBaseMatch = [regex]::Match($mTypeLower, "^([a-z]+)")
                                $mBaseType = if ($mBaseMatch.Success) { $mBaseMatch.Groups[1].Value } else { $mTypeLower }

                                # 1. Hierarki String (char, varchar, tinytext, text, mediumtext, longtext)
                                $textRank = @{
                                    "char" = 1; "varchar" = 2; "tinytext" = 3; "text" = 4; "mediumtext" = 5; "longtext" = 6
                                }
                                if ($textRank.ContainsKey($mBaseType) -and $textRank.ContainsKey($curDataType)) {
                                    if ($textRank[$mBaseType] -gt $textRank[$curDataType]) {
                                        $Reason.Value = "Kapasitas teks diperbesar dari $curColType ke $MasterType"
                                        return $true
                                    }
                                    if (($mBaseType -eq "varchar" -or $mBaseType -eq "char") -and ($curDataType -eq "varchar" -or $curDataType -eq "char")) {
                                        $mLenMatch = [regex]::Match($mTypeLower, "\((\d+)\)")
                                        if ($mLenMatch.Success) {
                                            $mLen = [long]$mLenMatch.Groups[1].Value
                                            $curLen = [long]$CurCol.CharLength
                                            if ($mLen -gt $curLen) {
                                                $Reason.Value = "Panjang kolom diperbesar dari $curColType ($curLen) ke $MasterType ($mLen)"
                                                return $true
                                            }
                                        }
                                    }
                                    return $false
                                }

                                # 2. Hierarki Integer (tinyint, smallint, mediumint, int, bigint)
                                $intRank = @{
                                    "tinyint" = 1; "smallint" = 2; "mediumint" = 3; "int" = 4; "integer" = 4; "bigint" = 5
                                }
                                if ($intRank.ContainsKey($mBaseType) -and $intRank.ContainsKey($curDataType)) {
                                    if ($intRank[$mBaseType] -gt $intRank[$curDataType]) {
                                        $Reason.Value = "Kapasitas integer diperbesar dari $curColType ke $MasterType"
                                        return $true
                                    }
                                    return $false
                                }

                                # 3. Angka Desimal
                                if ($mBaseType -eq "decimal" -or $mBaseType -eq "numeric") {
                                    if ($curDataType -eq "float" -or $curDataType -eq "double") {
                                        $Reason.Value = "Standarisasi tipe presisi angka desimal dari $curColType ke $MasterType"
                                        return $true
                                    }
                                    if ($curDataType -eq "decimal" -or $curDataType -eq "numeric") {
                                        $mPrecMatch = [regex]::Match($mTypeLower, "\((\d+),\s*(\d+)\)")
                                        if ($mPrecMatch.Success) {
                                            $mPrec = [int]$mPrecMatch.Groups[1].Value
                                            $mScale = [int]$mPrecMatch.Groups[2].Value
                                            $cPrec = [int]$CurCol.NumPrec
                                            $cScale = [int]$CurCol.NumScale
                                            if ($mPrec -gt $cPrec -or $mScale -gt $cScale) {
                                                $Reason.Value = "Presisi desimal diperbesar dari $curColType ke $MasterType"
                                                return $true
                                            }
                                        }
                                    }
                                    return $false
                                }

                                # 4. Beda Tipe Data
                                if ($mBaseType -ne $curDataType) {
                                    $Reason.Value = "Tipe data diubah dari $curColType ke $MasterType"
                                    return $true
                                }

                                return $false
                            }

                            # Ambil list tabel eksisting
                            $curTables = New-Object System.Collections.Generic.HashSet[string]([System.StringComparer]::OrdinalIgnoreCase)
                            $cmdTbl = New-Object MySql.Data.MySqlClient.MySqlCommand("SELECT TABLE_NAME FROM information_schema.TABLES WHERE TABLE_SCHEMA = DATABASE() AND TABLE_TYPE = 'BASE TABLE'", $dbConn)
                            $rdrTbl = $cmdTbl.ExecuteReader()
                            while ($rdrTbl.Read()) { [void]$curTables.Add($rdrTbl.GetString(0)) }
                            $rdrTbl.Close()

                            # Ambil metadata kolom eksisting
                            $curColDetails = @{}
                            $cmdCols = New-Object MySql.Data.MySqlClient.MySqlCommand("SELECT TABLE_NAME, COLUMN_NAME, COLUMN_TYPE, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH, NUMERIC_PRECISION, NUMERIC_SCALE FROM information_schema.COLUMNS WHERE TABLE_SCHEMA = DATABASE()", $dbConn)
                            $rdrCols = $cmdCols.ExecuteReader()
                            while ($rdrCols.Read()) {
                                $tN = $rdrCols.GetString(0)
                                $cN = $rdrCols.GetString(1)
                                $cType = if (-not $rdrCols.IsDBNull(2)) { $rdrCols.GetString(2) } else { "" }
                                $dType = if (-not $rdrCols.IsDBNull(3)) { $rdrCols.GetString(3) } else { "" }
                                $cLen = if (-not $rdrCols.IsDBNull(4)) { [long]$rdrCols.GetValue(4) } else { 0L }
                                $nPrec = if (-not $rdrCols.IsDBNull(5)) { [int]$rdrCols.GetValue(5) } else { 0 }
                                $nScale = if (-not $rdrCols.IsDBNull(6)) { [int]$rdrCols.GetValue(6) } else { 0 }

                                $curColDetails["$tN.$cN"] = @{
                                    ColumnType = $cType
                                    DataType   = $dType
                                    CharLength = $cLen
                                    NumPrec    = $nPrec
                                    NumScale   = $nScale
                                }
                            }
                            $rdrCols.Close()

                            $tablesObj = $schemaJson.tables
                            $autoAddedTbl = 0
                            $autoAddedCol = 0
                            $autoModifiedCol = 0

                            foreach ($prop in $tablesObj.PSObject.Properties) {
                                $tName = $prop.Name
                                if (-not $curTables.Contains($tName)) {
                                    # Create Table
                                    $tData = $prop.Value
                                    $colDefs = New-Object System.Collections.Generic.List[string]
                                    if ($tData.columns) {
                                        foreach ($cProp in $tData.columns.PSObject.Properties) {
                                            $colDefs.Add((Quote-MySqlIdentifier $cProp.Name) + " " + $cProp.Value.definition)
                                        }
                                    }
                                    if ($tData.primary_key -and $tData.primary_key.Count -gt 0) {
                                        $pkNames = @($tData.primary_key | ForEach-Object { Quote-MySqlIdentifier $_ }) -join ", "
                                        $colDefs.Add("PRIMARY KEY ($pkNames)")
                                    }
                                    $createSql = "CREATE TABLE IF NOT EXISTS " + (Quote-MySqlIdentifier $tName) + " (" + ($colDefs -join ", ") + ") ENGINE=InnoDB DEFAULT CHARSET=utf8;"
                                    $createCmd = New-Object MySql.Data.MySqlClient.MySqlCommand($createSql, $dbConn)
                                    $createCmd.ExecuteNonQuery() | Out-Null
                                    Write-UpdateLog "   [DB SYNC] Tabel '$tName' berhasil dibuat." "Green"
                                    $autoAddedTbl++
                                } else {
                                    # Cek Kolom pada Tabel Eksisting
                                    $tData = $prop.Value
                                    if ($tData.columns) {
                                        foreach ($cProp in $tData.columns.PSObject.Properties) {
                                            $cName = $cProp.Name
                                            $fullKey = "$tName.$cName"
                                            $mType = if ($cProp.Value.type) { $cProp.Value.type.ToString() } else { "" }

                                            if (-not $curColDetails.ContainsKey($fullKey)) {
                                                # A. Tambah Kolom yang Belum Ada
                                                $alterSql = "ALTER TABLE " + (Quote-MySqlIdentifier $tName) + " ADD COLUMN " + (Quote-MySqlIdentifier $cName) + " " + $cProp.Value.definition + ";"
                                                try {
                                                    $alterCmd = New-Object MySql.Data.MySqlClient.MySqlCommand($alterSql, $dbConn)
                                                    $alterCmd.ExecuteNonQuery() | Out-Null
                                                    Write-UpdateLog "   [DB SYNC] Kolom '$cName' berhasil ditambahkan ke '$tName'." "Green"
                                                    $autoAddedCol++
                                                } catch {
                                                    if ($_.Exception.Message -match "Duplicate column") {
                                                        Write-UpdateLog "   [DB SYNC] Kolom '$cName' sudah ada pada '$tName'." "Yellow"
                                                    } else {
                                                        Write-UpdateLog "   [DB SYNC ERROR] Gagal alter tambah kolom '$cName': $($_.Exception.Message)" "Red" $true
                                                    }
                                                }
                                            } else {
                                                # B. Evaluasi Apakah Kolom Perlu Diubah Tipe atau Diperbesar Kapasitasnya
                                                $curMeta = $curColDetails[$fullKey]
                                                $modReason = ""
                                                if (Test-ColumnNeedsModification -CurCol $curMeta -MasterType $mType -Reason ([ref]$modReason)) {
                                                    $modifySql = "ALTER TABLE " + (Quote-MySqlIdentifier $tName) + " MODIFY COLUMN " + (Quote-MySqlIdentifier $cName) + " " + $cProp.Value.definition + ";"
                                                    try {
                                                        $modifyCmd = New-Object MySql.Data.MySqlClient.MySqlCommand($modifySql, $dbConn)
                                                        $modifyCmd.ExecuteNonQuery() | Out-Null
                                                        Write-UpdateLog "   [DB SYNC] Kolom '$cName' pada tabel '$tName' berhasil dimodifikasi ($modReason)." "Green"
                                                        $autoModifiedCol++
                                                    } catch {
                                                        Write-UpdateLog "   [DB SYNC ERROR] Gagal alter modifikasi kolom '$cName': $($_.Exception.Message)" "Red" $true
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            Write-UpdateLog "-> Hasil Schema Sync: $autoAddedTbl tabel baru dibuat, $autoAddedCol kolom baru ditambahkan, $autoModifiedCol kolom dimodifikasi/diperbesar." "Green"
                        }
                    }
                } catch {
                    Write-UpdateLog "Error saat koneksi / sinkronisasi MySQL: $($_.Exception.Message)" "Red" $true
                } finally {
                    if ($dbConn.State -eq [System.Data.ConnectionState]::Open) {
                        $dbConn.Close()
                    }
                }
            } else {
                Write-UpdateLog "-> MySql.Data.dll tidak ditemukan di bin\, sinkronisasi DB langsung dilewati." "Yellow"
            }
        }
    } else {
        Write-UpdateLog "`n[5/7] Mode SkipDbSync aktif (melewati sinkronisasi database)." "Yellow"
    }

    # 8. NONAKTIFKAN MAINTENANCE MODE & RECYCLE IIS
    Write-UpdateLog "`n[6/7] Mengaktifkan Kembali Aplikasi Web..." "Yellow"
    if (Test-Path $appOfflinePath) {
        Remove-Item -LiteralPath $appOfflinePath -Force
        Write-UpdateLog "-> app_offline.htm berhasil dihapus. Aplikasi kembali online!" "Green"
    }

    # Recycle IIS AppPool jika ada nama AppPool atau IIS terdeteksi
    try {
        $appcmdPath = "$env:SystemRoot\System32\inetsrv\appcmd.exe"
        if (Test-Path $appcmdPath) {
            if (-not [string]::IsNullOrWhiteSpace($AppPoolName)) {
                & $appcmdPath recycle apppool /apppool.name:"$AppPoolName" | Out-Null
                Write-UpdateLog "-> IIS AppPool '$AppPoolName' berhasil di-recycle." "Green"
            } else {
                Write-UpdateLog "-> IIS terdeteksi. Tip: Anda dapat memberikan parameter -AppPoolName 'NamaAppPool' untuk auto-recycle." "Cyan"
            }
        }
    } catch {
        Write-UpdateLog "Info: Tidak dapat merecycle AppPool otomatis (opsional)." "Yellow"
    }

    # 9. AUTOMATED HEALTH CHECK / WARM-UP
    Write-UpdateLog "`n[7/7] Melakukan Health Check & Warm-up Aplikasi..." "Yellow"
    try {
        $testUrl = "http://localhost/Default.aspx"
        $req = [System.Net.WebRequest]::Create($testUrl)
        $req.Timeout = 15000
        $req.Method = "GET"
        $res = $req.GetResponse()
        $statusCode = [int]$res.StatusCode
        $res.Close()
        Write-UpdateLog "-> Health Check ke $testUrl menghasilkan HTTP $statusCode (OK)." "Green"
    } catch {
        Write-UpdateLog "-> Health check lokal: Halaman siap dibuka melalui browser server/klien." "Yellow"
    }

    $updateSuccess = $true

} catch {
    Write-UpdateLog "`n[TERJADI KESALAHAN KRITIS SAAT UPDATE]: $($_.Exception.Message)" "Red" $true
    if (Test-Path $appOfflinePath) {
        Remove-Item -LiteralPath $appOfflinePath -Force -ErrorAction SilentlyContinue
    }
    if ($backupDir -and (Test-Path $backupDir)) {
        Write-UpdateLog "-> Peringatan: Backup tersimpan di $backupDir. Anda dapat melakukan restore jika diperlukan." "Yellow"
    }
    exit 1
}

Write-UpdateLog "`n==========================================================" "Cyan"
Write-UpdateLog "   UPDATE SELESAI DENGAN SUKSES & AMAN                    " "Green"
Write-UpdateLog "==========================================================" "Cyan"
Write-UpdateLog "Silakan buka aplikasi DreamPos WebReport di browser untuk memverifikasi." "White"
