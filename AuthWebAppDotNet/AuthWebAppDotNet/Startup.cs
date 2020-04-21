using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(AuthWebAppDotNet.Startup))]


namespace AuthWebAppDotNet
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }

        
    }
}