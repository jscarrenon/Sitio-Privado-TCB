using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using System.Net;
using Sitio_Privado.Models;
using Newtonsoft.Json.Linq;

namespace Sitio_Privado.Helpers
{
    public class GraphApiClientHelper
    {
        #region Graph API Parameters
        //Keys used by Azure GraphAPI
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
        private static string UpdatedAtParamKey = ExtensionsPrefixe + "UpdatedAt";
        #endregion

        private const string AadGraphResourceId = "https://graph.windows.net/";
        private const string AadGraphEndpoint = "https://graph.windows.net/";
        private const string AadGraphVersion = "api-version=beta";

        private const string UsersApiPath = "/users";

        private static string Tenant = ConfigurationManager.AppSettings["b2c:Tenant"];
        private static string ClientId = ConfigurationManager.AppSettings["b2c:ClientId"];
        private static string ClientSecret = ConfigurationManager.AppSettings["b2c:ClientSecret"];

        private AuthenticationContext authContext;
        private ClientCredential credential;

        public GraphApiClientHelper() {
            // The AuthenticationContext is ADAL's primary class, in which you indicate the direcotry to use.
            this.authContext = new AuthenticationContext("https://login.microsoftonline.com/" + Tenant);

            // The ClientCredential is where you pass in your client_id and client_secret, which are 
            // provided to Azure AD in order to receive an access_token using the app's identity.
            this.credential = new ClientCredential(ClientId, ClientSecret);
        }

        public async Task<GraphApiResponseInfo> UpdateUser(string id, GraphUserModel user)
        {
            string path = UsersApiPath + "/" + id;
            string json = GetUpdateUserRequestBody(user);
            HttpResponseMessage graphResponse = await SendGraphPatchRequest(path, json);
            GraphApiResponseInfo response = new GraphApiResponseInfo();
            response.StatusCode = graphResponse.StatusCode;

            if (!graphResponse.IsSuccessStatusCode)
            {
                response.Message = "Could not find any object matching that Rut";
            }

            return response;
        }

        public async Task<GraphApiResponseInfo> CreateUser(GraphUserModel graphUser)
        {
            string json = GetCreateUserRequestBody(graphUser);
            HttpResponseMessage graphResponse = await SendGraphPostRequest(UsersApiPath, json);

            GraphApiResponseInfo response = new GraphApiResponseInfo();

            response.StatusCode = graphResponse.StatusCode;
            JObject bodyResponse = (JObject)await graphResponse.Content.ReadAsAsync(typeof(JObject));
            if (graphResponse.IsSuccessStatusCode)
            {
                response.User = GetUserResponse(bodyResponse);
            }
            else if(graphResponse.StatusCode == HttpStatusCode.BadGateway)
            {
                response.Message = bodyResponse.GetValue("message").ToString();
            }
            else
            {
                response.Message = bodyResponse.GetValue("odata.error").Value<JToken>("message").Value<string>("value").Replace("alternativeSignInNamesInfo", "rut");
            }
            return response;
        }

        public async Task<GraphApiResponseInfo> GetUserByRut(string rut)
        {
            string query = "$filter=" + RutParamKey + " eq '" + rut + "'";
            HttpResponseMessage graphResponse = await SendGraphGetRequest(UsersApiPath, query);

            //Set response
            GraphApiResponseInfo response = new GraphApiResponseInfo();
            response.StatusCode = graphResponse.StatusCode;
            JObject bodyResponse = (JObject)await graphResponse.Content.ReadAsAsync(typeof(JObject));

            //TODO: update codes
            if (graphResponse.IsSuccessStatusCode)
            {
                JArray graphApiResponseUsers = (JArray)bodyResponse.GetValue("value");
                if (graphApiResponseUsers.Count > 0)
                {
                    GraphUserModel user = GetUserResponse((JObject)graphApiResponseUsers.First);
                    response.User = user;
                }
                else
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Message = "Could not find any object matching that Rut";
                }
            }
            else if (graphResponse.StatusCode == HttpStatusCode.BadGateway)
            {
                response.Message = bodyResponse.GetValue("message").ToString();
            }
            else
            {
                response.Message = bodyResponse.GetValue("odata.error").Value<JToken>("message").Value<string>("value");
            }

            return response;
        }

        public async Task<GraphApiResponseInfo> GetUserByObjectId(string id)
        {
            HttpResponseMessage graphResponse = await SendGraphGetRequest(UsersApiPath + "/" + id, null);
            
            //Set response
            GraphApiResponseInfo response = new GraphApiResponseInfo();
            response.StatusCode = graphResponse.StatusCode;
            JObject bodyResponse = (JObject)await graphResponse.Content.ReadAsAsync(typeof(JObject));

            //TODO: update codes
            if (graphResponse.IsSuccessStatusCode)
            {
                GraphUserModel user = GetUserResponse(bodyResponse);
                response.User = user;
            }
            else if (graphResponse.StatusCode == HttpStatusCode.BadGateway)
            {
                response.Message = bodyResponse.GetValue("message").ToString();
            }
            else
            {
                response.Message = bodyResponse.GetValue("odata.error").Value<JToken>("message").Value<string>("value");
            }

            return response;
        }

