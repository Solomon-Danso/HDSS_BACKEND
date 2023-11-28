using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using HDSS_BACKEND.Data;
using HDSS_BACKEND.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HDSS_BACKEND.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SetupController : ControllerBase
    {
        private readonly DataContext context;
         Constants constant = new Constants();
          string Country;
        string City;
        double latitude;
        double logitude;
        public SetupController(DataContext ctx){
            context = ctx;
        }



      [HttpPost("RegisterSuperior")]
        public async Task<IActionResult>RegisterSuperior([FromForm]SuperiorAccountDto request){
            var count = context.SuperiorAccounts.Count();
            if(count >0){
                return BadRequest("Only one Superior account can be registered");
            }

             if (request.File == null || request.File.Length == 0)
    {
        return BadRequest("Invalid file");
    }

    // Create the uploads directory if it doesn't exist
    var uploadsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Superior", "Profile");
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

            var supi =  new SuperiorAccount{
                StaffID = AdminIdGenerator(),
                Name = request.Name,
                Contact = request.Contact,
                Email = request.Email,
                Role = constant.Admin,
                SpecificRole = constant.SuperiorUser,
                ProfilePicturePath = Path.Combine("Superior/Profile", fileName),

            };
            var Auth = new AuthenticationModel{
                Name = supi.Name,
                UserId = supi.Email,
                Role = supi.Role,
                UserPassword = BCrypt.Net.BCrypt.HashPassword(request.Password),
                SpecificUserRole = supi.SpecificRole

            };
            var admin = new Admin{
                Name = supi.Name,
                Contact = supi.Contact,
                Email = supi.Email,
                AdminID =supi.StaffID,
                Role = supi.Role,
                SpecificRole = supi.SpecificRole,
                ProfilePic = supi.ProfilePicturePath,
            };

            context.Admins.Add(admin);
            context.AuthenticationModels.Add(Auth);
            context.SuperiorAccounts.Add(supi);
            await context.SaveChangesAsync();

            await  AdminAuditor(supi.StaffID, constant.SuperiorAccount);

            return Ok("Superior Account Created Successfully");
        }

        [HttpPost("ChangePicture")]

        public async Task<IActionResult> ChangePicture(string Email,[FromForm]SuperiorAccountDto request ){
        
        
        var user = context.SuperiorAccounts.FirstOrDefault(a => a.Email == Email);
        if (user == null){
            return BadRequest("Account does not exist");
        }
        
        if (request.File == null || request.File.Length == 0)
    {
        return BadRequest("Invalid file");
    }

    // Create the uploads directory if it doesn't exist
    var uploadsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Superior", "Profile");
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

    user.ProfilePicturePath = Path.Combine("Superior/Profile", fileName);
    await context.SaveChangesAsync();
     await  AdminAuditor(user.StaffID, constant.ChangePicture);
    return Ok("Profile Picture Updated");

        }

    [HttpPost("ChangeDetails")]
    public async Task<IActionResult> ChangeDetails(string Email, SuperiorAccountDto request){
        var user = context.SuperiorAccounts.FirstOrDefault(a => a.Email == Email);
        if (user == null){
            return BadRequest("Account does not exist");
        }
        user.Name = request.Name;
        user.Email = request.Email;
        user.Contact = request.Contact;
        await context.SaveChangesAsync();
        await  AdminAuditor(user.StaffID, constant.ChangeDetails);
   
        return Ok("User details changed successfully");
    }

[HttpPost("ChangePassword")]

 public async Task<IActionResult> ChangePassword(string Email, SuperiorAccountDto request){
         var authuser = context.AuthenticationModels.FirstOrDefault(a=>a.UserId == request.Email);
        if ( authuser == null){
            return BadRequest("Account does not exist");
        }
       authuser.UserPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);
        await context.SaveChangesAsync();
         await  AuthAuditor(authuser.UserId, constant.ChangePassword);
   
        return Ok("User details changed successfully");
    }







    [HttpPost("registerInstitution")]
        public async Task<IActionResult> Institution([FromForm]InstituitionDto request){
    


    if (request.File == null || request.File.Length == 0)
    {
        return BadRequest("Invalid file");
    }

    // Create the uploads directory if it doesn't exist
    var uploadsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Institution", "Profile");
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

 var insti = new Instituition
    {
        Logo = Path.Combine("Institution/Profile", fileName),
        SchoolName = request.SchoolName,
        Location = request.Location,
        AdminName = request.AdminName,
        Position = request.Position,
       
    };

    var counter = context.Instituitions.Count();
    if(counter>0){
        var ist = context.Instituitions.FirstOrDefault(r=>r.Id>0);
        if (ist == null){
            return BadRequest("Data not found");
        }
        ist.Logo = insti.Logo;
        ist.SchoolName = insti.SchoolName;
        ist.Location = insti.Location;
        ist.AdminName = insti.AdminName;
        ist.Position = insti.Position;
        
    }
    else{
        context.Instituitions.Add(insti);
    }

    
    await context.SaveChangesAsync();
    return Ok(insti);
    
    }

[HttpGet("GetSchoolData")]
public async Task<IActionResult>GetSchool(){
    var inst = context.Instituitions.FirstOrDefault(a=>a.Id>0);
    return Ok(inst);

}



[HttpPost("AssignRoles")]
public async Task<IActionResult>AssignRoles([FromBody]Role request){

var role = new Role{
FullName = request.FullName,
StaffId = request.StaffId,
Position = request.Position
};
bool checker = await context.Roles.AnyAsync(a=>a.FullName==role.FullName&&a.StaffId==role.StaffId&&a.Position==role.Position);
if (checker){
    return BadRequest("User With That Role already exists");
}
else{
context.Roles.Add(role);
await context.SaveChangesAsync();
}


return Ok(role);


}

[HttpGet("UserRoles")]
public async Task<IActionResult>UserRoles(){
    var userRole = context.Roles.OrderBy(x=>x.Position).ToList();
    return Ok(userRole);
}

[HttpGet("SpecificUserRole")]
public async Task<IActionResult>SpecificUser(string UserId){
    var userRole = context.Roles.Where(x=>x.StaffId==UserId).ToList();
    return Ok(userRole);
}

[HttpDelete("DeleteSpecificUserRole")]
public async Task<IActionResult>DeleteSpecificUser(string UserId, string Position){
    var userRole = context.Roles.FirstOrDefault(x=>x.StaffId==UserId&&x.Position==Position);
    if (userRole == null){
        return BadRequest("User Role not found");
    }
    context.Roles.Remove(userRole);
    await context.SaveChangesAsync();
    return Ok("Deleted Successfully");
}



private string AdminIdGenerator()
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


    var user = context.Teachers.FirstOrDefault(a => a.StaffID==StudentId);
    if (user ==null){
         BadRequest("Teacher not found");
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
       
        ProfilePic= user.FilePath

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