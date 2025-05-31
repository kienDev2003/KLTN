using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KLTN.BLL;

namespace KLTN.pages
{
    public partial class exam : System.Web.UI.Page
    {
        private static ExamBLL _examBLL = new ExamBLL();
        private static LecturerBLL _lecturerBLL = new LecturerBLL();
        private static SubjectBLL _subjectBLL = new SubjectBLL();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["login"] == null) return;

            HandleGetAllExam();
        }

        private void HandleGetAllExam()
        {
            List<Models.Req.Exam> exams = _examBLL.GetAllExam();

            if (exams == null)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showWarring", "showWarring('Chưa có đề thi !');", true);
                return;
            }

            string html = "";
            foreach (var exam in exams)
            {
                html += $"<tr class=\"text-center\">" +
                            $"<td class=\"border border-gray-300 px-4 py-2 max-w-[250px] truncate\" title=\"{GetSubjectName(exam.SubjectCode)}\">{GetSubjectName(exam.SubjectCode)}</td>" +
                            $"<td class=\"border border-gray-300 px-4 py-2 max-w-[250px] truncate\" title=\"{exam.ExamCode}\">{exam.ExamCode}</td>" +
                            $"<td class=\"border border-gray-300 px-4 py-2\">{exam.ExamTime} phút</td>" +
                            $"<td class=\"border border-gray-300 px-4 py-2\">{GetNameLecturer(exam.CreateByLectuterCode)}</td>" +
                            $"<td class=\"border border-gray-300 px-4 py-2\">{exam.CreatedDate}</td>" +
                            $"<td class=\"border border-gray-300 px-4 py-2\">{(exam.IsApproved ? "Đã duyệt" : "Chưa duyệt")}</td>" +
                            $"<td class=\"border border-gray-300 px-4 py-2\">" +
                                $"<input type=\"button\" class=\"cursor-pointer bg-blue-500 text-white px-2 py-1 rounded\" onclick=\"HandleViewExam({exam.ExamCode})\" value=\"Xem chi tiết\">" +
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

        private string GetSubjectName(string subjectCode)
        {
            Models.Res.Subject subject = _subjectBLL.GetInfoBySubjectCode(subjectCode);

            return subject.SubjectName;
        }
    }
}