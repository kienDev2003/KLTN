using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace KLTN.DAL
{
    public class SubjectDAL
    {
        private DB.DbConn _db;
        public SubjectDAL()
        {
            _db = new DB.DbConn();
        }

        public DataTable GetInfoBySubjectCode(string subjectCode)
        {
            DataTable data = new DataTable();
            string query = @"SELECT SubjectCode, SubjectName, NumberOfCredits
                             FROM Subject
                             WHERE Subject.SubjectCode = @subjectCode";

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
                conn.Close();
            }
            return data;

        }

        public DataTable GetChapterBySubjectCode(string subjectCode)
        {
            DataTable data = new DataTable();
            string query = @"SELECT ChapterCode, ChapterName
                             FROM Chapter
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

        public DataTable GetSubjectAll()
        {
            DataTable data = new DataTable();
            string query = @"SELECT 
                                SubjectCode, 
                                SubjectName, 
                                NumberOfCredits,
                                CreateDate,
                                (SELECT COUNT(ChapterCode) FROM Chapter WHERE Chapter.SubjectCode = Subject.SubjectCode) AS NumberChapter
                            FROM Subject
                            WHERE IsDelete = 0
                            ORDER BY CreateDate DESC";

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

        public bool InsertSubject(Models.Req.Subject subject)
        {
            string query1 = @"INSERT INTO Subject (SubjectCode, SubjectName, NumberOfCredits, CreateDate, IsDelete)
                              VALUES (@subjectCode, @subjectName, @numberOfCredits, @createDate, 0)";

            string query2 = @"INSERT INTO Chapter (ChapterName, SubjectCode)
                              VALUES (@chapterName, @subjectCode)";

            using (SqlConnection conn = _db.GetConn())
            {
                using (SqlTransaction tran = conn.BeginTransaction())
                {
                    try
                    {
                        using (SqlCommand cmd1 = new SqlCommand(query1, conn, tran))
                        {
                            cmd1.Parameters.AddWithValue("@subjectName", subject.SubjectName);
                            cmd1.Parameters.AddWithValue("@subjectCode", subject.SubjectCode);
                            cmd1.Parameters.AddWithValue("@numberOfCredits", subject.NumberOfCredits);
                            cmd1.Parameters.AddWithValue("@createDate", subject.CreateDate);

                            cmd1.ExecuteNonQuery();
                        }

                        foreach (var chapter in subject.Chapters)
                        {
                            chapter.SubjectCode = subject.SubjectCode;

                            using (SqlCommand cmd2 = new SqlCommand(query2, conn, tran))
                            {
                                cmd2.Parameters.AddWithValue("@chapterName", chapter.ChapterName);
                                cmd2.Parameters.AddWithValue("@subjectCode", chapter.SubjectCode);

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

        public bool DeleteSubject(string subjectCode)
        {
            string query = @"UPDATE Subject SET IsDelete = 1 WHERE SubjectCode = @subjectCode";

            using (SqlConnection conn = _db.GetConn())
            {
                using (SqlTransaction tran = conn.BeginTransaction())
                {
                    try
                    {
                        using (SqlCommand cmd = new SqlCommand(query, conn, tran))
                        {
                            cmd.Parameters.AddWithValue("@subjectCode", subjectCode);

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