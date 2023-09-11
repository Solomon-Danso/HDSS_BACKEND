using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HDSS_BACKEND.Models
{
    public class Instituition
    {
       public int Id { get; set; }
       public string? Logo { get; set; }
       public string? SchoolName { get; set; }
       public string? Location { get; set;}  
       public string? AdminName { get; set; }    
    }

    public class InstituitionDto
    {
       public int Id { get; set; }
       public IFormFile? File { get; set; }
       public string? SchoolName { get; set; }
       public string? Location { get; set;}  
       public string? AdminName { get; set; }      
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