using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(VTIntranet.Startup))]
namespace VTIntranet
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}
