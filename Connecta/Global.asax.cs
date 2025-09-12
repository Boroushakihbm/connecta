using Connecta.Migrations.Connecta;
using Connecta.Models;
using Microsoft.Owin;
using System;
using System.Data.Entity;
using System.Web;

namespace Connecta
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            // Initialize the database and seed data
            Database.SetInitializer(new PhoneBookDatabaseInitializer());

            // Start the application
            //RouteConfig.RegisterRoutes(RouteTable.Routes);

            //// این خط باعث می‌شود مایگریشن‌ها اجرا شوند و سپس Seed اجرا شود.
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<PhoneBookContext, Configuration>());

            ////Force initialization so we can see errors now
            //using (var ctx = new PhoneBookContext())
            //{
            //    ctx.Database.Initialize(false);
            //}
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs
            Exception exc = Server.GetLastError();

            if (exc is HttpUnhandledException)
            {
                // Pass the error on to the error page.
                Server.Transfer("ErrorPage.aspx?handler=Application_Error%20-%20Global.asax", true);
            }
        }
    }
}