using Sitio_Privado.Models;
using System.Collections.Generic;

namespace Sitio_Privado.Services.Interfaces
{
    public interface IAuthorityClientService
    {
        Usuario VerifyLoginAndGetPersonInformation(string accessToken, IEnumerable<string> roles);
    }
}