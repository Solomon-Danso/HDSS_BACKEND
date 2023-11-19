using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HDSS_BACKEND.Models
{
    public class TestnQuizTeacher
    {
        public int Id { get; set; }
        public string? QuizId { get; set; }
        public string? Subject {get; set;}
        public string? Level {get; set;}
        public string? Question {get; set;}
        public string? OptionA {get; set;}
        public string? OptionB {get; set;}
        public string? OptionC {get; set;}
        public string? OptionD {get; set;}
        public string? OptionE {get; set;}
        public string? Answer {get; set;}
        public bool? IsAnswered {get; set;} 
   
        public DateTime? Deadline {get; set;}
        public int? Duration {get; set;}
        public string? TeacherId {get; set;}
        public string? TeacherName {get; set;}
        public string? ProfilePic {get; set;}
        public float DesignatedMarks {get; set;}
        public DateTime? UploadDate {get;set;}

    }

    public class TestnQuizStudent
    {
        public int Id { get; set; }
         public int QuestionId { get; set; }
        public string? QuizId { get; set; }
        public string? Subject {get; set;}
        public string? Level {get; set;}
        public string? Question {get; set;}
        public string? OptionA {get; set;}
        public string? OptionB {get; set;}
        public string? OptionC {get; set;}
        public string? OptionD {get; set;}
        public string? OptionE {get; set;}
        public string? Answer {get; set;}
        public float DesignatedMarks {get; set;}
        public bool? IsAnswered {get; set;} 
        public bool? IsStarted {get; set;} 
        public DateTime? Deadline {get; set;}
        public int? Duration {get; set;}
        public string? StudentId {get; set;}
        public string? StudentName {get; set;}
        public string? ProfilePic {get; set;}
        public DateTime? UploadDate {get;set;}

    }

    public class TestnQuizStudentMark{
        public int Id {get; set;}
        public int QuestionId {get; set;}
        public string? QuizId {get; set;}
        public string? StudentAnswer {get; set;}
        public float Mark {get; set;}
        public string? StudentId {get; set;}
        public DateTime? SolutionDate {get;set;}


    }

    public class TestnQuizStudentTotalScore{
        public int Id { get; set; }
        public float MarksObtained { get; set; }
        public float TotalScore { get; set; }
        public string? StudentId {get;set;}
        public string? StudentName {get;set;}
        public string? ProfilePic {get;set;}
        public string? QuizId {get;set;}
        public string? SubjectName{get;set;}
        public string? Level{get;set;}
    }

public class QuizTimer{
    public int Id {get; set;}
    public string? QuizId {get; set;}
    public string? StudentId {get; set;}
    public int? TimeLeft{get; set;}
}




}