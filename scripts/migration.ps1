#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Manage EF Core migrations
.PARAMETER Action
    Action to perform: add, remove, list, update
.PARAMETER Name
    Migration name (for add action)
#>

param(
    [Parameter(Mandatory=$true)]
    [ValidateSet('add', 'remove', 'list', 'update')]
    [string]$Action,
    
    [Parameter(Mandatory=$false)]
    [string]$Name
)

$ErrorActionPreference = "Stop"

$infraProject = "src/EastSeat.ResourceIdea.Infrastructure"
$serverProject = "src/EastSeat.ResourceIdea.Server"

Push-Location $infraProject

try {
    switch ($Action) {
        'add' {
            if (-not $Name) {
                Write-Host "❌ Migration name is required for 'add' action" -ForegroundColor Red
                Write-Host "Usage: .\migration.ps1 -Action add -Name MigrationName" -ForegroundColor Yellow
                exit 1
            }
            Write-Host "➕ Creating migration: $Name" -ForegroundColor Cyan
            dotnet ef migrations add $Name --startup-project ../$serverProject --context ApplicationDbContext
            Write-Host "✅ Migration created successfully" -ForegroundColor Green
        }
        'remove' {
            Write-Host "➖ Removing last migration..." -ForegroundColor Cyan
            dotnet ef migrations remove --startup-project ../$serverProject --context ApplicationDbContext
            Write-Host "✅ Migration removed successfully" -ForegroundColor Green
        }
        'list' {
            Write-Host "📋 Listing migrations..." -ForegroundColor Cyan
            dotnet ef migrations list --startup-project ../$serverProject --context ApplicationDbContext
        }
        'update' {
            Write-Host "🔄 Updating database..." -ForegroundColor Cyan
            dotnet ef database update --startup-project ../$serverProject --context ApplicationDbContext
            Write-Host "✅ Database updated successfully" -ForegroundColor Green
        }
    }
} finally {
    Pop-Location
}
