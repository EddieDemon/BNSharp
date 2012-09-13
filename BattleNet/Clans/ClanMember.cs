using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace BNSharp.BattleNet.Clans
{
    /// <summary>
    /// Represents a Battle.net Clan member.
    /// </summary>
    /// <remarks>
    /// <para>This class cannot be directly instantiated.  Rather, it is provided when you log on via the <see>TODO</see> event.</para>
    /// </remarks>
    [Serializable]
    [DataContract]
    public class ClanMember
    {
        #region fields
        [DataMember(Name = "Username")]
        private string m_userName;
        [DataMember(Name = "Rank")]
        private ClanRank m_rank;
        [DataMember(Name = "OnlineStatus")]
        private ClanMemberStatus m_online;
        [DataMember(Name = "Location")]
        private string m_location;
        #endregion

        #region constructors
        /// <summary>
        /// Creates a new <see>ClanMember</see> for a user who is currently offline.
        /// </summary>
        /// <param name="userName">The clan member's user name.</param>
        /// <param name="rank">The clan member's rank.</param>
        public ClanMember(string userName, ClanRank rank)
        {
            m_userName = userName;
            m_rank = rank;
            m_location = string.Empty;
        }

        /// <summary>
        /// Creates a new <see>ClanMember</see> for a user who is online or offline.
        /// </summary>
        /// <param name="userName">The clan member's user name.</param>
        /// <param name="rank">The clan member's rank.</param>
        /// <param name="status">The clan member's current status.</param>
        /// <param name="location">The name of the member's current location (such as a channel or game name).</param>
        public ClanMember(string userName, ClanRank rank, ClanMemberStatus status, string location)
            : this(userName, rank)
        {
            m_online = status;
            m_location = location;
        }
        #endregion

        #region properties
        /// <summary>
        /// Gets the user's name.
        /// </summary>
        /// <remarks>
        /// <para>When exposed under a WCF data contract, this property's backing store is given the name <c>Username</c>.</para>
        /// </remarks>
        public string Username
        {
            get { return m_userName; }
        }

        /// <summary>
        /// Gets, and in derived classes sets, the user's current <see cref="ClanRank">rank</see> within the clan.
        /// </summary>
        /// <remarks>
        /// <para>When exposed under a WCF data contract, this property's backing store is given the name <c>Rank</c>.</para>
        /// </remarks>
        public ClanRank Rank
        {
            get { return m_rank; }
            protected internal set
            {
                m_rank = value;
            } 
        }

        /// <summary>
        /// Gets, and in derived classes sets, the current location and status of the clan member.
        /// </summary>
        /// <remarks>
        /// <para>When exposed under a WCF data contract, this property's backing store is given the name <c>OnlineStatus</c>.</para>
        /// </remarks>
        public ClanMemberStatus CurrentStatus
        {
            get { return m_online; }
            protected internal set
            {
                m_online = value;
            }
        }

        /// <summary>
        /// Gets, and in derived classes sets, the user's current Battle.net location, if the user is online.
        /// </summary>
        /// <remarks>
        /// <para>This property will return <see langword="null" /> if the user is not online.</para>
        /// <para>When exposed under a WCF data contract, this property's backing store is given the name <c>Location</c>.</para>
        /// </remarks>
        public string Location
        {
            get { return m_location; }
            protected internal set
            {
                m_location = value;
            } 
        }
        #endregion
    }
}
