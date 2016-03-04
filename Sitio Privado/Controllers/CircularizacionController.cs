using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Sitio_Privado.Models;
using Sitio_Privado.CircularizacionCustodia;
using System.Threading.Tasks;

namespace Sitio_Privado.Controllers
{
    public class CircularizacionController : ApiBaseController
    {
        [HttpPost]
        public async Task<IHttpActionResult> GetPendiente([FromBody]CircularizacionPendienteInput input)
        {
            try
            {
                var usuario = await GetUsuarioActual();
                CircularizacionProcesoResultado proceso = new CircularizacionProcesoResultado(input, usuario);
                return Ok(proceso);
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IHttpActionResult> GetArchivo([FromBody]CircularizacionArchivoInput input)
        {
            try
            {
                var usuario = await GetUsuarioActual();
                CircularizacionArchivo archivo = new CircularizacionArchivo(input, usuario);
                return Ok(archivo);
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IHttpActionResult> SetLeida([FromBody]CircularizacionLeidaInput input)
        {
            try
            {
                var usuario = await GetUsuarioActual();
                CircularizacionProcesoResultado proceso = new CircularizacionProcesoResultado(input, usuario);
                return Ok(proceso);
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IHttpActionResult> SetRespondida([FromBody]CircularizacionRespondidaInput input)
        {
            try
            {
                var usuario = await GetUsuarioActual();
                CircularizacionProcesoResultado proceso = new CircularizacionProcesoResultado(input, usuario);
                return Ok(proceso);
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }

    }
}
