Param(
    [string]$projectDir = $PSScriptRoot
)

if ([string]::IsNullOrWhiteSpace($projectDir)) {
    $projectDir = (Get-Location).Path
}

$assemblyInfoPath = Join-Path $projectDir "My Project\AssemblyInfo.vb"

if (-not (Test-Path $assemblyInfoPath)) {
    Write-Warning "AssemblyInfo.vb tidak ditemukan di $assemblyInfoPath"
    exit 0
}

# 1. Ambil waktu saat ini
$now = Get-Date

# 2. Tanggal format DDMMYYYY
$day = $now.ToString("dd")
$month = $now.ToString("MM")
$year = $now.ToString("yyyy")

# 3. Hitung Obfuscation Jam & Menit
# Total Menit dari jam 00:00 (0 s/d 1439)
$totalMinutes = ($now.Hour * 60) + $now.Minute

# Tambahkan Salt / Pengacak tetap 1234
$salt = 1234
$obfuscatedCode = $totalMinutes + $salt

# 4. Susun Versi standard .NET: Major.Minor.Build.Revision (misal: 26.11.11.2155)
$major = $now.ToString("yy") # 2 digit tahun, misal 26
$minor = [int]$month         # bulan
$build = [int]$day           # hari
$revision = $obfuscatedCode  # kode menit + salt

$versionStr = "$major.$minor.$build.$revision"

Write-Host "Updating AssemblyInfo to version: $versionStr (Time: $($now.ToString('HH:mm')), TotalMin: $totalMinutes)" -ForegroundColor Green

# 5. Baca dan ganti isi AssemblyInfo.vb
$content = Get-Content -Path $assemblyInfoPath -Raw
$content = [System.Text.RegularExpressions.Regex]::Replace($content, '<Assembly:\s*AssemblyVersion\("[^"]*"\)>', "<Assembly: AssemblyVersion(`"$versionStr`")>")
$content = [System.Text.RegularExpressions.Regex]::Replace($content, '<Assembly:\s*AssemblyFileVersion\("[^"]*"\)>', "<Assembly: AssemblyFileVersion(`"$versionStr`")>")

Set-Content -Path $assemblyInfoPath -Value $content -Encoding UTF8
Write-Host "AssemblyInfo.vb updated successfully!" -ForegroundColor Green
