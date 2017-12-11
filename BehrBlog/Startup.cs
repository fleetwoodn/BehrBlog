using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BehrBlog.Startup))]
namespace BehrBlog
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
