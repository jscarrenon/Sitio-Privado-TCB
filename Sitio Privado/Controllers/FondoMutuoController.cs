using System;
using System.Collections.Generic;
using System.Web.Http;
using Sitio_Privado.Models;
using Sitio_Privado.ConsultaSaldosFondosMutuos;
using Sitio_Privado.Extras;
using Sitio_Privado.Filters;
using Sitio_Privado.Services.Interfaces;
using Sitio_Privado.Helpers;
using System.Security.Claims;

namespace Sitio_Privado.Controllers
{
   
    public class FondoMutuoController : ApiController
    {
        IHttpService httpService = null;
        IExternalUserService userService = null;

        public FondoMutuoController(IHttpService httpService, IExternalUserService userService) 
        {
            this.httpService = httpService;
            this.userService = userService;
        }

        [AuthorizeWithGroups(RequiredScopes = "openid profile")]
        [HttpPost]
        public IHttpActionResult GetList([FromBody]FondoMutuoInput input)
        {
            try
            {
                var usuario = userService.GetUserInfoByUsername(UserHelper.ExtractAuthorityId(User as ClaimsPrincipal));
                tann_fondos_mutuos webService = new tann_fondos_mutuos();
                int rutParteEntera = Converters.getRutParteEnteraInt(usuario.Rut);
                saldo_ffmm[] SaldosRF = webService.cn_saldo_ffmm_rf(rutParteEntera);
                saldo_ffmm[] SaldosRV = webService.cn_saldo_ffmm_rv(rutParteEntera);

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
                return InternalServerError(e);
            }
        }
    }
}