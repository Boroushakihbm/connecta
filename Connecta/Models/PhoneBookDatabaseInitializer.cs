using System;
using System.Linq;

namespace Connecta.Models
{
    public class PhoneBookDatabaseInitializer 
    {
        public static void Seed(PhoneBookContext context)
        {
            if (context.Users.Where(x => x.Username == "admin").Count() > 0) return;

            // در متد Seed
            // ایجاد کاربران نمونه با شماره تلفن
            var sampleUsers = new[]
            {
                new User
                {
                    Username = "admin",
                    Password = "admin123",
                    Email = "admin@phonebook.com",
                    PhoneNumber = "09123456789",
                    FirstName = "مدیر",
                    LastName = "سیستم",
                    IsAdmin = true,
                    ContactLimit = 1000
                },
                new User
                {
                    Username = "user1",
                    Password = "user123",
                    Email = "user1@phonebook.com",
                    PhoneNumber = "09129876543",
                    FirstName = "کاربر",
                    LastName = "یک",
                    IsAdmin = false,
                    ContactLimit = 10
                },
                new User
                {
                    Username = "user2",
                    Password = "user123",
                    Email = "user2@phonebook.com",
                    PhoneNumber = "09351234567",
                    FirstName = "کاربر",
                    LastName = "دو",
                    IsAdmin = false,
                    ContactLimit = 10
                }
            };

            foreach (var user in sampleUsers)
            {
                context.Users.Add(user);
                context.SaveChanges();
            }

            // ایجاد مخاطبین نمونه
            var sampleContacts = new[]
            {
                new Contact {
                    FirstName = "علی",
                    LastName = "رضایی",
                    PhoneNumber = "09111111111",
                    Email = "ali@example.com",
                    UserId = 2
                },
                new Contact {
                    FirstName = "مریم",
                    LastName = "محمدی",
                    PhoneNumber = "09222222222",
                    Email = "maryam@example.com",
                    UserId = 2
                },
                new Contact {
                    FirstName = "رضا",
                    LastName = "حسینی",
                    PhoneNumber = "09333333333",
                    Email = "reza@example.com",
                    UserId = 3
                }
            };


            foreach (var contact in sampleContacts)
            {
                context.Contacts.Add(contact);
                context.SaveChanges();
            }


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