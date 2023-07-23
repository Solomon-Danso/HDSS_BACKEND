using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HDSS_BACKEND.Migrations.OnlineData
{
    /// <inheritdoc />
    public partial class sdgfkvbdfdfvmdnfvkjbfdjkgncsd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AcademicTerms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    academicTerm = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcademicTerms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AcademicYears",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    academicYear = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcademicYears", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AmountsOwing",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<double>(type: "float", nullable: true),
                    DebtDate = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AmountsOwing", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AmountsPaid",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AmountDebtOld = table.Column<double>(type: "float", nullable: true),
                    Amountpaid = table.Column<double>(type: "float", nullable: true),
                    CreditAmount = table.Column<double>(type: "float", nullable: true),
                    AmountDebtNew = table.Column<double>(type: "float", nullable: true),
                    PaymentDate = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AmountsPaid", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AnnoucementForHOD",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AcademicYear = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AcademicTerm = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TheId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateAdded = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnnoucementForHOD", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AnnoucementForSubjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AcademicYear = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AcademicTerm = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubjectName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TheId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateAdded = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClassName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TeacherId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TeacherName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnnoucementForSubjects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AnnouncementForPTA",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AcademicYear = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AcademicTerm = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TheId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateAdded = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnnouncementForPTA", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AnnouncementForStudents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TheId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateAdded = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AcademicYear = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AcademicTerm = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnnouncementForStudents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AnnouncementForTeachers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TheId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateAdded = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AcademicYear = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AcademicTerm = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnnouncementForTeachers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Assignments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AcademicTerm = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AcademicYear = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubjectName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClassName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AssignmentPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExpireDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TeacherId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TeacherName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AssignmentNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AssignmentToken = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assignments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AssignmentSubmissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AcademicYear = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AcademicTerm = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubjectName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClassName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AssignmentPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    uploadDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AssignmentCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubmissionPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AssignmentToken = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssignmentSubmissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Audios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubjectName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClassName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AudioPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateAdded = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TeacherId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AcademicYear = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AcademicTerm = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TeacherName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Audios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuthenticationModels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserPassword = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TwoSteps = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TwoStepsExpire = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PasswordToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PasswordTokenExpire = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthenticationModels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Calendars",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClassName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CalendarPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateAdded = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TeacherId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TeacherName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AcademicYear = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AcademicTerm = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Calendars", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Classess",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClassName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateAdded = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classess", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClassScores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubjectName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClassName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Assignmentnumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Score = table.Column<double>(type: "float", nullable: true),
                    TeacherId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TeacherName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AcademicTerm = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AcademicYear = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateScored = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassScores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClassScoresSummarys",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClassName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubjectName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalScore = table.Column<double>(type: "float", nullable: true),
                    AcademicTerm = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AcademicYear = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassScoresSummarys", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Discussions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AcademicYear = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AcademicTerm = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SenderName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TheId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SenderId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateSent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClassName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Discussions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LessonNotes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NotesTicket = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    HeadTeacherDateSigned = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SearchId = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                    NotesTicket = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TeacherId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TeacherName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateUploaded = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LessonNotePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SearchId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HeadTeacherComment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HeadTeacherDateSigned = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LessonNotesUpload", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pictures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubjectName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClassName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PicturePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateAdded = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TeacherId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AcademicYear = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AcademicTerm = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TeacherName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pictures", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SchoolDirectors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DirectorID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Contact = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TwoSteps = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TwoStepsExpire = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PasswordToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PasswordTokenExpire = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolDirectors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SchoolFeeTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OldAmountOwing = table.Column<double>(type: "float", nullable: true),
                    CreditAmount = table.Column<double>(type: "float", nullable: true),
                    THEAmountPaid = table.Column<double>(type: "float", nullable: true),
                    NewAmountOwing = table.Column<double>(type: "float", nullable: true),
                    PaymentDate = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolFeeTransactions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Slides",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubjectName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClassName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SlidePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateAdded = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AcademicYear = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AcademicTerm = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TeacherId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TeacherName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Slides", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StudentForSubjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubjectName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClassName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentForSubjects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OtherName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfBirth = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HomeTown = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FathersName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FathersContact = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MothersName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MothersContact = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GuardianName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GuardianContact = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MedicalIInformation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Level = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    amountOwing = table.Column<double>(type: "float", nullable: true),
                    creditAmount = table.Column<double>(type: "float", nullable: true),
                    AdmissionDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SchoolBankAccount = table.Column<double>(type: "float", nullable: true),
                    ProfilePic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Subjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubjectName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClassName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateAdded = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subjects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Syllabuss",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubjectName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClassName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SyllabusPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateAdded = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TeacherId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TeacherName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AcademicYear = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AcademicTerm = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Syllabuss", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TeacherForSubjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StaffID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StaffName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TeacherCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubjectName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClassName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherForSubjects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Teachers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OtherName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfBirth = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaritalStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Education = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TeachingExperience = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NationalIds = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TaxNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SSNITNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HealthStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmergencyContacts = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Salary = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Debit = table.Column<double>(type: "float", nullable: true),
                    Credit = table.Column<double>(type: "float", nullable: true),
                    StaffID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CertPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AppointmentLetter = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdCards = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateAdded = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PasswordResetToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PasswordResetTokenExpire = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TwoStepsAuthToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TwoStepsAuthTokenExpire = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teachers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Videos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubjectName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClassName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VideoPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateAdded = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TeacherId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TeacherName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AcademicYear = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AcademicTerm = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Videos", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AcademicTerms");

            migrationBuilder.DropTable(
                name: "AcademicYears");

            migrationBuilder.DropTable(
                name: "AmountsOwing");

            migrationBuilder.DropTable(
                name: "AmountsPaid");

            migrationBuilder.DropTable(
                name: "AnnoucementForHOD");

            migrationBuilder.DropTable(
                name: "AnnoucementForSubjects");

            migrationBuilder.DropTable(
                name: "AnnouncementForPTA");

            migrationBuilder.DropTable(
                name: "AnnouncementForStudents");

            migrationBuilder.DropTable(
                name: "AnnouncementForTeachers");

            migrationBuilder.DropTable(
                name: "Assignments");

            migrationBuilder.DropTable(
                name: "AssignmentSubmissions");

            migrationBuilder.DropTable(
                name: "Audios");

            migrationBuilder.DropTable(
                name: "AuthenticationModels");

            migrationBuilder.DropTable(
                name: "Calendars");

            migrationBuilder.DropTable(
                name: "Classess");

            migrationBuilder.DropTable(
                name: "ClassScores");

            migrationBuilder.DropTable(
                name: "ClassScoresSummarys");

            migrationBuilder.DropTable(
                name: "Discussions");

            migrationBuilder.DropTable(
                name: "LessonNotes");

            migrationBuilder.DropTable(
                name: "LessonNotesUpload");

            migrationBuilder.DropTable(
                name: "Pictures");

            migrationBuilder.DropTable(
                name: "SchoolDirectors");

            migrationBuilder.DropTable(
                name: "SchoolFeeTransactions");

            migrationBuilder.DropTable(
                name: "Slides");

            migrationBuilder.DropTable(
                name: "StudentForSubjects");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Subjects");

            migrationBuilder.DropTable(
                name: "Syllabuss");

            migrationBuilder.DropTable(
                name: "TeacherForSubjects");

            migrationBuilder.DropTable(
                name: "Teachers");

            migrationBuilder.DropTable(
                name: "Videos");
        }
    }
}
