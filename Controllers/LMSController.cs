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
    public class LMSController : ControllerBase
    {
        private readonly DataContext context;
         string Country;
        string City;
        double latitude;
        double logitude;
         Constants constant = new Constants();
        public LMSController(DataContext ctx){
            context = ctx;
        }

        [HttpPost("AddSubject")]
        public async Task<IActionResult> AddSubject([FromBody]Subject request, string ID){
           
            var subject = new Subject{
            SubjectName = request.SubjectName,
            DateAdded =  DateTime.Today.Date.ToString("dd MMMM, yyyy"),
            };

            context.Subjects.Add(subject);
             bool subjectAlreadyExists = await context.Subjects.AnyAsync(s=>s.SubjectName == request.SubjectName);
            if (subjectAlreadyExists){
                return BadRequest("Subject already exists");
            }
            await context.SaveChangesAsync();
            await AdminAuditor(ID, constant.AddASubject);
            return Ok($"{subject.SubjectName} created successfully");
        }




        [HttpGet("viewAllSubject")]
        public async Task<IActionResult> ViewAllSubjects(){
            var subject = context.Subjects.OrderByDescending(R=>R.Id).ToList();
             
            return Ok(subject);
        }

        [HttpDelete("DeleteSubject")]
         public async Task<IActionResult> DeleteSubject(int Id, string SID){
            var subject = context.Subjects.FirstOrDefault(r=>r.Id==Id);
            if (subject == null){
                return BadRequest("Subject not found");
            }
            context.Subjects.Remove(subject);
            await context.SaveChangesAsync();
            await AdminAuditor(SID, constant.DeletASubject);
            return Ok("Subject Deleted");
         }




        [HttpPost("AddClass")]
        public async Task<IActionResult>AddClass([FromBody]Classes request, string ID){
            var classy = new Classes{
                ClassName = request.ClassName,
                ClassCode = request.ClassCode,
                Campus = request.Campus,
                ClassTeacher = request.ClassTeacher,
                DateAdded =  DateTime.Today.Date.ToString("dd MMMM, yyyy"),

            };

            var a = context.Teachers.FirstOrDefault(a=>a.StaffID==classy.ClassTeacher);
            if (a==null){
                return BadRequest("Teacher not found");
            }
            classy.ClassTeacher = a.Title+" "+a.FirstName+" "+a.OtherName+" "+a.LastName;
            classy.TeacherId = a.StaffID;

            bool checker = await context.Classess.AnyAsync(a=>a.ClassTeacher==classy.ClassTeacher&&a.TeacherId==classy.TeacherId&&a.ClassName==classy.ClassName);

            if(checker){
                return BadRequest("You cannot assign the same class to a teacher for multiple times");
            }
            else{
            context.Classess.Add(classy);

              bool classAlreadyExist = await context.Classess.AnyAsync(c=>c.ClassName == request.ClassName);
            if (classAlreadyExist){
                return BadRequest("Class already exists");
            }
          
            await context.SaveChangesAsync();
            }

         
            
            await AdminAuditor(ID,constant.AddAClass);
            return Ok($"{classy.ClassName} created successfully");


        }

        [HttpPost("UpdateClass")]
        public async Task<IActionResult>UpdateClass([FromBody]Classes request,int Id, string SID){
            var cla = context.Classess.FirstOrDefault(x=>x.Id==Id);
            if (cla==null){
                return BadRequest("Class does not exist");
            }
            var a = context.Teachers.FirstOrDefault(a=>a.StaffID==request.ClassTeacher);
            if (a==null){
                return BadRequest("Teacher not found");
            }
            cla.ClassName = request.ClassName;
            cla.ClassCode = request.ClassCode;
            cla.Campus = request.Campus;
    
            cla.ClassTeacher = a.Title+" "+a.FirstName+" "+a.OtherName+" "+a.LastName;
            cla.TeacherId = a.StaffID;

            await context.SaveChangesAsync();
            await AdminAuditor(SID,constant.UpdatAClass);
            return Ok("Updated Successfully");
        }

        [HttpDelete("deleteClass")]
        public async Task<IActionResult>DeleteClass(int Id, string SID){
             var cla = context.Classess.FirstOrDefault(x=>x.Id==Id);
            if (cla==null){
                return BadRequest("Class does not exist");
            }
            context.Classess.Remove(cla);
            await context.SaveChangesAsync();
            await AdminAuditor(SID,constant.DeletAClass);
            return Ok("Class Deleted Successfully");
        }

 
          [HttpGet("viewAllClasses")]
        public async Task<IActionResult> ViewAllClasses(){
            var classy = context.Classess.OrderByDescending(R=>R.Id).ToList();
            
            return Ok(classy);
        }

         [HttpGet("countStudentInClass")]
        public async Task<IActionResult> ViewAllClasses(string ClassN){
            var classy = context.Students.Where(x=>x.Level==ClassN).Count();
            
            return Ok(classy);
        }





        [HttpPost("AddTeacherToSubject")]
        public async Task<IActionResult> AddTeacherToSubject(string ID,[FromBody] TeacherInSubject request){
        
       

         var teacherforsub = new TeacherInSubject{
            StaffName = request.StaffName,
            SubjectName = request.SubjectName,
            
            ClassName = request.ClassName,
            DateAssigned = DateTime.Today.Date.ToString("dd MMMM, yyyy")


         };

          var q = context.Teachers.FirstOrDefault(r=>r.StaffID==teacherforsub.StaffName);
        if (q == null){
            return BadRequest("Teacher Not Found");
        }

        teacherforsub.StaffName = q.FirstName+" " + q.OtherName+" " + q.LastName;
        teacherforsub.StaffID = q.StaffID;
        bool checker = await context.TeacherInSubjects.AnyAsync(a=>a.ClassName==teacherforsub.ClassName&&a.StaffID==teacherforsub.StaffID&&a.SubjectName==teacherforsub.SubjectName);
        if (checker){
            return BadRequest("Teacher Already Assigned");
        }
         context.TeacherInSubjects.Add(teacherforsub);
         await context.SaveChangesAsync();
         await AdminAuditor(ID, constant.AssignTeacher);

         return Ok($"{teacherforsub.StaffName} has been assigned to {teacherforsub.SubjectName}");

        }
 
        [HttpGet("AllSubjectTeachers")]
        public async Task<IActionResult>AllSubjectTeachers(){
            var t = context.TeacherInSubjects.OrderByDescending(r=>r.Id).ToList();
            return Ok(t);
        }
      
  
        
         [HttpDelete("deleteTeacherFromClass")]
        public async Task<IActionResult>DeleteTeacherFromClass(int Id, string SID){
             var cla = context.TeacherInSubjects.FirstOrDefault(x=>x.Id==Id);
            if (cla==null){
                return BadRequest("Teacher does not exist");
            }
            context.TeacherInSubjects.Remove(cla);
            await context.SaveChangesAsync();
            await AdminAuditor(SID,constant.DeletASubT);
            return Ok("Teacher Deleted Successfully");
        }

      


      
  

   
  
    [HttpPost("AddAcademicYear")]
    public async Task<IActionResult> AddAcademicYear([FromBody]AcademicYear request){
        var year = new AcademicYear{
         academicYear = request.academicYear
        };
        context.AcademicYears.Add(year);
      await context.SaveChangesAsync();
      return Ok($"The academic year for {year.academicYear} has been added.");

    }

    [HttpGet("ViewAcademicYear")]
    public async Task<IActionResult> ViewAcademicYear(){
        var year = context.AcademicYears.OrderByDescending(r=>r.Id).ToList();
        return Ok(year);
    }

    [HttpPost("AddAcademicTerm")]
    public async Task<IActionResult> AddAcademicTerm([FromBody]AcademicTerm request){
        var Term = new AcademicTerm{
         academicTerm = request.academicTerm
        };
        context.AcademicTerms.Add(Term);
      await context.SaveChangesAsync();
      return Ok($"The academic Term for {Term.academicTerm} has been added.");

    }

    [HttpGet("ViewAcademicTerm")]
    public async Task<IActionResult> ViewAcademicTerm(){
        var term = context.AcademicTerms.ToList();
        return Ok(term);
    }


        [HttpGet("ViewTeacherSubject")]
    public async Task<IActionResult> ViewTeacherSubject(string ID){
        var term = context.TeacherInSubjects
        .Where(a=>a.StaffID==ID)
        .GroupBy(a=>a.SubjectName)
        .Select(a=>a.First())
        .ToList();
        return Ok(term);
    }

            [HttpGet("ViewTeacherClass")]
    public async Task<IActionResult> ViewTeacherClass(string ID){
        var term = context.TeacherInSubjects
        .Where(a=>a.StaffID==ID)
        .GroupBy(a=>a.ClassName)
        .Select(a=>a.First())
        .ToList();
        return Ok(term);
    }

        [HttpPost("UploadSlide")]
        public async Task<IActionResult> UploadSlide([FromForm]LMSDto request, string ID){
       
         
         if (request.Slide == null || request.Slide.Length == 0)
    {
        return BadRequest("Invalid slide");
    }

    // Create the uploads directory if it doesn't exist
    var uploadsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "LMS", "Slides");
    if (!Directory.Exists(uploadsDirectory))
    {
        Directory.CreateDirectory(uploadsDirectory);
    }

    // Get the original slide extension
    var slideExtension = Path.GetExtension(request.Slide.FileName);

    // Generate a unique slide name
    var slideName = Guid.NewGuid().ToString() + slideExtension;

    // Save the uploaded slide to the uploads directory
    var slidePath = Path.Combine(uploadsDirectory, slideName);
    using (var stream = new FileStream(slidePath, FileMode.Create))
    {
        await request.Slide.CopyToAsync(stream);
    }
   
   
    var s = new Slide{
        //Select the subject name from an option in the frontend
       SubjectName = request.SubjectName,
       Title = request.Title,
       ClassName = request.ClassName,
       DateAdded = DateTime.Today.Date.ToString("dd MMMM, yyyy"),
       SlidePath = Path.Combine("LMS/Slides", slideName),
      
        AcademicYear = request.AcademicYear,
       AcademicTerm = request.AcademicTerm
    };

    var teacher = context.TeacherInSubjects.FirstOrDefault(a=>a.SubjectName==s.SubjectName&&a.ClassName==s.ClassName&&a.StaffID==ID);
    if (teacher==null){
        return BadRequest("Teacher not found");
    }
    s.StaffID = teacher.StaffID;
    s.TeacherName = teacher.StaffName;



    context.Slides.Add(s);
    await context.SaveChangesAsync();
    await TeacherAuditor(ID, constant.UploadSlides);

    return Ok($"{s.Title} for {s.SubjectName} has been Uploaded successfully");
    
    }

    [HttpGet("ViewAllSlidesTeachers")]
    public async Task<IActionResult>ViewAllSlidesTeachers(string ID){
        var slide = context.Slides.Where(a=>a.StaffID==ID).ToList();
        await TeacherAuditor(ID,constant.ViewUploadedSlides);
        return Ok(slide);
    }

    [HttpDelete("deleteSlides")]
    public async Task<IActionResult>DeleteSlides(int Id, string SID){
        var slide = context.Slides.FirstOrDefault(a=>a.Id==Id);
        if(slide == null){
            return BadRequest("Slides not found");
        }
        context.Slides.Remove(slide);
        await context.SaveChangesAsync();
        try{
        await TeacherAuditor(SID, constant.DeletUploadedSlides);
        }
        catch(Exception ex){
            await AdminAuditor(SID, constant.DeletUploadedSlides);
        }
        
        return Ok("Slides deleted successfully");
    }


        [HttpPost("UploadVideo")]
        public async Task<IActionResult> UploadVideo([FromForm]LMSDto request, string ID){
       
         
         if (request.Slide == null || request.Slide.Length == 0)
    {
        return BadRequest("Invalid Video");
    }

    // Create the uploads directory if it doesn't exist
    var uploadsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "LMS", "Videos");
    if (!Directory.Exists(uploadsDirectory))
    {
        Directory.CreateDirectory(uploadsDirectory);
    }

    // Get the original slide extension
    var slideExtension = Path.GetExtension(request.Slide.FileName);

    // Generate a unique slide name
    var slideName = Guid.NewGuid().ToString() + slideExtension;

    // Save the uploaded slide to the uploads directory
    var slidePath = Path.Combine(uploadsDirectory, slideName);
    using (var stream = new FileStream(slidePath, FileMode.Create))
    {
        await request.Slide.CopyToAsync(stream);
    }
   
   
    var s = new Video{
        //Select the subject name from an option in the frontend
       SubjectName = request.SubjectName,
       Title = request.Title,
       ClassName = request.ClassName,
       DateAdded = DateTime.Today.Date.ToString("dd MMMM, yyyy"),
       SlidePath = Path.Combine("LMS/Videos", slideName),
      
        AcademicYear = request.AcademicYear,
       AcademicTerm = request.AcademicTerm
    };

    var teacher = context.TeacherInSubjects.FirstOrDefault(a=>a.SubjectName==s.SubjectName&&a.ClassName==s.ClassName&&a.StaffID==ID);
    if (teacher==null){
        return BadRequest("Teacher not found");
    }
    s.StaffID = teacher.StaffID;
    s.TeacherName = teacher.StaffName;



    context.Videos.Add(s);
    await context.SaveChangesAsync();
    await TeacherAuditor(ID, constant.UploadVideo);

    return Ok($"{s.Title} for {s.SubjectName} has been Uploaded successfully");
    
    }

    [HttpGet("ViewAllVideoTeachers")]
    public async Task<IActionResult>ViewAllVideoTeachers(string ID){
        var slide = context.Videos.Where(a=>a.StaffID==ID).ToList();
        await TeacherAuditor(ID,constant.ViewUploadedVideos);
        return Ok(slide);
    }

    [HttpDelete("deleteVideos")]
    public async Task<IActionResult>DeleteVideos(int Id, string SID){
        var slide = context.Videos.FirstOrDefault(a=>a.Id==Id);
        if(slide == null){
            return BadRequest("Video not found");
        }
        context.Videos.Remove(slide);
        await context.SaveChangesAsync();
        try{
            await TeacherAuditor(SID, constant.DeletUploadedVideo);
        }
        catch(Exception ex){
            await AdminAuditor(SID, constant.DeletUploadedVideo);
        }
        
        return Ok("Slides deleted successfully");
    }

           [HttpPost("UploadAudio")]
        public async Task<IActionResult> UploadAudio([FromForm]LMSDto request, string ID){
       
         
         if (request.Slide == null || request.Slide.Length == 0)
    {
        return BadRequest("Invalid Audio");
    }

    // Create the uploads directory if it doesn't exist
    var uploadsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "LMS", "Audios");
    if (!Directory.Exists(uploadsDirectory))
    {
        Directory.CreateDirectory(uploadsDirectory);
    }

    // Get the original slide extension
    var slideExtension = Path.GetExtension(request.Slide.FileName);

    // Generate a unique slide name
    var slideName = Guid.NewGuid().ToString() + slideExtension;

    // Save the uploaded slide to the uploads directory
    var slidePath = Path.Combine(uploadsDirectory, slideName);
    using (var stream = new FileStream(slidePath, FileMode.Create))
    {
        await request.Slide.CopyToAsync(stream);
    }
   
   
    var s = new Audio{
        //Select the subject name from an option in the frontend
       SubjectName = request.SubjectName,
       Title = request.Title,
       ClassName = request.ClassName,
       DateAdded = DateTime.Today.Date.ToString("dd MMMM, yyyy"),
       SlidePath = Path.Combine("LMS/Audios", slideName),
      
        AcademicYear = request.AcademicYear,
       AcademicTerm = request.AcademicTerm
    };

    var teacher = context.TeacherInSubjects.FirstOrDefault(a=>a.SubjectName==s.SubjectName&&a.ClassName==s.ClassName&&a.StaffID==ID);
    if (teacher==null){
        return BadRequest("Teacher not found");
    }
    s.StaffID = teacher.StaffID;
    s.TeacherName = teacher.StaffName;



    context.Audios.Add(s);
    await context.SaveChangesAsync();
    await TeacherAuditor(ID, constant.UploadAudio);

    return Ok($"{s.Title} for {s.SubjectName} has been Uploaded successfully");
    
    }

    [HttpGet("ViewAllAudioTeachers")]
    public async Task<IActionResult>ViewAllAudioTeachers(string ID){
        var slide = context.Audios.Where(a=>a.StaffID==ID).ToList();
        await TeacherAuditor(ID,constant.ViewUploadedAudios);
        return Ok(slide);
    }

    [HttpDelete("deleteAudios")]
    public async Task<IActionResult>DeleteAudios(int Id, string SID){
        var slide = context.Audios.FirstOrDefault(a=>a.Id==Id);
        if(slide == null){
            return BadRequest("Audio not found");
        }
        context.Audios.Remove(slide);
        await context.SaveChangesAsync();
        try{
            await TeacherAuditor(SID, constant.DeletUploadedAudio);
        }
        catch(Exception ex){
            await AdminAuditor(SID, constant.DeletUploadedAudio);
        }
        
        return Ok("Slides deleted successfully");
    }
    


        [HttpPost("UploadPicture")]
        public async Task<IActionResult> UploadPicture([FromForm]LMSDto request, string ID){
       
         
         if (request.Slide == null || request.Slide.Length == 0)
    {
        return BadRequest("Invalid Picture");
    }

    // Create the uploads directory if it doesn't exist
    var uploadsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "LMS", "Pictures");
    if (!Directory.Exists(uploadsDirectory))
    {
        Directory.CreateDirectory(uploadsDirectory);
    }

    // Get the original slide extension
    var slideExtension = Path.GetExtension(request.Slide.FileName);

    // Generate a unique slide name
    var slideName = Guid.NewGuid().ToString() + slideExtension;

    // Save the uploaded slide to the uploads directory
    var slidePath = Path.Combine(uploadsDirectory, slideName);
    using (var stream = new FileStream(slidePath, FileMode.Create))
    {
        await request.Slide.CopyToAsync(stream);
    }
   
   
    var s = new Picture{
        //Select the subject name from an option in the frontend
       SubjectName = request.SubjectName,
       Title = request.Title,
       ClassName = request.ClassName,
       DateAdded = DateTime.Today.Date.ToString("dd MMMM, yyyy"),
       SlidePath = Path.Combine("LMS/Pictures", slideName),
      
        AcademicYear = request.AcademicYear,
       AcademicTerm = request.AcademicTerm
    };

    var teacher = context.TeacherInSubjects.FirstOrDefault(a=>a.SubjectName==s.SubjectName&&a.ClassName==s.ClassName&&a.StaffID==ID);
    if (teacher==null){
        return BadRequest("Teacher not found");
    }
    s.StaffID = teacher.StaffID;
    s.TeacherName = teacher.StaffName;



    context.Pictures.Add(s);
    await context.SaveChangesAsync();
    await TeacherAuditor(ID, constant.UploadPicture);

    return Ok($"{s.Title} for {s.SubjectName} has been Uploaded successfully");
    
    }

    [HttpGet("ViewAllPictureTeachers")]
    public async Task<IActionResult>ViewAllPictureTeachers(string ID){
        var slide = context.Pictures.Where(a=>a.StaffID==ID).ToList();
        await TeacherAuditor(ID,constant.ViewUploadedPictures);
        return Ok(slide);
    }

    [HttpDelete("deletePictures")]
    public async Task<IActionResult>DeletePictures(int Id, string SID){
        var slide = context.Pictures.FirstOrDefault(a=>a.Id==Id);
        if(slide == null){
            return BadRequest("Picture not found");
        }
        context.Pictures.Remove(slide);
        await context.SaveChangesAsync();
        try{
            await TeacherAuditor(SID, constant.DeletUploadedPicture);
        }
        catch(Exception ex){
               await AdminAuditor(SID, constant.DeletUploadedPicture);
        }
     
        return Ok("Slides deleted successfully");
    }


        [HttpPost("UploadBook")]
        public async Task<IActionResult> UploadBook([FromForm]LMSDto request, string ID){
       
         
         if (request.Slide == null || request.Slide.Length == 0)
    {
        return BadRequest("Invalid Book");
    }

    // Create the uploads directory if it doesn't exist
    var uploadsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "LMS", "Books");
    if (!Directory.Exists(uploadsDirectory))
    {
        Directory.CreateDirectory(uploadsDirectory);
    }

    // Get the original slide extension
    var slideExtension = Path.GetExtension(request.Slide.FileName);

    // Generate a unique slide name
    var slideName = Guid.NewGuid().ToString() + slideExtension;

    // Save the uploaded slide to the uploads directory
    var slidePath = Path.Combine(uploadsDirectory, slideName);
    using (var stream = new FileStream(slidePath, FileMode.Create))
    {
        await request.Slide.CopyToAsync(stream);
    }
   
   
    var s = new Book{
        //Select the subject name from an option in the frontend
       SubjectName = request.SubjectName,
       Title = request.Title,
       ClassName = request.ClassName,
       DateAdded = DateTime.Today.Date.ToString("dd MMMM, yyyy"),
       SlidePath = Path.Combine("LMS/Books", slideName),
      
        AcademicYear = request.AcademicYear,
       AcademicTerm = request.AcademicTerm
    };

    var teacher = context.TeacherInSubjects.FirstOrDefault(a=>a.SubjectName==s.SubjectName&&a.ClassName==s.ClassName&&a.StaffID==ID);
    if (teacher==null){
        return BadRequest("Teacher not found");
    }
    s.StaffID = teacher.StaffID;
    s.TeacherName = teacher.StaffName;



    context.Books.Add(s);
    await context.SaveChangesAsync();
    await TeacherAuditor(ID, constant.UploadBook);

    return Ok($"{s.Title} for {s.SubjectName} has been Uploaded successfully");
    
    }

    [HttpGet("ViewAllBookTeachers")]
    public async Task<IActionResult>ViewAllBookTeachers(string ID){
        var slide = context.Books.Where(a=>a.StaffID==ID).ToList();
        await TeacherAuditor(ID,constant.ViewUploadedBooks);
        return Ok(slide);
    }

    [HttpDelete("deleteBooks")]
    public async Task<IActionResult>DeleteBooks(int Id, string SID){
        var slide = context.Books.FirstOrDefault(a=>a.Id==Id);
        if(slide == null){
            return BadRequest("Book not found");
        }
        context.Books.Remove(slide);
        await context.SaveChangesAsync();
        try{
            await TeacherAuditor(SID, constant.DeletUploadedBook);
        }
        catch(Exception ex){
            await AdminAuditor(SID, constant.DeletUploadedBook);
        }
        
        return Ok("Slides deleted successfully");
    }



        [HttpPost("UploadAssignment")]
        public async Task<IActionResult> UploadAssignment([FromForm]LMSDto request, string ID){
       
         
         if (request.Slide == null || request.Slide.Length == 0)
    {
        return BadRequest("Invalid slide");
    }

    // Create the uploads directory if it doesn't exist
    var uploadsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "LMS", "Assignments");
    if (!Directory.Exists(uploadsDirectory))
    {
        Directory.CreateDirectory(uploadsDirectory);
    }

    // Get the original slide extension
    var slideExtension = Path.GetExtension(request.Slide.FileName);

    // Generate a unique slide name
    var slideName = Guid.NewGuid().ToString() + slideExtension;

    // Save the uploaded slide to the uploads directory
    var slidePath = Path.Combine(uploadsDirectory, slideName);
    using (var stream = new FileStream(slidePath, FileMode.Create))
    {
        await request.Slide.CopyToAsync(stream);
    }
   
   
    var s = new Assignment{
        //Select the subject name from an option in the frontend
       SubjectName = request.SubjectName,
       Title = request.Title,
       ClassName = request.ClassName,
       DateAdded = DateTime.Today.Date.ToString("dd MMMM, yyyy"),
       SlidePath = Path.Combine("LMS/Assignments", slideName),
      
       AcademicYear = request.AcademicYear,
       AcademicTerm = request.AcademicTerm,
      
       Deadline = request.ExpireDate,

    };

    var teacher = context.TeacherInSubjects.FirstOrDefault(a=>a.SubjectName==s.SubjectName&&a.ClassName==s.ClassName&&a.StaffID==ID);
    if (teacher==null){
        return BadRequest("Teacher not found");
    }
    s.StaffID = teacher.StaffID;
    s.TeacherName = teacher.StaffName;
    if(s.Deadline<DateTime.Now){
        return BadRequest("The Deadline should be greater than or equal to Today's Date and Time");
    }



    context.Assignments.Add(s);
    await context.SaveChangesAsync();
    await TeacherAuditor(ID, constant.UploadAssignment);

    return Ok($"{s.Title} for {s.SubjectName} has been Uploaded successfully");
    
    }

 

 
 
    [HttpGet("ViewAllAssignmentTeachers")]
    public async Task<IActionResult>ViewAllAssignmentTeachers(string ID){
        var slide = context.Assignments
        .Where(a=>a.StaffID==ID)
        .OrderByDescending(r=>r.Id)
        .ToList();
        await TeacherAuditor(ID,constant.ViewUploadedAssignment);
        return Ok(slide);
    }



    [HttpDelete("deleteAssignment")]
    public async Task<IActionResult>DeleteAssignment(int Id, string SID){
        var slide = context.Assignments.FirstOrDefault(a=>a.Id==Id);
        if(slide == null){
            return BadRequest("Slides not found");
        }
        context.Assignments.Remove(slide);
        await context.SaveChangesAsync();
        try{
        await TeacherAuditor(SID, constant.DeletUploadedAssignment);
        }
        catch(Exception ex){
            await AdminAuditor(SID, constant.DeletUploadedSlides);
        }
        
        return Ok("Slides deleted successfully");
    }





