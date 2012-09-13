using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace BNSharp.BattleNet.Clans
{
    /// <summary>
    /// Specifies the types of responses when attempting to invite a user to join a clan.
    /// </summary>
    [DataContract]
    public enum ClanInvitationResponse 
    {
        /// <summary>
        /// Indicates that the user accepted the invitation.
        /// </summary>
        [EnumMember]
        Accepted = 0,
        /// <summary>
        /// Indicates that the user declined the invitation.
        /// </summary>
        [EnumMember]
        Declined = 4,
        /// <summary>
        /// Indicates that Battle.net was unable to invite the user (for instance, the user was not online).
        /// </summary>
        [EnumMember]
        FailedToInvite = 5,
        /// <summary>
        /// Indicates that the clan is full.
        /// </summary>
        [EnumMember]
        ClanFull = 9,
    }
}
