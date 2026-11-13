function Test-SchemaSqlFragment {
    param([string]$Text)
    $depth = 0
    $quote = [char]0
    for ($i = 0; $i -lt $Text.Length; $i++) {
        $c = $Text[$i]
        if ($quote -ne [char]0) {
            if ($c -eq [char]92) { $i++; continue }
            if ($c -eq $quote) {
                if ($i + 1 -lt $Text.Length -and $Text[$i + 1] -eq $quote) { $i++; continue }
                $quote = [char]0
            }
        } elseif ($c -eq [char]39 -or $c -eq [char]34 -or $c -eq [char]96) {
            $quote = $c
        } elseif ($c -eq '(') {
            $depth++
        } elseif ($c -eq ')') {
            $depth--
            if ($depth -lt 0) { return $false }
        } elseif ($c -eq ';') {
            return $false
        }
    }
    return ($depth -eq 0 -and $quote -eq [char]0)
}

function Test-MasterSchemaColumn {
    param([string]$Name, $Column)
    $columnType = [string]$Column.type
    $definition = [string]$Column.definition
    # A real column named FULLTEXT is allowed if its data type is a real MySQL type.
    if ($columnType -match '^(KEY|PRIMARY|UNIQUE|FULLTEXT|SPATIAL|CONSTRAINT|FOREIGN|CHECK)(\s|$)') { return $false }
    if ([string]::IsNullOrWhiteSpace($columnType) -or [string]::IsNullOrWhiteSpace($definition) -or
        -not (Test-SchemaSqlFragment $columnType) -or -not (Test-SchemaSqlFragment $definition) -or
        -not $definition.StartsWith($columnType, [StringComparison]::OrdinalIgnoreCase)) {
        throw "Skema master tidak valid pada '$Name' (type='$columnType'). Perbaiki schema_master.json; tidak ada DDL schema master yang dijalankan."
    }
    return $true
}
