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
    public partial class assignment_lecterer : System.Web.UI.Page
    {
        private static SubjectLecturerBLL _subjectLecturerBLL = new SubjectLecturerBLL();
        private static LecturerBLL _lecturerBLL = new LecturerBLL();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["login"] == null) return;

            HandleGetLecturer();
            HandleGetSubjectNotLecturer();
        }

        private void HandleGetLecturer()
        {
            List<Models.Res.Lecturer> lecturers = _lecturerBLL.GetAllLecturer();

            if (lecturers == null) return;

            string html = "";
            foreach (var lecturer in lecturers)
            {
                html += $"<div onclick=\"HandleSelectLecturer(this,'{lecturer.LecturerCode}')\" class=\"btn-lecturer border rounded-lg overflow-hidden shadow-sm\">" +
                            $"<div class=\"p-3 border-b flex justify-between items-center\">" +
                                $"<div class=\"flex items-center\">" +
                                    $"<img src=\"/pages/public/images/user_image.png\" alt=\"subject\" class=\"w-8 h-8 mr-3\">" +
                                    $"<p class=\"font-medium\">{lecturer.LecturerCode}-{lecturer.FullName}</p>" +
                                $"</div>" +
                            $"</div>" +
                        $"</div>";
            }

            LiteralControl literalControlLecturer = new LiteralControl(html);
            lecturer_list.Controls.Clear();
            lecturer_list.Controls.Add(literalControlLecturer);
        }

        private void HandleGetSubjectNotLecturer()
        {
            List<Models.Res.Subject> subjects = _subjectLecturerBLL.GetSubjectNotLecturer();

            if (subjects == null) return;

            string html = "";
            foreach(var subject in subjects)
            {
                html += $"<div class=\"border rounded-lg overflow-hidden shadow-sm\">" +
                            $"<div class=\"p-3 border-b flex justify-between items-center\">" +
                                $"<div class=\"flex items-center\">" +
                                    $"<img src=\"/pages/public/images/education_image.png\" alt=\"subject\" class=\"w-8 h-8 mr-3\">" +
                                    $"<p class=\"font-medium\">{subject.SubjectName}</p>" +
                                $"</div>" +
                                $"<div>" +
                                    $"<input id=\"{subject.SubjectCode}\" onchange=\"HandleChangeSubjectTeaching(this)\" type=\"checkbox\" class=\"h-5 w-5 text-green-600 rounded border-gray-300 focus:ring-green-500\">" +
                                $"</div>" +
                            $"</div>" +
                        $"</div>";
            }

            LiteralControl literalControlSubject = new LiteralControl(html);
            subject_not_lecturer_list.Controls.Clear();
            subject_not_lecturer_list.Controls.Add(literalControlSubject);
        }

        [WebMethod]
        public static object HandleGetSubjectLecturer(string lecturerCode)
        {
            List<Models.Res.Subject> subjects = _subjectLecturerBLL.GetSubjectLecturerByLecturerCode(lecturerCode);

            if (subjects == null) return new { status = "404", message = "Giảng viên chưa được phân phụ trách môn học nào !" };
            else return new { status = "200", subjects = subjects };
        }

        [WebMethod]
        public static object HandleChangeSubjectLecturer(Models.Req.SubjectTeaching subjectLecturer)
        {
            bool exec = false;

            if (subjectLecturer.Mode == "uncheck") exec = _subjectLecturerBLL.DeleteSubjectLecturer(subjectLecturer);
            else exec = _subjectLecturerBLL.InsertSubjectLecturer(subjectLecturer);

            if (exec) return new { status = "200", message = "Thay đổi giảng viên phụ trách thành công" };
            else return new { status = "500", message = "Server Error" };
        }
    }
}