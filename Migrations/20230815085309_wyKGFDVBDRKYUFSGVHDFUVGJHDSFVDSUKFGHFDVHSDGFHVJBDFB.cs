using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HDSS_BACKEND.Migrations
{
    /// <inheritdoc />
    public partial class wyKGFDVBDRKYUFSGVHDFUVGJHDSFVDSUKFGHFDVHSDGFHVJBDFB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Students",
                newName: "Religion");

            migrationBuilder.RenameColumn(
                name: "MothersContact",
                table: "Students",
                newName: "PhoneNumber");

            migrationBuilder.RenameColumn(
                name: "GuardianContact",
                table: "Students",
                newName: "MotherOccupation");

            migrationBuilder.RenameColumn(
                name: "FathersContact",
                table: "Students",
                newName: "GuardianOccupation");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Students",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FatherOccupation",
                table: "Students",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "FatherOccupation",
                table: "Students");

            migrationBuilder.RenameColumn(
                name: "Religion",
                table: "Students",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "Students",
                newName: "MothersContact");

            migrationBuilder.RenameColumn(
                name: "MotherOccupation",
                table: "Students",
                newName: "GuardianContact");

            migrationBuilder.RenameColumn(
                name: "GuardianOccupation",
                table: "Students",
                newName: "FathersContact");
        }
    }
}
