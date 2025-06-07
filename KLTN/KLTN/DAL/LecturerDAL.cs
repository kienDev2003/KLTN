using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;

namespace KLTN.DAL
{
    public class LecturerDAL
    {
        private DB.DbConn _db;

        public LecturerDAL()
        {
            _db = new DB.DbConn();
        }

        public DataTable GetInfo(int accountCode)
        {
            DataTable data = new DataTable();
            string query = @"SELECT LecturerCode, FullName, DateOfBirth, IsLeader, DepartmentName
                             FROM Lecturer
                             WHERE AccountCode = @accountCode";

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

        public DataTable GetInfoByLecturerCode(string lecturerCode)
        {
            DataTable data = new DataTable();
            string query = @"SELECT FullName, DateOfBirth, IsLeader, DepartmentName
                             FROM Lecturer
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
                conn.Close();
            }
            return data;
        }

        public DataTable GetAllLecturer()
        {
            DataTable data = new DataTable();
            string query = @"SELECT LecturerCode, FullName, DateOfBirth, Email, DepartmentName, IsLeader
                             FROM Lecturer
                             JOIN Account ON Lecturer.AccountCode = Account.AccountCode";

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

        public bool InsertLecturer(Models.Req.Lecturer lecturer)
        {
            string query1 = @"INSERT INTO Account (UserName, Password, Email, AccountType)
                              VALUES (@userName, @password, @email, @accountType);
                              SELECT SCOPE_IDENTITY();";

            string query2 = @"INSERT INTO Lecturer (LecturerCode, FullName, DateOfBirth, IsLeader, AccountCode, DepartmentName)
                              VALUES (@lecturerCode, @fullName, @dateOfBirth, @isLeader, @accountCode, @departmentName)";

            using(SqlConnection conn = _db.GetConn())
            {
                using(SqlTransaction tran = conn.BeginTransaction())
                {
                    try
                    {
                        int accountCode = -1;

                        using (SqlCommand cmd1 = new SqlCommand(query1, conn, tran))
                        {
                            cmd1.Parameters.AddWithValue("@userName", lecturer.LecturerCode);
                            cmd1.Parameters.AddWithValue("@password", lecturer.DateOfBirth);
                            cmd1.Parameters.AddWithValue("@email", lecturer.Email);
                            cmd1.Parameters.AddWithValue("@accountType", "GV");

                            accountCode = Convert.ToInt32(cmd1.ExecuteScalar());
                        }

                        using (SqlCommand cmd2 = new SqlCommand(query2, conn, tran))
                        {
                            cmd2.Parameters.AddWithValue("@lecturerCode", lecturer.LecturerCode);
                            cmd2.Parameters.AddWithValue("@fullName", lecturer.FullName);
                            cmd2.Parameters.AddWithValue("@dateOfBirth", DateTime.ParseExact(lecturer.DateOfBirth, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                            cmd2.Parameters.AddWithValue("@accountCode", accountCode);
                            cmd2.Parameters.AddWithValue("@isLeader", false);
                            cmd2.Parameters.AddWithValue("@departmentName", lecturer.DepartmentName);

                            cmd2.ExecuteNonQuery();
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

        public bool ChangeLeader(string lecturerCode, int status)
        {
            string query = @"UPDATE Lecturer SET IsLeader = @status WHERE LecturerCode = @lecturerCode";

            using(SqlConnection conn = _db.GetConn())
            {
                using(SqlTransaction tran = conn.BeginTransaction())
                {
                    try
                    {
                        using (SqlCommand cmd = new SqlCommand(query, conn, tran))
                        {
                            cmd.Parameters.AddWithValue("@status", status);
                            cmd.Parameters.AddWithValue("@lecturerCode", lecturerCode);

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
    }
}