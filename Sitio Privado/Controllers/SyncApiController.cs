using Newtonsoft.Json.Linq;
using Sitio_Privado.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Sitio_Privado.Controllers
{
    //TODO: include authentication
    public class SyncApiController : ApiController
    {
        #region SyncAPIController Parameters
        //Keys used by our API
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

        private GraphApiClientHelper syncApiHelper = new GraphApiClientHelper();

        [HttpPost]
        public async Task<HttpResponseMessage> CreateUser(HttpRequestMessage request)
        {
            JObject requestBody = JObject.Parse(await request.Content.ReadAsStringAsync());
            string graphApiRequestBody = GetCreateUserGraphApiRequestBody(requestBody);
            HttpResponseMessage graphApiResponse = await syncApiHelper.CreateUser(graphApiRequestBody);
            JObject graphApiResponseContent = (JObject)await graphApiResponse.Content.ReadAsAsync(typeof(JObject));
            string responseBody = GetUserResponseBody(graphApiResponseContent);
            HttpResponseMessage response = new HttpResponseMessage(graphApiResponse.StatusCode);
            response.Content = new StringContent(responseBody, Encoding.UTF8, "application/json");
            return response;
        }

        [HttpPatch]
        public async Task<HttpResponseMessage> UpdateUser(string id, HttpRequestMessage request)
        {
            HttpResponseMessage userGraphApiResponse = await syncApiHelper.GetUserByRut(id);
            JObject userGraphApiResponseContent = (JObject)await userGraphApiResponse.Content.ReadAsAsync(typeof(JObject));
            string userGraphId = userGraphApiResponseContent.GetValue("value").ElementAt(0).Value<string>("objectId");
            JObject requestContent = (JObject)await request.Content.ReadAsAsync(typeof(JObject));
            string requestJsonBody = GetUpdateUserGraphApiRequestBody(requestContent);
            HttpResponseMessage updateUserGraphApiResponse = await syncApiHelper.UpdateUser(userGraphId, requestJsonBody);
            HttpResponseMessage response = new HttpResponseMessage(updateUserGraphApiResponse.StatusCode);
            return response;
        }

        [HttpGet]
        public async Task<HttpResponseMessage> GetUser(string id) {
            HttpResponseMessage graphApiResponse = await syncApiHelper.GetUserByRut(id);
            JObject graphApiResponseContent = (JObject)await graphApiResponse.Content.ReadAsAsync(typeof(JObject));
            string responseBody = GetUserResponseBody((JObject)graphApiResponseContent.GetValue("value").ElementAt(0));
            HttpResponseMessage response = new HttpResponseMessage(graphApiResponse.StatusCode);
            response.Content = new StringContent(responseBody.ToString(), Encoding.UTF8,"application/json");
            return response;
        }

        private string GetUserResponseBody(JObject content)
        {
            JObject response = new JObject();
            response.Add(NameParam, content.Value<string>(GraphApiClientHelper.GivenNameParamKey));
            response.Add(SurnameParam, content.Value<string>(GraphApiClientHelper.SurnameParamKey));
            response.Add(RutParam, content.Value<string>(GraphApiClientHelper.RutParamKey));
            response.Add(WorkAddressParam, content.Value<string>(GraphApiClientHelper.WorkAddressParamKey));
            response.Add(HomeAddressParam, content.Value<string>(GraphApiClientHelper.HomeAddressParamKey));
            response.Add(CountryParam, content.Value<string>(GraphApiClientHelper.CountryParamKey));
            response.Add(CityParam, content.Value<string>(GraphApiClientHelper.CityParamKey));
            response.Add(WorkPhoneParam, content.Value<string>(GraphApiClientHelper.WorkPhoneParamKey));
            response.Add(HomePhoneParam, content.Value<string>(GraphApiClientHelper.HomePhoneParamKey));
            response.Add(EmailParam, content.Value<string>(GraphApiClientHelper.EmailParamKey));
            response.Add(CheckingAccountParam, content.Value<string>(GraphApiClientHelper.CheckingAccountParamKey));
            response.Add(BankParam, content.Value<string>(GraphApiClientHelper.BankParamKey));
            return response.ToString();
        }

        private string GetUpdateUserGraphApiRequestBody(JObject content)
        {
            //TODO: update
            JObject json = new JObject();

            if (content.GetValue(WorkAddressParam) != null)
                json.Add(GraphApiClientHelper.WorkAddressParamKey, content.GetValue(WorkAddressParam));
            
            if (content.GetValue(HomeAddressParam) != null)
                json.Add(GraphApiClientHelper.HomeAddressParamKey, content.GetValue(HomeAddressParam));
            
            if (content.GetValue(CountryParam) != null)
                json.Add(GraphApiClientHelper.CountryParamKey, content.GetValue(CountryParam));
            
            if (content.GetValue(CityParam) != null)
                json.Add(GraphApiClientHelper.CityParamKey, content.GetValue(CityParam));
            
            if (content.GetValue(WorkPhoneParam) != null)
                json.Add(GraphApiClientHelper.WorkPhoneParamKey, content.GetValue(WorkPhoneParam));

            if (content.GetValue(HomePhoneParam) != null)
                json.Add(GraphApiClientHelper.HomePhoneParamKey, content.GetValue(HomePhoneParam));
            
            if (content.GetValue(EmailParam) != null)
                json.Add(GraphApiClientHelper.EmailParamKey, content.GetValue(EmailParam));

            if (content.GetValue(CheckingAccountParam) != null)
                json.Add(GraphApiClientHelper.CheckingAccountParamKey, content.GetValue(CheckingAccountParam));
            
            if (content.GetValue(BankParam) != null)
                json.Add(GraphApiClientHelper.BankParamKey, content.GetValue(BankParam));

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

        private string GetCreateUserGraphApiRequestBody(JObject content)
        {
            JObject json = new JObject();
            //Fixed parameters
            json.Add(GraphApiClientHelper.AccountEnabledParamKey, true);
            json.Add(GraphApiClientHelper.CreationTypeParamKey, "NameCoexistence");
            json.Add(GraphApiClientHelper.PasswordPoliciesParamKey, "DisablePasswordExpiration");

            //General information
            json.Add(GraphApiClientHelper.GivenNameParamKey, content.GetValue(NameParam));
            json.Add(GraphApiClientHelper.SurnameParamKey, content.GetValue(SurnameParam));
            json.Add(GraphApiClientHelper.RutParamKey, content.GetValue(RutParam));
            json.Add(GraphApiClientHelper.WorkAddressParamKey, content.GetValue(WorkAddressParam));
            json.Add(GraphApiClientHelper.HomeAddressParamKey, content.GetValue(HomeAddressParam));
            json.Add(GraphApiClientHelper.CountryParamKey, content.GetValue(CountryParam));
            json.Add(GraphApiClientHelper.CityParamKey, content.GetValue(CityParam));
            json.Add(GraphApiClientHelper.WorkPhoneParamKey, content.GetValue(WorkPhoneParam));
            json.Add(GraphApiClientHelper.HomePhoneParamKey, content.GetValue(HomePhoneParam));
            json.Add(GraphApiClientHelper.EmailParamKey, content.GetValue(EmailParam));
            json.Add(GraphApiClientHelper.CheckingAccountParamKey, content.GetValue(CheckingAccountParam));
            json.Add(GraphApiClientHelper.BankParamKey, content.GetValue(BankParam));
            json.Add(GraphApiClientHelper.DisplayNameParamKey, content.GetValue(NameParam).ToString() + " " + 
                content.GetValue(SurnameParam).ToString());
            
            //Temporal password
            JObject passwordProfile = new JObject();
            passwordProfile.Add(GraphApiClientHelper.PasswordParamKey, content.GetValue(TemporalPasswordParam));
            passwordProfile.Add(GraphApiClientHelper.ForcePasswordChangeParamKey, true);
            json.Add(GraphApiClientHelper.PasswordProfileParamKey, passwordProfile);

            //Rut as login identifier
            JObject signInAlternative = new JObject();
            signInAlternative.Add(GraphApiClientHelper.SignInTypeParamKey, "userName");
            signInAlternative.Add(GraphApiClientHelper.SignInValueParamKey, content.GetValue(RutParam));
            JArray signInAlternativesArray = new JArray(signInAlternative);
            json.Add(GraphApiClientHelper.SignInAlternativesParamKey, signInAlternativesArray);

            return json.ToString();
        }
    }
}
