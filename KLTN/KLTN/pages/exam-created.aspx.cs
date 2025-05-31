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
    public partial class exam_created : System.Web.UI.Page
    {
        private static ExamBLL _examBLL = new ExamBLL();
        private static LecturerBLL _lecturerBLL = new LecturerBLL();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["login"] == null) return;

            string subjectCode = Request.QueryString["subjectCode"].ToString();
            HandleGetExam(subjectCode);
        }

        private void HandleGetExam(string subjectCode)
        {
            List<Models.Req.Exam> exams = _examBLL.GetAllExamBySubjectCode(subjectCode);

            if (exams == null)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showWarring", "showWarring('Bạn chưa tạo đề thi nào cho môn học này !');", true);
                return;
            }

            string html = "";
            foreach (var exam in exams)
            {
                html += $"<tr class=\"text-center\">" +
                            $"<td class=\"border border-gray-300 px-4 py-2 max-w-[250px] truncate\" title=\"{exam.ExamCode}\">{exam.ExamCode}</td>" +
                            $"<td class=\"border border-gray-300 px-4 py-2\">{exam.ExamTime}</td>" +
                            $"<td class=\"border border-gray-300 px-4 py-2\">{(exam.IsApproved ? "Đã duyệt" : "Chưa duyệt")}</td>" +
                            $"<td class=\"border border-gray-300 px-4 py-2\">{(exam.IsApproved ? GetNameLecturer(exam.ApprovedByLectuterCode) : "-")}</td>" +
                            $"<td class=\"border border-gray-300 px-4 py-2\">" +
                                $"<input type=\"button\" class=\"cursor-pointer bg-blue-500 text-white px-2 py-1 rounded\" onclick=\"HandleViewExam({exam.ExamCode})\" value=\"Xem\">"+
                                $"<input type=\"button\" class=\"{(exam.IsApproved ? "hidden" : "")} cursor-pointer bg-red-500 text-white px-2 py-1 rounded\" onclick=\"HandleDeleteExam({exam.ExamCode})\" value=\"Xóa\">"+
                            $"</td>" +
                        $"</tr>";
            }

            LiteralControl literalControl = new LiteralControl(html);
            examPaper_table.Controls.Clear();
            examPaper_table.Controls.Add(literalControl);
        }

        private string GetNameLecturer(string lecturerCode)
        {
            Models.Res.Lecturer lecturer = _lecturerBLL.GetInfoByLecturerCode(lecturerCode);

            return lecturer.FullName;
        }

        [WebMethod]
        public static object InsertExam(Models.Req.Exam exam)
        {
            foreach (var chapter in exam.chapterExams)
            {
                bool numberBasic = _examBLL.CheckNumberQuestion(exam.SubjectCode, chapter.ChapterCode, "basic", chapter.NumberBasic);
                bool numberMedium = _examBLL.CheckNumberQuestion(exam.SubjectCode, chapter.ChapterCode, "medium", chapter.NumberMedium);
                bool numberHard = _examBLL.CheckNumberQuestion(exam.SubjectCode, chapter.ChapterCode, "hard", chapter.NumberHard);

                if (!numberBasic || !numberMedium || !numberHard)
                {
                    return new
                    {
                        status = "500",
                        message = "Không đủ câu hỏi để tạo đề thi !"
                    };
                }
            }

            exam.CreatedDate = DateTime.Now;

            bool exec = _examBLL.InsertExam(exam);

            if (exec) return new { status = "200", message = "Tạo đề thi thành công" };
            else return new { status = "500", message = "Server error" };
        }

        [WebMethod]
        public static object GetExamPaper(int examPaperCode)
        {
            var exam = _examBLL.GetExamByExamPaperCode(examPaperCode);

            return new { exam = exam };
        }

        [WebMethod]
        public static object DeleteExam(int examPaperCode)
        {
            bool exec = _examBLL.DeleteExam(examPaperCode);

            if (exec) return new { status = "200", message = "Xóa đề thi thành công !" };
            else return new { status = "500", message = "Server error" };
        }

        [WebMethod]
        public static object ExamPaperApproved(int examPaperCode)
        {
            Models.Res.Login loginSession = HttpContext.Current.Session["login"] as Models.Res.Login;
            Models.Res.Lecturer lecturer = _lecturerBLL.GetInfo(loginSession.accountCode);

            bool exec = _examBLL.ExamPaperApproved(examPaperCode, lecturer.LecturerCode);

            if (exec) return new { status = "200", message = "Duyệt đề thi thành công" };
            else return new { status = "500", message = "Server Error" };
        }
    }
}