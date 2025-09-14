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
            PhoneBookDatabaseInitializer.Seed(context);
        }
    }
}
