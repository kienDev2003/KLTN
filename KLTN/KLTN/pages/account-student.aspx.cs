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
    public partial class account_student : System.Web.UI.Page
    {
        private static StudentBLL _studentBLL = new StudentBLL();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["login"] == null) return;

            HandleGetAllStudent();
        }

        private void HandleGetAllStudent()
        {
            List<Models.Req.Student> students = _studentBLL.GetAllStudents();

            if (students == null)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showWarring", "showWarring('Chưa có giảng viên nào !');", true);
                return;
            }

            string html = "";
            foreach (var student in students)
            {
                html += $"<tr class=\"text-center\">" +
                            $"<td class=\"border border-gray-300 px-4 py-2\">{student.StudentCode}</td>" +
                            $"<td class=\"border border-gray-300 px-4 py-2\">{student.FullName}</td>" +
                            $"<td class=\"border border-gray-300 px-4 py-2\">{student.DateOfBirth}</td>" +
                            $"<td class=\"border border-gray-300 px-4 py-2\">{student.ClassName}</td>" +
                            $"<td class=\"border border-gray-300 px-4 py-2 max-w-[250px] truncate\" title=\"{student.Email}\">{student.Email}</td>" +
                            $"<td class=\"border border-gray-300 px-4 py-2 max-w-[250px] truncate\" title=\"{student.MajorName}\">{student.MajorName}</td>" +
                            $"<td class=\"border border-gray-300 px-4 py-2\">" +
                                $"<input type=\"button\" class=\"cursor-pointer bg-blue-500 text-white px-2 py-1 rounded\" onclick=\"HandleEditStudent('{student.StudentCode}')\" value=\"Sửa\">" +
                                $"<input type=\"button\" class=\"cursor-pointer bg-red-500 text-white px-2 py-1 rounded\" onclick=\"HandleDeleteStudent('{student.StudentCode}')\" value=\"Xóa\">" +
                            $"</td>" +
                        $"</tr>";
            }

            LiteralControl literalControl = new LiteralControl(html);
            student_table.Controls.Clear();
            student_table.Controls.Add(literalControl);
        }

        [WebMethod]
        public static object HandleInsertStudent(Models.Req.Student student)
        {
            bool exec = _studentBLL.InsertStudent(student);

            if (exec) return new { status = "200" };
            else return new { status = "500" };
        }
    }
}