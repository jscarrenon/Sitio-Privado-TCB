using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Sitio_Privado.Models;
using System.Web.Mvc;
using Sitio_Privado.ConsultaSaldosFondosMutuos;

namespace Sitio_Privado.Controllers
{
    public class FondoMutuoController : ApiController
    {
        // POST api/fondosMutuos
        public IHttpActionResult Post([FromBody]FondoMutuoInput input)
        {
            try
            {
                tann_fondos_mutuos webService = new tann_fondos_mutuos();
                saldo_ffmm[] fondosMutuosRF = webService.cn_saldo_ffmm_rf(input.rut_cli);
                saldo_ffmm[] fondosMutuosRV = webService.cn_saldo_ffmm_rv(6190555);

                Dictionary<string, saldo_ffmm[]> fondosMutuosDictionary = new Dictionary<string, saldo_ffmm[]>();
                fondosMutuosDictionary.Add("fondosMutuosRF", fondosMutuosRF);
                fondosMutuosDictionary.Add("fondosMutuosRV", fondosMutuosRV);
                foreach(saldo_ffmm[] fondosMutuosArray in fondosMutuosDictionary.Values)
                {
                    foreach(saldo_ffmm fondoMutuo in fondosMutuosArray)
                    {
                        fondoMutuo.cta_pisys = fondoMutuo.cta_pisys.Trim();
                    }
                }
                return Ok(fondosMutuosDictionary);
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }
    }
}