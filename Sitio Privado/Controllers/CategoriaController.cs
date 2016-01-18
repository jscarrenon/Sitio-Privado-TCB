using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Sitio_Privado.Models.CategoriaInversionista;

namespace Sitio_Privado.Controllers
{
    public class CategoriaController : ApiController
    {
        // POST api/categoria
        public IHttpActionResult Post([FromBody]CategoriaInput input)
        {
            try
            {
                Categoria categoria = new Categoria(input);
                return Ok(categoria);
            }
            catch(Exception e)
            {
                return NotFound();
            }
        }
    }
}
