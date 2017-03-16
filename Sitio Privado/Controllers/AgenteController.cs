using System;
using System.Web.Http;
using Sitio_Privado.Models;
using Sitio_Privado.Services.Interfaces;
using Sitio_Privado.Filters;

namespace Sitio_Privado.Controllers
{
    public class AgenteController : ApiBaseController
    {
        IHttpService httpService = null;
        IAuthorityClientService authorityClientService = null;
        public AgenteController(IHttpService httpService, IAuthorityClientService authorityClientService) 
            
        {
            this.httpService = httpService;
            this.authorityClientService = authorityClientService;
        }
        [AuthorizeWithGroups(CheckLocalExistence = false, RequiredScopes = "openid profile")]
        [HttpPost]
        public IHttpActionResult GetSingle([FromBody]AgenteInput input)
        {
            try
            {
                Person user = authorityClientService.GetPersonInformationByToken(httpService.ExtractAccessToken(Request));

                Usuario usuario = GetUsuarioActual(user);
                Agente agente = new Agente(input, usuario);
                return Ok(agente);
            }
            catch(Exception e)
            {
                return InternalServerError(e);
            }
        }
    }
}