using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
public DbSet<ChatMessage>ChatMessages { get; set; }
public DbSet<ClassAnnouncement>ClassAnnouncements{ get; set; }
public DbSet<AssignmentFile>AssignmentsFile{ get; set; }
public DbSet<Slide>Slides{ get; set; }
public DbSet <Video>Videos{ get; set; }
public DbSet <Audio>Audios{ get; set; }
public DbSet <Picture>Pics{ get; set; }
public DbSet<RegisteredStudent>RegisteredStudents{ get; set; }
public DbSet<RegisteredTeacher>RegisteredTeachers{ get; set; }






    }
}