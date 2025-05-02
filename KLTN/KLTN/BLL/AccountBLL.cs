using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using KLTN.DAL;

namespace KLTN.BLL
{
    public class AccountBLL
    {
        private AccountDAL _accountDAL;

        public AccountBLL()
        {
            _accountDAL = new AccountDAL();
        }

        public Models.Res.Login Login(string userName, string password)
        {
            Models.Req.Login login = new Models.Req.Login()
            {
                userName = userName,
                password = password
            };
            DataTable data = _accountDAL.Login(login);

            if(data.Rows.Count <= 0 ) return null;

            Models.Res.Login loginRes = new Models.Res.Login();
            foreach(DataRow row in data.Rows)
            {
                loginRes.accountCode = Convert.ToInt32(row["AccountCode"]);
                loginRes.accountType = row["AccountType"].ToString();
            }

            return loginRes;
        }
    }
}