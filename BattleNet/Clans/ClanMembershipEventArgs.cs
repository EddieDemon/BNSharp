using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace BNSharp.BattleNet.Clans
{
    /// <summary>
    /// Provides information about the client's user's current clan during login.
    /// </summary>
    [DataContract]
    public class ClanMembershipEventArgs : BaseEventArgs
    {
        #region fields
        [DataMember(Name = "Rank")]
        private ClanRank m_rank;
        [DataMember(Name = "Tag")]
        private string m_tag;
        #endregion

        /// <summary>
        /// Creates a new instance of <see>ClanMembershipEventArgs</see>.
        /// </summary>
        /// <param name="clanTag">The tag of the clan to which the client belongs.</param>
        /// <param name="rank">The rank within the clan of the client.</param>
        public ClanMembershipEventArgs(string clanTag, ClanRank rank)
        {
            m_tag = clanTag;
            m_rank = rank;
        }

        /// <summary>
        /// Gets your current rank within the clan.
        /// </summary>
        /// <remarks>
        /// <para>When exposed under a WCF data contract, this property's backing store is given the name <c>Rank</c>.</para>
        /// </remarks>
        public ClanRank Rank
        {
            get { return m_rank; }
        }

        /// <summary>
        /// Gets the tag of the clan to which you belong.
        /// </summary>
        /// <remarks>
        /// <para>When exposed under a WCF data contract, this property's backing store is given the name <c>Tag</c>.</para>
        /// </remarks>
        public string Tag
        {
            get { return m_tag; }
        }
    }

    /// <summary>
    /// Specifies the contract for listeners that want to observe the clan membership event.
    /// </summary>
    /// <param name="sender">The <see>BattleNetClient</see> that originated the event.</param>
    /// <param name="e">The event arguments.</param>
    public delegate void ClanMembershipEventHandler(object sender, ClanMembershipEventArgs e);
}
