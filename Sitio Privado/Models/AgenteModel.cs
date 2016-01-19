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
        public int codigo { get; set; }
        public string nombre { get; set; }
        public string sucursal { get; set; }
        public string email { get; set; }
        public string pathImg { get; set; }
        public string telefono { get; set; }
        public string fechaInicioAcreditacion { get; set; }
        public string fechaExpiracionAcreditacion { get; set; }
        public string descriptor { get; set; }

        public Agente() { }

        public Agente(AgenteInput input)
        {
            tann_info_cliente webService = new tann_info_cliente();
            _agente agente = webService.cli_info_agente(input._rut, input._sec);
            codigo = agente._codigo;
            nombre = agente._nombre;
            sucursal = agente._sucursal;
            email = agente._email;
            pathImg = agente._pathimg;
            telefono = agente._fono;
            fechaInicioAcreditacion = agente._fechacert;
            fechaExpiracionAcreditacion = agente._fechavcto;
            descriptor = agente._glosacert;
        }
    }
}