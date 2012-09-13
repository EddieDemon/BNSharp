using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace BNSharp.BattleNet.Clans
{
    /// <summary>
    /// Specifies the status codes associated with changing a clan member's rank.
    /// </summary>
    [DataContract]
    public enum ClanRankChangeStatus : byte
    {
        /// <summary>
        /// Indicates a success.
        /// </summary>
        [EnumMember]
        Success = 0,
        /// <summary>
        /// Indicates a general failure.
        /// </summary>
        [EnumMember]
        Failed = 1,
        /// <summary>
        /// Indicates that the user is too new to be moved from Initiate status.
        /// </summary>
        [EnumMember]
        UserIsTooNew = 2,
        /// <summary>
        /// Indicates that the user requesting the change is not an officer.
        /// </summary>
        [EnumMember]
        NotAnOfficer = 7,
        /// <summary>
        /// Indicates that the user being changed is higher than the user requesting the change.
        /// </summary>
        [EnumMember]
        TargetIsTooHigh = 8,
    }
}
