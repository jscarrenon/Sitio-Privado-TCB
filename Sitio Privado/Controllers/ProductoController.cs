using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Sitio_Privado.Models.CategoriaInversionista;
using Sitio_Privado.CategoriaInversionista;

namespace Sitio_Privado.Controllers
{
    public class ProductoController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetList()
        {
            try
            {
                tann_catsvc webService = new tann_catsvc();
                _producto[] productos = webService.tann_list_prod();

                List<Producto> lista = new List<Producto>();
                foreach (_producto producto in productos)
                {
                    lista.Add(new Producto(producto));
                }

                return Ok(lista);
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public IHttpActionResult GetSingle([FromBody]ProductoInput input)
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
