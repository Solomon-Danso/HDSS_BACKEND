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
    [Route("api/Grade")]
    public class GradingSystems : ControllerBase
    {
        private readonly DataContext context;
        public GradingSystems(DataContext ctx)
        {
            context=ctx;
        }
        [HttpGet("ClassList")]
        public async Task<IActionResult>GetClassList(string Level){
            var user = context.Students.Where(a=>a.Level==Level).OrderBy(a=>a.LastName).ToList();
            return Ok(user);
        }
        
        [HttpPost("UploadResult")]
        public async Task<IActionResult>ResultUploader([FromBody] TermResult request, string SID){
            var teacher = context.Teachers.FirstOrDefault(a=>a.StaffID==SID);
            if(teacher==null){
                return BadRequest("Teacher Not Found");
            }
            var r = new TermResult{
                StudentId = request.StudentId,
                StudentName = request.StudentName,
                ClassScore = request.ClassScore,
                ExamScore = request.ExamScore,
                Level = request.Level,
                Subject = request.Subject,
                AcademicYear = request.AcademicYear,
                AcademicTerm = request.AcademicTerm,
                TeacherId = teacher.StaffID,
                TeacherName = teacher.FirstName+" "+teacher.OtherName+" "+teacher.LastName,
                DateUploaded = DateTime.Today.Date.ToString("dd MMMM, yyyy"),
                SpecificDateAndTime = DateTime.Now,

            };
            var a = r.ClassScore/200;
            var b = r.ExamScore/200;
            var c = a+b;
            var d = c*100;



            if(d>79.4){
                r.Grade="A";
                r.Comment = "Excellent";
            }
            else if(d>69.4 && d<=79.4){
                r.Grade="B";
                r.Comment="Very Good";
            }
             else if(d>59.4 && d<=69.4){
                r.Grade="C";
                r.Comment="Good";
            }
            else if(d>49.4 && d<=59.4){
                r.Grade="D";
                r.Comment="Pass";
            }
             else if(d>30.4 && d<=49.4){
                r.Grade="E";
                r.Comment="Weak";
            }
             else {
                r.Grade="F";
                r.Comment="Fail";
            }

            return Ok("Result Uploaded");


        }






















        }
}
