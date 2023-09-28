using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Transactions;
using HDSS_BACKEND.Data;
using HDSS_BACKEND.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace HDSS_BACKEND.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Accounting : ControllerBase
    {
        private readonly DataContext context;
        Constants constant = new Constants();
        public Accounting(DataContext ctx){
            context = ctx;
        }

        [HttpPost("AddFees")]
        public async  Task<IActionResult> AddFees([FromBody] Fee request){
            var fee = new Fee{
                Title = request.Title,
                Amount = request.Amount,
                AcademicYear = request.AcademicYear,
                AcademicTerm = request.AcademicTerm,
                Level = request.Level,

            };
            context.Fees.Add(fee);
            await context.SaveChangesAsync();

            return Ok("Fees added successfully");
        }

         [HttpPost("UpdatedFees")]
        public async  Task<IActionResult> UpdatedFees([FromBody] Fee request, int Id){
            var fee = context.Fees.FirstOrDefault(r => r.Id == Id);
            if(fee == null){
                return BadRequest("Fee not found");
            }

                fee.Title = request.Title;
                fee.Amount = request.Amount;
                fee.AcademicYear = request.AcademicYear;
                fee.AcademicTerm = request.AcademicTerm;
                fee.Level = request.Level;

            
            
            await context.SaveChangesAsync();

            return Ok("Fees Updated successfully");
        }

        [HttpGet("viewFees")]
        public async Task<IActionResult> GetFees(string Level, string AcaYear, string Term){
            var fees = context.Fees.Where(r=>r.Level == Level && r.AcademicYear == AcaYear && r.AcademicTerm==Term).OrderByDescending(r=>r.Id).ToList();
            return Ok(fees);
        }

        [HttpDelete("deleteFees")]
        public async Task<IActionResult> DeleteFees(int Id){
            var fees = context.Fees.FirstOrDefault(r=>r.Id==Id);
            if(fees == null){
                return BadRequest("Fees not found");
            }
            context.Fees.Remove(fees);
            await context.SaveChangesAsync();
            return Ok("Fees deleted");
        }


        [HttpPost("AddAdmissionFees")]
        public async  Task<IActionResult> AddAdmissionFees([FromBody] AdmissionFee request){
            var fee = new AdmissionFee{
               
                Amount = request.Amount,
                AcademicYear = request.AcademicYear,
                Level = request.Level,

            };
            bool checker = await context.AdmissionFees.AnyAsync(r=>r.AcademicYear==fee.AcademicYear && r.Level==fee.Level);
            if(checker){
                return BadRequest("You have already set an admission fee for this level");
            }
            else{
                context.AdmissionFees.Add(fee);
            await context.SaveChangesAsync();
            }
            

            return Ok("AdmissionFees added successfully");
        }

         [HttpPost("UpdatedAdmissionFees")]
        public async  Task<IActionResult> UpdatedAdmissionFees([FromBody] AdmissionFee request, int Id){
            var fee = context.AdmissionFees.FirstOrDefault(r => r.Id == Id);
            if(fee == null){
                return BadRequest("AdmissionFee not found");
            }

               
                fee.Amount = request.Amount;
                fee.AcademicYear = request.AcademicYear;
               
                fee.Level = request.Level;

            
            
            await context.SaveChangesAsync();

            return Ok("AdmissionFees Updated successfully");
        }

        [HttpGet("viewAdmissionFees")]
        public async Task<IActionResult> GetAdmissionFees(string Level, string AcaYear, string Term){
            var fees = context.AdmissionFees.Where(r=>r.Level == Level && r.AcademicYear == AcaYear).OrderByDescending(r=>r.Id).ToList();
            return Ok(fees);
        }

        [HttpDelete("deleteAdmissionFees")]
        public async Task<IActionResult> DeleteAdmissionFees(int Id){
            var fees = context.AdmissionFees.FirstOrDefault(r=>r.Id==Id);
            if(fees == null){
                return BadRequest("AdmissionFees not found");
            }
            context.AdmissionFees.Remove(fees);
            await context.SaveChangesAsync();
            return Ok("AdmissionFees deleted");
        }

        [HttpPost("PayFees")]
        public async Task<IActionResult>FeesPayment(string StudentId, [FromBody]Payment request, string StaffId){
            var stu = context.Students.FirstOrDefault(r=>r.StudentId==StudentId);
            var Staff = context.Roles.FirstOrDefault(r=>r.StaffId==StaffId&&r.Position==constant.Accountant);
             if(Staff==null){
                return BadRequest("You dont have the permission to perform this activity");
            }
            
            if (stu == null){
                return BadRequest("Student not found");
            }
           

            var Bill = new BillingCard{
                StudentId = stu.StudentId,
                OpeningBalance = stu.Balance,
                AcademicYear = stu.TheAcademicYear,
                AcademicTerm = stu.TheAcademicTerm,
                Level = stu.Level,
                TransactionDate = DateTime.Today.Date.ToString("dd MMMM, yyyy"),
                Action = request.Action,
                TransactionId = TransactionIdGenerator(),
                StaffId = Staff.StaffId,
                StaffName = Staff.FullName,
                PaymentMethod = request.PaymentMethod

            };
            
            Bill.Transaction = request.Amount;
            Bill.ClosingBalance = Bill.OpeningBalance-Bill.Transaction;
            stu.Balance = Bill.ClosingBalance;
            if(Bill.PaymentMethod.IsNullOrEmpty()|| Bill.Action.IsNullOrEmpty()){
                return BadRequest("All Fields are required");
            }
            else{
                 context.BillingCards.Add(Bill);
            await context.SaveChangesAsync();
            }

           
            return Ok(Bill);

        }

        [HttpGet("PaymentHis")]
        public async Task<IActionResult> GetPaymentHis(string StudentId){
            var stu = context.BillingCards.Where(a=>a.StudentId==StudentId).ToList();
            return Ok(stu);
        }

        [HttpPost("BillAClass")]
        public async Task<IActionResult> Biller(string Level, string Term, string Year){
        var fees = context.Fees.Where(r=>r.Level==Level && r.AcademicTerm ==Term && r.AcademicYear==Year).Sum(r=>r.Amount);
        var studentList = context.Students.Where(r => r.Level==Level).ToList();
        foreach (var student in studentList){
            var Bill = new BillingCard{
                StudentId = student.StudentId,
                OpeningBalance = student.Balance,
                AcademicYear = Year,
                AcademicTerm = Term,
                Level = Level,
                TransactionDate = DateTime.Today.Date.ToString("dd MMMM, yyyy"),
                Action = constant.TermFees,
                Bills = fees
            };
            Bill.Transaction = 0;
            var newBalance = student.Balance+fees;
            Bill.ClosingBalance = newBalance;
            context.BillingCards.Add(Bill);

            
            student.Balance = newBalance;
             student.Level = Level;
            student.TheAcademicTerm = Term;
            student.TheAcademicYear = Year;

        


            await context.SaveChangesAsync();
        }

            return Ok("Class Billed Successfully");
        }

        [HttpPost("RectifyBillAClass")]
        public async Task<IActionResult> RectifyBiller(string Level, string Term, string Year){
        var fees = context.Fees.Where(r=>r.Level==Level && r.AcademicTerm ==Term && r.AcademicYear==Year).Sum(r=>r.Amount);
        var studentList = context.Students.Where(r => r.Level==Level).ToList();
        foreach (var student in studentList){
            var newBalance = student.Balance-fees;

              var Bill = new BillingCard{
                StudentId = student.StudentId,
                OpeningBalance = student.Balance,
                AcademicYear = student.TheAcademicYear,
                AcademicTerm = student.TheAcademicTerm,
                Level = student.Level,
                TransactionDate = DateTime.Today.Date.ToString("dd MMMM, yyyy"),
                Action = constant.TermFeesUnbilling,
                Bills=fees
            };
            Bill.Transaction = 0;
            Bill.ClosingBalance = newBalance;
            context.BillingCards.Add(Bill);

            student.Balance = newBalance;
           
            await context.SaveChangesAsync();
        }

            return Ok("Class UnBilled Successfully");
        }

       [HttpPost("DiscountedFees")]
public async Task<IActionResult> DiscountedFees(int discountRate, string StudentId)
{
    var student = context.Students.FirstOrDefault(r => r.StudentId == StudentId);
    if (student == null)
    {
        return BadRequest("Student not found");
    }

    if (student.Balance.HasValue) // Check if student.Balance is not null
    {
        double rate = (double)discountRate / 100;
        double discount = (double)(rate * student.Balance.Value); 
        double newBalance = student.Balance.Value - discount;
        student.Balance = newBalance;

        
        

        await context.SaveChangesAsync();

        return Ok(discount); // Return the result, or adjust this as needed
    }
    else
    {
        // Handle the case where student.Balance is null (if needed)
        return BadRequest("Student balance is null.");
    }
}


private string TransactionIdGenerator()
{
    byte[] randomBytes = new byte[2]; // Increase the array length to 2 for a 4-digit random number
    using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
    {
        rng.GetBytes(randomBytes);
    }

    ushort randomNumber = BitConverter.ToUInt16(randomBytes, 0);
    int fullNumber = randomNumber; // 109000 is added to ensure the number is 5 digits long

    return fullNumber.ToString("D5");
}




    }
}