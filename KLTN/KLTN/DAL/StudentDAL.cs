using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;

namespace KLTN.DAL
{
    public class StudentDAL
    {
        private DB.DbConn _db;

        public StudentDAL()
        {
            _db = new DB.DbConn();
        }
        public DataTable GetInfo(int accountCode)
        {
            DataTable data = new DataTable();
            string query = @"SELECT StudentCode, FullName, DateOfBirth, Email, ClassName, MajorName
                             FROM Student
                             JOIN Account ON Account.AccountCode = Student.AccountCode
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

        public bool InsertStudent(Models.Req.Student student)
        {
            string query1 = @"INSERT INTO Account (UserName, Password, Email, AccountType)
                              VALUES (@userName, @password, @email, @accountType);
                              SELECT SCOPE_IDENTITY();";

            string query2 = @"INSERT INTO Student (StudentCode, FullName, DateOfBirth, ClassName, AccountCode, MajorName)
                              VALUES (@studentCode, @fullName, @dateOfBirth, @className, @accountCode, @majorName)";

            using (SqlConnection conn = _db.GetConn())
            {
                using (SqlTransaction tran = conn.BeginTransaction())
                {
                    try
                    {
                        int accountCode = -1;

                        using (SqlCommand cmd1 = new SqlCommand(query1, conn, tran))
                        {
                            cmd1.Parameters.AddWithValue("@userName", student.StudentCode);
                            cmd1.Parameters.AddWithValue("@password", student.DateOfBirth);
                            cmd1.Parameters.AddWithValue("@email", student.Email);
                            cmd1.Parameters.AddWithValue("@accountType", "SV");

                            accountCode = Convert.ToInt32(cmd1.ExecuteScalar());
                        }

                        using (SqlCommand cmd2 = new SqlCommand(query2, conn, tran))
                        {
                            cmd2.Parameters.AddWithValue("@studentCode", student.StudentCode);
                            cmd2.Parameters.AddWithValue("@fullName", student.FullName);
                            cmd2.Parameters.AddWithValue("@dateOfBirth", DateTime.ParseExact(student.DateOfBirth, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                            cmd2.Parameters.AddWithValue("@accountCode", accountCode);
                            cmd2.Parameters.AddWithValue("@className", student.ClassName);
                            cmd2.Parameters.AddWithValue("@majorName", student.MajorName);

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

        public DataTable GetAllStudent()
        {
            DataTable data = new DataTable();
            string query = @"SELECT StudentCode, FullName, DateOfBirth, Email, ClassName, MajorName
                             FROM Student
                             JOIN Account ON Student.AccountCode = Account.AccountCode";

            using (SqlConnection conn = _db.GetConn())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(data);
                    }
                }
                conn.Close();
            }
            return data;
        }

        public DataTable GetStudentByStudentCode(string studentCode)
        {
            DataTable data = new DataTable();
            string query = @"SELECT * FROM Student WHERE StudentCode = @studentCode";

            using (SqlConnection conn = _db.GetConn())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@studentCode", studentCode);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(data);
                    }
                }
                conn.Close();
            }
            return data;
        }
    }
}