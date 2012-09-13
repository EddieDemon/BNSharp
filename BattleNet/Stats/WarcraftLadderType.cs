using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace BNSharp.BattleNet.Stats
{
    /// <summary>
    /// Specifies the ladder types tracked by Warcraft III ladder stats.
    /// </summary>
    [DataContract]
    public enum WarcraftLadderType
    {
        /// <summary>
        /// Specifies free-for-all ladder games.
        /// </summary>
        [EnumMember]
        FreeForAll = 0x46464120,
        /// <summary>
        /// Specifies solo ladder games.
        /// </summary>
        [EnumMember]
        Solo = 0x534f4c4f,
        /// <summary>
        /// Specifies random-team ladder games.
        /// </summary>
        [EnumMember]
        Team = 0x5445414d,
    }
}
