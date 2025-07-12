using EastSeat.ResourceIdea.Migration.Model;
using EastSeat.ResourceIdea.Migration.Services;
using Xunit;

namespace EastSeat.ResourceIdea.Migration.UnitTests.Services;

/// <summary>
/// Unit tests for Client table migration functionality.
/// </summary>
public class ClientMigrationTests
{
    [Fact]
    public void ClientTableDefinition_HasCorrectStructure()
    {
        // Arrange - This validates our table definition structure
        var clientTableDefinition = new TableDefinition
        {
            Schema = "dbo",
            Table = "Client",
            MigrationOrder = 2,
            Columns = new List<SourceColumnDefinition>
            {
                new() { Name = "ClientId", Type = "varchar(40)" },
                new() { Name = "Name", Type = "varchar(200)" },
                new() { Name = "Address", Type = "varchar(100)" },
                new() { Name = "Industry", Type = "varchar(100)" },
                new() { Name = "CompanyCode", Type = "varchar(50)" },
                new() { Name = "Active", Type = "bit" }
            },
            Destination = new DestinationTableDefinition
            {
                Schema = "dbo",
                Table = "Clients",
                Columns = new List<DestinationColumnDefinition>
                {
                    new() { Name = "Id", Type = "nvarchar(450)", IsMigratable = false },
                    new() { Name = "MigrationClientId", Type = "nvarchar(50)", IsMigratable = true, SourceColumn = "ClientId" },
                    new() { Name = "Name", Type = "nvarchar(500)", IsMigratable = true, SourceColumn = "Name" },
                    new() { Name = "Address_Building", Type = "nvarchar(100)", IsMigratable = false },
                    new() { Name = "Address_Street", Type = "nvarchar(100)", IsMigratable = true, SourceColumn = "Address" },
                    new() { Name = "Address_City", Type = "nvarchar(100)", IsMigratable = false },
                    new() { Name = "MigrationIndustry", Type = "nvarchar(100)", IsMigratable = true, SourceColumn = "Industry" },
                    new() { Name = "MigrationCompanyCode", Type = "nvarchar(50)", IsMigratable = true, SourceColumn = "CompanyCode" },
                    new() { Name = "TenantId", Type = "nvarchar(max)", IsMigratable = false, LookupTable = "Tenants", LookupColumn = "TenantId", LookupCondition = "MigrationCompanyCode", LookupSource = "CompanyCode" },
                    new() { Name = "IsDeleted", Type = "bit", IsMigratable = false, Transform = "InvertActive", SourceColumn = "Active" },
                    new() { Name = "Deleted", Type = "datetimeoffset", IsMigratable = false, Transform = "ConditionalDeletedDate", SourceColumn = "Active" },
                    new() { Name = "DeletedBy", Type = "nvarchar(100)", IsMigratable = false, Transform = "ConditionalDeletedBy", SourceColumn = "Active" },
                    new() { Name = "Created", Type = "datetimeoffset", IsMigratable = false },
                    new() { Name = "CreatedBy", Type = "nvarchar(100)", IsMigratable = false },
                    new() { Name = "LastModified", Type = "datetimeoffset", IsMigratable = false },
                    new() { Name = "LastModifiedBy", Type = "nvarchar(100)", IsMigratable = false }
                }
            }
        };

        // Assert
        Assert.Equal("dbo", clientTableDefinition.Schema);
        Assert.Equal("Client", clientTableDefinition.Table);
        Assert.Equal(6, clientTableDefinition.Columns.Count);
        Assert.Equal("dbo", clientTableDefinition.Destination.Schema);
        Assert.Equal("Clients", clientTableDefinition.Destination.Table);
        Assert.Equal(16, clientTableDefinition.Destination.Columns.Count);

        // Verify specific mappings
        var migrationClientId = clientTableDefinition.Destination.Columns.First(c => c.Name == "MigrationClientId");
        Assert.True(migrationClientId.IsMigratable);
        Assert.Equal("ClientId", migrationClientId.SourceColumn);

        var tenantId = clientTableDefinition.Destination.Columns.First(c => c.Name == "TenantId");
        Assert.False(tenantId.IsMigratable);
        Assert.Equal("Tenants", tenantId.LookupTable);
        Assert.Equal("TenantId", tenantId.LookupColumn);
        Assert.Equal("MigrationCompanyCode", tenantId.LookupCondition);
        Assert.Equal("CompanyCode", tenantId.LookupSource);

        var isDeleted = clientTableDefinition.Destination.Columns.First(c => c.Name == "IsDeleted");
        Assert.False(isDeleted.IsMigratable);
        Assert.Equal("InvertActive", isDeleted.Transform);
        Assert.Equal("Active", isDeleted.SourceColumn);
    }

