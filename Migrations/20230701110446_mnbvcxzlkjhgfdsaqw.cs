using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HDSS_BACKEND.Migrations
{
    /// <inheritdoc />
    public partial class mnbvcxzlkjhgfdsaqw : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropIndex(
                name: "IX_Teachers_SubjectId",
                table: "Teachers");

            migrationBuilder.DropIndex(
                name: "IX_Students_SubjectId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "SubjectId",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "SubjectId",
                table: "Students");

            migrationBuilder.AlterColumn<int>(
                name: "SubjectId",
                table: "Videos",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "SubjectId",
                table: "Slides",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "SubjectId",
                table: "Pics",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "SubjectId",
                table: "ClassAnnouncements",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "SubjectId",
                table: "ChatMessages",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "SubjectId",
                table: "Audios",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "SubjectId",
                table: "AssignmentsFile",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "RegisteredStudents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateAdded = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubjectId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegisteredStudents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegisteredStudents_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_RegisteredStudents_SubjectId",
                table: "RegisteredStudents",
                column: "SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_AssignmentsFile_Subjects_SubjectId",
                table: "AssignmentsFile",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Audios_Subjects_SubjectId",
                table: "Audios",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatMessages_Subjects_SubjectId",
                table: "ChatMessages",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ClassAnnouncements_Subjects_SubjectId",
                table: "ClassAnnouncements",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Pics_Subjects_SubjectId",
                table: "Pics",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Slides_Subjects_SubjectId",
                table: "Slides",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Videos_Subjects_SubjectId",
                table: "Videos",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id");
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
                name: "FK_Videos_Subjects_SubjectId",
                table: "Videos");

            migrationBuilder.DropTable(
                name: "RegisteredStudents");

            migrationBuilder.AlterColumn<int>(
                name: "SubjectId",
                table: "Videos",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SubjectId",
                table: "Teachers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SubjectId",
                table: "Students",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "SubjectId",
                table: "Slides",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SubjectId",
                table: "Pics",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SubjectId",
                table: "ClassAnnouncements",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SubjectId",
                table: "ChatMessages",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SubjectId",
                table: "Audios",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SubjectId",
                table: "AssignmentsFile",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_SubjectId",
                table: "Teachers",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_SubjectId",
                table: "Students",
                column: "SubjectId");

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
    }
}
