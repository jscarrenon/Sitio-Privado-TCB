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

    public class CartolaTituloInput
    {
        public int _selector { get; set; }
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
        public string Rut { get; set; }
        public string Periodo { get; set; }
        public List<CartolaConcepto> Conceptos { get; set; }

        public CartolaTitulo() { }

        public CartolaTitulo(_titulo titulo)
        {
            Codigo = titulo._code;
            Descriptor = titulo._descriptor;
            Conceptos = new List<CartolaConcepto>();
        }
    }

    public class CartolaConceptosTituloResultado
    {
        public string Rut { get; set; }
        public string Periodo { get; set; }
        public List<CartolaConcepto> Conceptos { get; set; }

        public CartolaConceptosTituloResultado() { }
    }

    public class Cartola
    {
        private string authUsername = ConfigurationManager.AppSettings["ws:username"];
        private string authPassword = ConfigurationManager.AppSettings["ws:password"];

        public List<CartolaTitulo> Titulos { get; set; }

        public Cartola() { Titulos = new List<CartolaTitulo>(); }

        public Cartola(CartolaInput input, Usuario usuario)
        {
            Titulos = new List<CartolaTitulo>();

            tann_cartola_resumida webService = new tann_cartola_resumida();
            webService.AuthenticationValue = new Authentication{ UserName = authUsername, Password = authPassword };
            _titulos titulosWebService = webService.cns_titulos_cartola();

            foreach (_titulo tituloWebService in titulosWebService._listitulos)
            {
                Titulos.Add(new CartolaTitulo(tituloWebService) { });
            }
        }
    }
}