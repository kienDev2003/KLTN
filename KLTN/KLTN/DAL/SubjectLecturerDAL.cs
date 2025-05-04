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
    }
}