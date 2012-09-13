using System;
using System.Collections.Generic;
using System.Text;
using BNSharp.BattleNet.Clans;
using BNSharp.MBNCSUtil;
using System.Diagnostics;
using System.Threading;
using System.ComponentModel;

namespace BNSharp.BattleNet
{
    partial class BattleNetClient
    {
        private Dictionary<string, ClanMember> m_clanList = new Dictionary<string, ClanMember>(StringComparer.OrdinalIgnoreCase);
        private Dictionary<int, string> m_clanRankChangeToMemberList = new Dictionary<int, string>();
        private string m_clanTag;
        private int m_clanCookie;

        partial void ResetClanState()
        {
            m_clanList.Clear();
            m_clanRankChangeToMemberList.Clear();
            m_clanTag = null;
            m_clanCookie = 0;
        }

        /// <summary>
        /// If the client is logged on as a clan Chieftan or Shaman, sets the clan message-of-the-day.
        /// </summary>
        /// <param name="motd">The new message-of-the-day.</param>
        public void SetClanMessageOfTheDay(string motd)
        {
            BncsPacket pck = new BncsPacket((byte)BncsPacketId.ClanSetMOTD);
            pck.InsertInt32(Interlocked.Increment(ref m_clanCookie));
            pck.InsertCString(motd);

            Send(pck);
        }

        private void HandleClanInfo(ParseData pd)
        {
            DataReader dr = new DataReader(pd.Data);
            dr.Seek(1);
            string clanTag = dr.ReadDwordString(0);
            ClanRank rank = (ClanRank)dr.ReadByte();

            ClanMembershipEventArgs args = new ClanMembershipEventArgs(clanTag, rank);
            args.EventData = pd;
            OnClanMembershipReceived(args);

            BncsPacket pck = new BncsPacket((byte)BncsPacketId.ClanMemberList);
            pck.InsertInt32(Interlocked.Increment(ref m_clanCookie));
            Send(pck);
        }

        /// <summary>
        /// Begins searching for clan candidates in the channel and friends list, and checks the availability of the specified clan tag.
        /// </summary>
        /// <param name="clanTag">The clan tag to check for availability.</param>
        /// <returns>The request ID assigned to the request.</returns>
        /// <remarks>
        /// <para>This method will return immediately, but will cause the <see>ClanCandidatesSearchCompleted</see> event to be fired.  That event does not
        /// specifically indicate that the proper number of candidates were found, simply that Battle.net responded.  The event arguments sent
        /// as part of the event indicate the success or failure of the request.</para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="clanTag"/> is <see langword="null" />.</exception>
        public int BeginClanCandidatesSearch(string clanTag)
        {
            if (object.ReferenceEquals(clanTag, null))
                throw new ArgumentNullException(Strings.param_clanTag);

            int result = Interlocked.Increment(ref m_clanCookie);

            BncsPacket pck = new BncsPacket((byte)BncsPacketId.ClanFindCandidates);
            pck.InsertInt32(result);
            pck.InsertDwordString(clanTag, 0);

            Send(pck);

            return result;
        }

        /// <summary>
        /// Invites the specified number of users to form a new clan.
        /// </summary>
        /// <param name="clanName">The name of the clan to form.</param>
        /// <param name="clanTag">The tag of the clan to form.</param>
        /// <param name="usersToInvite">The list of users to invite.  This parameter must be exactly 9 items long.</param>
        /// <returns>The request ID assigned to this request.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="clanName"/>, <paramref name="clanTag"/>, 
        /// <paramref name="usersToInvite"/>, or any of the strings in the array of <paramref name="usersToInvite"/>
        /// is <see langword="null" />.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="usersToInvite"/> is not exactly 9 items long.</exception>
        public int InviteUsersToNewClan(string clanName, string clanTag, string[] usersToInvite)
        {
            if (object.ReferenceEquals(null, clanName)) throw new ArgumentNullException(Strings.param_clanName);
            if (object.ReferenceEquals(null, clanTag)) throw new ArgumentNullException(Strings.param_clanTag);
            if (object.ReferenceEquals(null, usersToInvite)) throw new ArgumentNullException(Strings.param_usersToInvite);
            if (usersToInvite.Length != 9) throw new ArgumentOutOfRangeException(Strings.param_usersToInvite, usersToInvite, Strings.BnetClient_InviteUsersToNewClan_WrongUserCount);
            for (int i = 0; i < 9; i++)
                if (object.ReferenceEquals(usersToInvite[i], null)) throw new ArgumentNullException(Strings.param_usersToInvite, Strings.BnetClient_InviteUsersToNewClan_NullUser);

            int result = Interlocked.Increment(ref m_clanCookie);

            BncsPacket pck = new BncsPacket((byte)BncsPacketId.ClanInviteMultiple);
            pck.InsertInt32(result);
            pck.InsertCString(clanName);
            pck.InsertDwordString(clanTag, 0);
            pck.InsertByte(9);
            for (int i = 0; i < 9; i++)
            {
                pck.InsertCString(usersToInvite[i]);
            }

            Send(pck);

            return result;
        }

