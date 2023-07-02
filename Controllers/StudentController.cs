using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HDSS_BACKEND.Data;
using HDSS_BACKEND.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace HDSS_BACKEND.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class StudentController : ControllerBase
    {
        private readonly DataContext context;
        public static double? credit1;
        public static double? owe2;

        public StudentController(DataContext ctx)
        {
            context = ctx;
        }



 [HttpPost("registerStudent")]
        public async Task<IActionResult> CreateStudent([FromForm]StudentDto studentDto){
     bool stu = await context.Students.AnyAsync(s => s.StudentId == studentDto.StudentId);
    if (stu)
    {
        return BadRequest("Student already exists");
    }


    if (studentDto.File == null || studentDto.File.Length == 0)
    {
        return BadRequest("Invalid file");
    }

    // Create the uploads directory if it doesn't exist
    var uploadsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Students", "Profile");
    if (!Directory.Exists(uploadsDirectory))
    {
        Directory.CreateDirectory(uploadsDirectory);
    }

    // Get the original file extension
    var fileExtension = Path.GetExtension(studentDto.File.FileName);

    // Generate a unique file name
    var fileName = Guid.NewGuid().ToString() + fileExtension;

    // Save the uploaded file to the uploads directory
    var filePath = Path.Combine(uploadsDirectory, fileName);
    using (var stream = new FileStream(filePath, FileMode.Create))
    {
        await studentDto.File.CopyToAsync(stream);
    }

 var student = new Student
    {
        StudentId = StudentIdGenerator(),
        Title = studentDto.Title,
        FirstName = studentDto.FirstName,
        OtherName = studentDto.OtherName,
        LastName = studentDto.LastName,
        DateOfBirth = studentDto.DateOfBirth,
        Gender = studentDto.Gender,
        HomeTown = studentDto.HomeTown,
        Location = studentDto.Location,
        Country = studentDto.Country,
        FathersName = studentDto.FathersName,
        FathersContact = studentDto.FathersContact,
        MothersName = studentDto.MothersName,
        MothersContact = studentDto.MothersContact,
        GuardianName = studentDto.GuardianName,
        GuardianContact = studentDto.GuardianContact,
        MedicalIInformation = studentDto.MedicalIInformation,
        BasicLevel = studentDto.BasicLevel,
        amountOwing = studentDto.amountOwing,
        creditAmount = studentDto.creditAmount,
        AdmissionDate = DateTime.Today.Date.ToString("dd MMMM, yyyy"),
        SchoolBankAccount = studentDto.SchoolBankAccount,
        ProfilePic = Path.Combine("Students/Profile", fileName),
    };

    context.Students.Add(student);
    await context.SaveChangesAsync();
    return Ok("Working");
    
    }


private string StudentIdGenerator()
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




[HttpPost("updateStudent")]
public async Task<IActionResult> UpdateStudent(string Id, [FromForm]StudentDto request){
  var student = context.Students.FirstOrDefault(s=>s.StudentId == Id);
  var amtpaid = context.AmountsPaid.FirstOrDefault(s=>s.StudentId == Id);
  var amtowed = context.AmountsOwing.FirstOrDefault(s=>s.StudentId ==Id);
  var schoolfees = context.SchoolFeeTransactions.FirstOrDefault(s=>s.StudentId == Id);
  if(student == null|| amtowed == null|| amtpaid==null||schoolfees == null){
    return BadRequest("Student does not exist");
  }

 if (request.File == null || request.File.Length == 0)
    {
        return BadRequest("Invalid file");
    }

    // Create the uploads directory if it doesn't exist
    var uploadsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Students", "Profile");
    if (!Directory.Exists(uploadsDirectory))
    {
        Directory.CreateDirectory(uploadsDirectory);
    }

    // Get the original file extension
    var fileExtension = Path.GetExtension(request.File.FileName);

    // Generate a unique file name
    var fileName = Guid.NewGuid().ToString() + fileExtension;

    // Save the uploaded file to the uploads directory
    var filePath = Path.Combine(uploadsDirectory, fileName);
    using (var stream = new FileStream(filePath, FileMode.Create))
    {
        await request.File.CopyToAsync(stream);
    }


student.StudentId = request.StudentId;
amtpaid.StudentId = request.StudentId;
amtowed.StudentId = request.StudentId;
schoolfees.StudentId = request.StudentId;
student.Title = request.Title;
student.FirstName = request.FirstName;
student.OtherName = request.OtherName;
student.LastName = request.LastName;
student.DateOfBirth = request.DateOfBirth;
student.Gender = request.Gender;
student.HomeTown = request.HomeTown;
student.Location = request.Location;
student.Country = request.Country;
student.FathersName = request.FathersName;
student.FathersContact = request.FathersContact;
student.MothersName = request.MothersName;
student.MothersContact = request.MothersContact;
student.GuardianName = request.GuardianName;
student.GuardianContact = request.GuardianContact;
student.MedicalIInformation = request.MedicalIInformation;
student.BasicLevel = request.BasicLevel;
student.amountOwing = request.amountOwing;
student.creditAmount = request.creditAmount;
student.AdmissionDate = request.AdmissionDate;
student.SchoolBankAccount = request.SchoolBankAccount;
student.ProfilePic = Path.Combine("Students/Profile", fileName);

await context.SaveChangesAsync();

return Ok("Student update was successful");
}



[HttpGet("getStudents")]
public async Task<IActionResult> GetStudents(){
    var studentList = context.Students.ToList();
    
    return Ok(studentList);
}

[HttpGet("getSpecificUser")]
public async Task<IActionResult> GetSpecificUser(string StudentId){
var tutor = context.Students.FirstOrDefault(e=>e.StudentId == StudentId);
return Ok(tutor);
}


[HttpDelete("deleteSpecificUser")]
public async Task<IActionResult> DeleteSpecificUser(string StudentId){
var tutor =  context.Students.FirstOrDefault(e=>e.StudentId == StudentId);
if(tutor == null){
    return BadRequest("Student does not exist");
}
context.Students.Remove(tutor);
await context.SaveChangesAsync();

return Ok("Student Account Deleted");
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

            
           var owe1 = student.amountOwing;
            

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
                credit1 = credit.AmountDebtNew;
                
            }
            else{
                student.amountOwing = credit.AmountDebtNew;
                student.creditAmount = 0;
                owe2 = student.amountOwing;
                
            }

            
             var transaction = new SchoolFeeTransaction{
             Id= paid.Id,
             StudentId= student.StudentId,
             StudentName= student.Title + " " + student.FirstName + " " + student.OtherName + " " +student.LastName,
             OldAmountOwing = owe1,
             CreditAmount = student.creditAmount,
             THEAmountPaid = paid.Amountpaid,
             NewAmountOwing = student.amountOwing,
             PaymentDate = DateTime.Today.Date.ToString("dd MMMM, yyyy")

            };

           context.SchoolFeeTransactions.Add(transaction);
            
            
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
    var payments = context.SchoolFeeTransactions.Where(a => a.StudentId == accountId).ToList();
    if (payments.Count == 0)
    {
        return BadRequest("No payments found for the student");
    }
    
    return Ok(payments);
}






        
    }
}