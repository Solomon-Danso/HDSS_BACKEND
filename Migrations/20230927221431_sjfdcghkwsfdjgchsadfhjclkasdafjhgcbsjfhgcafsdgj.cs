using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HDSS_BACKEND.Migrations
{
    /// <inheritdoc />
    public partial class sjfdcghkwsfdjgchsadfhjclkasdafjhgcbsjfhgcafsdgj : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SuperiorID",
                table: "SuperiorAccounts",
                newName: "StaffID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StaffID",
                table: "SuperiorAccounts",
                newName: "SuperiorID");
        }
    }
}
