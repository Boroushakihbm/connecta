using Connecta.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Connecta
{
    public partial class Register : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (Session["UserId"] != null)
            //{
            //    Response.Redirect("~/Contacts/Default.aspx");
            //}
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            using (var context = new PhoneBookContext())
            {
                // بررسی تکراری نبودن نام کاربری
                if (context.Users.Any(u => u.Username == txtUsername.Text))
                {
                    lblMessage.Text = "این نام کاربری قبلاً ثبت شده است";
                    return;
                }

                // بررسی تکراری نبودن ایمیل
                if (context.Users.Any(u => u.Email == txtEmail.Text))
                {
                    lblMessage.Text = "این ایمیل قبلاً ثبت شده است";
                    return;
                }

                // بررسی تکراری نبودن شماره تلفن
                if (context.Users.Any(u => u.PhoneNumber == txtPhoneNumber.Text))
                {
                    lblMessage.Text = "این شماره تلفن قبلاً ثبت شده است";
                    return;
                }

                // ایجاد کاربر جدید
                var newUser = new User
                {
                    Username = txtUsername.Text,
                    Password = txtPassword.Text,
                    Email = txtEmail.Text,
                    PhoneNumber = txtPhoneNumber.Text,
                    FirstName = txtFirstName.Text,
                    LastName = txtLastName.Text,
                    IsAdmin = false,
                    ContactLimit = 10,
                    IsOnline = false,
                    LastSeen = DateTime.Now,
                    RegisteredDate = DateTime.Now
                };

                context.Users.Add(newUser);
                context.SaveChanges();

                lblMessage.Text = "ثبت‌نام با موفقیت انجام شد. اکنون می‌توانید وارد شوید";
                lblMessage.CssClass = "text-success";

                // پاک کردن فیلدها
                txtUsername.Text = "";
                txtPassword.Text = "";
                txtEmail.Text = "";
                txtPhoneNumber.Text = "";
                txtFirstName.Text = "";
                txtLastName.Text = "";
            }
        }
    }
}