[HttpPost("AddannoucementForStudent")]
public async Task<IActionResult> AddAnnouncementForStudent([FromBody]AnnouncementForStudent request,string SID){

var annoucement = new AnnouncementForStudent{

Subject = request.Subject,
Content = request.Content,
TheId = AssignmentIdGenerator(),

DateAdded = DateTime.Today.Date.ToString("dd MMMM yyyy"),
AcademicTerm = request.AcademicTerm,
AcademicYear = request.AcademicYear

};
context.AnnouncementForStudents.Add(annoucement);
await context.SaveChangesAsync();
try{
await AdminAuditor(SID, constant.AnnouceStudent);
}catch(Exception ex){
await TeacherAuditor(SID, constant.AnnouceStudent);
}



return Ok("Student Annoucement Sent Successfully");
}



[HttpPost("UpdateannoucementForStudent")]
public async Task<IActionResult> UpdateAnnouncementForStudent([FromBody]AnnouncementForStudent request, string TheId, string SID){
    var annoucement = context.AnnouncementForStudents.FirstOrDefault(x=>x.TheId == TheId);
    if (annoucement == null){
        return BadRequest("No Announcement Found");
    }
    annoucement.Subject = request.Subject;
    annoucement.Content = request.Content;
    annoucement.AcademicTerm = request.AcademicTerm;
    annoucement.AcademicYear = request.AcademicYear;

    await context.SaveChangesAsync();

    try{
await AdminAuditor(SID, constant.AnnouceStudentU);
}catch(Exception ex){
await TeacherAuditor(SID, constant.AnnouceStudentU);
}
    return Ok("Student Annoucement Updated Successfully");
}


