using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HDSS_BACKEND.Migrations
{
    /// <inheritdoc />
    public partial class sdgfkvbdfdfvmdnfvkjbfdjkgncsdv : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "Teachers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "Students",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AuthenticationModels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserPassword = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TwoSteps = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TwoStepsExpire = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PasswordToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PasswordTokenExpire = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthenticationModels", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthenticationModels");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "Students");
        }
    }
}
