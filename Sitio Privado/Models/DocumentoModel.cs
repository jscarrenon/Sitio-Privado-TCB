﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitio_Privado.DocumentosPendientesFirma;
using Sitio_Privado.Extras;

namespace Sitio_Privado.Models
{
    public class DocumentosPendientesInput
    {
        public string rut { get; set; }
    }

    public class DocumentosFirmadosInput
    {
        public string rut { get; set; }
        public string fechaIni { get; set; } //dd-mm-YYYY
        public string fechaFin { get; set; } //dd-mm-YYYY
    }

    public class DocumentoLeidoInput
    {
        public string rut { get; set; }
        public string codigo { get; set; }
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
        public string Ruta { get; set; }
        public string Resultados { get; set; }

        public Documento() { }

        public Documento(_operacion documento)
        {
            Codigo = documento._code;
            Producto = documento._producto;
            Tipo = documento._tipo;
            Folio = documento._folio;
            Leido = documento._leido;
            Firmado = documento._firmado;
            Ruta = documento._ruta;
            Resultados = documento._results;
            FechaCreacion = Converters.getFecha(documento._fechacre).ToString("dd MMM yyyy");
        }
    }

    public class DocumentoLeidoResultado
    {
        public bool Resultado { get; set; }

        public DocumentoLeidoResultado(DocumentoLeidoInput input)
        {
            tann_documentos webService = new tann_documentos();
            bool resultado = webService.cns_documento_leido(input.rut, input.codigo);
            Resultado = resultado;
        }
    }
}