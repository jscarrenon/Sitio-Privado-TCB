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

        public CartolaConcepto(_cartola_alt item)
        {
            Concepto = item;
            Valor = item._valor;
            Porcentaje = item._porcentaje;
        }
    }

    public class CartolaTitulo
    {
        public int Codigo { get; set; }
        public string Descriptor { get; set; }

        public CartolaTitulo() { }
        public CartolaTitulo(_titulo titulo)
        {
            Codigo = titulo._code;
            Descriptor = titulo._descriptor;
        }
    }

    public class Cartola
    {
        List<CartolaTitulo> Titulos { get; set; }
        public string Rut { get; set; }
        public string Periodo { get; set; }
        public List<CartolaConcepto> Conceptos { get; set; }

        public Cartola() { }

        public Cartola(CartolaInput input, Usuario usuario)
        {
            tann_cartola_resumida webService = new tann_cartola_resumida();
            _titulos titulosWeb = webService.cns_titulos_cartola();
            Rut = usuario.Rut;
            foreach (_titulo tituloWeb in titulosWeb._listitulos)
            {
                Titulos.Add(new CartolaTitulo(tituloWeb));
            }
            foreach(CartolaTitulo titulo in Titulos)
            {
                var concepto = webService._cart_selector(Rut, titulo.Codigo);
                CartolaConcepto(concepto);
            }
            //_cartola_alt cartola = webService.cns_total_general(u);

            Periodo = DateTime.Today.ToString("dd-MM-yyyy");
            Conceptos = new List<CartolaConcepto>();
            webService._cart_selector
            foreach(_itemcartola item in cartola.conceptos)
            {
                Conceptos.Add(new CartolaConcepto(item));
            }
        }
    }
}