using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ExpenseTracker.App_Start.StartUp))]
namespace ExpenseTracker.App_Start
{
    public partial class StartUp
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}