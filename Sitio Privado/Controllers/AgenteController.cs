﻿using System;
using System.Web.Http;
using Sitio_Privado.Models;
using Sitio_Privado.Services.Interfaces;
using Sitio_Privado.Filters;
using Sitio_Privado.Helpers;
using System.Security.Claims;

namespace Sitio_Privado.Controllers
{
    public class AgenteController : ApiBaseController
    {
        IHttpService httpService = null;
        IExternalUserService userService = null;

        public AgenteController(IHttpService httpService, IExternalUserService userService) 
            
        {
            this.httpService = httpService;
            this.userService = userService;
        }
        [AuthorizeWithGroups(RequiredScopes = "openid profile")]
        [HttpPost]
        public IHttpActionResult GetSingle([FromBody]AgenteInput input)
        {
            try
            {
                var usuario = userService.GetUserInfoByUsername(UserHelper.ExtractAuthorityId(User as ClaimsPrincipal));

                Agente agente = new Agente(input, usuario);
                return Ok(agente);
            }
            catch(Exception e)
            {
                return InternalServerError(e);
            }
        }
    }
}