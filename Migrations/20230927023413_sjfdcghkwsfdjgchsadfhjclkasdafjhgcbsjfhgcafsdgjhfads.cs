using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HDSS_BACKEND.Migrations
{
    /// <inheritdoc />
    public partial class sjfdcghkwsfdjgchsadfhjclkasdafjhgcbsjfhgcafsdgjhfads : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Position",
                table: "Instituitions",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Position",
                table: "Instituitions");
        }
    }
}
