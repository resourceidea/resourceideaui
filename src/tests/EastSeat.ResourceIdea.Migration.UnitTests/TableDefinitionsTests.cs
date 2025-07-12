using EastSeat.ResourceIdea.Migration.Configuration;
using EastSeat.ResourceIdea.Migration.Model;
using Xunit;

namespace EastSeat.ResourceIdea.Migration.UnitTests;

/// <summary>
/// Unit tests for the TableDefinitions class.
/// </summary>
public class TableDefinitionsTests
{
    [Fact]
    public void TablesToMigrate_ShouldLoadCorrectNumberOfTables()
    {
        // Act
        var tables = TableDefinitions.TablesToMigrate;

        // Assert
        Assert.Equal(2, tables.Count); // Company and Client tables
    }

    [Fact]
    public void TablesToMigrate_ShouldContainExpectedTables()
    {
        // Arrange
        var expectedTableNames = new[]
        {
            "Company", "Client"
        };

        // Act
        var tables = TableDefinitions.TablesToMigrate;
        var actualTableNames = tables.Select(t => t.Table).ToHashSet();

        // Assert
        foreach (var expectedTableName in expectedTableNames)
        {
            Assert.Contains(expectedTableName, actualTableNames);
        }
    }

    [Fact]
    public void TablesToMigrate_CompanyTable_ShouldHaveCorrectStructure()
    {
        // Act
        var tables = TableDefinitions.TablesToMigrate;
        var companyTable = tables.FirstOrDefault(t => t.Table == "Company");

        // Assert
        Assert.NotNull(companyTable);
        Assert.Equal("dbo", companyTable.Schema);
        Assert.Equal(2, companyTable.Columns.Count);

        var companyCodeColumn = companyTable.Columns.FirstOrDefault(c => c.Name == "CompanyCode");
        Assert.NotNull(companyCodeColumn);
        Assert.Equal("varchar(50)", companyCodeColumn.Type);

        var organizationNameColumn = companyTable.Columns.FirstOrDefault(c => c.Name == "OrganizationName");
        Assert.NotNull(organizationNameColumn);
        Assert.Equal("varchar(256)", organizationNameColumn.Type);

        // Verify destination mapping
        Assert.Equal("dbo", companyTable.Destination.Schema);
        Assert.Equal("Tenants", companyTable.Destination.Table);
        Assert.Equal(10, companyTable.Destination.Columns.Count);
    }

    [Fact]
    public void TablesToMigrate_ClientTable_ShouldHaveCorrectStructure()
    {
        // Act
        var tables = TableDefinitions.TablesToMigrate;
        var clientTable = tables.FirstOrDefault(t => t.Table == "Client");

        // Assert
        Assert.NotNull(clientTable);
        Assert.Equal("dbo", clientTable.Schema);
        Assert.Equal(6, clientTable.Columns.Count);

        // Verify source columns
        var expectedSourceColumns = new[]
        {
            ("ClientId", "varchar(40)"),
            ("Name", "varchar(200)"),
            ("Address", "varchar(100)"),
            ("Industry", "varchar(100)"),
            ("CompanyCode", "varchar(50)"),
            ("Active", "bit")
        };

        foreach (var (name, type) in expectedSourceColumns)
        {
            var column = clientTable.Columns.FirstOrDefault(c => c.Name == name);
            Assert.NotNull(column);
            Assert.Equal(type, column.Type);
        }

        // Verify destination mapping
        Assert.Equal("dbo", clientTable.Destination.Schema);
        Assert.Equal("Clients", clientTable.Destination.Table);
        Assert.Equal(16, clientTable.Destination.Columns.Count);
    }

