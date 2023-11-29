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
            r.TotalScore = a;
            if(r.ClassScore>50){
                return BadRequest("Input The Correct 50% Class Score for "+r.StudentName);
            }

            if( r.ExamScore>50){
                return BadRequest("Input The Correct 50% Exams Score for "+r.StudentName);
            }
            
            var b = a/100;
             
            var d = b*100;
            r.Average = d;
            


            if(d>79.4){
                r.Grade="Level 1 - A";
                r.Comment = "Advance";
            }
            else if(d>74.4 && d<=79.4){
                r.Grade="Level 2 - P";
                r.Comment="Proficient";
            }
             else if(d>69.4 && d<=74.4){
                r.Grade="Level 3 - AP";
                r.Comment="A. Proficiency";
            }

            else if(d>64.4 && d<=69.4){
                r.Grade="Level 4 - D";
                r.Comment="Developing";
            }
             else if(d>0 && d<=64.4){
                r.Grade="Level 5 - B";
                r.Comment="Beginning";
            }
             
           
        bool checker =await context.TermResults.AnyAsync(a=>a.StudentId==r.StudentId&&a.Subject==r.Subject&&a.Level==r.Level&&a.AcademicYear==r.AcademicYear&&a.AcademicTerm==r.AcademicTerm);
        if(checker){
           return BadRequest("You have already uploaded the result for "+r.StudentName);
        }
        else{
             context.TermResults.Add(r);

        await context.SaveChangesAsync();
        }


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


[HttpGet("ViewTermGrades")]
public async Task<IActionResult> ViewTermGrades(string StudentId, string Year, string Term, string Level)
{
    var Grade = context.TermResults
        .Where(a => a.Level == Level && a.StudentId == StudentId && a.AcademicYear == Year && a.AcademicTerm == Term)
        .OrderBy(a => a.Subject) // Sort by average score in descending order
        .ToList();

    return Ok(Grade);
}

// Method to convert an integer to its ordinal representation

[HttpPost("GeneralTReportInfo")]
public async Task<IActionResult> GeneralTReportInfo([FromBody]GeneralTReportInfo request){

var info = new GeneralTReportInfo{
AcademicTerm = request.AcademicTerm,
AcademicYear = request.AcademicYear,
VacationDate = request.VacationDate,
ReOpeningDate = request.ReOpeningDate,
};

var c = context.GeneralTReportInfos.Where(a=>a.Id>0).Count();
if (c>0){
    var tr = context.GeneralTReportInfos.FirstOrDefault(a=>a.Id>0);
    if(tr==null){
        return BadRequest("Academic Info Not Found");
    }
    tr.AcademicTerm = info.AcademicTerm;
    tr.AcademicYear = info.AcademicYear;
    tr.VacationDate = info.VacationDate;
    tr.ReOpeningDate = info.ReOpeningDate;

   await context.SaveChangesAsync();

}
else{
    context.GeneralTReportInfos.Add(info);
      await context.SaveChangesAsync();
}

return Ok("Setup was successfull");

}

[HttpGet("StudentCounters")]
public async Task<IActionResult>GetStudentCounter(string Level){
    var c = context.Students.Where(a=>a.Level==Level).Count();
    return Ok(c);
}

[HttpGet("GeneralInfo")]
public async Task<IActionResult>GetGeneralInfo(){
    var info = context.GeneralTReportInfos.FirstOrDefault(a=>a.Id>0);
    return Ok(info);
}




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
