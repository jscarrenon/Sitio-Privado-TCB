using System;
using System.Web.Http;
using Sitio_Privado.Models;
using Sitio_Privado.Services.Interfaces;

namespace Sitio_Privado.Controllers
{
    public class BalanceController : ApiBaseController
    {
        IHttpService httpService = null;
        IAuthorityClientService authorityClientService = null;
        public BalanceController(IHttpService httpService, IAuthorityClientService authorityClientService) 
        {
            this.httpService = httpService;
            this.authorityClientService = authorityClientService;
        }

        [HttpPost]
        public IHttpActionResult GetSingle([FromBody]BalanceInput input)
        {
            try
            {
                //Person user = authorityClientService.GetPersonInformationByToken(httpService.ExtractAccessToken(Request));
                var usuario = authorityClientService.GetUserInformationByToken(httpService.ExtractAccessToken(Request));
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