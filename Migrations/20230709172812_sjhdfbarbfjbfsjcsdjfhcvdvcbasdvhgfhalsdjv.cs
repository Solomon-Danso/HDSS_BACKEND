using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HDSS_BACKEND.Migrations
{
    /// <inheritdoc />
    public partial class sjhdfbarbfjbfsjcsdjfhcvdvcbasdvhgfhalsdjv : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AssignmentCode",
                table: "Assignments",
                newName: "AssignmentToken");

            migrationBuilder.AddColumn<string>(
                name: "AcademicTerm",
                table: "Videos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AcademicYear",
                table: "Videos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AcademicTerm",
                table: "Syllabuss",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AcademicYear",
                table: "Syllabuss",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AcademicTerm",
                table: "Pictures",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AcademicYear",
                table: "Pictures",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AcademicTerm",
                table: "Discussions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AcademicYear",
                table: "Discussions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AcademicTerm",
                table: "Calendars",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AcademicYear",
                table: "Calendars",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AcademicTerm",
                table: "Audios",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AcademicYear",
                table: "Audios",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AcademicTerm",
                table: "AssignmentSubmissions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AcademicYear",
                table: "AssignmentSubmissions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AssignmentToken",
                table: "AssignmentSubmissions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AcademicTerm",
                table: "Assignments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AcademicYear",
                table: "Assignments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AssignmentNumber",
                table: "Assignments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AcademicTerm",
                table: "AnnouncementForTeachers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AcademicYear",
                table: "AnnouncementForTeachers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AcademicTerm",
                table: "AnnouncementForStudents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AcademicYear",
                table: "AnnouncementForStudents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AcademicTerm",
                table: "AnnouncementForPTA",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AcademicYear",
                table: "AnnouncementForPTA",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AcademicTerm",
                table: "AnnoucementForSubjects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AcademicYear",
                table: "AnnoucementForSubjects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AcademicTerm",
                table: "AnnoucementForHOD",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AcademicYear",
                table: "AnnoucementForHOD",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AcademicTerm",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "AcademicYear",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "AcademicTerm",
                table: "Syllabuss");

            migrationBuilder.DropColumn(
                name: "AcademicYear",
                table: "Syllabuss");

            migrationBuilder.DropColumn(
                name: "AcademicTerm",
                table: "Pictures");

            migrationBuilder.DropColumn(
                name: "AcademicYear",
                table: "Pictures");

            migrationBuilder.DropColumn(
                name: "AcademicTerm",
                table: "Discussions");

            migrationBuilder.DropColumn(
                name: "AcademicYear",
                table: "Discussions");

            migrationBuilder.DropColumn(
                name: "AcademicTerm",
                table: "Calendars");

            migrationBuilder.DropColumn(
                name: "AcademicYear",
                table: "Calendars");

            migrationBuilder.DropColumn(
                name: "AcademicTerm",
                table: "Audios");

            migrationBuilder.DropColumn(
                name: "AcademicYear",
                table: "Audios");

            migrationBuilder.DropColumn(
                name: "AcademicTerm",
                table: "AssignmentSubmissions");

            migrationBuilder.DropColumn(
                name: "AcademicYear",
                table: "AssignmentSubmissions");

            migrationBuilder.DropColumn(
                name: "AssignmentToken",
                table: "AssignmentSubmissions");

            migrationBuilder.DropColumn(
                name: "AcademicTerm",
                table: "Assignments");

            migrationBuilder.DropColumn(
                name: "AcademicYear",
                table: "Assignments");

            migrationBuilder.DropColumn(
                name: "AssignmentNumber",
                table: "Assignments");

            migrationBuilder.DropColumn(
                name: "AcademicTerm",
                table: "AnnouncementForTeachers");

            migrationBuilder.DropColumn(
                name: "AcademicYear",
                table: "AnnouncementForTeachers");

            migrationBuilder.DropColumn(
                name: "AcademicTerm",
                table: "AnnouncementForStudents");

            migrationBuilder.DropColumn(
                name: "AcademicYear",
                table: "AnnouncementForStudents");

            migrationBuilder.DropColumn(
                name: "AcademicTerm",
                table: "AnnouncementForPTA");

            migrationBuilder.DropColumn(
                name: "AcademicYear",
                table: "AnnouncementForPTA");

            migrationBuilder.DropColumn(
                name: "AcademicTerm",
                table: "AnnoucementForSubjects");

            migrationBuilder.DropColumn(
                name: "AcademicYear",
                table: "AnnoucementForSubjects");

            migrationBuilder.DropColumn(
                name: "AcademicTerm",
                table: "AnnoucementForHOD");

            migrationBuilder.DropColumn(
                name: "AcademicYear",
                table: "AnnoucementForHOD");

            migrationBuilder.RenameColumn(
                name: "AssignmentToken",
                table: "Assignments",
                newName: "AssignmentCode");
        }
    }
}
