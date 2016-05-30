

namespace Sitio_Privado.Extras
{
    public static class ExtraHelpers
    {
        /// <summary>
        /// Format rut as B2C User Id (Ex: 12345678K)
        /// </summary>
        /// <param name="rut">Rut</param>
        /// <returns>Rut as Id</returns>
        public static string FormatRutToId(string rut)
        {
            string id = "";

            if (!string.IsNullOrEmpty(rut))
            {
                id = rut.Replace(".", null);
                id = id.Replace("-", null);
                id = id.Replace("k", "K");
            }

            return id;
        }

        //Ex: 12.345.678-K
        //Source: http://www.qualityinfosolutions.com/formatear-rut-chileno-en-c/
        /// <summary>
        /// Format string as rut (Ex: 12.345.678-K)
        /// </summary>
        /// <param name="rut">Rut</param>
        /// <returns>String with rut format</returns>
        public static string FormatRutToText(string rut)
        {
            string format;
            if (string.IsNullOrEmpty(rut))
            {
                format = "";
            }
            else
            {
                int cont = 0;
                rut = rut.Replace(".", "");
                rut = rut.Replace("-", "");
                format = "-" + rut.Substring(rut.Length - 1);
                for (int i = rut.Length - 2; i >= 0; i--)
                {
                    format = rut.Substring(i, 1) + format;
                    cont++;
                    if (cont == 3 && i != 0)
                    {
                        format = "." + format;
                        cont = 0;
                    }
                }
            }

            return format;
        }
    }
}