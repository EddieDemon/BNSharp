using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace BNSharp.BattleNet.Clans
{
    /// <summary>
    /// Specifies the status of an attempt to designate a new clan leader.
    /// </summary>
    [DataContract]
    public class ClanChieftanChangeEventArgs : BaseEventArgs
    {
        #region fields
        [DataMember(Name = "Result")]
        private ClanChieftanChangeResult m_result;
        #endregion

        /// <summary>
        /// Creates a new <see>ClanChieftanChangeEventArgs</see>.
        /// </summary>
        /// <param name="result">The result code from Battle.net.</param>
        public ClanChieftanChangeEventArgs(ClanChieftanChangeResult result)
        {
            m_result = result;
        }

        /// <summary>
        /// Gets the result of the change attempt.
        /// </summary>
        /// <remarks>
        /// <para>When exposed under a WCF data contract, this property's backing store is given the name <c>Result</c>.</para>
        /// </remarks>
        public ClanChieftanChangeResult Result
        {
            get { return m_result; }
        }
    }

    /// <summary>
    /// Specifies the contract for handlers wishing to listen for clan chieftan change command events.
    /// </summary>
    /// <param name="sender">The object that originated the event.</param>
    /// <param name="e">The event arguments.</param>
    public delegate void ClanChieftanChangeEventHandler(object sender, ClanChieftanChangeEventArgs e);
}
