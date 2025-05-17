using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using KLTN.DAL;

namespace KLTN.BLL
{
    public class ExamSessionBLL
    {
        private ExamSessionDAL _examSessionDAL;
        public ExamSessionBLL()
        {
            _examSessionDAL = new ExamSessionDAL();
        }

        public bool InsertExamSession(Models.Req.ExamSession examSession)
        {
            return _examSessionDAL.InsertExamSession(examSession);
        }

        public List<Models.Req.ExamSession> GetAllExamSession()
        {
            DataTable data = _examSessionDAL.GetAllExamSession();

            if (data.Rows.Count <= 0) return null;

            List<Models.Req.ExamSession> examSessions = new List<Models.Req.ExamSession> ();
            foreach(DataRow row in data.Rows)
            {
                examSessions.Add(new Models.Req.ExamSession()
                {
                    ExamSessionCode = Convert.ToInt32(row["ExamSessionCode"]),
                    SubjectCode = row["SubjectCode"].ToString(),
                    StartExamDate = Convert.ToDateTime(row["StartExamDate"]),
                    EndExamDate = Convert.ToDateTime(row["EndExamDate"]),
                    ExamPaperCode = Convert.ToInt32(row["ExamPaperCode"]),
                    CreateByLecturer = row["CreateByLecturer"].ToString(),
                    ExamSessionPassword = row["ExamSessionPassword"].ToString(),
                    IsAssessment = Convert.ToBoolean(row["IsAssessment"])
                });
            }

            return examSessions;
        }
    }
}