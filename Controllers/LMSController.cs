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
    [Route("api/[controller]")]
    public class LMSController : ControllerBase
    {
        private readonly DataContext context;
        public LMSController(DataContext ctx){
            context = ctx;
        }

        [HttpPost("AddSubject")]
        public async Task<IActionResult> AddSubject([FromBody]Subject request,string Stage ){
            var classy = context.Classess.FirstOrDefault(c => c.ClassName == Stage);
            if (classy == null){
                return BadRequest("Enter A Valid Class Name");
            }
            bool subjectAlreadyExists = await context.Subjects.AnyAsync(s=>s.SubjectName == request.SubjectName);
            if (subjectAlreadyExists){
                return BadRequest("Subject already exists");
            }

            var subject = new Subject{
            SubjectName = request.SubjectName,
            ClassName = classy.ClassName,
            DateAdded =  DateTime.Today.Date.ToString("dd MMMM, yyyy"),
            };

            context.Subjects.Add(subject);
            await context.SaveChangesAsync();
            return Ok($"{subject.SubjectName} created successfully");
        }

        [HttpGet("viewAllSubject")]
        public async Task<IActionResult> ViewAllSubjects(){
            var subject = context.Subjects.OrderByDescending(R=>R.Id).ToList();
            return Ok(subject);
        }




        [HttpPost("AddClass")]
        public async Task<IActionResult>AddClass([FromBody]Classes request){
            bool classAlreadyExist = await context.Classess.AnyAsync(c=>c.ClassName == request.ClassName);
            if (classAlreadyExist){
                return BadRequest("Class already exists");
            }
            var classy = new Classes{
                ClassName = request.ClassName,
                 DateAdded =  DateTime.Today.Date.ToString("dd MMMM, yyyy"),
            };
            context.Classess.Add(classy);
            await context.SaveChangesAsync();
            return Ok($"{classy.ClassName} created successfully");


        }
 
          [HttpGet("viewAllClasses")]
        public async Task<IActionResult> ViewAllClasses(){
            var classy = context.Classess.OrderByDescending(R=>R.Id).ToList();
            return Ok(classy);
        }



        [HttpPost("AddTeacherToSubject")]
        public async Task<IActionResult> AddTeacherToSubject(string subjects, string StaffID,string Stage,[FromBody] TeacherForSubject request){
         var subject = context.Subjects.FirstOrDefault(s => s.SubjectName == subjects);
         var teacher = context.Teachers.FirstOrDefault(t=> t.StaffID == StaffID);
         if (subject == null||teacher == null){
            return BadRequest("Subject or Teacher not found");
         }
          var classy = context.Classess.FirstOrDefault(c => c.ClassName == Stage);
            if (classy == null){
                return BadRequest("Enter A Valid Class Name");
            }

         var code =  subject.SubjectName+teacher.StaffID+classy.ClassName;
         bool teacherAlreadyExist =await context.TeacherForSubjects.AnyAsync(t => t.TeacherCode == code);
         if(teacherAlreadyExist) {
            return BadRequest("Teacher Already Exist");
         }


         var teacherforsub = new TeacherForSubject{
            StaffID = teacher.StaffID,
            StaffName = teacher.Title+". "+teacher.FirstName+" "+teacher.OtherName+" " + teacher.LastName,
            SubjectName = subject.SubjectName,
            ClassName = classy.ClassName,
            TeacherCode = code

         };

         context.TeacherForSubjects.Add(teacherforsub);
         await context.SaveChangesAsync();

         return Ok($"{teacherforsub.StaffName} has been assigned to {teacherforsub.SubjectName}");

        }
 
        [HttpPost("AddStudentToSubject")]
        public async Task<IActionResult> AddStudentToSubject(string subjects, string StudentID,string Stage,[FromBody]StudentForSubject request){

          var subject = context.Subjects.FirstOrDefault(s => s.SubjectName == subjects);
         var student = context.Students.FirstOrDefault(t=> t.StudentId == StudentID);
         if (subject == null||student == null){
            return BadRequest("Subject or Student not found");
         }
          var classy = context.Classess.FirstOrDefault(c => c.ClassName == Stage);
            if (classy == null){
                return BadRequest("Enter A Valid Class Name");
            }

         var code =  subject.SubjectName+student.StudentId+classy.ClassName;
         bool studentAlreadyExist =await context.StudentForSubjects.AnyAsync(t => t.StudentCode == code);
         if(studentAlreadyExist) {
            return BadRequest("Student Already Registered");
         }


         var studentforsub = new StudentForSubject{
            StudentID  = student.StudentId ,
            StudentName = student.Title+". "+student.FirstName+" "+student.OtherName+" " + student.LastName,
            SubjectName = subject.SubjectName,
            ClassName = classy.ClassName,
            StudentCode = code

         };

         context.StudentForSubjects.Add(studentforsub);
         await context.SaveChangesAsync();

         return Ok($"{studentforsub.StudentName} has been assigned to {studentforsub.SubjectName}");

 
        }

  
        
        [HttpDelete("removeTeacherFromSubject")]
        public async Task<IActionResult> RemoveTeacherFromSubject(string subjects, string StaffID,string Stage){
            
            var subject = context.Subjects.FirstOrDefault(s => s.SubjectName == subjects);
            var teacher = context.Teachers.FirstOrDefault(t=> t.StaffID == StaffID);
            if (subject == null||teacher == null){
            return BadRequest("Subject or Teacher not found");
         }  
           var classy = context.Classess.FirstOrDefault(c => c.ClassName == Stage);
            if (classy == null){
                return BadRequest("Enter A Valid Class Name");
            }

         var code =  subject.SubjectName+teacher.StaffID+classy.ClassName;
            var teachercode = context.TeacherForSubjects.FirstOrDefault(t => t.TeacherCode == code);
            if (teachercode == null){
                return BadRequest("Teacher not found");
            }

            context.TeacherForSubjects.Remove(teachercode);
            await context.SaveChangesAsync();
            return Ok("Teacher has been removed");

        }
        

        [HttpDelete("removeStudentFromSubject")]
        public async Task<IActionResult> RemoveStudentFromSubject(string subjects, string StaffID,string Stage){
            
            var subject = context.Subjects.FirstOrDefault(s => s.SubjectName == subjects);
            var student = context.Students.FirstOrDefault(t=> t.StudentId == StaffID);
            if (subject == null||student == null){
            return BadRequest("Subject or Student not found");
         }  
           var classy = context.Classess.FirstOrDefault(c => c.ClassName == Stage);
            if (classy == null){
                return BadRequest("Enter A Valid Class Name");
            }

         var code =  subject.SubjectName+student.StudentId+classy.ClassName;
            var studentcode = context.StudentForSubjects.FirstOrDefault(t => t.StudentCode == code);
            if (studentcode == null){
                return BadRequest("Student not found");
            }

            context.StudentForSubjects.Remove(studentcode);
            await context.SaveChangesAsync();
            return Ok("Student has been removed");

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
  

        [HttpGet("ViewStudentsInSubject")]
        public async Task<IActionResult> ViewStudentsInSubject(string SubjectName,string Stage){
            var subject = context.StudentForSubjects.Where(t=>t.SubjectName == SubjectName && t.ClassName==Stage).OrderByDescending(t => t.Id).ToList();
            return Ok(subject);
        }

        [HttpGet("ViewStudentRegisteredSubjects")]
        public async Task<IActionResult> ViewStudentsAllSubject(string StudentId){
            var subject = context.StudentForSubjects.Where(t=>t.StudentID == StudentId).OrderByDescending(t => t.Id).ToList();
            return Ok(subject);
        }
  


        [HttpPost("UploadSlide")]
        public async Task<IActionResult> UploadSlide(string TeacherId, string SubjectN, string ClassN, [FromForm]SlidesDto request){
        var checker = SubjectN+TeacherId+ClassN;
        bool NoPower = await context.TeacherForSubjects.AnyAsync(p=>p.TeacherCode==checker);
         if(!NoPower){
            return BadRequest("You dont have permission to upload slides");
         }
         
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
   
    var teacher = context.Teachers.FirstOrDefault(t=> t.StaffID == TeacherId);
    if (teacher == null){
    return Unauthorized();
    }

    var slides = new Slide{
        //Select the subject name from an option in the frontend
       SubjectName = SubjectN,
       Title = request.Title,
       ClassName = ClassN,
       DateAdded = DateTime.Today.Date.ToString("dd MMMM, yyyy"),
       SlidePath = Path.Combine("LMS/Slides", slideName),
       TeacherId = teacher.StaffID,
       TeacherName = teacher.Title+". "+teacher.FirstName+" "+teacher.OtherName+" " + teacher.LastName,

    };

    context.Slides.Add(slides);
    await context.SaveChangesAsync();

    return Ok($"{slides.Title} for {slides.SubjectName} has been Uploaded successfully");
    
    }

    [HttpGet("ViewSlidesStudent")]
    public async Task<IActionResult> ViewSlidesStudent(string StudentId, string SubjectN, string ClassN){
     var checker = SubjectN+StudentId+ClassN;
        bool NoPower = await context.StudentForSubjects.AnyAsync(p=>p.StudentCode==checker);
         if(!NoPower){
            return BadRequest("You dont have permission to view this slides");
         }

         var slide = context.Slides.Where(t=>t.SubjectName == SubjectN && t.ClassName==ClassN).OrderByDescending(t => t.Id).ToList();
             if (slide.Count == 0) {
                return BadRequest("No slides found ");
                 }
            
            return Ok(slide);

                
    }

        [HttpGet("ViewSlidesTeachers")]
    public async Task<IActionResult> ViewSlidesTeachers(string TeacherId, string SubjectN, string ClassN){
     var checker = SubjectN+TeacherId+ClassN;
        bool NoPower = await context.TeacherForSubjects.AnyAsync(p=>p.TeacherCode==checker);
         if(!NoPower){
            return BadRequest("You dont have permission to view this slides");
         }

         var slide = context.Slides.Where(t=>t.SubjectName == SubjectN && t.ClassName==ClassN).OrderByDescending(t => t.Id).ToList();
           if (slide.Count == 0) {
                return BadRequest("No slides found ");
                 }
           
            return Ok(slide);
                
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

    };

    context.Videos.Add(video);
    await context.SaveChangesAsync();

    return Ok($"{video.Title} for {video.SubjectName} has been Uploaded successfully");
    
    }


[HttpGet("ViewVideoStudent")]
    public async Task<IActionResult> ViewVideoStudent(string StudentId, string SubjectN, string ClassN){
     var checker = SubjectN+StudentId+ClassN;
        bool NoPower = await context.StudentForSubjects.AnyAsync(p=>p.StudentCode==checker);
         if(!NoPower){
            return BadRequest("You dont have permission to view this videos");
         }

         var video = context.Videos.Where(t=>t.SubjectName == SubjectN && t.ClassName==ClassN).OrderByDescending(t => t.Id).ToList();
             if (video.Count == 0) {
                return BadRequest("No videos found ");
                 }
            
            return Ok(video);

                
    }


        [HttpGet("ViewVideosTeachers")]
    public async Task<IActionResult> ViewVideosTeachers(string TeacherId, string SubjectN, string ClassN){
     var checker = SubjectN+TeacherId+ClassN;
        bool NoPower = await context.TeacherForSubjects.AnyAsync(p=>p.TeacherCode==checker);
         if(!NoPower){
            return BadRequest("You dont have permission to view this slides");
         }

         var video = context.Videos.Where(t=>t.SubjectName == SubjectN && t.ClassName==ClassN).OrderByDescending(t => t.Id).ToList();
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

    };

    context.Audios.Add(audio);
    await context.SaveChangesAsync();

    return Ok($"{audio.Title} for {audio.SubjectName} has been Uploaded successfully");
    
    }


