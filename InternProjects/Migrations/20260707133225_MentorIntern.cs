using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InternProjects.Migrations
{
    /// <inheritdoc />
    public partial class MentorIntern : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InternId",
                table: "TimeLog",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SourceType",
                table: "TimeLog",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<float>(
                name: "TaskHours",
                table: "Intern",
                type: "real",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AlterColumn<float>(
                name: "ReportedHours",
                table: "Intern",
                type: "real",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AlterColumn<float>(
                name: "RemainingHours",
                table: "Intern",
                type: "real",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AlterColumn<float>(
                name: "AddedHours",
                table: "Intern",
                type: "real",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AddColumn<int>(
                name: "MentorId",
                table: "Intern",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TimeLog_InternId",
                table: "TimeLog",
                column: "InternId");

            migrationBuilder.CreateIndex(
                name: "IX_Intern_MentorId",
                table: "Intern",
                column: "MentorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Intern_User_MentorId",
                table: "Intern",
                column: "MentorId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TimeLog_Intern_InternId",
                table: "TimeLog",
                column: "InternId",
                principalTable: "Intern",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Intern_User_MentorId",
                table: "Intern");

            migrationBuilder.DropForeignKey(
                name: "FK_TimeLog_Intern_InternId",
                table: "TimeLog");

            migrationBuilder.DropIndex(
                name: "IX_TimeLog_InternId",
                table: "TimeLog");

            migrationBuilder.DropIndex(
                name: "IX_Intern_MentorId",
                table: "Intern");

            migrationBuilder.DropColumn(
                name: "InternId",
                table: "TimeLog");

            migrationBuilder.DropColumn(
                name: "SourceType",
                table: "TimeLog");

            migrationBuilder.DropColumn(
                name: "MentorId",
                table: "Intern");

            migrationBuilder.AlterColumn<float>(
                name: "TaskHours",
                table: "Intern",
                type: "real",
                nullable: false,
                defaultValue: 0f,
                oldClrType: typeof(float),
                oldType: "real",
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "ReportedHours",
                table: "Intern",
                type: "real",
                nullable: false,
                defaultValue: 0f,
                oldClrType: typeof(float),
                oldType: "real",
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "RemainingHours",
                table: "Intern",
                type: "real",
                nullable: false,
                defaultValue: 0f,
                oldClrType: typeof(float),
                oldType: "real",
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "AddedHours",
                table: "Intern",
                type: "real",
                nullable: false,
                defaultValue: 0f,
                oldClrType: typeof(float),
                oldType: "real",
                oldNullable: true);
        }
    }
}
