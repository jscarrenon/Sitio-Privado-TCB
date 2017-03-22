using Newtonsoft.Json.Serialization;
using System.Text.RegularExpressions;

namespace Sitio_Privado.Helpers
{
    /// <summary>
    /// Method from http://stackoverflow.com/questions/18051395/custom-json-net-contract-resolver-for-lowercase-underscore-to-camelcase
    /// </summary>
    public class JsonLowerCaseUnderscoreContractResolver : DefaultContractResolver
    {
        private Regex regex = new Regex("(?!(^[A-Z]))([A-Z])");

        protected override string ResolvePropertyName(string propertyName)
        {
            return regex.Replace(propertyName, "_$2").ToLower();
        }
    }
}