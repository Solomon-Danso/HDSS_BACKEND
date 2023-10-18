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
            bool classAlreadyExist = await context.Classess.AnyAsync(c=>c.ClassName == request.ClassName);
            if (classAlreadyExist){
                return BadRequest("Class already exists");
            }
            var classy = new Classes{
                ClassName = request.ClassName,
                ClassCode = request.ClassCode,
                Campus = request.Campus,
                ClassTeacher = request.ClassTeacher,
                DateAdded =  DateTime.Today.Date.ToString("dd MMMM, yyyy"),


            };
            context.Classess.Add(classy);
            await context.SaveChangesAsync();
            await AdminAuditor(ID,constant.AddAClass);
            return Ok($"{classy.ClassName} created successfully");


        }

        [HttpPost("UpdateClass")]
        public async Task<IActionResult>UpdateClass([FromBody]Classes request,int Id, string SID){
            var cla = context.Classess.FirstOrDefault(x=>x.Id==Id);
            if (cla==null){
                return BadRequest("Class does not exist");
            }
            cla.ClassName = request.ClassName;
            cla.ClassCode = request.ClassCode;
            cla.Campus = request.Campus;
            cla.ClassTeacher = request.ClassTeacher;
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
        public async Task<IActionResult> AddTeacherToSubject(string ID,[FromBody] TeacherForSubject request){
        
       

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

      


        [HttpGet("ViewTeachersInSubject")]
        public async Task<IActionResult> ViewTeachersInSubject(string SubjectName,string Stage){
            var subject = context.TeacherForSubjects.Where(t=>t.SubjectName == SubjectName && t.ClassName==Stage).OrderByDescending(t => t.Id).ToList();
            return Ok(subject);
        }

         [HttpGet("ViewTeachersRegisteredSubjects")]
        public async Task<IActionResult> ViewTeacherAllSubject(string TeacherId){
            var subject = context.TeacherForSubjects.Where(t=>t.StaffID == TeacherId).OrderByDescending(t => t.Id).ToList();
            return Ok(subject);
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
        public async Task<IActionResult> UploadSlide([FromForm]SlidesDto request, string ID){
       
         
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

    [HttpGet("ViewSlidesStudent")]
    public async Task<IActionResult> ViewSlidesStudent(string StudentId, string SubjectN, string ClassN, string Term, string Year){
       bool IsValidStudent = await context.Students.AnyAsync(x => x.StudentId == StudentId&&x.Level==ClassN);
       if (!IsValidStudent){
        return BadRequest("You are not in the specified class");
       }


         var slide = context.Slides.Where(t=>t.SubjectName == SubjectN && t.ClassName==ClassN && t.AcademicTerm==Term&t.AcademicYear==Year).OrderByDescending(t => t.Id).ToList();
             if (slide.Count == 0) {
                return BadRequest("No slides found ");
                 }
            
            return Ok(slide);
                
    }

    [HttpGet("ViewAllSlidesTeachers")]
    public async Task<IActionResult>ViewAllSlidesTeachers(string ID){
        var slide = context.Slides.Where(a=>a.StaffID==ID).ToList();
        await TeacherAuditor(ID,constant.ViewUploadedSlides);
        return Ok(slide);
    }

    [HttpDelete("deleteSlides")]
    public async Task<IActionResult>DeleteSlides(int Id, string SID){
        var slide = context.Slides.FirstOrDefault(a=>a.Id==Id&&a.StaffID==SID);
        if(slide == null){
            return BadRequest("Slides not found");
        }
        context.Slides.Remove(slide);
        await context.SaveChangesAsync();
        await TeacherAuditor(SID, constant.DeletUploadedSlides);
        return Ok("Slides deleted successfully");
    }


       

            [HttpPost("UploadVideo")]
        public async Task<IActionResult> UploadVideo(string TeacherId, string SubjectN, string ClassN, [FromForm]VideoDto request){
        var checker = SubjectN+TeacherId+ClassN;
        bool NoPower = await context.TeacherForSubjects.AnyAsync(p=>p.TeacherCode==checker);
         if(!NoPower){
            return BadRequest("You dont have permission to upload videos");
         }
         
         if (request.Video == null || request.Video.Length == 0)
    {
        return BadRequest("Invalid videos");
    }

    // Create the uploads directory if it doesn't exist
    var uploadsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "LMS", "Videos");
    if (!Directory.Exists(uploadsDirectory))
    {
        Directory.CreateDirectory(uploadsDirectory);
    }

    // Get the original slide extension
    var videoExtension = Path.GetExtension(request.Video.FileName);

    // Generate a unique slide name
    var videoName = Guid.NewGuid().ToString() + videoExtension;

    // Save the uploaded slide to the uploads directory
    var videoPath = Path.Combine(uploadsDirectory, videoName);
    using (var stream = new FileStream(videoPath, FileMode.Create))
    {
        await request.Video.CopyToAsync(stream);
    }
   
    var teacher = context.Teachers.FirstOrDefault(t=> t.StaffID == TeacherId);
    if (teacher == null){
    return Unauthorized();
    }

    var video = new Video{
        //Select the subject name from an option in the frontend
       SubjectName = SubjectN,
       Title = request.Title,
       ClassName = ClassN,
       DateAdded = DateTime.Today.Date.ToString("dd MMMM, yyyy"),
       VideoPath = Path.Combine("LMS/Videos", videoName),
       TeacherId = teacher.StaffID,
       TeacherName = teacher.Title+". "+teacher.FirstName+" "+teacher.OtherName+" " + teacher.LastName,
        AcademicYear = request.AcademicYear,
       AcademicTerm = request.AcademicTerm

    };

    context.Videos.Add(video);
    await context.SaveChangesAsync();

    return Ok($"{video.Title} for {video.SubjectName} has been Uploaded successfully");
    
    }


[HttpGet("ViewVideoStudent")]
    public async Task<IActionResult> ViewVideoStudent(string StudentId, string SubjectN, string ClassN,string Term, string Year){
      bool IsValidStudent = await context.Students.AnyAsync(x => x.StudentId == StudentId&&x.Level==ClassN);
       if (!IsValidStudent){
        return BadRequest("You are not in the specified class");
       }

         var video = context.Videos.Where(t=>t.SubjectName == SubjectN && t.ClassName==ClassN&&t.AcademicTerm==Term && t.AcademicYear==Year).OrderByDescending(t => t.Id).ToList();
             if (video.Count == 0) {
                return BadRequest("No videos found ");
                 }
            
            return Ok(video);

                
    }


        [HttpGet("ViewVideosTeachers")]
    public async Task<IActionResult> ViewVideosTeachers(string TeacherId, string SubjectN, string ClassN, string Term, string Year){
     var checker = SubjectN+TeacherId+ClassN;
        bool NoPower = await context.TeacherForSubjects.AnyAsync(p=>p.TeacherCode==checker);
         if(!NoPower){
            return BadRequest("You dont have permission to view this slides");
         }

         var video = context.Videos.Where(t=>t.SubjectName == SubjectN && t.ClassName==ClassN&&t.AcademicTerm==Term && t.AcademicYear==Year).OrderByDescending(t => t.Id).ToList();
           if (video.Count == 0) {
                return BadRequest("No videos found ");
                 }
           
            return Ok(video);
                
    }




            [HttpPost("UploadAudio")]
        public async Task<IActionResult> UploadAudio(string TeacherId, string SubjectN, string ClassN, [FromForm]AudioDto request){
        var checker = SubjectN+TeacherId+ClassN;
        bool NoPower = await context.TeacherForSubjects.AnyAsync(p=>p.TeacherCode==checker);
         if(!NoPower){
            return BadRequest("You dont have permission to upload audios");
         }
         
         if (request.Audio == null || request.Audio.Length == 0)
    {
        return BadRequest("Invalid audios");
    }

    // Create the uploads directory if it doesn't exist
    var uploadsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "LMS", "Audios");
    if (!Directory.Exists(uploadsDirectory))
    {
        Directory.CreateDirectory(uploadsDirectory);
    }

    // Get the original slide extension
    var audioExtension = Path.GetExtension(request.Audio.FileName);

    // Generate a unique slide name
    var audioName = Guid.NewGuid().ToString() + audioExtension;

    // Save the uploaded slide to the uploads directory
    var audioPath = Path.Combine(uploadsDirectory, audioName);
    using (var stream = new FileStream(audioPath, FileMode.Create))
    {
        await request.Audio.CopyToAsync(stream);
    }
   
    var teacher = context.Teachers.FirstOrDefault(t=> t.StaffID == TeacherId);
    if (teacher == null){
    return Unauthorized();
    }

    var audio = new Audio{
        //Select the subject name from an option in the frontend
       SubjectName = SubjectN,
       Title = request.Title,
       ClassName = ClassN,
       DateAdded = DateTime.Today.Date.ToString("dd MMMM, yyyy"),
       AudioPath = Path.Combine("LMS/Audios", audioName),
       TeacherId = teacher.StaffID,
       TeacherName = teacher.Title+". "+teacher.FirstName+" "+teacher.OtherName+" " + teacher.LastName,
        AcademicYear = request.AcademicYear,
        AcademicTerm = request.AcademicTerm,
    };

    context.Audios.Add(audio);
    await context.SaveChangesAsync();

    return Ok($"{audio.Title} for {audio.SubjectName} has been Uploaded successfully");
    
    }


[HttpGet("ViewAudioStudent")]
    public async Task<IActionResult> ViewAudioStudent(string StudentId, string SubjectN, string ClassN, string Term, string Year){
      bool IsValidStudent = await context.Students.AnyAsync(x => x.StudentId == StudentId&&x.Level==ClassN);
       if (!IsValidStudent){
        return BadRequest("You are not in the specified class");
       }

         var audio = context.Audios.Where(t=>t.SubjectName == SubjectN && t.ClassName==ClassN&&t.AcademicTerm==Term&&t.AcademicYear==Year).OrderByDescending(t => t.Id).ToList();
             if (audio.Count == 0) {
                return BadRequest("No audios found ");
                 }
            
            return Ok(audio);

                
    }


        [HttpGet("ViewAudiosTeachers")]
    public async Task<IActionResult> ViewAudiosTeachers(string TeacherId, string SubjectN, string ClassN,string Year, string Term){
     var checker = SubjectN+TeacherId+ClassN;
        bool NoPower = await context.TeacherForSubjects.AnyAsync(p=>p.TeacherCode==checker);
         if(!NoPower){
            return BadRequest("You dont have permission to view this slides");
         }

         var audio = context.Audios.Where(t=>t.SubjectName == SubjectN && t.ClassName==ClassN&&t.AcademicTerm==Term && t.AcademicYear==Year).OrderByDescending(t => t.Id).ToList();
           if (audio.Count == 0) {
                return BadRequest("No audios found ");
                 }
           
            return Ok(audio);
                
    }


            [HttpPost("UploadPicture")]
        public async Task<IActionResult> UploadPicture(string TeacherId, string SubjectN, string ClassN, [FromForm]PictureDto request){
        var checker = SubjectN+TeacherId+ClassN;
        bool NoPower = await context.TeacherForSubjects.AnyAsync(p=>p.TeacherCode==checker);
         if(!NoPower){
            return BadRequest("You dont have permission to upload pictures");
         }
         
         if (request.Picture == null || request.Picture.Length == 0)
    {
        return BadRequest("Invalid pictures");
    }

    // Create the uploads directory if it doesn't exist
    var uploadsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "LMS", "Pictures");
    if (!Directory.Exists(uploadsDirectory))
    {
        Directory.CreateDirectory(uploadsDirectory);
    }

    // Get the original slide extension
    var pictureExtension = Path.GetExtension(request.Picture.FileName);

    // Generate a unique slide name
    var pictureName = Guid.NewGuid().ToString() + pictureExtension;

    // Save the uploaded slide to the uploads directory
    var picturePath = Path.Combine(uploadsDirectory, pictureName);
    using (var stream = new FileStream(picturePath, FileMode.Create))
    {
        await request.Picture.CopyToAsync(stream);
    }
   
    var teacher = context.Teachers.FirstOrDefault(t=> t.StaffID == TeacherId);
    if (teacher == null){
    return Unauthorized();
    }

    var picture = new Picture{
        //Select the subject name from an option in the frontend
       SubjectName = SubjectN,
       Title = request.Title,
       ClassName = ClassN,
       DateAdded = DateTime.Today.Date.ToString("dd MMMM, yyyy"),
       PicturePath = Path.Combine("LMS/Pictures", pictureName),
       TeacherId = teacher.StaffID,
       TeacherName = teacher.Title+". "+teacher.FirstName+" "+teacher.OtherName+" " + teacher.LastName,
        AcademicYear = request.AcademicYear,
        AcademicTerm = request.AcademicTerm,
    };

    context.Pictures.Add(picture);
    await context.SaveChangesAsync();

    return Ok($"{picture.Title} for {picture.SubjectName} has been Uploaded successfully");
    
    }


[HttpGet("ViewPictureStudent")]
    public async Task<IActionResult> ViewPictureStudent(string StudentId, string SubjectN, string ClassN, string Year, string Term){
      bool IsValidStudent = await context.Students.AnyAsync(x => x.StudentId == StudentId&&x.Level==ClassN);
       if (!IsValidStudent){
        return BadRequest("You are not in the specified class");
       }

         var picture = context.Pictures.Where(t=>t.SubjectName == SubjectN && t.ClassName==ClassN&&t.AcademicYear==Year&&t.AcademicTerm==Term).OrderByDescending(t => t.Id).ToList();
             if (picture.Count == 0) {
                return BadRequest("No pictures found ");
                 }
            
            return Ok(picture);

                
    }


        [HttpGet("ViewPicturesTeachers")]
    public async Task<IActionResult> ViewPicturesTeachers(string TeacherId, string SubjectN, string ClassN,string Year, string Term){
     var checker = SubjectN+TeacherId+ClassN;
        bool NoPower = await context.TeacherForSubjects.AnyAsync(p=>p.TeacherCode==checker);
         if(!NoPower){
            return BadRequest("You dont have permission to view this slides");
         }

         var picture = context.Pictures.Where(t=>t.SubjectName == SubjectN && t.ClassName==ClassN &&t.AcademicTerm==Term&&t.AcademicYear==Year).OrderByDescending(t => t.Id).ToList();
           if (picture.Count == 0) {
                return BadRequest("No pictures found ");
                 }
           
            return Ok(picture);
                
    }



            [HttpPost("UploadSyllabus")]
        public async Task<IActionResult> UploadSyllabus(string TeacherId, string SubjectN, string ClassN, [FromForm]SyllabusDto request){
        var checker = SubjectN+TeacherId+ClassN;
        bool NoPower = await context.TeacherForSubjects.AnyAsync(p=>p.TeacherCode==checker);
         if(!NoPower){
            return BadRequest("You dont have permission to upload syllabuss");
         }
         
         if (request.Syllabus == null || request.Syllabus.Length == 0)
    {
        return BadRequest("Invalid syllabuss");
    }

    // Create the uploads directory if it doesn't exist
    var uploadsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "LMS", "Syllabuss");
    if (!Directory.Exists(uploadsDirectory))
    {
        Directory.CreateDirectory(uploadsDirectory);
    }

    // Get the original slide extension
    var syllabusExtension = Path.GetExtension(request.Syllabus.FileName);

    // Generate a unique slide name
    var syllabusName = Guid.NewGuid().ToString() + syllabusExtension;

    // Save the uploaded slide to the uploads directory
    var syllabusPath = Path.Combine(uploadsDirectory, syllabusName);
    using (var stream = new FileStream(syllabusPath, FileMode.Create))
    {
        await request.Syllabus.CopyToAsync(stream);
    }
   
    var teacher = context.Teachers.FirstOrDefault(t=> t.StaffID == TeacherId);
    if (teacher == null){
    return Unauthorized();
    }

    var syllabus = new Syllabus{
        //Select the subject name from an option in the frontend
       SubjectName = SubjectN,
       Title = request.Title,
       ClassName = ClassN,
       DateAdded = DateTime.Today.Date.ToString("dd MMMM, yyyy"),
       SyllabusPath = Path.Combine("LMS/Syllabuss", syllabusName),
       TeacherId = teacher.StaffID,
       TeacherName = teacher.Title+". "+teacher.FirstName+" "+teacher.OtherName+" " + teacher.LastName,
        AcademicTerm = request.AcademicTerm,
        AcademicYear = request.AcademicYear
    };

    context.Syllabuss.Add(syllabus);
    await context.SaveChangesAsync();

    return Ok($"{syllabus.Title} for {syllabus.SubjectName} has been Uploaded successfully");
    
    }


