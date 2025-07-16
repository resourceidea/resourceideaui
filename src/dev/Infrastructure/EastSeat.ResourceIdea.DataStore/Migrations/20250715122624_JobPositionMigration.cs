using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EastSeat.ResourceIdea.DataStore.Migrations
{
    /// <inheritdoc />
    public partial class JobPositionMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MigrationCompanyCode",
                table: "JobPositions",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MigrationJobLevel",
                table: "JobPositions",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MigrationJobPositionId",
                table: "JobPositions",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MigrationCompanyCode",
                table: "JobPositions");

            migrationBuilder.DropColumn(
                name: "MigrationJobLevel",
                table: "JobPositions");

            migrationBuilder.DropColumn(
                name: "MigrationJobPositionId",
                table: "JobPositions");
        }
    }
}
