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

        #region SyncAPIController Parameters
        private static string NameParam = "names";
        private static string SurnameParam = "surnames";
        private static string RutParam = "rut";
        private static string WorkAddressParam = "workAddress";
        private static string HomeAddressParam = "homeAddress";
        private static string CountryParam = "country";
        private static string CityParam = "city";
        private static string WorkPhoneParam = "workPhone";
        private static string HomePhoneParam = "homePhone";
        private static string EmailParam = "email";
        private static string CheckingAccountParam = "checkingAccount";
        private static string BankParam = "bank";
        private static string TemporalPasswordParam = "temporalPassword";
        #endregion

        private SyncApiClientHelper syncApiHelper = new SyncApiClientHelper();

        [HttpGet]
        public async Task<HttpResponseMessage> GetUsers()
        {
            return await syncApiHelper.GetAllUsers(null);
        }

        [HttpPost]
        public async Task<HttpResponseMessage> CreateUser(HttpRequestMessage request)
        {
            JObject content = JObject.Parse(await request.Content.ReadAsStringAsync());
            string json = GetCreateUserRequestBody(content);
            //TODO: Change response
            return await syncApiHelper.CreateUser(json);
        }

        [HttpPatch]
        public async Task<HttpResponseMessage> UpdateUser(string id, HttpRequestMessage request)
        {
            HttpResponseMessage getUserResponse = await syncApiHelper.GetUserByRut(id);
            dynamic userResponse = JObject.Parse(await getUserResponse.Content.ReadAsStringAsync()).GetValue("value").ElementAt(0);
            JObject content = JObject.Parse(await request.Content.ReadAsStringAsync());
            string json = GetUpdateUserRequestBody(content);
            //TODO: Change response
            return await syncApiHelper.UpdateUser(userResponse.objectId.ToString(), json);
        }

        private string GetUpdateUserRequestBody(JObject content)
        {
            //TODO: update
            JObject json = new JObject();

            if (content.GetValue(WorkAddressParam) != null)
                json.Add(WorkAddressParamKey, content.GetValue(WorkAddressParam));
            
            if (content.GetValue(HomeAddressParam) != null)
                json.Add(HomeAddressParamKey, content.GetValue(HomeAddressParam));
            
            if (content.GetValue(CountryParam) != null)
                json.Add(CountryParamKey, content.GetValue(CountryParam));
            
            if (content.GetValue(CityParam) != null)
                json.Add(CityParamKey, content.GetValue(CityParam));
            
            if (content.GetValue(WorkPhoneParam) != null)
                json.Add(WorkPhoneParamKey, content.GetValue(WorkPhoneParam));

            if (content.GetValue(HomePhoneParam) != null)
                json.Add(HomePhoneParamKey, content.GetValue(HomePhoneParam));
            
            if (content.GetValue(EmailParam) != null)
                json.Add(EmailParamKey, content.GetValue(EmailParam));

            if (content.GetValue(CheckingAccountParam) != null)
                json.Add(CheckingAccountParamKey, content.GetValue(CheckingAccountParam));
            
            if (content.GetValue(BankParam) != null)
                json.Add(BankParamKey, content.GetValue(BankParam));

            /*if (content.GetValue(TemporalPasswordParam) != null)
            {
                //Temporal password
                JObject passwordProfile = new JObject();
                passwordProfile.Add(PasswordParamKey, content.GetValue(TemporalPasswordParam));
                passwordProfile.Add(ForcePasswordChangeParamKey, true);
                json.Add(PasswordProfileParamKey, passwordProfile);
            }*/

            return json.ToString();
        }

        private string GetCreateUserRequestBody(JObject content)
        {
            JObject json = new JObject();
            //Fixed parameters
            json.Add(AccountEnabledParamKey, true);
            json.Add(CreationTypeParamKey, "NameCoexistence");
            json.Add(PasswordPoliciesParamKey, "DisablePasswordExpiration");

            //General information
            json.Add(GivenNameParamKey, content.GetValue(NameParam));
            json.Add(SurnameParamKey, content.GetValue(SurnameParam));
            json.Add(RutParamKey, content.GetValue(RutParam));
            json.Add(WorkAddressParamKey, content.GetValue(WorkAddressParam));
            json.Add(HomeAddressParamKey, content.GetValue(HomeAddressParam));
            json.Add(CountryParamKey, content.GetValue(CountryParam));
            json.Add(CityParamKey, content.GetValue(CityParam));
            json.Add(WorkPhoneParamKey, content.GetValue(WorkPhoneParam));
            json.Add(HomePhoneParamKey, content.GetValue(HomePhoneParam));
            json.Add(EmailParamKey, content.GetValue(EmailParam));
            json.Add(CheckingAccountParamKey, content.GetValue(CheckingAccountParam));
            json.Add(BankParamKey, content.GetValue(BankParam));
            json.Add(DisplayNameParamKey, content.GetValue(NameParam).ToString() + " " + 
                content.GetValue(SurnameParam).ToString());
            
            //Temporal password
            JObject passwordProfile = new JObject();
            passwordProfile.Add(PasswordParamKey, content.GetValue(TemporalPasswordParam));
            passwordProfile.Add(ForcePasswordChangeParamKey, true);
            json.Add(PasswordProfileParamKey, passwordProfile);

            //Rut as login identifier
            JObject signInAlternative = new JObject();
            signInAlternative.Add(SignInTypeParamKey, "userName");
            signInAlternative.Add(SignInValueParamKey, content.GetValue(RutParam));
            JArray signInAlternativesArray = new JArray(signInAlternative);
            json.Add(SignInAlternativesParamKey, signInAlternativesArray);

            return json.ToString();
        }
    }
}
