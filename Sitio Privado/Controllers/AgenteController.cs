using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Sitio_Privado.Models;

namespace Sitio_Privado.Controllers
{
    public class AgenteController : ApiController
    {
        [HttpPost]
        public IHttpActionResult GetSingle([FromBody]AgenteInput input)
        {
            try
            {
                Agente agente = new Agente(input);
                return Ok(agente);
            }
            catch(Exception e)
            {
                return NotFound();
            }
        }
    }
}
