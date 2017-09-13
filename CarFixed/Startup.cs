using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CarFixed.Startup))]
namespace CarFixed
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
