using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Sitio_Privado.Models;
using Sitio_Privado.CategoriaInversionista;
using System.Threading.Tasks;

namespace Sitio_Privado.Controllers
{
    public class ProductoController : ApiBaseController
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
                return InternalServerError(e);
            }
        }

        [HttpPost]
        public async Task<IHttpActionResult> GetSingle([FromBody]ProductoInput input)
        {
            try
            {
                var usuario = await GetUsuarioActual();
                Producto producto = new Producto(input);
                return Ok(producto);
            }
            catch(Exception e)
            {
                return InternalServerError(e);
            }
        }
    }
}
