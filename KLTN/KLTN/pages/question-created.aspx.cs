using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.WebControls;
using KLTN.BLL;

namespace KLTN.pages
{
    public partial class question_created : System.Web.UI.Page
    {
        private static QuestionBLL _questionBLL = new QuestionBLL();
        private static LecturerBLL _lecturerBLL = new LecturerBLL();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["login"] == null) return;
            HandleGetQuestionCreatedByAccountCode(1);
        }

        private void HandleGetQuestionCreatedByAccountCode(int pageIndex)
        {
            Models.Res.Login loginSession = Session["login"] as Models.Res.Login;

            List<Models.Res.Question> questions = _questionBLL.GetQuestionByAccountCode(loginSession.accountCode, pageIndex);

            if(questions == null)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showWarring", "showWarring('Bạn chưa tạo câu hỏi nào cho môn học này !');", true);
                return;
            }

            string html = "";
            foreach(var question in questions)
            {
                html += $"<tr class=\"text-center\">" +
                            $"<td class=\"border border-gray-300 px-4 py-2 max-w-[250px] truncate\" title=\"{question.QuestionText}\">{question.QuestionText}</td>"+
                            $"<td class=\"border border-gray-300 px-4 py-2\">{HandleFormatQuestionType(question.QuestionType)}</td>" +
                            $"<td class=\"border border-gray-300 px-4 py-2\">{HandleFormatQuestionLevel(question.QuestionLevel)}</td>" +
                            $"<td class=\"border border-gray-300 px-4 py-2\">{question.CreateDate}</td>" +
                            $"<td class=\"border border-gray-300 px-4 py-2\">{HandleFormatQuestionIsApproved(question.IsApproved)}</td>" +
                            $"<td class=\"border border-gray-300 px-4 py-2\">" +
                                $"<input type=\"button\" class=\"cursor-pointer bg-blue-500 text-white px-2 py-1 rounded\" onclick=\"HandleViewQuestion({question.QuestionCode})\" value=\"Xem\">" +
                                $"<input type=\"button\" class=\"{(question.IsApproved ? "hidden" : "")} cursor-pointer bg-yellow-500 text-white px-2 py-1 rounded\" onclick=\"HandleEditQuestion({question.QuestionCode})\" value=\"Sửa\">" +
                                $"<input type=\"button\" class=\"{(question.IsApproved ? "hidden" : "")} cursor-pointer bg-red-500 text-white px-2 py-1 rounded\" onclick=\"HandleDeteleQuestion({question.QuestionCode})\" value=\"Xóa\">" +
                            $"</td>" +
                        $"</tr>";
            }

            LiteralControl literalControl = new LiteralControl(html);
            question_table.Controls.Clear();
            question_table.Controls.Add(literalControl);

            HandleCreatePageNumber(loginSession.accountCode);
        }

        private void HandleCreatePageNumber(int accountCode)
        {
            int questionNumber = _questionBLL.GetQuestionNumberByAccountCode(accountCode);
            int page = questionNumber / 10;

            if (page < 1)
            {
                pagination.Controls.Clear();
                return;
            }

            string html = "";
            for(int i = 0;i< page;i++)
            {
                string temp = "";
                if (i == 0) temp = $"<input type=\"button\" class=\"pagination-btn pagination-active px-4 py-2 border border-gray-300 bg-blue-500 text-white text-sm font-medium rounded hover:bg-blue-600\" value=\"{i + 1}\">";
                temp = $"<input type=\"button\" class=\"pagination-btn px-4 py-2 border border-gray-300 bg-white text-sm font-medium text-gray-700 rounded hover:bg-gray-50\" value=\"{i+1}\">";
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

        [WebMethod]
        public static object HandleInsertQuestion(Models.Req.Question questionRequest)
        {
            Models.Res.Login loginSession = HttpContext.Current.Session["login"] as Models.Res.Login;
            Models.Res.Lecturer lecturer = _lecturerBLL.GetInfo(loginSession.accountCode);

            if (lecturer == null) return new { status="400", message= "Quyền truy cập không xác định !" };

            questionRequest.lecturerCode = lecturer.LecturerCode;
            questionRequest.createDate = DateTime.Now;

            bool exec = _questionBLL.InsertQuestion(questionRequest);

            if (!exec) return new { status = "500", message = "Lỗi máy chủ !" };
            else return new { status = "200", message = "Thêm câu hỏi thành công !" };
        }

        [WebMethod]
        public static object HandleViewQuestion(int questionCode)
        {
            Models.Res.Question question = _questionBLL.GetQuestionByQuestionCode(questionCode);

            if (question == null) return new { status = "404", message = "Không tìm thấy câu hỏi" };
            else return new { status = "200", question = question };
        }

        [WebMethod]
        public static object HandleDeleteQuestion(int questionCode)
        {
            bool exec = _questionBLL.DeleteQuestion(questionCode);

            if (!exec) return new { status = "404", message = "Không tìm thấy câu hỏi để xóa !" };
            else return new { status = "200", message="Xóa câu hỏi thành công !" };
        }

        [WebMethod]
        public static object HandleUpdateQuestion(Models.Req.Question questionRequest)
        {
            questionRequest.createDate = DateTime.Now;

            bool exec = _questionBLL.UpdateQuestion(questionRequest);

            if (!exec) return new { status = "404", message = "Không tìm thấy câu hỏi !" };
            else return new { status = "200", message = "Sửa câu hỏi thành công !" };
        }
    }
}