using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HDSS_BACKEND.Models
{
    public class Subject
    {
        public int Id { get; set; }
        public string? SubjectId { get; set; }
        
         public List<RegisteredStudent> Students { get; set; } = new List<RegisteredStudent>();
         public List<RegisteredTeacher> Teachers { get; set; } = new List<RegisteredTeacher>();

        public List<Slide>? Slides { get; set; }
        //More solutions later
        public List<AssignmentFile>? AssignmentsFile { get; set; }
        public List<Video>? Videos { get; set; }
        public List<ChatMessage>? ChatMessages { get; set; }
        public List<ClassAnnouncement>? ClassAnnouncements { get; set; }
        public List<Audio>? Audios { get; set;}
        public List<Picture>? Pics {get; set;}
        
    }


public class RegisteredStudent{
   public int Id { get; set; }
   public string? StudentName { get; set; }
   public string? StudentId { get; set; }
   public string? DateAdded { get; set; }

}


public class RegisteredTeacher{
   public int Id { get; set; }
   public string? TeacherName { get; set; }
   public string? TeacherId { get; set; }
   public string? DateAdded { get; set; }

}


public class ChatMessage{
           public int Id { get; set; }         
           public string? DateUploaded { get; set; }
           public string? Message { get; set; }
 
        }

public class ClassAnnouncement{
           public int Id { get; set; }         
           public string? DateUploaded { get; set; }
           public string? Message { get; set; }
 
        }

     public class AssignmentFile{
           public int Id { get; set; }
           public string? Title { get; set; }
           public string? DateUploaded { get; set; }
           public string? AssignmentFilePath { get; set; }
 
        }
    public class AssignmentFileDto{
        public int Id { get; set; }
           public string? Title { get; set; }
           public string? DateUploaded { get; set; }
           public IFormFile? AssignmentFilePath { get; set; }
    }


     public class Slide{
           public int Id { get; set; }
           public string? Title { get; set; }
           public string? DateUploaded { get; set; }
           public string? SlidePath { get; set; }
 
        }
    public class SlideDto{
        public int Id { get; set; }
           public string? Title { get; set; }
           public string? DateUploaded { get; set; }
           public IFormFile? Slide { get; set; }
    }


 public class Video{
           public int Id { get; set; }
           public string? Title { get; set; }
           public string? DateUploaded { get; set; }
           public string? VideoPath { get; set; }
 
        }
    public class VideoDto{
        public int Id { get; set; }
           public string? Title { get; set; }
           public string? DateUploaded { get; set; }
           public IFormFile? Video { get; set; }
    }


 public class Audio{
           public int Id { get; set; }
           public string? Title { get; set; }
           public string? DateUploaded { get; set; }
           public string? AudioPath { get; set; }
 
        }
    public class AudioDto{
        public int Id { get; set; }
           public string? Title { get; set; }
           public string? DateUploaded { get; set; }
           public IFormFile? Audio { get; set; }
    }



 public class Picture{
           public int Id { get; set; }
           public string? Title { get; set; }
           public string? DateUploaded { get; set; }
           public string? PicturePath { get; set; }
 
        }
    public class PictureDto{
        public int Id { get; set; }
           public string? Title { get; set; }
           public string? DateUploaded { get; set; }
           public IFormFile? Picture { get; set; }
    }



}