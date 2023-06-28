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
    [Route("api/LessonNotes")]
    public class LessonNotesController : ControllerBase
    {
          private readonly DataContext context;

        public LessonNotesController(DataContext ctx)
        {
            context = ctx;
        }


[HttpPost("CreateLessonNotes")]
    public async Task<IActionResult> CreateNotes([FromBody]LessonNote request ,string Id){
        var teacher = context.Teachers.FirstOrDefault(t => t.StaffID == Id);
        if(teacher == null){
            return Unauthorized();
        }

        var Lesson = new LessonNote{
            Id = request.Id,
            TeacherId = teacher.StaffID,
            TeacherName = teacher.Title+". "+teacher.FirstName+" "+teacher.OtherName+" " + teacher.LastName,
            DateWritten = DateTime.Today.Date.ToString("dd MMMM, yyyy"),
            Subject = request.Subject,

Stage = request.Stage,
classSize = request.classSize,
WeekStartDate = request.WeekStartDate,
WeekEndDate = request.WeekEndDate,
Period = request.Period,
Lesson = request.Lesson,
strand = request.strand,
substrand = request.substrand,
indicator = request.indicator,
performanceIndicator = request.performanceIndicator,
contentStandard = request.contentStandard,
coreCompetence = request.coreCompetence,
keywords = request.keywords,
TLMS = request.TLMS,
references = request.references,
Day1 = request.Day1,
Day1Phase1 = request.Day1Phase1,
Day1Phase2 = request.Day1Phase2,
Day1Phase3 = request.Day1Phase3,
Day2 = request.Day2,
Day2Phase1 = request.Day2Phase1,
Day2Phase2 = request.Day2Phase2,
Day2Phase3 = request.Day2Phase3,
Day3 = request.Day3,
Day3Phase1 = request.Day3Phase1,
Day3Phase2 = request.Day3Phase2,
Day3Phase3 = request.Day3Phase3,
Day4 = request.Day4,
Day4Phase1 = request.Day4Phase1,
Day4Phase2 = request.Day4Phase2,
Day4Phase3 = request.Day4Phase3,
Day5 = request.Day5,
Day5Phase1 = request.Day5Phase1,
Day5Phase2 = request.Day5Phase2,
Day5Phase3 = request.Day5Phase3


        };

        context.LessonNotes.Add(Lesson);
        await context.SaveChangesAsync();
        
     return Ok("Lesson Notes Submitted Successfully");
    }

[HttpPost("updateLessonNotes")]
public async Task<IActionResult> UpdateNotes([FromBody]LessonNote request ,string Id){

var Lesson = context.LessonNotes.FirstOrDefault(n=>n.TeacherId == Id);
if(Lesson == null){
    return Unauthorized();
}

Lesson.Subject = request.Subject;
Lesson.Stage = request.Stage;
Lesson.classSize = request.classSize;
Lesson.WeekStartDate = request.WeekStartDate;
Lesson.WeekEndDate = request.WeekEndDate;
Lesson.Period = request.Period;
Lesson.Lesson = request.Lesson;
Lesson.strand = request.strand;
Lesson.substrand = request.substrand;
Lesson.indicator = request.indicator;
Lesson.performanceIndicator = request.performanceIndicator;
Lesson.contentStandard = request.contentStandard;
Lesson.coreCompetence = request.coreCompetence;
Lesson.keywords = request.keywords;
Lesson.TLMS = request.TLMS;
Lesson.references = request.references;
Lesson.Day1 = request.Day1;
Lesson.Day1Phase1 = request.Day1Phase1;
Lesson.Day1Phase2 = request.Day1Phase2;
Lesson.Day1Phase3 = request.Day1Phase3;
Lesson.Day2 = request.Day2;
Lesson.Day2Phase1 = request.Day2Phase1;
Lesson.Day2Phase2 = request.Day2Phase2;
Lesson.Day2Phase3 = request.Day2Phase3;
Lesson.Day3 = request.Day3;
Lesson.Day3Phase1 = request.Day3Phase1;
Lesson.Day3Phase2 = request.Day3Phase2;
Lesson.Day3Phase3 = request.Day3Phase3;
Lesson.Day4 = request.Day4;
Lesson.Day4Phase1 = request.Day4Phase1;
Lesson.Day4Phase2 = request.Day4Phase2;
Lesson.Day4Phase3 = request.Day4Phase3;
Lesson.Day5 = request.Day5;
Lesson.Day5Phase1 = request.Day5Phase1;
Lesson.Day5Phase2 = request.Day5Phase2;
Lesson.Day5Phase3 = request.Day5Phase3;




await context.SaveChangesAsync();
return Ok("Lesson Notes Updated Successfully");
}

[HttpGet("AllLessonNotes")]

public async Task<IActionResult> GetAllLessonNotes(){
    var notes =  context.LessonNotes.ToList();
    return Ok(notes);
}

[HttpGet("OneLessonNotes")]

public async Task<IActionResult> GetOneLessonNotes(string Id){
    var notes =  context.LessonNotes.FirstOrDefault(e=>e.TeacherId == Id);
    return Ok(notes);
}

[HttpGet("GetLessonHistory")]
public async Task<IActionResult> GetLessonHistory(string accountId)
{
    var lessonnotes = context.LessonNotes.Where(a => a.TeacherId == accountId).ToList();
    if (lessonnotes.Count == 0)
    {
        return BadRequest("No lessonnotes found ");
    }
    
    return Ok(lessonnotes);
}













    }
}