using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Sitio_Privado.Controllers
{
    public class SyncApiController : ApiController
    {
        [HttpGet]
        public string Action() {
            return "action";
        }
    }
}
