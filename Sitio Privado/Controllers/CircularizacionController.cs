using System;
using System.Web.Http;
using Sitio_Privado.Models;
using Sitio_Privado.CircularizacionCustodia;
using Sitio_Privado.Extras;
using System.Globalization;
using Sitio_Privado.Services.Interfaces;
using Sitio_Privado.Helpers;
using System.Security.Claims;
using Sitio_Privado.Filters;

namespace Sitio_Privado.Controllers
{
    [AuthorizeWithGroups]
    public class CircularizacionController : ApiController
    {
        IHttpService httpService = null;
        IExternalUserService userService = null;

        public CircularizacionController(IHttpService httpService, IExternalUserService userService) 
            
        {
            this.httpService = httpService;
            this.userService = userService;
        }

        [HttpPost]
        public IHttpActionResult GetPendiente([FromBody]CircularizacionPendienteInput input)
        {
            try
            {
                var usuario = userService.GetUserInfoByUsername(UserHelper.ExtractAuthorityId(User as ClaimsPrincipal));
                CircularizacionProcesoResultado proceso = new CircularizacionProcesoResultado(input, usuario);
                return Ok(proceso);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        [HttpPost]
        public IHttpActionResult GetArchivo([FromBody]CircularizacionArchivoInput input)
        {
            try
            {
                var usuario = userService.GetUserInfoByUsername(UserHelper.ExtractAuthorityId(User as ClaimsPrincipal));
                CircularizacionArchivo archivo = new CircularizacionArchivo(input, usuario);
                return Ok(archivo);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        [HttpPost]
        public IHttpActionResult SetLeida([FromBody]CircularizacionLeidaInput input)
        {
            try
            {
                var usuario = userService.GetUserInfoByUsername(UserHelper.ExtractAuthorityId(User as ClaimsPrincipal));
                CircularizacionProcesoResultado proceso = new CircularizacionProcesoResultado(input, usuario);
                return Ok(proceso);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        [HttpPost]
        public IHttpActionResult SetRespondida([FromBody]CircularizacionRespondidaInput input)
        {
            try
            {
                var usuario = userService.GetUserInfoByUsername(UserHelper.ExtractAuthorityId(User as ClaimsPrincipal));
                CircularizacionProcesoResultado proceso = new CircularizacionProcesoResultado(input, usuario);
                return Ok(proceso);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        [HttpPost]
        public IHttpActionResult GetFecha([FromBody]CircularizacionFechaInput input)
        {
            try
            {
                var usuario = userService.GetUserInfoByUsername(UserHelper.ExtractAuthorityId(User as ClaimsPrincipal));
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