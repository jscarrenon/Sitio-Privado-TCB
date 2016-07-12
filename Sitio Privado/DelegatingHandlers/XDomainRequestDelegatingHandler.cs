using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Sitio_Privado.DelegatingHandlers
{
    /// <summary>
    /// Gives the WebAPI the ability to handle XDomainRequest objects with embedded JSON data.
    /// </summary>
    public class XDomainRequestDelegatingHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // XDomainRequest objects set the Content Type to null, which is an unchangable setting.
            // Microsoft specification states that XDomainRequest always has a contenttype of text/plain, but the documentation is wrong.
            // Obviously, this breaks just about every specification, so it turns out the ONLY extensibility
            // point to handle this is before the request hits the WebAPI framework, as we do here.

            // To read an apology from the developer that created the XDomainRequest object, see here: 
            // http://blogs.msdn.com/b/ieinternals/archive/2010/05/13/xdomainrequest-restrictions-limitations-and-workarounds.aspx

            // By international specification, a null content type is supposed to result in application/octect-stream (spelling mistake?),
            // But since this is such an edge case, the WebAPI framework doesn't convert that for us before we hit this point.  It is unlikely, 
            // but possible that in a future Web.Api release, we will need to also sniff here for the octect header.
            if (request.Content.Headers.ContentType == null && request.Method == HttpMethod.Post)
            {
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                request.Headers.Add("X-Domain-Request", "ie9");
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}