using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Sitio_Privado.Models;

namespace Sitio_Privado.Controllers
{
    public class BalanceController : ApiController
    {
        [HttpPost]
        public IHttpActionResult GetSingle([FromBody]BalanceInput input)
        {
            try
            {
                Balance balance = new Balance(input);
                return Ok(balance);
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }
    }
}
