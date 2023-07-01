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
}