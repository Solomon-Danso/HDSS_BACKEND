using System;
using System.Collections.Generic;
using System.Globalization;
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
         string Country;
        string City;
        double latitude;
        double logitude;
    
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





    [HttpPost("RegisterSchooDirector")]
        public async Task<IActionResult>RegisterSchooDirector([FromForm]SchoolDirectorDto request, string ID){
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
            await  AdminAuditor(ID, constant.RegisterSchooDirector);

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
     await  AdminAuditor(DirectorID, constant.UpdateDirector);

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
        await  AdminAuditor(DirectorID, constant.UpdateDirectorInfo);

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
        await  AuthAuditor(authuser.UserId, constant.ChangePassword);
        return Ok("User details changed successfully");
    }


//Start
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

[HttpPost("SearchTeacher")]
        public async Task<IActionResult> SearchTeacher(string searchTerm){
            var searchResult = context.Teachers.ToList().Where(v=>v.LastName != null && v.LastName.Contains(searchTerm,StringComparison.OrdinalIgnoreCase) || v.FirstName != null && v.FirstName.Contains(searchTerm,StringComparison.OrdinalIgnoreCase) || v.Position != null && v.Position.Contains(searchTerm,StringComparison.OrdinalIgnoreCase) ||v.StaffID != null && v.StaffID.Contains(searchTerm,StringComparison.OrdinalIgnoreCase) ).OrderByDescending(r=>r.LastName).ToList();
            if(searchResult.Count()==0){
                return NotFound("No Result Found");
            }
            return Ok(searchResult);    
        }

[HttpPost("SearchClass")]
        public async Task<IActionResult> SearchClass(string searchTerm){
            var searchResult = context.Classess.ToList().Where(
                v=>v.ClassName != null && v.ClassName.Contains(searchTerm,StringComparison.OrdinalIgnoreCase)
                 || v.ClassCode != null && v.ClassCode.Contains(searchTerm,StringComparison.OrdinalIgnoreCase) 
                 || v.Campus != null && v.Campus.Contains(searchTerm,StringComparison.OrdinalIgnoreCase) 
                 ||v.ClassTeacher != null && v.ClassTeacher.Contains(searchTerm,StringComparison.OrdinalIgnoreCase) ).OrderByDescending(r=>r.Id).ToList();
            if(searchResult.Count()==0){
                return NotFound("No Result Found");
            }
            return Ok(searchResult);    
        }

[HttpPost("SearchSubject")]
        public async Task<IActionResult> SearchSubject(string searchTerm){
            var searchResult = context.TeacherInSubjects.ToList().Where(
                v=>v.StaffName != null && v.StaffName.Contains(searchTerm,StringComparison.OrdinalIgnoreCase)
                 || v.SubjectName != null && v.SubjectName.Contains(searchTerm,StringComparison.OrdinalIgnoreCase) 
                 || v.DateAssigned != null && v.DateAssigned.Contains(searchTerm,StringComparison.OrdinalIgnoreCase)
                 || v.ClassName != null && v.ClassName.Contains(searchTerm,StringComparison.OrdinalIgnoreCase) 
                 || v.StaffID != null && v.StaffID.Contains(searchTerm,StringComparison.OrdinalIgnoreCase)
                
                 ).OrderByDescending(r=>r.Id).ToList();
            if(searchResult.Count()==0){
                return NotFound("No Result Found");
            }
            return Ok(searchResult);    
        }


