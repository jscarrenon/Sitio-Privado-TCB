using System.Web.Http;
using Sitio_Privado.DelegatingHandlers;
using Microsoft.Practices.Unity;
using Sitio_Privado.Configuration.Application;
using Sitio_Privado.Infraestructure.WebApi;

namespace Sitio_Privado
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API routes
            IUnityContainer container = UnityConfiguration.GetConfiguredContainer();
            config.DependencyResolver = new UnityDependencyResolver(container);
            WebApiUnityActionFilterProvider.RegisterFilterProviders(config, container);
            // Deserialize / Model Bind IE 8 and 9 Ajax Requests
            config.MessageHandlers.Add(new XDomainRequestDelegatingHandler());

            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "SyncApi",
                routeTemplate: "SyncApi/Users/{action}/{id}",
                defaults: new
                {
                    controller = "SyncApi",
                    action = "All",
                    id = RouteParameter.Optional
                }
            );
        }
    }
}
