using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KLTN.Models.Req
{
    public class ExamSubmitted
    {
        public int ExamSessionCode { get; set; }
        public int ExamPaperCode { get; set; } 
        public string Note { get; set; }
        public List<Models.Req.Question> Questions { get; set; }
        
        public string QuestionSubmitJson { get; set; }
    }
}