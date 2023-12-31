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
    public class StudentApp : ControllerBase
    {
        private readonly DataContext context;
          string Country;
        string City;
        double latitude;
        double logitude;
         Constants constant = new Constants();
        public StudentApp(DataContext ctx)
        {
            context = ctx;
        }

                [HttpPost("UploadTimeTable")]
        public async Task<IActionResult> UploadTimeTable([FromForm]LMSDto request, string ID){
       
         
         if (request.Slide == null || request.Slide.Length == 0)
    {
        return BadRequest("Invalid slide");
    }

    // Create the uploads directory if it doesn't exist
    var uploadsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "LMS", "TimeTables");
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
   
   
    var s = new TimeTable{
        //Select the subject name from an option in the frontend
      
      
       ClassName = request.ClassName,
       DateAdded = DateTime.Today.Date.ToString("dd MMMM, yyyy"),
       SlidePath = Path.Combine("LMS/TimeTables", slideName),
      
       AcademicYear = request.AcademicYear,
       AcademicTerm = request.AcademicTerm,
      
      
    };

    var teacher = context.Classess.FirstOrDefault(a=>a.ClassName==s.ClassName);
    if (teacher==null){
        return BadRequest("Teacher not found");
    }
    s.TeacherName = teacher.ClassTeacher;
    s.StaffID = teacher.TeacherId;
  var c = context.TimeTables.Where(a=>a.ClassName==s.ClassName).Count();
  if (c>0){
    var d = context.TimeTables.FirstOrDefault(a=>a.Id>0);
    if (d==null){
        return BadRequest("TimeTables not found");
    }
    context.TimeTables.Remove(d);
    context.TimeTables.Add(s);

  }else{
context.TimeTables.Add(s);
  }



    
    await context.SaveChangesAsync();
    await TeacherAuditor(ID, constant.UploadATimeTable);

    return Ok($"Timetable has been Uploaded successfully");
    
    }