[HttpGet("ViewSyllabusStudent")]
    public async Task<IActionResult> ViewSyllabusStudent(string StudentId, string SubjectN, string ClassN,string Term, string Year){
      bool IsValidStudent = await context.Students.AnyAsync(x => x.StudentId == StudentId&&x.Level==ClassN);
       if (!IsValidStudent){
        return BadRequest("You are not in the specified class");
       }

         var syllabus = context.Syllabuss.Where(t=>t.SubjectName == SubjectN && t.ClassName==ClassN&&t.AcademicTerm==Term&&t.AcademicYear==Year).OrderByDescending(t => t.Id).ToList();
             if (syllabus.Count == 0) {
                return BadRequest("No syllabuss found ");
                 }
            
            return Ok(syllabus);

                
    }


        [HttpGet("ViewSyllabussTeachers")]
    public async Task<IActionResult> ViewSyllabussTeachers(string TeacherId, string SubjectN, string ClassN, string Year, string Term){
     var checker = SubjectN+TeacherId+ClassN;
        bool NoPower = await context.TeacherForSubjects.AnyAsync(p=>p.TeacherCode==checker);
         if(!NoPower){
            return BadRequest("You dont have permission to view this slides");
         }

         var syllabus = context.Syllabuss.Where(t=>t.SubjectName == SubjectN && t.ClassName==ClassN&&t.AcademicTerm==Term&&t.AcademicYear==Year).OrderByDescending(t => t.Id).ToList();
           if (syllabus.Count == 0) {
                return BadRequest("No syllabuss found ");
                 }
           
            return Ok(syllabus);
                
    }





            [HttpPost("UploadCalendar")]
        public async Task<IActionResult> UploadCalendar(string TeacherId,  string ClassN, [FromForm]CalendarDto request){
      
        bool NoPower = await context.TeacherForSubjects.AnyAsync(p=>p.StaffID==TeacherId && p.ClassName==ClassN);
         if(!NoPower){
            return BadRequest("You dont have permission to upload calendars");
         }
         
         if (request.Calendar == null || request.Calendar.Length == 0)
    {
        return BadRequest("Invalid calendars");
    }

    // Create the uploads directory if it doesn't exist
    var uploadsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "LMS", "Calendars");
    if (!Directory.Exists(uploadsDirectory))
    {
        Directory.CreateDirectory(uploadsDirectory);
    }

    // Get the original slide extension
    var calendarExtension = Path.GetExtension(request.Calendar.FileName);

    // Generate a unique slide name
    var calendarName = Guid.NewGuid().ToString() + calendarExtension;

    // Save the uploaded slide to the uploads directory
    var calendarPath = Path.Combine(uploadsDirectory, calendarName);
    using (var stream = new FileStream(calendarPath, FileMode.Create))
    {
        await request.Calendar.CopyToAsync(stream);
    }
   
    var teacher = context.Teachers.FirstOrDefault(t=> t.StaffID == TeacherId);
    if (teacher == null){
    return Unauthorized();
    }

    var calendar = new Models.Calendar{
        //Select the subject name from an option in the frontend
       
       Title = request.Title,
       ClassName = ClassN,
       DateAdded = DateTime.Today.Date.ToString("dd MMMM, yyyy"),
       CalendarPath = Path.Combine("LMS/Calendars", calendarName),
       TeacherId = teacher.StaffID,
       TeacherName = teacher.Title+". "+teacher.FirstName+" "+teacher.OtherName+" " + teacher.LastName,
       AcademicTerm = request.AcademicTerm,
       AcademicYear = request.AcademicYear
    };

    context.Calendars.Add(calendar);
    await context.SaveChangesAsync();

    return Ok($"{calendar.Title} for {calendar.ClassName} has been Uploaded successfully");
    
    }


