using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HDSS_BACKEND.Migrations
{
    /// <inheritdoc />
    public partial class wyKGFDVBDRKYUFSGVHDFUVGJHDSFVDSUKFGHFDVHSDGFHV : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ParentPhoneNumber",
                table: "Students",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParentPhoneNumber",
                table: "Students");
        }
    }
}
