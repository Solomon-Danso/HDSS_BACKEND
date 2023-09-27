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
        Constants constant = new Constants();

        public StudentController(DataContext ctx)
        {
            context = ctx;
        }



 [HttpPost("registerStudent")]
        public async Task<IActionResult> CreateStudent([FromForm]StudentDto studentDto){
    


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
        FirstName = studentDto.FirstName,
        OtherName = studentDto.OtherName,
        LastName = studentDto.LastName,
        DateOfBirth = studentDto.DateOfBirth,
        Gender = studentDto.Gender,
        HomeTown = studentDto.HomeTown,
        Location = studentDto.Location,
        Country = studentDto.Country,
        FathersName = studentDto.FathersName,
        FatherOccupation = studentDto.FatherOccupation,
        MothersName = studentDto.MothersName,
       MotherOccupation = studentDto.MotherOccupation,
        GuardianName = studentDto.GuardianName,
      GuardianOccupation = studentDto.GuardianOccupation,
        MedicalIInformation = studentDto.MedicalIInformation,
        ParentLocation = studentDto.ParentLocation,
        ParentDigitalAddress = studentDto.ParentDigitalAddress,
        ParentReligion = studentDto.ParentReligion,
        ParentEmail = studentDto.ParentEmail,
        EmergencyContactName = studentDto.EmergencyContactName,
        EmergencyPhoneNumber = studentDto.EmergencyPhoneNumber,
        EmergencyAlternatePhoneNumber = studentDto.EmergencyAlternatePhoneNumber,
        RelationshipWithChild = studentDto.RelationshipWithChild,
        Religion = studentDto.Religion,
        Email = studentDto.Email,
        PhoneNumber = studentDto.PhoneNumber,
        ParentPhoneNumber = studentDto.ParentPhoneNumber,
        AlternatePhoneNumber = studentDto.AlternatePhoneNumber,
        Level = studentDto.Level,
        AdmissionDate = DateTime.Today.Date.ToString("dd MMMM, yyyy"),
        SchoolBankAccount = studentDto.SchoolBankAccount,
        ProfilePic = Path.Combine("Students/Profile", fileName),
        Role = constant.Student,
        TheAcademicTerm = studentDto.TheAcademicTerm,
        TheAcademicYear = studentDto.TheAcademicYear,
    };
    bool IdExist = await context.Students.AnyAsync(x => x.StudentId == student.StudentId);
    if(IdExist){
        studentDto.StudentId = StudentIdGenerator();
    }
    var rawPassword = StudentIdGenerator();
     var Auth = new AuthenticationModel{
        Name = student.FirstName+" " + student.OtherName+" " + student.LastName,
        UserId = student.StudentId,
        Role = student.Role,
        UserPassword = BCrypt.Net.BCrypt.HashPassword(rawPassword),
        RawPassword = rawPassword,

     };
     var Only = new OnlySuperiorsCanViewThisDueToSecurityReasonsNtia{
         Name = student.FirstName+" " + student.OtherName+" " + student.LastName,
        UserId = student.StudentId,
        Role = student.Role,
        UserPassword = rawPassword,

     };
    context.OnlySuperiorsCanViewThisDueToSecurityReasons.Add(Only);
    context.AuthenticationModels.Add(Auth);

    var parent = new ParentsModel{
        ParentId = StudentIdGenerator(),
        StudentPicture = student.ProfilePic,
        StudentName = student.FirstName+" " + student.OtherName+" "+student.LastName,
        StudentId = student.StudentId,
        StudentLevel = student.Level,
        FathersName = student.FathersName,
        FatherOccupation = student.FatherOccupation,
        MothersName = student.MothersName,
        MotherOccupation = student.MotherOccupation,
        GuardianName = student.GuardianName,
        GuardianOccupation = student.GuardianOccupation,
        ParentLocation = student.ParentLocation,
        ParentDigitalAddress = student.ParentDigitalAddress,
        ParentReligion = student.ParentReligion,
        ParentEmail = student.ParentEmail,
       EmergencyContactName = student.EmergencyContactName,
       EmergencyPhoneNumber = student.EmergencyPhoneNumber,
       EmergencyAlternatePhoneNumber = student.EmergencyAlternatePhoneNumber,
       RelationshipWithChild = student.RelationshipWithChild,
        NumberOfWards = 1,
        Role = constant.Parent,
        DateAdded = DateTime.Today.Date.ToString("dd MMMM, yyyy"),

    }; 

    bool checker = await context.Parents.AnyAsync(p=>p.EmergencyPhoneNumber==parent.EmergencyPhoneNumber||p.EmergencyPhoneNumber==parent.EmergencyAlternatePhoneNumber||p.EmergencyAlternatePhoneNumber==parent.EmergencyPhoneNumber||p.EmergencyAlternatePhoneNumber==parent.EmergencyAlternatePhoneNumber);
    if (checker){
        var ck = context.Parents.FirstOrDefault(p=>p.EmergencyPhoneNumber==parent.EmergencyPhoneNumber||p.EmergencyPhoneNumber==parent.EmergencyAlternatePhoneNumber||p.EmergencyAlternatePhoneNumber==parent.EmergencyPhoneNumber||p.EmergencyAlternatePhoneNumber==parent.EmergencyAlternatePhoneNumber);
        if (ck!=null){
            ck.NumberOfWards = ck.NumberOfWards+1;
        }
    }
    else{
   

     var ParentrawPassword = StudentIdGenerator();
     var ParentAuth = new AuthenticationModel{
        Name =constant.Parent +" Of "+student.FirstName+" " + student.OtherName+" " + student.LastName,
        UserId = parent.ParentId,
        Role = parent.Role,
        UserPassword = BCrypt.Net.BCrypt.HashPassword(ParentrawPassword),

     };
     var ParentOnly = new OnlySuperiorsCanViewThisDueToSecurityReasonsNtia{
         Name = constant.Parent +" Of "+student.FirstName+" " + student.OtherName+" " + student.LastName,
        UserId = parent.ParentId,
        Role = parent.Role,
        UserPassword = ParentrawPassword

     };
    context.OnlySuperiorsCanViewThisDueToSecurityReasons.Add(ParentOnly);
    context.AuthenticationModels.Add(ParentAuth);
    context.Parents.Add(parent);

    }

    //Accounting 

