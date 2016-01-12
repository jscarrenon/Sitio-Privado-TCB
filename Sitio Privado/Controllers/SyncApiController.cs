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
        #region Graph API Parameters
        private static string ExtensionsPrefixe = ConfigurationManager.AppSettings["b2c:Extensions"];
        private static string AccountEnabledParamKey = "accountEnabled";
        private static string CreationTypeParamKey = "creationType";
        private static string PasswordPoliciesParamKey = "passwordPolicies";
        private static string GivenNameParamKey = "givenName";
        private static string SurnameParamKey = "surname";
        private static string RutParamKey = ExtensionsPrefixe + "RUT";
        private static string WorkAddressParamKey = ExtensionsPrefixe + "WorkAddress";
        private static string HomeAddressParamKey = ExtensionsPrefixe + "HomeAddress";
        private static string CountryParamKey = "country";
        private static string CityParamKey = "city";
        private static string WorkPhoneParamKey = ExtensionsPrefixe + "WorkPhoneNumber";
        private static string HomePhoneParamKey = ExtensionsPrefixe + "HomePhoneNumber";
        private static string EmailParamKey = ExtensionsPrefixe + "Email";
        private static string CheckingAccountParamKey = ExtensionsPrefixe + "CheckingAccount";
        private static string BankParamKey = ExtensionsPrefixe + "Bank";
        private static string DisplayNameParamKey = "displayName";
        private static string PasswordParamKey = "password";
        private static string ForcePasswordChangeParamKey = "forceChangePasswordNextLogin";
        private static string PasswordProfileParamKey = "passwordProfile";
        private static string SignInTypeParamKey = "type";
        private static string SignInValueParamKey = "value";
        private static string SignInAlternativesParamKey = "alternativeSignInNamesInfo";
        #endregion

        private SyncApiClientHelper syncApiHelper = new SyncApiClientHelper();

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
            json.Add(GivenNameParamKey, "Guille");
            return json.ToString();
        }

        private string GetCreateUserRequestBody(dynamic content)
        {
            JObject json = new JObject();
            //Fixed parameters
            json.Add(AccountEnabledParamKey, true);
            json.Add(CreationTypeParamKey, "NameCoexistence");
            json.Add(PasswordPoliciesParamKey, "DisablePasswordExpiration");

            //General information
            json.Add(GivenNameParamKey, content.names);
            json.Add(SurnameParamKey, content.surnames);
            json.Add(RutParamKey, content.rut);
            json.Add(WorkAddressParamKey, content.workAddress);
            json.Add(HomeAddressParamKey, content.homeAddress);
            json.Add(CountryParamKey, content.country);
            json.Add(CityParamKey, content.city);
            json.Add(WorkPhoneParamKey, content.workPhone);
            json.Add(HomePhoneParamKey, content.homePhone);
            json.Add(EmailParamKey, content.email);
            json.Add(CheckingAccountParamKey, content.checkingAccount);
            json.Add(BankParamKey, content.bank);
            json.Add(DisplayNameParamKey, content.names + " " + content.surnames);
            
            //Temporal password
            JObject passwordProfile = new JObject();
            passwordProfile.Add(PasswordParamKey, content.temporalPassword);
            passwordProfile.Add(ForcePasswordChangeParamKey, true);
            json.Add(PasswordProfileParamKey, passwordProfile);

            //Rut as login identifier
            JObject signInAlternative = new JObject();
            signInAlternative.Add(SignInTypeParamKey, "userName");
            signInAlternative.Add(SignInValueParamKey, content.rut);
            JArray signInAlternativesArray = new JArray(signInAlternative);
            json.Add(SignInAlternativesParamKey, signInAlternativesArray);

            return json.ToString();
        }
    }
}
