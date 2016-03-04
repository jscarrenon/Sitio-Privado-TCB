using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitio_Privado.ConsultaSaldosFondosMutuos;

namespace Sitio_Privado.Models
{
    public class FondoMutuoInput
    {
    }

    public class FondoMutuo
    {
        public string Descripcion { get; set; } //Descriptor del Fondo Mutuo

        public string Tipo { get; set; } //Tipo de Fondo

        public string CtaPisys { get; set; } //N° Cuenta del Fondo

        public decimal ValorCuota { get; set; } //Valor cuota actual

        public decimal SaldoCuota { get; set; } //Saldo en cuotas

        public string Csbis { get; set; } //Indicador Tributario

        public string Renta { get; set; } //Tipo de Renta

        public decimal Pesos { get; set; } //Saldo Pesos

        public FondoMutuo() { }

        public FondoMutuo(saldo_ffmm saldo)
        {
            Descripcion = saldo.descripcion;
            Tipo = saldo.tipo;
            CtaPisys = saldo.cta_pisys.Trim();
            ValorCuota = saldo.valor_cuota;
            SaldoCuota = saldo.saldo_cuota;
            Csbis = saldo.csbis; //Indicador Tributario
            Renta = saldo.renta;
            Pesos = saldo.pesos;
        }
              
    }
}