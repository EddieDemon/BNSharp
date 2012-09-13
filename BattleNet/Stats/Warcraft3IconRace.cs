using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace BNSharp.BattleNet.Stats
{
    /// <summary>
    /// The list of races that are supported by Warcraft III for Battle.net.
    /// </summary>
    [DataContract]
    public enum Warcraft3IconRace
    {
        /// <summary>
        /// Specifies that the icon race sent from Battle.net was unrecognized.
        /// </summary>
        [EnumMember]
        Unknown,
        /// <summary>
        /// Specifies that a user's icon is based on the random list.
        /// </summary>
        [EnumMember]
        Random,
        /// <summary>
        /// Specifies that a user's icon is based on the tournament list.
        /// </summary>
        [EnumMember]
        Tournament,
        /// <summary>
        /// Specifies that a user's icon is based on the human list.
        /// </summary>
        [EnumMember]
        Human,
        /// <summary>
        /// Specifies that a user's icon is based on the orc list.
        /// </summary>
        [EnumMember]
        Orc,
        /// <summary>
        /// Specifies that a user's icon is based on the night elf list.
        /// </summary>
        [EnumMember]
        NightElf,
        /// <summary>
        /// Specifies that a user's icon is based on the undead list.
        /// </summary>
        [EnumMember]
        Undead,
    }
}
