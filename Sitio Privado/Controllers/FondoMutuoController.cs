using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Sitio_Privado.Models;
using Sitio_Privado.ConsultaSaldosFondosMutuos;

namespace Sitio_Privado.Controllers
{
    public class FondoMutuoController : ApiController
    {
        [HttpPost]
        public IHttpActionResult GetList([FromBody]FondoMutuoInput input)
        {
            try
            {
                tann_fondos_mutuos webService = new tann_fondos_mutuos();
                saldo_ffmm[] SaldosRF = webService.cn_saldo_ffmm_rf(input.rut_cli);
                saldo_ffmm[] SaldosRV = webService.cn_saldo_ffmm_rv(input.rut_cli);

                List<FondoMutuo> fondosMutuosRF = new List<FondoMutuo>();
                List<FondoMutuo> fondosMutuosRV = new List<FondoMutuo>();

                foreach (saldo_ffmm saldo in SaldosRF)
                {
                    fondosMutuosRF.Add(new FondoMutuo(saldo));
                }

                foreach(saldo_ffmm saldo in SaldosRV)
                {
                    fondosMutuosRV.Add(new FondoMutuo(saldo));
                }                               

                Dictionary<string, List<FondoMutuo>> fondosMutuosDictionary = new Dictionary<string, List<FondoMutuo>>();
                fondosMutuosDictionary.Add("fondosMutuosRF", fondosMutuosRF);
                fondosMutuosDictionary.Add("fondosMutuosRV", fondosMutuosRV);
                return Ok(fondosMutuosDictionary);
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }
    }
}