using KLTN.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KLTN.pages
{
    public partial class exam_schedule : System.Web.UI.Page
    {
        private static LecturerBLL _lecturerBLL = new LecturerBLL();
        private static ExamSessionBLL _examSessionBLL = new ExamSessionBLL();
        private static SubjectBLL _subjectBLL = new SubjectBLL();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["login"] == null) return;

            HandleGetExamSessionByInvigilatorCode();
        }

        private void HandleGetExamSessionByInvigilatorCode()
        {
            Models.Res.Login loginSession = Session["login"] as Models.Res.Login;
            Models.Res.Lecturer lecturer = _lecturerBLL.GetInfo(loginSession.accountCode);

            List<Models.Req.ExamSession> examSessions = _examSessionBLL.GetAllExamSessionByInvigilatorCode(lecturer.LecturerCode);

            if (examSessions == null)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showWarring", "showWarring('Bạn chưa được phân công trông thi ca thi nào !');", true);
                return;
            }

            string html = "";
            foreach (var examSession in examSessions)
            {
                string subjectName = GetNameSubject(examSession.SubjectCode);
                string InvigilatorName = GetNameLecturer(examSession.InvigilatorCode);
                string InvigilatorMainName = GetNameLecturer(examSession.InvigilatorMainCode);

                html += $"<a href=\"/pages/examSession.aspx?examSessionCode={examSession.ExamSessionCode}\">" +
                            $"<div class=\"flex flex-col bg-white rounded-lg p-5 shadow-md h-full flex items-center\">" +
                                $"<h3 class=\"mb-2 text-secondary text-base font-medium\">Môn: {subjectName}</h3>" +
                                $"<h3 class=\"mb-2 text-secondary text-base font-medium\">Bắt đầu: {examSession.StartExamDate.ToString("HH:mm dd/MM/yyyy")}</h3>" +
                                $"<h3 class=\"mb-2 text-secondary text-base font-medium\">Kết thúc: {examSession.EndExamDate.ToString("HH:mm dd/MM/yyyy")}</h3>" +
                                $"<h3 class=\"mb-2 text-secondary text-base font-medium\">GV chính: {InvigilatorMainName}</h3>" +
                                $"<h3 class=\"mb-2 text-secondary text-base font-medium\">GV phụ: {InvigilatorName}</h3>" +
                            $"</div>" +
                        $"</a>";
            }

            LiteralControl literalControl = new LiteralControl(html);
            list_examSession.Controls.Clear();
            list_examSession.Controls.Add(literalControl);
        }

        private static string GetNameSubject(string subjectCode)
        {
            Models.Res.Subject subject = _subjectBLL.GetInfoBySubjectCode(subjectCode);

            return subject.SubjectName;
        }

        private static string GetNameLecturer(string lecturerCode)
        {
            Models.Res.Lecturer lecturer = _lecturerBLL.GetInfoByLecturerCode(lecturerCode);

            return lecturer.FullName;
        }
    }
}