[HttpGet("ViewAudioStudent")]
    public async Task<IActionResult> ViewAudioStudent(string StudentId, string SubjectN, string ClassN){
     var checker = SubjectN+StudentId+ClassN;
        bool NoPower = await context.StudentForSubjects.AnyAsync(p=>p.StudentCode==checker);
         if(!NoPower){
            return BadRequest("You dont have permission to view this audios");
         }

         var audio = context.Audios.Where(t=>t.SubjectName == SubjectN && t.ClassName==ClassN).OrderByDescending(t => t.Id).ToList();
             if (audio.Count == 0) {
                return BadRequest("No audios found ");
                 }
            
            return Ok(audio);

                
    }


        [HttpGet("ViewAudiosTeachers")]
    public async Task<IActionResult> ViewAudiosTeachers(string TeacherId, string SubjectN, string ClassN){
     var checker = SubjectN+TeacherId+ClassN;
        bool NoPower = await context.TeacherForSubjects.AnyAsync(p=>p.TeacherCode==checker);
         if(!NoPower){
            return BadRequest("You dont have permission to view this slides");
         }

         var audio = context.Audios.Where(t=>t.SubjectName == SubjectN && t.ClassName==ClassN).OrderByDescending(t => t.Id).ToList();
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

    };

    context.Pictures.Add(picture);
    await context.SaveChangesAsync();

    return Ok($"{picture.Title} for {picture.SubjectName} has been Uploaded successfully");
    
    }


