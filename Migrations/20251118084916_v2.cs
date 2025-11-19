using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Luno_platform.Migrations
{
    /// <inheritdoc />
    public partial class v2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SubjectName",
                table: "Subjects",
                newName: "SubjectNameAR");

            migrationBuilder.AddColumn<string>(
                name: "SubjectNameEN",
                table: "Subjects",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubjectNameEN",
                table: "Subjects");

            migrationBuilder.RenameColumn(
                name: "SubjectNameAR",
                table: "Subjects",
                newName: "SubjectName");
        }
    }
}
