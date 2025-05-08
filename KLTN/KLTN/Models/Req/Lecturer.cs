using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KLTN.Models.Req
{
    public class Lecturer
    {
        public string LecturerCode { get; set; }
        public string FullName { get; set; }
        public string DateOfBirth { get; set; }
        public string DepartmentName { get; set; }
        public string Email {  get; set; }
    }
}