[HttpGet("GetTeacherClass")]
public async Task<IActionResult>GetTeacherClass(string ID){
    var c= context.Classess.Where(a=>a.TeacherId==ID).OrderBy(a=>a.ClassName).ToList();
    return Ok(c);
}

    [HttpPost("AddStudentNotes")]
    public async Task<IActionResult> AddStudentNotes([FromBody] StudentNote request, string SID){
        var stud = context.Students.FirstOrDefault(a=>a.StudentId==SID);
        if(stud==null){
            return BadRequest("Student not found");
        }

        var note = new StudentNote{
            StudentId = stud.StudentId,
            FullName = stud.FirstName+" "+stud.OtherName+" "+stud.LastName,
            Level = stud.Level,
            ResourceType = "Typed Notes",
            Subject = request.Subject,
            Notes = request.Notes,
            AcademicTerm = request.AcademicTerm,
            AcademicYear = request.AcademicYear,
            DateAdded = DateTime.Today.Date.ToString("dd MMMM, yyyy")
        };
        context.StudentNotes.Add(note);
        await context.SaveChangesAsync();
        await StudentAuditor(SID, constant.AstudNote);
        return Ok("Notes Added successfully");


    }



    [HttpGet("ViewAllNotesForAStudent")]
    public async Task<IActionResult>GetStudentNotes(string StudentID){
        var c = context.StudentNotes.Where(a=>a.StudentId==StudentID).OrderByDescending(r=>r.Id).ToList();
        await StudentAuditor(StudentID, constant.VstudNotes);
        return Ok(c);
    }

    [HttpGet("ViewAllNotesForAStudentByTeacher")]
    public async Task<IActionResult>GetStudentNotesByTeacher(string StudentID, string SID){
        var c = context.StudentNotes.Where(a=>a.StudentId==StudentID).OrderByDescending(r=>r.Id).ToList();
        await TeacherAuditor(SID, constant.TVstudNotes);
        return Ok(c);
    }

    [HttpGet("ViewOneNotesForAStudent")]
    public async Task<IActionResult>GetOneStudentNotes(int ID, string StudentId){
        var c = context.StudentNotes.FirstOrDefault(a=>a.Id==ID&&a.StudentId==StudentId);
        if(c==null){
            return BadRequest("Notes not found");
        }
        await StudentAuditor(StudentId, constant.VstudNotesD);
        return Ok(c);
    }

    [HttpGet("ViewOneNotesForAStudentByTeacher")]
    public async Task<IActionResult>GetOneStudentNotesByTeacher(int ID, string StudentId, string SID){
        var c = context.StudentNotes.FirstOrDefault(a=>a.Id==ID&&a.StudentId==StudentId);
        if(c==null){
            return BadRequest("Notes not found");
        }
           await TeacherAuditor(SID, constant.TVstudNotesD);
        return Ok(c);
    }

    [HttpDelete("DeleteOneNotesForAStudent")]
    public async Task<IActionResult>DeleteOneStudentNotes(int ID, string StudentId){
        var c = context.StudentNotes.FirstOrDefault(a=>a.Id==ID&&a.StudentId==StudentId);
        if(c==null){
            return BadRequest("Notes not found");
        }
        context.StudentNotes.Remove(c);
        await context.SaveChangesAsync();
        await StudentAuditor(StudentId, constant.DstudNote);
        return Ok("Notes Deleted Successfully");
    }



    [HttpGet("ViewAllTimeTableTeachers")]
    public async Task<IActionResult>ViewAllBookTeachers(string ID){
        var slide = context.TimeTables.Where(a=>a.StaffID==ID).ToList();
        await TeacherAuditor(ID,constant.ViewUploadedTimeTables);
        return Ok(slide);
    }

    [HttpDelete("deleteTimeTables")]
    public async Task<IActionResult>DeleteBooks(int Id, string SID){
        var slide = context.TimeTables.FirstOrDefault(a=>a.Id==Id);
        if(slide == null){
            return BadRequest("TimeTable not found");
        }
        context.TimeTables.Remove(slide);
        await context.SaveChangesAsync();
        try{
            await TeacherAuditor(SID, constant.DeletUploadedTimeTable);
        }
        catch(Exception ex){
            await AdminAuditor(SID, constant.DeletUploadedTimeTable);
        }
        
        return Ok("Slides deleted successfully");
    }


    [HttpGet("TimeTable")]
    public async Task<IActionResult>TimeTable(string SID, string ClassName){
        var t = context.TimeTables.Where(a=>a.ClassName==ClassName).OrderByDescending(r=>r.Id).ToList();
        await StudentAuditor(SID,constant.STimeTable);
        return Ok(t);

    }

     [HttpGet("Video")]
    public async Task<IActionResult>Video(string SID, string ClassName, string Subject){
        var t = context.Videos.Where(a=>a.ClassName==ClassName&&a.SubjectName==Subject).OrderByDescending(r=>r.Id).ToList();
        await StudentAuditor(SID,constant.SVideos);
        return Ok(t);

    }

    [HttpGet("SingleVideo")]
    public async Task<IActionResult>SingleVideo(string SID, int Id){
        var t = context.Videos.FirstOrDefault(a=>a.Id==Id);
        if (t==null){
            return BadRequest("Video Not Found");
        }
        t.NumberOfViews = t.NumberOfViews+1;
        await context.SaveChangesAsync();
        await StudentAuditor(SID,constant.SVideosS);
        return Ok(t);

    }

    [HttpGet("SingleAudio")]
    public async Task<IActionResult>SingleAudio(string SID, int Id){
        var t = context.Audios.FirstOrDefault(a=>a.Id==Id);
        if (t==null){
            return BadRequest("Audio Not Found");
        }
        t.NumberOfViews = t.NumberOfViews+1;
        await context.SaveChangesAsync();
        await StudentAuditor(SID,constant.SVideosS);
        return Ok(t);

    }


    [HttpGet("SinglePicture")]
    public async Task<IActionResult>SinglePicture(string SID, int Id){
        var t = context.Pictures.FirstOrDefault(a=>a.Id==Id);
        if (t==null){
            return BadRequest("Picture Not Found");
        }
        t.NumberOfViews = t.NumberOfViews+1;
        await context.SaveChangesAsync();
        await StudentAuditor(SID,constant.SVideosS);
        return Ok(t);

    }

    [HttpGet("SingleSlide")]
    public async Task<IActionResult>SingleSlide(string SID, int Id){
        var t = context.Slides.FirstOrDefault(a=>a.Id==Id);
        if (t==null){
            return BadRequest("Slide Not Found");
        }
        t.NumberOfViews = t.NumberOfViews+1;
        await context.SaveChangesAsync();
        await StudentAuditor(SID,constant.SVideosS);
        return Ok(t);

    }

    [HttpGet("SingleBook")]
    public async Task<IActionResult>SingleBook(string SID, int Id){
        var t = context.Books.FirstOrDefault(a=>a.Id==Id);
        if (t==null){
            return BadRequest("Book Not Found");
        }
        t.NumberOfViews = t.NumberOfViews+1;
        await context.SaveChangesAsync();
        await StudentAuditor(SID,constant.SVideosS);
        return Ok(t);

    }











