using System;
using System.Collections.Generic;
using System.Text;
using BNSharp.Plugins;
using System.Diagnostics;
using BNSharp.BattleNet.Clans;
using BNSharp.BattleNet;
using BNSharp.BattleNet.Stats;

namespace BNSharp.BattleNet
{
    partial class BattleNetClient
    {
        private class EventSink : IBattleNetEvents
        {
            private BattleNetClient m_host;
            public EventSink(BattleNetClient host)
            {
                Debug.Assert(host != null);

                m_host = host;
            }

            #region IBattleNetEvents Members


            public void OnAccountCreated(AccountCreationEventArgs e)
            {
                m_host.OnAccountCreated(e);
            }

            public void OnAccountCreationFailed(AccountCreationFailedEventArgs e)
            {
                m_host.OnAccountCreationFailed(e);
            }

            public void OnChannelDidNotExist(ServerChatEventArgs e)
            {
                m_host.OnChannelDidNotExist(e);
            }

            public void OnChannelListReceived(ChannelListEventArgs e)
            {
                m_host.OnChannelListReceived(e);
            }

            public void OnChannelWasFull(ServerChatEventArgs e)
            {
                m_host.OnChannelWasFull(e);
            }

            public void OnChannelWasRestricted(ServerChatEventArgs e)
            {
                m_host.OnChannelWasRestricted(e);
            }

            public void OnClanMemberListReceived(ClanMemberListEventArgs e)
            {
                m_host.OnClanMemberListReceived(e);
            }

            public void OnClanMemberQuit(BNSharp.BattleNet.Clans.ClanMemberStatusEventArgs e)
            {
                m_host.OnClanMemberQuit(e);
            }

            public void OnClanMemberRemoved(BNSharp.BattleNet.Clans.ClanMemberStatusEventArgs e)
            {
                m_host.OnClanMemberRemoved(e);
            }

            public void OnClanMembershipReceived(BNSharp.BattleNet.Clans.ClanMembershipEventArgs e)
            {
                m_host.OnClanMembershipReceived(e);
            }

            public void OnClanMemberStatusChanged(BNSharp.BattleNet.Clans.ClanMemberStatusEventArgs e)
            {
                m_host.OnClanMemberStatusChanged(e);
            }

            public void OnClanMessageOfTheDay(InformationEventArgs e)
            {
                m_host.OnClanMessageOfTheDay(e);
            }

            public void OnClanMemberRankChanged(ClanMemberRankChangeEventArgs e)
            {
                m_host.OnClanMemberRankChanged(e);
            }

            public void OnClanRankChangeResponseReceived(ClanRankChangeEventArgs e)
            {
                m_host.OnClanRankChangeResponseReceived(e);
            }

            public void OnClientCheckFailed(ClientCheckFailedEventArgs e)
            {
                m_host.OnClientCheckFailed(e);
            }

            public void OnClientCheckPassed(BaseEventArgs e)
            {
                m_host.OnClientCheckPassed(e);
            }

            public void OnCommandSent(InformationEventArgs e)
            {
                m_host.OnCommandSent(e);
            }

            public void OnEnteredChat(EnteredChatEventArgs e)
            {
                m_host.OnEnteredChat(e);
            }

            public void OnError(ErrorEventArgs e)
            {
                m_host.OnError(e);
            }

            public void OnInformation(InformationEventArgs e)
            {
                m_host.OnInformation(e);
            }

            public void OnInformationReceived(ServerChatEventArgs e)
            {
                m_host.OnInformationReceived(e);
            }

            public void OnJoinedChannel(ServerChatEventArgs e)
            {
                m_host.OnJoinedChannel(e);
            }

            public void OnLoginFailed(LoginFailedEventArgs e)
            {
                m_host.OnLoginFailed(e);
            }

            public void OnLoginSucceeded(EventArgs e)
            {
                m_host.OnLoginSucceeded(e);
            }

            public void OnMessageSent(ChatMessageEventArgs e)
            {
                m_host.OnMessageSent(e);
            }

            public void OnServerBroadcast(ServerChatEventArgs e)
            {
                m_host.OnServerBroadcast(e);
            }

