using EastSeat.ResourceIdea.Migration.Configuration;
using EastSeat.ResourceIdea.Migration.Model;

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
        Assert.Equal(15, tables.Count);
    }

    [Fact]
    public void TablesToMigrate_ShouldContainExpectedTables()
    {
        // Arrange
        var expectedTableNames = new[]
        {
            "AspNetUsers", "AspNetRoles", "AspNetUserRoles", "AspNetUserClaims",
            "AspNetUserLogins", "AspNetRoleClaims", "AspNetUserTokens",
            "Tenants", "Clients", "Departments", "Engagements",
            "SubscriptionServices", "Subscriptions", "JobPositions", "Employees"
        };

        // Act
        var tables = TableDefinitions.TablesToMigrate;
        var actualTableNames = tables.Select(t => t.TableName).ToHashSet();

        // Assert
        foreach (var expectedTableName in expectedTableNames)
        {
            Assert.Contains(expectedTableName, actualTableNames);
        }
    }

    [Fact]
    public void TablesToMigrate_AspNetUsersTable_ShouldHaveCorrectColumns()
    {
        // Act
        var tables = TableDefinitions.TablesToMigrate;
        var aspNetUsersTable = tables.FirstOrDefault(t => t.TableName == "AspNetUsers");

        // Assert
        Assert.NotEqual(default, aspNetUsersTable);
        Assert.Equal(15, aspNetUsersTable.Columns.Count);

        var idColumn = aspNetUsersTable.Columns.FirstOrDefault(c => c.Name == "Id");
        Assert.NotEqual(default(ColumnSchema), idColumn);
        Assert.Equal("nvarchar(450)", idColumn.Type);
    }

    [Fact]
    public void TablesToMigrate_TenantsTable_ShouldHaveCorrectColumns()
    {
        // Act
        var tables = TableDefinitions.TablesToMigrate;
        var tenantsTable = tables.FirstOrDefault(t => t.TableName == "Tenants");

        // Assert
        Assert.NotEqual(default, tenantsTable);
        Assert.Equal(4, tenantsTable.Columns.Count);

        var expectedColumns = new[]
        {
            new ColumnSchema("Id", "uniqueidentifier"),
            new ColumnSchema("Name", "nvarchar(255)"),
            new ColumnSchema("CreatedDate", "datetime2"),
            new ColumnSchema("ModifiedDate", "datetime2")
        };

        foreach (var expectedColumn in expectedColumns)
        {
            Assert.Contains(expectedColumn, tenantsTable.Columns);
        }
    }

    [Fact]
    public void TablesToMigrate_ShouldBeLoadedLazily()
    {
        // This test ensures that the lazy loading mechanism works
        // and doesn't throw exceptions during initialization

        // Act & Assert
        Assert.DoesNotThrow(() =>
        {
            var tables1 = TableDefinitions.TablesToMigrate;
            var tables2 = TableDefinitions.TablesToMigrate;

            // Should return the same instance due to lazy loading
            Assert.Same(tables1, tables2);
        });
    }
}