[HttpDelete("DeleteannoucementForStudent")]
public async Task<IActionResult> DeleteAnnouncementForStudent(string TheId, string SID){
    var annoucement = context.AnnouncementForStudents.FirstOrDefault(x=>x.TheId == TheId);
    if (annoucement == null){
        return BadRequest("No Announcement Found");
    } 

    context.AnnouncementForStudents.Remove(annoucement);

        await context.SaveChangesAsync();
         try{
await AdminAuditor(SID, constant.AnnouceStudentD);
}catch(Exception ex){
await TeacherAuditor(SID, constant.AnnouceStudentD);
}
    return Ok("Student Annoucement Deleted Successfully");

}

[HttpGet("GetannoucementForStudent")]

public async Task<IActionResult> GetAnnouncementForStudent(){
  var annoucement = context.AnnouncementForStudents.OrderByDescending(R=>R.Id).ToList();
  return Ok(annoucement);

}





[HttpPost("AddannoucementForTeacher")]
public async Task<IActionResult> AddAnnouncementForTeachers([FromBody]AnnouncementForTeachers request,string SID){

var annoucement = new AnnouncementForTeachers{

Subject = request.Subject,
Content = request.Content,
TheId = AssignmentIdGenerator(),

DateAdded = DateTime.Today.Date.ToString("dd MMMM yyyy"),
AcademicTerm = request.AcademicTerm,
AcademicYear = request.AcademicYear

};
context.AnnouncementForTeachers.Add(annoucement);
await context.SaveChangesAsync();
try{
await AdminAuditor(SID, constant.AnnouceTeacher);
}catch(Exception ex){
await TeacherAuditor(SID, constant.AnnouceTeacher);
}


return Ok("Teacher Annoucement Sent Successfully");
}


