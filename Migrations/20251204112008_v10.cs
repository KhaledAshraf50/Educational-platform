using System;
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
            migrationBuilder.CreateTable(
                name: "Teacher_Payments2",
                columns: table => new
                {
                    paymentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaymentRefId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AmountPaid = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    instructorID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teacher_Payments2", x => x.paymentID);
                    table.ForeignKey(
                        name: "FK_Teacher_Payments2_Instructors_instructorID",
                        column: x => x.instructorID,
                        principalTable: "Instructors",
                        principalColumn: "instructorID");
                    table.ForeignKey(
                        name: "FK_Teacher_Payments2_Payments_PaymentRefId",
                        column: x => x.PaymentRefId,
                        principalTable: "Payments",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Teacher_Payments2_instructorID",
                table: "Teacher_Payments2",
                column: "instructorID");

            migrationBuilder.CreateIndex(
                name: "IX_Teacher_Payments2_PaymentRefId",
                table: "Teacher_Payments2",
                column: "PaymentRefId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Teacher_Payments2");
        }
    }
}
