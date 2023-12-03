using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HDSS_BACKEND.Data;
using HDSS_BACKEND.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Globalization;

namespace HDSS_BACKEND.Controllers
{
    [ApiController]
    [Route("api/Grade")]
    public class GradingSystems : ControllerBase
    {
        private readonly DataContext context;
          string Country;
        string City;
        double latitude;
        double logitude;
        Constants constant = new Constants();
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
           var ch = context.TermResults.FirstOrDefault(a=>a.StudentId==r.StudentId&&a.Subject==r.Subject&&a.Level==r.Level&&a.AcademicYear==r.AcademicYear&&a.AcademicTerm==r.AcademicTerm);
            if(ch==null){
                return BadRequest("Student not found");
            }

                ch.StudentId = r.StudentId;
                ch.StudentName = r.StudentName;
                ch.ClassScore = r.ClassScore;
                ch.ExamScore = r.ExamScore;
                ch.Level = r.Level;
                ch.Subject = r.Subject;
                ch.AcademicYear = r.AcademicYear;
                ch.AcademicTerm = r.AcademicTerm;
                ch.TeacherId = teacher.StaffID;
                ch.TeacherName = teacher.FirstName+" "+teacher.OtherName+" "+teacher.LastName;
                ch.DateUploaded = DateTime.Today.Date.ToString("dd MMMM, yyyy");
                ch.SpecificDateAndTime = DateTime.Now;
                ch.Grade = r.Grade;
                ch.Comment = r.Comment;
                ch.Average = r.Average;
                ch.TotalScore = r.TotalScore;

                await context.SaveChangesAsync();
                



           
           
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

await ReportAnalysis(r.Level, r.StudentId, r.AcademicYear, r.AcademicTerm);

           

