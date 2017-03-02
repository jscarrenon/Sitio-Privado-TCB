using Sitio_Privado.Services.Interfaces;
using System.Collections.Generic;
using System.Web;
using Sitio_Privado.Models;
using RestSharp;
using System.Net;
using Newtonsoft.Json;
using Sitio_Privado.Infraestructure.Constants;

namespace Sitio_Privado.Services
{
    public class AuthorityClientService : IAuthorityClientService
    {
        private const string UserInfoPath = "connect/userinfo";
        public virtual Usuario VerifyLoginAndGetPersonInformation(string accessToken, IEnumerable<string> roles)
        {
            Usuario usuario = null;
            string userRole = RoleConstants.Client;
            AuthorityUserInfo authorityUserInfo = GetUserInformation(accessToken);

            // GetUserFrom
            // person = repository.GetSingle<Person>(p => p.AuthorityId == authorityUserInfo.AuthorityId, true);

            if (usuario != null )
            {
                return null;
            }

            return usuario;
        }
        public AuthorityUserInfo GetUserInformation(string accessToken)
        {
            RestClient client = new RestClient();
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
                Rut = userInfo["rut"]
            };

            return userInformation;
        }
    }
}