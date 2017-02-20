using HtmlAgilityPack;
using Newtonsoft.Json.Linq;
using ScrapySharp.Extensions;
using Sitio_Privado.Extras;
using Sitio_Privado.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.ApplicationInsights;

namespace Sitio_Privado.Helpers
{
    public class SignInHelper
    {
        private const string countryClaimKey = "country";
        private const string cityClaimKey = "city";

        GraphApiClientHelper graphApiClient = new GraphApiClientHelper();

        TelemetryClient telemetry = new TelemetryClient();

        public async Task<string> GetToken(LoginModel model)
        {
            var success = false;
            var startTime = DateTime.UtcNow;
            var timer = System.Diagnostics.Stopwatch.StartNew();

            try
            {

                //Ask B2C login page
                Uri azureLoginPageUri = GetMicrosoftLoginUri();
                HttpWebRequest azureLoginPageRequest = HttpWebRequest.CreateHttp(azureLoginPageUri);
                HttpWebResponse azureLoginPageResponse = await InitialLoginPageRequest(azureLoginPageRequest);

                //Save cookies for later usage
                CookieCollection azureLoginPageCookies = azureLoginPageRequest.CookieContainer.GetCookies(azureLoginPageUri);
                azureLoginPageCookies.Add(azureLoginPageResponse.Cookies);

                //Scrap the document for inserting the data
                HtmlNode html = GetScrappedDoc(azureLoginPageResponse.GetResponseStream());
                HttpRequestMessage tokenRequest = GetTokenRequestMessage(html, model);

                //Prepare data for login request
                var baseAddress = new Uri("https://login.microsoftonline.com");
                CookieContainer cookieContainer = new CookieContainer();
                cookieContainer.Add(azureLoginPageCookies);

                using (var handler = new WebRequestHandler() { CookieContainer = cookieContainer })
                {
                    handler.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => { return true; };
                    using (var client = new HttpClient(handler) { BaseAddress = baseAddress })
                    {
                        //Send request
                        var result = await client.SendAsync(tokenRequest);

                        //Confirmation
                        if (result.StatusCode == HttpStatusCode.OK)
                        {
                            //Ask B2C login confirmed page
                            Uri azureLoginConfirmedPageUri = GetMicrosoftLoginConfirmedUri(tokenRequest);
                            HttpWebRequest azureLoginConfirmedPageRequest = HttpWebRequest.CreateHttp(azureLoginConfirmedPageUri);
                            HttpWebResponse azureLoginConfirmedPageResponse = await LoginConfirmedPageRequest(azureLoginConfirmedPageRequest, cookieContainer);

                            //Read id_token
                            HtmlNode htmlNode = GetScrappedDoc(azureLoginConfirmedPageResponse.GetResponseStream());
                            if (htmlNode.CssSelect("input[name=id_token]").Count() > 0)
                            {
                                return htmlNode.CssSelect("input[name=id_token]").First().GetAttributeValue("value");
                            }
                            else
                            {
                                return null;
                            }
                        }
                        else
                        {
                            return null;
                        }
                    }
                }

                success = true;

            }
            catch(Exception e)
            {
                telemetry.TrackException(e);
                return null;

            }
            finally
            {
                timer.Stop();
                telemetry.TrackDependency("Scrapper", "Login", startTime, timer.Elapsed, success);
            }

        }

        private async Task<HttpWebResponse> InitialLoginPageRequest(HttpWebRequest request)
        {
            request.CookieContainer = new CookieContainer();
            request.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => { return true; };
            return await request.GetResponseAsync() as HttpWebResponse;
        }

        private async Task<HttpWebResponse> LoginConfirmedPageRequest(HttpWebRequest request, CookieContainer cookieContainer)
        {
            request.CookieContainer = cookieContainer;
            request.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => { return true; };
            return await request.GetResponseAsync() as HttpWebResponse;
        }

        private HtmlNode GetScrappedDoc(Stream stream)
        {
            HtmlDocument html = new HtmlDocument();
            var encoding = ASCIIEncoding.ASCII;
            using (var reader = new System.IO.StreamReader(stream, encoding))
            {
                string responseText = reader.ReadToEnd();
                html.LoadHtml(responseText);
            }

            return html.DocumentNode;
        }

