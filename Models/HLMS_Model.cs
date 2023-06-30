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
        
        public ICollection<Teacher>? Teachers { get; set; }
        public ICollection<Student>? Students { get; set; }
        public ICollection<Slide>? Slides { get; set; }
        //More solutions later
        public ICollection<AssignmentFile>? AssignmentsFile { get; set; }
        public ICollection<Video>? Videos { get; set; }
        public ICollection<ChatMessage>? ChatMessages { get; set; }
        public ICollection<ClassAnnouncement>? ClassAnnouncements { get; set; }
        public ICollection<Audio>? Audios { get; set;}
        public ICollection<Picture>? Pics {get; set;}
        
    }



public class ChatMessage{
           public int Id { get; set; }         
           public string? DateUploaded { get; set; }
           public string? Message { get; set; }
           public int SubjectId { get; set; }

    // Navigation property
    public Subject? Subject { get; set; }
        }

public class ClassAnnouncement{
           public int Id { get; set; }         
           public string? DateUploaded { get; set; }
           public string? Message { get; set; }
           public int SubjectId { get; set; }

    // Navigation property
    public Subject? Subject { get; set; }
        }

     public class AssignmentFile{
           public int Id { get; set; }
           public string? Title { get; set; }
           public string? DateUploaded { get; set; }
           public string? AssignmentFilePath { get; set; }
           public int SubjectId { get; set; }

    // Navigation property
    public Subject? Subject { get; set; }
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
           public int SubjectId { get; set; }

    // Navigation property
    public Subject? Subject { get; set; }
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
           public int SubjectId { get; set; }

    // Navigation property
    public Subject? Subject { get; set; }
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
           public int SubjectId { get; set; }

    // Navigation property
    public Subject? Subject { get; set; }
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
           public int SubjectId { get; set; }

    // Navigation property
    public Subject? Subject { get; set; }
        }
    public class PictureDto{
        public int Id { get; set; }
           public string? Title { get; set; }
           public string? DateUploaded { get; set; }
           public IFormFile? Picture { get; set; }
    }



}