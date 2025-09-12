using Connecta.Models;
using System;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace Connecta.Hubs
{
    public class ChatHub : Hub
    {
        private readonly PhoneBookContext _context = new PhoneBookContext();

        public void SendMessage(int receiverId, string message)
        {
            var senderId = int.Parse(Context.QueryString["userId"]);

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
            Clients.User(receiverId.ToString()).receiveMessage(senderId, message, newMessage.Timestamp);

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
            }
        }

        public override Task OnConnected()
        {
            var userId = Context.QueryString["userId"];
            if (!string.IsNullOrEmpty(userId))
            {
                Groups.Add(Context.ConnectionId, userId);
            }
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var userId = Context.QueryString["userId"];
            if (!string.IsNullOrEmpty(userId))
            {
                Groups.Remove(Context.ConnectionId, userId);
            }
            return base.OnDisconnected(stopCalled);
        }
    }
}