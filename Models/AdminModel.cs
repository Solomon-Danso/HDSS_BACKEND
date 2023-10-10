using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HDSS_BACKEND.Models
{

public class SuperiorAccount
    {
        public int Id { get; set; }
         public string? StaffID { get; set; }
        public string? Name { get; set; }
        public string? Contact { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
        public string? SpecificRole { get; set; }
        public string? ProfilePicturePath { get; set; }

    }

public class SuperiorAccountDto
    {
        public int Id { get; set; }
         public string? StaffID { get; set; }
        public string? Name { get; set; }
        public string? Contact { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? TwoSteps { get; set; }
        public DateTime? TwoStepsExpire { get; set; }
        public string? PasswordToken { get; set; }
        public DateTime? PasswordTokenExpire { get; set; }
        public IFormFile? File { get; set; }

    }


    public class SchoolDirector
    {
        public int Id { get; set; }
         public string? DirectorID { get; set; }
        public string? Name { get; set; }
        public string? Contact { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
        public string? SpecificRole { get; set; }
        public string? ProfilePicturePath { get; set; }

    }

    public class SchoolDirectorDto
    {
        public int Id { get; set; }
         public string? DirectorID { get; set; }
        public string? Name { get; set; }
        public string? Contact { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? TwoSteps { get; set; }
        public DateTime? TwoStepsExpire { get; set; }
        public string? PasswordToken { get; set; }
        public DateTime? PasswordTokenExpire { get; set; }

        public IFormFile? File { get; set; }

    }

     public class Managers
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Contact { get; set; }
        public string? Email { get; set; }
        public string? ManagerID { get; set; }
        public string? Role { get; set; }
        public string? ProfilePicturePath { get; set; }
        public string? SpecificRole { get; set; }

    }

     public class Admin
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Contact { get; set; }
        public string? Email { get; set; }
        public string? AdminID { get; set; }
        public string? Role { get; set; }
       public string? SpecificRole { get; set; }
       public string? ProfilePic {get; set;}

    }
 public class ManagersDto
    {
        public int Id { get; set; }
        public string? SpecificRole { get; set; }
        public string? Name { get; set; }
        public string? Contact { get; set; }
        public string? Email { get; set; }
        public string? ManagerID { get; set; }
        public string? Password { get; set; }
        public string? TwoSteps { get; set; }
        public DateTime? TwoStepsExpire { get; set; }
        public string? PasswordToken { get; set; }
        public DateTime? PasswordTokenExpire { get; set; }
        public IFormFile? File { get; set; }

    }

    public class TheEvent{
        public int Id{get;set;}
        public string? Title{get;set;}
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }

    }


   
}