using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;

namespace BNSharp.BattleNet.Clans
{
    /// <summary>
    /// Specifies the result of a search for clan candidates.
    /// </summary>
    [DataContract]
    public class ClanCandidatesSearchEventArgs : BaseEventArgs
    {
        #region fields
        [DataMember(Name = "Status")]
        private ClanCandidatesSearchStatus m_status;
        [DataMember(Name = "CandidateNames")]
        private string[] m_candidateNames;
        #endregion

        #region constructors
        /// <summary>
        /// Creates a new <see>ClanCandidatesSearchEventArgs</see> for a request that was unsuccessful.
        /// </summary>
        /// <param name="status">The status reported by Battle.net.</param>
        public ClanCandidatesSearchEventArgs(ClanCandidatesSearchStatus status)
        {
            m_status = status;
            m_candidateNames = new string[0];
        }

        /// <summary>
        /// Creates a new <see>ClanCandidatesSearchEventArgs</see> for a request that was successful.
        /// </summary>
        /// <param name="status">The status reported by Battle.net.</param>
        /// <param name="candidateNames">The list of candidate names provided by Battle.net.</param>
        public ClanCandidatesSearchEventArgs(ClanCandidatesSearchStatus status, string[] candidateNames)
        {
            m_status = status;
            m_candidateNames = candidateNames;
        }
        #endregion

        /// <summary>
        /// Gets a read-only list of the candidate names returned as a result of the search.
        /// </summary>
        /// <remarks>
        /// <para>When exposed under a WCF data contract, this property's backing store is given the name <c>CandidateNames</c>.</para>
        /// </remarks>
        public ReadOnlyCollection<string> Candidates
        {
            get
            {
                return new ReadOnlyCollection<string>(m_candidateNames);
            }
        }

        /// <summary>
        /// Gets the functional result of the search.
        /// </summary>
        /// <remarks>
        /// <para>When exposed under a WCF data contract, this property's backing store is given the name <c>Status</c>.</para>
        /// </remarks>
        public ClanCandidatesSearchStatus Status
        {
            get { return m_status; }
        }
    }

    /// <summary>
    /// Specifies the contract for event handlers that want to listen to clan candidates search events.
    /// </summary>
    /// <param name="sender">The object that originated the event.</param>
    /// <param name="e">The event arguments.</param>
    public delegate void ClanCandidatesSearchEventHandler(object sender, ClanCandidatesSearchEventArgs e);
}
