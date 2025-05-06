using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;

namespace KLTN.DAL
{
    public class SubjectLecturerDAL
    {
        private DB.DbConn _db;

        public SubjectLecturerDAL()
        {
            _db = new DB.DbConn();
        }

        public DataTable GetSubjectLecturer(int accountCode)
        {
            DataTable data = new DataTable();
            string query = @"SELECT Subject.SubjectCode, Subject.SubjectName 
                             FROM Subject
                             JOIN Subject_Lecturer ON Subject.SubjectCode = Subject_Lecturer.SubjectCode
                             JOIN Lecturer ON Subject_Lecturer.LecturerCode = Lecturer.LecturerCode
                             JOIN Account ON Lecturer.AccountCode = Account.AccountCode
                             WHERE Account.AccountCode = @accountCode";

            using (SqlConnection conn = _db.GetConn())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@accountCode", accountCode);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(data);
                    }

                }
                conn.Close();
            }
            return data;
        }
        public DataTable GetSubjectNotLecturer()
        {
            DataTable data = new DataTable();
            string query = @"SELECT SubjectCode, SubjectName
                             FROM Subject
                             WHERE SubjectCode NOT IN (SELECT DISTINCT SubjectCode FROM Subject_Lecturer)
                             AND Subject.IsDelete = 0";

            using(SqlConnection conn = _db.GetConn())
            {
                using(SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using(SqlDataAdapter  adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(data);
                    }
                }
            }

            return data;
        }
        public DataTable GetSubjectLecturerByLecturerCode(string lecturerCode)
        {
            DataTable data = new DataTable();
            string query = @"SELECT Subject_Lecturer.SubjectCode, SubjectName
                             FROM Subject_Lecturer
                             JOIN Subject ON Subject_Lecturer.SubjectCode = Subject.SubjectCode
                             WHERE LecturerCode = @lecturerCode";

            using (SqlConnection conn = _db.GetConn())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@lecturerCode", lecturerCode);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(data);
                    }
                }
            }

            return data;
        }
        public bool InsertSubjectLecturer(Models.Req.SubjectTeaching subjectLecturer)
        {
            string query = @"INSERT INTO Subject_Lecturer (SubjectCode, LecturerCode)
                             VALUES (@subjectCode, @lecturerCode)";

            using(SqlConnection conn = _db.GetConn())
            {
                using(SqlTransaction tran = conn.BeginTransaction())
                {
                    try
                    {
                        using (SqlCommand cmd = new SqlCommand(query, conn, tran))
                        {
                            cmd.Parameters.AddWithValue("@subjectCode", subjectLecturer.SubjectCode);
                            cmd.Parameters.AddWithValue("@lecturerCode", subjectLecturer.LecturerCode);

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
        public bool DeleteSubjectLecturer(Models.Req.SubjectTeaching subjectLecturer)
        {
            string query = @"DELETE Subject_Lecturer WHERE SubjectCode = @subjectCode AND LecturerCode = @lecturerCode";

            using (SqlConnection conn = _db.GetConn())
            {
                using (SqlTransaction tran = conn.BeginTransaction())
                {
                    try
                    {
                        using (SqlCommand cmd = new SqlCommand(query, conn, tran))
                        {
                            cmd.Parameters.AddWithValue("@subjectCode", subjectLecturer.SubjectCode);
                            cmd.Parameters.AddWithValue("@lecturerCode", subjectLecturer.LecturerCode);

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

                return false;
            }
        }
    }
}