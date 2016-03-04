using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Sitio_Privado.Models;
using System.Threading.Tasks;

namespace Sitio_Privado.Controllers
{
    public class IndicesController : ApiBaseController
    {
        [HttpPost]
        public async Task<IHttpActionResult> GetSingle([FromBody]IndicesInput input)
        {
            try
            {
                var usuario = await GetUsuarioActual();
                Indices indices = new Indices(input);
                return Ok(indices);
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }
    }
}
