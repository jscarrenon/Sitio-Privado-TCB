using Microsoft.Owin;
using Sitio_Privado.Extras;
using Sitio_Privado.Tasks;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Caching;
using System.Web.Http;
using System.Web.Http.Tracing;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Sitio_Privado
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            GlobalConfiguration.Configuration.Services.Replace(typeof(ITraceWriter), new NLogger());
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
           // SyncUsersDataJobScheduler.Start();
        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            // Any AJAX request that ends in a redirect should get mapped to an unauthorized request
            // since it should only happen when the request is not authorized and gets automatically
            // redirected to the login page.
            var context = new HttpContextWrapper(Context);
            if (context.Response.StatusCode == 302 && IsAjaxRequest(context.Request))
            {
                context.Response.Clear();
                Context.Response.StatusCode = 401;
            }
        }

        private static bool IsAjaxRequest(HttpRequestBase request)
        {
            NameValueCollection query = request.QueryString;
            if ((query != null) && (query["X-Requested-With"] == "XMLHttpRequest"))
            {
                return true;
            }
            NameValueCollection headers = request.Headers;
            return ((headers != null) && (headers["X-Requested-With"] == "XMLHttpRequest"));
        }
    }
}