[HttpGet("ViewPictureStudent")]
    public async Task<IActionResult> ViewPictureStudent(string StudentId, string SubjectN, string ClassN){
     var checker = SubjectN+StudentId+ClassN;
        bool NoPower = await context.StudentForSubjects.AnyAsync(p=>p.StudentCode==checker);
         if(!NoPower){
            return BadRequest("You dont have permission to view this pictures");
         }

         var picture = context.Pictures.Where(t=>t.SubjectName == SubjectN && t.ClassName==ClassN).OrderByDescending(t => t.Id).ToList();
             if (picture.Count == 0) {
                return BadRequest("No pictures found ");
                 }
            
            return Ok(picture);

                
    }


        [HttpGet("ViewPicturesTeachers")]
    public async Task<IActionResult> ViewPicturesTeachers(string TeacherId, string SubjectN, string ClassN){
     var checker = SubjectN+TeacherId+ClassN;
        bool NoPower = await context.TeacherForSubjects.AnyAsync(p=>p.TeacherCode==checker);
         if(!NoPower){
            return BadRequest("You dont have permission to view this slides");
         }

         var picture = context.Pictures.Where(t=>t.SubjectName == SubjectN && t.ClassName==ClassN).OrderByDescending(t => t.Id).ToList();
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

    };

    context.Syllabuss.Add(syllabus);
    await context.SaveChangesAsync();

    return Ok($"{syllabus.Title} for {syllabus.SubjectName} has been Uploaded successfully");
    
    }


