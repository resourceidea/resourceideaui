using EastSeat.ResourceIdea.Migration.Configuration;
using EastSeat.ResourceIdea.Migration.Model;
using System.Linq;
using Xunit;

namespace EastSeat.ResourceIdea.Migration.UnitTests.Services;

/// <summary>
/// Unit tests for engagement migration functionality.
/// </summary>
public class EngagementMigrationTests
{
    [Fact]
    public void TableDefinitions_Should_Include_Job_Table_Only()
    {
        // Arrange & Act
        var tables = TableDefinitions.TablesToMigrate;

        // Assert
        Assert.NotEmpty(tables);

        var jobTable = tables.FirstOrDefault(t => t.Schema == "dbo" && t.Table == "Job");
        var projectTable = tables.FirstOrDefault(t => t.Schema == "dbo" && t.Table == "Project");

        Assert.NotNull(jobTable);
        Assert.Null(projectTable);
    }

    [Fact]
    public void Job_Table_Should_Have_Correct_Migration_Order()
    {
        // Arrange & Act
        var tables = TableDefinitions.TablesToMigrate;
        var jobTable = tables.FirstOrDefault(t => t.Schema == "dbo" && t.Table == "Job");

        // Assert
        Assert.NotNull(jobTable);
        Assert.Equal(3, jobTable!.MigrationOrder);
    }


    [Fact]
    public void Job_Table_Should_Map_To_Engagements_Table()
    {
        // Arrange & Act
        var tables = TableDefinitions.TablesToMigrate;
        var jobTable = tables.FirstOrDefault(t => t.Schema == "dbo" && t.Table == "Job");

        // Assert
        Assert.NotNull(jobTable);
        Assert.Equal("dbo", jobTable!.Destination.Schema);
        Assert.Equal("Engagements", jobTable.Destination.Table);
    }
    [Fact]
    public void Job_Table_Should_Have_Combined_Source_Columns()
    {
        // Arrange & Act
        var tables = TableDefinitions.TablesToMigrate;
        var jobTable = tables.FirstOrDefault(t => t.Schema == "dbo" && t.Table == "Job");

        // Assert
        Assert.NotNull(jobTable);
        Assert.Equal(9, jobTable!.Columns.Count);

        var expectedColumns = new[]
        {
            ("JobId", "varchar(40)"),
            ("Description", "varchar(200)"),
            ("ProjectId", "varchar(40)"),
            ("Color", "varchar(10)"),
            ("Status", "varchar(20)"),
            ("Manager", "varchar(40)"),
            ("Partner", "varchar(40)"),
            ("ProjectName", "varchar(100)"),
            ("ProjectClientId", "varchar(40)")
        };

        foreach (var (name, type) in expectedColumns)
        {
            var column = jobTable.Columns.FirstOrDefault(c => c.Name == name);
            Assert.NotNull(column);
            Assert.Equal(type, column!.Type);
        }
    }
    [Fact]
    public void Job_Table_Should_Map_ProjectName_To_Title()
    {
        // Arrange & Act
        var tables = TableDefinitions.TablesToMigrate;
        var jobTable = tables.FirstOrDefault(t => t.Schema == "dbo" && t.Table == "Job");

        // Assert
        Assert.NotNull(jobTable);

        var titleColumn = jobTable!.Destination.Columns
            .FirstOrDefault(c => c.Name == "Title");

        Assert.NotNull(titleColumn);
        Assert.True(titleColumn!.IsMigratable);
        Assert.Equal("ProjectName", titleColumn.SourceColumn);
    }

    [Fact]
    public void Job_Table_Should_Map_ProjectClientId_To_MigrationClientId()
    {
        // Arrange & Act
        var tables = TableDefinitions.TablesToMigrate;
        var jobTable = tables.FirstOrDefault(t => t.Schema == "dbo" && t.Table == "Job");

        // Assert
        Assert.NotNull(jobTable);

        var migrationClientIdColumn = jobTable!.Destination.Columns
            .FirstOrDefault(c => c.Name == "MigrationClientId");

        Assert.NotNull(migrationClientIdColumn);
        Assert.True(migrationClientIdColumn!.IsMigratable);
        Assert.Equal("ProjectClientId", migrationClientIdColumn.SourceColumn);
    }

