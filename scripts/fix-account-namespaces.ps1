# Script to fix Account entity namespaces
# Updates namespace from Bank.Domain.Entities to Bank.Domain.Entities.Account

param(
    [switch]$DryRun,
    [switch]$Verbose
)

$ErrorActionPreference = "Continue"
$repoRoot = Split-Path -Parent $PSScriptRoot
$accountEntitiesPath = Join-Path $repoRoot "src\Bank.Domain\Entities\Account"

function Write-Log {
    param([string]$Message, [string]$Level = "INFO")
    $timestamp = Get-Date -Format "yyyy-MM-dd HH:mm:ss"
    $color = switch ($Level) {
        "ERROR" { "Red" }
        "WARNING" { "Yellow" }
        "SUCCESS" { "Green" }
        default { "White" }
    }
    Write-Host "[$timestamp] [$Level] $Message" -ForegroundColor $color
}

function Fix-EntityNamespace {
    param([string]$FilePath)
    
    $fileName = Split-Path $FilePath -Leaf
    
    if ($Verbose) { Write-Log "Processing $fileName..." }
    
    if (-not $DryRun) {
        $content = Get-Content $FilePath -Raw
        $originalContent = $content
        
        # Update namespace
        $content = $content -replace 'namespace Bank\.Domain\.Entities;', 'namespace Bank.Domain.Entities.Account;'
        
        if ($content -ne $originalContent) {
            Set-Content -Path $FilePath -Value $content -NoNewline
            Write-Log "Updated namespace in $fileName" "SUCCESS"
        } else {
            Write-Log "No changes needed for $fileName" "INFO"
        }
    } else {
        Write-Log "[DRY RUN] Would update namespace in $fileName" "INFO"
    }
}

Write-Log "Starting Account entity namespace fix..." "INFO"
Write-Log "Account entities path: $accountEntitiesPath" "INFO"

if ($DryRun) {
    Write-Log "DRY RUN MODE - No files will be modified" "WARNING"
}

# Get all .cs files in Account entities folder
$entityFiles = Get-ChildItem -Path $accountEntitiesPath -Filter "*.cs"

foreach ($file in $entityFiles) {
    Fix-EntityNamespace -FilePath $file.FullName
}

Write-Log "Account entity namespace fix complete!" "SUCCESS"

if ($DryRun) {
    Write-Log "DRY RUN COMPLETE - No changes were made" "WARNING"
}
