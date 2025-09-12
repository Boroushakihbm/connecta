using Connecta.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
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
                    Response.Redirect("~/Login.aspx");
                }

                BindContacts();
                CheckContactLimit();
            }
        }

        private void BindContacts()
        {
            int userId = (int)Session["UserId"];

            using (var context = new PhoneBookContext())
            {
                var contacts = context.Contacts.Where(c => c.UserId == userId).ToList();
                gvContacts.DataSource = contacts;
                gvContacts.DataBind();

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
                    btnAddContact.Enabled = false;
                    btnAddContact.CssClass = "btn btn-secondary";
                }
                else
                {
                    limitAlert.Visible = false;
                    btnAddContact.Enabled = true;
                    btnAddContact.CssClass = "btn btn-primary";
                }
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            int userId = (int)Session["UserId"];
            string searchTerm = txtSearch.Text.Trim();

            using (var context = new PhoneBookContext())
            {
                var contacts = context.Contacts.Where(c => c.UserId == userId &&
                    (c.FirstName.Contains(searchTerm) || c.LastName.Contains(searchTerm) ||
                     c.PhoneNumber.Contains(searchTerm) || c.Email.Contains(searchTerm))).ToList();

                gvContacts.DataSource = contacts;
                gvContacts.DataBind();
            }
        }

        protected void btnAddContact_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(
                this,
                this.GetType(),
                "ShowAddContactModal",
                "var myModal = new bootstrap.Modal(document.getElementById('addContactModal')); myModal.show();",
                true
            );

        }

        protected void btnSaveContact_Click(object sender, EventArgs e)
        {
            int userId = (int)Session["UserId"];

            try
            {
                using (var context = new PhoneBookContext())
                {
                    var a = context.Users.Where(x => x.Id == userId).FirstOrDefault();
                    var newContact = new Contact
                    {
                        FirstName = txtNewFirstName.Text,
                        LastName = txtNewLastName.Text,
                        PhoneNumber = txtNewPhone.Text,
                        Email = txtNewEmail.Text,
                        UserId = userId,
                        User = a
                    };

                    context.Contacts.Add(newContact);
                    context.SaveChanges();
                }
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var eve in ex.EntityValidationErrors)
                {
                    Console.WriteLine($"Entity of type {eve.Entry.Entity.GetType().Name} in state {eve.Entry.State} has validation errors:");

                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine($"- Property: {ve.PropertyName}, Error: {ve.ErrorMessage}");
                    }
                }
                throw;
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
            

            // پاک کردن فیلدها
            txtNewFirstName.Text = "";
            txtNewLastName.Text = "";
            txtNewPhone.Text = "";
            txtNewEmail.Text = "";

            // بارگذاری مجدد داده‌ها
            BindContacts();
            CheckContactLimit();
        }

        protected void gvContacts_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvContacts.EditIndex = e.NewEditIndex;
            BindContacts();
        }

        protected void gvContacts_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int contactId = Convert.ToInt32(gvContacts.DataKeys[e.RowIndex].Value);

            using (var context = new PhoneBookContext())
            {
                var contact = context.Contacts.Find(contactId);

                if (contact != null)
                {
                    contact.FirstName = (gvContacts.Rows[e.RowIndex].FindControl("txtFirstName") as TextBox)?.Text;
                    contact.LastName = (gvContacts.Rows[e.RowIndex].FindControl("txtLastName") as TextBox)?.Text;
                    contact.PhoneNumber = (gvContacts.Rows[e.RowIndex].FindControl("txtPhoneNumber") as TextBox)?.Text;
                    contact.Email = (gvContacts.Rows[e.RowIndex].FindControl("txtEmail") as TextBox)?.Text;

                    context.SaveChanges();
                }
            }

            gvContacts.EditIndex = -1;
            BindContacts();
        }

        protected void gvContacts_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvContacts.EditIndex = -1;
            BindContacts();
        }

        protected void gvContacts_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int contactId = Convert.ToInt32(gvContacts.DataKeys[e.RowIndex].Value);

            using (var context = new PhoneBookContext())
            {
                var contact = context.Contacts.Find(contactId);

                if (contact != null)
                {
                    context.Contacts.Remove(contact);
                    context.SaveChanges();
                }
            }

            BindContacts();
            CheckContactLimit();
        }
    }

}