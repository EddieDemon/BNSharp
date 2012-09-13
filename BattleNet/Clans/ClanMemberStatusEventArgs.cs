using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace BNSharp.BattleNet.Clans
{
    /// <summary>
    /// Specifies the event arguments for a clan member's status change event.
    /// </summary>
    [DataContract]
    public class ClanMemberStatusEventArgs : BaseEventArgs
    {
        [DataMember(Name = "Member")]
        private ClanMember m_member;

        /// <summary>
        /// Creates a new <see>ClanMemberStatusEventArgs</see>.
        /// </summary>
        /// <param name="associatedMember">The members whose status has changed.</param>
        public ClanMemberStatusEventArgs(ClanMember associatedMember)
        {
            m_member = associatedMember;
        }

        /// <summary>
        /// Gets the associated clan member.
        /// </summary>
        /// <remarks>
        /// <para>When exposed under a WCF data contract, this property's backing store is given the name <c>Member</c>.</para>
        /// </remarks>
        public ClanMember Member
        {
            get { return m_member; }
        }
    }

    /// <summary>
    /// Specifies the contract for handlers of clan member status events.
    /// </summary>
    /// <param name="sender">The <see>BattleNetClient</see> that originated the event.</param>
    /// <param name="e">The event arguments.</param>
    public delegate void ClanMemberStatusEventHandler(object sender, ClanMemberStatusEventArgs e);
}
