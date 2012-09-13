using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace BNSharp.BattleNet
{
    /// <summary>
    /// Specifies the character classes supported by Diablo 2 characters.
    /// </summary>
    [DataContract]
    public enum Diablo2CharacterClass
    {
        /// <summary>
        /// Specifies that the class is unknown or invalid.
        /// </summary>
        [EnumMember]
        Unknown = 0,
        /// <summary>
        /// Specifies the Amazon class (female).
        /// </summary>
        [EnumMember]
        Amazon = 1,
        /// <summary>
        /// Specifies the Sorceress class (female).
        /// </summary>
        [EnumMember]
        Sorceress = 2,
        /// <summary>
        /// Specifies the Necromancer class (male).
        /// </summary>
        [EnumMember]
        Necromancer = 3,
        /// <summary>
        /// Specifies the Paladin class (male).
        /// </summary>
        [EnumMember]
        Paladin = 4,
        /// <summary>
        /// Specifies the Barbarian class (male).
        /// </summary>
        [EnumMember]
        Barbarian = 5,
        /// <summary>
        /// Specifies the Druid class (male).
        /// </summary>
        [EnumMember]
        Druid = 6,
        /// <summary>
        /// Specifies the Assassin class (female).
        /// </summary>
        [EnumMember]
        Assassin = 7,
    }
}
