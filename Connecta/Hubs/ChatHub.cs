using Connecta.Models;
using System;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using System.Linq;

namespace Connecta.Hubs
{
    public class ChatHub : Hub
    {
        private readonly PhoneBookContext _context = new PhoneBookContext();

        public override Task OnConnected()
        {
            var userId = Context.QueryString["userId"];
            if (!string.IsNullOrEmpty(userId) && int.TryParse(userId, out int id))
            {
                // علامت گذاری کاربر به عنوان آنلاین
                var user = _context.Users.Find(id);
                if (user != null)
                {
                    user.IsOnline = true;
                    user.LastSeen = DateTime.Now;
                    _context.SaveChanges();

                    // اطلاع رسانی به مخاطبین
                    var contactIds = _context.UserContacts
                        .Where(uc => uc.ContactUserId == id)
                        .Select(uc => uc.UserId)
                        .ToList();

                    foreach (var contactId in contactIds)
                    {
                        Clients.User(contactId.ToString()).userStatusChanged(id, true);
                    }
                }

                Groups.Add(Context.ConnectionId, userId);
            }
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var userId = Context.QueryString["userId"];
            if (!string.IsNullOrEmpty(userId) && int.TryParse(userId, out int id))
            {
                // علامت گذاری کاربر به عنوان آفلاین
                var user = _context.Users.Find(id);
                if (user != null)
                {
                    user.IsOnline = false;
                    user.LastSeen = DateTime.Now;
                    _context.SaveChanges();

                    // اطلاع رسانی به مخاطبین
                    var contactIds = _context.UserContacts
                        .Where(uc => uc.ContactUserId == id)
                        .Select(uc => uc.UserId)
                        .ToList();

                    foreach (var contactId in contactIds)
                    {
                        Clients.User(contactId.ToString()).userStatusChanged(id, false);
                    }
                }

                Groups.Remove(Context.ConnectionId, userId);
            }
            return base.OnDisconnected(stopCalled);
        }

        public void SendMessage(int receiverId, string message)
        {
            var senderId = int.Parse(Context.QueryString["userId"]);

            // بررسی آیا receiverId جزو مخاطبین sender است
            var canChat = _context.UserContacts
                .Any(uc => uc.UserId == senderId && uc.ContactUserId == receiverId && !uc.IsBlocked);

            if (!canChat)
            {
                Clients.Caller.showError("شما اجازه چت با این کاربر را ندارید.");
                return;
            }

            // ذخیره پیام در پایگاه داده
            var newMessage = new Message
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                Content = message,
                Timestamp = DateTime.Now,
                IsRead = false
            };

            _context.Messages.Add(newMessage);
            _context.SaveChanges();

            // ارسال پیام به کاربر مقصد
            Clients.User(receiverId.ToString()).receiveMessage(senderId, message, newMessage.Timestamp, newMessage.Id);

            // ارسال پیام به فرستنده برای تأیید ارسال
            Clients.Caller.messageSent(newMessage.Id);
        }

        public void MarkAsRead(int messageId)
        {
            var message = _context.Messages.Find(messageId);
            if (message != null)
            {
                message.IsRead = true;
                _context.SaveChanges();
                Clients.User(message.SenderId.ToString()).messageRead(messageId);
            }
        }

        public void SearchUsers(string query)
        {
            var userId = int.Parse(Context.QueryString["userId"]);
            var users = _context.Users
                .Where(u => u.Id != userId &&
                           (u.Username.Contains(query) || u.Email.Contains(query)))
                .Select(u => new { u.Id, u.Username, u.Email, u.IsOnline })
                .Take(10)
                .ToList();

            Clients.Caller.showSearchResults(users);
        }

        public void AddContact(int contactUserId, string nickname = null)
        {
            var userId = int.Parse(Context.QueryString["userId"]);

            // بررسی آیا قبلاً اضافه شده
            var existingContact = _context.UserContacts
                .FirstOrDefault(uc => uc.UserId == userId && uc.ContactUserId == contactUserId);

            if (existingContact != null)
            {
                Clients.Caller.showError("این کاربر قبلاً به مخاطبین شما اضافه شده است.");
                return;
            }

            var userContact = new UserContact
            {
                UserId = userId,
                ContactUserId = contactUserId,
                Nickname = nickname,
                AddedDate = DateTime.Now,
                IsBlocked = false
            };

            _context.UserContacts.Add(userContact);
            _context.SaveChanges();

            Clients.Caller.contactAdded(contactUserId);

            // اطلاع به کاربر اضافه شده (اختیاری)
            Clients.User(contactUserId.ToString()).addedToContacts(userId);
        }
        
        public void SearchUserByPhone(string phoneNumber)
        {
            var currentUserId = int.Parse(Context.QueryString["userId"]);

            using (var context = new PhoneBookContext())
            {
                // جستجوی کاربر بر اساس شماره تلفن
                var user = context.Users
                    .FirstOrDefault(u => u.PhoneNumber == phoneNumber && u.Id != currentUserId);

                if (user != null)
                {
                    // بررسی آیا قبلاً اضافه شده
                    var alreadyAdded = context.UserContacts
                        .Any(uc => uc.UserId == currentUserId && uc.ContactUserId == user.Id);

                    Clients.Caller.foundUserByPhone(user.Id, user.FullName, user.Username, user.IsOnline, alreadyAdded);
                }
                else
                {
                    Clients.Caller.userNotFoundByPhone("کاربری با این شماره تلفن یافت نشد");
                }
            }
        }

        public void NotifyContactJoined(int contactId)
        {
            var currentUserId = int.Parse(Context.QueryString["userId"]);

            using (var context = new PhoneBookContext())
            {
                // پیدا کردن تمام کاربرانی که این شخص را در مخاطبین خود دارند
                var usersWithThisContact = context.Contacts
                    .Where(c => c.PhoneNumber == context.Users.First(u => u.Id == contactId).PhoneNumber)
                    .Select(c => c.UserId)
                    .Distinct()
                    .ToList();

                foreach (var userId in usersWithThisContact)
                {
                    // ارسال通知 به کاربران
                    Clients.User(userId.ToString()).contactJoinedSystem(contactId, context.Users.Find(contactId).FullName);
                }
            }
        }
    }
}