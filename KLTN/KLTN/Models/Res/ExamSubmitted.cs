using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KLTN.Models.Res
{
    public class ExamSubmitted
    {
        public int ExamSessionCode { get; set; }
        public int ExamPaperCode { get; set; }
        public string StudentCode { get; set; }
        public string SubmittedDate { get; set; }
        public double Score { get; set; }
        public string Note { get; set; }
        public string SubjectName { get; set; }
        public string StudentName { get; set; }
        public string StudentDateOfBrith { get; set; }
        public string StudentClassName { get; set; }
    }
}