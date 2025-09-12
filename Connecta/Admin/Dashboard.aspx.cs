using Connecta.Models;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace Connecta.Admin
{
    public partial class Dashboard : System.Web.UI.Page
    {
        public int RegularUsersCount { get; set; }
        public int AdminUsersCount { get; set; }
        public int TotalUsersCount { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["IsAdmin"] == null || !(bool)Session["IsAdmin"])
            {
                Response.Redirect("~/Login.aspx");
            }

            if (!IsPostBack)
            {
                BindUsers();
                LoadStatistics();
            }
        }

        private void BindUsers()
        {
            using (var context = new PhoneBookContext())
            {
                var users = context.Users.ToList();
                gvUsers.DataSource = users;
                gvUsers.DataBind();

                // نمایش آخرین کاربران
                var recentUsers = users.OrderByDescending(u => u.Id).Take(5).ToList();
                gvRecentUsers.DataSource = recentUsers;
                gvRecentUsers.DataBind();
            }
        }

        private void LoadStatistics()
        {
            using (var context = new PhoneBookContext())
            {
                TotalUsersCount = context.Users.Count();
                AdminUsersCount = context.Users.Count(u => u.IsAdmin);
                RegularUsersCount = TotalUsersCount - AdminUsersCount;

                int totalContacts = context.Contacts.Count();

                litTotalUsers.Text = TotalUsersCount.ToString();
                litTotalContacts.Text = totalContacts.ToString();
                litActiveUsers.Text = TotalUsersCount.ToString(); // در این نسخه ساده، همه کاربران فعال در نظر گرفته می‌شوند
                litAdminUsers.Text = AdminUsersCount.ToString();
            }
        }

        protected void btnAddUser_Click(object sender, EventArgs e)
        {
            // در این نسخه، افزودن کاربر از طریق seed data انجام می‌شود
            // می‌توانید این بخش را扩展 دهید
        }

        protected void gvUsers_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvUsers.EditIndex = e.NewEditIndex;
            BindUsers();
        }

        protected void gvUsers_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int userId = Convert.ToInt32(gvUsers.DataKeys[e.RowIndex].Value);

            using (var context = new PhoneBookContext())
            {
                var user = context.Users.Find(userId);

                if (user != null)
                {
                    user.Email = (gvUsers.Rows[e.RowIndex].FindControl("txtEmail") as TextBox)?.Text;

                    var chkIsAdmin = gvUsers.Rows[e.RowIndex].FindControl("chkIsAdminEdit") as CheckBox;
                    if (chkIsAdmin != null)
                    {
                        user.IsAdmin = chkIsAdmin.Checked;
                    }

                    var txtContactLimit = gvUsers.Rows[e.RowIndex].FindControl("txtContactLimit") as TextBox;
                    if (txtContactLimit != null && int.TryParse(txtContactLimit.Text, out int contactLimit))
                    {
                        user.ContactLimit = contactLimit;
                    }

                    context.SaveChanges();
                }
            }

            gvUsers.EditIndex = -1;
            BindUsers();
        }

        protected void gvUsers_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvUsers.EditIndex = -1;
            BindUsers();
        }

        protected void gvUsers_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int userId = Convert.ToInt32(gvUsers.DataKeys[e.RowIndex].Value);

            using (var context = new PhoneBookContext())
            {
                var user = context.Users.Find(userId);

                if (user != null)
                {
                    // حذف مخاطبین کاربر قبل از حذف کاربر
                    var userContacts = context.Contacts.Where(c => c.UserId == userId).ToList();
                    context.Contacts.RemoveRange(userContacts);

                    context.Users.Remove(user);
                    context.SaveChanges();
                }
            }

            BindUsers();
            LoadStatistics();
        }
    }
}