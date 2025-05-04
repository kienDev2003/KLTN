using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KLTN.Models.Res
{
    public class Subject
    {
        public string SubjectCode { get; set; }
        public string SubjectName { get; set; }
        public int NumberOfCredits { get; set; }
        public int NumberChapter {  get; set; }
        public DateTime CreateDate { get; set; }

        public List<Chapter> Chapters { get; set; }
    }

    public class Chapter
    {
        public int ChapterCode { get; set; }
        public string ChapterName { get; set; }
        public string SubjectCode { get; set; }
    }
}