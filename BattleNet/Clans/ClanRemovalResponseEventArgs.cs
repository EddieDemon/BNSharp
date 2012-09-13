using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace BNSharp.BattleNet.Clans
{
    /// <summary>
    /// Contains information about the result of an attempt of the client to remove another member from the clan.
    /// </summary>
    [DataContract]
    public class ClanRemovalResponseEventArgs : BaseEventArgs
    {
        [DataMember(Name = "RequestID")]
        private int m_requestID;
        [DataMember(Name = "Response")]
        private ClanMemberRemovalResponse m_response;

        /// <summary>
        /// Creates a new <see>ClanRemovalResponseEventArgs</see>.
        /// </summary>
        /// <param name="requestID">The request ID assigned to the request.</param>
        /// <param name="response">The response from the server.</param>
        public ClanRemovalResponseEventArgs(int requestID, ClanMemberRemovalResponse response)
        {
            m_requestID = requestID;
            m_response = response;
        }

        /// <summary>
        /// Gets the request ID associated with the request.
        /// </summary>
        public int RequestID
        {
            get { return m_requestID; }
        }

        /// <summary>
        /// Gets the user's or server's response to the request.
        /// </summary>
        public ClanMemberRemovalResponse Response
        {
            get { return m_response; }
        }
    }

    /// <summary>
    /// Specifies the contract for handlers wishing to listen for clan member removal response events.
    /// </summary>
    /// <param name="sender">The object that originated the event.</param>
    /// <param name="e">The event arguments.</param>
    public delegate void ClanRemovalResponseEventHandler(object sender, ClanRemovalResponseEventArgs e);
}
