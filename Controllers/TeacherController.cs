using System;
using System.Collections.Generic;
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
    [Route("api/Admin")]

    public class TeacherController : ControllerBase
    {
       private readonly DataContext context;
        public TeacherController(DataContext ctx){
            context = ctx;

        }          

[HttpPost("registerTeacher")]
public async Task<IActionResult> RegisterTeacher([FromForm] TeacherDto request)
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




if (request.AppointFile == null || request.AppointFile.Length == 0)
    {
        return BadRequest("Invalid file");
    }

    // Create the uploads directory if it doesn't exist
    var uploadsAppointDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot","Teachers", "Appoint");
    if (!Directory.Exists(uploadsAppointDirectory))
    {
        Directory.CreateDirectory(uploadsAppointDirectory);
    }

    // Get the original file extension
    var appointExtension = Path.GetExtension(request.AppointFile.FileName);

    // Generate a unique file name
    var appointName = Guid.NewGuid().ToString() + appointExtension;

    // Save the uploaded file to the uploads directory
    var appointPath = Path.Combine(uploadsAppointDirectory, appointName);
    using (var stream = new FileStream(appointPath, FileMode.Create))
    {
        await request.AppointFile.CopyToAsync(stream);
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
        Salary = request.Salary,
       
        StaffID = StaffIdGenerator(),
        DateAdded = DateTime.Today.Date.ToString("dd MMMM, yyyy"),
        FilePath = Path.Combine("Teachers/Profile", fileName),
        CertPath = Path.Combine("Teachers/Certificates", certName),
        IdCards = Path.Combine("Teachers/IdCards", idcardsName),
        AppointmentLetter = Path.Combine("Teachers/Appoint",appointName),
    };
    
    if (StaffID){
        tutor.StaffID = StaffIdGenerator();
    }

    context.Teachers.Add(tutor);
    await context.SaveChangesAsync();

    return Ok("Tutor admitted successfully");
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

[HttpPost("updateTeacher")]
public async Task<IActionResult> UpdateTeacher(TeacherDto request, string StaffId){
var teacher =  context.Teachers.FirstOrDefault(t=>t.StaffID == StaffId);
if(teacher == null){
    return BadRequest("Teacher not found");
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

teacher.FilePath = Path.Combine("Teachers/Profile", fileName);
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


await context.SaveChangesAsync();


return Ok("Tutor successfully Updated");
}

[HttpGet("getTeachers")]
public async Task<IActionResult> GetTeachers(){
    var teacherList = context.Teachers.ToList();
    
    return Ok(teacherList);
}

[HttpGet("getSpecificUser")]
public async Task<IActionResult> GetSpecificUser(string StaffID){
var tutor = context.Teachers.FirstOrDefault(e=>e.StaffID == StaffID);
return Ok(tutor);
}


[HttpDelete("deleteSpecificUser")]
public async Task<IActionResult> DeleteSpecificUser(string StaffID){
var tutor =  context.Teachers.FirstOrDefault(e=>e.StaffID == StaffID);
if(tutor == null){
    return BadRequest("Teacher does not exist");
}
context.Teachers.Remove(tutor);
await context.SaveChangesAsync();

return Ok("Teacher Account Deleted");
}





    }
}