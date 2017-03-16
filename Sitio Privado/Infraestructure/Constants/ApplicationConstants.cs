using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitio_Privado.Infraestructure.Constants
{
    public static class ApplicationConstants
    {
        /// <summary>
        /// The short name of the aplication
        /// </summary>
        public const string AppShortName = "Corredora"; // TODO: nombre temporal
        /// <summary>
        /// The required group to access this application resources
        /// </summary>
        public const string RequiredGroupName = "corredora"; // TODO: nombre temporal
        /// <summary>
        /// The name of the subject claim taken from the access token
        /// </summary>
        public const string SubjectClaimName = "sub";

        /// <summary>
        /// The name of the groups claim taken from the access token
        /// </summary>
        public const string GroupsClaimName = "groups";

        /// <summary>
        /// The name of the scopes claim taken from the access token
        /// </summary>
        public const string ScopesClaimName = "scope";

        /// <summary>
        /// The name of the person id taken from local db
        /// </summary>
        public const string PersonIdClaimName = "personId";

        /// <summary>
        /// The type of the person id taken from local db
        /// </summary>
        public const string PersonTypeClaimName = "personType";

        /// <summary>
        /// Semantic version string pattern
        /// </summary>
        public const string SemVerRegexPattern = @"[\d].[\d].[\d]";

        /// <summary>
        /// Lenght of special identifiers used for embedded json objects
        /// </summary>
        public const int ObjectIdLength = 24;

        public const string MainDbContext = "StorageConnection";
    }
}