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
                string startDate = examSession.StartExamDate.ToString("dd/MM/yyyy HH:mm:ss");
                string endDate = examSession.EndExamDate.ToString("dd/MM/yyyy HH:mm:ss");
                string lecturerCreateName = GetNameLecturer(examSession.CreateByLecturer);
                string InvigilatorMainName = GetNameLecturer(examSession.InvigilatorMainCode);
                string InvigilatorName = GetNameLecturer(examSession.InvigilatorCode);
                string subjectName = GetNameSubject(examSession.SubjectCode);
                string password = examSession.ExamSessionPassword;

                html += $"<tr class=\"text-center\">" +
                             $"<td class=\"border border-gray-300 px-4 py-2 max-w-[150px] truncate\" title=\"{subjectName}\">{subjectName}</td>" +
                             $"<td class=\"border border-gray-300 px-4 py-2 max-w-[150px] truncate\" title=\"{startDate}\">{startDate}</td>" +
                             $"<td class=\"border border-gray-300 px-4 py-2 max-w-[150px] truncate\" title=\"{endDate}\">{endDate}</td>" +
                             $"<td class=\"border border-gray-300 px-4 py-2 max-w-[150px] truncate\" title=\"{examSession.ExamPaperCode}\">{examSession.ExamPaperCode}</td>" +
                             $"<td class=\"border border-gray-300 px-4 py-2 max-w-[150px] truncate\" title=\"{password}\">{password}</td>" +
                             $"<td class=\"border border-gray-300 px-4 py-2 max-w-[150px] truncate\" title=\"{InvigilatorMainName}\">{InvigilatorMainName}</td>" +
                             $"<td class=\"border border-gray-300 px-4 py-2 max-w-[150px] truncate\" title=\"{InvigilatorName}\">{InvigilatorName}</td>" +
                             $"<td class=\"border border-gray-300 px-4 py-2\">" +
                                 $"<input type=\"button\" class=\"{(DateTime.Now >= examSession.StartExamDate ? "hidden" : "")} cursor-pointer bg-yellow-500 text-white px-2 py-1 rounded\" onclick=\"HandleEditExamSession({examSession.ExamSessionCode})\" value=\"Sửa\">" +
                                 $"<input type=\"button\" class=\"{(DateTime.Now >= examSession.StartExamDate ? "hidden" : "")} cursor-pointer bg-red-500 text-white px-2 py-1 rounded\" onclick=\"HandleDeteleExamSession({examSession.ExamSessionCode})\" value=\"Xóa\">" +
                             $"</td>" +
                         $"</tr>";
            }

            LiteralControl literalControl = new LiteralControl(html);
            examSession_table.Controls.Clear();
            examSession_table.Controls.Add(literalControl);
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

            if(exams == null) return new { status = "404", message = "Not Found" };

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

        [WebMethod]
        public static object HandleUpdateExamSession(Models.Req.ExamSession examSession)
        {
            Models.Res.Login loginSession = HttpContext.Current.Session["login"] as Models.Res.Login;
            Models.Res.Lecturer lecturer = _lecturerBLL.GetInfo(loginSession.accountCode);

            examSession.CreateByLecturer = lecturer.LecturerCode;

            bool exec = _examSessionBLL.UpdateExamSession(examSession);

            if (exec) return new { status = "200", message = "Sua ca thi thành công " };
            else return new { status = "500", message = "Server Error" };
        }

        [WebMethod]
        public static object HandleDeleteExamSession(int examSessionCode)
        {
            bool exec = _examSessionBLL.DeleteExamSession(examSessionCode);

            if (!exec) return new { status = "404", message = "Không tìm thấy ca thi để xóa !" };
            else return new { status = "200", message = "Xóa ca thi thành công !" };
        }

        [WebMethod]
        public static object HandleGetLecturers()
        {
            List<Models.Res.Lecturer> lecturers = _lecturerBLL.GetAllLecturer();

            if (lecturers != null) return new { status = "200", lecturers = lecturers };
            else return new { status = "500", message = "Server error" };
        }

        [WebMethod]
        public static object GetExamSessionByExamSessionCode(int examSessionCode)
        {
            Models.Req.ExamSession examSessionRes = _examSessionBLL.GetExamSession(examSessionCode);

            if (examSessionRes != null) return new { status = "200", examSession = examSessionRes };
            else return new { status = "500", message = "Server error" };
        }
    }
}