[HttpPost("UpdateannoucementForTeacher")]
public async Task<IActionResult> UpdateAnnouncementForTeacher([FromBody]AnnouncementForTeachers request, string TheId, string SID){
    var annoucement = context.AnnouncementForTeachers.FirstOrDefault(x=>x.TheId == TheId);
    if (annoucement == null){
        return BadRequest("No Announcement Found");
    }
    annoucement.Subject = request.Subject;
    annoucement.Content = request.Content;
    annoucement.AcademicTerm = request.AcademicTerm;
    annoucement.AcademicYear = request.AcademicYear;

    await context.SaveChangesAsync();

    try{
await AdminAuditor(SID, constant.AnnouceTeacherU);
}catch(Exception ex){
await TeacherAuditor(SID, constant.AnnouceTeacherU);
}
    return Ok("Teacher Annoucement Updated Successfully");
}


[HttpDelete("DeleteannoucementForTeacher")]
public async Task<IActionResult> DeleteAnnouncementForTeacher(string TheId, string SID){
    var annoucement = context.AnnouncementForTeachers.FirstOrDefault(x=>x.TheId == TheId);
    if (annoucement == null){
        return BadRequest("No Announcement Found");
    } 

    context.AnnouncementForTeachers.Remove(annoucement);

        await context.SaveChangesAsync();
         try{
await AdminAuditor(SID, constant.AnnouceTeacherD);
}catch(Exception ex){
await TeacherAuditor(SID, constant.AnnouceTeacherD);
}
    return Ok("Teacher Annoucement Deleted Successfully");

}