[HttpGet("ViewCalendarStudent")]
    public async Task<IActionResult> ViewCalendarStudent(string StudentId,  string ClassN){
         bool IsValidStudent = await context.Students.AnyAsync(x => x.StudentId == StudentId&&x.Level==ClassN);
       if (!IsValidStudent){
        return BadRequest("You are not in the specified class");
       }
       
         var calendar = context.Calendars.Where(t=> t.ClassName==ClassN).OrderByDescending(t => t.Id).ToList();
             if (calendar.Count == 0) {
                return BadRequest("No calendars found ");
                 }
            
            return Ok(calendar);

                
    }


        [HttpGet("ViewCalendarsTeachers")]
    public async Task<IActionResult> ViewCalendarsTeachers(string TeacherId, string ClassN){
     
        

         var calendar = context.Calendars.Where(t=> t.ClassName==ClassN).OrderByDescending(t => t.Id).ToList();
           if (calendar.Count == 0) {
                return BadRequest("No calendars found ");
                 }
           
            return Ok(calendar);
                
    }

[HttpPost("AddannoucementForStudent")]
public async Task<IActionResult> AddAnnouncementForStudent([FromBody]AnnouncementForStudent request){

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

return Ok("Student Annoucement Sent Successfully");
}

[HttpPut("UpdateannoucementForStudent")]
public async Task<IActionResult> UpdateAnnouncementForStudent([FromBody]AnnouncementForStudent request, string TheId){
    var annoucement = context.AnnouncementForStudents.FirstOrDefault(x=>x.TheId == TheId);
    if (annoucement == null){
        return BadRequest("No Announcement Found");
    }
    annoucement.Subject = request.Subject;
    annoucement.Content = request.Content;

    await context.SaveChangesAsync();
    return Ok("Student Annoucement Updated Successfully");
}


