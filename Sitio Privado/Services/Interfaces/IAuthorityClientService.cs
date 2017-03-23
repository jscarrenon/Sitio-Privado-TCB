using Sitio_Privado.Models;
using System.Collections.Generic;

namespace Sitio_Privado.Services.Interfaces
{
    public interface IAuthorityClientService
    {
        List<SiteInformation> GetUserSitesByToken(IEnumerable<string> userGroups);
        List<SiteInformation> GetDummySites(IEnumerable<string> userGroups);
    }
}