[HttpGet("GetannoucementForTeacher")]

public async Task<IActionResult> GetAnnouncementForTeacher(){
  var annoucement = context.AnnouncementForTeachers.OrderByDescending(R=>R.Id).ToList();
  return Ok(annoucement);

}




[HttpPost("AddannoucementForPTA")]
public async Task<IActionResult> AddAnnouncementForPTA([FromBody]AnnouncementForPTA request,string SID){

var annoucement = new AnnouncementForPTA{

Subject = request.Subject,
Content = request.Content,
TheId = AssignmentIdGenerator(),

DateAdded = DateTime.Today.Date.ToString("dd MMMM yyyy"),
AcademicTerm = request.AcademicTerm,
AcademicYear = request.AcademicYear

};
context.AnnouncementForPTA.Add(annoucement);
await context.SaveChangesAsync();
try{
await AdminAuditor(SID, constant.AnnoucePTA);
}catch(Exception ex){
await TeacherAuditor(SID, constant.AnnoucePTA);
}



return Ok("PTA Annoucement Sent Successfully");
}



[HttpPost("UpdateannoucementForPTA")]
public async Task<IActionResult> UpdateAnnouncementForPTA([FromBody]AnnouncementForPTA request, string TheId, string SID){
    var annoucement = context.AnnouncementForPTA.FirstOrDefault(x=>x.TheId == TheId);
    if (annoucement == null){
        return BadRequest("No Announcement Found");
    }
    annoucement.Subject = request.Subject;
    annoucement.Content = request.Content;
    annoucement.AcademicTerm = request.AcademicTerm;
    annoucement.AcademicYear = request.AcademicYear;

    await context.SaveChangesAsync();

    try{
await AdminAuditor(SID, constant.AnnoucePTAU);
}catch(Exception ex){
await TeacherAuditor(SID, constant.AnnoucePTAU);
}
    return Ok("PTA Annoucement Updated Successfully");
}