var adfee = context.AdmissionFees.FirstOrDefault(r=>r.Level ==student.Level&&r.AcademicYear==student.TheAcademicYear);
var otherfee = context.Fees.Where(r=>r.Level ==student.Level&&r.AcademicYear==student.TheAcademicYear&&r.AcademicTerm==student.TheAcademicTerm).Sum(r=>r.Amount);

if(adfee==null){
  var Bill = new BillingCard{
StudentId = student.StudentId,
OpeningBalance =  otherfee,
Transaction = 0,
AcademicYear = student.TheAcademicYear,
AcademicTerm = student.TheAcademicTerm,
Level = student.Level,
TransactionDate = student.AdmissionDate,
 
};

Bill.ClosingBalance = Bill.OpeningBalance-Bill.Transaction;
student.Balance = Bill.ClosingBalance;



context.BillingCards.Add(Bill);
  
}
else{
var Bill = new BillingCard{
StudentId = student.StudentId,
OpeningBalance = adfee?.Amount + otherfee,
Transaction = 0,
AcademicYear = student.TheAcademicYear,
AcademicTerm = student.TheAcademicTerm,
Level = student.Level,
TransactionDate = student.AdmissionDate,
 
};

Bill.ClosingBalance = Bill.OpeningBalance-Bill.Transaction;
student.Balance = Bill.ClosingBalance;



context.BillingCards.Add(Bill);
}



context.Students.Add(student);
    
    await context.SaveChangesAsync();
    return Ok(student);
    
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

  if(student == null){
    return BadRequest("Student does not exist");
  }

 

student.FirstName = request.FirstName;
student.OtherName = request.OtherName;
student.LastName = request.LastName;
student.DateOfBirth = request.DateOfBirth;
student.Gender = request.Gender;
student.HomeTown = request.HomeTown;
student.Location = request.Location;
student.Country = request.Country;
student.FathersName = request.FathersName;
student.FatherOccupation = request.FatherOccupation;
student.MothersName = request.MothersName;
student.MotherOccupation = request.MotherOccupation;
student.GuardianName = request.GuardianName;
student.GuardianOccupation = request.GuardianOccupation;
student.MedicalIInformation = request.MedicalIInformation;
student.Level = request.Level;

student.AdmissionDate = request.AdmissionDate;
student.SchoolBankAccount = request.SchoolBankAccount;
student.Religion = request.Religion;
student.Email = request.Email;
student.PhoneNumber = request.PhoneNumber;
student.AlternatePhoneNumber = request.AlternatePhoneNumber;
student.ParentLocation = request.ParentLocation;
student.ParentDigitalAddress = request.ParentDigitalAddress;
student.ParentReligion = request.ParentReligion;
student.ParentEmail = request.ParentEmail;
student.ParentPhoneNumber = request.ParentPhoneNumber;
student.EmergencyContactName = request.EmergencyContactName;
student.EmergencyPhoneNumber = request.EmergencyPhoneNumber;
student.EmergencyAlternatePhoneNumber = request.EmergencyAlternatePhoneNumber;
student.RelationshipWithChild = request.RelationshipWithChild;

await context.SaveChangesAsync();

return Ok("Student update was successful");
}



[HttpGet("getStudents")]
public async Task<IActionResult> GetStudents(string stage)
{
    var studentList = context.Students.Where(s=>s.Level==stage).OrderBy(student => student.LastName).ToList();
    
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
             StudentName =  student.FirstName + " " + student.OtherName + " " +student.LastName,
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
            StudentName =student.FirstName + " " + student.OtherName + " " +student.LastName,
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
             StudentName=  student.FirstName + " " + student.OtherName + " " +student.LastName,
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