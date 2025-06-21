#!/bin/bash

# Azure SQL Database Migration Script
# This script demonstrates how to use the EastSeat.ResourceIdea.Migration tool

# Set configuration variables
export MIGRATION_KeyVault__VaultUri="https://your-keyvault.vault.azure.net/"
export MIGRATION_Migration__BatchSize=1000
export MIGRATION_Migration__CommandTimeoutSeconds=300
export MIGRATION_Migration__MaxRetryAttempts=3
export MIGRATION_Migration__EnableDetailedLogging=true

# Build the migration application
echo "Building migration application..."
dotnet build src/Infrastructure/EastSeat.ResourceIdea.Migration/EastSeat.ResourceIdea.Migration.csproj

# Show help
echo "Migration tool help:"
dotnet run --project src/Infrastructure/EastSeat.ResourceIdea.Migration/EastSeat.ResourceIdea.Migration.csproj -- --help

# Example: Migrate all tables
echo "To migrate all tables, run:"
echo "dotnet run --project src/Infrastructure/EastSeat.ResourceIdea.Migration/EastSeat.ResourceIdea.Migration.csproj -- migrate-all"

# Example: Migrate specific table
echo "To migrate a specific table, run:"
echo "dotnet run --project src/Infrastructure/EastSeat.ResourceIdea.Migration/EastSeat.ResourceIdea.Migration.csproj -- migrate-table \"TableName\""

echo "Note: Ensure your Azure Key Vault contains the following secrets:"
echo "  - source-database-connection-string"
echo "  - destination-database-connection-string"
echo "And that you have appropriate Azure permissions configured."
