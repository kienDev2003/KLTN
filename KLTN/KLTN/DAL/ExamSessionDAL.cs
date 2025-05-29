using KLTN.Models.Req;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace KLTN.DAL
{
    public class ExamSessionDAL
    {
        private DB.DbConn _db;
        public ExamSessionDAL()
        {
            _db = new DB.DbConn();
        }

        public bool InsertExamSession(Models.Req.ExamSession examSession)
        {
            string query = @"INSERT INTO ExamSession (SubjectCode, StartExamDate, EndExamDate, CreateByLecturer, ExamPaperCode, ExamSessionPassword, InvigilatorMainCode, InvigilatorCode)
                             VALUES (@subjectCode, @startExamDate, @endExamDate, @createByLecturer, @examPaperCode, @examSessionPassword, @invigilatorMainCode, @invigilatorCode);";

            using(SqlConnection conn = _db.GetConn())
            {
                using(SqlTransaction tran = conn.BeginTransaction())
                {
                    try
                    {
                        using(SqlCommand cmd = new SqlCommand(query, conn,tran))
                        {
                            cmd.Parameters.AddWithValue("@subjectCode", examSession.SubjectCode);
                            cmd.Parameters.AddWithValue("@startExamDate", examSession.StartExamDate);
                            cmd.Parameters.AddWithValue("@endExamDate", examSession.EndExamDate);
                            cmd.Parameters.AddWithValue("@createByLecturer", examSession.CreateByLecturer);
                            cmd.Parameters.AddWithValue("@examPaperCode", examSession.ExamPaperCode);
                            cmd.Parameters.AddWithValue("@examSessionPassword", examSession.ExamSessionPassword);
                            cmd.Parameters.AddWithValue("@invigilatorMainCode", examSession.InvigilatorMainCode);
                            cmd.Parameters.AddWithValue("@invigilatorCode", examSession.InvigilatorCode);

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

        public bool UpdateExamSession(Models.Req.ExamSession examSession)
        {
            string query = @"UPDATE ExamSession SET SubjectCode = @subjectCode, StartExamDate = @startExamDate, EndExamDate = @endExamDate,
                                                    CreateByLecturer = @createByLecturer, ExamPaperCode = @examPaperCode, ExamSessionPassword = @examSessionPassword,
                                                    InvigilatorMainCode = @invigilatorMainCode, InvigilatorCode = @invigilatorCode
                             WHERE ExamSessionCode = @examSessionCode";

            using (SqlConnection conn = _db.GetConn())
            {
                using (SqlTransaction tran = conn.BeginTransaction())
                {
                    try
                    {
                        using (SqlCommand cmd = new SqlCommand(query, conn, tran))
                        {
                            cmd.Parameters.AddWithValue("@subjectCode", examSession.SubjectCode);
                            cmd.Parameters.AddWithValue("@startExamDate", examSession.StartExamDate);
                            cmd.Parameters.AddWithValue("@endExamDate", examSession.EndExamDate);
                            cmd.Parameters.AddWithValue("@createByLecturer", examSession.CreateByLecturer);
                            cmd.Parameters.AddWithValue("@examPaperCode", examSession.ExamPaperCode);
                            cmd.Parameters.AddWithValue("@examSessionPassword", examSession.ExamSessionPassword);
                            cmd.Parameters.AddWithValue("@invigilatorMainCode", examSession.InvigilatorMainCode);
                            cmd.Parameters.AddWithValue("@invigilatorCode", examSession.InvigilatorCode);
                            cmd.Parameters.AddWithValue("@examSessionCode", examSession.ExamSessionCode);

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

        public bool DeleteExamSession(int examSessionCode)
        {
            string query = @"DELETE ExamSession
                             WHERE ExamSessionCode = @examSessionCode";

            using (SqlConnection conn = _db.GetConn())
            {
                using (SqlTransaction tran = conn.BeginTransaction())
                {
                    try
                    {
                        using (SqlCommand cmd = new SqlCommand(query, conn, tran))
                        {
                            cmd.Parameters.AddWithValue("@examSessionCode", examSessionCode);

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

        public DataTable GetAllExamSession()
        {
            DataTable data = new DataTable();
            string query = @"SELECT * FROM ExamSession";

            using(SqlConnection conn = _db.GetConn())
            {
                using(SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using(SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(data);
                    }
                }
            }

            return data;
        }

        public DataTable GetAllExamSessionByInvigilatorCode(string invigilatorCode)
        {
            DataTable data = new DataTable();
            string query = @"SELECT *
                            FROM ExamSession
                            WHERE InvigilatorCode = @invigilatorCode OR InvigilatorMainCode = @invigilatorCode;";

            using (SqlConnection conn = _db.GetConn())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@invigilatorCode", invigilatorCode);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(data);
                    }
                }
            }

            return data;
        }

        public DataTable GetAllExamSessionByStudentCode(string studentCode)
        {
            DataTable data = new DataTable();
            string query = @"SELECT ExamSession.* FROM ExamSession_Student
                             JOIN ExamSession ON ExamSession.ExamSessionCode = ExamSession_Student.ExamSessionCode
                             WHERE ExamSession_Student.StudentCode = @studentCode";

            using(SqlConnection conn = _db.GetConn())
            {
                using(SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@studentCode", studentCode);

                    using(SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(data);
                    }
                }
            }
            return data;
        }

        public DataTable GetExamSession(int examSessionCode)
        {
            DataTable data = new DataTable();
            string query = @"SELECT * FROM ExamSession WHERE ExamSessionCode = @examSessionCode";

            using (SqlConnection conn = _db.GetConn())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@examSessionCode", examSessionCode);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(data);
                    }
                }
            }

            return data;
        } 

        public bool InsertStudentInExamSession_Student(string studentCode, int examSessionCode)
        {
            string query = @"INSERT INTO ExamSession_Student (ExamSessionCode, StudentCode, StudentHaveEntered, SubmissionRequirements)
                             VALUES (@examSessionCode, @studentCode, 0, 0)";

            using(SqlConnection conn = _db.GetConn())
            {
                using(SqlTransaction tran = conn.BeginTransaction())
                {
                    try
                    {
                        using (SqlCommand cmd = new SqlCommand(query, conn, tran))
                        {
                            cmd.Parameters.AddWithValue("@examSessionCode", examSessionCode);
                            cmd.Parameters.AddWithValue("@studentCode", studentCode);

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

                return false;
            }
        }

        public DataTable GetStudentByExamSessionCode(int examSessionCode)
        {
            string query = @"SELECT * FROM ExamSession_Student WHERE ExamSessionCode = @examSessionCode";
            DataTable data = new DataTable();

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

        public bool HandleLogoutStudent(int examSessionCode, string studentCode)
        {
            string query = @"UPDATE ExamSession_Student SET StudentHaveEntered = 0 
                             WHERE ExamSessionCode = @examSessionCode AND StudentCode = @studentCode";

            using(SqlConnection conn = _db.GetConn())
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

        public bool HandleLoginStudent(int examSessionCode, string studentCode)
        {
            string query = @"UPDATE ExamSession_Student SET StudentHaveEntered = 1 
                             WHERE ExamSessionCode = @examSessionCode AND StudentCode = @studentCode";

            using (SqlConnection conn = _db.GetConn())
            {
                using (SqlTransaction tran = conn.BeginTransaction())
                {
                    try
                    {
                        using (SqlCommand cmd = new SqlCommand(query, conn, tran))
                        {
                            cmd.Parameters.AddWithValue("@examSessionCode", examSessionCode);
                            cmd.Parameters.AddWithValue("@studentCode", studentCode);

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

        public bool HandleSubmissionRequirements(int examSessionCode, string studentCode, string noteSubmissionRequirements)
        {
            string query = @"UPDATE ExamSession_Student SET SubmissionRequirements = 1, NoteSubmissionRequirements = @noteSubmissionRequirements
                             WHERE ExamSessionCode = @examSessionCode AND StudentCode = @studentCode";

            using (SqlConnection conn = _db.GetConn())
            {
                using (SqlTransaction tran = conn.BeginTransaction())
                {
                    try
                    {
                        using (SqlCommand cmd = new SqlCommand(query, conn, tran))
                        {
                            cmd.Parameters.AddWithValue("@examSessionCode", examSessionCode);
                            cmd.Parameters.AddWithValue("@studentCode", studentCode);
                            cmd.Parameters.AddWithValue("@noteSubmissionRequirements", noteSubmissionRequirements);

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

        public bool HandleCheckStudentHaveEntered(int examSessionCode, string studentCode)
        {
            string query = @"SELECT StudentHaveEntered FROM ExamSession_Student
                             WHERE ExamSessionCode = @examSessionCode AND StudentCode = @studentCode AND StudentHaveEntered = 1";

            using(SqlConnection conn = _db.GetConn())
            {
                using(SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@examSessionCode", examSessionCode);
                    cmd.Parameters.AddWithValue("@studentCode", studentCode);

                    using(SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read()) return true;
                    }
                }
            }

            return false;
        }

    }
}