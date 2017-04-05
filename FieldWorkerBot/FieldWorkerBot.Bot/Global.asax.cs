using Castle.Windsor;
using System.Web.Http;
using FieldWorkerBot.Bot.Infrastructure;

namespace FieldWorkerBot.Bot
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        public static WindsorContainer Container;

        protected void Application_Start()
        {
            Container = new WindsorContainer();
            Container.Install(new DependencyInstaller());

            GlobalConfiguration.Configure(WebApiConfig.Register);
        }

        protected void Application_End()
        {
            Container.Dispose();
        }
    }
}
