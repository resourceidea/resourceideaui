using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EastSeat.ResourceIdea.DataStore.Migrations
{
    /// <inheritdoc />
    public partial class JobResourceMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "WorkItems",
                newName: "PlannedStartDate");

            migrationBuilder.AddColumn<string>(
                name: "MigrationJobId",
                table: "WorkItems",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MigrationJobResourceId",
                table: "WorkItems",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MigrationResourceId",
                table: "WorkItems",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "PlannedEndDate",
                table: "WorkItems",
                type: "datetimeoffset",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MigrationJobId",
                table: "WorkItems");

            migrationBuilder.DropColumn(
                name: "MigrationJobResourceId",
                table: "WorkItems");

            migrationBuilder.DropColumn(
                name: "MigrationResourceId",
                table: "WorkItems");

            migrationBuilder.DropColumn(
                name: "PlannedEndDate",
                table: "WorkItems");

            migrationBuilder.RenameColumn(
                name: "PlannedStartDate",
                table: "WorkItems",
                newName: "StartDate");
        }
    }
}
