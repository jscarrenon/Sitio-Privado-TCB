using Sitio_Privado.Models;
using System.Collections.Generic;

namespace Sitio_Privado.Services.Interfaces
{
    public interface IAuthorityClientService
    {
        Person VerifyLoginAndGetPersonInformation(string accessToken, IEnumerable<string> roles);
        Person GetPersonInformationByToken(string accessToken);
        Usuario GetUserInformationByToken(string accessToken);
    }
}