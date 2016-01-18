using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Sitio_Privado.Models.CategoriaInversionista;

namespace Sitio_Privado.Controllers
{
    public class ProductoController : ApiController
    {
        // POST api/producto
        public IHttpActionResult Post([FromBody]ProductoInput input)
        {
            try
            {
                Producto producto = new Producto(input);
                return Ok(producto);
            }
            catch(Exception e)
            {
                return NotFound();
            }
        }
    }
}
