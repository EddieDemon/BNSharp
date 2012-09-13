using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace BNSharp.BattleNet.Clans
{
    /// <summary>
    /// Sent to the client after the client has invited another user to join an existing clan.
    /// </summary>
    [DataContract]
    public class ClanInvitationResponseEventArgs : BaseEventArgs
    {
        [DataMember(Name = "RequestID")]
        private int m_reqID;
        [DataMember(Name = "Response")]
        private ClanInvitationResponse m_response;

        /// <summary>
        /// Creates a new <see>ClanInvitationResponseEventArgs</see>.
        /// </summary>
        /// <param name="requestID">The ID of the request assigned by the invitation.</param>
        /// <param name="response">The response from Battle.net.</param>
        public ClanInvitationResponseEventArgs(int requestID, ClanInvitationResponse response)
        {
            m_reqID = requestID;
            m_response = response;
        }

        /// <summary>
        /// Gets the ID of the associated invitation request.
        /// </summary>
        public int RequestID
        {
            get { return m_reqID; }
        }

        /// <summary>
        /// Gets the server's response.
        /// </summary>
        public ClanInvitationResponse Response
        {
            get { return m_response; }
        }
    }

    /// <summary>
    /// Specifies the contract for handlers wishing to listen for clan invitation response events.
    /// </summary>
    /// <param name="sender">The object that originated the event.</param>
    /// <param name="e">The event arguments.</param>
    public delegate void ClanInvitationResponseEventHandler(object sender, ClanInvitationResponseEventArgs e);
}
