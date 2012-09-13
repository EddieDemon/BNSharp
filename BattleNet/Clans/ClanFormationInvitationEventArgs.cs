using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace BNSharp.BattleNet.Clans
{
    /// <summary>
    /// Specifies the event arguments provided from Battle.net when the client is invited to join a new clan.
    /// </summary>
    [DataContract]
    public class ClanFormationInvitationEventArgs : BaseEventArgs
    {
        #region fields
        [DataMember(Name = "RequestID")]
        private int m_cookie;
        [DataMember(Name = "Tag")]
        private string m_tag;
        [DataMember(Name = "Name")]
        private string m_clanName;
        [DataMember(Name = "Inviter")]
        private string m_inviterName;
        [DataMember(Name = "InvitedUsers")]
        private string[] m_invitedUsers;
        #endregion

        /// <summary>
        /// Creates a new <see>ClanFormationInvitationEventArgs</see>.
        /// </summary>
        /// <param name="requestNumber">The unique ID of the request.</param>
        /// <param name="tag">The clan tag.</param>
        /// <param name="clanName">The full name of the new clan.</param>
        /// <param name="inviter">The user responsible for the invitation.</param>
        /// <param name="invitees">The users being invited.</param>
        public ClanFormationInvitationEventArgs(int requestNumber, string tag, string clanName, string inviter, string[] invitees)
        {
            Debug.Assert(invitees != null);

            m_cookie = requestNumber;
            m_tag = tag;
            m_clanName = clanName;
            m_inviterName = inviter;
            m_invitedUsers = invitees;
        }

        /// <summary>
        /// Gets the unique ID of the request.
        /// </summary>
        /// <remarks>
        /// <para>This value should be used in the response.</para>
        /// <para>When exposed under a WCF data contract, this property's backing store is given the name <c>RequestID</c>.</para>
        /// </remarks>
        public int RequestID
        {
            get { return m_cookie; }
        }

        /// <summary>
        /// Gets the Tag of the clan being formed.
        /// </summary>
        /// <remarks>
        /// <para>When exposed under a WCF data contract, this property's backing store is given the name <c>Tag</c>.</para>
        /// </remarks>
        public string ClanTag
        {
            get { return m_tag; }
        }

        /// <summary>
        /// Gets the full name of the clan being formed.
        /// </summary>
        /// <remarks>
        /// <para>When exposed under a WCF data contract, this property's backing store is given the name <c>Name</c>.</para>
        /// </remarks>
        public string ClanName
        {
            get { return m_clanName; }
        }

        /// <summary>
        /// Gets the screen name of the user sending the invitation.
        /// </summary>
        /// <remarks>
        /// <para>When exposed under a WCF data contract, this property's backing store is given the name <c>Inviter</c>.</para>
        /// </remarks>
        public string InvitingUser
        {
            get { return m_inviterName; }
        }

        /// <summary>
        /// Gets a copy of the list of users being invited to join.
        /// </summary>
        /// <remarks>
        /// <para>When exposed under a WCF data contract, this property's backing store is given the name <c>InvitedUsers</c>.</para>
        /// </remarks>
        public ReadOnlyCollection<string> InvitedUsers
        {
            get
            {
                return new ReadOnlyCollection<string>(m_invitedUsers);
            }
        }
    }

    /// <summary>
    /// Specifies the contract for handlers wishing to listen for clan formation invitation events.
    /// </summary>
    /// <param name="sender">The object that originated the event.</param>
    /// <param name="e">The event arguments.</param>
    public delegate void ClanFormationInvitationEventHandler(object sender, ClanFormationInvitationEventArgs e);
}