[HttpDelete("DeleteannoucementForStudent")]
public async Task<IActionResult> DeleteAnnouncementForStudent(string TheId){
    var annoucement = context.AnnouncementForStudents.FirstOrDefault(x=>x.TheId == TheId);
    if (annoucement == null){
        return BadRequest("No Announcement Found");
    } 

    context.AnnouncementForStudents.Remove(annoucement);

        await context.SaveChangesAsync();
    return Ok("Student Annoucement Deleted Successfully");

}

[HttpGet("GetannoucementForStudent")]

public async Task<IActionResult> GetAnnouncementForStudent(){
  var annoucement = context.AnnouncementForStudents.OrderByDescending(R=>R.Id).ToList();
  return Ok(annoucement);

}



[HttpPost("AddannoucementForTeachers")]
public async Task<IActionResult> AddAnnouncementForTeachers([FromBody]AnnouncementForTeachers request){

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

return Ok("Student Annoucement Sent Successfully");
}

[HttpPut("UpdateannoucementForTeachers")]
public async Task<IActionResult> UpdateAnnouncementForTeachers([FromBody]AnnouncementForTeachers request, string TheId){
    var annoucement = context.AnnouncementForTeachers.FirstOrDefault(x=>x.TheId == TheId);
    if (annoucement == null){
        return BadRequest("No Announcement Found");
    }
    annoucement.Subject = request.Subject;
    annoucement.Content = request.Content;

    await context.SaveChangesAsync();
    return Ok("Student Annoucement Updated Successfully");
}


