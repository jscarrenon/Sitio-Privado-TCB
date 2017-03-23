using System;
using System.Collections.Generic;
using System.Web.Http;
using Sitio_Privado.Models;
using Sitio_Privado.CategoriaInversionista;
using Sitio_Privado.Services.Interfaces;
using Sitio_Privado.Helpers;
using System.Security.Claims;

namespace Sitio_Privado.Controllers
{
    public class ProductoController : ApiController
    {
        IHttpService httpService = null;
        IExternalUserService userService = null;

        public ProductoController(IHttpService httpService, IExternalUserService userService) 
        {
            this.httpService = httpService;
            this.userService = userService;
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
                var usuario = userService.GetUserInfoByUsername(UserHelper.ExtractAuthorityId(User as ClaimsPrincipal));
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