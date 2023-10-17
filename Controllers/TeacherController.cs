using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using HDSS_BACKEND.Data;
using HDSS_BACKEND.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.NetworkInformation;
using System.Globalization;

namespace HDSS_BACKEND.Controllers
{
    [ApiController]
    [Route("api/Teacher")]

    public class TeacherController : ControllerBase
    {
        Constants constant = new Constants();
          string Country;
        string City;
        double latitude;
        double logitude;
       private readonly DataContext context;
    
        public TeacherController(DataContext ctx){
            context = ctx;
        }          

[HttpPost("registerTeacher")]
public async Task<IActionResult> RegisterTeacher([FromForm] TeacherDto request, string ID)
{
   bool email = await context.Teachers.AnyAsync(e=>e.Email==request.Email);
   bool Phone = await context.Teachers.AnyAsync(e=>e.PhoneNumber==request.PhoneNumber);
   bool StaffID = await context.Teachers.AnyAsync(e=>e.StaffID==request.StaffID);

   if (email || Phone || StaffID){
    return BadRequest("Teacher already registered");
   }
   
    if (request.File == null || request.File.Length == 0)
    {
        return BadRequest("Invalid file");
    }

    // Create the uploads directory if it doesn't exist
    var uploadsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Teachers", "Profile");
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





if (request.CertFile == null || request.CertFile.Length == 0)
    {
        return BadRequest("Invalid file");
    }

    // Create the uploads directory if it doesn't exist
    var uploadsCertDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot","Teachers", "Certificates");
    if (!Directory.Exists(uploadsCertDirectory))
    {
        Directory.CreateDirectory(uploadsCertDirectory);
    }

    // Get the original file extension
    var certExtension = Path.GetExtension(request.CertFile.FileName);

    // Generate a unique file name
    var certName = Guid.NewGuid().ToString() + certExtension;

    // Save the uploaded file to the uploads directory
    var certPath = Path.Combine(uploadsCertDirectory, certName);
    using (var stream = new FileStream(certPath, FileMode.Create))
    {
        await request.CertFile.CopyToAsync(stream);
    }



if (request.IdCardsFile == null || request.IdCardsFile.Length == 0)
    {
        return BadRequest("Invalid file");
    }

    // Create the uploads directory if it doesn't exist
    var uploadsIdCardsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot","Teachers", "IdCards");
    if (!Directory.Exists(uploadsIdCardsDirectory))
    {
        Directory.CreateDirectory(uploadsIdCardsDirectory);
    }

    // Get the original file extension
    var idcardsExtension = Path.GetExtension(request.IdCardsFile.FileName);

    // Generate a unique file name
    var idcardsName = Guid.NewGuid().ToString() + idcardsExtension;

    // Save the uploaded file to the uploads directory
    var idcardsPath = Path.Combine(uploadsIdCardsDirectory, idcardsName);
    using (var stream = new FileStream(idcardsPath, FileMode.Create))
    {
        await request.IdCardsFile.CopyToAsync(stream);
    }


    

    // Create a new Picture entity and save it to the database
    var tutor = new Teacher
    {
        Title = request.Title,
        FirstName = request.FirstName,
        OtherName = request.OtherName,
        LastName = request.LastName,
        DateOfBirth = request.DateOfBirth,
        Gender = request.Gender,
        Salary = request.Salary,
        Position = request.Position,
        ReportingTime = request.ReportingTime,
        StartDate = request.StartDate,
        MaritalStatus = request.MaritalStatus,
        Location = request.Location,
        Country = request.Country,
        PhoneNumber = request.PhoneNumber,
        Email = request.Email,
        Education = request.Education,   
        TeachingExperience = request.TeachingExperience,
        TaxNumber = request.TaxNumber,
        SSNITNumber = request.SSNITNumber,
        HealthStatus = request.HealthStatus,
        EmergencyContacts = request.EmergencyContacts,
        Role =  constant.Teacher,
        SpecificRole = constant.Teacher,
       EmergencyPhone = request.EmergencyPhone,
        StaffID = StaffIdGenerator(),
        DateAdded = DateTime.Today.Date.ToString("dd MMMM, yyyy"),
        FilePath = Path.Combine("Teachers/Profile", fileName),
        CertPath = Path.Combine("Teachers/Certificates", certName),
        IdCards = Path.Combine("Teachers/IdCards", idcardsName),
    
    };
    
    var rawPassword = StaffIdGenerator();
    var Auth = new AuthenticationModel{
        UserId = tutor.StaffID,
        Role = tutor.Role,
        Name = tutor.FirstName+" " +tutor.OtherName+" " +tutor.LastName,
        UserPassword = BCrypt.Net.BCrypt.HashPassword(rawPassword),
        RawPassword = rawPassword
    };

    var Only = new OnlySuperiorsCanViewThisDueToSecurityReasonsNtia{
         UserId = tutor.StaffID,
        Role = tutor.Role,
        Name = tutor.FirstName+" " +tutor.OtherName+" " +tutor.LastName,
        UserPassword = rawPassword,
    };

    context.OnlySuperiorsCanViewThisDueToSecurityReasons.Add(Only);
    
    context.AuthenticationModels.Add(Auth);
    context.Teachers.Add(tutor);
    await context.SaveChangesAsync();
    
await AdminAuditor(ID, constant.AdmitTeacher);

   

return Ok(tutor);
}

private string StaffIdGenerator()
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
[HttpGet("viewTeacher")]
public async Task<IActionResult> ViewTeacher([FromForm]TeacherDto request, string StaffId, string ID){
var teacher =  context.Teachers.FirstOrDefault(t=>t.StaffID == StaffId);
if(teacher == null){
    return BadRequest("Teacher not found");
}
return Ok(teacher);

}



[HttpPost("updateTeacher")]
public async Task<IActionResult> UpdateTeacher([FromForm]TeacherDto request, string StaffId, string ID){
var teacher =  context.Teachers.FirstOrDefault(t=>t.StaffID == StaffId);
if(teacher == null){
    return BadRequest("Teacher not found");
}

teacher.Title = request.Title;
teacher.FirstName = request.FirstName;
teacher.LastName = request.LastName;
teacher.Email = request.Email;
teacher.PhoneNumber = request.PhoneNumber;
teacher.OtherName = request.OtherName;
teacher.DateOfBirth = request.DateOfBirth;
teacher.Gender = request.Gender;
teacher.MaritalStatus = request.MaritalStatus;
teacher.Location = request.Location;
teacher.Country = request.Country;
teacher.PhoneNumber = request.PhoneNumber;
teacher.Email = request.Email;
teacher.Education = request.Education;
teacher.TeachingExperience = request.TeachingExperience;
teacher.TaxNumber = request.TaxNumber;
teacher.SSNITNumber = request.SSNITNumber; 
teacher.HealthStatus = request.HealthStatus;
teacher.EmergencyContacts = request.EmergencyContacts;
teacher.Salary = request.Salary;
teacher.Position = request.Position;
teacher.ReportingTime = request.ReportingTime;
teacher.EmergencyPhone = request.EmergencyPhone;


await context.SaveChangesAsync();
await AdminAuditor(ID, constant.UpdaTeacher);

return Ok("Tutor successfully Updated");
}

[HttpGet("getTeachers")]
public async Task<IActionResult> GetTeachers(string SID){
    var teacherList = context.Teachers.ToList();
    await AdminAuditor(SID,constant.GetTeacher);
    return Ok(teacherList);
}

[HttpGet("getOneTeacher")]
public async Task<IActionResult> GetSpecificUser(string StaffID, string ID){
var tutor = context.Teachers.FirstOrDefault(e=>e.StaffID == StaffID);
await AdminAuditor(ID, constant.ViewOneTeacher);
return Ok(tutor);
}


[HttpDelete("deleteTeacherAccount")]
public async Task<IActionResult> DeleteSpecificUser(string StaffID, string ID){
var tutor =  context.Teachers.FirstOrDefault(e=>e.StaffID == StaffID);
if(tutor == null){
    return BadRequest("Teacher does not exist");
}
context.Teachers.Remove(tutor);
await context.SaveChangesAsync();
await AdminAuditor(ID, constant.DeletedTeacher);
return Ok("Teacher Account Deleted");
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
        Level = user.Level,
        ProfilePic = user.ProfilePic

    };

    context.AuditTrials.Add(audit);
    await context.SaveChangesAsync();

   
}

[ApiExplorerSettings(IgnoreApi = true)] 
public async Task AdminAuditor(string StaffId, string Action)
{
    var user = context.Admins.FirstOrDefault(a => a.AdminID == StaffId);
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
        ProfilePic = user.ProfilePic
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
        Level = user.Level,
        ProfilePic= user.ProfilePic

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