[HttpGet("ViewSyllabusStudent")]
    public async Task<IActionResult> ViewSyllabusStudent(string StudentId, string SubjectN, string ClassN){
     var checker = SubjectN+StudentId+ClassN;
        bool NoPower = await context.StudentForSubjects.AnyAsync(p=>p.StudentCode==checker);
         if(!NoPower){
            return BadRequest("You dont have permission to view this syllabuss");
         }

         var syllabus = context.Syllabuss.Where(t=>t.SubjectName == SubjectN && t.ClassName==ClassN).OrderByDescending(t => t.Id).ToList();
             if (syllabus.Count == 0) {
                return BadRequest("No syllabuss found ");
                 }
            
            return Ok(syllabus);

                
    }


        [HttpGet("ViewSyllabussTeachers")]
    public async Task<IActionResult> ViewSyllabussTeachers(string TeacherId, string SubjectN, string ClassN){
     var checker = SubjectN+TeacherId+ClassN;
        bool NoPower = await context.TeacherForSubjects.AnyAsync(p=>p.TeacherCode==checker);
         if(!NoPower){
            return BadRequest("You dont have permission to view this slides");
         }

         var syllabus = context.Syllabuss.Where(t=>t.SubjectName == SubjectN && t.ClassName==ClassN).OrderByDescending(t => t.Id).ToList();
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

    var calendar = new Calendar{
        //Select the subject name from an option in the frontend
       
       Title = request.Title,
       ClassName = ClassN,
       DateAdded = DateTime.Today.Date.ToString("dd MMMM, yyyy"),
       CalendarPath = Path.Combine("LMS/Calendars", calendarName),
       TeacherId = teacher.StaffID,
       TeacherName = teacher.Title+". "+teacher.FirstName+" "+teacher.OtherName+" " + teacher.LastName,

    };

    context.Calendars.Add(calendar);
    await context.SaveChangesAsync();

    return Ok($"{calendar.Title} for {calendar.ClassName} has been Uploaded successfully");
    
    }