[HttpDelete("DeleteannoucementForTeachers")]
public async Task<IActionResult> DeleteAnnouncementForTeachers(string TheId){
    var annoucement = context.AnnouncementForTeachers.FirstOrDefault(x=>x.TheId == TheId);
    if (annoucement == null){
        return BadRequest("No Announcement Found");
    } 

    context.AnnouncementForTeachers.Remove(annoucement);

        await context.SaveChangesAsync();
    return Ok("Student Annoucement Deleted Successfully");

}

[HttpGet("GetannoucementForTeachers")]

public async Task<IActionResult> GetAnnouncementForTeachers(){
  var annoucement = context.AnnouncementForTeachers.OrderByDescending(R=>R.Id).ToList();
  return Ok(annoucement);

}


[HttpPost("AddannoucementForPTA")]
public async Task<IActionResult> AddAnnouncementForPTA([FromBody]AnnouncementForPTA request){

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

return Ok("Student Annoucement Sent Successfully");
}

[HttpPut("UpdateannoucementForPTA")]
public async Task<IActionResult> UpdateAnnouncementForPTA([FromBody]AnnouncementForPTA request, string TheId){
    var annoucement = context.AnnouncementForPTA.FirstOrDefault(x=>x.TheId == TheId);
    if (annoucement == null){
        return BadRequest("No Announcement Found");
    }
    annoucement.Subject = request.Subject;
    annoucement.Content = request.Content;

    await context.SaveChangesAsync();
    return Ok("Student Annoucement Updated Successfully");
}


