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
                saldo_ffmm[] fondosMutuosRV = webService.cn_saldo_ffmm_rv(input.rut_cli);
                List<FondoMutuo> listaFondosMutuosCliente = new List<FondoMutuo>();

                foreach (saldo_ffmm saldoFondoMutuo in fondosMutuosRF)
                {
                    FondoMutuo fondoMutuo = new FondoMutuo();
                    fondoMutuo.descripcion = saldoFondoMutuo.descripcion;
                    fondoMutuo.tipo = saldoFondoMutuo.tipo;
                    fondoMutuo.ctaPisys = saldoFondoMutuo.cta_pisys;
                    fondoMutuo.valor_cuota = saldoFondoMutuo.valor_cuota;
                    fondoMutuo.saldo_cuota = saldoFondoMutuo.saldo_cuota;
                    fondoMutuo.csbis = saldoFondoMutuo.csbis;
                    fondoMutuo.renta = saldoFondoMutuo.renta;
                    fondoMutuo.pesos = saldoFondoMutuo.pesos;
                    listaFondosMutuosCliente.Add(fondoMutuo);
                }

                foreach (saldo_ffmm saldoFondoMutuo in fondosMutuosRV)
                {
                    FondoMutuo fondoMutuo = new FondoMutuo();
                    fondoMutuo.descripcion = saldoFondoMutuo.descripcion;
                    fondoMutuo.tipo = saldoFondoMutuo.tipo;
                    fondoMutuo.ctaPisys = saldoFondoMutuo.cta_pisys;
                    fondoMutuo.valor_cuota = saldoFondoMutuo.valor_cuota;
                    fondoMutuo.saldo_cuota = saldoFondoMutuo.saldo_cuota;
                    fondoMutuo.csbis = saldoFondoMutuo.csbis;
                    fondoMutuo.renta = saldoFondoMutuo.renta;
                    fondoMutuo.pesos = saldoFondoMutuo.pesos;
                    listaFondosMutuosCliente.Add(fondoMutuo);
                }

                Dictionary<string, saldo_ffmm[]> fondosMutuosDictionary = new Dictionary<string, saldo_ffmm[]>();
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