using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Luno_platform.Migrations
{
    /// <inheritdoc />
    public partial class v10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "classID",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Courses_classID",
                table: "Courses",
                column: "classID");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Classes_classID",
                table: "Courses",
                column: "classID",
                principalTable: "Classes",
                principalColumn: "ClassID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Classes_classID",
                table: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_Courses_classID",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "classID",
                table: "Courses");
        }
    }
}
