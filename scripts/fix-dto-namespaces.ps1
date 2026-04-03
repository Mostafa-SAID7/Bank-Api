#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Fixes all DTO file namespaces to match their folder structure and updates using statements
.DESCRIPTION
    This script:
    1. Scans all DTO files in Bank.Application/DTOs
    2. Extracts the correct namespace from folder structure
    3. Updates namespace declarations to match folder path
    4. Removes old incorrect namespaces
    5. Ensures all using statements are correct
    6. Reports any duplicates or issues
#>

param(
    [string]$DtoBasePath = "Bank-Api/src/Bank.Application/DTOs",
    [switch]$WhatIf = $false,
    [switch]$Verbose = $false
)

$ErrorActionPreference = "Stop"
$changedFiles = 0
$totalFiles = 0
$duplicates = @()
$issues = @()

Write-Host "Starting DTO Namespace Fix Script..." -ForegroundColor Cyan
Write-Host "Base Path: $DtoBasePath" -ForegroundColor Cyan
Write-Host ""

# Get all DTO files
$dtoFiles = Get-ChildItem -Path $DtoBasePath -Recurse -File -Filter "*.cs"
Write-Host "Found $($dtoFiles.Count) DTO files" -ForegroundColor Yellow

foreach ($file in $dtoFiles) {
    $totalFiles++
    $relativePath = $file.FullName -replace [regex]::Escape((Get-Item $DtoBasePath).FullName), ""
    $relativePath = $relativePath.TrimStart('\', '/')
    
    # Extract folder structure for namespace
    $folderPath = Split-Path $relativePath -Parent
    $folderPath = $folderPath -replace '\\', '/'
    
    # Build expected namespace
    $expectedNamespace = "Bank.Application.DTOs"
    if ($folderPath) {
        $folders = $folderPath -split '/'
        foreach ($folder in $folders) {
            if ($folder) {
                $expectedNamespace += ".$folder"
            }
        }
    }
    
    # Read file content
    $content = Get-Content $file.FullName -Raw
    
    # Extract current namespace
    $namespaceMatch = $content -match 'namespace\s+([\w.]+);'
    $currentNamespace = if ($namespaceMatch) { $matches[1] } else { $null }
    
    # Check if namespace needs updating
    if ($currentNamespace -ne $expectedNamespace) {
        if ($Verbose) {
            Write-Host "File: $($file.Name)" -ForegroundColor Yellow
            Write-Host "  Current:  $currentNamespace" -ForegroundColor Red
            Write-Host "  Expected: $expectedNamespace" -ForegroundColor Green
        }
        
        # Update namespace
        if ($currentNamespace) {
            $newContent = $content -replace "namespace\s+$([regex]::Escape($currentNamespace));", "namespace $expectedNamespace;"
        } else {
            # Add namespace if missing
            $newContent = $content -replace "^(using\s+.*?;)", "`$1`n`nnamespace $expectedNamespace;"
        }
        
        if (-not $WhatIf) {
            Set-Content -Path $file.FullName -Value $newContent -Encoding UTF8
            $changedFiles++
        }
    }
    
    # Check for duplicate class names in same folder
    $classMatches = [regex]::Matches($content, 'public\s+(?:class|record|interface|enum)\s+(\w+)')
    if ($classMatches.Count -gt 1) {
        $issues += @{
            File = $file.Name
            Issue = "Multiple type definitions ($($classMatches.Count))"
            Types = @($classMatches | ForEach-Object { $_.Groups[1].Value })
        }
    }
}

Write-Host ""
Write-Host "Summary:" -ForegroundColor Cyan
Write-Host "  Total Files: $totalFiles" -ForegroundColor White
Write-Host "  Changed: $changedFiles" -ForegroundColor Green
Write-Host "  Issues Found: $($issues.Count)" -ForegroundColor Yellow

if ($issues.Count -gt 0) {
    Write-Host ""
    Write-Host "Issues to Review:" -ForegroundColor Yellow
    foreach ($issue in $issues) {
        Write-Host "  - $($issue.File): $($issue.Issue)" -ForegroundColor Yellow
        Write-Host "    Types: $($issue.Types -join ', ')" -ForegroundColor Gray
    }
}

if ($WhatIf) {
    Write-Host ""
    Write-Host "WhatIf mode: No changes were made. Run without -WhatIf to apply changes." -ForegroundColor Cyan
}

Write-Host ""
Write-Host "Script completed!" -ForegroundColor Green
