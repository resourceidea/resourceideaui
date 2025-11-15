#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Setup and start the EastSeat.ResourceIdea application
.DESCRIPTION
    This script configures, builds, and runs the ResourceIdea application.
    It handles database setup, migrations, and application startup.
.PARAMETER SkipDocker
    Skip starting Docker containers (use existing database)
.PARAMETER SkipMigrations
    Skip running database migrations
.PARAMETER SkipBuild
    Skip building the solution
.PARAMETER Production
    Run in production mode
.EXAMPLE
    .\setup.ps1
    .\setup.ps1 -SkipDocker
    .\setup.ps1 -Production
#>

param(
    [switch]$SkipDocker,
    [switch]$SkipMigrations,
    [switch]$SkipBuild,
    [switch]$Production
)

$ErrorActionPreference = "Stop"
$ProgressPreference = "SilentlyContinue"

# Colors for output
function Write-Info { Write-Host "ℹ️  $args" -ForegroundColor Cyan }
function Write-Success { Write-Host "✅ $args" -ForegroundColor Green }
function Write-Error { Write-Host "❌ $args" -ForegroundColor Red }
function Write-Warning { Write-Host "⚠️  $args" -ForegroundColor Yellow }

Write-Host @"
╔════════════════════════════════════════════════════════════╗
║         EastSeat ResourceIdea - Setup Script               ║
║                                                            ║
║  Resource Planning for Audit & Tax Advisory Firms          ║
╚════════════════════════════════════════════════════════════╝

"@ -ForegroundColor Cyan

# Check prerequisites
Write-Info "Checking prerequisites..."

# Check .NET SDK
try {
    $dotnetVersion = dotnet --version
    Write-Success ".NET SDK found: $dotnetVersion"
} catch {
    Write-Error ".NET SDK not found. Please install .NET 10 SDK from https://dotnet.microsoft.com/download"
    exit 1
}

# Check Docker (unless skipped)
if (-not $SkipDocker) {
    try {
        $dockerVersion = docker --version
        Write-Success "Docker found: $dockerVersion"
    } catch {
        Write-Warning "Docker not found. Use -SkipDocker if you have an existing PostgreSQL instance."
        $response = Read-Host "Continue without Docker? (y/n)"
        if ($response -ne 'y') {
            exit 1
        }
        $SkipDocker = $true
    }
}

# Set environment
$env:ASPNETCORE_ENVIRONMENT = if ($Production) { "Production" } else { "Development" }
Write-Info "Environment: $($env:ASPNETCORE_ENVIRONMENT)"

# Load environment variables from .env if exists
$envFile = Join-Path $PSScriptRoot ".env"
if (Test-Path $envFile) {
    Write-Info "Loading environment variables from .env file..."
    Get-Content $envFile | ForEach-Object {
        if ($_ -match '^([^=]+)=(.*)$') {
            $key = $matches[1].Trim()
            $value = $matches[2].Trim()
            if ($key -and -not $key.StartsWith('#')) {
                [Environment]::SetEnvironmentVariable($key, $value, "Process")
                Write-Success "Loaded: $key"
            }
        }
    }
} else {
    Write-Warning ".env file not found. Creating from .env.sample..."
    if (Test-Path ".env.sample") {
        Copy-Item ".env.sample" ".env"
        Write-Success "Created .env file. Please review and update the configuration."
        Write-Warning "Default password will be used for admin account: Admin@123"
    } else {
        Write-Error ".env.sample not found!"
        exit 1
    }
}

# Start Docker containers
if (-not $SkipDocker) {
    Write-Info "Starting PostgreSQL container..."
    try {
        docker-compose up -d
        Write-Success "Docker containers started"
        
        # Wait for PostgreSQL to be ready
        Write-Info "Waiting for PostgreSQL to be ready..."
        $maxAttempts = 30
        $attempt = 0
        $dbReady = $false
        
        while ($attempt -lt $maxAttempts -and -not $dbReady) {
            $attempt++
            try {
                docker-compose exec -T postgres pg_isready -U postgres | Out-Null
                $dbReady = $true
                Write-Success "PostgreSQL is ready!"
            } catch {
                Write-Host "." -NoNewline
                Start-Sleep -Seconds 2
            }
        }
        
        if (-not $dbReady) {
            Write-Error "PostgreSQL failed to start within expected time"
            exit 1
        }
    } catch {
        Write-Error "Failed to start Docker containers: $_"
        exit 1
    }
}

