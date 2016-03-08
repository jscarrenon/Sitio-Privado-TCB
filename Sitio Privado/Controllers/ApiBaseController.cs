﻿using Sitio_Privado.Filters;
using Sitio_Privado.Helpers;
using Sitio_Privado.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Sitio_Privado.Controllers
{
    [NoCache]
    public abstract class ApiBaseController : ApiController
    {
        private static string ObjectIdClaim = "http://schemas.microsoft.com/identity/claims/objectidentifier";
        GraphApiClientHelper graphApiHelper = new GraphApiClientHelper();

        private Usuario Usuario
        {
            get { return new Usuario(base.User as ClaimsPrincipal); }
        }

        public async Task<Usuario> GetUsuarioActual()
        {
            var usuario = this.Usuario;
            await SetUserExtendedAttributes(usuario);
            return usuario;
        }

        private async Task SetUserExtendedAttributes(Usuario usuario)
        {
            //Retrieve user info
            Claim idClaim = ((ClaimsIdentity)usuario.Identity).Claims.Where(c => c.Type == ObjectIdClaim).First();
            GraphApiResponseInfo response = await graphApiHelper.GetUserByObjectId(idClaim.Value);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                //Save used info
                usuario.Banco = response.User.Bank;
                usuario.CuentaCorriente = response.User.CheckingAccount;
                usuario.Email = response.User.Email;
                usuario.TelefonoComercial = response.User.WorkPhone;
                usuario.TelefonoParticular = response.User.HomePhone;
                usuario.DireccionComercial = response.User.WorkAddress;
                usuario.DireccionParticular = response.User.HomeAddress;
                usuario.Rut = response.User.Rut.Insert(response.User.Rut.Length - 1, "-");
            }
        }
    }
}