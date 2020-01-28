using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(IdentityServerSample3.Startup))]
namespace IdentityServerSample3
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
