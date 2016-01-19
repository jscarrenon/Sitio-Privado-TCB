using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitio_Privado.ConsultaSaldosFondosMutuos;

namespace Sitio_Privado.Models
{
    public class FondoMutuoInput
    {
        public int rut_cli { get; set; }
    }

    public class FondoMutuo
    {
        public string descripcion { get; set; } //Descriptor del Fondo Mutuo

        public string tipo { get; set; } //Tipo de Fondo

        public string ctaPisys { get; set; } //N° Cuenta del Fondo

        public decimal valor_cuota { get; set; } //Valor cuota actual

        public decimal saldo_cuota { get; set; } //Saldo en cuotas

        public string csbis { get; set; } //Indicador Tributario

        public string renta { get; set; } //Tipo de Renta

        public decimal pesos { get; set; } //Saldo Pesos

        public FondoMutuo() { }

        public FondoMutuo(saldo_ffmm saldo)
        {
            descripcion = saldo.descripcion;
            tipo = saldo.tipo;
            ctaPisys = saldo.cta_pisys;
            valor_cuota = saldo.valor_cuota;
            saldo_cuota = saldo.saldo_cuota;
            csbis = saldo.csbis; //Indicador Tributario
            renta = saldo.renta;
            pesos = saldo.pesos;
        }
              
    }
}