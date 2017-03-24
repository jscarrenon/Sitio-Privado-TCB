using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;
using Microsoft.ApplicationInsights;
using NLog;

namespace Sitio_Privado.Infrastructure.Logging
{
    /// <summary>
    /// This class logs any WebApi unhandled exception into Serilog
    /// </summary>
    public class WebApiNLogExceptionLogger : IExceptionLogger
    {
        Logger logger = LogManager.GetLogger("SessionLog");

        public Task LogAsync(ExceptionLoggerContext context, CancellationToken cancellationToken)
        {
            TelemetryClient telemetry = new TelemetryClient();
            telemetry.TrackException(context.Exception);

            logger.Error(context.Exception, string.Format("Unhandled endpoint exception calling: {0} {1}. Message: {2}. Stack: {3}", context.Request.Method, context.Request.RequestUri, context.Exception.Message, context.Exception.StackTrace));

            return Task.FromResult(0);
        }
    }
}