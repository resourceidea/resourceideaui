using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace resourceideaui.Migrations
{
    /// <inheritdoc />
    public partial class TableRename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "IX_CompanyCode1",
                table: "RoleGrouping",
                newName: "IX_CompanyCode");

            migrationBuilder.RenameColumn(
                name: "ProjectId",
                table: "Project",
                newName: "EngagementId");

            migrationBuilder.RenameColumn(
                name: "JobId",
                table: "JobResource",
                newName: "TaskId");

            migrationBuilder.RenameIndex(
                name: "IX_JobResource_JobId",
                table: "JobResource",
                newName: "IX_JobResource_TaskId");

            migrationBuilder.RenameColumn(
                name: "ProjectId",
                table: "Job",
                newName: "EngagementId");

            migrationBuilder.RenameColumn(
                name: "JobId",
                table: "Job",
                newName: "TaskId");

            migrationBuilder.RenameIndex(
                name: "IX_Job_ProjectId",
                table: "Job",
                newName: "IX_Job_EngagementId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "UserTokens",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "UserTokens",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "UserLogins",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "UserLogins",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "IX_CompanyCode",
                table: "RoleGrouping",
                newName: "IX_CompanyCode1");

            migrationBuilder.RenameColumn(
                name: "EngagementId",
                table: "Project",
                newName: "ProjectId");

            migrationBuilder.RenameColumn(
                name: "TaskId",
                table: "JobResource",
                newName: "JobId");

            migrationBuilder.RenameIndex(
                name: "IX_JobResource_TaskId",
                table: "JobResource",
                newName: "IX_JobResource_JobId");

            migrationBuilder.RenameColumn(
                name: "EngagementId",
                table: "Job",
                newName: "ProjectId");

            migrationBuilder.RenameColumn(
                name: "TaskId",
                table: "Job",
                newName: "JobId");

            migrationBuilder.RenameIndex(
                name: "IX_Job_EngagementId",
                table: "Job",
                newName: "IX_Job_ProjectId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "UserTokens",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "UserTokens",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "UserLogins",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "UserLogins",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
