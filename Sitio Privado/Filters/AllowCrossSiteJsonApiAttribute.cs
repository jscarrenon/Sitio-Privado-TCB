using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;

namespace Sitio_Privado.Filters
{
    public class AllowCrossSiteJsonApiAttribute: ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            var domains = new List<string> { "kunder.cl", "www.kunder.cl", "tanner.cl", "www.tanner.cl" };

            if (domains.Contains(actionExecutedContext.ActionContext.RequestContext.Url.Request.Headers.Referrer.Host))
            {
                if (actionExecutedContext.Response != null)
                {
                    actionExecutedContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                }
            }

            base.OnActionExecuted(actionExecutedContext);
        }
    }
}