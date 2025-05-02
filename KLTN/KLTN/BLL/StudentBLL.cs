using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using KLTN.DAL;

namespace KLTN.BLL
{
    public class StudentBLL
    {
        private StudentDAL _studentDAL;

        public StudentBLL()
        {
            _studentDAL = new StudentDAL();
        }

        public Models.Res.Student GetInfo(int accountCode)
        {
            DataTable data = _studentDAL.GetInfo(accountCode);

            if (data.Rows.Count <= 0) return null;

            Models.Res.Student student = new Models.Res.Student();
            foreach(DataRow row in data.Rows)
            {
                student.StudentCode = row["StudentCode"].ToString();
                student.FullName = row["FullName"].ToString();
                student.DateOfBirth = Convert.ToDateTime(row["DateOfBirth"]);
                student.ClassName = row["ClassName"].ToString();
                student.MajorName = row["MajorName"].ToString();
            }
            return student;
        }
    }
}