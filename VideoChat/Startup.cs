using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(VideoChat.Startup))]
namespace VideoChat
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
