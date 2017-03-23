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

        // return values to list allowed sites to view
        public List<SiteInformation> GetUserSitesByToken(IEnumerable<string> userGroups)
        {
            var resultSites = userService.GetAllSites();
            var userSites = MatchUserInfo(resultSites, userGroups);

            return userSites;
        }
       

        // Method to verify group assigned to the user
        public List<SiteInformation> MatchUserInfo(List<SiteInformation> allSites, IEnumerable<string> userGroups)
        {
            List<SiteInformation> list = new List<SiteInformation>();

            foreach (var site in allSites)
            {
                var groupName = site.Cn.Split('_')[1].ToLower();

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

        //Dummy method
        public List<SiteInformation> GetDummySites(IEnumerable<string> userGroups)
        {
            var result = new List<SiteInformation>();

            result.Add(new SiteInformation()
            {
                AbbreviateName = "CB",
                Id = 1,
                Cn = "",
                Description = "",
                SiteName = "Corredora de Bolsa",
                SiteType = "Sitio Privado",
                UserId = 1,
                Url = "https://www.tanner.cl/",
                Priority = 2
            });
            result.Add(new SiteInformation()
            {
                AbbreviateName = "F",
                Cn = "",
                Description = "",
                Id = 2,
                SiteName = "Factoring",
                SiteType = "Sitio Privado",
                UserId = 1,
                Url = "factoring.tanner.cl/",
                Priority = 3
            });
            result.Add(new SiteInformation()
            {
                AbbreviateName = "CA",
                Id = 3,
                Cn = "",
                Description = "",
                SiteName = "Crédito Automotriz",
                SiteType = "Sitio Privado",
                UserId = 1,
                Url = "creditoautomotriz.tanner.cl/",
                Priority = 1
            });

            return result.OrderBy(s => s.Priority).ToList();
        }
    }
}