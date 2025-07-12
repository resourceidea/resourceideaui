using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EastSeat.ResourceIdea.DataStore.Migrations
{
    /// <inheritdoc />
    public partial class RenameClientMigrationColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MigratedCompanyCode",
                table: "Clients",
                newName: "MigrationCompanyCode");

            migrationBuilder.RenameColumn(
                name: "MigratedClientId",
                table: "Clients",
                newName: "MigrationClientId");

            migrationBuilder.AlterColumn<string>(
                name: "MigrationCompanyCode",
                table: "Tenants",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MigrationCompanyCode",
                table: "Clients",
                newName: "MigratedCompanyCode");

            migrationBuilder.RenameColumn(
                name: "MigrationClientId",
                table: "Clients",
                newName: "MigratedClientId");

            migrationBuilder.AlterColumn<string>(
                name: "MigrationCompanyCode",
                table: "Tenants",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);
        }
    }
}
