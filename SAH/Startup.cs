using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SAH.Startup))]
namespace SAH
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
