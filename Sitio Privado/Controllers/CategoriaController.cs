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
    public class CategoriaController : ApiController
    {
        // GET api/categoria
        public IHttpActionResult Get()
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
                return NotFound();
            }
        }

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

        // POST api/categoria
        public IHttpActionResult Post([FromBody]CategoriaClienteInput input)
        {
            try
            {
                Categoria categoria = new Categoria(input);
                return Ok(categoria);
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }
    }
}
