using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SSSSO_MHG.Startup))]
namespace SSSSO_MHG
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
