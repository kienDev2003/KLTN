using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using KLTN.DAL;

namespace KLTN.BLL
{
    public class LecturerBLL
    {
        private LecturerDAL _lecturerDAL;

        public LecturerBLL()
        {
            _lecturerDAL = new LecturerDAL();
        }
        public Models.Res.Lecturer GetInfo(int accountCode)
        {
            DataTable data = _lecturerDAL.GetInfo(accountCode);

            if (data.Rows.Count <= 0) return null;

            Models.Res.Lecturer lecturer = new Models.Res.Lecturer();
            foreach (DataRow row in data.Rows)
            {
                lecturer.LecturerCode = row["LecturerCode"].ToString();
                lecturer.FullName = row["FullName"].ToString();
                lecturer.DateOfBirth = Convert.ToDateTime(row["DateOfBirth"]);
                lecturer.IsLeader = Convert.ToBoolean(row["IsLeader"]);
                lecturer.DepartmentName = row["DepartmentName"].ToString();
            }
            return lecturer;
        }

        public Models.Res.Lecturer GetInfoByLecturerCode(string lecturerCode)
        {
            DataTable data = _lecturerDAL.GetInfoByLecturerCode(lecturerCode);

            if (data.Rows.Count <= 0) return null;

            Models.Res.Lecturer lecturer = new Models.Res.Lecturer();
            foreach (DataRow row in data.Rows)
            {
                lecturer.FullName = row["FullName"].ToString();
                lecturer.DateOfBirth = Convert.ToDateTime(row["DateOfBirth"]);
                lecturer.IsLeader = Convert.ToBoolean(row["IsLeader"]);
                lecturer.DepartmentName = row["DepartmentName"].ToString();
            }
            return lecturer;
        }

        public List<Models.Res.Lecturer> GetAllLecturer()
        {
            DataTable data = _lecturerDAL.GetAllLecturer();

            if (data.Rows.Count <= 0) return null;

            List<Models.Res.Lecturer> lecturers = new List<Models.Res.Lecturer>();
            foreach(DataRow row in data.Rows)
            {
                lecturers.Add(new Models.Res.Lecturer()
                {
                    LecturerCode = row["LecturerCode"].ToString(),
                    DateOfBirth = Convert.ToDateTime(row["DateOfBirth"]),
                    Email = row["Email"].ToString(),
                    DepartmentName = row["DepartmentName"].ToString(),
                    FullName = row["FullName"].ToString(),
                    IsLeader = Convert.ToBoolean(row["IsLeader"])
                });
            }

            return lecturers;
        }

        public bool InsertLecturer(Models.Req.Lecturer lecturer)
        {
            return _lecturerDAL.InsertLecturer(lecturer);
        }

        public bool ChangeLeader(string lecturerCode, int status)
        {
            return _lecturerDAL.ChangeLeader(lecturerCode, status);
        }
    }
}