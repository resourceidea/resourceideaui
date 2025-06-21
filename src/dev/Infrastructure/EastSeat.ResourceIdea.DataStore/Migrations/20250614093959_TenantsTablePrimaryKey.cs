using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EastSeat.ResourceIdea.DataStore.Migrations
{
    /// <inheritdoc />
    public partial class TenantsTablePrimaryKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ClientId",
                table: "Engagements",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Engagements_ClientId",
                table: "Engagements",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Engagements_Clients_ClientId",
                table: "Engagements",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tenants",
                table: "Tenants",
                column: "TenantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tenants",
                table: "Tenants");

            migrationBuilder.DropForeignKey(
                name: "FK_Engagements_Clients_ClientId",
                table: "Engagements");

            migrationBuilder.DropIndex(
                name: "IX_Engagements_ClientId",
                table: "Engagements");

            migrationBuilder.AlterColumn<string>(
                name: "ClientId",
                table: "Engagements",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
