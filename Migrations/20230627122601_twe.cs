using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HDSS_BACKEND.Migrations
{
    /// <inheritdoc />
    public partial class twe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "creditAmount",
                table: "Students",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "CreditAmount",
                table: "AmountsPaid",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "creditAmount",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "CreditAmount",
                table: "AmountsPaid");
        }
    }
}
