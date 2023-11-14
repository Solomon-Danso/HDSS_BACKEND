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
            var a = r.ClassScore+r.ExamScore;
            
            var b = a/200;
             
            var d = b*100;
            r.Average = d;
            


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

           
        bool checker =await context.TermResults.AnyAsync(a=>a.StudentId==r.StudentId&&a.Subject==r.Subject&&a.Level==r.Level&&a.AcademicYear==r.AcademicYear&&a.AcademicTerm==r.AcademicTerm);
        if(checker){
            context.TermResults.Add(r);
        }
        else{
             context.TermResults.Add(r);

        await context.SaveChangesAsync();
        }

// Retrieve TermResults from the database
// Retrieve TermResults from the database
// Retrieve TermResults from the database
var termResults = context.TermResults
    .Where(a => a.Level == r.Level && a.Subject == r.Subject && a.AcademicYear == r.AcademicYear && a.AcademicTerm == r.AcademicTerm)
    .ToList();

// Group TermResults by Average and order the groups by Average in descending order
var groupedGrades = termResults
    .GroupBy(a => a.Average)
    .OrderByDescending(g => g.Key);

int position = 1;

foreach (var group in groupedGrades)
{
    var sortedGroup = group.OrderBy(a => Guid.NewGuid()).ToList(); // Shuffle the group to randomize order
    int samePosition = position;

    foreach (var result in sortedGroup)
    {
        result.Position = GetOrdinal(samePosition);
    }

    position += sortedGroup.Count;
}

await context.SaveChangesAsync();

           

            return Ok("Result Uploaded");


        }




[HttpGet("ViewGrades")]
public async Task<IActionResult> ViewGrades(string l, string s, string y, string t)
{
    var classGrade = context.TermResults
        .Where(a => a.Level == l && a.Subject == s && a.AcademicYear == y && a.AcademicTerm == t)
        .OrderByDescending(a => a.Average) // Sort by average score in descending order
        .ToList();

    

    return Ok(classGrade);
}

// Method to convert an integer to its ordinal representation
private string GetOrdinal(int number)
{
    if (number <= 0) return number.ToString();

    switch (number % 100)
    {
        case 11:
        case 12:
        case 13:
            return number + "th";
    }

    switch (number % 10)
    {
        case 1:
            return number + "st";
        case 2:
            return number + "nd";
        case 3:
            return number + "rd";
        default:
            return number + "th";
    }
}


















        }
}
