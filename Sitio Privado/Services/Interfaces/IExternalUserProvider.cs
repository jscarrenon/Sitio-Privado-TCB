using Sitio_Privado.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitio_Privado.Services.Interfaces
{
    public interface IExternalUserService
    {
        /// <summary>
        /// Authenticates a user against the external user provider.
        /// </summary>
        /// <param name="username">The user identifier. Can be a RUT or an email.</param>
        /// <param name="password">The user password</param>
        /// <returns>True if authentication passes. False otherwise.</returns>
        bool Authenticate(string username, string password);

        /// <summary>
        /// Gets the user information from external user provider from the user's username
        /// </summary>
        /// <param name="username">The user's username to look for</param>
        /// <returns>An instance of the UserInfo model. Null if the user is not found.</returns>
        UserInfo GetUserInfoByUsername(string username);
      
    }
}