[HttpDelete("DeleteannoucementForPTA")]
public async Task<IActionResult> DeleteAnnouncementForPTA(string TheId, string SID){
    var annoucement = context.AnnouncementForPTA.FirstOrDefault(x=>x.TheId == TheId);
    if (annoucement == null){
        return BadRequest("No Announcement Found");
    } 

    context.AnnouncementForPTA.Remove(annoucement);

        await context.SaveChangesAsync();
         try{
await AdminAuditor(SID, constant.AnnoucePTAD);
}catch(Exception ex){
await TeacherAuditor(SID, constant.AnnoucePTAD);
}
    return Ok("PTA Annoucement Deleted Successfully");

}

[HttpGet("GetannoucementForPTA")]

public async Task<IActionResult> GetAnnouncementForPTA(){
  var annoucement = context.AnnouncementForPTA.OrderByDescending(R=>R.Id).ToList();
  return Ok(annoucement);

}


[HttpPost("AddannoucementForHOD")]
public async Task<IActionResult> AddAnnouncementForHOD([FromBody]AnnoucementForHOD request,string SID){

var annoucement = new AnnoucementForHOD{

Subject = request.Subject,
Content = request.Content,
TheId = AssignmentIdGenerator(),

DateAdded = DateTime.Today.Date.ToString("dd MMMM yyyy"),
AcademicTerm = request.AcademicTerm,
AcademicYear = request.AcademicYear

};
context.AnnoucementForHOD.Add(annoucement);
await context.SaveChangesAsync();
try{
await AdminAuditor(SID, constant.AnnouceHOD);
}catch(Exception ex){
await TeacherAuditor(SID, constant.AnnouceHOD);
}



return Ok("HOD Annoucement Sent Successfully");
}



