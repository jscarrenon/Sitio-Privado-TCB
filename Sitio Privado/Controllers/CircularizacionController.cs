using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Sitio_Privado.Models;
using Sitio_Privado.CircularizacionCustodia;

namespace Sitio_Privado.Controllers
{
    public class CircularizacionController : ApiController
    {
        [HttpPost]
        public IHttpActionResult GetPendiente([FromBody]CircularizacionPendienteInput input)
        {
            try
            {
                CircularizacionProcesoResultado proceso = new CircularizacionProcesoResultado(input);
                return Ok(proceso);
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public IHttpActionResult GetArchivo([FromBody]CircularizacionArchivoInput input)
        {
            try
            {
                CircularizacionArchivo archivo = new CircularizacionArchivo(input);
                return Ok(archivo);
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public IHttpActionResult SetLeido([FromBody]CircularizacionLeidaInput input)
        {
            try
            {
                CircularizacionProcesoResultado proceso = new CircularizacionProcesoResultado(input);
                return Ok(proceso);
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public IHttpActionResult SetRespondida([FromBody]CircularizacionRespondidaInput input)
        {
            try
            {
                CircularizacionProcesoResultado proceso = new CircularizacionProcesoResultado(input);
                return Ok(proceso);
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }

    }
}