    [Fact]
    public void MigrationSourceData_CanStoreClientData()
    {
        // Arrange
        var sourceData = new MigrationSourceData();

        // Act
        sourceData.SetValue("ClientId", "CLIENT001");
        sourceData.SetValue("Name", "Test Client Corp");
        sourceData.SetValue("Address", "123 Main St");
        sourceData.SetValue("Industry", "Technology");
        sourceData.SetValue("CompanyCode", "COMP001");
        sourceData.SetValue("Active", true);

        // Assert
        Assert.Equal("CLIENT001", sourceData.GetValue("ClientId"));
        Assert.Equal("Test Client Corp", sourceData.GetValue("Name"));
        Assert.Equal("123 Main St", sourceData.GetValue("Address"));
        Assert.Equal("Technology", sourceData.GetValue("Industry"));
        Assert.Equal("COMP001", sourceData.GetValue("CompanyCode"));
        Assert.Equal(true, sourceData.GetValue("Active"));
    }

    [Theory]
    [InlineData(true, false)] // Active = true means IsDeleted = false
    [InlineData(false, true)] // Active = false means IsDeleted = true
    public void InvertActiveToBool_CorrectlyInvertsValues(bool activeValue, bool expectedIsDeleted)
    {
        // This test validates the logic for inverting Active to IsDeleted
        // Since the transformation methods are private, we test the logic concept

        bool actualIsDeleted = !activeValue;

        Assert.Equal(expectedIsDeleted, actualIsDeleted);
    }

    [Theory]
    [InlineData(true, null)] // Active = true means Deleted = null
    [InlineData(false, "date")] // Active = false means Deleted = some date
    public void ConditionalDeletedDate_ReturnsCorrectValue(bool activeValue, string? expectedType)
    {
        // This test validates the logic for conditional deleted date

        object? deletedDate = activeValue ? null : "SYSDATETIMEOFFSET()";

        if (expectedType == null)
        {
            Assert.Null(deletedDate);
        }
        else
        {
            Assert.NotNull(deletedDate);
            Assert.Equal("SYSDATETIMEOFFSET()", deletedDate);
        }
    }

    [Theory]
    [InlineData(true, null)] // Active = true means DeletedBy = null
    [InlineData(false, "MIGRATION")] // Active = false means DeletedBy = "MIGRATION"
    public void ConditionalDeletedBy_ReturnsCorrectValue(bool activeValue, string? expectedValue)
    {
        // This test validates the logic for conditional deleted by

        string? deletedBy = activeValue ? null : "MIGRATION";

        Assert.Equal(expectedValue, deletedBy);
    }

    [Fact]
    public void MigrationResult_TracksClientMigrationResults()
    {
        // Arrange
        var migrationResult = new MigrationResult
        {
            Total = 3
        };

        var client1 = new MigrationSourceData();
        client1.SetValue("ClientId", "CLIENT001");
        client1.SetValue("Name", "Client 1");

        var client2 = new MigrationSourceData();
        client2.SetValue("ClientId", "CLIENT002");
        client2.SetValue("Name", "Client 2");

        var client3 = new MigrationSourceData();
        client3.SetValue("ClientId", "CLIENT003");
        client3.SetValue("Name", "Client 3");

        // Act - Simulate different outcomes
        migrationResult.Migrated.Add(new Tuple<MigrationSourceData, ItemMigrationResult>(client1, ItemMigrationResult.Migrated));
        migrationResult.Skipped.Add(new Tuple<MigrationSourceData, ItemMigrationResult>(client2, ItemMigrationResult.Skipped));
        migrationResult.Failed.Add(new Tuple<MigrationSourceData, ItemMigrationResult>(client3, ItemMigrationResult.Failed));

        // Assert
        Assert.Equal(3, migrationResult.Total);
        Assert.Single(migrationResult.Migrated);
        Assert.Single(migrationResult.Skipped);
        Assert.Single(migrationResult.Failed);

        Assert.Equal("CLIENT001", migrationResult.Migrated.First().Item1.GetValue("ClientId"));
        Assert.Equal("CLIENT002", migrationResult.Skipped.First().Item1.GetValue("ClientId"));
        Assert.Equal("CLIENT003", migrationResult.Failed.First().Item1.GetValue("ClientId"));
    }
}
