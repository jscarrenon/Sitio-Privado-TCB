using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitio_Privado.CircularizacionCustodia;
using Sitio_Privado.Extras;

namespace Sitio_Privado.Models
{
    public class CircularizacionPendienteInput
    {
    }

    public class CircularizacionArchivoInput
    {
    }

    public class CircularizacionLeidaInput
    {
    }

    public class CircularizacionRespondidaInput
    {
        public string respuesta { get; set; } // "S" ó "N"
        public string comentario { get; set; } //Sólo en caso de rechazo ("N")
    }

    public class CircularizacionArchivo
    {
        public string UrlCartola { get; set; }
        public string UrlCircularizacion { get; set; }

        public CircularizacionArchivo() { }

        public CircularizacionArchivo(CircularizacionArchivoInput input, Usuario usuario)
        {
            tann_circularizacion webService = new tann_circularizacion();
            archivocli archivo = webService.cli_archivo_circularizacion(usuario.Rut);

            UrlCartola = archivo._cartola;
            UrlCircularizacion = archivo._circula;
        }
    }

    public class CircularizacionProcesoResultado
    {
        public bool Resultado { get; set; }

        public CircularizacionProcesoResultado(CircularizacionPendienteInput input, Usuario usuario)
        {
            tann_circularizacion webService = new tann_circularizacion();
            bool resultado = webService.cli_circularizacion(Converters.getRutParteEnteraInt(usuario.Rut));
            Resultado = resultado;
        }

        public CircularizacionProcesoResultado(CircularizacionLeidaInput input, Usuario usuario)
        {
            tann_circularizacion webService = new tann_circularizacion();
            bool resultado = webService.cli_leer_circularizacion(Converters.getRutParteEnteraInt(usuario.Rut));
            Resultado = resultado;
        }

        public CircularizacionProcesoResultado(CircularizacionRespondidaInput input, Usuario usuario)
        {
            tann_circularizacion webService = new tann_circularizacion();
            bool resultado = webService.cli_respuesta_circularizacion(Converters.getRutParteEnteraInt(usuario.Rut), input.respuesta, input.comentario);
            Resultado = resultado;
        }
    }
}