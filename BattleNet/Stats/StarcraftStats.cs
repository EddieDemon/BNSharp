using System;
using System.Collections.Generic;
using System.Text;
using BNSharp.MBNCSUtil;
using System.Runtime.Serialization;

namespace BNSharp.BattleNet.Stats
{
    /// <summary>
    /// Represents statistics logged onto Battle.net with Starcraft, Japan Starcraft, Warcraft II: Battle.net Edition, 
    /// or the original Diablo.
    /// </summary>
    /// <remarks>
    /// <para>This class cannot be instantiated directly.  To obtain an instance of this class, use 
    /// <see cref="UserStats.Parse">UserStats.Parse</see>, and cast the result to this class.</para>
    /// </remarks>
    [DataContract]
    public class StarcraftStats : UserStats
    {
        #region fields
        [DataMember(Name = "Product")]
        private Product m_prod;
        [DataMember(Name = "LiteralText")]
        private string m_literal;

        [DataMember(Name = "LadderRating")]
        private int m_ladderRating;
        [DataMember(Name = "LadderRank")]
        private int m_ladderRank;
        [DataMember(Name = "Wins")]
        private int m_wins;
        [DataMember(Name = "HighestLadderRating")]
        private int m_highestLadderRating;
        [DataMember(Name = "IconCode")]
        private string m_iconCode;
        [DataMember(Name = "IsSpawned")]
        private bool m_isSpawned;
        #endregion

        #region Constructor
        internal StarcraftStats(byte[] stats)
        {
            m_literal = Encoding.ASCII.GetString(stats);

            // RATS 0 0 200 0 0 0 0 0 RATS
            // pcode rating rank wins spawn unknown hirating unkn unkn icon
            DataReader dr = new DataReader(stats);
            string productCode = dr.ReadDwordString(0);
            m_prod = Product.GetByProductCode(productCode);
            if (m_prod == null)
                m_prod = Product.UnknownProduct;

            if (stats.Length > 4)
            {
                try
                {
                    dr.ReadTerminatedString(' ', Encoding.ASCII);
                    string sRating = dr.ReadTerminatedString(' ', Encoding.ASCII);
                    int.TryParse(sRating, out m_ladderRating);
                    string sRank = dr.ReadTerminatedString(' ', Encoding.ASCII);
                    int.TryParse(sRank, out m_ladderRank);
                    string sWins = dr.ReadTerminatedString(' ', Encoding.ASCII);
                    int.TryParse(sWins, out m_wins);
                    int nSpawn;
                    string sSpawn = dr.ReadTerminatedString(' ', Encoding.ASCII);
                    int.TryParse(sSpawn, out nSpawn);
                    m_isSpawned = (nSpawn == 1);
                    dr.ReadTerminatedString(' ', Encoding.ASCII);
                    string sHighRating = dr.ReadTerminatedString(' ', Encoding.ASCII);
                    int.TryParse(sHighRating, out m_highestLadderRating);
                    dr.ReadTerminatedString(' ', Encoding.ASCII);
                    if (dr.Length > dr.Position)
                    {
                        dr.ReadTerminatedString(' ', Encoding.ASCII);
                        m_iconCode = dr.ReadDwordString(0);
                    }
                    else
                    {
                        m_iconCode = productCode;
                    }
                }
                catch (ArgumentOutOfRangeException) { }
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets whether the user is logged on with a spawned client.
        /// </summary>
        public bool IsSpawn
        {
            get { return m_isSpawned; }
        }

        /// <summary>
        /// Gets the icon code for the user issued by Battle.net.
        /// </summary>
        public string IconCode
        {
            get { return m_iconCode; }
        }

        /// <summary>
        /// Gets the user's highest ladder rating.
        /// </summary>
        public int HighestLadderRating
        {
            get { return m_highestLadderRating; }
        }

        /// <summary>
        /// Gets the user's win count.
        /// </summary>
        public int Wins
        {
            get { return m_wins; }
        }

        /// <summary>
        /// Gets the user's current ladder rank.
        /// </summary>
        public int LadderRank
        {
            get { return m_ladderRank; }
        }

        /// <summary>
        /// Gets the user's current ladder rating.
        /// </summary>
        public int LadderRating
        {
            get { return m_ladderRating; }
        }

        /// <summary>
        /// Gets the <see cref="BNSharp.BattleNet.Product">Product</see> with which the user is logged on.
        /// </summary>
        public override Product Product
        {
            get { return m_prod; }
        }

        /// <summary>
        /// Gets the literal text of the user's statstring.
        /// </summary>
        public override string LiteralText
        {
            get { return m_literal; }
        }
        #endregion
    }
}
