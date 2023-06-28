using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HDSS_BACKEND.Migrations
{
    /// <inheritdoc />
    public partial class thirteenth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LessonNotes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TeacherId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TeacherName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateWritten = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Stage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    classSize = table.Column<int>(type: "int", nullable: true),
                    WeekStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    WeekEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Period = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Lesson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    strand = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    substrand = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    indicator = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    performanceIndicator = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    contentStandard = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    coreCompetence = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    keywords = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TLMS = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    references = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Day1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Day1Phase1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Day1Phase2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Day1Phase3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Day2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Day2Phase2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Day2Phase1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Day2Phase3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Day3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Day3Phase2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Day3Phase1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Day3Phase3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Day4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Day4Phase2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Day4Phase1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Day4Phase3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Day5 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Day5Phase2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Day5Phase1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Day5Phase3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HeadTeacherComment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HeadTeacherDateSigned = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LessonNotes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LessonNotesUpload",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TeacherId = table.Column<int>(type: "int", nullable: false),
                    TeacherName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateUploaded = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LessonNotePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HeadTeacherComment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HeadTeacherDateSigned = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LessonNotesUpload", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LessonNotes");

            migrationBuilder.DropTable(
                name: "LessonNotesUpload");
        }
    }
}
