using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Sitio_Privado.Controllers
{
    [OverrideAuthorization]
    public class CustomController : ApiController
    {
        private CookieContainer container = new CookieContainer();

        [HttpGet]
        public async Task<HttpWebResponse> Test()
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
            var uri = builder.Uri;
            CookieContainer c = new CookieContainer();
            HttpWebRequest request = HttpWebRequest.CreateHttp(uri);request.Method = HttpMethod.Get.ToString();
            request.CookieContainer = c;
            request.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => { return true; };
            WebResponse r = await request.GetResponseAsync();
            HttpWebResponse response = r as HttpWebResponse;
            CookieCollection container = request.CookieContainer.GetCookies(uri);
            CookieCollection cookies = response.Cookies;

            return response;

        }

        private void ReadCookies(HttpWebResponse r)
        {
            CookieCollection cookies = r.Cookies;
            container.Add(cookies);
        }
    }
}
