using Sitio_Privado.Services.Interfaces;
using System.Collections.Generic;
using System.Web;
using Sitio_Privado.Models;
using RestSharp;
using System.Net;
using Newtonsoft.Json;
using Sitio_Privado.Infraestructure.Constants;
using Microsoft.IdentityModel.Protocols;
using System.Configuration;
using System;
using Sitio_Privado.Helpers;

namespace Sitio_Privado.Services
{
    public class AuthorityClientService : IAuthorityClientService
    {
        private const string UserInfoPath = "connect/userinfo";
        IExternalUserService userService;
        public AuthorityClientService(IExternalUserService userService)
        {
            this.userService = userService;

        }
        public virtual Person VerifyLoginAndGetPersonInformation(string accessToken, IEnumerable<string> roles)
        {
            Person persona = null;
            //string userRole = RoleConstants.Client;
            AuthorityUserInfo authorityUserInfo = GetUserInformation(accessToken);

            // GetUserFrom
            // person = repository.GetSingle<Person>(p => p.AuthorityId == authorityUserInfo.AuthorityId, true);

            if (authorityUserInfo != null )
            {
                persona = new Person();
                persona.Autenticado = true;
                persona.Rut = authorityUserInfo.Rut;
                persona.Nombres = authorityUserInfo.GivenName;
                persona.Apellidos = authorityUserInfo.LastName;
                persona.NombreCompleto = authorityUserInfo.GivenName + " " + authorityUserInfo.LastName + " " + authorityUserInfo.MothersLastName;
                persona.Email = authorityUserInfo.Email;

                //UserHelper.AttachPersonAsUserClaims()
            }

            return persona;
        }
        public AuthorityUserInfo GetUserInformation(string accessToken)
        {
            RestClient client = new RestClient();
            client.BaseUrl = new Uri(ConfigurationManager.AppSettings["AuthorityUrl"]);
            var request = new RestRequest(UserInfoPath, Method.POST);
            request.AddHeader("Authorization", "Bearer " + accessToken);
            IRestResponse response = client.Execute(request);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new HttpException((int)response.StatusCode, "Error obtaining user information from authority server");
            }

            var content = response.Content;
            dynamic userInfo = JsonConvert.DeserializeObject(content);

            AuthorityUserInfo userInformation = new AuthorityUserInfo()
            {
                AuthorityId = userInfo["sub"],
                GivenName = userInfo["first_name"],
                LastName = userInfo["last_name"],
                MothersLastName = userInfo["mothers_last_name"],
                Rut = userInfo["rut"],
                Email = userInfo["email"]
            };

            return userInformation;
        }

        public Person GetPersonInformationByToken(string accessToken)
        {
            Person persona = null;
            AuthorityUserInfo authorityUserInfo = GetUserInformation(accessToken);

            if (authorityUserInfo != null)
            {
                persona = new Person();
                persona.Autenticado = true;
                persona.Rut = authorityUserInfo.Rut;
                persona.Nombres = authorityUserInfo.GivenName;
                persona.Apellidos = authorityUserInfo.LastName;
                persona.NombreCompleto = authorityUserInfo.GivenName + " " + authorityUserInfo.LastName + " " + authorityUserInfo.MothersLastName;
                persona.Email = authorityUserInfo.Email;

            }

            return persona;
        }
        public Usuario GetUserInformationByToken(string accessToken)
        {
            Usuario usuario = null;
            AuthorityUserInfo authorityUserInfo = GetUserInformation(accessToken);

            if (authorityUserInfo != null)
            {
                var userInfo = userService.GetUserInfoByUsername(authorityUserInfo.Rut);

                usuario = new Usuario()
                {
                    Rut = userInfo.Rut.Insert(userInfo.Rut.Length - 1, "-"),
                    Banco = userInfo.Bank,
                    CuentaCorriente = userInfo.CheckingAccount,
                    DireccionComercial = userInfo.WorkAddress,
                    DireccionParticular = userInfo.HomeAddress,
                    Email = userInfo.Email,
                    TelefonoComercial = userInfo.WorkPhone
                };
            }

            return usuario;
        }
    }
}