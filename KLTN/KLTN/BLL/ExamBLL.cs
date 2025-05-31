using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Printing;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using KLTN.DAL;

namespace KLTN.BLL
{
    public class ExamBLL
    {
        private ExamDAL _examDAL;
        private QuestionBLL _questionBLL;

        public ExamBLL()
        {
            _examDAL = new ExamDAL();
            _questionBLL = new QuestionBLL();
        }

        public bool CheckNumberQuestion(string subjectCode, int chapterCode, string questionLevel, int numberQuestion)
        {
            return _examDAL.CheckNumberQuestion(subjectCode, chapterCode, questionLevel, numberQuestion);
        }

        public bool InsertExam(Models.Req.Exam exam)
        {
            return _examDAL.InsertExam(exam);
        }

        public List<Models.Req.Exam> GetAllExamBySubjectCode(string subjectCode)
        {
            DataTable data = _examDAL.GetAllExamBySubjectCode(subjectCode);

            if (data.Rows.Count <= 0) return null;

            List<Models.Req.Exam> exams = new List<Models.Req.Exam>();
            foreach (DataRow row in data.Rows)
            {
                exams.Add(new Models.Req.Exam
                {
                    ExamTime = Convert.ToInt32(row["ExamTime"]),
                    CreateByLectuterCode = row["CreateByLectuterCode"].ToString(),
                    CreatedDate = Convert.ToDateTime(row["CreatedDate"]),
                    ExamCode = Convert.ToInt32(row["ExamPaperCode"]),
                    ApprovedByLectuterCode = row["ApprovedByLectuterCode"].ToString(),
                    IsApproved = Convert.ToBoolean(row["IsApproved"])
                });
            }

            return exams;
        }

        public List<Models.Req.Exam> GetAllExam()
        {
            DataTable data = _examDAL.GetAllExam();

            if (data.Rows.Count <= 0) return null;

            List<Models.Req.Exam> exams = new List<Models.Req.Exam>();
            foreach (DataRow row in data.Rows)
            {
                exams.Add(new Models.Req.Exam
                {
                    ExamTime = Convert.ToInt32(row["ExamTime"]),
                    CreateByLectuterCode = row["CreateByLectuterCode"].ToString(),
                    CreatedDate = Convert.ToDateTime(row["CreatedDate"]),
                    ExamCode = Convert.ToInt32(row["ExamPaperCode"]),
                    SubjectCode = row["SubjectCode"].ToString(),
                    IsApproved = Convert.ToBoolean(row["IsApproved"])
                });
            }

            return exams;
        }

        public Models.Res.ExamPaper GetExamByExamPaperCode(int examPaperCode)
        {
            DataTable dataExam = _examDAL.GetExamPaper(examPaperCode);

            if (dataExam.Rows.Count <= 0) return null;

            Models.Res.ExamPaper examPaper = new Models.Res.ExamPaper();
            foreach (DataRow row in dataExam.Rows)
            {
                examPaper.ExamTime = Convert.ToInt32(row["ExamTime"]);
                examPaper.CreateByLectuterCode = row["CreateByLectuterCode"].ToString();
                examPaper.CreatedDate = Convert.ToDateTime(row["CreatedDate"]);
                examPaper.ExamPaperCode = Convert.ToInt32(row["ExamPaperCode"]);
            }

            DataTable dataQuestion = _examDAL.GetListQuestionByExamPaperCode(examPaperCode);

            if (dataQuestion.Rows.Count <= 0) return null;

            List<Models.Res.Question> questions = new List<Models.Res.Question>();
            foreach (DataRow row in dataQuestion.Rows)
            {
                questions.Add(_questionBLL.GetQuestionByQuestionCode(Convert.ToInt32(row["QuestionCode"])));
            }

            examPaper.questions = questions;


            return examPaper;
        }

        public bool DeleteExam(int examPaperCode)
        {
            return _examDAL.DeleteExam(examPaperCode);
        }

        public bool ExamPaperApproved(int examPaperCode, string lecturerCodeApproved)
        {
            return _examDAL.ExamPaperApproved(examPaperCode, lecturerCodeApproved);
        }

        public int GetNumberQuestion(int examPaperCode)
        {
            return _examDAL.GetNumberQuestion(examPaperCode);
        }

        public bool InsertExamSubmitted(Models.Req.ExamSubmitted examSubmit, float score, string studentCode,int numberQuestionTotal,int numberQuestionTrue)
        {
            return _examDAL.InsertExamSubmitted(examSubmit, score, studentCode, numberQuestionTotal, numberQuestionTrue);
        }

        public Models.Res.Exam_Result GetExamResultForStudent(int examSessionCode, string studentCode)
        {
            DataTable data = _examDAL.GetExamResultForStudent(examSessionCode, studentCode);

            if (data.Rows.Count <= 0) return null;

            Models.Res.Exam_Result exam_Result = new Models.Res.Exam_Result();
            foreach(DataRow row in data.Rows)
            {
                exam_Result.ExamPaperName = row["ExamPaperText"].ToString();
                exam_Result.TotalQuestions = Convert.ToInt32(row["NumberQuestionTotal"]);
                exam_Result.CorrectAnswers = Convert.ToInt32(row["NumberQuestionTrue"]);
                exam_Result.Score = Math.Round(Convert.ToDouble(row["Score"]), 2);
                exam_Result.Note = row["Note"].ToString();
            }

            return exam_Result;
        }

        public bool InsertWarringHiddenWindow(int examSessionCode, string studentCode)
        {
            return _examDAL.InsertWarringHiddenWindow(examSessionCode, studentCode);
        }

        public List<Models.Res.ExamSessionWarring> GetAllExamSessionWarring(int examSessionCode)
        {
            DataTable data = _examDAL.GetAllExamSessionWarring(examSessionCode);

            if (data.Rows.Count <= 0) return null;

            List<Models.Res.ExamSessionWarring> examSessionWarrings = new List<Models.Res.ExamSessionWarring>();
            foreach(DataRow row in data.Rows)
            {
                examSessionWarrings.Add(new Models.Res.ExamSessionWarring()
                {
                    StudentCode = row["StudentCode"].ToString(),
                    DateWarring = Convert.ToDateTime(row["DateWarring"]),
                    ExamSessionCode = Convert.ToInt32(row["ExamSessionCode"])
                });
            }

            return examSessionWarrings;
        }

        public bool CheckedWarring(string studentCode, int examSessionCode)
        {
            return _examDAL.CheckedWarring(examSessionCode,studentCode);
        }

        public Models.Res.SubmissionRequirements CheckSubmissionRequirements(int examSessionCode, string studentCode)
        {
            DataTable data = _examDAL.CheckSubmissionRequirements(studentCode,examSessionCode);

            if (data.Rows.Count <= 0) return null;

            Models.Res.SubmissionRequirements submissionRequirements = new Models.Res.SubmissionRequirements();
            foreach(DataRow row in data.Rows)
            {
                submissionRequirements.Status = Convert.ToBoolean(row["SubmissionRequirements"]);
                submissionRequirements.NoteSubmissionRequirements = Convert.ToString(row["NoteSubmissionRequirements"]);
            }

            return submissionRequirements;
        }
    }
}