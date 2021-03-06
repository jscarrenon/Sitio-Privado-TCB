﻿using Sitio_Privado.Models;
using System.Collections.Generic;

namespace Sitio_Privado.Services.Interfaces
{
    public interface IExternalUserService
    {
        /// <summary>
        /// Gets the user information from external user provider from the user's username
        /// </summary>
        /// <param name="username">The user's username to look for</param>
        /// <returns>An instance of the Person model. Null if the user is not found.</returns>
        Usuario GetUserInfoByUsername(string username);

        /// <summary>
        /// Gets the user information from external user provider from the user's username with a different implementation
        /// </summary>
        /// <param name="username">The user's username to look for</param>
        /// <returns>An instance of the Person model. Null if the user is not found.</returns>
        Usuario GetUserInfoByUsernameV2(string username);

        /// <summary>
        /// Gets the groups of the given user
        /// </summary>
        /// <param name="username">The username of the user to get the groups</param>
        List<SiteInformation> GetAllSites();
    }
}