[HttpPost("StudentNote")]
public async Task<IActionResult>StudentNote([FromBody] StudentNote request,string SID){
var stu = context.Students.FirstOrDefault(a=>a.StudentId==SID);
if(stu == null){
    return BadRequest("Student Note Found");
}

var s = new StudentNote{
StudentId = stu.StudentId,
FullName = stu.LastName+" "+stu.OtherName+" "+stu.FirstName,
Level = stu.Level,
ResourceUrl = request.ResourceUrl,
ResourceType = request.ResourceType,
Notes = request.Notes,
Subject = request.Subject,
AcademicTerm = request.AcademicTerm,
AcademicYear = request.AcademicYear,
DateAdded = DateTime.Today.Date.ToString("dd MMMM, yyyy")

};
context.StudentNotes.Add(s);
await context.SaveChangesAsync();
await StudentAuditor(SID, constant.StudentNote);

return Ok("Notes Uploaded successfully");
}

[HttpPost("UpdateStudentNote")]
public async Task<IActionResult>UpdateStudentNote([FromBody] StudentNote request,string SID, int Id){
   var s = context.StudentNotes.FirstOrDefault(x => x.Id == Id);
    if(s==null){
        return BadRequest("Student notes not found");
    }

s.FullName = request.FullName;
s.Level = request.Level;
s.ResourceUrl = request.ResourceUrl;
s.ResourceType = request.ResourceType;
s.Notes = request.Notes;
s.Subject = request.Subject;
s.AcademicTerm = request.AcademicTerm;
s.AcademicYear = request.AcademicYear;
s.DateAdded = DateTime.Today.Date.ToString("dd MMMM, yyyy");


await context.SaveChangesAsync();
await StudentAuditor(SID, constant.StudentUpdatedNote);

return Ok("Notes Uploaded successfully");
}



[HttpGet("GetMyStudentNotes")]
public async Task<IActionResult>MyStudentNotes(string SID){
    var s = context.StudentNotes.Where(x => x.StudentId == SID).OrderByDescending(r=>r.Id).ToList();
    await StudentAuditor(SID,constant.StudentListedNote);
    return Ok(s);
}

[HttpDelete("DeleteStudentNotes")]
public async Task<IActionResult>DeleteStudentNotes(string SID, int Id){
    var s = context.StudentNotes.FirstOrDefault(x => x.Id == Id);
    if(s==null){
        return BadRequest("Student notes not found");
    }
    context.StudentNotes.Remove(s);
    await context.SaveChangesAsync();
    await StudentAuditor(SID,constant.StudentViewedNote);
    return Ok("Notes Deleted Successfully");
}

