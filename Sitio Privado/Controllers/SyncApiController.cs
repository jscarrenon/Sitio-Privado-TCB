using Newtonsoft.Json.Linq;
using Sitio_Privado.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Sitio_Privado.Controllers
{
    //TODO: include authentication
    public class SyncApiController : ApiController
    {
        private SyncApiClientHelper syncApiHelper = new SyncApiClientHelper();
        private static string ExtensionsPrefixe = ConfigurationManager.AppSettings["b2c:Extensions"];

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
            //TODO: change response
            return await syncApiHelper.CreateUser(json);
        }

        [HttpPatch]
        public async Task<HttpResponseMessage> UpdateUser(string id, HttpRequestMessage request) {
            HttpResponseMessage getUserResponse = await syncApiHelper.GetUserByRut(id);
            dynamic userResponse = JObject.Parse(await getUserResponse.Content.ReadAsStringAsync()).GetValue("value").ElementAt(0);
            dynamic content = JObject.Parse(await request.Content.ReadAsStringAsync());
            string json = GetUpdateUserRequestBody(content);
            return await syncApiHelper.UpdateUser(userResponse.objectId.ToString(), json);
        }

        private string GetUpdateUserRequestBody(dynamic content)
        {
            JObject json = new JObject();
            json.Add("givenName", "Guille");
            return json.ToString();
        }

        private string GetCreateUserRequestBody(dynamic content)
        {
            JObject json = new JObject();
            //Fixed parameters
            json.Add("accountEnabled", true);
            json.Add("creationType", "NameCoexistence");
            json.Add("passwordPolicies", "DisablePasswordExpiration");

            //General information
            json.Add("givenName", content.names);
            json.Add("surname", content.surnames);
            json.Add(ExtensionsPrefixe + "RUT", content.rut);
            json.Add("streetAddress", content.workAddress);
            json.Add(ExtensionsPrefixe + "HomeAddress", content.homeAddress);
            json.Add("country", content.country);
            json.Add("city", content.city);
            json.Add("telephoneNumber", content.workPhone);
            json.Add("mobile", content.mobilePhone);
            json.Add(ExtensionsPrefixe + "Email", content.email);
            json.Add(ExtensionsPrefixe + "CheckingAccount", content.checkingAccount);
            json.Add(ExtensionsPrefixe + "Bank", content.bank);
            json.Add("displayName", content.names + " " + content.surnames);
            
            //Temporal password
            JObject passwordProfile = new JObject();
            passwordProfile.Add("password", content.temporalPassword);
            passwordProfile.Add("forceChangePasswordNextLogin", true);
            json.Add("passwordProfile", passwordProfile);

            //Rut as login identifier
            JObject signInAlternative = new JObject();
            signInAlternative.Add("type", "userName");
            signInAlternative.Add("value", content.rut);
            JArray signInAlternativesArray = new JArray(signInAlternative);
            json.Add("alternativeSignInNamesInfo", signInAlternativesArray);

            return json.ToString();
        }
    }
}
