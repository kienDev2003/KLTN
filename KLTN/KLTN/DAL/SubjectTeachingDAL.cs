using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace KLTN.DAL
{
    public class SubjectTeachingDAL
    {
        private DB.DbConn _dB;

        public SubjectTeachingDAL()
        {
            _dB = new DB.DbConn();
        }

        public DataTable GetSubjectTeaching(int accountCode)
        {
            DataTable data = new DataTable();
            string query = @"SELECT Subject.SubjectCode, Subject.SubjectName 
                             FROM Subject
                             JOIN Subject_Teaching ON Subject.SubjectCode = Subject_Teaching.SubjectCode
                             JOIN Lecturer ON Subject_Teaching.LecturerCode = Lecturer.LecturerCode
                             JOIN Account ON Lecturer.AccountCode = Account.AccountCode
                             WHERE Account.AccountCode = @accountCode AND Subject.IsDelete = 0";

            using (SqlConnection conn = _dB.GetConn())
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

        public DataTable GetSubjectTeachingByLecturerCode(string lecturerCode)
        {
            DataTable data = new DataTable();
            string query = @"SELECT SubjectCode 
                             FROM Subject_Teaching
                             WHERE LecturerCode = @lecturerCode";

            using (SqlConnection conn = _dB.GetConn())
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

        public bool InsertSubjectTeaching(Models.Req.SubjectTeaching subjectTeaching)
        {
            string query = @"INSERT INTO Subject_Teaching (LecturerCode, SubjectCode)
                             VALUES (@lecturerCode, @subjectCode)";

            using (SqlConnection conn = _dB.GetConn())
            {
                using (SqlTransaction tran = conn.BeginTransaction())
                {
                    try
                    {
                        using (SqlCommand cmd = new SqlCommand(query, conn, tran))
                        {
                            cmd.Parameters.AddWithValue("@lecturerCode", subjectTeaching.LecturerCode);
                            cmd.Parameters.AddWithValue("@subjectCode", subjectTeaching.SubjectCode);

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

        public bool DeleteSubjectTeaching(Models.Req.SubjectTeaching subjectTeaching)
        {
            string query = @"DELETE Subject_Teaching
                             WHERE LecturerCode = @lecturerCode AND SubjectCode = @subjectCode";

            using (SqlConnection conn = _dB.GetConn())
            {
                using (SqlTransaction tran = conn.BeginTransaction())
                {
                    try
                    {
                        using (SqlCommand cmd = new SqlCommand(query, conn, tran))
                        {
                            cmd.Parameters.AddWithValue("@lecturerCode", subjectTeaching.LecturerCode);
                            cmd.Parameters.AddWithValue("@subjectCode", subjectTeaching.SubjectCode);

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
    }
}