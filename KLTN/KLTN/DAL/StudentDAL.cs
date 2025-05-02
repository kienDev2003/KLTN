using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
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
            string query = @"SELECT StudentCode, FullName, DateOfBirth, ClassName, MajorName
                             FROM Student
                             JOIN Major ON Student.MajorCode = Major.MajorCode
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
    }
}