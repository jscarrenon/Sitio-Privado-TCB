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
    public class CategoriaController : ApiBaseController
    {
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

        [HttpPost]
        public IHttpActionResult GetSingleCliente([FromBody]CategoriaClienteInput input)
        {
            try
            {
                var usuario = GetUsuarioActual();
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