using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KLTN.BLL;
using KLTN.Models.Res;

namespace KLTN.pages.master
{
    public partial class home : System.Web.UI.MasterPage
    {
        LecturerBLL _lecturerBLL = new LecturerBLL();
        StudentBLL _studentBLL = new StudentBLL();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["login"] == null) Response.Redirect("/pages/index.aspx");

            HandleGetUserInfo();
        }

        protected void HandleLogout(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("/pages/index.aspx");
        }

        private void HandleGetUserInfo()
        {
            Models.Res.Login login = Session["login"] as Models.Res.Login;

            if (login.accountType == "GV")
            {
                Models.Res.Lecturer lecturer = _lecturerBLL.GetInfo(login.accountCode);
                if (lecturer == null)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showError", "showError('Không tìm thấy thông tin người dùng !');", true);
                    return;
                }

                user_code.InnerText = lecturer.LecturerCode;
                user_fullname.InnerText = lecturer.FullName;
                HandleGetUserFeature(login.accountType, lecturer.IsLeader);
            }
            if (login.accountType == "SV")
            {
                Models.Res.Student student = _studentBLL.GetInfo(login.accountCode);
                if (student == null)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showError", "showError('Không tìm thấy thông tin người dùng !');", true);
                    return;
                }

                user_code.InnerText = student.StudentCode;
                user_fullname.InnerText = student.FullName;
                HandleGetUserFeature(login.accountType, false);
            }
            if (login.accountType == "QT")
            {
                user_code.InnerText = "0101010";
                user_fullname.InnerText = "Quản trị viên";
                HandleGetUserFeature(login.accountType, false);
            }
        }

        private void HandleGetUserFeature(string accountType, bool isLeader)
        {
            if (isLeader && accountType == "GV")
            {
                string html = @"<li class=""mt-2""><a class=""text-green-500"" href=""/pages/subject-teaching.aspx"">Môn học giảng dạy</a></li>
                                <li class=""mt-2""><a class=""text-green-500"" href=""/pages/subject-lecturer.aspx"">Môn học phụ trách</a></li>
                                <li class=""mt-2""><a class=""text-green-500"" href=""/pages/assignment-lecterer.aspx"">Phân công phụ trách môn học</a></li>
                                <li class=""mt-2""><a class=""text-green-500"" href=""/pages/assignment-teaching.aspx"">Phân công giảng dạy</a></li>
                                <li class=""mt-2""><a class=""text-green-500"" href=""/pages/assignment-exam"">Phân công trông thi</a></li>
                                <li class=""mt-2""><a class=""text-green-500"" href=""/pages/create-exam-session.aspx"">Tạo ca thi</a></li>
                                <li class=""mt-2""><a class=""text-green-500"" href=""/pages/exam.aspx"">Duyệt đề thi</a></li>
                                <li class=""mt-2""><a class=""text-green-500"" href=""/pages/subject.aspx"">Danh sách môn học</a></li>
                               ";

                LiteralControl literalControl = new LiteralControl(html);
                user_feature.Controls.Clear();
                user_feature.Controls.Add(literalControl);

                return;
            }
            if (accountType == "GV")
            {
                string html = @"<li class=""mt-2""><a class=""text-green-500"" href=""/pages/subject-teaching.aspx"">Môn học giảng dạy</a></li>
                                <li class=""mt-2""><a class=""text-green-500"" href=""/pages/subject-lecturer.aspx"">Môn học phụ trách</a></li>
                                <li class=""mt-2""><a class=""text-green-500"" href=""/pages/exam-schedule.aspx"">Lịch trông thi</a></li>
                               ";

                LiteralControl literalControl = new LiteralControl(html);
                user_feature.Controls.Clear();
                user_feature.Controls.Add(literalControl);

                return;
            }
            if (accountType == "SV")
            {
                string html = @"<li class=""mt-2""><a class=""text-green-500"" href=""/pages/exam-session-taken.aspx"">Ca thi đã thi</a></li>
                                <li class=""mt-2""><a class=""text-green-500"" href=""/pages/exam-session-upcoming.aspx"">Ca thi sắp tới</a></li>
                               ";

                LiteralControl literalControl = new LiteralControl(html);
                user_feature.Controls.Clear();
                user_feature.Controls.Add(literalControl);

                return;
            }
            if (accountType == "QT")
            {
                string html = @"<li class=""mt-2""><a class=""text-green-500"" href=""/pages/account-lecturer.aspx"">Tài khoản giảng viên</a></li>
                                <li class=""mt-2""><a class=""text-green-500"" href=""/pages/account-student.aspx"">Tài khoản sinh viên</a></li>
                                <li class=""mt-2""><a class=""text-green-500"" href=""/pages/assignment-leader.aspx"">Phân công trưởng bộ môn</a></li>
                               ";

                LiteralControl literalControl = new LiteralControl(html);
                user_feature.Controls.Clear();
                user_feature.Controls.Add(literalControl);

                return;
            }
        }
    }
}