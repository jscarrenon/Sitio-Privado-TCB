using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitio_Privado.ConsultaSaldosFondosMutuos;

namespace Sitio_Privado.Models
{
    public class FondoMutuoModel
    {
        public class FondoMutuoInput
        {
            public string rut_cli { get; set; }
        }

        public class FondoMutuo
        {
            public string descripcion { get; set; } //Descriptor del Fondo Mutuo

            public string tipo { get; set; } //Tipo de Fondo

            public string ctaPisys { get; set; } //N° Cuenta del Fondo

            public int valor_cuota { get; set; } //Valor cuota actual

            public int saldo_cuota { get; set; } //Saldo en cuotas

            public string renta { get; set; } //Tipo de Renta

            public int pesos { get; set; } //Saldo Pesos

            public FondoMutuo() { }

            public FondoMutuo(FondoMutuoInput input)
            {
                tann_fondos_mutuos webService = new tann_fondos_mutuos();
                //_agente agente = webService.cli_info_agente(input._rut, input._sec);
                //Codigo = agente._codigo;
                //Nombre = agente._nombre;
                //Sucursal = agente._sucursal;
                //Email = agente._email;
                //PathImg = agente._pathimg;
            }
        }        
    }
}