# Restore and build solution
if (-not $SkipBuild) {
    $manifest = Join-Path $PSScriptRoot "EastSeat.ResourceIdea.slnx"
    $solutionFile = Join-Path $PSScriptRoot "EastSeat.ResourceIdea.slnx"
    if (-not (Test-Path $manifest)) {
        Write-Error "XML manifest '$manifest' not found. Cannot generate solution."
        exit 1
    }
    Write-Info "Using .slnx manifest directly (generation of .sln skipped)..."
    try {
        # Generation step removed per migration to .slnx-only usage
        Write-Success "Manifest ready: $solutionFile"
    } catch {
        Write-Error "Generation failed: $_"
        exit 1
    }

    Write-Info "Restoring NuGet packages..."
    try {
        dotnet restore $solutionFile
        Write-Success "Packages restored"
    } catch {
        Write-Error "Failed to restore packages: $_"
        exit 1
    }

    Write-Info "Building solution..."
    try {
        $buildConfig = if ($Production) { "Release" } else { "Debug" }
        dotnet build $solutionFile --configuration $buildConfig --no-restore
        Write-Success "Build completed"
    } catch {
        Write-Error "Build failed: $_"
        exit 1
    }
}

# Run database migrations
if (-not $SkipMigrations) {
    Write-Info "Checking for EF Core tools..."
    
    # Check if migrations exist
    $migrationsPath = "src/EastSeat.ResourceIdea.Infrastructure/Migrations"
    if (-not (Test-Path $migrationsPath) -or (Get-ChildItem $migrationsPath -ErrorAction SilentlyContinue).Count -eq 0) {
        Write-Info "Creating initial migration..."
        try {
            Push-Location "src/EastSeat.ResourceIdea.Infrastructure"
            dotnet ef migrations add InitialCreate --startup-project ../EastSeat.ResourceIdea.Server --context ApplicationDbContext
            Pop-Location
            Write-Success "Initial migration created"
        } catch {
            Pop-Location
            Write-Error "Failed to create migration: $_"
            exit 1
        }
    } else {
        Write-Info "Migrations already exist"
    }

    Write-Info "Applying database migrations..."
    try {
        Push-Location "src/EastSeat.ResourceIdea.Infrastructure"
        dotnet ef database update --startup-project ../EastSeat.ResourceIdea.Server --context ApplicationDbContext
        Pop-Location
        Write-Success "Database updated"
    } catch {
        Pop-Location
        Write-Error "Migration failed: $_"
        Write-Warning "If database doesn't exist, ensure PostgreSQL is running and connection string is correct."
        exit 1
    }
}

# Run tests
Write-Info "Running tests..."
try {
    dotnet test $solutionFile --no-build --verbosity quiet
    Write-Success "All tests passed"
} catch {
    Write-Warning "Some tests failed. Check output above for details."
}

# Start application
Write-Host @"

╔════════════════════════════════════════════════════════════╗
║                   Starting Application                     ║
╚════════════════════════════════════════════════════════════╝

"@ -ForegroundColor Green

Write-Info "Starting ResourceIdea..."
Write-Host ""
Write-Host "Application will be available at:" -ForegroundColor Cyan
Write-Host "  • https://localhost:7001" -ForegroundColor Yellow
Write-Host "  • http://localhost:5001" -ForegroundColor Yellow
Write-Host ""
Write-Host "Default admin credentials:" -ForegroundColor Cyan
Write-Host "  • Email: admin@eastseat.com" -ForegroundColor Yellow
Write-Host "  • Password: Admin@123 (or from ADMIN_PASSWORD env)" -ForegroundColor Yellow
Write-Host ""
Write-Host "Press Ctrl+C to stop the application" -ForegroundColor Gray
Write-Host ""

try {
    Push-Location "src/EastSeat.ResourceIdea.Server"
    if ($Production) {
        dotnet run --configuration Release --no-build
    } else {
        dotnet run --configuration Debug --no-build
    }
} catch {
    Write-Error "Failed to start application: $_"
    exit 1
} finally {
    Pop-Location
}