        private async Task<HttpResponseMessage> SendGraphPostRequest(string api, string json)
        {
            // NOTE: This client uses ADAL v2, not ADAL v4
            AuthenticationResult result = authContext.AcquireToken(AadGraphResourceId, credential);
            HttpClient http = new HttpClient();
            string url = AadGraphEndpoint + Tenant + api + "?" + AadGraphVersion;

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage graphApiResponse;
            try
            {
                graphApiResponse = await http.SendAsync(request);
            }
            catch(Exception)
            {
                graphApiResponse = new HttpResponseMessage();
                graphApiResponse.StatusCode = HttpStatusCode.BadGateway;
                JObject content = new JObject();
                content.Add("message", "There was an unexpected error when performing the operation in B2C server");
                graphApiResponse.Content = new StringContent(content.ToString(), Encoding.UTF8, "application/json");
            }
            return graphApiResponse;
        }

        private async Task<HttpResponseMessage> SendGraphGetRequest(string api, string query)
        {
            // First, use ADAL to acquire a token using the app's identity (the credential)
            // The first parameter is the resource we want an access_token for; in this case, the Graph API.
            AuthenticationResult result = authContext.AcquireToken("https://graph.windows.net", credential);

            // For B2C user managment, be sure to use the beta Graph API version.
            HttpClient http = new HttpClient();
            string url = "https://graph.windows.net/" + Tenant + api + "?" + "api-version=beta";
            if (!string.IsNullOrEmpty(query))
            {
                url += "&" + query;
            }

            // Append the access token for the Graph API to the Authorization header of the request, using the Bearer scheme.
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);
            HttpResponseMessage graphApiResponse;

            try
            {
                graphApiResponse = await http.SendAsync(request);
            }
            catch (Exception)
            {
                graphApiResponse = new HttpResponseMessage();
                graphApiResponse = new HttpResponseMessage();
                graphApiResponse.StatusCode = HttpStatusCode.BadGateway;
                JObject content = new JObject();
                content.Add("message", "There was an unexpected error when performing the operation in B2C server");
            }

