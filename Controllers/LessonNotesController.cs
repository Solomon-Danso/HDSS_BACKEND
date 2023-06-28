using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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


private string NotesIdGenerator()
{
    byte[] randomBytes = new byte[4];
    using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
    {
        rng.GetBytes(randomBytes);
    }

    uint randomNumber = BitConverter.ToUInt32(randomBytes, 0);
    return randomNumber.ToString("D8");
}



[HttpPost("CreateLessonNotes")]
    public async Task<IActionResult> CreateNotes([FromBody]LessonNote request ,string StaffId){
        var teacher = context.Teachers.FirstOrDefault(t => t.StaffID == StaffId);
        if(teacher == null){
            return Unauthorized();
        }

var NotesId = NotesIdGenerator();

        var Lesson = new LessonNote{
            Id = request.Id,
            TeacherId = teacher.StaffID,
            TeacherName = teacher.Title+". "+teacher.FirstName+" "+teacher.OtherName+" " + teacher.LastName,
            DateWritten = DateTime.Today.Date.ToString("dd MMMM, yyyy"),
            Subject = request.Subject,
            SearchId = teacher.StaffID+NotesId,

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
Day5Phase3 = request.Day5Phase3,
NotesTicket = NotesId,


        };

        context.LessonNotes.Add(Lesson);
        await context.SaveChangesAsync();
        
     return Ok("Lesson Notes Submitted Successfully");
    }





[HttpPost("updateLessonNotes")]
public async Task<IActionResult> UpdateNotes([FromBody]LessonNote request ,string NoteId, string StaffID){
//The StaffId will be loaded from the frontend sessionStorage 
var searchId = StaffID + NoteId;
var Lesson = context.LessonNotes.FirstOrDefault(n=>n.SearchId == searchId);
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

public async Task<IActionResult> GetOneLessonNotes(string NoteId, string StaffID){
    var searchId = StaffID + NoteId;
    var notes =  context.LessonNotes.FirstOrDefault(e=>e.SearchId == searchId);
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


[HttpDelete("deleteLessonNotesText")]
public async Task<IActionResult> DeleteLessonNotesTextAsync(string NoteId, string StaffID){
 var searchId = StaffID + NoteId;

 var lesson = context.LessonNotes.FirstOrDefault(a => a.SearchId == searchId);
 if (lesson == null){
    return BadRequest("Enter Correct Ids for Notes and Staff");
 }
 context.LessonNotes.Remove(lesson);

 await context.SaveChangesAsync();
 return Ok("Notes Successfully deleted");

}




[HttpPost("MarkLessonNotes")]
public async Task<IActionResult> MarkLessonNotes([FromBody]LessonNote request ,string NoteId, string StaffID){
// The TeacherId will be inputed Manually by the Headteacher
var searchId = StaffID + NoteId;
var Lesson = context.LessonNotes.FirstOrDefault(n=>n.SearchId == searchId);
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
Lesson.HeadTeacherComment = request.HeadTeacherComment;
Lesson.HeadTeacherDateSigned = DateTime.Today.Date.ToString("dd MMMM, yyyy");



await context.SaveChangesAsync();
return Ok("Lesson Notes Marked Successfully");
}


[HttpPost("UploadNotes")]
public async Task<IActionResult> UploadNotes([FromForm] LessonNoteUploadDto request,string StaffId){

    
var teacher = context.Teachers.FirstOrDefault(t => t.StaffID == StaffId);
    if(teacher == null){
            return Unauthorized();
        }

 if (request.File == null || request.File.Length == 0)
    {
        return BadRequest("Invalid file");
    }


   // Create the uploads directory if it doesn't exist
    var uploadsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "LessonNotes", "Profile");
    if (!Directory.Exists(uploadsDirectory))
    {
        Directory.CreateDirectory(uploadsDirectory);
    }

    // Get the original file extension
    var fileExtension = Path.GetExtension(request.File.FileName);

    // Generate a unique file name
    var fileName = Guid.NewGuid().ToString() + fileExtension;

    // Save the uploaded file to the uploads directory
    var filePath = Path.Combine(uploadsDirectory, fileName);
    using (var stream = new FileStream(filePath, FileMode.Create))
    {
        await request.File.CopyToAsync(stream);
    }

var NoteId = NotesIdGenerator();

var uploadNote = new LessonNoteUpload{

     TeacherId = teacher.StaffID,
     TeacherName = teacher.Title+". "+teacher.FirstName+" "+teacher.OtherName+" " + teacher.LastName,
     DateUploaded = DateTime.Today.Date.ToString("dd MMMM, yyyy"),
     LessonNotePath = Path.Combine("LessonNotes/Profile", fileName),
     NotesTicket = NoteId,
     SearchId = teacher.StaffID+NoteId,

};
context.LessonNotesUpload.Add(uploadNote);

await context.SaveChangesAsync();

return Ok("Lessons Note Uploaded Successfully");

}





[HttpPut("UpdateNotes")]
public async Task<IActionResult> UpdatesNotes([FromForm] LessonNoteUploadDto request,string NoteId, string StaffID){

var searchId = StaffID + NoteId;
    
var lesson = context.LessonNotesUpload.FirstOrDefault(t => t.SearchId==searchId);
    
    if(lesson == null){
            return BadRequest("Please Enter A correct staff ID and lesson Notes Id"); 
        }

 if (request.File == null || request.File.Length == 0)
    {
        return BadRequest("Invalid file");
    }


   // Create the uploads directory if it doesn't exist
    var uploadsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "LessonNotes", "Profile");
    if (!Directory.Exists(uploadsDirectory))
    {
        Directory.CreateDirectory(uploadsDirectory);
    }

    // Get the original file extension
    var fileExtension = Path.GetExtension(request.File.FileName);

    // Generate a unique file name
    var fileName = Guid.NewGuid().ToString() + fileExtension;

    // Save the uploaded file to the uploads directory
    var filePath = Path.Combine(uploadsDirectory, fileName);
    using (var stream = new FileStream(filePath, FileMode.Create))
    {
        await request.File.CopyToAsync(stream);
    }


lesson.DateUploaded = DateTime.Today.Date.ToString("dd MMMM, yyyy");
lesson.LessonNotePath = Path.Combine("LessonNotes/Profile", fileName);
 

await context.SaveChangesAsync();

return Ok("Lessons Note Updated Successfully");

}