[HttpGet("GetClassNotes")]
public async Task<IActionResult>MyClassNotes(string SID, string Level, string Subject, string Term, string Year){
    var s = context.StudentNotes.Where(x => x.Level==Level&&x.Subject==Subject&&x.AcademicTerm==Term&&x.AcademicYear==Year).OrderBy(r=>r.FullName).ToList();
    await TeacherAuditor(SID,constant.TeacherViewedNote);
    return Ok(s);
}







     [HttpGet("Audio")]
    public async Task<IActionResult>Audio(string SID, string ClassName,string Subject){
        var t = context.Audios.Where(a=>a.ClassName==ClassName&&a.SubjectName==Subject).OrderByDescending(r=>r.Id).ToList();
        await StudentAuditor(SID,constant.SAudios);
        return Ok(t);

    }

     [HttpGet("Picture")]
    public async Task<IActionResult>Picture(string SID, string ClassName, string Subject){
        var t = context.Pictures.Where(a=>a.ClassName==ClassName&&a.SubjectName==Subject).OrderByDescending(r=>r.Id).ToList();
        await StudentAuditor(SID,constant.SPictures);
        return Ok(t);

    }


     [HttpGet("Slide")]
    public async Task<IActionResult>Slide(string SID, string ClassName, string Subject){
        var t = context.Slides.Where(a=>a.ClassName==ClassName && a.SubjectName==Subject).OrderByDescending(r=>r.Id).ToList();
        await StudentAuditor(SID,constant.SSlides);
        return Ok(t);
    }

     [HttpGet("Book")]
    public async Task<IActionResult>Book(string SID, string ClassName, string Subject){
        var t = context.Books.Where(a=>a.ClassName==ClassName && a.SubjectName==Subject).OrderByDescending(r=>r.Id).ToList();
        await StudentAuditor(SID,constant.SBooks);
        return Ok(t);

    }

    [HttpGet("Assignment")]
    public async Task<IActionResult>StudentAssignment( string SID, string ClassName){
        var assign = context.AssignmentForStudents.Where(a=>a.ClassName == ClassName&&DateTime.Now<=a.Deadline).OrderByDescending(r=>r.Id).ToList();
        await StudentAuditor(SID,constant.StudViewAssignment);
        return Ok(assign);
    }

    [HttpGet("AssignmentDetails")]
    public async Task<IActionResult>StudentAssignmentView( string SID, string ID){
        var assign = context.AssignmentForStudents.FirstOrDefault(a=>a.QuestionId==ID&&a.StudentId==SID );
        if(assign==null){
            return BadRequest("Assignment Not Found");
        }

        await StudentAuditor(SID,constant.StudViewAssignment);
        return Ok(assign);
    }

[HttpPost("UploadTestnQuiz")]
public async Task<IActionResult>UploadTestnQuiz([FromBody]TestnQuizTeacher request, string Level, string Subject, string Status, string StaffID){

var t = context.Teachers.FirstOrDefault(a=>a.StaffID==StaffID);
if(t==null){
    return BadRequest("Teacher not found");
}


var tnq = new TestnQuizTeacher{
Subject = Subject,
Level = Level,
Question = request.Question,
OptionA = request.OptionA,
OptionB = request.OptionB,
OptionC = request.OptionC,
OptionD = request.OptionD,
OptionE = request.OptionE,
Answer = request.Answer,
IsAnswered = false,
Duration = request.Duration,
TeacherId = t.StaffID,
TeacherName = t.FirstName+" " + t.OtherName+" "+t.LastName,
ProfilePic = t.FilePath,
DesignatedMarks = request.DesignatedMarks,
UploadDate = DateTime.Today.Date
};
if(Status=="New"){
    tnq.QuizId = AssignmentIdGenerator();
    tnq.Deadline = request.Deadline;
}
else if(Status=="Existing"){
var q = context.TestnQuizTeachers.OrderBy(a=>a.Id).LastOrDefault(a=>a.TeacherId==tnq.TeacherId&&a.Subject==tnq.Subject&&a.Level==tnq.Level);
if(q==null){
    tnq.QuizId = AssignmentIdGenerator();
}
else{
    tnq.QuizId = q.QuizId;
    tnq.Deadline = q.Deadline;
    tnq.Duration = q.Duration;
}

}
 context.TestnQuizTeachers.Add(tnq);
 await context.SaveChangesAsync();

var studentList = context.Students.Where(a=>a.Level==tnq.Level).ToList();
foreach(var student in studentList){
    var stud = new TestnQuizStudent{
        QuizId = tnq.QuizId,
        Subject = tnq.Subject,
        Level = tnq.Level,
        Question = tnq.Question,
        OptionA = tnq.OptionA,
        OptionB = tnq.OptionB,
        OptionC = tnq.OptionC,
        OptionD = tnq.OptionD,
        OptionE = tnq.OptionE,
        IsAnswered = tnq.IsAnswered,
        Deadline = tnq.Deadline,
        Duration = tnq.Duration,
        StudentId = student.StudentId,
        StudentName = student.FirstName+" " + student.OtherName+" " + student.LastName,
        ProfilePic = student.ProfilePic,
        QuestionId = tnq.Id,
        Answer = tnq.Answer,
        DesignatedMarks=tnq.DesignatedMarks,
        UploadDate = tnq.UploadDate

    };
    context.TestnQuizStudents.Add(stud);
    await context.SaveChangesAsync();
    await TeacherAuditor(t.StaffID, constant.TeacherQuiz);


}



return Ok("Quiz Uploaded Successfully");


}


