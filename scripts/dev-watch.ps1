#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Start the application in watch mode with hot reload
.DESCRIPTION
    Runs the application with dotnet watch for automatic recompilation on file changes
#>

$ErrorActionPreference = "Stop"

Write-Host "🔥 Starting ResourceIdea in watch mode (hot reload enabled)..." -ForegroundColor Cyan
Write-Host ""
Write-Host "The application will automatically reload when you save changes to .cs or .razor files" -ForegroundColor Yellow
Write-Host ""

# Ensure we're in the correct directory
$serverPath = Join-Path $PSScriptRoot ".." "src" "EastSeat.ResourceIdea.Server"

if (-not (Test-Path $serverPath)) {
    Write-Host "❌ Server project not found at: $serverPath" -ForegroundColor Red
    exit 1
}

Push-Location $serverPath
try {
    dotnet watch run
} finally {
    Pop-Location
}
