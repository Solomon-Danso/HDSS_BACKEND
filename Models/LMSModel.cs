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
        public string? ClassName { get; set; }
        public string? DateAdded { get; set; }

    }

    public class Classes
    {
        public int Id { get; set; }
        public string? ClassName { get; set; }
        public string? DateAdded { get; set; }

    }

    public class TeacherForSubject{
        public int Id { get; set; }
        public string? StaffID { get; set; }
        public string? StaffName { get; set; }
        public string? TeacherCode { get; set; }
        public string? SubjectName { get; set; }
          public string? ClassName { get; set; }
    }

    public class StudentForSubject{
        public int Id { get; set; }
        public string? StudentID { get; set; }
        public string? StudentName { get; set; }
        public string? StudentCode { get; set; }
        public string? SubjectName { get; set; }
          public string? ClassName { get; set; }
    }

    public class Slide{
        public int Id { get; set; }
        public string? SubjectName { get; set; }
        public string? Title { get; set; }
        public string? ClassName { get; set; }
        public string? SlidePath { get; set; }
        public string? DateAdded{ get; set; }
        public string? TeacherId { get; set; }
        public string? TeacherName { get; set; }

    }

     public class SlidesDto{
  
        public string? Title { get; set; }
    
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

    }

     public class VideoDto{
  
        public string? Title { get; set; }
    
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
        public string? TeacherName { get; set; }

    }

     public class AudioDto{
  
        public string? Title { get; set; }
    
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
        public string? TeacherName { get; set; }

    }

     public class PictureDto{
  
        public string? Title { get; set; }
    
        public IFormFile? Picture { get; set; }

    }






}