    [Fact]
    public void TablesToMigrate_ClientTable_ShouldHaveCorrectMappings()
    {
        // Act
        var tables = TableDefinitions.TablesToMigrate;
        var clientTable = tables.FirstOrDefault(t => t.Table == "Client");

        // Assert
        Assert.NotNull(clientTable);

        // Verify specific column mappings
        var migrationClientId = clientTable.Destination.Columns.FirstOrDefault(c => c.Name == "MigrationClientId");
        Assert.NotNull(migrationClientId);
        Assert.True(migrationClientId.IsMigratable);
        Assert.Equal("ClientId", migrationClientId.SourceColumn);

        var name = clientTable.Destination.Columns.FirstOrDefault(c => c.Name == "Name");
        Assert.NotNull(name);
        Assert.True(name.IsMigratable);
        Assert.Equal("Name", name.SourceColumn);

        var addressBuilding = clientTable.Destination.Columns.FirstOrDefault(c => c.Name == "Address_Building");
        Assert.NotNull(addressBuilding);
        Assert.False(addressBuilding.IsMigratable);
        Assert.Null(addressBuilding.SourceColumn);

        var addressStreet = clientTable.Destination.Columns.FirstOrDefault(c => c.Name == "Address_Street");
        Assert.NotNull(addressStreet);
        Assert.True(addressStreet.IsMigratable);
        Assert.Equal("Address", addressStreet.SourceColumn);

        var migrationIndustry = clientTable.Destination.Columns.FirstOrDefault(c => c.Name == "MigrationIndustry");
        Assert.NotNull(migrationIndustry);
        Assert.True(migrationIndustry.IsMigratable);
        Assert.Equal("Industry", migrationIndustry.SourceColumn);

        var migrationCompanyCode = clientTable.Destination.Columns.FirstOrDefault(c => c.Name == "MigrationCompanyCode");
        Assert.NotNull(migrationCompanyCode);
        Assert.True(migrationCompanyCode.IsMigratable);
        Assert.Equal("CompanyCode", migrationCompanyCode.SourceColumn);
    }

    [Fact]
    public void TablesToMigrate_ClientTable_ShouldHaveLookupConfiguration()
    {
        // Act
        var tables = TableDefinitions.TablesToMigrate;
        var clientTable = tables.FirstOrDefault(t => t.Table == "Client");

        // Assert
        Assert.NotNull(clientTable);

        var tenantId = clientTable.Destination.Columns.FirstOrDefault(c => c.Name == "TenantId");
        Assert.NotNull(tenantId);
        Assert.False(tenantId.IsMigratable);
        Assert.Equal("Tenants", tenantId.LookupTable);
        Assert.Equal("TenantId", tenantId.LookupColumn);
        Assert.Equal("MigrationCompanyCode", tenantId.LookupCondition);
        Assert.Equal("CompanyCode", tenantId.LookupSource);
    }

    [Fact]
    public void TablesToMigrate_ClientTable_ShouldHaveTransformConfiguration()
    {
        // Act
        var tables = TableDefinitions.TablesToMigrate;
        var clientTable = tables.FirstOrDefault(t => t.Table == "Client");

        // Assert
        Assert.NotNull(clientTable);

        var isDeleted = clientTable.Destination.Columns.FirstOrDefault(c => c.Name == "IsDeleted");
        Assert.NotNull(isDeleted);
        Assert.False(isDeleted.IsMigratable);
        Assert.Equal("InvertActive", isDeleted.Transform);
        Assert.Equal("Active", isDeleted.SourceColumn);

        var deleted = clientTable.Destination.Columns.FirstOrDefault(c => c.Name == "Deleted");
        Assert.NotNull(deleted);
        Assert.False(deleted.IsMigratable);
        Assert.Equal("ConditionalDeletedDate", deleted.Transform);
        Assert.Equal("Active", deleted.SourceColumn);

        var deletedBy = clientTable.Destination.Columns.FirstOrDefault(c => c.Name == "DeletedBy");
        Assert.NotNull(deletedBy);
        Assert.False(deletedBy.IsMigratable);
        Assert.Equal("ConditionalDeletedBy", deletedBy.Transform);
        Assert.Equal("Active", deletedBy.SourceColumn);
    }

    [Fact]
    public void TablesToMigrate_ShouldBeLoadedLazily()
    {
        // This test ensures that the lazy loading mechanism works
        // and doesn't throw exceptions during initialization

        // Act & Assert
        var exception = Record.Exception(() =>
        {
            var tables1 = TableDefinitions.TablesToMigrate;
            var tables2 = TableDefinitions.TablesToMigrate;

            // Should return the same instance due to lazy loading
            Assert.Same(tables1, tables2);
        });

        Assert.Null(exception);
    }

    [Fact]
    public void TablesToMigrate_ShouldBeInCorrectMigrationOrder()
    {
        // Act
        var tables = TableDefinitions.TablesToMigrate;

        // Assert
        Assert.Equal(2, tables.Count);

        // Company should be first (migration order 1)
        Assert.Equal("Company", tables[0].Table);
        Assert.Equal(1, tables[0].MigrationOrder);

        // Client should be second (migration order 2)
        Assert.Equal("Client", tables[1].Table);
        Assert.Equal(2, tables[1].MigrationOrder);
    }
}