[HttpGet("ViewCalendarStudent")]
    public async Task<IActionResult> ViewCalendarStudent(string StudentId,  string ClassN){
  
        bool NoPower = await context.StudentForSubjects.AnyAsync(p=>p.StudentID==StudentId && p.ClassName==ClassN);
         if(!NoPower){
            return BadRequest("You dont have permission to view this calendars");
         }

         var calendar = context.Calendars.Where(t=> t.ClassName==ClassN).OrderByDescending(t => t.Id).ToList();
             if (calendar.Count == 0) {
                return BadRequest("No calendars found ");
                 }
            
            return Ok(calendar);

                
    }


        [HttpGet("ViewCalendarsTeachers")]
    public async Task<IActionResult> ViewCalendarsTeachers(string TeacherId, string ClassN){
     
        bool NoPower = await context.TeacherForSubjects.AnyAsync(p=>p.StaffID==TeacherId && p.ClassName==ClassN);
         if(!NoPower){
            return BadRequest("You dont have permission to view this slides");
         }

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
DateAdded = DateTime.Today.Date.ToString("dd MMMM yyyy"),

};
context.AnnouncementForStudents.Add(annoucement);
await context.SaveChangesAsync();

return Ok("Student Annoucement Sent Successfully");
}

[HttpPut("UpdateannoucementForStudent")]
public async Task<IActionResult> UpdateAnnouncementForStudent([FromBody]AnnouncementForStudent request, string DateAdded){
    var annoucement = context.AnnouncementForStudents.FirstOrDefault(x=>x.DateAdded == DateAdded);
    if (annoucement == null){
        return BadRequest("No Announcement Found");
    }
    annoucement.Subject = request.Subject;
    annoucement.Content = request.Content;

    await context.SaveChangesAsync();
    return Ok("Student Annoucement Updated Successfully");
}


[HttpDelete("DeleteannoucementForStudent")]
public async Task<IActionResult> DeleteAnnouncementForStudent(string DateAdded){
    var annoucement = context.AnnouncementForStudents.FirstOrDefault(x=>x.DateAdded == DateAdded);
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
DateAdded = DateTime.Today.Date.ToString("dd MMMM yyyy"),

};
context.AnnouncementForTeachers.Add(annoucement);
await context.SaveChangesAsync();

return Ok("Student Annoucement Sent Successfully");
}

[HttpPut("UpdateannoucementForTeachers")]
public async Task<IActionResult> UpdateAnnouncementForTeachers([FromBody]AnnouncementForTeachers request, string DateAdded){
    var annoucement = context.AnnouncementForTeachers.FirstOrDefault(x=>x.DateAdded == DateAdded);
    if (annoucement == null){
        return BadRequest("No Announcement Found");
    }
    annoucement.Subject = request.Subject;
    annoucement.Content = request.Content;

    await context.SaveChangesAsync();
    return Ok("Student Annoucement Updated Successfully");
}


[HttpDelete("DeleteannoucementForTeachers")]
public async Task<IActionResult> DeleteAnnouncementForTeachers(string DateAdded){
    var annoucement = context.AnnouncementForTeachers.FirstOrDefault(x=>x.DateAdded == DateAdded);
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
DateAdded = DateTime.Today.Date.ToString("dd MMMM yyyy"),

};
context.AnnouncementForPTA.Add(annoucement);
await context.SaveChangesAsync();

return Ok("Student Annoucement Sent Successfully");
}

[HttpPut("UpdateannoucementForPTA")]
public async Task<IActionResult> UpdateAnnouncementForPTA([FromBody]AnnouncementForPTA request, string DateAdded){
    var annoucement = context.AnnouncementForPTA.FirstOrDefault(x=>x.DateAdded == DateAdded);
    if (annoucement == null){
        return BadRequest("No Announcement Found");
    }
    annoucement.Subject = request.Subject;
    annoucement.Content = request.Content;

    await context.SaveChangesAsync();
    return Ok("Student Annoucement Updated Successfully");
}


