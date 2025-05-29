using KLTN.Models.Res;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace KLTN.DAL
{
    public class ExamDAL
    {
        private DB.DbConn _db;

        public ExamDAL()
        {
            _db = new DB.DbConn();
        }

        public bool CheckNumberQuestion(string subjectCode, int chapterCode, string questionLevel, int numberQuestion)
        {
            string query = @"SELECT COUNT (QuestionCode)
                             FROM Question
                             WHERE SubjectCode = @subjectCode
                                AND ChapterCode = @chapterCode
                                AND QuestionLevel = @questionLevel";

            int number = -1;

            using (SqlConnection conn = _db.GetConn())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@subjectCode", subjectCode);
                    cmd.Parameters.AddWithValue("@chapterCode", chapterCode);
                    cmd.Parameters.AddWithValue("@questionLevel", questionLevel);

                    number = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }

            if (number < numberQuestion) return false;
            else return true;
        }

        public bool InsertExam(Models.Req.Exam exam)
        {
            string query1 = @"INSERT INTO ExamPaper (SubjectCode, ExamPaperText, CreateByLectuterCode, CreatedDate, ExamTime, IsApproved)
                              VALUES (@subjectCode, @examName, @createByLectuterCode, @createdDate, @examTime, 0);
                              SELECT SCOPE_IDENTITY();";

            string query2 = @"INSERT INTO ExamPaper_Question (ExamPaperCode, QuestionCode)
                              VALUES (@examPaperCode, @questionCode)";

            using (SqlConnection conn = _db.GetConn())
            {
                using (SqlTransaction tran = conn.BeginTransaction())
                {
                    try
                    {
                        int examPaperCode = -1;

                        using (SqlCommand cmd1 = new SqlCommand(query1, conn, tran))
                        {
                            cmd1.Parameters.AddWithValue("@subjectCode", exam.SubjectCode);
                            cmd1.Parameters.AddWithValue("@examName", exam.ExamName);
                            cmd1.Parameters.AddWithValue("@createByLectuterCode", exam.CreateByLectuterCode);
                            cmd1.Parameters.AddWithValue("@createdDate", exam.CreatedDate);
                            cmd1.Parameters.AddWithValue("@examTime", exam.ExamTime);

                            examPaperCode = Convert.ToInt32(cmd1.ExecuteScalar());
                        }

                        foreach (var chapter in exam.chapterExams)
                        {
                            DataTable dataQuestionBasic = GetQuestionCode(chapter.NumberBasic, chapter.ChapterCode, "basic");
                            DataTable dataQuestionMedium = GetQuestionCode(chapter.NumberMedium, chapter.ChapterCode, "medium");
                            DataTable dataQuestionHard = GetQuestionCode(chapter.NumberHard, chapter.ChapterCode, "hard");

                            foreach (DataRow row in dataQuestionBasic.Rows)
                            {
                                using (SqlCommand cmd2 = new SqlCommand(query2, conn, tran))
                                {
                                    cmd2.Parameters.AddWithValue("@examPaperCode", examPaperCode);
                                    cmd2.Parameters.AddWithValue("@questionCode", Convert.ToInt32(row["QuestionCode"]));

                                    cmd2.ExecuteNonQuery();
                                }
                            }

                            foreach (DataRow row in dataQuestionMedium.Rows)
                            {
                                using (SqlCommand cmd2 = new SqlCommand(query2, conn, tran))
                                {
                                    cmd2.Parameters.AddWithValue("@examPaperCode", examPaperCode);
                                    cmd2.Parameters.AddWithValue("@questionCode", Convert.ToInt32(row["QuestionCode"]));

                                    cmd2.ExecuteNonQuery();
                                }
                            }

                            foreach (DataRow row in dataQuestionHard.Rows)
                            {
                                using (SqlCommand cmd2 = new SqlCommand(query2, conn, tran))
                                {
                                    cmd2.Parameters.AddWithValue("@examPaperCode", examPaperCode);
                                    cmd2.Parameters.AddWithValue("@questionCode", Convert.ToInt32(row["QuestionCode"]));

                                    cmd2.ExecuteNonQuery();
                                }
                            }

                        }

                        tran.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                    }
                }
            }

            return false;
        }

        public DataTable GetQuestionCode(int numberQuestion, int chapterCode, string questionLevel)
        {
            DataTable data = new DataTable();
            string query = @"SELECT TOP (@numberQuestion) QuestionCode
                              FROM Question
                              WHERE ChapterCode = @chapterCode
                              AND QuestionLevel = @questionLevel
                              ORDER BY NEWID()";

            using (SqlConnection conn = _db.GetConn())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@numberQuestion", numberQuestion);
                    cmd.Parameters.AddWithValue("@chapterCode", chapterCode);
                    cmd.Parameters.AddWithValue("@questionLevel", questionLevel);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(data);
                    }
                }
            }

            return data;
        }

        public DataTable GetAllExamBySubjectCode(string subjectCode)
        {
            DataTable data = new DataTable();
            string query = @"SELECT * 
                             FROM ExamPaper
                             WHERE SubjectCode = @subjectCode";

            using (SqlConnection conn = _db.GetConn())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@subjectCode", subjectCode);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(data);
                    }
                }
            }

            return data;
        }

        public DataTable GetAllExam()
        {
            DataTable data = new DataTable();
            string query = @"SELECT * 
                             FROM ExamPaper";

            using (SqlConnection conn = _db.GetConn())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(data);
                    }
                }
            }

            return data;
        }

        public DataTable GetExamPaper(int examPaperCode)
        {
            DataTable data = new DataTable();
            string query = @"SELECT * 
                             FROM ExamPaper
                             WHERE ExamPaperCode = @examPaperCode";

            using (SqlConnection conn = _db.GetConn())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@examPaperCode", examPaperCode);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(data);
                    }
                }
            }

            return data;
        }

        public DataTable GetListQuestionByExamPaperCode(int examPaperCode)
        {
            DataTable data = new DataTable();
            string query = @"SELECT QuestionCode
                             FROM ExamPaper_Question
                             WHERE ExamPaperCode = @examPaperCode";

            using (SqlConnection conn = _db.GetConn())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@examPaperCode", examPaperCode);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(data);
                    }
                }
            }

            return data;
        }

        public bool DeleteExam(int examPaperCode)
        {
            string query1 = @"DELETE ExamPaper_Question WHERE ExamPaperCode = @examPaperCode";
            string query2 = @"DELETE ExamPaper WHERE ExamPaperCode = @examPaperCode";

            using (SqlConnection conn = _db.GetConn())
            {
                using (SqlTransaction tran = conn.BeginTransaction())
                {
                    try
                    {
                        using (SqlCommand cmd1 = new SqlCommand(query1, conn, tran))
                        {
                            cmd1.Parameters.AddWithValue("@examPaperCode", examPaperCode);

                            cmd1.ExecuteNonQuery();
                        }

                        using (SqlCommand cmd2 = new SqlCommand(query2, conn, tran))
                        {
                            cmd2.Parameters.AddWithValue("@examPaperCode", examPaperCode);

                            cmd2.ExecuteNonQuery();
                        }

                        tran.Commit();
                        return true;

                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                    }
                }
            }

            return false;
        }

        public bool ExamPaperApproved(int examPaperCode, string lecturerCodeApproved)
        {
            string query = @"UPDATE ExamPaper SET IsApproved = 1, ApprovedByLectuterCode = @lecturerCodeApproved, ApprovedDate = @approvedDate WHERE ExamPaperCode = @examPaperCode";

            using (SqlConnection conn = _db.GetConn())
            {
                using (SqlTransaction tran = conn.BeginTransaction())
                {
                    try
                    {
                        using (SqlCommand cmd = new SqlCommand(query, conn, tran))
                        {
                            cmd.Parameters.AddWithValue("@examPaperCode", examPaperCode);
                            cmd.Parameters.AddWithValue("@lecturerCodeApproved", lecturerCodeApproved);
                            cmd.Parameters.AddWithValue("@approvedDate", DateTime.Now);

                            cmd.ExecuteNonQuery();
                        }

                        tran.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                    }
                }

            }

            return false;
        }

        public int GetNumberQuestion(int examPaperCode)
        {
            string query = @"SELECT COUNT (QuestionCode)
                             FROM ExamPaper_Question
                             WHERE ExamPaperCode = @examPaperCode";

            int number = -1;

            using (SqlConnection conn = _db.GetConn())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@examPaperCode", examPaperCode);

                    number = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }

            return number;
        }

        public bool InsertExamSubmitted(Models.Req.ExamSubmitted examSubmit, float score, string studentCode, int numberQuestionTotal, int numberQuestionTrue)
        {
            string query = @"INSERT INTO ExamSubmitted (ExamSessionCode, StudentCode, SubmittedDate, Score, AnswersJson, Note, NumberQuestionTotal, NumberQuestionTrue, ExamPaperCode)
                             VALUES (@examSessionCode, @studentCode, @submittedDate, @score, @answerJson, @note, @numberQuestionTotal, @numberQuestionTrue, @examPaperCode)";

            using (SqlConnection conn = _db.GetConn())
            {
                using (SqlTransaction tran = conn.BeginTransaction())
                {
                    try
                    {
                        using (SqlCommand cmd = new SqlCommand(query, conn, tran))
                        {
                            cmd.Parameters.AddWithValue("@examSessionCode", examSubmit.ExamSessionCode);
                            cmd.Parameters.AddWithValue("@studentCode", studentCode);
                            cmd.Parameters.AddWithValue("@submittedDate", DateTime.Now);
                            cmd.Parameters.AddWithValue("@score", Math.Round(Convert.ToDouble(score), 2));
                            cmd.Parameters.AddWithValue("@answerJson", examSubmit.QuestionSubmitJson);
                            cmd.Parameters.AddWithValue("@note", examSubmit.Note);
                            cmd.Parameters.AddWithValue("@numberQuestionTotal", numberQuestionTotal);
                            cmd.Parameters.AddWithValue("@numberQuestionTrue", numberQuestionTrue);
                            cmd.Parameters.AddWithValue("@examPaperCode", examSubmit.ExamPaperCode);

                            cmd.ExecuteNonQuery();
                        }

                        tran.Commit();
                        return true;

                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                    }
                }
            }

            return false;
        }

        public DataTable GetExamResultForStudent(int examSessionCode, string studentCode)
        {
            string query = @"SELECT ExamPaper.ExamPaperText, NumberQuestionTrue, NumberQuestionTotal, Score, Note FROM ExamSubmitted
	                             JOIN ExamPaper ON ExamSubmitted.ExamPaperCode = ExamPaper.ExamPaperCode
                             WHERE ExamSubmitted.ExamSessionCode = @examSessionCode AND ExamSubmitted.StudentCode = @studentCode";

            DataTable data = new DataTable();

            using (SqlConnection conn = _db.GetConn())
            {
                using(SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@examSessionCode", examSessionCode);
                    cmd.Parameters.AddWithValue("@studentCode", studentCode);

                    using(SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(data);
                    }
                }
            }

            return data;
        }

        public bool InsertWarringHiddenWindow(int examSessionCode, string studentCode)
        {
            string query1 = @"SELECT Status, DateWarring FROM ExamSession_Warring WHERE ExamSessionCode = @examSessionCode
                              AND StudentCode = @studentCode";

            string query2 = @"INSERT INTO ExamSession_Warring (ExamSessionCode, StudentCode, Status, DateWarring)
                             VALUES (@examSessionCode, @studentCode, 1, @dateWarring)";

            string query3 = @"UPDATE ExamSession_Warring SET Status = 1, DateWarring = @dateWarring
                              WHERE ExamSessionCode = @examSessionCode AND StudentCode = @studentCode";

            using(SqlConnection conn = _db.GetConn())
            {
                using(SqlTransaction tran = conn.BeginTransaction())
                {
                    try
                    {
                        bool exist = false;

                        using(SqlCommand cmd1 = new SqlCommand(query1, conn, tran))
                        {
                            cmd1.Parameters.AddWithValue("@examSessionCode", examSessionCode);
                            cmd1.Parameters.AddWithValue("@studentCode", studentCode);

                            using(SqlDataReader reader = cmd1.ExecuteReader())
                            {
                                if (reader.Read()) exist = true;
                            }
                        }

                        if (!exist)
                        {
                            using (SqlCommand cmd2 = new SqlCommand(query2, conn, tran))
                            {
                                cmd2.Parameters.AddWithValue("@examSessionCode", examSessionCode);
                                cmd2.Parameters.AddWithValue("@studentCode", studentCode);
                                cmd2.Parameters.AddWithValue("@dateWarring", DateTime.Now);

                                cmd2.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            using (SqlCommand cmd3 = new SqlCommand(query3, conn, tran))
                            {
                                cmd3.Parameters.AddWithValue("@examSessionCode", examSessionCode);
                                cmd3.Parameters.AddWithValue("@studentCode", studentCode);
                                cmd3.Parameters.AddWithValue("@dateWarring", DateTime.Now);

                                cmd3.ExecuteNonQuery();
                            }
                        }
                        

                        tran.Commit();
                        return true;

                    }catch(Exception ex)
                    {
                        tran.Rollback();
                    }
                }
            }

            return false;
        }

        public DataTable GetAllExamSessionWarring(int examSessionCode)
        {
            DataTable data = new DataTable();
            string query = @"SELECT StudentCode, DateWarring, ExamSessionCode FROM ExamSession_Warring
                             WHERE ExamSessionCode = @examSessionCode AND Status = 1";

            using(SqlConnection conn = _db.GetConn())
            {
                using(SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@examSessionCode", examSessionCode);

                    using(SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(data);
                    }
                }
            }

            return data;
        }

        public bool CheckedWarring(int examSessionCode, string studentCode)
        {
            string query = @"UPDATE ExamSession_Warring SET Status = 0
                             WHERE ExamSessionCode = @examSessionCode AND StudentCode = @studentCode";

            using(SqlConnection conn= _db.GetConn())
            {
                using(SqlTransaction tran = conn.BeginTransaction())
                {
                    try
                    {
                        using(SqlCommand cmd = new SqlCommand(query, conn, tran))
                        {
                            cmd.Parameters.AddWithValue("@examSessionCode", examSessionCode);
                            cmd.Parameters.AddWithValue("@studentCode", studentCode);

                            cmd.ExecuteNonQuery();
                        }

                        tran.Commit();
                        return true;
                    }catch(Exception ex)
                    {
                        tran.Rollback();
                    }
                }
            }

            return false;
        }

        public DataTable CheckSubmissionRequirements(string studentCode, int examSessionCode)
        {
            DataTable data = new DataTable();
            string query = @"SELECT SubmissionRequirements, NoteSubmissionRequirements FROM ExamSession_Student
                             WHERE ExamSessionCode = @examSessionCode AND StudentCode = @studentCode";

            using(SqlConnection conn = _db.GetConn())
            {
                using(SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@examSessionCode", examSessionCode);
                    cmd.Parameters.AddWithValue("@studentCode", studentCode);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(data);
                    }
                }
            }

            return data;
        }
    }
}