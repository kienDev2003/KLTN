using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using KLTN.DAL;
using KLTN.Models.Res;

namespace KLTN.BLL
{
    public class QuestionBLL
    {
        private QuestionDAL _questionDAL;

        public QuestionBLL()
        {
            _questionDAL = new QuestionDAL();
        }

        public List<Models.Res.Question> GetQuestionBySubjectTeaching(int accountCode,string subjectCode, int pageIndex)
        {
            DataTable data = _questionDAL.GetQuestionBySubjectTeaching(accountCode, subjectCode, pageIndex);

            if (data.Rows.Count <= 0) return null;

            List<Models.Res.Question> questions = new List<Models.Res.Question>();
            foreach(DataRow row in data.Rows)
            {
                questions.Add(new Models.Res.Question()
                {
                    QuestionCode = Convert.ToInt32(row["QuestionCode"]),
                    QuestionText = row["QuestionText"].ToString(),
                    QuestionLevel = row["QuestionLevel"].ToString(),
                    QuestionType = row["QuestionType"].ToString(),
                    IsApproved = Convert.ToBoolean(row["IsApproved"]),
                    CreateDate = Convert.ToDateTime(row["CreateDate"])
                });
            }
            return questions;
        }

        public List<Models.Res.Question> GetQuestionBySubjectLecturer(int accountCode, string subjectCode, int pageIndex)
        {
            DataTable data = _questionDAL.GetQuestionBySubjectLecturer(accountCode, subjectCode, pageIndex);

            if (data.Rows.Count <= 0) return null;

            List<Models.Res.Question> questions = new List<Models.Res.Question>();
            foreach (DataRow row in data.Rows)
            {
                questions.Add(new Models.Res.Question()
                {
                    QuestionCode = Convert.ToInt32(row["QuestionCode"]),
                    QuestionText = row["QuestionText"].ToString(),
                    QuestionLevel = row["QuestionLevel"].ToString(),
                    QuestionType = row["QuestionType"].ToString(),
                    IsApproved = Convert.ToBoolean(row["IsApproved"]),
                    CreateDate = Convert.ToDateTime(row["CreateDate"]),
                    LecturerCode = row["LecturerCode"].ToString()
                });
            }
            return questions;
        }

        public bool DeleteQuestion(int questionCode)
        {
            return _questionDAL.DeleteQuestionByQuestionCode(questionCode);
        }

        public bool UpdateQuestion(Models.Req.Question questionRequest)
        {
            return _questionDAL.UpdateQuestion(questionRequest);
        }

        public int GetNumberQuestionBySubjectTeaching(int accountCode, string subjectCode)
        {
            return _questionDAL.GetNumberQuestionBySubjectTeaching(accountCode, subjectCode);
        }

        public int GetNumberQuestionBySubjectLecturer(int accountCode, string subjectCode)
        {
            return _questionDAL.GetNumberQuestionBySubjectLecturer(accountCode, subjectCode);
        }

        public bool InsertQuestion(Models.Req.Question questionRequest)
        {
            return _questionDAL.InsertQuestion(questionRequest);
        }

        public Models.Res.Question GetQuestionByQuestionCode(int questionCode)
        {
            DataTable dataQuestion = _questionDAL.GetQuestionByQuestionCode(questionCode);

            if (dataQuestion.Rows.Count <= 0) return null;

            Models.Res.Question question = new Models.Res.Question();
            foreach (DataRow row in dataQuestion.Rows)
            {
                question.QuestionCode = Convert.ToInt32(row["QuestionCode"]);
                question.QuestionText = row["QuestionText"].ToString();
                question.QuestionLevel = row["QuestionLevel"].ToString();
                question.QuestionType = row["QuestionType"].ToString();
                question.IsApproved = Convert.ToBoolean(row["IsApproved"]);
                question.CreateDate = Convert.ToDateTime(row["CreateDate"]);
            }

            DataTable dataAnswer = _questionDAL.GetAnswerByQuestionCode(questionCode);

            if (dataAnswer.Rows.Count <= 0) return null;

            List<Models.Res.Answer> answers = new List<Models.Res.Answer>();
            foreach (DataRow row in dataAnswer.Rows)
            {
                answers.Add(new Models.Res.Answer()
                {
                    AnswerCode = Convert.ToInt32(row["AnswerCode"]),
                    AnswerText = row["AnswerText"].ToString(),
                    AnswerTrue = Convert.ToBoolean(row["AnswerTrue"]),
                    QuestionCode = Convert.ToInt32(row["QuestionCode"])
                });
            }

            question.Answers = answers;

            return question;
        }

        public bool ApprovalQuestion(int questionCode)
        {
            return _questionDAL.ApprovalQuestion(questionCode);
        }
    }
}