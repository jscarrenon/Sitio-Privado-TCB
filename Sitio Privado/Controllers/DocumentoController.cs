using System;
using System.Collections.Generic;
using System.Web.Http;
using Sitio_Privado.Models;
using Sitio_Privado.DocumentosPendientesFirma;
using Sitio_Privado.Extras;
using Sitio_Privado.Services.Interfaces;
using Sitio_Privado.Filters;

namespace Sitio_Privado.Controllers
{
    [AuthorizeWithGroups(CheckLocalExistence = false, RequiredScopes = "openid profile")]
    public class DocumentoController : ApiBaseController
    {
        IHttpService httpService = null;
        IAuthorityClientService authorityClientService = null;
        public DocumentoController(IHttpService httpService, IAuthorityClientService authorityClientService) 
        {
            this.httpService = httpService;
            this.authorityClientService = authorityClientService;
        }

        [HttpPost]
        public IHttpActionResult GetListPendientes([FromBody]DocumentosPendientesInput input)
        {
            try
            {
                var usuario = authorityClientService.GetUserInformationByToken(httpService.ExtractAccessToken(Request));
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
                return InternalServerError(e);
            }
        }

        [HttpPost]
        public IHttpActionResult GetListFirmados([FromBody]DocumentosFirmadosInput input)
        {
            try
            {
                var usuario = authorityClientService.GetPersonInformationByToken(httpService.ExtractAccessToken(Request));
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
                return InternalServerError(e);
            }
        }

        [HttpPost]
        public IHttpActionResult SetLeido([FromBody]DocumentoLeidoInput input)
        {
            try
            {
                var usuario = authorityClientService.GetUserInformationByToken(httpService.ExtractAccessToken(Request));
                DocumentoLeidoResultado resultado = new DocumentoLeidoResultado(input, usuario);

                return Ok(resultado);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        [HttpPost]
        public IHttpActionResult SetFirmarDocumento([FromBody]DocumentoFirmarInput input)
        {
            try
            {
                var usuario = authorityClientService.GetUserInformationByToken(httpService.ExtractAccessToken(Request));
                DocumentoFirmarResultado resultado = new DocumentoFirmarResultado(input, usuario);

                return Ok(resultado);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }


        [HttpPost]
        public IHttpActionResult SetFirmarOperacion([FromBody]OperacionFirmarInput input)
        {
            try
            {
                var usuario = authorityClientService.GetUserInformationByToken(httpService.ExtractAccessToken(Request));
                DocumentoFirmarResultado resultado = new DocumentoFirmarResultado(input, usuario);

                return Ok(resultado);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        [HttpPost]
        public IHttpActionResult GetCantidadPendientes([FromBody]DocumentosPendientesCantidadInput input)
        {
            try
            {
                var usuario = authorityClientService.GetUserInformationByToken(httpService.ExtractAccessToken(Request));
                DocumentosPendientesCantidadResultado resultado = new DocumentosPendientesCantidadResultado(input, usuario);

                return Ok(resultado);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }


        [HttpPost]
        public IHttpActionResult SetRespuestaSusFirmaElecDoc([FromBody]SuscripcionFirmaElectronica input)
        {
            try
            {
                var usuario = authorityClientService.GetUserInformationByToken(httpService.ExtractAccessToken(Request));
                RespuestaClienteSusFirmaElectronicaDocs resultado = new RespuestaClienteSusFirmaElectronicaDocs(usuario.Rut, input.Glosa, input.Respuesta);

                return Ok(resultado.Resultado);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        [HttpPost]
        public IHttpActionResult GetConsultaRespuestaSusFirmaElecDoc()
        {
            try
            {
                var usuario = authorityClientService.GetUserInformationByToken(httpService.ExtractAccessToken(Request));

                ConsultaRespuestaSusFirmaElecDocs resultado = new ConsultaRespuestaSusFirmaElecDocs(usuario.Rut);

                return Ok(resultado.Resultado);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }
    }
}
