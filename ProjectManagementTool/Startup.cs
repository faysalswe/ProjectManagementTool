using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ProjectManagementTool.Startup))]
namespace ProjectManagementTool
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
