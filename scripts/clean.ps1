#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Clean build artifacts and reset development environment
.PARAMETER All
    Clean everything including Docker volumes
#>

param(
    [switch]$All
)

$ErrorActionPreference = "Stop"

Write-Host "🧹 Cleaning build artifacts..." -ForegroundColor Cyan

# Clean solution manifest (using .slnx)
dotnet clean "../EastSeat.ResourceIdea.slnx"

# Remove bin and obj directories
Get-ChildItem -Path ".." -Include bin,obj -Recurse -Directory | ForEach-Object {
    Write-Host "Removing: $($_.FullName)" -ForegroundColor Gray
    Remove-Item $_.FullName -Recurse -Force
}

# Remove test results
$coveragePath = Join-Path $PSScriptRoot ".." "coverage"
if (Test-Path $coveragePath) {
    Write-Host "Removing coverage reports..." -ForegroundColor Gray
    Remove-Item $coveragePath -Recurse -Force
}

$testResultsPath = Join-Path $PSScriptRoot ".." "TestResults"
if (Test-Path $testResultsPath) {
    Write-Host "Removing test results..." -ForegroundColor Gray
    Remove-Item $testResultsPath -Recurse -Force
}

if ($All) {
    Write-Host "🗑️  Performing deep clean..." -ForegroundColor Yellow
    
    # Stop and remove Docker containers
    Push-Location (Join-Path $PSScriptRoot "..")
    try {
        Write-Host "Stopping Docker containers..." -ForegroundColor Gray
        docker-compose down -v
        Write-Host "✅ Docker containers and volumes removed" -ForegroundColor Green
    } catch {
        Write-Host "⚠️  Failed to remove Docker containers (may not be running)" -ForegroundColor Yellow
    } finally {
        Pop-Location
    }
}

Write-Host "✅ Clean completed!" -ForegroundColor Green
