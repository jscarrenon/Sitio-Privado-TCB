using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

// The following using statements were added for this sample.
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OpenIdConnect;
using Microsoft.Owin.Security.Cookies;
using Sitio_Privado.Policies;
using System.Security.Claims;
using Sitio_Privado.Models;
using System.Web.Http;
using System.Threading.Tasks;
using System.Globalization;
using System.Configuration;
using Microsoft.AspNet.Identity;
using System.Net;
using System.Net.Http;
using System.IO;
using System.Text;
using HtmlAgilityPack;
using ScrapySharp.Extensions;
using Sitio_Privado.Extras;
using Newtonsoft.Json.Linq;

namespace Sitio_Privado.Controllers
{
    public class AccountController : BaseController
    {
        // App config settings
        private static string clientId = ConfigurationManager.AppSettings["ida:ClientId"];
        private static string aadInstance = ConfigurationManager.AppSettings["ida:AadInstance"];
        private static string tenant = ConfigurationManager.AppSettings["ida:Tenant"];
        private static string redirectUri = ConfigurationManager.AppSettings["ida:RedirectUri"];
        private static string signInPolicy = ConfigurationManager.AppSettings["ida:SignInPolicyId"];

        public void SignIn()
        {
            if (!Request.IsAuthenticated)
            {
                // To execute a policy, you simply need to trigger an OWIN challenge.
                // You can indicate which policy to use by adding it to the AuthenticationProperties using the PolicyKey provided.

                HttpContext.GetOwinContext().Authentication.Challenge(
                    new AuthenticationProperties(
                        new Dictionary<string, string>
                        {
                            {Startup.PolicyKey, Startup.SignInPolicyId}
                        })
                    {
                        RedirectUri = "/",
                    }, OpenIdConnectAuthenticationDefaults.AuthenticationType);
            }
        }

        public ActionResult SignOut()
        {
            HttpContext.GetOwinContext().Authentication.SignOut();

            return Redirect("https://www.tanner.cl");
        }

        [System.Web.Mvc.AllowAnonymous]
        [System.Web.Mvc.HttpGet]
        public ActionResult SignInExternal()
        {
            if (!Request.IsAuthenticated)
            {
                return View();
            }

            return RedirectToAction("Index", "Home");
        }

        [System.Web.Mvc.AllowAnonymous]
        [System.Web.Mvc.HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> SignInExternal(LoginModel model)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Index", "Home"); ; //TODO change

            IdToken token = await GetToken(model);

            if (token == null)
            {
                ModelState.AddModelError("", "RUT o contraseña no válidos. Por favor intente nuevamente.");
                return View(model); //TODO change
            }

            var identity = new ClaimsIdentity(DefaultAuthenticationTypes.ApplicationCookie);
            identity.AddClaim(new Claim("http://schemas.microsoft.com/identity/claims/objectidentifier", token.Oid));
            identity.AddClaim(new Claim(ClaimTypes.GivenName, token.Names));
            identity.AddClaim(new Claim(ClaimTypes.Surname, token.Surnames));
            identity.AddClaim(new Claim("country", token.Country));
            identity.AddClaim(new Claim("city", token.City));
            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;
            authManager.SignIn(identity);

            return RedirectToAction("Index", "Home"); 
        }

        private async Task<IdToken> GetToken(LoginModel model)
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
                        Uri azureLoginConfirmedPageUri = GetMicrosoftLoginConfirmedUri(csrf,tx);
                        HttpWebRequest azureLoginConfirmedPageRequest = HttpWebRequest.CreateHttp(azureLoginConfirmedPageUri);
                        HttpWebResponse azureLoginConfirmedPageResponse = await LoginConfirmedPageRequest(azureLoginConfirmedPageRequest, cookieContainer);

                        //Read id_token
                        HtmlNode htmlNode = GetScrappedDoc(azureLoginConfirmedPageResponse.GetResponseStream());
                        if (htmlNode.CssSelect("input[name=id_token]").Count() > 0)
                        {
                            string id_token = htmlNode.CssSelect("input[name=id_token]").First().GetAttributeValue("value");
                            IdToken parsedToken = new IdToken(id_token);
                            return parsedToken;
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
            HtmlNode node = nodes.Where(x => x.InnerHtml.Contains("settings.data")).FirstOrDefault();
            string script = node.InnerText;
            string settingsData = script.Substring(script.IndexOf("define('settings.data'"));
            settingsData = settingsData.Substring(0, settingsData.IndexOf(");"));
            string contentData = settingsData.Substring(settingsData.IndexOf("{\"remoteResource\""));
            JObject settingsDataJson = JObject.Parse(contentData);

            tx = settingsDataJson.GetValue("transId").ToString();
            csrf = settingsDataJson.GetValue("csrf").ToString();

            var request = new HttpRequestMessage(HttpMethod.Post, "https://login.microsoftonline.com/kundertannerprivado.onmicrosoft.com/B2C_1_Custom_B2C_Policy/SelfAsserted?tx="+tx+"&p=B2C_1_Custom_B2C_Policy");
            request.Content = new FormUrlEncodedContent(keyValues);
            request.Headers.Add("X-CSRF-TOKEN", csrf);
            request.Headers.Add("X-Requested-With", "XMLHttpRequest");

            return request;
        }

        private static string csrf = "";
        private static string tx = "";

        private Uri GetMicrosoftLoginUri()
        {
            UriBuilder builder = new UriBuilder(String.Format(CultureInfo.InvariantCulture, aadInstance, tenant, "/oauth2/v2.0", "/authorize"));
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["client_Id"] = clientId;
            parameters["response_type"] = "id_token";
            parameters["redirect_uri"] = redirectUri;
            parameters["response_mode"] = "form_post";
            parameters["scope"] = "openid";
            parameters["p"] = signInPolicy;
            parameters["prompt"] = "login";
            parameters["nonce"] = "defaultNonce";
            builder.Query = parameters.ToString();
            return builder.Uri;
        }

        private Uri GetMicrosoftLoginConfirmedUri(string csrf, string tx)
        {
            UriBuilder builder = new UriBuilder("https://login.microsoftonline.com/kundertannerprivado.onmicrosoft.com/B2C_1_Custom_B2C_Policy/api/CombinedSigninAndSignup/confirmed");
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["csrf_token"] = csrf;
            parameters["tx"] = tx;
            parameters["metrics"] = "";
            parameters["p"] = signInPolicy;
            builder.Query = parameters.ToString();
            return builder.Uri;
        }
    }
}