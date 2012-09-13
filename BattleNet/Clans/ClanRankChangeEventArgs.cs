using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace BNSharp.BattleNet.Clans
{
    /// <summary>
    /// Represents the result of an attempt to change a user's clan rank.
    /// </summary>
#if !NET_2_ONLY
    [DataContract]
#endif
    public class ClanRankChangeEventArgs : BaseEventArgs
    {
        [DataMember(Name = "MemberName")]
        private string m_memberName;
        [DataMember(Name = "Status")]
        private ClanRankChangeStatus m_status;

        /// <summary>
        /// Creates a new <see>ClanRankChangeEventArgs</see>.
        /// </summary>
        /// <param name="memberName">The name of the target user.</param>
        /// <param name="status">The result of the change request.</param>
        public ClanRankChangeEventArgs(string memberName, ClanRankChangeStatus status)
        {
            m_memberName = memberName;
            m_status = status;
        }

        /// <summary>
        /// Gets the name of the user whose rank was requested to be changed.
        /// </summary>
        public string MemberName
        {
            get { return m_memberName; }
        }

        /// <summary>
        /// Gets the status of the change request.
        /// </summary>
        public ClanRankChangeStatus Status
        {
            get { return m_status; }
        }
    }

    /// <summary>
    /// Specifies the contract for handlers wishing to listen for clan rank change response events.
    /// </summary>
    /// <param name="sender">The object that originated the event.</param>
    /// <param name="e">The event arguments.</param>
    public delegate void ClanRankChangeEventHandler(object sender, ClanRankChangeEventArgs e);
}
