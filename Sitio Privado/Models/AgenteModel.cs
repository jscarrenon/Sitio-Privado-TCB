using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitio_Privado.InformacionClienteAgente;

namespace Sitio_Privado.Models
{
    public class AgenteInput
    {
    }

    public class Agente
    {
        public int Codigo { get; set; }
        public string Nombre { get; set; }
        public string Sucursal { get; set; }
        public string Email { get; set; }
        public string PathImg { get; set; }
        public string Telefono { get; set; }
        public string FechaInicioAcreditacion { get; set; }
        public string FechaExpiracionAcreditacion { get; set; }
        public string Descriptor { get; set; }

        public Agente() { }

        public Agente(AgenteInput input, Usuario usuario)
        {
            tann_info_cliente webService = new tann_info_cliente();
            _agente agente = webService.cli_info_agente(usuario.Rut, 0);
            Codigo = agente._codigo;
            Nombre = agente._nombre;
            Sucursal = agente._sucursal;
            Email = agente._email;
            PathImg = agente._pathimg;
            Telefono = agente._fono;
            FechaInicioAcreditacion = agente._fechacert;
            FechaExpiracionAcreditacion = agente._fechavcto;
            Descriptor = agente._glosacert;
        }
    }
}