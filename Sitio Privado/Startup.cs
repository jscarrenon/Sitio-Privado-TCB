using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Sitio_Privado.Startup))]
namespace Sitio_Privado
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