[HttpDelete("DeleteannoucementForPTA")]
public async Task<IActionResult> DeleteAnnouncementForPTA(string TheId){
    var annoucement = context.AnnouncementForPTA.FirstOrDefault(x=>x.TheId == TheId);
    if (annoucement == null){
        return BadRequest("No Announcement Found");
    } 

    context.AnnouncementForPTA.Remove(annoucement);

        await context.SaveChangesAsync();
    return Ok("Student Annoucement Deleted Successfully");

}

[HttpGet("GetannoucementForPTA")]

public async Task<IActionResult> GetAnnouncementForPTA(){
  var annoucement = context.AnnouncementForPTA.OrderByDescending(R=>R.Id).ToList();
  return Ok(annoucement);

}






[HttpPost("AddannoucementForHOD")]
public async Task<IActionResult> AddAnnoucementForHOD([FromBody]AnnoucementForHOD request){

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

return Ok("Student Annoucement Sent Successfully");
}

[HttpPut("UpdateannoucementForHOD")]
public async Task<IActionResult> UpdateAnnoucementForHOD([FromBody]AnnoucementForHOD request, string TheId){
    var annoucement = context.AnnoucementForHOD.FirstOrDefault(x=>x.TheId == TheId);
    if (annoucement == null){
        return BadRequest("No Announcement Found");
    }
    annoucement.Subject = request.Subject;
    annoucement.Content = request.Content;

    await context.SaveChangesAsync();
    return Ok("Student Annoucement Updated Successfully");
}


[HttpDelete("DeleteannoucementForHOD")]
public async Task<IActionResult> DeleteAnnoucementForHOD(string TheId){
    var annoucement = context.AnnoucementForHOD.FirstOrDefault(x=>x.TheId == TheId);
    if (annoucement == null){
        return BadRequest("No Announcement Found");
    } 

    context.AnnoucementForHOD.Remove(annoucement);

        await context.SaveChangesAsync();
    return Ok("Student Annoucement Deleted Successfully");

}

[HttpGet("GetannoucementForHOD")]

