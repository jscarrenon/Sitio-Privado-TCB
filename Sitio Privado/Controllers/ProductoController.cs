using System;
using System.Collections.Generic;
using System.Web.Http;
using Sitio_Privado.Models;
using Sitio_Privado.CategoriaInversionista;
using Sitio_Privado.Services.Interfaces;

namespace Sitio_Privado.Controllers
{
    public class ProductoController : ApiBaseController
    {
        IHttpService httpService = null;
        IAuthorityClientService authorityClientService = null;
        public ProductoController(IHttpService httpService, IAuthorityClientService authorityClientService) 
        {
            this.httpService = httpService;
            this.authorityClientService = authorityClientService;
        }

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
        public IHttpActionResult GetSingle([FromBody]ProductoInput input)
        {
            try
            {
                var usuario = authorityClientService.GetUserInformationByToken(httpService.ExtractAccessToken(Request));
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