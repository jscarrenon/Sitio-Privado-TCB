using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitio_Privado.Extras
{
    public static class Converters
    {
        /// <summary>
        /// Convierte string de fecha a tipo DateTime
        /// </summary>
        /// <param name="fecha">Fecha en formato dd-MM-yyyy HH:mm:ss</param>
        /// <returns>DateTime</returns>
        public static DateTime getFecha(string fecha)
        {
            fecha = fecha.Substring(0, 10);
            return DateTime.ParseExact(fecha, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Retorna la parte entera de un rut (antes del guión)
        /// </summary>
        /// <param name="rut">Rut en formato "12345678-9"</param>
        /// <returns>String con parte entera del rut</returns>
        public static string getRutParteEntera(string rut)
        {
            if (!string.IsNullOrEmpty(rut)) {
                int index = rut.IndexOf('-');
                if (index != -1) {
                    return rut.Substring(0, index);
                }
            }
            return "";
        }

        /// <summary>
        /// Retorna la parte entera de un rut (antes del guión) como entero
        /// </summary>
        /// <param name="rut">Rut en formato "12345678-9"</param>
        /// <returns>Int con parte entera del rut</returns>
        public static int getRutParteEnteraInt(string rut)
        {
            return Convert.ToInt32(getRutParteEntera(rut));
        }

    }
}