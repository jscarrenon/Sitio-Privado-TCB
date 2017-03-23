using Sitio_Privado.Models;
using System.Collections.Generic;

namespace Sitio_Privado.Services.Interfaces
{
    public interface IAuthorityClientService
    {
        List<SiteInformation> GetUserSitesByToken(string accessToken, bool dummy);
        List<SiteInformation> GetDummySites(string accessToken);
    }
}