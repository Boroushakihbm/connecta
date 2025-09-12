namespace Connecta.Migrations.Connecta
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Contacts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 50),
                        LastName = c.String(nullable: false, maxLength: 50),
                        PhoneNumber = c.String(nullable: false),
                        Address = c.String(maxLength: 200),
                        Email = c.String(),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Username = c.String(nullable: false, maxLength: 50),
                        Password = c.String(nullable: false, maxLength: 100),
                        Email = c.String(nullable: false),
                        PhoneNumber = c.String(nullable: false),
                        FirstName = c.String(maxLength: 50),
                        LastName = c.String(maxLength: 50),
                        IsAdmin = c.Boolean(nullable: false),
                        ContactLimit = c.Int(nullable: false),
                        IsOnline = c.Boolean(nullable: false),
                        LastSeen = c.DateTime(nullable: false),
                        RegisteredDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Content = c.String(nullable: false),
                        Timestamp = c.DateTime(nullable: false),
                        SenderId = c.Int(nullable: false),
                        ReceiverId = c.Int(nullable: false),
                        IsRead = c.Boolean(nullable: false),
                        MessageType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.ReceiverId)
                .ForeignKey("dbo.Users", t => t.SenderId)
                .Index(t => t.SenderId)
                .Index(t => t.ReceiverId);
            
            CreateTable(
                "dbo.UserContacts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        ContactUserId = c.Int(nullable: false),
                        Nickname = c.String(),
                        AddedDate = c.DateTime(nullable: false),
                        IsBlocked = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.ContactUserId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.ContactUserId);
            
            CreateTable(
                "dbo.Plans",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        Price = c.Int(nullable: false),
                        ContactLimit = c.Int(nullable: false),
                        DurationDays = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserPlans",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        Price = c.Int(nullable: false),
                        ContactLimit = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserContacts", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserContacts", "ContactUserId", "dbo.Users");
            DropForeignKey("dbo.Messages", "SenderId", "dbo.Users");
            DropForeignKey("dbo.Messages", "ReceiverId", "dbo.Users");
            DropForeignKey("dbo.Contacts", "UserId", "dbo.Users");
            DropIndex("dbo.UserContacts", new[] { "ContactUserId" });
            DropIndex("dbo.UserContacts", new[] { "UserId" });
            DropIndex("dbo.Messages", new[] { "ReceiverId" });
            DropIndex("dbo.Messages", new[] { "SenderId" });
            DropIndex("dbo.Contacts", new[] { "UserId" });
            DropTable("dbo.UserPlans");
            DropTable("dbo.Plans");
            DropTable("dbo.UserContacts");
            DropTable("dbo.Messages");
            DropTable("dbo.Users");
            DropTable("dbo.Contacts");
        }
    }
}
