﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Sitio_Privado.Models;
using Sitio_Privado.DocumentosPendientesFirma;
using System.Threading.Tasks;
using Sitio_Privado.Extras;

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
    }
}

