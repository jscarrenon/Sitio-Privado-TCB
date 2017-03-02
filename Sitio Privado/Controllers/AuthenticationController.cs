using System.Security.Claims;
using System.Globalization;
using System.Configuration;
using Sitio_Privado.Helpers;
using NLog;
using System.Web;
using System.Web.Http;
using Sitio_Privado.Infraestructure.Constants;
using Sitio_Privado.Filters;
using Sitio_Privado.Infraestructure.ExceptionHandling;
using Sitio_Privado.Services;
using Sitio_Privado.Models;

namespace Sitio_Privado.Controllers
{
    [RoutePrefix("api/authentication")]
    public class AuthenticationController : ApiBaseController
    {
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
            HttpService httpService = new HttpService();
            AuthorityClientService clientService = new AuthorityClientService();

            Usuario usuario = clientService.VerifyLoginAndGetPersonInformation(
                httpService.ExtractAccessToken(Request),
                UserHelper.ExtractRolesFromGroup(User as ClaimsPrincipal, ApplicationConstants.RequiredGroupName));

            if (usuario != null)
            {
                //return RedirectToAction("Index", "Home");
                return Ok(usuario);
            }
            else
            {
                return Redirect(ConfigurationManager.AppSettings["web:PostLogoutRedirectUrl"] + "?action=login");
               // return ResponseMessage(httpService.GenerateErrorResponse(ApiErrorCode.GenericUnauthorized, ActionContext.Request));
            }
        }
    }
}