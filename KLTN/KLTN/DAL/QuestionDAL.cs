using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace KLTN.DAL
{
    public class QuestionDAL
    {
        private DB.DbConn _db;

        public QuestionDAL()
        {
            _db = new DB.DbConn();
        }

        public DataTable GetQuestionBySubjectTeaching(int accountCode, string subjectCode, int pageIndex)
        {
            int start = -1;
            if (pageIndex == 1) start = pageIndex - 1;
            else start = ((pageIndex - 1) * 10) + 1;

            DataTable data = new DataTable();
            string query = @"SELECT QuestionCode, QuestionText, QuestionLevel, QuestionType, IsApproved, Question.CreateDate
                             FROM Question
                             JOIN Lecturer ON Question.LecturerCode = Lecturer.LecturerCode
                             JOIN Account ON Lecturer.AccountCode = Account.AccountCode
                             WHERE Account.AccountCode = @accountCode AND Question.SubjectCode = @subjectCode
                             ORDER BY IsApproved ASC, CreateDate DESC
                             OFFSET @start ROWS FETCH NEXT 10 ROWS ONLY";

            using (SqlConnection conn = _db.GetConn())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@accountCode", accountCode);
                    cmd.Parameters.AddWithValue("@start", start);
                    cmd.Parameters.AddWithValue("@subjectCode", subjectCode);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(data);
                    }
                }
                conn.Close();
            }
            return data;
        }

        public DataTable GetQuestionBySubjectLecturer(int accountCode, string subjectCode, int pageIndex)
        {
            int start = -1;
            if (pageIndex == 1) start = pageIndex - 1;
            else start = ((pageIndex - 1) * 10) + 1;

            DataTable data = new DataTable();
            string query = @"SELECT QuestionCode, QuestionText, ChapterCode, QuestionLevel, QuestionType, IsApproved, Question.CreateDate, Question.LecturerCode
                             FROM Question
                             JOIN Subject ON Question.SubjectCode = Subject.SubjectCode
                             JOIN Subject_Lecturer ON Subject.SubjectCode = Subject_Lecturer.SubjectCode
                             JOIN Lecturer ON Subject_Lecturer.LecturerCode = Lecturer.LecturerCode
                             JOIN Account ON Lecturer.AccountCode = Account.AccountCode
                             WHERE Account.AccountCode = @accountCode AND Subject.SubjectCode = @subjectCode
                             ORDER BY IsApproved ASC, CreateDate DESC
                             OFFSET @start ROWS FETCH NEXT 10 ROWS ONLY";

            using (SqlConnection conn = _db.GetConn())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@accountCode", accountCode);
                    cmd.Parameters.AddWithValue("@start", start);
                    cmd.Parameters.AddWithValue("@subjectCode", subjectCode);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(data);
                    }
                }
                conn.Close();
            }
            return data;
        }

        public bool DeleteQuestionByQuestionCode(int questionCode)
        {
            string query1 = @"DELETE Answer WHERE Answer.QuestionCode = @questionCode";
            string query2 = @"DELETE Question WHERE Question.QuestionCode = @questionCode";

            using (SqlConnection conn = _db.GetConn())
            {
                using (SqlTransaction tran = conn.BeginTransaction())
                {
                    try
                    {
                        using (SqlCommand cmd1 = new SqlCommand(query1, conn, tran))
                        {
                            cmd1.Parameters.AddWithValue("@questionCode", questionCode);

                            cmd1.ExecuteNonQuery();
                        }

                        using (SqlCommand cmd2 = new SqlCommand(query2, conn, tran))
                        {
                            cmd2.Parameters.AddWithValue("@questionCode", questionCode);

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

        public int GetNumberQuestionBySubjectTeaching(int accountCode, string subjectCode)
        {
            DataTable data = new DataTable();
            string query = @"SELECT COUNT(Question.QuestionCode)
                             FROM Question
                             JOIN Subject ON Question.SubjectCode = Subject.SubjectCode
                             JOIN Subject_Teaching ON Subject.SubjectCode = Subject_Teaching.SubjectCode
                             JOIN Lecturer ON Subject_Teaching.LecturerCode = Lecturer.LecturerCode
                             JOIN Account ON Lecturer.AccountCode = Account.AccountCode
                             WHERE Account.AccountCode = @accountCode AND Subject.SubjectCode = @subjectCode";

            using (SqlConnection conn = _db.GetConn())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@accountCode", accountCode);
                    cmd.Parameters.AddWithValue("@subjectCode", subjectCode);

                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        public int GetNumberQuestionBySubjectLecturer(int accountCode, string subjectCode)
        {
            DataTable data = new DataTable();
            string query = @"SELECT COUNT(Question.QuestionCode)
                             FROM Question
                             JOIN Subject ON Question.SubjectCode = Subject.SubjectCode
                             JOIN Subject_Lecturer ON Subject.SubjectCode = Subject_Lecturer.SubjectCode
                             JOIN Lecturer ON Subject_Lecturer.LecturerCode = Lecturer.LecturerCode
                             JOIN Account ON Lecturer.AccountCode = Account.AccountCode
                             WHERE Account.AccountCode = @accountCode AND Subject.SubjectCode = @subjectCode";

            using (SqlConnection conn = _db.GetConn())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@accountCode", accountCode);
                    cmd.Parameters.AddWithValue("@subjectCode", subjectCode);

                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        public bool InsertQuestion(Models.Req.Question questionRequest)
        {
            string query1 = @"INSERT INTO Question (QuestionText,QuestionType,QuestionLevel,ChapterCode,SubjectCode,LecturerCode,IsApproved,CreateDate)
                              VALUES (@questionText,@questionType,@questionLevel,@chapterCode,@subjectCode,@lecturerCode,@isApproved,@createDate);
                              SELECT SCOPE_IDENTITY();";

            string query2 = @"INSERT INTO Answer (AnswerText,AnswerTrue,QuestionCode)
                              VALUES (@answerText,@answerTrue,@questionCode)";

            using (SqlConnection conn = _db.GetConn())
            {
                using (SqlTransaction tran = conn.BeginTransaction())
                {
                    int questionCode = -1;
                    try
                    {
                        using (SqlCommand cmd1 = new SqlCommand(query1, conn, tran))
                        {
                            cmd1.Parameters.AddWithValue("@questionText", questionRequest.questionText);
                            cmd1.Parameters.AddWithValue("@questionType", questionRequest.questionType);
                            cmd1.Parameters.AddWithValue("@questionLevel", questionRequest.questionLevel);
                            cmd1.Parameters.AddWithValue("@subjectCode", questionRequest.subjectCode);
                            cmd1.Parameters.AddWithValue("@lecturerCode", questionRequest.lecturerCode);
                            cmd1.Parameters.AddWithValue("@isApproved", false);
                            cmd1.Parameters.AddWithValue("@createDate", questionRequest.createDate);
                            cmd1.Parameters.AddWithValue("@chapterCode", questionRequest.ChapterCode);

                            questionCode = Convert.ToInt32(cmd1.ExecuteScalar());
                        }

                        foreach (var answer in questionRequest.answers)
                        {
                            using (SqlCommand cmd2 = new SqlCommand(query2, conn, tran))
                            {
                                cmd2.Parameters.AddWithValue("@answerText", answer.AnswerText);
                                cmd2.Parameters.AddWithValue("@answerTrue", answer.AnswerTrue);
                                cmd2.Parameters.AddWithValue("@questionCode", questionCode);

                                cmd2.ExecuteNonQuery();
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

        public bool UpdateQuestion(Models.Req.Question questionRequest)
        {
            string query1 = @"DELETE Answer WHERE Answer.QuestionCode = @questionCode";

            string query2 = @"UPDATE Question
                              SET
                                QuestionText = @questionText,
                                QuestionType = @questionType,
                                QuestionLevel = @questionLevel,
                                CreateDate = @createDate,
                                ChapterCode = @chapterCode
                              WHERE
                                QuestionCode = @questionCode;";

            string query3 = @"INSERT INTO Answer (AnswerText,AnswerTrue,QuestionCode)
                              VALUES (@answerText,@answerTrue,@questionCode)";

            using (SqlConnection conn = _db.GetConn())
            {
                using (SqlTransaction tran = conn.BeginTransaction())
                {
                    try
                    {
                        using (SqlCommand cmd1 = new SqlCommand(query1, conn, tran))
                        {
                            cmd1.Parameters.AddWithValue("@questionCode", questionRequest.questionCode);

                            cmd1.ExecuteNonQuery();
                        }

                        using (SqlCommand cmd2 = new SqlCommand(query2, conn, tran))
                        {
                            cmd2.Parameters.AddWithValue("@questionText", questionRequest.questionText);
                            cmd2.Parameters.AddWithValue("@questionType", questionRequest.questionType);
                            cmd2.Parameters.AddWithValue("@questionLevel", questionRequest.questionLevel);
                            cmd2.Parameters.AddWithValue("@createDate", questionRequest.createDate);
                            cmd2.Parameters.AddWithValue("@questionCode", questionRequest.questionCode);
                            cmd2.Parameters.AddWithValue("@chapterCode", questionRequest.ChapterCode);

                            cmd2.ExecuteNonQuery();
                        }

                        foreach (var answer in questionRequest.answers)
                        {
                            using (SqlCommand cmd3 = new SqlCommand(query3, conn, tran))
                            {
                                cmd3.Parameters.AddWithValue("@answerText", answer.AnswerText);
                                cmd3.Parameters.AddWithValue("@answerTrue", answer.AnswerTrue);
                                cmd3.Parameters.AddWithValue("@questionCode", questionRequest.questionCode);

                                cmd3.ExecuteNonQuery();
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

        public DataTable GetQuestionByQuestionCode(int questionCode)
        {
            DataTable data = new DataTable();
            string query = @"SELECT QuestionCode, QuestionText, QuestionLevel, QuestionType, IsApproved, CreateDate, ChapterCode
                             FROM Question
                             WHERE QuestionCode = @questionCode";

            using (SqlConnection conn = _db.GetConn())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@questionCode", questionCode);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(data);
                    }
                }
            }

            return data;
        }

        public DataTable GetAnswerByQuestionCode(int questionCode)
        {
            DataTable data = new DataTable();
            string query = @"SELECT AnswerCode, AnswerText, AnswerTrue, QuestionCode
                             FROM Answer
                             WHERE QuestionCode = @questionCode";

            using (SqlConnection conn = _db.GetConn())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@questionCode", questionCode);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(data);
                    }
                }
            }

            return data;
        }

        public bool ApprovalQuestion(int questionCode)
        {
            string query = @"UPDATE Question SET IsApproved = 1 WHERE QuestionCode = @questionCode";

            using(SqlConnection conn = _db.GetConn())
            {
                using(SqlTransaction tran = conn.BeginTransaction())
                {
                    try
                    {
                        using (SqlCommand cmd = new SqlCommand(query, conn, tran))
                        {
                            cmd.Parameters.AddWithValue("@questionCode", questionCode);

                            cmd.ExecuteNonQuery();
                        }

                        tran.Commit();
                        return true;
                    }
                    catch(Exception ex)
                    {
                        tran.Rollback();
                    }
                }
            }

            return false;
        }

        public bool CheckAnswerTrue(int answerCode)
        {
            string query = @"SELECT AnswerTrue FROM Answer WHERE AnswerCode = @answerCode";

            using(SqlConnection conn = _db.GetConn())
            {
                using(SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@answerCode", answerCode);

                    using(SqlDataReader reader =  cmd.ExecuteReader())
                    {
                        if(reader.Read())
                        {
                            bool statusAnswer = Convert.ToBoolean(reader["AnswerTrue"]);

                            if (statusAnswer) return true;
                            else return false;
                        }
                    }
                }
            }

            return false;
        }
    }
}