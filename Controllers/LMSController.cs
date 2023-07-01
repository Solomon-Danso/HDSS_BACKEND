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
        
        [HttpGet("ViewTeachersInSubject")]
        public async Task<IActionResult> ViewTeachersInSubject(string SubjectName,string Stage){
            var subject = context.TeacherForSubjects.Where(t=>t.SubjectName == SubjectName && t.ClassName==Stage).ToList();
            return Ok(subject);
        }
  
  
  
  
  
  
    }
}