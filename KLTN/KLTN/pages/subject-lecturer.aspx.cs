using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KLTN.BLL;

namespace KLTN.pages
{
    public partial class subject_lecturer : System.Web.UI.Page
    {
        private SubjectLecturerBLL _subjectLecturerBLL = new SubjectLecturerBLL();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["login"] == null) return;

            HandleGetSubjectLecturer();
        }

        private void HandleGetSubjectLecturer()
        {
            Models.Res.Login login = Session["login"] as Models.Res.Login;

            List<Models.Res.Subject> subjects = _subjectLecturerBLL.GetSubjectLecturer(login.accountCode);

            if (subjects == null)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showWarring", "showWarring('Bạn chưa được phân công phụ trách môn học nào !');", true);
                return;
            }

            string html = "";
            foreach (var subject in subjects)
            {
                html += $"<a href=\"/pages/assessment.aspx?subjectCode={subject.SubjectCode}\">" +
                            $"<div class=\"bg-white rounded-lg p-5 shadow-md h-full flex items-center\">" +
                                $"<div class=\"w-16 h-16 mr-4 flex-shrink-0 flex items-center justify-center\">" +
                                    $"<img src=\"./assets/images/user.jpg\" class=\"max-w-full max-h-full\">" +
                                $"</div>" +
                                $"<h3 class=\"text-secondary text-base font-medium\">{subject.SubjectName}</h3>" +
                            $"</div>" +
                        $"</a>";
            }

            LiteralControl literalControl = new LiteralControl(html);
            list_subject_lecturer.Controls.Clear();
            list_subject_lecturer.Controls.Add(literalControl);
        }
    }
}