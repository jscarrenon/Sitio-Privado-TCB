using System.Security.Claims;
using System.Configuration;
using Sitio_Privado.Helpers;
using System.Web.Http;
using Sitio_Privado.Filters;
using Sitio_Privado.Models;
using Sitio_Privado.Services.Interfaces;
using System.Collections.Generic;
using NLog;
using Sitio_Privado.Extras;
using System.Web;

namespace Sitio_Privado.Controllers
{
    [RoutePrefix("api/authentication")]
    public class AuthenticationController : ApiBaseController
    {
        IHttpService httpService = null;
        IAuthorityClientService authorityClientService = null;
        IExternalUserService userService = null;
        Logger logger;

        public AuthenticationController(IHttpService httpService, IAuthorityClientService authorityClientService, IExternalUserService userService)
        {
            this.httpService = httpService;
            this.authorityClientService = authorityClientService;
            this.userService = userService;
            logger = LogManager.GetLogger("SessionLog");
        }

        /// <summary>
        /// Verifies if the logged user has the minimum group requirements to use the application
        /// and do some initalization if the user does not exist locally
        /// </summary>
        /// <returns></returns>
        [AuthorizeWithGroups(RequiredScopes = "openid profile")]
        [Route("verifylogin")]
        [HttpPost]
        public IHttpActionResult VerifyLogin()
        {
            var username = UserHelper.ExtractAuthorityId(User as ClaimsPrincipal);
            Usuario user = userService.GetUserInfoByUsername(username);

            if (user != null)
            {
                return Ok(user);
            }
            else
            {
                logger.Warn("User not found in the user service => Username: " + username + "; Email: ");
                return Redirect(ConfigurationManager.AppSettings["web:PostLogoutRedirectUrl"] + "?action=login");
            }
        }
        /// <summary>
        /// Verifies if the logged user has the minimum group requirements to use the application
        /// and return the user sites allowed to view
        /// </summary>
        /// <returns></returns>
        [AuthorizeWithGroups(RequiredScopes = "openid profile")]
        [Route("usersites")]
        [HttpPost]
        public IHttpActionResult GetUserSites()
        {
            //List<SiteInformation> userSites = authorityClientService.GetUserSitesByToken(httpService.ExtractAccessToken(Request));
            List<SiteInformation> userSites = authorityClientService.GetDummySites(httpService.ExtractAccessToken(Request));

            if (userSites != null)
            {
                return Ok(userSites);
            }
            else
            {
                return Redirect(ConfigurationManager.AppSettings["web:PostLogoutRedirectUrl"] + "?action=login");
            }
        }

        [AuthorizeWithGroups(RequiredScopes = "openid profile")]
        [Route("signout")]
        [HttpPost]
        public IHttpActionResult SignOut()
        {
            Logger logger = LogManager.GetLogger("SessionLog");
            var usuario = GetUsuario();
            logger.Info("User signed out => Rut: " + ExtraHelpers.FormatRutToText(usuario.Rut) + "; Email: " +
                usuario.Email + "; IP: " + Request.ServerVariables["REMOTE_ADDR"] + ";");

            HttpContext.GetOwinContext().Authentication.SignOut();

            return Redirect(ConfigurationManager.AppSettings["web:PostLogoutRedirectUrl"]);
        }
    }
}