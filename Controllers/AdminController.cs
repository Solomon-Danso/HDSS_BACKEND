using System;
using System.Collections.Generic;
using System.Linq;
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
       private readonly DataContext _context;
        public AdminController(DataContext context){
            _context = context;

        }          

[HttpPost("registerTeacher")]
public async Task<IActionResult> UploadPicture([FromForm] TeacherDto request)
{
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
        Status = request.Status,
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
        SchoolBankAccount = request.SchoolBankAccount,
        ClassToTeaches = request.ClassToTeaches,
        SubjectToTeaches = request.SubjectToTeaches,
        StaffID = request.StaffID,
        DateAdded = DateTime.Today.Date.ToString("dd MMMM, yyyy"),
        FilePath = Path.Combine("Teachers/Profile", fileName),
        CertPath = Path.Combine("Teachers/Certificates", certName),
        IdCards = Path.Combine("Teachers/IdCards", idcardsName),
        AppointmentLetter = Path.Combine("Teachers/Appoint",appointName),
    };

    _context.Teachers.Add(tutor);
    await _context.SaveChangesAsync();

    return Ok("Tutor admitted successfully");
}

    }
}