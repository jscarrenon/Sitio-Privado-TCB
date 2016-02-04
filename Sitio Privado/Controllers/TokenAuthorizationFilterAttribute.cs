using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Sitio_Privado.Controllers
{
    public class TokenAuthorizationFilterAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var authorization = actionContext.Request.Headers.Authorization;
            if(authorization == null || authorization.ToString() != "asd")
            {
                HttpResponseMessage response = new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
                JObject json = new JObject();
                json.Add("message", "You don't have permission");
                response.Content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
                return;
            }
        }

        public override Task OnAuthorizationAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            var authorization = actionContext.Request.Headers.Authorization;
            if (authorization == null || authorization.ToString() != "asd")
            {
                HttpResponseMessage response = new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
                JObject json = new JObject();
                json.Add("message", "You don't have permission");
                response.Content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
                actionContext.Response = response;
                return Task.FromResult<object>(null);
            }
            return Task.FromResult<object>(null);
        }
    }
}