using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KLTN.Models.Req
{
    public class ExamSession
    {
        public int ExamSessionCode { get; set; }
        public string SubjectCode { get; set; }
        public DateTime StartExamDate { get; set; }
        public DateTime EndExamDate { get; set; }
        public int ExamPaperCode { get; set; }
        public string CreateByLecturer { get; set; }
        public string ExamSessionPassword { get; set; }
        public bool IsAssessment {  get; set; }
    }
}