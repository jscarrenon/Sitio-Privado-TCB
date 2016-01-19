using Newtonsoft.Json.Linq;
using Sitio_Privado.Helpers;
using Sitio_Privado.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
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
        private static string WorkAddressParam = "work_address";
        private static string HomeAddressParam = "home_address";
        private static string CountryParam = "country";
        private static string CityParam = "city";
        private static string WorkPhoneParam = "work_phone";
        private static string HomePhoneParam = "home_phone";
        private static string EmailParam = "email";
        private static string CheckingAccountParam = "checking_account";
        private static string BankParam = "bank";
        private static string TemporalPasswordParam = "temporal_password";
        #endregion

        private GraphApiClientHelper syncApiHelper = new GraphApiClientHelper();

        [HttpPost]
        public async Task<HttpResponseMessage> CreateUser(HttpRequestMessage request)
        {
            //Response
            HttpResponseMessage response = new HttpResponseMessage();

            //Read request's parameters
            JObject requestBody = JObject.Parse(await request.Content.ReadAsStringAsync());
            if (!CheckNeededAttributesForCreatingUser(requestBody))
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                JObject json = new JObject();
                json.Add("message", "One or more required parameters is missing");
                response.Content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
                return response;
            }

            //Create user
            GraphUserModel graphUser = GetGraphUserCreateRequest(requestBody);
            GraphApiResponseInfo graphApiResponse = await syncApiHelper.CreateUser(graphUser);
            
            //Read result and set response
            response.StatusCode = graphApiResponse.StatusCode;
            if (graphApiResponse.StatusCode == HttpStatusCode.Created)
            {
                string responseBody = GetUserResponseBody(graphApiResponse.User);
                response.Content = new StringContent(responseBody, Encoding.UTF8, "application/json");
            }
            else
            {
                JObject errorMessage = new JObject();
                //TODO: filter error messages
                errorMessage.Add("message", graphApiResponse.Message);
                response.Content = new StringContent(errorMessage.ToString(), Encoding.UTF8, "application/json");
            }
            return response;
        }

        [HttpPatch]
        public async Task<HttpResponseMessage> UpdateUser(string id, HttpRequestMessage request)
        {
            HttpResponseMessage response = new HttpResponseMessage();

            //Read request's parameters
            JObject requestContent = (JObject)await request.Content.ReadAsAsync(typeof(JObject));
            string requestJsonBody = GetUpdateUserGraphApiRequestBody(requestContent);
            if (requestJsonBody == null) {
                response.StatusCode = HttpStatusCode.BadRequest;
                return response;
            }

            //Get user
            GraphApiResponseInfo getGraphResponse = await syncApiHelper.GetUserByRut(id);

            if (getGraphResponse.User == null)
            {
                response.StatusCode = HttpStatusCode.NotFound;
                JObject errorMessage = new JObject();
                //TODO: filter error messages
                errorMessage.Add("message", getGraphResponse.Message);
                response.Content = new StringContent(errorMessage.ToString(), Encoding.UTF8, "application/json");
                return response;
            }

            string userGraphId = getGraphResponse.User.ObjectId;
            GraphApiResponseInfo graphResponse = await syncApiHelper.UpdateUser(userGraphId, requestJsonBody);
            response.StatusCode = graphResponse.StatusCode;

            if (graphResponse.StatusCode != HttpStatusCode.NoContent)
            {
                JObject errorMessage = new JObject();
                //TODO: filter error messages
                errorMessage.Add("message", graphResponse.Message);
                response.Content = new StringContent(errorMessage.ToString(), Encoding.UTF8, "application/json");
            }
            return response;
        }
    
        [HttpGet]
        public async Task<HttpResponseMessage> GetUser(string id)
        {
            HttpResponseMessage response = new HttpResponseMessage();

            GraphApiResponseInfo graphApiResponse = await syncApiHelper.GetUserByRut(id);
            response.StatusCode = graphApiResponse.StatusCode;
            if(response.StatusCode == HttpStatusCode.OK)
            {
                string responseBody = GetUserResponseBody(graphApiResponse.User);
                response.Content = new StringContent(responseBody, Encoding.UTF8, "application/json");
            }
            else
            {
                JObject errorMessage = new JObject();
                //TODO: filter error messages
                errorMessage.Add("message", graphApiResponse.Message);
                response.Content = new StringContent(errorMessage.ToString(), Encoding.UTF8, "application/json");
            }
            
            return response;
        }

        private string GetUserResponseBody(GraphUserModel user)
        {
            JObject response = new JObject();
            response.Add(NameParam, user.Name);
            response.Add(SurnameParam, user.Surname);
            response.Add(RutParam, user.Rut);
            response.Add(WorkAddressParam, user.WorkAddress);
            response.Add(HomeAddressParam, user.HomeAddress);
            response.Add(CountryParam, user.Country);
            response.Add(CityParam, user.City);
            response.Add(WorkPhoneParam, user.WorkPhone);
            response.Add(HomePhoneParam, user.HomePhone);
            response.Add(EmailParam, user.Email);
            response.Add(CheckingAccountParam, user.CheckingAccount);
            response.Add(BankParam, user.Bank);
            return response.ToString();
        }

        private string GetUpdateUserGraphApiRequestBody(JObject content)
        {
            if (content == null)
                return null;

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

            return json.ToString();
        }

        private GraphUserModel GetGraphUserCreateRequest(JObject content)
        {
            GraphUserModel user = new GraphUserModel();
            user.Name = content.GetValue(NameParam).ToString();
            user.Surname = content.GetValue(SurnameParam).ToString();
            user.Rut = content.GetValue(RutParam).ToString();
            user.WorkAddress = content.GetValue(WorkAddressParam).ToString();
            user.HomeAddress = content.GetValue(HomeAddressParam).ToString();
            user.Country = content.GetValue(CountryParam).ToString();
            user.City = content.GetValue(CityParam).ToString();
            user.WorkPhone = content.GetValue(WorkPhoneParam).ToString();
            user.HomePhone = content.GetValue(HomePhoneParam).ToString();
            user.Email = content.GetValue(EmailParam).ToString();
            user.CheckingAccount = content.GetValue(CheckingAccountParam).ToString();
            user.Bank = content.GetValue(BankParam).ToString();
            user.TemporalPassword = content.GetValue(TemporalPasswordParam).ToString();
            return user;
        }

        private bool CheckNeededAttributesForCreatingUser(JObject requestBody)
        {
            if (requestBody.GetValue(NameParam) == null || requestBody.GetValue(SurnameParam) == null ||
                requestBody.GetValue(RutParam) == null || requestBody.GetValue(TemporalPasswordParam) == null)
                return false;
            return true;
        }
    }
}
