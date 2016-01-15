using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitio_Privado.InformacionClienteAgente;

namespace Sitio_Privado.Models
{
    public class AgenteInput
    {
        public string _rut { get; set; }
        public short _sec { get; set; }
    }

    public class Agente
    {
        public int Codigo { get; set; }

        public string Nombre { get; set; }

        public string Sucursal { get; set; }

        public string Email { get; set; }

        public string PathImg { get; set; }

        public string Telefono { get; set; }

        public string FechaAcreditacion { get; set; }

        public Agente() { }

        public Agente(AgenteInput input)
        {
            tann_info_cliente webService = new tann_info_cliente();
            _agente agente = webService.cli_info_agente(input._rut, input._sec);
            Codigo = agente._codigo;
            Nombre = agente._nombre;
            Sucursal = agente._sucursal;
            Email = agente._email;
            PathImg = agente._pathimg;
        }
    }
}