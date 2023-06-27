using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HDSS_BACKEND.Migrations
{
    /// <inheritdoc />
    public partial class eleven : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SchoolFees_Students_StudentId",
                table: "SchoolFees");

            migrationBuilder.DropIndex(
                name: "IX_SchoolFees_StudentId",
                table: "SchoolFees");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "SchoolFees");

            migrationBuilder.AddColumn<double>(
                name: "amountOwing",
                table: "Students",
                type: "float",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AmountsOwing",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AmountsOwing", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AmountsPaid",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AmountDebtOld = table.Column<double>(type: "float", nullable: true),
                    Amountpaid = table.Column<double>(type: "float", nullable: true),
                    AmountDebtNew = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AmountsPaid", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AmountsOwing");

            migrationBuilder.DropTable(
                name: "AmountsPaid");

            migrationBuilder.DropColumn(
                name: "amountOwing",
                table: "Students");

            migrationBuilder.AddColumn<int>(
                name: "StudentId",
                table: "SchoolFees",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SchoolFees_StudentId",
                table: "SchoolFees",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_SchoolFees_Students_StudentId",
                table: "SchoolFees",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id");
        }
    }
}
