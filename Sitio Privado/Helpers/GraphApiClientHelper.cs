﻿using System;
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

namespace Sitio_Privado.Helpers
{
    public class GraphApiClientHelper
    {
        #region Graph API Parameters
        //Keys used by Azure GraphAPI
        private static string ExtensionsPrefixe = ConfigurationManager.AppSettings["b2c:Extensions"];
        public static string AccountEnabledParamKey = "accountEnabled";
        public static string CreationTypeParamKey = "creationType";
        public static string PasswordPoliciesParamKey = "passwordPolicies";
        public static string GivenNameParamKey = "givenName";
        public static string SurnameParamKey = "surname";
        public static string RutParamKey = ExtensionsPrefixe + "RUT";
        public static string WorkAddressParamKey = ExtensionsPrefixe + "WorkAddress";
        public static string HomeAddressParamKey = ExtensionsPrefixe + "HomeAddress";
        public static string CountryParamKey = "country";
        public static string CityParamKey = "city";
        public static string WorkPhoneParamKey = ExtensionsPrefixe + "WorkPhoneNumber";
        public static string HomePhoneParamKey = ExtensionsPrefixe + "HomePhoneNumber";
        public static string EmailParamKey = ExtensionsPrefixe + "Email";
        public static string CheckingAccountParamKey = ExtensionsPrefixe + "CheckingAccount";
        public static string BankParamKey = ExtensionsPrefixe + "Bank";
        public static string DisplayNameParamKey = "displayName";
        public static string PasswordParamKey = "password";
        public static string ForcePasswordChangeParamKey = "forceChangePasswordNextLogin";
        public static string PasswordProfileParamKey = "passwordProfile";
        public static string SignInTypeParamKey = "type";
        public static string SignInValueParamKey = "value";
        public static string SignInAlternativesParamKey = "alternativeSignInNamesInfo";
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

        public async Task<HttpResponseMessage> UpdateUser(string id, string json)
        {
            return await SendGraphPatchRequest(UsersApiPath + "/" + id, json);
        }

        public async Task<HttpResponseMessage> CreateUser(string json)
        {
            return await SendGraphPostRequest(UsersApiPath, json);
        }

        public async Task<HttpResponseMessage> GetUserByRut(string rut)
        {
            string query = "$filter=" + RutParamKey + " eq '" + rut + "'";
            return await SendGraphGetRequest(UsersApiPath, query);
        }

        public async Task<HttpResponseMessage> GetUserByObjectId(string id)
        {
            return await SendGraphGetRequest(UsersApiPath + "/" + id, null);
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
            HttpResponseMessage response = await http.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                string error = await response.Content.ReadAsStringAsync();
                object formatted = JsonConvert.DeserializeObject(error);
                throw new WebException("Error Calling the Graph API: \n" + JsonConvert.SerializeObject(formatted, Formatting.Indented));
            }
        
            return response;
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
            HttpResponseMessage response = await http.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                string error = await response.Content.ReadAsStringAsync();
                object formatted = JsonConvert.DeserializeObject(error);
                throw new WebException("Error Calling the Graph API: \n" + JsonConvert.SerializeObject(formatted, Formatting.Indented));
            }

            return response;
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
            HttpResponseMessage response = await http.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                string error = await response.Content.ReadAsStringAsync();
                object formatted = JsonConvert.DeserializeObject(error);
                throw new WebException("Error Calling the Graph API: \n" + JsonConvert.SerializeObject(formatted, Formatting.Indented));
            }

            return response;
        }
    }
}