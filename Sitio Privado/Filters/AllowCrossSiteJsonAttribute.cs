using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sitio_Privado.Filters
{
    public class AllowCrossSiteJsonAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var domains = new List<string> { "kunder.cl", "www.kunder.cl", "tanner.cl", "www.tanner.cl" };

            if (domains.Contains(filterContext.RequestContext.HttpContext.Request.UrlReferrer.Host))
            {
                filterContext.RequestContext.HttpContext.Response.AddHeader("Access-Control-Allow-Origin", "*");
            }

            base.OnActionExecuting(filterContext);
        }
    }
}