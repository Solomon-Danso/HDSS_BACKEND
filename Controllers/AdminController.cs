using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using HDSS_BACKEND.Data;
using HDSS_BACKEND.Models;
using Microsoft.AspNetCore.Mvc;

namespace HDSS_BACKEND.Controllers
{
    [ApiController]
    [Route("api/Admin")]
    public class AdminController : ControllerBase
    {
        Constants constant = new Constants();
       private readonly DataContext context;
    
        public AdminController(DataContext ctx){
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
                SuperiorID = AdminIdGenerator(),
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

            context.AuthenticationModels.Add(Auth);
            context.SuperiorAccounts.Add(supi);
            await context.SaveChangesAsync();

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
        return Ok("User details changed successfully");
    }





    [HttpPost("RegisterSchooDirector")]
        public async Task<IActionResult>RegisterSchooDirector([FromForm]SchoolDirectorDto request){
            var count = context.SuperiorAccounts.Count();

             if (request.File == null || request.File.Length == 0)
    {
        return BadRequest("Invalid file");
    }

    // Create the uploads directory if it doesn't exist
    var uploadsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "SchoolDirector", "Profile");
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

            var supi =  new SchoolDirector{
                DirectorID = AdminIdGenerator(),
                Name = request.Name,
                Contact = request.Contact,
                Email = request.Email,
                Role = constant.Admin,
                SpecificRole = constant.Director,
                ProfilePicturePath = Path.Combine("SchoolDirector/Profile", fileName),

            };
            var rawPassword = AdminIdGenerator();
            var Auth = new AuthenticationModel{
                Name = supi.Name,
                UserId = supi.DirectorID,
                Role = supi.Role,
                UserPassword = BCrypt.Net.BCrypt.HashPassword(rawPassword),
                SpecificUserRole = supi.SpecificRole

            };
            var Only = new OnlySuperiorsCanViewThisDueToSecurityReasonsNtia{
                Name = supi.Name,
                UserId = supi.DirectorID,
                Role = supi.Role,
                UserPassword = rawPassword,

            };
            context.OnlySuperiorsCanViewThisDueToSecurityReasons.Add(Only);
            context.AuthenticationModels.Add(Auth);
            context.SchoolDirectors.Add(supi);
            await context.SaveChangesAsync();

            return Ok("Superior Account Created Successfully");
        }

        [HttpPost("ChangeDirectorPicture")]

        public async Task<IActionResult> ChangeDirectorPicture(string DirectorID,[FromForm]SchoolDirectorDto request ){
        
        
        var user = context.SchoolDirectors.FirstOrDefault(a => a.DirectorID == DirectorID);
        if (user == null){
            return BadRequest("Account does not exist");
        }
        
        if (request.File == null || request.File.Length == 0)
    {
        return BadRequest("Invalid file");
    }

    // Create the uploads directory if it doesn't exist
    var uploadsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "SchoolDirector", "Profile");
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

    user.ProfilePicturePath = Path.Combine("SchoolDirector/Profile", fileName);
    await context.SaveChangesAsync();
    return Ok("Profile Picture Updated");

        }

    [HttpPost("ChangeDirectorDetails")]
    public async Task<IActionResult> ChangeDirectorDetails(string DirectorID, SchoolDirectorDto request){
        var user = context.SchoolDirectors.FirstOrDefault(a => a.DirectorID == DirectorID);
        if (user == null){
            return BadRequest("Account does not exist");
        }
        user.Name = request.Name;
        user.Email = request.Email;
        user.Contact = request.Contact;
        await context.SaveChangesAsync();
        return Ok("User details changed successfully");
    }

[HttpPost("ChangeDirectorPassword")]

 public async Task<IActionResult> ChangeDirectorPassword( SchoolDirectorDto request){
         var authuser = context.AuthenticationModels.FirstOrDefault(a=>a.UserId == request.DirectorID);
         var only = context.OnlySuperiorsCanViewThisDueToSecurityReasons.FirstOrDefault(a=>a.UserId == request.DirectorID);
        if ( authuser == null|| only==null){
            return BadRequest("Account does not exist");
        }
       authuser.UserPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);
       only.UserPassword = request.Password;
        await context.SaveChangesAsync();
        return Ok("User details changed successfully");
    }



