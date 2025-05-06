using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KLTN.Models.Res;

namespace KLTN.Models.Req
{
    public class Question
    {
        public int questionCode {  get; set; }
        public string questionText {  get; set; }
        public string questionType { get; set; }
        public string questionLevel { get; set; }
        public string subjectCode { get; set; }
        public string lecturerCode { get; set; }
        public int ChapterCode { get; set; }
        public DateTime createDate { get; set; }
        public List<Answer> answers { get; set; }
    }
}