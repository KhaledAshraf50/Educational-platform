using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Luno_platform.Migrations
{
    /// <inheritdoc />
    public partial class v3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Courses_CourseContents_CourseId",
            //    table: "Courses");

            //migrationBuilder.DropColumn(
            //    name: "cousrsid",
            //    table: "CourseContents");

            //migrationBuilder.AlterColumn<int>(
            //    name: "CourseId",
            //    table: "Courses",
            //    type: "int",
            //    nullable: false,
            //    oldClrType: typeof(int),
            //    oldType: "int")
            //    .Annotation("SqlServer:Identity", "1, 1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AlterColumn<int>(
            //    name: "CourseId",
            //    table: "Courses",
            //    type: "int",
            //    nullable: false,
            //    oldClrType: typeof(int),
            //    oldType: "int")
            //    .OldAnnotation("SqlServer:Identity", "1, 1");

            //migrationBuilder.AddColumn<int>(
            //    name: "cousrsid",
            //    table: "CourseContents",
            //    type: "int",
            //    maxLength: 1000,
            //    nullable: false,
            //    defaultValue: 0);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Courses_CourseContents_CourseId",
            //    table: "Courses",
            //    column: "CourseId",
            //    principalTable: "CourseContents",
            //    principalColumn: "Id");
        }
    }
}