        // 0x71
        private void HandleClanInviteMultiple(ParseData pd)
        {
            DataReader dr = new DataReader(pd.Data);
            dr.Seek(4); // cookie
            ClanResponseCode response = (ClanResponseCode)dr.ReadByte();
            ClanFormationEventArgs args = null;
            if (response == ClanResponseCode.Success)
            {
                args = new ClanFormationEventArgs();
            }
            else
            {
                List<string> names = new List<string>();
                int nextByte = dr.Peek();
                while (nextByte > 0)
                {
                    names.Add(dr.ReadCString());
                    nextByte = dr.Peek();
                }
                args = new ClanFormationEventArgs(response == ClanResponseCode.InvitationDeclined, response == ClanResponseCode.Decline, names.ToArray());
            }
            args.EventData = pd;

            OnClanFormationCompleted(args);
        }

        // 0x72
        private void HandleClanCreationInvitation(ParseData pd)
        {
            DataReader dr = new DataReader(pd.Data);
            int cookie = dr.ReadInt32();
            string tag = dr.ReadDwordString(0);
            string name = dr.ReadCString();
            string inviter = dr.ReadCString();
            int inviteeCount = dr.ReadByte();
            string[] invitees = new string[inviteeCount];
            for (int i = 0; i < inviteeCount; i++)
            {
                invitees[i] = dr.ReadCString();
            }

            ClanFormationInvitationEventArgs args = new ClanFormationInvitationEventArgs(cookie, tag, name, inviter, invitees) { EventData = pd };
            OnClanFormationInvitationReceived(args);
        }

        /// <summary>
        /// Responds to the invitation to form a new clan.
        /// </summary>
        /// <param name="requestID">The request ID, provided by the <see cref="ClanFormationInvitationEventArgs.RequestID">ClanFormationInvitationEventArgs</see>.</param>
        /// <param name="clanTag">The clan tag.</param>
        /// <param name="inviter">The user who invited the client to the clan.</param>
        /// <param name="accept">Whether to accept the invitation.</param>
        public void RespondToNewClanInvitation(int requestID, string clanTag, string inviter, bool accept)
        {
            BncsPacket pck = new BncsPacket((byte)BncsPacketId.ClanCreationInvitation);
            pck.InsertInt32(requestID);
            pck.InsertDwordString(clanTag, 0);
            pck.InsertCString(inviter);
            pck.InsertByte((byte)(accept ? ClanResponseCode.Accept : ClanResponseCode.Decline));

            Send(pck);
        }

        /// <summary>
        /// Disbands the clan to which the client belongs.
        /// </summary>
        /// <returns>The request ID assigned to the request.</returns>
        /// <remarks>
        /// <para>The client must be the leader of the clan in order to send this command.</para>
        /// </remarks>
        public int DisbandClan()
        {
            int result = Interlocked.Increment(ref m_clanCookie);

            BncsPacket pck = new BncsPacket((byte)BncsPacketId.ClanDisband);
            pck.InsertInt32(result);
            Send(pck);
            return result;
        }

        // 0x73
        private void HandleDisbandClan(ParseData pd)
        {
            DataReader dr = new DataReader(pd.Data);
            dr.Seek(4);
            ClanResponseCode status = (ClanResponseCode)dr.ReadByte();
            ClanDisbandEventArgs args = new ClanDisbandEventArgs(status == ClanResponseCode.Success);
            args.EventData = pd;
            OnClanDisbandCompleted(args);
        }

        /// <summary>
        /// Designates a user as a new clan chieftan (leader).
        /// </summary>
        /// <returns>The unique request ID assigned to the request.</returns>
        /// <param name="newChieftanName">The name of the new clan chieftan.</param>
        public int DesignateClanChieftan(string newChieftanName)
        {
            if (object.ReferenceEquals(null, newChieftanName))
                throw new ArgumentNullException(Strings.param_newChieftanName);

            int result = Interlocked.Increment(ref m_clanCookie);

            BncsPacket pck = new BncsPacket((byte)BncsPacketId.ClanMakeChieftan);
            pck.InsertInt32(result);
            pck.InsertCString(newChieftanName);

            Send(pck);

            return result;
        }

        private void HandleClanMakeChieftan(ParseData pd)
        {
            DataReader dr = new DataReader(pd.Data);
            dr.Seek(4);
            ClanChieftanChangeResult result = (ClanChieftanChangeResult)dr.ReadByte();
            ClanChieftanChangeEventArgs args = new ClanChieftanChangeEventArgs(result);
            args.EventData = pd;
            OnClanChangeChieftanCompleted(args);
        }

