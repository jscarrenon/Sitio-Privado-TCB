using Sitio_Privado.Services.Interfaces;
using System;
using System.Runtime.InteropServices;
using System.DirectoryServices;
using System.Configuration;
using System.Collections.Generic;
using Sitio_Privado.Models;
using System.Reflection;

namespace Sitio_Privado.Services.ExternalUserProvider
{
    public class LDAPService : IExternalUserService
    {
        private const int invalidCredentialsErrorCode = -2147023570;

        private static readonly string domain = ConfigurationManager.AppSettings["LDAPDomain"];
        private static readonly string usersBaseDN = ConfigurationManager.AppSettings["LDAPUsersBaseDN"];
        private static readonly string groupsBaseDN = ConfigurationManager.AppSettings["LDAPGroupsBaseDN"];

        private const AuthenticationTypes normalAuthenticationTypes = AuthenticationTypes.None;

        // This variable stores the mapping between the LDAP user properties and the UserInfo model.
        private Dictionary<string, string> ldapUserModelMapper;
      
        public LDAPService()
        {
            SetUserSchemaEntries();
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

        public UserInfo GetUserInfoByUsername(string username)
        {
            DirectorySearcher searcher = InitializeSearcher(usersBaseDN);
            UserInfo userInfo = null;
            DirectoryEntry userEntry = null;

            try
            {
                searcher.Filter = String.Format("cn={0}", username);
                SearchResult searchResult = searcher.FindOne();

                if (searchResult != null)
                {
                    userEntry = searchResult.GetDirectoryEntry();
                    userInfo = BuildUserFromDirectoryEntry(userEntry);
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

            return userInfo;
        }
        /// <summary>
        /// Initializes a new instance of the DirectorySearcher class, pointing to differents base DN based on the user type given.
        /// </summary>
        /// <returns>The initialized DirectorySearcher instance</returns>
        private static DirectorySearcher InitializeSearcher(string baseDN)
        {
            DirectoryEntry searchRoot = null;
            DirectorySearcher searcher = null;

            searchRoot = new DirectoryEntry(String.Format("LDAP://{0}/{1}", domain, baseDN),
                    ConfigurationManager.AppSettings["LDAPAdminUsername"],
                    ConfigurationManager.AppSettings["LDAPAdminPassword"],
                    normalAuthenticationTypes);

            searcher = new DirectorySearcher(searchRoot);
            searcher.SearchScope = SearchScope.OneLevel;
            searcher.CacheResults = false;

            return searcher;
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
                if (ldapUserModelMapper.TryGetValue(propertyName, out modelPropName))
                {
                    PropertyInfo prop = typeof(UserInfo).GetProperty(modelPropName);
                    prop.SetValue(userInfo, userEntry.Properties[propertyName].Value.ToString());
                }
            }

            return userInfo;
        }
        private void SetUserSchemaEntries()
        {
            this.ldapUserModelMapper = new Dictionary<string, string>
            {
                {"uid","Rut" },
                {"givenname","FirstName" },
                { "sn","LastName"},
                { "mail","Email"},
                { "cuenta_corriente","CheckingAccount"},
                { "banco","Bank"},
                { "fono_comercial","WorkPhone"},
                { "fono_particular","HomePhone"},
                { "domicilio_particular","HomeAddress"},
                { "domicilio_comercial","WorkAddress"},
                { "city","City"},
                { "countryCode","Country"}
            };
        }
    }
}