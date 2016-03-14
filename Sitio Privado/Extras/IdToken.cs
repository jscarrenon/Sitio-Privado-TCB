using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Sitio_Privado.Extras
{
    public class IdToken
    {
        public IdToken(string base64)
        {
            var strings = base64.Split('.');
            var encodedData = strings.ElementAt(1);
            DecodeFromString(encodedData);
        }

        private void DecodeFromString(string encodedData)
        {
            byte[] encodedDataAsBytes = JWT.JsonWebToken.Base64UrlDecode(encodedData);
            string decoded = System.Text.ASCIIEncoding.UTF8.GetString(encodedDataAsBytes);
            JObject json = JObject.Parse(decoded);
            Oid = json.GetValue("oid").ToString();
        }

        public string Oid { get; set; }
    }
}