[HttpPost("UpdateannoucementForHOD")]
public async Task<IActionResult> UpdateAnnouncementForHOD([FromBody]AnnoucementForHOD request, string TheId, string SID){
    var annoucement = context.AnnoucementForHOD.FirstOrDefault(x=>x.TheId == TheId);
    if (annoucement == null){
        return BadRequest("No Announcement Found");
    }
    annoucement.Subject = request.Subject;
    annoucement.Content = request.Content;
    annoucement.AcademicTerm = request.AcademicTerm;
    annoucement.AcademicYear = request.AcademicYear;

    await context.SaveChangesAsync();

    try{
await AdminAuditor(SID, constant.AnnouceHODU);
}catch(Exception ex){
await TeacherAuditor(SID, constant.AnnouceHODU);
}
    return Ok("HOD Annoucement Updated Successfully");
}


[HttpDelete("DeleteannoucementForHOD")]
public async Task<IActionResult> DeleteAnnouncementForHOD(string TheId, string SID){
    var annoucement = context.AnnoucementForHOD.FirstOrDefault(x=>x.TheId == TheId);
    if (annoucement == null){
        return BadRequest("No Announcement Found");
    } 

    context.AnnoucementForHOD.Remove(annoucement);

        await context.SaveChangesAsync();
         try{
await AdminAuditor(SID, constant.AnnouceHODD);
}catch(Exception ex){
await TeacherAuditor(SID, constant.AnnouceHODD);
}
    return Ok("HOD Annoucement Deleted Successfully");

}

