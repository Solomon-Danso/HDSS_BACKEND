using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HDSS_BACKEND.Models
{
public class TermResult{
public int Id { get; set; }
public string? StudentId {get;set;}
public string? StudentName {get; set;}
public float ClassScore {get; set;}
public float ExamScore {get; set;}
public string? Position {get; set;}
public string? Grade {get; set;}
public string? Comment {get; set;}
public float Average{get;set;}

public string? Level {get; set;}
public string? Subject { get; set; }
public string? AcademicYear {get; set;}
public string? AcademicTerm{get; set;}
public string? TeacherId {get; set;}
public string? TeacherName{get; set;}
public string? DateUploaded {get; set;}
public DateTime? SpecificDateAndTime {get;set;}

}


}