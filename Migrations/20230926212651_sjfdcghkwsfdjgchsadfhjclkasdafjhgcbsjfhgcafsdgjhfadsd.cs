using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HDSS_BACKEND.Migrations
{
    /// <inheritdoc />
    public partial class sjfdcghkwsfdjgchsadfhjclkasdafjhgcbsjfhgcafsdgjhfadsd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Action",
                table: "BillingCards",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Bills",
                table: "BillingCards",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Action",
                table: "BillingCards");

            migrationBuilder.DropColumn(
                name: "Bills",
                table: "BillingCards");
        }
    }
}