[HttpGet("GetannoucementForHOD")]

public async Task<IActionResult> GetAnnouncementForHOD(){
  var annoucement = context.AnnoucementForHOD.OrderByDescending(R=>R.Id).ToList();
  return Ok(annoucement);

}







    [HttpPost("subjectDiscussionTeacher")]
        public async Task<IActionResult> SubjectDiscussionTeacher(string TeacherId, string SubjectN, string ClassN, [FromBody]DiscussionsForTeacher request){
        var checker = SubjectN+TeacherId+ClassN;
        bool NoPower = await context.TeacherInSubjects.AnyAsync(p=>p.StaffID==checker);
         if(!NoPower){
            return BadRequest("You dont have permission to write a comment on this subject");
         }
         
    
    var teacher = context.Teachers.FirstOrDefault(t=> t.StaffID == TeacherId);
    if (teacher == null){
    return Unauthorized();
    }

    var announce = new Discussions{

       Subject = SubjectN,
       Content = request.Content,
       TheId = AssignmentIdGenerator(),
       ClassName = ClassN,
       DateSent = DateTime.Today.Date.ToString("dd MMMM, yyyy"),
       SenderId = teacher.StaffID,
       SenderName = teacher.Title+". "+teacher.FirstName+" "+teacher.OtherName+" " + teacher.LastName,

    };

    context.Discussions.Add(announce);
    await context.SaveChangesAsync();

    return Ok($"{announce.Content} from {announce.SenderName} at {announce.DateSent}");
    
    }


  [HttpPost("subjectDiscussionStudent")]
        public async Task<IActionResult> SubjectDiscussionStuden(string StudentId, string SubjectN, string ClassN, [FromBody]DiscussionsForStudent request){
         bool IsValidStudent = await context.Students.AnyAsync(x => x.StudentId == StudentId&&x.Level==ClassN);
       if (!IsValidStudent){
        return BadRequest("You are not in the specified class");
       }

         
    
    var teacher = context.Students.FirstOrDefault(t=> t.StudentId == StudentId);
    if (teacher == null){
    return Unauthorized();
    }

    var announce = new Discussions{

       Subject = SubjectN,
       Content = request.Content,
      TheId = AssignmentIdGenerator(),
       ClassName = ClassN,
       DateSent = DateTime.Today.Date.ToString("dd MMMM, yyyy"),
       SenderId = teacher.StudentId,
       SenderName = teacher.FirstName+" "+teacher.OtherName+" " + teacher.LastName,

    };

    context.Discussions.Add(announce);
    await context.SaveChangesAsync();

    return Ok($"{announce.Content} from {announce.SenderName} at {announce.DateSent}");
    
    }


[HttpGet("getSubjectDiscussion")]
public async Task<IActionResult> GetSubjectDiscussion(string Subject, string ClassName){
    var discuss = context.Discussions.Where(x => x.Subject == Subject && x.ClassName == ClassName).OrderByDescending(R=>R.Id).ToList();
    if (discuss.Count == 0){
        return BadRequest("No discussion");
    };
    return Ok(discuss);
}







private string AssignmentIdGenerator()
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