        private void HandleClanQuitNotify(ParseData pd)
        {
            bool removed = (pd.Data[0] == 1);
            LeftClanEventArgs args = new LeftClanEventArgs(removed) { EventData = pd };
            OnLeftClan(args);
        }

        /// <summary>
        /// Begins the process of inviting a user to join a clan.
        /// </summary>
        /// <param name="userToInvite">The name of the user to invite.</param>
        /// <returns>A unique request identifier.</returns>
        public int InviteUserToClan(string userToInvite)
        {
            if (string.IsNullOrEmpty(userToInvite))
                throw new ArgumentNullException(Strings.param_userToInvite, Strings.BnetClient_InviteUserToClan_NullUser);

            int result = Interlocked.Increment(ref m_clanCookie);

            BncsPacket pck = new BncsPacket((byte)BncsPacketId.ClanInvitation);
            pck.InsertInt32(result);
            pck.InsertCString(userToInvite);

            return result;
        }

        private void HandleClanInvitation(ParseData pd)
        {
            DataReader dr = new DataReader(pd.Data);
            int cookie = dr.ReadInt32();
            ClanInvitationResponse response = (ClanInvitationResponse)dr.ReadByte();

            ClanInvitationResponseEventArgs args = new ClanInvitationResponseEventArgs(cookie, response) { EventData = pd };
            OnClanInvitationResponseReceived(args);
        }

        /// <summary>
        /// Begins the process for removing a member from the clan.
        /// </summary>
        /// <param name="memberToRemove">The name of the clan member to remove.</param>
        /// <returns>The request ID assigned to this request.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="memberToRemove"/> is <see langword="null" /> or zero-length.</exception>
        public int RemoveClanMember(string memberToRemove)
        {
            if (string.IsNullOrEmpty(memberToRemove))
                throw new ArgumentNullException(Strings.param_memberToRemove);

            int result = Interlocked.Increment(ref m_clanCookie);

            BncsPacket pck = new BncsPacket((byte)BncsPacketId.ClanRemoveMember);
            pck.InsertInt32(result);
            pck.InsertCString(memberToRemove);

            Send(pck);

            return result;
        }

        private void HandleClanRemoveMember(ParseData pd)
        {
            DataReader dr = new DataReader(pd.Data);
            int reqID = dr.ReadInt32();
            ClanMemberRemovalResponse response = (ClanMemberRemovalResponse)dr.ReadByte();

            ClanRemovalResponseEventArgs args = new ClanRemovalResponseEventArgs(reqID, response) { EventData = pd };
            OnClanRemovalResponse(args);
        }

        private void HandleClanInvitationResponse(ParseData pd)
        {
            DataReader dr = new DataReader(pd.Data);
            int cookie = dr.ReadInt32();
            string tag = dr.ReadDwordString(0);
            string name = dr.ReadCString();
            string inviter = dr.ReadCString();

            ClanInvitationEventArgs args = new ClanInvitationEventArgs(cookie, tag, name, inviter) { EventData = pd };
            OnClanInvitationReceived(args);
        }

        /// <summary>
        /// Responds to a clan invitation received via the <see>ClanInvitationReceived</see> event.
        /// </summary>
        /// <param name="invitation">The arguments that accompanied the invitation.</param>
        /// <param name="accept"><see langword="true" /> to accept the invitation and join the clan; otherwise <see langword="false" />.</param>
        /// <remarks>
        /// <para>Following the acceptance of an invitation, the client should receive <see>ClanMembershipReceived</see> and automatically respond by requesting clan 
        /// membership information.</para>
        /// </remarks>
        public void RespondToClanInvitation(ClanInvitationEventArgs invitation, bool accept)
        {
            BncsPacket pck = new BncsPacket((byte)BncsPacketId.ClanInvitationResponse);
            pck.InsertInt32(invitation.RequestID);
            pck.InsertDwordString(invitation.ClanTag);
            pck.InsertCString(invitation.Inviter);
            pck.InsertByte(accept ? (byte)6 : (byte)4);

            Send(pck);
        }

        private void HandleClanFindCandidates(ParseData pd)
        {
            DataReader dr = new DataReader(pd.Data);
            dr.Seek(4); // skip the cookie
            ClanCandidatesSearchStatus status = (ClanCandidatesSearchStatus)dr.ReadByte();
            int numCandidates = dr.ReadByte();
            string[] usernames = new string[numCandidates];
            for (int i = 0; i < numCandidates; i++)
            {
                usernames[i] = dr.ReadCString();
            }

            ClanCandidatesSearchEventArgs args = new ClanCandidatesSearchEventArgs(status, usernames);
            args.EventData = pd;
            OnClanCandidatesSearchCompleted(args);
        }

