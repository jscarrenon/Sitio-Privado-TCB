using Sitio_Privado.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace Sitio_Privado.Helpers
{
    public class GraphApiResponseInfo
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }
        public GraphUserModel User { get; set; }
    }
}