using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace KLTN.DAL
{
    public class AccountDAL
    {
        private DB.DbConn _dB;
        
        public AccountDAL()
        {
            _dB = new DB.DbConn();
        }

        public DataTable Login(Models.Req.Login login)
        {
            DataTable data = new DataTable();
            string query = @"SELECT AccountCode, AccountType
                             FROM Account
                             WHERE UserName = @userName AND Password = @password";

            using(SqlConnection conn = _dB.GetConn())
            {
                using(SqlCommand cmd = new SqlCommand(query,conn))
                {
                    cmd.Parameters.AddWithValue("@userName", login.userName);
                    cmd.Parameters.AddWithValue("@password", login.password);

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