using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HDSS_BACKEND.Migrations
{
    /// <inheritdoc />
    public partial class mnbvcxzlkjhgfdsa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RegisteredStudents_Subjects_SubjectId",
                table: "RegisteredStudents");

            migrationBuilder.AlterColumn<int>(
                name: "SubjectId",
                table: "RegisteredStudents",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RegisteredStudents_Subjects_SubjectId",
                table: "RegisteredStudents",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RegisteredStudents_Subjects_SubjectId",
                table: "RegisteredStudents");

            migrationBuilder.AlterColumn<int>(
                name: "SubjectId",
                table: "RegisteredStudents",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_RegisteredStudents_Subjects_SubjectId",
                table: "RegisteredStudents",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id");
        }
    }
}
