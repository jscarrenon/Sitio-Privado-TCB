using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Claims;
using Sitio_Privado.Models;
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
using Sitio_Privado.Helpers;
using System.Net.Configuration;
using Microsoft.Owin.Security;
using Sitio_Privado.Filters;
using System.Security.Principal;

namespace Sitio_Privado.Controllers
{
    public class AccountController : BaseController
    {
        private const string countryClaimKey = "country";
        private const string cityClaimKey = "city";

        GraphApiClientHelper graphApiClient = new GraphApiClientHelper();
        private static readonly double passwordExpiresInHours = double.Parse(Startup.temporalPasswordTimeout, CultureInfo.InvariantCulture);

        [SkipPasswordExpired]
        public ActionResult SignOut()
        {
            HttpContext.GetOwinContext().Authentication.SignOut();

            return Redirect("https://www.tanner.cl");
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult SignInExternal()
        {
            if (!Request.IsAuthenticated)
            {
                return View();
            }

            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        [HttpPost]
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
            await SetSignInClaims(identity, token);

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
                        Uri azureLoginConfirmedPageUri = GetMicrosoftLoginConfirmedUri(tokenRequest);
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

        private async Task SetSignInClaims(ClaimsIdentity identity, IdToken token)
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
                DateTime temporalPasswordTimestamp = DateTime.Parse(response.User.TemporalPasswordTimestamp);
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

        [SkipPasswordExpired]
        [HttpGet]
        public ActionResult ChangePassword()
        {
            IPrincipal user = this.User; 

            Claim claim = ((ClaimsIdentity)user.Identity).Claims.Where(c => c.Type == Startup.isTemporalPasswordClaimKey).First();
            bool isTemporalPassword = bool.Parse(claim.Value);

            claim = ((ClaimsIdentity)user.Identity).Claims.Where(c => c.Type == Startup.temporalPasswordTimestampClaimKey).First();
            DateTime temporalPasswordTimestamp = DateTime.Parse(claim.Value);

            if (isTemporalPassword)
            {
                DateTime limit = temporalPasswordTimestamp.AddHours(passwordExpiresInHours);

                if (DateTime.Now > limit)
                {
                    ViewBag.Message = "Su contraseña temporal ha caducado. Por favor solicite una nueva.";
                }
            }

            return View();
        }

        [SkipPasswordExpired]
        [HttpPost]
        public async Task<ActionResult> ChangePassword(ChangePasswordModel model)
        {
            IPrincipal user = this.User;

            Claim isTemporalPasswordClaim = ((ClaimsIdentity)user.Identity).Claims.Where(c => c.Type == Startup.isTemporalPasswordClaimKey).First();
            bool isTemporalPassword = bool.Parse(isTemporalPasswordClaim.Value);

            Claim temporalPasswordTimestampClaim = ((ClaimsIdentity)user.Identity).Claims.Where(c => c.Type == Startup.temporalPasswordTimestampClaimKey).First();
            DateTime temporalPasswordTimestamp = DateTime.Parse(temporalPasswordTimestampClaim.Value);

            if (isTemporalPassword)
            {
                DateTime limit = temporalPasswordTimestamp.AddHours(passwordExpiresInHours);

                if (DateTime.Now > limit)
                {
                    return RedirectToAction("ChangePassword");
                }
            }

            if (ModelState.IsValid)
            {
                //Retrieve user info
                Claim idClaim = ((ClaimsIdentity)user.Identity).Claims.Where(c => c.Type == Startup.objectIdClaimKey).First();
                GraphApiResponseInfo getUserResponse = await graphApiClient.GetUserByObjectId(idClaim.Value);
                if (getUserResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    getUserResponse.User.TemporalPassword = model.Password;
                    getUserResponse.User.IsTemporalPassword = false;
                    getUserResponse.User.TemporalPasswordTimestamp = DateTime.MinValue.ToString();

                    var apiResponse = await graphApiClient.ResetUserPassword(getUserResponse.User.ObjectId, getUserResponse.User);

                    if (apiResponse.StatusCode != System.Net.HttpStatusCode.NoContent)
                    {
                        ModelState.AddModelError("", "Error al intentar cambiar la contraseña. Intente otra vez.");
                        return View(model);
                    }
                    else
                    {
                        //Update claims
                        ((ClaimsIdentity)user.Identity).RemoveClaim(isTemporalPasswordClaim);
                        ((ClaimsIdentity)user.Identity).AddClaim(new Claim(Startup.isTemporalPasswordClaimKey, bool.FalseString));

                        ((ClaimsIdentity)user.Identity).RemoveClaim(temporalPasswordTimestampClaim);
                        ((ClaimsIdentity)user.Identity).AddClaim(new Claim(Startup.temporalPasswordTimestampClaimKey, DateTime.MinValue.ToString()));

                        var ctx = Request.GetOwinContext();
                        var authManager = ctx.Authentication;
                        authManager.AuthenticationResponseGrant = new AuthenticationResponseGrant(new ClaimsPrincipal(user.Identity), new AuthenticationProperties() { IsPersistent = true });

                        //Sign out B2C TODO

                        //Display success message: "Su contraseña ha sido modificadda con éxito" TODO

                        //Send mail
                        try
                        {
                            SmtpSection settings = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");
                            var email = new ChangePasswordEmailModel
                            {
                                From = settings.From,
                                User = getUserResponse.User
                            };
                            email.Send();
                        }
                        catch(Exception e)
                        {
                            //TODO
                        }

                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    throw new NullReferenceException("No se encontró información asociada al usuario.");
                }
            }

            return View(model);
        }
    }
}