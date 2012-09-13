using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace BNSharp.BattleNet.Clans
{
    /// <summary>
    /// Specifies the ranks a clan member may have within a clan.
    /// </summary>
    [DataContract]
    public enum ClanRank
    {
        /// <summary>
        /// Specifies that the member is a new recruit who has been with the clan less than one week.
        /// </summary>
        [EnumMember]
        Initiate = 0, 
        /// <summary>
        /// Specifies that the member is a new recruit who has been with the clan at least one week.
        /// </summary>
        [EnumMember]
        Peon = 1,
        /// <summary>
        /// Specifies that the member is a regular clan member.
        /// </summary>
        [EnumMember]
        Grunt = 2,
        /// <summary>
        /// Specifies that the member is a clan officer.
        /// </summary>
        [EnumMember]
        Shaman = 3,
        /// <summary>
        /// Specifies that the member is the clan leader.
        /// </summary>
        [EnumMember]
        Chieftan = 4,
    }
}
