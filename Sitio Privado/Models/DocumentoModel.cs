﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitio_Privado.DocumentosPendientesFirma;
using Sitio_Privado.Extras;
using Sitio_Privado.SuscripcionFirmaElecDoc;
using System.Web.Configuration;

namespace Sitio_Privado.Models
{
    public class DocumentosPendientesInput
    {
    }

    public class DocumentosFirmadosInput
    {
        public string fechaIni { get; set; } //dd-mm-YYYY
        public string fechaFin { get; set; } //dd-mm-YYYY
    }

    public class DocumentoLeidoInput
    {
        public string codigo { get; set; }
    }

    public class DocumentoFirmarInput
    {
        public string codigo { get; set; }
    }

    public class OperacionFirmarInput
    {
        public string codigo { get; set; }
    }

    public class DocumentosPendientesCantidadInput
    {
        public string rut { get; set; }
    }

    public class Documento
    {
        public string Codigo { get; set; }
        public string Producto { get; set; }
        public string Tipo { get; set; }
        public string Folio { get; set; }
        public string FechaCreacion { get; set; }
        public string Leido { get; set; }
        public string Firmado { get; set; }
        public string TipoFirma { get; set; }
        public string FechaFirma { get; set; }
        public string Ruta { get; set; }
        public string NombreCliente { get; set; }
        public string RutaFirmado { get; set; }
        public bool Seleccionado { get; set; }

        public Documento() { }

        public Documento(_operacion documento)
        {
            Codigo = documento._code;
            Producto = documento._producto;
            Tipo = documento._tipo;
            Folio = documento._folio;
            Leido = documento._leido != null ? documento._leido.Trim() : documento._leido;
            Firmado = documento._firmado != null ? documento._firmado.Trim() : documento._firmado;
            TipoFirma = "Web";
            FechaFirma = documento._fechafirm != null ? Converters.getFecha(documento._fechafirm).ToString("dd MMM yyyy") : "";
            Ruta = documento._ruta;
            NombreCliente = documento._nombrecli;
            RutaFirmado = documento._results;
            FechaCreacion = documento._fechacre != null ? Converters.getFecha(documento._fechacre).ToString("dd MMM yyyy") : "";
            Seleccionado = false;
        }

        public Documento(_documento documento)
        {
            Codigo = documento._code;
            Producto = documento._producto;
            Tipo = documento._tipo;
            Folio = documento._folio;
            Leido = documento._leido != null ? documento._leido.Trim() : documento._leido;
            Firmado = documento._firmado != null ? documento._firmado.Trim() : documento._firmado;
            TipoFirma = "Web";
            FechaFirma = documento._fechafirm != null ? Converters.getFecha(documento._fechafirm).ToString("dd MMM yyyy") : "";
            Ruta = documento._ruta;
            NombreCliente = documento._nombrecli;
            RutaFirmado = documento._results;
            FechaCreacion = documento._fechacre != null ? Converters.getFecha(documento._fechacre).ToString("dd MMM yyyy") : "";
            Seleccionado = false;
        }
    }

    public class DocumentoLeidoResultado
    {
        public bool Resultado { get; set; }

        public DocumentoLeidoResultado(DocumentoLeidoInput input, Usuario usuario)
        {
            tann_documentos webService = new tann_documentos();
            bool resultado = webService.cns_documento_leido(Converters.getRutParteEntera(usuario.Rut), input.codigo);
            Resultado = resultado;
        }
    }

    public class DocumentoFirmarResultado
    {
        public List<Documento> Documentos { get; set; }

        public DocumentoFirmarResultado(DocumentoFirmarInput input, Usuario usuario)
        {
            tann_documentos webService = new tann_documentos();
            _operacion[] documentos = webService.cns_firmar_documento(Converters.getRutParteEntera(usuario.Rut), input.codigo);
            Documentos = new List<Documento>();

            foreach(_operacion documento in documentos)
            {
                Documentos.Add(new Documento(documento));
            }
        }

        public DocumentoFirmarResultado(OperacionFirmarInput input, Usuario usuario)
        {
            tann_documentos webService = new tann_documentos();
            _operacion[] documentos = webService.cns_firmar_contrato(Converters.getRutParteEntera(usuario.Rut), input.codigo);
            Documentos = new List<Documento>();

            foreach (_operacion documento in documentos)
            {
                Documentos.Add(new Documento(documento));
            }
        }
    }

    public class DocumentosPendientesCantidadResultado
    {
        public int Resultado { get; set; }

        public DocumentosPendientesCantidadResultado(DocumentosPendientesCantidadInput input, Usuario usuario)
        {
            tann_documentos webService = new tann_documentos();
            int resultadoOperaciones = webService.cns_operaciones_pendientes(Converters.getRutParteEntera(usuario.Rut));
            int resultadoDocumentos = webService.cns_documentos_pendientes(Converters.getRutParteEntera(usuario.Rut));
            Resultado = resultadoOperaciones + resultadoDocumentos;
        }
    }



    public class RespuestaClienteSusFirmaElectronicaDocs
    {
        public int Resultado { get; set; }

        public RespuestaClienteSusFirmaElectronicaDocs(string rut,string glosa, string respuesta)
        {
            tann_suscrip_firmelec webService = new tann_suscrip_firmelec();
            Authentication auth = new Authentication();
            var userName = WebConfigurationManager.AppSettings["ws:username"];
            var password = WebConfigurationManager.AppSettings["ws:password"];

            auth.Password = password;
            auth.UserName = userName;
            webService.AuthenticationValue = auth;
            webService.reg_resp_cliente(rut, glosa, respuesta);
            Resultado = 1;
        }
    }

    public class ConsultaRespuestaSusFirmaElecDocs
    {
        public int Resultado { get; set; }

        public ConsultaRespuestaSusFirmaElecDocs(string rut)
        {
            tann_suscrip_firmelec webService = new tann_suscrip_firmelec();
            Authentication auth = new Authentication();
            var userName = WebConfigurationManager.AppSettings["ws:username"];
            var password = WebConfigurationManager.AppSettings["ws:password"];

            auth.Password = password;
            auth.UserName = userName;
            webService.AuthenticationValue = auth;
            _itemWs respuesta =  webService.val_resp_cliente(rut);
            Resultado = (int) respuesta._valor;


        }
    }
}