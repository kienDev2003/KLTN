using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KLTN.BLL;

namespace KLTN.pages
{
    public partial class subject_teaching : System.Web.UI.Page
    {
        private SubjectTeachingBLL _subjectTeachingBLL = new SubjectTeachingBLL();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["login"] == null) return;
             
            HandleGetSubjectTeaching();
        }

        private void HandleGetSubjectTeaching()
        {
            Models.Res.Login login = Session["login"] as Models.Res.Login;

            List<Models.Res.Subject> subjects = _subjectTeachingBLL.GetSubjectTeaching(login.accountCode);

            if (subjects == null)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showWarring", "showWarring('Bạn chưa được phân công giảng dạy môn học nào !');", true);
                return;
            }

            string html = "";
            foreach (var subject in subjects)
            {
                html += $"<a href=\"/pages/question-created.aspx?subjectCode={subject.SubjectCode}\">" +
                            $"<div class=\"bg-white rounded-lg p-5 shadow-md h-full flex items-center\">" +
                                $"<div class=\"w-16 h-16 mr-4 flex-shrink-0 flex items-center justify-center\">" +
                                    $"<img src=\"/pages/public/images/education_image.png\" class=\"max-w-full max-h-full\">" +
                                $"</div>" +
                                $"<h3 class=\"text-secondary text-base font-medium\">{subject.SubjectName}</h3>" +
                            $"</div>" +
                        $"</a>";
            }

            LiteralControl literalControl = new LiteralControl(html);
            list_subject_teaching.Controls.Clear();
            list_subject_teaching.Controls.Add(literalControl);

        }
    }
}