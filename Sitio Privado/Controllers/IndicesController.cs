using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Sitio_Privado.Models;
using System.Threading.Tasks;
using Sitio_Privado.Services.Interfaces;

namespace Sitio_Privado.Controllers
{
    public class IndicesController : ApiBaseController
    {
        IHttpService httpService = null;
        IAuthorityClientService authorityClientService = null;
        public IndicesController(IHttpService httpService, IAuthorityClientService authorityClientService) 
        {
            this.httpService = httpService;
            this.authorityClientService = authorityClientService;
        }
        

        [HttpPost]
        public IHttpActionResult GetSingle([FromBody]IndicesInput input)
        {
            try
            {
                var usuario = authorityClientService.GetUserInformationByUsername(httpService.ExtractAccessToken(Request));
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