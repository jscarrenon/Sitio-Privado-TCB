using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Sitio_Privado.Models;

namespace Sitio_Privado.Controllers
{
    public class CartolaController : ApiController
    {
        [HttpPost]
        public IHttpActionResult GetSingle([FromBody]CartolaInput input)
        {
            try
            {
                Cartola cartola = new Cartola(input);
                return Ok(cartola);
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }
    }
}