        private HttpRequestMessage GetTokenRequestMessage(HtmlNode html, LoginModel model)
        {
            var keyValues = new List<KeyValuePair<string, string>>();

            keyValues.Add(new KeyValuePair<string, string>("logonIdentifier", model.Rut));
            keyValues.Add(new KeyValuePair<string, string>("password", model.Password));
            keyValues.Add(new KeyValuePair<string, string>("request_type", "RESPONSE"));

            HtmlNodeCollection nodes = html.SelectNodes("//script");
            HtmlNode node = nodes.Where(x => x.InnerHtml.Contains("SETTINGS")).FirstOrDefault();
            string script = node.InnerText;
            string settingsData = script.Substring(script.IndexOf("var SETTINGS = "));
            settingsData = settingsData.Substring(0, settingsData.IndexOf(";"));
            string contentData = settingsData.Substring(settingsData.IndexOf("{\"remoteResource\""));
            JObject settingsDataJson = JObject.Parse(contentData);

            string tx = settingsDataJson.GetValue("transId").ToString();
            string csrf = settingsDataJson.GetValue("csrf").ToString();

            var request = new HttpRequestMessage(HttpMethod.Post, GetMicrosoftLoginSelfAssertedUri(tx));
            request.Content = new FormUrlEncodedContent(keyValues);
            request.Headers.Add("X-CSRF-TOKEN", csrf);
            request.Headers.Add("X-Requested-With", "XMLHttpRequest");

            return request;
        }

        private Uri GetMicrosoftLoginUri()
        {
            UriBuilder builder = new UriBuilder(String.Format(CultureInfo.InvariantCulture, Startup.aadInstance, Startup.tenant, "/oauth2/v2.0", "/authorize"));
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["client_Id"] = Startup.clientId;
            parameters["response_type"] = "id_token";
            parameters["redirect_uri"] = Startup.redirectUri;
            parameters["response_mode"] = "form_post";
            parameters["scope"] = "openid";
            parameters["p"] = Startup.SignInPolicyId;
            parameters["prompt"] = "login";
            parameters["nonce"] = "defaultNonce";
            builder.Query = parameters.ToString();
            return builder.Uri;
        }

        private Uri GetMicrosoftLoginConfirmedUri(HttpRequestMessage tokenRequest)
        {
            UriBuilder builder = new UriBuilder(String.Format(CultureInfo.InvariantCulture, Startup.aadInstance, Startup.tenant, "/" + Startup.SignInPolicyId, "/api/CombinedSigninAndSignup/confirmed"));
            var parameters = HttpUtility.ParseQueryString(string.Empty);

            string csrf = tokenRequest.Headers.GetValues("X-CSRF-TOKEN").First();
            string tx = HttpUtility.ParseQueryString(tokenRequest.RequestUri.Query).Get("tx");

            parameters["csrf_token"] = csrf;
            parameters["tx"] = tx;
            parameters["metrics"] = "";
            parameters["p"] = Startup.SignInPolicyId;
            builder.Query = parameters.ToString();
            return builder.Uri;
        }

        private Uri GetMicrosoftLoginSelfAssertedUri(string tx)
        {
            UriBuilder builder = new UriBuilder(String.Format(CultureInfo.InvariantCulture, Startup.aadInstance, Startup.tenant, "/" + Startup.SignInPolicyId, "/SelfAsserted"));
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["tx"] = tx;
            parameters["p"] = Startup.SignInPolicyId;
            builder.Query = parameters.ToString();
            return builder.Uri;
        }

        public async Task SetSignInClaims(ClaimsIdentity identity, IdToken token)
        {
            identity.AddClaim(new Claim(Startup.objectIdClaimKey, token.Oid));
            identity.AddClaim(new Claim(ClaimTypes.GivenName, token.Names));
            identity.AddClaim(new Claim(ClaimTypes.Surname, token.Surnames));
            identity.AddClaim(new Claim(countryClaimKey, token.Country));
            identity.AddClaim(new Claim(cityClaimKey, token.City));

            //Retrieve user info
            GraphApiResponseInfo response = await graphApiClient.GetUserByObjectId(token.Oid);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                DateTime temporalPasswordTimestamp;
                bool timeStampResult = DateTime.TryParse(response.User.TemporalPasswordTimestamp, out temporalPasswordTimestamp);

                identity.AddClaim(new Claim(Startup.temporalPasswordTimestampClaimKey, temporalPasswordTimestamp.ToString()));

                if (response.User.IsTemporalPassword)
                {
                    identity.AddClaim(new Claim(Startup.isTemporalPasswordClaimKey, bool.TrueString));
                }
                else
                {
                    identity.AddClaim(new Claim(Startup.isTemporalPasswordClaimKey, bool.FalseString));
                }
            }
            else
            {
                throw new NullReferenceException("No se encontró información asociada al usuario.");
            }
        }

    }
}