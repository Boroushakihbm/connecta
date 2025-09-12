using System.Data.Entity;
using System.Numerics;

namespace Connecta.Models
{
    public class PhoneBookContext : DbContext
    {
        public PhoneBookContext() : base("ConnectaDB")
        {
        }
        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // تنظیمات رابطه برای UserContact
            modelBuilder.Entity<UserContact>()
                .HasRequired(uc => uc.User)
                .WithMany(u => u.UserContacts)
                .HasForeignKey(uc => uc.UserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserContact>()
                .HasRequired(uc => uc.ContactUser)
                .WithMany()
                .HasForeignKey(uc => uc.ContactUserId)
                .WillCascadeOnDelete(false);

            // تنظیمات رابطه برای Message
            modelBuilder.Entity<Message>()
                .HasRequired(m => m.Sender)
                .WithMany(u => u.SentMessages)
                .HasForeignKey(m => m.SenderId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Message>()
                .HasRequired(m => m.Receiver)
                .WithMany(u => u.ReceivedMessages)
                .HasForeignKey(m => m.ReceiverId)
                .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }


        public DbSet<User> Users { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<UserPlan> UserPlans { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<UserContact> UserContacts { get; set; }

    }
}