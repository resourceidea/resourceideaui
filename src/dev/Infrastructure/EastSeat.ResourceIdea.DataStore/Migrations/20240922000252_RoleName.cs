using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EastSeat.ResourceIdea.DataStore.Migrations
{
    /// <inheritdoc />
    public partial class RoleName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RoleName",
                schema: "Identity",
                table: "Roles",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoleName",
                schema: "Identity",
                table: "Roles");
        }
    }
}
