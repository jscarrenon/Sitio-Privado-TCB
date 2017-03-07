using System.Linq;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Security.Claims;
using Sitio_Privado.Infraestructure.Constants;
using System.ComponentModel.Design;
using System.Runtime.CompilerServices;
using Sitio_Privado.Helpers;
using NLog;
using Sitio_Privado.Infraestructure.ExceptionHandling;
using Sitio_Privado.Services;

namespace Sitio_Privado.Filters
{
    public class AuthorizeWithGroupsAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// The required group to perform the required action
        /// </summary> 
        private static Logger logger = LogManager.GetLogger("SessionLog");
        public string RequiredGroup { get; }

        /// <summary>
        /// List of roles allowed to perform the required action
        /// </summary>
        public IEnumerable<string> AllowedRoles { get; }

        /// <summary>
        /// Whitespace separated scopes required to perform the required action
        /// </summary>
        public string RequiredScopes { get; set; }

        /// <summary>
        /// If true, the attribute checks if the user exists locally in the Person table
        /// </summary>
        public bool CheckLocalExistence { get; set; } = true;

        public HttpService httpService = null;

        /// <summary>
        /// Initializes a new instance of the AuthorizeWithGroupsAttribute class
        /// </summary>
        /// <param name="allowedRoles">List of roles allowed to perform the required action</param>
        public AuthorizeWithGroupsAttribute(params string[] allowedRoles) : base()
        {
            httpService = new HttpService();
            //RequiredGroup = ApplicationConstants.RequiredGroupName;
            //AllowedRoles = allowedRoles;
        }

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            if (base.IsAuthorized(actionContext))
            {
                ClaimsPrincipal user = actionContext.RequestContext.Principal as ClaimsPrincipal;
                var username = user.FindFirst(ApplicationConstants.SubjectClaimName).Value;

                List<string> userGroups = UserHelper.ExtractGroups(user).ToList();
                //bool hasGroup = userGroups.Contains(RequiredGroup);
                bool hasGroup = true;

                List<string> userRoles = UserHelper.ExtractRolesFromGroup(user, RequiredGroup).ToList();
                // If there are no required groups, true is assigned immediately, because Linq.Any returns false otherwise for that case
                //bool hasRoles = AllowedRoles.Count() == 0 || AllowedRoles.Any(e => userRoles.Contains(e));

                List<string> userScopes = UserHelper.ExtractScopes(user).ToList();
                bool hasScopes = GetRequiredScopesList().All(e => userScopes.Contains(e));


                // Checks if user exists locally only is the CheckLocalExistence flag is enabled
                bool existLocally = true;
                if (CheckLocalExistence)
                {
                    //IRepository repository = UnityConfiguration.GetConfiguredContainer().Resolve<IRepository>();
                    //var person = repository.GetSingle<Person>(p => p.AuthorityId == username);
                    //if (person == null)
                    //{
                       //existLocally = false; 
                    //}
                    //else
                    //{
                    //    // if the user does exist, some person attributes are added to the user claims
                    //UserHelper.AttachPersonAsUserClaims(person, user);
                    //}
                }

                if (!hasGroup)
                {
                    logger.Warn("User '{username}' does not meet groups requirements", username);
                }

                //if (!hasRoles)
                //{
                //    logger.Warn("User '{username}' does not meet roles requirements", username);
                //}

                if (!hasScopes)
                {
                    logger.Warn("User '{username}' does not meet scopes requirements", username);
                }

                if (!existLocally)
                {
                    logger.Warn("User '{username}' does not exist locally", username);
                }

                //return hasGroup && hasRoles && hasScopes && existLocally;
                return hasGroup && hasScopes && existLocally;
            }
            else
            {
                return false;
            }
        }

        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            if (actionContext.RequestContext.Principal != null && actionContext.RequestContext.Principal.Identity.IsAuthenticated)
            {
                actionContext.Response = httpService.GenerateErrorResponse(ApiErrorCode.GenericForbidden, actionContext.Request);
            }
            else
            {
                actionContext.Response = httpService.GenerateErrorResponse(ApiErrorCode.GenericUnauthorized, actionContext.Request);
            }
        }

        public IEnumerable<string> GetRequiredScopesList()
        {
            return (RequiredScopes != null) ? RequiredScopes.Split(' ').ToList() : new List<string>();
        }

    }
}