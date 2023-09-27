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
    [Route("api/[controller]")]
    public class SetupController : ControllerBase
    {
        private readonly DataContext context;
        public SetupController(DataContext ctx){
            context = ctx;
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
        Position = request.Position
        
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


[HttpPost("AdmissionLetter")]
public async Task<IActionResult> AdmissionLetter(string StudentId){

var student = context.Students.FirstOrDefault(r=>r.StudentId==StudentId);
var auth = context.AuthenticationModels.FirstOrDefault(r=>r.UserId==StudentId);
var inst = context.Instituitions.FirstOrDefault(r=>r.Id>0);
if (student == null|| inst==null || auth==null){
    return BadRequest("Student not found");
}

var admission = new AdmissionLetter{
StudentId = student.StudentId,

SchoolName = inst.SchoolName,
Location = inst.Location,
Name = $"Dear {student.FirstName} {student.OtherName} {student.LastName}, ",
Paragraph1 = $"We are delighted to extend an offer of admission to {inst.SchoolName} for this academic year. It brings us great pleasure to welcome you to our school community, and we are excited about the prospect of having you as a student in our school.",
Paragraph2 = $"Your application to {inst.SchoolName} was thoroughly reviewed by our admissions committee, and we were impressed by your academic potential, your eagerness to learn, and the positive qualities you exhibited during the admission process. Your enthusiasm for education and your readiness to engage with the our school curriculum make you an excellent fit for our school.",
Paragraph3 = $"At {inst.SchoolName}, we are committed to providing a nurturing and stimulating learning environment that encourages the holistic development of each child. Our dedicated faculty and staff are excited to work with you to help you reach your full potential academically, socially, and emotionally.",
Paragraph4 = $"Your student identification number is {student.StudentId} This is the number that you will use throughout your studies in this school. Your Password is {auth.RawPassword}, do well not to misplace this information",
Paragraph5 = $"If you have any questions or require further assistance, please feel free to contact our admissions office. We look forward to having you join our {inst.SchoolName} family and embark on an exciting educational adventure. We are confident that your time with us will be filled with learning, growth, and lasting friendships.",
Paragraph6 = $"Congratulations once again on your admission to {inst.SchoolName}. We can't wait to get to know you better and watch you thrive as a member of our school community.",
AdmissionDate = student.AdmissionDate,
AdminName = inst.AdminName,
}
;

context.AdmissionLetters.Add(admission);
await context.SaveChangesAsync();
return Ok(admission);

}













    }
}