            return Ok("Result Uploaded Successfully");


        }

[HttpGet("GetReportAnalysis")]
public async Task<IActionResult>GetReportAnalysis(string StudentId){
    var analysis = context.ReportAnalysis.FirstOrDefault(a=>a.StudentId==StudentId);
    return Ok(analysis);
}





[ApiExplorerSettings(IgnoreApi = true)] 
public async Task ReportAnalysis(string level, string SID, string year, string term){

    var TotalScoreOb = context.TermResults.Where(a=>a.Level==level&&a.StudentId==SID&&a.AcademicYear==year&&a.AcademicTerm==term).Sum(r=>r.Average);
    var subjList = context.StudentForSubjects.Where(a=>a.StudentID==SID&&a.ClassName==level).Count();
    var EntireScore = subjList*100;
    var TermAverage = TotalScoreOb/subjList;
    var TotalPass = context.TermResults.Where(a=>a.Level==level&&a.StudentId==SID&&a.AcademicYear==year&&a.AcademicTerm==term&&a.Average>49.49).Count();
    var TotalFailed = context.TermResults.Where(a=>a.Level==level&&a.StudentId==SID&&a.AcademicYear==year&&a.AcademicTerm==term&&a.Average<=49.49).Count();
    var s = context.Students.FirstOrDefault(a=>a.StudentId==SID);


    var analysis = new ReportAnalysis{
        ThisTermTotalScoreObtained = TotalScoreOb,
        ThisTermEntireTotalScore = EntireScore,
        ThisTermAverageScore = TermAverage,
        ThisTermTotalPass = TotalPass,
        ThisTermTotalFailed = TotalFailed,
        StudentId = s?.StudentId,
        ClassName = level,
        StudentName = s?.FirstName+" " + s?.OtherName+" " + s?.LastName,
        ThisTermAcademicTerm = term,
        ThisTermAcademicYear = year,
        DateAdded = DateTime.Today.Date.ToString("dd MMMM, yyyy")

    };
    

var counter = context.ReportAnalysis.Where(a => a.ClassName == analysis.ClassName && a.ThisTermAcademicYear == analysis.ThisTermAcademicYear && a.ThisTermAcademicTerm == analysis.ThisTermAcademicTerm&&a.StudentId==analysis.StudentId).Count();

if(counter>0){

bool checker = await context.ReportAnalysis.AnyAsync(a => a.ClassName == analysis.ClassName && a.ThisTermAcademicYear == analysis.ThisTermAcademicYear && a.ThisTermAcademicTerm == analysis.ThisTermAcademicTerm&&a.StudentId==analysis.StudentId);
if(checker){
  var ch = context.ReportAnalysis.FirstOrDefault(a => a.ClassName == analysis.ClassName && a.ThisTermAcademicYear == analysis.ThisTermAcademicYear && a.ThisTermAcademicTerm == analysis.ThisTermAcademicTerm&&a.StudentId==analysis.StudentId);

  ch.ThisTermTotalScoreObtained = analysis.ThisTermTotalScoreObtained;
  ch.ThisTermEntireTotalScore = analysis.ThisTermEntireTotalScore;
  ch.ThisTermAverageScore = analysis.ThisTermAverageScore;
  ch.ThisTermTotalPass = analysis.ThisTermTotalPass;
  ch.ThisTermTotalFailed = analysis.ThisTermTotalFailed;
  ch.StudentId = analysis.StudentId;
  ch.ClassName = analysis.ClassName;
  ch.StudentName = analysis.StudentName;
  ch.ThisTermAcademicTerm = analysis.ThisTermAcademicTerm;
  ch.ThisTermAcademicYear = analysis.ThisTermAcademicYear;

await context.SaveChangesAsync();

}
else{
var cl = context.ReportAnalysis.OrderBy(a=>a.Id).LastOrDefault(a => a.ClassName == analysis.ClassName && a.ThisTermAcademicYear == analysis.ThisTermAcademicYear && a.ThisTermAcademicTerm == analysis.ThisTermAcademicTerm&&a.StudentId==analysis.StudentId);


var ch = context.ReportAnalysis.FirstOrDefault(a => a.ClassName == analysis.ClassName && a.ThisTermAcademicYear == analysis.ThisTermAcademicYear && a.ThisTermAcademicTerm == analysis.ThisTermAcademicTerm&&a.StudentId==analysis.StudentId);


ch.PreviousTermTotalScoreObtained = cl.ThisTermTotalScoreObtained;
  ch.PreviousTermEntireTotalScore = cl.ThisTermEntireTotalScore;
  ch.PreviousTermAverageScore = cl.ThisTermAverageScore;
  ch.PreviousTermTotalPass = cl.ThisTermTotalPass;
  ch.PreviousTermTotalFailed = cl.ThisTermTotalFailed;
  ch.StudentId = cl.StudentId;
  ch.ClassName = cl.ClassName;
  ch.StudentName = cl.StudentName;
  ch.PreviousTermAcademicTerm = cl.ThisTermAcademicTerm;
  ch.PreviousTermAcademicYear = cl.ThisTermAcademicYear;
  ch.PreviousTermPosition = cl.ThisTermPosition;




ch.ThisTermTotalScoreObtained = analysis.ThisTermTotalScoreObtained;
  ch.ThisTermEntireTotalScore = analysis.ThisTermEntireTotalScore;
  ch.ThisTermAverageScore = analysis.ThisTermAverageScore;
  ch.ThisTermTotalPass = analysis.ThisTermTotalPass;
  ch.ThisTermTotalFailed = analysis.ThisTermTotalFailed;
  ch.StudentId = analysis.StudentId;
  ch.ClassName = analysis.ClassName;
  ch.StudentName = analysis.StudentName;
  ch.ThisTermAcademicTerm = analysis.ThisTermAcademicTerm;
  ch.ThisTermAcademicYear = analysis.ThisTermAcademicYear;


await context.SaveChangesAsync();



}


}
else{

context.ReportAnalysis.Add(analysis);
    await context.SaveChangesAsync();


}






var existingAnalysis = context.ReportAnalysis.FirstOrDefault(a =>
        a.ClassName == level && 
        a.ThisTermAcademicYear == year && 
        a.ThisTermAcademicTerm == term && 
        a.StudentId == SID);

    if (existingAnalysis != null) {
        // Update the existing record with the new term data and shift the existing "This" to "Previous"
        existingAnalysis.PreviousTermTotalScoreObtained = existingAnalysis.ThisTermTotalScoreObtained;
        existingAnalysis.PreviousTermEntireTotalScore = existingAnalysis.ThisTermEntireTotalScore;
        existingAnalysis.PreviousTermAverageScore = existingAnalysis.ThisTermAverageScore;
        existingAnalysis.PreviousTermTotalPass = existingAnalysis.ThisTermTotalPass;
        existingAnalysis.PreviousTermTotalFailed = existingAnalysis.ThisTermTotalFailed;

        // Update the "This" with the new term data
        existingAnalysis.ThisTermTotalScoreObtained = TotalScoreOb;
        existingAnalysis.ThisTermEntireTotalScore = EntireScore;
        existingAnalysis.ThisTermAverageScore = TermAverage;
        existingAnalysis.ThisTermTotalPass = TotalPass;
        existingAnalysis.ThisTermTotalFailed = TotalFailed;

        await context.SaveChangesAsync();
    } else {
        // If no existing record, create a new one
        var analy = new ReportAnalysis{
        ThisTermTotalScoreObtained = TotalScoreOb,
        ThisTermEntireTotalScore = EntireScore,
        ThisTermAverageScore = TermAverage,
        ThisTermTotalPass = TotalPass,
        ThisTermTotalFailed = TotalFailed,
        StudentId = s?.StudentId,
        ClassName = level,
        StudentName = s?.FirstName+" " + s?.OtherName+" " + s?.LastName,
        ThisTermAcademicTerm = term,
        ThisTermAcademicYear = year,
        DateAdded = DateTime.Today.Date.ToString("dd MMMM, yyyy")

    };

        context.ReportAnalysis.Add(analy);
        await context.SaveChangesAsync();
    }















var termResults = context.ReportAnalysis
    .Where(a => a.ClassName == existingAnalysis.ClassName && a.ThisTermAcademicYear == existingAnalysis.ThisTermAcademicYear && a.ThisTermAcademicTerm == existingAnalysis.ThisTermAcademicTerm)
    .ToList();

// Group TermResults by Average and order the groups by Average in descending order
var groupedGrades = termResults
    .GroupBy(a => a.ThisTermAverageScore)
    .OrderByDescending(g => g.Key);

int position = 1;

foreach (var group in groupedGrades)
{
    var sortedGroup = group.OrderBy(a => Guid.NewGuid()).ToList(); // Shuffle the group to randomize order
    int samePosition = position;

    foreach (var result in sortedGroup)
    {
        result.ThisTermPosition = GetOrdinal(samePosition);
    }

    position += sortedGroup.Count;
}

await context.SaveChangesAsync();









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

[HttpGet("ViewSingleSubjectGradeStudent")]
public async Task<IActionResult> ViewSingleSubjectGradeStudent(string StudentId, string Year, string Term, string Level, string Subject)
{
    var Grade = context.TermResults.FirstOrDefault(a => a.Level == Level && a.StudentId == StudentId && a.AcademicYear == Year && a.AcademicTerm == Term&&a.Subject==Subject);        
    
    return Ok(Grade);
}




// Method to convert an integer to its ordinal representation


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


[HttpPost("TReportInfo")]
public async Task<IActionResult>TReportInfo([FromBody] TerminalReportsInformation request, string TID, string SID){
 var t = context.Classess.FirstOrDefault(a=>a.TeacherId==TID);
    if (t==null){
        return BadRequest("Teacher not found");
    }
var s = context.Students.FirstOrDefault(a=>a.StudentId==SID);
if (s==null){
    return BadRequest("Student not found");
}
var tr = context.GeneralTReportInfos.FirstOrDefault(a=>a.Id>0);
    if(tr==null){
        return BadRequest("Academic Info Not Found");
    }

var r  = new TerminalReportsInformation{
Attendance = request.Attendance,
OutOf = request.OutOf,
PromotedTo = request.PromotedTo,
Conduct = request.Conduct,
Attitude = request.Attitude,
Interest = request.Interest,
ClassTeacherRemarks = request.ClassTeacherRemarks,
TeacherId = t.TeacherId,
TeacherName = t.ClassTeacher,
StudentId = s.StudentId,
StudentName = s.FirstName+" "+s.OtherName+" "+s.LastName,
AcademicTerm = tr.AcademicTerm,
AcademicYear = tr.AcademicYear,
DateAdded = DateTime.Today.Date.ToString("dd MMMM, yyyy"),
Level = s.Level

};

bool checker = await context.TerminalReportsInformations.AnyAsync(a=>a.StudentId==r.StudentId&&a.AcademicTerm==r.AcademicTerm&&a.AcademicYear==r.AcademicYear);
if(checker){
var a = context.TerminalReportsInformations.FirstOrDefault(a=>a.StudentId==r.StudentId&&a.AcademicTerm==r.AcademicTerm&&a.AcademicYear==r.AcademicYear);
if(a==null){
return BadRequest("Term Report does not exist");
};
a.Level = r.Level;
a.Attendance = r.Attendance;
a.OutOf = r.OutOf;
a.PromotedTo = r.PromotedTo;
a.Conduct = r.Conduct;
a.Attitude = r.Attitude;
a.Interest = r.Interest;
a.ClassTeacherRemarks = r.ClassTeacherRemarks;
a.TeacherId = r.TeacherId;
a.TeacherName = r.TeacherName;
a.StudentId = r.StudentId;
a.StudentName = r.StudentName;
a.AcademicTerm = r.AcademicTerm;
a.AcademicYear = r.AcademicYear;
a.DateAdded = DateTime.Today.Date.ToString("dd MMMM, yyyy");

await context.SaveChangesAsync();

}else{
    context.TerminalReportsInformations.Add(r);
    await context.SaveChangesAsync();
}

await TeacherAuditor(TID, constant.TSubReport);

return Ok("Term Report Sent Successfully");


}

[HttpGet("GetTermReport")]
public async Task<IActionResult> GetTermReport(string Year, string Term, string Level){
    var term = context.TerminalReportsInformations.Where(a=>a.AcademicYear==Year && a.AcademicTerm==Term&&a.Level==Level);
    return Ok(term);
}


[HttpGet("GetTermReportOnReload")]
public async Task<IActionResult> GetTermReportOnReload( string Level, string SID){
    
    var tr = context.GeneralTReportInfos.FirstOrDefault(a=>a.Id>0);
    if(tr==null){
        return BadRequest("Academic Info Not Found");
    }


    var term = context.TerminalReportsInformations.FirstOrDefault(a=>a.AcademicYear==tr.AcademicYear && a.AcademicTerm==tr.AcademicTerm&&a.Level==Level&&a.StudentId==SID);
    if (term == null) {
    // Return an empty array as response
    return Ok(new object[] { });
}
    
    return Ok(term);
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
