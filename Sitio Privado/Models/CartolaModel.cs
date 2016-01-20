using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitio_Privado.CartolaResumida;

namespace Sitio_Privado.Models
{
    public class CartolaInput
    {
        public string _rut { get; set; }
        public short _secuencia { get; set; }
    }

    public class Cartola
    {
        public string Rut { get; set; }
        public string Periodo { get; set; }
        public double SaldoCaja { get; set; }
        public double SaldoCajaPorcentaje { get; set; }
        public double TotalRentaFija { get; set; }
        public double TotalRentaFijaPorcentaje { get; set; }
        public double InstrumentosRentaFija { get; set; }
        public double InstrumentosRentaFijaPorcentaje { get; set; }
        public double FondosMutuosRentaFija { get; set; }
        public double FondosMutuosRentaFijaPorcentaje { get; set; }
        public double TotalRentaVariable { get; set; }
        public double TotalRentaVariablePorcentaje { get; set; }
        public double AccionesNacionales { get; set; }
        public double AccionesNacionalesPorcentaje { get; set; }
        public double FondosMutuosRentaVariable { get; set; }
        public double FondosMutuosRentaVariablePorcentaje { get; set; }
        public double ForwardCompra { get; set; }
        public double ForwardCompraPorcentaje { get; set; }
        public double ForwardVenta { get; set; }
        public double ForwardVentaPorncentaje { get; set; }
        public double TotalInversiones { get; set; }
        public double TotalInversionesPorcentaje { get; set; }

        public Cartola() { }

        public Cartola(CartolaInput input)
        {
            tann_cartola_resumida webService = new tann_cartola_resumida();
            _cartola cartola = webService.cns_total_general(input._rut, input._secuencia);
            Rut = cartola._rut;
            Periodo = cartola._periodo;

            SaldoCaja = cartola._saldocaja;
            TotalRentaFija = cartola._totalrentafija;
            InstrumentosRentaFija = cartola._instrf;
            FondosMutuosRentaFija = cartola._ffmmrf;
            TotalRentaVariable = cartola._totalrentavari;
            AccionesNacionales = cartola._accnacion;
            FondosMutuosRentaVariable = cartola._ffmmrv;
            ForwardCompra = cartola._fwdcompra;
            ForwardVenta = cartola._fwdventa;
            TotalInversiones = cartola._totinversiones;

            SaldoCajaPorcentaje = GetPorcentaje(SaldoCaja, TotalInversiones);
            TotalRentaFijaPorcentaje = GetPorcentaje(TotalRentaFija, TotalInversiones);
            InstrumentosRentaFijaPorcentaje = GetPorcentaje(InstrumentosRentaFija, TotalInversiones);
            FondosMutuosRentaFijaPorcentaje = GetPorcentaje(FondosMutuosRentaFija, TotalInversiones);
            TotalRentaVariablePorcentaje = GetPorcentaje(TotalRentaVariable, TotalInversiones);
            AccionesNacionalesPorcentaje = GetPorcentaje(AccionesNacionales, TotalInversiones);
            FondosMutuosRentaVariablePorcentaje = GetPorcentaje(FondosMutuosRentaVariable, TotalInversiones);
            ForwardCompraPorcentaje = GetPorcentaje(ForwardCompra, TotalInversiones);
            ForwardVentaPorncentaje = GetPorcentaje(ForwardVenta, TotalInversiones);
            TotalInversionesPorcentaje = GetPorcentaje(TotalInversiones, TotalInversiones);
        }

        private static double GetPorcentaje(double monto, double total)
        {
            double resultado = 0;

            if(total != 0)
            {
                resultado = monto / total;
                resultado = resultado * 100;
            }

            return resultado;
        }
    }
}