            return graphApiResponse;
        }

        private async Task<HttpResponseMessage> SendGraphPatchRequest(string api, string json)
        {
            // NOTE: This client uses ADAL v2, not ADAL v4
            AuthenticationResult result = authContext.AcquireToken(AadGraphResourceId, credential);
            HttpClient http = new HttpClient();
            string url = AadGraphEndpoint + Tenant + api + "?" + AadGraphVersion;

            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("PATCH"), url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage graphApiResponse;

            try
            {
                graphApiResponse = await http.SendAsync(request);
            }
            catch (Exception)
            {
                graphApiResponse = new HttpResponseMessage();
                graphApiResponse = new HttpResponseMessage();
                graphApiResponse.StatusCode = HttpStatusCode.BadGateway;
                JObject content = new JObject();
                content.Add("message", "There was an unexpected error when performing the operation in B2C server");
            }

            return graphApiResponse;
        }

        private string GetCreateUserRequestBody(GraphUserModel graphUser)
        {
            JObject json = new JObject();
            //Fixed parameters
            json.Add(AccountEnabledParamKey, true);
            json.Add(CreationTypeParamKey, "NameCoexistence");
            json.Add(PasswordPoliciesParamKey, "DisablePasswordExpiration");

            //General information
            json.Add(GivenNameParamKey, graphUser.Name);
            json.Add(EmailParamKey, graphUser.Email);
            json.Add(RutParamKey, graphUser.Rut);
            if (graphUser.Surname != null && graphUser.Surname != "") json.Add(SurnameParamKey, graphUser.Surname);
            if (graphUser.WorkAddress != null && graphUser.WorkAddress != "") json.Add(WorkAddressParamKey, graphUser.WorkAddress);
            if (graphUser.HomeAddress != null && graphUser.HomeAddress != "") json.Add(HomeAddressParamKey, graphUser.HomeAddress);
            if (graphUser.Country != null && graphUser.Country != "") json.Add(CountryParamKey, graphUser.Country);
            if (graphUser.City != null && graphUser.City != "") json.Add(CityParamKey, graphUser.City);
            if (graphUser.WorkPhone != null && graphUser.WorkPhone != "") json.Add(WorkPhoneParamKey, graphUser.WorkPhone);
            if (graphUser.HomePhone != null && graphUser.HomePhone != "") json.Add(HomePhoneParamKey, graphUser.HomePhone);
            if (graphUser.CheckingAccount != null && graphUser.CheckingAccount != "") json.Add(CheckingAccountParamKey, graphUser.CheckingAccount);
            if (graphUser.Bank != null && graphUser.Bank != "") json.Add(BankParamKey, graphUser.Bank);
            json.Add(DisplayNameParamKey, graphUser.DisplayName);
            json.Add(UpdatedAtParamKey, graphUser.UpdatedAt);

            //Temporal password
            JObject passwordProfile = new JObject();
            passwordProfile.Add(PasswordParamKey, graphUser.TemporalPassword);
            passwordProfile.Add(ForcePasswordChangeParamKey, true);
            json.Add(PasswordProfileParamKey, passwordProfile);

            //Rut as login identifier and email as support
            JObject signInAlternative = new JObject();
            signInAlternative.Add(SignInTypeParamKey, "userName");
            signInAlternative.Add(SignInValueParamKey, graphUser.Rut);

            JArray signInAlternativesArray = new JArray();
            signInAlternativesArray.Add(signInAlternative);

            if(graphUser.Email != null && graphUser.Email != "")
            {
                JObject signInAlternativeEmail = new JObject();
                signInAlternativeEmail.Add(SignInTypeParamKey, "emailAddress");
                signInAlternativeEmail.Add(SignInValueParamKey, graphUser.Email);
                signInAlternativesArray.Add(signInAlternativeEmail);
            }

            json.Add(SignInAlternativesParamKey, signInAlternativesArray);
            

            return json.ToString();
        }

        private string GetUpdateUserRequestBody(GraphUserModel graphUser)
        {
            JObject json = new JObject();

            if (graphUser.Name != null && graphUser.Name != "")
                json.Add(GivenNameParamKey, graphUser.Name);

            if (graphUser.Surname != null && graphUser.Surname != "")
                json.Add(SurnameParamKey, graphUser.Surname);

            if (graphUser.WorkAddress != null && graphUser.WorkAddress != "")
                json.Add(WorkAddressParamKey, graphUser.WorkAddress);

            if (graphUser.HomeAddress != null && graphUser.HomeAddress != "")
                json.Add(HomeAddressParamKey, graphUser.HomeAddress);

            if (graphUser.Country != null && graphUser.Country != "")
                json.Add(CountryParamKey, graphUser.Country);

            if (graphUser.City != null && graphUser.City != "")
                json.Add(CityParamKey, graphUser.City);

            if (graphUser.WorkPhone != null && graphUser.WorkPhone != "")
                json.Add(WorkPhoneParamKey, graphUser.WorkPhone);

            if (graphUser.HomePhone != null && graphUser.HomePhone != "")
                json.Add(HomePhoneParamKey, graphUser.HomePhone);

            if (graphUser.Email != null && graphUser.Email != "")
                json.Add(EmailParamKey, graphUser.Email);

            if (graphUser.CheckingAccount != null && graphUser.CheckingAccount != "")
                json.Add(CheckingAccountParamKey, graphUser.CheckingAccount);

            if (graphUser.Bank != null && graphUser.Bank != "")
                json.Add(BankParamKey, graphUser.Bank);

            //If email is updated, then set sign-in options again.
            if (graphUser.Email != null && graphUser.Email != "")
            {
                JArray signInAlternativesArray = new JArray();

                JObject signInAlternative = new JObject();
                signInAlternative.Add(SignInTypeParamKey, "userName");
                signInAlternative.Add(SignInValueParamKey, graphUser.Rut);
                signInAlternativesArray.Add(signInAlternative);

                JObject signInAlternativeEmail = new JObject();
                signInAlternativeEmail.Add(SignInTypeParamKey, "emailAddress");
                signInAlternativeEmail.Add(SignInValueParamKey, graphUser.Email);
                signInAlternativesArray.Add(signInAlternativeEmail);

                json.Add(SignInAlternativesParamKey, signInAlternativesArray);
            }

            json.Add(UpdatedAtParamKey, graphUser.UpdatedAt);

            return json.ToString();
        }

        private GraphUserModel GetUserResponse(JObject body)
        {
            GraphUserModel user = new GraphUserModel();
            user.Name = body.GetValue(GivenNameParamKey).ToString();
            user.Surname = body.GetValue(SurnameParamKey).ToString();
            user.Rut = body.GetValue(RutParamKey).ToString();

            if (body.GetValue(EmailParamKey) != null)
                user.Email = body.GetValue(EmailParamKey).ToString();

            if(body.GetValue(CountryParamKey) != null)
                user.Country = body.GetValue(CountryParamKey).ToString();

            if(body.GetValue(CityParamKey) != null)
                user.City = body.GetValue(CityParamKey).ToString();

            if(body.GetValue(BankParamKey) != null)
                user.Bank = body.GetValue(BankParamKey).ToString();

            if(body.GetValue(CheckingAccountParamKey) != null)
                user.CheckingAccount = body.GetValue(CheckingAccountParamKey).ToString();

            if(body.GetValue(HomeAddressParamKey) != null)
                user.HomeAddress = body.GetValue(HomeAddressParamKey).ToString();

            if(body.GetValue(HomePhoneParamKey) != null)
                user.HomePhone = body.GetValue(HomePhoneParamKey).ToString();

            if(body.GetValue(WorkAddressParamKey) != null)
                user.WorkAddress = body.GetValue(WorkAddressParamKey).ToString();

            if(body.GetValue(WorkPhoneParamKey) != null)
                user.WorkPhone = body.GetValue(WorkPhoneParamKey).ToString();

            if (body.GetValue(UpdatedAtParamKey) != null)
                user.UpdatedAt = body.GetValue(UpdatedAtParamKey).ToString();

            user.ObjectId = body.GetValue("objectId").ToString();

            return user;
        }
    }
}