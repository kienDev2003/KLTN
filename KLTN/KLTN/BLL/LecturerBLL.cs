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
    }
}