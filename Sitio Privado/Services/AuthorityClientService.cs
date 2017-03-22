using Sitio_Privado.Services.Interfaces;
using System.Collections.Generic;
using System.Web;
using Sitio_Privado.Models;
using RestSharp;
using System.Net;
using Newtonsoft.Json;
using System.Configuration;
using System;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Sitio_Privado.Helpers;
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
        public virtual Person VerifyLoginAndGetPersonInformation(string accessToken, IEnumerable<string> roles)
        {
            Person persona = null;
            //string userRole = RoleConstants.Client;
            AuthorityUserInfo authorityUserInfo = GetUserInformation(accessToken);

            // GetUserFrom
            // person = repository.GetSingle<Person>(p => p.AuthorityId == authorityUserInfo.AuthorityId, true);

            if (authorityUserInfo != null )
            {
                persona = new Person();
                persona.Autenticado = true;
                persona.Rut = authorityUserInfo.Rut;
                persona.Nombres = authorityUserInfo.GivenName;
                persona.Apellidos = authorityUserInfo.LastName;
                persona.NombreCompleto = authorityUserInfo.GivenName + " " + authorityUserInfo.LastName + " " + authorityUserInfo.MothersLastName;
                persona.Email = authorityUserInfo.Email;

                //UserHelper.AttachPersonAsUserClaims()
            }

            return persona;
        }
        public AuthorityUserInfo GetUserInformation(string accessToken)
        {
            RestClient client = new RestClient();
            client.BaseUrl = new Uri(ConfigurationManager.AppSettings["AuthorityUrl"]);
            var request = new RestRequest(UserInfoPath, Method.POST);
            request.AddHeader("Authorization", "Bearer " + accessToken);
            IRestResponse response = client.Execute(request);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new HttpException((int)response.StatusCode, "Error obtaining user information from authority server");
            }

            var content = response.Content;
            dynamic userInfo = JsonConvert.DeserializeObject(content);

            AuthorityUserInfo userInformation = new AuthorityUserInfo()
            {
                AuthorityId = userInfo["sub"],
                GivenName = userInfo["first_name"],
                LastName = userInfo["last_name"],
                MothersLastName = userInfo["mothers_last_name"],
                Rut = userInfo["rut"],
                Email = userInfo["email"]
            };

            return userInformation;
        }

        public Person GetPersonInformationByToken(string accessToken)
        {
            Person persona = null;
            AuthorityUserInfo authorityUserInfo = GetUserInformation(accessToken);

            if (authorityUserInfo != null)
            {
                persona = new Person();
                persona.Autenticado = true;
                persona.Rut = authorityUserInfo.Rut;
                persona.Nombres = authorityUserInfo.GivenName;
                persona.Apellidos = authorityUserInfo.LastName;
                persona.NombreCompleto = authorityUserInfo.GivenName + " " + authorityUserInfo.LastName + " " + authorityUserInfo.MothersLastName;
                persona.Email = authorityUserInfo.Email;

            }

            return persona;
        }
        public Usuario GetUserInformationByToken(string accessToken)
        {
            Usuario usuario = null;
            AuthorityUserInfo authorityUserInfo = GetUserInformation(accessToken);

            if (authorityUserInfo != null)
            {
                var userInfo = userService.GetUserInfoByUsername(authorityUserInfo.Rut);

                usuario = new Usuario()
                {
                    Rut = userInfo.Rut.Insert(userInfo.Rut.Length - 1, "-"),
                    Banco = userInfo.Bank,
                    CuentaCorriente = userInfo.CheckingAccount,
                    DireccionComercial = userInfo.WorkAddress,
                    DireccionParticular = userInfo.HomeAddress,
                    Email = userInfo.Email,
                    TelefonoComercial = userInfo.WorkPhone
                };
            }

            return usuario;
        }

        // return values to list allowed sites to view
        public List<SiteInformation> GetUserSitesByToken(string userToken)
        {
            RestClient client = new RestClient();
            client.BaseUrl = new Uri(ConfigurationManager.AppSettings["AuthorityUrl"]);

            var settings = new JsonSerializerSettings()
            {
                ContractResolver = new JsonLowerCaseUnderscoreContractResolver()
            };

            var request = new RestRequest(ValidateTokenPath, Method.POST);

            request.AddParameter("token", userToken);

            IRestResponse response = client.Execute(request);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }

            var contentTokenDecoded = DecodeToken(response);

            if (contentTokenDecoded == null)
            {
                return null;
            }

            var resultSites = userService.GetAllSites();
            var userSites = MatchUserInfo(resultSites, contentTokenDecoded);

            return userSites;
        }
       

        // Method to verify group assigned to the user
        public List<SiteInformation> MatchUserInfo(List<SiteInformation> allSites, TokenValidationVM resultToken)
        {
            List<SiteInformation> list = new List<SiteInformation>();

            foreach (var site in allSites)
            {
                var groupName = site.Cn.Split('_')[1].ToLower();

                if (resultToken.Groups != null)
                {
                    foreach (var item in resultToken.Groups)
                    {
                        var assignGroup = item.Split('_')[0].ToLower();

                        if (site.SiteName.ToLower().Equals(item.ToLower()) && !list.Contains(site))
                        {
                            list.Add(site);
                        }
                    }
                }
                else if (resultToken.Group.Length > 0)
                {
                    var assignGroup = resultToken.Group.Split('_')[0].ToLower();

                    if (groupName.Equals(assignGroup))
                    {
                        list.Add(site);
                    }
                }
            }

            return  list.OrderBy(s => s.Priority).ToList();
        }

        public TokenValidationVM DecodeToken(IRestResponse contentRequest)
        {
            var settings = new JsonSerializerSettings()
            {
                ContractResolver = new JsonLowerCaseUnderscoreContractResolver()
            };

            TokenValidationVM resultToken = null;
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            JToken value;
            string groupAssigned = "";
            dynamic requestTokenResult = JsonConvert.DeserializeObject(contentRequest.Content, settings);
            string[] list = null;

            if (requestTokenResult.TryGetValue("groups", out value))
            {
                if (value.HasValues)
                {
                    list = requestTokenResult["groups"].ToObject<string[]>();
                }
                else
                {
                    groupAssigned = requestTokenResult["groups"].ToObject<string>();
                }
            }

            string[] listScopes = requestTokenResult.TryGetValue("scope", out value) ? requestTokenResult["scope"].ToObject<string[]>() : null;

            if (requestTokenResult != null)
            {
                resultToken = new TokenValidationVM()
                {
                    Amr = requestTokenResult["amr"],
                    Aud = requestTokenResult["aud"],
                    AuthTime = requestTokenResult["auth_time"],
                    ClientId = requestTokenResult["client_id"],
                    Exp = requestTokenResult["exp"],
                    Group = groupAssigned,
                    Groups = list != null ? new List<string>(list) : null,
                    Idp = requestTokenResult["idp"],
                    Iss = requestTokenResult["iss"],
                    Nbf = requestTokenResult["nbf"],
                    Scope = listScopes != null ? new List<string>(listScopes) : null,
                    Sub = requestTokenResult["sub"]
                };
            }

            return resultToken;
        }

        //Dummy method
        public List<SiteInformation> GetDummySites(string accessToken)
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