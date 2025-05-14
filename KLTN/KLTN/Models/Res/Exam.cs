using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KLTN.Models.Res
{
    public class ExamPaper
    {
        public int ExamPaperCode { get; set; }
        public string SubjectCode { get; set; }
        public string ExamPaperText { get; set; }
        public string CreateByLectuterCode { get; set; }
        public string ApprovedByLectuterCode { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ApprovedDate { get; set; }

        public int ExamTime {  get; set; }
        public List<Question> questions { get; set; }

    }
}