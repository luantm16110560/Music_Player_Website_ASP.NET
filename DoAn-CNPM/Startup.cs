using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DoAn_CNPM.Startup))]
namespace DoAn_CNPM
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
