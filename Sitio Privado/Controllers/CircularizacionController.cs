using System;
using System.Web.Http;
using Sitio_Privado.Models;
using Sitio_Privado.CircularizacionCustodia;
using Sitio_Privado.Extras;
using System.Globalization;
using Sitio_Privado.Services.Interfaces;

namespace Sitio_Privado.Controllers
{
    public class CircularizacionController : ApiBaseController
    {
        IHttpService httpService = null;
        IAuthorityClientService authorityClientService = null;
        public CircularizacionController(IHttpService httpService, IAuthorityClientService authorityClientService) 
            
        {
            this.httpService = httpService;
            this.authorityClientService = authorityClientService;
        }

        [HttpPost]
        public IHttpActionResult GetPendiente([FromBody]CircularizacionPendienteInput input)
        {
            try
            {
                //Person user = authorityClientService.GetPersonInformationByToken(httpService.ExtractAccessToken(Request));
                var usuario = authorityClientService.GetUserInformationByToken(httpService.ExtractAccessToken(Request));
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
                Person user = authorityClientService.GetPersonInformationByToken(httpService.ExtractAccessToken(Request));
                var usuario = GetUsuarioActual(user);
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
               // Person user = authorityClientService.GetPersonInformationByToken(httpService.ExtractAccessToken(Request));
               // var usuario =  GetUsuarioActual(user);
                var usuario = authorityClientService.GetUserInformationByToken(httpService.ExtractAccessToken(Request));
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
                var usuario = authorityClientService.GetUserInformationByToken(httpService.ExtractAccessToken(Request));
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
                var usuario = authorityClientService.GetUserInformationByToken(httpService.ExtractAccessToken(Request));
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