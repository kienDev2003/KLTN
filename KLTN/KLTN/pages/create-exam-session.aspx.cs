using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using KLTN.BLL;

namespace KLTN.pages
{
    public partial class create_exam_session : System.Web.UI.Page
    {
        private static SubjectBLL _subjectBLL = new SubjectBLL();
        private static ExamBLL _examBLL = new ExamBLL();
        private static ExamSessionBLL _examSessionBLL = new ExamSessionBLL();
        private static LecturerBLL _lecturerBLL = new LecturerBLL();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["login"] == null) return;

            HandleGetListExamSession();
        }

        private void HandleGetListExamSession()
        {
            List<Models.Req.ExamSession> examSessions = _examSessionBLL.GetAllExamSession();

            if (examSessions == null) return;

            string html = "";
            foreach (var examSession in examSessions)
            {
                html += $"<tr class=\"text-center\">" +
                             $"<td class=\"border border-gray-300 px-4 py-2 max-w-[250px] truncate\" title=\"{EscapeHTML(question.QuestionText)}\">{EscapeHTML(question.QuestionText)}</td>" +
                             $"<td class=\"border border-gray-300 px-4 py-2\">{HandleFormatQuestionType(question.QuestionType)}</td>" +
                             $"<td class=\"border border-gray-300 px-4 py-2\">{HandleFormatQuestionLevel(question.QuestionLevel)}</td>" +
                             $"<td class=\"border border-gray-300 px-4 py-2\">{question.CreateDate}</td>" +
                             $"<td class=\"border border-gray-300 px-4 py-2\">{HandleFormatQuestionIsApproved(question.IsApproved)}</td>" +
                             $"<td class=\"border border-gray-300 px-4 py-2\">" +
                                 $"<input type=\"button\" class=\"cursor-pointer bg-blue-500 text-white px-2 py-1 rounded\" onclick=\"HandleViewQuestion({question.QuestionCode})\" value=\"Xem\">" +
                                 $"<input type=\"button\" class=\"{(question.IsApproved ? "hidden" : "")} cursor-pointer bg-yellow-500 text-white px-2 py-1 rounded\" onclick=\"HandleEditQuestion({question.QuestionCode})\" value=\"Sửa\">" +
                                 $"<input type=\"button\" class=\"{(question.IsApproved ? "hidden" : "")} cursor-pointer bg-red-500 text-white px-2 py-1 rounded\" onclick=\"HandleDeteleQuestion({question.QuestionCode})\" value=\"Xóa\">" +
                             $"</td>" +
                         $"</tr>";
            }
        }

        private string GetNameSubject(string subjectCode)
        {
            Models.Res.Subject subject = _subjectBLL.GetInfoBySubjectCode(subjectCode);

            return subject.SubjectName;
        }

        private string GetNameLecturer(string lecturerCode)
        {
            Models.Res.Lecturer lecturer = _lecturerBLL.GetInfoByLecturerCode(lecturerCode);

            return lecturer.FullName;
        }

        [WebMethod]
        public static object HandleGetSubjects()
        {
            List<Models.Res.Subject> subjects = _subjectBLL.GetSubjectAll();

            if (subjects != null) return new { status = "200", subjects = subjects };
            else return new { status = "500", message = "Server error" };
        }

        [WebMethod]
        public static object HandleGetExamBySubject(string subjectCode)
        {
            List<Models.Req.Exam> exams = _examBLL.GetAllExamBySubjectCode(subjectCode);

            for (int i = exams.Count - 1; i >= 0; i--)
            {
                if (exams[i].IsApproved == false)
                {
                    exams.RemoveAt(i);
                }
            }

            if (exams != null) return new { status = "200", exams = exams };
            else return new { status = "500", message = "Server error" };
        }

        [WebMethod]
        public static object HandleInsertExamSession(Models.Req.ExamSession examSession)
        {
            Models.Res.Login loginSession = HttpContext.Current.Session["login"] as Models.Res.Login;
            Models.Res.Lecturer lecturer = _lecturerBLL.GetInfo(loginSession.accountCode);

            examSession.CreateByLecturer = lecturer.LecturerCode;

            bool exec = _examSessionBLL.InsertExamSession(examSession);

            if (exec) return new { status = "200", message = "Thêm ca thi thành công " };
            else return new { status = "500", message = "Server Error" };
        }
    }
}