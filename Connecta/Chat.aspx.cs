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
                Response.Redirect("~/UserPages/Login.aspx");
            }
        }

        [WebMethod]
        public static List<UserContactInfo> GetUserContacts()
        {
            var contacts = new List<UserContactInfo>();
            var userId = int.Parse(System.Web.HttpContext.Current.Session["UserId"].ToString());

            using (var context = new PhoneBookContext())
            {
                var userContacts = context.UserContacts
                    .Where(uc => uc.UserId == userId && !uc.IsBlocked)
                    .Join(context.Users,
                          uc => uc.ContactUserId,
                          u => u.Id,
                          (uc, u) => new { UserContact = uc, User = u })
                    .ToList();

                foreach (var item in userContacts)
                {
                    contacts.Add(new UserContactInfo
                    {
                        Id = item.User.Id,
                        Username = item.User.Username,
                        Nickname = item.UserContact.Nickname,
                        IsOnline = item.User.IsOnline
                    });
                }
            }

            return contacts;
        }

        [WebMethod]
        public static List<MessageInfo> GetChatHistory(int userId)
        {
            var messages = new List<MessageInfo>();
            var currentUserId = int.Parse(System.Web.HttpContext.Current.Session["UserId"].ToString());

            using (var context = new PhoneBookContext())
            {
                var chatHistory = context.Messages
                    .Where(m => (m.SenderId == currentUserId && m.ReceiverId == userId) ||
                                (m.SenderId == userId && m.ReceiverId == currentUserId))
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

        [WebMethod]
        public static bool GetUserStatus(int userId)
        {
            using (var context = new PhoneBookContext())
            {
                var user = context.Users.Find(userId);
                return user?.IsOnline ?? false;
            }
        }
    }

    public class UserContactInfo
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Nickname { get; set; }
        public bool IsOnline { get; set; }
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