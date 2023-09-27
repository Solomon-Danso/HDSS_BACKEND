using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HDSS_BACKEND.Models
{
    public class Fee
    {
        public int Id{ get; set; }
        public string? Title{ get; set; }
        public double? Amount{ get; set; }
        public string? AcademicYear{ get; set; }
        public string? AcademicTerm { get; set; }
        public string? Level{ get; set; }
    }

    public class AdmissionFee{
        public int Id{ get; set;}
         public double? Amount{ get; set; }
        public string? AcademicYear{ get; set; }
        public string? Level{ get; set; }

    }

    public class BillingCard{
        public int Id{ get; set;}
        public string? StudentId{ get; set; }
        public double? OpeningBalance{ get; set;}
        public double? Transaction{ get; set;}
        public double? ClosingBalance{ get; set;}
        public double? Bills {get; set;}
        public string? AcademicYear  {get; set;}
        public string? AcademicTerm  {get; set;}
        public string? Level {get; set;}
        public string? TransactionDate {get; set;}
        public string? Action {get; set;}

    }

    public class Payment{
        public double Amount {get; set;}
    }





}