            public void OnServerErrorReceived(ServerChatEventArgs e)
            {
                m_host.OnServerErrorReceived(e);
            }

            public void OnServerNews(BNSharp.BattleNet.ServerNewsEventArgs e)
            {
                m_host.OnServerNews(e);
            }

            public void OnUserEmoted(ChatMessageEventArgs e)
            {
                m_host.OnUserEmoted(e);
            }

            public void OnUserFlagsChanged(UserEventArgs e)
            {
                m_host.OnUserFlagsChanged(e);
            }

            public void OnUserJoined(UserEventArgs e)
            {
                m_host.OnUserJoined(e);
            }

            public void OnUserLeft(UserEventArgs e)
            {
                m_host.OnUserLeft(e);
            }

            public void OnUserShown(UserEventArgs e)
            {
                m_host.OnUserShown(e);
            }

            public void OnUserSpoke(ChatMessageEventArgs e)
            {
                m_host.OnUserSpoke(e);
            }

            public void OnWhisperReceived(ChatMessageEventArgs e)
            {
                m_host.OnWhisperReceived(e);
            }

            public void OnWhisperSent(ChatMessageEventArgs e)
            {
                m_host.OnWhisperSent(e);
            }

            public void OnUserProfileReceived(UserProfileEventArgs e)
            {
                m_host.OnUserProfileReceived(e);
            }

            public void OnClanCandidatesSearchCompleted(ClanCandidatesSearchEventArgs e)
            {
                m_host.OnClanCandidatesSearchCompleted(e);
            }

            public void OnClanChangeChieftanCompleted(ClanChieftanChangeEventArgs e)
            {
                m_host.OnClanChangeChieftanCompleted(e);
            }

            public void OnClanDisbandCompleted(ClanDisbandEventArgs e)
            {
                m_host.OnClanDisbandCompleted(e);
            }

            public void OnClanFormationCompleted(ClanFormationEventArgs e)
            {
                m_host.OnClanFormationCompleted(e);
            }

            public void OnClanFormationInvitationReceived(ClanFormationInvitationEventArgs e)
            {
                m_host.OnClanFormationInvitationReceived(e);
            }

            public void OnFriendAdded(BNSharp.BattleNet.Friends.FriendAddedEventArgs e)
            {
                m_host.OnFriendAdded(e);
            }

            public void OnFriendListReceived(BNSharp.BattleNet.Friends.FriendListReceivedEventArgs e)
            {
                m_host.OnFriendListReceived(e);
            }

            public void OnFriendMoved(BNSharp.BattleNet.Friends.FriendMovedEventArgs e)
            {
                m_host.OnFriendMoved(e);
            }

            public void OnFriendRemoved(BNSharp.BattleNet.Friends.FriendRemovedEventArgs e)
            {
                m_host.OnFriendRemoved(e);
            }

            public void OnFriendUpdated(BNSharp.BattleNet.Friends.FriendUpdatedEventArgs e)
            {
                m_host.OnFriendUpdated(e);
            }

            public void OnClanInvitationReceived(ClanInvitationEventArgs e)
            {
                m_host.OnClanInvitationReceived(e);
            }

            public void OnClanInvitationResponseReceived(ClanInvitationResponseEventArgs e)
            {
                m_host.OnClanInvitationResponseReceived(e);
            }

            public void OnClanRemovalResponse(ClanRemovalResponseEventArgs e)
            {
                m_host.OnClanRemovalResponse(e);
            }

            public void OnLeftClan(LeftClanEventArgs e)
            {
                m_host.OnLeftClan(e);
            }

            public void OnWarcraftProfileReceived(WarcraftProfileEventArgs e)
            {
                m_host.OnWarcraftProfileReceived(e);
            }

            public void OnProfileLookupFailed(ProfileLookupFailedEventArgs e)
            {
                m_host.OnProfileLookupFailed(e);
            }

            public void OnAdChanged(AdChangedEventArgs e)
            {
                m_host.OnAdChanged(e);
            }

            #endregion
        }
    }
}
