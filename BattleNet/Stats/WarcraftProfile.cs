using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;

namespace BNSharp.BattleNet.Stats
{
    /// <summary>
    /// Contains information about the user's Warcraft 3 profile, including statistics.
    /// </summary>
    [DataContract]
    public class WarcraftProfile
    {
        #region fields
        [DataMember(Name = "ArrangedTeams")]
        private ArrangedTeamRecord[] m_arrangedTeams;
        [DataMember(Name = "RaceRecords")]
        private WarcraftRaceRecord[] m_raceStats;
        [DataMember(Name = "LadderRecords")]
        private WarcraftLadderRecord[] m_ladderRecords;
        [DataMember(Name = "IconID")]
        private string m_iconID;
        [DataMember(Name = "Description")]
        private string m_desc;
        [DataMember(Name = "Location")]
        private string m_loc;
        [DataMember(Name = "ClanTag")]
        private string m_clanTag;
        #endregion

        #region constructor
        /// <summary>
        /// Creates a new <see>WarcraftProfile</see>.
        /// </summary>
        /// <param name="description">The user's Description field value.</param>
        /// <param name="location">The user's Location field value.</param>
        /// <param name="clanTag">The user's clan tag.</param>
        protected internal WarcraftProfile(string description, string location, string clanTag)
        {
            m_desc = description;
            m_loc = location;
            m_clanTag = clanTag;
        }
        #endregion

        #region properties
        /// <summary>
        /// Gets the user's Description profile field.
        /// </summary>
        public string Description
        {
            get { return m_desc; }
        }

        /// <summary>
        /// Gets the user's Location profile field.
        /// </summary>
        public string Location
        {
            get { return m_loc; }
        }

        /// <summary>
        /// Gets the user's clan tag, if any.
        /// </summary>
        /// <remarks>
        /// <para>If the user is not in a clan, this property will be <see langword="null" />.</para>
        /// </remarks>
        public string ClanTag
        {
            get { return m_clanTag; }
        }

        /// <summary>
        /// Gets (and in derived classes, sets) the selected icon ID of the user.
        /// </summary>
        public string IconID
        {
            get { return m_iconID; }
            protected internal set { m_iconID = value; }
        }

        /// <summary>
        /// Retrieves a read-only collection of arranged team statistics.
        /// </summary>
        public IEnumerable<ArrangedTeamRecord> ArrangedTeams
        {
            get
            {
                return new ReadOnlyCollection<ArrangedTeamRecord>(m_arrangedTeams);
            }
        }

        /// <summary>
        /// Retrieves a read-only collection of race statistics.
        /// </summary>
        public IEnumerable<WarcraftRaceRecord> RaceRecords
        {
            get { return new ReadOnlyCollection<WarcraftRaceRecord>(m_raceStats); }
        }

        /// <summary>
        /// Retrieves a read-only collection of ladder records for the user.
        /// </summary>
        public IEnumerable<WarcraftLadderRecord> LadderRecords
        {
            get { return new ReadOnlyCollection<WarcraftLadderRecord>(m_ladderRecords); }
        }
        #endregion

        #region methods
        /// <summary>
        /// In derived classes, sets the statistical data associated with the user's profile.
        /// </summary>
        /// <param name="ladderRecords">The user's ladder game records.</param>
        /// <param name="arrangedTeams">The user's arranged team game records.</param>
        /// <param name="raceRecords">The user's records by playable race.</param>
        /// <exception cref="ArgumentNullException">Thrown if any parameters are <see langword="null" />.</exception>
        protected internal void SetStats(WarcraftLadderRecord[] ladderRecords, ArrangedTeamRecord[] arrangedTeams,
            WarcraftRaceRecord[] raceRecords)
        {
            if (object.ReferenceEquals(null, ladderRecords))
                throw new ArgumentNullException("ladderRecords");
            if (object.ReferenceEquals(null, arrangedTeams))
                throw new ArgumentNullException("arrangedTeams");
            if (object.ReferenceEquals(null, raceRecords))
                throw new ArgumentNullException("raceRecords");

            m_ladderRecords = ladderRecords;
            m_arrangedTeams = arrangedTeams;
            m_raceStats = raceRecords;
        }
        #endregion
    }
}
