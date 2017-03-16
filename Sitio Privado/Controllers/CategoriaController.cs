using System;
using System.Collections.Generic;
using System.Web.Http;
using Sitio_Privado.Models;
using Sitio_Privado.CategoriaInversionista;
using Sitio_Privado.Filters;
using Sitio_Privado.Services.Interfaces;

namespace Sitio_Privado.Controllers
{
    public class CategoriaController : ApiBaseController
    {
        IHttpService httpService = null;
        IAuthorityClientService authorityClientService = null;
        public CategoriaController(IHttpService httpService, IAuthorityClientService authorityClientService) 
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
        [AuthorizeWithGroups(CheckLocalExistence = false, RequiredScopes = "openid profile")]
        [HttpPost]
        public IHttpActionResult GetSingleCliente([FromBody]CategoriaClienteInput input)
        {
            try
            {
                //Person user = authorityClientService.GetUserInformationByToken(httpService.ExtractAccessToken(Request));
                var usuario = authorityClientService.GetUserInformationByToken(httpService.ExtractAccessToken(Request));
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