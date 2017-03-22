using System.Security.Claims;
using System.Configuration;
using Sitio_Privado.Helpers;
using System.Web.Http;
using Sitio_Privado.Infraestructure.Constants;
using Sitio_Privado.Filters;
using Sitio_Privado.Models;
using Sitio_Privado.Services.Interfaces;
using System.Collections.Generic;

namespace Sitio_Privado.Controllers
{
    [RoutePrefix("api/authentication")]
    public class AuthenticationController : ApiBaseController
    {
        IHttpService httpService = null;
        IAuthorityClientService authorityClientService = null;

        public AuthenticationController(IHttpService httpService, IAuthorityClientService authorityClientService)
            
        {
            this.httpService = httpService;
            this.authorityClientService = authorityClientService;
        }

        /// <summary>
        /// Verifies if the logged user has the minimum group requirements to use the application
        /// and do some initalization if the user does not exist locally
        /// </summary>
        /// <returns></returns>
        [AuthorizeWithGroups(CheckLocalExistence = false, RequiredScopes = "openid profile")]
        [Route("verifylogin")]
        [HttpPost]
        public IHttpActionResult VerifyLogin()
        {
            Person user = authorityClientService.VerifyLoginAndGetPersonInformation(
               httpService.ExtractAccessToken(Request),
               UserHelper.ExtractRolesFromGroup(User as ClaimsPrincipal, ApplicationConstants.RequiredGroupName));
           
            if (user != null)
            {
                return Ok(user);
            }
            else
            {
                return Redirect(ConfigurationManager.AppSettings["web:PostLogoutRedirectUrl"] + "?action=login");
            }
        }
        /// <summary>
        /// Verifies if the logged user has the minimum group requirements to use the application
        /// and return the user sites allowed to view
        /// </summary>
        /// <returns></returns>
        [AuthorizeWithGroups(CheckLocalExistence = false, RequiredScopes = "openid profile")]
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

    }
}