using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace BNSharp.BattleNet.Friends
{
    /// <summary>
    /// Specifies additional information about a person on your friend list.
    /// </summary>
    [Flags]
    [DataContract]
    public enum FriendStatus
    {
        /// <summary>
        /// No additional information is provided.
        /// </summary>
        [EnumMember]
        None = 0,
        /// <summary>
        /// The user also listed you as a friend.
        /// </summary>
        [EnumMember]
        Mutual = 1,
        /// <summary>
        /// The user has flagged themselves as do-not-disturb.
        /// </summary>
        [EnumMember]
        DoNotDisturb = 2,
        /// <summary>
        /// The user is away.
        /// </summary>
        [EnumMember]
        Away = 4,
    }
}
