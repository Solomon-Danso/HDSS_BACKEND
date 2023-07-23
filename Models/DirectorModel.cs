using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HDSS_BACKEND.Models
{
    public class SchoolDirector
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
        public string? Role { get; set; }

    }

     public class Managers
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Contact { get; set; }
        public string? Email { get; set; }
        public string? DirectorID { get; set; }
        public string? Password { get; set; }
        public string? TwoSteps { get; set; }
        public DateTime? TwoStepsExpire { get; set; }
        public string? PasswordToken { get; set; }
        public DateTime? PasswordTokenExpire { get; set; }
        public string? Role { get; set; }

    }

   
}