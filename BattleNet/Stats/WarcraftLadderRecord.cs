using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace BNSharp.BattleNet.Stats
{
    /// <summary>
    /// Contains information about a player's records playing on official Warcraft 3 ladder matches.
    /// </summary>
    [DataContract]
    public class WarcraftLadderRecord
    {
        #region fields
        [DataMember(Name = "LadderType")]
        private WarcraftLadderType m_type;
        [DataMember(Name = "Wins")]
        private int m_wins;
        [DataMember(Name = "Losses")]
        private int m_losses;
        [DataMember(Name = "Level")]
        private int m_level;
        [DataMember(Name = "HoursUntilExperienceDecay")]
        private int m_hoursToXpDecay;
        [DataMember(Name = "TotalExperience")]
        private int m_totalExperience;
        [DataMember(Name = "Rank")]
        private int m_rank;
        #endregion

        #region constructor
        /// <summary>
        /// Creates a new <see>WarcraftLadderRecord</see>.
        /// </summary>
        /// <param name="teamType">The type of ladder represented by this record.</param>
        /// <param name="wins">The player's win count.</param>
        /// <param name="losses">The player's loss count.</param>
        /// <param name="level">The player's level.</param>
        /// <param name="hoursUntilExperienceDecay">The time (in hours) until the player's experience decays without playing.  For more information,
        /// see <see>HoursUntilExperienceDecay</see>.</param>
        /// <param name="totalExperience">The player's total experience value.</param>
        /// <param name="rank">The player's rank.</param>
        public WarcraftLadderRecord(WarcraftLadderType teamType, int wins, int losses,
            int level, int hoursUntilExperienceDecay, int totalExperience, int rank)
        {
            if (!Enum.IsDefined(typeof(WarcraftLadderType), teamType))
                throw new InvalidEnumArgumentException("teamType", (int)teamType, typeof(WarcraftLadderType));
            m_type = teamType;

            if (wins < 0)
                throw new ArgumentOutOfRangeException("wins", wins, "Cannot have a negative win count.");
            m_wins = wins;

            if (losses < 0)
                throw new ArgumentOutOfRangeException("losses", losses, "Cannot have a negative loss count.");
            m_losses = losses;

            if (level < 1)
                throw new ArgumentOutOfRangeException("level", level, "Level must be nonzero and nonnegative.");
            m_level = level;

            m_hoursToXpDecay = hoursUntilExperienceDecay;

            if (totalExperience < 0)
                throw new ArgumentOutOfRangeException("totalExperience", totalExperience, "Experience must be nonnegative.");
            m_totalExperience = totalExperience;

            if (rank < 0)
                throw new ArgumentOutOfRangeException("rank", rank, "Rank must be nonnegative.");
            m_rank = rank;

        }
        #endregion

        #region properties
        /// <summary>
        /// Gets the type of ladder record represented by this team record.
        /// </summary>
        public WarcraftLadderType LadderType
        {
            get { return m_type; }
        }

        /// <summary>
        /// Gets the number of wins played by the team.
        /// </summary>
        public int Wins
        {
            get { return m_wins; }
        }

        /// <summary>
        /// Gets the number of losses played by the team.
        /// </summary>
        public int Losses
        {
            get { return m_losses; }
        }

        /// <summary>
        /// Gets the team's level.
        /// </summary>
        public int Level
        {
            get { return m_level; }
        }

        /// <summary>
        /// Gets the number of hours until the team's experience decays.
        /// </summary>
        /// <remarks>
        /// <para>This value is the only piece that does not have a corresponding representation within the Warcraft III user interface.
        /// It is hypothesized that this is the meaning of the underlying value; however, it is unconfirmed.  This property may be removed
        /// or changed in future versions.</para>
        /// </remarks>
        public int HoursUntilExperienceDecay
        {
            get { return m_hoursToXpDecay; }
        }

        /// <summary>
        /// Gets the team's experience level.
        /// </summary>
        public int TotalExperience
        {
            get { return m_totalExperience; }
        }

        /// <summary>
        /// Gets the team's rank, if the team is ranked.
        /// </summary>
        /// <remarks>
        /// <para>If the team is unranked, this property will return 0.</para>
        /// </remarks>
        public int Rank
        {
            get { return m_rank; }
        }
        #endregion
    }
}
