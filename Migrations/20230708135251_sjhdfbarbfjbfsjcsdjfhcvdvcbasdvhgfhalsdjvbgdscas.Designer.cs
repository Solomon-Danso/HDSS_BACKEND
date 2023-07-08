﻿// <auto-generated />
using System;
using HDSS_BACKEND.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace HDSS_BACKEND.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20230708135251_sjhdfbarbfjbfsjcsdjfhcvdvcbasdvhgfhalsdjvbgdscas")]
    partial class sjhdfbarbfjbfsjcsdjfhcvdvcbasdvhgfhalsdjvbgdscas
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("HDSS_BACKEND.Models.AmountOwing", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<double?>("Amount")
                        .HasColumnType("float");

                    b.Property<string>("DebtDate")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StudentId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StudentName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("AmountsOwing");
                });

            modelBuilder.Entity("HDSS_BACKEND.Models.AmountPaid", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<double?>("AmountDebtNew")
                        .HasColumnType("float");

                    b.Property<double?>("AmountDebtOld")
                        .HasColumnType("float");

                    b.Property<double?>("Amountpaid")
                        .HasColumnType("float");

                    b.Property<double?>("CreditAmount")
                        .HasColumnType("float");

                    b.Property<string>("PaymentDate")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StudentId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StudentName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("AmountsPaid");
                });

            modelBuilder.Entity("HDSS_BACKEND.Models.Audio", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AudioPath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClassName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DateAdded")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SubjectName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TeacherId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TeacherName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Audios");
                });

            modelBuilder.Entity("HDSS_BACKEND.Models.Classes", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClassName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DateAdded")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Classess");
                });

            modelBuilder.Entity("HDSS_BACKEND.Models.LessonNote", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("DateWritten")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Day1")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Day1Phase1")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Day1Phase2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Day1Phase3")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Day2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Day2Phase1")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Day2Phase2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Day2Phase3")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Day3")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Day3Phase1")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Day3Phase2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Day3Phase3")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Day4")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Day4Phase1")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Day4Phase2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Day4Phase3")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Day5")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Day5Phase1")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Day5Phase2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Day5Phase3")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HeadTeacherComment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HeadTeacherDateSigned")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Lesson")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NotesTicket")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Period")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SearchId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Stage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Subject")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TLMS")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TeacherId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TeacherName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("WeekEndDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("WeekStartDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("classSize")
                        .HasColumnType("int");

                    b.Property<string>("contentStandard")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("coreCompetence")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("indicator")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("keywords")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("performanceIndicator")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("references")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("strand")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("substrand")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("LessonNotes");
                });

            modelBuilder.Entity("HDSS_BACKEND.Models.LessonNoteUpload", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("DateUploaded")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HeadTeacherComment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HeadTeacherDateSigned")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LessonNotePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NotesTicket")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SearchId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TeacherId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TeacherName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("LessonNotesUpload");
                });

            modelBuilder.Entity("HDSS_BACKEND.Models.Picture", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClassName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DateAdded")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PicturePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SubjectName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TeacherId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TeacherName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Pictures");
                });

            modelBuilder.Entity("HDSS_BACKEND.Models.SchoolFeeTransaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<double?>("CreditAmount")
                        .HasColumnType("float");

                    b.Property<double?>("NewAmountOwing")
                        .HasColumnType("float");

                    b.Property<double?>("OldAmountOwing")
                        .HasColumnType("float");

                    b.Property<string>("PaymentDate")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StudentId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StudentName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("THEAmountPaid")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("SchoolFeeTransactions");
                });

            modelBuilder.Entity("HDSS_BACKEND.Models.Slide", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClassName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DateAdded")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SlidePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SubjectName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TeacherId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TeacherName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Slides");
                });

            modelBuilder.Entity("HDSS_BACKEND.Models.Student", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AdmissionDate")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BasicLevel")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DateOfBirth")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FathersContact")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FathersName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Gender")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("GuardianContact")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("GuardianName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HomeTown")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Location")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MedicalIInformation")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MothersContact")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MothersName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OtherName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProfilePic")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("SchoolBankAccount")
                        .HasColumnType("float");

                    b.Property<string>("StudentId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("amountOwing")
                        .HasColumnType("float");

                    b.Property<double?>("creditAmount")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("HDSS_BACKEND.Models.StudentForSubject", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClassName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StudentCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StudentID")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StudentName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SubjectName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("StudentForSubjects");
                });

            modelBuilder.Entity("HDSS_BACKEND.Models.Subject", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClassName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DateAdded")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SubjectName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Subjects");
                });

            modelBuilder.Entity("HDSS_BACKEND.Models.Teacher", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AppointmentLetter")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CertPath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("Credit")
                        .HasColumnType("float");

                    b.Property<string>("DateAdded")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DateOfBirth")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("Debit")
                        .HasColumnType("float");

                    b.Property<string>("Education")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EmergencyContacts")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FilePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Gender")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HealthStatus")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IdCards")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Location")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MaritalStatus")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NationalIds")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OtherName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordResetToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordResetTokenExpire")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SSNITNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Salary")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StaffID")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TaxNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TeachingExperience")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TwoStepsAuthToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TwoStepsAuthTokenExpire")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Teachers");
                });

            modelBuilder.Entity("HDSS_BACKEND.Models.TeacherForSubject", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClassName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StaffID")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StaffName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SubjectName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TeacherCode")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("TeacherForSubjects");
                });

            modelBuilder.Entity("HDSS_BACKEND.Models.Video", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClassName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DateAdded")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SubjectName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TeacherId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TeacherName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("VideoPath")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Videos");
                });
#pragma warning restore 612, 618
        }
    }
}
