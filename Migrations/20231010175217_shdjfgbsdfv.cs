using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HDSS_BACKEND.Migrations
{
    /// <inheritdoc />
    public partial class shdjfgbsdfv : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AppointmentLetter",
                table: "Teachers",
                newName: "StartDate");

            migrationBuilder.AddColumn<string>(
                name: "Position",
                table: "Teachers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReportingTime",
                table: "Teachers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Position",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "ReportingTime",
                table: "Teachers");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "Teachers",
                newName: "AppointmentLetter");
        }
    }
}
