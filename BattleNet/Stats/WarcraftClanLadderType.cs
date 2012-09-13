using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace BNSharp.BattleNet.Stats
{
    /// <summary>
    /// Specifies the types of ladder rankings for clans.
    /// </summary>
    [DataContract]
    public enum WarcraftClanLadderType
    {
        /// <summary>
        /// Specifies a clan solo match type.
        /// </summary>
        [EnumMember]
        Solo = 0x434c4e53,
        /// <summary>
        /// Specifies a clan two-vs-two match type.
        /// </summary>
        [EnumMember]
        TwoVsTwo = 0x434c4e32,
        /// <summary>
        /// Specifies a clan three-vs-three match type.
        /// </summary>
        [EnumMember]
        ThreeVsThree = 0x434c4e33,
        /// <summary>
        /// Specifies a clan four-vs-four match type.
        /// </summary>
        [EnumMember]
        FourVsFour = 0x434c4e34,        
    }
}
