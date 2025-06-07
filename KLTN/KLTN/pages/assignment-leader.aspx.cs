using KLTN.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KLTN.pages
{
    public partial class schedule_leader : System.Web.UI.Page
    {
        private static LecturerBLL _lecturerBLL = new LecturerBLL();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["login"] == null) return;

            HandleRenderLecturer();
        }

        private void HandleRenderLecturer()
        {
            lecturer_list.Controls.Clear();

            List<Models.Res.Lecturer> lecturers = _lecturerBLL.GetAllLecturer();

            if (lecturers == null) return;

            string html = "";
            foreach (var lecturer in lecturers)
            {
                html += $"<div class=\"{(lecturer.IsLeader ? "" : "opacity-60 grayscale")} border rounded-lg overflow-hidden shadow-sm\">" +
                            $"<div class=\"p-3 border-b flex justify-between items-center\">" +
                                $"<div class=\"flex items-center\">" +
                                    $"<img src=\"/pages/public/images/user_image.png\" alt=\"subject\" class=\"w-8 h-8 mr-3\">" +
                                    $"<p class=\"font-medium\">{lecturer.LecturerCode}-{lecturer.FullName}</p>" +
                                $"</div>" +
                                $"<div>" +
                                    $"<input type=\"checkbox\" {(lecturer.IsLeader ? "checked" : "")} class=\"h-5 w-5 text-green-600 rounded border-gray-300 focus:ring-green-500\" onchange=\"HandleChangeLeader(this, '{lecturer.LecturerCode}')\">" +
                                $"</div>" +
                            $"</div>" +
                        $"</div>";
            }

            LiteralControl literalControl = new LiteralControl(html);
            lecturer_list.Controls.Add(literalControl);
        }

        [WebMethod]
        public static object HandleChangeLeader(string lecturerCode, int status)
        {
            bool exec = _lecturerBLL.ChangeLeader(lecturerCode, status);

            if (exec) return new { status = "200", message = "Thay đổi trạng thái leader thành công !" };
            else return new { status = "500", message = "Server error" };
        }
    }
}