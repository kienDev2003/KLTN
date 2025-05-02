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
        public string DepartmentName { get; set; }
    }
}