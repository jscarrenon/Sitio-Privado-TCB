using Sitio_Privado.Services.Interfaces;
using System;
using System.Runtime.InteropServices;
using System.DirectoryServices;
using System.DirectoryServices.Protocols;
using System.Configuration;
using System.Collections.Generic;
using Sitio_Privado.Models;
using System.Reflection;
using System.Text;
using NLog;
using System.Net;

namespace Sitio_Privado.Services.ExternalUserProvider
{
    public class LDAPService : IExternalUserService
    {
        private const int invalidCredentialsErrorCode = -2147023570;

        private static readonly string domain = ConfigurationManager.AppSettings["LDAPDomain"];
        private static readonly string usersBaseDN = ConfigurationManager.AppSettings["LDAPUsersBaseDN"];
        private static readonly string groupsBaseDN = ConfigurationManager.AppSettings["LDAPGroupsBaseDN"];

        private const AuthenticationTypes normalAuthenticationTypes = AuthenticationTypes.Delegation;
        private Logger logger;

        // This variable stores the mapping between the LDAP user properties and the UserInfo model.
        private Dictionary<string, string> ldapUserModelMapper;
      
        public LDAPService()
        {
            SetUserSchemaEntries();
            logger = LogManager.GetLogger("SessionLog");
        }

        private Dictionary<string, string> LoadUserModelMapper(IUserSchemaEntryRepository rep)
        {
            return rep.GetExternalFieldToPropetyNameDictionary();
        }

        private Dictionary<string, UserSchemaEntry> LoadUserSchemaByPropertyName(IUserSchemaEntryRepository rep)
        {
            return rep.GetAllByPropertyName();
        }

        public bool Authenticate(string username, string password)
        {
            DirectoryEntry userEntry = null;

            try
            {
                string path = string.Format("LDAP://{0}/{1}", domain, usersBaseDN);
                string ldapUsername = string.Format("cn={0},{1}", username, usersBaseDN);
                userEntry = new DirectoryEntry(path, ldapUsername, password, normalAuthenticationTypes);
                var cnStat = userEntry.NativeObject;    // If the authentication fails, this line throws an exception
                return true;
            }
            catch (COMException ex)
            {
                if (ex.ErrorCode != invalidCredentialsErrorCode)
                {
                    throw ex;
                }
            }
            finally
            {
                if (userEntry != null) userEntry.Dispose();
            }

            return false;
        }

        public Usuario GetUserInfoByUsername(string username)
        {
            DirectorySearcher searcher = InitializeSearcher(usersBaseDN);
            UserInfo userInfo = null;
            Usuario usuario = null;
            DirectoryEntry userEntry = null;

            try
            {
                searcher.Filter = string.Format("cn={0}", username);
                SearchResult searchResult = searcher.FindOne();

                if (searchResult != null)
                {
                    userEntry = searchResult.GetDirectoryEntry();
                    userInfo = BuildUserFromDirectoryEntry(userEntry);

                    usuario = new Usuario()
                    {
                        Nombres = userInfo.FirstName,
                        Apellidos = userInfo.LastName,
                        Rut = userInfo.Rut.Insert(userInfo.Rut.Length - 1, "-"),
                        Banco = userInfo.Bank,
                        CuentaCorriente = userInfo.CheckingAccount,
                        DireccionComercial = userInfo.WorkAddress,
                        DireccionParticular = userInfo.HomeAddress,
                        Email = userInfo.Email,
                        TelefonoComercial = userInfo.WorkPhone,
                        TelefonoParticular = userInfo.HomePhone
                    };
                }
            }
            catch (COMException ex)
            {
                throw ex;
            }
            finally
            {
                if (searcher != null) searcher.Dispose();
                if (userEntry != null) userEntry.Dispose();
            }

            return usuario;
        }

        /// <summary>
        /// Initializes a new instance of the DirectorySearcher class, pointing to differents base DN based on the user type given.
        /// </summary>
        /// <returns>The initialized DirectorySearcher instance</returns>
        private DirectorySearcher InitializeSearcher(string baseDN)
        {
            DirectoryEntry searchRoot = null;
            DirectorySearcher searcher = null;

            searchRoot = new DirectoryEntry(String.Format("LDAP://{0}/{1}", domain, baseDN),
                    ConfigurationManager.AppSettings["LDAPAdminUsername"],
                    ConfigurationManager.AppSettings["LDAPAdminPassword"],
                    normalAuthenticationTypes);

            searcher = new DirectorySearcher(searchRoot);
            searcher.SearchScope = System.DirectoryServices.SearchScope.OneLevel;
            searcher.CacheResults = false;

            return searcher;
        }

