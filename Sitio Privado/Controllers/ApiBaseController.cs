using Sitio_Privado.Filters;
using Sitio_Privado.Models;
using Sitio_Privado.Services.ExternalUserProvider;
using Sitio_Privado.Services.Interfaces;
using System.Security.Claims;
using System.Web.Http;

namespace Sitio_Privado.Controllers
{
    [NoCache]
    public abstract class ApiBaseController : ApiController
    {
        private IExternalUserService userProvider = new LDAPService();
      
        private Usuario Usuario
        {
            get { return new Usuario(base.User as ClaimsPrincipal); }
        }
      
        public Usuario GetUsuarioActual(Person person)
        {
            var usuario = this.Usuario;
           
            if (usuario.Autenticado)
            {
                usuario.Rut = person.Rut;
                usuario.Email = person.Email;

                SetUserExtendedAttributes(usuario);

            }
            return usuario;
        }
       
        private void SetUserExtendedAttributes(Usuario usuario)
        {
            
            //Retrieve user info to inject in middleware
           // Claim idClaim = ((ClaimsIdentity)usuario.Identity).Claims.Where(c => c.Type == Startup.objectIdClaimKey).First();
            // GraphApiResponseInfo response = await graphApiHelper.GetUserByObjectId(idClaim.Value);
            //var userData = userProvider.GetUserInfoByUsername(usuario.Rut);

            //if (userData != null)
            //{
            //    //Save used info
            //    usuario.Banco = userData.Bank;
            //    usuario.CuentaCorriente = userData.CheckingAccount;
            //    usuario.Email = userData.Email;
            //    usuario.TelefonoComercial = userData.WorkPhone;
            //    //usuario.TelefonoParticular = userData.HomePhone;
            //    usuario.DireccionComercial = userData.WorkAddress;
            //    usuario.DireccionParticular = userData.HomeAddress;
            //    //usuario.Rut = response.User.Rut.Insert(response.User.Rut.Length - 1, "-");
            //}


            // dummy

            //usuario.Banco = "Banco";
            //usuario.CuentaCorriente = "111111111111";
            //usuario.Email = "em@il.com";
            //usuario.TelefonoComercial = "22222222";
            //usuario.TelefonoParticular = "3333333";
            //usuario.DireccionComercial = "Dirección comercial";
            //usuario.DireccionParticular = "Dirección particular";
            //usuario.Rut = "17435156-2";
        }
    }
}