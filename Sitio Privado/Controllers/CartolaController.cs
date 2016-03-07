using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Sitio_Privado.Models;
using System.Threading.Tasks;
using Sitio_Privado.CartolaResumida;
using System.Configuration;
using Sitio_Privado.Extras;

namespace Sitio_Privado.Controllers
{
    public class CartolaController : ApiBaseController
    {
        private string authUsername = ConfigurationManager.AppSettings["ws:username"];
        private string authPassword = ConfigurationManager.AppSettings["ws:password"];

        [HttpPost]
        public async Task<IHttpActionResult> GetSingle([FromBody]CartolaInput input)
        {
            try
            {
                var usuario = await GetUsuarioActual();
                Cartola cartola = new Cartola(input, usuario);
                return Ok(cartola);
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IHttpActionResult> GetConceptosTitulo([FromBody]CartolaTituloInput input)
        {
            try
            {
                var usuario = await GetUsuarioActual();

                List<CartolaConcepto> conceptos = new List<CartolaConcepto>();

                tann_cartola_resumida webService = new tann_cartola_resumida();
                webService.AuthenticationValue = new Authentication { UserName = authUsername, Password = authPassword };

                _cartola_alt cartolaAlt = webService._cart_selector(usuario.Rut, input._selector);

                CartolaConcepto ultimoCartolaConcepto = new CartolaConcepto();

                if (cartolaAlt.conceptos.Length > 0)
                {
                    ultimoCartolaConcepto = new CartolaConcepto(cartolaAlt.conceptos.Last<_itemcartola>());
                    ultimoCartolaConcepto.Porcentaje = 100;
                }

                bool todosCeros = true;

                for (int i = 0; i < cartolaAlt.conceptos.Length - 1; i++)
                {
                    _itemcartola item = cartolaAlt.conceptos[i];

                    if (item._valor != 0) { todosCeros = false; }

                    CartolaConcepto cartolaConcepto = new CartolaConcepto(item);
                    cartolaConcepto.Porcentaje = Utils.GetPorcentaje(cartolaConcepto.Valor, ultimoCartolaConcepto.Valor);
                    conceptos.Add(cartolaConcepto);
                }

                if (todosCeros)
                {
                    foreach (CartolaConcepto cartolaConcepto in conceptos)
                    {
                        cartolaConcepto.Porcentaje = Utils.GetPorcentaje(1, conceptos.Count);
                    }
                }

                conceptos.Add(ultimoCartolaConcepto);

                return Ok(conceptos);
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }
    }
}
