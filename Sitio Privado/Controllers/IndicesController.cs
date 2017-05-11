using System;
using System.Web.Http;
using Sitio_Privado.Models;
using Sitio_Privado.Services.Interfaces;
using Sitio_Privado.Helpers;
using System.Security.Claims;
using Sitio_Privado.Filters;

namespace Sitio_Privado.Controllers
{
    [AuthorizeWithGroups]
    public class IndicesController : ApiController
    {
        IHttpService httpService = null;
        IExternalUserService userService = null;

        public IndicesController(IHttpService httpService, IExternalUserService userService) 
        {
            this.httpService = httpService;
            this.userService = userService;
        }
        

        [HttpPost]
        public IHttpActionResult GetSingle([FromBody]IndicesInput input)
        {
            try
            {
                var usuario = userService.GetUserInfoByUsernameV2(UserHelper.ExtractAuthorityId(User as ClaimsPrincipal));
                Indices indices = new Indices(input);
                return Ok(indices);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }
    }
}