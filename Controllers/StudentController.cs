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

            student.amountOwing = owe.Amount + student.amountOwing;
            student.amountOwing = student.amountOwing * -1;

            var debt = new AmountOwing{
             Id = owe.Id,
             StudentId= student.StudentId,
             StudentName = student.Title + " " + student.FirstName + " " + student.OtherName + " " +student.LastName,
             Amount = student.amountOwing,
             DebtDate = DateTime.Today.Date.ToString("dd MMMM, yyyy")
            };

            context.AmountsOwing.Add(debt);
            await context.SaveChangesAsync();

            return Ok($" Successfull new amount {student.amountOwing}");


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
            AmountDebtNew = student.amountOwing + paid.Amountpaid,
            PaymentDate = DateTime.Today.Date.ToString("dd MMMM, yyyy")

            };
           
            context.AmountsPaid.Add(credit);
            student.amountOwing = credit.AmountDebtNew;
            await context.SaveChangesAsync();
            return Ok($"Successfull new amount {student.amountOwing}");


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
            return Ok($"{student.Title}  {student.FirstName} {student.OtherName} {student.LastName} {student.amountOwing}");

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
