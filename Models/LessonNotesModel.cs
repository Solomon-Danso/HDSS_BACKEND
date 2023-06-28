using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HDSS_BACKEND.Models
{
    public class LessonNote
    {
        public int Id { get; set; }
        public string? TeacherId { get; set; }
        public string? TeacherName { get; set; }
        public string? DateWritten { get; set; }
        public string? Subject { get; set; }
        public string? Stage { get; set; }
        public int? classSize { get; set; }
        public DateTime? WeekStartDate { get; set; }
        public DateTime? WeekEndDate { get; set; }
        public string? Period { get; set; }
        public string? Lesson{ get; set; }
        public string? strand { get; set; }
        public string? substrand { get; set; }
        public string? indicator { get; set; }
        public string? performanceIndicator { get; set; }
        public string? contentStandard { get; set; }
        public string? coreCompetence { get; set; }
        public string? keywords { get; set; }
        public string? TLMS { get; set; }
        public string? references { get; set; }
        public string? Day1 { get; set; }
        public string? Day1Phase1 { get; set; }
        public string? Day1Phase2 { get; set; }
        public string? Day1Phase3 { get; set; }

        public string? Day2 { get; set; }
        public string? Day2Phase2 { get; set; }
        public string? Day2Phase1 { get; set; }
        public string? Day2Phase3 { get; set; }

        public string? Day3 { get; set; }
        public string? Day3Phase2 { get; set; }
        public string? Day3Phase1 { get; set; }
        public string? Day3Phase3 { get; set; }


        public string? Day4 { get; set; }
        public string? Day4Phase2 { get; set; }
        public string? Day4Phase1 { get; set; }
        public string? Day4Phase3 { get; set; }

        public string? Day5 { get; set; }
        public string? Day5Phase2 { get; set; }
        public string? Day5Phase1 { get; set; }
        public string? Day5Phase3 { get; set; }

        public string? HeadTeacherComment { get; set; }
        public DateTime? HeadTeacherDateSigned { get; set; }


    }

    public class LessonNoteUpload{
        public int Id { get; set; }
        public int TeacherId { get; set; }
        public string? TeacherName { get; set; }
        public string? DateUploaded{ get; set; }
        public string? LessonNotePath { get; set; }

         public string? HeadTeacherComment { get; set; }
        public DateTime? HeadTeacherDateSigned { get; set; }


    }

        public class LessonNoteUploadDto{
        public int Id { get; set; }
        public int TeacherId { get; set; }
        public string? TeacherName { get; set; }
        public string? DateUploaded{ get; set; }
        public IFormFile? File { get; set; }

         public string? HeadTeacherComment { get; set; }
        public DateTime? HeadTeacherDateSigned { get; set; }


    }


}