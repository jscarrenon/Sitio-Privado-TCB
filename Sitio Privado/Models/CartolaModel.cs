using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitio_Privado.CartolaResumida;
using Sitio_Privado.Extras;
using System.Configuration;

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
        }
    }

    public class CartolaTitulo
    {
        public int Codigo { get; set; }
        public string Descriptor { get; set; }
        public List<CartolaConcepto> Conceptos { get; set; }

        public CartolaTitulo() { }

        public CartolaTitulo(_titulo titulo)
        {
            Codigo = titulo._code;
            Descriptor = titulo._descriptor;
            Conceptos = new List<CartolaConcepto>();
        }
    }

    public class Cartola
    {
        private string authUsername = ConfigurationManager.AppSettings["ws:username"];
        private string authPassword = ConfigurationManager.AppSettings["ws:password"];

        public string Rut { get; set; }
        public string Periodo { get; set; }
        public List<CartolaTitulo> Titulos { get; set; }

        public Cartola() { }

        public Cartola(CartolaInput input, Usuario usuario)
        {
            tann_cartola_resumida webService = new tann_cartola_resumida();
            webService.AuthenticationValue = new Authentication{ UserName = authUsername, Password = authPassword };
            _titulos titulosWebService = webService.cns_titulos_cartola();

            foreach (_titulo tituloWebService in titulosWebService._listitulos)
            {
                CartolaTitulo cartolaTitulo = new CartolaTitulo(tituloWebService);

                _cartola_alt cartolaAlt = webService._cart_selector(usuario.Rut, cartolaTitulo.Codigo);

                Rut = cartolaAlt._rutcli;
                Periodo = cartolaAlt._periodo;

                double totalConceptos = 0;

                foreach(_itemcartola item in cartolaAlt.conceptos)
                {
                    CartolaConcepto cartolaConcepto = new CartolaConcepto(item);
                    totalConceptos += cartolaConcepto.Valor;
                    cartolaTitulo.Conceptos.Add(cartolaConcepto);
                }

                foreach(CartolaConcepto cartolaConcepto in cartolaTitulo.Conceptos)
                {
                    cartolaConcepto.Porcentaje = Utils.GetPorcentaje(cartolaConcepto.Valor, totalConceptos);
                }

                Titulos.Add(cartolaTitulo);
            }

            if (string.IsNullOrEmpty(Rut)) {
                Rut = usuario.Rut;
            }

            if (string.IsNullOrEmpty(Periodo)) {
                Periodo = DateTime.Today.ToString("dd-MM-yyyy");
            }
        }
    }
}