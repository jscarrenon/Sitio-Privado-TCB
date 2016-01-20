using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitio_Privado.InformacionClienteAgente;

namespace Sitio_Privado.Models
{
    public class BalanceInput
    {
        public string rut { get; set; }
    }

    public class Balance
    {
        public string Enlace { get; set; }

        public Balance() { }

        public Balance(BalanceInput input)
        {
            tann_info_cliente webService = new tann_info_cliente();
            string enlace = webService.cli_itoken(input.rut);
            Enlace = enlace;
        }
    }
}