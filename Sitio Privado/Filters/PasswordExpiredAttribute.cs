using System;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web.Mvc;

namespace Sitio_Privado.Filters
{
    public class PasswordExpiredAttribute : AuthorizeAttribute
    {
        private static readonly double passwordExpiresInHours = double.Parse(ConfigurationManager.AppSettings["tempPass:Timeout"]);
        private const string isTemporalPasswordClaim = "isTemporalPassword";
        private const string TemporalPasswordTimestampClaim = "temporalPasswordTimestamp";

        public override void OnAuthorization(System.Web.Mvc.AuthorizationContext filterContext)
        {
            if (filterContext.ActionDescriptor.GetCustomAttributes(typeof(SkipPasswordExpiredAttribute), false).Any())
            {
                return;
            }

            IPrincipal user = filterContext.HttpContext.User;

            if (user != null && user.Identity.IsAuthenticated)
            {
                Claim claim = ((ClaimsIdentity)user.Identity).Claims.Where(c => c.Type == isTemporalPasswordClaim).First();
                bool isTemporalPassword = bool.Parse(claim.Value);

                claim = ((ClaimsIdentity)user.Identity).Claims.Where(c => c.Type == TemporalPasswordTimestampClaim).First();
                DateTime temporalPasswordTimestamp = DateTime.Parse(claim.Value);

                if (isTemporalPassword)
                {
                    DateTime limit = temporalPasswordTimestamp.AddHours(passwordExpiresInHours);

                    if (DateTime.Now <= limit)
                    {
                        filterContext.HttpContext.Response.Redirect(string.Format("~/{0}/{1}?{2}", "Account", "ChangePassword", "reason=expired"));
                    }
                    else
                    {
                        throw new TimeoutException("Su contraseña temporal ha caducado. Por favor solicite una nueva.");
                    }
                }                    
            }            
            
            base.OnAuthorization(filterContext);
        }
    }
}