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
    public partial class account_lecturer : System.Web.UI.Page
    {
        private static LecturerBLL _lecturerBLL = new LecturerBLL();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["login"] == null) return;

            HandleGetAllLecturer();
        }

        private void HandleGetAllLecturer()
        {
            List<Models.Res.Lecturer> lecturers = _lecturerBLL.GetAllLecturer();

            if (lecturers == null)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showWarring", "showWarring('Chưa có giảng viên nào !');", true);
                return;
            }

            string html = "";
            foreach (var lecturer in lecturers)
            {
                html += $"<tr class=\"text-center\">" +
                            $"<td class=\"border border-gray-300 px-4 py-2\">{lecturer.LecturerCode}</td>" +
                            $"<td class=\"border border-gray-300 px-4 py-2\">{lecturer.FullName}</td>" +
                            $"<td class=\"border border-gray-300 px-4 py-2\">{lecturer.DateOfBirth.ToString("dd/MM/yyyy")}</td>" +
                            $"<td class=\"border border-gray-300 px-4 py-2 max-w-[250px] truncate\" title=\"{lecturer.Email}\">{lecturer.Email}</td>" +
                            $"<td class=\"border border-gray-300 px-4 py-2 max-w-[250px] truncate\" title=\"{lecturer.DepartmentName}\">{lecturer.DepartmentName}</td>" +
                            $"<td class=\"border border-gray-300 px-4 py-2\">" +
                                $"<input type=\"button\" class=\"cursor-pointer bg-blue-500 text-white px-2 py-1 rounded\" onclick=\"HandleEditLecturer('{lecturer.LecturerCode}')\" value=\"Sửa\">" +
                                $"<input type=\"button\" class=\"cursor-pointer bg-red-500 text-white px-2 py-1 rounded\" onclick=\"HandleDeleteLecturer('{lecturer.LecturerCode}')\" value=\"Xóa\">" +
                            $"</td>" +
                        $"</tr>";
            }

            LiteralControl literalControl = new LiteralControl(html);
            lecturer_table.Controls.Clear();
            lecturer_table.Controls.Add(literalControl);
        }

        [WebMethod]
        public static object HandleInsertLecturer(Models.Req.Lecturer lecturer)
        {
            bool exec = _lecturerBLL.InsertLecturer(lecturer);

            if (exec) return new { status = "200" };
            else return new { status = "500" };
        }
    }
}