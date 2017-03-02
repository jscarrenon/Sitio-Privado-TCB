using Microsoft.Owin;
using System;
using System.Threading.Tasks;
using Microsoft.Owin.Logging;
using Owin;

namespace Sitio_Privado.Infraestructure.ExceptionHandling
{
    public class OwinExceptionHandler : OwinMiddleware
    {
        private readonly ILogger logger;

        public OwinExceptionHandler(OwinMiddleware next, IAppBuilder app) : base(next)
        {
            logger = app.CreateLogger<OwinExceptionHandler>();
        }

        public override async Task Invoke(IOwinContext context)
        {
            try
            {
                await Next.Invoke(context);
            }
            catch (Exception ex)
            {
                logger.WriteError("Unhandled exception in Owin pipeline", ex);
                throw ex;
            }
        }
    }
}