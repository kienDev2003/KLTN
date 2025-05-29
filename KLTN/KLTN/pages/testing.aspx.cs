using KLTN.BLL;
using KLTN.Models.Res;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KLTN.pages
{
    public partial class testing : System.Web.UI.Page
    {
        private static ExamBLL _examBLL = new ExamBLL();
        private static ExamSessionBLL _examSessionBLL = new ExamSessionBLL();
        private static StudentBLL _studentBLL = new StudentBLL();
        private static QuestionBLL _questionBLL = new QuestionBLL();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["login"] == null) Response.Redirect("/");

            LoginStudent();
        }

        private void LoginStudent()
        {
            int examSessionCode = Convert.ToInt32(Request.QueryString["examSessionCode"]);
            Models.Res.Login loginSession = Session["login"] as Models.Res.Login;
            Models.Res.Student student = _studentBLL.GetInfo(loginSession.accountCode);

            student_name.InnerText = student.FullName;

            bool exec = _examSessionBLL.HandleLoginStudent(student.StudentCode, examSessionCode);

            if (!exec) Response.Redirect("/");
        }

        [WebMethod]
        public static object GetExamPaperByExamPaperCode(int examPaperCode)
        {
            Models.Res.ExamPaper examPaper = _examBLL.GetExamByExamPaperCode(examPaperCode);

            if (examPaper == null) return new { status = "404", message = "Not Found" };
            else return new { status = "200", examPaper = examPaper };
        }

        [WebMethod]
        public static object ExamSubmit(Models.Req.ExamSubmitted examSubmit)
        {
            int numberQuestionTotal = _examBLL.GetNumberQuestion(examSubmit.ExamPaperCode);
            int numberQuestionTrue = 0;

            foreach (var question in examSubmit.Questions)
            {
                bool statusQuestionTrue = true;

                foreach (var answer in question.answers)
                {
                    bool statusAnswer = _questionBLL.CheckAnswerTrue(answer.AnswerCode);

                    if (statusAnswer == false) statusQuestionTrue = false;
                }

                if (statusQuestionTrue == true) numberQuestionTrue++;
            }

            float score = (float)Math.Round((double)numberQuestionTrue / numberQuestionTotal * 10, 2);

            Models.Res.Login loginSession = HttpContext.Current.Session["login"] as Models.Res.Login;
            Models.Res.Student student = _studentBLL.GetInfo(loginSession.accountCode);

            bool statusInsertExamSubmit = _examBLL.InsertExamSubmitted(examSubmit, score, student.StudentCode, numberQuestionTotal, numberQuestionTrue);

            if (statusInsertExamSubmit) return new { status = "200" };
            else return new { status = "500", messgae = "Server Error" };
        }

        [WebMethod]
        public static object InsertWarringHiddenWindow(int examSessionCode)
        {
            Models.Res.Login loginSession = HttpContext.Current.Session["login"] as Models.Res.Login;
            Models.Res.Student student = _studentBLL.GetInfo(loginSession.accountCode);

            bool exec = _examBLL.InsertWarringHiddenWindow(examSessionCode, student.StudentCode);

            if (!exec) return new { status = "500", message = "Server Insert Warring Hidden Window Error" };
            else return new { status = "200" };
        }

        [WebMethod]
        public static object CheckSubmissionRequirements(int examSessionCode)
        {
            Models.Res.Login loginSession = HttpContext.Current.Session["login"] as Models.Res.Login;
            Models.Res.Student student = _studentBLL.GetInfo(loginSession.accountCode);

            Models.Res.SubmissionRequirements submissionRequirements = _examBLL.CheckSubmissionRequirements(examSessionCode, student.StudentCode);

            if(submissionRequirements != null || submissionRequirements.Status != false)
            {
                return new { status = submissionRequirements.Status, note = submissionRequirements.NoteSubmissionRequirements };
            }

            return new { status = false };
        }
    }
}