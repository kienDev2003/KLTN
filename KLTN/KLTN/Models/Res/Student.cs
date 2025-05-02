using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KLTN.Models.Res
{
    public class Student
    {
        public string StudentCode { get; set; }
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string ClassName { get; set; }
        public string MajorName { get; set; }
    }
}