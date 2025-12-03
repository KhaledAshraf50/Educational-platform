using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Luno_platform.Migrations
{
    /// <inheritdoc />
    public partial class v7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PaymentRefId",
                table: "Teacher_Payments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Teacher_Payments_PaymentRefId",
                table: "Teacher_Payments",
                column: "PaymentRefId");

            migrationBuilder.AddForeignKey(
                name: "FK_Teacher_Payments_Payments_PaymentRefId",
                table: "Teacher_Payments",
                column: "PaymentRefId",
                principalTable: "Payments",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teacher_Payments_Payments_PaymentRefId",
                table: "Teacher_Payments");

            migrationBuilder.DropIndex(
                name: "IX_Teacher_Payments_PaymentRefId",
                table: "Teacher_Payments");

            migrationBuilder.DropColumn(
                name: "PaymentRefId",
                table: "Teacher_Payments");
        }
    }
}