public async Task<IActionResult> GetAnnoucementForHOD(){
  var annoucement = context.AnnoucementForHOD.OrderByDescending(R=>R.Id).ToList();
  return Ok(annoucement);

}




            [HttpPost("anouncementForSubjects")]
        public async Task<IActionResult> AnouncementForSubjects(string TeacherId, string SubjectN, string ClassN, [FromBody]AnnoucementForSubject request){
        var checker = SubjectN+TeacherId+ClassN;
        bool NoPower = await context.TeacherForSubjects.AnyAsync(p=>p.TeacherCode==checker);
         if(!NoPower){
            return BadRequest("You dont have permission to post an announcement");
         }
         
    
    var teacher = context.Teachers.FirstOrDefault(t=> t.StaffID == TeacherId);
    if (teacher == null){
    return Unauthorized();
    }

    var announce = new AnnoucementForSubject{
        //Select the subject name from an option in the frontend
       SubjectName = SubjectN,
       Content = request.Content,
TheId = AssignmentIdGenerator(),
       Title = request.Title,
       ClassName = ClassN,
       DateAdded = DateTime.Today.Date.ToString("dd MMMM, yyyy"),
       TeacherId = teacher.StaffID,
       TeacherName = teacher.Title+". "+teacher.FirstName+" "+teacher.OtherName+" " + teacher.LastName,

    };

    context.AnnoucementForSubjects.Add(announce);
    await context.SaveChangesAsync();

    return Ok($"{announce.Title} for {announce.SubjectName} has been added successfully");
    
    }



    [HttpPost("subjectDiscussionTeacher")]
        public async Task<IActionResult> SubjectDiscussionTeacher(string TeacherId, string SubjectN, string ClassN, [FromBody]DiscussionsForTeacher request){
        var checker = SubjectN+TeacherId+ClassN;
        bool NoPower = await context.TeacherForSubjects.AnyAsync(p=>p.TeacherCode==checker);
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



    [HttpPost("UploadAssignment")]
        public async Task<IActionResult> UploadAssignment(string TeacherId, string SubjectN, string ClassN, [FromForm]AssignmentDto request){
        var checker = SubjectN+TeacherId+ClassN;
        bool NoPower = await context.TeacherForSubjects.AnyAsync(p=>p.TeacherCode==checker);
         if(!NoPower){
            return BadRequest("You dont have permission to upload slides");
         }
         
         if (request.AssignmentFile == null || request.AssignmentFile.Length == 0)
    {
        return BadRequest("Invalid assignment file");
    }

    // Create the uploads directory if it doesn't exist
    var uploadsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "LMS", "AssignmentFiles");
    if (!Directory.Exists(uploadsDirectory))
    {
        Directory.CreateDirectory(uploadsDirectory);
    }

    // Get the original slide extension
    var slideExtension = Path.GetExtension(request.AssignmentFile.FileName);

    // Generate a unique slide name
    var slideName = Guid.NewGuid().ToString() + slideExtension;

    // Save the uploaded slide to the uploads directory
    var slidePath = Path.Combine(uploadsDirectory, slideName);
    using (var stream = new FileStream(slidePath, FileMode.Create))
    {
        await request.AssignmentFile.CopyToAsync(stream);
    }
   
    var teacher = context.Teachers.FirstOrDefault(t=> t.StaffID == TeacherId);
    if (teacher == null){
    return Unauthorized();
    }

    var assignment = new Assignment{
        //Select the subject name from an option in the frontend
       SubjectName = SubjectN,
       StartDate = request.StartDate,
       ExpireDate = request.ExpireDate,
       ClassName = ClassN,
       AssignmentPath = Path.Combine("LMS/AssignmentFiles", slideName),
       TeacherId = teacher.StaffID,
       TeacherName = teacher.Title+". "+teacher.FirstName+" "+teacher.OtherName+" " + teacher.LastName,
       AssignmentNumber = request.AssignmentNumber,
       AcademicYear = request.AcademicYear,
       AcademicTerm = request.AcademicTerm,
       AssignmentToken = SubjectN+ClassN+ request.AcademicYear + request.AcademicTerm + request.AssignmentNumber,
    };
    bool assignmentExist = await context.Assignments.AnyAsync(a=>a.AssignmentToken==assignment.AssignmentToken);
    if(assignmentExist){
        return BadRequest("Assignment already exists");
    }

    context.Assignments.Add(assignment);
    await context.SaveChangesAsync();

    return Ok($"{assignment .SubjectName} assignment has been uploaded succesfully ");
    
    }



    [HttpPost("EditAssignment")]
        public async Task<IActionResult> EditAssignment(string TeacherId, string SubjectN, string ClassN, [FromForm]AssignmentDto request){
        var checker = SubjectN+TeacherId+ClassN;
        bool NoPower = await context.TeacherForSubjects.AnyAsync(p=>p.TeacherCode==checker);
         if(!NoPower){
            return BadRequest("You dont have permission ");
         }
         
         if (request.AssignmentFile == null || request.AssignmentFile.Length == 0)
    {
        return BadRequest("Invalid assignment file");
    }

    // Create the uploads directory if it doesn't exist
    var uploadsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "LMS", "AssignmentFiles");
    if (!Directory.Exists(uploadsDirectory))
    {
        Directory.CreateDirectory(uploadsDirectory);
    }

    // Get the original slide extension
    var slideExtension = Path.GetExtension(request.AssignmentFile.FileName);

    // Generate a unique slide name
    var slideName = Guid.NewGuid().ToString() + slideExtension;

    // Save the uploaded slide to the uploads directory
    var slidePath = Path.Combine(uploadsDirectory, slideName);
    using (var stream = new FileStream(slidePath, FileMode.Create))
    {
        await request.AssignmentFile.CopyToAsync(stream);
    }
   
    var teacher = context.Teachers.FirstOrDefault(t=> t.StaffID == TeacherId);
    if (teacher == null){
    return Unauthorized();
    }
    var Token = SubjectN+ClassN+ request.AcademicYear + request.AcademicTerm + request.AssignmentNumber;

    var oldWork = context.Assignments.FirstOrDefault(a=> a.AssignmentToken == Token);
    if (oldWork == null){
        return BadRequest("Assignment not found");
    }
        //Select the subject name from an option in the frontend
       oldWork.SubjectName = SubjectN;
       oldWork.StartDate = request.StartDate;
       oldWork.ExpireDate = request.ExpireDate;
       oldWork.ClassName = ClassN;
       oldWork.AssignmentPath = Path.Combine("LMS/AssignmentFiles", slideName);
       oldWork.TeacherId = teacher.StaffID;
       oldWork.TeacherName = teacher.Title+". "+teacher.FirstName+" "+teacher.OtherName+" " + teacher.LastName;
       oldWork.AssignmentNumber = request.AssignmentNumber;
       oldWork.AcademicYear = request.AcademicYear;
       oldWork.AcademicTerm = request.AcademicTerm;
       oldWork.AssignmentToken = SubjectN+ClassN+ request.AcademicYear + request.AcademicTerm + request.AssignmentNumber;
    
    
    await context.SaveChangesAsync();

    return Ok($"Assignment has been updated succesfully ");
    
    }


    [HttpPost("UploadAssignmentSolutions")]
        public async Task<IActionResult> UploadAssignmentSolutions(string StudentId, string SubjectN, string ClassN, string Year,string Term,string assignmentNumber, [FromForm]AssignmentSubmissionDto request){
         bool IsValidStudent = await context.Students.AnyAsync(x => x.StudentId == StudentId&&x.Level==ClassN);
       if (!IsValidStudent){
        return BadRequest("You are not in the specified class");
       }

         var assignmentToken = SubjectN+ClassN+ Year + Term + assignmentNumber;
 
         var question = context.Assignments.FirstOrDefault(a=>a.AssignmentToken == assignmentToken);
         if (question == null){
            return BadRequest("No Assignment Found");
         }
         if (DateTime.Now>question.ExpireDate){
            return BadRequest("Assignment has expired");
         }
         if (request.AssignmentFile == null || request.AssignmentFile.Length == 0)
    {
        return BadRequest("Invalid assignment file");
    }

    // Create the uploads directory if it doesn't exist
    var uploadsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "LMS", "SubmissionFiles");
    if (!Directory.Exists(uploadsDirectory))
    {
        Directory.CreateDirectory(uploadsDirectory);
    }

    // Get the original slide extension
    var slideExtension = Path.GetExtension(request.AssignmentFile.FileName);

    // Generate a unique slide name
    var slideName = Guid.NewGuid().ToString() + slideExtension;

    // Save the uploaded slide to the uploads directory
    var slidePath = Path.Combine(uploadsDirectory, slideName);
    using (var stream = new FileStream(slidePath, FileMode.Create))
    {
        await request.AssignmentFile.CopyToAsync(stream);
    }
   
    var student = context.Students.FirstOrDefault(t=> t.StudentId == StudentId);
    if (student == null){
    return Unauthorized();
    }

    var submission = new AssignmentSubmission{
        //Select the subject name from an option in the frontend
       SubjectName = SubjectN,
       AssignmentPath = question.AssignmentPath,
       ClassName = ClassN,
       SubmissionPath = Path.Combine("LMS/SubmissionFiles", slideName),
        uploadDate = DateTime.Today.Date.ToString("dd MMMM, yyyy"),
       StudentId = student.StudentId,
       StudentName = student.FirstName+" "+student.OtherName+" " + student.LastName,
       AssignmentToken = question.AssignmentToken,
       AcademicYear = Year,
       AcademicTerm = Term,
    };
     bool assignmentExist = await context.AssignmentSubmissions.AnyAsync(a=>a.AssignmentToken==submission.AssignmentToken);
    if(assignmentExist){
        return BadRequest("You have already submitted a solution for this assignment");
    }
    else{
        context.AssignmentSubmissions.Add(submission);
    } 
    await context.SaveChangesAsync();

    return Ok($"{submission.SubjectName} solution has been submitted succesfully ");
    
    }


