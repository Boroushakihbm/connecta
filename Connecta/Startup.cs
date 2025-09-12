using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Connecta.Startup))]
namespace Connecta
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}