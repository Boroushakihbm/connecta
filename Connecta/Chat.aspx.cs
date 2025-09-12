using Connecta.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Services;
using System.Web.UI.WebControls;

namespace Connecta
{
    public partial class Chat : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] == null)
            {
                Response.Redirect("~/Login.aspx");
            }

            if (!IsPostBack)
            {
                LoadContacts();
            }
        }

        private void LoadContacts()
        {
            int userId = (int)Session["UserId"];

            using (var context = new PhoneBookContext())
            {
                var contacts = context.Contacts.Where(c => c.UserId == userId).ToList();
                lstContacts.DataSource = contacts;
                lstContacts.DataTextField = "FullName";
                lstContacts.DataValueField = "Id";
                lstContacts.DataBind();
            }
        }

        protected void lstContacts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstContacts.SelectedItem != null)
            {
                int contactId = int.Parse(lstContacts.SelectedValue);
                string contactName = lstContacts.SelectedItem.Text;

                // ثبت در session برای دسترسی از JavaScript
                Session["SelectedContactId"] = contactId;
                Session["SelectedContactName"] = contactName;
            }
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            // ارسال پیام از طریق SignalR انجام می‌شود
            // این متد برای پست‌بک سنتی نگه داشته شده است
        }

        [WebMethod]
        public static List<MessageInfo> GetChatHistory(int contactId)
        {
            var messages = new List<MessageInfo>();
            var userId = int.Parse(System.Web.HttpContext.Current.Session["UserId"].ToString());

            using (var context = new PhoneBookContext())
            {
                var chatHistory = context.Messages
                    .Where(m => (m.SenderId == userId && m.ReceiverId == contactId) ||
                                (m.SenderId == contactId && m.ReceiverId == userId))
                    .OrderBy(m => m.Timestamp)
                    .ToList();

                foreach (var message in chatHistory)
                {
                    messages.Add(new MessageInfo
                    {
                        Id = message.Id,
                        SenderId = message.SenderId,
                        ReceiverId = message.ReceiverId,
                        Content = message.Content,
                        Timestamp = message.Timestamp,
                        IsRead = message.IsRead
                    });
                }
            }

            return messages;
        }
    }

    public class MessageInfo
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
        public bool IsRead { get; set; }
    }

}