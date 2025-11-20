using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Luno_platform.Migrations
{
    /// <inheritdoc />
    public partial class v98 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // أضف العمود الصحيح في CourseContents
            migrationBuilder.AddColumn<int>(
                name: "CourseId",
                table: "CourseContents",
                type: "int",
                nullable: false,
                defaultValue: 0);

            // أضف Index فريد (1:1 relationship)
            migrationBuilder.CreateIndex(
                name: "IX_CourseContents_CourseId",
                table: "CourseContents",
                column: "CourseId",
                unique: true);

            // أضف FK من CourseContents إلى Courses
            migrationBuilder.AddForeignKey(
                name: "FK_CourseContents_Courses_CourseId",
                table: "CourseContents",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Courseid", // مهم جداً: استخدم اسم العمود الصحيح
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseContents_Courses_CourseId",
                table: "CourseContents");

            migrationBuilder.DropIndex(
                name: "IX_CourseContents_CourseId",
                table: "CourseContents");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "CourseContents");
        }

    }
}