[HttpPut("markNotes")]
public async Task<IActionResult> markNotes([FromForm] LessonNoteUploadDto request,string NoteId, string StaffID){

var searchId = StaffID + NoteId;
    
var lesson = context.LessonNotesUpload.FirstOrDefault(t => t.SearchId==searchId);
    
    if(lesson == null){
            return BadRequest("Please Enter A correct staff ID and lesson Notes Id"); 
        }


lesson.HeadTeacherDateSigned = DateTime.Today.Date.ToString("dd MMMM, yyyy");
lesson.HeadTeacherComment = request.HeadTeacherComment;
 

await context.SaveChangesAsync();

return Ok("Lessons Note Updated Successfully");

}



[HttpGet("AllLessonNotesFiles")]

public async Task<IActionResult> GetAllLessonNotesFiles(){
    var notes =  context.LessonNotesUpload.ToList();
    return Ok(notes);
}

[HttpGet("OneLessonNotesFiles")]

public async Task<IActionResult> GetOneLessonNotesFiles(string NoteId, string StaffID){
    var searchId = StaffID + NoteId;
    var notes =  context.LessonNotesUpload.FirstOrDefault(e=>e.SearchId == searchId);
    return Ok(notes);
}

[HttpGet("GetLessonHistoryFiles")]
public async Task<IActionResult> GetLessonHistoryFiles(string accountId)
{
    var lessonnotes = context.LessonNotesUpload.Where(a => a.TeacherId == accountId).ToList();
    if (lessonnotes.Count == 0)
    {
        return BadRequest("No lessonnotes found ");
    }
    
    return Ok(lessonnotes);
}


[HttpDelete("deleteLessonNotesFiles")]
public async Task<IActionResult> DeleteLessonNotesTextFiles(string NoteId, string StaffID){
 var searchId = StaffID + NoteId;

 var lesson = context.LessonNotesUpload.FirstOrDefault(a => a.SearchId == searchId);
 if (lesson == null){
    return BadRequest("Enter Correct Ids for Notes and Staff");
 }
 context.LessonNotesUpload.Remove(lesson);

 await context.SaveChangesAsync();
 return Ok("Notes Successfully deleted");

}





    }
}