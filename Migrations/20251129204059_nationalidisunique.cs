using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Luno_platform.Migrations
{
    /// <inheritdoc />
    public partial class nationalidisunique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_nationalID",
                table: "AspNetUsers",
                column: "nationalID",
                unique: true,
                filter: "[nationalID] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_nationalID",
                table: "AspNetUsers");
        }
    }
}
