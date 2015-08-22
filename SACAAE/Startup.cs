using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SACAAE.Startup))]
namespace SACAAE
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
