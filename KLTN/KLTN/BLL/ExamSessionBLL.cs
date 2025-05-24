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

        public bool UpdateExamSession(Models.Req.ExamSession examSession)
        {
            return _examSessionDAL.UpdateExamSession(examSession);
        }

        public bool DeleteExamSession(int examSessionCode)
        {
            return _examSessionDAL.DeleteExamSession(examSessionCode);
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
                    InvigilatorMainCode = row["InvigilatorMainCode"].ToString(),
                    InvigilatorCode = row["InvigilatorCode"].ToString()
                });
            }

            return examSessions;
        }

        public List<Models.Req.ExamSession> GetAllExamSessionByInvigilatorCode(string invigilatorCode)
        {
            DataTable data = _examSessionDAL.GetAllExamSessionByInvigilatorCode(invigilatorCode);

            if (data.Rows.Count <= 0) return null;

            List<Models.Req.ExamSession> examSessions = new List<Models.Req.ExamSession>();
            foreach (DataRow row in data.Rows)
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
                    InvigilatorMainCode = row["InvigilatorMainCode"].ToString(),
                    InvigilatorCode = row["InvigilatorCode"].ToString()
                });
            }

            return examSessions;
        }

        public List<Models.Req.ExamSession> GetAllExamSessionByStudentCode(string studentCode)
        {
            DataTable data = _examSessionDAL.GetAllExamSessionByStudentCode(studentCode);

            if (data.Rows.Count <= 0) return null;

            List<Models.Req.ExamSession> examSessions = new List<Models.Req.ExamSession>();
            foreach (DataRow row in data.Rows)
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
                    InvigilatorMainCode = row["InvigilatorMainCode"].ToString(),
                    InvigilatorCode = row["InvigilatorCode"].ToString()
                });
            }

            return examSessions;
        }

        public Models.Req.ExamSession GetExamSession(int examSessionCode)
        {
            DataTable data = _examSessionDAL.GetExamSession(examSessionCode);

            if (data.Rows.Count <= 0) return null;

            Models.Req.ExamSession examSession = new Models.Req.ExamSession();
            foreach (DataRow row in data.Rows)
            {

                examSession.ExamSessionCode = Convert.ToInt32(row["ExamSessionCode"]);
                examSession.SubjectCode = row["SubjectCode"].ToString();
                examSession.StartExamDate = Convert.ToDateTime(row["StartExamDate"]);
                examSession.EndExamDate = Convert.ToDateTime(row["EndExamDate"]);
                examSession.ExamPaperCode = Convert.ToInt32(row["ExamPaperCode"]);
                examSession.CreateByLecturer = row["CreateByLecturer"].ToString();
                examSession.ExamSessionPassword = row["ExamSessionPassword"].ToString();
                examSession.InvigilatorMainCode = row["InvigilatorMainCode"].ToString();
                examSession.InvigilatorCode = row["InvigilatorCode"].ToString();
            }

            return examSession;
        }

        public bool InsertStudentInExamSession_Student(string studentCode, int examSessionCode)
        {
            return _examSessionDAL.InsertStudentInExamSession_Student(studentCode, examSessionCode);
        }

        public List<Models.Res.ExamSession_Student> GetStudentByExamSessionCode(int examSessionCode)
        {
            DataTable data = _examSessionDAL.GetStudentByExamSessionCode(examSessionCode);

            if (data.Rows.Count <= 0) return null;

            List<Models.Res.ExamSession_Student> examSession_Students = new List<Models.Res.ExamSession_Student>();
            foreach(DataRow row in data.Rows)
            {
                examSession_Students.Add(new Models.Res.ExamSession_Student()
                {
                    StudentCode = row["StudentCode"].ToString(),
                    ExamSessionCode = Convert.ToInt32(row["ExamSessionCode"]),
                    StudentHaveEntered = Convert.ToBoolean(row["StudentHaveEntered"]),
                    SubmissionRequirements = Convert.ToBoolean(row["SubmissionRequirements"])
                });
            }

            return examSession_Students;
        }

        public bool HandleLogoutStudent(string studentCode, int examSessionCode)
        {
            return _examSessionDAL.HandleLogoutStudent(examSessionCode, studentCode);
        }
        public bool HandleLoginStudent(string studentCode, int examSessionCode)
        {
            return _examSessionDAL.HandleLoginStudent(examSessionCode, studentCode);
        }

        public bool HandleCheckStudentHaveEntered(string studentCode, int examSessionCode)
        {
            return _examSessionDAL.HandleCheckStudentHaveEntered(examSessionCode,studentCode);
        }

        public bool HandleSubmissionRequirements(string  studentCode, int examSessionCode)
        {
            return _examSessionDAL.HandleSubmissionRequirements(examSessionCode, studentCode);
        }
    }
}