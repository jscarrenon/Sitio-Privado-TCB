using Microsoft.Practices.Unity;
using Sitio_Privado.Services;
using Sitio_Privado.Services.ExternalUserProvider;
using Sitio_Privado.Services.Interfaces;
using System;

namespace Sitio_Privado.Configuration.Application
{
    public class UnityConfiguration
    {
        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        /// <summary>
        /// Gets the configured Unity container.
        /// </summary>
        public static IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }


        /// <summary>Registers the type mappings with the Unity container.</summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>There is no need to register concrete types such as controllers or API controllers (unless you want to
        /// change the defaults), as Unity allows resolving a concrete type even if it was not previously registered.</remarks>
        private static void RegisterTypes(IUnityContainer container)
        {
            // Services
            container.RegisterType<IHttpService, HttpService>();
            container.RegisterType<IAuthorityClientService, AuthorityClientService>();

            // Services (single instance)
            container.RegisterInstance(typeof(IExternalUserService), container.Resolve<LDAPService>(), new ContainerControlledLifetimeManager());


        }
    }
}