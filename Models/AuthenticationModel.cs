using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HDSS_BACKEND.Models
{
    public class AuthenticationModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Role { get; set; }
        public string? SpecificUserRole { get; set; }
        public string? UserId { get; set; }
        public string? UserPassword { get; set; }
        public string? RawPassword { get; set; }
        public string? TwoSteps { get; set; }
        public DateTime? TwoStepsExpire { get; set; }
        public string? PasswordToken { get; set; }
        public DateTime? PasswordTokenExpire { get; set; }


    }
    public class OnlySuperiorsCanViewThisDueToSecurityReasonsNtia{
       public int Id { get; set; }
        public string? Name { get; set; }
        public string? Role { get; set; }
        public string? UserId { get; set; }
        public string? UserPassword { get; set; } 
    }
}