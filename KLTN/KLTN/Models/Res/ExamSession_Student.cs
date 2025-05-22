using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KLTN.Models.Res
{
    public class ExamSession_Student
    {
        public int ExamSessionCode { get; set; }
        public string StudentCode { get; set; }
        public bool StudentHaveEntered { get; set; }
        public bool SubmissionRequirements { get; set; }

    }
}