using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace BNSharp
{
    /// <summary>
    /// Specifies the known reasons for a login failure to have occurred.
    /// </summary>
    [DataContract]
    public enum LoginFailureReason
    {
        /// <summary>
        /// Specifies that an unknown reason caused the login failure.
        /// </summary>
        [EnumMember]
        Unknown,
        /// <summary>
        /// Specifies that the selected account does not exist.  This status is only available when the 
        /// <see cref="LoginFailedEventArgs.ProvidesExtendedInformation">ProvidesExtendedInformation</see> property of the login 
        /// failure arguments is <see langword="true" />.
        /// </summary>
        [EnumMember]
        AccountDoesNotExist,
        /// <summary>
        /// Specifies that either the account was not found, or that the password was invalid.  For extended information check the 
        /// <see cref="LoginFailedEventArgs.ProvidesExtendedInformation">ProvidesExtendedInformation</see> property of the login 
        /// failure arguments.
        /// </summary>
        [EnumMember]
        InvalidAccountOrPassword,
        /// <summary>
        /// Specifies that the account has been closed.  This status is only available when the 
        /// <see cref="LoginFailedEventArgs.ProvidesExtendedInformation">ProvidesExtendedInformation</see> property of the login 
        /// failure arguments is <see langword="true" />.
        /// </summary>
        [EnumMember]
        AccountClosed,
    }
}
