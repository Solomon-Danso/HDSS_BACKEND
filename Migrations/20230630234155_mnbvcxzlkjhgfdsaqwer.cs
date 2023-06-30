using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HDSS_BACKEND.Migrations
{
    /// <inheritdoc />
    public partial class mnbvcxzlkjhgfdsaqwer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssignmentsFile_Courses_CourseId",
                table: "AssignmentsFile");

            migrationBuilder.DropForeignKey(
                name: "FK_Audios_Courses_CourseId",
                table: "Audios");

            migrationBuilder.DropForeignKey(
                name: "FK_ChatMessages_Courses_CourseId",
                table: "ChatMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_ClassAnnouncements_Courses_CourseId",
                table: "ClassAnnouncements");

            migrationBuilder.DropForeignKey(
                name: "FK_Pics_Courses_CourseId",
                table: "Pics");

            migrationBuilder.DropForeignKey(
                name: "FK_Slides_Courses_CourseId",
                table: "Slides");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_Courses_CourseId",
                table: "Students");

            migrationBuilder.DropForeignKey(
                name: "FK_Teachers_Courses_CourseId",
                table: "Teachers");

            migrationBuilder.DropForeignKey(
                name: "FK_Videos_Courses_CourseId",
                table: "Videos");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.RenameColumn(
                name: "CourseId",
                table: "Videos",
                newName: "SubjectId");

            migrationBuilder.RenameIndex(
                name: "IX_Videos_CourseId",
                table: "Videos",
                newName: "IX_Videos_SubjectId");

            migrationBuilder.RenameColumn(
                name: "CourseId",
                table: "Teachers",
                newName: "SubjectId");

            migrationBuilder.RenameIndex(
                name: "IX_Teachers_CourseId",
                table: "Teachers",
                newName: "IX_Teachers_SubjectId");

            migrationBuilder.RenameColumn(
                name: "CourseId",
                table: "Students",
                newName: "SubjectId");

            migrationBuilder.RenameIndex(
                name: "IX_Students_CourseId",
                table: "Students",
                newName: "IX_Students_SubjectId");

            migrationBuilder.RenameColumn(
                name: "CourseId",
                table: "Slides",
                newName: "SubjectId");

            migrationBuilder.RenameIndex(
                name: "IX_Slides_CourseId",
                table: "Slides",
                newName: "IX_Slides_SubjectId");

            migrationBuilder.RenameColumn(
                name: "CourseId",
                table: "Pics",
                newName: "SubjectId");

            migrationBuilder.RenameIndex(
                name: "IX_Pics_CourseId",
                table: "Pics",
                newName: "IX_Pics_SubjectId");

            migrationBuilder.RenameColumn(
                name: "CourseId",
                table: "ClassAnnouncements",
                newName: "SubjectId");

            migrationBuilder.RenameIndex(
                name: "IX_ClassAnnouncements_CourseId",
                table: "ClassAnnouncements",
                newName: "IX_ClassAnnouncements_SubjectId");

            migrationBuilder.RenameColumn(
                name: "CourseId",
                table: "ChatMessages",
                newName: "SubjectId");

            migrationBuilder.RenameIndex(
                name: "IX_ChatMessages_CourseId",
                table: "ChatMessages",
                newName: "IX_ChatMessages_SubjectId");

            migrationBuilder.RenameColumn(
                name: "CourseId",
                table: "Audios",
                newName: "SubjectId");

            migrationBuilder.RenameIndex(
                name: "IX_Audios_CourseId",
                table: "Audios",
                newName: "IX_Audios_SubjectId");

            migrationBuilder.RenameColumn(
                name: "CourseId",
                table: "AssignmentsFile",
                newName: "SubjectId");

            migrationBuilder.RenameIndex(
                name: "IX_AssignmentsFile_CourseId",
                table: "AssignmentsFile",
                newName: "IX_AssignmentsFile_SubjectId");

            migrationBuilder.CreateTable(
                name: "Subjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubjectId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subjects", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_AssignmentsFile_Subjects_SubjectId",
                table: "AssignmentsFile",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Audios_Subjects_SubjectId",
                table: "Audios",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChatMessages_Subjects_SubjectId",
                table: "ChatMessages",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClassAnnouncements_Subjects_SubjectId",
                table: "ClassAnnouncements",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pics_Subjects_SubjectId",
                table: "Pics",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Slides_Subjects_SubjectId",
                table: "Slides",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Subjects_SubjectId",
                table: "Students",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Teachers_Subjects_SubjectId",
                table: "Teachers",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Videos_Subjects_SubjectId",
                table: "Videos",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssignmentsFile_Subjects_SubjectId",
                table: "AssignmentsFile");

            migrationBuilder.DropForeignKey(
                name: "FK_Audios_Subjects_SubjectId",
                table: "Audios");

            migrationBuilder.DropForeignKey(
                name: "FK_ChatMessages_Subjects_SubjectId",
                table: "ChatMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_ClassAnnouncements_Subjects_SubjectId",
                table: "ClassAnnouncements");

            migrationBuilder.DropForeignKey(
                name: "FK_Pics_Subjects_SubjectId",
                table: "Pics");

            migrationBuilder.DropForeignKey(
                name: "FK_Slides_Subjects_SubjectId",
                table: "Slides");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_Subjects_SubjectId",
                table: "Students");

            migrationBuilder.DropForeignKey(
                name: "FK_Teachers_Subjects_SubjectId",
                table: "Teachers");

            migrationBuilder.DropForeignKey(
                name: "FK_Videos_Subjects_SubjectId",
                table: "Videos");

            migrationBuilder.DropTable(
                name: "Subjects");

            migrationBuilder.RenameColumn(
                name: "SubjectId",
                table: "Videos",
                newName: "CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_Videos_SubjectId",
                table: "Videos",
                newName: "IX_Videos_CourseId");

            migrationBuilder.RenameColumn(
                name: "SubjectId",
                table: "Teachers",
                newName: "CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_Teachers_SubjectId",
                table: "Teachers",
                newName: "IX_Teachers_CourseId");

            migrationBuilder.RenameColumn(
                name: "SubjectId",
                table: "Students",
                newName: "CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_Students_SubjectId",
                table: "Students",
                newName: "IX_Students_CourseId");

            migrationBuilder.RenameColumn(
                name: "SubjectId",
                table: "Slides",
                newName: "CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_Slides_SubjectId",
                table: "Slides",
                newName: "IX_Slides_CourseId");

            migrationBuilder.RenameColumn(
                name: "SubjectId",
                table: "Pics",
                newName: "CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_Pics_SubjectId",
                table: "Pics",
                newName: "IX_Pics_CourseId");

            migrationBuilder.RenameColumn(
                name: "SubjectId",
                table: "ClassAnnouncements",
                newName: "CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_ClassAnnouncements_SubjectId",
                table: "ClassAnnouncements",
                newName: "IX_ClassAnnouncements_CourseId");

            migrationBuilder.RenameColumn(
                name: "SubjectId",
                table: "ChatMessages",
                newName: "CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_ChatMessages_SubjectId",
                table: "ChatMessages",
                newName: "IX_ChatMessages_CourseId");

            migrationBuilder.RenameColumn(
                name: "SubjectId",
                table: "Audios",
                newName: "CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_Audios_SubjectId",
                table: "Audios",
                newName: "IX_Audios_CourseId");

            migrationBuilder.RenameColumn(
                name: "SubjectId",
                table: "AssignmentsFile",
                newName: "CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_AssignmentsFile_SubjectId",
                table: "AssignmentsFile",
                newName: "IX_AssignmentsFile_CourseId");

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_AssignmentsFile_Courses_CourseId",
                table: "AssignmentsFile",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Audios_Courses_CourseId",
                table: "Audios",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChatMessages_Courses_CourseId",
                table: "ChatMessages",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClassAnnouncements_Courses_CourseId",
                table: "ClassAnnouncements",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pics_Courses_CourseId",
                table: "Pics",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Slides_Courses_CourseId",
                table: "Slides",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Courses_CourseId",
                table: "Students",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Teachers_Courses_CourseId",
                table: "Teachers",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Videos_Courses_CourseId",
                table: "Videos",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
