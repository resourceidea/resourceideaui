#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Run all tests with optional coverage
.PARAMETER Coverage
    Generate code coverage report
.PARAMETER Watch
    Run tests in watch mode
#>

param(
    [switch]$Coverage,
    [switch]$Watch
)

$ErrorActionPreference = "Stop"

Write-Host "🧪 Running tests..." -ForegroundColor Cyan

$solutionPath = Join-Path $PSScriptRoot ".." "EastSeat.ResourceIdea.slnx"

if ($Watch) {
    Write-Host "Running in watch mode. Press Ctrl+C to stop." -ForegroundColor Yellow
    dotnet watch test $solutionPath --verbosity normal
} elseif ($Coverage) {
    Write-Host "Generating coverage report..." -ForegroundColor Yellow
    dotnet test $solutionPath `
        --collect:"XPlat Code Coverage" `
        --results-directory:"./coverage" `
        --verbosity normal
    
    Write-Host "✅ Coverage report generated in ./coverage directory" -ForegroundColor Green
} else {
    dotnet test $solutionPath --verbosity normal
}

if ($LASTEXITCODE -eq 0) {
    Write-Host "✅ All tests passed!" -ForegroundColor Green
} else {
    Write-Host "❌ Some tests failed!" -ForegroundColor Red
    exit 1
}
