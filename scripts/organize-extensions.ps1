# Script to organize extension files into logical subfolders
# This script moves extension files from flat structure to organized structure

param(
    [switch]$DryRun,
    [switch]$Verbose
)

$ErrorActionPreference = "Continue"
$repoRoot = Split-Path -Parent $PSScriptRoot
$extensionsPath = Join-Path $repoRoot "src\Bank.Api\Extensions"

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

function Ensure-Directory {
    param([string]$Path)
    if (-not (Test-Path $Path)) {
        if ($Verbose) { Write-Log "Creating directory: $Path" }
        if (-not $DryRun) {
            New-Item -ItemType Directory -Path $Path -Force | Out-Null
        }
    }
}

function Move-ExtensionFile {
    param(
        [string]$FileName,
        [string]$Subfolder
    )
    
    $sourcePath = Join-Path $extensionsPath $FileName
    $targetDir = Join-Path $extensionsPath $Subfolder
    $targetPath = Join-Path $targetDir $FileName
    
    if (Test-Path $sourcePath) {
        if ($Verbose) { Write-Log "Moving $FileName to $Subfolder/" }
        
        Ensure-Directory $targetDir
        
        if (-not $DryRun) {
            Move-Item -Path $sourcePath -Destination $targetPath -Force
            
            # Update namespace in the moved file
            $content = Get-Content $targetPath -Raw
            $content = $content -replace 'namespace Bank\.Api\.Extensions;', "namespace Bank.Api.Extensions.$Subfolder;"
            Set-Content -Path $targetPath -Value $content -NoNewline
            
            Write-Log "Moved and updated namespace: $FileName -> $Subfolder/" "SUCCESS"
        } else {
            Write-Log "[DRY RUN] Would move: $FileName -> $Subfolder/" "INFO"
        }
    } else {
        Write-Log "File not found (skipping): $FileName" "WARNING"
    }
}

# Organize extensions by purpose
# DependencyInjection - Service registration extensions
$dependencyInjectionExtensions = @(
    "ApplicationServiceExtensions.cs",
    "RepositoryServiceExtensions.cs",
    "InfrastructureServiceExtensions.cs",
    "BackgroundJobServiceExtensions.cs",
    "CqrsServiceExtensions.cs"
)

# Infrastructure - Database, caching, authentication
$infrastructureExtensions = @(
    "DatabaseServiceExtensions.cs",
    "CachingServiceExtensions.cs",
    "AuthenticationServiceExtensions.cs"
)

# Configuration - API configuration (CORS, documentation)
$configurationExtensions = @(
    "ApiDocumentationServiceExtensions.cs",
    "CorsServiceExtensions.cs"
)

# Middleware - Middleware pipeline extensions
$middlewareExtensions = @(
    "MiddlewareExtensions.cs"
)

# Data - Data seeding and initialization
$dataExtensions = @(
    "DataSeedingExtensions.cs"
)

Write-Log "Starting extensions organization..." "INFO"
Write-Log "Extensions path: $extensionsPath" "INFO"

if ($DryRun) {
    Write-Log "DRY RUN MODE - No files will be moved" "WARNING"
}

# Move DependencyInjection extensions
Write-Log "Processing DependencyInjection extensions..." "INFO"
foreach ($file in $dependencyInjectionExtensions) {
    Move-ExtensionFile -FileName $file -Subfolder "DependencyInjection"
}

# Move Infrastructure extensions
Write-Log "Processing Infrastructure extensions..." "INFO"
foreach ($file in $infrastructureExtensions) {
    Move-ExtensionFile -FileName $file -Subfolder "Infrastructure"
}

# Move Configuration extensions
Write-Log "Processing Configuration extensions..." "INFO"
foreach ($file in $configurationExtensions) {
    Move-ExtensionFile -FileName $file -Subfolder "Configuration"
}

# Move Middleware extensions
Write-Log "Processing Middleware extensions..." "INFO"
foreach ($file in $middlewareExtensions) {
    Move-ExtensionFile -FileName $file -Subfolder "Middleware"
}

# Move Data extensions
Write-Log "Processing Data extensions..." "INFO"
foreach ($file in $dataExtensions) {
    Move-ExtensionFile -FileName $file -Subfolder "Data"
}

Write-Log "Extensions organization complete!" "SUCCESS"
Write-Log "Note: ServiceCollectionExtensions.cs remains at root level (main orchestrator)" "INFO"

if ($DryRun) {
    Write-Log "DRY RUN COMPLETE - No changes were made" "WARNING"
}