        /// <summary>
        /// Attempts to change the specified clan member's rank.
        /// </summary>
        /// <remarks>
        /// <para>This method does not attempt to verify that the current user is allowed to change the specified user's rank, or even if the specified
        /// user exists or is in the current user's clan.  The results of this method call are returned via the 
        /// <see>ClanRankChangeResponseReceived</see> event.</para>
        /// </remarks>
        /// <param name="name">The name of the user to change.</param>
        /// <param name="newRank">The user's new rank.</param>
        /// <exception cref="InvalidEnumArgumentException">Thrown if <paramref name="newRank"/> is not a valid value of the <see>ClanRank</see>
        /// enumeration</exception>.
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="name"/> is <see langword="null" /> or zero-length.</exception>
        public void ChangeClanMemberRank(string name, ClanRank newRank)
        {
            if (!Enum.IsDefined(typeof(ClanRank), newRank))
                throw new InvalidEnumArgumentException(Strings.param_newRank, (int)newRank, typeof(ClanRank));
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(Strings.param_name);

            BncsPacket pck = new BncsPacket((byte)BncsPacketId.ClanRankChange);
            int cookie = Interlocked.Increment(ref m_clanCookie);
            m_clanRankChangeToMemberList.Add(cookie, name);
            pck.InsertInt32(cookie);
            pck.InsertCString(name);
            pck.InsertByte((byte)newRank);

            Send(pck);
        }

        private void HandleClanRankChange(ParseData pd)
        {
            DataReader dr = new DataReader(pd.Data);
            int cookie = dr.ReadInt32();
            string userName = m_clanRankChangeToMemberList[cookie];
            m_clanRankChangeToMemberList.Remove(cookie);
            ClanRankChangeStatus status = (ClanRankChangeStatus)dr.ReadByte();

            ClanRankChangeEventArgs args = new ClanRankChangeEventArgs(userName, status) { EventData = pd };
            OnClanRankChangeResponseReceived(args);
        }

        private void HandleClanMotd(ParseData pd)
        {
            DataReader dr = new DataReader(pd.Data);
            dr.Seek(8);
            string motd = dr.ReadCString();
            InformationEventArgs args = new InformationEventArgs(motd);
            args.EventData = pd;
            OnClanMessageOfTheDay(args);
        }

        private void HandleClanMemberList(ParseData pd)
        {
            DataReader dr = new DataReader(pd.Data);
            dr.Seek(4);
            byte memCount = dr.ReadByte();
            for (int i = 0; i < memCount; i++)
            {
                string userName = dr.ReadCString();
                ClanRank rank = (ClanRank)dr.ReadByte();
                ClanMemberStatus status = (ClanMemberStatus)dr.ReadByte();
                string location = dr.ReadCString();
                m_clanList.Add(userName, new ClanMember(userName, rank, status, location));

            }

            ClanMember[] members = new ClanMember[m_clanList.Count];
            m_clanList.Values.CopyTo(members, 0);

            ClanMemberListEventArgs args = new ClanMemberListEventArgs(members);
            args.EventData = pd;
            OnClanMemberListReceived(args);
        }

        private void HandleClanMemberRemoved(ParseData pd)
        {
            DataReader dr = new DataReader(pd.Data);
            string memberName = dr.ReadCString();
            ClanMember member = m_clanList[memberName];
            m_clanList.Remove(memberName);

            ClanMemberStatusEventArgs args = new ClanMemberStatusEventArgs(member) { EventData = pd };
            OnClanMemberRemoved(args);
        }

        private void HandleClanMemberStatusChanged(ParseData pd)
        {
            DataReader dr = new DataReader(pd.Data);
            string userName = dr.ReadCString();
            if (m_clanList.ContainsKey(userName))
            {
                ClanMember member = m_clanList[userName];
                ClanRank rank = (ClanRank)dr.ReadByte();
                ClanMemberStatus status = (ClanMemberStatus)dr.ReadByte();
                string location = dr.ReadCString();
                member.Rank = rank;
                member.CurrentStatus = status;
                member.Location = location;

                ClanMemberStatusEventArgs args = new ClanMemberStatusEventArgs(member);
                args.EventData = pd;
                OnClanMemberStatusChanged(args);
            }
        }

        private void HandleClanMemberRankChange(ParseData pd)
        {
            DataReader dr = new DataReader(pd.Data);
            ClanRank old = (ClanRank)dr.ReadByte();
            ClanRank newRank = (ClanRank)dr.ReadByte();
            string memberName = dr.ReadCString();
            ClanMember member = null;
            if (m_clanList.ContainsKey(memberName))
                member = m_clanList[memberName];

            ClanMemberRankChangeEventArgs args = new ClanMemberRankChangeEventArgs(old, newRank, member);
            args.EventData = pd;
            OnClanMemberRankChanged(args);
        }
    }
}
