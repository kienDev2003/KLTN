using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KLTN.BLL;

namespace KLTN.pages
{
    public partial class index : System.Web.UI.Page
    {
        private AccountBLL _accountBLL = new AccountBLL();
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }
        protected void HandleLogin(object sender, EventArgs e)
        {
            string userName = txt_userName.Value;
            string password = txt_password.Value;

            if(string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showError", "showError('Tài khoản / Mật khẩu không được để trống !');", true);
                return;
            }

            Models.Res.Login loginRes = _accountBLL.Login(userName, password);

            if(loginRes == null)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showError", "showError('Tài khoản / Mật khẩu không đúng !');", true);
                return;
            }

            Session["login"] = loginRes;
            Response.Redirect("~/pages/home.aspx");
        }
    }
}