    [Fact]
    public void Job_Table_Should_Map_JobId_To_MigrationJobId()
    {
        // Arrange & Act
        var tables = TableDefinitions.TablesToMigrate;
        var jobTable = tables.FirstOrDefault(t => t.Schema == "dbo" && t.Table == "Job");

        // Assert
        Assert.NotNull(jobTable);

        var migrationJobIdColumn = jobTable!.Destination.Columns
            .FirstOrDefault(c => c.Name == "MigrationJobId");

        Assert.NotNull(migrationJobIdColumn);
        Assert.True(migrationJobIdColumn!.IsMigratable);
        Assert.Equal("JobId", migrationJobIdColumn.SourceColumn);
    }

    [Fact]
    public void Job_Table_Should_Map_Description_To_Description()
    {
        // Arrange & Act
        var tables = TableDefinitions.TablesToMigrate;
        var jobTable = tables.FirstOrDefault(t => t.Schema == "dbo" && t.Table == "Job");

        // Assert
        Assert.NotNull(jobTable);

        var descriptionColumn = jobTable!.Destination.Columns
            .FirstOrDefault(c => c.Name == "Description");

        Assert.NotNull(descriptionColumn);
        Assert.True(descriptionColumn!.IsMigratable);
        Assert.Equal("Description", descriptionColumn.SourceColumn);
    }

    [Fact]
    public void Job_Table_Should_Map_Status_To_EngagementStatus()
    {
        // Arrange & Act
        var tables = TableDefinitions.TablesToMigrate;
        var jobTable = tables.FirstOrDefault(t => t.Schema == "dbo" && t.Table == "Job");

        // Assert
        Assert.NotNull(jobTable);

        var engagementStatusColumn = jobTable!.Destination.Columns
            .FirstOrDefault(c => c.Name == "EngagementStatus");

        Assert.NotNull(engagementStatusColumn);
        Assert.True(engagementStatusColumn!.IsMigratable);
        Assert.Equal("Status", engagementStatusColumn.SourceColumn);
    }

    [Fact]
    public void Job_Table_Should_Map_Manager_To_MigrationManager()
    {
        // Arrange & Act
        var tables = TableDefinitions.TablesToMigrate;
        var jobTable = tables.FirstOrDefault(t => t.Schema == "dbo" && t.Table == "Job");

        // Assert
        Assert.NotNull(jobTable);

        var migrationManagerColumn = jobTable!.Destination.Columns
            .FirstOrDefault(c => c.Name == "MigrationManager");

        Assert.NotNull(migrationManagerColumn);
        Assert.True(migrationManagerColumn!.IsMigratable);
        Assert.Equal("Manager", migrationManagerColumn.SourceColumn);
    }

    [Fact]
    public void Job_Table_Should_Map_Partner_To_MigrationPartner()
    {
        // Arrange & Act
        var tables = TableDefinitions.TablesToMigrate;
        var jobTable = tables.FirstOrDefault(t => t.Schema == "dbo" && t.Table == "Job");

        // Assert
        Assert.NotNull(jobTable);

        var migrationPartnerColumn = jobTable!.Destination.Columns
            .FirstOrDefault(c => c.Name == "MigrationPartner");

        Assert.NotNull(migrationPartnerColumn);
        Assert.True(migrationPartnerColumn!.IsMigratable);
        Assert.Equal("Partner", migrationPartnerColumn.SourceColumn);
    }
    [Fact]
    public void Job_Table_Should_Use_Standard_Lookup_For_ClientId_And_TenantId()
    {
        // Arrange & Act
        var tables = TableDefinitions.TablesToMigrate;
        var jobTable = tables.FirstOrDefault(t => t.Schema == "dbo" && t.Table == "Job");

        // Assert
        Assert.NotNull(jobTable);

        // Check ClientId lookup - should use standard lookup pattern now
        var clientIdColumn = jobTable!.Destination.Columns
            .FirstOrDefault(c => c.Name == "ClientId");
        Assert.NotNull(clientIdColumn);
        Assert.Equal("Clients", clientIdColumn!.LookupTable);
        Assert.Equal("Id", clientIdColumn.LookupColumn);
        Assert.Equal("MigrationClientId", clientIdColumn.LookupCondition);
        Assert.Equal("ProjectClientId", clientIdColumn.LookupSource);

        // Check TenantId lookup - should use standard lookup pattern now
        var tenantIdColumn = jobTable.Destination.Columns
            .FirstOrDefault(c => c.Name == "TenantId");
        Assert.NotNull(tenantIdColumn);
        Assert.Equal("Clients", tenantIdColumn!.LookupTable);
        Assert.Equal("TenantId", tenantIdColumn.LookupColumn);
        Assert.Equal("MigrationClientId", tenantIdColumn.LookupCondition);
        Assert.Equal("ProjectClientId", tenantIdColumn.LookupSource);
    }
}