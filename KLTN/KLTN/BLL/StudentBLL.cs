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

        public bool InsertStudent(Models.Req.Student student)
        {
            return _studentDAL.InsertStudent(student);
        }

        public List<Models.Req.Student> GetAllStudents()
        {
            DataTable data = _studentDAL.GetAllStudent();

            if (data.Rows.Count <= 0) return null;

            List<Models.Req.Student> students = new List<Models.Req.Student>();
            foreach (DataRow row in data.Rows)
            {
                Models.Req.Student student = new Models.Req.Student();

                student.StudentCode = row["StudentCode"].ToString();
                student.FullName = row["FullName"].ToString();
                student.DateOfBirth = Convert.ToDateTime(row["DateOfBirth"]).ToString("dd/MM/yyyy");
                student.ClassName = row["ClassName"].ToString();
                student.Email = row["Email"].ToString();
                student.MajorName = row["MajorName"].ToString();

                students.Add(student);
            }
            return students;
        }
    }
}