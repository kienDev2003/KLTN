using System;
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

        public DataTable GetAllExam(string subjectCode)
        {
            DataTable data = new DataTable();
            string query = @"SELECT * 
                             FROM ExamPaper
                             WHERE SubjectCode = @subjectCode";

            using(SqlConnection conn = _db.GetConn())
            {
                using(SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@subjectCode", subjectCode);

                    using(SqlDataAdapter adapter = new SqlDataAdapter(cmd))
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

            using(SqlConnection conn = _db.GetConn())
            {
                using(SqlTransaction tran = conn.BeginTransaction())
                {
                    try
                    {
                        using(SqlCommand cmd1 = new SqlCommand(query1, conn, tran))
                        {
                            cmd1.Parameters.AddWithValue("@examPaperCode", examPaperCode);

                            cmd1.ExecuteNonQuery();
                        }

                        using(SqlCommand cmd2 = new SqlCommand(query2, conn, tran))
                        {
                            cmd2.Parameters.AddWithValue("@examPaperCode", examPaperCode);

                            cmd2.ExecuteNonQuery();
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
    }
}