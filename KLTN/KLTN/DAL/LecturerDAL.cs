using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
                             JOIN Department ON Lecturer.DepartmentCode = Department.DepartmentCode
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
                             JOIN Department ON Lecturer.DepartmentCode = Department.DepartmentCode
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
            string query = @"SELECT LecturerCode, FullName
                             FROM Lecturer";

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