using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AllIsFair.Startup))]
namespace AllIsFair
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
