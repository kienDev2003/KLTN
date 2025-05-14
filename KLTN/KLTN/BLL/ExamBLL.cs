using System;
using System.Collections.Generic;
using System.Data;
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

        public List<Models.Req.Exam> GetAllExam(string subjectCode)
        {
            DataTable data = _examDAL.GetAllExam(subjectCode);

            if (data.Rows.Count <= 0) return null;

            List<Models.Req.Exam> exams = new List<Models.Req.Exam>();
            foreach (DataRow row in data.Rows)
            {
                exams.Add(new Models.Req.Exam
                {
                    ExamName = row["ExamPaperText"].ToString(),
                    ExamTime = Convert.ToInt32(row["ExamTime"]),
                    CreateByLectuterCode = row["CreateByLectuterCode"].ToString(),
                    CreatedDate = Convert.ToDateTime(row["CreatedDate"]),
                    ExamCode = Convert.ToInt32(row["ExamPaperCode"]),
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
                examPaper.ExamPaperText = row["ExamPaperText"].ToString();
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
    }
}