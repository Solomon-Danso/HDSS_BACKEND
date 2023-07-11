using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HDSS_BACKEND.Models
{
    public class ClassScore
    {
        public int Id { get; set; }
        public string? StudentId { get; set; }
        public string? StudentName { get; set; }
        public string? SubjectName { get; set; }
        public string? ClassName { get; set; }
        public string? Assignmentnumber { get; set; }
        public double? Score { get; set; }
        public string? TeacherId { get; set; }
        public string? TeacherName { get; set; }
        public string? AcademicTerm { get; set; }
        public string? AcademicYear { get; set; }
        public string? Token { get; set; }
        public string? DateScored { get; set; }
    }
    public class ClassScoreSummary{
        public int Id { get; set;}
        public string? StudentId { get; set;}
        public string? StudentName { get; set;}
        public string? ClassName { get; set; }
        public string? SubjectName { get; set;}
         public double? TotalScore { get; set; }
        public string? AcademicTerm { get; set; }
        public string? AcademicYear { get; set; }
        public string? Token { get; set; }

     

    }
}