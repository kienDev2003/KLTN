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
                             WHERE Account.AccountCode = @accountCode";

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
    }
}