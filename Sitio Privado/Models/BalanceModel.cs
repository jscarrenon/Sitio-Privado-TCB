using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitio_Privado.InformacionClienteAgente;

namespace Sitio_Privado.Models
{
    public class BalanceInput
    {
    }

    public class Balance
    {
        public string Enlace { get; set; }

        public Balance() { }

        public Balance(BalanceInput input, Usuario usuario)
        {
            tann_info_cliente webService = new tann_info_cliente();
            string enlace = webService.cli_itoken(usuario.Rut);
            Enlace = enlace;
        }
    }
}