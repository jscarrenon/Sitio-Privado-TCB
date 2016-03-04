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
    public class AgenteController : ApiBaseController
    {
        [HttpPost]
        public async Task<IHttpActionResult> GetSingle([FromBody]AgenteInput input)
        {
            try
            {
                Usuario usuario = await this.GetUsuarioActual();
                Agente agente = new Agente(input, usuario);
                return Ok(agente);
            }
            catch(Exception e)
            {
                return NotFound();
            }
        }
    }
}
