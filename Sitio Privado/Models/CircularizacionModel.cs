using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitio_Privado.CircularizacionCustodia;

namespace Sitio_Privado.Models
{
    public class CircularizacionPendienteInput
    {
        public int rut { get; set; } 
        public string fecha { get; set; } //dd-MM-yyyy
    }

    public class CircularizacionArchivoInput
    {
        public string rut { get; set; } //completo
        public string fecha { get; set; } //dd-MM-yyyy
    }

    public class CircularizacionLeidaInput
    {
        public int rut { get; set; } 
        public string fecha { get; set; } //dd-MM-yyyy
    }

    public class CircularizacionRespondidaInput
    {
        public int rut_cli { get; set; } 
        public string fecha { get; set; } 
        public string respuesta { get; set; } // "S" ó "N"
        public string comentario { get; set; } //Sólo en caso de rechazo ("N")
    }

    public class CircularizacionArchivo
    {
        public string UrlCartola { get; set; }
        public string UrlCircularizacion { get; set; }

        public CircularizacionArchivo() { }

        public CircularizacionArchivo(CircularizacionArchivoInput input)
        {
            tann_circularizacion webService = new tann_circularizacion();
            archivocli archivo = webService.cli_archivo_circularizacion(input.rut, input.fecha);

            UrlCartola = archivo._cartola;
            UrlCircularizacion = archivo._circula;
        }
    }

    public class CircularizacionProcesoResultado
    {
        public bool Resultado { get; set; }

        public CircularizacionProcesoResultado(CircularizacionPendienteInput input)
        {
            tann_circularizacion webService = new tann_circularizacion();
            bool resultado = webService.cli_circularizacion(input.rut, input.fecha);
            Resultado = resultado;
        }

        public CircularizacionProcesoResultado(CircularizacionLeidaInput input)
        {
            tann_circularizacion webService = new tann_circularizacion();
            bool resultado = webService.cli_leer_circularizacion(input.rut, input.fecha);
            Resultado = resultado;
        }

        public CircularizacionProcesoResultado(CircularizacionRespondidaInput input)
        {
            tann_circularizacion webService = new tann_circularizacion();
            bool resultado = webService.cli_respuesta_circularizacion(input.rut_cli, input.fecha, input.respuesta, input.comentario);
            Resultado = resultado;
        }
    }
}