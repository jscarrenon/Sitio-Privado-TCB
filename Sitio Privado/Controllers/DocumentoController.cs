using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Sitio_Privado.Models;
using Sitio_Privado.DocumentosPendientesFirma;

namespace Sitio_Privado.Controllers
{
    public class DocumentoController : ApiController
    {
        [HttpPost]
        public IHttpActionResult GetListPendientes([FromBody]DocumentosPendientesInput input)
        {
            try
            {
                tann_documentos webService = new tann_documentos();
                _operacion[] operaciones = webService.cns_operacion_pendiente(input.rut);

                List<Documento> listaOperaciones = new List<Documento>();
                foreach (_operacion operacion in operaciones.Where(x => x._tipo == "Operacion"))
                {
                    listaOperaciones.Add(new Documento(operacion));
                }

                List<Documento> listaDocumentos = new List<Documento>();
                foreach (_operacion operacion in operaciones.Where(x => x._tipo == "Documento"))
                {
                    listaDocumentos.Add(new Documento(operacion));
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
        public IHttpActionResult GetListFirmados([FromBody]DocumentosFirmadosInput input)
        {
            try
            {
                tann_documentos webService = new tann_documentos();
                _operacion[] operaciones = webService.cns_operacion_firmada(input.rut, input.fechaIni, input.fechaFin);

                List<Documento> listaOperaciones = new List<Documento>();
                foreach (_operacion operacion in operaciones.Where(x => x._tipo == "Operacion"))
                {
                    listaOperaciones.Add(new Documento(operacion));
                }

                List<Documento> listaDocumentos = new List<Documento>();
                foreach (_operacion operacion in operaciones.Where(x => x._tipo == "Documento"))
                {
                    listaDocumentos.Add(new Documento(operacion));
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
        public IHttpActionResult SetLeido([FromBody]DocumentoLeidoInput input)
        {
            try
            {
                DocumentoLeidoResultado resultado = new DocumentoLeidoResultado(input);

                return Ok(resultado);
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }
    }
}
