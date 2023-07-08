using System;
using System.Collections.Generic;
using System.Linq;
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
            var subject = context.Subjects.ToList();
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
            var classy = context.Classess.ToList();
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







  
  
  
  
    }
}