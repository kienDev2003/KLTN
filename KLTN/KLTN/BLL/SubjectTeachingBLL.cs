﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using KLTN.DAL;

namespace KLTN.BLL
{
    public class SubjectTeachingBLL
    {
        private SubjectTeachingDAL _subjectTeachingDAL;

        public SubjectTeachingBLL()
        {
            _subjectTeachingDAL = new SubjectTeachingDAL();
        }

        public List<Models.Res.Subject> GetSubjectTeaching(int accountCode)
        {
            DataTable data = _subjectTeachingDAL.GetSubjectTeaching(accountCode);

            if (data.Rows.Count <= 0) return null;

            List<Models.Res.Subject> subjects = new List<Models.Res.Subject>();
            foreach(DataRow row in data.Rows)
            {
                subjects.Add(new Models.Res.Subject()
                {
                    SubjectCode = row["SubjectCode"].ToString(),
                    SubjectName = row["SubjectName"].ToString()
                });
            }
            return subjects;
        }

        public List<Models.Res.Subject> GetSubjectTeachingByLecturerCode(string lecturerCode)
        {
            DataTable data = _subjectTeachingDAL.GetSubjectTeachingByLecturerCode(lecturerCode);

            if (data.Rows.Count <= 0) return null;

            List<Models.Res.Subject> subjects = new List<Models.Res.Subject>();
            foreach (DataRow row in data.Rows)
            {
                subjects.Add(new Models.Res.Subject()
                {
                    SubjectCode = row["SubjectCode"].ToString()
                });
            }
            return subjects;
        }

        public bool InsertSubjectTeaching(Models.Req.SubjectTeaching subjectTeaching)
        {
            return _subjectTeachingDAL.InsertSubjectTeaching(subjectTeaching);
        }

        public bool DeteleSubjectTeaching(Models.Req.SubjectTeaching subjectTeaching)
        {
            return _subjectTeachingDAL.DeleteSubjectTeaching(subjectTeaching);
        }
    }
}