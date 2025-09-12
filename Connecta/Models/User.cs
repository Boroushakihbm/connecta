using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using Connecta.Models;

namespace Connecta.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        [StringLength(100)]
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        //[Phone]
        public string PhoneNumber { get; set; }

        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}".Trim();

        public bool IsAdmin { get; set; }

        public int ContactLimit { get; set; }

        public bool IsOnline { get; set; }

        public DateTime LastSeen { get; set; }

        public DateTime RegisteredDate { get; set; }

        // ارتباطات
        public virtual ICollection<Contact> Contacts { get; set; }
        public virtual ICollection<Message> SentMessages { get; set; }
        public virtual ICollection<Message> ReceivedMessages { get; set; }
        public virtual ICollection<UserContact> UserContacts { get; set; }

        public User()
        {
            Contacts = new HashSet<Contact>();
            SentMessages = new HashSet<Message>();
            ReceivedMessages = new HashSet<Message>();
            UserContacts = new HashSet<UserContact>();
            LastSeen = DateTime.Now;
            RegisteredDate = DateTime.Now;
        }
    }
}