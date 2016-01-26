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

    }
}