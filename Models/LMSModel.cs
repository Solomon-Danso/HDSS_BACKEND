using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HDSS_BACKEND.Models
{
    public class Subject
    {
        public int Id { get; set; }
        public string? SubjectName { get; set; }
       
        public string? DateAdded { get; set; }

    }

    public class Classes
    {
        public int Id { get; set; }
        public string? ClassName { get; set; }
        public string? ClassCode {get;set;}
        public string? Campus {get;set;}
        public string? ClassTeacher{get;set;}
        public string? DateAdded { get; set; }

    }

    public class TeacherForSubject{
        public int Id { get; set; }
        public string? StaffID { get; set; }
        public string? StaffName { get; set; }
        public string? TeacherCode { get; set; }
        public string? SubjectName { get; set; }
          public string? ClassName { get; set; }
          public string? DateAssigned {get;set;}
    }

    public class TeacherInSubject{
        public int Id { get; set; }
        public string? StaffID { get; set; }
        public string? StaffName { get; set; }
        public string? ClassName {get;set;}

        public string? SubjectName { get; set; }
        
          public string? DateAssigned {get;set;}
    }

    public class StudentForSubject{
        public int Id { get; set; }
        public string? StudentID { get; set; }
        public string? StudentName { get; set; }
        public string? StudentCode { get; set; }
        public string? SubjectName { get; set; }
          public string? ClassName { get; set; }
    }

    public class AcademicYear{
        public int Id { get; set; }
        public string? academicYear { get; set; }
    }
    public class AcademicTerm{
        public int Id { get; set; }
        public string? academicTerm { get; set; }
    }

    public class Slide{
        public int Id { get; set; }
        public string? SubjectName { get; set; }
        public string? Title { get; set; }
        public string? ClassName { get; set; }
        public string? SlidePath { get; set; }
        public string? DateAdded{ get; set; }
        public string? AcademicYear{ get; set; }
        public string? AcademicTerm{ get; set; }
        public string? TeacherId { get; set; }
        public string? TeacherName { get; set; }

    }

     public class SlidesDto{
  
        public string? Title { get; set; }
        public string? AcademicYear{ get; set; }
        public string? AcademicTerm{ get; set; }
        public IFormFile? Slide { get; set; }

    }


      public class Video{
        public int Id { get; set; }
        public string? SubjectName { get; set; }
        public string? Title { get; set; }
        public string? ClassName { get; set; }
        public string? VideoPath { get; set; }
        public string? DateAdded{ get; set; }
        public string? TeacherId { get; set; }
        public string? TeacherName { get; set; }
              public string? AcademicYear{ get; set; }
        public string? AcademicTerm{ get; set; }

    }

     public class VideoDto{
  
        public string? Title { get; set; }
          public string? AcademicYear{ get; set; }
        public string? AcademicTerm{ get; set; }
        public IFormFile? Video { get; set; }

    }

      public class Audio{
        public int Id { get; set; }
        public string? SubjectName { get; set; }
        public string? Title { get; set; }
        public string? ClassName { get; set; }
        public string? AudioPath { get; set; }
        public string? DateAdded{ get; set; }
        public string? TeacherId { get; set; }
              public string? AcademicYear{ get; set; }
        public string? AcademicTerm{ get; set; }
        public string? TeacherName { get; set; }

    }

     public class AudioDto{
  
        public string? Title { get; set; }
              public string? AcademicYear{ get; set; }
        public string? AcademicTerm{ get; set; }
    
        public IFormFile? Audio { get; set; }

    }



      public class Picture{
        public int Id { get; set; }
        public string? SubjectName { get; set; }
        public string? Title { get; set; }
        public string? ClassName { get; set; }
        public string? PicturePath { get; set; }
        public string? DateAdded{ get; set; }
        public string? TeacherId { get; set; }
              public string? AcademicYear{ get; set; }
        public string? AcademicTerm{ get; set; }
        public string? TeacherName { get; set; }

    }

     public class PictureDto{
  
        public string? Title { get; set; }
              public string? AcademicYear{ get; set; }
        public string? AcademicTerm{ get; set; }
    
        public IFormFile? Picture { get; set; }

    }



      public class Syllabus{
        public int Id { get; set; }
        public string? SubjectName { get; set; }
        public string? Title { get; set; }
        public string? ClassName { get; set; }
        public string? SyllabusPath { get; set; }
        public string? DateAdded{ get; set; }
        public string? TeacherId { get; set; }
        public string? TeacherName { get; set; }
              public string? AcademicYear{ get; set; }
        public string? AcademicTerm{ get; set; }

    }

     public class SyllabusDto{
  
        public string? Title { get; set; }
              public string? AcademicYear{ get; set; }
        public string? AcademicTerm{ get; set; }
    
        public IFormFile? Syllabus { get; set; }

    }


      public class Calendar{
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? ClassName { get; set; }
        public string? CalendarPath { get; set; }
        public string? DateAdded{ get; set; }
        public string? TeacherId { get; set; }
        public string? TeacherName { get; set; }
              public string? AcademicYear{ get; set; }
        public string? AcademicTerm{ get; set; }

    }

     public class CalendarDto{
  
        public string? Title { get; set; }
           public string? AcademicYear{ get; set; }
        public string? AcademicTerm{ get; set; }

        public IFormFile? Calendar { get; set; }

    }

    public class AnnouncementForStudent{
        public int Id { get; set; }
        public string? Subject { get; set; }
            public string? Content { get; set; }
    public string? TheId { get; set; }
        public string? DateAdded { get; set; }
        public string? AcademicYear{ get; set; }
        public string? AcademicTerm{ get; set; }

    }

    public class AnnouncementForTeachers{
        public int Id { get; set; }
        public string? Subject { get; set; }
            public string? Content { get; set; }
    public string? TheId { get; set; }
        public string? DateAdded { get; set; }
               public string? AcademicYear{ get; set; }
        public string? AcademicTerm{ get; set; }

    }

    public class AnnouncementForPTA{
         public int Id { get; set; }        public string? AcademicYear{ get; set; }
        public string? AcademicTerm{ get; set; }

        public string? Subject { get; set; }
            public string? Content { get; set; }
    public string? TheId { get; set; }
        public string? DateAdded { get; set; }
    }

    public class AnnoucementForHOD{
        public int Id { get; set; }        public string? AcademicYear{ get; set; }
        public string? AcademicTerm{ get; set; }

        public string? Subject { get; set; }
            public string? Content { get; set; }
    public string? TheId { get; set; }
        public string? DateAdded { get; set; }
    }

    

      public class AnnoucementForSubject{
        public int Id { get; set; }        public string? AcademicYear{ get; set; }
        public string? AcademicTerm{ get; set; }

         public string? SubjectName { get; set; }
         public string? Title { get; set; }
            public string? Content { get; set; }
    public string? TheId { get; set; }
        public string? DateAdded { get; set; }
       public string? ClassName { get; set; }
        public string? TeacherId { get; set; }
        public string? TeacherName { get; set; }

    }

public class Discussions{
    public int Id { get; set; }     
       public string? AcademicYear{ get; set; }
        public string? AcademicTerm{ get; set; }

    public string? SenderName { get; set; }
        public string? Content { get; set; }
    public string? TheId { get; set; }
    public string? SenderId { get; set; }
    public string? DateSent { get; set; }
    public string? Subject { get; set; }
     public string? ClassName { get; set; }

}

public class DiscussionsForStudent{
    public string? StudentName { get; set; }
        public string? Content { get; set; }
    public string? TheId { get; set; }
    public string? StudentId { get; set; }
    public string? DateSent { get; set; }
    public string? Subject { get; set; }
    public string? ClassName { get; set; }

}



public class DiscussionsForTeacher{
     public string? ClassName { get; set; }
    public string? TeacherName { get; set; }
        public string? Content { get; set; }
    public string? TheId { get; set; }
    public string? TeacherId { get; set; }
    public string? DateSent { get; set; }
    public string? Subject { get; set; }

}




     public class Assignment{
        public int Id { get; set; }      
        public string? AcademicTerm{ get; set; }
         public string? AcademicYear{ get; set; }
        public string? SubjectName { get; set; }
        public string? ClassName { get; set; }
        public string? AssignmentPath { get; set; }
        public DateTime? StartDate{ get; set; }
        public DateTime? ExpireDate { get; set; }
        public string? TeacherId { get; set; }
        public string? TeacherName { get; set; }
        public string? AssignmentNumber { get; set; }
        public string? AssignmentToken { get; set; }

    }

     public class AssignmentDto{
  
        public DateTime? StartDate{ get; set; }
        public DateTime? ExpireDate { get; set; }
        public string? AcademicTerm{ get; set; }
        public string? AcademicYear{ get; set; }
        public IFormFile? AssignmentFile { get; set; }
         public string? AssignmentNumber { get; set; }

    }


public class AssignmentSubmission{
        public int Id { get; set; }     
           public string? AcademicYear{ get; set; }
        public string? AcademicTerm{ get; set; }

        public string? SubjectName { get; set; }
        public string? ClassName { get; set; }
        public string? AssignmentPath { get; set; }
        public string? uploadDate{ get; set; }
        public string? StudentId { get; set; }
        public string? StudentName { get; set; }
        public string? AssignmentCode { get; set; }
        public string? SubmissionPath { get; set; }
        public string? AssignmentToken { get; set; }
    }

     public class AssignmentSubmissionDto{
  
     
        public IFormFile? AssignmentFile { get; set; }
        public string? AcademicYear{ get; set; }
        public string? AcademicTerm{ get; set; }


    }









}