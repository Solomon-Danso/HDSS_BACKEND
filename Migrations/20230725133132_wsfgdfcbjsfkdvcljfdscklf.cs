using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HDSS_BACKEND.Migrations
{
    /// <inheritdoc />
    public partial class wsfgdfcbjsfkdvcljfdscklf : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "PasswordResetToken",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "PasswordResetTokenExpire",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "TwoStepsAuthToken",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "PasswordTokenExpire",
                table: "SchoolDirectors");

            migrationBuilder.DropColumn(
                name: "TwoStepsExpire",
                table: "SchoolDirectors");

            migrationBuilder.RenameColumn(
                name: "TwoStepsAuthTokenExpire",
                table: "Teachers",
                newName: "SpecificRole");

            migrationBuilder.RenameColumn(
                name: "TwoSteps",
                table: "SchoolDirectors",
                newName: "SpecificRole");

            migrationBuilder.RenameColumn(
                name: "PasswordToken",
                table: "SchoolDirectors",
                newName: "ProfilePicturePath");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "SchoolDirectors",
                newName: "DirectorID");

            migrationBuilder.CreateTable(
                name: "Managers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Contact = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ManagerID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfilePicturePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SpecificRole = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Managers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OnlySuperiorsCanViewThisDueToSecurityReasons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserPassword = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OnlySuperiorsCanViewThisDueToSecurityReasons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SuperiorAccounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SuperiorID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Contact = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SpecificRole = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfilePicturePath = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SuperiorAccounts", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Managers");

            migrationBuilder.DropTable(
                name: "OnlySuperiorsCanViewThisDueToSecurityReasons");

            migrationBuilder.DropTable(
                name: "SuperiorAccounts");

            migrationBuilder.RenameColumn(
                name: "SpecificRole",
                table: "Teachers",
                newName: "TwoStepsAuthTokenExpire");

            migrationBuilder.RenameColumn(
                name: "SpecificRole",
                table: "SchoolDirectors",
                newName: "TwoSteps");

            migrationBuilder.RenameColumn(
                name: "ProfilePicturePath",
                table: "SchoolDirectors",
                newName: "PasswordToken");

            migrationBuilder.RenameColumn(
                name: "DirectorID",
                table: "SchoolDirectors",
                newName: "Password");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Teachers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PasswordResetToken",
                table: "Teachers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PasswordResetTokenExpire",
                table: "Teachers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TwoStepsAuthToken",
                table: "Teachers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PasswordTokenExpire",
                table: "SchoolDirectors",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TwoStepsExpire",
                table: "SchoolDirectors",
                type: "datetime2",
                nullable: true);
        }
    }
}
