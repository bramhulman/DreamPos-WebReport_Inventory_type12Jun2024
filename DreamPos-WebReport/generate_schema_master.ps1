Param(
    [string]$projectDir = $PSScriptRoot,
    [string]$outputJson = ""
)

if ([string]::IsNullOrWhiteSpace($projectDir)) {
    $projectDir = (Get-Location).Path
}

if ([string]::IsNullOrWhiteSpace($outputJson)) {
    $outputJson = Join-Path $projectDir "Pathdata\schema_master.json"
}

Write-Host "==========================================================" -ForegroundColor Cyan
Write-Host "   DREAMPOS WEBREPORT - GENERATE SCHEMA MASTER DICTIONARY " -ForegroundColor Cyan
Write-Host "==========================================================" -ForegroundColor Cyan

# 1. Baca koneksi dari Web.config (ServerStr)
$webConfigPath = Join-Path $projectDir "Web.config"
if (-not (Test-Path $webConfigPath)) {
    Write-Error "Web.config tidak ditemukan di $webConfigPath"
    exit 1
}

[xml]$xml = Get-Content $webConfigPath
$settingNode = $xml.SelectSingleNode("//setting[@name='ServerStr']/value")
if ($settingNode -eq $null -or [string]::IsNullOrEmpty($settingNode.InnerText)) {
    Write-Error "Pengaturan 'ServerStr' tidak ditemukan di Web.config"
    exit 1
}

$connStr = $settingNode.InnerText.Trim()
Write-Host "-> Koneksi dibaca dari Web.config (ServerStr)." -ForegroundColor Green

# 2. Muat MySql.Data.dll
$mysqlDll = Join-Path $projectDir "bin\MySql.Data.dll"
if (-not (Test-Path $mysqlDll)) {
    $mysqlDll = Join-Path $projectDir "Dependencies\MySql.Data.dll"
}
if (-not (Test-Path $mysqlDll)) {
    Write-Error "MySql.Data.dll tidak ditemukan di folder bin atau Dependencies."
    exit 1
}
[System.Reflection.Assembly]::LoadFrom($mysqlDll) | Out-Null

# 3. Ambil Nama Database dari Connection String
$matchDb = [regex]::Match($connStr, "database=([^;]+)", [System.Text.RegularExpressions.RegexOptions]::IgnoreCase)
$dbName = if ($matchDb.Success) { $matchDb.Groups[1].Value.Trim() } else { "dbqubepos" }

Write-Host "-> Mengekstrak skema database: '$dbName'..." -ForegroundColor Yellow

$conn = New-Object MySql.Data.MySqlClient.MySqlConnection($connStr)
try {
    $conn.Open()
    Write-Host "-> Koneksi ke MySQL berhasil dibuka!" -ForegroundColor Green

    # Query daftar tabel dasar (BASE TABLE)
    $cmd = $conn.CreateCommand()
    $cmd.CommandText = @"
SELECT TABLE_NAME 
FROM information_schema.TABLES 
WHERE TABLE_SCHEMA = @db AND TABLE_TYPE = 'BASE TABLE'
ORDER BY TABLE_NAME
"@
    $cmd.Parameters.AddWithValue("@db", $dbName) | Out-Null
    $rdr = $cmd.ExecuteReader()
    $tables = New-Object System.Collections.Generic.List[string]
    while ($rdr.Read()) {
        $tables.Add($rdr.GetString(0))
    }
    $rdr.Close()

    Write-Host "-> Ditemukan $($tables.Count) tabel di database $dbName." -ForegroundColor Green

    # Struktur master schema
    $schemaDict = [ordered]@{
        "generated_at" = (Get-Date).ToString("yyyy-MM-dd HH:mm:ss")
        "database"     = $dbName
        "tables"       = [ordered]@{}
    }

    # Query kolom untuk setiap tabel
    foreach ($tbl in $tables) {
        $colCmd = $conn.CreateCommand()
        $colCmd.CommandText = @"
SELECT 
    COLUMN_NAME, 
    COLUMN_TYPE, 
    IS_NULLABLE, 
    COLUMN_DEFAULT, 
    COLUMN_KEY, 
    EXTRA 
FROM information_schema.COLUMNS 
WHERE TABLE_SCHEMA = @db AND TABLE_NAME = @tbl 
ORDER BY ORDINAL_POSITION
"@
        $colCmd.Parameters.AddWithValue("@db", $dbName) | Out-Null
        $colCmd.Parameters.AddWithValue("@tbl", $tbl) | Out-Null

        $cRdr = $colCmd.ExecuteReader()
        $cols = [ordered]@{}
        $pks = New-Object System.Collections.Generic.List[string]

        while ($cRdr.Read()) {
            $colName = $cRdr["COLUMN_NAME"].ToString()
            $colType = $cRdr["COLUMN_TYPE"].ToString()
            $isNullable = $cRdr["IS_NULLABLE"].ToString()
            $colDefault = if ($cRdr["COLUMN_DEFAULT"] -ne [DBNull]::Value) { $cRdr["COLUMN_DEFAULT"].ToString() } else { $null }
            $colKey = $cRdr["COLUMN_KEY"].ToString()
            $extra = $cRdr["EXTRA"].ToString()

            if ($colKey -eq "PRI") {
                $pks.Add($colName)
            }

            $def = $colType
            if ($isNullable -eq "NO") {
                $def += " NOT NULL"
            } else {
                $def += " NULL"
            }

            if ($colDefault -ne $null) {
                if ($colDefault -eq "CURRENT_TIMESTAMP") {
                    $def += " DEFAULT CURRENT_TIMESTAMP"
                } else {
                    $def += " DEFAULT '$colDefault'"
                }
            } elseif ($isNullable -eq "YES") {
                $def += " DEFAULT NULL"
            }

            if ($extra -match "auto_increment") {
                $def += " AUTO_INCREMENT"
            }

            $cols[$colName] = [ordered]@{
                "type"       = $colType
                "nullable"   = ($isNullable -eq "YES")
                "default"    = $colDefault
                "definition" = $def
            }
        }
        $cRdr.Close()

        $schemaDict["tables"][$tbl] = [ordered]@{
            "primary_key" = $pks
            "columns"     = $cols
        }
    }

    # Buat direktori jika belum ada
    $outDir = [System.IO.Path]::GetDirectoryName($outputJson)
    if (-not (Test-Path $outDir)) {
        New-Item -ItemType Directory -Path $outDir -Force | Out-Null
    }

    # Simpan ke JSON
    $json = $schemaDict | ConvertTo-Json -Depth 5
    [System.IO.File]::WriteAllText($outputJson, $json, [System.Text.Encoding]::UTF8)

    Write-Host "-> Skema berhasil disimpan ke: $outputJson" -ForegroundColor Green
    Write-Host "-> Total tabel tercatat: $($schemaDict.tables.Count)" -ForegroundColor Green
} catch {
    Write-Warning "Gagal mengambil skema live: $($_.Exception.Message)"
} finally {
    if ($conn.State -eq [System.Data.ConnectionState]::Open) {
        $conn.Close()
    }
}
