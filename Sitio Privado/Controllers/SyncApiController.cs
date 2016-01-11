using Sitio_Privado.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Sitio_Privado.Controllers
{
    public class SyncApiController : ApiController
    {
        private SyncApiHelper syncApiHelper = new SyncApiHelper();

        [HttpGet]
        public string Action() {
            return "action";
        }

        [HttpGet]
        public async Task<HttpResponseMessage> GetUsers() {
            return await syncApiHelper.GetAllUsers(null);
        }
    }
}
