using System.Security.Claims;
using System.Configuration;
using Sitio_Privado.Helpers;
using System.Web.Http;
using Sitio_Privado.Infraestructure.Constants;
using Sitio_Privado.Filters;
using Sitio_Privado.Models;
using Sitio_Privado.Services.Interfaces;

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

           // var usuario = this.GetUsuarioActual(user);

           
            if (user != null)
            {
                //return RedirectToAction("Index", "Home");
                return Ok(user);
            }
            else
            {
                return Redirect(ConfigurationManager.AppSettings["web:PostLogoutRedirectUrl"] + "?action=login");
               // return ResponseMessage(httpService.GenerateErrorResponse(ApiErrorCode.GenericUnauthorized, ActionContext.Request));
            }
        }
    }
}