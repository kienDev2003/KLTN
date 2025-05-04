using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using KLTN.DAL;

namespace KLTN.BLL
{
    public class SubjectLecturerBLL
    {
        private SubjectLecturerDAL _subjectLecturerDAL;

        public SubjectLecturerBLL()
        {
            _subjectLecturerDAL = new SubjectLecturerDAL();
        }

        public List<Models.Res.Subject> GetSubjectLecturer(int accountCode)
        {
            DataTable data = _subjectLecturerDAL.GetSubjectLecturer(accountCode);

            if (data.Rows.Count <= 0) return null;

            List<Models.Res.Subject> subjects = new List<Models.Res.Subject>();
            foreach (DataRow row in data.Rows)
            {
                subjects.Add(new Models.Res.Subject()
                {
                    SubjectCode = row["SubjectCode"].ToString(),
                    SubjectName = row["SubjectName"].ToString()
                });
            }
            return subjects;
        }
    }
}