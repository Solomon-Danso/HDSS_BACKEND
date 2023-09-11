using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HDSS_BACKEND.Migrations
{
    /// <inheritdoc />
    public partial class thirtybilliondollarsinmybankaccountafter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Balance",
                table: "Students",
                type: "float",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BillingCards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OpeningBalance = table.Column<double>(type: "float", nullable: true),
                    Transaction = table.Column<double>(type: "float", nullable: true),
                    ClosingBalance = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillingCards", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BillingCards");

            migrationBuilder.DropColumn(
                name: "Balance",
                table: "Students");
        }
    }
}
