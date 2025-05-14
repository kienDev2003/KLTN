using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KLTN.Models.Req
{
    public class Exam
    {
        public int ExamCode { get; set; }
        public string ExamName { get; set; }
        public int ExamTime { get; set; }
        public string SubjectCode { get; set; }
        public string CreateByLectuterCode { get; set; }
        public string ApprovedByLectuterCode { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ApprovedDate { get; set; }
        public bool IsApproved { get; set; }

        public List<Models.Req.ChapterExamRequest> chapterExams { get; set; }
    }

    public class ChapterExamRequest
    {
        public int ChapterCode { get; set; }
        public int NumberBasic { get; set; }
        public int NumberMedium { get; set; }
        public int NumberHard { get; set; }
    }
}