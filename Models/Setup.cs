using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HDSS_BACKEND.Models
{

public class Conduct{
    public int Id {get; set;}
    public string? AddConducts {get;set;}
}

public class Attitude{
    public int Id {get; set;}
    public string? AddAttitudes {get;set;}
}

    public class Instituition
    {
       public int Id { get; set; }
       public string? Logo { get; set; }
       public string? SchoolName { get; set; }
       public string? Location { get; set;}  
       public string? AdminName { get; set; }   
       public string? Position {get;set;} 
      
    }
    public class GeneralTReportInfo{
    public int Id{get; set;}
    public string? AcademicTerm {get; set;}
     public string? AcademicYear {get; set;}
    public string? VacationDate {get; set;}
    public string? ReOpeningDate {get; set;}


}
public class InstituitionDto
    {
       public int Id { get; set; }
       public string? Position {get;set;} 
       public IFormFile? File { get; set; }
       public string? SchoolName { get; set; }
       public string? Location { get; set;}  
       public string? AdminName { get; set; }      
    }

    public class Role{
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? StaffId { get; set; }
        public string? Position {get; set;}

    }

    public class AdmissionLetter{
        public int Id { get; set;}
        public string? StudentId { get; set;}
        public string? Logo { get; set; }
       public string? SchoolName { get; set; }
       public string? Location { get; set;}      
        public string? Paragraph1 { get; set; }
        public string? Paragraph2 { get; set; }
        public string? Paragraph3 { get; set; }
        public string? Paragraph4 { get; set; }
        public string? Paragraph5 { get; set; }
        public string? Paragraph6 { get; set; }
        public string? Name {get; set;}
        public string? AdmissionDate {get;set;}
        public string? AdminName { get; set; }  


    }
}