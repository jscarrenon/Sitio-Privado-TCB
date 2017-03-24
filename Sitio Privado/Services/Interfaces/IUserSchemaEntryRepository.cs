using Sitio_Privado.Models;
using System.Collections.Generic;

namespace Sitio_Privado.Services.Interfaces
{
    public interface IUserSchemaEntryRepository 
    {
        /// <summary>
        /// Creates a mapper dictionary with the external field name as the key and the property name as the value.
        /// </summary>
        /// <returns></returns>
        Dictionary<string, string> GetExternalFieldToPropetyNameDictionary();

        /// <summary>
        /// Gets all user schema entries and returns a dictionary using the property name as the key
        /// </summary>
        /// <returns></returns>
        Dictionary<string, UserSchemaEntry> GetAllByPropertyName();
    }
}