using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using BNSharp.Net;

namespace BNSharp.BattleNet.Clans
{
    /// <summary>
    /// Specifies the possible results when attempting to remove a clan member with <see cref="BattleNetClient.RemoveClanMember">RemoveClanMember</see>.
    /// </summary>
    [DataContract]
    public enum ClanMemberRemovalResponse
    {
        /// <summary>
        /// Specifies that the member was successfully removed.
        /// </summary>
        [EnumMember]
        Removed = 0,
        /// <summary>
        /// Specifies that Battle.net failed to remove the user from the clan.
        /// </summary>
        [EnumMember]
        RemovalFailed = 1,
        /// <summary>
        /// Specifies that the member is too new to be removed from the clan.
        /// </summary>
        [EnumMember]
        CannotRemoveNewMember = 2,
        /// <summary>
        /// Specifies that the client is not authorized to remove that member.
        /// </summary>
        [EnumMember]
        NotAuthorized = 7,
        /// <summary>
        /// Specifies that the user is not allowed to be removed.
        /// </summary>
        [EnumMember]
        NotAllowed = 8,
    }
}
