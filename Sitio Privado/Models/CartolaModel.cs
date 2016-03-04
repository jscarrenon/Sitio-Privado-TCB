using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitio_Privado.CartolaResumida;

namespace Sitio_Privado.Models
{
    public class CartolaInput
    {
    }

    public class CartolaConcepto
    {
        public string Concepto { get; set; }
        public double Valor { get; set; }
        public double Porcentaje { get; set; }

        public CartolaConcepto() { }

        public CartolaConcepto(_itemcartola item)
        {
            Concepto = item.concepto;
            Valor = item._valor;
            Porcentaje = item._porcentaje;
        }
    }

    public class Cartola
    {
        public string Rut { get; set; }
        public string Periodo { get; set; }
        public List<CartolaConcepto> Conceptos { get; set; }

        public Cartola() { }

        public Cartola(CartolaInput input, Usuario usuario)
        {
            tann_cartola_resumida webService = new tann_cartola_resumida();
            _cartola_alt cartola = webService.cns_total_general(usuario.Rut);
            Rut = cartola._rutcli;
            Periodo = cartola._periodo;
            Conceptos = new List<CartolaConcepto>();

            foreach(_itemcartola item in cartola.conceptos)
            {
                Conceptos.Add(new CartolaConcepto(item));
            }
        }
    }
}