[HttpPost("SearchSlides")]
public async Task<IActionResult> SearchSlides(string searchTerm, string StaffID)
{
    var searchResult = context.Slides.ToList().Where(
        v => v.Title != null && v.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
             || v.SubjectName != null && v.SubjectName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
             || v.DateAdded != null && v.DateAdded.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
             || v.ClassName != null && v.ClassName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
             || v.AcademicTerm != null && v.AcademicTerm.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
             || v.AcademicYear != null && v.AcademicYear.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
             || v.TeacherName != null && v.TeacherName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
    ).OrderByDescending(r => r.Id).ToList();

    if (!string.IsNullOrEmpty(StaffID))
    {
        searchResult = searchResult
            .Where(s => s.StaffID != null && s.StaffID.Equals(StaffID, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    if (searchResult.Count() == 0)
    {
        return NotFound("No Result Found");
    }

    return Ok(searchResult);
}

[HttpPost("SearchAssignments")]
public async Task<IActionResult> SearchAssignments(string searchTerm, string StaffID)
{
    var searchResult = context.Assignments.ToList().Where(
        v => v.Title != null && v.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
             || v.SubjectName != null && v.SubjectName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
             || v.DateAdded != null && v.DateAdded.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
             || v.ClassName != null && v.ClassName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
             || v.AcademicTerm != null && v.AcademicTerm.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
             || v.AcademicYear != null && v.AcademicYear.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
             || v.TeacherName != null && v.TeacherName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
            
    ).OrderByDescending(r => r.Id).ToList();

    if (!string.IsNullOrEmpty(StaffID))
    {
        searchResult = searchResult
            .Where(s => s.StaffID != null && s.StaffID.Equals(StaffID, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    if (searchResult.Count() == 0)
    {
        return NotFound("No Result Found");
    }

    return Ok(searchResult);
}


[HttpPost("SearchVideo")]
public async Task<IActionResult> SearchVideo(string searchTerm, string StaffID)
{
    var searchResult = context.Videos.ToList().Where(
        v => v.Title != null && v.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
             || v.SubjectName != null && v.SubjectName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
             || v.DateAdded != null && v.DateAdded.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
             || v.ClassName != null && v.ClassName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
             || v.AcademicTerm != null && v.AcademicTerm.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
             || v.AcademicYear != null && v.AcademicYear.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
             || v.TeacherName != null && v.TeacherName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
    ).OrderByDescending(r => r.Id).ToList();

    if (!string.IsNullOrEmpty(StaffID))
    {
        searchResult = searchResult
            .Where(s => s.StaffID != null && s.StaffID.Equals(StaffID, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    if (searchResult.Count() == 0)
    {
        return NotFound("No Result Found");
    }

    return Ok(searchResult);
}


[HttpPost("SearchAudio")]
public async Task<IActionResult> SearchAudio(string searchTerm, string StaffID)
{
    var searchResult = context.Audios.ToList().Where(
        v => v.Title != null && v.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
             || v.SubjectName != null && v.SubjectName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
             || v.DateAdded != null && v.DateAdded.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
             || v.ClassName != null && v.ClassName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
             || v.AcademicTerm != null && v.AcademicTerm.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
             || v.AcademicYear != null && v.AcademicYear.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
             || v.TeacherName != null && v.TeacherName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
    ).OrderByDescending(r => r.Id).ToList();

    if (!string.IsNullOrEmpty(StaffID))
    {
        searchResult = searchResult
            .Where(s => s.StaffID != null && s.StaffID.Equals(StaffID, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    if (searchResult.Count() == 0)
    {
        return NotFound("No Result Found");
    }

    return Ok(searchResult);
}

[HttpPost("SearchPicture")]
public async Task<IActionResult> SearchPicture(string searchTerm, string StaffID)
{
    var searchResult = context.Pictures.ToList().Where(
        v => v.Title != null && v.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
             || v.SubjectName != null && v.SubjectName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
             || v.DateAdded != null && v.DateAdded.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
             || v.ClassName != null && v.ClassName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
             || v.AcademicTerm != null && v.AcademicTerm.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
             || v.AcademicYear != null && v.AcademicYear.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
             || v.TeacherName != null && v.TeacherName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
    ).OrderByDescending(r => r.Id).ToList();

    if (!string.IsNullOrEmpty(StaffID))
    {
        searchResult = searchResult
            .Where(s => s.StaffID != null && s.StaffID.Equals(StaffID, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    if (searchResult.Count() == 0)
    {
        return NotFound("No Result Found");
    }

    return Ok(searchResult);
}



[HttpPost("SearchBook")]
public async Task<IActionResult> SearchBook(string searchTerm, string StaffID)
{
    var searchResult = context.Books.ToList().Where(
        v => v.Title != null && v.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
             || v.SubjectName != null && v.SubjectName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
             || v.DateAdded != null && v.DateAdded.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
             || v.ClassName != null && v.ClassName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
             || v.AcademicTerm != null && v.AcademicTerm.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
             || v.AcademicYear != null && v.AcademicYear.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
             || v.TeacherName != null && v.TeacherName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
    ).OrderByDescending(r => r.Id).ToList();

    if (!string.IsNullOrEmpty(StaffID))
    {
        searchResult = searchResult
            .Where(s => s.StaffID != null && s.StaffID.Equals(StaffID, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    if (searchResult.Count() == 0)
    {
        return NotFound("No Result Found");
    }

    return Ok(searchResult);
}



[HttpPost("AdminSearchSlides")]
public async Task<IActionResult> AdminSearchSlides(string searchTerm)
{
    var searchResult = context.Slides.ToList().Where(
        v => v.Title != null && v.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
             || v.SubjectName != null && v.SubjectName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
             || v.DateAdded != null && v.DateAdded.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
             || v.ClassName != null && v.ClassName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
             || v.AcademicTerm != null && v.AcademicTerm.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
             || v.AcademicYear != null && v.AcademicYear.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
             || v.TeacherName != null && v.TeacherName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
    ).OrderByDescending(r => r.Id).ToList();


    if (searchResult.Count() == 0)
    {
        return NotFound("No Result Found");
    }

    return Ok(searchResult);
}

[HttpPost("AdminSearchAudios")]
public async Task<IActionResult> AdminSearchAudios(string searchTerm)
{
    var searchResult = context.Audios.ToList().Where(
        v => v.Title != null && v.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
             || v.SubjectName != null && v.SubjectName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
             || v.DateAdded != null && v.DateAdded.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
             || v.ClassName != null && v.ClassName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
             || v.AcademicTerm != null && v.AcademicTerm.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
             || v.AcademicYear != null && v.AcademicYear.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
             || v.TeacherName != null && v.TeacherName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
    ).OrderByDescending(r => r.Id).ToList();


    if (searchResult.Count() == 0)
    {
        return NotFound("No Result Found");
    }

    return Ok(searchResult);
}

[HttpPost("AdminSearchVideos")]
public async Task<IActionResult> AdminSearchVideos(string searchTerm)
{
    var searchResult = context.Videos.ToList().Where(
        v => v.Title != null && v.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
             || v.SubjectName != null && v.SubjectName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
             || v.DateAdded != null && v.DateAdded.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
             || v.ClassName != null && v.ClassName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
             || v.AcademicTerm != null && v.AcademicTerm.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
             || v.AcademicYear != null && v.AcademicYear.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
             || v.TeacherName != null && v.TeacherName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
    ).OrderByDescending(r => r.Id).ToList();


    if (searchResult.Count() == 0)
    {
        return NotFound("No Result Found");
    }

    return Ok(searchResult);
}


[HttpPost("AdminSearchBooks")]
public async Task<IActionResult> AdminSearchBooks(string searchTerm)
{
    var searchResult = context.Books.ToList().Where(
        v => v.Title != null && v.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
             || v.SubjectName != null && v.SubjectName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
             || v.DateAdded != null && v.DateAdded.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
             || v.ClassName != null && v.ClassName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
             || v.AcademicTerm != null && v.AcademicTerm.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
             || v.AcademicYear != null && v.AcademicYear.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
             || v.TeacherName != null && v.TeacherName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
    ).OrderByDescending(r => r.Id).ToList();


    if (searchResult.Count() == 0)
    {
        return NotFound("No Result Found");
    }

    return Ok(searchResult);
}


[HttpPost("AdminSearchPictures")]
public async Task<IActionResult> AdminSearchPictures(string searchTerm)
{
    var searchResult = context.Pictures.ToList().Where(
        v => v.Title != null && v.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
             || v.SubjectName != null && v.SubjectName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
             || v.DateAdded != null && v.DateAdded.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
             || v.ClassName != null && v.ClassName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
             || v.AcademicTerm != null && v.AcademicTerm.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
             || v.AcademicYear != null && v.AcademicYear.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
             || v.TeacherName != null && v.TeacherName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
    ).OrderByDescending(r => r.Id).ToList();


    if (searchResult.Count() == 0)
    {
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