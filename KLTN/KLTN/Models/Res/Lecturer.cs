using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KLTN.Models.Res
{
    public class Lecturer
    {
        public string LecturerCode { get; set; }
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email {  get; set; }
        public bool IsLeader { get; set; }
        public string DepartmentName { get; set; }
    }
}