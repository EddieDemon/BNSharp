using System;
using System.Collections.Generic;
using System.Text;
using BNSharp.Net;
using BNSharp.BattleNet.Clans;
using BNSharp.BattleNet;
using BNSharp.BattleNet.Friends;
using BNSharp.BattleNet.Stats;

namespace BNSharp.Plugins
{
    /// <summary>
    /// Provided by the BN# Plugins Infrastructure to enable custom packet handlers to fire events.
    /// </summary>
    /// <remarks>
    /// <para><b>THIS IS PRELIMINARY SUPPORT FOR CUSTOM PACKET HANDLING!</b>  New events will be introduced before this is completed.</para>
    /// </remarks>
    public interface IBattleNetEvents
    {
        /// <summary>
        /// Fires the <see cref="BattleNetClient.AccountCreated">AccountCreated</see> event.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        void OnAccountCreated(AccountCreationEventArgs e);
        /// <summary>
        /// Fires the <see cref="BattleNetClient.AccountCreationFailed">AccountCreationFailed</see> event.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        void OnAccountCreationFailed(AccountCreationFailedEventArgs e);

        /// <summary>
        /// Fires the <see cref="BattleNetClient.ChannelDidNotExist">ChannelDidNotExist</see> event.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        void OnChannelDidNotExist(ServerChatEventArgs e);
        /// <summary>
        /// Fires the <see cref="BattleNetClient.ChannelListReceived">ChannelListReceived</see> event.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        void OnChannelListReceived(ChannelListEventArgs e);
        /// <summary>
        /// Fires the <see cref="BattleNetClient.ChannelWasFull">ChannelWasFull</see> event.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        void OnChannelWasFull(ServerChatEventArgs e);
        /// <summary>
        /// Fires the <see cref="BattleNetClient.ChannelWasRestricted">ChannelWasRestricted</see> event.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        void OnChannelWasRestricted(ServerChatEventArgs e);
        /// <summary>
        /// Fires the <see cref="BattleNetClient.ClanCandidatesSearchCompleted">ClanCandidatesSearchCompleted</see> event.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        void OnClanCandidatesSearchCompleted(ClanCandidatesSearchEventArgs e);
        /// <summary>
        /// Fires the <see cref="BattleNetClient.ClanChangeChieftanCompleted">ClanChangeChieftanCompleted</see> event.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        void OnClanChangeChieftanCompleted(ClanChieftanChangeEventArgs e);
        /// <summary>
        /// Fires the <see cref="BattleNetClient.ClanDisbandCompleted">ClanDisbandCompleted</see> event.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        void OnClanDisbandCompleted(ClanDisbandEventArgs e);
        /// <summary>
        /// Fires the <see cref="BattleNetClient.ClanFormationCompleted">ClanFormationCompleted</see> event.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        void OnClanFormationCompleted(ClanFormationEventArgs e);
        /// <summary>
        /// Fires the <see cref="BattleNetClient.ClanFormationInvitationReceived">ClanFormationInvitationReceived</see> event.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        void OnClanFormationInvitationReceived(ClanFormationInvitationEventArgs e);
        /// <summary>
        /// Fires the <see cref="BattleNetClient.ClanInvitationReceived">ClanInvitationReceived</see> event.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        void OnClanInvitationReceived(ClanInvitationEventArgs e);
        /// <summary>
        /// Fires the <see cref="BattleNetClient.ClanInvitationResponseReceived">ClanInvitationResponseReceived</see> event.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        void OnClanInvitationResponseReceived(ClanInvitationResponseEventArgs e);
        /// <summary>
        /// Fires the <see cref="BattleNetClient.ClanMemberListReceived">ClanMemberListReceived</see> event.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        void OnClanMemberListReceived(ClanMemberListEventArgs e);
        /// <summary>
        /// Fires the <see cref="BattleNetClient.ClanMemberQuit">ClanMemberQuit</see> event.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        void OnClanMemberQuit(ClanMemberStatusEventArgs e);
        /// <summary>
        /// Fires the <see cref="BattleNetClient.ClanMemberRemoved">ClanMemberRemoved</see> event.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        void OnClanMemberRemoved(ClanMemberStatusEventArgs e);
        /// <summary>
        /// Fires the <see cref="BattleNetClient.ClanMembershipReceived">ClanMembershipReceived</see> event.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        void OnClanMembershipReceived(ClanMembershipEventArgs e);
        /// <summary>
        /// Fires the <see cref="BattleNetClient.ClanMemberStatusChanged">ClanMemberStatusChanged</see> event.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        void OnClanMemberStatusChanged(ClanMemberStatusEventArgs e);
        /// <summary>
        /// Fires the <see cref="BattleNetClient.ClanMessageOfTheDay">ClanMessageOfTheDay</see> event.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        void OnClanMessageOfTheDay(InformationEventArgs e);
        /// <summary>
        /// Fires the <see cref="BattleNetClient.ClanMemberRankChanged">ClanMemberRankChanged</see> event.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        void OnClanMemberRankChanged(ClanMemberRankChangeEventArgs e);
        /// <summary>
        /// Fires the <see cref="BattleNetClient.ClanRankChangeResponseReceived">ClanRankChangeResponseReceived</see> event.
        /// </summary>
        /// <param name="e"></param>
        void OnClanRankChangeResponseReceived(ClanRankChangeEventArgs e);
        /// <summary>
        /// Fires the <see cref="BattleNetClient.ClanRemovalResponse">ClanRemovalResponse</see> event.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        void OnClanRemovalResponse(ClanRemovalResponseEventArgs e);
        /// <summary>
        /// Fires the <see cref="BattleNetClient.LeftClan">LeftClan</see> event.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        void OnLeftClan(LeftClanEventArgs e);
        /// <summary>
        /// Fires the <see cref="BattleNetClient.ClientCheckFailed">ClientCheckFailed</see> event.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        void OnClientCheckFailed(ClientCheckFailedEventArgs e);
        /// <summary>
        /// Fires the <see cref="BattleNetClient.ClientCheckPassed">ClientCheckPassed</see> event.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        void OnClientCheckPassed(BaseEventArgs e);
        /// <summary>
        /// Fires the <see cref="BattleNetClient.CommandSent">CommandSent</see> event.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        void OnCommandSent(InformationEventArgs e);
        /// <summary>
        /// Fires the <see cref="BattleNetClient.EnteredChat">EnteredChat</see> event.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        void OnEnteredChat(EnteredChatEventArgs e);
        /// <summary>
        /// Fires the <see cref="BattleNetClient.FriendAdded">FriendAdded</see> event.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        void OnFriendAdded(FriendAddedEventArgs e);
        /// <summary>
        /// Fires the <see cref="BattleNetClient.FriendListReceived">FriendListReceived</see> event.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        void OnFriendListReceived(FriendListReceivedEventArgs e);
        /// <summary>
        /// Fires the <see cref="BattleNetClient.FriendMoved">FriendMoved</see> event.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        void OnFriendMoved(FriendMovedEventArgs e);
        /// <summary>
        /// Fires the <see cref="BattleNetClient.FriendRemoved">FriendRemoved</see> event.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        void OnFriendRemoved(FriendRemovedEventArgs e);
        /// <summary>
        /// Fires the <see cref="BattleNetClient.FriendUpdated">FriendUpdated</see> event.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        void OnFriendUpdated(FriendUpdatedEventArgs e);
        /// <summary>
        /// Fires the <see cref="BattleNetClient.Error">Error</see> event.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        void OnError(ErrorEventArgs e);
        /// <summary>
        /// Fires the <see cref="BattleNetClient.Information">Information</see> event.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        void OnInformation(InformationEventArgs e);
        /// <summary>
        /// Fires the <see cref="BattleNetClient.InformationReceived">InformationReceived</see> event.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        void OnInformationReceived(ServerChatEventArgs e);
        /// <summary>
        /// Fires the <see cref="BattleNetClient.JoinedChannel">JoinedChannel</see> event.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        void OnJoinedChannel(ServerChatEventArgs e);
        /// <summary>
        /// Fires the <see cref="BattleNetClient.LoginFailed">LoginFailed</see> event.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        void OnLoginFailed(LoginFailedEventArgs e);
        /// <summary>
        /// Fires the <see cref="BattleNetClient.LoginSucceeded">LoginSucceeded</see> event.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        void OnLoginSucceeded(EventArgs e);
        /// <summary>
        /// Fires the <see cref="BattleNetClient.MessageSent">MessageSent</see> event.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        void OnMessageSent(ChatMessageEventArgs e);
        /// <summary>
        /// Fires the <see cref="BattleNetClient.ServerBroadcast">ServerBroadcast</see> event.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        void OnServerBroadcast(ServerChatEventArgs e);
        /// <summary>
        /// Fires the <see cref="BattleNetClient.ServerErrorReceived">ServerErrorReceived</see> event.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        void OnServerErrorReceived(ServerChatEventArgs e);
        /// <summary>
        /// Fires the <see cref="BattleNetClient.WhisperSent">WhisperSent</see> event.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        void OnServerNews(ServerNewsEventArgs e);
        /// <summary>
        /// Fires the <see cref="BattleNetClient.UserEmoted">UserEmoted</see> event.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        void OnUserEmoted(ChatMessageEventArgs e);
        /// <summary>
        /// Fires the <see cref="BattleNetClient.UserFlagsChanged">UserFlagsChanged</see> event.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        void OnUserFlagsChanged(UserEventArgs e);
        /// <summary>
        /// Fires the <see cref="BattleNetClient.UserJoined">UserJoined</see> event.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        void OnUserJoined(UserEventArgs e);
        /// <summary>
        /// Fires the <see cref="BattleNetClient.UserLeft">UserLeft</see> event.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        void OnUserLeft(UserEventArgs e);
        /// <summary>
        /// Fires the <see cref="BattleNetClient.UserProfileReceived">UserProfileReceived</see> event.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        void OnUserProfileReceived(UserProfileEventArgs e);
        /// <summary>
        /// Fires the <see cref="BattleNetClient.UserShown">UserShown</see> event.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        void OnUserShown(UserEventArgs e);
        /// <summary>
        /// Fires the <see cref="BattleNetClient.UserSpoke">UserSpoke</see> event.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        void OnUserSpoke(ChatMessageEventArgs e);
        /// <summary>
        /// Fires the <see cref="BattleNetClient.WhisperReceived">WhisperReceived</see> event.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        void OnWhisperReceived(ChatMessageEventArgs e);
        /// <summary>
        /// Fires the <see cref="BattleNetClient.WhisperSent">WhisperSent</see> event.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        void OnWhisperSent(ChatMessageEventArgs e);

        /// <summary>
        /// Fires the <see cref="BattleNetClient.WarcraftProfileReceived">WarcraftProfileReceived</see> event.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        void OnWarcraftProfileReceived(WarcraftProfileEventArgs e);
        /// <summary>
        /// Fires the <see cref="BattleNetClient.ProfileLookupFailed">ProfileLookupFailed</see> event.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        void OnProfileLookupFailed(ProfileLookupFailedEventArgs e);

        /// <summary>
        /// Fires the <see cref="BattleNetClient.AdChanged">AdChanged</see> event.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        void OnAdChanged(AdChangedEventArgs e);
    }
}
