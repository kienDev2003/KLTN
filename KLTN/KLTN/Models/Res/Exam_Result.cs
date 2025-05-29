using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KLTN.Models.Res
{
    public class Exam_Result
    {
        public string ExamPaperName { get; set; }
        public int CorrectAnswers { get; set; }
        public int TotalQuestions { get; set; }
        public double Score { get; set; }
        public string Note { get; set; }
        public string StudentCode { get; set; }
        public int ExamPaperCode { get; set; }
    }
}