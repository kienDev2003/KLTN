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
    public partial class bank_question : System.Web.UI.Page
    {
        private static QuestionBLL _questionBLL = new QuestionBLL();
        private static LecturerBLL _lecturerBLL = new LecturerBLL();
        private static SubjectBLL _subjectBLL = new SubjectBLL();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["login"] == null) return;
            string subjectCode = Request.QueryString["subjectCode"].ToString();

            HandleGetQuestionBySubjectLecturer(1, subjectCode);
            HandlGetSubjectName(subjectCode);
        }

        private void HandlGetSubjectName(string subjectCode)
        {
            Models.Res.Subject subject = _subjectBLL.GetInfoBySubjectCode(subjectCode);

            if (subject != null) subject_name.InnerText = $"Môn : {subject.SubjectName}";
            else subject_name.InnerText = $"Môn : Chưa xác định !";
        }

        private void HandleGetQuestionBySubjectLecturer(int pageIndex, string subjectCode)
        {
            Models.Res.Login loginSession = Session["login"] as Models.Res.Login;
            

            List<Models.Res.Question> questions = _questionBLL.GetQuestionBySubjectLecturer(loginSession.accountCode, subjectCode, pageIndex);

            if (questions == null)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showWarring", "showWarring('Bạn chưa tạo câu hỏi nào cho môn học này !');", true);
                return;
            }

            string html = "";
            foreach (var question in questions)
            {
                string fullNameLectureCreateQuestion = HandleGetUserCreateQuestion(question.LecturerCode);
                string questionType = HandleFormatQuestionType(question.QuestionType);
                string questionLevel = HandleFormatQuestionLevel(question.QuestionLevel);
                string questionApproval = HandleFormatQuestionIsApproved(question.IsApproved);

                html += $"<tr class=\"text-center\">" +
                            $"<td class=\"border border-gray-300 px-4 py-2 max-w-[200px] truncate\" title=\"{question.QuestionText}\">{question.QuestionText}</td>" +
                            $"<td class=\"border border-gray-300 px-4 py-2 max-w-[200px] truncate\">{questionType}</td>" +
                            $"<td class=\"border border-gray-300 px-4 py-2 max-w-[200px] truncate\">{questionLevel}</td>" +
                            $"<td class=\"border border-gray-300 px-4 py-2 max-w-[200px] truncate\">{question.CreateDate}</td>" +
                            $"<td class=\"border border-gray-300 px-4 py-2 truncate max-w-[100px]\" title=\"{fullNameLectureCreateQuestion}\">{fullNameLectureCreateQuestion}</td>" +
                            $"<td class=\"border border-gray-300 px-4 py-2 max-w-[200px] truncate\">{questionApproval}</td>" +
                            $"<td class=\"border border-gray-300 px-4 py-2\">" +
                                $"<input type=\"button\" class=\"cursor-pointer bg-blue-500 text-white px-2 py-1 rounded\" onclick=\"HandleViewQuestion({question.QuestionCode})\" value=\"Xem\">" +
                                $"<input type=\"button\" class=\"{(question.IsApproved ? "hidden" : "")} cursor-pointer bg-red-500 text-white px-2 py-1 rounded\" onclick=\"HandleQuestionApproval({question.QuestionCode})\" value=\"Duyệt\">" +
                            $"</td>" +
                        $"</tr>";
            }

            LiteralControl literalControl = new LiteralControl(html);
            question_table.Controls.Clear();
            question_table.Controls.Add(literalControl);

            HandleCreatePageNumber(loginSession.accountCode, subjectCode);
        }

        private void HandleCreatePageNumber(int accountCode, string subjectCode)
        {
            int questionNumber = _questionBLL.GetNumberQuestionBySubjectLecturer(accountCode, subjectCode);
            int page = questionNumber / 10;

            if (page < 1)
            {
                pagination.Controls.Clear();
                return;
            }

            string html = "";
            for (int i = 0; i < page; i++)
            {
                string temp = "";
                if (i == 0) temp = $"<input type=\"button\" class=\"pagination-btn pagination-active px-4 py-2 border border-gray-300 bg-blue-500 text-white text-sm font-medium rounded hover:bg-blue-600\" value=\"{i + 1}\">";
                temp = $"<input type=\"button\" class=\"pagination-btn px-4 py-2 border border-gray-300 bg-white text-sm font-medium text-gray-700 rounded hover:bg-gray-50\" value=\"{i + 1}\">";
                html += temp;
            }

            LiteralControl literalControl = new LiteralControl(html);
            pagination.Controls.Clear();
            pagination.Controls.Add(literalControl);

        }

        private string HandleFormatQuestionLevel(string levelEnglish)
        {
            if (levelEnglish == "basic") return "Dễ";
            if (levelEnglish == "medium") return "Trung Bình";
            if (levelEnglish == "hard") return "Khó";
            return "Null";
        }

        private string HandleFormatQuestionType(string levelEnglish)
        {
            if (levelEnglish == "single") return "Một câu trả lời";
            if (levelEnglish == "multiple") return "Nhiều câu trả lời";
            return "Null";
        }

        private string HandleFormatQuestionIsApproved(bool IsApproved)
        {
            if (IsApproved) return "Đã duyệt";
            return "Chưa duyệt";
        }

        private string HandleGetUserCreateQuestion(string lecturerCode)
        {
            Models.Res.Lecturer lecturer = _lecturerBLL.GetInfoByLecturerCode(lecturerCode);

            return lecturer.FullName;
        }

        [WebMethod]
        public static object HandleQuestionApproval(int questionCode)
        {
            bool exec = _questionBLL.ApprovalQuestion(questionCode);

            if (exec) return new { status = "200", message = "Duyệt câu hỏi thành công !" };
            else return new { status = "404", message = "Không tìm thấy câu hỏi !" };
        }
    }
}