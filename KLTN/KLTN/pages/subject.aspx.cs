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
    public partial class subject : System.Web.UI.Page
    {
        private static SubjectBLL _subjectBLL = new SubjectBLL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["login"] == null) return;

            HandleGetSubjectList();
        }

        private void HandleGetSubjectList()
        {
            List<Models.Res.Subject> subjects = _subjectBLL.GetSubjectAll();

            if(subjects == null)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showWarring", "showWarring('Chưa có môn học nào !');", true);
                return;
            }

            string html = "";
            foreach (var subject in subjects)
            {
                html += $"<tr class=\"text-center\">" +
                            $"<td class=\"border border-gray-300 px-4 py-2 max-w-[250px] truncate\" title=\"{subject.SubjectName}\">{subject.SubjectName}</td>" +
                            $"<td class=\"border border-gray-300 px-4 py-2\">{subject.NumberOfCredits}</td>" +
                            $"<td class=\"border border-gray-300 px-4 py-2\">{subject.NumberChapter}</td>" +
                            $"<td class=\"border border-gray-300 px-4 py-2\">{subject.CreateDate}</td>" +
                            $"<td class=\"border border-gray-300 px-4 py-2\">" +
                                $"<input type=\"button\" class=\"cursor-pointer bg-blue-500 text-white px-2 py-1 rounded\" onclick=\"HandleViewSubject('{subject.SubjectCode}')\" value=\"Xem\">" +
                                $"<input type=\"button\" class=\"cursor-pointer bg-yellow-500 text-white px-2 py-1 rounded\" onclick=\"HandleEditSubject('{subject.SubjectCode}')\" value=\"Sửa\">" +
                                $"<input type=\"button\" class=\"cursor-pointer bg-red-500 text-white px-2 py-1 rounded\" onclick=\"HandleDeteleSubject('{subject.SubjectCode}')\" value=\"Xóa\">" +
                            $"</td>" +
                        $"</tr>";
            }

            LiteralControl literalControl = new LiteralControl(html);
            subject_table.Controls.Clear();
            subject_table.Controls.Add(literalControl);
        }

        [WebMethod]
        public static object HandleInsertSubject(Models.Req.Subject subjectRequest)
        {
            bool exec = _subjectBLL.InsertSubject(subjectRequest);

            if (exec) return new { status = "200", message = "Thêm môn học thành công !" };
            else return new { status = "500", message = "Server error" };
        }

        [WebMethod]
        public static object HandleViewSubject(string subjectCode)
        {
            Models.Res.Subject subject = _subjectBLL.GetDetailSubject(subjectCode);

            if (subject == null) return new { status = "404", message = "Không tìm thấy môn học !" };
            else return new { status = "200", subject = subject };
        }
    }
}