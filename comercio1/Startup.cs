using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(comercio1.Startup))]
namespace comercio1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
