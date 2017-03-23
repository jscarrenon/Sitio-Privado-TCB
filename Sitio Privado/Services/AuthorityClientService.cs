using Sitio_Privado.Services.Interfaces;
using System.Collections.Generic;
using Sitio_Privado.Models;
using System.Linq;

namespace Sitio_Privado.Services
{
    public class AuthorityClientService : IAuthorityClientService
    {
        private const string UserInfoPath = "connect/userinfo";
        private const string ValidateTokenPath = "connect/accesstokenvalidation";
        IExternalUserService userService;
        public AuthorityClientService(IExternalUserService userService)
        {
            this.userService = userService;
        }

        /// <summary>
        /// Returns all the sites information of the groups given
        /// </summary>
        /// <param name="userGroups">The groups of permissions of the user</param>
        /// <returns>A list of the sites of the user</returns>
        public List<SiteInformation> GetUserSites(IEnumerable<string> userGroups)
        {
            var resultSites = userService.GetAllSites();
            var userSites = MatchUserInfo(resultSites, userGroups);

            return userSites;
        }
       
        private List<SiteInformation> MatchUserInfo(List<SiteInformation> allSites, IEnumerable<string> userGroups)
        {
            List<SiteInformation> list = new List<SiteInformation>();

            foreach (var site in allSites)
            {
                if (userGroups != null)
                {
                    foreach (var group in userGroups)
                    {
                        if (site.SiteName.ToLower().Equals(group.ToLower()))
                        {
                            list.Add(site);
                        }
                    }
                }
            }

            return  list.OrderBy(s => s.Priority).ToList();
        }
    }
}