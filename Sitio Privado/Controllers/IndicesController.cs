using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Sitio_Privado.Models;

namespace Sitio_Privado.Controllers
{
    public class IndicesController : ApiController
    {
        [HttpPost]
        public IHttpActionResult GetSingle([FromBody]IndicesInput input)
        {
            try
            {
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
