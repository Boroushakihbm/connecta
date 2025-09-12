using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Connecta.Models
{
    public class Message
    {
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime Timestamp { get; set; }

        public int SenderId { get; set; }
        public virtual User Sender { get; set; }

        public int ReceiverId { get; set; }
        public virtual User Receiver { get; set; }

        public bool IsRead { get; set; }

        public MessageType MessageType { get; set; } = MessageType.Text;
    }

    public enum MessageType
    {
        Text,
        Image,
        File
    }

}