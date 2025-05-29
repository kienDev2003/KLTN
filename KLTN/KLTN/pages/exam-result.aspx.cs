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
    public partial class exam_result : System.Web.UI.Page
    {
        private static StudentBLL _studentBLL = new StudentBLL();
        private static ExamBLL _examBLL = new ExamBLL();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["login"] == null) Response.Redirect("/");
        }

        [WebMethod]
        public static object GetInfoExamResult(int examSessionCode)
        {
            Models.Res.Login loginSession = HttpContext.Current.Session["login"] as Models.Res.Login;
            Models.Res.Student student = _studentBLL.GetInfo(loginSession.accountCode);

            Models.Res.Exam_Result exam_Result = _examBLL.GetExamResultForStudent(examSessionCode, student.StudentCode);

            if (exam_Result != null) return new { status = "200", exam_Result = exam_Result };
            else return new { status = "404", message = "Not Found" };
        }
    }
}