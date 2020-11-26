using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE1_MTCG.Enums
{
    /// <summary>
    /// Describes what type of authentication the webServer should use.
    /// </summary>
    public enum AuthenticationType
    {
        /// <summary>
        /// Resolves to username-mtcgToken
        /// </summary>
        Simple,
        /// <summary>
        /// Resolves to session-Guid
        /// </summary>
        Basic
    }
}
