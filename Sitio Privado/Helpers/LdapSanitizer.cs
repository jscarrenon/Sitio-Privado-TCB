using System;

namespace Sitio_Privado.Helpers
{
    /// <summary>
    /// This class exposes methods to allow user input sanitazion for LDAP requests.
    /// Obtained from http://projects.webappsec.org/w/page/13246947/LDAP%20Injection
    /// </summary>
    public class LdapSanitizer
    {
        /// <summary>
        /// Characters that must be escaped in an LDAP filter path
        /// WARNING: Always keep '\\' at the very beginning to avoid recursive replacements
        /// </summary>
        private static char[] ldapFilterEscapeSequence = new char[] { '\\', '*', '(', ')', '\0', '/' };

        /// <summary>
        /// Mapping strings of the LDAP filter escape sequence characters
        /// </summary>
        private static string[] ldapFilterEscapeSequenceCharacter = new string[] { "\\5c", "\\2a", "\\28", "\\29", "\\00", "\\2f" };

        /// <summary>
        /// Characters that must be escaped in an LDAP DN path
        /// </summary>
        private static char[] ldapDnEscapeSequence = new char[] { '\\', ',', '+', '"', '<', '>', ';' };

        /// <summary>
        /// Canonicalize a ldap filter string by inserting LDAP escape sequences.
        /// </summary>
        /// <param name="userInput">User input string to canonicalize</param>
        /// <returns>Canonicalized user input so it can be used in LDAP filter</returns>
        public static string CanonicalizeStringForLdapFilter(string userInput)
        {
            if (String.IsNullOrEmpty(userInput))
            {
                return userInput;
            }

            string name = (string)userInput.Clone();

            for (int charIndex = 0; charIndex < ldapFilterEscapeSequence.Length; ++charIndex)
            {
                int index = name.IndexOf(ldapFilterEscapeSequence[charIndex]);
                if (index != -1)
                {
                    name = name.Replace(new String(ldapFilterEscapeSequence[charIndex], 1), ldapFilterEscapeSequenceCharacter[charIndex]);
                }
            }

            return name;
        }

        /// <summary>
        /// Canonicalize a ldap dn string by inserting LDAP escape sequences.
        /// </summary>
        /// <param name="userInput">User input string to canonicalize</param>
        /// <returns>Canonicalized user input so it can be used in LDAP filter</returns>
        public static string CanonicalizeStringForLdapDN(string userInput)
        {
            if (String.IsNullOrEmpty(userInput))
            {
                return userInput;
            }

            string name = (string)userInput.Clone();

            for (int charIndex = 0; charIndex < ldapDnEscapeSequence.Length; ++charIndex)
            {
                int index = name.IndexOf(ldapDnEscapeSequence[charIndex]);
                if (index != -1)
                {
                    name = name.Replace(new string(ldapDnEscapeSequence[charIndex], 1), @"\" + ldapDnEscapeSequence[charIndex]);
                }
            }

            return name;
        }

        /// <summary>
        /// Ensure that a user provided string can be plugged into an LDAP search filter 
        /// such that there is no risk of an LDAP injection attack.
        /// </summary>
        /// <param name="userInput">String value to check.</param>
        /// <returns>True if value is valid or null, false otherwise.</returns>
        public static bool IsUserGivenStringPluggableIntoLdapSearchFilter(string userInput)
        {
            if (string.IsNullOrEmpty(userInput))
            {
                return true;
            }

            if (userInput.IndexOfAny(ldapDnEscapeSequence) != -1)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Ensure that a user provided string can be plugged into an LDAP DN 
        /// such that there is no risk of an LDAP injection attack.
        /// </summary>
        /// <param name="userInput">String value to check.</param>
        /// <returns>True if value is valid or null, false otherwise.</returns>
        public static bool IsUserGivenStringPluggableIntoLdapDN(string userInput)
        {
            if (string.IsNullOrEmpty(userInput))
            {
                return true;
            }

            if (userInput.IndexOfAny(ldapFilterEscapeSequence) != -1)
            {
                return false;
            }

            return true;
        }
    }
}