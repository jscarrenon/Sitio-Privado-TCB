using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Sitio_Privado.Models;
using Sitio_Privado.CircularizacionCustodia;
using System.Threading.Tasks;
using Sitio_Privado.Extras;
using System.Globalization;

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
                return InternalServerError(e);
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
                return InternalServerError(e);
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
                return InternalServerError(e);
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
                return InternalServerError(e);
            }
        }

        [HttpPost]
        public async Task<IHttpActionResult> GetFecha([FromBody]CircularizacionFechaInput input)
        {
            try
            {
                var usuario = await GetUsuarioActual();
                tann_circularizacion webService = new tann_circularizacion();
                string fecha = webService.cli_fecha_circularizacion(Converters.getRutParteEnteraInt(usuario.Rut));
                DateTime? resultado = null;

                if(fecha.ToLower() != "sin periodo" && fecha.ToLower() != "sin proceso")
                {
                    resultado = DateTime.ParseExact(fecha, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                }

                return Ok(resultado);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

    }
}