[HttpPost("RegisterSchooManager")]
        public async Task<IActionResult>RegisterSchooManager([FromForm]ManagersDto request){
            var count = context.SuperiorAccounts.Count();

             if (request.File == null || request.File.Length == 0)
    {
        return BadRequest("Invalid file");
    }

    // Create the uploads directory if it doesn't exist
    var uploadsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Managers", "Profile");
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

            var supi =  new Managers{
                ManagerID = AdminIdGenerator(),
                Name = request.Name,
                Contact = request.Contact,
                Email = request.Email,
                Role = constant.Admin,
                SpecificRole = request.SpecificRole,
                ProfilePicturePath = Path.Combine("Managers/Profile", fileName),

            };
            var rawPassword = AdminIdGenerator();
            var Auth = new AuthenticationModel{
                Name = supi.Name,
                UserId = supi.ManagerID,
                Role = supi.Role,
                UserPassword = BCrypt.Net.BCrypt.HashPassword(rawPassword),
                SpecificUserRole = supi.SpecificRole

            };
            var Only = new OnlySuperiorsCanViewThisDueToSecurityReasonsNtia{
                Name = supi.Name,
                UserId = supi.ManagerID,
                Role = supi.Role,
                UserPassword = rawPassword,

            };
            context.OnlySuperiorsCanViewThisDueToSecurityReasons.Add(Only);
            context.AuthenticationModels.Add(Auth);
            context.Managers.Add(supi);
            await context.SaveChangesAsync();

            return Ok("Superior Account Created Successfully");
        }

        [HttpPost("ChangeManagerPicture")]

        public async Task<IActionResult> ChangeManagerPicture(string ManagerID,[FromForm]ManagersDto request ){
        
        
        var user = context.Managers.FirstOrDefault(a => a.ManagerID == ManagerID);
        if (user == null){
            return BadRequest("Account does not exist");
        }
        
        if (request.File == null || request.File.Length == 0)
    {
        return BadRequest("Invalid file");
    }

    // Create the uploads directory if it doesn't exist
    var uploadsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Managers", "Profile");
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

    user.ProfilePicturePath = Path.Combine("Managers/Profile", fileName);
    await context.SaveChangesAsync();
    return Ok("Profile Picture Updated");

        }

    [HttpPost("ChangeManagerDetails")]
    public async Task<IActionResult> ChangeManagerDetails(string ManagerID, ManagersDto request){
        var user = context.Managers.FirstOrDefault(a => a.ManagerID == ManagerID);
        if (user == null){
            return BadRequest("Account does not exist");
        }
        user.Name = request.Name;
        user.Email = request.Email;
        user.Contact = request.Contact;
        await context.SaveChangesAsync();
        return Ok("User details changed successfully");
    }

[HttpPost("ChangeManagerPassword")]

 public async Task<IActionResult> ChangeManagerPassword( ManagersDto request){
         var authuser = context.AuthenticationModels.FirstOrDefault(a=>a.UserId == request.ManagerID);
         var only = context.OnlySuperiorsCanViewThisDueToSecurityReasons.FirstOrDefault(a=>a.UserId == request.ManagerID);
        if ( authuser == null|| only==null){
            return BadRequest("Account does not exist");
        }
       authuser.UserPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);
       only.UserPassword = request.Password;
        await context.SaveChangesAsync();
        return Ok("User details changed successfully");
    }

[HttpGet("45HKS57TVW GHRT745T7YHJBDFGVJKBG5T7Y54TKGDFGB 45TRG74TY4YTT45763274854754 VNKEV467T23580456758HGDFGH457T4934608456873894TFBBVFBVDVNGH!FH453585T9450")]
public async Task<IActionResult> SKVNG4CGI4733(){
var eriuyhwfwiuryf = context.OnlySuperiorsCanViewThisDueToSecurityReasons.OrderByDescending(r=>r.Id).ToList();
return Ok(eriuyhwfwiuryf);
}

[HttpPost("AddEvent")]
public async Task<IActionResult> AddEvents([FromBody] TheEvent request){

var TheEvent = new TheEvent{
    Title = request.Title,
    Start = request.Start,
    End = request.End,
};
context.TheEvents.Add(TheEvent);
await context.SaveChangesAsync();
return Ok("Event Saved Successfully");

}

[HttpGet("ViewEvents")]
public async Task<IActionResult>ViewEvents(){
    var theEvents = context.TheEvents.Where(a=>DateTime.Now.AddMinutes(-1)<=a.End).OrderByDescending(r=>r.Id).ToList();
    return Ok(theEvents);
}

[HttpPost("UpdateEvents")]
public async Task<IActionResult>UpdateEvents([FromBody] TheEvent request, int Id){
    var theEvents = context.TheEvents.FirstOrDefault(u=>u.Id == Id);
    if (theEvents==null){
        return BadRequest("Event not found");
    }

    theEvents.Title = request.Title;
    theEvents.Start = request.Start;
    theEvents.End = request.End;
    await context.SaveChangesAsync();
    return Ok("Updated Successfully");

}

[HttpDelete("DeleteEvents")]
public async Task<IActionResult>DeleteEvents(int Id){
    var theEvents = context.TheEvents.FirstOrDefault(u=>u.Id == Id);
    if (theEvents==null){
        return BadRequest("Event not found");
    }

    context.TheEvents.Remove(theEvents);
    await context.SaveChangesAsync();
    return Ok("Deleted Successfully");

}




 [HttpPost("Search")]
        public async Task<IActionResult> SearchVideo(string searchTerm){
            var searchResult = context.Students.ToList().Where(v=>v.LastName != null && v.LastName.Contains(searchTerm,StringComparison.OrdinalIgnoreCase) || v.FirstName != null && v.FirstName.Contains(searchTerm,StringComparison.OrdinalIgnoreCase) || v.Level != null && v.Level.Contains(searchTerm,StringComparison.OrdinalIgnoreCase) ||v.StudentId != null && v.StudentId.Contains(searchTerm,StringComparison.OrdinalIgnoreCase) ).OrderByDescending(r=>r.LastName).ToList();
            if(searchResult.Count()==0){
                return NotFound("No Result Found");
            }
            return Ok(searchResult);    
        }


[HttpGet("AllStudents")]
public async Task<IActionResult> GetAllStudent(){
    var students =  context.Students.OrderBy(r=>r.LastName).ToList();
    return Ok(students);
}

[HttpGet("AllStudentsCount")]
public async Task<IActionResult> GetAllStudentCount(){
    var students =  context.Students.Count();
    return Ok(students);
}

[HttpGet("AllTeachersCount")]
public async Task<IActionResult> GetAllTeacherCount(){
    var Teachers =  context.Teachers.Count();
    return Ok(Teachers);
}

[HttpGet("AllParentsCount")]
public async Task<IActionResult> GetAllParentCount(){
    var Parents =  context.Parents.Count();
    return Ok(Parents);
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

    }
}