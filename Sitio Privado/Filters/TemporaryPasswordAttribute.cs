﻿using System;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web.Mvc;
using System.Web.Routing;

namespace Sitio_Privado.Filters
{
    public class TemporaryPasswordAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(System.Web.Mvc.AuthorizationContext filterContext)
        {
            if (filterContext.ActionDescriptor.GetCustomAttributes(typeof(SkipTemporaryPasswordAttribute), false).Any())
            {
                return;
            }

            IPrincipal user = filterContext.HttpContext.User;

            if (user != null && user.Identity.IsAuthenticated)
            {
                Claim claim = ((ClaimsIdentity)user.Identity).Claims.Where(c => c.Type == Startup.isTemporalPasswordClaimKey).First();
                bool isTemporalPassword = bool.Parse(claim.Value);

                if (isTemporalPassword)
                {
                    filterContext.Result = new RedirectResult("#/cambiar-contrasena");
                }                    
            }            
            
            base.OnAuthorization(filterContext);
        }
    }
}