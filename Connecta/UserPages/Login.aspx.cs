using Connecta.Models;
using System;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Connecta.UserPages
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Username"] != null)
            {
                Response.Redirect(Session["IsAdmin"] != null && (bool)Session["IsAdmin"] ?
                    "~/Admin/Dashboard.aspx" : "~/Contacts/Default.aspx");
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            using (var context = new PhoneBookContext())
            {
                var user = context.Users.FirstOrDefault(u => u.Username == txtUsername.Text && u.Password == txtPassword.Text);

                if (user != null)
                {
                    // ایجاد تیکت احراز هویت
                    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                        1,
                        user.Username,
                        DateTime.Now,
                        DateTime.Now.AddMinutes(30),
                        chkRemember.Checked,
                        user.IsAdmin ? "Admin" : "User",
                        FormsAuthentication.FormsCookiePath);

                    string encryptedTicket = FormsAuthentication.Encrypt(ticket);
                    HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                    Response.Cookies.Add(authCookie);

                    // ذخیره اطلاعات کاربر در سشن
                    Session["UserId"] = user.Id;
                    Session["Username"] = user.Username;
                    Session["IsAdmin"] = user.IsAdmin;

                    Application.Lock();
                    Application["OnlineUsers"] = ((int)Application["OnlineUsers"]) + 1;
                    Application.UnLock();

                    Response.Redirect(user.IsAdmin ? "~/Admin/Dashboard.aspx" : "~/Contacts/Default.aspx");
                }
                else
                {
                    lblMessage.Text = "نام کاربری یا کلمه عبور اشتباه است";
                }
            }
        }
    }
}