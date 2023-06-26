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
        public string? Status { get; set; }
        public string? Location { get; set; }
        public string? Country { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Education { get; set; }
        public string? TeachingExperience { get; set; }
        public string? NationalIds { get; set; }
        public string? TaxNumber { get; set; }
        public string? SSNITNumber { get; set; }
        public string? HealthStatus { get; set; }
        public string? EmergencyContacts { get; set; }
        public string? Salary { get; set; }
        public List<SchoolBank>? SchoolBankAccount { get; set; }
        public List<ClassToTeach>? ClassToTeaches { get; set; }
        public List<SubjectToTeach>? SubjectToTeaches { get; set; }
        public string? StaffID { get; set; }

    public string? FilePath { get; set; }
    public string? CertPath { get; set; }
    public string? AppointmentLetter { get; set; }
    public string? IdCards { get; set; }
    public string? DateAdded { get; set; }
}





public class TeacherDto
{
    public IFormFile? File { get; set; }
    public string? Title { get; set; }
        public string? FirstName { get; set; }
        public string? OtherName { get; set; }
        public string? LastName { get; set; }
        public string? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? Status { get; set; }
        public string? Location { get; set; }
        public string? Country { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Education { get; set; }
        public string? TeachingExperience { get; set; }
        public string? NationalIds { get; set; }
        public string? TaxNumber { get; set; }
        public string? SSNITNumber { get; set; }
        public string? HealthStatus { get; set; }
        public string? EmergencyContacts { get; set; }
        public string? Salary { get; set; }
        public List<SchoolBank>? SchoolBankAccount { get; set; }
        public List<ClassToTeach>? ClassToTeaches { get; set; }
        public List<SubjectToTeach>? SubjectToTeaches { get; set; }
        public string? StaffID { get; set; }

    public IFormFile? CertFile { get; set; }
    public IFormFile? IdCardsFile { get; set; }
    public IFormFile? AppointFile { get; set; }
}



}


        public class ClassToTeach
        {
        public int Id { get; set; }
        public string? Path { get; set; }
        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; }
        }

        public class SubjectToTeach
        {
        public int Id { get; set; }
        public string? Path { get; set; }
        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; }
        }

        public class SchoolBank
        {
        public int Id { get; set; }
        public string? Path { get; set; }
        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; }
        }

