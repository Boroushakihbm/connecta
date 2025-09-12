using System;



namespace Connecta.Models
{
    public class UserContact
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }

        public int ContactUserId { get; set; }
        public virtual User ContactUser { get; set; }

        public string Nickname { get; set; }

        public DateTime AddedDate { get; set; }

        public bool IsBlocked { get; set; }
    }
}