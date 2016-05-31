using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Sitio_Privado.Models;
using Sitio_Privado.DocumentosPendientesFirma;
using System.Threading.Tasks;
using Sitio_Privado.Extras;
using Sitio_Privado.SuscripcionFirmaElecDoc;

namespace Sitio_Privado.Controllers
{
    public class DocumentoController : ApiBaseController
    {
        [HttpPost]
        public async Task<IHttpActionResult> GetListPendientes([FromBody]DocumentosPendientesInput input)
        {
            try
            {
                var usuario = await GetUsuarioActual();
                tann_documentos webService = new tann_documentos();
                _operacion[] operaciones = webService.cns_operacion_pendiente(Converters.getRutParteEntera(usuario.Rut));

                List<Documento> listaOperaciones = new List<Documento>();
                foreach (_operacion operacion in operaciones)
                {
                    listaOperaciones.Add(new Documento(operacion));
                }

                _documento[] documentos = webService.cns_contrato_pendiente(Converters.getRutParteEntera(usuario.Rut));
                
                List<Documento> listaDocumentos = new List<Documento>();
                foreach (_documento documento in documentos)
                {
                    listaDocumentos.Add(new Documento(documento));
                }

                Dictionary<string, List<Documento>> documentosDictionary = new Dictionary<string, List<Documento>>();
                documentosDictionary.Add("operaciones", listaOperaciones);
                documentosDictionary.Add("documentos", listaDocumentos);
                return Ok(documentosDictionary);
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IHttpActionResult> GetListFirmados([FromBody]DocumentosFirmadosInput input)
        {
            try
            {
                var usuario = await GetUsuarioActual();
                tann_documentos webService = new tann_documentos();
                _operacion[] operaciones = webService.cns_operacion_firmada(Converters.getRutParteEntera(usuario.Rut), input.fechaIni, input.fechaFin);

                List<Documento> listaOperaciones = new List<Documento>();
                foreach (_operacion operacion in operaciones)
                {
                    listaOperaciones.Add(new Documento(operacion));
                }

                _operacion[] documentos = webService.cns_contrato_firmado(Converters.getRutParteEntera(usuario.Rut), input.fechaIni, input.fechaFin);

                List<Documento> listaDocumentos = new List<Documento>();
                foreach (_operacion documento in documentos)
                {
                    listaDocumentos.Add(new Documento(documento));
                }

                Dictionary<string, List<Documento>> documentosDictionary = new Dictionary<string, List<Documento>>();
                documentosDictionary.Add("operaciones", listaOperaciones);
                documentosDictionary.Add("documentos", listaDocumentos);
                return Ok(documentosDictionary);
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IHttpActionResult> SetLeido([FromBody]DocumentoLeidoInput input)
        {
            try
            {
                var usuario = await GetUsuarioActual();
                DocumentoLeidoResultado resultado = new DocumentoLeidoResultado(input, usuario);

                return Ok(resultado);
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IHttpActionResult> SetFirmarDocumento([FromBody]DocumentoFirmarInput input)
        {
            try
            {
                var usuario = await GetUsuarioActual();
                DocumentoFirmarResultado resultado = new DocumentoFirmarResultado(input, usuario);

                return Ok(resultado);
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }


        [HttpPost]
        public async Task<IHttpActionResult> SetFirmarOperacion([FromBody]OperacionFirmarInput input)
        {
            try
            {
                var usuario = await GetUsuarioActual();
                DocumentoFirmarResultado resultado = new DocumentoFirmarResultado(input, usuario);

                return Ok(resultado);
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IHttpActionResult> GetCantidadPendientes([FromBody]DocumentosPendientesCantidadInput input)
        {
            try
            {
                var usuario = await GetUsuarioActual();
                DocumentosPendientesCantidadResultado resultado = new DocumentosPendientesCantidadResultado(input, usuario);

                return Ok(resultado);
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }


        [HttpPost]
        public async Task<IHttpActionResult> SetRespuestaSusFirmaElecDoc([FromBody]SuscripcionFirmaElectronica input)
        {
            try
            {
                var usuario = await GetUsuarioActual();
                RespuestaClienteSusFirmaElectronicaDocs resultado = new RespuestaClienteSusFirmaElectronicaDocs(usuario.Rut, input.Glosa, input.Respuesta);

                return Ok(resultado.Resultado);
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IHttpActionResult> GetConsultaRespuestaSusFirmaElecDoc()
        {
            try
            {
                var usuario = await GetUsuarioActual();

                ConsultaRespuestaSusFirmaElecDocs resultado = new ConsultaRespuestaSusFirmaElecDocs(usuario.Rut);

                return Ok(resultado.Resultado);
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }


    }
}

