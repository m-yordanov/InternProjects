using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InternProjects.Migrations
{
    /// <inheritdoc />
    public partial class FixRemoveUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimeLogs_Interns_InternId",
                table: "TimeLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_TimeLogs_TaskAssignments_AssignmentId",
                table: "TimeLogs");

            migrationBuilder.AddForeignKey(
                name: "FK_TimeLogs_Interns_InternId",
                table: "TimeLogs",
                column: "InternId",
                principalTable: "Interns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TimeLogs_TaskAssignments_AssignmentId",
                table: "TimeLogs",
                column: "AssignmentId",
                principalTable: "TaskAssignments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimeLogs_Interns_InternId",
                table: "TimeLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_TimeLogs_TaskAssignments_AssignmentId",
                table: "TimeLogs");

            migrationBuilder.AddForeignKey(
                name: "FK_TimeLogs_Interns_InternId",
                table: "TimeLogs",
                column: "InternId",
                principalTable: "Interns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TimeLogs_TaskAssignments_AssignmentId",
                table: "TimeLogs",
                column: "AssignmentId",
                principalTable: "TaskAssignments",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
