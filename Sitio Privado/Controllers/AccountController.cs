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
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using Sitio_Privado.Extras;
using System.Net;
using HtmlAgilityPack;
using ScrapySharp.Extensions;
using System.Text;
using System.Net.Http;
using System.IO;

namespace Sitio_Privado.Controllers
{
    [AllowAnonymous]
    public class AccountController : BaseController
    {
        [HttpGet]
        public ActionResult SignIn()
        {
            if (!Request.IsAuthenticated)
            {
                // To execute a policy, you simply need to trigger an OWIN challenge.
                // You can indicate which policy to use by adding it to the AuthenticationProperties using the PolicyKey provided.

                /*HttpContext.GetOwinContext().Authentication.Challenge(
                    new AuthenticationProperties(
                        new Dictionary<string, string>
                        {
                            {Startup.PolicyKey, Startup.SignInPolicyId}
                        })
                    {
                        RedirectUri = "/",
                    }, OpenIdConnectAuthenticationDefaults.AuthenticationType);*/

                return View();
            }

            return RedirectToAction("Index", "Home");
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SignIn(LoginModel model)
        {
            if (!ModelState.IsValid)
                return View(model); //TODO change

            IdToken token = await GetToken(model);

            if (token == null)
                return null; //TODO change

            var identity = new ClaimsIdentity(DefaultAuthenticationTypes.ApplicationCookie);
            identity.AddClaim(new Claim("http://schemas.microsoft.com/identity/claims/objectidentifier", token.Oid));
            identity.AddClaim(new Claim(ClaimTypes.GivenName, token.Names));
            identity.AddClaim(new Claim(ClaimTypes.Surname, token.Surnames));
            identity.AddClaim(new Claim("country", token.Country));
            identity.AddClaim(new Claim("city", token.City));
            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;
            authManager.SignIn(identity);

            return RedirectToAction("Index", "Home"); //TODO check
        }

        public ActionResult SignOut()
        {
            // To sign out the user, you should issue an OpenIDConnect sign out request using the last policy that the user executed.
            // This is as easy as looking up the current value of the ACR claim, adding it to the AuthenticationProperties, and making an OWIN SignOut call.

            /*HttpContext.GetOwinContext().Authentication.SignOut(
                new AuthenticationProperties(
                    new Dictionary<string, string>
                    {
                        {Startup.PolicyKey, ClaimsPrincipal.Current.FindFirst(Startup.AcrClaimType).Value}
                    }), OpenIdConnectAuthenticationDefaults.AuthenticationType, CookieAuthenticationDefaults.AuthenticationType);*/

            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;

            authManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("SignIn", "Account");
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

                    //Read id_token
                    HtmlNode htmlNode = GetScrappedDoc(await result.Content.ReadAsStreamAsync());
                    if(htmlNode.CssSelect("input[name=id_token]").Count() > 0)
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
            }
        }

        private async Task<HttpWebResponse> InitialLoginPageRequest(HttpWebRequest request)
        {
            request.CookieContainer = new CookieContainer();
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
            keyValues.Add(new KeyValuePair<string, string>("LoginOptions", "3"));
            keyValues.Add(new KeyValuePair<string, string>("NewUser", "1"));
            keyValues.Add(new KeyValuePair<string, string>("PwdPad", ""));
            keyValues.Add(new KeyValuePair<string, string>("ctx", html.CssSelect("input[name=ctx]").First().GetAttributeValue("value")));
            keyValues.Add(new KeyValuePair<string, string>("flowToken", html.CssSelect("input[name=flowToken]").First().GetAttributeValue("value")));
            keyValues.Add(new KeyValuePair<string, string>("i12", "1"));
            keyValues.Add(new KeyValuePair<string, string>("i13", "Chrome"));
            keyValues.Add(new KeyValuePair<string, string>("i14", "48.0.2564.116"));
            keyValues.Add(new KeyValuePair<string, string>("i15", "1366"));
            keyValues.Add(new KeyValuePair<string, string>("i16", "768"));
            keyValues.Add(new KeyValuePair<string, string>("i20", ""));
            keyValues.Add(new KeyValuePair<string, string>("idsbho", "1"));
            keyValues.Add(new KeyValuePair<string, string>("login", model.Rut));
            keyValues.Add(new KeyValuePair<string, string>("passwd", model.Password));
            keyValues.Add(new KeyValuePair<string, string>("sso", ""));
            keyValues.Add(new KeyValuePair<string, string>("type", "11"));
            keyValues.Add(new KeyValuePair<string, string>("uiver", "1"));
            keyValues.Add(new KeyValuePair<string, string>("vv", ""));

            var request = new HttpRequestMessage(HttpMethod.Post, "/kundertannerprivado.onmicrosoft.com/login");
            request.Content = new FormUrlEncodedContent(keyValues);

            return request;
        }

        private Uri GetMicrosoftLoginUri()
        {
            UriBuilder builder = new UriBuilder("https://login.microsoftonline.com/kundertannerprivado.onmicrosoft.com/oauth2/v2.0/authorize");
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["client_id"] = "cf6f8deb-1db9-47c7-a870-d78973dacfd8";
            parameters["response_type"] = "id_token";
            parameters["redirect_uri"] = "https://privado.tanner.kunder.cl";
            parameters["response_mode"] = "form_post";
            parameters["scope"] = "openid";
            parameters["p"] = "b2c_1_signin";
            builder.Query = parameters.ToString();
            return builder.Uri;
        }
    }
}