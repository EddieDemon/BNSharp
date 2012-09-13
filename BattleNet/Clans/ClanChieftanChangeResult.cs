using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace BNSharp.BattleNet.Clans
{
    /// <summary>
    /// Specifies result codes for an attempt to change the clan chieftan (leader).
    /// </summary>
    [DataContract]
    public enum ClanChieftanChangeResult
    {
        /// <summary>
        /// Indicates that the change succeeded.
        /// </summary>
        [EnumMember]
        Success = 0,
        /// <summary>
        /// Indicates that the clan is less than a week old.
        /// </summary>
        [EnumMember]
        ClanTooYoung = 2,
        /// <summary>
        /// Indicates that the designee did not accept becoming chieftan.
        /// </summary>
        [EnumMember]
        Declined = 4,
        /// <summary>
        /// Indicates that the request has failed.
        /// </summary>
        [EnumMember]
        Failed = 5, 
        /// <summary>
        /// Indicates that the client is not the present clan chieftan.
        /// </summary>
        [EnumMember]
        NotAuthorized = 7,
        /// <summary>
        /// Indicates that the user does not exist, is not in the clan, or that the user has not been in the clan long enough.
        /// </summary>
        [EnumMember]
        NoSuchUser = 8,
    }
}
