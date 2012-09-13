using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace BNSharp.BattleNet.Clans
{
    /// <summary>
    /// Specifies the status of a clan candidates search.
    /// </summary>
    [DataContract]
    public enum ClanCandidatesSearchStatus
    {
        /// <summary>
        /// Indicates that the search was a success and that the tag is available.
        /// </summary>
        [EnumMember]
        Success,
        /// <summary>
        /// Indicates that the requested tag is already taken.
        /// </summary>
        [EnumMember]
        ClanTagTaken,
        /// <summary>
        /// Indicates that the client user is already in a clan.
        /// </summary>
        [EnumMember]
        AlreadyInClan,
        /// <summary>
        /// Specifies the tag requested was invalid.
        /// </summary>
        [EnumMember]
        InvalidTag,
    }
}
