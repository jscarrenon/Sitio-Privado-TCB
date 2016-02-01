using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitio_Privado.IndicesLiquidezSolvencia;

namespace Sitio_Privado.Models
{
    public class IndicesInput
    {
        public string xfecha { get; set; }
    }

    public class Indices
    {
        public string RutTCB { get; set; }
        public string DescriptorTCB { get; set; }
        public double ActivosSieteDias { get; set; }
        public double PasivosSieteDias { get; set; }
        public double ActivosIntermediacion { get; set; }
        public double AcreedoresIntermediacion { get; set; }
        public double TotalPasivosExigibles { get; set; }
        public double PatrimonioLiquido { get; set; }
        public double MontoCoberturaPatrimonial { get; set; }
        public double PatrimonioDepurado { get; set; }
        public string FechaConsulta { get; set; }

        public double LiquidezGeneral { get; set; }
        public double LiquidezIntermediacion { get; set; }
        public double RazonEndeudamiento { get; set; }
        public double RazonCoberturaPatrimonial { get; set; }

        public Indices() { }

        public Indices(IndicesInput input)
        {
            tann_indicadores webService = new tann_indicadores();
            _indicadores indices = webService.cns_ind_liq(input.xfecha);

            RutTCB = indices._rutemp;
            DescriptorTCB = indices._nomemp;
            ActivosSieteDias = indices._adrsietedias;
            PasivosSieteDias = indices._pexsietedias;
            ActivosIntermediacion = indices._addif;
            AcreedoresIntermediacion = indices._accif;
            TotalPasivosExigibles = indices._totpasex;
            PatrimonioLiquido = indices._patrliq;
            MontoCoberturaPatrimonial = indices._moncobpart;
            PatrimonioDepurado = indices._patdep;
            FechaConsulta = indices._fecha;

            LiquidezGeneral = GetVeces(ActivosSieteDias, PasivosSieteDias);
            LiquidezIntermediacion = GetVeces(ActivosIntermediacion, AcreedoresIntermediacion);
            RazonEndeudamiento = GetVeces(TotalPasivosExigibles, PatrimonioLiquido);
            RazonCoberturaPatrimonial = GetPorcentaje(MontoCoberturaPatrimonial, PatrimonioLiquido);
        }

        private static double GetPorcentaje(double numerador, double denominador)
        {
            double resultado = 0;

            if(denominador != 0)
            {
                resultado = GetVeces(numerador, denominador);
                resultado = resultado * 100;
            }

            return resultado;
        }

        private static double GetVeces(double numerador, double denominador)
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