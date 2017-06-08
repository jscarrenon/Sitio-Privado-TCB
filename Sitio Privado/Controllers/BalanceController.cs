using System;
using System.Security.Claims;
using System.Web.Http;
using Sitio_Privado.Filters;
using Sitio_Privado.Helpers;
using Sitio_Privado.Models;
using Sitio_Privado.Services.Interfaces;

namespace Sitio_Privado.Controllers
{
    [AuthorizeWithGroups]
    public class BalanceController : ApiController
    {
        IHttpService httpService = null;
        IExternalUserService userService = null;

        public BalanceController(IHttpService httpService, IExternalUserService userService) 
        {
            this.httpService = httpService;
            this.userService = userService;
        }

        [HttpPost]
        public IHttpActionResult GetSingle([FromBody]BalanceInput input)
        {
            try
            {
                var usuario = userService.GetUserInfoByUsernameV2(UserHelper.ExtractAuthorityId(User as ClaimsPrincipal));
                Balance balance = new Balance(input, usuario);
                return Ok(balance);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }
    }
}