        public  Usuario GetUserInfoByUsernameV2(string username)
        {
            Usuario usuario = null;

            NetworkCredential credential = new NetworkCredential(
                ConfigurationManager.AppSettings["LDAPAdminUsername"],
                ConfigurationManager.AppSettings["LDAPAdminPassword"]);

            LdapDirectoryIdentifier identifier = new LdapDirectoryIdentifier(domain, 389);
            LdapConnection connection = new LdapConnection(identifier, credential);
            connection.AuthType = AuthType.Basic;
            connection.Bind(credential);
            connection.SessionOptions.ProtocolVersion = 3;

            var searchRequest = new SearchRequest(
                    usersBaseDN,
                    string.Format("cn={0}", username),
                    System.DirectoryServices.Protocols.SearchScope.Subtree);

            var searchOptions = new SearchOptionsControl(SearchOption.DomainScope);
            searchRequest.Controls.Add(searchOptions);

            var searchResponse = (SearchResponse)connection.SendRequest(searchRequest);

            if (searchResponse != null)
            {
                var userEntry = searchResponse.Entries[0];
                UserInfo userInfo = BuildUserFromDirectoryEntryV2(userEntry);

                usuario = new Usuario()
                {
                    Nombres = userInfo.FirstName,
                    Apellidos = userInfo.LastName,
                    Rut = userInfo.Rut.Insert(userInfo.Rut.Length - 1, "-"),
                    Banco = userInfo.Bank,
                    CuentaCorriente = userInfo.CheckingAccount,
                    DireccionComercial = userInfo.WorkAddress,
                    DireccionParticular = userInfo.HomeAddress,
                    Email = userInfo.Email,
                    TelefonoComercial = userInfo.WorkPhone,
                    TelefonoParticular = userInfo.HomePhone,
                    Ciudad = userInfo.City,
                    Pais = userInfo.Country
                };
            }

            return usuario;
        }

        private void SetUserSchemaEntries()
        {
            // IMPORTANT: All keys of the mapper should be lowercase!
            this.ldapUserModelMapper = new Dictionary<string, string>
            {
                { "uid", "Rut" },
                { "givenname", "FirstName" },
                { "sn", "LastName"},
                { "mail", "Email"},
                { "cuenta_corriente", "CheckingAccount"},
                { "banco", "Bank"},
                { "fono_comercial", "WorkPhone"},
                { "fono_particular", "HomePhone"},
                { "domicilio_particular", "HomeAddress"},
                { "domicilio_comercial", "WorkAddress"},
                { "city", "City"},
                { "countrycode", "Country"}
            };
        }

        public List<SiteInformation> GetAllSites()
        {
            List<SiteInformation> groups = null;
            DirectorySearcher searcher = InitializeSearcher(groupsBaseDN);
            DirectoryEntry userEntry = null;
            var description = "";
            try
            {
                searcher.Filter = string.Format("(&(cn=SP*)(objectclass=posixGroup))");
                SearchResultCollection searchResults = searcher.FindAll();

                if (searchResults != null)
                {
                    groups = new List<SiteInformation>();
                    for (int i = 0; i < searchResults.Count; i++)
                    {
                        description = searchResults[i].Properties["cn"][0].ToString().ToLower();
                        groups.Add(new SiteInformation()
                        {
                            AbbreviateName = searchResults[i].Properties["shortName"].Count > 0 ? searchResults[i].Properties["shortName"][0].ToString().ToUpper() : "",
                            Description = searchResults[i].Properties["description"].Count > 0 ? searchResults[i].Properties["description"][0].ToString().ToLower() : "",
                            SiteName = searchResults[i].Properties["name"].Count > 0 ? searchResults[i].Properties["name"][0].ToString().ToLower() : "",
                            Url = searchResults[i].Properties["url"].Count > 0 ? searchResults[i].Properties["url"][0].ToString().ToLower() : "",
                            SiteType = description.Contains("spr") ? "Sitio Privado" : "Sitio Público",
                            Cn = description,
                            Priority = searchResults[i].Properties["gidNumber"].Count > 0 ? ConvertPropertyToInt(searchResults[i].Properties["gidNumber"][0]) : 99999
                        });
                    }
                }

                return groups;
            }
            catch (COMException ex)
            {
                throw ex;
            }
            finally
            {
                if (searcher != null) searcher.Dispose();
                if (userEntry != null) userEntry.Dispose();
            }
        }

        /// <summary>
        /// Converts an LDAP property to int
        /// </summary>
        /// <returns>The converted value</returns>
        private int ConvertPropertyToInt(object value)
        {
            if (value.GetType() == typeof(byte[]))
            {
                value = Encoding.UTF8.GetString((byte[])value);
            }

            return int.Parse(value.ToString());
        }

        /// <summary>
        /// Builds a UserInfo instance from the PropertyCollection property of a DirectoryService entry using Reflection.
        /// </summary>
        /// <param name="userEntry">The directory entry to extract the properties from</param>
        /// <returns></returns>
        private UserInfo BuildUserFromDirectoryEntry(DirectoryEntry userEntry)
        {
            UserInfo userInfo = new UserInfo();

            foreach (string propertyName in userEntry.Properties.PropertyNames)
            {
                string modelPropName = null;

                if (ldapUserModelMapper.TryGetValue(propertyName.ToLower(), out modelPropName))
                {
                    PropertyInfo prop = typeof(UserInfo).GetProperty(modelPropName);
                    prop.SetValue(userInfo, userEntry.InvokeGet(propertyName));
                }
            }

            return userInfo;
        }

        /// <summary>
        /// Builds a UserInfo instance from a SearchResultEntry using Reflection.
        /// </summary>
        /// <param name="userEntry">The directory entry to extract the properties from</param>
        /// <returns></returns>
        private UserInfo BuildUserFromDirectoryEntryV2(SearchResultEntry userEntry)
        {
            UserInfo userInfo = new UserInfo();

            foreach (string propertyName in userEntry.Attributes.AttributeNames)
            {
                string modelPropName = null;

                if (ldapUserModelMapper.TryGetValue(propertyName.ToLower(), out modelPropName))
                {
                    PropertyInfo prop = typeof(UserInfo).GetProperty(modelPropName);
                    prop.SetValue(userInfo, userEntry.Attributes[propertyName][0]);
                }
            }

            return userInfo;
        }
    }
}