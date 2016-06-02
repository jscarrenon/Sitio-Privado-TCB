using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sitio_Privado.Filters
{
    public class AllowCrossSiteJsonAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var domains = ConfigurationManager.AppSettings["web:AcceptedDomains"].Split(';');

            if (domains.Contains(filterContext.RequestContext.HttpContext.Request.UrlReferrer.Host))
            {
                filterContext.RequestContext.HttpContext.Response.AddHeader("Access-Control-Allow-Origin", "*");
            }

            base.OnActionExecuting(filterContext);
        }
    }
}