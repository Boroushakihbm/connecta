using Connecta.Models;
using System;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Connecta.Contacts
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UserId"] == null)
                {
                    Response.Redirect("~/UserPages/Login.aspx");
                }

                BindContacts();
                CheckContactLimit();
            }
        }

        [WebMethod]
        public static string UpdateContact(string id, string firstName, string lastName, string phoneNumber, string email)
        {
            try
            {
                int contactId = int.Parse(id);
                int userId = (int)HttpContext.Current.Session["UserId"];

                using (var context = new PhoneBookContext())
                {
                    var contact = context.Contacts.Find(contactId);
                    if (contact != null && contact.UserId == userId)
                    {
                        contact.FirstName = firstName;
                        contact.LastName = lastName;
                        contact.PhoneNumber = phoneNumber;
                        contact.Email = email;

                        context.SaveChanges();
                        return "success";
                    }
                }
            }
            catch (Exception ex)
            {
                // ثبت خطا
                System.Diagnostics.Debug.WriteLine($"Error updating contact: {ex.Message}");
            }

            return "error";
        }

        [WebMethod]
        public static string DeleteContact(string id)
        {
            try
            {
                int contactId = int.Parse(id);
                int userId = (int)HttpContext.Current.Session["UserId"];

                using (var context = new PhoneBookContext())
                {
                    var contact = context.Contacts.Find(contactId);
                    if (contact != null && contact.UserId == userId)
                    {
                        context.Contacts.Remove(contact);
                        context.SaveChanges();
                        return "success";
                    }
                }
            }
            catch (Exception ex)
            {
                // ثبت خطا
                System.Diagnostics.Debug.WriteLine($"Error deleting contact: {ex.Message}");
            }

            return "error";
        }

        private void BindContacts()
        {
            int userId = (int)Session["UserId"];

            using (var context = new PhoneBookContext())
            {
                var contacts = context.Contacts.Where(c => c.UserId == userId).ToList();

                rptContacts.DataSource = contacts;
                rptContacts.DataBind();

                litContactCount.Text = contacts.Count.ToString();
                litContactLimit.Text = Session["ContactLimit"]?.ToString() ?? "10";
            }
        }

        private void CheckContactLimit()
        {
            int userId = (int)Session["UserId"];
            int contactLimit = Session["ContactLimit"] != null ? (int)Session["ContactLimit"] : 10;

            using (var context = new PhoneBookContext())
            {
                int contactCount = context.Contacts.Count(c => c.UserId == userId);

                if (contactCount >= contactLimit)
                {
                    limitAlert.Visible = true;
                }
                else
                {
                    limitAlert.Visible = false;
                }
            }
        }

        protected void btnSearchPhone_Click(object sender, EventArgs e)
        {
            string phoneNumber = txtSearchPhone.Text.Trim();

            using (var context = new PhoneBookContext())
            {
                int currentUserId = (int)Session["UserId"];
                var user = context.Users.FirstOrDefault(u => u.PhoneNumber == phoneNumber && u.Id != currentUserId);

                if (user != null)
                {
                    bool alreadyAdded = context.UserContacts
                        .Any(uc => uc.UserId == currentUserId && uc.ContactUserId == user.Id);

                    string resultHtml = $@"
                <div class='alert alert-info'>
                    <h5>نتایج جستجو</h5>
                    <p><strong>نام:</strong> {user.FullName}</p>
                    <p><strong>نام کاربری:</strong> {user.Username}</p>
                    <p><strong>وضعیت:</strong> {(user.IsOnline ? "آنلاین" : "آفلاین")}</p>
                    {(alreadyAdded ?
                                "<div class='text-success'>این کاربر قبلاً به لیست چت شما اضافه شده است</div>" :
                                $"<button class='btn btn-success mt-2' onclick='addToChatContacts({user.Id})'>افزودن به لیست چت</button>")}
                </div>";

                    searchResults.InnerHtml = resultHtml;
                }
                else
                {
                    searchResults.InnerHtml = "<div class='alert alert-warning'>کاربری با این شماره تلفن در سیستم یافت نشد</div>";
                }
            }
        }

        protected void btnSaveContact_Click(object sender, EventArgs e)
        {
            int userId = (int)Session["UserId"];

            try
            {
                using (var context = new PhoneBookContext())
                {
                    var user = context.Users.FirstOrDefault(x => x.Id == userId);
                    var newContact = new Contact
                    {
                        FirstName = txtNewFirstName.Text,
                        LastName = txtNewLastName.Text,
                        PhoneNumber = txtNewPhone.Text,
                        Email = txtNewEmail.Text,
                        UserId = userId,
                        User = user
                    };

                    context.Contacts.Add(newContact);
                    context.SaveChanges();
                }
            }
            catch (DbEntityValidationException ex)
            {
                string errorMessage = "خطا در ذخیره اطلاعات: ";
                foreach (var eve in ex.EntityValidationErrors)
                {
                    foreach (var ve in eve.ValidationErrors)
                    {
                        errorMessage += ve.ErrorMessage + " ";
                    }
                }
                ScriptManager.RegisterStartupScript(this, GetType(), "showError", $"alert('{errorMessage}');", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showError", "alert('خطا در ذخیره اطلاعات');", true);
            }

            // پاک کردن فیلدها
            txtNewFirstName.Text = "";
            txtNewLastName.Text = "";
            txtNewPhone.Text = "";
            txtNewEmail.Text = "";

            // بارگذاری مجدد داده‌ها
            BindContacts();
            CheckContactLimit();

            // بستن مودال
            ScriptManager.RegisterStartupScript(this, GetType(), "closeModal",
                "if ($('#addContactModal').length) $('#addContactModal').modal('hide');", true);
        }
    }
}