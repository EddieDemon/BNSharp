using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;

namespace BNSharp.BattleNet.Clans
{
    /// <summary>
    /// The event arguments for the clan member list notification.
    /// </summary>
    [Serializable]
    [DataContract]
    public class ClanMemberListEventArgs : BaseEventArgs
    {
        #region fields
        [DataMember(Name = "Members")]
        private ClanMember[] m_members;
        #endregion

        /// <summary>
        /// Creates a new instance of <see>ClanMemberListEventArgs</see>.
        /// </summary>
        /// <param name="members">The clan members in the list.</param>
        public ClanMemberListEventArgs(ClanMember[] members)
        {
            m_members = members;
        }

        /// <summary>
        /// Gets the list of members received from Battle.net.
        /// </summary>
        /// <remarks>
        /// <para>When exposed under a WCF data contract, this property's backing store is given the name <c>Members</c>.</para>
        /// </remarks>
        public ReadOnlyCollection<ClanMember> Members
        {
            get { return new ReadOnlyCollection<ClanMember>(m_members); }
        }
    }

    /// <summary>
    /// Specifies the contract for clan member list events.
    /// </summary>
    /// <param name="sender">The <see>BattleNetClient</see> connection that originated the event.</param>
    /// <param name="e">The event arguments.</param>
    public delegate void ClanMemberListEventHandler(object sender, ClanMemberListEventArgs e);
}
