using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(IdentityServerSample2.Startup))]
namespace IdentityServerSample2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