[HttpDelete("DeleteannoucementForPTA")]
public async Task<IActionResult> DeleteAnnouncementForPTA(string DateAdded){
    var annoucement = context.AnnouncementForPTA.FirstOrDefault(x=>x.DateAdded == DateAdded);
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
DateAdded = DateTime.Today.Date.ToString("dd MMMM yyyy"),

};
context.AnnoucementForHOD.Add(annoucement);
await context.SaveChangesAsync();

return Ok("Student Annoucement Sent Successfully");
}

[HttpPut("UpdateannoucementForHOD")]
public async Task<IActionResult> UpdateAnnoucementForHOD([FromBody]AnnoucementForHOD request, string DateAdded){
    var annoucement = context.AnnoucementForHOD.FirstOrDefault(x=>x.DateAdded == DateAdded);
    if (annoucement == null){
        return BadRequest("No Announcement Found");
    }
    annoucement.Subject = request.Subject;
    annoucement.Content = request.Content;

    await context.SaveChangesAsync();
    return Ok("Student Annoucement Updated Successfully");
}


[HttpDelete("DeleteannoucementForHOD")]
public async Task<IActionResult> DeleteAnnoucementForHOD(string DateAdded){
    var annoucement = context.AnnoucementForHOD.FirstOrDefault(x=>x.DateAdded == DateAdded);
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
        var checker = SubjectN+StudentId+ClassN;
        bool NoPower = await context.StudentForSubjects.AnyAsync(p=>p.StudentCode==checker);
         if(!NoPower){
            return BadRequest("You dont have permission to write a comment on this subject");
         }
         
    
    var teacher = context.Students.FirstOrDefault(t=> t.StudentId == StudentId);
    if (teacher == null){
    return Unauthorized();
    }

    var announce = new Discussions{

       Subject = SubjectN,
       Content = request.Content,
       ClassName = ClassN,
       DateSent = DateTime.Today.Date.ToString("dd MMMM, yyyy"),
       SenderId = teacher.StudentId,
       SenderName = teacher.Title+". "+teacher.FirstName+" "+teacher.OtherName+" " + teacher.LastName,

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
       AssignmentCode = AssignmentIdGenerator()
    };

    context.Assignments.Add(assignment);
    await context.SaveChangesAsync();

    return Ok($"{assignment .SubjectName} assignment has been uploaded succesfully ");
    
    }







    [HttpPost("UploadAssignmentSolutions")]
        public async Task<IActionResult> UploadAssignmentSolutions(string StudentId, string SubjectN, string ClassN,string Code, [FromForm]AssignmentSubmissionDto request){
        var checker = SubjectN+StudentId+ClassN;
        bool NoPower = await context.StudentForSubjects.AnyAsync(p=>p.StudentCode==checker);
         if(!NoPower){
            return BadRequest("You dont have permission to view this assignment");
         }
         var question = context.Assignments.FirstOrDefault(a=>a.AssignmentCode == Code);
         if (question == null||question.ClassName!=ClassN||question.SubjectName!=SubjectN){
            return BadRequest("No Assignment Found");
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
       StudentName = student.Title+". "+student.FirstName+" "+student.OtherName+" " + student.LastName,
       AssignmentCode = question.AssignmentCode
    };

    context.AssignmentSubmissions.Add(submission);
    await context.SaveChangesAsync();

    return Ok($"{submission.SubjectName} solution has been sent succesfully ");
    
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
        var checker = SubjectN+StudentId+ClassN;
        bool NoPower = await context.StudentForSubjects.AnyAsync(p=>p.StudentCode==checker);
         if(!NoPower){
            return BadRequest("You dont have permission to view this ");
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
        var checker = SubjectN+StudentId+ClassN;
        bool NoPower = await context.StudentForSubjects.AnyAsync(p=>p.StudentCode==checker);
         if(!NoPower){
            return BadRequest("You dont have permission to view this ");
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
  
  
  
  
    }
}