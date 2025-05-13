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
    public partial class exam_created : System.Web.UI.Page
    {
        private static ExamBLL _examBLL = new ExamBLL();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["login"] == null) return;


        }

        [WebMethod]
        public static object InsertExam(Models.Req.Exam exam)
        {
            foreach(var chapter in exam.chapterExams)
            {
                bool numberBasic = _examBLL.CheckNumberQuestion(exam.SubjectCode, chapter.ChapterCode, "basic", chapter.NumberBasic);
                bool numberMedium = _examBLL.CheckNumberQuestion(exam.SubjectCode, chapter.ChapterCode, "medium", chapter.NumberMedium);
                bool numberHard = _examBLL.CheckNumberQuestion(exam.SubjectCode, chapter.ChapterCode, "hard", chapter.NumberHard);

                if(!numberBasic || !numberMedium || !numberHard)
                {
                    return new
                    {
                        status = "500",
                        message = "Không đủ câu hỏi để tạo đề thi !"
                    };
                }
            }

            exam.CreatedDate = DateTime.Now;

            bool exec = _examBLL.InsertExam(exam);

            if (exec) return new { status = "200", message = "Tạo đề thi thành công" };
            else return new { status = "500", message = "Server error" };
        }
    }
}