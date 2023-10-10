using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HDSS_BACKEND.Models;

namespace HDSS_BACKEND.Models
{



public class Teacher
{
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? FirstName { get; set; }
        public string? OtherName { get; set; }
        public string? LastName { get; set; }
        public string? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? MaritalStatus { get; set; }
        public string? Location { get; set; }
        public string? Country { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Education { get; set; }
        public string? TeachingExperience { get; set; }
        public string? TaxNumber { get; set; }
        public string? SSNITNumber { get; set; }
        public string? HealthStatus { get; set; }
        public string? EmergencyContacts { get; set; }
         public string? EmergencyPhone { get; set; }
        public string? Salary { get; set; }
        public double? Debit { get; set; }
        public double? Credit{ get; set; }
        public string? StaffID { get; set; }
        public string? Role { get; set; }
        public string? SpecificRole { get; set; }
         

    public string? FilePath { get; set; }
    public string? CertPath { get; set; }
    public string? IdCards { get; set; }
    public string? DateAdded { get; set; }

    public string? Position {get; set;}
    public string? ReportingTime {get;set;}
    public string? StartDate {get;set;}

}





public class TeacherDto
{
   // public int Id { get; set; }
    public IFormFile? File { get; set; }
    public string? Title { get; set; }
        public string? Position {get; set;}
    public string? ReportingTime {get;set;}
    public string? StartDate {get;set;}
        public string? FirstName { get; set; }
        public string? OtherName { get; set; }
        public string? LastName { get; set; }
         public string? EmergencyPhone { get; set; }
        public string? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? MaritalStatus { get; set; }
        public string? Location { get; set; }
        public string? Country { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Education { get; set; }
        public string? TeachingExperience { get; set; }
        public string? TaxNumber { get; set; }
        public string? SSNITNumber { get; set; }
        public string? HealthStatus { get; set; }
        public string? EmergencyContacts { get; set; }
        public string? Salary { get; set; }
    
        public string? StaffID { get; set; }

    public IFormFile? CertFile { get; set; }
    public IFormFile? IdCardsFile { get; set; }
   
     
}



}


    
        public class SchoolBank
        {
        public int Id { get; set; }
        public string? Path { get; set; }
        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; }
        }

