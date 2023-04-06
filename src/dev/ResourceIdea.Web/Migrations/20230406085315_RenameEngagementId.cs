using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace resourceideaui.Migrations
{
    /// <inheritdoc />
    public partial class RenameEngagementId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TaskId",
                table: "Job",
                newName: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Job",
                newName: "TaskId");
        }
    }
}
