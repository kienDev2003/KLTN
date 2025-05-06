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
    public partial class assignment_teaching : System.Web.UI.Page
    {
        private static LecturerBLL _lecturerBLL = new LecturerBLL();
        private static SubjectBLL _subjectBLL = new SubjectBLL();
        private static SubjectTeachingBLL _subjectTeachingBLL = new SubjectTeachingBLL();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["login"] == null) return;

            HandleGetLecturerAndSubject();
        }

        private void HandleGetLecturerAndSubject()
        {
            List<Models.Res.Lecturer> lecturers = _lecturerBLL.GetAllLecturer();
            List<Models.Res.Subject> subjects = _subjectBLL.GetSubjectAll();

            if (lecturers == null || subjects == null)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showWarring", "showWarring('Chưa có giảng viên / môn học !');", true);
                return;
            }

            string html = "";
            foreach(var lecturer in lecturers)
            {
                html += $"<div onclick=\"HandleSelectLecturer(this,'{lecturer.LecturerCode}')\" class=\"btn-lecturer border rounded-lg overflow-hidden shadow-sm\">" +
                            $"<div class=\"p-3 border-b flex justify-between items-center\">" +
                                $"<div class=\"flex items-center\">" +
                                    $"<img src=\"/api/placeholder/40/40\" alt=\"subject\" class=\"w-8 h-8 mr-3\">" +
                                    $"<p class=\"font-medium\">{lecturer.LecturerCode}-{lecturer.FullName}</p>" +
                                $"</div>" +
                            $"</div>" +
                        $"</div>";
            }

            LiteralControl literalControlLecturer = new LiteralControl(html);
            lecturer_list.Controls.Clear();
            lecturer_list.Controls.Add(literalControlLecturer);

            html = "";
            foreach(var subject in subjects)
            {
                html += $"<div class=\"border rounded-lg overflow-hidden shadow-sm\">" +
                            $"<div class=\"p-3 border-b flex justify-between items-center\">" +
                                $"<div class=\"flex items-center\">" +
                                    $"<img src=\"/api/placeholder/40/40\" alt=\"subject\" class=\"w-8 h-8 mr-3\">" +
                                    $"<p class=\"font-medium\">{subject.SubjectName}</p>" +
                                $"</div>" +
                                $"<div>" +
                                    $"<input id=\"{subject.SubjectCode}\" onchange=\"HandleChangeSubjectTeaching(this)\" type=\"checkbox\" class=\"h-5 w-5 text-green-600 rounded border-gray-300 focus:ring-green-500\">" +
                                $"</div>" +
                            $"</div>" +
                        $"</div>";
            }

            LiteralControl literalControlSubject = new LiteralControl(html);
            subject_list.Controls.Clear();
            subject_list.Controls.Add(literalControlSubject);
        }

        [WebMethod]
        public static object HandleGetSubjectTeaching(string lecturerCode)
        {
            List<Models.Res.Subject> subjects = _subjectTeachingBLL.GetSubjectTeachingByLecturerCode(lecturerCode);

            if (subjects == null) return new { status = "404", message = "Giảng viên chưa được phân dạy môn học nào !" };
            else return new { status = "200", subjects = subjects };
            
        }

        [WebMethod]
        public static object HandleChangeSubjectTeaching(Models.Req.SubjectTeaching subjectTeaching)
        {
            bool exec = false;

            if(subjectTeaching.Mode == "uncheck") exec = _subjectTeachingBLL.DeteleSubjectTeaching(subjectTeaching);
            else exec = _subjectTeachingBLL.InsertSubjectTeaching(subjectTeaching);

            if (exec) return new { status = "200", message = "Phân công giảng dạy thành công !" };
            else return new { status = "500", message = "Server Error" };
        }
    }
}