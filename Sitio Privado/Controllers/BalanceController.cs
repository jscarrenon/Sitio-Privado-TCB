using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Sitio_Privado.Models;
using System.Threading.Tasks;

namespace Sitio_Privado.Controllers
{
    public class BalanceController : ApiBaseController
    {
        [HttpPost]
        public IHttpActionResult GetSingle([FromBody]BalanceInput input)
        {
            try
            {
                var usuario = GetUsuarioActual();
                Balance balance = new Balance(input, usuario);
                return Ok(balance);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }
    }
}