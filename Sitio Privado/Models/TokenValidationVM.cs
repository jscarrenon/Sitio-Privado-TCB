using System.Collections.Generic;

namespace Sitio_Privado.Models
{
    public class TokenValidationVM
    {
        public string Iss { get; set; }
        public string Aud { get; set; }
        public long? Exp { get; set; }
        public long? Nbf { get; set; }
        public string ClientId { get; set; }
        public List<string> Scope { get; set; }
        public string Sub { get; set; }
        public long? AuthTime { get; set; }
        public string Group { get; set; }
        public List<string> Groups { get; set; }
        public string Idp { get; set; }
        public string Amr { get; set; }
    }
}