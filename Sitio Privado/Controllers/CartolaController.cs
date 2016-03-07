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
    public class CartolaController : ApiBaseController
    {
        [HttpPost]
        public async Task<IHttpActionResult> GetSingle([FromBody]CartolaInput input)
        {
            try
            {
                var usuario = await GetUsuarioActual();
                Cartola cartola = new Cartola(input, usuario);
                return Ok(cartola);
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }
    }
}
