using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HDSS_BACKEND.Migrations
{
    /// <inheritdoc />
    public partial class thirtybilliondollarsinmybankaccountafte : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AcademicTerm",
                table: "BillingCards",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AcademicYear",
                table: "BillingCards",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Level",
                table: "BillingCards",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TransactionDate",
                table: "BillingCards",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AcademicTerm",
                table: "BillingCards");

            migrationBuilder.DropColumn(
                name: "AcademicYear",
                table: "BillingCards");

            migrationBuilder.DropColumn(
                name: "Level",
                table: "BillingCards");

            migrationBuilder.DropColumn(
                name: "TransactionDate",
                table: "BillingCards");
        }
    }
}
