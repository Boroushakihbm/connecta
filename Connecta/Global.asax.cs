using Connecta.Migrations.Connecta;
using Connecta.Models;
using System;
using System.Data.Entity;
using System.Web;

namespace Connecta
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            Application["OnlineUsers"] = 0;

            Database.SetInitializer(new MigrateDatabaseToLatestVersion<PhoneBookContext, Configuration>());

            using (var ctx = new PhoneBookContext())
            {
                ctx.Database.Initialize(false);
            }
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exc = Server.GetLastError();

            if (exc is HttpUnhandledException)
            {
                Server.Transfer("ErrorPage.aspx?handler=Application_Error%20-%20Global.asax", true);
            }
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {
            Application.Lock();
            Application["OnlineUsers"] = ((int)Application["OnlineUsers"]) - 1;
            Application.UnLock();
        }

    }
}