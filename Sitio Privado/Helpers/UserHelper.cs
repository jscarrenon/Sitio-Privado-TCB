using Sitio_Privado.Infraestructure.Constants;
using Sitio_Privado.Models;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace Sitio_Privado.Helpers
{
    /// <summary>
    /// Helper to class allow easy ClaimsPrincipal manipulation
    /// </summary>
    public static class UserHelper
    {
        /// <summary>
        /// Extracts the roles of a user of the specified group.
        /// </summary>
        /// <param name="user">The user to extract the roles from.</param>
        /// <param name="targetGroup">The target group.</param>
        /// <returns>A collection of roles.</returns>
        public static IEnumerable<string> ExtractRolesFromGroup(ClaimsPrincipal user, string targetGroup)
        {
            return user.FindAll(ApplicationConstants.GroupsClaimName)                       // Gets all group-role pairs
                        .Where(e => e.Value.Split('_').First() == targetGroup)              // Selects only the pairs with group equal to the target group
                        .Select(e => e.Value.Split('_').Skip(1).Take(1).FirstOrDefault())   // Selects the roles from the pairs
                        .Where(e => e != null)                                              // Removes null values
                        .Distinct();                                                        // Removes duplicates
        }

        /// <summary>
        /// Extracts the groups of a user.
        /// </summary>
        /// <param name="user">The user to extract the roles from.</param>
        /// <returns>A collection of groups.</returns>
        public static IEnumerable<string> ExtractGroups(ClaimsPrincipal user)
        {
            return user.FindAll(ApplicationConstants.GroupsClaimName)                       // Gets all group-role pairs
                        .Select(e => e.Value.Split('_').Take(1).FirstOrDefault())           // Selects the groups from the pairs
                        .Where(e => e != null)                                              // Removes null values
                        .Distinct();                                                        // Removes duplicates
        }

        /// <summary>
        /// Extracts the scopes of a user.
        /// </summary>
        /// <param name="user">The user to extract the scopes from.</param>
        /// <returns>A collection of groups.</returns>
        public static IEnumerable<string> ExtractScopes(ClaimsPrincipal user)
        {
            return user.FindAll(ApplicationConstants.ScopesClaimName)                       // Gets all the subject claims matching the claim to look for
                        .Select(e => e.Value);                                              // Selects the scope value
        }

        /// <summary>
        /// Extracts the authority id of a user.
        /// </summary>
        /// <param name="user">The user to extract the id from.</param>
        /// <returns>The authority id.</returns>
        public static string ExtractAuthorityId(ClaimsPrincipal user)
        {
            return user.FindAll(ApplicationConstants.SubjectClaimName)                      // Gets all the subject claims matching the claim to look for
                        .Select(e => e.Value)                                               // Selects the scope value
                        .FirstOrDefault();                                                  // Returns the first ocurrence
        }

        /// <summary>
        /// Extracts the person id of a user.
        /// </summary>
        /// <param name="user">The user to extract the id from.</param>
        /// <returns>The person id.</returns>
        public static int ExtractPersonId(ClaimsPrincipal user)
        {
            string id = user.FindAll(ApplicationConstants.PersonIdClaimName)                // Gets all the subject claims matching the claim to look for
                        .Select(e => e.Value)                                               // Selects the scope value
                        .FirstOrDefault();                                                  // Returns the first ocurrence

            return int.Parse(id);
        }

        /// <summary>
        /// Extracts the person type of a user.
        /// </summary>
        /// <param name="user">The user to extract the type from.</param>
        /// <returns>The person id.</returns>
        public static string ExtractPersonType(ClaimsPrincipal user)
        {
            return user.FindAll(ApplicationConstants.PersonTypeClaimName)                // Gets all the subject claims matching the claim to look for
                        .Select(e => e.Value)                                               // Selects the scope value
                        .FirstOrDefault();                                                  // Returns the first ocurrence
        }

        /// <summary>
        /// Attaches person information in a ClaimsPrincipal instance as claims.
        /// </summary>
        /// <param name="person">The person information to be attached.</param>
        /// <param name="user">The ClaimsPrincipal instance to attach person information.</param>
        public static void AttachPersonAsUserClaims(Usuario person, ClaimsPrincipal user)
        {
            var identity = user.Identity as ClaimsIdentity;

            // TODO: revisar Id, que es propiedad "sub" 
            identity.AddClaim(new Claim(ApplicationConstants.PersonIdClaimName, person.Email.ToString()));
            identity.AddClaim(new Claim(ApplicationConstants.PersonTypeClaimName, person.GetType().ToString()));
        }

        /// <summary>
        /// Builds a person instance from the claims the user has.
        /// </summary>
        /// <param name="principal">The claims that the user has.</param>
        /// <returns>A person instance built with the claims that the user has.</returns>
        public static Usuario BuildPersonFromClaims(IPrincipal principal)
        {
            ClaimsPrincipal claimsPrincipal = principal as ClaimsPrincipal;
            Usuario person = new Usuario();
            //Usuario person = new Usuario
            //{
            //    Id = ExtractPersonId(claimsPrincipal),
            //    AuthorityId = ExtractAuthorityId(claimsPrincipal),
            //    Type = ExtractPersonType(claimsPrincipal),
            //    Roles = ExtractRolesFromGroup(claimsPrincipal, ApplicationConstants.RequiredGroupName)
            //};

            return person;
        }
    }
}