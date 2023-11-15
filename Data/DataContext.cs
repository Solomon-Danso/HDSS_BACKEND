using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HDSS_BACKEND.HyChat.Models;
using HDSS_BACKEND.Models;
using Microsoft.EntityFrameworkCore;




namespace HDSS_BACKEND.Data
{
    public class DataContext:DbContext
    {
                //Empty constructor
public DataContext(): base(){
}

//Database Connection String
protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
base.OnConfiguring(optionsBuilder); optionsBuilder.UseSqlServer("Server=localhost,1433;Database=HDSS;User=sa;Password=HydotTech;TrustServerCertificate=true;");
}
//Data Set, where Project and User are models in the Model folder

public DbSet<Teacher> Teachers => Set<Teacher>(); 
 public DbSet<Student> Students { get; set; }
public DbSet<SchoolFeeTransaction> SchoolFeeTransactions { get; set; }
public DbSet<AmountOwing> AmountsOwing { get; set; }
public DbSet<AmountPaid> AmountsPaid { get; set; }
public DbSet<LessonNote> LessonNotes { get; set; }
public DbSet<LessonNoteUpload> LessonNotesUpload { get; set; }
public DbSet<Subject> Subjects { get; set; }

public DbSet<TeacherInSubject>TeacherInSubjects { get;set; }
public DbSet<StudentForSubject>StudentForSubjects { get;set; }
public DbSet<Classes> Classess { get; set; }
public DbSet<Video> Videos { get; set; }
public DbSet<Slide>Slides { get; set; }
public DbSet<Audio> Audios { get; set; }
public DbSet<Picture> Pictures { get; set; }
public DbSet<Book> Books { get; set; }
public DbSet<Calendar> Calendars { get;set; }
public DbSet<AnnouncementForStudent>AnnouncementForStudents { get; set; }
public DbSet<AnnouncementForTeachers>AnnouncementForTeachers { get; set; }
public DbSet<AnnouncementForPTA>AnnouncementForPTA { get; set; }
public DbSet<AnnoucementForHOD>AnnoucementForHOD { get; set; }
public DbSet<AnnoucementForSubject>AnnoucementForSubjects { get; set;}
public DbSet<Discussions>Discussions { get; set; }
public DbSet<Assignment> Assignments {get;set;}
public DbSet<AcademicYear>AcademicYears { get; set; }
public DbSet<AcademicTerm>AcademicTerms { get; set; }
public DbSet<SchoolDirector> SchoolDirectors { get; set; }
public DbSet<AuthenticationModel>AuthenticationModels { get; set; }
public DbSet<SuperiorAccount>SuperiorAccounts { get; set; }
public DbSet<Managers>Managers { get; set; }
public DbSet<TheEvent>TheEvents { get; set; }
public DbSet<ParentsModel> Parents {get; set;}
public DbSet<Instituition> Instituitions{get; set;}
public DbSet<AdmissionLetter> AdmissionLetters {get; set;}
public DbSet<Fee> Fees {get; set;}
public DbSet<AdmissionFee> AdmissionFees {get; set;}
public DbSet<BillingCard> BillingCards {get; set;}
public DbSet<Role> Roles {get; set;}
public DbSet<AuditTrial> AuditTrials {get;set;}
public DbSet<Admin> Admins {get;set;}

public DbSet<TimeTable> TimeTables {get; set;}
public DbSet<StudentNote> StudentNotes {get; set;}

public DbSet<GroupChat> Groups {get; set;}
public DbSet<SingleChat> Singles {get; set;}
public DbSet<GroupParticipant> GroupParticipants {get; set;}
public DbSet<GroupMessage> GroupMessages {get; set;}
public DbSet<UserPersonalMessage> UserPersonalMessages {get; set;}
public DbSet<TermResult> TermResults {get; set;}
public DbSet<AssignmentSolution> AssignmentSolutions {get; set;}











    }
}