# Azure SQL Database Migration Script
# This script demonstrates how to use the EastSeat.ResourceIdea.Migration tool

# Set configuration variables
$env:MIGRATION_KeyVault__VaultUri = "https://your-keyvault.vault.azure.net/"
$env:MIGRATION_Migration__BatchSize = "1000"
$env:MIGRATION_Migration__CommandTimeoutSeconds = "300"
$env:MIGRATION_Migration__MaxRetryAttempts = "3"
$env:MIGRATION_Migration__EnableDetailedLogging = "true"

# Build the migration application
Write-Host "Building migration application..." -ForegroundColor Green
dotnet build src/dev/Infrastructure/EastSeat.ResourceIdea.Migration/EastSeat.ResourceIdea.Migration.csproj

# Show help
Write-Host "Migration tool help:" -ForegroundColor Green
dotnet run --project src/dev/Infrastructure/EastSeat.ResourceIdea.Migration/EastSeat.ResourceIdea.Migration.csproj -- --help

# Example commands
Write-Host "To migrate all tables, run:" -ForegroundColor Yellow
Write-Host "dotnet run --project src/dev/Infrastructure/EastSeat.ResourceIdea.Migration/EastSeat.ResourceIdea.Migration.csproj -- migrate-all" -ForegroundColor Cyan

Write-Host "To migrate a specific table, run:" -ForegroundColor Yellow
Write-Host "dotnet run --project src/dev/Infrastructure/EastSeat.ResourceIdea.Migration/EastSeat.ResourceIdea.Migration.csproj -- migrate-table `"TableName`"" -ForegroundColor Cyan

Write-Host "Note: Ensure your Azure Key Vault contains the following secrets:" -ForegroundColor Red
Write-Host "  - source-database-connection-string" -ForegroundColor Red
Write-Host "  - destination-database-connection-string" -ForegroundColor Red
Write-Host "And that you have appropriate Azure permissions configured." -ForegroundColor Red
