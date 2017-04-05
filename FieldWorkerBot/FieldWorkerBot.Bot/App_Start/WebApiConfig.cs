using System.Web.Http;
using System.Web.Http.Dispatcher;
using FieldWorkerBot.Bot.Infrastructure;

namespace FieldWorkerBot.Bot
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // DI setup
            config.Services.Replace(typeof(IHttpControllerActivator), new WindsorControllerActivator(WebApiApplication.Container));

            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
