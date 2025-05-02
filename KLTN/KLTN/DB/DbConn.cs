using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace KLTN.DB
{
    public class DbConn
    {
        private readonly string strConn = ConfigurationManager.ConnectionStrings["strConn"].ConnectionString;
        
        public SqlConnection GetConn()
        {
            SqlConnection conn = new SqlConnection(strConn);
            conn.Open();

            return conn;
        }
    }
}