$assemblyPath = "d:\PCL\DreamPos-WebReport_Inventory_type12Jun2024\DreamPos-WebReport\bin\MySql.Data.dll"
[Reflection.Assembly]::LoadFile($assemblyPath) | Out-Null

$connStr = "Server=localhost;port=3307;user id=root;password=t4v14n4ul1;database=cldmemberbarburger7; Connection Timeout=3000; default command timeout=2400;"
$conn = New-Object MySql.Data.MySqlClient.MySqlConnection($connStr)
$conn.Open()

$cmd = $conn.CreateCommand()
$cmd.CommandText = "SHOW TABLES;"
$reader = $cmd.ExecuteReader()

$tables = @()
while ($reader.Read()) {
    $tables += $reader.GetString(0)
}
$reader.Close()

$schema = @{}

foreach ($table in $tables) {
    $cmd.CommandText = "DESCRIBE $table;"
    $reader = $cmd.ExecuteReader()
    $columns = @()
    while ($reader.Read()) {
        $columns += "$($reader.GetString(0)) $($reader.GetString(1))"
    }
    $reader.Close()
    $schema[$table] = $columns
}

$conn.Close()

$schema | ConvertTo-Json -Depth 3 > "d:\PCL\DreamPos-WebReport_Inventory_type12Jun2024\DreamPos-WebReport\schema.json"
Write-Output "Schema dumped to schema.json"
