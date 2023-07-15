using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HDSS_BACKEND.Migrations
{
    /// <inheritdoc />
    public partial class poohsfjhvnsdjfgchjfhgsvbdjnvnsfnbfsdjvfbbvhdvdfmvhdbfjmbfdvbasghbdsmvbdxdfnvbd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BasicLevel",
                table: "Students",
                newName: "Level");

            migrationBuilder.AddColumn<string>(
                name: "TheId",
                table: "Discussions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TheId",
                table: "AnnouncementForTeachers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TheId",
                table: "AnnouncementForStudents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TheId",
                table: "AnnouncementForPTA",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TheId",
                table: "AnnoucementForSubjects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TheId",
                table: "AnnoucementForHOD",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TheId",
                table: "Discussions");

            migrationBuilder.DropColumn(
                name: "TheId",
                table: "AnnouncementForTeachers");

            migrationBuilder.DropColumn(
                name: "TheId",
                table: "AnnouncementForStudents");

            migrationBuilder.DropColumn(
                name: "TheId",
                table: "AnnouncementForPTA");

            migrationBuilder.DropColumn(
                name: "TheId",
                table: "AnnoucementForSubjects");

            migrationBuilder.DropColumn(
                name: "TheId",
                table: "AnnoucementForHOD");

            migrationBuilder.RenameColumn(
                name: "Level",
                table: "Students",
                newName: "BasicLevel");
        }
    }
}
