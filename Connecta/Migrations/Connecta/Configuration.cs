namespace Connecta.Migrations.Connecta
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Models;
    internal sealed class Configuration : DbMigrationsConfiguration<PhoneBookContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            MigrationsDirectory = @"Migrations\Connecta";
        }

        protected override void Seed(PhoneBookContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
            // ایجاد کاربر ادمین پیش‌فرض
            var adminUser = new User
            {
                Username = "admin",
                Password = "admin123", // در عمل باید هش شود
                Email = "admin@phonebook.com",
                IsAdmin = true,
                ContactLimit = 1000
            };

            // ایجاد کاربر معمولی پیش‌فرض
            var regularUser = new User
            {
                Username = "user",
                Password = "user123", // در عمل باید هش شود
                Email = "user@phonebook.com",
                IsAdmin = false,
                ContactLimit = 10
            };

            context.Users.Add(adminUser);
            context.SaveChanges();
            context.Users.Add(regularUser);
            context.SaveChanges();

            // ایجاد طرح‌های پیش‌فرض
            var basicPlan = new Plan
            {
                Name = "پایه",
                Description = "طرح پایه برای کاربران جدید",
                Price = 0,
                ContactLimit = 10,
                DurationDays = 365
            };

            var premiumPlan = new Plan
            {
                Name = "پرمیوم",
                Description = "طرح پرمیوم با قابلیت‌های بیشتر",
                Price = 10,
                ContactLimit = 100,
                DurationDays = 365
            };

            var unlimitedPlan = new Plan
            {
                Name = "نامحدود",
                Description = "طرح نامحدود برای کاربران حرفه‌ای",
                Price = 25,
                ContactLimit = 1000,
                DurationDays = 365
            };

            context.Plans.Add(basicPlan);
            context.Plans.Add(premiumPlan);
            context.Plans.Add(unlimitedPlan);
            context.SaveChanges();
            // ایجاد چند مخاطب نمونه
            var sampleContacts = new[]
            {
                new Contact { FirstName = "علی", LastName = "رضایی", PhoneNumber = "09123456789", Email = "ali@example.com", UserId = 2, User =  adminUser},
                new Contact { FirstName = "مریم", LastName = "محمدی", PhoneNumber = "09129876543", Email = "maryam@example.com", UserId = 2, User =  adminUser },
                new Contact { FirstName = "رضا", LastName = "حسینی", PhoneNumber = "09351234567", Email = "reza@example.com", UserId = 2, User =  adminUser }
            };

            foreach (var contact in sampleContacts)
            {
                context.Contacts.Add(contact);
                context.SaveChanges();
            }

            // ایجاد چند پیام نمونه
            var sampleMessages = new[]
            {
                new Message {
                    Content = "سلام، چطوری؟",
                    Timestamp = DateTime.Now.AddHours(-2),
                    SenderId = 2,
                    ReceiverId = 1,
                    IsRead = true
                },
                new Message {
                    Content = "سلام، خوبم. تو چطوری؟",
                    Timestamp = DateTime.Now.AddHours(-1),
                    SenderId = 1,
                    ReceiverId = 2,
                    IsRead = true
                },
                new Message {
                    Content = "ممنون، من هم خوبم. پروژه جدیدت چطور پیش میره؟",
                    Timestamp = DateTime.Now.AddMinutes(-30),
                    SenderId = 2,
                    ReceiverId = 1,
                    IsRead = false
                }
            };

            foreach (var message in sampleMessages)
            {
                context.Messages.Add(message);
                context.SaveChanges();
            }

            
        }
    }
}
