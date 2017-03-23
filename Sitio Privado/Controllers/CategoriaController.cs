using System;
using System.Collections.Generic;
using System.Web.Http;
using Sitio_Privado.Models;
using Sitio_Privado.CategoriaInversionista;
using Sitio_Privado.Filters;
using Sitio_Privado.Services.Interfaces;
using Sitio_Privado.Helpers;
using System.Security.Claims;

namespace Sitio_Privado.Controllers
{
    public class CategoriaController : ApiController
    {
        IHttpService httpService = null;
        IExternalUserService userService = null;

        public CategoriaController(IHttpService httpService, IExternalUserService userService) 
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
                _categoria[] categorias = webService.tann_list_cat();

                List<Categoria> lista = new List<Categoria>();
                foreach (_categoria categoria in categorias)
                {
                    lista.Add(new Categoria(categoria));
                }

                return Ok(lista);
            }
            catch(Exception e)
            {
                return InternalServerError(e);
            }
        }

        [HttpPost]
        public IHttpActionResult GetSingle([FromBody]CategoriaInput input)
        {
            try
            {
                Categoria categoria = new Categoria(input);
                return Ok(categoria);
            }
            catch(Exception e)
            {
                return InternalServerError(e);
            }
        }
        [AuthorizeWithGroups(RequiredScopes = "openid profile")]
        [HttpPost]
        public IHttpActionResult GetSingleCliente([FromBody]CategoriaClienteInput input)
        {
            try
            {
                var usuario = userService.GetUserInfoByUsername(UserHelper.ExtractAuthorityId(User as ClaimsPrincipal));
                Categoria categoria = new Categoria(input, usuario);
                return Ok(categoria);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }
    }
}