[HttpGet("ViewAllStudentAssignmentsTeacher")]
        public async Task<IActionResult> ViewAllStudentAssignmentsTeacher(string TeacherId, string SubjectN, string ClassN){
        var checker = SubjectN+TeacherId+ClassN;
        bool NoPower = await context.TeacherForSubjects.AnyAsync(p=>p.TeacherCode==checker);
         if(!NoPower){
            return BadRequest("You dont have permission to view this ");
         }

         var assignment = context.Assignments.Where(a=>a.SubjectName==SubjectN && a.ClassName==ClassN).OrderByDescending(R=>R.Id).ToList();
         if(assignment.Count==0){
            return BadRequest("No Assignment Found");
         }
         return Ok(assignment);
}

[HttpGet("ViewAllStudentAssignmentsStudent")]
        public async Task<IActionResult> ViewAllStudentAssignmentsStudent(string StudentId, string SubjectN, string ClassN){
        bool IsValidStudent = await context.Students.AnyAsync(x => x.StudentId == StudentId&&x.Level==ClassN);
       if (!IsValidStudent){
        return BadRequest("You are not in the specified class");
       }


         var assignment = context.Assignments.Where(a=>a.SubjectName==SubjectN && a.ClassName==ClassN).OrderByDescending(R=>R.Id).ToList();
          if(assignment.Count==0){
            return BadRequest("No Assignment Found");
         }
         return Ok(assignment);
}



[HttpGet("ViewAllStudentSolutionsTeacher")]
        public async Task<IActionResult> ViewAllStudentSolutionsTeacher(string TeacherId, string SubjectN, string ClassN){
        var checker = SubjectN+TeacherId+ClassN;
        bool NoPower = await context.TeacherForSubjects.AnyAsync(p=>p.TeacherCode==checker);
         if(!NoPower){
            return BadRequest("You dont have permission to view this ");
         }

         var Solutions = context.AssignmentSubmissions.Where(a=>a.SubjectName==SubjectN && a.ClassName==ClassN).OrderByDescending(R=>R.Id).ToList();
          if(Solutions.Count==0){
            return BadRequest("No Solutions Found");
         }
         return Ok(Solutions);
}


[HttpGet("ViewAllStudentSolutionsStudent")]
        public async Task<IActionResult> ViewAllStudentSolutionsStudent(string StudentId, string SubjectN, string ClassN){
        bool IsValidStudent = await context.Students.AnyAsync(x => x.StudentId == StudentId&&x.Level==ClassN);
       if (!IsValidStudent){
        return BadRequest("You are not in the specified class");
       }


         var submissions = context.AssignmentSubmissions.Where(a=>a.SubjectName==SubjectN && a.ClassName==ClassN&&a.StudentId==StudentId).OrderByDescending(R=>R.Id).ToList();
          if(submissions.Count==0){
            return BadRequest("No Submission Found");
         }
         return Ok(submissions);
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