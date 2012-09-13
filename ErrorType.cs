using System;
using System.Collections.Generic;
using System.Text;

namespace BNSharp
{
    /// <summary>
    /// Indicates the causes of errors that are provided through BN#.
    /// </summary>
    public enum ErrorType
    {
        /// <summary>
        /// No error occurred.
        /// </summary>
        None = 0,
        /// <summary>
        /// No additional error cause is available.
        /// </summary>
        General = 1,
        /// <summary>
        /// Indicates that the server was unable to provide evidence that it is really the server to which the client is connected.  Connection
        /// will continue, but you may be connecting to an illegitimate server, or someone may be attempting to impersonate your client.
        /// </summary>
        Warcraft3ServerValidationFailure,
        /// <summary>
        /// Indicates that the designated Warden module failed to initialize.
        /// </summary>
        WardenModuleFailure,
        /// <summary>
        /// Indicates that Battle.net requested an account upgrade, but this functionality is not available in BN#.
        /// </summary>
        AccountUpgradeUnsupported,
        /// <summary>
        /// Indicates that the client provided an invalid username or password.
        /// </summary>
        InvalidUsernameOrPassword,
        /// <summary>
        /// Indicates that the server was unable to provide evidence that it knew the client password.  Connection will continue,
        /// but you may be connecting to an illegitimate server, or someone may be attempting to impersonate your client.
        /// </summary>
        LoginServerProofFailed,
    }
}
