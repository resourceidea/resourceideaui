using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EastSeat.ResourceIdea.DataStore.Migrations
{
    /// <inheritdoc />
    public partial class ChangeEngagementsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CompletionDate",
                table: "Engagements",
                newName: "StartDate");

            migrationBuilder.RenameColumn(
                name: "CommencementDate",
                table: "Engagements",
                newName: "EndDate");

            migrationBuilder.AddColumn<string>(
                name: "ManagerId",
                table: "Engagements",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PartnerId",
                table: "Engagements",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Engagements",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ManagerId",
                table: "Engagements");

            migrationBuilder.DropColumn(
                name: "PartnerId",
                table: "Engagements");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Engagements");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "Engagements",
                newName: "CompletionDate");

            migrationBuilder.RenameColumn(
                name: "EndDate",
                table: "Engagements",
                newName: "CommencementDate");
        }
    }
}
