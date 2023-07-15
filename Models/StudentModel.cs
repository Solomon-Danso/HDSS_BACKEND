using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HDSS_BACKEND.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string? StudentId { get; set; }
        public string? Title { get; set; }
        public string? FirstName { get; set; }
        public string? OtherName { get; set; }
        public string? LastName { get; set; }
        public string? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? HomeTown { get; set; }
        public string? Location { get; set; }
        public string? Country { get; set; }
        public string? FathersName { get; set; }
        public string? FathersContact { get; set; }
        public string? MothersName { get; set; }
        public string? MothersContact { get; set; }
        public string? GuardianName { get; set; }
        public string? GuardianContact { get; set; }
        public string? MedicalIInformation { get; set; }
        public string? Level { get; set; }
        public double? amountOwing { get; set; }
        public double? creditAmount { get; set; }
        public string? AdmissionDate { get; set; }
        public double? SchoolBankAccount { get; set; }
        public string? ProfilePic { get; set; }



    }


    public class StudentDto
    {
        public int Id { get; set; }
        public string? StudentId { get; set; }
        public string? Title { get; set; }
        public string? FirstName { get; set; }
        public string? OtherName { get; set; }
        public string? LastName { get; set; }
        public string? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? HomeTown { get; set; }
        public string? Location { get; set; }
        public string? Country { get; set; }
        public string? FathersName { get; set; }
        public string? FathersContact { get; set; }
        public string? MothersName { get; set; }
        public string? MothersContact { get; set; }
        public string? GuardianName { get; set; }
        public string? GuardianContact { get; set; }
        public string? MedicalIInformation { get; set; }
        public string? Level { get; set; }
        public double? amountOwing { get; set; }
        public double? creditAmount { get; set; }
        public string? AdmissionDate { get; set; }
        public double? SchoolBankAccount { get; set; }
        public IFormFile? File { get; set; }


    }




    public class AmountOwing{
        public int Id { get; set; }
        public string? StudentId { get; set; }
        public string? StudentName { get; set; }
        public double? Amount { get; set; }
        public string? DebtDate { get; set; }
    }

    public class AmountPaid{
        public int Id { get; set; }
        public string? StudentId { get; set; }
        public string? StudentName { get; set; }
        public double? AmountDebtOld { get; set; }
        public double? Amountpaid { get; set; }
        public double? CreditAmount{ get; set; }
        public double? AmountDebtNew { get; set; }
        public string? PaymentDate { get; set; }

    }


    public class SchoolFeeTransaction
    {
       public int Id { get; set; }
       public string? StudentId { get; set; }
       public string? StudentName { get; set; }
       public double? OldAmountOwing { get; set; }
       public double? CreditAmount { get; set; }
       public double? THEAmountPaid { get; set; }
       public double? NewAmountOwing { get; set; }
       public string? PaymentDate { get; set; }



    }
}