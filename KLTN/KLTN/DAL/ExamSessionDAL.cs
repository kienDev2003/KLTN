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
            string query = @"INSERT INTO ExamSession (SubjectCode, StartExamDate, EndExamDate, CreateByLecturer, ExamPaperCode, ExamSessionPassword, IsAssessment)
                             VALUES (@subjectCode, @startExamDate, @endExamDate, @createByLecturer, @examPaperCode, @examSessionPassword, 0);";

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
    }
}