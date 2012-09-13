using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace BNSharp.BattleNet
{
    /// <summary>
    /// Specifies the difficulty level for Diablo II games and characters.
    /// </summary>
    [DataContract]
    public enum Diablo2DifficultyLevel
    {
        /// <summary>
        /// Specifies that the difficulty level is unrecognized.
        /// </summary>
        [EnumMember]
        Unknown = 0,
        /// <summary>
        /// Specifies the Normal difficulty level.
        /// </summary>
        [EnumMember]
        Normal = 1,
        /// <summary>
        /// Specifies the Nightmare difficulty level.
        /// </summary>
        [EnumMember]
        Nightmare = 2,
        /// <summary>
        /// Specifies the Hell difficulty level.
        /// </summary>
        [EnumMember]
        Hell = 3,
    }
}