[HttpPost("TestnQuizMarking")]
public async Task<IActionResult> TestnQuizMarking(int questionId, string quizid, string studentId, string answer){

var ques = context.TestnQuizStudents.FirstOrDefault(a=>a.QuestionId==questionId&&a.QuizId==quizid&&a.StudentId==studentId);
if (ques == null){
    return BadRequest("Quiz not found");
}

var mark = new TestnQuizStudentMark{
    QuestionId = ques.QuestionId,
    QuizId = ques.QuizId,
    StudentAnswer = answer,
    StudentId = ques.StudentId,
    SolutionDate = DateTime.Today.Date
};
if(mark.StudentAnswer==ques.Answer){
    mark.Mark = ques.DesignatedMarks;
}

ques.IsAnswered = true;
context.TestnQuizStudentMarks.Add(mark);
await context.SaveChangesAsync();

var mk = context.TestnQuizStudentMarks.Where(a=>a.QuizId==mark.QuizId&&a.StudentId==mark.StudentId).Sum(r=>r.Mark);
var total = context.TestnQuizStudents.Where(a=>a.QuizId==mark.QuizId&&a.StudentId==mark.StudentId).Sum(r=>r.DesignatedMarks);
var w = context.TestnQuizStudents.FirstOrDefault(a=>a.QuizId==mark.QuizId&&a.StudentId==mark.StudentId);

var q = new TestnQuizStudentTotalScore{
    StudentId = w?.StudentId,
    MarksObtained = mk,
    TotalScore= total,
    QuizId = w?.QuizId,
    SubjectName = w?.Subject,
    Level=w?.Level,
    StudentName = ques.StudentName,
    ProfilePic = ques.ProfilePic,

};
bool checker = await context.TestnQuizStudentTotalScores.AnyAsync(a=>a.StudentId==q.StudentId&&a.QuizId==q.QuizId);
if(checker){
    var a = context.TestnQuizStudentTotalScores.FirstOrDefault(a=>a.StudentId==q.StudentId&&a.QuizId==q.QuizId);
    if(a==null){
        return BadRequest("Total Score Not Found");
    }
    a.TotalScore=total;
    a.MarksObtained=mk;
    await context.SaveChangesAsync();
}
else{
context.TestnQuizStudentTotalScores.Add(q);
await context.SaveChangesAsync();
}


 var G = context.TestnQuizStudentTotalScores.FirstOrDefault(a=>a.StudentId==q.StudentId&&a.QuizId==q.QuizId);
if(G==null){
        return BadRequest("Total Score Not Found");
}

var grade = new GradeBook{
QuizId = G.QuizId,
StudentName = G.StudentName,
Level = G.Level,
ProfilePic = G.ProfilePic,
SubjectName = G.SubjectName,
MarksObtained = G.MarksObtained,
TotalObtained = G.TotalScore,
DateUploaded = DateTime.Today.Date.ToString("dd MMMM, yyyy"),
StudentId = G.StudentId
};



bool checks = await context.GradeBooks.AnyAsync(a=>a.StudentId==grade.StudentId&&a.QuizId==grade.QuizId&&a.SubjectName==grade.SubjectName);
if (checks){
    var ch = context.GradeBooks.FirstOrDefault(a=>a.StudentId==grade.StudentId&&a.QuizId==grade.QuizId&&a.SubjectName==grade.SubjectName);
    if(ch==null){
        return BadRequest("No Grade Book found");
    }
    
    ch.MarksObtained = grade.MarksObtained;
    ch.TotalObtained = grade.TotalObtained;
    await context.SaveChangesAsync();

}
else{
context.GradeBooks.Add(grade);
 await context.SaveChangesAsync();
}


var termResults = context.GradeBooks
    .Where(a=>a.QuizId==grade.QuizId)
    .ToList();

// Group TermResults by Average and order the groups by Average in descending order
var groupedGrades = termResults
    .GroupBy(a => a.MarksObtained)
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












await StudentAuditor(ques.StudentId, constant.StudentQuiz);
return Ok("Marks Recorded Successfully");

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





[HttpGet("QuizTotalScore")]
public async Task<IActionResult>QuizTotalScore(string QuizId, string Level){
    var stud = context.TestnQuizStudentTotalScores.Where(a=>a.QuizId==QuizId&&a.Level==Level).OrderByDescending(r=>r.MarksObtained).ToList();
    return Ok(stud);
}

[HttpGet("ViewTestnQuiz")]
public async Task<IActionResult> ViewTestnQuiz(string level, string studentId){
    var quiz = context.TestnQuizStudents
        .Where(a => a.Level == level && DateTime.Now <= a.Deadline&&a.StudentId==studentId&&a.IsAnswered==false)
        .OrderByDescending(r=>r.Id)
        .GroupBy(a => a.QuizId)
        .Select(group => group.FirstOrDefault()) // Selecting the first record from each group
        .ToList();

    return Ok(quiz);
}

[HttpGet("ViewTestnQuizAllQuestion")]
public async Task<IActionResult> ViewTestnQuizAllQuestion(string level,string quizid, string studentId){
    var quiz = context.TestnQuizStudents
        .Where(a => a.Level == level && DateTime.Now <= a.Deadline&&a.QuizId==quizid&&a.StudentId==studentId&&a.IsAnswered==false)
        .ToList();
        foreach(var q in quiz){
            q.IsStarted = true;
            await context.SaveChangesAsync();
        }
         var quizTimer = new System.Timers.Timer(TimeSpan.FromMinutes(1).TotalMilliseconds);
        quizTimer.Elapsed += async (sender, e) => await ReduceDurationAndCheckCompletion(quizid,studentId);
        quizTimer.Start();

    return Ok(quiz);
}

[HttpGet("GetTimer")]
public async Task<IActionResult>GetTimer(string QuizId, string StudentId){

var t = context.TestnQuizStudents.FirstOrDefault(a => a.QuizId == QuizId && a.StudentId==StudentId);
if(t==null){
    return BadRequest("No Last Quiz");
}



var theTimer = new QuizTimer{
    QuizId = t.QuizId,
    StudentId = t.StudentId,
    TimeLeft = t.Duration,
};
bool checker = await context.QuizTimers.AnyAsync(a => a.QuizId == QuizId &&a.StudentId==StudentId);
if(checker){
    var a = context.QuizTimers.FirstOrDefault(a => a.QuizId == QuizId &&a.StudentId==StudentId);
    if(a==null){
    return BadRequest("No Quiz Timer");
}
    
    a.TimeLeft = theTimer.TimeLeft;
    await context.SaveChangesAsync();
}
else{
    context.QuizTimers.Add(theTimer);
    await context.SaveChangesAsync();
}


    var timer = context.QuizTimers.FirstOrDefault(a=>a.QuizId==QuizId&&a.StudentId==StudentId);
    return Ok(timer);
}


[HttpGet("GetGradeBook")]
public async Task<IActionResult>GetGradeBook(string studentId){
    var grade = context.GradeBooks.Where(a=>a.StudentId==studentId).ToList();
    return Ok(grade);
}


 

private async Task ReduceDurationAndCheckCompletion(string quizId, string studentId)
{
    using (var newContext = new DataContext()) // Create a new context instance
    {
        var quiz = await newContext.TestnQuizStudents
            .Where(a => a.QuizId == quizId && a.IsAnswered == false&&a.StudentId==studentId)
            .ToListAsync();

  
        foreach (var q in quiz)
        {
            q.Duration--;
            
            if (q.Duration <= 0)
            {
                q.IsAnswered = true; // Mark as answered when duration is zero
            }

        }

        await newContext.SaveChangesAsync();
    }
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