using ScrapySharp.Extensions;
using ScrapySharp.Html.Forms;
using ScrapySharp.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using HtmlAgilityPack;

namespace Sitio_Privado.Controllers
{
    [OverrideAuthorization]
    public class CustomController : ApiController
    {
        private CookieContainer container = new CookieContainer();

        private async Task<HttpWebResponse> InitialLoginPageRequest(HttpWebRequest request)
        {
            CookieContainer c = new CookieContainer();
            request.CookieContainer = c;
            request.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => { return true; };
            WebResponse r = await request.GetResponseAsync();
            HttpWebResponse response = r as HttpWebResponse;
            return response;
        }

        private HtmlNode GetScrappedDoc(HttpWebResponse response)
        {
            HtmlDocument html = new HtmlDocument();
            var encoding = ASCIIEncoding.ASCII;
            using (var reader = new System.IO.StreamReader(response.GetResponseStream(), encoding))
            {
                string responseText = reader.ReadToEnd();
                html.LoadHtml(responseText);
            }

            return html.DocumentNode;
        }

        [HttpGet]
        public async Task<HttpResponseMessage> Test()
        {
            Uri azureLoginPageUri = GetInitialLoginUri();
            HttpWebRequest azureLoginPageRequest = HttpWebRequest.CreateHttp(azureLoginPageUri);
            HttpWebResponse azureLoginPageResponse = await InitialLoginPageRequest(azureLoginPageRequest);
            CookieCollection azureLoginPageCookies = azureLoginPageRequest.CookieContainer.GetCookies(azureLoginPageUri);
            azureLoginPageCookies.Add(azureLoginPageResponse.Cookies);
            HtmlNode html = GetScrappedDoc(azureLoginPageResponse);

            HttpRequestMessage tokenRequest = GetTokenRequestMessage(html);

            var baseAddress = new Uri("https://login.microsoftonline.com");
            CookieContainer cookieContainer = new CookieContainer();
            cookieContainer.Add(azureLoginPageCookies);
            using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
            {
                using (var client = new HttpClient(handler) { BaseAddress = baseAddress })
                {
                    var result = await client.SendAsync(tokenRequest);
                    string content = await result.Content.ReadAsStringAsync();

                    HttpRequestMessage finalRequest = new HttpRequestMessage(HttpMethod.Get, result.RequestMessage.RequestUri.AbsoluteUri);
                    var finalResult = await client.SendAsync(finalRequest);

                }
            }


            return new HttpResponseMessage();
        }

        private HttpRequestMessage GetTokenRequestMessage(HtmlNode html)
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
            keyValues.Add(new KeyValuePair<string, string>("login", "181163631"));
            keyValues.Add(new KeyValuePair<string, string>("passwd", "Kunder2016"));
            keyValues.Add(new KeyValuePair<string, string>("sso", ""));
            keyValues.Add(new KeyValuePair<string, string>("type", "11"));
            keyValues.Add(new KeyValuePair<string, string>("uiver", "1"));
            keyValues.Add(new KeyValuePair<string, string>("vv", ""));

            var request = new HttpRequestMessage(HttpMethod.Post, "/kundertannerprivado.onmicrosoft.com/login");
            request.Content = new FormUrlEncodedContent(keyValues);

            return request;
        }


        private Uri GetInitialLoginUri()
        {
            UriBuilder builder = new UriBuilder("https://login.microsoftonline.com/kundertannerprivado.onmicrosoft.com/oauth2/v2.0/authorize");
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters["client_id"] = "cf6f8deb-1db9-47c7-a870-d78973dacfd8";
            parameters["response_type"] = "code";
            parameters["redirect_uri"] = "https://google.cl";
            parameters["response_mode"] = "query";
            parameters["scope"] = "openid";
            parameters["p"] = "b2c_1_signin";
            builder.Query = parameters.ToString();
            return builder.Uri;
        }

        private void ReadCookies(HttpWebResponse r)
        {
            CookieCollection cookies = r.Cookies;
            container.Add(cookies);
        }
    }
}
