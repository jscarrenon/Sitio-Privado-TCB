using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;

namespace Sitio_Privado.Filters
{
    public class AllowCrossSiteJsonApiAttribute: ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            var domains = ConfigurationManager.AppSettings["web:AcceptedDomains"].Split(';');

            if (actionExecutedContext.ActionContext.RequestContext.Url.Request.Headers.Referrer != null)
            {
                if (domains.Contains(actionExecutedContext.ActionContext.RequestContext.Url.Request.Headers.Referrer.Host)
                    && actionExecutedContext.Response != null)
                {
                    actionExecutedContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                }
            }
            else
            {
                if (actionExecutedContext.Request.Headers.GetValues("X-Domain-Request").Contains("ie9")
                    && actionExecutedContext.Response != null)
                {
                    actionExecutedContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                }
            }

            base.OnActionExecuted(actionExecutedContext);
        }
    }
}