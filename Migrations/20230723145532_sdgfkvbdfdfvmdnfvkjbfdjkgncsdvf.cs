using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HDSS_BACKEND.Migrations
{
    /// <inheritdoc />
    public partial class sdgfkvbdfdfvmdnfvkjbfdjkgncsdvf : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SchoolDirectors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Contact = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TwoSteps = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TwoStepsExpire = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PasswordToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PasswordTokenExpire = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolDirectors", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SchoolDirectors");
        }
    }
}
