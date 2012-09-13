using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Diagnostics;

namespace BNSharp.BattleNet.Clans
{
    /// <summary>
    /// Specifies information provided to the client when being invited to join an existing clan.
    /// </summary>
    [DataContract]
    public class ClanInvitationEventArgs : BaseEventArgs
    {
        [DataMember(Name = "RequestID")]
        private int m_requestID;
        [DataMember(Name = "ClanTag")]
        private string m_tag;
        [DataMember(Name = "ClanName")]
        private string m_clanName;
        [DataMember(Name = "Inviter")]
        private string m_inviter;

        /// <summary>
        /// Creates a new instance of <see>ClanInvitationEventArgs</see>.
        /// </summary>
        /// <param name="requestID">The unique request ID specified by Battle.net.</param>
        /// <param name="tag">The clan tag.</param>
        /// <param name="clanName">The clan name.</param>
        /// <param name="inviter">The name of the user inviting the client.</param>
        public ClanInvitationEventArgs(int requestID, string tag, string clanName, string inviter)
        {
            Debug.Assert(!string.IsNullOrEmpty(tag));
            Debug.Assert(!string.IsNullOrEmpty(clanName));
            Debug.Assert(!string.IsNullOrEmpty(inviter));

            m_requestID = requestID;

            m_tag = tag;
            m_clanName = clanName;
            m_inviter = inviter;
        }

        /// <summary>
        /// Gets the unique request ID associated with the invitation.
        /// </summary>
        public int RequestID
        {
            get { return m_requestID; }
        }

        /// <summary>
        /// Gets the clan tag of the clan to which the client was invited.
        /// </summary>
        public string ClanTag
        {
            get { return m_tag; }
        }

        /// <summary>
        /// Gets the name of the clan to which the client was invited.
        /// </summary>
        public string ClanName
        {
            get { return m_clanName; }
        }

        /// <summary>
        /// Gets the name of the Battle.net user who invited the client to the clan.
        /// </summary>
        public string Inviter
        {
            get { return m_inviter; }
        }
    }

    /// <summary>
    /// Specifies the contract for handlers wishing to listen for clan invitation events.
    /// </summary>
    /// <param name="sender">The object that originated the event.</param>
    /// <param name="e">The event arguments.</param>
    public delegate void ClanInvitationEventHandler(object sender, ClanInvitationEventArgs e);
}
