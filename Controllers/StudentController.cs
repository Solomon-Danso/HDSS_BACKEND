using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HDSS_BACKEND.Data;
using HDSS_BACKEND.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Globalization;

namespace HDSS_BACKEND.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class StudentController : ControllerBase
    {
        private readonly DataContext context;

        public static double? credit1;
        public static double? owe2;
          string Country;
        string City;
        double latitude;
        double logitude;
        Constants constant = new Constants();

        public StudentController(DataContext ctx)
        {
            context = ctx;
           
        }



 [HttpPost("registerStudent")]
        public async Task<IActionResult> CreateStudent([FromForm]StudentDto studentDto,string ID){
    


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




 await  AdminAuditor(ID, constant.AdmitStudent);






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
await StudentAuditor(Id, constant.UpdateStudent);


return Ok("Student update was successful");
}



[HttpGet("getStudents")]
public async Task<IActionResult> GetStudents(string stage, string ID)
{
    var studentList = context.Students.Where(s=>s.Level==stage).OrderBy(student => student.LastName).ToList();
    await  AdminAuditor(ID, constant.GetStudent);
    return Ok(studentList);
}



[HttpGet("getSpecificUser")]
public async Task<IActionResult> GetSpecificUser(string StudentId, string ID){
var tutor = context.Students.FirstOrDefault(e=>e.StudentId == StudentId);
await  AdminAuditor(ID, constant.GetSpecificStudent); 
return Ok(tutor);
}


[HttpDelete("deleteSpecificUser")]
public async Task<IActionResult> DeleteSpecificUser(string StudentId, string ID){
var tutor =  context.Students.FirstOrDefault(e=>e.StudentId == StudentId);
if(tutor == null){
    return BadRequest("Student does not exist");
}
context.Students.Remove(tutor);
await context.SaveChangesAsync();
await  AdminAuditor(ID, constant.DeleteStudent); 
return Ok("Student Account Deleted");
}



[ApiExplorerSettings(IgnoreApi = true)] 
public async Task StudentAuditor(string StudentId,string Action)
{


    var user = context.Students.FirstOrDefault(a => a.StudentId==StudentId);
    if (user ==null){
         BadRequest("Student not found");
    }

    var ipAddress = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
    var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();
    var deviceCategory = GetDeviceCategory(userAgent);
     //var ipAddress = "41.155.45.174";
    if (string.IsNullOrEmpty(ipAddress))
    {
        ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

    }

        try
    {
        using (var client = new HttpClient())
        {
            var response = await client.GetFromJsonAsync<IpApiLocationResponse>($"http://ip-api.com/json/{ipAddress}");

            Country = response.country;
            City = response.city;
            latitude = response.lat;
            logitude = response.lon;
        }
    }
    catch (HttpRequestException ex)
    {
        // Handle the exception or log it as needed
        // You can set default values for Country, City, latitude, and longitude here
        // For example:
        Country = "Unknown";
        City = "Unknown";
        latitude = 0.0;
        logitude = 0.0;
    }

//fast
 string formattedTime = DateTime.Now.ToString(@"hh:mm:ss tt", CultureInfo.InvariantCulture);


    var audit = new AuditTrial
    {
        IpAddress = ipAddress,
        BrowserType = userAgent,
        DeviceType = deviceCategory,
        UserLocation = $"https://www.google.com/maps?q={latitude},{logitude}",
        Country = Country,
        City = City,
        Maker = user.FirstName+" "+user.OtherName+" "+user.LastName,
        TheDateStamp = DateTime.Today.Date.ToString("MMMM dd, yyyy"),
       TheTimeStamp = formattedTime,
        Email= user.Email,
        ActionDescription = Action,
        Role = user.Role,
        Level = user.Level

    };

    context.AuditTrials.Add(audit);
    await context.SaveChangesAsync();

   
}

[ApiExplorerSettings(IgnoreApi = true)] 
public async Task AdminAuditor(string StaffId, string Action)
{
    var user = context.SuperiorAccounts.FirstOrDefault(a => a.StaffID == StaffId);
    if (user == null)
    {
          BadRequest("User not found");
    }

    var ipAddress = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
    var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();
    var deviceCategory = GetDeviceCategory(userAgent);

    // Ensure ipAddress is not null or empty
    if (string.IsNullOrEmpty(ipAddress))
    {
        ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
    }

    try
    {
        using (var client = new HttpClient())
        {
            var response = await client.GetFromJsonAsync<IpApiLocationResponse>($"http://ip-api.com/json/{ipAddress}");

            if (response != null)
            {
                Country = response.country;
                City = response.city;
                latitude = response.lat;
                logitude = response.lon;
            }
            else
            {
                // Log a message when the response is null
                // You can replace this with your preferred logging mechanism
                Console.WriteLine("IP API response is null");
                Country = "Unknown";
                City = "Unknown";
                latitude = 0.0;
                logitude = 0.0;
            }
        }
    }
    catch (HttpRequestException ex)
    {
        // Log the exception
        // You can replace this with your preferred logging mechanism
        Console.WriteLine($"HttpRequestException: {ex.Message}");

        // Set default values for Country, City, latitude, and longitude
        Country = "Unknown";
        City = "Unknown";
        latitude = 0.0;
        logitude = 0.0;
    }

    // Fast
    string formattedTime = DateTime.Now.ToString(@"hh:mm:ss tt", CultureInfo.InvariantCulture);

    var audit = new AuditTrial
    {
        IpAddress = ipAddress,
        BrowserType = userAgent,
        DeviceType = deviceCategory,
        UserLocation = $"https://www.google.com/maps?q={latitude},{logitude}",
        Country = Country,
        City = City,
        Maker = user.Name,
        TheDateStamp = DateTime.Today.Date.ToString("MMMM dd, yyyy"),
        TheTimeStamp = formattedTime,
        Email = user.Email,
        ActionDescription = Action,
        Role = user.Role,
    };

    context.AuditTrials.Add(audit);
    await context.SaveChangesAsync();

      Ok();
}

[ApiExplorerSettings(IgnoreApi = true)] 
public async Task TeacherAuditor(string StudentId,string Action)
{


    var user = context.Students.FirstOrDefault(a => a.StudentId==StudentId);
    if (user ==null){
         BadRequest("Student not found");
    }

    var ipAddress = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
    var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();
    var deviceCategory = GetDeviceCategory(userAgent);
     //var ipAddress = "41.155.45.174";
    if (string.IsNullOrEmpty(ipAddress))
    {
        ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

    }

        try
    {
        using (var client = new HttpClient())
        {
            var response = await client.GetFromJsonAsync<IpApiLocationResponse>($"http://ip-api.com/json/{ipAddress}");

            Country = response.country;
            City = response.city;
            latitude = response.lat;
            logitude = response.lon;
        }
    }
    catch (HttpRequestException ex)
    {
        // Handle the exception or log it as needed
        // You can set default values for Country, City, latitude, and longitude here
        // For example:
        Country = "Unknown";
        City = "Unknown";
        latitude = 0.0;
        logitude = 0.0;
    }

//fast
 string formattedTime = DateTime.Now.ToString(@"hh:mm:ss tt", CultureInfo.InvariantCulture);


    var audit = new AuditTrial
    {
        IpAddress = ipAddress,
        BrowserType = userAgent,
        DeviceType = deviceCategory,
        UserLocation = $"https://www.google.com/maps?q={latitude},{logitude}",
        Country = Country,
        City = City,
        Maker = user.FirstName+" "+user.OtherName+" "+user.LastName,
        TheDateStamp = DateTime.Today.Date.ToString("MMMM dd, yyyy"),
       TheTimeStamp = formattedTime,
        Email= user.Email,
        ActionDescription = Action,
        Role = user.Role,
        Level = user.Level

    };

    context.AuditTrials.Add(audit);
    await context.SaveChangesAsync();

   
}

[ApiExplorerSettings(IgnoreApi = true)] 
public async Task AuthAuditor(string StaffId,string Action)
{


    var user = context.AuthenticationModels.FirstOrDefault(a => a.UserId==StaffId);
    if (user ==null){
         BadRequest("Student not found");
    }

    var ipAddress = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
    var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();
    var deviceCategory = GetDeviceCategory(userAgent);
     //var ipAddress = "41.155.45.174";
    if (string.IsNullOrEmpty(ipAddress))
    {
        ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

    }

        try
    {
        using (var client = new HttpClient())
        {
            var response = await client.GetFromJsonAsync<IpApiLocationResponse>($"http://ip-api.com/json/{ipAddress}");

            Country = response.country;
            City = response.city;
            latitude = response.lat;
            logitude = response.lon;
        }
    }
    catch (HttpRequestException ex)
    {
        // Handle the exception or log it as needed
        // You can set default values for Country, City, latitude, and longitude here
        // For example:
        Country = "Unknown";
        City = "Unknown";
        latitude = 0.0;
        logitude = 0.0;
    }

//fast
 string formattedTime = DateTime.Now.ToString(@"hh:mm:ss tt", CultureInfo.InvariantCulture);


    var audit = new AuditTrial
    {
        IpAddress = ipAddress,
        BrowserType = userAgent,
        DeviceType = deviceCategory,
        UserLocation = $"https://www.google.com/maps?q={latitude},{logitude}",
        Country = Country,
        City = City,
        Maker = user.Name,
        TheDateStamp = DateTime.Today.Date.ToString("MMMM dd, yyyy"),
       TheTimeStamp = formattedTime,
        ActionDescription = Action,
        Role = user.SpecificUserRole,
        

    };

    context.AuditTrials.Add(audit);
     try
    {
        // Existing code for AuthAuditor
        // ...
        await context.SaveChangesAsync();
    }
    catch (Exception ex)
    {
        // Log any exceptions here
        Console.WriteLine(ex.Message);
    }

   
}



private string GetDeviceCategory(string userAgent)
{
    userAgent = userAgent.ToLower();

    if (userAgent.Contains("mobile") || userAgent.Contains("android") || userAgent.Contains("iphone") || userAgent.Contains("ipad") || userAgent.Contains("ipod"))
    {
        return "Mobile";
    }
    else if (userAgent.Contains("tablet"))
    {
        return "Tablet";
    }
    else
    {
        return "Desktop";
    }
}


        
    }
}