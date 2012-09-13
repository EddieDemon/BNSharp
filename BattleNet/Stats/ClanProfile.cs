using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using BNSharp.BattleNet.Clans;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace BNSharp.BattleNet.Stats
{
    /// <summary>
    /// Contains statistical information about a user's clan.
    /// </summary>
    [DataContract]
    public class ClanProfile
    {
        #region fields
        [DataMember(Name = "RaceRecords")]
        private WarcraftRaceRecord[] m_raceStats;
        [DataMember(Name = "LadderRecords")]
        private WarcraftClanLadderRecord[] m_ladderRecords;
        [DataMember(Name = "ClanName")]
        private string m_clanName;
        [DataMember(Name = "Joined")]
        private DateTime m_joined;
        [DataMember(Name = "Rank")]
        private ClanRank m_rank;
        #endregion

        #region constructor
        /// <summary>
        /// Creates a new <see>ClanProfile</see>.
        /// </summary>
        /// <param name="clanName">The name of the user's clan.</param>
        /// <param name="rank">The rank of the user within the clan.</param>
        /// <param name="joinDate">The date at which the user joined the clan.</param>
        protected internal ClanProfile(string clanName, ClanRank rank, DateTime joinDate)
        {
            if (string.IsNullOrEmpty(clanName))
                throw new ArgumentNullException("clanName");
            if (!Enum.IsDefined(typeof(ClanRank), rank))
                throw new InvalidEnumArgumentException("rank", (int)rank, typeof(ClanRank));

            m_clanName = clanName;
            m_rank = rank;
            m_joined = joinDate;
        }
        #endregion

        #region properties
        /// <summary>
        /// Gets the name of the clan to which the user belongs.
        /// </summary>
        public string ClanName
        {
            get { return m_clanName; }
        }

        /// <summary>
        /// Gets the user's rank within the clan.
        /// </summary>
        public ClanRank Rank
        {
            get { return m_rank; }
        }

        /// <summary>
        /// Gets the date at which the user joined the clan.
        /// </summary>
        public DateTime Joined
        {
            get { return m_joined; }
        }

        /// <summary>
        /// Retrieves a read-only collection of ladder records for the user's clan.
        /// </summary>
        public IEnumerable<WarcraftClanLadderRecord> LadderRecords
        {
            get { return new ReadOnlyCollection<WarcraftClanLadderRecord>(m_ladderRecords); }
        }

        /// <summary>
        /// Retrieves a read-only collection of race statistics for the user's clan.
        /// </summary>
        public IEnumerable<WarcraftRaceRecord> RaceRecords
        {
            get { return new ReadOnlyCollection<WarcraftRaceRecord>(m_raceStats); }
        }
        #endregion

        #region methods
        /// <summary>
        /// In derived classes, sets the statistical data associated with the clan's profile.
        /// </summary>
        /// <param name="ladderRecords">The user's ladder game records.</param>
        /// <param name="raceRecords">The user's records by playable race.</param>
        /// <exception cref="ArgumentNullException">Thrown if any parameters are <see langword="null" />.</exception>
        protected internal void SetStats(WarcraftClanLadderRecord[] ladderRecords, WarcraftRaceRecord[] raceRecords)
        {
            if (object.ReferenceEquals(null, ladderRecords))
                throw new ArgumentNullException("ladderRecords");
            if (object.ReferenceEquals(null, raceRecords))
                throw new ArgumentNullException("raceRecords");

            m_ladderRecords = ladderRecords;
            m_raceStats = raceRecords;
        }
        #endregion
    }
}
