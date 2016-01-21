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
                _operacion[] documentos = webService.cns_operacion_pendiente(input.rut);

                List<Documento> lista = new List<Documento>();
                foreach (_operacion documento in documentos)
                {
                    lista.Add(new Documento(documento));
                }

                return Ok(lista);
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
                _operacion[] documentos = webService.cns_operacion_firmada(input.rut, input.fechaIni, input.fechaFin);

                List<Documento> lista = new List<Documento>();
                foreach (_operacion documento in documentos)
                {
                    lista.Add(new Documento(documento));
                }

                return Ok(lista);
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }
    }
}
