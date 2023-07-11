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
    [Route("api/Grading")]
    public class GradingController : ControllerBase
    {
        private readonly DataContext context;
        public GradingController(DataContext CTX){
            context = CTX;
        }

[HttpPost("uploadClassScores")]
        public async Task<IActionResult> UploadClassScores (string TeacherId, string SubjectN, string ClassN,string StudentId,string Year,string Term, string assignmentNumber, [FromBody]ClassScore request){
            var checker = SubjectN+TeacherId+ClassN;
        bool NoPower = await context.TeacherForSubjects.AnyAsync(p=>p.TeacherCode==checker);
         if(!NoPower){
            return BadRequest("You dont have permission to upload this class scores");
         }

         var checker2 = SubjectN+StudentId+ClassN;
        bool NoPower2 = await context.StudentForSubjects.AnyAsync(p=>p.StudentCode==checker2);
        var student = context.Students.FirstOrDefault(s=>s.StudentId == StudentId);
        if(student==null){
            return BadRequest("Student with id "+StudentId+" does not exist in the school");
        }

         if(!NoPower){
            return BadRequest($"{student.Title}, {student.FirstName} {student.LastName}  has not registered for this course ");
         }
          var assignmentToken = SubjectN+ClassN+ Year + Term + assignmentNumber;
 
         var q = context.Assignments.FirstOrDefault(a=>a.AssignmentToken == assignmentToken);
         if (q == null){
            return BadRequest("No Assignment Found");
         }
        
        var e = new ClassScore{
          StudentId = student.StudentId,
          StudentName = $"{student.Title}, {student.FirstName} {student.LastName} ",
          SubjectName = q.SubjectName,
          Assignmentnumber = q.AssignmentNumber,
          Score= request.Score,
          TeacherId = q.TeacherId,
          TeacherName = q.TeacherName,
          AcademicTerm = q.AcademicTerm,
          AcademicYear= q.AcademicYear,
          ClassName = q.ClassName,
          Token = student.StudentId+ $"{student.Title}, {student.FirstName} {student.LastName} "+ q.ClassName+q.SubjectName + q.AssignmentNumber+q.AcademicTerm+q.AcademicYear,
          DateScored =  DateTime.Today.Date.ToString("dd MMMM, yyyy"),
        };

        var Token = e.StudentId+e.StudentName+e.ClassName+e.SubjectName+e.Assignmentnumber+e.AcademicTerm+e.AcademicYear;
        bool scoreChecker = await context.ClassScores.AnyAsync(c=> c.Token==Token);
        if (scoreChecker){
            return BadRequest("Score already entered");
        }


        var cSummary = new ClassScoreSummary{
           StudentId = student.StudentId,
           StudentName = $"{student.Title}, {student.FirstName} {student.LastName} ",
           ClassName = q.ClassName,
            SubjectName = q.SubjectName,
            AcademicTerm = q.AcademicTerm,
            AcademicYear= q.AcademicYear,
            TotalScore = request.Score,
            Token = student.StudentId+q.ClassName+q.SubjectName+q.AcademicTerm+q.AcademicYear
          };

           var TheToken = cSummary.StudentId+cSummary.ClassName+cSummary.SubjectName+cSummary.AcademicTerm+cSummary.AcademicYear;
          
          bool scores = await context.ClassScoresSummarys.AnyAsync(c=>c.Token==TheToken);
          if (scores){
            var c = context.ClassScoresSummarys.FirstOrDefault(a=> a.Token==TheToken);
            if(c==null){
                 return BadRequest(" Not Working");
            }
            c.TotalScore = c.TotalScore + request.Score;
           
        
          }
          else{
            context.ClassScoresSummarys.Add(cSummary);

          }




        context.ClassScores.Add(e);
        await context.SaveChangesAsync();



            return Ok($"{e.StudentName} got {e.Score} in {e.SubjectName} {e.Assignmentnumber}");
        }
        
    }
}