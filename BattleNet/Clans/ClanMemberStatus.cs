using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace BNSharp.BattleNet.Clans
{
    /// <summary>
    /// Specifies the current status of a clan member.
    /// </summary>
    [DataContract]
    public enum ClanMemberStatus
    {
        /// <summary>
        /// Specifies that the user is offline.
        /// </summary>
        [EnumMember]
        Offline = 0,
        /// <summary>
        /// Specifies that the user is online but not in a channel or a game.
        /// </summary>
        [EnumMember]
        Online = 1,
        /// <summary>
        /// Specifies that the user is in a channel.
        /// </summary>
        [EnumMember]
        InChannel = 2,
        /// <summary>
        /// Specifies that the user is in a public game.
        /// </summary>
        [EnumMember]
        InPublicGame = 3,
        /// <summary>
        /// Specifies that the user is in a private game.
        /// </summary>
        [EnumMember]
        InPrivateGame = 5,
    }
}
