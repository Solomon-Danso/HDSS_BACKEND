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
public float TotalScore {get; set;}
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


public class TerminalReportsInformation{
    public int Id{get; set;}
    public int Attendance {get; set;}
    public int OutOf {get; set;}
    public string? PromotedTo {get; set;}
    public string? Conduct {get; set;}
    public string? Attitude {get; set;}
    public string? Interest {get; set;}
    public string? ClassTeacherRemarks {get; set;}
    public string? TeacherId {get; set;}
    public string? TeacherName {get; set;}
    public string? StudentId {get; set;}
    public string? StudentName {get; set;}
    public string? Level {get;set;}
    public string? AcademicYear {get; set;}
    public string? AcademicTerm {get; set;}
    public string? DateAdded {get; set;}
}

public class ReportAnalysis{
    public int Id{get;set;}
    public string? ThisTermPosition {get;set;}
    public float ThisTermTotalScoreObtained {get;set;}
    public float ThisTermEntireTotalScore {get;set;}
    public float ThisTermAverageScore{get;set;}
    public int ThisTermTotalPass {get; set;}
    public int ThisTermTotalFailed {get;set;}
    public string? ThisTermAcademicTerm {get; set;}
    public string? ThisTermAcademicYear {get; set;}


    public string? PreviousTermPosition {get;set;}
    public float PreviousTermTotalScoreObtained {get;set;}
    public float PreviousTermEntireTotalScore {get;set;}
    public float PreviousTermAverageScore{get;set;}
    public int PreviousTermTotalPass {get; set;}
    public int PreviousTermTotalFailed {get;set;}
    public string? PreviousTermAcademicTerm {get; set;}
    public string? PreviousTermAcademicYear {get; set;}

    public string? StudentId {get;set;}
    public string? StudentName {get;set;}

}



}