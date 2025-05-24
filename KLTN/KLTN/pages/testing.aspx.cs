using KLTN.BLL;
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

            bool exec = _examSessionBLL.HandleLoginStudent(student.StudentCode, examSessionCode);

            if (!exec)  Response.Redirect("/");
        }

        [WebMethod]
        public static object GetExamPaperByExamPaperCode(int examPaperCode)
        {
            Models.Res.ExamPaper examPaper = _examBLL.GetExamByExamPaperCode(examPaperCode);

            if (examPaper == null) return new { status = "404", message = "Not Found" };
            else return new { status = "200", examPaper = examPaper };
        }
    }
}