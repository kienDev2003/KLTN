using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KLTN.DAL;

namespace KLTN.BLL
{
    public class ExamBLL
    {
        private ExamDAL _examDAL;

        public ExamBLL()
        {
            _examDAL = new ExamDAL();
        }

        public bool CheckNumberQuestion(string subjectCode, int chapterCode, string questionLevel, int numberQuestion)
        {
            return _examDAL.CheckNumberQuestion(subjectCode, chapterCode, questionLevel, numberQuestion);
        }

        public bool InsertExam(Models.Req.Exam exam)
        {
            return _examDAL.InsertExam(exam);
        }
    }
}