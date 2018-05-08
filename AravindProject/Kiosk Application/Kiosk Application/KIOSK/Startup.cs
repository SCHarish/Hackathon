using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(KIOSK.Startup))]
namespace KIOSK
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //ConfigureAuth(app);
        }
    }
}
