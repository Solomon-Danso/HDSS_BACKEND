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
    [Route("api/LMS")]
    public class LMSController : ControllerBase
    {
        private readonly DataContext context;
        public LMSController(DataContext ctx){
            context = ctx;
        }

        [HttpPost("AddSubject")]
        public async Task<IActionResult> AddSubject([FromBody]Subject request){
            var subject = new Subject{
              Id = request.Id,
              SubjectId = request.SubjectId,  
            };

            context.Subjects.Add(subject);
            await context.SaveChangesAsync();
            return Ok($"{subject.SubjectId} created successfully");
        }

        [HttpPost("AddStudentToSubject")]
        public async Task<IActionResult> AddStudentToSubject(string subjectId, string studentId,[FromBody]RegisteredStudent request){
        var subject =  context.Subjects.FirstOrDefault(s => s.SubjectId == subjectId);
        var student = context.Students.FirstOrDefault(s => s.StudentId == studentId);
        
        if(subject == null||student == null){
            return BadRequest("Student or Subject not found");
        }

        var registered = new RegisteredStudent{
            StudentId = student.StudentId,
            StudentName = student.Title + " " + student.FirstName + " " + student.OtherName + " " +student.LastName,
            DateAdded =  DateTime.Today.Date.ToString("dd MMMM, yyyy"),
        };

        subject.Students.Add(registered);
        await context.SaveChangesAsync();
        return Ok($"{registered.StudentName} has been added as a registered student of {subject.SubjectId}");    

        }

        [HttpPost("AddTeacherToSubject")]
        public async Task<IActionResult> AddTeacherToSubject(string subjectId, string teacherId,[FromBody]RegisteredTeacher request){
        var subject =  context.Subjects.FirstOrDefault(s => s.SubjectId == subjectId);
        var teacher = context.Teachers.FirstOrDefault(s => s.StaffID == teacherId);
        
        if(subject == null||teacher == null){
            return BadRequest("Teacher or Subject not found");
        }

        var registered = new RegisteredTeacher{
            TeacherId = teacher.StaffID,
            TeacherName = teacher.Title + " " + teacher.FirstName + " " + teacher.OtherName + " " +teacher.LastName,
            DateAdded =  DateTime.Today.Date.ToString("dd MMMM, yyyy"),
        };

        subject.Teachers.Add(registered);
        await context.SaveChangesAsync();
        return Ok($"{registered.TeacherName} has been added as a registered teacher for {subject.SubjectId}");    

        }


        
    }
}