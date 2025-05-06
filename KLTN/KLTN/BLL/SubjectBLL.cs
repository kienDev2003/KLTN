using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using KLTN.DAL;

namespace KLTN.BLL
{
    public class SubjectBLL
    {
        private SubjectDAL _subjectDAL;
        public SubjectBLL()
        {
            _subjectDAL = new SubjectDAL();
        }

        public Models.Res.Subject GetInfoBySubjectCode(string subjectCode)
        {
            DataTable data = _subjectDAL.GetInfoBySubjectCode(subjectCode);

            if (data.Rows.Count <= 0) return null;

            Models.Res.Subject subject = new Models.Res.Subject();
            foreach (DataRow row in data.Rows)
            {
                subject.SubjectCode = row["SubjectCode"].ToString();
                subject.SubjectName = row["SubjectName"].ToString();
                subject.NumberOfCredits = Convert.ToInt32(row["NumberOfCredits"]);
            }
            return subject;
        }

        public List<Models.Res.Subject> GetSubjectAll()
        {
            DataTable data = _subjectDAL.GetSubjectAll();

            if (data.Rows.Count <= 0) return null;

            List<Models.Res.Subject> subjects = new List<Models.Res.Subject>();
            foreach (DataRow row in data.Rows)
            {
                subjects.Add(new Models.Res.Subject()
                {
                    SubjectCode = row["SubjectCode"].ToString(),
                    SubjectName = row["SubjectName"].ToString(),
                    NumberChapter = Convert.ToInt32(row["NumberChapter"]),
                    CreateDate = Convert.ToDateTime(row["CreateDate"]),
                    NumberOfCredits = Convert.ToInt32(row["NumberOfCredits"])
                });
            }
            return subjects;
        }

        public Models.Res.Subject GetDetailSubject(string subjectCode)
        {
            DataTable dataSubject = _subjectDAL.GetInfoBySubjectCode(subjectCode);

            if (dataSubject.Rows.Count <= 0) return null;

            Models.Res.Subject subject = new Models.Res.Subject();
            foreach (DataRow row in dataSubject.Rows)
            {
                subject.SubjectCode = row["SubjectCode"].ToString();
                subject.SubjectName = row["SubjectName"].ToString();
                subject.NumberOfCredits = Convert.ToInt32(row["NumberOfCredits"]);
            }

            DataTable dataChapter = _subjectDAL.GetChapterBySubjectCode(subjectCode);

            if (dataChapter.Rows.Count <= 0) return subject;

            List<Models.Res.Chapter> chapters = new List<Models.Res.Chapter>();
            foreach(DataRow chapter in dataChapter.Rows)
            {
                chapters.Add(new Models.Res.Chapter()
                {
                    ChapterCode = Convert.ToInt32(chapter["ChapterCode"]),
                    ChapterName = chapter["ChapterName"].ToString(),
                });
            }

            subject.Chapters = chapters;

            return subject;
        }

        public bool InsertSubject(Models.Req.Subject subject)
        {
            subject.CreateDate = DateTime.Now;

            return _subjectDAL.InsertSubject(subject);
        }

        public bool DeleteSubject(string subjectCode)
        {
            return _subjectDAL.DeleteSubject(subjectCode);
        }
    }
}