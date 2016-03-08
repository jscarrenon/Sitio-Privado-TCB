using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitio_Privado.Extras
{
    public static class Utils
    {
        public static double GetPorcentaje(double numerador, double denominador)
        {
            double resultado = 0;

            if (denominador != 0)
            {
                resultado = GetVeces(numerador, denominador);
                resultado = resultado * 100;
            }

            return resultado;
        }

        public static double GetVeces(double numerador, double denominador)
        {
            double resultado = 0;

            if (denominador != 0)
            {
                resultado = numerador / denominador;
            }

            return resultado;
        }
    }
}