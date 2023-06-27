using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HDSS_BACKEND.Data;
using HDSS_BACKEND.Models;
using Microsoft.EntityFrameworkCore;

namespace HDSS_BACKEND.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class StudentController : ControllerBase
    {
        private readonly DataContext context;

        public StudentController(DataContext ctx)
        {
            context = ctx;
        }


        [HttpPost("registerStudent")]
        public async Task<IActionResult> CreateStudent(Student student)
        {
            student.AdmissionDate = DateTime.Today.Date.ToString("dd MMMM, yyyy");
            context.Students.Add(student);
            await context.SaveChangesAsync();
            return Ok(student.StudentId);
        }


        [HttpPost("StudentOwing")]
        public async Task<IActionResult>Debtors(string accountId,AmountOwing owe){
            var student = context.Students.FirstOrDefault(x => x.StudentId == accountId);
            if (student == null){
                return BadRequest("Student does not exist");
            }
            
            
            

            var debt = new AmountOwing{
             Id = owe.Id,
             StudentId= student.StudentId,
             StudentName = student.Title + " " + student.FirstName + " " + student.OtherName + " " +student.LastName,
             Amount = owe.Amount,
             DebtDate = DateTime.Today.Date.ToString("dd MMMM, yyyy")
            };
                   
            context.AmountsOwing.Add(debt);
            var owingAmount = student.creditAmount - (student.amountOwing+debt.Amount);
            if(owingAmount>=0){
                student.creditAmount = owingAmount;
                student.amountOwing = 0;
            }
            else{
                student.amountOwing = owingAmount;
                student.creditAmount = 0;
            }
            
            await context.SaveChangesAsync();

            return Ok($"Done");


        }


        [HttpPost("StudentPayment")]
        public async Task<IActionResult>Creditors(string accountId,AmountPaid paid){
           var student = context.Students.FirstOrDefault(x => x.StudentId == accountId);
            if (student == null){
                return BadRequest("Student does not exist");
            }

            
           
            

            var credit = new AmountPaid{

             Id = paid.Id,
             StudentId = student.StudentId,
            StudentName = student.Title + " " + student.FirstName + " " + student.OtherName + " " +student.LastName,
            AmountDebtOld = student.amountOwing,
            Amountpaid = paid.Amountpaid,
            CreditAmount = student.creditAmount,
            AmountDebtNew = student.amountOwing+paid.Amountpaid+student.creditAmount,
            PaymentDate = DateTime.Today.Date.ToString("dd MMMM, yyyy")

            };
           
            context.AmountsPaid.Add(credit);
            if(credit.AmountDebtNew>=0){
               student.creditAmount = credit.AmountDebtNew;
               student.amountOwing = 0;
            }
            else{
                student.amountOwing = credit.AmountDebtNew;
                student.creditAmount = 0;
            }
            
            
            await context.SaveChangesAsync();
            return Ok($"Done");


        }


        [HttpGet("AllStudentStatus")]
        public async Task<IActionResult> GetAllStudentStatus(){
            var students =  context.Students.ToList();
            return Ok(students);
        }

        [HttpGet("SingleStudentStatus")]
        public async Task<IActionResult> GetStudentStatus(string accountId){
            var student = context.Students.FirstOrDefault(s=>s.StudentId == accountId);
            if(student == null){
                return BadRequest("Student not found");
            }
            return Ok($"Debit=> {student.amountOwing}, Credit => {student.creditAmount}");

        }

        [HttpGet("PaymentHistory")]
public async Task<IActionResult> GetPaymentHistory(string accountId)
{
    var payments = context.AmountsPaid.Where(a => a.StudentId == accountId).ToList();
    if (payments.Count == 0)
    {
        return BadRequest("No payments found for the student");
    }
    
    return Ok(payments);
}






        
    }
}