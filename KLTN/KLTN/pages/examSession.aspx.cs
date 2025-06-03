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
    public partial class examSession : System.Web.UI.Page
    {
        private static ExamSessionBLL _examSessionBLL = new ExamSessionBLL();
        private static SubjectBLL _subjectBLL = new SubjectBLL();
        private static ExamBLL _examBLL = new ExamBLL();
        private static LecturerBLL _lecturerBLL = new LecturerBLL();
        private static StudentBLL _studentBLL = new StudentBLL();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["login"] == null) Response.Redirect("/");

            GetInfoExam();
        }

        private void GetInfoExam()
        {
            int examSessionCode = Convert.ToInt32(Request.QueryString["examSessionCode"]);
            Models.Req.ExamSession examSession = _examSessionBLL.GetExamSession(examSessionCode);

            string html = $"<div class=\"exam-field\">" +
                            $"<label>Mã ca thi: {examSession.ExamSessionCode}</label>" +
                          $"</div>" +
                          $"<div class=\"exam-field\">" +
                            $"<label>Đề thi: {GetNameExamPaper(examSession.ExamPaperCode)}</label>" +
                          $"</div>" +
                          $"<div class=\"exam-field\">" +
                            $"<label>Môn thi: {GetNameSubject(examSession.SubjectCode)}</label>" +
                          $"</div>" +
                          $"<div class=\"exam-field\">" +
                            $"<label>Giờ thi: {examSession.StartExamDate.ToString("HH:mm dd/MM/yyyy")} - {examSession.EndExamDate.ToString("HH:mm dd/MM/yyyy")}</label>" +
                          $"</div>" +
                          $"<div class=\"exam-field\">" +
                            $"<label>Giảng viên trông thi chính: {GetNameLecturer(examSession.InvigilatorMainCode)}</label>" +
                          $"</div>" +
                          $"<div class=\"exam-field\">" +
                            $"<label>Giảng viên trông thi phụ: {GetNameLecturer(examSession.InvigilatorCode)}</label>" +
                          $"</div>" +
                          $"<div class=\"exam-field\">" +
                            $"<label>Mật khẩu ca thi: {examSession.ExamSessionPassword}</label>" +
                          $"</div>";

            LiteralControl literalControl = new LiteralControl(html);
            exam_info.Controls.Clear();
            exam_info.Controls.Add(literalControl);

            if(examSession.EndExamDate <= DateTime.Now)
            {
                var classAttrExportTestScores = ExportTestScores.Attributes["class"] ?? "";
                var classes = classAttrExportTestScores.Split(' ').ToList();
                classes.Remove("hidden");
                ExportTestScores.Attributes["class"] = string.Join(" ", classes);

                var classAttrAddStudent = AddStudent.Attributes["class"] ?? "";
                var classesAddStudent = classAttrAddStudent.Split(' ').ToList();
                classesAddStudent.Add("hidden");
                AddStudent.Attributes["class"] = string.Join(" ", classesAddStudent);
                AddStudentAuto.Attributes["class"] = string.Join(" ", classesAddStudent);
                MoreTime.Attributes["class"] = string.Join(" ", classesAddStudent);
            }

        }

        private static string GetNameExamPaper(int examPaperCode)
        {
            Models.Res.ExamPaper examPaper = _examBLL.GetExamByExamPaperCode(examPaperCode);

            return examPaper.ExamPaperText;
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
        public static object InsertStudentInExamSession_Student(string studentCode, int examSessionCode)
        {
            bool exec = _examSessionBLL.InsertStudentInExamSession_Student(studentCode, examSessionCode);

            if (exec) return new { status = "200", message = "Thêm sinh viên vào ca thi thành công" };
            else return new { status = "500", message = "Server Error" };
        }

        [WebMethod]
        public static object GetListStudent(int examSessionCode)
        {
            List<Models.Res.ExamSession_Student> examSession_Students = _examSessionBLL.GetStudentByExamSessionCode(examSessionCode);

            if (examSession_Students != null) return new { status = "200", examSession_Students = examSession_Students };
            else return new { status = "404", message = "Not Found" };
        }

        [WebMethod]
        public static object GetInfoStudentByStudentCode(string studentCode)
        {
            Models.Res.Student student = _studentBLL.GetStudentByStudentCode(studentCode);

            if (student != null) return new { status = "200", student = student };
            else return new { status = "404", message = "Not Found" };
        }

        [WebMethod]
        public static object HandleLogoutStudent(string studentCode, int examSessionCode)
        {
            bool exec = _examSessionBLL.HandleLogoutStudent(studentCode, examSessionCode);

            if (exec) return new { status = "200", message = "Cấp quyền vào lại thành công" };
            else return new { status = "404", message = "Not Found" };
        }

        [WebMethod]
        public static object HandleSubmissionRequirements(string studentCode, int examSessionCode, string noteSubmissionRequirements)
        {
            bool exec = _examSessionBLL.HandleSubmissionRequirements(studentCode, examSessionCode, noteSubmissionRequirements);

            if (exec) return new { status = "200", message = "Yêu cầu nộp bài thành công" };
            else return new { status = "404", message = "Not Found" };
        }

        [WebMethod]
        public static object HandleGetExamSessionWarring(int examSessionCode)
        {
            List<Models.Res.ExamSessionWarring> examSessionWarrings = _examBLL.GetAllExamSessionWarring(examSessionCode);

            if (examSessionWarrings == null) return new { status = "404", message = "Not Found" };
            else return new { status = "200", examSessionWarrings = examSessionWarrings };
        }

        [WebMethod]
        public static object HandleCheckedWarring(string studentCode, int examSessionCode)
        {
            bool exec = _examBLL.CheckedWarring(studentCode, examSessionCode);

            if (exec) return new { status = "200" };
            else return new { status = "500", message = "Server Update Status Warring Error" };
        }

        [WebMethod]
        public static object HandleExportListScroes(int examSessionCode)
        {
            List<Models.Res.ExamSubmitted> examSubmitteds = _examSessionBLL.HandleGetExportListScroesByExamSessionCode(examSessionCode);

            if (examSubmitteds == null) return new { status = "404", message = "Not found" };
            else return new { status = "200", examSubmitteds = examSubmitteds };
        }
    }
}