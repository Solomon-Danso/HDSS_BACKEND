using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HDSS_BACKEND.Migrations
{
    /// <inheritdoc />
    public partial class mnbvcxz : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClassToTeach");

            migrationBuilder.DropTable(
                name: "SchoolBank");

            migrationBuilder.DropTable(
                name: "SubjectToTeach");

            migrationBuilder.AddColumn<double>(
                name: "Credit",
                table: "Teachers",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Debit",
                table: "Teachers",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Credit",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "Debit",
                table: "Teachers");

            migrationBuilder.CreateTable(
                name: "ClassToTeach",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TeacherId = table.Column<int>(type: "int", nullable: false),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassToTeach", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClassToTeach_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SchoolBank",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TeacherId = table.Column<int>(type: "int", nullable: false),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolBank", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SchoolBank_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubjectToTeach",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TeacherId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectToTeach", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubjectToTeach_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClassToTeach_TeacherId",
                table: "ClassToTeach",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolBank_TeacherId",
                table: "SchoolBank",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectToTeach_TeacherId",
                table: "SubjectToTeach",
                column: "TeacherId");
        }
    }
}
