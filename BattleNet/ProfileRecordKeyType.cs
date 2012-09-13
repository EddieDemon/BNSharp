using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace BNSharp.BattleNet
{
    /// <summary>
    /// Specifies the type of profile record keys that can be requested.
    /// </summary>
    [DataContract]
    public enum ProfileRecordKeyType
    {
        /// <summary>
        /// Specifies Normal.  This is valid on Starcraft, Diablo, Warcraft II: Battle.net Edition, and Warcraft III.
        /// </summary>
        [EnumMember]
        Normal,
        /// <summary>
        /// Specifies Ladder.  This is valid on Starcraft, Diablo, and Warcraft II: Battle.net Edition.
        /// </summary>
        [EnumMember]
        Ladder,
        /// <summary>
        /// Specifies an Iron Man Ladder.  This is only valid on Warcraft II: Battle.net Edition.
        /// </summary>
        [EnumMember]
        IronManLadder,
    }
}
