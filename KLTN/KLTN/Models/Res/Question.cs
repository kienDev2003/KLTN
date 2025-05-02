using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KLTN.Models.Res
{
    public class Question
    {
        public int QuestionCode { get; set; }
        public string QuestionText { get; set; }
        public string QuestionLevel { get; set; }
        public string QuestionType { get; set; }
        public string SubjectCode { get; set; }
        public string LecturerCode { get; set; }
        public bool IsApproved { get; set; }
        public DateTime CreateDate { get; set; }
        public List<Answer> Answers { get; set; }
    }

    public class Answer
    {
        public int AnswerCode { get; set; }
        public string AnswerText { get; set; }
        public bool AnswerTrue { get; set; }
        public int QuestionCode { get; set; }
    }
}