using Newtonsoft.Json.Linq;
using Sitio_Privado.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Sitio_Privado.Controllers
{
    public class SyncApiController : ApiController
    {
        private SyncApiClientHelper syncApiHelper = new SyncApiClientHelper();

        [HttpGet]
        public string Action() {
            return "action";
        }

        [HttpGet]
        public async Task<HttpResponseMessage> GetUsers() {
            return await syncApiHelper.GetAllUsers(null);
        }

        [HttpPost]
        public async Task<HttpResponseMessage> CreateUser(HttpRequestMessage request) {
            dynamic content = JObject.Parse(await request.Content.ReadAsStringAsync());
            string json = GetCreateUserRequestBody(content);
            return await syncApiHelper.CreateUser(json);
        }

        private string GetCreateUserRequestBody(dynamic content)
        {
            JObject json = new JObject();
            json.Add("accountEnabled", true);
            json.Add("creationType", "NameCoexistence");
            json.Add("displayName", "Fake User");
            json.Add("mailNickname", "NickName");
            json.Add("city", "Santiago");
            json.Add("givenName", "Fake");
            json.Add("postalCode", "9999999");
            json.Add("state", "Santiago");
            json.Add("surname", "User");
            json.Add("passwordPolicies", "DisablePasswordExpiration");

            JObject passwordProfile = new JObject();
            passwordProfile.Add("password", "Kunder2015");
            passwordProfile.Add("forceChangePasswordNextLogin", true);
            json.Add("passwordProfile", passwordProfile);

            JObject signInAlternative = new JObject();
            signInAlternative.Add("type", "userName");
            signInAlternative.Add("value", content.rut);
            JArray signInAlternativesArray = new JArray(signInAlternative);

            json.Add("alternativeSignInNamesInfo", signInAlternativesArray);

            return json.ToString();
        }
    }
}
